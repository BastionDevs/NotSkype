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

        private HttpListener _listener;
        private Thread _listenerThread;

        public IMWindow(string recepient, string sender)
        {
            InitializeComponent();

            displayName = PythonUtils.GetDisplayName(recepient);
            displayNameSelf = PythonUtils.CurrentUserDisplayName();

            PythonUtils.IntendedUser(recepient);

            recepientName = recepient;
            label6.Text = recepient;
            label5.Text = displayName;

            StartHttpListener();
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
            AddMessage(textBoxChatMsg.Text, displayNameSelf);
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

        public void AddMessage(string message, string username)
        {
            textBoxChatLog.Text += "\r\n <" + username + "> " + message;
        }

        public void SendMessageToUser(string message) {
            PythonUtils.SendMessage(recepientName, message);
        }

        //the majestic
        //NotSkype Project
        //HTTP POST Listener
        //
        //(C) 2024 ChatGPT
        //it works, so why not use it?

        private void StartHttpListener()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{Config.ClientPort}/");
            _listener.Start();
            _listenerThread = new Thread(new ThreadStart(ListenForRequests));
            _listenerThread.IsBackground = true;
            _listenerThread.Start();
        }

        private void ListenForRequests()
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = _listener.GetContext();
                    if (context.Request.HttpMethod == "POST")
                    {
                        HandlePostRequest(context);
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                        context.Response.Close();
                    }
                }
                catch (HttpListenerException)
                {
                    // Listener was stopped, ignore exception
                }
                catch (Exception ex)
                {
                    // Handle any other exceptions
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void HandlePostRequest(HttpListenerContext context)
        {
            string requestBody;
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                requestBody = reader.ReadToEnd();
            }

            // Here you can process the request body as needed
            Console.WriteLine(requestBody);

            // Send a response
            AddMessage(requestBody, displayName);
            var responseString = "Data received";
            var buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        private void IMWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_listener != null && _listener.IsListening)
            {
                _listener.Stop();
            }
            if (_listenerThread != null && _listenerThread.IsAlive)
            {
                _listenerThread.Join();
            }
        }
    }
}
