<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLMHomePage
    Inherits SP.frmGenericScriptAndServicesEnabled

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLMHomePage))
        Me.btnBackToBins = New System.Windows.Forms.Button
        Me.cbPreviousContacts = New System.Windows.Forms.ComboBox
        Me.tbIncomingCallSSN = New System.Windows.Forms.TextBox
        Me.TabControl = New System.Windows.Forms.TabControl
        Me.tabMain = New System.Windows.Forms.TabPage
        Me.gbCC = New System.Windows.Forms.GroupBox
        Me.tbCCCmts = New System.Windows.Forms.TextBox
        Me.tbCCLtrID = New System.Windows.Forms.TextBox
        Me.Label38 = New System.Windows.Forms.Label
        Me.Label39 = New System.Windows.Forms.Label
        Me.cbCCRea = New System.Windows.Forms.ComboBox
        Me.Label40 = New System.Windows.Forms.Label
        Me.Label41 = New System.Windows.Forms.Label
        Me.cbCCCat = New System.Windows.Forms.ComboBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.tbPayOffDate = New System.Windows.Forms.MaskedTextBox
        Me.Label37 = New System.Windows.Forms.Label
        Me.tbPayOffAmount = New System.Windows.Forms.TextBox
        Me.Label36 = New System.Windows.Forms.Label
        Me.gbRepaymentCalc = New System.Windows.Forms.GroupBox
        Me.tbRepay25Year = New System.Windows.Forms.TextBox
        Me.Label35 = New System.Windows.Forms.Label
        Me.tbRepay10Year = New System.Windows.Forms.TextBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.tbRepay7Year = New System.Windows.Forms.TextBox
        Me.Label31 = New System.Windows.Forms.Label
        Me.tbRepay5Year = New System.Windows.Forms.TextBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.tbRepay3Year = New System.Windows.Forms.TextBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.tbRepay30Day = New System.Windows.Forms.TextBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.Label42 = New System.Windows.Forms.Label
        Me.tbDPA = New System.Windows.Forms.TextBox
        Me.tbDueDate = New System.Windows.Forms.TextBox
        Me.tbMonthInt = New System.Windows.Forms.TextBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.tbMonthPayAmt = New System.Windows.Forms.TextBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.tbTotalAmtDue = New System.Windows.Forms.TextBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.tbCollectCost = New System.Windows.Forms.TextBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.tbCurrInt = New System.Windows.Forms.TextBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.tbCurrPrinc = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.tbActionCode = New System.Windows.Forms.TextBox
        Me.lblActionCode = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.cbActivityDesc = New System.Windows.Forms.ComboBox
        Me.Label33 = New System.Windows.Forms.Label
        Me.cbContactDesc = New System.Windows.Forms.ComboBox
        Me.tbActivityCode = New System.Windows.Forms.TextBox
        Me.tbContactCode = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.lvReferences = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.tbActivityComments = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.gbBorrowerInfo = New System.Windows.Forms.GroupBox
        Me.btnContact = New System.Windows.Forms.Button
        Me.btnAttempt = New System.Windows.Forms.Button
        Me.tbDemoVerified = New System.Windows.Forms.TextBox
        Me.tbCurrentEmployer = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.tbDtLastContact = New System.Windows.Forms.MaskedTextBox
        Me.tbDtLastAttempt = New System.Windows.Forms.MaskedTextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.tbEmail = New System.Windows.Forms.TextBox
        Me.tbZIP = New System.Windows.Forms.MaskedTextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.tbAPExt = New System.Windows.Forms.TextBox
        Me.tbAltPhn = New System.Windows.Forms.MaskedTextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.tbHPExt = New System.Windows.Forms.TextBox
        Me.tbHomePhn = New System.Windows.Forms.MaskedTextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.tbCMSSN = New System.Windows.Forms.MaskedTextBox
        Me.tbSSN = New System.Windows.Forms.MaskedTextBox
        Me.tbState = New System.Windows.Forms.TextBox
        Me.tbCity = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.tbAddr2 = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.tbAddr1 = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.tbCMName = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.tbQueueText = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.tbQueue = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.tbBin = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.tbTaskLeftInBin = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tabForwarding = New System.Windows.Forms.TabPage
        Me.lstCallF = New System.Windows.Forms.ListBox
        Me.tabLegal = New System.Windows.Forms.TabPage
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.tb1098Dt = New System.Windows.Forms.TextBox
        Me.tb1098 = New System.Windows.Forms.TextBox
        Me.tbYearsOffset = New System.Windows.Forms.TextBox
        Me.tbTaxOffsetAmout = New System.Windows.Forms.TextBox
        Me.tbCertified = New System.Windows.Forms.TextBox
        Me.Label43 = New System.Windows.Forms.Label
        Me.tbTaxOffset = New System.Windows.Forms.TextBox
        Me.Label79 = New System.Windows.Forms.Label
        Me.Label81 = New System.Windows.Forms.Label
        Me.Label83 = New System.Windows.Forms.Label
        Me.Label82 = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.tbAJOutstandingDt = New System.Windows.Forms.TextBox
        Me.tbAJOutstanding = New System.Windows.Forms.TextBox
        Me.Label76 = New System.Windows.Forms.Label
        Me.lblPrimary2 = New System.Windows.Forms.Label
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.tbDay30Notice = New System.Windows.Forms.TextBox
        Me.tbPrimaryAction = New System.Windows.Forms.TextBox
        Me.Label44 = New System.Windows.Forms.Label
        Me.tbGarnish = New System.Windows.Forms.TextBox
        Me.tbLastPaymentDt = New System.Windows.Forms.TextBox
        Me.tbLastPaymentAmount = New System.Windows.Forms.TextBox
        Me.Label70 = New System.Windows.Forms.Label
        Me.lblPrimary1 = New System.Windows.Forms.Label
        Me.Label65 = New System.Windows.Forms.Label
        Me.tabAcctInfo = New System.Windows.Forms.TabPage
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.tbRehabCode = New System.Windows.Forms.TextBox
        Me.Label63 = New System.Windows.Forms.Label
        Me.lstServicer = New System.Windows.Forms.ListBox
        Me.Label60 = New System.Windows.Forms.Label
        Me.Label59 = New System.Windows.Forms.Label
        Me.tbPendingPayments = New System.Windows.Forms.TextBox
        Me.tbLoanPg = New System.Windows.Forms.TextBox
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label56 = New System.Windows.Forms.Label
        Me.tbIneligibleForRehabCode = New System.Windows.Forms.TextBox
        Me.tbRehabCounter = New System.Windows.Forms.TextBox
        Me.tbReinstatementDate = New System.Windows.Forms.TextBox
        Me.lstLC41 = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.tbReinstatementEligibilityCode = New System.Windows.Forms.TextBox
        Me.tbCollectionInd = New System.Windows.Forms.TextBox
        Me.Label58 = New System.Windows.Forms.Label
        Me.Label86 = New System.Windows.Forms.Label
        Me.Label61 = New System.Windows.Forms.Label
        Me.Label62 = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.Label52 = New System.Windows.Forms.Label
        Me.tbTotalR = New System.Windows.Forms.TextBox
        Me.lblLine = New System.Windows.Forms.Label
        Me.tbTotalP = New System.Windows.Forms.TextBox
        Me.Label54 = New System.Windows.Forms.Label
        Me.tbTotalC = New System.Windows.Forms.TextBox
        Me.Label53 = New System.Windows.Forms.Label
        Me.tbProjectedR = New System.Windows.Forms.TextBox
        Me.Label51 = New System.Windows.Forms.Label
        Me.tbProjectedC = New System.Windows.Forms.TextBox
        Me.Label50 = New System.Windows.Forms.Label
        Me.tbCollectionR = New System.Windows.Forms.TextBox
        Me.Label49 = New System.Windows.Forms.Label
        Me.tbCollectionP = New System.Windows.Forms.TextBox
        Me.Label48 = New System.Windows.Forms.Label
        Me.tbCollectionC = New System.Windows.Forms.TextBox
        Me.Label47 = New System.Windows.Forms.Label
        Me.tbOtherR = New System.Windows.Forms.TextBox
        Me.Label46 = New System.Windows.Forms.Label
        Me.tbOtherP = New System.Windows.Forms.TextBox
        Me.Label45 = New System.Windows.Forms.Label
        Me.tbOtherC = New System.Windows.Forms.TextBox
        Me.tbPrincipalC = New System.Windows.Forms.TextBox
        Me.tbLegalR = New System.Windows.Forms.TextBox
        Me.tbPrincipalP = New System.Windows.Forms.TextBox
        Me.tbLegalP = New System.Windows.Forms.TextBox
        Me.tbPrincipalR = New System.Windows.Forms.TextBox
        Me.tbLegalC = New System.Windows.Forms.TextBox
        Me.tbInterestC = New System.Windows.Forms.TextBox
        Me.tbInterestR = New System.Windows.Forms.TextBox
        Me.tbInterestP = New System.Windows.Forms.TextBox
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsbtnPaymentArrangements = New System.Windows.Forms.ToolStripButton
        Me.tsbtnCheckByPhone = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbtnUpdateDemos = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbtnLoanDetail = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbtn411 = New System.Windows.Forms.ToolStripButton
        Me.tsbtnAskDUDE = New System.Windows.Forms.ToolStripButton
        Me.tsbtnWipeOut = New System.Windows.Forms.ToolStripButton
        Me.tsbtnBrightIdea = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbtnActHist30 = New System.Windows.Forms.ToolStripButton
        Me.tsbtnActHist90 = New System.Windows.Forms.ToolStripButton
        Me.tsbtnActHist180 = New System.Windows.Forms.ToolStripButton
        Me.tsbtnActHistAll = New System.Windows.Forms.ToolStripButton
        Me.tsbtnSinceOpen = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbtnOutstandingQueues = New System.Windows.Forms.ToolStripButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblAcctNum = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lbStatuses = New System.Windows.Forms.ListBox
        Me.btnMoStatus = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cbSpecialHandling = New System.Windows.Forms.CheckBox
        Me.cbVIP = New System.Windows.Forms.CheckBox
        Me.btnSaveAndCont = New System.Windows.Forms.Button
        Me.btnIncomingCall = New System.Windows.Forms.Button
        Me.ActCmt = New MDLMHome.ActivityCmts
        Me.TabControl.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.gbCC.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.gbRepaymentCalc.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.gbBorrowerInfo.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.tabForwarding.SuspendLayout()
        Me.tabLegal.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.tabAcctInfo.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBackToBins
        '
        Me.btnBackToBins.Location = New System.Drawing.Point(556, 779)
        Me.btnBackToBins.Name = "btnBackToBins"
        Me.btnBackToBins.Size = New System.Drawing.Size(161, 23)
        Me.btnBackToBins.TabIndex = 1
        Me.btnBackToBins.Text = "Back to Bins Screen"
        Me.btnBackToBins.UseVisualStyleBackColor = True
        '
        'cbPreviousContacts
        '
        Me.cbPreviousContacts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPreviousContacts.FormattingEnabled = True
        Me.cbPreviousContacts.Location = New System.Drawing.Point(908, 793)
        Me.cbPreviousContacts.Name = "cbPreviousContacts"
        Me.cbPreviousContacts.Size = New System.Drawing.Size(178, 21)
        Me.cbPreviousContacts.TabIndex = 3
        '
        'tbIncomingCallSSN
        '
        Me.tbIncomingCallSSN.Location = New System.Drawing.Point(908, 769)
        Me.tbIncomingCallSSN.Name = "tbIncomingCallSSN"
        Me.tbIncomingCallSSN.Size = New System.Drawing.Size(144, 20)
        Me.tbIncomingCallSSN.TabIndex = 4
        Me.tbIncomingCallSSN.Text = "Incoming Call SSN or Acct #"
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.tabMain)
        Me.TabControl.Controls.Add(Me.tabForwarding)
        Me.TabControl.Controls.Add(Me.tabLegal)
        Me.TabControl.Controls.Add(Me.tabAcctInfo)
        Me.TabControl.HotTrack = True
        Me.TabControl.Location = New System.Drawing.Point(2, 139)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(1087, 623)
        Me.TabControl.TabIndex = 6
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.gbCC)
        Me.tabMain.Controls.Add(Me.GroupBox6)
        Me.tabMain.Controls.Add(Me.gbRepaymentCalc)
        Me.tabMain.Controls.Add(Me.GroupBox5)
        Me.tabMain.Controls.Add(Me.GroupBox4)
        Me.tabMain.Controls.Add(Me.GroupBox3)
        Me.tabMain.Controls.Add(Me.tbActivityComments)
        Me.tabMain.Controls.Add(Me.Label20)
        Me.tabMain.Controls.Add(Me.gbBorrowerInfo)
        Me.tabMain.Controls.Add(Me.Label19)
        Me.tabMain.Controls.Add(Me.GroupBox2)
        Me.tabMain.Controls.Add(Me.ActCmt)
        Me.tabMain.Location = New System.Drawing.Point(4, 22)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMain.Size = New System.Drawing.Size(1079, 597)
        Me.tabMain.TabIndex = 0
        Me.tabMain.Text = "Main"
        Me.tabMain.UseVisualStyleBackColor = True
        '
        'gbCC
        '
        Me.gbCC.Controls.Add(Me.tbCCCmts)
        Me.gbCC.Controls.Add(Me.tbCCLtrID)
        Me.gbCC.Controls.Add(Me.Label38)
        Me.gbCC.Controls.Add(Me.Label39)
        Me.gbCC.Controls.Add(Me.cbCCRea)
        Me.gbCC.Controls.Add(Me.Label40)
        Me.gbCC.Controls.Add(Me.Label41)
        Me.gbCC.Controls.Add(Me.cbCCCat)
        Me.gbCC.Location = New System.Drawing.Point(738, 442)
        Me.gbCC.Name = "gbCC"
        Me.gbCC.Size = New System.Drawing.Size(335, 149)
        Me.gbCC.TabIndex = 35
        Me.gbCC.TabStop = False
        Me.gbCC.Text = "Call Categorization"
        '
        'tbCCCmts
        '
        Me.tbCCCmts.Enabled = False
        Me.tbCCCmts.Location = New System.Drawing.Point(158, 84)
        Me.tbCCCmts.MaxLength = 30
        Me.tbCCCmts.Multiline = True
        Me.tbCCCmts.Name = "tbCCCmts"
        Me.tbCCCmts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbCCCmts.Size = New System.Drawing.Size(171, 59)
        Me.tbCCCmts.TabIndex = 7
        '
        'tbCCLtrID
        '
        Me.tbCCLtrID.Enabled = False
        Me.tbCCLtrID.Location = New System.Drawing.Point(158, 63)
        Me.tbCCLtrID.MaxLength = 10
        Me.tbCCLtrID.Name = "tbCCLtrID"
        Me.tbCCLtrID.Size = New System.Drawing.Size(171, 20)
        Me.tbCCLtrID.TabIndex = 6
        '
        'Label38
        '
        Me.Label38.Location = New System.Drawing.Point(8, 87)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(64, 16)
        Me.Label38.TabIndex = 5
        Me.Label38.Text = "Comments:"
        '
        'Label39
        '
        Me.Label39.Location = New System.Drawing.Point(8, 67)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(64, 16)
        Me.Label39.TabIndex = 4
        Me.Label39.Text = "Letter ID:"
        '
        'cbCCRea
        '
        Me.cbCCRea.Enabled = False
        Me.cbCCRea.Location = New System.Drawing.Point(158, 41)
        Me.cbCCRea.Name = "cbCCRea"
        Me.cbCCRea.Size = New System.Drawing.Size(171, 21)
        Me.cbCCRea.TabIndex = 3
        '
        'Label40
        '
        Me.Label40.Location = New System.Drawing.Point(8, 44)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(64, 16)
        Me.Label40.TabIndex = 2
        Me.Label40.Text = "Reason:"
        '
        'Label41
        '
        Me.Label41.Location = New System.Drawing.Point(8, 23)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(64, 16)
        Me.Label41.TabIndex = 1
        Me.Label41.Text = "Category:"
        '
        'cbCCCat
        '
        Me.cbCCCat.Location = New System.Drawing.Point(158, 19)
        Me.cbCCCat.Name = "cbCCCat"
        Me.cbCCCat.Size = New System.Drawing.Size(171, 21)
        Me.cbCCCat.TabIndex = 0
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.tbPayOffDate)
        Me.GroupBox6.Controls.Add(Me.Label37)
        Me.GroupBox6.Controls.Add(Me.tbPayOffAmount)
        Me.GroupBox6.Controls.Add(Me.Label36)
        Me.GroupBox6.Location = New System.Drawing.Point(738, 369)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(335, 68)
        Me.GroupBox6.TabIndex = 16
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Paid In Full Amount:"
        '
        'tbPayOffDate
        '
        Me.tbPayOffDate.Location = New System.Drawing.Point(156, 42)
        Me.tbPayOffDate.Mask = "00/00/0000"
        Me.tbPayOffDate.Name = "tbPayOffDate"
        Me.tbPayOffDate.Size = New System.Drawing.Size(93, 20)
        Me.tbPayOffDate.TabIndex = 27
        Me.tbPayOffDate.ValidatingType = GetType(Date)
        '
        'Label37
        '
        Me.Label37.Location = New System.Drawing.Point(6, 46)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(113, 20)
        Me.Label37.TabIndex = 4
        Me.Label37.Text = "Pay Off Date:"
        '
        'tbPayOffAmount
        '
        Me.tbPayOffAmount.Location = New System.Drawing.Point(157, 19)
        Me.tbPayOffAmount.Name = "tbPayOffAmount"
        Me.tbPayOffAmount.ReadOnly = True
        Me.tbPayOffAmount.Size = New System.Drawing.Size(172, 20)
        Me.tbPayOffAmount.TabIndex = 3
        '
        'Label36
        '
        Me.Label36.Location = New System.Drawing.Point(6, 22)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(113, 20)
        Me.Label36.TabIndex = 2
        Me.Label36.Text = "Pay Off Amount:"
        '
        'gbRepaymentCalc
        '
        Me.gbRepaymentCalc.Controls.Add(Me.tbRepay25Year)
        Me.gbRepaymentCalc.Controls.Add(Me.Label35)
        Me.gbRepaymentCalc.Controls.Add(Me.tbRepay10Year)
        Me.gbRepaymentCalc.Controls.Add(Me.Label34)
        Me.gbRepaymentCalc.Controls.Add(Me.tbRepay7Year)
        Me.gbRepaymentCalc.Controls.Add(Me.Label31)
        Me.gbRepaymentCalc.Controls.Add(Me.tbRepay5Year)
        Me.gbRepaymentCalc.Controls.Add(Me.Label30)
        Me.gbRepaymentCalc.Controls.Add(Me.tbRepay3Year)
        Me.gbRepaymentCalc.Controls.Add(Me.Label29)
        Me.gbRepaymentCalc.Controls.Add(Me.tbRepay30Day)
        Me.gbRepaymentCalc.Controls.Add(Me.Label28)
        Me.gbRepaymentCalc.Location = New System.Drawing.Point(380, 416)
        Me.gbRepaymentCalc.Name = "gbRepaymentCalc"
        Me.gbRepaymentCalc.Size = New System.Drawing.Size(350, 175)
        Me.gbRepaymentCalc.TabIndex = 15
        Me.gbRepaymentCalc.TabStop = False
        Me.gbRepaymentCalc.Text = "Repayment Calculations:"
        '
        'tbRepay25Year
        '
        Me.tbRepay25Year.Location = New System.Drawing.Point(168, 129)
        Me.tbRepay25Year.Name = "tbRepay25Year"
        Me.tbRepay25Year.ReadOnly = True
        Me.tbRepay25Year.Size = New System.Drawing.Size(173, 20)
        Me.tbRepay25Year.TabIndex = 13
        '
        'Label35
        '
        Me.Label35.Location = New System.Drawing.Point(8, 132)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(100, 17)
        Me.Label35.TabIndex = 12
        Me.Label35.Text = "25 Year:"
        '
        'tbRepay10Year
        '
        Me.tbRepay10Year.Location = New System.Drawing.Point(168, 107)
        Me.tbRepay10Year.Name = "tbRepay10Year"
        Me.tbRepay10Year.ReadOnly = True
        Me.tbRepay10Year.Size = New System.Drawing.Size(173, 20)
        Me.tbRepay10Year.TabIndex = 11
        '
        'Label34
        '
        Me.Label34.Location = New System.Drawing.Point(8, 110)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(100, 17)
        Me.Label34.TabIndex = 10
        Me.Label34.Text = "10 Year:"
        '
        'tbRepay7Year
        '
        Me.tbRepay7Year.Location = New System.Drawing.Point(168, 85)
        Me.tbRepay7Year.Name = "tbRepay7Year"
        Me.tbRepay7Year.ReadOnly = True
        Me.tbRepay7Year.Size = New System.Drawing.Size(173, 20)
        Me.tbRepay7Year.TabIndex = 9
        '
        'Label31
        '
        Me.Label31.Location = New System.Drawing.Point(8, 88)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(100, 17)
        Me.Label31.TabIndex = 8
        Me.Label31.Text = "7 Year:"
        '
        'tbRepay5Year
        '
        Me.tbRepay5Year.Location = New System.Drawing.Point(168, 63)
        Me.tbRepay5Year.Name = "tbRepay5Year"
        Me.tbRepay5Year.ReadOnly = True
        Me.tbRepay5Year.Size = New System.Drawing.Size(173, 20)
        Me.tbRepay5Year.TabIndex = 7
        '
        'Label30
        '
        Me.Label30.Location = New System.Drawing.Point(8, 66)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(100, 17)
        Me.Label30.TabIndex = 6
        Me.Label30.Text = "5 Year:"
        '
        'tbRepay3Year
        '
        Me.tbRepay3Year.Location = New System.Drawing.Point(168, 41)
        Me.tbRepay3Year.Name = "tbRepay3Year"
        Me.tbRepay3Year.ReadOnly = True
        Me.tbRepay3Year.Size = New System.Drawing.Size(173, 20)
        Me.tbRepay3Year.TabIndex = 5
        '
        'Label29
        '
        Me.Label29.Location = New System.Drawing.Point(8, 44)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(100, 17)
        Me.Label29.TabIndex = 4
        Me.Label29.Text = "3 Year:"
        '
        'tbRepay30Day
        '
        Me.tbRepay30Day.Location = New System.Drawing.Point(168, 19)
        Me.tbRepay30Day.Name = "tbRepay30Day"
        Me.tbRepay30Day.ReadOnly = True
        Me.tbRepay30Day.Size = New System.Drawing.Size(173, 20)
        Me.tbRepay30Day.TabIndex = 3
        '
        'Label28
        '
        Me.Label28.Location = New System.Drawing.Point(8, 22)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(100, 17)
        Me.Label28.TabIndex = 2
        Me.Label28.Text = "30 Day:"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.Label42)
        Me.GroupBox5.Controls.Add(Me.tbDPA)
        Me.GroupBox5.Controls.Add(Me.tbDueDate)
        Me.GroupBox5.Controls.Add(Me.tbMonthInt)
        Me.GroupBox5.Controls.Add(Me.Label27)
        Me.GroupBox5.Controls.Add(Me.tbMonthPayAmt)
        Me.GroupBox5.Controls.Add(Me.Label26)
        Me.GroupBox5.Controls.Add(Me.Label25)
        Me.GroupBox5.Controls.Add(Me.tbTotalAmtDue)
        Me.GroupBox5.Controls.Add(Me.Label24)
        Me.GroupBox5.Controls.Add(Me.tbCollectCost)
        Me.GroupBox5.Controls.Add(Me.Label23)
        Me.GroupBox5.Controls.Add(Me.tbCurrInt)
        Me.GroupBox5.Controls.Add(Me.Label22)
        Me.GroupBox5.Controls.Add(Me.tbCurrPrinc)
        Me.GroupBox5.Controls.Add(Me.Label21)
        Me.GroupBox5.Location = New System.Drawing.Point(380, 216)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(350, 195)
        Me.GroupBox5.TabIndex = 14
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Payment Information:"
        '
        'Label42
        '
        Me.Label42.Location = New System.Drawing.Point(8, 173)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(136, 18)
        Me.Label42.TabIndex = 33
        Me.Label42.Text = "Direct Payment:"
        '
        'tbDPA
        '
        Me.tbDPA.Location = New System.Drawing.Point(168, 170)
        Me.tbDPA.Name = "tbDPA"
        Me.tbDPA.ReadOnly = True
        Me.tbDPA.Size = New System.Drawing.Size(93, 20)
        Me.tbDPA.TabIndex = 32
        '
        'tbDueDate
        '
        Me.tbDueDate.Location = New System.Drawing.Point(168, 106)
        Me.tbDueDate.Name = "tbDueDate"
        Me.tbDueDate.ReadOnly = True
        Me.tbDueDate.Size = New System.Drawing.Size(93, 20)
        Me.tbDueDate.TabIndex = 15
        '
        'tbMonthInt
        '
        Me.tbMonthInt.Location = New System.Drawing.Point(168, 148)
        Me.tbMonthInt.Name = "tbMonthInt"
        Me.tbMonthInt.ReadOnly = True
        Me.tbMonthInt.Size = New System.Drawing.Size(173, 20)
        Me.tbMonthInt.TabIndex = 31
        '
        'Label27
        '
        Me.Label27.Location = New System.Drawing.Point(8, 152)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(154, 21)
        Me.Label27.TabIndex = 30
        Me.Label27.Text = "Monthly Interest:"
        '
        'tbMonthPayAmt
        '
        Me.tbMonthPayAmt.Location = New System.Drawing.Point(168, 127)
        Me.tbMonthPayAmt.Name = "tbMonthPayAmt"
        Me.tbMonthPayAmt.ReadOnly = True
        Me.tbMonthPayAmt.Size = New System.Drawing.Size(173, 20)
        Me.tbMonthPayAmt.TabIndex = 29
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(8, 131)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(154, 21)
        Me.Label26.TabIndex = 28
        Me.Label26.Text = "Monthly Payment Amount:"
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(8, 110)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(136, 18)
        Me.Label25.TabIndex = 26
        Me.Label25.Text = "Due Date:"
        '
        'tbTotalAmtDue
        '
        Me.tbTotalAmtDue.Location = New System.Drawing.Point(168, 85)
        Me.tbTotalAmtDue.Name = "tbTotalAmtDue"
        Me.tbTotalAmtDue.ReadOnly = True
        Me.tbTotalAmtDue.Size = New System.Drawing.Size(173, 20)
        Me.tbTotalAmtDue.TabIndex = 7
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(8, 89)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(100, 21)
        Me.Label24.TabIndex = 6
        Me.Label24.Text = "Total Amount Due:"
        '
        'tbCollectCost
        '
        Me.tbCollectCost.Location = New System.Drawing.Point(168, 64)
        Me.tbCollectCost.Name = "tbCollectCost"
        Me.tbCollectCost.ReadOnly = True
        Me.tbCollectCost.Size = New System.Drawing.Size(173, 20)
        Me.tbCollectCost.TabIndex = 5
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(8, 68)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(100, 21)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "Collection Costs:"
        '
        'tbCurrInt
        '
        Me.tbCurrInt.Location = New System.Drawing.Point(168, 42)
        Me.tbCurrInt.Name = "tbCurrInt"
        Me.tbCurrInt.ReadOnly = True
        Me.tbCurrInt.Size = New System.Drawing.Size(173, 20)
        Me.tbCurrInt.TabIndex = 3
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(8, 46)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(100, 21)
        Me.Label22.TabIndex = 2
        Me.Label22.Text = "Current Interest:"
        '
        'tbCurrPrinc
        '
        Me.tbCurrPrinc.Location = New System.Drawing.Point(168, 20)
        Me.tbCurrPrinc.Name = "tbCurrPrinc"
        Me.tbCurrPrinc.ReadOnly = True
        Me.tbCurrPrinc.Size = New System.Drawing.Size(173, 20)
        Me.tbCurrPrinc.TabIndex = 1
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(8, 25)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(100, 21)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Current Principal:"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.tbActionCode)
        Me.GroupBox4.Controls.Add(Me.lblActionCode)
        Me.GroupBox4.Controls.Add(Me.Label32)
        Me.GroupBox4.Controls.Add(Me.cbActivityDesc)
        Me.GroupBox4.Controls.Add(Me.Label33)
        Me.GroupBox4.Controls.Add(Me.cbContactDesc)
        Me.GroupBox4.Controls.Add(Me.tbActivityCode)
        Me.GroupBox4.Controls.Add(Me.tbContactCode)
        Me.GroupBox4.Location = New System.Drawing.Point(8, 5)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(366, 95)
        Me.GroupBox4.TabIndex = 13
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Activity/Contact/Action Codes:"
        '
        'tbActionCode
        '
        Me.tbActionCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.tbActionCode.Location = New System.Drawing.Point(130, 68)
        Me.tbActionCode.MaxLength = 5
        Me.tbActionCode.Name = "tbActionCode"
        Me.tbActionCode.Size = New System.Drawing.Size(93, 20)
        Me.tbActionCode.TabIndex = 13
        '
        'lblActionCode
        '
        Me.lblActionCode.Location = New System.Drawing.Point(6, 66)
        Me.lblActionCode.Name = "lblActionCode"
        Me.lblActionCode.Size = New System.Drawing.Size(80, 23)
        Me.lblActionCode.TabIndex = 12
        Me.lblActionCode.Text = "Action Code:"
        Me.lblActionCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label32
        '
        Me.Label32.Location = New System.Drawing.Point(6, 19)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(80, 23)
        Me.Label32.TabIndex = 10
        Me.Label32.Text = "Activity Code:"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbActivityDesc
        '
        Me.cbActivityDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbActivityDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbActivityDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbActivityDesc.ItemHeight = 13
        Me.cbActivityDesc.Items.AddRange(New Object() {"", "Account Maintenance", "Court Document", "Claim", "Computer Letter", "Electronic Document", "E-mail", "Electronic Transmission", "Fax", "Form", "Letter", "Miscellaneous", "Preclaim", "Reference Contact Helpful (Home#)", "Reference Attempt (Home#)", "Reference Contact Not Helpful (Home#)", "Reference Do Not Contact (Home#)", "Reference Contact Helpful (Alt#)", "Reference Attempt (Alt#)", "Reference Contact Not Helpful (Alt#)", "Reference Do Not Contact (Alt#)", "Tape", "Telephone Contact", "Telephone Call", "Telephone Attempt", "Office Visit"})
        Me.cbActivityDesc.Location = New System.Drawing.Point(157, 20)
        Me.cbActivityDesc.Name = "cbActivityDesc"
        Me.cbActivityDesc.Size = New System.Drawing.Size(201, 21)
        Me.cbActivityDesc.TabIndex = 7
        '
        'Label33
        '
        Me.Label33.Location = New System.Drawing.Point(6, 42)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(80, 23)
        Me.Label33.TabIndex = 11
        Me.Label33.Text = "Contact Code:"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbContactDesc
        '
        Me.cbContactDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbContactDesc.ItemHeight = 13
        Me.cbContactDesc.Items.AddRange(New Object() {"", "To: Attorney", "From: Attorney", "To: Borrower", "From: Borrower", "To: Comaker", "From: Comaker", "To: Credit Bureau", "From: Credit Bureau", "To: DMV", "From: DMV", "To: Employer", "From: Employer", "To: Endorser", "From: Endorser", "To: Family", "From: Family", "To: Guarantor", "From: Guarantor", "To: Lender", "From: Lender", "To: Miscellaneous", "From: Miscellaneous", "To: Post Office", "From: Post Office", "To: Prison", "From: Prison", "To: Reference", "From: Reference", "To: School", "From: School", "To: UHEAA Staff", "From: UHEAA Staff", "To 3rd Party", "From 3rd Party"})
        Me.cbContactDesc.Location = New System.Drawing.Point(157, 44)
        Me.cbContactDesc.Name = "cbContactDesc"
        Me.cbContactDesc.Size = New System.Drawing.Size(201, 21)
        Me.cbContactDesc.TabIndex = 8
        '
        'tbActivityCode
        '
        Me.tbActivityCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.tbActivityCode.Location = New System.Drawing.Point(130, 21)
        Me.tbActivityCode.MaxLength = 2
        Me.tbActivityCode.Name = "tbActivityCode"
        Me.tbActivityCode.Size = New System.Drawing.Size(24, 20)
        Me.tbActivityCode.TabIndex = 6
        '
        'tbContactCode
        '
        Me.tbContactCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.tbContactCode.Location = New System.Drawing.Point(130, 45)
        Me.tbContactCode.Name = "tbContactCode"
        Me.tbContactCode.Size = New System.Drawing.Size(24, 20)
        Me.tbContactCode.TabIndex = 9
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lvReferences)
        Me.GroupBox3.Location = New System.Drawing.Point(8, 431)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(366, 160)
        Me.GroupBox3.TabIndex = 12
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "References:"
        '
        'lvReferences
        '
        Me.lvReferences.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader7, Me.ColumnHeader2})
        Me.lvReferences.FullRowSelect = True
        Me.lvReferences.Location = New System.Drawing.Point(9, 19)
        Me.lvReferences.MultiSelect = False
        Me.lvReferences.Name = "lvReferences"
        Me.lvReferences.Size = New System.Drawing.Size(349, 132)
        Me.lvReferences.TabIndex = 0
        Me.lvReferences.UseCompatibleStateImageBehavior = False
        Me.lvReferences.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 181
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Thrd Prty Auth"
        Me.ColumnHeader7.Width = 81
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Reference ID"
        Me.ColumnHeader2.Width = 83
        '
        'tbActivityComments
        '
        Me.tbActivityComments.Location = New System.Drawing.Point(394, 132)
        Me.tbActivityComments.MaxLength = 600
        Me.tbActivityComments.Multiline = True
        Me.tbActivityComments.Name = "tbActivityComments"
        Me.tbActivityComments.Size = New System.Drawing.Size(677, 75)
        Me.tbActivityComments.TabIndex = 3
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(380, 115)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(125, 14)
        Me.Label20.TabIndex = 2
        Me.Label20.Text = "Add Comments:"
        '
        'gbBorrowerInfo
        '
        Me.gbBorrowerInfo.Controls.Add(Me.btnContact)
        Me.gbBorrowerInfo.Controls.Add(Me.btnAttempt)
        Me.gbBorrowerInfo.Controls.Add(Me.tbDemoVerified)
        Me.gbBorrowerInfo.Controls.Add(Me.tbCurrentEmployer)
        Me.gbBorrowerInfo.Controls.Add(Me.Label18)
        Me.gbBorrowerInfo.Controls.Add(Me.tbDtLastContact)
        Me.gbBorrowerInfo.Controls.Add(Me.tbDtLastAttempt)
        Me.gbBorrowerInfo.Controls.Add(Me.Label17)
        Me.gbBorrowerInfo.Controls.Add(Me.tbEmail)
        Me.gbBorrowerInfo.Controls.Add(Me.tbZIP)
        Me.gbBorrowerInfo.Controls.Add(Me.Label16)
        Me.gbBorrowerInfo.Controls.Add(Me.tbAPExt)
        Me.gbBorrowerInfo.Controls.Add(Me.tbAltPhn)
        Me.gbBorrowerInfo.Controls.Add(Me.Label15)
        Me.gbBorrowerInfo.Controls.Add(Me.tbHPExt)
        Me.gbBorrowerInfo.Controls.Add(Me.tbHomePhn)
        Me.gbBorrowerInfo.Controls.Add(Me.Label14)
        Me.gbBorrowerInfo.Controls.Add(Me.tbCMSSN)
        Me.gbBorrowerInfo.Controls.Add(Me.tbSSN)
        Me.gbBorrowerInfo.Controls.Add(Me.tbState)
        Me.gbBorrowerInfo.Controls.Add(Me.tbCity)
        Me.gbBorrowerInfo.Controls.Add(Me.Label13)
        Me.gbBorrowerInfo.Controls.Add(Me.tbAddr2)
        Me.gbBorrowerInfo.Controls.Add(Me.Label12)
        Me.gbBorrowerInfo.Controls.Add(Me.tbAddr1)
        Me.gbBorrowerInfo.Controls.Add(Me.Label11)
        Me.gbBorrowerInfo.Controls.Add(Me.Label10)
        Me.gbBorrowerInfo.Controls.Add(Me.tbCMName)
        Me.gbBorrowerInfo.Controls.Add(Me.Label9)
        Me.gbBorrowerInfo.Controls.Add(Me.Label8)
        Me.gbBorrowerInfo.Location = New System.Drawing.Point(8, 106)
        Me.gbBorrowerInfo.Name = "gbBorrowerInfo"
        Me.gbBorrowerInfo.Size = New System.Drawing.Size(366, 319)
        Me.gbBorrowerInfo.TabIndex = 2
        Me.gbBorrowerInfo.TabStop = False
        Me.gbBorrowerInfo.Text = "Borrower Information:"
        '
        'btnContact
        '
        Me.btnContact.Location = New System.Drawing.Point(202, 287)
        Me.btnContact.Name = "btnContact"
        Me.btnContact.Size = New System.Drawing.Size(75, 23)
        Me.btnContact.TabIndex = 31
        Me.btnContact.Text = "Contact"
        Me.btnContact.UseVisualStyleBackColor = True
        '
        'btnAttempt
        '
        Me.btnAttempt.Location = New System.Drawing.Point(92, 287)
        Me.btnAttempt.Name = "btnAttempt"
        Me.btnAttempt.Size = New System.Drawing.Size(75, 23)
        Me.btnAttempt.TabIndex = 30
        Me.btnAttempt.Text = "Attempt"
        Me.btnAttempt.UseVisualStyleBackColor = True
        '
        'tbDemoVerified
        '
        Me.tbDemoVerified.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbDemoVerified.Location = New System.Drawing.Point(9, 260)
        Me.tbDemoVerified.Name = "tbDemoVerified"
        Me.tbDemoVerified.ReadOnly = True
        Me.tbDemoVerified.Size = New System.Drawing.Size(349, 21)
        Me.tbDemoVerified.TabIndex = 29
        Me.tbDemoVerified.Text = "VERIFIED"
        Me.tbDemoVerified.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbCurrentEmployer
        '
        Me.tbCurrentEmployer.Location = New System.Drawing.Point(130, 238)
        Me.tbCurrentEmployer.Name = "tbCurrentEmployer"
        Me.tbCurrentEmployer.ReadOnly = True
        Me.tbCurrentEmployer.Size = New System.Drawing.Size(228, 20)
        Me.tbCurrentEmployer.TabIndex = 28
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(6, 241)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(100, 18)
        Me.Label18.TabIndex = 27
        Me.Label18.Text = "Current Employer:"
        '
        'tbDtLastContact
        '
        Me.tbDtLastContact.Location = New System.Drawing.Point(225, 216)
        Me.tbDtLastContact.Mask = "00/00/0000"
        Me.tbDtLastContact.Name = "tbDtLastContact"
        Me.tbDtLastContact.ReadOnly = True
        Me.tbDtLastContact.Size = New System.Drawing.Size(93, 20)
        Me.tbDtLastContact.TabIndex = 26
        Me.tbDtLastContact.ValidatingType = GetType(Date)
        '
        'tbDtLastAttempt
        '
        Me.tbDtLastAttempt.Location = New System.Drawing.Point(130, 216)
        Me.tbDtLastAttempt.Mask = "00/00/0000"
        Me.tbDtLastAttempt.Name = "tbDtLastAttempt"
        Me.tbDtLastAttempt.ReadOnly = True
        Me.tbDtLastAttempt.Size = New System.Drawing.Size(93, 20)
        Me.tbDtLastAttempt.TabIndex = 25
        Me.tbDtLastAttempt.ValidatingType = GetType(Date)
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(6, 221)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(118, 18)
        Me.Label17.TabIndex = 24
        Me.Label17.Text = "Date Last Atmpt/Cntct:"
        '
        'tbEmail
        '
        Me.tbEmail.Location = New System.Drawing.Point(130, 194)
        Me.tbEmail.Name = "tbEmail"
        Me.tbEmail.ReadOnly = True
        Me.tbEmail.Size = New System.Drawing.Size(228, 20)
        Me.tbEmail.TabIndex = 23
        '
        'tbZIP
        '
        Me.tbZIP.Location = New System.Drawing.Point(297, 128)
        Me.tbZIP.Mask = "00000-9999"
        Me.tbZIP.Name = "tbZIP"
        Me.tbZIP.ReadOnly = True
        Me.tbZIP.Size = New System.Drawing.Size(61, 20)
        Me.tbZIP.TabIndex = 15
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(6, 199)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(100, 18)
        Me.Label16.TabIndex = 22
        Me.Label16.Text = "Email:"
        '
        'tbAPExt
        '
        Me.tbAPExt.Location = New System.Drawing.Point(268, 172)
        Me.tbAPExt.Name = "tbAPExt"
        Me.tbAPExt.ReadOnly = True
        Me.tbAPExt.Size = New System.Drawing.Size(26, 20)
        Me.tbAPExt.TabIndex = 21
        '
        'tbAltPhn
        '
        Me.tbAltPhn.Location = New System.Drawing.Point(130, 172)
        Me.tbAltPhn.Mask = "(999) 000-0000"
        Me.tbAltPhn.Name = "tbAltPhn"
        Me.tbAltPhn.ReadOnly = True
        Me.tbAltPhn.Size = New System.Drawing.Size(136, 20)
        Me.tbAltPhn.TabIndex = 20
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(6, 175)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(100, 18)
        Me.Label15.TabIndex = 19
        Me.Label15.Text = "Alt Phone#/Ext:"
        '
        'tbHPExt
        '
        Me.tbHPExt.Location = New System.Drawing.Point(268, 150)
        Me.tbHPExt.Name = "tbHPExt"
        Me.tbHPExt.ReadOnly = True
        Me.tbHPExt.Size = New System.Drawing.Size(26, 20)
        Me.tbHPExt.TabIndex = 18
        '
        'tbHomePhn
        '
        Me.tbHomePhn.Location = New System.Drawing.Point(130, 150)
        Me.tbHomePhn.Mask = "(999) 000-0000"
        Me.tbHomePhn.Name = "tbHomePhn"
        Me.tbHomePhn.ReadOnly = True
        Me.tbHomePhn.Size = New System.Drawing.Size(136, 20)
        Me.tbHomePhn.TabIndex = 17
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(6, 153)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(100, 18)
        Me.Label14.TabIndex = 16
        Me.Label14.Text = "Home Phone#/Ext:"
        '
        'tbCMSSN
        '
        Me.tbCMSSN.Location = New System.Drawing.Point(130, 62)
        Me.tbCMSSN.Mask = "000-00-0000"
        Me.tbCMSSN.Name = "tbCMSSN"
        Me.tbCMSSN.ReadOnly = True
        Me.tbCMSSN.Size = New System.Drawing.Size(93, 20)
        Me.tbCMSSN.TabIndex = 14
        '
        'tbSSN
        '
        Me.tbSSN.Location = New System.Drawing.Point(130, 18)
        Me.tbSSN.Mask = "000-00-0000"
        Me.tbSSN.Name = "tbSSN"
        Me.tbSSN.ReadOnly = True
        Me.tbSSN.Size = New System.Drawing.Size(93, 20)
        Me.tbSSN.TabIndex = 3
        '
        'tbState
        '
        Me.tbState.Location = New System.Drawing.Point(268, 128)
        Me.tbState.Name = "tbState"
        Me.tbState.ReadOnly = True
        Me.tbState.Size = New System.Drawing.Size(27, 20)
        Me.tbState.TabIndex = 12
        '
        'tbCity
        '
        Me.tbCity.Location = New System.Drawing.Point(130, 128)
        Me.tbCity.Name = "tbCity"
        Me.tbCity.ReadOnly = True
        Me.tbCity.Size = New System.Drawing.Size(136, 20)
        Me.tbCity.TabIndex = 11
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(6, 131)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(100, 18)
        Me.Label13.TabIndex = 10
        Me.Label13.Text = "City/State/ZIP:"
        '
        'tbAddr2
        '
        Me.tbAddr2.Location = New System.Drawing.Point(130, 106)
        Me.tbAddr2.Name = "tbAddr2"
        Me.tbAddr2.ReadOnly = True
        Me.tbAddr2.Size = New System.Drawing.Size(228, 20)
        Me.tbAddr2.TabIndex = 9
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(6, 109)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(100, 18)
        Me.Label12.TabIndex = 8
        Me.Label12.Text = "Address #2:"
        '
        'tbAddr1
        '
        Me.tbAddr1.Location = New System.Drawing.Point(130, 84)
        Me.tbAddr1.Name = "tbAddr1"
        Me.tbAddr1.ReadOnly = True
        Me.tbAddr1.Size = New System.Drawing.Size(228, 20)
        Me.tbAddr1.TabIndex = 7
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(6, 87)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(100, 18)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Address #1:"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(6, 65)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(100, 18)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Co-Maker SSN:"
        '
        'tbCMName
        '
        Me.tbCMName.Location = New System.Drawing.Point(130, 40)
        Me.tbCMName.Name = "tbCMName"
        Me.tbCMName.ReadOnly = True
        Me.tbCMName.Size = New System.Drawing.Size(228, 20)
        Me.tbCMName.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(6, 43)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(100, 18)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Co-Maker Name:"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(6, 21)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(100, 18)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "SSN:"
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(380, 5)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(125, 14)
        Me.Label19.TabIndex = 1
        Me.Label19.Text = "Last Five Comments:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.tbQueueText)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.tbQueue)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.tbBin)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.tbTaskLeftInBin)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(738, 216)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(335, 150)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Bin and Queue Information:"
        '
        'tbQueueText
        '
        Me.tbQueueText.Location = New System.Drawing.Point(157, 84)
        Me.tbQueueText.Multiline = True
        Me.tbQueueText.Name = "tbQueueText"
        Me.tbQueueText.ReadOnly = True
        Me.tbQueueText.Size = New System.Drawing.Size(172, 57)
        Me.tbQueueText.TabIndex = 7
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(6, 87)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 14)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Queue Text:"
        '
        'tbQueue
        '
        Me.tbQueue.Location = New System.Drawing.Point(157, 62)
        Me.tbQueue.Name = "tbQueue"
        Me.tbQueue.ReadOnly = True
        Me.tbQueue.Size = New System.Drawing.Size(172, 20)
        Me.tbQueue.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(6, 65)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 20)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Queue:"
        '
        'tbBin
        '
        Me.tbBin.Location = New System.Drawing.Point(157, 41)
        Me.tbBin.Name = "tbBin"
        Me.tbBin.ReadOnly = True
        Me.tbBin.Size = New System.Drawing.Size(172, 20)
        Me.tbBin.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 44)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(86, 20)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Bin:"
        '
        'tbTaskLeftInBin
        '
        Me.tbTaskLeftInBin.Location = New System.Drawing.Point(157, 20)
        Me.tbTaskLeftInBin.Name = "tbTaskLeftInBin"
        Me.tbTaskLeftInBin.ReadOnly = True
        Me.tbTaskLeftInBin.Size = New System.Drawing.Size(172, 20)
        Me.tbTaskLeftInBin.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Tasks left in bin:"
        '
        'tabForwarding
        '
        Me.tabForwarding.Controls.Add(Me.lstCallF)
        Me.tabForwarding.Location = New System.Drawing.Point(4, 22)
        Me.tabForwarding.Name = "tabForwarding"
        Me.tabForwarding.Padding = New System.Windows.Forms.Padding(3)
        Me.tabForwarding.Size = New System.Drawing.Size(1079, 597)
        Me.tabForwarding.TabIndex = 1
        Me.tabForwarding.Text = "Call Forwarding"
        Me.tabForwarding.UseVisualStyleBackColor = True
        '
        'lstCallF
        '
        Me.lstCallF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstCallF.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstCallF.Location = New System.Drawing.Point(6, 6)
        Me.lstCallF.Name = "lstCallF"
        Me.lstCallF.Size = New System.Drawing.Size(184, 574)
        Me.lstCallF.TabIndex = 25
        Me.lstCallF.TabStop = False
        '
        'tabLegal
        '
        Me.tabLegal.Controls.Add(Me.GroupBox9)
        Me.tabLegal.Controls.Add(Me.GroupBox8)
        Me.tabLegal.Controls.Add(Me.GroupBox7)
        Me.tabLegal.Location = New System.Drawing.Point(4, 22)
        Me.tabLegal.Name = "tabLegal"
        Me.tabLegal.Size = New System.Drawing.Size(1079, 597)
        Me.tabLegal.TabIndex = 2
        Me.tabLegal.Text = "Legal"
        Me.tabLegal.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.tb1098Dt)
        Me.GroupBox9.Controls.Add(Me.tb1098)
        Me.GroupBox9.Controls.Add(Me.tbYearsOffset)
        Me.GroupBox9.Controls.Add(Me.tbTaxOffsetAmout)
        Me.GroupBox9.Controls.Add(Me.tbCertified)
        Me.GroupBox9.Controls.Add(Me.Label43)
        Me.GroupBox9.Controls.Add(Me.tbTaxOffset)
        Me.GroupBox9.Controls.Add(Me.Label79)
        Me.GroupBox9.Controls.Add(Me.Label81)
        Me.GroupBox9.Controls.Add(Me.Label83)
        Me.GroupBox9.Controls.Add(Me.Label82)
        Me.GroupBox9.Location = New System.Drawing.Point(7, 223)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(278, 164)
        Me.GroupBox9.TabIndex = 66
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Tax Offset:"
        '
        'tb1098Dt
        '
        Me.tb1098Dt.Location = New System.Drawing.Point(167, 132)
        Me.tb1098Dt.Name = "tb1098Dt"
        Me.tb1098Dt.ReadOnly = True
        Me.tb1098Dt.Size = New System.Drawing.Size(100, 20)
        Me.tb1098Dt.TabIndex = 72
        '
        'tb1098
        '
        Me.tb1098.Location = New System.Drawing.Point(167, 109)
        Me.tb1098.Name = "tb1098"
        Me.tb1098.ReadOnly = True
        Me.tb1098.Size = New System.Drawing.Size(100, 20)
        Me.tb1098.TabIndex = 71
        '
        'tbYearsOffset
        '
        Me.tbYearsOffset.Location = New System.Drawing.Point(167, 86)
        Me.tbYearsOffset.Name = "tbYearsOffset"
        Me.tbYearsOffset.ReadOnly = True
        Me.tbYearsOffset.Size = New System.Drawing.Size(100, 20)
        Me.tbYearsOffset.TabIndex = 70
        '
        'tbTaxOffsetAmout
        '
        Me.tbTaxOffsetAmout.Location = New System.Drawing.Point(167, 63)
        Me.tbTaxOffsetAmout.Name = "tbTaxOffsetAmout"
        Me.tbTaxOffsetAmout.ReadOnly = True
        Me.tbTaxOffsetAmout.Size = New System.Drawing.Size(100, 20)
        Me.tbTaxOffsetAmout.TabIndex = 69
        '
        'tbCertified
        '
        Me.tbCertified.Location = New System.Drawing.Point(167, 41)
        Me.tbCertified.Name = "tbCertified"
        Me.tbCertified.ReadOnly = True
        Me.tbCertified.Size = New System.Drawing.Size(100, 20)
        Me.tbCertified.TabIndex = 68
        '
        'Label43
        '
        Me.Label43.Location = New System.Drawing.Point(8, 136)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(64, 16)
        Me.Label43.TabIndex = 64
        Me.Label43.Text = "1098 Sent:"
        '
        'tbTaxOffset
        '
        Me.tbTaxOffset.Location = New System.Drawing.Point(11, 19)
        Me.tbTaxOffset.Name = "tbTaxOffset"
        Me.tbTaxOffset.ReadOnly = True
        Me.tbTaxOffset.Size = New System.Drawing.Size(256, 20)
        Me.tbTaxOffset.TabIndex = 55
        '
        'Label79
        '
        Me.Label79.Location = New System.Drawing.Point(8, 44)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(96, 16)
        Me.Label79.TabIndex = 49
        Me.Label79.Text = "Certified:"
        '
        'Label81
        '
        Me.Label81.Location = New System.Drawing.Point(8, 66)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(96, 16)
        Me.Label81.TabIndex = 52
        Me.Label81.Text = "Amount:"
        '
        'Label83
        '
        Me.Label83.Location = New System.Drawing.Point(8, 112)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(96, 16)
        Me.Label83.TabIndex = 54
        Me.Label83.Text = "1098 Amt:"
        '
        'Label82
        '
        Me.Label82.Location = New System.Drawing.Point(8, 89)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(139, 17)
        Me.Label82.TabIndex = 53
        Me.Label82.Text = "Number Of Years Offset:"
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.tbAJOutstandingDt)
        Me.GroupBox8.Controls.Add(Me.tbAJOutstanding)
        Me.GroupBox8.Controls.Add(Me.Label76)
        Me.GroupBox8.Controls.Add(Me.lblPrimary2)
        Me.GroupBox8.Location = New System.Drawing.Point(7, 148)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(278, 69)
        Me.GroupBox8.TabIndex = 65
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Judgement:"
        '
        'tbAJOutstandingDt
        '
        Me.tbAJOutstandingDt.Location = New System.Drawing.Point(167, 40)
        Me.tbAJOutstandingDt.Name = "tbAJOutstandingDt"
        Me.tbAJOutstandingDt.ReadOnly = True
        Me.tbAJOutstandingDt.Size = New System.Drawing.Size(100, 20)
        Me.tbAJOutstandingDt.TabIndex = 67
        '
        'tbAJOutstanding
        '
        Me.tbAJOutstanding.Location = New System.Drawing.Point(167, 18)
        Me.tbAJOutstanding.Name = "tbAJOutstanding"
        Me.tbAJOutstanding.ReadOnly = True
        Me.tbAJOutstanding.Size = New System.Drawing.Size(100, 20)
        Me.tbAJOutstanding.TabIndex = 66
        '
        'Label76
        '
        Me.Label76.Location = New System.Drawing.Point(8, 22)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(96, 16)
        Me.Label76.TabIndex = 44
        Me.Label76.Text = "Outstanding:"
        '
        'lblPrimary2
        '
        Me.lblPrimary2.Location = New System.Drawing.Point(8, 44)
        Me.lblPrimary2.Name = "lblPrimary2"
        Me.lblPrimary2.Size = New System.Drawing.Size(96, 16)
        Me.lblPrimary2.TabIndex = 63
        Me.lblPrimary2.Text = "Primary Action Dt:"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.tbDay30Notice)
        Me.GroupBox7.Controls.Add(Me.tbPrimaryAction)
        Me.GroupBox7.Controls.Add(Me.Label44)
        Me.GroupBox7.Controls.Add(Me.tbGarnish)
        Me.GroupBox7.Controls.Add(Me.tbLastPaymentDt)
        Me.GroupBox7.Controls.Add(Me.tbLastPaymentAmount)
        Me.GroupBox7.Controls.Add(Me.Label70)
        Me.GroupBox7.Controls.Add(Me.lblPrimary1)
        Me.GroupBox7.Controls.Add(Me.Label65)
        Me.GroupBox7.Location = New System.Drawing.Point(7, 3)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(278, 139)
        Me.GroupBox7.TabIndex = 52
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Garnishment:"
        '
        'tbDay30Notice
        '
        Me.tbDay30Notice.Location = New System.Drawing.Point(167, 109)
        Me.tbDay30Notice.Name = "tbDay30Notice"
        Me.tbDay30Notice.ReadOnly = True
        Me.tbDay30Notice.Size = New System.Drawing.Size(100, 20)
        Me.tbDay30Notice.TabIndex = 68
        '
        'tbPrimaryAction
        '
        Me.tbPrimaryAction.Location = New System.Drawing.Point(167, 87)
        Me.tbPrimaryAction.Name = "tbPrimaryAction"
        Me.tbPrimaryAction.ReadOnly = True
        Me.tbPrimaryAction.Size = New System.Drawing.Size(100, 20)
        Me.tbPrimaryAction.TabIndex = 67
        '
        'Label44
        '
        Me.Label44.Location = New System.Drawing.Point(8, 47)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(152, 16)
        Me.Label44.TabIndex = 66
        Me.Label44.Text = "Last Garn Rcvd:"
        '
        'tbGarnish
        '
        Me.tbGarnish.Location = New System.Drawing.Point(11, 21)
        Me.tbGarnish.Name = "tbGarnish"
        Me.tbGarnish.ReadOnly = True
        Me.tbGarnish.Size = New System.Drawing.Size(256, 20)
        Me.tbGarnish.TabIndex = 54
        '
        'tbLastPaymentDt
        '
        Me.tbLastPaymentDt.Location = New System.Drawing.Point(167, 43)
        Me.tbLastPaymentDt.Name = "tbLastPaymentDt"
        Me.tbLastPaymentDt.ReadOnly = True
        Me.tbLastPaymentDt.Size = New System.Drawing.Size(100, 20)
        Me.tbLastPaymentDt.TabIndex = 65
        '
        'tbLastPaymentAmount
        '
        Me.tbLastPaymentAmount.Location = New System.Drawing.Point(167, 65)
        Me.tbLastPaymentAmount.Name = "tbLastPaymentAmount"
        Me.tbLastPaymentAmount.ReadOnly = True
        Me.tbLastPaymentAmount.Size = New System.Drawing.Size(100, 20)
        Me.tbLastPaymentAmount.TabIndex = 53
        '
        'Label70
        '
        Me.Label70.Location = New System.Drawing.Point(8, 116)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(96, 16)
        Me.Label70.TabIndex = 40
        Me.Label70.Text = "30 Day Notice:"
        '
        'lblPrimary1
        '
        Me.lblPrimary1.Location = New System.Drawing.Point(8, 92)
        Me.lblPrimary1.Name = "lblPrimary1"
        Me.lblPrimary1.Size = New System.Drawing.Size(96, 16)
        Me.lblPrimary1.TabIndex = 4
        Me.lblPrimary1.Text = "Primary Action Dt:"
        '
        'Label65
        '
        Me.Label65.Location = New System.Drawing.Point(8, 70)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(152, 16)
        Me.Label65.TabIndex = 1
        Me.Label65.Text = "Last Garn Amt Rcvd:"
        '
        'tabAcctInfo
        '
        Me.tabAcctInfo.Controls.Add(Me.GroupBox11)
        Me.tabAcctInfo.Controls.Add(Me.GroupBox10)
        Me.tabAcctInfo.Location = New System.Drawing.Point(4, 22)
        Me.tabAcctInfo.Name = "tabAcctInfo"
        Me.tabAcctInfo.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAcctInfo.Size = New System.Drawing.Size(1079, 597)
        Me.tabAcctInfo.TabIndex = 3
        Me.tabAcctInfo.Text = "Account Information"
        Me.tabAcctInfo.UseVisualStyleBackColor = True
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.tbRehabCode)
        Me.GroupBox11.Controls.Add(Me.Label63)
        Me.GroupBox11.Controls.Add(Me.lstServicer)
        Me.GroupBox11.Controls.Add(Me.Label60)
        Me.GroupBox11.Controls.Add(Me.Label59)
        Me.GroupBox11.Controls.Add(Me.tbPendingPayments)
        Me.GroupBox11.Controls.Add(Me.tbLoanPg)
        Me.GroupBox11.Controls.Add(Me.Label57)
        Me.GroupBox11.Controls.Add(Me.Label56)
        Me.GroupBox11.Controls.Add(Me.tbIneligibleForRehabCode)
        Me.GroupBox11.Controls.Add(Me.tbRehabCounter)
        Me.GroupBox11.Controls.Add(Me.tbReinstatementDate)
        Me.GroupBox11.Controls.Add(Me.lstLC41)
        Me.GroupBox11.Controls.Add(Me.tbReinstatementEligibilityCode)
        Me.GroupBox11.Controls.Add(Me.tbCollectionInd)
        Me.GroupBox11.Controls.Add(Me.Label58)
        Me.GroupBox11.Controls.Add(Me.Label86)
        Me.GroupBox11.Controls.Add(Me.Label61)
        Me.GroupBox11.Controls.Add(Me.Label62)
        Me.GroupBox11.Controls.Add(Me.Label55)
        Me.GroupBox11.Location = New System.Drawing.Point(532, 3)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(544, 506)
        Me.GroupBox11.TabIndex = 128
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Other Account Information:"
        '
        'tbRehabCode
        '
        Me.tbRehabCode.Location = New System.Drawing.Point(151, 120)
        Me.tbRehabCode.Name = "tbRehabCode"
        Me.tbRehabCode.ReadOnly = True
        Me.tbRehabCode.Size = New System.Drawing.Size(117, 20)
        Me.tbRehabCode.TabIndex = 135
        '
        'Label63
        '
        Me.Label63.Location = New System.Drawing.Point(16, 120)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(89, 17)
        Me.Label63.TabIndex = 134
        Me.Label63.Text = "Rehab Code:"
        '
        'lstServicer
        '
        Me.lstServicer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstServicer.Location = New System.Drawing.Point(151, 431)
        Me.lstServicer.Name = "lstServicer"
        Me.lstServicer.Size = New System.Drawing.Size(258, 54)
        Me.lstServicer.TabIndex = 133
        Me.lstServicer.TabStop = False
        '
        'Label60
        '
        Me.Label60.Location = New System.Drawing.Point(16, 431)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(88, 23)
        Me.Label60.TabIndex = 132
        Me.Label60.Text = "Servicer:"
        '
        'Label59
        '
        Me.Label59.Location = New System.Drawing.Point(15, 408)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(117, 17)
        Me.Label59.TabIndex = 131
        Me.Label59.Text = "Pending Payments:"
        '
        'tbPendingPayments
        '
        Me.tbPendingPayments.Location = New System.Drawing.Point(151, 405)
        Me.tbPendingPayments.Name = "tbPendingPayments"
        Me.tbPendingPayments.ReadOnly = True
        Me.tbPendingPayments.Size = New System.Drawing.Size(388, 20)
        Me.tbPendingPayments.TabIndex = 130
        '
        'tbLoanPg
        '
        Me.tbLoanPg.Location = New System.Drawing.Point(151, 381)
        Me.tbLoanPg.Name = "tbLoanPg"
        Me.tbLoanPg.ReadOnly = True
        Me.tbLoanPg.Size = New System.Drawing.Size(388, 20)
        Me.tbLoanPg.TabIndex = 129
        '
        'Label57
        '
        Me.Label57.Location = New System.Drawing.Point(15, 171)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(130, 18)
        Me.Label57.TabIndex = 128
        Me.Label57.Text = "Last Nine Payments:"
        '
        'Label56
        '
        Me.Label56.Location = New System.Drawing.Point(15, 385)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(80, 16)
        Me.Label56.TabIndex = 126
        Me.Label56.Text = "Loan Program:"
        '
        'tbIneligibleForRehabCode
        '
        Me.tbIneligibleForRehabCode.Location = New System.Drawing.Point(151, 146)
        Me.tbIneligibleForRehabCode.Name = "tbIneligibleForRehabCode"
        Me.tbIneligibleForRehabCode.ReadOnly = True
        Me.tbIneligibleForRehabCode.Size = New System.Drawing.Size(117, 20)
        Me.tbIneligibleForRehabCode.TabIndex = 127
        '
        'tbRehabCounter
        '
        Me.tbRehabCounter.Location = New System.Drawing.Point(151, 97)
        Me.tbRehabCounter.Name = "tbRehabCounter"
        Me.tbRehabCounter.ReadOnly = True
        Me.tbRehabCounter.Size = New System.Drawing.Size(117, 20)
        Me.tbRehabCounter.TabIndex = 126
        '
        'tbReinstatementDate
        '
        Me.tbReinstatementDate.Location = New System.Drawing.Point(151, 74)
        Me.tbReinstatementDate.Name = "tbReinstatementDate"
        Me.tbReinstatementDate.ReadOnly = True
        Me.tbReinstatementDate.Size = New System.Drawing.Size(117, 20)
        Me.tbReinstatementDate.TabIndex = 125
        '
        'lstLC41
        '
        Me.lstLC41.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstLC41.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader6})
        Me.lstLC41.FullRowSelect = True
        Me.lstLC41.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstLC41.Location = New System.Drawing.Point(151, 171)
        Me.lstLC41.Name = "lstLC41"
        Me.lstLC41.Size = New System.Drawing.Size(388, 204)
        Me.lstLC41.TabIndex = 125
        Me.lstLC41.UseCompatibleStateImageBehavior = False
        Me.lstLC41.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Amt Appl"
        Me.ColumnHeader5.Width = 100
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Effective Dt"
        Me.ColumnHeader3.Width = 102
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Trans Type"
        Me.ColumnHeader4.Width = 111
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Rev Type"
        Me.ColumnHeader6.Width = 72
        '
        'tbReinstatementEligibilityCode
        '
        Me.tbReinstatementEligibilityCode.Location = New System.Drawing.Point(151, 51)
        Me.tbReinstatementEligibilityCode.Name = "tbReinstatementEligibilityCode"
        Me.tbReinstatementEligibilityCode.ReadOnly = True
        Me.tbReinstatementEligibilityCode.Size = New System.Drawing.Size(117, 20)
        Me.tbReinstatementEligibilityCode.TabIndex = 123
        '
        'tbCollectionInd
        '
        Me.tbCollectionInd.Location = New System.Drawing.Point(151, 28)
        Me.tbCollectionInd.Name = "tbCollectionInd"
        Me.tbCollectionInd.ReadOnly = True
        Me.tbCollectionInd.Size = New System.Drawing.Size(117, 20)
        Me.tbCollectionInd.TabIndex = 114
        '
        'Label58
        '
        Me.Label58.Location = New System.Drawing.Point(15, 149)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(137, 17)
        Me.Label58.TabIndex = 119
        Me.Label58.Text = "Rehab Ineligible Reason:"
        '
        'Label86
        '
        Me.Label86.Location = New System.Drawing.Point(15, 31)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(137, 17)
        Me.Label86.TabIndex = 91
        Me.Label86.Text = "Collection Costs Indicator:"
        '
        'Label61
        '
        Me.Label61.Location = New System.Drawing.Point(15, 55)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(78, 17)
        Me.Label61.TabIndex = 121
        Me.Label61.Text = "Reinstatement:"
        '
        'Label62
        '
        Me.Label62.Location = New System.Drawing.Point(15, 78)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(125, 17)
        Me.Label62.TabIndex = 123
        Me.Label62.Text = "Reinstatement Date:"
        '
        'Label55
        '
        Me.Label55.Location = New System.Drawing.Point(15, 100)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(89, 17)
        Me.Label55.TabIndex = 117
        Me.Label55.Text = "Rehab Counter:"
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.Label52)
        Me.GroupBox10.Controls.Add(Me.tbTotalR)
        Me.GroupBox10.Controls.Add(Me.lblLine)
        Me.GroupBox10.Controls.Add(Me.tbTotalP)
        Me.GroupBox10.Controls.Add(Me.Label54)
        Me.GroupBox10.Controls.Add(Me.tbTotalC)
        Me.GroupBox10.Controls.Add(Me.Label53)
        Me.GroupBox10.Controls.Add(Me.tbProjectedR)
        Me.GroupBox10.Controls.Add(Me.Label51)
        Me.GroupBox10.Controls.Add(Me.tbProjectedC)
        Me.GroupBox10.Controls.Add(Me.Label50)
        Me.GroupBox10.Controls.Add(Me.tbCollectionR)
        Me.GroupBox10.Controls.Add(Me.Label49)
        Me.GroupBox10.Controls.Add(Me.tbCollectionP)
        Me.GroupBox10.Controls.Add(Me.Label48)
        Me.GroupBox10.Controls.Add(Me.tbCollectionC)
        Me.GroupBox10.Controls.Add(Me.Label47)
        Me.GroupBox10.Controls.Add(Me.tbOtherR)
        Me.GroupBox10.Controls.Add(Me.Label46)
        Me.GroupBox10.Controls.Add(Me.tbOtherP)
        Me.GroupBox10.Controls.Add(Me.Label45)
        Me.GroupBox10.Controls.Add(Me.tbOtherC)
        Me.GroupBox10.Controls.Add(Me.tbPrincipalC)
        Me.GroupBox10.Controls.Add(Me.tbLegalR)
        Me.GroupBox10.Controls.Add(Me.tbPrincipalP)
        Me.GroupBox10.Controls.Add(Me.tbLegalP)
        Me.GroupBox10.Controls.Add(Me.tbPrincipalR)
        Me.GroupBox10.Controls.Add(Me.tbLegalC)
        Me.GroupBox10.Controls.Add(Me.tbInterestC)
        Me.GroupBox10.Controls.Add(Me.tbInterestR)
        Me.GroupBox10.Controls.Add(Me.tbInterestP)
        Me.GroupBox10.Location = New System.Drawing.Point(6, 3)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(522, 224)
        Me.GroupBox10.TabIndex = 116
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Borrower Status:"
        '
        'Label52
        '
        Me.Label52.Location = New System.Drawing.Point(15, 67)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(104, 16)
        Me.Label52.TabIndex = 63
        Me.Label52.Text = "Interest:"
        '
        'tbTotalR
        '
        Me.tbTotalR.Location = New System.Drawing.Point(397, 188)
        Me.tbTotalR.Name = "tbTotalR"
        Me.tbTotalR.ReadOnly = True
        Me.tbTotalR.Size = New System.Drawing.Size(117, 20)
        Me.tbTotalR.TabIndex = 115
        '
        'lblLine
        '
        Me.lblLine.BackColor = System.Drawing.Color.Black
        Me.lblLine.ForeColor = System.Drawing.Color.Black
        Me.lblLine.Location = New System.Drawing.Point(18, 181)
        Me.lblLine.Name = "lblLine"
        Me.lblLine.Size = New System.Drawing.Size(496, 2)
        Me.lblLine.TabIndex = 68
        '
        'tbTotalP
        '
        Me.tbTotalP.Location = New System.Drawing.Point(274, 188)
        Me.tbTotalP.Name = "tbTotalP"
        Me.tbTotalP.ReadOnly = True
        Me.tbTotalP.Size = New System.Drawing.Size(117, 20)
        Me.tbTotalP.TabIndex = 114
        '
        'Label54
        '
        Me.Label54.Location = New System.Drawing.Point(15, 192)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(104, 16)
        Me.Label54.TabIndex = 60
        Me.Label54.Text = "Total:"
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTotalC
        '
        Me.tbTotalC.Location = New System.Drawing.Point(151, 188)
        Me.tbTotalC.Name = "tbTotalC"
        Me.tbTotalC.ReadOnly = True
        Me.tbTotalC.Size = New System.Drawing.Size(117, 20)
        Me.tbTotalC.TabIndex = 113
        '
        'Label53
        '
        Me.Label53.Location = New System.Drawing.Point(15, 44)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(104, 16)
        Me.Label53.TabIndex = 62
        Me.Label53.Text = "Principal:"
        '
        'tbProjectedR
        '
        Me.tbProjectedR.Location = New System.Drawing.Point(397, 156)
        Me.tbProjectedR.Name = "tbProjectedR"
        Me.tbProjectedR.ReadOnly = True
        Me.tbProjectedR.Size = New System.Drawing.Size(117, 20)
        Me.tbProjectedR.TabIndex = 112
        '
        'Label51
        '
        Me.Label51.Location = New System.Drawing.Point(15, 90)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(104, 16)
        Me.Label51.TabIndex = 64
        Me.Label51.Text = "Legal Costs:"
        '
        'tbProjectedC
        '
        Me.tbProjectedC.Location = New System.Drawing.Point(151, 156)
        Me.tbProjectedC.Name = "tbProjectedC"
        Me.tbProjectedC.ReadOnly = True
        Me.tbProjectedC.Size = New System.Drawing.Size(117, 20)
        Me.tbProjectedC.TabIndex = 110
        '
        'Label50
        '
        Me.Label50.Location = New System.Drawing.Point(15, 114)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(104, 16)
        Me.Label50.TabIndex = 65
        Me.Label50.Text = "Other Charges:"
        '
        'tbCollectionR
        '
        Me.tbCollectionR.Location = New System.Drawing.Point(397, 133)
        Me.tbCollectionR.Name = "tbCollectionR"
        Me.tbCollectionR.ReadOnly = True
        Me.tbCollectionR.Size = New System.Drawing.Size(117, 20)
        Me.tbCollectionR.TabIndex = 109
        '
        'Label49
        '
        Me.Label49.Location = New System.Drawing.Point(15, 137)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(104, 16)
        Me.Label49.TabIndex = 66
        Me.Label49.Text = "Collection Costs:"
        '
        'tbCollectionP
        '
        Me.tbCollectionP.Location = New System.Drawing.Point(274, 133)
        Me.tbCollectionP.Name = "tbCollectionP"
        Me.tbCollectionP.ReadOnly = True
        Me.tbCollectionP.Size = New System.Drawing.Size(117, 20)
        Me.tbCollectionP.TabIndex = 108
        '
        'Label48
        '
        Me.Label48.Location = New System.Drawing.Point(15, 159)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(104, 16)
        Me.Label48.TabIndex = 67
        Me.Label48.Text = "Projected Coll Csts:"
        '
        'tbCollectionC
        '
        Me.tbCollectionC.Location = New System.Drawing.Point(151, 133)
        Me.tbCollectionC.Name = "tbCollectionC"
        Me.tbCollectionC.ReadOnly = True
        Me.tbCollectionC.Size = New System.Drawing.Size(117, 20)
        Me.tbCollectionC.TabIndex = 107
        '
        'Label47
        '
        Me.Label47.Location = New System.Drawing.Point(151, 22)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(117, 16)
        Me.Label47.TabIndex = 69
        Me.Label47.Text = "Begin/Accrued"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbOtherR
        '
        Me.tbOtherR.Location = New System.Drawing.Point(397, 110)
        Me.tbOtherR.Name = "tbOtherR"
        Me.tbOtherR.ReadOnly = True
        Me.tbOtherR.Size = New System.Drawing.Size(117, 20)
        Me.tbOtherR.TabIndex = 106
        '
        'Label46
        '
        Me.Label46.Location = New System.Drawing.Point(274, 22)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(117, 16)
        Me.Label46.TabIndex = 70
        Me.Label46.Text = "Paid"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbOtherP
        '
        Me.tbOtherP.Location = New System.Drawing.Point(274, 110)
        Me.tbOtherP.Name = "tbOtherP"
        Me.tbOtherP.ReadOnly = True
        Me.tbOtherP.Size = New System.Drawing.Size(117, 20)
        Me.tbOtherP.TabIndex = 105
        '
        'Label45
        '
        Me.Label45.Location = New System.Drawing.Point(397, 22)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(117, 16)
        Me.Label45.TabIndex = 71
        Me.Label45.Text = "Remaining"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbOtherC
        '
        Me.tbOtherC.Location = New System.Drawing.Point(151, 110)
        Me.tbOtherC.Name = "tbOtherC"
        Me.tbOtherC.ReadOnly = True
        Me.tbOtherC.Size = New System.Drawing.Size(117, 20)
        Me.tbOtherC.TabIndex = 104
        '
        'tbPrincipalC
        '
        Me.tbPrincipalC.Location = New System.Drawing.Point(151, 41)
        Me.tbPrincipalC.Name = "tbPrincipalC"
        Me.tbPrincipalC.ReadOnly = True
        Me.tbPrincipalC.Size = New System.Drawing.Size(117, 20)
        Me.tbPrincipalC.TabIndex = 93
        '
        'tbLegalR
        '
        Me.tbLegalR.Location = New System.Drawing.Point(397, 87)
        Me.tbLegalR.Name = "tbLegalR"
        Me.tbLegalR.ReadOnly = True
        Me.tbLegalR.Size = New System.Drawing.Size(117, 20)
        Me.tbLegalR.TabIndex = 103
        '
        'tbPrincipalP
        '
        Me.tbPrincipalP.Location = New System.Drawing.Point(274, 41)
        Me.tbPrincipalP.Name = "tbPrincipalP"
        Me.tbPrincipalP.ReadOnly = True
        Me.tbPrincipalP.Size = New System.Drawing.Size(117, 20)
        Me.tbPrincipalP.TabIndex = 96
        '
        'tbLegalP
        '
        Me.tbLegalP.Location = New System.Drawing.Point(274, 87)
        Me.tbLegalP.Name = "tbLegalP"
        Me.tbLegalP.ReadOnly = True
        Me.tbLegalP.Size = New System.Drawing.Size(117, 20)
        Me.tbLegalP.TabIndex = 102
        '
        'tbPrincipalR
        '
        Me.tbPrincipalR.Location = New System.Drawing.Point(397, 41)
        Me.tbPrincipalR.Name = "tbPrincipalR"
        Me.tbPrincipalR.ReadOnly = True
        Me.tbPrincipalR.Size = New System.Drawing.Size(117, 20)
        Me.tbPrincipalR.TabIndex = 97
        '
        'tbLegalC
        '
        Me.tbLegalC.Location = New System.Drawing.Point(151, 87)
        Me.tbLegalC.Name = "tbLegalC"
        Me.tbLegalC.ReadOnly = True
        Me.tbLegalC.Size = New System.Drawing.Size(117, 20)
        Me.tbLegalC.TabIndex = 101
        '
        'tbInterestC
        '
        Me.tbInterestC.Location = New System.Drawing.Point(151, 64)
        Me.tbInterestC.Name = "tbInterestC"
        Me.tbInterestC.ReadOnly = True
        Me.tbInterestC.Size = New System.Drawing.Size(117, 20)
        Me.tbInterestC.TabIndex = 98
        '
        'tbInterestR
        '
        Me.tbInterestR.Location = New System.Drawing.Point(397, 64)
        Me.tbInterestR.Name = "tbInterestR"
        Me.tbInterestR.ReadOnly = True
        Me.tbInterestR.Size = New System.Drawing.Size(117, 20)
        Me.tbInterestR.TabIndex = 100
        '
        'tbInterestP
        '
        Me.tbInterestP.Location = New System.Drawing.Point(274, 64)
        Me.tbInterestP.Name = "tbInterestP"
        Me.tbInterestP.ReadOnly = True
        Me.tbInterestP.Size = New System.Drawing.Size(117, 20)
        Me.tbInterestP.TabIndex = 99
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(24, 22)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnPaymentArrangements, Me.tsbtnCheckByPhone, Me.ToolStripSeparator3, Me.tsbtnUpdateDemos, Me.ToolStripSeparator2, Me.tsbtnLoanDetail, Me.ToolStripSeparator5, Me.tsbtn411, Me.tsbtnAskDUDE, Me.tsbtnWipeOut, Me.tsbtnBrightIdea, Me.ToolStripSeparator1, Me.tsbtnActHist30, Me.tsbtnActHist90, Me.tsbtnActHist180, Me.tsbtnActHistAll, Me.tsbtnSinceOpen, Me.ToolStripSeparator4, Me.tsbtnOutstandingQueues})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1091, 29)
        Me.ToolStrip1.TabIndex = 7
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnPaymentArrangements
        '
        Me.tsbtnPaymentArrangements.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnPaymentArrangements.Image = Global.MDLMHome.My.Resources.Resources.DollarSign
        Me.tsbtnPaymentArrangements.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnPaymentArrangements.Name = "tsbtnPaymentArrangements"
        Me.tsbtnPaymentArrangements.Size = New System.Drawing.Size(28, 26)
        Me.tsbtnPaymentArrangements.Text = "ToolStripButton1"
        Me.tsbtnPaymentArrangements.ToolTipText = "Payment Arrangements"
        '
        'tsbtnCheckByPhone
        '
        Me.tsbtnCheckByPhone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnCheckByPhone.Image = Global.MDLMHome.My.Resources.Resources.Phone4ToolBar
        Me.tsbtnCheckByPhone.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCheckByPhone.Name = "tsbtnCheckByPhone"
        Me.tsbtnCheckByPhone.Size = New System.Drawing.Size(28, 26)
        Me.tsbtnCheckByPhone.Text = "ToolStripButton1"
        Me.tsbtnCheckByPhone.ToolTipText = "Check By Phone"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 29)
        '
        'tsbtnUpdateDemos
        '
        Me.tsbtnUpdateDemos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnUpdateDemos.Image = Global.MDLMHome.My.Resources.Resources.Book
        Me.tsbtnUpdateDemos.ImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(2, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.tsbtnUpdateDemos.Name = "tsbtnUpdateDemos"
        Me.tsbtnUpdateDemos.Size = New System.Drawing.Size(28, 26)
        Me.tsbtnUpdateDemos.Text = "ToolStripButton1"
        Me.tsbtnUpdateDemos.ToolTipText = "Demographics"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 29)
        '
        'tsbtnLoanDetail
        '
        Me.tsbtnLoanDetail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnLoanDetail.Image = CType(resources.GetObject("tsbtnLoanDetail.Image"), System.Drawing.Image)
        Me.tsbtnLoanDetail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnLoanDetail.Name = "tsbtnLoanDetail"
        Me.tsbtnLoanDetail.Size = New System.Drawing.Size(28, 26)
        Me.tsbtnLoanDetail.Text = "ToolStripButton1"
        Me.tsbtnLoanDetail.ToolTipText = "Loan Detail"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 29)
        '
        'tsbtn411
        '
        Me.tsbtn411.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn411.Image = Global.MDLMHome.My.Resources.Resources.Info
        Me.tsbtn411.ImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(2, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.tsbtn411.Name = "tsbtn411"
        Me.tsbtn411.Size = New System.Drawing.Size(28, 26)
        Me.tsbtn411.Text = "ToolStripButton1"
        Me.tsbtn411.ToolTipText = "Borrower Information (411)"
        '
        'tsbtnAskDUDE
        '
        Me.tsbtnAskDUDE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnAskDUDE.Image = Global.MDLMHome.My.Resources.Resources.Question
        Me.tsbtnAskDUDE.ImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(2, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.tsbtnAskDUDE.Name = "tsbtnAskDUDE"
        Me.tsbtnAskDUDE.Size = New System.Drawing.Size(28, 26)
        Me.tsbtnAskDUDE.Text = "ToolStripButton1"
        Me.tsbtnAskDUDE.ToolTipText = "Ask DUDE"
        '
        'tsbtnWipeOut
        '
        Me.tsbtnWipeOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnWipeOut.Image = Global.MDLMHome.My.Resources.Resources.mask
        Me.tsbtnWipeOut.ImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(2, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.tsbtnWipeOut.Name = "tsbtnWipeOut"
        Me.tsbtnWipeOut.Size = New System.Drawing.Size(28, 26)
        Me.tsbtnWipeOut.Text = "ToolStripButton2"
        Me.tsbtnWipeOut.ToolTipText = "Did something no work the way it should have?  Let us know."
        '
        'tsbtnBrightIdea
        '
        Me.tsbtnBrightIdea.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnBrightIdea.Image = Global.MDLMHome.My.Resources.Resources.light
        Me.tsbtnBrightIdea.ImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(2, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.tsbtnBrightIdea.Name = "tsbtnBrightIdea"
        Me.tsbtnBrightIdea.Size = New System.Drawing.Size(28, 26)
        Me.tsbtnBrightIdea.Text = "ToolStripButton1"
        Me.tsbtnBrightIdea.ToolTipText = "Got a bright idea?  Share it with us."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 29)
        '
        'tsbtnActHist30
        '
        Me.tsbtnActHist30.Image = CType(resources.GetObject("tsbtnActHist30.Image"), System.Drawing.Image)
        Me.tsbtnActHist30.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnActHist30.Name = "tsbtnActHist30"
        Me.tsbtnActHist30.Size = New System.Drawing.Size(75, 26)
        Me.tsbtnActHist30.Text = "30 Days"
        Me.tsbtnActHist30.ToolTipText = "Last 30 Days of  Activity Comments"
        '
        'tsbtnActHist90
        '
        Me.tsbtnActHist90.Image = CType(resources.GetObject("tsbtnActHist90.Image"), System.Drawing.Image)
        Me.tsbtnActHist90.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnActHist90.Name = "tsbtnActHist90"
        Me.tsbtnActHist90.Size = New System.Drawing.Size(75, 26)
        Me.tsbtnActHist90.Text = "90 Days"
        Me.tsbtnActHist90.ToolTipText = "Last 90 Days of  Activity Comments"
        '
        'tsbtnActHist180
        '
        Me.tsbtnActHist180.Image = CType(resources.GetObject("tsbtnActHist180.Image"), System.Drawing.Image)
        Me.tsbtnActHist180.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnActHist180.Name = "tsbtnActHist180"
        Me.tsbtnActHist180.Size = New System.Drawing.Size(81, 26)
        Me.tsbtnActHist180.Text = "180 Days"
        Me.tsbtnActHist180.ToolTipText = "Last 180 Days of  Activity Comments"
        '
        'tsbtnActHistAll
        '
        Me.tsbtnActHistAll.Image = CType(resources.GetObject("tsbtnActHistAll.Image"), System.Drawing.Image)
        Me.tsbtnActHistAll.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnActHistAll.Name = "tsbtnActHistAll"
        Me.tsbtnActHistAll.Size = New System.Drawing.Size(111, 26)
        Me.tsbtnActHistAll.Text = "All Comments"
        Me.tsbtnActHistAll.ToolTipText = "All Activity Comments"
        '
        'tsbtnSinceOpen
        '
        Me.tsbtnSinceOpen.Image = CType(resources.GetObject("tsbtnSinceOpen.Image"), System.Drawing.Image)
        Me.tsbtnSinceOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSinceOpen.Name = "tsbtnSinceOpen"
        Me.tsbtnSinceOpen.Size = New System.Drawing.Size(95, 26)
        Me.tsbtnSinceOpen.Text = "Since Open"
        Me.tsbtnSinceOpen.ToolTipText = "All comments since the account was open "
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 29)
        '
        'tsbtnOutstandingQueues
        '
        Me.tsbtnOutstandingQueues.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnOutstandingQueues.Image = CType(resources.GetObject("tsbtnOutstandingQueues.Image"), System.Drawing.Image)
        Me.tsbtnOutstandingQueues.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnOutstandingQueues.Name = "tsbtnOutstandingQueues"
        Me.tsbtnOutstandingQueues.Size = New System.Drawing.Size(158, 26)
        Me.tsbtnOutstandingQueues.Text = "Outstanding Tasks (Default)"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(20, 81)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Acct #:"
        '
        'lblAcctNum
        '
        Me.lblAcctNum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAcctNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAcctNum.Location = New System.Drawing.Point(90, 77)
        Me.lblAcctNum.Name = "lblAcctNum"
        Me.lblAcctNum.Size = New System.Drawing.Size(132, 25)
        Me.lblAcctNum.TabIndex = 9
        Me.lblAcctNum.Text = "9999999999"
        Me.lblAcctNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblName
        '
        Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(292, 77)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(319, 25)
        Me.lblName.TabIndex = 11
        Me.lblName.Text = "Earl J Picklesnarf"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(228, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 17)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Name:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(616, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 17)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Status:"
        '
        'lbStatuses
        '
        Me.lbStatuses.FormattingEnabled = True
        Me.lbStatuses.Location = New System.Drawing.Point(682, 61)
        Me.lbStatuses.Name = "lbStatuses"
        Me.lbStatuses.Size = New System.Drawing.Size(138, 56)
        Me.lbStatuses.TabIndex = 13
        '
        'btnMoStatus
        '
        Me.btnMoStatus.Enabled = False
        Me.btnMoStatus.Location = New System.Drawing.Point(826, 80)
        Me.btnMoStatus.Name = "btnMoStatus"
        Me.btnMoStatus.Size = New System.Drawing.Size(75, 23)
        Me.btnMoStatus.TabIndex = 14
        Me.btnMoStatus.Text = "Mo' Status"
        Me.btnMoStatus.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cbSpecialHandling)
        Me.GroupBox1.Controls.Add(Me.cbVIP)
        Me.GroupBox1.Location = New System.Drawing.Point(911, 57)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(139, 75)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Special Status"
        '
        'cbSpecialHandling
        '
        Me.cbSpecialHandling.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbSpecialHandling.Enabled = False
        Me.cbSpecialHandling.Location = New System.Drawing.Point(7, 39)
        Me.cbSpecialHandling.Name = "cbSpecialHandling"
        Me.cbSpecialHandling.Size = New System.Drawing.Size(124, 24)
        Me.cbSpecialHandling.TabIndex = 1
        Me.cbSpecialHandling.Text = "Special Handling"
        Me.cbSpecialHandling.UseVisualStyleBackColor = True
        '
        'cbVIP
        '
        Me.cbVIP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbVIP.Enabled = False
        Me.cbVIP.Location = New System.Drawing.Point(7, 19)
        Me.cbVIP.Name = "cbVIP"
        Me.cbVIP.Size = New System.Drawing.Size(124, 24)
        Me.cbVIP.TabIndex = 0
        Me.cbVIP.Text = "VIP"
        Me.cbVIP.UseVisualStyleBackColor = True
        '
        'btnSaveAndCont
        '
        Me.btnSaveAndCont.Location = New System.Drawing.Point(373, 779)
        Me.btnSaveAndCont.Name = "btnSaveAndCont"
        Me.btnSaveAndCont.Size = New System.Drawing.Size(161, 23)
        Me.btnSaveAndCont.TabIndex = 16
        Me.btnSaveAndCont.Text = "Save And Continue"
        Me.btnSaveAndCont.UseVisualStyleBackColor = True
        '
        'btnIncomingCall
        '
        Me.btnIncomingCall.Image = Global.MDLMHome.My.Resources.Resources.PHONE
        Me.btnIncomingCall.Location = New System.Drawing.Point(1060, 768)
        Me.btnIncomingCall.Name = "btnIncomingCall"
        Me.btnIncomingCall.Size = New System.Drawing.Size(24, 22)
        Me.btnIncomingCall.TabIndex = 5
        Me.btnIncomingCall.UseVisualStyleBackColor = True
        '
        'ActCmt
        '
        Me.ActCmt.Location = New System.Drawing.Point(394, 22)
        Me.ActCmt.Name = "ActCmt"
        Me.ActCmt.Size = New System.Drawing.Size(677, 90)
        Me.ActCmt.TabIndex = 0
        '
        'frmLMHomePage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1091, 818)
        Me.Controls.Add(Me.btnSaveAndCont)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnMoStatus)
        Me.Controls.Add(Me.lbStatuses)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblAcctNum)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.TabControl)
        Me.Controls.Add(Me.btnIncomingCall)
        Me.Controls.Add(Me.tbIncomingCallSSN)
        Me.Controls.Add(Me.cbPreviousContacts)
        Me.Controls.Add(Me.btnBackToBins)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1099, 850)
        Me.Name = "frmLMHomePage"
        Me.Text = "Account Resolution Home Page"
        Me.TransparencyKey = System.Drawing.Color.Magenta
        Me.Controls.SetChildIndex(Me.btnBackToBins, 0)
        Me.Controls.SetChildIndex(Me.cbPreviousContacts, 0)
        Me.Controls.SetChildIndex(Me.tbIncomingCallSSN, 0)
        Me.Controls.SetChildIndex(Me.btnIncomingCall, 0)
        Me.Controls.SetChildIndex(Me.TabControl, 0)
        Me.Controls.SetChildIndex(Me.ToolStrip1, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.lblAcctNum, 0)
        Me.Controls.SetChildIndex(Me.Label4, 0)
        Me.Controls.SetChildIndex(Me.lblName, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.lbStatuses, 0)
        Me.Controls.SetChildIndex(Me.btnMoStatus, 0)
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.Controls.SetChildIndex(Me.btnSaveAndCont, 0)
        Me.TabControl.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabMain.PerformLayout()
        Me.gbCC.ResumeLayout(False)
        Me.gbCC.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.gbRepaymentCalc.ResumeLayout(False)
        Me.gbRepaymentCalc.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.gbBorrowerInfo.ResumeLayout(False)
        Me.gbBorrowerInfo.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.tabForwarding.ResumeLayout(False)
        Me.tabLegal.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.tabAcctInfo.ResumeLayout(False)
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ActCmt As MDLMHome.ActivityCmts
    Friend WithEvents btnBackToBins As System.Windows.Forms.Button
    Friend WithEvents cbPreviousContacts As System.Windows.Forms.ComboBox
    Friend WithEvents tbIncomingCallSSN As System.Windows.Forms.TextBox
    Friend WithEvents btnIncomingCall As System.Windows.Forms.Button
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents tabMain As System.Windows.Forms.TabPage
    Friend WithEvents tabForwarding As System.Windows.Forms.TabPage
    Friend WithEvents tabLegal As System.Windows.Forms.TabPage
    Friend WithEvents tabAcctInfo As System.Windows.Forms.TabPage
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnPaymentArrangements As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblAcctNum As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbStatuses As System.Windows.Forms.ListBox
    Friend WithEvents btnMoStatus As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbVIP As System.Windows.Forms.CheckBox
    Friend WithEvents cbSpecialHandling As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbTaskLeftInBin As System.Windows.Forms.TextBox
    Friend WithEvents tbBin As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbQueue As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbQueueText As System.Windows.Forms.TextBox
    Friend WithEvents tsbtnCheckByPhone As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtnActHist30 As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnActHist90 As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnActHist180 As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnActHistAll As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtn411 As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnAskDUDE As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnWipeOut As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnBrightIdea As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtnUpdateDemos As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSaveAndCont As System.Windows.Forms.Button
    Friend WithEvents gbBorrowerInfo As System.Windows.Forms.GroupBox
    Friend WithEvents tbAddr1 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbCMName As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tbSSN As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbState As System.Windows.Forms.TextBox
    Friend WithEvents tbCity As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents tbAddr2 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tbEmail As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents tbAPExt As System.Windows.Forms.TextBox
    Friend WithEvents tbAltPhn As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents tbHPExt As System.Windows.Forms.TextBox
    Friend WithEvents tbHomePhn As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents tbZIP As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbCMSSN As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tsbtnSinceOpen As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbDtLastAttempt As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents tbDtLastContact As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbDemoVerified As System.Windows.Forms.TextBox
    Friend WithEvents tbCurrentEmployer As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents tbActivityComments As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnContact As System.Windows.Forms.Button
    Friend WithEvents btnAttempt As System.Windows.Forms.Button
    Protected Friend WithEvents Label33 As System.Windows.Forms.Label
    Protected Friend WithEvents Label32 As System.Windows.Forms.Label
    Protected Friend WithEvents tbContactCode As System.Windows.Forms.TextBox
    Protected Friend WithEvents tbActivityCode As System.Windows.Forms.TextBox
    Protected Friend WithEvents cbContactDesc As System.Windows.Forms.ComboBox
    Protected Friend WithEvents cbActivityDesc As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lvReferences As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents tbCurrPrinc As System.Windows.Forms.TextBox
    Friend WithEvents tbTotalAmtDue As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents tbCollectCost As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents tbCurrInt As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents tbMonthInt As System.Windows.Forms.TextBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents tbMonthPayAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents tbDueDate As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtnOutstandingQueues As System.Windows.Forms.ToolStripButton
    Friend WithEvents gbRepaymentCalc As System.Windows.Forms.GroupBox
    Friend WithEvents tbRepay5Year As System.Windows.Forms.TextBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents tbRepay3Year As System.Windows.Forms.TextBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents tbRepay30Day As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents tbRepay25Year As System.Windows.Forms.TextBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents tbRepay10Year As System.Windows.Forms.TextBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents tbRepay7Year As System.Windows.Forms.TextBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents tbPayOffAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents tbPayOffDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbDPA As System.Windows.Forms.TextBox
    Friend WithEvents gbCC As System.Windows.Forms.GroupBox
    Friend WithEvents tbCCCmts As System.Windows.Forms.TextBox
    Friend WithEvents tbCCLtrID As System.Windows.Forms.TextBox
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents cbCCRea As System.Windows.Forms.ComboBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents cbCCCat As System.Windows.Forms.ComboBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Protected Friend WithEvents tbActionCode As System.Windows.Forms.TextBox
    Protected Friend WithEvents lblActionCode As System.Windows.Forms.Label
    Friend WithEvents tsbtnLoanDetail As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents lstCallF As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents tbLastPaymentDt As System.Windows.Forms.TextBox
    Friend WithEvents tbLastPaymentAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents lblPrimary2 As System.Windows.Forms.Label
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents Label81 As System.Windows.Forms.Label
    Friend WithEvents Label79 As System.Windows.Forms.Label
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents lblPrimary1 As System.Windows.Forms.Label
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents tbGarnish As System.Windows.Forms.TextBox
    Friend WithEvents tbDay30Notice As System.Windows.Forms.TextBox
    Friend WithEvents tbPrimaryAction As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents tbAJOutstanding As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents tbCertified As System.Windows.Forms.TextBox
    Friend WithEvents tbTaxOffset As System.Windows.Forms.TextBox
    Friend WithEvents tbAJOutstandingDt As System.Windows.Forms.TextBox
    Friend WithEvents tbTaxOffsetAmout As System.Windows.Forms.TextBox
    Friend WithEvents tb1098Dt As System.Windows.Forms.TextBox
    Friend WithEvents tb1098 As System.Windows.Forms.TextBox
    Friend WithEvents tbYearsOffset As System.Windows.Forms.TextBox
    Friend WithEvents tbPrincipalC As System.Windows.Forms.TextBox
    Friend WithEvents Label86 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents lblLine As System.Windows.Forms.Label
    Friend WithEvents tbPrincipalP As System.Windows.Forms.TextBox
    Friend WithEvents tbLegalR As System.Windows.Forms.TextBox
    Friend WithEvents tbLegalP As System.Windows.Forms.TextBox
    Friend WithEvents tbLegalC As System.Windows.Forms.TextBox
    Friend WithEvents tbInterestR As System.Windows.Forms.TextBox
    Friend WithEvents tbInterestP As System.Windows.Forms.TextBox
    Friend WithEvents tbInterestC As System.Windows.Forms.TextBox
    Friend WithEvents tbPrincipalR As System.Windows.Forms.TextBox
    Friend WithEvents tbCollectionR As System.Windows.Forms.TextBox
    Friend WithEvents tbCollectionP As System.Windows.Forms.TextBox
    Friend WithEvents tbCollectionC As System.Windows.Forms.TextBox
    Friend WithEvents tbOtherR As System.Windows.Forms.TextBox
    Friend WithEvents tbOtherP As System.Windows.Forms.TextBox
    Friend WithEvents tbOtherC As System.Windows.Forms.TextBox
    Friend WithEvents tbTotalR As System.Windows.Forms.TextBox
    Friend WithEvents tbTotalP As System.Windows.Forms.TextBox
    Friend WithEvents tbTotalC As System.Windows.Forms.TextBox
    Friend WithEvents tbProjectedR As System.Windows.Forms.TextBox
    Friend WithEvents tbProjectedC As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents lstLC41 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents tbReinstatementEligibilityCode As System.Windows.Forms.TextBox
    Friend WithEvents tbCollectionInd As System.Windows.Forms.TextBox
    Friend WithEvents tbIneligibleForRehabCode As System.Windows.Forms.TextBox
    Friend WithEvents tbRehabCounter As System.Windows.Forms.TextBox
    Friend WithEvents tbReinstatementDate As System.Windows.Forms.TextBox
    Friend WithEvents tbLoanPg As System.Windows.Forms.TextBox
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents tbPendingPayments As System.Windows.Forms.TextBox
    Friend WithEvents lstServicer As System.Windows.Forms.ListBox
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbRehabCode As System.Windows.Forms.TextBox
    Friend WithEvents Label63 As System.Windows.Forms.Label
End Class
