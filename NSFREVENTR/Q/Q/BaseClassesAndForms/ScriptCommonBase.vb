Imports Q.Common
Imports Q.DocumentHandling

Public MustInherit Class ScriptCommonBase

    Private _testMode As Boolean

    ''' <summary>
    ''' Test mode indicator
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property TestModeProperty() As Boolean
        Get
            Return _testMode
        End Get
    End Property

    'Protected _ProcessLogData As ProcessLogData
    'Protected Property ProcessLogData() As ProcessLogData
    '    Get
    '        Return _ProcessLogData
    '    End Get
    '    Set(value As ProcessLogData)
    '        _ProcessLogData = value
    '    End Set
    'End Property

    Public Sub New(ByVal tTestMode As Boolean)

        _testMode = tTestMode
    End Sub

    ''' <summary>
    ''' Reads a comma-delimited file into a DataTable.
    ''' </summary>
    ''' <param name="fileName">The full path and file name of the file to be read.</param>
    ''' <returns>A DataTable object populated with the file's contents.</returns>
    Protected Function CreateDataTableFromFile(ByVal fileName As String) As System.Data.DataTable
        Return Common.CreateDataTableFromFile(fileName)
    End Function

    ''' <summary>
    ''' Reads a comma-delimited file into a DataTable.
    ''' </summary>
    ''' <param name="fileName">The full path and file name of the file to be read.</param>
    ''' <param name="firstRowIsHeader">True if the first line in the file has header fields.</param>
    ''' <param name="columnNames">The names to be used for the DataTable's columns.</param>
    ''' <returns>A DataTable object populated with the file's contents.</returns>
    Protected Function CreateDataTableFromFile(ByVal fileName As String, ByVal firstRowIsHeader As Boolean, ByVal ParamArray columnNames() As String) As System.Data.DataTable
        Return Common.CreateDataTableFromFile(fileName, firstRowIsHeader, columnNames)
    End Function

    ''' <summary>
    ''' Reads a comma-delimited file into a DataTable.
    ''' </summary>
    ''' <param name="fileName">The full path and file name of the file to be read.</param>
    ''' <param name="firstRowIsHeader">True if the first line in the file has header fields.</param>
    ''' <param name="fields">Dictionary of field names (key) and the system type of the column (value).</param>
    ''' <returns>A DataTable object populated with the file's contents.</returns>
    ''' <remarks></remarks>
    Protected Function CreateDataTableFromFile(ByVal fileName As String, ByVal firstRowIsHeader As Boolean, ByVal fields As Dictionary(Of String, Type)) As System.Data.DataTable
        Return Common.CreateDataTableFromFile(fileName, firstRowIsHeader, fields)
    End Function

    ''' <summary>
    ''' Throws EndDLLException to end DLL functionality (this should be caught and handled by your main method)
    ''' </summary>
    Protected Sub EndDLLScript()
        'If Not ProcessLogData Is Nothing Then
        '    ProcessLogger.LogEnd(ProcessLogData.ProcessLogId)
        'End If
        Common.EndDLLScript()
    End Sub

    ''' <summary>
    ''' Deletes a file, checking that it is really gone before returning.
    ''' </summary>
    ''' <param name="fileName">Full path and name of file to be deleted.</param>
    Protected Sub KillUntilDead(ByVal fileName As String)
        Common.KillUntilDead(fileName)
    End Sub

    ''' <summary>
    ''' Send an e-mail message using SMTP
    ''' </summary>
    ''' <param name="testMode">If in test mode.</param>
    ''' <param name="mto">Email address of who to send the email to.  Comma delimit multiple entries.</param>
    ''' <param name="mFrom">Who the email is being sent from.  Send "" if you want the sub to figure it out for you.</param>
    ''' <param name="mSubject">The email subject line.</param>
    ''' <param name="mBody">The email body text.</param>
    ''' <param name="mCC">Recipients to carbon copy.  Comma delimit multiple entries.</param>
    ''' <param name="mBCC">Recipients to blind copy.  Comma delimit multiple entries.</param>
    ''' <param name="mAttach">Attachments for email.  Comma delimit multiple entries.  Send blank string if non are desired.</param>
    ''' <param name="mImportance">The importance level assigned to the email.</param>
    ''' <param name="TestIt">If this indicator is set to true and the application is in test mode then the current user will be added to the to list and "THIS IS A TEST" is added to the subject and cody text.  Was defaulted to True before the C# conversion.</param>
    Protected Sub SendMail(ByVal testMode As Boolean, ByVal mto As String, ByVal mFrom As String, ByVal mSubject As String, ByVal mBody As String, ByVal mCC As String, ByVal mBCC As String, ByVal mAttach As String, ByVal mImportance As Common.EmailImportanceLevel, ByVal TestIt As Boolean)
        Common.SendMail(testMode, mto, mFrom, mSubject, mBody, mCC, mBCC, mAttach, mImportance, TestIt)
    End Sub

    ''' <summary>
    ''' Send an e-mail message using SMTP (provides option of HTML or plain text)
    ''' </summary>
    ''' <param name="testMode">If in test mode.</param>
    ''' <param name="mto">Email address of who to send the email to.  Comma delimit multiple entries.</param>
    ''' <param name="mFrom">Who the email is being sent from.  Send "" if you want the sub to figure it out for you.</param>
    ''' <param name="mSubject">The email subject line.</param>
    ''' <param name="mBody">The email body text.</param>
    ''' <param name="mCC">Recipients to carbon copy.  Comma delimit multiple entries.</param>
    ''' <param name="mBCC">Recipients to blind copy.  Comma delimit multiple entries.</param>
    ''' <param name="mAttach">Attachments for email.  Comma delimit multiple entries.  Send blank string if non are desired.</param>
    ''' <param name="mImportance">The importance level assigned to the email.</param>
    ''' <param name="TestIt">If this indicator is set to true and the application is in test mode then the current user will be added to the to list and "THIS IS A TEST" is added to the subject and cody text.  Was defaulted to True before the C# conversion.</param>
    ''' <param name="AsHTML">Indicator as to whether the email should be sent as HTML or text.</param>
    Protected Sub SendMail(ByVal testMode As Boolean, ByVal mto As String, ByVal mFrom As String, ByVal mSubject As String, ByVal mBody As String, ByVal mCC As String, ByVal mBCC As String, ByVal mAttach As String, ByVal mImportance As Common.EmailImportanceLevel, ByVal TestIt As Boolean, ByVal AsHTML As Boolean)
        Common.SendMail(testMode, mto, mFrom, mSubject, mBody, mCC, mBCC, mAttach, mImportance, TestIt, AsHTML)
    End Sub

    ''' <summary>
    ''' Determine if the user is running the script in test mode and return object with set directory locations
    ''' </summary>
    ''' <param name="docFolder">The directory where the letter is saved</param>
    ''' <returns>TestModeResults with all object level variables populated</returns>
    Protected Function TestMode(ByVal docFolder As String) As TestModeResults
        Return Common.TestMode(docFolder, TestModeProperty)
    End Function

    ''' <summary>
    ''' Replicates VBA method for closing a file.
    ''' </summary>
    ''' <param name="fileNumbers">File handle numbers</param>
    Protected Sub VbaStyleFileClose(ByVal ParamArray fileNumbers() As Integer)
        Common.VbaStyleFileClose(fileNumbers)
    End Sub

    ''' <summary>
    ''' Replicates VBA method for inputing a field from a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <returns></returns>
    Protected Function VbaStyleFileInput(ByVal fileNumber As Integer) As String
        Return Common.VbaStyleFileInput(fileNumber)
    End Function

    ''' <summary>
    ''' Replicates VBA method for inputing a line from a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <returns></returns>
    Protected Function VbaStyleFileLineInput(ByVal fileNumber As Object) As String
        Return Common.VbaStyleFileLineInput(fileNumber)
    End Function

    ''' <summary>
    ''' Replicates VBA method for opening a file
    ''' </summary>
    ''' <param name="fileName">File name</param>
    ''' <param name="fileNumber">File handle number</param>
    ''' <param name="openMode">Open mode</param>
    Protected Sub VbaStyleFileOpen(ByVal fileName As String, ByVal fileNumber As Integer, ByVal openMode As Common.MSOpenMode)
        Common.VbaStyleFileOpen(fileName, fileNumber, openMode)
    End Sub

    ''' <summary>
    ''' Replicates VBA method for writing to a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <param name="fields">Fields to add</param>
    Protected Sub VbaStyleFileWrite(ByVal fileNumber As Integer, ByVal ParamArray fields() As String)
        Common.VbaStyleFileWrite(fileNumber, fields)
    End Sub

    ''' <summary>
    ''' Replicates VBA method for writing a line to a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <param name="output">Fields to output</param>
    Protected Sub VbaStyleFileWriteLine(ByVal fileNumber As Integer, ByVal ParamArray output() As String)
        Common.VbaStyleFileWriteLine(fileNumber, output)
    End Sub

    ''' <summary>
    ''' Replicates VBA method for EOF
    ''' </summary>
    ''' <param name="fileNumber"></param>
    ''' <remarks></remarks>
    Protected Function VbaStyleEOF(ByVal fileNumber) As Boolean
        Return Common.VbaStyleEOF(fileNumber)
    End Function

    ''' <summary>
    ''' Gets the user's Windows user name from Environment object
    ''' </summary>
    Protected Function WindowsUserName() As String
        Return Common.WindowsUserName()
    End Function

    ''' <summary>
    ''' Calculates the ACS keyline for the borrower
    ''' </summary>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <param name="personType">Person type (was defaulted to borrower)</param>
    ''' <param name="addressType">Address type (was defaulted to legal)</param>
    ''' <returns>ACS Keyline</returns>
    ''' <remarks></remarks>
    Protected Function ACSKeyLine(ByVal ssn As String, ByVal personType As ACSKeyLinePersonType, ByVal addressType As ACSKeyLineAddressType) As String
        Return Common.ACSKeyLine(ssn, personType, addressType)
    End Function

    ''' <summary>
    ''' Confirms or denies whether the lender code is affiliated with UHEAA based off database table data.
    ''' </summary>
    ''' <param name="lenderCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function IsAffiliatedLenderCode(ByVal lenderCode As String) As Boolean
        Return Common.IsAffiliatedLenderCode(lenderCode, TestModeProperty)
    End Function

    ''' <summary>
    ''' Confirms or denies whether the lender code is affiliated with lender name based off database table data.
    ''' </summary>
    ''' <param name="lenderCode"></param>
    ''' <param name="lenderName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function IsAffiliatedLenderCode(ByVal lenderCode As String, ByVal lenderName As String) As Boolean
        Return Common.IsAffiliatedLenderCode(lenderCode, TestModeProperty, lenderName)
    End Function

End Class
