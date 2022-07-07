using System;
using System.Net;

namespace CSD
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client  {
                TcpIPAddress = IPAddress.Parse("192.168.1.167"),
                UdpIPAddress = IPAddress.Parse("192.168.1.255"),                
                TcpPort = 46000,
                UdpPort = 45000,                
            };

            client.Run();
        }
    }
}
