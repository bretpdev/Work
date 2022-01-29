Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Data.SqlClient

Public Class frmDemographics
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Private conn As New SqlConnection

    Public Sub New(ByVal tBor As SP.Borrower)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        If SP.TestMode Then
            conn = ConnTest
        Else
            conn = ConnLive
        End If

        'Disable the consent checkbox for address and email
        CheckboxIndicatorsForAddress.ckbConsent.Enabled = False
        CheckboxIndicatorsForEmail.ckbConsent.Enabled = False
        CheckboxIndicatorsForAddress.ckbConsent.Visible = False
        CheckboxIndicatorsForEmail.ckbConsent.Visible = False
        CheckboxIndicatorsForAddress.GroupBox1.Visible = False
        CheckboxIndicatorsForEmail.GroupBox1.Visible = False

        SetupFormControls(tBor)
        Dim RM As Resources.ResourceManager
        SetupFormImages(RM) 'pass in null resource manager and it'll default to SP
        'setup indicators
        SetupIndicators()
        DemoForUpdate.SyncWithMainFormControls(Bor.UserProvidedDemos, Indicators)
    End Sub

    Public Sub New(ByVal tBor As SP.Borrower, ByVal RM As Resources.ResourceManager)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        If SP.TestMode Then
            conn = ConnTest
        Else
            conn = ConnLive
        End If

        'Disable the consent checkbox for address and email
        CheckboxIndicatorsForAddress.ckbConsent.Enabled = False
        CheckboxIndicatorsForEmail.ckbConsent.Enabled = False
        CheckboxIndicatorsForAddress.ckbConsent.Visible = False
        CheckboxIndicatorsForEmail.ckbConsent.Visible = False
        CheckboxIndicatorsForAddress.GroupBox1.Visible = False
        CheckboxIndicatorsForEmail.GroupBox1.Visible = False

        SetupFormControls(tBor)
        SetupFormImages(RM)
        'setup indicators
        SetupIndicators()
        DemoForUpdate.SyncWithMainFormControls(Bor.UserProvidedDemos, Indicators)
    End Sub

    Public Sub New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Disable the consent checkbox for address and email
        CheckboxIndicatorsForAddress.ckbConsent.Enabled = False
        CheckboxIndicatorsForEmail.ckbConsent.Enabled = False
        CheckboxIndicatorsForAddress.ckbConsent.Visible = False
        CheckboxIndicatorsForEmail.ckbConsent.Visible = False
        CheckboxIndicatorsForAddress.GroupBox1.Visible = False
        CheckboxIndicatorsForEmail.GroupBox1.Visible = False

        'setup indicators
        SetupIndicators()
        DemoForUpdate.SyncWithMainFormControls(Bor.UserProvidedDemos, Indicators)
    End Sub

    Private Sub SetupIndicators()
        CheckboxIndicatorsForAddress.DataBind(New IndicatorsForDemographicPart(), DemoForUpdate.Addr, DemoForUpdate, Bor, DemographicsForUpdate.DemographicPartType.Address, ckbAddAlternateAddress)
        CheckboxIndicatorsForHomePhone.DataBind(New IndicatorsForDemographicPart(), DemoForUpdate.HomePhn, DemoForUpdate, Bor, DemographicsForUpdate.DemographicPartType.HomePhone)
        CheckboxIndicatorsForOtherPhone.DataBind(New IndicatorsForDemographicPart(), DemoForUpdate.OtherPhn, DemoForUpdate, Bor, DemographicsForUpdate.DemographicPartType.OtherPhone)
        CheckboxIndicatorsForOtherPhone2.DataBind(New IndicatorsForDemographicPart(), DemoForUpdate.Other2Phn, DemoForUpdate, Bor, DemographicsForUpdate.DemographicPartType.OtherPhone2)
        CheckboxIndicatorsForEmail.DataBind(New IndicatorsForDemographicPart(), DemoForUpdate.txtEmail, DemoForUpdate, Bor, DemographicsForUpdate.DemographicPartType.Email)
        Indicators.Add("Address", CheckboxIndicatorsForAddress)
        Indicators.Add("HomePhone", CheckboxIndicatorsForHomePhone)
        Indicators.Add("OtherPhone", CheckboxIndicatorsForOtherPhone)
        Indicators.Add("OtherPhone2", CheckboxIndicatorsForOtherPhone2)
        Indicators.Add("Email", CheckboxIndicatorsForEmail)
    End Sub

    Public Sub SetupFormImages(ByVal RM As Resources.ResourceManager)
        'create resource manager for SP if a specific one wasn't sent in
        If RM Is Nothing Then
            RM = New Resources.ResourceManager("SP.MauiDUDERes", Me.GetType.Assembly)
        End If
        If Today.Month = 12 Then
            ButtonBGPic.Image = CType(RM.GetObject("DemoGPalmChristmas"), System.Drawing.Image)
        ElseIf Today.Month = 1 Or Today.Month = 2 Then
            If Today.Month = 1 Then
                ButtonBGPic.Image = CType(RM.GetObject("DemoGPalmFireWorks"), System.Drawing.Image)
            ElseIf Today.Month = 2 Then
                ButtonBGPic.Image = CType(RM.GetObject("DemoGPalmValentines"), System.Drawing.Image)
            End If
        ElseIf Today.Month = 3 Or Today.Month = 4 Or Today.Month = 5 Then
            ButtonBGPic.Image = CType(RM.GetObject("DemoGPalmReg"), System.Drawing.Image)
        Else
            If Today.Month = 10 Then
                ButtonBGPic.Image = CType(RM.GetObject("DemoGPalmHalloween"), System.Drawing.Image)
            ElseIf Today.Month = 7 Then
                ButtonBGPic.Image = CType(RM.GetObject("DemoGPalmFireWorks"), System.Drawing.Image)
            Else
                ButtonBGPic.Image = CType(RM.GetObject("DemoGPalmReg"), System.Drawing.Image)
            End If
        End If
        CompassDemographicsForDisplay.SetupImages(RM)
        OneLINKDemographicsForDisplay.SetupImages(RM)
    End Sub

    'called by new to setup form controls
    Public Sub SetupFormControls(ByVal tBor As SP.Borrower)

        Bor = tBor
        Bor.GetDemographicsFrmSystem() 'get demographic info from system
        'arrays for activity and contact combo boxes
        'IMPORTANT: if an item is added to one of these array lists then it must be added to all of them
        ActivityCode = New ArrayList 'this is the only list not related to the others
        ContactCode = New ArrayList 'linked to contact description box
        LP22Source = New ArrayList 'linked to contact description box
        TX1JSource = New ArrayList 'linked to contact description box
        'IMPORTANT: if an item is added to one of these array lists then it must be added to all of them
        ActivityCode.Add("AM")
        ActivityCode.Add("CD")
        ActivityCode.Add("CL")
        ActivityCode.Add("CO")
        ActivityCode.Add("ED")
        ActivityCode.Add("EM")
        ActivityCode.Add("ET")
        ActivityCode.Add("FA")
        ActivityCode.Add("FO")
        ActivityCode.Add("LT")
        ActivityCode.Add("MS")
        ActivityCode.Add("PC")
        ActivityCode.Add("T1")
        ActivityCode.Add("T2")
        ActivityCode.Add("T3")
        ActivityCode.Add("T4")
        ActivityCode.Add("T5")
        ActivityCode.Add("T6")
        ActivityCode.Add("T7")
        ActivityCode.Add("T8")
        ActivityCode.Add("TA")
        ActivityCode.Add("TC")
        ActivityCode.Add("TE")
        ActivityCode.Add("TT")
        ActivityCode.Add("OV")

        'IMPORTANT: if an item is added to one of these array lists then it must be added to all of them
        ContactCode.Add("33")
        ContactCode.Add("34")
        ContactCode.Add("03")
        ContactCode.Add("04")
        ContactCode.Add("93")
        ContactCode.Add("94")
        ContactCode.Add("83")
        ContactCode.Add("84")
        ContactCode.Add("91")
        ContactCode.Add("92")
        ContactCode.Add("81")
        ContactCode.Add("82")
        ContactCode.Add("69")
        ContactCode.Add("70")
        ContactCode.Add("11")
        ContactCode.Add("12")
        ContactCode.Add("29")
        ContactCode.Add("30")
        ContactCode.Add("05")
        ContactCode.Add("06")
        ContactCode.Add("95")
        ContactCode.Add("96")
        ContactCode.Add("89")
        ContactCode.Add("90")
        ContactCode.Add("85")
        ContactCode.Add("86")
        ContactCode.Add("27")
        ContactCode.Add("28")
        ContactCode.Add("07")
        ContactCode.Add("08")
        ContactCode.Add("09")
        ContactCode.Add("10")
        ContactCode.Add("TO")
        ContactCode.Add("TI")
        'LP22 Source codes
        'IMPORTANT: if an item is added to one of these array lists then it must be added to all of them
        LP22Source.Add("1")
        LP22Source.Add("1")
        LP22Source.Add("F")
        LP22Source.Add("F")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("3")
        LP22Source.Add("3")
        LP22Source.Add("6")
        LP22Source.Add("6")
        LP22Source.Add("4")
        LP22Source.Add("4")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("1")
        LP22Source.Add("1")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("2")
        LP22Source.Add("2")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("1")
        LP22Source.Add("1")
        LP22Source.Add("D")
        LP22Source.Add("D")
        LP22Source.Add("K")
        LP22Source.Add("K")
        LP22Source.Add("1")
        LP22Source.Add("1")
        'TX1J source codes
        'IMPORTANT: if an item is added to one of these array lists then it must be added to all of them
        TX1JSource.Add("43")
        TX1JSource.Add("43")
        TX1JSource.Add("41")
        TX1JSource.Add("41")
        TX1JSource.Add("42")
        TX1JSource.Add("42")
        TX1JSource.Add("11")
        TX1JSource.Add("11")
        TX1JSource.Add("26")
        TX1JSource.Add("26")
        TX1JSource.Add("23")
        TX1JSource.Add("23")
        TX1JSource.Add("42")
        TX1JSource.Add("42")
        TX1JSource.Add("43")
        TX1JSource.Add("43")
        TX1JSource.Add("56")
        TX1JSource.Add("56")
        TX1JSource.Add("31")
        TX1JSource.Add("31")
        TX1JSource.Add("31")
        TX1JSource.Add("31")
        TX1JSource.Add("25")
        TX1JSource.Add("25")
        TX1JSource.Add("44")
        TX1JSource.Add("44")
        TX1JSource.Add("43")
        TX1JSource.Add("43")
        TX1JSource.Add("43")
        TX1JSource.Add("43")
        TX1JSource.Add("31")
        TX1JSource.Add("31")
        TX1JSource.Add("43")
        TX1JSource.Add("43")
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
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblSSN As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents ckbNoPhone As System.Windows.Forms.CheckBox
    Protected Friend WithEvents Label33 As System.Windows.Forms.Label
    Protected Friend WithEvents Label32 As System.Windows.Forms.Label
    Protected Friend WithEvents txtContactCode As System.Windows.Forms.TextBox
    Protected Friend WithEvents txtActivityCode As System.Windows.Forms.TextBox
    Protected Friend WithEvents cbContactDesc As System.Windows.Forms.ComboBox
    Protected Friend WithEvents cbActivityDesc As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents lblDOB As System.Windows.Forms.Label
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuBorrInfo As System.Windows.Forms.MenuItem
    Friend WithEvents MenuDictionary As System.Windows.Forms.MenuItem
    Friend WithEvents MenuAboutDude As System.Windows.Forms.MenuItem
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents mnuUtilities As System.Windows.Forms.MenuItem
    Friend WithEvents MenuAskDUDE As System.Windows.Forms.MenuItem
    Friend WithEvents btnHygiene As System.Windows.Forms.Button
    Friend WithEvents ButtonBGPic As System.Windows.Forms.PictureBox
    Protected Friend WithEvents cboAttempt As System.Windows.Forms.ComboBox
    Protected Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents btnBrightIdea As System.Windows.Forms.Button
    Friend WithEvents btnUnexpected As System.Windows.Forms.Button
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents lblAN As System.Windows.Forms.Label
    Protected Friend WithEvents gbActivity As System.Windows.Forms.GroupBox
    Friend WithEvents gbBorrowerInfo As System.Windows.Forms.GroupBox
    Friend WithEvents ConnTest As System.Data.SqlClient.SqlConnection
    Friend WithEvents ConnLive As System.Data.SqlClient.SqlConnection
    Friend WithEvents btnSAndC As System.Windows.Forms.Button
    Friend WithEvents CompassDemographicsForDisplay As SP.CompassDemographicsForDisplayOnly
    Friend WithEvents OneLINKDemographicsForDisplay As SP.OneLINKDemographicsForDisplayOnly
    Friend WithEvents ckbAddAlternateAddress As System.Windows.Forms.CheckBox
    Friend WithEvents CheckboxIndicatorsForEmail As SP.CheckboxIndicatorsForDemographicPart
    Friend WithEvents CheckboxIndicatorsForOtherPhone2 As SP.CheckboxIndicatorsForDemographicPart
    Friend WithEvents CheckboxIndicatorsForOtherPhone As SP.CheckboxIndicatorsForDemographicPart
    Friend WithEvents CheckboxIndicatorsForHomePhone As SP.CheckboxIndicatorsForDemographicPart
    Friend WithEvents CheckboxIndicatorsForAddress As SP.CheckboxIndicatorsForDemographicPart
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents DemoForUpdate As SP.DemographicsForUpdate
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDemographics))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblSSN = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ckbNoPhone = New System.Windows.Forms.CheckBox
        Me.txtContactCode = New System.Windows.Forms.TextBox
        Me.txtActivityCode = New System.Windows.Forms.TextBox
        Me.cbContactDesc = New System.Windows.Forms.ComboBox
        Me.cbActivityDesc = New System.Windows.Forms.ComboBox
        Me.btnRefresh = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnSAndC = New System.Windows.Forms.Button
        Me.btnUnexpected = New System.Windows.Forms.Button
        Me.btnBrightIdea = New System.Windows.Forms.Button
        Me.Label36 = New System.Windows.Forms.Label
        Me.gbBorrowerInfo = New System.Windows.Forms.GroupBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ckbAddAlternateAddress = New System.Windows.Forms.CheckBox
        Me.btnHygiene = New System.Windows.Forms.Button
        Me.Label46 = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.gbActivity = New System.Windows.Forms.GroupBox
        Me.Label52 = New System.Windows.Forms.Label
        Me.cboAttempt = New System.Windows.Forms.ComboBox
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ButtonBGPic = New System.Windows.Forms.PictureBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.lblDOB = New System.Windows.Forms.Label
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuAskDUDE = New System.Windows.Forms.MenuItem
        Me.mnuUtilities = New System.Windows.Forms.MenuItem
        Me.MenuBorrInfo = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuDictionary = New System.Windows.Forms.MenuItem
        Me.MenuAboutDude = New System.Windows.Forms.MenuItem
        Me.Label55 = New System.Windows.Forms.Label
        Me.lblAN = New System.Windows.Forms.Label
        Me.ConnTest = New System.Data.SqlClient.SqlConnection
        Me.ConnLive = New System.Data.SqlClient.SqlConnection
        Me.CompassDemographicsForDisplay = New SP.CompassDemographicsForDisplayOnly
        Me.OneLINKDemographicsForDisplay = New SP.OneLINKDemographicsForDisplayOnly
        Me.CheckboxIndicatorsForEmail = New SP.CheckboxIndicatorsForDemographicPart
        Me.CheckboxIndicatorsForOtherPhone2 = New SP.CheckboxIndicatorsForDemographicPart
        Me.CheckboxIndicatorsForOtherPhone = New SP.CheckboxIndicatorsForDemographicPart
        Me.CheckboxIndicatorsForHomePhone = New SP.CheckboxIndicatorsForDemographicPart
        Me.CheckboxIndicatorsForAddress = New SP.CheckboxIndicatorsForDemographicPart
        Me.DemoForUpdate = New SP.DemographicsForUpdate
        Me.gbBorrowerInfo.SuspendLayout()
        Me.gbActivity.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.ButtonBGPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(1117, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Maui DUDE Demographics Screen"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 24)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "SSN:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSSN
        '
        Me.lblSSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSSN.Location = New System.Drawing.Point(40, 37)
        Me.lblSSN.Name = "lblSSN"
        Me.lblSSN.Size = New System.Drawing.Size(72, 23)
        Me.lblSSN.TabIndex = 0
        Me.lblSSN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.lblSSN, "Borrower's SSN")
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(128, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 23)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Name:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblName
        '
        Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblName.Location = New System.Drawing.Point(168, 38)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(516, 23)
        Me.lblName.TabIndex = 0
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolTip1.SetToolTip(Me.lblName, "Borrower's full name")
        '
        'ckbNoPhone
        '
        Me.ckbNoPhone.Location = New System.Drawing.Point(88, 209)
        Me.ckbNoPhone.Name = "ckbNoPhone"
        Me.ckbNoPhone.Size = New System.Drawing.Size(104, 16)
        Me.ckbNoPhone.TabIndex = 13
        Me.ckbNoPhone.TabStop = False
        Me.ckbNoPhone.Text = "No Phone"
        Me.ToolTip1.SetToolTip(Me.ckbNoPhone, "The borrower doesn't have a phone.")
        '
        'txtContactCode
        '
        Me.txtContactCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtContactCode.Location = New System.Drawing.Point(88, 40)
        Me.txtContactCode.Name = "txtContactCode"
        Me.txtContactCode.Size = New System.Drawing.Size(24, 20)
        Me.txtContactCode.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.txtContactCode, "Two character contact code.")
        '
        'txtActivityCode
        '
        Me.txtActivityCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtActivityCode.Location = New System.Drawing.Point(88, 16)
        Me.txtActivityCode.MaxLength = 2
        Me.txtActivityCode.Name = "txtActivityCode"
        Me.txtActivityCode.Size = New System.Drawing.Size(24, 20)
        Me.txtActivityCode.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.txtActivityCode, "Two character activity code.")
        '
        'cbContactDesc
        '
        Me.cbContactDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbContactDesc.ItemHeight = 13
        Me.cbContactDesc.Items.AddRange(New Object() {"", "To: Attorney", "From: Attorney", "To: Borrower", "From: Borrower", "To: Comaker", "From: Comaker", "To: Credit Bureau", "From: Credit Bureau", "To: DMV", "From: DMV", "To: Employer", "From: Employer", "To: Endorser", "From: Endorser", "To: Family", "From: Family", "To: Guarantor", "From: Guarantor", "To: Lender", "From: Lender", "To: Miscellaneous", "From: Miscellaneous", "To: Post Office", "From: Post Office", "To: Prison", "From: Prison", "To: Reference", "From: Reference", "To: School", "From: School", "To: UHEAA Staff", "From: UHEAA Staff", "To 3rd Party", "From 3rd Party"})
        Me.cbContactDesc.Location = New System.Drawing.Point(120, 40)
        Me.cbContactDesc.Name = "cbContactDesc"
        Me.cbContactDesc.Size = New System.Drawing.Size(320, 21)
        Me.cbContactDesc.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.cbContactDesc, "Contact code description.")
        '
        'cbActivityDesc
        '
        Me.cbActivityDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbActivityDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbActivityDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbActivityDesc.ItemHeight = 13
        Me.cbActivityDesc.Items.AddRange(New Object() {"", "Account Maintenance", "Court Document", "Claim", "Computer Letter", "Electronic Document", "E-mail", "Electronic Transmission", "Fax", "Form", "Letter", "Miscellaneous", "Preclaim", "Reference Contact Helpful (Home#)", "Reference Attempt (Home#)", "Reference Contact Not Helpful (Home#)", "Reference Do Not Contact (Home#)", "Reference Contact Helpful (Alt#)", "Reference Attempt (Alt#)", "Reference Contact Not Helpful (Alt#)", "Reference Do Not Contact (Alt#)", "Tape", "Telephone Contact", "Telephone Call", "Telephone Attempt", "Office Visit"})
        Me.cbActivityDesc.Location = New System.Drawing.Point(120, 16)
        Me.cbActivityDesc.Name = "cbActivityDesc"
        Me.cbActivityDesc.Size = New System.Drawing.Size(320, 21)
        Me.cbActivityDesc.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.cbActivityDesc, "Activity code description.")
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(401, 473)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(96, 23)
        Me.btnRefresh.TabIndex = 37
        Me.btnRefresh.Text = "&Refresh Screen"
        Me.ToolTip1.SetToolTip(Me.btnRefresh, "Click here to refresh the screen.")
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(273, 473)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(120, 23)
        Me.btnBack.TabIndex = 36
        Me.btnBack.Text = "&Back To Main Menu"
        Me.ToolTip1.SetToolTip(Me.btnBack, "Click here to return to the Main Menu screen.")
        '
        'btnSAndC
        '
        Me.btnSAndC.Location = New System.Drawing.Point(145, 473)
        Me.btnSAndC.Name = "btnSAndC"
        Me.btnSAndC.Size = New System.Drawing.Size(120, 23)
        Me.btnSAndC.TabIndex = 35
        Me.btnSAndC.Text = "&Save and Continue"
        Me.ToolTip1.SetToolTip(Me.btnSAndC, "Click here to record updates.")
        '
        'btnUnexpected
        '
        Me.btnUnexpected.Image = CType(resources.GetObject("btnUnexpected.Image"), System.Drawing.Image)
        Me.btnUnexpected.Location = New System.Drawing.Point(1051, 27)
        Me.btnUnexpected.Name = "btnUnexpected"
        Me.btnUnexpected.Size = New System.Drawing.Size(43, 43)
        Me.btnUnexpected.TabIndex = 27
        Me.btnUnexpected.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnUnexpected, "Unexpected Result? Report Maui DUDE Errors Here.")
        '
        'btnBrightIdea
        '
        Me.btnBrightIdea.Image = CType(resources.GetObject("btnBrightIdea.Image"), System.Drawing.Image)
        Me.btnBrightIdea.Location = New System.Drawing.Point(995, 27)
        Me.btnBrightIdea.Name = "btnBrightIdea"
        Me.btnBrightIdea.Size = New System.Drawing.Size(43, 43)
        Me.btnBrightIdea.TabIndex = 26
        Me.btnBrightIdea.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnBrightIdea, "Got a Bright Idea for Maui DUDE? Click here to Send your good Idea to the Big Kah" & _
                "una")
        '
        'Label36
        '
        Me.Label36.BackColor = System.Drawing.Color.Transparent
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(649, 495)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(456, 23)
        Me.Label36.TabIndex = 33
        Me.Label36.Text = "*ADDRESS INFORMATION IS FOR LEGAL ADDRESS, PHONE, AND E-MAIL"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gbBorrowerInfo
        '
        Me.gbBorrowerInfo.Controls.Add(Me.Label7)
        Me.gbBorrowerInfo.Controls.Add(Me.Label6)
        Me.gbBorrowerInfo.Controls.Add(Me.Label5)
        Me.gbBorrowerInfo.Controls.Add(Me.Label3)
        Me.gbBorrowerInfo.Controls.Add(Me.CheckboxIndicatorsForEmail)
        Me.gbBorrowerInfo.Controls.Add(Me.CheckboxIndicatorsForOtherPhone2)
        Me.gbBorrowerInfo.Controls.Add(Me.CheckboxIndicatorsForOtherPhone)
        Me.gbBorrowerInfo.Controls.Add(Me.CheckboxIndicatorsForHomePhone)
        Me.gbBorrowerInfo.Controls.Add(Me.CheckboxIndicatorsForAddress)
        Me.gbBorrowerInfo.Controls.Add(Me.ckbAddAlternateAddress)
        Me.gbBorrowerInfo.Controls.Add(Me.DemoForUpdate)
        Me.gbBorrowerInfo.Controls.Add(Me.btnHygiene)
        Me.gbBorrowerInfo.Controls.Add(Me.Label46)
        Me.gbBorrowerInfo.Controls.Add(Me.Label45)
        Me.gbBorrowerInfo.Controls.Add(Me.Label35)
        Me.gbBorrowerInfo.Controls.Add(Me.Label25)
        Me.gbBorrowerInfo.Controls.Add(Me.Label21)
        Me.gbBorrowerInfo.Controls.Add(Me.ckbNoPhone)
        Me.gbBorrowerInfo.Location = New System.Drawing.Point(8, 96)
        Me.gbBorrowerInfo.Name = "gbBorrowerInfo"
        Me.gbBorrowerInfo.Size = New System.Drawing.Size(638, 232)
        Me.gbBorrowerInfo.TabIndex = 1
        Me.gbBorrowerInfo.TabStop = False
        Me.gbBorrowerInfo.Text = "Borrower Info"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(533, 71)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 104
        Me.Label7.Text = "Consent"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(476, 53)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(55, 31)
        Me.Label6.TabIndex = 101
        Me.Label6.Text = "Invalidate First"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(414, 71)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 13)
        Me.Label5.TabIndex = 100
        Me.Label5.Text = "Verified"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(356, 71)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 99
        Me.Label3.Text = "Not Valid"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'ckbAddAlternateAddress
        '
        Me.ckbAddAlternateAddress.AutoSize = True
        Me.ckbAddAlternateAddress.Location = New System.Drawing.Point(372, 21)
        Me.ckbAddAlternateAddress.Name = "ckbAddAlternateAddress"
        Me.ckbAddAlternateAddress.Size = New System.Drawing.Size(131, 17)
        Me.ckbAddAlternateAddress.TabIndex = 93
        Me.ckbAddAlternateAddress.Text = "Add Alternate Address"
        Me.ckbAddAlternateAddress.UseVisualStyleBackColor = True
        '
        'btnHygiene
        '
        Me.btnHygiene.Location = New System.Drawing.Point(557, 14)
        Me.btnHygiene.Name = "btnHygiene"
        Me.btnHygiene.Size = New System.Drawing.Size(75, 24)
        Me.btnHygiene.TabIndex = 81
        Me.btnHygiene.Text = "&Da Rules"
        '
        'Label46
        '
        Me.Label46.Location = New System.Drawing.Point(588, 189)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(46, 16)
        Me.Label46.TabIndex = 80
        Me.Label46.Text = "E-mail"
        '
        'Label45
        '
        Me.Label45.Location = New System.Drawing.Point(588, 166)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(46, 16)
        Me.Label45.TabIndex = 79
        Me.Label45.Text = "Other 2"
        '
        'Label35
        '
        Me.Label35.Location = New System.Drawing.Point(588, 141)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(46, 16)
        Me.Label35.TabIndex = 78
        Me.Label35.Text = "Other"
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(588, 117)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(46, 16)
        Me.Label25.TabIndex = 77
        Me.Label25.Text = "Home"
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(588, 93)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(46, 16)
        Me.Label21.TabIndex = 72
        Me.Label21.Text = "Address"
        '
        'gbActivity
        '
        Me.gbActivity.Controls.Add(Me.Label52)
        Me.gbActivity.Controls.Add(Me.cboAttempt)
        Me.gbActivity.Controls.Add(Me.Label33)
        Me.gbActivity.Controls.Add(Me.Label32)
        Me.gbActivity.Controls.Add(Me.txtContactCode)
        Me.gbActivity.Controls.Add(Me.txtActivityCode)
        Me.gbActivity.Controls.Add(Me.cbContactDesc)
        Me.gbActivity.Controls.Add(Me.cbActivityDesc)
        Me.gbActivity.Location = New System.Drawing.Point(8, 16)
        Me.gbActivity.Name = "gbActivity"
        Me.gbActivity.Size = New System.Drawing.Size(638, 72)
        Me.gbActivity.TabIndex = 2
        Me.gbActivity.TabStop = False
        Me.gbActivity.Text = "Activity Comment"
        '
        'Label52
        '
        Me.Label52.Location = New System.Drawing.Point(446, 16)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(100, 16)
        Me.Label52.TabIndex = 7
        Me.Label52.Text = "Attempt Type"
        Me.Label52.Visible = False
        '
        'cboAttempt
        '
        Me.cboAttempt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAttempt.Items.AddRange(New Object() {"", "No Answer", "Answering Machine/Service", "Wrong Number", "Phone Busy", "Disconnected Phone/Out of Service"})
        Me.cboAttempt.Location = New System.Drawing.Point(446, 40)
        Me.cboAttempt.Name = "cboAttempt"
        Me.cboAttempt.Size = New System.Drawing.Size(156, 21)
        Me.cboAttempt.TabIndex = 3
        Me.cboAttempt.Visible = False
        '
        'Label33
        '
        Me.Label33.Location = New System.Drawing.Point(8, 40)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(80, 23)
        Me.Label33.TabIndex = 5
        Me.Label33.Text = "Contact Code:"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label32
        '
        Me.Label32.Location = New System.Drawing.Point(8, 16)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(80, 23)
        Me.Label32.TabIndex = 4
        Me.Label32.Text = "Activity Code:"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CompassDemographicsForDisplay)
        Me.GroupBox1.Controls.Add(Me.OneLINKDemographicsForDisplay)
        Me.GroupBox1.Controls.Add(Me.btnRefresh)
        Me.GroupBox1.Controls.Add(Me.btnBack)
        Me.GroupBox1.Controls.Add(Me.btnSAndC)
        Me.GroupBox1.Controls.Add(Me.ButtonBGPic)
        Me.GroupBox1.Controls.Add(Me.Label36)
        Me.GroupBox1.Controls.Add(Me.gbBorrowerInfo)
        Me.GroupBox1.Controls.Add(Me.gbActivity)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 80)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1117, 528)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        '
        'ButtonBGPic
        '
        Me.ButtonBGPic.Location = New System.Drawing.Point(81, 334)
        Me.ButtonBGPic.Name = "ButtonBGPic"
        Me.ButtonBGPic.Size = New System.Drawing.Size(488, 184)
        Me.ButtonBGPic.TabIndex = 34
        Me.ButtonBGPic.TabStop = False
        '
        'Label34
        '
        Me.Label34.Location = New System.Drawing.Point(690, 43)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(32, 23)
        Me.Label34.TabIndex = 6
        Me.Label34.Text = "DOB:"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDOB
        '
        Me.lblDOB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDOB.Location = New System.Drawing.Point(722, 39)
        Me.lblDOB.Name = "lblDOB"
        Me.lblDOB.Size = New System.Drawing.Size(72, 23)
        Me.lblDOB.TabIndex = 7
        Me.lblDOB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuAskDUDE, Me.mnuUtilities, Me.MenuItem3})
        '
        'MenuAskDUDE
        '
        Me.MenuAskDUDE.Index = 0
        Me.MenuAskDUDE.Text = "&Ask DUDE"
        '
        'mnuUtilities
        '
        Me.mnuUtilities.Index = 1
        Me.mnuUtilities.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuBorrInfo})
        Me.mnuUtilities.Text = "&Utilities"
        '
        'MenuBorrInfo
        '
        Me.MenuBorrInfo.Index = 0
        Me.MenuBorrInfo.Text = "&Borrower Information"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 2
        Me.MenuItem3.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuDictionary, Me.MenuAboutDude})
        Me.MenuItem3.Text = "&Help"
        '
        'MenuDictionary
        '
        Me.MenuDictionary.Index = 0
        Me.MenuDictionary.Text = "&Dictionary"
        '
        'MenuAboutDude
        '
        Me.MenuAboutDude.Index = 1
        Me.MenuAboutDude.Text = "&About Dude"
        '
        'Label55
        '
        Me.Label55.Location = New System.Drawing.Point(810, 39)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(64, 23)
        Me.Label55.TabIndex = 28
        Me.Label55.Text = "Account #:"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAN
        '
        Me.lblAN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAN.Location = New System.Drawing.Point(874, 39)
        Me.lblAN.Name = "lblAN"
        Me.lblAN.Size = New System.Drawing.Size(96, 23)
        Me.lblAN.TabIndex = 29
        Me.lblAN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ConnTest
        '
        Me.ConnTest.ConnectionString = "Data Source=OPSDEV;Initial Catalog=MauiDUDE;Integrated Security=SSPI;"
        Me.ConnTest.FireInfoMessageEventOnUserErrors = False
        '
        'ConnLive
        '
        Me.ConnLive.ConnectionString = "Data Source=NOCHOUSE;Initial Catalog=MauiDUDE;Integrated Security=SSPI;"
        Me.ConnLive.FireInfoMessageEventOnUserErrors = False
        '
        'CompassDemographicsForDisplay
        '
        Me.CompassDemographicsForDisplay.Demos = Nothing
        Me.CompassDemographicsForDisplay.Location = New System.Drawing.Point(646, 253)
        Me.CompassDemographicsForDisplay.Name = "CompassDemographicsForDisplay"
        Me.CompassDemographicsForDisplay.Size = New System.Drawing.Size(463, 239)
        Me.CompassDemographicsForDisplay.TabIndex = 39
        '
        'OneLINKDemographicsForDisplay
        '
        Me.OneLINKDemographicsForDisplay.Demos = Nothing
        Me.OneLINKDemographicsForDisplay.Location = New System.Drawing.Point(646, 12)
        Me.OneLINKDemographicsForDisplay.Name = "OneLINKDemographicsForDisplay"
        Me.OneLINKDemographicsForDisplay.Size = New System.Drawing.Size(465, 241)
        Me.OneLINKDemographicsForDisplay.TabIndex = 38
        '
        'CheckboxIndicatorsForEmail
        '
        Me.CheckboxIndicatorsForEmail.Indicators = Nothing
        Me.CheckboxIndicatorsForEmail.Location = New System.Drawing.Point(364, 183)
        Me.CheckboxIndicatorsForEmail.Name = "CheckboxIndicatorsForEmail"
        Me.CheckboxIndicatorsForEmail.Size = New System.Drawing.Size(218, 27)
        Me.CheckboxIndicatorsForEmail.TabIndex = 98
        '
        'CheckboxIndicatorsForOtherPhone2
        '
        Me.CheckboxIndicatorsForOtherPhone2.Indicators = Nothing
        Me.CheckboxIndicatorsForOtherPhone2.Location = New System.Drawing.Point(364, 159)
        Me.CheckboxIndicatorsForOtherPhone2.Name = "CheckboxIndicatorsForOtherPhone2"
        Me.CheckboxIndicatorsForOtherPhone2.Size = New System.Drawing.Size(218, 27)
        Me.CheckboxIndicatorsForOtherPhone2.TabIndex = 97
        '
        'CheckboxIndicatorsForOtherPhone
        '
        Me.CheckboxIndicatorsForOtherPhone.Indicators = Nothing
        Me.CheckboxIndicatorsForOtherPhone.Location = New System.Drawing.Point(364, 135)
        Me.CheckboxIndicatorsForOtherPhone.Name = "CheckboxIndicatorsForOtherPhone"
        Me.CheckboxIndicatorsForOtherPhone.Size = New System.Drawing.Size(218, 27)
        Me.CheckboxIndicatorsForOtherPhone.TabIndex = 96
        '
        'CheckboxIndicatorsForHomePhone
        '
        Me.CheckboxIndicatorsForHomePhone.Indicators = Nothing
        Me.CheckboxIndicatorsForHomePhone.Location = New System.Drawing.Point(364, 111)
        Me.CheckboxIndicatorsForHomePhone.Name = "CheckboxIndicatorsForHomePhone"
        Me.CheckboxIndicatorsForHomePhone.Size = New System.Drawing.Size(218, 27)
        Me.CheckboxIndicatorsForHomePhone.TabIndex = 95
        '
        'CheckboxIndicatorsForAddress
        '
        Me.CheckboxIndicatorsForAddress.Indicators = Nothing
        Me.CheckboxIndicatorsForAddress.Location = New System.Drawing.Point(364, 87)
        Me.CheckboxIndicatorsForAddress.Name = "CheckboxIndicatorsForAddress"
        Me.CheckboxIndicatorsForAddress.Size = New System.Drawing.Size(218, 27)
        Me.CheckboxIndicatorsForAddress.TabIndex = 94
        '
        'DemoForUpdate
        '
        Me.DemoForUpdate.Location = New System.Drawing.Point(6, 14)
        Me.DemoForUpdate.Name = "DemoForUpdate"
        Me.DemoForUpdate.Size = New System.Drawing.Size(360, 196)
        Me.DemoForUpdate.TabIndex = 89
        '
        'frmDemographics
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1128, 615)
        Me.Controls.Add(Me.lblAN)
        Me.Controls.Add(Me.Label55)
        Me.Controls.Add(Me.btnUnexpected)
        Me.Controls.Add(Me.btnBrightIdea)
        Me.Controls.Add(Me.lblDOB)
        Me.Controls.Add(Me.Label34)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblSSN)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmDemographics"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE Demographics"
        Me.gbBorrowerInfo.ResumeLayout(False)
        Me.gbBorrowerInfo.PerformLayout()
        Me.gbActivity.ResumeLayout(False)
        Me.gbActivity.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.ButtonBGPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Class Variables"
    'class variables (objects) these objects interact with Reflection
    Private Bor As Borrower
    Private ActivityRecs As SP.ActivityComments
    Private YN As SP.frmYesNo
    'arrays for activity and contact combo boxes
    Public Shared ActivityCode As ArrayList
    Public Shared ContactCode As ArrayList
    'arrays for LP22 and TX1J source codes
    Private LP22Source As ArrayList
    Private TX1JSource As ArrayList
    Private IsACP As Boolean
    Public BackButtonClicked As Boolean 'this is true if Back to Main Menu was sp.q.hited.
    Private FirstName As String
    Private MI As String
    Private LastName As String
    Private AddressWarning As Boolean 'this is true if the user has been warned of another valid address
    Private CLAccNum As String
    Private VerificationRequired As Boolean
    Private CompassAndOneLINKDiffer As Boolean
    Private Indicators As New Dictionary(Of String, CheckboxIndicatorsForDemographicPart)
    Private _contactPhone As String
    Public ReadOnly Property ContactPhone() As String
        Get
            Return _contactPhone
        End Get
    End Property
#End Region


    Public Overloads Function Showdialog(ByVal SSN As String, ByVal IsACPl As Boolean, ByVal TransPar As Double, ByVal BackColor As Color, ByVal ForeColor As Color) As Boolean
        'Parentfrm = frm
        BackButtonClicked = False
        'set opacity level
        Me.Opacity = TransPar
        'set backcolor
        Me.BackColor = BackColor
        'set forecolor
        Me.ForeColor = ForeColor
        IsACP = IsACPl
        Bor.RunSpecialComments = True
    End Function

    Public Overloads Function Showdialog(ByVal TVerificationRequired As Boolean, ByVal FromHomePage As Boolean) As Boolean
        BackButtonClicked = False
        If FromHomePage Then
            gbBorrowerInfo.Enabled = True
            OneLINKDemographicsForDisplay.Enabled = True
            CompassDemographicsForDisplay.Enabled = True
            gbActivity.Enabled = True
            DemoForUpdate.Addr.txtAddr1.Focus()
        Else
            gbBorrowerInfo.Enabled = False
            OneLINKDemographicsForDisplay.Enabled = False
            CompassDemographicsForDisplay.Enabled = False
            gbActivity.Enabled = True
            txtActivityCode.Focus()
        End If
        VerificationRequired = TVerificationRequired
        Me.ShowDialog()
    End Function

    Public Sub DisableForm()
        gbBorrowerInfo.Enabled = False
        OneLINKDemographicsForDisplay.Enabled = False
        CompassDemographicsForDisplay.Enabled = False
        gbActivity.Enabled = True
    End Sub

    Public Sub EnableForm()
        If txtActivityCode.Text.Length = 2 And txtContactCode.Text.Length = 2 Then
            gbBorrowerInfo.Enabled = True
            OneLINKDemographicsForDisplay.Enabled = True
            CompassDemographicsForDisplay.Enabled = True
            gbActivity.Enabled = True
            DemoForUpdate.Addr.txtAddr1.Focus()
        Else
            DisableForm()
        End If
    End Sub

    'main function for taking info from demographic objects and adding populating the form with it
    Public Function PopulateFrm(ByVal IsACPl As Boolean) As Boolean
        Dim FoundOnPreviousSystem As Boolean
        FoundOnPreviousSystem = False
        AddressWarning = False
        BackButtonClicked = False
        IsACP = IsACPl
        'initalize the form componenets (blank all data holding text boxes and labels)
        'Get special comments as listed in database
        Bor.GetSpecialComment()
        'get Userid
        UsrInf.GetUserIDFromLP40I()
        txtActivityCode.Focus()
        DisableForm()

        'set SSN label value 
        lblSSN.Text = Bor.SSN
        'set system labels if found on applicable system
        If Bor.OneLINKDemos.FoundOnSystem Then
            FoundOnPreviousSystem = True 'borrower was found on OneLINK
            If Bor.OneLINKDemos.State = "FC" Then
                SP.frmWhoaDUDE.WhoaDUDE("Sorry DUDE, this borrower is way outta town.  You'll hafta update their foreign address or phone manually.", "Knarly DUDE", True)
                Return False
                Exit Function
            Else
                OneLINKDemographicsForDisplay.DataBind(Bor.OneLINKDemos)
            End If
        End If
        If Bor.CompassDemos.FoundOnSystem Then
            FoundOnPreviousSystem = True 'borrower's information found on COMPASS
            If Bor.CompassDemos.State = "" Then 'check for foreign state
                SP.frmWhoaDUDE.WhoaDUDE("Sorry DUDE, this borrower is way outta town.  You'll hafta update their foreign address or phone manually.", "Knarly DUDE", True)
                Return False
                Exit Function
            Else
                CompassDemographicsForDisplay.DataBind(Bor.CompassDemos)
            End If
        End If
        If FoundOnPreviousSystem Then
            lblName.Text = Bor.Name
            lblDOB.Text = Bor.DOB
            lblAN.Text = Bor.CLAccNum
            SP.Processing.Visible = False
            OneLINKDemographicsForDisplay.HideOrShowDataLabels()
            CompassDemographicsForDisplay.HideOrShowDataLabels()
            DemoForUpdate.PrePopulateCheck(CompassAndOneLINKDiffer, Bor)
            SP.BorrInfo.ShowDialog(lblSSN.Text, lblName.Text)
            'Me.Showdialog()
            Return True
        Else
            If FoundOnPreviousSystem = False Then
                'Check LCO for address
                SP.Processing.Visible = False
                'get address from LCO
                If Bor.UserProvidedDemos.FoundOnSystem Then
                    lblName.Text = Bor.Name
                    lblDOB.Text = Bor.DOB
                    OneLINKDemographicsForDisplay.HideOrShowDataLabels()
                    CompassDemographicsForDisplay.HideOrShowDataLabels()
                    'Load demographics from Borrower class if its LCO or if OneLINK and Compass Match
                    DemoForUpdate.PrePopulateCheck(CompassAndOneLINKDiffer, Bor)
                    Return True
                Else
                    Return False
                End If
            End If
        End If
        Return False
    End Function

    Private Sub frmDemographics_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        Me.Dispose()
    End Sub

    'this function figures what needs to be updated on TX1J and LP22
    Public Overridable Sub WhatToUpdate(ByVal Indicators As UpdateDemoCompassIndicators)


        If ckbNoPhone.Checked Then
            Indicators.PhoneNoPhoneIndicator = True
        End If

        'COMPASS 
        Indicators.Address = CheckboxIndicatorsForAddress.ckbNotValid.Checked = False
        Indicators.Phone = CheckboxIndicatorsForHomePhone.ckbNotValid.Checked = False
        Indicators.OtherPhone = CheckboxIndicatorsForOtherPhone.ckbNotValid.Checked = False
        Indicators.Other2Phone = CheckboxIndicatorsForOtherPhone2.ckbNotValid.Checked = False
        Indicators.Email = CheckboxIndicatorsForEmail.ckbNotValid.Checked = False

        Indicators.AddressIndicator = (Bor.CompassDemos.SPAddrInd = "N" And CheckboxIndicatorsForAddress.ckbVerified.Checked)
        Indicators.PhoneIndicator = True ' ((CheckboxIndicatorsForHomePhone.ckbVerified.Checked And CheckboxIndicatorsForHomePhone.ckbNotValid.Checked = False) And DemoForUpdate.HomePhn.txtPhone1.Text <> "" And DemoForUpdate.HomePhn.txtPhone2.Text <> "" And DemoForUpdate.HomePhn.txtPhone3.Text <> "")
        Indicators.OtherPhoneIndicator = True ' ((CheckboxIndicatorsForOtherPhone.ckbVerified.Checked And CheckboxIndicatorsForOtherPhone.ckbNotValid.Checked = False) And DemoForUpdate.OtherPhn.txtPhone1.Text <> "" And DemoForUpdate.OtherPhn.txtPhone2.Text <> "" And DemoForUpdate.OtherPhn.txtPhone3.Text <> "")
        Indicators.Other2PhoneIndicator = True ' ((CheckboxIndicatorsForOtherPhone2.ckbVerified.Checked And CheckboxIndicatorsForOtherPhone2.ckbNotValid.Checked = False) And DemoForUpdate.Other2Phn.txtPhone1.Text <> "" And DemoForUpdate.Other2Phn.txtPhone2.Text <> "" And DemoForUpdate.Other2Phn.txtPhone3.Text <> "")
        Indicators.EmailIndicator = (CheckboxIndicatorsForEmail.ckbVerified.Checked And CheckboxIndicatorsForEmail.ckbNotValid.Checked = False) And DemoForUpdate.txtEmail.Text <> "" Or (CheckboxIndicatorsForEmail.ckbNotValid.Checked And Bor.CompassDemos.SPEmailInd = "Y")

        If CheckboxIndicatorsForAddress.ckbNotValid.Checked Then
            Indicators.AddressIndicator = (DemoForUpdate.Addr.txtAddr1.Text = Bor.CompassDemos.Addr1 And DemoForUpdate.Addr.txtAddr2.Text = Bor.CompassDemos.Addr2 And DemoForUpdate.Addr.txtCity.Text = Bor.CompassDemos.City And DemoForUpdate.Addr.cbState.Text = Bor.CompassDemos.State And DemoForUpdate.Addr.txtZip.Text = Bor.CompassDemos.Zip)
        End If

    End Sub

    'this function coordinates the matching between the Activity combo box and the applicable text box
    Private Sub cbActivityDesc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbActivityDesc.SelectedIndexChanged
        If cbActivityDesc.SelectedIndex <> 0 Then
            txtActivityCode.Text = ActivityCode(cbActivityDesc.SelectedIndex - 1)
        Else
            txtActivityCode.Text = ""
        End If
    End Sub

    'this function coordinates the matching between the contact combo box and the applicable text box
    Private Sub cbContactDesc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbContactDesc.SelectedIndexChanged
        If cbContactDesc.SelectedIndex <> 0 Then

            txtContactCode.Text = ContactCode(cbContactDesc.SelectedIndex - 1)

        Else
            txtContactCode.Text = ""
        End If
    End Sub

    Private Sub ckbNoPhone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbNoPhone.CheckedChanged
        DemoForUpdate.NoPhone(ckbNoPhone.Checked)
        If ckbNoPhone.Checked Then
            CheckboxIndicatorsForHomePhone.ckbNotValid.Checked = True
            CheckboxIndicatorsForOtherPhone.ckbNotValid.Checked = True
            CheckboxIndicatorsForOtherPhone2.ckbNotValid.Checked = True
            CheckboxIndicatorsForHomePhone.ckbNotValid.Enabled = False
            CheckboxIndicatorsForOtherPhone.ckbNotValid.Enabled = False
            CheckboxIndicatorsForOtherPhone2.ckbNotValid.Enabled = False
            CheckboxIndicatorsForHomePhone.ckbVerified.Enabled = False
            CheckboxIndicatorsForOtherPhone.ckbVerified.Enabled = False
            CheckboxIndicatorsForOtherPhone2.ckbVerified.Enabled = False
        Else
            CheckboxIndicatorsForHomePhone.ckbNotValid.Enabled = True
            CheckboxIndicatorsForOtherPhone.ckbNotValid.Enabled = True
            CheckboxIndicatorsForOtherPhone2.ckbNotValid.Enabled = True
            CheckboxIndicatorsForHomePhone.ckbVerified.Enabled = True
            CheckboxIndicatorsForOtherPhone.ckbVerified.Enabled = True
            CheckboxIndicatorsForOtherPhone2.ckbVerified.Enabled = True
        End If
    End Sub

    'this function coordinates the matching of the activity code text box and the applicable description combo box
    Private Sub txtActivityCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtActivityCode.TextChanged
        Dim counter As Integer
        Dim Found As Boolean
        If txtActivityCode.TextLength = 2 Then
            While counter < ActivityCode.Count And Found = False
                If UCase(txtActivityCode.Text) = ActivityCode(counter) Then
                    Found = True
                End If
                counter = counter + 1
            End While
            'if a match was found then move to the next box else stay in textbox and highlight text
            If Found Then
                cbActivityDesc.SelectedIndex = counter
                txtActivityCode.Text = txtActivityCode.Text.ToUpper
                txtContactCode.Focus()
                If txtActivityCode.Text = "TE" Or _
                txtActivityCode.Text = "TT" Or _
                txtActivityCode.Text = "T1" Or _
                txtActivityCode.Text = "T2" Or _
                txtActivityCode.Text = "T3" Or _
                txtActivityCode.Text = "T4" Or _
                txtActivityCode.Text = "T5" Or _
                txtActivityCode.Text = "T6" Or _
                txtActivityCode.Text = "T7" Or _
                txtActivityCode.Text = "T8" Then
                    cboAttempt.Visible = True
                    Label52.Visible = True
                Else
                    Label52.Visible = False
                    cboAttempt.Visible = False
                    cboAttempt.Text = String.Empty
                End If
            Else
                SP.frmWhoaDUDE.WhoaDUDE("DUDE couldn't do the hula with that entry.", "Bad Activity Code", True)
                txtActivityCode.Focus()
                txtActivityCode.SelectAll()
            End If
            EnableForm()
            txtContactCode.Focus()
        End If
    End Sub

    'this function coordinates the matching of the contact code text box and the applicable description combo box
    Private Sub txtContactCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtContactCode.TextChanged
        Dim counter As Integer
        Dim Found As Boolean
        If txtContactCode.TextLength = 2 Then
            While counter < ContactCode.Count And Found = False
                If UCase(txtContactCode.Text) = ContactCode(counter) Then
                    Found = True
                End If
                counter = counter + 1
            End While
            'if a match was found then move to the next box else stay in textbox and highlight text
            If Found Then
                cbContactDesc.SelectedIndex = counter
                txtContactCode.Text = txtContactCode.Text.ToUpper
                If cboAttempt.Visible Then
                    cboAttempt.Focus()
                Else
                    btnSAndC.Focus()
                End If

            Else
                SP.frmWhoaDUDE.WhoaDUDE("DUDE couldn't do the hula with that entry.", "Bad Contact Code", True)
                txtContactCode.Focus()
                txtContactCode.SelectAll()
            End If
            EnableForm()
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        BackButtonClicked = True
        Me.Hide()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        DemoForUpdate.ResetControls()
        CheckboxIndicatorsForAddress.ckbVerified.Checked = False
        CheckboxIndicatorsForAddress.ckbNotValid.Checked = False
        CheckboxIndicatorsForHomePhone.ckbVerified.Checked = False
        CheckboxIndicatorsForOtherPhone.ckbVerified.Checked = False
        CheckboxIndicatorsForOtherPhone2.ckbVerified.Checked = False
        CheckboxIndicatorsForEmail.ckbVerified.Checked = False
        CheckboxIndicatorsForHomePhone.ckbNotValid.Checked = False
        CheckboxIndicatorsForOtherPhone.ckbNotValid.Checked = False
        CheckboxIndicatorsForOtherPhone2.ckbNotValid.Checked = False
        CheckboxIndicatorsForEmail.ckbNotValid.Checked = False
        ckbNoPhone.Checked = False
        cbActivityDesc.SelectedIndex = 0
        cbContactDesc.SelectedIndex = 0
        'check if COMPASS and OneLINK info is the same if so then populate textboxes
        DemoForUpdate.PrePopulateCheck(CompassAndOneLINKDiffer, Bor)
        DemoForUpdate.Addr.txtAddr1.Focus() 'give the addr1 field focus.
    End Sub

    Public Overridable Sub UpdateSys()
        Dim Indicators As New UpdateDemoCompassIndicators()
        Dim IsSchool As Boolean 'tracks whether to do the double entry thing
        Dim CommentO As String
        Dim CommentC As String
        If DemoForUpdate.ValidUserInput(VerificationRequired, AddressWarning, CompassAndOneLINKDiffer, Bor) Then
            AddressWarning = False
            DemoForUpdate.Addr.txtAddr1.Focus() 'give the addr1 field focus for when the form reappears.
            'create comment string
            CommentO = DemoForUpdate.CreateCommentString(Bor.OneLINKDemos)
            CommentC = DemoForUpdate.CreateCommentString(Bor.CompassDemos)
            SP.Processing.Show()
            Me.Visible = False
            'Processing.Visible = True 'display processing window
            SP.Processing.Refresh() 'the form doesn't get written to the screen without this
            'decide whether to add comments to OneLINK only or OneLINK and COMPASS
            If Bor.BorrowerIsLCOOnly = False Then
                If Bor.CompassDemos.Addr1 = "" And Bor.OneLINKDemos.Addr1 <> "" Then
                    'OneLINK only
                    If Bor.ActivityCmts.AddCommentsToLP50(CommentO, "MXADD", Bor.ContactCode, Bor.ActivityCode) = False Then
                        Bor.SpillGuts()
                        Process.GetCurrentProcess.Kill()
                    End If
                ElseIf Bor.CompassDemos.Addr1 <> "" And Bor.OneLINKDemos.Addr1 = "" Then
                    'COMPASS only
                    If Bor.ActivityCmts.AddCommentsToTD22AllLoans(CommentC, "MXADD") = False Then
                        Bor.SpillGuts()
                        Process.GetCurrentProcess.Kill()
                    End If
                Else
                    'Both systems
                    If Bor.ActivityCmts.AddCommentsToLP50AndTD22(CommentO, CommentC, "MXADD", Bor.ContactCode, Bor.ActivityCode, "MXADD") = False Then
                        Bor.SpillGuts()
                        Process.GetCurrentProcess.Kill()
                    End If
                End If
            End If

            WhatToUpdate(Indicators)

            'if info was gathered for COMPASS do COMPASS processing
            If Bor.BorrowerIsLCOOnly = False Then
                If Bor.CompassDemos.Addr1 <> "" Then
                    'update compass only if data was pulled from COMPASS
                    If cbContactDesc.SelectedIndex = 7 Or cbContactDesc.SelectedIndex = 8 Then
                        IsSchool = True
                    End If
                    Demographics.UpdateCOMPASSSystem(TX1JSource(cbContactDesc.SelectedIndex - 1), Indicators, IsSchool, Bor.UserProvidedDemos, CreateDUDEProcessingDictionaryCopy(), Bor.AltAddress)
                End If
            End If
            'decide whether to go to LP22
            Demographics.UpdateOneLINKSystem(LP22Source(cbContactDesc.SelectedIndex - 1), Indicators, Bor.UserProvidedDemos, CreateDUDEProcessingDictionaryCopy(), Bor.AltAddress)
            'Update LCO Demographics
            Demographics.UpdateLCOSystem(Bor.UserProvidedDemos)

            SP.Processing.Hide()
            Me.Hide()
        End If
    End Sub

    Private Function CreateDUDEProcessingDictionaryCopy() As Dictionary(Of String, CheckboxIndicatorsForDemographicPart)
        Dim processingCopy As New Dictionary(Of String, CheckboxIndicatorsForDemographicPart)
        'remove handlers so check boxes can be manipulated during processing
        For Each indicatorSet As KeyValuePair(Of String, CheckboxIndicatorsForDemographicPart) In Indicators
            processingCopy.Add(indicatorSet.Key, indicatorSet.Value.CloneWithCheckboxesMarked()) 'create entry in copy and remove checkbox event handlers
        Next
        Return processingCopy
    End Function

    Private Sub btnSAndC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSAndC.Click
        Dim line As String
        Dim UserI() As String
        If DemoForUpdate.ValidUserInput(VerificationRequired, AddressWarning, CompassAndOneLINKDiffer, Bor) Then
            DemoForUpdate.PopulateDemographicObject(Bor)
            Bor.SpillGuts()
            If IsACP Then
                Me.Hide()
            Else
                UpdateSys()
                'return to favorite screen
                Try
                    If Dir$("T:\userinfo.txt") <> "" Then
                        FileOpen(1, "T:\UserInfo.txt", OpenMode.Input, OpenAccess.Read)
                        line = LineInput(1)
                        UserI = line.Split(CChar(", "))
                        If UserI(5) <> "" Then
                            SP.Q.FastPath(Replace(UserI(5), """", "")) 'remove quotes
                        End If
                        FileClose(1)
                    End If
                Catch ex As Exception

                End Try
            End If

        End If
    End Sub

    'This sub ensures that addr1 is in focus if when ever the form is activated
    Private Sub frmDemographics_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        DemoForUpdate.Addr.txtAddr1.Focus()
    End Sub

    'display the dictionary form
    Private Sub MenuDictionary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDictionary.Click
        SP.DisplayHawaiianDictionary()
    End Sub

    'display the about DUDE form
    Private Sub MenuAboutDude_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAboutDude.Click
        SP.DisplayAboutDude()
    End Sub

    'display the borrower information form
    Private Sub MenuBorrInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuBorrInfo.Click
        SP.BorrInfo.Show(Bor.SSN, Bor.Name)
    End Sub

    'display the Ask DUDE information form
    Private Sub MenuAskDUDE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAskDUDE.Click
        SP.DisplayAskDude()
    End Sub

    Private Sub btnHygiene_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHygiene.Click
        Dim Hygiene As frmHygiene
        Hygiene = New frmHygiene
        Hygiene.Show()
    End Sub

    Private Sub cboAttempt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAttempt.SelectedIndexChanged
        btnSAndC.Focus()
        Bor.AttemptType = cboAttempt.Text
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        conn.Open()
        cmd.Connection = conn
        If txtActivityCode.Text = "TT" Or txtActivityCode.Text = "TE" Then
            cmd.CommandText = "SELECT TCX01 FROM ContactCode WHERE ActivityCode = '" + txtActivityCode.Text + "' AND ContactCode = '" + txtContactCode.Text + "'  AND Description = '" + cboAttempt.Text + "'"
        Else
            cmd.CommandText = "SELECT TCX01 FROM ContactCode WHERE ActivityCode = '" + txtActivityCode.Text + "' AND ContactCode = '" + txtContactCode.Text + "'"
        End If
        reader = cmd.ExecuteReader
        While reader.Read
            Bor.BorLite.ACPSelection = reader.Item("TCX01").ToString().Trim()
        End While
        conn.Close()
    End Sub

    Private Sub btnBrightIdea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrightIdea.Click
        If SP.frmEmailComments.BrightIdea() = True Then
            AppActivate(Me.Text)
            Exit Sub
        End If
        AppActivate(Me.Text)
    End Sub

    Private Sub btnUnexpected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnexpected.Click
        If SP.frmEmailComments.UnexpectedResults() = True Then
            AppActivate(Me.Text)
            Exit Sub
        End If
        AppActivate(Me.Text)
    End Sub


    Private Sub txtActivityCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtActivityCode.GotFocus
        If Bor.RunSpecialComments Then
            Bor.ShowSpecialComments()
            Bor.RunSpecialComments = False
        End If
    End Sub

    Private Sub ckbAddAlternateAddress_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbAddAlternateAddress.CheckedChanged
        If ckbAddAlternateAddress.Checked And CheckboxIndicatorsForAddress.ckbNotValid.Checked Then
            frmWhoaDUDE.WhoaDUDE("DUDE can't create an alternate address unless you provide a valid legal address.  Please try again", "Need Valid Address")
            ckbAddAlternateAddress.Checked = False
        ElseIf ckbAddAlternateAddress.Checked And CheckboxIndicatorsForAddress.ckbInvalidateFirst.Checked Then
            frmWhoaDUDE.WhoaDUDE("DUDE can't do adding alternate address and invalidate first functionality at the same time.  Please try again.", "Need Valid Data")
            ckbAddAlternateAddress.Checked = False
        ElseIf ckbAddAlternateAddress.Checked And CheckboxIndicatorsForAddress.ckbVerified.Checked = False Then
            frmWhoaDUDE.WhoaDUDE("DUDE must have a valid, verified legal address before you can provide an alternate address.  Please provide a valid address and check the verified checkbox to continue.", "Need Valid Data")
            ckbAddAlternateAddress.Checked = False
        ElseIf ckbAddAlternateAddress.Checked Then
            Bor.AltAddress = New Demographics(Bor.SSN)
            Dim altAddressForm As New frmAltAddress(Bor.AltAddress)
            If altAddressForm.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Bor.AltAddress = Nothing
                ckbAddAlternateAddress.Checked = False
            End If
        ElseIf ckbAddAlternateAddress.Checked = False Then
            Bor.AltAddress = Nothing
        End If
    End Sub

End Class