Public Class NewMessageReceivedEventArgs

    Private strMSISDN As String         ' SMS sender phone number
    Private iReferenceNo As Integer     ' Concatenated SMS reference number (valid if NewMessageConcatenate property is set as False, and will be same for all parts of same concatenated SMS) Integer 
    Private iSeqNo As Integer           ' Concatenated SMS sequence number (valid if NewMessageConcatenate property is set to False, and will range from 1 to TotalParts property Integer 
    Private strSMSC As String           ' Sender SMSC address
    Private strTextMsg As String        ' SMS text message
    Private dtTimestamp As DateTime     ' SMS sending date and time
    Private strTimestampRFC As String   ' SMS sending date and time as RFC-822 format string 
    Private iTotalParts As Integer      ' Total messages if this message is a part of a concatenated SMS (valid if NewMessageConcatenate property is set to False) 

    Public Property MSISDN() As String
        Get
            Return Me.strMSISDN
        End Get
        Set(ByVal value As String)
            Me.strMSISDN = value
        End Set
    End Property

    Public Property ReferenceNo() As Integer
        Get
            Return Me.iReferenceNo
        End Get
        Set(ByVal value As Integer)
            Me.iReferenceNo = value
        End Set
    End Property

    Public Property SeqNo() As Integer
        Get
            Return Me.iSeqNo
        End Get
        Set(ByVal value As Integer)
            Me.iSeqNo = value
        End Set
    End Property

    Public Property SMSC() As String
        Get
            Return Me.strSMSC
        End Get
        Set(ByVal value As String)
            Me.strSMSC = value
        End Set
    End Property

    Public Property TextMessage() As String
        Get
            Return Me.strTextMsg
        End Get
        Set(ByVal value As String)
            Me.strTextMsg = value
        End Set
    End Property

    Public Property Timestamp() As DateTime
        Get
            Return Me.dtTimestamp
        End Get
        Set(ByVal value As DateTime)
            Me.dtTimestamp = value
        End Set
    End Property

    Public Property TimestampRFC() As String
        Get
            Return Me.strTimestampRFC
        End Get
        Set(ByVal value As String)
            Me.strTimestampRFC = value
        End Set
    End Property

    Public Property TotalParts() As Integer
        Get
            Return Me.iTotalParts
        End Get
        Set(ByVal value As Integer)
            Me.iTotalParts = value
        End Set
    End Property
End Class
