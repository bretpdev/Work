Public Class DSEndOfCycleColleges
    Private _college As String
    Public Property College() As String
        Get
            Return _college
        End Get
        Set(ByVal value As String)
            _college = value
        End Set
    End Property

    Private _baseOnly As Integer
    Public Property BaseOnly() As Integer
        Get
            Return _baseOnly
        End Get
        Set(ByVal value As Integer)
            _baseOnly = value
        End Set
    End Property

    Private _exemplary As Integer
    Public Property Exemplary() As Integer
        Get
            Return _exemplary
        End Get
        Set(ByVal value As Integer)
            _exemplary = value
        End Set
    End Property
End Class
