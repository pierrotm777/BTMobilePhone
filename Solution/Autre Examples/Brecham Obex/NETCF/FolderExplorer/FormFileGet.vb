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
        RunWorkerAsync(m_args)
    End Sub

    Private Sub DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)
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
                _ReportProgress(-1)
            End If
            Dim lastReport As Integer = 0
            Const reportPeriod As Integer = 250    'milliseconds
            Console.WriteLine("[Get-Metadata Length={0}]", totalLength)

            '---------------------------------
            ' Go!
            '---------------------------------
            While (True)
                If m_CancellationPending Then
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
                    ReportProgressPosition(curPosition, totalLength)
                    lastReport = Environment.TickCount
                End If
            End While
            ' Final progress report.
            ReportProgressPosition(curPosition, totalLength)
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

    Sub ReportProgressPosition(ByVal currentPosition As Int64, ByVal totalLength As Int64)
        Dim percentage As Int32
        If totalLength = 0 Then
            percentage = 0
        Else
            percentage = CInt(100 * currentPosition \ totalLength)
        End If
        _ReportProgress(percentage)
    End Sub

    ' Update the progress bar on the UI thread
    Private Sub ProgressChanged(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs)
        Console.Write(("" & e.ProgressPercentage & "% "))
        If e.ProgressPercentage = -1 Then
            'TODO 1109Me.ProgressBar1.Style = ProgressBarStyle.Marquee
        Else
            Me.ProgressBar1.Value = e.ProgressPercentage
        End If
    End Sub

    Private Sub RunWorkerCompleted(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs)
        'TODO 1109If Me.ProgressBar1.Style = ProgressBarStyle.Marquee Then
        'Me.ProgressBar1.Visible = False ' Stop the constant marquee movement.
        'End If
        If e.Cancelled Then
            Me.Label1.Text = "Cancelled"
            MessageBox.Show("Cancelled")
            MyBase.DialogResult = System.Windows.Forms.DialogResult.Cancel
        ElseIf Not e.Error Is Nothing Then
            Me.Label1.Text = "Download Failed."
            MessageBox.Show("Download Failed with Exception: " & e.Error.Message, "Download Failed")
            Me.Button1.Text = "Close"
        Else
            ' Success
            Dim obj1 As Object = e.Result
            Me.Label1.Text = "Complete"
            MessageBox.Show("Complete")
            Me.Button1.Text = "Close"
            Me.m_completedSuccessfully = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.m_completedSuccessfully Then
            MyBase.DialogResult = System.Windows.Forms.DialogResult.OK
        Else
            CancelAsync()
            MyBase.DialogResult = System.Windows.Forms.DialogResult.Abort
        End If
    End Sub

    ''TODO 1109Private Sub FormFileGet_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    'If e.CloseReason = CloseReason.UserClosing Then
    '    Me.m_manualInterrupting = True
    ' This is actually enough!
    '   Me.BackgroundWorkerFileDownload.CancelAsync()
    'Test this to get error with Belkin stack-- Me.m_args.GetStream.Close()
    '' TODO Me.m_args.GetStream.Abort("User cancelled")
    '  End If
    'End Sub


    '---------------------------------------------------------------------------
    '---------------------------------------------------------------------------
#Region "MSFT BackgroundWorker simulated bits"
    Private m_cancellationPending As Boolean
    Private m_RunWorkerAsyncArguments As Object 'TODO 1109BgWorkerArgs


    Private Sub _ReportProgress(ByVal percentProgress As Integer)
        _ReportProgress(percentProgress, Nothing)
    End Sub

    Private Sub _ReportProgress(ByVal percentProgress As Integer, ByVal userState As Object)
        Dim callback As ProgressChangedEventHandler = AddressOf ProgressChanged
        Dim sender As Object = Nothing
        Me.BeginInvoke(callback, New Object() {sender, New ProgressChangedEventArgs(percentProgress, userState)})
    End Sub

    Private Sub CancelAsync()
        m_cancellationPending = True
    End Sub

    Private Sub RunWorkerAsync()
        RunWorkerAsync(Nothing)
    End Sub

    Private Sub RunWorkerAsync(ByVal argument As Object)
        m_RunWorkerAsyncArguments = argument
        Dim backgroundThread As System.Threading.Thread _
            = New System.Threading.Thread(AddressOf ThreadRunnerRunWorkerAsync)
        backgroundThread.Start()
    End Sub

    Private Sub ThreadRunnerRunWorkerAsync()
        System.Threading.Thread.CurrentThread.IsBackground = True
        ' Do the Work
        Dim eDoWork As New DoWorkEventArgs(m_RunWorkerAsyncArguments)
        Dim exceptionFromDoWork As Exception = Nothing
        Try
            DoWork(Me, eDoWork)
        Catch ex As Exception
            ' Catch any exceptions it produces!
            exceptionFromDoWork = ex
        End Try
        ' Now call the 'completed' function, on the UI thread, passing the results!
        Dim eRunCompleted As New RunWorkerCompletedEventArgs(eDoWork.Result, exceptionFromDoWork, eDoWork.Cancel)
        Dim callback As New RunWorkerCompletedDelegate(AddressOf RunWorkerCompleted)
        Me.BeginInvoke(callback, New Object() {Nothing, eRunCompleted})
    End Sub


    Public Class DoWorkEventArgs
        Inherits System.ComponentModel.CancelEventArgs

        Public Sub New(ByVal argument As Object)
            m_argument = argument
        End Sub

        Public Property Result() As Object
            Get
                Return m_result
            End Get
            Set(ByVal value As Object)
                m_result = value
            End Set
        End Property

        Public ReadOnly Property Argument() As Object
            Get
                Return m_argument
            End Get
        End Property

        ' Fields
        Private m_argument As Object
        Private m_result As Object
    End Class 'DoWorkEventArgs

    Public Delegate Sub RunWorkerCompletedDelegate(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)

    Public Class RunWorkerCompletedEventArgs
        '      Inherits AsyncCompletedEventArgs

        'Fields
        Private m_result As Object
        Private m_error As Exception
        Private m_cancelled As Boolean

        ' Methods
        Public Sub New(ByVal result As Object, ByVal [error] As Exception, ByVal cancelled As Boolean)
            m_result = result
            m_error = [error]
            m_cancelled = cancelled
        End Sub

        ' Properties
        Public ReadOnly Property Result() As Object
            Get
                If Not IsNothing(m_error) Then
                    Throw m_error
                Else
                    Return m_result
                End If
            End Get
        End Property

        Public ReadOnly Property Cancelled() As Boolean
            Get
                Return m_cancelled
            End Get
        End Property

        Public ReadOnly Property [Error]() As Exception
            Get
                Return m_error
            End Get
        End Property

    End Class 'RunWorkerCompletedEventArgs


    Public Delegate Sub ProgressChangedEventHandler(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)

    Public Class ProgressChangedEventArgs
        Inherits EventArgs

        ' Methods
        Public Sub New(ByVal progressPercentage As Integer, ByVal userState As Object)
            m_percentage = progressPercentage
            m_userState = userState
        End Sub

        ' Properties
        Public ReadOnly Property ProgressPercentage() As Integer
            Get
                Return m_percentage
            End Get
        End Property

        Public ReadOnly Property UserState() As Object
            Get
                Return m_userState
            End Get
        End Property

        ' Fields
        Private ReadOnly m_percentage As Integer
        Private ReadOnly m_userState As Object
    End Class 'ProgressChangedEventArgs

#End Region
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
            If m_dest IsNot Nothing Then m_dest.Close()
        Finally
            If m_getStream IsNot Nothing Then m_getStream.Close()
        End Try
    End Sub
End Class 'FileDownloadArgs
