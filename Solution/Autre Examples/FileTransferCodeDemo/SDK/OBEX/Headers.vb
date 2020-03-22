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
Namespace Dreamworld.Protocol.OBEXClient
    Namespace Header
        ''' <summary>
        ''' Header ID
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum HI
            Count = &HC0                    'Number of objects (used by Connect)
            Name = &H1                      'Name of the object (often a file name)
            Type = &H42                     'type of object - e.g. text, html, binary, manufacturer specific
            Length = &HC3                   'the length of the object in bytes
            TimeISO = &H44                  'date/time stamp ¨C ISO 8601 version - preferred
            TimeCompatibility = &HC4        'date/time stamp ¨C 4 byte version (for compatibility only)
            Descripition = &H5              'text description of the object
            Target = &H46                   'name of service that operation is targeted to
            HTTP = &H47                     'an HTTP 1.x header
            Body = &H48                     'a chunk of the object body.
            EndOfBody = &H49                'the final chunk of the object body
            Who = &H4A                      'identifies the OBEX application, used to tell if talking to a peer
            ConnectionID = &HCB             'an identifier used for OBEX connection multiplexing
            AppParameters = &H4C            'extended application request & response information
            AuthChallenge = &H4D            'authentication digest-challenge
            AuthResponse = &H4E             'authentication digest-response
            ObjectClass = &H4F              'OBEX Object class of object
        End Enum

#Region "Headers"
        Public MustInherit Class HeaderBase
            Public headerID As HI
            Public headerValue As Byte()
            Private Const headerSize As Integer = 1
            Private Const unsignedIntSize As Integer = 2
            Public Function ToByte() As Byte()
                Dim length As Integer = headerValue.Length + 1  '1 for HI
                Dim rst(length - 1) As Byte
                rst(0) = CByte(headerID)
                headerValue.CopyTo(rst, 1)
                Return rst
            End Function
            ''' <summary>
            ''' Process HI with Bits 8 and 7 set to 00 (0x00)
            ''' </summary>
            ''' <param name="str"></param>
            ''' <remarks></remarks>
            Protected Sub HI0x00(ByVal str As String)
                If str = String.Empty Then
                    'An empty Name header is defined as a Name header of length 3 (one byte opcode + two byte length).
                    ReDim headerValue(1)
                    headerValue(0) = &H0
                    headerValue(1) = &H3
                Else
                    Dim LengthOfName As Byte = CByte((str.Length + 1) * 2)    'Name string is a null-terminated string
                    Dim length As Integer = LengthOfName + 2    '2 for "Length"
                    ReDim headerValue(length - 1)
                    Dim headerValueLength As Integer = LengthOfName + 3
                    headerValue(0) = CByte(((headerValueLength And &HFF00) >> 8))
                    headerValue(1) = CByte((headerValueLength And &HFF))
                    Encoding.BigEndianUnicode.GetBytes(str).CopyTo(headerValue, 2)
                End If
            End Sub

            ''' <summary>
            ''' Process HI with Bits 8 and 7 set to 01 (0x40)
            ''' </summary>
            ''' <param name="bytes"></param>
            ''' <remarks></remarks>
            Protected Sub HI0x40(ByVal bytes As Byte())
                ReDim headerValue(unsignedIntSize + bytes.Length - 1)      '2 for 2byte unsigned integer
                headerValue(0) = CByte((((bytes.Length + unsignedIntSize + headerSize) And &HFF00) >> 8))
                headerValue(1) = CByte((bytes.Length + unsignedIntSize + headerSize) And &HFF)
                bytes.CopyTo(headerValue, 2)
            End Sub

            ''' <summary>
            ''' Process HI with Bits 8 and 7 set to 10 (0x80)
            ''' </summary>
            ''' <param name="aByte"></param>
            ''' <remarks></remarks>
            Protected Sub HI0x80(ByVal aByte As Byte)
                Stop
            End Sub

            ''' <summary>
            ''' Process HI with Bits 8 and 7 set to 11 (0xC0)
            ''' </summary>
            ''' <param name="fourByte"></param>
            ''' <remarks></remarks>
            Protected Overloads Sub HI0xC0(ByVal fourByte As Byte())
                headerValue = fourByte
            End Sub

            ''' <summary>
            ''' Process HI with Bits 8 and 7 set to 11
            ''' </summary>
            ''' <param name="int"></param>
            ''' <remarks></remarks>
            Protected Overloads Sub HI0xC0(ByVal int As Integer)
                ReDim headerValue(3)
                headerValue(0) = CByte(((int And &HFF000000) >> 24))
                headerValue(1) = CByte((int And &HFF0000) >> 16)
                headerValue(2) = CByte((int And &HFF00) >> 8)
                headerValue(3) = CByte(int And &HFF)
            End Sub
        End Class

        Public Class CountHeader
            Inherits HeaderBase

            Sub New(ByVal count As Integer)
                MyBase.headerID = HI.Count
                HI0xC0(count)
            End Sub
        End Class

        Public Class NameHeader
            Inherits HeaderBase
            Sub New(ByVal name As String)
                MyBase.headerID = HI.Name
                HI0x00(name)
            End Sub
        End Class

        Public Class TypeHeader
            Inherits HeaderBase
            Sub New(ByVal type As Byte())
                MyBase.headerID = HI.Type
                HI0x40(type)
            End Sub
        End Class

        Public Class LengthHeader
            Inherits HeaderBase
            Sub New(ByVal length As Integer)
                MyBase.headerID = HI.Length
                HI0xC0(length)
            End Sub
        End Class

        Public Class TimeStampISOHeader
            Inherits HeaderBase
            Sub New(ByVal dateTime As Date)
                MyBase.headerID = HI.TimeISO
                Dim timeStamp As String = Format(dateTime, "yyyyMMdd") + "T" + Format(dateTime, "HHmmss")
                HI0x40(Encoding.ASCII.GetBytes(timeStamp))
            End Sub
        End Class

        Public Class TimeStampCompatibilityHeader
            Sub New(ByVal dateTime As DateTimeKind)
                Throw New NotImplementedException
            End Sub
        End Class

        Public Class DescriptionHeader
            Inherits HeaderBase
            Sub New(ByVal description As String)
                MyBase.headerID = HI.Descripition
                Dim LengthOfName As Byte = CByte((description.Length + 1) * 2)         'Name string is a null-terminated string
                HI0x00(description)
            End Sub
        End Class

        Public Class TargetHeader
            Inherits HeaderBase
            Sub New(ByVal target As Byte())
                MyBase.headerID = HI.Target
                HI0x40(target)
            End Sub

            Sub New(ByVal target As String)
                MyBase.headerID = HI.Target
                HI0x40(Encoding.ASCII.GetBytes(target & Chr(0)))
            End Sub
        End Class

        Public Class BodyHeader
            Inherits HeaderBase

            Sub New(ByVal Body As Byte())
                MyBase.headerID = HI.Body
                HI0x40(Body)
            End Sub
        End Class

        Public Class EndBodyHeader
            Inherits HeaderBase

            Sub New(ByVal EndBody As Byte())
                MyBase.headerID = HI.EndOfBody
                HI0x40(EndBody)
            End Sub
        End Class

        Public Class ConnectionIDHeader
            Inherits HeaderBase
            Sub New(ByVal fourByte As Byte())
                MyBase.headerID = HI.ConnectionID
                HI0xC0(fourByte)
            End Sub

            Sub New(ByVal id As Integer)
                MyBase.headerID = HI.ConnectionID
                Dim fourByte(3) As Byte
                fourByte(0) = (id And &HFF000000) >> 24
                fourByte(1) = (id And &HFF0000) >> 16
                fourByte(2) = (id And &HFF00) >> 8
                fourByte(3) = (id And &HFF)
                HI0xC0(fourByte)
            End Sub
        End Class

        Public Class AppParaHeader
            Inherits HeaderBase
            Sub New(ByVal para As Byte())
                MyBase.headerID = HI.AppParameters
                HI0x40(para)
            End Sub
        End Class

        Public Class CustomData
            Inherits HeaderBase
            Sub New(ByVal headerID As Byte, ByVal headerValue As Byte())
                MyBase.headerID = CType(headerID, HI)
                MyBase.headerValue = headerValue
            End Sub
        End Class

        Public Class ObjectClass
            Inherits HeaderBase
            Sub New(ByVal bytes As Byte())
                MyBase.headerID = HI.ObjectClass
                HI0x40(bytes)
            End Sub
        End Class
#End Region
    End Namespace
End Namespace