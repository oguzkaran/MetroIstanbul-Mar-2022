using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSD
{
    class App
    {
        public static void Main()
        {
            try
            {
                Server server = new Server(5050);

                server.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class Server
    {
        private Socket m_serverSock;
        private Socket m_clientSock; 
        private int m_port;

        public Server(int port)
        {
            m_port = port;
        }

        public void Run()
        {
            m_serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_serverSock.Bind(new IPEndPoint(IPAddress.Any, m_port));
            m_serverSock.Listen(8);

            while (true)
            {
                Console.WriteLine("waiting for connection");
                m_clientSock = m_serverSock.Accept();
                Console.WriteLine("Connected: {0}", m_clientSock.RemoteEndPoint.ToString());

                byte[] buf = new byte[1024];
                int result;
                string msg;

                while (true)
                {
                    result = m_clientSock.Receive(buf, 0, 1024, SocketFlags.None);
                    if (result == 0)
                        break;
                    msg = Encoding.UTF8.GetString(buf, 0, result);  // Dikkat ihmal var: gönderilen yazının tamamının tek hamlede alınması garanti değildir.
                    if (msg == "exit")
                        break;
                    Console.WriteLine(msg);
                }

                m_clientSock.Shutdown(SocketShutdown.Both);
                m_clientSock.Close();
            }
        }
    }
} 