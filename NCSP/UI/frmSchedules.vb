Imports System
Imports System.IO

Public Class frmSchedules
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
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnApproved As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btnLink As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnGrades As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnStudentInfo As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtHoursPaid As System.Windows.Forms.TextBox
    Friend WithEvents cmbYear As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSemester As System.Windows.Forms.ComboBox
    Friend WithEvents txtHoursEnrolled As System.Windows.Forms.TextBox
    Friend WithEvents txtCreated As System.Windows.Forms.TextBox
    Friend WithEvents txtSchHoursRemaining As System.Windows.Forms.TextBox
    Friend WithEvents txtGPA As System.Windows.Forms.TextBox
    Friend WithEvents txtHoursCompleted As System.Windows.Forms.TextBox
    Friend WithEvents btnFirst As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents btnLast As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents cmbInstitution As System.Windows.Forms.ComboBox
    Friend WithEvents chkLFTOverride As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbCommunications As System.Windows.Forms.ComboBox
    Friend WithEvents btnTransHistory As System.Windows.Forms.Button
    Friend WithEvents txtNumber As System.Windows.Forms.TextBox
    Friend WithEvents hdrHeader As NCSP.FormHeader
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSchedules))
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnApproved = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkLFTOverride = New System.Windows.Forms.CheckBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtHoursPaid = New System.Windows.Forms.TextBox
        Me.cmbInstitution = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbYear = New System.Windows.Forms.ComboBox
        Me.cmbSemester = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtHoursEnrolled = New System.Windows.Forms.TextBox
        Me.txtCreated = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtSchHoursRemaining = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtGPA = New System.Windows.Forms.TextBox
        Me.txtHoursCompleted = New System.Windows.Forms.TextBox
        Me.btnStudentInfo = New System.Windows.Forms.Button
        Me.btnTransHistory = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.btnLink = New System.Windows.Forms.Button
        Me.btnView = New System.Windows.Forms.Button
        Me.btnGrades = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnFirst = New System.Windows.Forms.Button
        Me.btnPrev = New System.Windows.Forms.Button
        Me.btnLast = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.cmbCommunications = New System.Windows.Forms.ComboBox
        Me.hdrHeader = New NCSP.FormHeader
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(824, 112)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(144, 23)
        Me.btnSave.TabIndex = 24
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save"
        '
        'btnApproved
        '
        Me.btnApproved.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApproved.Location = New System.Drawing.Point(824, 144)
        Me.btnApproved.Name = "btnApproved"
        Me.btnApproved.Size = New System.Drawing.Size(144, 23)
        Me.btnApproved.TabIndex = 25
        Me.btnApproved.TabStop = False
        Me.btnApproved.Text = "Approved"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtNumber)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.chkLFTOverride)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtHoursPaid)
        Me.GroupBox1.Controls.Add(Me.cmbInstitution)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.cmbYear)
        Me.GroupBox1.Controls.Add(Me.cmbSemester)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtHoursEnrolled)
        Me.GroupBox1.Controls.Add(Me.txtCreated)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.txtSchHoursRemaining)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(16, 72)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(784, 240)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Schedule Information"
        '
        'txtNumber
        '
        Me.txtNumber.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtNumber.Cursor = System.Windows.Forms.Cursors.No
        Me.txtNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumber.Location = New System.Drawing.Point(144, 24)
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.ReadOnly = True
        Me.txtNumber.Size = New System.Drawing.Size(144, 20)
        Me.txtNumber.TabIndex = 69
        Me.txtNumber.TabStop = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 20)
        Me.Label2.TabIndex = 68
        Me.Label2.Text = "Number"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(624, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 20)
        Me.Label1.TabIndex = 67
        Me.Label1.Text = "LFT Override"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkLFTOverride
        '
        Me.chkLFTOverride.Location = New System.Drawing.Point(752, 24)
        Me.chkLFTOverride.Name = "chkLFTOverride"
        Me.chkLFTOverride.Size = New System.Drawing.Size(16, 24)
        Me.chkLFTOverride.TabIndex = 66
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(16, 96)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 20)
        Me.Label7.TabIndex = 64
        Me.Label7.Text = "Hours Paid"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtHoursPaid
        '
        Me.txtHoursPaid.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtHoursPaid.Cursor = System.Windows.Forms.Cursors.No
        Me.txtHoursPaid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHoursPaid.Location = New System.Drawing.Point(144, 96)
        Me.txtHoursPaid.Name = "txtHoursPaid"
        Me.txtHoursPaid.ReadOnly = True
        Me.txtHoursPaid.Size = New System.Drawing.Size(144, 20)
        Me.txtHoursPaid.TabIndex = 65
        Me.txtHoursPaid.TabStop = False
        '
        'cmbInstitution
        '
        Me.cmbInstitution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbInstitution.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbInstitution.Items.AddRange(New Object() {""})
        Me.cmbInstitution.Location = New System.Drawing.Point(144, 128)
        Me.cmbInstitution.Name = "cmbInstitution"
        Me.cmbInstitution.Size = New System.Drawing.Size(288, 21)
        Me.cmbInstitution.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(16, 128)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(120, 20)
        Me.Label12.TabIndex = 51
        Me.Label12.Text = "Institution"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 176)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 20)
        Me.Label8.TabIndex = 55
        Me.Label8.Text = "Year"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbYear
        '
        Me.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbYear.Items.AddRange(New Object() {""})
        Me.cmbYear.Location = New System.Drawing.Point(144, 176)
        Me.cmbYear.Name = "cmbYear"
        Me.cmbYear.Size = New System.Drawing.Size(144, 21)
        Me.cmbYear.TabIndex = 2
        '
        'cmbSemester
        '
        Me.cmbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSemester.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSemester.Items.AddRange(New Object() {"", "Fall", "Winter", "Spring", "Summer"})
        Me.cmbSemester.Location = New System.Drawing.Point(144, 152)
        Me.cmbSemester.Name = "cmbSemester"
        Me.cmbSemester.Size = New System.Drawing.Size(144, 21)
        Me.cmbSemester.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(16, 152)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(120, 20)
        Me.Label9.TabIndex = 53
        Me.Label9.Text = "Semester"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(16, 200)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(120, 20)
        Me.Label10.TabIndex = 57
        Me.Label10.Text = "Hours Enrolled"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtHoursEnrolled
        '
        Me.txtHoursEnrolled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHoursEnrolled.Location = New System.Drawing.Point(144, 200)
        Me.txtHoursEnrolled.Name = "txtHoursEnrolled"
        Me.txtHoursEnrolled.Size = New System.Drawing.Size(144, 20)
        Me.txtHoursEnrolled.TabIndex = 3
        '
        'txtCreated
        '
        Me.txtCreated.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtCreated.Cursor = System.Windows.Forms.Cursors.No
        Me.txtCreated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreated.Location = New System.Drawing.Point(144, 48)
        Me.txtCreated.Name = "txtCreated"
        Me.txtCreated.ReadOnly = True
        Me.txtCreated.Size = New System.Drawing.Size(144, 20)
        Me.txtCreated.TabIndex = 44
        Me.txtCreated.TabStop = False
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(16, 48)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(120, 20)
        Me.Label18.TabIndex = 43
        Me.Label18.Text = "Date Created"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSchHoursRemaining
        '
        Me.txtSchHoursRemaining.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtSchHoursRemaining.Cursor = System.Windows.Forms.Cursors.No
        Me.txtSchHoursRemaining.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSchHoursRemaining.Location = New System.Drawing.Point(144, 72)
        Me.txtSchHoursRemaining.Name = "txtSchHoursRemaining"
        Me.txtSchHoursRemaining.ReadOnly = True
        Me.txtSchHoursRemaining.Size = New System.Drawing.Size(144, 20)
        Me.txtSchHoursRemaining.TabIndex = 50
        Me.txtSchHoursRemaining.TabStop = False
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(16, 72)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(120, 20)
        Me.Label11.TabIndex = 49
        Me.Label11.Text = "Hours Remaining"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.txtGPA)
        Me.GroupBox2.Controls.Add(Me.txtHoursCompleted)
        Me.GroupBox2.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(16, 320)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(784, 88)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Hours and Grades"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(16, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(120, 20)
        Me.Label5.TabIndex = 59
        Me.Label5.Text = "Hours Completed"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(120, 20)
        Me.Label6.TabIndex = 61
        Me.Label6.Text = "Semester GPA"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtGPA
        '
        Me.txtGPA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGPA.Location = New System.Drawing.Point(144, 48)
        Me.txtGPA.Name = "txtGPA"
        Me.txtGPA.Size = New System.Drawing.Size(144, 20)
        Me.txtGPA.TabIndex = 1
        '
        'txtHoursCompleted
        '
        Me.txtHoursCompleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHoursCompleted.Location = New System.Drawing.Point(144, 24)
        Me.txtHoursCompleted.Name = "txtHoursCompleted"
        Me.txtHoursCompleted.Size = New System.Drawing.Size(144, 20)
        Me.txtHoursCompleted.TabIndex = 0
        '
        'btnStudentInfo
        '
        Me.btnStudentInfo.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStudentInfo.Location = New System.Drawing.Point(824, 488)
        Me.btnStudentInfo.Name = "btnStudentInfo"
        Me.btnStudentInfo.Size = New System.Drawing.Size(144, 23)
        Me.btnStudentInfo.TabIndex = 40
        Me.btnStudentInfo.TabStop = False
        Me.btnStudentInfo.Text = "Student Information"
        '
        'btnTransHistory
        '
        Me.btnTransHistory.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTransHistory.Location = New System.Drawing.Point(824, 520)
        Me.btnTransHistory.Name = "btnTransHistory"
        Me.btnTransHistory.Size = New System.Drawing.Size(144, 23)
        Me.btnTransHistory.TabIndex = 52
        Me.btnTransHistory.TabStop = False
        Me.btnTransHistory.Text = "Transaction History"
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
        Me.Label33.Text = "Schedules and Grades"
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
        'btnLink
        '
        Me.btnLink.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLink.Location = New System.Drawing.Point(824, 208)
        Me.btnLink.Name = "btnLink"
        Me.btnLink.Size = New System.Drawing.Size(144, 23)
        Me.btnLink.TabIndex = 67
        Me.btnLink.TabStop = False
        Me.btnLink.Text = "Link Documents"
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Location = New System.Drawing.Point(824, 240)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(144, 23)
        Me.btnView.TabIndex = 66
        Me.btnView.TabStop = False
        Me.btnView.Text = "View Documents"
        '
        'btnGrades
        '
        Me.btnGrades.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGrades.Location = New System.Drawing.Point(824, 176)
        Me.btnGrades.Name = "btnGrades"
        Me.btnGrades.Size = New System.Drawing.Size(144, 23)
        Me.btnGrades.TabIndex = 64
        Me.btnGrades.TabStop = False
        Me.btnGrades.Text = "Update Grades"
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(824, 80)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(144, 23)
        Me.btnAdd.TabIndex = 68
        Me.btnAdd.TabStop = False
        Me.btnAdd.Text = "Add Schedule"
        '
        'btnFirst
        '
        Me.btnFirst.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFirst.Image = CType(resources.GetObject("btnFirst.Image"), System.Drawing.Image)
        Me.btnFirst.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnFirst.Location = New System.Drawing.Point(16, 584)
        Me.btnFirst.Name = "btnFirst"
        Me.btnFirst.Size = New System.Drawing.Size(24, 24)
        Me.btnFirst.TabIndex = 69
        '
        'btnPrev
        '
        Me.btnPrev.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrev.Image = CType(resources.GetObject("btnPrev.Image"), System.Drawing.Image)
        Me.btnPrev.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnPrev.Location = New System.Drawing.Point(40, 584)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(24, 24)
        Me.btnPrev.TabIndex = 70
        '
        'btnLast
        '
        Me.btnLast.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLast.Image = CType(resources.GetObject("btnLast.Image"), System.Drawing.Image)
        Me.btnLast.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnLast.Location = New System.Drawing.Point(776, 584)
        Me.btnLast.Name = "btnLast"
        Me.btnLast.Size = New System.Drawing.Size(24, 24)
        Me.btnLast.TabIndex = 71
        '
        'btnNext
        '
        Me.btnNext.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNext.Image = CType(resources.GetObject("btnNext.Image"), System.Drawing.Image)
        Me.btnNext.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.btnNext.Location = New System.Drawing.Point(752, 584)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(24, 24)
        Me.btnNext.TabIndex = 72
        '
        'GroupBox3
        '
        Me.GroupBox3.Location = New System.Drawing.Point(16, 576)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(784, 1)
        Me.GroupBox3.TabIndex = 76
        Me.GroupBox3.TabStop = False
        '
        'cmbCommunications
        '
        Me.cmbCommunications.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCommunications.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCommunications.Items.AddRange(New Object() {"Communications", "Add", "All", "Call", "Letter", "Fax", "Email", "Account Maintenance"})
        Me.cmbCommunications.Location = New System.Drawing.Point(824, 552)
        Me.cmbCommunications.Name = "cmbCommunications"
        Me.cmbCommunications.Size = New System.Drawing.Size(144, 24)
        Me.cmbCommunications.TabIndex = 77
        Me.cmbCommunications.TabStop = False
        '
        'hdrHeader
        '
        Me.hdrHeader.Location = New System.Drawing.Point(-1, -1)
        Me.hdrHeader.Name = "hdrHeader"
        Me.hdrHeader.Size = New System.Drawing.Size(997, 68)
        Me.hdrHeader.TabIndex = 78
        '
        'frmSchedules
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(992, 653)
        Me.Controls.Add(Me.hdrHeader)
        Me.Controls.Add(Me.cmbCommunications)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnLast)
        Me.Controls.Add(Me.btnPrev)
        Me.Controls.Add(Me.btnFirst)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.btnStudentInfo)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnApproved)
        Me.Controls.Add(Me.btnTransHistory)
        Me.Controls.Add(Me.btnGrades)
        Me.Controls.Add(Me.btnLink)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1000, 680)
        Me.Name = "frmSchedules"
        Me.Text = "Schedules and Grades"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private sqlCmd As New SqlClient.SqlCommand
    Private sqlRdr As SqlClient.SqlDataReader
    Private maxSched As Integer
    Private Inst As String
    Private BegHrsEnrolled As Double
    Private BegSemester As String
    Private BegYear As String
    Private BegInst As String

#Region "Form Functions"
    'load form
    Private Sub frmSchedules_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'add schools to list
        sqlCmd.CommandText = "SELECT DISTINCT InstLong FROM Inst ORDER BY InstLong"
        sqlRdr = sqlCmd.ExecuteReader
        While sqlRdr.Read
            cmbInstitution.Items.Add(sqlRdr("InstLong"))
        End While
        sqlRdr.Close()

        'add years to list
        For i = Val(Year(Today)) + 1 To 1999 Step -1
            cmbYear.Items.Add(Trim(Str(i)))
        Next i

        'set text of communications drop down
        cmbCommunications.Text = "Communications"

        'fill student info in header
        frmStudentsForm.StuInfo(hdrHeader.txtAccountID.Text, hdrHeader.txtStudentName.Text, hdrHeader.txtStatus.Text, hdrHeader.txtBalance.Text, hdrHeader.txtHoursRemaining.Text, hdrHeader.txtPaidSchedules.Text)

        'maximum schedule number
        sqlCmd.CommandText = "SELECT MAX(SchedID) AS SchedID FROM Schedule WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        maxSched = sqlRdr("SchedID")
        sqlRdr.Close()
        dbConnection.Close()

        'fill schedule info
        FillFields(maxSched)
    End Sub

    'close form
    Private Sub frmSchedules_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        sqlCmd.CommandText = "UPDATE Schedule SET LockedBy = '' WHERE LockedBy = '" & UserID & "'"
        sqlCmd.ExecuteNonQuery()
        dbConnection.Close()
    End Sub
#End Region

#Region "Processing Buttons"
    'add new schedule
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim Warning As String = ""
        Dim currHrs As Double
        Dim currGPA As Double

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'determine if a new schedule can be added if there is already more than one
        If maxSched >= 1 Then

            'get hours completed and GPA from most recent schedule
            sqlCmd.CommandText = "SELECT CredHrComp, SemesterGPA FROM Schedule WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A' AND SchedID = " & maxSched
            sqlRdr = sqlCmd.ExecuteReader
            sqlRdr.Read()

            'set default numberic values if values are missing
            If IsDBNull(sqlRdr("CredHrComp")) Then currHrs = -1
            If IsDBNull(sqlRdr("SemesterGPA")) Then currGPA = -1

            sqlRdr.Close()

            'warn the user if the hours completed or semester GPA is blank
            If currHrs = -1 Or currGPA = -1 Then
                Warning = "The hours completed and/or semester GPA for the current schedule is blank.  The student must submit his or her grades for the semester before more schedules can be added and paid."
                'warn the user and do not add a new schedule if there is only one and the transcript received date is blank
            ElseIf Val(hdrHeader.txtPaidSchedules.Text) >= 4 Then
                MsgBox("The student has been paid for the maximum number of semesters (4).", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Maximum Number of Semesters Paid")
                If MsgBox("Click OK to add a 5th semester or click Cancel to quit.", MsgBoxStyle.Critical + MsgBoxStyle.OkCancel, "Allow 5 semesters") = MsgBoxResult.Cancel Then
                    dbConnection.Close()
                    Exit Sub
                End If
            End If

        ElseIf frmStudentsForm.TransRevdDt = "" Then
            Warning = "The associate's degree transcript received date on the student information screen is blank.  The student must submit his or her associates degree transcript before additional schedules may be added and paid."
        End If

        'warn the user if a schedule can't be added
        If Warning <> "" Then
            MsgBox(Warning, MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            dbConnection.Close()

            'add the schedule
        Else
            maxSched = maxSched + 1

            sqlCmd.CommandText = "INSERT INTO Schedule (AcctID, SchedID, SchedHrRem, LastUpdateUser) VALUES ('" & hdrHeader.txtAccountID.Text & "', " & maxSched & ", " & CDbl(hdrHeader.txtHoursRemaining.Text) & ", '" & UserID & "')"
            sqlCmd.ExecuteNonQuery()

            dbConnection.Close()
            FillFields(maxSched)

            Directory.CreateDirectory(DocPath & hdrHeader.txtAccountID.Text & "\Schedules\" & maxSched)

            MsgBox("Schedule added.", MsgBoxStyle.Information, "New Century Scholarship Program")
        End If
    End Sub

    'save changes
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'warn the user if the schedule already exists for the semester and year
        If SemesterExists() Then Exit Sub

        'warn the user if the hours enrolled have been changed and the schedule has already been approved
        If txtHoursEnrolled.Text <> "" Then
            If hdrHeader.txtStatus.Text <> "Adding" And (CDbl(txtHoursEnrolled.Text) <> BegHrsEnrolled Or cmbSemester.Text <> BegSemester Or cmbYear.Text <> BegYear) Then
                MsgBox("A payment has already been processed for this schedule but the hours enrolled has been changed.  If you change the hours enrolled, you must click the Approved button to process the change.", MsgBoxStyle.Information, "New Century Scholarship Program")
                Exit Sub
            End If
        End If

        SaveRecord(hdrHeader.txtStatus.Text)
        MsgBox("Schedule information saved.", MsgBoxStyle.Information, "New Century Scholarship Program")
    End Sub

    Private Sub btnApproved_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApproved.Click
        Dim ComDt As String = String.Empty
        Dim TransID As String = String.Empty
        Dim TransHrs As Double = 0
        Dim TransAmt As Double = 0
        Dim Zip As String = String.Empty

        'warn user if required information is missing
        If cmbInstitution.SelectedItem = "" Or cmbSemester.SelectedItem = "" Or cmbYear.SelectedItem = "" Or txtHoursEnrolled.Text = "" Then
            MsgBox("The institution, semester, year, and hours enrolled must be entered before the schedule can be approved.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub

            'warn the user if the schedule already exists for the semester and year
        ElseIf SemesterExists() Then
            Exit Sub

            'warn user if there are no changes to approve
        ElseIf (CDbl(txtHoursEnrolled.Text) = BegHrsEnrolled And cmbSemester.Text = BegSemester And cmbYear.Text = BegYear) And cmbInstitution.SelectedItem = BegInst And hdrHeader.txtStatus.Text <> "Adding" Then
            If MsgBox("The number of hours enrolled, semester or year has not changed so no payments or adjustments will be processed.  Do you want to save your changes?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "New Century Scholarship Program") = MsgBoxResult.Yes Then
                SaveRecord(hdrHeader.txtStatus.Text)
                MsgBox("Schedule information saved.", MsgBoxStyle.Information, "New Century Scholarship Program")
            End If
            Exit Sub
        End If

        sqlCmd.Connection = dbConnection

        'get institution ID
        Inst = getInstId(cmbInstitution.SelectedItem)

        ComDt = Now()
        TransID = Format(DateValue(ComDt), "MMddyyyy") & Format(TimeValue(ComDt), "HHmmss")

        If hdrHeader.txtStatus.Text = "Adding" Or hdrHeader.txtStatus.Text = "Unpaid" Then
            If HasMetTwoSemesterRule() Then
                If MsgBox("The student has already been paid for two semesters for the academic year.  Click OK to save the schedule as unpaid.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                'update the schedule status and save any changes
                SaveRecord("Unpaid")

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, ComDt, True, False, "Account Maintenance", "schedule for " & GetSemYr() & " approved, student has already been paid for two semesters for the academic year, schedule unpaid", "Unpaid Schedule Added")

                'update schedule and print LFT letter if hours enrolled < 12
            ElseIf CDbl(txtHoursEnrolled.Text) < 12 And CDbl(txtHoursEnrolled.Text) < CDbl(hdrHeader.txtHoursRemaining.Text) And chkLFTOverride.Checked = False Then
                'warn the user of action to be taken and abort if canceled
                If MsgBox("The student is enrolled less than full-time.  Click OK to send a NCSP ENROLLED LESS THAN FULL TIME letter to the student or click Cancel to quit.", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                'update the schedule status and save any changes
                SaveRecord("Less Than Full-Time")

                'get account information
                dbConnection.Open()
                sqlCmd.CommandText = "SELECT * FROM Account WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A'"
                sqlRdr = sqlCmd.ExecuteReader
                sqlRdr.Read()

                'format zip
                If Len(sqlRdr("Zip")) > 5 Then Zip = CLng(sqlRdr("Zip")).ToString("00000-0000") Else Zip = CStr(sqlRdr("Zip"))

                'print letter
                FileOpen(1, "T:\NCSPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
                WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Semester", "School", "Hours", "StaticCurrentDate")
                WriteLine(1, sqlRdr("AcctID"), sqlRdr("FName"), sqlRdr("LName"), sqlRdr("Add1"), sqlRdr("Add2"), sqlRdr("City"), sqlRdr("ST"), Zip, cmbSemester.SelectedItem, cmbInstitution.SelectedItem, txtHoursEnrolled.Text, Date.Now.ToShortDateString())
                FileClose(1)

                sqlRdr.Close()
                dbConnection.Close()

                PrintDoc("NCSLSHLFTM", hdrHeader.txtAccountID.Text & "\Communications", TransID)

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, ComDt, True, False, "Account Maintenance", "schedule for " & GetSemYr() & " approved, hours enrolled less than full-time, NCSP ENROLLED LESS THAN FULL TIME letter sent", "NCSP ENROLLED LESS THAN FULL TIME Letter Sent")
            Else
                'warn the user of action to be taken and abort if canceled
                If MsgBox("The schedule has been approved and is ready to be paid.  Click OK to create a payment transaction for the schedule or click Cancel to quit.", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                'add transaction record and update account balance and hours remaining
                TransHrs = LesserOf(txtHoursEnrolled.Text, hdrHeader.txtHoursRemaining.Text)
                TransAmt = GetScholarshipAmount((cmbYear.SelectedItem))
                'TransAmt = GetTuition(Inst, cmbSemester.SelectedItem, CInt(cmbYear.SelectedItem), TransHrs)
                If TransAmt = 0 Then Exit Sub
                txtSchHoursRemaining.Text = (CInt(hdrHeader.txtHoursRemaining.Text) - CInt(txtHoursEnrolled.Text)).ToString()
                AddnSave("Payment", TransHrs, TransAmt, "Payment Pending")

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "schedule for " & GetSemYr() & " approved, payment for " & FormatCurrency(TransAmt, 2, TriState.True, TriState.True) & " entered", "Schedule Approved")
            End If

        ElseIf hdrHeader.txtStatus.Text = "Less Than Full-Time" Then
            'warn the user no action is necessary if the hours enrolled have changed but the schedule is still below full time
            If CDbl(txtHoursEnrolled.Text) < 12 And CDbl(txtHoursEnrolled.Text) < CDbl(hdrHeader.txtHoursRemaining.Text) And chkLFTOverride.Checked = False Then
                If MsgBox("The hours enrolled has changed but is still less than full-time so no adjustment will be necessary.  Do you want to save your changes?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "New Century Scholarship Program") = MsgBoxResult.Yes Then
                    SaveRecord(hdrHeader.txtStatus.Text)
                    MsgBox("Schedule information saved.", MsgBoxStyle.Information, "New Century Scholarship Program")
                End If
                Exit Sub

                'warn the user no action is necessary if the hours enrolled have changed but there are no hours of eligibility left to pay
            ElseIf CDbl(hdrHeader.txtHoursRemaining.Text) = 0 Then
                If MsgBox("The hours enrolled has been increased but there are no hours of eligibility remaining so no further funds will be awarded.  Do you want to save your changes?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "New Century Scholarship Program") = MsgBoxResult.Yes Then
                    SaveRecord(hdrHeader.txtStatus.Text)
                    MsgBox("Schedule information saved.", MsgBoxStyle.Information, "New Century Scholarship Program")
                End If
                Exit Sub

                'process an adjustment if the hours enrolled has increased above full-time
            Else
                If MsgBox("The hours enrolled has been increased so the student may be paid for the semester.  Click OK to save your changes and create a payment.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                'add transaction record and update account balance and hours remaining
                TransHrs = LesserOf(txtHoursEnrolled.Text, hdrHeader.txtHoursRemaining.Text)
                TransAmt = GetScholarshipAmount((cmbYear.SelectedItem))
                'TransAmt = GetTuition(Inst, cmbSemester.SelectedItem, CInt(cmbYear.SelectedItem), TransHrs)
                If TransAmt = 0 Then Exit Sub
                AddnSave("Payment", TransHrs, TransAmt, "Payment Pending")

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "hours added, payment for " & GetSemYr() & " for " & FormatCurrency(TransAmt, 2, TriState.True, TriState.True) & " entered", "Hours Added, Payment Entered")
            End If

        Else
            'process an adjustment if the hours enrolled has decreased above full-time
            '?? close for LFT?
            If CDbl(txtHoursEnrolled.Text) < 12 And CDbl(txtHoursEnrolled.Text) - CDbl(txtHoursPaid.Text) < CDbl(hdrHeader.txtHoursRemaining.Text) And chkLFTOverride.Checked = False Then
                If MsgBox("A payment has already been processed for this schedule but the hours enrolled are now below full-time.  Click OK to save your changes and process the change.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                TransHrs = CDbl(txtHoursPaid.Text)
                TransAmt = GetScholarshipAmount((cmbYear.SelectedItem))
                'TransAmt = GetTuition(Inst, cmbSemester.SelectedItem, CInt(cmbYear.SelectedItem), TransHrs)
                If TransAmt = 0 Then Exit Sub
                AddnSave("Adjustment", 0 - TransHrs, 0 - TransAmt, "Less Than Full-Time")

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "hours dropped, student attending less than full-time", "Hours Dropped Below Full-Time")
            ElseIf CDbl(txtHoursEnrolled.Text) < BegHrsEnrolled Then
                'warn the user no action is necessary as the hours enrolled have decreased but there are still no hours remaining
                If CDbl(hdrHeader.txtHoursRemaining.Text) = 0 And CDbl(txtHoursEnrolled.Text) >= CDbl(txtHoursPaid.Text) Then
                    If MsgBox("The hours enrolled has been decreased but there are no hours of eligibility remaining so no adjustment will be necessary.  Do you want to save your changes?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "New Century Scholarship Program") = MsgBoxResult.Yes Then
                        SaveRecord(hdrHeader.txtStatus.Text)
                        MsgBox("Schedule information saved.", MsgBoxStyle.Information, "New Century Scholarship Program")
                    End If
                    Exit Sub

                    'process an adjustment if the hours enrolled has decreased
                Else
                    If MsgBox("A payment has already been processed for this schedule but the hours enrolled has been decreased so the remaining hours will be increased.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                    'add transaction record and update account balance and hours remaining
                    TransHrs = CDbl(txtHoursEnrolled.Text) - LesserOf(CStr(BegHrsEnrolled), txtHoursPaid.Text)
                    AddnSave("Adjustment", TransHrs, 0, hdrHeader.txtStatus.Text)

                    'add communications record
                    AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "hours dropped", "Hours Dropped")
                End If
            ElseIf txtHoursEnrolled.Text > BegHrsEnrolled Then
                'warn the user no action is necessary if the hours enrolled have increased but there are no hours remaining
                If CDbl(hdrHeader.txtHoursRemaining.Text) = 0 Then
                    If MsgBox("The hours enrolled has been increased but there are no hours of eligibility remaining so the remaining hours will not be adjusted.  Do you want to save your changes?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "New Century Scholarship Program") = MsgBoxResult.Yes Then
                        SaveRecord(hdrHeader.txtStatus.Text)
                        MsgBox("Schedule information saved.", MsgBoxStyle.Information, "New Century Scholarship Program")
                    End If
                    Exit Sub

                    'process an adjustment if the hours enrolled has increased
                Else
                    If MsgBox("This schedule has already been approved but the hours enrolled has been increased.  Click OK to save your changes and reduce the hours remaining.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                    'add transaction record and update account balance and hours remaining
                    TransHrs = LesserOf(CStr(CDbl(txtHoursEnrolled.Text) - BegHrsEnrolled), hdrHeader.txtHoursRemaining.Text)
                    AddnSave("Payment", TransHrs, 0, hdrHeader.txtStatus.Text)

                    'add communications record
                    AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "hours added, payment for " & GetSemYr() & " for " & FormatCurrency(TransAmt, 2, TriState.True, TriState.True) & " entered", "Hours Added, Payment Entered")
                End If
            End If

            If cmbSemester.Text <> BegSemester Or cmbYear.Text <> BegYear Or BegInst <> cmbInstitution.SelectedItem Then
                'If the semester, year, or institution changed

                'update all transactions to match schedule changes
                sqlCmd.Connection = dbConnection
                sqlCmd.CommandText = "update [Transaction] set SchedInst = '" & Inst & "', SchedSemEnr = '" & cmbSemester.Text & "', SchedYrEnr = '" & cmbYear.Text & "' where SchedID = " & txtNumber.Text & " and AcctID = " & hdrHeader.txtAccountID.Text
                dbConnection.Open()
                sqlCmd.ExecuteNonQuery()
                dbConnection.Close()
                SaveRecord(hdrHeader.txtStatus.Text)
            End If
        End If

        'enable update grades button, reset beginning hours enrolled, and prompt user
        btnGrades.Enabled = True
        BegHrsEnrolled = CDbl(txtHoursEnrolled.Text)
        BegSemester = cmbSemester.Text
        BegYear = cmbYear.Text
        BegInst = cmbInstitution.SelectedItem
        MsgBox("Schedule information saved.", MsgBoxStyle.Information, "New Century Scholarship Program")
    End Sub

    Function sumTransactions(ByVal AcctID As Integer, ByVal SchedID As Integer) As Double
        Dim tot As Double
        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        sqlCmd.CommandText = "SELECT SUM(TranAmt) as TranAmt FROM [Transaction] where AcctID = " & AcctID & " and SchedID = " & SchedID
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        If sqlRdr.HasRows Then
            tot = CDbl(sqlRdr("TranAmt"))
        End If
        sqlRdr.Close()
        dbConnection.Close()
        Return tot
    End Function

    Function getInstId(ByVal inst As String) As String
        Dim str As String = String.Empty
        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        sqlCmd.CommandText = "SELECT InstID FROM Inst where InstLong = '" & inst & "'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        If sqlRdr.HasRows Then
            str = sqlRdr("InstID")
        End If
        sqlRdr.Close()
        dbConnection.Close()
        Return str
    End Function

    'review updated grades and hours completed information
    Private Sub btnGrades_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrades.Click
        'Dim GE30 As Boolean
        Dim LT30 As Boolean = False
        Dim EligLost As Boolean

        Dim ComDt As String = String.Empty
        Dim TransID As String = String.Empty
        Dim TransHrs As Double = 0

        Dim PrevSem As String = String.Empty
        Dim PrevSch As String = String.Empty
        Dim PrevGPA As String = String.Empty

        Dim Doc2Print As String = String.Empty

        If txtHoursCompleted.Text = "" Or txtGPA.Text = "" Then
            MsgBox("The hours completed and semester GPA information must be complete before grades can be updated.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        End If

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'get institution ID
        sqlCmd.CommandText = "SELECT InstID FROM Inst WHERE InstLong = '" & cmbInstitution.SelectedItem & "'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        Inst = sqlRdr("InstID")
        sqlRdr.Close()

        'determine if hours completed and GPA for previous schedule was entered
        sqlCmd.CommandText = "SELECT CredHrComp, SemesterGPA FROM Schedule WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A' AND SchedID = " & CInt(txtNumber.Text) & " - 1"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        If sqlRdr.HasRows Then
            'determine if GPA for previous schedule was >= 3.0 or warn the user if the GPA is missing for the previous schedule
            If IsDBNull(sqlRdr("CredHrComp")) Or IsDBNull(sqlRdr("SemesterGPA")) Then
                MsgBox("The hours completed and semester GPA information for the previous schedule must be complete before grades can be updated for the current schedule.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                sqlRdr.Close()
                dbConnection.Close()
                Exit Sub
            End If
        End If
        sqlRdr.Close()

        'determine if GPA for a previous non-withdrawn schedule was < 3.0
        sqlCmd.CommandText = "SELECT * FROM Schedule WHERE SemesterGPA < 3 AND AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A' AND SchedStat <> 'Withdrawn'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        If sqlRdr.HasRows Then
            LT30 = True
            PrevSem = sqlRdr("Semester")
            PrevSch = sqlRdr("InstAtt")
            PrevGPA = Format(sqlRdr("SemesterGPA"), "0.00")
        Else
            LT30 = False
            PrevSem = ""
            PrevSch = ""
            PrevGPA = ""
        End If
        sqlRdr.Close()

        dbConnection.Close()

        'determine if eligibility was lost
        If CDbl(txtGPA.Text) < 3 And LT30 And frmStudentsForm.LowGPAOV Then EligLost = True Else EligLost = False

        'set values to link communication record to document file
        ComDt = Now()
        TransID = Format(DateValue(ComDt), "MMddyyyy") & Format(TimeValue(ComDt), "HHmmss")

        'just save the record if the status is unpaid
        If hdrHeader.txtStatus.Text = "Unpaid" Then
            SaveRecord(hdrHeader.txtStatus.Text)
            MsgBox("Grades updated.", MsgBoxStyle.Information, "New Century Scholarship Program")
            Exit Sub
        End If

        'close the acccount if the student dropped below full time
        If hdrHeader.txtStatus.Text <> "Less Than Full-Time" And CDbl(txtHoursEnrolled.Text) <> CDbl(txtHoursCompleted.Text) Then
            'warn the user and update account if hours completed = 0
            If CDbl(txtHoursCompleted.Text) = 0 Then
                If MsgBox("The student completed zero hours so the account will be closed.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                'add transaction record and update account balance and hours remaining
                TransHrs = CDbl(txtHoursPaid.Text)
                AddnSave("Adjustment", 0 - TransHrs, 0, hdrHeader.txtStatus.Text)

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, zero hours completed", "Hours Completed and Grades Received, Zero Hours Completed")

                'close account
                frmStudentsForm.UpdatedbySchedule(, , "Closed", Format(Today(), "MM/dd/yyyy"), "Less Than Full-Time")
                PrintNotatePromptOnCloseAccount("Less Than Full-Time", hdrHeader.txtAccountID.Text)

                Exit Sub

                'if hours completed < hours enrolled and LFT and box is checked, unless completed remaining hours of eligibility close account and 
            ElseIf CDbl(txtHoursCompleted.Text) < 12 And chkLFTOverride.Checked = True Then
                AddnSave("Adjustment", 0 - (txtHoursPaid.Text - txtHoursCompleted.Text), 0, hdrHeader.txtStatus.Text)

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, hours dropped below full-time", "Hours Completed and Grades Received, Hours Dropped")

                'warn the user and update account if hours completed < 12
            ElseIf CDbl(txtHoursCompleted.Text) < 12 And chkLFTOverride.Checked = False Then
                If MsgBox("The student completed less than the full-time equivalent hours so the account will be closed.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                '*modify adjustment to full amount paid
                'add transaction record and update account balance and hours remaining
                TransHrs = CDbl(txtHoursPaid.Text)
                If hdrHeader.txtStatus.Text <> "Low GPA" Then AddnSave("Adjustment", 0 - TransHrs, 0, "Less Than Full-Time") Else AddnSave("Adjustment", 0 - TransHrs, 0, hdrHeader.txtStatus.Text)

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, hours dropped below full-time", "Hours Completed and Grades Received, Hours Dropped")

                'close account
                frmStudentsForm.UpdatedbySchedule(, , "Closed", Format(Today(), "MM/dd/yyyy"), "Less Than Full-Time")
                PrintNotatePromptOnCloseAccount("Less Than Full-Time", hdrHeader.txtAccountID.Text)

                Exit Sub

            End If
        End If

        'update account if hours enrolled does not equal hours completed
        If hdrHeader.txtStatus.Text <> "Less Than Full-Time" And CDbl(txtHoursEnrolled.Text) <> CDbl(txtHoursCompleted.Text) Then
            If CDbl(txtHoursEnrolled.Text) > CDbl(txtHoursCompleted.Text) Then
                'warn the user if hours completed < hours enrolled but > hours paid
                If CDbl(txtHoursCompleted.Text) > CDbl(txtHoursPaid.Text) Then
                    If MsgBox("The student completed less hours than enrolled but more hours than have been paid so the remaining hours will be reduced.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                    'add transaction record and update account balance and hours remaining
                    TransHrs = CDbl(txtHoursCompleted.Text) - CDbl(txtHoursPaid.Text)
                    If TransHrs > Val(hdrHeader.txtHoursRemaining.Text) Then TransHrs = Val(hdrHeader.txtHoursRemaining.Text)
                    AddnSave("Adjustment", 0 - TransHrs, 0, hdrHeader.txtStatus.Text)

                    'add communications record
                    AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, hours dropped", "Hours Completed and Grades Received, Hours Added")

                    'warn the user update account if hours completed < hours enrolled
                ElseIf CDbl(txtHoursCompleted.Text) <= CDbl(txtHoursPaid.Text) Then
                    If MsgBox("The student completed less hours than have been paid so the remaining hours will be increased.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                    txtSchHoursRemaining.Text = (CInt(txtSchHoursRemaining.Text) - (CInt(txtHoursCompleted.Text) - CInt(txtHoursEnrolled.Text))).ToString()
                    'add transaction record and update account balance and hours remaining
                    TransHrs = CDbl(txtHoursCompleted.Text) - CDbl(txtHoursPaid.Text)
                    AddnSave("Adjustment", TransHrs, 0, hdrHeader.txtStatus.Text)

                    'add communications record
                    AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, hours dropped", "Hours Completed and Grades Received, Hours Added")
                End If

            ElseIf CDbl(txtHoursEnrolled.Text) < CDbl(txtHoursCompleted.Text) Then
                'warn the user and save changes if hours completed > hours enrolled but no hours of eligibility left
                If hdrHeader.txtHoursRemaining.Text = 0 Then
                    MsgBox("The student completed more hours than originally enrolled but has no hours of eligibility left so the hours remaining will not be adjusted.", MsgBoxStyle.Information, "New Century Scholarship Program")
                    SaveRecord(hdrHeader.txtStatus.Text)

                    'warn the user and save changes if hours completed > hours enrolled
                Else
                    If MsgBox("The student completed more hours than originally enrolled so the remaining hours will be reduced.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

                    'add transaction record and update account balance and hours remaining
                    TransHrs = LesserOf(CStr(CDbl(txtHoursCompleted.Text) - CDbl(txtHoursPaid.Text)), hdrHeader.txtHoursRemaining.Text)
                    AddnSave("Adjustment", TransHrs, 0, hdrHeader.txtStatus.Text)

                    'add communications record
                    AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, hours added", "Hours Completed and Grades Received, Hours Added")
                End If
            End If

            'warn the user and save changes if hours completed > 12 and status is LFT
        ElseIf hdrHeader.txtStatus.Text = "Less Than Full-Time" And (CDbl(txtHoursCompleted.Text) >= 12 Or CDbl(txtHoursCompleted.Text) - CDbl(txtHoursPaid.Text) >= CDbl(hdrHeader.txtHoursRemaining.Text) Or chkLFTOverride.Checked = True) And Not EligLost Then
            If MsgBox("The student enrolled Less Than Full-Time but completed enough hours to be paid for the hours completed (or the Less Than Full-Time override indicator is checked) so the remaining balance will be reduced.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub

            'add transaction record and update account balance and hours remaining
            TransHrs = LesserOf(CDbl(txtHoursCompleted.Text) - CDbl(txtHoursPaid.Text), hdrHeader.txtHoursRemaining.Text)
            If hdrHeader.txtStatus.Text <> "Low GPA" Then AddnSave("Adjustment", TransHrs, 0, "Paid") Else AddnSave("Adjustment", TransHrs, 0, hdrHeader.txtStatus.Text)

            'add communications record
            AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "received hours completed and grades, hours added", "Hours Completed and Grades Received, Hours Added")
        Else
            AddCommunications(hdrHeader.txtAccountID.Text, ComDt, True, False, "Account Maintenance", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated", "Received Hours Completed and Grades")
            SaveRecord(hdrHeader.txtStatus.Text)
            MsgBox("Grades updated.", MsgBoxStyle.Information, "New Century Scholarship Program")
        End If

        'Do not print letter or add communication record if the account is already closed.
        If frmStudentsForm.hdrHeader.txtStatus.Text = "Closed" Then
            MsgBox("Account is already closed. There is no probation letter to print", MsgBoxStyle.Information, "Account Closed")
            Exit Sub
        End If

        'warn user, print letter, and add communications record if GPA < 3 for current schedule
        If CDbl(txtGPA.Text) < 3 AndAlso Not LT30 AndAlso Not hdrHeader.txtStatus.Text = "Closed" Then
            If MsgBox("The GPA for the current schedule is below 3.0 so a NCSP PROBATION WARNING letter will be generated.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub
            PrintLetter("NCSPROBAT", TransID, PrevSem, PrevSch, PrevGPA)
            AddCommunications(hdrHeader.txtAccountID.Text, ComDt, False, True, "Letter", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, GPA below 3.0, NCSP PROBATION WARNING letter sent", "NCSP PROBATION WARNING Letter Sent")
            SaveRecord(hdrHeader.txtStatus.Text)
            Exit Sub
            'warn user, print letter, and add communications record if GPA < 3 for previous and current schedules
        ElseIf CDbl(txtGPA.Text) < 3 And LT30 Then
            If Not frmStudentsForm.LowGPAOV Then
                If MsgBox("The GPA for the current and a previous schedule is below 3.0 so the student has lost eligibility.  The account will be closed and a NCSP ELIGIBILITY LOST LOW GPA letter will be generated.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub
                frmStudentsForm.UpdatedbySchedule(, , "Closed", Format(Today(), "MM/dd/yyyy"), "Low GPA")
                hdrHeader.txtStatus.Text = "Low GPA"
                'PrintLetter("NCSELIGLOS", TransID, PrevSem, PrevSch, PrevGPA)
                PrintNotatePromptOnCloseAccount("Low GPA", hdrHeader.txtAccountID.Text)
                AddCommunications(hdrHeader.txtAccountID.Text, ComDt, False, True, "Letter", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, account closed due to low GPA, NCSP ELIGIBILITY LOST LOW GPA letter sent", "NCSP ELIGIBILITY LOST LOW GPA Letter Sent")
                SaveRecord(hdrHeader.txtStatus.Text)
                Exit Sub
                'warn user, print letter, and add communications record if GPA < 3 for previous and current schedules and student already used grace
            Else
                If MsgBox("The GPA for the current and a previous schedule is below 3.0 and the low GPA override indicator is checked so the student has permanently lost eligibility.  The account will be closed and a NCSP ELIGIBILITY PERMANENTLY LOST LOW GPA letter will be generated.  Click OK to continue or Cancel to quit.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program") = MsgBoxResult.Cancel Then Exit Sub
                frmStudentsForm.UpdatedbySchedule(, , "Closed", Format(Today(), "MM/dd/yyyy"), "Low GPA - Grace Used")
                hdrHeader.txtStatus.Text = "Low GPA"
                PrintNotatePromptOnCloseAccount("Low GPA - Grace Used", hdrHeader.txtAccountID.Text)
                AddCommunications(hdrHeader.txtAccountID.Text, ComDt, False, True, "Letter", "received hours completed and grades for " & GetSemYr() & " schedule, schedule updated, account permanently closed due to low GPA, NCSP ELIGIBILITY PERMANENTLY LOST LOW GPA letter sent", "NCSP ELIGIBILITY PERMANENTLY LOST LOW GPA Letter Sent")
                SaveRecord(hdrHeader.txtStatus.Text)
                Exit Sub
            End If
        End If
    End Sub

    'link document
    Private Sub btnLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLink.Click
        Dim FileOpener As New OpenFileDialog
        Dim NewFile As String

        'prompt user for file
        FileOpener.ShowDialog()
        If FileOpener.FileName = "" Then Exit Sub
        NewFile = DocPath & hdrHeader.txtAccountID.Text & "\Schedules\" & txtNumber.Text & "\" & Dir(FileOpener.FileName, FileAttribute.Hidden)

        'warn if file already exists
        If File.Exists(NewFile) Then
            MsgBox("A file with the name selected already exists for the student.  Change the file name and try again.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        End If

        'copy file and warn user if copy not sucessful
        Try
            File.Move(FileOpener.FileName, NewFile)
        Catch ex As Exception
            MsgBox("The file was not moved to the student's folder for the following reason:  " & ex.Message & "  Please try again or contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        End Try

        MsgBox("The file has been moved to the student's folder.", MsgBoxStyle.Information, "New Century Scholarship Program")
    End Sub

    'view document
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim FileOpener As New OpenFileDialog

        'prompt user to select file
        FileOpener.InitialDirectory = DocPath & hdrHeader.txtAccountID.Text & "\Schedules\" & txtNumber.Text
        FileOpener.ShowDialog()

        'display file if one was selected
        If FileOpener.FileName <> "" Then
            Process.Start(FileOpener.FileName)
        End If
    End Sub
#End Region

#Region "General Functions"
    'fill schedule information
    Sub FillFields(ByVal SchNo As Integer)
        Dim LockedBy As String
        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'lock schedule record for editing
        If txtNumber.Text <> "" Then
            sqlCmd.CommandText = "UPDATE Schedule SET LockedBy = '' WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A' AND SchedID = " & txtNumber.Text
            sqlCmd.ExecuteNonQuery()
        End If

        'get schedule information
        sqlCmd.CommandText = "SELECT * FROM Schedule WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A' AND SchedID = " & SchNo
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()

        'fill controls with schedule information
        hdrHeader.txtStatus.Text = sqlRdr("SchedStat")
        txtNumber.Text = sqlRdr("SchedID")
        txtCreated.Text = Format(sqlRdr("SchedDt"), "MM/dd/yyyy")
        txtSchHoursRemaining.Text = Format(sqlRdr("SchedHrRem"), "0.00")
        txtHoursPaid.Text = Format(sqlRdr("CredHrPd"), "0.00")
        cmbInstitution.SelectedItem = sqlRdr("InstAtt")
        BegInst = sqlRdr("InstAtt")
        cmbSemester.SelectedItem = sqlRdr("Semester")
        BegSemester = sqlRdr("Semester")
        If sqlRdr("SchedYr") = 0 Then
            cmbYear.SelectedItem = ""
            BegYear = ""
        Else
            cmbYear.SelectedItem = CStr(sqlRdr("SchedYr"))
            BegYear = CStr(sqlRdr("SchedYr"))
        End If
        If Not IsDBNull(sqlRdr("CredHrEnr")) Then
            txtHoursEnrolled.Text = Format(sqlRdr("CredHrEnr"), "0.00")
            BegHrsEnrolled = CDbl(txtHoursEnrolled.Text)
        Else
            txtHoursEnrolled.Text = ""
            BegHrsEnrolled = 0
        End If
        If Not IsDBNull(sqlRdr("CredHrComp")) Then txtHoursCompleted.Text = Format(sqlRdr("CredHrComp"), "0.00") Else txtHoursCompleted.Text = ""
        If Not IsDBNull(sqlRdr("SemesterGPA")) Then txtGPA.Text = Format(sqlRdr("SemesterGPA"), "0.00") Else txtGPA.Text = ""
        chkLFTOverride.Checked = sqlRdr("LTH")

        LockedBy = sqlRdr("LockedBy")
        sqlRdr.Close()

        'lock controls for editing if necessary
        If (LockedBy <> "" And LockedBy <> UserID) Or UserAccess = "Read Only Access" Then
            If UserAccess <> "Read Only Access" Then MsgBox("The record is locked by user " & LockedBy & ".", MsgBoxStyle.Information, "New Century Scholarship Program")
            LockEm(False)
        Else
            sqlCmd.CommandText = "UPDATE Schedule SET LockedBy = '" & UserID & "' WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A' AND SchedID = " & SchNo
            sqlCmd.ExecuteNonQuery()
            LockEm(True)
        End If

        dbConnection.Close()

        'enable/disable navigation buttons
        If txtNumber.Text = 1 Then btnFirst.Enabled = False Else btnFirst.Enabled = True
        If txtNumber.Text = 1 Then btnPrev.Enabled = False Else btnPrev.Enabled = True
        If txtNumber.Text = maxSched Then btnNext.Enabled = False Else btnNext.Enabled = True
        If txtNumber.Text = maxSched Then btnLast.Enabled = False Else btnLast.Enabled = True
    End Sub

    'enable or disable controls for editing
    Sub LockEm(ByVal Enabler As Boolean)
        cmbInstitution.Enabled = Enabler
        cmbSemester.Enabled = Enabler
        cmbYear.Enabled = Enabler
        txtHoursEnrolled.Enabled = Enabler
        txtHoursCompleted.Enabled = Enabler
        txtGPA.Enabled = Enabler
        chkLFTOverride.Enabled = Enabler
        If frmStudentsForm.AccountStatus <> "Approved" Then
            btnAdd.Enabled = False
            btnApproved.Enabled = False
        Else
            If CDbl(hdrHeader.txtHoursRemaining.Text) = 0 Then btnAdd.Enabled = False Else btnAdd.Enabled = Enabler
            btnApproved.Enabled = Enabler
        End If
        btnSave.Enabled = Enabler
        If hdrHeader.txtStatus.Text = "Adding" Then
            btnGrades.Enabled = False
        Else
            btnGrades.Enabled = Enabler
        End If
        btnLink.Enabled = Enabler
    End Sub

    'save schedule record
    Sub SaveRecord(ByVal schStatus As String)
        Dim LTH As Integer
        Dim fieldList As String
        Dim valueList As String
        Dim Yr As Integer

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'update current record to historical
        sqlCmd.CommandText = "UPDATE Schedule SET RowStatus = 'H', LockedBy = '' WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A' AND SchedID = " & Val(txtNumber.Text)
        sqlCmd.ExecuteNonQuery()

        hdrHeader.txtStatus.Text = schStatus

        'format values, build SQL statement and create new record for the schedule
        If cmbYear.SelectedItem = "" Then Yr = 0 Else Yr = CInt(cmbYear.SelectedItem)
        If chkLFTOverride.Checked Then LTH = 1 Else LTH = 0
        valueList = "'" & hdrHeader.txtAccountID.Text & "', " & Val(txtNumber.Text) & ", '" & txtCreated.Text & "', '" & Replace(schStatus, "'", "''") & "', " & CDbl(txtSchHoursRemaining.Text) & ", " & CDbl(txtHoursPaid.Text) & ", '" & Replace(cmbInstitution.SelectedItem, "'", "''") & "', '" & Replace(cmbSemester.SelectedItem, "'", "''") & "', " & Yr & ", " & LTH & ", '" & UserID & "', '" & UserID & "'"
        fieldList = "AcctID, SchedID, SchedDt, SchedStat, SchedHrRem, CredHrPd, InstAtt, Semester, SchedYr, LTH, LastUpdateUser, LockedBy"
        If txtHoursEnrolled.Text <> "" Then
            fieldList = fieldList & ", CredHrEnr"
            valueList = valueList & ", " & CDbl(txtHoursEnrolled.Text)
        End If
        If txtHoursCompleted.Text <> "" Then
            fieldList = fieldList & ", CredHrComp"
            valueList = valueList & ", " & CDbl(txtHoursCompleted.Text)
        End If
        If txtGPA.Text <> "" Then
            fieldList = fieldList & ", SemesterGPA"
            valueList = valueList & ", " & CDbl(txtGPA.Text)
        End If
        sqlCmd.CommandText = "INSERT INTO Schedule (" & fieldList & ") VALUES (" & valueList & ")"
        sqlCmd.ExecuteNonQuery()

        dbConnection.Close()
    End Sub

    'check to see if a schedule already exists for the same semester
    Function SemesterExists() As Boolean
        If cmbSemester.SelectedItem = "" Or cmbYear.SelectedItem = "" Then Exit Function

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        SemesterExists = False

        sqlCmd.CommandText = "SELECT * FROM Schedule WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A' AND Semester = '" & cmbSemester.SelectedItem & "' AND SchedYr = " & cmbYear.SelectedItem & " AND InstAtt = '" & cmbInstitution.Text & "' AND SchedID <> " & Val(txtNumber.Text)
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        If sqlRdr.HasRows Then
            MsgBox("A schedule already exists for " & cmbSemester.SelectedItem & " semester " & cmbYear.SelectedItem & ".  Another schedule cannot be created for the same semester and year.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            SemesterExists = True
        End If
        sqlRdr.Close()
        dbConnection.Close()
    End Function

    'combine semester and year
    Function GetSemYr() As String
        GetSemYr = cmbSemester.SelectedItem & " " & cmbYear.SelectedItem
    End Function

    'get lesser value of two arguments
    Function LesserOf(ByVal val1 As String, ByVal val2 As String) As Double
        If CDbl(val1) < CDbl(val2) Then LesserOf = CDbl(val1) Else LesserOf = CDbl(val2)
    End Function

    'update value and save updates
    Sub AddnSave(ByVal tType As String, ByVal tHrs As Double, ByVal tAmt As Double, ByVal sSta As String)
        'update balance, hours remaining, and hours paid
        hdrHeader.txtBalance.Text = Format(CDbl(hdrHeader.txtBalance.Text) + tAmt, "$###,##0.00;($###,##0.00)")
        hdrHeader.txtHoursRemaining.Text = Format(CDbl(hdrHeader.txtHoursRemaining.Text) - tHrs, "#0.00;(#0.00)")
        txtHoursPaid.Text = Format(CDbl(txtHoursPaid.Text) + tHrs, "#0.00;(#0.00)")

        'save schedule info
        SaveRecord(sSta)

        'update and save student info
        frmStudentsForm.UpdatedbySchedule(tAmt, tHrs)

        ''add transaction
        AddTransaction(tType, hdrHeader.txtAccountID.Text, cmbSemester.SelectedItem, CInt(cmbYear.SelectedItem), Inst, tHrs, tAmt, txtNumber.Text)

        'disable the add button if the hours remaining is zero
        If CDbl(hdrHeader.txtHoursRemaining.Text) = 0 Then btnAdd.Enabled = False
    End Sub

    'print letters
    Sub PrintLetter(ByVal Doc As String, ByVal TransID As String, ByVal Sem As String, ByVal Sch As String, ByVal GPA As String)
        Dim Zip As String

        'get account information
        dbConnection.Open()
        sqlCmd.CommandText = "SELECT * FROM Account WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()

        'format zip code
        If Len(sqlRdr("Zip")) > 5 Then Zip = CLng(sqlRdr("Zip")).ToString("00000-0000") Else Zip = CStr(sqlRdr("Zip"))

        'print letter
        FileOpen(1, "T:\NCSPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
        WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Semester", "School", "GPA", "PreviousSemester", "PreviousSchool", "PreviousGPA", "StaticCurrentDate")
        WriteLine(1, sqlRdr("AcctID"), sqlRdr("FName"), sqlRdr("LName"), sqlRdr("Add1"), sqlRdr("Add2"), sqlRdr("City"), sqlRdr("ST"), Zip, cmbSemester.SelectedItem, cmbInstitution.SelectedItem, txtGPA.Text, Sem, Sch, GPA, Date.Now.ToShortDateString())
        FileClose(1)
        sqlRdr.Close()
        dbConnection.Close()
        PrintDoc(Doc, hdrHeader.txtAccountID.Text & "\Communications", TransID)
    End Sub

    'determine if the student has already met the two-semester-a-year rule (the student can only be paid for two semesters per year)
    Function HasMetTwoSemesterRule() As Boolean
        Dim YearEdits(0 To 3) As Integer

        HasMetTwoSemesterRule = False
        Dim standardQuery As String = "SELECT COUNT(DISTINCT Semester) AS CountOfSchedules FROM Schedule WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND SchedID <> " & txtNumber.Text & " AND SchedStat <> 'Withdrawn' AND RowStatus = 'A' AND "
        Dim customWhere As String = ""

        'determine the year edits to use according to the circumstances
        If (cmbSemester.SelectedItem = "Spring" And cmbInstitution.SelectedItem = "Brigham Young University") Or cmbSemester.SelectedItem = "Summer" Or cmbSemester.SelectedItem = "Fall" Then
            YearEdits(0) = CInt(cmbYear.SelectedItem)
            YearEdits(1) = CInt(cmbYear.SelectedItem) + 1
            YearEdits(2) = CInt(cmbYear.SelectedItem)
            YearEdits(3) = CInt(cmbYear.SelectedItem) + 1
        Else
            YearEdits(0) = CInt(cmbYear.SelectedItem) - 1
            YearEdits(1) = CInt(cmbYear.SelectedItem)
            YearEdits(2) = CInt(cmbYear.SelectedItem) - 1
            YearEdits(3) = CInt(cmbYear.SelectedItem)
        End If

        'add year edits to where statement
        customWhere = "((Semester IN ('Spring') AND InstAtt = 'Brigham Young University' AND SchedYr = " & YearEdits(0) & ") OR (Semester IN ('Spring') AND InstAtt <> 'Brigham Young University' AND SchedYr = " & YearEdits(1) & ") OR (Semester IN ('Summer','Fall') AND SchedYr = " & YearEdits(2) & ")	OR (Semester IN ('Winter') AND SchedYr = " & YearEdits(3) & "))"

        '(
        '(Semester IN ('Spring') AND InstAtt = 'Brigham Young University' AND SchedYr = [yearedit])
        'OR 
        '(Semester IN ('Spring') AND InstAtt <> 'Brigham Young University' AND SchedYr = [yearedit])
        'OR
        '(Semester IN ('Summer','Fall') AND SchedYr = [yearedit])
        'OR
        '(Semester IN ('Winter') AND SchedYr = [yearedit])
        ')

        dbConnection.Open()
        sqlCmd.CommandText = standardQuery & customWhere

        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        If sqlRdr("CountOfSchedules") > 1 Then HasMetTwoSemesterRule = True

        sqlRdr.Close()
        dbConnection.Close()

    End Function
#End Region

#Region "Control Events"
    'verify and format data
    Private Sub txtHoursEnrolled_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtHoursEnrolled.Validating
        If Not IsNumeric(txtHoursEnrolled.Text) And txtHoursEnrolled.Text <> "" Then
            MsgBox("Numeric data required.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtHoursEnrolled.Text <> "" Then
            If CDbl(txtHoursEnrolled.Text) > 25 Then
                MsgBox("The maximum number of hours enrolled allowed is 25.", MsgBoxStyle.Exclamation, "Invalid Data")
                e.Cancel = True
            ElseIf CDbl(txtHoursEnrolled.Text) = 0 Then
                MsgBox("If the student is not attending for the current semester, please process the school refund or reversal through payment processing to update the schedule.", MsgBoxStyle.Exclamation, "Invalid Data")
                e.Cancel = True
            Else
                txtHoursEnrolled.Text = Format(CDbl(txtHoursEnrolled.Text), "0.00")
            End If
        End If
    End Sub

    Private Sub txtHoursCompleted_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtHoursCompleted.Validating
        If Not IsNumeric(txtHoursCompleted.Text) And txtHoursCompleted.Text <> "" Then
            MsgBox("Numeric data required.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtHoursCompleted.Text <> "" Then
            If CDbl(txtHoursCompleted.Text) > 25 Then
                MsgBox("The maximum number of hours completed allowed is 25.", MsgBoxStyle.Exclamation, "Invalid Data")
                e.Cancel = True
            Else
                txtHoursCompleted.Text = Format(CDbl(txtHoursCompleted.Text), "0.00")
            End If
        End If
    End Sub

    Private Sub txtGPA_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtGPA.Validating
        If Not IsNumeric(txtGPA.Text) And txtGPA.Text <> "" Then
            MsgBox("Numeric data required.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtGPA.Text <> "" Then
            txtGPA.Text = Format(CDbl(txtGPA.Text), "0.00")
        End If
    End Sub
#End Region

#Region "Navigation Buttons"
    Private Sub btnStudentInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStudentInfo.Click
        ShowForms.CloseForms()
    End Sub

    Private Sub btnTransHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransHistory.Click
        ShowForms.TransHistory()
        Me.Close()
    End Sub

    Private Sub cmbCommunications_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCommunications.SelectedIndexChanged
        If cmbCommunications.SelectedItem <> "Communications" Then
            ShowForms.Communications(cmbCommunications.SelectedItem)
        End If
    End Sub

    Private Sub btnFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        FillFields(1)
    End Sub

    Private Sub btnPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        FillFields(Val(txtNumber.Text) - 1)
    End Sub

    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        FillFields(Val(txtNumber.Text) + 1)
    End Sub

    Private Sub btnLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLast.Click
        FillFields(maxSched)
    End Sub
#End Region


End Class
