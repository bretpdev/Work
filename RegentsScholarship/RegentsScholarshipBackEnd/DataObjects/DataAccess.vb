Imports System.Collections.Generic
Imports System.Data.Common
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions
Imports Q

Public Class DataAccess
    Private Shared _regentsDataContext As DataContext

    ''' <summary>
    ''' Exposes the connection string used by the current RegentsDataContext.
    ''' </summary>
    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return RegentsDataContext.Connection.ConnectionString
        End Get
    End Property

    ''' <summary>
    ''' Creates DataContext as needed.  Ensures that only one is created to save processor and memory use.
    ''' </summary>
    Protected Shared ReadOnly Property RegentsDataContext() As DataContext
        Get
            If _regentsDataContext Is Nothing Then
                Dim connectionString As String = My.Settings.RegentsLiveConnectionString
                If Constants.TEST_MODE Then connectionString = My.Settings.RegentsTestConnectionString
                _regentsDataContext = New DataContext(connectionString)
                _regentsDataContext.CommandTimeout = 300
            End If
            Return _regentsDataContext
        End Get
    End Property

    Public Shared Sub AddTransactionRecord(ByVal userId As String, ByVal stateStudentId As String, ByVal changedProperty As String, ByVal oldValue As String, ByVal newValue As String)
        Dim sproc As New SprocCommandBuilder("spAddTransactionRecord")
        sproc.AddParameter("UserId", userId)
        sproc.AddParameter("StateStudentId", stateStudentId)
        sproc.AddParameter("Property", changedProperty)
        sproc.AddParameter("OldValue", oldValue)
        sproc.AddParameter("NewValue", newValue)
        Try
            RegentsDataContext.ExecuteCommand(sproc.Command, sproc.ParameterValues)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to add transaction record for {0}, showing that {1} changed from {2} to {3}.", stateStudentId, changedProperty, oldValue, newValue), ex)
        End Try
    End Sub

    Public Shared Sub BeginTransaction()
        If (RegentsDataContext.Connection.State <> ConnectionState.Open) Then RegentsDataContext.Connection.Open()
        RegentsDataContext.Transaction = RegentsDataContext.Connection.BeginTransaction()
    End Sub

    Public Shared Sub CommitTransaction()
        RegentsDataContext.Transaction.Commit()
        RegentsDataContext.Transaction.Dispose()
        RegentsDataContext.Connection.Close()
    End Sub

    Public Shared Sub RollbackTransaction()
        RegentsDataContext.Transaction.Rollback()
        RegentsDataContext.Transaction.Dispose()
        RegentsDataContext.Connection.Close()
    End Sub

#Region "Application batch support"
    ''' <summary>
    ''' Completes the in-progress first quick review and adds a transaction record.
    ''' </summary>
    ''' <param name="u"></param>
    ''' <param name="stateStudentID"></param>
    ''' <remarks></remarks>
    Public Shared Sub BatchFirstQuickReviewUpdate(ByVal u As User, ByVal stateStudentID As String)
        Dim query As String = String.Format("UPDATE dbo.Review SET CompletionDate = GetDate(), UserId = '{0}' WHERE StateStudentId = '{1}' AND TypeCode = 7", u.Id, stateStudentID)
        RegentsDataContext().ExecuteCommand(query)
        AddTransactionRecord(u.Id, stateStudentID, "Completed First Quick Review", "", "")
    End Sub

    ''' <summary>
    ''' Completes the in-progress second quick review and adds a transaction record.
    ''' </summary>
    ''' <param name="u"></param>
    ''' <param name="stateStudentID"></param>
    ''' <remarks></remarks>
    Public Shared Sub BatchSecondQuickReviewUpdate(ByVal u As User, ByVal stateStudentID As String)
        Dim query As String = String.Format("UPDATE dbo.Review SET CompletionDate = GetDate(), UserId = '{0}' WHERE StateStudentId = '{1}' AND TypeCode = 11", u.Id, stateStudentID)
        RegentsDataContext().ExecuteCommand(query)
        AddTransactionRecord(u.Id, stateStudentID, "Completed Second Quick Review", "", "")
    End Sub

    Public Shared Function GetApprovalLetterStudentIds(ByVal forSimplexLetter As Boolean) As List(Of String)
        If (forSimplexLetter) Then
            Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForSimplexApprovalLetters {0}", Constants.CURRENT_AWARD_YEAR.ToString()).ToList()
        Else
            Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForDuplexApprovalLetters {0}", Constants.CURRENT_AWARD_YEAR.ToString()).ToList()
        End If
    End Function

    Public Shared Function GetConditionallyApprovedStudentIds() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForConditionalApprovalLetter {0}", Constants.CURRENT_AWARD_YEAR.ToString()).ToList()
    End Function

    Public Shared Function GetDenialLetterStudentIds(ByVal denialCategory As ApplicationLetters.DenialCategory) As List(Of String)
        Select Case denialCategory
            Case ApplicationLetters.DenialCategory.CitizenshipOrCriminalRecord
                Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForNonCitizenOrCriminalRecordDenialLetter {0}", Constants.CURRENT_AWARD_YEAR.ToString()).ToList()
            Case ApplicationLetters.DenialCategory.IncompleteApplication
                Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForIncompleteApplicationDenialLetter {0}", Constants.CURRENT_AWARD_YEAR.ToString()).ToList()
            Case ApplicationLetters.DenialCategory.Other
                Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForOtherDenialLetter {0}", Constants.CURRENT_AWARD_YEAR.ToString()).ToList()
            Case ApplicationLetters.DenialCategory.Final
                Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForFinalDenialLetter {0}", Constants.CURRENT_AWARD_YEAR.ToString()).ToList()
            Case Else
                Debug.Assert(False, "Unrecognized denial category.")
                Return Nothing
        End Select
    End Function

    Public Shared Function GetStudentIdsForExemplaryAwardReview(ByVal applicationYear As String) As List(Of String)
        Dim arguments() As String = {applicationYear}
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForExemplaryAwardReview {0}", arguments).ToList()
    End Function

    Public Shared Function GetStudentIdsForFinalCategoryReview(ByVal applicationYear As String) As List(Of String)
        Dim arguments() As String = {applicationYear}
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForFinalCategoryReview {0}", arguments).ToList()
    End Function

    Public Shared Function GetStudentIdsForFinalClassReview(ByVal applicationYear As String) As List(Of String)
        Dim arguments() As String = {applicationYear}
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForFinalClassReview {0}", arguments).ToList()
    End Function

    Public Shared Function GetStudentIdsForInitialAwardDecision(ByVal applicationYear As String) As List(Of String)
        Dim arguments() As String = {applicationYear}
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForInitialAwardDecision {0}", arguments).ToList()
    End Function

    Public Shared Function GetStudentIdsForInitialCategoryReview(ByVal applicationYear As String) As List(Of String)
        Dim arguments() As String = {applicationYear}
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForInitialCategoryReview {0}", arguments).ToList()
    End Function

    Public Shared Function GetStudentIdsForInitialClassReview(ByVal applicationYear As String) As List(Of String)
        Dim arguments() As String = {applicationYear}
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForInitialClassReview {0}", arguments).ToList()
    End Function

    Public Shared Function GetStudentIdsForPendingAwardReview(ByVal applicationYear As String) As List(Of String)
        Dim arguments() As String = {applicationYear}
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForPendingAwardReview {0}", arguments).ToList()
    End Function

    Public Shared Function GetStudentsWithReviewInProgress() As List(Of BatchQuickReviewItem)
        Dim queryBuilder As New StringBuilder("SELECT")
        queryBuilder.Append(" STU.LastName,")
        queryBuilder.Append(" STU.FirstName,")
        queryBuilder.Append(" STU.StateStudentId,")
        queryBuilder.Append(" CASE WHEN QUICK1.CompletionDate IS NULL THEN 1 ELSE 2 END as ReviewInProgress")
        queryBuilder.Append(" FROM dbo.Student STU")
        queryBuilder.Append(" LEFT OUTER JOIN dbo.Review QUICK1")
        queryBuilder.Append(" ON STU.StateStudentId = QUICK1.StateStudentId")
        queryBuilder.Append(" AND QUICK1.TypeCode = 7")
        queryBuilder.Append(" LEFT OUTER JOIN dbo.Review QUICK2")
        queryBuilder.Append(" ON STU.StateStudentId = QUICK2.StateStudentId")
        queryBuilder.Append(" AND QUICK2.TypeCode = 11")
        queryBuilder.Append(" WHERE (QUICK1.CompletionDate IS NULL AND QUICK1.StateStudentId IS NOT NULL)")
        queryBuilder.Append(" OR (QUICK2.CompletionDate IS NULL AND QUICK2.StateStudentId IS NOT NULL)")
        Return RegentsDataContext().ExecuteQuery(Of BatchQuickReviewItem)(queryBuilder.ToString()).ToList()
    End Function
#End Region 'Application batch support

#Region "Payment batch support"
    Public Shared Function GetStudentIdsForFinalPaymentReview() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForFinalPaymentReview").ToList()
    End Function

    Public Shared Function GetStudentIdsForPaymentLetters(ByVal batchNumber As String) As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT DISTINCT StateStudentId FROM Payment WHERE BatchNumber = {0} AND Status = 'Approved'", batchNumber).ToList()
    End Function

    Public Shared Function GetStudentIdsForPreliminaryPaymentReview() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetStudentIdsForPreliminaryPaymentReview").ToList()
    End Function

    Public Shared Function StudentHasNewCenturyAward(ByVal student As Student) As Boolean
        'Clean the SSN so that it only has numbers (no spaces, dashes, etc.).
        Dim ssnBuilder As New StringBuilder()
        For Each ssnCharacter As Char In student.SocialSecurityNumber
            If (Regex.IsMatch(ssnCharacter.ToString(), "\d")) Then ssnBuilder.Append(ssnCharacter)
        Next ssnCharacter
        'Run the query using the cleaned-up SSN.
        Dim queryResults As List(Of String) = RegentsDataContext.ExecuteQuery(Of String)("EXEC spNewCenturyIndividualMatch {0}", ssnBuilder.ToString()).ToList()

        Return (queryResults.Count > 0)
    End Function
#End Region 'Payment batch support

#Region "Business object support"
    Public Shared Sub DeleteCourse(ByVal course As Course)
        Dim arguments() As String = {course.ParentCategory.ParentHighSchool.ParentStudent.StateStudentId, course.ParentCategory.Category, course.SequenceNo}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spDeleteCourse {0}, {1}, {2}", arguments)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to delete class titled {0}.", course.Title), ex)
        End Try
    End Sub

    Public Shared Sub DeleteGrade(ByVal grade As Grade)
        Dim arguments() As String = {grade.ParentCourse.ParentCategory.ParentHighSchool.ParentStudent.StateStudentId, grade.ParentCourse.ParentCategory.Category, grade.ParentCourse.SequenceNo, grade.Term}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spDeleteGrade {0}, {1}, {2}, {3}", arguments)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to delete {0} grade {1}.", grade.ParentCourse.Title, grade.Term.ToString()), ex)
        End Try
    End Sub

    Public Shared Sub DeleteLeaveAndDeferral(ByVal parentApplication As ScholarshipApplication)
        Dim command As String = String.Format("DELETE FROM LeaveDeferral WHERE StateStudentId = '{0}'", parentApplication.ParentStudent.StateStudentId)
        RegentsDataContext.ExecuteCommand(command)
    End Sub

    Public Shared Sub DeletePayments(ByVal parentStudent As Student)
        Dim command As String = String.Format("DELETE FROM Payment WHERE StateStudentId = '{0}'", parentStudent.StateStudentId)
        RegentsDataContext.ExecuteCommand(command)
    End Sub

    Public Shared Sub DeleteReviews(ByVal parentApplication As ScholarshipApplication)
        Dim command As String = String.Format("DELETE FROM Review WHERE StateStudentId = '{0}'", parentApplication.ParentStudent.StateStudentId)
        RegentsDataContext.ExecuteCommand(command)
    End Sub

    Public Shared Function GetActScores(ByVal parentHighSchool As HighSchool) As Dictionary(Of String, Double)
        Dim arguments() As String = {parentHighSchool.ParentStudent.StateStudentId}
        Dim queryResults As IEnumerable(Of PseudoActScore) = RegentsDataContext.ExecuteQuery(Of PseudoActScore)("EXEC spGetActScores {0}", arguments)
        Dim actScores As New Dictionary(Of String, Double)()
        For Each queryResult As PseudoActScore In queryResults
            actScores.Add(queryResult.ScoreType, queryResult.Score)
        Next queryResult
        Return actScores
    End Function

    Public Shared Function GetAuthorizedThirdParties(ByVal parentStudent As Student) As List(Of AuthorizedThirdParty)
        Dim arguments() As String = {parentStudent.StateStudentId}
        Return RegentsDataContext.ExecuteQuery(Of AuthorizedThirdParty)("EXEC spGetAuthorizedThirdParties {0}", arguments).ToList()
    End Function

    Public Shared Function GetCalculatedPaymentAmount(ByVal stateStudentId As String, ByVal collegeName As String, ByVal year As Integer, ByVal semester As String, ByVal paymentType As String, ByVal credits As Double) As Double
        Dim arguments() As String = {stateStudentId, collegeName, year, semester, paymentType, credits}
        Dim calculatedPaymentAmount As Double = RegentsDataContext.ExecuteQuery(Of Double)("EXEC spGetCalculatedPaymentAmount {0}, {1}, {2}, {3}, {4}, {5}", arguments).Single()
        'Fractions of credit hours can result in fractions of cents, which are not allowed by the UI and generally undesirable.
        calculatedPaymentAmount = Math.Round(calculatedPaymentAmount, 2)
        If credits < 0 Then
            calculatedPaymentAmount = calculatedPaymentAmount * -1
        End If
        Return calculatedPaymentAmount
    End Function

    Public Shared Function GetCollege(ByVal parentStudent As Student) As College
        Dim arguments() As String = {parentStudent.StateStudentId}
        Return RegentsDataContext.ExecuteQuery(Of College)("EXEC spGetCollege {0}", arguments).Single()
    End Function

    Public Shared Function GetCourses(ByVal parentCategory As CourseCategory) As List(Of Course)
        Dim arguments() As String = {parentCategory.ParentHighSchool.ParentStudent.StateStudentId, parentCategory.Category}
        Dim queryResults As IEnumerable(Of PseudoCourse) = RegentsDataContext.ExecuteQuery(Of PseudoCourse)("EXEC spGetCourses {0}, {1}", arguments)
        Dim courses As New List(Of Course)()
        For Each queryResult As PseudoCourse In queryResults
            courses.Add(New Course(queryResult.SequenceNo, parentCategory) With {.AcademicYearTaken = queryResult.AcademicYear, .ConcurrentCollege = queryResult.ConcurrentCollege, .Credits = queryResult.Credits, .GradeLevel = queryResult.GradeLevel, .IsAcceptable = queryResult.IsAcceptable, .SchoolAttended = queryResult.School, .Title = queryResult.Title, .Verification = New Verification() With {.TimeStamp = queryResult.VerificationDate, .UserId = queryResult.VerificationUserId}, .Weight = queryResult.Weight})
        Next queryResult
        Return courses
    End Function

    Public Shared Function GetCourseCategory(ByVal category As String, ByVal parentHighSchool As HighSchool) As CourseCategory
        Dim arguments() As String = {parentHighSchool.ParentStudent.StateStudentId, category}
        Dim pcc As PseudoCourseCaregory = RegentsDataContext.ExecuteQuery(Of PseudoCourseCaregory)("EXEC spGetCourseCategory {0}, {1}", arguments).Single()
        Return New CourseCategory(category, parentHighSchool) With {.RequirementIsMet = pcc.RequirementIsMet, .Verification = New Verification() With {.TimeStamp = pcc.VerificationDate, .UserId = pcc.VerificationUserId}}
    End Function

    Public Shared Function GetDenialReasons(ByVal parentApplication As ScholarshipApplication) As HashSet(Of String)
        Dim denialReasons As New HashSet(Of String)()
        Dim arguments() As String = {parentApplication.ParentStudent.StateStudentId}
        For Each reason As String In RegentsDataContext.ExecuteQuery(Of String)("EXEC spGetDenialReasons {0}", arguments)
            denialReasons.Add(reason)
        Next reason
        Return denialReasons
    End Function

    Public Shared Function GetDocumentStatusDates(ByVal parentApplication As ScholarshipApplication) As Dictionary(Of String, Nullable(Of DateTime))
        Dim arguments() As String = {parentApplication.ParentStudent.StateStudentId}
        Dim queryResults As IEnumerable(Of PseudoDocument) = RegentsDataContext.ExecuteQuery(Of PseudoDocument)("EXEC spGetDocuments {0}", arguments)
        Dim documentStatusDates As New Dictionary(Of String, Nullable(Of DateTime))()
        For Each queryResult As PseudoDocument In queryResults
            documentStatusDates.Add(queryResult.Type, queryResult.StatusDate)
        Next queryResult
        Return documentStatusDates
    End Function

    Public Shared Function GetEmailAddress(ByVal emailType As String, ByVal parentContactInfo As ContactInformation) As Email
        Dim arguments() As String = {parentContactInfo.ParentStudent.StateStudentId, emailType}
        Return RegentsDataContext.ExecuteQuery(Of Email)("EXEC spGetEmailAddress {0}, {1}", arguments).SingleOrDefault()
    End Function

    Public Shared Function GetExemplaryAward(ByVal parentApplication As ScholarshipApplication) As AdditionalAward
        Dim arguments() As String = {parentApplication.ParentStudent.StateStudentId}
        Return RegentsDataContext.ExecuteQuery(Of AdditionalAward)("EXEC spGetExemplaryAward {0}", arguments).Single()
    End Function

    Public Shared Function GetGrades(ByVal parentCourse As Course) As List(Of Grade)
        Dim arguments() As String = {parentCourse.ParentCategory.ParentHighSchool.ParentStudent.StateStudentId, parentCourse.ParentCategory.Category, parentCourse.SequenceNo}
        Dim queryResults As IEnumerable(Of PseudoGrade) = RegentsDataContext.ExecuteQuery(Of PseudoGrade)("EXEC spGetGrades {0}, {1}, {2}", arguments)
        Dim grades As New List(Of Grade)()
        For Each queryResult As PseudoGrade In queryResults
            grades.Add(New Grade(queryResult.Term, parentCourse) With {.Letter = queryResult.Letter})
        Next queryResult
        Return grades
    End Function

    Public Shared Function GetHighSchool(ByVal parentStudent As Student) As HighSchool
        Dim arguments() As String = {parentStudent.StateStudentId}
        Dim hs As HighSchool = RegentsDataContext.ExecuteQuery(Of HighSchool)("EXEC spGetHighSchool {0}", arguments).Single()
        Return New HighSchool(parentStudent) With {.CeebCode = hs.CeebCode, .City = hs.City, .CumulativeGpa = hs.CumulativeGpa, .DiplomaIsInternationalBaccalaureate = hs.DiplomaIsInternationalBaccalaureate, .District = hs.District, .GraduationDate = hs.GraduationDate, .IsInUtah = hs.IsInUtah, .Name = hs.Name, .UsbctStatus = hs.UsbctStatus}
    End Function

    Public Shared Function GetLeaveOrDeferral(ByVal leaveOrDeferral As String, ByVal parentApplication As ScholarshipApplication) As LeaveDeferral
        Dim arguments() As String = {parentApplication.ParentStudent.StateStudentId, leaveOrDeferral}
        Return RegentsDataContext.ExecuteQuery(Of LeaveDeferral)("EXEC spGetLeaveOrDeferral {0}, {1}", arguments).SingleOrDefault()
    End Function

    Public Shared Function GetMailingAddress(ByVal addressType As String, ByVal parentContactInfo As ContactInformation) As MailingAddress
        Dim arguments() As String = {parentContactInfo.ParentStudent.StateStudentId, addressType}
        Return RegentsDataContext.ExecuteQuery(Of MailingAddress)("EXEC spGetMailingAddress {0}, {1}", arguments).SingleOrDefault()
    End Function

    Public Shared Function GetPayments(ByVal parentStudent As Student) As List(Of Payment)
        Dim arguments() As String = {parentStudent.StateStudentId}
        Return RegentsDataContext.ExecuteQuery(Of Payment)("EXEC spGetPayments {0}", arguments).ToList()
    End Function

    Public Shared Function GetPhoneNumber(ByVal phoneType As String, ByVal parentContactInfo As ContactInformation) As Phone
        Dim arguments() As String = {parentContactInfo.ParentStudent.StateStudentId, phoneType}
        Return RegentsDataContext.ExecuteQuery(Of Phone)("EXEC spGetPhoneNumber {0}, {1}", arguments).SingleOrDefault()
    End Function

    Public Shared Function GetPrimaryAward(ByVal parentApplication As ScholarshipApplication) As PrimaryAward
        Dim arguments() As String = {parentApplication.ParentStudent.StateStudentId}
        Return RegentsDataContext.ExecuteQuery(Of PrimaryAward)("EXEC spGetPrimaryAward {0}", arguments).SingleOrDefault()
    End Function

    Public Shared Function GetReviews(ByVal parentApplication As ScholarshipApplication) As List(Of Review)
        Dim arguments() As String = {parentApplication.ParentStudent.StateStudentId}
        Dim queryResults As IEnumerable(Of PseudoReview) = RegentsDataContext.ExecuteQuery(Of PseudoReview)("EXEC spGetReviews {0}", arguments)
        Dim reviews As New List(Of Review)()
        For Each queryResult As PseudoReview In queryResults
            reviews.Add(New Review(queryResult.ReviewType, parentApplication) With {.CompletionDate = queryResult.CompletionDate, .UserId = queryResult.UserId})
        Next queryResult
        Return reviews
    End Function

    Public Shared Function GetScholarshipApplication(ByVal parentStudent As Student) As ScholarshipApplication
        Dim arguments() As String = {parentStudent.StateStudentId}
        Return RegentsDataContext.ExecuteQuery(Of ScholarshipApplication)("EXEC spGetScholarshipApplication {0}", arguments).Single()
    End Function

    Public Shared Function GetStudent(ByVal studentId) As Student
        Dim arguments() As String = {studentId}
        Return RegentsDataContext.ExecuteQuery(Of Student)("EXEC spGetStudent {0}", arguments).Single()
    End Function

    Public Shared Function GetUespSupplementalAward(ByVal parentApplication As ScholarshipApplication) As AdditionalAward
        Dim arguments() As String = {parentApplication.ParentStudent.StateStudentId}
        Return RegentsDataContext.ExecuteQuery(Of AdditionalAward)("EXEC spGetUespSupplementalAward {0}", arguments).Single()
    End Function

    Public Shared Function SetAuthorizedThirdParty(ByVal thirdParty As AuthorizedThirdParty) As Long
        Dim adjustedState As String = If(thirdParty.Address1.Length > 0, thirdParty.State, "")
        Dim thirdPartyIsValidBit As Integer = If(thirdParty.IsValid, 1, 0)
        Dim arguments() As String = {thirdParty.Id, thirdParty.ParentStudent.StateStudentId, thirdParty.Name, thirdParty.Address1, thirdParty.Address2, thirdParty.City, adjustedState, thirdParty.Country, thirdParty.Zip, thirdPartyIsValidBit}
        Return RegentsDataContext.ExecuteQuery(Of Long)("EXEC spSetAuthorizedThirdParty {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", arguments).SingleOrDefault()
    End Function

    Public Shared Sub SetCollege(ByVal college As College)
        Dim arguments(6) As String
        arguments(0) = college.ParentStudent.StateStudentId()
        arguments(1) = If(college.HasOtherScholarships, "1", "0")
        arguments(2) = college.Name
        arguments(3) = college.NumberOfEnrolledCredits.ToString()
        arguments(4) = college.OtherScholarshipsAmount.ToString()
        arguments(5) = college.Term
        If college.TermBeginDate.HasValue Then
            arguments(6) = String.Format("'{0}'", college.TermBeginDate.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(6) = "NULL"
        End If
        Try
            RegentsDataContext.ExecuteCommand(String.Format("EXEC spSetCollege '{0}', {1}, '{2}', {3}, {4}, {5}, {6}", arguments))
        Catch ex As Exception
            Throw New Exception("Failed to save college details.", ex)
        End Try
    End Sub

    Public Shared Sub SetCourse(ByVal course As Course)
        Dim sproc As New SprocCommandBuilder("spSetCourse")
        sproc.AddParameter("StudentId", course.ParentCategory.ParentHighSchool.ParentStudent.StateStudentId)
        sproc.AddParameter("Category", course.ParentCategory.Category)
        sproc.AddParameter("Sequence", course.SequenceNo)
        sproc.AddParameter("Credits", course.Credits)
        sproc.AddParameter("GradeLevel", course.GradeLevel)
        sproc.AddParameter("IsAcceptable", course.IsAcceptable)
        sproc.AddParameter("Title", course.Title)
        If (course.Verification.TimeStamp.HasValue) Then sproc.AddParameter("VerificationDate", course.Verification.TimeStamp.Value)
        sproc.AddParameter("VerificationUserId", course.Verification.UserId)
        sproc.AddParameter("Weight", If(course.Weight, ""))
        If (Not String.IsNullOrEmpty(course.AcademicYearTaken)) Then sproc.AddParameter("AcademicYear", course.AcademicYearTaken)
        If (Not String.IsNullOrEmpty(course.SchoolAttended)) Then sproc.AddParameter("School", course.SchoolAttended)
        If (Not String.IsNullOrEmpty(course.ConcurrentCollege)) Then sproc.AddParameter("ConcurrentCollege", course.ConcurrentCollege)
        Try
            RegentsDataContext.ExecuteCommand(sproc.Command, sproc.ParameterValues)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save class {0}.", course.Title), ex)
        End Try
    End Sub

    Public Shared Sub SetCourseCategory(ByVal courseCategory As CourseCategory)
        Dim arguments(4) As String
        arguments(0) = courseCategory.ParentHighSchool.ParentStudent.StateStudentId
        arguments(1) = courseCategory.Category
        arguments(2) = courseCategory.RequirementIsMet
        If courseCategory.Verification.TimeStamp.HasValue Then
            arguments(3) = String.Format("'{0}'", courseCategory.Verification.TimeStamp.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(3) = "NULL"
        End If
        arguments(4) = courseCategory.Verification.UserId
        Try
            RegentsDataContext.ExecuteCommand(String.Format("EXEC spSetCourseCategory '{0}', '{1}', '{2}', {3}, '{4}'", arguments))
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save status of {0} category.", courseCategory.Category), ex)
        End Try
    End Sub

    Public Shared Sub SetEmailAddress(ByVal email As Email)
        Dim emailIsValidBit As Integer = If(email.IsValid, 1, 0)
        Dim arguments() As String = {email.ParentContactInfo.ParentStudent.StateStudentId, email.EmailType(), email.Address, emailIsValidBit}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spSetEmailAddress {0}, {1}, {2}, {3}", arguments)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save {0} E-mail address.", email.EmailType()), ex)
        End Try
    End Sub

    Public Shared Sub SetExemplaryAward(ByVal exemplaryAward As AdditionalAward)
        Dim awardIsApprovedBit As Integer = 0
        If (exemplaryAward.IsApproved.HasValue AndAlso exemplaryAward.IsApproved.Value = True) Then
            awardIsApprovedBit = 1
        End If
        Dim arguments() As String = {exemplaryAward.ParentApplication.ParentStudent.StateStudentId, exemplaryAward.ParentApplication.ApplicationYear, awardIsApprovedBit}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spSetExemplaryAward {0}, {1}, {2}", arguments)
        Catch ex As Exception
            Throw New Exception("Failed to save Exemplary Award details.", ex)
        End Try
    End Sub

    Public Shared Sub SetGrade(ByVal grade As Grade)
        Dim arguments() As String = {grade.ParentCourse.ParentCategory.ParentHighSchool.ParentStudent.StateStudentId, grade.ParentCourse.ParentCategory.Category, grade.ParentCourse.SequenceNo, grade.Term, grade.Letter}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spSetGrade {0}, {1}, {2}, {3}, {4}", arguments)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to set {0} grade {1}.", grade.ParentCourse.Title, grade.Term.ToString()), ex)
        End Try
    End Sub

    Public Shared Sub SetHighSchool(ByVal highSchool As HighSchool)
        Dim arguments(6) As String
        arguments(0) = highSchool.ParentStudent.StateStudentId()
        arguments(1) = highSchool.CeebCode
        arguments(2) = If(highSchool.DiplomaIsInternationalBaccalaureate, "1", "0")
        arguments(3) = highSchool.CumulativeGpa.ToString()
        If highSchool.GraduationDate.HasValue Then
            arguments(4) = String.Format("'{0}'", highSchool.GraduationDate.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(4) = "NULL"
        End If
        arguments(5) = If(highSchool.IsInUtah, "1", "0")
        arguments(6) = highSchool.UsbctStatus
        Try
            RegentsDataContext.ExecuteCommand(String.Format("EXEC spSetHighSchool '{0}', '{1}', {2}, {3}, {4}, {5}, '{6}'", arguments))
        Catch ex As Exception
            Throw New Exception("Failed to save high school details.", ex)
        End Try
        'Take care of the ActScores collection as well.
        For Each score As KeyValuePair(Of String, Double) In highSchool.ActScores
            Dim actArguments() As String = {highSchool.ParentStudent.StateStudentId, score.Key, score.Value}
            Try
                RegentsDataContext.ExecuteCommand("EXEC spSetActScore {0}, {1}, {2}", actArguments)
            Catch ex As Exception
                Throw New Exception(String.Format("Failed to save ACT {0} score.", score.Key), ex)
            End Try
        Next score
    End Sub

    Public Shared Sub SetLeaveOrDeferral(ByVal ld As LeaveDeferral)
        Dim arguments(4) As String
        arguments(0) = ld.ParentApplication.ParentStudent.StateStudentId
        arguments(1) = ld.LeaveOrDeferral
        If ld.BeginDate.HasValue Then
            arguments(2) = String.Format("'{0}'", ld.BeginDate.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(2) = "NULL"
        End If
        If ld.EndDate.HasValue Then
            arguments(3) = String.Format("'{0}'", ld.EndDate.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(3) = "NULL"
        End If
        arguments(4) = ld.Reason
        Try
            RegentsDataContext.ExecuteCommand(String.Format("EXEC spSetLeaveOrDeferral '{0}', '{1}', {2}, {3}, '{4}'", arguments))
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save {0}.", ld.LeaveOrDeferral), ex)
        End Try
    End Sub

    Public Shared Sub SetMailingAddress(ByVal address As MailingAddress)
        Dim sproc As New SprocCommandBuilder("spSetMailingAddress")
        sproc.AddParameter("StudentId", address.ParentContactInfo.ParentStudent.StateStudentId)
        sproc.AddParameter("AddressType", address.AddressType)
        sproc.AddParameter("Line1", address.Line1)
        sproc.AddParameter("Line2", address.Line2)
        sproc.AddParameter("City", address.City)
        sproc.AddParameter("State", address.State)
        sproc.AddParameter("ZipCode", address.ZipCode)
        sproc.AddParameter("Country", address.Country)
        sproc.AddParameter("IsValid", address.IsValid)
        sproc.AddParameter("LastUpdated", address.LastUpdated)
        Try
            RegentsDataContext.ExecuteCommand(sproc.Command, sproc.ParameterValues)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save {0} Address.", address.AddressType), ex)
        End Try
    End Sub

    Public Shared Sub SetPayment(ByVal pmt As Payment)
        Dim creditsOverrideBit As Integer = 0
        If (pmt.CreditsRequirementIsOverridden) Then creditsOverrideBit = 1
        Dim gpaOverrideBit As Integer = 0
        If (pmt.GpaRequirementIsOverridden) Then gpaOverrideBit = 1
        Dim semesterOverrideBit As Integer = 0
        If (pmt.SemesterUniquenessIsOverridden) Then semesterOverrideBit = 1

        Dim arguments(17) As String
        arguments(0) = pmt.ParentStudent().StateStudentId()
        arguments(1) = pmt.SequenceNo
        arguments(2) = pmt.College
        arguments(3) = pmt.Semester
        arguments(4) = pmt.Year
        arguments(5) = pmt.Credits
        If (pmt.Gpa.HasValue) Then
            arguments(6) = pmt.Gpa.Value
        Else
            arguments(6) = "NULL"
        End If
        arguments(7) = pmt.Type
        arguments(8) = pmt.Amount
        arguments(9) = pmt.Status
        arguments(10) = pmt.StatusDate.ToString("yyyy-MM-dd")
        arguments(11) = pmt.ScheduleReceivedDate.ToString("yyyy-MM-dd")
        If (pmt.GradesReceivedDate.HasValue) Then
            arguments(12) = String.Format("'{0}'", pmt.GradesReceivedDate.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(12) = "NULL"
        End If
        arguments(13) = creditsOverrideBit
        arguments(14) = gpaOverrideBit
        arguments(15) = semesterOverrideBit
        If (pmt.DenialReasons Is Nothing) Then
            arguments(16) = "NULL"
        Else
            arguments(16) = String.Format("'{0}'", pmt.DenialReasons)
        End If
        If (pmt.BatchNumber Is Nothing) Then
            arguments(17) = "NULL"
        Else
            arguments(17) = String.Format("'{0}'", pmt.BatchNumber)
        End If
        Try
            RegentsDataContext.ExecuteCommand(String.Format("EXEC spSetPayment '{0}', {1}, '{2}', '{3}', {4}, {5}, {6}, '{7}', {8}, '{9}', '{10}', '{11}', {12}, {13}, {14}, {15}, {16}, {17}", arguments))
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save {7} award payment to {2} for {3} {4:0000}.", arguments), ex)
        End Try
    End Sub

    Public Shared Sub SetPhoneNumber(ByVal phone As Phone)
        Dim phoneIsValidBit As Integer = If(phone.IsValid, 1, 0)
        Dim arguments() As String = {phone.ParentContactInfo.ParentStudent.StateStudentId, phone.PhoneType(), phone.Number, phoneIsValidBit}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spSetPhoneNumber {0}, {1}, {2}, {3}", arguments)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save {0} Phone.", phone.PhoneType()), ex)
        End Try
    End Sub

    Public Shared Sub SetPrimaryAward(ByVal award As PrimaryAward)
        Dim arguments(7) As String
        arguments(0) = award.ParentApplication.ParentStudent.StateStudentId()
        arguments(1) = award.Amount().ToString()
        arguments(2) = award.Description
        arguments(3) = award.ParentApplication.ApplicationYear
        arguments(4) = award.Status
        If (award.StatusDate.HasValue) Then
            arguments(5) = String.Format("'{0}'", award.StatusDate.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(5) = "NULL"
        End If
        If (award.StatusLetterSentDate.HasValue) Then
            arguments(6) = String.Format("'{0}'", award.StatusLetterSentDate.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(6) = "NULL"
        End If
        arguments(7) = award.StatusUserId
        Try
            RegentsDataContext.ExecuteCommand(String.Format("EXEC spSetPrimaryAward '{0}', {1}, '{2}', '{3}', '{4}', {5}, {6}, '{7}'", arguments))
        Catch ex As Exception
            Throw New Exception("Failed to save base award.", ex)
        End Try
    End Sub

    Public Shared Sub SetReview(ByVal rvw As Review)
        Dim arguments(3) As String
        arguments(0) = rvw.ParentApplication.ParentStudent.StateStudentId()
        arguments(1) = rvw.ReviewType()
        If rvw.CompletionDate.HasValue Then
            arguments(2) = String.Format("'{0}'", rvw.CompletionDate.Value.ToString("yyyy-MM-dd"))
        Else
            arguments(2) = "NULL"
        End If
        arguments(3) = rvw.UserId
        Try
            RegentsDataContext.ExecuteCommand(String.Format("EXEC spSetReview '{0}', '{1}', {2}, '{3}'", arguments))
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save {0} Review.", arguments(1)), ex)
        End Try
    End Sub

    Public Shared Sub SetScholarshipApplication(ByVal application As ScholarshipApplication)
        Dim attendedAnotherSchoolBit As Integer = 0
        If (application.AttendedAnotherSchool.HasValue AndAlso application.AttendedAnotherSchool.Value = True) Then
            attendedAnotherSchoolBit = 1
        End If
        Dim applicationArguments() As String = {application.ParentStudent.StateStudentId, application.ApplicationYear, attendedAnotherSchoolBit, application.HowTheyHeardAboutRegents, application.NinthGradeSchool, application.PlannedCollegeToAttend}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spSetScholarshipApplication {0}, {1}, {2}, {3}, {4}, {5}", applicationArguments)
        Catch ex As Exception
            Throw New Exception("Failed to save scholarship application.", ex)
        End Try
        'Take care of the DenialReasons and DocumentStatusDates collections as well.
        RegentsDataContext.ExecuteCommand(String.Format("DELETE FROM Denial WHERE StateStudentId = '{0}'", application.ParentStudent.StateStudentId))
        For Each denialReason As String In application.DenialReasons
            Dim denialArguments() As String = {application.ParentStudent.StateStudentId, denialReason}
            Try
                RegentsDataContext.ExecuteCommand("EXEC spSetDenialReason {0}, {1}", denialArguments)
            Catch ex As Exception
                Throw New Exception(String.Format("Failed to save denial reason '{0}.'", denialReason), ex)
            End Try
        Next denialReason
        RegentsDataContext.ExecuteCommand(String.Format("DELETE FROM Document WHERE StateStudentId = '{0}'", application.ParentStudent.StateStudentId))
        For Each document As KeyValuePair(Of String, Nullable(Of DateTime)) In application.DocumentStatusDates
            Dim documentName As String = document.Key
            Dim documentArguments(2) As String
            documentArguments(0) = application.ParentStudent.StateStudentId()
            documentArguments(1) = documentName
            If document.Value.HasValue Then
                documentArguments(2) = String.Format("'{0}'", document.Value.Value.ToString("yyyy-MM-dd"))
            Else
                documentArguments(2) = "NULL"
            End If
            Try
                RegentsDataContext.ExecuteCommand(String.Format("EXEC spSetDocument '{0}', '{1}', {2}", documentArguments))
            Catch ex As Exception
                Throw New Exception(String.Format("Failed to save status date for {0} Received.", documentName), ex)
            End Try
        Next document
    End Sub

    Public Shared Sub SetStudent(ByVal student As Student)
        Dim arguments(13) As String
        arguments(0) = student.StateStudentId
        arguments(1) = student.SocialSecurityNumber
        arguments(2) = student.FirstName
        arguments(3) = student.MiddleName
        arguments(4) = student.LastName
        arguments(5) = student.AlternateLastName
        arguments(6) = student.DateOfBirth.ToString("yyyy-MM-dd")
        arguments(7) = student.Gender
        arguments(8) = student.Ethnicity
        arguments(9) = If(student.HasCriminalRecord, "1", "0")
        arguments(10) = If(student.HasUespAccount, "1", "0")
        arguments(11) = If(student.IsUsCitizen, "1", "0")
        arguments(12) = If(student.IsEligibleForFederalAid, "1", "0")
        arguments(13) = If(student.IntendsToApplyForFederalAid, "1", "0")
        Try
            RegentsDataContext.ExecuteCommand("EXEC spSetStudent {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}", arguments)
        Catch ex As Exception
            Throw New Exception("Failed to save student.", ex)
        End Try
    End Sub

    Public Shared Sub SetUespSupplementalAward(ByVal uespSupplementalAward As AdditionalAward)
        Dim approvedBit As Integer = 0
        If (uespSupplementalAward.IsApproved.HasValue AndAlso uespSupplementalAward.IsApproved.Value = True) Then
            approvedBit = 1
        End If
        Dim arguments() As String = {uespSupplementalAward.ParentApplication.ParentStudent.StateStudentId, approvedBit, uespSupplementalAward.Amount}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spSetUespSupplementalAward {0}, {1}, {2}", arguments)
        Catch ex As Exception
            Throw New Exception("Failed to save UESP Supplemental Award details.", ex)
        End Try
    End Sub
#End Region 'Business object support

#Region "Front-end support"
    Public Shared Sub AddUser(ByVal newUser As User)
        Dim arguments() As String = {newUser.Id, newUser.PasswordDate, newUser.PasswordHash, newUser.AccessLevel}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spAddUser {0}, {1}, {2}, {3}", arguments)
        Catch ex As Exception
            Throw New Exception(String.Format("Failed to save {0}'s details.", newUser.Id), ex)
        End Try
    End Sub

    ''' <summary>
    ''' Updates the student ID in all tables that have a StateStudentId field, without touching any other values.
    ''' </summary>
    ''' <param name="userId">The logged-in user's ID.</param>
    ''' <param name="oldId">The existing StateStudentId that needs to be changed.</param>
    ''' <param name="newId">The ID to be used as the new StateStudentId.</param>
    Public Shared Sub ChangeStudentId(ByVal userId As String, ByVal oldId As String, ByVal newId As String)
        Dim arguments() As String = {userId, oldId, newId}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spChangeStudentId {0}, {1}, {2}", arguments)
        Catch ex As Exception
            Throw New Exception("Failed to change student ID.", ex)
        End Try
    End Sub

    Public Shared Function GetAllUsers() As IEnumerable(Of User)
        Return RegentsDataContext.ExecuteQuery(Of User)("EXEC spGetAllUsers")
    End Function

    Public Shared Function GetCommunications(ByVal entityId As String, ByVal entityType As String) As IEnumerable(Of Communication)
        Dim arguments() As String = {entityId, entityType}
        Return RegentsDataContext.ExecuteQuery(Of Communication)("EXEC spGetCommunications {0}, {1}", arguments)
    End Function

    Public Shared Function GetCurrentUserRecord(ByVal userId As String) As User
        Return GetAllUsers().Where(Function(p) p.Id = userId).OrderByDescending(Function(p) p.PasswordDate).FirstOrDefault()
    End Function

    Public Shared Function GetExistingStudentIds() As IEnumerable(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT StateStudentId FROM Student")
    End Function

    Public Shared Function GetMainMenuSearchResults(ByVal studentId As String, ByVal ssn As String, ByVal firstName As String, ByVal lastName As String, ByVal dateOfBirth As DateTime) As IEnumerable(Of MainMenuSearchResult)
        Dim sproc As New Q.SprocCommandBuilder("spGetMainMenuSearchResults")
        If (Not String.IsNullOrEmpty(studentId)) Then sproc.AddParameter("StudentId", studentId)
        If (Not String.IsNullOrEmpty(ssn)) Then sproc.AddParameter("SSN", ssn)
        If (Not String.IsNullOrEmpty(firstName)) Then sproc.AddParameter("FirstName", firstName)
        If (Not String.IsNullOrEmpty(lastName)) Then sproc.AddParameter("LastName", lastName)
        If (dateOfBirth <> DateTime.MinValue) Then sproc.AddParameter("DateOfBirth", dateOfBirth)
        Return RegentsDataContext.ExecuteQuery(Of MainMenuSearchResult)(sproc.Command, sproc.ParameterValues)
    End Function

    Public Shared Function GetTransactionRecords(ByVal userId As String, ByVal studentId As String) As IEnumerable(Of Transaction)
        Dim conditions As New List(Of String)()
        If (Not String.IsNullOrEmpty(userId)) Then conditions.Add(String.Format("UserId = '{0}'", userId))
        If (Not String.IsNullOrEmpty(studentId)) Then conditions.Add(String.Format("StateStudentId = '{0}'", studentId))

        Dim queryBuilder As New StringBuilder("SELECT")
        queryBuilder.Append(" UserId,")
        queryBuilder.Append(" StateStudentId,")
        queryBuilder.Append(" [TimeStamp],")
        queryBuilder.Append(" [Property] AS ChangedProperty,")
        queryBuilder.Append(" OldValue,")
        queryBuilder.Append(" NewValue")
        queryBuilder.Append(" FROM [Transaction]")
        If (conditions.Count > 0) Then
            Dim conditionStatement As String = String.Join(" AND ", conditions.ToArray())
            queryBuilder.Append(String.Format(" WHERE {0}", conditionStatement))
        End If
        Return RegentsDataContext.ExecuteQuery(Of Transaction)(queryBuilder.ToString())
    End Function

    Public Shared Function GetTransactionStudentIds(ByVal userId As String) As IEnumerable(Of String)
        Dim query As String = "SELECT DISTINCT StateStudentId FROM [Transaction]"
        If (Not String.IsNullOrEmpty(userId)) Then query += String.Format(" WHERE UserId = '{0}'", userId)
        query += " ORDER BY StateStudentId"
        Return RegentsDataContext.ExecuteQuery(Of String)(query)
    End Function

    Public Shared Function GetTransactionUserIds(ByVal studentId As String) As IEnumerable(Of String)
        Dim query As String = "SELECT DISTINCT UserId FROM [Transaction]"
        If (Not String.IsNullOrEmpty(studentId)) Then query += String.Format(" WHERE StateStudentId = '{0}'", studentId)
        query += " ORDER BY UserId"
        Return RegentsDataContext.ExecuteQuery(Of String)(query)
    End Function

    Public Shared Sub ReleaseLock(ByVal userId As String)
        Dim arguments() As String = {userId}
        RegentsDataContext.ExecuteCommand("EXEC spReleaseLock {0}", arguments)
    End Sub

    Public Shared Function ReviewIsStarted(ByVal studentId As String, ByVal userId As String, ByVal reviewType As String) As Boolean
        'We're only tracking the transcript reviews, so make sure the review type is one of those.
        Dim transcriptReviews() As String = {Constants.ReviewType.INITIAL_TRANSCRIPT, Constants.ReviewType.SECOND_TRANSCRIPT, Constants.ReviewType.FINAL_TRANSCRIPT}
        Debug.Assert(transcriptReviews.Contains(reviewType), reviewType + " is not one of the reviews used in performance tracking.")

        Dim query As String = "SELECT COUNT(*) FROM ReviewTime WHERE StateStudentId = {0} AND UserId = {1} AND ReviewType = {2} AND StopTime IS NULL"
        Return (RegentsDataContext.ExecuteQuery(Of Integer)(query, studentId, userId, reviewType).Single() > 0)
    End Function

    Public Shared Sub SetCommunication(ByVal comm As Communication)
        Dim arguments() As Object = {comm.EntityID, comm.EntityType, comm.TimeStamp, comm.UserId, comm.Type, comm.Source, comm.Subject, comm.Text, If(comm.Is411, 1, 0)}
        Try
            RegentsDataContext.ExecuteCommand("EXEC spSetCommunication {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", arguments)
        Catch ex As Exception
            Throw New Exception("Failed to save communication record.", ex)
        End Try
    End Sub

    Public Shared Function SetLock(ByVal userId As String, ByVal studentId As String) As String
        Dim arguments() As String = {userId, studentId}
        Return RegentsDataContext.ExecuteQuery(Of String)("EXEC spSetLock {0}, {1}", arguments).SingleOrDefault()
    End Function

    Public Shared Sub StartReview(ByVal studentId As String, ByVal userId As String, ByVal reviewType As String)
        'We're only tracking the transcript reviews, so make sure the review type is one of those.
        Dim transcriptReviews() As String = {Constants.ReviewType.INITIAL_TRANSCRIPT, Constants.ReviewType.SECOND_TRANSCRIPT, Constants.ReviewType.FINAL_TRANSCRIPT}
        Debug.Assert(transcriptReviews.Contains(reviewType), reviewType + " is not one of the reviews used in performance tracking.")

        Dim command As String = "EXEC spStartReview {0}, {1}, {2}, {3}"
        RegentsDataContext.ExecuteCommand(Command, studentId, userId, reviewType, DateTime.Now)
    End Sub

    Public Shared Sub StopReview(ByVal studentId As String, ByVal userId As String, ByVal reviewType As String)
        'We're only tracking the transcript reviews, so make sure the review type is one of those.
        Dim transcriptReviews() As String = {Constants.ReviewType.INITIAL_TRANSCRIPT, Constants.ReviewType.SECOND_TRANSCRIPT, Constants.ReviewType.FINAL_TRANSCRIPT}
        Debug.Assert(transcriptReviews.Contains(reviewType), reviewType + " is not one of the reviews used in performance tracking.")

        Dim command As String = "EXEC spStopReview {0}, {1}, {2}, {3}"
        RegentsDataContext.ExecuteCommand(command, studentId, userId, reviewType, DateTime.Now)
    End Sub
#End Region 'Front-end support

#Region "Report support"
    Public Shared Function GetApplicationsReceived(ByVal applicationYear As String) As List(Of DSApplicationsReceived)
        Return RegentsDataContext.ExecuteQuery(Of DSApplicationsReceived)("EXEC spApplicationsReceived {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetApplicationsReceivedByHighSchool(ByVal applicationYear As String) As List(Of DSApplicationsReceivedByHighSchool)
        Return RegentsDataContext.ExecuteQuery(Of DSApplicationsReceivedByHighSchool)("EXEC spApplicationsReceivedByHighSchool {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetApplicationStatus(ByVal applicationYear As String) As List(Of DSAppStatus)
        Return RegentsDataContext.ExecuteQuery(Of DSAppStatus)("EXEC spAppStatusReport {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetAwardStatusBySchool(ByVal applicationYear As String) As List(Of DSAwardStatusBySchool)
        Return RegentsDataContext.ExecuteQuery(Of DSAwardStatusBySchool)("EXEC spAwardStatusByHighSchool {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetCallCenterVolume() As List(Of DSCallCenterVolume)
        Return RegentsDataContext.ExecuteQuery(Of DSCallCenterVolume)("EXEC spCallCenterVolumeRpt").ToList()
    End Function

    Public Shared Function GetCollegeAttendance(ByVal awardStatusCode As Integer, ByVal applicationYear As String) As List(Of DSCollegeAttendance)
        Return RegentsDataContext.ExecuteQuery(Of DSCollegeAttendance)("EXEC spCollegeAttendanceRpts {0}, {1}", awardStatusCode, applicationYear).ToList()
    End Function

    Public Shared Function GetCommCategoryVolume(ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of DSCommCategoryVolume)
        Return RegentsDataContext.ExecuteQuery(Of DSCommCategoryVolume)("EXEC spCommCategoryVolume {0}, {1}", startDate, endDate).ToList()
    End Function

    Public Shared Function GetCommunicationRecords(ByVal entity As String, ByVal entityType As String, ByVal fromDate As String, ByVal toDate As String) As List(Of DSCommunicationRecord)
        Return RegentsDataContext.ExecuteQuery(Of DSCommunicationRecord)("EXEC spGetCommunicationRecords {0}, {1}, {2}, {3}", entity, entityType, fromDate, toDate).ToList()
    End Function

    Public Shared Function GetConditionalReviewAward(ByVal applicationYear As String) As List(Of DSConditionalReviewAward)
        Return RegentsDataContext.ExecuteQuery(Of DSConditionalReviewAward)("EXEC spConditionalReviewAwardRpt {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetConditionalReviewAwardBySchool(ByVal applicationYear As String) As List(Of DSConditionalReviewAwardBySchool)
        Return RegentsDataContext.ExecuteQuery(Of DSConditionalReviewAwardBySchool)("EXEC spConditionalReviewAwardBySchoolRpt {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetEndOfCycleAppeals(ByVal applicationYear As String) As List(Of DSEndOfCycleAppeals)
        Return RegentsDataContext.ExecuteQuery(Of DSEndOfCycleAppeals)("EXEC spEndOfCycleAppeals {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetEndOfCycleCallCenterVolume() As List(Of DSEndOfCycleCallCenterVolume)
        Return RegentsDataContext.ExecuteQuery(Of DSEndOfCycleCallCenterVolume)("EXEC spEndOfCycleCallCenterVolume").ToList()
    End Function

    Public Shared Function GetEndOfCycleColleges(ByVal applicationYear As String) As List(Of DSEndOfCycleColleges)
        Return RegentsDataContext.ExecuteQuery(Of DSEndOfCycleColleges)("EXEC spEndOfCycleColleges {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetEndOfCycleOutcomes(ByVal applicationYear As String) As List(Of DSEndOfCycleOutcomes)
        Return RegentsDataContext.ExecuteQuery(Of DSEndOfCycleOutcomes)("EXEC spEndOfCycleOutcomes {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetFinalApprovalNegativePayments(ByVal batchNumber As String) As List(Of DSFinalApprovalNegativePayments)
        Return RegentsDataContext.ExecuteQuery(Of DSFinalApprovalNegativePayments)("EXEC spFinalApprovalNegativePayments {0}", batchNumber).ToList()
    End Function

    Public Shared Function GetFinalApprovalPayments(ByVal batchNumber As String) As List(Of DSFinalApprovalPayments)
        Return RegentsDataContext.ExecuteQuery(Of DSFinalApprovalPayments)("EXEC spFinalApprovalPayments {0}", batchNumber).ToList()
    End Function

    Public Shared Function GetFinalDeniedPayments(ByVal batchNumber As String) As List(Of DSDeniedPayments)
        Return RegentsDataContext.ExecuteQuery(Of DSDeniedPayments)("EXEC spFinalDeniedPayments {0}", batchNumber).ToList()
    End Function

    Public Shared Function GetFinalReview() As List(Of DSReview)
        Return RegentsDataContext.ExecuteQuery(Of DSReview)("EXEC spReviewRpt 8").ToList()
    End Function

    Public Shared Function GetFinalWeeklyAppeals(ByVal applicationYear As String) As List(Of DSWeeklyAppeals)
        Return RegentsDataContext.ExecuteQuery(Of DSWeeklyAppeals)("EXEC spFinalWeeklyAppeals {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetFinalWeeklyApps(ByVal applicationYear As String) As List(Of DSFinalWeeklyApps)
        Return RegentsDataContext.ExecuteQuery(Of DSFinalWeeklyApps)("EXEC spFinalWeeklyApps {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetFinalWeeklyDeferments(ByVal applicationYear As String) As List(Of DSFinalWeeklyDeferments)
        Return RegentsDataContext.ExecuteQuery(Of DSFinalWeeklyDeferments)("EXEC spFinalWeeklyDeferments {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetFinalWeeklyOutcomes(ByVal applicationYear As String) As List(Of DSFinalWeeklyOutcomes)
        Return RegentsDataContext.ExecuteQuery(Of DSFinalWeeklyOutcomes)("EXEC spFinalWeeklyOutcomes {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetFinalWeeklyReadyForReview(ByVal applicationYear As String) As List(Of DSFinalWeeklyReadyForReview)
        Return RegentsDataContext.ExecuteQuery(Of DSFinalWeeklyReadyForReview)("EXEC spFinalWeeklyReadyForReview {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetFinalWeeklyStages(ByVal applicationYear As String) As List(Of DSFinalWeeklyStages)
        Return RegentsDataContext.ExecuteQuery(Of DSFinalWeeklyStages)("EXEC spFinalWeeklyStages {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetInitialReviewReport() As List(Of DSInitialReviewReport)
        Return RegentsDataContext.ExecuteQuery(Of DSInitialReviewReport)("EXEC spInitialReviewReport").ToList()
    End Function

    Public Shared Function GetInitialWeeklyAppeals(ByVal applicationYear As String) As List(Of DSWeeklyAppeals)
        Return RegentsDataContext.ExecuteQuery(Of DSWeeklyAppeals)("EXEC spInitialWeeklyAppeals {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetInitialWeeklyApps(ByVal applicationYear As String) As List(Of DSInitialWeeklyApps)
        Return RegentsDataContext.ExecuteQuery(Of DSInitialWeeklyApps)("EXEC spInitialWeeklyApps {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetInitialWeeklyStages(ByVal applicationYear As String) As List(Of DSInitialWeeklyStages)
        Return RegentsDataContext.ExecuteQuery(Of DSInitialWeeklyStages)("EXEC spInitialWeeklyStages {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetInitialWeeklyUesp(ByVal applicationYear As String) As List(Of DSGeneric)
        Return RegentsDataContext.ExecuteQuery(Of DSGeneric)("EXEC spInitialWeeklyUesp {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetMissingCollegeTranscripts(ByVal applicationYear As String) As List(Of DSGeneric)
        Return RegentsDataContext.ExecuteQuery(Of DSGeneric)("EXEC spMissingCollegeTranscriptListRpt {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetMissingCollegeTranscriptsFinal(ByVal applicationYear As String) As List(Of DSGeneric)
        Return RegentsDataContext.ExecuteQuery(Of DSGeneric)("EXEC spMissingCollegeTranscriptListFinal {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetNewCenturyAwardMatches(ByVal applicationYear As String) As List(Of DSNewCenturyMatches)
        Return RegentsDataContext.ExecuteQuery(Of DSNewCenturyMatches)("EXEC spNewCenturyAwardMatches {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetPaymentsNotRenewed() As List(Of DSPaymentsNotRenewed)
        Return RegentsDataContext.ExecuteQuery(Of DSPaymentsNotRenewed)("EXEC spPaymentsNotRenewedReport").ToList()
    End Function

    Public Shared Function GetPrelimApprovalPayments() As List(Of DSPrelimApprovalPayments)
        Return RegentsDataContext.ExecuteQuery(Of DSPrelimApprovalPayments)("EXEC spPrelimApprovalPayments").ToList()
    End Function

    Public Shared Function GetPrelimDeniedPayments() As List(Of DSDeniedPayments)
        Return RegentsDataContext.ExecuteQuery(Of DSDeniedPayments)("EXEC spPrelimDeniedPayments").ToList()
    End Function

    Public Shared Function GetReadyForConditionalReview(ByVal applicationYear As String) As List(Of DSGeneric)
        Return RegentsDataContext.ExecuteQuery(Of DSGeneric)("EXEC spReadyForConditionalReview {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetReadyForFinalReview(ByVal applicationYear As String) As List(Of DSGeneric)
        Return RegentsDataContext.ExecuteQuery(Of DSGeneric)("EXEC spReadyForFinalReview {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetRenewalAndPaymentStatus() As List(Of DSRenewalAndPaymentStatus)
        'Set up a list for the return value.
        Dim renewalAndPaymentStatuses As New List(Of DSRenewalAndPaymentStatus)()

        'Go through all of the student-level records to build payment-level records.
        For Each studentRecord As DSRenewalAndPaymentStatus In RegentsDataContext.ExecuteQuery(Of DSRenewalAndPaymentStatus)("EXEC spRenewalAndPaymentStatusReport")
            'Get the payment records for this student.
            Dim payments As IEnumerable(Of Payment) = RegentsDataContext.ExecuteQuery(Of Payment)("EXEC spGetPayments {0}", studentRecord.StudentId).ToList()

            'Determine how many credit hours and semesters of eligibility remain.
            Dim creditHoursRemaining As Double = 0
            Dim semestersRemaining As Integer = 0
            Dim positiveExemplaryPayments As IEnumerable(Of Payment) = payments.Where(Function(p) p.Type = Constants.PaymentType.EXEMPLARY AndAlso p.Amount > 0)
            'Check whether no payments have been made or whether only the base award was earned.
            If (positiveExemplaryPayments.Count() = 0) Then
                If (payments.Where(Function(p) p.Type = Constants.PaymentType.BASE OrElse p.Type = Constants.PaymentType.UESP).Count() > 0) Then
                    'Base award or Base/UESP only. The student is not eligible for more payments, so leave the eligibility numbers at 0.
                Else
                    'No payments have been made.
                    creditHoursRemaining = Constants.MAX_CREDIT_HOURS_PAYABLE
                    semestersRemaining = If(studentRecord.AwardLevel = "Base Only" OrElse studentRecord.AwardLevel = "Base & UESP", 1, Constants.MAX_SEMESTERS_PAYABLE)
                End If
            Else
                Dim creditHoursPaid As Double = positiveExemplaryPayments.Sum(Function(p) p.Credits)
                creditHoursRemaining = Constants.MAX_CREDIT_HOURS_PAYABLE - creditHoursPaid
                Dim semestersPaid As Integer = 0
                For Each paymentYear As Integer In positiveExemplaryPayments.Select(Function(p) p.Year).Distinct()
                    Dim paidYear As Integer = paymentYear
                    For Each paymentTerm As String In positiveExemplaryPayments.Where(Function(p) p.Year = paidYear).Select(Function(p) p.Semester).Distinct()
                        semestersPaid += 1
                    Next paymentTerm
                Next paymentYear
                semestersRemaining = Constants.MAX_SEMESTERS_PAYABLE - semestersPaid
            End If

            If (payments.Count() = 0) Then
                Dim semesterRecord As New DSRenewalAndPaymentStatus()
                semesterRecord.ApplicationYear = studentRecord.ApplicationYear
                semesterRecord.AwardLevel = studentRecord.AwardLevel
                semesterRecord.AwardStatus = studentRecord.AwardStatus
                semesterRecord.College = GetCollegeAbbreviation(studentRecord.College)
                semesterRecord.Disbursement = 0
                semesterRecord.FirstName = studentRecord.FirstName
                semesterRecord.LastName = studentRecord.LastName
                semesterRecord.LeaveOrDeferralEnd = studentRecord.LeaveOrDeferralEnd
                semesterRecord.RemainingHours = creditHoursRemaining
                semesterRecord.RemainingSemesters = semestersRemaining
                semesterRecord.Semester = ""
                semesterRecord.StudentId = studentRecord.StudentId
                semesterRecord.YearLimitIsMet = studentRecord.YearLimitIsMet
                renewalAndPaymentStatuses.Add(semesterRecord)
            Else
                'Find the college the student is currently attending, which is sometimes not the one they started at.
                Dim latestCollege As String = GetCollegeAbbreviation(payments.OrderBy(Function(p) p.SequenceNo).Last().College)
                'Create semester-level records.
                For Each paymentYear As Integer In payments.Select(Function(p) p.Year).Distinct().OrderBy(Function(p) p)
                    Dim paidYear As Integer = paymentYear
                    For Each term As String In New String() {"Winter", "Spring", "Summer", "Fall"}
                        Dim semester As String = term
                        Dim disbursement As Double = payments.Where(Function(p) p.Year = paidYear AndAlso p.Semester = semester).Sum(Function(p) p.Amount)
                        If (disbursement <> 0) Then
                            Dim semesterRecord As New DSRenewalAndPaymentStatus()
                            semesterRecord.ApplicationYear = studentRecord.ApplicationYear
                            semesterRecord.AwardLevel = studentRecord.AwardLevel
                            semesterRecord.AwardStatus = studentRecord.AwardStatus
                            semesterRecord.College = latestCollege
                            semesterRecord.Disbursement = disbursement
                            semesterRecord.FirstName = studentRecord.FirstName
                            semesterRecord.LastName = studentRecord.LastName
                            semesterRecord.LeaveOrDeferralEnd = studentRecord.LeaveOrDeferralEnd
                            semesterRecord.RemainingHours = creditHoursRemaining
                            semesterRecord.RemainingSemesters = semestersRemaining
                            semesterRecord.Semester = String.Format("{0} {1}", semester, paidYear)
                            semesterRecord.StudentId = studentRecord.StudentId
                            semesterRecord.YearLimitIsMet = studentRecord.YearLimitIsMet
                            renewalAndPaymentStatuses.Add(semesterRecord)
                        End If
                    Next term
                Next paymentYear
            End If
        Next studentRecord

        Return renewalAndPaymentStatuses
    End Function

    Public Shared Function GetReviewStatus(ByVal applicationYear As String) As List(Of DSReviewStatus)
        Return RegentsDataContext.ExecuteQuery(Of DSReviewStatus)("EXEC spReviewStatus {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetUespAwardReview() As List(Of DSUespAwardReview)
        Return RegentsDataContext.ExecuteQuery(Of DSUespAwardReview)("EXEC spUESPAwardReviewRpt").ToList()
    End Function

    Public Shared Function GetUespAwardsBySchool(ByVal applicationYear As String) As List(Of DSUespAwardsBySchool)
        Return RegentsDataContext.ExecuteQuery(Of DSUespAwardsBySchool)("EXEC spUESPAwardsBySchoolRpt {0}", applicationYear).ToList()
    End Function

    Public Shared Function GetOutstandingDocumentRequests(ByVal reportType As Integer, ByVal applicationYear As String) As List(Of DSDocRequestDecision)
        Return RegentsDataContext.ExecuteQuery(Of DSDocRequestDecision)("EXEC spDocRequestDecisionRpts {0}, {1}", reportType, applicationYear).ToList()
    End Function

    Private Shared Function GetCollegeAbbreviation(ByVal college As String)
        Select Case college
            Case "College of Eastern Utah"
                Return "CEU"
            Case "Dixie State College"
                Return "Dixie"
            Case "Salt Lake Community College"
                Return "SLCC"
            Case "Snow College"
                Return "Snow"
            Case "Southern Utah University"
                Return "SUU"
            Case "University of Utah"
                Return "U of U"
            Case "Utah College of Applied Technology"
                Return "UCAT"
            Case "Utah State University"
                Return "USU"
            Case "Utah Valley University"
                Return "UVU"
            Case "Weber State University"
                Return "Weber"
            Case "Brigham Young University"
                Return "BYU"
            Case "LDS Business College"
                Return "LDS"
            Case "Western Governors University"
                Return "WGU"
            Case "Westminster College"
                Return "Westminster"
            Case "USU-College of Eastern Utah"
                Return "USU-CEU"
            Case Else
                Return ""
        End Select
    End Function
#End Region 'Report support

#Region "Lookups"
    ''' <summary>
    ''' Return a list of access level descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetAccessLevelLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM AccessLevelLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of ACT type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetActTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM ActTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of address type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetAddressTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM AddressTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of award amounts with their corresponding award descriptions
    ''' and award years from the lookup table.
    ''' </summary>
    Public Shared Function GetAwardAmountLookups() As List(Of Lookups.AwardAmount)
        Return RegentsDataContext.ExecuteQuery(Of Lookups.AwardAmount)("SELECT Description, Amount, AwardYear FROM AwardAmountLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of award status descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetAwardStatusLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM AwardStatusLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of possible "class acceptable" status descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetClassStatusLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM ClassStatusLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of class titles with their corresponding attributes from the lookup table.
    ''' </summary>
    Public Shared Function GetClassTitleLookups() As List(Of Lookups.ClassTitle)
        Dim queryBuilder As New StringBuilder("SELECT")
        queryBuilder.Append(" A.Description AS Title,")
        queryBuilder.Append(" B.Description AS Type,")
        queryBuilder.Append(" A.ConditionalSchoolCode,")
        queryBuilder.Append(" A.IsInApprovedList,")
        queryBuilder.Append(" COALESCE(C.Description, '') AS Weight,")
        queryBuilder.Append(" A.ConditionalSchoolApprovalYears")
        queryBuilder.Append(" FROM ClassTitleLookup A")
        queryBuilder.Append(" INNER JOIN ClassTypeLookup B ON A.ClassTypeCode = B.Code")
        queryBuilder.Append(" LEFT OUTER JOIN ClassWeightLookup C ON A.WeightCode = C.Code")
        Return RegentsDataContext.ExecuteQuery(Of Lookups.ClassTitle)(queryBuilder.ToString()).ToList()
    End Function

    ''' <summary>
    ''' Returns a list of class type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetClassTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM ClassTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of class weight descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetClassWeightLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM ClassWeightLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of college names and indicators of whether they're in the default list from the lookup table.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCollegeLookups() As List(Of Lookups.College)
        Return RegentsDataContext.ExecuteQuery(Of Lookups.College)("SELECT Description AS Name, IsInDefaultList FROM CollegeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of college term descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetCollegeTermLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM CollegeTermLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of communication source descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetCommunicationSourceLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM CommunicationSourceLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of communication type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetCommunicationTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM CommunicationTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of denial reason descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetDenialReasonLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM DenialReasonLookup WHERE Active = 1").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of districts and their corresponding communication directory IDs from the lookup table.
    ''' </summary>
    Public Shared Function GetDistrictLookups() As List(Of Lookups.District)
        Return RegentsDataContext.ExecuteQuery(Of Lookups.District)("SELECT Name, CommunicationDirectoryID FROM DistrictLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of document type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetDocumentTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM DocumentTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of e-mail type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetEmailTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM EmailTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of ethnicity descriptions and indicators of whether they're in the default list from the lookup table.
    ''' </summary>
    Public Shared Function GetEthnicityLookups() As List(Of Lookups.Ethnicity)
        Return RegentsDataContext.ExecuteQuery(Of Lookups.Ethnicity)("SELECT Description, IsInDefaultList FROM EthnicityLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of gender descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetGenderLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM GenderLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of grade letters and their corresponding GPA values from the lookup table.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetGradeLookups() As List(Of Lookups.Grade)
        Return RegentsDataContext.ExecuteQuery(Of Lookups.Grade)("SELECT Description AS Letter, GpaValue FROM GradeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of sources from which a student may have heard about the scholarship from the lookup table.
    ''' </summary>
    Public Shared Function GetHearAboutSourceLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM HearAboutSourceLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of leave/deferral reason descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetLeaveDeferralReasonLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM LeaveDeferralReasonLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of leave/deferral type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetLeaveDeferralTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM LeaveDeferralTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of payment types from the lookup table.
    ''' </summary>
    Public Shared Function GetPaymentTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Type FROM PaymentTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of phone type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetPhoneTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM PhoneTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of program operation years and their corresponding deadlines from the lookup table.
    ''' </summary>
    Public Shared Function GetProgramOperationYearLookups() As List(Of Lookups.ProgramOperationYear)
        Return RegentsDataContext.ExecuteQuery(Of Lookups.ProgramOperationYear)("SELECT Year, ApplicationDeadline, PriorityDeadline, FinalDeadline FROM ProgramOperationYears").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of review type descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetReviewTypeLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM ReviewTypeLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of schools, with all their attributes, from the lookup table.
    ''' </summary>
    Public Shared Function GetSchoolLookups() As List(Of Lookups.School)
        Dim queryBuilder As New StringBuilder("SELECT")
        queryBuilder.Append(" A.CeebCode,")
        queryBuilder.Append(" A.Name,")
        queryBuilder.Append(" A.District,")
        queryBuilder.Append(" A.City,")
        queryBuilder.Append(" B.Abbreviation AS StateAbbreviation,")
        queryBuilder.Append(" A.Zip,")
        queryBuilder.Append(" A.Type")
        queryBuilder.Append(" FROM SchoolLookup A")
        queryBuilder.Append(" INNER JOIN StateLookup B ON A.StateCode = B.Code")
        Return RegentsDataContext.ExecuteQuery(Of Lookups.School)(queryBuilder.ToString()).ToList()
    End Function

    ''' <summary>
    ''' Returns a list of state names and abbreviations from the lookup table.
    ''' </summary>
    Public Shared Function GetStateLookups() As List(Of Lookups.State)
        Return RegentsDataContext.ExecuteQuery(Of Lookups.State)("SELECT Abbreviation, Name FROM StateLookup").ToList()
    End Function

    ''' <summary>
    ''' Returns a list of USBCT status descriptions from the lookup table.
    ''' </summary>
    Public Shared Function GetUsbctStatusLookups() As List(Of String)
        Return RegentsDataContext.ExecuteQuery(Of String)("SELECT Description FROM UsbctStatusLookup").ToList()
    End Function
#End Region 'Lookups

#Region "Projection classes"
    Private Class PseudoActScore
        Private _scoreType As String
        Public Property ScoreType() As String
            Get
                Return _scoreType
            End Get
            Set(ByVal value As String)
                _scoreType = value
            End Set
        End Property

        Private _score As Double
        Public Property Score() As Double
            Get
                Return _score
            End Get
            Set(ByVal value As Double)
                _score = value
            End Set
        End Property
    End Class

    Private Class PseudoCourse
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

        Private _isAcceptable As String
        Public Property IsAcceptable() As String
            Get
                Return _isAcceptable
            End Get
            Set(ByVal value As String)
                _isAcceptable = value
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

        Private _title As String
        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Private _verificationDate As Nullable(Of DateTime)
        Public Property VerificationDate() As Nullable(Of DateTime)
            Get
                Return _verificationDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _verificationDate = value
            End Set
        End Property

        Private _verificationUserId As String
        Public Property VerificationUserId() As String
            Get
                Return _verificationUserId
            End Get
            Set(ByVal value As String)
                _verificationUserId = value
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

        Private _academicYear As String
        Public Property AcademicYear() As String
            Get
                Return _academicYear
            End Get
            Set(ByVal value As String)
                _academicYear = value
            End Set
        End Property

        Private _school As String
        Public Property School() As String
            Get
                Return _school
            End Get
            Set(ByVal value As String)
                _school = value
            End Set
        End Property
    End Class

    Private Class PseudoCourseCaregory
        Private _requirementIsMet As String
        Public Property RequirementIsMet() As String
            Get
                Return _requirementIsMet
            End Get
            Set(ByVal value As String)
                _requirementIsMet = value
            End Set
        End Property

        Private _verificationDate As Nullable(Of DateTime)
        Public Property VerificationDate() As Nullable(Of DateTime)
            Get
                Return _verificationDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _verificationDate = value
            End Set
        End Property

        Private _verificationUserId As String
        Public Property VerificationUserId() As String
            Get
                Return _verificationUserId
            End Get
            Set(ByVal value As String)
                _verificationUserId = value
            End Set
        End Property
    End Class

    Private Class PseudoDocument
        Private _statusDate As Nullable(Of DateTime)
        Public Property StatusDate() As Nullable(Of DateTime)
            Get
                Return _statusDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
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
    End Class

    Private Class PseudoGrade
        Private _term As Integer
        Public Property Term() As Integer
            Get
                Return _term
            End Get
            Set(ByVal value As Integer)
                _term = value
            End Set
        End Property

        Private _letter As String
        Public Property Letter() As String
            Get
                Return _letter
            End Get
            Set(ByVal value As String)
                _letter = value
            End Set
        End Property
    End Class

    Private Class PseudoReview
        Private _completionDate As Nullable(Of DateTime)
        Public Property CompletionDate() As Nullable(Of Date)
            Get
                Return _completionDate
            End Get
            Set(ByVal value As Nullable(Of Date))
                _completionDate = value
            End Set
        End Property

        Private _reviewType As String
        Public Property ReviewType() As String
            Get
                Return _reviewType
            End Get
            Set(ByVal value As String)
                _reviewType = value
            End Set
        End Property

        Private _userId As String
        Public Property UserId() As String
            Get
                Return _userId
            End Get
            Set(ByVal value As String)
                _userId = value
            End Set
        End Property
    End Class
#End Region
End Class
