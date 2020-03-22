'Copyright (c) 2005,
'   HeSicong of UESTC, Dreamworld Site(http://www.hesicong.com), All rights reserved.

'Redistribution and use in source and binary forms, with or without modification, 
'are permitted provided that the following conditions are met:

'1.Redistributions of source code must retain the above copyright notice, this list
'  of conditions and the following disclaimer. 
'2.Redistributions in binary form must reproduce the above copyright notice, this
'  list of conditions and the following disclaimer in the documentation and/or other 
'  materials provided with the distribution. 
'3.Neither the name of the Dreamworld nor the names of its contributors may be 
'  used to endorse or promote products derived from this software without specific prior
'  written permission. 
'  
'THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS
'OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
'AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
'CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
'DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
'DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
'IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY 
'OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Namespace Dreamworld.FileTransfer.SDK
    Public Class PhoneMonitor
        Private mPort As New SerialPort
        Private mSerialPortMonitor As Thread
        Private mUSBMonitor As Thread
        Private mIrDAMonitor As Thread

        Private mStopMonitor As Boolean
        Private mATTimeOut As Integer = 1000

        Public Event PhoneConnected(ByVal portName As String, ByVal phoneID As String)

        Public Sub StartMonitor()
            mSerialPortMonitor = New Thread(AddressOf SerialPortMonitorThread)
            mSerialPortMonitor.Start()
        End Sub

        Public Sub EndMonitor()
            mStopMonitor = True
            If mPort IsNot Nothing And mPort.IsOpen Then mPort.Close()
        End Sub

#Region "SerialPortMonitor"
        Private Sub SerialPortMonitorThread()
            Dim response As String
            Do Until mStopMonitor
                Dim availablePorts As String() = IO.Ports.SerialPort.GetPortNames
                For Each port As String In availablePorts
                    Try
#If DEBUG Then
                        Console.WriteLine("Try to connect on {0}:", port)
#End If
                        'Fix a bug in .Net Framework 2.0
                        port = "COM" & CInt(Val(port.Substring(3)))
                        mPort.PortName = port
                        mPort.Open()
                        mPort.BaudRate = 115200
                        mPort.DtrEnable = True
                        mPort.RtsEnable = True

                        Thread.Sleep(100)
                        response = SendAT("AT")
                        If response.Contains("OK") Then
                            Dim modelID As String = Identify()
                            mPort.Close()
                            RaiseEvent PhoneConnected(mPort.PortName, modelID)
                        End If
                    Catch e As Exception
                        If mPort IsNot Nothing AndAlso mPort.IsOpen = True Then
                            mPort.Close()
                        End If
                    End Try
                Next
            Loop
        End Sub

        Private Function Identify() As String
            SendAT("ATE0")
            Dim response As String = SendAT("AT+CGMM")
            response = response.Replace("AT+CGMM", "")
            response = response.Replace(Chr(13), "")
            response = response.Replace(Chr(10), "")
            response = response.Replace("OK", "")
            Return response
        End Function

        Private Function SendAT(ByVal cmd As String) As String
            mPort.NewLine = Chr(13)
            mPort.WriteLine(cmd)
            Dim response As New Text.StringBuilder
            Console.ForegroundColor = ConsoleColor.White
            Dim start As Date = Now
            Do
                Thread.Sleep(10)
                Dim rsp As String = mPort.ReadExisting
                response.Append(rsp)
                If Now.Subtract(start).TotalMilliseconds > mATTimeOut Then Throw New TimeoutException("AT TimeOut")
                If response.ToString.Contains("OK") Or response.ToString.Contains("ERROR") Then Exit Do
            Loop
            Return response.ToString
        End Function
#End Region
    End Class
End Namespace