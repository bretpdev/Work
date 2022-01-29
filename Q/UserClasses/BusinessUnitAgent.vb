Public Class BusinessUnitAgent
    Private _agent As SqlUser
    Public Property Agent() As SqlUser
        Get
            Return _agent
        End Get
        Set(ByVal value As SqlUser)
            _agent = value
        End Set
    End Property

    Private _unit As BusinessUnit
    Public Property Unit() As BusinessUnit
        Get
            Return _unit
        End Get
        Set(ByVal value As BusinessUnit)
            _unit = value
        End Set
    End Property

    Private _role As String
    Public Property Role() As String
        Get
            Return _role
        End Get
        Set(ByVal value As String)
            _role = value
        End Set
    End Property
End Class
