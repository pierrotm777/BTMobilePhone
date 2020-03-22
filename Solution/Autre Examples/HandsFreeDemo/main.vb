Public Class fmMain

    Private WithEvents wclAPI As wcl.wclAPI
    Private WithEvents wclBluetoothDiscovery As wcl.wclBluetoothDiscovery
    Private WithEvents wclBluetoothHandsFreeClient As wcl.wclBluetoothHandsFreeClient

    Private Sub fmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        wclAPI.Load()
    End Sub

    Private Sub fmMain_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        wclAPI.Unload()

        wclBluetoothDiscovery.Dispose()
        wclBluetoothHandsFreeClient.Dispose()
        wclAPI.Dispose()
    End Sub

    Private Sub btDiscover_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btDiscover.Click
        Dim Radios As wcl.wclBluetoothRadios = New wcl.wclBluetoothRadios()
        If Not wcl.wclErrors.wclShowError(wclBluetoothDiscovery.EnumRadios(Radios)) Then
            If Radios.Count = 0 Then
                MessageBox.Show("No Bluetooth Radio Found")
            Else
                wcl.wclErrors.wclShowError(wclBluetoothDiscovery.Discovery(Radios(0)))
            End If
        End If
    End Sub

    Private Sub wclBluetoothDiscovery_OnDiscoveryStarted(ByVal sender As System.Object, ByVal e As wcl.wclBluetoothDiscoveryStartedEventArgs) Handles wclBluetoothDiscovery.OnDiscoveryStarted
        lvDevices.Items.Clear()
        lbLog.Items.Add("Discovering...")
    End Sub

    Private Sub wclBluetoothDiscovery_OnDiscoveryComplete(ByVal sender As System.Object, ByVal e As wcl.wclBluetoothDiscoveryCompleteEventArgs) Handles wclBluetoothDiscovery.OnDiscoveryComplete
        If TypeName(e.Devices) = TypeName(Nothing) Then
            lbLog.Items.Add("Discovering complete with error")
        Else
            If e.Devices.Count = 0 Then
                lbLog.Items.Add("Nothing found")
            Else
                lbLog.Items.Add("Discovering complete")
                Dim I As Integer
                For I = 0 To e.Devices.Count - 1
                    Dim Item As ListViewItem = lvDevices.Items.Add(e.Devices(I).Address)
                    Dim aName As String = ""
                    e.Devices(I).GetName(e.Radio, aName)
                    Item.SubItems.Add(aName)
                Next I
            End If
        End If
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnConnect(ByVal sender As System.Object, ByVal e As wcl.wclConnectEventArgs) Handles wclBluetoothHandsFreeClient.OnConnect
        If e.Error = wcl.wclErrors.WCL_E_SUCCESS Then
            lbLog.Items.Add("Connected")
        Else
            lbLog.Items.Add("Connect error: " + wcl.wclErrors.wclGetErrorMessage(e.Error))
        End If
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnDisconnect(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnDisconnect
        lbLog.Items.Add("Disconnected")
    End Sub

    Private Sub btDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btDisconnect.Click
        wclBluetoothHandsFreeClient.Disconnect()
    End Sub

    Private Sub btConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btConnect.Click
        Dim Radios As wcl.wclBluetoothRadios = New wcl.wclBluetoothRadios()
        If Not wcl.wclErrors.wclShowError(wclBluetoothDiscovery.EnumRadios(Radios)) Then
            If Radios.Count = 0 Then
                MessageBox.Show("No Bluetooth Radio Found")
            Else
                If lvDevices.SelectedItems.Count = 0 Then
                    MessageBox.Show("Selecte device")
                Else
                    wclBluetoothHandsFreeClient.Radio = Radios(0)
                    wclBluetoothHandsFreeClient.Address = lvDevices.SelectedItems(0).Text
                    wcl.wclErrors.wclShowError(wclBluetoothHandsFreeClient.Connect())
                End If
            End If
        End If
    End Sub

    Private Sub btCallNumber_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCallNumber.Click
        wcl.wclErrors.wclShowError(wclBluetoothHandsFreeClient.CallNumber(edNumber.Text))
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnStandBy(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnStandBy
        lbLog.Items.Add("Standby")
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnOperator(ByVal sender As System.Object, ByVal e As wcl.wclHFOperatorEventArgs) Handles wclBluetoothHandsFreeClient.OnOperator
        lbLog.Items.Add("Operator: " + e.OperatorName)
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnAudioConnect(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnAudioConnect
        lbLog.Items.Add("Audio connection established")
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnAudioDisconnect(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnAudioDisconnect
        lbLog.Items.Add("Audio connection released")
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnOutgoingCall(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnOutgoingCall
        lbLog.Items.Add("Outgoing call")
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnOngoingCall(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnOngoingCall
        lbLog.Items.Add("Ongoing call")
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnRinging(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnRinging
        lbLog.Items.Add("Ringing")
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnBattery(ByVal sender As System.Object, ByVal e As wcl.wclHFBatteryEventArgs) Handles wclBluetoothHandsFreeClient.OnBattery
        lbLog.Items.Add("Battery level: " + e.Level.ToString())
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnNetworkAvailable(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnNetworkAvailable
        lbLog.Items.Add("Network is available")
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnNetworkUnavailable(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wclBluetoothHandsFreeClient.OnNetworkUnavailable
        lbLog.Items.Add("Network is unavailable")
    End Sub

    Private Sub wclBluetoothHandsFreeClient_OnCLIP(ByVal sender As System.Object, ByVal e As wcl.wclHFCLIPEventArgs) Handles wclBluetoothHandsFreeClient.OnCLIP
        lbLog.Items.Add("CLIP Caller number: " + e.CallerNumber + " caller name: " + e.CallerName)
    End Sub

    Private Sub btGetManu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btGetManu.Click
        Dim s As String = ""
        If Not wcl.wclErrors.wclShowError(wclBluetoothHandsFreeClient.GetManufacturerID(s)) Then
            lbLog.Items.Add("Manufacturer: " + s)
        End If
    End Sub

    Private Sub btGetModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btGetModel.Click
        Dim s As String = ""
        If Not wcl.wclErrors.wclShowError(wclBluetoothHandsFreeClient.GetModelID(s)) Then
            lbLog.Items.Add("Model: " + s)
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        wclAPI = New wcl.wclAPI
        wclBluetoothDiscovery = New wcl.wclBluetoothDiscovery
        wclBluetoothHandsFreeClient = New wcl.wclBluetoothHandsFreeClient
    End Sub
End Class
