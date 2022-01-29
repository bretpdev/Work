Imports System.Collections.Generic
Imports RegentsScholarshipBackEnd

Public Class frmBatchQuickReview
    Private _user As User

    Public Sub New(ByVal u As User)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        _user = u
        Dim reviewItems As List(Of BatchQuickReviewItem) = DataAccess.GetStudentsWithReviewInProgress()
        Dim sortedReviewItems As List(Of BatchQuickReviewItem) = reviewItems.Select(Function(p) p).OrderBy(Function(p) p.FirstName).OrderBy(Function(p) p.LastName).ToList()
        For Each item As BatchQuickReviewItem In sortedReviewItems
            Dim aControl As New BatchReviewItemControl(item)
            If pnlReviews.Controls.Count Mod 2 = 0 Then
                aControl.BackColor = Color.White
            End If
            pnlReviews.Controls.Add(aControl)
        Next
    End Sub

    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        For Each cntrl As Control In pnlReviews.Controls
            CType(cntrl, BatchReviewItemControl).CompleteReview(_user)
        Next
        MessageBox.Show("The selected students have had the quick review completed. The system will now review their accounts and apply their final award status.", "Review Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Cursor = Cursors.WaitCursor
        Dim studentIds As List(Of String) = GetCheckedStudentIds()
        ApplicationReviews.RunPostFinalAwardReview(studentIds)
        Cursor = Cursors.Default
        MessageBox.Show("Final award statuses have been applied, and pending payments were created where appropriate.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Close()
    End Sub

    Private Sub frmBatchQuickReview_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If pnlReviews.Controls.Count = 0 Then
            MessageBox.Show("There don't appear to be any students with a first or second quick review in progress.", "No Quick Reviews Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnProcess.Enabled = False
        End If
    End Sub

    Private Function GetCheckedStudentIds() As List(Of String)
        Dim studentIds As New List(Of String)()
        For Each reviewItem As BatchReviewItemControl In pnlReviews.Controls
            If (reviewItem.chkReviewed.Checked AndAlso reviewItem.Item.ReviewInProgress = 2) Then studentIds.Add(reviewItem.Item.StateStudentId)
        Next reviewItem
        Return studentIds
    End Function
End Class