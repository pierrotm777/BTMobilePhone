Imports System


Namespace SMS

    ' <summary>
    ' Well known values used when generating WSP (Wireless Session Protocol) headers
    ' </summary>
    Public Class WSP
        Public Const TRANSACTIONID_CONNECTIONLESSWSP As Byte = &H25

        Public Const PDUTYPE_PUSH As Byte = &H6

        Public Const HEADER_CONTENTLENGTH As Byte = &H8D

        Public Shared HEADER_CONTENTTYPE_application_vnd_wap_sic_utf_8() As Byte = {&H3, &HAE, &H81, &HEA}

        Public Const HEADER_APPLICATIONTYPE As Byte = &HAF
        Public Const HEADER_APPLICATIONTYPE_x_wap_application_id_w2 As Byte = &H82

        Public Shared HEADER_PUSHFLAG() As Byte = {&HB4, &H84}
    End Class 'WSP 

End Namespace