Imports System.IO
Imports RegentsScholarshipBackEnd

Public Class frmBatch
    Private Enum PrinterSetting
        None
        Duplex
        Simplex
    End Enum

    Private _printerSetting As PrinterSetting

    Private Sub btnLetters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLetters.Click
        PrintSelectedLetters()
    End Sub

    Private Sub btnQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuit.Click
        Application.Exit()
    End Sub

    Private Sub btnReviews_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReviews.Click
        RunReviews()
    End Sub

    Private Sub btnWeeklyReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWeeklyReports.Click
        ShowWeeklyReports()
    End Sub

    Private Sub PrintSelectedLetters()
        'Check that the user has access to the documents area.
        Try
            Dim testFile As String = Constants.STUDENT_DOCUMENT_ROOT + "testAccess"
            Using testStream As New StreamWriter(testFile, False)
                testStream.Close()
            End Using
            File.Delete(testFile)
        Catch ex As Exception
            Dim message As String = String.Format("You do not have access to the {0} folder, where the students' documents are to be saved. Contact CNOC to gain read/write access to this area.", Constants.STUDENT_DOCUMENT_ROOT)
            MessageBox.Show(message, "Insufficient Access", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End Try

        Me.Enabled = False

        If (chkDenialForIncompleteApplication.Checked) Then
            If (_printerSetting <> PrinterSetting.Simplex) Then
                MessageBox.Show("Set the printer to simplex. Press OK when the printer is ready.", "Simplex Printing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                _printerSetting = PrinterSetting.Simplex
            End If
            ApplicationLetters.PrintDenialForIncompleteApplication()
        End If

        If (chkEverythingElse.Checked) Then
            'Print the simplex letters first.
            If (_printerSetting <> PrinterSetting.Simplex) Then
                MessageBox.Show("Set the printer to simplex. Press OK when the printer is ready.", "Simplex Printing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                _printerSetting = PrinterSetting.Simplex
            End If
            Cursor = Cursors.WaitCursor
            ApplicationLetters.PrintSimplexLetters()
            Cursor = Cursors.Default

            'Print the duplex letters.
            If (_printerSetting <> PrinterSetting.Duplex) Then
                MessageBox.Show("Set the printer to duplex for the remainder of the letters. Press OK when the printer is ready.", "Duplex Printing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                _printerSetting = PrinterSetting.Duplex
            End If
            Cursor = Cursors.WaitCursor
            ApplicationLetters.PrintDuplexLetters()
            Cursor = Cursors.Default
        End If

        'Prompt to set the printer back to simplex.
        MessageBox.Show("Printing is complete. Set the printer back to simplex.", "Printing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Enabled = True
    End Sub

    Private Sub RunReviews()
        Me.Enabled = False
        Cursor = Cursors.WaitCursor
        ApplicationReviews.RunAllReviews()
        Cursor = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub ShowWeeklyReports()
        Dim reportName As String = "Initial Review Weekly Report"
        If DateTime.Now >= New DateTime(Constants.CURRENT_AWARD_YEAR, 5, 1) Then reportName = "Final Review Weekly Report"
        Try
            Dim reportsForm As New frmReports()
            reportsForm.Show()
            reportsForm.RunReport(reportName)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _printerSetting = PrinterSetting.None
    End Sub
End Class