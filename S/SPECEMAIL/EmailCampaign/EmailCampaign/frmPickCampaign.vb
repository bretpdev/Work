Imports System.Data.SqlClient

Public Class frmPickCampaign

    Private DS As New DataSet

    Private Sub frmPickCampaign_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'fill list view
        Dim DA As SqlDataAdapter
        Dim DR As DataRow
        Dim Comm As SqlCommand
        If CBool(My.Resources.TestMode) Then
            DA = New SqlDataAdapter("SELECT * FROM EMCP_DAT_EmailCampaigns WHERE DateComplete IS NULL", My.Resources.BSYSTestConn)
            Comm = New SqlCommand(String.Format("SELECT COUNT(*) FROM GENR_REF_BU_Agent_Xref WHERE BusinessUnit = 'Systems Support' and Role = 'Member Of' and WindowsUserID = '{0}'", Environment.UserName()), New SqlConnection(My.Resources.BSYSTestConn))
        Else
            DA = New SqlDataAdapter("SELECT * FROM EMCP_DAT_EmailCampaigns WHERE DateComplete IS NULL", My.Resources.BSYSLiveConn)
            Comm = New SqlCommand(String.Format("SELECT COUNT(*) FROM GENR_REF_BU_Agent_Xref WHERE BusinessUnit = 'Systems Support' and Role = 'Member Of' and WindowsUserID = '{0}'", Environment.UserName()), New SqlConnection(My.Resources.BSYSLiveConn))
        End If
        'check if the user should even be able to run this app
        '' ''Comm.Connection.Open()
        '' ''If CInt(Comm.ExecuteScalar()) = 0 Then
        '' ''    MessageBox.Show("You don't have access to run this application.  Please contact Systems Support", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        '' ''    End
        '' ''End If
        '' ''Comm.Connection.Close()
        DA.Fill(DS, "Data")
        For Each DR In DS.Tables("Data").Rows
            lvExistingCamp.Items.Add(New Campaign(DR))
        Next
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If lvExistingCamp.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select a campaign.", "Select Campaign", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Me.Hide()
            Dim Detail As New frmProvideDetail(lvExistingCamp.SelectedItems(0))
            Detail.ShowDialog()
            End
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Me.Hide()
        Dim Detail As New frmProvideDetail(Nothing)
        Detail.ShowDialog()
        End
    End Sub

    Private Sub lvExistingCamp_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvExistingCamp.DoubleClick
        Me.Hide()
        Dim Detail As New frmProvideDetail(lvExistingCamp.SelectedItems(0))
        Detail.ShowDialog()
        End
    End Sub
End Class
