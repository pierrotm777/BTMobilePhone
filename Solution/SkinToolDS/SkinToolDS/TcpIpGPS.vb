'*********************************************************
'this class return if a TCP/IP GpsGate connection is valid
'*********************************************************

Imports System.IO
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading


Public Class TcpIpGPS
    Private Shared isGpsValid As Boolean = False
    Private Shared myThread As New Thread(New ThreadStart(AddressOf TcpIpGPS.func))

    Shared Sub New()
        TcpIpGPS.myThread.Start()
    End Sub

    Private Shared Sub func()
        Dim info As String = "20176" 'SDK.GetInfo("=$GPS_TCP_port$")
        Dim port As Integer = If(info Is Nothing OrElse info = "", 20176, CInt(Convert.ToInt16(info)))
        While True
            Dim flag As Boolean = False
            Dim tcpClient As New TcpClient()
            Try
                tcpClient.Connect("127.0.0.1", port)
                If tcpClient.Connected Then
                    flag = True
                    'Dim streamReader As New StreamReader(DirectCast(tcpClient.GetStream(), Stream), Encoding.ASCII)
                    'StreamReader.BaseStream.ReadTimeout = 1100
                    'Dim str1 As String = ""
                    'While (str1 = streamReader.ReadLine())
                    '    If str1.Length > 0 AndAlso str1.Substring(0, 1) = "$" Then
                    '        Dim str2 As String = str1.ToUpper()
                    '        Dim chArray As Char() = New Char(0) {}
                    '        Dim index As Integer = 0
                    '        Dim num As Integer = 44
                    '        chArray(index) = CChar(num.ToString)
                    '        Dim strArray As String() = str2.Split(chArray)
                    '        Dim str3 As String = strArray(0).Substring(3)
                    '        If Not (str3 = "GGA") Then
                    '            If str3 = "RMC" Then
                    '                flag = strArray(2) = "A"
                    '            End If
                    '        Else
                    '            flag = strArray(4) = "1" OrElse strArray(4) = "2"
                    '        End If
                    '        If flag Then
                    '            Exit While
                    '        End If
                    '    End If
                    'End While
                    'streamReader.Close()
                End If
            Catch ex As Exception
            End Try
            TcpIpGPS.isGpsValid = flag
            tcpClient.Close()
        End While
    End Sub

    Public Shared Function CheckGPS() As Boolean
        Return TcpIpGPS.isGpsValid
    End Function

    Public Shared Sub CloseCheckGPS()
        If TcpIpGPS.myThread.IsAlive = True Then TcpIpGPS.myThread.Abort()
    End Sub

End Class
