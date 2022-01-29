Public Class LG02LoanInfo
    Private _cluid As String
    Public Property Cluid() As String
        Get
            Return _cluid
        End Get
        Set(ByVal value As String)
            _cluid = value
        End Set
    End Property

    Private _loanHolderCode As String
    Public Property LoanHolderCode() As String
        Get
            Return _loanHolderCode
        End Get
        Set(ByVal value As String)
            _loanHolderCode = value
        End Set
    End Property

    Private _loanServicerCode As String
    Public Property LoanServicerCode() As String
        Get
            Return _loanServicerCode
        End Get
        Set(ByVal value As String)
            _loanServicerCode = value
        End Set
    End Property

    Private _loanType As String
    Public Property LoanType() As String
        Get
            Return _loanType
        End Get
        Set(ByVal value As String)
            _loanType = value
        End Set
    End Property

    Private _studentName As String
    Public Property StudentName() As String
        Get
            Return _studentName
        End Get
        Set(ByVal value As String)
            _studentName = value
        End Set
    End Property

    Private _studentSsn As String
    Public Property StudentSsn() As String
        Get
            Return _studentSsn
        End Get
        Set(ByVal value As String)
            _studentSsn = value
        End Set
    End Property
End Class
