namespace TestRemoteControl
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
            this.txtAnswer = new System.Windows.Forms.TextBox();
            this.btnEnableService = new System.Windows.Forms.Button();
            this.btnTitel = new System.Windows.Forms.Button();
            this.btnList = new System.Windows.Forms.Button();
            this.bntQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(386, 13);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(105, 27);
            this.btnDisconnect.TabIndex = 13;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // txtAnswer
            // 
            this.txtAnswer.Location = new System.Drawing.Point(13, 181);
            this.txtAnswer.Multiline = true;
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAnswer.Size = new System.Drawing.Size(478, 159);
            this.txtAnswer.TabIndex = 10;
            // 
            // btnEnableService
            // 
            this.btnEnableService.Location = new System.Drawing.Point(12, 12);
            this.btnEnableService.Name = "btnEnableService";
            this.btnEnableService.Size = new System.Drawing.Size(168, 28);
            this.btnEnableService.TabIndex = 14;
            this.btnEnableService.Text = "Enable Service";
            this.btnEnableService.UseVisualStyleBackColor = true;
            this.btnEnableService.Click += new System.EventHandler(this.btnEnableService_Click);
            // 
            // btnTitel
            // 
            this.btnTitel.Location = new System.Drawing.Point(33, 54);
            this.btnTitel.Name = "btnTitel";
            this.btnTitel.Size = new System.Drawing.Size(96, 30);
            this.btnTitel.TabIndex = 15;
            this.btnTitel.Text = "send titel";
            this.btnTitel.UseVisualStyleBackColor = true;
            this.btnTitel.Click += new System.EventHandler(this.btnTitel_Click);
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(164, 57);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(133, 26);
            this.btnList.TabIndex = 16;
            this.btnList.Text = "send list";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // bntQuit
            // 
            this.bntQuit.Location = new System.Drawing.Point(416, 89);
            this.bntQuit.Name = "bntQuit";
            this.bntQuit.Size = new System.Drawing.Size(75, 23);
            this.bntQuit.TabIndex = 17;
            this.bntQuit.Text = "Quit";
            this.bntQuit.UseVisualStyleBackColor = true;
            this.bntQuit.Click += new System.EventHandler(this.bntQuit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 352);
            this.Controls.Add(this.bntQuit);
            this.Controls.Add(this.btnList);
            this.Controls.Add(this.btnTitel);
            this.Controls.Add(this.btnEnableService);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.txtAnswer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TextBox txtAnswer;
        private System.Windows.Forms.Button btnEnableService;
        private System.Windows.Forms.Button btnTitel;
        private System.Windows.Forms.Button btnList;
        private System.Windows.Forms.Button bntQuit;
    }
}

