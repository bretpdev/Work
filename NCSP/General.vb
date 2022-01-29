Imports System
Imports System.IO
Imports Word

Module General
    Public IsTestMode As Boolean = True
    Public dbConnection As SqlClient.SqlConnection
    Public DocFolder As String
    Public DocPath As String
    Public BatchDocPath As String
    Public UserID As String
    Public UserAccess As String


#Region "Jay Davis generated code "
    'get db connection and other values for live and test modes
    Sub GetdbConnection()
        If IsTestMode Then
            'test
            DocFolder = "X:\PADD\NCSP\Test\"
            DocPath = "X:\PADD\NCSP\Test\Accounts\"
            BatchDocPath = "X:\PADD\NCSP\Test\Payment Batches\"
            dbConnection = New SqlClient.SqlConnection("workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""BART\BART"";persist security info=False;initial catalog=NCSP")
        Else
            'live
            DocFolder = "X:\PADD\NCSP\"
            DocPath = "Q:\New Century\New System\Current Accounts\"
            BatchDocPath = "Q:\New Century\New System\Payment Batches\"
            dbConnection = New SqlClient.SqlConnection("workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""NOCHOUSE"";persist security info=False;initial catalog=NCSP")
        End If
    End Sub

    'get the number of paid schedules for the account
    Function GetPaidSchedules(ByVal AcctID As String) As Integer
        Dim sqlCmd As New SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader

        'populate paid schedules
        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        sqlCmd.CommandText = "SELECT Count(Distinct SchedID) AS PaidSchedules FROM Schedule WHERE AcctID = '" & AcctID & "' AND SchedStat IN ('Paid','Payment Pending','Low GPA') AND RowStatus = 'A'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        GetPaidSchedules = Val(sqlRdr("PaidSchedules"))
        sqlRdr.Close()
        dbConnection.Close()
    End Function

    'shut down the applicaiton
    Sub ShutDown()
        Dim sqlCmd As New SqlClient.SqlCommand

        'open db connection
        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        Try
            'update user log
            sqlCmd.CommandText = "UPDATE UserLog SET Logout = GETDATE() WHERE UserID  = '" & UserID & "' AND Logout IS NULL"
            sqlCmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("The application cannot close because it was unable to update the user log.  Try closing the application again.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        Finally
            dbConnection.Close()
        End Try

        'close application
        End
    End Sub

    'print documents
    Sub PrintDoc(ByVal Doc As String, ByVal Folder As String, ByVal TransID As String, Optional ByVal PrintedPrompt As Boolean = True, Optional ByVal Dat As String = "T:\NCSPdat.txt")
        Dim newDoc As String

        'set path and file name of merged document to save and of the merge document to use
        newDoc = DocPath & Folder & "\" & Doc & "_" & TransID & ".doc"
        Doc = DocFolder & Doc & ".doc"

        'create the document
        Dim Word As New Microsoft.Office.Interop.Word.Application
        Word.Visible = False
        With Word
            'open merge document
            Word.Documents.Open(FileName:=Doc, ConfirmConversions:=False, _
                ReadOnly:=True, AddToRecentFiles:=False, PasswordDocument:="", _
                PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
                WritePasswordTemplate:="")
            'set data file
            Word.ActiveDocument.MailMerge.OpenDataSource(Name:=Dat, _
                ConfirmConversions:=False, ReadOnly:= _
                False, LinkToSource:=True, AddToRecentFiles:=False, PasswordDocument:="", _
                PasswordTemplate:="", WritePasswordDocument:="", WritePasswordTemplate:= _
                "", Revert:=False, Connection:="", SQLStatement _
                :="", SQLStatement1:="")
            'perform merge
            With .ActiveDocument.MailMerge
                .Destination = .Destination.wdSendToNewDocument
                .SuppressBlankLines = True
                .Execute(Pause:=False)
            End With
            'close form file
            Word.Documents(Doc).Close(False)
            'password protect and save the document
            Word.ActiveDocument.Protect(2, , "DX854av")
            Word.ActiveDocument.SaveAs(FileName:=newDoc)
            'display document if in test mode
            If IsTestMode Then
                Word.Visible = True
                'print document, close Word, and prompt user 
            Else
                Word.ActiveDocument.PrintOut(Background:=False)
                Word.Application.Quit(False)
                If PrintedPrompt Then MsgBox("Please retrieve your document from the printer.", MsgBoxStyle.Information, "Document Printed")
            End If
        End With
    End Sub

    Function GetLetterTextForIneligibilityReason(ByRef ineligibilityReason As String) As String
        Dim sqlCmd As New SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader

        'get the text for the letter for the eligibility end reason
        sqlCmd.CommandText = "SELECT LetterText FROM EligEnd WHERE EligEndReason = '" & ineligibilityReason & "'"
        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        GetLetterTextForIneligibilityReason = sqlRdr("LetterText")
        sqlRdr.Close()
        dbConnection.Close()
    End Function
    'closes the account
    Public Sub PrintNotatePromptOnCloseAccount(ByVal closedReason As String, ByVal accountId As String, ByVal first As String, ByVal last As String, ByVal street1 As String, ByVal street2 As String, ByVal city As String, ByVal state As String, ByVal zip As String, ByVal balance As String)
        'Dim sqlCmd As New SqlClient.SqlCommand
        'Dim sqlRdr As SqlClient.SqlDataReader
        Dim ComDt As String
        Dim TransID As String
        'Dim balanceOwedText As String = ""
        'Dim textFromTable As String = ""

        ''if a balance is owed to the scholarship program, add text to that effect to be merged into the letter
        'If CDbl(balance) < 0 Then
        '    sqlCmd.Connection = dbConnection
        '    dbConnection.Open()
        '    sqlCmd.CommandText = "SELECT BalanceOwedText FROM BalanceOwedText"
        '    sqlRdr = sqlCmd.ExecuteReader
        '    sqlRdr.Read()
        '    'remove negative sign and format with $ and merge with text from the table
        '    balanceOwedText = Replace(sqlRdr("BalanceOwedText"), "[[[Balance]]]", (CDbl(balance * -1)).ToString("C"))
        '    dbConnection.Close()
        'End If

        'sqlCmd.Connection = dbConnection
        'dbConnection.Open()

        ''get the text for the letter for the eligibility end reason
        'sqlCmd.CommandText = "SELECT LetterText FROM EligEnd WHERE EligEndReason = '" & closedReason & "'"
        'sqlRdr = sqlCmd.ExecuteReader
        'sqlRdr.Read()

        'Do not create a letter if the closed reason is deceased
        If closedReason.Contains("Deceased") Then
            Exit Sub
        End If

        'create a data file
        FileOpen(1, "T:\NCSPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
        'WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Reason", "BalanceOwed")
        'WriteLine(1, accountId, first, last, street1, street2, city, state, zip, GetLetterTextForIneligibilityReason(closedReason), balanceOwedText)
        WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Reason", "StaticCurrentDate")
        WriteLine(1, accountId, first, last, street1, street2, city, state, zip, GetLetterTextForIneligibilityReason(closedReason), Date.Now.ToShortDateString())
        FileClose(1)

        'sqlRdr.Close()
        'dbConnection.Close()

        'detemine comment date and ID for document file name so the comment can find the file
        ComDt = Now()
        TransID = Format(DateValue(ComDt), "MMddyyyy") & Format(TimeValue(ComDt), "HHmmss")
        If closedReason.Contains("Denied") Then
            'print the letter, add a communications record, and prompt the user
            PrintDoc("NCSPDENY", accountId & "\Communications", TransID, False)
            AddCommunications(accountId, ComDt, False, True, "Letter", closedReason & ", account closed, NSCP DENIAL letter sent " & Format(DateValue(Now()), "MMddyyyy"), "Denial Letter – " & closedReason)
            MsgBox("The account has been closed for the following reason: " & closedReason & ".  Please retrieve the NSCP DENIAL letter from the printer.", MsgBoxStyle.Information, "New Century Scholarship Program")
        ElseIf Not closedReason.Contains("Closed") Then
            'print the letter, add a communications record, and prompt the user
            PrintDoc("NCSACNTCLS", accountId & "\Communications", TransID, False)
            AddCommunications(accountId, ComDt, False, True, "Letter", closedReason & ", account closed, NSCP ACCOUNT CLOSED letter sent " & Format(DateValue(Now()), "MMddyyyy"), "Account Closed Letter – " & closedReason)
            MsgBox("The account has been closed for the following reason: " & closedReason & ".  Please retrieve the NSCP ACCOUNT CLOSED letter from the printer.", MsgBoxStyle.Information, "New Century Scholarship Program")
        End If
    End Sub

    'gets student info and closes account
    Public Sub PrintNotatePromptOnCloseAccount(ByVal closedReason As String, ByVal accountId As String)
        Dim sqlCmd As New SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim fName As String
        Dim lName As String
        Dim add1 As String
        Dim add2 As String
        Dim city As String
        Dim st As String
        Dim zip As String
        Dim balance As String

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'get student info
        sqlCmd.CommandText = "SELECT * FROM Account WHERE AcctID = '" & accountId & "' AND RowStatus = 'A'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()

        fName = sqlRdr("FName")
        lName = sqlRdr("LName")
        add1 = sqlRdr("Add1")
        add2 = sqlRdr("Add2")
        city = sqlRdr("City")
        st = sqlRdr("ST")
        zip = sqlRdr("Zip")
        balance = sqlRdr("Balance")

        sqlRdr.Close()
        dbConnection.Close()

        PrintNotatePromptOnCloseAccount(closedReason, accountId, fName, lName, add1, add2, city, st, zip, balance)
    End Sub
#End Region

#Region "Trent Packer generated code"
    'add financial transaction
    Function AddTransaction(ByVal TranTyp As String, ByVal AcctID As String, ByVal SchedSemEnr As String, ByVal SchedYrEnr As Integer, ByVal SchedInst As String, ByVal TranHrPd As Double, ByVal TranAmt As Double, ByVal SchedID As Integer, Optional ByVal BatchNumber As String = "", Optional ByVal BatchDate As String = "", Optional ByVal TranStat As String = "") As Boolean
        Dim SC As SqlClient.SqlCommand
        If BatchNumber = "" Then
            SC = New SqlClient.SqlCommand("INSERT INTO [Transaction] (TranTyp, AcctID, SchedSemEnr, SchedYrEnr, SchedInst, TranHrPd, TranAmt, UserID, SchedID) values('" & TranTyp & "','" & AcctID & "','" & Replace(SchedSemEnr, "'", "''") & "'," & SchedYrEnr & ",'" & Replace(SchedInst, "'", "''") & "'," & TranHrPd & "," & TranAmt & ",'" & UserID & "'," & SchedID & ")", dbConnection)
        Else
            SC = New SqlClient.SqlCommand("INSERT INTO [Transaction] (TranTyp, AcctID, SchedSemEnr, SchedYrEnr, SchedInst, TranHrPd, TranAmt, UserID, SchedID, NCSPBatchNum, NCSPBatchDt, TranStat) values('" & TranTyp & "','" & AcctID & "','" & Replace(SchedSemEnr, "'", "''") & "'," & SchedYrEnr & ",'" & Replace(SchedInst, "'", "''") & "'," & TranHrPd & "," & TranAmt & ",'" & UserID & "'," & SchedID & ",'" & BatchNumber & "','" & BatchDate & "','" & TranStat & "')", dbConnection)
        End If
        Try
            dbConnection.Open()
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The transaction was not able to be added for the following reason:  " & ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Return False
            Exit Function
        Finally
            dbConnection.Close()
        End Try
        Return True
    End Function

    Function AddToAccountBallance(ByVal amt As Double, ByVal Acct As String) As Boolean
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim Bal As Double
        Dim val As String
        SC = New SqlClient.SqlCommand("select AcctRecSeq from Account where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
        Try
            'Get Active row id
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The balance was not updated because the account ID was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Exit Function
            End If
            val = sqlRdr("AcctRecSeq")
            sqlRdr.Close()

            'Duplicate Active row!
            SC = New SqlClient.SqlCommand("insert Account (AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, LastUpdateDt, LastUpdateUser, RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied) select AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, GETDATE(), '" & UserID & "', RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied from Account Where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()

            'Set old record status to "H" for history
            SC = New SqlClient.SqlCommand("update Account set RowStatus = 'H', LockedBy = '' where AcctRecSeq = '" & val & "'", dbConnection)
            SC.ExecuteNonQuery()

            'get balance
            SC = New SqlClient.SqlCommand("Select Balance from Account where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The balance was not updated because the account ID was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Return False
                Exit Function
            End If
            Bal = CDbl(sqlRdr("Balance"))
            sqlRdr.Close()

            'add amount
            Bal = Bal + amt

            'update account
            SC = New SqlClient.SqlCommand("update Account set Balance = " & Bal & " where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The balance was not updated for the following reason:  " & ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Return False
            Exit Function
        Finally
            dbConnection.Close()
        End Try
        Return True
    End Function


    'add communications record
    Function AddCommunications(ByVal AcctID As String, ByVal ComRecDt As String, ByVal Incomming As Boolean, ByVal Outgoing As Boolean, ByVal ComType As String, ByVal ComRecCmnt As String, ByVal Subject As String) As Boolean
        Dim SC As SqlClient.SqlCommand
        Dim tIncomming As Integer
        Dim tOutgoing As Integer
        If Incomming = True Then tIncomming = 1 Else tIncomming = 0
        If Outgoing = True Then tOutgoing = 1 Else tOutgoing = 0
        SC = New SqlClient.SqlCommand("insert into ComRec (AcctID,ComRecDt,InComing,Outgoing,ComType,ComRecCmnt,UserID,Subject) Values ('" & AcctID & "','" & ComRecDt & "'," & tIncomming & "," & tOutgoing & ",'" & Replace(ComType, "'", "''") & "','" & Replace(ComRecCmnt, "'", "''") & "','" & UserID & "','" & Replace(Subject, "'", "''") & "')", dbConnection)
        Try
            dbConnection.Open()
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The communications record was not able to be added for the following reason:  " & ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Return False
            Exit Function
        Finally
            dbConnection.Close()
        End Try
        Return True
    End Function

    Function GetScholarshipAmount(ByRef SchoolYear As String) As Double
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader

        Try
            SC = New SqlClient.SqlCommand("SELECT Amount FROM ScholarshipAmount WHERE SchoolYear = '" & SchoolYear & "' and RowStatus = 'A'", dbConnection)
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("There was an error determining the tuition amount.  There may not be an amount listed for year on the schedule.", MsgBoxStyle.Critical, "New Century Scholarship Program")
                Return False
                Exit Function
            End If
            GetScholarshipAmount = CDbl(sqlRdr("Amount"))
            sqlRdr.Close()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try

    End Function

    Function GetTuition(ByVal InstID As String, ByVal TmPrd As String, ByVal FY As Integer, ByVal CrdHrs As Double) As Double
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim Tu As Double
        Dim IntPart As Integer
        Dim DecPart As Double

        IntPart = Int(CrdHrs)
        DecPart = CrdHrs - IntPart

        If DecPart > 0 Then
            'get the difference of the two closest hours and multiply it by the remainder
            Dim baseCrdHrs As Double
            Dim roofCrdHrs As Double
            Dim baseTu As Double
            Dim roofTu As Double
            baseCrdHrs = IntPart
            roofCrdHrs = IntPart + 1

            'BASE
            Try
                SC = New SqlClient.SqlCommand("Select ch" & baseCrdHrs & " from Tuition where InstID = '" & InstID & "' and Tmprd = '" & TmPrd & "' and Fy = " & FY, dbConnection)
                dbConnection.Open()
                sqlRdr = SC.ExecuteReader
                sqlRdr.Read()
                If Not sqlRdr.HasRows Then
                    MsgBox("There was an error determining the tuition amount.  There may not be an amount listed for institution/semester/year on the schedule.", MsgBoxStyle.Critical, "New Century Scholarship Program")
                    Return False
                    Exit Function
                End If
                baseTu = CDbl(sqlRdr("ch" & baseCrdHrs))
                sqlRdr.Close()
                dbConnection.Close()
            Catch ex As Exception
                MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Finally
                dbConnection.Close()
            End Try
            'ROOF
            Try
                SC = New SqlClient.SqlCommand("Select ch" & roofCrdHrs & " from Tuition where InstID = '" & InstID & "' and Tmprd = '" & TmPrd & "' and Fy = " & FY, dbConnection)
                dbConnection.Open()
                sqlRdr = SC.ExecuteReader
                sqlRdr.Read()
                If Not sqlRdr.HasRows Then
                    MsgBox("There was an error determining the tuition amount.  There may not be an amount listed for institution/semester/year on the schedule.", MsgBoxStyle.Critical, "New Century Scholarship Program")
                    Return False
                    Exit Function
                End If
                roofTu = CDbl(sqlRdr("ch" & roofCrdHrs))
                sqlRdr.Close()
                dbConnection.Close()
            Catch ex As Exception
                MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Finally
                dbConnection.Close()
            End Try
            Tu = baseTu + ((roofTu - baseTu) * DecPart)
            Return CDbl(Format(Tu, "f"))

        Else
            Try
                SC = New SqlClient.SqlCommand("Select ch" & CrdHrs & " from Tuition where InstID = '" & InstID & "' and Tmprd = '" & TmPrd & "' and Fy = " & FY, dbConnection)
                dbConnection.Open()
                sqlRdr = SC.ExecuteReader
                sqlRdr.Read()
                If Not sqlRdr.HasRows Then
                    MsgBox("There was an error determining the tuition amount.  There may not be an amount listed for institution/semester/year on the schedule.", MsgBoxStyle.Critical, "New Century Scholarship Program")
                    Return False
                    Exit Function
                End If
                Tu = CDbl(sqlRdr("ch" & CrdHrs))
                sqlRdr.Close()
                dbConnection.Close()
            Catch ex As Exception
                MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Finally
                dbConnection.Close()
            End Try
            Return CDbl(Format(Tu, "f"))
        End If
    End Function

    Function GetDifferenceInTuition(ByVal InstID As String, ByVal TmPrd As String, ByVal FY As Integer, ByVal StartHours As Double, ByVal EndHours As Double) As Double
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim startAmt As Double = 0
        Dim EndAmt As Double = 0

        Try
            'Get Starting Paid Amount
            SC = New SqlClient.SqlCommand("Select ch" & StartHours & " from Tuition where InstID = '" & InstID & "' and Tmprd = '" & TmPrd & "' and Fy = " & FY, dbConnection)
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("There was an error determining the Paid tuition amount.  There may not be an amount listed for institution/semester/year on the schedule.", MsgBoxStyle.Critical, "New Century Scholarship Program")
                Return False
                Exit Function
            End If
            startAmt = CDbl(sqlRdr("ch" & StartHours))
            sqlRdr.Close()

            'Get Completed Paid Amount
            SC = New SqlClient.SqlCommand("Select ch" & EndHours & " from Tuition where InstID = '" & InstID & "' and Tmprd = '" & TmPrd & "' and Fy = " & FY, dbConnection)
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("There was an error determining the Paid tuition amount.  There may not be an amount listed for institution/semester/year on the schedule.", MsgBoxStyle.Critical, "New Century Scholarship Program")
                Return False
                Exit Function
            End If
            EndAmt = CDbl(sqlRdr("ch" & EndHours))
            sqlRdr.Close()

        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try

        'Return the starting amount - the ending amount
        Return startAmt - EndAmt
    End Function

    Function GetFromAccount(ByVal attribute As String, ByVal Acct As String) As String
        'get an attribute from the account table with account ID
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String = String.Empty
        Try
            SC = New SqlClient.SqlCommand("Select " & attribute & " from Account where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The account ID or attribute was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Return ""
                Exit Function
            End If
            val = sqlRdr(attribute)
            sqlRdr.Close()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
        Return val
    End Function

    Function GetFromSchedule(ByVal attribute As String, ByVal Acct As String, ByVal SchedID As String) As String
        'Get an attribute from the schedule table with the account ID and schedule ID
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String = String.Empty
        Try
            SC = New SqlClient.SqlCommand("Select " & attribute & " from Schedule where RowStatus = 'A' and AcctID = '" & Acct & "' and SchedID = " & SchedID, dbConnection)
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The account ID, schedule ID, or attribute was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Return ""
                Exit Function
            End If
            val = sqlRdr(attribute)
            sqlRdr.Close()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox(ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
        Return val
    End Function

    Sub updateScheduleByAttribute(ByVal Acct As String, ByVal SchedID As Long, ByVal attribute As String, ByVal value As String)
        'Updates the schedule table with a specific attribute and value for a specific account and schedule ID
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String
        SC = New SqlClient.SqlCommand("select SchdlRecSeq from Schedule where AcctID = '" & Acct & "' and RowStatus = 'A' and SchedID = " & SchedID, dbConnection)
        Try
            'Get Active row id
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The schedule ID or attribute was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Exit Sub
            End If
            val = sqlRdr("SchdlRecSeq")
            sqlRdr.Close()

            'Duplicate Active row
            SC = New SqlClient.SqlCommand("insert Schedule (AcctID, SchedID, SchedDt, SchedStat, SchedHrRem, CredHrPd, InstAtt, Semester, SchedYr, CredHrEnr, CredHrComp, SemesterGPA, LTH, LastUpdateDt, LastUpdateUser, RowStatus, LockedBy ) select AcctID, SchedID, SchedDt, SchedStat, SchedHrRem, CredHrPd, InstAtt, Semester, SchedYr, CredHrEnr, CredHrComp, SemesterGPA, LTH, GETDATE(), '" & UserID & "', RowStatus, LockedBy from Schedule Where AcctID = '" & Acct & "' and SchedID = " & SchedID & " and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()

            'Set old record status to "H" for history
            SC = New SqlClient.SqlCommand("update Schedule set RowStatus = 'H', LockedBy = '' where SchdlRecSeq = " & val, dbConnection)
            SC.ExecuteNonQuery()

            'Update New Record
            SC = New SqlClient.SqlCommand("update Schedule set " & attribute & " = '" & value & "' where AcctID = '" & Acct & "' and RowStatus = 'A' and SchedID = " & SchedID, dbConnection)
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The schedule update was not completed for the following reason:  " & ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Exit Sub
        Finally
            dbConnection.Close()
        End Try
    End Sub


    Sub updateAccountByAttribute(ByVal Acct As String, ByVal attribute As String, ByVal value As String)
        'Updates the schedule table with a specific attribute and value for a specific account and schedule ID
        Dim SC As SqlClient.SqlCommand
        Dim sqlRdr As SqlClient.SqlDataReader
        Dim val As String
        SC = New SqlClient.SqlCommand("select AcctRecSeq from Account where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
        Try
            'Get Active row id
            dbConnection.Open()
            sqlRdr = SC.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The account ID or attribute was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
                Exit Sub
            End If
            val = sqlRdr("AcctRecSeq")
            sqlRdr.Close()


            'Duplicate Active row!
            SC = New SqlClient.SqlCommand("insert Account (AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, LastUpdateDt, LastUpdateUser, RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied) select AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, GETDATE(), '" & UserID & "', RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied from Account Where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()

            'Set old record status to "H" for history
            SC = New SqlClient.SqlCommand("update Account set RowStatus = 'H', LockedBy = '' where AcctRecSeq = '" & val & "'", dbConnection)
            SC.ExecuteNonQuery()

            'Update new record
            SC = New SqlClient.SqlCommand("update Account set " & attribute & " = '" & value & "' where AcctID = '" & Acct & "' and RowStatus = 'A'", dbConnection)
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The account update was not completed for the following reason:  " & ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Exit Sub
        Finally
            dbConnection.Close()
        End Try
    End Sub


    Sub updateTransactionByAttribute(ByVal TranID As String, ByVal attribute As String, ByVal value As String)
        'Updates the schedule table with a specific attribute and value for a specific account and schedule ID
        Dim SC As SqlClient.SqlCommand
        SC = New SqlClient.SqlCommand("update [Transaction] set " & attribute & " = '" & value & "' where TranID = '" & TranID & "'", dbConnection)
        Try
            dbConnection.Open()
            SC.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The transaction update was not completed for the following reason:  " & ex.Message & "  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
            Exit Sub
        Finally
            dbConnection.Close()
        End Try
    End Sub

    Function SemesterInteger(ByVal Sem As String) As Integer
        If Sem = "Spring" Then
            Return 1
        ElseIf Sem = "Summer" Then
            Return 2
        ElseIf Sem = "Fall" Then
            Return 3
        ElseIf Sem = "Winter" Then
            Return 4
        End If
    End Function

#End Region
End Module
