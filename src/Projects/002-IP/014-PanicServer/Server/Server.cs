using CSD.Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using static CSD.Net.TcpUtil;
using static CSD.Net.UdpUtil;

namespace CSD
{
    public class Server
    {
        private List<Socket> m_clients = new List<Socket>();
        private Socket m_udpSocket;
        private Socket m_tcpServerSocket;

        private class ClientMessageInfo
        {
            public byte[] Data { get; set; }
            public int Length { get; set; }
            public EndPoint EndPoint { get; set; }                            
        }

        private void handleTcpMessage(object o)
        {
            var socket = (Socket)o;
            var message = socket.ReceiveString();

            Console.WriteLine($"Panic Message:{message}");
            socket.SendString("Do not panic!!!");
        }

        private void handleUdpMessage(object o)
        {
            var clientMessageInfo = (ClientMessageInfo)o;
            string message;

            message = Encoding.UTF8.GetString(clientMessageInfo.Data, 0, clientMessageInfo.Length);

            Console.WriteLine($"{clientMessageInfo.EndPoint}:{message}");
        }        

        private void runUdpServer()
        {
            try
            {
                m_udpSocket = InitUdpServer(UdpPort);
                Console.WriteLine("Running....");

                while (true)
                {
                    EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

                    byte[] data = new byte[1024];

                    int result = m_udpSocket.ReceiveFrom(data, ref endPoint);

                    ThreadPool.QueueUserWorkItem(handleUdpMessage,
                        new ClientMessageInfo { Data = data, Length = result, EndPoint = endPoint });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                if (m_udpSocket != null)
                    m_udpSocket.Close();
            }
        }

        private void runTcpServer()
        {
            try
            {
                m_tcpServerSocket = InitTcpServer(TcpPort, 200);

                Console.WriteLine("TCP server is running");
                while (true) {
                    var socket = m_tcpServerSocket.Accept();

                    lock (m_clients) {
                        m_clients.Add(socket);
                    }

                    ThreadPool.QueueUserWorkItem(handleTcpMessage, socket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (m_tcpServerSocket != null)
                    m_tcpServerSocket.CloseSocket();
            }
        }

        public int UdpPort { get; set; }
        public int TcpPort { get; set; }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o => runUdpServer());
            ThreadPool.QueueUserWorkItem(o => runTcpServer());
            Console.ReadKey();
        }        
    }
}
