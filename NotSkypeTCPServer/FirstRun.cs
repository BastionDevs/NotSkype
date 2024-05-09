using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotSkypeTCPServer
{
    public class FirstRun
    {
        public static void MakeConfig()
        {
            //Creates main Server configuration INI
            using (StreamWriter sw = new StreamWriter(File.Open(Directory.GetCurrentDirectory() + @"\main.cfg", FileMode.Create), Encoding.UTF8))
            {
                sw.WriteLine(@"[main]");
                sw.WriteLine("servername = MyNewServer");
                sw.WriteLine("serverport = 13000");
            }

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\data");

            //Creates User database INI
            using (StreamWriter sw = new StreamWriter(File.Open(Directory.GetCurrentDirectory() + @"\data\user.cfg", FileMode.Create), Encoding.UTF8))
            {
                sw.WriteLine("[userdb]");
                sw.WriteLine("adminuserpassword = AdministratorPasswordHere");
                sw.WriteLine();
                sw.WriteLine("[codedb]");
            }
        }
    }
}
