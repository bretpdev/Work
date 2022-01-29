Public Class DSApplicationsReceived
    Private _highSchool As String
    Public Property HighSchool() As String
        Get
            Return _highSchool
        End Get
        Set(ByVal value As String)
            _highSchool = value
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
