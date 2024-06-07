using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace NotSkypeInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor == 1)
            {
                this.Hide();
                new InstallPyWin7().ShowDialog();
            }
            else if (System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor == 0)
            {
                this.Hide();
                new InstallPyWin7().ShowDialog();
            }
            else if (System.Environment.OSVersion.Version.Major == 5 && System.Environment.OSVersion.Version.Minor >= 1)
            {
                this.Hide();
                new InstallPyWinXP().ShowDialog();
            } else if (System.Environment.OSVersion.Version.Major >= 6)
            {
                this.Hide();
                new InstallPyWin10().ShowDialog();
            }

            MessageBox.Show("Installation completed!");
            Application.Exit();
        }
    }
}
