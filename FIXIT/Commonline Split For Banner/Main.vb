Public Class MainForm
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
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblCLSource As System.Windows.Forms.Label
    Friend WithEvents Browse1 As System.Windows.Forms.Button
    Friend WithEvents Browse2 As System.Windows.Forms.Button
    Friend WithEvents lblOldCL As System.Windows.Forms.Label
    Friend WithEvents Browse3 As System.Windows.Forms.Button
    Friend WithEvents Browse4 As System.Windows.Forms.Button
    Friend WithEvents lblArchive As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents FBD As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Browse6 As System.Windows.Forms.Button
    Friend WithEvents lblDepositAppSend As System.Windows.Forms.Label
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents Browse5 As System.Windows.Forms.Button
    Friend WithEvents lblSourceAppSend As System.Windows.Forms.Label
    Friend WithEvents cbDummyFunc As System.Windows.Forms.CheckBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Browse8 As System.Windows.Forms.Button
    Friend WithEvents Browse10 As System.Windows.Forms.Button
    Friend WithEvents Browse7 As System.Windows.Forms.Button
    Friend WithEvents Browse9 As System.Windows.Forms.Button
    Friend WithEvents lbEFTOLD As System.Windows.Forms.Label
    Friend WithEvents lbEFTArchive As System.Windows.Forms.Label
    Friend WithEvents lbEFTSource As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents GroupBox14 As System.Windows.Forms.GroupBox
    Friend WithEvents lblCurrent As System.Windows.Forms.Label
    Friend WithEvents lblGradPLUS As System.Windows.Forms.Label
    Friend WithEvents BrowseGradPLUS As System.Windows.Forms.Button
    Friend WithEvents radPLUS As System.Windows.Forms.RadioButton
    Friend WithEvents radLP As System.Windows.Forms.RadioButton
    Friend WithEvents radNone As System.Windows.Forms.RadioButton
    Friend WithEvents radBoth As System.Windows.Forms.RadioButton
    Friend WithEvents gbOldRes As System.Windows.Forms.GroupBox
    Friend WithEvents gbCurrRes As System.Windows.Forms.GroupBox
    Friend WithEvents gbArchRes As System.Windows.Forms.GroupBox
    Friend WithEvents gbGradRes As System.Windows.Forms.GroupBox
    Friend WithEvents gboldEFT As System.Windows.Forms.GroupBox
    Friend WithEvents gbGradEFT As System.Windows.Forms.GroupBox
    Friend WithEvents gbCurrEFT As System.Windows.Forms.GroupBox
    Friend WithEvents lbEFTCurr As System.Windows.Forms.Label
    Friend WithEvents lblEFTGradPLUS As System.Windows.Forms.Label
    Friend WithEvents BrowseGradPLUSEFT As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbBannerSchool As System.Windows.Forms.RadioButton
    Friend WithEvents rbUofU As System.Windows.Forms.RadioButton
    Friend WithEvents rbBYU As System.Windows.Forms.RadioButton
    Friend WithEvents gbTrig As System.Windows.Forms.GroupBox
    Friend WithEvents tbTriggerDate As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Browse1 = New System.Windows.Forms.Button
        Me.lblCLSource = New System.Windows.Forms.Label
        Me.gbOldRes = New System.Windows.Forms.GroupBox
        Me.Browse2 = New System.Windows.Forms.Button
        Me.lblOldCL = New System.Windows.Forms.Label
        Me.gbCurrRes = New System.Windows.Forms.GroupBox
        Me.Browse3 = New System.Windows.Forms.Button
        Me.lblCurrent = New System.Windows.Forms.Label
        Me.gbArchRes = New System.Windows.Forms.GroupBox
        Me.Browse4 = New System.Windows.Forms.Button
        Me.lblArchive = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.FBD = New System.Windows.Forms.FolderBrowserDialog
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.gbGradRes = New System.Windows.Forms.GroupBox
        Me.BrowseGradPLUS = New System.Windows.Forms.Button
        Me.lblGradPLUS = New System.Windows.Forms.Label
        Me.GroupBox14 = New System.Windows.Forms.GroupBox
        Me.radNone = New System.Windows.Forms.RadioButton
        Me.radBoth = New System.Windows.Forms.RadioButton
        Me.radPLUS = New System.Windows.Forms.RadioButton
        Me.radLP = New System.Windows.Forms.RadioButton
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.cbDummyFunc = New System.Windows.Forms.CheckBox
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.Browse6 = New System.Windows.Forms.Button
        Me.lblDepositAppSend = New System.Windows.Forms.Label
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.Browse5 = New System.Windows.Forms.Button
        Me.lblSourceAppSend = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.gbGradEFT = New System.Windows.Forms.GroupBox
        Me.BrowseGradPLUSEFT = New System.Windows.Forms.Button
        Me.lblEFTGradPLUS = New System.Windows.Forms.Label
        Me.gboldEFT = New System.Windows.Forms.GroupBox
        Me.Browse8 = New System.Windows.Forms.Button
        Me.lbEFTOLD = New System.Windows.Forms.Label
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.Browse10 = New System.Windows.Forms.Button
        Me.lbEFTArchive = New System.Windows.Forms.Label
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.Browse7 = New System.Windows.Forms.Button
        Me.lbEFTSource = New System.Windows.Forms.Label
        Me.gbCurrEFT = New System.Windows.Forms.GroupBox
        Me.Browse9 = New System.Windows.Forms.Button
        Me.lbEFTCurr = New System.Windows.Forms.Label
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.lblVersion = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbBYU = New System.Windows.Forms.RadioButton
        Me.rbUofU = New System.Windows.Forms.RadioButton
        Me.rbBannerSchool = New System.Windows.Forms.RadioButton
        Me.gbTrig = New System.Windows.Forms.GroupBox
        Me.tbTriggerDate = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.gbOldRes.SuspendLayout()
        Me.gbCurrRes.SuspendLayout()
        Me.gbArchRes.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.gbGradRes.SuspendLayout()
        Me.GroupBox14.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.gbGradEFT.SuspendLayout()
        Me.gboldEFT.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.gbCurrEFT.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.gbTrig.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Browse1)
        Me.GroupBox1.Controls.Add(Me.lblCLSource)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(40, 24)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(448, 48)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Browse To Directory for Response Files"
        '
        'Browse1
        '
        Me.Browse1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse1.Location = New System.Drawing.Point(360, 24)
        Me.Browse1.Name = "Browse1"
        Me.Browse1.Size = New System.Drawing.Size(75, 16)
        Me.Browse1.TabIndex = 1
        Me.Browse1.Text = "Browse"
        '
        'lblCLSource
        '
        Me.lblCLSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCLSource.Location = New System.Drawing.Point(16, 24)
        Me.lblCLSource.Name = "lblCLSource"
        Me.lblCLSource.Size = New System.Drawing.Size(336, 16)
        Me.lblCLSource.TabIndex = 0
        '
        'gbOldRes
        '
        Me.gbOldRes.Controls.Add(Me.Browse2)
        Me.gbOldRes.Controls.Add(Me.lblOldCL)
        Me.gbOldRes.Enabled = False
        Me.gbOldRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbOldRes.Location = New System.Drawing.Point(40, 74)
        Me.gbOldRes.Name = "gbOldRes"
        Me.gbOldRes.Size = New System.Drawing.Size(448, 48)
        Me.gbOldRes.TabIndex = 1
        Me.gbOldRes.TabStop = False
        Me.gbOldRes.Text = "Browse To Directory for Deposit of Old System Response Files"
        '
        'Browse2
        '
        Me.Browse2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse2.Location = New System.Drawing.Point(360, 24)
        Me.Browse2.Name = "Browse2"
        Me.Browse2.Size = New System.Drawing.Size(75, 16)
        Me.Browse2.TabIndex = 1
        Me.Browse2.Text = "Browse"
        '
        'lblOldCL
        '
        Me.lblOldCL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblOldCL.Location = New System.Drawing.Point(16, 24)
        Me.lblOldCL.Name = "lblOldCL"
        Me.lblOldCL.Size = New System.Drawing.Size(336, 16)
        Me.lblOldCL.TabIndex = 0
        '
        'gbCurrRes
        '
        Me.gbCurrRes.Controls.Add(Me.Browse3)
        Me.gbCurrRes.Controls.Add(Me.lblCurrent)
        Me.gbCurrRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbCurrRes.Location = New System.Drawing.Point(40, 124)
        Me.gbCurrRes.Name = "gbCurrRes"
        Me.gbCurrRes.Size = New System.Drawing.Size(448, 48)
        Me.gbCurrRes.TabIndex = 2
        Me.gbCurrRes.TabStop = False
        Me.gbCurrRes.Text = "Browse To Directory for Deposit of Current Response Files"
        '
        'Browse3
        '
        Me.Browse3.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse3.Location = New System.Drawing.Point(360, 24)
        Me.Browse3.Name = "Browse3"
        Me.Browse3.Size = New System.Drawing.Size(75, 16)
        Me.Browse3.TabIndex = 1
        Me.Browse3.Text = "Browse"
        '
        'lblCurrent
        '
        Me.lblCurrent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCurrent.Location = New System.Drawing.Point(16, 24)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(336, 16)
        Me.lblCurrent.TabIndex = 0
        '
        'gbArchRes
        '
        Me.gbArchRes.Controls.Add(Me.Browse4)
        Me.gbArchRes.Controls.Add(Me.lblArchive)
        Me.gbArchRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbArchRes.Location = New System.Drawing.Point(40, 224)
        Me.gbArchRes.Name = "gbArchRes"
        Me.gbArchRes.Size = New System.Drawing.Size(448, 48)
        Me.gbArchRes.TabIndex = 3
        Me.gbArchRes.TabStop = False
        Me.gbArchRes.Text = "Browse To Directory for Archive Response Files"
        '
        'Browse4
        '
        Me.Browse4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse4.Location = New System.Drawing.Point(360, 24)
        Me.Browse4.Name = "Browse4"
        Me.Browse4.Size = New System.Drawing.Size(75, 16)
        Me.Browse4.TabIndex = 1
        Me.Browse4.Text = "Browse"
        '
        'lblArchive
        '
        Me.lblArchive.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblArchive.Location = New System.Drawing.Point(16, 24)
        Me.lblArchive.Name = "lblArchive"
        Me.lblArchive.Size = New System.Drawing.Size(336, 16)
        Me.lblArchive.TabIndex = 0
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(232, 664)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.gbOldRes)
        Me.GroupBox5.Controls.Add(Me.gbArchRes)
        Me.GroupBox5.Controls.Add(Me.GroupBox1)
        Me.GroupBox5.Controls.Add(Me.gbCurrRes)
        Me.GroupBox5.Controls.Add(Me.gbGradRes)
        Me.GroupBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(504, 288)
        Me.GroupBox5.TabIndex = 5
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Response File Splitting"
        '
        'gbGradRes
        '
        Me.gbGradRes.Controls.Add(Me.BrowseGradPLUS)
        Me.gbGradRes.Controls.Add(Me.lblGradPLUS)
        Me.gbGradRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbGradRes.Location = New System.Drawing.Point(40, 174)
        Me.gbGradRes.Name = "gbGradRes"
        Me.gbGradRes.Size = New System.Drawing.Size(448, 48)
        Me.gbGradRes.TabIndex = 4
        Me.gbGradRes.TabStop = False
        Me.gbGradRes.Text = "Browse To Directory for GradPLUS Response Files"
        '
        'BrowseGradPLUS
        '
        Me.BrowseGradPLUS.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BrowseGradPLUS.Location = New System.Drawing.Point(360, 24)
        Me.BrowseGradPLUS.Name = "BrowseGradPLUS"
        Me.BrowseGradPLUS.Size = New System.Drawing.Size(75, 16)
        Me.BrowseGradPLUS.TabIndex = 1
        Me.BrowseGradPLUS.Text = "Browse"
        '
        'lblGradPLUS
        '
        Me.lblGradPLUS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblGradPLUS.Location = New System.Drawing.Point(16, 24)
        Me.lblGradPLUS.Name = "lblGradPLUS"
        Me.lblGradPLUS.Size = New System.Drawing.Size(336, 16)
        Me.lblGradPLUS.TabIndex = 0
        '
        'GroupBox14
        '
        Me.GroupBox14.Controls.Add(Me.radNone)
        Me.GroupBox14.Controls.Add(Me.radBoth)
        Me.GroupBox14.Controls.Add(Me.radPLUS)
        Me.GroupBox14.Controls.Add(Me.radLP)
        Me.GroupBox14.Location = New System.Drawing.Point(8, 168)
        Me.GroupBox14.Name = "GroupBox14"
        Me.GroupBox14.Size = New System.Drawing.Size(528, 72)
        Me.GroupBox14.TabIndex = 4
        Me.GroupBox14.TabStop = False
        Me.GroupBox14.Text = "Select One of the Following Options"
        '
        'radNone
        '
        Me.radNone.Checked = True
        Me.radNone.Location = New System.Drawing.Point(264, 40)
        Me.radNone.Name = "radNone"
        Me.radNone.Size = New System.Drawing.Size(256, 24)
        Me.radNone.TabIndex = 3
        Me.radNone.TabStop = True
        Me.radNone.Text = "None"
        '
        'radBoth
        '
        Me.radBoth.Enabled = False
        Me.radBoth.Location = New System.Drawing.Point(8, 40)
        Me.radBoth.Name = "radBoth"
        Me.radBoth.Size = New System.Drawing.Size(256, 24)
        Me.radBoth.TabIndex = 2
        Me.radBoth.Text = "Both Loan Period and Graduate PLUS Functionality"
        '
        'radPLUS
        '
        Me.radPLUS.Enabled = False
        Me.radPLUS.Location = New System.Drawing.Point(264, 16)
        Me.radPLUS.Name = "radPLUS"
        Me.radPLUS.Size = New System.Drawing.Size(256, 24)
        Me.radPLUS.TabIndex = 1
        Me.radPLUS.Text = "Graduate PLUS Functionality Only"
        '
        'radLP
        '
        Me.radLP.Location = New System.Drawing.Point(8, 16)
        Me.radLP.Name = "radLP"
        Me.radLP.Size = New System.Drawing.Size(256, 24)
        Me.radLP.TabIndex = 0
        Me.radLP.Text = "Loan Period Functionality Only"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.cbDummyFunc)
        Me.GroupBox6.Controls.Add(Me.GroupBox7)
        Me.GroupBox6.Controls.Add(Me.GroupBox8)
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(504, 152)
        Me.GroupBox6.TabIndex = 6
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "App Send File Correction"
        '
        'cbDummyFunc
        '
        Me.cbDummyFunc.Location = New System.Drawing.Point(40, 24)
        Me.cbDummyFunc.Name = "cbDummyFunc"
        Me.cbDummyFunc.Size = New System.Drawing.Size(376, 24)
        Me.cbDummyFunc.TabIndex = 6
        Me.cbDummyFunc.Text = "Check to use App Send File Correction functionality."
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Browse6)
        Me.GroupBox7.Controls.Add(Me.lblDepositAppSend)
        Me.GroupBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox7.Location = New System.Drawing.Point(40, 96)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(448, 48)
        Me.GroupBox7.TabIndex = 5
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Browse To Directory for Deposit of App Send Files"
        '
        'Browse6
        '
        Me.Browse6.Enabled = False
        Me.Browse6.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse6.Location = New System.Drawing.Point(360, 24)
        Me.Browse6.Name = "Browse6"
        Me.Browse6.Size = New System.Drawing.Size(75, 16)
        Me.Browse6.TabIndex = 1
        Me.Browse6.Text = "Browse"
        '
        'lblDepositAppSend
        '
        Me.lblDepositAppSend.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDepositAppSend.Location = New System.Drawing.Point(16, 24)
        Me.lblDepositAppSend.Name = "lblDepositAppSend"
        Me.lblDepositAppSend.Size = New System.Drawing.Size(336, 16)
        Me.lblDepositAppSend.TabIndex = 0
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.Browse5)
        Me.GroupBox8.Controls.Add(Me.lblSourceAppSend)
        Me.GroupBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox8.Location = New System.Drawing.Point(40, 48)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(448, 48)
        Me.GroupBox8.TabIndex = 4
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Browse To Directory for Source of App Send Files"
        '
        'Browse5
        '
        Me.Browse5.Enabled = False
        Me.Browse5.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse5.Location = New System.Drawing.Point(360, 24)
        Me.Browse5.Name = "Browse5"
        Me.Browse5.Size = New System.Drawing.Size(75, 16)
        Me.Browse5.TabIndex = 1
        Me.Browse5.Text = "Browse"
        '
        'lblSourceAppSend
        '
        Me.lblSourceAppSend.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSourceAppSend.Location = New System.Drawing.Point(16, 24)
        Me.lblSourceAppSend.Name = "lblSourceAppSend"
        Me.lblSourceAppSend.Size = New System.Drawing.Size(336, 16)
        Me.lblSourceAppSend.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(192, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(136, 80)
        Me.PictureBox1.TabIndex = 7
        Me.PictureBox1.TabStop = False
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.gbGradEFT)
        Me.GroupBox9.Controls.Add(Me.gboldEFT)
        Me.GroupBox9.Controls.Add(Me.GroupBox11)
        Me.GroupBox9.Controls.Add(Me.GroupBox12)
        Me.GroupBox9.Controls.Add(Me.gbCurrEFT)
        Me.GroupBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox9.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(504, 288)
        Me.GroupBox9.TabIndex = 8
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "EFT File Splitting"
        '
        'gbGradEFT
        '
        Me.gbGradEFT.Controls.Add(Me.BrowseGradPLUSEFT)
        Me.gbGradEFT.Controls.Add(Me.lblEFTGradPLUS)
        Me.gbGradEFT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbGradEFT.Location = New System.Drawing.Point(40, 174)
        Me.gbGradEFT.Name = "gbGradEFT"
        Me.gbGradEFT.Size = New System.Drawing.Size(448, 48)
        Me.gbGradEFT.TabIndex = 4
        Me.gbGradEFT.TabStop = False
        Me.gbGradEFT.Text = "Browse To Directory for Deposit of GradPLUS EFT Files"
        '
        'BrowseGradPLUSEFT
        '
        Me.BrowseGradPLUSEFT.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BrowseGradPLUSEFT.Location = New System.Drawing.Point(360, 24)
        Me.BrowseGradPLUSEFT.Name = "BrowseGradPLUSEFT"
        Me.BrowseGradPLUSEFT.Size = New System.Drawing.Size(75, 16)
        Me.BrowseGradPLUSEFT.TabIndex = 1
        Me.BrowseGradPLUSEFT.Text = "Browse"
        '
        'lblEFTGradPLUS
        '
        Me.lblEFTGradPLUS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblEFTGradPLUS.Location = New System.Drawing.Point(16, 24)
        Me.lblEFTGradPLUS.Name = "lblEFTGradPLUS"
        Me.lblEFTGradPLUS.Size = New System.Drawing.Size(336, 16)
        Me.lblEFTGradPLUS.TabIndex = 0
        '
        'gboldEFT
        '
        Me.gboldEFT.Controls.Add(Me.Browse8)
        Me.gboldEFT.Controls.Add(Me.lbEFTOLD)
        Me.gboldEFT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gboldEFT.Location = New System.Drawing.Point(40, 74)
        Me.gboldEFT.Name = "gboldEFT"
        Me.gboldEFT.Size = New System.Drawing.Size(448, 48)
        Me.gboldEFT.TabIndex = 1
        Me.gboldEFT.TabStop = False
        Me.gboldEFT.Text = "Browse To Directory for Deposit of Old System EFT Files"
        '
        'Browse8
        '
        Me.Browse8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse8.Location = New System.Drawing.Point(360, 24)
        Me.Browse8.Name = "Browse8"
        Me.Browse8.Size = New System.Drawing.Size(75, 16)
        Me.Browse8.TabIndex = 1
        Me.Browse8.Text = "Browse"
        '
        'lbEFTOLD
        '
        Me.lbEFTOLD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbEFTOLD.Location = New System.Drawing.Point(16, 24)
        Me.lbEFTOLD.Name = "lbEFTOLD"
        Me.lbEFTOLD.Size = New System.Drawing.Size(336, 16)
        Me.lbEFTOLD.TabIndex = 0
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.Browse10)
        Me.GroupBox11.Controls.Add(Me.lbEFTArchive)
        Me.GroupBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox11.Location = New System.Drawing.Point(40, 224)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(448, 48)
        Me.GroupBox11.TabIndex = 3
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Browse To Directory for Archive EFT Files"
        '
        'Browse10
        '
        Me.Browse10.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse10.Location = New System.Drawing.Point(360, 24)
        Me.Browse10.Name = "Browse10"
        Me.Browse10.Size = New System.Drawing.Size(75, 16)
        Me.Browse10.TabIndex = 1
        Me.Browse10.Text = "Browse"
        '
        'lbEFTArchive
        '
        Me.lbEFTArchive.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbEFTArchive.Location = New System.Drawing.Point(16, 24)
        Me.lbEFTArchive.Name = "lbEFTArchive"
        Me.lbEFTArchive.Size = New System.Drawing.Size(336, 16)
        Me.lbEFTArchive.TabIndex = 0
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.Browse7)
        Me.GroupBox12.Controls.Add(Me.lbEFTSource)
        Me.GroupBox12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox12.Location = New System.Drawing.Point(40, 24)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(448, 48)
        Me.GroupBox12.TabIndex = 0
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Browse To Directory for EFT Files"
        '
        'Browse7
        '
        Me.Browse7.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse7.Location = New System.Drawing.Point(360, 24)
        Me.Browse7.Name = "Browse7"
        Me.Browse7.Size = New System.Drawing.Size(75, 16)
        Me.Browse7.TabIndex = 1
        Me.Browse7.Text = "Browse"
        '
        'lbEFTSource
        '
        Me.lbEFTSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbEFTSource.Location = New System.Drawing.Point(16, 24)
        Me.lbEFTSource.Name = "lbEFTSource"
        Me.lbEFTSource.Size = New System.Drawing.Size(336, 16)
        Me.lbEFTSource.TabIndex = 0
        '
        'gbCurrEFT
        '
        Me.gbCurrEFT.Controls.Add(Me.Browse9)
        Me.gbCurrEFT.Controls.Add(Me.lbEFTCurr)
        Me.gbCurrEFT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbCurrEFT.Location = New System.Drawing.Point(40, 124)
        Me.gbCurrEFT.Name = "gbCurrEFT"
        Me.gbCurrEFT.Size = New System.Drawing.Size(448, 48)
        Me.gbCurrEFT.TabIndex = 2
        Me.gbCurrEFT.TabStop = False
        Me.gbCurrEFT.Text = "Browse To Directory for Deposit of Current System EFT Files"
        '
        'Browse9
        '
        Me.Browse9.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse9.Location = New System.Drawing.Point(360, 24)
        Me.Browse9.Name = "Browse9"
        Me.Browse9.Size = New System.Drawing.Size(75, 16)
        Me.Browse9.TabIndex = 1
        Me.Browse9.Text = "Browse"
        '
        'lbEFTCurr
        '
        Me.lbEFTCurr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbEFTCurr.Location = New System.Drawing.Point(16, 24)
        Me.lbEFTCurr.Name = "lbEFTCurr"
        Me.lbEFTCurr.Size = New System.Drawing.Size(336, 16)
        Me.lbEFTCurr.TabIndex = 0
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(8, 312)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(528, 336)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox5)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(520, 310)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Response File Splitting"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox9)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(520, 310)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "EFT File Splitting"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.GroupBox6)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(520, 310)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "App Send File Correction"
        '
        'lblVersion
        '
        Me.lblVersion.Font = New System.Drawing.Font("Century Gothic", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.Location = New System.Drawing.Point(336, 8)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(72, 23)
        Me.lblVersion.TabIndex = 10
        Me.lblVersion.Text = "VERSION 5.0"
        Me.lblVersion.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbBYU)
        Me.GroupBox2.Controls.Add(Me.rbUofU)
        Me.GroupBox2.Controls.Add(Me.rbBannerSchool)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 88)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(528, 72)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Please Select a Processing Mode"
        '
        'rbBYU
        '
        Me.rbBYU.Location = New System.Drawing.Point(264, 16)
        Me.rbBYU.Name = "rbBYU"
        Me.rbBYU.Size = New System.Drawing.Size(184, 24)
        Me.rbBYU.TabIndex = 2
        Me.rbBYU.Text = "Brigham Young University"
        '
        'rbUofU
        '
        Me.rbUofU.Location = New System.Drawing.Point(8, 40)
        Me.rbUofU.Name = "rbUofU"
        Me.rbUofU.TabIndex = 1
        Me.rbUofU.Text = "University Of Utah"
        '
        'rbBannerSchool
        '
        Me.rbBannerSchool.Checked = True
        Me.rbBannerSchool.Location = New System.Drawing.Point(8, 16)
        Me.rbBannerSchool.Name = "rbBannerSchool"
        Me.rbBannerSchool.TabIndex = 0
        Me.rbBannerSchool.TabStop = True
        Me.rbBannerSchool.Text = "Banner School"
        '
        'gbTrig
        '
        Me.gbTrig.Controls.Add(Me.tbTriggerDate)
        Me.gbTrig.Location = New System.Drawing.Point(8, 248)
        Me.gbTrig.Name = "gbTrig"
        Me.gbTrig.Size = New System.Drawing.Size(528, 56)
        Me.gbTrig.TabIndex = 12
        Me.gbTrig.TabStop = False
        Me.gbTrig.Text = "Trigger Date"
        '
        'tbTriggerDate
        '
        Me.tbTriggerDate.Location = New System.Drawing.Point(16, 24)
        Me.tbTriggerDate.Name = "tbTriggerDate"
        Me.tbTriggerDate.Size = New System.Drawing.Size(128, 18)
        Me.tbTriggerDate.TabIndex = 0
        Me.tbTriggerDate.Text = ""
        '
        'MainForm
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 11)
        Me.ClientSize = New System.Drawing.Size(544, 699)
        Me.Controls.Add(Me.gbTrig)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.GroupBox14)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "UHEAA FIXIT"
        Me.GroupBox1.ResumeLayout(False)
        Me.gbOldRes.ResumeLayout(False)
        Me.gbCurrRes.ResumeLayout(False)
        Me.gbArchRes.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.gbGradRes.ResumeLayout(False)
        Me.GroupBox14.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.gbGradEFT.ResumeLayout(False)
        Me.gboldEFT.ResumeLayout(False)
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox12.ResumeLayout(False)
        Me.gbCurrEFT.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.gbTrig.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private CLSource As String
    Private CLOld As String
    Private Current As String
    Private GradPLUS As String
    Private Archive As String
    Private AppSendSource As String
    Private AppSendDeposit As String
    Private EFTSource As String
    Private EFTOLD As String
    Private EFTCurrent As String
    Private EFTGradPLUS As String
    Private EFTArchive As String
    Private ProcFrm As New FrmProcessing
    Private ProcessedAnything As Boolean = False
    Private TriggerDt As String
    Private ProcMode As String

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim NoFiles As New FrmNoFiles
        Dim ProcCom As New FrmProcComplete
        'Draw version
        Dim I As Image
        Dim Gr As Graphics
        Dim F As New Font(lblVersion.Font.FontFamily, 7, FontStyle.Bold, GraphicsUnit.Pixel)
        I = PictureBox1.Image
        Gr = Graphics.FromImage(I)
        Gr.DrawString(lblVersion.Text, F, Brushes.Black, 85, 5)
        PictureBox1.Image = I
        If Dir("C:\Program Files\UHEAA Express\FIXIT\Directory Config.txt") <> "" Then
            If FileLen("C:\Program Files\UHEAA Express\FIXIT\Directory Config.txt") = 0 Then
                Kill("C:\Program Files\UHEAA Express\FIXIT\Directory Config.txt")
            End If
        End If
        If Dir("C:\Program Files\UHEAA Express\FIXIT\Directory Config.txt") <> "" Then
            'if the directory config file is found then read in information and use it in processing
            GatherDirectoryInfo()
            CheckForFiles()
            CheckForEFTFiles()
            If AppSendSource <> "" Then 'if the dir isn't blank then proc dummy lender id functionality 
                CheckForFilesDummyID()
            End If
            If ProcessedAnything = False Then
                NoFiles.ShowDialog()
                End
            Else
                ProcCom.ShowDialog()
                End
            End If
        End If
        'if the directory config file isn't found then show form for user to create file
    End Sub

    'this function checks for files to process and processes them if found
    Sub CheckForEFTFiles()
        Dim TS1 As New TimeSpan(0, 0, 2)
        Dim FileInProcessing As String
        FileInProcessing = FindOldestProcessableFile(EFTSource, "E004", 61) 'find oldest processable file
        If FileInProcessing = "" Then
            Exit Sub 'skip all proc if nothing to proc
        End If
        ProcessedAnything = True 'set flag if there are files to proc
        'show processing form 
        ProcFrm.Show()
        ProcFrm.Refresh()
        System.Threading.Thread.CurrentThread.Sleep(TS1) 'sleep for two seconds
        ProcFrm.PB.Minimum = 0
        'process all files in directory
        While FileInProcessing <> ""
            ProcFrm.FileProcessing.Text = "Processing: " & EFTSource & "\" & FileInProcessing
            ProcFrm.PB.Value = 0
            ProcFrm.Refresh()
            ProcFrm.PB.Maximum = FileLen(EFTSource & "\" & FileInProcessing)
            ProcessEFTFile(FileInProcessing)
            FileInProcessing = FindOldestProcessableFile(EFTSource, "E004", 61) 'find oldest processable file
        End While
        ProcFrm.Hide()
    End Sub

    'this function processes the file
    Sub ProcessEFTFile(ByVal FN As String)
        Dim HeaderRow As String
        Dim TS As New TimeSpan(0, 0, 1)
        Dim RecordStr As String
        Dim CLDSRecs As Long = 0
        Dim EFTRecs As Long = 0
        Dim GradPLUSRecs As Long = 0
        Dim GTGradPLUSRecs As Long = 0
        Dim GTCLDSRecs As Long = 0
        Dim GTEFTRecs As Long = 0

        Dim CLDSNDA As Long = 0
        Dim EFTNDA As Long = 0
        Dim GPNDA As Long = 0
        Dim CLDSNCA As Long = 0
        Dim EFTNCA As Long = 0
        Dim GPNCA As Long = 0
        Dim CLDSNDA_E As Long = 0
        Dim EFTNDA_E As Long = 0
        Dim GPNDA_E As Long
        Dim CLDSNDA_IM As Long = 0
        Dim EFTNDA_IM As Long = 0
        Dim GPNDA_IM As Long = 0
        Dim CLDSGD As Long = 0
        Dim EFTGD As Long = 0
        Dim GPGD As Long = 0
        Dim CLDS2 As Long = 0
        Dim EFT2 As Long = 0
        Dim GP2 As Long = 0
        Dim CLDS3 As Long = 0
        Dim EFT3 As Long = 0
        Dim GP3 As Long = 0
        Dim CLDSNCA_A As Long = 0
        Dim EFTNCA_A As Long = 0
        Dim GPNCA_A As Long = 0

        Dim SortToCurrentSystemFile As Boolean
        Dim SortToOldEFTFile As Boolean
        Dim SortToGradPLUSFile As Boolean
        Dim TempCLDSTRec As String
        Dim TempEFTTRec As String
        Dim TempGPTRec As String
        Try
            FileOpen(1, EFTSource & "\" & FN, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
            FileOpen(2, EFTSource & "\" & "TEMPe.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            FileOpen(3, EFTSource & "\" & "TEMPc.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            FileOpen(4, EFTSource & "\" & "TEMPg.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            'process all records if source file
            While Not EOF(1)
                RecordStr = LineInput(1)
                ProcFrm.PB.Step = Len(RecordStr) 'set step length for processing
                ProcFrm.PB.PerformStep() 'perform step
                If RecordStr.StartsWith("@H") Then 'header
                    HeaderRow = RecordStr 'save header if file has rows
                ElseIf RecordStr.StartsWith("@1") Then 'first data row for group
                    If EFTOLD <> "" And EFTGradPLUS = "" Then  'Loan Period only
                        If (RecordStr.Substring(283, 2) & "/" & RecordStr.Substring(285, 2) & "/" & RecordStr.Substring(279, 4)) = "00/00/0000" Or TriggerDateCheck(RecordStr.Substring(283, 2) & "/" & RecordStr.Substring(285, 2) & "/" & RecordStr.Substring(279, 4)) Then
                            If CLDSRecs = 0 Then PrintLine(3, HeaderRow) 'only enter header row if data is written between T ans H rows
                            'copy to CLDS file if PLUS rec type
                            '**********************
                            SortToCurrentSystemFile = False 'set flag for subsequent rows
                            SortToOldEFTFile = True
                            SortToGradPLUSFile = False
                            '**********************
                            CLDSNDA = CLDSNDA + CLng(RecordStr.Substring(403, 7))
                            CLDSNCA = CLDSNCA + CLng(RecordStr.Substring(465, 7))
                            If RecordStr.Substring(410, 1) = "E" Then CLDSNDA_E = CLDSNDA_E + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(410, 1) = "I" Or RecordStr.Substring(410, 1) = "M" Then CLDSNDA_IM = CLDSNDA_IM + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(2, 1) = "R" Then CLDSGD = CLDSGD + CLng(RecordStr.Substring(382, 7))
                            If RecordStr.Substring(2, 1) = "A" Then CLDSNCA_A = CLDSNCA_A + CLng(RecordStr.Substring(465, 7))
                            PrintLine(3, RecordStr)
                            CLDSRecs = CLDSRecs + 1
                            GTCLDSRecs = GTCLDSRecs + 1
                        Else
                            'copy to Current Response file
                            If EFTRecs = 0 Then PrintLine(2, HeaderRow) 'only enter header row if data is written between T ans H rows
                            '**********************
                            SortToCurrentSystemFile = True 'set flag for subsequent rows
                            SortToOldEFTFile = False
                            SortToGradPLUSFile = False
                            '**********************
                            EFTNDA = EFTNDA + CLng(RecordStr.Substring(403, 7))
                            EFTNCA = EFTNCA + CLng(RecordStr.Substring(465, 7))
                            If RecordStr.Substring(410, 1) = "E" Then EFTNDA_E = EFTNDA_E + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(410, 1) = "I" Or RecordStr.Substring(410, 1) = "M" Then EFTNDA_IM = EFTNDA_IM + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(2, 1) = "R" Then EFTGD = EFTGD + CLng(RecordStr.Substring(382, 7))
                            If RecordStr.Substring(2, 1) = "A" Then EFTNCA_A = EFTNCA_A + CLng(RecordStr.Substring(465, 7))
                            PrintLine(2, RecordStr)
                            EFTRecs = EFTRecs + 1
                            GTEFTRecs = GTEFTRecs + 1
                        End If
                    ElseIf EFTOLD = "" And EFTGradPLUS <> "" Then 'GradPLUS Only
                        If RecordStr.Substring(295, 2) = "GB" Then
                            If GradPLUSRecs = 0 Then PrintLine(4, HeaderRow) 'only enter header row if data is written between T ans H rows
                            '**********************
                            SortToCurrentSystemFile = False  'set flag for subsequent rows
                            SortToOldEFTFile = False
                            SortToGradPLUSFile = True
                            '**********************
                            GPNDA = GPNDA + CLng(RecordStr.Substring(403, 7))
                            GPNCA = GPNCA + CLng(RecordStr.Substring(465, 7))
                            If RecordStr.Substring(410, 1) = "E" Then GPNDA_E = GPNDA_E + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(410, 1) = "I" Or RecordStr.Substring(410, 1) = "M" Then GPNDA_IM = GPNDA_IM + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(2, 1) = "R" Then GPGD = GPGD + CLng(RecordStr.Substring(382, 7))
                            If RecordStr.Substring(2, 1) = "A" Then GPNCA_A = GPNCA_A + CLng(RecordStr.Substring(465, 7))
                            PrintLine(4, RecordStr)
                            GradPLUSRecs = GradPLUSRecs + 1
                            GTGradPLUSRecs = GTGradPLUSRecs + 1
                        Else
                            'copy to Current Response file
                            If EFTRecs = 0 Then PrintLine(2, HeaderRow) 'only enter header row if data is written between T ans H rows
                            '**********************
                            SortToCurrentSystemFile = True 'set flag for subsequent rows
                            SortToOldEFTFile = False
                            SortToGradPLUSFile = False
                            '**********************
                            EFTNDA = EFTNDA + CLng(RecordStr.Substring(403, 7))
                            EFTNCA = EFTNCA + CLng(RecordStr.Substring(465, 7))
                            If RecordStr.Substring(410, 1) = "E" Then EFTNDA_E = EFTNDA_E + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(410, 1) = "I" Or RecordStr.Substring(410, 1) = "M" Then EFTNDA_IM = EFTNDA_IM + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(2, 1) = "R" Then EFTGD = EFTGD + CLng(RecordStr.Substring(382, 7))
                            If RecordStr.Substring(2, 1) = "A" Then EFTNCA_A = EFTNCA_A + CLng(RecordStr.Substring(465, 7))
                            PrintLine(2, RecordStr)
                            EFTRecs = EFTRecs + 1
                            GTEFTRecs = GTEFTRecs + 1
                        End If
                    ElseIf GradPLUS <> "" And CLOld <> "" Then 'Both GradPLUS and Loan Period
                        If RecordStr.Substring(295, 2) <> "GB" Then
                            If (RecordStr.Substring(283, 2) & "/" & RecordStr.Substring(285, 2) & "/" & RecordStr.Substring(279, 4)) = "00/00/0000" Or TriggerDateCheck(RecordStr.Substring(283, 2) & "/" & RecordStr.Substring(285, 2) & "/" & RecordStr.Substring(279, 4)) Then
                                If CLDSRecs = 0 Then PrintLine(3, HeaderRow) 'only enter header row if data is written between T ans H rows
                                'copy to CLDS file if PLUS rec type
                                '**********************
                                SortToCurrentSystemFile = False 'set flag for subsequent rows
                                SortToOldEFTFile = True
                                SortToGradPLUSFile = False
                                '**********************
                                CLDSNDA = CLDSNDA + CLng(RecordStr.Substring(403, 7))
                                CLDSNCA = CLDSNCA + CLng(RecordStr.Substring(465, 7))
                                If RecordStr.Substring(410, 1) = "E" Then CLDSNDA_E = CLDSNDA_E + CLng(RecordStr.Substring(403, 7))
                                If RecordStr.Substring(410, 1) = "I" Or RecordStr.Substring(410, 1) = "M" Then CLDSNDA_IM = CLDSNDA_IM + CLng(RecordStr.Substring(403, 7))
                                If RecordStr.Substring(2, 1) = "R" Then CLDSGD = CLDSGD + CLng(RecordStr.Substring(382, 7))
                                If RecordStr.Substring(2, 1) = "A" Then CLDSNCA_A = CLDSNCA_A + CLng(RecordStr.Substring(465, 7))
                                PrintLine(3, RecordStr)
                                CLDSRecs = CLDSRecs + 1
                                GTCLDSRecs = GTCLDSRecs + 1
                            Else
                                'copy to Current Response file
                                If EFTRecs = 0 Then PrintLine(2, HeaderRow) 'only enter header row if data is written between T ans H rows
                                '**********************
                                SortToCurrentSystemFile = True 'set flag for subsequent rows
                                SortToOldEFTFile = False
                                SortToGradPLUSFile = False
                                '**********************
                                EFTNDA = EFTNDA + CLng(RecordStr.Substring(403, 7))
                                EFTNCA = EFTNCA + CLng(RecordStr.Substring(465, 7))
                                If RecordStr.Substring(410, 1) = "E" Then EFTNDA_E = EFTNDA_E + CLng(RecordStr.Substring(403, 7))
                                If RecordStr.Substring(410, 1) = "I" Or RecordStr.Substring(410, 1) = "M" Then EFTNDA_IM = EFTNDA_IM + CLng(RecordStr.Substring(403, 7))
                                If RecordStr.Substring(2, 1) = "R" Then EFTGD = EFTGD + CLng(RecordStr.Substring(382, 7))
                                If RecordStr.Substring(2, 1) = "A" Then EFTNCA_A = EFTNCA_A + CLng(RecordStr.Substring(465, 7))
                                PrintLine(2, RecordStr)
                                EFTRecs = EFTRecs + 1
                                GTEFTRecs = GTEFTRecs + 1
                            End If
                        Else '=GB
                            If GradPLUSRecs = 0 Then PrintLine(4, HeaderRow) 'only enter header row if data is written between T ans H rows
                            '**********************
                            SortToCurrentSystemFile = False  'set flag for subsequent rows
                            SortToOldEFTFile = False
                            SortToGradPLUSFile = True
                            '**********************
                            GPNDA = GPNDA + CLng(RecordStr.Substring(403, 7))
                            GPNCA = GPNCA + CLng(RecordStr.Substring(465, 7))
                            If RecordStr.Substring(410, 1) = "E" Then GPNDA_E = GPNDA_E + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(410, 1) = "I" Or RecordStr.Substring(410, 1) = "M" Then GPNDA_IM = GPNDA_IM + CLng(RecordStr.Substring(403, 7))
                            If RecordStr.Substring(2, 1) = "R" Then GPGD = GPGD + CLng(RecordStr.Substring(382, 7))
                            If RecordStr.Substring(2, 1) = "A" Then GPNCA_A = GPNCA_A + CLng(RecordStr.Substring(465, 7))
                            PrintLine(4, RecordStr)
                            GradPLUSRecs = GradPLUSRecs + 1
                            GTGradPLUSRecs = GTGradPLUSRecs + 1
                        End If
                    End If

                ElseIf RecordStr.StartsWith("@T") Then 'trailer row
                    'Number of @1 recs
                    TempCLDSTRec = "@T" & Format(CLDSRecs, "00000#") & RecordStr.Substring(8)
                    TempEFTTRec = "@T" & Format(EFTRecs, "00000#") & RecordStr.Substring(8)
                    TempGPTRec = "@T" & Format(GradPLUSRecs, "00000#") & RecordStr.Substring(8)
                    'total of net disbursement amounts
                    If (CLDSNDA - CLDSNCA) > -1 Then
                        TempCLDSTRec = TempCLDSTRec.Substring(0, 8) & Format(Math.Abs((CLDSNDA - CLDSNCA)), "0000000000000#") & TempCLDSTRec.Substring(22)
                        TempCLDSTRec = TempCLDSTRec.Substring(0, 118) & "00000000000000" & TempCLDSTRec.Substring(132)
                    Else
                        TempCLDSTRec = TempCLDSTRec.Substring(0, 118) & Format(Math.Abs((CLDSNDA - CLDSNCA)), "0000000000000#") & TempCLDSTRec.Substring(132)
                        TempCLDSTRec = TempCLDSTRec.Substring(0, 8) & "00000000000000" & TempCLDSTRec.Substring(22)
                    End If
                    If (EFTNDA - EFTNCA) > -1 Then
                        TempEFTTRec = TempEFTTRec.Substring(0, 8) & Format(Math.Abs((EFTNDA - EFTNCA)), "0000000000000#") & TempEFTTRec.Substring(22)
                        TempEFTTRec = TempEFTTRec.Substring(0, 118) & "00000000000000" & TempEFTTRec.Substring(132)
                    Else
                        TempEFTTRec = TempEFTTRec.Substring(0, 118) & Format(Math.Abs((EFTNDA - EFTNCA)), "0000000000000#") & TempEFTTRec.Substring(132)
                        TempEFTTRec = TempEFTTRec.Substring(0, 8) & "00000000000000" & TempEFTTRec.Substring(22)
                    End If
                    If (GPNDA - GPNCA) > -1 Then
                        TempGPTRec = TempGPTRec.Substring(0, 8) & Format(Math.Abs((GPNDA - GPNCA)), "0000000000000#") & TempGPTRec.Substring(22)
                        TempGPTRec = TempGPTRec.Substring(0, 118) & "00000000000000" & TempGPTRec.Substring(132)
                    Else
                        TempGPTRec = TempGPTRec.Substring(0, 118) & Format(Math.Abs((GPNDA - GPNCA)), "0000000000000#") & TempGPTRec.Substring(132)
                        TempGPTRec = TempGPTRec.Substring(0, 8) & "00000000000000" & TempGPTRec.Substring(22)
                    End If


                    'misc
                    TempCLDSTRec = TempCLDSTRec.Substring(0, 22) & Format(CLDSNDA_E, "0000000000000#") & TempCLDSTRec.Substring(36)
                    TempEFTTRec = TempEFTTRec.Substring(0, 22) & Format(EFTNDA_E, "0000000000000#") & TempEFTTRec.Substring(36)
                    TempGPTRec = TempGPTRec.Substring(0, 22) & Format(GPNDA_E, "0000000000000#") & TempGPTRec.Substring(36)
                    'misc
                    TempCLDSTRec = TempCLDSTRec.Substring(0, 36) & Format(CLDSNDA_IM, "0000000000000#") & TempCLDSTRec.Substring(50)
                    TempEFTTRec = TempEFTTRec.Substring(0, 36) & Format(EFTNDA_IM, "0000000000000#") & TempEFTTRec.Substring(50)
                    TempGPTRec = TempGPTRec.Substring(0, 36) & Format(GPNDA_IM, "0000000000000#") & TempGPTRec.Substring(50)
                    'gross disb
                    TempCLDSTRec = TempCLDSTRec.Substring(0, 50) & Format(CLDSGD, "0000000000000#") & TempCLDSTRec.Substring(64)
                    TempEFTTRec = TempEFTTRec.Substring(0, 50) & Format(EFTGD, "0000000000000#") & TempEFTTRec.Substring(64)
                    TempGPTRec = TempGPTRec.Substring(0, 50) & Format(GPGD, "0000000000000#") & TempGPTRec.Substring(64)
                    '@2 rows
                    TempCLDSTRec = TempCLDSTRec.Substring(0, 64) & Format(CLDS2, "00000#") & TempCLDSTRec.Substring(70)
                    TempEFTTRec = TempEFTTRec.Substring(0, 64) & Format(EFT2, "00000#") & TempEFTTRec.Substring(70)
                    TempGPTRec = TempGPTRec.Substring(0, 64) & Format(GP2, "00000#") & TempGPTRec.Substring(70)
                    '@3 rows
                    TempCLDSTRec = TempCLDSTRec.Substring(0, 70) & Format(CLDS3, "00000#") & TempCLDSTRec.Substring(76)
                    TempEFTTRec = TempEFTTRec.Substring(0, 70) & Format(EFT3, "00000#") & TempEFTTRec.Substring(76)
                    TempGPTRec = TempGPTRec.Substring(0, 70) & Format(GP3, "00000#") & TempGPTRec.Substring(76)
                    'net cancellations
                    TempCLDSTRec = TempCLDSTRec.Substring(0, 104) & Format(CLDSNCA_A, "0000000000000#") & TempCLDSTRec.Substring(118)
                    TempEFTTRec = TempEFTTRec.Substring(0, 104) & Format(EFTNCA_A, "0000000000000#") & TempEFTTRec.Substring(118)
                    TempGPTRec = TempGPTRec.Substring(0, 104) & Format(GPNCA_A, "0000000000000#") & TempGPTRec.Substring(118)
                    'write new @T rows to files
                    If CLDSRecs <> 0 Then PrintLine(3, TempCLDSTRec)
                    If EFTRecs <> 0 Then PrintLine(2, TempEFTTRec)
                    If GradPLUSRecs <> 0 Then PrintLine(4, TempGPTRec)
                    EFTRecs = 0
                    CLDSRecs = 0
                    GradPLUSRecs = 0
                    CLDSNCA = 0
                    CLDSNDA = 0
                    GPNCA = 0
                    GPNDA = 0
                    EFTNCA = 0
                    EFTNDA = 0
                    EFTNDA_E = 0
                    CLDSNDA_E = 0
                    GPNDA_E = 0
                    EFTNDA_IM = 0
                    CLDSNDA_IM = 0
                    GPNDA_IM = 0
                    EFTGD = 0
                    CLDSGD = 0
                    GPGD = 0
                    EFT2 = 0
                    EFT3 = 0
                    GP2 = 0
                    GP3 = 0
                    CLDS2 = 0
                    CLDS3 = 0
                    CLDSNCA_A = 0
                    EFTNCA_A = 0
                    GPNCA_A = 0
                Else 'subsequent rows for @1 row
                    'check if flag is set to sort to Banner ro CLDS
                    If SortToCurrentSystemFile Then
                        'Current Response file
                        If RecordStr.StartsWith("@2") Then
                            EFT2 = EFT2 + 1
                        ElseIf RecordStr.StartsWith("@3") Then
                            EFT3 = EFT3 + 1
                        End If
                        PrintLine(2, RecordStr)
                    ElseIf SortToOldEFTFile Then
                        'CLDS
                        If RecordStr.StartsWith("@2") Then
                            CLDS2 = CLDS2 + 1
                        ElseIf RecordStr.StartsWith("@3") Then
                            CLDS3 = CLDS3 + 1
                        End If
                        PrintLine(3, RecordStr)
                    ElseIf SortToGradPLUSFile Then
                        If RecordStr.StartsWith("@2") Then
                            GP2 = GP2 + 1
                        ElseIf RecordStr.StartsWith("@3") Then
                            GP3 = GP3 + 1
                        End If
                        PrintLine(4, RecordStr)
                    End If
                End If


            End While
            FileClose(1)
            FileClose(3)
            FileClose(2)
            FileClose(4)
            'copy files to specified destinations if they aren't empty
            If GTCLDSRecs = 0 Then
                Kill(EFTSource & "\" & "TEMPc.txt")
            Else
                FileCopy(EFTSource & "\" & "TEMPc.txt", EFTOLD & "\" & FN)
                Kill(EFTSource & "\" & "TEMPc.txt")
            End If
            If GTEFTRecs = 0 Then
                Kill(EFTSource & "\" & "TEMPe.txt")
            Else
                FileCopy(EFTSource & "\" & "TEMPe.txt", EFTCurrent & "\eft" & Format(Now, "MMddyyyyhhmmsstt") & ".dat")
                Kill(EFTSource & "\" & "TEMPe.txt")
            End If
            If GTGradPLUSRecs = 0 Then
                Kill(EFTSource & "\" & "TEMPg.txt")
            Else
                FileCopy(EFTSource & "\" & "TEMPg.txt", EFTGradPLUS & "\GradDisb" & Format(Now, "MMddyyyyhhmmsstt") & ".dat")
                Kill(EFTSource & "\" & "TEMPg.txt")
            End If
            'copy source file to archive and delete
            FileCopy(EFTSource & "\" & FN, EFTArchive & "\" & FN & Format(Now, "MMddyyyyhhmmsstt"))
            Kill(EFTSource & "\" & FN)
            System.Threading.Thread.CurrentThread.Sleep(TS) 'pause for a second to not run into files writing over each other
        Catch ex As Exception
            MsgBox("There was a problem while trying to access or process one of the needed files." & vbLf & vbLf & "Please contact UHEAA Technology Development at (801) 321-7201, or e-mail us at onelinksupport@utahsbr.edu", MsgBoxStyle.Critical, "Error While Accessing or Processing Files")
            End
        End Try
    End Sub

    'this function checks for Dummy Lender ID files and processes them
    Sub CheckForFilesDummyID()
        Dim TS1 As New TimeSpan(0, 0, 2)
        Dim FileInProcessing As String
        FileInProcessing = FindOldestProcessableFile(AppSendSource, "A004", 69) 'find oldest processable file
        If FileInProcessing = "" Then
            Exit Sub 'skip all proc if nothing to proc
        End If
        ProcessedAnything = True 'set flag if there are files to proc
        'show processing form 
        ProcFrm.Show()
        ProcFrm.Refresh()
        System.Threading.Thread.CurrentThread.Sleep(TS1) 'sleep for two seconds
        ProcFrm.PB.Minimum = 0
        'process all files in directory
        While FileInProcessing <> ""
            ProcFrm.FileProcessing.Text = "Processing: " & AppSendSource & "\" & FileInProcessing
            ProcFrm.PB.Value = 0
            ProcFrm.Refresh()
            ProcFrm.PB.Maximum = FileLen(AppSendSource & "\" & FileInProcessing)
            ProcessFileDummyID(FileInProcessing)
            FileInProcessing = FindOldestProcessableFile(AppSendSource, "A004", 69) 'find oldest processable file
        End While
        ProcFrm.Hide()
    End Sub

    'this function processes the Dummy ID files
    Sub ProcessFileDummyID(ByVal FN As String)
        Dim RecordStr As String
        Dim TS As New TimeSpan(0, 0, 1)
        Try
            FileOpen(1, AppSendSource & "\" & FN, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
            FileOpen(2, AppSendSource & "\" & "TEMPe.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            'process all records if source file
            While Not EOF(1)
                RecordStr = LineInput(1)
                ProcFrm.PB.Step = Len(RecordStr) 'set step length for processing
                ProcFrm.PB.PerformStep() 'perform step
                If RecordStr.StartsWith("@1") Then 'if @1 rec then proc
                    If RecordStr.Substring(171, 6) = "999999" _
                    Or RecordStr.Substring(171, 6) = "817455" _
                    Or RecordStr.Substring(171, 6) = "813894" _
                    Or RecordStr.Substring(171, 6) = "819628" Then
                        RecordStr = RecordStr.Substring(0, 171) & "      " & RecordStr.Substring(177)
                    End If
                    If RecordStr.Substring(487, 6) = "999999" _
                    Or RecordStr.Substring(487, 6) = "817455" _
                    Or RecordStr.Substring(487, 6) = "813894" _
                    Or RecordStr.Substring(487, 6) = "819628" Then
                        RecordStr = RecordStr.Substring(0, 487) & "      " & RecordStr.Substring(493)
                    End If
                    PrintLine(2, RecordStr)
                Else 'if different then rec 1 then just copy
                    PrintLine(2, RecordStr)
                End If
            End While
            FileClose(1)
            FileClose(2)
            'copy file to specified dir and delete applicable files
            FileCopy(AppSendSource & "\" & "TEMPe.txt", AppSendDeposit & "\elap" & Format(Now, "MMddyyyyhhmmsstt") & "op.dat")
            System.Threading.Thread.CurrentThread.Sleep(TS) 'wait for one second to ensure that didferent time date stamps are used for each file
            Kill(AppSendSource & "\" & "TEMPe.txt")
            Kill(AppSendSource & "\" & FN)
        Catch ex As Exception
            MsgBox("There was a problem while trying to access or process one of the needed files." & vbLf & vbLf & "Please contact UHEAA Technology Development at (801) 321-7201, or e-mail us at onelinksupport@utahsbr.edu", MsgBoxStyle.Critical, "Error While Accessing or Processing Files")
            End
        End Try
    End Sub

    'this function checks for files to process and processes them if found
    Sub CheckForFiles()
        Dim TS1 As New TimeSpan(0, 0, 2)
        Dim FileInProcessing As String
        FileInProcessing = FindOldestProcessableFile(CLSource, "R004", 69) 'find oldest processable file
        If FileInProcessing = "" Then
            Exit Sub 'skip all proc if nothing to proc
        End If
        ProcessedAnything = True 'set flag if there are files to proc
        'show processing form 
        ProcFrm.Show()
        ProcFrm.Refresh()
        System.Threading.Thread.CurrentThread.Sleep(TS1) 'sleep for two seconds
        ProcFrm.PB.Minimum = 0
        'process all files in directory
        While FileInProcessing <> ""
            ProcFrm.FileProcessing.Text = "Processing: " & CLSource & "\" & FileInProcessing
            ProcFrm.PB.Value = 0
            ProcFrm.Refresh()
            ProcFrm.PB.Maximum = FileLen(CLSource & "\" & FileInProcessing)
            ProcessFile(FileInProcessing)
            FileInProcessing = FindOldestProcessableFile(CLSource, "R004", 69) 'find oldest processable file
        End While
        ProcFrm.Hide()
    End Sub

    Function Date1LEDate2(ByVal DtStr As String, ByVal Dt As Date) As Boolean
        If IsDate(DtStr) Then
            If CDate(DtStr) <= Dt Then Return True
        End If
        Return False
    End Function

    'this function processes the file
    Sub ProcessFile(ByVal FN As String)
        Dim RecordStr As String
        Dim HeaderRow As String
        Dim CLXRRecs As Long
        Dim elupdtopRecs As Long
        Dim GradPLUStopRecs As Long
        Dim GTGradPLUStopRecs As Long 'grand total for GradPLUS
        Dim GTCLXRRecs As Long 'grand total for CLXR
        Dim GTelupdtopRecs As Long 'grand total for banner 
        Dim SortToCurrentRespFile As Boolean
        Dim SortToGradPLUSFile As Boolean
        Dim SortToCLXRFile As Boolean
        Dim TS As New TimeSpan(0, 0, 1)
        Try
            FileOpen(1, CLSource & "\" & FN, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
            FileOpen(2, CLSource & "\" & "TEMPe.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            FileOpen(3, CLSource & "\" & "TEMPc.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            FileOpen(4, CLSource & "\" & "TEMPg.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            'process all records if source file
            While Not EOF(1)
                RecordStr = LineInput(1)
                ProcFrm.PB.Step = Len(RecordStr) 'set step length for processing
                ProcFrm.PB.PerformStep() 'perform step
                If RecordStr.StartsWith("@H") Then 'header
                    HeaderRow = RecordStr 'save header row for when a row is written out to the file
                ElseIf RecordStr.StartsWith("@1") Then 'first data row for group
                    If CLOld <> "" And GradPLUS = "" Then   'Loan Period only
                        If ((RecordStr.Substring(360, 2) & "/" & RecordStr.Substring(362, 2) & "/" & RecordStr.Substring(356, 4)) = "00/00/0000") Or TriggerDateCheck(RecordStr.Substring(360, 2) & "/" & RecordStr.Substring(362, 2) & "/" & RecordStr.Substring(356, 4)) Then  'handle PLUS
                            'copy to CLXR file if PLUS rec type
                            '********************************set flag for subsequent rows
                            SortToCLXRFile = True
                            SortToCurrentRespFile = False
                            SortToGradPLUSFile = False
                            '********************************
                            If RecordStr.Substring(760, 1) = "F" Then
                                RecordStr = RecordStr.Substring(0, 760) & "H" & RecordStr.Substring(761)
                            End If
                            If RecordStr.Substring(761, 1) = "F" Then
                                RecordStr = RecordStr.Substring(0, 761) & "H" & RecordStr.Substring(762)
                            End If
                            If RecordStr.Substring(762, 1) = "F" Then
                                RecordStr = RecordStr.Substring(0, 762) & "H" & RecordStr.Substring(763)
                            End If
                            If RecordStr.Substring(763, 1) = "F" Then
                                RecordStr = RecordStr.Substring(0, 763) & "H" & RecordStr.Substring(764)
                            End If
                        Else
                            'copy to CURRENT RESPONSE file
                            '********************************set flag for subsequent rows
                            SortToCurrentRespFile = True
                            SortToCLXRFile = False
                            SortToGradPLUSFile = False
                            '************************************
                            If RecordStr.Substring(760, 1) = "F" Then
                                RecordStr = RecordStr.Substring(0, 760) & "H" & RecordStr.Substring(761)
                            End If
                            If RecordStr.Substring(761, 1) = "F" Then
                                RecordStr = RecordStr.Substring(0, 761) & "H" & RecordStr.Substring(762)
                            End If
                            If RecordStr.Substring(762, 1) = "F" Then
                                RecordStr = RecordStr.Substring(0, 762) & "H" & RecordStr.Substring(763)
                            End If
                            If RecordStr.Substring(763, 1) = "F" Then
                                RecordStr = RecordStr.Substring(0, 763) & "H" & RecordStr.Substring(764)
                            End If
                        End If
                    ElseIf GradPLUS <> "" And CLOld = "" Then 'GradPLUS only
                        If RecordStr.Substring(187, 2) = "GB" Then
                            'copy to GradPLUS file
                            '********************************set flag for subsequent rows
                            SortToGradPLUSFile = True
                            SortToCurrentRespFile = False
                            SortToCLXRFile = False
                            '*********************************
                        Else
                            '********************************set flag for subsequent rows
                            SortToGradPLUSFile = False
                            SortToCurrentRespFile = True
                            SortToCLXRFile = False
                            '*********************************
                        End If
                    ElseIf GradPLUS <> "" And CLOld <> "" Then 'Both GradPLUS and Loan Period
                        If RecordStr.Substring(187, 2) = "GB" Then
                            'copy to GradPLUS file
                            '********************************set flag for subsequent rows
                            SortToGradPLUSFile = True
                            SortToCurrentRespFile = False
                            SortToCLXRFile = False
                            '*********************************
                        Else
                            If ((RecordStr.Substring(360, 2) & "/" & RecordStr.Substring(362, 2) & "/" & RecordStr.Substring(356, 4)) = "00/00/0000") Or TriggerDateCheck(RecordStr.Substring(360, 2) & "/" & RecordStr.Substring(362, 2) & "/" & RecordStr.Substring(356, 4)) Then  'handle PLUS
                                'copy to CLXR file if PLUS rec type
                                '********************************set flag for subsequent rows
                                SortToCLXRFile = True
                                SortToCurrentRespFile = False
                                SortToGradPLUSFile = False
                                '********************************
                                If RecordStr.Substring(760, 1) = "F" Then
                                    RecordStr = RecordStr.Substring(0, 760) & "H" & RecordStr.Substring(761)
                                End If
                                If RecordStr.Substring(761, 1) = "F" Then
                                    RecordStr = RecordStr.Substring(0, 761) & "H" & RecordStr.Substring(762)
                                End If
                                If RecordStr.Substring(762, 1) = "F" Then
                                    RecordStr = RecordStr.Substring(0, 762) & "H" & RecordStr.Substring(763)
                                End If
                                If RecordStr.Substring(763, 1) = "F" Then
                                    RecordStr = RecordStr.Substring(0, 763) & "H" & RecordStr.Substring(764)
                                End If
                            Else
                                'copy to CURRENT RESPONSE file
                                '********************************set flag for subsequent rows
                                SortToCurrentRespFile = True
                                SortToCLXRFile = False
                                SortToGradPLUSFile = False
                                '************************************
                                If RecordStr.Substring(760, 1) = "F" Then
                                    RecordStr = RecordStr.Substring(0, 760) & "H" & RecordStr.Substring(761)
                                End If
                                If RecordStr.Substring(761, 1) = "F" Then
                                    RecordStr = RecordStr.Substring(0, 761) & "H" & RecordStr.Substring(762)
                                End If
                                If RecordStr.Substring(762, 1) = "F" Then
                                    RecordStr = RecordStr.Substring(0, 762) & "H" & RecordStr.Substring(763)
                                End If
                                If RecordStr.Substring(763, 1) = "F" Then
                                    RecordStr = RecordStr.Substring(0, 763) & "H" & RecordStr.Substring(764)
                                End If
                            End If
                        End If
                    End If
                    'decide where to print
                    If SortToCLXRFile Then
                        If CLXRRecs = 0 Then PrintLine(3, HeaderRow) 'write header row out if a @1 recs is found for file
                        PrintLine(3, RecordStr)
                        CLXRRecs = CLXRRecs + 1
                        GTCLXRRecs = GTCLXRRecs + 1
                    ElseIf SortToCurrentRespFile Then
                        If elupdtopRecs = 0 Then PrintLine(2, HeaderRow) 'write header row out if a @1 recs is found for file
                        PrintLine(2, RecordStr)
                        elupdtopRecs = elupdtopRecs + 1
                        GTelupdtopRecs = GTelupdtopRecs + 1
                    ElseIf SortToGradPLUSFile Then
                        If GradPLUStopRecs = 0 Then PrintLine(4, HeaderRow) 'write header row out if a @1 recs is found for file
                        PrintLine(4, RecordStr)
                        GradPLUStopRecs = GradPLUStopRecs + 1
                        GTGradPLUStopRecs = GTGradPLUStopRecs + 1
                    End If

                ElseIf RecordStr.StartsWith("@T") Then 'trailer row
                    If CLXRRecs <> 0 Then PrintLine(3, "@T" & Format(CLXRRecs, "00000#") & RecordStr.Substring(8))
                    If elupdtopRecs <> 0 Then PrintLine(2, "@T" & Format(elupdtopRecs, "00000#") & RecordStr.Substring(8))
                    If GradPLUStopRecs <> 0 Then PrintLine(4, "@T" & Format(GradPLUStopRecs, "00000#") & RecordStr.Substring(8))
                    CLXRRecs = 0
                    elupdtopRecs = 0
                    GradPLUStopRecs = 0
                Else 'subsequent rows for @1 row
                    'check if flag is set to sort to Current Response or CLXR or GradPLUS
                    If SortToCLXRFile Then
                        PrintLine(3, RecordStr)
                    ElseIf SortToCurrentRespFile Then
                        PrintLine(2, RecordStr)
                    ElseIf SortToGradPLUSFile Then
                        PrintLine(4, RecordStr)
                    End If
                End If
            End While
            FileClose(1)
            FileClose(3)
            FileClose(2)
            FileClose(4)
            'copy files to specified destinations if they aren't empty
            If GTCLXRRecs = 0 Then
                Kill(CLSource & "\" & "TEMPc.txt")
            Else
                FileCopy(CLSource & "\" & "TEMPc.txt", CLOld & "\" & FN)
                Kill(CLSource & "\" & "TEMPc.txt")
            End If
            If GTelupdtopRecs = 0 Then
                Kill(CLSource & "\" & "TEMPe.txt")
            Else
                FileCopy(CLSource & "\" & "TEMPe.txt", Current & "\elupdtop" & Format(Now, "MMddyyyyhhmmsstt") & ".dat")
                Kill(CLSource & "\" & "TEMPe.txt")
            End If
            If GTGradPLUStopRecs = 0 Then
                Kill(CLSource & "\" & "TEMPg.txt")
            Else
                FileCopy(CLSource & "\" & "TEMPg.txt", GradPLUS & "\GradResp" & Format(Now, "MMddyyyyhhmmsstt") & ".dat")
                Kill(CLSource & "\" & "TEMPg.txt")
            End If
            'copy source file to archive and delete
            FileCopy(CLSource & "\" & FN, Archive & "\" & FN & Format(Now, "MMddyyyyhhmmsstt"))
            Kill(CLSource & "\" & FN)
            System.Threading.Thread.CurrentThread.Sleep(TS) 'pause for a second to not run into files writing over each other
        Catch ex As Exception
            MsgBox("There was a problem while trying to access or process one of the needed files." & vbLf & vbLf & "Please contact UHEAA Technology Development at (801) 321-7201, or e-mail us at onelinksupport@utahsbr.edu", MsgBoxStyle.Critical, "Error While Accessing or Processing Files")
            End
        End Try
    End Sub

    Function TriggerDateCheck(ByVal DtStr As String) As Boolean
        If TriggerDt <> "" Then
            Return Date1LEDate2(DtStr, CDate(TriggerDt).AddDays(-1))
        Else
            Return Date1LEDate2(DtStr, CDate("07/31/2005"))
        End If
    End Function

    Function FindOldestProcessableFile(ByVal P As String, ByVal CheckText As String, ByVal CTPlace As Integer) As String
        Dim TempFN As String
        Dim TempFTD As Date
        Dim OFN As String
        Dim OFTD As Date
        OFN = Dir(P & "\*")
        'if nothing in directory then return nothing
        If OFN = "" Then
            Return OFN
        End If
        'find a processable file
        While HeaderRowCheck(CheckText, OFN, P, CTPlace) = False
            OFN = Dir()
            'if nothing in directory is processable then return nothing
            If OFN = "" Then
                Return OFN
            End If
        End While
        'if no file was found then exit function with blank string
        If OFN = "" Then
            Return OFN
        End If
        'process if a file is found
        OFTD = FileDateTime(P & "\" & OFN)
        TempFN = Dir(P & "\*")
        TempFTD = FileDateTime(P & "\" & TempFN)
        'search through files
        While TempFN <> ""
            If TempFTD < OFTD And HeaderRowCheck(CheckText, TempFN, P, CTPlace) Then
                OFN = TempFN
                OFTD = TempFTD
            End If
            TempFN = Dir() 'go to next file in directory
            If TempFN <> "" Then TempFTD = FileDateTime(P & "\" & TempFN)
        End While
        Return OFN
    End Function

    'this function gathers the applicable Loan Information
    Sub GatherDirectoryInfo()
        Dim RecordType As String
        FileOpen(1, "C:\Program Files\UHEAA Express\FIXIT\Directory Config.txt", OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
        Input(1, RecordType)
        Input(1, CLSource)
        Input(1, RecordType)
        Input(1, CLOld)
        Input(1, RecordType)
        Input(1, Current)
        Input(1, RecordType)
        Input(1, GradPLUS)
        Input(1, RecordType)
        Input(1, Archive)
        Input(1, RecordType)
        Input(1, AppSendSource)
        Input(1, RecordType)
        Input(1, AppSendDeposit)
        Input(1, RecordType)
        Input(1, EFTSource)
        Input(1, RecordType)
        Input(1, EFTOLD)
        Input(1, RecordType)
        Input(1, EFTCurrent)
        Input(1, RecordType)
        Input(1, EFTGradPLUS)
        Input(1, RecordType)
        Input(1, EFTArchive)
        Input(1, RecordType)
        Input(1, ProcMode)
        Input(1, RecordType)
        Input(1, TriggerDt)
        FileClose(1)
    End Sub

    Private Sub Browse1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse1.Click
        FBD.Reset()
        FBD.ShowDialog()
        lblCLSource.Text = FBD.SelectedPath
    End Sub

    Private Sub Browse2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse2.Click
        FBD.Reset()
        FBD.ShowDialog()
        lblOldCL.Text = FBD.SelectedPath
    End Sub

    Private Sub Browse3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse3.Click
        FBD.Reset()
        FBD.ShowDialog()
        lblCurrent.Text = FBD.SelectedPath
    End Sub

    Private Sub BrowseGradPLUS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseGradPLUS.Click
        FBD.Reset()
        FBD.ShowDialog()
        lblGradPLUS.Text = FBD.SelectedPath
    End Sub

    Private Sub Browse4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse4.Click
        FBD.Reset()
        FBD.ShowDialog()
        lblArchive.Text = FBD.SelectedPath
    End Sub

    Function ValidForm() As Boolean
        If lblCLSource.Text = "" Or lblArchive.Text = "" Or _
        lbEFTSource.Text = "" Or lbEFTArchive.Text = "" Then
            MsgBox("Some required pathways have not been selected.", MsgBoxStyle.Critical, "Provide Directories")
            Return False
        End If
        If radLP.Checked And (lblOldCL.Text = "" Or lbEFTOLD.Text = "" Or lblCurrent.Text = "" Or lbEFTCurr.Text = "") Then
            MsgBox("Some required pathways have not been selected.", MsgBoxStyle.Critical, "Provide Directories")
            Return False
        End If
        If radPLUS.Checked And (lblGradPLUS.Text = "" Or lblEFTGradPLUS.Text = "" Or lbEFTCurr.Text = "" Or lblCurrent.Text = "") Then
            MsgBox("Some required pathways have not been selected.", MsgBoxStyle.Critical, "Provide Directories")
            Return False
        End If
        If radBoth.Checked And (lblOldCL.Text = "" Or lbEFTOLD.Text = "" Or lblGradPLUS.Text = "" Or lblEFTGradPLUS.Text = "" Or lblCurrent.Text = "" Or lbEFTCurr.Text = "") Then
            MsgBox("Some required pathways have not been selected.", MsgBoxStyle.Critical, "Provide Directories")
            Return False
        End If
        'be sure that source and destination directories don't match
        If lblCLSource.Text = lblOldCL.Text Or lblCLSource.Text = lblCurrent.Text Or lblCLSource.Text = lblArchive.Text Or lblGradPLUS.Text = lblCLSource.Text Then
            MsgBox("One of the destinations for the response file splitting is the same as the source.", MsgBoxStyle.Critical, "Source and Destination Conflict")
            Return False
        End If
        If lbEFTSource.Text = lbEFTOLD.Text Or lbEFTSource.Text = lbEFTCurr.Text Or lbEFTSource.Text = lbEFTArchive.Text Or lblEFTGradPLUS.Text = lbEFTSource.Text Then
            MsgBox("One of the destinations for the EFT file splitting is the same as the source.", MsgBoxStyle.Critical, "Source and Destination Conflict")
            Return False
        End If
        If cbDummyFunc.Checked Then
            If lblSourceAppSend.Text = "" Or lblDepositAppSend.Text = "" Then
                MsgBox("Some required pathways have not been selected.", MsgBoxStyle.Critical, "Provide Directories")
                Return False
            Else
                If lblSourceAppSend.Text = lblDepositAppSend.Text Then
                    MsgBox("The destination of App send file is the same as the source.", MsgBoxStyle.Critical, "Source and Destination Conflict")
                    Return False
                End If
            End If
        End If
        'make sure that trigger date is provided if any of appropriate options are selected
        If radLP.Checked Or radBoth.Checked Or rbBannerSchool.Checked Then
            If tbTriggerDate.TextLength = 0 Then
                MsgBox("Based off the options you selected above you must provide a Trigger date and have not.  Please provide a Trigger Date.", MsgBoxStyle.Critical, "Trigger Date Required")
                Return False
            End If
        End If
        'check if trigger date is a valid date if provided
        If tbTriggerDate.TextLength <> 0 Then
            If IsDate(tbTriggerDate.Text) = False Then
                MsgBox("The Trigger Date you provided is not valid.  Please provide a valid Trigger Date.", MsgBoxStyle.Critical, "Valid Trigger Date Required")
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If ValidForm() Then
            If cbDummyFunc.Checked = False Then
                If MsgBoxResult.No = MsgBox("You have left the App Send File Correction option unselected.  Are you sure that you don't want to use that functionality?", MsgBoxStyle.YesNo, "Are You Sure?") Then
                    Exit Sub
                End If
            End If
            'if all values are populated as they should be then create file
            FileOpen(1, "C:\Program Files\UHEAA Express\FIXIT\Directory Config.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            WriteLine(1, "Source Of Commonline Response Files", lblCLSource.Text)
            WriteLine(1, "Deposit Of Old Commonline Files", lblOldCL.Text)
            WriteLine(1, "Deposit Of Current Response Files", lblCurrent.Text)
            WriteLine(1, "Deposit Of GradPLUS Files", lblGradPLUS.Text)
            WriteLine(1, "Deposit Of Archive Files", lblArchive.Text)
            WriteLine(1, "Source Of App Send", lblSourceAppSend.Text)
            WriteLine(1, "Deposit Of App Send", lblDepositAppSend.Text)
            WriteLine(1, "Source Of EFT Files", lbEFTSource.Text)
            WriteLine(1, "Deposit Of Old EFT Files", lbEFTOLD.Text)
            WriteLine(1, "Deposit Of Current EFT Files", lbEFTCurr.Text)
            WriteLine(1, "Deposit Of GradPLUS EFT Files", lblEFTGradPLUS.Text)
            WriteLine(1, "Deposit Of Archive EFT Files", lbEFTArchive.Text)
            'write out processing mode
            If rbBannerSchool.Checked Then
                WriteLine(1, "Processing Mode", "Banner")
            ElseIf rbUofU.Checked Then
                WriteLine(1, "Processing Mode", "UofU")
            ElseIf rbBYU.Checked Then
                WriteLine(1, "Processing Mode", "BYU")
            End If
            'write out trigger date
            WriteLine(1, "Trigger Date", tbTriggerDate.Text)
            FileClose(1)
            MsgBox("Setup Complete.", MsgBoxStyle.Information, "Setup Complete")
            End
        End If
    End Sub

    Private Sub Browse5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse5.Click
        FBD.Reset()
        FBD.ShowDialog()
        lblSourceAppSend.Text = FBD.SelectedPath
    End Sub

    Private Sub Browse6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse6.Click
        FBD.Reset()
        FBD.ShowDialog()
        lblDepositAppSend.Text = FBD.SelectedPath
    End Sub

    Private Sub cbDummyFunc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDummyFunc.CheckedChanged
        If cbDummyFunc.Checked Then
            Browse6.Enabled = True
            Browse5.Enabled = True
        Else
            Browse6.Enabled = False
            Browse5.Enabled = False
            lblDepositAppSend.Text = ""
            lblSourceAppSend.Text = ""
        End If
    End Sub

    Private Sub Browse7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse7.Click
        FBD.Reset()
        FBD.ShowDialog()
        lbEFTSource.Text = FBD.SelectedPath
    End Sub

    Private Sub Browse8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse8.Click
        FBD.Reset()
        FBD.ShowDialog()
        lbEFTOLD.Text = FBD.SelectedPath
    End Sub

    Private Sub Browse9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse9.Click
        FBD.Reset()
        FBD.ShowDialog()
        lbEFTCurr.Text = FBD.SelectedPath
    End Sub

    Private Sub BrowseGradPLUSEFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseGradPLUSEFT.Click
        FBD.Reset()
        FBD.ShowDialog()
        lblEFTGradPLUS.Text = FBD.SelectedPath
    End Sub

    Private Sub Browse10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse10.Click
        FBD.Reset()
        FBD.ShowDialog()
        lbEFTArchive.Text = FBD.SelectedPath
    End Sub

    'this function checks the header row of the file to be sure it's the right file
    Function HeaderRowCheck(ByVal Checktext As String, ByVal FN As String, ByVal PathNM As String, ByVal Place As Integer) As Boolean
        Dim HeaderRow As String
        Try
            FileOpen(5, PathNM & "\" & FN, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
            HeaderRow = LineInput(5)
            FileClose(5)
        Catch ex As Exception
            MsgBox("There was a problem while trying to access or process one of the needed files." & vbLf & vbLf & "Please contact UHEAA Technology Development at (801) 321-7201, or e-mail us at onelinksupport@utahsbr.edu", MsgBoxStyle.Critical, "Error While Accessing or Processing Files")
            End
        End Try

        If Len(HeaderRow) >= (Place + Len(Checktext)) Then
            'check if the text is in the header row
            If HeaderRow.Substring(Place, Len(Checktext)) <> Checktext Then
                Return False
            Else
                'check header row for UHEAA encoding
                If Checktext = "R004" Then
                    If HeaderRow.Substring(153, 6) = "700126" Or _
                        HeaderRow.Substring(153, 3) = "749" Or _
                        HeaderRow.Substring(106, 6) = "700126" Or _
                        HeaderRow.Substring(106, 3) = "749" Then
                        Return True
                    Else
                        Return False
                    End If
                ElseIf Checktext = "A004" Then
                    If HeaderRow.Substring(153, 6) = "700126" Or _
                        HeaderRow.Substring(153, 3) = "749" Then
                        Return True
                    Else
                        Return False
                    End If
                ElseIf Checktext = "E004" Then
                    If HeaderRow.Substring(98, 6) = "700126" Or _
                        HeaderRow.Substring(98, 3) = "749" Then
                        Return True
                    Else
                        Return False
                    End If
                End If
                Return True
            End If
        Else
            Return False
        End If
    End Function
#Region "Radio Buttons"
    Private Sub radLP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radLP.CheckedChanged
        SetGroupBoxes()
    End Sub
    Private Sub radPLUS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radPLUS.CheckedChanged
        SetGroupBoxes()
    End Sub
    Private Sub radBoth_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radBoth.CheckedChanged
        SetGroupBoxes()
    End Sub
    Private Sub radNone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radNone.CheckedChanged
        SetGroupBoxes()
    End Sub
    Sub SetGroupBoxes()
        If radLP.Checked Then
            gbOldRes.Enabled = True
            gbCurrRes.Enabled = True
            gbGradRes.Enabled = False
            gboldEFT.Enabled = True
            gbGradEFT.Enabled = False
            gbCurrEFT.Enabled = True
        ElseIf radPLUS.Checked Then
            gbOldRes.Enabled = False
            gbCurrRes.Enabled = True
            gbGradRes.Enabled = True
            gboldEFT.Enabled = False
            gbGradEFT.Enabled = True
            gbCurrEFT.Enabled = True
        ElseIf radBoth.Checked Then
            gbOldRes.Enabled = True
            gbCurrRes.Enabled = True
            gbGradRes.Enabled = True
            gboldEFT.Enabled = True
            gbGradEFT.Enabled = True
            gbCurrEFT.Enabled = True
        ElseIf radNone.Checked Then
            gbOldRes.Enabled = False
            gbCurrRes.Enabled = False
            gbGradRes.Enabled = False
            gboldEFT.Enabled = False
            gbGradEFT.Enabled = False
            gbCurrEFT.Enabled = False
        End If
    End Sub
#End Region


    Private Sub rbUofU_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUofU.CheckedChanged
        If rbUofU.Checked Then
            radPLUS.Enabled = True
            radBoth.Enabled = True
        Else
            radPLUS.Enabled = False
            radBoth.Enabled = False
        End If
    End Sub
End Class
