Imports System.Data.SqlClient






Public Class frmBorRefs
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
    Friend WithEvents gb1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbSSN As System.Windows.Forms.TextBox
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents gbRef1 As System.Windows.Forms.GroupBox
    Friend WithEvents gbRef2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents tbR1FN As System.Windows.Forms.TextBox
    Friend WithEvents tbR1MI As System.Windows.Forms.TextBox
    Friend WithEvents tbR1LN As System.Windows.Forms.TextBox
    Friend WithEvents tbR1Addr1 As System.Windows.Forms.TextBox
    Friend WithEvents tbR1Addr2 As System.Windows.Forms.TextBox
    Friend WithEvents tbR1City As System.Windows.Forms.TextBox
    Friend WithEvents tbR1Zip As System.Windows.Forms.TextBox
    Friend WithEvents cbR1AddValid As System.Windows.Forms.CheckBox
    Friend WithEvents cbR1HpnValid As System.Windows.Forms.CheckBox
    Friend WithEvents tbR1Hpn As System.Windows.Forms.TextBox
    Friend WithEvents cboxR1ST As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents cbR1Modify As System.Windows.Forms.CheckBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents cboxR2ST As System.Windows.Forms.ComboBox
    Friend WithEvents tbR2Hpn As System.Windows.Forms.TextBox
    Friend WithEvents cbR2HpnValid As System.Windows.Forms.CheckBox
    Friend WithEvents cbR2AddValid As System.Windows.Forms.CheckBox
    Friend WithEvents tbR2Zip As System.Windows.Forms.TextBox
    Friend WithEvents tbR2City As System.Windows.Forms.TextBox
    Friend WithEvents tbR2Addr2 As System.Windows.Forms.TextBox
    Friend WithEvents tbR2Addr1 As System.Windows.Forms.TextBox
    Friend WithEvents tbR2LN As System.Windows.Forms.TextBox
    Friend WithEvents tbR2MI As System.Windows.Forms.TextBox
    Friend WithEvents tbR2FN As System.Windows.Forms.TextBox
    Friend WithEvents cbR2Modify As System.Windows.Forms.CheckBox
    Friend WithEvents btSaveContinue As System.Windows.Forms.Button
    Friend WithEvents btCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBorRefs))
        Me.gb1 = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbSSN = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbName = New System.Windows.Forms.TextBox
        Me.gbRef1 = New System.Windows.Forms.GroupBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.cboxR1ST = New System.Windows.Forms.ComboBox
        Me.tbR1Hpn = New System.Windows.Forms.TextBox
        Me.cbR1HpnValid = New System.Windows.Forms.CheckBox
        Me.cbR1AddValid = New System.Windows.Forms.CheckBox
        Me.tbR1Zip = New System.Windows.Forms.TextBox
        Me.tbR1City = New System.Windows.Forms.TextBox
        Me.tbR1Addr2 = New System.Windows.Forms.TextBox
        Me.tbR1Addr1 = New System.Windows.Forms.TextBox
        Me.tbR1LN = New System.Windows.Forms.TextBox
        Me.tbR1MI = New System.Windows.Forms.TextBox
        Me.tbR1FN = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cbR1Modify = New System.Windows.Forms.CheckBox
        Me.gbRef2 = New System.Windows.Forms.GroupBox
        Me.cboxR2ST = New System.Windows.Forms.ComboBox
        Me.tbR2Hpn = New System.Windows.Forms.TextBox
        Me.cbR2HpnValid = New System.Windows.Forms.CheckBox
        Me.cbR2AddValid = New System.Windows.Forms.CheckBox
        Me.tbR2Zip = New System.Windows.Forms.TextBox
        Me.tbR2City = New System.Windows.Forms.TextBox
        Me.tbR2Addr2 = New System.Windows.Forms.TextBox
        Me.tbR2Addr1 = New System.Windows.Forms.TextBox
        Me.tbR2LN = New System.Windows.Forms.TextBox
        Me.tbR2MI = New System.Windows.Forms.TextBox
        Me.tbR2FN = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.cbR2Modify = New System.Windows.Forms.CheckBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.btSaveContinue = New System.Windows.Forms.Button
        Me.btCancel = New System.Windows.Forms.Button
        Me.gb1.SuspendLayout()
        Me.gbRef1.SuspendLayout()
        Me.gbRef2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gb1
        '
        Me.gb1.Controls.Add(Me.Label3)
        Me.gb1.Controls.Add(Me.tbSSN)
        Me.gb1.Controls.Add(Me.Label4)
        Me.gb1.Controls.Add(Me.tbName)
        Me.gb1.Location = New System.Drawing.Point(8, 0)
        Me.gb1.Name = "gb1"
        Me.gb1.Size = New System.Drawing.Size(728, 40)
        Me.gb1.TabIndex = 2
        Me.gb1.TabStop = False
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "SSN"
        '
        'tbSSN
        '
        Me.tbSSN.Location = New System.Drawing.Point(40, 12)
        Me.tbSSN.MaxLength = 9
        Me.tbSSN.Name = "tbSSN"
        Me.tbSSN.TabIndex = 3
        Me.tbSSN.Text = ""
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(144, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 12)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Name"
        '
        'tbName
        '
        Me.tbName.Location = New System.Drawing.Point(180, 12)
        Me.tbName.MaxLength = 50
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(544, 20)
        Me.tbName.TabIndex = 4
        Me.tbName.Text = ""
        '
        'gbRef1
        '
        Me.gbRef1.Controls.Add(Me.Label23)
        Me.gbRef1.Controls.Add(Me.cboxR1ST)
        Me.gbRef1.Controls.Add(Me.tbR1Hpn)
        Me.gbRef1.Controls.Add(Me.cbR1HpnValid)
        Me.gbRef1.Controls.Add(Me.cbR1AddValid)
        Me.gbRef1.Controls.Add(Me.tbR1Zip)
        Me.gbRef1.Controls.Add(Me.tbR1City)
        Me.gbRef1.Controls.Add(Me.tbR1Addr2)
        Me.gbRef1.Controls.Add(Me.tbR1Addr1)
        Me.gbRef1.Controls.Add(Me.tbR1LN)
        Me.gbRef1.Controls.Add(Me.tbR1MI)
        Me.gbRef1.Controls.Add(Me.tbR1FN)
        Me.gbRef1.Controls.Add(Me.Label13)
        Me.gbRef1.Controls.Add(Me.Label12)
        Me.gbRef1.Controls.Add(Me.Label11)
        Me.gbRef1.Controls.Add(Me.Label10)
        Me.gbRef1.Controls.Add(Me.Label9)
        Me.gbRef1.Controls.Add(Me.Label8)
        Me.gbRef1.Controls.Add(Me.Label7)
        Me.gbRef1.Controls.Add(Me.Label6)
        Me.gbRef1.Controls.Add(Me.Label5)
        Me.gbRef1.Controls.Add(Me.Label2)
        Me.gbRef1.Controls.Add(Me.Label1)
        Me.gbRef1.Controls.Add(Me.cbR1Modify)
        Me.gbRef1.Location = New System.Drawing.Point(8, 48)
        Me.gbRef1.Name = "gbRef1"
        Me.gbRef1.Size = New System.Drawing.Size(356, 280)
        Me.gbRef1.TabIndex = 0
        Me.gbRef1.TabStop = False
        Me.gbRef1.Text = "Reference 1"
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(296, 16)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(40, 16)
        Me.Label23.TabIndex = 24
        Me.Label23.Text = "Modify"
        '
        'cboxR1ST
        '
        Me.cboxR1ST.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboxR1ST.Items.AddRange(New Object() {"", "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY"})
        Me.cboxR1ST.Location = New System.Drawing.Point(116, 160)
        Me.cboxR1ST.MaxLength = 2
        Me.cboxR1ST.Name = "cboxR1ST"
        Me.cboxR1ST.Size = New System.Drawing.Size(36, 21)
        Me.cboxR1ST.TabIndex = 6
        '
        'tbR1Hpn
        '
        Me.tbR1Hpn.Location = New System.Drawing.Point(116, 232)
        Me.tbR1Hpn.MaxLength = 10
        Me.tbR1Hpn.Name = "tbR1Hpn"
        Me.tbR1Hpn.TabIndex = 9
        Me.tbR1Hpn.Text = ""
        '
        'cbR1HpnValid
        '
        Me.cbR1HpnValid.Location = New System.Drawing.Point(116, 256)
        Me.cbR1HpnValid.Name = "cbR1HpnValid"
        Me.cbR1HpnValid.Size = New System.Drawing.Size(16, 12)
        Me.cbR1HpnValid.TabIndex = 10
        '
        'cbR1AddValid
        '
        Me.cbR1AddValid.Location = New System.Drawing.Point(116, 208)
        Me.cbR1AddValid.Name = "cbR1AddValid"
        Me.cbR1AddValid.Size = New System.Drawing.Size(16, 12)
        Me.cbR1AddValid.TabIndex = 8
        '
        'tbR1Zip
        '
        Me.tbR1Zip.Location = New System.Drawing.Point(116, 184)
        Me.tbR1Zip.MaxLength = 9
        Me.tbR1Zip.Name = "tbR1Zip"
        Me.tbR1Zip.TabIndex = 7
        Me.tbR1Zip.Text = ""
        '
        'tbR1City
        '
        Me.tbR1City.Location = New System.Drawing.Point(116, 136)
        Me.tbR1City.MaxLength = 20
        Me.tbR1City.Name = "tbR1City"
        Me.tbR1City.Size = New System.Drawing.Size(172, 20)
        Me.tbR1City.TabIndex = 5
        Me.tbR1City.Text = ""
        '
        'tbR1Addr2
        '
        Me.tbR1Addr2.Location = New System.Drawing.Point(116, 112)
        Me.tbR1Addr2.MaxLength = 30
        Me.tbR1Addr2.Name = "tbR1Addr2"
        Me.tbR1Addr2.Size = New System.Drawing.Size(232, 20)
        Me.tbR1Addr2.TabIndex = 4
        Me.tbR1Addr2.Text = ""
        '
        'tbR1Addr1
        '
        Me.tbR1Addr1.Location = New System.Drawing.Point(116, 88)
        Me.tbR1Addr1.MaxLength = 30
        Me.tbR1Addr1.Name = "tbR1Addr1"
        Me.tbR1Addr1.Size = New System.Drawing.Size(232, 20)
        Me.tbR1Addr1.TabIndex = 3
        Me.tbR1Addr1.Text = ""
        '
        'tbR1LN
        '
        Me.tbR1LN.Location = New System.Drawing.Point(116, 64)
        Me.tbR1LN.MaxLength = 20
        Me.tbR1LN.Name = "tbR1LN"
        Me.tbR1LN.Size = New System.Drawing.Size(232, 20)
        Me.tbR1LN.TabIndex = 2
        Me.tbR1LN.Text = ""
        '
        'tbR1MI
        '
        Me.tbR1MI.Location = New System.Drawing.Point(116, 40)
        Me.tbR1MI.MaxLength = 1
        Me.tbR1MI.Name = "tbR1MI"
        Me.tbR1MI.Size = New System.Drawing.Size(16, 20)
        Me.tbR1MI.TabIndex = 1
        Me.tbR1MI.Text = ""
        '
        'tbR1FN
        '
        Me.tbR1FN.Location = New System.Drawing.Point(116, 20)
        Me.tbR1FN.MaxLength = 13
        Me.tbR1FN.Name = "tbR1FN"
        Me.tbR1FN.TabIndex = 0
        Me.tbR1FN.Text = ""
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(8, 260)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(100, 16)
        Me.Label13.TabIndex = 10
        Me.Label13.Text = "Home Phone Valid"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(8, 236)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(76, 16)
        Me.Label12.TabIndex = 9
        Me.Label12.Text = "Home Phone"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(8, 212)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(76, 16)
        Me.Label11.TabIndex = 8
        Me.Label11.Text = "Address Valid"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 164)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 16)
        Me.Label10.TabIndex = 7
        Me.Label10.Text = "State"
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 44)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(20, 12)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "MI"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 92)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 16)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "Street Address 1"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 116)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(88, 16)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Street Address 2"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 140)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 16)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "City"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 68)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 16)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Last Name"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 188)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(28, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Zip"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "First Name"
        '
        'cbR1Modify
        '
        Me.cbR1Modify.Location = New System.Drawing.Point(336, 16)
        Me.cbR1Modify.Name = "cbR1Modify"
        Me.cbR1Modify.Size = New System.Drawing.Size(16, 12)
        Me.cbR1Modify.TabIndex = 23
        '
        'gbRef2
        '
        Me.gbRef2.Controls.Add(Me.cboxR2ST)
        Me.gbRef2.Controls.Add(Me.tbR2Hpn)
        Me.gbRef2.Controls.Add(Me.cbR2HpnValid)
        Me.gbRef2.Controls.Add(Me.cbR2AddValid)
        Me.gbRef2.Controls.Add(Me.tbR2Zip)
        Me.gbRef2.Controls.Add(Me.tbR2City)
        Me.gbRef2.Controls.Add(Me.tbR2Addr2)
        Me.gbRef2.Controls.Add(Me.tbR2Addr1)
        Me.gbRef2.Controls.Add(Me.tbR2LN)
        Me.gbRef2.Controls.Add(Me.tbR2MI)
        Me.gbRef2.Controls.Add(Me.tbR2FN)
        Me.gbRef2.Controls.Add(Me.Label14)
        Me.gbRef2.Controls.Add(Me.Label15)
        Me.gbRef2.Controls.Add(Me.Label16)
        Me.gbRef2.Controls.Add(Me.Label17)
        Me.gbRef2.Controls.Add(Me.Label18)
        Me.gbRef2.Controls.Add(Me.Label19)
        Me.gbRef2.Controls.Add(Me.Label20)
        Me.gbRef2.Controls.Add(Me.Label21)
        Me.gbRef2.Controls.Add(Me.Label22)
        Me.gbRef2.Controls.Add(Me.Label24)
        Me.gbRef2.Controls.Add(Me.cbR2Modify)
        Me.gbRef2.Controls.Add(Me.Label25)
        Me.gbRef2.Controls.Add(Me.Label26)
        Me.gbRef2.Location = New System.Drawing.Point(380, 48)
        Me.gbRef2.Name = "gbRef2"
        Me.gbRef2.Size = New System.Drawing.Size(356, 280)
        Me.gbRef2.TabIndex = 1
        Me.gbRef2.TabStop = False
        Me.gbRef2.Text = "Reference2"
        '
        'cboxR2ST
        '
        Me.cboxR2ST.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboxR2ST.Items.AddRange(New Object() {"", "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY"})
        Me.cboxR2ST.Location = New System.Drawing.Point(116, 160)
        Me.cboxR2ST.MaxLength = 2
        Me.cboxR2ST.Name = "cboxR2ST"
        Me.cboxR2ST.Size = New System.Drawing.Size(36, 21)
        Me.cboxR2ST.TabIndex = 6
        '
        'tbR2Hpn
        '
        Me.tbR2Hpn.Location = New System.Drawing.Point(116, 232)
        Me.tbR2Hpn.MaxLength = 10
        Me.tbR2Hpn.Name = "tbR2Hpn"
        Me.tbR2Hpn.TabIndex = 9
        Me.tbR2Hpn.Text = ""
        '
        'cbR2HpnValid
        '
        Me.cbR2HpnValid.Location = New System.Drawing.Point(116, 256)
        Me.cbR2HpnValid.Name = "cbR2HpnValid"
        Me.cbR2HpnValid.Size = New System.Drawing.Size(16, 12)
        Me.cbR2HpnValid.TabIndex = 10
        '
        'cbR2AddValid
        '
        Me.cbR2AddValid.Location = New System.Drawing.Point(116, 208)
        Me.cbR2AddValid.Name = "cbR2AddValid"
        Me.cbR2AddValid.Size = New System.Drawing.Size(16, 12)
        Me.cbR2AddValid.TabIndex = 8
        '
        'tbR2Zip
        '
        Me.tbR2Zip.Location = New System.Drawing.Point(116, 184)
        Me.tbR2Zip.MaxLength = 9
        Me.tbR2Zip.Name = "tbR2Zip"
        Me.tbR2Zip.TabIndex = 7
        Me.tbR2Zip.Text = ""
        '
        'tbR2City
        '
        Me.tbR2City.Location = New System.Drawing.Point(116, 136)
        Me.tbR2City.MaxLength = 20
        Me.tbR2City.Name = "tbR2City"
        Me.tbR2City.Size = New System.Drawing.Size(172, 20)
        Me.tbR2City.TabIndex = 5
        Me.tbR2City.Text = ""
        '
        'tbR2Addr2
        '
        Me.tbR2Addr2.Location = New System.Drawing.Point(116, 112)
        Me.tbR2Addr2.MaxLength = 30
        Me.tbR2Addr2.Name = "tbR2Addr2"
        Me.tbR2Addr2.Size = New System.Drawing.Size(232, 20)
        Me.tbR2Addr2.TabIndex = 4
        Me.tbR2Addr2.Text = ""
        '
        'tbR2Addr1
        '
        Me.tbR2Addr1.Location = New System.Drawing.Point(116, 88)
        Me.tbR2Addr1.MaxLength = 30
        Me.tbR2Addr1.Name = "tbR2Addr1"
        Me.tbR2Addr1.Size = New System.Drawing.Size(232, 20)
        Me.tbR2Addr1.TabIndex = 3
        Me.tbR2Addr1.Text = ""
        '
        'tbR2LN
        '
        Me.tbR2LN.Location = New System.Drawing.Point(116, 64)
        Me.tbR2LN.MaxLength = 20
        Me.tbR2LN.Name = "tbR2LN"
        Me.tbR2LN.Size = New System.Drawing.Size(232, 20)
        Me.tbR2LN.TabIndex = 2
        Me.tbR2LN.Text = ""
        '
        'tbR2MI
        '
        Me.tbR2MI.Location = New System.Drawing.Point(116, 40)
        Me.tbR2MI.MaxLength = 1
        Me.tbR2MI.Name = "tbR2MI"
        Me.tbR2MI.Size = New System.Drawing.Size(16, 20)
        Me.tbR2MI.TabIndex = 1
        Me.tbR2MI.Text = ""
        '
        'tbR2FN
        '
        Me.tbR2FN.Location = New System.Drawing.Point(116, 20)
        Me.tbR2FN.MaxLength = 13
        Me.tbR2FN.Name = "tbR2FN"
        Me.tbR2FN.TabIndex = 0
        Me.tbR2FN.Text = ""
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(8, 260)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(100, 16)
        Me.Label14.TabIndex = 31
        Me.Label14.Text = "Home Phone Valid"
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(8, 236)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(76, 16)
        Me.Label15.TabIndex = 30
        Me.Label15.Text = "Home Phone"
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(8, 212)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(76, 16)
        Me.Label16.TabIndex = 29
        Me.Label16.Text = "Address Valid"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(8, 164)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(32, 16)
        Me.Label17.TabIndex = 28
        Me.Label17.Text = "State"
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(8, 92)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(88, 16)
        Me.Label18.TabIndex = 27
        Me.Label18.Text = "Street Address 1"
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(8, 116)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(88, 16)
        Me.Label19.TabIndex = 26
        Me.Label19.Text = "Street Address 2"
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(8, 68)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(60, 16)
        Me.Label20.TabIndex = 25
        Me.Label20.Text = "Last Name"
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(8, 188)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(28, 16)
        Me.Label21.TabIndex = 24
        Me.Label21.Text = "Zip"
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(8, 24)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(60, 16)
        Me.Label22.TabIndex = 23
        Me.Label22.Text = "First Name"
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(296, 16)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(40, 16)
        Me.Label24.TabIndex = 26
        Me.Label24.Text = "Modify"
        '
        'cbR2Modify
        '
        Me.cbR2Modify.Location = New System.Drawing.Point(336, 16)
        Me.cbR2Modify.Name = "cbR2Modify"
        Me.cbR2Modify.Size = New System.Drawing.Size(16, 12)
        Me.cbR2Modify.TabIndex = 25
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(8, 44)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(20, 12)
        Me.Label25.TabIndex = 25
        Me.Label25.Text = "MI"
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(8, 140)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(24, 16)
        Me.Label26.TabIndex = 25
        Me.Label26.Text = "City"
        '
        'btSaveContinue
        '
        Me.btSaveContinue.Location = New System.Drawing.Point(252, 336)
        Me.btSaveContinue.Name = "btSaveContinue"
        Me.btSaveContinue.Size = New System.Drawing.Size(116, 23)
        Me.btSaveContinue.TabIndex = 2
        Me.btSaveContinue.Text = "Save and Continue"
        '
        'btCancel
        '
        Me.btCancel.Location = New System.Drawing.Point(376, 336)
        Me.btCancel.Name = "btCancel"
        Me.btCancel.Size = New System.Drawing.Size(116, 23)
        Me.btCancel.TabIndex = 3
        Me.btCancel.Text = "Cancel"
        '
        'frmBorRefs
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(744, 361)
        Me.ControlBox = False
        Me.Controls.Add(Me.btCancel)
        Me.Controls.Add(Me.btSaveContinue)
        Me.Controls.Add(Me.gbRef2)
        Me.Controls.Add(Me.gbRef1)
        Me.Controls.Add(Me.gb1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(752, 389)
        Me.MinimumSize = New System.Drawing.Size(752, 389)
        Me.Name = "frmBorRefs"
        Me.Text = "Reference Information"
        Me.gb1.ResumeLayout(False)
        Me.gbRef1.ResumeLayout(False)
        Me.gbRef2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private selBor As borrower
    Private TestMode As Boolean
    Private TheUser As user
    Private theRefsT As New ArrayList
    Private Conn As SqlConnection
    Private Comm As SqlCommand
    Private BorDem As frmBorDemo
    Private TILPMain As frmTILPMain

    Private Sub frmBorRefs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        tbSSN.Enabled = False
        tbName.Enabled = False
        tbSSN.Text = selBor.getKey
        tbName.Text = selBor.FN + " " + selBor.MI + " " + selBor.LN

        If selBor.NewAcct = True Then 'For a new borrower where no references exist
            cbR1Modify.Enabled = False
            cbR2Modify.Enabled = False
            btSaveContinue.Enabled = True
            btSaveContinue.Text = "Continue"
        Else                          'For an existing borrower with references  
            btSaveContinue.Text = "Save"
            disGrp(gbRef1, cbR1Modify)
            disGrp(gbRef2, cbR2Modify)
            '*****************************************************************************************
            '* NOTE: The first ref object in the allRefs Array List is designated as reference 1 the 
            '* second ref object in the allRefs Array List is reference 2. There should never be more 
            '* then two objects in the allRefs Array List.
            '*****************************************************************************************
            'populate form for ref1
            tbR1FN.Text = CType(selBor.BorRefs.allRefs(0), ref).FN
            tbR1LN.Text = CType(selBor.BorRefs.allRefs(0), ref).LN
            tbR1MI.Text = CType(selBor.BorRefs.allRefs(0), ref).MI
            tbR1Addr1.Text = CType(selBor.BorRefs.allRefs(0), ref).Addr1
            tbR1Addr2.Text = CType(selBor.BorRefs.allRefs(0), ref).Addr2
            tbR1City.Text = CType(selBor.BorRefs.allRefs(0), ref).City
            cboxR1ST.Text = CType(selBor.BorRefs.allRefs(0), ref).ST
            tbR1Zip.Text = CType(selBor.BorRefs.allRefs(0), ref).Zip
            tbR1Hpn.Text = CType(selBor.BorRefs.allRefs(0), ref).hPhone
            If CType(selBor.BorRefs.allRefs(0), ref).AddValid = True Then
                cbR1AddValid.Checked = True
            Else
                cbR1AddValid.Checked = False
            End If
            If CType(selBor.BorRefs.allRefs(0), ref).hPhoneValid = True Then
                cbR1HpnValid.Checked = True
            Else
                cbR1HpnValid.Checked = False
            End If

            'populate form for ref2
            tbR2FN.Text = CType(selBor.BorRefs.allRefs(1), ref).FN
            tbR2LN.Text = CType(selBor.BorRefs.allRefs(1), ref).LN
            tbR2MI.Text = CType(selBor.BorRefs.allRefs(1), ref).MI
            tbR2Addr1.Text = CType(selBor.BorRefs.allRefs(1), ref).Addr1
            tbR2Addr2.Text = CType(selBor.BorRefs.allRefs(1), ref).Addr2
            tbR2City.Text = CType(selBor.BorRefs.allRefs(1), ref).City
            cboxR2ST.Text = CType(selBor.BorRefs.allRefs(1), ref).ST
            tbR2Zip.Text = CType(selBor.BorRefs.allRefs(1), ref).Zip
            tbR2Hpn.Text = CType(selBor.BorRefs.allRefs(1), ref).hPhone
            If CType(selBor.BorRefs.allRefs(1), ref).AddValid = True Then
                cbR2AddValid.Checked = True
            Else
                cbR2AddValid.Checked = False
            End If
            If CType(selBor.BorRefs.allRefs(1), ref).hPhoneValid = True Then
                cbR2HpnValid.Checked = True
            Else
                cbR2HpnValid.Checked = False
            End If
            'if access level is 4 then don't allow to modify
            If TheUser.GetAccessLevel() = 4 Then
                cbR1Modify.Enabled = False
                cbR2Modify.Enabled = False
                btSaveContinue.Enabled = False
            End If
        End If
        'set up DB connection
        If TestMode Then 'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else 'in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Comm = New SqlCommand
        Comm.Connection = Conn

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCancel.Click
        If TILPMain.selBor.NewAcct = True Then
            If MessageBox.Show("You are about to leave this form without saving Borrower or Reference information. This borrower will not be added to the TILP System." & vbCrLf & " Do you want to continue without saving? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                Me.Close()
            Else
                Exit Sub
            End If
        Else
            Me.Close()
        End If
    End Sub

    'disable controls within a specific group box
    Private Sub disGrp(ByVal grpBx, ByVal mCtrl)
        Dim I As Integer
        While I < grpBx.Controls.Count
            grpBx.Controls(I).Enabled = False
            I = I + 1
        End While
        mCtrl.Enabled = True
        enbSCbt()
    End Sub

    'enable controls within a specific group box
    Private Sub enbGrp(ByVal grpBx)
        Dim I As Integer
        While I < grpBx.Controls.Count
            grpBx.Controls(I).Enabled = True
            I = I + 1
        End While
        enbSCbt()
    End Sub

    'Enable save and continue button if modifications have been made to one or both of the refs
    Private Sub enbSCbt()
        If cbR1Modify.Checked = True Or cbR2Modify.Checked = True Then
            btSaveContinue.Enabled = True
        Else
            btSaveContinue.Enabled = False
        End If

    End Sub

    Private Sub cbR1Modify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbR1Modify.CheckedChanged
        If cbR1Modify.Checked Then
            enbGrp(gbRef1)
        Else
            disGrp(gbRef1, cbR1Modify)
        End If
    End Sub

    Private Sub cbR2Modify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbR2Modify.CheckedChanged
        If cbR2Modify.Checked Then
            enbGrp(gbRef2)
        Else
            disGrp(gbRef2, cbR2Modify)
        End If
    End Sub

    Private Sub btSaveContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSaveContinue.Click
        Dim i As Integer
        Dim rMsgNum As String
        tSaveFormDat()
        If TILPMain.selBor.NewAcct = True Then 'Adding a new borrower and refs

            Do While i < theRefsT.Count 'Perform data checks
                If i = 0 Then
                    rMsgNum = "1"
                Else
                    rMsgNum = "2"
                End If
                If demoRefCheck(CType(theRefsT(i), ref), rMsgNum) = False Then Exit Sub
                i = i + 1
            Loop

            ' If everything checks out with refs then add borrower and reference data to the db
            If MessageBox.Show("You are about to add this borrower and references to the TILP System. Do you wish to continue?", "Borrower and Reference Add", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.No Then
                MessageBox.Show("Borrower not added", "Cancel Borrower and Reference Add", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                aNoobie() 'Add borrower to db
                aNoobieRefs() ' Add borrower references to the db
                'MessageBox.Show("The information for " & selBor.FN & " " & selBor.LN & " has been successfully added to the TILP System", "Borrower Information Added Successfully", MessageBoxButtons.OK)
                'Reinitialize selbor with the data that was just added to the db
                TILPMain.selBor = New borrower(TestMode, False, selBor.getKey)
                TILPMain.selBor.LockAccount(TheUser.GetUID()) 'lock account
                'do borrower sub object calls to load data from DB
                TILPMain.selBor.borLoadDat() 'global borrower demographic info
                TILPMain.selBor.BorLoans.DoDBLoad() 'loan info
                TILPMain.selBor.BorDemo.loadDemo() 'borrower demographic info
                TILPMain.selBor.BorRefs.DoDBLoad() 'borrower reference info

                'If State Office of Edu create BELVCHR4HS letter
                If selBor.AwardInst = "State Office of Education" Then
                    'Create data file for mail merge
                    FileOpen(1, "T:\TILPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
                    WriteLine(1, "Date", "KeyLine", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP")
                    WriteLine(1, selBor.NewRecipLetterDt, ACSKeyLine(selBor.getKey), selBor.FN, selBor.LN, selBor.BorDemo.Addr1, selBor.BorDemo.Addr2, selBor.BorDemo.City, selBor.BorDemo.ST, selBor.BorDemo.Zip)
                    FileClose(1)
                    borrower.PrintDoc("TILPNEWHS", "X:\PADD\TILP\", selBor.TestMode)
                End If

                'Disalbe controls
                disGrp(gbRef1, cbR1Modify)
                disGrp(gbRef2, cbR2Modify)
                TILPMain.MenuOptionCoor(frmTILPMain.OpSelected.GeneralNav)
            End If
        Else 'update reference info for existing borrower
                'Reference 1 if necessary
                If cbR1Modify.Checked = True Then
                    If demoRefCheck(CType(theRefsT(0), ref), 1) = False Then
                        Exit Sub 'if the data checks fail then leave sub
                    Else 'if passes data checks continue
                        'write legacy address info to activity - this info comes from the existing reference class variables 
                        refDemo2Hist(0)
                        'update the info in the DB - this info comes from the reference demographics form
                        refUpDock(0, tbR1LN, tbR1FN, tbR1MI, tbR1Addr1, tbR1Addr2, tbR1City, cboxR1ST, tbR1Zip, cbR1AddValid, tbR1Hpn, cbR1HpnValid)
                        cbR1Modify.Checked = False
                        disGrp(gbRef1, cbR1Modify)
                    End If
                End If
                'Reference 2 if necessary
                If cbR2Modify.Checked = True Then
                    If demoRefCheck(CType(theRefsT(1), ref), 2) = False Then
                        Exit Sub 'if the data checks fail then leave sub
                    Else 'if passes data checks continue
                        'write legacy address info to activity - this info comes from the existing reference class variables 
                        refDemo2Hist(1)
                        'update the info in the DB - this info comes from the reference demographics form
                        refUpDock(1, tbR2LN, tbR2FN, tbR2MI, tbR2Addr1, tbR2Addr2, tbR2City, cboxR2ST, tbR2Zip, cbR2AddValid, tbR2Hpn, cbR2HpnValid)
                        cbR2Modify.Checked = False
                        disGrp(gbRef2, cbR2Modify)
                    End If
                End If

                'empty the legacy reference information
                selBor.BorRefs.allRefs.Clear()

                'reload borrower information so updated info will be displayed to the user - from the db
            selBor.BorRefs.DoDBLoad()
            'MessageBox.Show("The Reference information for this borrower has been updated.", "Reference Information Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    'saves changes to temporary form storage
    Private Sub tSaveFormDat()
        Dim R1 As New ref
        Dim R2 As New ref
        theRefsT = New ArrayList

        'Reference 1 Data
        R1.FN = tbR1FN.Text
        R1.LN = tbR1LN.Text
        R1.MI = tbR1MI.Text
        R1.Addr1 = tbR1Addr1.Text
        R1.Addr2 = tbR1Addr2.Text
        R1.City = tbR1City.Text
        R1.ST = cboxR1ST.Text
        R1.Zip = tbR1Zip.Text
        R1.hPhone = tbR1Hpn.Text
        If cbR1AddValid.Checked = True Then
            R1.AddValid = True
        Else
            R1.AddValid = False
        End If
        If cbR1HpnValid.Checked = True Then
            R1.hPhoneValid = True
        Else
            R1.hPhoneValid = False
        End If

        'Reference 2 Data
        R2.FN = tbR2FN.Text
        R2.LN = tbR2LN.Text
        R2.MI = tbR2MI.Text
        R2.Addr1 = tbR2Addr1.Text
        R2.Addr2 = tbR2Addr2.Text
        R2.City = tbR2City.Text
        R2.ST = cboxR2ST.Text
        R2.Zip = tbR2Zip.Text
        R2.hPhone = tbR2Hpn.Text
        If cbR2AddValid.Checked = True Then
            R2.AddValid = True
        Else
            R2.AddValid = False
        End If
        If cbR2HpnValid.Checked = True Then
            R2.hPhoneValid = True
        Else
            R2.hPhoneValid = False
        End If

        'add ref data to array list
        theRefsT.Add(R1)
        theRefsT.Add(R2)
    End Sub

    '***************************************************************************************************
    '* Functions for data checking the refs
    '***************************************************************************************************
    Private Function demoRefCheck(ByRef ref2Check As Object, ByVal rNum As String) As Boolean
        'Required Name data checks 
        If Not mustExist(ref2Check.FN.ToString, "First Name", rNum) Then
            Return False
        ElseIf Not isAlph(ref2Check.FN.ToString, "First Name", False, rNum) Then
            Return False
        ElseIf Not mustExist(ref2Check.LN.ToString, "Last Name", rNum) Then
            Return False
        ElseIf Not isAlph(ref2Check.LN.ToString, "Last Name", False, rNum) Then
            Return False
        End If

        'Middle initial is not required, but if it's entered it needs to be checked
        If ref2Check.MI.ToString.Length > 0 And Not isAlph(ref2Check.MI, "Middle initial", False, rNum) Then _
            Return False

        'Required Adress 1 data checks
        If Not mustExist(ref2Check.Addr1.ToString, "Street Address 1", rNum) Then
            Return False
        ElseIf Not checkChar(ref2Check.Addr1.ToString, "Street Address 1", rNum) Then
            Return False
        End If

        ''Address 2 is not required, but if it's entered it needs to be checked
        If ref2Check.Addr2.ToString.Length > 0 Then
            If Not checkChar(ref2Check.Addr2.ToString, "Street Address 2", rNum) Then
                Return False
            End If
        End If

        ''Required City data check
        If Not mustExist(ref2Check.City.ToString, "City", rNum) Then
            Return False
        ElseIf Not isAlph(ref2Check.City.ToString, "City", True, rNum) Then
            Return False
        End If

        'Required State data check
        If ref2Check.ST.ToString = "" Then
            MsgBox("The State Code cannot be blank")
            Return False
        ElseIf ref2Check.ST.ToString <> UCase(ref2Check.ST.ToString) Then
            MsgBox("The State Code must be uppercase.")
            Return False
        End If

        'Required Zip data check
        If Not isNum(ref2Check.zip.ToString, "Zip Code", rNum) Then
            Return False
        ElseIf ref2Check.Zip.ToString.Length <> 5 And ref2Check.Zip.ToString.Length <> 9 Then
            MsgBox("Zip Code must be either 5 or 9 characters long")
            Return False
        End If

        ''Home Phone isn't required but if entered it must be checked
        If ref2Check.hphone.ToString.Length > 0 Then
            If Not checkPhone(ref2Check.hphone.ToString, "Home Phone", rNum) Then _
                Return False
        End If

        Return True
    End Function

    Private Function mustExist(ByRef mExObj, ByVal mExMsg, ByVal mExRNum) As Boolean
        If mExObj.Length = 0 Then
            MsgBox("The " & mExMsg & " field for Reference " & mExRNum & " must be populated.")
            Return False
        End If
        Return True
    End Function

    Private Function isNum(ByRef iNumObj, ByVal iNumMsg, ByVal iNumRNum) As Boolean
        Dim iA As Integer = 0
        Do While iA <= iNumObj.Length - 1
            If Char.IsNumber(iNumObj.Chars(iA)) = False Then
                MsgBox("The " & iNumMsg & " field for Reference " & iNumRNum & " can only contain numbers.")
                Return False
                Exit Do
            Else
                iA = iA + 1
            End If
        Loop
        Return True
    End Function

    Private Function isAlph(ByRef iAlpObj, ByVal iAlpMsg, ByVal iAlpSpacesOk, ByVal iAlpRNum) As Boolean
        Dim iA As Integer = 0
        Do While iA <= iAlpObj.Length - 1
            If Char.IsLetter(iAlpObj.Chars(iA)) = False Then
                If iAlpObj.Chars(iA) <> " " Then
                    MsgBox("The " & iAlpMsg & " field for Reference " & iAlpRNum & " can only contain letters.")
                    Return False
                ElseIf iAlpObj.chars(iA) = " " And iAlpSpacesOk = False Then
                    MsgBox("No spaces allowed. The " & iAlpMsg & " field for Reference " & iAlpRNum & " can only contain letters.")
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

    Private Function checkChar(ByVal cChrString, ByVal cChrMsg, ByVal cChrRNum) As Boolean
        If cChrString.IndexOf("!") <> -1 Or cChrString.IndexOf("@") <> -1 Or _
            cChrString.IndexOf("$") <> -1 Or cChrString.IndexOf("%") <> -1 Or _
            cChrString.IndexOf("^") <> -1 Or cChrString.IndexOf("&") <> -1 Or _
            cChrString.IndexOf("*") <> -1 Or cChrString.IndexOf("(") <> -1 Or _
            cChrString.IndexOf(")") <> -1 Or cChrString.IndexOf("-") <> -1 Or _
            cChrString.IndexOf("+") <> -1 Or cChrString.IndexOf("=") <> -1 Or _
            cChrString.IndexOf("<") <> -1 Or cChrString.IndexOf(">") <> -1 Or _
            cChrString.IndexOf(".") <> -1 Or cChrString.IndexOf(",") <> -1 Or _
            cChrString.IndexOf(":") <> -1 Or cChrString.IndexOf("~") <> -1 Or _
            cChrString.IndexOf("`") <> -1 Or cChrString.IndexOf("?") <> -1 Or _
            cChrString.IndexOf("""") <> -1 Or cChrString.IndexOf(";") <> -1 Then
            MsgBox("The " & cChrMsg & " field for Reference " & cChrRNum & " contains invalid characters.")
            Return False
        Else
            Return True
        End If
    End Function

    Private Function checkPhone(ByRef cPhnObj, ByVal cPhnMsg, ByVal cPhnRNum)
        If cPhnObj <> "NA" And cPhnObj <> "N/A" Then ' NA and N/A are OK
            If Not isNum(cPhnObj, cPhnMsg, cPhnRNum) Then ' Make sure only numbers if not NA or N/A
                Return False
            ElseIf cPhnObj.Length < 10 Then 'if populated phone must be 10 digits long
                MsgBox("The " & cPhnMsg & " field for Reference " & cPhnRNum & " must contain 10 digits.")
                Return False
            ElseIf cPhnObj.Substring(3, 1) = "0" Or cPhnObj.Substring(3, 1) = "1" Then
                MsgBox("The forth position of the " & cPhnMsg & " number field for Reference " & cPhnRNum & " can't contain a 0 or 1.")
                Return False
            End If
        End If
        Return True
    End Function

    '***************************************************************************************************
    '* End data checking functions 
    '***************************************************************************************************

    'add new borrower to the database
    Private Sub aNoobie()

        Dim nAddVal As String
        Dim nHPVal As String
        Dim nAHVal As String

        'Get data formatted to insert into db
        If selBor.BorDemo.AddValid = True = True Then
            nAddVal = "1"
        Else
            nAddVal = "0"
        End If

        If selBor.BorDemo.hPhoneValid = True Then
            nHPVal = "1"
        Else
            nHPVal = "0"
        End If

        If selBor.BorDemo.aPhoneValid = True Then
            nAHVal = "1"
        Else
            nAHVal = "0"
        End If

        'open connection to db
        Comm.Connection.Open()
        'SQL statement to execute
        Comm.CommandText = "INSERT INTO BorrowerDat (SSN,DOB,PnoteSigDate,LastName,FirstName,MiddleInit,Add1,Add2,City,State,Zip,AddressValidity,HomePhone,HomePhoneValidity,AltPhone,AltPhoneValidity,Email,Gender,Ethnicity,NewRecipLetterDt,AwardInst) VALUES ('" & selBor.getKey & "','" & selBor.DOB & "','" & selBor.PromDtSig & "','" & selBor.LN & "','" & selBor.FN & "','" & selBor.MI & "','" & selBor.BorDemo.Addr1 & "','" & selBor.BorDemo.Addr2 & "','" & selBor.BorDemo.City & "','" & selBor.BorDemo.ST & "','" & selBor.BorDemo.Zip & "','" & nAddVal & "','" & selBor.BorDemo.hPhone & "','" & nHPVal & "','" & selBor.BorDemo.aPhone & "','" & nAHVal & "','" & selBor.BorDemo.Email & "','" & selBor.BorDemo.Gender & "','" & selBor.BorDemo.Ethnicity & "','" & selBor.NewRecipLetterDt & "','" & selBor.AwardInst & "')"
        Comm.ExecuteNonQuery()
        Comm.Connection.Close()
    End Sub

    'add borrower references to the database
    Private Sub aNoobieRefs()
        Dim i As Integer = 0
        Dim nRAddVal As String
        Dim nRHPVal As String
        'open connection to db
        Comm.Connection.Open()
        Do While i < 2
            If CType(theRefsT(i), ref).AddValid = True Then
                nRAddVal = "1"
            Else
                nRAddVal = "0"
            End If

            If CType(theRefsT(i), ref).hPhoneValid = True Then
                nRHPVal = "1"
            Else
                nRHPVal = "0"
            End If
            'SQL statement to execute
            Comm.CommandText = "INSERT INTO ReferenceDat (RefLastName,RefFirstName,RefMiddleInit,RefAdd1,RefAdd2,RefCity,RefState,RefZip,RefAddValidity,RefHomePhone,RefHomePhoneValidity,SSN) VALUES ('" & CType(theRefsT(i), ref).LN & "','" & CType(theRefsT(i), ref).FN & "','" & CType(theRefsT(i), ref).MI & "','" & CType(theRefsT(i), ref).Addr1 & "','" & CType(theRefsT(i), ref).Addr2 & "','" & CType(theRefsT(i), ref).City & "','" & CType(theRefsT(i), ref).ST & "','" & CType(theRefsT(i), ref).Zip & "','" & nRAddVal & "','" & CType(theRefsT(i), ref).hPhone & "','" & nRHPVal & "','" & selBor.getKey & "')"
            Comm.ExecuteNonQuery()
            i = i + 1
        Loop
        Comm.Connection.Close()
    End Sub

    'add legacy reference data to a history comment 
    Private Sub refDemo2Hist(ByVal refNum As Integer)
        Dim rAddVal As String
        Dim rHPVal As String
        Dim legRefDat As String

        If CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).AddValid = True Then
            rAddVal = "Y"
        Else
            rAddVal = "N"
        End If

        If CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).hPhoneValid = True Then
            rHPVal = "Y"
        Else
            rHPVal = "N"
        End If

        legRefDat = "Demographic information modified for Reference " & refNum + 1 & ". Previous information - First Name: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).FN & ", Middle Initial: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).MI & ", Last Name: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).LN & ", Address 1: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).Addr1 & ", Address 2: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).Addr2 & ", City: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).City & ", State: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).ST & ", Zip: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).Zip & ", Address Validity: " & rAddVal & ", Home Phone: " & CType(TILPMain.selBor.BorRefs.allRefs(refNum), ref).hPhone & ", Home Phone Validity: " & rHPVal & "."

        'open connection to db
        Comm.Connection.Open()
        'SQL statement to execute
        Comm.CommandText = "INSERT INTO ActivityDat (SSN, ActivitySeq, UserID, ActivityText) VALUES ('" + TILPMain.selBor.getKey + "', " + TILPMain.selBor.NextActivitySeqNum(TestMode, TILPMain.selBor.getKey).ToString + ", '" + TheUser.GetUID + "', '" + legRefDat.Replace("'", "''") + "')"
        Comm.ExecuteNonQuery()
        'close connection
        Comm.Connection.Close()
    End Sub

    'Private Sub refUpDock(ByVal uRefNum, ByVal addVld, ByVal phnCntrl)
    Private Sub refUpDock(ByVal uRefNum, ByVal LN, ByVal FN, ByVal MI, ByVal Addr1, ByVal Addr2, ByVal City, ByVal ST, ByVal Zip, ByVal addVld, ByVal Hpn, ByVal phnVld)
        Dim urAddVal As String
        Dim urHPVal As String
        Dim x As String

        If addVld.Checked = True Then
            urAddVal = "1"
        Else
            urAddVal = "0"
        End If

        If phnVld.Checked = True Then
            urHPVal = "1"
        Else
            urHPVal = "0"
        End If

        'open connection to db
        Comm.Connection.Open()
        'SQL statement to execute
        Comm.CommandText = "Update ReferenceDat set RefLastName = '" & LN.Text & "',RefFirstName = '" & FN.Text & "',RefMiddleInit = '" & MI.Text & "',RefAdd1 = '" & Addr1.Text & "',RefAdd2 = '" & Addr2.Text & "',RefCity = '" & City.Text & "',RefState = '" & ST.Text & "',RefZip = '" & Zip.Text & "',RefAddValidity  = '" & urAddVal & "',RefHomePhone = '" & Hpn.Text & "',RefHomePhoneValidity = '" & urHPVal & "' WHERE RefID = " & CType(TILPMain.selBor.BorRefs.allRefs(uRefNum), ref).ID
        Comm.ExecuteNonQuery()
        'close connection to db
        Comm.Connection.Close()
    End Sub

    'calculate the ACS keyline
    Function ACSKeyLine(ByVal SSN As String, Optional ByVal PersonTyp As String = "P", Optional ByVal AddressTyp As String = "L") As String
        Dim SSNi(9) As String  'array of individual SSN characters
        Dim SSNc As String          'decoded SSN
        Dim tKL As String           'temp keyline
        Dim ChkDig As Integer       'check digit
        Dim KLBit As Integer        'keyline bit value
        Dim i As Integer

        'encrypt SSN
        If PersonTyp = "P" Then
            For i = 1 To 9
                Select Case Mid(SSN, i, 1)
                    Case 1
                        SSNi(i) = "R"
                    Case 2
                        SSNi(i) = "E"
                    Case 3
                        SSNi(i) = "T"
                    Case 4
                        SSNi(i) = "H"
                    Case 5
                        SSNi(i) = "G"
                    Case 6
                        SSNi(i) = "U"
                    Case 7
                        SSNi(i) = "A"
                    Case 8
                        SSNi(i) = "L"
                    Case 9
                        SSNi(i) = "Y"
                    Case 0
                        SSNi(i) = "M"
                End Select
            Next i
            SSNc = SSNi(1) & SSNi(2) & SSNi(3) & SSNi(4) & SSNi(5) & _
                  SSNi(6) & SSNi(7) & SSNi(8) & SSNi(9)
            'encrypt ref id
        Else
            SSNc = Mid(SSN, 1, 2) & "/" & Mid(SSN, 4, 6)
        End If
        'add person type and address type to encrypted SSN/ref id for temp keyline
        tKL = PersonTyp & SSNc & Format(Month(Date.Today), "0#") & Format(Date.Today.Day, "0#") & AddressTyp
        'convert temp keyline characters to 4-bit numbers and calculate check digit
        ChkDig = 0
        For i = 1 To Len(tKL)
            Select Case Mid(tKL, i, 1)
                Case "A"
                    KLBit = 1
                Case "B"
                    KLBit = 2
                Case "C"
                    KLBit = 3
                Case "D"
                    KLBit = 4
                Case "E"
                    KLBit = 5
                Case "F"
                    KLBit = 6
                Case "G"
                    KLBit = 7
                Case "H"
                    KLBit = 8
                Case "I"
                    KLBit = 9
                Case "J"
                    KLBit = 10
                Case "K"
                    KLBit = 11
                Case "L"
                    KLBit = 12
                Case "M"
                    KLBit = 13
                Case "N"
                    KLBit = 14
                Case "O"
                    KLBit = 15
                Case "P"
                    KLBit = 0
                Case "Q"
                    KLBit = 1
                Case "R"
                    KLBit = 2
                Case "S"
                    KLBit = 3
                Case "T"
                    KLBit = 4
                Case "U"
                    KLBit = 5
                Case "V"
                    KLBit = 6
                Case "W"
                    KLBit = 7
                Case "X"
                    KLBit = 8
                Case "Y"
                    KLBit = 9
                Case "Z"
                    KLBit = 10
                Case "/"
                    KLBit = 15
                Case Else
                    KLBit = Val(Mid(tKL, i, 1))
            End Select
            'multiply the value by 2 if the position is odd
            If i = 1 Or i = 3 Or i = 5 Or i = 7 Or i = 9 Or i = 11 Or i = 13 Or i = 15 _
                Then KLBit = KLBit * 2
            'add the two digits of sum
            KLBit = Val(Mid(Format(KLBit, "0#"), 1, 1)) + Val(Mid(Format(KLBit, "0#"), 2, 1))
            'add the two digits of the sum
            KLBit = Val(Mid(Format(KLBit, "0#"), 1, 1)) + Val(Mid(Format(KLBit, "0#"), 2, 1))
            'add the sum to the check digit
            ChkDig = ChkDig + KLBit
        Next i

        'subtract the right digit of the check digit from 10 to get the final check digit
        ChkDig = 10 - Val(Microsoft.VisualBasic.Right(ChkDig, 1))
        'if the check digit is 10, the check digit is 0
        If ChkDig = 10 Then ChkDig = 0
        'add the check digit to the end of the temp keyline to get the ACS Keyline
        ACSKeyLine = "#" & tKL & ChkDig & "#"
    End Function
End Class
