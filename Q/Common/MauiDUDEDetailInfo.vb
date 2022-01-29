Public Class MauiDUDEDetailInfo


    Private _demos As MDBorrowerDemographics
    Public Property Demos() As MDBorrowerDemographics
        Get
            Return _demos
        End Get
        Set(ByVal value As MDBorrowerDemographics)
            _demos = value
        End Set
    End Property

    Private _numDaysDelinquent As String
    Public Property NumDaysDelinquent() As String
        Get
            Return _numDaysDelinquent
        End Get
        Set(ByVal value As String)
            _numDaysDelinquent = value
        End Set
    End Property

    Private _dateDelinquencyOcc As String
    Public Property DateDelinquencyOcc() As String
        Get
            Return _dateDelinquencyOcc
        End Get
        Set(ByVal value As String)
            _dateDelinquencyOcc = value
        End Set
    End Property

    Private _num20DayLetter As String
    Public Property Num20DayLetter() As String
        Get
            Return _num20DayLetter
        End Get
        Set(ByVal value As String)
            _num20DayLetter = value
        End Set
    End Property

    Private _amtPastDue As String
    Public Property AmtPastDue() As String
        Get
            Return _amtPastDue
        End Get
        Set(ByVal value As String)
            _amtPastDue = value
        End Set
    End Property

    Private _curAmtDue As String
    Public Property CurAmtDue() As String
        Get
            Return _curAmtDue
        End Get
        Set(ByVal value As String)
            _curAmtDue = value
        End Set
    End Property

    Private _totAmtDue As String
    Public Property TotAmtDue() As String
        Get
            Return _totAmtDue
        End Get
        Set(ByVal value As String)
            _totAmtDue = value
        End Set
    End Property

    Private _lateFees As String
    Public Property LateFees() As String
        Get
            Return _lateFees
        End Get
        Set(ByVal value As String)
            _lateFees = value
        End Set
    End Property

    Private _total As String
    Public Property Total() As String
        Get
            Return _total
        End Get
        Set(ByVal value As String)
            _total = value
        End Set
    End Property

    Private _principal As String
    Public Property Principal() As String
        Get
            Return _principal
        End Get
        Set(ByVal value As String)
            _principal = value
        End Set
    End Property

    Private _interest As String
    Public Property Interest() As String
        Get
            Return _interest
        End Get
        Set(ByVal value As String)
            _interest = value
        End Set
    End Property

    Private _dueDay As String
    Public Property DueDay() As String
        Get
            Return _dueDay
        End Get
        Set(ByVal value As String)
            _dueDay = value
        End Set
    End Property

    Private _nextDateDue As String
    Public Property NextDateDue() As String
        Get
            Return _nextDateDue
        End Get
        Set(ByVal value As String)
            _nextDateDue = value
        End Set
    End Property

    Private _lastPmtReceived As String
    Public Property LastPmtReceived() As String
        Get
            Return _lastPmtReceived
        End Get
        Set(ByVal value As String)
            _lastPmtReceived = value
        End Set
    End Property

    Private _monthlyPmtAmt As String
    Public Property MonthlyPmtAmt() As String
        Get
            Return _monthlyPmtAmt
        End Get
        Set(ByVal value As String)
            _monthlyPmtAmt = value
        End Set
    End Property

    Private _directDebit As String
    Public Property DirectDebit() As String
        Get
            Return _directDebit
        End Get
        Set(ByVal value As String)
            _directDebit = value
        End Set
    End Property

    Private _activeRePmtSch As String
    Public Property ActiveRePmtSch() As String
        Get
            Return _activeRePmtSch
        End Get
        Set(ByVal value As String)
            _activeRePmtSch = value
        End Set
    End Property

End Class
