Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmASHomePage
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal BL As SP.BorrowerLite)
        MyBase.New()

        ASBor = New BorrowerAS(BL)
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblAN As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents btnUnexpected As System.Windows.Forms.Button
    Friend WithEvents btnBrightIdea As System.Windows.Forms.Button
    Friend WithEvents lblSSN As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblContactType As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents gbPaymentInformation As System.Windows.Forms.GroupBox
    Friend WithEvents txtPayOffDate As System.Windows.Forms.TextBox
    Friend WithEvents lblDueDay As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents lblLateFeesDue As System.Windows.Forms.Label
    Friend WithEvents lblPastDueAmt As System.Windows.Forms.Label
    Friend WithEvents lblDailyInterest As System.Windows.Forms.Label
    Friend WithEvents lblPayOffAmount As System.Windows.Forms.Label
    Friend WithEvents lblLastPmtAmount As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnPayHist As System.Windows.Forms.Button
    Friend WithEvents lblMonthlyPmt As System.Windows.Forms.Label
    Friend WithEvents lblLastPmt As System.Windows.Forms.Label
    Friend WithEvents lblDueDt As System.Windows.Forms.Label
    Friend WithEvents lblInter As System.Windows.Forms.Label
    Friend WithEvents lblPrin As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblDDDate As System.Windows.Forms.Label
    Friend WithEvents lblDirectDebit As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents btnDirectDebit As System.Windows.Forms.Button
    Friend WithEvents gbBorrowerStatus As System.Windows.Forms.GroupBox
    Friend WithEvents lblCohort As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents lblLoanPg As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents lblDateOfDelinquency As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lstServicer As System.Windows.Forms.ListBox
    Friend WithEvents lstCallF As System.Windows.Forms.ListBox
    Friend WithEvents btnstatus As System.Windows.Forms.Button
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblCurrentAmountDue As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents lblNumDaysDelinquent As System.Windows.Forms.Label
    Friend WithEvents lblnum20day As System.Windows.Forms.Label
    Friend WithEvents lblLateFees As System.Windows.Forms.Label
    Friend WithEvents lblTotalAmountDue As System.Windows.Forms.Label
    Friend WithEvents lblAmountPastDue As System.Windows.Forms.Label
    Friend WithEvents btnOneLinkLoans As System.Windows.Forms.Button
    Friend WithEvents btnCompassLoans As System.Windows.Forms.Button
    Friend WithEvents lbltitlenum20day As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblLoanHistory As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lvSS As System.Windows.Forms.ListView
    Friend WithEvents btnSelect As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents txtBSHome As System.Windows.Forms.RichTextBox
    Friend WithEvents btnAskDUDE As System.Windows.Forms.Button
    Friend WithEvents btnDUDE As System.Windows.Forms.Button
    Friend WithEvents btn411 As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents gbBorrowerActivityHistory As System.Windows.Forms.GroupBox
    Friend WithEvents btnAddComments As System.Windows.Forms.Button
    Friend WithEvents lblDtAttempt As System.Windows.Forms.Label
    Friend WithEvents lblDtLastCntct As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents lblVerified As System.Windows.Forms.Label
    Friend WithEvents lblEmailVal As System.Windows.Forms.Label
    Friend WithEvents lblO2PhoneVal As System.Windows.Forms.Label
    Friend WithEvents lblOPhoneVal As System.Windows.Forms.Label
    Friend WithEvents lblHPhoneVal As System.Windows.Forms.Label
    Friend WithEvents lblAddrVal As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents lblAlter As System.Windows.Forms.Label
    Friend WithEvents lblWork As System.Windows.Forms.Label
    Friend WithEvents lblHome As System.Windows.Forms.Label
    Friend WithEvents btnLegalAddHist As System.Windows.Forms.Button
    Friend WithEvents btnUpdateDemo As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblAddr3 As System.Windows.Forms.Label
    Friend WithEvents lblAddr2 As System.Windows.Forms.Label
    Friend WithEvents lblAddr1 As System.Windows.Forms.Label
    Friend WithEvents btnOpenQ As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents pbSurfer As System.Windows.Forms.PictureBox
    Friend WithEvents lblAddSource As System.Windows.Forms.Label
    Friend WithEvents lblPhoneSource As System.Windows.Forms.Label
    Friend WithEvents ckbBor As System.Windows.Forms.CheckBox
    Friend WithEvents ckbRef As System.Windows.Forms.CheckBox
    Friend WithEvents lblReference As System.Windows.Forms.Label
    Friend WithEvents btnCompassHistory As System.Windows.Forms.Button
    Friend WithEvents btnOneLINKHistory As System.Windows.Forms.Button
    Friend WithEvents lblRefID As System.Windows.Forms.Label
    Friend WithEvents btn3rdParty As System.Windows.Forms.Button
    Friend WithEvents btnBankruptcy As System.Windows.Forms.Button
    Friend WithEvents btnKeyIdentifier As System.Windows.Forms.Button
    Friend WithEvents btnExitCounseling As System.Windows.Forms.Button
    Friend WithEvents btnOFAC As System.Windows.Forms.Button
    Friend WithEvents btnCreditBureau As System.Windows.Forms.Button
    Friend WithEvents btnSkipHistory As System.Windows.Forms.Button
    Friend WithEvents btnLegalPhoneHistory As System.Windows.Forms.Button
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents lblDOB As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmASHomePage))
        Me.lblAN = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.btnUnexpected = New System.Windows.Forms.Button
        Me.btnBrightIdea = New System.Windows.Forms.Button
        Me.lblSSN = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblContactType = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
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
        Me.gbBorrowerStatus = New System.Windows.Forms.GroupBox
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lvSS = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.btnSelect = New System.Windows.Forms.Button
        Me.txtBSHome = New System.Windows.Forms.RichTextBox
        Me.btnAskDUDE = New System.Windows.Forms.Button
        Me.btnDUDE = New System.Windows.Forms.Button
        Me.btn411 = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnRefresh = New System.Windows.Forms.Button
        Me.gbBorrowerActivityHistory = New System.Windows.Forms.GroupBox
        Me.lblRefID = New System.Windows.Forms.Label
        Me.lblReference = New System.Windows.Forms.Label
        Me.ckbRef = New System.Windows.Forms.CheckBox
        Me.ckbBor = New System.Windows.Forms.CheckBox
        Me.btnCompassHistory = New System.Windows.Forms.Button
        Me.lblDtAttempt = New System.Windows.Forms.Label
        Me.lblDtLastCntct = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.btnOneLINKHistory = New System.Windows.Forms.Button
        Me.btnAddComments = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.lblAddSource = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.lblPhoneSource = New System.Windows.Forms.Label
        Me.btnLegalPhoneHistory = New System.Windows.Forms.Button
        Me.lblVerified = New System.Windows.Forms.Label
        Me.lblEmailVal = New System.Windows.Forms.Label
        Me.lblO2PhoneVal = New System.Windows.Forms.Label
        Me.lblOPhoneVal = New System.Windows.Forms.Label
        Me.lblHPhoneVal = New System.Windows.Forms.Label
        Me.lblAddrVal = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
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
        Me.btn3rdParty = New System.Windows.Forms.Button
        Me.btnBankruptcy = New System.Windows.Forms.Button
        Me.btnKeyIdentifier = New System.Windows.Forms.Button
        Me.btnExitCounseling = New System.Windows.Forms.Button
        Me.btnOFAC = New System.Windows.Forms.Button
        Me.btnCreditBureau = New System.Windows.Forms.Button
        Me.pbSurfer = New System.Windows.Forms.PictureBox
        Me.btnOpenQ = New System.Windows.Forms.Button
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.btnSkipHistory = New System.Windows.Forms.Button
        Me.lblDOB = New System.Windows.Forms.Label
        Me.Label36 = New System.Windows.Forms.Label
        Me.gbPaymentInformation.SuspendLayout()
        Me.gbBorrowerStatus.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.gbBorrowerActivityHistory.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.pbSurfer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblAN
        '
        Me.lblAN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblAN.Location = New System.Drawing.Point(680, 56)
        Me.lblAN.Name = "lblAN"
        Me.lblAN.Size = New System.Drawing.Size(112, 23)
        Me.lblAN.TabIndex = 51
        Me.lblAN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label55
        '
        Me.Label55.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label55.Location = New System.Drawing.Point(608, 56)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(72, 23)
        Me.Label55.TabIndex = 50
        Me.Label55.Text = "Account #:"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnUnexpected
        '
        Me.btnUnexpected.Image = CType(resources.GetObject("btnUnexpected.Image"), System.Drawing.Image)
        Me.btnUnexpected.Location = New System.Drawing.Point(80, 8)
        Me.btnUnexpected.Name = "btnUnexpected"
        Me.btnUnexpected.Size = New System.Drawing.Size(43, 43)
        Me.btnUnexpected.TabIndex = 49
        Me.btnUnexpected.TabStop = False
        '
        'btnBrightIdea
        '
        Me.btnBrightIdea.Image = CType(resources.GetObject("btnBrightIdea.Image"), System.Drawing.Image)
        Me.btnBrightIdea.Location = New System.Drawing.Point(24, 8)
        Me.btnBrightIdea.Name = "btnBrightIdea"
        Me.btnBrightIdea.Size = New System.Drawing.Size(43, 43)
        Me.btnBrightIdea.TabIndex = 48
        Me.btnBrightIdea.TabStop = False
        '
        'lblSSN
        '
        Me.lblSSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSSN.Location = New System.Drawing.Point(72, 56)
        Me.lblSSN.Name = "lblSSN"
        Me.lblSSN.Size = New System.Drawing.Size(248, 23)
        Me.lblSSN.TabIndex = 42
        '
        'lblName
        '
        Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(376, 56)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(216, 23)
        Me.lblName.TabIndex = 41
        '
        'lblContactType
        '
        Me.lblContactType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactType.Location = New System.Drawing.Point(16, 360)
        Me.lblContactType.Name = "lblContactType"
        Me.lblContactType.Size = New System.Drawing.Size(304, 23)
        Me.lblContactType.TabIndex = 40
        Me.lblContactType.Text = "Non-authorized 3rd Party Contact"
        Me.lblContactType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblContactType.Visible = False
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(328, 56)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(48, 16)
        Me.Label16.TabIndex = 39
        Me.Label16.Text = "Name:"
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(24, 56)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(48, 16)
        Me.Label15.TabIndex = 38
        Me.Label15.Text = "SSN:"
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
        Me.gbPaymentInformation.Location = New System.Drawing.Point(16, 376)
        Me.gbPaymentInformation.Name = "gbPaymentInformation"
        Me.gbPaymentInformation.Size = New System.Drawing.Size(300, 232)
        Me.gbPaymentInformation.TabIndex = 35
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
        '
        'gbBorrowerStatus
        '
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
        Me.gbBorrowerStatus.Location = New System.Drawing.Point(600, 80)
        Me.gbBorrowerStatus.Name = "gbBorrowerStatus"
        Me.gbBorrowerStatus.Size = New System.Drawing.Size(376, 280)
        Me.gbBorrowerStatus.TabIndex = 34
        Me.gbBorrowerStatus.TabStop = False
        Me.gbBorrowerStatus.Text = "Borrower Status"
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
        Me.lblDateOfDelinquency.Location = New System.Drawing.Point(312, 56)
        Me.lblDateOfDelinquency.Name = "lblDateOfDelinquency"
        Me.lblDateOfDelinquency.Size = New System.Drawing.Size(56, 16)
        Me.lblDateOfDelinquency.TabIndex = 27
        Me.lblDateOfDelinquency.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(200, 56)
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
        Me.btnstatus.Location = New System.Drawing.Point(296, 24)
        Me.btnstatus.Name = "btnstatus"
        Me.btnstatus.Size = New System.Drawing.Size(72, 24)
        Me.btnstatus.TabIndex = 5
        Me.btnstatus.Text = "Mo' Status"
        '
        'lblTotal
        '
        Me.lblTotal.Location = New System.Drawing.Point(112, 152)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(80, 16)
        Me.lblTotal.TabIndex = 22
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(16, 152)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(100, 16)
        Me.Label17.TabIndex = 21
        Me.Label17.Text = "Total+Late Fees:"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCurrentAmountDue
        '
        Me.lblCurrentAmountDue.Location = New System.Drawing.Point(128, 104)
        Me.lblCurrentAmountDue.Name = "lblCurrentAmountDue"
        Me.lblCurrentAmountDue.Size = New System.Drawing.Size(64, 16)
        Me.lblCurrentAmountDue.TabIndex = 20
        Me.lblCurrentAmountDue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(16, 104)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(120, 16)
        Me.Label18.TabIndex = 19
        Me.Label18.Text = "Current Amount Due:"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblNumDaysDelinquent
        '
        Me.lblNumDaysDelinquent.Location = New System.Drawing.Point(136, 56)
        Me.lblNumDaysDelinquent.Name = "lblNumDaysDelinquent"
        Me.lblNumDaysDelinquent.Size = New System.Drawing.Size(56, 16)
        Me.lblNumDaysDelinquent.TabIndex = 17
        Me.lblNumDaysDelinquent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblnum20day
        '
        Me.lblnum20day.Location = New System.Drawing.Point(320, 104)
        Me.lblnum20day.Name = "lblnum20day"
        Me.lblnum20day.Size = New System.Drawing.Size(48, 24)
        Me.lblnum20day.TabIndex = 16
        Me.lblnum20day.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLateFees
        '
        Me.lblLateFees.Location = New System.Drawing.Point(312, 80)
        Me.lblLateFees.Name = "lblLateFees"
        Me.lblLateFees.Size = New System.Drawing.Size(56, 24)
        Me.lblLateFees.TabIndex = 15
        Me.lblLateFees.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTotalAmountDue
        '
        Me.lblTotalAmountDue.Location = New System.Drawing.Point(120, 128)
        Me.lblTotalAmountDue.Name = "lblTotalAmountDue"
        Me.lblTotalAmountDue.Size = New System.Drawing.Size(72, 16)
        Me.lblTotalAmountDue.TabIndex = 14
        Me.lblTotalAmountDue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblAmountPastDue
        '
        Me.lblAmountPastDue.Location = New System.Drawing.Point(120, 80)
        Me.lblAmountPastDue.Name = "lblAmountPastDue"
        Me.lblAmountPastDue.Size = New System.Drawing.Size(72, 16)
        Me.lblAmountPastDue.TabIndex = 13
        Me.lblAmountPastDue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnOneLinkLoans
        '
        Me.btnOneLinkLoans.Location = New System.Drawing.Point(296, 200)
        Me.btnOneLinkLoans.Name = "btnOneLinkLoans"
        Me.btnOneLinkLoans.Size = New System.Drawing.Size(72, 32)
        Me.btnOneLinkLoans.TabIndex = 8
        Me.btnOneLinkLoans.Text = "OneLINK Loans"
        '
        'btnCompassLoans
        '
        Me.btnCompassLoans.Location = New System.Drawing.Point(296, 160)
        Me.btnCompassLoans.Name = "btnCompassLoans"
        Me.btnCompassLoans.Size = New System.Drawing.Size(72, 32)
        Me.btnCompassLoans.TabIndex = 6
        Me.btnCompassLoans.Text = "Compass Loans"
        '
        'lbltitlenum20day
        '
        Me.lbltitlenum20day.Location = New System.Drawing.Point(200, 104)
        Me.lbltitlenum20day.Name = "lbltitlenum20day"
        Me.lbltitlenum20day.Size = New System.Drawing.Size(120, 23)
        Me.lbltitlenum20day.TabIndex = 9
        Me.lbltitlenum20day.Text = "# Times 20-Day Letter"
        Me.lbltitlenum20day.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(248, 80)
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
        Me.lblLoanHistory.Location = New System.Drawing.Point(296, 128)
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
        Me.Label3.Location = New System.Drawing.Point(16, 128)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Total Amount Due:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Amount Past Due:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "# Of Days Delinquent:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lvSS)
        Me.GroupBox1.Controls.Add(Me.btnSelect)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 80)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(300, 275)
        Me.GroupBox1.TabIndex = 32
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Scripts and Services"
        '
        'lvSS
        '
        Me.lvSS.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvSS.FullRowSelect = True
        Me.lvSS.Location = New System.Drawing.Point(8, 56)
        Me.lvSS.MultiSelect = False
        Me.lvSS.Name = "lvSS"
        Me.lvSS.Size = New System.Drawing.Size(280, 208)
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
        Me.btnSelect.Location = New System.Drawing.Point(8, 24)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(280, 23)
        Me.btnSelect.TabIndex = 1
        Me.btnSelect.Text = "Select"
        '
        'txtBSHome
        '
        Me.txtBSHome.BackColor = System.Drawing.SystemColors.Control
        Me.txtBSHome.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtBSHome.DetectUrls = False
        Me.txtBSHome.Font = New System.Drawing.Font("Bodoni MT Black", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBSHome.ForeColor = System.Drawing.SystemColors.ControlText
        Me.txtBSHome.Location = New System.Drawing.Point(320, 16)
        Me.txtBSHome.Multiline = False
        Me.txtBSHome.Name = "txtBSHome"
        Me.txtBSHome.ReadOnly = True
        Me.txtBSHome.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.txtBSHome.Size = New System.Drawing.Size(296, 32)
        Me.txtBSHome.TabIndex = 43
        Me.txtBSHome.TabStop = False
        Me.txtBSHome.Text = "Auxiliary Services Home Page"
        Me.txtBSHome.WordWrap = False
        '
        'btnAskDUDE
        '
        Me.btnAskDUDE.Location = New System.Drawing.Point(720, 16)
        Me.btnAskDUDE.Name = "btnAskDUDE"
        Me.btnAskDUDE.Size = New System.Drawing.Size(75, 32)
        Me.btnAskDUDE.TabIndex = 44
        Me.btnAskDUDE.Text = "Ask DUDE"
        '
        'btnDUDE
        '
        Me.btnDUDE.Location = New System.Drawing.Point(808, 16)
        Me.btnDUDE.Name = "btnDUDE"
        Me.btnDUDE.Size = New System.Drawing.Size(75, 32)
        Me.btnDUDE.TabIndex = 46
        Me.btnDUDE.Text = "Focus DUDE"
        '
        'btn411
        '
        Me.btn411.Location = New System.Drawing.Point(896, 16)
        Me.btn411.Name = "btn411"
        Me.btn411.Size = New System.Drawing.Size(75, 32)
        Me.btn411.TabIndex = 47
        Me.btn411.Text = "411"
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(368, 640)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(176, 32)
        Me.btnBack.TabIndex = 53
        Me.btnBack.Text = "Back to Main Menu"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(184, 640)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(176, 32)
        Me.btnSave.TabIndex = 52
        Me.btnSave.Text = "Save and Continue"
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(552, 640)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(176, 32)
        Me.btnRefresh.TabIndex = 54
        Me.btnRefresh.Text = "Refresh Screen"
        '
        'gbBorrowerActivityHistory
        '
        Me.gbBorrowerActivityHistory.Controls.Add(Me.lblRefID)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.lblReference)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.ckbRef)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.ckbBor)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.btnCompassHistory)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.lblDtAttempt)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.lblDtLastCntct)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.Label14)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.Label24)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.btnOneLINKHistory)
        Me.gbBorrowerActivityHistory.Controls.Add(Me.btnAddComments)
        Me.gbBorrowerActivityHistory.Location = New System.Drawing.Point(328, 424)
        Me.gbBorrowerActivityHistory.Name = "gbBorrowerActivityHistory"
        Me.gbBorrowerActivityHistory.Size = New System.Drawing.Size(264, 184)
        Me.gbBorrowerActivityHistory.TabIndex = 56
        Me.gbBorrowerActivityHistory.TabStop = False
        Me.gbBorrowerActivityHistory.Text = "Activity History"
        '
        'lblRefID
        '
        Me.lblRefID.Location = New System.Drawing.Point(160, 48)
        Me.lblRefID.Name = "lblRefID"
        Me.lblRefID.Size = New System.Drawing.Size(96, 24)
        Me.lblRefID.TabIndex = 23
        '
        'lblReference
        '
        Me.lblReference.Location = New System.Drawing.Point(160, 76)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.Size = New System.Drawing.Size(96, 20)
        Me.lblReference.TabIndex = 22
        '
        'ckbRef
        '
        Me.ckbRef.Appearance = System.Windows.Forms.Appearance.Button
        Me.ckbRef.Location = New System.Drawing.Point(192, 24)
        Me.ckbRef.Name = "ckbRef"
        Me.ckbRef.Size = New System.Drawing.Size(64, 20)
        Me.ckbRef.TabIndex = 21
        Me.ckbRef.Text = "Reference"
        '
        'ckbBor
        '
        Me.ckbBor.Appearance = System.Windows.Forms.Appearance.Button
        Me.ckbBor.Checked = True
        Me.ckbBor.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ckbBor.Location = New System.Drawing.Point(8, 24)
        Me.ckbBor.Name = "ckbBor"
        Me.ckbBor.Size = New System.Drawing.Size(60, 20)
        Me.ckbBor.TabIndex = 20
        Me.ckbBor.Text = "Borrower"
        '
        'btnCompassHistory
        '
        Me.btnCompassHistory.Location = New System.Drawing.Point(156, 108)
        Me.btnCompassHistory.Name = "btnCompassHistory"
        Me.btnCompassHistory.Size = New System.Drawing.Size(100, 32)
        Me.btnCompassHistory.TabIndex = 12
        Me.btnCompassHistory.Text = "Compass History 1 Year"
        '
        'lblDtAttempt
        '
        Me.lblDtAttempt.Location = New System.Drawing.Point(80, 80)
        Me.lblDtAttempt.Name = "lblDtAttempt"
        Me.lblDtAttempt.Size = New System.Drawing.Size(72, 23)
        Me.lblDtAttempt.TabIndex = 4
        Me.lblDtAttempt.Text = "None"
        '
        'lblDtLastCntct
        '
        Me.lblDtLastCntct.Location = New System.Drawing.Point(80, 52)
        Me.lblDtLastCntct.Name = "lblDtLastCntct"
        Me.lblDtLastCntct.Size = New System.Drawing.Size(72, 23)
        Me.lblDtLastCntct.TabIndex = 3
        Me.lblDtLastCntct.Text = "None"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(12, 80)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(48, 23)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "Attempt:"
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(12, 52)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(72, 23)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = "Last Contact:"
        '
        'btnOneLINKHistory
        '
        Me.btnOneLINKHistory.Location = New System.Drawing.Point(8, 108)
        Me.btnOneLINKHistory.Name = "btnOneLINKHistory"
        Me.btnOneLINKHistory.Size = New System.Drawing.Size(100, 32)
        Me.btnOneLINKHistory.TabIndex = 17
        Me.btnOneLINKHistory.Text = "OneLINK History 1 Year"
        '
        'btnAddComments
        '
        Me.btnAddComments.Location = New System.Drawing.Point(8, 144)
        Me.btnAddComments.Name = "btnAddComments"
        Me.btnAddComments.Size = New System.Drawing.Size(248, 32)
        Me.btnAddComments.TabIndex = 16
        Me.btnAddComments.Text = "Add Comments"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.GroupBox3)
        Me.GroupBox2.Controls.Add(Me.btnLegalPhoneHistory)
        Me.GroupBox2.Controls.Add(Me.lblVerified)
        Me.GroupBox2.Controls.Add(Me.lblEmailVal)
        Me.GroupBox2.Controls.Add(Me.lblO2PhoneVal)
        Me.GroupBox2.Controls.Add(Me.lblOPhoneVal)
        Me.GroupBox2.Controls.Add(Me.lblHPhoneVal)
        Me.GroupBox2.Controls.Add(Me.lblAddrVal)
        Me.GroupBox2.Controls.Add(Me.Label31)
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
        Me.GroupBox2.Location = New System.Drawing.Point(328, 80)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(264, 336)
        Me.GroupBox2.TabIndex = 55
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Borrower Demos"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label34)
        Me.GroupBox3.Controls.Add(Me.lblAddSource)
        Me.GroupBox3.Controls.Add(Me.Label25)
        Me.GroupBox3.Controls.Add(Me.lblPhoneSource)
        Me.GroupBox3.Location = New System.Drawing.Point(8, 264)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(248, 64)
        Me.GroupBox3.TabIndex = 27
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Most Recent Info Source"
        '
        'Label34
        '
        Me.Label34.Location = New System.Drawing.Point(8, 40)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(56, 16)
        Me.Label34.TabIndex = 24
        Me.Label34.Text = "Phone:"
        '
        'lblAddSource
        '
        Me.lblAddSource.Location = New System.Drawing.Point(72, 16)
        Me.lblAddSource.Name = "lblAddSource"
        Me.lblAddSource.Size = New System.Drawing.Size(168, 16)
        Me.lblAddSource.TabIndex = 25
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(8, 16)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(64, 16)
        Me.Label25.TabIndex = 23
        Me.Label25.Text = "Address:"
        '
        'lblPhoneSource
        '
        Me.lblPhoneSource.Location = New System.Drawing.Point(72, 40)
        Me.lblPhoneSource.Name = "lblPhoneSource"
        Me.lblPhoneSource.Size = New System.Drawing.Size(168, 16)
        Me.lblPhoneSource.TabIndex = 26
        '
        'btnLegalPhoneHistory
        '
        Me.btnLegalPhoneHistory.Location = New System.Drawing.Point(96, 224)
        Me.btnLegalPhoneHistory.Name = "btnLegalPhoneHistory"
        Me.btnLegalPhoneHistory.Size = New System.Drawing.Size(80, 32)
        Me.btnLegalPhoneHistory.TabIndex = 22
        Me.btnLegalPhoneHistory.Text = "Legal Phone History"
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
        Me.btnLegalAddHist.Location = New System.Drawing.Point(24, 224)
        Me.btnLegalAddHist.Name = "btnLegalAddHist"
        Me.btnLegalAddHist.Size = New System.Drawing.Size(64, 32)
        Me.btnLegalAddHist.TabIndex = 4
        Me.btnLegalAddHist.Text = "Legal Add History"
        '
        'btnUpdateDemo
        '
        Me.btnUpdateDemo.Location = New System.Drawing.Point(184, 224)
        Me.btnUpdateDemo.Name = "btnUpdateDemo"
        Me.btnUpdateDemo.Size = New System.Drawing.Size(56, 32)
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
        'btn3rdParty
        '
        Me.btn3rdParty.Location = New System.Drawing.Point(16, 24)
        Me.btn3rdParty.Name = "btn3rdParty"
        Me.btn3rdParty.Size = New System.Drawing.Size(128, 24)
        Me.btn3rdParty.TabIndex = 57
        Me.btn3rdParty.Text = "3rd Party Auth/POA"
        '
        'btnBankruptcy
        '
        Me.btnBankruptcy.Location = New System.Drawing.Point(16, 48)
        Me.btnBankruptcy.Name = "btnBankruptcy"
        Me.btnBankruptcy.Size = New System.Drawing.Size(128, 24)
        Me.btnBankruptcy.TabIndex = 58
        Me.btnBankruptcy.Text = "Bankruptcy"
        '
        'btnKeyIdentifier
        '
        Me.btnKeyIdentifier.Location = New System.Drawing.Point(16, 120)
        Me.btnKeyIdentifier.Name = "btnKeyIdentifier"
        Me.btnKeyIdentifier.Size = New System.Drawing.Size(128, 24)
        Me.btnKeyIdentifier.TabIndex = 59
        Me.btnKeyIdentifier.Text = "Key Identifier Change"
        '
        'btnExitCounseling
        '
        Me.btnExitCounseling.Location = New System.Drawing.Point(16, 96)
        Me.btnExitCounseling.Name = "btnExitCounseling"
        Me.btnExitCounseling.Size = New System.Drawing.Size(128, 24)
        Me.btnExitCounseling.TabIndex = 60
        Me.btnExitCounseling.Text = "Exit Counseling"
        '
        'btnOFAC
        '
        Me.btnOFAC.Location = New System.Drawing.Point(16, 144)
        Me.btnOFAC.Name = "btnOFAC"
        Me.btnOFAC.Size = New System.Drawing.Size(128, 24)
        Me.btnOFAC.TabIndex = 61
        Me.btnOFAC.Text = "OFAC"
        '
        'btnCreditBureau
        '
        Me.btnCreditBureau.Location = New System.Drawing.Point(16, 72)
        Me.btnCreditBureau.Name = "btnCreditBureau"
        Me.btnCreditBureau.Size = New System.Drawing.Size(128, 24)
        Me.btnCreditBureau.TabIndex = 62
        Me.btnCreditBureau.Text = "Credit Bureau"
        '
        'pbSurfer
        '
        Me.pbSurfer.Image = CType(resources.GetObject("pbSurfer.Image"), System.Drawing.Image)
        Me.pbSurfer.Location = New System.Drawing.Point(152, 24)
        Me.pbSurfer.Name = "pbSurfer"
        Me.pbSurfer.Size = New System.Drawing.Size(216, 184)
        Me.pbSurfer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbSurfer.TabIndex = 63
        Me.pbSurfer.TabStop = False
        '
        'btnOpenQ
        '
        Me.btnOpenQ.Location = New System.Drawing.Point(16, 168)
        Me.btnOpenQ.Name = "btnOpenQ"
        Me.btnOpenQ.Size = New System.Drawing.Size(128, 24)
        Me.btnOpenQ.TabIndex = 64
        Me.btnOpenQ.Text = "Open Queue Tasks"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnSkipHistory)
        Me.GroupBox4.Controls.Add(Me.btn3rdParty)
        Me.GroupBox4.Controls.Add(Me.btnCreditBureau)
        Me.GroupBox4.Controls.Add(Me.btnExitCounseling)
        Me.GroupBox4.Controls.Add(Me.btnBankruptcy)
        Me.GroupBox4.Controls.Add(Me.btnOpenQ)
        Me.GroupBox4.Controls.Add(Me.btnOFAC)
        Me.GroupBox4.Controls.Add(Me.btnKeyIdentifier)
        Me.GroupBox4.Controls.Add(Me.pbSurfer)
        Me.GroupBox4.Location = New System.Drawing.Point(600, 368)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(376, 240)
        Me.GroupBox4.TabIndex = 65
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Auxiliary Activity"
        '
        'btnSkipHistory
        '
        Me.btnSkipHistory.Location = New System.Drawing.Point(16, 192)
        Me.btnSkipHistory.Name = "btnSkipHistory"
        Me.btnSkipHistory.Size = New System.Drawing.Size(128, 24)
        Me.btnSkipHistory.TabIndex = 65
        Me.btnSkipHistory.Text = "Skip History"
        '
        'lblDOB
        '
        Me.lblDOB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDOB.Location = New System.Drawing.Point(848, 56)
        Me.lblDOB.Name = "lblDOB"
        Me.lblDOB.Size = New System.Drawing.Size(112, 23)
        Me.lblDOB.TabIndex = 67
        Me.lblDOB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label36
        '
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label36.Location = New System.Drawing.Point(808, 56)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(40, 23)
        Me.Label36.TabIndex = 66
        Me.Label36.Text = "DOB:"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmASHomePage
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(992, 685)
        Me.Controls.Add(Me.lblDOB)
        Me.Controls.Add(Me.Label36)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.gbBorrowerActivityHistory)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.lblAN)
        Me.Controls.Add(Me.Label55)
        Me.Controls.Add(Me.btnUnexpected)
        Me.Controls.Add(Me.btnBrightIdea)
        Me.Controls.Add(Me.lblSSN)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblContactType)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.gbPaymentInformation)
        Me.Controls.Add(Me.gbBorrowerStatus)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtBSHome)
        Me.Controls.Add(Me.btnAskDUDE)
        Me.Controls.Add(Me.btnDUDE)
        Me.Controls.Add(Me.btn411)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmASHomePage"
        Me.Text = "Maui DUDE Auxiliary Services Home Page"
        Me.gbPaymentInformation.ResumeLayout(False)
        Me.gbPaymentInformation.PerformLayout()
        Me.gbBorrowerStatus.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.gbBorrowerActivityHistory.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.pbSurfer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long

#Region "Class Variables"
    Private StatusArr(0) As String

    Public Demographic As SP.frmDemographics
    Private NoteDUDE As SP.frmNoteDUDE
    Private SS As SP.frmScriptsAndServices
    Private CompLoans As frmCompassLoans
    'Private DefFor As frmDeferForbHistory
    Private PayHist As frmPaymentHistory
    Private mostat As frmMoreStatus
    'Private AH30 As frmActivityHistory
    'Private AH90 As frmActivityHistory
    'Private AH180 As frmActivityHistory
    Private AH365 As frmActivityHistory
    'Private BB As frmBorrowerBenefits
    Private OLoans As frmOneLinkLoans
    Private DirectDebit As frmDirectDebit

    Private ASBor As BorrowerAS


#End Region

    Public Shadows Function Show() As Boolean
        Dim x As Integer

        ASBor.MonthlyPA = 0

        ReDim StatusArr(0)
        lvSS.Items.Clear()

        lstCallF.Items.Clear()
        lstServicer.Items.Clear()

        lblSSN.Text = ASBor.SSN
        'lblAN.Text = ASBor.CLAccNum
        '    'set opacity level
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

        NoteDUDE = New SP.frmNoteDUDE(ASBor)

        'DefFor = New frmDeferForbHistory

        CompLoans = New frmCompassLoans

        PayHist = New frmPaymentHistory

        Demographic = New SP.frmDemographics(ASBor)

        DirectDebit = New frmDirectDebit

        ASBor.TPDDCheck()

        If Demographic.PopulateFrm(True) = False Then
            Show = False
            Exit Function
        End If
        lblAN.Text = ASBor.CLAccNum
        lblDOB.Text = ASBor.DOB
        Dim turboSpeed As New Thread(AddressOf ASBor.Turbo)
        turboSpeed.Start()
        Demographic.Showdialog()

        If Demographic.BackButtonClicked Then
            Show = False
            Exit Function
        End If
        SP.Processing.Visible = True
        SP.Processing.Refresh()

        Do While turboSpeed.IsAlive
            Thread.CurrentThread.Sleep(100)

        Loop

        ASBor.ReturnToACP()

        UpdateDemo()
        'retrieve ACP Loan information
        If ASBor.BorLite.NoACPBSVCall = False Then
            If ASBor.FindACPLoanData(Me) = False Then
                If ASBor.ActivityCode <> "" Then
                    Show = True
                    Exit Function
                Else
                    Show = False
                    Exit Function
                End If
            End If
        End If

        ASBor.ACPEntryAnd3rdPrtyChk(Me)

        'combine statuses from LG10 and TS26
        For x = ASBor.LG10Data.GetLowerBound(1) To ASBor.LG10Data.GetUpperBound(1)
            If GetStatus(ASBor.LG10Data(5, x), ASBor.LG10Data(6, x)) <> "" And Array.IndexOf(StatusArr, GetStatus(ASBor.LG10Data(5, x), ASBor.LG10Data(6, x))) < 0 Then
                StatusArr(StatusArr.GetUpperBound(0)) = GetStatus(ASBor.LG10Data(5, x), ASBor.LG10Data(6, x))
                ReDim Preserve StatusArr(StatusArr.GetUpperBound(0) + 1)
            End If
        Next
        For x = ASBor.BorrStat.GetLowerBound(0) + 1 To ASBor.BorrStat.GetUpperBound(0)
            If GetStatus(ASBor.BorrStat(x), "") <> "" And Array.IndexOf(StatusArr, GetStatus(ASBor.BorrStat(x), "")) < 0 Then
                StatusArr(StatusArr.GetUpperBound(0)) = (GetStatus(ASBor.BorrStat(x), ""))
                ReDim Preserve StatusArr(StatusArr.GetUpperBound(0) + 1)
            End If
        Next
        'sort by rank
        Dim rank(0) As String
        'Dim SortedStatus As Array
        For x = StatusArr.GetLowerBound(0) To StatusArr.GetUpperBound(0)
            If CStr(StatusArr.GetValue(x)) <> "" Then
                rank(x) = CStr(Status.Item(StatusArr.GetValue(x)))
                ReDim Preserve rank(rank.GetUpperBound(0) + 1)
            End If
        Next

        'display to label
        lblStatus.Text = ""
        Array.Sort(rank, StatusArr)
        If StatusArr.GetUpperBound(0) < 4 Then
            For x = StatusArr.GetLowerBound(0) To StatusArr.GetUpperBound(0)
                lblStatus.Text = lblStatus.Text & "  " & StatusArr(x)
            Next
        Else
            btnstatus.Visible = True
            For x = 1 To 3
                lblStatus.Text = lblStatus.Text & "  " & StatusArr(x)
            Next
        End If

        'display all gathered data

        'Borrower Status
        Dim totamttemp As Double
        For x = 1 To ASBor.Installments.GetUpperBound(1)
            If CDate(ASBor.Installments(1, x)) < Today Then totamttemp = totamttemp + CDbl(ASBor.Installments(0, x))
        Next

        lblNumDaysDelinquent.Text = ASBor.NumDaysDelinquent
        lblDateOfDelinquency.Text = ASBor.DateDelinquencyOccurred

        'ASBor.AmountPastDue, ASBor.OutstandingLateFees,ASBor.CurrentDueAmount,ASBor.TotalAmountDue
        lblAmountPastDue.Text = FormatCurrency(ASBor.AmountPastDue, 2)
        lblLateFees.Text = FormatCurrency(ASBor.OutstandingLateFees, 2)
        lblCurrentAmountDue.Text = FormatCurrency(ASBor.CurrentDueAmount, 2)
        'lblTotalAmountDue.Text = FormatCurrency(ASBor.CurrentDueAmount + ASBor.AmountPastDue, 2)
        lblTotalAmountDue.Text = FormatCurrency(ASBor.TotalAmountDue, 2)
        lblTotal.Text = FormatCurrency(ASBor.CurrentDueAmount + ASBor.AmountPastDue + ASBor.OutstandingLateFees, 2)

        lblnum20day.Text = ASBor.Num20Day
        setCallForward(ASBor.HasTPDD)
        setServicer()

        'Payment Information
        lblPrin.Text = FormatCurrency(ASBor.TotalPrincipalDue, 2)
        lblInter.Text = FormatCurrency(ASBor.TotalInterestDue, 2)
        If ASBor.NextDueDate = "" Then 'use next due date unless it's blank
            lblDueDt.Text = ASBor.DueDate
        Else
            lblDueDt.Text = ASBor.NextDueDate
        End If
        'adjust due date to the next month if the payment day is past
        If IsDate(lblDueDt.Text) Then
            If CDate(lblDueDt.Text) < Today Then
                If IsNumeric(ASBor.DueDay) Then
                    lblDueDt.Text = Format(CDate(lblDueDt.Text).AddMonths(1), "MM/dd/yy")
                End If
            End If
        End If

        For x = 1 To ASBor.Installments.GetUpperBound(1)
            lblDueDay.Text = lblDueDay.Text & Format(CDate(ASBor.Installments(1, x)), "dd") & " "
        Next

        Dim mnthlyPayAmt As Double = 0
        For x = 1 To ASBor.Installments.GetUpperBound(1)
            lblMonthlyPmt.Text = lblMonthlyPmt.Text & FormatCurrency(ASBor.Installments(0, x), 2) & " "
            mnthlyPayAmt = mnthlyPayAmt + Double.Parse(ASBor.Installments(0, x))
        Next
        ASBor.MonthlyPaymentAmount = mnthlyPayAmt.ToString("$#,###,##0.00")

        lblLastPmt.Text = ASBor.DateLastPaymentReceived
        lblLastPmtAmount.Text = FormatCurrency(ASBor.LastPaymentAmount, 2)
        lblPayOffAmount.Text = FormatCurrency(ASBor.TotalPayoffAmount, 2)
        lblDirectDebit.Text = ASBor.ACHData.HasACH
        lblDDDate.Text = ASBor.ACHData.StatusDt
        lblDailyInterest.Text = FormatCurrency(ASBor.TotalDailyInterest, 2)
        lblPastDueAmt.Text = FormatCurrency(ASBor.TotalPastDueAmount, 2)
        lblLateFeesDue.Text = FormatCurrency(ASBor.TotalLateFeesDue, 2)
        txtPayOffDate.Text = Today.ToShortDateString

        'add loans from TS26
        For x = LBound(ASBor.Loans, 2) To UBound(ASBor.Loans, 2)
            If InStr(lblLoanPg.Text, TranslateLoanType(ASBor.Loans(1, x))) = 0 Then
                lblLoanPg.Text = lblLoanPg.Text & " " & TranslateLoanType(ASBor.Loans(1, x))
            End If
        Next x
        'Loan Programs
        For x = LBound(ASBor.LG10Data, 2) To UBound(ASBor.LG10Data, 2)
            If InStr(lblLoanPg.Text, ASBor.LG10Data(1, x)) = 0 Then
                If ASBor.LG10Data(1, x) <> "GL" Or _
                (ASBor.LG10Data(1, x) = "GL" And InStr(lblLoanPg.Text, "SF") = 0 And InStr(lblLoanPg.Text, "SU") = 0) Then
                    lblLoanPg.Text = lblLoanPg.Text & " " & ASBor.LG10Data(1, x)
                End If
            End If
        Next x


        'Cohort
        lblCohort.Text = ""
        'lonas from TS26
        For x = 1 To UBound(ASBor.Loans, 2)
            If ASBor.Loans(0, x) = "STFFRD" Or _
            ASBor.Loans(0, x) = "UNSTFD" Or _
            ASBor.Loans(0, x) = "SLS" Or _
            ASBor.Loans(0, x) = "SUBCNS" Or _
            ASBor.Loans(0, x) = "UNCNS" Then
                If CDate(ASBor.Loans(1, x)) >= CDate("10/01/" & Format(Today.AddYears(-1), "yyyy")) And _
                CDate(ASBor.Loans(1, x)) <= CDate("9/30/" & Format(Today, "yyyy")) Then
                    If InStr(lblCohort.Text, Format(Today, "yyyy")) = 0 Then
                        lblCohort.Text = lblCohort.Text & " " & Format(Today, "yyyy")
                    End If
                ElseIf CDate(ASBor.Loans(1, x)) >= CDate("10/01/" & Format(Today.AddYears(-2), "yyyy")) And _
                CDate(ASBor.Loans(1, x)) <= CDate("9/30/" & Format(Today.AddYears(-1), "yyyy")) Then
                    If InStr(lblCohort.Text, Format(Today.AddYears(-1), "yyyy")) = 0 Then
                        lblCohort.Text = lblCohort.Text & " " & Format(Today.AddYears(-1), "yyyy")
                    End If
                End If
            End If
        Next x

        'Demo Source
        lblAddSource.Text = ASBor.AddressSource
        lblPhoneSource.Text = ASBor.PhoneSource

        'Borrower Activity History
        If ASBor.DateLastCntct <> "" Then lblDtLastCntct.Text = ASBor.DateLastCntct
        If ASBor.DateLastAtempt <> "" Then lblDtAttempt.Text = ASBor.DateLastAtempt

        Dim CBPIneligible As Boolean = False
        If ASBor.BorLoanStatus = "PIF" Or IsNothing(ASBor.BorLoanStatus) Then CBPIneligible = True
        SS = New SP.frmScriptsAndServices(ASBor.SSN, NoteDUDE, lvSS, lblNumDaysDelinquent.Text, lblDueDt.Text, ASBor, ASBor.UserProvidedDemos.UPAddrVer, ASBor.UserProvidedDemos.UPPhoneNumVer, ASBor.UserProvidedDemos.UPEmailVer, Me.Text, "Auxiliary Services", CBPIneligible)

        'write out data for scripts
        ASBor.WriteOut()

        SP.Processing.Visible = False

        Me.Showdialog()
        Return True
        'End If
        'Return False
    End Function

    Function TranslateLoanType(ByVal prg As String) As String
        TranslateLoanType = ""
        If prg = "STFFRD" Then Return "SF"
        If prg = "UNSTFD" Then Return "SU"
        If prg = "SLS" Then Return "SL"
        If prg = "PLUS" Then Return "PL"
        If prg = "UNSPC" Then Return "SP"
        If prg = "SUBSPC" Then Return "SP"
        If prg = "SUBCNS" Then Return "CL"
        If prg = "UNCNS" Then Return "CL"
        If prg = "GRAD PLUS" Then Return "GB"
        If prg = "TILP" Then Return "TI"
    End Function

    Sub setServicer()
        'Dim LG02Data(6, 0) As String
        Dim x As Integer
        'LG02Data = ParentFrm.LG02Data
        '1 = loan type
        '2 = servicer code
        '3 = date processed
        '4 = guarantee amount
        '5 = loan status
        '6 = loan status reason code
        For x = 1 To ASBor.LG10Data.GetUpperBound(1)
            If ASBor.LG10Data(2, x) = "700126" And ASBor.LG10Data(5, x) <> "AL" And ASBor.LG10Data(5, x) <> "CA" And ASBor.LG10Data(5, x) <> "CP" And ASBor.LG10Data(5, x) <> "DN" And ASBor.LG10Data(5, x) <> "PC" And ASBor.LG10Data(5, x) <> "PF" And ASBor.LG10Data(5, x) <> "PM" And ASBor.LG10Data(5, x) <> "PN" And ASBor.LG10Data(5, x) <> "RF" Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("BSV")
                End If
            End If
            If ASBor.LG10Data(2, x) <> "813285" And _
                ASBor.LG10Data(2, x) = "821623" And _
                ASBor.LG10Data(2, x) = "820164" And _
                ASBor.LG10Data(2, x) = "821614" And _
                ASBor.LG10Data(5, x) <> "AL" And ASBor.LG10Data(5, x) <> "CA" And ASBor.LG10Data(5, x) <> "CP" And ASBor.LG10Data(5, x) <> "DN" And ASBor.LG10Data(5, x) <> "PC" And ASBor.LG10Data(5, x) <> "PF" And ASBor.LG10Data(5, x) <> "PM" And ASBor.LG10Data(5, x) <> "PN" And ASBor.LG10Data(5, x) <> "RF" Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("CHELA")
                End If
            End If
            If ASBor.LG10Data(2, x) <> "700126" And _
                ASBor.LG10Data(2, x) <> "813285" And _
                ASBor.LG10Data(2, x) <> "821623" And _
                ASBor.LG10Data(2, x) <> "820164" And _
                ASBor.LG10Data(2, x) <> "821614" And _
                ASBor.LG10Data(2, x) <> "830151" And _
                ASBor.LG10Data(2, x) <> "831053" And _
                ASBor.LG10Data(2, x) <> "899993" And _
                ASBor.LG10Data(2, x) <> "899986" And _
                ASBor.LG10Data(2, x) <> "829587" And _
                ASBor.LG10Data(2, x) <> "833253" And _
                ASBor.LG10Data(2, x) <> "888885" And _
                ASBor.LG10Data(2, x) <> "831474" And _
                ASBor.LG10Data(2, x) <> "830084" And _
                ASBor.LG10Data(5, x) <> "AL" And ASBor.LG10Data(5, x) <> "CA" And ASBor.LG10Data(5, x) <> "CP" And ASBor.LG10Data(5, x) <> "DN" And ASBor.LG10Data(5, x) <> "PC" And ASBor.LG10Data(5, x) <> "PF" And ASBor.LG10Data(5, x) <> "PM" And ASBor.LG10Data(5, x) <> "PN" And ASBor.LG10Data(5, x) <> "RF" Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("Nelnet")
                End If
            End If
            If ASBor.LG10Data(2, x) <> "700126" And _
                            ASBor.LG10Data(2, x) = "830151" And _
                            ASBor.LG10Data(2, x) = "831053" And _
                            ASBor.LG10Data(2, x) = "899993" And _
                            ASBor.LG10Data(2, x) = "899986" And _
                            ASBor.LG10Data(2, x) = "829587" And _
                            ASBor.LG10Data(2, x) = "833253" And _
                            ASBor.LG10Data(2, x) = "888885" And _
                            ASBor.LG10Data(2, x) = "831474" And _
                            ASBor.LG10Data(2, x) = "830084" And _
                            ASBor.LG10Data(5, x) <> "AL" And ASBor.LG10Data(5, x) <> "CA" And ASBor.LG10Data(5, x) <> "CP" And ASBor.LG10Data(5, x) <> "DN" And ASBor.LG10Data(5, x) <> "PC" And ASBor.LG10Data(5, x) <> "PF" And ASBor.LG10Data(5, x) <> "PM" And ASBor.LG10Data(5, x) <> "PN" And ASBor.LG10Data(5, x) <> "RF" Then
                If lstServicer.FindStringExact("BSV") = -1 Then
                    lstServicer.Items.Add("Salie Mae")
                End If
            End If

        Next




    End Sub

    Sub setCallForward(ByVal HasTPDD As Boolean)
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
        For x = ASBor.LG10Data.GetLowerBound(1) To ASBor.LG10Data.GetUpperBound(1)
            If (ASBor.LG10Data(5, x) = "CP" Or ASBor.LG10Data(5, x) = "DN") And ASBor.LG10Data(6, x) <> "BC" And ASBor.LG10Data(6, x) <> "BO" And ASBor.LG10Data(6, x) <> "BH" Then
                If lstCallF.FindStringExact("LMS: Default 7272") = -1 Then
                    lstCallF.Items.Add("LMS: Default 7272")
                End If
            End If
        Next
        'step 3
        For x = ASBor.LG10Data.GetLowerBound(1) To ASBor.LG10Data.GetUpperBound(1)
            If ASBor.LG10Data(5, x) = "CP" And (ASBor.LG10Data(6, x) = "BC" Or ASBor.LG10Data(6, x) = "BO" Or ASBor.LG10Data(6, x) = "BH") Then
                If lstCallF.FindStringExact("Auxiliary Services 7274") = -1 Then
                    lstCallF.Items.Add("Auxiliary Services 7274")
                    stp3 = True
                End If
            End If
        Next
        'step 4
        For x = ASBor.LG10Data.GetLowerBound(1) To ASBor.LG10Data.GetUpperBound(1)
            If (ASBor.LG10Data(5, x) = "CR") And ASBor.LG10Data(6, x) <> "BC" And ASBor.LG10Data(6, x) <> "BO" And ASBor.LG10Data(6, x) <> "BH" Then
                If lstCallF.FindStringExact("LMS: Preclaim 7246") = -1 Then
                    lstCallF.Items.Add("LMS: Preclaim 7246")
                    stp4 = True
                End If
            End If
        Next
        'step 5
        For x = ASBor.LG10Data.GetLowerBound(1) To ASBor.LG10Data.GetUpperBound(1)
            If ASBor.LG10Data(5, x) = "CR" And (ASBor.LG10Data(6, x) = "BC" Or ASBor.LG10Data(6, x) = "BO" Or ASBor.LG10Data(6, x) = "BH") Then
                If lstCallF.FindStringExact("Auxiliary Services 7274") = -1 Then
                    lstCallF.Items.Add("Auxiliary Services 7274")
                    stp5 = True
                End If
            End If
        Next
        For x = ASBor.LG10Data.GetLowerBound(1) To ASBor.LG10Data.GetUpperBound(1)
            If ASBor.LG10Data(2, x) = "700126" And ASBor.LG10Data(5, x) <> "CA" And ASBor.LG10Data(5, x) <> "PC" And ASBor.LG10Data(5, x) <> "PF" And ASBor.LG10Data(5, x) <> "PM" And ASBor.LG10Data(5, x) <> "PN" And ASBor.LG10Data(5, x) <> "RF" Then
                If lstCallF.FindStringExact("Borrower Services 7294") = -1 Then
                    lstCallF.Items.Add("Borrower Services 7294")
                End If
            End If
        Next

        For x = ASBor.LG10Data.GetLowerBound(1) To ASBor.LG10Data.GetUpperBound(1)
            If ASBor.LG10Data(2, x) = "700126" And ASBor.LG10Data(5, x) = "" Then
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
        For x = ASBor.LG10Data.GetLowerBound(1) To ASBor.LG10Data.GetUpperBound(1)
            If stp3 = False And (ASBor.LG10Data(2, x) = "700079" Or ASBor.LG10Data(2, x) = "700004" Or _
                ASBor.LG10Data(2, x) = "700789" Or ASBor.LG10Data(2, x) = "700191" Or _
                ASBor.LG10Data(2, x) = "700190" Or ASBor.LG10Data(2, x) = "700121") And _
                ASBor.LG10Data(5, x) <> "CA" And ASBor.LG10Data(5, x) <> "PC" And ASBor.LG10Data(5, x) <> "PF" And ASBor.LG10Data(5, x) <> "PM" And ASBor.LG10Data(5, x) <> "PN" And ASBor.LG10Data(5, x) <> "RF" Then
                If lstCallF.FindStringExact("LMS: Default 7272") = -1 Then
                    lstCallF.Items.Add("LMS: Default 7272")
                End If
            End If
        Next

    End Sub

    Function Status() As Collection

        Dim stat As Collection
        stat = New Collection
        stat.Add("A1", "Death")
        stat.Add("A2", "Alleged Death")
        stat.Add("A3", "Disability")
        stat.Add("A4", "Alleged Disability")
        stat.Add("A5", "Bankruptcy")
        stat.Add("A6", "Alleged Bankruptcy")
        stat.Add("A7", "CURE")
        stat.Add("B1", "In Repayment")
        stat.Add("B2", "In Grace")
        stat.Add("B3", "In School")
        stat.Add("B4", "In School/In Grace")
        stat.Add("B5", "Delinquent")
        stat.Add("B5", "Deferment")
        stat.Add("B6", "Forbearance")
        stat.Add("C1", "PIF/Deconverted")
        stat.Add("C2", "Default")
        stat.Add("C3", "Preclaim")
        stat.Add("C4", "Ineligible")
        Return stat
    End Function

    Function GetStatus(ByVal Stat As String, ByVal Rsn As String) As String
        GetStatus = ""
        If Stat = "DE" Or Stat = "Verified Death".ToUpper Then Return "Death"
        If Stat = "Alleged Death".ToUpper Then Return "Alleged Death"
        If Stat = "DI" Or Stat = "Verified Disability".ToUpper Then Return "Disability"
        If Stat = "Alleged Disability".ToUpper Then Return "Alleged Disability"
        If Stat = "BC" Or Stat = "BH" Or Stat = "BO" Or Stat = "Verified Bankruptcy".ToUpper Then Return "Bankruptcy"
        If Stat = "Alleged Bankruptcy".ToUpper Then Return "Alleged Bankruptcy"
        If Stat = "CURE" Then Return "CURE"
        If Stat = "Repayment".ToUpper Then Return "In Repayment"
        If Stat = "IG" Or Stat = "In Grace".ToUpper Then Return "In Grace"
        If Stat = "IA" Or Stat = "In School".ToUpper Then Return "In School"
        If Stat = "LD" Then Return "In School/In Grace"
        If Stat = "DA" Or Stat = "Deferment".ToUpper Then Return "Deferment"
        If Stat = "FB" Or Stat = "Forbearance".ToUpper Then Return "Forbearance"
        If Stat = "CP" And (Rsn = "DF" Or Rsn = "DB" Or Rsn = "DQ" Or Rsn = "DU") Then Return "Default"
        If (Stat = "CR" And Rsn = "DF") Or Stat = "PRE-CLAIM SUBMITTED" Then Return "Preclaim"
        If Stat = "CP" And Rsn = "IN" Then Return "Ineligible"

    End Function
    Sub UpdateDemo()
        'Dim AddValid As Boolean

        lblAddr1.Text = ""
        lblAddr2.Text = ""
        lblAddr3.Text = ""
        lblHome.Text = ""
        lblWork.Text = ""
        lblAlter.Text = ""
        lblEmail.Text = ""
        lblName.Text = ""

        'AddValid = Not Demographic.ckbAddrVal.Checked
        'This populates the Borrowers Demo with the instance of the Demographics screens text boxes
        lblName.Text = ASBor.Name  'Demographic.lblName.Text
        lblAddr1.Text = ASBor.UserProvidedDemos.Addr1  'Demographic.txtAddr1.Text
        lblAddr2.Text = ASBor.UserProvidedDemos.Addr2 'Demographic.txtAddr2.Text

        lblAddr3.Text = ASBor.UserProvidedDemos.City & " " & ASBor.UserProvidedDemos.State & " " & ASBor.UserProvidedDemos.Zip
        If ASBor.UserProvidedDemos.HomePhoneNum <> "" Then
            lblHome.Text = ASBor.UserProvidedDemos.HomePhoneNum 'Demographic.txtPhone1.Text & "-" & Demographic.txtPhone2.Text & "-" & Demographic.txtPhone3.Text
        Else
            lblHome.Text = ""
        End If
        If ASBor.UserProvidedDemos.OtherPhoneNum <> "" Then
            lblWork.Text = ASBor.UserProvidedDemos.OtherPhoneNum 'Demographic.txtOther1.Text & "-" & Demographic.txtOther2.Text & "-" & Demographic.txtOther3.Text
        Else
            lblWork.Text = ""
        End If
        If ASBor.UserProvidedDemos.OtherPhone2Num <> "" Then
            lblAlter.Text = ASBor.UserProvidedDemos.OtherPhone2Num 'Demographic.txtOther21.Text & "-" & Demographic.txtOther22.Text & "-" & Demographic.txtOther23.Text
        Else
            lblAlter.Text = ""
        End If

        lblEmail.Text = ASBor.UserProvidedDemos.Email
        'if address is invalid turn color red
        If ASBor.UserProvidedDemos.UPAddrVal Then
            NormalAddress()
            'pbDown.Visible = False
            'pbUp.Visible = True
        Else
            RedAddress()
            'pbUp.Visible = False
            'pbDown.Visible = True
        End If

        If ASBor.UserProvidedDemos.UPAddrVal Then
            lblAddrVal.Text = "Y"
        Else
            lblAddrVal.Text = "N"
        End If

        If ASBor.UserProvidedDemos.UPPhoneVal Then
            lblHPhoneVal.Text = "Y"
        Else
            lblHPhoneVal.Text = "N"
        End If

        If ASBor.UserProvidedDemos.UPOtherVal Then
            lblOPhoneVal.Text = "Y"
        Else
            lblOPhoneVal.Text = "N"
        End If

        If ASBor.UserProvidedDemos.UPOther2Val Then
            lblO2PhoneVal.Text = "Y"
        Else
            lblO2PhoneVal.Text = "N"
        End If

        If ASBor.UserProvidedDemos.UPEmailVal Then
            lblEmailVal.Text = "Y"
        Else
            lblEmailVal.Text = "N"
        End If


        If ASBor.DemographicsVerified = False Then
            btnUpdateDemo.BackColor = Me.ForeColor
            btnUpdateDemo.ForeColor = Me.BackColor
            lblVerified.Text = "Not Verified"
        Else
            btnUpdateDemo.BackColor = Me.BackColor
            btnUpdateDemo.ForeColor = Me.ForeColor
            lblVerified.Text = "Verified"
        End If

    End Sub
    Sub ThirdPartyYes()
        'show all controls when 3rd party is found
        lblContactType.Visible = False
        gbBorrowerStatus.Visible = True
        If ASBor.BorLite.NoACPBSVCall = False Then gbPaymentInformation.Visible = True
        'If ASBor.NoACPBSVCall = False Then gbAccountHistory.Visible = True
        txtBSHome.Location = New Point(304, 8)
        btnAskDUDE.Location = New Point(704, 8)
        btnDUDE.Location = New Point(792, 8)
        btn411.Location = New Point(880, 8)
        btnSave.Location = New Point(184, 624)
        btnBack.Location = New Point(368, 624)
        btnRefresh.Location = New Point(552, 624)
        'pbSurf.Location = New Point(696, 376)
        Me.MaximumSize = New Size(988, 702)
        Me.Size = New Size(988, 702)
        Me.MinimumSize = New Size(988, 702)
        btnBrightIdea.Location = New Point(16, 3)
        btnUnexpected.Location = New Point(72, 3)

    End Sub

    Sub ThirdPartyNo()
        'hide Unneeded controls when no 3rd part is found
        lblContactType.Visible = True
        gbBorrowerStatus.Visible = False
        gbPaymentInformation.Visible = False
        'gbAccountHistory.Visible = False
        txtBSHome.Location = New Point(16, 8)
        btnAskDUDE.Location = New Point(328, 8)
        btnDUDE.Location = New Point(416, 8)
        btn411.Location = New Point(504, 8)
        btnSave.Location = New Point(24, 624)
        btnBack.Location = New Point(208, 624)
        btnRefresh.Location = New Point(392, 624)
        'pbSurf.Location = New Point(80, 400)
        Me.MinimumSize = New Size(600, 702)
        Me.Size = New Size(600, 702)
        Me.MaximumSize = New Size(600, 702)
        btnBrightIdea.Location = New Point(24, 575)
        btnUnexpected.Location = New Point(80, 575)

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
    Sub NoACPBSVCall(ByVal b As Boolean)
        'if b = true then set fields to invisible else visible
        gbPaymentInformation.Visible = Not b
        'loan history buttons
        lblLoanHistory.Visible = Not b
        btnCompassLoans.Visible = Not b
        btnOneLinkLoans.Visible = Not b
    End Sub

    Sub CloseAllSubForms()
        If (SP.Q.BorrInfo Is Nothing) = False Then SP.Q.BorrInfo.Hide()
        If (Demographic Is Nothing) = False Then Demographic.Hide()
        If (NoteDUDE Is Nothing) = False Then NoteDUDE.Hide()
        If (SS Is Nothing) = False Then SS.Hide()
    End Sub

    Private Sub ckbBor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbBor.CheckedChanged
        If ckbBor.Checked Then
            ckbRef.Checked = False
            If ASBor.DateLastCntct <> "" Then lblDtLastCntct.Text = ASBor.DateLastCntct Else lblDtLastCntct.Text = "None"
            If ASBor.DateLastAtempt <> "" Then lblDtAttempt.Text = ASBor.DateLastAtempt Else lblDtAttempt.Text = "None"
            lblRefID.Text = ""
            lblReference.Text = ""
        End If
    End Sub

    Private Sub ckbRef_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbRef.CheckedChanged
        If ckbRef.Checked Then
            Dim i As Integer
            ckbBor.Checked = False
            If UBound(ASBor.RefereceArr, 2) = 0 Then
                ASBor.GetRefereces()
                i = 0
            End If
            If UBound(ASBor.RefereceArr, 2) = 1 Then
                i = 1
            End If
            If UBound(ASBor.RefereceArr, 2) > 1 Then
                Dim frmRef As New frmReferenceSelect
                frmRef.Showdialog(ASBor.RefereceArr)
                i = frmRef.index
            End If
            If i >= 0 Then
                lblRefID.Text = ASBor.RefereceArr(0, i)
                lblReference.Text = ASBor.RefereceArr(1, i)
                If ASBor.RefereceArr(3, i) <> "" Then lblDtLastCntct.Text = ASBor.RefereceArr(3, i) Else lblDtLastCntct.Text = "None"
                If ASBor.RefereceArr(4, i) <> "" Then lblDtAttempt.Text = ASBor.RefereceArr(4, i) Else lblDtAttempt.Text = "None"
            End If
        End If
    End Sub


    Private Sub btnOneLINKHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOneLINKHistory.Click
        Dim ID As String = ""
        If ckbBor.Checked Then
            ID = ASBor.SSN
        ElseIf ckbRef.Checked Then
            ID = lblRefID.Text
        End If
        Try
            AppActivate("Maui DUDE Borrower " & 365 & " Day Activity History - LP50")
        Catch
            SP.Processing.Visible = True
            SP.Processing.Refresh()
            'SS.ReturnToACP(LMBor.Bor.Queue, LMBor.Bor.SubQueue, LMBor.Bor.ACPSelection)
            AH365 = New frmActivityHistory
            SP.Processing.Visible = False
            AH365.Show(ID, "Maui DUDE Borrower " & 365 & " Day Activity History - LP50", "LP50")
        End Try
    End Sub

    Private Sub btnCompassHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompassHistory.Click
        Dim ID As String = ""
        If ckbBor.Checked Then
            ID = ASBor.SSN
        ElseIf ckbRef.Checked Then
            ID = lblRefID.Text
        End If
        Try
            AppActivate("Maui DUDE Borrower " & 365 & " Day Activity History - TD2A")
        Catch
            SP.Processing.Refresh()
            'SS.ReturnToACP(ASBor.Queue, ASBor.SubQueue, ASBor.ACPSelection)
            AH365 = New frmActivityHistory
            AH365.Show(ID, "Maui DUDE Borrower " & 365 & " Day Activity History - TD2A", "TD2A")
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim str As String
        RefreshSS()
        str = SS.RunRemainingScriptsAndServices()
        Me.Activate()
        If str <> "" Then
            SP.frmWhoaDUDE.WhoaDUDE("Here is a list of scripts that could not be run." & vbLf & str, "These Scripts are Shark Bait")
        End If
        CloseAllSubForms()
        Me.Close()
    End Sub
    Sub RefreshSS()
        Dim x As Integer
        CloseAllSubForms()
        If lvSS.Items.Count > 0 Then
            For x = lvSS.Items.Count To 1 Step -1
                If lvSS.Items(x - 1).SubItems(1).Text = "Queued to Run" Or lvSS.Items(x - 1).SubItems(1).Text = "Documented in Note DUDE" Then
                    lvSS.Items.Item(x - 1).Selected = True
                    SS.UpdateScriptListData()
                End If
            Next
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim str As String
        Dim Line As String
        Dim UserI() As String
        If ASBor.DemographicsVerified = False Then
            SP.frmWhoaDUDE.WhoaDUDE("You gota check the demographics in order to go on Dude!", "Verify Demographics")
            Exit Sub
        End If

        SP.Processing.Visible = True
        If ASBor.BorLite.NoACPBSVCall = False Then
            SS.ReturnToACP(ASBor.BorLite.Queue, ASBor.BorLite.SubQueue, ASBor.BorLite.ACPSelection)
            ASBor.Notes = NoteDUDE.tbNoteText.Text
            ASBor.SpillGuts()
            ASBor.EnterCommentsIntoSystem(True)
            Demographic.UpdateSys()
            str = SS.RunRemainingScriptsAndServices()
            Me.Activate()
            If str <> "" Then
                SP.frmWhoaDUDE.WhoaDUDE("Here is a list of scripts that could not be run." & vbLf & str, "These Scripts are Shark Bait")
            End If
        Else
            Demographic.UpdateSys()
            Me.Activate()
        End If
        CloseAllSubForms()

        'return to favorite screen
        Try
            If Dir$("T:\userinfo.txt") <> "" Then
                FileOpen(1, "T:\UserInfo.txt", OpenMode.Input, OpenAccess.Read)
                Line = LineInput(1)
                UserI = Line.Split(CChar(", "))
                If UserI(5) <> "" Then
                    SP.Q.FastPath(Replace(UserI(5), """", "")) 'remove quotes
                End If
                FileClose(1)
            End If
        Catch ex As Exception

        End Try


        Me.Hide()
        SP.Processing.Visible = False
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        RefreshSS()
    End Sub

    Private Sub lblDirectDebit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDirectDebit.Click
        btnDirectDebit.PerformClick()
    End Sub

    Private Sub lblDDDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDDDate.Click
        btnDirectDebit.PerformClick()
    End Sub

    Private Sub btnDirectDebit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDirectDebit.Click
        If DirectDebit.WindowState = FormWindowState.Minimized Then
            DirectDebit.WindowState = FormWindowState.Maximized
        ElseIf DirectDebit.Visible = True Then
            DirectDebit.BringToFront()
        Else
            DirectDebit.Show(ASBor)
        End If
    End Sub

    Private Sub btnPayHist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPayHist.Click
        If PayHist.WindowState = FormWindowState.Minimized Then
            PayHist.WindowState = FormWindowState.Maximized
        ElseIf PayHist.Visible = True Then
            PayHist.BringToFront()
        Else
            SS.ReturnToACP(ASBor.BorLite.Queue, ASBor.BorLite.SubQueue, ASBor.BorLite.ACPSelection)
            PayHist.Show(CStr(ASBor.OnTime48Monthly))
        End If
    End Sub

    Private Sub txtPayOffDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPayOffDate.KeyPress
        If e.KeyChar = Chr(13) Then
            If IsDate(txtPayOffDate.Text) Then
                SP.Processing.Visible = True
                SP.Processing.Refresh()
                Dim d As Date
                d = CDate(txtPayOffDate.Text)
                ASBor.SystemProc4PayOffDtKeyPress(d)
                lblPayOffAmount.Text = FormatCurrency(ASBor.TotalPayoffAmount, 2)
                txtPayOffDate.Text = d.ToShortDateString
                SP.Processing.Visible = False
            Else
                txtPayOffDate.Text = Today.ToShortDateString
                txtPayOffDate.Focus()
            End If
        Else
            lblPayOffAmount.Text = ""
        End If
    End Sub

    Private Sub btnUpdateDemo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateDemo.Click
        CloseAllSubForms()
        Me.Visible = False
        Demographic.Showdialog(True, True)
        UpdateDemo()
        ASBor.WriteOut()
        Me.Visible = True
    End Sub

    Private Sub btnstatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnstatus.Click
        Try
            AppActivate("Mo Status")
        Catch

            Dim sta As ArrayList

            sta = New ArrayList
            Dim x As Integer
            mostat = New frmMoreStatus
            For x = StatusArr.GetLowerBound(0) To StatusArr.GetUpperBound(0)
                If StatusArr(x) <> "" Then
                    sta.Add(StatusArr(x))
                End If
            Next
            mostat.Show(sta, Me.Opacity, Me.BackColor, Me.ForeColor)
        End Try
    End Sub

    Private Sub btnCompassLoans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompassLoans.Click
        If CompLoans.WindowState = FormWindowState.Minimized Then
            CompLoans.WindowState = FormWindowState.Maximized
        ElseIf CompLoans.Visible = True Then
            CompLoans.BringToFront()
        Else
            SS.ReturnToACP(ASBor.BorLite.Queue, ASBor.BorLite.SubQueue, ASBor.BorLite.ACPSelection)
            'use due day if populated
            If ASBor.DueDay = "" Then
                CompLoans.Show(lblDueDt.Text)
            Else
                CompLoans.Show(ASBor.DueDay)
            End If
        End If
    End Sub

    Private Sub btnOneLinkLoans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOneLinkLoans.Click
        Try
            AppActivate("Maui DUDE OneLINK Loans")
        Catch
            SS.ReturnToACP(ASBor.BorLite.Queue, ASBor.BorLite.SubQueue, ASBor.BorLite.ACPSelection)
            OLoans = New frmOneLinkLoans(ASBor.LG10Data)
            OLoans.Show()
        End Try
    End Sub

    Private Sub btnAskDUDE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAskDUDE.Click
        Try
            AppActivate("Maui DUDE - Ask DUDE")
        Catch
            SP.DisplayAskDude()
        End Try
    End Sub

    Private Sub btnDUDE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDUDE.Click
        SP.Q.RIBM.SwitchToWindow(1)
    End Sub

    Private Sub btn411_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn411.Click
        SP.BorrInfo.Show(ASBor.SSN, ASBor.Name)
    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        If SS.Visible = True Then
            SS.Activate()
        Else
            SS.Show()
        End If
    End Sub

    Private Sub btnAddComments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddComments.Click
        If NoteDUDE.WindowState = FormWindowState.Minimized Then
            NoteDUDE.WindowState = FormWindowState.Maximized
        ElseIf NoteDUDE.Visible = True Then
            NoteDUDE.Activate()
        Else
            'NoteDude goes to Compass
            NoteDUDE.Show(True)
        End If
    End Sub

    Private Sub UnderConstruction()
        SP.frmUnderConstruct.ShowUnderConstruct()
    End Sub

    Private Sub btn3rdParty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn3rdParty.Click
        UnderConstruction()
    End Sub

    Private Sub btnBankruptcy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBankruptcy.Click
        UnderConstruction()
    End Sub

    Private Sub btnCreditBureau_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreditBureau.Click
        UnderConstruction()
    End Sub

    Private Sub btnExitCounseling_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExitCounseling.Click
        UnderConstruction()
    End Sub

    Private Sub btnKeyIdentifier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKeyIdentifier.Click
        UnderConstruction()
    End Sub

    Private Sub btnOFAC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOFAC.Click
        UnderConstruction()
    End Sub

    Private Sub btnOpenQ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenQ.Click
        UnderConstruction()
    End Sub

    Private Sub btnSkipHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSkipHistory.Click
        UnderConstruction()
    End Sub

    Private Sub btnBrightIdea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrightIdea.Click
        If SP.frmEmailComments.BrightIdea() = True Then
            AppActivate(Me.Text)
            Exit Sub
        End If
        AppActivate(Me.Text)
    End Sub

    Private Sub btnUnexpected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnexpected.Click
        If SP.frmEmailComments.UnexpectedResults() = True Then
            AppActivate(Me.Text)
            Exit Sub
        End If
        AppActivate(Me.Text)
    End Sub

    Private Sub btnLegalAddHist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLegalAddHist.Click
        UnderConstruction()
    End Sub


    Private Sub btnLegalPhoneHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLegalPhoneHistory.Click
        UnderConstruction()
    End Sub

    Private Sub btnAddComments2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If NoteDUDE.WindowState = FormWindowState.Minimized Then
            NoteDUDE.WindowState = FormWindowState.Maximized
        ElseIf NoteDUDE.Visible = True Then
            NoteDUDE.Activate()
        Else
            'NoteDude goes to Compass
            NoteDUDE.Show(True)
        End If
    End Sub

End Class
