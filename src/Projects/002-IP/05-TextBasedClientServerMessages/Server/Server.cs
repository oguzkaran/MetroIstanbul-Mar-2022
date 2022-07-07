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
        private string m_nick;

        private void clientHandler(object obj)
        {
            Socket s = (Socket)obj;
            StreamWriter sw = null;
            StreamReader sr = null;

            try
            {                
                Console.WriteLine("Connected: {0}", s.RemoteEndPoint.ToString());

                NetworkStream ns = new NetworkStream(s);
                sw = new StreamWriter(ns);
                sr = new StreamReader(ns);

                string request;

                while (true)
                {
                    request = sr.ReadLine();
                    if (!parseRequest(request, sr, sw))
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
                sw.Close();
                sr.Close();
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
                Socket clientSocket = m_serverSock.Accept();
                ThreadPool.QueueUserWorkItem(clientHandler, clientSocket);
            }            
        }

        private bool parseRequest(string request, StreamReader sr, StreamWriter sw)
        {
            int index;
            string cmd, content;

            request = request.TrimStart();

            if ((index = request.IndexOf(' ')) == -1)
            {
                cmd = request;
                content = null;
            }
            else
            {
                cmd = request.Substring(0, index);
                content = request.Substring(index + 1).Trim();
            }

            switch (cmd)
            {
                case "CONNECT":
                    procConnect(content, sw);
                    break;
                case "GETDIRCONTENT":
                    procGetDirContent(content, sw);
                    break;
                case "GETFILECONTENT":
                    procGetFileContent(content, sw);
                    break;
                case "DISCONNECT":
                    procDisconnect(sw);
                    return false;
                default:
                    procError(string.Format("Command not found: {0}", cmd), sw);
                    break;
            }

            return true;
        }

        private void procConnect(string nick, StreamWriter sw)
        {
            m_nick = nick;
            Console.WriteLine("'{0}' user connected requested...", nick);
            sw.WriteLine("CONNECTED");
            sw.Flush();
        }

        private void procGetDirContent(string path, StreamWriter sw)
        {
            Console.WriteLine("'{0}' GETDIRCONTENT requested", m_nick);

            try
            {
                StringBuilder sb = new StringBuilder();
                string[] files = Directory.GetFiles(path);
                bool flag = false;

                foreach (string file in files)
                {
                    if (!flag)
                        flag = true;
                    else
                        sb.Append(",");
                    sb.Append(Path.GetFileName(file));
                }
                sw.WriteLine("DIRCONTENT {0}", sb.ToString());
                sw.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                procError(String.Format("'{0}' directory not found or cannt read", path), sw);
            }
        }

        private void procGetFileContent(string path, StreamWriter sw)
        {
            try
            {
                string text = File.ReadAllText(path);
                                
                sw.WriteLine("FILECONTENT {0}", text.Length);
                sw.Write(text.ToCharArray());
                sw.Flush();
            }
            catch (Exception e)
            {
                procError(String.Format("'{0}' file not found or cannt read", path), sw);
            }
        }

        private void procDisconnect(StreamWriter sw)
        {
            Console.WriteLine("{0} client disconnected", m_nick);
            sw.WriteLine("DISCONNECTED");
            sw.Flush();
        }

        private void procError(string msg, StreamWriter sw)
        {
            sw.WriteLine("ERROR {0}", msg);
            sw.Flush();
        }
    }
} 
