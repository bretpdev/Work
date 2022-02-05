Public Class DSPrelimApprovalPayments
    Private _college As String
    Public Property College() As String
        Get
            Return _college
        End Get
        Set(ByVal value As String)
            _college = value
        End Set
    End Property

    Private _lastName As String
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
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

    Private _paymentType As String
    Public Property PaymentType() As String
        Get
            Return _paymentType
        End Get
        Set(ByVal value As String)
            _paymentType = value
        End Set
    End Property

    Private _amount As Double
    Public Property Amount() As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            _amount = value
        End Set
    End Property

    Private _term As String
    Public Property Term() As String
        Get
            Return _term
        End Get
        Set(ByVal value As String)
            _term = value
        End Set
    End Property

    Private _termYear As Integer
    Public Property TermYear() As Integer
        Get
            Return _termYear
        End Get
        Set(ByVal value As Integer)
            _termYear = value
        End Set
    End Property
End Class
