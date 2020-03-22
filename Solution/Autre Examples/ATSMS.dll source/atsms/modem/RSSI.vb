


''' <summary>
''' RSSI class
''' </summary>
''' <remarks></remarks>
Public Class Rssi
    Private iCurrent As Integer
    Private iMin As Integer
    Private iMax As Integer

    Public Property Current() As Integer
        Get
            Return Me.iCurrent
        End Get
        Set(ByVal value As Integer)
            Me.iCurrent = value
        End Set
    End Property

    Public Property Minimum() As Integer
        Get
            Return Me.iMin
        End Get
        Set(ByVal value As Integer)
            Me.iMin = value
        End Set
    End Property

    Public Property Maximum() As Integer
        Get
            Return Me.iMax
        End Get
        Set(ByVal value As Integer)
            Me.iMax = value
        End Set
    End Property


End Class




