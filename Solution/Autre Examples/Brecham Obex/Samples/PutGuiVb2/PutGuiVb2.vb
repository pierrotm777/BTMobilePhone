Imports System.IO
Imports Brecham.Obex    'eg ObexClientSession
Imports Brecham.Obex.Net    'eg GuiObexSessionConnection
Imports System.ComponentModel


Public Class PutGuiVb2

    Private Sub browseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles browseButton.Click
        statusLabel.Text = "Choose a file."
        Dim result1 As DialogResult = openFileDialog1.ShowDialog(Me)
        If (result1 = System.Windows.Forms.DialogResult.OK) Then
            ' Open the source file
            statusLabel.Text = "Opening the source file..."
            Dim src As FileStream
            Try
                src = New FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read)
            Catch ex As Exception
                MessageBox.Show(Me, ex.GetType().ToString() + ": " + ex.Message, "Open file failed")
                Return
            End Try
            ' Connect to the peer device and service
            Dim pf As System.Net.Sockets.ProtocolFamily = ProtocolComboBox1.SelectedProtocol
            Dim conn As New GuiObexSessionConnection(pf, False, statusLabel)
            Dim connected As Boolean
            Try
                connected = conn.Connect
            Catch ex As Exception
                MessageBox.Show(Me, ex.GetType().ToString() + ": " + ex.Message, "Connect failed")
                Return
            End Try
            If (Not connected) Then Exit Sub
            ' Initiate the PUT now
            statusLabel.Text = "Initiating the PUT..."
            Dim name As String = Path.GetFileName(openFileDialog1.FileName)
            cancelButton1.Enabled = True
            backgroundWorker1.RunWorkerAsync(New BgWorkerArgs(name, src, conn))
            statusLabel.Text = "Sending PUT content..."
        End If
    End Sub


    Private Sub cancelButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cancelButton1.Click
        backgroundWorker1.CancelAsync()
    End Sub


    '---------------------------------------------------------------------------
    '---------------------------------------------------------------------------
    Sub DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles backgroundWorker1.DoWork
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
                If (backgroundWorker1.CancellationPending) Then
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
                elapsed = DateTime.UtcNow - lastProgress
                If (elapsed > updatePeriod) Then
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
        backgroundWorker1.ReportProgress(progressPercentage)
    End Sub

    ' Update the progress bar on the UI thread
    Sub ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles backgroundWorker1.ProgressChanged
        'Console.Write("" + e.ProgressPercentage.ToString + "% ")
        progressBar1.Value = e.ProgressPercentage
    End Sub

    Sub RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles backgroundWorker1.RunWorkerCompleted
        cancelButton1.Enabled = False
        If (e.Cancelled) Then
            statusLabel.Text = "Cancelled"
            MessageBox.Show(Me, "Cancelled", "Cancelled")
        ElseIf Not IsNothing(e.Error) Then
            ' Check explicitly if an exception that occured in DoWork!
            statusLabel.Text = e.Error.Message
            MessageBox.Show(Me, e.Error.GetType().ToString() + ": " + e.Error.Message, "Failed")
        Else
            Dim obj As Object = e.Result
            statusLabel.Text = "Complete"
            MessageBox.Show(Me, "Complete", "Complete")
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

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class

