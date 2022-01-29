<CLSCompliant(True)> _
Public Class MDClassACHInfo


    Private _hasACH As String
    ''' <summary>
    ''' Indicator as to whether the borrower has an ACH (auto pay) record.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HasACH() As String
        Get
            Return _hasACH
        End Get
        Set(ByVal value As String)
            _hasACH = value
        End Set
    End Property

    Private _statusDt As String
    ''' <summary>
    ''' Status date of ACH (auto pay) record.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusDt() As String
        Get
            Return _statusDt
        End Get
        Set(ByVal value As String)
            _statusDt = value
        End Set
    End Property

    Private _denialReason As String
    ''' <summary>
    ''' Denial reason of ACH (auto pay) record.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DenialReason() As String
        Get
            Return _denialReason
        End Get
        Set(ByVal value As String)
            _denialReason = value
        End Set
    End Property

    Private _nsfCounter As String
    ''' <summary>
    ''' Non Sufficent Funds occurance counter.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NSFCounter() As String
        Get
            Return _nsfCounter
        End Get
        Set(ByVal value As String)
            _nsfCounter = value
        End Set
    End Property

    Private _additionalWithdrawAmt As String
    ''' <summary>
    ''' Additional withdrawal amount for ACH (auto pay).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AdditionalWithdrawAmt() As String
        Get
            Return _additionalWithdrawAmt
        End Get
        Set(ByVal value As String)
            _additionalWithdrawAmt = value
        End Set
    End Property

    Private _routingNumber As String
    ''' <summary>
    ''' Routing number for bank that ACH (auto pay) has documented to use.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RoutingNumber() As String
        Get
            Return _routingNumber
        End Get
        Set(ByVal value As String)
            _routingNumber = value
        End Set
    End Property

    Private _accountNumber As String
    ''' <summary>
    ''' The account number of the bank account the ACH (auto pay) record has documented to use.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountNumber() As String
        Get
            Return _accountNumber
        End Get
        Set(ByVal value As String)
            _accountNumber = value
        End Set
    End Property

    Private _lnLvlInfo(,) As String 'seq num    'frst disb dt    'loan program    'ACH Sq Num
    ''' <summary>
    ''' Loan level information (index 0 = seq num, 1 = first disb date, 2 = loan program, 3 = ACH Seq Number) for ACH (auto pay) record.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LnLvlInfo() As String(,)
        Get
            Return _lnLvlInfo
        End Get
        Set(ByVal value(,) As String)
            _lnLvlInfo = value
        End Set
    End Property

    Private _achDataFound As Boolean
    ''' <summary>
    ''' Indicator as to whether a ACH (auto pay) record was found for the borrower.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ACHDataFound() As Boolean
        Get
            Return _achDataFound
        End Get
        Set(ByVal value As Boolean)
            _achDataFound = value
        End Set
    End Property

    Public ReadOnly Property ACHSeqNum() As String
        Get
            Return LnLvlInfo(3, 0)
        End Get
    End Property

End Class
