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
Imports System.Text.Encoding
Imports system.Text
Imports Dreamworld.Protocol.OBEXClient
Imports Dreamworld.Protocol.OBEXClient.Header

Namespace Dreamworld.Protocol.OBEXClient
    Public MustInherit Class Command
        Protected mPort As SerialPort
        Protected mFolderListingServiceUUID As Byte() = {&HF9, &HEC, &H7B, &HC4, &H95, &H3C, &H11, &HD2, &H98, &H4E, &H52, &H54, &H0, &HDC, &H9E, &H9}
        Protected mIrMCServiceUUID As Byte() = ASCII.GetBytes("IRMC-SYNC")
        Protected mConnectionID As Integer = -1
        Protected mMaxClientPacketSize As Integer = 400
        Protected mMaxServerPacketSize As Integer
        Protected mOBEXTimeOut As Integer = 10000 '10s
        Protected mAbort As Boolean   'About current process
        Protected mLastError As PacketDecoder.PacketStructure.EnumResponseCode

        Public TotalByteSend As Integer
        Public TotalByteReceived As Integer

        ''' <summary>
        ''' A serial port to perform OBEX commands.
        ''' </summary>
        ''' <value>A serial port</value>
        ''' <returns>A serial port</returns>
        ''' <remarks></remarks>
        Public Property SerialPort() As SerialPort
            Get
                Return mPort
            End Get
            Set(ByVal value As SerialPort)
                mPort = value
            End Set
        End Property

        ''' <summary>
        ''' Set or Get UUID of Folder Listing Service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FolderListingServiceUUID() As Byte()
            Get
                Return mFolderListingServiceUUID
            End Get
            Set(ByVal value As Byte())
                mFolderListingServiceUUID = value
            End Set
        End Property

        ''' <summary>
        ''' Set or Get UUID of IrMC Service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IrMCServiceUUID() As Byte()
            Get
                Return mIrMCServiceUUID
            End Get
            Set(ByVal value As Byte())
                mIrMCServiceUUID = value
            End Set
        End Property

        ''' <summary>
        ''' Set or Get max packet size a client can handle.Default 400
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MaxClientPacketSize() As Integer
            Get
                Return mMaxClientPacketSize
            End Get
            Set(ByVal value As Integer)
                mMaxClientPacketSize = value
            End Set
        End Property

        ''' <summary>
        ''' Get last error.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Header Value of response code</returns>
        ''' <remarks></remarks>
        Public Property LastError() As PacketDecoder.PacketStructure.EnumResponseCode
            Get
                Return mLastError
            End Get
            Set(ByVal value As PacketDecoder.PacketStructure.EnumResponseCode)
                mLastError = value
            End Set
        End Property

        ''' <summary>
        ''' Get max packet size a client can handle. 
        ''' </summary>
        ''' <value></value>
        ''' <returns>Size returned by the server.</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxServerPacketSize() As Integer
            Get
                Return mMaxServerPacketSize
            End Get
        End Property

        ''' <summary>
        ''' Set or Get timeout of sending OBEX packets. Default 10s
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TimeOut() As Integer
            Get
                Return mOBEXTimeOut
            End Get
            Set(ByVal value As Integer)
                mOBEXTimeOut = value
            End Set
        End Property

        Sub New(ByVal serialPort As SerialPort)
            mPort = serialPort
            mPort.NewLine = vbCrLf
        End Sub

        ''' <summary>
        ''' Connect to OBEX.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function EnterOBEX() As Boolean

        ''' <summary>
        ''' Exit OBEX mode
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function ExitOBEX() As Boolean

        ''' <summary>
        ''' Connect To Folder Listing Service. Default UUID is F9EC7BC4-953C-11d2-984E-525400DC9E09
        ''' </summary>
        ''' <returns>True if success;False if failed.</returns>
        ''' <remarks></remarks>
        Public Overridable Function ConnectToFolderListingService() As Boolean
            Dim conn As New PacketEncoder.ConnectRequestPacket()
            conn.MaxOBEXPacketLength = mMaxClientPacketSize
            conn.AddHeader(New TargetHeader(mFolderListingServiceUUID))

            Dim data As Byte() = Send(conn.ToByte)

            Dim rsp As New PacketDecoder.ConnectionResponseData
            rsp = PacketDecoder.DecodeConnectionResponse(data)

            mMaxServerPacketSize = rsp.MaxOBEXPacketLength

            Dim detail As New PacketDecoder.PacketDetail
            detail = PacketDecoder.GetDetail(rsp.PacketBase)

            mConnectionID = detail.ConnectionID

            If rsp.PacketBase.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                Return True
            Else
                mLastError = rsp.PacketBase.ResponseCode
                Return False
            End If
        End Function

        ''' <summary>
        ''' Connect to IrMC Serivce. Default target UUID is the ASCII code fo string "IRMC-SYNC"
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function ConnectToIrMCSerivce() As Boolean
            Dim conn As New PacketEncoder.ConnectRequestPacket()
            conn.MaxOBEXPacketLength = mMaxClientPacketSize
            conn.AddHeader(New TargetHeader(mIrMCServiceUUID))

            Dim data As Byte() = Send(conn.ToByte)

            Dim rsp As New PacketDecoder.ConnectionResponseData
            rsp = PacketDecoder.DecodeConnectionResponse(data)

            mMaxServerPacketSize = rsp.MaxOBEXPacketLength

            Dim detail As New PacketDecoder.PacketDetail
            detail = PacketDecoder.GetDetail(rsp.PacketBase)

            mConnectionID = detail.ConnectionID

            If rsp.PacketBase.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                Return True
            Else
                mLastError = rsp.PacketBase.ResponseCode
                Return False
            End If
        End Function

        ''' <summary>
        ''' Disconnect from current service
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Disconnect() As Boolean
            Dim packet As New PacketEncoder
            packet.OperateCode = PacketEncoder.OpCode.DISCONNECT
            packet.FinalBitSet = True
            If mConnectionID <> -1 Then
                packet.AddHeader(New ConnectionIDHeader(mConnectionID))
            End If
            Dim data As Byte() = Send(packet.ToByteData)
            Dim rsp As New PacketDecoder.PacketStructure
            rsp = PacketDecoder.Decode(data)
            If rsp.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                Return True
            Else
                mLastError = rsp.ResponseCode
                Return False
            End If
        End Function

        ''' <summary>
        ''' Retrive current Folder.
        ''' </summary>
        ''' <returns>Data from server. Normally a XML File (UTF8 encoding)</returns>
        ''' <remarks>
        ''' To retrieve the current folder: 
        ''' Send a GET Request with an empty Name header and a Typeheader that specifies the folder object type.
        ''' </remarks>
        Public Overridable Function GetCurrentFolderList() As String
            Dim encoder As New PacketEncoder
            encoder.OperateCode = PacketEncoder.OpCode.GET

            If mConnectionID >= 0 Then
                encoder.AddHeader(New ConnectionIDHeader(mConnectionID))
            End If

            encoder.AddHeader(New TypeHeader(ASCII.GetBytes("x-obex/folder-listing" & Chr(0))))
            'encoder.AddHeader(New TypeHeader(ASCII.GetBytes("x-obex/folder-listing")))


            Dim DirListXML As New StringBuilder
            Do
                Dim Response As Byte()
                'Send command
                Response = Send(encoder.ToByteData)
                'Decode OBEX Packet
                Dim Decoder As New PacketDecoder
                Dim Detail As PacketDecoder.PacketDetail

                Detail = PacketDecoder.GetDetail(PacketDecoder.Decode(Response))
                'Add to Response String

                Select Case Detail.ResponseCode
                    Case PacketDecoder.PacketStructure.EnumResponseCode.Continue
                        encoder = New PacketEncoder
                        encoder.OperateCode = PacketEncoder.OpCode.GET
                        DirListXML.Append(Encoding.UTF8.GetString(Detail.BodyHeader))
                    Case PacketDecoder.PacketStructure.EnumResponseCode.Success
                        DirListXML.Append(Encoding.UTF8.GetString(Detail.BodyHeader))
                        Exit Do
                    Case Else
                        mLastError = Detail.ResponseCode
                        Throw New Exception("Can't get dir list")
                End Select
            Loop
            Return DirListXML.ToString
        End Function

        ''' <summary>
        ''' Get files and directories under the path. Current folder will be set to path.
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns>Data from server. Normally a XML File (UTF8 encoding)</returns>
        ''' <remarks>
        ''' First change folder and the get current folder list.
        ''' </remarks>
        Public Overridable Function GetFolderList(ByVal path As String) As String
            If SetPath(path) = False Then
                Throw New Exception("Can't set path")
            End If
            Return GetCurrentFolderList()
        End Function

        ''' <summary>
        ''' Back to root path
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BackToRoot() As Boolean
            Dim packet As New PacketEncoder.SetPathPacket()
            packet.CreateFolderIfNotExist = False
            packet.BackupALevelBeforeApplyingName = False
            If mConnectionID >= 0 Then
                packet.AddHeader(New ConnectionIDHeader(mConnectionID))
            End If
            packet.AddHeader(New NameHeader(String.Empty))

            Dim data As Byte() = Send(packet.ToByte)

            Dim rsp As New PacketDecoder.PacketStructure
            rsp = PacketDecoder.Decode(data)
            If rsp.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                Return True
            Else
                mLastError = rsp.ResponseCode
                Return False
            End If
        End Function

        ''' <summary>
        ''' Set current folder path. Absoluate path required.
        ''' </summary>
        ''' <param name="Path">Set to string.empty will guide you to root. For example "\test1\test2"</param>
        ''' <returns>True if success;False if failed</returns>
        ''' <remarks></remarks>
        Public Overridable Function SetPath(ByVal path As String) As Boolean
            path = path.Replace("/", "\")
            Dim p() As String = path.Split("\")
            'Back to root first
            If BackToRoot() = False Then
                Return False
            End If

            For i As Integer = 1 To p.Length - 1
                Dim packet As New PacketEncoder.SetPathPacket()
                packet.CreateFolderIfNotExist = False
                packet.BackupALevelBeforeApplyingName = False

                If mConnectionID >= 0 Then
                    packet.AddHeader(New ConnectionIDHeader(mConnectionID))
                End If

                packet.AddHeader(New NameHeader(p(i)))

                Dim data As Byte() = Send(packet.ToByte)

                Dim rsp As New PacketDecoder.PacketStructure
                rsp = PacketDecoder.Decode(data)

                If rsp.ResponseCode <> PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                    mLastError = rsp.ResponseCode
                    Return False
                End If
            Next
            Return True
        End Function

        ''' <summary>
        ''' Create a folder in current directory.
        ''' </summary>
        ''' <param name="name">Folder name.</param>
        ''' <returns>True if success;False if failed</returns>
        ''' <remarks></remarks>
        Public Overridable Function CreateFolder(ByVal name As String) As Boolean
            Dim packet As New PacketEncoder.SetPathPacket()
            packet.CreateFolderIfNotExist = True
            packet.BackupALevelBeforeApplyingName = False

            If mConnectionID >= 0 Then
                packet.AddHeader(New ConnectionIDHeader(mConnectionID))
            End If
            packet.AddHeader(New NameHeader(name))

            Dim data As Byte() = Send(packet.ToByte)

            Dim rsp As New PacketDecoder.PacketStructure
            rsp = PacketDecoder.Decode(data)

            If rsp.ResponseCode <> PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                mLastError = rsp.ResponseCode
                Return False
            End If
            'Backup to a level
            packet = New PacketEncoder.SetPathPacket()
            packet.CreateFolderIfNotExist = False
            packet.BackupALevelBeforeApplyingName = True
            data = Send(packet.ToByte)
            rsp = PacketDecoder.Decode(data)
            If rsp.ResponseCode <> PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                mLastError = rsp.ResponseCode
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Del a folder
        ''' </summary>
        ''' <param name="name">
        ''' Where the EMPTY folder or file you want to del. For exmaple:
        ''' If you want to del "test2" under "\test1" folder, you can set name to "test2"
        ''' If you want to file "testFile" under \test1" folder, you can set name to "testFile"
        ''' </param>
        ''' <returns>True if success;False if failed</returns>
        ''' <remarks></remarks>
        Public Overridable Function Del(ByVal name As String) As Boolean
            Dim E As New PacketEncoder
            E.OperateCode = PacketEncoder.OpCode.PUT
            If mConnectionID >= 0 Then
                E.AddHeader(New ConnectionIDHeader(mConnectionID))
            End If
            E.AddHeader(New NameHeader(name))
            Dim d As New PacketDecoder
            Dim rsp As PacketDecoder.PacketStructure = PacketDecoder.Decode(Send(E.ToByteData))
            If rsp.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                Return True
            Else
                mLastError = rsp.ResponseCode
                Return False
            End If
        End Function

        ''' <summary>
        ''' Send bytes from stream to destination path.
        ''' </summary>
        ''' <param name="source">Source of file</param>
        ''' <param name="path">Destination path.</param>
        ''' <param name="fileName">Desctination name</param>
        ''' <returns>True if success;False if failed</returns>
        ''' <remarks></remarks>
        Public Overridable Function SendFile(ByVal source As IO.Stream, ByVal byteToSend As Integer, ByVal path As String, ByVal fileName As String) As Boolean
            Return SendFile(source, byteToSend, Nothing, path, fileName)
        End Function

        ''' <summary>
        ''' Send a file.
        ''' </summary>
        ''' <param name="source">Source of the data</param>
        ''' <param name="byteToSend">Bytes should send.</param>
        ''' <param name="customHeaderData">Custom Header Data.</param>
        ''' <param name="path">Destination folder of file. For example, "\Data\Folder"</param>
        ''' <param name="fileName">Destination file name.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function SendFile(ByVal source As IO.Stream, ByVal byteToSend As Integer, ByVal customHeaderData As Byte(), ByVal path As String, ByVal fileName As String) As Boolean
            mAbort = False
            If SetPath(path) = False Then
                Return False
            End If
            TotalByteSend = 0
            Dim packets() As PacketEncoder.SendFilePacket.Packet
            Dim sendFilePacketsEncoder As New PacketEncoder.SendFilePacket
            With sendFilePacketsEncoder
                .ConnectionID = mConnectionID
                .DataLength = byteToSend
                .FileName = fileName
                .MaxServerPacketSize = mMaxServerPacketSize
                .Source = source
                .CustomData = customHeaderData
                packets = .ToPackets
            End With
            For i As Integer = 0 To packets.Length - 1
                Dim data As Byte() = Send(packets(i).Data)
                Dim rsp As PacketDecoder.PacketStructure = PacketDecoder.Decode(data)
                If rsp.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Success Or _
                   rsp.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Continue Then
                    TotalByteSend += packets(i).ByteSend
                Else
                    mLastError = rsp.ResponseCode
                    Return False
                End If
            Next
            TotalByteSend = CInt(source.Length)
            Return True
        End Function

        ''' <summary>
        ''' Get bytes from destnation and write to a stream.
        ''' </summary>
        ''' <param name="sourcePath">Path of the file</param>
        ''' <param name="sourceName">File you want to get</param>
        ''' <param name="destination">IO stream to hold the data</param>
        ''' <returns>Total bytes received.
        ''' -1 for error</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetFile(ByVal sourcePath As String, ByVal sourceName As String, ByVal destination As IO.Stream) As Integer
            mAbort = False
            TotalByteReceived = 0

            If SetPath(sourcePath) = False Then
                Return -1
            End If
            Dim encoder As New PacketEncoder
            encoder.OperateCode = PacketEncoder.OpCode.GET
            encoder.FinalBitSet = True
            If mConnectionID >= 0 Then
                encoder.AddHeader(New ConnectionIDHeader(mConnectionID))
            End If
            encoder.AddHeader(New NameHeader(sourceName))

            Dim data As Byte() = Send(encoder.ToByteData)
            Dim rsp As New PacketDecoder.PacketStructure
            rsp = PacketDecoder.Decode(data)
            Dim detail As New PacketDecoder.PacketDetail
            detail = PacketDecoder.GetDetail(rsp)

            'Get first Packet
            If rsp.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Success Then
                If detail.BodyHeader Is Nothing = False Then
                    destination.Write(detail.BodyHeader, 0, detail.BodyHeader.Length())
                    destination.Flush()
                    Return detail.BodyHeader.Length
                Else
                    Return 0
                End If
            End If

            If rsp.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Continue Then
                destination.Write(detail.BodyHeader, 0, detail.BodyHeader.Length())
                destination.Flush()
                TotalByteReceived += detail.BodyHeader.Length()
            Else
                Return -1
            End If
            'Get remaining packets
            Do
                If mAbort = True Then Exit Do
                encoder = New PacketEncoder
                encoder.OperateCode = PacketEncoder.OpCode.GET
                encoder.FinalBitSet = True
                If mConnectionID > 0 Then
                    encoder.AddHeader(New ConnectionIDHeader(mConnectionID))
                End If
                data = Send(encoder.ToByteData)

                rsp = New PacketDecoder.PacketStructure
                rsp = PacketDecoder.Decode(data)
                detail = New PacketDecoder.PacketDetail
                If rsp.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Continue Then
                    encoder = New PacketEncoder
                    encoder.FinalBitSet = True
                    encoder.OperateCode = PacketEncoder.OpCode.GET
                    If mConnectionID >= 0 Then
                        encoder.AddHeader(New ConnectionIDHeader(mConnectionID))
                    End If
                End If
                detail = PacketDecoder.GetDetail(rsp)
                destination.Write(detail.BodyHeader, 0, detail.BodyHeader.Length())
                destination.Flush()
                TotalByteReceived += detail.BodyHeader.Length()
                If detail.ResponseCode = PacketDecoder.PacketStructure.EnumResponseCode.Success Then Exit Do
            Loop
            Return TotalByteReceived
        End Function

        ''' <summary>
        ''' Get file content and convert to string using encoder
        ''' </summary>
        ''' <param name="sourcePath"></param>
        ''' <param name="sourceName"></param>
        ''' <returns>String contains read data. If error or no data available. It returns String.Empty</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetFile(ByVal sourcePath As String, ByVal sourceName As String, ByVal encoder As Text.Encoding) As String
            Dim mem As New IO.MemoryStream
            Dim len As Integer = GetFile(sourcePath, sourceName, mem)
            If len <= 0 Then Return String.Empty
            Dim data(len - 1) As Byte
            mem.Position = 0
            mem.Read(data, 0, len)
            Dim result As String = encoder.GetString(data)
            Return result
        End Function

        ''' <summary>
        ''' Get file content using ASCII encoding
        ''' </summary>
        ''' <param name="sourcePath"></param>
        ''' <param name="sourceName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function GetFile(ByVal sourcePath As String, ByVal sourceName As String) As String
            Return GetFile(sourcePath, sourceName, Text.Encoding.ASCII)
        End Function

        ''' <summary>
        ''' Send a OBEX command.
        ''' </summary>
        ''' <param name="cmdToSend">Command to send.</param>
        ''' <returns>Response</returns>
        ''' <remarks></remarks>
        Public Function Send(ByVal cmdToSend As Byte()) As Byte()
            mPort.ReadExisting()
            mPort.Write(cmdToSend, 0, cmdToSend.Length)
            Dim t1 As Date = Now
            Dim buffer(1023) As Byte
            Dim readCount As Integer

            Dim byteReceived As New Collections.ArrayList
            Dim len As Integer = 0

            Do
                If mPort.BytesToRead > 0 Then
                    readCount = mPort.Read(buffer, 0, buffer.Length)
                Else
                    readCount = 0
                End If
                For i As Integer = 0 To readCount - 1
                    byteReceived.Add(buffer(i))
                Next
                If len = 0 AndAlso byteReceived.Count >= 3 Then
                    len = CByte(byteReceived(1)) * 256 + CByte(byteReceived(2))
                End If
                If len > 0 And byteReceived.Count = len Then Exit Do

                If Now.Subtract(t1).TotalMilliseconds > mOBEXTimeOut Then
                    Throw New TimeoutException("Send OBEX timeout")
                End If
            Loop

            Dim data(byteReceived.Count - 1) As Byte
            byteReceived.CopyTo(data)
            Return data
        End Function

        ''' <summary>
        ''' Abort current process. Only used when sending or receving file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Abort()
            mAbort = True
        End Sub
    End Class
End Namespace