Imports ATSMS.Common


Namespace SMS

    Public Class Message

#Region "Private Members"


        Private strBNumber As String
        Private strMessage As String
        Private blnAlertMessage As Boolean
        Private enumPriority As EnumQueuePriority
        Private enumEncoding As EnumEncoding
        Private strRefNo As String

#End Region

#Region "Constructor"

        Public Sub New()
            blnAlertMessage = False
            enumPriority = EnumQueuePriority.Normal
        End Sub

#End Region

#Region "Properties"

        Public Property Message() As String
            Get
                Return Me.strMessage
            End Get
            Set(ByVal value As String)
                Me.strMessage = value
            End Set
        End Property

        Public Property B_Number() As String
            Get
                Return Me.strBNumber
            End Get
            Set(ByVal value As String)
                Me.strBNumber = value
            End Set
        End Property

        Public Property AlertMessage() As Boolean
            Get
                Return Me.blnAlertMessage
            End Get
            Set(ByVal value As Boolean)
                Me.blnAlertMessage = value
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

        Public Property Encoding() As EnumEncoding
            Get
                Return Me.enumEncoding
            End Get
            Set(ByVal value As EnumEncoding)
                Me.enumEncoding = value
            End Set
        End Property

        Public Property ReferenceNo() As String
            Get
                Return Me.strRefNo
            End Get
            Set(ByVal value As String)
                Me.strRefNo = value
            End Set
        End Property


#End Region

    End Class


End Namespace





