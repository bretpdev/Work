Public Class frmReports
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(Optional ByVal ReportSelected As String = "", Optional ByRef ParentFrm As frmNCSP = Nothing)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        If ReportSelected <> "" Then UpdateReportAndForm(ReportSelected, ParentFrm)

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
    Friend WithEvents CRViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmReports))
        Me.CRViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.SuspendLayout()
        '
        'CRViewer
        '
        Me.CRViewer.ActiveViewIndex = -1
        Me.CRViewer.DisplayGroupTree = False
        Me.CRViewer.Location = New System.Drawing.Point(8, 8)
        Me.CRViewer.Name = "CRViewer"
        Me.CRViewer.ReportSource = Nothing
        Me.CRViewer.ShowCloseButton = False
        Me.CRViewer.ShowGroupTreeButton = False
        Me.CRViewer.Size = New System.Drawing.Size(984, 664)
        Me.CRViewer.TabIndex = 0
        '
        'frmReports
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1000, 676)
        Me.Controls.Add(Me.CRViewer)
        Me.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1008, 704)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1008, 704)
        Me.Name = "frmReports"
        Me.Text = "Reports"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub UpdateReportAndForm(ByVal ReportSelected As String, Optional ByRef ParentFrm As frmNCSP = Nothing)
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim DS As New DataSet
        Dim DA As SqlClient.SqlDataAdapter
        Dim DateRangeFrm As frmReportsDateRange
        Dim RecipientSpecificationFrm As frmReportsRecipientSpecification
        Dim BatchSpecificationFrm As frmReportBatchSpecification
        'create reports as needed
        If ReportSelected = "Current List Of Recipients" Then
            DA = New SqlClient.SqlDataAdapter("SELECT A.FName + ' ' + A.LName as FullName, A.AcctID, A.SSN, A.Degree, A.DegreeGPA, A.HSGradDt, A.AppAprDt, (A.CredHrRem - 60) as HoursUsed FROM Account A WHERE A.RowStatus = 'A' AND A.AppAprDt IS NOT NULL AND EligEndDt IS NULL", General.dbConnection.ConnectionString)
            Rpt = New AllOpenAccounts
            Rpt.SummaryInfo.ReportTitle = "Current Recipients"
        ElseIf ReportSelected = "Count Of New Recipients (By Date Range)" Then
            DateRangeFrm = New frmReportsDateRange
            DateRangeFrm.ShowDialog()
            If DateRangeFrm.FormWasCancelled Then
                Exit Sub
            End If
            DA = New SqlClient.SqlDataAdapter("SELECT COUNT(*) AS TheCount FROM Account WHERE AppAprDt >= '" + String.Format("{0:MM/dd/yyyy}", DateRangeFrm.dtpStartDate.Value) + " 00:00:00.000' AND AppAprDt <= '" + String.Format("{0:MM/dd/yyyy}", DateRangeFrm.dtpEndDate.Value) + " 23:59:59.999' AND RowStatus = 'A'", General.dbConnection.ConnectionString)
            Rpt = New OpenAccountsByDateRange
            Rpt.SummaryInfo.ReportTitle = "Count Of New Recipients From " + String.Format("{0:MM/dd/yyyy}", DateRangeFrm.dtpStartDate.Value) + " To " + String.Format("{0:MM/dd/yyyy}", DateRangeFrm.dtpEndDate.Value)
        ElseIf ReportSelected = "Account Recap" Then
            RecipientSpecificationFrm = New frmReportsRecipientSpecification
            RecipientSpecificationFrm.ShowDialog()
            If RecipientSpecificationFrm.FormWasCancelled Then
                Exit Sub
            End If
            DA = New SqlClient.SqlDataAdapter("SELECT A.FName + ' ' + A.LName AS TheName, A.AcctID, B.SchedSemEnr, B.SchedYrEnr, C.InstLong AS SchedInst, B.TranHrPd, B.TranTyp, B.TranDt, B.NCSPBatchNum,B.NCSPBatchDt, B.TranAmt FROM Account A JOIN [Transaction] B ON A.AcctID = B.AcctID JOIN Inst C ON B.SchedInst = C.InstID WHERE A.AcctID = '" + RecipientSpecificationFrm.tbAcctID.Text + "' AND A.RowStatus = 'A'", General.dbConnection.ConnectionString)
            Rpt = New AccountRecap
            Rpt.SummaryInfo.ReportTitle = "Account Recap"
        ElseIf ReportSelected = "Batch Recap" Then
            BatchSpecificationFrm = New frmReportBatchSpecification
            BatchSpecificationFrm.ShowDialog()
            If BatchSpecificationFrm.FormWasCancelled Then
                Exit Sub
            End If
            DA = New SqlClient.SqlDataAdapter("SELECT A.NCSPBatchNum, A.NCSPBatchDt, B.InstLong,	A.AcctID, A.TranHrPd, A.TranAmt FROM [Transaction] A JOIN Inst B ON A.SchedInst = B.InstID WHERE A.NCSPBatchNum = " + BatchSpecificationFrm.tbBatchNumber.Text, General.dbConnection.ConnectionString)
            Rpt = New BatchRecap
            Rpt.SummaryInfo.ReportTitle = "Batch Recap"
        ElseIf ReportSelected = "Refund(s) Due Student(s)" Then
            DA = New SqlClient.SqlDataAdapter("SELECT A.AcctID, B.TotalTramAmt FROM Account A JOIN ( SELECT Z.AcctID, SUM(Z.TranAmt) AS TotalTramAmt FROM [Transaction] Z WHERE Z.NCSPBatchNum = '' GROUP BY Z.AcctID ) B ON A.AcctID = B.AcctID WHERE B.TotalTramAmt > 5 AND A.RowStatus = 'A' AND A.AcctID NOT IN ( SELECT Y.AcctID FROM [Transaction] Y WHERE Y.TranTyp = 'Supplemental Payment' and Y.TranStat = 'Entered' ) AND A.AcctID NOT IN ( SELECT X.AcctID FROM Schedule X WHERE X.SchedStat = 'Payment Pending' )", General.dbConnection.ConnectionString)
            Rpt = New RefundDueStudents
            Rpt.SummaryInfo.ReportTitle = "Refund(s) Due Student(s)"
        ElseIf ReportSelected = "Associate Degree Recipients" Then
            DA = New SqlClient.SqlDataAdapter("SELECT A.FName + ' ' + A.LName as FullName, A.AcctID, A.SSN, A.Degree, A.DegreeInst, A.DegreeComplDt, A.DegreeGPA, A.HSAttended, A.HSGradDt, A.HSGPA, A.AppAprDt, (A.CredHrRem - 60) as HoursUsed FROM Account A WHERE A.RowStatus = 'A' AND A.AppAprDt IS NOT NULL AND EligEndDt IS NULL AND A.Degree <> ''", General.dbConnection.ConnectionString)
            Rpt = New AssociateDegreeRecipients
            Rpt.SummaryInfo.ReportTitle = "Associate Degree Recipients"
        ElseIf ReportSelected = "Disbursement Totals (By Date Range)" Then
            DateRangeFrm = New frmReportsDateRange
            DateRangeFrm.ShowDialog()
            If DateRangeFrm.FormWasCancelled Then
                Exit Sub
            End If
            DA = New SqlClient.SqlDataAdapter("SELECT A.TranTyp, A.TranID, A.NCSPBatchNum, A.NCSPBatchDt, A.TranAmt, A.AcctID FROM [Transaction] A WHERE A.AcctCheckDt >= '" + String.Format("{0:MM/dd/yyyy}", DateRangeFrm.dtpStartDate.Value) + " 00:00:00.000' AND A.AcctCheckDt <= '" + String.Format("{0:MM/dd/yyyy}", DateRangeFrm.dtpEndDate.Value) + " 23:59:59.999'", General.dbConnection.ConnectionString)
            Rpt = New DisbursementTotals
            Rpt.SummaryInfo.ReportTitle = "Disbursement Totals For " + String.Format("{0:MM/dd/yyyy}", DateRangeFrm.dtpStartDate.Value) + " To " + String.Format("{0:MM/dd/yyyy}", DateRangeFrm.dtpEndDate.Value)
        End If
        DA.Fill(DS, "Data")
        Rpt.SetDataSource(DS.Tables("Data"))
        CRViewer.ReportSource = Rpt
        CRViewer.Zoom(1)
        'set as mdiparent if it is not nothing
        If (ParentFrm Is Nothing) = False Then Me.MdiParent = ParentFrm
        Me.Show()
    End Sub

    Public Overloads Sub Show(ByVal ReportName As String)
        If ReportName = "Batch Recap" Then
            Dim rpt As New rptBatchRecap
            Dim DS As New DataSet
            GetdbConnection()

            'Dim SC As New SqlClient.SqlCommand("select A.AcctID, C.FName + ' ' + C.LName as FName, C.SSN, A.TranID, B.InstID as SchedInst, E.Semester, E.SchedYr, A.TranTyp, A.TranAmt, A.TranStat, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact, A.TranHrPd From [Transaction] A inner join (select A1.AcctID, B1.InstID, Sum(A1.TranAmt) as SumTranAmt , A1.SchedID from [Transaction] A1 inner join (select distinct Z.AcctID, Y2.InstID, Z.SchedID from [Transaction] Z inner join Schedule Y on Z.AcctID = Y.AcctID and Y.SchedStat = 'Payment Pending' and Y.RowStatus = 'A' and Z.SchedID = Y.SchedID inner join Inst Y2 on Y.InstAtt = Y2.InstLong where Z.NCSPBatchNum = '' UNION select distinct Y.AcctID, Y.SchedInst as InstID, Y.SchedID from [Transaction] Y where (Y.TranTyp = 'Supplemental Payment' or Y.TranTyp = 'Student Repayment') and Y.TranStat = 'Entered' and Y.NCSPBatchNum = '' ) B1 on A1.AcctID = B1.AcctID and A1.SchedID = B1.SchedID group by A1.AcctID, B1.InstID, A1.SchedID having Sum(A1.TranAmt) > 0 ) B on A.AcctID = B.AcctID and A.SchedID = B.SchedID inner join Account C on A.AcctID = C.AcctID and C.RowStatus = 'A' inner join Inst D on B.InstID = D.InstID left outer join Schedule E on A.AcctID = E.AcctID and A.SchedID = E.SchedID and A.SchedSemEnr = E.Semester and A.SchedYrEnr = E.SchedYr and E.RowStatus = 'A' Where A.NCSPBatchNum = '' order by C.LName, C.FName", dbConnection)
            Dim SC As New SqlClient.SqlCommand("select A.AcctID, C.FName + ' ' + C.LName as FName, C.SSN, A.TranID, B.InstID as SchedInst, E.Semester, E.SchedYr, A.TranTyp, A.TranAmt, A.TranStat, D.InstAdd1, D.InstAdd2, D.InstCity, D.InstST, D.InstZip, D.InstContact, A.TranHrPd From [Transaction] A inner join (select A1.AcctID, B1.InstID, Sum(A1.TranAmt) as SumTranAmt , A1.SchedID from [Transaction] A1 inner join (select R1.AcctID, Y2.InstID, R1.SchedID from [Transaction] R1 inner join (select distinct Z.AcctID, Y2.InstID, Z.SchedID from [Transaction] Z inner join Schedule Y on Z.AcctID = Y.AcctID and Y.SchedStat = 'Payment Pending' and Y.RowStatus = 'A' and Z.SchedID = Y.SchedID inner join Inst Y2 on Y.InstAtt = Y2.InstLong where Z.NCSPBatchNum = '' ) R6 on R6.AcctID = R1.AcctID inner join Schedule Y on R1.AcctID = Y.AcctID and Y.RowStatus = 'A' and R1.SchedID = Y.SchedID inner join Inst Y2 on Y.InstAtt = Y2.InstLong where R1.NCSPBatchNum = '' UNION select R1.AcctID, Y2.InstID, R1.SchedID	from [Transaction] R1 inner join (select distinct Y.AcctID, Y.SchedInst as InstID, Y.SchedID from [Transaction] Y where (Y.TranTyp = 'Supplemental Payment' or Y.TranTyp = 'Student Repayment') and Y.TranStat = 'Entered' and Y.NCSPBatchNum = '' ) R6 on R6.AcctID = R1.AcctID inner join Schedule Y on R1.AcctID = Y.AcctID and Y.RowStatus = 'A' and R1.SchedID = Y.SchedID inner join Inst Y2 on Y.InstAtt = Y2.InstLong where R1.NCSPBatchNum = '' ) B1 on A1.AcctID = B1.AcctID and A1.SchedID = B1.SchedID group by A1.AcctID, B1.InstID, A1.SchedID having Sum(A1.TranAmt) > 0 ) B on A.AcctID = B.AcctID and A.SchedID = B.SchedID inner join Account C on A.AcctID = C.AcctID and C.Balance > 0 and C.RowStatus = 'A' inner join Inst D on B.InstID = D.InstID left outer join Schedule E on A.AcctID = E.AcctID and A.SchedID = E.SchedID and A.SchedSemEnr = E.Semester and A.SchedYrEnr = E.SchedYr and E.RowStatus = 'A' Where A.NCSPBatchNum = '' order by C.LName, C.FName", dbConnection)
            Dim DA As New SqlClient.SqlDataAdapter(SC)
            dbConnection.Open()
            DA.Fill(DS)
            dbConnection.Close()
            'remove zero balance
            RemoveZeroBalance(ds)
            RemoveAdjustmentOnlys(ds)
            rpt.SetDataSource(DS.Tables(0))
            CRViewer.ReportSource = rpt
            CRViewer.DisplayGroupTree = False
        Else
            Dim rpt As New rptBatchDetail
            Dim DS As New DataSet
            GetdbConnection()
            Dim SC As New SqlClient.SqlCommand("select A.AcctID, C.FName + ' ' + C.LName as FName, C.SSN, A.SchedInst, A.tranInst, A.SchedSemEnr, A.SchedYrEnr, A.TranHrPd, A.TranTyp, A.TranAmt From [Transaction] A inner join Account C on A.AcctID = C.AcctID and C.RowStatus = 'A' Where A.NCSPBatchNum = '" & ReportName & "'", dbConnection)
            Dim DA As New SqlClient.SqlDataAdapter(SC)
            dbConnection.Open()
            DA.Fill(DS)
            dbConnection.Close()
            rpt.SetDataSource(DS.Tables(0))
            CRViewer.ReportSource = rpt
            CRViewer.DisplayGroupTree = False
        End If
        CRViewer.Zoom(1)
        Me.Show()

    End Sub

    Function RemoveAdjustmentOnlys(ByRef DS As DataSet) As DataSet
        Dim tempr As DataRow
        Dim hasPayment As Boolean
        'get list of unique accounts
        Dim AccountList As New ArrayList
        For Each tempr In DS.Tables(0).Rows
            If AccountList.IndexOf(tempr.Item("AcctID")) < 0 Then
                AccountList.Add(tempr.Item("AcctID"))
            End If
        Next
        Dim tempx As Integer
        'Loop for each account
        For tempx = 0 To AccountList.Count - 1
            Dim Total As Double
            Total = 0
            hasPayment = False
            'Loop for each row
            For Each tempr In DS.Tables(0).Rows
                If AccountList(tempx) = tempr.Item("AcctID") Then
                    If tempr.Item("TranTyp") = "Payment" Or tempr.Item("TranTyp") = "Supplemental Payment" Then
                        hasPayment = True
                    End If
                End If
            Next
            If hasPayment = False Then
                'if student does not have a payment then remove all records for account
                Dim Removed As Boolean
                Removed = True
                Do While Removed = True
                    Removed = False
                    For Each tempr In DS.Tables(0).Rows
                        If AccountList(tempx) = tempr.Item("AcctID") Then
                            DS.Tables(0).Rows.Remove(tempr)
                            Removed = True
                            Exit For
                        End If
                    Next
                Loop
            End If
        Next
    End Function

    Function RemoveZeroBalance(ByRef DS As DataSet) As DataSet
        Dim tempr As DataRow
        'get list of unique accounts
        Dim AccountList As New ArrayList
        For Each tempr In DS.Tables(0).Rows
            If AccountList.IndexOf(tempr.Item("AcctID")) < 0 Then
                AccountList.Add(tempr.Item("AcctID"))
            End If
        Next
        Dim tempx As Integer
        'Loop for each account
        For tempx = 0 To AccountList.Count - 1
            Dim Total As Double
            Total = 0
            'Loop for each row
            For Each tempr In DS.Tables(0).Rows
                If AccountList(tempx) = tempr.Item("AcctID") Then
                    Total = Total + CDbl(tempr.Item("TranAmt"))
                End If
            Next
            If Total <= 0 Then
                'if total = 0 then remove all records for account
                Dim Removed As Boolean
                Removed = True
                Do While Removed = True
                    Removed = False
                    For Each tempr In DS.Tables(0).Rows
                        If AccountList(tempx) = tempr.Item("AcctID") Then
                            DS.Tables(0).Rows.Remove(tempr)
                            Removed = True
                            Exit For
                        End If
                    Next
                Loop
            End If
        Next
    End Function

End Class
