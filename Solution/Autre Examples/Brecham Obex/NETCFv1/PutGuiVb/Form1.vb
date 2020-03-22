Public Class FormPutGuiVb
    Inherits System.Windows.Forms.Form
    Private WithEvents browseButton As System.Windows.Forms.Button
    Friend WithEvents progressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents cancelButton1 As System.Windows.Forms.Button
    Friend WithEvents statusLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents protocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_workings = New Workings(Me)
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        MyBase.Dispose(disposing)
    End Sub

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.Label1 = New System.Windows.Forms.Label
        Me.statusLabel = New System.Windows.Forms.Label
        Me.cancelButton1 = New System.Windows.Forms.Button
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.progressBar1 = New System.Windows.Forms.ProgressBar
        Me.browseButton = New System.Windows.Forms.Button
        Me.protocolComboBox1 = New Brecham.Obex.Net.Forms.ProtocolComboBox
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 12)
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.Text = "&Protocol:"
        '
        'statusLabel
        '
        Me.statusLabel.Location = New System.Drawing.Point(4, 136)
        Me.statusLabel.Size = New System.Drawing.Size(155, 67)
        Me.statusLabel.Text = "statusLabel"
        '
        'cancelButton1
        '
        Me.cancelButton1.Enabled = False
        Me.cancelButton1.Location = New System.Drawing.Point(163, 236)
        Me.cancelButton1.Size = New System.Drawing.Size(72, 20)
        Me.cancelButton1.Text = "&Cancel"
        '
        'MenuItem2
        '
        Me.MenuItem2.Text = "Cancel"
        '
        'MenuItem1
        '
        Me.MenuItem1.Text = "Browse…"
        '
        'progressBar1
        '
        Me.progressBar1.Location = New System.Drawing.Point(3, 209)
        Me.progressBar1.Size = New System.Drawing.Size(233, 20)
        '
        'browseButton
        '
        Me.browseButton.Location = New System.Drawing.Point(165, 164)
        Me.browseButton.Size = New System.Drawing.Size(72, 20)
        Me.browseButton.Text = "B&rowse…"
        '
        'protocolComboBox1
        '
        Me.protocolComboBox1.BackColor = System.Drawing.SystemColors.Control
        Me.protocolComboBox1.Location = New System.Drawing.Point(70, 11)
        Me.protocolComboBox1.Size = New System.Drawing.Size(83, 21)
        Me.protocolComboBox1.Text = "ProtocolComboBox1"
        '
        'FormPutGuiVb
        '
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.protocolComboBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.statusLabel)
        Me.Controls.Add(Me.cancelButton1)
        Me.Controls.Add(Me.progressBar1)
        Me.Controls.Add(Me.browseButton)
        Me.Menu = Me.MainMenu1
        Me.MinimizeBox = False
        Me.Text = "PutGuiVb CFv1"

    End Sub

#End Region

    Private m_workings As Workings

    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles browseButton.Click
        m_workings.button1_Click(sender, e)
    End Sub
    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelButton1.Click
        m_workings.cancelButton_Click(sender, e)
    End Sub
End Class
