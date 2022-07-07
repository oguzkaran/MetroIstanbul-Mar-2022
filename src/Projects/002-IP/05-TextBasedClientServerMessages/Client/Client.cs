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
        private StreamReader m_sr;
        private StreamWriter m_sw;
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
                        
            NetworkStream ns = new NetworkStream(m_clientSock);
            m_sw = new StreamWriter(ns);
            m_sr = new StreamReader(ns);

            string cmd, response;
                        
            for (; ; )
            {
                try
                {
                    Console.Write("CSD>");
                    cmd = Console.ReadLine();
                    m_sw.WriteLine(cmd);
                    m_sw.Flush();

                    response = m_sr.ReadLine();
                    if (!parseResponse(response))
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            m_sw.Close();
            m_sr.Close();

            m_clientSock.Shutdown(SocketShutdown.Both);
            m_clientSock.Close();
        }

        private bool parseResponse(string response)
        {
            int index;
            string cmd, content;

            response = response.TrimStart();

            if ((index = response.IndexOf(' ')) == -1) {
                cmd = response;
                content = null;
            }
            else
            {
                cmd = response.Substring(0, index);
                content = response.Substring(index + 1).Trim();
            }

            switch (cmd)
            {
                case "CONNECTED":
                    procConnected();
                    break;
                case "ERROR":
                    procError(content);
                    break;
                case  "DIRCONTENT":
                    procDirContent(content);
                    break;
                case "FILECONTENT":
                    procFileContent(content);
                    break;
                case "DISCONNECTED":
                    procDisconnected();
                    return false;
            }

            return true;
        }

        private void procConnected()
        {
            Console.WriteLine("Connected to server....");
        }

        private void procError(string content)
        {
            Console.WriteLine("Error: {0}", content);
        }

        private void procDirContent(string content)
        {
            string[] files = content.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string file in files)
                Console.WriteLine(file);
        }

        private void procFileContent(string length)
        {
            int len = int.Parse(length);
            char[] buf = new char[len];
            m_sr.ReadBlock(buf, 0, len);
            Console.WriteLine(new string(buf));
        }

        private void procDisconnected()
        {
            Console.WriteLine("Disconnected from server...");
        }
    }
}
