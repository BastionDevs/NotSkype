using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TCPServer
{
    private static bool isRunning = true;
    private static TcpListener server;

    static void Main(string[] args)
    {
        string USPVer = "1.89 Internal";
        string USPProj = "NotSkype Server";
        string USPProjVer = "Beta 1.46";

        Console.WriteLine("Bastion Unified Server Platform, version " + USPVer);
        Console.WriteLine("Current project: " + USPProj + ", " + USPProjVer);
        Console.WriteLine();
        StartServer();
    }

    static void StartServer()
    {
        try
        {
            // Set the TcpListener on port 13000.
            int port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // Create and start the TcpListener.
            server = new TcpListener(localAddr, port);
            server.Start();

            Console.WriteLine("Server started. Waiting for connections...");

            // Enter the listening loop.
            while (isRunning)
            {
                try
                {
                    // Accept the client connection
                    TcpClient client = server.AcceptTcpClient();

                    // Create a separate thread to handle the client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(client);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.Interrupted)
                        break;
                    else
                        throw;
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
            Console.WriteLine("Server stopped.");
        }
    }

    static void HandleClient(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;
        NetworkStream stream = client.GetStream();

        byte[] bytes = new byte[1024];
        string data;

        try
        {
            while (true)
            {
                int i = stream.Read(bytes, 0, bytes.Length);
                if (i == 0)
                    break;

                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                // Parse different inputs
                string response = "";
                if (data.Trim().Equals("time", StringComparison.OrdinalIgnoreCase))
                {
                    // Get current time
                    response = DateTime.Now.ToString("HH:mm:ss");
                }
                else if (data.Trim().Equals("date", StringComparison.OrdinalIgnoreCase))
                {
                    // Get current date
                    response = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else if (data.Trim().Equals("hello", StringComparison.OrdinalIgnoreCase))
                {
                    // Greet the client
                    response = "Hello! How can I assist you?";
                }
                else if (data.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    // Exit command: send response and stop the server
                    response = "Server is shutting down. Goodbye!";
                    byte[] msg = Encoding.ASCII.GetBytes(response);
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", response);
                    StopServer(); // Stop the server
                    break; // Exit the loop
                }
                else
                {
                    // Invalid input
                    response = "Invalid input. Available commands: time, date, hello, exit";
                }

                // Convert the response to bytes and send it back to the client
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                Console.WriteLine("Sent: {0}", response);
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: {0}", ex.Message);
        }
        finally
        {
            // Close the connection with the client
            stream.Close();
            client.Close();
        }
    }

    static void StopServer()
    {
        isRunning = false;
        server.Stop();
        Environment.Exit(0); // Terminate the application
    }
}
