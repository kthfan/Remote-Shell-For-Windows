using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteShell
{
    partial class Form1
    {
        private int ServerListCursor = -1;
        private List<FileServerEntry> ServerList = new List<FileServerEntry>();

        private FileServerEntry AddServerEntry(String directory, String host, int port, String token)
        {
            FileServerEntry entry = new FileServerEntry();
            entry.workingDirectory = directory;
            entry.host = host;
            entry.port = port;
            entry.token = token == "" ? null : token;
            if (entry.token != null) entry.hasDafaultToken = true;

            ServerList.Add(entry);

            return entry;
        }

        private bool RunServer(FileServerEntry fileServerEntry)
        {
            if (fileServerEntry.fileServer != null) return false;


            List<String> hostList = new List<String>() { "localhost:" + fileServerEntry.testPort, fileServerEntry.host };
            List<int> portList = new List<int>() { fileServerEntry.port };
            fileServerEntry.fileServer = new FileSystemServer(fileServerEntry.workingDirectory, fileServerEntry.token, hostList, portList);



            fileServerEntry.token = fileServerEntry.fileServer.Token;

            ThreadPool.QueueUserWorkItem((Object stateInfo) => {
                fileServerEntry.fileServer.Start();
            });
            return true;
        }

        private void _TestListenerCallback(IAsyncResult result)
        {
            FileServerEntry fileServerEntry = (FileServerEntry)result.AsyncState;
            HttpListener listener = fileServerEntry.testServer;
            // close listener if server is closed
            if (listener == null) return;
            if (!listener.IsListening)
            {
                listener.Abort();
                listener.Close();
                return;
            }
            try
            {
                HttpListenerContext httpContext = listener.EndGetContext(result);
                HttpListenerRequest request = httpContext.Request;
                HttpListenerResponse response = httpContext.Response;
                response.Headers.Add("Access-Control-Allow-Origin", "http://localhost");
                byte[] bb = Encoding.UTF8.GetBytes(this.ReadIndexHtml(fileServerEntry));
                response.OutputStream.Write(bb, 0, bb.Length);

                request.InputStream.Close();
                response.OutputStream.Close();
            }
            catch (HttpListenerException e)
            {
                Console.WriteLine("Http server might have been closed.");
            }
        }
        private bool RunTestServer(FileServerEntry fileServerEntry)
        {
            if (fileServerEntry.testServer != null) return false;
            ThreadPool.QueueUserWorkItem((Object stateInfo) => {
                fileServerEntry.testServer = new HttpListener();
                fileServerEntry.testServer.Prefixes.Add("http://localhost:" + fileServerEntry.testPort + "/");
                fileServerEntry.isTestServerRunning = true;
                fileServerEntry.testServer.Start();
                while (fileServerEntry.isTestServerRunning)
                {
                    fileServerEntry.testServer.BeginGetContext(new AsyncCallback(this._TestListenerCallback), fileServerEntry).AsyncWaitHandle.WaitOne(2000, true);
                }
                fileServerEntry.testServer.Close();
                fileServerEntry.testServer = null;
            });
           
            return true;
        }

        private bool StopServer(FileServerEntry fileServerEntry)
        {
            if (fileServerEntry.fileServer != null)
            {
                fileServerEntry.fileServer.Close();
                fileServerEntry.fileServer = null;
                if (!fileServerEntry.hasDafaultToken) fileServerEntry.token = "";
                return true;
            }
            return false;
        }

        private bool StopTestServer(FileServerEntry fileServerEntry)
        {
            if (fileServerEntry.testServer != null)
            {
                fileServerEntry.isTestServerRunning = false;
                return true;
            }
            return false;
        }


        private String InjectPort(int port){
            return $"<script>window.portToConnect=\"{port}\"</script>";
        }
        private String InjectToken(String token){ 
            return $"<script>document.querySelector(\"input\").value = \"{token}\"; setTimeout(()=>document.querySelector(\"button\").click(), 1000);</script>";
        }
        private String ReadIndexHtml(FileServerEntry fileServerEntry)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream("RemoteShell.raw.index.html");
            StreamReader reader = new StreamReader(stream);
            String str = reader.ReadToEnd() + this.InjectToken(fileServerEntry.token) + this.InjectPort(fileServerEntry.port);

            return str;
        }
    }
}
