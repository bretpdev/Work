Imports System.Collections.Generic

Public Class HighSchool
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _actScores As Dictionary(Of String, Double)
    Public Property ActScores() As Dictionary(Of String, Double)
        Get
            Return _actScores
        End Get
        Set(ByVal value As Dictionary(Of String, Double))
            _actScores = value
        End Set
    End Property

    Private _ceebCode As String
    Public Property CeebCode() As String
        Get
            Return _ceebCode
        End Get
        Set(ByVal value As String)
            _ceebCode = value
            'Set the other properties from the database.
            Dim hs As Lookups.School = ( _
                From hsl In Lookups.Schools _
                Where hsl.CeebCode = value _
            ).SingleOrDefault()
            If hs IsNot Nothing Then
                _city = hs.City
                _district = hs.District
                _name = hs.Name
            End If
        End Set
    End Property

    Private _city As String
    Public Property City() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property

    Private _courseCategories As CourseCategoryDictionary
    Public Property CourseCategories() As CourseCategoryDictionary
        Get
            Return _courseCategories
        End Get
        Set(ByVal value As CourseCategoryDictionary)
            _courseCategories = value
        End Set
    End Property

    Private _diplomaIsInternationalBaccalaureate As Boolean
    Public Property DiplomaIsInternationalBaccalaureate() As Boolean
        Get
            Return _diplomaIsInternationalBaccalaureate
        End Get
        Set(ByVal value As Boolean)
            _diplomaIsInternationalBaccalaureate = value
        End Set
    End Property

    Private _district As String
    Public Property District() As String
        Get
            Return _district
        End Get
        Set(ByVal value As String)
            _district = value
        End Set
    End Property

    Private _cumulativeGpa As Double
    Public Property CumulativeGpa() As Double
        Get
            Return _cumulativeGpa
        End Get
        Set(ByVal value As Double)
            _cumulativeGpa = value
        End Set
    End Property

    Private _graduationDate As Nullable(Of Date)
    Public Property GraduationDate() As Nullable(Of Date)
        Get
            Return _graduationDate
        End Get
        Set(ByVal value As Nullable(Of Date))
            If (value.HasValue AndAlso value.Value = Constants.MINIMUM_SQL_SERVER_DATE) Then
                _graduationDate = Nothing
            Else
                _graduationDate = value
            End If
        End Set
    End Property

    Private _isInUtah As Boolean
    Public Property IsInUtah() As Boolean
        Get
            Return _isInUtah
        End Get
        Set(ByVal value As Boolean)
            _isInUtah = value
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

    Private _parentStudent As Student
    Public Function ParentStudent() As Student
        Return _parentStudent
    End Function

    Private _usbctStatus As String
    Public Property UsbctStatus() As String
        Get
            Return _usbctStatus
        End Get
        Set(ByVal value As String)
            _usbctStatus = value
        End Set
    End Property
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _actScoresOriginal As Dictionary(Of String, Double)
    Private _ceebCodeOriginal As String
    Private _diplomaIsInternationalBaccalaureateOriginal As Boolean
    Private _cumulativeGpaOriginal As Double
    Private _graduationDateOriginal As Nullable(Of Date)
    Private _isInUtahOriginal As Boolean
    Private _usbctStatusOriginal As String
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal parentStudent As Student)
        _objectIsNew = True
        _actScores = New Dictionary(Of String, Double)()
        _ceebCode = ""
        _city = ""
        _courseCategories = New CourseCategoryDictionary()
        _diplomaIsInternationalBaccalaureate = False
        _district = ""
        _cumulativeGpa = 0
        _graduationDate = Nothing
        _isInUtah = False
        _name = ""
        _parentStudent = parentStudent
        _usbctStatus = Constants.UsbctStatus.FAIL

        'The list of course categories is fixed, so define that.
        _courseCategories.Add(New CourseCategory(Constants.CourseCategory.ENGLISH, Me))
        _courseCategories.Add(New CourseCategory(Constants.CourseCategory.MATHEMATICS, Me))
        _courseCategories.Add(New CourseCategory(Constants.CourseCategory.SCIENCE, Me))
        _courseCategories.Add(New CourseCategory(Constants.CourseCategory.SOCIAL_SCIENCE, Me))
        _courseCategories.Add(New CourseCategory(Constants.CourseCategory.FOREIGN_LANGUAGE, Me))

        SetChangeTrackingVariables()
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetHighSchool(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetHighSchool(Me)
            RecordTransactions(userId)
        End If

        'Call Commit() on each of our course categories.
        For Each cc As CourseCategory In _courseCategories.Values
            cc.Commit(userId)
        Next
    End Sub

    Public Shared Function Load(ByVal parentStudent As Student) As HighSchool
        Dim storedSchool As HighSchool = DataAccess.GetHighSchool(parentStudent)
        storedSchool._objectIsNew = False
        storedSchool._parentStudent = parentStudent
        storedSchool.ActScores = DataAccess.GetActScores(storedSchool)
        storedSchool.CourseCategories.Clear()
        storedSchool.CourseCategories.Add(CourseCategory.Load(Constants.CourseCategory.ENGLISH, storedSchool))
        storedSchool.CourseCategories.Add(CourseCategory.Load(Constants.CourseCategory.FOREIGN_LANGUAGE, storedSchool))
        storedSchool.CourseCategories.Add(CourseCategory.Load(Constants.CourseCategory.MATHEMATICS, storedSchool))
        storedSchool.CourseCategories.Add(CourseCategory.Load(Constants.CourseCategory.SCIENCE, storedSchool))
        storedSchool.CourseCategories.Add(CourseCategory.Load(Constants.CourseCategory.SOCIAL_SCIENCE, storedSchool))
        storedSchool.SetChangeTrackingVariables()
        Return storedSchool
    End Function

    Public Sub Validate()
        'Get the school lookup based on the CEEB code.
        Dim hs As Lookups.School = Lookups.Schools.Where(Function(p) p.CeebCode = _ceebCode).SingleOrDefault()

        'Check that a matching school was found.
        If (hs Is Nothing) Then
            Dim message As String = String.Format("{0} is not a known high school CEEB code.", _ceebCode)
            Throw New RegentsInvalidDataException(message)
        End If

        'Make sure the user didn't enter an invalid name or district for the high school.
        If (_name <> hs.Name) Then
            Dim message As String = String.Format("{0} is not the correct name for this high school.", _name)
            Throw New RegentsInvalidDataException(message)
        End If
        If (_district <> hs.District) Then
            Dim message As String = String.Format("{0} is not the correct district for this high school.", _district)
            Throw New RegentsInvalidDataException(message)
        End If

        'Call Validate() on member objects.
        For Each cc As CourseCategory In _courseCategories.Values
            'The exception message thrown by CourseCategory.Validate()
            'is plenty complete, so let it propogate if an exception is thrown.
            cc.Validate()
        Next
    End Sub

    Private Function HasChanges() As Boolean
        For Each actScore As KeyValuePair(Of String, Double) In _actScores
            If (Not _actScoresOriginal.ContainsKey(actScore.Key)) OrElse (_actScoresOriginal(actScore.Key) <> actScore.Value) Then Return True
        Next
        For Each actScore As KeyValuePair(Of String, Double) In _actScoresOriginal
            If (Not _actScores.ContainsKey(actScore.Key)) OrElse (_actScores(actScore.Key) <> actScore.Value) Then Return True
        Next
        If _ceebCode <> _ceebCodeOriginal Then Return True
        If _diplomaIsInternationalBaccalaureate <> _diplomaIsInternationalBaccalaureateOriginal Then Return True
        If _cumulativeGpa <> _cumulativeGpaOriginal Then Return True
        If (_graduationDate.HasValue AndAlso Not _graduationDateOriginal.HasValue) Then Return True
        If (_graduationDateOriginal.HasValue AndAlso Not _graduationDate.HasValue) Then Return True
        If (_graduationDate.HasValue AndAlso _graduationDateOriginal.HasValue) Then
            If (_graduationDate.Value.Date <> _graduationDateOriginal.Value.Date) Then Return True
        End If
        If _isInUtah <> _isInUtahOriginal Then Return True
        If _usbctStatus <> _usbctStatusOriginal Then Return True
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentStudent.StateStudentId
        For Each actScore As KeyValuePair(Of String, Double) In _actScores
            If (Not _actScoresOriginal.ContainsKey(actScore.Key)) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("ACT {0} score", actScore.Key), "", actScore.Value.ToString())
            ElseIf (_actScoresOriginal(actScore.Key) <> actScore.Value) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("ACT {0} score", actScore.Key), _actScoresOriginal(actScore.Key).ToString(), actScore.Value.ToString())
            End If
        Next
        For Each actScore As KeyValuePair(Of String, Double) In _actScoresOriginal
            If (Not _actScores.ContainsKey(actScore.Key)) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("ACT {0} score", actScore.Key), actScore.Value.ToString(), "")
            End If
        Next
        If _ceebCode <> _ceebCodeOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "High school CEEB code", _ceebCodeOriginal, _ceebCode)
        End If
        If _diplomaIsInternationalBaccalaureate <> _diplomaIsInternationalBaccalaureateOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "IB diploma", _diplomaIsInternationalBaccalaureateOriginal.ToString(), _diplomaIsInternationalBaccalaureate.ToString())
        End If
        If _cumulativeGpa <> _cumulativeGpaOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "Cumulative GPA", _cumulativeGpaOriginal.ToString(), _cumulativeGpa.ToString())
        End If
        If (_graduationDate.HasValue AndAlso Not _graduationDateOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "High school graduate date", "", _graduationDate.Value.ToShortDateString())
        ElseIf (_graduationDateOriginal.HasValue AndAlso Not _graduationDate.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "High school graduate date", _graduationDateOriginal.Value.ToShortDateString(), "")
        ElseIf (_graduationDate.HasValue AndAlso _graduationDateOriginal.HasValue) Then
            If (_graduationDate.Value.Date <> _graduationDateOriginal.Value.Date) Then
                DataAccess.AddTransactionRecord(userId, studentId, "High school graduate date", _graduationDateOriginal.Value.ToShortDateString(), _graduationDate.Value.ToShortDateString())
            End If
        End If
        If _isInUtah <> _isInUtahOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "Will graduate from a Utah high school", _isInUtahOriginal.ToString(), _isInUtah.ToString())
        End If
        If _usbctStatus <> _usbctStatusOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "USBCT status", _usbctStatusOriginal, _usbctStatus)
        End If
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _actScoresOriginal = New Dictionary(Of String, Double)(_actScores)
        _ceebCodeOriginal = _ceebCode
        _diplomaIsInternationalBaccalaureateOriginal = _diplomaIsInternationalBaccalaureate
        _cumulativeGpaOriginal = _cumulativeGpa
        _graduationDateOriginal = If(_graduationDate.HasValue, New Nullable(Of Date)(_graduationDate.Value), Nothing)
        _isInUtahOriginal = _isInUtah
        _usbctStatusOriginal = _usbctStatus
    End Sub
End Class
