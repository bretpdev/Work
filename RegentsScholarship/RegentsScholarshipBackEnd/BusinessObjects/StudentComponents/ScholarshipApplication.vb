Imports System.Collections.Generic

Public Class ScholarshipApplication
    Private _objectIsNew As Boolean

#Region "Properties"
    Private _applicationYear As String
    Public Property ApplicationYear() As String
        Get
            Return _applicationYear
        End Get
        Set(ByVal value As String)
            _applicationYear = value
        End Set
    End Property

    Private _attendedAnotherSchool As Nullable(Of Boolean)
    Public Property AttendedAnotherSchool() As Nullable(Of Boolean)
        Get
            Return _attendedAnotherSchool
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _attendedAnotherSchool = value
        End Set
    End Property

    Private _baseAward As PrimaryAward
    Public Property BaseAward() As PrimaryAward
        Get
            Return _baseAward
        End Get
        Set(ByVal value As PrimaryAward)
            _baseAward = value
        End Set
    End Property

    Private _deferment As LeaveDeferral
    Public Property Deferment() As LeaveDeferral
        Get
            Return _deferment
        End Get
        Set(ByVal value As LeaveDeferral)
            _deferment = value
        End Set
    End Property

    Private _denialReasons As HashSet(Of String)
    Public Property DenialReasons() As HashSet(Of String)
        Get
            Return _denialReasons
        End Get
        Set(ByVal value As HashSet(Of String))
            _denialReasons = value
        End Set
    End Property

    Private _documentStatusDates As Dictionary(Of String, Nullable(Of DateTime))
    Public Property DocumentStatusDates() As Dictionary(Of String, Nullable(Of DateTime))
        Get
            Return _documentStatusDates
        End Get
        Set(ByVal value As Dictionary(Of String, Nullable(Of DateTime)))
            _documentStatusDates = value
        End Set
    End Property

    Private _exemplaryAward As AdditionalAward
    Public Property ExemplaryAward() As AdditionalAward
        Get
            Return _exemplaryAward
        End Get
        Set(ByVal value As AdditionalAward)
            _exemplaryAward = value
        End Set
    End Property

    Private _howTheyHeardAboutRegents As String
    Public Property HowTheyHeardAboutRegents() As String
        Get
            Return _howTheyHeardAboutRegents
        End Get
        Set(ByVal value As String)
            _howTheyHeardAboutRegents = value
        End Set
    End Property

    Private _leaveOfAbsence As LeaveDeferral
    Public Property LeaveOfAbsence() As LeaveDeferral
        Get
            Return _leaveOfAbsence
        End Get
        Set(ByVal value As LeaveDeferral)
            _leaveOfAbsence = value
        End Set
    End Property

    Private _ninthGradeSchool As String
    Public Property NinthGradeSchool() As String
        Get
            Return _ninthGradeSchool
        End Get
        Set(ByVal value As String)
            _ninthGradeSchool = value
        End Set
    End Property

    Private _parentStudent As Student
    Public Function ParentStudent() As Student
        Return _parentStudent
    End Function

    Private _plannedCollegeToAttend As String
    Public Property PlannedCollegeToAttend() As String
        Get
            Return _plannedCollegeToAttend
        End Get
        Set(ByVal value As String)
            _plannedCollegeToAttend = value
        End Set
    End Property

    Private _reviews As ReviewDictionary
    Public Property Reviews() As ReviewDictionary
        Get
            Return _reviews
        End Get
        Set(ByVal value As ReviewDictionary)
            _reviews = value
        End Set
    End Property

    Private _uespSupplementalAward As AdditionalAward
    Public Property UespSupplementalAward() As AdditionalAward
        Get
            Return _uespSupplementalAward
        End Get
        Set(ByVal value As AdditionalAward)
            _uespSupplementalAward = value
        End Set
    End Property
#End Region 'Properties

#Region "Change tracking variables"
    'Keep track of original values for all member variables that are not business objects.
    'These will be updated to current values after committing current values to the database.
    'Business objects will take care of themselves when we call Commit() on them.
    Private _attendedAnotherSchoolOriginal As Nullable(Of Boolean)
    Private _defermentOriginal As LeaveDeferral
    Private _denialReasonsOriginal As HashSet(Of String)
    Private _documentStatusDatesOriginal As Dictionary(Of String, Nullable(Of Date))
    Private _howTheyHeardAboutRegentsOriginal As String
    Private _leaveOfAbsenceOriginal As LeaveDeferral
    Private _ninthGradeSchoolOriginal As String
    Private _plannedCollegeToAttendOriginal As String
    Private _reviewsOriginal As ReviewDictionary
#End Region 'Change tracking variables

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal parentStudent As Student)
        _objectIsNew = True
        _attendedAnotherSchool = Nothing
        _baseAward = New PrimaryAward(Me)
        _deferment = Nothing
        _denialReasons = New HashSet(Of String)()
        _documentStatusDates = New Dictionary(Of String, Nullable(Of Date))()
        _exemplaryAward = New AdditionalAward(Constants.AwardType.EXEMPLARY_AWARD, Me)
        _howTheyHeardAboutRegents = ""
        _leaveOfAbsence = Nothing
        _ninthGradeSchool = ""
        _parentStudent = parentStudent
        _plannedCollegeToAttend = ""
        _reviews = New ReviewDictionary()
        _uespSupplementalAward = New AdditionalAward(Constants.AwardType.UESP_SUPPLEMENTAL_AWARD, Me)
        SetChangeTrackingVariables()
        _applicationYear = Constants.CURRENT_AWARD_YEAR.ToString("0000")
    End Sub

    Public Sub Commit(ByVal userId As String)
        If (_objectIsNew) Then
            DataAccess.SetScholarshipApplication(Me)
            SetChangeTrackingVariables()
            _objectIsNew = False
        ElseIf (HasChanges()) Then
            DataAccess.SetScholarshipApplication(Me)
            RecordTransactions(userId)
        End If

        'Call Commit() on member objects.
        DataAccess.DeleteLeaveAndDeferral(Me)
        DataAccess.DeleteReviews(Me)
        _baseAward.Commit(userId)
        If (_deferment IsNot Nothing) Then _deferment.Commit()
        _exemplaryAward.Commit(userId)
        If (_leaveOfAbsence IsNot Nothing) Then _leaveOfAbsence.Commit()
        For Each rvw As Review In _reviews.Values
            rvw.Commit(userId)
        Next rvw
        _uespSupplementalAward.Commit(userId)
    End Sub

    Public Shared Function Load(ByVal parentStudent As Student) As ScholarshipApplication
        Dim storedApplication As ScholarshipApplication = DataAccess.GetScholarshipApplication(parentStudent)
        storedApplication._objectIsNew = False
        storedApplication._parentStudent = parentStudent
        storedApplication.BaseAward = PrimaryAward.Load(storedApplication)
        storedApplication.Deferment = LeaveDeferral.Load(Constants.LeaveDeferralType.DEFERRAL, storedApplication)
        storedApplication.DenialReasons = DataAccess.GetDenialReasons(storedApplication)
        storedApplication.DocumentStatusDates = DataAccess.GetDocumentStatusDates(storedApplication)
        storedApplication.ExemplaryAward = AdditionalAward.Load(Constants.AwardType.EXEMPLARY_AWARD, storedApplication)
        storedApplication.LeaveOfAbsence = LeaveDeferral.Load(Constants.LeaveDeferralType.LEAVE_OF_ABSENCE, storedApplication)
        storedApplication.Reviews = Review.Load(storedApplication)
        storedApplication.UespSupplementalAward = AdditionalAward.Load(Constants.AwardType.UESP_SUPPLEMENTAL_AWARD, storedApplication)
        storedApplication.SetChangeTrackingVariables()
        Return storedApplication
    End Function

    Public Sub Validate()
        'Check that the denial reasons are legitimate.
        For Each denialReason As String In _denialReasons
            If (Not Lookups.DenialReasons.Contains(denialReason)) Then
                Dim message As String = String.Format("The denial reason {0} is not acceptable.", denialReason)
                Throw New RegentsInvalidDataException(message)
            End If
        Next

        'Don't allow future dates for documents.
        For Each documentStatusDate As Nullable(Of Date) In DocumentStatusDates.Values
            If Validator.DateIsCurrentOrInPast(documentStatusDate) = False Then
                Dim message As String = "One of your document status dates is in the future.  Please resolve the issue and try again."
                Throw New RegentsInvalidDataException(message)
            End If
        Next documentStatusDate

        'Call Validate() on member objects.
        BaseAward.Validate()
        ExemplaryAward.Validate()
        UespSupplementalAward.Validate()
        If (_deferment IsNot Nothing) Then _deferment.Validate()
        If (_leaveOfAbsence IsNot Nothing) Then _leaveOfAbsence.Validate()
        For Each rev As Review In _reviews.Values
            rev.Validate()
        Next
    End Sub

    Private Function HasChanges() As Boolean
        If (_attendedAnotherSchool.HasValue AndAlso Not _attendedAnotherSchoolOriginal.HasValue) Then Return True
        If (_attendedAnotherSchoolOriginal.HasValue AndAlso Not _attendedAnotherSchool.HasValue) Then Return True
        If (_attendedAnotherSchool.HasValue AndAlso _attendedAnotherSchoolOriginal.HasValue) Then
            If (_attendedAnotherSchool.Value <> _attendedAnotherSchoolOriginal.Value) Then Return True
        End If
        If (_deferment Is Nothing AndAlso _defermentOriginal IsNot Nothing) Then Return True
        If (_defermentOriginal Is Nothing AndAlso _deferment IsNot Nothing) Then Return True
        If (_deferment IsNot Nothing AndAlso _defermentOriginal IsNot Nothing) Then
            If (_deferment.BeginDate.HasValue AndAlso Not _defermentOriginal.BeginDate.HasValue) Then Return True
            If (_defermentOriginal.BeginDate.HasValue AndAlso Not _deferment.BeginDate.HasValue) Then Return True
            If (_deferment.BeginDate.HasValue AndAlso _defermentOriginal.BeginDate.HasValue) Then
                If (_deferment.BeginDate.Value.Date <> _defermentOriginal.BeginDate.Value.Date) Then Return True
            End If
            If (_deferment.EndDate.HasValue AndAlso Not _defermentOriginal.EndDate.HasValue) Then Return True
            If (_defermentOriginal.EndDate.HasValue AndAlso Not _deferment.EndDate.HasValue) Then Return True
            If (_deferment.EndDate.HasValue AndAlso _defermentOriginal.EndDate.HasValue) Then
                If (_deferment.EndDate.Value.Date <> _defermentOriginal.EndDate.Value.Date) Then Return True
            End If
            If (_deferment.Reason <> _defermentOriginal.Reason) Then Return True
        End If
        If (Not _denialReasons.SetEquals(_denialReasonsOriginal)) Then Return True
        For Each newDocument As KeyValuePair(Of String, Nullable(Of Date)) In _documentStatusDates
            If (_documentStatusDatesOriginal.ContainsKey(newDocument.Key)) Then
                If (newDocument.Value.HasValue AndAlso Not _documentStatusDatesOriginal(newDocument.Key).HasValue) Then Return True
                If (_documentStatusDatesOriginal(newDocument.Key).HasValue AndAlso Not newDocument.Value.HasValue) Then Return True
                If (newDocument.Value.HasValue AndAlso _documentStatusDatesOriginal(newDocument.Key).HasValue) Then
                    If (newDocument.Value.Value.Date <> _documentStatusDatesOriginal(newDocument.Key).Value.Date) Then Return True
                End If
            Else
                Return True
            End If
        Next newDocument
        For Each oldDocument As KeyValuePair(Of String, Nullable(Of Date)) In _documentStatusDatesOriginal
            If (Not _documentStatusDates.ContainsKey(oldDocument.Key)) Then Return True
        Next oldDocument
        If _howTheyHeardAboutRegents <> _howTheyHeardAboutRegentsOriginal Then Return True
        If (_leaveOfAbsence Is Nothing AndAlso _leaveOfAbsenceOriginal IsNot Nothing) Then Return True
        If (_leaveOfAbsenceOriginal Is Nothing AndAlso _leaveOfAbsence IsNot Nothing) Then Return True
        If (_leaveOfAbsence IsNot Nothing AndAlso _leaveOfAbsenceOriginal IsNot Nothing) Then
            If (_leaveOfAbsence.BeginDate.HasValue AndAlso Not _leaveOfAbsenceOriginal.BeginDate.HasValue) Then Return True
            If (_leaveOfAbsenceOriginal.BeginDate.HasValue AndAlso Not _leaveOfAbsence.BeginDate.HasValue) Then Return True
            If (_leaveOfAbsence.BeginDate.HasValue AndAlso _leaveOfAbsenceOriginal.BeginDate.HasValue) Then
                If (_leaveOfAbsence.BeginDate.Value.Date <> _leaveOfAbsenceOriginal.BeginDate.Value.Date) Then Return True
            End If
            If (_leaveOfAbsence.EndDate.HasValue AndAlso Not _leaveOfAbsenceOriginal.EndDate.HasValue) Then Return True
            If (_leaveOfAbsenceOriginal.EndDate.HasValue AndAlso Not _leaveOfAbsence.EndDate.HasValue) Then Return True
            If (_leaveOfAbsence.EndDate.HasValue AndAlso _leaveOfAbsenceOriginal.EndDate.HasValue) Then
                If (_leaveOfAbsence.EndDate.Value.Date <> _leaveOfAbsenceOriginal.EndDate.Value.Date) Then Return True
            End If
            If (_leaveOfAbsence.Reason <> _leaveOfAbsenceOriginal.Reason) Then Return True
        End If
        If _ninthGradeSchool <> _ninthGradeSchoolOriginal Then Return True
        If _plannedCollegeToAttend <> _plannedCollegeToAttendOriginal Then Return True
        For Each reviewType As String In _reviews.Keys
            If (Not _reviewsOriginal.ContainsKey(reviewType)) Then Return True
        Next reviewType
        For Each reviewType As String In _reviewsOriginal.Keys
            If (Not _reviews.ContainsKey(reviewType)) Then Return True
        Next reviewType
        Return False
    End Function

    Private Sub RecordTransactions(ByVal userId As String)
        Dim studentId As String = _parentStudent.StateStudentId
        If (_attendedAnotherSchool.HasValue AndAlso Not _attendedAnotherSchoolOriginal.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Attended another school", "", _attendedAnotherSchool.Value.ToString())
        End If
        If (_attendedAnotherSchoolOriginal.HasValue AndAlso Not _attendedAnotherSchool.HasValue) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Attended another school", _attendedAnotherSchoolOriginal.Value.ToString(), "")
        End If
        If (_attendedAnotherSchool.HasValue AndAlso _attendedAnotherSchoolOriginal.HasValue) Then
            If (_attendedAnotherSchool.Value <> _attendedAnotherSchoolOriginal.Value) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Attended another school", _attendedAnotherSchoolOriginal.Value.ToString(), _attendedAnotherSchool.Value.ToString())
            End If
        End If
        If (_deferment Is Nothing AndAlso _defermentOriginal IsNot Nothing) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Deferment", _defermentOriginal.Reason, "")
        End If
        If (_defermentOriginal Is Nothing AndAlso _deferment IsNot Nothing) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Deferment", "", _deferment.Reason)
        End If
        If (_deferment IsNot Nothing AndAlso _defermentOriginal IsNot Nothing) Then
            If (_deferment.BeginDate.HasValue AndAlso Not _defermentOriginal.BeginDate.HasValue) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Deferment start date", "", _deferment.BeginDate.Value.ToShortDateString())
            ElseIf (_defermentOriginal.BeginDate.HasValue AndAlso Not _deferment.BeginDate.HasValue) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Deferment start date", _defermentOriginal.BeginDate.Value.ToShortDateString(), "")
            ElseIf (_deferment.BeginDate.HasValue AndAlso _defermentOriginal.BeginDate.HasValue) Then
                If (_deferment.BeginDate.Value.Date <> _defermentOriginal.BeginDate.Value.Date) Then
                    DataAccess.AddTransactionRecord(userId, studentId, "Deferment start date", _defermentOriginal.BeginDate.Value.ToShortDateString(), _deferment.BeginDate.Value.ToShortDateString())
                End If
            End If
            If (_deferment.EndDate.HasValue AndAlso Not _defermentOriginal.EndDate.HasValue) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Deferment end date", "", _deferment.EndDate.Value.ToShortDateString())
            ElseIf (_defermentOriginal.EndDate.HasValue AndAlso Not _deferment.EndDate.HasValue) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Deferment end date", _defermentOriginal.EndDate.Value.ToShortDateString(), "")
            ElseIf (_deferment.EndDate.HasValue AndAlso _defermentOriginal.EndDate.HasValue) Then
                If (_deferment.EndDate.Value.Date <> _defermentOriginal.EndDate.Value.Date) Then
                    DataAccess.AddTransactionRecord(userId, studentId, "Deferment end date", _defermentOriginal.EndDate.Value.ToShortDateString(), _deferment.EndDate.Value.ToShortDateString())
                End If
            End If
            If (_deferment.Reason <> _defermentOriginal.Reason) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Deferment reason", _defermentOriginal.Reason, _deferment.Reason)
            End If
        End If
        For Each newReason As String In _denialReasons
            If (Not _denialReasonsOriginal.Contains(newReason)) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Denial reason " + newReason, "", newReason)
            End If
        Next newReason
        For Each oldReason As String In _denialReasonsOriginal
            If (Not _denialReasons.Contains(oldReason)) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Denial reason " + oldReason, oldReason, "")
            End If
        Next oldReason
        For Each newDocument As KeyValuePair(Of String, Nullable(Of Date)) In _documentStatusDates
            If (_documentStatusDatesOriginal.ContainsKey(newDocument.Key)) Then
                If (newDocument.Value.HasValue AndAlso Not _documentStatusDatesOriginal(newDocument.Key).HasValue) Then
                    DataAccess.AddTransactionRecord(userId, studentId, String.Format("Document status date for {0}", newDocument.Key), "", newDocument.Value.Value.ToShortDateString())
                ElseIf (_documentStatusDatesOriginal(newDocument.Key).HasValue AndAlso Not newDocument.Value.HasValue) Then
                    DataAccess.AddTransactionRecord(userId, studentId, String.Format("Document status date for {0}", newDocument.Key), _documentStatusDatesOriginal(newDocument.Key).Value.ToShortDateString(), "")
                ElseIf (newDocument.Value.HasValue AndAlso _documentStatusDatesOriginal(newDocument.Key).HasValue) Then
                    If (newDocument.Value.Value.Date <> _documentStatusDatesOriginal(newDocument.Key).Value.Date) Then
                        DataAccess.AddTransactionRecord(userId, studentId, String.Format("Document status date for {0}", newDocument.Key), _documentStatusDatesOriginal(newDocument.Key).Value.ToShortDateString(), newDocument.Value.Value.ToShortDateString())
                    End If
                End If
            Else
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("Document status date for {0}", newDocument.Key), "", newDocument.Value.Value.ToShortDateString())
            End If
        Next newDocument
        For Each oldDocument As KeyValuePair(Of String, Nullable(Of Date)) In _documentStatusDatesOriginal
            If (Not _documentStatusDates.ContainsKey(oldDocument.Key)) Then
                DataAccess.AddTransactionRecord(userId, studentId, String.Format("Document status date for {0}", oldDocument.Key), oldDocument.Value.Value.ToShortDateString(), "")
            End If
        Next oldDocument
        If _howTheyHeardAboutRegents <> _howTheyHeardAboutRegentsOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "How they heard about the scholarship", _howTheyHeardAboutRegentsOriginal, _howTheyHeardAboutRegents)
        End If
        If (_leaveOfAbsence Is Nothing AndAlso _leaveOfAbsenceOriginal IsNot Nothing) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence", _leaveOfAbsenceOriginal.Reason, "")
        End If
        If (_leaveOfAbsenceOriginal Is Nothing AndAlso _leaveOfAbsence IsNot Nothing) Then
            DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence", "", _leaveOfAbsence.Reason)
        End If
        If (_leaveOfAbsence IsNot Nothing AndAlso _leaveOfAbsenceOriginal IsNot Nothing) Then
            If (_leaveOfAbsence.BeginDate.HasValue AndAlso Not _leaveOfAbsenceOriginal.BeginDate.HasValue) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence start date", "", _leaveOfAbsence.BeginDate.Value.ToShortDateString())
            End If
            If (_leaveOfAbsenceOriginal.BeginDate.HasValue AndAlso Not _leaveOfAbsence.BeginDate.HasValue) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence start date", _leaveOfAbsenceOriginal.BeginDate.Value.ToShortDateString(), "")
            End If
            If (_leaveOfAbsence.BeginDate.HasValue AndAlso _leaveOfAbsenceOriginal.BeginDate.HasValue) Then
                If (_leaveOfAbsence.BeginDate.Value.Date <> _leaveOfAbsenceOriginal.BeginDate.Value.Date) Then
                    DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence start date", _leaveOfAbsenceOriginal.BeginDate.Value.ToShortDateString(), _leaveOfAbsence.BeginDate.Value.ToShortDateString())
                End If
            End If
            If (_leaveOfAbsence.EndDate.HasValue AndAlso Not _leaveOfAbsenceOriginal.EndDate.HasValue) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence end date", "", _leaveOfAbsence.EndDate.Value.ToShortDateString())
            End If
            If (_leaveOfAbsenceOriginal.EndDate.HasValue AndAlso Not _leaveOfAbsence.EndDate.HasValue) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence end date", _leaveOfAbsenceOriginal.EndDate.Value.ToShortDateString(), "")
            End If
            If (_leaveOfAbsence.EndDate.HasValue AndAlso _leaveOfAbsenceOriginal.EndDate.HasValue) Then
                If (_leaveOfAbsence.EndDate.Value.Date <> _leaveOfAbsenceOriginal.EndDate.Value.Date) Then
                    DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence end date", _leaveOfAbsenceOriginal.EndDate.Value.ToShortDateString(), _leaveOfAbsence.EndDate.Value.ToShortDateString())
                End If
            End If
            If (_leaveOfAbsence.Reason <> _leaveOfAbsenceOriginal.Reason) Then
                DataAccess.AddTransactionRecord(userId, studentId, "Leave of absence reason", _leaveOfAbsenceOriginal.Reason, _leaveOfAbsence.Reason)
            End If
        End If
        If _ninthGradeSchool <> _ninthGradeSchoolOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "Ninth grade school", _ninthGradeSchoolOriginal, _ninthGradeSchool)
        End If
        If _plannedCollegeToAttend <> _plannedCollegeToAttendOriginal Then
            DataAccess.AddTransactionRecord(userId, studentId, "Planned college", _plannedCollegeToAttendOriginal, _plannedCollegeToAttend)
        End If
        For Each reviewType As String In _reviews.Keys
            If (Not _reviewsOriginal.ContainsKey(reviewType)) Then
                DataAccess.AddTransactionRecord(userId, studentId, reviewType + "Review completed", "", reviewType)
            End If
        Next reviewType
        For Each reviewType As String In _reviewsOriginal.Keys
            If (Not _reviews.ContainsKey(reviewType)) Then
                DataAccess.AddTransactionRecord(userId, studentId, reviewType + "Review rescinded", reviewType, "")
            End If
        Next reviewType
        SetChangeTrackingVariables()
    End Sub

    Private Sub SetChangeTrackingVariables()
        _attendedAnotherSchoolOriginal = If(_attendedAnotherSchool.HasValue, New Nullable(Of Boolean)(_attendedAnotherSchool.Value), Nothing)
        _defermentOriginal = If(_deferment Is Nothing, Nothing, New LeaveDeferral(Constants.LeaveDeferralType.DEFERRAL, Me) With {.BeginDate = New Nullable(Of Date)(_deferment.BeginDate), .EndDate = New Nullable(Of Date)(_deferment.EndDate), .Reason = _deferment.Reason})
        _denialReasonsOriginal = New HashSet(Of String)(_denialReasons)
        _documentStatusDatesOriginal = New Dictionary(Of String, Nullable(Of Date))(_documentStatusDates)
        _howTheyHeardAboutRegentsOriginal = _howTheyHeardAboutRegents
        _leaveOfAbsenceOriginal = If(_leaveOfAbsence Is Nothing, Nothing, New LeaveDeferral(Constants.LeaveDeferralType.LEAVE_OF_ABSENCE, Me) With {.BeginDate = New Nullable(Of Date)(_leaveOfAbsence.BeginDate), .EndDate = New Nullable(Of Date)(_leaveOfAbsence.EndDate), .Reason = _leaveOfAbsence.Reason})
        _ninthGradeSchoolOriginal = _ninthGradeSchool
        _plannedCollegeToAttendOriginal = _plannedCollegeToAttend
        _reviewsOriginal = New ReviewDictionary(_reviews)
    End Sub
End Class
