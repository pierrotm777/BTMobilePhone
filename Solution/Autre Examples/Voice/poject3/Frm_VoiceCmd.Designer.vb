<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Frm_VoiceCmd
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		'This form is an MDI child.
		'This code simulates the VB6 
		' functionality of automatically
		' loading and showing an MDI
		' child's parent.
		Me.MDIParent = Project3.Frm_Main
		Project3.Frm_Main.Show
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
	Public WithEvents Lst_Commands As System.Windows.Forms.ListBox
	Public WithEvents VU_Meter As System.Windows.Forms.ProgressBar
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Detect As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Lst_Commands = New System.Windows.Forms.ListBox
        Me.VU_Meter = New System.Windows.Forms.ProgressBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.Detect = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Lst_Commands
        '
        Me.Lst_Commands.BackColor = System.Drawing.Color.White
        Me.Lst_Commands.Cursor = System.Windows.Forms.Cursors.Default
        Me.Lst_Commands.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lst_Commands.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Lst_Commands.ItemHeight = 14
        Me.Lst_Commands.Location = New System.Drawing.Point(8, 32)
        Me.Lst_Commands.Name = "Lst_Commands"
        Me.Lst_Commands.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Lst_Commands.Size = New System.Drawing.Size(201, 88)
        Me.Lst_Commands.TabIndex = 1
        '
        'VU_Meter
        '
        Me.VU_Meter.Location = New System.Drawing.Point(8, 126)
        Me.VU_Meter.Maximum = 60000
        Me.VU_Meter.Name = "VU_Meter"
        Me.VU_Meter.Size = New System.Drawing.Size(201, 27)
        Me.VU_Meter.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(105, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Last Heard Command"
        '
        'Detect
        '
        Me.Detect.BackColor = System.Drawing.SystemColors.Control
        Me.Detect.Cursor = System.Windows.Forms.Cursors.Default
        Me.Detect.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Detect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Detect.Location = New System.Drawing.Point(128, 8)
        Me.Detect.Name = "Detect"
        Me.Detect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Detect.Size = New System.Drawing.Size(81, 17)
        Me.Detect.TabIndex = 2
        Me.Detect.Text = "<Nothing>"
        '
        'Frm_VoiceCmd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(217, 162)
        Me.Controls.Add(Me.Lst_Commands)
        Me.Controls.Add(Me.VU_Meter)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Detect)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Frm_VoiceCmd"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Voice command "
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class