using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

using CSD.Net;

namespace CSD
{
    public class Client
    {        
        private Socket m_socket;

        private static Socket initTcpClient()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void doWorkForConnection()
        {
            m_socket.SendVarData(Encoding.UTF8.GetBytes("x.jpg"));
            m_socket.SendFile("x.jpg", 1024);
            m_socket.ReceiveFile("x-gs.jpg");

            m_socket.Shutdown(SocketShutdown.Both); //graceful shutdown
            m_socket.Close();
        }

        public string Host { get; set; }

        public int Port { get; set; }

        public void Run()
        {
            try
            {
                m_socket = initTcpClient();

                m_socket.Connect(Host, Port);
                Console.WriteLine("Connected...");
                doWorkForConnection();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
