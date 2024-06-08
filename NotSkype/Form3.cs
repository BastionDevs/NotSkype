using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace NotSkype
{
    public partial class Form3 : Form
    {
        string CurrentUname;
        public Form3(string Username)
        {
            InitializeComponent();
            CurrentUname = Username;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label6.Text = CurrentUname;

            // URL for the GET request
            string url = "http://localhost:"+ Config.PythonFlaskPort +"/get_current_user_displayname";

            // Create a request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new IMWindow(textBoxUsername.Text, CurrentUname).Show();
        }
    }
}
