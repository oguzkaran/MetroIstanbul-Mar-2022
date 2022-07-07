using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CSD.Net;

namespace CSD
{
    class Program
    {
        public static void Main()
        {
            Socket socket = null;

            try
            {
                socket = TcpUtil.ConnectToServer(IPAddress.Parse("192.168.1.200"), 5120);
                Console.WriteLine("Connected...");
                    
                while (true)
                {
                    Console.Write("Text:");
                    var text = Console.ReadLine();

                    socket.SendString(text, Encoding.ASCII);

                    if (text == "quit")
                        break;

                    Console.WriteLine(socket.ReceiveString(Encoding.ASCII));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                if (socket != null)
                    socket.CloseSocket();                       
            }
        }
    }
}
