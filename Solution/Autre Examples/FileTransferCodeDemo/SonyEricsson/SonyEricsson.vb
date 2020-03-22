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
    Public Class SonyEricsson
        Implements IPhonePlugIn

        Private mPort As IO.Ports.SerialPort
        Private WithEvents mOBEX As Dreamworld.Protocol.OBEXClient.Command
        Private mSendFileThread As Thread
        Private SONY_ERICSSON_PICTURE As String = "Õº∆¨"
        Private SONY_ERICSSON_SOUND As String = "¡Â…˘"

        Public Function Close() As Boolean Implements IPhonePlugIn.Close
            mOBEX.Disconnect()
            mOBEX.ExitOBEX()
            mPort.Close()
        End Function

        Public Function Init() As Boolean Implements IPhonePlugIn.Init
            mOBEX = New SonyEricssonOBEX(mPort)
            mOBEX.MaxClientPacketSize = 400
            mOBEX.EnterOBEX()
            mOBEX.ConnectToFolderListingService()
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

        Private Sub InvokeSendCompleteEvent(ByVal fileName As String, ByVal success As Boolean)
            RaiseEvent SendingProgress(1)
            RaiseEvent SendComplete(fileName, success)
        End Sub

        Private Class SendFileClass
            Public srcFilePath As String
            Public desFilePath As String
            Public WithEvents mySelf As SonyEricsson
            Public Event SendProgress(ByVal progress As Double)
            Private mSendProgress As Thread
            Private fs As FileStream
            Public Sub Start()
                fs = New FileStream(srcFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                mSendProgress = New Thread(AddressOf SendProgressThread)
                mSendProgress.Start()
                Dim desFileDir As String = IO.Path.GetDirectoryName(desFilePath)
                mySelf.mOBEX.SetPath(desFileDir)
                mySelf.mOBEX.SendFile(fs, fs.Length, desFileDir, Path.GetFileName(desFilePath))
                mSendProgress.Abort()
                fs.Close()
                mySelf.InvokeSendCompleteEvent(srcFilePath, True)
            End Sub

            Private Sub SendProgressThread()
                Do
                    Thread.Sleep(100)
                    RaiseEvent SendProgress(mySelf.mOBEX.TotalByteSend / fs.Length)
                Loop
            End Sub
        End Class
        Private WithEvents sendFileInstance As SendFileClass

        Private Sub RefreshSendingProgressHandler(ByVal progress As Double) Handles sendFileInstance.SendProgress
            RaiseEvent SendingProgress(progress)
        End Sub

        Public Sub SendFile(ByVal srcFilePath As String, ByVal desFilePath As String) Implements IPhonePlugIn.SendFile
            sendFileInstance = New SendFileClass
            sendFileInstance.srcFilePath = srcFilePath
            sendFileInstance.mySelf = Me
            sendFileInstance.desFilePath = desFilePath
            mSendFileThread = New Thread(AddressOf sendFileInstance.Start)
            mSendFileThread.Priority = ThreadPriority.AboveNormal
            mSendFileThread.Start()
        End Sub

        Public Event SendingProgress(ByVal progress As Double) Implements IPhonePlugIn.SendingProgress

        Public ReadOnly Property SupportModelID() As String() Implements IPhonePlugIn.SupportModelID
            Get
                Return New String() {"AAB-1021011-CN", _
                                     "AAB-1021044-CN"}
                'AAB-1021011-CN   T618
                'AAB-1021044-CN   K508
            End Get
        End Property

        Public Class SonyEricssonOBEX
            Inherits Dreamworld.Protocol.OBEXClient.Command

            Sub New(ByVal mPort As IO.Ports.SerialPort)
                MyBase.New(mPort)
            End Sub

            Public Overrides Function EnterOBEX() As Boolean
                mPort.NewLine = Chr(&HD)
                mPort.ReadTimeout = 500
                mPort.WriteLine("AT")
                Thread.Sleep(500)
                mPort.WriteLine("AT")
                Thread.Sleep(500)
                mPort.ReadExisting()
                mPort.WriteLine("ATE0")
                Thread.Sleep(1000)
                mPort.ReadExisting()

                mPort.WriteLine("AT*EOBEX")
                Thread.Sleep(300)
                mPort.ReadExisting()
            End Function

            Public Overrides Function ExitOBEX() As Boolean

            End Function
        End Class

        Public ReadOnly Property DefaultDescFilePath() As String Implements SDK.IPhonePlugIn.DefaultDescFilePath
            Get
                Return "Õº∆¨"
            End Get
        End Property
    End Class
End Namespace