Public Class LoanInterestRate
    Private _lnSeqText As String = ""
    Public Property LnSeqText() As String
        Get
            Return _lnSeqText
        End Get
        Set(ByVal value As String)
            _lnSeqText = value
        End Set
    End Property

    Private _interestRateText As String = ""
    Public Property InterestRateText() As String
        Get
            Return _interestRateText
        End Get
        Set(ByVal value As String)
            _interestRateText = value
        End Set
    End Property
End Class
