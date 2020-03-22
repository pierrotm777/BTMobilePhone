<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Form1
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
    private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.buttonGetStrm = New System.Windows.Forms.Button
        Me.buttonGetTo = New System.Windows.Forms.Button
        Me.buttonSetPathUp = New System.Windows.Forms.Button
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.comboBoxFileType = New System.Windows.Forms.ComboBox
        Me.textBoxFileName = New System.Windows.Forms.TextBox
        Me.buttonSetPath = New System.Windows.Forms.Button
        Me.buttonConnect = New System.Windows.Forms.Button
        Me.protocolComboBox1 = New Brecham.Obex.Net.Forms.ProtocolComboBox
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.ButtonDisconnect = New System.Windows.Forms.Button
        Me.StatusLabel = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'buttonGetStrm
        '
        Me.buttonGetStrm.Enabled = False
        Me.buttonGetStrm.Location = New System.Drawing.Point(82, 186)
        Me.buttonGetStrm.Name = "buttonGetStrm"
        Me.buttonGetStrm.Size = New System.Drawing.Size(79, 20)
        Me.buttonGetStrm.TabIndex = 19
        Me.buttonGetStrm.Text = "Get[Strm]…"
        '
        'buttonGetTo
        '
        Me.buttonGetTo.Enabled = False
        Me.buttonGetTo.Location = New System.Drawing.Point(4, 186)
        Me.buttonGetTo.Name = "buttonGetTo"
        Me.buttonGetTo.Size = New System.Drawing.Size(72, 20)
        Me.buttonGetTo.TabIndex = 18
        Me.buttonGetTo.Text = "&GetTo…"
        '
        'buttonSetPathUp
        '
        Me.buttonSetPathUp.Enabled = False
        Me.buttonSetPathUp.Location = New System.Drawing.Point(82, 160)
        Me.buttonSetPathUp.Name = "buttonSetPathUp"
        Me.buttonSetPathUp.Size = New System.Drawing.Size(79, 20)
        Me.buttonSetPathUp.TabIndex = 17
        Me.buttonSetPathUp.Text = "SetPath &Up"
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(4, 134)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(49, 20)
        Me.label2.Text = "&Type:"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(4, 106)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(49, 20)
        Me.label1.Text = "&Name:"
        '
        'comboBoxFileType
        '
        Me.comboBoxFileType.Location = New System.Drawing.Point(59, 132)
        Me.comboBoxFileType.Name = "comboBoxFileType"
        Me.comboBoxFileType.Size = New System.Drawing.Size(178, 22)
        Me.comboBoxFileType.TabIndex = 16
        '
        'textBoxFileName
        '
        Me.textBoxFileName.Enabled = False
        Me.textBoxFileName.Location = New System.Drawing.Point(59, 105)
        Me.textBoxFileName.Name = "textBoxFileName"
        Me.textBoxFileName.Size = New System.Drawing.Size(178, 21)
        Me.textBoxFileName.TabIndex = 15
        '
        'buttonSetPath
        '
        Me.buttonSetPath.Enabled = False
        Me.buttonSetPath.Location = New System.Drawing.Point(3, 160)
        Me.buttonSetPath.Name = "buttonSetPath"
        Me.buttonSetPath.Size = New System.Drawing.Size(72, 20)
        Me.buttonSetPath.TabIndex = 14
        Me.buttonSetPath.Text = "Set&Path"
        '
        'buttonConnect
        '
        Me.buttonConnect.Location = New System.Drawing.Point(93, 6)
        Me.buttonConnect.Name = "buttonConnect"
        Me.buttonConnect.Size = New System.Drawing.Size(80, 20)
        Me.buttonConnect.TabIndex = 13
        Me.buttonConnect.Text = "&Connect…"
        '
        'protocolComboBox1
        '
        Me.protocolComboBox1.Location = New System.Drawing.Point(4, 6)
        Me.protocolComboBox1.Name = "protocolComboBox1"
        Me.protocolComboBox1.Size = New System.Drawing.Size(83, 20)
        Me.protocolComboBox1.TabIndex = 12
        '
        'ButtonDisconnect
        '
        Me.ButtonDisconnect.Enabled = False
        Me.ButtonDisconnect.Location = New System.Drawing.Point(93, 32)
        Me.ButtonDisconnect.Name = "ButtonDisconnect"
        Me.ButtonDisconnect.Size = New System.Drawing.Size(80, 20)
        Me.ButtonDisconnect.TabIndex = 22
        Me.ButtonDisconnect.Text = "&Disconnect"
        '
        'StatusLabel
        '
        Me.StatusLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusLabel.Location = New System.Drawing.Point(4, 55)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(233, 38)
        Me.StatusLabel.Text = "LabelStatus"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.StatusLabel)
        Me.Controls.Add(Me.ButtonDisconnect)
        Me.Controls.Add(Me.buttonGetStrm)
        Me.Controls.Add(Me.buttonGetTo)
        Me.Controls.Add(Me.buttonSetPathUp)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.comboBoxFileType)
        Me.Controls.Add(Me.textBoxFileName)
        Me.Controls.Add(Me.buttonSetPath)
        Me.Controls.Add(Me.buttonConnect)
        Me.Controls.Add(Me.protocolComboBox1)
        Me.Menu = Me.mainMenu1
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.Text = "GetUi"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents buttonGetStrm As System.Windows.Forms.Button
    Private WithEvents buttonGetTo As System.Windows.Forms.Button
    Private WithEvents buttonSetPathUp As System.Windows.Forms.Button
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents comboBoxFileType As System.Windows.Forms.ComboBox
    Private WithEvents textBoxFileName As System.Windows.Forms.TextBox
    Private WithEvents buttonSetPath As System.Windows.Forms.Button
    Private WithEvents buttonConnect As System.Windows.Forms.Button
    Private WithEvents protocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ButtonDisconnect As System.Windows.Forms.Button
    Friend WithEvents StatusLabel As System.Windows.Forms.Label

End Class
