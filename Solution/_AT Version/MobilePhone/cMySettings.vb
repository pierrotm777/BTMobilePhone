Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Collections.Generic

Public Class cMySettings

    Public PhoneDebugLog As Boolean
    Public PhoneMacAddress As String
    Public PhoneMacAddress2 As String
    Public RunOnStart As Boolean
    Public PhoneBookUpdate As Boolean
    Public PhoneBookListUpdate As Boolean
    Public PhoneBookList As String
    Public SmsServiceCentreAddress As String
    Public EmergencyNumber As String
    Public AutoSwapPhone As Boolean
    Public PhoneSpeechNumbers As String
    Public PhoneSpeechRecognition As Boolean
    Public PhoneCountryCodes As String  'networkCarrierCode , countryCode As String,  internationalCarrierCode
    Public PhoneAlarm1 As String
    Public PhoneAlarm2 As String
    Public PhoneAlarm3 As String
    Public PhoneAlarm4 As String
    Public PhoneAlarm5 As String
    Public LockInMotion As Boolean

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
        Me.PhoneMacAddress = Settings.PhoneMacAddress
        Me.PhoneMacAddress2 = Settings.PhoneMacAddress2
        Me.RunOnStart = Settings.RunOnStart
        Me.PhoneBookUpdate = Settings.PhoneBookUpdate
        Me.PhoneBookListUpdate = Settings.PhoneBookListUpdate
        Me.PhoneBookList = Settings.PhoneBookList
        Me.SmsServiceCentreAddress = Settings.SmsServiceCentreAddress
        Me.EmergencyNumber = Settings.EmergencyNumber
        Me.AutoSwapPhone = Settings.AutoSwapPhone
        Me.PhoneSpeechNumbers = Settings.PhoneSpeechNumbers
        Me.PhoneSpeechRecognition = Settings.PhoneSpeechRecognition
        Me.PhoneCountryCodes = Settings.PhoneCountryCodes
        Me.PhoneAlarm1 = Settings.PhoneAlarm1
        Me.PhoneAlarm2 = Settings.PhoneAlarm2
        Me.PhoneAlarm3 = Settings.PhoneAlarm3
        Me.PhoneAlarm4 = Settings.PhoneAlarm4
        Me.PhoneAlarm5 = Settings.PhoneAlarm5
        Me.LockInMotion = Settings.LockInMotion
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
        DestSettings.PhoneMacAddress = SourceSettings.PhoneMacAddress
        DestSettings.PhoneMacAddress2 = SourceSettings.PhoneMacAddress2
        DestSettings.RunOnStart = SourceSettings.RunOnStart
        DestSettings.PhoneBookUpdate = SourceSettings.PhoneBookUpdate
        DestSettings.PhoneBookListUpdate = SourceSettings.PhoneBookListUpdate
        DestSettings.PhoneBookList = SourceSettings.PhoneBookList
        DestSettings.SmsServiceCentreAddress = SourceSettings.SmsServiceCentreAddress
        DestSettings.EmergencyNumber = SourceSettings.EmergencyNumber
        DestSettings.AutoSwapPhone = SourceSettings.AutoSwapPhone
        DestSettings.PhoneSpeechNumbers = SourceSettings.PhoneSpeechNumbers
        DestSettings.PhoneSpeechRecognition = SourceSettings.PhoneSpeechRecognition
        DestSettings.PhoneCountryCodes = SourceSettings.PhoneCountryCodes
        DestSettings.PhoneAlarm1 = SourceSettings.PhoneAlarm1
        DestSettings.PhoneAlarm2 = SourceSettings.PhoneAlarm2
        DestSettings.PhoneAlarm3 = SourceSettings.PhoneAlarm3
        DestSettings.PhoneAlarm4 = SourceSettings.PhoneAlarm4
        DestSettings.PhoneAlarm5 = SourceSettings.PhoneAlarm5
        DestSettings.LockInMotion = SourceSettings.LockInMotion

    End Sub

    Public Shared Sub SetToDefault(ByRef Settings)
        Try
            Settings.PhoneDebugLog = False
            Settings.PhoneMacAddress = "01:02:03:04:05:06"
            Settings.PhoneMacAddress2 = "07:08:09:10:11:12"
            Settings.RunOnStart = True
            Settings.PhoneBookUpdate = True
            Settings.PhoneBookListUpdate = True
            Settings.PhoneBookList = "DC,RC,MC,SM,ME"
            Settings.SmsServiceCentreAddress = "0102030405"
            Settings.EmergencyNumber = "112"
            Settings.AutoSwapPhone = False
            Settings.PhoneSpeechNumbers = "zero,one,two,three,four,five,six,seven,eight,nine,more,star,sharp,delete,cancel,call,stop call,help call,by name,-,-,-"
            Settings.PhoneSpeechRecognition = False
            Settings.PhoneCountryCodes = "33"
            Settings.PhoneAlarm1 = """01:00""|1|1|""Alarm Text 1""|""1"""
            Settings.PhoneAlarm2 = """02:00""|2|1|""Alarm Text 2""|""1,2"""
            Settings.PhoneAlarm3 = """03:00""|3|1|""Alarm Text 3""|""1,2,3"""
            Settings.PhoneAlarm4 = """04:00""|4|1|""Alarm Text 4""|""1,2,3,4"""
            Settings.PhoneAlarm5 = """05:00""|5|1|""Alarm Text 5""|""1,2,3,4,5"""
            Settings.LockInMotion = True

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Shared Function Compare(ByRef Settings1 As cMySettings, ByRef Settings2 As cMySettings) As Boolean

        If Settings1.PhoneDebugLog <> Settings2.PhoneDebugLog Then Return False
        If Settings1.PhoneMacAddress <> Settings2.PhoneMacAddress Then Return False
        If Settings1.PhoneMacAddress2 <> Settings2.PhoneMacAddress2 Then Return False
        If Settings1.RunOnStart <> Settings2.RunOnStart Then Return False
        If Settings1.PhoneBookUpdate <> Settings2.PhoneBookUpdate Then Return False
        If Settings1.PhoneBookListUpdate <> Settings2.PhoneBookListUpdate Then Return False
        If Settings1.PhoneBookList <> Settings2.PhoneBookList Then Return False
        If Settings1.SmsServiceCentreAddress <> Settings2.SmsServiceCentreAddress Then Return False
        If Settings1.EmergencyNumber <> Settings2.EmergencyNumber Then Return False
        If Settings1.AutoSwapPhone <> Settings2.AutoSwapPhone Then Return False
        If Settings1.PhoneSpeechNumbers <> Settings2.PhoneSpeechNumbers Then Return False
        If Settings1.PhoneSpeechRecognition <> Settings2.PhoneSpeechRecognition Then Return False
        If Settings1.PhoneCountryCodes <> Settings2.PhoneCountryCodes Then Return False
        If Settings1.PhoneAlarm1 <> Settings2.PhoneAlarm1 Then Return False
        If Settings1.PhoneAlarm2 <> Settings2.PhoneAlarm2 Then Return False
        If Settings1.PhoneAlarm3 <> Settings2.PhoneAlarm3 Then Return False
        If Settings1.PhoneAlarm4 <> Settings2.PhoneAlarm4 Then Return False
        If Settings1.PhoneAlarm5 <> Settings2.PhoneAlarm5 Then Return False
        If Settings1.LockInMotion <> Settings2.LockInMotion Then Return False

        Return True
    End Function

End Class
