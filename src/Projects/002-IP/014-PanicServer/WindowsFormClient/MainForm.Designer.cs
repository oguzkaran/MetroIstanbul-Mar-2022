namespace WindowsFormClient
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_buttonPanic = new System.Windows.Forms.Button();
            this.m_timerStatus = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // m_buttonPanic
            // 
            this.m_buttonPanic.Location = new System.Drawing.Point(86, 48);
            this.m_buttonPanic.Name = "m_buttonPanic";
            this.m_buttonPanic.Size = new System.Drawing.Size(75, 23);
            this.m_buttonPanic.TabIndex = 0;
            this.m_buttonPanic.Text = "Panic Button";
            this.m_buttonPanic.UseVisualStyleBackColor = true;
            this.m_buttonPanic.Click += new System.EventHandler(this.m_buttonPanic_Click);
            // 
            // m_timerStatus
            // 
            this.m_timerStatus.Enabled = true;
            this.m_timerStatus.Interval = 500;
            this.m_timerStatus.Tick += new System.EventHandler(this.m_timerStatus_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 132);
            this.Controls.Add(this.m_buttonPanic);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_buttonPanic;
        private System.Windows.Forms.Timer m_timerStatus;
    }
}

