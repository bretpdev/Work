Imports System.IO
Imports System.Threading
Imports Q
Imports ReviewType = RegentsScholarshipBackEnd.Constants.ReviewType

Public Class ApplicationReviews
    Public Enum Phase
        Initial
        PendingFinal
    End Enum

    Private Shared ERROR_FILE As String = DataAccessBase.PersonalDataDirectory + "RegentsReviewErrors.txt"

    Public Shared Sub RunAllReviews()
        Try
            'Decrease the process priority so we don't bog down the system.
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal

            'Start with no error file present.
            If File.Exists(ERROR_FILE) Then
                File.Delete(ERROR_FILE)
            End If

            'figure out the application year to process for
            Dim currentProgramYear As Lookups.ProgramOperationYear = Lookups.ProgramYears.Where(Function(p) p.Year = Constants.CURRENT_AWARD_YEAR.ToString()).Single()

            'Run the initial reviews for each student in the database.
            RunClassReview(Phase.Initial, currentProgramYear.Year)
            RunCategoryReview(Phase.Initial, currentProgramYear.Year)
            MakeInitialAwardDecision(currentProgramYear)

            'Print the Quick Review Reports.
            Reports.InitialReview()

            'Run the final reviews for each student in the database.
            RunClassReview(Phase.PendingFinal, currentProgramYear.Year)
            RunCategoryReview(Phase.PendingFinal, currentProgramYear.Year)
            RunBaseAwardReview(currentProgramYear)
            RunExemplaryAwardReview(currentProgramYear.Year)

            'Create reports based on the full list of students.
            Reports.UespAwardReview()
            Reports.FinalReview()

            'Warn the user if an error file was written.
            If File.Exists(ERROR_FILE) Then
                Dim message As String = String.Format("Errors were encountered in processing. See the {0} file for details.", ERROR_FILE)
                Windows.Forms.MessageBox.Show(message, "Regents Reviews", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
            Else
                Windows.Forms.MessageBox.Show("All reviews completed.", "Regents Reviews", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.None)
            End If
        Catch ex As Exception
            Using ef As New ExceptionForm(ex)
                ef.ShowDialog()
            End Using
        Finally
            Thread.CurrentThread.Priority = ThreadPriority.Normal
        End Try
    End Sub

    Public Shared Sub RunPostFinalAwardReview(ByVal studentIds As List(Of String))
        Dim currentProgramYear As Lookups.ProgramOperationYear = Lookups.ProgramYears.OrderByDescending(Function(p) p.ApplicationDeadline).First()
        RunBaseAwardReview(currentProgramYear, studentIds)
        RunExemplaryAwardReview(currentProgramYear.Year, studentIds)
    End Sub

    Private Shared Sub RunClassReview(ByVal phase As Phase, ByVal applicationYear As String)
        Dim studentIds As List(Of String)
        If (phase = Phase.Initial) Then
            studentIds = DataAccess.GetStudentIdsForInitialClassReview(applicationYear)
        Else
            studentIds = DataAccess.GetStudentIdsForFinalClassReview(applicationYear)
        End If
        If (studentIds.Count = 0) Then Return

        Dim reviewName As String = If(phase = Phase.Initial, "Initial Class Review", "Final Class Review")
        Using progressForm As New StatusBar(studentIds.Count, reviewName)
            progressForm.Show()
            For Each studentId As String In studentIds
                'Update the progress bar.
                progressForm.updateStatBar(studentId)
                'Get the student from the database.
                Dim currentStudent As Student = Student.Load(studentId)
                'Review each class.
                For Each category As CourseCategory In currentStudent.HighSchool.CourseCategories.Values
                    For Each crs As Course In category.Courses
                        'Bypass this class if a user marked Yes or No for IsAcceptable.
                        If (crs.IsAcceptable = "Yes" OrElse crs.IsAcceptable = "No") _
                        AndAlso crs.Verification.UserId IsNot Nothing _
                        AndAlso crs.Verification.UserId.Length > 0 _
                        AndAlso crs.Verification.UserId <> "RSS" _
                        Then
                            Continue For
                        End If

                        Dim crsSchoolAttended As String = crs.SchoolAttended
                        Dim crsAcademicYearTaken As String = crs.AcademicYearTaken

                        Dim validTitles As List(Of String) = ( _
                            From ctl In Lookups.ClassTitles _
                            Where ctl.IsInApprovedList _
                            Where (ctl.ConditionalSchoolCode Is Nothing OrElse ctl.ConditionalSchoolCode.Trim() = "" OrElse ctl.ConditionalSchoolCode.Trim() = crsSchoolAttended) _
                            Where (ctl.ConditionalSchoolApprovalYears Is Nothing OrElse ctl.ConditionalSchoolApprovalYears.Trim() = "" OrElse ctl.ConditionalSchoolApprovalYears.Contains(crsAcademicYearTaken)) _
                            Select ctl.Title _
                        ).ToList()

                        'Mark the class as not acceptable if it's not in the approved list.
                        If (Not validTitles.Contains(crs.Title)) Then
                            crs.IsAcceptable = "No"
                            crs.Verification.UserId = Constants.SYSTEM_USER_ID
                            crs.Verification.TimeStamp = DateTime.Now
                            'Go on to the next class.
                            Continue For
                        End If

                        'Classes in the approved list need their grades checked.
                        'See if there are any P grades.
                        If (crs.Grades.Values.Select(Function(p) p.Letter).Contains("P")) Then
                            crs.IsAcceptable = "No"
                            crs.Verification.UserId = Constants.SYSTEM_USER_ID
                            crs.Verification.TimeStamp = DateTime.Now
                            'Add a denial reason.
                            currentStudent.ScholarshipApplication.DenialReasons.Add(Constants.DenialReason.MINIMUM_GRADE_REQUIREMENT_NOT_MET_DUE_TO_P_GRADE)
                            'Go on to the next class.
                            Continue For
                        End If
                        'See if all the grades are C (2.0) or better.
                        Dim allGradesAreCOrBetter As Boolean = True
                        For Each grd As Grade In crs.Grades.Values
                            If (grd.GpaValue < 2) Then
                                'See if the course weight brings the average above a C.
                                If (New String() {"AP", "CE"}.Contains(crs.Weight) AndAlso (crs.WeightedAverageGrade.GpaValue >= 2)) Then
                                    Continue For
                                Else
                                    allGradesAreCOrBetter = False
                                    Exit For
                                End If
                            End If
                        Next grd
                        If (allGradesAreCOrBetter) Then
                            'Check that the grade level is acceptable.
                            If (New String() {"9", "10", "11", "12"}.Contains(crs.GradeLevel)) Then
                                'Mark the class as acceptable and go on to the next class.
                                crs.IsAcceptable = "Yes"
                                crs.Verification.UserId = Constants.SYSTEM_USER_ID
                                crs.Verification.TimeStamp = Date.Now
                                Continue For
                            Else
                                'Check whether the grade level is "WC" or one of its equivalents.
                                If (crs.GradeLevel.ToLower().StartsWith("w")) Then
                                    'Mark the class as in progress and go on to the next class.
                                    crs.IsAcceptable = "In Progress"
                                    crs.Verification.UserId = Constants.SYSTEM_USER_ID
                                    crs.Verification.TimeStamp = Date.Now
                                    Continue For
                                Else
                                    'Mark the class as not acceptable and go on to the next class.
                                    crs.IsAcceptable = "No"
                                    crs.Verification.UserId = Constants.SYSTEM_USER_ID
                                    crs.Verification.TimeStamp = Date.Now
                                    Continue For
                                End If
                            End If
                        End If

                        'Grades don't measure up. Mark the class as acceptable,
                        'but add a denial reason if the class was not re-taken.
                        crs.IsAcceptable = "Yes"
                        crs.Verification.UserId = Constants.SYSTEM_USER_ID
                        crs.Verification.TimeStamp = Date.Now
                        Dim crsTitle As String = crs.Title
                        Dim duplicates As IEnumerable(Of Course) = category.Courses.Where(Function(p) p.Title = crsTitle)
                        If (duplicates.OrderBy(Function(p) Integer.Parse(p.AcademicYearTaken.Split("/").Last())).Last().AcademicYearTaken = crs.AcademicYearTaken) Then
                            currentStudent.ScholarshipApplication.DenialReasons.Add(Constants.DenialReason.MINIMUM_GRADE_REQUIREMENT_NOT_MET)
                        End If
                    Next crs
                    'After all courses in the category are reviewed, go back
                    'and look for courses that were taken more than once.
                    For Each courseTitle As String In category.Courses.Select(Function(p) p.Title).Distinct()
                        Dim title As String = courseTitle 'To appease the compiler gods.
                        Dim duplicates As IEnumerable(Of Course) = category.Courses.Where(Function(p) p.Title = title).OrderByDescending(Function(p) Integer.Parse(p.AcademicYearTaken.Split("/").Last()))
                        'Mark all but the highest grade level as not acceptable.
                        For i As Integer = 1 To duplicates.Count - 1
                            duplicates(i).IsAcceptable = "No"
                        Next i
                    Next courseTitle
                Next category

                'Mark this review as done and commit the student's changes.
                If (currentStudent.ScholarshipApplication.Reviews.ContainsKey(ReviewType.CLASS_)) Then
                    Dim classReview As Review = currentStudent.ScholarshipApplication.Reviews(ReviewType.CLASS_)
                    classReview.CompletionDate = Date.Now
                    classReview.UserId = Constants.SYSTEM_USER_ID
                Else
                    currentStudent.ScholarshipApplication.Reviews.Add(New Review(ReviewType.CLASS_, currentStudent.ScholarshipApplication) With {.CompletionDate = Date.Now, .UserId = Constants.SYSTEM_USER_ID})
                End If
                Try
                    currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Application)
                Catch ex As Exception
                    Using errorWriter As New StreamWriter(ERROR_FILE, True)
                        Dim message As String = String.Format("{0} for student ID {1}: {2}", reviewName, currentStudent.StateStudentId, ex.Message)
                        errorWriter.WriteLine(message)
                        errorWriter.Close()
                    End Using
                End Try
            Next studentId
            progressForm.Close()
        End Using
    End Sub

    Private Shared Sub RunCategoryReview(ByVal phase As Phase, ByVal applicationYear As String)
        Dim studentIds As List(Of String)
        If (phase = Phase.Initial) Then
            studentIds = DataAccess.GetStudentIdsForInitialCategoryReview(applicationYear)
        Else
            studentIds = DataAccess.GetStudentIdsForFinalCategoryReview(applicationYear)
        End If
        If (studentIds.Count = 0) Then Return

        Dim reviewName As String = If(phase = Phase.Initial, "Initial Category Review", "Final Category Review")
        Using progressForm As New StatusBar(studentIds.Count, reviewName)
            progressForm.Show()
            For Each studentId As String In studentIds
                'Update the progress bar.
                progressForm.updateStatBar(studentId)
                'Get the student from the database.
                Dim currentStudent As Student = Student.Load(studentId)
                'Go through each class category and check that enough credits are taken.
                Dim acceptableStatuses As New List(Of String)
                acceptableStatuses.Add("Yes")
                If (phase = Phase.Initial) Then
                    acceptableStatuses.Add("In Progress")
                End If
                For Each category As CourseCategory In currentStudent.HighSchool.CourseCategories.Values
                    'Bypass this category if a user marked Yes or No for IsAcceptable.
                    If (category.RequirementIsMet = "Yes" OrElse category.RequirementIsMet = "No") _
                    AndAlso (Not String.IsNullOrEmpty(category.Verification.UserId)) _
                    AndAlso category.Verification.UserId <> "RSS" _
                    Then
                        Continue For
                    End If

                    Dim requiredCredits As Double
                    Select Case category.Category
                        Case Constants.CourseCategory.ENGLISH, Constants.CourseCategory.MATHEMATICS
                            requiredCredits = 4
                        Case Constants.CourseCategory.SOCIAL_SCIENCE
                            requiredCredits = 3.5
                        Case Constants.CourseCategory.SCIENCE
                            requiredCredits = 3
                        Case Constants.CourseCategory.FOREIGN_LANGUAGE
                            requiredCredits = 2
                    End Select

                    Dim acceptableCourses As List(Of Course) = category.Courses.Where(Function(p) acceptableStatuses.Contains(p.IsAcceptable)).ToList()
                    If (acceptableCourses.Select(Function(p) p.Credits).Sum() >= requiredCredits) Then
                        'Check for in-progress courses during the initial review.
                        If (phase = phase.Initial) AndAlso (acceptableCourses.Select(Function(p) p.IsAcceptable).Contains("In Progress")) Then
                            category.RequirementIsMet = "In Progress"
                        Else
                            category.RequirementIsMet = "Yes"
                        End If
                    Else
                        category.RequirementIsMet = "No"
                    End If

                    'For the science category, check that the student has a full credit of physics, biology, and chemistry.
                    If (category.Category = Constants.CourseCategory.SCIENCE) Then
                        Dim physicsCredits As Double = acceptableCourses.Where(Function(p) p.Title.ToLower().Contains("physics") Or p.Title.ToLower().Contains("principles of technology")).Select(Function(p) p.Credits).Sum()
                        If (physicsCredits < 1) Then
                            category.RequirementIsMet = "No"
                            currentStudent.ScholarshipApplication.DenialReasons.Add(Constants.DenialReason.SCIENCE_PHYSICS_REQUIREMENT_NOT_MET)
                        End If
                        Dim biologyCredits As Double = acceptableCourses.Where(Function(p) p.Title.ToLower().Contains("biology")).Select(Function(p) p.Credits).Sum()
                        If (biologyCredits < 1) Then
                            category.RequirementIsMet = "No"
                            currentStudent.ScholarshipApplication.DenialReasons.Add(Constants.DenialReason.SCIENCE_BIOLOGY_REQUIREMENT_NOT_MET)
                        End If
                        Dim chemistryCredits As Double = acceptableCourses.Where(Function(p) p.Title.ToLower().Contains("chemistry")).Select(Function(p) p.Credits).Sum()
                        If (chemistryCredits < 1) Then
                            category.RequirementIsMet = "No"
                            currentStudent.ScholarshipApplication.DenialReasons.Add(Constants.DenialReason.SCIENCE_CHEMISTRY_REQUIREMENT_NOT_MET)
                        End If
                    End If

                    'For the foreign language category, check that there are enough credits in the same language.
                    If (category.Category = Constants.CourseCategory.FOREIGN_LANGUAGE) Then
                        For Each titleFirstWord As String In acceptableCourses.Select(Function(p) p.Title.Substring(0, p.Title.IndexOf(" "))).Distinct()
                            'Skip this title if the first word is IB.
                            If titleFirstWord.ToLower() = "ib" Then
                                Continue For
                            End If

                            'Get a list of the courses whose title contains the word we're after.
                            Dim languageIdentifier As String = titleFirstWord
                            Dim sameLanguageCourses As List(Of Course) = acceptableCourses.Where(Function(p) p.Title.Contains(languageIdentifier)).ToList()

                            'Nothing more needs to be done if all the courses are of the same language.
                            If sameLanguageCourses.Count = acceptableCourses.Count Then
                                Exit For
                            End If

                            'Do the same comparison as before, but break if the category is acceptable.
                            If (sameLanguageCourses.Select(Function(p) p.Credits).Sum() >= requiredCredits) Then
                                'Check for in-progress courses during the initial review.
                                If (phase = phase.Initial) AndAlso (sameLanguageCourses.Select(Function(p) p.IsAcceptable).Contains("In Progress")) Then
                                    category.RequirementIsMet = "In Progress"
                                Else
                                    category.RequirementIsMet = "Yes"
                                End If
                                Exit For
                            Else
                                category.RequirementIsMet = "No"
                            End If
                        Next titleFirstWord
                    End If

                    category.Verification.UserId = Constants.SYSTEM_USER_ID
                    category.Verification.TimeStamp = Date.Now
                Next category

                'Mark this review as done and commit the student's changes.
                If (currentStudent.ScholarshipApplication.Reviews.ContainsKey(ReviewType.CATEGORY)) Then
                    Dim categoryReview As Review = currentStudent.ScholarshipApplication.Reviews(ReviewType.CATEGORY)
                    categoryReview.CompletionDate = Date.Now
                    categoryReview.UserId = Constants.SYSTEM_USER_ID
                Else
                    currentStudent.ScholarshipApplication.Reviews.Add(New Review(ReviewType.CATEGORY, currentStudent.ScholarshipApplication) With {.CompletionDate = Date.Now, .UserId = Constants.SYSTEM_USER_ID})
                End If
                Try
                    currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Application)
                Catch ex As Exception
                    Using errorWriter As New StreamWriter(ERROR_FILE, True)
                        Dim message As String = String.Format("{0} for student ID {1}: {2}", reviewName, currentStudent.StateStudentId, ex.Message)
                        errorWriter.WriteLine(message)
                        errorWriter.Close()
                    End Using
                End Try
            Next studentId
            progressForm.Close()
        End Using
    End Sub

    Private Shared Sub MakeInitialAwardDecision(ByVal programYear As Lookups.ProgramOperationYear)
        Dim studentIds As List(Of String) = DataAccess.GetStudentIdsForInitialAwardDecision(programYear.Year)
        If (studentIds.Count = 0) Then Return

        Const reviewName As String = "Initial Award Decision"
        Using progressForm As New StatusBar(studentIds.Count, reviewName)
            progressForm.Show()
            For Each studentId As String In studentIds
                'Update the progress bar.
                progressForm.updateStatBar(studentId)
                'Get the student from the database.
                Dim currentStudent As Student = Student.Load(studentId)
                'Check the award criteria.
                Dim denialReasons As New List(Of String)()
                Dim applicationReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.APPLICATION).SingleOrDefault().Value
                If (Not applicationReceivedDate.HasValue) OrElse (applicationReceivedDate.Value = Date.MinValue) Then
                    denialReasons.Add(Constants.DenialReason.APPLICATION_NOT_RECEIVED)
                ElseIf (applicationReceivedDate.Value.Date > programYear.ApplicationDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.APPLICATION_RECEIVED_LATE)
                End If
                Dim highSchoolTranscriptReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.INITIAL_HIGH_SCHOOL_TRANSCRIPT).SingleOrDefault().Value
                If (Not highSchoolTranscriptReceivedDate.HasValue) OrElse (highSchoolTranscriptReceivedDate.Value = Date.MinValue) Then
                    denialReasons.Add(Constants.DenialReason.HIGH_SCHOOL_TRANSCRIPT_NOT_PROVIDED)
                ElseIf (highSchoolTranscriptReceivedDate.Value.Date > programYear.ApplicationDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.HIGH_SCHOOL_TRANSCRIPT_RECEIVED_LATE)
                End If
                Dim collegeTranscriptReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.INITIAL_COLLEGE_TRANSCRIPT).SingleOrDefault().Value
                'College transcript can be missing, so only check the date if it's there.
                If (collegeTranscriptReceivedDate.HasValue) AndAlso (collegeTranscriptReceivedDate.Value <> Date.MinValue) AndAlso (collegeTranscriptReceivedDate.Value.Date > programYear.ApplicationDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.COLLEGE_TRANSCRIPT_RECEIVED_LATE)
                End If
                'Check if High School Schedule has been received and that it wasn't late
                Dim highSchoolScheduleReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.HIGH_SCHOOL_SCHEDULE).SingleOrDefault().Value
                If (Not highSchoolScheduleReceivedDate.HasValue) OrElse (highSchoolScheduleReceivedDate.Value = Date.MinValue) Then
                    denialReasons.Add(Constants.DenialReason.COURSE_FORM_NOT_RECEIVED)
                ElseIf (highSchoolScheduleReceivedDate.Value.Date > programYear.ApplicationDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.COURSE_FORM_RECEIVED_LATE)
                End If
                If (currentStudent.HighSchool.CumulativeGpa < 3) Then
                    denialReasons.Add(Constants.DenialReason.CUMULATIVE_GPA_REQUIREMENT_NOT_MET)
                End If
                If (currentStudent.HighSchool.ActScores.Where(Function(p) p.Key = Constants.ActCategory.COMPOSITE).SingleOrDefault().Value = 0) Then
                    denialReasons.Add(Constants.DenialReason.ACT_SCORE_NOT_REPORTED)
                End If
                If (Not currentStudent.IsUsCitizen And Not currentStudent.IsEligibleForFederalAid) Then
                    denialReasons.Add(Constants.DenialReason.NOT_A_US_CITIZEN)
                End If
                If (currentStudent.HasCriminalRecord) Then
                    denialReasons.Add(Constants.DenialReason.CRIMINAL_RECORD_EXISTS)
                End If

                Dim acceptableStatuses As New List(Of String)
                acceptableStatuses.Add("Yes")
                acceptableStatuses.Add("In Progress")
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.ENGLISH).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.ENGLISH_REQUIREMENT_NOT_MET)
                End If
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.MATHEMATICS).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.MATH_REQUIREMENT_NOT_MET)
                End If
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.SOCIAL_SCIENCE).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.SOCIAL_SCIENCE_REQUIREMENT_NOT_MET)
                End If
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.SCIENCE).RequirementIsMet)) Then
                    Dim scienceDenialReasonExists As Boolean = False
                    For Each reason As String In currentStudent.ScholarshipApplication.DenialReasons
                        If (reason.ToLower().StartsWith("science")) Then
                            scienceDenialReasonExists = True
                            Exit For
                        End If
                    Next reason
                    If (Not scienceDenialReasonExists) Then denialReasons.Add(Constants.DenialReason.SCIENCE_REQUIREMENT_NOT_MET)
                End If
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.FOREIGN_LANGUAGE).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.FOREIGN_LANGUAGE_REQUIREMENT_NOT_MET)
                End If

                'Add denials from this review to any pre-existing denials.
                currentStudent.ScholarshipApplication.DenialReasons.UnionWith(denialReasons)

                'Update the award status.
                If (currentStudent.ScholarshipApplication.DenialReasons.Count = 0) Then
                    currentStudent.ScholarshipApplication.BaseAward.Status = Constants.AwardStatus.CONDITIONAL_APPROVAL
                Else
                    currentStudent.ScholarshipApplication.BaseAward.Status = Constants.AwardStatus.DENIED
                End If
                currentStudent.ScholarshipApplication.BaseAward.StatusDate = Date.Now
                currentStudent.ScholarshipApplication.BaseAward.StatusUserId = Constants.SYSTEM_USER_ID

                'Mark this review as done and commit the student's changes.
                If (currentStudent.ScholarshipApplication.Reviews.ContainsKey(ReviewType.INITIAL_AWARD)) Then
                    Dim initialAwardReview As Review = currentStudent.ScholarshipApplication.Reviews(ReviewType.INITIAL_AWARD)
                    initialAwardReview.CompletionDate = Date.Now
                    initialAwardReview.UserId = Constants.SYSTEM_USER_ID
                Else
                    currentStudent.ScholarshipApplication.Reviews.Add(New Review(ReviewType.INITIAL_AWARD, currentStudent.ScholarshipApplication) With {.CompletionDate = Date.Now, .UserId = Constants.SYSTEM_USER_ID})
                End If
                'create Quick review record without a completion date to show that the 1st quick review is in progress
                currentStudent.ScholarshipApplication.Reviews.Add(New Review(ReviewType.FIRST_QUICK, currentStudent.ScholarshipApplication))
                Try
                    currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Application)
                Catch ex As Exception
                    Using errorWriter As New StreamWriter(ERROR_FILE, True)
                        Dim message As String = String.Format("{0} for student ID {1}: {2}", reviewName, currentStudent.StateStudentId, ex.Message)
                        errorWriter.WriteLine(message)
                        errorWriter.Close()
                    End Using
                End Try
            Next studentId
            progressForm.Close()
        End Using
    End Sub

    Private Shared Sub RunBaseAwardReview(ByVal programYear As Lookups.ProgramOperationYear)
        RunBaseAwardReview(programYear, Nothing)
    End Sub

    Private Shared Sub RunBaseAwardReview(ByVal programYear As Lookups.ProgramOperationYear, ByVal studentIds As List(Of String))
        Const REVIEW_NAME As String = "Base Award Review"
        Dim approvalStatus As String = Constants.AwardStatus.PENDING_APPROVAL
        Dim denialStatus As String = Constants.AwardStatus.PENDING_DENIAL
        Dim secondQuickReviewIsDone As Boolean = False

        If (studentIds Is Nothing) Then
            'Normal batch review doesn't use pre-selected student IDs, so look up the appropriate ones from the database.
            studentIds = DataAccess.GetStudentIdsForPendingAwardReview(programYear.Year)
        Else
            'Passed-in student IDs come from the Batch Quick Review, which means the second quick review is done
            'and we need to apply the final award status and create payments;
            approvalStatus = Constants.AwardStatus.APPROVED
            denialStatus = Constants.AwardStatus.DENIED
            secondQuickReviewIsDone = True
        End If
        If (studentIds.Count = 0) Then Return

        Using progressForm As New StatusBar(studentIds.Count, REVIEW_NAME)
            progressForm.Show()
            For Each studentId As String In studentIds
                'Update the progress bar.
                progressForm.updateStatBar(studentId)
                'Get the student from the database.
                Dim currentStudent As Student = Student.Load(studentId)

                'Check that the criteria are met.
                Dim denialReasons As New List(Of String)()
                Dim applicationReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.APPLICATION).SingleOrDefault().Value
                If (Not applicationReceivedDate.HasValue) OrElse (applicationReceivedDate.Value = Date.MinValue) Then
                    denialReasons.Add(Constants.DenialReason.APPLICATION_NOT_RECEIVED)
                ElseIf (applicationReceivedDate.Value.Date > programYear.ApplicationDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.APPLICATION_RECEIVED_LATE)
                End If
                Dim highSchoolTranscriptReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.FINAL_HIGH_SCHOOL_TRANSCRIPT).SingleOrDefault().Value
                If (Not highSchoolTranscriptReceivedDate.HasValue) OrElse (highSchoolTranscriptReceivedDate.Value = Date.MinValue) Then
                    denialReasons.Add(Constants.DenialReason.HIGH_SCHOOL_TRANSCRIPT_NOT_PROVIDED)
                ElseIf (highSchoolTranscriptReceivedDate.Value.Date > programYear.FinalDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.HIGH_SCHOOL_TRANSCRIPT_RECEIVED_LATE)
                End If
                Dim hasConcurrentEnrollmentCourses As Boolean = False
                For Each category As CourseCategory In currentStudent.HighSchool.CourseCategories.Values
                    For Each crs As Course In category.Courses
                        If (crs.Weight = Constants.ClassWeight.CE) Then
                            hasConcurrentEnrollmentCourses = True
                            Exit For
                        End If
                    Next crs
                    If (hasConcurrentEnrollmentCourses) Then Exit For
                Next category
                Dim collegeTranscriptReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.FINAL_COLLEGE_TRANSCRIPT).SingleOrDefault().Value
                If (hasConcurrentEnrollmentCourses AndAlso (Not collegeTranscriptReceivedDate.HasValue)) Then
                    denialReasons.Add(Constants.DenialReason.COLLEGE_TRANSCRIPT_NOT_SUBMITTED)
                End If
                'College transcript can be missing, so only check the date if it's there.
                If (collegeTranscriptReceivedDate.HasValue) AndAlso (collegeTranscriptReceivedDate.Value <> Date.MinValue) AndAlso (collegeTranscriptReceivedDate.Value.Date > programYear.FinalDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.COLLEGE_TRANSCRIPT_RECEIVED_LATE)
                End If
                Dim proofOfCitizenshipReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.PROOF_OF_CITIZENSHIP).SingleOrDefault().Value
                If (Not proofOfCitizenshipReceivedDate.HasValue) OrElse (proofOfCitizenshipReceivedDate.Value = Date.MinValue) Then
                    denialReasons.Add(Constants.DenialReason.PROOF_OF_CITIZENSHIP_NOT_RECEIVED)
                ElseIf (proofOfCitizenshipReceivedDate.Value.Date > programYear.FinalDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.PROOF_OF_CITIZENSHIP_RECEIVED_LATE)
                End If
                Dim preliminaryAcceptanceFormReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.CONDITIONAL_ACCEPTANCE_FORM).SingleOrDefault().Value
                If (Not preliminaryAcceptanceFormReceivedDate.HasValue) OrElse (preliminaryAcceptanceFormReceivedDate.Value = Date.MinValue) Then
                    denialReasons.Add(Constants.DenialReason.CONDITIONAL_ACCEPTANCE_FORM_NOT_RECEIVED)
                ElseIf (preliminaryAcceptanceFormReceivedDate.Value.Date > programYear.FinalDeadline.Date) Then
                    denialReasons.Add(Constants.DenialReason.CONDITIONAL_ACCEPTANCE_FORM_RECEIVED_LATE)
                End If
                If currentStudent.ScholarshipApplication.BaseAward.Status <> Constants.AwardStatus.DEFERRED Then
                    Dim proofOfRegistrationReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.PROOF_OF_ENROLLMENT).SingleOrDefault().Value
                    If ((Not proofOfRegistrationReceivedDate.HasValue) OrElse (proofOfRegistrationReceivedDate.Value = Date.MinValue)) Then
                        denialReasons.Add(Constants.DenialReason.PROOF_OF_REGISTRATION_NOT_RECEIVED)
                    ElseIf (proofOfRegistrationReceivedDate.Value.Date > programYear.FinalDeadline.Date) Then
                        denialReasons.Add(Constants.DenialReason.PROOF_OF_REGISTRATION_RECEIVED_LATE)
                    End If
                End If
                If ((currentStudent.College.NumberOfEnrolledCredits < 12)) And (currentStudent.ScholarshipApplication.BaseAward.Status <> Constants.AwardStatus.DEFERRED) Then
                    denialReasons.Add(Constants.DenialReason.MINIMUM_NUMBER_OF_ENROLLED_COLLEGE_CREDITS_NOT_MET)
                End If
                Dim validColleges As List(Of String) = Lookups.Colleges.Where(Function(p) p.IsInDefaultList = True).Select(Function(p) p.Name).ToList()
                If ((Not validColleges.Contains(currentStudent.College.Name))) And (currentStudent.ScholarshipApplication.BaseAward.Status <> Constants.AwardStatus.DEFERRED) Then
                    denialReasons.Add(Constants.DenialReason.NOT_ENROLLED_AT_AN_ELIGIBLE_UTAH_INSTITUTION_OF_HIGHER_EDUCATION)
                End If
                If ((Not currentStudent.College.TermBeginDate.HasValue) OrElse (Not currentStudent.HighSchool.GraduationDate.HasValue) _
                OrElse (currentStudent.College.TermBeginDate.Value > currentStudent.HighSchool.GraduationDate.Value.AddMonths(12))) _
                And (currentStudent.ScholarshipApplication.BaseAward.Status <> Constants.AwardStatus.DEFERRED) Then
                    denialReasons.Add(Constants.DenialReason.NOT_ENROLLED_FALL_SEMESTER_IMMEDIATELY_AFTER_GRADUATION)
                End If
                If (currentStudent.HighSchool.CumulativeGpa < 3) Then
                    denialReasons.Add(Constants.DenialReason.CUMULATIVE_GPA_REQUIREMENT_NOT_MET)
                End If
                If (currentStudent.HighSchool.ActScores.Where(Function(p) p.Key = Constants.ActCategory.COMPOSITE).SingleOrDefault().Value = 0) Then
                    denialReasons.Add(Constants.DenialReason.ACT_SCORE_NOT_REPORTED)
                End If
                If (Not currentStudent.IsUsCitizen And Not currentStudent.IsEligibleForFederalAid) Then
                    denialReasons.Add(Constants.DenialReason.NOT_A_US_CITIZEN)
                End If
                If (currentStudent.HasCriminalRecord) Then
                    denialReasons.Add(Constants.DenialReason.CRIMINAL_RECORD_EXISTS)
                End If

                Dim acceptableStatuses As New List(Of String)
                acceptableStatuses.Add("Yes")
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.ENGLISH).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.ENGLISH_REQUIREMENT_NOT_MET)
                End If
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.MATHEMATICS).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.MATH_REQUIREMENT_NOT_MET)
                End If
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.SOCIAL_SCIENCE).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.SOCIAL_SCIENCE_REQUIREMENT_NOT_MET)
                End If
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.SCIENCE).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.SCIENCE_REQUIREMENT_NOT_MET)
                End If
                If (Not acceptableStatuses.Contains(currentStudent.HighSchool.CourseCategories(Constants.CourseCategory.FOREIGN_LANGUAGE).RequirementIsMet)) Then
                    denialReasons.Add(Constants.DenialReason.FOREIGN_LANGUAGE_REQUIREMENT_NOT_MET)
                End If

                'Add denials from this review to any pre-existing denials.
                currentStudent.ScholarshipApplication.DenialReasons.UnionWith(denialReasons)

                'Update the award status.
                If (currentStudent.ScholarshipApplication.DenialReasons.Count = 0) Then
                    If (Not currentStudent.ScholarshipApplication.BaseAward.Status = Constants.AwardStatus.DEFERRED) Then
                        currentStudent.ScholarshipApplication.BaseAward.Status = approvalStatus
                    End If
                    Dim priorityDeadlineWasMet As Boolean = True

                    If (applicationReceivedDate.Value.Date > programYear.PriorityDeadline.Date) Then
                        priorityDeadlineWasMet = False
                    End If
                    Dim initialHighSchoolTranscriptReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.INITIAL_HIGH_SCHOOL_TRANSCRIPT).SingleOrDefault().Value
                    If (initialHighSchoolTranscriptReceivedDate.Value.Date > programYear.PriorityDeadline.Date) Then
                        priorityDeadlineWasMet = False
                    End If
                    Dim initialCollegeTranscriptReceivedDate As Nullable(Of Date) = currentStudent.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = Constants.DocumentType.INITIAL_COLLEGE_TRANSCRIPT).SingleOrDefault().Value
                    'College transcript can be missing, so only check the date if it's there.
                    If (initialCollegeTranscriptReceivedDate.HasValue) AndAlso (initialCollegeTranscriptReceivedDate.Value <> Date.MinValue) AndAlso (initialCollegeTranscriptReceivedDate.Value.Date > programYear.PriorityDeadline.Date) Then
                        priorityDeadlineWasMet = False
                    End If

                    If priorityDeadlineWasMet Then
                        currentStudent.ScholarshipApplication.BaseAward.Description = Constants.AwardType.BASE_AWARD_PRIORITY_DEADLINE_MET
                    Else
                        currentStudent.ScholarshipApplication.BaseAward.Description = Constants.AwardType.BASE_AWARD_PRIORITY_DEADLINE_NOT_MET
                    End If
                Else
                    currentStudent.ScholarshipApplication.BaseAward.Status = denialStatus
                End If
                currentStudent.ScholarshipApplication.BaseAward.StatusDate = Date.Now
                currentStudent.ScholarshipApplication.BaseAward.StatusUserId = Constants.SYSTEM_USER_ID

                'Mark this review as done.
                If (currentStudent.ScholarshipApplication.Reviews.ContainsKey(ReviewType.BASE_AWARD)) Then
                    Dim existingReview As Review = currentStudent.ScholarshipApplication.Reviews(ReviewType.BASE_AWARD)
                    existingReview.CompletionDate = Date.Now
                    existingReview.UserId = Constants.SYSTEM_USER_ID
                Else
                    currentStudent.ScholarshipApplication.Reviews.Add(New Review(ReviewType.BASE_AWARD, currentStudent.ScholarshipApplication) With {.CompletionDate = Date.Now, .UserId = Constants.SYSTEM_USER_ID})
                End If
                If (Not secondQuickReviewIsDone) Then
                    'Make a 2nd quick review that is in progress.
                    currentStudent.ScholarshipApplication.Reviews.Add(New Review(ReviewType.SECOND_QUICK, currentStudent.ScholarshipApplication))
                End If
                'Commit the changes.
                Try
                    currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Application)
                    If (secondQuickReviewIsDone) Then
                        'Add a base award payment if merited.
                        Dim approvedStatuses() As String = {Constants.AwardStatus.APPROVED, Constants.AwardStatus.DEFERRED}
                        If (approvedStatuses.Contains(currentStudent.ScholarshipApplication.BaseAward.Status)) Then
                            currentStudent.Payments.Add(New Payment(currentStudent, Payment.AwardType.Base))
                            currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Payments)
                        End If
                    End If
                Catch ex As Exception
                    Using errorWriter As New StreamWriter(ERROR_FILE, True)
                        Dim message As String = String.Format("{0} for student ID {1}: {2}", REVIEW_NAME, currentStudent.StateStudentId, ex.Message)
                        errorWriter.WriteLine(message)
                        errorWriter.Close()
                    End Using
                End Try
            Next studentId
            progressForm.Close()
        End Using
    End Sub

    Private Shared Sub RunExemplaryAwardReview(ByVal applicationYear As String)
        RunExemplaryAwardReview(applicationYear, Nothing)
    End Sub

    Private Shared Sub RunExemplaryAwardReview(ByVal applicationYear As String, ByVal studentIds As List(Of String))
        Const REVIEW_NAME As String = "Exemplary Award Review"

        Dim secondQuickReviewIsDone As Boolean = False
        If (studentIds Is Nothing) Then
            studentIds = DataAccess.GetStudentIdsForExemplaryAwardReview(applicationYear)
        Else
            secondQuickReviewIsDone = True
        End If
        If (studentIds.Count = 0) Then Return

        Using progressForm As New StatusBar(studentIds.Count, REVIEW_NAME)
            progressForm.Show()
            For Each studentId As String In studentIds
                'Update the progress bar.
                progressForm.updateStatBar(studentId)
                'Get the student from the database.
                Dim currentStudent As Student = Student.Load(studentId)
                'Check the award criteria.
                Dim criteriaAreMet As Boolean = True
                If (currentStudent.ScholarshipApplication.BaseAward.Status <> Constants.AwardStatus.APPROVED AndAlso currentStudent.ScholarshipApplication.BaseAward.Status <> Constants.AwardStatus.PENDING_APPROVAL) Then
                    criteriaAreMet = False
                End If
                If (currentStudent.HighSchool.CumulativeGpa < 3.5) Then
                    criteriaAreMet = False
                End If
                If (currentStudent.HighSchool.ActScores.Where(Function(p) p.Key = Constants.ActCategory.COMPOSITE).SingleOrDefault().Value < 26) Then
                    criteriaAreMet = False
                End If
                For Each category As CourseCategory In currentStudent.HighSchool.CourseCategories.Values
                    For Each acceptableCourse As Course In category.Courses.Where(Function(p) p.IsAcceptable = "Yes")
                        For Each grd As Grade In acceptableCourse.Grades.Values
                            If (grd.GpaValue < 3) Then
                                'See if the course weight brings the average above a B.
                                If ((acceptableCourse.Weight <> "") AndAlso (acceptableCourse.WeightedAverageGrade.GpaValue >= 3)) Then
                                    Continue For
                                Else
                                    criteriaAreMet = False
                                    Exit For
                                End If
                            End If
                        Next grd
                    Next acceptableCourse
                Next category

                'Update the Exemplary Award status.
                If (criteriaAreMet) Then
                    currentStudent.ScholarshipApplication.ExemplaryAward.IsApproved = True
                Else
                    currentStudent.ScholarshipApplication.ExemplaryAward.IsApproved = False
                End If

                'Mark this review as done and commit the changes.
                If (currentStudent.ScholarshipApplication.Reviews.ContainsKey(ReviewType.EXEMPLARY_AWARD)) Then
                    Dim exemplaryAwardReview As Review = currentStudent.ScholarshipApplication.Reviews(ReviewType.EXEMPLARY_AWARD)
                    exemplaryAwardReview.CompletionDate = Date.Now
                    exemplaryAwardReview.UserId = Constants.SYSTEM_USER_ID
                Else
                    currentStudent.ScholarshipApplication.Reviews.Add(New Review(ReviewType.EXEMPLARY_AWARD, currentStudent.ScholarshipApplication) With {.CompletionDate = Date.Now, .UserId = Constants.SYSTEM_USER_ID})
                End If
                Try
                    currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Application)
                    'Add an exemplary award payment if merited.
                    If (secondQuickReviewIsDone AndAlso currentStudent.ScholarshipApplication.ExemplaryAward.IsApproved) Then
                        currentStudent.Payments.Add(New Payment(currentStudent, Payment.AwardType.Exemplary))
                        currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Payments)
                    End If
                Catch ex As Exception
                    Using errorWriter As New StreamWriter(ERROR_FILE, True)
                        Dim message As String = String.Format("{0} for student ID {1}: {2}", REVIEW_NAME, currentStudent.StateStudentId, ex.Message)
                        errorWriter.WriteLine(message)
                        errorWriter.Close()
                    End Using
                End Try
            Next studentId
            progressForm.Close()
        End Using
    End Sub
End Class
