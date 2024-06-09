using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NotSkypeInstaller
{
    public class DownloadUtils
    {

        public static int DlWithMirror(string primaryUrl, string mirrorUrl, string destinationPath)
        {
            if (DownloadFile(primaryUrl, destinationPath))
            {
                Console.WriteLine("Downloaded file from primary URL.");
                return 0;
            }
            else
            {
                Console.WriteLine("Primary URL failed. Trying mirror URL...");
                if (DownloadFile(mirrorUrl, destinationPath))
                {
                    Console.WriteLine("Downloaded file from mirror URL.");
                    return 1;
                }
                else
                {
                    Console.WriteLine("Failed to download file from both URLs.");
                    return 2;
                }
            }
        }

        public static bool DownloadFile(string url, string destinationPath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destinationPath))) { Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)); }
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, destinationPath);
                }
                return true; // Download succeeded
            }
            catch (WebException)
            {
                return false; // Download failed
            }
        }

    }
}
