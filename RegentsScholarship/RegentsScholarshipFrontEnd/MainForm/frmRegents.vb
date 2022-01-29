Imports System
Imports System.IO
Imports System.Linq
Imports RegentsScholarshipBackEnd
Imports Category = RegentsScholarshipBackEnd.Constants.CourseCategory
Imports ReviewType = RegentsScholarshipBackEnd.Constants.ReviewType

Public Class frmRegents
    ''' <summary>
    ''' Indicates a screen (tab) on the main form.
    ''' </summary>
    Private Enum Screen
        Application
        Communication
        Demographics
        Payments
    End Enum

    Public Sub New(ByVal loginUser As User)
        Try
            InitializeComponent()
            _paymentReviewer = New PaymentReviews()
            _user = loginUser
            GiveAccess(loginUser.AccessLevel)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

#Region "Member variables"
    Private WithEvents _paymentReviewer As PaymentReviews
    Private _student As Student = Nothing
    Private _user As User = Nothing
    Private _twoWayBindingSources As List(Of BindingSource) = Nothing

#Region "Child forms"
    Private _reportsForm As frmReports
    Private _highSchoolCommunicationsForm As frmHighSchoolCommunications
    Private _jrHighSchoolCommuncationsForm As frmJrHighSchoolCommunications
    Private _districtCommuncationsForm As frmDistrictCommunications
    Private _miscCommuncationsForm As frmMiscCommunications
    Private _transactionAuditHistoryForm As frmTransactionAuditHistory
    Private _webAppPasswordReset As frmWebAppPasswordReset
    Private _batchQuickReview As frmBatchQuickReview
#End Region 'Child forms
#End Region 'Member variables

#Region "Toolbar"
    Private Sub btnQuickBatchReview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuickBatchReview.Click
        Try
            If _batchQuickReview Is Nothing OrElse _batchQuickReview.Visible = False Then
                _batchQuickReview = New frmBatchQuickReview(_user)
                _batchQuickReview.Show()
            Else
                _batchQuickReview.Activate()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnWebPasswordReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWebPasswordReset.Click
        Try
            If _webAppPasswordReset Is Nothing OrElse _webAppPasswordReset.Visible = False Then
                _webAppPasswordReset = New frmWebAppPasswordReset()
                _webAppPasswordReset.Show()
            Else
                _webAppPasswordReset.Activate()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnDistrictCommunication_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDistrictCommunication.Click
        Try
            If _districtCommuncationsForm Is Nothing OrElse _districtCommuncationsForm.Visible = False Then
                _districtCommuncationsForm = New frmDistrictCommunications(_user)
                _districtCommuncationsForm.Show()
            Else
                _districtCommuncationsForm.Activate()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnJuniorHighSchoolCommuncation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJuniorHighSchoolCommuncation.Click
        Try
            If _jrHighSchoolCommuncationsForm Is Nothing OrElse _jrHighSchoolCommuncationsForm.Visible = False Then
                _jrHighSchoolCommuncationsForm = New frmJrHighSchoolCommunications(_user)
                _jrHighSchoolCommuncationsForm.Show()
            Else
                _jrHighSchoolCommuncationsForm.Activate()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnMiscCommunication_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMiscCommunication.Click
        Try
            If _miscCommuncationsForm Is Nothing OrElse _miscCommuncationsForm.Visible = False Then
                _miscCommuncationsForm = New frmMiscCommunications(_user)
                _miscCommuncationsForm.Show()
            Else
                _miscCommuncationsForm.Activate()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReports.Click
        Try
            If _reportsForm Is Nothing OrElse _reportsForm.Visible = False Then
                _reportsForm = New frmReports()
                _reportsForm.Show()
            Else
                _reportsForm.Activate()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnHighSchoolCommunications_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHighSchoolCommunications.Click
        Try
            If _highSchoolCommunicationsForm Is Nothing OrElse _highSchoolCommunicationsForm.Visible = False Then
                _highSchoolCommunicationsForm = New frmHighSchoolCommunications(_user)
                _highSchoolCommunicationsForm.Show()
            Else
                _highSchoolCommunicationsForm.Activate()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnTransactionAuditHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransactionAuditHistory.Click
        Try
            If _transactionAuditHistoryForm Is Nothing OrElse _transactionAuditHistoryForm.Visible = False Then
                _transactionAuditHistoryForm = New frmTransactionAuditHistory()
                _transactionAuditHistoryForm.Show()
            Else
                _transactionAuditHistoryForm.Activate()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
#End Region 'Toolbar

#Region "Event handlers"
#Region "Main Menu tab"
    Private Sub btnMainMenuSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMainMenuSearch.Click
        Try
            Search()
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub dgvMainMenuSearchMatches_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMainMenuSearchMatches.CellDoubleClick
        Try
            'Detect double-clicks on the column headers and ignore them.
            If e.RowIndex = -1 Then
                Return
            End If

            'Get the student ID from the row that was double-clicked.
            Dim studentId As String = dgvMainMenuSearchMatches.Rows(e.RowIndex).Cells("StateStudentID").Value
            'Clear the search form and load the student.
            ClearSearchResults()
            LoadStudent(studentId)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub dgvMainMenuSearchMatches_ColumnHeaderMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvMainMenuSearchMatches.ColumnHeaderMouseClick
        Try
            'Declare some static booleans to track the sort order for each column.
            Static cityIsAscending As Boolean = False
            Static firstNameIsAscending As Boolean = False
            Static lastNameIsAscending As Boolean = False
            Static ssnIsAscending As Boolean = False
            Static studentIdIsAscending As Boolean = False
            Static addressIsAscending As Boolean = False

            'Based on the clicked column, set all the static booleans and call SortedSearch.
            Select Case e.ColumnIndex
                Case dgvMainMenuSearchMatches.Columns.IndexOf(dgvMainMenuSearchMatches.Columns("City"))
                    cityIsAscending = Not cityIsAscending
                    firstNameIsAscending = False
                    lastNameIsAscending = False
                    ssnIsAscending = False
                    studentIdIsAscending = False
                    addressIsAscending = False
                    SortedSearch("City", cityIsAscending)
                Case dgvMainMenuSearchMatches.Columns.IndexOf(dgvMainMenuSearchMatches.Columns("FirstName"))
                    cityIsAscending = False
                    firstNameIsAscending = Not firstNameIsAscending
                    lastNameIsAscending = False
                    ssnIsAscending = False
                    studentIdIsAscending = False
                    addressIsAscending = False
                    SortedSearch("FirstName", firstNameIsAscending)
                Case dgvMainMenuSearchMatches.Columns.IndexOf(dgvMainMenuSearchMatches.Columns("LastName"))
                    cityIsAscending = False
                    firstNameIsAscending = False
                    lastNameIsAscending = Not lastNameIsAscending
                    ssnIsAscending = False
                    studentIdIsAscending = False
                    addressIsAscending = False
                    SortedSearch("LastName", lastNameIsAscending)
                Case dgvMainMenuSearchMatches.Columns.IndexOf(dgvMainMenuSearchMatches.Columns("SocialSecurityNumber"))
                    cityIsAscending = False
                    firstNameIsAscending = False
                    lastNameIsAscending = False
                    ssnIsAscending = Not ssnIsAscending
                    studentIdIsAscending = False
                    addressIsAscending = False
                    SortedSearch("SocialSecurityNumber", ssnIsAscending)
                Case dgvMainMenuSearchMatches.Columns.IndexOf(dgvMainMenuSearchMatches.Columns("StateStudentID"))
                    cityIsAscending = False
                    firstNameIsAscending = False
                    lastNameIsAscending = False
                    ssnIsAscending = False
                    studentIdIsAscending = Not studentIdIsAscending
                    addressIsAscending = False
                    SortedSearch("StateStudentID", studentIdIsAscending)
                Case dgvMainMenuSearchMatches.Columns.IndexOf(dgvMainMenuSearchMatches.Columns("StreetAddressLine1"))
                    cityIsAscending = False
                    firstNameIsAscending = False
                    lastNameIsAscending = False
                    ssnIsAscending = False
                    studentIdIsAscending = False
                    addressIsAscending = Not addressIsAscending
                    SortedSearch("StreetAddressLine1", addressIsAscending)
            End Select
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub dtpMainMenuSearchDateOfBirth_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If e.KeyCode = Keys.Enter Then
                Search()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub txtMainMenuSearchStateStudentId_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMainMenuSearchStateStudentId.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Search()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub txtMainMenuSearchSsn_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMainMenuSearchSsn.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Search()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub txtMainMenuSearchFirstName_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMainMenuSearchFirstName.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Search()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub txtMainMenuSearchLastName_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMainMenuSearchLastName.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Search()
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub
#End Region 'Main Menu tab

#Region "Demographics tab"
    Private Sub btnDemographicsSaveChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDemographicsSaveChanges.Click
        Try
            SaveStudent(Student.Component.Demographics)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub txtDemographicsDateOfBirth_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDemographicsDateOfBirth.Leave
        If txtDemographicsDateOfBirth.ValidateText() = Nothing Then
            Dim message As String = String.Format("The date of birth ({0}) is not a valid date.", txtDemographicsDateOfBirth.Text)
            MessageBox.Show(message, "Invalid Date of Birth", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub txtDemographicsStateStudentId_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDemographicsStateStudentId.DoubleClick
        Try
            ChangeStudentId()
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub
#End Region 'Demographics tab

#Region "Application tab"
    Private Sub btnApplicationSaveChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplicationSaveChanges.Click
        Try
            If ApplicationConstraintsAreMet() Then
                SaveStudent(Student.Component.Application)
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnApplicationLinkDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplicationLinkDocument.Click
        Try
            LinkDocument(Screen.Application)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnApplicationViewDocuments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplicationViewDocuments.Click
        Try
            FetchDocument(Screen.Application)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub chkApplicationAppealApproved_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationAppealApproved.CheckedChanged
        Try
            If (chkApplicationAppealApproved.Checked) Then
                'Uncheck the Denied check box and set the award status based on whether the final review is done.
                chkApplicationAppealDenied.Checked = False
                If (_student.ScholarshipApplication.Reviews.ContainsKey(Constants.ReviewType.BASE_AWARD)) Then
                    cmbApplicationAwardStatus.Text = Constants.AwardStatus.APPROVED
                Else
                    cmbApplicationAwardStatus.Text = Constants.AwardStatus.CONDITIONAL_APPROVAL
                End If
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub chkApplicationAppealDenied_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationAppealDenied.CheckedChanged
        Try
            If (chkApplicationAppealDenied.Checked) Then
                'Uncheck the Approved check box and set the award status to Denied.
                chkApplicationAppealApproved.Checked = False
                cmbApplicationAwardStatus.Text = Constants.AwardStatus.DENIED
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub chkApplicationExemplaryAwardEarned_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationExemplaryAwardEarned.CheckedChanged
        If chkApplicationExemplaryAwardEarned.Checked = True Then
            txtApplicationExemplaryAwardAmount.ForeColor = Color.Black
        Else
            txtApplicationExemplaryAwardAmount.ForeColor = Color.Transparent

            'See if any Exemplary payments need to be deleted.
            Dim existingExemplaryPayments As List(Of Payment) = _student.Payments.Where(Function(p) p.Type = RegentsScholarshipBackEnd.Constants.PaymentType.EXEMPLARY).ToList()
            If (existingExemplaryPayments.Count > 0) Then
                'Delete them if they're all pending; warn the user that they can't be deleted otherwise.
                If (existingExemplaryPayments.Where(Function(p) p.Status = RegentsScholarshipBackEnd.Constants.PaymentStatus.PENDING).Count() = existingExemplaryPayments.Count) Then
                    For Each existingPayment As Payment In existingExemplaryPayments
                        _student.Payments.RemoveAll(Function(p) p.Type = RegentsScholarshipBackEnd.Constants.PaymentType.EXEMPLARY)
                    Next
                    _student.Commit(_user.Id, Student.Component.Payments)
                Else
                    Dim message As String = "The Exemplary Award can't be removed because the student has one or more approved Exemplary payments."
                    MessageBox.Show(message, "Can't Remove Exemplary Award", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    chkApplicationExemplaryAwardEarned.Checked = True
                End If
            End If
        End If

    End Sub

    Private Sub chkApplicationSupplementalAwardApproved_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationSupplementalAwardApproved.CheckedChanged
        If chkApplicationSupplementalAwardApproved.Checked = True Then
            txtApplicationSupplementalAwardAmount.ForeColor = Color.Black
        Else
            txtApplicationSupplementalAwardAmount.ForeColor = Color.Transparent

            'See if a UESP payment needs to be deleted.
            Dim existingUespPayment As Payment = _student.Payments.Where(Function(p) p.Type = RegentsScholarshipBackEnd.Constants.PaymentType.UESP).SingleOrDefault()
            If (existingUespPayment IsNot Nothing) Then
                'Delete it if it's pending; warn the user that it can't be deleted otherwise.
                If (existingUespPayment.Status = RegentsScholarshipBackEnd.Constants.PaymentStatus.PENDING) Then
                    _student.Payments.Remove(existingUespPayment)
                    _student.Commit(_user.Id, Student.Component.Payments)
                Else
                    Dim message As String = "The UESP Supplemental Award can't be removed because the student has an approved UESP payment."
                    MessageBox.Show(message, "Can't Remove UESP Supplemental Award", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    chkApplicationSupplementalAwardApproved.Checked = True
                End If
            End If
        End If
    End Sub

    Private Sub cmbApplicationAwardStatus_DropDownClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbApplicationAwardStatus.DropDownClosed
        'Do nothing if there's no student loaded.
        If _student Is Nothing Then
            Return
        End If

        Try
            'Update the Status Date and User ID text fields if the award status changed.
            If cmbApplicationAwardStatus.SelectedItem.ToString() <> _student.ScholarshipApplication.BaseAward.Status Then
                txtApplicationAwardStatusDate.Text = DateTime.Now.ToShortDateString()
                txtApplicationAwardStatusUserId.Text = _user.Id
            End If
            'Must call EndEdit on the binding source for the change to persist.
            PrimaryAwardBindingSource.EndEdit()

            'Set the denial reasons group box according to the selected status.
            If cmbApplicationAwardStatus.SelectedItem.ToString() = Constants.AwardStatus.DENIED Then
                'Enable the denials reaons.
                grpApplicationDenialReasons.Enabled = True
            Else
                'Clear and disable all the denial reasons.
                For Each combo As ComboBox In grpApplicationDenialReasons.Controls
                    combo.Text = ""
                Next
                grpApplicationDenialReasons.Enabled = False
            End If

            'Update the appeal decision if needed.
            Dim approvedStatuses() As String = {Constants.AwardStatus.CONDITIONAL_APPROVAL, Constants.AwardStatus.APPROVED}
            If (approvedStatuses.Contains(cmbApplicationAwardStatus.SelectedItem.ToString()) AndAlso chkApplicationAppealApproved.Enabled) Then
                chkApplicationAppealApproved.Checked = True
            ElseIf (cmbApplicationAwardStatus.SelectedItem.ToString() = Constants.AwardStatus.DENIED AndAlso chkApplicationAppealDenied.Enabled) Then
                chkApplicationAppealDenied.Checked = True
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub dtpApplicationAppealDecisionDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpApplicationAppealDecisionDate.ValueChanged
        If (dtpApplicationAppealDecisionDate.Value Is Nothing) Then
            'No appeal decision. Uncheck and disable the check boxes.
            lblApplicationAppealDecision.Enabled = False
            chkApplicationAppealApproved.Checked = False
            chkApplicationAppealApproved.Enabled = False
            chkApplicationAppealDenied.Checked = False
            chkApplicationAppealDenied.Enabled = False
        Else
            'Appeal decision made. Enable the check boxes, but don't mark either one.
            lblApplicationAppealDecision.Enabled = True
            chkApplicationAppealApproved.Enabled = True
            chkApplicationAppealDenied.Enabled = True
        End If
    End Sub

#Region "Reviews"
    Private Sub chkApplicationInitialTranscriptReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationInitialTranscriptReview.CheckedChanged
        If chkApplicationInitialTranscriptReview.Checked Then
            chkApplicationInitialTranscriptReviewStartStop.Checked = False
            chkApplicationInitialTranscriptReviewStartStop.Visible = False
            txtApplicationInitialTranscriptReviewDate.Text = DateTime.Now.ToShortDateString()
            txtApplicationInitialTranscriptReviewUserId.Text = _user.Id
            txtApplicationInitialTranscriptReviewDate.Visible = True
            txtApplicationInitialTranscriptReviewUserId.Visible = True

        Else
            txtApplicationInitialTranscriptReviewDate.Text = ""
            txtApplicationInitialTranscriptReviewUserId.Text = ""
            txtApplicationInitialTranscriptReviewDate.Visible = False
            txtApplicationInitialTranscriptReviewUserId.Visible = False
            chkApplicationInitialTranscriptReviewStartStop.Visible = True
        End If
    End Sub

    Private Sub chkApplicationInitialTranscriptReviewStartStop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationInitialTranscriptReviewStartStop.CheckedChanged
        If (chkApplicationInitialTranscriptReviewStartStop.Checked) Then
            DataAccess.StartReview(_student.StateStudentId(), _user.Id, ReviewType.INITIAL_TRANSCRIPT)
            chkApplicationInitialTranscriptReviewStartStop.Text = "Stop"
            chkApplicationInitialTranscriptReviewStartStop.BackColor = Color.Red
        Else
            DataAccess.StopReview(_student.StateStudentId(), _user.Id, ReviewType.INITIAL_TRANSCRIPT)
            chkApplicationInitialTranscriptReviewStartStop.Text = "Start"
            chkApplicationInitialTranscriptReviewStartStop.BackColor = Color.LightGreen
        End If
    End Sub

    Private Sub chkApplicationSecondTranscriptReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationSecondTranscriptReview.CheckedChanged
        If chkApplicationSecondTranscriptReview.Checked Then
            chkApplicationSecondTranscriptReviewStartStop.Checked = False
            chkApplicationSecondTranscriptReviewStartStop.Visible = False
            txtApplicationSecondTranscriptReviewDate.Text = DateTime.Now.ToShortDateString()
            txtApplicationSecondTranscriptReviewUserId.Text = _user.Id
            txtApplicationSecondTranscriptReviewDate.Visible = True
            txtApplicationSecondTranscriptReviewUserId.Visible = True

        Else
            txtApplicationSecondTranscriptReviewDate.Text = ""
            txtApplicationSecondTranscriptReviewUserId.Text = ""
            txtApplicationSecondTranscriptReviewDate.Visible = False
            txtApplicationSecondTranscriptReviewUserId.Visible = False
            chkApplicationSecondTranscriptReviewStartStop.Visible = True
        End If
    End Sub

    Private Sub chkApplicationSecondTranscriptReviewStartStop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationSecondTranscriptReviewStartStop.CheckedChanged
        If (chkApplicationSecondTranscriptReviewStartStop.Checked) Then
            DataAccess.StartReview(_student.StateStudentId(), _user.Id, ReviewType.SECOND_TRANSCRIPT)
            chkApplicationSecondTranscriptReviewStartStop.Text = "Stop"
            chkApplicationSecondTranscriptReviewStartStop.BackColor = Color.Red
        Else
            DataAccess.StopReview(_student.StateStudentId(), _user.Id, ReviewType.SECOND_TRANSCRIPT)
            chkApplicationSecondTranscriptReviewStartStop.Text = "Start"
            chkApplicationSecondTranscriptReviewStartStop.BackColor = Color.LightGreen
        End If
    End Sub

    Private Sub chkApplicationFinalTranscriptReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationFinalTranscriptReview.CheckedChanged
        If chkApplicationFinalTranscriptReview.Checked Then
            chkApplicationFinalTranscriptReviewStartStop.Checked = False
            chkApplicationFinalTranscriptReviewStartStop.Visible = False
            txtApplicationFinalTranscriptReviewDate.Text = DateTime.Now.ToShortDateString()
            txtApplicationFinalTranscriptReviewUserId.Text = _user.Id
            txtApplicationFinalTranscriptReviewDate.Visible = True
            txtApplicationFinalTranscriptReviewUserId.Visible = True

        Else
            txtApplicationFinalTranscriptReviewDate.Text = ""
            txtApplicationFinalTranscriptReviewUserId.Text = ""
            txtApplicationFinalTranscriptReviewDate.Visible = False
            txtApplicationFinalTranscriptReviewUserId.Visible = False
            chkApplicationFinalTranscriptReviewStartStop.Visible = True
        End If
    End Sub

    Private Sub chkApplicationFinalTranscriptReviewStartStop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationFinalTranscriptReviewStartStop.CheckedChanged
        If (chkApplicationFinalTranscriptReviewStartStop.Checked) Then
            DataAccess.StartReview(_student.StateStudentId(), _user.Id, ReviewType.FINAL_TRANSCRIPT)
            chkApplicationFinalTranscriptReviewStartStop.Text = "Stop"
            chkApplicationFinalTranscriptReviewStartStop.BackColor = Color.Red
        Else
            DataAccess.StopReview(_student.StateStudentId(), _user.Id, ReviewType.FINAL_TRANSCRIPT)
            chkApplicationFinalTranscriptReviewStartStop.Text = "Start"
            chkApplicationFinalTranscriptReviewStartStop.BackColor = Color.LightGreen
        End If
    End Sub

    Private Sub chkApplicationClassReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationClassReview.CheckedChanged
        If chkApplicationClassReview.Checked = True Then
            txtApplicationClassReviewDate.Text = Date.Now.ToShortDateString()
            txtApplicationClassReviewUserId.Text = _user.Id
            txtApplicationClassReviewDate.Visible = True
            txtApplicationClassReviewUserId.Visible = True

        Else
            txtApplicationClassReviewDate.Text = ""
            txtApplicationClassReviewUserId.Text = ""
            txtApplicationClassReviewDate.Visible = False
            txtApplicationClassReviewUserId.Visible = False
        End If
    End Sub

    Private Sub chkApplicationCategoryReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationCategoryReview.CheckedChanged
        If chkApplicationCategoryReview.Checked = True Then
            txtApplicationCategoryReviewDate.Text = Date.Now.ToShortDateString()
            txtApplicationCategoryReviewUserId.Text = _user.Id
            txtApplicationCategoryReviewDate.Visible = True
            txtApplicationCategoryReviewUserId.Visible = True

        Else
            txtApplicationCategoryReviewDate.Text = ""
            txtApplicationCategoryReviewUserId.Text = ""
            txtApplicationCategoryReviewDate.Visible = False
            txtApplicationCategoryReviewUserId.Visible = False
        End If
    End Sub

    Private Sub chkApplicationInitialAwardReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationInitialAwardReview.CheckedChanged
        If chkApplicationInitialAwardReview.Checked = True Then
            txtApplicationInitialAwardReviewDate.Text = Date.Now.ToShortDateString()
            txtApplicationInitialAwardReviewUserId.Text = _user.Id
            txtApplicationInitialAwardReviewDate.Visible = True
            txtApplicationInitialAwardReviewUserId.Visible = True

        Else
            txtApplicationInitialAwardReviewDate.Text = ""
            txtApplicationInitialAwardReviewUserId.Text = ""
            txtApplicationInitialAwardReviewDate.Visible = False
            txtApplicationInitialAwardReviewUserId.Visible = False
        End If
    End Sub

    Private Sub chkApplicationFirstQuickReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationFirstQuickReview.CheckedChanged
        If chkApplicationFirstQuickReview.Checked = True Then
            txtApplicationFirstQuickReviewDate.Text = Date.Now.ToShortDateString()
            txtApplicationFirstQuickReviewUserId.Text = _user.Id
            txtApplicationFirstQuickReviewDate.Visible = True
            txtApplicationFirstQuickReviewUserId.Visible = True
            lblApplicationFirstQuickReviewInProgress.Visible = False
        Else
            txtApplicationFirstQuickReviewDate.Text = ""
            txtApplicationFirstQuickReviewUserId.Text = ""
            txtApplicationFirstQuickReviewDate.Visible = False
            txtApplicationFirstQuickReviewUserId.Visible = False
        End If
    End Sub

    Private Sub chkApplicationBaseAwardReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationBaseAwardReview.CheckedChanged
        If chkApplicationBaseAwardReview.Checked = True Then
            txtApplicationBaseAwardReviewDate.Text = Date.Now.ToShortDateString()
            txtApplicationBaseAwardReviewUserId.Text = _user.Id
            txtApplicationBaseAwardReviewDate.Visible = True
            txtApplicationBaseAwardReviewUserId.Visible = True

        Else
            txtApplicationBaseAwardReviewDate.Text = ""
            txtApplicationBaseAwardReviewUserId.Text = ""
            txtApplicationBaseAwardReviewDate.Visible = False
            txtApplicationBaseAwardReviewUserId.Visible = False
        End If
    End Sub

    Private Sub chkApplicationExemplaryAwardReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationExemplaryAwardReview.CheckedChanged
        If chkApplicationExemplaryAwardReview.Checked = True Then
            txtApplicationExemplaryAwardReviewDate.Text = Date.Now.ToShortDateString()
            txtApplicationExemplaryAwardReviewUserId.Text = _user.Id
            txtApplicationExemplaryAwardReviewDate.Visible = True
            txtApplicationExemplaryAwardReviewUserId.Visible = True

        Else
            txtApplicationExemplaryAwardReviewDate.Text = ""
            txtApplicationExemplaryAwardReviewUserId.Text = ""
            txtApplicationExemplaryAwardReviewDate.Visible = False
            txtApplicationExemplaryAwardReviewUserId.Visible = False
        End If
    End Sub

    Private Sub chkApplicationUespAwardReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationUespAwardReview.CheckedChanged
        If chkApplicationUespAwardReview.Checked = True Then
            txtApplicationUespAwardReviewDate.Text = Date.Now.ToShortDateString()
            txtApplicationUespAwardReviewUserId.Text = _user.Id
            txtApplicationUespAwardReviewDate.Visible = True
            txtApplicationUespAwardReviewUserId.Visible = True
            lblApplicationUespAwardReviewInProgress.Visible = False
        Else
            txtApplicationUespAwardReviewDate.Text = ""
            txtApplicationUespAwardReviewUserId.Text = ""
            txtApplicationUespAwardReviewDate.Visible = False
            txtApplicationUespAwardReviewUserId.Visible = False
        End If
    End Sub

    Private Sub chkApplicationSecondQuickReview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplicationSecondQuickReview.CheckedChanged
        If chkApplicationSecondQuickReview.Checked = True Then
            txtApplicationSecondQuickReviewDate.Text = Date.Now.ToShortDateString()
            txtApplicationSecondQuickReviewUserId.Text = _user.Id
            txtApplicationSecondQuickReviewDate.Visible = True
            txtApplicationSecondQuickReviewUserId.Visible = True
            lblApplicationSecondQuickReviewInProgress.Visible = False
        Else
            txtApplicationSecondQuickReviewDate.Text = ""
            txtApplicationSecondQuickReviewUserId.Text = ""
            txtApplicationSecondQuickReviewDate.Visible = False
            txtApplicationSecondQuickReviewUserId.Visible = False
        End If
    End Sub
#End Region 'Reviews

    Private Sub cmbApplicationHighSchoolName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbApplicationHighSchoolName.SelectedIndexChanged
        Try
            If String.IsNullOrEmpty(cmbApplicationHighSchoolName.Text) Then Return

            'Get any high schools whose names exactly match the selected value.
            Dim highSchools As List(Of Lookups.School) = ( _
                From hsl In Lookups.Schools _
                Where hsl.Type = "HIGH" _
                Where hsl.Name = cmbApplicationHighSchoolName.Text _
                Select hsl _
            ).ToList()
            'Blank the CEEB code if no matches were found.
            If highSchools.Count = 0 Then
                txtApplicationCeebCode.Text = ""
                Return
            End If
            'Set the CEEB code if one match was found.
            If highSchools.Count = 1 Then
                txtApplicationCeebCode.Text = highSchools.Single().CeebCode
                txtApplicationHighSchoolCity.Text = highSchools.Single().City
                txtApplicationHighSchoolDistrict.Text = highSchools.Single().District
                'The new high school doesn't stick until you focus on another control, so force the focus to move.
                txtApplicationCeebCode.Focus()
                Return
            End If
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub txtApplicationCeebCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtApplicationCeebCode.TextChanged
        Try
            If txtApplicationCeebCode.Text.Length < 6 Then Return

            'Get the high school that matches the typed-in CEEB code.
            Dim highSchool As Lookups.School = Lookups.Schools.Where(Function(p) p.CeebCode = txtApplicationCeebCode.Text).SingleOrDefault()
            If highSchool Is Nothing Then Return

            'Set the high school name, city, and district text.
            cmbApplicationHighSchoolName.Text = highSchool.Name
            txtApplicationHighSchoolCity.Text = highSchool.City
            txtApplicationHighSchoolDistrict.Text = highSchool.District
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

#Region "Classes"
    'Make the class tabs stand out from the rest of the form by giving them a different color.
    Private Sub tabControlClasses_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles tabControlClasses.DrawItem
        'The background color is different for the selected tab versus the other tabs.
        Dim backBrush As SolidBrush = If(e.Index = Me.tabControlClasses.SelectedIndex, New SolidBrush(Color.SteelBlue), New SolidBrush(Color.SlateGray))
        Dim tabName As String = Me.tabControlClasses.TabPages(e.Index).Text

        'Fill the rectangle defined by the tab's boundaries with the back color.
        e.Graphics.FillRectangle(backBrush, e.Bounds)
        'Draw the tab name centered in the same rectangle.
        Using nameFormat As New StringFormat()
            nameFormat.Alignment = StringAlignment.Center
            Dim rect As New Rectangle(e.Bounds.X, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height - 3)
            'Use the default font, but make it bold with a custom color.
            Using tabFont As New Font(e.Font, FontStyle.Bold)
                Using foreBrush As New SolidBrush(Color.White)
                    e.Graphics.DrawString(tabName, tabFont, foreBrush, rect, nameFormat)
                End Using
            End Using
        End Using
        'backBrush needs to be disposed explicitly.
        backBrush.Dispose()
    End Sub

    Private Sub UpdateAcademicYear(ByVal classControl As ClassDataControl)
        If (Not IsNumeric(classControl.txtGradeLevel.Text)) Then Return

        'Update the academic year.
        Dim graduationYear As Integer = Integer.Parse(_student.ScholarshipApplication.ApplicationYear)
        If (_student.HighSchool.GraduationDate.HasValue) Then graduationYear = _student.HighSchool.GraduationDate.Value.Year
        Dim yearDifference As Integer = 12 - Integer.Parse(classControl.txtGradeLevel.Text)
        Dim academicYearEnd As Integer = graduationYear - yearDifference
        classControl.txtAcademicYear.Text = String.Format("{0:00}/{1:00}", (academicYearEnd - 1) Mod 100, academicYearEnd Mod 100)
    End Sub

#Region "English"
    Private Sub englishClass1_GradeLevelChanged(ByVal sender As ClassDataControl) Handles englishClass1.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub englishClass1_RadioButtonChanged() Handles englishClass1.RadioButtonChanged
        englishClass1.txtClassVerifiedBy.Text = _user.Id()
        englishClass1.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub englishClass1_ClassWeightChanged(ByVal weight As String) Handles englishClass1.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            englishClass1.cmbConcurrentCollege.Enabled = True
        Else
            englishClass1.cmbConcurrentCollege.Enabled = False
            englishClass1.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub englishClass2_GradeLevelChanged(ByVal sender As ClassDataControl) Handles englishClass2.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub englishClass2_RadioButtonChanged() Handles englishClass2.RadioButtonChanged
        englishClass2.txtClassVerifiedBy.Text = _user.Id()
        englishClass2.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub englishClass2_ClassWeightChanged(ByVal weight As String) Handles englishClass2.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            englishClass2.cmbConcurrentCollege.Enabled = True
        Else
            englishClass2.cmbConcurrentCollege.Enabled = False
            englishClass2.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub englishClass3_GradeLevelChanged(ByVal sender As ClassDataControl) Handles englishClass3.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub englishClass3_RadioButtonChanged() Handles englishClass3.RadioButtonChanged
        englishClass3.txtClassVerifiedBy.Text = _user.Id()
        englishClass3.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub englishClass3_ClassWeightChanged(ByVal weight As String) Handles englishClass3.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            englishClass3.cmbConcurrentCollege.Enabled = True
        Else
            englishClass3.cmbConcurrentCollege.Enabled = False
            englishClass3.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub englishClass4_GradeLevelChanged(ByVal sender As ClassDataControl) Handles englishClass4.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub englishClass4_RadioButtonChanged() Handles englishClass4.RadioButtonChanged
        englishClass4.txtClassVerifiedBy.Text = _user.Id()
        englishClass4.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub englishClass4_ClassWeightChanged(ByVal weight As String) Handles englishClass4.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            englishClass4.cmbConcurrentCollege.Enabled = True
        Else
            englishClass4.cmbConcurrentCollege.Enabled = False
            englishClass4.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub englishClass5_GradeLevelChanged(ByVal sender As ClassDataControl) Handles englishClass5.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub englishClass5_RadioButtonChanged() Handles englishClass5.RadioButtonChanged
        englishClass5.txtClassVerifiedBy.Text = _user.Id()
        englishClass5.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub englishClass5_ClassWeightChanged(ByVal weight As String) Handles englishClass5.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            englishClass5.cmbConcurrentCollege.Enabled = True
        Else
            englishClass5.cmbConcurrentCollege.Enabled = False
            englishClass5.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub englishClass6_GradeLevelChanged(ByVal sender As ClassDataControl) Handles englishClass6.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub englishClass6_RadioButtonChanged() Handles englishClass6.RadioButtonChanged
        englishClass6.txtClassVerifiedBy.Text = _user.Id()
        englishClass6.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub englishClass6_ClassWeightChanged(ByVal weight As String) Handles englishClass6.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            englishClass6.cmbConcurrentCollege.Enabled = True
        Else
            englishClass6.cmbConcurrentCollege.Enabled = False
            englishClass6.cmbConcurrentCollege.Text = ""
        End If
    End Sub
#End Region ' English
#Region "Math"
    Private Sub mathClass1_GradeLevelChanged(ByVal sender As ClassDataControl) Handles mathematicsClass1.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub mathematicsClass1_RadioButtonChanged() Handles mathematicsClass1.RadioButtonChanged
        mathematicsClass1.txtClassVerifiedBy.Text = _user.Id()
        mathematicsClass1.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub mathClass1_ClassWeightChanged(ByVal weight As String) Handles mathematicsClass1.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            mathematicsClass1.cmbConcurrentCollege.Enabled = True
        Else
            mathematicsClass1.cmbConcurrentCollege.Enabled = False
            mathematicsClass1.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub mathClass2_GradeLevelChanged(ByVal sender As ClassDataControl) Handles mathematicsClass2.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub mathematicsClass2_RadioButtonChanged() Handles mathematicsClass2.RadioButtonChanged
        mathematicsClass2.txtClassVerifiedBy.Text = _user.Id()
        mathematicsClass2.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub mathClass2_ClassWeightChanged(ByVal weight As String) Handles mathematicsClass2.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            mathematicsClass2.cmbConcurrentCollege.Enabled = True
        Else
            mathematicsClass2.cmbConcurrentCollege.Enabled = False
            mathematicsClass2.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub mathClass3_GradeLevelChanged(ByVal sender As ClassDataControl) Handles mathematicsClass3.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub mathematicsClass3_RadioButtonChanged() Handles mathematicsClass3.RadioButtonChanged
        mathematicsClass3.txtClassVerifiedBy.Text = _user.Id()
        mathematicsClass3.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub mathClass3_ClassWeightChanged(ByVal weight As String) Handles mathematicsClass3.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            mathematicsClass3.cmbConcurrentCollege.Enabled = True
        Else
            mathematicsClass3.cmbConcurrentCollege.Enabled = False
            mathematicsClass3.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub mathClass4_GradeLevelChanged(ByVal sender As ClassDataControl) Handles mathematicsClass4.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub mathematicsClass4_RadioButtonChanged() Handles mathematicsClass4.RadioButtonChanged
        mathematicsClass4.txtClassVerifiedBy.Text = _user.Id()
        mathematicsClass4.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub mathClass4_ClassWeightChanged(ByVal weight As String) Handles mathematicsClass4.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            mathematicsClass4.cmbConcurrentCollege.Enabled = True
        Else
            mathematicsClass4.cmbConcurrentCollege.Enabled = False
            mathematicsClass4.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub mathClass5_GradeLevelChanged(ByVal sender As ClassDataControl) Handles mathematicsClass5.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub mathematicsClass5_RadioButtonChanged() Handles mathematicsClass5.RadioButtonChanged
        mathematicsClass5.txtClassVerifiedBy.Text = _user.Id()
        mathematicsClass5.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub mathClass5_ClassWeightChanged(ByVal weight As String) Handles mathematicsClass5.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            mathematicsClass5.cmbConcurrentCollege.Enabled = True
        Else
            mathematicsClass5.cmbConcurrentCollege.Enabled = False
            mathematicsClass5.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub mathClass6_GradeLevelChanged(ByVal sender As ClassDataControl) Handles mathematicsClass6.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub mathematicsClass6_RadioButtonChanged() Handles mathematicsClass6.RadioButtonChanged
        mathematicsClass6.txtClassVerifiedBy.Text = _user.Id()
        mathematicsClass6.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub mathClass6_ClassWeightChanged(ByVal weight As String) Handles mathematicsClass6.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            mathematicsClass6.cmbConcurrentCollege.Enabled = True
        Else
            mathematicsClass6.cmbConcurrentCollege.Enabled = False
            mathematicsClass6.cmbConcurrentCollege.Text = ""
        End If
    End Sub
#End Region 'Math
#Region "Science"
    Private Sub scienceClass1_GradeLevelChanged(ByVal sender As ClassDataControl) Handles scienceClass1.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub scienceClass1_RadioButtonChanged() Handles scienceClass1.RadioButtonChanged
        scienceClass1.txtClassVerifiedBy.Text = _user.Id()
        scienceClass1.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub scienceClass1_ClassWeightChanged(ByVal weight As String) Handles scienceClass1.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            scienceClass1.cmbConcurrentCollege.Enabled = True
        Else
            scienceClass1.cmbConcurrentCollege.Enabled = False
            scienceClass1.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub scienceClass2_GradeLevelChanged(ByVal sender As ClassDataControl) Handles scienceClass2.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub scienceClass2_RadioButtonChanged() Handles scienceClass2.RadioButtonChanged
        scienceClass2.txtClassVerifiedBy.Text = _user.Id()
        scienceClass2.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub scienceClass2_ClassWeightChanged(ByVal weight As String) Handles scienceClass2.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            scienceClass2.cmbConcurrentCollege.Enabled = True
        Else
            scienceClass2.cmbConcurrentCollege.Enabled = False
            scienceClass2.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub scienceClass3_GradeLevelChanged(ByVal sender As ClassDataControl) Handles scienceClass3.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub scienceClass3_RadioButtonChanged() Handles scienceClass3.RadioButtonChanged
        scienceClass3.txtClassVerifiedBy.Text = _user.Id()
        scienceClass3.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub scienceClass3_ClassWeightChanged(ByVal weight As String) Handles scienceClass3.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            scienceClass3.cmbConcurrentCollege.Enabled = True
        Else
            scienceClass3.cmbConcurrentCollege.Enabled = False
            scienceClass3.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub scienceClass4_GradeLevelChanged(ByVal sender As ClassDataControl) Handles scienceClass4.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub scienceClass4_RadioButtonChanged() Handles scienceClass4.RadioButtonChanged
        scienceClass4.txtClassVerifiedBy.Text = _user.Id()
        scienceClass4.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub scienceClass4_ClassWeightChanged(ByVal weight As String) Handles scienceClass4.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            scienceClass4.cmbConcurrentCollege.Enabled = True
        Else
            scienceClass4.cmbConcurrentCollege.Enabled = False
            scienceClass4.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub scienceClass5_GradeLevelChanged(ByVal sender As ClassDataControl) Handles scienceClass5.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub scienceClass5_RadioButtonChanged() Handles scienceClass5.RadioButtonChanged
        scienceClass5.txtClassVerifiedBy.Text = _user.Id()
        scienceClass5.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub scienceClass5_ClassWeightChanged(ByVal weight As String) Handles scienceClass5.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            scienceClass5.cmbConcurrentCollege.Enabled = True
        Else
            scienceClass5.cmbConcurrentCollege.Enabled = False
            scienceClass5.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub scienceClass6_GradeLevelChanged(ByVal sender As ClassDataControl) Handles scienceClass6.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub scienceClass6_RadioButtonChanged() Handles scienceClass6.RadioButtonChanged
        scienceClass6.txtClassVerifiedBy.Text = _user.Id()
        scienceClass6.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub scienceClass6_ClassWeightChanged(ByVal weight As String) Handles scienceClass6.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            scienceClass6.cmbConcurrentCollege.Enabled = True
        Else
            scienceClass6.cmbConcurrentCollege.Enabled = False
            scienceClass6.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub scienceClass7_GradeLevelChanged(ByVal sender As ClassDataControl) Handles scienceClass7.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub scienceClass7_RadioButtonChanged() Handles scienceClass7.RadioButtonChanged
        scienceClass7.txtClassVerifiedBy.Text = _user.Id()
        scienceClass7.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub scienceClass7_ClassWeightChanged(ByVal weight As String) Handles scienceClass7.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            scienceClass7.cmbConcurrentCollege.Enabled = True
        Else
            scienceClass7.cmbConcurrentCollege.Enabled = False
            scienceClass7.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub scienceClass8_GradeLevelChanged(ByVal sender As ClassDataControl) Handles scienceClass8.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub scienceClass8_RadioButtonChanged() Handles scienceClass8.RadioButtonChanged
        scienceClass8.txtClassVerifiedBy.Text = _user.Id()
        scienceClass8.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub scienceClass8_ClassWeightChanged(ByVal weight As String) Handles scienceClass8.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            scienceClass8.cmbConcurrentCollege.Enabled = True
        Else
            scienceClass8.cmbConcurrentCollege.Enabled = False
            scienceClass8.cmbConcurrentCollege.Text = ""
        End If
    End Sub
#End Region 'Science
#Region "Social Science"
    Private Sub socialScienceClass1_GradeLevelChanged(ByVal sender As ClassDataControl) Handles socialScienceClass1.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub socialScienceClass1_RadioButtonChanged() Handles socialScienceClass1.RadioButtonChanged
        socialScienceClass1.txtClassVerifiedBy.Text = _user.Id()
        socialScienceClass1.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub socialScienceClass1_ClassWeightChanged(ByVal weight As String) Handles socialScienceClass1.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            socialScienceClass1.cmbConcurrentCollege.Enabled = True
        Else
            socialScienceClass1.cmbConcurrentCollege.Enabled = False
            socialScienceClass1.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub socialScienceClass2_GradeLevelChanged(ByVal sender As ClassDataControl) Handles socialScienceClass2.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub socialScienceClass2_RadioButtonChanged() Handles socialScienceClass2.RadioButtonChanged
        socialScienceClass2.txtClassVerifiedBy.Text = _user.Id()
        socialScienceClass2.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub socialScienceClass2_ClassWeightChanged(ByVal weight As String) Handles socialScienceClass2.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            socialScienceClass2.cmbConcurrentCollege.Enabled = True
        Else
            socialScienceClass2.cmbConcurrentCollege.Enabled = False
            socialScienceClass2.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub socialScienceClass3_GradeLevelChanged(ByVal sender As ClassDataControl) Handles socialScienceClass3.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub socialScienceClass3_RadioButtonChanged() Handles socialScienceClass3.RadioButtonChanged
        socialScienceClass3.txtClassVerifiedBy.Text = _user.Id()
        socialScienceClass3.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub socialScienceClass3_ClassWeightChanged(ByVal weight As String) Handles socialScienceClass3.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            socialScienceClass3.cmbConcurrentCollege.Enabled = True
        Else
            socialScienceClass3.cmbConcurrentCollege.Enabled = False
            socialScienceClass3.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub socialScienceClass4_GradeLevelChanged(ByVal sender As ClassDataControl) Handles socialScienceClass4.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub socialScienceClass4_RadioButtonChanged() Handles socialScienceClass4.RadioButtonChanged
        socialScienceClass4.txtClassVerifiedBy.Text = _user.Id()
        socialScienceClass4.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub socialScienceClass4_ClassWeightChanged(ByVal weight As String) Handles socialScienceClass4.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            socialScienceClass4.cmbConcurrentCollege.Enabled = True
        Else
            socialScienceClass4.cmbConcurrentCollege.Enabled = False
            socialScienceClass4.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub socialScienceClass5_GradeLevelChanged(ByVal sender As ClassDataControl) Handles socialScienceClass5.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub socialScienceClass5_RadioButtonChanged() Handles socialScienceClass5.RadioButtonChanged
        socialScienceClass5.txtClassVerifiedBy.Text = _user.Id()
        socialScienceClass5.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub socialScienceClass5_ClassWeightChanged(ByVal weight As String) Handles socialScienceClass5.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            socialScienceClass5.cmbConcurrentCollege.Enabled = True
        Else
            socialScienceClass5.cmbConcurrentCollege.Enabled = False
            socialScienceClass5.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub socialScienceClass6_GradeLevelChanged(ByVal sender As ClassDataControl) Handles socialScienceClass6.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub socialScienceClass6_RadioButtonChanged() Handles socialScienceClass6.RadioButtonChanged
        socialScienceClass6.txtClassVerifiedBy.Text = _user.Id()
        socialScienceClass6.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub socialScienceClass6_ClassWeightChanged(ByVal weight As String) Handles socialScienceClass6.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            socialScienceClass6.cmbConcurrentCollege.Enabled = True
        Else
            socialScienceClass6.cmbConcurrentCollege.Enabled = False
            socialScienceClass6.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub socialScienceClass7_GradeLevelChanged(ByVal sender As ClassDataControl) Handles socialScienceClass7.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub socialScienceClass7_RadioButtonChanged() Handles socialScienceClass7.RadioButtonChanged
        socialScienceClass7.txtClassVerifiedBy.Text = _user.Id()
        socialScienceClass7.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub socialScienceClass7_ClassWeightChanged(ByVal weight As String) Handles socialScienceClass7.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            socialScienceClass7.cmbConcurrentCollege.Enabled = True
        Else
            socialScienceClass7.cmbConcurrentCollege.Enabled = False
            socialScienceClass7.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub socialScienceClass8_GradeLevelChanged(ByVal sender As ClassDataControl) Handles socialScienceClass8.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub socialScienceClass8_RadioButtonChanged() Handles socialScienceClass8.RadioButtonChanged
        socialScienceClass8.txtClassVerifiedBy.Text = _user.Id()
        socialScienceClass8.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub socialScienceClass8_ClassWeightChanged(ByVal weight As String) Handles socialScienceClass8.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            socialScienceClass8.cmbConcurrentCollege.Enabled = True
        Else
            socialScienceClass8.cmbConcurrentCollege.Enabled = False
            socialScienceClass8.cmbConcurrentCollege.Text = ""
        End If
    End Sub
#End Region 'Social Science
#Region "Foreign Language"
    Private Sub foreignLanguageClass1_GradeLevelChanged(ByVal sender As ClassDataControl) Handles foreignLanguageClass1.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub foreignLanguageClass1_RadioButtonChanged() Handles foreignLanguageClass1.RadioButtonChanged
        foreignLanguageClass1.txtClassVerifiedBy.Text = _user.Id()
        foreignLanguageClass1.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub foreignLanguageClass1_ClassWeightChanged(ByVal weight As String) Handles foreignLanguageClass1.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            foreignLanguageClass1.cmbConcurrentCollege.Enabled = True
        Else
            foreignLanguageClass1.cmbConcurrentCollege.Enabled = False
            foreignLanguageClass1.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub foreignLanguageClass2_GradeLevelChanged(ByVal sender As ClassDataControl) Handles foreignLanguageClass2.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub foreignLanguageClass2_RadioButtonChanged() Handles foreignLanguageClass2.RadioButtonChanged
        foreignLanguageClass2.txtClassVerifiedBy.Text = _user.Id()
        foreignLanguageClass2.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub foreignLanguageClass2_ClassWeightChanged(ByVal weight As String) Handles foreignLanguageClass2.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            foreignLanguageClass2.cmbConcurrentCollege.Enabled = True
        Else
            foreignLanguageClass2.cmbConcurrentCollege.Enabled = False
            foreignLanguageClass2.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub foreignLanguageClass3_GradeLevelChanged(ByVal sender As ClassDataControl) Handles foreignLanguageClass3.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub foreignLanguageClass3_RadioButtonChanged() Handles foreignLanguageClass3.RadioButtonChanged
        foreignLanguageClass3.txtClassVerifiedBy.Text = _user.Id()
        foreignLanguageClass3.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub foreignLanguageClass3_ClassWeightChanged(ByVal weight As String) Handles foreignLanguageClass3.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            foreignLanguageClass3.cmbConcurrentCollege.Enabled = True
        Else
            foreignLanguageClass3.cmbConcurrentCollege.Enabled = False
            foreignLanguageClass3.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub foreignLanguageClass4_GradeLevelChanged(ByVal sender As ClassDataControl) Handles foreignLanguageClass4.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub foreignLanguageClass4_RadioButtonChanged() Handles foreignLanguageClass4.RadioButtonChanged
        foreignLanguageClass4.txtClassVerifiedBy.Text = _user.Id()
        foreignLanguageClass4.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub foreignLanguageClass4_ClassWeightChanged(ByVal weight As String) Handles foreignLanguageClass4.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            foreignLanguageClass4.cmbConcurrentCollege.Enabled = True
        Else
            foreignLanguageClass4.cmbConcurrentCollege.Enabled = False
            foreignLanguageClass4.cmbConcurrentCollege.Text = ""
        End If
    End Sub

    Private Sub foreignLanguageClass5_GradeLevelChanged(ByVal sender As ClassDataControl) Handles foreignLanguageClass5.GradeLevelChanged
        UpdateAcademicYear(sender)
    End Sub

    Private Sub foreignLanguageClass5_RadioButtonChanged() Handles foreignLanguageClass5.RadioButtonChanged
        foreignLanguageClass5.txtClassVerifiedBy.Text = _user.Id()
        foreignLanguageClass5.txtClassVerifiedDate.Text = Date.Now.ToShortDateString()
    End Sub

    Private Sub foreignLanguageClass5_ClassWeightChanged(ByVal weight As String) Handles foreignLanguageClass5.ClassWeightChanged
        If (weight = Constants.ClassWeight.CE) Then
            foreignLanguageClass5.cmbConcurrentCollege.Enabled = True
        Else
            foreignLanguageClass5.cmbConcurrentCollege.Enabled = False
            foreignLanguageClass5.cmbConcurrentCollege.Text = ""
        End If
    End Sub
#End Region 'Foreign Language
#End Region 'Classes
#End Region 'Application tab

#Region "Communications tab"
    Private Sub btnCommunicationClearFields_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommunicationClearFields.Click
        ClearCommunicationFields()
    End Sub

    Private Sub btnCommunicationSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommunicationSave.Click
        If (txtCommunicationSubject.TextLength = 0 OrElse txtCommunicationComments.TextLength = 0) Then
            MessageBox.Show("A subject and text are needed.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If

        Try
            Dim comm As New Communication()
            comm.TimeStamp = DateTime.Now
            comm.EntityID = _student.StateStudentId
            comm.Source = cmbCommunicationSource.Text
            comm.Type = cmbCommunicationType.Text
            comm.UserId = _user.Id
            comm.Subject = txtCommunicationSubject.Text
            comm.Text = txtCommunicationComments.Text
            comm.EntityType = RegentsScholarshipBackEnd.Constants.CommunicationEntityType.STUDENT
            comm.Is411 = chkCommunications411.Checked
            DataAccess.SetCommunication(comm)
            MessageBox.Show("New comment added", "New Comment Added", MessageBoxButtons.OK)
            ClearCommunicationFields()
            LoadThe411()
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
        LoadComments(_student.StateStudentId)
    End Sub

    Private Sub btnCommunicationLinkDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommunicationLinkDocument.Click
        Try
            LinkDocument(Screen.Communication)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnCommunicationViewDocuments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommunicationViewDocuments.Click
        Try
            FetchDocument(Screen.Communication)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub chkCommunications411_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCommunications411.Click
        'Ignore it if there's not a communication record loaded.
        If (txtCommunicationDateTime.TextLength = 0) Then Return

        Try
            'Update the communication record in the database.
            Dim comm As New Communication()
            comm.TimeStamp = DateTime.Parse(txtCommunicationDateTime.Text)
            comm.EntityID = _student.StateStudentId
            comm.Source = cmbCommunicationSource.Text
            comm.Type = cmbCommunicationType.Text
            comm.UserId = _user.Id
            comm.Subject = txtCommunicationSubject.Text
            comm.Text = txtCommunicationComments.Text
            comm.EntityType = RegentsScholarshipBackEnd.Constants.CommunicationEntityType.STUDENT
            comm.Is411 = chkCommunications411.Checked
            DataAccess.SetCommunication(comm)
            'Reflect the change in the DataGridView and update the 411 panel on the Demographics tab.
            CommunicationDataGridView.SelectedRows(0).Cells("Is411").Value = chkCommunications411.Checked
            LoadThe411()
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub CommunicationDataGridView_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles CommunicationDataGridView.CellContentClick
        Try
            If e.RowIndex = -1 Then
                Return
            End If
            txtCommunicationUserId.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("UserId").Value
            txtCommunicationDateTime.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmTimeStamp").Value
            cmbCommunicationType.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmType").Value
            cmbCommunicationSource.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmSource").Value
            txtCommunicationSubject.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmSubject").Value
            txtCommunicationComments.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmText").Value
            chkCommunications411.Checked = CommunicationDataGridView.Rows(e.RowIndex).Cells("Is411").Value
            btnCommunicationSave.Enabled = False
            chkCommunications411.Enabled = (_user.AccessLevel = RegentsScholarshipBackEnd.Constants.AccessLevel.DCR)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub CommunicationDataGridView_ColumnHeaderMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles CommunicationDataGridView.ColumnHeaderMouseClick
        Try
            'Declare some static booleans to track the sort order for each column.
            Static userIDIsAscending As Boolean = False
            Static studentIDIsAscending As Boolean = False
            Static timeStampIsAscending As Boolean = False
            Static typeIsAscending As Boolean = False
            Static sourceIsAscending As Boolean = False
            Static subjectIsAscending As Boolean = False
            Static textIsAscending As Boolean = False

            'Based on the clicked column, set all the static booleans and call SortedSearch.
            Select Case e.ColumnIndex
                Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("UserID"))
                    userIDIsAscending = Not userIDIsAscending
                    studentIDIsAscending = False
                    timeStampIsAscending = False
                    typeIsAscending = False
                    sourceIsAscending = False
                    subjectIsAscending = False
                    CommSortedSearch("UserId", userIDIsAscending)
                Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmTimeStamp"))
                    userIDIsAscending = False
                    studentIDIsAscending = False
                    timeStampIsAscending = Not timeStampIsAscending
                    typeIsAscending = False
                    sourceIsAscending = False
                    subjectIsAscending = False
                    CommSortedSearch("TimeStamp", timeStampIsAscending)
                Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmType"))
                    userIDIsAscending = False
                    studentIDIsAscending = False
                    timeStampIsAscending = False
                    typeIsAscending = Not typeIsAscending
                    sourceIsAscending = False
                    subjectIsAscending = False
                    CommSortedSearch("Type", typeIsAscending)
                Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmSource"))
                    userIDIsAscending = False
                    studentIDIsAscending = False
                    timeStampIsAscending = False
                    typeIsAscending = False
                    sourceIsAscending = Not sourceIsAscending
                    subjectIsAscending = False
                    CommSortedSearch("Source", sourceIsAscending)
                Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmSubject"))
                    userIDIsAscending = False
                    studentIDIsAscending = False
                    timeStampIsAscending = False
                    typeIsAscending = False
                    sourceIsAscending = False
                    subjectIsAscending = Not subjectIsAscending
                    CommSortedSearch("Subject", subjectIsAscending)
                Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmText"))
                    userIDIsAscending = False
                    studentIDIsAscending = False
                    timeStampIsAscending = False
                    typeIsAscending = False
                    sourceIsAscending = False
                    subjectIsAscending = False
                    textIsAscending = Not textIsAscending
                    CommSortedSearch("Comments", textIsAscending)
            End Select
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub
#End Region 'Communications tab

#Region "Payments tab"
    Private Sub btnPaymentsLinkDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentsLinkDocument.Click
        Try
            LinkDocument(Screen.Payments)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnPaymentsNewPayment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentsNewPayment.Click
        Try
            Dim paymentType As Payment.AwardType = Payment.AwardType.Base
            If (_student.ScholarshipApplication.ExemplaryAward.IsApproved) Then paymentType = Payment.AwardType.Exemplary
            Dim newPayment As New Payment(_student, paymentType)
            _student.Payments.Add(newPayment)
            LoadPayments()
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnPaymentsSaveChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentsSaveChanges.Click
        Try
            SaveStudent(Student.Component.Payments)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub btnPaymentsViewDocuments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentsViewDocuments.Click
        Try
            FetchDocument(Screen.Payments)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub
#End Region 'Payments tab

#Region "Payment batch tab"
    Private Sub btnPaymentBatchFinal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentBatchFinal.Click
        Try
            Dim batchNumber As String = DateTime.Now.ToString("yyyy-MM-dd.hh:mm")
            lblPaymentBatchNumber.Visible = True
            lblPaymentBatchNumber.Text = "Batch number " + batchNumber
            txtPaymentBatchLog.Clear()
            Application.DoEvents()
            Cursor = Cursors.WaitCursor
            _paymentReviewer.RunFinalReview(_user.Id, batchNumber)
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnPaymentBatchPreliminary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentBatchPreliminary.Click
        Try
            lblPaymentBatchNumber.Visible = False
            txtPaymentBatchLog.Clear()
            Cursor = Cursors.WaitCursor
            _paymentReviewer.RunPreliminaryReview()
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub LogMessageHandler(ByVal sender As System.Object, ByVal e As PaymentReviews.LogMessageEventArgs) Handles _paymentReviewer.LogMessage
        txtPaymentBatchLog.AppendText(e.Message)
        txtPaymentBatchLog.AppendText(System.Environment.NewLine)
    End Sub
#End Region 'Payment batch tab

    Private Sub frmRegents_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            ReleaseRecordLock()
            Application.Exit()
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub frmRegents_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Have the title bar indicate when we're in test mode.
            If RegentsScholarshipBackEnd.Constants.TEST_MODE Then
                Me.Text += " -- TEST MODE"
            End If
            'Disable and hide items that will crash the program if a student isn't loaded.
            btnDemographicsSaveChanges.Enabled = False
            btnDemographicsSaveChanges.Visible = False
            btnApplicationSaveChanges.Enabled = False
            btnApplicationSaveChanges.Visible = False
            btnCommunicationSave.Enabled = False
            btnCommunicationSave.Visible = False
            btnCommunicationClearFields.Enabled = False
            btnCommunicationClearFields.Visible = False
            btnCommunicationLinkDocument.Enabled = False
            btnCommunicationLinkDocument.Visible = False
            btnCommunicationViewDocuments.Enabled = False
            btnCommunicationViewDocuments.Visible = False
            btnPaymentsSaveChanges.Enabled = False
            btnPaymentsSaveChanges.Visible = False
            btnPaymentsNewPayment.Enabled = False
            btnPaymentsNewPayment.Visible = False
            'Set the data sources for combo boxes that need special loading.
            LoadComboBoxDataSources()
            'Set the form up for a search.
            ClearSearchResults()
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub

    Private Sub tabControlMaster_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabControlMaster.SelectedIndexChanged
        Try
            'Release any locks if we're going to the main menu tab; check/get a lock otherwise.
            Select Case tabControlMaster.SelectedIndex
                Case tabControlMaster.TabPages.IndexOfKey("tabMainMenu")
                    ReleaseRecordLock()
                    ClearSearchCriteria()
                Case tabControlMaster.TabPages.IndexOfKey("tabCommunications")
                    ObtainRecordLock()
                    'The Communications tab's Save button tends to re-enable itself
                    'when returning to that tab, so fix it if needed.
                    btnCommunicationSave.Enabled = (txtCommunicationDateTime.TextLength = 0 AndAlso txtCommunicationSubject.Enabled)
                Case Else
                    ObtainRecordLock()
            End Select
        Catch ex As Exception
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        End Try
    End Sub
#End Region 'Event handlers

#Region "Special data binding"
#Region "Loading"
#Region "Combo box data sources"
    Private Sub LoadComboBoxDataSources()
        'Set the data sources for the drop-downs that don't need a blank item.
        cmbApplicationAwardStatus.DataSource = Lookups.AwardStatuses
        cmbApplicationHighSchoolName.DataSource = Lookups.Schools.Where(Function(p) p.Type = "HIGH").Select(Function(p) p.Name).Distinct().OrderBy(Function(p) p).ToList()
        cmbApplicationTerm.DataSource = Lookups.CollegeTerms
        cmbDemographicsEthnicity.DataSource = Lookups.Ethnicities.Where(Function(p) p.IsInDefaultList = True).Select(Function(p) p.Description).ToList()
        cmbDemographicsGender.DataSource = Lookups.Genders
        cmbDemographicsState.DataSource = Lookups.States.Select(Function(p) p.Abbreviation).ToList()
        cmbCommunicationType.DataSource = Lookups.CommunicationTypes
        cmbCommunicationSource.DataSource = Lookups.CommunicationSources
        SchoolOption.PopulateSchoolDropDown(cmbApplication9thGradeSchoolName)

        'Set the leave and deferment reason combo items.
        cmbApplicationDefermentReason.Items.AddRange(Lookups.LeaveDeferralReasons.ToArray())
        cmbApplicationLeaveOfAbsenceReason.Items.AddRange(Lookups.LeaveDeferralReasons.ToArray())
        'Add a blank item to the top of the list.
        cmbApplicationDefermentReason.Items.Insert(0, "")
        cmbApplicationLeaveOfAbsenceReason.Items.Insert(0, "")

        'Set the denial reason combo items.
        For Each combo As ComboBox In grpApplicationDenialReasons.Controls
            combo.Items.AddRange(Lookups.DenialReasons.ToArray())
            'Add a blank item to the top of the list.
            combo.Items.Insert(0, "")
        Next

        'Set the college name combo items to college names that are marked as being in the default list.
        cmbApplicationCollegeName.Items.AddRange(Lookups.Colleges.Where(Function(p) p.IsInDefaultList = True).Select(Function(p) p.Name).ToArray())
        'Add a blank item to the top of the list.
        cmbApplicationCollegeName.Items.Insert(0, "")

        'Call a recursive function to find and load all the grade, course weight, and concurrent enrollment college combos.
        LoadClassLevelCombos(grpApplicationClasses)
    End Sub

    Private Sub LoadClassTitles(ByVal tab As TabPage, ByVal category As String)
        'Get the list of class titles that should appear in the drop-downs.
        Dim classTitles As IEnumerable(Of String) = ( _
            From ctl In Lookups.ClassTitles _
            Where ctl.Type = category _
            Where ctl.IsInApprovedList _
            Where (String.IsNullOrEmpty(ctl.ConditionalSchoolCode)) _
            Select ctl.Title _
        ).OrderBy(Function(p) p)


        'Pick out the class data custom controls and set their class title data source.
        For Each ctrl As Control In tab.Controls
            If (TypeOf ctrl Is ClassDataControl) Then
                Dim cdc As ClassDataControl = CType(ctrl, ClassDataControl)
                cdc.cmbTitle.Items.Clear()
                cdc.cmbTitle.Items.AddRange(classTitles.ToArray())
                'Put a blank item at the top of the list.
                cdc.cmbTitle.Items.Insert(0, "")

                'The class weight combo should include IB only for 2009 applicants, so
                'load it here in the function that only gets called when a student is loaded.
                ChangeWeightDesignationBasedOffAppYear(cdc)
            End If
        Next
    End Sub

    Private Sub ChangeWeightDesignationBasedOffAppYear(ByVal cdc As ClassDataControl)
        If _student.ScholarshipApplication.ApplicationYear = "2009" Then
            cdc.cmbWeightDesignation.DataSource = Lookups.ClassWeights.OrderBy(Function(p) p).ToList()
        Else
            cdc.cmbWeightDesignation.DataSource = Lookups.ClassWeights.Where(Function(p) p.Contains("IB") = False).OrderBy(Function(p) p).ToList()
        End If
    End Sub

    Private Sub LoadClassLevelCombos(ByVal container As Control)
        Dim colleges As IEnumerable(Of String) = Lookups.Colleges.Where(Function(p) p.IsInDefaultList).Select(Function(p) p.Name).OrderBy(Function(p) p)
        For Each ctrl As Control In container.Controls
            If (TypeOf ctrl Is ClassDataControl) Then
                'This is a class data custom control. Set the grade and weight combo box data sources.
                Dim cdc As ClassDataControl = CType(ctrl, ClassDataControl)
                For Each combo As ComboBox In New ComboBox() {cdc.cmbTerm1Grade, cdc.cmbTerm2Grade, cdc.cmbTerm3Grade, cdc.cmbTerm4Grade, cdc.cmbTerm5Grade, cdc.cmbTerm6Grade}
                    combo.Items.AddRange(Lookups.Grades.Select(Function(p) p.Letter).ToArray())
                    combo.Items.Insert(0, "")
                Next
                SchoolOption.PopulateSchoolDropDown(cdc.cmbSchoolAttended)
                cdc.cmbConcurrentCollege.Items.AddRange(colleges.ToArray())
                cdc.cmbConcurrentCollege.Items.Insert(0, "")
            ElseIf ctrl.Controls.Count > 0 Then
                'This is not a combo box. Recurse into its children if there are any.
                LoadClassLevelCombos(ctrl)
            End If
        Next
    End Sub
#End Region 'Combo box data sources

    Private Sub SetUpBindingSources()
        'Set the data sources for the widgets and add them to the list of two-way binding sources.
        _twoWayBindingSources = New List(Of BindingSource)()
        CellPhoneBindingSource.DataSource = _student.ContactInfo.CellPhone
        _twoWayBindingSources.Add(CellPhoneBindingSource)
        CollegeBindingSource.DataSource = _student.College
        _twoWayBindingSources.Add(CollegeBindingSource)
        ExemplaryAwardBindingSource.DataSource = _student.ScholarshipApplication.ExemplaryAward
        _twoWayBindingSources.Add(ExemplaryAwardBindingSource)
        HighSchoolBindingSource.DataSource = _student.HighSchool
        _twoWayBindingSources.Add(HighSchoolBindingSource)
        MailingAddressBindingSource.DataSource = _student.ContactInfo.HomeAddress
        _twoWayBindingSources.Add(MailingAddressBindingSource)
        PersonalEmailBindingSource.DataSource = _student.ContactInfo.PersonalEmail
        _twoWayBindingSources.Add(PersonalEmailBindingSource)
        PrimaryAwardBindingSource.DataSource = _student.ScholarshipApplication.BaseAward
        _twoWayBindingSources.Add(PrimaryAwardBindingSource)
        PrimaryPhoneBindingSource.DataSource = _student.ContactInfo.PrimaryPhone
        _twoWayBindingSources.Add(PrimaryPhoneBindingSource)
        SchoolEmailBindingSource.DataSource = _student.ContactInfo.SchoolEmail
        _twoWayBindingSources.Add(SchoolEmailBindingSource)
        StudentBindingSource.DataSource = _student
        _twoWayBindingSources.Add(StudentBindingSource)
        UespSupplementalAwardBindingSource.DataSource = _student.ScholarshipApplication.UespSupplementalAward
        _twoWayBindingSources.Add(UespSupplementalAwardBindingSource)

        'set data binding for authorized third parties
        AuthorizedThirdParty1.AuthorizedThirdPartyData = _student.AuthorizedThirdParties(0)
        AuthorizedThirdParty1.AuthorizedThirdPartyBindingSource.DataSource = AuthorizedThirdParty1.AuthorizedThirdPartyData
        AuthorizedThirdParty2.AuthorizedThirdPartyData = _student.AuthorizedThirdParties(1)
        AuthorizedThirdParty2.AuthorizedThirdPartyBindingSource.DataSource = AuthorizedThirdParty2.AuthorizedThirdPartyData
        _twoWayBindingSources.Add(AuthorizedThirdParty1.AuthorizedThirdPartyBindingSource)
        _twoWayBindingSources.Add(AuthorizedThirdParty2.AuthorizedThirdPartyBindingSource)

        ScholarshipApplicationBindingSource1.DataSource = _student.ScholarshipApplication

        'Some widgets can't do simple data binding, so load their data separately.
        LoadSpecialWidgetData()
    End Sub

    'Some widgets can't do simple data binding, so their data-loading logic is implemented here.
    Private Sub LoadSpecialWidgetData()
        'Class titles need to be re-loaded for each student because of conditional courses at some high schools.
        'This must happen before the call to LoadCourses(), because that's where the conditional courses are added.
        LoadClassTitles(tabEnglish, Category.ENGLISH)
        LoadClassTitles(tabMathematics, Category.MATHEMATICS)
        LoadClassTitles(tabScienceWithLab, Category.SCIENCE)
        LoadClassTitles(tabSocialScience, Category.SOCIAL_SCIENCE)
        LoadClassTitles(tabForeignLanguage, Category.FOREIGN_LANGUAGE)

        btnDemographicsSaveChanges.Visible = True
        btnDemographicsSaveChanges.Enabled = True
        txtDemographicsAddressLastUpdated.Text = _student.ContactInfo.HomeAddress.LastUpdated.ToString("MM/dd/yyyy")
        btnApplicationSaveChanges.Visible = True
        btnApplicationSaveChanges.Enabled = True
        chkApplicationInitialTranscriptReviewStartStop.Checked = DataAccess.ReviewIsStarted(_student.StateStudentId(), _user.Id, ReviewType.INITIAL_TRANSCRIPT)
        chkApplicationSecondTranscriptReviewStartStop.Checked = DataAccess.ReviewIsStarted(_student.StateStudentId(), _user.Id, ReviewType.SECOND_TRANSCRIPT)
        chkApplicationFinalTranscriptReviewStartStop.Checked = DataAccess.ReviewIsStarted(_student.StateStudentId(), _user.Id, ReviewType.FINAL_TRANSCRIPT)
        btnCommunicationSave.Visible = True
        btnCommunicationSave.Enabled = True
        btnCommunicationClearFields.Visible = True
        btnCommunicationClearFields.Enabled = True
        btnCommunicationLinkDocument.Visible = True
        btnCommunicationLinkDocument.Enabled = True
        btnCommunicationViewDocuments.Visible = True
        btnCommunicationViewDocuments.Enabled = True

        LoadCourses()
        LoadDefermentAndLeave()
        LoadDenials()
        LoadDocumentStatusDates()
        LoadHighSchool()
        LoadPayments()
        LoadReviews()
        SetSpecialText()
    End Sub

#Region "Courses"
    Private Sub LoadCourses()
        LoadEnglishCourses()
        LoadMathCourses()
        LoadScienceCourses()
        LoadSocialScienceCourses()
        LoadForeignLanguageCourses()
    End Sub

    Private Sub LoadEnglishCourses()
        'Set the appropriate Requirement Met radio button, user ID, and date.
        Dim english As CourseCategory = _student.HighSchool.CourseCategories(Category.ENGLISH)
        Select Case english.RequirementIsMet.ToLower()
            Case "yes"
                radEnglishRequirementMetYes.Checked = True
                radEnglishRequirementMetNo.Checked = False
                radEnglishRequirementMetInProgress.Checked = False
            Case "no"
                radEnglishRequirementMetYes.Checked = False
                radEnglishRequirementMetNo.Checked = True
                radEnglishRequirementMetInProgress.Checked = False
            Case "in progress"
                radEnglishRequirementMetYes.Checked = False
                radEnglishRequirementMetNo.Checked = False
                radEnglishRequirementMetInProgress.Checked = True
            Case "undetermined"
                radEnglishRequirementMetYes.Checked = False
                radEnglishRequirementMetNo.Checked = False
                radEnglishRequirementMetInProgress.Checked = False
        End Select
        txtEnglishVerifiedBy.Text = english.Verification.UserId
        txtEnglishVerifiedDate.Text = If(english.Verification.TimeStamp Is Nothing, "", english.Verification.TimeStamp.Value.ToShortDateString())

        'Put the English ClassDataControls in a dictionary and call LoadClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim englishClasses As New Dictionary(Of Integer, ClassDataControl)
        englishClasses.Add(1, englishClass1)
        englishClasses.Add(2, englishClass2)
        englishClasses.Add(3, englishClass3)
        englishClasses.Add(4, englishClass4)
        englishClasses.Add(5, englishClass5)
        englishClasses.Add(6, englishClass6)
        LoadClassData(englishClasses, Category.ENGLISH)
    End Sub

    Private Sub LoadMathCourses()
        'Set the appropriate Requirement Met radio button, user ID, and date.
        Dim math As CourseCategory = _student.HighSchool.CourseCategories(Category.MATHEMATICS)
        Select Case math.RequirementIsMet.ToLower()
            Case "yes"
                radMathematicsRequirementMetYes.Checked = True
                radMathematicsRequirementMetNo.Checked = False
                radMathematicsRequirementMetInProgress.Checked = False
            Case "no"
                radMathematicsRequirementMetYes.Checked = False
                radMathematicsRequirementMetNo.Checked = True
                radMathematicsRequirementMetInProgress.Checked = False
            Case "in progress"
                radMathematicsRequirementMetYes.Checked = False
                radMathematicsRequirementMetNo.Checked = False
                radMathematicsRequirementMetInProgress.Checked = True
            Case "undetermined"
                radMathematicsRequirementMetYes.Checked = False
                radMathematicsRequirementMetNo.Checked = False
                radMathematicsRequirementMetInProgress.Checked = False
        End Select
        txtMathematicsVerifiedBy.Text = math.Verification.UserId
        txtMathematicsVerifiedDate.Text = If(math.Verification.TimeStamp Is Nothing, "", math.Verification.TimeStamp.Value.ToShortDateString())

        'Put the math ClassDataControls in a dictionary and call LoadClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim mathClasses As New Dictionary(Of Integer, ClassDataControl)
        mathClasses.Add(1, mathematicsClass1)
        mathClasses.Add(2, mathematicsClass2)
        mathClasses.Add(3, mathematicsClass3)
        mathClasses.Add(4, mathematicsClass4)
        mathClasses.Add(5, mathematicsClass5)
        mathClasses.Add(6, mathematicsClass6)
        LoadClassData(mathClasses, Category.MATHEMATICS)
    End Sub

    Private Sub LoadScienceCourses()
        'Set the appropriate Requirement Met radio button, user ID, and date.
        Dim science As CourseCategory = _student.HighSchool.CourseCategories(Category.SCIENCE)
        Select Case science.RequirementIsMet.ToLower()
            Case "yes"
                radScienceRequirementMetYes.Checked = True
                radScienceRequirementMetNo.Checked = False
                radScienceRequirementMetInProgress.Checked = False
            Case "no"
                radScienceRequirementMetYes.Checked = False
                radScienceRequirementMetNo.Checked = True
                radScienceRequirementMetInProgress.Checked = False
            Case "in progress"
                radScienceRequirementMetYes.Checked = False
                radScienceRequirementMetNo.Checked = False
                radScienceRequirementMetInProgress.Checked = True
            Case "undetermined"
                radScienceRequirementMetYes.Checked = False
                radScienceRequirementMetNo.Checked = False
                radScienceRequirementMetInProgress.Checked = False
        End Select
        txtScienceVerifiedBy.Text = science.Verification.UserId
        txtScienceVerifiedDate.Text = If(science.Verification.TimeStamp Is Nothing, "", science.Verification.TimeStamp.Value.ToShortDateString())

        'Put the science ClassDataControls in a dictionary and call LoadClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim scienceClasses As New Dictionary(Of Integer, ClassDataControl)
        scienceClasses.Add(1, scienceClass1)
        scienceClasses.Add(2, scienceClass2)
        scienceClasses.Add(3, scienceClass3)
        scienceClasses.Add(4, scienceClass4)
        scienceClasses.Add(5, scienceClass5)
        scienceClasses.Add(6, scienceClass6)
        scienceClasses.Add(7, scienceClass7)
        scienceClasses.Add(8, scienceClass8)
        LoadClassData(scienceClasses, Category.SCIENCE)
    End Sub

    Private Sub LoadSocialScienceCourses()
        'Set the appropriate Requirement Met radio button, user ID, and date.
        Dim socialScience As CourseCategory = _student.HighSchool.CourseCategories(Category.SOCIAL_SCIENCE)
        Select Case socialScience.RequirementIsMet.ToLower()
            Case "yes"
                radSocialScienceRequirementMetYes.Checked = True
                radSocialScienceRequirementMetNo.Checked = False
                radSocialScienceRequirementMetInProgress.Checked = False
            Case "no"
                radSocialScienceRequirementMetYes.Checked = False
                radSocialScienceRequirementMetNo.Checked = True
                radSocialScienceRequirementMetInProgress.Checked = False
            Case "in progress"
                radSocialScienceRequirementMetYes.Checked = False
                radSocialScienceRequirementMetNo.Checked = False
                radSocialScienceRequirementMetInProgress.Checked = True
            Case "undetermined"
                radSocialScienceRequirementMetYes.Checked = False
                radSocialScienceRequirementMetNo.Checked = False
                radSocialScienceRequirementMetInProgress.Checked = False
        End Select
        txtSocialScienceVerifiedBy.Text = socialScience.Verification.UserId
        txtSocialScienceVerifiedDate.Text = If(socialScience.Verification.TimeStamp Is Nothing, "", socialScience.Verification.TimeStamp.Value.ToShortDateString())

        'Put the social science ClassDataControls in a dictionary and call LoadClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim socialScienceClasses As New Dictionary(Of Integer, ClassDataControl)
        socialScienceClasses.Add(1, socialScienceClass1)
        socialScienceClasses.Add(2, socialScienceClass2)
        socialScienceClasses.Add(3, socialScienceClass3)
        socialScienceClasses.Add(4, socialScienceClass4)
        socialScienceClasses.Add(5, socialScienceClass5)
        socialScienceClasses.Add(6, socialScienceClass6)
        socialScienceClasses.Add(7, socialScienceClass7)
        socialScienceClasses.Add(8, socialScienceClass8)
        LoadClassData(socialScienceClasses, Category.SOCIAL_SCIENCE)
    End Sub

    Private Sub LoadForeignLanguageCourses()
        'Set the appropriate Requirement Met radio button, user ID, and date.
        Dim foreignLanguage As CourseCategory = _student.HighSchool.CourseCategories(Category.FOREIGN_LANGUAGE)
        Select Case foreignLanguage.RequirementIsMet.ToLower()
            Case "yes"
                radForeignLanguageRequirementMetYes.Checked = True
                radForeignLanguageRequirementMetNo.Checked = False
                radForeignLanguageRequirementMetInProgress.Checked = False
            Case "no"
                radForeignLanguageRequirementMetYes.Checked = False
                radForeignLanguageRequirementMetNo.Checked = True
                radForeignLanguageRequirementMetInProgress.Checked = False
            Case "in progress"
                radForeignLanguageRequirementMetYes.Checked = False
                radForeignLanguageRequirementMetNo.Checked = False
                radForeignLanguageRequirementMetInProgress.Checked = True
            Case "undetermined"
                radForeignLanguageRequirementMetYes.Checked = False
                radForeignLanguageRequirementMetNo.Checked = False
                radForeignLanguageRequirementMetInProgress.Checked = False
        End Select
        txtForeignLanguageVerifiedBy.Text = foreignLanguage.Verification.UserId
        txtForeignLanguageVerifiedDate.Text = If(foreignLanguage.Verification.TimeStamp Is Nothing, "", foreignLanguage.Verification.TimeStamp.Value.ToShortDateString())

        'Put the foreign language ClassDataControls in a dictionary and call LoadClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim foreignLanguageClasses As New Dictionary(Of Integer, ClassDataControl)
        foreignLanguageClasses.Add(1, foreignLanguageClass1)
        foreignLanguageClasses.Add(2, foreignLanguageClass2)
        foreignLanguageClasses.Add(3, foreignLanguageClass3)
        foreignLanguageClasses.Add(4, foreignLanguageClass4)
        foreignLanguageClasses.Add(5, foreignLanguageClass5)
        LoadClassData(foreignLanguageClasses, Category.FOREIGN_LANGUAGE)
    End Sub

    Private Sub LoadClassData(ByVal classControls As Dictionary(Of Integer, ClassDataControl), ByVal courseCategory As String)
        'See if there are any class titles that are conditionally acceptable
        'for this student's high school.
        Dim courseCategoryString As String = courseCategory
        Dim classTitles As List(Of String) = ( _
            From ctl In Lookups.ClassTitles _
            Where ctl.Type = courseCategoryString _
            Where ctl.IsInApprovedList = True _
            Where ctl.ConditionalSchoolCode = _student.HighSchool.CeebCode _
            Select ctl.Title _
        ).ToList()
        If classTitles.Count > 0 Then
            For Each cdc As ClassDataControl In classControls.Select(Function(p) p.Value)
                cdc.cmbTitle.Items.AddRange(classTitles.ToArray())
            Next
        End If

        'Get a list of the studen't courses in this category.
        Dim courses As List(Of Course) = _student.HighSchool.CourseCategories(courseCategory).Courses
        'Loop through each custom class data control, loading course data where it exists.
        For Each classControlPair As KeyValuePair(Of Integer, ClassDataControl) In classControls
            Dim sequenceNumber As Integer = classControlPair.Key
            Dim classControl As ClassDataControl = classControlPair.Value

            'See if the student has this course sequence number.
            Dim thisCourse As Course = courses.Where(Function(p) p.SequenceNo = sequenceNumber).SingleOrDefault()
            If thisCourse Is Nothing Then
                'Blank out the widgets for courses that don't exist.
                'Class title, weight, grade level, and credits
                classControl.cmbTitle.Text = ""
                classControl.cmbWeightDesignation.Text = ""
                classControl.txtGradeLevel.Text = ""
                classControl.txtCredits.Text = ""
                classControl.txtAcademicYear.Text = ""
                classControl.cmbSchoolAttended.SelectedValue = ""
                'Grades
                classControl.cmbTerm1Grade.Text = ""
                classControl.cmbTerm2Grade.Text = ""
                classControl.cmbTerm3Grade.Text = ""
                classControl.cmbTerm4Grade.Text = ""
                classControl.cmbTerm5Grade.Text = ""
                classControl.cmbTerm6Grade.Text = ""
                'Weighted average grade
                classControl.txtWeightedAverageGrade.Text = ""
                classControl.txtWeightedAverageGradeEquivalent.Text = ""
                'Verification
                classControl.radClassAcceptableYes.Checked = False
                classControl.radClassAcceptableNo.Checked = False
                classControl.radClassAcceptableInProgress.Checked = False
                classControl.txtClassVerifiedBy.Text = ""
                classControl.txtClassVerifiedDate.Text = ""
                Continue For
            Else
                'Load widget text for courses that do exist.
                'Class title, weight, grade level, and credits
                classControl.cmbTitle.Text = thisCourse.Title
                classControl.cmbWeightDesignation.Text = thisCourse.Weight
                If (thisCourse.Weight = Constants.ClassWeight.CE) Then
                    classControl.cmbConcurrentCollege.Enabled = True
                    classControl.cmbConcurrentCollege.Text = thisCourse.ConcurrentCollege
                Else
                    classControl.cmbConcurrentCollege.Enabled = False
                    classControl.cmbConcurrentCollege.Text = ""
                End If
                classControl.txtGradeLevel.Text = thisCourse.GradeLevel
                classControl.txtCredits.Text = thisCourse.Credits.ToString()
                classControl.txtAcademicYear.Text = thisCourse.AcademicYearTaken
                If thisCourse.SchoolAttended = Nothing OrElse thisCourse.SchoolAttended = "" Then
                    classControl.cmbSchoolAttended.SelectedValue = ""
                Else
                    classControl.cmbSchoolAttended.SelectedValue = thisCourse.SchoolAttended
                End If

                'Grades
                If thisCourse.Grades.ContainsKey(1) Then
                    classControl.cmbTerm1Grade.Text = thisCourse.Grades(1).Letter
                Else
                    classControl.cmbTerm1Grade.Text = ""
                End If
                If thisCourse.Grades.ContainsKey(2) Then
                    classControl.cmbTerm2Grade.Text = thisCourse.Grades(2).Letter
                Else
                    classControl.cmbTerm2Grade.Text = ""
                End If
                If thisCourse.Grades.ContainsKey(3) Then
                    classControl.cmbTerm3Grade.Text = thisCourse.Grades(3).Letter
                Else
                    classControl.cmbTerm3Grade.Text = ""
                End If
                If thisCourse.Grades.ContainsKey(4) Then
                    classControl.cmbTerm4Grade.Text = thisCourse.Grades(4).Letter
                Else
                    classControl.cmbTerm4Grade.Text = ""
                End If
                If thisCourse.Grades.ContainsKey(5) Then
                    classControl.cmbTerm5Grade.Text = thisCourse.Grades(5).Letter
                Else
                    classControl.cmbTerm5Grade.Text = ""
                End If
                If thisCourse.Grades.ContainsKey(6) Then
                    classControl.cmbTerm6Grade.Text = thisCourse.Grades(6).Letter
                Else
                    classControl.cmbTerm6Grade.Text = ""
                End If
                'Weighted average grade (only show if it's actually weighted)
                If thisCourse.Weight <> "" AndAlso thisCourse.WeightedAverageGrade.Letter <> "" Then
                    classControl.txtWeightedAverageGrade.Text = thisCourse.WeightedAverageGrade.Letter
                    classControl.txtWeightedAverageGradeEquivalent.Text = thisCourse.WeightedAverageGrade.GpaValue.ToString("0.00")
                Else
                    classControl.txtWeightedAverageGrade.Text = ""
                    classControl.txtWeightedAverageGradeEquivalent.Text = ""
                End If
                'Verification
                Select Case thisCourse.IsAcceptable.ToLower()
                    Case "yes"
                        classControl.radClassAcceptableYes.Checked = True
                        classControl.radClassAcceptableNo.Checked = False
                        classControl.radClassAcceptableInProgress.Checked = False
                    Case "no"
                        classControl.radClassAcceptableYes.Checked = False
                        classControl.radClassAcceptableNo.Checked = True
                        classControl.radClassAcceptableInProgress.Checked = False
                    Case "in progress"
                        classControl.radClassAcceptableYes.Checked = False
                        classControl.radClassAcceptableNo.Checked = False
                        classControl.radClassAcceptableInProgress.Checked = True
                    Case "undetermined"
                        classControl.radClassAcceptableYes.Checked = False
                        classControl.radClassAcceptableNo.Checked = False
                        classControl.radClassAcceptableInProgress.Checked = False
                End Select
                classControl.txtClassVerifiedBy.Text = thisCourse.Verification.UserId
                classControl.txtClassVerifiedDate.Text = If(thisCourse.Verification.TimeStamp Is Nothing, "", thisCourse.Verification.TimeStamp.Value.ToShortDateString())
            End If
        Next classControlPair
    End Sub
#End Region 'Courses

    Private Sub LoadDefermentAndLeave()
        'Set the deferment widgets according to whether the student has a deferment.
        If _student.ScholarshipApplication.Deferment Is Nothing Then
            dtpApplicationDefermentBeginDate.Value = Nothing
            dtpApplicationDefermentEndDate.Value = Nothing
            cmbApplicationDefermentReason.Text = ""
        Else
            dtpApplicationDefermentBeginDate.Value = _student.ScholarshipApplication.Deferment.BeginDate
            dtpApplicationDefermentEndDate.Value = _student.ScholarshipApplication.Deferment.EndDate
            cmbApplicationDefermentReason.Text = _student.ScholarshipApplication.Deferment.Reason
        End If

        'Set the leave of absence widgets according to whether the student has a leave of absence.
        If _student.ScholarshipApplication.LeaveOfAbsence Is Nothing Then
            dtpApplicationLeaveOfAbsenceBeginDate.Value = Nothing
            dtpApplicationLeaveOfAbsenceEndDate.Value = Nothing
            cmbApplicationLeaveOfAbsenceReason.Text = ""
        Else
            dtpApplicationLeaveOfAbsenceBeginDate.Value = _student.ScholarshipApplication.LeaveOfAbsence.BeginDate
            dtpApplicationLeaveOfAbsenceEndDate.Value = _student.ScholarshipApplication.LeaveOfAbsence.EndDate
            cmbApplicationLeaveOfAbsenceReason.Text = _student.ScholarshipApplication.LeaveOfAbsence.Reason
        End If
    End Sub

    Private Sub LoadDenials()
        'Set the denial reason widgets.
        If (_student.ScholarshipApplication.DenialReasons.Count > 0) Then
            cmbApplicationDenialReason1.Text = _student.ScholarshipApplication.DenialReasons(0)
        Else
            cmbApplicationDenialReason1.Text = ""
        End If
        If (_student.ScholarshipApplication.DenialReasons.Count > 1) Then
            cmbApplicationDenialReason2.Text = _student.ScholarshipApplication.DenialReasons(1)
        Else
            cmbApplicationDenialReason2.Text = ""
        End If
        If (_student.ScholarshipApplication.DenialReasons.Count > 2) Then
            cmbApplicationDenialReason3.Text = _student.ScholarshipApplication.DenialReasons(2)
        Else
            cmbApplicationDenialReason3.Text = ""
        End If
        If (_student.ScholarshipApplication.DenialReasons.Count > 3) Then
            cmbApplicationDenialReason4.Text = _student.ScholarshipApplication.DenialReasons(3)
        Else
            cmbApplicationDenialReason4.Text = ""
        End If
        If (_student.ScholarshipApplication.DenialReasons.Count > 4) Then
            cmbApplicationDenialReason5.Text = _student.ScholarshipApplication.DenialReasons(4)
        Else
            cmbApplicationDenialReason5.Text = ""
        End If
        If (_student.ScholarshipApplication.DenialReasons.Count > 5) Then
            cmbApplicationDenialReason6.Text = _student.ScholarshipApplication.DenialReasons(5)
        Else
            cmbApplicationDenialReason6.Text = ""
        End If
    End Sub

    Private Sub LoadDocumentStatusDates()
        'Application received
        Dim statusDate As Nullable(Of Date) = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.APPLICATION).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Initial high school transcript received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.INITIAL_HIGH_SCHOOL_TRANSCRIPT).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationInitialHighSchoolTranscriptReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Final high school transcript received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.FINAL_HIGH_SCHOOL_TRANSCRIPT).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationFinalHighSchoolTranscriptReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'High school schedule received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.HIGH_SCHOOL_SCHEDULE).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpHighSchoolScheduleReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Initial college transcript received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.INITIAL_COLLEGE_TRANSCRIPT).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationInitialCollegeTranscriptReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Final college transcript received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.FINAL_COLLEGE_TRANSCRIPT).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationFinalCollegeTranscriptReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'decide if Signature page controls should be enabled or not
        If _student.ScholarshipApplication.ApplicationYear = "2009" Then
            'Signature page received
            dtpApplicationSignaturePageReceivedDate.Enabled = True
            lblApplicationSignaturePageReceivedDate.Enabled = True
            statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.SIGNATURE_PAGE).Select(Function(p) p.Value.Value).SingleOrDefault()
            dtpApplicationSignaturePageReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        Else
            dtpApplicationSignaturePageReceivedDate.Value = Nothing
            dtpApplicationSignaturePageReceivedDate.Enabled = False
            lblApplicationSignaturePageReceivedDate.Enabled = False
        End If
        'Conditional acceptance form received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.CONDITIONAL_ACCEPTANCE_FORM).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationConditionalAcceptanceFormReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Proof of enrollment received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.PROOF_OF_ENROLLMENT).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationProofOfEnrollmentReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Proof of citizenship received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.PROOF_OF_CITIZENSHIP).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationProofOfCitizenshipReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Appeal received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.APPEAL).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationAppealReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Appeal decision
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.APPEAL_DECISION).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationAppealDecisionDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'LOA request received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.LEAVE_OF_ABSENCE_REQUEST).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationLoaRequestReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'LOA decision
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.LEAVE_OF_ABSENCE_DECISION).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationLoaDecisionDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Deferment request received
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.DEFERMENT_REQUEST).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationDefermentRequestReceivedDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
        'Deferment decision
        statusDate = _student.ScholarshipApplication.DocumentStatusDates.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.DocumentType.DEFERMENT_DECISION).Select(Function(p) p.Value.Value).SingleOrDefault()
        dtpApplicationDefermentDecisionDate.Value = If(statusDate = Date.MinValue, Nothing, statusDate)
    End Sub

    Private Sub LoadHighSchool()
        'decide whether IB Diploma check box should be enabled
        If _student.ScholarshipApplication.ApplicationYear = "2009" Then
            chkApplicationIbDiploma.Enabled = True
            radApplicationUbsctFail.Enabled = True
            radApplicationUbsctPass.Enabled = True
            lblApplicationUbsct.Enabled = True
            'Handle the USBCT status and ACT scores here.
            Select Case _student.HighSchool.UsbctStatus
                Case RegentsScholarshipBackEnd.Constants.UsbctStatus.FAIL
                    radApplicationUbsctFail.Checked = True
                Case RegentsScholarshipBackEnd.Constants.UsbctStatus.PASS
                    radApplicationUbsctPass.Checked = True
            End Select
        Else
            chkApplicationIbDiploma.Enabled = False
            radApplicationUbsctFail.Enabled = False
            radApplicationUbsctPass.Enabled = False
            lblApplicationUbsct.Enabled = False
        End If


        'Compoiste
        Dim actScore As Double = _student.HighSchool.ActScores.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.ActCategory.COMPOSITE).Select(Function(p) p.Value).SingleOrDefault()
        txtApplicationActCompositeScore.Text = If(actScore = 0, "", actScore.ToString())
        'English
        actScore = _student.HighSchool.ActScores.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.ActCategory.ENGLISH).Select(Function(p) p.Value).SingleOrDefault()
        txtApplicationActEnglishScore.Text = If(actScore = 0, "", actScore.ToString())
        'Math
        actScore = _student.HighSchool.ActScores.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.ActCategory.MATH).Select(Function(p) p.Value).SingleOrDefault()
        txtApplicationActMathScore.Text = If(actScore = 0, "", actScore.ToString())
        'Reading
        actScore = _student.HighSchool.ActScores.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.ActCategory.READING).Select(Function(p) p.Value).SingleOrDefault()
        txtApplicationActReadingScore.Text = If(actScore = 0, "", actScore.ToString())
        'Science
        actScore = _student.HighSchool.ActScores.Where(Function(p) p.Key = RegentsScholarshipBackEnd.Constants.ActCategory.SCIENCE).Select(Function(p) p.Value).SingleOrDefault()
        txtApplicationActScienceScore.Text = If(actScore = 0, "", actScore.ToString())
    End Sub

    Private Sub LoadPayments()
        btnPaymentsSaveChanges.Visible = True
        btnPaymentsSaveChanges.Enabled = True
        pnlPayments.Controls.Clear()
        'Add all of the student's payments to the panel.
        For Each pt As Payment In _student.Payments
            pnlPayments.Controls.Add(New PaymentControl(pt, _user.AccessLevel))
        Next pt
        'Re-add the "Add Payment" button if applicable, and scroll to the bottom.
        If (_student.Payments.Count < 15) Then
            btnPaymentsNewPayment.Visible = True
            btnPaymentsNewPayment.Enabled = True
            pnlPayments.Controls.Add(btnPaymentsNewPayment)
            pnlPayments.ScrollControlIntoView(btnPaymentsNewPayment)
        Else
            pnlPayments.ScrollControlIntoView(pnlPayments.Controls(14))
        End If
    End Sub

    Private Sub LoadReviews()
        'Initial transcript review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.INITIAL_TRANSCRIPT) Then
            Dim initialTranscriptReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.INITIAL_TRANSCRIPT)
            chkApplicationInitialTranscriptReview.Checked = True
            txtApplicationInitialTranscriptReviewDate.Text = initialTranscriptReview.CompletionDate.Value.ToShortDateString()
            txtApplicationInitialTranscriptReviewUserId.Text = initialTranscriptReview.UserId
        Else
            chkApplicationInitialTranscriptReview.Checked = False
            txtApplicationInitialTranscriptReviewDate.Visible = False
            txtApplicationInitialTranscriptReviewUserId.Visible = False
        End If

        'Second transcript review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.SECOND_TRANSCRIPT) Then
            Dim secondTranscriptReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.SECOND_TRANSCRIPT)
            chkApplicationSecondTranscriptReview.Checked = True
            txtApplicationSecondTranscriptReviewDate.Text = secondTranscriptReview.CompletionDate.Value.ToShortDateString()
            txtApplicationSecondTranscriptReviewUserId.Text = secondTranscriptReview.UserId
        Else
            chkApplicationSecondTranscriptReview.Checked = False
            txtApplicationSecondTranscriptReviewDate.Visible = False
            txtApplicationSecondTranscriptReviewUserId.Visible = False
        End If

        'Final transcript review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.FINAL_TRANSCRIPT) Then
            Dim finalTranscriptReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.FINAL_TRANSCRIPT)
            chkApplicationFinalTranscriptReview.Checked = True
            txtApplicationFinalTranscriptReviewDate.Text = finalTranscriptReview.CompletionDate.Value.ToShortDateString()
            txtApplicationFinalTranscriptReviewUserId.Text = finalTranscriptReview.UserId
        Else
            chkApplicationFinalTranscriptReview.Checked = False
            txtApplicationFinalTranscriptReviewDate.Visible = False
            txtApplicationFinalTranscriptReviewUserId.Visible = False
        End If

        'Class review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.CLASS_) Then
            Dim classReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.CLASS_)
            chkApplicationClassReview.Checked = True
            txtApplicationClassReviewDate.Text = classReview.CompletionDate.Value.ToShortDateString()
            txtApplicationClassReviewUserId.Text = classReview.UserId
        Else
            chkApplicationClassReview.Checked = False
            txtApplicationClassReviewDate.Visible = False
            txtApplicationClassReviewUserId.Visible = False
        End If

        'Category review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.CATEGORY) Then
            Dim categoryReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.CATEGORY)
            chkApplicationCategoryReview.Checked = True
            txtApplicationCategoryReviewDate.Text = categoryReview.CompletionDate.Value.ToShortDateString()
            txtApplicationCategoryReviewUserId.Text = categoryReview.UserId
        Else
            chkApplicationCategoryReview.Checked = False
            txtApplicationCategoryReviewDate.Visible = False
            txtApplicationCategoryReviewUserId.Visible = False
        End If

        'Initial award review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.INITIAL_AWARD) Then
            Dim initialAwardReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.INITIAL_AWARD)
            chkApplicationInitialAwardReview.Checked = True
            txtApplicationInitialAwardReviewDate.Text = initialAwardReview.CompletionDate.Value.ToShortDateString()
            txtApplicationInitialAwardReviewUserId.Text = initialAwardReview.UserId
        Else
            chkApplicationInitialAwardReview.Checked = False
            txtApplicationInitialAwardReviewDate.Visible = False
            txtApplicationInitialAwardReviewUserId.Visible = False
        End If

        'First quickreview
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.FIRST_QUICK) Then
            Dim firstQuickReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.FIRST_QUICK)
            'check if the review is int progress or complete
            If firstQuickReview.CompletionDate Is Nothing Then
                lblApplicationFirstQuickReviewInProgress.Visible = True
                chkApplicationFirstQuickReview.Checked = False
                txtApplicationFirstQuickReviewDate.Visible = False
                txtApplicationFirstQuickReviewUserId.Visible = False
            Else
                lblApplicationFirstQuickReviewInProgress.Visible = False
                chkApplicationFirstQuickReview.Checked = True
                txtApplicationFirstQuickReviewDate.Text = firstQuickReview.CompletionDate.Value.ToShortDateString()
                txtApplicationFirstQuickReviewUserId.Text = firstQuickReview.UserId
            End If
        Else
            chkApplicationFirstQuickReview.Checked = False
            txtApplicationFirstQuickReviewDate.Visible = False
            txtApplicationFirstQuickReviewUserId.Visible = False
            lblApplicationFirstQuickReviewInProgress.Visible = False
        End If

        'Base award review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.BASE_AWARD) Then
            Dim baseAwardReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.BASE_AWARD)
            chkApplicationBaseAwardReview.Checked = True
            txtApplicationBaseAwardReviewDate.Text = baseAwardReview.CompletionDate.Value.ToShortDateString()
            txtApplicationBaseAwardReviewUserId.Text = baseAwardReview.UserId
        Else
            chkApplicationBaseAwardReview.Checked = False
            txtApplicationBaseAwardReviewDate.Visible = False
            txtApplicationBaseAwardReviewUserId.Visible = False
        End If

        'Exemplary award review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.EXEMPLARY_AWARD) Then
            Dim exemplaryAwardReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.EXEMPLARY_AWARD)
            chkApplicationExemplaryAwardReview.Checked = True
            txtApplicationExemplaryAwardReviewDate.Text = exemplaryAwardReview.CompletionDate.Value.ToShortDateString()
            txtApplicationExemplaryAwardReviewUserId.Text = exemplaryAwardReview.UserId
        Else
            chkApplicationExemplaryAwardReview.Checked = False
            txtApplicationExemplaryAwardReviewDate.Visible = False
            txtApplicationExemplaryAwardReviewUserId.Visible = False
        End If

        'UESP award review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.UESP_AWARD) Then
            Dim uespAwardReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.UESP_AWARD)
            'check if the review is int progress or complete
            If uespAwardReview.CompletionDate Is Nothing Then
                chkApplicationUespAwardReview.Checked = False
                txtApplicationUespAwardReviewDate.Visible = False
                txtApplicationUespAwardReviewUserId.Visible = False
                lblApplicationUespAwardReviewInProgress.Visible = True
            Else
                lblApplicationUespAwardReviewInProgress.Visible = False
                chkApplicationUespAwardReview.Checked = True
                txtApplicationUespAwardReviewDate.Text = uespAwardReview.CompletionDate.Value.ToShortDateString()
                txtApplicationUespAwardReviewUserId.Text = uespAwardReview.UserId
            End If
        Else
            chkApplicationUespAwardReview.Checked = False
            txtApplicationUespAwardReviewDate.Visible = False
            txtApplicationUespAwardReviewUserId.Visible = False
            lblApplicationUespAwardReviewInProgress.Visible = False
        End If

        'Second quick review
        If _student.ScholarshipApplication.Reviews.ContainsKey(ReviewType.SECOND_QUICK) Then
            Dim secondQuickReview As Review = _student.ScholarshipApplication.Reviews(ReviewType.SECOND_QUICK)
            'check if review is in progress or is complete
            If secondQuickReview.CompletionDate Is Nothing Then
                lblApplicationSecondQuickReviewInProgress.Visible = True
                chkApplicationSecondQuickReview.Checked = False
                txtApplicationSecondQuickReviewDate.Visible = False
                txtApplicationSecondQuickReviewUserId.Visible = False
            Else
                lblApplicationSecondQuickReviewInProgress.Visible = False
                chkApplicationSecondQuickReview.Checked = True
                txtApplicationSecondQuickReviewDate.Text = secondQuickReview.CompletionDate.Value.ToShortDateString()
                txtApplicationSecondQuickReviewUserId.Text = secondQuickReview.UserId
            End If
        Else
            chkApplicationSecondQuickReview.Checked = False
            txtApplicationSecondQuickReviewDate.Visible = False
            txtApplicationSecondQuickReviewUserId.Visible = False
            lblApplicationSecondQuickReviewInProgress.Visible = False
        End If
    End Sub

    Private Sub SetSpecialText()
        'Set the header text boxes. These aren't data bound to avoid any problems with other widgets that show the same data.
        txtHeaderStudentId.Text = _student.StateStudentId
        txtHeaderStudentName.Text = String.Format("{0} {1}", _student.FirstName, _student.LastName)
        txtHeaderAwardStatus.Text = _student.ScholarshipApplication.BaseAward.Status

        'Set items that can't be data-bound because the business objects are using functions rather than read-only properties (there's a reason for that).
        txtDemographicsStateStudentId.Text = _student.StateStudentId()
        If _student.ScholarshipApplication.BaseAward.Status = RegentsScholarshipBackEnd.Constants.AwardStatus.APPROVED _
        OrElse _student.ScholarshipApplication.BaseAward.Status = RegentsScholarshipBackEnd.Constants.AwardStatus.DEFERRED _
        OrElse _student.ScholarshipApplication.BaseAward.Status = RegentsScholarshipBackEnd.Constants.AwardStatus.LEAVE_OF_ABSENCE _
        Then
            txtApplicationBaseAwardAmount.Text = _student.ScholarshipApplication.BaseAward.Amount()
        Else
            txtApplicationBaseAwardAmount.Text = ""
        End If
        txtPaymentsCumulativeCreditHoursPaid.Text = _student.CumulativeCreditHoursPaid()

        'The award amount text boxes are data-bound, so they show a value regardless of the award status.
        'It's preferable not to show an amount if a given award isn't approved.
        'We can't set the amount text to an empty string, because that would mess up the database.
        'So the current work-around is to make the text invisible.
        If _student.ScholarshipApplication.ExemplaryAward.IsApproved.HasValue _
        AndAlso _student.ScholarshipApplication.ExemplaryAward.IsApproved = True Then
            txtApplicationExemplaryAwardAmount.ForeColor = Color.Black
        Else
            txtApplicationExemplaryAwardAmount.ForeColor = Color.Transparent
        End If
        If _student.ScholarshipApplication.UespSupplementalAward.IsApproved.HasValue _
        AndAlso _student.ScholarshipApplication.UespSupplementalAward.IsApproved = True Then
            txtApplicationSupplementalAwardAmount.ForeColor = Color.Black
        Else
            txtApplicationSupplementalAwardAmount.ForeColor = Color.Transparent
        End If
    End Sub
#End Region 'Loading

#Region "Saving"
    'Some widgets can't do simple data binding, so their data-saving logic is implemented here.
    Private Sub SaveSpecialWidgetData()
        DeletePayments()
        SaveCourses()
        SaveDefermentAndLeave()
        SaveDenials()
        SaveDocumentStatusDates()
        SaveHighSchool()
        SaveReviews()
    End Sub

    Private Sub DeletePayments()
        For Each ctrl As Control In pnlPayments.Controls
            Dim pmtCtrl As PaymentControl = TryCast(ctrl, PaymentControl)
            If pmtCtrl Is Nothing Then Continue For
            If (pmtCtrl.chkDelete.Checked) Then
                Dim thisPayment As Payment = _student.Payments.Where( _
                    Function(p) p.SequenceNo = Double.Parse(pmtCtrl.txtSequenceNo.Text) _
                ).Single()
                _student.Payments.Remove(thisPayment)
            End If
        Next ctrl
    End Sub

#Region "Courses"
    Private Sub SaveCourses()
        SaveEnglishCourses()
        SaveMathCourses()
        SaveScienceCourses()
        SaveSocialScienceCourses()
        SaveForeignLanguageCourses()
    End Sub

    Private Sub SaveEnglishCourses()
        'See if the RequirementIsMet value changed.
        Dim english As CourseCategory = _student.HighSchool.CourseCategories(Category.ENGLISH)
        If radEnglishRequirementMetYes.Checked Then
            'Is this a change from the previous status?
            If english.RequirementIsMet.ToLower() <> "yes" Then
                'Set the new status, user ID, and date.
                english.RequirementIsMet = "Yes"
                english.Verification.UserId = _user.Id
                english.Verification.TimeStamp = Date.Now
            End If
        ElseIf radEnglishRequirementMetNo.Checked Then
            'Is this a change from the previous status?
            If english.RequirementIsMet.ToLower() <> "no" Then
                'Set the new status, user ID, and date.
                english.RequirementIsMet = "No"
                english.Verification.UserId = _user.Id
                english.Verification.TimeStamp = Date.Now
            End If
        ElseIf radEnglishRequirementMetInProgress.Checked Then
            'Is this a change from the previous status?
            If english.RequirementIsMet.ToLower() <> "in progress" Then
                'Set the new status, user ID, and date.
                english.RequirementIsMet = "In Progress"
                english.Verification.UserId = _user.Id
                english.Verification.TimeStamp = Date.Now
            End If
        End If

        'Put the English ClassDataControls in a dictionary and call SaveClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim englishClasses As New Dictionary(Of Integer, ClassDataControl)
        englishClasses.Add(1, englishClass1)
        englishClasses.Add(2, englishClass2)
        englishClasses.Add(3, englishClass3)
        englishClasses.Add(4, englishClass4)
        englishClasses.Add(5, englishClass5)
        englishClasses.Add(6, englishClass6)
        SaveClassData(englishClasses, english)
    End Sub

    Private Sub SaveMathCourses()
        'See if the RequirementIsMet value changed.
        Dim math As CourseCategory = _student.HighSchool.CourseCategories(Category.MATHEMATICS)
        If radMathematicsRequirementMetYes.Checked Then
            'Is this a change from the previous status?
            If math.RequirementIsMet.ToLower() <> "yes" Then
                'Set the new status, user ID, and date.
                math.RequirementIsMet = "Yes"
                math.Verification.UserId = _user.Id
                math.Verification.TimeStamp = Date.Now
            End If
        ElseIf radMathematicsRequirementMetNo.Checked Then
            'Is this a change from the previous status?
            If math.RequirementIsMet.ToLower() <> "no" Then
                'Set the new status, user ID, and date.
                math.RequirementIsMet = "No"
                math.Verification.UserId = _user.Id
                math.Verification.TimeStamp = Date.Now
            End If
        ElseIf radMathematicsRequirementMetInProgress.Checked Then
            'Is this a change from the previous status?
            If math.RequirementIsMet.ToLower() <> "in progress" Then
                'Set the new status, user ID, and date.
                math.RequirementIsMet = "In Progress"
                math.Verification.UserId = _user.Id
                math.Verification.TimeStamp = Date.Now
            End If
        End If

        'Put the English ClassDataControls in a dictionary and call SaveClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim mathClasses As New Dictionary(Of Integer, ClassDataControl)
        mathClasses.Add(1, mathematicsClass1)
        mathClasses.Add(2, mathematicsClass2)
        mathClasses.Add(3, mathematicsClass3)
        mathClasses.Add(4, mathematicsClass4)
        mathClasses.Add(5, mathematicsClass5)
        mathClasses.Add(6, mathematicsClass6)
        SaveClassData(mathClasses, math)
    End Sub

    Private Sub SaveScienceCourses()
        'See if the RequirementIsMet value changed.
        Dim science As CourseCategory = _student.HighSchool.CourseCategories(Category.SCIENCE)
        If radScienceRequirementMetYes.Checked Then
            'Is this a change from the previous status?
            If science.RequirementIsMet.ToLower() <> "yes" Then
                'Set the new status, user ID, and date.
                science.RequirementIsMet = "Yes"
                science.Verification.UserId = _user.Id
                science.Verification.TimeStamp = Date.Now
            End If
        ElseIf radScienceRequirementMetNo.Checked Then
            'Is this a change from the previous status?
            If science.RequirementIsMet.ToLower() <> "no" Then
                'Set the new status, user ID, and date.
                science.RequirementIsMet = "No"
                science.Verification.UserId = _user.Id
                science.Verification.TimeStamp = Date.Now
            End If
        ElseIf radScienceRequirementMetInProgress.Checked Then
            'Is this a change from the previous status?
            If science.RequirementIsMet.ToLower() <> "in progress" Then
                'Set the new status, user ID, and date.
                science.RequirementIsMet = "In Progress"
                science.Verification.UserId = _user.Id
                science.Verification.TimeStamp = Date.Now
            End If
        End If

        'Put the English ClassDataControls in a dictionary and call SaveClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim scienceClasses As New Dictionary(Of Integer, ClassDataControl)
        scienceClasses.Add(1, scienceClass1)
        scienceClasses.Add(2, scienceClass2)
        scienceClasses.Add(3, scienceClass3)
        scienceClasses.Add(4, scienceClass4)
        scienceClasses.Add(5, scienceClass5)
        scienceClasses.Add(6, scienceClass6)
        scienceClasses.Add(7, scienceClass7)
        scienceClasses.Add(8, scienceClass8)
        SaveClassData(scienceClasses, science)
    End Sub

    Private Sub SaveSocialScienceCourses()
        'See if the RequirementIsMet value changed.
        Dim socialScience As CourseCategory = _student.HighSchool.CourseCategories(Category.SOCIAL_SCIENCE)
        If radSocialScienceRequirementMetYes.Checked Then
            'Is this a change from the previous status?
            If socialScience.RequirementIsMet.ToLower() <> "yes" Then
                'Set the new status, user ID, and date.
                socialScience.RequirementIsMet = "Yes"
                socialScience.Verification.UserId = _user.Id
                socialScience.Verification.TimeStamp = Date.Now
            End If
        ElseIf radSocialScienceRequirementMetNo.Checked Then
            'Is this a change from the previous status?
            If socialScience.RequirementIsMet.ToLower() <> "no" Then
                'Set the new status, user ID, and date.
                socialScience.RequirementIsMet = "No"
                socialScience.Verification.UserId = _user.Id
                socialScience.Verification.TimeStamp = Date.Now
            End If
        ElseIf radSocialScienceRequirementMetInProgress.Checked Then
            'Is this a change from the previous status?
            If socialScience.RequirementIsMet.ToLower() <> "in progress" Then
                'Set the new status, user ID, and date.
                socialScience.RequirementIsMet = "In Progress"
                socialScience.Verification.UserId = _user.Id
                socialScience.Verification.TimeStamp = Date.Now
            End If
        End If

        'Put the social science ClassDataControls in a dictionary and call SaveClassData() to load their data.
        'Note: The dictionary key is the course sequence number.
        Dim socialScienceClasses As New Dictionary(Of Integer, ClassDataControl)
        socialScienceClasses.Add(1, socialScienceClass1)
        socialScienceClasses.Add(2, socialScienceClass2)
        socialScienceClasses.Add(3, socialScienceClass3)
        socialScienceClasses.Add(4, socialScienceClass4)
        socialScienceClasses.Add(5, socialScienceClass5)
        socialScienceClasses.Add(6, socialScienceClass6)
        socialScienceClasses.Add(7, socialScienceClass7)
        socialScienceClasses.Add(8, socialScienceClass8)
        SaveClassData(socialScienceClasses, socialScience)
    End Sub

    Private Sub SaveForeignLanguageCourses()
        'See if the RequirementIsMet value changed.
        Dim foreignLanguage As CourseCategory = _student.HighSchool.CourseCategories(Category.FOREIGN_LANGUAGE)
        If radForeignLanguageRequirementMetYes.Checked Then
            'Is this a change from the previous status?
            If foreignLanguage.RequirementIsMet.ToLower() <> "yes" Then
                'Set the new status, user ID, and date.
                foreignLanguage.RequirementIsMet = "Yes"
                foreignLanguage.Verification.UserId = _user.Id
                foreignLanguage.Verification.TimeStamp = Date.Now
            End If
        ElseIf radForeignLanguageRequirementMetNo.Checked Then
            'Is this a change from the previous status?
            If foreignLanguage.RequirementIsMet.ToLower() <> "no" Then
                'Set the new status, user ID, and date.
                foreignLanguage.RequirementIsMet = "No"
                foreignLanguage.Verification.UserId = _user.Id
                foreignLanguage.Verification.TimeStamp = Date.Now
            End If
        ElseIf radForeignLanguageRequirementMetInProgress.Checked Then
            'Is this a change from the previous status?
            If foreignLanguage.RequirementIsMet.ToLower() <> "in progress" Then
                'Set the new status, user ID, and date.
                foreignLanguage.RequirementIsMet = "In Progress"
                foreignLanguage.Verification.UserId = _user.Id
                foreignLanguage.Verification.TimeStamp = Date.Now
            End If
        End If

        'Put the foreign language ClassDataControls in a dictionary and call SaveClassData() to load their data.
        Dim foreignLanguageClasses As New Dictionary(Of Integer, ClassDataControl)
        foreignLanguageClasses.Add(1, foreignLanguageClass1)
        foreignLanguageClasses.Add(2, foreignLanguageClass2)
        foreignLanguageClasses.Add(3, foreignLanguageClass3)
        foreignLanguageClasses.Add(4, foreignLanguageClass4)
        foreignLanguageClasses.Add(5, foreignLanguageClass5)
        SaveClassData(foreignLanguageClasses, foreignLanguage)
    End Sub

    Private Sub SaveClassData(ByVal classControls As Dictionary(Of Integer, ClassDataControl), ByVal courseCategory As CourseCategory)
        Dim courses As List(Of Course) = _student.HighSchool.CourseCategories(courseCategory.Category).Courses
        'When saving courses, all class controls must be considered.
        For Each classControlPair As KeyValuePair(Of Integer, ClassDataControl) In classControls
            Dim sequenceNumber As Integer = classControlPair.Key
            Dim classControl As ClassDataControl = classControlPair.Value
            'We need to see whether we adding, deleting, or updating a course.

            'If this class control has a class title set and the key (course sequence number)
            'does not exist in the student's course sequence numbers, then we're adding a course.
            If Not String.IsNullOrEmpty(classControl.cmbTitle.Text) _
            AndAlso Not courses.Select(Function(p) p.SequenceNo).Contains(sequenceNumber) Then
                'Create a new Course object and set its members.
                Dim newCourse As New Course(sequenceNumber, courseCategory)
                newCourse.Title = classControl.cmbTitle.Text
                newCourse.Weight = classControl.cmbWeightDesignation.Text
                newCourse.ConcurrentCollege = classControl.cmbConcurrentCollege.Text
                newCourse.GradeLevel = classControl.txtGradeLevel.Text
                newCourse.AcademicYearTaken = classControl.txtAcademicYear.Text
                newCourse.SchoolAttended = classControl.cmbSchoolAttended.SelectedValue
                If classControl.txtCredits.Text = "" Then
                    newCourse.Credits = 0
                Else
                    If Not Double.TryParse(classControl.txtCredits.Text, newCourse.Credits) Then
                        Dim message As String = String.Format("Class credits for {0} must be numeric.", newCourse.Title)
                        Throw New RegentsInvalidDataException(message)
                    End If
                End If
                'Grades
                If Not String.IsNullOrEmpty(classControl.cmbTerm1Grade.Text) Then
                    Dim newGrade As New Grade(1, newCourse)
                    newGrade.Letter = classControl.cmbTerm1Grade.Text
                    newCourse.Grades.Add(newGrade)
                End If
                If Not String.IsNullOrEmpty(classControl.cmbTerm2Grade.Text) Then
                    Dim newGrade As New Grade(2, newCourse)
                    newGrade.Letter = classControl.cmbTerm2Grade.Text
                    newCourse.Grades.Add(newGrade)
                End If
                If Not String.IsNullOrEmpty(classControl.cmbTerm3Grade.Text) Then
                    Dim newGrade As New Grade(3, newCourse)
                    newGrade.Letter = classControl.cmbTerm3Grade.Text
                    newCourse.Grades.Add(newGrade)
                End If
                If Not String.IsNullOrEmpty(classControl.cmbTerm4Grade.Text) Then
                    Dim newGrade As New Grade(4, newCourse)
                    newGrade.Letter = classControl.cmbTerm4Grade.Text
                    newCourse.Grades.Add(newGrade)
                End If
                If Not String.IsNullOrEmpty(classControl.cmbTerm5Grade.Text) Then
                    Dim newGrade As New Grade(5, newCourse)
                    newGrade.Letter = classControl.cmbTerm5Grade.Text
                    newCourse.Grades.Add(newGrade)
                End If
                If Not String.IsNullOrEmpty(classControl.cmbTerm6Grade.Text) Then
                    Dim newGrade As New Grade(6, newCourse)
                    newGrade.Letter = classControl.cmbTerm6Grade.Text
                    newCourse.Grades.Add(newGrade)
                End If
                'Verification
                If classControl.radClassAcceptableYes.Checked Then
                    newCourse.IsAcceptable = "Yes"
                ElseIf classControl.radClassAcceptableNo.Checked Then
                    newCourse.IsAcceptable = "No"
                ElseIf classControl.radClassAcceptableInProgress.Checked Then
                    newCourse.IsAcceptable = "In Progress"
                End If
                newCourse.Verification.UserId = classControl.txtClassVerifiedBy.Text
                If classControl.txtClassVerifiedDate.Text.Length = 0 Then
                    newCourse.Verification.TimeStamp = Nothing
                Else
                    newCourse.Verification.TimeStamp = Date.Parse(classControl.txtClassVerifiedDate.Text)
                End If
                'Add a this course to the collection and move on to the next course.
                courses.Add(newCourse)
                Continue For
            End If

            'If this class control has no class title set and the key (course sequence number)
            'exists in the student's course sequence numbers, then we're deleting this course.
            If String.IsNullOrEmpty(classControl.cmbTitle.Text) _
            AndAlso courses.Select(Function(p) p.SequenceNo).Contains(sequenceNumber) Then
                'Remove this course from the collection.
                Dim oldCourse As Course = courses.Where(Function(p) p.SequenceNo = sequenceNumber).Single()
                courses.Remove(oldCourse)
                'Move on to the next course.
                Continue For
            End If

            'If this class control has a class title set and the key (course sequence number)
            'exists in the student's course sequence numbers, then we're updating this course.
            If Not String.IsNullOrEmpty(classControl.cmbTitle.Text) _
            AndAlso courses.Select(Function(p) p.SequenceNo).Contains(sequenceNumber) Then
                'Get this course from the collection.
                Dim thisCourse As Course = courses.Where(Function(p) p.SequenceNo = sequenceNumber).Single()
                'Set the course members.
                'Class title, weight, grade level, and credits
                thisCourse.Title = classControl.cmbTitle.Text
                thisCourse.Weight = classControl.cmbWeightDesignation.Text
                thisCourse.ConcurrentCollege = classControl.cmbConcurrentCollege.Text
                thisCourse.GradeLevel = classControl.txtGradeLevel.Text
                thisCourse.AcademicYearTaken = classControl.txtAcademicYear.Text
                thisCourse.SchoolAttended = classControl.cmbSchoolAttended.SelectedValue
                If classControl.txtCredits.Text = "" Then
                    thisCourse.Credits = 0
                Else
                    If Not Double.TryParse(classControl.txtCredits.Text, thisCourse.Credits) Then
                        Dim message As String = String.Format("Class credits for {0} must be numeric.", thisCourse.Title)
                        Throw New RegentsInvalidDataException(message)
                    End If
                End If
                'Grades
                If thisCourse.Grades.ContainsKey(1) Then
                    'See whether the user blanked the grade.
                    If String.IsNullOrEmpty(classControl.cmbTerm1Grade.Text) Then
                        'Blanked. Remove it from the collection.
                        thisCourse.Grades.Remove(1)
                    Else
                        'Make sure the grade in the collection reflects what's on the screen.
                        thisCourse.Grades(1).Letter = classControl.cmbTerm1Grade.Text
                    End If
                ElseIf Not String.IsNullOrEmpty(classControl.cmbTerm1Grade.Text) Then
                    'A grade for a new term was selected. Add it to the collection.
                    Dim newGrade As New Grade(1, thisCourse)
                    newGrade.Letter = classControl.cmbTerm1Grade.Text
                    thisCourse.Grades.Add(newGrade)
                End If
                If thisCourse.Grades.ContainsKey(2) Then
                    If String.IsNullOrEmpty(classControl.cmbTerm2Grade.Text) Then
                        thisCourse.Grades.Remove(2)
                    Else
                        thisCourse.Grades(2).Letter = classControl.cmbTerm2Grade.Text
                    End If
                ElseIf Not String.IsNullOrEmpty(classControl.cmbTerm2Grade.Text) Then
                    Dim newGrade As New Grade(2, thisCourse)
                    newGrade.Letter = classControl.cmbTerm2Grade.Text
                    thisCourse.Grades.Add(newGrade)
                End If
                If thisCourse.Grades.ContainsKey(3) Then
                    If String.IsNullOrEmpty(classControl.cmbTerm3Grade.Text) Then
                        thisCourse.Grades.Remove(3)
                    Else
                        thisCourse.Grades(3).Letter = classControl.cmbTerm3Grade.Text
                    End If
                ElseIf Not String.IsNullOrEmpty(classControl.cmbTerm3Grade.Text) Then
                    Dim newGrade As New Grade(3, thisCourse)
                    newGrade.Letter = classControl.cmbTerm3Grade.Text
                    thisCourse.Grades.Add(newGrade)
                End If
                If thisCourse.Grades.ContainsKey(4) Then
                    If String.IsNullOrEmpty(classControl.cmbTerm4Grade.Text) Then
                        thisCourse.Grades.Remove(4)
                    Else
                        thisCourse.Grades(4).Letter = classControl.cmbTerm4Grade.Text
                    End If
                ElseIf Not String.IsNullOrEmpty(classControl.cmbTerm4Grade.Text) Then
                    Dim newGrade As New Grade(4, thisCourse)
                    newGrade.Letter = classControl.cmbTerm4Grade.Text
                    thisCourse.Grades.Add(newGrade)
                End If
                If thisCourse.Grades.ContainsKey(5) Then
                    If String.IsNullOrEmpty(classControl.cmbTerm5Grade.Text) Then
                        thisCourse.Grades.Remove(5)
                    Else
                        thisCourse.Grades(5).Letter = classControl.cmbTerm5Grade.Text
                    End If
                ElseIf Not String.IsNullOrEmpty(classControl.cmbTerm5Grade.Text) Then
                    Dim newGrade As New Grade(5, thisCourse)
                    newGrade.Letter = classControl.cmbTerm5Grade.Text
                    thisCourse.Grades.Add(newGrade)
                End If
                If thisCourse.Grades.ContainsKey(6) Then
                    If String.IsNullOrEmpty(classControl.cmbTerm6Grade.Text) Then
                        thisCourse.Grades.Remove(6)
                    Else
                        thisCourse.Grades(6).Letter = classControl.cmbTerm6Grade.Text
                    End If
                ElseIf Not String.IsNullOrEmpty(classControl.cmbTerm6Grade.Text) Then
                    Dim newGrade As New Grade(6, thisCourse)
                    newGrade.Letter = classControl.cmbTerm6Grade.Text
                    thisCourse.Grades.Add(newGrade)
                End If
                'Verification
                If classControl.radClassAcceptableYes.Checked Then
                    thisCourse.IsAcceptable = "Yes"
                ElseIf classControl.radClassAcceptableNo.Checked Then
                    thisCourse.IsAcceptable = "No"
                ElseIf classControl.radClassAcceptableInProgress.Checked Then
                    thisCourse.IsAcceptable = "In Progress"
                End If
                thisCourse.Verification.UserId = classControl.txtClassVerifiedBy.Text
                If classControl.txtClassVerifiedDate.Text.Length = 0 Then
                    thisCourse.Verification.TimeStamp = Nothing
                Else
                    thisCourse.Verification.TimeStamp = DateTime.Parse(classControl.txtClassVerifiedDate.Text)
                End If
                'Move on to the next course.
                Continue For
            End If
        Next
    End Sub
#End Region

    Private Sub SaveDefermentAndLeave()
        'Set the student's deferment according to the reason text.
        If String.IsNullOrEmpty(cmbApplicationDefermentReason.Text) Then
            _student.ScholarshipApplication.Deferment = Nothing
        Else
            _student.ScholarshipApplication.Deferment = New LeaveDeferral(RegentsScholarshipBackEnd.Constants.LeaveDeferralType.DEFERRAL, _student.ScholarshipApplication) With {.BeginDate = dtpApplicationDefermentBeginDate.Value, .EndDate = dtpApplicationDefermentEndDate.Value, .Reason = cmbApplicationDefermentReason.Text}
        End If

        'Set the student's leave of absence according to the reason text.
        If String.IsNullOrEmpty(cmbApplicationLeaveOfAbsenceReason.Text) Then
            _student.ScholarshipApplication.LeaveOfAbsence = Nothing
        Else
            _student.ScholarshipApplication.LeaveOfAbsence = New LeaveDeferral(RegentsScholarshipBackEnd.Constants.LeaveDeferralType.LEAVE_OF_ABSENCE, _student.ScholarshipApplication) With {.BeginDate = dtpApplicationLeaveOfAbsenceBeginDate.Value, .EndDate = dtpApplicationLeaveOfAbsenceEndDate.Value, .Reason = cmbApplicationLeaveOfAbsenceReason.Text}
        End If
    End Sub

    Private Sub SaveDenials()
        'The quick-and-dirty way to set the denial reasons is to wipe out any that
        'already exist, and add whatever's in the combo boxes.
        '(It's dirty because it doesn't account for the possibility
        ' that the student has more than six denial reasons.)
        _student.ScholarshipApplication.DenialReasons = New HashSet(Of String)
        If (Not String.IsNullOrEmpty(cmbApplicationDenialReason1.Text)) Then
            _student.ScholarshipApplication.DenialReasons.Add(cmbApplicationDenialReason1.Text)
        End If
        If (Not String.IsNullOrEmpty(cmbApplicationDenialReason2.Text)) Then
            _student.ScholarshipApplication.DenialReasons.Add(cmbApplicationDenialReason2.Text)
        End If
        If (Not String.IsNullOrEmpty(cmbApplicationDenialReason3.Text)) Then
            _student.ScholarshipApplication.DenialReasons.Add(cmbApplicationDenialReason3.Text)
        End If
        If (Not String.IsNullOrEmpty(cmbApplicationDenialReason4.Text)) Then
            _student.ScholarshipApplication.DenialReasons.Add(cmbApplicationDenialReason4.Text)
        End If
        If (Not String.IsNullOrEmpty(cmbApplicationDenialReason5.Text)) Then
            _student.ScholarshipApplication.DenialReasons.Add(cmbApplicationDenialReason5.Text)
        End If
        If (Not String.IsNullOrEmpty(cmbApplicationDenialReason6.Text)) Then
            _student.ScholarshipApplication.DenialReasons.Add(cmbApplicationDenialReason6.Text)
        End If
    End Sub

    Private Sub SaveDocumentStatusDates()
        'As with denials, the quick-and-dirty way to set document status dates
        'is to wipe out the existing list and add status dates where appropriate.
        _student.ScholarshipApplication.DocumentStatusDates = New Dictionary(Of String, Nullable(Of Date))
        'Application received
        If dtpApplicationReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.APPLICATION, dtpApplicationReceivedDate.Value)
        End If
        'Initial high school transcript received
        If dtpApplicationInitialHighSchoolTranscriptReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.INITIAL_HIGH_SCHOOL_TRANSCRIPT, dtpApplicationInitialHighSchoolTranscriptReceivedDate.Value)
        End If
        'Final high school transcript received
        If dtpApplicationFinalHighSchoolTranscriptReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.FINAL_HIGH_SCHOOL_TRANSCRIPT, dtpApplicationFinalHighSchoolTranscriptReceivedDate.Value)
        End If
        'High school schedule received
        If dtpHighSchoolScheduleReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.HIGH_SCHOOL_SCHEDULE, dtpHighSchoolScheduleReceivedDate.Value)
        End If
        'Initial college transcript received
        If dtpApplicationInitialCollegeTranscriptReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.INITIAL_COLLEGE_TRANSCRIPT, dtpApplicationInitialCollegeTranscriptReceivedDate.Value)
        End If
        'Final college transcript received
        If dtpApplicationFinalCollegeTranscriptReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.FINAL_COLLEGE_TRANSCRIPT, dtpApplicationFinalCollegeTranscriptReceivedDate.Value)
        End If
        'Signature page received
        If dtpApplicationSignaturePageReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.SIGNATURE_PAGE, dtpApplicationSignaturePageReceivedDate.Value)
        End If
        'Conditional acceptance form received
        If dtpApplicationConditionalAcceptanceFormReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.CONDITIONAL_ACCEPTANCE_FORM, dtpApplicationConditionalAcceptanceFormReceivedDate.Value)
        End If
        'Proof of enrollment received
        If dtpApplicationProofOfEnrollmentReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.PROOF_OF_ENROLLMENT, dtpApplicationProofOfEnrollmentReceivedDate.Value)
        End If
        'Proof of citizenship received
        If dtpApplicationProofOfCitizenshipReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.PROOF_OF_CITIZENSHIP, dtpApplicationProofOfCitizenshipReceivedDate.Value)
        End If
        'Appeal received
        If dtpApplicationAppealReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.APPEAL, dtpApplicationAppealReceivedDate.Value)
        End If
        'Appeal decision
        If dtpApplicationAppealDecisionDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.APPEAL_DECISION, dtpApplicationAppealDecisionDate.Value)
        End If
        'LOA request received
        If dtpApplicationLoaRequestReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.LEAVE_OF_ABSENCE_REQUEST, dtpApplicationLoaRequestReceivedDate.Value)
        End If
        'LOA decision
        If dtpApplicationLoaDecisionDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.LEAVE_OF_ABSENCE_DECISION, dtpApplicationLoaDecisionDate.Value)
        End If
        'Deferment request received
        If dtpApplicationDefermentRequestReceivedDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.DEFERMENT_REQUEST, dtpApplicationDefermentRequestReceivedDate.Value)
        End If
        'Deferment decision
        If dtpApplicationDefermentDecisionDate.Value IsNot Nothing Then
            _student.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.DEFERMENT_DECISION, dtpApplicationDefermentDecisionDate.Value)
        End If
    End Sub

    Private Sub SaveHighSchool()
        'Name, city, state, and CEEB code (the HighSchool class will check that they jive)
        _student.HighSchool.Name = cmbApplicationHighSchoolName.Text
        _student.HighSchool.City = txtApplicationHighSchoolCity.Text
        _student.HighSchool.District = txtApplicationHighSchoolDistrict.Text
        _student.HighSchool.CeebCode = txtApplicationCeebCode.Text

        'USBCT status
        If radApplicationUbsctFail.Checked Then
            _student.HighSchool.UsbctStatus = RegentsScholarshipBackEnd.Constants.UsbctStatus.FAIL
        ElseIf radApplicationUbsctPass.Checked Then
            _student.HighSchool.UsbctStatus = RegentsScholarshipBackEnd.Constants.UsbctStatus.PASS
        End If

        'As with denials, the quick-and-dirty way to set the ACT scores is to
        'wipe out any existing ones and add those that are set on the form.
        _student.HighSchool.ActScores = New Dictionary(Of String, Double)
        If Not String.IsNullOrEmpty(txtApplicationActCompositeScore.Text) Then
            _student.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.COMPOSITE, Double.Parse(txtApplicationActCompositeScore.Text))
        End If
        If Not String.IsNullOrEmpty(txtApplicationActEnglishScore.Text) Then
            _student.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.ENGLISH, Double.Parse(txtApplicationActEnglishScore.Text))
        End If
        If Not String.IsNullOrEmpty(txtApplicationActMathScore.Text) Then
            _student.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.MATH, Double.Parse(txtApplicationActMathScore.Text))
        End If
        If Not String.IsNullOrEmpty(txtApplicationActReadingScore.Text) Then
            _student.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.READING, Double.Parse(txtApplicationActReadingScore.Text))
        End If
        If Not String.IsNullOrEmpty(txtApplicationActScienceScore.Text) Then
            _student.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.SCIENCE, Double.Parse(txtApplicationActScienceScore.Text))
        End If
    End Sub

    Private Sub SaveReviews()
        'There's nothing to update for reviews; all we can do is add or delete them.
        'That being the case, the quick-and-dirty way is once again
        'to delete all of them and add whichever ones are checked.
        Dim reviews As ReviewDictionary = _student.ScholarshipApplication.Reviews
        Dim firstQuickReviewIsInProgress As Boolean = False
        If (reviews.ContainsKey(ReviewType.FIRST_QUICK) AndAlso Not reviews(ReviewType.FIRST_QUICK).CompletionDate.HasValue) Then firstQuickReviewIsInProgress = True
        Dim secondQuickReviewIsInProgress As Boolean = False
        If (reviews.ContainsKey(ReviewType.SECOND_QUICK) AndAlso Not reviews(ReviewType.SECOND_QUICK).CompletionDate.HasValue) Then secondQuickReviewIsInProgress = True
        Dim uespReviewIsInProgress As Boolean = False
        If (reviews.ContainsKey(ReviewType.UESP_AWARD) AndAlso Not reviews(ReviewType.UESP_AWARD).CompletionDate.HasValue) Then uespReviewIsInProgress = True
        reviews = New ReviewDictionary()

        'Initial transcript review
        If chkApplicationInitialTranscriptReview.Checked Then
            reviews.Add(New Review(ReviewType.INITIAL_TRANSCRIPT, _student.ScholarshipApplication) With {.UserId = txtApplicationInitialTranscriptReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationInitialTranscriptReviewDate.Text)})
        End If
        'Second transcript review
        If chkApplicationSecondTranscriptReview.Checked Then
            reviews.Add(New Review(ReviewType.SECOND_TRANSCRIPT, _student.ScholarshipApplication) With {.UserId = txtApplicationSecondTranscriptReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationSecondTranscriptReviewDate.Text)})
        End If
        'Final transcript review
        If chkApplicationFinalTranscriptReview.Checked Then
            reviews.Add(New Review(ReviewType.FINAL_TRANSCRIPT, _student.ScholarshipApplication) With {.UserId = txtApplicationFinalTranscriptReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationFinalTranscriptReviewDate.Text)})
        End If
        'Class review
        If chkApplicationClassReview.Checked Then
            reviews.Add(New Review(ReviewType.CLASS_, _student.ScholarshipApplication) With {.UserId = txtApplicationClassReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationClassReviewDate.Text)})
        End If
        'Category review
        If chkApplicationCategoryReview.Checked Then
            reviews.Add(New Review(ReviewType.CATEGORY, _student.ScholarshipApplication) With {.UserId = txtApplicationCategoryReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationCategoryReviewDate.Text)})
        End If
        'Initial award review
        If chkApplicationInitialAwardReview.Checked Then
            reviews.Add(New Review(ReviewType.INITIAL_AWARD, _student.ScholarshipApplication) With {.UserId = txtApplicationInitialAwardReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationInitialAwardReviewDate.Text)})
        End If
        'First quick review
        If chkApplicationFirstQuickReview.Checked Then
            reviews.Add(New Review(ReviewType.FIRST_QUICK, _student.ScholarshipApplication) With {.UserId = txtApplicationFirstQuickReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationFirstQuickReviewDate.Text)})
        ElseIf firstQuickReviewIsInProgress Then
            reviews.Add(New Review(ReviewType.FIRST_QUICK, _student.ScholarshipApplication))
        End If
        'Base award review
        If chkApplicationBaseAwardReview.Checked Then
            reviews.Add(New Review(ReviewType.BASE_AWARD, _student.ScholarshipApplication) With {.UserId = txtApplicationBaseAwardReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationBaseAwardReviewDate.Text)})
        End If
        'Exemplary award review
        If chkApplicationExemplaryAwardReview.Checked Then
            reviews.Add(New Review(ReviewType.EXEMPLARY_AWARD, _student.ScholarshipApplication) With {.UserId = txtApplicationExemplaryAwardReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationExemplaryAwardReviewDate.Text)})
        End If
        'UESP award review
        If chkApplicationUespAwardReview.Checked Then
            reviews.Add(New Review(ReviewType.UESP_AWARD, _student.ScholarshipApplication) With {.UserId = txtApplicationUespAwardReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationUespAwardReviewDate.Text)})
        ElseIf uespReviewIsInProgress Then
            reviews.Add(New Review(ReviewType.UESP_AWARD, _student.ScholarshipApplication))
        End If
        'Second quick review
        If chkApplicationSecondQuickReview.Checked Then
            reviews.Add(New Review(ReviewType.SECOND_QUICK, _student.ScholarshipApplication) With {.UserId = txtApplicationSecondQuickReviewUserId.Text, .CompletionDate = Date.Parse(txtApplicationSecondQuickReviewDate.Text)})
        ElseIf secondQuickReviewIsInProgress Then
            reviews.Add(New Review(ReviewType.SECOND_QUICK, _student.ScholarshipApplication))
        End If

        _student.ScholarshipApplication.Reviews = reviews
    End Sub
#End Region 'Saving
#End Region 'Special data binding

#Region "Main menu search"
    Private Sub ClearSearchCriteria()
        txtMainMenuSearchDateOfBirth.Text = ""
        txtMainMenuSearchFirstName.Text = ""
        txtMainMenuSearchLastName.Text = ""
        txtMainMenuSearchSsn.Text = ""
        txtMainMenuSearchStateStudentId.Text = ""
    End Sub

    Private Sub ClearSearchResults()
        lblMainMenuNoSearchResults.Visible = False
        lblMainMenuSelectMatch.Visible = False
        dgvMainMenuSearchMatches.Visible = False
        dgvMainMenuSearchMatches.DataSource = Nothing
    End Sub

    Private Sub Search()
        ClearSearchResults()
        'Call SortedSearch, passing in a default column name.
        SortedSearch("StateStudentID", True)
    End Sub

    Private Sub SortedSearch(ByVal sortColumnName As String, ByVal sortAscending As Boolean)
        Cursor = Cursors.WaitCursor

        'Get the search criteria from the input widgets.
        Dim studentId As String = txtMainMenuSearchStateStudentId.Text
        Dim ssn As String = txtMainMenuSearchSsn.Text
        Dim firstName As String = txtMainMenuSearchFirstName.Text
        Dim lastName As String = txtMainMenuSearchLastName.Text
        Dim dateOfBirth As Date = Date.MinValue
        DateTime.TryParse(txtMainMenuSearchDateOfBirth.Text, dateOfBirth)

        'Search the database for any matches.
        Dim searchResults As IEnumerable(Of MainMenuSearchResult) = DataAccess.GetMainMenuSearchResults(studentId, ssn, firstName, lastName, dateOfBirth)

        'Sort according to the passed-in column name and sort order.
        Select Case sortColumnName
            Case "City"
                If sortAscending Then
                    searchResults = searchResults.OrderBy(Function(p) p.City)
                Else
                    searchResults = searchResults.OrderByDescending(Function(p) p.City)
                End If
            Case "FirstName"
                If sortAscending Then
                    searchResults = searchResults.OrderBy(Function(p) p.FirstName)
                Else
                    searchResults = searchResults.OrderByDescending(Function(p) p.FirstName)
                End If
            Case "LastName"
                If sortAscending Then
                    searchResults = searchResults.OrderBy(Function(p) p.LastName)
                Else
                    searchResults = searchResults.OrderByDescending(Function(p) p.LastName)
                End If
            Case "SocialSecurityNumber"
                If sortAscending Then
                    searchResults = searchResults.OrderBy(Function(p) p.SocialSecurityNumber)
                Else
                    searchResults = searchResults.OrderByDescending(Function(p) p.SocialSecurityNumber)
                End If
            Case "StateStudentID"
                If sortAscending Then
                    searchResults = searchResults.OrderBy(Function(p) p.StateStudentID)
                Else
                    searchResults = searchResults.OrderByDescending(Function(p) p.StateStudentID)
                End If
            Case "StreetAddressLine1"
                If sortAscending Then
                    searchResults = searchResults.OrderBy(Function(p) p.StreetAddressLine1)
                Else
                    searchResults = searchResults.OrderByDescending(Function(p) p.StreetAddressLine1)
                End If
            Case "AwardStatus"
                If sortAscending Then
                    searchResults = searchResults.OrderBy(Function(p) p.AwardStatus)
                Else
                    searchResults = searchResults.OrderByDescending(Function(p) p.AwardStatus)
                End If
        End Select

        Cursor = Cursors.Default

        'Proceed according to the number of results.
        searchResults = searchResults.ToList()
        Select Case searchResults.Count()
            Case 0
                'No results. Show the label that says as much.
                lblMainMenuNoSearchResults.Visible = True
            Case 1
                'One result. Load it up.
                LoadStudent(searchResults(0).StateStudentID)
            Case Else
                'Multiple results. Load them all into the data grid.
                lblMainMenuSelectMatch.Visible = True
                dgvMainMenuSearchMatches.DataSource = searchResults
                dgvMainMenuSearchMatches.Columns("AwardStatus").Visible = (_user.AccessLevel = RegentsScholarshipBackEnd.Constants.AccessLevel.APPLICATION_REVIEW)
                dgvMainMenuSearchMatches.Visible = True
        End Select
    End Sub
#End Region

    Private Function ApplicationConstraintsAreMet() As Boolean
        'Set up a string list to house any problem descriptions that come up.
        Dim problemDescriptions As New List(Of String)()

        'See if the denial reasons jive with the award status.
        Dim denialReasonExists As Boolean = False
        For Each combo As ComboBox In grpApplicationDenialReasons.Controls
            If (combo.Text <> "") Then
                denialReasonExists = True
                Exit For
            End If
        Next
        'An award status of "Denied" requires denial reasons to be set.
        If (cmbApplicationAwardStatus.Text.ToLower() = "denied") AndAlso (Not denialReasonExists) Then
            problemDescriptions.Add("At least one denial reason must be selected if the award status is Denied.")
        End If

        'Deferment fields must be all set if the chosen status is Deferment, or all blank otherwise.
        If (cmbApplicationAwardStatus.Text = RegentsScholarshipBackEnd.Constants.AwardStatus.DEFERRED) Then
            If (dtpApplicationDefermentBeginDate.Value Is Nothing) _
            OrElse (dtpApplicationDefermentEndDate.Value Is Nothing) _
            OrElse (cmbApplicationDefermentReason.Text = "") _
            Then
                problemDescriptions.Add("The deferment fields must be filled in if the award status is Deferment.")
            End If
        Else 'Award Status <> Deferment
            If (dtpApplicationDefermentBeginDate.Value IsNot Nothing) _
            OrElse (dtpApplicationDefermentEndDate.Value IsNot Nothing) _
            OrElse (cmbApplicationDefermentReason.Text <> "") _
            Then
                problemDescriptions.Add("The deferment fields must be blank if the award status is not Deferment.")
            End If
        End If

        'Leave of Absence fields must be all set if the chosen status is Leave of Absence, or all blank otherwise.
        If (cmbApplicationAwardStatus.Text = RegentsScholarshipBackEnd.Constants.AwardStatus.LEAVE_OF_ABSENCE) Then
            If (dtpApplicationLeaveOfAbsenceBeginDate.Value Is Nothing) _
            OrElse (dtpApplicationLeaveOfAbsenceEndDate.Value Is Nothing) _
            OrElse (cmbApplicationLeaveOfAbsenceReason.Text = "") _
            Then
                problemDescriptions.Add("The leave of absence fields must be filled in if the award status is Leave of Absence.")
            End If
        Else 'Award Status <> Leave of Absence
            If (dtpApplicationLeaveOfAbsenceBeginDate.Value IsNot Nothing) _
            OrElse (dtpApplicationLeaveOfAbsenceEndDate.Value IsNot Nothing) _
            OrElse (cmbApplicationLeaveOfAbsenceReason.Text <> "") _
            Then
                problemDescriptions.Add("The leave of absence fields must be blank if the award status is not Leave of Absence.")
            End If
        End If

        'Display any problems that came up, or return True if there aren't any.
        If (problemDescriptions.Count > 0) Then
            Dim message As String = String.Format("The changes could not be saved. Please correct the following problems:{0}", Environment.NewLine)
            message += String.Join(Environment.NewLine, problemDescriptions.ToArray())
            MessageBox.Show(message, "Cannot save changes", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub ChangeStudentId()
        'Make sure there's a student loaded.
        If (_student Is Nothing) Then
            Return
        End If
        'The current user must have DCR access to changea student's ID.
        If (_user.AccessLevel <> RegentsScholarshipBackEnd.Constants.AccessLevel.DCR) Then
            Return
        End If
        'Make sure the user has read/write access to the linked document area.
        Try
            Dim testFile As String = String.Format("{0}testAccess", RegentsScholarshipBackEnd.Constants.STUDENT_DOCUMENT_ROOT)
            Using testStream As New StreamWriter(testFile, False)
                testStream.Close()
            End Using
            File.Delete(testFile)
        Catch ex As Exception
            Dim message As String = String.Format("You don't have the needed access rights to the linked document directory ({0}). Please have CNOC grant you read/write aceess for that directory.", RegentsScholarshipBackEnd.Constants.STUDENT_DOCUMENT_ROOT)
            MessageBox.Show(message, "Insufficient access rights", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End Try

        'Get the new ID from the user.
        Dim newStateStudentId As String = InputBox("Enter the new student ID:", "Change State Student ID", _student.StateStudentId)
        If (newStateStudentId = _student.StateStudentId) OrElse (newStateStudentId = "") Then
            Return
        End If
        'Check that the ID fits the 10-character constraint.
        If (newStateStudentId.Length > 10) Then
            Dim message As String = String.Format("{0} is too long for a student ID.", newStateStudentId)
            MessageBox.Show(message, "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        'Make sure the chosen ID isn't already being used by another student.
        If (DataAccess.GetExistingStudentIds().Contains(newStateStudentId)) Then
            Dim message As String = String.Format("{0} is already assigned to another student.", newStateStudentId)
            MessageBox.Show(message, "ID belongs to someone else", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If
        'Check that the ID looks reasonable, based on the same criteria used by the DataImporter.
        Dim dummyInteger As Integer = 0
        If newStateStudentId.Length < 4 _
        OrElse Integer.TryParse(newStateStudentId, dummyInteger) = False _
        OrElse (Integer.TryParse(newStateStudentId, dummyInteger) AndAlso Integer.Parse(newStateStudentId) = 0) Then
            Dim message As String = String.Format("{0} looks abnormal for a student ID. Are you sure you want to use it?", newStateStudentId)
            If MessageBox.Show(message, "Abnormal ID", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> Windows.Forms.DialogResult.Yes Then
                Return
            End If
        End If

        'Work that old black magic.
        Cursor = Cursors.WaitCursor
        Try
            Dim oldStateStudentId As String = _student.StateStudentId
            _student.ChangeStateStudentId(newStateStudentId, _user.Id)
            'Change the name of the linked documents path to reflect the new ID.
            Dim oldPath As String = RegentsScholarshipBackEnd.Constants.STUDENT_DOCUMENT_ROOT + String.Format("Student_{0}\", oldStateStudentId)
            Dim newPath As String = RegentsScholarshipBackEnd.Constants.STUDENT_DOCUMENT_ROOT + String.Format("Student_{0}\", newStateStudentId)
            If (Directory.Exists(oldPath)) Then
                Directory.Move(oldPath, newPath)
            End If
            'Call SetUpBindingSources() so that the form shows the updated student ID.
            SetUpBindingSources()
            'Assure the user that everything went well.
            Dim message As String = String.Format("{0} {1}'s state student ID was successfully changed to {2}.", _student.FirstName, _student.LastName, newStateStudentId)
            MessageBox.Show(message, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As RegentsInvalidDataException
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        Catch ex As Exception
            'Change the cursor back to normal and re-throw the exception.
            Throw
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ClearCommunicationFields()
        txtCommunicationComments.Clear()
        txtCommunicationSubject.Clear()
        txtCommunicationDateTime.Clear()
        txtCommunicationUserId.Clear()
        chkCommunications411.Checked = False
        chkCommunications411.Enabled = True
        btnCommunicationSave.Enabled = True
    End Sub

    Private Sub CommSortedSearch(ByVal sortColumnName As String, ByVal sortAscending As Boolean)
        Dim commSearch As IEnumerable(Of Communication) = DataAccess.GetCommunications(_student.StateStudentId, RegentsScholarshipBackEnd.Constants.CommunicationEntityType.STUDENT)
        Select Case sortColumnName
            Case "UserId"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.UserId)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.UserId)
                End If
            Case "TimeStamp"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.TimeStamp)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.TimeStamp)
                End If
            Case "Type"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.Type)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.Type)
                End If
            Case "Source"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.Source)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.Source)
                End If
            Case "Subject"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.Subject)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.Subject)
                End If
            Case "Comments"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.Text)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.Text)
                End If
        End Select

        CommunicationDataGridView.DataSource = commSearch.ToList()
        CommunicationDataGridView.Refresh()
        CommunicationPrinter.SortColumnName = sortColumnName
        CommunicationPrinter.SortAscending = sortAscending
    End Sub

    Private Sub FetchDocument(ByVal whichScreen As Screen)
        'Make sure a student has been selected.
        If _student Is Nothing Then
            Dim message As String = "Please select a student from the main menu first."
            MessageBox.Show(message, "No student selected", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Return
        End If

        'Set this studen't directory in the document path.
        Dim directoryStudent As String = String.Format("Student_{0}\", _student.StateStudentId)
        'Set the directory branch based on the selected screen.
        Dim directoryBranch As String = ""
        Select Case whichScreen
            Case Screen.Application
                directoryBranch = "Application\"
            Case Screen.Communication
                directoryBranch = "Communication\"
            Case Screen.Payments
                directoryBranch = "Payments\"
        End Select
        'Compose the full path.
        Dim fullPath As String = RegentsScholarshipBackEnd.Constants.STUDENT_DOCUMENT_ROOT + directoryStudent + directoryBranch

        'See if there are any documents.
        If Not Directory.Exists(fullPath) OrElse Directory.GetFiles(fullPath).Count() = 0 Then
            Dim message As String = String.Format("{0} {1} has no linked documents.", _student.FirstName, _student.LastName)
            MessageBox.Show(message, "No documents found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        'Show a file dialog so the user can select a file.
        Dim fetchDialog As New OpenFileDialog()
        fetchDialog.Multiselect = True  'Explicitly support multiple files.
        fetchDialog.InitialDirectory = fullPath
        fetchDialog.ShowDialog()

        'Check that the user selected at least one file.
        If fetchDialog.FileNames.Count() = 0 Then
            Return
        End If

        'Trust Windows to find the correct program to open each file.
        For Each fileName As String In fetchDialog.FileNames
            Try
                Process.Start(fileName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error opening a linked document", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
    End Sub

    Private Sub GiveAccess(ByVal accessLevel As String)
        'Start with everything enabled...
        For Each ctrlApp As Control In tabApplication.Controls
            ctrlApp.Enabled = True
        Next
        For Each ctrlDemo As Control In tabDemographics.Controls
            ctrlDemo.Enabled = True
        Next
        For Each ctrlComm As Control In tabCommunications.Controls
            ctrlComm.Enabled = True
        Next
        For Each ctrlPayments As Control In tabPayments.Controls
            ctrlPayments.Enabled = True
        Next
        For Each ctrlPaymentBatch As Control In tabPaymentBatch.Controls
            ctrlPaymentBatch.Enabled = True
        Next
        '...and disable things as needed.
        Select Case accessLevel
            Case RegentsScholarshipBackEnd.Constants.AccessLevel.READ_ONLY
                For Each ctrl As Control In tabApplication.Controls
                    ctrl.Enabled = False
                Next
                For Each ctrlDemo As Control In tabDemographics.Controls
                    ctrlDemo.Enabled = False
                Next
                For Each ctrlComm As Control In tabCommunications.Controls
                    ctrlComm.Enabled = True
                Next
                For Each ctrlPayments As Control In tabPayments.Controls
                    ctrlPayments.Enabled = False
                Next
                For Each ctrlPaymentBatch As Control In tabPaymentBatch.Controls
                    ctrlPaymentBatch.Enabled = False
                Next
                grpDemographicsContact.Enabled = True
                grpDemographicsAddress.Enabled = True
                grpAuthThirdParties.Enabled = True
                btnDemographicsSaveChanges.Enabled = True
                'Re-enable the Classes group so that the user can switch between
                'class category tabs, but disable the controls on each tab.
                grpApplicationClasses.Enabled = True
                Dim classTabs() As TabPage = {tabEnglish, tabMathematics, tabScienceWithLab, tabSocialScience, tabForeignLanguage}
                For Each classCategory As TabPage In classTabs
                    For Each categoryControl As Control In classCategory.Controls
                        categoryControl.Enabled = False
                    Next categoryControl
                Next classCategory
                'Let the read-only users view application-related documents.
                btnApplicationViewDocuments.Enabled = True
            Case RegentsScholarshipBackEnd.Constants.AccessLevel.APPLICATION_REVIEW
                For Each ctrl As Control In grpDemographicsPersonal.Controls
                    ctrl.Enabled = False
                Next
                For Each ctrlPayments As Control In tabPayments.Controls
                    ctrlPayments.Enabled = False
                Next
                For Each ctrlPaymentBatch As Control In tabPaymentBatch.Controls
                    ctrlPaymentBatch.Enabled = False
                Next
                btnQuickBatchReview.Enabled = True
                If txtDemographicsSsn.TextLength > 0 Then txtDemographicsSsn.Enabled = False
            Case RegentsScholarshipBackEnd.Constants.AccessLevel.PA
                For Each ctrlApp As Control In tabApplication.Controls
                    ctrlApp.Enabled = False
                Next
                For Each ctrlDemo As Control In tabDemographics.Controls
                    ctrlDemo.Enabled = False
                Next
                For Each ctrlComm As Control In tabCommunications.Controls
                    ctrlComm.Enabled = False
                Next
                For Each ctrlDemo As Control In tabDemographics.Controls
                    ctrlDemo.Enabled = False
                Next
                For Each ctrlPayments As Control In tabPayments.Controls
                    ctrlPayments.Enabled = False
                Next
                For Each ctrlPaymentBatch As Control In tabPaymentBatch.Controls
                    ctrlPaymentBatch.Enabled = False
                Next
                grpDemographicsPersonal.Enabled = True
            Case RegentsScholarshipBackEnd.Constants.AccessLevel.BATCH_PROCESSING
                For Each ctrlApp As Control In tabApplication.Controls
                    ctrlApp.Enabled = False
                Next
                For Each ctrlDemo As Control In tabDemographics.Controls
                    ctrlDemo.Enabled = False
                Next
                For Each ctrlComm As Control In tabCommunications.Controls
                    ctrlComm.Enabled = False
                Next
                For Each ctrlPayments As Control In tabPayments.Controls
                    ctrlPayments.Enabled = False
                Next
                For Each ctrlPaymentBatch As Control In tabPaymentBatch.Controls
                    ctrlPaymentBatch.Enabled = False
                Next
            Case RegentsScholarshipBackEnd.Constants.AccessLevel.DCR
                For Each ctrlApp As Control In tabApplication.Controls
                    ctrlApp.Enabled = False
                Next
                For Each ctrlDemo As Control In tabDemographics.Controls
                    ctrlDemo.Enabled = True
                Next
                For Each ctrlPayments As Control In tabPayments.Controls
                    ctrlPayments.Enabled = False
                Next
                For Each ctrlPaymentBatch As Control In tabPaymentBatch.Controls
                    ctrlPaymentBatch.Enabled = False
                Next
                grpDemographicsAddress.Enabled = False
                grpDemographicsContact.Enabled = False
                grpDemographicsEligibility.Enabled = False
                grpAuthThirdParties.Enabled = False
                grpHearAboutUs.Enabled = False
                cmbDemographicsGender.Enabled = False
                cmbDemographicsEthnicity.Enabled = False
                For Each ctrlComm As Control In tabCommunications.Controls
                    ctrlComm.Enabled = False
                Next
                grpCommunicationRecord.Enabled = True
                For Each ctrlComm As Control In grpCommunicationRecord.Controls
                    ctrlComm.Enabled = False
                Next
                CommunicationDataGridView.Enabled = True
            Case RegentsScholarshipBackEnd.Constants.AccessLevel.PAYMENT_PROCESSING, RegentsScholarshipBackEnd.Constants.AccessLevel.PAYMENT_PROCESSING_OVERRIDE
                For Each ctrl As Control In grpDemographicsPersonal.Controls
                    ctrl.Enabled = False
                Next
                If txtDemographicsSsn.TextLength > 0 Then txtDemographicsSsn.Enabled = False
                btnQuickBatchReview.Enabled = True
        End Select
    End Sub

    Private Sub LinkDocument(ByVal whichScreen As Screen)
        'Make sure a student has been selected.
        If _student Is Nothing Then
            Dim message As String = "Please select a student from the main menu first."
            MessageBox.Show(message, "No student selected", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Return
        End If

        'Specify a directory that the file picker should start in.
        Const initialDirectory As String = "\\AD4\Restricted\Regents Scholarships\Scan\"

        'Set this studen't directory in the document path.
        Dim directoryStudent As String = String.Format("Student_{0}\", _student.StateStudentId)
        'Set the directory branch based on the selected screen.
        Dim directoryBranch As String = ""
        Select Case whichScreen
            Case Screen.Application
                directoryBranch = "Application\"
            Case Screen.Communication
                directoryBranch = "Communication\"
            Case Screen.Payments
                directoryBranch = "Payments\"
        End Select
        'Compose the full path.
        Dim fullPath As String = RegentsScholarshipBackEnd.Constants.STUDENT_DOCUMENT_ROOT + directoryStudent + directoryBranch
        'Make sure the full path exists.
        If Not Directory.Exists(fullPath) Then
            Try
                Directory.CreateDirectory(fullPath)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Filesystem error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        'Pop up a file dialog so the user can select the file(s) to link.
        Dim linkDialog As New OpenFileDialog()
        linkDialog.InitialDirectory = initialDirectory
        linkDialog.Multiselect = True   'Explicitly support multiple files.
        linkDialog.ShowDialog()

        'Check that the user selected at least one file to link.
        If linkDialog.FileNames.Count() = 0 Then
            Return
        End If

        'Move all selected files to the network directory.
        For Each fileName As String In linkDialog.FileNames
            Try
                Dim saveName As String = fileName.Substring(fileName.LastIndexOf("\") + 1)
                File.Move(fileName, fullPath + saveName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error linking document", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
    End Sub

    Private Sub LoadComments(ByVal studentId As String)
        CommSortedSearch("TimeStamp", True)
    End Sub

    Private Sub LoadStudent(ByVal stateStudentId As String)
        Cursor = Cursors.WaitCursor

        'Load the selected student from the database.
        _student = Student.Load(stateStudentId)

        'Bind the form's widgets to the data source.
        SetUpBindingSources()

        'Prep the form for displaying a new student and show the Demographics tab.
        SetupTabsForNewStudent()
        tabControlMaster.SelectTab("tabDemographics")

        Cursor = Cursors.Default
    End Sub

    Private Sub LoadThe411()
        pnlDemographics411.Controls.Clear()
        If (_student.DateOfBirth.Date <= DateTime.Now.Date.AddYears(-18)) Then
            Dim adultLabel As New Label()
            adultLabel.AutoSize = True
            adultLabel.ForeColor = Color.Red
            adultLabel.Font = New Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold, GraphicsUnit.Point)
            adultLabel.Text = "This student is 18. Please verify authorization before speaking with anyone else."
            pnlDemographics411.Controls.Add(adultLabel)
        End If
        For Each comm As Communication In DataAccess.GetCommunications(_student.StateStudentId(), RegentsScholarshipBackEnd.Constants.CommunicationEntityType.STUDENT).Where(Function(p) p.Is411)
            pnlDemographics411.Controls.Add(New FourOneOneControl(comm))
        Next comm
    End Sub

    Private Sub ObtainRecordLock()
        'Locks are only useful if a student is loaded.
        If _student Is Nothing Then
            GiveAccess(_user.AccessLevel)
            Return
        End If

        'Attempt to get a lock on this student.
        Dim lockHolder As String = DataAccess.SetLock(_user.Id, _student.StateStudentId)
        If (lockHolder = _user.Id) Then
            'The lock is ours. Set up the form for the user's access level.
            GiveAccess(_user.AccessLevel)
        Else
            'Someone else has a lock. Set up the form for restricted access.
            GiveAccess(RegentsScholarshipBackEnd.Constants.AccessLevel.BATCH_PROCESSING)
            lblHeaderRecordLocked.Text = String.Format("Record locked by {0}", lockHolder)
            lblHeaderRecordLocked.Visible = True
        End If
    End Sub

    Private Sub ReleaseRecordLock()
        'Remove all of this user's locks from the database.
        DataAccess.ReleaseLock(_user.Id)
        'Give the user their normal access level.
        GiveAccess(_user.AccessLevel)
        'Hide the lock label.
        lblHeaderRecordLocked.Visible = False
    End Sub

    Private Sub SaveStudent(ByVal components As Student.Component)
        Cursor = Cursors.WaitCursor

        'Write the widget values back into the data sources.
        For Each twoWayBindingSource As BindingSource In _twoWayBindingSources
            For Each twoWayBinding As Binding In twoWayBindingSource.CurrencyManager.Bindings
                twoWayBinding.WriteValue()
            Next
        Next

        'Some widgets can't do simple data binding, so save their data separately.
        SaveSpecialWidgetData()

        'Check the data and commit it to the database.
        Try
            _student.Commit(_user.Id, components)
            'See if a UESP payment needs to be added.
            Dim approvedStatuses() As String = {RegentsScholarshipBackEnd.Constants.AwardStatus.PENDING_APPROVAL, RegentsScholarshipBackEnd.Constants.AwardStatus.APPROVED, RegentsScholarshipBackEnd.Constants.AwardStatus.DEFERRED}
            Dim studentHasUespPayment As Boolean = (_student.Payments.Where(Function(p) p.Type = RegentsScholarshipBackEnd.Constants.PaymentType.UESP).Count() > 0)
            If (approvedStatuses.Contains(_student.ScholarshipApplication.BaseAward.Status) AndAlso _student.ScholarshipApplication.UespSupplementalAward.IsApproved AndAlso Not studentHasUespPayment) Then
                _student.Payments.Add(New Payment(_student, Payment.AwardType.Uesp))
                _student.Commit(_user.Id, Student.Component.Payments)
            End If
            'Assure the user that everything went well.
            Dim message As String = String.Format("{0} {1}'s details were successfully saved.", _student.FirstName, _student.LastName)
            MessageBox.Show(message, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Re-bind the widgets to refresh the calculated widgets.
            SetUpBindingSources()
        Catch ex As RegentsInvalidDataException
            Dim ef As New ExceptionForm(ex)
            ef.ShowDialog()
        Catch ex As Exception
            'Change the cursor back to normal and re-throw the exception.
            Throw
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub SetupTabsForNewStudent()
        ClearCommunicationFields()
        LoadComments(_student.StateStudentId)
        LoadThe411()

        'Set the denial reasons group box according to the award status.
        If _student.ScholarshipApplication.BaseAward.Status = RegentsScholarshipBackEnd.Constants.AwardStatus.DENIED Then
            'Enable the denials reaons.
            grpApplicationDenialReasons.Enabled = True
        Else
            'Clear and disable all the denial reasons.
            For Each combo As ComboBox In grpApplicationDenialReasons.Controls
                combo.Text = ""
            Next
            grpApplicationDenialReasons.Enabled = False
        End If

        'Enable or disable the appeal decision check boxes depending on whether an appeal decision document exists.
        If (_student.ScholarshipApplication.DocumentStatusDates.ContainsKey(Constants.DocumentType.APPEAL_DECISION)) Then
            lblApplicationAppealDecision.Enabled = True
            chkApplicationAppealApproved.Enabled = True
            chkApplicationAppealDenied.Enabled = True
            'Check the appropriate box as well, since they're not data-bound.
            Dim approvedStatuses() As String = {Constants.AwardStatus.APPROVED, Constants.AwardStatus.CONDITIONAL_APPROVAL}
            If (approvedStatuses.Contains(_student.ScholarshipApplication.BaseAward.Status)) Then
                chkApplicationAppealApproved.Checked = True
            ElseIf (_student.ScholarshipApplication.BaseAward.Status = Constants.AwardStatus.DENIED) Then
                chkApplicationAppealDenied.Checked = True
            Else
                chkApplicationAppealApproved.Checked = False
                chkApplicationAppealDenied.Checked = False
            End If
        Else
            lblApplicationAppealDecision.Enabled = False
            chkApplicationAppealApproved.Checked = False
            chkApplicationAppealApproved.Enabled = False
            chkApplicationAppealDenied.Checked = False
            chkApplicationAppealDenied.Enabled = False
        End If

        'Give communication printer needed data.
        CommunicationPrinter.Entity = _student.StateStudentId
        CommunicationPrinter.EntityType = RegentsScholarshipBackEnd.Constants.CommunicationEntityType.STUDENT
        CommunicationPrinter.EntityName = String.Format("{0} {1}", _student.FirstName, _student.LastName)
        CommunicationPrinter.SortColumnName = "TimeStamp"
        CommunicationPrinter.SortAscending = True
    End Sub
End Class
