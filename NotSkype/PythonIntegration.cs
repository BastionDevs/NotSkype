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

        public static bool ReceiveMessages(string username, string password)
        {
            bool success = false;
            //code- execute python

            if (success) {
                return true;
            } else
            {
                return false;
            }

        }

        public static bool SendMessages(string username, string password, string message)
        {
            bool success = false;
            //code- execute python
            
            if (success)
            {
                return true;
            } else 
            { 
                return false; 
            }
        }

    }
}
