Public Class DSDocRequestDecision
    Private _studentsName As String
    Public Property StudentsName() As String
        Get
            Return _studentsName
        End Get
        Set(ByVal value As String)
            _studentsName = value
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

    Private _numDaysOutstanding As Integer
    Public Property NumDaysOutstanding() As Integer
        Get
            Return _numDaysOutstanding
        End Get
        Set(ByVal value As Integer)
            _numDaysOutstanding = value
        End Set
    End Property
End Class
