Imports System
Imports System.IO
Imports System.Text

Public Class frmStudents
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'add citizenship documentation to list
        sqlCmd.CommandText = "SELECT Documentation FROM CitizenshipDocumentation ORDER BY Documentation"
        sqlRdr = sqlCmd.ExecuteReader
        cmbCitizenship.Items.Add(String.Empty)
        While sqlRdr.Read
            cmbCitizenship.Items.Add(sqlRdr("Documentation"))
        End While
        sqlRdr.Close()

        'add high schools to list
        sqlCmd.CommandText = "SELECT HSName FROM HighSchl ORDER BY HSName"
        sqlRdr = sqlCmd.ExecuteReader
        While sqlRdr.Read
            cmbSchool.Items.Add(sqlRdr("HSName"))
        End While
        sqlRdr.Close()

        'add schools to list
        sqlCmd.CommandText = "SELECT DISTINCT InstLong FROM Inst ORDER BY InstLong"
        sqlRdr = sqlCmd.ExecuteReader
        While sqlRdr.Read
            cmbInstitution.Items.Add(sqlRdr("InstLong"))
            cmbBSInstitution.Items.Add(sqlRdr("InstLong"))
        End While
        sqlRdr.Close()

        'add schools to list for application section
        sqlCmd.CommandText = "SELECT DISTINCT InstLong FROM Inst WHERE YearType = 4 ORDER BY InstLong"
        sqlRdr = sqlCmd.ExecuteReader
        While sqlRdr.Read
            cmbAppInstitution.Items.Add(sqlRdr("InstLong"))
        End While
        sqlRdr.Close()

        'add degrees to list
        sqlCmd.CommandText = "SELECT DegreeNm FROM Degree ORDER BY DegreeNm"
        sqlRdr = sqlCmd.ExecuteReader
        While sqlRdr.Read
            cmbDegree.Items.Add(sqlRdr("DegreeNm"))
        End While
        sqlRdr.Close()

        'add eligibility ended reasons to list
        sqlCmd.CommandText = "SELECT EligEndReason FROM EligEnd ORDER BY EligEndReason"
        sqlRdr = sqlCmd.ExecuteReader
        While sqlRdr.Read
            cmbEndedReason.Items.Add(sqlRdr("EligEndReason"))
        End While
        sqlRdr.Close()

        'set text of communications drop down
        cmbCommunications.Text = "Communications"

        dbConnection.Close()
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtEligibilityEnded As System.Windows.Forms.TextBox
    Friend WithEvents txtEligibilityEndedComments As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtAppRecd As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtAppApvd As System.Windows.Forms.TextBox
    Friend WithEvents btnLinkApp As System.Windows.Forms.Button
    Friend WithEvents btnViewApp As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnApproved As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnSchedules As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkLowGPA As System.Windows.Forms.CheckBox
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents txtSSN As System.Windows.Forms.TextBox
    Friend WithEvents txtBirthDate As System.Windows.Forms.TextBox
    Friend WithEvents cmbState As System.Windows.Forms.ComboBox
    Friend WithEvents txtZip As System.Windows.Forms.TextBox
    Friend WithEvents txtCity As System.Windows.Forms.TextBox
    Friend WithEvents txtStreet2 As System.Windows.Forms.TextBox
    Friend WithEvents txtStreet1 As System.Windows.Forms.TextBox
    Friend WithEvents txtLast As System.Windows.Forms.TextBox
    Friend WithEvents txtMI As System.Windows.Forms.TextBox
    Friend WithEvents txtFirst As System.Windows.Forms.TextBox
    Friend WithEvents txtDistrict As System.Windows.Forms.TextBox
    Friend WithEvents cmbSchool As System.Windows.Forms.ComboBox
    Friend WithEvents txtTranscriptRevd As System.Windows.Forms.TextBox
    Friend WithEvents txtACT As System.Windows.Forms.TextBox
    Friend WithEvents txtGPA As System.Windows.Forms.TextBox
    Friend WithEvents txtGradDate As System.Windows.Forms.TextBox
    Friend WithEvents cmbDegree As System.Windows.Forms.ComboBox
    Friend WithEvents cmbInstitution As System.Windows.Forms.ComboBox
    Friend WithEvents txtDegreeTranscript As System.Windows.Forms.TextBox
    Friend WithEvents txtDegreeGPA As System.Windows.Forms.TextBox
    Friend WithEvents txtDegreeCompleted As System.Windows.Forms.TextBox
    Friend WithEvents txtLOAComments As System.Windows.Forms.TextBox
    Friend WithEvents txtLOAEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtLOABegin As System.Windows.Forms.TextBox
    Friend WithEvents txtLowGPACOmments As System.Windows.Forms.TextBox
    Friend WithEvents cmbCommunications As System.Windows.Forms.ComboBox
    Friend WithEvents cmbGender As System.Windows.Forms.ComboBox
    Friend WithEvents cmbEthnic As System.Windows.Forms.ComboBox
    Friend WithEvents cmbEndedReason As System.Windows.Forms.ComboBox
    Friend WithEvents btnTransHistory As System.Windows.Forms.Button
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents txtBSDegreeCompleted As System.Windows.Forms.TextBox
    Friend WithEvents cmbBSInstitution As System.Windows.Forms.ComboBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents txtCellPhone As System.Windows.Forms.TextBox
    Friend WithEvents cmbCitizenship As System.Windows.Forms.ComboBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents txtInfoRelease As System.Windows.Forms.TextBox
    Friend WithEvents cmbCriminalRecord As System.Windows.Forms.ComboBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents txtAppApprovedComments As System.Windows.Forms.TextBox
    Friend WithEvents cmbAppInstitution As System.Windows.Forms.ComboBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents rdoLOA As System.Windows.Forms.RadioButton
    Friend WithEvents rdoDeferral As System.Windows.Forms.RadioButton
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents txtLOARequested As System.Windows.Forms.TextBox
    Friend WithEvents rdoLOADenied As System.Windows.Forms.RadioButton
    Friend WithEvents rdoLOAApproved As System.Windows.Forms.RadioButton
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbLOAYear As System.Windows.Forms.ComboBox
    Friend WithEvents cmbLOASemester As System.Windows.Forms.ComboBox
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents txtACTDate As System.Windows.Forms.TextBox
    Friend WithEvents hdrHeader As NCSP.FormHeader
    Friend WithEvents txtAltLast As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStudents))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtEligibilityEndedComments = New System.Windows.Forms.TextBox
        Me.txtEligibilityEnded = New System.Windows.Forms.TextBox
        Me.chkLowGPA = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label41 = New System.Windows.Forms.Label
        Me.txtInfoRelease = New System.Windows.Forms.TextBox
        Me.cmbCriminalRecord = New System.Windows.Forms.ComboBox
        Me.Label40 = New System.Windows.Forms.Label
        Me.cmbCitizenship = New System.Windows.Forms.ComboBox
        Me.Label39 = New System.Windows.Forms.Label
        Me.Label38 = New System.Windows.Forms.Label
        Me.txtCellPhone = New System.Windows.Forms.TextBox
        Me.Label37 = New System.Windows.Forms.Label
        Me.txtAltLast = New System.Windows.Forms.TextBox
        Me.Label31 = New System.Windows.Forms.Label
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.cmbGender = New System.Windows.Forms.ComboBox
        Me.cmbEthnic = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtPhone = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtSSN = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtBirthDate = New System.Windows.Forms.TextBox
        Me.cmbState = New System.Windows.Forms.ComboBox
        Me.txtZip = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtCity = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtStreet2 = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtStreet1 = New System.Windows.Forms.TextBox
        Me.txtLast = New System.Windows.Forms.TextBox
        Me.txtMI = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtFirst = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label43 = New System.Windows.Forms.Label
        Me.txtAppApprovedComments = New System.Windows.Forms.TextBox
        Me.cmbAppInstitution = New System.Windows.Forms.ComboBox
        Me.Label42 = New System.Windows.Forms.Label
        Me.btnViewApp = New System.Windows.Forms.Button
        Me.btnLinkApp = New System.Windows.Forms.Button
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtAppApvd = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtAppRecd = New System.Windows.Forms.TextBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.Label44 = New System.Windows.Forms.Label
        Me.txtACTDate = New System.Windows.Forms.TextBox
        Me.txtDistrict = New System.Windows.Forms.TextBox
        Me.cmbSchool = New System.Windows.Forms.ComboBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.txtTranscriptRevd = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.txtACT = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.txtGPA = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtGradDate = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.cmbDegree = New System.Windows.Forms.ComboBox
        Me.cmbInstitution = New System.Windows.Forms.ComboBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.txtDegreeTranscript = New System.Windows.Forms.TextBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.txtDegreeGPA = New System.Windows.Forms.TextBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.txtDegreeCompleted = New System.Windows.Forms.TextBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.cmbLOAYear = New System.Windows.Forms.ComboBox
        Me.cmbLOASemester = New System.Windows.Forms.ComboBox
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.rdoLOAApproved = New System.Windows.Forms.RadioButton
        Me.rdoLOADenied = New System.Windows.Forms.RadioButton
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.rdoDeferral = New System.Windows.Forms.RadioButton
        Me.rdoLOA = New System.Windows.Forms.RadioButton
        Me.Label47 = New System.Windows.Forms.Label
        Me.Label46 = New System.Windows.Forms.Label
        Me.txtLOARequested = New System.Windows.Forms.TextBox
        Me.txtLOAComments = New System.Windows.Forms.TextBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.txtLOAEnd = New System.Windows.Forms.TextBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.txtLOABegin = New System.Windows.Forms.TextBox
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.cmbEndedReason = New System.Windows.Forms.ComboBox
        Me.txtLowGPACOmments = New System.Windows.Forms.TextBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnApproved = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.Label36 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.cmbBSInstitution = New System.Windows.Forms.ComboBox
        Me.txtBSDegreeCompleted = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnSchedules = New System.Windows.Forms.Button
        Me.cmbCommunications = New System.Windows.Forms.ComboBox
        Me.btnTransHistory = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.hdrHeader = New NCSP.FormHeader
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 20)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Comments"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 20)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Eligibility Ended"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtEligibilityEndedComments
        '
        Me.txtEligibilityEndedComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEligibilityEndedComments.Location = New System.Drawing.Point(144, 48)
        Me.txtEligibilityEndedComments.Multiline = True
        Me.txtEligibilityEndedComments.Name = "txtEligibilityEndedComments"
        Me.txtEligibilityEndedComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEligibilityEndedComments.Size = New System.Drawing.Size(432, 56)
        Me.txtEligibilityEndedComments.TabIndex = 2
        '
        'txtEligibilityEnded
        '
        Me.txtEligibilityEnded.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEligibilityEnded.Location = New System.Drawing.Point(144, 24)
        Me.txtEligibilityEnded.Name = "txtEligibilityEnded"
        Me.txtEligibilityEnded.Size = New System.Drawing.Size(144, 20)
        Me.txtEligibilityEnded.TabIndex = 0
        '
        'chkLowGPA
        '
        Me.chkLowGPA.Location = New System.Drawing.Point(144, 112)
        Me.chkLowGPA.Name = "chkLowGPA"
        Me.chkLowGPA.Size = New System.Drawing.Size(16, 24)
        Me.chkLowGPA.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 20)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Low GPA Override"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label41)
        Me.GroupBox2.Controls.Add(Me.txtInfoRelease)
        Me.GroupBox2.Controls.Add(Me.cmbCriminalRecord)
        Me.GroupBox2.Controls.Add(Me.Label40)
        Me.GroupBox2.Controls.Add(Me.cmbCitizenship)
        Me.GroupBox2.Controls.Add(Me.Label39)
        Me.GroupBox2.Controls.Add(Me.Label38)
        Me.GroupBox2.Controls.Add(Me.txtCellPhone)
        Me.GroupBox2.Controls.Add(Me.Label37)
        Me.GroupBox2.Controls.Add(Me.txtAltLast)
        Me.GroupBox2.Controls.Add(Me.Label31)
        Me.GroupBox2.Controls.Add(Me.txtEmail)
        Me.GroupBox2.Controls.Add(Me.cmbGender)
        Me.GroupBox2.Controls.Add(Me.cmbEthnic)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.txtPhone)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.txtSSN)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.txtBirthDate)
        Me.GroupBox2.Controls.Add(Me.cmbState)
        Me.GroupBox2.Controls.Add(Me.txtZip)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.txtCity)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.txtStreet2)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.txtStreet1)
        Me.GroupBox2.Controls.Add(Me.txtLast)
        Me.GroupBox2.Controls.Add(Me.txtMI)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.txtFirst)
        Me.GroupBox2.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(17, 1)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(592, 438)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Student Information"
        '
        'Label41
        '
        Me.Label41.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.Location = New System.Drawing.Point(16, 364)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(120, 56)
        Me.Label41.TabIndex = 49
        Me.Label41.Text = "Confidential Information Release"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtInfoRelease
        '
        Me.txtInfoRelease.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfoRelease.Location = New System.Drawing.Point(144, 364)
        Me.txtInfoRelease.Multiline = True
        Me.txtInfoRelease.Name = "txtInfoRelease"
        Me.txtInfoRelease.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtInfoRelease.Size = New System.Drawing.Size(432, 56)
        Me.txtInfoRelease.TabIndex = 18
        '
        'cmbCriminalRecord
        '
        Me.cmbCriminalRecord.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCriminalRecord.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCriminalRecord.Items.AddRange(New Object() {"", "Has Record", "No Record"})
        Me.cmbCriminalRecord.Location = New System.Drawing.Point(144, 339)
        Me.cmbCriminalRecord.Name = "cmbCriminalRecord"
        Me.cmbCriminalRecord.Size = New System.Drawing.Size(144, 21)
        Me.cmbCriminalRecord.TabIndex = 17
        '
        'Label40
        '
        Me.Label40.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(16, 339)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(120, 20)
        Me.Label40.TabIndex = 47
        Me.Label40.Text = "Criminal Record"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCitizenship
        '
        Me.cmbCitizenship.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCitizenship.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCitizenship.Location = New System.Drawing.Point(144, 314)
        Me.cmbCitizenship.Name = "cmbCitizenship"
        Me.cmbCitizenship.Size = New System.Drawing.Size(432, 21)
        Me.cmbCitizenship.TabIndex = 16
        '
        'Label39
        '
        Me.Label39.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(16, 314)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(120, 20)
        Me.Label39.TabIndex = 43
        Me.Label39.Text = "US Citizenship"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label38
        '
        Me.Label38.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(16, 216)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(120, 20)
        Me.Label38.TabIndex = 41
        Me.Label38.Text = "Cell Phone Number"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCellPhone
        '
        Me.txtCellPhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCellPhone.Location = New System.Drawing.Point(144, 216)
        Me.txtCellPhone.MaxLength = 14
        Me.txtCellPhone.Name = "txtCellPhone"
        Me.txtCellPhone.Size = New System.Drawing.Size(144, 20)
        Me.txtCellPhone.TabIndex = 12
        '
        'Label37
        '
        Me.Label37.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(16, 48)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(120, 20)
        Me.Label37.TabIndex = 39
        Me.Label37.Text = "Alt Last Name"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAltLast
        '
        Me.txtAltLast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAltLast.Location = New System.Drawing.Point(144, 48)
        Me.txtAltLast.MaxLength = 50
        Me.txtAltLast.Name = "txtAltLast"
        Me.txtAltLast.Size = New System.Drawing.Size(200, 20)
        Me.txtAltLast.TabIndex = 3
        '
        'Label31
        '
        Me.Label31.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(16, 240)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(120, 20)
        Me.Label31.TabIndex = 37
        Me.Label31.Text = "e-Mail"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtEmail
        '
        Me.txtEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmail.Location = New System.Drawing.Point(144, 240)
        Me.txtEmail.MaxLength = 100
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(432, 20)
        Me.txtEmail.TabIndex = 13
        '
        'cmbGender
        '
        Me.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGender.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbGender.Items.AddRange(New Object() {"", "Male", "Female"})
        Me.cmbGender.Location = New System.Drawing.Point(144, 264)
        Me.cmbGender.Name = "cmbGender"
        Me.cmbGender.Size = New System.Drawing.Size(144, 21)
        Me.cmbGender.TabIndex = 14
        '
        'cmbEthnic
        '
        Me.cmbEthnic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEthnic.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEthnic.Items.AddRange(New Object() {"", "Black", "Caucasian", "Hispanic", "American Indian", "Alaskan Native", "Asian", "Pacific Islander", "Other"})
        Me.cmbEthnic.Location = New System.Drawing.Point(144, 289)
        Me.cmbEthnic.Name = "cmbEthnic"
        Me.cmbEthnic.Size = New System.Drawing.Size(144, 21)
        Me.cmbEthnic.TabIndex = 15
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(16, 289)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(120, 20)
        Me.Label13.TabIndex = 34
        Me.Label13.Text = "Ethnic Background"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(16, 264)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(120, 20)
        Me.Label12.TabIndex = 32
        Me.Label12.Text = "Gender"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(16, 192)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(120, 20)
        Me.Label11.TabIndex = 30
        Me.Label11.Text = "Phone Number"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPhone
        '
        Me.txtPhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhone.Location = New System.Drawing.Point(144, 192)
        Me.txtPhone.MaxLength = 14
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(144, 20)
        Me.txtPhone.TabIndex = 11
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(16, 168)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(120, 20)
        Me.Label10.TabIndex = 28
        Me.Label10.Text = "SSN"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSSN
        '
        Me.txtSSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSSN.Location = New System.Drawing.Point(144, 168)
        Me.txtSSN.MaxLength = 11
        Me.txtSSN.Name = "txtSSN"
        Me.txtSSN.Size = New System.Drawing.Size(144, 20)
        Me.txtSSN.TabIndex = 10
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(16, 144)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(120, 20)
        Me.Label9.TabIndex = 26
        Me.Label9.Text = "Birth Date"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtBirthDate
        '
        Me.txtBirthDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBirthDate.Location = New System.Drawing.Point(144, 144)
        Me.txtBirthDate.Name = "txtBirthDate"
        Me.txtBirthDate.Size = New System.Drawing.Size(144, 20)
        Me.txtBirthDate.TabIndex = 9
        '
        'cmbState
        '
        Me.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbState.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbState.Items.AddRange(New Object() {"", "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY"})
        Me.cmbState.Location = New System.Drawing.Point(384, 120)
        Me.cmbState.Name = "cmbState"
        Me.cmbState.Size = New System.Drawing.Size(48, 21)
        Me.cmbState.TabIndex = 7
        '
        'txtZip
        '
        Me.txtZip.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZip.Location = New System.Drawing.Point(432, 120)
        Me.txtZip.MaxLength = 10
        Me.txtZip.Name = "txtZip"
        Me.txtZip.Size = New System.Drawing.Size(144, 20)
        Me.txtZip.TabIndex = 8
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 120)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 20)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "City, State, Zip"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCity
        '
        Me.txtCity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCity.Location = New System.Drawing.Point(144, 120)
        Me.txtCity.MaxLength = 50
        Me.txtCity.Name = "txtCity"
        Me.txtCity.Size = New System.Drawing.Size(240, 20)
        Me.txtCity.TabIndex = 6
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(16, 96)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 20)
        Me.Label7.TabIndex = 19
        Me.Label7.Text = "Street 2"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStreet2
        '
        Me.txtStreet2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStreet2.Location = New System.Drawing.Point(144, 96)
        Me.txtStreet2.MaxLength = 75
        Me.txtStreet2.Name = "txtStreet2"
        Me.txtStreet2.Size = New System.Drawing.Size(432, 20)
        Me.txtStreet2.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 72)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(120, 20)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Street 1"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStreet1
        '
        Me.txtStreet1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStreet1.Location = New System.Drawing.Point(144, 72)
        Me.txtStreet1.MaxLength = 75
        Me.txtStreet1.Name = "txtStreet1"
        Me.txtStreet1.Size = New System.Drawing.Size(432, 20)
        Me.txtStreet1.TabIndex = 4
        '
        'txtLast
        '
        Me.txtLast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLast.Location = New System.Drawing.Point(384, 24)
        Me.txtLast.MaxLength = 50
        Me.txtLast.Name = "txtLast"
        Me.txtLast.Size = New System.Drawing.Size(192, 20)
        Me.txtLast.TabIndex = 2
        '
        'txtMI
        '
        Me.txtMI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMI.Location = New System.Drawing.Point(344, 24)
        Me.txtMI.MaxLength = 1
        Me.txtMI.Name = "txtMI"
        Me.txtMI.Size = New System.Drawing.Size(40, 20)
        Me.txtMI.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(16, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(120, 20)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Name (F,M,L)"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFirst
        '
        Me.txtFirst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirst.Location = New System.Drawing.Point(144, 24)
        Me.txtFirst.MaxLength = 25
        Me.txtFirst.Name = "txtFirst"
        Me.txtFirst.Size = New System.Drawing.Size(200, 20)
        Me.txtFirst.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label43)
        Me.GroupBox3.Controls.Add(Me.txtAppApprovedComments)
        Me.GroupBox3.Controls.Add(Me.cmbAppInstitution)
        Me.GroupBox3.Controls.Add(Me.Label42)
        Me.GroupBox3.Controls.Add(Me.btnViewApp)
        Me.GroupBox3.Controls.Add(Me.btnLinkApp)
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.txtAppApvd)
        Me.GroupBox3.Controls.Add(Me.Label14)
        Me.GroupBox3.Controls.Add(Me.txtAppRecd)
        Me.GroupBox3.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(17, 449)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(592, 170)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Application Information"
        '
        'Label43
        '
        Me.Label43.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(16, 97)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(120, 34)
        Me.Label43.TabIndex = 51
        Me.Label43.Text = "Approval Comments"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAppApprovedComments
        '
        Me.txtAppApprovedComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAppApprovedComments.Location = New System.Drawing.Point(144, 97)
        Me.txtAppApprovedComments.Multiline = True
        Me.txtAppApprovedComments.Name = "txtAppApprovedComments"
        Me.txtAppApprovedComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAppApprovedComments.Size = New System.Drawing.Size(432, 56)
        Me.txtAppApprovedComments.TabIndex = 3
        '
        'cmbAppInstitution
        '
        Me.cmbAppInstitution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAppInstitution.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAppInstitution.Items.AddRange(New Object() {""})
        Me.cmbAppInstitution.Location = New System.Drawing.Point(144, 72)
        Me.cmbAppInstitution.Name = "cmbAppInstitution"
        Me.cmbAppInstitution.Size = New System.Drawing.Size(216, 21)
        Me.cmbAppInstitution.TabIndex = 2
        '
        'Label42
        '
        Me.Label42.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(16, 72)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(128, 20)
        Me.Label42.TabIndex = 41
        Me.Label42.Text = "Proposed Institution"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnViewApp
        '
        Me.btnViewApp.Location = New System.Drawing.Point(432, 48)
        Me.btnViewApp.Name = "btnViewApp"
        Me.btnViewApp.Size = New System.Drawing.Size(144, 23)
        Me.btnViewApp.TabIndex = 19
        Me.btnViewApp.TabStop = False
        Me.btnViewApp.Text = "View"
        '
        'btnLinkApp
        '
        Me.btnLinkApp.Location = New System.Drawing.Point(432, 24)
        Me.btnLinkApp.Name = "btnLinkApp"
        Me.btnLinkApp.Size = New System.Drawing.Size(144, 23)
        Me.btnLinkApp.TabIndex = 18
        Me.btnLinkApp.TabStop = False
        Me.btnLinkApp.Text = "Link"
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(16, 48)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(120, 20)
        Me.Label15.TabIndex = 16
        Me.Label15.Text = "Approved"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAppApvd
        '
        Me.txtAppApvd.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtAppApvd.Cursor = System.Windows.Forms.Cursors.No
        Me.txtAppApvd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAppApvd.Location = New System.Drawing.Point(144, 48)
        Me.txtAppApvd.Name = "txtAppApvd"
        Me.txtAppApvd.ReadOnly = True
        Me.txtAppApvd.Size = New System.Drawing.Size(144, 20)
        Me.txtAppApvd.TabIndex = 1
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(16, 24)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(120, 20)
        Me.Label14.TabIndex = 14
        Me.Label14.Text = "Received"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAppRecd
        '
        Me.txtAppRecd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAppRecd.Location = New System.Drawing.Point(144, 24)
        Me.txtAppRecd.Name = "txtAppRecd"
        Me.txtAppRecd.Size = New System.Drawing.Size(144, 20)
        Me.txtAppRecd.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label44)
        Me.GroupBox4.Controls.Add(Me.txtACTDate)
        Me.GroupBox4.Controls.Add(Me.txtDistrict)
        Me.GroupBox4.Controls.Add(Me.cmbSchool)
        Me.GroupBox4.Controls.Add(Me.Label21)
        Me.GroupBox4.Controls.Add(Me.txtTranscriptRevd)
        Me.GroupBox4.Controls.Add(Me.Label20)
        Me.GroupBox4.Controls.Add(Me.txtACT)
        Me.GroupBox4.Controls.Add(Me.Label19)
        Me.GroupBox4.Controls.Add(Me.txtGPA)
        Me.GroupBox4.Controls.Add(Me.Label18)
        Me.GroupBox4.Controls.Add(Me.txtGradDate)
        Me.GroupBox4.Controls.Add(Me.Label16)
        Me.GroupBox4.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(17, 629)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(592, 182)
        Me.GroupBox4.TabIndex = 2
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "High School Information"
        '
        'Label44
        '
        Me.Label44.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(16, 120)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(120, 20)
        Me.Label44.TabIndex = 49
        Me.Label44.Text = "ACT Date"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtACTDate
        '
        Me.txtACTDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtACTDate.Location = New System.Drawing.Point(144, 120)
        Me.txtACTDate.Name = "txtACTDate"
        Me.txtACTDate.Size = New System.Drawing.Size(144, 20)
        Me.txtACTDate.TabIndex = 48
        '
        'txtDistrict
        '
        Me.txtDistrict.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtDistrict.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDistrict.Location = New System.Drawing.Point(360, 24)
        Me.txtDistrict.Name = "txtDistrict"
        Me.txtDistrict.ReadOnly = True
        Me.txtDistrict.Size = New System.Drawing.Size(216, 20)
        Me.txtDistrict.TabIndex = 5
        Me.txtDistrict.TabStop = False
        '
        'cmbSchool
        '
        Me.cmbSchool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSchool.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSchool.Items.AddRange(New Object() {""})
        Me.cmbSchool.Location = New System.Drawing.Point(144, 24)
        Me.cmbSchool.Name = "cmbSchool"
        Me.cmbSchool.Size = New System.Drawing.Size(216, 21)
        Me.cmbSchool.TabIndex = 0
        '
        'Label21
        '
        Me.Label21.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(16, 144)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(120, 20)
        Me.Label21.TabIndex = 47
        Me.Label21.Text = "Transcript Received"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTranscriptRevd
        '
        Me.txtTranscriptRevd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTranscriptRevd.Location = New System.Drawing.Point(144, 144)
        Me.txtTranscriptRevd.Name = "txtTranscriptRevd"
        Me.txtTranscriptRevd.Size = New System.Drawing.Size(144, 20)
        Me.txtTranscriptRevd.TabIndex = 4
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(16, 96)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(120, 20)
        Me.Label20.TabIndex = 45
        Me.Label20.Text = "ACT Score"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtACT
        '
        Me.txtACT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtACT.Location = New System.Drawing.Point(144, 96)
        Me.txtACT.MaxLength = 3
        Me.txtACT.Name = "txtACT"
        Me.txtACT.Size = New System.Drawing.Size(144, 20)
        Me.txtACT.TabIndex = 3
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(16, 72)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(120, 20)
        Me.Label19.TabIndex = 43
        Me.Label19.Text = "Cumulatvie GPA"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtGPA
        '
        Me.txtGPA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGPA.Location = New System.Drawing.Point(144, 72)
        Me.txtGPA.Name = "txtGPA"
        Me.txtGPA.Size = New System.Drawing.Size(144, 20)
        Me.txtGPA.TabIndex = 2
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(16, 48)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(120, 20)
        Me.Label18.TabIndex = 41
        Me.Label18.Text = "Graduation Date"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtGradDate
        '
        Me.txtGradDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGradDate.Location = New System.Drawing.Point(144, 48)
        Me.txtGradDate.Name = "txtGradDate"
        Me.txtGradDate.Size = New System.Drawing.Size(144, 20)
        Me.txtGradDate.TabIndex = 1
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(16, 24)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(128, 20)
        Me.Label16.TabIndex = 37
        Me.Label16.Text = "High School / District"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.cmbDegree)
        Me.GroupBox5.Controls.Add(Me.cmbInstitution)
        Me.GroupBox5.Controls.Add(Me.Label26)
        Me.GroupBox5.Controls.Add(Me.txtDegreeTranscript)
        Me.GroupBox5.Controls.Add(Me.Label25)
        Me.GroupBox5.Controls.Add(Me.txtDegreeGPA)
        Me.GroupBox5.Controls.Add(Me.Label24)
        Me.GroupBox5.Controls.Add(Me.txtDegreeCompleted)
        Me.GroupBox5.Controls.Add(Me.Label23)
        Me.GroupBox5.Controls.Add(Me.Label22)
        Me.GroupBox5.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(17, 822)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(592, 158)
        Me.GroupBox5.TabIndex = 3
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Associate's Degree Information"
        '
        'cmbDegree
        '
        Me.cmbDegree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDegree.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDegree.Items.AddRange(New Object() {""})
        Me.cmbDegree.Location = New System.Drawing.Point(144, 48)
        Me.cmbDegree.Name = "cmbDegree"
        Me.cmbDegree.Size = New System.Drawing.Size(216, 21)
        Me.cmbDegree.TabIndex = 1
        '
        'cmbInstitution
        '
        Me.cmbInstitution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbInstitution.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbInstitution.Items.AddRange(New Object() {""})
        Me.cmbInstitution.Location = New System.Drawing.Point(144, 24)
        Me.cmbInstitution.Name = "cmbInstitution"
        Me.cmbInstitution.Size = New System.Drawing.Size(216, 21)
        Me.cmbInstitution.TabIndex = 0
        '
        'Label26
        '
        Me.Label26.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(16, 120)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(120, 20)
        Me.Label26.TabIndex = 47
        Me.Label26.Text = "Transcript Received"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDegreeTranscript
        '
        Me.txtDegreeTranscript.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDegreeTranscript.Location = New System.Drawing.Point(144, 120)
        Me.txtDegreeTranscript.Name = "txtDegreeTranscript"
        Me.txtDegreeTranscript.Size = New System.Drawing.Size(144, 20)
        Me.txtDegreeTranscript.TabIndex = 4
        '
        'Label25
        '
        Me.Label25.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(16, 96)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(120, 20)
        Me.Label25.TabIndex = 45
        Me.Label25.Text = "Cumulative GPA"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDegreeGPA
        '
        Me.txtDegreeGPA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDegreeGPA.Location = New System.Drawing.Point(144, 96)
        Me.txtDegreeGPA.Name = "txtDegreeGPA"
        Me.txtDegreeGPA.Size = New System.Drawing.Size(144, 20)
        Me.txtDegreeGPA.TabIndex = 3
        '
        'Label24
        '
        Me.Label24.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(16, 72)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(120, 20)
        Me.Label24.TabIndex = 43
        Me.Label24.Text = "Degree Completed"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDegreeCompleted
        '
        Me.txtDegreeCompleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDegreeCompleted.Location = New System.Drawing.Point(144, 72)
        Me.txtDegreeCompleted.Name = "txtDegreeCompleted"
        Me.txtDegreeCompleted.Size = New System.Drawing.Size(144, 20)
        Me.txtDegreeCompleted.TabIndex = 2
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(16, 48)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(120, 20)
        Me.Label23.TabIndex = 41
        Me.Label23.Text = "Degree"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label22
        '
        Me.Label22.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(16, 24)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(120, 20)
        Me.Label22.TabIndex = 39
        Me.Label22.Text = "Institution"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.cmbLOAYear)
        Me.GroupBox6.Controls.Add(Me.cmbLOASemester)
        Me.GroupBox6.Controls.Add(Me.GroupBox12)
        Me.GroupBox6.Controls.Add(Me.GroupBox11)
        Me.GroupBox6.Controls.Add(Me.Label47)
        Me.GroupBox6.Controls.Add(Me.Label46)
        Me.GroupBox6.Controls.Add(Me.txtLOARequested)
        Me.GroupBox6.Controls.Add(Me.txtLOAComments)
        Me.GroupBox6.Controls.Add(Me.Label29)
        Me.GroupBox6.Controls.Add(Me.Label28)
        Me.GroupBox6.Controls.Add(Me.txtLOAEnd)
        Me.GroupBox6.Controls.Add(Me.Label27)
        Me.GroupBox6.Controls.Add(Me.txtLOABegin)
        Me.GroupBox6.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(17, 1307)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(592, 292)
        Me.GroupBox6.TabIndex = 6
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Deferral / Leave of Absence"
        '
        'cmbLOAYear
        '
        Me.cmbLOAYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLOAYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLOAYear.Items.AddRange(New Object() {""})
        Me.cmbLOAYear.Location = New System.Drawing.Point(228, 175)
        Me.cmbLOAYear.Name = "cmbLOAYear"
        Me.cmbLOAYear.Size = New System.Drawing.Size(61, 21)
        Me.cmbLOAYear.TabIndex = 6
        '
        'cmbLOASemester
        '
        Me.cmbLOASemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLOASemester.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLOASemester.Items.AddRange(New Object() {"", "Fall", "Winter", "Spring", "Summer"})
        Me.cmbLOASemester.Location = New System.Drawing.Point(144, 175)
        Me.cmbLOASemester.Name = "cmbLOASemester"
        Me.cmbLOASemester.Size = New System.Drawing.Size(80, 21)
        Me.cmbLOASemester.TabIndex = 5
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.rdoLOAApproved)
        Me.GroupBox12.Controls.Add(Me.rdoLOADenied)
        Me.GroupBox12.Location = New System.Drawing.Point(145, 58)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(431, 40)
        Me.GroupBox12.TabIndex = 1
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Status"
        '
        'rdoLOAApproved
        '
        Me.rdoLOAApproved.AutoSize = True
        Me.rdoLOAApproved.Location = New System.Drawing.Point(10, 16)
        Me.rdoLOAApproved.Name = "rdoLOAApproved"
        Me.rdoLOAApproved.Size = New System.Drawing.Size(81, 20)
        Me.rdoLOAApproved.TabIndex = 0
        Me.rdoLOAApproved.TabStop = True
        Me.rdoLOAApproved.Text = "Approved"
        Me.rdoLOAApproved.UseVisualStyleBackColor = True
        '
        'rdoLOADenied
        '
        Me.rdoLOADenied.AutoSize = True
        Me.rdoLOADenied.Location = New System.Drawing.Point(237, 16)
        Me.rdoLOADenied.Name = "rdoLOADenied"
        Me.rdoLOADenied.Size = New System.Drawing.Size(67, 20)
        Me.rdoLOADenied.TabIndex = 1
        Me.rdoLOADenied.TabStop = True
        Me.rdoLOADenied.Text = "Denied"
        Me.rdoLOADenied.UseVisualStyleBackColor = True
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.rdoDeferral)
        Me.GroupBox11.Controls.Add(Me.rdoLOA)
        Me.GroupBox11.Location = New System.Drawing.Point(145, 17)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(431, 40)
        Me.GroupBox11.TabIndex = 0
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Type"
        '
        'rdoDeferral
        '
        Me.rdoDeferral.AutoSize = True
        Me.rdoDeferral.Location = New System.Drawing.Point(9, 16)
        Me.rdoDeferral.Name = "rdoDeferral"
        Me.rdoDeferral.Size = New System.Drawing.Size(75, 20)
        Me.rdoDeferral.TabIndex = 0
        Me.rdoDeferral.TabStop = True
        Me.rdoDeferral.Text = "Deferral"
        Me.rdoDeferral.UseVisualStyleBackColor = True
        '
        'rdoLOA
        '
        Me.rdoLOA.AutoSize = True
        Me.rdoLOA.Location = New System.Drawing.Point(237, 16)
        Me.rdoLOA.Name = "rdoLOA"
        Me.rdoLOA.Size = New System.Drawing.Size(127, 20)
        Me.rdoLOA.TabIndex = 1
        Me.rdoLOA.TabStop = True
        Me.rdoLOA.Text = "Leave of Absence"
        Me.rdoLOA.UseVisualStyleBackColor = True
        '
        'Label47
        '
        Me.Label47.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(16, 175)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(120, 20)
        Me.Label47.TabIndex = 55
        Me.Label47.Text = "Semester Returning"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label46
        '
        Me.Label46.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(16, 103)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(120, 20)
        Me.Label46.TabIndex = 53
        Me.Label46.Text = "Requested"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLOARequested
        '
        Me.txtLOARequested.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLOARequested.Location = New System.Drawing.Point(144, 103)
        Me.txtLOARequested.Name = "txtLOARequested"
        Me.txtLOARequested.Size = New System.Drawing.Size(144, 20)
        Me.txtLOARequested.TabIndex = 2
        '
        'txtLOAComments
        '
        Me.txtLOAComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLOAComments.Location = New System.Drawing.Point(144, 200)
        Me.txtLOAComments.Multiline = True
        Me.txtLOAComments.Name = "txtLOAComments"
        Me.txtLOAComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLOAComments.Size = New System.Drawing.Size(432, 76)
        Me.txtLOAComments.TabIndex = 7
        '
        'Label29
        '
        Me.Label29.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(16, 199)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(120, 20)
        Me.Label29.TabIndex = 47
        Me.Label29.Text = "Comments"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label28
        '
        Me.Label28.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(16, 151)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(120, 20)
        Me.Label28.TabIndex = 45
        Me.Label28.Text = "End"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLOAEnd
        '
        Me.txtLOAEnd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLOAEnd.Location = New System.Drawing.Point(144, 151)
        Me.txtLOAEnd.Name = "txtLOAEnd"
        Me.txtLOAEnd.Size = New System.Drawing.Size(144, 20)
        Me.txtLOAEnd.TabIndex = 4
        '
        'Label27
        '
        Me.Label27.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(16, 127)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(120, 20)
        Me.Label27.TabIndex = 43
        Me.Label27.Text = "Begin"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLOABegin
        '
        Me.txtLOABegin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLOABegin.Location = New System.Drawing.Point(144, 127)
        Me.txtLOABegin.Name = "txtLOABegin"
        Me.txtLOABegin.Size = New System.Drawing.Size(144, 20)
        Me.txtLOABegin.TabIndex = 3
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.cmbEndedReason)
        Me.GroupBox7.Controls.Add(Me.txtLowGPACOmments)
        Me.GroupBox7.Controls.Add(Me.Label30)
        Me.GroupBox7.Controls.Add(Me.txtEligibilityEndedComments)
        Me.GroupBox7.Controls.Add(Me.Label2)
        Me.GroupBox7.Controls.Add(Me.Label1)
        Me.GroupBox7.Controls.Add(Me.txtEligibilityEnded)
        Me.GroupBox7.Controls.Add(Me.chkLowGPA)
        Me.GroupBox7.Controls.Add(Me.Label3)
        Me.GroupBox7.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox7.Location = New System.Drawing.Point(17, 1088)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(592, 208)
        Me.GroupBox7.TabIndex = 5
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Eligibility"
        '
        'cmbEndedReason
        '
        Me.cmbEndedReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEndedReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEndedReason.Items.AddRange(New Object() {""})
        Me.cmbEndedReason.Location = New System.Drawing.Point(288, 24)
        Me.cmbEndedReason.Name = "cmbEndedReason"
        Me.cmbEndedReason.Size = New System.Drawing.Size(288, 21)
        Me.cmbEndedReason.TabIndex = 1
        '
        'txtLowGPACOmments
        '
        Me.txtLowGPACOmments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLowGPACOmments.Location = New System.Drawing.Point(144, 136)
        Me.txtLowGPACOmments.Multiline = True
        Me.txtLowGPACOmments.Name = "txtLowGPACOmments"
        Me.txtLowGPACOmments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLowGPACOmments.Size = New System.Drawing.Size(432, 56)
        Me.txtLowGPACOmments.TabIndex = 4
        '
        'Label30
        '
        Me.Label30.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(16, 136)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(120, 20)
        Me.Label30.TabIndex = 20
        Me.Label30.Text = "Comments"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(824, 80)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(144, 23)
        Me.btnSave.TabIndex = 24
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save"
        '
        'btnApproved
        '
        Me.btnApproved.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApproved.Location = New System.Drawing.Point(824, 112)
        Me.btnApproved.Name = "btnApproved"
        Me.btnApproved.Size = New System.Drawing.Size(144, 23)
        Me.btnApproved.TabIndex = 25
        Me.btnApproved.TabStop = False
        Me.btnApproved.Text = "Approved"
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.GroupBox10)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox4)
        Me.Panel1.Controls.Add(Me.GroupBox5)
        Me.Panel1.Controls.Add(Me.GroupBox3)
        Me.Panel1.Controls.Add(Me.GroupBox7)
        Me.Panel1.Controls.Add(Me.GroupBox6)
        Me.Panel1.Location = New System.Drawing.Point(0, 72)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(800, 536)
        Me.Panel1.TabIndex = 0
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.Label36)
        Me.GroupBox10.Controls.Add(Me.Label35)
        Me.GroupBox10.Controls.Add(Me.cmbBSInstitution)
        Me.GroupBox10.Controls.Add(Me.txtBSDegreeCompleted)
        Me.GroupBox10.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox10.Location = New System.Drawing.Point(17, 990)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(592, 88)
        Me.GroupBox10.TabIndex = 4
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Bachelor's Degree Information"
        '
        'Label36
        '
        Me.Label36.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(16, 48)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(120, 20)
        Me.Label36.TabIndex = 48
        Me.Label36.Text = "Degree Completed"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label35
        '
        Me.Label35.Font = New System.Drawing.Font("Century Schoolbook", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(16, 24)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(120, 20)
        Me.Label35.TabIndex = 47
        Me.Label35.Text = "Institution"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbBSInstitution
        '
        Me.cmbBSInstitution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBSInstitution.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBSInstitution.Items.AddRange(New Object() {""})
        Me.cmbBSInstitution.Location = New System.Drawing.Point(144, 24)
        Me.cmbBSInstitution.Name = "cmbBSInstitution"
        Me.cmbBSInstitution.Size = New System.Drawing.Size(216, 21)
        Me.cmbBSInstitution.TabIndex = 0
        '
        'txtBSDegreeCompleted
        '
        Me.txtBSDegreeCompleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBSDegreeCompleted.Location = New System.Drawing.Point(144, 48)
        Me.txtBSDegreeCompleted.Name = "txtBSDegreeCompleted"
        Me.txtBSDegreeCompleted.Size = New System.Drawing.Size(144, 20)
        Me.txtBSDegreeCompleted.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New System.Drawing.Point(17, 1610)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(752, 8)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        '
        'btnSchedules
        '
        Me.btnSchedules.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSchedules.Location = New System.Drawing.Point(824, 520)
        Me.btnSchedules.Name = "btnSchedules"
        Me.btnSchedules.Size = New System.Drawing.Size(144, 23)
        Me.btnSchedules.TabIndex = 40
        Me.btnSchedules.TabStop = False
        Me.btnSchedules.Text = "Schedules"
        '
        'cmbCommunications
        '
        Me.cmbCommunications.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCommunications.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCommunications.Items.AddRange(New Object() {"Communications", "Add", "All", "Call", "Letter", "Fax", "Email", "Account Maintenance"})
        Me.cmbCommunications.Location = New System.Drawing.Point(824, 584)
        Me.cmbCommunications.Name = "cmbCommunications"
        Me.cmbCommunications.Size = New System.Drawing.Size(144, 24)
        Me.cmbCommunications.TabIndex = 51
        Me.cmbCommunications.TabStop = False
        '
        'btnTransHistory
        '
        Me.btnTransHistory.Font = New System.Drawing.Font("Century Schoolbook", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTransHistory.Location = New System.Drawing.Point(824, 552)
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
        Me.Label33.Text = "Student Information"
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
        'hdrHeader
        '
        Me.hdrHeader.Location = New System.Drawing.Point(-1, -1)
        Me.hdrHeader.Name = "hdrHeader"
        Me.hdrHeader.Size = New System.Drawing.Size(997, 68)
        Me.hdrHeader.TabIndex = 63
        '
        'frmStudents
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(992, 653)
        Me.Controls.Add(Me.hdrHeader)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.cmbCommunications)
        Me.Controls.Add(Me.btnSchedules)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnApproved)
        Me.Controls.Add(Me.btnTransHistory)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1000, 680)
        Me.Name = "frmStudents"
        Me.Text = "Student Information"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private sqlCmd As New SqlClient.SqlCommand
    Private sqlRdr As SqlClient.SqlDataReader
    Private EligEnded As String

#Region "Form Functions"

    Public Overloads Sub Show(ByVal dataSet As DataSet)
        Dim LOA As Boolean
        Dim FiveYrs As Boolean

        'add years to cmbYear list
        Dim i As Integer
        For i = Val(Year(Today)) To Val(Year(Today)) + 5 Step 1
            cmbLOAYear.Items.Add(Trim(Str(i)))
        Next i

        'fill controls
        hdrHeader.txtAccountID.Text = dataSet.Tables(0).Rows(0).Item("AcctID")
        hdrHeader.txtStudentName.Text = dataSet.Tables(0).Rows(0).Item("FName") & " " & dataSet.Tables(0).Rows(0).Item("LName")
        hdrHeader.txtStatus.Text = dataSet.Tables(0).Rows(0).Item("Status")
        hdrHeader.txtBalance.Text = Format(dataSet.Tables(0).Rows(0).Item("Balance"), "$###,##0.00;($###,##0.00)")
        hdrHeader.txtHoursRemaining.Text = Format(dataSet.Tables(0).Rows(0).Item("CredHrRem"), "#0.00;(#0.00)")
        hdrHeader.txtPaidSchedules.Text = GetPaidSchedules(dataSet.Tables(0).Rows(0).Item("AcctID"))

        txtFirst.Text = dataSet.Tables(0).Rows(0).Item("FName")
        txtMI.Text = dataSet.Tables(0).Rows(0).Item("MI")
        txtLast.Text = dataSet.Tables(0).Rows(0).Item("LName")
        txtAltLast.Text = dataSet.Tables(0).Rows(0).Item("AltLName")
        txtStreet1.Text = dataSet.Tables(0).Rows(0).Item("Add1")
        txtStreet2.Text = dataSet.Tables(0).Rows(0).Item("Add2")
        txtCity.Text = dataSet.Tables(0).Rows(0).Item("City")
        cmbState.SelectedItem = dataSet.Tables(0).Rows(0).Item("ST")
        If Len(dataSet.Tables(0).Rows(0).Item("Zip")) > 5 Then
            txtZip.Text = CLng(dataSet.Tables(0).Rows(0).Item("Zip")).ToString("00000-0000")
        Else
            txtZip.Text = dataSet.Tables(0).Rows(0).Item("Zip")
        End If
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("DOB")) Then txtBirthDate.Text = Format(dataSet.Tables(0).Rows(0).Item("DOB"), "MM/dd/yyyy")
        If dataSet.Tables(0).Rows(0).Item("SSN") <> "" Then txtSSN.Text = CLng(dataSet.Tables(0).Rows(0).Item("SSN")).ToString("000-00-0000")
        If dataSet.Tables(0).Rows(0).Item("Phone") <> "" Then txtPhone.Text = CLng(dataSet.Tables(0).Rows(0).Item("Phone")).ToString("(000) 000-0000")
        If dataSet.Tables(0).Rows(0).Item("CellPhone") <> "" Then txtCellPhone.Text = CLng(dataSet.Tables(0).Rows(0).Item("CellPhone")).ToString("(000) 000-0000")
        txtEmail.Text = dataSet.Tables(0).Rows(0).Item("Email")
        cmbGender.SelectedItem = dataSet.Tables(0).Rows(0).Item("Gender")
        cmbEthnic.SelectedItem = dataSet.Tables(0).Rows(0).Item("EthnicBG")

        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("AppRcvDt")) Then txtAppRecd.Text = Format(dataSet.Tables(0).Rows(0).Item("AppRcvDt"), "MM/dd/yyyy")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("AppAprDt")) Then txtAppApvd.Text = Format(dataSet.Tables(0).Rows(0).Item("AppAprDt"), "MM/dd/yyyy")
        cmbAppInstitution.SelectedItem = dataSet.Tables(0).Rows(0).Item("AppInst")
        txtAppApprovedComments.Text = dataSet.Tables(0).Rows(0).Item("AppApvdCom")

        cmbSchool.SelectedItem = dataSet.Tables(0).Rows(0).Item("HSAttended")
        cmbCitizenship.SelectedItem = dataSet.Tables(0).Rows(0).Item("Citizenship")
        cmbCriminalRecord.SelectedItem = dataSet.Tables(0).Rows(0).Item("Criminal")
        txtInfoRelease.Text = dataSet.Tables(0).Rows(0).Item("InfoRelease")
        txtDistrict.Text = dataSet.Tables(0).Rows(0).Item("HSDist")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("HSGradDt")) Then txtGradDate.Text = Format(dataSet.Tables(0).Rows(0).Item("HSGradDt"), "MM/dd/yyyy")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("HSGPA")) Then txtGPA.Text = Format(dataSet.Tables(0).Rows(0).Item("HSGPA"), "0.00")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("ActScore")) Then txtACT.Text = Format(dataSet.Tables(0).Rows(0).Item("ActScore"), "##0")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("ACTDate")) Then txtACTDate.Text = Format(dataSet.Tables(0).Rows(0).Item("ACTDate"), "MM/dd/yyyy")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("OffTranRcvDt")) Then txtTranscriptRevd.Text = Format(dataSet.Tables(0).Rows(0).Item("OffTranRcvDt"), "MM/dd/yyyy")

        cmbInstitution.SelectedItem = dataSet.Tables(0).Rows(0).Item("DegreeInst")
        cmbDegree.SelectedItem = dataSet.Tables(0).Rows(0).Item("Degree")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("DegreeComplDt")) Then txtDegreeCompleted.Text = Format(dataSet.Tables(0).Rows(0).Item("DegreeComplDt"), "MM/dd/yyyy")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("DegreeGPA")) Then txtDegreeGPA.Text = Format(dataSet.Tables(0).Rows(0).Item("DegreeGPA"), "0.00")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("DegreeTranRcvDt")) Then txtDegreeTranscript.Text = Format(dataSet.Tables(0).Rows(0).Item("DegreeTranRcvDt"), "MM/dd/yyyy")

        cmbBSInstitution.SelectedItem = dataSet.Tables(0).Rows(0).Item("BSInst")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("BSDate")) Then txtBSDegreeCompleted.Text = Format(dataSet.Tables(0).Rows(0).Item("BSDate"), "MM/dd/yyyy")

        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("EligEndDt")) Then
            txtEligibilityEnded.Text = Format(dataSet.Tables(0).Rows(0).Item("EligEndDt"), "MM/dd/yyyy")
            EligEnded = Format(dataSet.Tables(0).Rows(0).Item("EligEndDt"), "MM/dd/yyyy")
        Else
            EligEnded = ""
        End If
        cmbEndedReason.SelectedItem = dataSet.Tables(0).Rows(0).Item("EligEndRea")
        txtEligibilityEndedComments.Text = dataSet.Tables(0).Rows(0).Item("EligEndCom")
        chkLowGPA.Checked = dataSet.Tables(0).Rows(0).Item("LowGPA")
        If dataSet.Tables(0).Rows(0).Item("LowGPA") = True Then chkLowGPA.Enabled = False
        txtLowGPACOmments.Text = dataSet.Tables(0).Rows(0).Item("LowGPACom")

        rdoDeferral.Checked = CBool(dataSet.Tables(0).Rows(0).Item("LOADeferral"))
        rdoLOA.Checked = CBool(dataSet.Tables(0).Rows(0).Item("LOALOA"))
        rdoLOAApproved.Checked = CBool(dataSet.Tables(0).Rows(0).Item("LOAApproved"))
        rdoLOADenied.Checked = CBool(dataSet.Tables(0).Rows(0).Item("LOADenied"))
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("LOARequested")) Then txtLOARequested.Text = Format(dataSet.Tables(0).Rows(0).Item("LOARequested"), "MM/dd/yyyy")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("LOAStart")) Then txtLOABegin.Text = Format(dataSet.Tables(0).Rows(0).Item("LOAStart"), "MM/dd/yyyy")
        If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("LOAEnd")) Then txtLOAEnd.Text = Format(dataSet.Tables(0).Rows(0).Item("LOAEnd"), "MM/dd/yyyy")
        cmbLOASemester.SelectedItem = dataSet.Tables(0).Rows(0).Item("LOASemReturn")
        cmbLOAYear.SelectedItem = CStr(dataSet.Tables(0).Rows(0).Item("LOAYearReturn"))
        txtLOAComments.Text = dataSet.Tables(0).Rows(0).Item("LOACom")
        If txtLOAEnd.Text <> "" Then
            If DateValue(txtLOAEnd.Text) > Today And rdoLOAApproved.Checked Then LOA = True Else LOA = False
        Else
            LOA = False
        End If

        'lock controls for editing
        If (dataSet.Tables(0).Rows(0).Item("LockedBy") <> "" And dataSet.Tables(0).Rows(0).Item("LockedBy") <> UserID) Or UserAccess = "Read Only Access" Or LOA Then
            txtFirst.Enabled = False
            txtMI.Enabled = False
            txtLast.Enabled = False
            txtAltLast.Enabled = False
            txtStreet1.Enabled = False
            txtStreet2.Enabled = False
            txtCity.Enabled = False
            cmbState.Enabled = False
            txtZip.Enabled = False
            txtBirthDate.Enabled = False
            txtSSN.Enabled = False
            txtPhone.Enabled = False
            txtEmail.Enabled = False
            cmbGender.Enabled = False
            cmbEthnic.Enabled = False
            txtCellPhone.Enabled = False
            cmbCitizenship.Enabled = False
            cmbCriminalRecord.Enabled = False
            txtInfoRelease.Enabled = False

            txtAppRecd.Enabled = False
            txtAppApvd.BackColor = SystemColors.Control
            txtAppApvd.ForeColor = SystemColors.ControlDark
            cmbAppInstitution.Enabled = False
            txtAppApprovedComments.Enabled = False

            cmbSchool.Enabled = False
            txtDistrict.BackColor = SystemColors.Control
            txtDistrict.ForeColor = SystemColors.ControlDark
            txtGradDate.Enabled = False
            txtGPA.Enabled = False
            txtACT.Enabled = False
            txtACTDate.Enabled = False
            txtTranscriptRevd.Enabled = False

            cmbInstitution.Enabled = False
            cmbDegree.Enabled = False
            txtDegreeCompleted.Enabled = False
            txtDegreeGPA.Enabled = False
            txtDegreeTranscript.Enabled = False

            cmbBSInstitution.Enabled = False
            txtBSDegreeCompleted.Enabled = False

            txtEligibilityEnded.Enabled = False
            cmbEndedReason.Enabled = False
            txtEligibilityEndedComments.ReadOnly = True
            txtEligibilityEndedComments.BackColor = SystemColors.Control
            txtEligibilityEndedComments.ForeColor = SystemColors.ControlDark

            chkLowGPA.Enabled = False
            txtLowGPACOmments.ReadOnly = True
            txtLowGPACOmments.BackColor = SystemColors.Control
            txtLowGPACOmments.ForeColor = SystemColors.ControlDark

            If Not LOA Then
                rdoDeferral.Enabled = False
                rdoLOA.Enabled = False
                rdoLOAApproved.Enabled = False
                rdoLOADenied.Enabled = False
                txtLOARequested.Enabled = False
                txtLOABegin.Enabled = False
                txtLOAEnd.Enabled = False
                cmbLOASemester.Enabled = False
                cmbLOAYear.Enabled = False
                txtLOAComments.ReadOnly = True
                txtLOAComments.BackColor = SystemColors.Control
                txtLOAComments.ForeColor = SystemColors.ControlDark
                btnSave.Enabled = False
            End If

            If (dataSet.Tables(0).Rows(0).Item("LockedBy") <> "" And dataSet.Tables(0).Rows(0).Item("LockedBy") <> UserID) Or UserAccess = "Read Only Access" Then
                btnLinkApp.Enabled = False 'still allow docs to be linked if only on LOA and access isn't a factor
            End If
            btnApproved.Enabled = False

            Me.WindowState = FormWindowState.Maximized
            Me.Show()

            If LOA Then
                MsgBox("The student has a current leave of absence .", MsgBoxStyle.Information, "New Century Scholarship Program")
            ElseIf UserAccess <> "Read Only Access" Then
                MsgBox("The record is locked by user " & dataSet.Tables(0).Rows(0).Item("LockedBy") & ".", MsgBoxStyle.Information, "New Century Scholarship Program")
            End If
        Else
            'lock record
            dbConnection.Open()
            sqlCmd.CommandText = "UPDATE Account SET LockedBy = '" & UserID & "' WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A'"
            sqlCmd.ExecuteNonQuery()
            dbConnection.Close()

            'determine if 5-year eligibility has expired
            If txtGradDate.Text <> "" And hdrHeader.txtStatus.Text <> "Closed" Then
                If DateValue(txtGradDate.Text).AddYears(5) < Today() Then
                    FiveYrs = True
                    hdrHeader.txtStatus.Text = "Closed"
                    txtEligibilityEnded.Text = Format(DateAdd(DateInterval.Year, 5, DateValue(txtGradDate.Text)), "MM/dd/yyyy")
                    cmbEndedReason.SelectedItem = "Time Elapsed"
                    SaveRecord(hdrHeader.txtStatus.Text)
                Else
                    FiveYrs = False
                End If
            Else
                FiveYrs = False
            End If

            'disable approved button if not in pending status
            If hdrHeader.txtStatus.Text <> "Pending" Then btnApproved.Enabled = False

            'maximize and display windows
            Me.WindowState = FormWindowState.Maximized
            Me.Show()

        End If

        'prompt user if account closed because 5-year eligibility has expired
        If FiveYrs Then PrintNotatePromptOnCloseAccount("Time Elapsed", hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text, hdrHeader.txtBalance.Text)

    End Sub

    'unlock record
    Private Sub frmStudents_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        sqlCmd.CommandText = "UPDATE Account SET LockedBy = '' WHERE LockedBy = '" & UserID & "'"
        sqlCmd.ExecuteNonQuery()
        dbConnection.Close()
    End Sub

#End Region

#Region "Processing Buttons"

    'save the record
    Sub SaveRecord(ByVal acctStatus As String)
        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'update current record to historical
        sqlCmd.CommandText = "UPDATE Account SET RowStatus = 'H', LockedBy = '' WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A'"
        sqlCmd.ExecuteNonQuery()

        sqlCmd.CommandText = CreateInsertCommandStringForDB(acctStatus)
        sqlCmd.ExecuteNonQuery()

        dbConnection.Close()
    End Sub

    'creates the insert string for DB
    Private Function CreateInsertCommandStringForDB(ByVal acctStatus As String) As String
        Dim rawPhone As String
        Dim rawCellPhone As String
        Dim lowGPA As Integer
        Dim fieldList As New StringBuilder()
        Dim valueList As New StringBuilder()

        hdrHeader.txtStudentName.Text = txtFirst.Text & " " & txtLast.Text
        hdrHeader.txtStatus.Text = acctStatus

        'format phone
        rawPhone = Replace(txtPhone.Text, "-", "")
        rawPhone = Replace(rawPhone, " ", "")
        rawPhone = Replace(rawPhone, "(", "")
        rawPhone = Replace(rawPhone, ")", "")

        'format cell phone
        rawCellPhone = Replace(txtCellPhone.Text, "-", "")
        rawCellPhone = Replace(rawCellPhone, " ", "")
        rawCellPhone = Replace(rawCellPhone, "(", "")
        rawCellPhone = Replace(rawCellPhone, ")", "")

        If chkLowGPA.Checked Then lowGPA = 1 Else lowGPA = 0

        'create both value list and field list for db update

        'account ID
        valueList.AppendFormat("'{0}',", hdrHeader.txtAccountID.Text)
        fieldList.Append("AcctID,")

        'status
        valueList.AppendFormat("'{0}',", Replace(acctStatus, "'", "''"))
        fieldList.Append("Status,")

        'balance
        valueList.AppendFormat("{0},", CDbl(hdrHeader.txtBalance.Text))
        fieldList.Append("Balance,")

        'CredHrRem
        valueList.AppendFormat("{0},", CDbl(hdrHeader.txtHoursRemaining.Text))
        fieldList.Append("CredHrRem,")

        'FName
        valueList.AppendFormat("'{0}',", Replace(txtFirst.Text, "'", "''"))
        fieldList.Append("FName,")

        'MI
        valueList.AppendFormat("'{0}',", txtMI.Text)
        fieldList.Append("MI,")

        'LName
        valueList.AppendFormat("'{0}',", Replace(txtLast.Text, "'", "''"))
        fieldList.Append("LName,")

        'AltLName
        valueList.AppendFormat("'{0}',", Replace(txtAltLast.Text, "'", "''"))
        fieldList.Append("AltLName,")

        'Add1
        valueList.AppendFormat("'{0}',", Replace(txtStreet1.Text, "'", "''"))
        fieldList.Append("Add1,")

        'Add2
        valueList.AppendFormat("'{0}',", Replace(txtStreet2.Text, "'", "''"))
        fieldList.Append("Add2,")

        'City
        valueList.AppendFormat("'{0}',", Replace(txtCity.Text, "'", "''"))
        fieldList.Append("City,")

        'ST
        valueList.AppendFormat("'{0}',", cmbState.SelectedItem)
        fieldList.Append("ST,")

        'Zip
        valueList.AppendFormat("'{0}',", Replace(txtZip.Text, "-", ""))
        fieldList.Append("Zip,")

        'SSN
        valueList.AppendFormat("'{0}',", Replace(txtSSN.Text, "-", ""))
        fieldList.Append("SSN,")

        'Phone
        valueList.AppendFormat("'{0}',", rawPhone)
        fieldList.Append("Phone,")

        'CellPhone
        valueList.AppendFormat("'{0}',", rawCellPhone)
        fieldList.Append("CellPhone,")

        'Email
        valueList.AppendFormat("'{0}',", Replace(txtEmail.Text, "'", "''"))
        fieldList.Append("Email,")

        'Gender
        valueList.AppendFormat("'{0}',", Replace(cmbGender.SelectedItem, "'", "''"))
        fieldList.Append("Gender,")

        'EthnicBG
        valueList.AppendFormat("'{0}',", Replace(cmbEthnic.SelectedItem, "'", "''"))
        fieldList.Append("EthnicBG,")

        'HSAttended
        valueList.AppendFormat("'{0}',", Replace(cmbSchool.SelectedItem, "'", "''"))
        fieldList.Append("HSAttended,")

        'HSDist
        valueList.AppendFormat("'{0}',", Replace(txtDistrict.Text, "'", "''"))
        fieldList.Append("HSDist,")

        'DegreeInst
        valueList.AppendFormat("'{0}',", Replace(cmbInstitution.SelectedItem, "'", "''"))
        fieldList.Append("DegreeInst,")

        'Degree
        valueList.AppendFormat("'{0}',", Replace(cmbDegree.SelectedItem, "'", "''"))
        fieldList.Append("Degree,")

        'EligEndRea
        valueList.AppendFormat("'{0}',", Replace(cmbEndedReason.SelectedItem, "'", "''"))
        fieldList.Append("EligEndRea,")

        'EligEndCom
        valueList.AppendFormat("'{0}',", Replace(txtEligibilityEndedComments.Text, "'", "''"))
        fieldList.Append("EligEndCom,")

        'LowGPA
        valueList.AppendFormat("{0},", lowGPA)
        fieldList.Append("LowGPA,")

        'LowGPACom
        valueList.AppendFormat("'{0}',", Replace(txtLowGPACOmments.Text, "'", "''"))
        fieldList.Append("LowGPACom,")

        'LOACom
        valueList.AppendFormat("'{0}',", Replace(txtLOAComments.Text, "'", "''"))
        fieldList.Append("LOACom,")

        'LastUpdateDt
        valueList.Append("GETDATE(),")
        fieldList.Append("LastUpdateDt,")

        'LastUpdateUser
        valueList.AppendFormat("'{0}',", UserID)
        fieldList.Append("LastUpdateUser,")

        'RowStatus
        valueList.Append("'A',")
        fieldList.Append("RowStatus,")

        'LockedBy
        valueList.AppendFormat("'{0}',", UserID)
        fieldList.Append("LockedBy,")

        'BSInst
        valueList.AppendFormat("'{0}',", Replace(cmbBSInstitution.SelectedItem, "'", "''"))
        fieldList.Append("BSInst,")

        'Criminal
        valueList.AppendFormat("'{0}',", Replace(cmbCriminalRecord.SelectedItem, "'", "''"))
        fieldList.Append("Criminal,")

        'InfoRelease
        valueList.AppendFormat("'{0}',", Replace(txtInfoRelease.Text, "'", "''"))
        fieldList.Append("InfoRelease,")

        'AppInst
        valueList.AppendFormat("'{0}',", cmbAppInstitution.SelectedItem)
        fieldList.Append("AppInst,")

        'AppApvdCom
        valueList.AppendFormat("'{0}',", txtAppApprovedComments.Text.Replace("'", "''"))
        fieldList.Append("AppApvdCom,")

        'LOADeferral
        valueList.AppendFormat("{0},", If(rdoDeferral.Checked, 1, 0))
        fieldList.Append("LOADeferral,")

        'LOALOA
        valueList.AppendFormat("{0},", If(rdoLOA.Checked, 1, 0))
        fieldList.Append("LOALOA,")

        'LOAApproved
        valueList.AppendFormat("{0},", If(rdoLOAApproved.Checked, 1, 0))
        fieldList.Append("LOAApproved,")

        'LOADenied
        valueList.AppendFormat("{0},", If(rdoLOADenied.Checked, 1, 0))
        fieldList.Append("LOADenied,")


        'Citizenship
        valueList.AppendFormat("'{0}'", cmbCitizenship.SelectedItem)
        fieldList.Append("Citizenship")

        'add null values to SQL

        'DOB
        If txtBirthDate.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtBirthDate.Text)
            fieldList.Append(", DOB")
        End If

        'AppRcvDt
        If txtAppRecd.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtAppRecd.Text)
            fieldList.Append(", AppRcvDt")
        End If

        'AppAprDt
        If txtAppApvd.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtAppApvd.Text)
            fieldList.Append(", AppAprDt")
        End If

        'HSGradDt
        If txtGradDate.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtGradDate.Text)
            fieldList.Append(", HSGradDt")
        End If

        'HSGPA
        If txtGPA.Text <> "" Then
            valueList.AppendFormat(", '{0}'", CDbl(txtGPA.Text).ToString())
            fieldList.Append(", HSGPA")
        End If

        'ActScore
        If txtACT.Text <> "" Then
            valueList.AppendFormat(", '{0}'", Val(txtACT.Text).ToString())
            fieldList.Append(", ActScore")
        End If

        'OffTranRcvDt
        If txtTranscriptRevd.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtTranscriptRevd.Text)
            fieldList.Append(", OffTranRcvDt")
        End If

        'DegreeComplDt
        If txtDegreeCompleted.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtDegreeCompleted.Text)
            fieldList.Append(", DegreeComplDt")
        End If

        'DegreeGPA
        If txtDegreeGPA.Text <> "" Then
            valueList.AppendFormat(", '{0}'", CDbl(txtDegreeGPA.Text).ToString())
            fieldList.Append(", DegreeGPA")
        End If

        'DegreeTranRcvDt
        If txtDegreeTranscript.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtDegreeTranscript.Text)
            fieldList.Append(", DegreeTranRcvDt")
        End If

        'EligEndDt
        If txtEligibilityEnded.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtEligibilityEnded.Text)
            fieldList.Append(", EligEndDt")
        End If

        'LOAStart
        If txtLOABegin.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtLOABegin.Text)
            fieldList.Append(", LOAStart")
        End If

        'LOAEnd
        If txtLOAEnd.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtLOAEnd.Text)
            fieldList.Append(", LOAEnd")
        End If

        'LOARequested
        If txtLOARequested.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtLOARequested.Text)
            fieldList.Append(", LOARequested")
        End If

        'SemesterReturn
        If cmbLOASemester.SelectedItem <> "" Then
            valueList.AppendFormat(", '{0}'", cmbLOASemester.SelectedItem)
            fieldList.Append(", LOASemReturn")
        End If

        'SemesterReturn
        If cmbLOAYear.SelectedItem <> "" Then
            valueList.AppendFormat(", {0}", cmbLOAYear.SelectedItem)
            fieldList.Append(", LOAYearReturn")
        End If

        'BSDate
        If txtBSDegreeCompleted.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtBSDegreeCompleted.Text)
            fieldList.Append(", BSDate")
        End If

        'ACTDate
        If txtACTDate.Text <> "" Then
            valueList.AppendFormat(", '{0}'", txtACTDate.Text)
            fieldList.Append(", ACTDate")
        End If

        'run SQL
        Return "INSERT INTO Account (" & fieldList.ToString() & ") VALUES (" & valueList.ToString() & ")"
    End Function

    'save record
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim closeIt As Boolean = False
        Dim IsNewlyReceived As Boolean = False

        'warn user if eligibility ended reason was left blank if the eligibility ended date is populated
        If txtEligibilityEnded.Text <> "" And cmbEndedReason.SelectedItem = "" Then
            MsgBox("The eligibility ended reason may not be left blank if the eligibility ended date is populated.", MsgBoxStyle.Exclamation, "Eligibility Ended Reason Required")
            Exit Sub
        End If

        'update status to closed if eligibility ended date is populated
        If txtEligibilityEnded.Text <> "" And hdrHeader.txtStatus.Text <> "Closed" Then
            hdrHeader.txtStatus.Text = "Closed"
            btnApproved.Enabled = False
            closeIt = True
        End If

        'process override
        If chkLowGPA.Checked = True Then

            'warn user if comment is blank
            If txtLowGPACOmments.Text = "" Then
                MsgBox("The low GPA override has been checked but no comment was entered.  Enter a comment and then click Save again.", MsgBoxStyle.Exclamation, "Comment Needed")
                Exit Sub
            ElseIf cmbEndedReason.SelectedItem = "Low GPA" Then

                'blank eligibility ended fields, update status, and disable override
                txtEligibilityEnded.Text = ""
                cmbEndedReason.SelectedItem = ""
                txtEligibilityEndedComments.Text = ""
                hdrHeader.txtStatus.Text = "Approved"
                chkLowGPA.Enabled = False

                'add communications record
                AddCommunications(hdrHeader.txtAccountID.Text, Now(), True, False, "Account Maintenance", "low GPA ineligibility overridden - " & txtLowGPACOmments.Text, "Low GPA Ineligibility Overridden")
            End If

        End If

        'determine if the application received date is newly entered
        If txtAppRecd.Text <> "" Then
            sqlCmd.Connection = dbConnection
            dbConnection.Open()

            sqlCmd.CommandText = "SELECT AppRcvDt FROM Account WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A'"
            sqlRdr = sqlCmd.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows OrElse IsDBNull(sqlRdr("AppRcvDt")) Then
                IsNewlyReceived = True
            End If
            sqlRdr.Close()
            dbConnection.Close()
        End If

        'close the account
        If closeIt Then
            'save record and close the account
            SaveRecord(hdrHeader.txtStatus.Text)
            PrintNotatePromptOnCloseAccount(cmbEndedReason.SelectedItem, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text, hdrHeader.txtBalance.Text)
            'print letter and prompt user if the received date was newly populated
        ElseIf IsNewlyReceived Then
            Dim deadLine As Date

            'determine application deadline
            If Month(Now) > 10 Then
                deadLine = CDate("02/01/" & CStr(Year(Now) + 1))
            Else
                deadLine = CDate("02/01/" & CStr(Year(Now)))
            End If

            'send application recevied letter if received on time
            If CDate(txtAppRecd.Text).Date <= deadLine.Date Then
                'save record
                SaveRecord(hdrHeader.txtStatus.Text)

                'create data file
                FileOpen(1, "T:\NCSPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
                WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP")
                WriteLine(1, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text)
                FileClose(1)

                'print letter, add a communication record, and prompt user
                Dim ComDt As String = Now()
                Dim TransID As String = Format(DateValue(ComDt), "MMddyyyy") & Format(TimeValue(ComDt), "HHmmss")
                PrintDoc("NCSAPPRECD", hdrHeader.txtAccountID.Text & "\Communications", TransID, False)
                AddCommunications(hdrHeader.txtAccountID.Text, ComDt, False, True, "Letter", "Application Received " & txtAppRecd.Text & ", Confirmation Letter Sent " & Format(DateValue(Now()), "MMddyyyy"), "Application Received and Confirmation Letter Sent")
                MsgBox("Student information saved and Application Received Confirmation letter printed.  Please retrieve the letter from the printer.", MsgBoxStyle.Information, "Saved")
                'send denial letter if received late
            Else
                'change status and eligibility ended info
                hdrHeader.txtStatus.Text = "Closed"
                txtEligibilityEnded.Text = Format(DateValue(Today()), "MM/dd/yyyy")
                cmbEndedReason.SelectedItem = "Application Received Late"
                btnApproved.Enabled = False

                'save record
                SaveRecord(hdrHeader.txtStatus.Text)

                'create data file
                FileOpen(1, "T:\NCSPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
                WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP")
                WriteLine(1, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text)
                FileClose(1)

                'print letter, add a communication record, and prompt user
                Dim ComDt As String = Now()
                Dim TransID As String = Format(DateValue(ComDt), "MMddyyyy") & Format(TimeValue(ComDt), "HHmmss")
                PrintDoc("NCSAPPLATE", hdrHeader.txtAccountID.Text & "\Communications", TransID, False)
                AddCommunications(hdrHeader.txtAccountID.Text, ComDt, False, True, "Letter", "Application received " & txtAppRecd.Text & " after the deadline, denied letter sent " & Format(DateValue(Now()), "MMddyyyy"), "Application Denied Letter Sent")
                MsgBox("Student information saved.  The application was received after the deadline.  Application Denied letter printed.  Please retrieve the letter from the printer.", MsgBoxStyle.Information, "Saved")
            End If
            'prompt user if the received date was not newly populated
        Else
            'save record and prompt user
            SaveRecord(hdrHeader.txtStatus.Text)
            MsgBox("Student information saved.", MsgBoxStyle.Information, "Saved")
        End If

    End Sub

    'approve application
    Private Sub btnApproved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApproved.Click
        Dim ComDt As String
        Dim TransID As String
        Dim dateLength As String = txtGradDate.Text.Length
        Dim gradYear As Integer = 0
        If txtGradDate.Text <> String.Empty Then
            gradYear = CInt(txtGradDate.Text.Substring(dateLength - 4, 4))
        End If

        Dim missingData As Boolean = False

        'warn user if required fields are missing
        If Mid(cmbSchool.SelectedItem, 1, 14) <> "Home Schooling" AndAlso cmbSchool.Text <> String.Empty Then
            If txtFirst.Text = "" Or txtLast.Text = "" Or txtStreet1.Text = "" Or txtCity.Text = "" Or cmbState.SelectedItem = "" Or txtZip.Text = "" Or txtBirthDate.Text = "" Or txtSSN.Text = "" Or txtPhone.Text = "" Or cmbGender.SelectedItem = "" Or txtAppRecd.Text = "" Or cmbSchool.SelectedItem = "" Or txtGradDate.Text = "" Or txtTranscriptRevd.Text = "" Or cmbInstitution.SelectedItem = "" Or cmbDegree.SelectedItem = "" Or txtDegreeCompleted.Text = "" Or txtDegreeGPA.Text = "" Or cmbCitizenship.SelectedItem = "" Or cmbCriminalRecord.SelectedItem = "" Or txtGPA.Text = "" Then
                MsgBox("Information required to approve the application is missing.  Required information includes all fields except the following:  middle initial, street 2, e-mail, gender, ethnic background, ACT/SAT score, ACT/SAT date, and information in the bachelor's degree, eligibility, and leave of absence sections.", MsgBoxStyle.Exclamation, "Missing Information")
                missingData = True
                If MsgBox("Do you want to continue running the approval", MsgBoxStyle.YesNo, "Run Approval?") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        Else
            If txtFirst.Text = "" Or txtLast.Text = "" Or txtStreet1.Text = "" Or txtCity.Text = "" Or cmbState.SelectedItem = "" Or txtZip.Text = "" Or txtBirthDate.Text = "" Or txtSSN.Text = "" Or txtPhone.Text = "" Or cmbGender.SelectedItem = "" Or txtAppRecd.Text = "" Or cmbSchool.SelectedItem = "" Or txtGradDate.Text = "" Or txtTranscriptRevd.Text = "" Or cmbInstitution.SelectedItem = "" Or cmbDegree.SelectedItem = "" Or txtDegreeCompleted.Text = "" Or txtDegreeGPA.Text = "" Or cmbCitizenship.SelectedItem = "" Or cmbCriminalRecord.SelectedItem = "" Or txtGPA.Text = "" Or txtACT.Text = "" Or txtACTDate.Text = "" Then
                MsgBox("Information required to approve the application for a home schooled student is missing.  Required information includes all fields except the following:  middle initial, street 2, e-mail, gender, ethnic background, and information in the bachelor's degree, eligibility, and leave of absence sections.", MsgBoxStyle.Exclamation, "Missing Information")
                missingData = True
                If MsgBox("Do you want to continue running the approval", MsgBoxStyle.YesNo, "Run Approval?") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        End If

        If txtGPA.Text <> String.Empty Then
            'For the year 2011 and greater, GPA must be above 3.5
            If CDbl(txtGPA.Text) < 3.5 And gradYear > 2010 Then
                MsgBox("The high school GPA entered is less than 3.5.", MsgBoxStyle.Exclamation, "Ineligible Student")
                'save record and close the account
                hdrHeader.txtStatus.Text = "Closed"
                cmbEndedReason.Text = "Denied - Low High School GPA"
                txtEligibilityEnded.Text = Date.Now.ToShortDateString()
                SaveRecord(hdrHeader.txtStatus.Text)
                PrintNotatePromptOnCloseAccount(cmbEndedReason.Text, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text, hdrHeader.txtBalance.Text)
                btnApproved.Enabled = False
                Exit Sub
            End If
        End If

        If txtDegreeGPA.Text <> String.Empty Then
            'For grad year 2011 and greater, College GPA must be above 3.0
            If CDbl(txtDegreeGPA.Text) < 3 Then
                If gradYear = 0 OrElse gradYear > 2010 Then
                    MsgBox("The College GPA entered is less than 3.0.", MsgBoxStyle.Exclamation, "Ineligible Student")
                    'save record and close the account
                    hdrHeader.txtStatus.Text = "Closed"
                    cmbEndedReason.Text = "Denied - Low AS GPA"
                    txtEligibilityEnded.Text = Date.Now.ToShortDateString()
                    SaveRecord(hdrHeader.txtStatus.Text)
                    PrintNotatePromptOnCloseAccount(cmbEndedReason.Text, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text, hdrHeader.txtBalance.Text)
                    btnApproved.Enabled = False
                    Exit Sub
                End If
            End If
        End If

        'completed date is after the high school graduation date
        If Mid(cmbSchool.SelectedItem, 1, 14) <> "Home Schooling" AndAlso cmbSchool.Text <> String.Empty Then
            If txtDegreeCompleted.Text <> String.Empty AndAlso txtGradDate.Text <> String.Empty Then
                If DateValue(txtDegreeCompleted.Text) > DateValue(txtGradDate.Text) Then
                    MsgBox("The associate's degree completed date is after the high school graduation date.", MsgBoxStyle.Exclamation, "Ineligible Student")
                    'save record and close the account
                    hdrHeader.txtStatus.Text = "Closed"
                    cmbEndedReason.Text = "Denied - AS not completed by High School Grad Date"
                    txtEligibilityEnded.Text = Date.Now.ToShortDateString()
                    SaveRecord(hdrHeader.txtStatus.Text)
                    PrintNotatePromptOnCloseAccount(cmbEndedReason.Text, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text, hdrHeader.txtBalance.Text)
                    btnApproved.Enabled = False
                    Exit Sub
                End If
            End If
            'home schooled student edits
        Else
            'completed date is after June 15 of the year the home schooled student would have graduated from high school
            If txtDegreeCompleted.Text <> String.Empty Then
                If DateValue(txtDegreeCompleted.Text) > DateSerial(Year(Now), 6, 15) Then
                    MsgBox("The associate's degree completed date is after June 15 of the year the student would have graduated from high school.", MsgBoxStyle.Exclamation, "Ineligible Student")
                    'save record and close the account
                    hdrHeader.txtStatus.Text = "Closed"
                    cmbEndedReason.Text = "Denied - AS not completed by High School Grad Date"
                    txtEligibilityEnded.Text = Date.Now.ToShortDateString()
                    SaveRecord(hdrHeader.txtStatus.Text)
                    PrintNotatePromptOnCloseAccount(cmbEndedReason.Text, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text, hdrHeader.txtBalance.Text)
                    btnApproved.Enabled = False
                    Exit Sub
                End If
            End If
            'ACT taken after June 15 of the year the home schooled student would have graduated from high school
            If txtACTDate.Text <> String.Empty Then
                If DateValue(txtACTDate.Text) > DateSerial(Year(Now), 6, 15) Then
                    MsgBox("The ACT/SAT date entered is after June 15 of the year the student would have graduated from high school.", MsgBoxStyle.Exclamation, "Ineligible Student")
                    'save record and close the account
                    hdrHeader.txtStatus.Text = "Closed"
                    cmbEndedReason.Text = "Denied - ACT Test Taken after June 15th"
                    txtEligibilityEnded.Text = Date.Now.ToShortDateString()
                    SaveRecord(hdrHeader.txtStatus.Text)
                    PrintNotatePromptOnCloseAccount(cmbEndedReason.Text, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text, hdrHeader.txtBalance.Text)
                    btnApproved.Enabled = False
                    Exit Sub
                End If
            End If
            'For grad year 2011 and greater, home school students must have greater than 26 ACT scores
            If txtACT.Text <> String.Empty Then
                If CInt(txtACT.Text) < 26 And gradYear > 2010 Then
                    MsgBox("The ACT score is less than 26 or the SAT score is less than 700.", MsgBoxStyle.Exclamation, "Ineligible Student")
                    'save record and close the account
                    hdrHeader.txtStatus.Text = "Closed"
                    cmbEndedReason.Text = "Denied - ACT Test Score < 26"
                    txtEligibilityEnded.Text = Date.Now.ToShortDateString()
                    SaveRecord(hdrHeader.txtStatus.Text)
                    PrintNotatePromptOnCloseAccount(cmbEndedReason.Text, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text, hdrHeader.txtBalance.Text)
                    btnApproved.Enabled = False
                    Exit Sub
                End If
            End If
        End If

        If missingData Then
            MsgBox("Account not denied. Please provide the missing data to complete the approval", MsgBoxStyle.OkOnly, "Provide Missing Data")
            Exit Sub
        End If

        'process approval
        If txtAppApvd.Text = "" Then
            'update approved date
            txtAppApvd.Text = Format(Today, "MM/dd/yyyy")

            ComDt = Now()
            TransID = Format(DateValue(ComDt), "MMddyyyy") & Format(TimeValue(ComDt), "HHmmss")

            'print letter
            FileOpen(1, "T:\NCSPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP")
            WriteLine(1, hdrHeader.txtAccountID.Text, txtFirst.Text, txtLast.Text, txtStreet1.Text, txtStreet2.Text, txtCity.Text, cmbState.SelectedItem, txtZip.Text)
            FileClose(1)
            PrintDoc("NCSAWARD", hdrHeader.txtAccountID.Text & "\Communications", TransID)

            'add communications record
            AddCommunications(hdrHeader.txtAccountID.Text, ComDt, False, True, "Letter", "application approved and award letter sent", "Application Approved, NCSP Awarded Letter Sent")
        End If

        'save record and prompt user
        SaveRecord("Approved")
        MsgBox("Student information approved and saved.  Please retrieve the NCSP Awarded letter from the printer.", MsgBoxStyle.Information, "Approved")
    End Sub

    'link app document
    Private Sub btnLinkApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLinkApp.Click
        Dim FileOpener As New OpenFileDialog
        Dim NewFile As String

        'prompt user for file
        FileOpener.ShowDialog()
        If FileOpener.FileName = "" Then Exit Sub
        NewFile = DocPath & hdrHeader.txtAccountID.Text & "\Apps\" & Dir(FileOpener.FileName, FileAttribute.Hidden)

        'warn if file already exists
        If File.Exists(NewFile) Then
            MsgBox("A file with the name selected already exists for the student.  Change the file name and try again.", MsgBoxStyle.Exclamation, "File Already Exists")
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

    'view app document
    Private Sub btnViewApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewApp.Click
        Dim FileOpener As New OpenFileDialog

        'prompt user to select file
        FileOpener.InitialDirectory = DocPath & hdrHeader.txtAccountID.Text & "\Apps\"
        FileOpener.ShowDialog()

        'display file if one was selected
        If FileOpener.FileName <> "" Then
            Process.Start(FileOpener.FileName)
        End If
    End Sub
#End Region

#Region "Control Events"
    'verify and format data

    Private Sub txtZip_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtZip.Validating
        Dim rawZip As String

        rawZip = Replace(txtZip.Text, "-", "")
        If (Not IsNumeric(rawZip) Or (Len(rawZip) <> 5 And Len(rawZip) <> 9)) And txtZip.Text <> "" Then
            MsgBox("Invalid zip code entered.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtZip.Text <> "" Then
            If Len(rawZip) = 9 Then txtZip.Text = CLng(rawZip).ToString("#####-####")
        End If
    End Sub

    Private Sub txtBirthDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtBirthDate.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtSSN_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtSSN.Validating
        Dim rawSSN As String

        rawSSN = Replace(txtSSN.Text, "-", "")
        If (Not IsNumeric(rawSSN) Or Len(rawSSN) <> 9) And txtSSN.Text <> "" Then
            MsgBox("Invalid SSN entered.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtSSN.Text <> "" Then
            txtSSN.Text = rawSSN.Insert(3, "-").Insert(6, "-")
        End If
    End Sub

    Private Sub txtPhone_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPhone.Validating
        Dim rawPhone As String

        rawPhone = Replace(txtPhone.Text, "-", "")
        rawPhone = Replace(rawPhone, " ", "")
        rawPhone = Replace(rawPhone, "(", "")
        rawPhone = Replace(rawPhone, ")", "")

        If (Not IsNumeric(rawPhone) Or Len(rawPhone) <> 10) And txtPhone.Text <> "" Then
            MsgBox("Invalid phone number entered.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtPhone.Text <> "" Then
            txtPhone.Text = CLng(rawPhone).ToString("(###) ###-####")
        End If
    End Sub

    Private Sub txtCellPhone_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCellPhone.Validating
        Dim rawPhone As String

        rawPhone = Replace(txtCellPhone.Text, "-", "")
        rawPhone = Replace(rawPhone, " ", "")
        rawPhone = Replace(rawPhone, "(", "")
        rawPhone = Replace(rawPhone, ")", "")

        If (Not IsNumeric(rawPhone) Or Len(rawPhone) <> 10) And txtCellPhone.Text <> "" Then
            MsgBox("Invalid phone number entered.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtCellPhone.Text <> "" Then
            txtCellPhone.Text = CLng(rawPhone).ToString("(###) ###-####")
        End If
    End Sub

    Private Sub txtEmail_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtEmail.Validating
        If (InStr(1, txtEmail.Text, "@") = 0 Or InStr(1, txtEmail.Text, ".") = 0) And txtEmail.Text <> "" Then
            MsgBox("The e-mail address must contain an '@' and a '.'.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        End If
    End Sub

    Private Sub txtAppRecd_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtAppRecd.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtAppApvd_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtAppApvd.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub cmbSchool_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSchool.SelectedIndexChanged
        If cmbSchool.SelectedItem <> "" Then
            sqlCmd.Connection = dbConnection
            dbConnection.Open()

            sqlCmd.CommandText = "SELECT B.HSName, A.DistName FROM District A INNER JOIN HighSchl B ON A.DistID = B.DistID WHERE B.HSName = '" & cmbSchool.SelectedItem & "'"
            sqlRdr = sqlCmd.ExecuteReader
            sqlRdr.Read()
            txtDistrict.Text = sqlRdr("DistName")
            sqlRdr.Close()
            dbConnection.Close()
        End If
    End Sub

    Private Sub txtGradDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtGradDate.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
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

    Private Sub txtACT_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtACT.Validating
        If Not IsNumeric(txtACT.Text) And txtACT.Text <> "" Then
            MsgBox("Numeric data required.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtACT.Text <> "" Then
            txtACT.Text = Format(Val(txtACT.Text), "##0")
        End If
    End Sub

    Private Sub txtACTDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtACTDate.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtTranscriptRevd_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtTranscriptRevd.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtDegreeCompleted_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDegreeCompleted.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtDegreeGPA_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDegreeGPA.Validating
        If Not IsNumeric(txtDegreeGPA.Text) And txtDegreeGPA.Text <> "" Then
            MsgBox("Numeric data required.", MsgBoxStyle.Exclamation, "Invalid Data")
            e.Cancel = True
        ElseIf txtDegreeGPA.Text <> "" Then
            txtDegreeGPA.Text = Format(CDbl(txtDegreeGPA.Text), "0.00")
        End If
    End Sub

    Private Sub txtDegreeTranscript_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDegreeTranscript.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtBSDegreeCompleted_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtBSDegreeCompleted.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtEligibilityEnded_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtEligibilityEnded.Validating
        If txtEligibilityEnded.Text = "" And EligEnded <> "" Then
            If MsgBox("The eligibility ended date has been blanked out.  Do you want to reopen the account?  Click Yes to delete the eligibility ended reason and comments and update the account status to 'Pending' (the application will need to be reapproved to save the changes and allow schedules to be added and paid).", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Reopen Account") = MsgBoxResult.Yes Then
                cmbEndedReason.SelectedItem = ""
                txtEligibilityEndedComments.Text = ""
                hdrHeader.txtStatus.Text = "Pending"
                btnApproved.Enabled = True
            Else
                e.Cancel = True
                If sender.CanUndo = True Then
                    sender.Undo()
                    sender.ClearUndo()
                End If
            End If
        ElseIf Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtLOABegin_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtLOABegin.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Private Sub txtLOAEnd_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtLOAEnd.Validating
        If Not IsValidDate(sender) Then
            sender.Select(0, sender.Text.Length)
            e.Cancel = True
        End If
    End Sub

    Function IsValidDate(ByVal dtControl As Control) As Boolean
        IsValidDate = True

        If IsDate(dtControl.Text) Then
            dtControl.Text = Format(DateValue(dtControl.Text), "MM/dd/yyyy")
        ElseIf Not IsDBNull(dtControl.Text) And dtControl.Text <> "" Then
            MsgBox("Invalid date entered.", MsgBoxStyle.Exclamation, "Invalid Data")
            IsValidDate = False
        End If
    End Function

#End Region

#Region "Navigation Buttons"

    Private Sub btnSchedules_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSchedules.Click
        Dim ShowEm As Boolean = True
        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'determine the schedule to display or prompt the user to add a new schedule if there aren't any
        sqlCmd.CommandText = "SELECT * FROM Schedule WHERE AcctID = '" & hdrHeader.txtAccountID.Text & "' AND RowStatus = 'A'"
        sqlRdr = sqlCmd.ExecuteReader
        If Not sqlRdr.HasRows Then
            sqlRdr.Close()
            If UserAccess = "Read Only Access" Or hdrHeader.txtStatus.Text <> "Approved" Then
                MsgBox("There are no schedules to display.", MsgBoxStyle.Information, "No Schedules to Display")
                ShowEm = False
            Else
                If MsgBox("There are no schedules to display.  Do you want to add a new schedule?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Add Schedule") = MsgBoxResult.Yes Then
                    sqlCmd.CommandText = "INSERT INTO Schedule (AcctID, SchedID, SchedHrRem, LastUpdateUser) VALUES ('" & hdrHeader.txtAccountID.Text & "', 1, " & CDbl(hdrHeader.txtHoursRemaining.Text) & ", '" & UserID & "')"
                    sqlCmd.ExecuteNonQuery()
                Else
                    ShowEm = False
                End If
            End If
        Else
            sqlRdr.Close()
        End If

        dbConnection.Close()

        'display schedule 
        If ShowEm Then ShowForms.Schedules()
    End Sub

    'display transaction history
    Private Sub btnTransHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransHistory.Click
        ShowForms.TransHistory()
    End Sub

    'display communications records selected
    Private Sub cmbCommunications_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCommunications.SelectedIndexChanged
        Dim FilterStr As String

        If cmbCommunications.SelectedItem <> "Communications" Then
            FilterStr = cmbCommunications.SelectedItem
            cmbCommunications.Text = "Communications"
            ShowForms.Communications(FilterStr)
        End If
    End Sub
#End Region

#Region "Public Functions"
    'pass student info to calling form
    Public Sub StuInfo(ByRef AccountID As String, ByRef StuName As String, ByRef StuStatus As String, ByRef StuBalance As String, ByRef StuHours As String, ByRef StuPaidSchedules As String)
        AccountID = hdrHeader.txtAccountID.Text
        StuName = hdrHeader.txtStudentName.Text
        StuStatus = hdrHeader.txtStatus.Text
        StuBalance = hdrHeader.txtBalance.Text
        StuHours = hdrHeader.txtHoursRemaining.Text
        StuPaidSchedules = hdrHeader.txtPaidSchedules.Text
    End Sub

    'update student info with info from calling form
    Public Sub UpdatedbySchedule(Optional ByVal Bal As Double = 0, Optional ByVal Hrs As Double = 0, Optional ByVal Sta As String = "", Optional ByVal EligDate As String = "", Optional ByVal EligReason As String = "")
        If Bal <> 0 Then hdrHeader.txtBalance.Text = Format(CDbl(hdrHeader.txtBalance.Text) + Bal, "$###,##0.00;($###,##0.00)")
        If Hrs <> 0 Then hdrHeader.txtHoursRemaining.Text = Format(CDbl(hdrHeader.txtHoursRemaining.Text) - Hrs, "#0.00;(#0.00)")
        If Sta <> "" Then hdrHeader.txtStatus.Text = Sta
        If EligDate <> "" Then txtEligibilityEnded.Text = EligDate
        If EligReason <> "" Then cmbEndedReason.SelectedItem = EligReason

        SaveRecord(hdrHeader.txtStatus.Text)
    End Sub

    'pass account status to calling form
    Public Function AccountStatus() As String
        AccountStatus = hdrHeader.txtStatus.Text
    End Function

    'pass transcript recieved date to calling form
    Public Function TransRevdDt() As String
        TransRevdDt = txtDegreeTranscript.Text
    End Function

    'pass low GPA override value to calling form
    Public Function LowGPAOV() As Boolean
        LowGPAOV = chkLowGPA.Checked
    End Function
#End Region

    '****************experimental code, may be useful some day************************
    ''Function KeyPresses(ByVal kyControl As Control, ByVal kyPressed As Char)
    ''If e.KeyChar = Microsoft.VisualBasic.ChrW(27) Then
    ''    If sender.CanUndo = True Then
    ''        ' Undo the last operation.
    ''        sender.Undo()
    ''        ' Clear the undo buffer to prevent last action from being redone.
    ''        sender.ClearUndo()
    ''    End If
    ''End If
    ''If kyPressed = Microsoft.VisualBasic.ChrW(27) Then
    ''    If kyControl.CanUndo = True Then
    ''        ' Undo the last operation.
    ''        kyControl.Undo()
    ''        ' Clear the undo buffer to prevent last action from being redone.
    ''        kyControl.ClearUndo()
    ''    End If
    ''End If
    ''End Function

    ''Private Sub txtLOAEnd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLOAEnd.KeyPress
    ''    KeyPresses(sender, e.KeyChar)
    ''    If e.KeyChar = Microsoft.VisualBasic.ChrW(27) Then
    ''        If sender.CanUndo = True Then
    ''            ' Undo the last operation.
    ''            sender.Undo()
    ''            ' Clear the undo buffer to prevent last action from being redone.
    ''            sender.ClearUndo()
    ''        End If
    ''    ElseIf e.KeyChar = Microsoft.VisualBasic.ChrW(13) Then

    ''    End If

    ''End Sub


    Private Sub rdoLOAApproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoLOAApproved.CheckedChanged
        txtLOABegin.Enabled = True
        txtLOAEnd.Enabled = True
        cmbLOASemester.Enabled = True
        cmbLOAYear.Enabled = True
    End Sub

    Private Sub rdoLOADenied_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoLOADenied.CheckedChanged
        txtLOABegin.Enabled = False
        txtLOABegin.Text = ""
        txtLOAEnd.Enabled = False
        txtLOAEnd.Text = ""
        cmbLOASemester.Enabled = False
        cmbLOASemester.SelectedItem = ""
        cmbLOAYear.Enabled = False
        cmbLOAYear.SelectedItem = ""
    End Sub

End Class

