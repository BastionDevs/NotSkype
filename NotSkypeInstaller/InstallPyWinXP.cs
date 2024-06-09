using Ionic.Zip;
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
    public partial class InstallPyWinXP : Form
    {
        public InstallPyWinXP()
        {
            InitializeComponent();
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

                string url= "https://raw.githubusercontent.com/ItsAndrewDev/NotSkype/master/Python/py3-4-10.zip";

                string filename = @"C:\BastionSG\NotSkype\InstallTemp\py3-4-10.zip";

                DownloadUtils.DownloadFile(url, filename);
                //ExecuteAsAdmin(filename, "/passive InstallAllUsers=1 PrependPath=1");

                string unzippath = "C:\\BastionSG\\NotSkype\\InstallTemp\\pyxp";
                unzip(filename, unzippath);

                Process proc = new Process();
                proc.StartInfo.FileName = "msiexec.exe";
                proc.StartInfo.Arguments = "/i "+ unzippath +"\\python-3.4.10.msi ALLUSERS=1 ADDLOCAL=ALL /passive";
                proc.Start();
                proc.WaitForExit();

                Process proc2 = new Process();
                proc2.StartInfo.FileName = "python.exe";
                proc2.StartInfo.Arguments = "get-pip.py --no-index --find-links=.";
                proc2.StartInfo.WorkingDirectory = unzippath;
                proc2.Start();
                proc2.WaitForExit();

            }
            else
            {
                //next form
            }
        }

        private void unzip(string zipFilePath, string extractPath)
        {
            //string zipFilePath = @"C:\path\to\your\zipfile.zip";
            //string extractPath = @"C:\path\to\extract\directory";

            try
            {
                using (ZipFile zip = ZipFile.Read(zipFilePath))
                {
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                }

                Console.WriteLine("Extraction complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
