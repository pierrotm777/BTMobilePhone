<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormFolderEx
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
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnName = New System.Windows.Forms.ColumnHeader
        Me.ColumnSize = New System.Windows.Forms.ColumnHeader
        Me.ColumnType = New System.Windows.Forms.ColumnHeader
        Me.ColumnModified = New System.Windows.Forms.ColumnHeader
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.ProtocolComboBox1 = New Brecham.Obex.Net.Forms.ProtocolComboBox
        Me.connectButton = New System.Windows.Forms.Button
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.disconnectButton = New System.Windows.Forms.Button
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItemQuit = New System.Windows.Forms.MenuItem
        Me.IgnoreFolderListingFaultsMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.Add(Me.ColumnName)
        Me.ListView1.Columns.Add(Me.ColumnSize)
        Me.ListView1.Columns.Add(Me.ColumnType)
        Me.ListView1.Columns.Add(Me.ColumnModified)
        Me.ListView1.Location = New System.Drawing.Point(0, 57)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(240, 189)
        Me.ListView1.TabIndex = 2
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnName
        '
        Me.ColumnName.Text = "Name"
        Me.ColumnName.Width = 95
        '
        'ColumnSize
        '
        Me.ColumnSize.Text = "Size"
        Me.ColumnSize.Width = 45
        '
        'ColumnType
        '
        Me.ColumnType.Text = "Type"
        Me.ColumnType.Width = 40
        '
        'ColumnModified
        '
        Me.ColumnModified.Text = "Modified"
        Me.ColumnModified.Width = 60
        '
        'ComboBox1
        '
        Me.ComboBox1.Location = New System.Drawing.Point(3, 32)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(75, 22)
        Me.ComboBox1.TabIndex = 3
        '
        'ProtocolComboBox1
        '
        Me.ProtocolComboBox1.Location = New System.Drawing.Point(3, 3)
        Me.ProtocolComboBox1.Name = "ProtocolComboBox1"
        Me.ProtocolComboBox1.Size = New System.Drawing.Size(75, 20)
        Me.ProtocolComboBox1.TabIndex = 9
        '
        'connectButton
        '
        Me.connectButton.Location = New System.Drawing.Point(84, 3)
        Me.connectButton.Name = "connectButton"
        Me.connectButton.Size = New System.Drawing.Size(75, 20)
        Me.connectButton.TabIndex = 8
        Me.connectButton.Text = "&Connect…"
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 246)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(240, 22)
        Me.StatusBar1.Text = "StatusBar1"
        '
        'disconnectButton
        '
        Me.disconnectButton.Location = New System.Drawing.Point(165, 3)
        Me.disconnectButton.Name = "disconnectButton"
        Me.disconnectButton.Size = New System.Drawing.Size(75, 20)
        Me.disconnectButton.TabIndex = 10
        Me.disconnectButton.Text = "&Disconnect"
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItem1)
        Me.mainMenu1.MenuItems.Add(Me.MenuItem2)
        '
        'MenuItem1
        '
        Me.MenuItem1.Enabled = False
        Me.MenuItem1.Text = "Connect"
        '
        'MenuItem2
        '
        Me.MenuItem2.MenuItems.Add(Me.MenuItem7)
        Me.MenuItem2.MenuItems.Add(Me.MenuItem3)
        Me.MenuItem2.MenuItems.Add(Me.IgnoreFolderListingFaultsMenuItem)
        Me.MenuItem2.MenuItems.Add(Me.MenuItem5)
        Me.MenuItem2.MenuItems.Add(Me.MenuItemQuit)
        Me.MenuItem2.Text = "Menu"
        '
        'MenuItem7
        '
        Me.MenuItem7.Text = "Disconnect"
        '
        'MenuItem3
        '
        Me.MenuItem3.Enabled = False
        Me.MenuItem3.MenuItems.Add(Me.MenuItem4)
        Me.MenuItem3.Text = "View style"
        '
        'MenuItem4
        '
        Me.MenuItem4.Text = "tmp, auto created here"
        '
        'MenuItemQuit
        '
        Me.MenuItemQuit.Text = "Quit"
        '
        'IgnoreFolderListingFaultsMenuItem
        '
        Me.IgnoreFolderListingFaultsMenuItem.Checked = True
        Me.IgnoreFolderListingFaultsMenuItem.Text = "Ignore Folder Listing Faults"
        '
        'MenuItem5
        '
        Me.MenuItem5.MenuItems.Add(Me.MenuItem6)
        Me.MenuItem5.Text = "Test"
        '
        'MenuItem6
        '
        Me.MenuItem6.Text = "Dummy List 1"
        '
        'FormFolderEx
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.disconnectButton)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.ProtocolComboBox1)
        Me.Controls.Add(Me.connectButton)
        Me.Controls.Add(Me.ComboBox1)
        Me.Menu = Me.mainMenu1
        Me.Name = "FormFolderEx"
        Me.Text = "FolderExplorer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ProtocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox
    Friend WithEvents connectButton As System.Windows.Forms.Button
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents ColumnName As System.Windows.Forms.ColumnHeader
    Friend WithEvents disconnectButton As System.Windows.Forms.Button
    Private WithEvents mainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemQuit As System.Windows.Forms.MenuItem
    Friend WithEvents ColumnSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnType As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnModified As System.Windows.Forms.ColumnHeader
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents IgnoreFolderListingFaultsMenuItem As System.Windows.Forms.MenuItem

End Class
