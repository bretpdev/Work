Public Class DSInitialWeeklyStages
    Private _readyForReview As Integer
    Public Property ReadyForReview() As Integer
        Get
            Return _readyForReview
        End Get
        Set(ByVal value As Integer)
            _readyForReview = value
        End Set
    End Property

    Private _firstTranscriptReviewCompleted As Integer
    Public Property FirstTranscriptReviewCompleted() As Integer
        Get
            Return _firstTranscriptReviewCompleted
        End Get
        Set(ByVal value As Integer)
            _firstTranscriptReviewCompleted = value
        End Set
    End Property

    Private _secondTranscriptReviewCompleted As Integer
    Public Property SecondTranscriptReviewCompleted() As Integer
        Get
            Return _secondTranscriptReviewCompleted
        End Get
        Set(ByVal value As Integer)
            _secondTranscriptReviewCompleted = value
        End Set
    End Property

    Private _firstQuickReviewCompleted As Integer
    Public Property FirstQuickReviewCompleted() As Integer
        Get
            Return _firstQuickReviewCompleted
        End Get
        Set(ByVal value As Integer)
            _firstQuickReviewCompleted = value
        End Set
    End Property
End Class
