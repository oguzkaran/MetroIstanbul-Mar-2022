namespace Client
{
    partial class ConnectDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_labelHost = new System.Windows.Forms.Label();
            this.m_textBoxHost = new System.Windows.Forms.TextBox();
            this.m_labelPort = new System.Windows.Forms.Label();
            this.m_textBoxPort = new System.Windows.Forms.TextBox();
            this.m_textBoxNick = new System.Windows.Forms.TextBox();
            this.m_labelNick = new System.Windows.Forms.Label();
            this.m_buttonConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_labelHost
            // 
            this.m_labelHost.AutoSize = true;
            this.m_labelHost.Location = new System.Drawing.Point(4, 14);
            this.m_labelHost.Name = "m_labelHost";
            this.m_labelHost.Size = new System.Drawing.Size(29, 13);
            this.m_labelHost.TabIndex = 0;
            this.m_labelHost.Text = "Host";
            // 
            // m_textBoxHost
            // 
            this.m_textBoxHost.Location = new System.Drawing.Point(57, 11);
            this.m_textBoxHost.Name = "m_textBoxHost";
            this.m_textBoxHost.Size = new System.Drawing.Size(351, 20);
            this.m_textBoxHost.TabIndex = 0;
            // 
            // m_labelPort
            // 
            this.m_labelPort.AutoSize = true;
            this.m_labelPort.Location = new System.Drawing.Point(5, 43);
            this.m_labelPort.Name = "m_labelPort";
            this.m_labelPort.Size = new System.Drawing.Size(26, 13);
            this.m_labelPort.TabIndex = 2;
            this.m_labelPort.Text = "Port";
            // 
            // m_textBoxPort
            // 
            this.m_textBoxPort.Location = new System.Drawing.Point(58, 43);
            this.m_textBoxPort.Name = "m_textBoxPort";
            this.m_textBoxPort.Size = new System.Drawing.Size(106, 20);
            this.m_textBoxPort.TabIndex = 1;
            // 
            // m_textBoxNick
            // 
            this.m_textBoxNick.Location = new System.Drawing.Point(57, 80);
            this.m_textBoxNick.Name = "m_textBoxNick";
            this.m_textBoxNick.Size = new System.Drawing.Size(351, 20);
            this.m_textBoxNick.TabIndex = 2;
            // 
            // m_labelNick
            // 
            this.m_labelNick.AutoSize = true;
            this.m_labelNick.Location = new System.Drawing.Point(5, 81);
            this.m_labelNick.Name = "m_labelNick";
            this.m_labelNick.Size = new System.Drawing.Size(29, 13);
            this.m_labelNick.TabIndex = 0;
            this.m_labelNick.Text = "Nick";
            // 
            // m_buttonConnect
            // 
            this.m_buttonConnect.Location = new System.Drawing.Point(333, 121);
            this.m_buttonConnect.Name = "m_buttonConnect";
            this.m_buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.m_buttonConnect.TabIndex = 3;
            this.m_buttonConnect.Text = "Connect";
            this.m_buttonConnect.UseVisualStyleBackColor = true;
            this.m_buttonConnect.Click += new System.EventHandler(this.m_buttonConnect_Click);
            // 
            // ConnectDialogForm
            // 
            this.AcceptButton = this.m_buttonConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 155);
            this.Controls.Add(this.m_buttonConnect);
            this.Controls.Add(this.m_textBoxPort);
            this.Controls.Add(this.m_labelPort);
            this.Controls.Add(this.m_textBoxNick);
            this.Controls.Add(this.m_textBoxHost);
            this.Controls.Add(this.m_labelNick);
            this.Controls.Add(this.m_labelHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ConnectDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConnectDialogForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_labelHost;
        private System.Windows.Forms.TextBox m_textBoxHost;
        private System.Windows.Forms.Label m_labelPort;
        private System.Windows.Forms.TextBox m_textBoxPort;
        private System.Windows.Forms.TextBox m_textBoxNick;
        private System.Windows.Forms.Label m_labelNick;
        private System.Windows.Forms.Button m_buttonConnect;
    }
}