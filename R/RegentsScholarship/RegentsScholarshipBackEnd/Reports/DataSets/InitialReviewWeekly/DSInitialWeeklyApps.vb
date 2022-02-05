Public Class DSInitialWeeklyApps
    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _pastWeek As Integer
    Public Property PastWeek() As Integer
        Get
            Return _pastWeek
        End Get
        Set(ByVal value As Integer)
            _pastWeek = value
        End Set
    End Property
End Class
