using System;
using System.Net;
using System.Net.Sockets;

namespace CSD
{
    class App
    {
        public static void Main()
        {
            Socket clientSock;

            TcpListener server = new TcpListener(IPAddress.Any, 5050);

            server.Start();

            while (true)
            {
                Console.WriteLine("Waiting for connection...");                
                clientSock = server.AcceptSocket();

                
                Console.WriteLine("Client connected");
            }
        }
    }
}