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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItemQuit = New System.Windows.Forms.MenuItem
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnName = New System.Windows.Forms.ColumnHeader
        Me.ColumnSize = New System.Windows.Forms.ColumnHeader
        Me.ColumnType = New System.Windows.Forms.ColumnHeader
        Me.ColumnModified = New System.Windows.Forms.ColumnHeader
        Me.Label1 = New System.Windows.Forms.Label
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.IgnoreFolderListingFaultsMenuItem = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItem1)
        Me.mainMenu1.MenuItems.Add(Me.MenuItem2)
        '
        'MenuItem1
        '
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
        'ListView1
        '
        Me.ListView1.Columns.Add(Me.ColumnName)
        Me.ListView1.Columns.Add(Me.ColumnSize)
        Me.ListView1.Columns.Add(Me.ColumnType)
        Me.ListView1.Columns.Add(Me.ColumnModified)
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(176, 158)
        Me.ListView1.TabIndex = 2
        Me.ListView1.View = System.Windows.Forms.View.SmallIcon
        '
        'ColumnName
        '
        Me.ColumnName.Text = "Name"
        Me.ColumnName.Width = 60
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
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label1.Font = New System.Drawing.Font("Segoe Condensed", 10.0!, System.Drawing.FontStyle.Regular)
        Me.Label1.Location = New System.Drawing.Point(0, 158)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 22)
        Me.Label1.Text = "Label1"
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
        'IgnoreFolderListingFaultsMenuItem
        '
        Me.IgnoreFolderListingFaultsMenuItem.Checked = True
        Me.IgnoreFolderListingFaultsMenuItem.Text = "Ignore Folder Listing Faults"
        '
        'FormFolderEx
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(176, 180)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Label1)
        Me.Menu = Me.mainMenu1
        Me.Name = "FormFolderEx"
        Me.Text = "FolderExplorer for Smartphone"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnName As System.Windows.Forms.ColumnHeader
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuItemQuit As System.Windows.Forms.MenuItem
    Friend WithEvents ColumnSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnType As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnModified As System.Windows.Forms.ColumnHeader
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents IgnoreFolderListingFaultsMenuItem As System.Windows.Forms.MenuItem

End Class
