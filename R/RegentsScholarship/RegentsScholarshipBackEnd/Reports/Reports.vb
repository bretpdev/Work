Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports Q

Public Class Reports
    'As of SR 3235, the initial and final review reports have different report templates,
    'different stored procedures, and different functions creating them, so this enum isn't used.
    'In an upcoming request they might re-align enough that we can use the enum again.
    Public Enum ReviewRptType
        Initial = 6
        Final = 8
    End Enum

    Public Enum OutstandingRequestRptType
        AppealsInProcess = 1
        OutstandingLeaveOfAbsence = 2
        OutstandingDefermentRequests = 3
    End Enum

    Public Enum CollegeAttendanceAwardCodes
        FinalApproval = 1
        ConditionalApproval = 3
    End Enum

    Public Enum GenericReportType
        MissingCollegeTranscriptConditional
        MissingCollegeTranscriptFinal
        ReadyForConditionalReview
        ReadyForFinalReview
    End Enum

    Public Shared Function GetAppYear() As String
        Dim getAppYearFrm As New frmGetAppYearToReportOn()
        getAppYearFrm.ShowDialog()
        Return getAppYearFrm.ApplicationYear
    End Function

    Public Shared Sub InitialReview()
        Dim report As New InitialReview()
        report.SetDataSource(DataAccess.GetInitialReviewReport())
        'report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Q.DataAccessBase.PersonalDataDirectory + "Test.pdf")
        report.PrintToPrinter(1, False, 1, -1)
    End Sub

    Public Shared Sub FinalPayments(ByVal batchNumber As String)
        Dim approvedPayments As List(Of DSFinalApprovalPayments) = DataAccess.GetFinalApprovalPayments(batchNumber)
        Dim positivePayments As List(Of DSFinalApprovalPayments) = approvedPayments.Where(Function(p) p.Amount >= 0).ToList()
        Dim report As New FinalPayments()
        Dim groupedPayments As List(Of DSFinalApprovalPayments) = GroupPayments(approvedPayments)
        report.OpenSubreport("FinalApprovalPositivePayments.rpt").SetDataSource(groupedPayments)
        report.OpenSubreport("FinalApprovalGrandTotals.rpt").SetDataSource(groupedPayments)
        report.OpenSubreport("FinalApprovalNegativePayments.rpt").SetDataSource(DataAccess.GetFinalApprovalNegativePayments(batchNumber))
        report.OpenSubreport("DeniedPayments.rpt").SetDataSource(DataAccess.GetFinalDeniedPayments(batchNumber))
        report.PrintToPrinter(1, False, 1, -1)
        'report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "T:\Test.pdf")

        WriteCollegePaymentDetailFiles(groupedPayments)
    End Sub

    Public Shared Sub FinalReview()
        Dim report As New ReviewRpt()
        report.SetDataSource(DataAccess.GetFinalReview())
        'Rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Q.DataAccessBase.PersonalDataDirectory + "Test.pdf")
        report.PrintToPrinter(1, False, 1, -1)
    End Sub

    Public Shared Sub CommunicationRecords(ByVal entity As String, ByVal entityType As String, ByVal fromDate As String, ByVal toDate As String, ByVal entityName As String, ByVal sortColumnName As String, ByVal sortAscending As Boolean)
        Dim Rpt As New CommunicationRecords()
        Rpt.SummaryInfo.ReportTitle = String.Format("Communication Records for ({0}) - {1}", entityType, entityName)
        Rpt.SetDataSource(DataAccess.GetCommunicationRecords(entity, entityType, fromDate, toDate))
        Rpt.DataDefinition.SortFields(0).Field = Rpt.Database.Tables(0).Fields(sortColumnName)
        If sortAscending Then
            Rpt.DataDefinition.SortFields(0).SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder
        Else
            Rpt.DataDefinition.SortFields(0).SortDirection = CrystalDecisions.Shared.SortDirection.DescendingOrder
        End If
        'Rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "T:\Test.pdf")
        Rpt.PrintToPrinter(1, False, 1, -1)
    End Sub

    Public Shared Sub PrelimPayments()
        Dim report As New PrelimPayments()
        report.OpenSubreport("PrelimApprovalPayments.rpt").SetDataSource(DataAccess.GetPrelimApprovalPayments())
        report.OpenSubreport("DeniedPayments.rpt").SetDataSource(DataAccess.GetPrelimDeniedPayments())
        report.PrintToPrinter(1, False, 1, -1)
        'report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "T:\Test.pdf")
    End Sub

    Public Shared Sub UespAwardReview()
        Dim report As New UESPAwardReviewRpt()
        report.SetDataSource(DataAccess.GetUespAwardReview())
        'report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "T:\UespAwardReviewTest.pdf")
        report.PrintToPrinter(1, False, 1, -1)
    End Sub

    Public Shared Function OutstandingRequest(ByVal reportType As OutstandingRequestRptType) As OutstandingRequestsRpt
        Dim report As New OutstandingRequestsRpt()
        report.SetDataSource(DataAccess.GetOutstandingDocumentRequests(DirectCast(reportType, Integer), GetAppYear()))
        If reportType = OutstandingRequestRptType.AppealsInProcess Then
            report.SummaryInfo.ReportTitle = "Appeals In Process"
        ElseIf reportType = OutstandingRequestRptType.OutstandingDefermentRequests Then
            report.SummaryInfo.ReportTitle = "Outstanding Deferment Requests"
        ElseIf reportType = OutstandingRequestRptType.OutstandingLeaveOfAbsence Then
            report.SummaryInfo.ReportTitle = "Outstanding Leave Of Absence Requests"
        End If
        Return report
    End Function

    Public Shared Function ApplicationsReceivedBySchool() As ApplicationsReceivedBySchool
        Dim report As New ApplicationsReceivedBySchool()
        report.SetDataSource(DataAccess.GetApplicationsReceived(GetAppYear()))
        Return report
    End Function

    Public Shared Function ApplicationStatus() As AppStatus
        Dim report As New AppStatus()
        report.SetDataSource(DataAccess.GetApplicationStatus(GetAppYear()))
        Return report
    End Function

    Public Shared Function AwardStatusBySchool() As AwardStatusBySchool
        Dim report As New AwardStatusBySchool()
        report.SetDataSource(DataAccess.GetAwardStatusBySchool(GetAppYear()))
        Return report
    End Function

    Public Shared Function AwardStatusBySchoolWithAddresses() As String
        Dim fileName As String = String.Format("T:\AwardStatusByHighSchool.{0:yyyyMMdd.HHmm}.csv", DateTime.Now)
        Using reportWriter As New StreamWriter(fileName)
            reportWriter.WriteCommaDelimitedLine("SchoolName", "StudentID", "FirstName", "LastName", "Address", "City", "State", "Zip", "Country", "Denied", "Base", "Exemplary", "UESP", "Incomplete")
            For Each record As DSAwardStatusBySchool In DataAccess.GetAwardStatusBySchool(GetAppYear())
                Dim denied As String = If(record.Denied, "X", "")
                Dim base As String = If(record.BaseAward, "X", "")
                Dim exemplary As String = If(record.ExemplaryAward, "X", "")
                Dim uesp As String = If(record.UespAward, "X", "")
                Dim incomplete As String = If(record.Incomplete, "X", "")
                reportWriter.WriteCommaDelimitedLine(record.Name, record.StateStudentId, record.FirstName, record.LastName, record.Address1, record.City, record.Abbreviation, record.Zip, record.Country, denied, base, exemplary, uesp, incomplete)
            Next record
        End Using
        Return fileName
    End Function

    Public Shared Function EndOfInitialReviewCycle() As EndOfInitialReviewCycle
        Dim report As New EndOfInitialReviewCycle()
        report.OpenSubreport("EndOfCycleOutcomes.rpt").SetDataSource(DataAccess.GetEndOfCycleOutcomes(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("EndOfCycleColleges.rpt").SetDataSource(DataAccess.GetEndOfCycleColleges(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("EndOfCycleAppeals.rpt").SetDataSource(DataAccess.GetEndOfCycleAppeals(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("EndOfCycleCallCenterVolume.rpt").SetDataSource(DataAccess.GetEndOfCycleCallCenterVolume())
        Return report
    End Function

    Public Shared Function FinalReviewWeekly() As FinalReviewWeekly
        Dim report As New FinalReviewWeekly()
        report.OpenSubreport("FinalWeeklyApps.rpt").SetDataSource(DataAccess.GetFinalWeeklyApps(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("FinalWeeklyReadyForReview.rpt").SetDataSource(DataAccess.GetFinalWeeklyReadyForReview(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("FinalWeeklyStages.rpt").SetDataSource(DataAccess.GetFinalWeeklyStages(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("FinalWeeklyOutcomes.rpt").SetDataSource(DataAccess.GetFinalWeeklyOutcomes(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("FinalWeeklyDeferments.rpt").SetDataSource(DataAccess.GetFinalWeeklyDeferments(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("FinalWeeklyAppeals.rpt").SetDataSource(DataAccess.GetFinalWeeklyAppeals(Constants.CURRENT_AWARD_YEAR.ToString()))
        Return report
    End Function

    Public Shared Function InitialReviewWeekly() As InitialReviewWeekly
        Dim report As New InitialReviewWeekly()
        report.OpenSubreport("InitialWeeklyApps.rpt").SetDataSource(DataAccess.GetInitialWeeklyApps(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("InitialWeeklyStages.rpt").SetDataSource(DataAccess.GetInitialWeeklyStages(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("EndOfCycleOutcomes.rpt").SetDataSource(DataAccess.GetEndOfCycleOutcomes(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("InitialWeeklyUesp.rpt").SetDataSource(DataAccess.GetInitialWeeklyUesp(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("InitialWeeklyColleges.rpt").SetDataSource(DataAccess.GetEndOfCycleColleges(Constants.CURRENT_AWARD_YEAR.ToString()))
        report.OpenSubreport("InitialWeeklyAppeals.rpt").SetDataSource(DataAccess.GetInitialWeeklyAppeals(Constants.CURRENT_AWARD_YEAR.ToString()))
        Return report
    End Function

    Public Shared Function NewCenturyMatches() As NewCenturyMatchesRpt
        Dim report As New NewCenturyMatchesRpt()
        report.SetDataSource(DataAccess.GetNewCenturyAwardMatches(GetAppYear()))
        Return report
    End Function

    Public Shared Function PaymentsNotRenewed() As PaymentsNotRenewed
        Dim report As New PaymentsNotRenewed()
        report.SetDataSource(DataAccess.GetPaymentsNotRenewed())
        Return report
    End Function

    Public Shared Function ReviewStatus() As ReviewStatus
        Dim report As New ReviewStatus()
        report.SetDataSource(DataAccess.GetReviewStatus(GetAppYear()))
        Return report
    End Function

    Public Shared Function ReviewStatusBySchool() As ReviewStatusBySchool
        Dim report As New ReviewStatusBySchool()
        report.SetDataSource(DataAccess.GetReviewStatus(GetAppYear()))
        Return report
    End Function

    Public Shared Function CallCenterVolume() As CallCenterVolume
        Dim report As New CallCenterVolume()
        report.SetDataSource(DataAccess.GetCallCenterVolume())
        Return report
    End Function

    Public Shared Function ConditionalReviewAward() As ConditionalReviewAwardRpt
        Dim report As New ConditionalReviewAwardRpt()
        report.SetDataSource(DataAccess.GetConditionalReviewAward(GetAppYear()))
        Return report
    End Function

    Public Shared Function ConditionalReviewAwardBySchool() As ConditionalReviewAwardBySchoolRpt
        Dim report As New ConditionalReviewAwardBySchoolRpt()
        report.SetDataSource(DataAccess.GetConditionalReviewAwardBySchool(GetAppYear()))
        Return report
    End Function

    Public Shared Function CollegeAttendance(ByVal awardStatusCode As CollegeAttendanceAwardCodes) As CollegeAttendanceRpt
        Dim report As New CollegeAttendanceRpt()
        report.SetDataSource(DataAccess.GetCollegeAttendance(DirectCast(awardStatusCode, Integer), GetAppYear()))
        If awardStatusCode = CollegeAttendanceAwardCodes.FinalApproval Then
            report.SummaryInfo.ReportTitle = "Final Approval College Attendance"
        ElseIf awardStatusCode = CollegeAttendanceAwardCodes.ConditionalApproval Then
            report.SummaryInfo.ReportTitle = "Conditional Review College Attendance"
        End If
        Return report
    End Function

    Public Shared Function CommCategoryVolume(ByVal startDate As DateTime, ByVal endDate As DateTime) As CommCategoryVolume
        Dim report As New CommCategoryVolume()
        report.SetDataSource(DataAccess.GetCommCategoryVolume(startDate, endDate))
        report.SummaryInfo.ReportTitle = "Comm Category Volume"
        Return report
    End Function

    Public Shared Function ApplicationsReceivedByHighSchool() As ApplicationsReceivedByHighSchoolRpt
        Dim report As New ApplicationsReceivedByHighSchoolRpt()
        report.SetDataSource(DataAccess.GetApplicationsReceivedByHighSchool(GetAppYear()))
        Return report
    End Function

    Public Shared Function RenewalAndPaymentStatus() As RenewalAndPaymentStatus
        Dim report As New RenewalAndPaymentStatus()
        report.SetDataSource(DataAccess.GetRenewalAndPaymentStatus())
        Return report
    End Function

    Public Shared Function UespAwardsBySchool() As UESPAwardsBySchoolRpt
        Dim report As New UESPAwardsBySchoolRpt()
        report.SetDataSource(DataAccess.GetUespAwardsBySchool(GetAppYear()))
        Return report
    End Function

    Public Shared Function GenericStudentLevelReport(ByVal reportType As GenericReportType) As GenericReportAtStudentLvl
        Dim report As New GenericReportAtStudentLvl()
        Select Case reportType
            Case GenericReportType.MissingCollegeTranscriptConditional
                report.SummaryInfo.ReportTitle = "Missing College Transcript List--Conditional Review Period"
                report.SetDataSource(DataAccess.GetMissingCollegeTranscripts(GetAppYear()))
            Case GenericReportType.MissingCollegeTranscriptFinal
                report.SummaryInfo.ReportTitle = "Missing College Transcript List--Final Review Period"
                report.SetDataSource(DataAccess.GetMissingCollegeTranscriptsFinal(GetAppYear()))
            Case GenericReportType.ReadyForConditionalReview
                report.SummaryInfo.ReportTitle = "Ready For Conditional Review List"
                report.SetDataSource(DataAccess.GetReadyForConditionalReview(GetAppYear()))
            Case GenericReportType.ReadyForFinalReview
                report.SummaryInfo.ReportTitle = "Ready For Final Review List"
                report.SetDataSource(DataAccess.GetReadyForFinalReview(GetAppYear()))
        End Select
        Return report
    End Function

    Private Shared Function GroupPayments(ByVal approvedPayments As List(Of DSFinalApprovalPayments)) As List(Of DSFinalApprovalPayments)
        Dim positivePayments As List(Of DSFinalApprovalPayments) = approvedPayments.Where(Function(p) p.Amount >= 0).ToList()
        Dim negativePayments As List(Of DSFinalApprovalPayments) = approvedPayments.Where(Function(p) p.Amount < 0).ToList()
        For Each negativePayment As DSFinalApprovalPayments In negativePayments
            Dim np As DSFinalApprovalPayments = negativePayment 'For use in lambda expressions.
            'FirstOrDefault() is used on the next line in case there are multiple semesters getting positive payments.
            'It doesn't matter which one we subtract the negative payment from, since the report groups the payments at the student level.
            Dim positivePayment As DSFinalApprovalPayments = positivePayments.Where(Function(p) p.College = np.College AndAlso p.PaymentType = np.PaymentType AndAlso p.SSN = np.SSN).FirstOrDefault()
            If (positivePayment IsNot Nothing) Then
                positivePayment.Amount += np.Amount
            End If
        Next negativePayment
        positivePayments = positivePayments.Where(Function(p) p.Amount > 0).ToList()

        'There may be positive adjustments, which means a student has more than one positive payment in the batch.
        'In that case, roll it up by adding the adjustment amount to the latest semester's payment
        '(which is assumed to be the semester we want to report) and removing the adjustment record.
        Dim multiSemesters As New List(Of DSFinalApprovalPayments)()
        For Each positivePayment As DSFinalApprovalPayments In positivePayments
            Dim pp As DSFinalApprovalPayments = positivePayment 'For use in lambda expressions.
            If (positivePayments.Where(Function(p) p.College = pp.College AndAlso p.PaymentType = pp.PaymentType AndAlso p.SSN = pp.SSN).Count() > 1) Then
                multiSemesters.Add(pp)
            End If
        Next positivePayment
        For Each multiSemester As DSFinalApprovalPayments In multiSemesters
            Dim ms As DSFinalApprovalPayments = multiSemester 'For use in lambda expressions.
            'Dtermine the latest year for this student at this college, and get the payments for that year.
            Dim lastMsYear As Integer = multiSemesters.Where(Function(p) p.College = ms.College AndAlso p.PaymentType = ms.PaymentType AndAlso p.SSN = ms.SSN).OrderBy(Function(p) p.TermYear).Last().TermYear
            Dim lastYearPayments As List(Of DSFinalApprovalPayments) = multiSemesters.Where(Function(p) p.College = ms.College AndAlso p.PaymentType = ms.PaymentType AndAlso p.SSN = ms.SSN AndAlso p.TermYear = lastMsYear).ToList()
            'Find the payment for the latest semester. (Assumes there's not more than one payment per semester in the batch.)
            Dim lastMs As DSFinalApprovalPayments = lastYearPayments.Where(Function(p) p.Term = Constants.CollegeTerm.FALL).SingleOrDefault()
            If (lastMs Is Nothing) Then lastMs = lastYearPayments.Where(Function(p) p.Term = Constants.CollegeTerm.SUMMER).SingleOrDefault()
            If (lastMs Is Nothing) Then lastMs = lastYearPayments.Where(Function(p) p.Term = Constants.CollegeTerm.SPRING).SingleOrDefault()
            If (lastMs Is Nothing) Then lastMs = lastYearPayments.Where(Function(p) p.Term = Constants.CollegeTerm.WINTER).SingleOrDefault()
            'Adjust the latest semester's payment if this is an adjustment.
            If (ms.Term <> lastMs.Term OrElse ms.TermYear <> lastMs.TermYear) Then
                positivePayments.Where(Function(p) p.College = lastMs.College AndAlso p.PaymentType = lastMs.PaymentType AndAlso p.SSN = lastMs.SSN AndAlso p.Term = lastMs.Term AndAlso p.TermYear = lastMs.TermYear).Single().Amount += ms.Amount
                positivePayments.Remove(ms)
            End If
        Next multiSemester

        Return positivePayments
    End Function

    Private Shared Sub WriteCollegePaymentDetailFiles(ByVal payments As List(Of DSFinalApprovalPayments))
        Dim ftpFolder As String = DataAccessBase.SASDataFileDirectory
        If (Constants.TEST_MODE) Then ftpFolder += "Test\"

        For Each collegeName As String In payments.Select(Function(p) p.College).Distinct()
            Dim college As String = collegeName 'Avoids compiler warnings about lambda comparisons.
            Dim collegeFile As String = String.Format("{0}{1} {2}.txt", ftpFolder, college, DateTime.Now.ToString("MMddyyyy"))
            Using fileWriter As New StreamWriter(collegeFile)
                Dim collegePayments As List(Of DSFinalApprovalPayments) = payments.Where(Function(p) p.College = college).ToList()
                For Each studentSsn As String In collegePayments.OrderBy(Function(p) p.LastName).Select(Function(p) p.SSN).Distinct()
                    Dim ssn As String = studentSsn 'Avoids compiler warnings about lambda comparisons.
                    Dim studentPayments As List(Of DSFinalApprovalPayments) = collegePayments.Where(Function(p) p.SSN = ssn).ToList()
                    Dim firstPayment As DSFinalApprovalPayments = studentPayments.First()
                    Dim studentPaymentTotal As Double = studentPayments.Select(Function(p) p.Amount).Sum()
                    fileWriter.WriteCommaDelimitedLine(firstPayment.LastName, firstPayment.FirstName, firstPayment.SSN, studentPaymentTotal.ToString("$0.00"))
                Next studentSsn
                fileWriter.Close()
            End Using
        Next collegeName
    End Sub
End Class
