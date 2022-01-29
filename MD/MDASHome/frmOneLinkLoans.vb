Public Class frmOneLinkLoans
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal Loans(,) As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        '****Me.ForeColor = SP.GeneralForeColor
        '****Me.BackColor = SP.GeneralBackColor
        '****Me.Opacity = SP.GeneralTransparency
        lvLoans.BackColor = Me.BackColor
        lvLoans.ForeColor = Me.ForeColor
        Dim x As Integer
        Dim Ldex As Integer
        For x = Loans.GetLowerBound(1) To Loans.GetUpperBound(1)
            If Loans(1, x) <> "" Then
                Ldex = lvLoans.Items.Add(Loans(1, x)).Index
                lvLoans.Items(Ldex).SubItems.Add(Loans(2, x))
                lvLoans.Items(Ldex).SubItems.Add(Loans(3, x).Insert(2, "/").Insert(5, "/"))
                lvLoans.Items(Ldex).SubItems.Add(Loans(4, x))
                lvLoans.Items(Ldex).SubItems.Add(Loans(0, x))
                lvLoans.Items(Ldex).SubItems.Add(Loans(5, x))
                lvLoans.Items(Ldex).SubItems.Add(Loans(6, x))
            End If
        Next

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.Hide()
        'If disposing Then
        '    If Not (components Is Nothing) Then
        '        components.Dispose()
        '    End If
        'End If
        'MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents LoanType As System.Windows.Forms.ColumnHeader
    Friend WithEvents Servicer As System.Windows.Forms.ColumnHeader
    Friend WithEvents DateProcessed As System.Windows.Forms.ColumnHeader
    Friend WithEvents GuaranteeAmount As System.Windows.Forms.ColumnHeader
    Friend WithEvents PrincipalBalance As System.Windows.Forms.ColumnHeader
    Friend WithEvents LoanStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents ReasonCode As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lvLoans As System.Windows.Forms.ListView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmOneLinkLoans))
        Me.lvLoans = New System.Windows.Forms.ListView
        Me.LoanType = New System.Windows.Forms.ColumnHeader
        Me.Servicer = New System.Windows.Forms.ColumnHeader
        Me.DateProcessed = New System.Windows.Forms.ColumnHeader
        Me.GuaranteeAmount = New System.Windows.Forms.ColumnHeader
        Me.PrincipalBalance = New System.Windows.Forms.ColumnHeader
        Me.LoanStatus = New System.Windows.Forms.ColumnHeader
        Me.ReasonCode = New System.Windows.Forms.ColumnHeader
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lvLoans
        '
        Me.lvLoans.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.LoanType, Me.Servicer, Me.DateProcessed, Me.GuaranteeAmount, Me.PrincipalBalance, Me.LoanStatus, Me.ReasonCode})
        Me.lvLoans.FullRowSelect = True
        Me.lvLoans.Location = New System.Drawing.Point(8, 40)
        Me.lvLoans.Name = "lvLoans"
        Me.lvLoans.Size = New System.Drawing.Size(624, 160)
        Me.lvLoans.TabIndex = 0
        Me.lvLoans.View = System.Windows.Forms.View.Details
        '
        'LoanType
        '
        Me.LoanType.Text = "Loan Type"
        Me.LoanType.Width = 75
        '
        'Servicer
        '
        Me.Servicer.Text = "Servicer"
        Me.Servicer.Width = 67
        '
        'DateProcessed
        '
        Me.DateProcessed.Text = "First Disbursement"
        Me.DateProcessed.Width = 98
        '
        'GuaranteeAmount
        '
        Me.GuaranteeAmount.Text = "Guarantee Amount"
        Me.GuaranteeAmount.Width = 107
        '
        'PrincipalBalance
        '
        Me.PrincipalBalance.Text = "Principal Balance"
        Me.PrincipalBalance.Width = 102
        '
        'LoanStatus
        '
        Me.LoanStatus.Text = "Loan Status"
        Me.LoanStatus.Width = 80
        '
        'ReasonCode
        '
        Me.ReasonCode.Text = "Reason Code"
        Me.ReasonCode.Width = 91
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(280, 208)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Bodoni MT Black", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(16, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(616, 24)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "OneLINK Loans"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmOneLinkLoans
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(640, 238)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lvLoans)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(648, 272)
        Me.MinimumSize = New System.Drawing.Size(648, 272)
        Me.Name = "frmOneLinkLoans"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE OneLINK Loans"
        Me.ResumeLayout(False)

    End Sub

#End Region
    
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
