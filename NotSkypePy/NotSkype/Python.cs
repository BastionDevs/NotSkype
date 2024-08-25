using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NotSkype
{
    internal class Python
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
            PythonUtils.StartPython(username, password, Config.ClientPort, Config.PythonFlaskPort);
        }

    }
}
