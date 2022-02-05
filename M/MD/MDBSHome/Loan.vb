Public Class Loan
    Private _loanType As String = ""
    Public Property LoanType() As String
        Get
            Return _loanType
        End Get
        Set(ByVal value As String)
            _loanType = value
        End Set
    End Property

    Private _status As String = ""
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _repaymentStartDate As String = ""
    Public Property RepaymentStartDate() As String
        Get
            Return _repaymentStartDate
        End Get
        Set(ByVal value As String)
            _repaymentStartDate = value
        End Set
    End Property

    Private _daysDelinquent As Integer = 0
    Public Property DaysDelinquent() As Integer
        Get
            Return _daysDelinquent
        End Get
        Set(ByVal value As Integer)
            _daysDelinquent = value
        End Set
    End Property

    Private _delinquencyDate As String = ""
    Public Property DelinquencyDate() As String
        Get
            Return _delinquencyDate
        End Get
        Set(ByVal value As String)
            _delinquencyDate = value
        End Set
    End Property

    Private _installment As Double = 0
    Public Property Installment() As Double
        Get
            Return _installment
        End Get
        Set(ByVal value As Double)
            _installment = value
        End Set
    End Property

    Private _term As Integer = 0
    Public Property Term() As Integer
        Get
            Return _term
        End Get
        Set(ByVal value As Integer)
            _term = value
        End Set
    End Property

    Private _eftRate As Double = 0
    Public Property EftRate() As Double
        Get
            Return _eftRate
        End Get
        Set(ByVal value As Double)
            _eftRate = value
        End Set
    End Property
End Class
