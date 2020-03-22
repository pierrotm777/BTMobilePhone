Imports System.IO

Module modGlobalDeclare
    Public SDKIsReady As Boolean = False
    Public DebuLogIsActive As Boolean = False
    Public RRPath As String = ""
    Public RRScreen As String = ""
    Public RRSkinPath As String = ""
    Public FrontEndName As String = ""
    Public INI As IniFile
    Public INIPath As String = ""
    Public MousePositionX As String = ""
    Public MousePositionY As String = ""
    Public caretIndex As Integer = 0
    Public lineNumber As Integer = 0

    Public Function CheckIfNppExist() As Boolean
        CheckIfNppExist = False
        Dim env As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        If File.Exists(env & "\Notepad++\notepad++.exe") = True Then
            CheckIfNppExist = True
        Else
            CheckIfNppExist = False
        End If
        Return CheckIfNppExist
    End Function

    Public Function ResizeImage(ByVal InputImage As Image) As Image
        Return New Bitmap(InputImage, New Size(22, 22))
    End Function

    Public Function CheckIfRunning(sProcessName As String) As Boolean
        Dim bRet As Boolean = False
        Try
            Dim listProc() As System.Diagnostics.Process
            listProc = System.Diagnostics.Process.GetProcessesByName(sProcessName)
            If listProc.Length > 0 Then
                ' Process is running
                bRet = True
            Else
                ' Process is not running
                bRet = False
            End If
        Catch ex As Exception

        End Try
        Return bRet
    End Function
End Module

