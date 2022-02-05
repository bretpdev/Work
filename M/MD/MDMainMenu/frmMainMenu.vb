Imports System.IO
Imports System.Threading
Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Class frmMainMenu
    Inherits SP.frmGenericFrmWToolBar

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'init session
        SP.UsrInf.FigureOutDB() 'setup DB connection
        SP.Q.GatherLoginInfo()
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
    Friend WithEvents txtAccountID As System.Windows.Forms.TextBox
    Friend WithEvents btnContinue As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnGetTask As System.Windows.Forms.Button
    Friend WithEvents ColorsDlg As System.Windows.Forms.ColorDialog
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnAsk As System.Windows.Forms.Button
    Friend WithEvents btnAuto As System.Windows.Forms.Button
    Friend WithEvents txtAuto As System.Windows.Forms.TextBox
    Friend WithEvents btnTraining As System.Windows.Forms.Button
    Friend WithEvents ckbOverflow As System.Windows.Forms.CheckBox
    Friend WithEvents btnUnexpected As System.Windows.Forms.Button
    Friend WithEvents btnBrightIdea As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnForbearance As System.Windows.Forms.Button
    Friend WithEvents btnWho As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainMenu))
        Me.txtAccountID = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnGetTask = New System.Windows.Forms.Button
        Me.btnContinue = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.ColorsDlg = New System.Windows.Forms.ColorDialog
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAsk = New System.Windows.Forms.Button
        Me.btnAuto = New System.Windows.Forms.Button
        Me.btnTraining = New System.Windows.Forms.Button
        Me.btnUnexpected = New System.Windows.Forms.Button
        Me.btnBrightIdea = New System.Windows.Forms.Button
        Me.btnForbearance = New System.Windows.Forms.Button
        Me.btnWho = New System.Windows.Forms.Button
        Me.txtAuto = New System.Windows.Forms.TextBox
        Me.ckbOverflow = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtAccountID
        '
        Me.txtAccountID.Location = New System.Drawing.Point(104, 136)
        Me.txtAccountID.MaxLength = 10
        Me.txtAccountID.Name = "txtAccountID"
        Me.txtAccountID.Size = New System.Drawing.Size(112, 20)
        Me.txtAccountID.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.txtAccountID, "Enter either an Account Number or Social Security Number")
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(128, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 23)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Maui DUDE"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnGetTask
        '
        Me.btnGetTask.Location = New System.Drawing.Point(104, 168)
        Me.btnGetTask.Name = "btnGetTask"
        Me.btnGetTask.Size = New System.Drawing.Size(112, 24)
        Me.btnGetTask.TabIndex = 1
        Me.btnGetTask.Text = "&Get Next Task"
        Me.ToolTip1.SetToolTip(Me.btnGetTask, "Click here to get the SSN for the next task")
        '
        'btnContinue
        '
        Me.btnContinue.Location = New System.Drawing.Point(104, 200)
        Me.btnContinue.Name = "btnContinue"
        Me.btnContinue.Size = New System.Drawing.Size(112, 23)
        Me.btnContinue.TabIndex = 2
        Me.btnContinue.Text = "&Continue"
        Me.ToolTip1.SetToolTip(Me.btnContinue, "Click here to get the demographic information for the borrower")
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(104, 408)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(112, 23)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "&Exit"
        Me.ToolTip1.SetToolTip(Me.btnExit, "Click here to exit Maui DUDE")
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(128, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(144, 23)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Main Menu"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(80, 120)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(168, 16)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Enter SSN or Account Number"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.PictureBox2)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(304, 112)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(8, 8)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(104, 98)
        Me.PictureBox2.TabIndex = 6
        Me.PictureBox2.TabStop = False
        '
        'btnAsk
        '
        Me.btnAsk.Location = New System.Drawing.Point(104, 264)
        Me.btnAsk.Name = "btnAsk"
        Me.btnAsk.Size = New System.Drawing.Size(112, 23)
        Me.btnAsk.TabIndex = 4
        Me.btnAsk.Text = "&Ask DUDE"
        Me.ToolTip1.SetToolTip(Me.btnAsk, "Click here to access frequently asked questions.")
        '
        'btnAuto
        '
        Me.btnAuto.Location = New System.Drawing.Point(104, 232)
        Me.btnAuto.Name = "btnAuto"
        Me.btnAuto.Size = New System.Drawing.Size(112, 23)
        Me.btnAuto.TabIndex = 3
        Me.btnAuto.Text = "A&uto DUDE"
        Me.ToolTip1.SetToolTip(Me.btnAuto, "Click here to toggle Auto Dude on and off.  Auto DUDE gets the SSN from the fastp" & _
                "ath so you don't have to enter it.")
        '
        'btnTraining
        '
        Me.btnTraining.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTraining.Location = New System.Drawing.Point(104, 296)
        Me.btnTraining.Name = "btnTraining"
        Me.btnTraining.Size = New System.Drawing.Size(112, 23)
        Me.btnTraining.TabIndex = 11
        Me.btnTraining.Text = "Training &Modules"
        Me.ToolTip1.SetToolTip(Me.btnTraining, "Click here to exit Maui DUDE")
        '
        'btnUnexpected
        '
        Me.btnUnexpected.Image = CType(resources.GetObject("btnUnexpected.Image"), System.Drawing.Image)
        Me.btnUnexpected.Location = New System.Drawing.Point(272, 392)
        Me.btnUnexpected.Name = "btnUnexpected"
        Me.btnUnexpected.Size = New System.Drawing.Size(43, 43)
        Me.btnUnexpected.TabIndex = 29
        Me.btnUnexpected.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnUnexpected, "Unexpected Result? Report Maui DUDE Errors Here.")
        '
        'btnBrightIdea
        '
        Me.btnBrightIdea.Image = CType(resources.GetObject("btnBrightIdea.Image"), System.Drawing.Image)
        Me.btnBrightIdea.Location = New System.Drawing.Point(224, 392)
        Me.btnBrightIdea.Name = "btnBrightIdea"
        Me.btnBrightIdea.Size = New System.Drawing.Size(43, 43)
        Me.btnBrightIdea.TabIndex = 28
        Me.btnBrightIdea.TabStop = False
        Me.ToolTip1.SetToolTip(Me.btnBrightIdea, "Got a Bright Idea for Maui DUDE? Click here to Send your good Idea to the Big Kah" & _
                "una")
        '
        'btnForbearance
        '
        Me.btnForbearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForbearance.Location = New System.Drawing.Point(104, 328)
        Me.btnForbearance.Name = "btnForbearance"
        Me.btnForbearance.Size = New System.Drawing.Size(112, 23)
        Me.btnForbearance.TabIndex = 30
        Me.btnForbearance.Text = "Forbearance Script"
        Me.ToolTip1.SetToolTip(Me.btnForbearance, "Click here to exit Maui DUDE")
        '
        'btnWho
        '
        Me.btnWho.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWho.Location = New System.Drawing.Point(104, 360)
        Me.btnWho.Name = "btnWho"
        Me.btnWho.Size = New System.Drawing.Size(112, 23)
        Me.btnWho.TabIndex = 31
        Me.btnWho.Text = "&Who am I"
        Me.ToolTip1.SetToolTip(Me.btnWho, "Click here to exit Maui DUDE")
        '
        'txtAuto
        '
        Me.txtAuto.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtAuto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAuto.ForeColor = System.Drawing.Color.Red
        Me.txtAuto.Location = New System.Drawing.Point(232, 232)
        Me.txtAuto.Name = "txtAuto"
        Me.txtAuto.Size = New System.Drawing.Size(48, 20)
        Me.txtAuto.TabIndex = 9
        Me.txtAuto.TabStop = False
        Me.txtAuto.Text = "Off"
        Me.txtAuto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ckbOverflow
        '
        Me.ckbOverflow.Location = New System.Drawing.Point(224, 136)
        Me.ckbOverflow.Name = "ckbOverflow"
        Me.ckbOverflow.Size = New System.Drawing.Size(72, 24)
        Me.ckbOverflow.TabIndex = 12
        Me.ckbOverflow.TabStop = False
        Me.ckbOverflow.Text = "Overflow"
        '
        'frmMainMenu
        '
        Me.AcceptButton = Me.btnContinue
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(318, 443)
        Me.Controls.Add(Me.btnWho)
        Me.Controls.Add(Me.btnForbearance)
        Me.Controls.Add(Me.btnUnexpected)
        Me.Controls.Add(Me.btnBrightIdea)
        Me.Controls.Add(Me.ckbOverflow)
        Me.Controls.Add(Me.btnTraining)
        Me.Controls.Add(Me.txtAuto)
        Me.Controls.Add(Me.txtAccountID)
        Me.Controls.Add(Me.btnAuto)
        Me.Controls.Add(Me.btnAsk)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnContinue)
        Me.Controls.Add(Me.btnGetTask)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(328, 520)
        Me.Name = "frmMainMenu"
        Me.Text = "Maui DUDE Main Menu"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private FPText As String
    'Declarations of Windows API functions. These are used to be sure that no more than one instance of MauiDUDE is running
    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long


    Private Sub frmMainMenu_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If InStr(SP.Q.RIBM.Caption, "BorrowerServices") Then
            ckbOverflow.Visible = False
        Else
            ckbOverflow.Visible = True
        End If

        GetLP9ASSN()
    End Sub

    Private Sub GetLP9ASSN()
        If (SP.Q.Check4Text(1, 67, "WORKGROUP TASK") And SP.Q.Check4Text(22, 3, "49000")) Or (SP.Q.Check4Text(1, 71, "QUEUE TASK") And SP.RIBM.GetFieldAttributes(4, 22) <> 10) Then
            SP.frmWhoaDUDE.WhoaDUDE("Aloha from Maui DUDE!  I'd love to get the SSN from that task but it's done.  Enter the SSN or account number you want and lets catch a wave!", "Unfinished Task", True)
        ElseIf SP.Q.Check4Text(1, 67, "WORKGROUP TASK") Then
            txtAccountID.Text = SP.Q.GetText(5, 70, 9)
        ElseIf SP.Q.Check4Text(1, 71, "QUEUE TASK") Then
            txtAccountID.Text = SP.Q.GetText(17, 70, 9)
        End If
    End Sub

    Private Function ExecuteList(ByVal CommandName As String, ByVal Param As SqlParameter, Optional ByVal Column As Integer = 0) As List(Of String)
        Dim connectionString As String = String.Format("Data Source={0};Initial Catalog=UDW;Integrated Security=SSPI;", IIf(SP.TestModeBool, "OPSDEV", "UHEAASQLDB"))
        Dim conn As SqlConnection = New SqlConnection(connectionString)
        conn.Open()
        Dim comm As SqlCommand = New SqlCommand(CommandName, conn)
        comm.CommandType = CommandType.StoredProcedure
        comm.Parameters.Add(Param)
        Dim results As List(Of String) = New List(Of String)()
        Dim reader As SqlDataReader = comm.ExecuteReader()
        While reader.Read()
            results.Add(reader(Column))
        End While
        conn.Close()
        Return results
    End Function

    Private Function IsCustomerServiceAccount(ByVal SsnOrAccountNum As String) As Boolean
        Try
            Dim ssn As String = SsnOrAccountNum
            If ssn.Length = 9 Then
                ssn = ExecuteList("[spGetAccountNumberFromSSN]", New SqlParameter("Ssn", ssn))(0)
            End If
            Dim guarantorIds As List(Of String) = ExecuteList("[spMD_GetCallForwardingData]", New SqlParameter("@AccountNumber", ssn), 1)
            Dim idsAreValid As Boolean = False
            If Not guarantorIds.Any() Then
                Return False
            End If
            For Each guarantor As String In guarantorIds
                If guarantor = "000749" Or guarantor = "I00059" Then
                    idsAreValid = True
                End If
            Next
            Return Not idsAreValid
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        Dim BorrLite As New SP.BorrowerLite
        Dim OriginatedFromACP As Boolean
        Dim AccountResolution As Boolean
        Dim AuxServices As Boolean
        Dim Line As String
        Dim UserI As Array
        Dim skipFastPath As Boolean
        Dim Demographics As SP.frmDemographics
        Dim HomePage As MDBSHome.frmBSHomePage
        Dim AXHomePage As MDASHome.frmASHomePage
        skipFastPath = False
        If IsCustomerServiceAccount(txtAccountID.Text) Then
            MessageBox.Show("Please transfer this call to Customer Service.")
            Return
        End If
        'Check for Borrower Services Session
        If (SP.UsrInf.BusinessUnit = "Customer Services" And txtAuto.Text = "On") Or ckbOverflow.Checked Or SP.UsrInf.BusinessUnit = "Auxiliary Services" Then 'use ssn provided by user
            If txtAccountID.TextLength = 9 And Not IsNumeric(txtAccountID.Text) Then
                txtAccountID.Text = SP.ACSTranslation(txtAccountID.Text)
            End If
            If txtAccountID.TextLength >= 9 And IsNumeric(txtAccountID.Text) Then
                SP.FastPath("TX3Z/ATC00" & txtAccountID.Text)
                If SP.Q.Check4Text(23, 2, "01019") = False And SP.Q.Check4Text(23, 2, "03363") = False Then
                    SP.Q.PutText(19, 38, "1", True)
                Else
                    'No ACP BSV Call
                    BorrLite.NoACPBSVCall = True

                End If
            End If
        End If
        'Check for Loan Management Session
        If (SP.UsrInf.BusinessUnit = "Account Resolution" And txtAuto.Text = "On" And ckbOverflow.Checked = False) Then
            AccountResolution = True
        Else
            AccountResolution = False
        End If
        'Check for Aux Services Session
        If (SP.UsrInf.BusinessUnit = "Auxiliary Services" And ckbOverflow.Checked = False) Then
            AuxServices = True
        Else
            AuxServices = False
        End If
        ''check if MauiDUDE is starting from ACP (TC00)
        OriginatedFromACP = BorrLite.CheckStartingFromACP(txtAccountID)
        'get fast path information and populate account ID if Auto DUDE is on
        If OriginatedFromACP = False And txtAuto.Text = "On" And txtAccountID.Text = "" Then
            'OneLINK/LO
            If SP.Q.GetText(1, 2, 1) <> "" Then
                FPText = Replace(SP.Q.GetText(1, 2, 16), " ", "")
                If SP.Q.GetText(1, 2, 1) <> "L" Then FPText = "TX3Z" & FPText
                txtAccountID.Text = SP.Q.GetText(1, 9, 9)
                'Compass TX1J
            ElseIf SP.Q.GetText(1, 5, 4) = "TX1J" Then
                FPText = "TX3Z" & SP.Q.GetText(1, 4, 16)
                txtAccountID.Text = SP.Q.GetText(1, 11, 9)
                'Compass other screens
            ElseIf Not SP.Q.Check4Text(1, 4, " ") And Not SP.Q.Check4Text(1, 4, "_") Then
                FPText = "TX3Z" & SP.Q.GetText(1, 4, 14)
                txtAccountID.Text = SP.Q.GetText(1, 9, 9)
            Else
                FPText = ""
            End If
        Else
            FPText = ""
        End If
        'check if at least 9 numeric characters are in the account id field
        If txtAccountID.TextLength = 9 And Not IsNumeric(txtAccountID.Text) Then
            txtAccountID.Text = SP.ACSTranslation(txtAccountID.Text)
        End If
        If txtAccountID.TextLength >= 9 And IsNumeric(txtAccountID.Text) Then
            'Valid SSN or Account #
            If BorrLite.ConvertAccToSSN(txtAccountID.Text) = False Then Exit Sub 'do SSN conversion from Acct Num
            Me.Hide()
            SP.Processing.Visible = True 'display processing window
            SP.Processing.Refresh() 'the form doesn't get written to the screen without this



            If AuxServices Then
                '//////////////////////////////////////////////////////////////
                '//////////////////////////////////////////////////////////////
                SP.UsrInf.BUHasHomePage = True 'BU has home page
                AXHomePage = New MDASHome.frmASHomePage(BorrLite)
                If AXHomePage.Show() Then
                    Me.Show()
                    txtAccountID.Clear()
                Else
                    Me.Show()
                    txtAccountID.Focus()
                    txtAccountID.SelectAll()
                End If
                '//////////////////////////////////////////////////////////////
                '//////////////////////////////////////////////////////////////
            ElseIf OriginatedFromACP Then
                '//////////////////////////////////////////////////////////////
                '//////////////////////////////////////////////////////////////
                SP.UsrInf.BUHasHomePage = True 'BU has home page
                HomePage = New MDBSHome.frmBSHomePage(BorrLite)
                If HomePage.Show() Then
                    Me.Show()
                    txtAccountID.Clear()
                Else
                    Me.Show()
                    txtAccountID.Focus()
                    txtAccountID.SelectAll()
                End If
                '//////////////////////////////////////////////////////////////
                '//////////////////////////////////////////////////////////////
            ElseIf BorrLite.NoACPBSVCall Then
                '//////////////////////////////////////////////////////////////
                '//////////////////////////////////////////////////////////////
                SP.UsrInf.BUHasHomePage = True 'BU has home page
                HomePage = New MDBSHome.frmBSHomePage(BorrLite)
                If HomePage.Show() Then
                    Me.Show()
                    txtAccountID.Clear()
                Else
                    Me.Show()
                    txtAccountID.Focus()
                    txtAccountID.SelectAll()
                End If
                '//////////////////////////////////////////////////////////////
                '//////////////////////////////////////////////////////////////
            Else
                '//////////////////////////////////////////////////////////////
                '//////////////////////////////////////////////////////////////
                Dim Bor As New SP.Borrower(BorrLite)
                Demographics = New SP.frmDemographics(Bor)
                If Demographics.PopulateFrm(OriginatedFromACP) Then
                    Demographics.ShowDialog(True, False)
                    Me.Show()
                    txtAccountID.Clear()
                Else
                    Me.Show()
                    txtAccountID.Focus()
                    txtAccountID.SelectAll()
                End If
                '//////////////////////////////////////////////////////////////
                '//////////////////////////////////////////////////////////////
            End If
            If SP.Processing.Visible = True Then 'make the processing form invisible if it is visible
                SP.Processing.Visible = False
            End If
        Else
            'Invalid SSN or Account Number
            SP.frmWhoaDUDE.WhoaDUDE("That was totally knarly, but DUDE needs a valid SSN or account number from OneLINK or a nine character ACS keyline - hold the numbers.", "Knarly DUDE", True)
        End If
        If skipFastPath = False Then
            'do return to ACP functionality if Maui DUDE started from ACP
            If Not OriginatedFromACP Then 'ACP functionality
                'Auto DUDE
                If txtAuto.Text = "On" And FPText <> "" Then
                    SP.Q.FastPath(FPText)
                    'favorite screen
                ElseIf Dir$("T:\userinfo.txt") <> "" Then
                    FileOpen(1, "T:\UserInfo.txt", OpenMode.Input, OpenAccess.Read)
                    Line = LineInput(1)
                    UserI = Line.Split(", ")
                    If UserI(5) <> "" Then
                        SP.Q.FastPath(Replace(UserI(5), """", "")) 'remove quotes
                    End If
                    FileClose(1)
                End If
            End If
        End If
        SP.Hit("Home") 'be sure the user doesn't happen to change data on screen
        'blank out text file
        FileOpen(1, "T:\TempDemoUpdate.txt", OpenMode.Output)
        FileClose(1)
        'clear out DUDE's LastWords
        If Dir("T:\MauiDUDE_LastWords.txt") <> "" Then Kill("T:\MauiDUDE_LastWords.txt")
        If Dir("T:\MauiDUDE_LastWords.txt") <> "" Then Kill("T:\MauiDUDE_LastWords.txt")

    End Sub

    Private Sub btnGetTask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetTask.Click
        If (SP.Q.Check4Text(1, 67, "WORKGROUP TASK") And SP.Q.Check4Text(22, 3, "49000") = False) Or (SP.Q.Check4Text(1, 71, "QUEUE TASK") And SP.Q.RIBM.GetFieldAttributes(4, 22) = 10) Then
            SP.frmWhoaDUDE.WhoaDUDE("Whoa DUDE!  It doesn't look like you've wrapped up this task.  Ya gotta be all done ta get the next task.", "Gotta Finish The Task", True)
        ElseIf SP.Q.Check4Text(1, 67, "WORKGROUP TASK") Then
            GetNextTask(5, 70)
        ElseIf SP.Q.Check4Text(1, 71, "QUEUE TASK") Then
            GetNextTask(17, 70)
        Else
            SP.frmWhoaDUDE.WhoaDUDE("Whoa DUDE!  Ya gotta, like, be on LP9A ta get the next task.", "Wrong Screen", True)
        End If
    End Sub

    Private Sub GetNextTask(ByVal row As Integer, ByVal col As Integer)
        Dim PrevSSN As String

        PrevSSN = SP.Q.GetText(row, col, 9)
        SP.Q.Hit("F8")
        If SP.Q.Check4Text(22, 3, "46004") Then
            SP.frmWhoaDUDE.WhoaDUDE("Whoa DUDE!  Stick a fork in it, this queue's done.", "Stick a Fork in It", True)
            Exit Sub
        End If
        txtAccountID.Text = SP.Q.GetText(row, col, 9)
        If PrevSSN = txtAccountID.Text Then SP.frmWhoaDUDE.WhoaDUDE("Whoa DUDE!  Better check this out.  The SSN didn't change.", "SSN didn't change", True)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        CloseSession()
    End Sub

    Private Sub frmMainMenu_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        CloseSession()
    End Sub

    Sub ActivatePrevInstance(ByVal argStrAppToFind As String)
        Dim PrevHndl As Long
        Dim result As Long
        'Variable to hold individual Process.
        Dim objProcess As New Process
        'Collection of all the Processes running on local machine
        Dim objProcesses() As Process
        'Get all processes into the collection
        objProcesses = Process.GetProcesses()
        For Each objProcess In objProcesses
            'Check and exit if we have SMS running already
            If UCase(objProcess.MainWindowTitle) = UCase(argStrAppToFind) Then
                SP.frmWipeOut.WipeOut("Maui DUDE is already running.", "Maui DUDE already open", True)
                PrevHndl = objProcess.MainWindowHandle.ToInt32()
                Exit For
            End If
        Next
        'If no previous instance found exit the application.
        If PrevHndl = 0 Then Exit Sub
        'If previous instance found.
        result = OpenIcon(PrevHndl) 'Restore the program.
        result = SetForegroundWindow(PrevHndl) 'Activate the application.
        'End the current instance of the application.
        End
    End Sub

    'toggle auto DUDE on and off
    Private Sub btnAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuto.Click
        If txtAuto.Text = "On" Then
            txtAuto.Text = "Off"
            txtAuto.ForeColor = System.Drawing.Color.Red
        Else
            txtAuto.Text = "On"
            txtAuto.ForeColor = System.Drawing.Color.Green
        End If
        txtAccountID.Focus()
    End Sub

    Function CloseSession() As Boolean
        Try
            If SP.frmYesNo.YesNo("Do you want Maui DUDE and the session to become pupu for a mano?") Then
                SP.Q.RIBM.exit()
                Dim MyPro As Process()
                Dim Pro As Process
                MyPro = Process.GetProcessesByName("R8win")
                For Each Pro In MyPro
                    If Pro.MainWindowTitle = Me.Text And Pro.ProcessName() = "R8win" Then
                        Pro.WaitForExit(3000)
                        Pro.Kill()
                        Pro.WaitForExit(1000)
                        If Pro.HasExited = False Then
                            MsgBox("There was an error closing the session.", MsgBoxStyle.Critical)
                        End If
                        Exit For
                    End If
                Next
                'SP.Q.RIBM.exit()

                End
            Else
                Exit Function
                'MsgBox(SP.Q.ConnRIBMCaption)
            End If
        Catch ex As Exception
            Dim MyPro As Process()
            Dim Pro As Process
            MyPro = Process.GetProcessesByName("R8win")
            For Each Pro In MyPro
                If Pro.MainWindowTitle = Me.Text And Pro.ProcessName() = "R8win" Then
                    Pro.WaitForExit(3000)
                    Pro.Kill()
                    Pro.WaitForExit(1000)
                    If Pro.HasExited = False Then
                        MsgBox("There was an error closing the session.", MsgBoxStyle.Critical)
                    End If
                    Exit For
                End If
            Next
            End
        End Try
    End Function

    Private Sub btnTraining_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTraining.Click
        Dim T As Thread = New Thread(AddressOf MyBase.runPP)
        T.Start()
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
    End Sub


    Sub WaitForSession(ByVal Path As String)
        Dim OpeningFound As Boolean = False
        Dim CLS() As String
        Dim Obj As Object
        Do
            Try
                Obj = GetObject("RIBM")
                CLS = Split(Replace(Obj.CommandLineSwitches, """", ""), "\")
                Thread.CurrentThread.Sleep(500)
                Exit Do
            Catch ex As Exception
            End Try
        Loop
    End Sub

    Private Sub MenuAskDUDE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SP.DisplayAskDude()
    End Sub

    Private Sub btnForbearance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForbearance.Click
        SP.frmUnderConstruct.ShowUnderConstruct()
    End Sub

    Private Sub btnWho_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWho.Click
        Dim who As New SP.frmWhoAmI
        who.ShowDialog()
        txtAccountID.Text = who.SSN
        If txtAccountID.Text <> "" Then
            btnContinue.PerformClick()
        End If
    End Sub

    Private Sub btnAsk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsk.Click
        SP.DisplayAskDude()
    End Sub
End Class
