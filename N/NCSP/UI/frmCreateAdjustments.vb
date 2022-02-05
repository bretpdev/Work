Public Class frmCreateAdjustments
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

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
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtAccntID As System.Windows.Forms.TextBox
    Friend WithEvents btnGet As System.Windows.Forms.Button
    Friend WithEvents lstSchedules As System.Windows.Forms.ListView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents hdrHeader As NCSP.FormHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCreateAdjustments))
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtAccntID = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lstSchedules = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnGet = New System.Windows.Forms.Button
        Me.hdrHeader = New NCSP.FormHeader
        Me.Panel3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.Panel3.Controls.Add(Me.Label33)
        Me.Panel3.Controls.Add(Me.Label32)
        Me.Panel3.Controls.Add(Me.GroupBox8)
        Me.Panel3.Location = New System.Drawing.Point(0, 630)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1000, 24)
        Me.Panel3.TabIndex = 60
        '
        'Label33
        '
        Me.Label33.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label33.Location = New System.Drawing.Point(544, -2)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(424, 23)
        Me.Label33.TabIndex = 63
        Me.Label33.Text = "Create Adjustments"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label32
        '
        Me.Label32.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label32.Location = New System.Drawing.Point(16, -2)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(424, 23)
        Me.Label32.TabIndex = 62
        Me.Label32.Text = "New Century Scholarship Program"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox8
        '
        Me.GroupBox8.Location = New System.Drawing.Point(0, -8)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(992, 8)
        Me.GroupBox8.TabIndex = 61
        Me.GroupBox8.TabStop = False
        '
        'GroupBox9
        '
        Me.GroupBox9.Location = New System.Drawing.Point(0, 622)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(1000, 8)
        Me.GroupBox9.TabIndex = 61
        Me.GroupBox9.TabStop = False
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.SystemColors.ControlText
        Me.Panel4.Location = New System.Drawing.Point(0, 62)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(992, 1)
        Me.Panel4.TabIndex = 62
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnSave.Enabled = False
        Me.btnSave.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(824, 112)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(144, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'txtAccntID
        '
        Me.txtAccntID.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccntID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccntID.Location = New System.Drawing.Point(160, 80)
        Me.txtAccntID.Name = "txtAccntID"
        Me.txtAccntID.Size = New System.Drawing.Size(144, 20)
        Me.txtAccntID.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(16, 80)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 20)
        Me.Label7.TabIndex = 81
        Me.Label7.Text = "Account ID"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbType)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.lstSchedules)
        Me.GroupBox1.Controls.Add(Me.txtAmount)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(16, 104)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(784, 504)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Adjustment Information"
        '
        'cmbType
        '
        Me.cmbType.Enabled = False
        Me.cmbType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbType.Items.AddRange(New Object() {"", "Supplemental Payment", "Reversal", "School Refund", "Student Repayment", "Check to Student", "Balance Write Off"})
        Me.cmbType.Location = New System.Drawing.Point(144, 24)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(288, 21)
        Me.cmbType.TabIndex = 1
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(16, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(120, 20)
        Me.Label12.TabIndex = 89
        Me.Label12.Text = "Type"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 20)
        Me.Label1.TabIndex = 88
        Me.Label1.Text = "Schedules"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lstSchedules
        '
        Me.lstSchedules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lstSchedules.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstSchedules.FullRowSelect = True
        Me.lstSchedules.GridLines = True
        Me.lstSchedules.HideSelection = False
        Me.lstSchedules.Location = New System.Drawing.Point(144, 72)
        Me.lstSchedules.MultiSelect = False
        Me.lstSchedules.Name = "lstSchedules"
        Me.lstSchedules.Size = New System.Drawing.Size(624, 416)
        Me.lstSchedules.TabIndex = 3
        Me.lstSchedules.UseCompatibleStateImageBehavior = False
        Me.lstSchedules.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Status"
        Me.ColumnHeader1.Width = 147
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Semester"
        Me.ColumnHeader2.Width = 100
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Year"
        Me.ColumnHeader3.Width = 49
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Institution"
        Me.ColumnHeader4.Width = 160
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Hours Enrolled"
        Me.ColumnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader5.Width = 82
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Hours Paid"
        Me.ColumnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader6.Width = 82
        '
        'txtAmount
        '
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Enabled = False
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.Location = New System.Drawing.Point(144, 48)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(144, 20)
        Me.txtAmount.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 20)
        Me.Label2.TabIndex = 86
        Me.Label2.Text = "Amount"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnGet
        '
        Me.btnGet.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGet.Location = New System.Drawing.Point(824, 80)
        Me.btnGet.Name = "btnGet"
        Me.btnGet.Size = New System.Drawing.Size(144, 23)
        Me.btnGet.TabIndex = 1
        Me.btnGet.Text = "Get Schedules"
        '
        'hdrHeader
        '
        Me.hdrHeader.Location = New System.Drawing.Point(-1, -1)
        Me.hdrHeader.Name = "hdrHeader"
        Me.hdrHeader.Size = New System.Drawing.Size(994, 64)
        Me.hdrHeader.TabIndex = 82
        '
        'frmCreateAdjustments
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(992, 653)
        Me.Controls.Add(Me.hdrHeader)
        Me.Controls.Add(Me.btnGet)
        Me.Controls.Add(Me.txtAccntID)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1000, 680)
        Me.Name = "frmCreateAdjustments"
        Me.Text = "Create Adjustments"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel3.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Class Atrributes #"
    Private DA As SqlClient.SqlDataAdapter
    Private SC As SqlClient.SqlCommand
    Private DS As New DataSet
#End Region

    Private Sub btnGet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGet.Click
        lstSchedules.Items.Clear()
        DisplayAccountInfo()
        DS.Tables.Clear()
        'get adjustment
        SC = New SqlClient.SqlCommand("select SchedID, SchedStat, Semester, SchedYr, InstAtt, CredHrEnr, CredHrPd from Schedule where AcctID = '" & txtAccntID.Text & "' and RowStatus = 'A' AND SchedStat <> 'Adding'", dbConnection)
        DA = New SqlClient.SqlDataAdapter(SC)
        Try
            dbConnection.Open()
            DA.Fill(DS)
            dbConnection.Close()
        Catch ex As Exception
            'sql error
            cmbType.Enabled = False
            txtAmount.Enabled = False
            txtAmount.BackColor = Me.BackColor
            btnSave.Enabled = False
            MsgBox("No schedules were found for the entered account number.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            txtAccntID.Focus()
            Exit Sub
        End Try
        'no records found
        If DS.Tables(0).Rows.Count = 0 Then
            cmbType.Enabled = False
            txtAmount.Enabled = False
            txtAmount.BackColor = Me.BackColor
            btnSave.Enabled = False
            MsgBox("No schedules were found for the entered account number.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            txtAccntID.Focus()
            Exit Sub
        End If

        'enable fields
        cmbType.Enabled = True
        txtAmount.Enabled = True
        txtAmount.BackColor = txtAccntID.BackColor
        btnSave.Enabled = True

        'add to dataview
        Dim R As DataRow
        For Each R In DS.Tables(0).Rows
            Dim L As New ListViewItem
            L.Text = R.Item("SchedStat")
            L.SubItems.Add(R.Item("Semester")) '1
            L.SubItems.Add(R.Item("SchedYr")) '2
            L.SubItems.Add(R.Item("InstAtt")) '3
            L.SubItems.Add(Format(R.Item("CredHrEnr"), "0.00")) '4
            L.SubItems.Add(Format(R.Item("CredHrPd"), "0.00")) '5
            L.SubItems.Add(R.Item("SchedID")) '6 R.Item("SchedID")
            lstSchedules.Items.Add(L)
        Next
        cmbType.Focus()
    End Sub

    Sub DisplayAccountInfo()
        DS.Tables.Clear()
        GetdbConnection()
        'get account info
        SC = New SqlClient.SqlCommand("select AcctID, Fname, MI, LName, Status, Balance, CredHrRem from Account where AcctID = '" & txtAccntID.Text & "' and RowStatus = 'A'", dbConnection)
        DA = New SqlClient.SqlDataAdapter(SC)
        Try
            dbConnection.Open()
            DA.Fill(DS)
            dbConnection.Close()
        Catch ex As Exception
            'sql error
            cmbType.Enabled = False
            txtAmount.Enabled = False
            txtAmount.BackColor = Me.BackColor
            btnSave.Enabled = False
            MsgBox("The account was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            txtAccntID.Focus()
            Exit Sub
        End Try
        'no records found
        If DS.Tables(0).Rows.Count = 0 Then
            cmbType.Enabled = False
            txtAmount.Enabled = False
            txtAmount.BackColor = Me.BackColor
            btnSave.Enabled = False
            MsgBox("The account was not found", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            txtAccntID.Focus()
            Exit Sub
        End If
        hdrHeader.txtAccountID.Text = DS.Tables(0).Rows(0).Item("AcctID")
        hdrHeader.txtStudentName.Text = DS.Tables(0).Rows(0).Item("FName") & " " & DS.Tables(0).Rows(0).Item("MI") & " " & DS.Tables(0).Rows(0).Item("LName")
        hdrHeader.txtStatus.Text = DS.Tables(0).Rows(0).Item("Status")
        hdrHeader.txtBalance.Text = FormatCurrency(CDbl(DS.Tables(0).Rows(0).Item("Balance")), 2)
        hdrHeader.txtHoursRemaining.Text = Format(CDbl(DS.Tables(0).Rows(0).Item("CredHrRem")), "0.00")

    End Sub

    Private Sub frmCreateAdjustments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbType.Enabled = False
        txtAmount.Enabled = False
        txtAmount.BackColor = Me.BackColor
        btnSave.Enabled = False
        txtAccntID.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsNumeric(txtAmount.Text) = False Then
            MsgBox("The amount field must be numeric.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        End If

        If lstSchedules.SelectedItems.Count = 0 And (cmbType.SelectedItem = "Supplemental Payment" Or cmbType.SelectedItem = "Reversal" Or cmbType.SelectedItem = "School Refund") Then
            MsgBox("You must select a schedule.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        End If
        'add transaction
        Dim TranTyp As String = String.Empty
        Dim TranDt As String = String.Empty
        Dim NCSPBatchNum As String = String.Empty
        Dim NCSPBatchDt As String = String.Empty
        Dim AcctID As String = String.Empty
        Dim SchedSemEnr As String = String.Empty
        Dim SchedYrEnr As Integer = 0
        Dim SchedInst As String = String.Empty
        Dim TranInst As String = String.Empty
        Dim TranHrPd As Double = 0
        Dim TranAmt As Double = 0
        Dim SchedID As String = String.Empty
        If lstSchedules.SelectedItems.Count > 0 Then SchedID = lstSchedules.SelectedItems(0).SubItems(6).Text

        If cmbType.SelectedItem = "Supplemental Payment" Or cmbType.SelectedItem = "Reversal" Or cmbType.SelectedItem = "School Refund" Then
            'get institution ID
            Dim sqlCmd As New SqlClient.SqlCommand
            Dim sqlRdr As SqlClient.SqlDataReader
            sqlCmd.Connection = dbConnection
            dbConnection.Open()
            sqlCmd.CommandText = "SELECT InstID FROM Inst WHERE InstLong = '" & lstSchedules.SelectedItems(0).SubItems(3).Text & "'"
            sqlRdr = sqlCmd.ExecuteReader
            sqlRdr.Read()
            SchedInst = sqlRdr("InstID")
            sqlRdr.Close()
            dbConnection.Close()
        End If

        If cmbType.Text = "" Then
            MsgBox("The adjustment type must be populated.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        ElseIf cmbType.Text = "Supplemental Payment" Then
            TranTyp = "Supplemental Payment"
            TranDt = Now
            NCSPBatchNum = ""
            NCSPBatchDt = ""
            AcctID = hdrHeader.txtAccountID.Text
            SchedSemEnr = lstSchedules.SelectedItems(0).SubItems(1).Text
            SchedYrEnr = lstSchedules.SelectedItems(0).SubItems(2).Text
            TranInst = ""
            TranHrPd = 0
            TranAmt = Math.Abs(CDbl(txtAmount.Text)) 'absolut value
            If Not AddTransaction(TranTyp, AcctID, SchedSemEnr, SchedYrEnr, SchedInst, TranHrPd, TranAmt, SchedID) Then Exit Sub
            AddToAccountBallance(TranAmt, AcctID)
            'Add Comunication Record
            AddCommunications(AcctID, TranDt, False, False, "Account Maintenance", "Supplemental Payment for " & SchedSemEnr & " " & SchedYrEnr & " for " & TranAmt & " entered.", "Supplemental Payment Entered")

        ElseIf cmbType.Text = "Reversal" Or cmbType.Text = "School Refund" Then
            'ElseIf cmbType.Text = "School Refund" Then
            TranTyp = "Adjustment"
            TranDt = Now
            NCSPBatchNum = ""
            NCSPBatchDt = ""
            AcctID = hdrHeader.txtAccountID.Text
            SchedSemEnr = lstSchedules.SelectedItems(0).SubItems(1).Text
            SchedYrEnr = lstSchedules.SelectedItems(0).SubItems(2).Text
            TranInst = ""
            TranHrPd = Math.Abs(CDbl(lstSchedules.SelectedItems(0).SubItems(5).Text)) * (-1)
            TranAmt = GetScholarshipAmount(CInt(SchedYrEnr)) * -1

            If Not AddTransaction(TranTyp, AcctID, SchedSemEnr, SchedYrEnr, SchedInst, TranHrPd, TranAmt, SchedID) Then Exit Sub
            If cmbType.Text = "Reversal" Then
                updateSchedule(AcctID, lstSchedules.SelectedItems(0).SubItems(6).Text, "CredHrComp = '0', SemesterGPA = '0.0', SchedStat = 'Withdrawn'")
            Else
                updateSchedule(AcctID, lstSchedules.SelectedItems(0).SubItems(6).Text, "CredHrComp = '0', SemesterGPA = '0.0'")
            End If
            TranTyp = cmbType.Text
            TranDt = Now
            NCSPBatchNum = ""
            NCSPBatchDt = ""
            AcctID = hdrHeader.txtAccountID.Text
            SchedSemEnr = lstSchedules.SelectedItems(0).SubItems(1).Text
            SchedYrEnr = lstSchedules.SelectedItems(0).SubItems(2).Text
            TranInst = ""
            TranHrPd = 0
            TranAmt = Math.Abs(CDbl(txtAmount.Text)) 'absolut value
            If Not AddTransaction(TranTyp, AcctID, SchedSemEnr, SchedYrEnr, SchedInst, TranHrPd, TranAmt, SchedID) Then Exit Sub
            'Increase Credit Hours Remaining
            updateAccountByAttribute(AcctID, "CredHrRem", (CDbl(GetFromSchedule("CredHrEnr", AcctID, lstSchedules.SelectedItems(0).SubItems(6).Text)) + CDbl(GetFromAccount("CredHrRem", AcctID))).ToString)

            'Decrease the account balance by the adjustment amount and increase it by the reversal amount (just do it)
            AddToAccountBallance(GetScholarshipAmount(CInt(SchedYrEnr)) * -1, AcctID)
            AddToAccountBallance(TranAmt, AcctID)

            Dim Comment As String
            Comment = cmbType.Text & " for " & SchedSemEnr & " " & SchedYrEnr & " for " & TranAmt & " received."
            If GetFromAccount("Status", AcctID) = "Closed" And _
            GetFromAccount("EligEndRea", AcctID) = "Eligibility Used" And _
            CDbl(GetFromAccount("CredHrRem", AcctID)) > 0 Then
                updateAccount2(AcctID, "Status = 'Approved', EligEndRea = '', EligEndCom = ''")
                Comment = Comment & " Account Reactivated."
            End If
            'Add Comunication Record
            AddCommunications(AcctID, TranDt, True, False, "Account Maintenance", Comment, cmbType.Text & " entered")

        ElseIf cmbType.Text = "Student Repayment" Then
            TranTyp = cmbType.Text
            TranDt = Now
            NCSPBatchNum = ""
            NCSPBatchDt = ""
            AcctID = hdrHeader.txtAccountID.Text
            SchedSemEnr = ""
            SchedYrEnr = 0
            SchedInst = ""
            TranInst = ""
            TranHrPd = 0
            TranAmt = Math.Abs(CDbl(txtAmount.Text)) 'absolut value
            If Not AddTransaction(TranTyp, AcctID, SchedSemEnr, SchedYrEnr, SchedInst, TranHrPd, TranAmt, SchedID, NCSPBatchNum, NCSPBatchDt, "Entered") Then Exit Sub
            AddToAccountBallance(TranAmt, AcctID)
            AddCommunications(AcctID, TranDt, True, False, "Account Maintenance", "Student Repayment for " & FormatCurrency(txtAmount.Text, 2) & " received.", "Payment Received from Student")

        ElseIf cmbType.Text = "Check to Student" Then
            TranTyp = cmbType.Text
            TranDt = Now
            NCSPBatchNum = ""
            NCSPBatchDt = ""
            AcctID = hdrHeader.txtAccountID.Text
            SchedSemEnr = ""
            SchedYrEnr = 0
            SchedInst = ""
            TranInst = ""
            TranHrPd = 0
            TranAmt = Math.Abs(CDbl(txtAmount.Text)) * (-1) 'negative value
            If Not AddTransaction(TranTyp, AcctID, SchedSemEnr, SchedYrEnr, SchedInst, TranHrPd, TranAmt, 0, ) Then Exit Sub
            AddToAccountBallance(TranAmt, AcctID)
            AddCommunications(AcctID, TranDt, False, True, "Account Maintenance", "Check for " & FormatCurrency(txtAmount.Text, 2) & " for over payment sent to student.", "Check for Overpayment Sent to Student")

        ElseIf cmbType.Text = "Balance Write Off" Then
            TranTyp = cmbType.Text
            TranDt = Now
            NCSPBatchNum = ""
            NCSPBatchDt = ""
            AcctID = hdrHeader.txtAccountID.Text
            SchedSemEnr = ""
            SchedYrEnr = 0
            SchedInst = ""
            TranInst = ""
            TranHrPd = 0
            TranAmt = CDbl(txtAmount.Text)
            If Not AddTransaction(TranTyp, AcctID, SchedSemEnr, SchedYrEnr, SchedInst, TranHrPd, TranAmt, 1) Then Exit Sub
            AddToAccountBallance(TranAmt, AcctID)
            AddCommunications(AcctID, TranDt, False, True, "Account Maintenance", "Balance write off adjustment for " & FormatCurrency(txtAmount.Text, 2) & " entered to reconcile account balance due to program regulation change.", "Balance Write Off Adjustment")
        End If


        cmbType.SelectedIndex = 0
        txtAmount.Text = ""
        btnGet.PerformClick()
        MsgBox("The adjustment has been entered.", MsgBoxStyle.Information, "New Century Scholarship Program")
    End Sub

    Sub updateAccount2(ByVal Acct As String, ByVal Update As String)
        'Updates the schedule table with a specific attribute and value for a specific account and schedule ID
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String
        SC = New SqlClient.SqlCommand("select AcctRecSeq from Account where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
        Try
            'Get Active row id
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The account ID was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Exit Sub
            End If
            val = sqlRdr("AcctRecSeq")
            sqlRdr.Close()

            'Duplicate Active row!
            SC = New SqlClient.SqlCommand("insert Account (AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, LastUpdateDt, LastUpdateUser, RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied, LOAYearReturn) select AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, GETDATE(), '" & UserID & "', RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied, LOAYearReturn from Account Where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()

            'Set old record status to "H" for history
            SC = New SqlClient.SqlCommand("update Account set RowStatus = 'H', LockedBy = '' where AcctRecSeq = '" & val & "'", dbConnection)
            SC.ExecuteNonQuery()

            'Update new record
            SC = New SqlClient.SqlCommand("update Account set " & Update & " where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The account update was not completed.  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Exit Sub
        Finally
            dbConnection.Close()
        End Try
    End Sub

    Sub updateSchedule(ByVal Acct As String, ByVal SchedID As Long, ByVal Update As String)
        'Updates the schedule table with a specific attribute and value for a specific account and schedule ID
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String
        SC = New SqlClient.SqlCommand("select SchdlRecSeq from Schedule where AcctID = '" & Acct & "' and RowStatus = 'A' and SchedID = " & SchedID, dbConnection)
        Try
            'Get Active row id
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The schedule ID was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Exit Sub
            End If
            val = sqlRdr("SchdlRecSeq")
            sqlRdr.Close()

            'Duplicate Active row
            SC = New SqlClient.SqlCommand("insert Schedule (AcctID, SchedID, SchedDt, SchedStat, SchedHrRem, CredHrPd, InstAtt, Semester, SchedYr, CredHrEnr, CredHrComp, SemesterGPA, LTH, LastUpdateDt, LastUpdateUser, RowStatus, LockedBy) select AcctID, SchedID, SchedDt, SchedStat, SchedHrRem, CredHrPd, InstAtt, Semester, SchedYr, CredHrEnr, CredHrComp, SemesterGPA, LTH, GETDATE(), '" & UserID & "', RowStatus, LockedBy from Schedule Where AcctID = '" & Acct & "' and SchedID = " & SchedID & " and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()

            'Set old record status to "H" for history
            SC = New SqlClient.SqlCommand("update Schedule set RowStatus = 'H', LockedBy = '' where SchdlRecSeq = " & val, dbConnection)
            SC.ExecuteNonQuery()

            'Update New Record
            SC = New SqlClient.SqlCommand("update Schedule set " & Update & " where AcctID = '" & Acct & "' and RowStatus = 'A' and SchedID = " & SchedID, dbConnection)
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The schedule update was not completed.  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Exit Sub
        Finally
            dbConnection.Close()
        End Try
    End Sub

    Private Sub txtAccntID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAccntID.KeyPress
        If e.KeyChar = Chr(13) Then
            'Enter pressed 
            btnGet.PerformClick()
        End If
    End Sub

    Private Sub txtAmount_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmount.Leave
        If InStr(txtAmount.Text, ".") Then
            If IsNumeric(txtAmount.Text) Then
                txtAmount.Text = Format(CDbl(txtAmount.Text), "###0.00")
            Else
                txtAmount.Text = "0.00"
            End If
        Else
            If IsNumeric(txtAmount.Text) Then
                txtAmount.Text = Format(CDbl(txtAmount.Text) * 0.01, "###0.00")
            Else
                txtAmount.Text = "0.00"
            End If
        End If

    End Sub
End Class
