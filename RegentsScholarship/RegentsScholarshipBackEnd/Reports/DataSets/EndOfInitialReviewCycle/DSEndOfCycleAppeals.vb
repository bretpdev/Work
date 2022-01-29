Public Class DSEndOfCycleAppeals
    Private _received As String
    Public Property Received() As String
        Get
            Return _received
        End Get
        Set(ByVal value As String)
            _received = value
        End Set
    End Property

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
End Class
