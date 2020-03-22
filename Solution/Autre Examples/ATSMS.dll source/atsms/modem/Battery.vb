

''' <summary>
''' Battery class
''' </summary>
''' <remarks></remarks>
Public Class Battery

    Private iBatteryLevel As Integer
    Private iMinLevel As Integer
    Private iMaxLevel As Integer
    Private bBatteryCharged As Boolean

    Public Property BatteryLevel() As Integer
        Get
            Return Me.iBatteryLevel
        End Get
        Set(ByVal value As Integer)
            Me.iBatteryLevel = value
        End Set
    End Property

    Public Property MinimumLevel() As Integer
        Get
            Return Me.iMinLevel
        End Get
        Set(ByVal value As Integer)
            Me.iMinLevel = value
        End Set
    End Property

    Public Property MaximumLevel() As Integer
        Get
            Return Me.iMaxLevel
        End Get
        Set(ByVal value As Integer)
            Me.iMaxLevel = value
        End Set
    End Property

    Public Property BatteryCharged() As Boolean
        Get
            Return Me.bBatteryCharged
        End Get
        Set(ByVal value As Boolean)
            Me.bBatteryCharged = value
        End Set
    End Property
End Class

