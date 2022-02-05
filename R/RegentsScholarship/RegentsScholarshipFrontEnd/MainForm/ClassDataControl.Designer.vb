<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ClassDataControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.pnlClassAcceptable = New System.Windows.Forms.Panel
        Me.lblClassAcceptable = New System.Windows.Forms.Label
        Me.radClassAcceptableInProgress = New System.Windows.Forms.RadioButton
        Me.radClassAcceptableYes = New System.Windows.Forms.RadioButton
        Me.radClassAcceptableNo = New System.Windows.Forms.RadioButton
        Me.txtWeightedAverageGradeEquivalent = New System.Windows.Forms.TextBox
        Me.cmbTerm6Grade = New System.Windows.Forms.ComboBox
        Me.cmbWeightDesignation = New System.Windows.Forms.ComboBox
        Me.cmbTerm5Grade = New System.Windows.Forms.ComboBox
        Me.cmbTerm4Grade = New System.Windows.Forms.ComboBox
        Me.txtClassVerifiedDate = New System.Windows.Forms.TextBox
        Me.cmbTerm3Grade = New System.Windows.Forms.ComboBox
        Me.cmbTerm2Grade = New System.Windows.Forms.ComboBox
        Me.txtGradeLevel = New System.Windows.Forms.TextBox
        Me.cmbTerm1Grade = New System.Windows.Forms.ComboBox
        Me.txtWeightedAverageGrade = New System.Windows.Forms.TextBox
        Me.txtCredits = New System.Windows.Forms.TextBox
        Me.txtClassVerifiedBy = New System.Windows.Forms.TextBox
        Me.cmbTitle = New System.Windows.Forms.ComboBox
        Me.lblClassVerifiedBy = New System.Windows.Forms.Label
        Me.txtAcademicYear = New System.Windows.Forms.MaskedTextBox
        Me.cmbSchoolAttended = New System.Windows.Forms.ComboBox
        Me.cmbConcurrentCollege = New System.Windows.Forms.ComboBox
        Me.pnlClassAcceptable.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlClassAcceptable
        '
        Me.pnlClassAcceptable.Controls.Add(Me.lblClassAcceptable)
        Me.pnlClassAcceptable.Controls.Add(Me.radClassAcceptableInProgress)
        Me.pnlClassAcceptable.Controls.Add(Me.radClassAcceptableYes)
        Me.pnlClassAcceptable.Controls.Add(Me.radClassAcceptableNo)
        Me.pnlClassAcceptable.Location = New System.Drawing.Point(343, 33)
        Me.pnlClassAcceptable.Name = "pnlClassAcceptable"
        Me.pnlClassAcceptable.Size = New System.Drawing.Size(245, 14)
        Me.pnlClassAcceptable.TabIndex = 315
        '
        'lblClassAcceptable
        '
        Me.lblClassAcceptable.AutoSize = True
        Me.lblClassAcceptable.Location = New System.Drawing.Point(3, 0)
        Me.lblClassAcceptable.Name = "lblClassAcceptable"
        Me.lblClassAcceptable.Size = New System.Drawing.Size(72, 13)
        Me.lblClassAcceptable.TabIndex = 33
        Me.lblClassAcceptable.Text = "Class Accept."
        '
        'radClassAcceptableInProgress
        '
        Me.radClassAcceptableInProgress.AutoSize = True
        Me.radClassAcceptableInProgress.Location = New System.Drawing.Point(158, -2)
        Me.radClassAcceptableInProgress.Name = "radClassAcceptableInProgress"
        Me.radClassAcceptableInProgress.Size = New System.Drawing.Size(78, 17)
        Me.radClassAcceptableInProgress.TabIndex = 15
        Me.radClassAcceptableInProgress.TabStop = True
        Me.radClassAcceptableInProgress.Text = "In Progress"
        Me.radClassAcceptableInProgress.UseVisualStyleBackColor = True
        '
        'radClassAcceptableYes
        '
        Me.radClassAcceptableYes.AutoSize = True
        Me.radClassAcceptableYes.Location = New System.Drawing.Point(81, -2)
        Me.radClassAcceptableYes.Name = "radClassAcceptableYes"
        Me.radClassAcceptableYes.Size = New System.Drawing.Size(32, 17)
        Me.radClassAcceptableYes.TabIndex = 13
        Me.radClassAcceptableYes.TabStop = True
        Me.radClassAcceptableYes.Text = "Y"
        Me.radClassAcceptableYes.UseVisualStyleBackColor = True
        '
        'radClassAcceptableNo
        '
        Me.radClassAcceptableNo.AutoSize = True
        Me.radClassAcceptableNo.Location = New System.Drawing.Point(119, -2)
        Me.radClassAcceptableNo.Name = "radClassAcceptableNo"
        Me.radClassAcceptableNo.Size = New System.Drawing.Size(33, 17)
        Me.radClassAcceptableNo.TabIndex = 14
        Me.radClassAcceptableNo.TabStop = True
        Me.radClassAcceptableNo.Text = "N"
        Me.radClassAcceptableNo.UseVisualStyleBackColor = True
        '
        'txtWeightedAverageGradeEquivalent
        '
        Me.txtWeightedAverageGradeEquivalent.Location = New System.Drawing.Point(841, 3)
        Me.txtWeightedAverageGradeEquivalent.Name = "txtWeightedAverageGradeEquivalent"
        Me.txtWeightedAverageGradeEquivalent.ReadOnly = True
        Me.txtWeightedAverageGradeEquivalent.Size = New System.Drawing.Size(33, 20)
        Me.txtWeightedAverageGradeEquivalent.TabIndex = 0
        Me.txtWeightedAverageGradeEquivalent.TabStop = False
        '
        'cmbTerm6Grade
        '
        Me.cmbTerm6Grade.FormattingEnabled = True
        Me.cmbTerm6Grade.Location = New System.Drawing.Point(761, 3)
        Me.cmbTerm6Grade.Name = "cmbTerm6Grade"
        Me.cmbTerm6Grade.Size = New System.Drawing.Size(41, 21)
        Me.cmbTerm6Grade.TabIndex = 11
        '
        'cmbWeightDesignation
        '
        Me.cmbWeightDesignation.FormattingEnabled = True
        Me.cmbWeightDesignation.Location = New System.Drawing.Point(343, 3)
        Me.cmbWeightDesignation.Name = "cmbWeightDesignation"
        Me.cmbWeightDesignation.Size = New System.Drawing.Size(55, 21)
        Me.cmbWeightDesignation.TabIndex = 2
        '
        'cmbTerm5Grade
        '
        Me.cmbTerm5Grade.FormattingEnabled = True
        Me.cmbTerm5Grade.Location = New System.Drawing.Point(714, 3)
        Me.cmbTerm5Grade.Name = "cmbTerm5Grade"
        Me.cmbTerm5Grade.Size = New System.Drawing.Size(41, 21)
        Me.cmbTerm5Grade.TabIndex = 10
        '
        'cmbTerm4Grade
        '
        Me.cmbTerm4Grade.FormattingEnabled = True
        Me.cmbTerm4Grade.Location = New System.Drawing.Point(667, 3)
        Me.cmbTerm4Grade.Name = "cmbTerm4Grade"
        Me.cmbTerm4Grade.Size = New System.Drawing.Size(41, 21)
        Me.cmbTerm4Grade.TabIndex = 9
        '
        'txtClassVerifiedDate
        '
        Me.txtClassVerifiedDate.Location = New System.Drawing.Point(808, 29)
        Me.txtClassVerifiedDate.Name = "txtClassVerifiedDate"
        Me.txtClassVerifiedDate.ReadOnly = True
        Me.txtClassVerifiedDate.Size = New System.Drawing.Size(66, 20)
        Me.txtClassVerifiedDate.TabIndex = 0
        Me.txtClassVerifiedDate.TabStop = False
        '
        'cmbTerm3Grade
        '
        Me.cmbTerm3Grade.FormattingEnabled = True
        Me.cmbTerm3Grade.Location = New System.Drawing.Point(620, 3)
        Me.cmbTerm3Grade.Name = "cmbTerm3Grade"
        Me.cmbTerm3Grade.Size = New System.Drawing.Size(41, 21)
        Me.cmbTerm3Grade.TabIndex = 8
        '
        'cmbTerm2Grade
        '
        Me.cmbTerm2Grade.FormattingEnabled = True
        Me.cmbTerm2Grade.Location = New System.Drawing.Point(573, 3)
        Me.cmbTerm2Grade.Name = "cmbTerm2Grade"
        Me.cmbTerm2Grade.Size = New System.Drawing.Size(41, 21)
        Me.cmbTerm2Grade.TabIndex = 7
        '
        'txtGradeLevel
        '
        Me.txtGradeLevel.Location = New System.Drawing.Point(404, 4)
        Me.txtGradeLevel.MaxLength = 2
        Me.txtGradeLevel.Name = "txtGradeLevel"
        Me.txtGradeLevel.Size = New System.Drawing.Size(26, 20)
        Me.txtGradeLevel.TabIndex = 3
        '
        'cmbTerm1Grade
        '
        Me.cmbTerm1Grade.FormattingEnabled = True
        Me.cmbTerm1Grade.Location = New System.Drawing.Point(526, 3)
        Me.cmbTerm1Grade.Name = "cmbTerm1Grade"
        Me.cmbTerm1Grade.Size = New System.Drawing.Size(41, 21)
        Me.cmbTerm1Grade.TabIndex = 6
        '
        'txtWeightedAverageGrade
        '
        Me.txtWeightedAverageGrade.Location = New System.Drawing.Point(808, 3)
        Me.txtWeightedAverageGrade.Name = "txtWeightedAverageGrade"
        Me.txtWeightedAverageGrade.ReadOnly = True
        Me.txtWeightedAverageGrade.Size = New System.Drawing.Size(27, 20)
        Me.txtWeightedAverageGrade.TabIndex = 0
        Me.txtWeightedAverageGrade.TabStop = False
        '
        'txtCredits
        '
        Me.txtCredits.Location = New System.Drawing.Point(490, 4)
        Me.txtCredits.Name = "txtCredits"
        Me.txtCredits.Size = New System.Drawing.Size(30, 20)
        Me.txtCredits.TabIndex = 5
        '
        'txtClassVerifiedBy
        '
        Me.txtClassVerifiedBy.Location = New System.Drawing.Point(683, 29)
        Me.txtClassVerifiedBy.Name = "txtClassVerifiedBy"
        Me.txtClassVerifiedBy.ReadOnly = True
        Me.txtClassVerifiedBy.Size = New System.Drawing.Size(119, 20)
        Me.txtClassVerifiedBy.TabIndex = 0
        Me.txtClassVerifiedBy.TabStop = False
        '
        'cmbTitle
        '
        Me.cmbTitle.FormattingEnabled = True
        Me.cmbTitle.Location = New System.Drawing.Point(3, 3)
        Me.cmbTitle.MaxLength = 35
        Me.cmbTitle.Name = "cmbTitle"
        Me.cmbTitle.Size = New System.Drawing.Size(334, 21)
        Me.cmbTitle.TabIndex = 1
        '
        'lblClassVerifiedBy
        '
        Me.lblClassVerifiedBy.AutoSize = True
        Me.lblClassVerifiedBy.Location = New System.Drawing.Point(603, 33)
        Me.lblClassVerifiedBy.Name = "lblClassVerifiedBy"
        Me.lblClassVerifiedBy.Size = New System.Drawing.Size(74, 13)
        Me.lblClassVerifiedBy.TabIndex = 307
        Me.lblClassVerifiedBy.Text = "Class Verif. By"
        '
        'txtAcademicYear
        '
        Me.txtAcademicYear.Location = New System.Drawing.Point(436, 4)
        Me.txtAcademicYear.Mask = "00/00"
        Me.txtAcademicYear.Name = "txtAcademicYear"
        Me.txtAcademicYear.Size = New System.Drawing.Size(48, 20)
        Me.txtAcademicYear.TabIndex = 4
        '
        'cmbSchoolAttended
        '
        Me.cmbSchoolAttended.FormattingEnabled = True
        Me.cmbSchoolAttended.Location = New System.Drawing.Point(3, 30)
        Me.cmbSchoolAttended.MaxLength = 35
        Me.cmbSchoolAttended.Name = "cmbSchoolAttended"
        Me.cmbSchoolAttended.Size = New System.Drawing.Size(171, 21)
        Me.cmbSchoolAttended.TabIndex = 12
        '
        'cmbConcurrentCollege
        '
        Me.cmbConcurrentCollege.Enabled = False
        Me.cmbConcurrentCollege.FormattingEnabled = True
        Me.cmbConcurrentCollege.Location = New System.Drawing.Point(180, 30)
        Me.cmbConcurrentCollege.MaxLength = 35
        Me.cmbConcurrentCollege.Name = "cmbConcurrentCollege"
        Me.cmbConcurrentCollege.Size = New System.Drawing.Size(157, 21)
        Me.cmbConcurrentCollege.TabIndex = 316
        '
        'ClassDataControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cmbConcurrentCollege)
        Me.Controls.Add(Me.cmbSchoolAttended)
        Me.Controls.Add(Me.txtAcademicYear)
        Me.Controls.Add(Me.pnlClassAcceptable)
        Me.Controls.Add(Me.txtWeightedAverageGradeEquivalent)
        Me.Controls.Add(Me.cmbTerm6Grade)
        Me.Controls.Add(Me.cmbWeightDesignation)
        Me.Controls.Add(Me.cmbTerm5Grade)
        Me.Controls.Add(Me.cmbTerm4Grade)
        Me.Controls.Add(Me.txtClassVerifiedDate)
        Me.Controls.Add(Me.cmbTerm3Grade)
        Me.Controls.Add(Me.cmbTerm2Grade)
        Me.Controls.Add(Me.txtGradeLevel)
        Me.Controls.Add(Me.cmbTerm1Grade)
        Me.Controls.Add(Me.txtWeightedAverageGrade)
        Me.Controls.Add(Me.txtCredits)
        Me.Controls.Add(Me.txtClassVerifiedBy)
        Me.Controls.Add(Me.cmbTitle)
        Me.Controls.Add(Me.lblClassVerifiedBy)
        Me.Name = "ClassDataControl"
        Me.Size = New System.Drawing.Size(879, 56)
        Me.pnlClassAcceptable.ResumeLayout(False)
        Me.pnlClassAcceptable.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlClassAcceptable As System.Windows.Forms.Panel
    Friend WithEvents lblClassAcceptable As System.Windows.Forms.Label
    Friend WithEvents radClassAcceptableInProgress As System.Windows.Forms.RadioButton
    Friend WithEvents radClassAcceptableYes As System.Windows.Forms.RadioButton
    Friend WithEvents radClassAcceptableNo As System.Windows.Forms.RadioButton
    Friend WithEvents txtWeightedAverageGradeEquivalent As System.Windows.Forms.TextBox
    Friend WithEvents cmbTerm6Grade As System.Windows.Forms.ComboBox
    Friend WithEvents cmbWeightDesignation As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTerm5Grade As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTerm4Grade As System.Windows.Forms.ComboBox
    Friend WithEvents txtClassVerifiedDate As System.Windows.Forms.TextBox
    Friend WithEvents cmbTerm3Grade As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTerm2Grade As System.Windows.Forms.ComboBox
    Friend WithEvents txtGradeLevel As System.Windows.Forms.TextBox
    Friend WithEvents cmbTerm1Grade As System.Windows.Forms.ComboBox
    Friend WithEvents txtWeightedAverageGrade As System.Windows.Forms.TextBox
    Friend WithEvents txtCredits As System.Windows.Forms.TextBox
    Friend WithEvents txtClassVerifiedBy As System.Windows.Forms.TextBox
    Friend WithEvents cmbTitle As System.Windows.Forms.ComboBox
    Friend WithEvents lblClassVerifiedBy As System.Windows.Forms.Label
    Friend WithEvents txtAcademicYear As System.Windows.Forms.MaskedTextBox
    Friend WithEvents cmbSchoolAttended As System.Windows.Forms.ComboBox
    Friend WithEvents cmbConcurrentCollege As System.Windows.Forms.ComboBox

End Class
