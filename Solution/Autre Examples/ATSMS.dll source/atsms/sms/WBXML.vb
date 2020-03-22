Imports System


Namespace SMS

    ' <summary>
    ' Series of well known constants and static byte values used when encoding
    ' a document to WBXML
    ' </summary>
    Public Class WBXML
        Public Const NULL As Byte = &H0

        Public Const VERSION_1_1 As Byte = &H1
        Public Const VERSION_1_2 As Byte = &H2

        Public Const CHARSET_UTF_8 As Byte = &H6A

        Public Const TAGTOKEN_END As Byte = &H1
        Public Const TOKEN_INLINE_STRING_FOLLOWS As Byte = &H3
        Public Const TOKEN_OPAQUEDATA_FOLLOWS As Byte = &HC3


        Public Shared Function SetTagTokenIndications(ByVal token As Byte, ByVal hasAttributes As Boolean, ByVal hasContent As Boolean) As Byte
            If hasAttributes Then
                token = token Or &HC0
            End If
            If hasContent Then
                token = token Or &H40
            End If
            Return token
        End Function 'SetTagTokenIndications
    End Class 'WBXML

End Namespace 'com.esendex.articles.wappush