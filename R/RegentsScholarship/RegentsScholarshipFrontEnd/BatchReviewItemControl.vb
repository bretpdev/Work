Imports RegentsScholarshipBackEnd

Public Class BatchReviewItemControl


    Private _item As BatchQuickReviewItem
    Public Property Item() As BatchQuickReviewItem
        Get
            Return _item
        End Get
        Set(ByVal value As BatchQuickReviewItem)
            _item = value
        End Set
    End Property


    Public Sub New(ByVal reviewItem As BatchQuickReviewItem)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        _item = reviewItem
        BatchQuickReviewItemBindingSource.DataSource = reviewItem

    End Sub

    Public Sub CompleteReview(ByVal u As User)
        If chkReviewed.Checked Then
            _item.CompleteReview(u)
        End If
    End Sub

End Class
