Imports System.Collections


Namespace SMS

    Public Class MessageStore : Inherits CollectionBase

#Region "Enumeration"

        Public Enum EnumMessageType
            ReceivedUnreadMessages
            ReceivedReadMessages
            StoredUnsentMessages
            StoredSentMessages
            AllMessages
        End Enum

#End Region


        ' AT+CMGF=1;+CMGL="ALL", "REC READ", "REC UNREAD"


        ' AT+CMGL=0-4 , 0 = "REC UNREAD", 1 = "REC READ", 2="STO UNSENT", 3 = "STO SENT", 4="ALL"

#Region "Private Members"

        Private blnConcatenate As Boolean
        Private blnHasMessages As Boolean
        Private blnIsEmpty As Boolean
        Private oGsmModem As GSMModem


#End Region

#Region "Constructor"
        Public Sub New()

        End Sub
#End Region

#Region "Friend Properties"

        Public Property Modem() As GSMModem
            Get
                Return Me.oGsmModem
            End Get
            Set(ByVal value As GSMModem)
                Me.oGsmModem = value
            End Set
        End Property

#End Region

#Region "Public Properties"

        Public Property Concatenate() As Boolean
            Get
                Return Me.blnConcatenate
            End Get
            Set(ByVal value As Boolean)
                Me.blnConcatenate = value
            End Set
        End Property

        Public Property HasMessages() As Boolean
            Get
                Return Me.blnHasMessages
            End Get
            Set(ByVal value As Boolean)
                Me.blnHasMessages = value
            End Set
        End Property

        Public Property IsEmpty() As Boolean
            Get
                Return Me.blnIsEmpty
            End Get
            Set(ByVal value As Boolean)
                Me.blnIsEmpty = value
            End Set
        End Property
#End Region

#Region "Public Functions"

#End Region

#Region "Public Procedures"

        Public Sub Refresh(Optional ByVal messageType As EnumMessageType = EnumMessageType.AllMessages)
            If oGsmModem.IsConnected Then
                Me.Clear()
                Dim messages() As SMSMessage = oGsmModem.GetSMSMessages(messageType)
                If Not messages Is Nothing Then
                    Dim i As Integer
                    Dim sms As SMSMessage
                    For i = 0 To messages.Length - 1
                        sms = messages(i)
                        sms.Modem = Me.Modem
                        Add(sms)
                    Next
                End If
            End If

        End Sub

        Default Public ReadOnly Property Message(ByVal index As Integer) As SMSMessage
            Get
                Return Item(index)
            End Get
        End Property

#End Region

#Region "Friend Procedures"

        Friend Overloads Function Add(ByVal value As SMSMessage) As SMSMessage
            ' After you inherit the CollectionBase class, you can access an intrinsic object
            ' called InnerList that represents your collection. InnerList is of type ArrayList.
            Me.InnerList.Add(value)
            Return value
        End Function

        Friend Overloads Function Item(ByVal index As Integer) As SMSMessage
            ' To retrieve an item from the InnerList, pass the index of that item to the .Item property.
            Return CType(Me.InnerList.Item(index), SMSMessage)
        End Function

        Friend Overloads Sub Remove(ByVal index As Integer)
            ' This Remove expects an index.
            Dim sms As SMSMessage
            sms = CType(Me.InnerList.Item(index), SMSMessage)
            If Not sms Is Nothing Then
                Me.InnerList.Remove(sms)
            End If
        End Sub

#End Region


    End Class

End Namespace