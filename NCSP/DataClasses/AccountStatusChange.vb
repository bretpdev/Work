Public Class AccountStatusChange

    Private _AccountId As String
    Public Property AccountId() As String
        Get
            Return _AccountId
        End Get
        Set(ByVal value As String)
            _AccountId = value
        End Set
    End Property

    Private _EligEndRea As String
    Public Property EligEndRea() As String
        Get
            Return _EligEndRea
        End Get
        Set(ByVal value As String)
            _EligEndRea = value
        End Set
    End Property

    Public Sub New(ByVal accountNumberIn As String, ByVal eligEndReaIn As String)
        _AccountId = accountNumberIn
        _EligEndRea = EligEndReaIn
    End Sub
End Class
