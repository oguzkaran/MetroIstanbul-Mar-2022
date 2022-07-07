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
            Client client = new Client(IPAddress.Parse("127.0.0.1"), 5050);

            client.Run();
        }
    }


    class Client
    {
        private IPAddress m_ipAddr;
        private int m_port;
        private Socket m_clientSock;

        public Client(IPAddress ipAddr, int port)
        {
            m_ipAddr = ipAddr;
            m_port = port;
        }

        public void Run()
        {
            int result;

            try
            {
                m_clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                for (; ; )
                {
                    Console.Write("Text:");
                    string str = Console.ReadLine();
                    byte[] buf = Encoding.UTF8.GetBytes(str);
                    result = m_clientSock.SendTo(buf, new IPEndPoint(m_ipAddr, m_port));
                    if (result == 0)
                        break;
                    if (str == "quit")
                        break;
                }

                m_clientSock.Close();       // unreachable code...
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