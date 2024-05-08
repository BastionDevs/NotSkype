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
            using (StreamWriter sw = new StreamWriter(File.Open(Directory.GetCurrentDirectory() + @"\main.cfg", FileMode.Create), Encoding.UTF8))
            {
                sw.WriteLine(@"[main]");
                sw.WriteLine("servername = MyNewServer");
                sw.WriteLine("serverport = 13000");
            }
        }

    }
}
