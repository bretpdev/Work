Public Class DSAwardStatusBySchool

    Private _district As String
    Public Property District() As String
        Get
            Return _district
        End Get
        Set(ByVal value As String)
            _district = value
        End Set
    End Property

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
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

    Private _address1 As String
    Public Property Address1() As String
        Get
            Return _address1
        End Get
        Set(ByVal value As String)
            _address1 = value
        End Set
    End Property

    Private _address2 As String
    Public Property Address2() As String
        Get
            Return _address2
        End Get
        Set(ByVal value As String)
            _address2 = value
        End Set
    End Property

    Private _city As String
    Public Property City() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property

    Private _abbreviation As String
    Public Property Abbreviation() As String
        Get
            Return _abbreviation
        End Get
        Set(ByVal value As String)
            _abbreviation = value
        End Set
    End Property

    Private _zip As String
    Public Property Zip() As String
        Get
            Return _zip
        End Get
        Set(ByVal value As String)
            _zip = value
        End Set
    End Property

    Private _country As String
    Public Property Country() As String
        Get
            Return _country
        End Get
        Set(ByVal value As String)
            _country = value
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

    Private _denied As Integer
    Public Property Denied() As Integer
        Get
            Return _denied
        End Get
        Set(ByVal value As Integer)
            _denied = value
        End Set
    End Property

    Private _incomplete As Integer
    Public Property Incomplete() As Integer
        Get
            Return _incomplete
        End Get
        Set(ByVal value As Integer)
            _incomplete = value
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
End Class
