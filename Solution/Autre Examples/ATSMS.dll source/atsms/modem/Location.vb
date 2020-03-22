

''' <summary>
''' Location class
''' </summary>
''' <remarks></remarks>
Public Class Location
    Private strMCC As String
    Private strMNC As String
    Private strLAI As String
    Private strCellID As String

    Public Property MCC() As String
        Get
            Return Me.strMCC
        End Get
        Set(ByVal value As String)
            Me.strMCC = value
        End Set
    End Property

    Public Property MNC() As String
        Get
            Return Me.strMNC
        End Get
        Set(ByVal value As String)
            Me.strMNC = value
        End Set
    End Property

    Public Property LAI() As String
        Get
            Return Me.strLAI
        End Get
        Set(ByVal value As String)
            Me.strLAI = value
        End Set
    End Property

    Public Property CellID() As String
        Get
            Return Me.strCellID
        End Get
        Set(ByVal value As String)
            Me.strCellID = value
        End Set
    End Property



End Class

