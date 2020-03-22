namespace ObeXplorer
{
    partial class FileForm
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
            this.prbProgress = new System.Windows.Forms.ProgressBar();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCurrentFile = new System.Windows.Forms.Label();
            this.bgwWorker = new System.ComponentModel.BackgroundWorker();
            this.lblSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // prbProgress
            // 
            this.prbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prbProgress.Location = new System.Drawing.Point(20, 62);
            this.prbProgress.Name = "prbProgress";
            this.prbProgress.Size = new System.Drawing.Size(432, 23);
            this.prbProgress.TabIndex = 0;
            // 
            // lblFileCount
            // 
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Location = new System.Drawing.Point(17, 14);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(110, 13);
            this.lblFileCount.TabIndex = 1;
            this.lblFileCount.Text = "Uploading File 1 of 10";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(377, 99);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCurrentFile
            // 
            this.lblCurrentFile.AutoSize = true;
            this.lblCurrentFile.Location = new System.Drawing.Point(17, 39);
            this.lblCurrentFile.Name = "lblCurrentFile";
            this.lblCurrentFile.Size = new System.Drawing.Size(55, 13);
            this.lblCurrentFile.TabIndex = 3;
            this.lblCurrentFile.Text = "Uploading";
            // 
            // bgwWorker
            // 
            this.bgwWorker.WorkerReportsProgress = true;
            this.bgwWorker.WorkerSupportsCancellation = true;
            this.bgwWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwWorker_DoWork);
            this.bgwWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwWorker_RunWorkerCompleted);
            this.bgwWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwWorker_ProgressChanged);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(161, 14);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(131, 13);
            this.lblSize.TabIndex = 4;
            this.lblSize.Text = "Downloaded 1 of 10 bytes";
            // 
            // FileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 133);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblCurrentFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblFileCount);
            this.Controls.Add(this.prbProgress);
            this.Name = "FileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Shown += new System.EventHandler(this.FileForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar prbProgress;
        private System.Windows.Forms.Label lblFileCount;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCurrentFile;
        private System.ComponentModel.BackgroundWorker bgwWorker;
        private System.Windows.Forms.Label lblSize;
    }
}