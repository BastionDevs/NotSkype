using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NotSkype
{
    public class PythonUtils
    {

        //starting and stopping
        public static void StartPython(string email, string password, string clientport, string pyport)
        {
            Process process = new Process();
            //process.StartInfo.FileName = "python.exe";
            //process.StartInfo.UseShellExecute = false;
            //process.StartInfo.CreateNoWindow = false;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.RedirectStandardError = true;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

            //process.StartInfo.Arguments = $"C:\\BastionSG\\NotSkype\\PythonScript\\skype_listener.py {email} {password} {clientport} {pyport}";

            //cmd method
            process.StartInfo.FileName = "CMD.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.Arguments = $"/K python.exe C:\\BastionSG\\NotSkype\\PythonScript\\skype_listener.py {email} {password} http://localhost:{clientport} {pyport}";

            process.Start();

            var outputStream = new StreamWriter("output.txt");

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    outputStream.WriteLine(e.Data);
                }
            });

            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    outputStream.WriteLine(e.Data);
                }
            });
        }

        public static void StopPython()
        {
            NetUtils.POSTRequest("http://localhost:" + Config.PythonFlaskPort + "/shutdown", "");
        }

        //user info
        public static void SendMessage(string user, string message)
        {
            NetUtils.POSTRequestJSON("http://localhost:"+Config.PythonFlaskPort+"/sendmessage", "{\"recipient\": \""+user+"\", \"message\": \""+message+"\"}");
        }

        public static string GetDisplayName(string user)
        {
            return NetUtils.POSTRequest("http://localhost:"+Config.PythonFlaskPort+"/displayname", user);
        }

        public static string CurrentUserDisplayName()
        {
            return NetUtils.POSTRequest("http://localhost:" + Config.PythonFlaskPort + "/currentdisplayname", "");
        }

        public static void IntendedUser(string user)
        {
            NetUtils.POSTRequest("http://localhost:" + Config.PythonFlaskPort + "/intendeduser", user);
        }

        public static string GetSelfUsername()
        {
            string result = NetUtils.POSTRequest("http://localhost:" + Config.PythonFlaskPort + "/currentusername", "");
            if (result.Contains("WebException"))
            {
                result = NetUtils.POSTRequest("http://localhost:" + Config.PythonFlaskPort + "/currentusername", "");
                if (result.Contains("WebException"))
                {
                    result = NetUtils.POSTRequest("http://localhost:" + Config.PythonFlaskPort + "/currentusername", "");
                    if (result.Contains("WebException"))
                    {
                        result = "Error occured while obtaining username.";
                    }
                }
            }

            return result;
        }

    }
}
