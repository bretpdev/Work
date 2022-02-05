Imports System.Text.RegularExpressions
Imports RegentsScholarshipBackEnd

Public Class PaymentControl
    'Define some constants for pattern-matching user-provided text.
    Private Const AMOUNT_PATTERN As String = "^(-)?(\d{0,4})?(\.\d{0,2})?$"
    Private Const CREDITS_PATTERN As String = "^(-)?(\d{0,2})?(\.\d{0,1})?$"
    Private Const GPA_PATTERN As String = "^(\d{0,2})?(\.|\.\d{0,3})?$"
    Private Const YEAR_PATTERN As String = "^\d{0,4}$"

    Private _payment As Payment

    'Declare some variables to hold onto previous user-provided text,
    'so we can revert back to it if new input is not acceptable.
    Private _txtYearPreviousText As String
    Private _txtCreditsPreviousText As String
    Private _txtGpaPreviousText As String
    Private _txtAmountPreviousText As String

    'Create indicators of whether combo boxes have values, so we can
    'avoid re-calculating the payment amount when the form initializes.
    Private _cmbCollegeIsPopulated As Boolean = False
    Private _cmbSemesterIsPopulated As Boolean = False
    Private _cmbTypeIsPopulated As Boolean = False

    ''' <summary>
    ''' DO NOT USE THE DEFAULT CONSTRUCTOR!!!
    ''' It is required for the Windows Forms Designer, but it's useless for coding.
    ''' </summary>
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    Public Sub New(ByVal payment As Payment, ByVal accessLevel As String)
        InitializeComponent()

        'Set up the combo boxes.
        cmbCollege.Items.AddRange(Lookups.Colleges.Where(Function(p) p.IsInDefaultList).Select(Function(p) p.Name).OrderBy(Function(p) p).ToArray())
        cmbSemester.Items.AddRange(Lookups.CollegeTerms.ToArray())
        cmbType.Items.Add(Constants.PaymentType.BASE)
        If (payment.ParentStudent().ScholarshipApplication.ExemplaryAward.IsApproved) Then
            cmbType.Items.Add(Constants.PaymentType.EXEMPLARY)
        End If
        If (payment.ParentStudent().ScholarshipApplication.UespSupplementalAward.IsApproved) Then
            cmbType.Items.Add(Constants.PaymentType.UESP)
        End If

        'Disallow future dates on the DateTimePickers.
        dtpGradesReceived.MaxDate = DateTime.Now
        dtpScheduleReceived.MaxDate = DateTime.Now

        'Bind the form's controls to the passed-in Payment object.
        _payment = payment
        PaymentBindingSource.DataSource = _payment

        'Set access according to the user's access level.
        Select Case accessLevel
            Case Constants.AccessLevel.PAYMENT_PROCESSING_OVERRIDE
                'Disable the payment amount text box if the payment is not pending.
                If (payment.Status <> Constants.PaymentStatus.PENDING) Then
                    txtAmount.Enabled = False
                End If
            Case Constants.AccessLevel.PAYMENT_PROCESSING
                'Disable the override check boxes and payment amount text box.
                chkCreditsOverride.Enabled = False
                chkGpaOverride.Enabled = False
                txtAmount.Enabled = False
            Case Else
                'Disable everything.
                For Each ctrl As Control In Me.Controls
                    ctrl.Enabled = False
                Next
        End Select

        'Disable everything if the payment is already approved.
        If (payment.Status = Constants.PaymentStatus.APPROVED) Then
            For Each ctrl As Control In Me.Controls
                ctrl.Enabled = False
            Next ctrl
        End If

        'Set the private "PreviousText" variables so we have something to fall back on.
        _txtAmountPreviousText = _payment.Amount
        _txtCreditsPreviousText = _payment.Credits
        _txtGpaPreviousText = _payment.Gpa
        _txtYearPreviousText = _payment.Year
    End Sub

#Region "EventHandlers"
    Private Sub chkDelete_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDelete.CheckedChanged
        'The UESP payment can't be deleted if the student is marked as having earned it, so watch for that situation.
        If (chkDelete.Checked AndAlso _payment.Type = Constants.PaymentType.UESP AndAlso _payment.ParentStudent().ScholarshipApplication.UespSupplementalAward.IsApproved) Then
            Dim message As String = "To delete the UESP payment, you must first un-check ""UESP Supplemental Award Approved"" on the Application page."
            MessageBox.Show(message, "Cannot Delete Payment", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            chkDelete.Checked = False
        End If
    End Sub

    Private Sub cmbCollege_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCollege.SelectedValueChanged
        If (_cmbCollegeIsPopulated) Then
            UpdatePaymentAmount()
        Else
            _cmbCollegeIsPopulated = True
        End If
    End Sub

    Private Sub cmbSemester_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSemester.SelectedValueChanged
        If (_cmbSemesterIsPopulated) Then
            UpdatePaymentAmount()
        Else
            _cmbSemesterIsPopulated = True
        End If
    End Sub

    Private Sub cmbType_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedValueChanged
        If (_cmbTypeIsPopulated) Then
            UpdatePaymentAmount()
        Else
            _cmbTypeIsPopulated = True
        End If
    End Sub

    Private Sub txtAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAmount.TextChanged
        CheckTextPattern(txtAmount, AMOUNT_PATTERN, _txtAmountPreviousText)
    End Sub

    Private Sub txtCredits_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCredits.Leave
        If String.IsNullOrEmpty(txtCredits.Text) Then txtCredits.Text = "0"
        If (_payment.ParentStudent().CumulativeCreditHoursPaid() + Double.Parse(txtCredits.Text) > Constants.MAX_CREDIT_HOURS_PAYABLE) Then
            txtCredits.Text = Constants.MAX_CREDIT_HOURS_PAYABLE - _payment.ParentStudent().CumulativeCreditHoursPaid()
            Dim message As String = String.Format("The student only has {0} credits eligibility remaining, so the credit hours are limited to that number.", txtCredits.Text)
            MessageBox.Show(message, "Exceeded Credit Limit", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        UpdatePaymentAmount()
    End Sub

    Private Sub txtCredits_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCredits.TextChanged
        CheckTextPattern(txtCredits, CREDITS_PATTERN, _txtCreditsPreviousText)
    End Sub

    Private Sub txtGpa_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGpa.TextChanged
        CheckTextPattern(txtGpa, GPA_PATTERN, _txtGpaPreviousText)
    End Sub

    Private Sub txtYear_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtYear.Leave
        'If the user doesn't enter a full year, fill in the digits to the left.
        Select Case txtYear.Text.Length
            Case 0
                txtYear.Text = DateTime.Now.Year.ToString()
            Case 1
                txtYear.Text = "200" + txtYear.Text
            Case 2
                txtYear.Text = "20" + txtYear.Text
            Case 3
                txtYear.Text = "2" + txtYear.Text
		End Select

		UpdatePaymentAmount()
	End Sub

    Private Sub txtYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtYear.TextChanged
        CheckTextPattern(txtYear, YEAR_PATTERN, _txtYearPreviousText)
    End Sub
#End Region

    Private Sub CheckTextPattern(ByVal textBoxControl As TextBox, ByVal pattern As String, ByRef previousText As String)
        If (Regex.IsMatch(textBoxControl.Text, pattern)) Then
            previousText = textBoxControl.Text
        Else
            If (Regex.IsMatch(previousText, pattern)) Then
                textBoxControl.Text = previousText
            Else
                textBoxControl.Text = ""
                previousText = ""
            End If
        End If
    End Sub

    Private Sub UpdatePaymentAmount()
        'All of the fields used in the calculation are bound to data that will never be blank, but the fields start out blank
        'until the form's initializer populates them. Don't run the calculation if they're not all populated yet.
        If (String.IsNullOrEmpty(cmbCollege.Text) OrElse String.IsNullOrEmpty(cmbSemester.Text) OrElse String.IsNullOrEmpty(cmbType.Text) OrElse String.IsNullOrEmpty(txtCredits.Text) OrElse String.IsNullOrEmpty(txtYear.Text)) Then Return
        Try
            txtAmount.Text = DataAccess.GetCalculatedPaymentAmount(_payment.ParentStudent.StateStudentId, cmbCollege.Text, txtYear.Text, cmbSemester.Text, cmbType.Text, txtCredits.Text)
        Catch ex As Exception
            MessageBox.Show("The payment can't be calculated because there is no tuition data for the information entered.", "Payment not Calculated", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtAmount.Text = "0"
        End Try
    End Sub
End Class
