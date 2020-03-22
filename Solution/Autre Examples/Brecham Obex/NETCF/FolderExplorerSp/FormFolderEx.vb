Imports Brecham.Obex
Imports Brecham.Obex.Objects
Imports Brecham.Obex.Net
Imports System.IO

Public Class FormFolderEx
    '-------------------------------------------------------------------
    Const ImageIndexFolder As Integer = 0
    Const ImageIndexFile As Integer = 1

    '-------------------------------------------------------------------
    Private m_conn As ObexSessionConnection

    '-------------------------------------------------------------------
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub '

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub '

    '-------------------------------------------------------------------
    Function CheckConnected() As Boolean
        Dim isConnected As Boolean = Not m_conn Is Nothing AndAlso m_conn.Connected
        If Not isConnected Then
            System.Diagnostics.Debug.Assert(Not Me.InvokeRequired, "CheckConnected at: Not Me.InvokeRequired")
            MessageBox.Show("Not connected", "Not connected")
        End If
        Return isConnected
    End Function

    Sub FillFromCurrentFolder()
        If Not CheckConnected() Then Exit Sub
        Cursor.Current = Cursors.WaitCursor
        Me.ListView1.Items.Clear()
        Me.RunWorkerAsync()
    End Sub

    Private Sub DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)
        'DEBUGGING System.Threading.Thread.Sleep(10000)
        Using stream As ObexGetStream = Me.m_conn.ObexClientSession.Get(Nothing, ObexConstant.Type.FolderListing)
            Dim parser As New ObexFolderListingParser(stream)
            Dim ignoreFaults As Boolean = Me.IgnoreFolderListingFaultsMenuItem.Checked
            parser.IgnoreUnknownAttributeNames = ignoreFaults
            parser.IgnoreBadDateFormats = ignoreFaults
            Dim count As Integer = 0
            Do While True
                ' User cancelled?  (Not implemented in UI as can't test with my devices
                ' as no listing takes any time to complete).
                ' In fact the listing arrives so quickly that reading them individually
                ' and adding them individually to the listboxes may add overhead, so it
                ' might be better to simply read them all, or at least to batch them in 
                ' groups to add to the list boxes.
                If m_cancellationPending Then
                    e.Cancel = True
                    Exit Do
                End If
                '
                Dim item As ObexFolderListingItem = parser.GetNextItem
                ' Done last item?
                If (item Is Nothing) Then
                    Exit Do
                End If
                ReportProgress(-1, item)
                count += 1
            Loop
            e.Result = count
        End Using
        ' Note exceptions will be passed throught to _RunWorkerCompleted
    End Sub

    Private Sub RunWorkerCompleted(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs)
        System.Diagnostics.Debug.Assert(Not Me.InvokeRequired, "BackgroundWorkerFillCurrentFolder_RunWorkerCompleted: Not Me.InvokeRequired")
        Cursor.Current = Cursors.Default
        Try
            Dim count As Integer
            count = CInt(e.Result)
        If (e.Cancelled) Then
            Me.Label1.Text = ("Listing was cancelled after " & count & " items.")
        Else
            Me.Label1.Text = ("Listing contained " & count & " items.")
        End If
            'TODO Catch XmlException
        Catch obexEx As ObexResponseException
            MessageBox.Show("Error: " + obexEx.Message, "OBEX Folder Listing failed")
        Catch pvEx As System.Net.ProtocolViolationException
            MessageBox.Show("Error: " + pvEx.Message, "Invalid response from server")
        Catch ioex As IOException
            MessageBox.Show("Connection lost")
            m_conn = Nothing
        End Try
    End Sub

    ' The worker thread passes us a folder-listing item, add it to the 
    ' respective list box.
    ' Is this a little hacky?  Using the ReportProgress to do actual work, well
    ' it handles the worker thread to UI thread transition for us...
    Private Sub ProgressChanged(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs)
        System.Diagnostics.Debug.Assert(Not Me.InvokeRequired, "BackgroundWorkerFillCurrentFolder_ProgressChanged: Not Me.InvokeRequired")
        ' Get the folder-listing item passed from the worker thread, and do 
        ' a preliminary cast to ensure that its of the correct type.
        Dim objItem As ObexFolderListingItem = CType(e.UserState, ObexFolderListingItem)
        ' For the three types of folder-listing item do a different action.
        Dim item As New ListViewItem
        item.Tag = objItem
        If (objItem.GetType Is GetType(ObexParentFolderItem)) Then
            item.Text = ".."
            item.ImageIndex = ImageIndexFolder
        ElseIf (objItem.GetType Is GetType(ObexFolderItem)) Then
            Dim fi As ObexFolderItem = DirectCast(objItem, ObexFolderItem)
            item.Text = fi.Name
            item.ImageIndex = ImageIndexFolder
        ElseIf (objItem.GetType Is GetType(ObexFileItem)) Then
            Dim fi As ObexFileItem = DirectCast(objItem, ObexFileItem)
            item.Text = fi.Name
            item.ImageIndex = ImageIndexFile
            Dim sizeStr As String
            If fi.HasSize Then
                Dim size As Long = fi.Size
                ' Convert to KB (ensure if one byte or more never display it as '0 KB')
                If (size <> 0) Then
                    Dim sizeK As Long = CLng(Math.Ceiling(size / 1024)) 'convert to KB
                    ' Ensure we've not made a zero value on rescaling, but allow for
                    ' the Broadcom/Widcomm bug, which returns files >2G as negative sized!
                    System.Diagnostics.Debug.Assert(sizeK > 0 OrElse size < 0, "sizeK > 0")
                    size = sizeK
                End If
                sizeStr = size.ToString() + " KB"
            Else
                sizeStr = String.Empty
            End If
            item.SubItems.Add(sizeStr)
            item.SubItems.Add(fi.Type)
            If fi.Modified <> DateTime.MinValue Then
                item.SubItems.Add(fi.Modified.ToString())
            Else
                item.SubItems.Add(String.Empty)
            End If
        Else
            System.Diagnostics.Debug.Fail("unknown folder item type")
        End If
        Me.ListView1.Items.Add(item)
    End Sub

    Private Sub TestFolderList1(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItem6.Click
        Dim list As New List(Of ObexFolderListingItem)
        Dim item As ObexFolderListingItem, itemFi As ObexFileItem ', itemFo As ObexFolderItem
        '
        item = New ObexParentFolderItem
        list.Add(item)
        itemFi = New ObexFileItem("file2_2gb")
        itemFi = New ObexFileItem("file1")
        itemFi.Size = 2 * 1024
        list.Add(itemFi)
        item = New ObexFolderItem("folder2")
        list.Add(item)
        itemFi = New ObexFileItem("file2_2gb")
        itemFi.Size = Integer.MaxValue
        itemFi.Modified = #10/3/2007 11:25:22 AM#
        itemFi.Type = "image/jpeq"
        list.Add(itemFi)
        itemFi = New ObexFileItem("file3_1b")
        itemFi.Size = 1
        itemFi.Modified = #1/31/2007 11:26:23 PM#
        itemFi.Type = "text/plain"
        list.Add(itemFi)
        itemFi = New ObexFileItem("file4_noSize")
        itemFi.Type = "text/plain"
        list.Add(itemFi)
        itemFi = New ObexFileItem("file5_0b")
        itemFi.Size = 0
        itemFi.Modified = #1/31/2007 11:26:23 PM#
        itemFi.Type = "text/plain"
        list.Add(itemFi)
        '
        For Each item In list
            ProgressChanged(Nothing, New ProgressChangedEventArgs(0, item))
        Next
    End Sub

    Private Sub connectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        ' Connect
        Me.ListView1.Items.Clear()
        '
        Dim form1 As New FormConnectSp
        form1.StatusControl = Me.Label1
        Dim rslt As DialogResult = form1.ShowDialog()
        If rslt <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        Me.m_conn = form1.Connection
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim connected As Boolean = Me.m_conn.Connect
            ' Fill from the folder-listing.
            If connected Then
                Me.FillFromCurrentFolder()
            End If
            Cursor.Current = Cursors.Default
        Catch sEx As System.Net.Sockets.SocketException
            Cursor.Current = Cursors.Default
            MessageBox.Show("Error: " + " code: " + sEx.ErrorCode.ToString() + " " + sEx.Message, _
                "Connect failed")
        Catch ioEx As IOException
            Cursor.Current = Cursors.Default
            MessageBox.Show("Error: " + ioEx.Message, "Connect failed")
        Catch obexEx As ObexResponseException
            Cursor.Current = Cursors.Default
            If obexEx.ResponseCode = Pdus.ObexResponseCode.PeerUnsupportedService Then
                Dim title As String = "Peer doesn't support the Folder-Browsing OBEX service"
                Dim descr As String = obexEx.Description
                If String.IsNullOrEmpty(descr) Then
                    descr = title
                Else
                    descr = "Reason: " + descr
                End If
                MessageBox.Show(descr, title)
            Else
                MessageBox.Show("Error: " + obexEx.Message, "OBEX Connect failed")
            End If
        Catch pvEx As System.Net.ProtocolViolationException
            Cursor.Current = Cursors.Default
            MessageBox.Show("Error: " + pvEx.Message, "Invalid response from server")
        End Try
    End Sub

    Private Sub disconnectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem7.Click
        If Not CheckConnected() Then Exit Sub
        Me.m_conn.Close()
        Me.Label1.Text = "Disconnected"
    End Sub

    Private Sub MenuItemQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemQuit.Click
        Dim rslt As DialogResult = MessageBox.Show("Confirm quit?", "Confirm quit?", _
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If rslt = DialogResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub ListView1_ItemActivate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.ItemActivate
        If Not CheckConnected() Then Exit Sub
        Dim indexes As ListView.SelectedIndexCollection = Me.ListView1.SelectedIndices
        If indexes.Count <> 0 Then
            Dim itemIdx As Integer = indexes(0)
            Dim obj As Object = Me.ListView1.Items(itemIdx).Tag
            Dim item As ObexFolderListingItem = DirectCast(obj, ObexFolderListingItem)
            If item.GetType Is GetType(ObexParentFolderItem) Then
                ChangeFolder("..")
            ElseIf item.GetType Is GetType(ObexFolderItem) Then
                Dim fi As ObexFolderItem = DirectCast(item, ObexFolderItem)
                ChangeFolder(fi.Name)
            Else
                Dim fi As ObexFileItem = DirectCast(item, ObexFileItem)
                DownloadFile(fi.Name)
            End If
        End If
    End Sub

    Sub ChangeFolder(ByVal folderName As String)
        Try
        If (folderName Is "..") Then
            Me.m_conn.ObexClientSession.SetPathUp()
        Else
            Me.m_conn.ObexClientSession.SetPath(folderName)
        End If
            Me.FillFromCurrentFolder()
        Catch obrex As ObexResponseException
            MessageBox.Show(obrex.Message, "Folder change failed")
        Catch ioex As IOException
            MessageBox.Show("Connection lost")
            m_conn.Close()
        End Try
    End Sub

    Sub DownloadFile(ByVal name As String)
        Dim form2 As FormFileGet = New FormFileGet()
        form2.Work = New FileDownloadArgs(name, Me.m_conn)
        Dim result1 As DialogResult = form2.ShowDialog()
    End Sub

    Private Sub MenuItemViewStyle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim menuItem As MenuItem = DirectCast(sender, MenuItem)
        Dim style As View = DirectCast([Enum].Parse(GetType(View), menuItem.Text, False), View)
        Me.ListView1.View = style
    End Sub

    Private Sub InitViewStyleUi()
        Dim viewValues() As View = {View.List, View.SmallIcon, View.Details, View.LargeIcon}
        Dim subMenu As MenuItem = Me.MenuItem3
        subMenu.MenuItems.Clear()
        For Each style As View In viewValues
            Dim curMenu As New MenuItem()
            curMenu.Text = style.ToString()
            AddHandler curMenu.Click, New EventHandler(AddressOf MenuItemViewStyle_Click)
            subMenu.MenuItems.Add(curMenu)
        Next
        Me.MenuItem3.Enabled = True
    End Sub

    Private Sub IgnoreFolderListingFaultsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IgnoreFolderListingFaultsMenuItem.Click
        Dim mi As MenuItem = DirectCast(sender, MenuItem)
        mi.Checked = Not mi.Checked
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' view hacking
        InitViewStyleUi()
        ' Images
        Dim imagesS As New ImageList
        Dim imagesL As New ImageList
        imagesS.ImageSize = New Size(16, 16)
        imagesL.ImageSize = New Size(32, 32)
        System.Diagnostics.Debug.Assert(imagesS.Images.Count = ImageIndexFolder)
        imagesS.Images.Add(My.Resources.Folder)
        imagesL.Images.Add(My.Resources.Folder)
        System.Diagnostics.Debug.Assert(imagesS.Images.Count = ImageIndexFile)
        imagesS.Images.Add(My.Resources.textdoc)
        imagesL.Images.Add(My.Resources.textdoc)
        Me.ListView1.SmallImageList = imagesS
        Me.ListView1.LargeImageList = imagesL
    End Sub

    '---------------------------------------------------------------------------
    '---------------------------------------------------------------------------
    Class BgWorkerArgs
        ' Fields
        'Private ReadOnly m_putStream As ObexPutStream
        Private ReadOnly m_name As String
        Private ReadOnly m_connection As ObexSessionConnection

        ' Properties
        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property


        Public ReadOnly Property Connection() As ObexSessionConnection
            Get
                Return m_connection
            End Get
        End Property


        ' Constructor
        Public Sub New(ByVal name As String, ByVal conn As ObexSessionConnection)
            If IsNothing(name) Then Throw New ArgumentNullException("name")
            If IsNothing(conn) Then Throw New ArgumentNullException("conn")
            m_name = name
            m_connection = conn
        End Sub
    End Class 'BgWorkerArgs


    '---------------------------------------------------------------------------
    '---------------------------------------------------------------------------
#Region "MSFT BackgroundWorker simulated bits"
    Private m_cancellationPending As Boolean
    Private m_RunWorkerAsyncArguments As BgWorkerArgs


    Private Sub ReportProgress(ByVal percentProgress As Integer)
        ReportProgress(percentProgress, Nothing)
    End Sub

    Private Sub ReportProgress(ByVal percentProgress As Integer, ByVal userState As Object)
        Dim callback As ProgressChangedEventHandler = AddressOf ProgressChanged
        Dim sender As Object = Nothing
        Me.BeginInvoke(callback, New Object() {sender, New ProgressChangedEventArgs(percentProgress, userState)})
    End Sub


    Private Sub RunWorkerAsync()
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

End Class
