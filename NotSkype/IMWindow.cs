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
    public partial class IMWindow : Form
    {
        string senderName = "";

        public IMWindow()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExitIM();
        }

        private void ExitIM()
        {
            //maybe store chat here

            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonSendChat_Click(object sender, EventArgs e)
        {
            textBoxChatLog.Text += "\r\n <" + senderName + "> " + textBoxChatMsg.Text;

            //send chat to person
        }

        private void IMWindow_Load(object sender, EventArgs e)
        {
            textBoxChatLog.ForeColor = Color.Black;
        }

        private void textBoxChatMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSendChat.PerformClick();
            }
        }
    }
}
