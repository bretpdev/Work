Public Class PrimaryAward
    Private _objectIsNew As Boolean

#Region "Properties"
    Public Function Amount() As Double
        Dim award As Lookups.AwardAmount = Lookups.AwardAmounts.Where(Function(p) p.Description = _description AndAlso p.AwardYear = _parentApplication.ApplicationYear).SingleOrDefault()
        Dim awardAmount As Double = If(award Is Nothing, 0, award.Amount)
        Return awardAmount
    End Function

    Private _description As String
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Private _parentApplication As ScholarshipApplication
    Public Function ParentApplication() As ScholarshipApplication
        Return _parentApplication
    End Function

    Private _status As String
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _statusDate As Nullable(Of DateTime)
    Public Property StatusDate() As Nullable(Of DateTime)
        Get
            Return _statusDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            If (value.HasValue AndAlso value.Value = Constants.MINIMUM_SQL_SERVER_DATE) Then
                _statusDate = Nothing
            Else
                _statusDate = value
            End If
        End Set
    End Property

    Private _statusLetterSentDate As Nullable(Of DateTime)
    Public Property StatusLetterSentDate() As Nullable(Of DateTime)
        Get
            Return _statusLetterSentDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            If (value.HasValue AndAlso value.Value = Constants.MINIMUM_SQL_SERVER_DATE) Then
                _statusLetterSentDate = Nothing
            Else
                _statusLetterSentDate = value
            End If
        End Set
    End Property

    Private _statusUserId As String
    Public Property StatusUserId() As String
        Get
            Return _statusUserId
        End Get
        Set(ByVal value As String)
            _statusUserId = value
        End Set
    End Property
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _descriptionOriginal As String
    Private _statusOriginal As String
    Private _statusDateOriginal As Nullable(Of Date)
    Private _statusLetterSentDateOriginal As Nullable(Of Date)
    Private _statusUserIdOriginal As String
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal parentApplication As ScholarshipApplication)
        _objectIsNew = True
        _description = "Base Award - Priority Deadline Met"
        _parentApplication = parentApplication
        _status = Constants.AwardStatus.APPLICATION_DOWNLOADED
        _statusDate = New Nullable(Of Date)(Date.Now)
        _statusLetterSentDate = Nothing
        _statusUserId = ""
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetPrimaryAward(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetPrimaryAward(Me)
            RecordTransactions(userId)
        End If
    End Sub

    Public Shared Function Load(ByVal parentApplication As ScholarshipApplication) As PrimaryAward
        Dim storedAward As PrimaryAward = DataAccess.GetPrimaryAward(parentApplication)
        storedAward._objectIsNew = False
        storedAward._parentApplication = parentApplication
        storedAward.SetChangeTrackingVariables()
        Return storedAward
    End Function

    Public Sub Validate()
        'Don't do anything if there's no award.
        If (String.IsNullOrEmpty(_description)) Then Return
        'check if dates are current or in the past
        If Validator.DateIsCurrentOrInPast(StatusLetterSentDate) = False Then
            Dim message As String = "The Status Letter Sent Date is in the future.  Please resolve the issue and try again."
            Throw New RegentsInvalidDataException(message)
        End If
    End Sub

    Private Function HasChanges() As Boolean
        If (_description <> _descriptionOriginal) Then Return True
        If (_status <> _statusOriginal) Then Return True
        If (_statusDate.HasValue AndAlso Not _statusDateOriginal.HasValue) Then Return True
        If (_statusDateOriginal.HasValue AndAlso Not _statusDate.HasValue) Then Return True
        If (_statusDate.HasValue AndAlso _statusDateOriginal.HasValue) Then
            If (_statusDate.Value.Date <> _statusDateOriginal.Value.Date) Then Return True
        End If
        If (_statusLetterSentDate.HasValue AndAlso Not _statusLetterSentDateOriginal.HasValue) Then Return True
        If (_statusLetterSentDateOriginal.HasValue AndAlso Not _statusLetterSentDate.HasValue) Then Return True
        If (_statusLetterSentDate.HasValue AndAlso _statusLetterSentDateOriginal.HasValue) Then
            If (_statusLetterSentDate.Value.Date <> _statusLetterSentDateOriginal.Value.Date) Then Return True
        End If
        If (_statusUserId <> _statusUserIdOriginal) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentApplication.ParentStudent.StateStudentId
        If (_description <> _descriptionOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Base award type", _descriptionOriginal, _description)
        End If
        If (_status <> _statusOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Award status", _statusOriginal, _status)
        End If
        If (_statusDate.HasValue AndAlso Not _statusDateOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Award status date", "", _statusDate.Value.ToShortDateString())
        End If
        If (_statusDateOriginal.HasValue AndAlso Not _statusDate.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Award status date", _statusDateOriginal.Value.ToShortDateString(), "")
        End If
        If (_statusDate.HasValue AndAlso _statusDateOriginal.HasValue) Then
            If (_statusDate.Value.Date <> _statusDateOriginal.Value.Date) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Award status date", _statusDateOriginal.Value.ToShortDateString(), _statusDate.Value.ToShortDateString())
            End If
        End If
        If (_statusLetterSentDate.HasValue AndAlso Not _statusLetterSentDateOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Award status letter sent date", "", _statusLetterSentDate.Value.ToShortDateString())
        End If
        If (_statusLetterSentDateOriginal.HasValue AndAlso Not _statusLetterSentDate.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Award status letter sent date", _statusLetterSentDateOriginal.Value.ToShortDateString(), "")
        End If
        If (_statusLetterSentDate.HasValue AndAlso _statusLetterSentDateOriginal.HasValue) Then
            If (_statusLetterSentDate.Value.Date <> _statusLetterSentDateOriginal.Value.Date) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Award status letter sent date", _statusLetterSentDateOriginal.Value.ToShortDateString(), _statusLetterSentDate.Value.ToShortDateString())
            End If
        End If
        If (_statusUserId <> _statusUserIdOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Award status user ID", _statusUserIdOriginal, _statusUserId)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _descriptionOriginal = _description
        _statusOriginal = _status
        _statusDateOriginal = If(_statusDate.HasValue, New Nullable(Of Date)(_statusDate.Value), Nothing)
        _statusLetterSentDateOriginal = If(_statusLetterSentDate.HasValue, New Nullable(Of Date)(_statusLetterSentDate.Value), Nothing)
        _statusUserIdOriginal = _statusUserId
    End Sub
End Class
