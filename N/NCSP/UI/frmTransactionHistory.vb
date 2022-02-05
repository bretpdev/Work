Public Class frmTransactionHistory
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
    Friend WithEvents btnStudentInfo As System.Windows.Forms.Button
    Friend WithEvents cmbCommunications As System.Windows.Forms.ComboBox
    Friend WithEvents btnSchedules As System.Windows.Forms.Button
    Friend WithEvents lstTransactions As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents hdrHeader As NCSP.FormHeader
    Friend WithEvents ColumnHeader16 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTransactionHistory))
        Me.btnStudentInfo = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.cmbCommunications = New System.Windows.Forms.ComboBox
        Me.btnSchedules = New System.Windows.Forms.Button
        Me.lstTransactions = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader13 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader14 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader15 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader16 = New System.Windows.Forms.ColumnHeader
        Me.hdrHeader = New NCSP.FormHeader
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnStudentInfo
        '
        Me.btnStudentInfo.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStudentInfo.Location = New System.Drawing.Point(824, 520)
        Me.btnStudentInfo.Name = "btnStudentInfo"
        Me.btnStudentInfo.Size = New System.Drawing.Size(144, 23)
        Me.btnStudentInfo.TabIndex = 40
        Me.btnStudentInfo.Text = "Student Information"
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
        Me.Label33.Text = "Transaction History"
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
        'cmbCommunications
        '
        Me.cmbCommunications.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCommunications.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCommunications.Items.AddRange(New Object() {"Communications", "Add", "All", "Call", "Letter", "Fax", "Email", "Account Maintenance"})
        Me.cmbCommunications.Location = New System.Drawing.Point(824, 584)
        Me.cmbCommunications.Name = "cmbCommunications"
        Me.cmbCommunications.Size = New System.Drawing.Size(144, 24)
        Me.cmbCommunications.TabIndex = 77
        '
        'btnSchedules
        '
        Me.btnSchedules.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSchedules.Location = New System.Drawing.Point(824, 552)
        Me.btnSchedules.Name = "btnSchedules"
        Me.btnSchedules.Size = New System.Drawing.Size(144, 23)
        Me.btnSchedules.TabIndex = 78
        Me.btnSchedules.Text = "Schedules"
        '
        'lstTransactions
        '
        Me.lstTransactions.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.lstTransactions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader13, Me.ColumnHeader14, Me.ColumnHeader15, Me.ColumnHeader16})
        Me.lstTransactions.FullRowSelect = True
        Me.lstTransactions.GridLines = True
        Me.lstTransactions.Location = New System.Drawing.Point(16, 80)
        Me.lstTransactions.Name = "lstTransactions"
        Me.lstTransactions.Size = New System.Drawing.Size(784, 528)
        Me.lstTransactions.TabIndex = 79
        Me.lstTransactions.TabStop = False
        Me.lstTransactions.UseCompatibleStateImageBehavior = False
        Me.lstTransactions.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Type"
        Me.ColumnHeader1.Width = 120
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Date"
        Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader2.Width = 70
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Batch"
        Me.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Batch Date"
        Me.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader4.Width = 70
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Semester"
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Year"
        Me.ColumnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader6.Width = 40
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Institution"
        Me.ColumnHeader7.Width = 57
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Inst Paid"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Hours"
        Me.ColumnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader9.Width = 54
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Amount"
        Me.ColumnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader10.Width = 73
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Check"
        Me.ColumnHeader13.Width = 46
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "Check Date"
        Me.ColumnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader14.Width = 70
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "Status"
        '
        'ColumnHeader16
        '
        Me.ColumnHeader16.Text = "User"
        '
        'hdrHeader
        '
        Me.hdrHeader.Location = New System.Drawing.Point(-2, -1)
        Me.hdrHeader.Name = "hdrHeader"
        Me.hdrHeader.Size = New System.Drawing.Size(994, 64)
        Me.hdrHeader.TabIndex = 80
        '
        'frmTransactionHistory
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(992, 653)
        Me.Controls.Add(Me.hdrHeader)
        Me.Controls.Add(Me.lstTransactions)
        Me.Controls.Add(Me.btnSchedules)
        Me.Controls.Add(Me.cmbCommunications)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.btnStudentInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1000, 680)
        Me.Name = "frmTransactionHistory"
        Me.Text = "Transaction History"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private sqlCmd As New SqlClient.SqlCommand
    Private sqlRdr As SqlClient.SqlDataReader

    'display transactions
    Private Sub frmTransactionHistory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim AccountID As String
        'Dim StuName As String
        'Dim StuStatus As String
        'Dim StuBalance As String
        'Dim StuHours As String
        'Dim StuPaidSchedules As String

        ''get account information for header
        'frmStudentsForm.StuInfo(AccountID, StuName, StuStatus, StuBalance, StuHours, StuPaidSchedules)
        'hdrHeader.txtAccountID.Text = AccountID
        'hdrHeader.txtStudentName.Text = StuName
        'hdrHeader.txtStatus.Text = StuStatus
        'hdrHeader.txtBalance.Text = StuBalance
        'hdrHeader.txtHoursRemaining.Text = StuHours
        'hdrHeader.txtPaidSchedules.Text = StuPaidSchedules

        'get account information for header
        frmStudentsForm.StuInfo(hdrHeader.txtAccountID.Text, hdrHeader.txtStudentName.Text, hdrHeader.txtStatus.Text, hdrHeader.txtBalance.Text, hdrHeader.txtHoursRemaining.Text, hdrHeader.txtPaidSchedules.Text)

        'set text of communications drop down
        cmbCommunications.Text = "Communications"

        Dim dataAdapter As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet

        'clear list view
        dataSet.Clear()
        lstTransactions.Items.Clear()

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'get transactions to display
        sqlCmd.CommandText = "SELECT * FROM [Transaction] WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' ORDER BY TranDt DESC"

        dataAdapter = New SqlClient.SqlDataAdapter(sqlCmd)

        'fill data set if transactions were found or warn user if none were found
        Try
            dataAdapter.Fill(dataSet)
        Catch ex As Exception
            MsgBox("There are no financial transactions for the student.", MsgBoxStyle.Information, "New Century Scholarship Program")
            dbConnection.Close()
            Exit Sub
        End Try

        dbConnection.Close()

        'fill list view from dataset
        Dim r As DataRow
        For Each r In dataSet.Tables(0).Rows
            Dim l As New ListViewItem

            l.Text = CStr(r.Item("TranTyp"))
            l.SubItems.Add(Format(DateValue(CStr(r.Item("TranDt"))), "MM/dd/yyyy"))
            l.SubItems.Add(CStr(r.Item("NCSPBatchNum")))
            If IsDBNull(r.Item("NCSPBatchDt")) Then l.SubItems.Add("") Else l.SubItems.Add(Format(DateValue(CStr(r.Item("NCSPBatchDt"))), "MM/dd/yyyy"))
            l.SubItems.Add(CStr(r.Item("SchedSemEnr")))
            l.SubItems.Add(CStr(r.Item("SchedYrEnr")))
            l.SubItems.Add(CStr(r.Item("SchedInst")))
            l.SubItems.Add(CStr(r.Item("TranInst")))
            l.SubItems.Add(Format(r.Item("TranHrPd"), "#0.00;(#0.00)"))
            l.SubItems.Add(Format(r.Item("TranAmt"), "$###,##0.00;($###,##0.00)"))
            l.SubItems.Add(CStr(r.Item("AcctCheckNum")))
            If IsDBNull(r.Item("AcctCheckDt")) Then l.SubItems.Add("") Else l.SubItems.Add(Format(DateValue(CStr(r.Item("AcctCheckDt"))), "MM/dd/yyyy"))
            l.SubItems.Add(CStr(r.Item("TranStat")))
            l.SubItems.Add(CStr(r.Item("UserID")))

            lstTransactions.Items.Add(l)
        Next
    End Sub

    Private Sub btnStudentInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStudentInfo.Click
        ShowForms.CloseForms()
    End Sub

    Private Sub btnSchedules_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSchedules.Click
        ShowForms.Schedules()
        Me.Close()
    End Sub

    Private Sub cmbCommunications_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCommunications.SelectedIndexChanged
        If cmbCommunications.SelectedItem <> "Communications" Then
            ShowForms.Communications(cmbCommunications.SelectedItem)
        End If
    End Sub
End Class
