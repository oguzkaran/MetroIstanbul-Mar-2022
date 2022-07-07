using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Client
{
    public partial class MainForm : Form
    {
        private Socket m_clientSock;
        private int m_port;
        private string m_nick;

        private byte[] m_bufRecv;
        private int m_indexRecv;
        private int m_leftRecv;

        private byte[] m_bufSend;
        private int m_indexSend;
        private int m_leftSend;
        private bool m_lengthFlagRecv;

        private byte[] m_bufImage;

        private int m_clientResolX;
        private int m_clientResolY;

        private const int MOUSEEVENTF_LEFTDOWN = 0x02; 
        private const int MOUSEEVENTF_LEFTUP = 0x04; 
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08; 
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public MainForm()
        {
            InitializeComponent();

            m_mainPanel.Visible = false;
            m_bufSend = new byte[1024];
            m_bufRecv = new byte[4 * 1024 * 1024];
            m_bufImage = new byte[4 * 1024 * 1024];

            m_mainToolBarDisconnectItem.Enabled = false;
            m_connectionDisconnectItem.Enabled = false;
        } 

        private void m_connectionConnectItem_Click(object sender, EventArgs e)
        {
            ConnectDialogForm cdf = new ConnectDialogForm();
            cdf.Port = "5050";
            cdf.Host = "127.0.0.1";

            if (cdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_port = int.Parse(cdf.Port);
                m_nick = cdf.Nick;

                try
                {
                    m_clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPHostEntry hostEntry = Dns.GetHostEntry(cdf.Host);
                    IPAddress ipv4 = IPAddress.None;

                    foreach (IPAddress ipaddr in hostEntry.AddressList)
                        if (ipaddr.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipv4 = ipaddr;
                            break;
                        }
                    m_clientSock.BeginConnect(new IPEndPoint(ipv4, m_port), new AsyncCallback(connectProc), null);
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    return;
                }
            }
        }

        private void receiveProc(IAsyncResult iar)
        {
            int n = 0;

            try
            {
                n = m_clientSock.EndReceive(iar);


                m_indexRecv += n;
                m_leftRecv -= n;

                if (m_leftRecv == 0)
                {
                    if (m_lengthFlagRecv)
                    {
                        m_leftRecv = BitConverter.ToInt32(m_bufRecv, 0);
                        m_indexRecv = 0;
                        m_lengthFlagRecv = false;
                    }
                    else
                    {
                        byte[] msg = new byte[m_bufRecv.Length];
                        Array.Copy(m_bufRecv, msg, m_bufRecv.Length);
                        msgProc(msg);
                        m_lengthFlagRecv = true;
                        m_indexRecv = 0;
                        m_leftRecv = 4;
                    }
                }
                
                m_clientSock.BeginReceive(m_bufRecv, m_indexRecv, m_leftRecv, SocketFlags.None, new AsyncCallback(receiveProc), null);
            }
            catch 
            {
                if (m_clientSock != null)
                {
                    m_clientSock.Close();
                    m_clientSock = null;
                }
                return;
            }
        }

        private void msgProc(byte[] msg)
        {
            parseResponse(msg);
        }

        private bool parseResponse(byte[] msg)
        {
            ServerToClientCommandCode cmd = (ServerToClientCommandCode)BitConverter.ToInt32(msg, 0);
            
            switch (cmd)
            {
                case ServerToClientCommandCode.Connected:
                    procConnected(msg);
                    break;
                case ServerToClientCommandCode.DesktopImage:
                    procDesktopImage(msg);
                    break;
                case ServerToClientCommandCode.Disconnected:
                    procDisconnected();
                    break;
            }

            return true;
        }

        private void procConnected(byte[] msg)
        {
            m_clientResolX = BitConverter.ToInt32(msg, 4);
            m_clientResolY = BitConverter.ToInt32(msg, 8);

            this.Text = string.Format("x = {0}, Y = {1}", m_clientResolX, m_clientResolY);

            if (m_statusBar.InvokeRequired)
                m_statusBar.Invoke((MethodInvoker)delegate
                {
                    m_statusPane1.Text = "Connected";
                });
            else
                m_statusPane1.Text = "Connected";

            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = m_nick;
                });
            else
                this.Text = m_nick;
            
            if (m_mainToolBar.InvokeRequired)
                m_mainToolBar.Invoke((MethodInvoker)delegate
                {
                    m_mainToolBarConnectItem.Enabled = false;
                    m_mainToolBarDisconnectItem.Enabled = true;
                });
            else
            {
                m_mainToolBarConnectItem.Enabled = false;
                m_mainToolBarDisconnectItem.Enabled = true;
            }

            if (m_mainMenu.InvokeRequired)
                m_mainMenu.Invoke((MethodInvoker)delegate
                {
                    m_connectionConnectItem.Enabled = false;
                    m_connectionDisconnectItem.Enabled = true;
                });
            else
            {
                m_connectionConnectItem.Enabled = false;
                m_connectionDisconnectItem.Enabled = true;
            }
        }

        private void procDisconnected()
        {
            m_clientSock.Shutdown(SocketShutdown.Both);
            m_clientSock.Close();
            m_clientSock = null;

            if (m_statusBar.InvokeRequired)
                m_statusBar.Invoke((MethodInvoker)delegate
                {
                    m_statusPane1.Text = "Ready";
                });
            else
                m_statusPane1.Text = "Ready";

            if (m_mainPanel.InvokeRequired)
                m_mainPanel.Invoke((MethodInvoker)delegate
                {
                    m_mainPanel.Visible = false;
                });
            else
            {
                m_mainPanel.Visible = false;
            }

            if (m_mainToolBar.InvokeRequired)
                m_mainToolBar.Invoke((MethodInvoker)delegate
                {
                    m_mainToolBarConnectItem.Enabled = true;
                    m_mainToolBarDisconnectItem.Enabled = false;
                });
            else
            {
                m_mainToolBarConnectItem.Enabled = true;
                m_mainToolBarDisconnectItem.Enabled = false;
            }

            if (m_mainMenu.InvokeRequired)
                m_mainMenu.Invoke((MethodInvoker)delegate
                {
                    m_connectionConnectItem.Enabled = true;
                    m_connectionDisconnectItem.Enabled = false;
                });
            else
            {
                m_connectionConnectItem.Enabled = true;
                m_connectionDisconnectItem.Enabled = false;
            }
        }

        private void connectProc(IAsyncResult iar)
        {
            try
            {
                m_clientSock.EndConnect(iar);
                      
                if (m_mainPanel.InvokeRequired)
                    m_mainPanel.Invoke((MethodInvoker)delegate
                    {
                        m_mainPanel.Visible = true;
                    });
                else
                    m_mainPanel.Visible = true;

                m_indexRecv = 0;
                m_leftRecv = 4;
                m_lengthFlagRecv = true;
                m_clientSock.BeginReceive(m_bufRecv, m_indexRecv, m_leftRecv, SocketFlags.None, new AsyncCallback(receiveProc), null);

                byte[] buf2 = BitConverter.GetBytes((int)ClientToServerCommandCode.Connect);
                byte[] buf4 = Encoding.UTF8.GetBytes(m_nick);
                byte[] buf3 = BitConverter.GetBytes(buf4.Length);
                byte[] buf1 = BitConverter.GetBytes(buf2.Length + buf3.Length + buf4.Length);
                
                byte[] msg = new byte[buf1.Length + buf2.Length + buf3.Length + buf4.Length];
                Array.Copy(buf1, 0, msg, 0, buf1.Length);
                Array.Copy(buf2, 0, msg, 4, buf2.Length);
                Array.Copy(buf3, 0, msg, 8, buf3.Length);
                Array.Copy(buf4, 0, msg, 12, buf4.Length);

                sendMsgToServer(msg);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void procDesktopImage(byte[] msg)
        {
            int len = BitConverter.ToInt32(msg, 4);
            MemoryStream ms = new MemoryStream(msg, 8, len);
            Bitmap bmp = new Bitmap(ms);

            if (m_pictureBox.InvokeRequired)
                m_pictureBox.Invoke((MethodInvoker)delegate
                {
                    m_pictureBox.Image = bmp;
                });
            else
            {
                m_pictureBox.Image = bmp;
            }
        }

        private void sendMsgToServer(byte[] msg)
        {
            m_indexSend = 0;
            m_leftSend = msg.Length;
            m_bufSend = msg;

            try
            {
                m_clientSock.BeginSend(m_bufSend, m_indexSend, m_leftSend, SocketFlags.None, new AsyncCallback(sendProc), null);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception oluştu:" + e.Message);
            }
        }

        private static int stringToByteMsg(string str, byte[] buf)
        {
            int textLen;

            textLen = Encoding.UTF8.GetBytes(str, 0, str.Length, buf, 4);
            byte[] bufLen = BitConverter.GetBytes(textLen);
            Array.Copy(bufLen, buf, 4);

            return textLen + 4;
        }

        private void sendProc(IAsyncResult iar)
        {
            try
            {
                int n = m_clientSock.EndSend(iar);

                m_indexSend += n;
                m_leftSend -= n;

                if (m_leftSend != 0)
                    m_clientSock.BeginSend(m_bufSend, m_indexSend, m_leftSend, SocketFlags.None, new AsyncCallback(sendProc), null);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception oluştu:" + e.Message);
            }
        }

        private void m_connectionDisconnectItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bağlantıyı koparmak istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dr == System.Windows.Forms.DialogResult.Yes)
                sendDisconnectMsg();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_clientSock == null)
                return;

            DialogResult dr = MessageBox.Show("Bağlantıyı koparmak istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (dr == System.Windows.Forms.DialogResult.OK)
                sendDisconnectMsg();
            else
                e.Cancel = true;
        }

        private void sendDisconnectMsg()
        {
            byte[] buf2 = BitConverter.GetBytes((int)ClientToServerCommandCode.Disconnect);
            byte[] buf1 = BitConverter.GetBytes(buf2.Length);

            byte[] msg = new byte[buf1.Length + buf2.Length];
            Array.Copy(buf1, 0, msg, 0, buf1.Length);
            Array.Copy(buf2, 0, msg, 4, buf2.Length);

            sendMsgToServer(msg);
        }

        private void m_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = convertMousePos(e.Location);
            
            byte[] buf2 = BitConverter.GetBytes((int)ClientToServerCommandCode.MouseMove);
            byte[] buf3 = BitConverter.GetBytes(pt.X);
            byte[] buf4 = BitConverter.GetBytes(pt.Y);
            byte[] buf1 = BitConverter.GetBytes(buf2.Length + buf3.Length + buf4.Length);

            byte[] msg = new byte[buf1.Length + buf2.Length + buf3.Length + buf4.Length];
            Array.Copy(buf1, 0, msg, 0, buf1.Length);
            Array.Copy(buf2, 0, msg, 4, buf2.Length);
            Array.Copy(buf3, 0, msg, 8, buf3.Length);
            Array.Copy(buf4, 0, msg, 12, buf4.Length);

            sendMsgToServer(msg);
        }

        private Point convertMousePos(Point pt)
        {
            return new Point((int)Math.Round((double)m_clientResolX / m_pictureBox.ClientSize.Width * pt.X), (int)Math.Round((double)m_clientResolY / m_pictureBox.ClientSize.Height * pt.Y));
        }

        private void m_pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                sendMouseOp(MOUSEEVENTF_LEFTDOWN, 0);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                sendMouseOp(MOUSEEVENTF_RIGHTDOWN, 0);
        }

        private void m_pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                sendMouseOp(MOUSEEVENTF_LEFTUP, 0);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                sendMouseOp(MOUSEEVENTF_RIGHTUP, 0);
        }

        private void sendMouseOp(int op, int button)
        {
            byte[] buf2 = BitConverter.GetBytes((int)ClientToServerCommandCode.MouseButtonOp);
            byte[] buf3 = BitConverter.GetBytes(op);
            byte[] buf4 = BitConverter.GetBytes(button);
            byte[] buf1 = BitConverter.GetBytes(buf2.Length + buf3.Length + buf4.Length);

            byte[] msg = new byte[buf1.Length + buf2.Length + buf3.Length + buf4.Length];
            Array.Copy(buf1, 0, msg, 0, buf1.Length);
            Array.Copy(buf2, 0, msg, 4, buf2.Length);
            Array.Copy(buf3, 0, msg, 8, buf3.Length);
            Array.Copy(buf4, 0, msg, 12, buf4.Length);

            sendMsgToServer(msg);
        }
    }

    enum ClientToServerCommandCode
    {
        Connect, Disconnect, MouseMove, MouseButtonOp
    }

    enum ServerToClientCommandCode
    {
        Connected, Disconnected, DesktopImage
    }
}
