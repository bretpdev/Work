Public Class DSReviewStatus
    Private _sortOrder As Integer
    Public Property SortOrder() As Integer
        Get
            Return _sortOrder
        End Get
        Set(ByVal value As Integer)
            _sortOrder = value
        End Set
    End Property

    Private _reviewStatus As String
    Public Property ReviewStatus() As String
        Get
            Return _reviewStatus
        End Get
        Set(ByVal value As String)
            _reviewStatus = value
        End Set
    End Property

    Private _highSchool As String
    Public Property HighSchool() As String
        Get
            Return _highSchool
        End Get
        Set(ByVal value As String)
            _highSchool = value
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
End Class
