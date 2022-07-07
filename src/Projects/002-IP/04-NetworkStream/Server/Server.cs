using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

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
        private int m_port;

        private void clientHandler(object obj)
        {
            Socket s = null;

            try
            {
                s = (Socket)obj;

                Console.WriteLine("Connected: {0}", s.RemoteEndPoint.ToString());

                NetworkStream ns = new NetworkStream(s);
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                string str;

                while ((str = sr.ReadLine()) != null)
                {
                    if (str == "exit")
                        break;

                    Console.WriteLine(str);
                    sw.WriteLine(str.ToUpper());
                    sw.Flush();
                }

                sr.Close();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                if (s != null)
                {
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }                    
            }
        }

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
                Socket s = m_serverSock.Accept();

                ThreadPool.QueueUserWorkItem(clientHandler, s);               
            }
        }
    }
} 
