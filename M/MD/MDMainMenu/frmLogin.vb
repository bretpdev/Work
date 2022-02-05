Imports System.Data.SqlClient
Imports System.Diagnostics
Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings
Imports Q

Public Class frmLogin
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'create user object
        SP.UsrInf = New SP.UserInfo
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbBU As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbPW As System.Windows.Forms.TextBox
    Friend WithEvents tbUserID As System.Windows.Forms.TextBox
    Friend WithEvents gbSessionAndBU As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents rbTest As System.Windows.Forms.RadioButton
    Friend WithEvents rbLive As System.Windows.Forms.RadioButton
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogin))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbPW = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbUserID = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.cbBU = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.gbSessionAndBU = New System.Windows.Forms.GroupBox
        Me.rbTest = New System.Windows.Forms.RadioButton
        Me.rbLive = New System.Windows.Forms.RadioButton
        Me.btnLogin = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.gbSessionAndBU.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.tbPW)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.tbUserID)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 88)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(288, 64)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "User Information"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(96, 20)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(28, 16)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "UT0"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Password:"
        '
        'tbPW
        '
        Me.tbPW.Location = New System.Drawing.Point(96, 36)
        Me.tbPW.MaxLength = 8
        Me.tbPW.Name = "tbPW"
        Me.tbPW.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tbPW.Size = New System.Drawing.Size(184, 20)
        Me.tbPW.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "User ID:"
        '
        'tbUserID
        '
        Me.tbUserID.Location = New System.Drawing.Point(124, 16)
        Me.tbUserID.MaxLength = 4
        Me.tbUserID.Name = "tbUserID"
        Me.tbUserID.Size = New System.Drawing.Size(156, 20)
        Me.tbUserID.TabIndex = 0
        Me.tbUserID.Text = "0"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 16)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Session Type:"
        '
        'cbBU
        '
        Me.cbBU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBU.Enabled = False
        Me.cbBU.Location = New System.Drawing.Point(96, 44)
        Me.cbBU.Name = "cbBU"
        Me.cbBU.Size = New System.Drawing.Size(184, 21)
        Me.cbBU.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 16)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Business Unit:"
        '
        'gbSessionAndBU
        '
        Me.gbSessionAndBU.Controls.Add(Me.rbTest)
        Me.gbSessionAndBU.Controls.Add(Me.rbLive)
        Me.gbSessionAndBU.Controls.Add(Me.Label7)
        Me.gbSessionAndBU.Controls.Add(Me.cbBU)
        Me.gbSessionAndBU.Controls.Add(Me.Label6)
        Me.gbSessionAndBU.Location = New System.Drawing.Point(8, 8)
        Me.gbSessionAndBU.Name = "gbSessionAndBU"
        Me.gbSessionAndBU.Size = New System.Drawing.Size(288, 72)
        Me.gbSessionAndBU.TabIndex = 0
        Me.gbSessionAndBU.TabStop = False
        Me.gbSessionAndBU.Text = "Session And Business Unit:"
        '
        'rbTest
        '
        Me.rbTest.AutoSize = True
        Me.rbTest.Location = New System.Drawing.Point(194, 19)
        Me.rbTest.Name = "rbTest"
        Me.rbTest.Size = New System.Drawing.Size(46, 17)
        Me.rbTest.TabIndex = 15
        Me.rbTest.Text = "Test"
        Me.rbTest.UseVisualStyleBackColor = True
        '
        'rbLive
        '
        Me.rbLive.AutoSize = True
        Me.rbLive.Location = New System.Drawing.Point(99, 20)
        Me.rbLive.Name = "rbLive"
        Me.rbLive.Size = New System.Drawing.Size(45, 17)
        Me.rbLive.TabIndex = 14
        Me.rbLive.Text = "Live"
        Me.rbLive.UseVisualStyleBackColor = True
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(72, 168)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(75, 23)
        Me.btnLogin.TabIndex = 2
        Me.btnLogin.Text = "Login"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(160, 168)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        '
        'frmLogin
        '
        Me.AcceptButton = Me.btnLogin
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(304, 205)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.gbSessionAndBU)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLogin"
        Me.Text = "Maui DUDE Login"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gbSessionAndBU.ResumeLayout(False)
        Me.gbSessionAndBU.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Const TestSessionDir As String = "X:\PADU\Test Sessions\"
    Const LiveSessionDir As String = "X:\Sessions\"
    Private Conn As SqlConnection
    Private MM As Form

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        End
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim Comm As SqlCommand
        Dim RefSession As String
        'be sure that needed data is provided
        If rbTest.Checked = False And rbLive.Checked = False Or cbBU.SelectedIndex = -1 Or tbUserID.TextLength <> 4 Or tbPW.TextLength <> 8 Then
            SP.frmWhoaDUDE.WhoaDUDE("We'll start surfin when you provide everything we need.", "We Need All Equipment To Surf")
            Exit Sub
        End If
        'if user selected to log into test, confirm with the user they wanted to log into test.
        If rbTest.Checked Then
            If SP.frmYesNo.YesNo("You have chosen to log into Maui DUDE and the systems in test.  Is this really what you want to do?") = False Then
                Exit Sub 'exit sub and allow the user to cahnge login info if the user didn't want to log into test
            End If
        End If
        'get session from DB
        Comm = New SqlCommand(String.Format("SELECT ReflectionSession FROM BUSessionXRef WHERE BusinessUnit = '{0}'", cbBU.SelectedItem.ToString), Conn)
        Conn.Open()
        RefSession = Comm.ExecuteScalar().ToString
        Conn.Close()
        'if BU has totally moved to MD then open a generic session and link code to it else open BUs actual session
        If cbBU.SelectedItem = "Account Resolution" Then
            'open generic session
            OpenExistingSession(RefSession)
        Else
            'open existing session
            OpenExistingSession(RefSession)
        End If
        'log in to system
        If LoginSuccessfully() = False Then
            SP.RIBM.Exit()  'close session
        Else
            'note userid for later use
            SP.UsrInf.Userid = "UT0" + tbUserID.Text
            'set test mode value
            SP.TestModeBool = rbTest.Checked
            'set BU value
            SP.UsrInf.BusinessUnit = cbBU.SelectedItem
            'change caption of session
            If cbBU.SelectedItem = "Account Resolution" Then
                SP.RIBM.caption = "Maui DUDE - " + cbBU.SelectedItem + " - MD #" + SP.UsrInf.MDSeqID
            Else
                Dim CLS() As String
                CLS = Split(Replace(SP.RIBM.CommandLineSwitches, """", ""), "\")
                SP.RIBM.SaveChanges = 0 'default to not save settings
                SP.RIBM.Caption = CLS(CLS.GetUpperBound(0)).Replace(".rsf", "") & " (MD #" & SP.UsrInf.MDSeqID & ")" 'change caption
            End If
            If cbBU.SelectedItem = "Account Resolution" Then
                Me.Hide()
                MDLMHome.frmLMHomePage.LMBinHomePageProcCoord(cbBU.SelectedItem)
            Else
                MM = New frmMainMenu
                Me.Hide()
                MM.Show()
            End If
        End If
    End Sub

    Private Sub OpenExistingSession(ByVal RefSession As String)
        Dim Pro As New System.Diagnostics.Process
        Dim TempSession As Object
        If rbTest.Checked Then
            RefSession = RefSession.Replace(".", "Tst.") 'add Tst to end of file name if needed
            Pro.StartInfo.FileName = TestSessionDir + RefSession
        Else
            Pro.StartInfo.FileName = LiveSessionDir + RefSession
        End If
        Pro.Start()
        'wait until session is ready to handle commands
        Threading.Thread.Sleep(New TimeSpan(0, 0, 3))
        While True
            Try
                TempSession = GetObject("RIBM")
                Exit While
            Catch ex As Exception
                'sleep for two seconds and try again
                Threading.Thread.Sleep(New TimeSpan(0, 0, 2))
            End Try
        End While
        'End If
        SP.UsrInf.MDSeqID = CalculateNextMDSeqID()
        InitSession()
        SP.RIBM.BDTIgnoreScrollLock = True
        SP.RIBM.Visible = True
    End Sub

    Private Sub InitSession()
        Try

            SP.RIBM = GetObject("RIBM")
            If SP.RIBM Is Nothing Then
                Throw New Exception
            Else
                SP.RIBM.OLEServerName = "RIBMMD" & SP.UsrInf.MDSeqID 'change server name so the session isn't accessed again by another instance of MD
            End If
        Catch ex As Exception
            SP.frmWipeOut.WipeOut("The Reflection Session must have been shutdown.  Please shutdown Maui DUDE and tray again.", "Reflection Not Open", True)
            End
        End Try
    End Sub

    Public Function CalculateNextMDSeqID() As String
        Dim SearchInt As Integer
        Dim OpeningFound As Boolean = False
        Dim TempRIBM As Object
        'search for opening in sequence
        While True
            Try
                SearchInt += 1
                If SearchInt = 17 Then
                    MsgBox("Maui DUDE wasn't able to find a Reflection session to connect to.  Please start up a new Reflection session for Maui DUDE to connect to.")
                    End 'end app
                End If
                TempRIBM = GetObject("RIBMMD" & SearchInt)
            Catch ex As Exception
                Return SearchInt.ToString()
            End Try
        End While
    End Function

    Private Function WaitForText(ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal seconds As Integer) As Boolean
        Dim milliseconds As Integer = seconds * 1000
        Dim s As New Stopwatch()
        s.Start()
        While (s.ElapsedMilliseconds <= milliseconds)
            If (SP.Check4Text(row, column, text)) Then
                Return True
            End If
        End While
        Return False
    End Function

    'try and login
    Private Function LoginSuccessfully() As Boolean
        'wait for the logon screen to be displayed
        'SP.RIBM.WaitForDisplayString(">", "0:2:00", 16, 10)
        If Not (WaitForText(16, 2, "LOGON", 60)) Then
            Return False
        End If
        If rbTest.Checked Then 'test
            SP.PutText(16, 12, "QTOR", True)
            'wait for the greetings screen to be displayed
            'SP.RIBM.WaitForDisplayString("USERID", "0:0:30", 20, 8)
            If Not (WaitForText(20, 8, "USERID", 30)) Then
                Return False
            End If
            SP.PutText(20, 18, "UT0" + tbUserID.Text)
            SP.PutText(20, 40, tbPW.Text, True)
        Else 'live 
            SP.PutText(16, 12, "PHEAA", True)
            'wait for the greetings screen to be displayed
            'SP.RIBM.WaitForDisplayString("USERID", "0:0:30", 20, 8)
            If Not (WaitForText(20, 8, "USERID", 30)) Then
                Return False
            End If
            SP.PutText(20, 18, "UT0" + tbUserID.Text)
            SP.PutText(20, 40, tbPW.Text, True)
        End If
        'check for all of the error situations
        If SP.Check4Text(23, 2, "ON007: INCORRECT PASSWORD ENTERED; PLEASE CORRECT.") Or SP.Check4Text(23, 2, "ON006: USERID NOT DEFINED TO RACF.") Then
            SP.frmWipeOut.WipeOut("Incorrect id or password, please enter your information again.", "Bad ID or Password")
            Return False
        ElseIf SP.Check4Text(23, 2, "ON002: YOUR USERID IS REVOKED; CONTACT CLIENT SUPPORT SERVICES") Then
            frmPasswordResetInfo.WipeOut("Your password has been revoked, please use password reset for a new password and try again.", "Revoked Password", "https://host98.aessuccess.org/PasswordReset.cgi")
            Return False
        ElseIf SP.Check4Text(23, 2, "ON001") Then 'password expired
            Dim PWExp As New frmPasswordExpired
            PWExp.ShowDialog()
            SP.PutText(20, 65, PWExp.GetPW(), True) 'enter new password and hit enter
        ElseIf SP.Check4Text(23, 2, "ON009: SECURITY VIOLATION: YOU ARE ALREADY LOGGED IN AT TERMINAL") Then
            SP.frmWipeOut.WipeOut("You may already be logged on.  Please make sure you are logged off of any Reflection session.  Wait a few minutes and try again.  If the problem persists and you are logged off, please contact the Systems Support Help Desk.", "Already Logged On To System?")
            Return False
        End If
        If rbTest.Checked Then 'test
            'be sure that it got pass the login
            If SP.Check4Text(1, 20, "=== PLEASE SELECT ONE OF THE FOLLOWING ===") = False Then
                SP.frmWipeOut.WipeOut("An error occured while trying to log in.  Please investigate the problem and either try again or contact the Systems Support Help Desk.", "Fatal Error While Logging In")
                End
            End If
            'do the STUP thing
            SP.RIBM.FindText("RS/UT", 3, 5)
            SP.PutText(SP.RIBM.FoundTextRow, SP.RIBM.FoundTextColumn - 2, "X", True)
        Else 'live
            'be sure that it got pass the login
            If SP.Check4Text(7, 11, "YOU ARE LOGGED ON TO THE PHEAA NETWORK.") = False Then
                SP.frmWipeOut.WipeOut("An error occured while trying to log in.  Please investigate the problem and either try again or contact the Systems Support Help Desk.", "Fatal Error While Logging In")
                End
            End If
        End If
        Return True
    End Function

    Private Sub PopulateSessionOptions()
        Dim Comm As SqlCommand
        Dim Reader As SqlDataReader
        If rbLive.Checked Then
            Conn = New SqlConnection("Server=NOCHOUSE;Database=MauiDUDE;Trusted_Connection=True;")
        Else
            Conn = New SqlConnection("Server=OPSDEV;Database=MauiDUDE;Trusted_Connection=True;")
        End If
        cbBU.Items.Clear() 'clear out all list items
        'add list items from DB
        Comm = New SqlCommand("SELECT BusinessUnit FROM BUSessionXRef ORDER BY BusinessUnit", Conn)
        Try
            Conn.Open()
            Reader = Comm.ExecuteReader()
            While Reader.Read()
                cbBU.Items.Add(Reader("BusinessUnit"))
            End While
            Conn.Close()
            cbBU.Enabled = True
        Catch ex As Exception
            SP.frmWipeOut.WipeOut("DUDE, I had a problem communicating with the database.  Please contact Systems Support and try again later.", "Database Communication Error")
            Conn.Close()
            cbBU.Enabled = False
        End Try
    End Sub

    Private Sub rbLive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLive.CheckedChanged
        PopulateSessionOptions()
    End Sub

    Private Sub rbTest_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTest.CheckedChanged
        PopulateSessionOptions()
    End Sub
End Class
