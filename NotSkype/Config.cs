using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyINIController;

namespace NotSkype
{
    internal class Config
    {

        static string ProjVer;
        static string ProjName = "NotSkype";
        static string InstallPath;
        static string UtilsPath;

        static int pythonport = 25162;
        static int clientport = 25161;

        static void GetConfig()
        {
            IniFile ConfigFile = new IniFile(Directory.GetCurrentDirectory() + @"\config.ini");
            ProjVer = ConfigFile.Read("projver", "swinfo");
            InstallPath = ConfigFile.Read("installdir", "swinfo");
            UtilsPath = ConfigFile.Read("utilsdir", "swinfo");


        }

    }
}
