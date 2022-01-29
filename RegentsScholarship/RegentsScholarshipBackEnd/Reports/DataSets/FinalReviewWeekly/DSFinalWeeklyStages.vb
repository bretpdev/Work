Public Class DSFinalWeeklyStages
    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _finalTranscriptReviewCompleted As Integer
    Public Property FinalTranscriptReviewCompleted() As Integer
        Get
            Return _finalTranscriptReviewCompleted
        End Get
        Set(ByVal value As Integer)
            _finalTranscriptReviewCompleted = value
        End Set
    End Property

    Private _secondQuickReviewCompleted As Integer
    Public Property SecondQuickReviewCompleted() As Integer
        Get
            Return _secondQuickReviewCompleted
        End Get
        Set(ByVal value As Integer)
            _secondQuickReviewCompleted = value
        End Set
    End Property
End Class
