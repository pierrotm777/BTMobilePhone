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


Imports System.IO.Ports
Imports System.io
Imports System.Text
Imports System.Threading
Imports Dreamworld.FileTransfer.SDK

Namespace Dreamworld.FileTransfer.Phone
    Public Class NokiaS40
        Implements IPhonePlugIn

        Private FBUS_FILE_NAME_HEADER As Byte() = New Byte() {&H0, &H1, &H0, &H72, &H11, &H2}
        Private FBUS_FILE_DATA_HEADER As Byte() = New Byte() {&H0, &H1, &H1, &H58, &H0, &H0, &H0, &H0, &H0, &H1, &H0, &H0}
        Private FBUS_FILE_SEND_COMPLETE As Byte() = New Byte() {&H0, &H1, &H0, &H74, &H0, &H0, &H0, &H0, &H0, &H1}
        Private FBUS_FILE_MAX_PACKET_LENGTH As Integer = 1000

        Public DES_PICTURE_PATH As String = "a:/predefgallery/predefphotos/"
        Public DES_SOUND_PATH As String = "a:/predefgallery/predeftones/"

        Private fbus As Dreamworld.FileTransfer.Phone.Link.FBUS
        Private FBUS_TIMEOUT As Integer = 500
        Private mPort As SerialPort

        Private mProgress As Double
        Public Event SendComplete(ByVal fileName As String, ByVal success As Boolean) Implements IPhonePlugIn.SendComplete
        Public Event SendingProgress(ByVal progress As Double) Implements IPhonePlugIn.SendingProgress

        Public Enum FileType
            PICTURE
            SOUND
        End Enum

        Public Function Init() As Boolean Implements IPhonePlugIn.Init
            fbus.Init()
        End Function

        Private Function privateSendFile(ByVal srcFilePath As String, ByVal desFilePath As String) As Boolean
            Dim fileName As String = Path.GetFileName(desFilePath)
            fbus.ClearInputBuffer()
            Prepare()

            'Prepare file transfer
            Dim start As Date = Now

            desFilePath = desFilePath & Chr(0)

            Dim desFileNameByte As Byte() = Encoding.BigEndianUnicode.GetBytes(desFilePath)
            Dim dscFileNameLength As Integer = desFileNameByte.Length
            'Send file name and prepare to send
            Dim toSend(desFileNameByte.Length + 9 - 1) As Byte
            Dim curPos As Integer = 0
            Array.Copy(FBUS_FILE_NAME_HEADER, toSend, FBUS_FILE_NAME_HEADER.Length)
            curPos += FBUS_FILE_NAME_HEADER.Length
            toSend(7) = dscFileNameLength
            Array.Copy(desFileNameByte, 0, toSend, 8, desFileNameByte.Length)
            SendData(&H6D, toSend)

            'Start sending data
            Dim fs As New FileStream(srcFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
            Dim br As New BinaryReader(fs)
            Dim piece As Integer = Fix((fs.Length + FBUS_FILE_MAX_PACKET_LENGTH - 1) / FBUS_FILE_MAX_PACKET_LENGTH)
            Dim lastPacketSize As Integer = fs.Length - (piece - 1) * FBUS_FILE_MAX_PACKET_LENGTH
            Dim fileData() As Byte
#If DEBUG Then
            Console.WriteLine("Total packets to send:{0}", piece)
#End If

            For i As Integer = 0 To piece - 2
#If DEBUG Then
                Console.WriteLine("Sending {0} Percent:{1:P}", i, fs.Position / fs.Length)
#End If
                mProgress = fs.Position / fs.Length

                RaiseEvent SendingProgress(mProgress)

                curPos = 0
                ReDim toSend(FBUS_FILE_MAX_PACKET_LENGTH + 14 - 1)
                Array.Copy(FBUS_FILE_DATA_HEADER, toSend, FBUS_FILE_DATA_HEADER.Length) : curPos += FBUS_FILE_DATA_HEADER.Length
                toSend(curPos) = &H3 : curPos += 1
                toSend(curPos) = &HE8 : curPos += 1
                fileData = br.ReadBytes(FBUS_FILE_MAX_PACKET_LENGTH)
                Array.Copy(fileData, 0, toSend, curPos, fileData.Length)
                fbus.Send(&H6D, toSend)
                fbus.ReadFrame()
                Thread.Sleep(300)
                fbus.ReadFrame()
                fbus.SendAck(&H6D)
                fbus.ReadFrame()
            Next
            'Send last packet

#If DEBUG Then
            Console.WriteLine("Send Last Packet")
#End If

            curPos = 0
            ReDim toSend(lastPacketSize + 14 - 1)
            Array.Copy(FBUS_FILE_DATA_HEADER, toSend, FBUS_FILE_DATA_HEADER.Length)
            curPos += FBUS_FILE_DATA_HEADER.Length
            toSend(curPos) = CByte((lastPacketSize And &HFF00) >> 8) : curPos += 1
            toSend(curPos) = CByte(lastPacketSize And &HFF) : curPos += 1
            fileData = br.ReadBytes(lastPacketSize)
            Array.Copy(fileData, 0, toSend, curPos, fileData.Length)
            fbus.Send(&H6D, toSend)
            fbus.ReadFrame()
            Thread.Sleep(500)
            fbus.ReadFrame()
            fbus.SendAck(&H6D)
            'Finish
            Thread.Sleep(300)
            fbus.ClearInputBuffer()
            Dim lastFrame As Dreamworld.FileTransfer.Phone.Link.FBUS.Frame = SendData(&H6D, FBUS_FILE_SEND_COMPLETE)
            fbus.ReadFrame()
            fbus.ReadFrame()
            mProgress = 1

            RaiseEvent SendingProgress(mProgress)

#If DEBUG Then
            Console.WriteLine("End")
            Dim secondsTotal As Integer = Now.Subtract(start).TotalSeconds
            Console.WriteLine("Time:{0}, Speed:{1}", secondsTotal, Fix(br.BaseStream.Length / secondsTotal) / 1000)
            'Console.ReadLine()
#End If
            fs.Close()
            RaiseEvent SendComplete(srcFilePath, True)
        End Function

        Private Sub Prepare()
            Dim FBUS_INIT_1 As Byte() = New Byte() {&H0, &H1, &H0, &HB, &H0, &H2, &H0, &H0, &H0}
            SendData(&HA, FBUS_INIT_1)

            SendData(&HA, FBUS_INIT_1)

            Dim FBUS_INIT_2 As Byte() = New Byte() {0, &H1, &H0, &H0, &H41, &H1}
            SendData(&H1B, FBUS_INIT_2)

            Dim FBUS_INIT_3 As Byte() = New Byte() {0, &H1, &H0, &HA, &H2, &H0}
            SendData(&H17, FBUS_INIT_3)
        End Sub

        Private Function SendData(ByVal cmdID As Byte, ByVal data As Byte()) As Dreamworld.FileTransfer.Phone.Link.FBUS.Frame
            Dim start As Date = Now
            Dim frameRead As Dreamworld.FileTransfer.Phone.Link.FBUS.Frame
            With fbus
                .Send(cmdID, data)
                .WaitAck()
                Do
                    Thread.Sleep(10)
                    frameRead = .ReadFrame()
                    If frameRead IsNot Nothing Then Exit Do
                    If Now.Subtract(start).TotalMilliseconds > FBUS_TIMEOUT Then Return Nothing
                Loop
                .SendAck(frameRead.CommandID)
            End With
            Return frameRead
        End Function

        Public Class SendFileThreadClass
            Public s40Plugin As NokiaS40
            Public srcFilePath As String
            Public desFilePath As String
            Public Sub Start()
                s40Plugin.privateSendFile(srcFilePath, desFilePath)
            End Sub
        End Class

        Public Sub SendFile(ByVal srcFilePath As String, ByVal desFilePath As String) Implements IPhonePlugIn.SendFile
            Try
                Dim sendFileClass As New SendFileThreadClass
                With sendFileClass
                    .srcFilePath = srcFilePath
                    .s40Plugin = Me
                    .desFilePath = desFilePath
                End With

                Dim sendFileThread As New Thread(AddressOf sendFileClass.Start)
                sendFileThread.Priority = ThreadPriority.AboveNormal
                sendFileThread.Start()
                '                SendFile(srcFilePath, Path.GetFileName(srcFilePath), fileType)
            Catch e As Exception
                Throw New ApplicationException("Send file failed")
            End Try
        End Sub

        Public Function Close() As Boolean Implements Dreamworld.FileTransfer.SDK.IPhonePlugIn.Close
            mPort.Close()
        End Function

        Public ReadOnly Property SupportModelID() As String() Implements Dreamworld.FileTransfer.SDK.IPhonePlugIn.SupportModelID
            Get
                Return New String() {"Nokia 6021", "Nokia 6020", "Nokia 3100"}
            End Get
        End Property

        Public Property Port() As System.IO.Ports.SerialPort Implements Dreamworld.FileTransfer.SDK.IPhonePlugIn.Port
            Get
                Return mPort
            End Get
            Set(ByVal value As System.IO.Ports.SerialPort)
                mPort = value
                If mPort.IsOpen = False Then
                    mPort.Close()
                End If
                mPort.Open()
                mPort.NewLine = vbCrLf
                fbus = New Dreamworld.FileTransfer.Phone.Link.FBUS(mPort)
            End Set
        End Property

        Public ReadOnly Property DefaultDescFilePath() As String Implements Dreamworld.FileTransfer.SDK.IPhonePlugIn.DefaultDescFilePath
            Get
                Return "a:/predefgallery/predefphotos/"
            End Get
        End Property
    End Class
End Namespace