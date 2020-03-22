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
Imports System.Threading

Namespace Dreamworld.FileTransfer.Phone.Link
    Public Class FBUS
        Private Const FBUS_MAX_FRAME_DATA_LENGTH As Integer = &H78
        Private Const FBUS_SOURCE_DEVICE_ID As Integer = &H0            'For Phone
        Private Const FBUS_DESCTINATION_DEVICE_ID As Integer = &HC     'For PC SUITE
        Private Const FBUS_FRAME_START As Integer = &H1E
        Private Const FBUS_TIMEOUT As Integer = 100
        Private mSendSequenceNumber As Byte
        Private mReceivedSequenceNumber As Byte
        Public SequenceNumberHighBit As Byte = &H40
        Private mDataBuffer(-1) As Byte

        Private WithEvents mPort As SerialPort

        Sub New(ByVal port As SerialPort)
            mPort = port
        End Sub

        Sub Init()
            mPort.Close()
            mPort.DtrEnable = True
            mPort.RtsEnable = True
            mPort.Open()
            mPort.WriteLine("at&f")
            Thread.Sleep(1000)
            mPort.ReadExisting()
            mPort.WriteLine("at*nokiafbus")
            Thread.Sleep(1000)
            mPort.ReadExisting()
            mPort.Close()
            mPort.DtrEnable = False
            mPort.RtsEnable = False
            mPort.Open()
            Dim initData As New String(CChar("U"), 128)
            mPort.Write(initData)
            Thread.Sleep(500)
            mPort.ReadExisting()
        End Sub

        Public Class Frame
            Public CommandID As Byte
            Public Data As Byte()
            Public PieceOfData As Byte
            Public FirstPiece As Boolean
            Public IsAck As Boolean
            'Public SquenceNumber As Byte
        End Class

        Private Sub SendFrame(ByVal frame As Frame)
            'Make frame to send
            Dim frameLength As Integer = frame.Data.Length + 2 + 8
            Dim frameSend(frameLength - 1) As Byte
            Dim curPos As Integer
            frameSend(curPos) = FBUS_FRAME_START : curPos += 1                                          'Frame Started
            frameSend(curPos) = FBUS_SOURCE_DEVICE_ID : curPos += 1                         'Source
            frameSend(curPos) = FBUS_DESCTINATION_DEVICE_ID : curPos += 1                   'Dest
            frameSend(curPos) = frame.CommandID : curPos += 1                               'Command ID
            frameSend(curPos) = 0 : curPos += 1           'Length High Byte
            frameSend(curPos) = CByte((frame.Data.Length + 2) And &HFF) : curPos += 1       'Length Low Byte
            Array.Copy(frame.Data, 0, frameSend, curPos, frame.Data.Length) : curPos += frame.Data.Length   'Data
            frameSend(curPos) = frame.PieceOfData : curPos += 1                             'Piece of data

            If frame.FirstPiece Then
                frameSend(curPos) = SequenceNumberHighBit + (mSendSequenceNumber Mod 8) : curPos += 1
            Else
                frameSend(curPos) = mSendSequenceNumber : curPos += 1
            End If
            mSendSequenceNumber = (mSendSequenceNumber + 1) Mod 8

            'Padding byte
            If (frame.Data.Length Mod 2) = 1 Then
                ReDim Preserve frameSend(frameSend.Length)
                frameSend(curPos) = 0 : curPos += 1
            End If
            'Calc CheckSum1
            Dim checkSum1 As Byte
            Dim checkSum2 As Byte
            For i As Integer = 0 To frameSend.Length - 2 - 1 Step 2
                checkSum1 = checkSum1 Xor frameSend(i)
                checkSum2 = checkSum2 Xor frameSend(i + 1)
            Next
            frameSend(curPos) = checkSum1 : curPos += 1
            frameSend(curPos) = checkSum2
            mPort.Write(frameSend, 0, frameSend.Length)
        End Sub

        Sub WaitAck()
            Dim start As Date = Now
            Dim frame As Frame
            Do
                frame = ReadFrame()
                If frame IsNot Nothing Then
                    If frame.IsAck = True Then Exit Do
                End If
                If Now.Subtract(start).TotalMilliseconds > FBUS_TIMEOUT Then Exit Do
            Loop
        End Sub

        Public Sub SendAck(ByVal cmdId As Byte)
            If cmdId = &H7F Then Exit Sub
            Dim actData As Byte() = New Byte() {FBUS_FRAME_START, FBUS_SOURCE_DEVICE_ID, FBUS_DESCTINATION_DEVICE_ID, &H7F _
                                                , &H0, &H2, cmdId, mReceivedSequenceNumber, &H0, &H0}
            Dim checkSum1 As Byte
            Dim checkSum2 As Byte
            For i As Integer = 0 To actData.Length - 3 Step 2
                checkSum1 = checkSum1 Xor actData(i)
                checkSum2 = checkSum2 Xor actData(i + 1)
            Next
            actData(actData.Length - 2) = checkSum1
            actData(actData.Length - 1) = checkSum2

            mPort.Write(actData, 0, actData.Length)
        End Sub

        Private Sub ReadDataIntoBuffer(ByVal sender As Object, ByVal data As System.IO.Ports.SerialDataReceivedEventArgs) Handles mPort.DataReceived
            Dim buff(1023) As Byte
            Dim dataRead As Integer
            dataRead = mPort.Read(buff, 0, buff.Length)
            Dim dataBufferOldSize As Integer = mDataBuffer.Length
            SyncLock mDataBuffer
                ReDim Preserve mDataBuffer(dataBufferOldSize + dataRead - 1)
                Array.Copy(buff, 0, mDataBuffer, dataBufferOldSize, dataRead)
            End SyncLock
        End Sub

        Public Function ReadFrame() As Frame
            Dim frameRead As New Frame
            Dim frameStart As Integer
            SyncLock mDataBuffer
                'Find a frame
                Do
                    If frameStart + 6 > mDataBuffer.Length Then Return Nothing
                    If mDataBuffer(frameStart) = FBUS_FRAME_START And _
                        mDataBuffer(frameStart + 1) = FBUS_DESCTINATION_DEVICE_ID Then
                        'mDataBuffer(frameStart + 2) = FBUS_SOURCE_DEVICE_ID Then
                        Exit Do
                    Else
                        frameStart += 1
                    End If
                Loop
                Dim dataLength As Integer = ((mDataBuffer(frameStart + 4) << 1) + mDataBuffer(frameStart + 5)) - 2
                Dim dataStartPos As Integer = frameStart + 6
                Dim dataEndPos As Integer = dataStartPos + dataLength - 1
                Dim frameLength As Integer = dataLength + 10
                ReDim frameRead.Data(frameLength - 8 - 2 - 1)

                If (dataLength Mod 2) = 1 Then
                    'Padding byte exists
                    frameLength += 1
                End If

                If mDataBuffer.Length - frameStart < frameLength Then Return Nothing

                'Check data
                Dim checkSum1 As Byte
                Dim checkSum2 As Byte
                For i As Integer = frameStart To frameStart + frameLength - 3 Step 2
                    checkSum1 = checkSum1 Xor mDataBuffer(i)
                    checkSum2 = checkSum2 Xor mDataBuffer(i + 1)
                Next
                If checkSum1 <> mDataBuffer(frameStart + frameLength - 2) Or checkSum2 <> mDataBuffer(frameStart + frameLength - 1) Then
                    Throw New Exception("Check sum error")
                End If
                'Start processing
                frameRead.CommandID = mDataBuffer(frameStart + 3)
                Array.Copy(mDataBuffer, dataStartPos, frameRead.Data, 0, dataLength)
                If frameRead.CommandID = &H7F Then
                    'Ack response
                    frameRead.PieceOfData = 0
                    frameRead.IsAck = True
                Else
                    frameRead.PieceOfData = mDataBuffer(dataEndPos + 1)
                    mReceivedSequenceNumber = mDataBuffer(dataEndPos + 2) And &HF
                End If
                frameRead.FirstPiece = CBool((mDataBuffer(dataEndPos + 2) And &H40) >> 1)

                'Clear the frame read
                Dim newDataBuffer(mDataBuffer.Length - frameLength - 1) As Byte
                Array.Copy(mDataBuffer, frameStart + frameLength, newDataBuffer, 0, newDataBuffer.Length)
                mDataBuffer = newDataBuffer
                Return frameRead
            End SyncLock
        End Function

        Public Sub Send(ByVal cmdId As Byte, ByVal data As Byte())
            Dim frameCount As Integer = Fix((data.Length + FBUS_MAX_FRAME_DATA_LENGTH - 1) / FBUS_MAX_FRAME_DATA_LENGTH)
            Dim frameToSend(frameCount - 1) As Frame

            For i As Integer = 0 To frameCount - 2
                frameToSend(i) = New Frame
                With frameToSend(i)
                    .CommandID = cmdId
                    .PieceOfData = frameCount - i
                    ReDim .Data(FBUS_MAX_FRAME_DATA_LENGTH - 1)
                    Array.Copy(data, i * FBUS_MAX_FRAME_DATA_LENGTH, frameToSend(i).Data, 0, FBUS_MAX_FRAME_DATA_LENGTH)
                End With
            Next
            'Last frame
            frameToSend(frameCount - 1) = New Frame
            Dim lastFrameDataLength As Integer = data.Length - (frameCount - 1) * FBUS_MAX_FRAME_DATA_LENGTH
            With frameToSend(frameCount - 1)
                .CommandID = cmdId
                .PieceOfData = 1
                ReDim .Data(lastFrameDataLength - 1)
                Array.Copy(data, data.Length - lastFrameDataLength, .Data, 0, lastFrameDataLength)
            End With
            'Find first frame
            'frameToSend(frameCount - 1).FirstPiece = True
            frameToSend(0).FirstPiece = True
            'Send packet
            For i As Integer = 0 To frameCount - 1
                SendFrame(frameToSend(i))
                Thread.Sleep(1)
            Next
        End Sub

        Public Sub ClearInputBuffer()
            SyncLock mDataBuffer
                ReDim mDataBuffer(-1)
            End SyncLock
        End Sub

    End Class
End Namespace