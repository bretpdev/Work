Imports System.IO
Imports System.Data.SqlClient

Public Class frmTILPMain
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

        'check if borrower object exists
        If (selBor Is Nothing) = False Then
            'unlock account is it exists
            selBor.UnlockAccount()
        End If

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
    Friend WithEvents PNavigation As System.Windows.Forms.Panel
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents miDemographics As System.Windows.Forms.MenuItem
    Friend WithEvents miActivityComments As System.Windows.Forms.MenuItem
    Friend WithEvents miReferences As System.Windows.Forms.MenuItem
    Friend WithEvents miLoanInfo As System.Windows.Forms.MenuItem
    Friend WithEvents miNewSearch As System.Windows.Forms.MenuItem
    Friend WithEvents MIRpts As System.Windows.Forms.MenuItem
    Friend WithEvents MIPDR As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MIDisb As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTILPMain))
        Me.PNavigation = New System.Windows.Forms.Panel
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.miNewSearch = New System.Windows.Forms.MenuItem
        Me.miActivityComments = New System.Windows.Forms.MenuItem
        Me.miDemographics = New System.Windows.Forms.MenuItem
        Me.miLoanInfo = New System.Windows.Forms.MenuItem
        Me.miReferences = New System.Windows.Forms.MenuItem
        Me.MIRpts = New System.Windows.Forms.MenuItem
        Me.MIPDR = New System.Windows.Forms.MenuItem
        Me.MIDisb = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'PNavigation
        '
        Me.PNavigation.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PNavigation.Location = New System.Drawing.Point(0, 484)
        Me.PNavigation.Name = "PNavigation"
        Me.PNavigation.Size = New System.Drawing.Size(920, 8)
        Me.PNavigation.TabIndex = 1
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miNewSearch, Me.miActivityComments, Me.miDemographics, Me.miLoanInfo, Me.miReferences, Me.MIRpts})
        '
        'miNewSearch
        '
        Me.miNewSearch.Index = 0
        Me.miNewSearch.Text = "Account &Selection"
        '
        'miActivityComments
        '
        Me.miActivityComments.Index = 1
        Me.miActivityComments.Text = "&Activity Comments"
        '
        'miDemographics
        '
        Me.miDemographics.Index = 2
        Me.miDemographics.Text = "&Demographics"
        '
        'miLoanInfo
        '
        Me.miLoanInfo.Index = 3
        Me.miLoanInfo.Text = "&Loan Information"
        '
        'miReferences
        '
        Me.miReferences.Index = 4
        Me.miReferences.Text = "&References"
        '
        'MIRpts
        '
        Me.MIRpts.Index = 5
        Me.MIRpts.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MIPDR, Me.MIDisb, Me.MenuItem1})
        Me.MIRpts.Text = "Reports"
        '
        'MIPDR
        '
        Me.MIPDR.Index = 0
        Me.MIPDR.Text = "Pending Disbursement Reconciliation"
        '
        'MIDisb
        '
        Me.MIDisb.Index = 1
        Me.MIDisb.Text = "Disbursement (Full Roster and Individual School Rosters)"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 2
        Me.MenuItem1.Text = "Funds Transfer Form"
        '
        'frmTILPMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(920, 492)
        Me.Controls.Add(Me.PNavigation)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Menu = Me.MainMenu1
        Me.Name = "frmTILPMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TILP System"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Enum OpSelected
        AcctSel = 1
        NewBorr = 2
        GeneralNav = 3
    End Enum

    Const TestMode As Boolean = False
    Public AcctSelection As frmSelection
    Public selBor As borrower
    Public BorDem As frmBorDemo
    Public BorRefs As frmBorRefs
    Public LoanInfo As frmLoanInfo
    Public ActivityHistAndComments As frmActivityComments
    Public TheUser As user
    Private DA As SqlClient.SqlDataAdapter
    Private DS As New DataSet
    Private Conn As SqlClient.SqlConnection
    Private bsysConn As SqlClient.SqlConnection


    Private Sub frmTILPMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim LogIn As New frmLogIn(TestMode)
        'do login stuff
        LogIn.ShowDialog()
        'check if form was cancelled, if it was then end app
        If LogIn.FormWasCancelled() Then End
        TheUser = New user(LogIn.GetUID(), LogIn.GetAccessLvl(), True)
        'disable reports if the user doesn't have appropriate access
        If TheUser.GetAccessLevel() <> 1 And TheUser.GetAccessLevel() <> 4 And TheUser.GetAccessLevel() <> 5 Then
            MIRpts.Enabled = False
        ElseIf TheUser.GetAccessLevel() = 5 Then
            MIDisb.Enabled = False
        End If
        AcctSelection = New frmSelection(TestMode, Me, TheUser)
        AcctSelection.MdiParent = Me
        AcctSelection.Show()
        MenuOptionCoor(OpSelected.AcctSel)
        'setup data adapters for reports below
        If TestMode Then
            DA = New SqlClient.SqlDataAdapter("", "Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
            bsysConn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=BSYS;Integrated Security=SSPI;")
        Else
            DA = New SqlClient.SqlDataAdapter("", "Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
            bsysConn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=BSYS;Integrated Security=SSPI;")
        End If

    End Sub

    Private Sub miLoanInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miLoanInfo.Click
        Dim I As Integer
        'cycle through and be sure that all forms are closed
        While I < Me.MdiChildren.GetLength(0)
            If Me.MdiChildren(I).Visible Then
                If Me.MdiChildren(I).GetType.ToString = "TILP_System.frmLoanInfo" Then
                    Me.MdiChildren(I).Focus() 'set focus if loan info form is already open
                    Exit Sub
                Else
                    Me.MdiChildren(I).Close()
                End If
            End If
        End While
        LoanInfo = New frmLoanInfo(selBor, TheUser)
        LoanInfo.MdiParent = Me
        LoanInfo.Show()
        MenuOptionCoor(OpSelected.GeneralNav)
    End Sub


    Private Sub miDemographics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miDemographics.Click
        Dim I As Integer
        'cycle through and be sure that all forms are closed
        While I < Me.MdiChildren.GetLength(0)
            If Me.MdiChildren(I).Visible Then
                If Me.MdiChildren(I).GetType.ToString = "TILP_System.frmBorDemo" Then
                    Me.MdiChildren(I).Focus() 'set focus if loan info form is already open
                    Exit Sub
                Else
                    Me.MdiChildren(I).Close()
                End If
            End If
        End While
        BorDem = New frmBorDemo(selBor, TestMode, TheUser, Me)
        BorDem.MdiParent = Me
        BorDem.Show()
        MenuOptionCoor(OpSelected.GeneralNav)
    End Sub

    Private Sub miActivityComments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miActivityComments.Click
        Dim I As Integer
        'cycle through and be sure that all forms are closed
        While I < Me.MdiChildren.GetLength(0)
            If Me.MdiChildren(I).Visible Then
                If Me.MdiChildren(I).GetType.ToString = "TILP_System.frmActivityComments" Then
                    Me.MdiChildren(I).Focus() 'set focus if loan info form is already open
                    Exit Sub
                Else
                    Me.MdiChildren(I).Close()
                End If
            End If
        End While
        ActivityHistAndComments = New frmActivityComments(selBor.getKey, selBor.TestMode, selBor.FN + " " + selBor.LN, TheUser)
        ActivityHistAndComments.MdiParent = Me
        ActivityHistAndComments.Show()
        MenuOptionCoor(OpSelected.GeneralNav)
    End Sub

    Private Sub miReferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miReferences.Click
        Dim I As Integer
        'cycle through and be sure that all forms are closed
        While I < Me.MdiChildren.GetLength(0)
            If Me.MdiChildren(I).Visible Then
                If Me.MdiChildren(I).GetType.ToString = "TILP_System.frmBorRefs" Then
                    Me.MdiChildren(I).Focus() 'set focus if loan info form is already open
                    Exit Sub
                Else
                    Me.MdiChildren(I).Close()
                End If
            End If
        End While
        BorRefs = New frmBorRefs(selBor, TestMode, TheUser, Me)
        BorRefs.MdiParent = Me
        BorRefs.Show()
        MenuOptionCoor(OpSelected.GeneralNav)
    End Sub

    Private Sub miNewSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miNewSearch.Click
        Dim I As Integer
        'check if borrower object exists
        If (selBor Is Nothing) = False Then
            'unlock account is it exists
            selBor.UnlockAccount()
        End If
        selBor = Nothing
        'cycle through and be sure that all forms are closed
        While I < Me.MdiChildren.GetLength(0)
            If Me.MdiChildren(I).Visible Then
                If Me.MdiChildren(I).GetType.ToString = "TILP_System.frmSelection" Then
                    Me.MdiChildren(I).Focus() 'set focus if loan info form is already open
                    Exit Sub
                Else
                    Me.MdiChildren(I).Close()
                End If
            End If
        End While
        AcctSelection = New frmSelection(TestMode, Me, TheUser)
        AcctSelection.MdiParent = Me
        AcctSelection.Show()
        MenuOptionCoor(OpSelected.AcctSel)
    End Sub

    Private Sub MIPDR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MIPDR.Click
        PrintGeneralDisbRpt("Terrel H. Bell Pending Disbursement Reconciliation Report", True)
        MsgBox("Please retrieve the report from the printer.", MsgBoxStyle.Information, "Report Printed")
    End Sub

    'print general disbursement report
    Private Sub PrintGeneralDisbRpt(ByVal Title As String, ByVal SuppressCheckSummary As Boolean)
        Dim Rpt As New crGeneralDisb
        DS.Clear()
        DA.SelectCommand.CommandText = "EXEC spDisbRpt '" + TheUser.GetUserName() + "'"
        DA.Fill(DS, "General")
        Rpt.SetDataSource(DS.Tables("General"))
        Rpt.SummaryInfo.ReportTitle = Title
        Rpt.OpenSubreport("rptCheckInformation").SetDataSource(DS.Tables("General"))
        'suppress subreport?
        Rpt.Section9.ReportObjects("Subreport1").ObjectFormat.EnableSuppress = SuppressCheckSummary
        Rpt.PrintToPrinter(1, True, 0, 0)
    End Sub

    'print school specific reports
    Private Sub PrintSchoolSpecificRpt()
        Dim Reader As SqlClient.SqlDataReader
        Dim Comm As New SqlClient.SqlCommand("EXEC spExtSchLst", Conn)
        Dim SchRpt As crIndividualSchoolRpt
        Conn.Open()
        Reader = Comm.ExecuteReader
        While Reader.Read
            'DS.Tables("General") should still be populated from previous "PrintGeneralDisbRpt" call
            'only create report if the school has data in the disbursement data
            If DS.Tables("General").Select("SchoolCode = '" + Reader("SchoolCode") + "'").Length > 0 Then
                SchRpt = New crIndividualSchoolRpt
                DA.SelectCommand.CommandText = "EXEC spDisbRpt '" + TheUser.GetUID() + "', '" + Reader("SchoolCode") + "'"
                DA.Fill(DS, Reader("SchoolCode"))
                'print disb letter
                FileOpen(1, "T:\TILPSchDisbDat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
                WriteLine(1, "SchName", "Addr1", "Addr2", "City", "State", "Zip", "SchCd")
                WriteLine(1, Reader("SchoolName"), Reader("Addr1"), Reader("Addr2"), Reader("City"), Reader("State"), Reader("Zip"), Reader("SchoolCode"))
                FileClose(1)
                borrower.PrintDoc("TILPDISB", "X:\PADD\TILP\", TestMode, "T:\TILPSchDisbDat.txt")
                'create school specific report
                SchRpt.SetDataSource(DS.Tables(Reader("SchoolCode")))
                SchRpt.SummaryInfo.ReportTitle = "Terrel H. Bell Disbursement Roster for " + Reader("SchoolName")
                SchRpt.SummaryInfo.ReportComments = Reader("Addr1") + " " + Reader("Addr2") + vbCrLf + Reader("City") + ", " + Reader("State") + " " + Reader("Zip")
                SchRpt.PrintToPrinter(1, True, 0, 0)
            End If
        End While
        Conn.Close()
    End Sub

    Private Sub UpdateLoansToDisbStatus()
        Dim Comm As New SqlClient.SqlCommand("EXEC spUpdateLoans2BDisb", Conn)
        Conn.Open()
        Comm.ExecuteNonQuery()
        Conn.Close()
    End Sub

    Private Sub MIDisb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MIDisb.Click
        'make sure the sure knows what this option does
        If MsgBox("By running this report you not only produce the report but also update all loans listed on the report to a disbursed status with a disbursed date of today.  Are you sure you want to do this?", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, "Disbursement Warning") = MsgBoxResult.Yes Then
            PrintGeneralDisbRpt("Terrel H. Bell Disbursement Roster", False)
            PrintSchoolSpecificRpt()
            UpdateLoansToDisbStatus()
            MsgBox("All loans listed on the reports have been updated to a disbursed status.  Please retrieve all reports from the printer.", MsgBoxStyle.Information, "Loans Disbursed And Reports Printed")
        End If
    End Sub

    Friend Sub MenuOptionCoor(ByVal Op As OpSelected)
        If Op = OpSelected.AcctSel Then
            miNewSearch.Enabled = False
            miActivityComments.Enabled = False
            miDemographics.Enabled = False
            miLoanInfo.Enabled = False
            miReferences.Enabled = False
        ElseIf Op = OpSelected.NewBorr Then
            miNewSearch.Enabled = True
            miActivityComments.Enabled = False
            miDemographics.Enabled = False
            miLoanInfo.Enabled = False
            miReferences.Enabled = False
        ElseIf Op = OpSelected.GeneralNav Then
            miNewSearch.Enabled = True
            miActivityComments.Enabled = True
            miDemographics.Enabled = True
            miLoanInfo.Enabled = True
            miReferences.Enabled = True
        End If
    End Sub

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Dim frmLog As New frmLogIn(TestMode)
        Dim deadlineDate As String = Now.ToShortDateString
        Dim deadlineTime As String = "4:30 PM"
        Dim program As String = "Teacher Incentive Loan Program (TILP)"
        Dim from As String = "PTIF 4825 - Teacher Incentive Loan Program (TILP)"
        Dim strTo As String = "ZFNB 002247260 - Teacher Incentive Loan Program (TILP)"
        Dim purpose As String = "TILP Disbursement"
        Dim desc As String = "TILP Disbursement for " & Now.ToShortDateString
        Dim amount As Decimal = 0
        Dim amt As String = ""
        Dim reqBy As String = Environment.UserName
        Dim dateTime As String = Now.ToShortDateString & ", " & Now.ToShortTimeString
        Dim strWriter As StreamWriter
        Dim fs As New FileStream("T:\FundsTransForm.txt", FileMode.Create, FileAccess.ReadWrite)
        Dim dt As String = Now.ToString("MM/dd/yyyy")
        Dim cmd As SqlCommand
        Dim rdr As SqlDataReader
        Dim tempStr As String
        'Get the username from BYSY table SYSA_LST_Users
        Try
            cmd = New SqlCommand("SELECT Firstname, Lastname from SYSA_LST_Users WHERE WindowsUserName = '" & reqBy & "'", bsysConn)
            bsysConn.Open()
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                reqBy = rdr(0) & " " & rdr(1)
            End While
            rdr.Close()
            bsysConn.Close()
        Catch ex As Exception
            MessageBox.Show(reqBy.ToString() & " does not exist in the database")
        End Try
        'Set the amount
        Try
            cmd = New SqlCommand("SELECT SUM(DisbAmount) FROM LoanDat WHERE DisbDate = '" & dt & "'", Conn)
            Conn.Open()
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                amount = rdr(0)
            End While
            rdr.Close()
            Conn.Close()
        Catch ex As Exception
            If MessageBox.Show("There are no loan amounts showing for today. Do you want to continue printing the Funds Transfer Letter", "No Amount", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then End
        End Try
        amt = String.Format("{0:C}", amount).ToString
        tempStr = String.Format(deadlineDate & ", " & deadlineTime & ", " & program & ", " & from & ", " & _
                            strTo & ", " & purpose & ", " & desc & ", {0}" & amt & "{0}, " & reqBy & ", {0}" & dateTime & "{0}", """")
        Try
            strWriter = New StreamWriter(fs)
            strWriter.WriteLine("DeadLineDate, DeadLineTime, Program, From, To, Purpose, Desc, Amt, ReqBy, CurrDatAndTime")
            strWriter.Write(tempStr)
            strWriter.Close()
            borrower.PrintDoc("FUNDSTRAN", "X:\PADD\Operational Accounting\", TestMode, "T:\FundsTransForm.txt")
            'MessageBox.Show("Please retrieve Funds Transfer Form from the printer", "Print Successful", MessageBoxButtons.OK)
        Catch ex As Exception
            MessageBox.Show("Failed to create Funds Transfer Letter. Please contact system support", "Letter Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class

