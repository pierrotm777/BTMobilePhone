namespace TestOBEXParser
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.listDevices = new System.Windows.Forms.ListBox();
            this.btnSearchDevices = new System.Windows.Forms.Button();
            this.txtAnswer = new System.Windows.Forms.TextBox();
            this.lstBrowser = new System.Windows.Forms.ListView();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(259, 57);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(105, 27);
            this.btnDisconnect.TabIndex = 11;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(126, 57);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(105, 27);
            this.btnConnect.TabIndex = 10;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // listDevices
            // 
            this.listDevices.FormattingEnabled = true;
            this.listDevices.Location = new System.Drawing.Point(126, 10);
            this.listDevices.Name = "listDevices";
            this.listDevices.Size = new System.Drawing.Size(364, 43);
            this.listDevices.TabIndex = 9;
            // 
            // btnSearchDevices
            // 
            this.btnSearchDevices.Location = new System.Drawing.Point(15, 26);
            this.btnSearchDevices.Name = "btnSearchDevices";
            this.btnSearchDevices.Size = new System.Drawing.Size(105, 27);
            this.btnSearchDevices.TabIndex = 8;
            this.btnSearchDevices.Text = "Search Devices";
            this.btnSearchDevices.UseVisualStyleBackColor = true;
            this.btnSearchDevices.Click += new System.EventHandler(this.btnSearchDevices_Click);
            // 
            // txtAnswer
            // 
            this.txtAnswer.Location = new System.Drawing.Point(12, 90);
            this.txtAnswer.Multiline = true;
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAnswer.Size = new System.Drawing.Size(478, 113);
            this.txtAnswer.TabIndex = 12;
            // 
            // lstBrowser
            // 
            this.lstBrowser.Location = new System.Drawing.Point(12, 209);
            this.lstBrowser.Name = "lstBrowser";
            this.lstBrowser.Size = new System.Drawing.Size(478, 190);
            this.lstBrowser.TabIndex = 13;
            this.lstBrowser.UseCompatibleStateImageBehavior = false;
            this.lstBrowser.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstBrowser_MouseDoubleClick);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(12, 405);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(66, 40);
            this.btnUp.TabIndex = 14;
            this.btnUp.Text = "up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(147, 405);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(84, 40);
            this.btnLoad.TabIndex = 15;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 480);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.lstBrowser);
            this.Controls.Add(this.txtAnswer);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.listDevices);
            this.Controls.Add(this.btnSearchDevices);
            this.Name = "Form1";
            this.Text = "Bluetooth OBEX Parser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox listDevices;
        private System.Windows.Forms.Button btnSearchDevices;
        private System.Windows.Forms.TextBox txtAnswer;
        private System.Windows.Forms.ListView lstBrowser;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnLoad;
    }
}

