Imports Microsoft.Office.Interop

Public Class Form1
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbUserID As System.Windows.Forms.TextBox
    Friend WithEvents cbSystem As System.Windows.Forms.ComboBox
    Friend WithEvents cbBusinessUnit As System.Windows.Forms.ComboBox
    Friend WithEvents tbOtherUserIDs As System.Windows.Forms.TextBox
    Friend WithEvents lblTDS As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnSendEmail As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tbComments As System.Windows.Forms.TextBox
    Friend WithEvents lblUID As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbAnswer1 As System.Windows.Forms.RadioButton
    Friend WithEvents rbAnswer2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbAnswer3 As System.Windows.Forms.RadioButton
    Friend WithEvents rbAnswer4 As System.Windows.Forms.RadioButton
    Friend WithEvents gbQuestion2 As System.Windows.Forms.GroupBox
    Friend WithEvents cbAbend As System.Windows.Forms.CheckBox
    Friend WithEvents rbAnswer6 As System.Windows.Forms.RadioButton
    Friend WithEvents rbAnswer5 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cbAccess As System.Windows.Forms.CheckBox
    Friend WithEvents tbScreen As System.Windows.Forms.TextBox
    Friend WithEvents lbScreen As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
        Me.tbUserID = New System.Windows.Forms.TextBox
        Me.tbComments = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cbAccess = New System.Windows.Forms.CheckBox
        Me.cbSystem = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cbBusinessUnit = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbOtherUserIDs = New System.Windows.Forms.TextBox
        Me.lblUID = New System.Windows.Forms.Label
        Me.lblTDS = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.btnSendEmail = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbAnswer2 = New System.Windows.Forms.RadioButton
        Me.rbAnswer1 = New System.Windows.Forms.RadioButton
        Me.gbQuestion2 = New System.Windows.Forms.GroupBox
        Me.rbAnswer4 = New System.Windows.Forms.RadioButton
        Me.rbAnswer3 = New System.Windows.Forms.RadioButton
        Me.cbAbend = New System.Windows.Forms.CheckBox
        Me.rbAnswer6 = New System.Windows.Forms.RadioButton
        Me.rbAnswer5 = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.tbScreen = New System.Windows.Forms.TextBox
        Me.lbScreen = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.gbQuestion2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbUserID
        '
        Me.tbUserID.Location = New System.Drawing.Point(88, 24)
        Me.tbUserID.MaxLength = 7
        Me.tbUserID.Name = "tbUserID"
        Me.tbUserID.Size = New System.Drawing.Size(88, 20)
        Me.tbUserID.TabIndex = 0
        Me.tbUserID.Text = ""
        '
        'tbComments
        '
        Me.tbComments.Location = New System.Drawing.Point(88, 528)
        Me.tbComments.Multiline = True
        Me.tbComments.Name = "tbComments"
        Me.tbComments.Size = New System.Drawing.Size(424, 96)
        Me.tbComments.TabIndex = 6
        Me.tbComments.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 24)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "User ID:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 512)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Comments:"
        '
        'cbAccess
        '
        Me.cbAccess.Location = New System.Drawing.Point(8, 96)
        Me.cbAccess.Name = "cbAccess"
        Me.cbAccess.Size = New System.Drawing.Size(152, 32)
        Me.cbAccess.TabIndex = 4
        Me.cbAccess.Text = "SYSTEM ACCESS"
        '
        'cbSystem
        '
        Me.cbSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSystem.Items.AddRange(New Object() {"OneLINK", "COMPASS", "LCO", "Imaging", "VuMaster"})
        Me.cbSystem.Location = New System.Drawing.Point(88, 48)
        Me.cbSystem.MaxDropDownItems = 3
        Me.cbSystem.Name = "cbSystem"
        Me.cbSystem.Size = New System.Drawing.Size(136, 21)
        Me.cbSystem.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 24)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "System:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbBusinessUnit
        '
        Me.cbBusinessUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBusinessUnit.Items.AddRange(New Object() {"Not Listed", "Accounting", "Account Services", "Auxiliary Services", "Borrower Services", "Claims Services", "Document Services", "Loan Management", "Loan Origination", "System Operations", "Systems Support", "Process Automation"})
        Me.cbBusinessUnit.Location = New System.Drawing.Point(88, 72)
        Me.cbBusinessUnit.Name = "cbBusinessUnit"
        Me.cbBusinessUnit.Size = New System.Drawing.Size(136, 21)
        Me.cbBusinessUnit.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 24)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Business Unit:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbOtherUserIDs
        '
        Me.tbOtherUserIDs.Enabled = False
        Me.tbOtherUserIDs.Location = New System.Drawing.Point(88, 200)
        Me.tbOtherUserIDs.Multiline = True
        Me.tbOtherUserIDs.Name = "tbOtherUserIDs"
        Me.tbOtherUserIDs.Size = New System.Drawing.Size(424, 56)
        Me.tbOtherUserIDs.TabIndex = 5
        Me.tbOtherUserIDs.Text = ""
        '
        'lblUID
        '
        Me.lblUID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUID.Location = New System.Drawing.Point(8, 160)
        Me.lblUID.Name = "lblUID"
        Me.lblUID.Size = New System.Drawing.Size(400, 24)
        Me.lblUID.TabIndex = 10
        Me.lblUID.Text = "List the User IDs  who STILL have access to the system:"
        Me.lblUID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTDS
        '
        Me.lblTDS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTDS.Location = New System.Drawing.Point(8, 0)
        Me.lblTDS.Name = "lblTDS"
        Me.lblTDS.Size = New System.Drawing.Size(504, 16)
        Me.lblTDS.TabIndex = 12
        Me.lblTDS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(8, 184)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(400, 16)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "EXAMPLE: ""UT00110, UT00120, UT00130"""
        '
        'Label7
        '
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(184, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 24)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "EXAMPLE: ""UT00110"""
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSendEmail
        '
        Me.btnSendEmail.Location = New System.Drawing.Point(176, 632)
        Me.btnSendEmail.Name = "btnSendEmail"
        Me.btnSendEmail.TabIndex = 7
        Me.btnSendEmail.Text = "Send Email"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(272, 632)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbAnswer2)
        Me.GroupBox1.Controls.Add(Me.rbAnswer1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 264)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(504, 72)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "When your connection was terminated, what action(s) were you performing?"
        '
        'rbAnswer2
        '
        Me.rbAnswer2.Enabled = False
        Me.rbAnswer2.Location = New System.Drawing.Point(24, 40)
        Me.rbAnswer2.Name = "rbAnswer2"
        Me.rbAnswer2.Size = New System.Drawing.Size(360, 24)
        Me.rbAnswer2.TabIndex = 1
        Me.rbAnswer2.Text = "I was logged into the system but was not actively working."
        '
        'rbAnswer1
        '
        Me.rbAnswer1.Enabled = False
        Me.rbAnswer1.Location = New System.Drawing.Point(24, 16)
        Me.rbAnswer1.Name = "rbAnswer1"
        Me.rbAnswer1.Size = New System.Drawing.Size(360, 24)
        Me.rbAnswer1.TabIndex = 0
        Me.rbAnswer1.Text = "I was actively working on the system that went down."
        '
        'gbQuestion2
        '
        Me.gbQuestion2.Controls.Add(Me.rbAnswer4)
        Me.gbQuestion2.Controls.Add(Me.rbAnswer3)
        Me.gbQuestion2.Location = New System.Drawing.Point(8, 336)
        Me.gbQuestion2.Name = "gbQuestion2"
        Me.gbQuestion2.Size = New System.Drawing.Size(504, 72)
        Me.gbQuestion2.TabIndex = 17
        Me.gbQuestion2.TabStop = False
        Me.gbQuestion2.Text = "After your connection was terminated, what error did you receive when you tried t" & _
        "o log back in?"
        '
        'rbAnswer4
        '
        Me.rbAnswer4.Enabled = False
        Me.rbAnswer4.Location = New System.Drawing.Point(24, 40)
        Me.rbAnswer4.Name = "rbAnswer4"
        Me.rbAnswer4.Size = New System.Drawing.Size(448, 24)
        Me.rbAnswer4.TabIndex = 1
        Me.rbAnswer4.Text = "I received a message indicating no connection could be established."
        '
        'rbAnswer3
        '
        Me.rbAnswer3.Enabled = False
        Me.rbAnswer3.Location = New System.Drawing.Point(24, 16)
        Me.rbAnswer3.Name = "rbAnswer3"
        Me.rbAnswer3.Size = New System.Drawing.Size(448, 24)
        Me.rbAnswer3.TabIndex = 0
        Me.rbAnswer3.Text = "I received a Security Violation indicating that I was already logged in."
        '
        'cbAbend
        '
        Me.cbAbend.Location = New System.Drawing.Point(8, 120)
        Me.cbAbend.Name = "cbAbend"
        Me.cbAbend.Size = New System.Drawing.Size(152, 32)
        Me.cbAbend.TabIndex = 18
        Me.cbAbend.Text = "SYSTEM ABEND"
        '
        'rbAnswer6
        '
        Me.rbAnswer6.Enabled = False
        Me.rbAnswer6.Location = New System.Drawing.Point(24, 64)
        Me.rbAnswer6.Name = "rbAnswer6"
        Me.rbAnswer6.Size = New System.Drawing.Size(448, 24)
        Me.rbAnswer6.TabIndex = 3
        Me.rbAnswer6.Text = "I was in VuMaster and the system did not allow me to pull up the imaged document." & _
        ""
        '
        'rbAnswer5
        '
        Me.rbAnswer5.Enabled = False
        Me.rbAnswer5.Location = New System.Drawing.Point(24, 16)
        Me.rbAnswer5.Name = "rbAnswer5"
        Me.rbAnswer5.Size = New System.Drawing.Size(448, 24)
        Me.rbAnswer5.TabIndex = 2
        Me.rbAnswer5.Text = "I'm currently receiving an abend in (please select a system above)."
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lbScreen)
        Me.GroupBox2.Controls.Add(Me.tbScreen)
        Me.GroupBox2.Controls.Add(Me.rbAnswer5)
        Me.GroupBox2.Controls.Add(Me.rbAnswer6)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 408)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(504, 96)
        Me.GroupBox2.TabIndex = 19
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "System can still be accessed but:"
        '
        'tbScreen
        '
        Me.tbScreen.Enabled = False
        Me.tbScreen.Location = New System.Drawing.Point(160, 40)
        Me.tbScreen.Name = "tbScreen"
        Me.tbScreen.TabIndex = 4
        Me.tbScreen.Text = ""
        '
        'lbScreen
        '
        Me.lbScreen.Enabled = False
        Me.lbScreen.Location = New System.Drawing.Point(56, 44)
        Me.lbScreen.Name = "lbScreen"
        Me.lbScreen.Size = New System.Drawing.Size(100, 16)
        Me.lbScreen.TabIndex = 5
        Me.lbScreen.Text = "What screen:"
        Me.lbScreen.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Form1
        '
        Me.AcceptButton = Me.btnSendEmail
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(520, 661)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cbAbend)
        Me.Controls.Add(Me.gbQuestion2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSendEmail)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblTDS)
        Me.Controls.Add(Me.lblUID)
        Me.Controls.Add(Me.tbOtherUserIDs)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cbBusinessUnit)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cbSystem)
        Me.Controls.Add(Me.cbAccess)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbComments)
        Me.Controls.Add(Me.tbUserID)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "System Access Lost"
        Me.GroupBox1.ResumeLayout(False)
        Me.gbQuestion2.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private TestMode As Boolean = True 'Change when the app goes live


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim RefToActivate As String
        Dim ImageSysToActivate As String
        Dim VuMasterToActivate As String
        Dim TS As New TimeSpan(0, 0, 1)
        RefToActivate = GetReflectionWindowText()  'get title text from Reflection Session window
        If RefToActivate <> "" Then
            AppActivate(RefToActivate) 'activate the open reflection session
            SendKeys.SendWait("%( x)")  'Maximize window
            SendKeys.Flush()
            System.Threading.Thread.CurrentThread.Sleep(TS)
            SendKeys.SendWait("^(+{PRTSC})") 'do a screen print
            SendKeys.Flush()
            SendKeys.SendWait("%( n)") 'minimize window
            SendKeys.Flush()
            SaveScreenPrint("T:\System Down Screen Print.doc")
        End If
        ImageSysToActivate = GetImagingWindowText()
        If ImageSysToActivate <> "" Then
            AppActivate(ImageSysToActivate) 'activate the open reflection session
            SendKeys.SendWait("%( x)")  'Maximize window
            SendKeys.Flush()
            System.Threading.Thread.CurrentThread.Sleep(TS)
            SendKeys.SendWait("^(+{PRTSC})") 'do a screen print
            SendKeys.Flush()
            SendKeys.SendWait("%( n)") 'minimize window
            SendKeys.Flush()
            SaveScreenPrint("T:\Imaging System Down Screen Print.doc")
        End If
        VuMasterToActivate = GetVuMasterWindowText()
        If VuMasterToActivate <> "" Then
            AppActivate(VuMasterToActivate) 'activate the open reflection session
            SendKeys.SendWait("%( x)")  'Maximize window
            SendKeys.Flush()
            System.Threading.Thread.CurrentThread.Sleep(TS)
            SendKeys.SendWait("^(+{PRTSC})") 'do a screen print
            SendKeys.Flush()
            SendKeys.SendWait("%( n)") 'minimize window
            SendKeys.Flush()
            SaveScreenPrint("T:\VuMaster Down Screen Print.doc")
        End If
        If VuMasterToActivate = "" Then
            cbSystem.Items.Remove("VuMaster")
        End If
        If ImageSysToActivate = "" Then
            cbSystem.Items.Remove("Imaging")
        End If
        If RefToActivate = "" Then
            cbSystem.Items.Remove("LCO")
            cbSystem.Items.Remove("OneLINK")
            cbSystem.Items.Remove("COMPASS")
        End If
        If ImageSysToActivate = "" And RefToActivate = "" And VuMasterToActivate = "" Then
            MsgBox("None of the necessary systems were found running.", MsgBoxStyle.Critical, "Systems Not Running")
            End
        End If
        lblTDS.Text = Now()
    End Sub

    'PrintDocs merges the merge data source text file with the specified document
    Sub SaveScreenPrint(ByVal FileNm As String)
        'set up Word object
        Dim word As New Word.Application
        word.Visible = False
        word.Documents.Add()
        'make page landscape
        'Word.ActiveDocument.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape
        word.Selection.Paste() 'paste screen print
        'save the file
        word.ActiveDocument.SaveAs(FileNm)
        'close word
        word.Application.Quit()
    End Sub

    Function GetImagingWindowText() As String
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
            If UCase(objProcess.MainWindowTitle).IndexOf("ASCENT CAPTURE") <> -1 Then
                GetImagingWindowText = objProcess.MainWindowTitle
                Exit Function
            End If
        Next
        'if the code leaves through here then the imaging system software wasn't found
        GetImagingWindowText = ""
    End Function

    Function GetVuMasterWindowText() As String
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
            If UCase(objProcess.MainWindowTitle).IndexOf("VUMASTER") <> -1 Then
                GetVuMasterWindowText = objProcess.MainWindowTitle
                Exit Function
            End If
        Next
        'if the code leaves through here then the imaging system software wasn't found
        GetVuMasterWindowText = ""
    End Function

    Function GetReflectionWindowText() As String
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
            'If (UCase(objProcess.MainWindowTitle).IndexOf(".RSF") <> -1) And UCase(objProcess.MainWindowTitle).IndexOf("NELNET") = -1 Then
            If UCase(objProcess.ProcessName) = "R8WIN" And UCase(objProcess.MainWindowTitle).IndexOf("NELNET") = -1 Then
                GetReflectionWindowText = objProcess.MainWindowTitle
                Exit Function
            End If
        Next
        'if the code leaves through here then no Reflection sessions were found
        GetReflectionWindowText = ""
    End Function

    'bring app to front
    Private Sub Form1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Try
            AppActivate("System Access Lost")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub cbOthersStillIn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAccess.CheckedChanged
        If cbAccess.Checked Then
            cbAbend.Enabled = False
            cbAbend.Checked = False
            rbAnswer1.Enabled = True
            rbAnswer2.Enabled = True
            rbAnswer3.Enabled = True
            rbAnswer4.Enabled = True
            tbOtherUserIDs.Enabled = True
            'lblUID.Font.Bold = True
            lblUID.ForeColor = Color.Blue
        Else
            cbAbend.Enabled = True
            rbAnswer1.Enabled = False
            rbAnswer2.Enabled = False
            rbAnswer3.Enabled = False
            rbAnswer4.Enabled = False
            rbAnswer1.Checked = False
            rbAnswer2.Checked = False
            rbAnswer3.Checked = False
            rbAnswer4.Checked = False
            tbOtherUserIDs.Enabled = False
            tbOtherUserIDs.Clear()
            lblUID.ForeColor = Color.Black
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        End
    End Sub

    Function ValidData() As Boolean
        If tbUserID.TextLength <> 7 Then
            MsgBox("A valid user id must be provided.", MsgBoxStyle.Critical)
            Return False
        End If
        If cbSystem.SelectedIndex = -1 Then
            MsgBox("A system must be provided.", MsgBoxStyle.Critical)
            Return False
        End If
        If cbAccess.Checked Then
            'check if all questions are answered
            If rbAnswer1.Checked = False And rbAnswer2.Checked = False Then
                MsgBox("Please answer all questions.", MsgBoxStyle.Information)
                Return False
            End If
            If cbSystem.SelectedItem <> "Imaging" Then
                If rbAnswer3.Checked = False And rbAnswer4.Checked = False Then
                    MsgBox("Please answer all questions.", MsgBoxStyle.Information)
                    Return False
                End If
            End If
        ElseIf cbAbend.Checked Then
            If rbAnswer6.Checked = False And rbAnswer5.Checked = False Then
                MsgBox("Please answer all questions.", MsgBoxStyle.Information)
                Return False
            End If
            If rbAnswer5.Checked And tbScreen.TextLength = 0 Then
                MsgBox("The screen the abend is occurring on is required.", MsgBoxStyle.Information)
                Return False
            End If
        Else
            MsgBox("Please indicate whether you are encountering an abend or have lost access to the system.", MsgBoxStyle.Information)
            Return False
        End If
        ValidData = True 'everything is valid
    End Function

    Private Sub btnSendEmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendEmail.Click
        'Dim GWAcc As GroupwareTypeLibrary.Account
        'Dim GWApp As New GroupwareTypeLibrary.Application
        'Dim GWMsg As GroupwareTypeLibrary.Message
        Dim Recip As String
        Dim Attach As String
        Dim Subject As String
        Dim Body As String
        Dim pls As New PlsWait
        'validate user provided data
        If ValidData() Then
            'GWAcc = GWApp.Login 'login to Groupwise 
            'GWMsg = GWAcc.MailBox.Messages.Add()
            'figure out manager email
            If cbBusinessUnit.SelectedItem = "Accounting" Then
                Recip = "dperry@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "Account Services" Then
                Recip = "dcox@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "Borrower Services" Then
                Recip = "bhill@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "Loan Origination" Then
                Recip = "tvig@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "Loan Management" Then
                Recip = "kjorgensen@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "Auxiliary Services" Then
                Recip = "rrobbins@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "Claims Services" Then
                Recip = "mhansen@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "Process Automation" Then
                Recip = "jdavis@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "System Operations" Then
                Recip = "manderson@utahsbr.edu"
            ElseIf cbBusinessUnit.SelectedItem = "Document Services" Then
                Recip = "manderson@utahsbr.edu"
            End If
            'add System Notification
            Recip = Recip & ",AESSystemAccess@utahsbr.edu"
            'attach screen print to email
            If cbSystem.SelectedItem = "Imaging" Then
                Attach = "T:\Imaging System Down Screen Print.doc"
            ElseIf cbSystem.SelectedItem = "VuMaster" Then
                Attach = "T:\VuMaster Down Screen Print.doc"
            Else
                Attach = "T:\System Down Screen Print.doc"
            End If
            'figure subject line and body text
            If TestMode Then
                Subject = "(THIS IS A TEST.  PLEASE IGNORE THIS EMAIL.) System Down Notification"
                Body = "THIS IS A TEST.  PLEASE IGNORE THIS EMAIL." & vbLf & vbLf & _
                                        "Please see attached screen print." & vbLf & vbLf & _
                                        "Time Date Stamp of Occurrence: " & lblTDS.Text & vbLf & _
                                        "User Name: " & System.Environment.UserName & vbLf & _
                                        "User ID: " & tbUserID.Text & vbLf & _
                                        "System: " & cbSystem.SelectedItem & vbLf & _
                                        "Business Unit: " & cbBusinessUnit.SelectedItem & vbLf
                If cbAccess.Checked Then
                    Body = Body & "System access lost." & vbLf
                    If tbOtherUserIDs.TextLength <> 0 Then
                        Body = Body & "The following users still have system access: " & tbOtherUserIDs.Text & vbLf
                    End If
                    Body = Body & AnswerToQuestion1() & vbLf & AnswerToQuestion2() & vbLf & "Comments: " & tbComments.Text
                Else
                    Body = Body & "Experiencing a system abend." & vbLf
                    Body = Body & AnswerToQuestion3() & vbLf & "Screen: " & tbScreen.Text & vbLf & "Comments: " & tbComments.Text
                End If
                'If cbOthersStillIn.Checked Then
                '    GWMsg.BodyText.PlainText = GWMsg.BodyText.PlainText & TranslateCheckBox() & ", others did have access the the system." & vbLf & _
                '    "The following users still have system access: " & tbOtherUserIDs.Text & vbLf & AnswerToQuestion1() & vbLf & AnswerToQuestion2() & vbLf & _
                '    "Comments: " & tbComments.Text
                'Else
                '    GWMsg.BodyText.PlainText = GWMsg.BodyText.PlainText & TranslateCheckBox() & ", others didn't have access the the system." & vbLf & AnswerToQuestion1() & vbLf & AnswerToQuestion2() & vbLf & _
                '    "Comments: " & tbComments.Text
                'End If
                Recip = Recip & ",jgutierrez@utahsbr.edu"
                Recip = Recip & "," & System.Environment.UserName & "@utahsbr.edu"
            Else
                Subject = "System Down Notification"
                Body = "Please see attached screen print." & vbLf & vbLf & _
                                        "Time Date Stamp of Occurrence: " & lblTDS.Text & vbLf & _
                                        "User Name: " & System.Environment.UserName & vbLf & _
                                        "User ID: " & tbUserID.Text & vbLf & _
                                        "System: " & cbSystem.SelectedItem & vbLf & _
                                        "Business Unit: " & cbBusinessUnit.SelectedItem & vbLf
                If cbAccess.Checked Then
                    Body = Body & "System access lost." & vbLf
                    If tbOtherUserIDs.TextLength <> 0 Then
                        Body = Body & "The following users still have system access: " & tbOtherUserIDs.Text & vbLf
                    End If
                    Body = Body & AnswerToQuestion1() & vbLf & AnswerToQuestion2() & vbLf & "Comments: " & tbComments.Text
                Else
                    Body = Body & "Experiencing a system abend." & vbLf
                    Body = Body & AnswerToQuestion3() & vbLf & "Screen: " & tbScreen.Text & vbLf & "Comments: " & tbComments.Text
                End If
                'If cbOthersStillIn.Checked Then
                '    GWMsg.BodyText.PlainText = GWMsg.BodyText.PlainText & TranslateCheckBox() & ", others did have access the the system." & vbLf & _
                '    "The following users still have system access: " & tbOtherUserIDs.Text & vbLf & AnswerToQuestion1() & vbLf & AnswerToQuestion2() & vbLf & _
                '    "Comments: " & tbComments.Text
                'Else
                '    GWMsg.BodyText.PlainText = GWMsg.BodyText.PlainText & TranslateCheckBox() & ", others didn't have access the the system." & vbLf & AnswerToQuestion1() & vbLf & AnswerToQuestion2() & vbLf & _
                '    "Comments: " & tbComments.Text
                'End If
            End If
            SendMail(System.Environment.UserName & "@utahsbr.edu", Recip, Subject, Body, , , Attach)
            Do
                pls.Show()
                Try
                    If TestMode Then
                        FileOpen(1, "X:\PADD\System Down\Test\Log File.txt", OpenMode.Append)
                    Else
                        FileOpen(1, "X:\PADD\System Down\Log File.txt", OpenMode.Append)
                    End If

                    'write data to file
                    'WriteLine(1, lblTDS.Text, GWAcc.Owner.DisplayName, tbUserID.Text, cbSystem.SelectedItem, cbBusinessUnit.SelectedItem, TranslateCheckBox(), tbOtherUserIDs.Text, AnswerToQuestion1(), AnswerToQuestion2(), tbComments.Text)
                    WriteLine(1, lblTDS.Text, System.Environment.UserName, tbUserID.Text, cbSystem.SelectedItem, cbBusinessUnit.SelectedItem, "", tbOtherUserIDs.Text, AnswerToQuestion1(), AnswerToQuestion2(), tbComments.Text, AnswerToQuestion3(), TranslateCheckBox(), TranslateCheckBox2(), tbScreen.Text)
                    'close file
                    FileClose(1)
                    pls.Hide()
                    MsgBox("Notification of System Down Time has been sent to the help desk and Systems Support.  The information has also been logged to a file.")
                    Exit Do
                Catch ex As Exception

                End Try
            Loop
            End
        End If
    End Sub

    'send an e-mail message using SMTP
    Public Function SendMail(ByVal mFrom As String, ByVal mTo As String, Optional ByVal mSubject As String = "", Optional ByVal mBody As String = "", Optional ByVal mCC As String = "", Optional ByVal mBCC As String = "", Optional ByVal mAttach As String = "") As Boolean
        Dim aAttach() As String
        Dim i As Integer
        Dim eMail As OSSMTP.SMTPSession
        eMail = New OSSMTP.SMTPSession

        'set server
        eMail.Server = "mail.utahsbr.edu"

        'create message
        eMail.MailFrom = mFrom
        eMail.SendTo = mTo
        eMail.CC = mCC
        eMail.BCC = mBCC
        eMail.MessageSubject = mSubject
        eMail.MessageText = mBody

        'add attachments if there are any
        If Len(mAttach) > 0 Then
            'split file names from string
            aAttach = Split(mAttach, ",")

            'add attachments
            For i = 0 To UBound(aAttach)
                eMail.Attachments.Add(aAttach(i))
            Next i
        End If

        'send message
        eMail.SendEmail()

        'verify the message was sent
        If eMail.Status = "SMTP connection closed" Then
            SendMail = True
        Else
            SendMail = False
        End If
    End Function


    Function TranslateCheckBox() As String
        If cbAccess.Checked Then
            TranslateCheckBox = "Yes"
        Else
            TranslateCheckBox = "No"
        End If
    End Function

    Function TranslateCheckBox2() As String
        If cbAbend.Checked Then
            TranslateCheckBox2 = "Yes"
        Else
            TranslateCheckBox2 = "No"
        End If
    End Function

    Function AnswerToQuestion1() As String
        If rbAnswer1.Checked Then
            Return rbAnswer1.Text
        ElseIf rbAnswer2.Checked Then
            Return rbAnswer2.Text
        Else
            Return ""
        End If
    End Function

    Function AnswerToQuestion3() As String
        If rbAnswer5.Checked Then
            Return rbAnswer5.Text
        ElseIf rbAnswer6.Checked Then
            Return rbAnswer6.Text
        Else
            Return ""
        End If
    End Function

    Function AnswerToQuestion2() As String
        If rbAnswer3.Checked Then
            Return rbAnswer3.Text
        ElseIf rbAnswer4.Checked Then
            Return rbAnswer4.Text
        Else
            Return ""
        End If
    End Function

    Private Sub cbSystem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSystem.SelectedIndexChanged
        'make sure they have selected a system
        If cbSystem.SelectedItem = "Imaging" Then
            gbQuestion2.Enabled = False
            rbAnswer3.Enabled = False
            rbAnswer4.Enabled = False
        Else
            If cbAccess.Checked Then
                gbQuestion2.Enabled = True
                rbAnswer3.Enabled = True
                rbAnswer4.Enabled = True
            ElseIf cbAbend.Checked Then
                If cbSystem.SelectedItem = "LCO" Or cbSystem.SelectedItem = "COMPASS" Or cbSystem.SelectedItem = "OneLINK" Or cbSystem.SelectedItem = "Imaging" Then
                    rbAnswer5.Enabled = True
                    rbAnswer6.Enabled = False
                ElseIf cbSystem.SelectedItem = "VuMaster" Then
                    rbAnswer5.Enabled = False
                    rbAnswer6.Enabled = True
                End If
            End If
        End If
        rbAnswer5.Text = "I'm currently receiving an abend in " & cbSystem.SelectedItem & "."
    End Sub

    Private Sub cbAbend_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAbend.CheckedChanged
        If cbAbend.Checked Then
            cbAccess.Enabled = False
            cbAccess.Checked = False
            rbAnswer1.Enabled = False
            rbAnswer2.Enabled = False
            rbAnswer3.Enabled = False
            rbAnswer4.Enabled = False
            If cbSystem.SelectedItem = "Imaging" Or cbSystem.SelectedItem = "OneLINK" Or cbSystem.SelectedItem = "LCO" Or cbSystem.SelectedItem = "COMPASS" Then
                rbAnswer5.Enabled = True
            Else
                rbAnswer5.Enabled = False
            End If
            If cbSystem.SelectedItem = "VuMaster" Then
                rbAnswer6.Enabled = True
            Else
                rbAnswer6.Enabled = False
            End If
            rbAnswer1.Checked = False
            rbAnswer2.Checked = False
            rbAnswer3.Checked = False
            rbAnswer4.Checked = False
        Else
            cbAccess.Enabled = True
            rbAnswer5.Enabled = False
            rbAnswer6.Enabled = False
            rbAnswer5.Checked = False
            rbAnswer6.Checked = False
        End If
    End Sub

    Private Sub rbAnswer5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbAnswer5.CheckedChanged
        If rbAnswer5.Checked Then
            lbScreen.Enabled = True
            tbScreen.Enabled = True
        Else
            lbScreen.Enabled = False
            tbScreen.Enabled = False
            tbScreen.Clear()
        End If
    End Sub
End Class
