Imports System.Collections.Generic
Imports System.Linq

Public Class frmPaymentPosting
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
    Friend WithEvents lable3 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnPost As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtBatchNo As System.Windows.Forms.TextBox
    Friend WithEvents cmbBatchNo As System.Windows.Forms.ComboBox
    Friend WithEvents txtBatchDate As System.Windows.Forms.TextBox
    Friend WithEvents txtBatchAmt As System.Windows.Forms.TextBox
    Friend WithEvents dtgInstitutions As System.Windows.Forms.DataGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPaymentPosting))
        Me.lable3 = New System.Windows.Forms.Label
        Me.txtBatchNo = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.btnPost = New System.Windows.Forms.Button
        Me.cmbBatchNo = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtBatchDate = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtBatchAmt = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.dtgInstitutions = New System.Windows.Forms.DataGrid
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.dtgInstitutions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lable3
        '
        Me.lable3.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lable3.ForeColor = System.Drawing.SystemColors.ScrollBar
        Me.lable3.Location = New System.Drawing.Point(16, 8)
        Me.lable3.Name = "lable3"
        Me.lable3.Size = New System.Drawing.Size(136, 16)
        Me.lable3.TabIndex = 20
        Me.lable3.Text = "Batch Number"
        Me.lable3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtBatchNo
        '
        Me.txtBatchNo.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtBatchNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBatchNo.Location = New System.Drawing.Point(16, 24)
        Me.txtBatchNo.Name = "txtBatchNo"
        Me.txtBatchNo.ReadOnly = True
        Me.txtBatchNo.Size = New System.Drawing.Size(136, 20)
        Me.txtBatchNo.TabIndex = 22
        Me.txtBatchNo.Text = ""
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel2.Controls.Add(Me.lable3)
        Me.Panel2.Controls.Add(Me.txtBatchNo)
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(992, 62)
        Me.Panel2.TabIndex = 59
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
        Me.Label33.Text = "Payment Posting"
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
        'btnPost
        '
        Me.btnPost.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPost.Location = New System.Drawing.Point(824, 80)
        Me.btnPost.Name = "btnPost"
        Me.btnPost.Size = New System.Drawing.Size(144, 23)
        Me.btnPost.TabIndex = 68
        Me.btnPost.Text = "Post"
        '
        'cmbBatchNo
        '
        Me.cmbBatchNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBatchNo.Location = New System.Drawing.Point(144, 80)
        Me.cmbBatchNo.Name = "cmbBatchNo"
        Me.cmbBatchNo.Size = New System.Drawing.Size(144, 21)
        Me.cmbBatchNo.TabIndex = 64
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 20)
        Me.Label3.TabIndex = 63
        Me.Label3.Text = "Select Batch"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtBatchDate
        '
        Me.txtBatchDate.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtBatchDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBatchDate.Location = New System.Drawing.Point(144, 112)
        Me.txtBatchDate.Name = "txtBatchDate"
        Me.txtBatchDate.ReadOnly = True
        Me.txtBatchDate.Size = New System.Drawing.Size(144, 20)
        Me.txtBatchDate.TabIndex = 80
        Me.txtBatchDate.Text = ""
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 112)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(120, 20)
        Me.Label6.TabIndex = 79
        Me.Label6.Text = "Batch Date"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtBatchAmt
        '
        Me.txtBatchAmt.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtBatchAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBatchAmt.Location = New System.Drawing.Point(144, 136)
        Me.txtBatchAmt.Name = "txtBatchAmt"
        Me.txtBatchAmt.ReadOnly = True
        Me.txtBatchAmt.Size = New System.Drawing.Size(144, 20)
        Me.txtBatchAmt.TabIndex = 82
        Me.txtBatchAmt.Text = ""
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(16, 136)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 20)
        Me.Label7.TabIndex = 81
        Me.Label7.Text = "Batch Amount"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtgInstitutions
        '
        Me.dtgInstitutions.DataMember = ""
        Me.dtgInstitutions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtgInstitutions.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dtgInstitutions.Location = New System.Drawing.Point(16, 24)
        Me.dtgInstitutions.Name = "dtgInstitutions"
        Me.dtgInstitutions.Size = New System.Drawing.Size(752, 400)
        Me.dtgInstitutions.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dtgInstitutions)
        Me.GroupBox1.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(16, 168)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(784, 440)
        Me.GroupBox1.TabIndex = 74
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Institutions"
        '
        'frmPaymentPosting
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(992, 653)
        Me.Controls.Add(Me.txtBatchAmt)
        Me.Controls.Add(Me.txtBatchDate)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnPost)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.cmbBatchNo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1000, 680)
        Me.Name = "frmPaymentPosting"
        Me.Text = "Payment Posting"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        CType(Me.dtgInstitutions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub frmPaymentPosting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader

        SC = New SqlClient.SqlCommand("select distinct NCSPBatchNum from [Transaction] where AcctCheckNum = '' and NCSPBatchNum <> ''", dbConnection)
        dbConnection.Open()
        sqlRdr = SC.ExecuteReader
        While sqlRdr.Read
            'populate the dropdown box with batch numbers
            cmbBatchNo.Items.Add(sqlRdr.Item("NCSPBatchNum"))
        End While

        sqlRdr.Close()
        dbConnection.Close()
    End Sub

    Private Sub cmbBatchNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBatchNo.SelectedIndexChanged
        Dim SC As SqlClient.SqlCommand
        Dim DA As SqlClient.SqlDataAdapter
        Dim DS As New DataSet
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim tot As Double
        SC = New SqlClient.SqlCommand("select NCSPBatchNum, NCSPBatchDt, SUM(TranAmt) as TranAmt from [Transaction] where AcctCheckNum = '' and NCSPBatchNum = '" & cmbBatchNo.Text & "' group by NCSPBatchNum, NCSPBatchDt", dbConnection)
        dbConnection.Open()
        sqlRdr = SC.ExecuteReader
        While sqlRdr.Read
            txtBatchNo.Text = sqlRdr.Item("NCSPBatchNum")
            txtBatchDate.Text = sqlRdr.Item("NCSPBatchDt")
            tot = tot + CDbl(sqlRdr.Item("TranAmt"))
        End While
        txtBatchAmt.Text = FormatCurrency(tot, 2)
        sqlRdr.Close()

        SC = New SqlClient.SqlCommand("select A.SchedInst, B.InstLong, Sum(Amount) as Amount, A.AcctCheckNum, A.AcctCheckDt from (select A.NCSPBatchNum ,A4.InstID as SchedInst , Sum(TranAmt) as Amount,A.AcctCheckNum, A.AcctCheckDt , A.SchedID from [Transaction] A inner join (select z.AcctID, max(z.SchedID) as SchedID from [Transaction] z where z.NCSPBatchNum = '" & cmbBatchNo.Text & "' group by z.AcctID) A2 on A.AcctID = A2.AcctID inner join Schedule A3 on A2.AcctID = A3.AcctID and A2.SchedID = A3.SchedID and A3.RowStatus = 'A' inner join Inst A4 on A3.InstAtt = A4.InstLong where A.NCSPBatchNum = '" & cmbBatchNo.Text & "' and A.TranTyp <> 'Payment' and A.TranTyp <> 'Supplemental Payment' group by A.NCSPBatchNum, A.SchedInst,A.AcctCheckNum, A.AcctCheckDt  , A.SchedID,A4.InstID UNION select A.NCSPBatchNum ,A4.InstID as SchedInst , Sum(TranAmt) as Amount,A.AcctCheckNum, A.AcctCheckDt , A.SchedID from [Transaction] A inner join Schedule A3 on A.AcctID = A3.AcctID and A.SchedID = A3.SchedID and A3.RowStatus = 'A' inner join Inst A4 on A3.InstAtt = A4.InstLong where A.NCSPBatchNum = '" & cmbBatchNo.Text & "' and (A.TranTyp = 'Payment' or A.TranTyp = 'Supplemental Payment') group by A.NCSPBatchNum, A.SchedInst,A.AcctCheckNum, A.AcctCheckDt  , A.SchedID,A4.InstID ) A inner join Inst B on A.SchedInst = B.InstID where A.NCSPBatchNum = '" & cmbBatchNo.Text & "' group by A.SchedInst, B.InstLong, A.AcctCheckNum, A.AcctCheckDt", dbConnection)
        DA = New SqlClient.SqlDataAdapter(SC)
        DA.Fill(DS, "instDetail")
        Dim DV As New DataView(DS.Tables("instDetail"))
        DV.AllowNew = False
        dtgInstitutions.DataSource = DV
        dtgInstitutions.TableStyles.Clear()
        dtgInstitutions.TableStyles.Add(GetTableStyle)



        dbConnection.Close()
    End Sub

    Private Function GetTableStyle() As DataGridTableStyle
        Dim dgTableStyle As New DataGridTableStyle
        dgTableStyle.MappingName = "instDetail"
        dgTableStyle.RowHeadersVisible = False


        Dim column1 As New DataGridTextBoxColumnGray
        With column1
            .ReadOnly = True
            .MappingName = "InstLong"
            .HeaderText = "Institution"
            .TextBox.BackColor = Color.LightGray
            .Width = 210
        End With
        dgTableStyle.GridColumnStyles.Add(column1)

        Dim column2 As New DataGridTextBoxColumnGray
        With column2
            .ReadOnly = True
            .MappingName = "Amount"
            .HeaderText = "Amount"
            .TextBox.BackColor = Color.LightGray
            .Format = "0.00"
            .Width = 100
            .Alignment = HorizontalAlignment.Center
        End With
        dgTableStyle.GridColumnStyles.Add(column2)

        Dim column5 As New DataGridTextBoxColumn
        With column5
            .MappingName = "AcctCheckNum"
            .HeaderText = "Check Number"
            .Width = 100
            .Alignment = HorizontalAlignment.Center
        End With
        dgTableStyle.GridColumnStyles.Add(column5)

        Dim column6 As New DataGridTextBoxColumn
        With column6
            .MappingName = "AcctCheckDt"
            .HeaderText = "Check Date"
            .NullText = ""
            .Width = 100
            .Alignment = HorizontalAlignment.Center
        End With
        dgTableStyle.GridColumnStyles.Add(column6)

        'invisible institute ID
        Dim column7 As New DataGridTextBoxColumn
        With column7
            .MappingName = "SchedInst"
            .HeaderText = ""
            .Width = 0
        End With
        dgTableStyle.GridColumnStyles.Add(column7)

        Return dgTableStyle
    End Function

    Private Sub btnPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPost.Click
        Dim TranID As Date
        ''TranID = Now
        Dim Zip As String
        Dim NewAccountBalances As New List(Of AccountBalanceChange)
        Dim NewAccountStatuses As New List(Of AccountStatusChange)
        Dim R As DataRow
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim LetterType As String
        Dim SC As SqlClient.SqlCommand
        Dim DA As SqlClient.SqlDataAdapter
        Dim DS As New DataSet

        If cmbBatchNo.Text = "" Then Exit Sub
        If ValidateDatagrid() = False Then
            Exit Sub
        End If

        'Update transactions with check number
        Try
            SC = New SqlClient.SqlCommand("select A.TranID,A.SchedInst, B.InstLong,Amount from (select A.TranID,A.NCSPBatchNum ,A4.InstID as SchedInst , TranAmt as Amount, A.SchedID from [Transaction] A inner join (select z.AcctID, max(z.SchedID) as SchedID from [Transaction] z where z.NCSPBatchNum = '" & cmbBatchNo.Text & "' group by z.AcctID) A2 on A.AcctID = A2.AcctID inner join Schedule A3 on A2.AcctID = A3.AcctID and A2.SchedID = A3.SchedID and A3.RowStatus = 'A' inner join Inst A4 on A3.InstAtt = A4.InstLong where A.NCSPBatchNum = '" & cmbBatchNo.Text & "' and A.TranTyp <> 'Payment' and A.TranTyp <> 'Supplemental Payment' UNION select A.TranID,A.NCSPBatchNum ,A4.InstID as SchedInst ,TranAmt as Amount, A.SchedID from [Transaction] A inner join Schedule A3 on A.AcctID = A3.AcctID and A.SchedID = A3.SchedID and A3.RowStatus = 'A' inner join Inst A4 on A3.InstAtt = A4.InstLong where A.NCSPBatchNum = '" & cmbBatchNo.Text & "' and (A.TranTyp = 'Payment' or A.TranTyp = 'Supplemental Payment')) A inner join Inst B on A.SchedInst = B.InstID where A.NCSPBatchNum = '" & cmbBatchNo.Text & "'", dbConnection)
            dbConnection.Open()
            DA = New SqlClient.SqlDataAdapter(SC)
            DS.Clear()
            DA.Fill(DS)
            dbConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
        Dim x As Integer
        For x = 0 To dtgInstitutions.VisibleRowCount() - 1
            For Each R In DS.Tables(0).Select("SchedInst = '" & CStr(dtgInstitutions.Item(x, 4)) & "'")
                updateTransactionByAttribute(R.Item("TranID"), "AcctCheckNum", CStr(dtgInstitutions.Item(x, 2)))
                updateTransactionByAttribute(R.Item("TranID"), "AcctCheckDt", CStr(dtgInstitutions.Item(x, 3)))
            Next
        Next x

        'update account balance
        Try
            SC = New SqlClient.SqlCommand("SELECT A.AcctID, B.Balance - SUM(A.TranAmt) AS NewBalance FROM [Transaction] A INNER JOIN Account B on A.AcctID = B.AcctID AND B.RowStatus = 'A' WHERE A.NCSPBatchNum = '" & cmbBatchNo.Text & "' GROUP BY A.AcctID, B.Balance", dbConnection)
            dbConnection.Open()
            DA = New SqlClient.SqlDataAdapter(SC)
            DS.Clear()
            DA.Fill(DS)
            dbConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
        For Each R In DS.Tables(0).Rows
            NewAccountBalances.Add(New AccountBalanceChange(CStr(R.Item("AcctID")), CStr(R.Item("NewBalance"))))
        Next

        'get total payment amount
        Dim totalPaymentAmount As String = GetScholarshipAmount(Date.Now.Year.ToString())

        'Update Status from "Payment Pending to "Paid""
        Try
            SC = New SqlClient.SqlCommand("select distinct A.AcctID, B.SchedID from [Transaction] A inner join Schedule B on A.AcctID = B.AcctID and A.SchedID = B.SchedID and B.RowStatus = 'A' where NCSPBatchNum = '" & cmbBatchNo.Text & "' and B.SchedStat = 'Payment Pending'", dbConnection)

            dbConnection.Open()
            DA = New SqlClient.SqlDataAdapter(SC)
            DS.Clear()
            DA.Fill(DS)
            dbConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
        For Each R In DS.Tables(0).Rows
            'AccountStatusPaid.Add(CStr(R.Item("AcctID")))
            updateScheduleByAttribute(CStr(R.Item("AcctID")), CStr(R.Item("SchedID")), "SchedStat", "Paid")
        Next

        'LETTERS
        Try
            SC = New SqlClient.SqlCommand("select A.TranTyp,A.AcctID, SUM(A.TranAmt) as TranAmt, A.SchedSemEnr, A.SchedYrEnr, D.CredHrEnr, D.SchedHrRem, C.InstLong, B.CredHrRem, B.FName, B.LName, B.MI, B.Add1, B.Add2, B.City, B.ST, B.Zip from (select A.AcctID, A.TranTyp,A.SchedSemEnr,A.SchedYrEnr,A.NCSPBatchNum ,A4.InstID as SchedInst , A.TranAmt,A.AcctCheckNum, A.AcctCheckDt , A2.SchedID from [Transaction] A inner join (select z.AcctID, max(x.SchedID) as SchedID from [Transaction] z inner join Schedule x on z.AcctID = x.AcctID where z.NCSPBatchNum = '" & cmbBatchNo.Text & "' and z.TranTyp IN ('Payment','Supplemental Payment') and x.RowStatus = 'A' group by z.AcctID) A2 on A.AcctID = A2.AcctID inner join Schedule A3 on A2.AcctID = A3.AcctID and A2.SchedID = A3.SchedID and A3.RowStatus = 'A' inner join Inst A4 on A3.InstAtt = A4.InstLong where A.NCSPBatchNum = '" & cmbBatchNo.Text & "' and A.TranTyp not IN ('Payment','Supplemental Payment') UNION select A.AcctID, A.TranTyp,A.SchedSemEnr,A.SchedYrEnr,A.NCSPBatchNum ,Case when A.SchedInst = '' then A4.InstID when A.SchedInst <> '' then A.SchedInst end as SchedInst , A.TranAmt,A.AcctCheckNum, A.AcctCheckDt , A.SchedID from [Transaction] A inner join (select z.AcctID, max(z.SchedID) as SchedID from [Transaction] z where z.NCSPBatchNum = '" & cmbBatchNo.Text & "' group by z.AcctID) A2 on A.AcctID = A2.AcctID inner join Schedule A3 on A2.AcctID = A3.AcctID and A2.SchedID = A3.SchedID and A3.RowStatus = 'A' inner join Inst A4 on A3.InstAtt = A4.InstLong where A.NCSPBatchNum = '" & cmbBatchNo.Text & "' and A.TranTyp IN ('Payment','Supplemental Payment')) A inner join Account B on A.AcctID = B.AcctID and B.RowStatus = 'A' inner join Inst C on A.SchedInst = C.InstID inner join Schedule D on A.AcctID = D.AcctID and A.SchedID = D.SchedID and D.RowStatus = 'A' where NCSPBatchNum = '" & cmbBatchNo.Text & "' group by A.TranTyp, A.AcctID,A.SchedSemEnr,A.SchedYrEnr, B.CredHrRem, B.FName, B.LName, B.MI, B.Add1, B.Add2, B.City, B.ST, B.Zip, C.InstLong, D.CredHrEnr, D.SchedHrRem", dbConnection)
            dbConnection.Open()
            DA = New SqlClient.SqlDataAdapter(SC)
            DS.Clear()
            DA.Fill(DS)
            dbConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try

        'Adding Adjustments to Payments
        Dim R2 As DataRow
        For Each R In DS.Tables(0).Rows
            If R.Item("TranTyp") <> "Payment" And R.Item("TranTyp") <> "Supplemental Payment" Then
                Dim nextYear As Integer
                Dim nextSemester As Integer
                nextYear = 0
                nextSemester = 0
                'Find the next semester
                For Each R2 In DS.Tables(0).Select("InstLong = '" & R.Item("InstLong") & "' and AcctID = '" & R.Item("AcctID") & "' and TranTyp in ('Payment','Supplemental Payment')")
                    If nextYear = 0 Then
                        nextYear = R2.Item("SchedYrEnr")
                        nextSemester = SemesterInteger(R2.Item("SchedSemEnr"))
                    ElseIf (CInt(R2.Item("SchedYrEnr")) * 4) + SemesterInteger(R2.Item("SchedSemEnr")) < (CInt(nextYear) * 4) + nextSemester And ((CInt(R2.Item("SchedYrEnr")) * 4) + SemesterInteger(R2.Item("SchedSemEnr")) >= (CInt(R.Item("SchedYrEnr")) * 4) + SemesterInteger(R.Item("SchedSemEnr"))) Then
                        nextYear = R2.Item("SchedYrEnr")
                        nextSemester = SemesterInteger(R2.Item("SchedSemEnr"))
                    End If
                    'End If
                Next
                'Add the Transaction amount to the next transaction for school
                For Each R2 In DS.Tables(0).Select("InstLong = '" & R.Item("InstLong") & "' and AcctID = '" & R.Item("AcctID") & "' and TranTyp in ('Payment','Supplemental Payment')")
                    If (CInt(R2.Item("SchedYrEnr")) * 4) + SemesterInteger(R2.Item("SchedSemEnr")) = (CInt(nextYear) * 4) + nextSemester Then
                        R2.Item("TranAmt") = CDbl(R2.Item("TranAmt")) + CDbl(R.Item("TranAmt"))
                        R.Item("TranAmt") = 0
                        Exit For
                    End If
                Next
            End If
        Next

        For Each R In DS.Tables(0).Select("TranTyp IN ('Payment','Supplemental Payment')")
            TranID = Now
            If Len(CStr(R.Item("Zip"))) > 5 Then Zip = CLng(CStr(R.Item("Zip"))).ToString("00000-0000") Else Zip = CStr(CStr(R.Item("Zip")))
            If R.Item("CredHrRem") = 0 Or GetPaidSchedules(R.Item("AcctID")) >= 4 Then
                'NCSP PAYMENT NOTIFICATION AND CREDIT HOURS USED

                LetterType = "NCSP PAYMENT NOTIFICATION AND CREDIT HOURS USED"
                'AccountStatusClosed.Add(CStr(R.Item("AcctID")))
                NewAccountStatuses.Add(New AccountStatusChange(CStr(R.Item("AcctID")), "Eligibility Used"))

                'NCSCRHUSED
                FileOpen(1, "T:\NCSP_NCSCRHUSED.txt", OpenMode.Output)
                WriteLine(1, "AcctID", "FName", "LName", "Add1", "Add2", "City", "ST", "Zip", "Semester", "School", "CurrentHours", "PrevSemester", "HoursRem", "Total_Payment_Amount", "StaticCurrentDate")
                WriteLine(1, R.Item("AcctID"), R.Item("FName"), R.Item("LName"), R.Item("Add1"), R.Item("Add2"), R.Item("City"), R.Item("ST"), Zip, R.Item("SchedSemEnr"), R.Item("InstLong"), R.Item("CredHrEnr"), GetPreviousSemester(R.Item("AcctID")), R.Item("CredHrRem"), FormatCurrency(CDbl(totalPaymentAmount), 2), Date.Now.ToShortDateString())
                FileClose(1)

                PrintDoc("NCSCRHUSED", R.Item("AcctID") & "\" & "Communications", Format(DateValue(TranID), "MMddyyyy") & Format(TimeValue(TranID), "HHmmss"), False, "T:\NCSP_NCSCRHUSED.txt")
                
            ElseIf GetPaidSchedules(R.Item("AcctID")) >= 4 Then
                'NCSP MAXIMUM SEMESTERS PAID

                LetterType = "NCSP MAXIMUM SEMESTERS PAID"
                'AccountStatusClosed.Add(CStr(R.Item("AcctID")))
                NewAccountStatuses.Add(New AccountStatusChange(CStr(R.Item("AcctID")), "Maximum Semesters Paid"))

                'NCSACNTCLS
                FileOpen(1, "T:\NCSPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
                WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Reason", "BalanceOwed")
                WriteLine(1, R.Item("AcctID"), R.Item("FName"), R.Item("LName"), R.Item("Add1"), R.Item("Add2"), R.Item("City"), R.Item("ST"), Zip, GetLetterTextForIneligibilityReason("Eligibility Used"), "")
                FileClose(1)

                PrintDoc("NCSACNTCLS", R.Item("AcctID") & "\" & "Communications", Format(DateValue(TranID), "MMddyyyy") & Format(TimeValue(TranID), "HHmmss"), False, "T:\NCSACNTCLS.txt")
            ElseIf HrCompleteGPA_NULL(R.Item("AcctID")) Then
                'NCSP PAYMENT NOTIFICATION AND REQUEST FOR GRADES
                LetterType = "NCSP PAYMENT NOTIFICATION AND REQUEST FOR GRADES"

                'NCSREQGRAD
                FileOpen(1, "T:\NCSP_NCSREQGRAD.txt", OpenMode.Output)
                WriteLine(1, "AcctID", "FName", "LName", "Add1", "Add2", "City", "ST", "Zip", "Semester", "School", "CurrentHours", "PrevSemester", "HoursRem", "Total_Payment_Amount")
                WriteLine(1, R.Item("AcctID"), R.Item("FName"), R.Item("LName"), R.Item("Add1"), R.Item("Add2"), R.Item("City"), R.Item("ST"), Zip, R.Item("SchedSemEnr"), R.Item("InstLong"), R.Item("CredHrEnr"), GetPreviousSemester(R.Item("AcctID")), R.Item("CredHrRem"), FormatCurrency(CDbl(totalPaymentAmount), 2))
                FileClose(1)

                PrintDoc("NCSREQGRAD", R.Item("AcctID") & "\" & "Communications", Format(DateValue(TranID), "MMddyyyy") & Format(TimeValue(TranID), "HHmmss"), False, "T:\NCSP_NCSREQGRAD.txt")
                '................................................
            Else
                'NCSP Payment Notification and Credit Hour Balance (NCSCRHRBAL)
                LetterType = "NCSP PAYMENT NOTIFICATION AND CREDIT HOUR BALANCE"

                'NCSCRHRBAL
                FileOpen(1, "T:\NCSP_NCSCRHRBAL.txt", OpenMode.Output)
                WriteLine(1, "AcctID", "FName", "LName", "Add1", "Add2", "City", "ST", "Zip", "Semester", "School", "CurrentHours", "PrevSemester", "HoursRem", "Total_Payment_Amount", "StaticCurrentDate")
                WriteLine(1, R.Item("AcctID"), R.Item("FName"), R.Item("LName"), R.Item("Add1"), R.Item("Add2"), R.Item("City"), R.Item("ST"), Zip, R.Item("SchedSemEnr"), R.Item("InstLong"), R.Item("CredHrEnr"), GetPreviousSemester(R.Item("AcctID")), R.Item("CredHrRem"), FormatCurrency(CDbl(totalPaymentAmount), 2), Date.Now.ToShortDateString())
                FileClose(1)

                PrintDoc("NCSCRHRBAL", R.Item("AcctID") & "\" & "Communications", Format(DateValue(TranID), "MMddyyyy") & Format(TimeValue(TranID), "HHmmss"), False, "T:\NCSP_NCSCRHRBAL.txt")
                '........................................................
            End If

            AddCommunications(R.Item("AcctID"), TranID, False, True, "Account Maintenance", "Payment for " & FormatCurrency(R.Item("TranAmt"), 2) & " posted, " & LetterType & " letter sent.", "Payment and " & LetterType & " Letter Sent")
            UpdateAccount(CStr(R.Item("AcctID")), NewAccountBalances, NewAccountStatuses)
        Next
        'Update all batch transactions to Posted
        SC = New SqlClient.SqlCommand("update [Transaction] set TranStat = 'Posted' where NCSPBatchNum = '" & cmbBatchNo.Text & "'", dbConnection)
        Try
            dbConnection.Open()
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try

        'clear form
        txtBatchNo.Text = ""
        txtBatchDate.Text = ""
        txtBatchAmt.Text = ""
        cmbBatchNo.Items.Clear()
        cmbBatchNo.Text = ""
        dtgInstitutions.DataSource = Nothing
        SC = New SqlClient.SqlCommand("select distinct NCSPBatchNum from [Transaction] where AcctCheckNum = '' and NCSPBatchNum <> ''", dbConnection)
        dbConnection.Open()
        sqlRdr = SC.ExecuteReader
        While sqlRdr.Read
            'populate the dropdown box with batch numbers
            cmbBatchNo.Items.Add(sqlRdr.Item("NCSPBatchNum"))
        End While
        sqlRdr.Close()
        dbConnection.Close()
        MsgBox("The batch has been posted.", MsgBoxStyle.Information, "New Century Scholarship Program")
    End Sub

    Sub UpdateAccount(ByVal Acct As String, ByVal NewAccountBalances As List(Of AccountBalanceChange), ByVal NewAccountStatuses As List(Of AccountStatusChange))
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

            End If
            val = sqlRdr("AcctRecSeq")
            sqlRdr.Close()

            'Duplicate Active row!
            SC = New SqlClient.SqlCommand("insert Account (AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, LastUpdateDt, LastUpdateUser, RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied, LOAYearReturn) select AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, GETDATE(), '" & UserID & "', RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied, LOAYearReturn from Account Where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()

            'Set old record status to "H" for history
            SC = New SqlClient.SqlCommand("update Account set RowStatus = 'H' where AcctRecSeq = '" & val & "'", dbConnection)
            SC.ExecuteNonQuery()

            'build SQL update string
            Dim Updates As String = ""

            'add new balance update
            Dim newBalanceResult As String = (From nb In NewAccountBalances Where nb.AccountId = Acct Select nb.NewBalance).SingleOrDefault
            If newBalanceResult <> "" Then
                Updates = "Balance = " & newBalanceResult
            End If

            'update status
            Dim newStatusResult As String = (From nb In NewAccountStatuses Where nb.AccountId = Acct Select nb.EligEndRea).Distinct.SingleOrDefault
            If newStatusResult <> "" Then
                If Updates <> "" Then
                    Updates = Updates & ", Status = 'Closed', EligEndDt = '" & Now & "', EligEndRea = '" & newStatusResult & "'"
                Else
                    Updates = "Status = 'Closed', EligEndDt = '" & Now & "', EligEndRea = '" & newStatusResult & "'"
                End If
            End If

            If Updates <> String.Empty Then
                'Update new record
                SC = New SqlClient.SqlCommand("update Account set " & Updates & " where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
                SC.ExecuteNonQuery()
                dbConnection.Close()
            End If
        Catch ex As Exception
            MsgBox("The account update was not completed.  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
    End Sub

    'if the payment to the school includes an adjustment transaction, return true
    Function CheckForAdjustment(ByVal AcctID As String, ByVal BatchID As String) As Boolean
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String = String.Empty
        Try
            dbConnection.Open()
            SC = New SqlClient.SqlCommand("Select TranID from [Transaction] where AcctID = '" & AcctID & "' and NCSPBatchNum = '" & BatchID & "' and TranTyp = 'Adjustment'", dbConnection)
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If sqlRdr.HasRows Then
                sqlRdr.Close()
                dbConnection.Close()
                Return True
            Else
                sqlRdr.Close()
                dbConnection.Close()
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
    End Function

    Function AdjustmentPositive(ByVal AcctID As String, ByVal BatchID As String) As Boolean
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String = String.Empty
        Try
            dbConnection.Open()
            SC = New SqlClient.SqlCommand("Select TranAmt from [Transaction] where AcctID = '" & AcctID & "' and NCSPBatchNum = '" & BatchID & "' and TranTyp = 'Adjustment'", dbConnection)
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()

            If CDbl(sqlRdr("TranAmt")) >= 0 Then
                sqlRdr.Close()
                dbConnection.Close()
                Return True
            Else
                sqlRdr.Close()
                dbConnection.Close()
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
    End Function


    'if the hours complete or GPA is blank on the next to the last schedule, return true
    Function HrCompleteGPA_NULL(ByVal AcctID As String) As Boolean
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String
        Try
            SC = New SqlClient.SqlCommand("Select max(SchedID) as SchedID from Schedule where AcctID = '" & AcctID & "' and RowStatus = 'A'", dbConnection)
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            val = sqlRdr("SchedID")
            sqlRdr.Close()
            If CInt(val) > 1 Then
                SC = New SqlClient.SqlCommand("Select CredHrComp, SemesterGPA from Schedule where AcctID = '" & AcctID & "' and SchedID = " & val - 1 & " and RowStatus = 'A'", dbConnection)
                sqlRdr = SC.ExecuteReader
                sqlRdr.Read()
                'val = sqlRdr("SchedID")
                If sqlRdr.HasRows Then
                    If sqlRdr("CredHrComp").GetType.ToString = "System.DBNull" Or sqlRdr("SemesterGPA").GetType.ToString = "System.DBNull" Then
                        sqlRdr.Close()
                        dbConnection.Close()
                        Return True
                    Else
                        sqlRdr.Close()
                        dbConnection.Close()
                        Return False
                    End If
                Else
                    'there is no previous schedule.
                    sqlRdr.Close()
                    dbConnection.Close()
                    Return False
                End If
            Else
                dbConnection.Close()
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try

    End Function

    Function GetPreviousSemester(ByVal AcctID As String) As String
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String
        Dim Sem As String
        Try
            SC = New SqlClient.SqlCommand("Select max(SchedID) as SchedID from Schedule where AcctID = '" & AcctID & "' and RowStatus = 'A'", dbConnection)
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            val = sqlRdr("SchedID")
            sqlRdr.Close()
            If CInt(val) > 1 Then
                SC = New SqlClient.SqlCommand("Select Semester from Schedule where AcctID = '" & AcctID & "' and SchedID = " & val - 1 & " and RowStatus = 'A'", dbConnection)
                sqlRdr = SC.ExecuteReader
                sqlRdr.Read()
                If sqlRdr.HasRows Then
                    Sem = sqlRdr("Semester")
                    sqlRdr.Close()
                    dbConnection.Close()
                    Return Sem
                Else
                    'there is no previous schedule.
                    sqlRdr.Close()
                    dbConnection.Close()
                    Return ""
                End If
            Else
                dbConnection.Close()
                Return ""
            End If
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try

    End Function

    Sub updateTransactionAttributeByBatchSchool(ByVal SchedInst As String, ByVal NCSPBatchNum As String, ByVal attribute As String, ByVal value As String)
        'Updates the schedule table with a specific attribute and value for a specific account and schedule ID
        Dim SC As SqlClient.SqlCommand
        SC = New SqlClient.SqlCommand("update [Transaction] set " & attribute & " = '" & value & "' where NCSPBatchNum = '" & NCSPBatchNum & "' and SchedInst = '" & Replace(SchedInst, "'", "''") & "'", dbConnection)
        Try
            dbConnection.Open()
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The Transaction update was not completed.  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Exit Sub
        Finally
            dbConnection.Close()
        End Try
    End Sub

    Function ValidateDatagrid() As Boolean
        Dim x As Integer
        For x = 0 To dtgInstitutions.VisibleRowCount() - 1
            If Trim(CStr(dtgInstitutions.Item(x, 2))) = "" Then
                MsgBox("The Accounting Check Number can NOT be left blank.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Return False
            End If
            If dtgInstitutions.Item(x, 3).GetType.ToString = "System.DBNull" Then
                MsgBox("The Accounting Check Date is invalid.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Return False
            End If
            If IsDate(CStr(dtgInstitutions.Item(x, 3))) = False Then
                MsgBox("The Accounting Check Date is invalid.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Return False
            End If
        Next
        Return True
    End Function

End Class
