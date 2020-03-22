<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.tbReceiver = New System.Windows.Forms.TextBox
        Me.tbMessage = New System.Windows.Forms.TextBox
        Me.lTo = New System.Windows.Forms.Label
        Me.lMessage = New System.Windows.Forms.Label
        Me.bSend = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'tbReceiver
        '
        Me.tbReceiver.Location = New System.Drawing.Point(71, 8)
        Me.tbReceiver.Name = "tbReceiver"
        Me.tbReceiver.Size = New System.Drawing.Size(209, 20)
        Me.tbReceiver.TabIndex = 0
        Me.tbReceiver.Text = "+1234567890"
        '
        'tbMessage
        '
        Me.tbMessage.Location = New System.Drawing.Point(71, 38)
        Me.tbMessage.Multiline = True
        Me.tbMessage.Name = "tbMessage"
        Me.tbMessage.Size = New System.Drawing.Size(209, 89)
        Me.tbMessage.TabIndex = 1
        Me.tbMessage.Text = "Hello World!"
        '
        'lTo
        '
        Me.lTo.AutoSize = True
        Me.lTo.Location = New System.Drawing.Point(11, 15)
        Me.lTo.Name = "lTo"
        Me.lTo.Size = New System.Drawing.Size(23, 13)
        Me.lTo.TabIndex = 2
        Me.lTo.Text = "To:"
        '
        'lMessage
        '
        Me.lMessage.AutoSize = True
        Me.lMessage.Location = New System.Drawing.Point(12, 41)
        Me.lMessage.Name = "lMessage"
        Me.lMessage.Size = New System.Drawing.Size(53, 13)
        Me.lMessage.TabIndex = 3
        Me.lMessage.Text = "Message:"
        '
        'bSend
        '
        Me.bSend.Location = New System.Drawing.Point(71, 133)
        Me.bSend.Name = "bSend"
        Me.bSend.Size = New System.Drawing.Size(209, 23)
        Me.bSend.TabIndex = 4
        Me.bSend.Text = "Send"
        Me.bSend.UseVisualStyleBackColor = True
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 164)
        Me.Controls.Add(Me.bSend)
        Me.Controls.Add(Me.lMessage)
        Me.Controls.Add(Me.lTo)
        Me.Controls.Add(Me.tbMessage)
        Me.Controls.Add(Me.tbReceiver)
        Me.Name = "fMain"
        Me.Text = "SMS Client test form"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbReceiver As System.Windows.Forms.TextBox
    Friend WithEvents tbMessage As System.Windows.Forms.TextBox
    Friend WithEvents lTo As System.Windows.Forms.Label
    Friend WithEvents lMessage As System.Windows.Forms.Label
    Friend WithEvents bSend As System.Windows.Forms.Button

End Class
