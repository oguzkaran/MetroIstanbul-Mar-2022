using CSD.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using CSD.Util;

namespace WindowsFormClient
{

    public partial class MainForm : Form
    {        
        private static Socket initTcpClient()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private string doWorkForImage(Socket socket)
        {
            FileStream fileStream = null;
            
            try
            {
                var networkStream = new NetworkStream(socket);
                var imagePath = m_textBoxLocation.Text;
                var filename = Path.GetFileName(imagePath);

                socket.SendVarData(Encoding.UTF8.GetBytes(filename));

                //Text = imagePath;

                fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                //fileStream.CopyTo(networkStream, 1024);

                fileStream.Copy(networkStream, 1024);
                fileStream.Flush();          

                fileStream.Close();
                var grayScalePath = Path.GetFileNameWithoutExtension(imagePath) + "-gs" + Path.GetExtension(imagePath);
                socket.ReceiveFile(grayScalePath);

                return grayScalePath;
            }
            finally {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        private string makeGrayScale()
        {
            Socket socket = null;

            try
            {
                socket = initTcpClient();

                socket.Connect("192.168.1.167", 50500);
                return doWorkForImage(socket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                if (socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }

            return "";
        }

        private async void makeGrayScaleAsync()
        {
            var task = new Task<string>(makeGrayScale);

            task.Start();

            var path = await task;

            m_buttonMakeGrayscale.Enabled = true;

            if (path != "")            
                m_pictureBoxGrayScale.Image = Image.FromFile(path);               
            
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void m_buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            m_textBoxLocation.Text = openFileDialog.FileName;

            m_pictureBoxOriginal.Image = Image.FromFile(m_textBoxLocation.Text);
            this.Text = Path.GetFileName(m_textBoxLocation.Text);
        }

        private void m_buttonMakeGrayscale_Click(object sender, EventArgs e)
        {
            if (m_pictureBoxOriginal.Image == null)
                return;

            if (!File.Exists(m_textBoxLocation.Text)) {
                MessageBox.Show("Image location is not valid");
                return;
            }

            if (m_pictureBoxGrayScale.Image != null)
                m_pictureBoxGrayScale.Image.Dispose();

            m_pictureBoxGrayScale.Image = null;

            m_buttonMakeGrayscale.Enabled = false;

            makeGrayScaleAsync();
        }
    }
}
