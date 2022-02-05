Public Class Grade
    Private _objectIsNew As Boolean

#Region "Properties"
    Public Function GpaValue() As Double
        Dim matchingGrade As Lookups.Grade = Lookups.Grades.Where(Function(p) p.Letter = _letter).SingleOrDefault()
        Dim lookupValue As Double = If(matchingGrade Is Nothing, 0, matchingGrade.GpaValue.Value)
        Return lookupValue
    End Function

    Private _letter As String
    Public Property Letter() As String
        Get
            Return _letter
        End Get
        Set(ByVal value As String)
            _letter = value
        End Set
    End Property

    Private _parentCourse As Course
    Public Function ParentCourse() As Course
        Return _parentCourse
    End Function

    Private ReadOnly _term As Integer
    Public Function Term() As Integer
        Return _term
    End Function
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _letterOriginal As String
#End Region 'Change tracking variables

    Public Sub New(ByVal term As Integer, ByVal parentCourse As Course)
        _objectIsNew = True
        _letter = ""
        _parentCourse = parentCourse
        _term = term
        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetGrade(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetGrade(Me)
            RecordTransactions(userId)
        End If
    End Sub

    Public Shared Function Load(ByVal parentCourse As Course) As GradeDictionary
        Dim grades As New GradeDictionary()
        Dim storedGrades As List(Of Grade) = DataAccess.GetGrades(parentCourse)
        For Each storedGrade As Grade In storedGrades
            storedGrade._objectIsNew = False
            storedGrade._parentCourse = parentCourse
            storedGrade.SetChangeTrackingVariables()
            grades.Add(storedGrade)
        Next storedGrade
        Return grades
    End Function

    Public Sub Validate()
        'Check that the grade letter exists in the lookup.
        If (Not String.IsNullOrEmpty(_letter)) AndAlso (Not Lookups.Grades.Select(Function(p) p.Letter).Contains(_letter)) Then
            Throw New RegentsInvalidDataException(String.Format("The grade {0} is not a valid grade.", _letter))
        End If
        'Check that the term is properly set. (This is the programmer's responsibility.)
        Debug.Assert(_term > 0 AndAlso _term < 7)
    End Sub

    Private Function HasChanges() As Boolean
        If (_letter <> _letterOriginal) Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentCourse.ParentCategory.ParentHighSchool.ParentStudent.StateStudentId
        Dim gradeDescription As String = String.Format("{0} class number {1} grade {2}", _parentCourse.ParentCategory.Category, _parentCourse.SequenceNo, _term)
        If (_letter <> _letterOriginal) Then
            DataAccess.AddTransactionRecord(userId, studentId, gradeDescription, _letterOriginal, _letter)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _letterOriginal = _letter
    End Sub
End Class
