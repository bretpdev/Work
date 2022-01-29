Public Class MainMenuSearchResult
    Private _firstName As String
    Private _lastName As String
    Private _stateStudentId As String
    Private _socialSecurityNumber As String
    Private _streetAddressLine1 As String
    Private _city As String
    Private _awardStatus As String

    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Public Property StateStudentID() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Public Property SocialSecurityNumber() As String
        Get
            Return _socialSecurityNumber
        End Get
        Set(ByVal value As String)
            _socialSecurityNumber = value
        End Set
    End Property

    Public Property StreetAddressLine1() As String
        Get
            Return _streetAddressLine1
        End Get
        Set(ByVal value As String)
            _streetAddressLine1 = value
        End Set
    End Property

    Public Property City() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property

    Public Property AwardStatus() As String
        Get
            Return _awardStatus
        End Get
        Set(ByVal value As String)
            _awardStatus = value
        End Set
    End Property
End Class
