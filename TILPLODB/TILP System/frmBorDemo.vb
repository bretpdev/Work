Imports System.Data.SqlClient

Public Class frmBorDemo
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByRef tBor As borrower, ByVal tTestMode As Boolean, ByRef tTheUser As user, ByRef tTILPMain As frmTILPMain)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        selBor = tBor
        TestMode = tTestMode
        TheUser = tTheUser
        TILPMain = tTILPMain


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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tbSSN As System.Windows.Forms.TextBox
    Friend WithEvents tbFN As System.Windows.Forms.TextBox
    Friend WithEvents tbLN As System.Windows.Forms.TextBox
    Friend WithEvents tbAddr1 As System.Windows.Forms.TextBox
    Friend WithEvents tbAddr2 As System.Windows.Forms.TextBox
    Friend WithEvents tbCity As System.Windows.Forms.TextBox
    Friend WithEvents tbZip As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tbDOB As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbAddVal As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents tbHPhn As System.Windows.Forms.TextBox
    Friend WithEvents tbAltPhn As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents tbEmail As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents gbAll As System.Windows.Forms.GroupBox
    Friend WithEvents btSaveAndContinue As System.Windows.Forms.Button
    Friend WithEvents btCan As System.Windows.Forms.Button
    Friend WithEvents gbAdd As System.Windows.Forms.GroupBox
    Friend WithEvents gbPhone As System.Windows.Forms.GroupBox
    Friend WithEvents gbOth As System.Windows.Forms.GroupBox
    Friend WithEvents cbAPV As System.Windows.Forms.CheckBox
    Friend WithEvents cbHPV As System.Windows.Forms.CheckBox
    Friend WithEvents tbPnoteSig As System.Windows.Forms.TextBox
    Friend WithEvents cbChanges As System.Windows.Forms.CheckBox
    Friend WithEvents tbMI As System.Windows.Forms.TextBox
    Friend WithEvents cboxST As System.Windows.Forms.ComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents cboxGender As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents cboxEth As System.Windows.Forms.ComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cbInst As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBorDemo))
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbSSN = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.tbFN = New System.Windows.Forms.TextBox
        Me.tbLN = New System.Windows.Forms.TextBox
        Me.tbAddr1 = New System.Windows.Forms.TextBox
        Me.tbAddr2 = New System.Windows.Forms.TextBox
        Me.tbCity = New System.Windows.Forms.TextBox
        Me.cboxST = New System.Windows.Forms.ComboBox
        Me.tbZip = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.tbDOB = New System.Windows.Forms.TextBox
        Me.gbAdd = New System.Windows.Forms.GroupBox
        Me.cbAddVal = New System.Windows.Forms.CheckBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.tbMI = New System.Windows.Forms.TextBox
        Me.gbPhone = New System.Windows.Forms.GroupBox
        Me.tbAltPhn = New System.Windows.Forms.TextBox
        Me.tbHPhn = New System.Windows.Forms.TextBox
        Me.cbAPV = New System.Windows.Forms.CheckBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.cbHPV = New System.Windows.Forms.CheckBox
        Me.gbOth = New System.Windows.Forms.GroupBox
        Me.cbInst = New System.Windows.Forms.ComboBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.cboxEth = New System.Windows.Forms.ComboBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.cboxGender = New System.Windows.Forms.ComboBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.tbEmail = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.tbPnoteSig = New System.Windows.Forms.TextBox
        Me.cbChanges = New System.Windows.Forms.CheckBox
        Me.gbAll = New System.Windows.Forms.GroupBox
        Me.btSaveAndContinue = New System.Windows.Forms.Button
        Me.btCan = New System.Windows.Forms.Button
        Me.gbAdd.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.gbPhone.SuspendLayout()
        Me.gbOth.SuspendLayout()
        Me.gbAll.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SSN"
        '
        'tbSSN
        '
        Me.tbSSN.Location = New System.Drawing.Point(36, 16)
        Me.tbSSN.MaxLength = 9
        Me.tbSSN.Name = "tbSSN"
        Me.tbSSN.Size = New System.Drawing.Size(120, 20)
        Me.tbSSN.TabIndex = 0
        Me.tbSSN.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(184, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "First Name"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(456, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Last Name"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(88, 16)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Street Address 1"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 44)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 16)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Street Address 2"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 68)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 16)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "City"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 92)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(32, 16)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "State"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 116)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(32, 16)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Zip"
        '
        'tbFN
        '
        Me.tbFN.Location = New System.Drawing.Point(244, 16)
        Me.tbFN.MaxLength = 13
        Me.tbFN.Name = "tbFN"
        Me.tbFN.Size = New System.Drawing.Size(124, 20)
        Me.tbFN.TabIndex = 1
        Me.tbFN.Text = ""
        '
        'tbLN
        '
        Me.tbLN.Location = New System.Drawing.Point(520, 16)
        Me.tbLN.MaxLength = 20
        Me.tbLN.Name = "tbLN"
        Me.tbLN.Size = New System.Drawing.Size(192, 20)
        Me.tbLN.TabIndex = 3
        Me.tbLN.Text = ""
        '
        'tbAddr1
        '
        Me.tbAddr1.Location = New System.Drawing.Point(96, 16)
        Me.tbAddr1.MaxLength = 30
        Me.tbAddr1.Name = "tbAddr1"
        Me.tbAddr1.Size = New System.Drawing.Size(268, 20)
        Me.tbAddr1.TabIndex = 0
        Me.tbAddr1.Text = ""
        '
        'tbAddr2
        '
        Me.tbAddr2.Location = New System.Drawing.Point(96, 40)
        Me.tbAddr2.MaxLength = 30
        Me.tbAddr2.Name = "tbAddr2"
        Me.tbAddr2.Size = New System.Drawing.Size(268, 20)
        Me.tbAddr2.TabIndex = 1
        Me.tbAddr2.Text = ""
        '
        'tbCity
        '
        Me.tbCity.Location = New System.Drawing.Point(96, 64)
        Me.tbCity.MaxLength = 20
        Me.tbCity.Name = "tbCity"
        Me.tbCity.Size = New System.Drawing.Size(268, 20)
        Me.tbCity.TabIndex = 2
        Me.tbCity.Text = ""
        '
        'cboxST
        '
        Me.cboxST.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboxST.Items.AddRange(New Object() {"", "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY"})
        Me.cboxST.Location = New System.Drawing.Point(96, 88)
        Me.cboxST.Name = "cboxST"
        Me.cboxST.Size = New System.Drawing.Size(40, 21)
        Me.cboxST.TabIndex = 3
        '
        'tbZip
        '
        Me.tbZip.Location = New System.Drawing.Point(96, 112)
        Me.tbZip.MaxLength = 9
        Me.tbZip.Name = "tbZip"
        Me.tbZip.TabIndex = 4
        Me.tbZip.Text = ""
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(740, 20)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(68, 16)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Date of Birth"
        '
        'tbDOB
        '
        Me.tbDOB.Location = New System.Drawing.Point(808, 16)
        Me.tbDOB.MaxLength = 10
        Me.tbDOB.Name = "tbDOB"
        Me.tbDOB.Size = New System.Drawing.Size(108, 20)
        Me.tbDOB.TabIndex = 4
        Me.tbDOB.Text = "MM/DD/YYYY"
        '
        'gbAdd
        '
        Me.gbAdd.Controls.Add(Me.cbAddVal)
        Me.gbAdd.Controls.Add(Me.Label10)
        Me.gbAdd.Controls.Add(Me.Label8)
        Me.gbAdd.Controls.Add(Me.tbCity)
        Me.gbAdd.Controls.Add(Me.Label4)
        Me.gbAdd.Controls.Add(Me.tbAddr2)
        Me.gbAdd.Controls.Add(Me.Label7)
        Me.gbAdd.Controls.Add(Me.Label5)
        Me.gbAdd.Controls.Add(Me.cboxST)
        Me.gbAdd.Controls.Add(Me.tbAddr1)
        Me.gbAdd.Controls.Add(Me.Label6)
        Me.gbAdd.Controls.Add(Me.tbZip)
        Me.gbAdd.Location = New System.Drawing.Point(8, 52)
        Me.gbAdd.Name = "gbAdd"
        Me.gbAdd.Size = New System.Drawing.Size(372, 172)
        Me.gbAdd.TabIndex = 1
        Me.gbAdd.TabStop = False
        Me.gbAdd.Text = "Address"
        '
        'cbAddVal
        '
        Me.cbAddVal.Location = New System.Drawing.Point(96, 132)
        Me.cbAddVal.Name = "cbAddVal"
        Me.cbAddVal.Size = New System.Drawing.Size(16, 24)
        Me.cbAddVal.TabIndex = 5
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 140)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(76, 16)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "Address Valid"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.tbDOB)
        Me.GroupBox2.Controls.Add(Me.tbFN)
        Me.GroupBox2.Controls.Add(Me.tbLN)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.tbSSN)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.tbMI)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(924, 40)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(392, 20)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(20, 16)
        Me.Label12.TabIndex = 21
        Me.Label12.Text = "MI"
        '
        'tbMI
        '
        Me.tbMI.Location = New System.Drawing.Point(412, 16)
        Me.tbMI.MaxLength = 1
        Me.tbMI.Name = "tbMI"
        Me.tbMI.Size = New System.Drawing.Size(16, 20)
        Me.tbMI.TabIndex = 2
        Me.tbMI.Text = ""
        '
        'gbPhone
        '
        Me.gbPhone.Controls.Add(Me.tbAltPhn)
        Me.gbPhone.Controls.Add(Me.tbHPhn)
        Me.gbPhone.Controls.Add(Me.cbAPV)
        Me.gbPhone.Controls.Add(Me.Label14)
        Me.gbPhone.Controls.Add(Me.Label15)
        Me.gbPhone.Controls.Add(Me.Label11)
        Me.gbPhone.Controls.Add(Me.Label13)
        Me.gbPhone.Controls.Add(Me.cbHPV)
        Me.gbPhone.Location = New System.Drawing.Point(380, 52)
        Me.gbPhone.Name = "gbPhone"
        Me.gbPhone.Size = New System.Drawing.Size(244, 172)
        Me.gbPhone.TabIndex = 2
        Me.gbPhone.TabStop = False
        Me.gbPhone.Text = "Phone"
        '
        'tbAltPhn
        '
        Me.tbAltPhn.Location = New System.Drawing.Point(128, 52)
        Me.tbAltPhn.MaxLength = 10
        Me.tbAltPhn.Name = "tbAltPhn"
        Me.tbAltPhn.Size = New System.Drawing.Size(108, 20)
        Me.tbAltPhn.TabIndex = 2
        Me.tbAltPhn.Text = ""
        '
        'tbHPhn
        '
        Me.tbHPhn.Location = New System.Drawing.Point(128, 16)
        Me.tbHPhn.MaxLength = 10
        Me.tbHPhn.Name = "tbHPhn"
        Me.tbHPhn.Size = New System.Drawing.Size(108, 20)
        Me.tbHPhn.TabIndex = 0
        Me.tbHPhn.Text = ""
        '
        'cbAPV
        '
        Me.cbAPV.Location = New System.Drawing.Point(128, 72)
        Me.cbAPV.Name = "cbAPV"
        Me.cbAPV.Size = New System.Drawing.Size(16, 24)
        Me.cbAPV.TabIndex = 3
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(8, 60)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(100, 16)
        Me.Label14.TabIndex = 20
        Me.Label14.Text = "Alternate Phone"
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(8, 80)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(116, 16)
        Me.Label15.TabIndex = 21
        Me.Label15.Text = "Alternate Phone Valid"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(8, 20)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(100, 16)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Home Phone"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(8, 40)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(100, 16)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "Home Phone Valid"
        '
        'cbHPV
        '
        Me.cbHPV.Location = New System.Drawing.Point(128, 32)
        Me.cbHPV.Name = "cbHPV"
        Me.cbHPV.Size = New System.Drawing.Size(16, 24)
        Me.cbHPV.TabIndex = 1
        '
        'gbOth
        '
        Me.gbOth.Controls.Add(Me.cbInst)
        Me.gbOth.Controls.Add(Me.Label21)
        Me.gbOth.Controls.Add(Me.cboxEth)
        Me.gbOth.Controls.Add(Me.Label19)
        Me.gbOth.Controls.Add(Me.cboxGender)
        Me.gbOth.Controls.Add(Me.Label18)
        Me.gbOth.Controls.Add(Me.Label17)
        Me.gbOth.Controls.Add(Me.tbEmail)
        Me.gbOth.Controls.Add(Me.Label16)
        Me.gbOth.Controls.Add(Me.tbPnoteSig)
        Me.gbOth.Location = New System.Drawing.Point(628, 52)
        Me.gbOth.Name = "gbOth"
        Me.gbOth.Size = New System.Drawing.Size(304, 172)
        Me.gbOth.TabIndex = 3
        Me.gbOth.TabStop = False
        Me.gbOth.Text = "Other Info"
        '
        'cbInst
        '
        Me.cbInst.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbInst.Location = New System.Drawing.Point(108, 124)
        Me.cbInst.Name = "cbInst"
        Me.cbInst.Size = New System.Drawing.Size(188, 21)
        Me.cbInst.TabIndex = 4
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(8, 128)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(104, 16)
        Me.Label21.TabIndex = 9
        Me.Label21.Text = "Awarding Institution"
        '
        'cboxEth
        '
        Me.cboxEth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboxEth.Items.AddRange(New Object() {"Unspecific", "Caucasian", "African American", "Hispanic/Latino", "Asian/Pacific Islander", "American Indian/Alaskan Native", "Other"})
        Me.cboxEth.Location = New System.Drawing.Point(108, 68)
        Me.cboxEth.Name = "cboxEth"
        Me.cboxEth.Size = New System.Drawing.Size(188, 21)
        Me.cboxEth.TabIndex = 2
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(8, 72)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(52, 16)
        Me.Label19.TabIndex = 5
        Me.Label19.Text = "Ethnicity"
        '
        'cboxGender
        '
        Me.cboxGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboxGender.Items.AddRange(New Object() {"M", "F"})
        Me.cboxGender.Location = New System.Drawing.Point(108, 40)
        Me.cboxGender.Name = "cboxGender"
        Me.cboxGender.Size = New System.Drawing.Size(44, 21)
        Me.cboxGender.TabIndex = 1
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(8, 44)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(44, 16)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = "Gender"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(8, 100)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(100, 16)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "Date Pnote Signed"
        '
        'tbEmail
        '
        Me.tbEmail.Location = New System.Drawing.Point(108, 16)
        Me.tbEmail.MaxLength = 200
        Me.tbEmail.Name = "tbEmail"
        Me.tbEmail.Size = New System.Drawing.Size(188, 20)
        Me.tbEmail.TabIndex = 0
        Me.tbEmail.Text = ""
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(8, 20)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(36, 16)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Email"
        '
        'tbPnoteSig
        '
        Me.tbPnoteSig.Location = New System.Drawing.Point(108, 96)
        Me.tbPnoteSig.MaxLength = 10
        Me.tbPnoteSig.Name = "tbPnoteSig"
        Me.tbPnoteSig.Size = New System.Drawing.Size(104, 20)
        Me.tbPnoteSig.TabIndex = 3
        Me.tbPnoteSig.Text = "MM/DD/YYYY"
        '
        'cbChanges
        '
        Me.cbChanges.Location = New System.Drawing.Point(4, 8)
        Me.cbChanges.Name = "cbChanges"
        Me.cbChanges.Size = New System.Drawing.Size(140, 16)
        Me.cbChanges.TabIndex = 1
        Me.cbChanges.Text = "Change Demographics"
        '
        'gbAll
        '
        Me.gbAll.Controls.Add(Me.gbAdd)
        Me.gbAll.Controls.Add(Me.gbPhone)
        Me.gbAll.Controls.Add(Me.GroupBox2)
        Me.gbAll.Controls.Add(Me.gbOth)
        Me.gbAll.Location = New System.Drawing.Point(16, 28)
        Me.gbAll.Name = "gbAll"
        Me.gbAll.Size = New System.Drawing.Size(940, 232)
        Me.gbAll.TabIndex = 2
        Me.gbAll.TabStop = False
        '
        'btSaveAndContinue
        '
        Me.btSaveAndContinue.Location = New System.Drawing.Point(340, 268)
        Me.btSaveAndContinue.Name = "btSaveAndContinue"
        Me.btSaveAndContinue.Size = New System.Drawing.Size(120, 23)
        Me.btSaveAndContinue.TabIndex = 4
        Me.btSaveAndContinue.Text = "Save and Continue"
        '
        'btCan
        '
        Me.btCan.Location = New System.Drawing.Point(476, 268)
        Me.btCan.Name = "btCan"
        Me.btCan.Size = New System.Drawing.Size(120, 23)
        Me.btCan.TabIndex = 5
        Me.btCan.Text = "Cancel"
        '
        'frmBorDemo
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(976, 303)
        Me.ControlBox = False
        Me.Controls.Add(Me.btCan)
        Me.Controls.Add(Me.btSaveAndContinue)
        Me.Controls.Add(Me.gbAll)
        Me.Controls.Add(Me.cbChanges)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(984, 330)
        Me.MinimumSize = New System.Drawing.Size(984, 330)
        Me.Name = "frmBorDemo"
        Me.Text = "Borrower Demographics"
        Me.gbAdd.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.gbPhone.ResumeLayout(False)
        Me.gbOth.ResumeLayout(False)
        Me.gbAll.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private selBor As borrower
    Private TestMode As Boolean
    Private TheUser As user
    Private Conn As SqlConnection
    Private Comm As SqlCommand
    Private BorRefs As frmBorRefs
    Private TILPMain As frmTILPMain

    Private Sub frmBorDemo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DA As SqlDataAdapter
        Dim DS As New DataSet
        Dim i As Integer

        'Set up DB connection and data adapter for Awarding Instituion control
        If TestMode Then
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
            DA = New Data.SqlClient.SqlDataAdapter("SELECT DISTINCT SchoolName FROM ParticipatingSchoolsList WHERE Valid = 1 ORDER BY SchoolName", "Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
            DA = New Data.SqlClient.SqlDataAdapter("SELECT DISTINCT SchoolName FROM ParticipatingSchoolsList WHERE Valid = 1 ORDER BY SchoolName", "Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If

        Comm = New SqlCommand
        Comm.Connection = Conn

        'Fill data sets for control population
        DA.Fill(DS, "Schools")

        'Add hard coded value for State Office of Education
        cbInst.Items.Add("State Office of Education")
        'Populate the rest of the Awarding Instituion control
        While i < DS.Tables("Schools").Rows.Count
            cbInst.Items.Add(DS.Tables("Schools").Rows(i).Item("SchoolName"))
            i = i + 1
        End While

        ' If the borrower is a new borrower NewAcct will be true  
        If selBor.NewAcct = True Then
            cbChanges.Enabled = False
            btSaveAndContinue.Text = "Continue"
        Else
            disableAll()
            btSaveAndContinue.Text = "Save"

            'required fields
            tbSSN.Text = selBor.getKey()
            tbFN.Text = selBor.FN
            tbLN.Text = selBor.LN
            tbAddr1.Text = selBor.BorDemo.Addr1
            tbCity.Text = selBor.BorDemo.City
            cboxST.Text = selBor.BorDemo.ST
            tbZip.Text = selBor.BorDemo.Zip
            tbDOB.Text = selBor.DOB
            If selBor.PromDtSig <> "01/01/1900" Then 'date may be blank in DB so don't cahnge if it is
                tbPnoteSig.Text = selBor.PromDtSig
            End If
            cbAddVal.Checked = selBor.BorDemo.AddValid
            cbInst.SelectedItem = selBor.AwardInst
            'optional fields
            tbMI.Text = selBor.MI
            tbAddr2.Text = selBor.BorDemo.Addr2
            tbEmail.Text = selBor.BorDemo.Email
            tbHPhn.Text = selBor.BorDemo.hPhone
            tbAltPhn.Text = selBor.BorDemo.aPhone
            cbHPV.Checked = selBor.BorDemo.hPhoneValid
            cbAPV.Checked = selBor.BorDemo.aPhoneValid
            cboxGender.Text = selBor.BorDemo.Gender
            cboxEth.Text = selBor.BorDemo.Ethnicity
            'if access level is 4 then don't allow any updates
            If TheUser.GetAccessLevel() = 4 Then
                cbChanges.Enabled = False
                btSaveAndContinue.Enabled = False
            End If
        End If
    End Sub

    Private Sub btCan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCan.Click
        If selBor.NewAcct = True Then
            TILPMain.AcctSelection = New frmSelection(TestMode, TILPMain, TheUser)
            TILPMain.AcctSelection.MdiParent = TILPMain
            TILPMain.AcctSelection.Show()
            Me.Close()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub btSaveAndContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSaveAndContinue.Click
        'make sure all data is in proper format
        If demoDatCheck() Then
            If selBor.NewAcct = True Then 'New borrower - insert data into the db
                'check and make sure the SSN doesn't already exist
                If dupKey(tbSSN.Text) <> 0 Then
                    MessageBox.Show("A borrower with this SSN already exists in the TILP System. Please enter a different SSN.", "SSN Conflict", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                    Exit Sub
                End If
                'load info to borrower object
                iNoobie()
                'create borrower reference form and close demographic form
                TILPMain.BorRefs = New frmBorRefs(selBor, TestMode, TheUser, TILPMain)
                TILPMain.BorRefs.MdiParent = TILPMain
                TILPMain.BorRefs.Show()
                Me.Close()
            Else 'update existing borrower
                'write legacy address info to activity - this info comes from the borrower object 
                demo2Hist()
                disableAll()
                'update the info in the DB - this info comes from the borrower demographics form
                upDock()
                'reload borrower information so updated info will be displayed to the user - from the db
                selBor.changeKey(tbSSN.Text)
                selBor.BorDemo.loadDemo()
                selBor.borLoadDat()
                'MessageBox.Show("The demographic information for borrower " & selBor.getKey & " has been updated.", "Demographic information Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    'Perform data checks - if all data checks are met this will return true
    Private Function demoDatCheck() As Boolean
        'SSN data checks
        If tbSSN.TextLength < 9 Then
            MsgBox("SSN must be nine characters long")
            Return False
        End If
        If Not isNum(tbSSN, "SSN") Then _
            Return False

        'Required Name data checks 
        If Not mustExist(tbFN, "First Name") Then
            Return False
        ElseIf Not isAlph(tbFN, "First Name", False) Then
            Return False
        ElseIf Not mustExist(tbLN, "Last Name") Then
            Return False
        ElseIf Not isAlph(tbLN, "Last Name", False) Then
            Return False
        End If

        'Middle initial is not required, but if it's entered it needs to be checked
        If tbMI.TextLength > 0 And Not isAlph(tbMI, "Middle initial", False) Then _
            Return False

        'Required Adress 1 data checks
        If Not mustExist(tbAddr1, "Street Address 1") Then
            Return False
        ElseIf Not checkChar(tbAddr1.Text, "Street Address 1") Then
            Return False
        ElseIf Not goPostal(tbAddr1.Text, "Street Address 1") Then
            Return False
        End If

        'Address 2 is not required, but if it's entered it needs to be checked
        If tbAddr2.TextLength > 0 Then
            If Not checkChar(tbAddr2.Text, "Street Address 2") Then
                Return False
            ElseIf Not goPostal(tbAddr2.Text, "Street Address 2") Then
                Return False
            End If
        End If

        'Required City data check
        If Not mustExist(tbCity, "City") Then
            Return False
        ElseIf Not isAlph(tbCity, "City", True) Then
            Return False
        End If

        'Required State data check
        If cboxST.Text = "" Then
            MsgBox("The State Code cannot be blank")
            Return False
        ElseIf cboxST.Text <> UCase(cboxST.Text) Then
            MsgBox("The State Code must be uppercase.")
            Return False
        End If

        'Required Zip data check
        If Not isNum(tbZip, "Zip Code") Then
            Return False
        ElseIf tbZip.TextLength <> 5 And tbZip.TextLength <> 9 Then
            MsgBox("Zip Code must be either 5 or 9 characters long")
            Return False
        End If

        'Email address is not required but if it's entered it needs to have an @ and a .
        If tbEmail.TextLength > 0 Then
            If tbEmail.Text.IndexOf("@") = -1 Then
                MsgBox("An Email Address must have an @ symbol")
                Return False
            ElseIf tbEmail.Text.IndexOf(".") = -1 Then
                MsgBox("An Email Address must have a period(.) ")
                Return False
            End If
        End If

        'Home Phone isn't required but if entered it must be checked
        If tbHPhn.TextLength > 0 Then
            If Not checkPhone(tbHPhn, "Home Phone") Then _
                Return False
        End If

        'Alt Phone isn't required but if entered it must be checked
        If tbAltPhn.TextLength > 0 Then
            If Not checkPhone(tbAltPhn, "Alternate Phone") Then _
            Return False
        End If

        'Check DOB and make sure it's valid
        If Not IsDate(tbDOB.Text) Then
            MsgBox("The Date of Birth must contain a valid date.")
            Return False
        ElseIf Not CDate(tbDOB.Text) < Date.Today Then
            MsgBox("The Date of Birth must be in the past.")
            Return False
        End If

        'only check if pnote signed date if was populated by user
        If tbPnoteSig.Text <> "MM/DD/YYYY" And tbPnoteSig.TextLength <> 0 Then
            'Check Prom Note Signed Date and make sure it's valid
            If Not IsDate(tbPnoteSig.Text) Then
                MsgBox("The Date Pnote Signed must contain a valid date.")
                Return False
            ElseIf Not CDate(tbPnoteSig.Text) <= Date.Today Then
                MsgBox("The Date Pnote Signed must be before or on today.")
                Return False
            End If
        End If
        Return True
    End Function

    Private Function isNum(ByRef dControl, ByVal dMsg) As Boolean
        Dim iA As Integer = 0
        Do While iA <= dControl.TextLength - 1
            If Char.IsNumber(dControl.text.Chars(iA)) = False Then
                MsgBox("The " & dMsg & " field can only contain numbers.")
                Return False
                Exit Do
            Else
                iA = iA + 1
            End If
        Loop
        Return True
    End Function

    Private Function isAlph(ByRef aControl, ByVal dMsg, ByVal spacesOk) As Boolean
        Dim iA As Integer = 0
        Do While iA <= aControl.TextLength - 1
            If Char.IsLetter(aControl.text.Chars(iA)) = False Then
                If aControl.Text.Chars(iA) <> " " Then
                    MsgBox("The " & dMsg & " field can only contain letters.")
                    Return False
                ElseIf aControl.text.chars(iA) = " " And spacesOk = False Then
                    MsgBox("No spaces allowed. The " & dMsg & " field can only contain letters.")
                    Return False
                Else
                    iA = iA + 1
                End If
            Else
                iA = iA + 1
            End If
        Loop
        Return True
    End Function

    Private Function mustExist(ByRef mExcontrol, ByVal mExMsg) As Boolean
        If mExcontrol.TextLength = 0 Then
            MsgBox(mExMsg & " field must be populated")
            Return False
        End If
        Return True
    End Function

    Private Function checkChar(ByVal checkString, ByVal msgVar) As Boolean
        If checkString.IndexOf("!") <> -1 Or checkString.IndexOf("@") <> -1 Or _
            checkString.IndexOf("$") <> -1 Or checkString.IndexOf("%") <> -1 Or _
            checkString.IndexOf("^") <> -1 Or checkString.IndexOf("&") <> -1 Or _
            checkString.IndexOf("*") <> -1 Or checkString.IndexOf("(") <> -1 Or _
            checkString.IndexOf(")") <> -1 Or checkString.IndexOf("-") <> -1 Or _
            checkString.IndexOf("+") <> -1 Or checkString.IndexOf("=") <> -1 Or _
            checkString.IndexOf("<") <> -1 Or checkString.IndexOf(">") <> -1 Or _
            checkString.IndexOf(".") <> -1 Or checkString.IndexOf(",") <> -1 Or _
            checkString.IndexOf(":") <> -1 Or checkString.IndexOf("~") <> -1 Or _
            checkString.IndexOf("`") <> -1 Or checkString.IndexOf("?") <> -1 Or _
            checkString.IndexOf("""") <> -1 Or checkString.IndexOf(";") <> -1 Then
            MsgBox("The " & msgVar & " contains invalid characters")
            Return False
        Else
            Return True
        End If
    End Function

    Private Function goPostal(ByVal checkString, ByVal msgVar)
        Dim x As String
        If checkString.ToUpper.IndexOf("P.O. BOX") <> -1 Or checkString.ToUpper.IndexOf("PO BOX") <> -1 Or _
            checkString.ToUpper.IndexOf("P.O BOX") <> -1 Or checkString.ToUpper.IndexOf("POBOX") <> -1 Or _
            checkString.ToUpper.IndexOf("P.O.BOX") <> -1 Or checkString.ToUpper.IndexOf("P/O BOX") <> -1 Or _
            checkString.ToUpper.IndexOf("P O BOX") <> -1 Then
            MsgBox("The " & msgVar & " can't contain a PO Box")
            Return False
        Else
            Return True
        End If
    End Function

    Private Function checkPhone(ByVal phoneControl, ByVal msgPhnCntrl)
        If phoneControl.Text <> "NA" And phoneControl.Text <> "N/A" Then ' NA and N/A are OK
            If Not isNum(phoneControl, msgPhnCntrl) Then ' Make sure only numbers if not NA or N/A
                Return False
            ElseIf phoneControl.TextLength < 10 Then 'if populated phone must be 10 digits long
                MsgBox(msgPhnCntrl & " must contain 10 digits")
                Return False
            ElseIf phoneControl.Text.Substring(3, 1) = "0" Or phoneControl.Text.Substring(3, 1) = "1" Then
                MsgBox("The forth position of the " & msgPhnCntrl & " number can't contain a 0 or 1")
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub cbChanges_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbChanges.CheckedChanged
        If cbChanges.Checked Then
            If TheUser.GetAccessLevel.ToString = "1" Then
                dSidious()
            ElseIf TheUser.GetAccessLevel.ToString = "2" Then
                dVader()
            Else
                Clone()
            End If
        Else
            disableAll()
        End If
    End Sub

    Private Sub dSidious() 'Authority Level 1
        tbSSN.Enabled = True
        tbFN.Enabled = True
        tbLN.Enabled = True
        tbMI.Enabled = True
        tbAddr1.Enabled = True
        tbAddr2.Enabled = True
        tbCity.Enabled = True
        cboxST.Enabled = True
        tbZip.Enabled = True
        tbEmail.Enabled = True
        tbHPhn.Enabled = True
        tbAltPhn.Enabled = True
        tbDOB.Enabled = True
        tbPnoteSig.Enabled = True
        cbAddVal.Enabled = True
        cbHPV.Enabled = True
        cbAPV.Enabled = True
        btSaveAndContinue.Enabled = True
        cboxGender.Enabled = True
        cboxEth.Enabled = True
    End Sub

    Private Sub dVader() 'Authority Level 2
        tbSSN.Enabled = False
        tbFN.Enabled = False
        tbLN.Enabled = False
        tbMI.Enabled = False
        tbAddr1.Enabled = True
        tbAddr2.Enabled = True
        tbCity.Enabled = True
        cboxST.Enabled = True
        tbZip.Enabled = True
        tbEmail.Enabled = True
        tbHPhn.Enabled = True
        tbAltPhn.Enabled = True
        tbDOB.Enabled = False
        tbPnoteSig.Enabled = True
        cbAddVal.Enabled = True
        cbHPV.Enabled = True
        cbAPV.Enabled = True
        btSaveAndContinue.Enabled = True
        cboxGender.Enabled = True
        cboxEth.Enabled = True
    End Sub

    Private Sub Clone() 'Authority Level 3
        tbSSN.Enabled = False
        tbFN.Enabled = False
        tbLN.Enabled = False
        tbMI.Enabled = False
        tbAddr1.Enabled = True
        tbAddr2.Enabled = True
        tbCity.Enabled = True
        cboxST.Enabled = True
        tbZip.Enabled = True
        tbEmail.Enabled = True
        tbHPhn.Enabled = True
        tbAltPhn.Enabled = True
        tbDOB.Enabled = False
        tbPnoteSig.Enabled = False
        cbAddVal.Enabled = True
        cbHPV.Enabled = True
        cbAPV.Enabled = True
        btSaveAndContinue.Enabled = True
        cboxGender.Enabled = True
        cboxEth.Enabled = True
    End Sub

    Private Sub disableAll()
        cbChanges.Checked = False
        cbChanges.Enabled = True
        tbSSN.Enabled = False
        tbFN.Enabled = False
        tbLN.Enabled = False
        tbMI.Enabled = False
        tbAddr1.Enabled = False
        tbAddr2.Enabled = False
        tbCity.Enabled = False
        cboxST.Enabled = False
        tbZip.Enabled = False
        tbEmail.Enabled = False
        tbHPhn.Enabled = False
        tbAltPhn.Enabled = False
        tbDOB.Enabled = False
        tbPnoteSig.Enabled = False
        cbAddVal.Enabled = False
        cbHPV.Enabled = False
        cbAPV.Enabled = False
        btSaveAndContinue.Enabled = False
        cboxGender.Enabled = False
        cboxEth.Enabled = False
        cbInst.Enabled = False
        btCan.Text = "Cancel"
    End Sub

    Private Sub demo2Hist()
        Dim legAdd As String
        Dim pAddVal As String
        Dim pHPVal As String
        Dim pAHVal As String

        If selBor.BorDemo.AddValid = True Then
            pAddVal = "Y"
        Else
            pAddVal = "N"
        End If

        If selBor.BorDemo.hPhoneValid = True Then
            pHPVal = "Y"
        Else
            pHPVal = "N"
        End If

        If selBor.BorDemo.aPhoneValid = True Then
            pAHVal = "Y"
        Else
            pAHVal = "N"
        End If

        'create string with data to be inserted into activity history
        legAdd = "Demographic information modified. Previous information - SSN: " & selBor.getKey & ", First Name: " & selBor.FN & ", Middle Initial: " & selBor.MI & ", Last Name: " & selBor.LN & ", Address 1: " & selBor.BorDemo.Addr1 & ", Address 2: " & selBor.BorDemo.Addr2 & ", City: " & selBor.BorDemo.City & ", State: " & selBor.BorDemo.ST & ", Zip: " & selBor.BorDemo.Zip & ", DOB: " & selBor.DOB & ", Prom Note Sign Date: " & selBor.PromDtSig & ", Address Validity: " & pAddVal & ", Email: " & selBor.BorDemo.Email & ", Home Phone: " & selBor.BorDemo.hPhone & ", Alt Phone: " & selBor.BorDemo.aPhone & ", Home Phone Validity: " & pHPVal & ", Alt Phone Validity: " & pAHVal & ", Gender: " & selBor.BorDemo.Gender & ", Ethnicity: " & selBor.BorDemo.Ethnicity & "."
        'MsgBox(legAdd)

        'open connection to db
        Comm.Connection.Open()
        'SQL statement to execute
        Comm.CommandText = "INSERT INTO ActivityDat (SSN, ActivitySeq, UserID, ActivityText) VALUES ('" + selBor.getKey + "', " + borrower.NextActivitySeqNum(TestMode, selBor.getKey).ToString + ", '" + TheUser.GetUID + "', '" + legAdd.Replace("'", "''") + "')"
        Comm.ExecuteNonQuery()
        Comm.Connection.Close()
        'MessageBox.Show("Activity history has been updated.", "History Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub upDock()
        Dim uAddVal As String
        Dim uHPVal As String
        Dim uAHVal As String
        Dim PNoteSig As String

        If cbAddVal.Checked = True Then
            uAddVal = "1"
        Else
            uAddVal = "0"
        End If

        If cbHPV.Checked = True Then
            uHPVal = "1"
        Else
            uHPVal = "0"
        End If

        If cbAPV.Checked = True Then
            uAHVal = "1"
        Else
            uAHVal = "0"
        End If

        If IsDate(tbPnoteSig.Text) Then
            PNoteSig = tbPnoteSig.Text
        Else
            PNoteSig = ""
        End If
        'open connection to db
        Comm.Connection.Open()
        'SQL statement to execute
        Comm.CommandText = "Update BorrowerDat set SSN = '" & tbSSN.Text & "',DOB = '" & tbDOB.Text & "',PnoteSigDate = '" & PNoteSig & "',LastName = '" & tbLN.Text & "',FirstName = '" & tbFN.Text & "',MiddleInit = '" & tbMI.Text & "',Add1 = '" & tbAddr1.Text & "',Add2 = '" & tbAddr2.Text & "',City  = '" & tbCity.Text & "',State = '" & cboxST.Text & "',Zip = '" & tbZip.Text & "',AddressValidity = '" & uAddVal & "',HomePhone = '" & tbHPhn.Text & "',HomePhoneValidity = '" & uHPVal & "',AltPhone = '" & tbAltPhn.Text & "',AltPhoneValidity = '" & uAHVal & "',Email = '" & tbEmail.Text & "',Gender = '" & cboxGender.Text & "',Ethnicity = '" & cboxEth.Text & "' WHERE SSN = '" & selBor.getKey & "'"
        Comm.ExecuteNonQuery()
        Comm.Connection.Close()
    End Sub

    Private Function dupKey(ByVal keyVal) As Integer
        Dim Reader As SqlDataReader
        Dim qRes As String
        'open connection to db
        Comm.Connection.Open()
        'SQL statement to execute
        Comm.CommandText = "SELECT Count(*) as 'Count' From BorrowerDat WHERE SSN = '" & keyVal & "'"
        Reader = Comm.ExecuteReader
        While Reader.Read
            qRes = Reader("Count").ToString()
        End While
        Reader.Close()
        Comm.Connection.Close()
        Return qRes
    End Function
    'populate borrower object with demographic poperties if new borrower
    Private Sub iNoobie()
        'required fields
        selBor.changeKey(tbSSN.Text)
        selBor.FN = tbFN.Text
        selBor.LN = tbLN.Text
        selBor.BorDemo.Addr1 = tbAddr1.Text
        selBor.BorDemo.City = tbCity.Text
        selBor.BorDemo.ST = cboxST.Text
        selBor.BorDemo.Zip = tbZip.Text
        selBor.DOB = tbDOB.Text
        If IsDate(tbPnoteSig.Text) Then
            selBor.PromDtSig = tbPnoteSig.Text
        Else
            selBor.PromDtSig = ""
        End If
        If cbAddVal.Checked Then
            selBor.BorDemo.AddValid = True
        Else
            selBor.BorDemo.AddValid = False
        End If
        'optional fields'	
        selBor.MI = tbMI.Text
        selBor.BorDemo.Addr2 = tbAddr2.Text
        selBor.BorDemo.Email = tbEmail.Text
        selBor.BorDemo.hPhone = tbHPhn.Text
        selBor.BorDemo.aPhone = tbAltPhn.Text
        If cbHPV.Checked Then
            selBor.BorDemo.hPhoneValid = True
        Else
            selBor.BorDemo.hPhoneValid = False
        End If
        If cbAPV.Checked Then
            selBor.BorDemo.aPhoneValid = True
        Else
            selBor.BorDemo.aPhoneValid = False
        End If
        selBor.BorDemo.Gender = cboxGender.Text
        selBor.BorDemo.Ethnicity = cboxEth.Text
        selBor.AwardInst = cbInst.SelectedItem
        selBor.NewRecipLetterDt = Format(Date.Today, "MM/dd/yyyy")
    End Sub
End Class
