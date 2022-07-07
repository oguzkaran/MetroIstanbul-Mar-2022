using System;

namespace CSD
{
    class Program
    {
        public static void Main(string[] args)
        {
            var server = new Server { UdpPort = 45000, TcpPort = 46000 };

            server.Run();
        }
    }
}
