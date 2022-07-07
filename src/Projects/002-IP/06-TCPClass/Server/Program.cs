using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using CSD;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket server = null;
            Socket clientSocket = null;

            try
            {
                server = TCP.InitServer(5050, 8);                

                while (true)
                {
                    clientSocket = server.Accept();

                    while (true)
                    {
                        byte[] buf = TCP.ReceiveVarData(clientSocket);

                        string msg = Encoding.UTF8.GetString(buf);

                        Console.WriteLine(msg);

                        if (msg == "exit")
                            break;
                    }
                    TCP.CloseSafely(clientSocket);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                TCP.CloseSafely(server);
            }
        }
    }
}
