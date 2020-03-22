
Namespace SMS


    ''' <summary>
    ''' WAP push message class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WapPushMessage

        Private href As String
        Private text As String

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            href = String.Empty
            text = String.Empty
        End Sub


        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="href"></param>
        ''' <param name="text"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal href As String, ByVal text As String)
            Me.href = href
            Me.text = text
        End Sub


        Public Property Link() As String
            Get
                Return Me.href
            End Get
            Set(ByVal value As String)
                Me.href = value
            End Set
        End Property


        Public Property Message() As String
            Get
                Return Me.text
            End Get
            Set(ByVal value As String)
                Me.text = value
            End Set
        End Property


        ''' <summary>
        ''' Return the PDU coded message content
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Dim msg As PushMessage = New PushMessage(href, text)
            Dim decoder As HexDecoder = New HexDecoder
            Return New String(decoder.GetChars(msg.GetSMSBytes()))
        End Function
    End Class


End Namespace
