
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lstPlugins = New System.Windows.Forms.ListBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.sendProgress = New System.Windows.Forms.ProgressBar()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.txtDest = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtSrc = New System.Windows.Forms.TextBox()
        Me.btnBrowser = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPlugin = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lstPlugins)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupBox1.Size = New System.Drawing.Size(275, 96)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Available Plugins"
        '
        'lstPlugins
        '
        Me.lstPlugins.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstPlugins.FormattingEnabled = True
        Me.lstPlugins.Location = New System.Drawing.Point(6, 20)
        Me.lstPlugins.Name = "lstPlugins"
        Me.lstPlugins.Size = New System.Drawing.Size(263, 64)
        Me.lstPlugins.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Panel2)
        Me.GroupBox2.Controls.Add(Me.txtDest)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Panel1)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.txtPlugin)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox2.Location = New System.Drawing.Point(0, 96)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(5, 2, 5, 5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(6, 3, 6, 6)
        Me.GroupBox2.Size = New System.Drawing.Size(275, 164)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Status"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.sendProgress)
        Me.Panel2.Controls.Add(Me.btnSend)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(6, 138)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(263, 24)
        Me.Panel2.TabIndex = 6
        '
        'sendProgress
        '
        Me.sendProgress.Dock = System.Windows.Forms.DockStyle.Fill
        Me.sendProgress.Location = New System.Drawing.Point(80, 0)
        Me.sendProgress.Name = "sendProgress"
        Me.sendProgress.Size = New System.Drawing.Size(183, 24)
        Me.sendProgress.TabIndex = 1
        '
        'btnSend
        '
        Me.btnSend.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnSend.Location = New System.Drawing.Point(0, 0)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(80, 24)
        Me.btnSend.TabIndex = 0
        Me.btnSend.Text = "Send"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'txtDest
        '
        Me.txtDest.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtDest.Location = New System.Drawing.Point(6, 118)
        Me.txtDest.Name = "txtDest"
        Me.txtDest.Size = New System.Drawing.Size(263, 20)
        Me.txtDest.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Location = New System.Drawing.Point(6, 98)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(263, 20)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Destination File Patch:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtSrc)
        Me.Panel1.Controls.Add(Me.btnBrowser)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(6, 76)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(263, 22)
        Me.Panel1.TabIndex = 3
        '
        'txtSrc
        '
        Me.txtSrc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSrc.Location = New System.Drawing.Point(0, 0)
        Me.txtSrc.Name = "txtSrc"
        Me.txtSrc.Size = New System.Drawing.Size(181, 20)
        Me.txtSrc.TabIndex = 0
        '
        'btnBrowser
        '
        Me.btnBrowser.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnBrowser.Location = New System.Drawing.Point(181, 0)
        Me.btnBrowser.Name = "btnBrowser"
        Me.btnBrowser.Size = New System.Drawing.Size(82, 22)
        Me.btnBrowser.TabIndex = 1
        Me.btnBrowser.Text = "Browser"
        Me.btnBrowser.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Location = New System.Drawing.Point(6, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(263, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Source File Patch:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPlugin
        '
        Me.txtPlugin.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtPlugin.Location = New System.Drawing.Point(6, 36)
        Me.txtPlugin.Name = "txtPlugin"
        Me.txtPlugin.Size = New System.Drawing.Size(263, 20)
        Me.txtPlugin.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(263, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Use Plugin:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MainForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(275, 260)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.Name = "MainForm"
        Me.Text = "FileTransferCodeDemo"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lstPlugins As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtPlugin As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtSrc As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowser As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDest As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents sendProgress As System.Windows.Forms.ProgressBar
End Class
