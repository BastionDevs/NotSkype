using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NotSkype
{
    public partial class Form2 : Form
    {
        string placeholdertext;
        string username;
        public Form2(string text)
        {
            InitializeComponent();
            username = text;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label2.Left = (this.ClientSize.Width - label2.Size.Width) / 2;
            label3.Left = (this.ClientSize.Width - label3.Size.Width) / 2;
            textBox1.Left = (this.ClientSize.Width - textBox1.Size.Width) / 2;
            button1.Left = (this.ClientSize.Width - button1.Size.Width) / 2;
            linkLabel1.Left = (this.ClientRectangle.Width - linkLabel1.Size.Width) / 2;

            textBox1.GotFocus += RemoveText;
            textBox1.LostFocus += AddText;

            placeholdertext = "Password";

            button1.Focus();
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (textBox1.Text == placeholdertext)
            {
                textBox1.UseSystemPasswordChar = true;
                textBox1.Text = "";
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) {
                textBox1.UseSystemPasswordChar = false;
                textBox1.Text = placeholdertext;
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PythonIntegration.Login(username, textBox1.Text);
            this.Hide();
            PythonIntegration.Login(username, textBox1.Text);
            new Form3(username).ShowDialog();
            PythonIntegration.StopServer();
            Application.Exit();
        }

        private void SendToServer(string cmdstring)
        {
            //MessageBox.Show("TCP Server Not Configured!");
            //MessageBox.Show("[DEBUG] " + cmdstring);
        }
    }
}
