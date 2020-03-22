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

Imports Dreamworld.Protocol.OBEXClient.Header
Namespace Dreamworld.Protocol.OBEXClient
    Public Class PacketDecoder
#Region "Decode basic structure"
        Public Structure PacketStructure
            Public Structure HeaderValue
                Public Content As Byte()
            End Structure

            ''' <summary>
            ''' Response Code Values with Final Bit Set according to OBEX Version 2
            ''' </summary>
            ''' <remarks></remarks>
            Public Enum EnumResponseCode
                Reserved = &H0

                [Continue] = &H90
                Success = &HA0
                Created = &HA1
                Accepted = &HA2
                NonAuthoritativeInformation = &HA3
                NoContent = &HA4
                ResetContent = &HA5
                PartialContent = &HA6

                MultipleChoices = &HB0
                MovedPermanently = &HB1
                MovedTemporaily = &HB2
                SeeOther = &HB3
                NotModified = &HB4
                UserProxy = &HB5

                BadRequest = &HC0
                Unauthorized = &HC1
                PaymentRequired = &HC2
                Forbidden = &HC3
                NotFound = &HC4
                MethodNotAllowed = &HC5
                NotAcceptable = &HC6
                ProxyAuthenticationRequired = &HC7
                RequestTiemOut = &HC8
                Conflict = &HC9
                Gone = &HCA
                LengthRequired = &HCB
                PreconditionFailed = &HCC
                RequestedEntityTooLarge = &HCD
                UnsupportedMediaType = &HCF

                InternalServerError = &HD0
                NotImplemented = &HD1
                BadGateway = &HD2
                ServiceUnavailable = &HD3
                GatewayTimeout = &HD4
                HTTPVersionNotSupport = &HD5

                DatabaseFull = &HE0
                DatabaseLocked = &HE1
            End Enum

            Public ResponseCode As EnumResponseCode
            Public PacketLength As Integer
            Public HI() As Byte
            Public HV() As HeaderValue
        End Structure

        Public Shared Function Decode(ByVal PacketByte As Byte()) As PacketStructure
            Dim Packet As New PacketStructure
            If PacketByte.Length < 3 Then Throw New Exception("Invalid OBEX Packet")
            With Packet
                Try
                    .ResponseCode = CType(PacketByte(0), PacketStructure.EnumResponseCode)
                Catch e As InvalidCastException
                    .ResponseCode = PacketStructure.EnumResponseCode.Reserved
                End Try

                .PacketLength = (CInt(PacketByte(1)) << 8) + PacketByte(2)
                'Check Packet size
                If .PacketLength <> PacketByte.Length Then Throw New Exception("OBEX Packet is not integrated")
                'Find HI and HV
                If .PacketLength > 3 Then
                    Dim position As Integer = 3, j As Integer = 0
                    Do Until position >= .PacketLength - 1
                        ReDim Preserve .HI(j)
                        ReDim Preserve .HV(j)
                        .HI(j) = PacketByte(position)
                        'Select HI type and Get HV
                        position += 1
                        Dim HVLength As Integer
                        Select Case (.HI(j) And &HC0) '11000000
                            Case &H0
                                'null terminated Unicode text,length prefixed with 2 byte unsinged integer
                                HVLength = (CInt(PacketByte(position)) << 8) + PacketByte(position + 1) - 3
                                'Skip 2 byte unsinged integer
                                position += 2
                            Case &H40
                                'byte sequence,length prefixed with 2 byte unsinged interger
                                HVLength = (CInt(PacketByte(position)) << 8) + PacketByte(position + 1) - 3
                                'Skip 2 byte unsinged integer
                                position += 2
                            Case &H80
                                '1 byte quantity
                                'I've never used it.
                                Stop
                                HVLength = 1
                            Case &HC0
                                '4 byte quantity-transmitted in network byte order(high byte first)
                                HVLength = 4
                        End Select

                        ReDim .HV(j).Content(HVLength - 1)
                        System.Array.Copy(PacketByte, position, .HV(j).Content, 0, HVLength)
                        position += HVLength
                        j += 1
                    Loop
                Else
                    .HI = Nothing
                    .HV = Nothing
                End If
                Return Packet
            End With
        End Function

        Public Structure ConnectionResponseData
            Public OBEXVersion As Integer
            Public MaxOBEXPacketLength As Integer
            Public PacketBase As PacketStructure
        End Structure

        Public Shared Function DecodeConnectionResponse(ByVal packetByte As Byte()) As ConnectionResponseData
            Dim responsePacket As New ConnectionResponseData
            If packetByte.Length < 3 Then Throw New Exception("Invalid OBEX Packet")
            'Check Packet size
            responsePacket.PacketBase.PacketLength = (CInt(packetByte(1)) << 8) + packetByte(2)
            If responsePacket.PacketBase.PacketLength <> packetByte.Length Then Throw New Exception("OBEX Packet is not integrated")

            With responsePacket
                .OBEXVersion = packetByte(3)
                .MaxOBEXPacketLength = (CInt(packetByte(5)) << 8) + packetByte(6)
            End With

            Dim commPacket(packetByte.Length - 4 - 1) As Byte
            commPacket(0) = packetByte(0)
            commPacket(1) = packetByte(1)
            commPacket(2) = CByte(packetByte(2) - 4)
            For i As Integer = 7 To packetByte.Length - 1
                commPacket(i - 4) = packetByte(i)
            Next
            responsePacket.PacketBase = Decode(commPacket)
            Return responsePacket
        End Function
#End Region

#Region "Details"
        Public Class PacketDetail
            Public ResponseCode As PacketDecoder.PacketStructure.EnumResponseCode
            Public PacketLength As Integer
            Public CountHeader As Integer
            Public NameHeader As String
            Public TypeHeader As Byte()
            Public LengthHeader As Integer
            Public TimeStampeHeader As Date
            Public DescrptionHeader As String
            Public TargetHeader As Byte()
            Public HttpHeader As Byte()
            Public BodyHeader As Byte()
            Public WhoHeader As Byte()
            Public ConnectionID As Integer = -1
            Public AppParaHeader As Byte()
            Public AuthChallenge As Byte()
            Public AuthResponse As Byte()
            Public ObjectClass As Byte()
        End Class

        Public Shared Function GetDetail(ByVal BaseInfo As PacketStructure) As PacketDetail
            Dim HeaderID As Byte
            Dim PacketDetail As New PacketDetail
            Dim i As Integer

            PacketDetail.ResponseCode = BaseInfo.ResponseCode
            If BaseInfo.HI Is Nothing Then
                Return PacketDetail
            Else
                For Each HeaderID In BaseInfo.HI
                    With BaseInfo.HV(i)
                        Select Case HeaderID
                            Case CByte(HI.Count)
                                PacketDetail.CountHeader = HI0xC0(.Content)
                            Case CByte(HI.Name)
                                PacketDetail.NameHeader = HI0x00(.Content)
                            Case CByte(HI.Type)
                                PacketDetail.TypeHeader = .Content
                            Case CByte(HI.Length)
                                PacketDetail.LengthHeader = HI0xC0(.Content)
                            Case CByte(HI.TimeISO)
                                Dim timeString As String = Text.Encoding.ASCII.GetString(.Content)
                                PacketDetail.TimeStampeHeader = DateTime.Parse(timeString.Replace("T", ""))
                            Case CByte(HI.Descripition)
                                PacketDetail.DescrptionHeader = HI0x00(.Content)
                            Case CByte(HI.Body)
                                PacketDetail.BodyHeader = .Content
                            Case CByte(HI.EndOfBody)
                                PacketDetail.BodyHeader = .Content
                            Case CByte(HI.Who)
                                PacketDetail.WhoHeader = .Content
                            Case CByte(HI.ConnectionID)
                                PacketDetail.ConnectionID = HI0xC0(.Content)
                            Case CByte(HI.AppParameters)
                                PacketDetail.AppParaHeader = .Content
                            Case CByte(HI.AuthChallenge)
                                PacketDetail.AuthChallenge = .Content
                            Case CByte(HI.AuthResponse)
                                PacketDetail.AuthResponse = .Content
                            Case CByte(HI.ObjectClass)
                                PacketDetail.ObjectClass = .Content
                            Case Else
                                Stop
                        End Select
                    End With
                    i += 1
                Next
                Return PacketDetail
            End If
        End Function

        Private Shared Function HI0x00(ByVal bytes As Byte()) As String
            Return Text.Encoding.BigEndianUnicode.GetString(bytes, 0, bytes.Length - 2)  'Avoid null terminal char
        End Function

        'Private Shared Function HI0x40(ByVal bytes As Byte()) As Byte()
        '    Return bytes
        'End Function

        Private Shared Function HI0xC0(ByVal bytes As Byte()) As Integer
            Return CInt((CInt(bytes(0)) << 24) + (CInt(bytes(1)) << 16) + (CInt(bytes(2)) << 8) + CInt(bytes(3)))
        End Function

        'Private Shared Function HI0x80(ByVal bytes As Byte()) As Byte
        '    Return bytes(0)
        'End Function
#End Region
    End Class
End Namespace

