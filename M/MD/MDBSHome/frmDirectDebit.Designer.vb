Partial Public Class frmDirectDebit
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.Hide()
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblNC As System.Windows.Forms.Label
    Friend WithEvents lblAWA As System.Windows.Forms.Label
    Friend WithEvents lblAN As System.Windows.Forms.Label
    Friend WithEvents lblRN As System.Windows.Forms.Label
    Friend WithEvents lblSD As System.Windows.Forms.Label
    Friend WithEvents lblASN As System.Windows.Forms.Label
    Friend WithEvents lblDD As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents LVLoanInfo As System.Windows.Forms.ListView
    Friend WithEvents lblNoInfo As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblDR As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDirectDebit))
        Me.LVLoanInfo = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lblNC = New System.Windows.Forms.Label
        Me.lblAWA = New System.Windows.Forms.Label
        Me.lblAN = New System.Windows.Forms.Label
        Me.lblRN = New System.Windows.Forms.Label
        Me.lblSD = New System.Windows.Forms.Label
        Me.lblASN = New System.Windows.Forms.Label
        Me.lblDD = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.lblNoInfo = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.lblDR = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'LVLoanInfo
        '
        Me.LVLoanInfo.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.LVLoanInfo.Location = New System.Drawing.Point(8, 204)
        Me.LVLoanInfo.Name = "LVLoanInfo"
        Me.LVLoanInfo.Size = New System.Drawing.Size(352, 124)
        Me.LVLoanInfo.TabIndex = 0
        Me.LVLoanInfo.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Seq #"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "1st Disb Date"
        Me.ColumnHeader2.Width = 131
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Loan Type"
        Me.ColumnHeader3.Width = 156
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(168, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Direct Debit:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(168, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "ACH Seq #:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(168, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Status Date:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 68)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(168, 16)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Routing #:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 88)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(168, 16)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Account #:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 108)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(168, 16)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Additional Withdrawal Amt:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 128)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(168, 16)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "NSF Counter:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblNC
        '
        Me.lblNC.Location = New System.Drawing.Point(196, 128)
        Me.lblNC.Name = "lblNC"
        Me.lblNC.Size = New System.Drawing.Size(168, 16)
        Me.lblNC.TabIndex = 14
        Me.lblNC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAWA
        '
        Me.lblAWA.Location = New System.Drawing.Point(196, 108)
        Me.lblAWA.Name = "lblAWA"
        Me.lblAWA.Size = New System.Drawing.Size(168, 16)
        Me.lblAWA.TabIndex = 13
        Me.lblAWA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAN
        '
        Me.lblAN.Location = New System.Drawing.Point(196, 88)
        Me.lblAN.Name = "lblAN"
        Me.lblAN.Size = New System.Drawing.Size(168, 16)
        Me.lblAN.TabIndex = 12
        Me.lblAN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblRN
        '
        Me.lblRN.Location = New System.Drawing.Point(196, 68)
        Me.lblRN.Name = "lblRN"
        Me.lblRN.Size = New System.Drawing.Size(168, 16)
        Me.lblRN.TabIndex = 11
        Me.lblRN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSD
        '
        Me.lblSD.Location = New System.Drawing.Point(196, 28)
        Me.lblSD.Name = "lblSD"
        Me.lblSD.Size = New System.Drawing.Size(168, 16)
        Me.lblSD.TabIndex = 10
        Me.lblSD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblASN
        '
        Me.lblASN.Location = New System.Drawing.Point(196, 48)
        Me.lblASN.Name = "lblASN"
        Me.lblASN.Size = New System.Drawing.Size(168, 16)
        Me.lblASN.TabIndex = 9
        Me.lblASN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDD
        '
        Me.lblDD.Location = New System.Drawing.Point(196, 8)
        Me.lblDD.Name = "lblDD"
        Me.lblDD.Size = New System.Drawing.Size(168, 16)
        Me.lblDD.TabIndex = 8
        Me.lblDD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(4, 184)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(100, 16)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Loan Info:"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(144, 336)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 16
        Me.btnClose.Text = "Close"
        '
        'lblNoInfo
        '
        Me.lblNoInfo.Font = New System.Drawing.Font("Stencil", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoInfo.ForeColor = System.Drawing.Color.Red
        Me.lblNoInfo.Image = CType(resources.GetObject("lblNoInfo.Image"), System.Drawing.Image)
        Me.lblNoInfo.Location = New System.Drawing.Point(4, 16)
        Me.lblNoInfo.Name = "lblNoInfo"
        Me.lblNoInfo.Size = New System.Drawing.Size(360, 300)
        Me.lblNoInfo.TabIndex = 17
        Me.lblNoInfo.Text = "Sorry, No Direct Debit (ACH) Information Available."
        Me.lblNoInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 148)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(168, 16)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Denial Reason (If Applicable):"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDR
        '
        Me.lblDR.Location = New System.Drawing.Point(196, 148)
        Me.lblDR.Name = "lblDR"
        Me.lblDR.Size = New System.Drawing.Size(168, 16)
        Me.lblDR.TabIndex = 19
        Me.lblDR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmDirectDebit
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(368, 366)
        Me.Controls.Add(Me.lblDR)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.lblNoInfo)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lblNC)
        Me.Controls.Add(Me.lblAWA)
        Me.Controls.Add(Me.lblAN)
        Me.Controls.Add(Me.lblRN)
        Me.Controls.Add(Me.lblSD)
        Me.Controls.Add(Me.lblASN)
        Me.Controls.Add(Me.lblDD)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LVLoanInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(376, 400)
        Me.MinimumSize = New System.Drawing.Size(376, 400)
        Me.Name = "frmDirectDebit"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Direct Debit (ACH) Information"
        Me.ResumeLayout(False)
    End Sub
End Class
