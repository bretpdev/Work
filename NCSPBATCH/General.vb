Imports Word

Module General
    Private sqlCmd As New SqlClient.SqlCommand
    Private sqlRdr As SqlClient.SqlDataReader

    'update the account, add a communications record, and write the data to the letter data file
    Public Function UpdateAddWriteSave(ByVal DR As DataRow, ByVal eligEndReason As String, ByVal lettertext As String)
        Dim balancedOwed As String = ""

        'update account table
        UpdateAccount(DR.Item("AcctID"), eligEndReason)

        'add communication record
        AddCommunications(DR.Item("AcctId"), Now(), False, True, "Letter", eligEndReason & ", account closed, closed account letter sent " & Format(DateValue(Now()), "MMddyyyy"), "Closed Account Letter – " & eligEndReason)

        If DR.Item("Balance") <> 0 Then
            balancedOwed = GetBalanceOwedText(DR.Item("Balance"))
        End If

        'add to letter data file for printing
        FileOpen(1, "T:\NCSP_batch_dat.txt", OpenMode.Append, OpenAccess.Write, OpenShare.LockWrite)
        WriteLine(1, DR.Item("AcctId"), DR.Item("FName"), DR.Item("LName"), DR.Item("Add1"), DR.Item("Add2"), DR.Item("City"), DR.Item("ST"), DR.Item("Zip"), lettertext, balancedOwed)
        FileClose(1)

        'add to letter data file to save copy of letter
        FileOpen(1, "T:\NCSPdat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
        WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Reason", "BalanceOwed")
        WriteLine(1, DR.Item("AcctId"), DR.Item("FName"), DR.Item("LName"), DR.Item("Add1"), DR.Item("Add2"), DR.Item("City"), DR.Item("ST"), DR.Item("Zip"), lettertext, balancedOwed)
        FileClose(1)

        'save a copy of the letter
        SaveDoc("NCSACNTCLS", DR.Item("AcctId") & "\Communications", Format(DateValue(Now()), "MMddyyyy") & Format(TimeValue(Now()), "HHmmss"))

        'update counter
        ProcessedRecordsCount = ProcessedRecordsCount + 1
    End Function

    'add communications record
    Public Function AddCommunications(ByVal AcctID As String, ByVal ComRecDt As String, ByVal Incomming As Boolean, ByVal Outgoing As Boolean, ByVal ComType As String, ByVal ComRecCmnt As String, ByVal Subject As String) As Boolean
        Dim tIncomming As Integer
        Dim tOutgoing As Integer

        If Incomming = True Then tIncomming = 1 Else tIncomming = 0
        If Outgoing = True Then tOutgoing = 1 Else tOutgoing = 0
        sqlCmd = New SqlClient.SqlCommand("insert into ComRec (AcctID,ComRecDt,InComing,Outgoing,ComType,ComRecCmnt,UserID,Subject) Values ('" & AcctID & "','" & ComRecDt & "'," & tIncomming & "," & tOutgoing & ",'" & Replace(ComType, "'", "''") & "','" & Replace(ComRecCmnt, "'", "''") & "','NCSPBATCH','" & Replace(Subject, "'", "''") & "')", dbConnection)
        Try
            dbConnection.Open()
            sqlCmd.ExecuteNonQuery()
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

    'update the account
    Public Function UpdateAccount(ByVal acctId As String, ByVal eligEndedReason As String)
        Dim acctRecSeq As String

        sqlCmd = New SqlClient.SqlCommand("select AcctRecSeq from Account where AcctID = '" & acctId & "' and RowStatus = 'A'", dbConnection)
        Try
            'Get Active row id
            dbConnection.Open()
            sqlRdr = sqlCmd.ExecuteReader
            sqlRdr.Read()
            If Not sqlRdr.HasRows Then
                MsgBox("The account ID was not found.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            End If
            acctRecSeq = sqlRdr("AcctRecSeq")
            sqlRdr.Close()

            'Duplicate Active row!
            sqlCmd = New SqlClient.SqlCommand("insert Account (AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, LastUpdateDt, LastUpdateUser, RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied) select AcctID, Status, Balance, CredHrRem, LName, AltLName, FName, MI, Add1, Add2, City, ST, Zip, DOB, SSN, Phone, Email, Gender, EthnicBG, AppRcvDt, AppAprDt, HSAttended, HSDist, HSGradDt, HSGPA, ActScore, OffTranRcvDt, DegreeInst, Degree, DegreeComplDt, DegreeGPA, DegreeTranRcvDt, EligEndDt, EligEndRea, EligEndCom, LowGPA, LowGPACom, LOAStart, LOAEnd, LOACom, GETDATE(), 'NCSPBATCH', RowStatus, LockedBy, CellPhone, Citizenship, Criminal, InfoRelease, AppInst, AppApvdCom, LOADeferral, LOALOA, LOARequested, LOASemReturn, LOAApproved, LOADenied from Account Where AcctID = '" & acctId & "' and RowStatus = 'A'", dbConnection)
            sqlCmd.ExecuteNonQuery()

            'Set old record status to "H" for history
            sqlCmd = New SqlClient.SqlCommand("update Account set RowStatus = 'H' where AcctRecSeq = '" & acctRecSeq & "'", dbConnection)
            sqlCmd.ExecuteNonQuery()

            'Update new record
            sqlCmd = New SqlClient.SqlCommand("UPDATE Account SET Status = 'Closed', EligEndDt = '" & Today & "', EligEndRea = '" & eligEndedReason & "' WHERE AcctID = '" & acctId & "' and RowStatus = 'A'", dbConnection)
            sqlCmd.ExecuteNonQuery()
            dbConnection.Close()
        Catch ex As Exception
            MsgBox("The account update was not completed.  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Critical, "New Century Scholarship Program")
        Finally
            dbConnection.Close()
        End Try
    End Function

    'print documents
    Function SaveDoc(ByVal Doc As String, ByVal Folder As String, ByVal TransID As String, Optional ByVal Dat As String = "T:\NCSPdat.txt")
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
            Word.Application.Quit(False)

        End With
    End Function

    'print documents
    Function PrintDocs(ByVal Doc As String, Optional ByVal Dat As String = "T:\NCSP_batch_dat.txt")
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

            'display document if in test mode
            If IsTestMode Then
                Word.Visible = True
                'print document, close Word, and prompt user 
            Else
                Word.ActiveDocument.PrintOut(Background:=False)
                Word.Application.Quit(False)
            End If
        End With
    End Function

    'reset progress bar on splash screen
    Public Function ResetProgressBar(ByVal max As Integer, ByVal labelText As String)
        SplashForm.lblStatus.Text = labelText
        SplashForm.ProgressBar.Minimum = 0
        SplashForm.ProgressBar.Maximum = max
        SplashForm.ProgressBar.Value = 0
        SplashForm.ProgressBar.Step = 1
        SplashForm.ProgressBar.PerformStep()
        SplashForm.Refresh()
    End Function
End Module
