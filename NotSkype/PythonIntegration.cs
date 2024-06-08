using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSkype
{
    internal class PythonIntegration
    {
        static string pythonpath;
        static string pythonport;
        static string os;

        static string uname;
        static string password;

        public static void InitPython(string path, string port, string winver) {
            pythonpath = path;
            pythonport = port;
            os = winver;

            if (os == "7")
            {

            } else if (os == "XP")
            {

            } else if (os == "10")
            {

            }
        }

        public static void Login(string username, string pwd)
        {
            uname = username;
            password = pwd;
        }

        public static void StartServer(string username, string password)
        {
            //code- execute python
        }

    }
}
