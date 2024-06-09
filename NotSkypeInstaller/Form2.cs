using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NotSkypeInstaller
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label3.Text = "Current action:\r\nDownloading the NotSkype executable...";
            DownloadUtils.DownloadFile("https://raw.githubusercontent.com/ItsAndrewDev/NotSkype/main/Release/latest/bin/NotSkype.exe", @"C:\BastionSG\NotSkype\NotSkype.exe");
            progressBar1.Value += 10;
            label3.Text = "Current action:\r\nDownloading configuration files...";
            DownloadUtils.DownloadFile("https://raw.githubusercontent.com/ItsAndrewDev/NotSkype/main/Release/latest/bin/config.ini", @"C:\BastionSG\NotSkype\config.ini");
            progressBar1.Value += 5;
            label3.Text = "Current action:\r\nDownloading scripts...";
            DownloadUtils.DownloadFile("https://raw.githubusercontent.com/ItsAndrewDev/NotSkype/main/Release/latest/bin/PythonScript/skype_listener.py", @"C:\BastionSG\NotSkype\PythonScript\skype_listener.py");
            progressBar1.Value += 10;
            label3.Text = "Current action:\r\nDownloading resources...";
            DownloadUtils.DownloadFile("https://raw.githubusercontent.com/ItsAndrewDev/NotSkype/main/Release/latest/bin/Images/NotSkype.ico", @"C:\BastionSG\NotSkype\Images\NotSkype.ico");
            progressBar1.Value += 5;

            label3.Text = "Current action:\r\nCreating a Desktop shortcut for NotSkype...";
            // Define the shortcut parameters
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\NotSkype.lnk";
            progressBar1.Value += 5;
            string targetPath = @"C:\BastionSG\NotSkype\NotSkype.exe"; // Path to the application executable
            string workingDirectory = @"C:\BastionSG\NotSkype\"; // Path to the application working directory
            progressBar1.Value += 5;
            string description = "Launch the NotSkype Client";
            string iconLocation = @"C:\BastionSG\NotSkype\Images\NotSkype.ico"; // Path to the icon file
            progressBar1.Value += 10;
            // Create a new WshShell object
            WshShell shell = new WshShell();
            // Create the shortcut
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            progressBar1.Value += 10;
            // Set the shortcut properties
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = workingDirectory;
            progressBar1.Value += 10;
            shortcut.Description = description;
            shortcut.IconLocation = iconLocation;
            progressBar1.Value += 10;
            // Save the shortcut
            shortcut.Save();
            progressBar1.Value += 10;

            Console.WriteLine("Shortcut created successfully!");

            label3.Text = "Current action:\r\nDeleting temporary files...";
            Directory.Delete(@"C:\BastionSG\NotSkype\InstallTemp", true);
            progressBar1.Value += 10;

            label3.Text = "Installation complete!\r\nClick on 'Finish' to exit the installer.";
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
