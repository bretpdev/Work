Imports RegentsScholarshipBackEnd

Public Class frmTransactionAuditHistory

    Private Sub frmTransactionAuditHistory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Load the combo box data sources.
            LoadRelevantUserIds("")
            LoadRelevantStudentIds("")
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnViewTransactionHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewTransactionHistory.Click
        ShowReport()
    End Sub

    Private Sub cmbUserId_DropDownClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbUserId.DropDownClosed
        Try
            LoadRelevantStudentIds(If(cmbUserId.SelectedItem Is Nothing, "", cmbUserId.SelectedItem.ToString()))
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub cmbStudentId_DropDownClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbStudentId.DropDownClosed
        Try
            LoadRelevantUserIds(If(cmbStudentId.SelectedItem Is Nothing, "", cmbStudentId.SelectedItem.ToString()))
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub LoadRelevantStudentIds(ByVal userId As String)
        'Grab the current student ID so we can go back to it if it's still part of the resulting list.
        Dim previouslySelectedStudentId As String = cmbStudentId.Text

        Dim studentIds As List(Of String) = DataAccess.GetTransactionStudentIds(userId).ToList()
        studentIds.Insert(0, "")
        cmbStudentId.DataSource = studentIds

        'Put the student ID back in the text field if it's still part of the list.
        If studentIds.Contains(previouslySelectedStudentId) Then
            cmbStudentId.Text = previouslySelectedStudentId
        End If
    End Sub

    Private Sub LoadRelevantUserIds(ByVal studentId As String)
        'Grab the current user ID so we can go back to it if it's still part of the resulting list.
        Dim previsoulySelectedUserId As String = cmbUserId.Text

        Dim userIds As List(Of String) = DataAccess.GetTransactionUserIds(studentId).ToList()
        userIds.Insert(0, "")
        cmbUserId.DataSource = userIds

        'Put the user ID back in the text field if it's still part of the list.
        If userIds.Contains(previsoulySelectedUserId) Then
            cmbUserId.Text = previsoulySelectedUserId
        End If
    End Sub

    Private Sub ShowReport()
        'Check that at least one combo box has text.
        If String.IsNullOrEmpty(cmbUserId.Text) AndAlso String.IsNullOrEmpty(cmbStudentId.Text) Then
            Dim message As String = "You must select a user ID and/or student ID for whom you wish to view transactions."
            MessageBox.Show(message, "No ID selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Cursor = Cursors.WaitCursor
        Try
            'Get an initial queryable of the Transaction table.
            Dim transactions As IEnumerable(Of Transaction) = DataAccess.GetTransactionRecords(cmbUserId.Text, cmbStudentId.Text)

            'Declare a report, set its data source, and load the report into the viewer.
            Dim report As New TransactionHistory()
            report.SetDataSource(transactions.ToList())
            If (radSortByDate.Checked) Then
                report.DataDefinition.SortFields(0).Field = report.Database.Tables(0).Fields("TimeStamp")
                report.DataDefinition.SortFields(1).Field = report.Database.Tables(0).Fields("UserId")
            Else
                report.DataDefinition.SortFields(0).Field = report.Database.Tables(0).Fields("UserId")
                report.DataDefinition.SortFields(1).Field = report.Database.Tables(0).Fields("TimeStamp")
            End If
            CrystalReportViewer1.ReportSource = report
            CrystalReportViewer1.RefreshReport()
            Cursor = Cursors.Default
        Catch ex As Exception
            Cursor = Cursors.Default
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub
End Class