using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NotSkypeInstaller
{
    public partial class InstallPyWin10 : Form
    {
        public InstallPyWin10()
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

        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int RtlGetVersion(out OSVERSIONINFOEX lpVersionInformation);

        private void InstallPyWin10_Load(object sender, EventArgs e)
        {
            OSVERSIONINFOEX versionInfo = new OSVERSIONINFOEX();
            versionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));
            RtlGetVersion(out versionInfo);

            Console.WriteLine($"OS Version: {versionInfo.dwMajorVersion}.{versionInfo.dwMinorVersion}.{versionInfo.dwBuildNumber}");

            if (versionInfo.dwMajorVersion == 6 && versionInfo.dwMinorVersion == 2)
            {
                label4.Text += "8";
            }
            else if (versionInfo.dwMajorVersion == 6 && versionInfo.dwMinorVersion == 3)
            {
                label4.Text += "8.1";
            }
            else if (versionInfo.dwMajorVersion == 10)
            {
                if (versionInfo.dwBuildNumber < 22000)
                {
                    label4.Text += "10";
                }
                else
                {
                    label4.Text += "11";
                }
            }
            else
            {
                Console.WriteLine("Unknown Windows version");
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (!radioButtonSkipPy.Checked)
            {
                string arch;
                if (System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "AMD64") { arch = "x64"; } else { arch = "x86"; }

                string url;
                if (arch == "x64") { url = "https://www.python.org/ftp/python/3.12.4/python-3.12.4-amd64.exe"; } else { url = "https://www.python.org/ftp/python/3.12.4/python-3.12.4.exe"; }

                string filename = @"C:\BastionSG\NotSkype\InstallTemp\pyinstall-latest.exe";
                string filefolder = @"C:\BastionSG\NotSkype\InstallTemp\";

                DownloadUtils.DownloadFile(url, filename);
                ExecuteAsAdmin(filename, "/passive InstallAllUsers=1 PrependPath=1", filefolder);
                
            }
            new Form2().Show();
        }

        public void ExecuteAsAdmin(string fileName, string args, string folder)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.Arguments = args;
            proc.StartInfo.WorkingDirectory = folder;
            proc.Start();
            proc.WaitForExit();
        }
    }
}
