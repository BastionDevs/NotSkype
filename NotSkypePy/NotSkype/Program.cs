using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NotSkype
{
    internal static class Program
    {
        const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Get the current process
            var currentProcess = Process.GetCurrentProcess();
            var processName = currentProcess.ProcessName;

            // Find all processes with the same name
            var processes = Process.GetProcessesByName(processName);

            // Check if there is more than one process with the same name
            if (processes.Length > 1)
            {
                // Get the other process (assuming the first one is the current process)
                var otherProcess = processes.FirstOrDefault(p => p.Id != currentProcess.Id);

                if (otherProcess != null)
                {
                    // Bring the other process to the foreground
                    IntPtr handle = otherProcess.MainWindowHandle;
                    if (handle == IntPtr.Zero)
                    {
                        handle = FindWindow(null, otherProcess.MainWindowTitle);
                    }
                    if (handle != IntPtr.Zero)
                    {
                        ShowWindow(handle, SW_RESTORE);
                        SetForegroundWindow(handle);
                    }
                    return; // Exit the current instance
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
