Imports System.Collections.Generic
Imports System.Drawing
Imports System.Threading
Imports System.Windows.Forms

Public Class frmBSHomePage
#Region "Class Variables"
    'class variables (objects) these objects interact with Reflection
    Private _loanStatuses As New List(Of String)()

    Public DemographicsForm As SP.frmDemographics
    Public PhoneIndicatorForm As frmPhoneIndicator
    Private _contactPhone As New BorrowerContactPhone(_bsBorrower)
    Private _noteDudeForm As BSNoteDude
    Private _oneLinkNoteDudeForm As BSNoteDude
    Private _scriptsAndServicesForm As SP.frmScriptsAndServices
    Private _compassLoansForm As frmCompassLoans
    Private _privateLoansForm As frmPrivateLoans
    Private _defermentAndForbearanceHistoryForm As SP.frmDeferForbHistory
    Private _employerInfoForm As frmEmployer
    Private _contactForm As MDBSContact.frmContact
    Private _activityAndContactCode As MDBSContact.frmContact.ContactType
    Private _paymentHistoryForm As frmPaymentHistory
    Private _moStatusForm As frmMoreStatus
    Private _thirtyDayActivityHistoryForm As frmActivityHistory
    Private _ninetyDayActivityHistoryForm As frmActivityHistory
    Private _oneEightyDayActivityHistoryForm As frmActivityHistory
    Private _threeSixtyDayActivityHistoryForm As frmActivityHistory
    Private _oneLink30DayActivityHistoryForm As frmActivityHistory
    Private _oneLink90DayActivityHistoryForm As frmActivityHistory
    Private _oneLink180DayActivityHistoryForm As frmActivityHistory
    Private _oneLink360DayActivityHistoryForm As frmActivityHistory
    Private _borrowerBenefitsForm As frmBorrowerBenefits
    Private _oneLinkLoansForm As frmOneLinkLoans
    Private _directDebitForm As frmDirectDebit
    Private _thirdPartyForm As frm3rdParty

    Private _bsBorrower As BorrowerBS
    Private _isAuthorizedThirdParty As Boolean
#End Region

    Public Sub New(ByVal liteBorrower As SP.BorrowerLite)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _bsBorrower = New BorrowerBS(liteBorrower)
        'imaging
        Dim resourceManager As New Resources.ResourceManager("MDBSHome.MauiDUDERes", Me.GetType.Assembly)
        If DateTime.Now.Month = 12 Then
            pbSurf.Image = CType(resourceManager.GetObject("SantaBSHP"), System.Drawing.Image)
        ElseIf DateTime.Now.Month < 6 Then
            pbSurf.Image = CType(resourceManager.GetObject("ShaggyBSHP"), System.Drawing.Image)
        Else
            pbSurf.Image = CType(resourceManager.GetObject("RunningBSHP"), System.Drawing.Image)
        End If
        'populate call category drop down box 
        cbCCCat.Items.AddRange(DataAccess.GetCallCategories(SP.TestMode()).ToArray())
    End Sub

    'this function overloads the frmDemographics show function
    Public Shadows Function Show() As Boolean
        _bsBorrower.MonthlyPA = 0

        lvSS.Items.Clear()
        lstCallF.Items.Clear()
        lstServicer.Items.Clear()

        lblSSN.Text = _bsBorrower.SSN
        'set backcolor
        txtBSHome.BackColor = Me.BackColor
        lstCallF.BackColor = Me.BackColor
        lstServicer.BackColor = Me.BackColor
        lvSS.BackColor = Me.BackColor
        'set forecolor
        txtBSHome.SelectAll()
        txtBSHome.ForeColor = Me.ForeColor
        lstCallF.ForeColor = Me.ForeColor
        lstServicer.ForeColor = Me.ForeColor
        lvSS.ForeColor = Me.ForeColor

        _noteDudeForm = New BSNoteDude(CType(_bsBorrower, SP.Borrower), BSNoteDude.ReflectionSystem.Compass)
        _oneLinkNoteDudeForm = New BSNoteDude(CType(_bsBorrower, SP.Borrower), BSNoteDude.ReflectionSystem.OneLink)
        _defermentAndForbearanceHistoryForm = New SP.frmDeferForbHistory()
        _compassLoansForm = New frmCompassLoans()
        _privateLoansForm = New frmPrivateLoans()
        _paymentHistoryForm = New frmPaymentHistory()
        DemographicsForm = New SP.frmDemographics(_bsBorrower)
        _directDebitForm = New frmDirectDebit()

        _bsBorrower.SetWhetherBorrowerExistsInLco()

        If DemographicsForm.PopulateFrm(True) = False Then Return False

        lblDOB.Text = _bsBorrower.DOB
        lblAN.Text = _bsBorrower.CLAccNum

        Dim turboSpeed As New Thread(AddressOf _bsBorrower.Turbo)
        turboSpeed.Start()

        DemographicsForm.ShowDialog()
        If DemographicsForm.BackButtonClicked Then Return False

        If _bsBorrower.ActivityCode = "TT" Or _bsBorrower.ActivityCode = "TE" Or _bsBorrower.ActivityCode = "TC" Then
            If _bsBorrower.ContactCode = "03" Then
                Dim homePhone As String = _bsBorrower.CompassDemos.Phone
                If Not homePhone = "" Then
                    homePhone = homePhone.Insert(3, "-")
                    homePhone = homePhone.Insert(7, "-")
                End If
                Dim otherPhone As String = _bsBorrower.CompassDemos.OtherPhoneNum
                If Not otherPhone = "" Then
                    otherPhone = otherPhone.Insert(3, "-")
                    otherPhone = otherPhone.Insert(7, "-")
                End If
                Dim otherPhone2 As String = _bsBorrower.CompassDemos.OtherPhone2Num
                If Not otherPhone2 = "" Then
                    otherPhone2 = otherPhone2.Insert(3, "-")
                    otherPhone2 = otherPhone2.Insert(7, "-")
                End If
                Dim phoneIndicator As New frmPhoneIndicator(_contactPhone, homePhone, otherPhone, otherPhone2)
                If Not homePhone = "--" Or Not otherPhone = "--" Or Not otherPhone2 = "--" Or Not homePhone = "" Or Not otherPhone = "" Or Not otherPhone2 = "" Then
                    phoneIndicator.ShowDialog()
                End If
            End If
        End If

        SP.Processing.Visible = True
        SP.Processing.Refresh()

        Do While turboSpeed.IsAlive
            Thread.Sleep(100)
        Loop

        _bsBorrower.ReturnToACP()

        SetDemographicsWidgets()

        'retrieve ACP Loan information
        If _bsBorrower.BorLite.NoACPBSVCall = False AndAlso _bsBorrower.FoundAcpLoanData(Me, DemographicsForm.ContactPhone) = False Then
            Return _bsBorrower.ActivityCode.Length > 0
        End If

        _bsBorrower.EnterAcpAndCheckThirdParties(Me)

        'combine statuses from LG10 and TS26 and private loans
        Dim loanStatusDictionary As New Dictionary(Of String, String)()
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If GetStatus(_bsBorrower.LG10Data(5, x), _bsBorrower.LG10Data(6, x)).Length > 0 And loanStatusDictionary.Select(Function(p) p.Value).Contains(GetStatus(_bsBorrower.LG10Data(5, x), _bsBorrower.LG10Data(6, x))) = False Then
                Dim fullStatus As String = GetStatus(_bsBorrower.LG10Data(5, x), _bsBorrower.LG10Data(6, x))
                Dim rank As String = GetStatusRank(fullStatus)
                loanStatusDictionary.Add(rank, fullStatus)
            End If
        Next x
        For Each bsLoan As Loan In _bsBorrower.Loans
            If GetStatus(bsLoan.Status, "").Length > 0 And loanStatusDictionary.Select(Function(p) p.Value).Contains(GetStatus(bsLoan.Status, "")) = False Then
                Dim fullStatus As String = GetStatus(bsLoan.Status, "")
                Dim rank As String = GetStatusRank(fullStatus)
                loanStatusDictionary.Add(rank, fullStatus)
            End If
        Next bsLoan
        For Each fullStatus As String In _bsBorrower.FullStatusDescriptions
            If (Not String.IsNullOrEmpty(fullStatus)) AndAlso (Not loanStatusDictionary.Select(Function(p) p.Value).Contains(fullStatus)) Then
                Dim rank As String = GetStatusRank(fullStatus)
                loanStatusDictionary.Add(rank, fullStatus)
            End If
        Next fullStatus

        'sort by rank
        _loanStatuses = loanStatusDictionary.OrderBy(Function(p) p.Key).Select(Function(p) p.Value).ToList()

        'display to label
        lblStatus.Text = ""
        If _loanStatuses.Count() < 4 Then
            For Each loanStatus As String In _loanStatuses
                lblStatus.Text += "  " + loanStatus
            Next loanStatus
        Else
            btnstatus.Visible = True
            For x As Integer = 0 To 2
                lblStatus.Text += "  " + _loanStatuses(x)
            Next x
        End If

        'display all gathered data

        'Borrower Status
        Dim totalInstallmentAmount As Double = 0
        For Each thisInstallment As Installment In _bsBorrower.Installments
            If thisInstallment.FirstDueDate < Today Then totalInstallmentAmount += thisInstallment.Amount
        Next thisInstallment

        lblNumDaysDelinquent.Text = _bsBorrower.NumDaysDelinquent
        lblDateOfDelinquency.Text = _bsBorrower.DateDelinquencyOccurred
        lblAmountPastDue.Text = FormatCurrency(_bsBorrower.AmountPastDue, 2)
        lblLateFees.Text = FormatCurrency(_bsBorrower.OutstandingLateFees, 2)
        lblCurrentAmountDue.Text = FormatCurrency(_bsBorrower.CurrentAmountDue, 2) 'wrong
        lblTotalAmountDue.Text = FormatCurrency(_bsBorrower.TotalAmountDue, 2) 'wrong
        lblTotal.Text = FormatCurrency(_bsBorrower.CurrentAmountDue + _bsBorrower.AmountPastDue + _bsBorrower.OutstandingLateFees, 2)
        lblnum20day.Text = _bsBorrower.TwentyDayLetter.NumberSent
        SetCallForward(_bsBorrower.ExistsInLco)
        SetServicers()

        'Payment Information
        lblPrin.Text = FormatCurrency(_bsBorrower.TotalPrincipalDue, 2)
        lblInter.Text = FormatCurrency(_bsBorrower.TotalInterestDue, 2)
        lblDueDt.Text = _bsBorrower.NextDueDateText
        'adjust due date to the next month if the payment day is past
        If IsDate(lblDueDt.Text) AndAlso DateTime.Parse(lblDueDt.Text) < DateTime.Now.Date AndAlso IsNumeric(_bsBorrower.BillingDueDayText) Then
            lblDueDt.Text = DateTime.Parse(lblDueDt.Text).AddMonths(1).ToString("MM/dd/yy")
        End If

        For Each thisInstallment As Installment In _bsBorrower.Installments
            lblDueDay.Text += thisInstallment.FirstDueDate.ToString("dd ")
        Next thisInstallment

        Dim mnthlyPayAmt As Double = 0
        For Each thisInstallment As Installment In _bsBorrower.Installments
            lblMonthlyPmt.Text += thisInstallment.Amount.ToString("$##,##0.00 ")
            mnthlyPayAmt = mnthlyPayAmt + thisInstallment.Amount
        Next thisInstallment
        _bsBorrower.MonthlyPaymentAmount = mnthlyPayAmt.ToString("$#,###,##0.00")

        If _bsBorrower.HasSuspension Then
            lblLastPmt.Text = "S"
        Else
            lblLastPmt.Text = ""
        End If
        lblLastPmt.Text += _bsBorrower.LastPayment.ReceivedDate

        lblLastPmtAmount.Text = FormatCurrency(_bsBorrower.LastPayment.Amount, 2)
        lblPayOffAmount.Text = FormatCurrency(_bsBorrower.TotalPayoffAmount, 2)
        lblDirectDebit.Text = _bsBorrower.ACHData.HasACH
        lblDDDate.Text = _bsBorrower.ACHData.StatusDt
        lblDailyInterest.Text = FormatCurrency(_bsBorrower.TotalDailyInterest, 2)
        lblPastDueAmt.Text = FormatCurrency(_bsBorrower.TotalAmountPastDue, 2)
        lblLateFeesDue.Text = FormatCurrency(_bsBorrower.TotalLateFeesDue, 2)
        txtPayOffDate.Text = DateTime.Now.ToShortDateString()

        'add loans from TS26
        For Each thisLoan As Loan In _bsBorrower.Loans
            Dim twoLetterLoanType As String = GetTwoLetterLoanType(thisLoan.LoanType)
            If lblLoanPg.Text.Contains(twoLetterLoanType) = False Then
                lblLoanPg.Text += " " + twoLetterLoanType
            End If
        Next thisLoan
        'Loan Programs
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If String.IsNullOrEmpty(_bsBorrower.LG10Data(1, x)) = False AndAlso lblLoanPg.Text.Contains(_bsBorrower.LG10Data(1, x)) = False Then
                If _bsBorrower.LG10Data(1, x) <> "GL" OrElse _
                (_bsBorrower.LG10Data(1, x) = "GL" AndAlso lblLoanPg.Text.Contains("SF") = False AndAlso lblLoanPg.Text.Contains("SU") = False) Then
                    lblLoanPg.Text += " " + _bsBorrower.LG10Data(1, x)
                End If
            End If
        Next x

        'Cohort
        lblCohort.Text = ""
        'loans from TS26
        Dim interestingLoanTypes As List(Of String) = New String() {"STFFRD", "UNSTFD", "SLS", "SUBCNS", "UNCNS"}.ToList()
        Dim octoberLastYear As New DateTime(DateTime.Now.Year - 1, 10, 1)
        Dim septemberThisYear As New DateTime(DateTime.Now.Year, 9, 30)
        Dim octoberTwoYearsAgo As DateTime = octoberLastYear.AddYears(-1)
        Dim septemberLastYear As DateTime = septemberThisYear.AddYears(-1)
        For Each thisLoan As Loan In _bsBorrower.Loans
            If interestingLoanTypes.Contains(thisLoan.LoanType) Then
                Dim repaymentStartDate As DateTime = DateTime.Parse(thisLoan.RepaymentStartDate)
                If repaymentStartDate >= octoberLastYear AndAlso repaymentStartDate <= septemberThisYear Then
                    If Not lblCohort.Text.Contains(DateTime.Now.ToString("yyyy")) Then
                        lblCohort.Text += DateTime.Now.ToString(" yyyy")
                    End If
                ElseIf repaymentStartDate >= octoberTwoYearsAgo AndAlso repaymentStartDate <= septemberLastYear Then
                    If Not lblCohort.Text.Contains(DateTime.Now.AddYears(-1).ToString("yyyy")) Then
                        lblCohort.Text += DateTime.Now.AddYears(-1).ToString(" yyyy")
                    End If
                End If
            End If
        Next thisLoan

        'rehab and/or repurchase
        If _bsBorrower.IsRehabilitated Then
            lblRehab.Text = "Rehabilitated"
        End If
        If _bsBorrower.IsRepurchased Then
            lblRepurch.Text = "Repurchase"
        End If

        'Borrower Activity History
        If _bsBorrower.LastContactDateText.Length > 0 Then lblDtLastCntct.Text = _bsBorrower.LastContactDateText
        If _bsBorrower.LastAttemptedContactDateText.Length > 0 Then lblDtAttempt.Text = _bsBorrower.LastAttemptedContactDateText

        Dim cbpIneligible As Boolean = False
        If String.IsNullOrEmpty(_bsBorrower.LoanStatus) OrElse _bsBorrower.LoanStatus = "PIF" Then cbpIneligible = True
        _scriptsAndServicesForm = New SP.frmScriptsAndServices(_bsBorrower.SSN, CType(_noteDudeForm, SP.frmNoteDUDE), lvSS, lblNumDaysDelinquent.Text, lblDueDt.Text, CType(_bsBorrower, SP.Borrower), _bsBorrower.UserProvidedDemos.UPAddrVer, _bsBorrower.UserProvidedDemos.UPPhoneNumVer, _bsBorrower.UserProvidedDemos.UPEmailVer, Me.Text, "Borrower Services", cbpIneligible)

        _thirdPartyForm = New frm3rdParty(_bsBorrower.ThirdPartyListViewItems)

        'call categorization section 
        If _bsBorrower.ActivityCode = "TC" AndAlso (New String() {"02", "04", "70", "94"}.Contains(_bsBorrower.ContactCode)) Then
            gbCC.Visible = True
            pbSurf.Visible = False
        Else
            gbCC.Visible = False
            pbSurf.Visible = True
        End If

        'write out data for scripts
        _bsBorrower.WriteOut()

        SP.Processing.Visible = False

        'center homepage on primary screen
        Me.Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Me.Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 2) - (Me.Height / 2)

        'added so note dude would display properly on one screen.
        If System.Windows.Forms.Screen.AllScreens.Count() = 1 Then
            'center on main screen
            If rbActHistCOMPASS.Checked Then
                _noteDudeForm.Location = System.Windows.Forms.Screen.AllScreens(0).Bounds.Location
            Else
                _oneLinkNoteDudeForm.Location = System.Windows.Forms.Screen.AllScreens(0).Bounds.Location
            End If
        Else
            'display note DUDE on opposite screen
            Dim secondScreenIndex As Integer = 0
            While System.Windows.Forms.Screen.AllScreens(secondScreenIndex).Equals(System.Windows.Forms.Screen.PrimaryScreen)
                secondScreenIndex += 1
            End While
            'center on opposite screen
            If rbActHistCOMPASS.Checked Then
                _noteDudeForm.Location = System.Windows.Forms.Screen.AllScreens(secondScreenIndex).Bounds.Location
            Else
                _oneLinkNoteDudeForm.Location = System.Windows.Forms.Screen.AllScreens(secondScreenIndex).Bounds.Location
            End If
        End If


        'add text to VIP and Special Handling label
        If _bsBorrower.SpecialHandlingIsRequired AndAlso _bsBorrower.IsVIP Then
            lblVIPOrSH.Text = "VIP & Special Handling -- VIP & Special Handling"
        ElseIf _bsBorrower.SpecialHandlingIsRequired Then
            lblVIPOrSH.Text = "Special Handling -- Special Handling"
        ElseIf _bsBorrower.IsVIP Then
            lblVIPOrSH.Text = "VIP -- VIP -- VIP -- VIP"
        Else
            lblVIPOrSH.Text = ""
        End If

        'Alert the user to special situations:
        'start VIP and Special Handling label blinking
        Dim blinkingThread As New Thread(AddressOf BlinkVipText)
        blinkingThread.IsBackground = True
        blinkingThread.Start()
        'Split borrower?
        If lstServicer.Items.Count > 1 Then
            Dim message As String = "The borrower's loans are split among more than one servicer. See the ""Servicer"" list in the ""Borrower Status"" area on the home page."
            MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        'Initialize ActivityAndContactCode to something useful, in case the conditions below don't apply.
        _activityAndContactCode = MDBSContact.frmContact.ContactType.Other
        'See if we need to be more specific about the ActivityAndContactCode.
        If _bsBorrower.ActivityCode = "TC" And (New String() {"03", "TO", "11", "93"}.Contains(_bsBorrower.ContactCode)) Then
            Select Case _bsBorrower.ContactCode
                Case "03"
                    _activityAndContactCode = MDBSContact.frmContact.ContactType.TC03
                Case "TO"
                    If _isAuthorizedThirdParty Then
                        _activityAndContactCode = MDBSContact.frmContact.ContactType.TCTO_Authorized3rdParty
                    Else
                        _activityAndContactCode = MDBSContact.frmContact.ContactType.TCTO_NotAuthorized3rdParty
                    End If
                Case "11"
                    If _isAuthorizedThirdParty Then
                        _activityAndContactCode = MDBSContact.frmContact.ContactType.TC11_Authorized3rdParty
                    Else
                        _activityAndContactCode = MDBSContact.frmContact.ContactType.TC11_NotAuthorized3rdParty
                    End If
                Case "93"
                    If _isAuthorizedThirdParty Then
                        _activityAndContactCode = MDBSContact.frmContact.ContactType.TC93_Authorized3rdParty
                    Else
                        _activityAndContactCode = MDBSContact.frmContact.ContactType.TC93_NotAuthorized3rdParty
                    End If
            End Select
            'Launch the Contact dialog automatically if merited by the borrower.
            If _bsBorrower.NumDaysDelinquent > 0 Then
                Dim contactThread As New Thread(AddressOf StartContact)
                contactThread.IsBackground = True
                _contactForm = New MDBSContact.frmContact(_activityAndContactCode, _scriptsAndServicesForm, _bsBorrower, _bsBorrower.Loans.To2DArray(), _bsBorrower.CurrentAmountDue, _bsBorrower.LastPayment.Amount, _bsBorrower.OutstandingLateFees, _bsBorrower.InterestRateText, _bsBorrower.DisbursementDateText, _bsBorrower.LoanProgram)
                AddHandler _contactForm.Activated, AddressOf EnableHomePage
                contactThread.Start()
            End If
        End If

        Me.ShowDialog()
        Return True
    End Function

    Private Function GetTwoLetterLoanType(ByVal loanType As String) As String
        Select Case loanType
            Case "STFFRD"
                Return "SF"
            Case "UNSTFD"
                Return "SU"
            Case "SLS"
                Return "SL"
            Case "PLUS"
                Return "PL"
            Case "UNSPC"
                Return "SP"
            Case "SUBSPC"
                Return "SP"
            Case "SUBCNS"
                Return "CL"
            Case "UNCNS"
                Return "CL"
            Case "GRAD PLUS"
                Return "GB"
            Case "TILP"
                Return "TI"
            Case Else
                Return ""
        End Select
    End Function

    Private Sub SetServicers()
        '1 = loan type
        '2 = servicer code
        '3 = date processed
        '4 = guarantee amount
        '5 = loan status
        '6 = loan status reason code
        Dim unacceptableLoanStatuses() As String = {"AL", "CA", "CP", "DN", "PC", "PF", "PM", "PN", "RF"}
        Dim chelaServicers() As String = {"813285", "821623", "820164", "821614"}
        Dim nonNelnetServicers() As String = {"700126", "700579", "700191", "813285", "821623", "820164", "821614", "830151", "831053", "899993", "899986", "829587", "833253", "888885", "831474", "830084"}
        Dim sallieMaeServicers() As String = {"700191", "830151", "831053", "899993", "899986", "829587", "833253", "888885", "831474", "830084"}
        For x As Integer = 1 To _bsBorrower.LG10Data.GetUpperBound(1)
            If _bsBorrower.LG10Data(2, x) = "700126" AndAlso (unacceptableLoanStatuses.Contains(_bsBorrower.LG10Data(5, x)) = False OrElse _bsBorrower.HasPrivateLoan) Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("BSV")
                End If
            End If
            If _bsBorrower.LG10Data(2, x) = "700579" AndAlso unacceptableLoanStatuses.Contains(_bsBorrower.LG10Data(5, x)) = False Then
                If lstServicer.FindStringExact("FedLoan Servicing") = -1 Then
                    lstServicer.Items.Add("FedLoan Servicing")
                End If
            End If
            If chelaServicers.Contains(_bsBorrower.LG10Data(2, x)) AndAlso unacceptableLoanStatuses.Contains(_bsBorrower.LG10Data(5, x)) = False Then
                If lstServicer.FindStringExact("CHELA") = -1 Then
                    lstServicer.Items.Add("CHELA")
                End If
            End If
            If nonNelnetServicers.Contains(_bsBorrower.LG10Data(2, x)) = False AndAlso unacceptableLoanStatuses.Contains(_bsBorrower.LG10Data(5, x)) = False Then
                If lstServicer.FindStringExact("Nelnet") = -1 Then
                    lstServicer.Items.Add("Nelnet")
                End If
            End If
            If sallieMaeServicers.Contains(_bsBorrower.LG10Data(2, x)) AndAlso unacceptableLoanStatuses.Contains(_bsBorrower.LG10Data(5, x)) = False Then
                If lstServicer.FindStringExact("Sallie Mae") = -1 Then
                    lstServicer.Items.Add("Sallie Mae")
                End If
            End If
        Next
    End Sub

    Private Sub SetCallForward(ByVal borrowerHasLcoDemographicsOnTPDD As Boolean)
        Dim stp3 As Boolean = False
        Dim stp4 As Boolean = False
        Dim stp5 As Boolean = False
        '1 = loan type
        '2 = servicer code
        '3 = date processed
        '4 = guarantee amount
        '5 = loan status
        '6 = loan status reason code
        Dim defaultStatuses() As String = {"CP", "DN"}
        Dim defaultUnacceptableReasonCodes() As String = {"BC", "BO", "BH"}
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If defaultStatuses.Contains(_bsBorrower.LG10Data(5, x)) AndAlso defaultUnacceptableReasonCodes.Contains(_bsBorrower.LG10Data(6, x)) = False Then
                If lstCallF.FindStringExact("LMS: Default 7272") = -1 Then
                    lstCallF.Items.Add("LMS: Default 7272")
                End If
            End If
        Next x
        'step 3
        Dim auxReasonCodes() As String = {"BC", "BO", "BH"}
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If _bsBorrower.LG10Data(5, x) = "CP" AndAlso auxReasonCodes.Contains(_bsBorrower.LG10Data(6, x)) Then
                If lstCallF.FindStringExact("Auxiliary Services 7274") = -1 Then
                    lstCallF.Items.Add("Auxiliary Services 7274")
                    stp3 = True
                End If
            End If
        Next x
        'step 4
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If _bsBorrower.LG10Data(5, x) = "CR" AndAlso auxReasonCodes.Contains(_bsBorrower.LG10Data(6, x)) = False Then
                If lstCallF.FindStringExact("LMS: Preclaim 7246") = -1 Then
                    lstCallF.Items.Add("LMS: Preclaim 7246")
                    stp4 = True
                End If
            End If
        Next x
        'step 5
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If _bsBorrower.LG10Data(5, x) = "CR" AndAlso auxReasonCodes.Contains(_bsBorrower.LG10Data(6, x)) Then
                If lstCallF.FindStringExact("Auxiliary Services 7274") = -1 Then
                    lstCallF.Items.Add("Auxiliary Services 7274")
                    stp5 = True
                End If
            End If
        Next x
        Dim uninterestingLoanStatuses() As String = {"CA", "PC", "PF", "PM", "PN", "RF"}
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If _bsBorrower.LG10Data(2, x) = "700126" AndAlso uninterestingLoanStatuses.Contains(_bsBorrower.LG10Data(5, x)) = False Then
                If lstCallF.FindStringExact("Borrower Services 7294") = -1 Then
                    lstCallF.Items.Add("Borrower Services 7294")
                End If
            End If
        Next x
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If _bsBorrower.LG10Data(2, x) = "700126" AndAlso _bsBorrower.LG10Data(5, x).Length = 0 Then
                If lstCallF.FindStringExact("Borrower Services 7294") = -1 Then
                    lstCallF.Items.Add("Borrower Services 7294")
                End If
            End If
        Next x
        If borrowerHasLcoDemographicsOnTPDD AndAlso stp3 = False Then
            If lstCallF.FindStringExact("Borrower Services 7294") = -1 Then
                lstCallF.Items.Add("Borrower Services 7294")
            End If
        End If
        Dim defaultServicers() As String = {"700079", "700004", "700789", "700191", "700190", "700121"}
        For x As Integer = _bsBorrower.LG10Data.GetLowerBound(1) To _bsBorrower.LG10Data.GetUpperBound(1)
            If stp3 = False AndAlso defaultServicers.Contains(_bsBorrower.LG10Data(2, x)) AndAlso uninterestingLoanStatuses.Contains(_bsBorrower.LG10Data(5, x)) = False Then
                If lstCallF.FindStringExact("LMS: Default 7272") = -1 Then
                    lstCallF.Items.Add("LMS: Default 7272")
                End If
            End If
        Next x
    End Sub

    Private Function GetStatusRank(ByVal loanStatus As String) As String
        Select Case loanStatus
            Case "Death"
                Return "A1"
            Case "Alleged Death"
                Return "A2"
            Case "Disability"
                Return "A3"
            Case "Alleged Disability"
                Return "A4"
            Case "Bankruptcy"
                Return "A5"
            Case "Alleged Bankruptcy"
                Return "A6"
            Case "CURE"
                Return "A7"
            Case "Pending"
                Return "A9"
            Case "Rejected"
                Return "A10"
            Case "Suspended"
                Return "A11"
            Case "Incomplete"
                Return "A12"
            Case "In Repayment"
                Return "B1"
            Case "In Grace"
                Return "B2"
            Case "In School"
                Return "B3"
            Case "In School/In Grace"
                Return "B4"
            Case "Delinquent"
                Return "B5"
            Case "Deferment"
                Return "B5"
            Case "Forbearance"
                Return "B6"
            Case "Claim Submitted"
                Return "B9"
            Case "PIF/Deconverted"
                Return "C1"
            Case "Default"
                Return "C2"
            Case "Preclaim"
                Return "C3"
            Case "Ineligible"
                Return "C4"
            Case Else
                Return ""
        End Select
    End Function

    Private Function GetStatus(ByVal status As String, ByVal reason As String) As String
        If String.IsNullOrEmpty(status) Then Return ""
        Select Case status.ToUpper()
            Case "DE", "VERIFIED DEATH"
                Return "Death"
            Case "ALLEGED DEATH"
                Return "Alleged Death"
            Case "DI", "VERIFIED DISABILITY"
                Return "Disability"
            Case "ALLEGED DISABILITY"
                Return "Alleged Disability"
            Case "BC", "BH", "BO", "VERIFIED BANKRUPTCY"
                Return "Bankruptcy"
            Case "ALLEGED BANKRUPTCY"
                Return "Alleged Bankruptcy"
            Case "CURE"
                Return "CURE"
            Case "REPAYMENT"
                Return "In Repayment"
            Case "IG", "IN GRACE"
                Return "In Grace"
            Case "IA", "IN SCHOOL"
                Return "In School"
            Case "LD"
                Return "In School/In Grace"
            Case "DA", "DEFERMENT"
                Return "Deferment"
            Case "FB", "FORBEARANCE"
                Return "Forbearance"
            Case "CP"
                Select Case reason
                    Case "DF", "DB", "DQ", "DU"
                        Return "Default"
                    Case "IN"
                        Return "Ineligible"
                    Case Else
                        Return ""
                End Select
            Case "CR"
                If reason = "DF" Then
                    Return "Preclaim"
                Else
                    Return ""
                End If
            Case "PRE-CLAIM SUBMITTED"
                Return "Preclaim"
            Case "CLAIM SUBMITTED"
                Return "Claim Submitted"
            Case Else
                Return ""
        End Select
    End Function

    Private Sub SetDemographicsWidgets()
        lblAddr1.Text = ""
        lblAddr2.Text = ""
        lblAddr3.Text = ""
        lblHome.Text = ""
        lblWork.Text = ""
        lblAlter.Text = ""
        lblEmail.Text = ""
        lblName.Text = ""

        'This populates the Borrowers Demo with the instance of the Demographics screens text boxes
        lblName.Text = _bsBorrower.Name
        lblAddr1.Text = _bsBorrower.UserProvidedDemos.Addr1
        lblAddr2.Text = _bsBorrower.UserProvidedDemos.Addr2
        lblAddr3.Text = _bsBorrower.UserProvidedDemos.City + " " + _bsBorrower.UserProvidedDemos.State + " " + _bsBorrower.UserProvidedDemos.Zip
        lblHome.Text = _bsBorrower.UserProvidedDemos.HomePhoneNum
        lblWork.Text = _bsBorrower.UserProvidedDemos.OtherPhoneNum
        lblAlter.Text = _bsBorrower.UserProvidedDemos.OtherPhone2Num
        lblEmail.Text = _bsBorrower.UserProvidedDemos.Email
        'if address is invalid turn color red
        If _bsBorrower.UserProvidedDemos.UPAddrVal Then
            NormalAddress()
            pbDown.Visible = False
            pbUp.Visible = True
            lblAddrVal.Text = "Y"
        Else
            RedAddress()
            pbUp.Visible = False
            pbDown.Visible = True
            lblAddrVal.Text = "N"
        End If

        If _bsBorrower.UserProvidedDemos.UPPhoneVal Then
            lblHPhoneVal.Text = "Y"
        Else
            lblHPhoneVal.Text = "N"
        End If

        If _bsBorrower.UserProvidedDemos.UPOtherVal Then
            lblOPhoneVal.Text = "Y"
        Else
            lblOPhoneVal.Text = "N"
        End If

        If _bsBorrower.UserProvidedDemos.UPOther2Val Then
            lblO2PhoneVal.Text = "Y"
        Else
            lblO2PhoneVal.Text = "N"
        End If

        If _bsBorrower.UserProvidedDemos.UPEmailVal Then
            lblEmailVal.Text = "Y"
        Else
            lblEmailVal.Text = "N"
        End If

        If _bsBorrower.DemographicsVerified = False Then
            btnUpdateDemo.BackColor = Me.ForeColor
            btnUpdateDemo.ForeColor = Me.BackColor
            lblVerified.Text = "Not Verified"
        Else
            btnUpdateDemo.BackColor = Me.BackColor
            btnUpdateDemo.ForeColor = Me.ForeColor
            lblVerified.Text = "Verified"
        End If
    End Sub

    Private Sub btnLegalAddHist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLegalAddHist.Click
        UnderConstruction()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RefreshScriptsAndServices()
        Dim unrunnableScriptsAndServices As String = _scriptsAndServicesForm.RunRemainingScriptsAndServices()
        Me.Activate()
        If unrunnableScriptsAndServices.Length > 0 Then
            Dim message As String = String.Format("Here is a list of scripts that could not be run.{0}{1}", Environment.NewLine, unrunnableScriptsAndServices)
            SP.frmWhoaDUDE.WhoaDUDE(message, "These Scripts are Shark Bait")
        End If
        CloseAllSubForms()
        If _contactForm IsNot Nothing Then _contactForm.Hide()
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'make sure that call categorization info was provided if needed
        If gbCC.Visible Then
            'if Call Categorization is visible then it is required
            'make sure that category is provided
            If cbCCCat.SelectedIndex = -1 Then
                SP.frmWhoaDUDE.WhoaDUDE("Gotta provide a call category!", "Call Category")
                Return
            End If
            'check if reason is given if reasons are avail
            If cbCCRea.Items.Count <> 0 And cbCCRea.SelectedIndex = -1 Then
                SP.frmWhoaDUDE.WhoaDUDE("Gotta provide a call categorization reason!", "Call Categorization Reason")
                Return
            End If
            'make sure that misc info is provided if certain options are selected
            If cbCCCat.SelectedItem = "Received Letter" And tbCCLtrID.Text = "" Then
                SP.frmWhoaDUDE.WhoaDUDE("Gotta provide a letter id if the call is about a letter the borrower received!", "Letter ID")
                Return
            ElseIf cbCCCat.SelectedItem = "Other" And tbCCCmts.Text = "" Then
                SP.frmWhoaDUDE.WhoaDUDE("Gotta provide some comments if the call category is ""Other""!", "Additional Comments Needed")
                Return
            End If
            'update DB
            DataAccess.InsertCallCategorization(SP.TestMode(), cbCCCat.SelectedItem, cbCCRea.SelectedItem, tbCCLtrID.Text, tbCCCmts.Text)
        End If
        If _bsBorrower.DemographicsVerified = False Then
            SP.frmWhoaDUDE.WhoaDUDE("You gota check the demographics in order to go on Dude!", "Verify Demographics")
            Return
        End If

        'Create DMP01 arc if borrower is more than 10 days deliquent
        If (_bsBorrower.ContactCode = "04" OrElse _bsBorrower.ContactCode = "70") AndAlso (_bsBorrower.ActivityCode = "TC" OrElse _bsBorrower.ActivityCode = "OV") Then
            If _bsBorrower.DateDelinquencyOccurred <> "" AndAlso (_bsBorrower.DateDelinquencyOccurred < CDate(Date.FromOADate(Date.Now.Date.ToOADate() - 9))) Then
                Dim comments As New SP.ActivityComments(_bsBorrower.SSN)
                comments.AddCommentsToTD22AllLoans("", "DMP01")
            End If
        End If

        SP.Processing.Visible = True
        If _bsBorrower.BorLite.NoACPBSVCall = False Then
            _scriptsAndServicesForm.ReturnToACP(_bsBorrower.BorLite.Queue, _bsBorrower.BorLite.SubQueue, _bsBorrower.BorLite.ACPSelection, _contactPhone.ContactPhoneNumber)
            _bsBorrower.Notes = _noteDudeForm.tbNoteText.Text
            _bsBorrower.NoteDudeNotesForOneLink = _oneLinkNoteDudeForm.tbNoteText.Text
            _bsBorrower.SpillGuts()
            _bsBorrower.EnterCommentsIntoSystem(True) 'COMPASS COMMENTS
            _bsBorrower.EnterCommentsIntoSystem(True, "DUDEC") 'OneLINK COMMENTS
            DemographicsForm.UpdateSys()
            Dim unrunnableScriptsAndServices As String = _scriptsAndServicesForm.RunRemainingScriptsAndServices()
            Me.Activate()
            If unrunnableScriptsAndServices.Length > 0 Then
                Dim message As String = String.Format("Here is a list of scripts that could not be run.{0}{1}", Environment.NewLine, unrunnableScriptsAndServices)
                SP.frmWhoaDUDE.WhoaDUDE(message, "These Scripts are Shark Bait")
            End If
        Else
            _bsBorrower.EnterCommentsIntoSystem(True, "DUDEC") 'OneLINK COMMENTS
            DemographicsForm.UpdateSys()
            Me.Activate()
        End If
        CloseAllSubForms()
        If _contactForm IsNot Nothing Then _contactForm.Hide()

        'return to favorite screen
        Dim favoriteScreen As String = DataAccess.GetFavoriteScreen(SP.TestMode())
        SP.Q.FastPath(favoriteScreen)

        Me.Hide()
        SP.Processing.Visible = False
    End Sub

    Private Sub btnUpdateDemo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateDemo.Click
        CloseAllSubForms()
        Me.Visible = False
        DemographicsForm.ShowDialog(True, True)
        SetDemographicsWidgets()
        _bsBorrower.WriteOut()
        Me.Visible = True
    End Sub

    Private Sub btnstatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnstatus.Click
        Try
            AppActivate("Mo Status")
        Catch
            Dim nonEmptyLoanStatuses As List(Of String) = _loanStatuses.Where(Function(p) p.Length > 0).ToList()
            _moStatusForm = New frmMoreStatus()
            _moStatusForm.Show(nonEmptyLoanStatuses, Me.BackColor, Me.ForeColor)
        End Try
    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        If _scriptsAndServicesForm.Visible Then
            _scriptsAndServicesForm.Activate()
        Else
            _scriptsAndServicesForm.Show()
        End If
    End Sub

    Private Sub lvSS_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvSS.DoubleClick
        If lvSS.SelectedItems.Item(0).SubItems.Item(1).Text = "Queued to Run" OrElse lvSS.SelectedItems.Item(0).SubItems.Item(1).Text = "Documented in Note DUDE" Then
            _scriptsAndServicesForm.UpdateScriptListData()
        Else
            SP.frmWhoaDUDE.WhoaDUDE("No way Dude. This Script is already complete or partialy complete, and my surf board doesnt have a Flex Capacitor!", "Scripts and Services")
        End If
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        RefreshScriptsAndServices()
    End Sub

    Private Sub RefreshScriptsAndServices()
        CloseAllSubForms()
        If lvSS.Items.Count > 0 Then
            For x As Integer = lvSS.Items.Count To 1 Step -1
                If lvSS.Items(x - 1).SubItems(1).Text = "Queued to Run" OrElse lvSS.Items(x - 1).SubItems(1).Text = "Documented in Note DUDE" Then
                    lvSS.Items.Item(x - 1).Selected = True
                    _scriptsAndServicesForm.UpdateScriptListData()
                End If
            Next
        End If
    End Sub

    Private Sub UnderConstruction()
        SP.frmUnderConstruct.ShowUnderConstruct()
    End Sub

    Private Sub btnOneLinkLoans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOneLinkLoans.Click
        Try
            AppActivate("Maui DUDE OneLINK Loans")
        Catch
            _scriptsAndServicesForm.ReturnToACP(_bsBorrower.BorLite.Queue, _bsBorrower.BorLite.SubQueue, _bsBorrower.BorLite.ACPSelection)
            _oneLinkLoansForm = New frmOneLinkLoans(_bsBorrower.LG10Data)
            _oneLinkLoansForm.Show()
        End Try
    End Sub

    Private Sub btnAddComments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddComments.Click
        If rbActHistCOMPASS.Checked Then
            If _noteDudeForm.WindowState = FormWindowState.Minimized Then
                _noteDudeForm.WindowState = FormWindowState.Maximized
            ElseIf _noteDudeForm.Visible Then
                _noteDudeForm.Activate()
            Else
                'NoteDude goes to Compass
                _noteDudeForm.Show(True, False)
            End If
        Else
            If _oneLinkNoteDudeForm.WindowState = FormWindowState.Minimized Then
                _oneLinkNoteDudeForm.WindowState = FormWindowState.Maximized
            ElseIf _oneLinkNoteDudeForm.Visible Then
                _oneLinkNoteDudeForm.Activate()
            Else
                'NoteDude goes to onelink
                _oneLinkNoteDudeForm.Show(True, False)
            End If
        End If
    End Sub

    Private Sub btnLateFeesWaived_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLateFeesWaived.Click
        UnderConstruction()
    End Sub

    Private Sub btnTimesDelinquent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTimesDelinquent.Click
        UnderConstruction()
    End Sub

    Private Sub RunActivityHistory(ByVal days As Integer)
        Try
            Dim historyScreen As String = "LP50"
            If rbActHistCOMPASS.Checked Then
                historyScreen = "TD2A"
            End If
            AppActivate(String.Format("Maui DUDE Borrower {0} Day Activity History - {1}", days.ToString(), historyScreen))
        Catch
            SP.Processing.Refresh()
            If rbActHistCOMPASS.Checked Then _scriptsAndServicesForm.ReturnToACP(_bsBorrower.BorLite.Queue, _bsBorrower.BorLite.SubQueue, _bsBorrower.BorLite.ACPSelection)
            If rbActHistCOMPASS.Checked Then
                If days = 30 Then
                    _thirtyDayActivityHistoryForm = New frmActivityHistory(_bsBorrower)
                    _thirtyDayActivityHistoryForm.Show(days, "Maui DUDE Borrower 30 Day Activity History - TD2A", frmActivityHistory.FromScreen.TD2A)
                End If
                If days = 90 Then
                    _ninetyDayActivityHistoryForm = New frmActivityHistory(_bsBorrower)
                    _ninetyDayActivityHistoryForm.Show(days, "Maui DUDE Borrower 90 Day Activity History - TD2A", frmActivityHistory.FromScreen.TD2A)
                End If
                If days = 180 Then
                    _oneEightyDayActivityHistoryForm = New frmActivityHistory(_bsBorrower)
                    _oneEightyDayActivityHistoryForm.Show(days, "Maui DUDE Borrower 180 Day Activity History - TD2A", frmActivityHistory.FromScreen.TD2A)
                End If
                If days = 360 Then
                    _threeSixtyDayActivityHistoryForm = New frmActivityHistory(_bsBorrower)
                    _threeSixtyDayActivityHistoryForm.Show(days, "Maui DUDE Borrower 360 Day Activity History - TD2A", frmActivityHistory.FromScreen.TD2A)
                End If
            Else
                If days = 30 Then
                    _oneLink30DayActivityHistoryForm = New frmActivityHistory(_bsBorrower)
                    _oneLink30DayActivityHistoryForm.Show(days, "Maui DUDE Borrower 30 Day Activity History - LP50", frmActivityHistory.FromScreen.LP50)
                End If
                If days = 90 Then
                    _oneLink90DayActivityHistoryForm = New frmActivityHistory(_bsBorrower)
                    _oneLink90DayActivityHistoryForm.Show(days, "Maui DUDE Borrower 90 Day Activity History - LP50", frmActivityHistory.FromScreen.LP50)
                End If
                If days = 180 Then
                    _oneLink180DayActivityHistoryForm = New frmActivityHistory(_bsBorrower)
                    _oneLink180DayActivityHistoryForm.Show(days, "Maui DUDE Borrower 180 Day Activity History - LP50", frmActivityHistory.FromScreen.LP50)
                End If
                If days = 360 Then
                    _oneLink360DayActivityHistoryForm = New frmActivityHistory(_bsBorrower)
                    _oneLink360DayActivityHistoryForm.Show(days, "Maui DUDE Borrower 360 Day Activity History - LP50", frmActivityHistory.FromScreen.LP50)
                End If
            End If
        End Try
    End Sub

    Private Sub btn30D_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn30D.Click
        RunActivityHistory(30)
    End Sub

    Private Sub btn90D_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn90D.Click
        RunActivityHistory(90)
    End Sub

    Private Sub btn180d_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn180d.Click
        RunActivityHistory(180)
    End Sub

    Private Sub btn360d_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn360d.Click
        RunActivityHistory(360)
    End Sub

    Private Sub btnAskDUDE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAskDUDE.Click
        Try
            AppActivate("Maui DUDE - Ask DUDE")
        Catch
            SP.DisplayAskDude()
        End Try
    End Sub

    Private Sub btnDUDE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDUDE.Click
        'activate the Reflection session window.
        Dim commandLineSwitches() As String = SP.Q.RIBM.CommandLineSwitches.ToString().Replace("""", "").Split("\")
        ActivateExistingWindow(commandLineSwitches.Last())
        SP.Q.RIBM.SwitchToWindow(1)
        ActivateExistingWindow(commandLineSwitches.Last())
        SP.Q.RIBM.SwitchToWindow(1)
    End Sub

    Private Sub btn411_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn411.Click
        SP.BorrInfo.Show(_bsBorrower.SSN, _bsBorrower.Name)
    End Sub

    Private Sub btnCompassLoans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompassLoans.Click
        If _compassLoansForm.WindowState = FormWindowState.Minimized Then
            _compassLoansForm.WindowState = FormWindowState.Maximized
        ElseIf _compassLoansForm.Visible Then
            _compassLoansForm.BringToFront()
        Else
            _scriptsAndServicesForm.ReturnToACP(_bsBorrower.BorLite.Queue, _bsBorrower.BorLite.SubQueue, _bsBorrower.BorLite.ACPSelection)
            'use due day if populated
            If _bsBorrower.BillingDueDayText.Length = 0 Then
                _compassLoansForm.Show(lblDueDt.Text)
            Else
                _compassLoansForm.Show(_bsBorrower.BillingDueDayText)
            End If
        End If
    End Sub

    Private Sub btnDefForbHist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefForbHist.Click
        If _defermentAndForbearanceHistoryForm.WindowState = FormWindowState.Minimized Then
            _defermentAndForbearanceHistoryForm.WindowState = FormWindowState.Maximized
        ElseIf _defermentAndForbearanceHistoryForm.Visible Then
            _defermentAndForbearanceHistoryForm.BringToFront()
        Else
            _scriptsAndServicesForm.ReturnToACP(_bsBorrower.BorLite.Queue, _bsBorrower.BorLite.SubQueue, _bsBorrower.BorLite.ACPSelection)
            _defermentAndForbearanceHistoryForm.Show()
            If _defermentAndForbearanceHistoryForm.NoResults Then Me.Activate()
        End If
    End Sub

    Private Sub btnPayHist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPayHist.Click
        If _paymentHistoryForm.WindowState = FormWindowState.Minimized Then
            _paymentHistoryForm.WindowState = FormWindowState.Maximized
        ElseIf _paymentHistoryForm.Visible Then
            _paymentHistoryForm.BringToFront()
        Else
            _scriptsAndServicesForm.ReturnToACP(_bsBorrower.BorLite.Queue, _bsBorrower.BorLite.SubQueue, _bsBorrower.BorLite.ACPSelection)
            _paymentHistoryForm.Show(_bsBorrower.OnTime48Monthly)
        End If
    End Sub

    Sub CloseAllSubForms()
        If SP.Q.BorrInfo IsNot Nothing Then SP.Q.BorrInfo.Hide()
        If DemographicsForm IsNot Nothing Then DemographicsForm.Hide()
        If _noteDudeForm IsNot Nothing Then _noteDudeForm.Hide()
        If _oneLinkNoteDudeForm IsNot Nothing Then _oneLinkNoteDudeForm.Hide()
        If _scriptsAndServicesForm IsNot Nothing Then _scriptsAndServicesForm.Hide()
        If _compassLoansForm IsNot Nothing Then _compassLoansForm.Hide()
        If _defermentAndForbearanceHistoryForm IsNot Nothing Then _defermentAndForbearanceHistoryForm.Hide()
        If _paymentHistoryForm IsNot Nothing Then _paymentHistoryForm.Hide()
        If _moStatusForm IsNot Nothing Then _moStatusForm.Hide()
        If _thirtyDayActivityHistoryForm IsNot Nothing Then _thirtyDayActivityHistoryForm.Hide()
        If _ninetyDayActivityHistoryForm IsNot Nothing Then _ninetyDayActivityHistoryForm.Hide()
        If _oneEightyDayActivityHistoryForm IsNot Nothing Then _oneEightyDayActivityHistoryForm.Hide()
        If _threeSixtyDayActivityHistoryForm IsNot Nothing Then _threeSixtyDayActivityHistoryForm.Hide()
        If _oneLinkLoansForm IsNot Nothing Then _oneLinkLoansForm.Hide()
        If SP.Q.AskDUDE IsNot Nothing Then SP.Q.AskDUDE.Hide()
        If _borrowerBenefitsForm IsNot Nothing Then _borrowerBenefitsForm.Hide()
        If _directDebitForm IsNot Nothing Then _directDebitForm.Hide()
        If _privateLoansForm IsNot Nothing Then _privateLoansForm.Hide()
    End Sub

    Public Sub SetThirdParty(ByVal isThirdParty As Boolean)
        _isAuthorizedThirdParty = isThirdParty
        'Show or hide controls depending on whether 3rd party is found.
        lblContactType.Visible = Not isThirdParty
        gbBorrowerStatus.Visible = isThirdParty
        gbPaymentInformation.Visible = (isThirdParty AndAlso _bsBorrower.BorLite.NoACPBSVCall = False)
        gbAccountHistory.Visible = (isThirdParty AndAlso _bsBorrower.BorLite.NoACPBSVCall = False)
        If isThirdParty Then
            txtBSHome.Location = New Point(304, 0)
            lblVIPOrSH.Location = New Point(304, 24)
            btnAskDUDE.Location = New Point(704, 8)
            btnDUDE.Location = New Point(792, 8)
            btn411.Location = New Point(880, 8)
            btnSave.Location = New Point(184, 648)
            btnBack.Location = New Point(368, 648)
            btnRefresh.Location = New Point(552, 648)
            gbCC.Location = New Point(632, 400)
            pbSurf.Location = New Point(696, 408)
            Me.MaximumSize = New Size(992, 725)
            Me.Size = New Size(992, 725)
            Me.MinimumSize = New Size(992, 725)
            btnBrightIdea.Location = New Point(16, 3)
            btnUnexpected.Location = New Point(72, 3)
        Else
            gbBorrowerStatus.Visible = False
            txtBSHome.Location = New Point(16, 0)
            lblVIPOrSH.Location = New Point(16, 24)
            btnAskDUDE.Location = New Point(328, 8)
            btnDUDE.Location = New Point(416, 8)
            btn411.Location = New Point(504, 8)
            btnSave.Location = New Point(24, 648)
            btnBack.Location = New Point(208, 648)
            btnRefresh.Location = New Point(392, 648)
            pbSurf.Location = New Point(85, 420)
            gbCC.Location = New Point(8, 400)
            Me.MinimumSize = New Size(600, 725)
            Me.Size = New Size(600, 725)
            Me.MaximumSize = New Size(600, 725)
            btnBrightIdea.Location = New Point(24, 575)
            btnUnexpected.Location = New Point(80, 575)
        End If
    End Sub

    Public Sub SetDisplayForNoAcpBsvCall(ByVal b As Boolean)
        'if b = true then set fields to invisible else visible
        gbPaymentInformation.Visible = Not b
        rbActHistCOMPASS.Checked = Not b
        If b Then rbActHistCOMPASS.Enabled = False
        rbActHistOneLINK.Checked = b
        gbAccountHistory.Visible = Not b
        'loan history buttons
        lblLoanHistory.Visible = Not b
        btnCompassLoans.Visible = Not b
        btnDefForbHist.Visible = Not b
        btnOneLinkLoans.Visible = Not b
        'scripts and services
        btnSelect.Visible = Not b
        btnSendConsolApp.Visible = b
    End Sub

    Sub RedAddress()
        'Turns address red if it is invalid
        lblAddr1.ForeColor = Color.Red
        lblAddr2.ForeColor = Color.Red
        lblAddr3.ForeColor = Color.Red
        lblAddrVal.ForeColor = Color.Red
    End Sub

    Sub NormalAddress()
        'Turns address back to normal if it is valid
        lblAddr1.ForeColor = Me.ForeColor
        lblAddr2.ForeColor = Me.ForeColor
        lblAddr3.ForeColor = Me.ForeColor
        lblAddrVal.ForeColor = Me.ForeColor
    End Sub

    Private Sub lblDirectDebit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDirectDebit.Click
        btnDirectDebit.PerformClick()
    End Sub

    Private Sub btnDirectDebit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDirectDebit.Click
        If _directDebitForm.WindowState = FormWindowState.Minimized Then
            _directDebitForm.WindowState = FormWindowState.Maximized
        ElseIf _directDebitForm.Visible Then
            _directDebitForm.BringToFront()
        Else
            _directDebitForm.Show(_bsBorrower)
        End If
    End Sub

    Private Sub lblDDDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDDDate.Click
        btnDirectDebit.PerformClick()
    End Sub

    Private Sub btnSendConsolApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendConsolApp.Click
        'check if the script has already been selected 
        If lvSS.Items.Count > 0 Then
            SP.frmWhoaDUDE.WhoaDUDE("Whoa Dude, that script has already been selected.  You cannot select it again.", "Already Selected", True)
            Return
        End If
        Dim commandLineSwitches() As String = SP.Q.RIBM.CommandLineSwitches.ToString().Replace("""", "").Split("\")
        ActivateExistingWindow(commandLineSwitches.Last())
        SP.Q.RIBM.SwitchToWindow(1)
        Dim newItemIndex As Integer = lvSS.Items.Add("Send Consolidation Application").Index()
        lvSS.Items(newItemIndex).SubItems.Add("Complete")
        lvSS.Items(newItemIndex).SubItems.Add("22")
        SP.Q.RIBM.RunMacro("sp.LCOSendApp.Main", _bsBorrower.SSN + ",1")
    End Sub

    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long

    Private Sub ActivateExistingWindow(ByVal targetWindowTitle As String)
        Dim existingWindowHandle As Long = 0
        For Each runningProcess As Process In Process.GetProcesses()
            If String.Compare(runningProcess.MainWindowTitle, targetWindowTitle, True) = 0 Then
                existingWindowHandle = runningProcess.MainWindowHandle.ToInt32()
                Exit For
            End If
        Next runningProcess
        If existingWindowHandle = 0 Then Return
        OpenIcon(existingWindowHandle) 'Restore the program.
        SetForegroundWindow(existingWindowHandle) 'Activate the application.
    End Sub

    Private Sub btnBorrowerBenefits_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrowerBenefits.Click
        Try
            AppActivate("Maui DUDE Borrower Benefits")
        Catch
            SP.Processing.Visible = True
            SP.Processing.Refresh()
            _scriptsAndServicesForm.ReturnToACP(_bsBorrower.BorLite.Queue, _bsBorrower.BorLite.SubQueue, _bsBorrower.BorLite.ACPSelection)
            _borrowerBenefitsForm = New frmBorrowerBenefits(_bsBorrower)
            SP.Processing.Visible = False
            _borrowerBenefitsForm.Show()
        End Try
    End Sub

    Private Sub btnBrightIdea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrightIdea.Click
        If SP.frmEmailComments.BrightIdea() Then
            AppActivate(Me.Text)
            Return
        End If
        AppActivate(Me.Text)
    End Sub

    Private Sub btnUnexpected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnexpected.Click
        If SP.frmEmailComments.UnexpectedResults() Then
            AppActivate(Me.Text)
            Return
        End If
        AppActivate(Me.Text)
    End Sub

    Private Sub txtPayOffDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPayOffDate.KeyPress
        If e.KeyChar = Chr(13) Then 'Enter key
            If IsDate(txtPayOffDate.Text) Then
                SP.Processing.Visible = True
                SP.Processing.Refresh()
                Dim payoffDate As DateTime = CDate(txtPayOffDate.Text)
                _bsBorrower.SetPayoffAmountForGivenPayoffDate(payoffDate)
                lblPayOffAmount.Text = FormatCurrency(_bsBorrower.TotalPayoffAmount, 2)
                txtPayOffDate.Text = payoffDate.ToShortDateString()
                SP.Processing.Visible = False
            Else
                txtPayOffDate.Text = DateTime.Now.ToShortDateString()
                txtPayOffDate.Focus()
            End If
        Else
            lblPayOffAmount.Text = ""
        End If
    End Sub

    Private Sub btnThrdParty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThrdParty.Click
        If _thirdPartyForm.WindowState = FormWindowState.Minimized Then
            _thirdPartyForm.WindowState = FormWindowState.Maximized
        ElseIf _compassLoansForm.Visible Then
            _thirdPartyForm.BringToFront()
        Else
            _thirdPartyForm.Show()
        End If
    End Sub

    Private Sub cbCCCat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCCCat.SelectedIndexChanged
        'wipe out current population
        cbCCRea.Items.Clear()
        'populate call category reason drop down box 
        cbCCRea.Items.AddRange(DataAccess.GetCallCategoryReasons(SP.TestMode(), cbCCCat.SelectedItem.ToString()).ToArray())
        cbCCRea.Enabled = (cbCCRea.Items.Count <> 0)
        tbCCLtrID.Enabled = True
        tbCCCmts.Enabled = True
    End Sub

    Private Sub rbActHistCOMPASS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbActHistCOMPASS.CheckedChanged
        ActHistDateLastContact.Visible = rbActHistCOMPASS.Checked
        ActHistAttempt.Visible = rbActHistCOMPASS.Checked
        lblDtLastCntct.Visible = rbActHistCOMPASS.Checked
        lblDtAttempt.Visible = rbActHistCOMPASS.Checked
        If rbActHistCOMPASS.Checked Then
            ActHistSummary.Text = "TD2A Summary"
        Else
            ActHistSummary.Text = "LP50 Summary"
        End If
    End Sub

    Private Sub BlinkVipText()
        If lblVIPOrSH.Text.Length = 0 Then Return
        Do
            Thread.Sleep(700)
            If lblVIPOrSH.BackColor.Equals(Me.ForeColor) Then
                lblVIPOrSH.BackColor = Me.BackColor
                lblVIPOrSH.ForeColor = Me.ForeColor
            ElseIf lblVIPOrSH.BackColor.Equals(Me.BackColor) Then
                lblVIPOrSH.BackColor = Me.ForeColor
                lblVIPOrSH.ForeColor = Me.BackColor
            End If
        Loop
    End Sub

    Private Sub btnContact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContact.Click
        If _bsBorrower.NumDaysDelinquent = 0 Then
            Dim message As String = "The borrower doesn't appear to be delinquent.  Are you sure you want to run the Contact Dialogue script?"
            Dim caption As String = "Contact Dialogue Script"
            If MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If
        End If
        Dim contactThread As New Thread(AddressOf StartContact)
        contactThread.IsBackground = True
        If (_contactForm Is Nothing) Then
            _contactForm = New MDBSContact.frmContact(_activityAndContactCode, _scriptsAndServicesForm, _bsBorrower, _bsBorrower.Loans.To2DArray(), _bsBorrower.CurrentAmountDue, _bsBorrower.LastPayment.Amount, _bsBorrower.OutstandingLateFees, _bsBorrower.InterestRateText, _bsBorrower.DisbursementDateText, _bsBorrower.LoanProgram)
            AddHandler _contactForm.Activated, AddressOf EnableHomePage
        End If
        contactThread.Start()
    End Sub

    Private Sub EnableHomePage()
        Me.Enabled = True
    End Sub

    Private Sub StartContact()
        Do While Me.Visible = False
            Thread.Sleep(New TimeSpan(0, 0, 0, 1))
        Loop
        Me.Enabled = False
        'There's already a contact form.
        If _contactForm.Visible Then
            'It may be behind other stuff.
            _contactForm.BringToFront()    'This isn't working at the moment. I'm letting it go because it's just a convenience feature.
        Else
            'The Hide() method was called on it.
            _contactForm.Show()
        End If
    End Sub

    Private Sub frmBSHomePage_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If rbActHistCOMPASS.Checked Then
            _noteDudeForm.Show(True, True)
        Else
            _oneLinkNoteDudeForm.Show(True, True)
        End If
    End Sub

    Private Sub btnEmpInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmpInfo.Click
        _employerInfoForm = New frmEmployer()
        _employerInfoForm.Show()
    End Sub

    Private Sub btnPrivateLoans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrivateLoans.Click
        _privateLoansForm.Show(_bsBorrower.PrivateLoansDataTable)
    End Sub

End Class
