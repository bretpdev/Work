Public Class frmLoanInfo
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByRef tBor As borrower, ByRef tTheUser As user)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        Bor = tBor
        TheUser = tTheUser
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
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbSSN As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents LVSummaryInfo As System.Windows.Forms.ListView
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents tbSeqNum As System.Windows.Forms.TextBox
    Friend WithEvents cbSchName As System.Windows.Forms.ComboBox
    Friend WithEvents tbSchCode As System.Windows.Forms.TextBox
    Friend WithEvents tbIntRt As System.Windows.Forms.TextBox
    Friend WithEvents tbDisbAmt As System.Windows.Forms.TextBox
    Friend WithEvents cbTerm As System.Windows.Forms.ComboBox
    Friend WithEvents tbTermBegin As System.Windows.Forms.TextBox
    Friend WithEvents tbTermEnd As System.Windows.Forms.TextBox
    Friend WithEvents cbLoanStatus As System.Windows.Forms.ComboBox
    Friend WithEvents cbModify As System.Windows.Forms.CheckBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tbDisbDate As System.Windows.Forms.TextBox
    Friend WithEvents cbEnrlSta As System.Windows.Forms.ComboBox
    Friend WithEvents gbLoanInfoDetail As System.Windows.Forms.GroupBox
    Friend WithEvents cbTermYear As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmLoanInfo))
        Me.LVSummaryInfo = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbName = New System.Windows.Forms.TextBox
        Me.tbSSN = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.tbSeqNum = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.cbSchName = New System.Windows.Forms.ComboBox
        Me.tbSchCode = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.tbIntRt = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.tbDisbAmt = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.cbTerm = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.tbTermBegin = New System.Windows.Forms.TextBox
        Me.tbTermEnd = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.gbLoanInfoDetail = New System.Windows.Forms.GroupBox
        Me.cbTermYear = New System.Windows.Forms.ComboBox
        Me.cbEnrlSta = New System.Windows.Forms.ComboBox
        Me.tbDisbDate = New System.Windows.Forms.TextBox
        Me.cbModify = New System.Windows.Forms.CheckBox
        Me.cbLoanStatus = New System.Windows.Forms.ComboBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.gbLoanInfoDetail.SuspendLayout()
        Me.SuspendLayout()
        '
        'LVSummaryInfo
        '
        Me.LVSummaryInfo.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.LVSummaryInfo.FullRowSelect = True
        Me.LVSummaryInfo.HideSelection = False
        Me.LVSummaryInfo.Location = New System.Drawing.Point(8, 24)
        Me.LVSummaryInfo.MultiSelect = False
        Me.LVSummaryInfo.Name = "LVSummaryInfo"
        Me.LVSummaryInfo.Size = New System.Drawing.Size(356, 260)
        Me.LVSummaryInfo.TabIndex = 0
        Me.LVSummaryInfo.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Loan Seq"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Term Begin"
        Me.ColumnHeader2.Width = 126
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Status"
        Me.ColumnHeader3.Width = 164
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.tbName)
        Me.GroupBox2.Controls.Add(Me.tbSSN)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(716, 40)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(160, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Name"
        '
        'tbName
        '
        Me.tbName.Enabled = False
        Me.tbName.Location = New System.Drawing.Point(196, 12)
        Me.tbName.MaxLength = 13
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(512, 20)
        Me.tbName.TabIndex = 9
        Me.tbName.TabStop = False
        Me.tbName.Text = ""
        '
        'tbSSN
        '
        Me.tbSSN.Enabled = False
        Me.tbSSN.Location = New System.Drawing.Point(36, 12)
        Me.tbSSN.Name = "tbSSN"
        Me.tbSSN.Size = New System.Drawing.Size(120, 20)
        Me.tbSSN.TabIndex = 1
        Me.tbSSN.TabStop = False
        Me.tbSSN.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SSN"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(352, 16)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "Loan Information Summary"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(4, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(340, 16)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Loan Information Detail"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(24, 52)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(120, 16)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "Loan Sequence:"
        '
        'tbSeqNum
        '
        Me.tbSeqNum.Enabled = False
        Me.tbSeqNum.Location = New System.Drawing.Point(144, 48)
        Me.tbSeqNum.Name = "tbSeqNum"
        Me.tbSeqNum.Size = New System.Drawing.Size(36, 20)
        Me.tbSeqNum.TabIndex = 2
        Me.tbSeqNum.Text = ""
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(24, 72)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 16)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "School Name:"
        '
        'cbSchName
        '
        Me.cbSchName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSchName.Location = New System.Drawing.Point(144, 68)
        Me.cbSchName.Name = "cbSchName"
        Me.cbSchName.Size = New System.Drawing.Size(200, 21)
        Me.cbSchName.TabIndex = 3
        '
        'tbSchCode
        '
        Me.tbSchCode.Enabled = False
        Me.tbSchCode.Location = New System.Drawing.Point(224, 92)
        Me.tbSchCode.Name = "tbSchCode"
        Me.tbSchCode.Size = New System.Drawing.Size(120, 20)
        Me.tbSchCode.TabIndex = 27
        Me.tbSchCode.Text = ""
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(24, 120)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 16)
        Me.Label8.TabIndex = 28
        Me.Label8.Text = "Interest Rate:"
        '
        'tbIntRt
        '
        Me.tbIntRt.Enabled = False
        Me.tbIntRt.Location = New System.Drawing.Point(144, 116)
        Me.tbIntRt.Name = "tbIntRt"
        Me.tbIntRt.TabIndex = 4
        Me.tbIntRt.Text = ""
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(24, 140)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(120, 16)
        Me.Label9.TabIndex = 30
        Me.Label9.Text = "Disbursement Date:"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(24, 160)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(120, 16)
        Me.Label10.TabIndex = 32
        Me.Label10.Text = "Disbursement Amount:"
        '
        'tbDisbAmt
        '
        Me.tbDisbAmt.Location = New System.Drawing.Point(144, 156)
        Me.tbDisbAmt.Name = "tbDisbAmt"
        Me.tbDisbAmt.TabIndex = 6
        Me.tbDisbAmt.Text = ""
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(24, 180)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(120, 16)
        Me.Label11.TabIndex = 34
        Me.Label11.Text = "Loan Status:"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(24, 200)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(120, 16)
        Me.Label13.TabIndex = 36
        Me.Label13.Text = "Enrollment Status:"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(24, 220)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(120, 16)
        Me.Label14.TabIndex = 38
        Me.Label14.Text = "Term/Term Year:"
        '
        'cbTerm
        '
        Me.cbTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTerm.Location = New System.Drawing.Point(144, 216)
        Me.cbTerm.Name = "cbTerm"
        Me.cbTerm.Size = New System.Drawing.Size(100, 21)
        Me.cbTerm.TabIndex = 9
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(140, 244)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(80, 16)
        Me.Label15.TabIndex = 40
        Me.Label15.Text = "Term Begin:"
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(140, 264)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(80, 16)
        Me.Label16.TabIndex = 41
        Me.Label16.Text = "Term End:"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(140, 96)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(80, 16)
        Me.Label17.TabIndex = 42
        Me.Label17.Text = "Code:"
        '
        'tbTermBegin
        '
        Me.tbTermBegin.Enabled = False
        Me.tbTermBegin.Location = New System.Drawing.Point(224, 240)
        Me.tbTermBegin.Name = "tbTermBegin"
        Me.tbTermBegin.Size = New System.Drawing.Size(120, 20)
        Me.tbTermBegin.TabIndex = 11
        Me.tbTermBegin.Text = ""
        '
        'tbTermEnd
        '
        Me.tbTermEnd.Enabled = False
        Me.tbTermEnd.Location = New System.Drawing.Point(224, 260)
        Me.tbTermEnd.Name = "tbTermEnd"
        Me.tbTermEnd.Size = New System.Drawing.Size(120, 20)
        Me.tbTermEnd.TabIndex = 12
        Me.tbTermEnd.Text = ""
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.LVSummaryInfo)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 40)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(368, 288)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'gbLoanInfoDetail
        '
        Me.gbLoanInfoDetail.Controls.Add(Me.cbTermYear)
        Me.gbLoanInfoDetail.Controls.Add(Me.cbEnrlSta)
        Me.gbLoanInfoDetail.Controls.Add(Me.tbDisbDate)
        Me.gbLoanInfoDetail.Controls.Add(Me.cbModify)
        Me.gbLoanInfoDetail.Controls.Add(Me.cbLoanStatus)
        Me.gbLoanInfoDetail.Controls.Add(Me.tbIntRt)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label8)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label5)
        Me.gbLoanInfoDetail.Controls.Add(Me.tbSchCode)
        Me.gbLoanInfoDetail.Controls.Add(Me.cbSchName)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label9)
        Me.gbLoanInfoDetail.Controls.Add(Me.tbSeqNum)
        Me.gbLoanInfoDetail.Controls.Add(Me.cbTerm)
        Me.gbLoanInfoDetail.Controls.Add(Me.tbTermBegin)
        Me.gbLoanInfoDetail.Controls.Add(Me.tbTermEnd)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label17)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label16)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label15)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label14)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label13)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label11)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label6)
        Me.gbLoanInfoDetail.Controls.Add(Me.tbDisbAmt)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label10)
        Me.gbLoanInfoDetail.Controls.Add(Me.Label7)
        Me.gbLoanInfoDetail.Location = New System.Drawing.Point(376, 40)
        Me.gbLoanInfoDetail.Name = "gbLoanInfoDetail"
        Me.gbLoanInfoDetail.Size = New System.Drawing.Size(348, 288)
        Me.gbLoanInfoDetail.TabIndex = 2
        Me.gbLoanInfoDetail.TabStop = False
        '
        'cbTermYear
        '
        Me.cbTermYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTermYear.Location = New System.Drawing.Point(244, 216)
        Me.cbTermYear.Name = "cbTermYear"
        Me.cbTermYear.Size = New System.Drawing.Size(100, 21)
        Me.cbTermYear.TabIndex = 10
        '
        'cbEnrlSta
        '
        Me.cbEnrlSta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEnrlSta.Location = New System.Drawing.Point(144, 196)
        Me.cbEnrlSta.Name = "cbEnrlSta"
        Me.cbEnrlSta.Size = New System.Drawing.Size(136, 21)
        Me.cbEnrlSta.TabIndex = 8
        '
        'tbDisbDate
        '
        Me.tbDisbDate.Location = New System.Drawing.Point(144, 136)
        Me.tbDisbDate.Name = "tbDisbDate"
        Me.tbDisbDate.TabIndex = 5
        Me.tbDisbDate.Text = ""
        '
        'cbModify
        '
        Me.cbModify.Location = New System.Drawing.Point(144, 24)
        Me.cbModify.Name = "cbModify"
        Me.cbModify.Size = New System.Drawing.Size(60, 24)
        Me.cbModify.TabIndex = 1
        Me.cbModify.Text = "Modify"
        '
        'cbLoanStatus
        '
        Me.cbLoanStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLoanStatus.Location = New System.Drawing.Point(144, 176)
        Me.cbLoanStatus.Name = "cbLoanStatus"
        Me.cbLoanStatus.Size = New System.Drawing.Size(136, 21)
        Me.cbLoanStatus.TabIndex = 7
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(232, 332)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(120, 23)
        Me.btnSave.TabIndex = 13
        Me.btnSave.Text = "Save"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(376, 332)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 23)
        Me.btnCancel.TabIndex = 14
        Me.btnCancel.Text = "Cancel"
        '
        'frmLoanInfo
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(728, 384)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.gbLoanInfoDetail)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(736, 392)
        Me.MinimumSize = New System.Drawing.Size(736, 392)
        Me.Name = "frmLoanInfo"
        Me.Text = "Loan Information"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.gbLoanInfoDetail.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Bor As borrower
    Private TheLoans As ArrayList
    Private DS As New DataSet
    Private DA As SqlClient.SqlDataAdapter
    Private CurrentIntRate As String
    Private SelectedSeqNum As Integer 'tracks which seq number the form is on
    Private LVSelectedIdxChg As EventHandler
    Private CBModifyChkChg As EventHandler
    Private LVClick As EventHandler
    Private MaxSeqNum As Integer
    Private TheUser As user


    Private Sub frmLoanInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim LI As Integer
        Dim YearCounter As Integer
        Dim I As Integer
        'set header fields
        tbSSN.Text = Bor.getKey
        If Bor.MI <> "" Then
            tbName.Text = Bor.FN + " " + Bor.MI + " " + Bor.LN
        Else
            tbName.Text = Bor.FN + " " + Bor.LN
        End If
        'set up event handlers
        'these handlers needed to be manually made for greater control over when the event fires
        LVSelectedIdxChg = New EventHandler(AddressOf LVSummaryInfoSelection)
        AddHandler LVSummaryInfo.SelectedIndexChanged, LVSelectedIdxChg
        LVClick = New EventHandler(AddressOf LVSummaryInfoSelection)
        AddHandler LVSummaryInfo.Click, LVClick
        CBModifyChkChg = New EventHandler(AddressOf cbModifyCheckedChanged)
        AddHandler cbModify.CheckedChanged, CBModifyChkChg
        'init controls
        'set up data adapters based off test mode var
        If Bor.TestMode Then
            DA = New Data.SqlClient.SqlDataAdapter("SELECT Term FROM TermsList WHERE Valid = 1 ORDER BY Term", "Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            DA = New Data.SqlClient.SqlDataAdapter("SELECT Term FROM TermsList WHERE Valid = 1 ORDER BY Term", "Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        'fill data sets for control population
        DA.Fill(DS, "Terms")
        DA.SelectCommand.CommandText = "SELECT SchoolName, SchoolCode FROM ParticipatingSchoolsList WHERE Valid = 1 ORDER BY SchoolName"
        DA.Fill(DS, "Schools")
        DA.SelectCommand.CommandText = "SELECT LoanStatDesc FROM LoanStatusList WHERE Valid = 1  ORDER BY LoanStatDesc"
        DA.Fill(DS, "LoanStats")
        DA.SelectCommand.CommandText = "SELECT Status FROM EnrollStatList WHERE Valid = 1 ORDER BY Status"
        DA.Fill(DS, "EnrollStats")
        DA.SelectCommand.CommandText = "SELECT * FROM IRateDat WHERE (IntEndDate IS NULL) OR (DATEDIFF(d,IntBeginDate,'" + DateTime.Now + "') >= 0 AND DATEDIFF(d,'" + DateTime.Now + "',IntEndDate) >= 0)"
        DA.Fill(DS, "IntRate")
        'figure out interest rate
        If DS.Tables("IntRate").Select("IntEndDate IS NOT NULL").GetLength(0) <> 0 Then
            CurrentIntRate = DS.Tables("IntRate").Select("IntEndDate IS NOT NULL")(0).Item("IntRate")
        ElseIf DS.Tables("IntRate").Select("IntEndDate IS NULL").GetLength(0) <> 0 Then
            CurrentIntRate = DS.Tables("IntRate").Select("IntEndDate IS NULL")(0).Item("IntRate")
        Else
            CurrentIntRate = ""
        End If
        'load data to combo boxes
        While LI < DS.Tables("Terms").Rows.Count
            cbTerm.Items.Add(DS.Tables("Terms").Rows(LI).Item("Term"))
            LI = LI + 1
        End While
        LI = 0
        While LI < DS.Tables("Schools").Rows.Count
            cbSchName.Items.Add(DS.Tables("Schools").Rows(LI).Item("SchoolName"))
            LI = LI + 1
        End While
        LI = 0
        While LI < DS.Tables("LoanStats").Rows.Count
            cbLoanStatus.Items.Add(DS.Tables("LoanStats").Rows(LI).Item("LoanStatDesc"))
            LI = LI + 1
        End While
        LI = 0
        While LI < DS.Tables("EnrollStats").Rows.Count
            cbEnrlSta.Items.Add(DS.Tables("EnrollStats").Rows(LI).Item("Status"))
            LI = LI + 1
        End While
        'figure out years options for term year
        YearCounter = DateTime.Now.Year()
        YearCounter = YearCounter - 1
        For I = 0 To 3
            cbTermYear.Items.Add(YearCounter)
            YearCounter = YearCounter + 1
        Next
        'get DB info from object and init controls
        PopForm()
    End Sub

    'populates form for use
    Private Sub PopForm()
        Dim I As Integer
        Dim LVI As ListViewItem
        MaxSeqNum = Bor.BorLoans.MaxSeqNum
        SelectedSeqNum = 0
        'get data from object
        Bor.BorLoans.GetData(TheLoans)
        'blank list view in case it is already populated
        While LVSummaryInfo.Items.Count > 0
            LVSummaryInfo.Items.RemoveAt(0)
        End While
        'populate summary list view with DB data
        While I < TheLoans.Count
            LVI = New ListViewItem(CStr(CType(TheLoans(I), loan).SeqNum))
            LVI.SubItems.Add(CType(TheLoans(I), loan).TermBeginDt)
            LVI.SubItems.Add(CType(TheLoans(I), loan).LnSta)
            LVSummaryInfo.Items.Add(LVI)
            I = I + 1
        End While
        'only add "Add Loan" option if user doesn't have authority level 3
        If TheUser.GetAccessLevel() <> 3 And TheUser.GetAccessLevel() <> 4 Then
            'add an additional list item to be used for adding loans
            LVI = New ListViewItem("Add Loan")
            LVSummaryInfo.Items.Add(LVI)
        End If
        'if there are no loans then disable save button
        If TheLoans.Count = 0 Then
            btnSave.Enabled = False
        End If
        'select the first list view item index changed event will handle 
        If LVSummaryInfo.Items.Count > 0 Then 'the list may not have an option to select if there are no loans and the user has authority level 3 access (no adding of loans)
            'there is something to select in the list of loans
            'only select first list item if not "Add Loan" option
            If LVSummaryInfo.Items(0).Text <> "Add Loan" Then
                LVSummaryInfo.Items(0).Selected = True
            End If
            I = 0
            While I < gbLoanInfoDetail.Controls.Count
                gbLoanInfoDetail.Controls(I).Enabled = False
                I = I + 1
            End While
        Else
            'there is nothing to select in the list
            AuthorityLevelCheck() 'disable controls if authority is level 3
        End If
    End Sub

    Private Sub LVSummaryInfoSelection(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim L As loan
        Dim LVI As ListViewItem
        'make sure the list view has something selected
        If LVSummaryInfo.SelectedItems.Count > 0 Then
            'enable save button because something may have been changed at this point
            btnSave.Enabled = True 'button may be disabled in authority check call
            'save info to form level info if there was something selected previously
            SaveDataToFormVars()
            'handle adding loan if that is where the user clicked
            If LVSummaryInfo.SelectedItems(0).Text = "Add Loan" Then
                'check if there are any other loans in a pending disbursement, if there are then don't allow the user to add a loan
                If PendingDisbCheck() = False Then
                    Exit Sub
                End If
                'check if the borrower is new loan eligible
                If Bor.BorLoans.AddLoanEligible() = False Then
                    If OverrideApproved("Number") = False Then
                        Exit Sub 'can't add so exit sub
                    End If
                End If
                'create new loan
                cbModify.Enabled = False
                cbTerm.SelectedItem = -1
                cbSchName.SelectedItem = -1
                cbLoanStatus.SelectedItem = -1
                MaxSeqNum = MaxSeqNum + 1
                L = New loan
                L.SeqNum = MaxSeqNum
                L.IntRate = CurrentIntRate
                L.LnSta = "Pending Disbursement"
                'insert new list view item
                LVI = New ListViewItem(CStr(L.SeqNum))
                LVSummaryInfo.Items.Insert(TheLoans.Count, LVI)
                RemoveHandler LVSummaryInfo.SelectedIndexChanged, LVSelectedIdxChg
                LVSummaryInfo.Items(TheLoans.Count).Selected = True
                AddHandler LVSummaryInfo.SelectedIndexChanged, LVSelectedIdxChg
                LVSummaryInfo.Items.RemoveAt(LVSummaryInfo.Items.Count - 1)
                'add loan object to array list
                TheLoans.Add(L)
            End If
            SelectedSeqNum = CInt(LVSummaryInfo.SelectedItems(0).Text)
            'populate data from newly selected loan
            PopControls()
            AuthorityLevelCheck()
        End If
    End Sub

    'cycles through all loans and makes sure that there is no other loan in a pending disbursement
    Private Function PendingDisbCheck() As Boolean
        Dim I As Integer
        For I = 0 To (TheLoans.Count - 1)
            If CType(TheLoans(I), loan).LnSta = "Pending Disbursement" Then
                MsgBox("There is already a loan in a pending disbursment status.  A borrower may only have one loan in a pending disbursment status at a time.", MsgBoxStyle.Information, "Loan In Pending Disbursment Status Already Found")
                Return False 'pending disbursement already in place
            End If
        Next
        Return True 'no pending disbursement found
    End Function

    'checks for override
    Function OverrideApproved(ByVal Mode As String) As Boolean
        If Mode = "Number" Then
            'check if the user should be allowed to override the 8 loan limitation
            If TheUser.GetAccessLevel() = 1 Then
                'give user prompt that notifies them of the 8 loan limitation but allows them to override the limitation
                If MessageBox.Show("This borrower has reached the award limit (8 awards) allotted by the Board of Regents.  Special approval from the Board of Regents is needed for this loan." & vbLf & vbLf & "Do you wish to override this limitation?", "Override Award Limitation?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                    Return True
                Else
                    Return False
                End If
            Else
                MessageBox.Show("This borrower has reached the award limit (8 awards) allotted by the Board of Regents.  Special approval from the Board of Regents is needed for this loan.", "Borrower Reached Award Limitation", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Return False
            End If
        Else
            'check if the user should be allowed to override the 8 loan limitation
            If TheUser.GetAccessLevel() = 1 Then
                'give user prompt that notifies them of the 8 loan limitation but allows them to override the limitation
                If MessageBox.Show("Adding the loan which is in a ""Pending Disbursement"" status will put the borrower over the slot amount limit for this academic year.  Special approval from the Board of Regents is needed for this loan." & vbLf & vbLf & "Do you wish to override this limitation?", "Override Award Limitation?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                    Return True
                Else
                    Return False
                End If
            Else
                MessageBox.Show("Adding the loan which is in a ""Pending Disbursement"" status will put the borrower over the slot amount limit for this academic year.  Special approval from the Board of Regents is needed for this loan.", "Borrower Reached Award Limitation", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Return False
            End If
        End If
    End Function

    'saves changes to temporary form storage
    Private Sub SaveDataToFormVars()
        Dim I As Integer
        'do nothing if selected index is 0, which means nothing was selected previous to this
        If SelectedSeqNum <> 0 Then
            'figure out which item in array list needs to be saved
            While SelectedSeqNum <> CType(TheLoans(I), loan).SeqNum
                I = I + 1
            End While
            'save data to loan vars
            CType(TheLoans(I), loan).SeqNum = CInt(tbSeqNum.Text)
            If (cbSchName.SelectedItem Is Nothing) = False Then CType(TheLoans(I), loan).SchName = cbSchName.SelectedItem
            CType(TheLoans(I), loan).SchCode = tbSchCode.Text
            CType(TheLoans(I), loan).IntRate = CDbl(tbIntRt.Text)
            CType(TheLoans(I), loan).DisbDt = tbDisbDate.Text
            CType(TheLoans(I), loan).DisbAmt = tbDisbAmt.Text
            If (cbLoanStatus.SelectedItem Is Nothing) = False Then CType(TheLoans(I), loan).LnSta = cbLoanStatus.SelectedItem
            If (cbEnrlSta.SelectedItem Is Nothing) = False Then CType(TheLoans(I), loan).EnrolSta = cbEnrlSta.SelectedItem
            If (cbTerm.SelectedItem Is Nothing) = False Then CType(TheLoans(I), loan).Term = cbTerm.SelectedItem
            If (cbTermYear.SelectedItem Is Nothing) = False Then CType(TheLoans(I), loan).TermYear = cbTermYear.SelectedItem
            CType(TheLoans(I), loan).TermBeginDt = tbTermBegin.Text
            CType(TheLoans(I), loan).TermEndDt = tbTermEnd.Text
            CType(TheLoans(I), loan).Modified = cbModify.Checked
        End If
    End Sub

    'populates controls based of list view selected index
    Private Sub PopControls()
        Dim I As Integer
        If TheLoans.Count <> 0 Then
            'figure out which item in array list needs to be used
            While SelectedSeqNum <> CType(TheLoans(I), loan).SeqNum
                I = I + 1
            End While
            'populate contols from loan object in array list
            tbSeqNum.Text = CType(TheLoans(I), loan).SeqNum
            If CType(TheLoans(I), loan).SchName <> "" Then
                cbSchName.SelectedItem = CType(TheLoans(I), loan).SchName
            Else
                cbSchName.SelectedIndex = -1
            End If
            tbIntRt.Text = CType(TheLoans(I), loan).IntRate
            tbDisbDate.Text = CType(TheLoans(I), loan).DisbDt
            tbDisbAmt.Text = CType(TheLoans(I), loan).DisbAmt
            cbLoanStatus.SelectedItem = CType(TheLoans(I), loan).LnSta
            If CType(TheLoans(I), loan).EnrolSta <> "" Then
                cbEnrlSta.SelectedItem = CType(TheLoans(I), loan).EnrolSta
            Else
                cbEnrlSta.SelectedIndex = -1
            End If
            If CType(TheLoans(I), loan).Term <> "" Then
                cbTerm.SelectedItem = CType(TheLoans(I), loan).Term
            Else
                cbTerm.SelectedIndex = -1
            End If
            'if loan hasn't been saved to DB yet then make it possible for the user to select a term and term year
            If CType(TheLoans(I), loan).AlreadyExisting = False Then
                cbTermYear.Visible = True
                cbTerm.Enabled = True
                If CType(TheLoans(I), loan).TermYear = "" Then
                    cbTermYear.SelectedIndex = -1
                Else
                    cbTermYear.SelectedItem = CInt(CType(TheLoans(I), loan).TermYear)
                End If
            Else
                cbTerm.Enabled = False
                cbTermYear.Visible = False
            End If
            tbTermBegin.Text = CType(TheLoans(I), loan).TermBeginDt
            tbTermEnd.Text = CType(TheLoans(I), loan).TermEndDt
            'disable and enable controls based off source of loan 
            'If CType(TheLoans(I), loan).AlreadyExisting Then
            '    SetEnableAndDisableLoanInfoDetailCrls(True)
            'Else
            '    SetEnableAndDisableLoanInfoDetailCrls(False)
            'End If
            'if marked to modify then remove handler, mark as modify, set enable controls, and add handler back
            If CType(TheLoans(I), loan).Modified = True Then
                RemoveHandler cbModify.CheckedChanged, CBModifyChkChg
                cbModify.Checked = CType(TheLoans(I), loan).Modified
                SetEnableAndDisableLoanInfoDetailCrls(False, True)
                AddHandler cbModify.CheckedChanged, CBModifyChkChg
            Else
                If CType(TheLoans(I), loan).AlreadyExisting = False Then
                    'if new
                    RemoveHandler cbModify.CheckedChanged, CBModifyChkChg
                    cbModify.Checked = False
                    SetEnableAndDisableLoanInfoDetailCrls(False, False)
                    tbDisbDate.Enabled = False
                    cbLoanStatus.Enabled = False
                    AddHandler cbModify.CheckedChanged, CBModifyChkChg
                Else
                    'if existing
                    RemoveHandler cbModify.CheckedChanged, CBModifyChkChg
                    cbModify.Checked = False
                    SetEnableAndDisableLoanInfoDetailCrls(True, True)
                    AddHandler cbModify.CheckedChanged, CBModifyChkChg
                End If
            End If
        End If
    End Sub

    Private Sub SetEnableAndDisableLoanInfoDetailCrls(ByVal DisableCtrls As Boolean, Optional ByVal Modifying As Boolean = False)
        Dim I As Integer
        While I < gbLoanInfoDetail.Controls.Count
            'be sure to not enable fields that are being pre-populated
            If gbLoanInfoDetail.Controls(I).Name <> "tbSeqNum" And gbLoanInfoDetail.Controls(I).Name <> "tbSchCode" And _
               gbLoanInfoDetail.Controls(I).Name <> "tbIntRt" And gbLoanInfoDetail.Controls(I).Name <> "tbTermBegin" And _
               gbLoanInfoDetail.Controls(I).Name <> "tbTermEnd" And gbLoanInfoDetail.Controls(I).Name <> "cbTerm" Then
                gbLoanInfoDetail.Controls(I).Enabled = (Not (DisableCtrls))
            End If
            I = I + 1
        End While
        'enable or disable check box based off previous existance of loan
        If Modifying = False Then
            If DisableCtrls Then
                cbModify.Enabled = True
            Else
                cbModify.Enabled = False
            End If
        Else
            cbModify.Enabled = True
        End If
        'disable disbursement date field if authority level 5
        If TheUser.GetAccessLevel() = 5 Then
            tbDisbDate.Enabled = False
        End If
    End Sub

    'handles the index changing for school name
    Private Sub cbSchName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSchName.SelectedIndexChanged
        Dim DR() As DataRow
        If cbSchName.SelectedIndex = -1 Then
            tbSchCode.Clear()
        Else
            DR = DS.Tables("Schools").Select("SchoolName = '" + cbSchName.SelectedItem + "'")
            tbSchCode.Text = DR(0).Item("SchoolCode").ToString()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    'handles the data checks and calls the obejct sub to save to DB once data checks out
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim I As Integer
        Dim AllowedAmtPerSlot As Decimal
        Dim YearBegin As String
        Dim YearEnd As String
        Dim NewBorrowerEligible As Boolean
        Dim ID As Long
        Dim AnticipatedDisbAmt As Decimal
        Dim AwardAmtAvilblForYear As Decimal
        Dim LnStaChg2RejOrBorDecl As Boolean
        LnStaChg2RejOrBorDecl = False
        'save current record the user is on
        SaveDataToFormVars()
        'cycle through all loans, do data checks against all new and modified loans
        While I < TheLoans.Count
            If CType(TheLoans(I), loan).AlreadyExisting = False Or CType(TheLoans(I), loan).Modified = True Then
                'check if everything is populated other than disb date
                If CType(TheLoans(I), loan).SchName = "" Then
                    MessageBox.Show("A school name was not entered or was deleted on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + ".  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                ElseIf CType(TheLoans(I), loan).SchCode = "" Then
                    MessageBox.Show("A school code was not entered or was deleted on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + ".  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                ElseIf CType(TheLoans(I), loan).DisbAmt = "" Then
                    MessageBox.Show("A disbursement amount was not entered or was deleted on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + ".  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                ElseIf CType(TheLoans(I), loan).EnrolSta = "" Then
                    MessageBox.Show("An enrollment status was not entered or was deleted on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + ".  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                ElseIf CType(TheLoans(I), loan).Term = "" Then
                    MessageBox.Show("A term was not entered or was deleted on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + ".  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                ElseIf CType(TheLoans(I), loan).TermBeginDt = "" Then
                    MessageBox.Show("A term begin date was not entered or was deleted on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + ".  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                ElseIf CType(TheLoans(I), loan).TermEndDt = "" Then
                    MessageBox.Show("A term end date was not entered or was deleted on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + ".  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                End If
                'check if disb date is given if status is disbursed
                If CType(TheLoans(I), loan).LnSta = "Disbursed" And IsDate(CType(TheLoans(I), loan).DisbDt) = False Then
                    MessageBox.Show("The disbursement date entered on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + " is not a date.  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                End If
                'do disb date data check if populated
                If CType(TheLoans(I), loan).DisbDt <> "" Then
                    If IsDate(CType(TheLoans(I), loan).DisbDt) = False Then
                        MessageBox.Show("The disbursement date entered on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + " is not a date.  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        LVSummaryInfo.Items(I).Selected = True
                        Exit Sub
                    End If
                    If CDate(CType(TheLoans(I), loan).DisbDt) >= Format(DateTime.Now.AddDays(1), "MM/dd/yy 00:00:00") Then
                        MessageBox.Show("The disbursement date entered on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + " is not today or earlier.  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        LVSummaryInfo.Items(I).Selected = True
                        Exit Sub
                    End If
                End If
                'do disb amt data check
                If isDec(CType(TheLoans(I), loan).DisbAmt) = False Then
                    MessageBox.Show("The disbursement amount entered on loan " + CStr(CType(TheLoans(I), loan).SeqNum) + " is not valid.  The amount must only have numbers and a single decimal point.  Please fix the problem and try and save your changes again.", "Data Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LVSummaryInfo.Items(I).Selected = True
                    Exit Sub
                Else
                    CType(TheLoans(I), loan).DisbAmt = Format(CDbl(CType(TheLoans(I), loan).DisbAmt), "######0.00")
                End If
                'check if loan is in a "Rejected" or "Borrower Declined" status 
                If CType(TheLoans(I), loan).LnSta = "Rejected" Or CType(TheLoans(I), loan).LnSta = "Borrower Declined" Then
                    'if the logic has got to this point then the user has updated a loan to be borrower declined or rejected and is trying to save the change
                    LnStaChg2RejOrBorDecl = True 'flag so slot can be removed in SlotProc call below
                End If
                'check if loan is in a "Pending Disbursment" status
                If CType(TheLoans(I), loan).LnSta = "Pending Disbursement" Then
                    'if the loan is in a pending disbursement status
                    AnticipatedDisbAmt = CDec(CType(TheLoans(I), loan).DisbAmt)
                    'get the slot amount and term begin, end dates and new borrower eligible indicator
                    If Bor.BorLoans.RetSchoolSlotInfo(CType(TheLoans(I), loan).SchCode, CType(TheLoans(I), loan).TermBeginDt, YearBegin, YearEnd, AllowedAmtPerSlot, NewBorrowerEligible, ID, AwardAmtAvilblForYear) = False Then
                        MsgBox("Slot and academic year information can't be found for school code: " + CType(TheLoans(I), loan).SchCode + ".  Please call Systems Support for assistance.", MsgBoxStyle.Critical, "School Not Found")
                        Exit Sub
                    Else
                        'check if the borrower has exceeded the alloted dollar amount for the year
                        If Bor.BorLoans.GetTotalSumOfLoansForYear(YearBegin, YearEnd) + CDec(CType(TheLoans(I), loan).DisbAmt) > AllowedAmtPerSlot Then
                            If OverrideApproved("Amount") = False Then
                                Exit Sub
                            End If
                        End If
                    End If
                    'check if the school can accept new borrowers
                    If NewBorrowerEligible = False Then
                        If MsgBox("Does the the borrower already have TILP loans for " + CType(TheLoans(I), loan).SchName + " (" + CType(TheLoans(I), loan).SchCode + ")?", MsgBoxStyle.YesNo, "New Borrower?") <> vbYes Then
                            MsgBox("The selected school isn't accepting new TILP borrowers currently.  Please contact the manager of Account Services.", MsgBoxStyle.Information, "School Not Accepting New Borrowers")
                            Exit Sub
                        End If
                    End If
                End If
            End If
                I = I + 1
        End While
        'Do Slot Functionality and checks
        If ID > 0 Or LnStaChg2RejOrBorDecl = True Then
            'skip all other processing in sub it function returns false
            If Bor.BorLoans.SlotProc(ID, AnticipatedDisbAmt, TheUser.GetAccessLevel(), LnStaChg2RejOrBorDecl) = False Then Exit Sub
        End If
        'if logic makes it through all data checks then save to master object and save to DB
        Bor.BorLoans.SaveLoanData(TheLoans, TheUser.GetUID)
        'let user know that everthing was updated
        MessageBox.Show("The borrower's loan information has been updated.", "Data Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'update form data
        PopForm()
    End Sub


    Private Sub tbDisbDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDisbDate.TextChanged
        If tbDisbDate.Text = "" Then
            cbLoanStatus.SelectedItem = "Pending Disbursement"
        Else
            cbLoanStatus.SelectedItem = "Disbursed"
        End If
    End Sub

    Private Function isDec(ByRef TheStr As String) As Boolean
        Dim iA As Integer = 0
        Dim NumOfDec As Integer
        Do While iA <= TheStr.Length - 1
            If TheStr.Chars(iA) = "." Then
                'if decimal
                NumOfDec = NumOfDec + 1
            Else
                'if not a decimal
                If Char.IsNumber(TheStr.Chars(iA)) = False Then
                    Return False
                End If
            End If
            iA = iA + 1
        Loop
        If NumOfDec <> 1 Then
            Return False
        End If
        Return True
    End Function

    Private Sub cbModifyCheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'do not allow user to modify if loan is in disbursed status
        If cbLoanStatus.SelectedItem = "Disbursed" Or cbLoanStatus.SelectedItem = "Borrower Declined" Or cbLoanStatus.SelectedItem = "Rejected" Then
            If cbModify.Checked Then
                MessageBox.Show("A loan must be in a ""Pending Disbursement"" status in order to allow updates.", "Loan Has Disbursed Status", MessageBoxButtons.OK, MessageBoxIcon.Error)
                cbModify.Checked = False
            End If
        Else
            SetEnableAndDisableLoanInfoDetailCrls((Not cbModify.Checked), True)
        End If
    End Sub

    Private Sub cbTerm_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTerm.SelectedIndexChanged
        If cbTermYear.Visible Then
            If cbTermYear.SelectedIndex <> -1 And cbTerm.SelectedIndex <> -1 Then
                FigureTermDates()
            End If
        End If
    End Sub

    Private Sub FigureTermDates()
        If cbTerm.SelectedItem = "Fall" Then
            tbTermBegin.Text = "08/25/" + cbTermYear.SelectedItem.ToString()
            tbTermEnd.Text = "12/15/" + cbTermYear.SelectedItem.ToString()
        ElseIf cbTerm.SelectedItem = "Spring" Then
            tbTermBegin.Text = "01/03/" + cbTermYear.SelectedItem.ToString()
            tbTermEnd.Text = "04/25/" + cbTermYear.SelectedItem.ToString()
        ElseIf cbTerm.SelectedItem = "Summer" Then
            tbTermBegin.Text = "05/20/" + cbTermYear.SelectedItem.ToString()
            tbTermEnd.Text = "08/10/" + cbTermYear.SelectedItem.ToString()
        Else
            MessageBox.Show("A very bad thing happened.  Please contact Systems Support.", "A Very Bad Thing Happened", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End If
    End Sub

    Private Sub cbTermYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTermYear.SelectedIndexChanged
        If cbTermYear.SelectedIndex <> -1 And cbTerm.SelectedIndex <> -1 Then
            FigureTermDates()
        End If
    End Sub


    Private Sub AuthorityLevelCheck()
        Dim I As Integer
        'if user has level 3 authority then disable all updatable controls so the user can't update
        If TheUser.GetAccessLevel() = 3 Then
            While I < gbLoanInfoDetail.Controls.Count
                gbLoanInfoDetail.Controls(I).Enabled = False
                I = I + 1
            End While
            btnSave.Enabled = False
        End If
        If TheUser.GetAccessLevel() = 4 Then
            cbModify.Enabled = False
            btnSave.Enabled = False
        End If
    End Sub

End Class
