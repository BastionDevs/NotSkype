using Newtonsoft.Json.Linq;
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
        string CurrentUname;
        string CurrentDisp;
        public Form3(string Username)
        {
            InitializeComponent();
            CurrentUname = PythonUtils.GetSelfUsername();
            Thread.Sleep(100);
            CurrentDisp = PythonUtils.CurrentUserDisplayName();
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

        //httplistener
        private HttpListener _listener2;
        private Thread _listenerThread2;

        private void StartHttpListener2()
        {
            _listener2 = new HttpListener();
            _listener2.Prefixes.Add("http://localhost:" + Config.ClientPort + "/othermsg");
            _listener2.Start();
            _listenerThread2 = new Thread(new ThreadStart(ListenForRequests2));
            _listenerThread2.IsBackground = true;
            _listenerThread2.Start();
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

        private void HandlePostRequest2(HttpListenerContext context)
        {
            string requestBody;
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                requestBody = reader.ReadToEnd();
            }

            // Here you can process the request body as needed
            Console.WriteLine(requestBody);
            JObject jsonObject = JObject.Parse(requestBody);

            string username = (string)jsonObject["user"];
            string message = (string)jsonObject["message"];

            notifyIcon1.Text = "NotSkype";
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipTitle = "NotSkype - New message";
            notifyIcon1.BalloonTipText = $"{username}: {message}";
            notifyIcon1.ShowBalloonTip(3000);

            // Send a response
            var responseString = "Data received";
            var buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_listener2 != null && _listener2.IsListening)
            {
                _listener2.Stop();
            }
            if (_listenerThread2 != null && _listenerThread2.IsAlive)
            {
                _listenerThread2.Join();
            }
        }
    }
}
