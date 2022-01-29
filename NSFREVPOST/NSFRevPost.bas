Attribute VB_Name = "NSFRevPost"
Private ssn As String
Private AN As String
Private BatchCode As String
Private EffDate As String
Private Batch As String
Private PmtAmt As String
Private ACH As String
Private userId As String
Private NSFReason As String
Private TranType As String
'<2->
Private BatchCd As String
Private PmtCd As String
Private DPA As String
Private Sys As String
Private PostDate As String
Private lname As String
'</2>

Private batchNo As String
Private rUserID As String
Private DocFolder As String

Private pSta As Integer
Private pSSN As String
Private pEffDate As String
Private pBatchNo As String

Sub Main()
'<2->
    Load frmNSFRevPostSys
    frmNSFRevPostSys.Show
    
    If frmNSFRevPostSys.lbl1.Caption = "Processing reversals for Compass." Then
        DocFolder = "X:\PADD\Operational Accounting\Compass\"
        Sys = "Compass"
    Else
        DocFolder = "X:\PADD\Operational Accounting\OneLINK\"
        Sys = "OneLINK"
    End If
    
    Unload frmNSFRevPostSys

    'get folder for batch file
'    DocFolder = "X:\PADD\AccountServices\"
'</2>
    Sp.Common.TestMode , DocFolder

    'warn the user and end the script if there is no file to process
    If Dir(DocFolder & "NSF ENTRY.txt") = "" Then
        MsgBox "There is no reversal file to process.", 16, "No Reversal File"
        End
    End If
'<2->
    'go to sub for processing reversals from correct system
    If Sys = "Compass" Then CompassProc Else OneLINKProc

    'create backup of posting file and end
    CreateBak
    MsgBox "Processing Complete.", 64, "NSF Reversal Posting"

End Sub

'process compass reversals
Sub CompassProc()
'</2>
    'get the user id of the user
    rUserID = Sp.Common.GetUserID
    
    'get recovery values
    If Dir(DocFolder & "nsflog.txt") <> "" Then
        Open DocFolder & "nsflog.txt" For Input As #2
        Input #2, pSta, pSSN, pEffDate, pBatchNo
        Close #2
    End If
    
    If pSta < 5 Then
    
        'sort the file by SSN
        Sp.SortFile DocFolder & "NSF ENTRY.txt", 0
        
        'warn the user and end the script if the user entered any of the transactions
        Open DocFolder & "NSF ENTRY.txt" For Input As #1
        While Not EOF(1)
            Input #1, ssn, BatchCode, EffDate, Batch, PmtAmt, ACH, userId, NSFReason, TranType
            If userId = rUserID Then
                MsgBox "The same user cannot both enter and post reversal transactions.  Please have someone else post these transactions.", 16, "User Conflict"
                Close #1
                End
            End If
        Wend
        Close #1
    
        'prompt for transaction information
        frmNSFRevPost.Show
    
        'post payments
        If CDbl(frmNSFRevPost.CashNo) > 0 And pSta < 2 Then TS1G "1"
        If CDbl(frmNSFRevPost.WireNo) > 0 And pSta < 4 Then TS1G "2"
    
    
        'update the log
        Open DocFolder & "nsflog.txt" For Output As #2
        Write #2, 5, "", "", ""
        Close #2
        
        'add activity records
        Open DocFolder & "NSF ENTRY.txt" For Input As #1
        'find the last SSN processed in case of recovery
        ssn = ""
        EffDate = ""
        Do Until ssn = pSSN And Format(EffDate, "MMDDYY") = pEffDate
            Input #1, ssn, BatchCode, EffDate, Batch, PmtAmt, ACH, userId, NSFReason, TranType
        Loop
        'process remaining SSNs
        Dim previousSSN As String
        Dim previousEffDate As String
        While Not EOF(1)
            Input #1, ssn, BatchCode, EffDate, Batch, PmtAmt, ACH, userId, NSFReason, TranType
            'add the activity record
            If previousSSN <> ssn Or previousEffDate <> EffDate Then 'only process if one of them is not the same as the last
                previousSSN = ssn
                previousEffDate = EffDate
                If BatchCode = 1 Then
                    Sp.Common.ATD22AllLoans ssn, "PMNSF", "Payment effective " & Format(EffDate, "MMDDYYYY") & " for " & Format(PmtAmt, "$###,##0.00") & " was reversed due to " & NSFReason, "NSFRevPost", rUserID
                ElseIf BatchCode = 2 And ACH = "N" Then
                    Sp.Common.ATD22AllLoans ssn, "PMNSF", "Payment effective " & Format(EffDate, "MMDDYYYY") & " for " & Format(PmtAmt, "$###,##0.00") & " was reversed due to " & NSFReason, "NSFRevPost", rUserID
                ElseIf BatchCode = 2 And ACH = "Y" Then
                    If NSFReason = "Insufficient funds - 01" Or NSFReason = "Account Closed - 02" Or NSFReason = "Unable to locate account - 03" Or NSFReason = "Invalid account number - 04" _
                            Or NSFReason = "Prior authorization revoked - 07" Or NSFReason = "Stop payment - 08" Or NSFReason = "Uncollected/unavailable funds - 09" Or NSFReason = "Transaction unauthorized - 10" _
                            Or NSFReason = "Account frozen - 16" Or NSFReason = "Non-transaction account - 20" Or NSFReason = "Invalid ABA/routing number - 28" Then
                        Sp.Common.ATD22AllLoans ssn, "UNSFA", "Payment effective " & Format(EffDate, "MMDDYYYY") & " for " & Format(PmtAmt, "$###,##0.00") & " was reversed due to " & NSFReason, "NSFRevPost", rUserID
                    Else
                        Sp.Common.ATD22AllLoans ssn, "PMNSF", "Payment effective " & Format(EffDate, "MMDDYYYY") & " for " & Format(PmtAmt, "$###,##0.00") & " was reversed due to " & NSFReason, "NSFRevPost", rUserID
                    End If
                End If
            End If
            'update the log
            Open DocFolder & "nsflog.txt" For Output As #2
            Write #2, 5, ssn, Format(EffDate, "MMDDYY"), ""
            Close #2
        Wend
        Close #1
        
    End If
    
    
    'update the log
    Open DocFolder & "nsflog.txt" For Output As #2
    Write #2, 6, "", "", ""
    Close #2
    
    Dim name As String
    Dim dummy As String
    Dim Comment As String
    
    If Dir(DocFolder & "NON POSTING NSF ENTRY.txt") <> "" Then
        'add activity records
        Open DocFolder & "NON POSTING NSF ENTRY.txt" For Input As #1
        'find the last SSN processed in case of recovery
        ssn = ""
        EffDate = ""
        Do Until ssn = pSSN And Format(EffDate, "MMDDYY") = pEffDate
            Input #1, ssn, name, dummy, EffDate, PmtAmt, NSFReason
        Loop
        While Not EOF(1)
            Input #1, ssn, name, dummy, EffDate, PmtAmt, NSFReason
            Comment = "Payment effective " & Format(EffDate, "MMDDYYYY") & " for $ " & PmtAmt & " was returned due to " & NSFReason & ".  Payment tied to deconverted loans and has not been reversed by UHEAA."
            If Sp.Common.ATD22AllLoans(ssn, "NSFOT", Comment, "NSFREVPOST", rUserID) = False Then
                Sp.Common.ATD37FirstLoan ssn, "NSFOT", Comment, rUserID, "NSFREVPOST"
            End If
            'update the log
            Open DocFolder & "nsflog.txt" For Output As #2
            Write #2, 6, ssn, Format(EffDate, "MMDDYY"), ""
            Close #2
        Wend
        Close #1
    End If

    'print the error report if there is one
    If Dir(DocFolder & "NSF ENTRY ERR.txt") <> "" Then
        PrintErrorReport
        MsgBox "Processing is complete.  However, errors were encountered.  Please print and review the NSF Reversal Posting Script Error Report.", , "NSF Reveral Posting"
'<2->
    End If
    
'    Else
'        CreateBak
'        MsgBox "Processing Complete.", 64, "NSF Reversal Posting"
'    End If
'</2>
End Sub

'post reversals
Sub TS1G(BatCd As String)
    Dim BatTyp As String
    Dim row As Integer
    Dim selrow As String
    Dim MultipleMatches As Boolean
    'set up batch
    If (BatCd = "1" And pSta < 1) Or (BatCd = "2" And pSta < 3) Then
        FastPath "TX3Z/ATS1G;"
        puttext 6, 51, BatCd
        If BatCd = 1 Then
            puttext 10, 28, frmNSFRevPost.CashNo
            puttext 11, 28, frmNSFRevPost.CashAmt
            BatTyp = "cash"
        Else
            puttext 10, 28, frmNSFRevPost.WireNo
            puttext 11, 28, frmNSFRevPost.WireAmt
            BatTyp = "wire"
        End If
        puttext 12, 28, 10          'sub type
        puttext 14, 28, 1           'reversal reason
        puttext 15, 28, rUserID     'user ID
        hit "ENTER"
        batchNo = GetText(6, 18, 14)
        'update the log
        If BatCd = "1" Then pSta = 1 Else pSta = 3
        Open DocFolder & "nsflog.txt" For Output As #2
        Write #2, pSta, , , batchNo
        Close #2
    Else
        batchNo = pBatchNo
    End If
        
    'post reversals
    Open DocFolder & "NSF ENTRY.txt" For Input As #1
    While Not EOF(1)
        Input #1, ssn, BatchCode, EffDate, Batch, PmtAmt, ACH, userId, NSFReason, TranType
        If BatchCode = BatCd Then
            FastPath "TX3Z/ATS38" & batchNo
            puttext 8, 32, ssn
            puttext 10, 40, Format(EffDate, "MMDDYY")
            hit "ENTER"
            'look for the transaction with the same batch no if on the selection screen
            If check4text(1, 72, "TSX3D") Then
                selrow = ""
                row = 8
                Do
                    If check4text(row, 50, Batch) And val(GetText(row, 20, 10)) = val(PmtAmt) Then
                        'select the first row found
                        If selrow = "" Then
                            selrow = GetText(row, 5, 2)
                        'promote user to select the correct one if more than one is found
                        Else
                            MultipleMatches = True
                            Exit Do
                        End If
                    End If
                    row = row + 1
                    If check4text(row, 5, "  ") Then
                        hit "F8"
                        If check4text(23, 2, "90007") Then Exit Do
                    End If
                Loop
                'select the first transaction
                If MultipleMatches Then
                    MsgBox "There appears to be multiple possible selections and the script can't decide which to select.  Please select one and hit insert when ready to proceed."
                    Sp.PauseForInsert
                Else
                    If selrow <> "" Then puttext 21, 18, selrow, "Enter"
                End If
            End If
            If check4text(1, 72, "TSX3C") Then puttext 14, 77, Mid(NSFReason, Len(NSFReason) - 1, 2), "Enter"
        End If
    Wend
    Close #1
    
    'verify batch balances
    Do
        FastPath "TX3Z/CTS1R" & batchNo
        'warn the user and pause if the batch is not in balance
        If CDbl(GetText(10, 18, 7)) <> CDbl(GetText(9, 57, 7)) Or _
           CDbl(GetText(11, 29, 10)) <> CDbl(GetText(10, 68, 10)) Then
           MsgBox "The " & BatTyp & " batch does not balance.  Please correct either the detail in TS38 or the control data in TS1G and then press enter.", 48, "Batch Does not Balance"
           Sp.PauseForInsert
        'release the batch if it is in balance
        Else
            hit "F10"
            Exit Do
        End If
    Loop
    
    'update the log
    If BatCd = "1" Then pSta = 2 Else pSta = 4
    Open DocFolder & "nsflog.txt" For Output As #2
    Write #2, pSta, , , batchNo
    Close #2
End Sub

'print error report
Sub PrintErrorReport()
    Dim ErrReason As String                                             '<2>
    Dim Word As Word.Application
    Set Word = CreateObject("Word.Application")
    
    If Sys = "Compass" Then                                             '<2>
        With Word
            Open DocFolder & "NSF ENTRY ERR.txt" For Input As #3
            'open a blank Word doc
            .Documents.Add DocumentType:=wdNewBlankDocument
            .Visible = True
            'enter header
            .selection.TypeText Text:="NSF Reversal Posting Script Error Report"
            .selection.TypeParagraph
            .selection.TypeParagraph
            .selection.TypeText Text:="SSN, Batch Code, Effective Date, Batch Number, Payment, ACH, User Id, NSF Reason"
            .selection.TypeParagraph
            While Not EOF(3)
                Input #3, ssn, BatchCode, EffDate, Batch, PmtAmt, ACH, userId, NSFReason
                .selection.TypeParagraph
                .selection.TypeText Text:=ssn & "," & BatchCode & "," & EffDate & "," & Batch & "," & PmtAmt & "," & ACH & "," & userId & "," & NSFReason
            Wend
            Close #3
        End With
'<2->
    Else
        With Word
            Open DocFolder & "NSF ENTRY ERR.txt" For Input As #3
            'open a blank Word doc
            .Documents.Add DocumentType:=wdNewBlankDocument
            .Visible = True
            'enter header
            .selection.TypeText Text:="NSF Reversal Posting Script Error Report"
            .selection.TypeParagraph
            .selection.TypeParagraph
            .selection.TypeText Text:="SSN, Last Name, Effective Date, Posted Date, Payment, NSF, Trans Type, NSF Reason, Error Reason"
            .selection.TypeParagraph
            While Not EOF(3)
                Input #3, ssn, lname, EffDate, PostDate, PmtAmt, BatchCd, PmtCd, NSFReason, DPA, ErrReason
                .selection.TypeParagraph
                .selection.TypeText Text:=ssn & "," & lname & "," & EffDate & "," & PostDate & "," & PmtAmt & "," & BatchCd & "," & PmtCd & "," & NSFReason & "," & DPA & "," & ErrReason
            Wend
            Close #3
        End With
    End If
'</2>
    
    'clean up text files
    CreateBak
    
    'create a backup of the error file and delete the error file
    If Dir(DocFolder & "NSF ENTRY ERR.bak") <> "" Then Kill DocFolder & "NSF ENTRY ERR.bak"
    Name DocFolder & "NSF ENTRY ERR.txt" As DocFolder & "NSF ENTRY ERR.bak"
End Sub

'<2->
'process OneLINK payments
Sub OneLINKProc()
    Dim row As Integer
    Dim fndRow As Integer
    Dim ActCd As String
    Dim Comment As String
    Dim pamt As String
    Dim docPath As String
    Dim ToPrinter As Boolean
    
    Dim FirstName As String
    Dim LastName As String
    Dim Address1 As String
    Dim Address2 As String
    Dim City As String
    Dim State As String
    Dim Zip As String
    Dim Country As String
    
    'set processing values
    docPath = "X:\PADD\Collections\"
    If Sp.Common.TestMode(, docPath) Then ToPrinter = False Else ToPrinter = True
    
    'get recovery values
    If Dir(DocFolder & "nsflog.txt") <> "" Then
        Open DocFolder & "nsflog.txt" For Input As #2
        Input #2, pSta, pSSN, pEffDate, pamt
        Close #2
    Else
        pSSN = ""
        pSta = 0
    End If
    
    'open letter data files
    If pSSN <> "" Then
        Open DocFolder & "nsfdpadat.txt" For Append As #4
        Open DocFolder & "nsfdat.txt" For Append As #5
    Else
        Open DocFolder & "nsfdpadat.txt" For Output As #4
        Open DocFolder & "nsfdat.txt" For Output As #5
        'create new batch detail list file with header row
        Open DocFolder & "batchdetaillist.csv" For Output As #7
        Write #7, "ACCOUNT #", "NAME", "PAYMENT TYPE", "PAYMENT AMOUNT", "EFFECTIVE DATE", "POSTED DATE", "REASON FOR RETURN"
        Close #7
    End If

    'open posting file and create blank batch detail file
    Open DocFolder & "NSF ENTRY.txt" For Input As #1
    
    'process payments
    While Not EOF(1) And pSta < 6
        'read record from file
        Input #1, ssn, lname, EffDate, PostDate, PmtAmt, BatchCd, PmtCd, NSFReason, DPA
        
        'find last payment processed
        If pSSN <> "" Then
            While pSSN <> ssn Or pEffDate <> EffDate Or pamt <> PmtAmt
                Input #1, ssn, lname, EffDate, PostDate, PmtAmt, BatchCd, PmtCd, NSFReason, DPA
            Wend
        End If
        
        'reset variables to prevent carryover from previous payment and get demographic information
        AN = ""
        LastName = ""
        FirstName = ""
        Address1 = ""
        Address2 = ""
        City = ""
        State = ""
        Zip = ""
        Country = ""
        Sp.Common.GetLP22 ssn, AN, LastName, , FirstName, Address1, Address2, City, State, Zip, Country
        
        'reverse payment
        If pSta < 1 Then
            'access LC65
            FastPath "LC65C" & ssn & "BC" & Format(PostDate, "MMDDYYYY")
            If check4text(1, 43, "DEFAULT RETROACTIVE ADJUSTMENT DISPLAY") Then
                'enter payment amount
                puttext 4, 69, Format(CDbl(PmtAmt), "#####0.00"), "END"
                'review the payment
                row = 9
                fndRow = 0
                Do While Not check4text(21, 3, "46004")
                    'review the payment if the amount matches
                    If CDbl(GetText(9, 61, 11)) = CDbl(PmtAmt) Then
                        'prompt the user to select the payment if multiple matches are found
                        If fndRow <> 0 Then
                            fndRow = 99
                            MsgBox "Enter Y in the selection field for the payment to be reversed and hit <Insert> to continue.", vbInformation, "Select Payment"
                            Sp.PauseForInsert
                            hit "F6"
                            If check4text(21, 3, "44183") Then hit "F6"
                            If check4text(21, 3, "47027") Then
                                Open DocFolder & "NSF ENTRY ERR.txt" For Append As #3
                                Write #3, ssn, lname, EffDate, PostDate, PmtAmt, BatchCd, PmtCd, NSFReason, DPA, GetText(21, 3, 40)
                                Close #3
                            End If
                            Exit Do
                        'record row if match is found
                        Else
                            fndRow = row
                        End If
                    End If
                    row = row + 1

                    'go to next page
                    If Not check4text(row, 5, "_") Then
                        hit "F8"
                        row = 9
                    End If
                Loop

                'if one match was found, post
                If fndRow <> 0 And fndRow <> 99 Then
                    puttext fndRow, 5, "Y", "F6"
                    If check4text(21, 3, "44183") Then hit "F6"
                    If check4text(21, 3, "47027") Then
                        Open DocFolder & "NSF ENTRY ERR.txt" For Append As #3
                        Write #3, ssn, lname, EffDate, PostDate, PmtAmt, BatchCd, PmtCd, NSFReason, DPA, GetText(21, 3, 40)
                        Close #3
                    End If
                End If
            End If

            'verify the error comment
            If Not check4text(22, 3, "49000") And Not check4text(21, 3, "49000") Then
                MsgBox "Error message not completed.", vbCritical, "Not Verified"
                pSta = 5
                'update log
                Open DocFolder & "nsflog.txt" For Output As #2
                Write #2, 5, ssn, EffDate, PmtAmt
                Close #2
            Else
                'update log
                Open DocFolder & "nsflog.txt" For Output As #2
                Write #2, 1, ssn, EffDate, PmtAmt
                Close #2
            End If
        End If

        'add queue task to remove DPA
        If pSta < 2 And DPA = "Y" Then
            Sp.Common.AddLP9O ssn, "DREMDPA", , "more than 2 NSF within the past 6 months.  Remove DPA set up on FirstPointe"

            'update log
            Open DocFolder & "nsflog.txt" For Output As #2
            Write #2, 2, ssn, EffDate, PmtAmt
            Close #2
        End If
        
        'add print record
        If pSta < 3 Then
            'get demographic info
            If State = "FC" Then State = ""
            'write info to file
            If DPA = "Y" Then
                Write #4, ssn, Sp.ACSKeyLine(ssn), FirstName, LastName, Address1, Address2, City, State, Zip, Country, AN, EffDate, Format(CDbl(PmtAmt), "###,##0.00"), State, "MA2329"
            Else
                Write #5, ssn, Sp.ACSKeyLine(ssn), FirstName, LastName, Address1, Address2, City, State, Zip, Country, AN, EffDate, Format(CDbl(PmtAmt), "###,##0.00"), GetNSFReason(NSFReason), State, "MA2329"
            End If
            
            'update log
            Open DocFolder & "nsflog.txt" For Output As #2
            Write #2, 3, ssn, EffDate, PmtAmt
            Close #2
        End If

        'add activity record
        If pSta < 4 Then
            If BatchCd = "C" Then ActCd = "PMNSF" Else ActCd = "UNSFW"
            Comment = "Payment effective " & Format(EffDate, "MMDDYYYY") & " for " & FormatCurrency(CDbl(PmtAmt), 2, vbTrue, vbFalse, vbTrue) & " was reversed due to " & NSFReason & ".  NSF letter sent to borrower."
            If DPA = "Y" Then Comment = Comment & "  Requested DREMDPA queue task for loan mgt to review DPA setup."
            Sp.Common.AddLP50 ssn, ActCd, "NSFREVPOST", "MS", "16", Comment

            'update log
            Open DocFolder & "nsflog.txt" For Output As #2
            Write #2, 4, ssn, EffDate, PmtAmt
            Close #2
        End If
        
        'update report to be e-mailed
        If pSta < 5 Then
            Open DocFolder & "batchdetaillist.csv" For Append As #7
            Write #7, AN, FirstName & " " & LastName, PmtCd, PmtAmt, EffDate, PostDate, NSFReason
            Close #7
        
            'update log
            Open DocFolder & "nsflog.txt" For Output As #2
            Write #2, 5, ssn, EffDate, PmtAmt
            Close #2
        End If
        
        'reset recovery variables
        pSta = 0
        pSSN = ""
    Wend
    Close
    
    'print and image NSFDPA letters
    If pSta < 6 Then
        If PrintIt(DocFolder & "nsfdpadat.txt") Then
            Sp.Common.SortFile DocFolder & "nsfdpadat.txt", 13, "SSN,KeyLine,FirstName,LastName,Address1,Address2,City,State,ZIP,Country,Account_Number,ChkDate,Amt,State_Ind,COST_CENTER_CODE"
            Sp.DocCreateAndDeploy.AddBarcodeAndStaticCurrentDate DocFolder & "nsfdpadat.txt", "Account_Number", "NSFDPA"
            Sp.CostCenterPrinting.Main docPath, "NSFDPA", "NSF Letters", Page1, "NSF", DocFolder & "nsfdpadat.txt", Now, "NSFREVPOST", False, ToPrinter
            Sp.Common.ImageDocs 12, "LMNSF", docPath, "NSFDPA", DocFolder & "nsfdpadat.txt"
        End If
        
        'update log
        Open DocFolder & "nsflog.txt" For Output As #2
        Write #2, 6, ssn, EffDate, PmtAmt
        Close #2
    End If
    
    'print and image NSF letters
    If pSta < 7 Then
        If PrintIt(DocFolder & "nsfdat.txt") Then
            Sp.Common.SortFile DocFolder & "nsfdat.txt", 14, "SSN,KeyLine,FirstName,LastName,Address1,Address2,City,State,ZIP,Country,Account_Number,ChkDate,Amt,Reason,State_Ind,COST_CENTER_CODE"
            Sp.DocCreateAndDeploy.AddBarcodeAndStaticCurrentDate DocFolder & "nsfdat.txt", "Account_Number", "NSF"
            Sp.CostCenterPrinting.Main docPath, "NSF", "NSF Letters", Page1, "NSF", DocFolder & "nsfdat.txt", Now, "NSFREVPOST", False, ToPrinter
            Sp.Common.ImageDocs 12, "LMNSF", docPath, "NSF", DocFolder & "nsfdat.txt"
        End If
        
        'update log
        Open DocFolder & "nsflog.txt" For Output As #2
        Write #2, 7, ssn, EffDate, PmtAmt
        Close #2
    End If
    
    'send e-mail notification
    If pSta < 8 Then Sp.Common.SendMail Sp.Common.BSYSRecips("NSFREVPOST"), , "OneLINK NSF Reversals", , , , DocFolder & "batchdetaillist.csv"
    
    'update log
    Open DocFolder & "nsflog.txt" For Output As #2
    Write #2, 8, ssn, EffDate, PmtAmt
    Close #2
    
    'print the error report if there is one
    If Dir(DocFolder & "NSF ENTRY ERR.txt") <> "" Then
        PrintErrorReport
        MsgBox "Processing is complete.  However, errors were encountered.  Please print and review the NSF Reversal Posting Script Error Report.", , "NSF Reveral Posting"
    End If
    
End Sub
'</2>

'<2->
'determine if the text file has more than one row in it and needs to be printed
Function PrintIt(FileName As String) As Boolean
    Dim tvar As String
    PrintIt = False
    
    On Error GoTo fndErr
    
    Open FileName For Input As #6
    Line Input #6, tvar
    
    PrintIt = True
    
fndErr:
    Close #6
End Function
'</2>

'<2->
'get NSF reason
Function GetNSFReason(NSFRea As String) As String
    Select Case NSFRea
    Case "Insufficient funds - 01"
        GetNSFReason = " because of insufficient funds"
    Case "Account Closed - 02"
        GetNSFReason = " because the account is closed"
    Case "Unable to locate account - 03"
        GetNSFReason = " because we were unable to locate account"
    Case "Invalid account number - 04"
        GetNSFReason = " because of an invalid account number"
    Case "Per bank request - 06"
        GetNSFReason = " at the bank's request"
    Case "Prior authorization revoked - 07"
        GetNSFReason = " because prior authorization was revoked"
    Case "Stop payment - 08"
        GetNSFReason = " because a stop payment was placed on this payment"
    Case "Uncollected/unavailable funds - 09"
        GetNSFReason = " because of uncollected/unavailable funds"
    Case "Transaction unauthorized - 10"
        GetNSFReason = " because the transaction was unauthorized"
    Case "Non-participating bank - 13"
        GetNSFReason = " because your financial institution is a non-participating bank"
    Case "Payee deceased - 14"
        GetNSFReason = ".  Please contact your financial institution"
    Case "Account holder deceased - 15"
        GetNSFReason = " because the account holder is deceased"
    Case "Account frozen - 16"
        GetNSFReason = " because the account was frozen"
    Case "File edit error - 17"
        GetNSFReason = " because of file edit error.  Please contact your financial institution"
    Case "Non-transaction account - 20"
        GetNSFReason = " because it is a non-transaction account"
    Case "Invalid ABA/routing number - 28"
        GetNSFReason = " because of an invalid ABA/routing number"
    Case "Corporate customer not authorized - 29"
        GetNSFReason = " because the corporate customer is not authorized. Please contact your financial institution."
    Case "Postdated - 95"
        GetNSFReason = " because the payment was postdated. Please contact your financial institution"
    Case "Staledated - 96"
        GetNSFReason = " because the payment was staledated.  Please contact your financial institution"
    Case "Improper endorsement - 97"
        GetNSFReason = " because of an improper endorsement"
    Case "Refer to maker - 98"
        GetNSFReason = ".  Please contact your financial institution"
    Case "Other - 99"
        GetNSFReason = "; please contact UHEAA at the number below"
    Case "Contact your financial institution - 30"
        GetNSFReason = "; please contact your financial institution"
    End Select
End Function
'</2>

'clean up text files
Function CreateBak()
    'rename the posting and batch detail files to create backups
    If Dir(DocFolder & "NSF ENTRY.bak") <> "" Then Kill DocFolder & "NSF ENTRY.bak"
    If Dir(DocFolder & "NSF ENTRY.txt") <> "" Then Name DocFolder & "NSF ENTRY.txt" As DocFolder & "NSF ENTRY.bak"
    If Dir(DocFolder & "batchdetaillist.bak") <> "" Then Kill DocFolder & "batchdetaillist.bak"
    If Dir(DocFolder & "batchdetaillist.csv") <> "" Then Name DocFolder & "batchdetaillist.csv" As DocFolder & "batchdetaillist.bak"
    If Dir(DocFolder & "NON POSTING NSF ENTRY.txt") <> "" Then
        If Dir(DocFolder & "NON POSTING NSF ENTRY.bak") <> "" Then Kill DocFolder & "NON POSTING NSF ENTRY.bak"
        Name DocFolder & "NON POSTING NSF ENTRY.txt" As DocFolder & "NON POSTING NSF ENTRY.bak"
    End If
    
    'delete the log
    If Dir(DocFolder & "nsflog.txt") <> "" Then Kill DocFolder & "nsflog.txt"
End Function


'new sr1204, jd, 08/04/05
'<1> sr1417, jd
'<2> sr1872, jd

'promote frmNSFRevPostSys
'promote letters
