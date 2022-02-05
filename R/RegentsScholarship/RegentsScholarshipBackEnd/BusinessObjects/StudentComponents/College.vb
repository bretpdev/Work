Public Class College
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _hasOtherScholarships As Boolean
    Public Property HasOtherScholarships() As Boolean
        Get
            Return _hasOtherScholarships
        End Get
        Set(ByVal value As Boolean)
            _hasOtherScholarships = value
        End Set
    End Property

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _numberOfEnrolledCredits As Double
    Public Property NumberOfEnrolledCredits() As Double
        Get
            Return _numberOfEnrolledCredits
        End Get
        Set(ByVal value As Double)
            _numberOfEnrolledCredits = value
        End Set
    End Property

    Private _otherScholarshipsAmount As Double
    Public Property OtherScholarshipsAmount() As Double
        Get
            Return _otherScholarshipsAmount
        End Get
        Set(ByVal value As Double)
            _otherScholarshipsAmount = value
        End Set
    End Property

    Private _parentStudent As Student
    Public Function ParentStudent() As Student
        Return _parentStudent
    End Function

    Private _term As String
    Public Property Term() As String
        Get
            Return _term
        End Get
        Set(ByVal value As String)
            _term = value
        End Set
    End Property

    Private _termBeginDate As Nullable(Of Date)
    Public Property TermBeginDate() As Nullable(Of Date)
        Get
            Return _termBeginDate
        End Get
        Set(ByVal value As Nullable(Of Date))
            If (value.HasValue AndAlso value.Value = Constants.MINIMUM_SQL_SERVER_DATE) Then
                _termBeginDate = Nothing
            Else
                _termBeginDate = value
            End If
        End Set
    End Property
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _hasOtherScholarshipsOriginal As Boolean
    Private _nameOriginal As String
    Private _numberOfEnrolledCreditsOriginal As Double
    Private _otherScholarshipsAmountOriginal As Double
    Private _termOriginal As String
    Private _termBeginDateOriginal As Nullable(Of Date)
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal parentStudent As Student)
        _objectIsNew = True
        _hasOtherScholarships = False
        _name = ""
        _numberOfEnrolledCredits = 0
        _otherScholarshipsAmount = 0
        _parentStudent = parentStudent
        _term = Constants.CollegeTerm.FALL
        _termBeginDate = Nothing
        SetChangeTrackingVariables()
    End Sub

    Public Shared Function Load(ByVal parentStudent As Student) As College
        Dim storedCollege As College = DataAccess.GetCollege(parentStudent)
        storedCollege._objectIsNew = False
        storedCollege._parentStudent = parentStudent
        storedCollege.SetChangeTrackingVariables()
        Return storedCollege
    End Function

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetCollege(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetCollege(Me)
            RecordTransactions(userId)
        End If
    End Sub

    Public Sub Validate()
        'Check that the term is legitimate.
        If (Not String.IsNullOrEmpty(_term)) AndAlso (Not Lookups.CollegeTerms.Contains(_term)) Then
            Dim message As String = String.Format("{0} is not a valid college term.", _term)
            Throw New RegentsInvalidDataException(message)
        End If
    End Sub

    Private Function HasChanges() As Boolean
        If _hasOtherScholarships <> _hasOtherScholarshipsOriginal Then Return True
        If _name <> _nameOriginal Then Return True
        If _numberOfEnrolledCredits <> _numberOfEnrolledCreditsOriginal Then Return True
        If _otherScholarshipsAmount <> _otherScholarshipsAmountOriginal Then Return True
        If _term <> _termOriginal Then Return True
        If (_termBeginDate.HasValue AndAlso Not _termBeginDateOriginal.HasValue) Then Return True
        If (_termBeginDateOriginal.HasValue AndAlso Not _termBeginDate.HasValue) Then Return True
        If (_termBeginDate.HasValue AndAlso _termBeginDateOriginal.HasValue) Then
            If (_termBeginDate.Value.Date <> _termBeginDateOriginal.Value.Date) Then Return True
        End If
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentStudent.StateStudentId
        If _hasOtherScholarships <> _hasOtherScholarshipsOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "Has other scholarship awards", _hasOtherScholarshipsOriginal, _hasOtherScholarships)
        End If
        If _name <> _nameOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "College name", _nameOriginal, _name)
        End If
        If _numberOfEnrolledCredits <> _numberOfEnrolledCreditsOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "Enrolled college credits", _numberOfEnrolledCreditsOriginal, _numberOfEnrolledCredits)
        End If
        If _otherScholarshipsAmount <> _otherScholarshipsAmountOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "Other scholarship award amount", _otherScholarshipsAmountOriginal, _otherScholarshipsAmount)
        End If
        If _term <> _termOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "College term", _termOriginal, _term)
        End If
        If (_termBeginDate.HasValue AndAlso Not _termBeginDateOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "College term begin date", "", _termBeginDate.Value.ToShortDateString())
        ElseIf (_termBeginDateOriginal.HasValue AndAlso Not _termBeginDate.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "College term begin date", _termBeginDateOriginal.Value.ToShortDateString(), "")
        ElseIf (_termBeginDate.HasValue AndAlso _termBeginDateOriginal.HasValue) Then
            If (_termBeginDate.Value.Date <> _termBeginDateOriginal.Value.Date) Then
                DataAccess.AddTransactionRecord(userId, studentId, "College term begin date", _termBeginDateOriginal.Value.ToShortDateString(), _termBeginDate.Value.ToShortDateString())
            End If
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _hasOtherScholarshipsOriginal = _hasOtherScholarships
        _nameOriginal = _name
        _numberOfEnrolledCreditsOriginal = _numberOfEnrolledCredits
        _otherScholarshipsAmountOriginal = _otherScholarshipsAmount
        _termOriginal = _term
        _termBeginDateOriginal = If(_termBeginDate.HasValue, New Nullable(Of Date)(_termBeginDate.Value), Nothing)
    End Sub
End Class
