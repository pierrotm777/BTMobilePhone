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

Imports System.Text
Imports Dreamworld.Protocol.OBEXClient.Header
Namespace Dreamworld.Protocol.OBEXClient
    Public Class PacketEncoder
#Region "Enum"
        ''' <summary>
        ''' Operate code
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum OpCode
            PUT = &H2
            [GET] = &H3
            SETPATH = &H85
            ABORT = &HFF
            DISCONNECT = &H81
        End Enum
#End Region
        Private packetData(2) As Byte

        Private mFinalBitSet As Boolean = True  'Set final bit when get byte code

        ''' <summary>
        ''' Set Operate Code.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OperateCode() As OpCode
            Get
                Return CType(packetData(0), OpCode)
            End Get
            Set(ByVal Value As OpCode)
                packetData(0) = CByte(Value)
            End Set
        End Property

        ''' <summary>
        ''' Set Final Bit.
        ''' </summary>
        ''' <value>Default True</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FinalBitSet() As Boolean
            Get
                Return mFinalBitSet
            End Get
            Set(ByVal value As Boolean)
                mFinalBitSet = value
            End Set
        End Property

        ''' <summary>
        ''' Convert to byte data
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToByteData() As Byte()
            If packetData(0) = 0 Then
                Throw New Exception("Please set opCode")
            End If
            If mFinalBitSet = True And ((OperateCode And &HF) = OpCode.GET Or (OperateCode And &HF) = OpCode.PUT Or OperateCode = OpCode.DISCONNECT) Then
                packetData(0) = CByte((packetData(0) And &HF) + &H80)
            Else
                packetData(0) = CByte(packetData(0) And &HF)
            End If
            packetData(1) = CByte((packetData.Length And &HFF00) >> 8)
            packetData(2) = CByte((packetData.Length And &HFF))
            Return packetData
        End Function

        ''' <summary>
        ''' Add a header to data.
        ''' </summary>
        ''' <param name="header"></param>
        ''' <remarks></remarks>
        Public Sub AddHeader(ByVal header As Header.HeaderBase)
            Dim b As Byte() = header.ToByte()
            Dim CopyStartIndex As Integer = packetData.Length
            ReDim Preserve packetData(packetData.Length + b.Length - 1)
            b.CopyTo(packetData, CopyStartIndex)
        End Sub

        Public Sub AddByteData(ByVal data As Byte())
            Dim CopyStartIndex As Integer = packetData.Length
            ReDim Preserve packetData(packetData.Length + data.Length - 1)
            data.CopyTo(packetData, CopyStartIndex)
        End Sub

#Region "OBEX Special Packets"
        Public Class ConnectRequestPacket
            Private mMaxOBEXPacketLength(1) As Byte
            Private packetData(6) As Byte
            Public Property MaxOBEXPacketLength() As Integer
                Get
                    Return (CInt(mMaxOBEXPacketLength(0)) << 8) + mMaxOBEXPacketLength(1)
                End Get
                Set(ByVal value As Integer)
                    mMaxOBEXPacketLength(0) = CByte((value And &HFF00) >> 8)
                    mMaxOBEXPacketLength(1) = CByte(value And &HFF)
                End Set
            End Property

            Public Sub AddHeader(ByVal header As Header.HeaderBase)
                Dim b As Byte() = header.ToByte()
                Dim CopyStartIndex As Integer = packetData.Length
                ReDim Preserve packetData(packetData.Length + b.Length - 1)
                b.CopyTo(packetData, CopyStartIndex)
            End Sub

            Public Function ToByte() As Byte()
                packetData(0) = &H80
                packetData(1) = CByte((packetData.Length And &HFF00) >> 8)
                packetData(2) = CByte(packetData.Length And &HFF)
                packetData(3) = &H10 'OBEX Version Number
                packetData(4) = 0
                packetData(5) = mMaxOBEXPacketLength(0)
                packetData(6) = mMaxOBEXPacketLength(1)
                Return packetData
            End Function
        End Class

        Public Class SetPathPacket
            Private mCreateFolderIfNotExist As Boolean
            Private mBackupALevelBeforeApplyingName As Boolean
            Private packetData(4) As Byte

            ''' <summary>
            ''' Set or Get if the folder is not exist, device should create a new folder.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property CreateFolderIfNotExist() As Boolean
                Get
                    Return mCreateFolderIfNotExist
                End Get
                Set(ByVal value As Boolean)
                    mCreateFolderIfNotExist = value
                End Set
            End Property

            ''' <summary>
            ''' Set or Get if backup a level before applying name (equivalent to ../ on many systems)
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property BackupALevelBeforeApplyingName() As Boolean
                Get
                    Return mBackupALevelBeforeApplyingName
                End Get
                Set(ByVal value As Boolean)
                    mBackupALevelBeforeApplyingName = value
                End Set
            End Property

            Public Sub AddHeader(ByVal header As Header.HeaderBase)
                Dim b As Byte() = header.ToByte()
                Dim CopyStartIndex As Integer = packetData.Length
                ReDim Preserve packetData(packetData.Length + b.Length - 1)
                b.CopyTo(packetData, CopyStartIndex)
            End Sub

            Public Function ToByte() As Byte()
                packetData(0) = &H85
                packetData(1) = CByte((packetData.Length And &HFF00) >> 8)
                packetData(2) = CByte(packetData.Length And &HFF)
                packetData(3) = 0
                If mBackupALevelBeforeApplyingName = True Then
                    packetData(3) += 1
                End If
                If mCreateFolderIfNotExist = False Then
                    packetData(3) += 2
                End If
                packetData(4) = 0   'Reserved to 0 
                Return packetData
            End Function
        End Class

        Public Class SendFilePacket
            ''' <summary>
            ''' Source of the data
            ''' </summary>
            ''' <remarks></remarks>
            Public Source As IO.Stream

            ''' <summary>
            ''' Lenght of data to send
            ''' </summary>
            ''' <remarks></remarks>
            Public DataLength As Integer

            ''' <summary>
            ''' Desctination file name
            ''' </summary>
            ''' <remarks></remarks>
            Public FileName As String
            Public ConnectionID As Integer = -1
            Public MaxServerPacketSize As Integer

            Public CustomData As Byte()

            Public Structure Packet
                Public Data As Byte()
                Public ByteSend As Integer
            End Structure

            Public Function ToPackets() As Packet()
                Dim packets(0) As Packet
                Dim buffer() As Byte
                'Encode first packet
                Dim encoder As New PacketEncoder
                encoder.OperateCode = OpCode.PUT
                encoder.FinalBitSet = False
                If ConnectionID <> -1 Then
                    encoder.AddHeader(New ConnectionIDHeader(ConnectionID))
                End If

                encoder.AddHeader(New NameHeader(FileName))
                encoder.AddHeader(New LengthHeader(DataLength))
                encoder.AddHeader(New TimeStampISOHeader(Now))

                If (CustomData Is Nothing) = False Then
                    encoder.AddByteData(CustomData)
                End If
                Dim size As Integer = encoder.ToByteData.Length
                Dim firstDataSize As Integer = MaxServerPacketSize - 3 - size   '3 for Bodyheader HI, and 2 byte length prefix
                'Check if a packet is enough
                If firstDataSize >= DataLength Then
                    'Read out all data and return
                    ReDim buffer(DataLength - 1)
                    'Read first data
                    For i As Integer = 0 To buffer.Length - 1
                        buffer(i) = Source.ReadByte
                    Next
                    encoder.AddHeader(New EndBodyHeader(buffer))
                    packets(0).Data = encoder.ToByteData
                    packets(0).ByteSend = DataLength
                    Return packets
                Else
                    ReDim buffer(firstDataSize - 1)
                    'Read first data
                    For i As Integer = 0 To buffer.Length - 1
                        buffer(i) = Source.ReadByte
                    Next
                    'Require more packets
                    encoder.FinalBitSet = False
                    encoder.AddHeader(New BodyHeader(buffer))
                    packets(0).Data = encoder.ToByteData
                    packets(0).ByteSend = firstDataSize
                End If

                Dim remaingDataLength As Integer = DataLength - firstDataSize
                'Encode remaing packet
                Dim packetsCount As Integer
                Dim remaingPacketDataSize As Integer
                Dim templateEncoder As New PacketEncoder
                templateEncoder.FinalBitSet = False
                templateEncoder.OperateCode = OpCode.PUT

                remaingPacketDataSize = MaxServerPacketSize - templateEncoder.ToByteData.Length - 3

                Dim newEncoder As PacketEncoder
                Do Until remaingDataLength < remaingPacketDataSize
                    packetsCount += 1
                    ReDim Preserve packets(packetsCount)

                    newEncoder = New PacketEncoder
                    newEncoder.FinalBitSet = False
                    newEncoder.OperateCode = OpCode.PUT

                    ReDim buffer(remaingPacketDataSize - 1)
                    For i As Integer = 0 To buffer.Length - 1
                        buffer(i) = Source.ReadByte
                    Next
                    newEncoder.AddHeader(New BodyHeader(buffer))
                    packets(packetsCount).Data = newEncoder.ToByteData
                    packets(packetsCount).ByteSend = remaingPacketDataSize
                    remaingDataLength -= remaingPacketDataSize
                Loop

                packetsCount += 1
                ReDim Preserve packets(packetsCount)
                'Encode last packet
                encoder = New PacketEncoder
                encoder.FinalBitSet = True
                encoder.OperateCode = OpCode.PUT

                ReDim buffer(remaingDataLength - 1)
                For i As Integer = 0 To buffer.Length - 1
                    buffer(i) = Source.ReadByte
                Next
                encoder.AddHeader(New EndBodyHeader(buffer))
                packets(packetsCount).Data = encoder.ToByteData
                packets(packetsCount).ByteSend = remaingDataLength
                'End
                Return packets
            End Function
        End Class

#End Region
    End Class


End Namespace
