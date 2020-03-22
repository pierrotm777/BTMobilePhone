<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Main_F
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents Toolbar1 As AxComctlLib.AxToolbar
	Public CommonDialog1Open As System.Windows.Forms.OpenFileDialog
	Public CommonDialog1Save As System.Windows.Forms.SaveFileDialog
	Public WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
	Public WithEvents DirectSS1 As AxACTIVEVOICEPROJECTLib.AxDirectSS
	Public WithEvents Vdict1 As AxDICTLib.AxVdict
	Public WithEvents ImageList1 As AxComctlLib.AxImageList
	Public WithEvents New_Renamed As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents open As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents save As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents saveas As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents Split_1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents ExitApp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents File As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Cut As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Copy As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Paste As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Delete As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Split_2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents SelectAll As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Edit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents listening As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Split_3 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents showcorrection As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Addword As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Split_5 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents Options As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Dictation As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ReadDocument As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Split_4 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents Stopreading As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Read As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main_F))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Toolbar1 = New AxComctlLib.AxToolbar
        Me.CommonDialog1Open = New System.Windows.Forms.OpenFileDialog
        Me.CommonDialog1Save = New System.Windows.Forms.SaveFileDialog
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox
        Me.DirectSS1 = New AxACTIVEVOICEPROJECTLib.AxDirectSS
        Me.Vdict1 = New AxDICTLib.AxVdict
        Me.ImageList1 = New AxComctlLib.AxImageList
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.File = New System.Windows.Forms.ToolStripMenuItem
        Me.New_Renamed = New System.Windows.Forms.ToolStripMenuItem
        Me.open = New System.Windows.Forms.ToolStripMenuItem
        Me.save = New System.Windows.Forms.ToolStripMenuItem
        Me.saveas = New System.Windows.Forms.ToolStripMenuItem
        Me.Split_1 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitApp = New System.Windows.Forms.ToolStripMenuItem
        Me.Edit = New System.Windows.Forms.ToolStripMenuItem
        Me.Cut = New System.Windows.Forms.ToolStripMenuItem
        Me.Copy = New System.Windows.Forms.ToolStripMenuItem
        Me.Paste = New System.Windows.Forms.ToolStripMenuItem
        Me.Delete = New System.Windows.Forms.ToolStripMenuItem
        Me.Split_2 = New System.Windows.Forms.ToolStripSeparator
        Me.SelectAll = New System.Windows.Forms.ToolStripMenuItem
        Me.Dictation = New System.Windows.Forms.ToolStripMenuItem
        Me.listening = New System.Windows.Forms.ToolStripMenuItem
        Me.Split_3 = New System.Windows.Forms.ToolStripSeparator
        Me.showcorrection = New System.Windows.Forms.ToolStripMenuItem
        Me.Addword = New System.Windows.Forms.ToolStripMenuItem
        Me.Split_5 = New System.Windows.Forms.ToolStripSeparator
        Me.Options = New System.Windows.Forms.ToolStripMenuItem
        Me.Read = New System.Windows.Forms.ToolStripMenuItem
        Me.ReadDocument = New System.Windows.Forms.ToolStripMenuItem
        Me.Split_4 = New System.Windows.Forms.ToolStripSeparator
        Me.Stopreading = New System.Windows.Forms.ToolStripMenuItem
        CType(Me.Toolbar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DirectSS1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Vdict1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageList1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Toolbar1
        '
        Me.Toolbar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Toolbar1.Location = New System.Drawing.Point(0, 24)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.OcxState = CType(resources.GetObject("Toolbar1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Toolbar1.Size = New System.Drawing.Size(644, 28)
        Me.Toolbar1.TabIndex = 1
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.Location = New System.Drawing.Point(1, 32)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.RightMargin = 1
        Me.RichTextBox1.Size = New System.Drawing.Size(628, 156)
        Me.RichTextBox1.TabIndex = 0
        Me.RichTextBox1.Text = ""
        '
        'DirectSS1
        '
        Me.DirectSS1.Enabled = True
        Me.DirectSS1.Location = New System.Drawing.Point(88, 192)
        Me.DirectSS1.Name = "DirectSS1"
        Me.DirectSS1.OcxState = CType(resources.GetObject("DirectSS1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.DirectSS1.Size = New System.Drawing.Size(33, 25)
        Me.DirectSS1.TabIndex = 3
        Me.DirectSS1.Visible = False
        '
        'Vdict1
        '
        Me.Vdict1.Enabled = True
        Me.Vdict1.Location = New System.Drawing.Point(128, 192)
        Me.Vdict1.Name = "Vdict1"
        Me.Vdict1.OcxState = CType(resources.GetObject("Vdict1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Vdict1.Size = New System.Drawing.Size(192, 192)
        Me.Vdict1.TabIndex = 2
        Me.Vdict1.Visible = False
        '
        'ImageList1
        '
        Me.ImageList1.Enabled = True
        Me.ImageList1.Location = New System.Drawing.Point(8, 192)
        Me.ImageList1.Name = "ImageList1"
        Me.ImageList1.OcxState = CType(resources.GetObject("ImageList1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.ImageList1.Size = New System.Drawing.Size(38, 38)
        Me.ImageList1.TabIndex = 4
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.File, Me.Edit, Me.Dictation, Me.Read})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(644, 24)
        Me.MainMenu1.TabIndex = 5
        '
        'File
        '
        Me.File.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.New_Renamed, Me.open, Me.save, Me.saveas, Me.Split_1, Me.ExitApp})
        Me.File.Name = "File"
        Me.File.Size = New System.Drawing.Size(35, 20)
        Me.File.Text = "File"
        '
        'New_Renamed
        '
        Me.New_Renamed.Name = "New_Renamed"
        Me.New_Renamed.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.New_Renamed.Size = New System.Drawing.Size(163, 22)
        Me.New_Renamed.Text = "New"
        '
        'open
        '
        Me.open.Name = "open"
        Me.open.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.open.Size = New System.Drawing.Size(163, 22)
        Me.open.Text = "Open..."
        '
        'save
        '
        Me.save.Name = "save"
        Me.save.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.save.Size = New System.Drawing.Size(163, 22)
        Me.save.Text = "&Save"
        '
        'saveas
        '
        Me.saveas.Name = "saveas"
        Me.saveas.Size = New System.Drawing.Size(163, 22)
        Me.saveas.Text = "Save As..."
        '
        'Split_1
        '
        Me.Split_1.Name = "Split_1"
        Me.Split_1.Size = New System.Drawing.Size(160, 6)
        '
        'ExitApp
        '
        Me.ExitApp.Name = "ExitApp"
        Me.ExitApp.Size = New System.Drawing.Size(163, 22)
        Me.ExitApp.Text = "Exit"
        '
        'Edit
        '
        Me.Edit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Cut, Me.Copy, Me.Paste, Me.Delete, Me.Split_2, Me.SelectAll})
        Me.Edit.Name = "Edit"
        Me.Edit.Size = New System.Drawing.Size(37, 20)
        Me.Edit.Text = "Edit"
        '
        'Cut
        '
        Me.Cut.Name = "Cut"
        Me.Cut.Size = New System.Drawing.Size(167, 22)
        Me.Cut.Text = "Cut"
        '
        'Copy
        '
        Me.Copy.Name = "Copy"
        Me.Copy.Size = New System.Drawing.Size(167, 22)
        Me.Copy.Text = "Copy"
        '
        'Paste
        '
        Me.Paste.Name = "Paste"
        Me.Paste.Size = New System.Drawing.Size(167, 22)
        Me.Paste.Text = "Paste"
        '
        'Delete
        '
        Me.Delete.Name = "Delete"
        Me.Delete.Size = New System.Drawing.Size(167, 22)
        Me.Delete.Text = "Delete"
        '
        'Split_2
        '
        Me.Split_2.Name = "Split_2"
        Me.Split_2.Size = New System.Drawing.Size(164, 6)
        '
        'SelectAll
        '
        Me.SelectAll.Name = "SelectAll"
        Me.SelectAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.SelectAll.Size = New System.Drawing.Size(167, 22)
        Me.SelectAll.Text = "Select All"
        '
        'Dictation
        '
        Me.Dictation.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.listening, Me.Split_3, Me.showcorrection, Me.Addword, Me.Split_5, Me.Options})
        Me.Dictation.Name = "Dictation"
        Me.Dictation.Size = New System.Drawing.Size(61, 20)
        Me.Dictation.Text = "Dictation"
        '
        'listening
        '
        Me.listening.Checked = True
        Me.listening.CheckState = System.Windows.Forms.CheckState.Checked
        Me.listening.Name = "listening"
        Me.listening.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.listening.Size = New System.Drawing.Size(225, 22)
        Me.listening.Text = "Listening for dictation"
        '
        'Split_3
        '
        Me.Split_3.Name = "Split_3"
        Me.Split_3.Size = New System.Drawing.Size(222, 6)
        '
        'showcorrection
        '
        Me.showcorrection.Name = "showcorrection"
        Me.showcorrection.Size = New System.Drawing.Size(225, 22)
        Me.showcorrection.Text = "Show Correction Window"
        '
        'Addword
        '
        Me.Addword.Name = "Addword"
        Me.Addword.Size = New System.Drawing.Size(225, 22)
        Me.Addword.Text = "Add Word..."
        '
        'Split_5
        '
        Me.Split_5.Name = "Split_5"
        Me.Split_5.Size = New System.Drawing.Size(222, 6)
        '
        'Options
        '
        Me.Options.Name = "Options"
        Me.Options.Size = New System.Drawing.Size(225, 22)
        Me.Options.Text = "Dictation Options"
        '
        'Read
        '
        Me.Read.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReadDocument, Me.Split_4, Me.Stopreading})
        Me.Read.Name = "Read"
        Me.Read.Size = New System.Drawing.Size(44, 20)
        Me.Read.Text = "Read"
        '
        'ReadDocument
        '
        Me.ReadDocument.Name = "ReadDocument"
        Me.ReadDocument.Size = New System.Drawing.Size(208, 22)
        Me.ReadDocument.Text = "Read from Insertion Point"
        '
        'Split_4
        '
        Me.Split_4.Name = "Split_4"
        Me.Split_4.Size = New System.Drawing.Size(205, 6)
        '
        'Stopreading
        '
        Me.Stopreading.Name = "Stopreading"
        Me.Stopreading.Size = New System.Drawing.Size(208, 22)
        Me.Stopreading.Text = "Stop Reading"
        '
        'Main_F
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(644, 547)
        Me.Controls.Add(Me.Toolbar1)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.DirectSS1)
        Me.Controls.Add(Me.Vdict1)
        Me.Controls.Add(Me.ImageList1)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(11, 49)
        Me.Name = "Main_F"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Dictation Pad (Visual Basic Version)"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.Toolbar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DirectSS1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Vdict1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageList1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class