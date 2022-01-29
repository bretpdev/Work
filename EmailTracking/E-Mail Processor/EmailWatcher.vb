Imports System.Data.SqlClient
Imports MailBee.ImapMail
Imports MailBee
Imports MailBee.Mime
Imports MailBee.SmtpMail

Public Class EmailWatcher

	Private UserNotified As Boolean = False
	Private Notify As New frmNotification
	Private TestMode As Boolean
	Private ActualConn As SqlConnection
	Private CounterList As ArrayList
	Private TEM As String
	Private EmailWatchingThread As Threading.Thread
	Private Account As Imap
	Private LKey As String
	Private SMTPLKey As String
	Private Server As String
	Private UID As String
	Private PW As String
	Private OwnerOfAccount As String
	Private DialogBoxThread As Threading.Thread
	Private MonitorThread As Threading.Thread
	Private WaitTimeSpan As Double = 1800000 'wait time for timer 30 minutes
	'Private WaitTimeSpan As Double = 10000 'wait time for timer 30 minutes
	Private aTimer As System.Timers.Timer
	Private ProcCheck As New ProcessedObj



	Sub New(ByVal TTestMode As Boolean, ByVal TesterEmail As String, ByVal TAccID As String, ByVal TPW As String, ByVal ConnStr As String, ByVal tOwnerOfAccount As String, ByVal tLKey As String, ByVal tSMTPLKey As String, ByVal tServer As String, ByVal BatchProcessingMode As Boolean)
		Server = tServer
		LKey = tLKey
		SMTPLKey = tSMTPLKey
		MailBee.Global.LicenseKey = LKey
		'Imap.LicenseKey = LKey
		'Smtp.LicenseKey = SMTPLKey
		Account = New Imap
		Account.Connect(Server, 143) 'set up server and port info
		Account.Login(TAccID, TPW)
		Account.SelectFolder("Inbox")
		TEM = TesterEmail
		TestMode = TTestMode
		UID = TAccID
		PW = TPW
		OwnerOfAccount = tOwnerOfAccount
		'decide which DB to communicate with
		ActualConn = New SqlConnection(ConnStr)
		LoadCounterList() 'load QC lists
		If BatchProcessingMode Then
			'for batch processing
			EmailWatchingThread = New Threading.Thread(AddressOf Proc)
			EmailWatchingThread.IsBackground = True
			EmailWatchingThread.Start()
			MonitorThread = New Threading.Thread(AddressOf WatcherMonitor)
			MonitorThread.IsBackground = True
			MonitorThread.Start()
		Else
			'for all other users
			EmailWatchingThread = New Threading.Thread(AddressOf Notification)
			EmailWatchingThread.IsBackground = True
			EmailWatchingThread.Start()
			'set up keep alive handler
			aTimer = New System.Timers.Timer
			AddHandler aTimer.Elapsed, AddressOf ResetUserNotified
			aTimer.Enabled = True
			aTimer.Interval = WaitTimeSpan '30 Minutes
			aTimer.Start() 'start timer
		End If
	End Sub

	'timer elapsed event handler
	Sub ResetUserNotified(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
		UserNotified = False
	End Sub

	'<4->

	Sub Proc()
		Dim msg As MailMessage	' a message
		Dim msgs As UidCollection 'message collection
		Dim SMTPMsg As New Smtp	'create SMTP object
		SMTPMsg.SmtpServers.Clear()
		SMTPMsg.DnsServers.Clear()
		SMTPMsg.SmtpServers.Add(Server)	'add smtp server
		Dim TS As New TimeSpan(0, 1, 0)	'for processing pause
		Dim SQLStr As String 'for SQL Statements
		Dim OriginalTimeDate As String
		Dim WorkingMsgTimeDate As String
		Dim Reader As SqlDataReader	'for data reading
		Dim NumInMaster As Integer
		Dim QCGroup As String
		Dim SlotFound As Boolean = False
		Dim Recp As String
		Dim I As Integer
		Dim Cmd1 As SqlCommand
		Dim Cmd2 As SqlCommand
		Dim Cmd3 As SqlCommand
		Dim Cmd4 As SqlCommand
		Dim Cmd5 As New SqlCommand
		Dim EndTime As DateTime
		Dim StartTime As New DateTime(Now.Year, Now.Month, Now.Day, 6, 0, 0)
		Dim CurrentDate As New Date

		CurrentDate = Date.Now
		If CurrentDate.DayOfWeek = DayOfWeek.Friday Then
			EndTime = New DateTime(Now.Year, Now.Month, Now.Day, 17, 0, 0)
		Else
            EndTime = New DateTime(Now.Year, Now.Month, Now.Day, 19, 0, 0)
		End If
		If Now < StartTime Then
			Threading.Thread.Sleep(StartTime.Subtract(Now))	'sleep until 6:00 AM
		End If
		Cmd5.Connection = ActualConn
		While 1	'cycle until the parent script ends the thread
			ActualConn.Open()
			Account.SelectFolder("Inbox")
			'get all messages in inbox
			msgs = Account.Search(True, "ALL", Nothing)
			I = 0 'restart counter
			'Process for each message category
			While msgs.Count <> I
				msg = Account.DownloadEntireMessage(msgs(I), True) 'get message
				'*********************************************messages forwarded from BU email accounts*********************************************
				If msg.To.ToString.ToUpper.IndexOf("UHEAAHELP@UTAHSBR.EDU".ToUpper) <> -1 Or msg.To.ToString.ToUpper.IndexOf("UHEAALOCATE@UTAHSBR.EDU".ToUpper) <> -1 Or _
				 msg.To.ToString.ToUpper.IndexOf("UHEAASUPPORT@UTAHSBR.EDU".ToUpper) <> -1 Or msg.To.ToString.ToUpper.IndexOf("UHEAACUSTOMERSERVICE@UTAHSBR.EDU".ToUpper) <> -1 Then
					Dim Group As String = ""
					If msg.To.ToString.ToUpper.IndexOf("UHEAAHELP@UTAHSBR.EDU".ToUpper) <> -1 Then
						Group = "UHEAAHELP"
					ElseIf msg.To.ToString.ToUpper.IndexOf("UHEAALOCATE@UTAHSBR.EDU".ToUpper) <> -1 Then
						Group = "UHEAALOCATE"
					ElseIf msg.To.ToString.ToUpper.IndexOf("UHEAASUPPORT@UTAHSBR.EDU".ToUpper) <> -1 Then
						Group = "UHEAASUPPORT"
					ElseIf msg.To.ToString.ToUpper.IndexOf("UHEAACUSTOMERSERVICE@UTAHSBR.EDU".ToUpper) <> -1 Then
						Group = "UHEAACUSTOMERSERVICE"
					End If
					'send on to specified staff depending on which account it wend to
					SMTPMsg.Message = msg.ForwardAsAttachment()
					Recp = AddRecp(Group, SMTPMsg)
					If TestMode Then
						SMTPMsg.Subject = "TEST EMAIL - " & msg.Subject
					Else
						SMTPMsg.Subject = msg.Subject

					End If
					SMTPMsg.Send() 'send email
					'move message to another folder for a history
					Account.CopyMessages(msg.UidOnServer, True, "OriginalDeleted")
					Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
				Else
					'******************************************undeliverable mail**************************************************
                    If msg.Subject.ToLower().Contains("delivery notification") Then
                        'create SMTP message
                        SMTPMsg.Message = msg.ForwardAsAttachment()
                        Recp = AddRecp("Undeliverable Mail", SMTPMsg)
                        If TestMode Then
                            SMTPMsg.Subject = "TEST EMAIL - " & "The E-mail Tracking System received this undeliverable message back."
                        Else
                            SMTPMsg.Subject = "The E-mail Tracking System received this undeliverable message back."
                        End If
                        SMTPMsg.Send()
                        'move message to another folder for a history
                        Account.CopyMessages(msg.UidOnServer, True, "OriginalDeleted")
                        Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                    End If
					''**************************************Process loanhelp emails first********************************************************
					'*****************************change of addrs
					If msg.Subject.ToUpper.IndexOf("Change of Address and Telephone Information".ToUpper) <> -1 And _
					   msg.From.ToString.ToUpper.IndexOf("help".ToUpper()) <> -1 Then
						QCCopying("Change of Address and Telephone Information", msg, TEM)
						'create SMTP message
						SMTPMsg.Message = msg.ForwardAsAttachment()
						Recp = AddRecp("Change of Address and Telephone Information", SMTPMsg)
						'Check for duplicate records
						WorkingMsgTimeDate = msg.Date.ToString("MM/dd/yyyy hh:mm:ss tt")
						SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
						Cmd5.CommandText = SQLStr
						While SlotFound = False
							Reader = Cmd5.ExecuteReader()
							If Reader.Read() = False Then
								SlotFound = True
							Else
								WorkingMsgTimeDate = CStr(DateAdd(DateInterval.Second, 1, CDate(WorkingMsgTimeDate)))
							End If
							Reader.Close()
							SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
							Cmd5.CommandText = SQLStr
						End While
						SlotFound = False
						'update database
						'create master record
						SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsgTimeDate & "','" & Recp & "','" & msg.Subject.Replace("'", "''") & "')"
						Cmd1 = New SqlCommand(SQLStr, ActualConn)
						Cmd1.ExecuteNonQuery()
						If TestMode Then
							SMTPMsg.Subject = "TEST EMAIL - " & msg.Subject
						Else
							SMTPMsg.Subject = msg.Subject
						End If
						SMTPMsg.Send() 'send email
						'move message to another folder for a history
						Account.CopyMessages(msg.UidOnServer, True, "OriginalDeleted")
						Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
					Else
						''************************stafford loan decrease
						If msg.Subject.ToUpper.IndexOf("stafford loan decrease".ToUpper) <> -1 And _
						   msg.From.ToString.ToUpper.IndexOf("help".ToUpper()) <> -1 Then
							QCCopying("Stafford Loan Decrease", msg, TEM)
							'create SMTP message
							SMTPMsg.Message = msg.ForwardAsAttachment()
							Recp = AddRecp("Stafford Loan Decrease", SMTPMsg)
							'Check for duplicate records
							WorkingMsgTimeDate = msg.Date.ToString("MM/dd/yyyy hh:mm:ss tt")
							SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
							Cmd5.CommandText = SQLStr
							While SlotFound = False
								Reader = Cmd5.ExecuteReader()
								If Reader.Read() = False Then
									SlotFound = True
								Else
									WorkingMsgTimeDate = CStr(DateAdd(DateInterval.Second, 1, CDate(WorkingMsgTimeDate)))
								End If
								Reader.Close()
								SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
								Cmd5.CommandText = SQLStr
							End While
							SlotFound = False
							'update database
							'create master record
							SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsgTimeDate & "','" & Recp & "','" & msg.Subject.Replace("'", "''") & "')"
							Cmd1 = New SqlCommand(SQLStr, ActualConn)
							Cmd1.ExecuteNonQuery()
							If TestMode Then
								SMTPMsg.Subject = "TEST EMAIL - " & msg.Subject
							Else
								SMTPMsg.Subject = msg.Subject
							End If
							SMTPMsg.Send() 'send email
							'move message to another folder for a history
							Account.CopyMessages(msg.UidOnServer, True, "OriginalDeleted")
							Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
						Else
							''************************Plus PreApproval Comments
							If msg.Subject.ToUpper.IndexOf("Plus PreApproval Comments".ToUpper) <> -1 And _
							  msg.From.ToString.ToUpper.IndexOf("help".ToUpper()) <> -1 Then
								QCCopying("Plus PreApproval Comments", msg, TEM)
								'create SMTP message
								SMTPMsg.Message = msg.ForwardAsAttachment()
								Recp = AddRecp("Plus PreApproval Comments", SMTPMsg)
								'Check for duplicate records
								WorkingMsgTimeDate = msg.Date.ToString("MM/dd/yyyy hh:mm:ss tt")
								SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
								Cmd5.CommandText = SQLStr
								While SlotFound = False
									Reader = Cmd5.ExecuteReader()
									If Reader.Read() = False Then
										SlotFound = True
									Else
										WorkingMsgTimeDate = CStr(DateAdd(DateInterval.Second, 1, CDate(WorkingMsgTimeDate)))
									End If
									Reader.Close()
									SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
									Cmd5.CommandText = SQLStr
								End While
								SlotFound = False
								'update database
								'create master record
								SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsgTimeDate & "','" & Recp & "','" & msg.Subject.Replace("'", "''") & "')"
								Cmd1 = New SqlCommand(SQLStr, ActualConn)
								Cmd1.ExecuteNonQuery()
								If TestMode Then
									SMTPMsg.Subject = "TEST EMAIL - " & msg.Subject
								Else
									SMTPMsg.Subject = msg.Subject
								End If
								SMTPMsg.Send() 'send email
								'move message to another folder for a history
								Account.CopyMessages(msg.UidOnServer, True, "OriginalDeleted")
								Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
							Else
								'******************************************Online Billing***********************************************************************
                                If (msg.Subject.ToUpper.IndexOf("Update Billing".ToUpper) <> -1 Or _
                                   msg.Subject.ToUpper.IndexOf("Billing Option Change Notification".ToUpper) <> -1) AndAlso _
                                   (Not msg.From.ToString().ToUpper().Contains("DONOTREPLY@MYCORNERSTONELOAN.ORG")) Then

                                    QCCopying("Online Billing", msg, TEM)
                                    'create SMTP message
                                    SMTPMsg.Message = msg.ForwardAsAttachment()
                                    Recp = AddRecp("Online Billing", SMTPMsg)
                                    'Check for duplicate records
                                    WorkingMsgTimeDate = msg.Date.ToString("MM/dd/yyyy hh:mm:ss tt")
                                    SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
                                    Cmd5.CommandText = SQLStr
                                    While SlotFound = False
                                        Reader = Cmd5.ExecuteReader()
                                        If Reader.Read() = False Then
                                            SlotFound = True
                                        Else
                                            WorkingMsgTimeDate = CStr(DateAdd(DateInterval.Second, 1, CDate(WorkingMsgTimeDate)))
                                        End If
                                        Reader.Close()
                                        SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
                                        Cmd5.CommandText = SQLStr
                                    End While
                                    SlotFound = False
                                    'update database
                                    'create master record
                                    SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsgTimeDate & "','" & Recp & "','" & msg.Subject.Replace("'", "''") & "')"
                                    Cmd1 = New SqlCommand(SQLStr, ActualConn)
                                    Cmd1.ExecuteNonQuery()
                                    If TestMode Then
                                        SMTPMsg.Subject = "TEST EMAIL - " & msg.Subject
                                    Else
                                        SMTPMsg.Subject = msg.Subject
                                    End If
                                    SMTPMsg.Send() 'send email
                                    'move message to another folder for a history
                                    Account.CopyMessages(msg.UidOnServer, True, "OriginalDeleted")
                                    Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                Else
                                    '**********************************************************************************
                                    If msg.Subject.ToUpper.IndexOf("UHEAA Requests Your Assistance".ToUpper) <> -1 Then
                                        QCCopying("Requests Assistance", msg, TEM)
                                        'create SMTP message
                                        SMTPMsg.Message = msg.ForwardAsAttachment()
                                        Recp = AddRecp("Requests Assistance", SMTPMsg)
                                        'Check for duplicate records
                                        WorkingMsgTimeDate = msg.Date.ToString("MM/dd/yyyy hh:mm:ss tt")
                                        SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
                                        Cmd5.CommandText = SQLStr
                                        While SlotFound = False
                                            Reader = Cmd5.ExecuteReader()
                                            If Reader.Read() = False Then
                                                SlotFound = True
                                            Else
                                                WorkingMsgTimeDate = CStr(DateAdd(DateInterval.Second, 1, CDate(WorkingMsgTimeDate)))
                                            End If
                                            Reader.Close()
                                            SQLStr = "SELECT * From Master WHERE CreationTimeDateStamp = '" & WorkingMsgTimeDate & "'"
                                            Cmd5.CommandText = SQLStr
                                        End While
                                        SlotFound = False
                                        'update database
                                        'create master record
                                        SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsgTimeDate & "','" & Recp & "','" & msg.Subject.Replace("'", "''") & "')"
                                        Cmd1 = New SqlCommand(SQLStr, ActualConn)
                                        Cmd1.ExecuteNonQuery()
                                        If TestMode Then
                                            SMTPMsg.Subject = "TEST EMAIL - " & msg.Subject
                                        Else
                                            SMTPMsg.Subject = msg.Subject
                                        End If
                                        SMTPMsg.Send() 'send email
                                        'move message to another folder for a history
                                        Account.CopyMessages(msg.UidOnServer, True, "OriginalDeleted")
                                        Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                    Else

                                        '*********************************************SPAM Reports**********************************************************************
                                        If msg.Subject.ToUpper.IndexOf("Spam Quarantine Summary".ToUpper) <> -1 Then
                                            'create SMTP message
                                            SMTPMsg.Message = msg.ForwardAsAttachment()
                                            Recp = AddRecp("SPAM Reports", SMTPMsg)
                                            If TestMode Then
                                                SMTPMsg.Subject = "TEST EMAIL - " & msg.Subject
                                            Else
                                                SMTPMsg.Subject = msg.Subject
                                            End If
                                            SMTPMsg.Send() 'send email
                                            'move message to another folder for a history
                                            Account.CopyMessages(msg.UidOnServer, True, "OriginalDeleted")
                                            Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                        Else
                                            '********************************************Messages forwarded back to Student Loan from Student Loan**************************
                                            If msg.From.ToString.ToUpper.IndexOf("emailtracking".ToUpper) <> -1 Then
                                                'move message to another folder for a history
                                                Account.CopyMessages(msg.UidOnServer, True, "IntermediateDeleted")
                                                Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                            ElseIf msg.From.ToString().ToUpper().Contains("DONOTREPLY@MYCORNERSTONELOAN.ORG") Then
                                                Account.CopyMessages(msg.UidOnServer, True, "CSError")
                                                Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete

                                            Else
                                                '*******************************************Messages forwarded back to Student Loan from the Business Units***********************
                                                If msg.From.ToString.ToUpper.IndexOf("@utahsbr.edu".ToUpper) <> -1 And _
                                                   msg.From.ToString.ToUpper.IndexOf("loanhelp".ToUpper) = -1 Then
                                                    'if the magical "--" doesn't appear in the message then send the e-mail to Kelli Page and delete the one in the inbox
                                                    If msg.Subject.IndexOf("--") > -1 Then
                                                        'split out time date stamp of original email message
                                                        If msg.Subject.IndexOf("--") = 0 Then
                                                            'mid function must start at 1
                                                            OriginalTimeDate = Mid(msg.Subject, msg.Subject.IndexOf("--") + 1, 25)
                                                            OriginalTimeDate = OriginalTimeDate.Substring(3)
                                                        Else
                                                            OriginalTimeDate = Mid(msg.Subject, msg.Subject.IndexOf("--"), 26)
                                                            OriginalTimeDate = OriginalTimeDate.Substring(4)
                                                        End If
                                                        If IsDate(OriginalTimeDate) = False Then
                                                            'error email is not time date stamp
                                                            SMTPMsg.Message = msg.ForwardAsAttachment()
                                                            Recp = AddRecp("ERRORS", SMTPMsg)
                                                            SMTPMsg.BodyPlainText = "The E-mail Tracking System was unable to handle this e-mail."
                                                            If TestMode Then
                                                                SMTPMsg.Subject = "TEST EMAIL - " & "Email Tracking ERROR"
                                                            Else
                                                                SMTPMsg.Subject = "Email Tracking ERROR"
                                                            End If
                                                            SMTPMsg.Send()
                                                        Else
                                                            'add to database (third message in RespondedToEmail Table)
                                                            SQLStr = "SELECT Num, Recipient FROM [Master] WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
                                                            Cmd2 = New SqlCommand(SQLStr, ActualConn)
                                                            Reader = Cmd2.ExecuteReader() 'execute reader
                                                            Reader.Read() 'move to first record in the reader
                                                            NumInMaster = Reader.GetValue(0)
                                                            QCGroup = Reader.GetString(1)
                                                            Reader.Close()
                                                            'check if there are three records, if so then just delete the e-mail
                                                            SQLStr = "SELECT COUNT(*) as X FROM RespondedToEmail WHERE MasterNum = " & NumInMaster
                                                            Cmd4 = New SqlCommand(SQLStr, ActualConn)
                                                            Reader = Cmd4.ExecuteReader() 'execute reader
                                                            Reader.Read()
                                                            If Reader.GetValue(0) <> 3 Then
                                                                'create first Responded record
                                                                Reader.Close()
                                                                SQLStr = "INSERT INTO RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient,Responder) Values(" & NumInMaster & " ,'" & msg.Date & "','BORROWER','" & msg.From.AsString & "')"
                                                                Cmd3 = New SqlCommand(SQLStr, ActualConn)
                                                                Cmd3.ExecuteNonQuery()
                                                                Cmd3.Dispose()
                                                                QCCopying(QCGroup, msg, TEM)
                                                            Else
                                                                Reader.Close()
                                                            End If
                                                            Cmd2.Dispose()
                                                            Cmd4.Dispose()
                                                        End If
                                                        'move message to another folder for a history
                                                        Account.CopyMessages(msg.UidOnServer, True, "FinalDeleted")
                                                        Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                                    Else
                                                        'Emails that come from UHEAA.org are sent from Internal email Contact Us
                                                        If msg.Subject.ToString.Contains("Contact message via uheaa.org") Then
                                                            Account.CopyMessages(msg.UidOnServer, True, "ManualProcess")
                                                            Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                                        Else
                                                            'sent from internal source but there is no time date stamp in the subject line 
                                                            'create SMTP message
                                                            SMTPMsg.Message = msg.ForwardAsAttachment()
                                                            Recp = AddRecp("ERRORS", SMTPMsg)
                                                            SMTPMsg.BodyPlainText = "The E-mail Tracking System was unable to handle this e-mail."
                                                            If TestMode Then
                                                                SMTPMsg.Subject = "TEST EMAIL - " & "Email Tracking ERROR"
                                                            Else
                                                                SMTPMsg.Subject = "Email Tracking ERROR"
                                                            End If
                                                            SMTPMsg.Send()
                                                        End If
                                                        'move message to another folder for a history
                                                        Account.CopyMessages(msg.UidOnServer, True, "FinalDeleted")
                                                        Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                                    End If
                                                Else
                                                    ''*******************************************Notification for Recp*****************************************************************
                                                    Account.CopyMessages(msg.UidOnServer, True, "ManualProcess")
                                                    Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                                    'If ((msg.From.ToString.ToUpper.IndexOf("HELP".ToUpper) <> -1 And _
                                                    '   msg.Subject.ToUpper.IndexOf("Change of Address and Telephone Information".ToUpper) = -1 And _
                                                    '   msg.Subject.ToUpper.IndexOf("Stafford Loan Decrease".ToUpper) = -1 And _
                                                    '   msg.Subject.ToUpper.IndexOf("PLUS Preapproval Comments".ToUpper) = -1) Or _
                                                    '   ((msg.To.ToString.ToUpper.IndexOf("uheaa@utahsbr.edu".ToUpper) <> -1 Or _
                                                    '   msg.To.ToString.ToUpper.IndexOf("studentloan@utahsbr.edu".ToUpper) <> -1 Or _
                                                    '   msg.To.ToString.ToUpper.IndexOf("loantest".ToUpper) <> -1) And _
                                                    '   msg.Subject.ToUpper.IndexOf("Update Billing".ToUpper) = -1 And _
                                                    '   msg.Subject.ToUpper.IndexOf("UHEAA Bill Reminder".ToUpper) = -1) Or _
                                                    '   msg.From.ToString.ToUpper.IndexOf("Forward webaddress".ToUpper) <> -1 Or _
                                                    '   msg.From.ToString.ToUpper.IndexOf("LPP HELP".ToUpper) <> -1 Or _
                                                    '   msg.From.ToString.ToUpper.IndexOf("student loan".ToUpper) <> -1) And _
                                                    '   msg.Subject.ToUpper.IndexOf("Spam Quarantine Summary".ToUpper) = -1 Then
                                                    '    'move message to another folder for manual process to pick up
                                                    '    Account.CopyMessages(msg.UidOnServer, True, "ManualProcess")
                                                    '    Account.DeleteMessages(msgs(I).ToString(), True) 'mark to delete
                                                    'Else
                                                    '    'error
                                                    '    MsgBox("Email Tracking has encountered and error.  Please contact Systems Support.")
                                                    '    End
                                                    'End If

                                                End If
                                            End If
                                        End If
                                    End If
                                End If
							End If
						End If
					End If
				End If
				Account.Expunge() 'throw away all marked for delete email
				I = I + 1
			End While
			''check for messages that manual process will need to handle 
			'Account.SelectFolder("ManualProcess")
			'msgs = Account.Search(True, "ALL", Nothing)
			'If msgs.Count > 0 Then
			'    Notify.NotifyIcon.Visible = True
			'    If UserNotified = False And Notify.Visible = False Then
			'        DialogBoxThread = New Threading.Thread(AddressOf DisplayDialog)
			'        DialogBoxThread.IsBackground = True
			'        DialogBoxThread.Start()
			'    End If
			'    UserNotified = True
			'Else
			'    Notify.NotifyIcon.Visible = False
			'    Notify.Hide()
			'    UserNotified = False
			'End If
			msgs = Nothing
			ActualConn.Close()
			If Now > EndTime Then
				End	'end processing if after 5:00 PM
			End If
			Threading.Monitor.Enter(ProcCheck) 'lock object for multi-threading
			ProcCheck.Processed = True
			Threading.Monitor.Exit(ProcCheck) 'unlock object for multi-threading
			Threading.Thread.Sleep(TS)
		End While
	End Sub

	Sub Notification()
		Dim msgs As UidCollection 'message collection
		'check for messages that manual process will need to handle 
		Account.SelectFolder("ManualProcess")
		While 1	'endless loop
			msgs = Account.Search(True, "ALL", Nothing)
			If msgs.Count > 0 Then
				Notify.NotifyIcon.Visible = True
				If UserNotified = False And Notify.Visible = False Then
					DialogBoxThread = New Threading.Thread(AddressOf DisplayDialog)
					DialogBoxThread.IsBackground = True
					DialogBoxThread.Start()
				End If
				UserNotified = True
			Else
				Notify.NotifyIcon.Visible = False
				Notify.Hide()
				UserNotified = False
			End If
			'sleep for one second
			Threading.Thread.Sleep(New TimeSpan(0, 1, 0))
		End While
	End Sub

	Function AddRecp(ByVal Group As String, ByRef msg As Smtp) As String
		Dim SQLstr As String = "SELECT * From EmailGroups WHERE TheGroup LIKE '" & Group & "'"
		Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(ActualConn.ConnectionString)
		Dim Cmd5 As New SqlCommand(SQLstr, Conn)
		Dim Reader As SqlDataReader	'create ref to datareader
		Dim SendTo As String
		Conn.Open()
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
			SendTo = SendTo & ";" & TEM
		End If
		'for GW to Exchange functionality
		'SendTo = SendTo.Replace("@", "@test.")
		msg.To.AsString = SendTo
		msg.From.AsString = "EmailTracking"
		Reader.Close()
		Conn.Close()
	End Function


	'decides if the msg should be forwarded and forwards the email if is should be QCd
	Private Sub QCCopying(ByVal Group As String, ByVal Msg As MailMessage, Optional ByVal Tester As String = "")
		Dim SQLStr As String = "SELECT * FROM QCGroups Where [DBEntry/SubjectLine] LIKE '" & Group & "'"
		Dim Comm As New SqlCommand(SQLStr, ActualConn)
		Dim Reader As SqlDataReader
		Dim counter As Integer = 3
		Dim ALCounter As Integer
		Dim QCMsg As New Smtp 'create SMTP object
		QCMsg.SmtpServers.Add(Server) 'add smtp server
		'find matching counter
		While CounterList.Item(ALCounter)(0) <> Group
			ALCounter = ALCounter + 1
			If ALCounter = CounterList.Count Then
				Exit Sub 'don't send QC email if not listed in QC groups table
			End If
		End While
		CounterList.Item(ALCounter)(1) = CounterList.Item(ALCounter)(1) + 1
		'send QC msg if counter and Ratio =
		If CounterList.Item(ALCounter)(1) = CounterList.Item(ALCounter)(2) Then
			CounterList.Item(ALCounter)(1) = 0
			QCMsg.Message = Msg.ForwardAsAttachment()
			QCMsg.From.AsString = "EmailTracking"
			Reader = Comm.ExecuteReader
			Reader.Read() 'gather QC forwarding info from DB
			If Reader.GetString(0) = "Grammar" Then
				QCMsg.Subject = "QC1 " & Msg.Subject
			Else
				QCMsg.Subject = "QC2 " & Msg.Subject
			End If
			While counter < 12
				'if the field contains a email address then add the address to the email to be forwarded
				If Reader.GetString(counter) <> "Nothing" Then
					If QCMsg.To.AsString = "" Then
						QCMsg.To.AsString = Reader.GetString(counter) 'add name to
					Else
						QCMsg.To.AsString = QCMsg.To.AsString & ";" & Reader.GetString(counter)	'add name to
					End If
				End If
				counter = counter + 1
			End While
			'add tester to recipient list if app is in test mode
			If Tester <> "" Then
				QCMsg.To.AsString = QCMsg.To.AsString & ";" & Tester
				QCMsg.Subject = "TEST EMAIL - " & QCMsg.Subject
			End If
			'for GW to Exchange functionality
			'QCMsg.To.AsString = QCMsg.To.ToString.Replace("@", "@test.")
			QCMsg.Send()
			Reader.Close()
		End If
	End Sub

	'loads counter information
	Private Sub LoadCounterList()
		Dim SQLstr As String = "SELECT [DBEntry/SubjectLine], OneInWhatRatio FROM QCGroups"
		Dim Comm As New SqlCommand(SQLstr, ActualConn)
		Dim Reader As SqlDataReader
		Dim TempList As ArrayList
		CounterList = New ArrayList
		ActualConn.Open()
		Reader = Comm.ExecuteReader
		'create and array list to house the counters and applicable information about the counter
		While Reader.Read
			'create new temp array
			TempList = New ArrayList
			'collect data elements
			TempList.Add(Reader.GetString(0))
			TempList.Add(0)
			TempList.Add(Reader.GetValue(1))
			CounterList.Add(TempList) 'add elements to master list
		End While
		Reader.Close()
		ActualConn.Close()
	End Sub

	Private Sub DisplayDialog()
		Notify.ShowDialog()
		aTimer.Interval = WaitTimeSpan 'start timer over
		Notify.StartPosition = FormStartPosition.Manual
	End Sub

	'this sub monitors the watcher thread to be sure that it is working
	Private Sub WatcherMonitor()
		'Dim msg As MailMessage  ' a message
		Dim SMTPMsg As New Smtp	'create SMTP object
		Dim TS As New TimeSpan(0, 5, 0)
		SMTPMsg.SmtpServers.Clear()
		SMTPMsg.DnsServers.Clear()
		SMTPMsg.SmtpServers.Add(Server)	'add smtp server
		Dim EightOClock As New DateTime(Now.Year, Now.Month, Now.Day, 8, 0, 0)
		Dim FiveOClock As New DateTime(Now.Year, Now.Month, Now.Day, 17, 0, 0)
		If Now < EightOClock Then
			Threading.Thread.Sleep(EightOClock.Subtract(Now)) 'sleep until 8:00 AM
		End If
		While 1
			Threading.Thread.Sleep(TS) 'sleep for five minutes
			If Now > FiveOClock Then
				'end monitoring if after 5:00 PM
				End
			End If
			Threading.Monitor.Enter(ProcCheck) 'lock object for multi-threading
			If ProcCheck.Processed = False Then
				SMTPMsg.Message = New MailMessage
				AddRecp("Email Tracking Plug Up", SMTPMsg) 'add recipients
				SMTPMsg.Message.Subject = "Email Tracking Processing Problem"
				SMTPMsg.Message.BodyPlainText = "Email Tracking is no longer distributing email.  Please investigate."
				If TestMode Then
					SMTPMsg.Message.Subject = "TEST EMAIL - " & SMTPMsg.Message.Subject
					SMTPMsg.Message.BodyPlainText = "TEST EMAIL - " & vbLf & SMTPMsg.Message.BodyPlainText
				End If
				SMTPMsg.Message.Priority = MailPriority.Highest
				SMTPMsg.Message.From.AsString = "EmailTrackingMonitoringProcess"
				SMTPMsg.Send()
			End If
			ProcCheck.Processed = False	'reset for another five minutes
			Threading.Monitor.Exit(ProcCheck) 'unlock object for multi-threading
		End While
	End Sub

#Region " Deleted Stuff "
	'Sub Main()
	'    '<1>   Dim Log As New Login
	'    Dim TesterEmail As String = ""                                  '<1>
	'    Log = New Login                                             '<1>
	'    Log.ShowDialog() 'get login info
	'    Try
	'        GWAccount = GW.MultiLogin(Log.tbAccID.Text, , Log.tbPassword.Text)
	'    Catch ex As Exception
	'        MsgBox("That wasn't a valid account ID and password.  Please try again.")
	'        End
	'    End Try
	'    '<1>    If GWAccount.Owner.DisplayName.ToString <> OwnerOfAccount2 Then
	'    'be sure that the account being log in to is either the test or live student loan account
	'    If GWAccount.Owner.DisplayName.ToString <> "student_loan" And GWAccount.Owner.DisplayName.ToString <> "Student loantest" Then          '<1>
	'        MsgBox("That wasn't a valid account ID and password.  Please try again.")
	'        End
	'    End If
	'    '<1-> decide if the user is in Test or live mode and prompt them accordingly
	'    'is the user logged into live
	'    If GWAccount.Owner.DisplayName.ToString = "student_loan" Then
	'        MsgBox("Welcome to the LIVE environment for the e-mail tracking system.", MsgBoxStyle.Information, "LIVE")
	'        TestMode = False
	'        'setup DB connection
	'        ActualConn = Log.SQLConn
	'        Log.TestSqlConn.Dispose()
	'        LoadCounterList()                           '<3>
	'        OwnerOfAccount = "student_loan"
	'        '<4>           LiveProc()
	'    Else 'the user is in the test environment
	'        MsgBox("Welcome to the TEST environment for the e-mail tracking system.", MsgBoxStyle.Information, "TEST")
	'        MsgBox("If you haven't already, access the Student Loan Test e-mail account and be sure the account is setup for the type of testing you want to perform.", MsgBoxStyle.Critical, "Test Setup")
	'        TestMode = True
	'        'setup DB connection
	'        ActualConn = Log.TestSqlConn
	'        Log.SQLConn.Dispose()
	'        While TesterEmail = ""
	'            TesterEmail = InputBox("As a tester it is important that you see what is being sent out to the business units.  Please enter your e-mail address and you will be copied on every e-mail sent out by E-mail Watcher.  Example: Earl J Pickelsnarf would enter ""epickelsnarf@utahsbr.edu"".", "Tester E-mail address")
	'            If TesterEmail = "" Then
	'                MsgBox("You didn't enter an e-mail address.  Please try again.", MsgBoxStyle.Critical, "E-mail Address Needed")
	'            End If
	'        End While
	'        LoadCounterList()                           '<3>
	'        OwnerOfAccount = "Student loantest"
	'        '<4>            TestProc(TesterEmail)
	'    End If
	'    Proc(TesterEmail)
	'    'While 1 'cycle until the parent script ends the thread
	'    '    'search for emails to process
	'    '    '**************************************Process loanhelp emails first********************************************************
	'    '    MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"") AND(SUBJECT CONTAINS ""Change of Address and Telephone Information"" OR SUBJECT CONTAINS ""Stafford Loan Decrease"" OR SUBJECT CONTAINS ""PLUS Preapproval Comments"")")
	'    '    While MsgLst.Count <> 0
	'    '        WorkingMsg = MsgLst.Item(1)
	'    '        If WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Change of Address and Telephone Information") <> -1 Then
	'    '            MsgToForward = WorkingMsg.Forward
	'    '            MsgToForward.Recipients.Add("bcenter@utahsbr.edu")
	'    '            MsgToForward.Recipients.Add("rrobbins@utahsbr.edu")
	'    '            'update database
	'    '            'create master record
	'    '            Log.SQLConn.Open()
	'    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Richard Robbins','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'    '            Dim Cmd1 As New SqlCommand(SQLStr, Log.SQLConn)
	'    '            Cmd1.ExecuteNonQuery()
	'    '            Log.SQLConn.Close()
	'    '            cmd1.Dispose()
	'    '            'delete old message and send new one
	'    '            GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'    '            MsgToForward.Send()
	'    '        ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Stafford Loan Decrease") <> -1 Then
	'    '            MsgToForward = WorkingMsg.Forward
	'    '            MsgToForward.Recipients.Add("tvig@utahsbr.edu")
	'    '            MsgToForward.Recipients.Add("coman@utahsbr.edu")
	'    '            'update database
	'    '            'create master record
	'    '            Log.SQLConn.Open()
	'    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Teri Vig','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'    '            Dim Cmd1 As New SqlCommand(SQLStr, Log.SQLConn)
	'    '            Cmd1.ExecuteNonQuery()
	'    '            Log.SQLConn.Close()
	'    '            cmd1.Dispose()
	'    '            'delete old message and send new one
	'    '            GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'    '            MsgToForward.Send()
	'    '        ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Plus PreApproval Comments") <> -1 Then
	'    '            MsgToForward = WorkingMsg.Forward
	'    '            MsgToForward.Recipients.Add("tvig@utahsbr.edu")
	'    '            MsgToForward.Recipients.Add("coman@utahsbr.edu")
	'    '            'update database
	'    '            'create master record
	'    '            Log.SQLConn.Open()
	'    '            SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Teri Vig','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'    '            Dim Cmd1 As New SqlCommand(SQLStr, Log.SQLConn)
	'    '            Cmd1.ExecuteNonQuery()
	'    '            Log.SQLConn.Close()
	'    '            cmd1.Dispose()
	'    '            'delete old message and send new one
	'    '            GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'    '            MsgToForward.Send()
	'    '        End If
	'    '        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"") AND(SUBJECT CONTAINS ""Change of Address and Telephone Information"" OR SUBJECT CONTAINS ""Stafford Loan Decrease"" OR SUBJECT CONTAINS ""PLUS Preapproval Comments"")")
	'    '    End While
	'    '    '*********************************************SPAM Reports**********************************************************************
	'    '    MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""Daily SPAM Report"")")
	'    '    While MsgLst.Count <> 0
	'    '        MsgToForward = MsgLst.Item(1).Forward
	'    '        MsgToForward.Recipients.Add("kpage@utahsbr.edu")
	'    '        GWAccount.MailBox.Messages.Remove(MsgLst.Item(1))
	'    '        MsgToForward.Send()
	'    '        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""Daily SPAM Report"")")
	'    '    End While
	'    '    '********************************************Messages forwarded back to Student Loan from Student Loan**************************
	'    '    MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS """ & OwnerOfAccount2 & """)")
	'    '    While MsgLst.Count <> 0
	'    '        'delete from mailbox
	'    '        MsgLst.Item(1).Delete()
	'    '        'search again
	'    '        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS """ & OwnerOfAccount2 & """)")
	'    '    End While
	'    '    '*******************************************Messages forwarded back to Student Loan from the Business Units***********************
	'    '    MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM DOESNOTCONTAIN ""HELP"" AND TO DOESNOTCONTAIN ""uheaa@utahsbr.edu"" AND FROM DOESNOTCONTAIN ""Forward webaddress"" AND FROM DOESNOTCONTAIN ""LPP HELP"" AND FROM DOESNOTCONTAIN ""STUDENT LOAN"")")
	'    '    While MsgLst.Count <> 0
	'    '        'if the magical "--" doesn't appear in the message then send the e-mail to Kelli Page and delete the one in the inbox
	'    '        If MsgLst.Item(1).Subject.PlainText.IndexOf("--") > -1 Then
	'    '            'split out time date stamp of original email message
	'    '            If MsgLst.Item(1).Subject.PlainText.IndexOf("--") = 0 Then
	'    '                'mid function must start at 1
	'    '                OriginalTimeDate = Mid(MsgLst.Item(1).Subject.PlainText, MsgLst.Item(1).Subject.PlainText.IndexOf("--") + 1, 25)
	'    '                OriginalTimeDate = OriginalTimeDate.Substring(3)
	'    '            Else
	'    '                OriginalTimeDate = Mid(MsgLst.Item(1).Subject.PlainText, MsgLst.Item(1).Subject.PlainText.IndexOf("--"), 26)
	'    '                OriginalTimeDate = OriginalTimeDate.Substring(4)
	'    '            End If
	'    '            'add to database (third message in RespondedToEmail Table)
	'    '            Log.SQLConn.Open()
	'    '            SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
	'    '            Dim Cmd2 As New SqlCommand(SQLStr, Log.SQLConn)
	'    '            Reader = Cmd2.ExecuteReader() 'execute reader
	'    '            Reader.Read() 'move to first record in the reader
	'    '            NumInMaster = Reader.GetValue(0)
	'    '            Reader.Close()
	'    '            'check if there are three records, if so then just delete the e-mail
	'    '            SQLStr = "SELECT COUNT(*) as X FROM RespondedToEmail WHERE MasterNum = " & NumInMaster
	'    '            Dim Cmd4 As New SqlCommand(SQLStr, Log.SQLConn)
	'    '            Reader = Cmd4.ExecuteReader() 'execute reader
	'    '            Reader.Read()
	'    '            If Reader.GetValue(0) <> 3 Then
	'    '                'create first Responded record
	'    '                Reader.Close()
	'    '                SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & MsgLst.Item(1).CreationDate & "','BORROWER')"
	'    '                Dim Cmd3 As New SqlCommand(SQLStr, Log.SQLConn)
	'    '                Cmd3.ExecuteNonQuery()
	'    '                Log.SQLConn.Close()
	'    '                Cmd3.Dispose()
	'    '            Else
	'    '                Reader.Close()
	'    '                Log.SQLConn.Close()
	'    '            End If
	'    '            Cmd2.Dispose()
	'    '            Cmd4.Dispose()
	'    '        Else
	'    '            WorkingMsg = MsgLst.Item(1)
	'    '            MsgToForward = WorkingMsg.Forward
	'    '            MsgToForward.Recipients.Add("aadams@utahsbr.edu")
	'    '            MsgToForward.Recipients.Add("kpage@utahsbr.edu")
	'    '            MsgToForward.Send()
	'    '        End If
	'    '        'delete from mailbox
	'    '        MsgLst.Item(1).Delete()
	'    '        'search again
	'    '        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM DOESNOTCONTAIN ""HELP"" AND TO DOESNOTCONTAIN ""uheaa@utahsbr.edu"" AND FROM DOESNOTCONTAIN ""Forward webaddress"" AND FROM DOESNOTCONTAIN ""LPP HELP"" AND FROM DOESNOTCONTAIN ""STUDENT LOAN"")")
	'    '    End While
	'    '    '*******************************************Notification for Recp*****************************************************************
	'    '    MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"" AND SUBJECT DOESNOTCONTAIN ""Change of Address and Telephone Information"" AND SUBJECT DOESNOTCONTAIN ""Stafford Loan Decrease"" AND SUBJECT DOESNOTCONTAIN ""PLUS Preapproval Comments"") OR (TO CONTAINS ""uheaa@utahsbr.edu"") OR (FROM CONTAINS ""Forward webaddress"") OR (FROM CONTAINS ""LPP HELP"")") ' OR (FROM CONTAINS ""Student Loan"")")
	'    '    If MsgLst.Count() > 0 Then
	'    '        Notify.NotifyIcon.Visible = True
	'    '        If UserNotified = False Then
	'    '            Notify.ShowDialog()
	'    '        End If
	'    '        UserNotified = True
	'    '    Else
	'    '        Notify.NotifyIcon.Visible = False
	'    '        UserNotified = False
	'    '    End If
	'    '    System.Threading.Thread.CurrentThread.Sleep(TS)
	'    'End While
	'    '</1>
	'End Sub

	''<1->

	'Sub LiveProc()
	'    Dim MsgLst As GroupwareTypeLibrary.MessageList
	'    Dim MsgToForward As GroupwareTypeLibrary.Mail
	'    Dim WorkingMsg As GroupwareTypeLibrary.Mail
	'    Dim TS As New TimeSpan(0, 1, 0) 'for processing pause
	'    Dim SQLStr As String 'for SQL Statements
	'    Dim OriginalTimeDate As String
	'    Dim Reader As SqlDataReader 'for data reading
	'    Dim NumInMaster As Integer
	'    Dim QCGroup As String                                           '<3>
	'    While 1 'cycle until the parent script ends the thread
	'        'search for emails to process
	'        '<3->
	'        'undeliverable mail
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""undeliverable"")")
	'        While MsgLst.Count <> 0
	'            MsgToForward = MsgLst.Item(1).Forward
	'            MsgToForward.Recipients.Add("aadams@utahsbr.edu")
	'            MsgToForward.Recipients.Add("kpage@utahsbr.edu")
	'            MsgToForward.Subject.PlainText = "The E-mail Tracking System received this undeliverable message back."
	'            MsgToForward.Send()
	'            MsgLst.Item(1).Delete()
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""undeliverable"")")
	'        End While
	'        '</3>
	'        '**************************************Process loanhelp emails first********************************************************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"") AND(SUBJECT CONTAINS ""Change of Address and Telephone Information"" OR SUBJECT CONTAINS ""Stafford Loan Decrease"" OR SUBJECT CONTAINS ""PLUS Preapproval Comments"")")
	'        While MsgLst.Count <> 0
	'            WorkingMsg = MsgLst.Item(1)
	'            '<3>If WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Change of Address and Telephone Information") <> -1 Then
	'            If WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.ToLower.LastIndexOf("change of address and telephone information") <> -1 Then             '<3>
	'                QCCopying("Change of Address and Telephone Information", MsgLst.Item(1))                                      '<3>
	'                MsgToForward = WorkingMsg.Forward
	'                MsgToForward.Recipients.Add("bcenter@utahsbr.edu")
	'                MsgToForward.Recipients.Add("rrobbins@utahsbr.edu")
	'                'update database
	'                'create master record
	'                ActualConn.Open()
	'                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Richard Robbins','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
	'                Cmd1.ExecuteNonQuery()
	'                ActualConn.Close()
	'                cmd1.Dispose()
	'                'delete old message and send new one
	'                GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'                MsgToForward.Send()
	'                '<3>ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Stafford Loan Decrease") <> -1 Then
	'            ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.ToLower.LastIndexOf("stafford loan decrease") <> -1 Then      '<3>
	'                QCCopying("Stafford Loan Decrease", MsgLst.Item(1))                                      '<3>
	'                MsgToForward = WorkingMsg.Forward
	'                MsgToForward.Recipients.Add("tvig@utahsbr.edu")
	'                MsgToForward.Recipients.Add("coman@utahsbr.edu")
	'                'update database
	'                'create master record
	'                ActualConn.Open()
	'                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Teri Vig','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
	'                Cmd1.ExecuteNonQuery()
	'                ActualConn.Close()
	'                cmd1.Dispose()
	'                'delete old message and send new one
	'                GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'                MsgToForward.Send()
	'                '<3>ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Plus PreApproval Comments") <> -1 Then
	'            ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.ToLower.LastIndexOf("plus preapproval comments") <> -1 Then               '<3>
	'                QCCopying("Plus PreApproval Comments", MsgLst.Item(1))                                      '<3>
	'                MsgToForward = WorkingMsg.Forward
	'                MsgToForward.Recipients.Add("tvig@utahsbr.edu")
	'                MsgToForward.Recipients.Add("coman@utahsbr.edu")
	'                'update database
	'                'create master record
	'                ActualConn.Open()
	'                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Teri Vig','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
	'                Cmd1.ExecuteNonQuery()
	'                ActualConn.Close()
	'                cmd1.Dispose()
	'                'delete old message and send new one
	'                GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'                MsgToForward.Send()
	'            End If
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"") AND(SUBJECT CONTAINS ""Change of Address and Telephone Information"" OR SUBJECT CONTAINS ""Stafford Loan Decrease"" OR SUBJECT CONTAINS ""PLUS Preapproval Comments"")")
	'        End While
	'        '*********************************************SPAM Reports**********************************************************************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""Daily SPAM Report"")")
	'        While MsgLst.Count <> 0
	'            MsgToForward = MsgLst.Item(1).Forward
	'            MsgToForward.Recipients.Add("kpage@utahsbr.edu")
	'            MsgToForward.Recipients.Add("aturner@utahsbr.edu")              '<4>
	'            GWAccount.MailBox.Messages.Remove(MsgLst.Item(1))
	'            MsgToForward.Send()
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""Daily SPAM Report"")")
	'        End While
	'        '********************************************Messages forwarded back to Student Loan from Student Loan**************************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS """ & OwnerOfAccount2 & """)")
	'        While MsgLst.Count <> 0
	'            'delete from mailbox
	'            MsgLst.Item(1).Delete()
	'            'search again
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS """ & OwnerOfAccount2 & """)")
	'        End While
	'        '*******************************************Messages forwarded back to Student Loan from the Business Units***********************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM DOESNOTCONTAIN ""HELP"" AND TO DOESNOTCONTAIN ""uheaa@utahsbr.edu"" AND FROM DOESNOTCONTAIN ""Forward webaddress"" AND FROM DOESNOTCONTAIN ""LPP HELP"" AND FROM DOESNOTCONTAIN ""STUDENT LOAN"")")
	'        While MsgLst.Count <> 0
	'            'if the magical "--" doesn't appear in the message then send the e-mail to Kelli Page and delete the one in the inbox
	'            If MsgLst.Item(1).Subject.PlainText.IndexOf("--") > -1 Then
	'                'split out time date stamp of original email message
	'                If MsgLst.Item(1).Subject.PlainText.IndexOf("--") = 0 Then
	'                    'mid function must start at 1
	'                    OriginalTimeDate = Mid(MsgLst.Item(1).Subject.PlainText, MsgLst.Item(1).Subject.PlainText.IndexOf("--") + 1, 25)
	'                    OriginalTimeDate = OriginalTimeDate.Substring(3)
	'                Else
	'                    OriginalTimeDate = Mid(MsgLst.Item(1).Subject.PlainText, MsgLst.Item(1).Subject.PlainText.IndexOf("--"), 26)
	'                    OriginalTimeDate = OriginalTimeDate.Substring(4)
	'                End If
	'                'add to database (third message in RespondedToEmail Table)
	'                ActualConn.Open()
	'                '<2>                SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
	'                SQLStr = "SELECT Num, Recipient FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"          '<2>
	'                Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
	'                Reader = Cmd2.ExecuteReader() 'execute reader
	'                Reader.Read() 'move to first record in the reader
	'                NumInMaster = Reader.GetValue(0)
	'                QCGroup = Reader.GetString(1)                               '<3>
	'                Reader.Close()
	'                'check if there are three records, if so then just delete the e-mail
	'                SQLStr = "SELECT COUNT(*) as X FROM RespondedToEmail WHERE MasterNum = " & NumInMaster
	'                Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
	'                Reader = Cmd4.ExecuteReader() 'execute reader
	'                Reader.Read()
	'                If Reader.GetValue(0) <> 3 Then
	'                    'create first Responded record
	'                    Reader.Close()
	'                    SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & MsgLst.Item(1).CreationDate & "','BORROWER')"
	'                    Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
	'                    Cmd3.ExecuteNonQuery()
	'                    ActualConn.Close()
	'                    Cmd3.Dispose()
	'                    QCCopying(QCGroup, MsgLst.Item(1))                         '<3>
	'                Else
	'                    Reader.Close()
	'                    ActualConn.Close()
	'                End If
	'                Cmd2.Dispose()
	'                Cmd4.Dispose()
	'            Else
	'                WorkingMsg = MsgLst.Item(1)
	'                MsgToForward = WorkingMsg.Forward
	'                MsgToForward.Recipients.Add("aadams@utahsbr.edu")
	'                MsgToForward.Recipients.Add("kpage@utahsbr.edu")
	'                MsgToForward.BodyText.PlainText = "The E-mail Tracking System was unable to handle this e-mail."
	'                MsgToForward.Send()
	'            End If
	'            'delete from mailbox
	'            MsgLst.Item(1).Delete()
	'            'search again
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM DOESNOTCONTAIN ""HELP"" AND TO DOESNOTCONTAIN ""uheaa@utahsbr.edu"" AND FROM DOESNOTCONTAIN ""Forward webaddress"" AND FROM DOESNOTCONTAIN ""LPP HELP"" AND FROM DOESNOTCONTAIN ""STUDENT LOAN"")")
	'        End While
	'        '*******************************************Notification for Recp*****************************************************************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"" AND SUBJECT DOESNOTCONTAIN ""Change of Address and Telephone Information"" AND SUBJECT DOESNOTCONTAIN ""Stafford Loan Decrease"" AND SUBJECT DOESNOTCONTAIN ""PLUS Preapproval Comments"") OR (TO CONTAINS ""uheaa@utahsbr.edu"") OR (FROM CONTAINS ""Forward webaddress"") OR (FROM CONTAINS ""LPP HELP"")") ' OR (FROM CONTAINS ""Student Loan"")")
	'        If MsgLst.Count() > 0 Then
	'            Notify.NotifyIcon.Visible = True
	'            If UserNotified = False Then
	'                Notify.ShowDialog()
	'            End If
	'            UserNotified = True
	'        Else
	'            Notify.NotifyIcon.Visible = False
	'            UserNotified = False
	'        End If
	'        System.Threading.Thread.CurrentThread.Sleep(TS)
	'    End While
	'End Sub

	'Sub TestProc(ByVal TEM As String)
	'    Dim MsgLst As GroupwareTypeLibrary.MessageList
	'    Dim MsgToForward As GroupwareTypeLibrary.Mail
	'    Dim WorkingMsg As GroupwareTypeLibrary.Mail
	'    Dim TS As New TimeSpan(0, 1, 0) 'for processing pause
	'    Dim SQLStr As String 'for SQL Statements
	'    Dim OriginalTimeDate As String
	'    Dim Reader As SqlDataReader 'for data reading
	'    Dim NumInMaster As Integer
	'    Dim QCGroup As String                                           '<3>
	'    While 1 'cycle until the parent script ends the thread
	'        'search for emails to process
	'        '<3->
	'        'undeliverable mail
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""undeliverable"")")
	'        While MsgLst.Count <> 0
	'            MsgToForward = MsgLst.Item(1).Forward
	'            MsgToForward.Recipients.Add("aadams@utahsbr.edu")
	'            MsgToForward.Recipients.Add("kpage@utahsbr.edu")
	'            MsgToForward.Subject.PlainText = "The E-mail Tracking System received this undeliverable message back."
	'            MsgToForward.Send()
	'            MsgLst.Item(1).Delete()
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""undeliverable"")")
	'        End While
	'        '</3>
	'        '**************************************Process loanhelp emails first********************************************************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"") AND(SUBJECT CONTAINS ""Change of Address and Telephone Information"" OR SUBJECT CONTAINS ""Stafford Loan Decrease"" OR SUBJECT CONTAINS ""PLUS Preapproval Comments"")")
	'        While MsgLst.Count <> 0
	'            WorkingMsg = MsgLst.Item(1)
	'            '<3>If WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Change of Address and Telephone Information") <> -1 Then
	'            If WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.ToLower.LastIndexOf("change of address and telephone information") <> -1 Then             '<3>
	'                QCCopying("Change of Address and Telephone Information", MsgLst.Item(1), TEM)                                     '<3>
	'                MsgToForward = WorkingMsg.Forward
	'                MsgToForward.Recipients.Add("bcenter@utahsbr.edu")
	'                MsgToForward.Recipients.Add("rrobbins@utahsbr.edu")
	'                MsgToForward.Recipients.Add(TEM) 'add tester e-mail to message being forwarded
	'                'update database
	'                'create master record
	'                ActualConn.Open()
	'                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Richard Robbins','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
	'                Cmd1.ExecuteNonQuery()
	'                ActualConn.Close()
	'                cmd1.Dispose()
	'                'delete old message and send new one
	'                GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'                MsgToForward.Subject.PlainText = "E-mail Tracking System TEST (DO NOT PROCESS AS A LIVE E-MAIL). " & MsgToForward.Subject.PlainText
	'                MsgToForward.Send()
	'                '<3>ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Stafford Loan Decrease") <> -1 Then
	'            ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.ToLower.LastIndexOf("stafford loan decrease") <> -1 Then      '<3>
	'                QCCopying("Stafford Loan Decrease", MsgLst.Item(1), TEM)                                     '<3>
	'                MsgToForward = WorkingMsg.Forward
	'                MsgToForward.Recipients.Add("tvig@utahsbr.edu")
	'                MsgToForward.Recipients.Add("coman@utahsbr.edu")
	'                MsgToForward.Recipients.Add("klorensen@utahsbr.edu")
	'                MsgToForward.Recipients.Add(TEM) 'add tester e-mail to message being forwarded
	'                'update database
	'                'create master record
	'                ActualConn.Open()
	'                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Teri Vig','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
	'                Cmd1.ExecuteNonQuery()
	'                ActualConn.Close()
	'                cmd1.Dispose()
	'                'delete old message and send new one
	'                GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'                MsgToForward.Subject.PlainText = "E-mail Tracking System TEST (DO NOT PROCESS AS A LIVE E-MAIL). " & MsgToForward.Subject.PlainText
	'                MsgToForward.Send()
	'                '<3>ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.LastIndexOf("Plus PreApproval Comments") <> -1 Then
	'            ElseIf WorkingMsg.FromText.ToString.ToUpper = "HELP" And WorkingMsg.Subject.PlainText.ToLower.LastIndexOf("plus preapproval comments") <> -1 Then               '<3>
	'                QCCopying("Plus PreApproval Comments", MsgLst.Item(1), TEM)                                     '<3>
	'                MsgToForward = WorkingMsg.Forward
	'                MsgToForward.Recipients.Add("tvig@utahsbr.edu")
	'                MsgToForward.Recipients.Add("coman@utahsbr.edu")
	'                MsgToForward.Recipients.Add("klorensen@utahsbr.edu")
	'                MsgToForward.Recipients.Add(TEM) 'add tester e-mail to message being forwarded
	'                'update database
	'                'create master record
	'                ActualConn.Open()
	'                SQLStr = "INSERT INTO Master (CreationTimeDateStamp,Recipient,Subject) VALUES('" & WorkingMsg.CreationDate & "','Teri Vig','" & WorkingMsg.Subject.PlainText.Replace("'", "''") & "')"
	'                Dim Cmd1 As New SqlCommand(SQLStr, ActualConn)
	'                Cmd1.ExecuteNonQuery()
	'                ActualConn.Close()
	'                cmd1.Dispose()
	'                'delete old message and send new one
	'                GWAccount.MailBox.Messages.Remove(WorkingMsg)
	'                MsgToForward.Subject.PlainText = "E-mail Tracking System TEST (DO NOT PROCESS AS A LIVE E-MAIL). " & MsgToForward.Subject.PlainText
	'                MsgToForward.Send()
	'            End If
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"") AND(SUBJECT CONTAINS ""Change of Address and Telephone Information"" OR SUBJECT CONTAINS ""Stafford Loan Decrease"" OR SUBJECT CONTAINS ""PLUS Preapproval Comments"")")
	'        End While
	'        '*********************************************SPAM Reports**********************************************************************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""Daily SPAM Report"")")
	'        While MsgLst.Count <> 0
	'            MsgToForward = MsgLst.Item(1).Forward
	'            MsgToForward.Recipients.Add("kpage@utahsbr.edu")
	'            MsgToForward.Recipients.Add("aturner@utahsbr.edu")              '<4>
	'            MsgToForward.Recipients.Add(TEM) 'add tester e-mail to message being forwarded
	'            GWAccount.MailBox.Messages.Remove(MsgLst.Item(1))
	'            MsgToForward.Subject.PlainText = "E-mail Tracking System TEST (DO NOT PROCESS AS A LIVE E-MAIL). " & MsgToForward.Subject.PlainText
	'            MsgToForward.Send()
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (SUBJECT CONTAINS ""Daily SPAM Report"")")
	'        End While
	'        '********************************************Messages forwarded back to Student Loan from Student Loan**************************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS """ & OwnerOfAccount & """)")
	'        While MsgLst.Count <> 0
	'            'delete from mailbox
	'            MsgLst.Item(1).Delete()
	'            'search again
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS """ & OwnerOfAccount & """)")
	'        End While
	'        '*******************************************Messages forwarded back to Student Loan from the Business Units***********************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM DOESNOTCONTAIN ""HELP"" AND TO DOESNOTCONTAIN ""uheaa@utahsbr.edu"" AND FROM DOESNOTCONTAIN ""Forward webaddress"" AND FROM DOESNOTCONTAIN ""LPP HELP"" AND FROM DOESNOTCONTAIN ""student_loan"")")
	'        While MsgLst.Count <> 0
	'            'if the magical "--" doesn't appear in the message then send the e-mail to Kelli Page and delete the one in the inbox
	'            If MsgLst.Item(1).Subject.PlainText.IndexOf("--") > -1 Then
	'                'split out time date stamp of original email message
	'                If MsgLst.Item(1).Subject.PlainText.IndexOf("--") = 0 Then
	'                    'mid function must start at 1
	'                    OriginalTimeDate = Mid(MsgLst.Item(1).Subject.PlainText, MsgLst.Item(1).Subject.PlainText.IndexOf("--") + 1, 25)
	'                    OriginalTimeDate = OriginalTimeDate.Substring(3)
	'                Else
	'                    OriginalTimeDate = Mid(MsgLst.Item(1).Subject.PlainText, MsgLst.Item(1).Subject.PlainText.IndexOf("--"), 26)
	'                    OriginalTimeDate = OriginalTimeDate.Substring(4)
	'                End If
	'                'add to database (third message in RespondedToEmail Table)
	'                ActualConn.Open()
	'                '<2>                SQLStr = "SELECT Num FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"
	'                SQLStr = "SELECT Num, Recipient FROM Master WHERE CreationTimeDateStamp = '" & OriginalTimeDate & "'"          '<2>
	'                Dim Cmd2 As New SqlCommand(SQLStr, ActualConn)
	'                Reader = Cmd2.ExecuteReader() 'execute reader
	'                Reader.Read() 'move to first record in the reader
	'                NumInMaster = Reader.GetValue(0)
	'                QCGroup = Reader.GetString(1)                           '<3>
	'                Reader.Close()
	'                'check if there are three records, if so then just delete the e-mail
	'                SQLStr = "SELECT COUNT(*) as X FROM RespondedToEmail WHERE MasterNum = " & NumInMaster
	'                Dim Cmd4 As New SqlCommand(SQLStr, ActualConn)
	'                Reader = Cmd4.ExecuteReader() 'execute reader
	'                Reader.Read()
	'                If Reader.GetValue(0) <> 3 Then
	'                    'create first Responded record
	'                    Reader.Close()
	'                    SQLStr = "Insert Into RespondedToEmail (MasterNum,CreationTimeDateStamp,Recipient) Values(" & NumInMaster & " ,'" & MsgLst.Item(1).CreationDate & "','BORROWER')"
	'                    Dim Cmd3 As New SqlCommand(SQLStr, ActualConn)
	'                    Cmd3.ExecuteNonQuery()
	'                    ActualConn.Close()
	'                    Cmd3.Dispose()
	'                    QCCopying(QCGroup, MsgLst.Item(1), TEM)                        '<3>
	'                Else
	'                    Reader.Close()
	'                    ActualConn.Close()
	'                End If
	'                Cmd2.Dispose()
	'                Cmd4.Dispose()
	'            Else
	'                WorkingMsg = MsgLst.Item(1)
	'                MsgToForward = WorkingMsg.Forward
	'                MsgToForward.Recipients.Add("aadams@utahsbr.edu")
	'                MsgToForward.Recipients.Add("kpage@utahsbr.edu")
	'                MsgToForward.Recipients.Add(TEM) 'add tester e-mail to message being forwarded
	'                MsgToForward.BodyText.PlainText = "The E-mail Tracking System was unable to handle this e-mail."
	'                MsgToForward.Subject.PlainText = "E-mail Tracking System TEST (DO NOT PROCESS AS A LIVE E-MAIL). " & MsgToForward.Subject.PlainText
	'                MsgToForward.Send()
	'            End If
	'            'delete from mailbox
	'            MsgLst.Item(1).Delete()
	'            'search again
	'            MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM DOESNOTCONTAIN ""HELP"" AND TO DOESNOTCONTAIN ""uheaa@utahsbr.edu"" AND FROM DOESNOTCONTAIN ""Forward webaddress"" AND FROM DOESNOTCONTAIN ""LPP HELP"" AND FROM DOESNOTCONTAIN ""Student loantest"")")
	'        End While
	'        '*******************************************Notification for Recp*****************************************************************
	'        MsgLst = GWAccount.MailBox.Messages.Find("(BOX_TYPE = INCOMING AND MAIL) AND (FROM CONTAINS ""HELP"" AND SUBJECT DOESNOTCONTAIN ""Change of Address and Telephone Information"" AND SUBJECT DOESNOTCONTAIN ""Stafford Loan Decrease"" AND SUBJECT DOESNOTCONTAIN ""PLUS Preapproval Comments"") OR (TO CONTAINS ""uheaa@utahsbr.edu"") OR (FROM CONTAINS ""Forward webaddress"") OR (FROM CONTAINS ""LPP HELP"") OR (FROM CONTAINS ""student_loan"")")
	'        If MsgLst.Count() > 0 Then
	'            Notify.NotifyIcon.Visible = True
	'            If UserNotified = False Then
	'                Notify.ShowDialog()
	'            End If
	'            UserNotified = True
	'        Else
	'            Notify.NotifyIcon.Visible = False
	'            UserNotified = False
	'        End If
	'        System.Threading.Thread.CurrentThread.Sleep(TS)
	'    End While
	'End Sub

	'</4>
	'</1>
#End Region

End Class

Public Class ProcessedObj
	Public Processed As Boolean

	Public Sub New()
		Processed = False
	End Sub
End Class
