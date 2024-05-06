using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyINIController;

namespace NotSkypeTCPServer
{
    public class Config
    {
        public static string ReadINI(string file, string key, string section)
        {
            IniFile inifile = new IniFile(file);
            if (inifile.KeyExists(key, section)) { return inifile.Read(key, section); } else { return "[TinyINI]MissingKey"; }
        }

        public static void WriteINI(string file, string key, string value, string section)
        {
            IniFile inifile = new IniFile(file);
            inifile.Write(key, value, section);
        }
    }
}
