Imports System
Imports System.Text


Namespace SMS

    ' <summary>
    ' Methods for decoding a byte array to it's Hex representation
    ' </summary>
    Public Class HexDecoder
        Inherits System.Text.Decoder

        Public Sub New()

        End Sub 'New


        ' <summary>
        ' Returns the length of the Hex represenation of a byte array
        ' </summary>
        ' <param name="bytes">array of bytes to represent</param>
        ' <param name="index">start index in buffer</param>
        ' <param name="maxBytes">number of bytes from start to represent</param>
        ' <returns></returns>
        Public Overrides Function GetCharCount(ByVal bytes() As Byte, ByVal index As Integer, ByVal maxBytes As Integer) As Integer
            Return Math.Min(bytes.Length - index, maxBytes) * 2
        End Function 'GetCharCount


        ' <summary>
        ' Returns the char for a given byte value
        ' </summary>
        ' <param name="b"></param>
        ' <returns></returns>
        Private Shared Function GetChar(ByVal b As Byte) As Char
            Dim newValue As String
            If b <= 9 Then
                newValue = Asc("0") + b
                Return Chr(newValue)
            Else
                newValue = Asc("A") + b - 10
                Return Chr(newValue)
            End If
        End Function 'GetChar

        ' <summary>
        ' Write the Hex representation of a byte array to the char array passed in
        ' </summary>
        ' <param name="bytes">array to represent</param>
        ' <param name="byteIndex">start index</param>
        ' <param name="maxBytes">number of bytes to represent</param>
        ' <param name="chars">target array</param>
        ' <param name="charIndex">start index in target array</param>
        ' <returns>number of characters written</returns>
        Public Overloads Overrides Function GetChars(ByVal bytes() As Byte, ByVal byteIndex As Integer, ByVal maxBytes As Integer, ByVal chars() As Char, ByVal charIndex As Integer) As Integer
            'Work out how many chars to return.
            Dim charCount As Integer = GetCharCount(bytes, byteIndex, maxBytes)

            'Check the buffer size.
            If chars.Length - charIndex < charCount Then
                Throw New ArgumentException("The character array is not large enough to contain the characters that will be generated from the byte buffer.", "chars")
            End If

            Dim i As Integer = byteIndex

            While i < maxBytes
                Dim upperValue As Byte = CByte(Math.Floor((bytes(i) / 16)))
                Dim lowerValue As Byte = CByte(bytes(i) Mod 16)
                chars(charIndex) = GetChar(upperValue)
                chars((charIndex + 1)) = GetChar(lowerValue)
                i = i + 1
                charIndex = charIndex + 2
            End While

            Return charCount
        End Function 'GetChars

        ' <summary>
        ' Returns the Hex representation of a byte array
        ' </summary>
        ' <param name="bytes">The byte array to represent</param>
        ' <returns>char array representing the byte array</returns>
        Public Overloads Function GetChars(ByVal bytes() As Byte) As Char()
            Dim charCount As Integer = GetCharCount(bytes, 0, bytes.Length)
            Dim chars(charCount) As Char
            GetChars(bytes, 0, bytes.Length, chars, 0)
            Return chars
        End Function 'GetChars
    End Class 'HexDecoder 
End Namespace