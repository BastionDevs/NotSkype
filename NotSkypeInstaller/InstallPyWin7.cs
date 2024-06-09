using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NotSkypeInstaller
{
    public partial class InstallPyWin7 : Form
    {
        public InstallPyWin7()
        {
            InitializeComponent();
        }

        private void InstallPyWin7_Load(object sender, EventArgs e)
        {
            if (System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor == 1)
            {
                label4.Text += "7/2008R2 ";
            } else if (System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor == 0) 
            {
                label4.Text += "Vista/2008 ";
            }

            string arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");

            if (arch == "AMD64" || arch == "IA64" || arch == "ARM64")
            {
                label4.Text += " x64";
            }
            else
            {
                label4.Text += " x86";
            }
        }

        private void radioButtonSkipPy_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSkipPy.Checked)
            {
                radioButtonInstall.Checked = false;
                radioButtonSelfInstall.Checked = false;
            }
        }

        private void radioButtonSelfInstall_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSelfInstall.Checked)
            {
                radioButtonInstall.Checked = false;
                radioButtonSkipPy.Checked = false;
            }
        }

        private void radioButtonInstall_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonInstall.Checked)
            {
                radioButtonSelfInstall.Checked = false;
                radioButtonSkipPy.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!radioButtonSkipPy.Checked)
            {
                string arch;
                if (System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "AMD64") { arch = "x64"; } else { arch = "x86"; }

                string url;
                if (arch == "x64") { url = "https://raw.githubusercontent.com/adang1345/PythonWindows/master/3.8.19/python-3.8.19-amd64-full.exe"; } else { url = "https://raw.githubusercontent.com/adang1345/PythonWindows/master/3.8.19/python-3.8.19-full.exe"; }

                string filename = @"C:\BastionSG\NotSkype\InstallTemp\pyinstall-3-8-19.exe";

                DownloadUtils.DownloadFile(url, filename);
                ExecuteAsAdmin(filename, "/passive InstallAllUsers=1 PrependPath=1");

            }
            else
            {
                //next form
            }
        }

        public void ExecuteAsAdmin(string fileName, string args)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.Arguments = args;
            proc.Start();
            proc.WaitForExit();
        }
    }
}
