Public Class frmChangeDueDate
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal tBor As SP.Borrower)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        Bor = tBor
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
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents CalNewDD As System.Windows.Forms.MonthCalendar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmChangeDueDate))
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.CalNewDD = New System.Windows.Forms.MonthCalendar
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(96, 176)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(200, 176)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'CalNewDD
        '
        Me.CalNewDD.CalendarDimensions = New System.Drawing.Size(2, 1)
        Me.CalNewDD.Location = New System.Drawing.Point(8, 8)
        Me.CalNewDD.Name = "CalNewDD"
        Me.CalNewDD.ShowToday = False
        Me.CalNewDD.TabIndex = 3
        Me.CalNewDD.TitleBackColor = System.Drawing.Color.Green
        Me.CalNewDD.TrailingForeColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        '
        'frmChangeDueDate
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(376, 205)
        Me.ControlBox = False
        Me.Controls.Add(Me.CalNewDD)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmChangeDueDate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Change Due Date"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private DD As String
    Private FrmCancelled As Boolean
    Private NewDD As String
    Private Bor As SP.Borrower

    Public Overloads Function Show(ByVal TempDD As String) As Boolean
        DD = TempDD
        CalNewDD.TodayDate = CDate(DD)
        CalNewDD.SelectionStart = CDate(DD)
        CalNewDD.MinDate = CDate(CDate(DD).Month & "/01/" & CDate(DD).Year)
        CalNewDD.MaxDate = CDate(CDate(DD).AddMonths(1).Month & "/" & CDate(DD).AddMonths(1).DaysInMonth(CDate(DD).AddMonths(1).Year, CDate(DD).AddMonths(1).Month) & "/" & CDate(DD).AddMonths(1).Year)
        FrmCancelled = False
        Me.Showdialog()
        If FrmCancelled = False Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        FrmCancelled = True
        Me.Hide()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim FourteenDays As New TimeSpan(14, 0, 0, 0)
        If CalNewDD.SelectionStart <= CDate(DD) Then
            SP.frmWhoaDUDE.WhoaDUDE("The new due date must come after the current due date.", "Invalid Date")
        ElseIf CalNewDD.SelectionStart.Day > 29 Then
            SP.frmWhoaDUDE.WhoaDUDE("The new due date must be on or before the 28th of the month.", "Invalid Date")
        Else
            'if data is valid then hide the form and return to scripts and services
            'if new due date is greater than 14 days from now then give warning
            If CalNewDD.SelectionStart.Subtract(CDate(DD)).Days > FourteenDays.Days Then
                SP.frmWhoaDUDE.WhoaDUDE("Please inform the borrower that the new due date will require two billing cycles in order to change the date to the requested date.", "Two Billing Cycles")
            End If
            FrmCancelled = False
            SP.frmKnarlyDUDE.KnarlyDude("Processing Complete", "Processing Complete")
            Me.Hide()
        End If
    End Sub

    'updates system with appropriate activity comment
    Public Sub WriteActivityCommentOut()
        Bor.ActivityCmts.AddCommentsToTD22AllLoans("Borrower Requested repayment due day " & CalNewDD.SelectionStart.Day & ".", "DUEDT")
    End Sub

End Class
