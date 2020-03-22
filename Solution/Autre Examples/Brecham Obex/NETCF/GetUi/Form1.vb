Imports Brecham.Obex.Net
Imports System.IO
Imports Brecham.Obex

Public Class Form1

    Private m_conn As GuiObexSessionConnection

    '---------------------------------------------------------------------------

    Private Sub buttonConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonConnect.Click
        Dim pf As System.Net.Sockets.ProtocolFamily = Me.protocolComboBox1.SelectedProtocol
        m_conn = New GuiObexSessionConnection(pf, True, Me.StatusLabel)
        Try
            If (Not m_conn.Connect()) Then
                m_conn.Dispose()
                m_conn = Nothing
                Return
            End If
            EnableActionButtons()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Connect failed.")
        End Try
    End Sub

    Private Sub buttonDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDisconnect.Click
        DisableActionButtons()
        If Not m_conn Is Nothing Then
            m_conn.Dispose()
            m_conn = Nothing
        End If
    End Sub

    Private Sub buttonGetTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonGetTo.Click
        DoGet(True)
    End Sub

    Private Sub buttonGetStrm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonGetStrm.Click
        DoGet(False)
    End Sub

    '-------------------------------------------------------------------
    Private Sub DoGet(ByVal useGetToOrOtheriseUseGetStream As Boolean)
        Dim name As String = Me.textBoxFileName.Text
        If (name.Length = 0) Then
            name = Nothing
        End If
        Dim type As String = Me.comboBoxFileType.Text
        If (type.Length = 0) Then
            type = Nothing
        End If
        '
        Me.SaveFileDialog1.FileName = name
        Dim rslt As DialogResult = Me.SaveFileDialog1.ShowDialog()
        If (rslt <> Windows.Forms.DialogResult.OK) Then
            Return
        End If
        ' The following should be done on a background thread...
        Try
            Using dst As FileStream = File.OpenWrite(Me.SaveFileDialog1.FileName)
                If (useGetToOrOtheriseUseGetStream) Then
                    m_conn.ObexClientSession.GetTo(dst, name, type)
                Else
                    Using src As Stream = m_conn.ObexClientSession.Get(name, type)
                        Dim buf(2048) As Byte
                        While (True)
                            Dim count As Int32 = src.Read(buf, 0, buf.Length)
                            If count = 0 Then
                                Exit While
                            End If
                            dst.Write(buf, 0, count)
                        End While
                    End Using
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Download failed.")
        End Try
        MessageBox.Show("Success", "Download complete.")
    End Sub

    '-------------------------------------------------------------------
    Private Sub EnableActionButtons()
        Me.buttonConnect.Enabled = False
        Me.ButtonDisconnect.Enabled = True
        '
        'Me.buttonSetPath.Enabled = True
        'Me.buttonSetPathUp.Enabled = True
        Me.buttonGetTo.Enabled = True
        Me.buttonGetStrm.Enabled = True
        '
        Me.textBoxFileName.Enabled = True
        Me.comboBoxFileType.Enabled = True
    End Sub

    Private Sub DisableActionButtons()
        Me.buttonConnect.Enabled = True
        Me.ButtonDisconnect.Enabled = False
        '
        Me.buttonGetTo.Enabled = False
        Me.buttonGetStrm.Enabled = False
        '
        Me.textBoxFileName.Enabled = False
        Me.comboBoxFileType.Enabled = False
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim mediaTypeArray() As String = New String() { _
                String.Empty, _
                ObexConstant.Type.FolderListing, _
                ObexConstant.Type.Capability, _
                ObexConstant.Type.ObjectProfile, _
                ObexConstant.Type.TextVcard _
            }
        Me.comboBoxFileType.DataSource = mediaTypeArray
        DisableActionButtons()
    End Sub

End Class
