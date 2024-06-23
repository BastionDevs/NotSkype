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
    public partial class IMWindow : Form
    {

        string senderName = "";
        string recepientName = "";
        string displayName = "";
        string displayNameSelf = "";

        public IMWindow(string recepient, string sender)
        {
            InitializeComponent();
            recepientName = recepient;
            senderName = PythonUtils.GetSelfUsername();
            displayName = PythonUtils.CurrentUserDisplayName();
            label6.Text = recepient;
            label5.Text = PythonUtils.GetDisplayName(recepient);
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
            string msg = textBoxChatMsg.Text;
            AddMessage(textBoxChatMsg.Text, senderName);
            textBoxChatMsg.Text = string.Empty;

            //send chat to person
            SendMessageToUser(msg);
        }

        private void IMWindow_Load(object sender, EventArgs e)
        {
            textBoxChatLog.ForeColor = Color.Black;
            this.Text = "NotSkype - " + recepientName;
        }

        private void textBoxChatMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSendChat.PerformClick();
            }
        }

        private void IMWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public void AddMessage(string message, string username)
        {
            textBoxChatLog.Text += "\r\n <" + username + "> " + message;
        }

        public void SendMessageToUser(string message) {
            PythonUtils.SendMessage(recepientName, message);
        }
        
    }
}
