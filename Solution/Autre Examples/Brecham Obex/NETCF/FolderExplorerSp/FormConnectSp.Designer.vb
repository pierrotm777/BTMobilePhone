<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormConnectSp
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
        Me.MenuItemOk = New System.Windows.Forms.MenuItem
        Me.MenuItemCancel = New System.Windows.Forms.MenuItem
        Me.ProtocolComboBox1 = New Brecham.Obex.Net.Forms.ProtocolComboBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItemOk)
        Me.mainMenu1.MenuItems.Add(Me.MenuItemCancel)
        '
        'MenuItemOk
        '
        Me.MenuItemOk.Text = "OK"
        '
        'MenuItemCancel
        '
        Me.MenuItemCancel.Text = "Cancel"
        '
        'ProtocolComboBox1
        '
        Me.ProtocolComboBox1.Location = New System.Drawing.Point(3, 26)
        Me.ProtocolComboBox1.Name = "ProtocolComboBox1"
        Me.ProtocolComboBox1.Size = New System.Drawing.Size(75, 20)
        Me.ProtocolComboBox1.TabIndex = 10
        '
        'FormConnectSp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(176, 180)
        Me.Controls.Add(Me.ProtocolComboBox1)
        Me.Menu = Me.mainMenu1
        Me.Name = "FormConnectSp"
        Me.Text = "Connect"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ProtocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox
    Friend WithEvents MenuItemOk As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemCancel As System.Windows.Forms.MenuItem
End Class
