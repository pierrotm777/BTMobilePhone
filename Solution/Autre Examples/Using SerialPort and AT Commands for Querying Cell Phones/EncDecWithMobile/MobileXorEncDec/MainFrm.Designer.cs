namespace MobileXorEncDec
{
    partial class MainFrm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.btnSelectFixFile = new System.Windows.Forms.Button();
            this.txtDistenation = new System.Windows.Forms.TextBox();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEncDec = new System.Windows.Forms.Button();
            this.btnRefreshports = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(464, 152);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 37);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "Save File Path";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.SaveFixFilePath_Click);
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(66, 49);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSource.Size = new System.Drawing.Size(392, 37);
            this.txtSource.TabIndex = 22;
            this.txtSource.TabStop = false;
            // 
            // btnSelectFixFile
            // 
            this.btnSelectFixFile.Location = new System.Drawing.Point(464, 49);
            this.btnSelectFixFile.Name = "btnSelectFixFile";
            this.btnSelectFixFile.Size = new System.Drawing.Size(75, 37);
            this.btnSelectFixFile.TabIndex = 23;
            this.btnSelectFixFile.Text = "Open File Path";
            this.btnSelectFixFile.UseVisualStyleBackColor = true;
            this.btnSelectFixFile.Click += new System.EventHandler(this.btnSelectFixFile_Click);
            // 
            // txtDistenation
            // 
            this.txtDistenation.Location = new System.Drawing.Point(66, 152);
            this.txtDistenation.Multiline = true;
            this.txtDistenation.Name = "txtDistenation";
            this.txtDistenation.ReadOnly = true;
            this.txtDistenation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDistenation.Size = new System.Drawing.Size(392, 37);
            this.txtDistenation.TabIndex = 25;
            this.txtDistenation.TabStop = false;
            // 
            // cmbPort
            // 
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(66, 111);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(392, 21);
            this.cmbPort.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(4, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Select Port:";
            // 
            // btnEncDec
            // 
            this.btnEncDec.Location = new System.Drawing.Point(66, 211);
            this.btnEncDec.Name = "btnEncDec";
            this.btnEncDec.Size = new System.Drawing.Size(473, 37);
            this.btnEncDec.TabIndex = 28;
            this.btnEncDec.Text = "Encrypt/Decrypt With Mobile";
            this.btnEncDec.UseVisualStyleBackColor = true;
            this.btnEncDec.Click += new System.EventHandler(this.btnEncDec_Click);
            // 
            // btnRefreshports
            // 
            this.btnRefreshports.Location = new System.Drawing.Point(464, 101);
            this.btnRefreshports.Name = "btnRefreshports";
            this.btnRefreshports.Size = new System.Drawing.Size(74, 38);
            this.btnRefreshports.TabIndex = 29;
            this.btnRefreshports.Text = "Refresh Ports";
            this.btnRefreshports.UseVisualStyleBackColor = true;
            this.btnRefreshports.Click += new System.EventHandler(this.btnRefreshports_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(580, 283);
            this.Controls.Add(this.btnRefreshports);
            this.Controls.Add(this.btnEncDec);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbPort);
            this.Controls.Add(this.txtDistenation);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.btnSelectFixFile);
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MobileEncDec";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Button btnSelectFixFile;
        private System.Windows.Forms.TextBox txtDistenation;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEncDec;
        private System.Windows.Forms.Button btnRefreshports;
    }
}

