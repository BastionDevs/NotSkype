using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NotSkype
{
    public partial class Form3 : Form
    {
        string CurrentUname = PythonUtils.GetSelfUsername();
        string CurrentDisp = PythonUtils.CurrentUserDisplayName();
        public Form3(string Username)
        {
            InitializeComponent();
            Thread.Sleep(100);
            label6.Text = CurrentUname;
            label5.Text = CurrentDisp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new IMWindow(textBoxUsername.Text, CurrentUname).Show(this);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
