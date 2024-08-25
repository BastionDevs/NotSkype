using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyINIController;

namespace NotSkype
{
    class Config
    {

        public static string ProjVer;
        public static string ProjName = "NotSkype";
        public static string InstallPath;
        public static string UtilsPath;

        public static string PythonFlaskPort;
        public static string ClientPort;

        public static string FirstBoot;

        public static void GetConfig()
        {
            IniFile ConfigFile = new IniFile(Directory.GetCurrentDirectory() + @"\config.ini");
            ProjVer = ConfigFile.Read("projver", "swinfo");
            InstallPath = ConfigFile.Read("installdir", "swinfo");
            UtilsPath = ConfigFile.Read("utilsdir", "swinfo");

            PythonFlaskPort = ConfigFile.Read("pyport", "servercfg");
            ClientPort = ConfigFile.Read("clientport", "servercfg");

            FirstBoot = ConfigFile.Read("firstboot", "appcfg");
        }

    }
}
