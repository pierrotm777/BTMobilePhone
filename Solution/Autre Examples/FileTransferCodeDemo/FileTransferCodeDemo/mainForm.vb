Imports Dreamworld.FileTransfer.SDK

Class MainForm
    Private WithEvents mMonitor As New PhoneMonitor
    Private ap As PluginServices.AvailablePlugin()
    Private WithEvents phone As IPhonePlugIn

    Private Sub MonitorHandler(ByVal portName As String, ByVal PhoneID As String) Handles mMonitor.PhoneConnected
        Me.Invoke(New RefreshTextDelegate(AddressOf RefreshText), portName, PhoneID)
    End Sub

    Private Delegate Sub RefreshTextDelegate(ByVal portName As String, ByVal phoneID As String)

    Private Sub RefreshText(ByVal portName As String, ByVal phoneID As String)
        Me.Text = portName & " " & phoneID
        mMonitor.EndMonitor()
        Application.DoEvents()
        Dim pluginToUse As Integer = PluginServices.SelectPlugin(phoneID, ap)
        txtPlugin.Text = ap(pluginToUse).ClassName
        phone = CType(PluginServices.CreateInstance(ap(pluginToUse)), IPhonePlugIn)
        AddHandler phone.SendComplete, AddressOf CompleteHandler

        txtDest.Text = IO.Path.Combine(phone.DefaultDescFilePath, IO.Path.GetFileName(txtSrc.Text))
        phone.Port = New IO.Ports.SerialPort(portName, 115200)
        phone.Init()
    End Sub

    Private Sub ProgressHandler(ByVal progress As Double) Handles phone.SendingProgress
        Me.Invoke(New RefreshProgressDelegate(AddressOf RefreshProgress), progress)
    End Sub

    Private Delegate Sub RefreshProgressDelegate(ByVal progress As Double)
    Private Sub RefreshProgress(ByVal progress As Double)
        sendProgress.Value = progress * 100
    End Sub

    Private Sub CompleteHandler(ByVal fileName As String, ByVal success As Boolean)
        MsgBox(String.Format("{0} complete: {1}", fileName, success))
    End Sub

    Private Sub Demo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        mMonitor.EndMonitor()
        If phone IsNot Nothing Then
            phone.Close()
        End If
    End Sub

    Private Sub Demo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mMonitor.StartMonitor()
        ap = PluginServices.FindPlugins(Application.StartupPath, "Dreamworld.FileTransfer.SDK.IPhonePlugIn")
        For Each aAp As PluginServices.AvailablePlugin In ap
            lstPlugins.Items.Add(aAp.ClassName)
        Next
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        phone.SendFile(txtSrc.Text, txtDest.Text)
    End Sub

    Private Sub btnBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowser.Click
        Dim br As New OpenFileDialog()
        br.Multiselect = False
        If br.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtSrc.Text = br.FileName
        End If
    End Sub

    Private Sub txtSrc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSrc.TextChanged
        If IO.Path.GetFileName(txtDest.Text) = "" Then
            txtDest.Text = IO.Path.Combine(phone.DefaultDescFilePath, IO.Path.GetFileName(txtSrc.Text))
        End If
    End Sub
End Class
