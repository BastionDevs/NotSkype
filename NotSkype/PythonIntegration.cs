using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            RunScript();
        }

        public static void StopServer()
        {
            // URL for the POST request
            string url = "http://localhost:"+Config.PythonFlaskPort+"/shutdown";

            // Create a request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST"; // Specify the POST method

            try
            {
                // Get the response from the server
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Check if the response status is OK (200)
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Server shutdown request sent successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle any exceptions that occur during the request
                if (ex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
                    {
                        Console.WriteLine("Error: " + errorResponse.StatusCode);
                        // You can read error details here if necessary
                    }
                }
                else
                {
                    Console.WriteLine("WebException: " + ex.Message);
                }
            }
        }

        static void RunScript()
        {
            // Define the path to the python executable
            string pythonExePath = "python.exe";

            // Define the arguments for the Python script
            string scriptPath = Config.InstallPath + @"\PythonScript\skype_listener.py";
            string email = uname;
            string pwd = password;
            string url = "http://localhost:"+clientport;
            string port = pythonport;

            // Combine the script path and arguments into one string
            string arguments = $"{scriptPath} {email} {pwd} {url} {port}";

            // Create a new process start info
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonExePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Start the process
            Process process = new Process
            {
                StartInfo = psi
            };

            try
            {
                process.Start();

                // Read the standard output and error
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Print the output and error to the console
                Console.WriteLine("Output:");
                Console.WriteLine(output);
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }
            catch (Exception e)
            {
                //Console.WriteLine($"An error occurred: {e.Message}");
                MessageBox.Show($"An error occurred: {e.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
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
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Create a new process
            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine("ERROR: " + e.Data);

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
