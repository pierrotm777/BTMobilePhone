

''' <summary>
''' Storage class
''' </summary>
''' <remarks></remarks>
Public Class Storage

    Private strName As String
    Private iUsed As Integer
    Private iTotal As Integer


    Public Property Name() As String
        Get
            Return Me.strName
        End Get
        Set(ByVal Value As String)
            Me.strName = Value
        End Set
    End Property

    Public Property Used() As Integer
        Get
            Return Me.iUsed
        End Get
        Set(ByVal Value As Integer)
            Me.iUsed = Value
        End Set
    End Property

    Public Property Total() As Integer
        Get
            Return Me.iTotal
        End Get
        Set(ByVal Value As Integer)
            Me.iTotal = Value
        End Set
    End Property

End Class


