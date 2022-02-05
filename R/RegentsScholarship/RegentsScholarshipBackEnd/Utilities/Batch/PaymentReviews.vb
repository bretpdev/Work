Imports System.Threading
Imports ReviewType = RegentsScholarshipBackEnd.Constants.ReviewType

Public Class PaymentReviews
    Public Event LogMessage As EventHandler(Of LogMessageEventArgs)

    Private Enum ReviewPhase
        Final
        Preliminary
    End Enum

    Private Const MIN_SEMSTER_CREDIT_HOURS As Double = 12.0
    Private Const MIN_GPA As Double = 3.0
    Private Shared PAYABLE_AWARD_STATUSES() As String = {Constants.AwardStatus.APPROVED, Constants.AwardStatus.PROBATION}

    ''' <summary>
    ''' Performs a final review of payments in a preliminary status, and generates associated reports and letters.
    ''' </summary>
    ''' <param name="batchNumber">The batch number to be applied to approved payments.</param>
    Public Sub RunFinalReview(ByVal userId As String, ByVal batchNumber As String)
        RaiseEvent LogMessage(Me, New LogMessageEventArgs(String.Format("Starting final payment review for batch {0}", batchNumber)))
        ReviewPayments(ReviewPhase.Final, batchNumber)

        'Print batch report.
        RaiseEvent LogMessage(Me, New LogMessageEventArgs(""))
        RaiseEvent LogMessage(Me, New LogMessageEventArgs("Creating the batch report"))
        Reports.FinalPayments(batchNumber)

        'print approved payment letters.
        RaiseEvent LogMessage(Me, New LogMessageEventArgs(""))
        RaiseEvent LogMessage(Me, New LogMessageEventArgs("Sending approval letters to centralized printing"))
        PaymentLetters.PrintLetters(userId, batchNumber)

        RaiseEvent LogMessage(Me, New LogMessageEventArgs(""))
        RaiseEvent LogMessage(Me, New LogMessageEventArgs("Finished"))
    End Sub

    ''' <summary>
    ''' Performs a preliminary review of payments in a pending or preliminary status, and generates associated reports.
    ''' </summary>
    Public Sub RunPreliminaryReview()
        RaiseEvent LogMessage(Me, New LogMessageEventArgs("Starting preliminary payment review"))
        ReviewPayments(ReviewPhase.Preliminary, "")

        'Print preliminary report.
        RaiseEvent LogMessage(Me, New LogMessageEventArgs(""))
        RaiseEvent LogMessage(Me, New LogMessageEventArgs("Creating the preliminary report"))
        Reports.PrelimPayments()

        RaiseEvent LogMessage(Me, New LogMessageEventArgs(""))
        RaiseEvent LogMessage(Me, New LogMessageEventArgs("Finished"))
    End Sub

    Private Function GpasAreSatisfactory(ByVal currentPayment As Payment, ByVal allPayments As List(Of Payment)) As Boolean
        If (currentPayment.GpaRequirementIsOverridden) Then
            Return True
        Else
            'Only check exemplary payments that have a GPA.
            Dim lowGpaPayments As IEnumerable(Of Payment) = allPayments.Where(Function(p) p.Type = Constants.PaymentType.EXEMPLARY AndAlso p.Gpa.HasValue AndAlso p.Gpa.Value < MIN_GPA)
            'More than one GPA below the minimum standard is not satisfactory.
            Return lowGpaPayments.Count <= 1
        End If
    End Function

    Private Function PreviousAttendedSemesterHasGpa(ByVal currentPayment As Payment, ByVal allPayments As List(Of Payment), ByVal applicationYear As Integer) As Boolean
        'Go back through past semesters until we find one where a payment was made.
        Dim previousSemesterYear As Payment.SemesterYear = Payment.GetPreviousSemesterYear(currentPayment.Semester, currentPayment.Year, currentPayment.College)
        Dim previousSemesterPayment As Payment = allPayments.Where(Function(p) p.Semester = previousSemesterYear.Semester AndAlso p.Year = previousSemesterYear.Year AndAlso p.Type = Constants.PaymentType.EXEMPLARY).OrderBy(Function(p) p.SequenceNo).LastOrDefault()
        Do While previousSemesterPayment Is Nothing AndAlso previousSemesterYear.Year >= applicationYear
            previousSemesterYear = Payment.GetPreviousSemesterYear(previousSemesterYear.Semester, previousSemesterYear.Year, currentPayment.College)
            previousSemesterPayment = allPayments.Where(Function(p) p.Semester = previousSemesterYear.Semester AndAlso p.Year = previousSemesterYear.Year AndAlso p.Type = Constants.PaymentType.EXEMPLARY).OrderBy(Function(p) p.SequenceNo).LastOrDefault()
        Loop
        'We're okay under any of three circumstances:
        '1-There was no payment for a previous semester.
        '2-The previous semester has a non-zero GPA.
        '3-The previous semester is the first one we paid out, i.e., it includes a base award payment.
        If (previousSemesterPayment Is Nothing) Then
            Return True
        ElseIf (previousSemesterPayment.Gpa > 0) Then
            Return True
        Else
            Dim previousSemesterPayments As List(Of Payment) = allPayments.Where(Function(p) p.Semester = previousSemesterPayment.Semester AndAlso p.Year = previousSemesterPayment.Year).ToList()
            If (previousSemesterPayments.Select(Function(p) p.Type).Contains(Constants.PaymentType.BASE)) Then
                Return True
            End If
        End If
        Return False
    End Function

    Private Sub ReviewPayments(ByVal phase As ReviewPhase, ByVal batchNumber As String)
        'Declare some variables that are contingent on the review phase, and set them up appropriately.
        Dim reviewApproved As String
        Dim reviewDenied As String
        Dim reviewableStatuses As New List(Of String)()
        reviewableStatuses.Add(Constants.PaymentStatus.PRELIM_APPROVAL)
        reviewableStatuses.Add(Constants.PaymentStatus.PRELIM_DENIED)
        Dim studentIds As New List(Of String)()

        If (phase = ReviewPhase.Preliminary) Then
            reviewApproved = Constants.PaymentStatus.PRELIM_APPROVAL
            reviewDenied = Constants.PaymentStatus.PRELIM_DENIED
            reviewableStatuses.Add(Constants.PaymentStatus.PENDING)
            studentIds = DataAccess.GetStudentIdsForPreliminaryPaymentReview()
        Else
            reviewApproved = Constants.PaymentStatus.APPROVED
            reviewDenied = Constants.PaymentStatus.DENIED
            studentIds = DataAccess.GetStudentIdsForFinalPaymentReview()
        End If

        'Review the payments.
        For Each studentId As String In studentIds
            Dim currentStudent As Student = Student.Load(studentId)
            RaiseEvent LogMessage(Me, New LogMessageEventArgs(""))
            RaiseEvent LogMessage(Me, New LogMessageEventArgs(String.Format("Reviewing {0} {1}, SSID {2}", currentStudent.FirstName, currentStudent.LastName, currentStudent.StateStudentId())))
            For Each currentPayment As Payment In currentStudent.Payments.Where(Function(p) reviewableStatuses.Contains(p.Status))
                'Make sure the student has an SSN.
                If (String.IsNullOrEmpty(currentStudent.SocialSecurityNumber)) Then
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Missing SSN"))
                    Continue For
                End If

                'Make sure there aren't multiple payments for the same semester, unless overridden.
                If (Not SemesterIsUnique(currentPayment, currentStudent.Payments)) Then
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Multiple payments for the same semester"))
                    Continue For
                End If

                'Payments should not be sent out until all reviews are done, so that we don't end up
                'creating more payment types later and having to send another check to the school.
                Dim reviews As ReviewDictionary = currentStudent.ScholarshipApplication.Reviews
                If Not (reviews.ContainsKey(ReviewType.SECOND_QUICK) AndAlso reviews(ReviewType.SECOND_QUICK).CompletionDate.HasValue) Then
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Second quick review not done"))
                    Continue For
                End If
                If Not (reviews.ContainsKey(ReviewType.EXEMPLARY_AWARD) AndAlso reviews(ReviewType.EXEMPLARY_AWARD).CompletionDate.HasValue) Then
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Exemplary award review not done"))
                    Continue For
                End If
                If Not (reviews.ContainsKey(ReviewType.UESP_AWARD) AndAlso reviews(ReviewType.UESP_AWARD).CompletionDate.HasValue) Then
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("UESP award review not done"))
                    Continue For
                End If

                'Keep a list of denial reasons for use at the end of the review.
                Dim denialReasons As New List(Of String)()

                'Check the total credit hours for the semester.
                If (Not SemesterCreditsAreSatisfactory(currentPayment, currentStudent.Payments)) Then
                    currentPayment.Status = reviewDenied
                    denialReasons.Add("Credit hours")
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Denial for credit hours"))
                End If

                'Check for multiple low GPAs.
                If (Not GpasAreSatisfactory(currentPayment, currentStudent.Payments)) Then
                    currentPayment.Status = reviewDenied
                    denialReasons.Add("GPA")
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Denial for GPA"))
                End If

                'Check for NCSP award.
                If (DataAccess.StudentHasNewCenturyAward(currentStudent)) Then
                    currentPayment.Status = reviewDenied
                    denialReasons.Add("New Century")
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Denial for New Century"))
                End If

                'Check that there's a GPA for the previous semester attended.
                If (Not PreviousAttendedSemesterHasGpa(currentPayment, currentStudent.Payments, Integer.Parse(currentStudent.ScholarshipApplication.ApplicationYear))) Then
                    currentPayment.Status = reviewDenied
                    denialReasons.Add("Missing GPA")
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Denial for missing GPA"))
                End If

                'Check that we don't have too many credits.
                Dim creditHoursApproved As Double = currentStudent.CumulativeCreditHoursPaid()
                If (phase = ReviewPhase.Preliminary) Then
                    creditHoursApproved += currentStudent.Payments.Where(Function(p) p.Type = Constants.PaymentType.EXEMPLARY AndAlso p.Status = Constants.PaymentStatus.PRELIM_APPROVAL).Select(Function(p) p.Credits).Sum()
                End If
                If (creditHoursApproved + currentPayment.Credits > Constants.MAX_CREDIT_HOURS_PAYABLE) Then
                    currentPayment.Status = reviewDenied
                    denialReasons.Add("Cumulative credits")
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Denial for cumulative credits"))
                End If

                'Check that the scholarship was awarded.
                If (Not PAYABLE_AWARD_STATUSES.Contains(currentStudent.ScholarshipApplication.BaseAward.Status)) Then
                    currentPayment.Status = reviewDenied
                    denialReasons.Add("Award status")
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs("Denial for award status"))
                End If

                'Set the batch number if this is the final review.
                If (phase = ReviewPhase.Final) Then
                    currentPayment.BatchNumber = batchNumber
                End If

                'Approve the payment, or set the full string of denial reasons if not approved.
                If (denialReasons.Count > 0) Then
                    currentPayment.DenialReasons = String.Join(", ", denialReasons.ToArray())
                Else
                    currentPayment.Status = reviewApproved
                    RaiseEvent LogMessage(Me, New LogMessageEventArgs(currentPayment.Status))
                End If
            Next currentPayment

            'Commit all payment changes for this student.
            currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Payments)
        Next studentId
    End Sub

    Private Function SemesterCreditsAreSatisfactory(ByVal currentPayment As Payment, ByVal allPayments As List(Of Payment)) As Boolean
        'Create a copy of currentPayment that can be used in lambda expressions.
        Dim pmt As Payment = currentPayment

        If (currentPayment.CreditsRequirementIsOverridden) Then
            Return True
        Else
            'Count up the credits from Exemplary payments for this semester.
            Dim semesterPayments As List(Of Payment) = allPayments.Where(Function(p) p.Type = Constants.PaymentType.EXEMPLARY AndAlso p.Year = pmt.Year AndAlso p.Semester = pmt.Semester AndAlso (p.Status = Constants.PaymentStatus.APPROVED OrElse p.Status = Constants.PaymentStatus.PENDING)).ToList()
            'If there are no Exemplary payments for this semester, then just look at the credits for this payment.
            If (semesterPayments.Count = 0) Then
                semesterPayments = New List(Of Payment)()
                semesterPayments.Add(currentPayment)
            End If
            Dim totalCredits As Double = semesterPayments.Select(Function(p) p.Credits).Sum()
            Return totalCredits >= MIN_SEMSTER_CREDIT_HOURS
        End If
    End Function

    Private Function SemesterIsUnique(ByVal currentPayment As Payment, ByVal allPayments As List(Of Payment)) As Boolean
        'Create a copy of currentPayment that can be used in lambda expressions.
        Dim pmt As Payment = currentPayment

        'Get all exemplary payments with the same semester/year as the current payment.
        Dim semesterPayments As List(Of Payment) = allPayments.Where(Function(p) p.Type = Constants.PaymentType.EXEMPLARY AndAlso p.Year = pmt.Year AndAlso p.Semester = pmt.Semester).ToList()
        'We're good if there are fewer than two exemplary payments for the semester, or if any of them have the semester uniqueness override.
        If (semesterPayments.Count < 2) Then Return True
        If (semesterPayments.Where(Function(p) pmt.SemesterUniquenessIsOverridden).Count > 0) Then Return True
        Return False
    End Function

    ''' <summary>
    ''' EventArgs class for MessageReceived events
    ''' </summary>
    Public Class LogMessageEventArgs
        Inherits EventArgs

        Private _message As String
        Public ReadOnly Property Message() As String
            Get
                Return _message
            End Get
        End Property

        Public Sub New(ByVal message As String)
            _message = message
        End Sub
    End Class
End Class
