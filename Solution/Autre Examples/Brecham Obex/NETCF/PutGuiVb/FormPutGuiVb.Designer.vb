<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormPutGuiVb
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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.browseButton = New System.Windows.Forms.Button
        Me.progressBar1 = New System.Windows.Forms.ProgressBar
        Me.cancelButton1 = New System.Windows.Forms.Button
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.statusLabel = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ProtocolComboBox1 = New Brecham.Obex.Net.Forms.ProtocolComboBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItem1)
        Me.mainMenu1.MenuItems.Add(Me.MenuItem2)
        '
        'MenuItem1
        '
        Me.MenuItem1.Text = "Browse…"
        '
        'MenuItem2
        '
        Me.MenuItem2.Text = "Cancel"
        '
        'browseButton
        '
        Me.browseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.browseButton.Location = New System.Drawing.Point(165, 156)
        Me.browseButton.Name = "browseButton"
        Me.browseButton.Size = New System.Drawing.Size(72, 20)
        Me.browseButton.TabIndex = 1
        Me.browseButton.Text = "B&rowse…"
        '
        'progressBar1
        '
        Me.progressBar1.Location = New System.Drawing.Point(3, 201)
        Me.progressBar1.Name = "progressBar1"
        Me.progressBar1.Size = New System.Drawing.Size(233, 20)
        '
        'cancelButton1
        '
        Me.cancelButton1.Enabled = False
        Me.cancelButton1.Location = New System.Drawing.Point(163, 228)
        Me.cancelButton1.Name = "cancelButton1"
        Me.cancelButton1.Size = New System.Drawing.Size(72, 20)
        Me.cancelButton1.TabIndex = 2
        Me.cancelButton1.Text = "&Cancel"
        '
        'statusLabel
        '
        Me.statusLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.statusLabel.Location = New System.Drawing.Point(4, 128)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(155, 67)
        Me.statusLabel.Text = "Label1"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.Text = "&Protocol:"
        '
        'ProtocolComboBox1
        '
        Me.ProtocolComboBox1.Location = New System.Drawing.Point(71, 4)
        Me.ProtocolComboBox1.Name = "ProtocolComboBox1"
        Me.ProtocolComboBox1.Size = New System.Drawing.Size(100, 22)
        Me.ProtocolComboBox1.TabIndex = 0
        '
        'FormPutGuiVb
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.ProtocolComboBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.statusLabel)
        Me.Controls.Add(Me.cancelButton1)
        Me.Controls.Add(Me.progressBar1)
        Me.Controls.Add(Me.browseButton)
        Me.Menu = Me.mainMenu1
        Me.MinimizeBox = False
        Me.Name = "FormPutGuiVb"
        Me.Text = "PutGuiVb"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Private WithEvents browseButton As System.Windows.Forms.Button
    Friend WithEvents cancelButton1 As System.Windows.Forms.Button
    Friend WithEvents progressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents statusLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ProtocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem

End Class
