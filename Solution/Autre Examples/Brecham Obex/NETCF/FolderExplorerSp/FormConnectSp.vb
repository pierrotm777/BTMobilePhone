Imports Brecham.Obex.Net

Public Class FormConnectSp

    Private m_conn As ObexSessionConnection
    Private m_statusControl As Control

    Private Sub MenuItemOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemOk.Click
        Dim pf As System.Net.Sockets.ProtocolFamily = Me.ProtocolComboBox1.SelectedProtocol
        m_conn = New GuiObexSessionConnection(pf, True, Me.m_statusControl)
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Public WriteOnly Property StatusControl() As Control
        Set(ByVal value As Control)
            m_statusControl = value
        End Set
    End Property

    Public ReadOnly Property Connection() As ObexSessionConnection
        Get
            Return m_conn
        End Get
    End Property

    Private Sub MenuItemCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub


End Class