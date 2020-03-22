Imports ATSMS.Common

Public Class OutboxSMSSendingEventArgs

#Region "Private Members"
    Private strDestinationNumber As String
    Private enumPriority As EnumQueuePriority
    Private strMsgNo As String
    Private strMsg As String
#End Region


#Region "Properties"

    Public Property DestinationNumber() As String
        Get
            Return Me.strDestinationNumber
        End Get
        Set(ByVal value As String)
            Me.strDestinationNumber = value
        End Set
    End Property

    Public Property Priority() As EnumQueuePriority
        Get
            Return Me.enumPriority
        End Get
        Set(ByVal value As EnumQueuePriority)
            Me.enumPriority = value
        End Set
    End Property

    Public Property QueueMessageKey() As String
        Get
            Return Me.strMsgNo
        End Get
        Set(ByVal value As String)
            Me.strMsgNo = value
        End Set
    End Property

    Public Property Message() As String
        Get
            Return strMsg
        End Get
        Set(ByVal value As String)
            strMsg = value
        End Set
    End Property
#End Region
End Class
