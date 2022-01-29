Imports System.Threading


'public delegates for async calls
Public Delegate Sub RemoveItemFromLVDelegate(ByRef LV As ListView, ByRef LVI As ListViewItem)
Public Delegate Sub AddItemToLVDelegate(ByRef LV As ListView, ByRef LVI As ListViewItem)
Public Delegate Sub DisableControlDelegate(ByRef Cnt As Control)
Public Delegate Sub EnableControlDelegate(ByRef Cnt As Control)

Public Class frmMasterBatch
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'get run time/run option
        Dim runTime As New frmGetFutureRunTime
        runTime.ShowDialog()
        _selectedTimeDate = runTime.SelectedTimeDate
        _selectedRunOption = runTime.SelectedRunOption
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
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnValidate As System.Windows.Forms.Button
    Friend WithEvents lstSel As System.Windows.Forms.ListView
    Friend WithEvents txtPass As System.Windows.Forms.TextBox
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents lstIds As System.Windows.Forms.ListView
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents ch1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ch2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lstB1 As System.Windows.Forms.ListView
    Friend WithEvents Col1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Col2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lstB2 As System.Windows.Forms.ListView
    Friend WithEvents lstB3 As System.Windows.Forms.ListView
    Friend WithEvents lstB4 As System.Windows.Forms.ListView
    Friend WithEvents lstB5 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents lblRun As System.Windows.Forms.Label
    Friend WithEvents lblPause As System.Windows.Forms.Label
    Friend WithEvents lblClose As System.Windows.Forms.Label
    Friend WithEvents lblRefresh As System.Windows.Forms.Label
    Friend WithEvents PictureBox16 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox15 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox14 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox13 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblbatchNum As System.Windows.Forms.Label
    Friend WithEvents lblCurrent As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox12 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox11 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox10 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox9 As System.Windows.Forms.PictureBox
    Friend WithEvents lblB1 As System.Windows.Forms.Label
    Friend WithEvents lblB2 As System.Windows.Forms.Label
    Friend WithEvents lblB3 As System.Windows.Forms.Label
    Friend WithEvents lblB4 As System.Windows.Forms.Label
    Friend WithEvents lblB5 As System.Windows.Forms.Label
    Friend WithEvents pnlUser As System.Windows.Forms.Panel
    Friend WithEvents pnlLogo As System.Windows.Forms.Panel
    Friend WithEvents ckbMute As System.Windows.Forms.CheckBox
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMasterBatch))
        Me.lstSel = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.lstB5 = New System.Windows.Forms.ListView
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.lstB4 = New System.Windows.Forms.ListView
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.lstB3 = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.lstB2 = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.lstB1 = New System.Windows.Forms.ListView
        Me.Col1 = New System.Windows.Forms.ColumnHeader
        Me.Col2 = New System.Windows.Forms.ColumnHeader
        Me.btnAdd = New System.Windows.Forms.Button
        Me.lstIds = New System.Windows.Forms.ListView
        Me.ch1 = New System.Windows.Forms.ColumnHeader
        Me.ch2 = New System.Windows.Forms.ColumnHeader
        Me.btnValidate = New System.Windows.Forms.Button
        Me.txtPass = New System.Windows.Forms.TextBox
        Me.txtID = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.lblRun = New System.Windows.Forms.Label
        Me.lblPause = New System.Windows.Forms.Label
        Me.lblClose = New System.Windows.Forms.Label
        Me.lblRefresh = New System.Windows.Forms.Label
        Me.PictureBox16 = New System.Windows.Forms.PictureBox
        Me.PictureBox15 = New System.Windows.Forms.PictureBox
        Me.PictureBox14 = New System.Windows.Forms.PictureBox
        Me.PictureBox13 = New System.Windows.Forms.PictureBox
        Me.ckbMute = New System.Windows.Forms.CheckBox
        Me.pnlUser = New System.Windows.Forms.Panel
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.pnlLogo = New System.Windows.Forms.Panel
        Me.lblbatchNum = New System.Windows.Forms.Label
        Me.lblCurrent = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.PictureBox8 = New System.Windows.Forms.PictureBox
        Me.PictureBox7 = New System.Windows.Forms.PictureBox
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.PictureBox5 = New System.Windows.Forms.PictureBox
        Me.PictureBox12 = New System.Windows.Forms.PictureBox
        Me.PictureBox11 = New System.Windows.Forms.PictureBox
        Me.PictureBox10 = New System.Windows.Forms.PictureBox
        Me.PictureBox9 = New System.Windows.Forms.PictureBox
        Me.lblB1 = New System.Windows.Forms.Label
        Me.lblB2 = New System.Windows.Forms.Label
        Me.lblB3 = New System.Windows.Forms.Label
        Me.lblB4 = New System.Windows.Forms.Label
        Me.lblB5 = New System.Windows.Forms.Label
        Me.lblHelp = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Panel4.SuspendLayout()
        Me.pnlUser.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstSel
        '
        Me.lstSel.AllowDrop = True
        Me.lstSel.BackColor = System.Drawing.Color.Black
        Me.lstSel.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader11, Me.ColumnHeader2})
        Me.lstSel.Font = New System.Drawing.Font("r_ansi", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstSel.ForeColor = System.Drawing.Color.Lime
        Me.lstSel.FullRowSelect = True
        Me.lstSel.Location = New System.Drawing.Point(8, 18)
        Me.lstSel.MultiSelect = False
        Me.lstSel.Name = "lstSel"
        Me.lstSel.Size = New System.Drawing.Size(472, 254)
        Me.lstSel.TabIndex = 5
        Me.lstSel.TabStop = False
        Me.lstSel.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Script"
        Me.ColumnHeader1.Width = 251
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Status"
        Me.ColumnHeader11.Width = 82
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Resources / Holds"
        Me.ColumnHeader2.Width = 207
        '
        'lstB5
        '
        Me.lstB5.AllowDrop = True
        Me.lstB5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstB5.BackColor = System.Drawing.Color.Black
        Me.lstB5.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader9, Me.ColumnHeader10})
        Me.lstB5.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstB5.ForeColor = System.Drawing.Color.Lime
        Me.lstB5.FullRowSelect = True
        Me.lstB5.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstB5.Location = New System.Drawing.Point(743, 328)
        Me.lstB5.MultiSelect = False
        Me.lstB5.Name = "lstB5"
        Me.lstB5.Size = New System.Drawing.Size(154, 76)
        Me.lstB5.TabIndex = 25
        Me.lstB5.TabStop = False
        Me.lstB5.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Name"
        Me.ColumnHeader9.Width = 92
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Status"
        Me.ColumnHeader10.Width = 58
        '
        'lstB4
        '
        Me.lstB4.AllowDrop = True
        Me.lstB4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstB4.BackColor = System.Drawing.Color.Black
        Me.lstB4.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader7, Me.ColumnHeader8})
        Me.lstB4.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstB4.ForeColor = System.Drawing.Color.Lime
        Me.lstB4.FullRowSelect = True
        Me.lstB4.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstB4.Location = New System.Drawing.Point(743, 251)
        Me.lstB4.MultiSelect = False
        Me.lstB4.Name = "lstB4"
        Me.lstB4.Size = New System.Drawing.Size(154, 76)
        Me.lstB4.TabIndex = 24
        Me.lstB4.TabStop = False
        Me.lstB4.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Name"
        Me.ColumnHeader7.Width = 92
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Status"
        Me.ColumnHeader8.Width = 58
        '
        'lstB3
        '
        Me.lstB3.AllowDrop = True
        Me.lstB3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstB3.BackColor = System.Drawing.Color.Black
        Me.lstB3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lstB3.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstB3.ForeColor = System.Drawing.Color.Lime
        Me.lstB3.FullRowSelect = True
        Me.lstB3.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstB3.Location = New System.Drawing.Point(743, 174)
        Me.lstB3.MultiSelect = False
        Me.lstB3.Name = "lstB3"
        Me.lstB3.Size = New System.Drawing.Size(154, 76)
        Me.lstB3.TabIndex = 23
        Me.lstB3.TabStop = False
        Me.lstB3.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Name"
        Me.ColumnHeader5.Width = 92
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Status"
        Me.ColumnHeader6.Width = 58
        '
        'lstB2
        '
        Me.lstB2.AllowDrop = True
        Me.lstB2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstB2.BackColor = System.Drawing.Color.Black
        Me.lstB2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lstB2.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstB2.ForeColor = System.Drawing.Color.Lime
        Me.lstB2.FullRowSelect = True
        Me.lstB2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstB2.Location = New System.Drawing.Point(743, 97)
        Me.lstB2.MultiSelect = False
        Me.lstB2.Name = "lstB2"
        Me.lstB2.Size = New System.Drawing.Size(154, 76)
        Me.lstB2.TabIndex = 22
        Me.lstB2.TabStop = False
        Me.lstB2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Name"
        Me.ColumnHeader3.Width = 92
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Status"
        Me.ColumnHeader4.Width = 58
        '
        'lstB1
        '
        Me.lstB1.AllowDrop = True
        Me.lstB1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstB1.BackColor = System.Drawing.Color.Black
        Me.lstB1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Col1, Me.Col2})
        Me.lstB1.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstB1.ForeColor = System.Drawing.Color.Lime
        Me.lstB1.FullRowSelect = True
        Me.lstB1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstB1.Location = New System.Drawing.Point(743, 20)
        Me.lstB1.MultiSelect = False
        Me.lstB1.Name = "lstB1"
        Me.lstB1.Size = New System.Drawing.Size(154, 76)
        Me.lstB1.TabIndex = 21
        Me.lstB1.TabStop = False
        Me.lstB1.View = System.Windows.Forms.View.Details
        '
        'Col1
        '
        Me.Col1.Text = "Name"
        Me.Col1.Width = 92
        '
        'Col2
        '
        Me.Col2.Text = "Status"
        Me.Col2.Width = 58
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.Transparent
        Me.btnAdd.Font = New System.Drawing.Font("r_ansi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ForeColor = System.Drawing.Color.Lime
        Me.btnAdd.Location = New System.Drawing.Point(24, 56)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(177, 20)
        Me.btnAdd.TabIndex = 4
        Me.btnAdd.Text = "Add User ID"
        '
        'lstIds
        '
        Me.lstIds.BackColor = System.Drawing.Color.Black
        Me.lstIds.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ch1, Me.ch2})
        Me.lstIds.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstIds.Font = New System.Drawing.Font("r_ansi", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstIds.ForeColor = System.Drawing.Color.Lime
        Me.lstIds.FullRowSelect = True
        Me.lstIds.Location = New System.Drawing.Point(8, 80)
        Me.lstIds.Name = "lstIds"
        Me.lstIds.Size = New System.Drawing.Size(200, 144)
        Me.lstIds.TabIndex = 5
        Me.lstIds.View = System.Windows.Forms.View.Details
        '
        'ch1
        '
        Me.ch1.Text = "UserID"
        Me.ch1.Width = 91
        '
        'ch2
        '
        Me.ch2.Text = "Status"
        Me.ch2.Width = 105
        '
        'btnValidate
        '
        Me.btnValidate.BackColor = System.Drawing.Color.Transparent
        Me.btnValidate.Font = New System.Drawing.Font("r_ansi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnValidate.ForeColor = System.Drawing.Color.Lime
        Me.btnValidate.Location = New System.Drawing.Point(40, 232)
        Me.btnValidate.Name = "btnValidate"
        Me.btnValidate.Size = New System.Drawing.Size(144, 16)
        Me.btnValidate.TabIndex = 6
        Me.btnValidate.Text = "Validate"
        '
        'txtPass
        '
        Me.txtPass.BackColor = System.Drawing.Color.Black
        Me.txtPass.Font = New System.Drawing.Font("r_ansi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPass.ForeColor = System.Drawing.Color.Lime
        Me.txtPass.Location = New System.Drawing.Point(104, 32)
        Me.txtPass.Name = "txtPass"
        Me.txtPass.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtPass.Size = New System.Drawing.Size(104, 21)
        Me.txtPass.TabIndex = 3
        Me.txtPass.Text = ""
        '
        'txtID
        '
        Me.txtID.BackColor = System.Drawing.Color.Black
        Me.txtID.Font = New System.Drawing.Font("r_ansi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtID.ForeColor = System.Drawing.Color.Lime
        Me.txtID.Location = New System.Drawing.Point(8, 32)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(96, 21)
        Me.txtID.TabIndex = 2
        Me.txtID.Text = ""
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("r_ansi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Lime
        Me.Label7.Location = New System.Drawing.Point(112, 8)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 16)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Password"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("r_ansi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Lime
        Me.Label4.Location = New System.Drawing.Point(32, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 16)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "User ID"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Transparent
        Me.Panel4.BackgroundImage = CType(resources.GetObject("Panel4.BackgroundImage"), System.Drawing.Image)
        Me.Panel4.Controls.Add(Me.lblRun)
        Me.Panel4.Controls.Add(Me.lblPause)
        Me.Panel4.Controls.Add(Me.lblClose)
        Me.Panel4.Controls.Add(Me.lblRefresh)
        Me.Panel4.Controls.Add(Me.PictureBox16)
        Me.Panel4.Controls.Add(Me.PictureBox15)
        Me.Panel4.Controls.Add(Me.PictureBox14)
        Me.Panel4.Controls.Add(Me.PictureBox13)
        Me.Panel4.Controls.Add(Me.ckbMute)
        Me.Panel4.Location = New System.Drawing.Point(1, 267)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(212, 124)
        Me.Panel4.TabIndex = 55
        '
        'lblRun
        '
        Me.lblRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRun.ForeColor = System.Drawing.Color.Red
        Me.lblRun.Image = CType(resources.GetObject("lblRun.Image"), System.Drawing.Image)
        Me.lblRun.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.lblRun.Location = New System.Drawing.Point(136, 28)
        Me.lblRun.Name = "lblRun"
        Me.lblRun.Size = New System.Drawing.Size(56, 60)
        Me.lblRun.TabIndex = 64
        Me.lblRun.Text = "Run"
        Me.lblRun.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'lblPause
        '
        Me.lblPause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPause.ForeColor = System.Drawing.Color.Red
        Me.lblPause.Image = CType(resources.GetObject("lblPause.Image"), System.Drawing.Image)
        Me.lblPause.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.lblPause.Location = New System.Drawing.Point(76, 0)
        Me.lblPause.Name = "lblPause"
        Me.lblPause.Size = New System.Drawing.Size(56, 60)
        Me.lblPause.TabIndex = 63
        Me.lblPause.Text = "Pause"
        Me.lblPause.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'lblClose
        '
        Me.lblClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClose.ForeColor = System.Drawing.Color.Red
        Me.lblClose.Image = CType(resources.GetObject("lblClose.Image"), System.Drawing.Image)
        Me.lblClose.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.lblClose.Location = New System.Drawing.Point(76, 60)
        Me.lblClose.Name = "lblClose"
        Me.lblClose.Size = New System.Drawing.Size(56, 60)
        Me.lblClose.TabIndex = 62
        Me.lblClose.Text = "Close"
        Me.lblClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'lblRefresh
        '
        Me.lblRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRefresh.ForeColor = System.Drawing.Color.Red
        Me.lblRefresh.Image = CType(resources.GetObject("lblRefresh.Image"), System.Drawing.Image)
        Me.lblRefresh.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.lblRefresh.Location = New System.Drawing.Point(16, 0)
        Me.lblRefresh.Name = "lblRefresh"
        Me.lblRefresh.Size = New System.Drawing.Size(56, 60)
        Me.lblRefresh.TabIndex = 61
        Me.lblRefresh.Text = "Refresh"
        Me.lblRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'PictureBox16
        '
        Me.PictureBox16.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox16.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox16.Image = CType(resources.GetObject("PictureBox16.Image"), System.Drawing.Image)
        Me.PictureBox16.Location = New System.Drawing.Point(196, 108)
        Me.PictureBox16.Name = "PictureBox16"
        Me.PictureBox16.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox16.TabIndex = 60
        Me.PictureBox16.TabStop = False
        '
        'PictureBox15
        '
        Me.PictureBox15.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox15.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox15.Image = CType(resources.GetObject("PictureBox15.Image"), System.Drawing.Image)
        Me.PictureBox15.Location = New System.Drawing.Point(1, 108)
        Me.PictureBox15.Name = "PictureBox15"
        Me.PictureBox15.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox15.TabIndex = 59
        Me.PictureBox15.TabStop = False
        '
        'PictureBox14
        '
        Me.PictureBox14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox14.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox14.Image = CType(resources.GetObject("PictureBox14.Image"), System.Drawing.Image)
        Me.PictureBox14.Location = New System.Drawing.Point(196, 1)
        Me.PictureBox14.Name = "PictureBox14"
        Me.PictureBox14.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox14.TabIndex = 58
        Me.PictureBox14.TabStop = False
        '
        'PictureBox13
        '
        Me.PictureBox13.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox13.Image = CType(resources.GetObject("PictureBox13.Image"), System.Drawing.Image)
        Me.PictureBox13.Location = New System.Drawing.Point(0, 2)
        Me.PictureBox13.Name = "PictureBox13"
        Me.PictureBox13.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox13.TabIndex = 57
        Me.PictureBox13.TabStop = False
        '
        'ckbMute
        '
        Me.ckbMute.Appearance = System.Windows.Forms.Appearance.Button
        Me.ckbMute.Font = New System.Drawing.Font("Stencil", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ckbMute.ForeColor = System.Drawing.Color.Black
        Me.ckbMute.Location = New System.Drawing.Point(16, 76)
        Me.ckbMute.Name = "ckbMute"
        Me.ckbMute.Size = New System.Drawing.Size(60, 32)
        Me.ckbMute.TabIndex = 65
        Me.ckbMute.Text = "Stealth"
        Me.ckbMute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlUser
        '
        Me.pnlUser.BackgroundImage = CType(resources.GetObject("pnlUser.BackgroundImage"), System.Drawing.Image)
        Me.pnlUser.Controls.Add(Me.PictureBox4)
        Me.pnlUser.Controls.Add(Me.PictureBox3)
        Me.pnlUser.Controls.Add(Me.PictureBox2)
        Me.pnlUser.Controls.Add(Me.PictureBox1)
        Me.pnlUser.Controls.Add(Me.lstIds)
        Me.pnlUser.Controls.Add(Me.btnValidate)
        Me.pnlUser.Controls.Add(Me.Label7)
        Me.pnlUser.Controls.Add(Me.Label4)
        Me.pnlUser.Controls.Add(Me.btnAdd)
        Me.pnlUser.Controls.Add(Me.txtPass)
        Me.pnlUser.Controls.Add(Me.txtID)
        Me.pnlUser.Controls.Add(Me.pnlLogo)
        Me.pnlUser.Location = New System.Drawing.Point(0, 8)
        Me.pnlUser.Name = "pnlUser"
        Me.pnlUser.Size = New System.Drawing.Size(214, 254)
        Me.pnlUser.TabIndex = 56
        '
        'PictureBox4
        '
        Me.PictureBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(198, 2)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox4.TabIndex = 10
        Me.PictureBox4.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(2, 2)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox3.TabIndex = 9
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(197, 237)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox2.TabIndex = 8
        Me.PictureBox2.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 237)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox1.TabIndex = 7
        Me.PictureBox1.TabStop = False
        '
        'pnlLogo
        '
        Me.pnlLogo.BackgroundImage = CType(resources.GetObject("pnlLogo.BackgroundImage"), System.Drawing.Image)
        Me.pnlLogo.Location = New System.Drawing.Point(8, 96)
        Me.pnlLogo.Name = "pnlLogo"
        Me.pnlLogo.Size = New System.Drawing.Size(176, 128)
        Me.pnlLogo.TabIndex = 11
        '
        'lblbatchNum
        '
        Me.lblbatchNum.BackColor = System.Drawing.Color.Transparent
        Me.lblbatchNum.Font = New System.Drawing.Font("Stencil", 36.0!)
        Me.lblbatchNum.Location = New System.Drawing.Point(450, 481)
        Me.lblbatchNum.Name = "lblbatchNum"
        Me.lblbatchNum.Size = New System.Drawing.Size(65, 54)
        Me.lblbatchNum.TabIndex = 58
        Me.lblbatchNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCurrent
        '
        Me.lblCurrent.BackColor = System.Drawing.Color.Transparent
        Me.lblCurrent.Font = New System.Drawing.Font("Stencil", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrent.ForeColor = System.Drawing.Color.FromArgb(CType(192, Byte), CType(0, Byte), CType(0, Byte))
        Me.lblCurrent.Location = New System.Drawing.Point(279, 312)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(378, 30)
        Me.lblCurrent.TabIndex = 57
        Me.lblCurrent.Text = "Waiting for Authrization"
        Me.lblCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BackgroundImage = CType(resources.GetObject("Panel2.BackgroundImage"), System.Drawing.Image)
        Me.Panel2.Controls.Add(Me.PictureBox8)
        Me.Panel2.Controls.Add(Me.PictureBox7)
        Me.Panel2.Controls.Add(Me.PictureBox6)
        Me.Panel2.Controls.Add(Me.PictureBox5)
        Me.Panel2.Controls.Add(Me.lstSel)
        Me.Panel2.Location = New System.Drawing.Point(224, 8)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(488, 291)
        Me.Panel2.TabIndex = 63
        '
        'PictureBox8
        '
        Me.PictureBox8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox8.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox8.Image = CType(resources.GetObject("PictureBox8.Image"), System.Drawing.Image)
        Me.PictureBox8.Location = New System.Drawing.Point(3, 275)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox8.TabIndex = 14
        Me.PictureBox8.TabStop = False
        '
        'PictureBox7
        '
        Me.PictureBox7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox7.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox7.Image = CType(resources.GetObject("PictureBox7.Image"), System.Drawing.Image)
        Me.PictureBox7.Location = New System.Drawing.Point(474, 276)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(13, 15)
        Me.PictureBox7.TabIndex = 13
        Me.PictureBox7.TabStop = False
        '
        'PictureBox6
        '
        Me.PictureBox6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox6.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(474, 3)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(13, 15)
        Me.PictureBox6.TabIndex = 12
        Me.PictureBox6.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(2, 1)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(13, 14)
        Me.PictureBox5.TabIndex = 11
        Me.PictureBox5.TabStop = False
        '
        'PictureBox12
        '
        Me.PictureBox12.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox12.Image = CType(resources.GetObject("PictureBox12.Image"), System.Drawing.Image)
        Me.PictureBox12.Location = New System.Drawing.Point(678, 437)
        Me.PictureBox12.Name = "PictureBox12"
        Me.PictureBox12.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox12.TabIndex = 67
        Me.PictureBox12.TabStop = False
        '
        'PictureBox11
        '
        Me.PictureBox11.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox11.Image = CType(resources.GetObject("PictureBox11.Image"), System.Drawing.Image)
        Me.PictureBox11.Location = New System.Drawing.Point(249, 437)
        Me.PictureBox11.Name = "PictureBox11"
        Me.PictureBox11.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox11.TabIndex = 66
        Me.PictureBox11.TabStop = False
        '
        'PictureBox10
        '
        Me.PictureBox10.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox10.Image = CType(resources.GetObject("PictureBox10.Image"), System.Drawing.Image)
        Me.PictureBox10.Location = New System.Drawing.Point(678, 348)
        Me.PictureBox10.Name = "PictureBox10"
        Me.PictureBox10.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox10.TabIndex = 65
        Me.PictureBox10.TabStop = False
        '
        'PictureBox9
        '
        Me.PictureBox9.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox9.Image = CType(resources.GetObject("PictureBox9.Image"), System.Drawing.Image)
        Me.PictureBox9.Location = New System.Drawing.Point(249, 348)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox9.TabIndex = 64
        Me.PictureBox9.TabStop = False
        '
        'lblB1
        '
        Me.lblB1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblB1.BackColor = System.Drawing.Color.Transparent
        Me.lblB1.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblB1.Image = CType(resources.GetObject("lblB1.Image"), System.Drawing.Image)
        Me.lblB1.Location = New System.Drawing.Point(714, 21)
        Me.lblB1.Name = "lblB1"
        Me.lblB1.Size = New System.Drawing.Size(30, 65)
        Me.lblB1.TabIndex = 72
        Me.lblB1.Text = "Run All"
        Me.lblB1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblB2
        '
        Me.lblB2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblB2.BackColor = System.Drawing.Color.Transparent
        Me.lblB2.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblB2.Image = CType(resources.GetObject("lblB2.Image"), System.Drawing.Image)
        Me.lblB2.Location = New System.Drawing.Point(714, 98)
        Me.lblB2.Name = "lblB2"
        Me.lblB2.Size = New System.Drawing.Size(30, 65)
        Me.lblB2.TabIndex = 71
        Me.lblB2.Text = "Run All"
        Me.lblB2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblB3
        '
        Me.lblB3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblB3.BackColor = System.Drawing.Color.Transparent
        Me.lblB3.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblB3.Image = CType(resources.GetObject("lblB3.Image"), System.Drawing.Image)
        Me.lblB3.Location = New System.Drawing.Point(714, 175)
        Me.lblB3.Name = "lblB3"
        Me.lblB3.Size = New System.Drawing.Size(30, 65)
        Me.lblB3.TabIndex = 70
        Me.lblB3.Text = "Run All"
        Me.lblB3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblB4
        '
        Me.lblB4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblB4.BackColor = System.Drawing.Color.Transparent
        Me.lblB4.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblB4.Image = CType(resources.GetObject("lblB4.Image"), System.Drawing.Image)
        Me.lblB4.Location = New System.Drawing.Point(714, 250)
        Me.lblB4.Name = "lblB4"
        Me.lblB4.Size = New System.Drawing.Size(30, 65)
        Me.lblB4.TabIndex = 69
        Me.lblB4.Text = "Run All"
        Me.lblB4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblB5
        '
        Me.lblB5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblB5.BackColor = System.Drawing.Color.Transparent
        Me.lblB5.Font = New System.Drawing.Font("r_ansi", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblB5.Image = CType(resources.GetObject("lblB5.Image"), System.Drawing.Image)
        Me.lblB5.Location = New System.Drawing.Point(714, 328)
        Me.lblB5.Name = "lblB5"
        Me.lblB5.Size = New System.Drawing.Size(30, 65)
        Me.lblB5.TabIndex = 68
        Me.lblB5.Text = "Run All"
        Me.lblB5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHelp
        '
        Me.lblHelp.BackColor = System.Drawing.Color.Transparent
        Me.lblHelp.Location = New System.Drawing.Point(775, 478)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(113, 139)
        Me.lblHelp.TabIndex = 73
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Master Batch"
        Me.NotifyIcon1.Visible = True
        '
        'frmMasterBatch
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(64, Byte), CType(64, Byte))
        Me.ClientSize = New System.Drawing.Size(900, 633)
        Me.Controls.Add(Me.lblHelp)
        Me.Controls.Add(Me.lblB1)
        Me.Controls.Add(Me.lblB2)
        Me.Controls.Add(Me.lblB3)
        Me.Controls.Add(Me.lblB4)
        Me.Controls.Add(Me.lblB5)
        Me.Controls.Add(Me.PictureBox12)
        Me.Controls.Add(Me.PictureBox11)
        Me.Controls.Add(Me.PictureBox10)
        Me.Controls.Add(Me.PictureBox9)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.lblbatchNum)
        Me.Controls.Add(Me.lblCurrent)
        Me.Controls.Add(Me.pnlUser)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.lstB1)
        Me.Controls.Add(Me.lstB4)
        Me.Controls.Add(Me.lstB3)
        Me.Controls.Add(Me.lstB5)
        Me.Controls.Add(Me.lstB2)
        Me.ForeColor = System.Drawing.Color.Yellow
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(910, 662)
        Me.MinimumSize = New System.Drawing.Size(910, 662)
        Me.Name = "frmMasterBatch"
        Me.Text = "Master Batch Script"
        Me.Panel4.ResumeLayout(False)
        Me.pnlUser.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region " Class Variables "
    'Dim UserList As New Collection
    Dim Tnk As New Tank
    Public DoDrawPie As Thread
    Private MainScriptThread As Threading.Thread
    Private Sound As New Audio
    Private Mute As Boolean
    Private _selectedTimeDate As DateTime
    Private _selectedRunOption As frmGetFutureRunTime.RunOption
    'Private CoverUsers As Boolean 'This is true after the run button has been pressed.
#End Region

    
#Region " Delegate Targets "

    'delegate target to be called for List view update
    Public Sub RemoveItemFromLV(ByRef LV As ListView, ByRef LVI As ListViewItem)
        'remove specified item from a list view
        LV.Items.Remove(LVI)
    End Sub

    'delegate target to be called for List view update
    Public Sub AddItemToLV(ByRef LV As ListView, ByRef LVI As ListViewItem)
        'remove specified item from a list view
        LV.Items.Add(LVI)
    End Sub

    Sub DisableControl(ByRef Cnt As Control)
        Cnt.Enabled = False
    End Sub

    Sub EnableControl(ByRef Cnt As Control)
        Cnt.Enabled = True
    End Sub

#End Region

#Region " Animation "
    Sub DrawBang()
        'Me.CreateGraphics.DrawImage(Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\boom.gif"), New RectangleF(Me.Width / 2 - 50, Me.Height / 2, 100, 100))
        Me.CreateGraphics.DrawImage(Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\boom.gif"), New RectangleF(Me.Width / 2 - 50, Me.Height / 2, 100, 100))
    End Sub

    Sub TurnLeft()
        If Tnk.ShowTanks = False Then Exit Sub
        If Tnk.TargetBatch > 1 Then
            Tnk.TargetBatch = Tnk.TargetBatch - 1
            lblbatchNum.Text = Tnk.TargetBatch
        End If
        Tnk.lefthandDown = True
        Tnk.DrawTank()
        Me.BackgroundImage = Tnk.BackBmp
        Me.Refresh()
        Tnk.lefthandDown = False
        Tnk.DrawTank()
        Me.BackgroundImage = Tnk.BackBmp
        Me.Refresh()
    End Sub
    Sub TurnRight()
        If Tnk.ShowTanks = False Then Exit Sub
        If Tnk.TargetBatch < 5 Then
            Tnk.TargetBatch = Tnk.TargetBatch + 1
            lblbatchNum.Text = Tnk.TargetBatch
        End If
        Tnk.righthandDown = True
        Tnk.DrawTank()
        Me.BackgroundImage = Tnk.BackBmp
        Me.Refresh()
        Tnk.righthandDown = False
        Tnk.DrawTank()
        Me.BackgroundImage = Tnk.BackBmp
        Me.Refresh()
    End Sub

    Private Sub DrawPie()
        Dim x As Integer
        Dim c As Integer
        Dim brush As System.drawing.SolidBrush
        Dim rect As Rectangle
        brush = New System.Drawing.SolidBrush(Color.Green)
        Dim angles() As Single = {0, 4, 8, 12, 16, 20, 24, 28, 32, 36, 40}
        'Dim colors As New ArrayList
        Dim colors(0) As Color
        c = 0
        For x = 1 To 10
            ReDim Preserve colors(x)
            colors(x) = Color.FromArgb(0, c, 0)
            'colors.Add(Color.FromArgb(0, c, 0))
            c += 25
        Next
        Try
            rect = New Rectangle(0, 0, lstSel.Width - 5, lstSel.Height - 5)
            Dim angle As Integer
            Do

                For angle = 1 To angles.GetUpperBound(0)
                    brush.Color = colors(angle - 1)
                    angles(angle) = angles(angle) + 2
                    If angles(angle) > 360 Then
                        For x = 0 To angles.GetUpperBound(0)
                            angles(x) = angles(x) - 360
                        Next
                    End If

                    Dim g As System.Drawing.Graphics
                    g = lstSel.CreateGraphics
                    If (angles(angle) > 270 And angles(angle) < 360) Or (angles(angle) > 0 And angles(angle) < 30) Then
                        'If angles(angle) < 30 Then
                        '    g.DrawString("Batch 1", lstSel.Font, New System.Drawing.SolidBrush(Color.FromArgb(0, CInt(255 - (360 + angles(angle) - 270) * 2), 0)), 60, 5)
                        'Else
                        '    g.DrawString("Jay " & PriSum(1, 0), lstSel.Font, New System.Drawing.SolidBrush(Color.FromArgb(0, CInt(255 - (angles(angle) - 270) * 2), 0)), 60, 5)
                        'End If
                    End If
                    If angles(angle) > 180 And angles(angle) < 300 Then
                        'g.DrawString("Batch 2", lstSel.Font, New System.Drawing.SolidBrush(Color.FromArgb(0, CInt(255 - (angles(angle) - 180) * 2), 0)), 10, 20)
                    End If
                    If angles(angle) > 90 And angles(angle) < 210 Then
                        'g.DrawString("Batch 3", lstSel.Font, New System.Drawing.SolidBrush(Color.FromArgb(0, CInt(255 - (angles(angle) - 90) * 2), 0)), 60, 30)
                    End If
                    If angles(angle) > 0 And angles(angle) < 120 Then
                        'g.DrawString("Batch 4", lstSel.Font, New System.Drawing.SolidBrush(Color.FromArgb(0, CInt(255 - (angles(angle) - 0) * 2), 0)), 100, 20)
                    End If

                    g.FillPie(brush, rect, angles(angle - 1), angles(angle) - angles(angle - 1))
                    g.Dispose()
                    'If angles(angle) = 360 Then
                    '    angles(angle) = 0
                    'End If
                Next
                'g.DrawEllipse(Pens.DarkGreen, rect)
                Thread.Sleep(20)
                'g.Dispose()
            Loop
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region " Scheduling "

    Sub GetScripts()
        Dim DS As DataSet
        Dim x As Integer
        Dim x2 As Integer
        Dim Index As Integer
        Dim delcnt As Integer 'count number deleted from list
        Dim R As DataRow
        Dim RArr() As DataRow
        Dim KeepRow As Boolean
        Dim DateArr() As Date 'list of dates that the script ran within the last five days
        Dim FoundDate As Boolean
        EnableScriptsAndBatchs(False)
        'collect all scripts
        DS = GetSacker("Select A.Script, dMon, dTue, dWed, dThu, dFri, mJan, mFeb, mMar, mApr, mMay, mJun, mJul, mAug, mSep, mOct, mNov, mDec, mOn, OnDemandDate, Other, Hold, Holder, Duplex, Perforated, Manual, UserID, B.ID as LogFile, B.[Module], B.Subroutine from SCKR_DAT_ScriptSchedule A inner join SCKR_DAT_Scripts B on A.Script = B.Script Where B.Status = 'Active' ORDER BY A.Script")
        ReDim RArr(0)

        'add column for recovery
        DS.Tables(0).Columns.Add("Status")
        DS.Tables(0).Columns.Add("Batch")


        For Each R In DS.Tables(0).Rows
            KeepRow = False
            R.Item("Status") = ""
            R.Item("Batch") = ""
            For x = 0 To -5 Step -1
                'check the most recent date this should have run
                If ShouldHaveRun(R, _selectedTimeDate.Date.AddDays(x)) Then '**
                    KeepRow = True
                    If RunMe(R, _selectedTimeDate.Date.AddDays(x)) = False Then '**
                        KeepRow = False
                    End If
                    Exit For
                End If
            Next x
            'Make list of rows to remove
            If KeepRow = False Then
                RArr(UBound(RArr)) = R
                ReDim Preserve RArr(UBound(RArr) + 1)
            End If
        Next
        ReDim Preserve RArr(UBound(RArr) - 1)
        'remove unscheduled rows
        For Each R In RArr
            DS.Tables(0).Rows.Remove(R)
        Next

        LoadListView(DS)
    End Sub

    Function ShouldHaveRun(ByVal R As DataRow, ByVal CheckDate As Date) As Boolean
        Dim mOn As String
        If R.Item("mOn") Is System.DBNull.Value Then
            'MsgBox("DBNull")
            mOn = ""
        Else
            mOn = R.Item("mOn")
        End If
        'Monday
        If CheckDate.DayOfWeek = 1 And R.Item("dMon") = True Then
            ShouldHaveRun = True
        End If
        'Tuesday
        If CheckDate.DayOfWeek = 2 And R.Item("dTue") = True Then
            ShouldHaveRun = True
        End If
        'Wednessday
        If CheckDate.DayOfWeek = 3 And R.Item("dWed") = True Then
            ShouldHaveRun = True
        End If
        'Thursday
        If CheckDate.DayOfWeek = 4 And R.Item("dThu") = True Then
            ShouldHaveRun = True
        End If
        'Friday
        If CheckDate.DayOfWeek = 5 And R.Item("dFri") = True Then
            ShouldHaveRun = True
        End If

        'January
        If CheckDate.Month = 1 And R.Item("mJan") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'Febuary
        If CheckDate.Month = 2 And R.Item("mFeb") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'March
        If CheckDate.Month = 3 And R.Item("mMar") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'April
        If CheckDate.Month = 4 And R.Item("mApr") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'May
        If CheckDate.Month = 5 And R.Item("mMay") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'June
        If CheckDate.Month = 6 And R.Item("mJun") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'July
        If CheckDate.Month = 7 And R.Item("mJul") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'Aug
        If CheckDate.Month = 8 And R.Item("mAug") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'September
        If CheckDate.Month = 9 And R.Item("mSep") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'October
        If CheckDate.Month = 10 And R.Item("mOct") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'November
        If CheckDate.Month = 11 And R.Item("mNov") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'December
        If CheckDate.Month = 12 And R.Item("mDec") = True And _
        InStr(mOn, nthDay(CheckDate.Day)) Then
            ShouldHaveRun = True
        End If
        'OnDemand Date
        If R.Item("OnDemandDate").GetType.ToString <> "System.DBNull" Then
            If CheckDate = R.Item("OnDemandDate") Then
                ShouldHaveRun = True
            End If
        End If

    End Function

    Function nthDay(ByVal d As Integer) As String
        Dim ending As String
        Dim d2 As String
        d2 = Format(d, "00")
        If d2.Chars(1) = "1" Then
            If d2.Chars(0) = "1" Then
                ending = "th"
            Else
                ending = "st"
            End If
        ElseIf d2.Chars(1) = "2" Then
            If d2.Chars(0) = "1" Then
                ending = "th"
            Else
                ending = "nd"
            End If
        ElseIf d2.Chars(1) = "3" Then
            If d2.Chars(0) = "1" Then
                ending = "th"
            Else
                ending = "rd"
            End If
        Else
            ending = "th"
        End If
        Return d & ending
    End Function

    Function RunMe(ByRef R As DataRow, ByVal checkDate As Date) As Boolean
        'returns true if the script should be run
        'This also sets the status for recovery
        Dim DS As New DataSet
        Dim R2 As DataRow
        'DS = GetMasterBatchData("SELECT Status, PC, Batch FROM ScriptHistory WHERE ActionTime > '" & checkDate & "' AND ActionTime < '" & checkDate.AddDays(1) & "' AND Script = '" & R.Item("Script") & "'")
        DS = GetMasterBatchData("SELECT Status, PC, Batch FROM ScriptHistory WHERE ID = (Select MAX(ID) as TID from ScriptHistory WHERE ActionTime > '" & checkDate & "' AND ActionTime < '" & Today.AddDays(1) & "' AND Script = '" & R.Item("Script") & "')")
        If DS.Tables(0).Rows.Count > 0 Then
            For Each R2 In DS.Tables(0).Rows
                'MsgBox("*" & R2.Item("Status") & "* *" & R2.Item("PC") & "*")
                If R2.Item("Status") = "Complete" Then
                    Return False
                ElseIf R2.Item("Status") = "Debug" And Trim(R2.Item("PC")) = Environment.MachineName Then
                    R.Item("Status") = "Debug"
                    Return True
                ElseIf R2.Item("Status") = "Pending" Then
                    Return True
                ElseIf R2.Item("Status") = "Scheduled" And Trim(R2.Item("PC")) = Environment.MachineName Then
                    R.Item("Status") = "Scheduled"
                    R.Item("Batch") = R2.Item("Batch")
                    Return True
                ElseIf R2.Item("Status") = "Ready" And Trim(R2.Item("PC")) = Environment.MachineName Then
                    R.Item("Status") = "Scheduled"
                    R.Item("Batch") = R2.Item("Batch")
                    Return True
                ElseIf R2.Item("Status") = "Running" And Trim(R2.Item("PC")) = Environment.MachineName Then
                    R.Item("Status") = "Debug"
                    Return True
                Else
                    Return False
                End If
            Next
            Return True
        Else
            'did not run this day
            Return True
        End If
    End Function

    Sub LoadListView(ByVal ds As DataSet)
        'Add to list view
        Dim Index As Integer
        Dim S As ScriptItem
        Dim R As DataRow
        lstSel.Items.Clear()
        lstB1.Items.Clear()
        lstB2.Items.Clear()
        lstB3.Items.Clear()
        lstB4.Items.Clear()
        lstB5.Items.Clear()
        'For Index = 0 To ds.Tables(0).Rows.Count - 1
        For Each R In ds.Tables(0).Rows
            If (R.Item("Module") Is System.DBNull.Value) = False And _
            (R.Item("Subroutine") Is System.DBNull.Value) = False And _
            (R.Item("LogFile") Is System.DBNull.Value) = False Then
                S = New ScriptItem
                If R.Item("UserID") Is System.DBNull.Value Then
                    R.Item("UserID") = ""
                End If
                If R.Item("mOn") Is System.DBNull.Value Then
                    R.Item("mOn") = ""
                End If
                If R.Item("Other") Is System.DBNull.Value Then
                    R.Item("Other") = ""
                End If
                If R.Item("OnDemandDate") Is System.DBNull.Value Then
                    S.SetData(R.Item("Script"), R.Item("dMon"), R.Item("dTue"), R.Item("dWed"), R.Item("dThu"), R.Item("dFri"), R.Item("mJan"), R.Item("mFeb"), R.Item("mMar"), R.Item("mApr"), R.Item("mMay"), R.Item("mJun"), R.Item("mJul"), R.Item("mAug"), R.Item("mSep"), R.Item("mOct"), R.Item("mNov"), R.Item("mDec"), R.Item("mOn"), Nothing, R.Item("Other"), R.Item("Hold"), R.Item("Holder"), R.Item("Duplex"), R.Item("Perforated"), R.Item("Manual"), R.Item("UserID"), R.Item("LogFile"), R.Item("Module") & "." & R.Item("Subroutine"), TestMode, R.Item("Status"))
                Else
                    S.SetData(R.Item("Script"), R.Item("dMon"), R.Item("dTue"), R.Item("dWed"), R.Item("dThu"), R.Item("dFri"), R.Item("mJan"), R.Item("mFeb"), R.Item("mMar"), R.Item("mApr"), R.Item("mMay"), R.Item("mJun"), R.Item("mJul"), R.Item("mAug"), R.Item("mSep"), R.Item("mOct"), R.Item("mNov"), R.Item("mDec"), R.Item("mOn"), R.Item("OnDemandDate"), R.Item("Other"), R.Item("Hold"), R.Item("Holder"), R.Item("Duplex"), R.Item("Perforated"), R.Item("Manual"), R.Item("UserID"), R.Item("LogFile"), R.Item("Module") & "." & R.Item("Subroutine"), TestMode, R.Item("Status"))
                End If
                'S.SetData(R.Item("Script"), R.Item("dMon"), R.Item("dTue"), R.Item("dWed"), R.Item("dThu"), R.Item("dFri"), R.Item("mJan"), R.Item("mFeb"), R.Item("mMar"), R.Item("mApr"), R.Item("mMay"), R.Item("mJun"), R.Item("mJul"), R.Item("mAug"), R.Item("mSep"), R.Item("mOct"), R.Item("mNov"), R.Item("mDec"), R.Item("mOn"), R.Item("OnDemandDate"), R.Item("Other"), R.Item("Hold"), R.Item("Holder"), R.Item("Duplex"), R.Item("Perforated"), R.Item("Manual"), R.Item("UserID"), R.Item("LogFile"), R.Item("Module") & "." & R.Item("Subroutine"), TestMode, R.Item("Status"))
                If R.Item("Batch") = "1" Then
                    lstB1.Items.Add(S)
                    S.SetScheduled()
                ElseIf R.Item("Batch") = "2" Then
                    lstB2.Items.Add(S)
                    S.SetScheduled()
                ElseIf R.Item("Batch") = "3" Then
                    lstB3.Items.Add(S)
                    S.SetScheduled()
                ElseIf R.Item("Batch") = "4" Then
                    lstB4.Items.Add(S)
                    S.SetScheduled()
                ElseIf R.Item("Batch") = "5" Then
                    lstB5.Items.Add(S)
                    S.SetScheduled()
                Else
                    lstSel.Items.Add(S)
                    S.SetPending()
                End If
            End If
        Next
    End Sub

#End Region


    Sub AddUser()
        'Add user to the colleciton 
        If Mid(UCase(txtID.Text), 1, 4) <> "UT00" Or txtPass.Text = "" Or txtID.Text.Length <> 7 Then
            txtID.Focus()
            Exit Sub
        End If
        Dim U As New UserID
        U.SetID(txtID.Text)
        U.Pass = txtPass.Text
        U.SetNotVerified()
        'U.TestMode = TestMode
        Try
            lstIds.Items.Add(U)
            lstIds.Refresh()
        Catch ex As ArgumentException
            'duplicate found
            MsgBox("You can not use the same UserID twice!")
            Exit Sub
        End Try
        txtID.Text = ""
        txtPass.Text = ""
        txtID.Focus()
    End Sub
    Sub RemoveUser(ByVal key As ListViewItem)
        'remove Userid from database for this PC
        Dim Conn As New SqlClient.SqlConnection
        Dim Comm As New SqlClient.SqlCommand
        If TestMode Then
            Conn.ConnectionString = TestSQLConnStr
        Else
            Conn.ConnectionString = LiveSQLConnStr
        End If
        Comm.Connection = Conn
        Conn.Open()
        'clear all user IDs for this PC
        Comm.CommandText = "DELETE FROM UserIDsInUse WHERE PCName = '" & Environment.MachineName & "' AND UserID = '" & key.Text & "'"
        Comm.ExecuteNonQuery()
        'remove from listview
        lstIds.Items.Remove(key)
        lstIds.Refresh()
    End Sub

#Region "Controls"
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If lstIds.Items.Count > 14 Then
            MsgBox("You have exceeded your limit of 14 for User IDs.")
            Exit Sub
        End If
        AddUser()
    End Sub
    Private Sub txtPass_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPass.KeyPress
        If e.KeyChar = Chr(13) Then
            AddUser()
            e.Handled = True
        End If
    End Sub
    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtID.KeyPress
        If e.KeyChar = Chr(13) Then
            txtPass.Focus()
            e.Handled = True
        End If
    End Sub
    Private Sub btnValidate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidate.Click
        Dim PWChecker As New PasswordCheck(lstIds)
        Dim Processing As New frmValidatingUIDs
        Me.Visible = False

        If PWChecker.PerformPWCheck() Then
            'at least one password was validated
            If DoDrawPie Is Nothing = False Then
                If DoDrawPie.IsAlive Then
                    DoDrawPie.Abort()
                End If
            End If
            Processing.Visible = True
            Processing.Refresh()
            GetScripts()
            lblCurrent.Text = "Targets Aquired"
            Tnk.ShowTanks = True
            Tnk.DrawTank()
            Me.BackgroundImage = Tnk.BackBmp
            EnableScriptsAndBatchs(True)
            Processing.Visible = False
            Me.Visible = True
        Else
            'none of the passwords were validated
            MessageBox.Show("None of the user IDs could be validated.  Please try again.", "No Valid User IDs", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        Me.Visible = True
    End Sub
    Private Sub lblRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblRun.Click
        Cursor.Current = System.Windows.Forms.Cursors.No

        'if user selected to run at a future date and/or time then go to sleep
        If _selectedRunOption = frmGetFutureRunTime.RunOption.Future Then
            'hide main form
            Me.Hide()
            Me.Refresh()
            'sleep until it is the selected run time
            System.Threading.Thread.CurrentThread.Sleep(_selectedTimeDate.Subtract(Now))
            'show form
            Me.Show()
            Me.Refresh()
        End If

        MainScriptThread = New Thread(AddressOf MainScriptProc)
        MainScriptThread.IsBackground = True
        MainScriptThread.Start()
    End Sub
    Private Sub lblRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblRefresh.Click
        'clear all list and start over

        'remove userids from database
        Dim Conn As New SqlClient.SqlConnection
        Dim Comm As New SqlClient.SqlCommand
        If TestMode Then
            Conn.ConnectionString = TestSQLConnStr
        Else
            Conn.ConnectionString = LiveSQLConnStr
        End If
        Comm.Connection = Conn
        Conn.Open()
        '***********************************************************************
        '*      Vaughn didn't like the user ID part to this and I couldn't see anywhere that it was requested it in the spec so I removed it.  AA
        '*      clear all user IDs for this PC
        '*      Comm.CommandText = "DELETE FROM UserIDsInUse WHERE PCName = '" & Environment.MachineName & "'"
        '*      Comm.ExecuteNonQuery()
        '*      lstIds.Items.Clear()
        '***********************************************************************

        'move from Batch listviews to master listview
        returnScriptToSelectLV(lstB1)
        returnScriptToSelectLV(lstB2)
        returnScriptToSelectLV(lstB3)
        returnScriptToSelectLV(lstB4)
        returnScriptToSelectLV(lstB5)

        lstSel.Items.Clear()
        lstB1.Items.Clear()
        lstB2.Items.Clear()
        lstB3.Items.Clear()
        lstB4.Items.Clear()
        lstB5.Items.Clear()
        Tnk.Batch1Done = False
        Tnk.Batch2Done = False
        Tnk.Batch3Done = False
        Tnk.Batch4Done = False
        Tnk.Batch5Done = False
        lblbatchNum.Text = ""
        EnableScriptsAndBatchs(False)
        Tnk.ShowTanks = False
        Tnk.DrawTank()
        Me.BackgroundImage = Tnk.BackBmp
        txtID.Text = ""
        txtPass.Text = ""
        lblCurrent.Text = "Waiting for Authorization"
        'If DoDrawPie Is Nothing Then
        '    DoDrawPie = New Thread(AddressOf DrawPie)
        '    DoDrawPie.IsBackground = True
        '    DoDrawPie.Start()
        'Else
        '    If DoDrawPie.ThreadState = ThreadState.Stopped Then
        '        DoDrawPie = New Thread(AddressOf DrawPie)
        '        DoDrawPie.IsBackground = True
        '        DoDrawPie.Start()
        '    Else
        '        If InStr(DoDrawPie.ThreadState.ToString, "Suspended") > 0 Then
        '            DoDrawPie.Resume()
        '        End If
        '    End If
        'End If
        btnValidate.PerformClick()
    End Sub

    Private Sub returnScriptToSelectLV(ByVal lv As ListView)
        'this will return all the scripts from the batch list view to the selection list view
        Dim S As ScriptItem 'Item recieved
        For Each S In lv.Items
            Dim TS As New ScriptItem 'Item to add to batch box
            TS.SetData(S.Script, S.dMon, S.dTue, S.dWed, S.dThu, S.dFri, S.mJan, S.mFeb, S.mMar, S.mApr, S.mMay, S.mJun, S.mJul, S.mAug, S.mSep, S.mOct, S.mNov, S.mDec, S.mOn, S.OnDemandDate, S.Other, S.Hold, S.Holder, S.Duplex, S.Perforated, S.Manual, S.DBUserID, S.LogFile, S.ModAndSub, TestMode, S.Status)
            'change subitem to display 
            lstSel.Items.Add(TS)
            lstSel.Refresh()
            TS.SetPending()
            S.ListView.Items.Remove(S)
        Next
    End Sub

    Private Sub lblClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblClose.Click
        'remove userids from database
        Dim Conn As New SqlClient.SqlConnection
        Dim Comm As New SqlClient.SqlCommand
        If TestMode Then
            Conn.ConnectionString = TestSQLConnStr
        Else
            Conn.ConnectionString = LiveSQLConnStr
        End If
        Comm.Connection = Conn
        Conn.Open()
        'clear all user IDs for this PC
        Comm.CommandText = "DELETE FROM UserIDsInUse WHERE PCName = '" & Environment.MachineName & "'"
        Comm.ExecuteNonQuery()
        End
    End Sub
    Private Sub lblB1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblB1.Click
        RunSetting(lblB1)
    End Sub
    Private Sub lblB2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblB2.Click
        RunSetting(lblB2)
    End Sub
    Private Sub lblB3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblB3.Click
        RunSetting(lblB3)
    End Sub
    Private Sub lblB4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblB4.Click
        RunSetting(lblB4)
    End Sub
    Private Sub lblB5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblB5.Click
        RunSetting(lblB5)
    End Sub
    Sub RunSetting(ByRef lbl As Label)
        'batch run setting
        If lbl.Text = "Run All" Then
            lbl.Text = "Pause"
            lbl.Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\SmallShellVertical.gif")
        ElseIf lbl.Text = "Pause" Then
            lbl.Text = "Que"
            lbl.Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\SmallBulletsVertical.gif")
        ElseIf lbl.Text = "Que" Then
            lbl.Text = "Run All"
            lbl.Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\fissionbomb.gif")
        End If
    End Sub
    Private Sub lstSel_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSel.DoubleClick
        Dim SI As New ScriptItem
        If lstSel.SelectedItems.Count < 1 Then Exit Sub
        If MsgBox("Did you run this script manually?", MsgBoxStyle.YesNo, "Master Batch") = MsgBoxResult.Yes Then
            SI = lstSel.SelectedItems(0)
            If Dir(SI.LogFile) <> "" Or SI.Manual Then 'doesn't matter if log file exists as long as script is marked as manual run
                If Dir(SI.LogFile) <> "" Then Kill(SI.LogFile)
                SI.SetComplete()
                lstSel.Items.Remove(SI)
            Else
                MsgBox("The log file was not found. The Script may not have completed successfully. Contact System support with questions.")
            End If
        End If
    End Sub
    Private Sub lblPause_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblPause.Click
        Cursor.Current = System.Windows.Forms.Cursors.No
        If lblPause.Text = "Pause" Then
            lblPause.Text = "No Pause"
        ElseIf lblPause.Text = "No Pause" Then
            lblPause.Text = "Pause"
        End If
    End Sub
    Private Sub lstIds_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstIds.DoubleClick
        RemoveUser(lstIds.SelectedItems(0))
    End Sub
    Private Sub lstSel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstSel.SelectedIndexChanged
        If lstSel.SelectedIndices.Count > 0 Then
            Dim S As New ScriptItem
            Dim str As String
            S = CType(lstSel.Items.Item(lstSel.SelectedIndices(0)), ScriptItem)
            If S.Perforated Then
                str = "Perforated"
            ElseIf S.Duplex Then
                str = "Duplex"
            End If
            If S.Hold Then
                str = str & vbCrLf & "Hold for " & S.Holder
            End If
            If S.Manual Then
                str = str & vbCrLf & "Run Manually"
            End If
            If S.Other <> "" Then
                str = str & vbCrLf & S.Other
            End If

            'lblinfo.Text = S.Script & vbCrLf & str
        Else
            'lblinfo.Text = ""
        End If
    End Sub
#End Region

#Region " Drag Events "

    Private Sub lstSel_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstSel.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub lstSel_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lstSel.ItemDrag
        lstSel.DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub lstSel_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstSel.DragDrop
        Dim S As New ScriptItem 'Item recieved
        Dim TS As New ScriptItem 'Item to add to batch box
        If e.Data.GetDataPresent("MasterBatch.ScriptItem", False) Then
            S = CType(e.Data.GetData("MasterBatch.ScriptItem"), ScriptItem)
            If S.ListView.Name = "lstSel" Then Exit Sub
            TS.SetData(S.Script, S.dMon, S.dTue, S.dWed, S.dThu, S.dFri, S.mJan, S.mFeb, S.mMar, S.mApr, S.mMay, S.mJun, S.mJul, S.mAug, S.mSep, S.mOct, S.mNov, S.mDec, S.mOn, S.OnDemandDate, S.Other, S.Hold, S.Holder, S.Duplex, S.Perforated, S.Manual, S.DBUserID, S.LogFile, S.ModAndSub, TestMode, S.Status)
            'change subitem to display 
            lstSel.Items.Add(TS)
            lstSel.Refresh()
            TS.SetPending()
            S.ListView.Items.Remove(S)
        End If
    End Sub

    Private Sub lstB1_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB1.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub lstB1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB1.DragDrop
        Dim S As New ScriptItem 'Item recieved
        Dim TS As New ScriptItem 'Item to add to batch box
        If e.Data.GetDataPresent("MasterBatch.ScriptItem", False) Then
            S = CType(e.Data.GetData("MasterBatch.ScriptItem"), ScriptItem)
            If S.Hold Or S.Manual Or S.Status = "Debug" Then
                If S.Hold Then
                    MsgBox("This Script is on hold for " & S.Holder)
                ElseIf S.Manual Then
                    MsgBox("This script needs to be run manually.")
                ElseIf S.Status = "Debug" Then
                    MsgBox("This script previously debugged. Please run manually.")
                End If
                Exit Sub
            End If
            If S.ListView.Name = "lstB1" Then Exit Sub
            'make new ScriptItem to pass to list view
            TS.SetData(S.Script, S.dMon, S.dTue, S.dWed, S.dThu, S.dFri, S.mJan, S.mFeb, S.mMar, S.mApr, S.mMay, S.mJun, S.mJul, S.mAug, S.mSep, S.mOct, S.mNov, S.mDec, S.mOn, S.OnDemandDate, S.Other, S.Hold, S.Holder, S.Duplex, S.Perforated, S.Manual, S.DBUserID, S.LogFile, S.ModAndSub, TestMode, S.Status)
            lstB1.Items.Add(TS)
            TS.SetReady() 'this call must be after the listview is set
            lstB1.Refresh()
            S.ListView.Items.Remove(S)
        End If
    End Sub

    Private Sub lstB2_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB2.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub lstB3_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB3.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub lstB4_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB4.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub lstB5_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB5.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub lstB2_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB2.DragDrop
        Dim S As New ScriptItem 'Item recieved
        Dim TS As New ScriptItem 'Item to add to batch box
        If e.Data.GetDataPresent("MasterBatch.ScriptItem", False) Then
            S = CType(e.Data.GetData("MasterBatch.ScriptItem"), ScriptItem)
            If S.Hold Or S.Manual Or S.Status = "Debug" Then
                If S.Hold Then
                    MsgBox("This Script is on hold for " & S.Holder)
                ElseIf S.Manual Then
                    MsgBox("This script needs to be run manually.")
                ElseIf S.Status = "Debug" Then
                    MsgBox("This script previously debugged. Please run manually.")
                End If
                Exit Sub
            End If
            If S.ListView.Name = "lstB2" Then Exit Sub
            TS.SetData(S.Script, S.dMon, S.dTue, S.dWed, S.dThu, S.dFri, S.mJan, S.mFeb, S.mMar, S.mApr, S.mMay, S.mJun, S.mJul, S.mAug, S.mSep, S.mOct, S.mNov, S.mDec, S.mOn, S.OnDemandDate, S.Other, S.Hold, S.Holder, S.Duplex, S.Perforated, S.Manual, S.DBUserID, S.LogFile, S.ModAndSub, TestMode, S.Status)
            lstB2.Items.Add(TS)
            TS.SetReady() 'this call must be after the listview is set
            lstB2.Refresh()
            S.ListView.Items.Remove(S)
        End If
    End Sub

    Private Sub lstB3_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB3.DragDrop
        Dim S As New ScriptItem 'Item recieved
        Dim TS As New ScriptItem 'Item to add to batch box
        If e.Data.GetDataPresent("MasterBatch.ScriptItem", False) Then
            S = CType(e.Data.GetData("MasterBatch.ScriptItem"), ScriptItem)
            If S.Hold Or S.Manual Or S.Status = "Debug" Then
                If S.Hold Then
                    MsgBox("This Script is on hold for " & S.Holder)
                ElseIf S.Manual Then
                    MsgBox("This script needs to be run manually.")
                ElseIf S.Status = "Debug" Then
                    MsgBox("This script previously debugged. Please run manually.")
                End If
                Exit Sub
            End If
            If S.ListView.Name = "lstB3" Then Exit Sub
            TS.SetData(S.Script, S.dMon, S.dTue, S.dWed, S.dThu, S.dFri, S.mJan, S.mFeb, S.mMar, S.mApr, S.mMay, S.mJun, S.mJul, S.mAug, S.mSep, S.mOct, S.mNov, S.mDec, S.mOn, S.OnDemandDate, S.Other, S.Hold, S.Holder, S.Duplex, S.Perforated, S.Manual, S.DBUserID, S.LogFile, S.ModAndSub, TestMode, S.Status)
            lstB3.Items.Add(TS)
            TS.SetReady() 'this call must be after the listview is set
            lstB3.Refresh()
            S.ListView.Items.Remove(S)
        End If
    End Sub

    Private Sub lstB4_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB4.DragDrop
        Dim S As New ScriptItem 'Item recieved
        Dim TS As New ScriptItem 'Item to add to batch box
        If e.Data.GetDataPresent("MasterBatch.ScriptItem", False) Then
            S = CType(e.Data.GetData("MasterBatch.ScriptItem"), ScriptItem)
            If S.Hold Or S.Manual Or S.Status = "Debug" Then
                If S.Hold Then
                    MsgBox("This Script is on hold for " & S.Holder)
                ElseIf S.Manual Then
                    MsgBox("This script needs to be run manually.")
                ElseIf S.Status = "Debug" Then
                    MsgBox("This script previously debugged. Please run manually.")
                End If
                Exit Sub
            End If
            If S.ListView.Name = "lstB4" Then Exit Sub
            TS.SetData(S.Script, S.dMon, S.dTue, S.dWed, S.dThu, S.dFri, S.mJan, S.mFeb, S.mMar, S.mApr, S.mMay, S.mJun, S.mJul, S.mAug, S.mSep, S.mOct, S.mNov, S.mDec, S.mOn, S.OnDemandDate, S.Other, S.Hold, S.Holder, S.Duplex, S.Perforated, S.Manual, S.DBUserID, S.LogFile, S.ModAndSub, TestMode, S.Status)
            lstB4.Items.Add(TS)
            TS.SetReady() 'this call must be after the listview is set
            lstB4.Refresh()
            S.ListView.Items.Remove(S)
        End If
    End Sub

    Private Sub lstB5_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lstB5.DragDrop
        Dim S As New ScriptItem 'Item recieved
        Dim TS As New ScriptItem 'Item to add to batch box
        If e.Data.GetDataPresent("MasterBatch.ScriptItem", False) Then
            S = CType(e.Data.GetData("MasterBatch.ScriptItem"), ScriptItem)
            If S.Hold Or S.Manual Or S.Status = "Debug" Then
                If S.Hold Then
                    MsgBox("This Script is on hold for " & S.Holder)
                ElseIf S.Manual Then
                    MsgBox("This script needs to be run manually.")
                ElseIf S.Status = "Debug" Then
                    MsgBox("This script previously debugged. Please run manually.")
                End If
                Exit Sub
            End If
            If S.ListView.Name = "lstB5" Then Exit Sub
            TS.SetData(S.Script, S.dMon, S.dTue, S.dWed, S.dThu, S.dFri, S.mJan, S.mFeb, S.mMar, S.mApr, S.mMay, S.mJun, S.mJul, S.mAug, S.mSep, S.mOct, S.mNov, S.mDec, S.mOn, S.OnDemandDate, S.Other, S.Hold, S.Holder, S.Duplex, S.Perforated, S.Manual, S.DBUserID, S.LogFile, S.ModAndSub, TestMode, S.Status)
            lstB5.Items.Add(TS)
            TS.SetReady() 'this call must be after the listview is set
            lstB5.Refresh()
            S.ListView.Items.Remove(S)
        End If
    End Sub

    Private Sub lstB1_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lstB1.ItemDrag
        lstB1.DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub lstB2_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lstB2.ItemDrag
        lstB2.DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub lstB3_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lstB3.ItemDrag
        lstB3.DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub lstB4_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lstB4.ItemDrag
        lstB4.DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub lstB5_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lstB5.ItemDrag
        lstB5.DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub
#End Region

    Sub EnableScriptsAndBatchs(ByVal V As Boolean)
        lstB1.Visible = V
        lstB2.Visible = V
        lstB3.Visible = V
        lstB4.Visible = V
        lstB5.Visible = V
        lblB1.Enabled = V
        lblB2.Enabled = V
        lblB3.Enabled = V
        lblB4.Enabled = V
        lblB5.Enabled = V

        lblPause.Enabled = V
        lblRun.Enabled = V
        lblRefresh.Enabled = V
    End Sub

#Region "Batch Threads"
    'main sub/thread for processing
    Private Sub MainScriptProc()
        StartBatch(1)
        If RunBatch(lstB1, lblB1) Then
            FinishBatch(1)
            If lblPause.Text = "Pause" Then If MsgBox("On your command Sir.", MsgBoxStyle.OKCancel) = MsgBoxResult.Cancel Then Exit Sub
            StartBatch(2)
            If RunBatch(lstB2, lblB2) Then
                FinishBatch(2)
                If lblPause.Text = "Pause" Then If MsgBox("On your command Sir.", MsgBoxStyle.OKCancel) = MsgBoxResult.Cancel Then Exit Sub
                StartBatch(3)
                If RunBatch(lstB3, lblB3) Then
                    FinishBatch(3)
                    If lblPause.Text = "Pause" Then If MsgBox("On your command Sir.", MsgBoxStyle.OKCancel) = MsgBoxResult.Cancel Then Exit Sub
                    StartBatch(4)
                    If RunBatch(lstB4, lblB4) Then
                        FinishBatch(4)
                        If lblPause.Text = "Pause" Then If MsgBox("On your command Sir.", MsgBoxStyle.OKCancel) = MsgBoxResult.Cancel Then Exit Sub
                        StartBatch(5)
                        If RunBatch(lstB5, lblB5) Then
                            FinishBatch(5)
                            MsgBox("Peace at Last! (Process complete!)")
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Sub StartBatch(ByVal B As Integer)
        'where B is the current batch number
        Tnk.TargetBatch = B
        lblbatchNum.Text = Tnk.TargetBatch
        Tnk.DrawTank()
        Me.BackgroundImage = Tnk.BackBmp
        Me.Refresh()
        DrawBang()
        lstIds.Enabled = False
    End Sub
    Sub FinishBatch(ByVal B As Integer)
        If B = 1 Then Tnk.Batch1Done = True
        If B = 2 Then Tnk.Batch2Done = True
        If B = 3 Then Tnk.Batch3Done = True
        If B = 4 Then Tnk.Batch4Done = True
        If B = 5 Then Tnk.Batch5Done = True
        Tnk.Batch1Done = True
        If Mute = False Then Sound.PlayWaveResource("explode.wav")
        Tnk.DrawTank()
        Me.BackgroundImage = Tnk.BackBmp
        Me.Refresh()
        If B <> 5 Then TurnRight()
        lstIds.Enabled = True
    End Sub
    Function RunBatch(ByRef BatchLst As ListView, ByRef lblProcessMode As Label) As Boolean
        Dim ReadySIsFound As Boolean
        Dim BatchProc() As ScriptThread
        ReDim BatchProc(0)
        Dim PauseBatchProc As ScriptThread
        Dim SI As ScriptItem
        Dim UID As UserID
        Dim IScripts As Integer
        Dim IUIDs As Integer
        Dim ValidUIDFound As Boolean 'tracks if at least one valid UID is found
        Dim UseableUIDFound As Boolean 'tracks if a usable UID was found
        Dim TS As New TimeSpan(0, 0, 1) 'one second
        Dim TS10Secs As New TimeSpan(0, 0, 10) '10 seconds
        Dim TS5Secs As New TimeSpan(0, 0, 5) '5 seconds
        RunBatch = True 'default is true, "Everything processed fine"
        'disable all controls for batch
        Me.Invoke(New DisableControlDelegate(AddressOf DisableControl), New Object() {BatchLst})
        Me.Invoke(New DisableControlDelegate(AddressOf DisableControl), New Object() {lblProcessMode})
        'check if batch has anything in it.
        If BatchLst.Items.Count = 0 Then
            'enable contorols
            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
            Exit Function
        End If
        IScripts = 0
        'update all scripts in batch to ready
        While IScripts < BatchLst.Items.Count
            SI = CType(BatchLst.Items(IScripts), ScriptItem)
            SI.SetReady()
            IScripts = IScripts + 1
        End While
        'check for pausing
        If lblProcessMode.Text = "Pause" Then
            'pause between each script *********************************************
            While BatchLst.Items.Count <> 0
                'get scripts in ready state
                If BatchLst.Items(0).SubItems(1).Text.ToUpper = "Ready".ToUpper Then
                    'cast into script item
                    SI = CType(BatchLst.Items(0), ScriptItem)
                    IUIDs = 0 'init var for a search
                    ValidUIDFound = False
                    'check if script needs specific user id
                    If SI.DBUserID = "" Then '*******************************************************
                        'if no specific user id needed then search for first availible one
                        While IUIDs < lstIds.Items.Count
                            UID = CType(lstIds.Items(IUIDs), UserID)
                            'check if user id is valid and that it is not currently in use
                            If UID.Status.ToUpper = "Valid".ToUpper Then
                                ValidUIDFound = True 'at least one valid UID was found so looping can continue
                                If UID.BeingUsedByMBS = False Then
                                    UID.BeingUsedByMBS = True 'mark as in use
                                    Exit While 'exit loop for processing
                                End If
                            End If
                            IUIDs = IUIDs + 1
                        End While
                        'check if at least one valid UID was found
                        If ValidUIDFound = False Then
                            MessageBox.Show("None of the user ids are valid.  MBS needs at least one valid user id to process.", "No Valid User IDs", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
                            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
                            Return False 'skip the rest of the batches for revalidation of user ids
                        End If
                    Else
                        'specific user id needed **************************************
                        While IUIDs < lstIds.Items.Count
                            UID = CType(lstIds.Items(IUIDs), UserID)
                            'check if user id that is needed is valid and not currently in use
                            If UID.Status.ToUpper = "Valid".ToUpper And UID.ID.ToUpper = SI.DBUserID.ToUpper Then
                                ValidUIDFound = True 'at least one valid UID was found so looping can continue
                                If UID.BeingUsedByMBS = False Then
                                    UID.BeingUsedByMBS = True 'mark as in use
                                    Exit While 'exit loop for processing
                                End If
                            End If
                            IUIDs = IUIDs + 1
                        End While
                        'check if at least one valid UID was found
                        If ValidUIDFound = False Then
                            MessageBox.Show("The needed user id was not valid.  User id " & SI.DBUserID & " must be entered and validated to continue.", "Needed User ID Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
                            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
                            Return False 'skip the rest of the batches for revalidation of needed user id
                        End If
                    End If
                    PauseBatchProc = New ScriptThread(SI, UID, Me, BatchLst, lstSel)
                    PauseBatchProc.StartProc() 'start script
                    'wait while script is processed
                    Threading.Thread.Sleep(TS)
                    While Threading.Monitor.TryEnter(PauseBatchProc.LockedData()) = False
                        Threading.Thread.Sleep(TS)
                    End While
                    Threading.Monitor.Exit(PauseBatchProc.LockedData()) 'unlock thread logic
                    If MessageBox.Show(SI.Script & " has just run.  Should MBS continue processing?", "Continue Processing?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                        'end processing if the user chooses to
                        Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
                        Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
                        Return False
                    End If
                End If
            End While
        ElseIf lblProcessMode.Text = "Run All" Then
            'BLITZ, mutiple scripts, as fast as you can, using all user ids you can **********************************
            ReadySIsFound = True 'init to get into loop
            While ReadySIsFound
                ReadySIsFound = False 'mark that none have been found yet
                IScripts = 0
                'pause between each script *********************************************
                While IScripts < BatchLst.Items.Count
                    'get scripts in ready state
                    If BatchLst.Items(IScripts).SubItems(1).Text.ToUpper = "Ready".ToUpper Then
                        ReadySIsFound = True 'there is at least one script still in a ready state
                        'cast into script item
                        SI = CType(BatchLst.Items(IScripts), ScriptItem)
                        IUIDs = 0 'init var for a search
                        ValidUIDFound = False
                        UseableUIDFound = False
                        'check if script needs specific user id
                        If SI.DBUserID = "" Then '*******************************************************
                            'if no specific user id needed then search for first availible one
                            While IUIDs < lstIds.Items.Count
                                UID = CType(lstIds.Items(IUIDs), UserID)
                                'check if user id is valid and that it is not currently in use
                                If UID.Status.ToUpper = "Valid".ToUpper Then
                                    ValidUIDFound = True 'at least one valid UID was found so looping can continue
                                    If UID.BeingUsedByMBS = False Then
                                        UID.BeingUsedByMBS = True 'mark as in use
                                        UseableUIDFound = True
                                        Exit While 'exit loop for processing
                                    End If
                                End If
                                IUIDs = IUIDs + 1
                            End While
                            'check if at least one valid UID was found
                            If ValidUIDFound = False Then
                                MessageBox.Show("None of the user ids are valid.  MBS needs at least one valid user id to process.", "No Valid User IDs", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
                                Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
                                Return False 'skip the rest of the batches for revalidation of user ids
                            End If
                        Else
                            'specific user id needed **************************************
                            While IUIDs < lstIds.Items.Count
                                UID = CType(lstIds.Items(IUIDs), UserID)
                                'check if user id that is needed is valid and not currently in use
                                If UID.Status.ToUpper = "Valid".ToUpper And UID.ID.ToUpper = SI.DBUserID.ToUpper Then
                                    ValidUIDFound = True 'at least one valid UID was found so looping can continue
                                    If UID.BeingUsedByMBS = False Then
                                        UID.BeingUsedByMBS = True 'mark as in use
                                        UseableUIDFound = True
                                        Exit While 'exit loop for processing
                                    End If
                                End If
                                IUIDs = IUIDs + 1
                            End While
                            'check if at least one valid UID was found
                            If ValidUIDFound = False Then
                                MessageBox.Show("The needed user id was not valid.  User id " & SI.DBUserID & " must be entered and validated to continue.", "Needed User ID Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
                                Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
                                Return False 'skip the rest of the batches for revalidation of needed user id
                            End If
                        End If
                        'check if usable user ID was found and if not then skip processing for this script and go on to next
                        If UseableUIDFound Then
                            BatchProc(BatchProc.GetUpperBound(0)) = New ScriptThread(SI, UID, Me, BatchLst, lstSel)
                            BatchProc(BatchProc.GetUpperBound(0)).StartProc() 'start script
                            'create another blank for next script
                            ReDim Preserve BatchProc(BatchProc.GetUpperBound(0) + 1)
                            'wait while script is processed
                            Threading.Thread.Sleep(TS)
                        Else
                            Threading.Thread.Sleep(TS10Secs) 'sleep for 10 seconds so not all processing power is used
                        End If
                    End If
                    IScripts = IScripts + 1
                End While
            End While
            'all scripts have been started no the app just needs to wait until they are all done
            IScripts = 0
            'loop through threads until all of completed
            While IScripts < BatchProc.GetUpperBound(0)
                'wait for all scripts to finish
                If Threading.Monitor.TryEnter(BatchProc(IScripts).LockedData) = False Then
                    'at least one thread is still processing 
                    While Threading.Monitor.TryEnter(BatchProc(IScripts).LockedData) = False
                        'continue to check identified thread until it is done
                        Threading.Thread.Sleep(TS5Secs)
                    End While
                    Threading.Monitor.Exit(BatchProc(IScripts).LockedData) 'unlock thread logic
                    IScripts = 0 'start back at the top of the list because one thread was still processing
                Else
                    Threading.Monitor.Exit(BatchProc(IScripts).LockedData) 'unlock thread
                    IScripts = IScripts + 1 'go on to next thread because this one isn't running
                End If
            End While
        Else
            'run with no pause, full speed not a blitz though ******************************************************************
            While BatchLst.Items.Count <> 0
                'get scripts in ready state
                If BatchLst.Items(0).SubItems(1).Text.ToUpper = "Ready".ToUpper Then
                    'cast into script item
                    SI = CType(BatchLst.Items(0), ScriptItem)
                    IUIDs = 0 'init var for a search
                    ValidUIDFound = False
                    'check if script needs specific user id
                    If SI.DBUserID = "" Then '*******************************************************
                        'if no specific user id needed then search for first availible one
                        While IUIDs < lstIds.Items.Count
                            UID = CType(lstIds.Items(IUIDs), UserID)
                            'check if user id is valid and that it is not currently in use
                            If UID.Status.ToUpper = "Valid".ToUpper Then
                                ValidUIDFound = True 'at least one valid UID was found so looping can continue
                                If UID.BeingUsedByMBS = False Then
                                    UID.BeingUsedByMBS = True 'mark as in use
                                    Exit While 'exit loop for processing
                                End If
                            End If
                            IUIDs = IUIDs + 1
                        End While
                        'check if at least one valid UID was found
                        If ValidUIDFound = False Then
                            MessageBox.Show("None of the user ids are valid.  MBS needs at least one valid user id to process.", "No Valid User IDs", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
                            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
                            Return False 'skip the rest of the batches for revalidation of user ids
                        End If
                    Else
                        'specific user id needed **************************************
                        While IUIDs < lstIds.Items.Count
                            UID = CType(lstIds.Items(IUIDs), UserID)
                            'check if user id that is needed is valid and not currently in use
                            If UID.Status.ToUpper = "Valid".ToUpper And UID.ID.ToUpper = SI.DBUserID.ToUpper Then
                                ValidUIDFound = True 'at least one valid UID was found so looping can continue
                                If UID.BeingUsedByMBS = False Then
                                    UID.BeingUsedByMBS = True 'mark as in use
                                    Exit While 'exit loop for processing
                                End If
                            End If
                            IUIDs = IUIDs + 1
                        End While
                        'check if at least one valid UID was found
                        If ValidUIDFound = False Then
                            MessageBox.Show("The needed user id was not valid.  User id " & SI.DBUserID & " must be entered and validated to continue.", "Needed User ID Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
                            Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
                            Return False 'skip the rest of the batches for revalidation of needed user id
                        End If
                    End If
                    PauseBatchProc = New ScriptThread(SI, UID, Me, BatchLst, lstSel)
                    PauseBatchProc.StartProc() 'start script
                    'wait while script is processed
                    Threading.Thread.Sleep(TS)
                    While Threading.Monitor.TryEnter(PauseBatchProc.LockedData()) = False
                        Threading.Thread.Sleep(TS)
                    End While
                    Threading.Monitor.Exit(PauseBatchProc.LockedData()) 'unlock thread logic
                End If
            End While
        End If
        Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {BatchLst})
        Me.Invoke(New EnableControlDelegate(AddressOf EnableControl), New Object() {lblProcessMode})
    End Function
#End Region

    Private Sub frmMasterBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Click
        Dim LeftR As New RectangleF(217, 519, 123, 62)
        Dim RightR As New RectangleF(564, 520, 132, 62)
        Dim LeftTrigR As New RectangleF(353, 542, 17, 27)
        Dim RightTrigR As New RectangleF(532, 540, 17, 27)
        Dim p As New PointF(Me.MousePosition.X - Me.Location.X, Me.MousePosition.Y - Me.Location.Y)

        'left
        If LeftR.Contains(p) Then
            TurnLeft()
        End If
        'left triger
        If LeftTrigR.Contains(p) Then
            If Mute = False Then Sound.PlayWaveResource("cannon6.wav")
            DrawBang()
            Me.Refresh()
        End If
        'Right
        If RightR.Contains(p) Then
            TurnRight()
        End If
        'Right triger
        If RightTrigR.Contains(p) Then
            If Mute = False Then Sound.PlayWaveResource("cannon6.wav")
            DrawBang()
            Me.Refresh()
        End If

    End Sub
    Private Sub frmMasterBatch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Conn As New SqlClient.SqlConnection
        Dim Comm As New SqlClient.SqlCommand
        If TestMode Then Me.Text = Me.Text & "  [TEST]"
        Tnk.Width = Me.Width
        Tnk.Height = Me.Height
        Tnk.TargetBatch = 1
        Tnk.ShowTanks = False
        Tnk.DrawTank()
        Me.BackgroundImage = Tnk.BackBmp
        EnableScriptsAndBatchs(False)
        'draw radar
        If DoDrawPie Is Nothing Then
            DoDrawPie = New Thread(AddressOf DrawPie)
            DoDrawPie.IsBackground = True
            DoDrawPie.Start()
        Else
            If DoDrawPie.ThreadState = ThreadState.Stopped Then
                DoDrawPie = New Thread(AddressOf DrawPie)
                DoDrawPie.IsBackground = True
                DoDrawPie.Start()
            Else
                If InStr(DoDrawPie.ThreadState.ToString, "Suspended") > 0 Then
                    DoDrawPie.Resume()
                End If
            End If
        End If
        If TestMode Then
            Conn.ConnectionString = TestSQLConnStr
        Else
            Conn.ConnectionString = LiveSQLConnStr
        End If
        Comm.Connection = Conn
        Conn.Open()
        'clear all user IDs for this PC
        Comm.CommandText = "DELETE FROM UserIDsInUse WHERE PCName = '" & Environment.MachineName & "'"
        Comm.ExecuteNonQuery()
        'clear all user IDs for dates before today
        Comm.CommandText = "DELETE FROM UserIDsInUse WHERE EntryDate < '" & _selectedTimeDate.Date & "'" '**
        Comm.ExecuteNonQuery()
        If Mute = False Then Sound.PlayWaveResource("metalDoor.WAV")

    End Sub

    
    Private Sub ckbMute_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbMute.CheckedChanged
        If ckbMute.Checked Then
            ckbMute.ForeColor = Color.Lime
            Mute = True
        Else
            ckbMute.ForeColor = Color.Black
            Mute = False
        End If
    End Sub

End Class




