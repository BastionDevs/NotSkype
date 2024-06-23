using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace NotSkype
{
    internal class PythonIntegration
    {

        static string pythonport;
        static string clientport;

        static string uname;
        static string password;

        static string firstboot;

        public static void InitPython() 
        {
            Config.GetConfig();
            pythonport = Config.PythonFlaskPort;
            clientport = Config.ClientPort;
            firstboot = Config.FirstBoot;
        }

        public static void Login(string username, string pwd)
        {
            uname = username;
            password = pwd;

            StartServer(uname, pwd);
        }

        public static void StartServer(string username, string password)
        {
            //code- execute python
            if (firstboot == "true")
            {
                InstallPythonPackages();
            }
            PythonUtils.StartPython(username, password, Config.ClientPort, Config.PythonFlaskPort);
        }

        static void InstallPythonPackages()
        {
            // Path to pip.exe
            string pipPath = "python.exe";

            // The arguments for pip install command
            string arguments = "-m pip install requests Flask skpy simplejson";

            // Create a new process start info
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pipPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            // Create a new process
            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => MessageBox.Show("ERROR: " + e.Data);

                // Start the process
                process.Start();

                // Begin reading the output and error streams
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Wait for the process to exit
                process.WaitForExit();

                Console.WriteLine($"Process exited with code {process.ExitCode}");
            }
        }
    }
}
