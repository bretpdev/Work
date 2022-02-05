Public Class DSUespAwardReview
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

    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _dateOfBirth As DateTime
    Public Property DateOfBirth() As DateTime
        Get
            Return _dateOfBirth
        End Get
        Set(ByVal value As DateTime)
            _dateOfBirth = value
        End Set
    End Property

    Private _ssn As String
    Public Property SSN() As String
        Get
            Return _ssn
        End Get
        Set(ByVal value As String)
            _ssn = value
        End Set
    End Property
End Class
