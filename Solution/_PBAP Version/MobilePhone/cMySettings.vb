Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Collections.Generic

Public Class cMySettings : Implements IDisposable

    Public PhoneDebugLog As Boolean
    Public PhoneDeviceName As String
    Public PhoneDeviceName2 As String
    Public RunOnStart As Boolean
    Public RestartBTBeforeStart As Boolean
    Public PhoneBookAutoUpdate As Boolean
    Public PhoneBookUseVcardSupplement As Boolean
    Public EmergencyNumber As String
    Public VocalMessageryNumber As String
    Public AutoSwapPhone As Boolean
    Public PhoneSpeechNumbers As String
    Public PhoneSpeechRecognition As Boolean
    Public PhoneCountryCodes As New List(Of String) 'networkCarrierCode , countryCode As String,  internationalCarrierCode
    'Public PhoneAlarm1 As String
    'Public PhoneAlarm2 As String
    'Public PhoneAlarm3 As String
    'Public PhoneAlarm4 As String
    'Public PhoneAlarm5 As String
    Public LockInMotion As Boolean
    Public LockInMotionForCMD As String
    Public PhoneExecATCmd As Boolean
    Public Language As String
    Public AutoNotAnswerCallIn As Boolean
    Public PhonePANAutoRun As Boolean
    Public PhoneNoAddContact As Boolean
    Public PhoneNoSMSPopupInfo As Boolean
    Public PhoneStartupDelay As Integer
    Public PhonePinCode As String

    Private Shared XMLFilename As String

    Public Sub New()
        SetToDefault(Me)

    End Sub

    Public Sub New(FileName As String)
        XMLFilename = FileName
        If Path.GetExtension(XMLFilename).ToLower() <> "xml" Then XMLFilename = XMLFilename + ".xml"
        If File.Exists(XMLFilename) = False Then SerializeToXML(New cMySettings())
    End Sub

    Public Sub New(ByRef Settings As cMySettings)
        Me.PhoneDebugLog = Settings.PhoneDebugLog
        Me.PhoneDeviceName = Settings.PhoneDeviceName
        Me.PhoneDeviceName2 = Settings.PhoneDeviceName2
        Me.RunOnStart = Settings.RunOnStart
        Me.RestartBTBeforeStart = Settings.RestartBTBeforeStart
        Me.PhoneBookAutoUpdate = Settings.PhoneBookAutoUpdate
        Me.PhoneBookUseVcardSupplement = Settings.PhoneBookUseVcardSupplement
        Me.EmergencyNumber = Settings.EmergencyNumber
        Me.VocalMessageryNumber = Settings.VocalMessageryNumber
        Me.AutoSwapPhone = Settings.AutoSwapPhone
        Me.PhoneSpeechNumbers = Settings.PhoneSpeechNumbers
        Me.PhoneSpeechRecognition = Settings.PhoneSpeechRecognition
        Me.PhoneCountryCodes = New List(Of String)(Settings.PhoneCountryCodes)
        'Me.PhoneAlarm1 = Settings.PhoneAlarm1
        'Me.PhoneAlarm2 = Settings.PhoneAlarm2
        'Me.PhoneAlarm3 = Settings.PhoneAlarm3
        'Me.PhoneAlarm4 = Settings.PhoneAlarm4
        'Me.PhoneAlarm5 = Settings.PhoneAlarm5
        Me.LockInMotion = Settings.LockInMotion
        Me.LockInMotionForCMD = Settings.LockInMotionForCMD
        Me.PhoneExecATCmd = Settings.PhoneExecATCmd
        Me.Language = Settings.Language
        Me.AutoNotAnswerCallIn = Settings.AutoNotAnswerCallIn
        Me.PhonePANAutoRun = Settings.PhonePANAutoRun
        Me.PhoneNoAddContact = Settings.PhoneNoAddContact
        Me.PhoneNoSMSPopupInfo = Settings.PhoneNoSMSPopupInfo
        Me.PhoneStartupDelay = Settings.PhoneStartupDelay
        Me.PhonePinCode = Settings.PhonePinCode

    End Sub

    Public Shared Sub SerializeToXML(ByRef Settings As cMySettings)

        Dim xmlSerializer As New XmlSerializer(GetType(cMySettings))

        Using xmlTextWriter As New XmlTextWriter(XMLFilename, Encoding.UTF8)
            xmlTextWriter.Formatting = Formatting.Indented
            xmlSerializer.Serialize(xmlTextWriter, Settings)
            xmlTextWriter.Close()
        End Using


    End Sub

    Public Shared Sub DeseralizeFromXML(ByRef Settings As cMySettings)

        Dim fs As FileStream = Nothing

        ' do i have settings?
        If File.Exists(XMLFilename) = True Then
            Try
                fs = New FileStream(XMLFilename, FileMode.Open, FileAccess.Read)
                Dim xmlSerializer As New XmlSerializer(GetType(cMySettings))
                Settings = xmlSerializer.Deserialize(fs)
            Catch
                'load error of some sort, or OBJECT deserialize error
                'do we tell anyone?
                Exit Sub
            Finally
                If Not fs Is Nothing Then fs.Close()
                fs = Nothing
            End Try
        End If

    End Sub


    Public Shared Sub Copy(ByRef SourceSettings As cMySettings, ByRef DestSettings As cMySettings)

        DestSettings.PhoneDebugLog = SourceSettings.PhoneDebugLog
        DestSettings.PhoneDeviceName = SourceSettings.PhoneDeviceName
        DestSettings.PhoneDeviceName2 = SourceSettings.PhoneDeviceName2
        DestSettings.RunOnStart = SourceSettings.RunOnStart
        DestSettings.RestartBTBeforeStart = SourceSettings.RestartBTBeforeStart
        DestSettings.PhoneBookAutoUpdate = SourceSettings.PhoneBookAutoUpdate
        DestSettings.PhoneBookUseVcardSupplement = SourceSettings.PhoneBookUseVcardSupplement
        DestSettings.EmergencyNumber = SourceSettings.EmergencyNumber
        DestSettings.VocalMessageryNumber = SourceSettings.VocalMessageryNumber
        DestSettings.AutoSwapPhone = SourceSettings.AutoSwapPhone
        DestSettings.PhoneSpeechNumbers = SourceSettings.PhoneSpeechNumbers
        DestSettings.PhoneSpeechRecognition = SourceSettings.PhoneSpeechRecognition
        DestSettings.PhoneCountryCodes.Clear()
        For i As Integer = 0 To 2 'avec for next
            DestSettings.PhoneCountryCodes.Add(SourceSettings.PhoneCountryCodes(i))
        Next
        'DestSettings.PhoneAlarm1 = SourceSettings.PhoneAlarm1
        'DestSettings.PhoneAlarm2 = SourceSettings.PhoneAlarm2
        'DestSettings.PhoneAlarm3 = SourceSettings.PhoneAlarm3
        'DestSettings.PhoneAlarm4 = SourceSettings.PhoneAlarm4
        'DestSettings.PhoneAlarm5 = SourceSettings.PhoneAlarm5
        DestSettings.LockInMotion = SourceSettings.LockInMotion
        DestSettings.LockInMotionForCMD = SourceSettings.LockInMotionForCMD
        DestSettings.PhoneExecATCmd = SourceSettings.PhoneExecATCmd
        DestSettings.Language = SourceSettings.Language
        DestSettings.AutoNotAnswerCallIn = SourceSettings.AutoNotAnswerCallIn
        DestSettings.PhonePANAutoRun = SourceSettings.PhonePANAutoRun
        DestSettings.PhoneNoAddContact = SourceSettings.PhoneNoAddContact
        DestSettings.PhoneNoSMSPopupInfo = SourceSettings.PhoneNoSMSPopupInfo
        DestSettings.PhoneStartupDelay = SourceSettings.PhoneStartupDelay
        DestSettings.PhonePinCode = SourceSettings.PhonePinCode
    End Sub

    Public Shared Sub SetToDefault(ByRef Settings As Object, Optional ResetTrue As Boolean = False)
        Try
            Settings.PhoneDebugLog = False
            Settings.PhoneDeviceName = "NONAME"
            Settings.PhoneDeviceName2 = "NONAME"
            Settings.RunOnStart = True
            Settings.RestartBTBeforeStart = False
            Settings.PhoneBookAutoUpdate = True
            Settings.PhoneBookUseVcardSupplement = False
            Settings.EmergencyNumber = "112"
            Settings.VocalMessageryNumber = "123"
            Settings.AutoSwapPhone = False
            Settings.PhoneSpeechNumbers = "zero,one,two,three,four,five,six,seven,eight,nine,more,star,sharp,delete,cancel,call,stop call,help call,by name,-,-,-"
            Settings.PhoneSpeechRecognition = False
            If ResetTrue = True Then Settings.PhoneCountryCodes = New List(Of String) From {"0", "33", "00"}
            If ResetTrue = False Then Settings.PhoneCountryCodes = New List(Of String)
            'Settings.PhoneAlarm1 = """01:00""|1|1|""Alarm Text 1""|""1"""
            'Settings.PhoneAlarm2 = """02:00""|2|1|""Alarm Text 2""|""1,2"""
            'Settings.PhoneAlarm3 = """03:00""|3|1|""Alarm Text 3""|""1,2,3"""
            'Settings.PhoneAlarm4 = """04:00""|4|1|""Alarm Text 4""|""1,2,3,4"""
            'Settings.PhoneAlarm5 = """05:00""|5|1|""Alarm Text 5""|""1,2,3,4,5"""
            Settings.LockInMotion = True
            Settings.LockInMotionForCMD = "NoCMDLock"
            Settings.PhoneExecATCmd = False
            Settings.Language = "english"
            Settings.AutoNotAnswerCallIn = False
            Settings.PhonePANAutoRun = True
            Settings.PhoneNoAddContact = True
            Settings.PhoneNoSMSPopupInfo = True
            Settings.PhoneStartupDelay = 2
            Settings.PhonePinCode = "0000"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Settings")
        End Try

    End Sub

    Public Shared Function Compare(ByRef Settings1 As cMySettings, ByRef Settings2 As cMySettings) As Boolean

        If Settings1.PhoneDebugLog <> Settings2.PhoneDebugLog Then Return False
        If Settings1.PhoneDeviceName <> Settings2.PhoneDeviceName Then Return False
        If Settings1.PhoneDeviceName2 <> Settings2.PhoneDeviceName2 Then Return False
        If Settings1.RunOnStart <> Settings2.RunOnStart Then Return False
        If Settings1.RestartBTBeforeStart <> Settings2.RestartBTBeforeStart Then Return False
        If Settings1.PhoneBookAutoUpdate <> Settings2.PhoneBookAutoUpdate Then Return False
        If Settings1.PhoneBookUseVcardSupplement <> Settings2.PhoneBookUseVcardSupplement Then Return False
        If Settings1.EmergencyNumber <> Settings2.EmergencyNumber Then Return False
        If Settings1.VocalMessageryNumber <> Settings2.VocalMessageryNumber Then Return False
        If Settings1.AutoSwapPhone <> Settings2.AutoSwapPhone Then Return False
        If Settings1.PhoneSpeechNumbers <> Settings2.PhoneSpeechNumbers Then Return False
        If Settings1.PhoneSpeechRecognition <> Settings2.PhoneSpeechRecognition Then Return False
        For i As Integer = 0 To 2
            If Not String.Compare(Settings1.PhoneCountryCodes(i), Settings2.PhoneCountryCodes(i)) = 0 Then
                Return False
            End If
        Next
        'If Settings1.PhoneAlarm1 <> Settings2.PhoneAlarm1 Then Return False
        'If Settings1.PhoneAlarm2 <> Settings2.PhoneAlarm2 Then Return False
        'If Settings1.PhoneAlarm3 <> Settings2.PhoneAlarm3 Then Return False
        'If Settings1.PhoneAlarm4 <> Settings2.PhoneAlarm4 Then Return False
        'If Settings1.PhoneAlarm5 <> Settings2.PhoneAlarm5 Then Return False
        If Settings1.LockInMotion <> Settings2.LockInMotion Then Return False
        If Settings1.LockInMotionForCMD <> Settings2.LockInMotionForCMD Then Return False
        If Settings1.PhoneExecATCmd <> Settings2.PhoneExecATCmd Then Return False
        If Settings1.Language <> Settings2.Language Then Return False
        If Settings1.AutoNotAnswerCallIn <> Settings2.AutoNotAnswerCallIn Then Return False
        If Settings1.PhonePANAutoRun <> Settings2.PhonePANAutoRun Then Return False
        If Settings1.PhoneNoAddContact <> Settings2.PhoneNoAddContact Then Return False
        If Settings1.PhoneNoSMSPopupInfo <> Settings2.PhoneNoSMSPopupInfo Then Return False
        If Settings1.PhoneStartupDelay <> Settings2.PhoneStartupDelay Then Return False
        If Settings1.PhonePinCode <> Settings2.PhonePinCode Then Return False
        Return True
    End Function

    Protected disposed As Boolean = False
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If disposing Then
                ' Insert code to free managed resources.
            End If
            ' Insert code to free unmanaged resources.
        End If
        Me.disposed = True
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub

End Class
