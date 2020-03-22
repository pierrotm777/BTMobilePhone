Imports System.IO.Ports
Imports System.Text
Imports System.Threading
Imports System.Text.RegularExpressions

Imports ATSMS.Common



Public Class SerialDriver : Inherits SerialPort

    Private Const BUFFER_SIZE As Integer = 16384
    Private Const TIME_OUT As Integer = 3 * 1000
    Private Const WAIT_DATA_RETRIES = 50

    Private portEncoding As System.Text.Encoding
    Private isSendingCommand As Boolean

    'private rxOK as new Regex("\\s*[\\x00-\\x7F]*\\s+OK\\s", RegexOptions.Compiled or RegexOptions.IgnoreCase);
    'private rxERROR1 as new Regex("\\s*[\\x00-\\x7F]*\\s+ERROR\\s", RegexOptions.Compiled or RegexOptions.IgnoreCase);
    'private rxERROR2 as New Regex ("\\s*[\\x00-\\x7F]*\\s+ERROR: \\d+\\s", RegexOptions.Compiled or RegexOptions.IgnoreCase);
    'private rxREADY as new Regex("\\s*[\\x00-\\x7F]*\\s+READY\\s", RegexOptions.Compiled or RegexOptions.IgnoreCase);
    'private rxPIN as New Regex ("\\s*[\\x00-\\x7F]*\\s+SIM PIN\\s", RegexOptions.Compiled or RegexOptions.IgnoreCase);

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        Me.ReadTimeout = TIME_OUT
        Me.WriteTimeout = TIME_OUT
        portEncoding = New ASCIIEncoding()
        isSendingCommand = False
    End Sub


    ''' <summary>
    ''' Sending AT commands to the serial port
    ''' </summary>
    ''' <param name="commandString"></param>
    ''' <remarks></remarks>
    Public Sub Send(ByVal commandString As String)
        Try
            Dim callText As String
            Dim callCommand As Byte()
            Dim iCount As Integer
            callText = commandString
            iCount = 0
            callCommand = portEncoding.GetBytes(callText)
            isSendingCommand = True
            Me.Write(callCommand, 0, callCommand.Length)
            While Me.BytesToRead <= 0
                Thread.Sleep(1000)
                iCount = iCount + 1
                If (iCount = 20) Then
                    isSendingCommand = False
                    Exit While
                End If
            End While
            isSendingCommand = False
        Catch ex As System.Exception
            isSendingCommand = False
            Throw New InvalidCommandException(ex.Message, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Send AT command
    ''' </summary>
    ''' <param name="commandString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SendCmd(ByVal commandString As String) As String
        If Not commandString.EndsWith(Chr(26)) Then
            If Not (commandString.EndsWith(ControlChars.Cr) Or _
                commandString.EndsWith(ControlChars.CrLf)) Then
                commandString = commandString + ControlChars.Cr
            End If
        End If
        Send(commandString)
        If BytesToRead > 0 Then
            Return ReadExisting()
        End If
        Return String.Empty
    End Function


    ''' <summary>
    ''' Send AT command and expect certain response
    ''' </summary>
    ''' <param name="commandString"></param>
    ''' <param name="expectedResponse"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SendCmd(ByVal commandString As String, _
                              ByVal expectedResponse As String) As String
        Dim response As String = ""
        Dim iCount As Integer
        If Not commandString.EndsWith(Chr(26)) Then
            If Not (commandString.EndsWith(ControlChars.Cr) Or _
                commandString.EndsWith(ControlChars.CrLf)) Then
                commandString = commandString + ControlChars.Cr
            End If
        End If
        Send(commandString)
        If BytesToRead > 0 Then
            response = ReadExisting()
            If response Is Nothing Or response.IndexOf(expectedResponse) < 0 Then
                If Not response Is Nothing Then
                    If response.Trim = ATHandler.RESPONSE_ERROR Then
                        Throw New UnknownException("Unknown exception in sending command")
                    End If
                    If (response.IndexOf(ATHandler.RESPONSE_CMS_ERROR) >= 0) Then
                        ' Error in the command
                        Dim cols() As String = response.Split(":")
                        If cols.Length > 1 Then
                            Dim errorCode As String = cols(1)

                            ' Check the error code in the resource
                            Dim errorDescription As String = Resource.GetMessage(errorCode.Trim)

                            ' Set error code and description

                            ' Throw exception
                            If errorDescription <> "" Then
                                Throw New InvalidCommandException(errorDescription)
                            Else
                                Throw New UnknownException("CMS Error code " + errorCode)
                            End If
                        End If
                    End If
                End If

                For iCount = 1 To 100
                    Thread.Sleep(1000)
                    response = response + ReadExisting()
                    If response.IndexOf(expectedResponse) >= 0 Then
                        Exit For
                    End If
                Next
            End If
        End If
        Return response
    End Function


    ''' <summary>
    ''' Send diagnostics commands
    ''' </summary>
    ''' <param name="commandString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Diagnose(ByVal commandString) As String
        Try
            Dim commands() As String = commandString.Split(ControlChars.Cr)
            Dim i As Integer
            Dim response As String = String.Empty
            For i = 0 To commands.Length - 1
                If commands(i).Trim <> String.Empty Then
                    response = response & commands(i) & ControlChars.CrLf
                    response = response & SendCmd(commands(i).Trim)
                End If
            Next
            Return response
        Catch ex As System.Exception
            Throw New InvalidCommandException(ex.Message, ex)
        End Try
        Return String.Empty
    End Function

    ''' <summary>
    ''' Send diagnostics command in a file
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="outputFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Diagnose(ByVal fileName As String, ByVal outputFileName As String) As Boolean
        Try
            Dim fileReader As String
            fileReader = My.Computer.FileSystem.ReadAllText(fileName)
            Dim commands() As String = fileReader.Split(ControlChars.Cr)
            Dim i As Integer
            Dim response As String = String.Empty
            For i = 0 To commands.Length - 1
                If commands(i).Trim <> String.Empty Then
                    response = response & commands(i) & ControlChars.CrLf
                    response = response & SendCmd(commands(i).Trim)
                End If
            Next
            My.Computer.FileSystem.WriteAllText(outputFileName, response, True)
            Return True
        Catch ex As System.Exception
            Throw New InvalidCommandException(ex.Message, ex)
        End Try
        Return False
    End Function


    ''' <summary>
    ''' Read response from the serial port
    ''' </summary>
    ''' <param name="expectedResponse"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReadResponse(ByVal expectedResponse As String) As String

        Dim response As String
        Dim iCount As Integer
        response = ""
        For iCount = 1 To 60
            response = ReadExisting()
            If response.Trim = ATHandler.RESPONSE_ERROR Then
                Throw New UnknownException("Unknown exception in retrieving data")
            End If

            If (response.IndexOf(ATHandler.RESPONSE_CMS_ERROR) >= 0) Then
                ' Error in the command
                Dim cols() As String = response.Split(":")
                If cols.Length > 1 Then
                    Dim errorCode As String = cols(1)

                    ' Check the error code in the resource
                    Dim errorDescription As String = Resource.GetMessage(errorCode.Trim)

                    ' Set error code and description

                    ' Throw exception
                    If errorDescription <> "" Then
                        Throw New InvalidCommandException(errorDescription)
                    Else
                        Throw New UnknownException("CMS Error code " + errorCode)
                    End If
                End If
            End If
            If response.IndexOf(expectedResponse) >= 0 Then
                Exit For
            End If
            Thread.Sleep(1000)
        Next
        Return response
    End Function

    ''' <summary>
    ''' Indicate if command is being sent
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsCommandMode() As Boolean
        Get
            Return isSendingCommand
        End Get
    End Property

End Class

