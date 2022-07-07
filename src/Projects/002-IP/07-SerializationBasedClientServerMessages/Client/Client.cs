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
                Client client = new Client("127.0.0.1", 5050);

                client.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class Client
    {
        private Socket m_clientSock;
        private int m_port;
        private string m_host;
        private BinaryFormatter m_bf;
        private NetworkStream m_ns;

        public Client(string host, int port)
        {
            m_port = port;
            m_host = host;
        }

        public void Run()
        {
            m_clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //m_clientSock.Bind(new IPEndPoint(IPAddress.Any, 5051));
            IPHostEntry hostEntry = Dns.GetHostEntry(m_host);

            IPAddress ipv4 = IPAddress.None;
            foreach (IPAddress ipaddr in hostEntry.AddressList)
                if (ipaddr.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipv4 = ipaddr;
                    break;
                }
            m_clientSock.Connect(ipv4, m_port);
            m_bf = new BinaryFormatter();
            m_ns = new NetworkStream(m_clientSock);

            string cmdLine, cmd, content;
            int index;
            
            for (; ; )
            {
                Console.Write("CSD>");
                cmdLine = Console.ReadLine().Trim();
                index = cmdLine.IndexOf(' ');
                cmd = cmdLine.Substring(0, index);
                content = cmdLine.Substring(index + 1).Trim();
                switch (cmd)
                { 
                    case "connect":
                        procConnect(content);
                        break;
                    case "send":
                        procSend(content);
                        break;
                    case "exit":
                        procSend("exit");
                        goto EXIT;
                }
            }
        EXIT:
            m_clientSock.Shutdown(SocketShutdown.Both);
            m_clientSock.Close();
        }

        private void procConnect(string nick)
        {
            CTSConnect ctsConnect = new CTSConnect(nick);
            m_bf.Serialize(m_ns, ctsConnect);

            if (m_bf.Deserialize(m_ns) is STCConnected)
            {
                Console.WriteLine("Message From server: 'connected'");
            }
        }

        private void procSend(string msg)
        {
            CTSSend ctsSend = new CTSSend(msg);
            m_bf.Serialize(m_ns, ctsSend);
        }
    }
}
