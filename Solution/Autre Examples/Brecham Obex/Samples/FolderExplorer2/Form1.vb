Imports Brecham.Obex
Imports Brecham.Obex.Objects
Imports Brecham.Obex.Net
Imports System.IO

Public Class Form1

#If 1 = 0 Then
    Public Overloads Property UseWaitCursor() As Boolean
        Get

        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.Cursor = Cursors.WaitCursor
            Else
                Me.Cursor = Cursors.Default
            End If
        End Set
    End Property
#End If

    '-------------------------------------------------------------------
    'Const ImageIndexFolder As Integer = 
    Const ImageIndexFile As Integer = 0

    '-------------------------------------------------------------------
    Private m_conn As GuiObexSessionConnection

    '-------------------------------------------------------------------
    Function CheckConnected() As Boolean
        Dim isConnected As Boolean = Not m_conn Is Nothing AndAlso m_conn.Connected
        If Not isConnected Then
            System.Diagnostics.Debug.Assert(Not Me.InvokeRequired, "CheckConnected at: Not Me.InvokeRequired")
            MessageBox.Show(Me, "Not connected", "Not connected")
        End If
        Return isConnected
    End Function

    Sub FillFromCurrentFolder()
        If Not CheckConnected() Then Exit Sub
        UseWaitCursor = True
        Me.foldersListBox.Items.Clear()
        Me.filesListView.Items.Clear()
        m_Items.Clear()
        Me.BackgroundWorkerFillCurrentFolder.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorkerFillCurrentFolder_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerFillCurrentFolder.DoWork
        'DEBUGGING System.Threading.Thread.Sleep(10000)
        Using stream As ObexGetStream = Me.m_conn.ObexClientSession.Get(Nothing, ObexConstant.Type.FolderListing)
            Dim parser As New ObexFolderListingParser(stream)
            Dim ignoreFaults As Boolean = Me.IgnoreFolderListingFaultsToolStripMenuItem.Checked
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
                If Me.BackgroundWorkerFillCurrentFolder.CancellationPending Then
                    e.Cancel = True
                    Exit Do
                End If
                '
                Dim item As ObexFolderListingItem = parser.GetNextItem
                ' Done last item?
                If (item Is Nothing) Then
                    Exit Do
                End If
                Me.BackgroundWorkerFillCurrentFolder.ReportProgress(0, item)
                count += 1
            Loop
            e.Result = count
        End Using
        ' Note exceptions will be passed throught to _RunWorkerCompleted
    End Sub

    Private Sub BackgroundWorkerFillCurrentFolder_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerFillCurrentFolder.RunWorkerCompleted
        System.Diagnostics.Debug.Assert(Not Me.InvokeRequired, "BackgroundWorkerFillCurrentFolder_RunWorkerCompleted: Not Me.InvokeRequired")
        UseWaitCursor = False
        Try
            Dim count As Integer
            Try
                count = CInt(e.Result)
                System.Diagnostics.Debug.Assert(e.Error Is Nothing, "BackgroundWorkerFillCurrentFolder_RunWorkerCompleted: e.Error Is Nothing")
                If Not e.Error Is Nothing Then
                    Console.Error.WriteLine("RWCEA.Result should have thrown:: " & e.Error.ToString())
                End If
            Catch tiex As System.Reflection.TargetInvocationException
                Console.WriteLine("tiex")
                MessageBox.Show(Me, "tiex", "tiex")
                Throw tiex.InnerException
            End Try
            If (e.Cancelled) Then
                Me.ToolStripStatusLabel1.Text = ("Listing was cancelled after " & count & " items.")
            Else
                Me.ToolStripStatusLabel1.Text = ("Listing contained " & count & " items.")
            End If
            'TODO Catch XmlException
        Catch obexEx As ObexResponseException
            UseWaitCursor = False
            MessageBox.Show(Me, "Error: " + obexEx.Message, "OBEX Folder Listing failed")
        Catch pvEx As System.Net.ProtocolViolationException
            UseWaitCursor = False
            MessageBox.Show(Me, "Error: " + pvEx.Message, "Invalid response from server")
        Catch ioex As IOException
            MessageBox.Show("Connection lost")
            m_conn = Nothing
        End Try
    End Sub

    ' The worker thread passes us a folder-listing item, add it to the 
    ' respective list box.
    ' Is this a little hacky?  Using the ReportProgress to do actual work, well
    ' it handles the worker thread to UI thread transition for us...
    Private Sub BackgroundWorkerFillCurrentFolder_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerFillCurrentFolder.ProgressChanged
        System.Diagnostics.Debug.Assert(Not Me.InvokeRequired, "BackgroundWorkerFillCurrentFolder_ProgressChanged: Not Me.InvokeRequired")
        ' Get the folder-listing item passed from the worker thread, and do 
        ' a preliminary cast to ensure that its of the correct type.
        Dim objItem As ObexFolderListingItem = CType(e.UserState, ObexFolderListingItem)
        ' For the three types of folder-listing item do a different action.
        If (objItem.GetType Is GetType(ObexParentFolderItem)) Then
            Me.foldersListBox.Items.Insert(0, "..")
        ElseIf (objItem.GetType Is GetType(ObexFolderItem)) Then
            Dim fi As ObexFolderItem = DirectCast(objItem, ObexFolderItem)
            Me.foldersListBox.Items.Add(fi.Name)
        ElseIf (objItem.GetType Is GetType(ObexFileItem)) Then
            Dim fi As ObexFileItem = DirectCast(objItem, ObexFileItem)
            Dim item As New ListViewItem(fi.Name, ImageIndexFile)
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
            Me.filesListView.Items.Add(item)
        Else
            System.Diagnostics.Debug.Fail("unknown folder item type")
        End If
        m_Items.Add(objItem)
    End Sub

    Private Sub TestFolderList1(ByVal sender As Object, ByVal e As EventArgs) Handles DummyFolder1ToolStripMenuItem.Click
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
        list.Add(itemFi)
        itemFi = New ObexFileItem("file5_0b")
        itemFi.Size = 0
        itemFi.Modified = #1/31/2007 11:26:23 PM#
        itemFi.Type = "text/plain"
        list.Add(itemFi)
        '
        ''Me.m_Items.Clear()
        For Each item In list
            BackgroundWorkerFillCurrentFolder_ProgressChanged(Nothing, _
                New System.ComponentModel.ProgressChangedEventArgs(0, item))
        Next
    End Sub


    Private Sub connectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles connectButton.Click
        ' Connect
        Me.foldersListBox.Items.Clear()
        Me.filesListView.Items.Clear()
        '
        Dim pf As System.Net.Sockets.ProtocolFamily = Me.ProtocolComboBox1.SelectedProtocol
        UseWaitCursor = True
        Me.m_conn = New GuiObexSessionConnection(pf, True, Me.ToolStripStatusLabel1)
        Try
            Dim connected As Boolean = Me.m_conn.Connect
            ' Fill from the folder-listing.
            If connected Then
                Me.FillFromCurrentFolder()
            End If
            UseWaitCursor = False
        Catch sEx As System.Net.Sockets.SocketException
            UseWaitCursor = False
            MessageBox.Show(Me, "Error: " + sEx.Message, "Connect failed")
        Catch ioEx As IOException
            UseWaitCursor = False
            MessageBox.Show(Me, "Error: " + ioEx.Message, "Connect failed")
        Catch obexEx As ObexResponseException
            UseWaitCursor = False
            If obexEx.ResponseCode = Pdus.ObexResponseCode.PeerUnsupportedService Then
                Dim title As String = "Peer doesn't support the Folder-Browsing OBEX service"
                Dim descr As String = obexEx.Description
                If String.IsNullOrEmpty(descr) Then
                    descr = title
                Else
                    descr = "Reason: " + descr
                End If
                MessageBox.Show(Me, descr, title)
            Else
                MessageBox.Show(Me, "Error: " + obexEx.Message, "OBEX Connect failed")
            End If
        Catch pvEx As System.Net.ProtocolViolationException
            UseWaitCursor = False
            MessageBox.Show(Me, "Error: " + pvEx.Message, "Invalid response from server")
        End Try
    End Sub

    Private Sub disconnectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles disconnectButton.Click
        If Not CheckConnected() Then Exit Sub
        Me.m_conn.Close()
        Me.ToolStripStatusLabel1.Text = "Disconnected"
    End Sub

    Private Sub foldersListBox_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles foldersListBox.DoubleClick
        If Not CheckConnected() Then Exit Sub
        Dim folderName As String = CStr(Me.foldersListBox.SelectedItem)
        ChangeFolder(folderName)
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

    Private Sub ListViewFiles_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles filesListView.DoubleClick
        If Not CheckConnected() Then Exit Sub
        Dim text1 As String = CStr(Me.filesListView.SelectedItems(0).Text)
        DownloadFile(text1)
    End Sub

    Sub DownloadFile(ByVal name As String)
        Dim form2 As FormFileGet = New FormFileGet()
        form2.Work = New FileDownloadArgs(name, Me.m_conn)
        Dim result1 As DialogResult = form2.ShowDialog(Me)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.filesListView.LargeImageList = New ImageList() 'Me.components)
        Me.filesListView.LargeImageList.ImageSize = New Size(32, 32)
        Me.filesListView.LargeImageList.Images.Add(My.Resources.Icon1)
        Me.filesListView.SmallImageList = New ImageList() 'Me.components)
        Me.filesListView.SmallImageList.Images.Add(My.Resources.Icon1)
        '
        Dim v As View = Me.filesListView.View
        Me.ComboBoxLvView.DataSource = [Enum].GetValues(GetType(View))
        Me.ComboBoxLvView.SelectedItem = v
        Me.m_Items = New List(Of ObexFolderListingItem)
    End Sub

    Private Sub ComboBoxLvView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxLvView.SelectedIndexChanged
        System.Diagnostics.Debug.Assert([Enum].GetNames(GetType(View)).Length = Me.ComboBoxLvView.Items.Count, _
            "ComboBoxLvView filled on Form load?")
        System.Diagnostics.Debug.Assert([Enum].IsDefined(GetType(View), Me.ComboBoxLvView.SelectedItem), _
            "ComboBoxLvView wrong item type!")
        Dim v As View = CType(Me.ComboBoxLvView.SelectedItem, View)
        Me.filesListView.View = v
    End Sub

    '-------------------------------------------------------------------
    Const MaxLinesPerPage As Integer = Integer.MaxValue
    '
    Private m_PrintDocument1_LineNumber As Integer
    Private m_Items As List(Of ObexFolderListingItem)

    Private Sub PrintDocument1_BeginPrint(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
#If DEBUG Then
        Console.WriteLine("PrintDocument1_BeginPrint")
#End If
        Me.m_PrintDocument1_LineNumber = 0
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        System.Diagnostics.Debug.Assert(m_Items IsNot Nothing, "No items--null.")
        System.Diagnostics.Debug.Assert(m_Items.Count <> 0, "No items--0.")
        If Me.m_Items Is Nothing OrElse Me.m_Items.Count = 0 Then
            Return
        End If
        Dim itemIdx As Integer
        Dim x, y As Single
        x = e.MarginBounds.Left
        y = e.MarginBounds.Top
        Dim lineHeight As Single = e.Graphics.MeasureString("WW", SystemFonts.DefaultFont).Height
        itemIdx = m_PrintDocument1_LineNumber
        For i As Integer = 0 To MaxLinesPerPage - 1
            If itemIdx >= m_Items.Count Then
                System.Diagnostics.Debug.Assert(i <> 0, _
                    "Shouldn't be a new page with zero items to print.")
                Exit For
            End If
            '
            If y + lineHeight > e.MarginBounds.Bottom Then
                Exit For
            End If
            '
            Dim objItem As ObexFolderListingItem = m_Items(itemIdx)
            Dim line As String
            If (TypeOf objItem Is ObexParentFolderItem) Then
                line = "../"
            ElseIf TypeOf objItem Is ObexFolderItem Then
                Dim fi As ObexFolderItem = DirectCast(objItem, ObexFolderItem)
                line = fi.Name + "/"
            Else
                System.Diagnostics.Debug.Assert(TypeOf objItem Is ObexFileItem)
                Dim fi As ObexFileItem = DirectCast(objItem, ObexFileItem)
                line = fi.Name
            End If
            Dim fmt As StringFormat = StringFormat.GenericDefault
            e.Graphics.DrawString(line, SystemFonts.DefaultFont, Brushes.Black, _
                x, y, fmt)
            y += lineHeight
            itemIdx += 1
        Next
        m_PrintDocument1_LineNumber = itemIdx
        e.HasMorePages = m_PrintDocument1_LineNumber < Me.m_Items.Count
        e.Graphics.DrawRectangle(Pens.Blue, e.MarginBounds)
#If DEBUG Then
        Console.WriteLine("PrintDocument1_PrintPage OUT")
#End If
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        If Me.m_Items Is Nothing OrElse Me.m_Items.Count = 0 Then
            MessageBox.Show("No items to print.")
            Return
        End If
        Dim rslt As Windows.Forms.DialogResult = Me.PrintDialog1.ShowDialog()
#If DEBUG Then
        Console.WriteLine("pd1.result: {0}", rslt)
#End If
        If rslt = Windows.Forms.DialogResult.OK Then
            Me.PrintDocument1.Print()
        End If
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        If Me.m_Items Is Nothing OrElse Me.m_Items.Count = 0 Then
            MessageBox.Show("No items to print.")
            Return
        End If
        Me.PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PageSetupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageSetupToolStripMenuItem.Click
        Dim rslt As DialogResult = Me.PageSetupDialog1.ShowDialog()
    End Sub

    '-------------------------------------------------------------------
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
End Class
