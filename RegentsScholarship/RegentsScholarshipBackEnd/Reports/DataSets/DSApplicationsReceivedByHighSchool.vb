Public Class DSApplicationsReceivedByHighSchool
    Private _schoolName As String
    Public Property SchoolName() As String
        Get
            Return _schoolName
        End Get
        Set(ByVal value As String)
            _schoolName = value
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

    Private _applicationReceivedLate As Integer
    Public Property ApplicationReceivedLate() As Integer
        Get
            Return _applicationReceivedLate
        End Get
        Set(ByVal value As Integer)
            _applicationReceivedLate = value
        End Set
    End Property

    Private _applicationReceivedOnTime As Integer
    Public Property ApplicationReceivedOnTime() As Integer
        Get
            Return _applicationReceivedOnTime
        End Get
        Set(ByVal value As Integer)
            _applicationReceivedOnTime = value
        End Set
    End Property

    Private _baseAward As Integer
    Public Property BaseAward() As Integer
        Get
            Return _baseAward
        End Get
        Set(ByVal value As Integer)
            _baseAward = value
        End Set
    End Property

    Private _exemplaryAward As Integer
    Public Property ExemplaryAward() As Integer
        Get
            Return _exemplaryAward
        End Get
        Set(ByVal value As Integer)
            _exemplaryAward = value
        End Set
    End Property

    Private _uespAward As Integer
    Public Property UespAward() As Integer
        Get
            Return _uespAward
        End Get
        Set(ByVal value As Integer)
            _uespAward = value
        End Set
    End Property

    Private _denied As Integer
    Public Property Denied() As Integer
        Get
            Return _denied
        End Get
        Set(ByVal value As Integer)
            _denied = value
        End Set
    End Property
End Class
