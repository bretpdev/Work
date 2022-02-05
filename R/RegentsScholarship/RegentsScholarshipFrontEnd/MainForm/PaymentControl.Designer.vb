<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PaymentControl
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
        Me.components = New System.ComponentModel.Container
        Me.cmbCollege = New System.Windows.Forms.ComboBox
        Me.PaymentBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.cmbSemester = New System.Windows.Forms.ComboBox
        Me.chkGpaOverride = New System.Windows.Forms.CheckBox
        Me.chkCreditsOverride = New System.Windows.Forms.CheckBox
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.txtStatus = New System.Windows.Forms.TextBox
        Me.txtStatusDate = New System.Windows.Forms.TextBox
        Me.chkDelete = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtYear = New System.Windows.Forms.TextBox
        Me.txtCredits = New System.Windows.Forms.TextBox
        Me.txtGpa = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.dtpScheduleReceived = New System.Windows.Forms.DateTimePicker
        Me.txtSequenceNo = New System.Windows.Forms.TextBox
        Me.chkSemesterOverride = New System.Windows.Forms.CheckBox
        Me.dtpGradesReceived = New RegentsScholarshipFrontEnd.NullableDateTimePicker
        CType(Me.PaymentBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbCollege
        '
        Me.cmbCollege.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "College", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cmbCollege.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCollege.FormattingEnabled = True
        Me.cmbCollege.Location = New System.Drawing.Point(3, 20)
        Me.cmbCollege.Name = "cmbCollege"
        Me.cmbCollege.Size = New System.Drawing.Size(225, 21)
        Me.cmbCollege.TabIndex = 0
        '
        'PaymentBindingSource
        '
        Me.PaymentBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Payment)
        '
        'cmbSemester
        '
        Me.cmbSemester.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "Semester", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cmbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSemester.FormattingEnabled = True
        Me.cmbSemester.Location = New System.Drawing.Point(235, 20)
        Me.cmbSemester.Name = "cmbSemester"
        Me.cmbSemester.Size = New System.Drawing.Size(121, 21)
        Me.cmbSemester.TabIndex = 1
        '
        'chkGpaOverride
        '
        Me.chkGpaOverride.AutoSize = True
        Me.chkGpaOverride.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.PaymentBindingSource, "GpaRequirementIsOverridden", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkGpaOverride.Location = New System.Drawing.Point(510, 37)
        Me.chkGpaOverride.Name = "chkGpaOverride"
        Me.chkGpaOverride.Size = New System.Drawing.Size(89, 17)
        Me.chkGpaOverride.TabIndex = 13
        Me.chkGpaOverride.Text = "GPA override"
        Me.chkGpaOverride.UseVisualStyleBackColor = True
        '
        'chkCreditsOverride
        '
        Me.chkCreditsOverride.AutoSize = True
        Me.chkCreditsOverride.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.PaymentBindingSource, "CreditsRequirementIsOverridden", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkCreditsOverride.Location = New System.Drawing.Point(510, 21)
        Me.chkCreditsOverride.Name = "chkCreditsOverride"
        Me.chkCreditsOverride.Size = New System.Drawing.Size(99, 17)
        Me.chkCreditsOverride.TabIndex = 12
        Me.chkCreditsOverride.Text = "Credits override"
        Me.chkCreditsOverride.UseVisualStyleBackColor = True
        '
        'cmbType
        '
        Me.cmbType.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "Type", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(3, 72)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(92, 21)
        Me.cmbType.TabIndex = 5
        '
        'txtStatus
        '
        Me.txtStatus.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "Status", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtStatus.Location = New System.Drawing.Point(159, 72)
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.ReadOnly = True
        Me.txtStatus.Size = New System.Drawing.Size(86, 20)
        Me.txtStatus.TabIndex = 7
        '
        'txtStatusDate
        '
        Me.txtStatusDate.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "StatusDate", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, Nothing, "d"))
        Me.txtStatusDate.Location = New System.Drawing.Point(251, 72)
        Me.txtStatusDate.Name = "txtStatusDate"
        Me.txtStatusDate.ReadOnly = True
        Me.txtStatusDate.Size = New System.Drawing.Size(73, 20)
        Me.txtStatusDate.TabIndex = 8
        Me.txtStatusDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkDelete
        '
        Me.chkDelete.AutoSize = True
        Me.chkDelete.Location = New System.Drawing.Point(555, 68)
        Me.chkDelete.Name = "chkDelete"
        Me.chkDelete.Size = New System.Drawing.Size(67, 30)
        Me.chkDelete.TabIndex = 14
        Me.chkDelete.Text = "Delete" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Payment"
        Me.chkDelete.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "College"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(232, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Semester"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(360, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Year"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(400, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Credits"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(445, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(29, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "GPA"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(0, 57)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(75, 13)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "Payment Type"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(98, 57)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(43, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Amount"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(156, 57)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Status"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(248, 57)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(63, 13)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Status Date"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(327, 57)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(101, 13)
        Me.Label10.TabIndex = 23
        Me.Label10.Text = "Schedule Received"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(428, 57)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(90, 13)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "Grades Received"
        '
        'txtYear
        '
        Me.txtYear.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "Year", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtYear.Location = New System.Drawing.Point(363, 20)
        Me.txtYear.MaxLength = 4
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Size = New System.Drawing.Size(34, 20)
        Me.txtYear.TabIndex = 2
        Me.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCredits
        '
        Me.txtCredits.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "Credits", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtCredits.Location = New System.Drawing.Point(403, 20)
        Me.txtCredits.MaxLength = 5
        Me.txtCredits.Name = "txtCredits"
        Me.txtCredits.Size = New System.Drawing.Size(31, 20)
        Me.txtCredits.TabIndex = 3
        Me.txtCredits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtGpa
        '
        Me.txtGpa.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "Gpa", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtGpa.Location = New System.Drawing.Point(440, 21)
        Me.txtGpa.MaxLength = 5
        Me.txtGpa.Name = "txtGpa"
        Me.txtGpa.Size = New System.Drawing.Size(37, 20)
        Me.txtGpa.TabIndex = 4
        Me.txtGpa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAmount
        '
        Me.txtAmount.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "Amount", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtAmount.Location = New System.Drawing.Point(101, 72)
        Me.txtAmount.MaxLength = 8
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(52, 20)
        Me.txtAmount.TabIndex = 6
        Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'dtpScheduleReceived
        '
        Me.dtpScheduleReceived.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.PaymentBindingSource, "ScheduleReceivedDate", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.dtpScheduleReceived.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpScheduleReceived.Location = New System.Drawing.Point(330, 72)
        Me.dtpScheduleReceived.Name = "dtpScheduleReceived"
        Me.dtpScheduleReceived.Size = New System.Drawing.Size(95, 20)
        Me.dtpScheduleReceived.TabIndex = 9
        '
        'txtSequenceNo
        '
        Me.txtSequenceNo.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.PaymentBindingSource, "SequenceNo", True))
        Me.txtSequenceNo.Location = New System.Drawing.Point(555, 42)
        Me.txtSequenceNo.Name = "txtSequenceNo"
        Me.txtSequenceNo.Size = New System.Drawing.Size(0, 20)
        Me.txtSequenceNo.TabIndex = 26
        '
        'chkSemesterOverride
        '
        Me.chkSemesterOverride.AutoSize = True
        Me.chkSemesterOverride.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.PaymentBindingSource, "SemesterUniquenessIsOverridden", True))
        Me.chkSemesterOverride.Location = New System.Drawing.Point(510, 4)
        Me.chkSemesterOverride.Name = "chkSemesterOverride"
        Me.chkSemesterOverride.Size = New System.Drawing.Size(111, 17)
        Me.chkSemesterOverride.TabIndex = 11
        Me.chkSemesterOverride.Text = "Semester override"
        Me.chkSemesterOverride.UseVisualStyleBackColor = True
        '
        'dtpGradesReceived
        '
        Me.dtpGradesReceived.CustomFormat = ""
        Me.dtpGradesReceived.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.PaymentBindingSource, "GradesReceivedDate", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.dtpGradesReceived.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpGradesReceived.FormatAsString = "M/d/yyyy"
        Me.dtpGradesReceived.Location = New System.Drawing.Point(431, 72)
        Me.dtpGradesReceived.Name = "dtpGradesReceived"
        Me.dtpGradesReceived.NullValue = " "
        Me.dtpGradesReceived.Size = New System.Drawing.Size(95, 20)
        Me.dtpGradesReceived.TabIndex = 10
        Me.dtpGradesReceived.Value = Nothing
        '
        'PaymentControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Controls.Add(Me.chkSemesterOverride)
        Me.Controls.Add(Me.txtSequenceNo)
        Me.Controls.Add(Me.dtpScheduleReceived)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.txtGpa)
        Me.Controls.Add(Me.txtCredits)
        Me.Controls.Add(Me.txtYear)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkDelete)
        Me.Controls.Add(Me.dtpGradesReceived)
        Me.Controls.Add(Me.txtStatusDate)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.cmbType)
        Me.Controls.Add(Me.chkCreditsOverride)
        Me.Controls.Add(Me.chkGpaOverride)
        Me.Controls.Add(Me.cmbSemester)
        Me.Controls.Add(Me.cmbCollege)
        Me.Name = "PaymentControl"
        Me.Size = New System.Drawing.Size(624, 110)
        CType(Me.PaymentBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbCollege As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSemester As System.Windows.Forms.ComboBox
    Friend WithEvents chkGpaOverride As System.Windows.Forms.CheckBox
    Friend WithEvents chkCreditsOverride As System.Windows.Forms.CheckBox
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents txtStatus As System.Windows.Forms.TextBox
    Friend WithEvents txtStatusDate As System.Windows.Forms.TextBox
    Friend WithEvents dtpGradesReceived As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents chkDelete As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents PaymentBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents txtYear As System.Windows.Forms.TextBox
    Friend WithEvents txtCredits As System.Windows.Forms.TextBox
    Friend WithEvents txtGpa As System.Windows.Forms.TextBox
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents dtpScheduleReceived As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtSequenceNo As System.Windows.Forms.TextBox
    Friend WithEvents chkSemesterOverride As System.Windows.Forms.CheckBox

End Class
