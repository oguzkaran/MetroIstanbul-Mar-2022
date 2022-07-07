using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

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
        private BinaryFormatter m_bf;
        private NetworkStream m_ns;

        public Server(int port)
        {
            m_port = port;
        }

        public void Run()
        {
            m_serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_serverSock.Bind(new IPEndPoint(IPAddress.Any, m_port));
            m_serverSock.Listen(8);
           
            Console.WriteLine("waiting for connection");
            m_clientSock = m_serverSock.Accept();
            Console.WriteLine("Connected: {0}", m_clientSock.RemoteEndPoint.ToString());

            m_bf = new BinaryFormatter();
            m_ns = new NetworkStream(m_clientSock);

            object o;

            try
            {
                for (; ; )
                {
                    o = m_bf.Deserialize(m_ns);
                    switch (o.GetType().Name)
                    {
                        case "CTSConnect":
                            procCTSConnect(o);
                            break;
                        case "CTSSend":
                            if (!procCTSSend(o))
                                goto EXIT;
                            break;
                    }
                }
            }
            catch
            { }
        EXIT:
            m_clientSock.Shutdown(SocketShutdown.Both);
            m_clientSock.Close();
        }

        private void procCTSConnect(object o)
        {
            CTSConnect ctsConnect = o as CTSConnect;
            Console.WriteLine("Connected: {0}", ctsConnect.Nick);

            STCConnected stcConnected = new STCConnected();
            m_bf.Serialize(m_ns, stcConnected);
        }

        private bool procCTSSend(object o)
        {
            CTSSend ctsSend = o as CTSSend;
            if (ctsSend.Msg == "exit")
                return false;

            Console.WriteLine("{0}", ctsSend.Msg);
            return true;
        }

        public static int ReadSocket(Socket sock, byte[] buf, int n)
        {
            int index = 0;
            int left = n;
            int result;

            while (left > 0)
            {
                result = sock.Receive(buf, index, left, SocketFlags.None);
                if (result == 0)
                    break;
                index += result;
                left -= result;
            }

            return index;
        }
    }
} 