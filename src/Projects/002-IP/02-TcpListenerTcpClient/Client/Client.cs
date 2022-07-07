using System;
using System.Net;
using System.Net.Sockets;

namespace CSD
{
    class App
    {
        public static void Main()
        {
            TcpClient tcpClient = new TcpClient("192.168.0.124", 5050);
                        
            Console.WriteLine("Connected...");
        }
    }
}
