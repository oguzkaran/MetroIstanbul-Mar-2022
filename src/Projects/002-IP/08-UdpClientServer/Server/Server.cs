using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSD
{
    class App
    {
        public static void Main()
        {
            Server server = new Server(IPAddress.Parse("127.0.0.1"), 5050);

            server.Run();
        }
    }


    class Server
    {
        private IPAddress m_ipAddr;
        private int m_port;
        private Socket m_serverSock;

        public Server(IPAddress ipAddr, int port)
        {
            m_ipAddr = ipAddr;
            m_port = port;
        }

        public void Run()
        {
            EndPoint endPoint;
            int result;

            try
            {
                m_serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_serverSock.Bind(new IPEndPoint(IPAddress.Any, m_port));

                Console.WriteLine("Running...");
                for (; ;)
                {
                    endPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] buf = new byte[1024];
                    result = m_serverSock.ReceiveFrom(buf, ref endPoint);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(procUdpMsg), new ClientMsg(buf, result, endPoint));
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void procUdpMsg(object o)
        {
            ClientMsg clientMsg = (ClientMsg)o;
            string msg;

            msg = Encoding.UTF8.GetString(clientMsg.Buf, 0, clientMsg.Length);

            Console.WriteLine("{0}:{1}", clientMsg.EndPoint.ToString(), msg);
        }

        private struct ClientMsg
        {
            public ClientMsg(byte[] buf, int length, EndPoint endPoint)
            {
                Buf = buf;
                Length = length;
                EndPoint = endPoint;
            }

            public byte[] Buf;
            public int Length;
            public EndPoint EndPoint;
        }
    }
}