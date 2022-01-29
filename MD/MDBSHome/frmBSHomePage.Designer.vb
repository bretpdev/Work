Partial Public Class frmBSHomePage
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblAddr1 As System.Windows.Forms.Label
    Friend WithEvents lblAddr2 As System.Windows.Forms.Label
    Friend WithEvents lblAddr3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents btnLegalAddHist As System.Windows.Forms.Button
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents lblHome As System.Windows.Forms.Label
    Friend WithEvents lblWork As System.Windows.Forms.Label
    Friend WithEvents lblAlter As System.Windows.Forms.Label
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnCompassLoans As System.Windows.Forms.Button
    Friend WithEvents btnDefForbHist As System.Windows.Forms.Button
    Friend WithEvents btnOneLinkLoans As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnAskDUDE As System.Windows.Forms.Button
    Friend WithEvents btnDUDE As System.Windows.Forms.Button
    Friend WithEvents btn411 As System.Windows.Forms.Button
    Friend WithEvents lblAmountPastDue As System.Windows.Forms.Label
    Friend WithEvents lblTotalAmountDue As System.Windows.Forms.Label
    Friend WithEvents lblLateFees As System.Windows.Forms.Label
    Friend WithEvents lblNumDaysDelinquent As System.Windows.Forms.Label
    Friend WithEvents btnSelect As System.Windows.Forms.Button
    Friend WithEvents lblCurrentAmountDue As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblContactType As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblSSN As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnUpdateDemo As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents gbBorrowerStatus As System.Windows.Forms.GroupBox
    Friend WithEvents gbPaymentInformation As System.Windows.Forms.GroupBox
    Friend WithEvents gbAccountHistory As System.Windows.Forms.GroupBox
    Friend WithEvents txtBSHome As System.Windows.Forms.RichTextBox
    Friend WithEvents btnstatus As System.Windows.Forms.Button
    Friend WithEvents lstCallF As System.Windows.Forms.ListBox
    Friend WithEvents lblnum20day As System.Windows.Forms.Label
    Friend WithEvents lbltitlenum20day As System.Windows.Forms.Label
    Friend WithEvents lblPrin As System.Windows.Forms.Label
    Friend WithEvents lblInter As System.Windows.Forms.Label
    Friend WithEvents lblDueDt As System.Windows.Forms.Label
    Friend WithEvents lblLastPmt As System.Windows.Forms.Label
    Friend WithEvents lblMonthlyPmt As System.Windows.Forms.Label
    Friend WithEvents lblDirectDebit As System.Windows.Forms.Label
    Friend WithEvents lblDDDate As System.Windows.Forms.Label
    Friend WithEvents btnPayHist As System.Windows.Forms.Button
    Friend WithEvents lblDtLastCntct As System.Windows.Forms.Label
    Friend WithEvents lblDtAttempt As System.Windows.Forms.Label
    Friend WithEvents btnAddComments As System.Windows.Forms.Button
    Friend WithEvents lstServicer As System.Windows.Forms.ListBox
    Friend WithEvents lvSS As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnTimesDelinquent As System.Windows.Forms.Button
    Friend WithEvents btnLateFeesWaived As System.Windows.Forms.Button
    Friend WithEvents btn360d As System.Windows.Forms.Button
    Friend WithEvents btn90D As System.Windows.Forms.Button
    Friend WithEvents btn30D As System.Windows.Forms.Button
    Friend WithEvents btn180d As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents lblLastPmtAmount As System.Windows.Forms.Label
    Friend WithEvents lblPayOffAmount As System.Windows.Forms.Label
    Friend WithEvents lblDailyInterest As System.Windows.Forms.Label
    Friend WithEvents lblPastDueAmt As System.Windows.Forms.Label
    Friend WithEvents lblLateFeesDue As System.Windows.Forms.Label
    Friend WithEvents pbUp As System.Windows.Forms.PictureBox
    Friend WithEvents pbDown As System.Windows.Forms.PictureBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnDirectDebit As System.Windows.Forms.Button
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents lblDueDay As System.Windows.Forms.Label
    Friend WithEvents btnSendConsolApp As System.Windows.Forms.Button
    Friend WithEvents gbBorrowerActivityHistory As System.Windows.Forms.GroupBox
    Friend WithEvents lblLoanHistory As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblDateOfDelinquency As System.Windows.Forms.Label
    Friend WithEvents btnBorrowerBenefits As System.Windows.Forms.Button
    Friend WithEvents btnBrightIdea As System.Windows.Forms.Button
    Friend WithEvents btnUnexpected As System.Windows.Forms.Button
    Friend WithEvents lblAN As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents lblAddrVal As System.Windows.Forms.Label
    Friend WithEvents lblHPhoneVal As System.Windows.Forms.Label
    Friend WithEvents lblOPhoneVal As System.Windows.Forms.Label
    Friend WithEvents lblO2PhoneVal As System.Windows.Forms.Label
    Friend WithEvents lblEmailVal As System.Windows.Forms.Label
    Friend WithEvents lblVerified As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents lblLoanPg As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents lblCohort As System.Windows.Forms.Label
    Friend WithEvents txtPayOffDate As System.Windows.Forms.TextBox
    Friend WithEvents lblDOB As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents btnThrdParty As System.Windows.Forms.Button
    Friend WithEvents lblRepurch As System.Windows.Forms.Label
    Friend WithEvents lblRehab As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents cbCCCat As System.Windows.Forms.ComboBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents tbCCLtrID As System.Windows.Forms.TextBox
    Friend WithEvents tbCCCmts As System.Windows.Forms.TextBox
    Friend WithEvents gbCC As System.Windows.Forms.GroupBox
    Friend WithEvents pbSurf As System.Windows.Forms.PictureBox
    Friend WithEvents cbCCRea As System.Windows.Forms.ComboBox
    Friend WithEvents rbActHistCOMPASS As System.Windows.Forms.RadioButton
    Friend WithEvents rbActHistOneLINK As System.Windows.Forms.RadioButton
    Friend WithEvents ActHistAttempt As System.Windows.Forms.Label
    Friend WithEvents ActHistDateLastContact As System.Windows.Forms.Label
    Friend WithEvents ActHistSummary As System.Windows.Forms.Label
    Friend WithEvents lblVIPOrSH As System.Windows.Forms.Label
    Friend WithEvents btnEmpInfo As System.Windows.Forms.Button
    Friend WithEvents btnPrivateLoans As System.Windows.Forms.Button
    Friend WithEvents btnContact As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBSHomePage))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnSendConsolApp = New System.Windows.Forms.Button
        Me.lvSS = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.btnSelect = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.btnEmpInfo = New System.Windows.Forms.Button
        Me.btnThrdParty = New System.Windows.Forms.Button
        Me.lblVerified = New System.Windows.Forms.Label
        Me.lblEmailVal = New System.Windows.Forms.Label
        Me.lblO2PhoneVal = New System.Windows.Forms.Label
        Me.lblOPhoneVal = New System.Windows.Forms.Label
        Me.lblHPhoneVal = New System.Windows.Forms.Label
        Me.lblAddrVal = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.pbDown = New System.Windows.Forms.PictureBox
        Me.pbUp = New System.Windows.Forms.PictureBox
        Me.lblEmail = New System.Windows.Forms.Label
        Me.lblAlter = New System.Windows.Forms.Label
        Me.lblWork = New System.Windows.Forms.Label
        Me.lblHome = New System.Windows.Forms.Label
        Me.btnLegalAddHist = New System.Windows.Forms.Button
        Me.btnUpdateDemo = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblAddr3 = New System.Windows.Forms.Label
        Me.lblAddr2 = New System.Windows.Forms.Label
        Me.lblAddr1 = New System.Windows.Forms.Label
        Me.gbBorrowerStatus = New System.Windows.Forms.GroupBox
        Me.btnPrivateLoans = New System.Windows.Forms.Button
        Me.lblRehab = New System.Windows.Forms.Label
        Me.lblRepurch = New System.Windows.Forms.Label
        Me.lblCohort = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.lblLoanPg = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.lblDateOfDelinquency = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.lstServicer = New System.Windows.Forms.ListBox
        Me.lstCallF = New System.Windows.Forms.ListBox
        Me.btnstatus = New System.Windows.Forms.Button
        Me.lblTotal = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.lblCurrentAmountDue = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.lblNumDaysDelinquent = New System.Windows.Forms.Label
        Me.lblnum20day = New System.Windows.Forms.Label
        Me.lblLateFees = New System.Windows.Forms.Label
        Me.lblTotalAmountDue = New System.Windows.Forms.Label
        Me.lblAmountPastDue = New System.Windows.Forms.Label
        Me.btnOneLinkLoans = New System.Windows.Forms.Button
        Me.btnDefForbHist = New System.Windows.Forms.Button
        Me.btnCompassLoans = New System.Windows.Forms.Button
        Me.lbltitlenum20day = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblLoanHistory = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.gbPaymentInformation = New System.Windows.Forms.GroupBox
        Me.txtPayOffDate = New System.Windows.Forms.TextBox
        Me.lblDueDay = New System.Windows.Forms.Label
        Me.Label30 = New System.Windows.Forms.Label
        Me.lblLateFeesDue = New System.Windows.Forms.Label
        Me.lblPastDueAmt = New System.Windows.Forms.Label
        Me.lblDailyInterest = New System.Windows.Forms.Label
        Me.lblPayOffAmount = New System.Windows.Forms.Label
        Me.lblLastPmtAmount = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.Label27 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.btnContact = New System.Windows.Forms.Button
        Me.btnPayHist = New System.Windows.Forms.Button
        Me.lblMonthlyPmt = New System.Windows.Forms.Label
        Me.lblLastPmt = New System.Windows.Forms.Label
        Me.lblDueDt = New System.Windows.Forms.Label
        Me.lblInter = New System.Windows.Forms.Label
        Me.lblPrin = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblDDDate = New System.Windows.Forms.Label
        Me.lblDirectDebit = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.btnDirectDebit = New System.Windows.Forms.Button
        Me.gbBorrowerActivityHistory = New System.Windows.Forms.GroupBox
        Me.rbActHistOneLINK = New System.Windows.Forms.RadioButton
        Me.rbActHistCOMPASS = New System.Windows.Forms.RadioButton
        Me.btn360d = New System.Windows.Forms.Button
        Me.btn180d = New System.Windows.Forms.Button
        Me.btn90D = New System.Windows.Forms.Button
        Me.btn30D = New System.Windows.Forms.Button
        Me.btnAddComments = New System.Windows.Forms.Button
        Me.lblDtAttempt = New System.Windows.Forms.Label
        Me.lblDtLastCntct = New System.Windows.Forms.Label
        Me.ActHistAttempt = New System.Windows.Forms.Label
        Me.ActHistSummary = New System.Windows.Forms.Label
        Me.ActHistDateLastContact = New System.Windows.Forms.Label
        Me.gbAccountHistory = New System.Windows.Forms.GroupBox
        Me.btnBorrowerBenefits = New System.Windows.Forms.Button
        Me.btnTimesDelinquent = New System.Windows.Forms.Button
        Me.btnLateFeesWaived = New System.Windows.Forms.Button
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.btnAskDUDE = New System.Windows.Forms.Button
        Me.btnDUDE = New System.Windows.Forms.Button
        Me.btn411 = New System.Windows.Forms.Button
        Me.lblContactType = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblSSN = New System.Windows.Forms.Label
        Me.btnRefresh = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.txtBSHome = New System.Windows.Forms.RichTextBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnBrightIdea = New System.Windows.Forms.Button
        Me.btnUnexpected = New System.Windows.Forms.Button
        Me.lblAN = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.lblDOB = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.gbCC = New System.Windows.Forms.GroupBox
        Me.tbCCCmts = New System.Windows.Forms.TextBox
        Me.tbCCLtrID = New System.Windows.Forms.TextBox
        Me.Label38 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.cbCCRea = New System.Windows.Forms.ComboBox
        Me.Label36 = New System.Windows.Forms.Label
        Me.Label34 = New System.Windows.Forms.Label
        Me.cbCCCat = New System.Windows.Forms.ComboBox
        Me.pbSurf = New System.Windows.Forms.PictureBox
        Me.lblVIPOrSH = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.pbDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbUp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbBorrowerStatus.SuspendLayout()
        Me.gbPaymentInformation.SuspendLayout()
        Me.gbBorrowerActivityHistory.SuspendLayout()
        Me.gbAccountHistory.SuspendLayout()
        Me.gbCC.SuspendLayout()
        CType(Me.pbSurf, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnSendConsolApp)
        Me.GroupBox1.Controls.Add(Me.lvSS)
        Me.GroupBox1.Controls.Add(Me.btnSelect)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 72)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(300, 312)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Scripts and Services"
        '
        'btnSendConsolApp
        '
        Me.btnSendConsolApp.Enabled = False
        Me.btnSendConsolApp.Location = New System.Drawing.Point(8, 24)
        Me.btnSendConsolApp.Name = "btnSendConsolApp"
        Me.btnSendConsolApp.Size = New System.Drawing.Size(192, 23)
        Me.btnSendConsolApp.TabIndex = 3
        Me.btnSendConsolApp.Text = "Send Consolidation Application"
        Me.btnSendConsolApp.Visible = False
        '
        'lvSS
        '
        Me.lvSS.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvSS.FullRowSelect = True
        Me.lvSS.Location = New System.Drawing.Point(8, 56)
        Me.lvSS.MultiSelect = False
        Me.lvSS.Name = "lvSS"
        Me.lvSS.Size = New System.Drawing.Size(280, 240)
        Me.lvSS.TabIndex = 2
        Me.lvSS.UseCompatibleStateImageBehavior = False
        Me.lvSS.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Script Name"
        Me.ColumnHeader1.Width = 145
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Status"
        Me.ColumnHeader2.Width = 130
        '
        'btnSelect
        '
        Me.btnSelect.Location = New System.Drawing.Point(208, 24)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(75, 23)
        Me.btnSelect.TabIndex = 1
        Me.btnSelect.Text = "Select"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnEmpInfo)
        Me.GroupBox2.Controls.Add(Me.btnThrdParty)
        Me.GroupBox2.Controls.Add(Me.lblVerified)
        Me.GroupBox2.Controls.Add(Me.lblEmailVal)
        Me.GroupBox2.Controls.Add(Me.lblO2PhoneVal)
        Me.GroupBox2.Controls.Add(Me.lblOPhoneVal)
        Me.GroupBox2.Controls.Add(Me.lblHPhoneVal)
        Me.GroupBox2.Controls.Add(Me.lblAddrVal)
        Me.GroupBox2.Controls.Add(Me.Label31)
        Me.GroupBox2.Controls.Add(Me.pbDown)
        Me.GroupBox2.Controls.Add(Me.pbUp)
        Me.GroupBox2.Controls.Add(Me.lblEmail)
        Me.GroupBox2.Controls.Add(Me.lblAlter)
        Me.GroupBox2.Controls.Add(Me.lblWork)
        Me.GroupBox2.Controls.Add(Me.lblHome)
        Me.GroupBox2.Controls.Add(Me.btnLegalAddHist)
        Me.GroupBox2.Controls.Add(Me.btnUpdateDemo)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.lblAddr3)
        Me.GroupBox2.Controls.Add(Me.lblAddr2)
        Me.GroupBox2.Controls.Add(Me.lblAddr1)
        Me.GroupBox2.Location = New System.Drawing.Point(320, 72)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(264, 312)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Borrower Demos"
        '
        'btnEmpInfo
        '
        Me.btnEmpInfo.Location = New System.Drawing.Point(134, 228)
        Me.btnEmpInfo.Name = "btnEmpInfo"
        Me.btnEmpInfo.Size = New System.Drawing.Size(122, 20)
        Me.btnEmpInfo.TabIndex = 23
        Me.btnEmpInfo.Text = "Employee Info"
        '
        'btnThrdParty
        '
        Me.btnThrdParty.Location = New System.Drawing.Point(200, 256)
        Me.btnThrdParty.Name = "btnThrdParty"
        Me.btnThrdParty.Size = New System.Drawing.Size(56, 40)
        Me.btnThrdParty.TabIndex = 22
        Me.btnThrdParty.Text = "3rd Party"
        '
        'lblVerified
        '
        Me.lblVerified.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVerified.Location = New System.Drawing.Point(8, 24)
        Me.lblVerified.Name = "lblVerified"
        Me.lblVerified.Size = New System.Drawing.Size(248, 16)
        Me.lblVerified.TabIndex = 21
        Me.lblVerified.Text = "Not Verified"
        Me.lblVerified.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblEmailVal
        '
        Me.lblEmailVal.Location = New System.Drawing.Point(240, 184)
        Me.lblEmailVal.Name = "lblEmailVal"
        Me.lblEmailVal.Size = New System.Drawing.Size(16, 16)
        Me.lblEmailVal.TabIndex = 20
        Me.lblEmailVal.Text = "N"
        '
        'lblO2PhoneVal
        '
        Me.lblO2PhoneVal.Location = New System.Drawing.Point(240, 160)
        Me.lblO2PhoneVal.Name = "lblO2PhoneVal"
        Me.lblO2PhoneVal.Size = New System.Drawing.Size(16, 16)
        Me.lblO2PhoneVal.TabIndex = 19
        Me.lblO2PhoneVal.Text = "N"
        '
        'lblOPhoneVal
        '
        Me.lblOPhoneVal.Location = New System.Drawing.Point(240, 136)
        Me.lblOPhoneVal.Name = "lblOPhoneVal"
        Me.lblOPhoneVal.Size = New System.Drawing.Size(16, 16)
        Me.lblOPhoneVal.TabIndex = 18
        Me.lblOPhoneVal.Text = "N"
        '
        'lblHPhoneVal
        '
        Me.lblHPhoneVal.Location = New System.Drawing.Point(240, 112)
        Me.lblHPhoneVal.Name = "lblHPhoneVal"
        Me.lblHPhoneVal.Size = New System.Drawing.Size(16, 16)
        Me.lblHPhoneVal.TabIndex = 17
        Me.lblHPhoneVal.Text = "N"
        '
        'lblAddrVal
        '
        Me.lblAddrVal.Location = New System.Drawing.Point(240, 88)
        Me.lblAddrVal.Name = "lblAddrVal"
        Me.lblAddrVal.Size = New System.Drawing.Size(16, 16)
        Me.lblAddrVal.TabIndex = 16
        Me.lblAddrVal.Text = "N"
        '
        'Label31
        '
        Me.Label31.Location = New System.Drawing.Point(232, 56)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(24, 16)
        Me.Label31.TabIndex = 15
        Me.Label31.Text = "Ver:"
        '
        'pbDown
        '
        Me.pbDown.Image = CType(resources.GetObject("pbDown.Image"), System.Drawing.Image)
        Me.pbDown.Location = New System.Drawing.Point(72, 248)
        Me.pbDown.Name = "pbDown"
        Me.pbDown.Size = New System.Drawing.Size(56, 56)
        Me.pbDown.TabIndex = 14
        Me.pbDown.TabStop = False
        Me.pbDown.Visible = False
        '
        'pbUp
        '
        Me.pbUp.Image = CType(resources.GetObject("pbUp.Image"), System.Drawing.Image)
        Me.pbUp.Location = New System.Drawing.Point(72, 248)
        Me.pbUp.Name = "pbUp"
        Me.pbUp.Size = New System.Drawing.Size(56, 56)
        Me.pbUp.TabIndex = 13
        Me.pbUp.TabStop = False
        '
        'lblEmail
        '
        Me.lblEmail.Location = New System.Drawing.Point(48, 184)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(184, 32)
        Me.lblEmail.TabIndex = 12
        '
        'lblAlter
        '
        Me.lblAlter.Location = New System.Drawing.Point(48, 160)
        Me.lblAlter.Name = "lblAlter"
        Me.lblAlter.Size = New System.Drawing.Size(184, 16)
        Me.lblAlter.TabIndex = 11
        '
        'lblWork
        '
        Me.lblWork.Location = New System.Drawing.Point(48, 136)
        Me.lblWork.Name = "lblWork"
        Me.lblWork.Size = New System.Drawing.Size(184, 16)
        Me.lblWork.TabIndex = 10
        '
        'lblHome
        '
        Me.lblHome.Location = New System.Drawing.Point(48, 112)
        Me.lblHome.Name = "lblHome"
        Me.lblHome.Size = New System.Drawing.Size(184, 16)
        Me.lblHome.TabIndex = 9
        '
        'btnLegalAddHist
        '
        Me.btnLegalAddHist.Location = New System.Drawing.Point(8, 256)
        Me.btnLegalAddHist.Name = "btnLegalAddHist"
        Me.btnLegalAddHist.Size = New System.Drawing.Size(64, 40)
        Me.btnLegalAddHist.TabIndex = 4
        Me.btnLegalAddHist.Text = "Legal Add History"
        '
        'btnUpdateDemo
        '
        Me.btnUpdateDemo.Location = New System.Drawing.Point(136, 256)
        Me.btnUpdateDemo.Name = "btnUpdateDemo"
        Me.btnUpdateDemo.Size = New System.Drawing.Size(56, 40)
        Me.btnUpdateDemo.TabIndex = 3
        Me.btnUpdateDemo.Text = "Update Demos"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 184)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 16)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Email:"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 160)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Alter:"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 136)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 16)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Work:"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 112)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 16)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Home:"
        '
        'lblAddr3
        '
        Me.lblAddr3.Location = New System.Drawing.Point(8, 88)
        Me.lblAddr3.Name = "lblAddr3"
        Me.lblAddr3.Size = New System.Drawing.Size(224, 16)
        Me.lblAddr3.TabIndex = 2
        Me.lblAddr3.Text = "lblAddr3"
        '
        'lblAddr2
        '
        Me.lblAddr2.Location = New System.Drawing.Point(8, 72)
        Me.lblAddr2.Name = "lblAddr2"
        Me.lblAddr2.Size = New System.Drawing.Size(224, 16)
        Me.lblAddr2.TabIndex = 1
        Me.lblAddr2.Text = "lblAddr2"
        '
        'lblAddr1
        '
        Me.lblAddr1.Location = New System.Drawing.Point(8, 56)
        Me.lblAddr1.Name = "lblAddr1"
        Me.lblAddr1.Size = New System.Drawing.Size(224, 16)
        Me.lblAddr1.TabIndex = 0
        Me.lblAddr1.Text = "lblAddr1"
        '
        'gbBorrowerStatus
        '
        Me.gbBorrowerStatus.Controls.Add(Me.btnPrivateLoans)
        Me.gbBorrowerStatus.Controls.Add(Me.lblRehab)
        Me.gbBorrowerStatus.Controls.Add(Me.lblRepurch)
        Me.gbBorrowerStatus.Controls.Add(Me.lblCohort)
        Me.gbBorrowerStatus.Controls.Add(Me.Label33)
        Me.gbBorrowerStatus.Controls.Add(Me.lblLoanPg)
        Me.gbBorrowerStatus.Controls.Add(Me.Label32)
        Me.gbBorrowerStatus.Controls.Add(Me.lblDateOfDelinquency)
        Me.gbBorrowerStatus.Controls.Add(Me.Label11)
        Me.gbBorrowerStatus.Controls.Add(Me.lstServicer)
        Me.gbBorrowerStatus.Controls.Add(Me.lstCallF)
        Me.gbBorrowerStatus.Controls.Add(Me.btnstatus)
        Me.gbBorrowerStatus.Controls.Add(Me.lblTotal)
        Me.gbBorrowerStatus.Controls.Add(Me.Label17)
        Me.gbBorrowerStatus.Controls.Add(Me.lblCurrentAmountDue)
        Me.gbBorrowerStatus.Controls.Add(Me.Label18)
        Me.gbBorrowerStatus.Controls.Add(Me.lblNumDaysDelinquent)
        Me.gbBorrowerStatus.Controls.Add(Me.lblnum20day)
        Me.gbBorrowerStatus.Controls.Add(Me.lblLateFees)
        Me.gbBorrowerStatus.Controls.Add(Me.lblTotalAmountDue)
        Me.gbBorrowerStatus.Controls.Add(Me.lblAmountPastDue)
        Me.gbBorrowerStatus.Controls.Add(Me.btnOneLinkLoans)
        Me.gbBorrowerStatus.Controls.Add(Me.btnDefForbHist)
        Me.gbBorrowerStatus.Controls.Add(Me.btnCompassLoans)
        Me.gbBorrowerStatus.Controls.Add(Me.lbltitlenum20day)
        Me.gbBorrowerStatus.Controls.Add(Me.Label13)
        Me.gbBorrowerStatus.Controls.Add(Me.lblStatus)
        Me.gbBorrowerStatus.Controls.Add(Me.lblLoanHistory)
        Me.gbBorrowerStatus.Controls.Add(Me.Label10)
        Me.gbBorrowerStatus.Controls.Add(Me.Label8)
        Me.gbBorrowerStatus.Controls.Add(Me.Label3)
        Me.gbBorrowerStatus.Controls.Add(Me.Label2)
        Me.gbBorrowerStatus.Controls.Add(Me.Label1)
        Me.gbBorrowerStatus.Location = New System.Drawing.Point(592, 72)
        Me.gbBorrowerStatus.Name = "gbBorrowerStatus"
        Me.gbBorrowerStatus.Size = New System.Drawing.Size(376, 312)
        Me.gbBorrowerStatus.TabIndex = 2
        Me.gbBorrowerStatus.TabStop = False
        Me.gbBorrowerStatus.Text = "Borrower Status"
        '
        'btnPrivateLoans
        '
        Me.btnPrivateLoans.Location = New System.Drawing.Point(294, 135)
        Me.btnPrivateLoans.Name = "btnPrivateLoans"
        Me.btnPrivateLoans.Size = New System.Drawing.Size(72, 32)
        Me.btnPrivateLoans.TabIndex = 34
        Me.btnPrivateLoans.Text = "Private Loans"
        '
        'lblRehab
        '
        Me.lblRehab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRehab.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRehab.Location = New System.Drawing.Point(208, 280)
        Me.lblRehab.Name = "lblRehab"
        Me.lblRehab.Size = New System.Drawing.Size(160, 23)
        Me.lblRehab.TabIndex = 33
        Me.lblRehab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRepurch
        '
        Me.lblRepurch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRepurch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRepurch.Location = New System.Drawing.Point(16, 280)
        Me.lblRepurch.Name = "lblRepurch"
        Me.lblRepurch.Size = New System.Drawing.Size(160, 23)
        Me.lblRepurch.TabIndex = 32
        Me.lblRepurch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCohort
        '
        Me.lblCohort.Location = New System.Drawing.Point(104, 232)
        Me.lblCohort.Name = "lblCohort"
        Me.lblCohort.Size = New System.Drawing.Size(184, 16)
        Me.lblCohort.TabIndex = 31
        '
        'Label33
        '
        Me.Label33.Location = New System.Drawing.Point(16, 232)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(88, 16)
        Me.Label33.TabIndex = 30
        Me.Label33.Text = "COHORT Year:"
        '
        'lblLoanPg
        '
        Me.lblLoanPg.Location = New System.Drawing.Point(104, 256)
        Me.lblLoanPg.Name = "lblLoanPg"
        Me.lblLoanPg.Size = New System.Drawing.Size(184, 16)
        Me.lblLoanPg.TabIndex = 29
        '
        'Label32
        '
        Me.Label32.Location = New System.Drawing.Point(16, 256)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(80, 16)
        Me.Label32.TabIndex = 28
        Me.Label32.Text = "Loan Program:"
        '
        'lblDateOfDelinquency
        '
        Me.lblDateOfDelinquency.Location = New System.Drawing.Point(312, 47)
        Me.lblDateOfDelinquency.Name = "lblDateOfDelinquency"
        Me.lblDateOfDelinquency.Size = New System.Drawing.Size(56, 16)
        Me.lblDateOfDelinquency.TabIndex = 27
        Me.lblDateOfDelinquency.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(200, 47)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(112, 16)
        Me.Label11.TabIndex = 26
        Me.Label11.Text = "Date of Delinquency:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lstServicer
        '
        Me.lstServicer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstServicer.Location = New System.Drawing.Point(104, 200)
        Me.lstServicer.Name = "lstServicer"
        Me.lstServicer.Size = New System.Drawing.Size(184, 26)
        Me.lstServicer.TabIndex = 25
        Me.lstServicer.TabStop = False
        '
        'lstCallF
        '
        Me.lstCallF.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstCallF.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstCallF.Location = New System.Drawing.Point(104, 176)
        Me.lstCallF.Name = "lstCallF"
        Me.lstCallF.Size = New System.Drawing.Size(184, 26)
        Me.lstCallF.TabIndex = 24
        Me.lstCallF.TabStop = False
        '
        'btnstatus
        '
        Me.btnstatus.Location = New System.Drawing.Point(296, 16)
        Me.btnstatus.Name = "btnstatus"
        Me.btnstatus.Size = New System.Drawing.Size(72, 24)
        Me.btnstatus.TabIndex = 5
        Me.btnstatus.Text = "Mo' Status"
        '
        'lblTotal
        '
        Me.lblTotal.Location = New System.Drawing.Point(112, 143)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(80, 16)
        Me.lblTotal.TabIndex = 22
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(16, 143)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(100, 16)
        Me.Label17.TabIndex = 21
        Me.Label17.Text = "Total+Late Fees:"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCurrentAmountDue
        '
        Me.lblCurrentAmountDue.Location = New System.Drawing.Point(128, 95)
        Me.lblCurrentAmountDue.Name = "lblCurrentAmountDue"
        Me.lblCurrentAmountDue.Size = New System.Drawing.Size(64, 16)
        Me.lblCurrentAmountDue.TabIndex = 20
        Me.lblCurrentAmountDue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(16, 95)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(120, 16)
        Me.Label18.TabIndex = 19
        Me.Label18.Text = "Current Amount Due:"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblNumDaysDelinquent
        '
        Me.lblNumDaysDelinquent.Location = New System.Drawing.Point(136, 47)
        Me.lblNumDaysDelinquent.Name = "lblNumDaysDelinquent"
        Me.lblNumDaysDelinquent.Size = New System.Drawing.Size(56, 16)
        Me.lblNumDaysDelinquent.TabIndex = 17
        Me.lblNumDaysDelinquent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblnum20day
        '
        Me.lblnum20day.Location = New System.Drawing.Point(320, 95)
        Me.lblnum20day.Name = "lblnum20day"
        Me.lblnum20day.Size = New System.Drawing.Size(48, 24)
        Me.lblnum20day.TabIndex = 16
        Me.lblnum20day.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLateFees
        '
        Me.lblLateFees.Location = New System.Drawing.Point(312, 71)
        Me.lblLateFees.Name = "lblLateFees"
        Me.lblLateFees.Size = New System.Drawing.Size(56, 24)
        Me.lblLateFees.TabIndex = 15
        Me.lblLateFees.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTotalAmountDue
        '
        Me.lblTotalAmountDue.Location = New System.Drawing.Point(120, 119)
        Me.lblTotalAmountDue.Name = "lblTotalAmountDue"
        Me.lblTotalAmountDue.Size = New System.Drawing.Size(72, 16)
        Me.lblTotalAmountDue.TabIndex = 14
        Me.lblTotalAmountDue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblAmountPastDue
        '
        Me.lblAmountPastDue.Location = New System.Drawing.Point(120, 71)
        Me.lblAmountPastDue.Name = "lblAmountPastDue"
        Me.lblAmountPastDue.Size = New System.Drawing.Size(72, 16)
        Me.lblAmountPastDue.TabIndex = 13
        Me.lblAmountPastDue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnOneLinkLoans
        '
        Me.btnOneLinkLoans.Location = New System.Drawing.Point(294, 240)
        Me.btnOneLinkLoans.Name = "btnOneLinkLoans"
        Me.btnOneLinkLoans.Size = New System.Drawing.Size(72, 32)
        Me.btnOneLinkLoans.TabIndex = 8
        Me.btnOneLinkLoans.Text = "OneLINK Loans"
        '
        'btnDefForbHist
        '
        Me.btnDefForbHist.Location = New System.Drawing.Point(294, 205)
        Me.btnDefForbHist.Name = "btnDefForbHist"
        Me.btnDefForbHist.Size = New System.Drawing.Size(72, 32)
        Me.btnDefForbHist.TabIndex = 7
        Me.btnDefForbHist.Text = "Defer/Forb History"
        '
        'btnCompassLoans
        '
        Me.btnCompassLoans.Location = New System.Drawing.Point(294, 170)
        Me.btnCompassLoans.Name = "btnCompassLoans"
        Me.btnCompassLoans.Size = New System.Drawing.Size(72, 32)
        Me.btnCompassLoans.TabIndex = 6
        Me.btnCompassLoans.Text = "Compass Loans"
        '
        'lbltitlenum20day
        '
        Me.lbltitlenum20day.Location = New System.Drawing.Point(200, 95)
        Me.lbltitlenum20day.Name = "lbltitlenum20day"
        Me.lbltitlenum20day.Size = New System.Drawing.Size(120, 23)
        Me.lbltitlenum20day.TabIndex = 9
        Me.lbltitlenum20day.Text = "# Times 20-Day Letter"
        Me.lbltitlenum20day.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(248, 71)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(64, 23)
        Me.Label13.TabIndex = 8
        Me.Label13.Text = "Late Fees:"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblStatus
        '
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(16, 24)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(272, 23)
        Me.lblStatus.TabIndex = 7
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLoanHistory
        '
        Me.lblLoanHistory.Location = New System.Drawing.Point(296, 119)
        Me.lblLoanHistory.Name = "lblLoanHistory"
        Me.lblLoanHistory.Size = New System.Drawing.Size(72, 16)
        Me.lblLoanHistory.TabIndex = 6
        Me.lblLoanHistory.Text = "Loan History"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(16, 200)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(88, 23)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Servicer:"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(16, 176)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(96, 23)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Call Forwarding:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(16, 119)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Total Amount Due:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Amount Past Due:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "# Of Days Delinquent:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbPaymentInformation
        '
        Me.gbPaymentInformation.Controls.Add(Me.txtPayOffDate)
        Me.gbPaymentInformation.Controls.Add(Me.lblDueDay)
        Me.gbPaymentInformation.Controls.Add(Me.Label30)
        Me.gbPaymentInformation.Controls.Add(Me.lblLateFeesDue)
        Me.gbPaymentInformation.Controls.Add(Me.lblPastDueAmt)
        Me.gbPaymentInformation.Controls.Add(Me.lblDailyInterest)
        Me.gbPaymentInformation.Controls.Add(Me.lblPayOffAmount)
        Me.gbPaymentInformation.Controls.Add(Me.lblLastPmtAmount)
        Me.gbPaymentInformation.Controls.Add(Me.Label29)
        Me.gbPaymentInformation.Controls.Add(Me.Label28)
        Me.gbPaymentInformation.Controls.Add(Me.Label27)
        Me.gbPaymentInformation.Controls.Add(Me.Label26)
        Me.gbPaymentInformation.Controls.Add(Me.Label9)
        Me.gbPaymentInformation.Controls.Add(Me.btnContact)
        Me.gbPaymentInformation.Controls.Add(Me.btnPayHist)
        Me.gbPaymentInformation.Controls.Add(Me.lblMonthlyPmt)
        Me.gbPaymentInformation.Controls.Add(Me.lblLastPmt)
        Me.gbPaymentInformation.Controls.Add(Me.lblDueDt)
        Me.gbPaymentInformation.Controls.Add(Me.lblInter)
        Me.gbPaymentInformation.Controls.Add(Me.lblPrin)
        Me.gbPaymentInformation.Controls.Add(Me.Label22)
        Me.gbPaymentInformation.Controls.Add(Me.Label21)
        Me.gbPaymentInformation.Controls.Add(Me.Label20)
        Me.gbPaymentInformation.Controls.Add(Me.Label19)
        Me.gbPaymentInformation.Controls.Add(Me.Label12)
        Me.gbPaymentInformation.Controls.Add(Me.lblDDDate)
        Me.gbPaymentInformation.Controls.Add(Me.lblDirectDebit)
        Me.gbPaymentInformation.Controls.Add(Me.Label23)
        Me.gbPaymentInformation.Controls.Add(Me.btnDirectDebit)
        Me.gbPaymentInformation.Location = New System.Drawing.Point(8, 400)
        Me.gbPaymentInformation.Name = "gbPaymentInformation"
        Me.gbPaymentInformation.Size = New System.Drawing.Size(300, 232)
        Me.gbPaymentInformation.TabIndex = 3
        Me.gbPaymentInformation.TabStop = False
        Me.gbPaymentInformation.Text = "Payment Information"
        '
        'txtPayOffDate
        '
        Me.txtPayOffDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayOffDate.Location = New System.Drawing.Point(152, 100)
        Me.txtPayOffDate.Name = "txtPayOffDate"
        Me.txtPayOffDate.Size = New System.Drawing.Size(68, 18)
        Me.txtPayOffDate.TabIndex = 29
        Me.txtPayOffDate.Text = "01/01/2001"
        '
        'lblDueDay
        '
        Me.lblDueDay.Location = New System.Drawing.Point(92, 120)
        Me.lblDueDay.Name = "lblDueDay"
        Me.lblDueDay.Size = New System.Drawing.Size(200, 16)
        Me.lblDueDay.TabIndex = 27
        Me.lblDueDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label30
        '
        Me.Label30.Location = New System.Drawing.Point(8, 120)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(56, 16)
        Me.Label30.TabIndex = 26
        Me.Label30.Text = "Due Day:"
        '
        'lblLateFeesDue
        '
        Me.lblLateFeesDue.Location = New System.Drawing.Point(92, 56)
        Me.lblLateFeesDue.Name = "lblLateFeesDue"
        Me.lblLateFeesDue.Size = New System.Drawing.Size(60, 16)
        Me.lblLateFeesDue.TabIndex = 25
        Me.lblLateFeesDue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPastDueAmt
        '
        Me.lblPastDueAmt.Location = New System.Drawing.Point(240, 36)
        Me.lblPastDueAmt.Name = "lblPastDueAmt"
        Me.lblPastDueAmt.Size = New System.Drawing.Size(56, 16)
        Me.lblPastDueAmt.TabIndex = 24
        Me.lblPastDueAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDailyInterest
        '
        Me.lblDailyInterest.Location = New System.Drawing.Point(240, 20)
        Me.lblDailyInterest.Name = "lblDailyInterest"
        Me.lblDailyInterest.Size = New System.Drawing.Size(56, 16)
        Me.lblDailyInterest.TabIndex = 23
        Me.lblDailyInterest.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPayOffAmount
        '
        Me.lblPayOffAmount.Location = New System.Drawing.Point(80, 100)
        Me.lblPayOffAmount.Name = "lblPayOffAmount"
        Me.lblPayOffAmount.Size = New System.Drawing.Size(72, 16)
        Me.lblPayOffAmount.TabIndex = 22
        Me.lblPayOffAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLastPmtAmount
        '
        Me.lblLastPmtAmount.Location = New System.Drawing.Point(228, 68)
        Me.lblLastPmtAmount.Name = "lblLastPmtAmount"
        Me.lblLastPmtAmount.Size = New System.Drawing.Size(68, 16)
        Me.lblLastPmtAmount.TabIndex = 21
        Me.lblLastPmtAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label29
        '
        Me.Label29.Location = New System.Drawing.Point(156, 68)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(88, 16)
        Me.Label29.TabIndex = 20
        Me.Label29.Text = "Last Pmt Amt:"
        '
        'Label28
        '
        Me.Label28.Location = New System.Drawing.Point(8, 56)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(84, 16)
        Me.Label28.TabIndex = 19
        Me.Label28.Text = "Late Fees Due:"
        '
        'Label27
        '
        Me.Label27.Location = New System.Drawing.Point(156, 36)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(80, 16)
        Me.Label27.TabIndex = 18
        Me.Label27.Text = "Past Due Amt:"
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(156, 20)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(80, 16)
        Me.Label26.TabIndex = 17
        Me.Label26.Text = "Daily Interest:"
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 100)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(68, 16)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Pay Off Amt:"
        '
        'btnContact
        '
        Me.btnContact.Location = New System.Drawing.Point(216, 192)
        Me.btnContact.Name = "btnContact"
        Me.btnContact.Size = New System.Drawing.Size(75, 32)
        Me.btnContact.TabIndex = 11
        Me.btnContact.Text = "Contact Dialogue"
        '
        'btnPayHist
        '
        Me.btnPayHist.Location = New System.Drawing.Point(12, 192)
        Me.btnPayHist.Name = "btnPayHist"
        Me.btnPayHist.Size = New System.Drawing.Size(75, 32)
        Me.btnPayHist.TabIndex = 9
        Me.btnPayHist.Text = "Payment History"
        '
        'lblMonthlyPmt
        '
        Me.lblMonthlyPmt.Location = New System.Drawing.Point(92, 136)
        Me.lblMonthlyPmt.Name = "lblMonthlyPmt"
        Me.lblMonthlyPmt.Size = New System.Drawing.Size(200, 16)
        Me.lblMonthlyPmt.TabIndex = 10
        Me.lblMonthlyPmt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLastPmt
        '
        Me.lblLastPmt.Location = New System.Drawing.Point(228, 84)
        Me.lblLastPmt.Name = "lblLastPmt"
        Me.lblLastPmt.Size = New System.Drawing.Size(68, 16)
        Me.lblLastPmt.TabIndex = 9
        Me.lblLastPmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDueDt
        '
        Me.lblDueDt.Location = New System.Drawing.Point(232, 52)
        Me.lblDueDt.Name = "lblDueDt"
        Me.lblDueDt.Size = New System.Drawing.Size(64, 16)
        Me.lblDueDt.TabIndex = 8
        Me.lblDueDt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblDueDt.Visible = False
        '
        'lblInter
        '
        Me.lblInter.Location = New System.Drawing.Point(72, 40)
        Me.lblInter.Name = "lblInter"
        Me.lblInter.Size = New System.Drawing.Size(80, 16)
        Me.lblInter.TabIndex = 7
        Me.lblInter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPrin
        '
        Me.lblPrin.Location = New System.Drawing.Point(72, 24)
        Me.lblPrin.Name = "lblPrin"
        Me.lblPrin.Size = New System.Drawing.Size(80, 16)
        Me.lblPrin.TabIndex = 6
        Me.lblPrin.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(8, 136)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(88, 16)
        Me.Label22.TabIndex = 4
        Me.Label22.Text = "Mthly Pymt Amt:"
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(156, 84)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(84, 16)
        Me.Label21.TabIndex = 3
        Me.Label21.Text = "Last Pmt Rcd:"
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(156, 52)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(84, 16)
        Me.Label20.TabIndex = 2
        Me.Label20.Text = "Next Date Due:"
        Me.Label20.Visible = False
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(8, 40)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(96, 16)
        Me.Label19.TabIndex = 1
        Me.Label19.Text = "Interest:"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(8, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(96, 16)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Principal:"
        '
        'lblDDDate
        '
        Me.lblDDDate.Location = New System.Drawing.Point(188, 164)
        Me.lblDDDate.Name = "lblDDDate"
        Me.lblDDDate.Size = New System.Drawing.Size(96, 16)
        Me.lblDDDate.TabIndex = 12
        Me.lblDDDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDirectDebit
        '
        Me.lblDirectDebit.Location = New System.Drawing.Point(92, 164)
        Me.lblDirectDebit.Name = "lblDirectDebit"
        Me.lblDirectDebit.Size = New System.Drawing.Size(56, 16)
        Me.lblDirectDebit.TabIndex = 11
        Me.lblDirectDebit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(8, 164)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(68, 16)
        Me.Label23.TabIndex = 5
        Me.Label23.Text = "Direct Debit:"
        '
        'btnDirectDebit
        '
        Me.btnDirectDebit.Location = New System.Drawing.Point(76, 156)
        Me.btnDirectDebit.Name = "btnDirectDebit"
        Me.btnDirectDebit.Size = New System.Drawing.Size(216, 32)
        Me.btnDirectDebit.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.btnDirectDebit, "Click here to view detailed direct debit information.")
        '
        'gbBorrowerActivityHistory
        '
        Me.gbBorrowerActivityHistory.Controls.Add(Me.rbActHistOneLINK)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.rbActHistCOMPASS)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.btn360d)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.btn180d)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.btn90D)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.btn30D)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.btnAddComments)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.lblDtAttempt)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.lblDtLastCntct)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.ActHistAttempt)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.ActHistSummary)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.ActHistDateLastContact)
        Me.gbBorrowerActivityHistory.Location = New System.Drawing.Point(320, 400)
        Me.gbBorrowerActivityHistory.Name = "gbBorrowerActivityHistory"
        Me.gbBorrowerActivityHistory.Size = New System.Drawing.Size(264, 232)
        Me.gbBorrowerActivityHistory.TabIndex = 4
        Me.gbBorrowerActivityHistory.TabStop = False
        Me.gbBorrowerActivityHistory.Text = "Borrower Activity History"
        '
        'rbActHistOneLINK
        '
        Me.rbActHistOneLINK.Location = New System.Drawing.Point(140, 24)
        Me.rbActHistOneLINK.Name = "rbActHistOneLINK"
        Me.rbActHistOneLINK.Size = New System.Drawing.Size(104, 24)
        Me.rbActHistOneLINK.TabIndex = 18
        Me.rbActHistOneLINK.Text = "OneLINK"
        '
        'rbActHistCOMPASS
        '
        Me.rbActHistCOMPASS.Location = New System.Drawing.Point(8, 24)
        Me.rbActHistCOMPASS.Name = "rbActHistCOMPASS"
        Me.rbActHistCOMPASS.Size = New System.Drawing.Size(104, 24)
        Me.rbActHistCOMPASS.TabIndex = 17
        Me.rbActHistCOMPASS.Text = "COMPASS"
        '
        'btn360d
        '
        Me.btn360d.Location = New System.Drawing.Point(200, 140)
        Me.btn360d.Name = "btn360d"
        Me.btn360d.Size = New System.Drawing.Size(56, 40)
        Me.btn360d.TabIndex = 15
        Me.btn360d.Text = "360 Days"
        '
        'btn180d
        '
        Me.btn180d.Location = New System.Drawing.Point(136, 140)
        Me.btn180d.Name = "btn180d"
        Me.btn180d.Size = New System.Drawing.Size(56, 40)
        Me.btn180d.TabIndex = 14
        Me.btn180d.Text = "180 Days"
        '
        'btn90D
        '
        Me.btn90D.Location = New System.Drawing.Point(72, 140)
        Me.btn90D.Name = "btn90D"
        Me.btn90D.Size = New System.Drawing.Size(56, 40)
        Me.btn90D.TabIndex = 13
        Me.btn90D.Text = "90 Days"
        '
        'btn30D
        '
        Me.btn30D.Location = New System.Drawing.Point(8, 140)
        Me.btn30D.Name = "btn30D"
        Me.btn30D.Size = New System.Drawing.Size(56, 40)
        Me.btn30D.TabIndex = 12
        Me.btn30D.Text = "30 Days"
        '
        'btnAddComments
        '
        Me.btnAddComments.Location = New System.Drawing.Point(8, 188)
        Me.btnAddComments.Name = "btnAddComments"
        Me.btnAddComments.Size = New System.Drawing.Size(248, 32)
        Me.btnAddComments.TabIndex = 16
        Me.btnAddComments.Text = "Add Comments"
        '
        'lblDtAttempt
        '
        Me.lblDtAttempt.Location = New System.Drawing.Point(120, 84)
        Me.lblDtAttempt.Name = "lblDtAttempt"
        Me.lblDtAttempt.Size = New System.Drawing.Size(100, 23)
        Me.lblDtAttempt.TabIndex = 4
        Me.lblDtAttempt.Text = "None"
        '
        'lblDtLastCntct
        '
        Me.lblDtLastCntct.Location = New System.Drawing.Point(8, 84)
        Me.lblDtLastCntct.Name = "lblDtLastCntct"
        Me.lblDtLastCntct.Size = New System.Drawing.Size(100, 23)
        Me.lblDtLastCntct.TabIndex = 3
        Me.lblDtLastCntct.Text = "None"
        '
        'ActHistAttempt
        '
        Me.ActHistAttempt.Location = New System.Drawing.Point(120, 60)
        Me.ActHistAttempt.Name = "ActHistAttempt"
        Me.ActHistAttempt.Size = New System.Drawing.Size(136, 23)
        Me.ActHistAttempt.TabIndex = 2
        Me.ActHistAttempt.Text = "Attempt"
        '
        'ActHistSummary
        '
        Me.ActHistSummary.Location = New System.Drawing.Point(8, 116)
        Me.ActHistSummary.Name = "ActHistSummary"
        Me.ActHistSummary.Size = New System.Drawing.Size(248, 23)
        Me.ActHistSummary.TabIndex = 1
        Me.ActHistSummary.Text = "Activity Summary"
        '
        'ActHistDateLastContact
        '
        Me.ActHistDateLastContact.Location = New System.Drawing.Point(8, 60)
        Me.ActHistDateLastContact.Name = "ActHistDateLastContact"
        Me.ActHistDateLastContact.Size = New System.Drawing.Size(96, 23)
        Me.ActHistDateLastContact.TabIndex = 0
        Me.ActHistDateLastContact.Text = "Date Last Contact"
        '
        'gbAccountHistory
        '
        Me.gbAccountHistory.Controls.Add(Me.btnBorrowerBenefits)
        Me.gbAccountHistory.Controls.Add(Me.btnTimesDelinquent)
        Me.gbAccountHistory.Controls.Add(Me.btnLateFeesWaived)
        Me.gbAccountHistory.Location = New System.Drawing.Point(592, 552)
        Me.gbAccountHistory.Name = "gbAccountHistory"
        Me.gbAccountHistory.Size = New System.Drawing.Size(376, 80)
        Me.gbAccountHistory.TabIndex = 5
        Me.gbAccountHistory.TabStop = False
        Me.gbAccountHistory.Text = "Account History"
        '
        'btnBorrowerBenefits
        '
        Me.btnBorrowerBenefits.Location = New System.Drawing.Point(20, 20)
        Me.btnBorrowerBenefits.Name = "btnBorrowerBenefits"
        Me.btnBorrowerBenefits.Size = New System.Drawing.Size(160, 24)
        Me.btnBorrowerBenefits.TabIndex = 16
        Me.btnBorrowerBenefits.Text = "Borrower Benefits"
        '
        'btnTimesDelinquent
        '
        Me.btnTimesDelinquent.Location = New System.Drawing.Point(196, 48)
        Me.btnTimesDelinquent.Name = "btnTimesDelinquent"
        Me.btnTimesDelinquent.Size = New System.Drawing.Size(160, 24)
        Me.btnTimesDelinquent.TabIndex = 18
        Me.btnTimesDelinquent.Text = "# Times Delinquent"
        '
        'btnLateFeesWaived
        '
        Me.btnLateFeesWaived.Location = New System.Drawing.Point(20, 48)
        Me.btnLateFeesWaived.Name = "btnLateFeesWaived"
        Me.btnLateFeesWaived.Size = New System.Drawing.Size(160, 24)
        Me.btnLateFeesWaived.TabIndex = 17
        Me.btnLateFeesWaived.Text = "Late Fees Waived"
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(16, 48)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(48, 16)
        Me.Label15.TabIndex = 6
        Me.Label15.Text = "SSN:"
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(320, 48)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(48, 16)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "Name:"
        '
        'btnAskDUDE
        '
        Me.btnAskDUDE.Location = New System.Drawing.Point(672, 8)
        Me.btnAskDUDE.Name = "btnAskDUDE"
        Me.btnAskDUDE.Size = New System.Drawing.Size(75, 32)
        Me.btnAskDUDE.TabIndex = 22
        Me.btnAskDUDE.Text = "Ask DUDE"
        '
        'btnDUDE
        '
        Me.btnDUDE.Location = New System.Drawing.Point(800, 8)
        Me.btnDUDE.Name = "btnDUDE"
        Me.btnDUDE.Size = New System.Drawing.Size(75, 32)
        Me.btnDUDE.TabIndex = 23
        Me.btnDUDE.Text = "FOCUS"
        '
        'btn411
        '
        Me.btn411.Location = New System.Drawing.Point(888, 8)
        Me.btn411.Name = "btn411"
        Me.btn411.Size = New System.Drawing.Size(75, 32)
        Me.btn411.TabIndex = 24
        Me.btn411.Text = "411"
        '
        'lblContactType
        '
        Me.lblContactType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactType.Location = New System.Drawing.Point(8, 392)
        Me.lblContactType.Name = "lblContactType"
        Me.lblContactType.Size = New System.Drawing.Size(304, 23)
        Me.lblContactType.TabIndex = 14
        Me.lblContactType.Text = "Non-authorized 3rd Party Contact"
        Me.lblContactType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblContactType.Visible = False
        '
        'lblName
        '
        Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(368, 48)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(216, 23)
        Me.lblName.TabIndex = 15
        '
        'lblSSN
        '
        Me.lblSSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSSN.Location = New System.Drawing.Point(64, 48)
        Me.lblSSN.Name = "lblSSN"
        Me.lblSSN.Size = New System.Drawing.Size(248, 23)
        Me.lblSSN.TabIndex = 16
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(552, 648)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(176, 32)
        Me.btnRefresh.TabIndex = 21
        Me.btnRefresh.Text = "Refresh Screen"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(184, 648)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(176, 32)
        Me.btnSave.TabIndex = 19
        Me.btnSave.Text = "Save and Continue"
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(368, 648)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(176, 32)
        Me.btnBack.TabIndex = 20
        Me.btnBack.Text = "Back to Main Menu"
        '
        'txtBSHome
        '
        Me.txtBSHome.BackColor = System.Drawing.SystemColors.Control
        Me.txtBSHome.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtBSHome.DetectUrls = False
        Me.txtBSHome.Font = New System.Drawing.Font("Bodoni MT Black", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBSHome.ForeColor = System.Drawing.SystemColors.ControlText
        Me.txtBSHome.Location = New System.Drawing.Point(312, 0)
        Me.txtBSHome.Multiline = False
        Me.txtBSHome.Name = "txtBSHome"
        Me.txtBSHome.ReadOnly = True
        Me.txtBSHome.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.txtBSHome.Size = New System.Drawing.Size(296, 24)
        Me.txtBSHome.TabIndex = 20
        Me.txtBSHome.TabStop = False
        Me.txtBSHome.Text = "Borrower Services Home Page"
        Me.txtBSHome.WordWrap = False
        '
        'btnBrightIdea
        '
        Me.btnBrightIdea.Image = CType(resources.GetObject("btnBrightIdea.Image"), System.Drawing.Image)
        Me.btnBrightIdea.Location = New System.Drawing.Point(16, 3)
        Me.btnBrightIdea.Name = "btnBrightIdea"
        Me.btnBrightIdea.Size = New System.Drawing.Size(43, 43)
        Me.btnBrightIdea.TabIndex = 25
        Me.btnBrightIdea.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnBrightIdea, "Got a Bright Idea for Maui DUDE? Click here to Send your good Idea to the Big Kah" & _
                "una")
        '
        'btnUnexpected
        '
        Me.btnUnexpected.Image = CType(resources.GetObject("btnUnexpected.Image"), System.Drawing.Image)
        Me.btnUnexpected.Location = New System.Drawing.Point(72, 3)
        Me.btnUnexpected.Name = "btnUnexpected"
        Me.btnUnexpected.Size = New System.Drawing.Size(43, 43)
        Me.btnUnexpected.TabIndex = 26
        Me.btnUnexpected.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnUnexpected, "Unexpected Result? Report Maui DUDE Errors Here.")
        '
        'lblAN
        '
        Me.lblAN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblAN.Location = New System.Drawing.Point(672, 48)
        Me.lblAN.Name = "lblAN"
        Me.lblAN.Size = New System.Drawing.Size(112, 23)
        Me.lblAN.TabIndex = 31
        Me.lblAN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label55
        '
        Me.Label55.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label55.Location = New System.Drawing.Point(600, 48)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(72, 23)
        Me.Label55.TabIndex = 30
        Me.Label55.Text = "Account #:"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDOB
        '
        Me.lblDOB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDOB.Location = New System.Drawing.Point(840, 48)
        Me.lblDOB.Name = "lblDOB"
        Me.lblDOB.Size = New System.Drawing.Size(96, 23)
        Me.lblDOB.TabIndex = 33
        Me.lblDOB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label35
        '
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label35.Location = New System.Drawing.Point(800, 48)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(40, 23)
        Me.Label35.TabIndex = 32
        Me.Label35.Text = "DOB:"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbCC
        '
        Me.gbCC.Controls.Add(Me.tbCCCmts)
        Me.gbCC.Controls.Add(Me.tbCCLtrID)
        Me.gbCC.Controls.Add(Me.Label38)
        Me.gbCC.Controls.Add(Me.Label37)
        Me.gbCC.Controls.Add(Me.cbCCRea)
        Me.gbCC.Controls.Add(Me.Label36)
        Me.gbCC.Controls.Add(Me.Label34)
        Me.gbCC.Controls.Add(Me.cbCCCat)
        Me.gbCC.Location = New System.Drawing.Point(632, 400)
        Me.gbCC.Name = "gbCC"
        Me.gbCC.Size = New System.Drawing.Size(300, 144)
        Me.gbCC.TabIndex = 34
        Me.gbCC.TabStop = False
        Me.gbCC.Text = "Call Categorization"
        '
        'tbCCCmts
        '
        Me.tbCCCmts.Enabled = False
        Me.tbCCCmts.Location = New System.Drawing.Point(108, 88)
        Me.tbCCCmts.MaxLength = 30
        Me.tbCCCmts.Multiline = True
        Me.tbCCCmts.Name = "tbCCCmts"
        Me.tbCCCmts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbCCCmts.Size = New System.Drawing.Size(184, 52)
        Me.tbCCCmts.TabIndex = 7
        '
        'tbCCLtrID
        '
        Me.tbCCLtrID.Enabled = False
        Me.tbCCLtrID.Location = New System.Drawing.Point(108, 68)
        Me.tbCCLtrID.MaxLength = 10
        Me.tbCCLtrID.Name = "tbCCLtrID"
        Me.tbCCLtrID.Size = New System.Drawing.Size(184, 20)
        Me.tbCCLtrID.TabIndex = 6
        '
        'Label38
        '
        Me.Label38.Location = New System.Drawing.Point(44, 92)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(64, 16)
        Me.Label38.TabIndex = 5
        Me.Label38.Text = "Comments:"
        '
        'Label37
        '
        Me.Label37.Location = New System.Drawing.Point(44, 72)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(64, 16)
        Me.Label37.TabIndex = 4
        Me.Label37.Text = "Letter ID:"
        '
        'cbCCRea
        '
        Me.cbCCRea.Enabled = False
        Me.cbCCRea.Location = New System.Drawing.Point(72, 44)
        Me.cbCCRea.Name = "cbCCRea"
        Me.cbCCRea.Size = New System.Drawing.Size(220, 21)
        Me.cbCCRea.TabIndex = 3
        '
        'Label36
        '
        Me.Label36.Location = New System.Drawing.Point(8, 48)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(64, 16)
        Me.Label36.TabIndex = 2
        Me.Label36.Text = "Reason:"
        '
        'Label34
        '
        Me.Label34.Location = New System.Drawing.Point(8, 24)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(64, 16)
        Me.Label34.TabIndex = 1
        Me.Label34.Text = "Category:"
        '
        'cbCCCat
        '
        Me.cbCCCat.Location = New System.Drawing.Point(72, 20)
        Me.cbCCCat.Name = "cbCCCat"
        Me.cbCCCat.Size = New System.Drawing.Size(220, 21)
        Me.cbCCCat.TabIndex = 0
        '
        'pbSurf
        '
        Me.pbSurf.Location = New System.Drawing.Point(704, 400)
        Me.pbSurf.Name = "pbSurf"
        Me.pbSurf.Size = New System.Drawing.Size(152, 144)
        Me.pbSurf.TabIndex = 35
        Me.pbSurf.TabStop = False
        '
        'lblVIPOrSH
        '
        Me.lblVIPOrSH.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblVIPOrSH.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVIPOrSH.Location = New System.Drawing.Point(312, 24)
        Me.lblVIPOrSH.Name = "lblVIPOrSH"
        Me.lblVIPOrSH.Size = New System.Drawing.Size(296, 20)
        Me.lblVIPOrSH.TabIndex = 36
        Me.lblVIPOrSH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmBSHomePage
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(982, 696)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblVIPOrSH)
        Me.Controls.Add(Me.gbCC)
        Me.Controls.Add(Me.lblDOB)
        Me.Controls.Add(Me.Label35)
        Me.Controls.Add(Me.lblAN)
        Me.Controls.Add(Me.Label55)
        Me.Controls.Add(Me.btnUnexpected)
        Me.Controls.Add(Me.btnBrightIdea)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.lblSSN)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblContactType)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.gbAccountHistory)
        Me.Controls.Add(Me.gbBorrowerActivityHistory)
        Me.Controls.Add(Me.gbPaymentInformation)
        Me.Controls.Add(Me.gbBorrowerStatus)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtBSHome)
        Me.Controls.Add(Me.btnAskDUDE)
        Me.Controls.Add(Me.btnDUDE)
        Me.Controls.Add(Me.btn411)
        Me.Controls.Add(Me.pbSurf)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(992, 725)
        Me.MinimumSize = New System.Drawing.Size(992, 725)
        Me.Name = "frmBSHomePage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Maui DUDE Borrower Services HomePage"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.pbDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbUp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbBorrowerStatus.ResumeLayout(False)
        Me.gbPaymentInformation.ResumeLayout(False)
        Me.gbPaymentInformation.PerformLayout()
        Me.gbBorrowerActivityHistory.ResumeLayout(False)
        Me.gbAccountHistory.ResumeLayout(False)
        Me.gbCC.ResumeLayout(False)
        Me.gbCC.PerformLayout()
        CType(Me.pbSurf, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
End Class
