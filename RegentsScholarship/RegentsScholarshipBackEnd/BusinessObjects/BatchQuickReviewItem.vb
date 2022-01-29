Public Class BatchQuickReviewItem
    Private __firstName As String
    Public Property FirstName() As String
        Get
            Return __firstName
        End Get
        Set(ByVal value As String)
            __firstName = value
        End Set
    End Property

    Private _lastname As String
    Public Property LastName() As String
        Get
            Return _lastname
        End Get
        Set(ByVal value As String)
            _lastname = value
        End Set
    End Property

    Private _reviewInProgress As Integer
    Public Property ReviewInProgress() As Integer
        Get
            Return _reviewInProgress
        End Get
        Set(ByVal value As Integer)
            _reviewInProgress = value
        End Set
    End Property

    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    ''' <summary>
    ''' Completes review that is in progress
    ''' </summary>
    ''' <param name="u"></param>
    ''' <remarks></remarks>
    Public Sub CompleteReview(ByVal u As User)
        If _reviewInProgress = 1 Then 'first quick review
            DataAccess.BatchFirstQuickReviewUpdate(u, _stateStudentId)
        Else 'second quick review
            DataAccess.BatchSecondQuickReviewUpdate(u, _stateStudentId)
        End If
    End Sub
End Class
