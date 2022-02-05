Public Class Course
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _concurrentCollege As String
    Public Property ConcurrentCollege() As String
        Get
            Return _concurrentCollege
        End Get
        Set(ByVal value As String)
            _concurrentCollege = value
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

    Private _gradeLevel As String
    Public Property GradeLevel() As String
        Get
            Return _gradeLevel
        End Get
        Set(ByVal value As String)
            _gradeLevel = value
        End Set
    End Property

    Private _grades As GradeDictionary
    Public Property Grades() As GradeDictionary
        Get
            Return _grades
        End Get
        Set(ByVal value As GradeDictionary)
            _grades = value
        End Set
    End Property

    Private _isAcceptable As String
    Public Property IsAcceptable() As String
        Get
            Return _isAcceptable
        End Get
        Set(ByVal value As String)
            _isAcceptable = value
        End Set
    End Property

    Private _parentCategory As CourseCategory
    Public Function ParentCategory() As CourseCategory
        Return _parentCategory
    End Function

    Private ReadOnly _sequenceNo As Integer
    Public Function SequenceNo() As Integer
        Return _sequenceNo
    End Function

    Private _title As String
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
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

    Private _weight As String
    Public Property Weight() As String
        Get
            Return _weight
        End Get
        Set(ByVal value As String)
            _weight = value
        End Set
    End Property

    Private _academicYearTaken As String
    Public Property AcademicYearTaken() As String
        Get
            Return _academicYearTaken
        End Get
        Set(ByVal value As String)
            If (value IsNot Nothing AndAlso value.Length < 5) Then
                _academicYearTaken = ""
            Else
                _academicYearTaken = value
            End If
        End Set
    End Property

    Private _schoolAttended As String
    Public Property SchoolAttended() As String
        Get
            Return _schoolAttended
        End Get
        Set(ByVal value As String)
            _schoolAttended = value
        End Set
    End Property

    ''' <summary>
    ''' Calculated from the Grades collection.
    ''' </summary>
    Public Function WeightedAverageGrade() As AverageGrade
        'Start off by defining the weighted GPA value as the average GPA value.
        Dim weightedGpaValue As Double = 0
        If _grades.Count > 0 Then
            Dim gradeValues As List(Of Double) = _grades.Values.Select(Function(p) p.GpaValue).ToList()
            If gradeValues.Count > 0 Then
                weightedGpaValue = gradeValues.Average()
            End If
            'See if this course's weight is in the qualifying list.
            '(The database lookup only hold qualifying weights and an empty string,
            ' so just check that it's not an empty string.)
            If Not String.IsNullOrEmpty(_weight) Then
                'Apply a weight according to the number of credits.
                weightedGpaValue += Credits * 0.5
            End If
        Else
            Return New AverageGrade() With {.GpaValue = 0, .Letter = ""}
        End If
        'Find the letter grade that corresponds to the weighted GPA value.
        Dim weightedLetterGrade As String = Lookups.Grades.Where(Function(p) weightedGpaValue >= p.GpaValue).OrderByDescending(Function(p) p.GpaValue).Select(Function(p) p.Letter).First()
        'stop the dumb rounding
        weightedGpaValue = weightedGpaValue * 100
        weightedGpaValue = Math.Floor(weightedGpaValue) / 100
        'Define a new AverageGrade object as the return value.
        Return New AverageGrade() With {.GpaValue = weightedGpaValue, .Letter = weightedLetterGrade}
    End Function
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _academicYearTakenOriginal As String
    Private _concurrentCollegeOriginal As String
    Private _creditsOriginal As Double
    Private _gradeLevelOriginal As String
    Private _gradesOriginal As GradeDictionary
    Private _isAcceptableOriginal As String
    Private _schoolAttendedOriginal As String
    Private _titleOriginal As String = _title
    Private _verificationOriginal As Verification
    Private _weightOriginal As String
#End Region 'Change tracking variables

    Public Sub New(ByVal courseSequenceNumber As Integer, ByVal parentCategory As CourseCategory)
        _objectIsNew = True
        _concurrentCollege = ""
        _credits = 0
        _gradeLevel = ""
        _grades = New GradeDictionary()
        _isAcceptable = "Undetermined"
        _parentCategory = parentCategory
        _sequenceNo = 0
        _title = ""
        _verification = New Verification()
        _weight = ""
        _academicYearTaken = ""
        _schoolAttended = ""
        _sequenceNo = courseSequenceNumber
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetCourse(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetCourse(Me)
            'Take care of any deleted grades.
            For Each originalTerm As Integer In _gradesOriginal.Keys
                If (Not _grades.ContainsKey(originalTerm)) Then DataAccess.DeleteGrade(_gradesOriginal(originalTerm))
            Next
            RecordTransactions(userId)
        End If

        'Commit all our grades.
        For Each courseGrade As Grade In _grades.Values
            courseGrade.Commit(userId)
        Next
    End Sub

    Public Shared Function Load(ByVal parentCategory As CourseCategory) As List(Of Course)
        Dim storedCourses As List(Of Course) = DataAccess.GetCourses(parentCategory)
        For Each storedCourse As Course In storedCourses
            storedCourse._objectIsNew = False
            storedCourse._parentCategory = parentCategory
            storedCourse.Grades = Grade.Load(storedCourse)
            storedCourse.SetChangeTrackingVariables()
        Next
        Return storedCourses
    End Function

    Public Sub Validate()
        'Check that _isAcceptable is valid.
        If (Not Lookups.ClassStatuses.Contains(_isAcceptable)) Then
            Dim message As String = String.Format("{0} is not a valid course status.", _isAcceptable)
            Throw New RegentsInvalidDataException(message)
        End If

        'Check that all our grades are ready to commit.
        For Each courseGrade As Grade In _grades.Values
            Try
                courseGrade.Validate()
            Catch ex As RegentsInvalidDataException
                Dim message As String = String.Format("The class titled {0} at grade level {1} has an invalid grade in term {2}", _title, _gradeLevel, courseGrade.Term.ToString())
                Throw New RegentsInvalidDataException(message, ex)
            End Try
        Next

        'Make sure that if the Academic year is populated if app year is not 2009
        If _parentCategory.ParentHighSchool.ParentStudent.ScholarshipApplication.ApplicationYear <> "2009" Then
            'don't update the DB with "/" if nothing was entered
            If AcademicYearTaken IsNot Nothing AndAlso AcademicYearTaken.Trim = "/" Then
                AcademicYearTaken = ""
            End If
            If AcademicYearTaken Is Nothing OrElse AcademicYearTaken = "" Then
                Dim message As String = "All courses must have the Academic Year Taken populated.  Please try again."
                Throw New RegentsInvalidDataException(message)
            End If
        End If

        If String.IsNullOrEmpty(SchoolAttended) Then
            Dim message As String = "All courses must have the School Attended populated.  Please try again."
            Throw New RegentsInvalidDataException(message)
        End If
    End Sub

    Private Function HasChanges() As Boolean
        If (_academicYearTaken <> _academicYearTakenOriginal) Then Return True
        If (_concurrentCollege <> _concurrentCollegeOriginal) Then Return True
        If (_credits <> _creditsOriginal) Then Return True
        If (_gradeLevel <> _gradeLevelOriginal) Then Return True
        For Each newTerm As Integer In _grades.Keys
            If (Not _gradesOriginal.ContainsKey(newTerm)) Then Return True
        Next
        For Each oldTerm As Integer In _gradesOriginal.Keys
            If (Not _grades.ContainsKey(oldTerm)) Then Return True
        Next
        If (_isAcceptable <> _isAcceptableOriginal) Then Return True
        If (_schoolAttended <> _schoolAttendedOriginal) Then Return True
        If (_title <> _titleOriginal) Then Return True
        If (_verification.TimeStamp.HasValue AndAlso Not _verificationOriginal.TimeStamp.HasValue) Then Return True
        If (_verificationOriginal.TimeStamp.HasValue AndAlso Not _verification.TimeStamp.HasValue) Then Return True
        If (_verification.TimeStamp.HasValue AndAlso _verificationOriginal.TimeStamp.HasValue) Then
            If (_verification.TimeStamp.Value.Date <> _verificationOriginal.TimeStamp.Value.Date) Then Return True
        End If
        If (_verification.UserId <> _verificationOriginal.UserId) Then Return True
        If (_weight <> _weightOriginal) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentCategory.ParentHighSchool.ParentStudent.StateStudentId
        Dim courseDescription As String = String.Format("{0} class number {1}", _parentCategory.Category, _sequenceNo)
        If (_academicYearTaken <> _academicYearTakenOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} academic year", courseDescription), _academicYearTakenOriginal, _academicYearTaken)
        End If
        If (_concurrentCollege <> _concurrentCollegeOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} concurrent enrollment college", courseDescription), _concurrentCollegeOriginal, _concurrentCollege)
        End If
        If (_credits <> _creditsOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} credits", courseDescription), _creditsOriginal.ToString(), _credits.ToString())
        End If
        If (_gradeLevel <> _gradeLevelOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} grade level", courseDescription), _gradeLevelOriginal, _gradeLevel)
        End If
        For Each newTerm As Integer In _grades.Keys
            If (Not _gradesOriginal.ContainsKey(newTerm)) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} grade {1}", courseDescription, newTerm), "", _grades(newTerm).Letter)
            End If
        Next
        For Each oldTerm As Integer In _gradesOriginal.Keys
            If (Not _grades.ContainsKey(oldTerm)) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} grade {1}", courseDescription, oldTerm), _gradesOriginal(oldTerm).Letter, "")
            End If
        Next
        If (_isAcceptable <> _isAcceptableOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} is acceptable", courseDescription), _isAcceptableOriginal, _isAcceptable)
        End If
        If (_schoolAttended <> _schoolAttendedOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} school attended", courseDescription), _schoolAttendedOriginal, _schoolAttended)
        End If
        If (_title <> _titleOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} title", courseDescription), _titleOriginal, _title)
        End If
        If (_verification.TimeStamp.HasValue AndAlso Not _verificationOriginal.TimeStamp.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} verification date", courseDescription), "", _verification.TimeStamp.Value.ToShortDateString())
        ElseIf (_verificationOriginal.TimeStamp.HasValue AndAlso Not _verification.TimeStamp.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} verification date", courseDescription), _verificationOriginal.TimeStamp.Value.ToShortDateString(), "")
        ElseIf (_verification.TimeStamp.HasValue AndAlso _verificationOriginal.TimeStamp.HasValue) Then
            If (_verification.TimeStamp.Value.Date <> _verificationOriginal.TimeStamp.Value.Date) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} verification date", courseDescription), _verificationOriginal.TimeStamp.Value.ToShortDateString(), _verification.TimeStamp.Value.ToShortDateString())
            End If
        End If
        If (_verification.UserId <> _verificationOriginal.UserId) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} verified by", courseDescription), _verificationOriginal.UserId, _verification.UserId)
        End If
        If (_weight <> _weightOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, String.Format("{0} weight", courseDescription), _weightOriginal, _weight)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _academicYearTakenOriginal = _academicYearTaken
        _concurrentCollegeOriginal = _concurrentCollege
        _creditsOriginal = _credits
        _gradeLevelOriginal = _gradeLevel
        _gradesOriginal = New GradeDictionary(_grades)
        _isAcceptableOriginal = _isAcceptable
        _schoolAttendedOriginal = _schoolAttended
        _titleOriginal = _title
        If (_verificationOriginal Is Nothing) Then
            _verificationOriginal = New Verification() With {.TimeStamp = If(_verification.TimeStamp.HasValue, New Nullable(Of Date)(_verification.TimeStamp.Value), Nothing), .UserId = _verification.UserId}
        Else
            _verificationOriginal.TimeStamp = If(_verification.TimeStamp.HasValue, New Nullable(Of Date)(_verification.TimeStamp.Value), Nothing)
            _verificationOriginal.UserId = _verification.UserId
        End If
        _weightOriginal = _weight
    End Sub
End Class
