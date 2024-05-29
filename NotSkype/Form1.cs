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
    public partial class Form1 : Form
    {
        string placeholdertext;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Left = (this.ClientSize.Width - label2.Size.Width) / 2;
            label3.Left = (this.ClientSize.Width - label3.Size.Width) / 2;
            textBox1.Left = (this.ClientSize.Width - textBox1.Size.Width) / 2;
            button1.Left = (this.ClientSize.Width - button1.Size.Width) / 2;
            linkLabel1.Left = (this.ClientRectangle.Width - linkLabel1.Size.Width) / 2;

            textBox1.GotFocus += RemoveText;
            textBox1.LostFocus += AddText;
            
            placeholdertext = "Email, phone, or Skype name";

            button1.Focus();
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (textBox1.Text == placeholdertext)
            {
                textBox1.Text = "";
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = placeholdertext;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form2(textBox1.Text).ShowDialog();
            this.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.WindowState = FormWindowState.Minimized;
                e.Cancel = true;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            new IMWindow().ShowDialog();
        }
    }
}
