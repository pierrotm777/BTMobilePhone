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

Imports Dreamworld.FileTransfer.SDK
Imports System.IO
Imports System.Threading

Namespace Dreamworld.FileTransfer.Phone
    Public Class Samsung
        Implements FileTransfer.SDK.IPhonePlugIn

        Private mPort As Ports.SerialPort
        Private mSendFileThread As Thread
        Private mCMDTimeOut As Integer = 2000
        Private WithEvents sendFileInstance As SendFileClass
        Private SAMSUNG_PICTURE As String = "/ͼƬ"


        Public Function Close() As Boolean Implements FileTransfer.SDK.IPhonePlugIn.Close
            mPort.Close()
        End Function

        Public Function Init() As Boolean Implements FileTransfer.SDK.IPhonePlugIn.Init
            mPort.NewLine = Chr(&HD)
            mPort.ReadTimeout = 2000
            mPort.Encoding = Text.Encoding.UTF8
            SendCMD("AT+FSIF")
        End Function

        Private Enum ResponseType
            OK
            [ERROR]
        End Enum

        Private Function SendCMD(ByVal cmd) As ResponseType
            mPort.WriteLine(cmd)
            Dim start As Date = Now
            Dim rsp As String = String.Empty
            Do
                Thread.Sleep(500)
                rsp += mPort.ReadExisting().ToUpper
                If rsp.Contains("OK") Then Return ResponseType.OK
                If rsp.Contains("#OK#" & vbCrLf) Then
                    mPort.WriteLine("##>")
                End If
                If rsp.Contains("ERROR") Then Return ResponseType.ERROR
                If Now.Subtract(start).TotalMilliseconds > mCMDTimeOut Then Throw New Exception("command time out")
            Loop
        End Function

        Public Property Port() As System.IO.Ports.SerialPort Implements IPhonePlugIn.Port
            Get
                Return mPort
            End Get
            Set(ByVal value As System.IO.Ports.SerialPort)
                mPort = value
                mPort.Open()
                mPort.NewLine = Chr(&HD)
                mPort.RtsEnable = True
                mPort.DtrEnable = True
            End Set
        End Property

        Public Event SendComplete(ByVal fileName As String, ByVal success As Boolean) Implements IPhonePlugIn.SendComplete

        Public Sub SendProgressEventHandler(ByVal progress As Double) Handles sendFileInstance.SendProgress
            RaiseEvent SendingProgress(progress)
            If progress = 1 Then
                RaiseEvent SendComplete(sendFileInstance.srcFilePath, True)
            End If
        End Sub


        Private Class SendFileClass
            Public srcFilePath As String
            Public desFilePath As String
            Public WithEvents mPort As IO.Ports.SerialPort
            Public Event SendProgress(ByVal progress As Double)
            Private mSendProgress As Thread
            Private mATTimeOut As Integer = 5000
            Private fs As FileStream
            Public Sub Start()

                fs = New FileStream(srcFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)

                Dim crc As New Dreamworld.Tools.CRC32
                Dim crcNum As Integer = crc.GetCrc32(fs)
                Dim crcNumDec As String
                If crcNum < 0 Then
                    crcNumDec = (&H100000000 + crcNum).ToString
                Else
                    crcNumDec = crcNum
                End If
                fs.Position = 0

                mPort.WriteLine(String.Format("AT+FSCD=""/"""))
                Thread.Sleep(500)
                mPort.ReadExisting()

                Dim desFileDir As String = Path.GetDirectoryName(desFilePath)

                mPort.WriteLine(String.Format("AT+FSDI=""{0}""", desFileDir))
                Thread.Sleep(500)
                mPort.ReadExisting()


                mPort.WriteLine(String.Format("AT+FSCD=""{0}""", desFileDir))
                Thread.Sleep(500)
                mPort.ReadExisting()

                mPort.WriteLine(String.Format("AT+FSCD=""{0}""", desFileDir))
                Thread.Sleep(500)
                mPort.ReadExisting()


                Dim piece As Integer = Fix((fs.Length + 512 - 1) / 512)
                Dim lastPacketSize As Integer = fs.Length - (piece - 1) * 512
                Dim rsp As String

                'mPort.WriteLine(String.Format("AT+FSFE=0,""{0}""", IO.Path.GetFileName(fs.Name)))
                'Thread.Sleep(1000)
                'mPort.ReadExisting()

                mPort.WriteLine((String.Format("AT+FSFW=-1,""{0}"",0,"""",{1},{2}", IO.Path.GetFileName(desFileDir), fs.Length, crcNumDec)))
                Thread.Sleep(1000)
                rsp = mPort.ReadLine
                mPort.ReadExisting()

                Dim buff(511) As Byte
                For i As Integer = 0 To piece - 2
                    fs.Read(buff, 0, buff.Length)
                    RaiseEvent SendProgress(fs.Position / fs.Length)
                    mPort.Write(buff, 0, buff.Length)
                    Thread.Sleep(20)
                    rsp = mPort.ReadLine()
                    mPort.ReadExisting()
                Next
                ReDim buff(lastPacketSize - 1)
                fs.Read(buff, 0, buff.Length)
                mPort.Write(buff, 0, buff.Length)
                Thread.Sleep(1000)
                mPort.ReadLine()
                mPort.ReadExisting()
                fs.Close()
                RaiseEvent SendProgress(1)
            End Sub
        End Class

        Public Sub SendFile(ByVal srcFilePath As String, ByVal desFilePath As String) Implements SDK.IPhonePlugIn.SendFile
            sendFileInstance = New SendFileClass
            sendFileInstance.srcFilePath = srcFilePath
            sendFileInstance.mPort = Me.mPort
            sendFileInstance.desFilePath = desFilePath

            mSendFileThread = New Thread(AddressOf sendFileInstance.Start)
            mSendFileThread.Priority = ThreadPriority.AboveNormal
            mSendFileThread.Start()
        End Sub

        Public Event SendingProgress(ByVal progress As Double) Implements SDK.IPhonePlugIn.SendingProgress

        Public ReadOnly Property SupportModelID() As String() Implements SDK.IPhonePlugIn.SupportModelID
            Get
                Return New String() {"SGH-X108"}
            End Get
        End Property

        Public ReadOnly Property DefaultDescFilePath() As String Implements SDK.IPhonePlugIn.DefaultDescFilePath
            Get
                Return "/ͼƬ"
            End Get
        End Property
    End Class
End Namespace