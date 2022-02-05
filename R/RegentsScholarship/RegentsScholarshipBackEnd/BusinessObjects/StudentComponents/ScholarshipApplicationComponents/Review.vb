Public Class Review
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _completionDate As Nullable(Of DateTime)
    Public Property CompletionDate() As Nullable(Of DateTime)
        Get
            Return _completionDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            If (value.HasValue AndAlso value.Value = Constants.MINIMUM_SQL_SERVER_DATE) Then
                _completionDate = Nothing
            Else
                _completionDate = value
            End If
        End Set
    End Property

    Private _parentApplication As ScholarshipApplication
    Public Function ParentApplication() As ScholarshipApplication
        Return _parentApplication
    End Function

    Private ReadOnly _reviewType As String
    Public Function ReviewType() As String
        Return _reviewType
    End Function

    Private _userId As String
    Public Property UserId() As String
        Get
            Return _userId
        End Get
        Set(ByVal value As String)
            _userId = value
        End Set
    End Property
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _completionDateOriginal As Nullable(Of Date)
    Private _userIdOriginal As String
#End Region 'Change tracking variables

    Public Sub New(ByVal reviewType As String, ByVal parentApplication As ScholarshipApplication)
        _objectIsNew = True
        _completionDate = Nothing
        _parentApplication = parentApplication
        _reviewType = reviewType
        _userId = ""
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        'Call SetReview() every time, because ScholarshipApplication wipes out all reviews before committing them.
        DataAccess.SetReview(Me)
        If (_objectIsNew) Then
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            RecordTransactions(userId)
        End If
    End Sub

    Public Shared Function Load(ByVal parentApplication As ScholarshipApplication) As ReviewDictionary
        Dim reviews As New ReviewDictionary()
        Dim storedReviews As List(Of Review) = DataAccess.GetReviews(parentApplication)
        For Each storedReview As Review In storedReviews
            storedReview._objectIsNew = False
            storedReview.SetChangeTrackingVariables()
            reviews.Add(storedReview)
        Next
        Return reviews
    End Function

    Public Sub Validate()
        'Nothing to check for this class.
    End Sub

    Private Function HasChanges() As Boolean
        If (_completionDate.HasValue AndAlso Not _completionDateOriginal.HasValue) Then Return True
        If (_completionDateOriginal.HasValue AndAlso Not _completionDate.HasValue) Then Return True
        If (_completionDate.HasValue AndAlso _completionDateOriginal.HasValue) Then
            If (_completionDate.Value.Date <> _completionDateOriginal.Value.Date) Then Return True
        End If
        If (_userId <> _userIdOriginal) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = ParentApplication.ParentStudent.StateStudentId
        Dim reviewDescription As String = _reviewType + " Review"
        If (_completionDate.HasValue AndAlso Not _completionDateOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} completed", reviewDescription), "", _completionDate.Value.ToShortDateString())
        End If
        If (_completionDateOriginal.HasValue AndAlso Not _completionDate.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} completed", reviewDescription), _completionDateOriginal.Value.ToShortDateString(), "")
        End If
        If (_completionDate.HasValue AndAlso _completionDateOriginal.HasValue) Then
            If (_completionDate.Value.Date <> _completionDateOriginal.Value.Date) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} completed", reviewDescription), _completionDateOriginal.Value.ToShortDateString(), _completionDate.Value.ToShortDateString())
            End If
        End If
        If (_userId <> _userIdOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} user ID", reviewDescription), _userIdOriginal, _userId)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _completionDateOriginal = If(_completionDate.HasValue, New Nullable(Of Date)(_completionDate.Value), Nothing)
        _userIdOriginal = _userId
    End Sub
End Class
