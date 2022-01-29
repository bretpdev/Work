Public Class AccountBalanceChange

    Private _AccountId As String
    Public Property AccountId() As String
        Get
            Return _AccountId
        End Get
        Set(ByVal value As String)
            _AccountId = value
        End Set
    End Property


    Private _NewBalance As String
    Public Property NewBalance() As String
        Get
            Return _NewBalance
        End Get
        Set(ByVal value As String)
            _NewBalance = value
        End Set
    End Property

    Public Sub New(ByVal accountNumberIn As String, ByVal newBalanceIn As String)
        _AccountId = accountNumberIn
        _NewBalance = newBalanceIn
    End Sub


End Class
