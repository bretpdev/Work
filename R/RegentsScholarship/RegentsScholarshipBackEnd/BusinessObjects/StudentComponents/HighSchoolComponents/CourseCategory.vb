Public Class CourseCategory
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _category As String
    Public Function Category() As String
        Return _category
    End Function

    Private _courses As List(Of Course)
    Public Property Courses() As List(Of Course)
        Get
            Return _courses
        End Get
        Set(ByVal value As List(Of Course))
            _courses = value
        End Set
    End Property

    Private _parentHighSchool As HighSchool
    Public Function ParentHighSchool() As HighSchool
        Return _parentHighSchool
    End Function

    Private _requirementIsMet As String
    Public Property RequirementIsMet() As String
        Get
            Return _requirementIsMet
        End Get
        Set(ByVal value As String)
            _requirementIsMet = value
        End Set
    End Property

    Private _verification As Verification
    Public Property Verification() As Verification
        Get
            Return _verification
        End Get
        Set(ByVal value As Verification)
            _verification = value
        End Set
    End Property
#End Region

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _coursesOriginal As List(Of Course)
    Private _requirementIsMetOriginal As String
    Private _verificationOriginal As Verification
#End Region 'Change tracking variables

    Public Sub New(ByVal category As String, ByVal parentHighSchool As HighSchool)
        _category = category
        _objectIsNew = True
        _parentHighSchool = parentHighSchool
        _courses = New List(Of Course)()
        _requirementIsMet = "Undetermined"
        _verification = New Verification()
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetCourseCategory(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetCourseCategory(Me)
            'Take care of any deleted courses.
            For Each deletedCourse As Course In _coursesOriginal.Where(Function(p) Not _courses.Contains(p))
                DataAccess.DeleteCourse(deletedCourse)
            Next deletedCourse
            RecordTransactions(userId)
        End If

        'Call Commit() for each course in turn.
        For Each crs In _courses
            crs.Commit(userId)
        Next
    End Sub

    Public Shared Function Load(ByVal category As String, ByVal parentHighSchool As HighSchool) As CourseCategory
        Dim storedCategory As CourseCategory = DataAccess.GetCourseCategory(category, parentHighSchool)
        storedCategory._objectIsNew = False
        storedCategory._parentHighSchool = parentHighSchool
        storedCategory.Courses = Course.Load(storedCategory)
        storedCategory.SetChangeTrackingVariables()
        Return storedCategory
    End Function

    Public Sub Validate()
        'Call Validate() on each of our Course objects.
        For Each crs As Course In _courses
            Try
                crs.Validate()
            Catch ex As RegentsInvalidDataException
                Dim message As String = String.Format("There is a problem with the {0} course at grade level {1} titled {2}.", _category, crs.GradeLevel, crs.Title)
                Throw New RegentsInvalidDataException(message, ex)
            End Try
        Next
    End Sub

    Private Function HasChanges() As Boolean
        For Each newCourse As Course In _courses
            If (Not _coursesOriginal.Contains(newCourse)) Then Return True
        Next newCourse
        For Each oldCourse As Course In _coursesOriginal
            If (Not _courses.Contains(oldCourse)) Then Return True
        Next oldCourse
        If (_requirementIsMet <> _requirementIsMetOriginal) Then Return True
        If (_verification.TimeStamp.HasValue AndAlso Not _verificationOriginal.TimeStamp.HasValue) Then Return True
        If (_verificationOriginal.TimeStamp.HasValue AndAlso Not _verification.TimeStamp.HasValue) Then Return True
        If (_verification.TimeStamp.HasValue AndAlso _verificationOriginal.TimeStamp.HasValue) Then
            If (_verification.TimeStamp.Value.Date <> _verificationOriginal.TimeStamp.Value.Date) Then Return True
        End If
        If (_verification.UserId <> _verificationOriginal.UserId) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentHighSchool.ParentStudent.StateStudentId
        For Each newCourse As Course In _courses
            If (Not _coursesOriginal.Contains(newCourse)) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} class number {1} added", _category, newCourse.SequenceNo.ToString()), "", newCourse.Title)
            End If
        Next newCourse
        For Each oldCourse As Course In _coursesOriginal
            If (Not _courses.Contains(oldCourse)) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} class number {1} removed", _category, oldCourse.SequenceNo.ToString()), oldCourse.Title, "")
            End If
        Next oldCourse
        If (_requirementIsMet <> _requirementIsMetOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} requirement met", _category), _requirementIsMetOriginal, _requirementIsMet)
        End If
        If (_verification.TimeStamp.HasValue AndAlso Not _verificationOriginal.TimeStamp.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} requirement verification date", _category), "", _verification.TimeStamp.Value.ToShortDateString())
        ElseIf (_verificationOriginal.TimeStamp.HasValue AndAlso Not _verification.TimeStamp.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} requirement verification date", _category), _verificationOriginal.TimeStamp.Value.ToShortDateString(), "")
        ElseIf (_verification.TimeStamp.HasValue AndAlso _verificationOriginal.TimeStamp.HasValue) Then
            If (_verification.TimeStamp.Value.Date <> _verificationOriginal.TimeStamp.Value.Date) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} requirement verification date", _category), _verificationOriginal.TimeStamp.Value.ToShortDateString(), _verification.TimeStamp.Value.ToShortDateString())
            End If
        End If
        If (_verification.UserId <> _verificationOriginal.UserId) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} requirement verified by", _category), _verificationOriginal.UserId, _verification.UserId)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _coursesOriginal = New List(Of Course)(_courses)
        _requirementIsMetOriginal = _requirementIsMet
        If (_verificationOriginal Is Nothing) Then
            _verificationOriginal = New Verification() With {.TimeStamp = If(_verification.TimeStamp.HasValue, New Nullable(Of Date)(_verification.TimeStamp.Value), Nothing), .UserId = _verification.UserId}
        Else
            _verificationOriginal.TimeStamp = If(_verification.TimeStamp.HasValue, New Nullable(Of Date)(_verification.TimeStamp.Value), Nothing)
            _verificationOriginal.UserId = _verification.UserId
        End If
    End Sub
End Class
