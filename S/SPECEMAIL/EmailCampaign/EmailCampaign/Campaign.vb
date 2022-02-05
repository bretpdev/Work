Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports OSSMTP

Public Class Campaign
    Inherits System.Windows.Forms.ListViewItem

    Private _data As DataRow
    Public Property Data() As DataRow
        Get
            Return _data
        End Get
        Set(ByVal value As DataRow)
            _data = value
        End Set
    End Property

    Private _iD As String
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _iD = value
        End Set
    End Property

    Private _refCommenter As Commenter 'deals with Reflection
    Private _processingform As frmProcessing
    Private _Processingformthread As New Threading.Thread(AddressOf DisplayProcessing)

    Public Sub New(ByRef tData As DataRow)
        MyBase.New(tData.Item("EmailSubjectLine").ToString)
        Me.SubItems.Add(tData.Item("DataFile").ToString)
        Me.SubItems.Add(tData.Item("HTMLFile").ToString)
        Me.SubItems.Add(tData.Item("EmailFrom").ToString)
        Me.SubItems.Add(tData.Item("EmailFromDisplay").ToString)
        _iD = tData.Item("CampID")
        _data = tData
    End Sub

    Public Sub New(ByRef tID As Integer)
        _iD = tID
    End Sub

    Public Shared Function ConnStr() As String
        If CBool(My.Resources.TestMode) Then
            ConnStr = My.Resources.BSYSTestConn
        Else
            ConnStr = My.Resources.BSYSLiveConn
        End If
    End Function


    Public Shared Sub UpdateDB(ByVal Vals As ArrayList)
        Dim Conn As SqlConnection
        Dim Comm As SqlCommand
        Conn = New SqlConnection(ConnStr())
        Comm = New SqlCommand(String.Format("UPDATE EMCP_DAT_EmailCampaigns SET EmailSubjectLine = '{1}', Compass = '{2}', OneLINK = '{3}', ARC = '{4}', ActionCode = '{5}', CommentText = '{6}', DataFile = '{7}', HTMLFile = '{8}', EmailFrom = '{9}', EmailFromDisplay = '{10}' WHERE CampID = {0}", Vals.ToArray()), Conn)
        Conn.Open()
        Comm.ExecuteNonQuery()
        Conn.Close()
    End Sub

    Public Shared Function InsertIntoDB(ByVal Vals As ArrayList) As Integer
        Dim Conn As SqlConnection
        Dim Comm As SqlCommand
        Conn = New SqlConnection(ConnStr())
        Comm = New SqlCommand(String.Format("INSERT INTO EMCP_DAT_EmailCampaigns (EmailSubjectLine, Compass, OneLINK, ARC, ActionCode, CommentText, DataFile, HTMLFile, EmailFrom, EmailFromDisplay) VALUES ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", Vals.ToArray()), Conn)
        Conn.Open()
        Comm.ExecuteNonQuery()
        Comm.CommandText = String.Format("SELECT MAX(CampID) as CampID FROM EMCP_DAT_EmailCampaigns WHERE EmailSubjectLine = '{0}'", Vals(1).ToString)
        InsertIntoDB = CInt(Comm.ExecuteScalar()) 'return campaign ID
        Conn.Close()
    End Function

    Public Shared Function GetUpdatedDataRow(ByVal ID As Integer) As DataRow
        Dim DS As New DataSet()
        Dim DA As New SqlDataAdapter(String.Format("SELECT * FROM EMCP_DAT_EmailCampaigns WHERE CampID = {0}", ID), ConnStr())
        DA.Fill(DS, "Data")
        Return DS.Tables("Data").Rows(0)
    End Function

    Public Sub Run(Optional ByVal DataFile As String = "")
        'create stream reader to get HTML file text
        Dim Reader As New StreamReader(_data.Item("HTMLFile").ToString)
        'get text from HTML file
        Dim HTMLFileText As String = Reader.ReadToEnd()
        Reader.Close()
        _refCommenter = New Commenter()
        Const HIDDEN_EMAIL_ID_HTML As String = "<input type=""hidden"" name=""UHEAAEmailID"" id=""UHEAAEmailID"" value=""[[[EmailID]]]""/>"
        Dim EndOfBodyLocation As Long
        'place hidden HTML element just inside the body tag or at the end of the HTML text
        EndOfBodyLocation = HTMLFileText.ToUpper.IndexOf("</BODY>")
        If EndOfBodyLocation = -1 Then
            'insert at end of HTML
            HTMLFileText = HTMLFileText + HIDDEN_EMAIL_ID_HTML
        Else
            'insert just inside the body tag
            HTMLFileText = HTMLFileText.Insert(EndOfBodyLocation, HIDDEN_EMAIL_ID_HTML)
        End If
        'determine error logs name
        Dim ErrorLogName As String
        If CBool(My.Resources.TestMode) Then
            ErrorLogName = "X:\Reports\Special Email Campaign Error Reports\Test\"
        Else
            ErrorLogName = "X:\Reports\Special Email Campaign Error Reports\"
        End If
        ErrorLogName = ErrorLogName + "LOG ID" + _iD + "--" + Format(Now, "MM-dd-yyyy h-m-s") + ".txt"
        If DataFile = "" Then DataFile = _data.Item("DataFile").ToString
        _Processingformthread.IsBackground = True
        _Processingformthread.Start()
        Process(DataFile, _data.Item("EmailSubjectLine").ToString, HTMLFileText, _data.Item("Compass").ToString, _data.Item("OneLINK").ToString, _data.Item("ARC").ToString, _data.Item("ActionCode").ToString, _data.Item("CommentText").ToString, ErrorLogName, _data.Item("EmailFrom").ToString)
        'delete log file
        'Recovery.DeleteLog()
        _Processingformthread.Abort()
        'check if error log was created and notify user of it if does exist
        If File.Exists(ErrorLogName) Then MessageBox.Show("There is an error log.  Please notify the person whom requested that the script be run.  The error log is named " + ErrorLogName + ".", "Error Log Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        MessageBox.Show("Processing Complete!", "Processing Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End
    End Sub

    Private Sub Process(ByVal DataFile As String, ByVal EmailSubject As String, ByVal HTMLFileText As String, ByVal Compass As String, ByVal OneLink As String, ByVal ARC As String, ByVal ActionCode As String, ByVal CommentText As String, ByVal ErrorLogName As String, ByVal EmailFrom As String)
        Dim AccountNumber As String = ""
        Dim SSN As String = ""
        Dim Name As String = ""
        Dim EmailAddr As String = ""
        Dim TotalEmailedSincePause As Integer = 0
        Dim ThePhase As Recovery.Phase
        Recovery.IsAppInRecovery() 'flags whether the app is in recovery or not
        ThePhase = Recovery.RecoveryPhase()
        FileOpen(1, DataFile, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
        'get header row
        Input(1, AccountNumber)
        Input(1, Name)
        Input(1, EmailAddr)
        'check for and do recovery thing
        If ThePhase <> Recovery.Phase.NotInRecovery Then
            'find last ssn successfully processed
            Dim rAccountNumber As String = ""
            rAccountNumber = Recovery.GetAcctNum()
            While AccountNumber <> rAccountNumber
                Input(1, AccountNumber)
                Input(1, Name)
                Input(1, EmailAddr)
                If ThePhase = Recovery.Phase.Emailing Then 'if phase = emailing then the last ssn was only processed partly so the activity comments needs to be added as well
                    AddComments(AccountNumber, Compass, OneLink, ARC, ActionCode, CommentText, ErrorLogName)
                End If
            End While
        End If
        While Not EOF(1)
            'get data row
            Input(1, AccountNumber)
            Input(1, Name)
            Input(1, EmailAddr)
            'be sure that all elements have data
            If AccountNumber.Trim() = "" Or Name.Trim() = "" Or EmailAddr.Trim() = "" Then
                MessageBox.Show("One of the data elements from the data file came back blank.  Please investigate and start up the application again when you have resolved the problem.", "Blank Data Element", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End
            End If
            'emailing************************************************************************************************************
            Emailing(AccountNumber, ErrorLogName, EmailSubject, HTMLFileText, Name, EmailAddr, EmailFrom)
            Recovery.UpdateLog(Recovery.Phase.Emailing, AccountNumber) 'update recovery log
            'Add Comments********************************************************************************************************
            'do account number translation
            AddComments(AccountNumber, Compass, OneLink, ARC, ActionCode, CommentText, ErrorLogName)
            Recovery.UpdateLog(Recovery.Phase.ActivityComments, AccountNumber) 'update recovery log

            TotalEmailedSincePause = TotalEmailedSincePause + 1
            If TotalEmailedSincePause = 5000 Then
                Threading.Thread.Sleep(New TimeSpan(0, 1, 0)) 'sleep for a minute to allow the server to catch up
                TotalEmailedSincePause = 0
            End If
        End While
        FileClose(1)
        Recovery.DeleteLog() 'delete recovery log
    End Sub

    Private Sub Emailing(ByVal AccountNumber As String, ByVal ErrorLogName As String, ByVal EmailSubject As String, ByVal HTMLFileText As String, ByVal Name As String, ByVal EmailAddr As String, ByVal EmailFrom As String)
        Dim Email As OSSMTP.SMTPSession
        Dim SecondsWaited As Integer
        Try
            'MailAddress = New MailAddress("dbarton@utahsbr.edu", "Dan Barton")
            Email = New OSSMTP.SMTPSession
            'set server and time out (default = 10 secs, set to 20 secs)
            Email.Server = "mail.utahsbr.edu"
            Email.Timeout = 20000

            Email.SendTo = EmailAddr

            Email.MailFrom = EmailFrom

            'add email ID to hidden HTML element
            Email.MessageHTML = HTMLFileText.Replace("[[[BorName]]]", Name).Replace("[[[EmailID]]]", CreateTrackingEmailID(AccountNumber, EmailAddr))
            Email.MessageSubject = EmailSubject
            'send message
            Email.SendEmail()
            'wait up to five seconds for the email to successfully be sent
            While Email.Status <> "SMTP connection closed"
                System.Threading.Thread.Sleep(New TimeSpan(0, 0, 1))
                SecondsWaited = SecondsWaited + 1
                If SecondsWaited = 5 And Email.Status <> "SMTP connection closed" Then
                    'write to error log
                    WriteToErrorLog(ErrorLogName, AccountNumber, "Error encountered while emailing - possible invalid email address")
                    Exit While
                End If
            End While
            'if the script makes it through loop above then it successfully sent the email and received the all is OK message
        Catch ex As Exception
            'write to error log
            WriteToErrorLog(ErrorLogName, AccountNumber, "Error encountered while emailing - possible invalid email address")
        End Try
    End Sub

    Private Sub AddComments(ByVal AccountNumber As String, ByVal Compass As String, ByVal OneLink As String, ByVal ARC As String, ByVal ActionCode As String, ByVal CommentText As String, ByVal ErrorLogName As String)
        Dim SSN As String
        SSN = _refCommenter.AccountNumberTranslation(AccountNumber)
        If SSN = "" Then
            'write to error log
            WriteToErrorLog(ErrorLogName, AccountNumber, "Acct # not found on LP22")
        Else
            'add comments if an SSN was returned
            If Compass = "Y" Then
                If _refCommenter.AddCommentsToTD22AllLoansWithBal(SSN, CommentText, ARC) = False Then
                    If _refCommenter.AddCommentsToTD37(SSN, CommentText, ARC) = False Then
                        'write to error log
                        WriteToErrorLog(ErrorLogName, AccountNumber, "Could not add comment on COMPASS")
                    End If
                End If
            End If
            If OneLink = "Y" Then
                If _refCommenter.AddCommentsToLP50(SSN, CommentText, ActionCode, "03", "EM") = False Then
                    'write to error log
                    WriteToErrorLog(ErrorLogName, AccountNumber, "Could not add comment on OneLINK")
                End If
            End If
        End If
    End Sub

    'write data to error log
    Private Sub WriteToErrorLog(ByVal ErrorLogName As String, ByVal AcctNum As String, ByVal ErrorMsg As String)
        FileOpen(5, ErrorLogName, OpenMode.Append, OpenAccess.Write, OpenShare.LockWrite)
        PrintLine(5, AcctNum + "," + ErrorMsg)
        FileClose(5)
    End Sub

    'creates new Email ID record and returns ID
    Private Function CreateTrackingEmailID(ByVal AccountNumber As String, ByVal EmailAddr As String) As Long
        Dim Conn As SqlConnection
        Dim Comm As SqlCommand
        Conn = New SqlConnection(ConnStr())
        EmailAddr = "'" + EmailAddr + "'"
        Comm = New SqlCommand(String.Format("EXEC spEMCP_CreateEmailID {0},{1},{2}", AccountNumber, _iD, EmailAddr), Conn)
        Conn.Open()
        CreateTrackingEmailID = CType(Comm.ExecuteScalar(), Long)
        Conn.Close()
    End Function

    Private Sub DisplayProcessing()
        _processingform = New frmProcessing()
        _processingform.ShowDialog()
        _processingform.Refresh()
    End Sub

End Class
