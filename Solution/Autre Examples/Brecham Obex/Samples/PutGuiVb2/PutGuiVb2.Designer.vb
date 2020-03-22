<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PutGuiVb2
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
        Me.progressBar1 = New System.Windows.Forms.ProgressBar
        Me.statusLabel = New System.Windows.Forms.Label
        Me.browseButton = New System.Windows.Forms.Button
        Me.cancelButton1 = New System.Windows.Forms.Button
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.backgroundWorker1 = New System.ComponentModel.BackgroundWorker
        Me.ProtocolComboBox1 = New Brecham.Obex.Net.Forms.ProtocolComboBox
        Me.SuspendLayout()
        '
        'progressBar1
        '
        Me.progressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.progressBar1.Location = New System.Drawing.Point(13, 186)
        Me.progressBar1.Name = "progressBar1"
        Me.progressBar1.Size = New System.Drawing.Size(213, 23)
        Me.progressBar1.TabIndex = 3
        '
        'statusLabel
        '
        Me.statusLabel.Location = New System.Drawing.Point(13, 75)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(267, 105)
        Me.statusLabel.TabIndex = 2
        Me.statusLabel.Text = "Label1"
        Me.statusLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'browseButton
        '
        Me.browseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.browseButton.Location = New System.Drawing.Point(151, 12)
        Me.browseButton.Name = "browseButton"
        Me.browseButton.Size = New System.Drawing.Size(75, 23)
        Me.browseButton.TabIndex = 1
        Me.browseButton.Text = "&Browse…"
        Me.browseButton.UseVisualStyleBackColor = True
        '
        'cancelButton1
        '
        Me.cancelButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cancelButton1.Location = New System.Drawing.Point(151, 215)
        Me.cancelButton1.Name = "cancelButton1"
        Me.cancelButton1.Size = New System.Drawing.Size(75, 23)
        Me.cancelButton1.TabIndex = 4
        Me.cancelButton1.Text = "&Cancel"
        Me.cancelButton1.UseVisualStyleBackColor = True
        '
        'backgroundWorker1
        '
        Me.backgroundWorker1.WorkerReportsProgress = True
        Me.backgroundWorker1.WorkerSupportsCancellation = True
        '
        'ProtocolComboBox1
        '
        Me.ProtocolComboBox1.Location = New System.Drawing.Point(13, 12)
        Me.ProtocolComboBox1.Name = "ProtocolComboBox1"
        Me.ProtocolComboBox1.Size = New System.Drawing.Size(83, 21)
        Me.ProtocolComboBox1.TabIndex = 0
        '
        'PutGuiVb2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(238, 266)
        Me.Controls.Add(Me.ProtocolComboBox1)
        Me.Controls.Add(Me.cancelButton1)
        Me.Controls.Add(Me.browseButton)
        Me.Controls.Add(Me.statusLabel)
        Me.Controls.Add(Me.progressBar1)
        Me.Name = "PutGuiVb2"
        Me.Text = "PutGuiVb2"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents progressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents statusLabel As System.Windows.Forms.Label
    Friend WithEvents browseButton As System.Windows.Forms.Button
    Friend WithEvents cancelButton1 As System.Windows.Forms.Button
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents backgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents ProtocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox

End Class
