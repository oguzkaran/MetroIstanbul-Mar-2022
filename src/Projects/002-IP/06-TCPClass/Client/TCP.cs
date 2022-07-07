/********************************************************************************
Author          : Oğuz KARAN
Last Update     : 13.08.2018
Platform        : .NET Framework 2.0 and mono in POSIX Systems

Copyleft C and System Programmers Association
TCP class that simplifies the most used operations in client/server programming
paradigm (This class will be improved as soon as possible)
*********************************************************************************/
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace CSD
{	
    public class TCP
    {        
        public static Socket InitServer(int port, int backlog)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            sock.Bind(new IPEndPoint(IPAddress.Any, port));
            sock.Listen(backlog);

            return sock;
        }
        public static void CloseSafely(Socket sock)
        {
            if (sock == null)
                return;

            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
        public static Socket ConnectToServer(string IPstr, int port)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(new IPEndPoint(IPAddress.Parse(IPstr), port));

            return sock;
        }
        public static Socket ConnectToServer(IPAddress ip, int port)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(new IPEndPoint(ip, port));

            return sock;
        }
        public static Socket ConnectToServer(IPEndPoint ipep)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(ipep);
            return sock;
        }
        public static bool IsRunning()
        {
			bool result = false;
			
            try
            {
                Mutex.OpenExisting("TestMutex");
                result = true;
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                new Mutex(true, "TestMutex");                
            }
			
			return result;
        }
        public static Thread CreateThread(ThreadStart threadStart, bool bIsBackground, bool bStart)
        {
            Thread result = new Thread(threadStart);

            result.IsBackground = bIsBackground;

            if (bStart)
                result.Start();

            return result;
        }
        public static Thread CreateThread(ParameterizedThreadStart threadStart, bool bIsBackground, bool bStart, object o)
        {
            Thread result = new Thread(threadStart);

            result.IsBackground = bIsBackground;

            if (bStart)
                result.Start(o);

            return result;
        }
        public static int SendVarData(Socket s, byte[] data)
        {
            int result = 0;
            int size = data.Length;
            int left = size;
            int ret;
            byte[] dataSize = new byte[4];

            dataSize = BitConverter.GetBytes(size);
            ret = SendFixedData(s, dataSize);

            while (result < size)
            {
                ret = s.Send(data, result, left, SocketFlags.None);
                result += ret;
                left -= ret;
            }

            return result;
        }
        public static int SendFixedData(Socket s, byte[] data, int dataLength)
        {
            int result = 0;
            int size = dataLength;
            int left = size;
            int ret;

            while (result < size)
            {
                ret = s.Send(data, result, left, SocketFlags.None);
                result += ret;
                left -= ret;
            }

            return result;
        }
        public static int SendFixedData(Socket s, byte[] data)
        {
            return SendFixedData(s, data, data.Length);
        }
        public static byte[] ReceiveVarData(Socket s)
        {
            int result = 0;
            int ret;
            byte[] dataSize = new byte[4];
            if (ReceiveFixedData(s, dataSize, 4) == false)
                throw new SocketException();

            int size = BitConverter.ToInt32(dataSize, 0);
            int left = size;
            byte[] data = new byte[size];

            while (result < size)
            {
                ret = s.Receive(data, result, left, 0);
                if (ret == 0)
                    throw new SocketException();

                result += ret;
                left -= ret;
            }

            return data;
        }
        public static bool ReceiveVarData(Socket s, byte[] data)
        {
            int result = 0;
            int ret;
            byte[] dataSize = new byte[4];
            if (ReceiveFixedData(s, dataSize, 4) == false)
                return false;

            int size = BitConverter.ToInt32(dataSize, 0);
            int left = size;
            data = new byte[size];

            while (result < size)
            {
                ret = s.Receive(data, result, left, 0);
                if (ret == 0)
                    throw new SocketException();

                result += ret;
                left -= ret;
            }

            return true;
        }
        public static byte[] ReceiveFixedData(Socket s, int size)
        {
            int result = 0;
            int left = size;
            byte[] data = new byte[size];
            int ret;

            while (result < size)
            {
                ret = s.Receive(data, result, left, 0);
                if (ret == 0)
                    throw new SocketException();

                result += ret;
                left -= ret;
            }

            return data;
        }
        public static bool ReceiveFixedData(Socket s, byte[] data, int size)
        {
            int result = 0;
            int left = size;
            int ret;

            while (result < size)
            {
                ret = s.Receive(data, result, left, 0);
                if (ret == 0)
                    return false;

                result += ret;
                left -= ret;
            }

            return true;
        }
		public static void SendFile(Socket socket, string fileName, int chunkSize)
        {          
            FileStream fs = null;

            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                long chunkNumber = fs.Length / chunkSize;

                if (fs.Length % chunkSize != 0)
                    chunkNumber++;

                TCP.SendVarData(socket, BitConverter.GetBytes(chunkNumber)); 

                byte[] dataReal = null, data = new byte[chunkSize];
                int read = 0;

                while ((read = fs.Read(data, 0, chunkSize)) > 0)
                {
                    dataReal = new byte[read];
                    for (int i = 0; i < read; ++i)
                        dataReal[i] = data[i];

                    TCP.SendVarData(socket, dataReal);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public static void ReceiveFile(Socket socket, string fileName)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                long chunkNumber = BitConverter.ToInt64(TCP.ReceiveVarData(socket), 0);

                byte[] data = null;

                for (int i = 0; i < chunkNumber; i++)
                {
                    data = TCP.ReceiveVarData(socket);
                    fs.Write(data, 0, data.Length);
                }               
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
    }
}
