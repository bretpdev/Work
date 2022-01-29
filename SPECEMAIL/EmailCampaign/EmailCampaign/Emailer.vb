Imports System.IO
'Imports MailBee
'Imports MailBee.Mime
'Imports MailBee.SmtpMail
Imports OSSMTP

Public Class Emailer

    Private DataFile As String
    Private EmailSubject As String
    Private HTMLFileText As String
    Private ImageDirFileList() As String
    'Private Const Server As String = "mail.utahsbr.edu"
    'Private Const SMTPLKey As String = "MN100-4A2E2CBC2EB52E352EE8E592264B-1B79"

    Public Sub New(ByVal tDataFile As String, ByVal tEmailSubject As String, ByVal HTMLFile As String, ByVal ImageDir As String)
        Dim Reader As New StreamReader(HTMLFile) 'create stream reader to get HTML file text
        'initialize class vars
        DataFile = tDataFile
        EmailSubject = tEmailSubject
        'get text from HTML file
        HTMLFileText = Reader.ReadToEnd()
        Reader.Close()
        If ImageDir <> "" Then ImageDirFileList = Directory.GetFiles(ImageDir)
        'Smtp.LicenseKey = SMTPLKey
    End Sub

    Public Sub SendEmail()
        'Dim SMTPMsg As Smtp
        'Dim serv As New SmtpServer(Server, "grassroots", "Update03")
        Dim SSN As String = ""
        Dim Name As String = ""
        Dim EmailAddr As String = ""
        Dim Attachment As String = ""
        Dim TotalEmailedSincePause As Integer = 0
        Dim Email As OSSMTP.SMTPSession
        Dim SecondsWaited As Integer
        FileOpen(1, DataFile, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
        'get header row
        Input(1, SSN)
        Input(1, Name)
        Input(1, EmailAddr)
        'check for recovery
        If Recovery.RecoveryPhase() = Recovery.Phase.Emailing Then
            'find last ssn successfully processed
            Dim rSSN As String = ""
            rSSN = Recovery.GetSSN()
            While SSN <> rSSN
                Input(1, SSN)
                Input(1, Name)
                Input(1, EmailAddr)
            End While
            Recovery.InRecovery = False
        End If
        While Not EOF(1)
            'get data row
            Input(1, SSN)
            Input(1, Name)
            Input(1, EmailAddr)

            Email = New OSSMTP.SMTPSession

            'set server and time out (default = 10 secs, set to 20 secs)
            Email.Server = "mail.utahsbr.edu"
            Email.Timeout = 20000


            If CBool(My.Resources.TestMode) Then
                'send to user if in test mode
                Email.SendTo = Environment.UserName() + "@utahsbr.edu"
            Else
                'send to borrower if not in test mode
                Email.SendTo = EmailAddr
            End If
            Email.MailFrom = "uheaarayofhope@utahsbr.edu"
            Email.MessageHTML = HTMLFileText.Replace("[[[BorName]]]", Name)
            Email.MessageSubject = EmailSubject

            'add attachments if any
            If Not (ImageDirFileList Is Nothing) Then
                'add attachments
                For Each Attachment In ImageDirFileList
                    Email.Attachments.Add(Attachment)
                Next
            End If

            'send message
            Email.SendEmail()

            'wait up to five seconds for the email to successfully be sent
            While Email.Status <> "SMTP connection closed"
                System.Threading.Thread.Sleep(New TimeSpan(0, 0, 1))
                SecondsWaited = SecondsWaited + 1
                If SecondsWaited = 5 And Email.Status <> "SMTP connection closed" Then
                    'if the script has waited 5 seconds then give error message and exit function
                    MsgBox("Your message was not sent for the following reason:  " & Email.Status & "  Please contact Process Automation for assistance.", 48, "Message not Sent")
                    End
                End If
            End While
            'if the script makes it through loop above then it successfully sent the email and received the all is OK message


            'SMTPMsg = New Smtp
            'SMTPMsg.SmtpServers.Clear()
            'SMTPMsg.DnsServers.Clear()
            'SMTPMsg.SmtpServers.Add(serv) 'add smtp server
            'If CBool(My.Resources.TestMode) Then
            '    'send to user if in test mode
            '    SMTPMsg.To.AsString = Environment.UserName() + "@utahsbr.edu"
            'Else
            '    'send to borrower if not in test mode
            '    SMTPMsg.To.AsString = EmailAddr
            'End If
            ''SMTPMsg.From.AsString = "donotreply@utahsbr.edu"
            'SMTPMsg.From.AsString = "uheaarayofhope@utahsbr.edu"
            'If Not (ImageDirFileList Is Nothing) Then
            '    'add attachments
            '    For Each Attachment In ImageDirFileList
            '        SMTPMsg.AddAttachment(Attachment)
            '    Next
            'End If
            'SMTPMsg.BodyHtmlText = HTMLFileText.Replace("[[[BorName]]]", Name)
            'SMTPMsg.Subject = EmailSubject
            'SMTPMsg.Log.Enabled = True
            'SMTPMsg.Log.Filename = "C:\Windows\Temp\SMTPLog.txt"
            'SMTPMsg.Send()
            Recovery.UpdateLog(Recovery.Phase.Emailing, SSN)
            'TotalEmailedSincePause = TotalEmailedSincePause + 1
            'If TotalEmailedSincePause = 5000 Then
            '    Threading.Thread.Sleep(New TimeSpan(0, 1, 0)) 'sleep for a minute to allow the server to catch up
            '    TotalEmailedSincePause = 0
            'End If
        End While
        FileClose(1)
    End Sub

End Class
