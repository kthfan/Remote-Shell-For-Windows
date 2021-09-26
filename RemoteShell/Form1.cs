using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteShell
{
    public partial class Form1 : Form
    {


        


        private int CurrentPanelIndex = 0;

        public Form1()
        {
            InitializeComponent();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(this.folderBrowserDialog1.SelectedPath))
            {
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        

        private void button7_Click(object sender, EventArgs e)
        {
            if (this.ServerListCursor != -1)
            {
                if (this.StopServer(this.ServerList[this.ServerListCursor]) == false)
                {
                    MessageBox.Show("Server is already closed.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.ServerList[this.ServerListCursor].ConfigRunning(false);
                   this.ServerList[this.ServerListCursor].ConfigTesting(false);
               }
                this.StopTestServer(this.ServerList[this.ServerListCursor]);
            }
            else
            {
                MessageBox.Show("No server have created.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.SwitchAddServerEntry();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.SwitchMainPage();

            FileServerEntry entry =  this.AddServerEntry(this.CurrentWorkingDirectory, this.CurrentHost, this.CurrentPort, this.CurrentToken);
            System.Windows.Forms.FlowLayoutPanel viewEntry = entry.GetEntryView();

            // bind click event to switch entries
            System.EventHandler func = new System.EventHandler((object sender1, EventArgs e1) => {
                if (this.ServerListCursor != -1)
                {
                    this.ServerList[this.ServerListCursor].BlurViewEntry();
                }
                entry.FocusViewEntry();
                this.ServerListCursor = this.ServerList.IndexOf(entry);
            });
            viewEntry.Click += func;
            entry.hostView.Click += func;
            entry.portView.Click += func;
            entry.serverRunningView.Click += func;
            entry.testServerRunningView.Click += func;


            this.flowLayoutPanel1.Controls.Add(viewEntry);

            // if not entry have been created before, then focus the created entry
            if (this.ServerList.Count() == 1)
            {
                entry.FocusViewEntry();
                this.ServerListCursor = this.ServerList.IndexOf(entry);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.SwitchMainPage();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            this.SwitchDisplayServerEntry();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.ServerListCursor != -1)
            {
                // stop server
                FileServerEntry serverEntry = this.ServerList[this.ServerListCursor];
                this.StopServer(serverEntry);
                this.StopTestServer(serverEntry);

                // remove
                this.flowLayoutPanel1.Controls.Remove(serverEntry.viewEntry);
                this.ServerList.Remove(serverEntry);
                if (this.ServerList.Count() == 0) this.ServerListCursor = -1;
                else if (this.ServerListCursor > 0) this.ServerListCursor--;

                // focus next entry
                if (this.ServerList.Count() != 0) this.ServerList[this.ServerListCursor].FocusViewEntry();
            }
            else
            {
                MessageBox.Show("No server have created.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.ServerListCursor != -1)
            {
                try
                {
                    if (this.RunServer(this.ServerList[this.ServerListCursor]) == false)
                    {
                        MessageBox.Show("Server is already running.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else this.ServerList[this.ServerListCursor].ConfigRunning(true);
                }
                catch (UnauthorizedAccessException err)
                {
                    MessageBox.Show("Working directory access denied.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No server have created.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.ServerListCursor == -1)
            {
                MessageBox.Show("No server have created.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.ServerList[this.ServerListCursor].fileServer == null)
            {
                MessageBox.Show("Must start server first.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.RunTestServer(this.ServerList[this.ServerListCursor]) == false)
            {
            }
            else
            {
                this.ServerList[this.ServerListCursor].ConfigTesting(true);
           }
            // open browser
            System.Diagnostics.Process.Start("http://localhost:" + this.ServerList[this.ServerListCursor].testPort);
        }
    }
}
