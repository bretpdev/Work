Public Class DSFinalWeeklyDeferments
    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _pending As Integer
    Public Property Pending() As Integer
        Get
            Return _pending
        End Get
        Set(ByVal value As Integer)
            _pending = value
        End Set
    End Property
End Class
