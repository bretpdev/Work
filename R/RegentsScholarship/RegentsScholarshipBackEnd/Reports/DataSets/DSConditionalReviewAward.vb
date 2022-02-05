Public Class DSConditionalReviewAward
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

    Private _baseAward As String
    Public Property BaseAward() As String
        Get
            Return _baseAward
        End Get
        Set(ByVal value As String)
            _baseAward = value
        End Set
    End Property

    Private _exemplaryAward As String
    Public Property ExemplaryAward() As String
        Get
            Return _exemplaryAward
        End Get
        Set(ByVal value As String)
            _exemplaryAward = value
        End Set
    End Property

    Private _awardType As String
    Public Property AwardType() As String
        Get
            Return _awardType
        End Get
        Set(ByVal value As String)
            _awardType = value
        End Set
    End Property
End Class
