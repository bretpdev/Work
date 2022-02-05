Imports RegentsScholarshipBackEnd
Imports System.Diagnostics

Public Class frmReports
    Public Sub RunReport(ByVal reportName As String)
        cmbSelectReport.Visible = False
        cmbSelectReport.SelectedItem = reportName
        cmbSelectReport_DropDownClosed(Me, New EventArgs())
    End Sub

    Private Sub frmReports_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Populate the combo box with the report names, plus a prompt.
        cmbSelectReport.Items.Add("Select a report...")
        cmbSelectReport.Items.Add("Appeals in Process")
        cmbSelectReport.Items.Add("Application Status")
        cmbSelectReport.Items.Add("Applications Received by High School--Award Summary")
        cmbSelectReport.Items.Add("Applications by High School--Detail List")
        cmbSelectReport.Items.Add("Award Status by School")
        cmbSelectReport.Items.Add("Award Status by School with Addresses")
        cmbSelectReport.Items.Add("Call Center Volumn at Month End")
        cmbSelectReport.Items.Add("College Attendance - Conditional Review")
        cmbSelectReport.Items.Add("College Attendance - Final Approval")
        cmbSelectReport.Items.Add("Comm Category Volume")
        cmbSelectReport.Items.Add("Conditional Review Award Level")
        cmbSelectReport.Items.Add("Conditional Review Award Level By School")
        cmbSelectReport.Items.Add("End of Initial Review Cycle")
        cmbSelectReport.Items.Add("Final Review Weekly Report")
        cmbSelectReport.Items.Add("Initial Review Weekly Report")
        cmbSelectReport.Items.Add("Missing College Transcript List--Conditional Review Period")
        cmbSelectReport.Items.Add("Missing College Transcript List--Final Review Period")
        cmbSelectReport.Items.Add("New Century Award Matches")
        cmbSelectReport.Items.Add("Outstanding Deferment Requests")
        cmbSelectReport.Items.Add("Outstanding Leave of Absence Requests")
        cmbSelectReport.Items.Add("Payments Not Renewed")
        cmbSelectReport.Items.Add("Ready For Conditional Review List")
        cmbSelectReport.Items.Add("Ready For Final Review List")
        cmbSelectReport.Items.Add("Renewal and Payment Status")
        cmbSelectReport.Items.Add("Review Status")
        cmbSelectReport.Items.Add("Review Status by School")
        cmbSelectReport.Items.Add("UESP Award By School")
        'Select the first item.
        cmbSelectReport.SelectedIndex = 0
    End Sub

    Private Sub cmbSelectReport_DropDownClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSelectReport.DropDownClosed
        'Run the selected report.
        If cmbSelectReport.SelectedItem Is Nothing OrElse cmbSelectReport.SelectedItem.ToString() = "Select a report..." Then
            Return
        End If

        Cursor = Cursors.WaitCursor
        Select Case cmbSelectReport.SelectedItem.ToString()
            Case "Appeals in Process"
                CRptViewer.ReportSource = Reports.OutstandingRequest(Reports.OutstandingRequestRptType.AppealsInProcess)
                CRptViewer.DisplayGroupTree = False
            Case "Application Status"
                CRptViewer.ReportSource = Reports.ApplicationStatus()
                CRptViewer.DisplayGroupTree = False
            Case "Applications Received by High School--Award Summary"
                CRptViewer.ReportSource = Reports.ApplicationsReceivedByHighSchool()
                CRptViewer.DisplayGroupTree = True
            Case "Applications by High School--Detail List"
                CRptViewer.ReportSource = Reports.ApplicationsReceivedBySchool()
                CRptViewer.DisplayGroupTree = True
            Case "Award Status by School"
                CRptViewer.ReportSource = Reports.AwardStatusBySchool()
                CRptViewer.DisplayGroupTree = True
            Case "Award Status by School with Addresses"
                Dim fileName As String = Reports.AwardStatusBySchoolWithAddresses()
                Try
                    Process.Start(fileName)
                Catch ex As Exception
                    Dim message As String = String.Format("The report was saved as {0}.", fileName)
                    MessageBox.Show(message, "Report Saved to File", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            Case "Call Center Volumn at Month End"
                CRptViewer.ReportSource = Reports.CallCenterVolume
                CRptViewer.DisplayGroupTree = False
            Case "College Attendance - Conditional Review"
                CRptViewer.ReportSource = Reports.CollegeAttendance(Reports.CollegeAttendanceAwardCodes.ConditionalApproval)
                CRptViewer.DisplayGroupTree = True
            Case "College Attendance - Final Approval"
                CRptViewer.ReportSource = Reports.CollegeAttendance(Reports.CollegeAttendanceAwardCodes.FinalApproval)
                CRptViewer.DisplayGroupTree = True
            Case "Comm Category Volume"
                Dim lastMonth As Integer = DateTime.Now.AddMonths(-1).Month
                Dim lastMonthsYear As Integer = DateTime.Now.AddMonths(-1).Year
                Dim startDate As New DateTime(lastMonthsYear, lastMonth, 1)
                Dim endDate As DateTime = startDate.AddMonths(1).AddSeconds(-1)
                CRptViewer.ReportSource = Reports.CommCategoryVolume(startDate, endDate)
                CRptViewer.DisplayGroupTree = False
            Case "Conditional Review Award Level"
                CRptViewer.ReportSource = Reports.ConditionalReviewAward()
                CRptViewer.DisplayGroupTree = True
            Case "Conditional Review Award Level By School"
                CRptViewer.ReportSource = Reports.ConditionalReviewAwardBySchool()
                CRptViewer.DisplayGroupTree = True
            Case "End of Initial Review Cycle"
                CRptViewer.ReportSource = Reports.EndOfInitialReviewCycle()
                CRptViewer.DisplayGroupTree = False
            Case "Final Review Weekly Report"
                CRptViewer.ReportSource = Reports.FinalReviewWeekly()
                CRptViewer.DisplayGroupTree = False
            Case "Initial Review Weekly Report"
                CRptViewer.ReportSource = Reports.InitialReviewWeekly()
                CRptViewer.DisplayGroupTree = False
            Case "Missing College Transcript List--Conditional Review Period"
                CRptViewer.ReportSource = Reports.GenericStudentLevelReport(Reports.GenericReportType.MissingCollegeTranscriptConditional)
                CRptViewer.DisplayGroupTree = False
            Case "Missing College Transcript List--Final Review Period"
                CRptViewer.ReportSource = Reports.GenericStudentLevelReport(Reports.GenericReportType.MissingCollegeTranscriptFinal)
                CRptViewer.DisplayGroupTree = False
            Case "New Century Award Matches"
                CRptViewer.ReportSource = Reports.NewCenturyMatches
                CRptViewer.DisplayGroupTree = False
            Case "Outstanding Deferment Requests"
                CRptViewer.ReportSource = Reports.OutstandingRequest(Reports.OutstandingRequestRptType.OutstandingDefermentRequests)
                CRptViewer.DisplayGroupTree = False
            Case "Outstanding Leave of Absence Requests"
                CRptViewer.ReportSource = Reports.OutstandingRequest(Reports.OutstandingRequestRptType.OutstandingLeaveOfAbsence)
                CRptViewer.DisplayGroupTree = False
            Case "Payments Not Renewed"
                CRptViewer.ReportSource = Reports.PaymentsNotRenewed()
                CRptViewer.DisplayGroupTree = False
            Case "Ready For Conditional Review List"
                CRptViewer.ReportSource = Reports.GenericStudentLevelReport(Reports.GenericReportType.ReadyForConditionalReview)
                CRptViewer.DisplayGroupTree = False
            Case "Ready For Final Review List"
                CRptViewer.ReportSource = Reports.GenericStudentLevelReport(Reports.GenericReportType.ReadyForFinalReview)
                CRptViewer.DisplayGroupTree = False
            Case "Renewal and Payment Status"
                CRptViewer.ReportSource = Reports.RenewalAndPaymentStatus()
                CRptViewer.DisplayGroupTree = False
            Case "Review Status"
                CRptViewer.ReportSource = Reports.ReviewStatus()
                CRptViewer.DisplayGroupTree = False
            Case "Review Status by School"
                CRptViewer.ReportSource = Reports.ReviewStatusBySchool()
                CRptViewer.DisplayGroupTree = True
            Case "UESP Award By School"
                CRptViewer.ReportSource = Reports.UespAwardsBySchool()
                CRptViewer.DisplayGroupTree = True
        End Select
        CRptViewer.Refresh()
        Cursor = Cursors.Default
    End Sub
End Class