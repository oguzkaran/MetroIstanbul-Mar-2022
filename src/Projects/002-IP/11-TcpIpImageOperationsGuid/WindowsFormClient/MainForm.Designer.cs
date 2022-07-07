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
            this.m_pictureBoxGrayScale = new System.Windows.Forms.PictureBox();
            this.m_textBoxLocation = new System.Windows.Forms.TextBox();
            this.m_pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.m_buttonBrowse = new System.Windows.Forms.Button();
            this.m_buttonMakeGrayscale = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxGrayScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxOriginal)).BeginInit();
            this.SuspendLayout();
            // 
            // m_pictureBoxGrayScale
            // 
            this.m_pictureBoxGrayScale.Location = new System.Drawing.Point(581, 37);
            this.m_pictureBoxGrayScale.Name = "m_pictureBoxGrayScale";
            this.m_pictureBoxGrayScale.Size = new System.Drawing.Size(385, 325);
            this.m_pictureBoxGrayScale.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_pictureBoxGrayScale.TabIndex = 0;
            this.m_pictureBoxGrayScale.TabStop = false;
            // 
            // m_textBoxLocation
            // 
            this.m_textBoxLocation.Location = new System.Drawing.Point(57, 394);
            this.m_textBoxLocation.Name = "m_textBoxLocation";
            this.m_textBoxLocation.Size = new System.Drawing.Size(327, 23);
            this.m_textBoxLocation.TabIndex = 2;
            // 
            // m_pictureBoxOriginal
            // 
            this.m_pictureBoxOriginal.Location = new System.Drawing.Point(57, 37);
            this.m_pictureBoxOriginal.Name = "m_pictureBoxOriginal";
            this.m_pictureBoxOriginal.Size = new System.Drawing.Size(385, 325);
            this.m_pictureBoxOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_pictureBoxOriginal.TabIndex = 0;
            this.m_pictureBoxOriginal.TabStop = false;
            // 
            // m_buttonBrowse
            // 
            this.m_buttonBrowse.Location = new System.Drawing.Point(406, 394);
            this.m_buttonBrowse.Name = "m_buttonBrowse";
            this.m_buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.m_buttonBrowse.TabIndex = 3;
            this.m_buttonBrowse.Text = "Browse";
            this.m_buttonBrowse.UseVisualStyleBackColor = true;
            this.m_buttonBrowse.Click += new System.EventHandler(this.m_buttonBrowse_Click);
            // 
            // m_buttonMakeGrayscale
            // 
            this.m_buttonMakeGrayscale.Location = new System.Drawing.Point(684, 393);
            this.m_buttonMakeGrayscale.Name = "m_buttonMakeGrayscale";
            this.m_buttonMakeGrayscale.Size = new System.Drawing.Size(159, 23);
            this.m_buttonMakeGrayscale.TabIndex = 4;
            this.m_buttonMakeGrayscale.Text = "Make Grayscale";
            this.m_buttonMakeGrayscale.UseVisualStyleBackColor = true;
            this.m_buttonMakeGrayscale.Click += new System.EventHandler(this.m_buttonMakeGrayscale_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1059, 522);
            this.Controls.Add(this.m_buttonMakeGrayscale);
            this.Controls.Add(this.m_buttonBrowse);
            this.Controls.Add(this.m_textBoxLocation);
            this.Controls.Add(this.m_pictureBoxOriginal);
            this.Controls.Add(this.m_pictureBoxGrayScale);
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxGrayScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_pictureBoxOriginal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox m_pictureBoxGrayScale;
        private System.Windows.Forms.TextBox m_textBoxLocation;
        private System.Windows.Forms.PictureBox m_pictureBoxOriginal;
        private System.Windows.Forms.Button m_buttonBrowse;
        private System.Windows.Forms.Button m_buttonMakeGrayscale;
    }
}

