
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteShell
{
    class FileServerEntry
    {
        internal FileSystemServer fileServer = null;
        internal System.Net.HttpListener testServer = null;
        internal bool isTestServerRunning = false;
        internal int port = 80;
        internal String host = "*";
        internal String token = null;
        internal bool hasDafaultToken = false;
        internal String workingDirectory = null;
        internal int testPort = new Random().Next(10000, 60000);

        internal System.Windows.Forms.FlowLayoutPanel viewEntry;
        internal System.Windows.Forms.Label hostView;
        internal System.Windows.Forms.Label portView;
        internal System.Windows.Forms.Label serverRunningView;
        internal System.Windows.Forms.Label testServerRunningView;

        internal System.Windows.Forms.FlowLayoutPanel GetEntryView()
        {
            // initalize controlls
            this.viewEntry = new System.Windows.Forms.FlowLayoutPanel();
            this.hostView = new System.Windows.Forms.Label();
            this.portView = new System.Windows.Forms.Label();
            this.serverRunningView = new System.Windows.Forms.Label();
            this.testServerRunningView = new System.Windows.Forms.Label();

            //this.viewEntry.AutoSize = true;
            this.viewEntry.BackColor = System.Drawing.SystemColors.ControlDark;
            // this.viewEntry.Location = new System.Drawing.Point(3, 3);
            this.viewEntry.Name = "";
            this.viewEntry.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.viewEntry.Size = new System.Drawing.Size(632, 27);
            this.viewEntry.TabIndex = 0;

            this.hostView.AutoSize = true;
            // this.hostView.Location = new System.Drawing.Point(3, 5);
            this.hostView.Name = "host";
            this.hostView.Size = new System.Drawing.Size(56, 16);
            this.hostView.TabIndex = 1;
            this.hostView.Text = this.host;

            this.portView.AutoSize = true;
            // this.portView.Location = new System.Drawing.Point(70, 5);
            this.portView.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.portView.Name = "port";
            this.portView.Size = new System.Drawing.Size(56, 16);
            this.portView.TabIndex = 2;
            this.portView.Text = this.port.ToString();

            this.serverRunningView.AutoSize = true;
            this.serverRunningView.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.serverRunningView.Name = "isRunning";
            this.serverRunningView.Size = new System.Drawing.Size(56, 16);
            this.serverRunningView.TabIndex = 3;
            this.serverRunningView.Text = "stopped";

            this.testServerRunningView.AutoSize = true;
            this.testServerRunningView.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.testServerRunningView.Name = "isTesting";
            this.testServerRunningView.Size = new System.Drawing.Size(56, 16);
            this.testServerRunningView.TabIndex = 4;
            this.testServerRunningView.Text = "";

            this.viewEntry.Controls.Add(this.hostView);
            this.viewEntry.Controls.Add(this.portView);
            this.viewEntry.Controls.Add(this.serverRunningView);
            this.viewEntry.Controls.Add(this.testServerRunningView);

            return this.viewEntry;
        }

        internal void BlurViewEntry()
        {
            this.viewEntry.BackColor = System.Drawing.SystemColors.ControlDark;
        }
        internal void FocusViewEntry()
        {
            this.viewEntry.BackColor = System.Drawing.SystemColors.ScrollBar;
        }

        internal void ConfigRunning(bool b)
        {
            this.serverRunningView.Text = b ? "running" : "stopped";
        }

        internal void ConfigTesting(bool b)
        {
            this.testServerRunningView.Text = b ? "testing" : "";
        }
    }
}
