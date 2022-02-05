Public Class DSNewCenturyMatches
    Private _studentName As String
    Public Property StudentName() As String
        Get
            Return _studentName
        End Get
        Set(ByVal value As String)
            _studentName = value
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

    Private _dateOfBirth As DateTime
    Public Property DateOfBirth() As DateTime
        Get
            Return _dateOfBirth
        End Get
        Set(ByVal value As DateTime)
            _dateOfBirth = value
        End Set
    End Property
End Class
