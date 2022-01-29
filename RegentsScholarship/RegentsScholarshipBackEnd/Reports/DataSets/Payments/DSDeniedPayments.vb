Public Class DSDeniedPayments
    Private _college As String
    Public Property College() As String
        Get
            Return _college
        End Get
        Set(ByVal value As String)
            _college = value
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

    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
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

    Private _denialReasons As String
    Public Property DenialReasons() As String
        Get
            Return _denialReasons
        End Get
        Set(ByVal value As String)
            _denialReasons = value
        End Set
    End Property
End Class
