Public Class DSWeeklyAppeals
    Private _approved As Integer
    Public Property Approved() As Integer
        Get
            Return _approved
        End Get
        Set(ByVal value As Integer)
            _approved = value
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

    Private _outstanding As Integer
    Public Property Outstanding() As Integer
        Get
            Return _outstanding
        End Get
        Set(ByVal value As Integer)
            _outstanding = value
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
End Class
