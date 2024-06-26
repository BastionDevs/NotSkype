﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotSkypeTCPServer
{
    public class AuthClass
    {
        static string userconfigini = Config.ReadINI(Directory.GetCurrentDirectory() + @"\main.cfg", "auth", "usersini");
        
        public static bool Login(string uname, string pwd, string code)
        {
            Console.WriteLine("User " + uname + " is trying to log in.");
            Console.WriteLine("Checking credentials for user " + uname + ".");
            Console.WriteLine("Reading file at " + userconfigini + "...");
            if (Config.ReadINI(userconfigini, uname+"userpassword", "userdb") == pwd)
            {
                Console.WriteLine();
                Console.WriteLine("Username and Password matches, code " + code + "is activated.");
                Config.WriteINI(userconfigini, code, uname, "codedb");
                return true;
            } else
            {
                Console.WriteLine();
                Console.WriteLine("[WARN] Specified Password for user " + uname + " is incorrect.");
                return false;
            }
        }

        public static void Register(string uname, string pwd)
        {
            Console.WriteLine("Setting credentials for user " + uname + ".");
            Console.WriteLine("Writing to file at " + userconfigini + "...");
            Config.WriteINI(userconfigini, uname + "userpassword", pwd, "userdb");
            Console.WriteLine();
            Console.WriteLine("Username and Password has been written, user " + uname + " has been registered.");
        }

        // UserInfo("mode", new string[] {"arg1", "arg2"});
        static void UserInfo(string mode, string[] args)
        {
            
        }
    }
}
