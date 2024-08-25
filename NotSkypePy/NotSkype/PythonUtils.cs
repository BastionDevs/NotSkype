using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NotSkype
{
    internal class PythonUtils
    {

        //starting and stopping
        public static void StartPython(string email, string password, string clientport, string pyport)
        {
            Process process = new Process();

            //cmd method
            process.StartInfo.FileName = "CMD.exe";
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.Arguments = $"/K python.exe C:\\BastionSG\\NotSkype\\PythonScript\\skype_listener.py {email} {password} http://localhost:{clientport} {pyport}";

            process.Start();
        }

        public static void StopPython()
        {
            NetUtils.POSTRequest("http://localhost:" + Config.PythonFlaskPort + "/shutdown", "");
        }

        //user info
        public static void SendMessage(string user, string message)
        {
            NetUtils.POSTRequestJSON("http://localhost:" + Config.PythonFlaskPort + "/sendmessage", "{\"recipient\": \"" + user + "\", \"message\": \"" + message + "\"}");
        }

        public static string GetDisplayName(string user)
        {
            return NetUtils.POSTRequest("http://localhost:" + Config.PythonFlaskPort + "/displayname", user);
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
