Public Class DSFinalWeeklyReadyForReview
    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _range As String
    Public Property Range() As String
        Get
            Return _range
        End Get
        Set(ByVal value As String)
            _range = value
        End Set
    End Property
End Class
