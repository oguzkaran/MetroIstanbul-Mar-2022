using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Server
{
    public partial class MainForm : Form
    {
        private List<ClientInfo> m_clients;
        private Socket m_serverSock;
        private int m_port;
        private System.Threading.Timer m_timer;
        private Bitmap m_bitmap;
        private Graphics m_bitmapGraphics;
        private volatile bool m_sentFlag;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02; 
        private const int MOUSEEVENTF_LEFTUP = 0x04; 
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08; 
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public MainForm()
        {
            InitializeComponent();

            m_clients = new List<ClientInfo>();
            m_port = 5050;
        }

        private void m_serverStartItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_serverSock.Bind(new IPEndPoint(IPAddress.Any, m_port));
                m_serverSock.Listen(8);

                m_statusPane1.Text = "Running";

                m_serverSock.BeginAccept(new AsyncCallback(acceptProc), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void acceptProc(IAsyncResult iar)
        {
            m_serverSock.BeginAccept(new AsyncCallback(acceptProc), null);

            Socket clientSock;
            clientSock = m_serverSock.EndAccept(iar);

            ClientInfo ci = new ClientInfo();
            ci.Sock = clientSock;

            //...
            lock (m_clients)
                m_clients.Add(ci);

            ci.LeftRecv = 4;
            ci.IndexRecv = 0;
            ci.LengthFlagRecv = true;

            clientSock.BeginReceive(ci.BufRecv, ci.IndexRecv, ci.LeftRecv, SocketFlags.None, new AsyncCallback(receiveProc), ci);
        }

        private void receiveProc(IAsyncResult iar)
        {
            ClientInfo ci = (ClientInfo)iar.AsyncState;
            int n = 0;

            try
            {
                n = ci.Sock.EndReceive(iar);

                ci.IndexRecv += n;
                ci.LeftRecv -= n;

                if (ci.LeftRecv == 0)
                {
                    if (ci.LengthFlagRecv)
                    {
                        ci.LeftRecv = BitConverter.ToInt32(ci.BufRecv, 0);
                        ci.IndexRecv = 0;
                        ci.LengthFlagRecv = false;
                    }
                    else
                    {
                        byte[] msg = new byte[ci.IndexRecv];
                        Array.Copy(ci.BufRecv, msg, ci.IndexRecv);
                        msgProc(ci, msg);
                        ci.LengthFlagRecv = true;
                        ci.IndexRecv = 0;
                        ci.LeftRecv = 4;
                    }
                }

                ci.Sock.BeginReceive(ci.BufRecv, ci.IndexRecv, ci.LeftRecv, SocketFlags.None, new AsyncCallback(receiveProc), ci);
            }
            catch 
            {
                disposeClient(ci);
            }
        }

        private void msgProc(ClientInfo ci, byte[] msg)
        {
            parseRequest(ci, msg);
        }

        private void parseRequest(ClientInfo ci, byte[] msg)
        {
            ClientToServerCommandCode cmd = (ClientToServerCommandCode) BitConverter.ToInt32(msg, 0);

            if (m_textBoxLog.InvokeRequired)
                m_textBoxLog.Invoke((MethodInvoker)delegate
                {
                    m_textBoxLog.SelectionStart = m_textBoxLog.TextLength;
                    m_textBoxLog.SelectedText = string.Format("{0}\r\n", cmd.ToString());
                });
            else
            {
                m_textBoxLog.SelectionStart = m_textBoxLog.TextLength;
                m_textBoxLog.SelectedText = string.Format("{0}\r\n", cmd.ToString());
            }

            switch (cmd)
            {
                case ClientToServerCommandCode.Connect:
                    procConnect(ci, msg);
                    break;
                case ClientToServerCommandCode.Disconnect:
                    procDisconnect(ci);
                    break;
                case ClientToServerCommandCode.MouseMove:
                    procMouseMove(msg);
                    break;
                case ClientToServerCommandCode.MouseButtonOp:
                    procMouseOp(msg);
                    break;
                default:
                    // procError(string.Format("Command not found: {0}", cmd));
                    break;
            }
        }

        private void procConnect(ClientInfo ci, byte[] msg)
        {
            IPEndPoint ep = (IPEndPoint)ci.Sock.LocalEndPoint;

            int nickLen = BitConverter.ToInt32(msg, 4);
            string nick = Encoding.UTF8.GetString(msg, 8, nickLen);

            if (m_listViewClients.InvokeRequired)
                m_listViewClients.Invoke((MethodInvoker)delegate
                {
                    m_listViewClients.Items.Add(new ListViewItem(new string[] { nick, ep.Address.ToString(), ep.Port.ToString() }));
                });
            else
                m_listViewClients.Items.Add(new ListViewItem(new string[] { nick, ep.Address.ToString(), ep.Port.ToString() }));

            ci.Nick = nick;

            byte[] buf2 = BitConverter.GetBytes((int)ServerToClientCommandCode.Connected);
            byte[] buf3 = BitConverter.GetBytes(Screen.PrimaryScreen.Bounds.Width);
            byte[] buf4 = BitConverter.GetBytes(Screen.PrimaryScreen.Bounds.Height);
            byte[] buf1 = BitConverter.GetBytes(buf2.Length + buf3.Length + buf4.Length);

            byte[] sendMsg = new byte[buf1.Length + buf2.Length + buf3.Length + buf4.Length];
            Array.Copy(buf1, 0, sendMsg, 0, buf1.Length);
            Array.Copy(buf2, 0, sendMsg, 4, buf2.Length);
            Array.Copy(buf3, 0, sendMsg, 8, buf3.Length);
            Array.Copy(buf4, 0, sendMsg, 12, buf4.Length);

            sendMsgToClient(ci, sendMsg);

            if (m_timer == null)
            {
                m_sentFlag = true;
                m_timer = new System.Threading.Timer(new TimerCallback(timerProc), null, 0, 150);
            }
        }

        private void procDisconnect(ClientInfo ci)
        {
            byte[] buf2 = BitConverter.GetBytes((int)ServerToClientCommandCode.Disconnected);
            byte[] buf1 = BitConverter.GetBytes(buf2.Length);

            byte[] sendMsg = new byte[buf1.Length + buf2.Length];
            Array.Copy(buf1, 0, sendMsg, 0, buf1.Length);
            Array.Copy(buf2, 0, sendMsg, 4, buf2.Length);

            sendMsgToClient(ci, sendMsg);

            disposeClient(ci);
        }

        private void procMouseMove(byte[] msg)
        {
            int x = BitConverter.ToInt32(msg, 4);
            int y = BitConverter.ToInt32(msg, 8);

            Cursor.Position = new Point(x, y);
        }

        private void procMouseOp(byte[] msg)
        {
            MessageBox.Show("Ok");
            int op = BitConverter.ToInt32(msg, 4);
            mouse_event(op, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        }
        
        private void disposeClient(ClientInfo ci)
        {
            if (m_listViewClients.InvokeRequired)
                m_listViewClients.Invoke((MethodInvoker)delegate
                {
                    foreach (ListViewItem lvi in m_listViewClients.Items)
                    {
                        if (lvi.Text == ci.Nick)
                        {
                            m_listViewClients.Items.Remove(lvi);
                            break;
                        }
                    }
                });
            else
            {
                foreach (ListViewItem lvi in m_listViewClients.Items)
                {
                    if (lvi.Text == ci.Nick)
                    {
                        m_listViewClients.Items.Remove(lvi);
                        break;
                    }
                }
            }

            ci.Sock.Close();      // Socket'te Close ile Dispse aynı işmei yapıyor

            lock (m_clients)
                m_clients.Remove(ci);
        }

        private void timerProc(object o)
        {
            if (!m_sentFlag)
                return;

            MemoryStream bitmapMemoryStream;
            m_bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            m_bitmapGraphics = Graphics.FromImage(m_bitmap);
            bitmapMemoryStream = new MemoryStream();
            m_bitmapGraphics.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            m_bitmap.Save(bitmapMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] buf2 = BitConverter.GetBytes((int)ServerToClientCommandCode.DesktopImage);
            byte[] buf4 = bitmapMemoryStream.ToArray();
            byte[] buf3 = BitConverter.GetBytes(buf4.Length);
            byte[] buf1 = BitConverter.GetBytes(buf2.Length + buf3.Length + buf4.Length);

            byte[] msg = new byte[buf1.Length + buf2.Length + buf3.Length + buf4.Length];
            Array.Copy(buf1, 0, msg, 0, buf1.Length);
            Array.Copy(buf2, 0, msg, 4, buf2.Length);
            Array.Copy(buf3, 0, msg, 8, buf3.Length);
            Array.Copy(buf4, 0, msg, 12, buf4.Length);

            m_sentFlag = false;
            foreach (ClientInfo ci in m_clients)
                sendBytesToClient(ci, msg);

            bitmapMemoryStream.Dispose();
            m_bitmap.Dispose();
         }

        private void sendMsgToClient(ClientInfo ci, byte[] msg)
        {
            ci.BufSend = msg;
            ci.IndexSend = 0;
            ci.LeftSend = msg.Length;

            try
            {
                ci.Sock.BeginSend(ci.BufSend, ci.IndexSend, ci.LeftSend, SocketFlags.None, new AsyncCallback(sendProc), ci);
            }
            catch 
            {
                disposeClient(ci);

                //MessageBox.Show("Exception oluştu: " + e.Message);
            }
        }

        private void sendBytesToClient(ClientInfo ci, byte[] buf)
        {
            ci.BufSend = buf;
            ci.IndexSend = 0;
            ci.LeftSend = buf.Length;

            try
            {
                ci.Sock.BeginSend(ci.BufSend, ci.IndexSend, ci.LeftSend, SocketFlags.None, new AsyncCallback(sendProc), ci);
            }
            catch 
            {
                disposeClient(ci);
                //MessageBox.Show("Exception oluştu: " + e.Message);
            }
        }

        private void sendProc(IAsyncResult iar)
        {
            ClientInfo ci = (ClientInfo)iar.AsyncState;

            int n = ci.Sock.EndSend(iar);

            ci.IndexSend += n;
            ci.LeftSend -= n;

            try
            {
                if (ci.LeftSend != 0)
                    ci.Sock.BeginSend(ci.BufSend, ci.IndexSend, ci.LeftSend, SocketFlags.None, new AsyncCallback(sendProc), ci);
                else
                    m_sentFlag = true;
            }
            catch 
            {
                disposeClient(ci);

                //MessageBox.Show("Exception oluştu: " + e.Message);
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
    }

    class ClientInfo
    {
        private Socket m_sock;
        private byte[] m_bufRecv, m_bufSend;
        private int m_indexRecv, m_indexSend;
        private int m_leftRecv, m_leftSend;
        private bool m_lengthFlagRecv;
        private string m_nick;

        public ClientInfo()
        {
            m_bufRecv = new byte[4 * 1024 * 1024];
        }

        public Socket Sock
        {
            get { return m_sock; }
            set { m_sock = value; }
        }

        public byte[] BufRecv
        {
            get { return m_bufRecv; }
            set { m_bufRecv = value; }
        }

        public byte[] BufSend
        {
            get { return m_bufSend; }
            set { m_bufSend = value; }
        }

        public int IndexRecv
        {
            get { return m_indexRecv; }
            set { m_indexRecv = value; }
        }

        public int IndexSend
        {
            get { return m_indexSend; }
            set { m_indexSend = value; }
        }

        public int LeftRecv
        {
            get { return m_leftRecv; }
            set { m_leftRecv = value; }
        }

        public int LeftSend
        {
            get { return m_leftSend; }
            set { m_leftSend = value; }
        }

        public bool LengthFlagRecv
        {
            get { return m_lengthFlagRecv; }
            set { m_lengthFlagRecv = value; }
        }

        public string Nick
        {
            get { return m_nick; }
            set { m_nick = value; }
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
