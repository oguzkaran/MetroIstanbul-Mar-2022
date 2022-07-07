using CSD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Socket s = TCP.ConnectToServer("127.0.0.1", 5050);

                while (true)
                {
                    Console.WriteLine("Text:");
                    string str = Console.ReadLine();

                    byte[] buf = Encoding.UTF8.GetBytes(str);

                    TCP.SendVarData(s, buf);

                    if (str == "exit")
                        break;
                }
                TCP.CloseSafely(s);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
        }
    }
}
