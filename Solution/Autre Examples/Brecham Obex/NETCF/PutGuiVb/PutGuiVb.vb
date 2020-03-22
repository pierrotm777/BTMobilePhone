Imports System.IO
Imports Brecham.Obex    'eg ObexClientSession
Imports Brecham.Obex.Net    'eg GuiObexSessionConnection

Public Class Workings
    Private m_form As FormPutGuiVb

    Friend Sub New(ByVal form As FormPutGuiVb)
        m_form = form
    End Sub

    '---------------------------------------------------------------------------
    Friend Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs)
        m_form.statusLabel.Text = "Choose a file."
        Dim result1 As DialogResult = m_form.openFileDialog1.ShowDialog()
        If (result1 = System.Windows.Forms.DialogResult.OK) Then
            ' Open the source file
            m_form.statusLabel.Text = "Opening the source file..."
            Dim src As FileStream
            Try
                src = New FileStream(m_form.openFileDialog1.FileName, FileMode.Open, FileAccess.Read)
            Catch ex As Exception
                MessageBox.Show(ex.GetType().ToString() + ": " + ex.Message, "Open file failed")
                Return
            End Try
            ' Connect to the peer device and service
            Dim proto As System.Net.Sockets.ProtocolFamily = m_form.protocolComboBox1.SelectedProtocol
            'System.Net.Sockets.ProtocolFamily.InterNetwork,
            Dim conn As New GuiObexSessionConnection(proto, False, m_form.statusLabel)
            ' Set our receive size and restrict our send size
            conn.ObexBufferSize = 2028
            conn.MaxSendSize = 2048
            Dim connected As Boolean
            Try
                connected = conn.Connect
            Catch ex As Exception
                MessageBox.Show(ex.GetType().ToString() + ": " + ex.Message, "Connect failed")
                Return
            End Try
            If (Not connected) Then Exit Sub
            ' Initiate the PUT now
            m_form.statusLabel.Text = "Initiating the PUT..."
            Dim name As String = Path.GetFileName(m_form.openFileDialog1.FileName)
            m_form.cancelButton1.Enabled = True
            'Replace backgroundWorker1.RunWorkerAsync(New BgWorkerArgs(name, src, conn))
            RunWorkerAsync(New BgWorkerArgs(name, src, conn))
            m_form.statusLabel.Text = "Sending PUT content..."
        End If
    End Sub

    Friend Sub cancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        m_cancellationPending = True
    End Sub

    '---------------------------------------------------------------------------
    '---------------------------------------------------------------------------
    Sub DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) 'Handles backgroundWorker1.DoWork
        Dim args As BgWorkerArgs = CType(e.Argument, BgWorkerArgs)
        Dim buffer(1023) As Byte
        Dim length As Int32
        Dim updatePeriod As New TimeSpan(0, 0, 0, 0, 250)
        Dim lastProgress As DateTime = DateTime.UtcNow
        Dim elapsed As TimeSpan
        Dim putStream As ObexPutStream = Nothing
        '
        Try
            putStream = args.Connection.ObexClientSession.Put(args.Name, Nothing, args.Source.Length)
            ReportProgressPosition(0, args.Source.Length)
            While (True)
                If (m_cancellationPending) Then
                    e.Cancel = True
                    putStream.Abort("User cancelled")
                    Exit While
                End If
                length = args.Source.Read(buffer, 0, buffer.Length)
                If (length = 0) Then
                    Exit While
                End If
                putStream.Write(buffer, 0, length)
                ' Progress reporting; rate-limited.
                elapsed = DateTime.UtcNow.Subtract(lastProgress)
                If (elapsed.CompareTo(updatePeriod) > 0) Then
                    'If (elapsed > updatePeriod) Then
                    ReportProgressPosition(args.Source.Position, args.Source.Length)
                    lastProgress = DateTime.UtcNow
                End If
            End While
            ' The final update of the progress bar
            ReportProgressPosition(args.Source.Position, args.Source.Length)
        Finally
            If Not IsNothing(putStream) Then
                putStream.Close()
            End If
            args.Source.Close()
            ' If we wanted to do another Put to the same peer we wouldn't close 
            ' the session and its underlying network stream here.
            args.Connection.Dispose()
        End Try
    End Sub

    Private Sub ReportProgressPosition(ByVal position As Int64, ByVal length As Int64)
        Dim progressPercentage As Int32
        If length = 0 Then
            progressPercentage = 100
        Else
            progressPercentage = CType((100.0 * position) / length, Int32)
        End If
        Dim callback As ProgressChangedEventHandler = AddressOf ProgressChanged
        ReportProgress(progressPercentage)
    End Sub

    ' Update the progress bar on the UI thread
    Sub ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) 'Handles backgroundWorker1.ProgressChanged
        'Console.Write("" + e.ProgressPercentage.ToString + "% ")
        m_form.progressBar1.Value = e.ProgressPercentage
    End Sub

    Sub RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) 'Handles backgroundWorker1.RunWorkerCompleted
        m_form.cancelButton1.Enabled = False
        If (e.Cancelled) Then
            m_form.statusLabel.Text = "Cancelled"
            MessageBox.Show("Cancelled")
        ElseIf Not IsNothing(e.Error) Then
            ' Check explicitly if an exception that occured in DoWork!
            m_form.statusLabel.Text = e.Error.Message
            MessageBox.Show(e.Error.GetType().ToString() + ": " + e.Error.Message, "Failed")
        Else
            Dim obj As Object = e.Result
            m_form.statusLabel.Text = "Complete"
            MessageBox.Show("Complete")
        End If
    End Sub

    '---------------------------------------------------------------------------
    '---------------------------------------------------------------------------
    Class BgWorkerArgs
        ' Fields
        'Private ReadOnly m_putStream As ObexPutStream
        Private ReadOnly m_name As String
        Private ReadOnly m_source As Stream
        Private ReadOnly m_connection As ObexSessionConnection

        ' Properties
        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property

        Public ReadOnly Property Source() As Stream
            Get
                Return m_source
            End Get
        End Property

        Public ReadOnly Property Connection() As ObexSessionConnection
            Get
                Return m_connection
            End Get
        End Property


        ' Constructor
        Public Sub New(ByVal name As String, ByVal source As Stream, ByVal conn As ObexSessionConnection)
            If IsNothing(name) Then Throw New ArgumentNullException("name")
            If IsNothing(source) Then Throw New ArgumentNullException("source")
            If IsNothing(conn) Then Throw New ArgumentNullException("conn")
            m_name = name
            m_source = source
            m_connection = conn
        End Sub
    End Class 'BgWorkerArgs


    '---------------------------------------------------------------------------
    '---------------------------------------------------------------------------
#Region "MSFT BackgroundWorker simulated bits"
    Private m_cancellationPending As Boolean
    Private m_RunWorkerAsyncArguments As BgWorkerArgs

    Private m_pcEa As ProgressChangedEventArgs
    Private m_rwcEa As RunWorkerCompletedEventArgs

    Private Sub ReportProgress(ByVal percentProgress As Integer)
        ReportProgress(percentProgress, Nothing)
    End Sub

    Private Sub ReportProgress(ByVal percentProgress As Integer, ByVal userState As Object)
#If NETCF And FX1_1 Then
        ' NETCFv1 Control.BeginInvoke doesn't accept parameters
        m_pcEa = New ProgressChangedEventArgs(percentProgress, userState)
        Dim callback As EventHandler = AddressOf ProgressChanged__
        m_form.Invoke(callback)
#Else
        Dim callback As ProgressChangedEventHandler = AddressOf ProgressChanged
        Dim sender As Object = Nothing
        m_form.BeginInvoke(callback, New Object() {sender, New ProgressChangedEventArgs(percentProgress, userState)})
#End If
    End Sub

#If NETCF And FX1_1 Then
    Sub ProgressChanged__(ByVal sender As Object, ByVal e_ As EventArgs) 'Handles backgroundWorker1.ProgressChanged
        Dim e As ProgressChangedEventArgs = m_pcEa
        ProgressChanged(sender, e)
    End Sub
#End If


    Private Sub RunWorkerAsync(ByVal args As BgWorkerArgs)
        m_cancellationPending = False
        m_RunWorkerAsyncArguments = args
        Dim backgroundThread As System.Threading.Thread _
            = New System.Threading.Thread(AddressOf ThreadRunnerRunWorkerAsync)
        backgroundThread.Start()
    End Sub

    Private Sub ThreadRunnerRunWorkerAsync()
#If Not (NETCF And FX1_1) Then
        System.Threading.Thread.CurrentThread.IsBackground = True
#End If
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
#If NETCF And FX1_1 Then
        m_rwcEa = eRunCompleted
        Dim callback As New EventHandler(AddressOf RunWorkerCompleted__)
        m_form.Invoke(callback)
#Else
        Dim callback As New RunWorkerCompletedDelegate(AddressOf RunWorkerCompleted)
        m_form.BeginInvoke(callback, New Object() {Nothing, eRunCompleted})
#End If
    End Sub

#If NETCF And FX1_1 Then
    Sub RunWorkerCompleted__(ByVal sender As Object, ByVal e_ As EventArgs) 'Handles backgroundWorker1.ProgressChanged
        Dim e As RunWorkerCompletedEventArgs = m_rwcEa
        RunWorkerCompleted(sender, e)
    End Sub
#End If


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

    Public Sub New()

    End Sub
End Class
