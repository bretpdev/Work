Public Class FinancialTransaction
    Private _reversalReason As String
    Public Property ReversalReason() As String
        Get
            Return _reversalReason
        End Get
        Set(ByVal value As String)
            _reversalReason = value
        End Set
    End Property

    Private _effectiveDate As String
    Public Property EffectiveDate() As String
        Get
            Return _effectiveDate
        End Get
        Set(ByVal value As String)
            _effectiveDate = value
        End Set
    End Property

    Private _postedDate As String
    Public Property PostedDate() As String
        Get
            Return _postedDate
        End Get
        Set(ByVal value As String)
            _postedDate = value
        End Set
    End Property

    Private _transactionType As String
    Public Property TransactionType() As String
        Get
            Return _transactionType
        End Get
        Set(ByVal value As String)
            _transactionType = value
        End Set
    End Property

    Private _appliedPrincipal As Double
    Public Property AppliedPrincipal() As Double
        Get
            Return _appliedPrincipal
        End Get
        Set(ByVal value As Double)
            _appliedPrincipal = value
        End Set
    End Property

    Private _appliedInterest As Double
    Public Property AppliedInterest() As Double
        Get
            Return _appliedInterest
        End Get
        Set(ByVal value As Double)
            _appliedInterest = value
        End Set
    End Property

    Private _appliedLateFee As Double
    Public Property AppliedLateFee() As Double
        Get
            Return _appliedLateFee
        End Get
        Set(ByVal value As Double)
            _appliedLateFee = value
        End Set
    End Property

    Private _transactionAmount As Double
    Public Property TransactionAmount() As Double
        Get
            Return _transactionAmount
        End Get
        Set(ByVal value As Double)
            _transactionAmount = value
        End Set
    End Property

    Private _principalBalance As Double
    Public Property PrincipalBalance() As Double
        Get
            Return _principalBalance
        End Get
        Set(ByVal value As Double)
            _principalBalance = value
        End Set
    End Property
End Class
