using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

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
            Console.WriteLine("Connected...");

            string str;
            NetworkStream ns = new NetworkStream(m_clientSock);
            StreamWriter sw = new StreamWriter(ns);
            StreamReader sr = new StreamReader(ns);

            while (true)
            {
                Console.Write("Text:");
                str = Console.ReadLine();
                sw.WriteLine(str);
                sw.Flush();
                if (str == "exit")
                    break;

                if ((str = sr.ReadLine()) == null)
                    break;

                Console.WriteLine(str);
            }

            sw.Close();
            m_clientSock.Shutdown(SocketShutdown.Both);
            m_clientSock.Close();
        }
    }
}
