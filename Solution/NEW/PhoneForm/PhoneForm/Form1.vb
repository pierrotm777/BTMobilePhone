Imports Microsoft.Win32


Public Class Form1
    Public WithEvents bluetooth As BT
    Dim WithEvents tmr As New Timer


    Public Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf PowerModeChanged
        Try
            'Dim Mac As String = "00:19:63:48:E7:DA"
            'Dim MacSplit() As String = Mac.Split(CChar(":"))
            'Dim ADDRS(6) As Byte
            'For i As Integer = 5 To 0 Step -1
            '    ADDRS(i) = CByte("&H" & MacSplit(i))
            '    'MessageBox.Show(ADDRS(i))
            'Next
            bluetooth = New BT()
        Catch ex As Exception

        End Try
        tmr.Start()
        tmr.Enabled() = True
        tmr.Interval = 5000

    End Sub

    'Private Sub PowerModeChanged(ByVal sender As System.Object, ByVal e As Microsoft.Win32.PowerModeChangedEventArgs)
    '    Select Case e.Mode
    '        Case Microsoft.Win32.PowerModes.Resume
    '        Case Microsoft.Win32.PowerModes.Suspend
    '            'goodnite windows
    '    End Select
    'End Sub

    Private Sub Timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmr.Tick
        txtBattery.Text = bluetooth.batteryLevel
        txtSignal.Text = bluetooth.signalStrength
        ProgressBarBattery.Value = bluetooth.batteryLevel
        ProgressBarSignal.Value = bluetooth.signalStrength
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        bluetooth.unload()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtPhoneNum.TextLength = txtPhoneNum.MaxLength Then
        Else
            txtPhoneNum.AppendText("1")
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        txtPhoneNum.AppendText("2")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        txtPhoneNum.AppendText("3")
    End Sub


    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        txtPhoneNum.Clear()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        txtPhoneNum.AppendText("4")
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        txtPhoneNum.AppendText("5")
    End Sub


    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        txtPhoneNum.AppendText("6")
    End Sub


    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        txtPhoneNum.AppendText("7")
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        txtPhoneNum.AppendText("8")
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        txtPhoneNum.AppendText("9")
    End Sub


    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        txtPhoneNum.AppendText("0")
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        bluetooth.dial(txtPhoneNum.Text)
    End Sub


End Class
