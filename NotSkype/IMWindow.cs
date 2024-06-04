using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NotSkype
{
    public partial class IMWindow : Form
    {
        private HttpListener _listener;
        private Thread _listenerThread;

        private HttpListener _listener2;
        private Thread _listenerThread2;

        string senderName = "";
        string recepientName = "";
        string displayName = "";

        public IMWindow()
        {
            InitializeComponent();
            StartHttpListener();
            StartHttpListener2();
        }

        private void StartHttpListener()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:25161/");
            _listener.Start();
            _listenerThread = new Thread(new ThreadStart(ListenForRequests));
            _listenerThread.IsBackground = true;
            _listenerThread.Start();
        }

        private void StartHttpListener2()
        {
            _listener2 = new HttpListener();
            _listener2.Prefixes.Add("http://localhost:25161/displayname/");
            _listener2.Start();
            _listenerThread2 = new Thread(new ThreadStart(ListenForRequests2));
            _listenerThread2.IsBackground = true;
            _listenerThread2.Start();
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

        private void ListenForRequests2()
        {
            while (_listener2.IsListening)
            {
                try
                {
                    var context = _listener2.GetContext();
                    if (context.Request.HttpMethod == "POST")
                    {
                        HandlePostRequest2(context);
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
            ////Console.WriteLine(requestBody);

            ////MessageBox.Show(requestBody);

            AddMessage(requestBody, displayName);

            // Send a response
            var responseString = "Data received";
            var buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        //displayname
        private void HandlePostRequest2(HttpListenerContext context)
        {
            string requestBody;
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                requestBody = reader.ReadToEnd();
            }

            // Here you can process the request body as needed
            ////Console.WriteLine(requestBody);

            ////MessageBox.Show(requestBody);

            displayName = requestBody;

            // Send a response
            var responseString = "Data received";
            var buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
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
            AddMessage(textBoxChatMsg.Text, senderName);
            textBoxChatMsg.Text = string.Empty;

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

            if (_listener2 != null && _listener2.IsListening)
            {
                _listener2.Stop();
            }
            if (_listenerThread2 != null && _listenerThread2.IsAlive)
            {
                _listenerThread2.Join();
            }
        }

        public void AddMessage(string message, string username)
        {
            textBoxChatLog.Text += "\r\n <" + username + "> " + message;
        }
    }
}
