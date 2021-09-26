using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteShell
{
    partial class Form1
    {
        private const int ADD_PAGE = 1;
        private const int DISPLAY_PAGE = 2;
        private const int MAIN_PAGE = 3;


        private int CurrentPageFlag = MAIN_PAGE;

        private String CurrentHost = "";
        private String CurrentToken = "";
        private String CurrentWorkingDirectory = "";
        private int CurrentPort = 80;

        private void SwitchSecondPanel()
        {
            panel2.BringToFront();
            panel2.Visible = true;
            panel1.Visible = false;
        }

        private void SwitchAddServerEntry()
        {
            this.SwitchSecondPanel();

            // set default in page2
            textBox4.Text = "localhost";
            textBox3.Text = "";
            textBox2.Text = "8080";
            textBox1.Text = Directory.GetCurrentDirectory();

            this.CurrentPageFlag = ADD_PAGE;
        }

        private void SwitchDisplayServerEntry()
        {
            if(this.ServerListCursor != -1)
            {
                this.SwitchSecondPanel();

                // set information of current server
                FileServerEntry entry = this.ServerList[this.ServerListCursor];
                textBox4.Text = entry.host;
                textBox3.Text = entry.token;
                textBox2.Text = entry.port.ToString();
                textBox1.Text = entry.workingDirectory;

                button1.Enabled = false;
                button4.Enabled = false;
                this.CurrentPageFlag = DISPLAY_PAGE;
            }
            else MessageBox.Show("No server have created.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void SwitchMainPage()
        {
            if(this.CurrentPageFlag == DISPLAY_PAGE)
            {
                // cancel button disable
                button1.Enabled = true;
                button4.Enabled = true;
            }else if (this.CurrentPageFlag == ADD_PAGE)
            {
                // set added information
                try
                {
                    this.CurrentPort = int.Parse(textBox2.Text);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e);
                }
                this.CurrentHost = textBox4.Text;
                this.CurrentToken = textBox3.Text;
                this.CurrentWorkingDirectory = textBox1.Text;
            }
            panel1.BringToFront();
            panel1.Visible = true;
            panel2.Visible = false;
            this.CurrentPageFlag = MAIN_PAGE;
        }
    }
}
