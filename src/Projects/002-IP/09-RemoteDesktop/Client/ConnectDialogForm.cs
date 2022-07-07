using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class ConnectDialogForm : Form
    {
        public ConnectDialogForm()
        {
            InitializeComponent();
        }

        private void m_buttonConnect_Click(object sender, EventArgs e)
        {
            if (m_textBoxHost.Text.Trim() == "")
            {
                MessageBox.Show("Host name must be specified!", "Errro");
                m_textBoxHost.Focus();
                return;
            }

            if (m_textBoxPort.Text.Trim() == "")
            {
                MessageBox.Show("Port number must be specified!", "Errro");
                m_textBoxPort.Focus();
                return;
            }

            int port;

            if (!int.TryParse(m_textBoxPort.Text, out port))
            {
                MessageBox.Show("Incorrcet oort number!", "Errro");
                m_textBoxPort.Focus();
                return;
            }

            if (m_textBoxNick.Text.Trim() == "")
            {
                MessageBox.Show("Nickname must be specified!", "Errro");
                m_textBoxNick.Focus();
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }

        public string Host
        {
            get
            {
                return m_textBoxHost.Text.Trim();
            }

            set
            {
                m_textBoxHost.Text = value;
            }
        }

        public string Port
        {
            get
            {
                return m_textBoxPort.Text.Trim();
            }

            set
            {
                m_textBoxPort.Text = value;
            }
        }

        public string Nick
        {
            get
            {
                return m_textBoxNick.Text.Trim();
            }

            set
            {
                m_textBoxNick.Text = value;
            }
        }
    }
}
