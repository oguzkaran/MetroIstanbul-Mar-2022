using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using CSD.Net;
using System.IO;
using System.Drawing;

namespace CSD
{
    public class Server
    {        
        private Socket m_serverSocket;
        private const string ms_imageDirectory = "images";

        private Bitmap makeGrayScale(string imagePath) //Daha etkin yapılabilir. Sadece örnek amaçlı yapıldı
        {
            var bitmap = new Bitmap(imagePath);
            var width = bitmap.Width;
            var height = bitmap.Height;

            for (int i = 0; i < width; ++i) {
                for (int j = 0; j < height; ++j) {
                    var Color = bitmap.GetPixel(i, j);
                    var avg = (int)Math.Floor((Color.R + Color.G + Color.B) / 3.0);

                    bitmap.SetPixel(i, j, Color.FromArgb(avg, avg, avg));
                }
            }

            return bitmap;            
        }

        private static string getDateTemplate(DateTime dateTime)
        {
            return $"{dateTime.Day}.{dateTime.Month}.{dateTime.Year} {dateTime.Hour} {dateTime.Minute} {dateTime.Second} {dateTime.Millisecond}";
        }

        private void clientProc(Socket socket)
        {
            try
            {
                var ipEndPoint = socket.RemoteEndPoint as IPEndPoint;
                var address = ipEndPoint.Address;
                Directory.CreateDirectory(address.ToString());
                var connectedTime = DateTime.Now;
                var imageName = Encoding.UTF8.GetString(socket.ReceiveVarData());                
                var imagePath = Path.Combine(address.ToString(), getDateTemplate(connectedTime) + imageName); 

                socket.ReceiveFile(imagePath);
                var bitmap = makeGrayScale(imagePath);
                var grayScaleImagePath = Path.Combine(address.ToString(), getDateTemplate(connectedTime) + Path.GetFileNameWithoutExtension(imageName) + "-gs" + Path.GetExtension(imageName)); 

                // Dikkat çok dosya birikmesine karşılık önlem alınmalı
                bitmap.Save(grayScaleImagePath);
                socket.SendFile(grayScaleImagePath, 1024);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
        }

        private async void doWorkForClient(Socket socket)
        {
            var task = new Task(() => clientProc(socket));

            task.Start();
            await task;
            Console.WriteLine("Client operation finished");
        }

        private static Socket initTcpServer(IPEndPoint endPoint, int backLog)
        {            
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // TCP/IP          
            
            socket.Bind(endPoint);
            socket.Listen(backLog);

            return socket;
        }

        

        public int Port { get; set; }

        public Server()
        {
            Directory.CreateDirectory(ms_imageDirectory);
        }

        public void Run()
        {
            try
            {
                m_serverSocket = initTcpServer(new IPEndPoint(IPAddress.Any, Port), 200);

                Console.WriteLine($"Waiting for a connection on port:{Port}");

                while (true)
                {
                    var clientSocket = m_serverSocket.Accept();

                    doWorkForClient(clientSocket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                if (m_serverSocket != null)
                    m_serverSocket.Close();
            }
        }
    }
}
