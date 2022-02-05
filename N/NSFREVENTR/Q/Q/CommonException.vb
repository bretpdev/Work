Imports System.Data.Linq
Imports OSSMTP
Imports System.Windows.Forms

Public Class CommonException
    Inherits DataAccessBase

    Private Shared _dc As DataContext

    Enum MailTo
        Developer
        SystemsSupport
    End Enum

    ''' <summary>
    ''' Sends email to the MailTo group with the designated error message
    ''' </summary>
    ''' <param name="mTo">MailTo group: SystemsSupport or Developers</param>
    ''' <param name="id">The ID of the message that is stored in the database</param>
    ''' <param name="message">The message thrown at the time of the Exception</param>
    ''' <param name="exception">The Exception</param>
    ''' <remarks>If there is no InnerException, it will default to a 0 ID</remarks>
    Private Shared Sub SendMail(ByVal mTo As MailTo, ByVal id As Integer, ByVal system As String, ByVal message As String, ByVal exception As Exception)
        Dim mail As New OSSMTP.SMTPSession()
        mail.Server = "mail.utahsbr.edu"
        mail.Timeout = 20000
        mail.Importance = importance_level.ImportanceHigh
        mail.MailFrom = Environment.UserName + "@utahsbr.edu"

        'Get list of people to send the mail to according to the role they are in
        Dim mailTo As String = String.Empty
        For Each str As String In _dc.ExecuteQuery(Of String)("EXEC spGENR_GetEmailFromRole {0}", mTo.ToString)
            mailTo = mailTo + str + ", "
        Next

        mail.SendTo = mailTo
        mail.MessageSubject = "Exception was thrown"
        mail.MessageText = "ID: " + id.ToString() + vbCrLf + "System: " + system
        'If emailing the developers, add the inner exception message
        If mTo = CommonException.MailTo.Developer Then
            mail.MessageText += vbCrLf + "Exception: " + exception.Message + If(exception.InnerException Is Nothing, "", vbCrLf + vbCrLf + exception.InnerException.ToString())
        End If

        'send message
        mail.SendEmail()

        Dim SecondsWaited As Integer
        'Wait up to five seconds for the email to successfully be sent
        While mail.Status <> "SMTP connection closed"
            Threading.Thread.Sleep(New TimeSpan(0, 0, 1))
            SecondsWaited = SecondsWaited + 1
            If SecondsWaited = 5 AndAlso mail.Status <> "SMTP connection closed" Then
                'If the script has waited 5 seconds then give error message and exit Subroutine
                Throw New EmailException("Your message was not sent for the following reason:  " & mail.Status & "  Please contact Application Development for assistance.")
            End If
        End While
    End Sub

    ''' <summary>
    ''' Adds a Need Help ticket for the exception to be worked
    ''' </summary>
    ''' <param name="testMode">Test Mode Property</param>
    ''' <param name="system">Needs to be the DLL name and contain FED if it is a Federal system</param>
    ''' <param name="ex">The exception that was thrown</param>
    ''' <remarks></remarks>k
    Private Shared Sub BuildException(ByVal testMode As Boolean, ByVal errorType As String, ByVal system As String, ByVal ex As Exception)

        _dc = CSYSDataContext(testMode)
        Dim sqlUserID = _dc.ExecuteQuery(Of Integer)("EXEC spGENR_GetUserID {0}", Environment.UserName).FirstOrDefault()
        Dim businessUnitID = _dc.ExecuteQuery(Of Integer)("EXEC spGENR_GetUserBusinessUnit {0}", sqlUserID).FirstOrDefault()
        Dim status = _dc.ExecuteQuery(Of String)("EXEC spFLOW_GetStatusForFlowStep {0}, {1}", "FNC", 1).FirstOrDefault()
        Dim finalStatus = _dc.ExecuteQuery(Of String)("EXEC spFLOW_GetFinalStatusForFlowStep {0}", "FNC").FirstOrDefault()
        Dim court = _dc.ExecuteQuery(Of Integer)("EXEC spGENR_GetCommonExceptionCourt").FirstOrDefault()

        If testMode Then
            If system.ToUpper().Contains("FED") Then
                _dc = New DataContext("Data Source=OPSDEV;Initial Catalog=NeedHelpCornerStone;Integrated Security=SSPI;")
            Else
                _dc = New DataContext("Data Source=OPSDEV;Initial Catalog=NeedHelpUheaa;Integrated Security=SSPI;")
            End If
        Else
            If system.ToUpper().Contains("FED") Then
                _dc = New DataContext("Data Source=Nochouse;Initial Catalog=NeedHelpCornerStone;Integrated Security=SSPI;")
            Else
                _dc = New DataContext("Data Source=Nochouse;Initial Catalog=NeedHelpUheaa;Integrated Security=SSPI;")
            End If
        End If

        If _dc.ExecuteQuery(Of Long)("EXEC spGetOpenTicketID {0}, {1}, {2}", "FNC", errorType, finalStatus).FirstOrDefault() <= 0 Then
            Try
                _dc.ExecuteCommand("EXEC spAddCommonExceptionTicket {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", "FNC", errorType, businessUnitID, "There was an exception thrown in " + system + "\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace, status, "Creates Significant Operational Efficiency", "Resolve Customer Service Issue", sqlUserID, court)
            Catch
                MessageBox.Show("There was an error saving the Need Help ticket.")
                Return
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Display error to user and ask if a Need Help ticket is needed
    ''' </summary>
    ''' <param name="testMode">Test Mode Property</param>
    ''' <param name="systemOrTicketType">The System or Ticket Type. (ex: ACDC Flows, DCR, DCR FED)</param>
    ''' <param name="errorType">There was a '" + errorType + "' exception thrown: (ex: GetUserList() function, errorType = 'Error Getting User List')</param>
    ''' <param name="ex">The Exception thrown</param>
    ''' <remarks>Yes = creates Need Help ticket to fix the problem, No = returns</remarks>
    Public Shared Sub CatchException(ByVal testMode As Boolean, ByVal systemOrTicketType As String, ByVal errorType As String, ByVal ex As Exception)
        Dim message As String = "There was a '" + errorType + "' exception thrown:" + vbCrLf + vbCrLf + ex.Message + vbCrLf + ex.StackTrace
        If MessageBox.Show(message + vbCrLf + vbCrLf + "Do you want to send this error to Need Help?", "Exception Thrown", MessageBoxButtons.YesNo, MessageBoxIcon.Error) = DialogResult.Yes Then
            BuildException(testMode, errorType, systemOrTicketType, ex)
        End If
    End Sub

End Class
