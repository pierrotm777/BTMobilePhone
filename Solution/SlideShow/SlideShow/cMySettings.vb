Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text

Public Class cMySettings

    Public SendCommandToRR As String
    Public PictureSize_X As Integer
    Public PictureSize_Y As Integer
    Public BackGroundColor As String
    Public TextColor As String
    Public TextSize As Integer
    Public TextFont As String


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
        Me.SendCommandToRR = Settings.SendCommandToRR
        Me.PictureSize_X = Settings.PictureSize_X
        Me.PictureSize_Y = Settings.PictureSize_Y
        Me.BackGroundColor = Settings.BackGroundColor
        Me.TextColor = Settings.TextColor
        Me.TextSize = Settings.TextSize
        Me.TextFont = Settings.TextFont

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

        DestSettings.SendCommandToRR = SourceSettings.SendCommandToRR
        DestSettings.PictureSize_X = SourceSettings.PictureSize_X
        DestSettings.PictureSize_Y = SourceSettings.PictureSize_Y
        DestSettings.BackGroundColor = SourceSettings.BackGroundColor
        DestSettings.TextColor = SourceSettings.TextColor
        DestSettings.TextSize = SourceSettings.TextSize
        DestSettings.TextFont = SourceSettings.TextFont
    End Sub

    Public Shared Sub SetToDefault(ByRef Settings)
        Settings.SendCommandToRR = "MOBILEPHONE_ADDPICTURETOITEM;"
        Settings.PictureSize_X = 70
        Settings.PictureSize_Y = 70
        Settings.BackGroundColor = "0,0,0"
        Settings.TextColor = "255,255,255"
        Settings.TextSize = 10
        Settings.TextFont = "Arial"
    End Sub

    Public Shared Function Compare(ByRef Settings1 As cMySettings, ByRef Setting2 As cMySettings) As Boolean

        If Settings1.SendCommandToRR <> Setting2.SendCommandToRR Then Compare = False : Exit Function
        If Settings1.PictureSize_X <> Setting2.PictureSize_X Then Compare = False : Exit Function
        If Settings1.PictureSize_Y <> Setting2.PictureSize_Y Then Compare = False : Exit Function
        If Settings1.BackGroundColor <> Setting2.BackGroundColor Then Compare = False : Exit Function
        If Settings1.TextColor <> Setting2.TextColor Then Compare = False : Exit Function
        If Settings1.TextSize <> Setting2.TextSize Then Compare = False : Exit Function
        If Settings1.TextFont <> Setting2.TextFont Then Compare = False : Exit Function
        Compare = True

    End Function

End Class
