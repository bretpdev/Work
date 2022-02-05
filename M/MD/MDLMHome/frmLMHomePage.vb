Imports System.Windows.Forms
Imports System.Threading
Imports System.IO

Public Class frmLMHomePage
    Inherits SP.frmGenericScriptAndServicesEnabled

    'this function does object and screen creation and coordination for the Loan Management (Account Resolution) bin and home page process
    Public Shared Sub LMBinHomePageProcCoord(ByVal BU As String)
        Dim BinsFrm As frmLMBins
        Dim HomePage As Form 'can be the account resolution home page or the customer services home page
        Dim BinBeingWorked As SP.ABin
        Dim BorLt As SP.BorrowerLite
        Dim BinPageSelectedOption As frmLMBins.WhatIAmGoingToGiveYou
        Dim PreviousContacts As New ArrayList 'for tracking the previous 10 contacts
        Dim DisplayDemos As Boolean
        Dim ReturnResultFromHomePage As frmLMHomePage.HomePageReturnResult = HomePageReturnResult.SaveAndContinue 'needs to default to anything other than IncomingCall
        SP.Q.GatherLoginInfo()
        While True
            If ReturnResultFromHomePage <> HomePageReturnResult.IncomingCall Then
                'always start out with the bins screen if it isn't incoming call from homepage
                BinsFrm = New frmLMBins(BU, PreviousContacts)
                BinPageSelectedOption = BinsFrm.Showdialog()
                If BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSN Or BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSN4OverflowACPCall Or BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSN4OverflowNotACPCall Or BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.AccountMaintenance Then
                    BinBeingWorked = Nothing 'there isn't a bin being worked
                    'SSN provided by user on bins screen
                    BorLt = New SP.BorrowerLite
                    'get SSN
                    BorLt.SSN = BinsFrm.GetSSN()
                    'overflow for Customer Service
                    If BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSN4OverflowACPCall Then
                        BorLt.NoACPBSVCall = False
                        BorLt.CheckStartingFromACP()
                    ElseIf BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSN4OverflowNotACPCall Then
                        BorLt.NoACPBSVCall = True
                        BorLt.CheckStartingFromACP()
                    ElseIf BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.AccountMaintenance Then
                        'nothing is really needed if account maintenance is selected
                    End If
                    'do translation from account number to SSN
                    If BorLt.ConvertAccToSSN(BorLt.SSN) = False Then
                        'if SSN isn't valid then the borrower lite var is changed to nothing and flows through the rest of the loop and displays the bins page again immediately
                        BorLt = Nothing
                    End If
                Else
                    'get selected bin from Bins form
                    BinBeingWorked = BinsFrm.GetSelectedBin()
                    'Get task from queue with tasks in selected bin
                    BorLt = SP.BorrowerLite.GetOLTaskForBin(BinBeingWorked.GetQueuesToBeWorked())
                End If
            End If
            'check if borrower came through populated
            While Not (BorLt Is Nothing)
                'note SSN in array list for drop down box
                PreviousContacts.Insert(0, BorLt.SSN)
                If PreviousContacts.Count > 10 Then
                    PreviousContacts.RemoveAt(10) 'only keep 10 previous contacts (delete after that)
                End If
                'process borrower lite if populated else skip it and display bin screen again
                If BinPageSelectedOption <> frmLMBins.WhatIAmGoingToGiveYou.SSN4OverflowACPCall And BinPageSelectedOption <> frmLMBins.WhatIAmGoingToGiveYou.SSN4OverflowNotACPCall Then
                    'Account Resolution Home Page
                    HomePage = New frmLMHomePage(BorLt, PreviousContacts, BinPageSelectedOption)
                    'figure out if demos should be displayed on the way into the home page or not
                    DisplayDemos = (BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSN)
                    If CType(HomePage, frmLMHomePage).GatherInformation(DisplayDemos) = False Then
                        'if back to main menu was clicked on demos then return back to bins page
                        ReturnResultFromHomePage = HomePageReturnResult.BackToBins
                        ClearOutDUDEFiles()
                        Exit While
                    End If
                    'populate home page
                    If BinBeingWorked Is Nothing Then
                        CType(HomePage, frmLMHomePage).PopulateHomePage(0, "")
                    Else
                        CType(HomePage, frmLMHomePage).PopulateHomePage(BinBeingWorked.BinTaskCount, BinBeingWorked.BinName)
                    End If
                    'show home page
                    ReturnResultFromHomePage = CType(HomePage, frmLMHomePage).ShowDialog()
                    If ReturnResultFromHomePage = frmLMHomePage.HomePageReturnResult.BackToBins Then
                        ClearOutDUDEFiles()
                        Exit While 'if the user clicks back to bins button on home page
                    ElseIf ReturnResultFromHomePage = frmLMHomePage.HomePageReturnResult.IncomingCall Then
                        ClearOutDUDEFiles()
                        BorLt = New SP.BorrowerLite
                        'get SSN
                        BorLt.SSN = CType(HomePage, frmLMHomePage).tbIncomingCallSSN.Text
                        'set misc values
                        BorLt.NoACPBSVCall = True
                        BorLt.CheckStartingFromACP()
                        BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSN
                        Exit While 'after misc values are set this will return back to home page for the borrower entered on the homepage previously.  It will skip the bins page. 
                    ElseIf ReturnResultFromHomePage = HomePageReturnResult.SaveAndContinue Then
                        ClearOutDUDEFiles()
                    End If
                Else
                    'Borrower Services Home Page
                    HomePage = New MDBSHome.frmBSHomePage(BorLt)
                    'show home page
                    If CType(HomePage, MDBSHome.frmBSHomePage).Show() = False Then
                        ClearOutDUDEFiles()
                        Exit While
                    Else
                        ClearOutDUDEFiles()
                    End If
                End If

                If (BinBeingWorked Is Nothing) Then
                    'if a bin wasn't selected and an SSN was provided then exit the loop and show bin form again
                    Exit While
                Else
                    'complete the task that was opened
                    SP.BorrowerLite.CompleteOLTaskForBin(BinBeingWorked.GetQueuesToBeWorked())
                    'Get task from queue with tasks in selected bin
                    BorLt = SP.BorrowerLite.GetOLTaskForBin(BinBeingWorked.GetQueuesToBeWorked())
                End If
            End While
        End While
    End Sub

    Public Shared Sub ClearOutDUDEFiles()
        'blank out TempDemoUpdate.txt
        If Dir("T:\TempDemoUpdate.txt") <> "" Then
            FileOpen(1, "T:\TempDemoUpdate.txt", OpenMode.Output)
            FileClose(1)
        End If
        'clear out DUDE's LastWords
        If Dir("T:\MauiDUDE_LastWords.txt") <> "" Then Kill("T:\MauiDUDE_LastWords.txt")
    End Sub

    'does a translation from enum to string
    Public Shared Function TranslateEnumToScriptDisableKey(ByVal BinPageSelectedOption As frmLMBins.WhatIAmGoingToGiveYou) As String
        If BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.AccountMaintenance Then
            Return "AM"
        Else
            Return ""
        End If
    End Function

    Private LMBor As BorrowerLM
    Public DemographicForm As frmLMDemographics
    Private AH30 As frmActivityHistory
    Private AH60 As frmActivityHistory
    Private AH90 As frmActivityHistory
    Private AH180 As frmActivityHistory
    Private AHAll As frmActivityHistory
    Private AHSinceOpen As frmActivityHistory
    Private ReturnResult As HomePageReturnResult
    Private mostat As frmMoreStatus
    Private BinPageSelectedOption As frmLMBins.WhatIAmGoingToGiveYou
    Private GetAttemptInfoFrm As frmGetAttemptInfo
    'arrays for activity and contact combo boxes
    Private ActivityCode As New ArrayList
    Private ContactCode As New ArrayList
    Private ReferenceDetailForm As frmReferenceDetail
    Private Queues As frmQueues
    Private LDetail As frmLoanDetail

    Public Enum HomePageReturnResult
        BackToBins = 0
        IncomingCall = 1
        SaveAndContinue = 2
    End Enum


    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    Public Sub New(ByVal BL As SP.BorrowerLite, ByVal tPreviousContacts As ArrayList, ByVal tBinPageSelectedOption As frmLMBins.WhatIAmGoingToGiveYou)
        MyBase.New("Account Resolution", BL, TranslateEnumToScriptDisableKey(tBinPageSelectedOption))
        LMBor = New BorrowerLM(BL, Me)
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        BinPageSelectedOption = tBinPageSelectedOption
        'change tab colors
        Dim TTab As TabPage
        For Each TTab In TabControl.TabPages
            TTab.BackColor = Me.BackColor
            TTab.ForeColor = Me.ForeColor
        Next
        'fill previous contacts box
        cbPreviousContacts.Items.AddRange(tPreviousContacts.ToArray())
        'populate activity type and contact type
        PopulateActivityTypeAndContactType()
        'populate call categorization
        Dim Comm As New SqlClient.SqlCommand("SELECT Category FROM CallCat_Categories WHERE BusinessUnit = 'B' or BusinessUnit = 'A'", SP.UsrInf.Conn)
        SP.UsrInf.Conn.Open()
        Dim Reader As SqlClient.SqlDataReader
        Reader = Comm.ExecuteReader
        While Reader.Read
            cbCCCat.Items.Add(Reader("Category"))
        End While
        SP.UsrInf.Conn.Close()
    End Sub

    Public Function GatherInformation(ByVal DisplayDemographics As Boolean) As Boolean
        SP.Processing.Visible = True
        DemographicForm = New frmLMDemographics(LMBor, New Resources.ResourceManager("MDLMHome.MauiDUDERes", Me.GetType.Assembly))
        LMBor.TPDDCheck()
        If DemographicForm.PopulateFrm(True) = False Then
            Return False
        End If
        'check if the demographics screen should be displayed
        If DisplayDemographics Then
            'demographics should be displayed so multithreading can be used
            Dim turboSpeed As New Thread(AddressOf LMBor.Turbo)
            turboSpeed.Start()

            ShowDemoFrmAndUpdateHPWithResults(True)

            If DemographicForm.BackButtonClicked Then
                turboSpeed.Abort()
                Return False
            End If

            Do While turboSpeed.IsAlive
                Thread.CurrentThread.Sleep(100)
            Loop
        Else
            'demographics shouldn't be displayed so multithreading shouldn't be used (just get the information)
            LMBor.Turbo()
            tbDemoVerified.Text = "NOT VERIFIED" 'since the demographics screen was never displayed there is no way the demographics were verified
        End If

        SP.Processing.Visible = False

        LMBor.WriteOut()
        LMBor.SpillGuts()
        Return True
    End Function

    Public Sub PopulateHomePage(ByVal BinTaskCount As Integer, ByVal Bin As String)
        Dim x As Integer
        'populate form level info
        lblAcctNum.Text = LMBor.CLAccNum
        lblName.Text = LMBor.FirstName + " " + LMBor.LastName
        'figure status
        LMBor.FigureBorrowerStatus()
        'mark special status
        cbVIP.Checked = LMBor.VIP
        cbSpecialHandling.Checked = LMBor.SpecialHandling
        'main tab bin and queue info *************************************************************
        If BinTaskCount = 0 Then
            tbTaskLeftInBin.Text = "0"
        Else
            tbTaskLeftInBin.Text = (BinTaskCount - 1).ToString
        End If
        tbBin.Text = Bin
        tbQueue.Text = LMBor.BorLite.Queue
        tbQueueText.Text = LMBor.BorLite.QueueCommentText
        tbSSN.Text = LMBor.SSN
        tbCMSSN.Text = LMBor.CoMakerSSN
        tbCMName.Text = LMBor.CoMakerName
        'main tab populate demographics based off the user provided demos being populated ********
        If LMBor.UserProvidedDemos.Addr1 = "" Then
            PopulateDemoInfo(LMBor.OneLINKDemos)
        Else
            PopulateDemoInfo(LMBor.UserProvidedDemos)
        End If
        tbDtLastAttempt.Text = LMBor.DateLastAtempt
        tbDtLastContact.Text = LMBor.DateLastCntct
        tbCurrentEmployer.Text = LMBor.EmployerName
        If LMBor.EmployerName = "" Then
            Label18.ForeColor = Drawing.Color.Red
        End If
        'main tab Attempt and Contact buttons ***************************************************
        Dim PhoneNums As New Generic.List(Of String)
        PhoneNums.Add(tbHomePhn.Text + " Ext:" + tbHPExt.Text)
        If tbAltPhn.Text <> "" Then PhoneNums.Add(tbAltPhn.Text + " Ext:" + tbAPExt.Text)
        GetAttemptInfoFrm = New frmGetAttemptInfo(PhoneNums)
        'add handler for attempt info getter
        AddHandler GetAttemptInfoFrm.btnOK.Click, AddressOf OKReturnFromAttemptFrm
        AddHandler GetAttemptInfoFrm.btnCancel.Click, AddressOf CancelReturnFromAttemptFrm
        'main tab References ********************************************************************
        lvReferences.Items.AddRange(LMBor.References().ToArray)
        ReferenceDetailForm = New frmReferenceDetail()
        AddHandler ReferenceDetailForm.btnClose.Click, AddressOf ReturnFromReferenceDetail
        'main tab Payment Info ******************************************************************
        tbCurrPrinc.Text = FormatCurrency(LMBor.TotalDefaulted - LMBor.PrincipalCollected, 2)
        tbCurrInt.Text = FormatCurrency(LMBor.InterestAccrued - LMBor.InterestCollected, 2)
        tbCollectCost.Text = FormatCurrency(((LMBor.LegalCostAccrued - LMBor.LegalCostCollected) + (LMBor.OtherChargesAccrued - LMBor.OtherChargesCollected) + (LMBor.CollectionCostAccrued - LMBor.CollectionCostCollected) + LMBor.CollectionFeesPojected), 2)
        tbTotalAmtDue.Text = FormatCurrency((LMBor.TotalDefaulted - LMBor.PrincipalCollected) + (LMBor.InterestAccrued - LMBor.InterestCollected) + (LMBor.CollectionCostAccrued - LMBor.CollectionCostCollected) + CDbl(LMBor.CollectionFeesPojected), 2)
        tbDueDate.Text = LMBor.DueDate
        tbDPA.Text = LMBor.DirectDebit
        tbMonthPayAmt.Text = FormatCurrency(LMBor.MonthlyPA, 2)
        tbMonthInt.Text = FormatCurrency(LMBor.AgencyMonthlyInterest, 2)
        'main tab Repayment Calculations
        If LMBor.InfoFoundOnLG0H Then
            gbRepaymentCalc.Enabled = True
            tbRepay30Day.Text = Format(LMBor.Payoff30Days, "$ ###,##0")
            tbRepay3Year.Text = LMBor.RepaymentCalculator(36)
            tbRepay5Year.Text = LMBor.RepaymentCalculator(60)
            tbRepay7Year.Text = LMBor.RepaymentCalculator(84)
            tbRepay10Year.Text = LMBor.RepaymentCalculator(120)
            If LMBor.AcctBal >= 31000.0 And LMBor.EligibleForExtRepay Then
                tbRepay25Year.Text = LMBor.RepaymentCalculator(300)
            Else
                tbRepay25Year.Visible = False
            End If
        Else
            gbRepaymentCalc.Enabled = False
            tbRepay30Day.Text = "$0.00"
            tbRepay3Year.Text = "$0.00"
            tbRepay5Year.Text = "$0.00"
            tbRepay7Year.Text = "$0.00"
            tbRepay10Year.Text = "$0.00"
            tbRepay25Year.Text = "$0.00"
        End If
        'main tab paid in full
        tbPayOffAmount.Text = FormatCurrency(LMBor.OutstandingBalanceDue, 2)
        tbPayOffDate.Text = Format(Now, "MM/dd/yyyy")
        'call forwarding tab 
        SetCallForward(LMBor.HasTPDD)
        'legal tab
        Legal()
        'account information tab
        'principal
        tbPrincipalC.Text = FormatCurrency(CDbl(LMBor.TotalDefaulted), 2)
        tbPrincipalP.Text = FormatCurrency(CDbl(LMBor.PrincipalCollected), 2)
        tbPrincipalR.Text = FormatCurrency(CDbl(LMBor.TotalDefaulted) - CDbl(LMBor.PrincipalCollected))
        'interest
        tbInterestC.Text = FormatCurrency(CDbl(LMBor.InterestAccrued), 2)
        tbInterestP.Text = FormatCurrency(CDbl(LMBor.InterestCollected), 2)
        tbInterestR.Text = FormatCurrency(CDbl(LMBor.InterestAccrued) - CDbl(LMBor.InterestCollected))
        'legal
        tbLegalC.Text = FormatCurrency(CDbl(LMBor.LegalCostAccrued), 2)
        tbLegalP.Text = FormatCurrency(CDbl(LMBor.LegalCostCollected), 2)
        tbLegalR.Text = FormatCurrency(CDbl(LMBor.LegalCostAccrued) - CDbl(LMBor.LegalCostCollected))
        'other
        tbOtherC.Text = FormatCurrency(CDbl(LMBor.OtherChargesAccrued), 2)
        tbOtherP.Text = FormatCurrency(CDbl(LMBor.OtherChargesCollected), 2)
        tbOtherR.Text = FormatCurrency(CDbl(LMBor.OtherChargesAccrued) - CDbl(LMBor.OtherChargesCollected))
        'collections
        tbCollectionC.Text = FormatCurrency(CDbl(LMBor.CollectionCostAccrued), 2)
        tbCollectionP.Text = FormatCurrency(CDbl(LMBor.CollectionCostCollected), 2)
        tbCollectionR.Text = FormatCurrency(CDbl(LMBor.CollectionCostAccrued) - CDbl(LMBor.CollectionCostCollected))
        'projected
        tbProjectedC.Text = FormatCurrency(CDbl(LMBor.CollectionFeesPojected), 2)
        tbProjectedR.Text = FormatCurrency(CDbl(LMBor.CollectionFeesPojected), 2)
        'total
        tbTotalC.Text = FormatCurrency(CDbl(LMBor.TotalDefaulted) + CDbl(LMBor.InterestAccrued) + CDbl(LMBor.LegalCostAccrued) + CDbl(LMBor.OtherChargesAccrued) + CDbl(LMBor.CollectionCostAccrued) + CDbl(LMBor.CollectionFeesPojected), 2)
        tbTotalP.Text = FormatCurrency(CDbl(LMBor.PrincipalCollected) + CDbl(LMBor.InterestCollected) + CDbl(LMBor.LegalCostCollected) + CDbl(LMBor.OtherChargesCollected) + CDbl(LMBor.CollectionCostCollected), 2)
        tbTotalR.Text = FormatCurrency((CDbl(LMBor.TotalDefaulted) + CDbl(LMBor.InterestAccrued) + CDbl(LMBor.LegalCostAccrued) + CDbl(LMBor.OtherChargesAccrued) + CDbl(LMBor.CollectionCostAccrued) + CDbl(LMBor.CollectionFeesPojected)) - (CDbl(LMBor.PrincipalCollected) + CDbl(LMBor.InterestCollected) + CDbl(LMBor.LegalCostCollected) + CDbl(LMBor.OtherChargesCollected) + CDbl(LMBor.CollectionCostCollected)), 2)
        'last nine payments
        If LMBor.PaymentsFromLC41.Count > 8 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 9))
        If LMBor.PaymentsFromLC41.Count > 7 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 8))
        If LMBor.PaymentsFromLC41.Count > 6 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 7))
        If LMBor.PaymentsFromLC41.Count > 5 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 6))
        If LMBor.PaymentsFromLC41.Count > 4 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 5))
        If LMBor.PaymentsFromLC41.Count > 3 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 4))
        If LMBor.PaymentsFromLC41.Count > 2 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 3))
        If LMBor.PaymentsFromLC41.Count > 1 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 2))
        If LMBor.PaymentsFromLC41.Count > 0 Then lstLC41.Items.Add(LMBor.PaymentsFromLC41.Item(LMBor.PaymentsFromLC41.Count - 1))
        'reinstatement
        Select Case LMBor.ReinstatementEligibilityCode
            Case "N"
                tbReinstatementEligibilityCode.Text = "NOT ELIGIBLE"
            Case "R"
                tbReinstatementEligibilityCode.Text = "REINSTATED"
            Case "X"
                tbReinstatementEligibilityCode.Text = "REVOKED"
            Case "Y"
                tbReinstatementEligibilityCode.Text = "ELIGIBLE"
            Case Else
                tbReinstatementEligibilityCode.Text = ""
        End Select
        tbReinstatementDate.Text = LMBor.ReinstatementDate
        'rehab
        tbRehabCounter.Text = LMBor.RehabCounter
        tbRehabCode.Text = LMBor.RehabCode
        Select Case LMBor.IneligibleForRehabCode
            Case "IN"
                tbIneligibleForRehabCode.Text = "INELIGIBLE"
            Case "LB"
                tbIneligibleForRehabCode.Text = "LOW BALANCE"
            Case "PR"
                tbIneligibleForRehabCode.Text = "PREVIOUSLY REHABBED"
            Case "RT"
                tbIneligibleForRehabCode.Text = "REMAINING TERM"
            Case Else
                tbIneligibleForRehabCode.Text = ""
        End Select
        'Loan Programs
        x = 0
        For x = LBound(LMBor.LG10Data, 2) To UBound(LMBor.LG10Data, 2)
            If InStr(tbLoanPg.Text, LMBor.LG10Data(1, x)) = 0 Then
                tbLoanPg.Text = tbLoanPg.Text & "," & LMBor.LG10Data(1, x)
            End If
        Next x
        'collection code
        tbCollectionInd.Text = LMBor.CollectionInd
        'pending payments
        tbPendingPayments.Text = LMBor.PaymentPendingInfo
        'servicer info
        SetServicer()
    End Sub

    Sub SetServicer()
        Dim x As Integer
        '1 = loan type
        '2 = servicer code
        '3 = date processed
        '4 = guarantee amount
        '5 = loan status
        '6 = loan status reason code
        For x = 1 To LMBor.LG10Data.GetUpperBound(1)
            If LMBor.LG10Data(2, x) = "700126" And LMBor.LG10Data(5, x) <> "AL" And LMBor.LG10Data(5, x) <> "CA" And LMBor.LG10Data(5, x) <> "CP" And LMBor.LG10Data(5, x) <> "DN" And LMBor.LG10Data(5, x) <> "PC" And LMBor.LG10Data(5, x) <> "PF" And LMBor.LG10Data(5, x) <> "PM" And LMBor.LG10Data(5, x) <> "PN" And LMBor.LG10Data(5, x) <> "RF" Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("BSV")
                End If
            End If
            If LMBor.LG10Data(2, x) <> "813285" And _
             LMBor.LG10Data(2, x) = "821623" And _
             LMBor.LG10Data(2, x) = "820164" And _
             LMBor.LG10Data(2, x) = "821614" And _
             LMBor.LG10Data(5, x) <> "AL" And LMBor.LG10Data(5, x) <> "CA" And LMBor.LG10Data(5, x) <> "CP" And LMBor.LG10Data(5, x) <> "DN" And LMBor.LG10Data(5, x) <> "PC" And LMBor.LG10Data(5, x) <> "PF" And LMBor.LG10Data(5, x) <> "PM" And LMBor.LG10Data(5, x) <> "PN" And LMBor.LG10Data(5, x) <> "RF" Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("CHELA")
                End If
            End If
            If LMBor.LG10Data(2, x) <> "700126" And _
             LMBor.LG10Data(2, x) <> "813285" And _
             LMBor.LG10Data(2, x) <> "821623" And _
             LMBor.LG10Data(2, x) <> "820164" And _
             LMBor.LG10Data(2, x) <> "821614" And _
             LMBor.LG10Data(2, x) <> "830151" And _
             LMBor.LG10Data(2, x) <> "831053" And _
             LMBor.LG10Data(2, x) <> "899993" And _
             LMBor.LG10Data(2, x) <> "899986" And _
             LMBor.LG10Data(2, x) <> "829587" And _
             LMBor.LG10Data(2, x) <> "833253" And _
             LMBor.LG10Data(2, x) <> "888885" And _
             LMBor.LG10Data(2, x) <> "831474" And _
             LMBor.LG10Data(2, x) <> "830084" And _
             LMBor.LG10Data(5, x) <> "AL" And LMBor.LG10Data(5, x) <> "CA" And LMBor.LG10Data(5, x) <> "CP" And LMBor.LG10Data(5, x) <> "DN" And LMBor.LG10Data(5, x) <> "PC" And LMBor.LG10Data(5, x) <> "PF" And LMBor.LG10Data(5, x) <> "PM" And LMBor.LG10Data(5, x) <> "PN" And LMBor.LG10Data(5, x) <> "RF" Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("Nelnet")
                End If
            End If
            If LMBor.LG10Data(2, x) <> "700126" And _
             LMBor.LG10Data(2, x) = "830151" And _
             LMBor.LG10Data(2, x) = "831053" And _
             LMBor.LG10Data(2, x) = "899993" And _
             LMBor.LG10Data(2, x) = "899986" And _
             LMBor.LG10Data(2, x) = "829587" And _
             LMBor.LG10Data(2, x) = "833253" And _
             LMBor.LG10Data(2, x) = "888885" And _
             LMBor.LG10Data(2, x) = "831474" And _
             LMBor.LG10Data(2, x) = "830084" And _
             LMBor.LG10Data(5, x) <> "AL" And LMBor.LG10Data(5, x) <> "CA" And LMBor.LG10Data(5, x) <> "CP" And LMBor.LG10Data(5, x) <> "DN" And LMBor.LG10Data(5, x) <> "PC" And LMBor.LG10Data(5, x) <> "PF" And LMBor.LG10Data(5, x) <> "PM" And LMBor.LG10Data(5, x) <> "PN" And LMBor.LG10Data(5, x) <> "RF" Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("Salie Mae")
                End If
            End If
        Next
    End Sub

    'populates legal tab
    Private Sub Legal()
        'Judgment / Garnishment / Offsets

        'garnishments
        If LMBor.LastPaymentAmount = 0 Then
            tbLastPaymentAmount.Text = ""
            tbLastPaymentDt.Text = ""
        Else
            tbLastPaymentAmount.Text = FormatCurrency(CDbl(LMBor.LastPaymentAmount), 2)
            tbLastPaymentDt.Text = LMBor.LastPaymentRecieved
        End If
        If LMBor.GarnishmentType = "" Then
            tbAJOutstandingDt.Text = ""
            tbPrimaryAction.Text = ""
        End If
        tbGarnish.Text = LMBor.GarnishmentType
        If LMBor.Day30Notice = "MM/DD/CCYY" Then
            tbDay30Notice.Text = ""
        Else
            tbDay30Notice.Text = LMBor.Day30Notice
        End If

        'judgements
        If LMBor.OutstandingAWG = "Yes" Then
            tbPrimaryAction.Text = LMBor.PrimaryActionDate
            tbAJOutstandingDt.Text = ""
        ElseIf LMBor.ActiveJudgment = "Yes" Then
            tbAJOutstandingDt.Text = LMBor.PrimaryActionDate
            tbPrimaryAction.Text = ""
        End If
        tbAJOutstanding.Text = LMBor.ActiveJudgment

        'tax offset
        Select Case LMBor.TaxOffsetCode
            Case "01"
                LMBor.TaxOffsetCode = "65-DAY LETTER SENT IRS ADDRESS"
            Case "02"
                LMBor.TaxOffsetCode = "CERTIFIED WITH IRS ADDRESS"
            Case "03"
                LMBor.TaxOffsetCode = "REASSIGNED TO UHEAA (IRS ADDR)"
            Case "04"
                LMBor.TaxOffsetCode = "DELETION REQUEST (IRS ADDRESS)"
            Case "05"
                LMBor.TaxOffsetCode = "FORCED TREASURY OFFSET (IRS)"
            Case "06"
                LMBor.TaxOffsetCode = "65-DAY LETTER SENT UHEAA ADDRS"
            Case "07"
                LMBor.TaxOffsetCode = "CERTIFIED WITH UHEAA ADDRESS"
            Case "08"
                LMBor.TaxOffsetCode = "REASSIGNED TO UHEA (UHEAA ADDR)"
            Case "09"
                LMBor.TaxOffsetCode = "DELETION REQUEST (UHEAA ADDR)"
            Case "10"
                LMBor.TaxOffsetCode = "FORCED TREASURY OFFSET (UHEAA)"
            Case "11"
                LMBor.TaxOffsetCode = "INACTIVE - IRS ADDRESS"
            Case "12"
                LMBor.TaxOffsetCode = "INACTIVE - UHEAA ADDRESS"
            Case Else
                LMBor.TaxOffsetCode = ""
        End Select
        tbCertified.Text = LMBor.Certified
        tbTaxOffset.Text = LMBor.TaxOffsetCode
        tbYearsOffset.Text = LMBor.YearsCertified
        tbTaxOffsetAmout.Text = LMBor.OffsetAmt
        If LMBor.InterestReported <> "" Then tb1098.Text = FormatCurrency(CDbl(LMBor.InterestReported), 2) Else tb1098.Text = FormatCurrency(0, 2)
        tb1098Dt.Text = LMBor.Date1098ESent
    End Sub

    'populates the demographics on home page
    Private Sub PopulateDemoInfo(ByVal Demos As SP.Demographics)
        Dim StrikeThroughFont As New System.Drawing.Font(tbAddr1.Font, Drawing.FontStyle.Strikeout)
        Dim Addr As Boolean
        Dim HomePhone As Boolean
        Dim AltPhone As Boolean
        Dim Email As Boolean
        'populate indicators
        If Demos.TheSystem = SP.Demographics.WhatSystem.Onelink Then
            Addr = (Demos.SPAddrInd = "Y")
            HomePhone = (Demos.HomePhoneValidityIndicator = "Y")
            AltPhone = (Demos.OtherPhoneValidityIndicator = "Y")
            Email = (Demos.SPEmailInd = "Y")
        Else
            Addr = Demos.UPAddrVal
            HomePhone = Demos.UPPhoneVal
            AltPhone = Demos.UPOtherVal
            Email = Demos.UPEmailVal
        End If
        'address info
        tbAddr1.Text = Demos.Addr1
        tbAddr2.Text = Demos.Addr2
        tbCity.Text = Demos.City
        tbState.Text = Demos.State
        tbZIP.Text = Demos.Zip
        If Addr = False Then
            'strike through text if invalid address
            tbAddr1.Font = StrikeThroughFont
            tbAddr2.Font = StrikeThroughFont
            tbCity.Font = StrikeThroughFont
            tbState.Font = StrikeThroughFont
            tbZIP.Font = StrikeThroughFont
        End If
        'home phone info
        tbHomePhn.Text = Demos.HomePhoneNum
        tbHPExt.Text = Demos.HomePhoneExt
        If HomePhone = False And Demos.HomePhoneNum <> "" Then
            'strike through text if invalid home phone
            tbHomePhn.Font = StrikeThroughFont
            tbHPExt.Font = StrikeThroughFont
        End If
        'alt phone info
        tbAltPhn.Text = Demos.OtherPhoneNum
        tbAPExt.Text = Demos.OtherPhoneExt
        If AltPhone = False And Demos.OtherPhoneNum <> "" Then
            'strike through text if invalid home phone
            tbAltPhn.Font = StrikeThroughFont
            tbAPExt.Font = StrikeThroughFont
        End If
        'email info
        tbEmail.Text = Demos.Email
        If Email = False And Demos.Email <> "" Then
            'strike through text if invalid home phone
            tbEmail.Font = StrikeThroughFont
        End If
    End Sub

    Sub SetCallForward(ByVal HasTPDD As Boolean)
        Dim x As Integer
        Dim stp3 As Boolean
        Dim stp4 As Boolean
        Dim stp5 As Boolean
        '1 = loan type
        '2 = servicer code
        '3 = date processed
        '4 = guarantee amount
        '5 = loan status
        '6 = loan status reason code
        For x = LMBor.LG10Data.GetLowerBound(1) To LMBor.LG10Data.GetUpperBound(1)
            If (LMBor.LG10Data(5, x) = "CP" Or LMBor.LG10Data(5, x) = "DN") And LMBor.LG10Data(6, x) <> "BC" And LMBor.LG10Data(6, x) <> "BO" And LMBor.LG10Data(6, x) <> "BH" Then
                If lstCallF.FindStringExact("LMS: Default 7272") = -1 Then
                    lstCallF.Items.Add("LMS: Default 7272")
                End If
            End If
        Next
        'step 3
        For x = LMBor.LG10Data.GetLowerBound(1) To LMBor.LG10Data.GetUpperBound(1)
            If LMBor.LG10Data(5, x) = "CP" And (LMBor.LG10Data(6, x) = "BC" Or LMBor.LG10Data(6, x) = "BO" Or LMBor.LG10Data(6, x) = "BH") Then
                If lstCallF.FindStringExact("Auxiliary Services 7274") = -1 Then
                    lstCallF.Items.Add("Auxiliary Services 7274")
                    stp3 = True
                End If
            End If
        Next
        'step 4
        For x = LMBor.LG10Data.GetLowerBound(1) To LMBor.LG10Data.GetUpperBound(1)
            If (LMBor.LG10Data(5, x) = "CR") And LMBor.LG10Data(6, x) <> "BC" And LMBor.LG10Data(6, x) <> "BO" And LMBor.LG10Data(6, x) <> "BH" Then
                If lstCallF.FindStringExact("LMS: Preclaim 7246") = -1 Then
                    lstCallF.Items.Add("LMS: Preclaim 7246")
                    stp4 = True
                End If
            End If
        Next
        'step 5
        For x = LMBor.LG10Data.GetLowerBound(1) To LMBor.LG10Data.GetUpperBound(1)
            If LMBor.LG10Data(5, x) = "CR" And (LMBor.LG10Data(6, x) = "BC" Or LMBor.LG10Data(6, x) = "BO" Or LMBor.LG10Data(6, x) = "BH") Then
                If lstCallF.FindStringExact("Auxiliary Services 7274") = -1 Then
                    lstCallF.Items.Add("Auxiliary Services 7274")
                    stp5 = True
                End If
            End If
        Next
        For x = LMBor.LG10Data.GetLowerBound(1) To LMBor.LG10Data.GetUpperBound(1)
            If LMBor.LG10Data(2, x) = "700126" And LMBor.LG10Data(5, x) <> "CA" And LMBor.LG10Data(5, x) <> "PC" And LMBor.LG10Data(5, x) <> "PF" And LMBor.LG10Data(5, x) <> "PM" And LMBor.LG10Data(5, x) <> "PN" And LMBor.LG10Data(5, x) <> "RF" Then
                If lstCallF.FindStringExact("Borrower Services 7294") = -1 Then
                    lstCallF.Items.Add("Borrower Services 7294")
                End If
            End If
        Next
        For x = LMBor.LG10Data.GetLowerBound(1) To LMBor.LG10Data.GetUpperBound(1)
            If LMBor.LG10Data(2, x) = "700126" And LMBor.LG10Data(5, x) = "" Then
                If lstCallF.FindStringExact("Borrower Services 7294") = -1 Then
                    lstCallF.Items.Add("Borrower Services 7294")
                End If
            End If
        Next
        If HasTPDD And stp3 = False Then
            If lstCallF.FindStringExact("Borrower Services 7294") = -1 Then
                lstCallF.Items.Add("Borrower Services 7294")
            End If
        End If
        For x = LMBor.LG10Data.GetLowerBound(1) To LMBor.LG10Data.GetUpperBound(1)
            If stp3 = False And (LMBor.LG10Data(2, x) = "700079" Or LMBor.LG10Data(2, x) = "700004" Or _
             LMBor.LG10Data(2, x) = "700789" Or LMBor.LG10Data(2, x) = "700191" Or _
             LMBor.LG10Data(2, x) = "700190" Or LMBor.LG10Data(2, x) = "700121") And _
             LMBor.LG10Data(5, x) <> "CA" And LMBor.LG10Data(5, x) <> "PC" And LMBor.LG10Data(5, x) <> "PF" And LMBor.LG10Data(5, x) <> "PM" And LMBor.LG10Data(5, x) <> "PN" And LMBor.LG10Data(5, x) <> "RF" Then
                If lstCallF.FindStringExact("LMS: Default 7272") = -1 Then
                    lstCallF.Items.Add("LMS: Default 7272")
                End If
            End If
        Next
    End Sub

    Public Sub RunContactScript()
        MsgBox("Contact script goes here")
    End Sub

    Public Overloads Function ShowDialog() As HomePageReturnResult
        MyBase.ShowDialog()
        Return ReturnResult
    End Function

    Private Sub btnBackToBins_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackToBins.Click
        CloseAllSubForms()
        ReturnResult = HomePageReturnResult.BackToBins
        Me.Hide()
    End Sub

    Sub RunActivityHistory(ByVal days As Integer)
        Dim TempACFrm As frmActivityHistory
        Try
            If days = 0 Then
                AppActivate("Maui DUDE Complete Borrower Activity History - LP50")
            Else
                AppActivate("Maui DUDE Borrower " & days & " Day Activity History - LP50")
            End If
        Catch
            SP.Processing.Visible = True
            SP.Processing.Refresh()
            If days = 30 Then
                AH30 = New frmActivityHistory(LMBor)
                TempACFrm = AH30
            ElseIf days = 60 Then
                AH60 = New frmActivityHistory(LMBor)
                TempACFrm = AH60
            ElseIf days = 90 Then
                AH90 = New frmActivityHistory(LMBor)
                TempACFrm = AH90
            ElseIf days = 180 Then
                AH180 = New frmActivityHistory(LMBor)
                TempACFrm = AH180
            ElseIf days = 0 Then
                AHAll = New frmActivityHistory(LMBor)
                TempACFrm = AHAll
            Else
                AHSinceOpen = New frmActivityHistory(LMBor)
                TempACFrm = AHSinceOpen
            End If
            If days = 0 Then
                TempACFrm.Show(days, "Maui DUDE Complete Borrower Activity History - LP50")
            Else
                TempACFrm.Show(days, "Maui DUDE Borrower " & days & " Day Activity History - LP50")
            End If
            SP.Processing.Visible = False
        End Try
    End Sub

    Private Sub tbIncomingCallSSN_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbIncomingCallSSN.Enter
        tbIncomingCallSSN.SelectAll()
    End Sub

    Private Sub tbIncomingCallSSN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbIncomingCallSSN.KeyUp
        If e.KeyCode.Equals(Keys.Enter) Then
            IncomingCall()
        End If
    End Sub

    Private Sub tbIncomingCallSSN_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbIncomingCallSSN.MouseClick
        tbIncomingCallSSN.SelectAll()
    End Sub

    Private Sub tbIncomingCallSSN_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbIncomingCallSSN.MouseDoubleClick
        tbIncomingCallSSN.SelectAll()
    End Sub

    Private Sub cbPreviousContacts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPreviousContacts.SelectedIndexChanged
        tbIncomingCallSSN.Text = cbPreviousContacts.SelectedItem().ToString
        IncomingCall()
    End Sub

    Private Sub btnIncomingCall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIncomingCall.Click
        IncomingCall()
    End Sub

    Private Sub IncomingCall()
        ReturnResult = HomePageReturnResult.IncomingCall
        CloseAllSubForms()
        Me.Hide()
    End Sub

    Private Sub btnMoStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoStatus.Click
        Try
            AppActivate("Mo Status")
        Catch
            mostat = New frmMoreStatus
            mostat.Show(LMBor.AllStatuses())
        End Try
    End Sub

    Private Sub btnAttempt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttempt.Click
        Me.Enabled = False
        GetAttemptInfoFrm.Show()
    End Sub


    Private Sub btnContact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContact.Click
        Dim Contact As New frmContactScript(LMBor, Scripts)
        Contact.Show()
    End Sub

    'this function should be called each time the demographic form is displayed
    Private Sub ShowDemoFrmAndUpdateHPWithResults(Optional ByVal DisplayingOnEntry As Boolean = False)
        If DisplayingOnEntry = False Then Me.Visible = False
        'Populate activity type and contact type on demographic screen
        DemographicForm.LoadActivityAndContactType(tbActivityCode.Text, tbContactCode.Text)
        If DisplayingOnEntry Then
            DemographicForm.ShowDialog()
        Else
            DemographicForm.ShowDialog(True, True)
        End If
        'get activity type and conatact type from demographics and re-populate homepage controls
        DemographicForm.GetActivityAndContactType(tbActivityCode.Text, tbContactCode.Text)
        'check if the borrowers demographics were verified while on demographics form
        If LMBor.DemographicsVerified Then
            tbDemoVerified.Text = "VERIFIED"
            PopulateDemoInfo(LMBor.UserProvidedDemos)
        Else
            tbDemoVerified.Text = "NOT VERIFIED"
        End If
        LMBor.WriteOut()
        LMBor.SpillGuts()
        If DisplayingOnEntry = False Then Me.Visible = True
    End Sub

    Private Sub lvReferences_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvReferences.DoubleClick
        CType(lvReferences.SelectedItems.Item(0), Reference).GetInfoFromLP2C()
        ReferenceDetailForm.PopulateForm(CType(lvReferences.SelectedItems(0), Reference))
        lvReferences.Enabled = False
        ReferenceDetailForm.Show()
        ReferenceDetailForm.Focus()
    End Sub

    Private Sub tbPayOffDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbPayOffDate.KeyUp
        If e.KeyCode.Equals(Keys.Enter) Then
            If LMBor.InfoFoundOnLC10 Then 'only try and do update if LC10 info was collected previously
                If IsDate(tbPayOffDate.Text) = False Then
                    SP.frmWhoaDUDE.WhoaDUDE("DUDE, you gotta like, provide a valid date for the pay off calculation.", "Invalid Pay Off Date")
                Else
                    SP.FastPath("LC10I" + LMBor.SSN)
                    SP.PutText(9, 20, tbPayOffDate.Text.ToString.Replace("/", ""), True)
                    tbPayOffAmount.Text = FormatCurrency(SP.GetText(18, 36, 12), 2)
                End If
            End If
        End If
    End Sub

    Private Sub cbCCCat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCCCat.SelectedIndexChanged
        'wipe out current population
        While cbCCRea.Items.Count <> 0
            cbCCRea.Items.RemoveAt(0)
        End While
        'populate call category reason drop down box 
        Dim Comm As New SqlClient.SqlCommand("SELECT Reason FROM CallCat_CatReasonREF WHERE Category = '" + cbCCCat.SelectedItem + "' AND  (BusinessUnit = 'B' or BusinessUnit = 'A')", SP.UsrInf.Conn)
        SP.UsrInf.Conn.Open()
        Dim Reader As SqlClient.SqlDataReader
        Reader = Comm.ExecuteReader
        While Reader.Read
            cbCCRea.Items.Add(Reader("Reason"))
        End While
        SP.UsrInf.Conn.Close()
        If cbCCRea.Items.Count <> 0 Then
            cbCCRea.Enabled = True
        Else
            cbCCRea.Enabled = False
        End If
        tbCCLtrID.Enabled = True
        tbCCCmts.Enabled = True
    End Sub

    Private Sub btnSaveAndCont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAndCont.Click
        'ensure that a activity code and contact code are populated
        If tbActivityCode.Text = "" Or tbContactCode.Text = "" Or tbActionCode.Text = "" Then
            SP.frmWhoaDUDE.WhoaDUDE("DUDE, ya gotta provide an activity, contact and action code before I can do the hula for ya.", "Activity And Contact Code Needed")
            Exit Sub
        End If
        'if the user entered the homepage through clicking account maintanence then display demographics
        If LMBor.DemographicsVerified = False Then
            ShowDemoFrmAndUpdateHPWithResults(False) 'display demographics if they haven't been verified yet
        End If
        SP.Processing.Visible = True
        'check if call categorization needs are met
        If CallCategorizationCheckOK() Then
            'save notes to borrower object
            LMBor.Notes = tbActivityComments.Text
            'save codes to borrower object
            LMBor.ActivityCode = tbActivityCode.Text
            LMBor.ContactCode = tbContactCode.Text
            LMBor.ActionCode = tbActionCode.Text
            'spill guts to file in case DUDE freezes during updates
            LMBor.SpillGuts()
            'update demographics if they were verified
            DemographicForm.UpdateSys()
            'add comments to system
            LMBor.ActivityCmts.AddCommentsToLP50(LMBor.Notes, LMBor.ActionCode, LMBor.ContactCode, LMBor.ActivityCode)
            'closing of queue task occures in LMBinHomePageProcCoord sub
            ReturnResult = HomePageReturnResult.SaveAndContinue
            Me.Hide()
        End If

        SP.Processing.Visible = False
    End Sub

    'does call categorization data checks
    Private Function CallCategorizationCheckOK() As Boolean
        CallCategorizationCheckOK = True 'assume call categorization isn't needed or is valid
        'make sure that call categorization info was provided if needed
        If tbActivityCode.Text = "TC" And (tbContactCode.Text = "02" Or tbContactCode.Text = "04" Or _
                                           tbContactCode.Text = "70" Or tbContactCode.Text = "94") Then
            'if Call Categorization is visible then it is required
            'make sure that category is provided
            If cbCCCat.SelectedIndex = -1 Then
                SP.frmWhoaDUDE.WhoaDUDE("Gotta provide a call category!", "Call Category")
                Return False
            End If
            'check if reason is given if reasons are avail
            If cbCCRea.Items.Count <> 0 And cbCCRea.SelectedIndex = -1 Then
                SP.frmWhoaDUDE.WhoaDUDE("Gotta provide a call categorization reason!", "Call Categorization Reason")
                Return False
            End If
            'make sure that misc info is provided if certain options are selected
            If cbCCCat.SelectedItem = "Received Letter" And tbCCLtrID.Text = "" Then
                SP.frmWhoaDUDE.WhoaDUDE("Gotta provide a letter id if the call is about a letter the borrower received!", "Letter ID")
                Return False
            ElseIf cbCCCat.SelectedItem = "Other" And tbCCCmts.Text = "" Then
                SP.frmWhoaDUDE.WhoaDUDE("Gotta provide some comments if the call category is ""Other""!", "Additional Comments Needed")
                Return False
            End If
            'update DB
            Dim Comm As New SqlClient.SqlCommand(String.Format("EXEC spCallCategorizationInsert '{0}','{1}','{2}','{3}','{4}'", cbCCCat.SelectedItem, cbCCRea.SelectedItem, tbCCLtrID.Text.Replace("'", "''"), tbCCCmts.Text.Replace("'", "''"), Environment.UserName), SP.UsrInf.Conn)
            SP.UsrInf.Conn.Open()
            Comm.ExecuteNonQuery()
            SP.UsrInf.Conn.Close()
        End If
    End Function

    'closes all sub forms of home page when home page is dismissed
    Sub CloseAllSubForms()
        If (SP.BorrInfo Is Nothing) = False Then SP.BorrInfo.Hide()
        If (DemographicForm Is Nothing) = False Then DemographicForm.Hide()
        'If (PayHist Is Nothing) = False Then PayHist.Hide()
        If (mostat Is Nothing) = False Then mostat.Hide()
        If (AH30 Is Nothing) = False Then AH30.Hide()
        If (AH90 Is Nothing) = False Then AH90.Hide()
        If (AH180 Is Nothing) = False Then AH180.Hide()
        If (AH60 Is Nothing) = False Then AH60.Hide()
        If (AHAll Is Nothing) = False Then AHAll.Hide()
        If (AHSinceOpen Is Nothing) = False Then AHSinceOpen.Hide()
        'If (OLoans Is Nothing) = False Then OLoans.Hide()
        If (LDetail Is Nothing) = False Then LDetail.Hide()
        If (SP.AskDUDE Is Nothing) = False Then SP.AskDUDE.Hide()
        'If (DirectDebit Is Nothing) = False Then DirectDebit.Hide()
        If (Queues Is Nothing) = False Then Queues.Hide()
        If (ReferenceDetailForm Is Nothing) = False Then ReferenceDetailForm.Hide()
        If (GetAttemptInfoFrm Is Nothing) = False Then GetAttemptInfoFrm.Hide()
    End Sub

#Region "Activity code and contact code logic"

    Private Sub PopulateActivityTypeAndContactType()
        'IMPORTANT: if an item is added to one of these array lists then it must be added to all of them
        ActivityCode.Add("AM")
        ActivityCode.Add("CD")
        ActivityCode.Add("CL")
        ActivityCode.Add("CO")
        ActivityCode.Add("ED")
        ActivityCode.Add("EM")
        ActivityCode.Add("ET")
        ActivityCode.Add("FA")
        ActivityCode.Add("FO")
        ActivityCode.Add("LT")
        ActivityCode.Add("MS")
        ActivityCode.Add("PC")
        ActivityCode.Add("T1")
        ActivityCode.Add("T2")
        ActivityCode.Add("T3")
        ActivityCode.Add("T4")
        ActivityCode.Add("T5")
        ActivityCode.Add("T6")
        ActivityCode.Add("T7")
        ActivityCode.Add("T8")
        ActivityCode.Add("TA")
        ActivityCode.Add("TC")
        ActivityCode.Add("TE")
        ActivityCode.Add("TT")
        ActivityCode.Add("OV")

        'IMPORTANT: if an item is added to one of these array lists then it must be added to all of them
        ContactCode.Add("33")
        ContactCode.Add("34")
        ContactCode.Add("03")
        ContactCode.Add("04")
        ContactCode.Add("93")
        ContactCode.Add("94")
        ContactCode.Add("83")
        ContactCode.Add("84")
        ContactCode.Add("91")
        ContactCode.Add("92")
        ContactCode.Add("81")
        ContactCode.Add("82")
        ContactCode.Add("69")
        ContactCode.Add("70")
        ContactCode.Add("11")
        ContactCode.Add("12")
        ContactCode.Add("29")
        ContactCode.Add("30")
        ContactCode.Add("05")
        ContactCode.Add("06")
        ContactCode.Add("95")
        ContactCode.Add("96")
        ContactCode.Add("89")
        ContactCode.Add("90")
        ContactCode.Add("85")
        ContactCode.Add("86")
        ContactCode.Add("27")
        ContactCode.Add("28")
        ContactCode.Add("07")
        ContactCode.Add("08")
        ContactCode.Add("09")
        ContactCode.Add("10")
        ContactCode.Add("TO")
        ContactCode.Add("TI")
    End Sub

    'this function coordinates the matching between the Activity combo box and the applicable text box
    Private Sub cbActivityDesc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbActivityDesc.SelectedIndexChanged
        If cbActivityDesc.SelectedIndex <> 0 Then
            tbActivityCode.Text = ActivityCode(cbActivityDesc.SelectedIndex - 1)
        Else
            tbActivityCode.Text = ""
        End If
    End Sub

    'this function coordinates the matching between the contact combo box and the applicable text box
    Private Sub cbContactDesc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbContactDesc.SelectedIndexChanged
        If cbContactDesc.SelectedIndex <> 0 Then

            tbContactCode.Text = ContactCode(cbContactDesc.SelectedIndex - 1)

        Else
            tbContactCode.Text = ""
        End If
    End Sub

    'this function coordinates the matching of the activity code text box and the applicable description combo box
    Private Sub txtActivityCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbActivityCode.TextChanged
        Dim counter As Integer
        Dim Found As Boolean
        If tbActivityCode.TextLength = 2 Then
            While counter < ActivityCode.Count And Found = False
                If UCase(tbActivityCode.Text) = ActivityCode(counter) Then
                    Found = True
                End If
                counter = counter + 1
            End While
            'if a match was found then move to the next box else stay in textbox and highlight text
            If Found Then
                cbActivityDesc.SelectedIndex = counter
                tbActivityCode.Text = tbActivityCode.Text.ToUpper
                tbContactCode.Focus()
            Else
                SP.frmWhoaDUDE.WhoaDUDE("DUDE couldn't do the hula with that entry.", "Bad Activity Code", True)
                tbActivityCode.Focus()
                tbActivityCode.SelectAll()
            End If
        End If
    End Sub

    'this function coordinates the matching of the contact code text box and the applicable description combo box
    Private Sub txtContactCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbContactCode.TextChanged
        Dim counter As Integer
        Dim Found As Boolean
        If tbContactCode.TextLength = 2 Then
            While counter < ContactCode.Count And Found = False
                If UCase(tbContactCode.Text) = ContactCode(counter) Then
                    Found = True
                End If
                counter = counter + 1
            End While
            'if a match was found then move to the next box else stay in textbox and highlight text
            If Found Then
                cbContactDesc.SelectedIndex = counter
                tbContactCode.Text = tbContactCode.Text.ToUpper

            Else
                SP.frmWhoaDUDE.WhoaDUDE("DUDE couldn't do the hula with that entry.", "Bad Contact Code", True)
                tbContactCode.Focus()
                tbContactCode.SelectAll()
            End If
        End If
    End Sub

#End Region

#Region "Tool bar buttons"

    Private Sub tsbtnBrightIdea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnBrightIdea.Click
        If SP.frmEmailComments.BrightIdea() = True Then
            AppActivate(Me.Text)
            Exit Sub
        End If
        AppActivate(Me.Text)
    End Sub

    Private Sub tsbtnWipeOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnWipeOut.Click
        If SP.frmEmailComments.UnexpectedResults() = True Then
            AppActivate(Me.Text)
            Exit Sub
        End If
        AppActivate(Me.Text)
    End Sub

    Private Sub tsbtnAskDUDE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnAskDUDE.Click
        Try
            AppActivate("Maui DUDE - Ask DUDE")
        Catch
            SP.DisplayAskDude()
        End Try
    End Sub

    Private Sub tsbtn411_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn411.Click
        If SP.BorrInfo.Visible = True Then
            SP.BorrInfo.Activate()
        Else
            SP.BorrInfo.Show(LMBor.SSN, LMBor.Name)
        End If
    End Sub

    Private Sub tsbtnActHist30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnActHist30.Click
        RunActivityHistory(30)
    End Sub

    Private Sub tsbtnActHist90_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnActHist90.Click
        RunActivityHistory(90)
    End Sub

    Private Sub tsbtnActHist180_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnActHist180.Click
        RunActivityHistory(180)
    End Sub

    Private Sub tsbtnActHistAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnActHistAll.Click
        RunActivityHistory(0)
    End Sub

    Private Sub tsbtnCheckByPhone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnCheckByPhone.Click
        RunDotNETDll(Scripts("OneLink Check by Phone"), LMBor, 1)
        'RunScriptExternalScript(Scripts("OneLink Check by Phone"), LMBor.SSN)
    End Sub

    Private Sub tsbtnPaymentArrangements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnPaymentArrangements.Click
        RunDotNETDll(Scripts("Payment Arrangements"), LMBor, 1)
        'RunScriptExternalScript(Scripts("Payment Arrangements"), LMBor.SSN)
    End Sub

    Private Sub tsbtnUpdateDemos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnUpdateDemos.Click
        ShowDemoFrmAndUpdateHPWithResults()
    End Sub

    Private Sub tsbtnSinceOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnSinceOpen.Click
        Dim DaysSinceAccountOpen As Integer = Today.Subtract(CDate(LMBor.AccountOpenDate)).Days 'figure out how many days the account has been open
        RunActivityHistory(DaysSinceAccountOpen)
    End Sub

    Private Sub tsbtnOutstandingQueues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnOutstandingQueues.Click
        Queues = New frmQueues()
        Queues.Show(LMBor.GetOpenQueueTasksFrmSys())
    End Sub

    Private Sub tsbtnLoanDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnLoanDetail.Click
        Try
            AppActivate("Maui DUDE Loan Detail")
        Catch
            LDetail = New frmLoanDetail(LMBor)
            LDetail.Show()
        End Try
    End Sub
#End Region

#Region "Home Made Event Handlers"

    'EventHandler4MenuItems must be overridden
    Public Overrides Sub EventHandler4MenuItems(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If CType(sender, SP.ScriptAndServiceMenuItem).gsData.Item("InternalOrExternal") = "External" Then
            RunScriptExternalScript(sender, LMBor.SSN)
        ElseIf CType(sender, SP.ScriptAndServiceMenuItem).gsData.Item("InternalOrExternal") = "Internal" Then
            If CType(sender, SP.ScriptAndServiceMenuItem).gsData.Item("DisplayName") = "30 Days" Or _
                CType(sender, SP.ScriptAndServiceMenuItem).gsData.Item("DisplayName") = "60 Days" Or _
                CType(sender, SP.ScriptAndServiceMenuItem).gsData.Item("DisplayName") = "90 Days" Or _
                CType(sender, SP.ScriptAndServiceMenuItem).gsData.Item("DisplayName") = "All" Then
                RunActivityHistory(CInt(CType(sender, SP.ScriptAndServiceMenuItem).gsData.Item("DataForFunctionCall")))
            End If
        ElseIf CType(sender, SP.ScriptAndServiceMenuItem).gsData.Item("InternalOrExternal") = ".NET DLL" Then
            RunDotNETDll(sender, LMBor, 1)
        End If
    End Sub

    'event handler for attempt form OK button clicks
    Public Sub OKReturnFromAttemptFrm()
        Me.Enabled = True
        GetAttemptInfoFrm.Hide()
        Me.tbActionCode.Text = GetAttemptInfoFrm.ActionCode
        If GetAttemptInfoFrm.DisplayDemoGraphics Then
            ShowDemoFrmAndUpdateHPWithResults() 'show demo screen
        End If
        If GetAttemptInfoFrm.RunThirdPartyAuthScript Then
            RunScriptExternalScript(Scripts("Letters Menu"), LMBor.SSN)
        End If
    End Sub

    'event handler for attempt form Cancel button clicks
    Public Sub CancelReturnFromAttemptFrm()
        Me.Enabled = True
    End Sub

    'event handler for reference detail form
    Public Sub ReturnFromReferenceDetail()
        lvReferences.Enabled = True
    End Sub

#End Region


    Private Sub tbIncomingCallSSN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbIncomingCallSSN.TextChanged

    End Sub
End Class