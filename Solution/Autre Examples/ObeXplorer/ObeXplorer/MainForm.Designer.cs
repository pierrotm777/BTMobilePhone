namespace ObeXplorer
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mstMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sstStatus = new System.Windows.Forms.StatusStrip();
            this.tsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lsvExplorer = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmAccessed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsExplorerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsiNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.imlLarge = new System.Windows.Forms.ImageList(this.components);
            this.imlSmall = new System.Windows.Forms.ImageList(this.components);
            this.tstoolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUp = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDownload = new System.Windows.Forms.ToolStripButton();
            this.tsbUpload = new System.Windows.Forms.ToolStripButton();
            this.tsbNewFolder = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tscViewCombo = new System.Windows.Forms.ToolStripComboBox();
            this.bgwWorker = new System.ComponentModel.BackgroundWorker();
            this.opdOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.fbdChooseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.DragAndDrop = new DragAndDropFileControlLibrary.DragAndDropFileComponent(this.components);
            this.mstMenu.SuspendLayout();
            this.sstStatus.SuspendLayout();
            this.cmsExplorerContextMenu.SuspendLayout();
            this.tstoolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DragAndDrop)).BeginInit();
            this.SuspendLayout();
            // 
            // mstMenu
            // 
            this.mstMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tsiHelp});
            this.mstMenu.Location = new System.Drawing.Point(0, 0);
            this.mstMenu.Name = "mstMenu";
            this.mstMenu.Size = new System.Drawing.Size(754, 24);
            this.mstMenu.TabIndex = 0;
            this.mstMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiConnect,
            this.tsiDisconnect,
            this.toolStripSeparator,
            this.tsiExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // tsiConnect
            // 
            this.tsiConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsiConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiConnect.Name = "tsiConnect";
            this.tsiConnect.Size = new System.Drawing.Size(133, 22);
            this.tsiConnect.Text = "Connect";
            this.tsiConnect.Click += new System.EventHandler(this.tsiConnect_Click);
            // 
            // tsiDisconnect
            // 
            this.tsiDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiDisconnect.Name = "tsiDisconnect";
            this.tsiDisconnect.Size = new System.Drawing.Size(133, 22);
            this.tsiDisconnect.Text = "Disconnect";
            this.tsiDisconnect.Click += new System.EventHandler(this.tsiDisconnect_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(130, 6);
            // 
            // tsiExit
            // 
            this.tsiExit.Name = "tsiExit";
            this.tsiExit.Size = new System.Drawing.Size(133, 22);
            this.tsiExit.Text = "E&xit";
            this.tsiExit.Click += new System.EventHandler(this.tsiExit_Click);
            // 
            // tsiHelp
            // 
            this.tsiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.tsiHelp.Name = "tsiHelp";
            this.tsiHelp.Size = new System.Drawing.Size(44, 20);
            this.tsiHelp.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.tsiAbout_Click);
            // 
            // sstStatus
            // 
            this.sstStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatusLabel});
            this.sstStatus.Location = new System.Drawing.Point(0, 386);
            this.sstStatus.Name = "sstStatus";
            this.sstStatus.Size = new System.Drawing.Size(754, 22);
            this.sstStatus.TabIndex = 1;
            this.sstStatus.Text = "statusStrip1";
            // 
            // tsStatusLabel
            // 
            this.tsStatusLabel.Name = "tsStatusLabel";
            this.tsStatusLabel.Size = new System.Drawing.Size(86, 17);
            this.tsStatusLabel.Text = "Not connected";
            // 
            // lsvExplorer
            // 
            this.lsvExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvExplorer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmSize,
            this.clmModified,
            this.clmAccessed,
            this.clmCreated});
            this.lsvExplorer.ContextMenuStrip = this.cmsExplorerContextMenu;
            this.lsvExplorer.LargeImageList = this.imlLarge;
            this.lsvExplorer.Location = new System.Drawing.Point(0, 52);
            this.lsvExplorer.Name = "lsvExplorer";
            this.lsvExplorer.Size = new System.Drawing.Size(754, 334);
            this.lsvExplorer.SmallImageList = this.imlSmall;
            this.lsvExplorer.TabIndex = 2;
            this.lsvExplorer.UseCompatibleStateImageBehavior = false;
            this.lsvExplorer.View = System.Windows.Forms.View.Details;
            this.lsvExplorer.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lsvExplorer_AfterLabelEdit);
            this.lsvExplorer.SelectedIndexChanged += new System.EventHandler(this.lsvExplorer_SelectedIndexChanged);
            this.lsvExplorer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lsvExplorer_KeyUp);
            this.lsvExplorer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsvExplorer_MouseDoubleClick);
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 200;
            // 
            // clmSize
            // 
            this.clmSize.Text = "Size";
            this.clmSize.Width = 100;
            // 
            // clmModified
            // 
            this.clmModified.Text = "Last Modified";
            this.clmModified.Width = 150;
            // 
            // clmAccessed
            // 
            this.clmAccessed.Text = "Last Accessed";
            this.clmAccessed.Width = 150;
            // 
            // clmCreated
            // 
            this.clmCreated.Text = "Created at";
            this.clmCreated.Width = 150;
            // 
            // cmsExplorerContextMenu
            // 
            this.cmsExplorerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiNewFolder,
            this.tsiRefresh,
            this.tsiUpload,
            this.tsiSave,
            this.tsiDelete});
            this.cmsExplorerContextMenu.Name = "cmsExplorerContextMenu";
            this.cmsExplorerContextMenu.ShowImageMargin = false;
            this.cmsExplorerContextMenu.Size = new System.Drawing.Size(139, 114);
            // 
            // tsiNewFolder
            // 
            this.tsiNewFolder.Enabled = false;
            this.tsiNewFolder.Name = "tsiNewFolder";
            this.tsiNewFolder.Size = new System.Drawing.Size(138, 22);
            this.tsiNewFolder.Text = "Create new folder";
            this.tsiNewFolder.Click += new System.EventHandler(this.tsbNewFolder_Click);
            // 
            // tsiRefresh
            // 
            this.tsiRefresh.Enabled = false;
            this.tsiRefresh.Name = "tsiRefresh";
            this.tsiRefresh.Size = new System.Drawing.Size(138, 22);
            this.tsiRefresh.Text = "Refresh this folder";
            this.tsiRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsiUpload
            // 
            this.tsiUpload.Enabled = false;
            this.tsiUpload.Name = "tsiUpload";
            this.tsiUpload.Size = new System.Drawing.Size(138, 22);
            this.tsiUpload.Text = "Upload files";
            this.tsiUpload.Click += new System.EventHandler(this.tsbUpload_Click);
            // 
            // tsiSave
            // 
            this.tsiSave.Enabled = false;
            this.tsiSave.Name = "tsiSave";
            this.tsiSave.Size = new System.Drawing.Size(138, 22);
            this.tsiSave.Text = "Save";
            this.tsiSave.Click += new System.EventHandler(this.tsbDownload_Click);
            // 
            // tsiDelete
            // 
            this.tsiDelete.Enabled = false;
            this.tsiDelete.Name = "tsiDelete";
            this.tsiDelete.Size = new System.Drawing.Size(138, 22);
            this.tsiDelete.Text = "Delete";
            this.tsiDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // imlLarge
            // 
            this.imlLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlLarge.ImageStream")));
            this.imlLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imlLarge.Images.SetKeyName(0, "unknown32.ico");
            this.imlLarge.Images.SetKeyName(1, "folder32.ico");
            // 
            // imlSmall
            // 
            this.imlSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlSmall.ImageStream")));
            this.imlSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imlSmall.Images.SetKeyName(0, "unknown16.ico");
            this.imlSmall.Images.SetKeyName(1, "folder16.ico");
            // 
            // tstoolStrip
            // 
            this.tstoolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tstoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbConnect,
            this.toolStripSeparator1,
            this.tsbUp,
            this.tsbRefresh,
            this.toolStripSeparator6,
            this.tsbDownload,
            this.tsbUpload,
            this.tsbNewFolder,
            this.tsbDelete,
            this.toolStripSeparator2,
            this.tscViewCombo});
            this.tstoolStrip.Location = new System.Drawing.Point(0, 24);
            this.tstoolStrip.Name = "tstoolStrip";
            this.tstoolStrip.Size = new System.Drawing.Size(754, 25);
            this.tstoolStrip.TabIndex = 3;
            this.tstoolStrip.Text = "toolStrip1";
            // 
            // tsbConnect
            // 
            this.tsbConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbConnect.Image = ((System.Drawing.Image)(resources.GetObject("tsbConnect.Image")));
            this.tsbConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConnect.Name = "tsbConnect";
            this.tsbConnect.Size = new System.Drawing.Size(23, 22);
            this.tsbConnect.Text = "Connect";
            this.tsbConnect.Click += new System.EventHandler(this.tsiConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbUp
            // 
            this.tsbUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUp.Image = ((System.Drawing.Image)(resources.GetObject("tsbUp.Image")));
            this.tsbUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUp.Name = "tsbUp";
            this.tsbUp.Size = new System.Drawing.Size(23, 22);
            this.tsbUp.Text = "Up";
            this.tsbUp.Click += new System.EventHandler(this.tsbUp_Click);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefresh.Image")));
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbRefresh.Text = "Refresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDownload
            // 
            this.tsbDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDownload.Image = ((System.Drawing.Image)(resources.GetObject("tsbDownload.Image")));
            this.tsbDownload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDownload.Name = "tsbDownload";
            this.tsbDownload.Size = new System.Drawing.Size(23, 22);
            this.tsbDownload.Text = "Download Files";
            this.tsbDownload.Click += new System.EventHandler(this.tsbDownload_Click);
            // 
            // tsbUpload
            // 
            this.tsbUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUpload.Image = ((System.Drawing.Image)(resources.GetObject("tsbUpload.Image")));
            this.tsbUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpload.Name = "tsbUpload";
            this.tsbUpload.Size = new System.Drawing.Size(23, 22);
            this.tsbUpload.Text = "Upload Files";
            this.tsbUpload.Click += new System.EventHandler(this.tsbUpload_Click);
            // 
            // tsbNewFolder
            // 
            this.tsbNewFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewFolder.Image")));
            this.tsbNewFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewFolder.Name = "tsbNewFolder";
            this.tsbNewFolder.Size = new System.Drawing.Size(23, 22);
            this.tsbNewFolder.Text = "New Folder";
            this.tsbNewFolder.Click += new System.EventHandler(this.tsbNewFolder_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(23, 22);
            this.tsbDelete.Text = "Delete";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tscViewCombo
            // 
            this.tscViewCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscViewCombo.Items.AddRange(new object[] {
            "Large Icon",
            "Details",
            "Small Icons",
            "List",
            "Tile"});
            this.tscViewCombo.Name = "tscViewCombo";
            this.tscViewCombo.Size = new System.Drawing.Size(121, 25);
            this.tscViewCombo.SelectedIndexChanged += new System.EventHandler(this.tscViewCombo_SelectedIndexChanged);
            // 
            // bgwWorker
            // 
            this.bgwWorker.WorkerReportsProgress = true;
            this.bgwWorker.WorkerSupportsCancellation = true;
            this.bgwWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwWorker_DoWork);
            this.bgwWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwWorker_ProgressChanged);
            this.bgwWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwWorker_RunWorkerCompleted);
            // 
            // opdOpenDialog
            // 
            this.opdOpenDialog.Filter = "All files|*.*";
            this.opdOpenDialog.Multiselect = true;
            this.opdOpenDialog.RestoreDirectory = true;
            // 
            // fbdChooseFolder
            // 
            this.fbdChooseFolder.Description = "Select directory for saving files";
            // 
            // DragAndDrop
            // 
            this.DragAndDrop.HostingForm = this;
            this.DragAndDrop.FileDropped += new DragAndDropFileControlLibrary.FileDroppedEventHandler(this.DragDrop_FileDropped);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 408);
            this.Controls.Add(this.tstoolStrip);
            this.Controls.Add(this.lsvExplorer);
            this.Controls.Add(this.sstStatus);
            this.Controls.Add(this.mstMenu);
            this.MainMenuStrip = this.mstMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bluetooth Device Browser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.mstMenu.ResumeLayout(false);
            this.mstMenu.PerformLayout();
            this.sstStatus.ResumeLayout(false);
            this.sstStatus.PerformLayout();
            this.cmsExplorerContextMenu.ResumeLayout(false);
            this.tstoolStrip.ResumeLayout(false);
            this.tstoolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DragAndDrop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mstMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsiConnect;
        private System.Windows.Forms.ToolStripMenuItem tsiDisconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem tsiExit;
        private System.Windows.Forms.ToolStripMenuItem tsiHelp;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip sstStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsStatusLabel;
        private System.Windows.Forms.ListView lsvExplorer;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.ColumnHeader clmModified;
        private System.Windows.Forms.ColumnHeader clmAccessed;
        private System.Windows.Forms.ColumnHeader clmCreated;
        private System.Windows.Forms.ToolStrip tstoolStrip;
        private System.ComponentModel.BackgroundWorker bgwWorker;
        private System.Windows.Forms.ToolStripButton tsbConnect;
        private System.Windows.Forms.ToolStripButton tsbUp;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbDownload;
        private System.Windows.Forms.ToolStripButton tsbUpload;
        private System.Windows.Forms.ToolStripButton tsbNewFolder;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.OpenFileDialog opdOpenDialog;
        private System.Windows.Forms.ContextMenuStrip cmsExplorerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsiNewFolder;
        private System.Windows.Forms.ToolStripMenuItem tsiSave;
        private System.Windows.Forms.ToolStripMenuItem tsiUpload;
        private System.Windows.Forms.ToolStripMenuItem tsiRefresh;
        private System.Windows.Forms.ToolStripMenuItem tsiDelete;
        private System.Windows.Forms.FolderBrowserDialog fbdChooseFolder;
        private System.Windows.Forms.ToolStripComboBox tscViewCombo;
        private System.Windows.Forms.ImageList imlLarge;
        private System.Windows.Forms.ImageList imlSmall;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private DragAndDropFileControlLibrary.DragAndDropFileComponent DragAndDrop;
    }
}

