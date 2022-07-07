using CSD.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using static CSD.Net.UdpUtil;
using static CSD.Net.TcpUtil;

namespace WindowsFormClient
{
    public partial class MainForm : Form
    {
        private Socket m_udpSocket;

        public MainForm()
        {
            InitializeComponent();
        }

        private void handleWaitCallback(object o)
        {
            Socket socket = null;
            try
            {
                socket = ConnectToServer(IPAddress.Parse("192.168.1.167"), 46000);

                socket.SendString("HELP!!!!!!!");
                MessageBox.Show(socket.ReceiveString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally {
                if (socket != null)
                    socket.CloseSocket();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            m_udpSocket = InitUdpClient();
        }

        private void m_timerStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                Text = "Timer started";
                byte[] data = Encoding.UTF8.GetBytes("OK!!");
                m_udpSocket.SendTo(data, new IPEndPoint(IPAddress.Parse("192.168.1.255"), 45000));                
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_buttonPanic_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(handleWaitCallback));
        }
    }
}
