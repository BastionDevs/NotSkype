using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NotSkype
{
    public class NetUtils
    {

        public static string POSTRequest(string server, string data)
        {
            // Create a request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server);
            request.Method = "POST"; // Specify the POST method

            // Check if data is provided
            if (!string.IsNullOrEmpty(data))
            {
                // Set the ContentType header
                request.ContentType = "application/x-www-form-urlencoded";

                // Convert the data to a byte array
                byte[] byteArray = Encoding.UTF8.GetBytes(data);

                // Set the ContentLength property of the WebRequest
                request.ContentLength = byteArray.Length;

                // Get the request stream and write the data to it
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
            }
            else
            {
                // Set the ContentLength property to 0 if no data is provided
                request.ContentLength = 0;
            }

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
                            return reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        return "Error: " + response.StatusCode;
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
                        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            return "Error: " + errorResponse.StatusCode + " - " + reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    return "WebException: " + ex.Message;
                }
            }
        }

        public static string POSTRequestJSON(string server, string data)
        {
            // Create a request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server);
            request.Method = "POST"; // Specify the POST method

            // Check if data is provided
            if (!string.IsNullOrEmpty(data))
            {
                // Set the ContentType header
                request.ContentType = "application/json";

                // Convert the data to a byte array
                byte[] byteArray = Encoding.UTF8.GetBytes(data);

                // Set the ContentLength property of the WebRequest
                request.ContentLength = byteArray.Length;

                // Get the request stream and write the data to it
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
            }
            else
            {
                // Set the ContentLength property to 0 if no data is provided
                request.ContentLength = 0;
            }

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
                            return reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        return "Error: " + response.StatusCode;
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
                        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            return "Error: " + errorResponse.StatusCode + " - " + reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    return "WebException: " + ex.Message;
                }
            }
        }

        public static string GETRequest(string server)
        {
            // Create a request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server);
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
                            return reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        return "Error: " + response.StatusCode;
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
                        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            return "Error: " + errorResponse.StatusCode + " - " + reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    return "WebException: " + ex.Message;
                }
            }
        }

    }
}
