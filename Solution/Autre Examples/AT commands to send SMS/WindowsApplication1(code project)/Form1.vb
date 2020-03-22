Imports System
Imports System.Threading
Imports System.ComponentModel
Imports System.IO.Ports

Public Class Form1
    'connect your mobile/GSM modem to PC,
    'then go in device manager and check under ports which COM port has been slected
    'if say com1 is there then put com2 in following statement
    Dim SMSEngine As New SMSCOMMS("COM29")
    Dim i As Integer
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        SMSEngine.Open() 'open the port
        SMSEngine.SendSMS() 'send the SMS

    End Sub

    Private Sub TextBox2_TextChanged(sender As System.Object, e As System.EventArgs)

    End Sub
End Class
Public Class SMSCOMMS
    Private WithEvents SMSPort As SerialPort
    Private SMSThread As Thread
    Private ReadThread As Thread
    Shared _Continue As Boolean = False
    Shared _ContSMS As Boolean = False
    Private _Wait As Boolean = False
    Shared _ReadPort As Boolean = False
    Public Event Sending(ByVal Done As Boolean)
    Public Event DataReceived(ByVal Message As String)

    Public Sub New(ByRef COMMPORT As String)
        'initialize all values
        SMSPort = New SerialPort
        With SMSPort
            .PortName = COMMPORT
            .BaudRate = 19200
            .Parity = Parity.None
            .DataBits = 8
            .StopBits = StopBits.One
            .Handshake = Handshake.RequestToSend
            .DtrEnable = True
            .RtsEnable = True
            .NewLine = vbCrLf
        End With
    End Sub
    Public Function SendSMS() As Boolean
        If SMSPort.IsOpen = True Then
            'sending AT commands
            SMSPort.WriteLine("AT")
            SMSPort.WriteLine("AT+CMGF=0" & vbCrLf) 'set command message format to text mode(1)
            SMSPort.WriteLine("AT+CSCA=" & Chr(34) & "+33695000695" & Chr(34) & vbCrLf) 'set service center address (which varies for service providers (idea, airtel))
            SMSPort.WriteLine("AT+CMGS=" & Chr(34) & "+33671951288" & Chr(34) & vbCrLf) ' enter the mobile number whom you want to send the SMS
            _ContSMS = False
            SMSPort.WriteLine("Ceci est un test" & vbCrLf & Chr(26)) 'SMS sending
            MessageBox.Show(":send")
            SMSPort.Close()
        End If
    End Function

    Public Sub Open()
        If Not (SMSPort.IsOpen = True) Then
            SMSPort.Open()
        End If
    End Sub

    Public Sub Close()
        If SMSPort.IsOpen = True Then
            SMSPort.Close()
        End If
    End Sub
End Class
