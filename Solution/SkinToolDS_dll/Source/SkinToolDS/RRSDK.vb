Option Strict Off
Option Explicit On
Friend Class RRSDK
	
	'Dim RRSDK As RideRunner.SDK             ' Create SDK (DO NOT USE THIS ONE WHEN COMPILING OR DIFFERENT RR VERSIONS WILL NOT WORK!!)
    Dim SDK As Object ' Create Object if you dont have a REFERNCE set up, set one up... much easiry to program
    Dim PluginEnabled As Boolean ' Used to track if calls into RR should be made

    Public Sub New()
        MyBase.New()

        ' Create instance of SDK
        SDK = CreateObject("RideRunner.SDK")

    End Sub


    Public Property Enabled() As Boolean
        Get

            Enabled = PluginEnabled

        End Get
        Set(ByVal Value As Boolean)

            PluginEnabled = Value

        End Set
    End Property

    Public Sub RRlog(ByRef Message As String)

        If PluginEnabled = False Then Exit Sub

        SDK.RRlog(Message)

    End Sub

    Public Sub Execute(ByRef CMD As String, Optional ByRef Wait As Boolean = False)

        If PluginEnabled = False Then Exit Sub

        SDK.Execute(CMD, Wait)

    End Sub

    Public Function GetInfo(ByRef LBL As String, Optional ByRef FMT As String = "") As String

        GetInfo = ""
        If PluginEnabled = False Then Exit Function

        GetInfo = SDK.GetInfo(LBL, FMT)

    End Function

    Public Function GetInd(ByRef IND As String, Optional ByRef SCR As String = "") As String

        GetInd = ""
        If PluginEnabled = False Then Exit Function

        GetInd = SDK.GetInd(IND, SCR)

    End Function

    Public Function MixerCount() As Short

        If PluginEnabled = False Then Exit Function

        MixerCount = SDK.MixerCount()

    End Function

    Public Function MixerName(ByRef N As Short) As String

        MixerName = ""
        If PluginEnabled = False Then Exit Function

        MixerName = SDK.MixerName(N)

    End Function

    Public Function MixerVol(ByRef N As Short) As Integer

        If PluginEnabled = False Then Exit Function

        MixerVol = SDK.MixerVol(N)

    End Function

    Public Sub ErrScrn(ByRef Title As String, ByRef Subject As String, ByRef Message As String, Optional ByRef Timeout As Short = -1)

        If PluginEnabled = False Then Exit Sub

        SDK.ErrScrn(Title, Subject, Message, Timeout)

    End Sub

    Public Sub SetUserVar(ByRef UserVar As String, ByRef Value As String)

        If PluginEnabled = False Then Exit Sub

        SDK.SetUserVar(UserVar, Value)

    End Sub

    Public Function GetUserVar(ByRef UserVar As String) As String

        GetUserVar = ""
        If PluginEnabled = False Then Exit Function

        GetUserVar = SDK.GetUserVar(UserVar)

    End Function

    Public Sub AddRepeatCMD(ByRef Command As String)

        If PluginEnabled = False Then Exit Sub

        SDK.AddRepeatCMD(Command)

    End Sub

    Public Function GetMixerVol(ByRef Line As String) As Integer

        GetMixerVol = -1
        If PluginEnabled = False Then Exit Function

        GetMixerVol = SDK.GetMixerVol(Line)

    End Function

    Public Sub SetMixerVol(ByRef Line As String, ByRef Level As Integer)

        If PluginEnabled = False Then Exit Sub

        SDK.SetMixerVol(Line, Level)

    End Sub

    Public Function GetMixerMute(ByRef Line As String) As Boolean

        GetMixerMute = False
        If PluginEnabled = False Then Exit Function

        GetMixerMute = SDK.GetMixerMute(Line)

    End Function

    Public Sub SetMixerMute(ByRef Line As String, ByRef State As Boolean)

        If PluginEnabled = False Then Exit Sub

        SDK.SetMixerMute(Line, State)

    End Sub

End Class