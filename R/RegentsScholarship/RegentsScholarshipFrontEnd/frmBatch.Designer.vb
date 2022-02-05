<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatch
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatch))
        Me.btnWeeklyReports = New System.Windows.Forms.Button
        Me.btnReviews = New System.Windows.Forms.Button
        Me.btnLetters = New System.Windows.Forms.Button
        Me.btnQuit = New System.Windows.Forms.Button
        Me.grpLetters = New System.Windows.Forms.GroupBox
        Me.chkEverythingElse = New System.Windows.Forms.CheckBox
        Me.chkDenialForIncompleteApplication = New System.Windows.Forms.CheckBox
        Me.grpLetters.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnWeeklyReports
        '
        Me.btnWeeklyReports.BackColor = System.Drawing.Color.DarkKhaki
        Me.btnWeeklyReports.Location = New System.Drawing.Point(12, 130)
        Me.btnWeeklyReports.Name = "btnWeeklyReports"
        Me.btnWeeklyReports.Size = New System.Drawing.Size(111, 23)
        Me.btnWeeklyReports.TabIndex = 10
        Me.btnWeeklyReports.Text = "Weekly Reports"
        Me.btnWeeklyReports.UseVisualStyleBackColor = False
        '
        'btnReviews
        '
        Me.btnReviews.BackColor = System.Drawing.Color.DarkKhaki
        Me.btnReviews.Location = New System.Drawing.Point(12, 92)
        Me.btnReviews.Name = "btnReviews"
        Me.btnReviews.Size = New System.Drawing.Size(111, 23)
        Me.btnReviews.TabIndex = 11
        Me.btnReviews.Text = "Process Batch"
        Me.btnReviews.UseVisualStyleBackColor = False
        '
        'btnLetters
        '
        Me.btnLetters.BackColor = System.Drawing.Color.DarkKhaki
        Me.btnLetters.ForeColor = System.Drawing.Color.Black
        Me.btnLetters.Location = New System.Drawing.Point(40, 76)
        Me.btnLetters.Name = "btnLetters"
        Me.btnLetters.Size = New System.Drawing.Size(111, 23)
        Me.btnLetters.TabIndex = 12
        Me.btnLetters.Text = "Print Selected"
        Me.btnLetters.UseVisualStyleBackColor = False
        '
        'btnQuit
        '
        Me.btnQuit.BackColor = System.Drawing.Color.Firebrick
        Me.btnQuit.ForeColor = System.Drawing.Color.PeachPuff
        Me.btnQuit.Location = New System.Drawing.Point(12, 168)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(111, 23)
        Me.btnQuit.TabIndex = 13
        Me.btnQuit.Text = "Quit"
        Me.btnQuit.UseVisualStyleBackColor = False
        '
        'grpLetters
        '
        Me.grpLetters.BackColor = System.Drawing.Color.Transparent
        Me.grpLetters.Controls.Add(Me.chkDenialForIncompleteApplication)
        Me.grpLetters.Controls.Add(Me.chkEverythingElse)
        Me.grpLetters.Controls.Add(Me.btnLetters)
        Me.grpLetters.ForeColor = System.Drawing.Color.DarkKhaki
        Me.grpLetters.Location = New System.Drawing.Point(139, 82)
        Me.grpLetters.Name = "grpLetters"
        Me.grpLetters.Size = New System.Drawing.Size(190, 109)
        Me.grpLetters.TabIndex = 14
        Me.grpLetters.TabStop = False
        Me.grpLetters.Text = "Letters"
        '
        'chkEverythingElse
        '
        Me.chkEverythingElse.AutoSize = True
        Me.chkEverythingElse.Location = New System.Drawing.Point(6, 42)
        Me.chkEverythingElse.Name = "chkEverythingElse"
        Me.chkEverythingElse.Size = New System.Drawing.Size(99, 17)
        Me.chkEverythingElse.TabIndex = 13
        Me.chkEverythingElse.Text = "Everything Else"
        Me.chkEverythingElse.UseVisualStyleBackColor = True
        '
        'chkDenialForIncompleteApplication
        '
        Me.chkDenialForIncompleteApplication.AutoSize = True
        Me.chkDenialForIncompleteApplication.Location = New System.Drawing.Point(6, 19)
        Me.chkDenialForIncompleteApplication.Name = "chkDenialForIncompleteApplication"
        Me.chkDenialForIncompleteApplication.Size = New System.Drawing.Size(181, 17)
        Me.chkDenialForIncompleteApplication.TabIndex = 14
        Me.chkDenialForIncompleteApplication.Text = "Denial for Incomplete Application"
        Me.chkDenialForIncompleteApplication.UseVisualStyleBackColor = True
        '
        'frmBatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.ushe
        Me.ClientSize = New System.Drawing.Size(342, 204)
        Me.Controls.Add(Me.grpLetters)
        Me.Controls.Add(Me.btnQuit)
        Me.Controls.Add(Me.btnReviews)
        Me.Controls.Add(Me.btnWeeklyReports)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBatch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Regents' Scholarship Batch Options"
        Me.grpLetters.ResumeLayout(False)
        Me.grpLetters.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnWeeklyReports As System.Windows.Forms.Button
    Friend WithEvents btnReviews As System.Windows.Forms.Button
    Friend WithEvents btnLetters As System.Windows.Forms.Button
    Friend WithEvents btnQuit As System.Windows.Forms.Button
    Friend WithEvents grpLetters As System.Windows.Forms.GroupBox
    Friend WithEvents chkDenialForIncompleteApplication As System.Windows.Forms.CheckBox
    Friend WithEvents chkEverythingElse As System.Windows.Forms.CheckBox
End Class
