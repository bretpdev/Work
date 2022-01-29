Public Class frmNCSP
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        mnuReports.MenuItems.Add(New MenuItem("Account Recap", AddressOf ReportMenuHandler))
        mnuReports.MenuItems.Add(New MenuItem("Associate Degree Recipients", AddressOf ReportMenuHandler))
        mnuReports.MenuItems.Add(New MenuItem("Batch Recap", AddressOf ReportMenuHandler))
        mnuReports.MenuItems.Add(New MenuItem("Count Of New Recipients (By Date Range)", AddressOf ReportMenuHandler))
        mnuReports.MenuItems.Add(New MenuItem("Current List Of Recipients", AddressOf ReportMenuHandler))
        mnuReports.MenuItems.Add(New MenuItem("Disbursement Totals (By Date Range)", AddressOf ReportMenuHandler))
        mnuReports.MenuItems.Add(New MenuItem("Refund(s) Due Student(s)", AddressOf ReportMenuHandler))
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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuStudents As System.Windows.Forms.MenuItem
    Friend WithEvents mnuReports As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuPmtProcMnu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCreateAdj As System.Windows.Forms.MenuItem
    Friend WithEvents mnuBatchRecap As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCreateBatch As System.Windows.Forms.MenuItem
    Friend WithEvents mnuPostBatch As System.Windows.Forms.MenuItem
    Friend WithEvents mnuUtilitiesMnu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuPassword As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmNCSP))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuStudents = New System.Windows.Forms.MenuItem
        Me.mnuPmtProcMnu = New System.Windows.Forms.MenuItem
        Me.mnuCreateAdj = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.mnuBatchRecap = New System.Windows.Forms.MenuItem
        Me.mnuCreateBatch = New System.Windows.Forms.MenuItem
        Me.mnuPostBatch = New System.Windows.Forms.MenuItem
        Me.mnuReports = New System.Windows.Forms.MenuItem
        Me.mnuUtilitiesMnu = New System.Windows.Forms.MenuItem
        Me.mnuPassword = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuStudents, Me.mnuPmtProcMnu, Me.mnuReports, Me.mnuUtilitiesMnu, Me.mnuExit})
        '
        'mnuStudents
        '
        Me.mnuStudents.Index = 0
        Me.mnuStudents.Text = "&Students"
        '
        'mnuPmtProcMnu
        '
        Me.mnuPmtProcMnu.Index = 1
        Me.mnuPmtProcMnu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuCreateAdj, Me.MenuItem3, Me.mnuBatchRecap, Me.mnuCreateBatch, Me.mnuPostBatch})
        Me.mnuPmtProcMnu.Text = "&Payment Processing"
        '
        'mnuCreateAdj
        '
        Me.mnuCreateAdj.Index = 0
        Me.mnuCreateAdj.Text = "&Enter Adjustments"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Text = "-"
        '
        'mnuBatchRecap
        '
        Me.mnuBatchRecap.Index = 2
        Me.mnuBatchRecap.Text = "Batch &Recap"
        '
        'mnuCreateBatch
        '
        Me.mnuCreateBatch.Index = 3
        Me.mnuCreateBatch.Text = "Create &Batch"
        '
        'mnuPostBatch
        '
        Me.mnuPostBatch.Index = 4
        Me.mnuPostBatch.Text = "&Post Batch"
        '
        'mnuReports
        '
        Me.mnuReports.Index = 2
        Me.mnuReports.Text = "&Reports"
        '
        'mnuUtilitiesMnu
        '
        Me.mnuUtilitiesMnu.Index = 3
        Me.mnuUtilitiesMnu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuPassword})
        Me.mnuUtilitiesMnu.Text = "&Utilities"
        '
        'mnuPassword
        '
        Me.mnuPassword.Index = 0
        Me.mnuPassword.Text = "&Change Password"
        '
        'mnuExit
        '
        Me.mnuExit.Index = 4
        Me.mnuExit.Text = "&Exit"
        '
        'frmNCSP
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(992, 653)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MaximumSize = New System.Drawing.Size(1000, 700)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmNCSP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Century Scholarship Program"

    End Sub

#End Region

#Region "Form Functions"
    'load form
    Private Sub frmNCSP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim frmMain As New frmMain

        If UserAccess = "Read Only Access" Then
            mnuCreateAdj.Enabled = False
            mnuCreateBatch.Enabled = False
            mnuPostBatch.Enabled = False
        End If

        frmMain.MdiParent = Me
        frmMain.Show()
    End Sub

    'close application
    Private Sub frmNCSP_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        ShutDown()
    End Sub
#End Region

#Region "Menu Items"
    'display student selection (main menu) screen
    Protected Sub Students_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuStudents.Click
        Dim i As Integer

        'the main menu is always displayed, this function closes other forms so it is visible
        i = 0
        While i <= UBound(ShowForms.frmNCSPForm.MdiChildren)
            If ShowForms.frmNCSPForm.MdiChildren(i).Name = "frmMain" Then
                i = i + 1
            Else
                ShowForms.frmNCSPForm.MdiChildren(i).Close()
            End If

        End While
    End Sub

    'display create adjustments screen
    Protected Sub CreateAdjustments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCreateAdj.Click
        Dim frmCreateAdjustments As New frmCreateAdjustments
        frmCreateAdjustments.MdiParent = Me
        frmCreateAdjustments.Show()
    End Sub

    'generate batch recap report
    Private Sub mnuBatchRecap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBatchRecap.Click
        Dim frmReport As New frmReports
        frmReport.MdiParent = Me
        frmReport.Show("Batch Recap")
    End Sub

    'create batch
    Private Sub mnuCreateBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCreateBatch.Click
        Dim CB As New Batch
        CB.GenerateBatch()
    End Sub

    'display payment posting screen
    Protected Sub PaymentPosting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPostBatch.Click
        Dim frmPaymentPosting As New frmPaymentPosting
        frmPaymentPosting.MdiParent = Me
        frmPaymentPosting.Show()
    End Sub

    'generate reports
    Private Sub ReportMenuHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim RFrm As frmReports
        Dim I As Integer
        'check if the report form is already in existance
        While Me.MdiChildren.Length > I
            If Me.MdiChildren.GetValue(I).GetType.ToString = "NCSP.frmReports" Then
                CType(Me.MdiChildren.GetValue(I), NCSP.frmReports).UpdateReportAndForm(CType(sender, MenuItem).Text)
                CType(Me.MdiChildren.GetValue(I), NCSP.frmReports).Focus()
                Exit Sub
            End If
            I = I + 1
        End While
        'if not found above then create form
        RFrm = New frmReports(CType(sender, MenuItem).Text, Me)
    End Sub

    'change password
    Protected Sub Password_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPassword.Click
        ShowForms.ChangePassword(False)
    End Sub

    'close application
    Private Sub mnuExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        ShutDown()
    End Sub
#End Region

End Class
