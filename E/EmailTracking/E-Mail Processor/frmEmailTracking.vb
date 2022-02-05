Imports System.Threading
Imports System.Data.SqlClient
Imports MailBee.ImapMail
Imports MailBee
Imports MailBee.Mime
Imports MailBee.SmtpMail

Public Class frmEmailTracking
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
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbCC As System.Windows.Forms.ComboBox
    Friend WithEvents txtAC As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbSIT As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents btnFN As System.Windows.Forms.Button
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents SqlConn As System.Data.SqlClient.SqlConnection
    Friend WithEvents btndelete As System.Windows.Forms.Button
    Friend WithEvents MICurrentStatusReport As System.Windows.Forms.MenuItem
    Friend WithEvents TestSqlConn As System.Data.SqlClient.SqlConnection
    Friend WithEvents MIDirectToAuxSer As System.Windows.Forms.MenuItem
    Friend WithEvents MIDirectToAcctSer As System.Windows.Forms.MenuItem
    Friend WithEvents MIDirectToLoanOrig As System.Windows.Forms.MenuItem
    Friend WithEvents MonthEndBUTotals As System.Windows.Forms.MenuItem
    Friend WithEvents MonthEndResponderTotals As System.Windows.Forms.MenuItem
    Friend WithEvents MIQCReport As System.Windows.Forms.MenuItem
    Friend WithEvents MIBUPersCopied As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MonthEnd As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents NIBatchEmailTracking As System.Windows.Forms.NotifyIcon
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmEmailTracking))
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cbCC = New System.Windows.Forms.ComboBox
        Me.txtAC = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnFN = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.cbSIT = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtSubject = New System.Windows.Forms.TextBox
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MICurrentStatusReport = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MonthEnd = New System.Windows.Forms.MenuItem
        Me.MonthEndBUTotals = New System.Windows.Forms.MenuItem
        Me.MonthEndResponderTotals = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MIDirectToLoanOrig = New System.Windows.Forms.MenuItem
        Me.MIDirectToAcctSer = New System.Windows.Forms.MenuItem
        Me.MIDirectToAuxSer = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MIQCReport = New System.Windows.Forms.MenuItem
        Me.MIBUPersCopied = New System.Windows.Forms.MenuItem
        Me.SqlConn = New System.Data.SqlClient.SqlConnection
        Me.btndelete = New System.Windows.Forms.Button
        Me.TestSqlConn = New System.Data.SqlClient.SqlConnection
        Me.NIBatchEmailTracking = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.SuspendLayout()
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(16, 80)
        Me.txtEmail.Multiline = True
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.ReadOnly = True
        Me.txtEmail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEmail.Size = New System.Drawing.Size(632, 152)
        Me.txtEmail.TabIndex = 0
        Me.txtEmail.TabStop = False
        Me.txtEmail.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "E-Mail Body:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 280)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Canned Comments:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbCC
        '
        Me.cbCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCC.Items.AddRange(New Object() {"", "The borrower did not provide an account number, or enough other information to al" & _
        "low us to definitely identify his/her account.  Please verify identity before su" & _
        "pplying specific account information.", "Although the borrower did not provide an account number, based on two pieces of i" & _
        "dentifying information provided in the message, we were able to identify the pro" & _
        "bable account number as XX-XXXX-XXXX."})
        Me.cbCC.Location = New System.Drawing.Point(16, 304)
        Me.cbCC.Name = "cbCC"
        Me.cbCC.Size = New System.Drawing.Size(632, 21)
        Me.cbCC.TabIndex = 2
        '
        'txtAC
        '
        Me.txtAC.Location = New System.Drawing.Point(16, 352)
        Me.txtAC.Multiline = True
        Me.txtAC.Name = "txtAC"
        Me.txtAC.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAC.Size = New System.Drawing.Size(632, 152)
        Me.txtAC.TabIndex = 3
        Me.txtAC.Text = ""
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 328)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 23)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Additional Comments:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnFN
        '
        Me.btnFN.Location = New System.Drawing.Point(232, 512)
        Me.btnFN.Name = "btnFN"
        Me.btnFN.Size = New System.Drawing.Size(88, 23)
        Me.btnFN.TabIndex = 0
        Me.btnFN.Text = "&Forward/Next"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 232)
        Me.Label4.Name = "Label4"
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Send it to:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbSIT
        '
        Me.cbSIT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSIT.Location = New System.Drawing.Point(16, 256)
        Me.cbSIT.Name = "cbSIT"
        Me.cbSIT.Size = New System.Drawing.Size(128, 21)
        Me.cbSIT.Sorted = True
        Me.cbSIT.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Subject:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSubject
        '
        Me.txtSubject.Location = New System.Drawing.Point(16, 32)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.ReadOnly = True
        Me.txtSubject.Size = New System.Drawing.Size(632, 20)
        Me.txtSubject.TabIndex = 12
        Me.txtSubject.TabStop = False
        Me.txtSubject.Text = ""
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MICurrentStatusReport, Me.MenuItem2, Me.MenuItem4})
        Me.MenuItem1.Text = "Reports"
        '
        'MICurrentStatusReport
        '
        Me.MICurrentStatusReport.Index = 0
        Me.MICurrentStatusReport.Text = "Current Status"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 1
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MonthEnd, Me.MonthEndBUTotals, Me.MonthEndResponderTotals, Me.MenuItem3})
        Me.MenuItem2.Text = "Month End Reports"
        '
        'MonthEnd
        '
        Me.MonthEnd.Index = 0
        Me.MonthEnd.Text = "Month End Report"
        '
        'MonthEndBUTotals
        '
        Me.MonthEndBUTotals.Index = 1
        Me.MonthEndBUTotals.Text = "Month End Report (BU Totals)"
        '
        'MonthEndResponderTotals
        '
        Me.MonthEndResponderTotals.Index = 2
        Me.MonthEndResponderTotals.Text = "Month End Report (Responder Totals)"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 3
        Me.MenuItem3.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MIDirectToLoanOrig, Me.MIDirectToAcctSer, Me.MIDirectToAuxSer})
        Me.MenuItem3.Text = "Direct Email Reports"
        '
        'MIDirectToLoanOrig
        '
        Me.MIDirectToLoanOrig.Index = 0
        Me.MIDirectToLoanOrig.Text = "Direct Email To Loan Orig"
        '
        'MIDirectToAcctSer
        '
        Me.MIDirectToAcctSer.Index = 1
        Me.MIDirectToAcctSer.Text = "Direct Email To Acct Services"
        '
        'MIDirectToAuxSer
        '
        Me.MIDirectToAuxSer.Index = 2
        Me.MIDirectToAuxSer.Text = "Direct Email To Aux Services"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 2
        Me.MenuItem4.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MIQCReport, Me.MIBUPersCopied})
        Me.MenuItem4.Text = "General Reports"
        '
        'MIQCReport
        '
        Me.MIQCReport.Index = 0
        Me.MIQCReport.Text = "QC Report"
        '
        'MIBUPersCopied
        '
        Me.MIBUPersCopied.Index = 1
        Me.MIBUPersCopied.Text = "BU Personnel Copied Report"
        '
        'SqlConn
        '
        Me.SqlConn.ConnectionString = "workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=N" & _
        "OCHOUSE;persist security info=False;initial catalog=EmailTracking"
        '
        'btndelete
        '
        Me.btndelete.Location = New System.Drawing.Point(336, 512)
        Me.btndelete.Name = "btndelete"
        Me.btndelete.TabIndex = 13
        Me.btndelete.TabStop = False
        Me.btndelete.Text = "Delete"
        '
        'TestSqlConn
        '
        Me.TestSqlConn.ConnectionString = "Data Source=OPSDEV;Initial Catalog=EmailTracking;Integrated Security=SSPI;"
        '
        'NIBatchEmailTracking
        '
        Me.NIBatchEmailTracking.Icon = CType(resources.GetObject("NIBatchEmailTracking.Icon"), System.Drawing.Icon)
        Me.NIBatchEmailTracking.Text = "Batch Email Tracking"
        Me.NIBatchEmailTracking.Visible = True
        '
        'frmEmailTracking
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(656, 544)
        Me.Controls.Add(Me.btndelete)
        Me.Controls.Add(Me.txtSubject)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cbSIT)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnFN)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtAC)
        Me.Controls.Add(Me.cbCC)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtEmail)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(664, 592)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmEmailTracking"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "E-mail Tracking"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Const AccOwnerTest As String = "loantest"
    Private Const AccOwner As String = "student loan"
    'Private Proc As New Processing 'processing window
    Private TestMode As Boolean
    Private ActualConn As SqlConnection
    Private TesterEmail As String
    Private NonCompAttachFound As Boolean
    Private TheEmailWatcher As EmailWatcher
    Private Account As Imap
    Private msgs As UidCollection 'message collection
    Private msg As MailMessage
    Private WaitTimeSpan As Double = 600000 'wait time for timer 10 minutes
    Private aTimer As System.Timers.Timer
    Private BatchProcessingMode As Boolean
    Private Const Server As String = "mail.utahsbr.edu"
	Private Const LKey As String = "MN700-5199E66A998C99C999AD48E28858-997E"
    Private Const SMTPLKey As String = "MN100-4A2E2CBC2EB52E352EE8E592264B-1B79"

    Private Sub frmEmailTracking_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Loginfrm As New Login
		'set up keys for functionality
		MailBee.Global.LicenseKey = LKey
		'Imap.LicenseKey = LKey
		'Smtp.LicenseKey = SMTPLKey
        'create IMAP object
        Account = New Imap
        Account.Connect(Server, 143) 'set up server and port info
        Loginfrm.ShowDialog()
        'check if an account id is given
        If Loginfrm.tbAccID.TextLength <> 0 Then
            Try
                Account.Login(Loginfrm.tbAccID.Text, Loginfrm.tbPassword.Text)
            Catch ex As Exception
                MsgBox("That wasn't a valid account ID and password.  Please try again.")
                End
			End Try
			Try
				'if the account isn't then student loan live or test account then 
				If Loginfrm.tbAccID.Text.ToUpper <> AccOwner.ToUpper.Replace(" ", "") And Loginfrm.tbAccID.Text.ToUpper <> AccOwnerTest.ToUpper Then
					MsgBox("That wasn't a valid account ID and password.  Please try again.")
					End
				End If
				'is the user logging into the live environment
				If Loginfrm.tbAccID.Text.ToUpper = AccOwner.ToUpper.Replace(" ", "") Then
					MsgBox("Welcome to the LIVE environment for the E-mail Tracking System.", MsgBoxStyle.Information, "Welcome to LIVE")
					TestMode = False
					'setup for correct DB
					ActualConn = SqlConn
					TestSqlConn.Dispose()
				Else 'the user is in the test environment
					MsgBox("Welcome to the TEST environment for the E-mail Tracking System.", MsgBoxStyle.Information, "Welcome to TEST")
					TestMode = True
					'setup for correct DB
					ActualConn = TestSqlConn
					SqlConn.Dispose()
					'get the tester's email address
					While TesterEmail = ""
						TesterEmail = InputBox("As a tester it is important that you see what is being sent out to the business units.  Please enter your e-mail address and you will be copied on every e-mail sent out by E-mail Processor.  Example: Earl J Pickelsnarf would enter ""epickelsnarf@utahsbr.edu"".", "Tester E-mail address")
						If TesterEmail = "" Then
							MsgBox("You didn't enter an e-mail address.  Please try again.", MsgBoxStyle.Critical, "E-mail Address Needed")
						End If
					End While
				End If
				'get drop down options from DB
				ActualConn.Open()
				Dim Reader As SqlDataReader
				Dim Comm As New SqlCommand("SELECT COUNT(*) FROM BatchAccessPCs WHERE PCName = '" & Environment.MachineName & "'", ActualConn)
				'don't show notify icon if in batch processing mode
				If CType(Comm.ExecuteScalar(), Integer) > 0 Then
					BatchProcessingMode = True
					NIBatchEmailTracking.Visible = True
				Else
					BatchProcessingMode = False
					NIBatchEmailTracking.Visible = False
				End If
				Comm.CommandText = "SELECT TheGroup FROM EmailGroups WHERE EmailTrackerOnly = 'False'"
				Reader = Comm.ExecuteReader
				cbSIT.Items.Add("")
				While Reader.Read
					cbSIT.Items.Add(Reader.GetString(0))
				End While
				cbSIT.Sorted = True
				Reader.Close()
				ActualConn.Close()
				cbSIT.SelectedIndex = 0
				'make sure that needed folders exist and create if they don't
				FolderCheckAndCreation("OriginalDeleted")
				FolderCheckAndCreation("IntermediateDeleted")
				FolderCheckAndCreation("FinalDeleted")
				FolderCheckAndCreation("ManualProcess")
				'delete all messages older than 90 days in working folders
				DeleteMsgsOlderThan90days("OriginalDeleted")
				DeleteMsgsOlderThan90days("IntermediateDeleted")
				DeleteMsgsOlderThan90days("FinalDeleted")
				'select the manual process folder to have the email processor to work out of
				Account.SelectFolder("ManualProcess")
				'start up email watcher
				TheEmailWatcher = New EmailWatcher(TestMode, TesterEmail, Loginfrm.tbAccID.Text, Loginfrm.tbPassword.Text, ActualConn.ConnectionString, Loginfrm.tbAccID.Text.ToUpper, LKey, SMTPLKey, Server, BatchProcessingMode)
				'set up keep alive handler
				aTimer = New System.Timers.Timer
				AddHandler aTimer.Elapsed, AddressOf KeepConnAlive
				aTimer.Enabled = True
				aTimer.Interval = WaitTimeSpan 'Ten Minutes
				aTimer.Start() 'start timer
			Catch ex As Exception
				MessageBox.Show(ex.Message)
			End Try
		Else
			TestMode = False
			btnFN.Enabled = False
			btndelete.Enabled = False
		End If
        Me.Focus()
        cbCC.SelectedIndex = 0
        cbCC.Enabled = False
        cbSIT.Enabled = False
		txtAC.Enabled = False

    End Sub

    'deletes messages in folder that are older than 90 days
    Sub DeleteMsgsOlderThan90days(ByVal Folder As String)
        Dim msgs As UidCollection 'message collection
        Dim I As Integer
        Account.SelectFolder(Folder)
        msgs = Account.Search(True, "BEFORE " + Format(Now.AddDays(-90), "d-MMM-yyyy"), Nothing)
        While I < msgs.Count
            Account.DeleteMessages(msgs(I).ToString(), True)
            I = I + 1
        End While
        Account.Expunge()
    End Sub

    'timer elapsed event handler
    Sub KeepConnAlive(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        If Account.IsBusy Then
            Exit Sub
        End If
        Account.Noop() 'send ping to server to keep connection alive
    End Sub

    Sub FolderCheckAndCreation(ByVal Folder As String)
        Try
            'try and select folder
            Account.SelectFolder(Folder)
        Catch ex As MailBee.ImapMail.MailBeeImapNegativeResponseException
            'create folder if it didn't exist
            Account.CreateFolder(Folder)
        End Try
    End Sub

    'this function decides which staff members to send email to.
    Function FigureRecipientsAndUpdateDB(ByRef OriginalTimeDate As String, ByVal SubjectStr As String) As String
        Dim Reader As SqlDataReader 'create ref to datareader
        Dim SQLStr As String
        Dim NumInMaster As Integer
        Dim DBEntry As String
        Dim SlotFound As Boolean = False
        Dim ToStr As String
        If TestMode Then
            ToStr = "loantest@utahsbr.edu;" & TesterEmail
        Else
            ToStr = "student_loan@utahsbr.edu"
        End If
        ActualConn.Open()
        SQLStr = "SELECT * From EmailGroups WHERE TheGroup LIKE '" & cbSIT.SelectedItem & "'"
        Dim Cmd5 As New SqlCommand(SQLStr, ActualConn)
        Reader = Cmd5.ExecuteReader() 'execute reader
        Reader.Read() 'move to first record in the reader
        ToStr = ToStr & ";" & Reader.GetString(3) 'add main recipient
        DBEntry = Reader.GetString(4)
        'add all CCs
        If Reader.GetString(5) <> "Nothing" Then ToStr = ToStr & ";" & Reader.GetString(5)
        If Reader.GetString(6) <> "Nothing" Then ToStr = ToStr & ";" & Reader.GetString(6)
        If Reader.GetString(7) <> "Nothing" Then ToStr = ToStr & ";" & Reader.GetString(7)
        If Reader.GetString(8) <> "Nothing" Then ToStr = ToStr & ";" & Reader.GetString(8)
        If Reader.GetString(9) <> "Nothing" Then ToStr = ToStr & ";" & Reader.GetString(9)
        Reader.Close()
        'Check for duplicate records
        SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
        Cmd5.CommandText = SQLStr
        While SlotFound = False
            Reader = Cmd5.ExecuteReader()
            If Reader.Read() = False Then
                SlotFound = True
            Else
                OriginalTimeDate = CStr(DateAdd(DateInterval.Second, 1, CDate(OriginalTimeDate)))
            End If
            Reader.Close()
            SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
            Cmd5.CommandText = SQLStr
        End While
        'update database
        'create master record
        SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','" & DBEntry & "','" & SubjectStr.Replace("'", "''") & "')"
        Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
        Cmd1.ExecuteNonQuery()
        'get primary key from master record for Responded to insertions
        SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
        Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
        Reader = Cmd2.ExecuteReader() 'execute reader
        Reader.Read() 'move to first record in the reader
        NumInMaster = Reader.GetValue(0)
        Reader.Close()
        'create first Responded record
        SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
        Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
        Cmd3.ExecuteNonQuery()
        'create second Responded Record
        SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Format(Now, "MM/dd/yyyy hh:mm:ss tt") & "','" & DBEntry & "')"
        Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
        Cmd4.ExecuteNonQuery()
        ActualConn.Close()
        Return ToStr 'return recipient string
    End Function

    Private Sub btnFN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFN.Click
        Dim SMTPMsg As Smtp
        Dim Recips As String
        Dim WorkingDt As Date
        Dim TS As New TimeSpan(0, 0, 1)
        Dim OriginalSubject As String
        'wait while connection is busy could be NOOPing
        While Account.IsBusy
            Thread.Sleep(TS)
        End While
        aTimer.Interval = WaitTimeSpan 'restart timer
        'check if the app has a current msg
        If (msg Is Nothing) = False Then
            'check that the user has selected a group to email to.
            If cbSIT.SelectedIndex = 0 Then
                MsgBox("You must select a Business Group to send it to.")
                Exit Sub
            End If
            Me.Visible = False
            'Proc.Visible = True
            'Proc.Refresh()
            SMTPMsg = New Smtp
            'clear servers
            SMTPMsg.SmtpServers.Clear()
            SMTPMsg.DnsServers.Clear()
            SMTPMsg.SmtpServers.Add(Server) 'add smtp server
            ''if either the date recieved or composed dates are populated then use one of those two dates for out going email
            If msg.DateReceived <> CDate("12:00:00 AM") Or msg.Date = CDate("12:00:00 AM") Then
                'one of the two dates are populated
                If msg.DateReceived <> CDate("12:00:00 AM") Then
                    'use recieved date if populated
                    WorkingDt = msg.DateReceived
                ElseIf msg.Date <> CDate("12:00:00 AM") Then
                    'use composed date if populated
                    WorkingDt = msg.Date
                End If
                ''add recipients
                'Recips = FigureRecipientsAndUpdateDB(WorkingDt, msg.Subject)
                ''add time date stamp
                'SMTPMsg.Subject = "-- " & Format(CDate(WorkingDt.ToString), "MM/dd/yyyy hh:mm:ss tt") & " -- " & msg.Subject
                'If TestMode Then
                '    SMTPMsg.Subject = "TEST EMAIL - " & SMTPMsg.Subject
                'End If
                ''add recipients to message
                'SMTPMsg.To.AsString = Recips
                ''include any commented text
                'SMTPMsg.BodyPlainText = txtAC.Text
                'SMTPMsg.From.AsString = "EmailTracking"
                'take note of original subject
                OriginalSubject = msg.Subject

                'add recipients
                Recips = FigureRecipientsAndUpdateDB(WorkingDt, OriginalSubject)
                'figure subject message and add time date stamp
                msg.Subject = "-- " & Format(CDate(WorkingDt.ToString), "MM/dd/yyyy hh:mm:ss tt") & " -- " & msg.Subject
                If TestMode Then
                    msg.Subject = "TEST EMAIL - " & msg.Subject
                End If
                'add message to SMTP message as attachment
                SMTPMsg.Message = msg.ForwardAsAttachment()
                'use same subject line for parent message
                SMTPMsg.Subject = msg.Subject
                'add recipients to message
                SMTPMsg.To.AsString = Recips
                'include any commented text
                SMTPMsg.BodyPlainText = txtAC.Text
                SMTPMsg.From.AsString = "EmailTracking"
                SMTPMsg.Send()
                'move to another folder for history 
                Account.CopyMessages(msgs.Item(0), True, "OriginalDeleted")
                Account.DeleteMessages(msgs.Item(0), True) 'mark to delete
                Account.Expunge() 'delete marked messages
            Else
                'neither the recieved or composed dates are populated
                'add message to SMTP message as attachment
                SMTPMsg.Message = msg.ForwardAsAttachment()
                SMTPMsg.Subject = "NULL Time Date Stamp Error Email -- " & msg.Subject
                'if in testmode then add text to msg being forwarded
                If TestMode Then
                    SMTPMsg.Subject = "TEST EMAIL - " & SMTPMsg.Subject
                End If
                AddRecp("ERRORS", SMTPMsg) 'add recipients
                SMTPMsg.From.AsString = "EmailTracking"
                SMTPMsg.Send()
                'move to another folder for history 
                Account.CopyMessages(msgs.Item(0), True, "OriginalDeleted")
                Account.DeleteMessages(msgs.Item(0), True) 'mark to delete
                Account.Expunge() 'delete marked messages
            End If
        End If
        'processing for another message if one exists ************************************************************
        'if the form isn't already visible then make it visible
        'If Proc.Visible = False Then
        '    Me.Visible = False
        '    Proc.Visible = True
        '    Proc.Refresh()
        'End If
        cbCC.SelectedIndex = 0 'reset canned comment box
        'for some reason it needs to be told to search the folder again to reset itself at times
        Account.SelectFolder("ManualProcess")
        'search for another email to process
        msgs = Account.Search(True, "ALL", Nothing)
        'if a message was found then display it else give error message
        If msgs.Count = 0 Then
            MsgBox("There is currently no more e-mail to process.  Please click ""Forward/Next"" when more e-mail arrives.", MsgBoxStyle.Information, "No more e-mail to process")
            msg = Nothing 'clear message
            txtSubject.Clear()
            txtEmail.Clear()
            txtAC.Clear()
            cbSIT.SelectedIndex = 0
            txtAC.Enabled = False
            cbSIT.Enabled = False
            cbCC.Enabled = False
        Else 'if there are messages to be processed
            'get message
            msg = Account.DownloadEntireMessage(msgs.Item(0), True)
            'search through layered email until one is found from the original borrower
            If msg.Attachments.Count > 0 Then
                'while the @ sign can't be found in the FROM field of the email continue to search
                While msg.From.ToString.ToUpper.IndexOf("student loan".ToUpper) <> -1 Or _
                      msg.From.ToString.ToUpper.IndexOf("help".ToUpper) <> -1
                    If msg.Attachments.Count > 0 Then 'check if nested email has attachment
                        If msg.Attachments(0).IsMessageInside Then
                            msg = msg.Attachments(0).GetEncapsulatedMessage
                        Else
                            Exit While 'if the attachment isn't a message then exit with the previously found message
                        End If
                    End If
                End While
            End If
            'msg.Parser.HtmlToPlainMode = HtmlToPlainAutoConvert.IfHtml
            txtSubject.Text = msg.Subject.ToString()
            If msg.BodyPlainText = "" Then
                msg.MakePlainBodyFromHtmlBody()
            End If
            txtEmail.Text = msg.BodyPlainText
            txtAC.Clear() 'clear comment field
            txtAC.Enabled = True
            cbSIT.Enabled = True
            cbCC.Enabled = True
        End If
        'Proc.Visible = False
        Me.Visible = True
        cbSIT.SelectedIndex = 0
        Me.Focus()
        cbSIT.Focus()
    End Sub

    Private Sub cbCC_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCC.SelectedIndexChanged
        If txtAC.TextLength <> 0 Then
            txtAC.Text = txtAC.Text & " " & cbCC.SelectedItem
        Else
            txtAC.Text = cbCC.SelectedItem
        End If
    End Sub

    Private Sub btndelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndelete.Click
        Dim TS As New TimeSpan(0, 0, 1)
        'wait while connection is busy could be NOOPing
        While Account.IsBusy
            Thread.Sleep(TS)
        End While
        aTimer.Interval = WaitTimeSpan 'restart timer
        If vbYes = MsgBox("You have chosen to delete the displayed e-mail.  Are your sure you want to delete this e-mail.", MsgBoxStyle.YesNo, "Delete E-mail") Then
            'check if the app has a current msg
            If (msg Is Nothing) = False Then
                'Proc.Visible = True
                'Proc.Refresh()
                'move to another folder for history 
                Account.CopyMessages(msgs.Item(0), True, "OriginalDeleted")
                Account.DeleteMessages(msgs.Item(0), True) 'mark to delete
                Account.Expunge() 'delete marked messages
            End If
            'if the form isn't already visible then make it visible
            'If Proc.Visible = False Then
            '    Proc.Visible = True
            '    Proc.Refresh()
            'End If
            cbCC.SelectedIndex = 0 'reset canned comment box
            'for some reason it needs to be told to search the folder again to reset itself at times
            Account.SelectFolder("ManualProcess")
            'search for another email to process
            msgs = Account.Search(True, "ALL", Nothing)
            'if a message was found then display it else give error message
            If msgs.Count = 0 Then
                MsgBox("There is currently no more e-mail to process.  Please click ""Forward/Next"" when more e-mail arrives.", MsgBoxStyle.Information, "No more e-mail to process")
                msg = Nothing 'clear message
                txtSubject.Clear()
                txtEmail.Clear()
                txtAC.Clear()
                cbSIT.SelectedIndex = 0
                txtAC.Enabled = False
                cbSIT.Enabled = False
                cbCC.Enabled = False
            Else 'if there are messages to be processed
                'get message
                msg = Account.DownloadEntireMessage(msgs.Item(0), True)
                'search through layered email until one is found from the original borrower
                If msg.Attachments.Count > 0 Then
                    'while the @ sign can't be found in the FROM field of the email continue to search
                    While msg.From.ToString.ToUpper.IndexOf("student loan".ToUpper) <> -1 Or _
                          msg.From.ToString.ToUpper.IndexOf("help".ToUpper) <> -1
                        If msg.Attachments.Count > 0 Then 'check if nested email has attachment
                            If msg.Attachments(0).IsMessageInside Then
                                msg = msg.Attachments(0).GetEncapsulatedMessage
                            Else
                                Exit While 'if the attachment isn't a message then exit with the previously found message
                            End If
                        End If
                    End While
                End If
                msg.Parser.HtmlToPlainMode = HtmlToPlainAutoConvert.IfNoPlain
                txtSubject.Text = msg.Subject.ToString()
                txtEmail.Text = msg.BodyPlainText.ToString()
                txtAC.Clear() 'clear comment field
                txtAC.Enabled = True
                cbSIT.Enabled = True
                cbCC.Enabled = True
            End If
            'Proc.Visible = False
            Me.Visible = True
            cbSIT.SelectedIndex = 0
            Me.Focus()
            cbSIT.Focus()
        End If
    End Sub

    Private Sub MICurrentStatusReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MICurrentStatusReport.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        '<1->
        If TestMode Then
            Rpt = New Current_Status_Test
            RptFrm.ShowDialog(Rpt)
            'RptFrm.ShowDialog("C:\Program Files\E-MailTracking\Current Status Test.rpt")
        Else
            Rpt = New Current_Status
            RptFrm.ShowDialog(Rpt)
            'RptFrm.ShowDialog("C:\Program Files\E-MailTracking\Current Status.rpt")
        End If
        'RptFrm.ShowDialog("C:\Program Files\E-MailTracking\Current Status.rpt")
        '</1>
    End Sub



#Region " Deleted Stuff "
    '<1->
    ''this function decides which staff members to send email to.
    'Function FigureRecipientsAndUpdateDB(ByRef Msg As GroupwareTypeLibrary.Mail, ByVal OriginalTimeDate As String)
    '    Dim Reader As SqlDataReader 'create ref to datareader
    '    Dim SQLStr As String
    '    Dim NumInMaster As Integer
    '    Dim DBEntry As String
    '    Select Case cbSIT.SelectedItem
    '        Case "Postclaim Services"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>
    '            '<1->
    '            If Microsoft.VisualBasic.DateAndTime.Day(Today) Mod 2 = 0 Then 'if the day is an even day then send the email to Debra else to Terry
    '                Msg.BodyText.PlainText = "Debra," & vbLf & vbLf & Msg.BodyText.PlainText
    '                Msg.Recipients.Add("sdennis@utahsbr.edu")
    '                Msg.Recipients.Add("djones@utahsbr.edu")
    '                'Msg.Recipients.Add("bhill@utahsbr.edu")
    '                Msg.Recipients.Add("kjorgensen@utahsbr.edu")
    '                'update database
    '                'create master record
    '                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Debra Jones','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '                ActualConn.Open()
    '                Cmd1.ExecuteNonQuery()
    '                'get primary key from master record for Responded to insertions
    '                SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '                Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '                Reader = Cmd2.ExecuteReader() 'execute reader
    '                Reader.Read() 'move to first record in the reader
    '                NumInMaster = Reader.GetValue(0)
    '                Reader.Close()
    '                'create first Responded record
    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '                Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd3.ExecuteNonQuery()
    '                'create second Responded Record
    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Debra Jones')"
    '                Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd4.ExecuteNonQuery()
    '                ActualConn.Close()
    '            Else
    '                Msg.BodyText.PlainText = "Terry," & vbLf & vbLf & Msg.BodyText.PlainText
    '                Msg.Recipients.Add("sdennis@utahsbr.edu")
    '                Msg.Recipients.Add("tmiller@utahsbr.edu")
    '                Msg.Recipients.Add("bhill@utahsbr.edu")
    '                Msg.Recipients.Add("kjorgensen@utahsbr.edu")
    '                'update database
    '                Reader.Close()
    '                'create master record
    '                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Terry Miller','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd1.ExecuteNonQuery()
    '                'get primary key from master record for Responded to insertions
    '                SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '                Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '                Reader = Cmd2.ExecuteReader() 'execute reader
    '                Reader.Read() 'move to first record in the reader
    '                NumInMaster = Reader.GetValue(0)
    '                Reader.Close()
    '                'create first Responded record
    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '                Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd3.ExecuteNonQuery()
    '                'create second Responded Record
    '                '<1>SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Terry Miller')"
    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','" & DBEntry & "')"
    '                Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd4.ExecuteNonQuery()
    '                ActualConn.Close()
    '            End If
    '        Case "Account Services"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>
    '            Msg.BodyText.PlainText = "Dean," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("dcox@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Dean Cox','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Dean Cox')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Account Resolution"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Dennis," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("dbriggs@utahsbr.edu")
    '            'Msg.Recipients.Add("bhill@utahsbr.edu")
    '            Msg.Recipients.Add("kjorgensen@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Dennis Briggs','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Dennis Briggs')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Customer Service"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Dena," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("mriddle@utahsbr.edu")
    '            Msg.Recipients.Add("drichins@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Dena Richins','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Dena Richins')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Auxilary Services"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Richard," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("rrobbins@utahsbr.edu")
    '            Msg.Recipients.Add("bcenter@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Richard Robbins','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Richard Robbins')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Loan Management"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            If Microsoft.VisualBasic.DateAndTime.Day(Today) Mod 2 = 0 Then 'if the day is an even day then send the email to Sherri else to Christopher
    '                Msg.BodyText.PlainText = "Justin," & vbLf & vbLf & Msg.BodyText.PlainText
    '                Msg.Recipients.Add("jsjoblom@utahsbr.edu")
    '                'update database
    '                'create master record
    '                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Justin Sjoblom','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '                ActualConn.Open()
    '                Cmd1.ExecuteNonQuery()
    '                'get primary key from master record for Responded to insertions
    '                SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '                Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '                Reader = Cmd2.ExecuteReader() 'execute reader
    '                Reader.Read() 'move to first record in the reader
    '                NumInMaster = Reader.GetValue(0)
    '                Reader.Close()
    '                'create first Responded record
    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '                Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd3.ExecuteNonQuery()
    '                'create second Responded Record
    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Justin Sjoblom')"
    '                Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd4.ExecuteNonQuery()
    '                ActualConn.Close()
    '            Else
    '                Msg.BodyText.PlainText = "Chris," & vbLf & vbLf & Msg.BodyText.PlainText
    '                Msg.Recipients.Add("clund@utahsbr.edu")
    '                'update database
    '                'create master record
    '                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Christopher Lund','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '                ActualConn.Open()
    '                Cmd1.ExecuteNonQuery()
    '                'get primary key from master record for Responded to insertions
    '                SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '                Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '                Reader = Cmd2.ExecuteReader() 'execute reader
    '                Reader.Read() 'move to first record in the reader
    '                NumInMaster = Reader.GetValue(0)
    '                Reader.Close()
    '                'create first Responded record
    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '                Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd3.ExecuteNonQuery()
    '                'create second Responded Record
    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Christopher Lund')"
    '                Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '                Cmd4.ExecuteNonQuery()
    '                ActualConn.Close()
    '            End If
    '            Msg.Recipients.Add("lolney@utahsbr.edu")
    '        Case "Loan Origination"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Teri," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("coman@utahsbr.edu")
    '            Msg.Recipients.Add("tvig@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Teri Vig','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Teri Vig')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Account/Borrower"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Dawn," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("dmadsen@utahsbr.edu")
    '            'Msg.Recipients.Add("jhazelgren@utahsbr.edu")
    '            Msg.Recipients.Add("bhill@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Dawn Madsen','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Dawn Madsen')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Scholarships"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Lynda," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("lreid@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Lynda Reid','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Lynda Reid')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Nancy Caldwell"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Nancy," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("ncaldwell@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Nancy Caldwell','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Nancy Caldwell')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Cinda Eresuma"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Cinda," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("ceresuma@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Cinda Eresuma','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Cinda Eresuma')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Lynda Reid"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Lynda," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("lreid@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Lynda Reid','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Lynda Reid')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '        Case "Jed Spencer"
    '            '<1->
    '            If TestMode Then
    '                Msg.Recipients.Add("loantest@utahsbr.edu")
    '                Msg.Recipients.Add(TesterEmail)
    '            Else
    '                Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            End If
    '            'FOR TESTING ------------------------------------------------------
    '            'Msg.Recipients.Add("loantest@utahsbr.edu")
    '            'Msg.Recipients.Add("student_loan@utahsbr.edu")
    '            'END FOR TESTING --------------------------------------------------
    '            '</1>

    '            Msg.BodyText.PlainText = "Jed," & vbLf & vbLf & Msg.BodyText.PlainText
    '            Msg.Recipients.Add("jspencer@utahsbr.edu")
    '            'update database
    '            'create master record
    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & OriginalTimeDate & "','Jed Spencer','" & Msg.Subject.PlainText.Replace("'", "''") & "')"
    '            Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
    '            ActualConn.Open()
    '            Cmd1.ExecuteNonQuery()
    '            'get primary key from master record for Responded to insertions
    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
    '            Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
    '            Reader = Cmd2.ExecuteReader() 'execute reader
    '            Reader.Read() 'move to first record in the reader
    '            NumInMaster = Reader.GetValue(0)
    '            Reader.Close()
    '            'create first Responded record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & OriginalTimeDate & "','Receptionist')"
    '            Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd3.ExecuteNonQuery()
    '            'create second Responded Record
    '            SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & Msg.CreationDate & "','Jed Spencer')"
    '            Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
    '            Cmd4.ExecuteNonQuery()
    '            ActualConn.Close()
    '    End Select
    'End Function
    '</1>
#End Region

    Private Sub MIDirectToAuxSer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MIDirectToAuxSer.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        If TestMode Then
            Rpt = New AuxiliaryServicesDirectEmail_Test
            RptFrm.ShowDialog(Rpt)
        Else
            Rpt = New AuxiliaryServicesDirectEmail
            RptFrm.ShowDialog(Rpt)
        End If
    End Sub

    Private Sub MIDirectToAcctSer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MIDirectToAcctSer.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        If TestMode Then
            Rpt = New AccountServicesDirectEmail_Test
            RptFrm.ShowDialog(Rpt)
        Else
            Rpt = New AccountServicesDirectEmail
            RptFrm.ShowDialog(Rpt)
        End If
    End Sub

    Private Sub MIDirectToLoanOrig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MIDirectToLoanOrig.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        If TestMode Then
            Rpt = New LoanOriginationDirectEmail_Test
            RptFrm.ShowDialog(Rpt)
        Else
            Rpt = New LoanOriginationDirectEmail
            RptFrm.ShowDialog(Rpt)
        End If
    End Sub

    Private Sub MonthEndBUTotals_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MonthEndBUTotals.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        If TestMode Then
            Rpt = New MonthEndBUTotalsAndPrecentages_Test
            RptFrm.ShowDialog(Rpt)
        Else
            Rpt = New MonthEndBUTotalsAndPrecentages
            RptFrm.ShowDialog(Rpt)
        End If
    End Sub

    Private Sub MonthEndResponderTotals_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MonthEndResponderTotals.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        If TestMode Then
            Rpt = New ResponderTotalsAndPercentages_Test
            RptFrm.ShowDialog(Rpt)
        Else
            Rpt = New ResponderTotalsAndPercentages
            RptFrm.ShowDialog(Rpt)
        End If
    End Sub

    Private Sub MonthEnd_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MonthEnd.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        '<1->
        If TestMode Then
            Rpt = New MonthEndForRespTracking_Test
            'RptFrm.ShowDialog("C:\Program Files\E-MailTracking\MonthEndForRespTracking Test.rpt")
            RptFrm.ShowDialog(Rpt)
        Else
            Rpt = New MonthEndForRespTracking
            'RptFrm.ShowDialog("C:\Program Files\E-MailTracking\MonthEndForRespTracking.rpt")
            RptFrm.ShowDialog(Rpt)
        End If
        'RptFrm.ShowDialog("C:\Program Files\E-MailTracking\MonthEndForRespTracking.rpt")
        '</1>
    End Sub

    Private Sub MIQCReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MIQCReport.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        If TestMode Then
            Rpt = New QCReport_Test
            RptFrm.ShowDialog(Rpt)
        Else
            Rpt = New QCReport
            RptFrm.ShowDialog(Rpt)
        End If
    End Sub

    Private Sub MIBUPersCopied_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MIBUPersCopied.Click
        Dim RptFrm As New frmReports
        Dim Rpt As Object
        If TestMode Then
            Rpt = New EmailGroups_Test
            RptFrm.ShowDialog(Rpt)
        Else
            Rpt = New EmailGroups
            RptFrm.ShowDialog(Rpt)
        End If
    End Sub

    Function AddRecp(ByVal Group As String, ByRef msg As Smtp) As String
        Dim SQLstr As String = "SELECT * From EmailGroups WHERE TheGroup LIKE '" & Group & "'"
        Dim Cmd5 As New SqlCommand(SQLstr, ActualConn)
        Dim Reader As SqlDataReader 'create ref to datareader
        Dim SendTo As String
        ActualConn.Open()
        Reader = Cmd5.ExecuteReader() 'execute reader
        Reader.Read() 'move to first record in the reader
        SendTo = Reader.GetString(3) 'add main recipient
        AddRecp = Reader.GetString(4)
        'add all CCs
        If Reader.GetString(5) <> "Nothing" Then SendTo = SendTo & ";" & Reader.GetString(5)
        If Reader.GetString(6) <> "Nothing" Then SendTo = SendTo & ";" & Reader.GetString(6)
        If Reader.GetString(7) <> "Nothing" Then SendTo = SendTo & ";" & Reader.GetString(7)
        If Reader.GetString(8) <> "Nothing" Then SendTo = SendTo & ";" & Reader.GetString(8)
        If Reader.GetString(9) <> "Nothing" Then SendTo = SendTo & ";" & Reader.GetString(9)
        If TestMode Then
            SendTo = SendTo & ";" & TesterEmail
        End If
        'for GW to Exchange functionality
        'SendTo = SendTo.Replace("@", "@test.")
        msg.To.AsString = SendTo
        msg.From.AsString = "EmailTracking"
        Reader.Close()
        ActualConn.Close()
    End Function

    Private Sub frmEmailTracking_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        'if being run for batch processing don't show the form
        If BatchProcessingMode Then
            Me.Visible = False 'don't show form if batch processing is taking place
        End If
    End Sub
End Class

'<1>, sr835, aa, 10/28/04, 11/08/04
'<2>, sr851, aa, 11/10/04, 11/10/04
'<3>, sr898, aa, 11/14/04, 12/30/05
'<4>, sr1023, aa, 04/19/05, 06/01/05
'<5>, sr1242, aa