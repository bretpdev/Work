Public Class User
    Private _accessLevel As String
    Public Property AccessLevel() As String
        Get
            Return _accessLevel
        End Get
        Set(ByVal value As String)
            _accessLevel = value
        End Set
    End Property

    Private _id As String
    Public Property Id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _passwordDate As DateTime
    Public Property PasswordDate() As DateTime
        Get
            Return _passwordDate
        End Get
        Set(ByVal value As DateTime)
            _passwordDate = value
        End Set
    End Property

    Private _passwordHash As String
    Public Property PasswordHash() As String
        Get
            Return _passwordHash
        End Get
        Set(ByVal value As String)
            _passwordHash = value
        End Set
    End Property
End Class
