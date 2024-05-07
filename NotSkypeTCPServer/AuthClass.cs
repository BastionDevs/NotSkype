using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotSkypeTCPServer
{
    public class AuthClass
    {
        public static void Login(string uname, string pwd, string code)
        {
            Console.WriteLine("Reading file at " + Config.ReadINI(Directory.GetCurrentDirectory() + @"\main.cfg", "storefile", "userlist"));
        }

        public static void Register(string uname, string pwd, string email)
        {

        }
    }
}
