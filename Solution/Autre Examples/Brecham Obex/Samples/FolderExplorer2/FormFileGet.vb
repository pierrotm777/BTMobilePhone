Imports Brecham.Obex
Imports System.IO
Imports Brecham.Obex.Net


Public Class FormFileGet
    ' Fields
    Private m_completedSuccessfully As Boolean
    Private m_args As FileDownloadArgs
    Private m_manualInterrupting As Boolean

    ' Properties
    Friend Property Work() As FileDownloadArgs
        Get
            Return m_args
        End Get
        Set(ByVal value As FileDownloadArgs)
            If value Is Nothing Then Throw New ArgumentNullException("value")
            m_args = value
        End Set
    End Property

    ' Methods
    Private Sub FormFileGet_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Label1.Text = "Receiving GET content..."
        Me.BackgroundWorkerFileDownload.RunWorkerAsync(m_args)
    End Sub

    Private Sub BackgroundWorkerFileDownload_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerFileDownload.DoWork
        Dim args As FileDownloadArgs = DirectCast(e.Argument, FileDownloadArgs)
        Try
            Dim buffer As Byte() = New Byte(1024 - 1) {}

            '---------------------------------
            ' See if the server has given us the total download length
            '---------------------------------
            ' Force the arrival of the first content (body) header to force
            ' the arrival of all the metadata (hopefully).
            args.GetStream.Read(buffer, 0, 0)
            Dim headers As ObexHeaderCollection = args.GetStream.ResponseHeaders
            Dim totalLength As Int64 = 0
            Dim curPosition As Int64 = 0
            If headers.Contains(ObexHeaderId.Length) Then
                totalLength = headers.GetFourByteAsUnsigned(ObexHeaderId.Length)
            End If
            If (totalLength = 0) Then
                Me.BackgroundWorkerFileDownload.ReportProgress(-1)
            End If
            Dim lastReport As Integer = 0
            Const reportPeriod As Integer = 250    'milliseconds
            Console.WriteLine("[Get-Metadata Length={0}]", totalLength)

            '---------------------------------
            ' Go!
            '---------------------------------
            While (True)
                If Me.BackgroundWorkerFileDownload.CancellationPending Then
                    e.Cancel = True
                    args.GetStream.Abort("User cancelled")
                    Return
                End If
                Dim length As Integer = args.GetStream.Read(buffer, 0, buffer.Length)
                If (length = 0) Then
                    Exit While
                End If
                args.Destination.Write(buffer, 0, length)
                curPosition = curPosition + length
                ' Progress reporting!
                If totalLength <> 0 _
                        AndAlso Environment.TickCount - lastReport > reportPeriod Then
                    ReportProgress(curPosition, totalLength)
                    lastReport = Environment.TickCount
                End If
            End While
            ' Final progress report.
            ReportProgress(curPosition, totalLength)
        Catch
            ' Suppress all exceptions when forced shutdown
            If m_manualInterrupting Then
                ' Ensure that this doesn't look like a successful completion!
                e.Cancel = True
            Else
                Throw
            End If
        Finally
            args.Dispose()
        End Try
    End Sub

    Sub ReportProgress(ByVal currentPosition As Int64, ByVal totalLength As Int64)
        Dim percentage As Int32
        If totalLength = 0 Then
            percentage = 0
        Else
            percentage = CInt(100 * currentPosition \ totalLength)
        End If
        Me.BackgroundWorkerFileDownload.ReportProgress(percentage)
    End Sub

    ' Update the progress bar on the UI thread
    Private Sub BackgroundWorkerFileDownload_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerFileDownload.ProgressChanged
        Console.Write(("" & e.ProgressPercentage & "% "))
        If e.ProgressPercentage = -1 Then
            Me.ProgressBar1.Style = ProgressBarStyle.Marquee
        Else
            Me.ProgressBar1.Value = e.ProgressPercentage
        End If
    End Sub

    Private Sub BackgroundWorkerFileDownload_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerFileDownload.RunWorkerCompleted
        If Me.ProgressBar1.Style = ProgressBarStyle.Marquee Then
            Me.ProgressBar1.Visible = False ' Stop the constant marquee movement.
        End If
        If e.Cancelled Then
            Me.Label1.Text = "Cancelled"
            MessageBox.Show("Cancelled")
            MyBase.DialogResult = System.Windows.Forms.DialogResult.Cancel
        ElseIf Not e.Error Is Nothing Then
            Me.Label1.Text = "Download Failed."
            MessageBox.Show(Me, "Download Failed with Exception: " & e.Error.Message, "Download Failed")
            Me.Button1.Text = "Close"
            MyBase.AcceptButton = Me.Button1
            MyBase.CancelButton = Me.Button1
        Else
            ' Success
            Dim obj1 As Object = e.Result
            Me.Label1.Text = "Complete"
            MessageBox.Show("Complete")
            Me.Button1.Text = "Close"
            MyBase.AcceptButton = Me.Button1
            MyBase.CancelButton = Nothing
            Me.m_completedSuccessfully = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.m_completedSuccessfully Then
            MyBase.DialogResult = System.Windows.Forms.DialogResult.OK
        Else
            Me.BackgroundWorkerFileDownload.CancelAsync()
        End If
    End Sub

    Private Sub FormFileGet_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            Me.m_manualInterrupting = True
            ' This is actually enough!
            Me.BackgroundWorkerFileDownload.CancelAsync()
            'Test this to get error with Belkin stack-- Me.m_args.GetStream.Close()
            ' TODO Me.m_args.GetStream.Abort("User cancelled")
        End If
    End Sub

End Class 'FormFileGet


Class FileDownloadArgs
    Implements IDisposable
    ' Fields
    Private ReadOnly m_connection As ObexSessionConnection
    Private ReadOnly m_fileName As String
    '
    Private m_getStream As ObexGetStream
    Private m_dest As Stream

    ' Properties
    Public ReadOnly Property Connection() As ObexSessionConnection
        Get
            Return Me.m_connection
        End Get
    End Property

    Public ReadOnly Property GetStream() As ObexGetStream
        Get
            If m_getStream Is Nothing Then
                m_getStream = Connection.ObexClientSession.Get(m_fileName, Nothing)
            End If
            Return Me.m_getStream
        End Get
    End Property

    Public ReadOnly Property Destination() As Stream
        Get
            If m_dest Is Nothing Then
                m_dest = New FileStream(m_fileName, FileMode.Create, FileAccess.Write)
            End If
            Return Me.m_dest
        End Get
    End Property

    ' Methods
    Public Sub New(ByVal fileName As String, ByVal conn As ObexSessionConnection)
        If (fileName Is Nothing) Then
            Throw New ArgumentNullException("fileName")
        End If
        If (conn Is Nothing) Then
            Throw New ArgumentNullException("conn")
        End If
        Me.m_fileName = fileName
        Me.m_connection = conn
    End Sub

    '
    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            If m_dest IsNot Nothing Then m_dest.Dispose()
        Finally
            If m_getStream IsNot Nothing Then m_getStream.Dispose()
        End Try
    End Sub

End Class 'FileDownloadArgs
