<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.foldersListBox = New System.Windows.Forms.ListBox
        Me.filesListView = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.connectButton = New System.Windows.Forms.Button
        Me.BackgroundWorkerFillCurrentFolder = New System.ComponentModel.BackgroundWorker
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.disconnectButton = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboBoxLvView = New System.Windows.Forms.ComboBox
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.IgnoreFolderListingFaultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.PageSetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DummyFolder1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ProtocolComboBox1 = New Brecham.Obex.Net.Forms.ProtocolComboBox
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        Me.PageSetupDialog1 = New System.Windows.Forms.PageSetupDialog
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 60)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.foldersListBox)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.filesListView)
        Me.SplitContainer1.Size = New System.Drawing.Size(413, 232)
        Me.SplitContainer1.SplitterDistance = 137
        Me.SplitContainer1.TabIndex = 5
        '
        'foldersListBox
        '
        Me.foldersListBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.foldersListBox.FormattingEnabled = True
        Me.foldersListBox.HorizontalScrollbar = True
        Me.foldersListBox.IntegralHeight = False
        Me.foldersListBox.Location = New System.Drawing.Point(0, 0)
        Me.foldersListBox.Name = "foldersListBox"
        Me.foldersListBox.Size = New System.Drawing.Size(137, 232)
        Me.foldersListBox.TabIndex = 0
        '
        'filesListView
        '
        Me.filesListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.filesListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.filesListView.Location = New System.Drawing.Point(0, 0)
        Me.filesListView.Name = "filesListView"
        Me.filesListView.Size = New System.Drawing.Size(272, 232)
        Me.filesListView.TabIndex = 0
        Me.filesListView.UseCompatibleStateImageBehavior = False
        Me.filesListView.View = System.Windows.Forms.View.SmallIcon
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 100
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Size"
        Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader2.Width = 35
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Type"
        Me.ColumnHeader3.Width = 39
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Modified"
        Me.ColumnHeader4.Width = 97
        '
        'connectButton
        '
        Me.connectButton.Location = New System.Drawing.Point(153, 28)
        Me.connectButton.Name = "connectButton"
        Me.connectButton.Size = New System.Drawing.Size(75, 23)
        Me.connectButton.TabIndex = 2
        Me.connectButton.Text = "&Connect…"
        Me.connectButton.UseVisualStyleBackColor = True
        '
        'BackgroundWorkerFillCurrentFolder
        '
        Me.BackgroundWorkerFillCurrentFolder.WorkerReportsProgress = True
        Me.BackgroundWorkerFillCurrentFolder.WorkerSupportsCancellation = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 292)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(413, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'disconnectButton
        '
        Me.disconnectButton.Location = New System.Drawing.Point(234, 28)
        Me.disconnectButton.Name = "disconnectButton"
        Me.disconnectButton.Size = New System.Drawing.Size(75, 23)
        Me.disconnectButton.TabIndex = 3
        Me.disconnectButton.Text = "&Disconnect"
        Me.disconnectButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "&Protocol:"
        '
        'ComboBoxLvView
        '
        Me.ComboBoxLvView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxLvView.FormattingEnabled = True
        Me.ComboBoxLvView.Items.AddRange(New Object() {"is filled at runtime!"})
        Me.ComboBoxLvView.Location = New System.Drawing.Point(325, 29)
        Me.ComboBoxLvView.Name = "ComboBoxLvView"
        Me.ComboBoxLvView.Size = New System.Drawing.Size(76, 21)
        Me.ComboBoxLvView.TabIndex = 6
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.TestToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(413, 24)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IgnoreFolderListingFaultsToolStripMenuItem, Me.ToolStripSeparator2, Me.PageSetupToolStripMenuItem, Me.PrintPreviewToolStripMenuItem, Me.PrintToolStripMenuItem, Me.ToolStripSeparator3, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'IgnoreFolderListingFaultsToolStripMenuItem
        '
        Me.IgnoreFolderListingFaultsToolStripMenuItem.Checked = True
        Me.IgnoreFolderListingFaultsToolStripMenuItem.CheckOnClick = True
        Me.IgnoreFolderListingFaultsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.IgnoreFolderListingFaultsToolStripMenuItem.Name = "IgnoreFolderListingFaultsToolStripMenuItem"
        Me.IgnoreFolderListingFaultsToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.IgnoreFolderListingFaultsToolStripMenuItem.Text = "Ignore Folder-Listing faults"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(200, 6)
        '
        'PageSetupToolStripMenuItem
        '
        Me.PageSetupToolStripMenuItem.Name = "PageSetupToolStripMenuItem"
        Me.PageSetupToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.PageSetupToolStripMenuItem.Text = "Page Set&up..."
        '
        'PrintPreviewToolStripMenuItem
        '
        Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
        Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.PrintToolStripMenuItem.Text = "&Print..."
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DummyFolder1ToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.TestToolStripMenuItem.Text = "&Test"
        '
        'DummyFolder1ToolStripMenuItem
        '
        Me.DummyFolder1ToolStripMenuItem.Name = "DummyFolder1ToolStripMenuItem"
        Me.DummyFolder1ToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.DummyFolder1ToolStripMenuItem.Text = "Dummy Folder 1"
        '
        'ProtocolComboBox1
        '
        Me.ProtocolComboBox1.Location = New System.Drawing.Point(59, 29)
        Me.ProtocolComboBox1.Name = "ProtocolComboBox1"
        Me.ProtocolComboBox1.Size = New System.Drawing.Size(83, 21)
        Me.ProtocolComboBox1.TabIndex = 1
        '
        'PrintDocument1
        '
        Me.PrintDocument1.DocumentName = "Brecham.Obex FolderExplore Folder Listing"
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Document = Me.PrintDocument1
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Visible = False
        '
        'PrintDialog1
        '
        Me.PrintDialog1.Document = Me.PrintDocument1
        Me.PrintDialog1.UseEXDialog = True
        '
        'PageSetupDialog1
        '
        Me.PageSetupDialog1.Document = Me.PrintDocument1
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(200, 6)
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(200, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(413, 314)
        Me.Controls.Add(Me.ComboBoxLvView)
        Me.Controls.Add(Me.ProtocolComboBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.disconnectButton)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.connectButton)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "FolderExplorer2"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents filesListView As System.Windows.Forms.ListView
    Friend WithEvents connectButton As System.Windows.Forms.Button
    Friend WithEvents BackgroundWorkerFillCurrentFolder As System.ComponentModel.BackgroundWorker
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents foldersListBox As System.Windows.Forms.ListBox
    Friend WithEvents disconnectButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ProtocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox
    Friend WithEvents ComboBoxLvView As System.Windows.Forms.ComboBox
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents TestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DummyFolder1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IgnoreFolderListingFaultsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As System.Windows.Forms.PrintPreviewDialog
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PageSetupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PageSetupDialog1 As System.Windows.Forms.PageSetupDialog
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
