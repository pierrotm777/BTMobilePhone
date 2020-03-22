<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Frm_Main
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
	Public WithEvents _SBar_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents _SBar_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents SBar As System.Windows.Forms.StatusStrip
	Public WithEvents VoiceDic As AxDICTLib.AxVdict
	Public CD1Open As System.Windows.Forms.OpenFileDialog
	Public WithEvents VoiceCmd As AxHSRLib.AxVcommand
	Public WithEvents Picture1 As System.Windows.Forms.Panel
	Public WithEvents File_Open As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents File_Save As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents File_Close As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents Split1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents Main_Exit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents Menu_File As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents Voice_Listen As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents Diction_Listen As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents Voise_show As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents Main_Voice As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Frm_Main))
		Me.IsMDIContainer = True
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.SBar = New System.Windows.Forms.StatusStrip
		Me._SBar_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me._SBar_Panel2 = New System.Windows.Forms.ToolStripStatusLabel
		Me.Picture1 = New System.Windows.Forms.Panel
		Me.VoiceDic = New AxDICTLib.AxVdict
		Me.CD1Open = New System.Windows.Forms.OpenFileDialog
		Me.VoiceCmd = New AxHSRLib.AxVcommand
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.Menu_File = New System.Windows.Forms.ToolStripMenuItem
		Me.File_Open = New System.Windows.Forms.ToolStripMenuItem
		Me.File_Save = New System.Windows.Forms.ToolStripMenuItem
		Me.File_Close = New System.Windows.Forms.ToolStripMenuItem
		Me.Split1 = New System.Windows.Forms.ToolStripSeparator
		Me.Main_Exit = New System.Windows.Forms.ToolStripMenuItem
		Me.Main_Voice = New System.Windows.Forms.ToolStripMenuItem
		Me.Voice_Listen = New System.Windows.Forms.ToolStripMenuItem
		Me.Diction_Listen = New System.Windows.Forms.ToolStripMenuItem
		Me.Voise_show = New System.Windows.Forms.ToolStripMenuItem
		Me.SBar.SuspendLayout()
		Me.Picture1.SuspendLayout()
		Me.MainMenu1.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.VoiceDic, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.VoiceCmd, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.BackColor = System.Drawing.SystemColors.AppWorkspace
		Me.Text = "Main Application form"
		Me.ClientSize = New System.Drawing.Size(372, 241)
		Me.Location = New System.Drawing.Point(11, 49)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Enabled = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "Frm_Main"
		Me.SBar.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.SBar.Size = New System.Drawing.Size(372, 17)
		Me.SBar.Location = New System.Drawing.Point(0, 224)
		Me.SBar.TabIndex = 3
		Me.SBar.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SBar.Name = "SBar"
		Me._SBar_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._SBar_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		Me._SBar_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._SBar_Panel1.Visible = 0
		Me._SBar_Panel1.Size = New System.Drawing.Size(0, 17)
		Me._SBar_Panel1.AutoSize = True
		Me._SBar_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._SBar_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._SBar_Panel1.AutoSize = False
		Me._SBar_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._SBar_Panel2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		Me._SBar_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._SBar_Panel2.Visible = 0
		Me._SBar_Panel2.Size = New System.Drawing.Size(0, 17)
		Me._SBar_Panel2.AutoSize = True
		Me._SBar_Panel2.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._SBar_Panel2.Margin = New System.Windows.Forms.Padding(0)
		Me._SBar_Panel2.AutoSize = False
		Me.Picture1.Dock = System.Windows.Forms.DockStyle.Top
		Me.Picture1.Size = New System.Drawing.Size(372, 33)
		Me.Picture1.Location = New System.Drawing.Point(0, 0)
		Me.Picture1.TabIndex = 0
		Me.Picture1.Visible = False
		Me.Picture1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Picture1.BackColor = System.Drawing.SystemColors.Control
		Me.Picture1.CausesValidation = True
		Me.Picture1.Enabled = True
		Me.Picture1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Picture1.TabStop = True
		Me.Picture1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Picture1.Name = "Picture1"
		VoiceDic.OcxState = CType(resources.GetObject("VoiceDic.OcxState"), System.Windows.Forms.AxHost.State)
		Me.VoiceDic.Size = New System.Drawing.Size(33, 33)
		Me.VoiceDic.Location = New System.Drawing.Point(72, 0)
		Me.VoiceDic.TabIndex = 2
		Me.VoiceDic.Name = "VoiceDic"
		VoiceCmd.OcxState = CType(resources.GetObject("VoiceCmd.OcxState"), System.Windows.Forms.AxHost.State)
		Me.VoiceCmd.Size = New System.Drawing.Size(41, 33)
		Me.VoiceCmd.Location = New System.Drawing.Point(0, 0)
		Me.VoiceCmd.TabIndex = 1
		Me.VoiceCmd.Name = "VoiceCmd"
		Me.Menu_File.Name = "Menu_File"
		Me.Menu_File.Text = "File"
		Me.Menu_File.Checked = False
		Me.Menu_File.Enabled = True
		Me.Menu_File.Visible = True
		Me.File_Open.Name = "File_Open"
		Me.File_Open.Text = "Open Text File"
		Me.File_Open.Checked = False
		Me.File_Open.Enabled = True
		Me.File_Open.Visible = True
		Me.File_Save.Name = "File_Save"
		Me.File_Save.Text = "Save Text file"
		Me.File_Save.Visible = False
		Me.File_Save.Checked = False
		Me.File_Save.Enabled = True
		Me.File_Close.Name = "File_Close"
		Me.File_Close.Text = "Close Text File"
		Me.File_Close.Visible = False
		Me.File_Close.Checked = False
		Me.File_Close.Enabled = True
		Me.Split1.Enabled = True
		Me.Split1.Visible = True
		Me.Split1.Name = "Split1"
		Me.Main_Exit.Name = "Main_Exit"
		Me.Main_Exit.Text = "Exit"
		Me.Main_Exit.Checked = False
		Me.Main_Exit.Enabled = True
		Me.Main_Exit.Visible = True
		Me.Main_Voice.Name = "Main_Voice"
		Me.Main_Voice.Text = "Voice Commands"
		Me.Main_Voice.Checked = False
		Me.Main_Voice.Enabled = True
		Me.Main_Voice.Visible = True
		Me.Voice_Listen.Name = "Voice_Listen"
		Me.Voice_Listen.Text = "Listen for Commands"
		Me.Voice_Listen.Checked = False
		Me.Voice_Listen.Enabled = True
		Me.Voice_Listen.Visible = True
		Me.Diction_Listen.Name = "Diction_Listen"
		Me.Diction_Listen.Text = "Listen for Diction"
		Me.Diction_Listen.Checked = False
		Me.Diction_Listen.Enabled = True
		Me.Diction_Listen.Visible = True
		Me.Voise_show.Name = "Voise_show"
		Me.Voise_show.Text = "Show List"
		Me.Voise_show.Checked = False
		Me.Voise_show.Enabled = True
		Me.Voise_show.Visible = True
		Me.Controls.Add(SBar)
		Me.Controls.Add(Picture1)
		Me.SBar.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._SBar_Panel1})
		Me.SBar.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._SBar_Panel2})
		Me.Picture1.Controls.Add(VoiceDic)
		Me.Picture1.Controls.Add(VoiceCmd)
		CType(Me.VoiceCmd, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.VoiceDic, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Menu_File.MergeAction = System.Windows.Forms.MergeAction.Remove
		Me.Main_Voice.MergeAction = System.Windows.Forms.MergeAction.Remove
		MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.Menu_File, Me.Main_Voice})
		Menu_File.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.File_Open, Me.File_Save, Me.File_Close, Me.Split1, Me.Main_Exit})
		Main_Voice.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.Voice_Listen, Me.Diction_Listen, Me.Voise_show})
		Me.Controls.Add(MainMenu1)
		Me.SBar.ResumeLayout(False)
		Me.Picture1.ResumeLayout(False)
		Me.MainMenu1.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class