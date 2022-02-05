Public Class DSRenewalAndPaymentStatus
    Private _applicationYear As String
    Public Property ApplicationYear() As String
        Get
            Return _applicationYear
        End Get
        Set(ByVal value As String)
            _applicationYear = value
        End Set
    End Property

    Private _studentId As String
    Public Property StudentId() As String
        Get
            Return _studentId
        End Get
        Set(ByVal value As String)
            _studentId = value
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

    Private _lastName As String
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _awardLevel As String
    Public Property AwardLevel() As String
        Get
            Return _awardLevel
        End Get
        Set(ByVal value As String)
            _awardLevel = value
        End Set
    End Property

    Private _awardStatus As String
    Public Property AwardStatus() As String
        Get
            Return _awardStatus
        End Get
        Set(ByVal value As String)
            _awardStatus = value
        End Set
    End Property

    Private _leaveOrDeferralEnd As String
    Public Property LeaveOrDeferralEnd() As String
        Get
            Return _leaveOrDeferralEnd
        End Get
        Set(ByVal value As String)
            _leaveOrDeferralEnd = value
        End Set
    End Property

    Private _college As String
    Public Property College() As String
        Get
            Return _college
        End Get
        Set(ByVal value As String)
            _college = value
        End Set
    End Property

    Private _remainingHours As Double
    Public Property RemainingHours() As Double
        Get
            Return _remainingHours
        End Get
        Set(ByVal value As Double)
            _remainingHours = value
        End Set
    End Property

    Private _semester As String
    Public Property Semester() As String
        Get
            Return _semester
        End Get
        Set(ByVal value As String)
            _semester = value
        End Set
    End Property

    Private _disbursement As Double
    Public Property Disbursement() As Double
        Get
            Return _disbursement
        End Get
        Set(ByVal value As Double)
            _disbursement = value
        End Set
    End Property

    Private _remainingSemesters As Integer
    Public Property RemainingSemesters() As Integer
        Get
            Return _remainingSemesters
        End Get
        Set(ByVal value As Integer)
            _remainingSemesters = value
        End Set
    End Property

    Private _yearLimitIsMet As Boolean
    Public Property YearLimitIsMet() As Boolean
        Get
            Return _yearLimitIsMet
        End Get
        Set(ByVal value As Boolean)
            _yearLimitIsMet = value
        End Set
    End Property
End Class
