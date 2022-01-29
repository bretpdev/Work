Public Class DSAppStatus
    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Private _lastName As String
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _description As String
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Private _award As String
    Public Property Award() As String
        Get
            Return _award
        End Get
        Set(ByVal value As String)
            _award = value
        End Set
    End Property

    Private _cohortYear As String
    Public Property CohortYear() As String
        Get
            Return _cohortYear
        End Get
        Set(ByVal value As String)
            _cohortYear = value
        End Set
    End Property
End Class
