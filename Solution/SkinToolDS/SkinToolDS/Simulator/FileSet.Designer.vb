<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileSet
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.lblFile = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.boxEOF = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.boxDelimiter = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnStopReadingFile = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(31, 34)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(61, 23)
        Me.btnBrowse.TabIndex = 83
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'lblFile
        '
        Me.lblFile.AutoSize = True
        Me.lblFile.Location = New System.Drawing.Point(3, 11)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(26, 13)
        Me.lblFile.TabIndex = 101
        Me.lblFile.Text = "File:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(3, 137)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(140, 13)
        Me.Label16.TabIndex = 103
        Me.Label16.Text = "When end of file is reached:"
        '
        'boxEOF
        '
        Me.boxEOF.FormattingEnabled = True
        Me.boxEOF.Items.AddRange(New Object() {"Stop sending data", "Restart on same file"})
        Me.boxEOF.Location = New System.Drawing.Point(149, 134)
        Me.boxEOF.Name = "boxEOF"
        Me.boxEOF.Size = New System.Drawing.Size(178, 21)
        Me.boxEOF.TabIndex = 102
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(93, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 13)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Delimiter:"
        '
        'boxDelimiter
        '
        Me.boxDelimiter.FormattingEnabled = True
        Me.boxDelimiter.Items.AddRange(New Object() {"$GPGGA", "$GPRMC"})
        Me.boxDelimiter.Location = New System.Drawing.Point(149, 104)
        Me.boxDelimiter.Name = "boxDelimiter"
        Me.boxDelimiter.Size = New System.Drawing.Size(107, 21)
        Me.boxDelimiter.TabIndex = 105
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(262, 107)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(174, 13)
        Me.Label2.TabIndex = 106
        Me.Label2.Text = "(Wait BEFORE sending these lines)"
        '
        'btnStopReadingFile
        '
        Me.btnStopReadingFile.Location = New System.Drawing.Point(149, 164)
        Me.btnStopReadingFile.Name = "btnStopReadingFile"
        Me.btnStopReadingFile.Size = New System.Drawing.Size(61, 23)
        Me.btnStopReadingFile.TabIndex = 107
        Me.btnStopReadingFile.Text = "Stop"
        Me.btnStopReadingFile.UseVisualStyleBackColor = True
        Me.btnStopReadingFile.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(3, 73)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(83, 13)
        Me.lblStatus.TabIndex = 108
        Me.lblStatus.Text = "Status: Stopped"
        '
        'FileSet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnStopReadingFile)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.boxDelimiter)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.boxEOF)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.btnBrowse)
        Me.Name = "FileSet"
        Me.Size = New System.Drawing.Size(511, 217)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents boxEOF As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents boxDelimiter As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnStopReadingFile As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label

End Class
