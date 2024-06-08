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
        private HttpListener _listener;
        private Thread _listenerThread;

        private HttpListener _listener2;
        private Thread _listenerThread2;

        string senderName = "";
        string recepientName = "";
        string displayName = "";
        string displayNameSelf = "";

        public IMWindow(string recepient, string sender)
        {
            InitializeComponent();
            StartHttpListener();
            StartHttpListener2();
            recepientName = recepient;
            senderName = sender;

            try
            {
                string url = "http://localhost:"+Config.PythonFlaskPort+"/getdisplayname";
                string postData = recepientName;
                string contentType = "application/x-www-form-urlencoded";

                string responseBody = SendPostRequest(url, postData, contentType);
                displayName = responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            label6.Text = recepientName;

            // URL for the GET request
            string url2 = "http://localhost:"+Config.PythonFlaskPort+"/get_current_user_displayname";

            // Create a request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url2);
            request.Method = "GET"; // Specify the GET method

            try
            {
                // Get the response from the server
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Check if the response status is OK (200)
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Read the response stream
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseText = reader.ReadToEnd();
                            Console.WriteLine("Response: " + responseText);
                            displayNameSelf = responseText;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle any exceptions that occur during the request
                if (ex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
                    {
                        Console.WriteLine("Error: " + errorResponse.StatusCode);
                        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorText = reader.ReadToEnd();
                            Console.WriteLine("Error Details: " + errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("WebException: " + ex.Message);
                }
            }
        }

        private void StartHttpListener()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:"+Config.ClientPort+"/");
            _listener.Start();
            _listenerThread = new Thread(new ThreadStart(ListenForRequests));
            _listenerThread.IsBackground = true;
            _listenerThread.Start();
        }

        private void StartHttpListener2()
        {
            _listener2 = new HttpListener();
            _listener2.Prefixes.Add("http://localhost:"+Config.ClientPort+"/othermsg/");
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

        //othermsg
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

            //parse and notif msg

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
            string msg = textBoxChatMsg.Text;
            AddMessage(textBoxChatMsg.Text, displayNameSelf);
            textBoxChatMsg.Text = string.Empty;

            //send chat to person
            SendMessageToUser(msg);
        }

        private void SendMessageToUser(string msg)
        {
            // URL for the POST request
            string url = "http://localhost:"+Config.PythonFlaskPort+"/send_message";

            // JSON data to send in the POST request
            string postData = "{\"recipient\": "+recepientName+", \"message\": \""+msg+"\"}";

            // Create a request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST"; // Specify the POST method
            request.ContentType = "application/json"; // Set the Content-Type header

            try
            {
                // Convert the JSON data to a byte array
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Set the ContentLength property of the WebRequest
                request.ContentLength = byteArray.Length;

                // Get the request stream and write the data to it
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response from the server
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Check if the response status is OK (200)
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Read the response stream
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseText = reader.ReadToEnd();
                            Console.WriteLine("Response: " + responseText);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle any exceptions that occur during the request
                if (ex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
                    {
                        Console.WriteLine("Error: " + errorResponse.StatusCode);
                        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorText = reader.ReadToEnd();
                            Console.WriteLine("Error Details: " + errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("WebException: " + ex.Message);
                }
            }
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


        static string SendPostRequest(string url, string postData, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = contentType;

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
