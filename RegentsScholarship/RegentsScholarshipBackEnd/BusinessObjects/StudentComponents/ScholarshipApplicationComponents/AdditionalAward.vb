Public Class AdditionalAward
    Private _awardType As String
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

    Private _isApproved As Nullable(Of Boolean)
    Public Property IsApproved() As Nullable(Of Boolean)
        Get
            Return _isApproved
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _isApproved = value
        End Set
    End Property

    Private _parentApplication As ScholarshipApplication
    Public Function ParentApplication() As ScholarshipApplication
        Return _parentApplication
    End Function
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _amountOriginal As Double
    Private _isApprovedOriginal As Nullable(Of Boolean)
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal awardType As String, ByVal parentApplication As ScholarshipApplication)
        _objectIsNew = True
        _awardType = awardType
        _isApproved = New Nullable(Of Boolean)(False)
        _parentApplication = parentApplication
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId)
        If (_objectIsNew) Then
            If (_awardType = Constants.AwardType.EXEMPLARY_AWARD) Then
                DataAccess.SetExemplaryAward(Me)
            Else
                DataAccess.SetUespSupplementalAward(Me)
            End If
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            If (_awardType = Constants.AwardType.EXEMPLARY_AWARD) Then
                DataAccess.SetExemplaryAward(Me)
            Else
                DataAccess.SetUespSupplementalAward(Me)
            End If
            RecordTransactions(userId)
        End If
    End Sub

    Public Shared Function Load(ByVal awardType As String, ByVal parentApplication As ScholarshipApplication) As AdditionalAward
        Dim storedAward As AdditionalAward
        If (awardType = Constants.AwardType.EXEMPLARY_AWARD) Then
            storedAward = DataAccess.GetExemplaryAward(parentApplication)
        Else
            storedAward = DataAccess.GetUespSupplementalAward(parentApplication)
        End If
        storedAward._awardType = awardType
        storedAward._objectIsNew = False
        storedAward._parentApplication = parentApplication
        storedAward.SetChangeTrackingVariables()
        Return storedAward
    End Function

    Public Sub Validate()
        'Check that the amount isn't excessive if it's a UESP award.
        If (_awardType = Constants.AwardType.UESP_SUPPLEMENTAL_AWARD AndAlso _amount > Constants.MAX_UESP_AWARD_AMOUNT) Then
            Dim message As String = String.Format("The UESP award amound cannot exceed {0:$0.00}.", Constants.MAX_UESP_AWARD_AMOUNT)
            Throw New RegentsInvalidDataException(message)
        End If
    End Sub

    Private Function HasChanges() As Boolean
        If (_amount <> _amountOriginal) Then Return True
        If (_isApproved.HasValue AndAlso Not _isApprovedOriginal.HasValue) Then Return True
        If (_isApprovedOriginal.HasValue AndAlso Not _isApproved.HasValue) Then Return True
        If (_isApproved.HasValue AndAlso _isApprovedOriginal.HasValue) Then
            If (_isApproved.Value <> _isApprovedOriginal.Value) Then Return True
        End If
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentApplication.ParentStudent.StateStudentId
        If (_amount <> _amountOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} amount", _awardType), _amountOriginal.ToString("$0.00"), _amount.ToString("$0.00"))
        End If
        If (_isApproved.HasValue AndAlso Not _isApprovedOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} is approved", _awardType), "", _isApproved.Value.ToString())
        End If
        If (_isApprovedOriginal.HasValue AndAlso Not _isApproved.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} is approved", _awardType), _isApprovedOriginal.Value.ToString(), "")
        End If
        If (_isApproved.HasValue AndAlso _isApprovedOriginal.HasValue) Then
            If (_isApproved.Value <> _isApprovedOriginal.Value) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} is approved", _awardType), _isApprovedOriginal.Value.ToString(), _isApproved.Value.ToString())
            End If
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _amountOriginal = _amount
        _isApprovedOriginal = If(_isApproved.HasValue, New Nullable(Of Boolean)(_isApproved.Value), Nothing)
    End Sub
End Class
