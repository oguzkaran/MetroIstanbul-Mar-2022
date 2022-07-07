using CSD.Net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static CSD.Net.TcpUtil;

namespace CSD
{
    public class Client
    {
        private Socket m_udpSocket;
        private Socket m_tcpSocket;
        private Timer m_tcpTimer;        

        private void handleTimer(object o)
        {
            try
            {                
                m_tcpSocket = ConnectToServer(TcpIPAddress, TcpPort);

                m_tcpSocket.SendString("HELP!!!!");
                Console.WriteLine(m_tcpSocket.ReceiveString());
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void initPanicTcpTimer()
        {
            m_tcpTimer = new Timer(handleTimer, null, 3000, 3000);
        }

        public IPAddress UdpIPAddress { get; set; }
        public IPAddress TcpIPAddress { get; set; }
        public int UdpPort { get; set; }
        public int TcpPort { get; set; }

        public void Run()
        {
            var random = new Random();

            try
            {
                initPanicTcpTimer();
                m_udpSocket = UdpUtil.InitUdpClient();

                while (true)
                {
                    var val = random.Next(256);                    

                    byte[] data = Encoding.UTF8.GetBytes($"Ok:{val}");

                    int result = m_udpSocket.SendTo(data, new IPEndPoint(UdpIPAddress, UdpPort));

                    if (result == 0)
                        break;

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

    }
}

