Public Class Payment
    Public Enum AwardType
        Base
        Exemplary
        Uesp
    End Enum

    Private _objectIsNew As Boolean

#Region "Properties"
    Private _amount As Double
    Public Property Amount() As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            _amount = value
        End Set
    End Property

    Private _batchNumber As String
    Public Property BatchNumber() As String
        Get
            Return _batchNumber
        End Get
        Set(ByVal value As String)
            _batchNumber = value
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

    Private _credits As Double
    Public Property Credits() As Double
        Get
            Return _credits
        End Get
        Set(ByVal value As Double)
            _credits = value
        End Set
    End Property

    Private _creditsRequirementIsOverridden As Boolean
    Public Property CreditsRequirementIsOverridden() As Boolean
        Get
            Return _creditsRequirementIsOverridden
        End Get
        Set(ByVal value As Boolean)
            _creditsRequirementIsOverridden = value
        End Set
    End Property

    Private _denialReasons As String
    Public Property DenialReasons() As String
        Get
            Return _denialReasons
        End Get
        Set(ByVal value As String)
            _denialReasons = value
        End Set
    End Property

    Private _gpa As Nullable(Of Double)
    Public Property Gpa() As Nullable(Of Double)
        Get
            Return _gpa
        End Get
        Set(ByVal value As Nullable(Of Double))
            _gpa = value
        End Set
    End Property

    Private _gpaRequirementIsOverridden As Boolean
    Public Property GpaRequirementIsOverridden() As Boolean
        Get
            Return _gpaRequirementIsOverridden
        End Get
        Set(ByVal value As Boolean)
            _gpaRequirementIsOverridden = value
        End Set
    End Property

    Private _gradesReceivedDate As Nullable(Of DateTime)
    Public Property GradesReceivedDate() As Nullable(Of DateTime)
        Get
            Return _gradesReceivedDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _gradesReceivedDate = value
        End Set
    End Property

    Private _parentStudent As Student
    Public Function ParentStudent() As Student
        Return _parentStudent
    End Function

    Private _scheduleReceivedDate As DateTime
    Public Property ScheduleReceivedDate() As DateTime
        Get
            Return _scheduleReceivedDate
        End Get
        Set(ByVal value As DateTime)
            _scheduleReceivedDate = value
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

    Private _semesterUniquenessIsOverridden As Boolean
    Public Property SemesterUniquenessIsOverridden() As Boolean
        Get
            Return _semesterUniquenessIsOverridden
        End Get
        Set(ByVal value As Boolean)
            _semesterUniquenessIsOverridden = value
        End Set
    End Property

    Private _sequenceNo As Integer
    Public Property SequenceNo() As Integer
        Get
            Return _sequenceNo
        End Get
        Set(ByVal value As Integer)
            _sequenceNo = value
        End Set
    End Property

    Private _status As String
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
            StatusDate = New Nullable(Of DateTime)(DateTime.Now)
        End Set
    End Property

    Private _statusDate As DateTime
    Public Property StatusDate() As DateTime
        Get
            Return _statusDate
        End Get
        Set(ByVal value As DateTime)
            _statusDate = value
        End Set
    End Property

    Private _type As String
    Public Property Type() As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property

    Private _year As Integer
    Public Property Year() As Integer
        Get
            Return _year
        End Get
        Set(ByVal value As Integer)
            _year = value
        End Set
    End Property
#End Region 'Properties

#Region "Change tracking variables"
    Private _amountOriginal As Double
    Private _collegeOriginal As String
    Private _creditsOriginal As Double
    Private _denialReasonsOriginal As String
    Private _gpaOriginal As Nullable(Of Double)
    Private _gradesReceivedDateOriginal As Nullable(Of DateTime)
    Private _scheduleReceivedDateOriginal As DateTime
    Private _semesterOriginal As String
    Private _statusOriginal As String
    Private _typeOriginal As String
    Private _yearOriginal As Integer
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal parentStudent As Student, ByVal paymentType As AwardType)
        Const MIN_TUITION_YEAR As Integer = 2005
        _objectIsNew = True
        _parentStudent = parentStudent

        'Get some values that will help to appropriately initialize this payment.
        Dim latestApprovedPayment As Payment = GetLatestApprovedPayment(parentStudent.Payments)
        Dim nextSemesterYear As SemesterYear
        If (latestApprovedPayment Is Nothing) Then
            Dim term As String = parentStudent.College.Term
            If (String.IsNullOrEmpty(term)) Then term = Constants.CollegeTerm.FALL
            Dim year As Integer = DateTime.Now.Year
            If (parentStudent.College.TermBeginDate.HasValue) Then year = parentStudent.College.TermBeginDate.Value.Year
            nextSemesterYear = New SemesterYear(term, year)
        Else
            nextSemesterYear = GetNextSemesterYear(latestApprovedPayment.Semester, latestApprovedPayment.Year, latestApprovedPayment.College)
        End If

        'Initialize members based on the most recent payment, if any.
        _college = If(latestApprovedPayment Is Nothing, parentStudent.College.Name, latestApprovedPayment.College)
        _credits = If(latestApprovedPayment Is Nothing, parentStudent.College.NumberOfEnrolledCredits, latestApprovedPayment.Credits)
        If (_credits = 0) Then _credits = 12
        If (parentStudent.CumulativeCreditHoursPaid() + _credits > Constants.MAX_CREDIT_HOURS_PAYABLE) Then
            _credits = Constants.MAX_CREDIT_HOURS_PAYABLE - parentStudent.CumulativeCreditHoursPaid()
        End If
        _gpa = Nothing
        _gradesReceivedDate = Nothing
        _scheduleReceivedDate = DateTime.Now
        _semester = nextSemesterYear.Semester
        _sequenceNo = parentStudent.Payments.Select(Function(p) p.SequenceNo).OrderBy(Function(p) p).LastOrDefault() + 1
        _status = Constants.PaymentStatus.PENDING
        _statusDate = DateTime.Now
        Select Case paymentType
            Case AwardType.Base
                _type = Constants.PaymentType.BASE
            Case AwardType.Uesp
                _type = Constants.PaymentType.UESP
            Case Else
                _type = Constants.PaymentType.EXEMPLARY
        End Select
        _year = nextSemesterYear.Year
        'Check whether the chosen semester puts us over the limit for how many semesters we can make payments.
        'If so, use the latest payment's semester instead.
        If (GoesBeyondMaxSemestersPayable()) Then
            _semester = latestApprovedPayment.Semester
            _year = latestApprovedPayment.Year
        End If

        'Make sure we have tuition information for the chosen semester.
        Do While _year > MIN_TUITION_YEAR
            Try
                _amount = DataAccess.GetCalculatedPaymentAmount(parentStudent.StateStudentId, _college, _year, _semester, _type, _credits)
                Exit Do
            Catch ex As Exception
                Dim previousSemesterYear As SemesterYear = GetPreviousSemesterYear(_semester, _year, _college)
                _semester = previousSemesterYear.Semester
                _year = previousSemesterYear.Year
            End Try
        Loop

        If _year = MIN_TUITION_YEAR Then
            Throw New Exception("The payment can't be created because there is no tuition data for the semester.")
        End If

        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        'Call SetPayment() every time, because Student wipes out all payments before committing them.
        DataAccess.SetPayment(Me)
        If (_objectIsNew) Then
            SetChangeTrackingVariables()
            RecordTransactions(userId)
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            RecordTransactions(userId)
        End If
    End Sub

    Public Shared Function GetPreviousSemesterYear(ByVal currentSemester As String, ByVal currentYear As Integer, ByVal collegeName As String) As SemesterYear
        'Base the return value on the current semester.
        If (currentSemester = Constants.CollegeTerm.FALL) Then
            'The previous payment was for Summer of the current year.
            Return New SemesterYear(Constants.CollegeTerm.SUMMER, currentYear)
        ElseIf (currentSemester = Constants.CollegeTerm.SUMMER) Then
            'The previous payment was for Spring of the current year.
            Return New SemesterYear(Constants.CollegeTerm.SPRING, currentYear)
        ElseIf (currentSemester = Constants.CollegeTerm.SPRING) Then
            If (collegeName = Constants.CollegeName.BRIGHAM_YOUNG_UNIVERSITY) Then
                'The previous payment was for Winter of the current year.
                Return New SemesterYear(Constants.CollegeTerm.WINTER, currentYear)
            Else
                'The previous payment was for Fall of last year.
                Return New SemesterYear(Constants.CollegeTerm.FALL, currentYear - 1)
            End If
        Else
            Return New SemesterYear(Constants.CollegeTerm.FALL, currentYear - 1)
        End If
    End Function

    Public Shared Function Load(ByVal parentStudent As Student) As List(Of Payment)
        Dim storedPayments As List(Of Payment) = DataAccess.GetPayments(parentStudent)
        For Each storedPayment As Payment In storedPayments
            storedPayment._parentStudent = parentStudent
            storedPayment._objectIsNew = False
            storedPayment.SetChangeTrackingVariables()
        Next storedPayment
        Return storedPayments
    End Function

    Public Sub Validate()
        'Check if the amount is 0, which means some invalid data prevented the amount from being determined.
        If (_amount = 0) Then
            Dim message As String = String.Format("A payment to {0} for {1} {2} can't be saved because invalid data prevented the payment amount from being calculated.", _college, _semester, _year)
            message += " Check that the semester, year, and credit hours are correct."
            Throw New RegentsInvalidDataException(message)
        End If

        'Check that the selected year is not earlier than the scholarship award year.
        If (_year < Integer.Parse(_parentStudent.ScholarshipApplication.ApplicationYear)) Then
            Dim message As String = "Payments cannot be made for years earlier than the application year."
            Throw New RegentsInvalidDataException(message)
        End If

        'Check that the GPA is filled in, unless this is the first semester.
        If (_type = Constants.PaymentType.EXEMPLARY) Then
            Dim basePayment As Payment = _parentStudent.Payments.Where(Function(p) p.Type = Constants.PaymentType.BASE).SingleOrDefault()
            If (basePayment IsNot Nothing AndAlso (_year <> basePayment.Year OrElse _semester <> basePayment.Semester) AndAlso Not _gpa.HasValue) Then
                Dim message As String = "The previous semester's GPA must be entered."
                Throw New RegentsInvalidDataException(message)
            End If
        End If

        'Check that there aren't multiple base or UESP payments.
        If (_parentStudent.Payments.Where(Function(p) p.Type = Constants.PaymentType.BASE AndAlso p.Amount >= 0).Count() > 1) Then
            Dim message As String = "Only one base award payment is allowed."
            Throw New RegentsInvalidDataException(message)
        End If
        If (_parentStudent.Payments.Where(Function(p) p.Type = Constants.PaymentType.UESP AndAlso p.Amount >= 0).Count() > 1) Then
            Dim message As String = "Only one UESP award payment is allowed."
            Throw New RegentsInvalidDataException(message)
        End If

        'Don't allow primary key violations in the Transaction table
        '(multiple payments of the same type for the same semester at the same college in the same batch).
        Dim semesterPayments As List(Of Payment) = _parentStudent.Payments.Where(Function(p) p.Type = _type AndAlso p.College = _college AndAlso p.Semester = _semester AndAlso p.Year = _year AndAlso p.Status <> Constants.PaymentStatus.APPROVED AndAlso p.Status <> Constants.PaymentStatus.DENIED).ToList()
        If (semesterPayments.Count > 1) Then
            Dim message As String = String.Format("Adding a payment to {0} for {1} {2} would put more than one payment for that semester in the next batch, which the database won't allow.", _college, _semester, _year)
            Throw New RegentsInvalidDataException(message)
        End If

        'Check that we're not making more than two payments per academic year.
        Dim thisYearTerms As List(Of String) = New String() {Constants.CollegeTerm.SUMMER, Constants.CollegeTerm.FALL}.ToList()
        Dim nextYearTerms As List(Of String) = New String() {Constants.CollegeTerm.SPRING}.ToList()
        If (_college = Constants.CollegeName.BRIGHAM_YOUNG_UNIVERSITY) Then
            thisYearTerms.Remove(Constants.CollegeTerm.SUMMER)
            nextYearTerms.Add(Constants.CollegeTerm.WINTER)
            nextYearTerms.Add(Constants.CollegeTerm.SUMMER)
        ElseIf (_college = Constants.CollegeName.LDS_BUSINESS_COLLEGE) Then
            thisYearTerms.Remove(Constants.CollegeTerm.SUMMER)
            nextYearTerms.Add(Constants.CollegeTerm.SUMMER)
        End If
        Dim academicYearPayments As IEnumerable(Of Payment) = _parentStudent.Payments.Where(Function(p) p.Type = Constants.PaymentType.EXEMPLARY AndAlso p.SemesterUniquenessIsOverridden = False)
        If (thisYearTerms.Contains(_semester)) Then
            academicYearPayments = academicYearPayments.Where(Function(p) (p.Year = _year AndAlso thisYearTerms.Contains(p.Semester)) OrElse (p.Year = _year + 1 AndAlso nextYearTerms.Contains(p.Semester)))
        Else
            academicYearPayments = academicYearPayments.Where(Function(p) (p.Year = _year AndAlso nextYearTerms.Contains(p.Semester)) OrElse (p.Year = _year - 1 AndAlso thisYearTerms.Contains(p.Semester)))
        End If
        If (academicYearPayments.Count() > 2) Then
            Dim message As String = "Only two payments per academic year are allowed."
            Throw New RegentsInvalidDataException(message)
        End If

        'Check that we haven't gone beyond the maximum allowed number of semesters.
        If (GoesBeyondMaxSemestersPayable()) Then
            Dim message As String = String.Format("A payment to {0} for {1} {2} can't be saved because it goes past the limit of {3} semesters.", _college, _semester, _year, Constants.MAX_SEMESTERS_PAYABLE)
            Throw New RegentsInvalidDataException(message)
        End If
    End Sub

    Private Function GetLatestApprovedPayment(ByVal existingPayments As List(Of Payment)) As Payment
        If (existingPayments Is Nothing) Then Return Nothing
        Dim approvedPayments As IEnumerable(Of Payment) = existingPayments.Where(Function(p) p.Status = Constants.PaymentStatus.APPROVED)
        If (approvedPayments.Count = 0) Then Return Nothing

        'Find the latest year for which there are payments.
        Dim orderedPayments As IEnumerable(Of Payment) = approvedPayments.OrderBy(Function(p) p.Year)
        Dim latestYear As Integer = orderedPayments.Last().Year

        'Base the return value on the latest semester completed.
        Dim latestYearSemesters As IEnumerable(Of String) = approvedPayments.Where(Function(p) p.Year = latestYear).Select(Function(p) p.Semester)
        If (latestYearSemesters.Contains(Constants.CollegeTerm.FALL)) Then
            Return approvedPayments.Where(Function(p) p.Year = latestYear AndAlso p.Semester = Constants.CollegeTerm.FALL).Last()
        ElseIf (latestYearSemesters.Contains(Constants.CollegeTerm.SUMMER)) Then
            Return approvedPayments.Where(Function(p) p.Year = latestYear AndAlso p.Semester = Constants.CollegeTerm.SUMMER).Last()
        ElseIf (latestYearSemesters.Contains(Constants.CollegeTerm.SPRING)) Then
            Return approvedPayments.Where(Function(p) p.Year = latestYear AndAlso p.Semester = Constants.CollegeTerm.SPRING).Last()
        Else
            Return approvedPayments.Where(Function(p) p.Year = latestYear AndAlso p.Semester = Constants.CollegeTerm.WINTER).Last()
        End If
    End Function

    Private Function GetNextSemesterYear(ByVal currentSemester As String, ByVal currentYear As Integer, ByVal collegeName As String) As SemesterYear
        'Base the return value on the latest payment.
        If (currentSemester = Constants.CollegeTerm.FALL) Then
            If (collegeName = Constants.CollegeName.BRIGHAM_YOUNG_UNIVERSITY) Then
                'The next payment will be for Winter of the following year.
                Return New SemesterYear(Constants.CollegeTerm.WINTER, currentYear + 1)
            Else
                'The next payment will be for Spring of the following year.
                Return New SemesterYear(Constants.CollegeTerm.SPRING, currentYear + 1)
            End If
        ElseIf (currentSemester = Constants.CollegeTerm.SUMMER) Then
            'The next payment will be for Fall of the latest year.
            Return New SemesterYear(Constants.CollegeTerm.FALL, currentYear)
        ElseIf (currentSemester = Constants.CollegeTerm.SPRING) Then
            'The next payment will be for Summer of the latest year.
            Return New SemesterYear(Constants.CollegeTerm.SUMMER, currentYear)
        Else
            Return New SemesterYear(Constants.CollegeTerm.SPRING, currentYear)
        End If
    End Function

    Private Function GoesBeyondMaxSemestersPayable() As Boolean
        'Get a full list of existing semesters for which there are payments.
        Dim existingSemesters As List(Of SemesterYear) = _parentStudent.Payments.Select(Function(p) New SemesterYear(p.Semester, p.Year)).ToList()
        'Add this payment's semester to the list.
        '(This is crucial when the function is called from the Payment constructor, because the payment isn't part of the list yet.)
        existingSemesters.Add(New SemesterYear(_semester, _year))
        'Check whether the count of distinct semesters is within the allowed limit.
        Dim distinctSemesters As New List(Of SemesterYear)()
        For Each semYr As SemesterYear In existingSemesters
            Dim sy As SemesterYear = semYr
            If (distinctSemesters.Where(Function(p) p.Semester = sy.Semester AndAlso p.Year = sy.Year).Count() = 0) Then
                distinctSemesters.Add(sy)
            End If
        Next semYr
        Return (distinctSemesters.Count() > Constants.MAX_SEMESTERS_PAYABLE)
    End Function

    Private Function HasChanges() As Boolean
        If (_amount <> _amountOriginal) Then Return True
        If (_college <> _collegeOriginal) Then Return True
        If (_credits <> _creditsOriginal) Then Return True
        If (_denialReasons <> _denialReasonsOriginal) Then Return True
        If (_gpa.HasValue AndAlso Not _gpaOriginal.HasValue) Then Return True
        If (_gpaOriginal.HasValue AndAlso Not _gpa.HasValue) Then Return True
        If (_gpa.HasValue AndAlso _gpaOriginal.HasValue) Then
            If (_gpa.Value <> _gpaOriginal.Value) Then Return True
        End If
        If (_gradesReceivedDate.HasValue AndAlso Not _gradesReceivedDateOriginal.HasValue) Then Return True
        If (_gradesReceivedDateOriginal.HasValue AndAlso Not _gradesReceivedDate.HasValue) Then Return True
        If (_gradesReceivedDate.HasValue AndAlso _gradesReceivedDateOriginal.HasValue) Then
            If (_gradesReceivedDate.Value.Date <> _gradesReceivedDateOriginal.Value.Date) Then Return True
        End If
        If (_scheduleReceivedDate.Date <> _scheduleReceivedDateOriginal.Date) Then Return True
        If (_semester <> _semesterOriginal) Then Return True
        If (_status <> _statusOriginal) Then Return True
        If (_type <> _typeOriginal) Then Return True
        If (_year <> _yearOriginal) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = ParentStudent.StateStudentId
        Dim paymentDescription As String = String.Format("{0} award payment to {1} for {2} {3}:", _typeOriginal, _collegeOriginal, _semesterOriginal, _yearOriginal)
        If (_amount <> _amountOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} amount", paymentDescription), _amountOriginal.ToString("0.00"), _amount.ToString("0.00"))
        End If
        If (_college <> _collegeOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} college", paymentDescription), _collegeOriginal, _college)
        End If
        If (_credits <> _creditsOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} credits", paymentDescription), _creditsOriginal.ToString("0.0"), _credits.ToString("0.0"))
        End If
        If (_denialReasons <> _denialReasonsOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} denial reasons", paymentDescription), If(_denialReasonsOriginal, ""), If(_denialReasons, ""))
        End If
        If (_gpa.HasValue AndAlso Not _gpaOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} GPA", paymentDescription), "", _gpa.Value.ToString("0.00"))
        End If
        If (_gpaOriginal.HasValue AndAlso Not _gpa.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} GPA", paymentDescription), _gpaOriginal.Value.ToString("0.00"), "")
        End If
        If (_gpa.HasValue AndAlso _gpaOriginal.HasValue) Then
            If (_gpa.Value <> _gpaOriginal.Value) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} GPA", paymentDescription), _gpaOriginal.Value.ToString("0.00"), _gpa.Value.ToString("0.00"))
            End If
        End If
        If (_gradesReceivedDate.HasValue AndAlso Not _gradesReceivedDateOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} grades received date", paymentDescription), "", _gradesReceivedDate.Value.ToShortDateString())
        End If
        If (_gradesReceivedDateOriginal.HasValue AndAlso Not _gradesReceivedDate.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} grades received date", paymentDescription), _gradesReceivedDateOriginal.Value.ToShortDateString(), "")
        End If
        If (_gradesReceivedDate.HasValue AndAlso _gradesReceivedDateOriginal.HasValue) Then
            If (_gradesReceivedDate.Value.Date <> _gradesReceivedDateOriginal.Value.Date) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} grades received date", paymentDescription), _gradesReceivedDateOriginal.Value.ToShortDateString(), _gradesReceivedDate.Value.ToShortDateString())
            End If
        End If
        If (_scheduleReceivedDate.Date <> _scheduleReceivedDateOriginal.Date) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} schedule received date", paymentDescription), _scheduleReceivedDateOriginal.ToShortDateString(), _scheduleReceivedDate.ToShortDateString())
        End If
        If (_semester <> _semesterOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} semester", paymentDescription), _semesterOriginal, _semester)
        End If
        If (_status <> _statusOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} status", paymentDescription), _statusOriginal, _status)
        End If
        If (_type <> _typeOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} type", paymentDescription), _typeOriginal, _type)
        End If
        If (_year <> _yearOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} year", paymentDescription), _yearOriginal, _year)
        End If
    End Sub

    Private Sub SetChangeTrackingVariables()
        _amountOriginal = _amount
        _collegeOriginal = _college
        _creditsOriginal = _credits
        _denialReasonsOriginal = _denialReasons
        _gpaOriginal = If(_gpa.HasValue, New Nullable(Of Double)(_gpa.Value), Nothing)
        _gradesReceivedDateOriginal = If(_gradesReceivedDate.HasValue, New Nullable(Of Date)(_gradesReceivedDate.Value), Nothing)
        _scheduleReceivedDateOriginal = _scheduleReceivedDate
        _semesterOriginal = _semester
        _statusOriginal = _status
        _typeOriginal = _type
        _yearOriginal = _year
    End Sub

    Public Class SemesterYear
        Private _semester As String
        Public Property Semester() As String
            Get
                Return _semester
            End Get
            Set(ByVal value As String)
                _semester = value
            End Set
        End Property

        Private _year As Integer
        Public Property Year() As Integer
            Get
                Return _year
            End Get
            Set(ByVal value As Integer)
                _year = value
            End Set
        End Property

        Public Sub New(ByVal semester As String, ByVal year As Integer)
            _semester = semester
            _year = year
        End Sub
    End Class
End Class
