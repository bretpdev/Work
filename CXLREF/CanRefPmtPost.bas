Attribute VB_Name = "CanRefPmtPost"
'Dim AccountNumber As String
'Dim Amount As Double
'Dim aLnSeq(1) As Integer
'Dim APL As String
'Dim Bal As Double
'Dim Lender As String
'Dim LoanStatus As String
'Dim Name As String
'Dim CheckNo As String
'Dim CheckReason As String
'Dim CurrSchool As String
'Dim aDisbDate(1 To 99) As String
'Dim aDisbAmt(1 To 99) As String
'Dim counter As Integer
'Dim OrgLenderID As String
'Dim LID As String
'Dim PmtAmount As Double
'Dim PmtCode As String
'Dim Indy As String
'Dim SUID As String
'Dim oop As String
'Dim FrstDisbDt As String
'Dim GotTS26 As Boolean
'Dim NewDisbDate As String
'Dim NewDisbAmt As Double
'Dim IsOnSystem As Boolean
'Dim Voided As Boolean
'Dim Stopped As Boolean
'Dim ReasonF As String
'Dim DisbNo As Integer
'Dim AppID As String
'Dim AppSeq As String

Dim LenderID As String
Dim School As String
Dim lname As String
Dim LnTyp As String
Dim DisbDate As String
Dim DisbAmt As Double
Dim ChkAmt As Double
Dim PmtAmt As Double
Dim PmtCd As String
Dim PCV As Double
Dim lSSN As String
Dim lTrNo As String
Dim lDate As String
Dim EffDate As String
Dim CLID As String
Dim Reissue As String
Dim SeqNo As Integer
Dim OwnerID As String
Dim PrevRefAmt As Double
Dim TotPmts As String
Dim batchNo As String
Dim Targ As String
Dim SeqNos(0) As Integer
Dim userId As String


'process cancellation and refund payments
Sub BatchPost()
    Dim Batch As Integer                                        '<17>
    Dim row As Integer
    
'<30>
    'In some cases, local variables with the same names as some public variables
    ' are not properly going out of scope. The following function call will clear
    ' the contents of those variables so that the previous record's information
    ' does not persist into this iteration.
    ResetPublicVars
'</30>
    userId = Sp.Common.GetUserID()
    With Session
    
    'check the log file to see if the process completed normally the last time
        'create an empty log file if none exists
'<20>
        If Sp.Common.TestMode Then
            If Dir("T:\pmtlistcrlog.txt") = "" Then
                Open "T:\pmtlistcrlog.txt" For Output As #7
                Close #7
            'read the log if it is not empty
            ElseIf FileLen("T:\pmtlistcrlog.txt") <> 0 Then
                Open "T:\pmtlistcrlog.txt" For Input As #7
                Input #7, lSSN, lTrNo, lDate, batchNo
                Close #7
                'print any target errors and verify the batch if the credits have all been entered
                If (lSSN = "done" Or lTrNo = "LG0H Ref" Or lTrNo = "LG0H Canc" Or lTrNo = "done") And _
                   lDate = Format(Date, "MM/DD/YYYY") Then
                    PrintReports
                ElseIf lSSN = "done" Then
                    lSSN = ""
                    batchNo = ""
                End If
            End If
        
        Else
            If Dir("X:\PADD\Logs\pmtlistcrlog.txt") = "" Then
                Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
                Close #7
            'read the log if it is not empty
            ElseIf FileLen("X:\PADD\Logs\pmtlistcrlog.txt") <> 0 Then
                Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
                Input #7, lSSN, lTrNo, lDate, batchNo
                Close #7
                'print any target errors and verify the batch if the credits have all been entered
                If (lSSN = "done" Or lTrNo = "LG0H Ref" Or lTrNo = "LG0H Canc" Or lTrNo = "done") And _
                   lDate = Format(Date, "MM/DD/YYYY") Then
                    PrintReports
                ElseIf lSSN = "done" Then
                    lSSN = ""
                    batchNo = ""
                End If
            End If
        End If
'</20>
            
        'ssn = "" means no processing at all completed
        'ssn <> "" but batchno = blank means preprocessing complete
        
        'set up Excel object for error reports
        Dim ExcelApp As excel.Application
        Set ExcelApp = CreateObject("Excel.Application")
        
    'if the last SSN is blank, no processing was completed so posting setup is needed
        If lSSN = "" Then
            'prompt the user for the total amount of payments being posted
            TotPmts = InputBox("Enter the total amount of the payments being posted.", "Total Payments")
            If TotPmts = "" Then End

            If vbYes <> MsgBox("Was the payment made with cash or wire?  Click 'Yes' for cash and 'No' for wire.", vbYesNo, "Cash or Wire") Then
                Batch = 2
            Else
                Batch = 1
            End If
            
            'blank error files to make sure files from previous runs are not printed
            Open "T:\targerr.txt" For Output As #2
            Close #2
            Open "T:\pmtlistcreftmp.txt" For Output As #7
            Close #7
        End If
        
    'if the batch no is blank, processing did not get far enough to set up the batch so
    'preprocessing is needed or may need to be completed
        If batchNo = "" Then
            'open file for input
            Open "T:\pmtlistcr.txt" For Input As #1
            'if the last SSN is blank, no preprocessing has been completed so new files are needed
            If lSSN = "" Then
                Open "T:\pmtlistcref.txt" For Output As #3
                Open "T:\pmtlistccan.txt" For Output As #5
            'if the last SSN is not blank, some preprocessing has been completed so remaining records need to be added to the existing files
            Else
                Open "T:\pmtlistcref.txt" For Append As #3
                Open "T:\pmtlistccan.txt" For Append As #5
            End If
            Rec = 0
            'input the records that have already been processed, skip if lssn = "" (no processing complete)
            Do Until lSSN = "" Or lSSN = ssn Or EOF(1)
                Input #1, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                          DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV, OwnerID
                Rec = Rec + 1
            Loop
            'process records from file 1
            Do Until EOF(1)
                'read in the data
                Input #1, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                          DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV, OwnerID
                'increment the counter
                Rec = Rec + 1
                'add the pmt to the refund file
                If PmtCd = "40" And Targ = "Y" Then Write #3, ssn, SeqNo, LnTyp, EffDate, _
                                                              PmtAmt, Reissue, LenderID, _
                                                              School, DisbDate, DisbAmt, _
                                                              ChkAmt, PmtCd, lname, Targ, _
                                                              CLID, PCV
                'add the pmt to the cancellation file
                If PmtCd = "45" Then Write #5, ssn, CLID, SeqNo, EffDate, DisbDate, DisbAmt, LnTyp
                'add a comment for PLUS loans
                If LnTyp = "PLUS" Or LnTyp = "PLUSGB" Then ''Changed from "PLUS  " to "PLUS" don't know why it has extra spaces. SR 3114
                    SeqNos(0) = SeqNo
                    If Sp.Common.ATD22ByLoan(ssn, "RPLCA", EffDate & " funds returned from " & School, SeqNos, "CXL/REF", userId) = False Then
                        Sp.Common.ATD37FirstLoan ssn, "RPLCA", EffDate & " funds returned from " & School, userId, "CXL/REF"
                    End If
                    'ATD22 "RPLCA", SeqNo, EffDate & " funds returned from " & School
                End If
99              Loop
            'close the files
            Close #1
            Close #3
            Close #5
        End If
        
    'batch processing
        'if the batchno is blank, 'create a new batch
        If batchNo = "" Then
            'access ATS1G
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "TX3Z/ATS1G;"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If .GetDisplayText(23, 2, 5) = "01473" Then
                .TransmitTerminalKey rcIBMEraseEOFKey
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            End If
            'enter batch info
'<17>       .TransmitANSI "1"
            .TransmitANSI Batch                                     '<17>
            .TransmitANSI Rec
            .MoveCursor 11, 28
            .TransmitANSI TotPmts
            .MoveCursor 15, 28
            userId = .GetDisplayText(17, 28, 8)
            .TransmitANSI userId
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            batchNo = .GetDisplayText(6, 18, 14)
        End If
    'access the batch
        'access ATS1J
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "TX3Z/ATS1D" & batchNo
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
       'if the batch is not displayed, it has been verified so just print the reports
        If .GetDisplayText(1, 72, 5) <> "TSX1F" Then PrintReports
        row = 8
    'process payment records
        Open "T:\pmtlistcr.txt" For Input As #1
        'if the last SSN is not blank, the process did not complete previously, find and finish processing the last SSN
        If lSSN <> "" Then
            'find the last page
            Do While .GetDisplayText(5, 78, 1) = "+"
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                If .GetDisplayText(8, 5, 3) = "___" Then
                    .TransmitTerminalKey rcIBMPf7Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    Exit Do
                End If
            Loop
            'find the last transaction
            row = 8
            Do Until .GetDisplayText(row, 5, 3) = "___" Or .GetDisplayText(row, 5, 3) = "   "
                row = row + 1
            Loop
            row = row - 1
            
            'find the record for the last transaction
            Do Until ssn = GetText(row, 5, 9) And PmtAmt = CDbl(GetText(row, 17, 10)) And PmtCd = GetText(row, 43, 2)
                Input #1, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                          DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV, OwnerID
            Loop
            
            'target the last transaction if it did not be targeted
            If .GetDisplayText(row, 69, 1) <> "X" And Targ = "Y" Then
                GoTo targeting
            'increment row if last payment was already targeted
            Else
                row = row + 1
            End If
        End If
        'process each payment record
        Do While Not EOF(1)
            Input #1, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                      DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV, OwnerID
            'enter payment info
            .TransmitANSI ssn
            .MoveCursor row, 17                                         '<14>
            .TransmitANSI PmtAmt
            .TransmitTerminalKey rcIBMTabKey
            If PmtCd = "45" Then
                .TransmitANSI Format(DisbDate, "MMDDYY")
            Else
                .TransmitANSI Format(EffDate, "MMDDYY")
            End If
            .TransmitANSI PmtCd
            If PmtCd = "45" Then
                .TransmitANSI LenderID
            Else
                .TransmitANSI School
            End If
            .MoveCursor row, 59
            .TransmitANSI "9"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            
            'clear duplicate transactions error
            If .GetDisplayText(23, 2, 5) = "30008" Then ClearDuplicate GetText(row, 2, 2), lname
            
            If (check4text(23, 2, "04404 ALL LOANS DECONVERTED - COMMENT NECESSARY TO CONTINUE")) Then
                LoansDeconvertedProcessing GetText(row, 2, 2), lname
            ElseIf (check4text(23, 2, "05097")) Then 'account number not found on system
                AccountNotOnSystemProcessing GetText(row, 2, 2)
            Else

                'update the log
    '<20>
                If Sp.Common.TestMode Then
                    Open "T:\pmtlistcrlog.txt" For Output As #7
                Else
                    Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
                End If
    '</20>
                Write #7, ssn, val(.GetDisplayText(row, 72, 4)), SeqNo, batchNo
                Close #7
                
                
targeting:
                'target transactions
                If Targ = "Y" Then
                    'if targeting screen is not displayed
                    If Not TargetIt(GetText(row, 2, 2)) Then
                        If LenderID <> OwnerID Then 'try owner as institution if lender <> owner
                            'return to posting screen and switch to change mode
                            If Not check4text(1, 72, "TSX1F") Then hit "F12"
                            puttext 1, 4, "C", "ENTER"
                            'be sure that you are on the last page
                            While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                                hit "F8"
                            Wend
                            'enter owner in INST ID field
                            puttext row, 48, OwnerID
                            If Len(OwnerID) < 8 Then hit "END"
                            hit "ENTER"
                            'clear duplicate transactions error
                            If .GetDisplayText(23, 2, 5) = "30008" Then ClearDuplicate GetText(row, 2, 2), lname
                    
                            'try to target again, if targeting screen is not displayed
                            If Not TargetIt(GetText(row, 2, 2)) Then
                                'try UHEAA affiliated lenders as institution if owner is UHEAA affiliated lender
                                If Sp.Common.IsAffiliatedLenderCode(OwnerID) Then
                                    Dim affiliatedLenders() As String
                                    Dim i As Integer
                                    
                                    'get list of UHEAA affiliated lenders
                                    affiliatedLenders = Sp.Common.SqlEx("SELECT LenderID FROM GENR_REF_LenderAffiliation WHERE Affiliation = 'UHEAA'")
                                    
                                    'attempt to target transaction with each UHEAA affiliated lender
                                    i = 0
                                    Do
                                        'return to posting screen, switch to change modem, and enter new lender ID
                                        If Not check4text(1, 72, "TSX1F") Then hit "F12"
                                        puttext 1, 4, "C", "ENTER"
                                        
                                        While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                                            hit "F8"
                                        Wend
                            
                                        puttext row, 48, affiliatedLenders(0, i)
                                        hit "END"
                                        hit "ENTER"
                                        
                                        'clear duplicate transactions error
                                        If .GetDisplayText(23, 2, 5) = "30008" Then ClearDuplicate GetText(row, 2, 2), lname
                                        
                                        'exit loop if targeting succeeds
                                        If TargetIt(GetText(row, 2, 2)) Then
                                            Exit Do
                                        'write to error file if targeting fails with each lender code
                                        ElseIf i = UBound(affiliatedLenders, 2) - 1 Then
                                            Open "T:\targerr.txt" For Append As #2
                                            Write #2, ssn, SeqNo, PmtAmt, DisbDate, DisbAmt, ChkAmt
                                            Close #2
                                            Exit Do
                                        End If
                                        
                                        i = i + 1
                                    Loop
                                'write to error file if owner is not UHEAA affiliated lender
                                Else
                                    Open "T:\targerr.txt" For Append As #2
                                    Write #2, ssn, SeqNo, PmtAmt, DisbDate, DisbAmt, ChkAmt
                                    Close #2
                                End If
                            End If
                        'write to error file if if lender = owner
                        ElseIf LenderID = OwnerID Then
                            Open "T:\targerr.txt" For Append As #2
                            Write #2, ssn, SeqNo, PmtAmt, DisbDate, DisbAmt, ChkAmt
                            Close #2
                        End If
                    End If
                    
                    'return to posting screen and change hot keys
                    If Not check4text(1, 72, "TSX1F") Then hit "F12"
                    hit "F2"
                    
                    'change back to add mode and find next blank line if in change mode
                    If check4text(1, 4, "C") Then
                        puttext 1, 4, "A", "ENTER"
                        While Not check4text(23, 2, "90007")
                            hit "F8"
                        Wend
                    End If
                End If
                
            End If
            
            row = row + 1
            'print the screen and go to the next screen if the screen is full
            If row = 20 Then
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                row = 8
            End If
        Loop
        Close #1
        
        'update the log
'<20>
        If Sp.Common.TestMode Then
            Open "T:\pmtlistcrlog.txt" For Output As #7
        Else
            Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
        End If
        'Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
'</20>
        Write #7, "done", "LG0H Ref", Format(Date, "MM/DD/YYYY"), batchNo
        Close #7
        PrintReports
'</12>
    End With
End Sub
        
'<12->
'verify the batch and print the reports
Sub PrintReports()
    With Session
        Dim ExcelApp As excel.Application
        Set ExcelApp = CreateObject("Excel.Application")
        'update recovery variables
'<20>
        If Sp.Common.TestMode Then
            Open "T:\pmtlistcrlog.txt" For Input As #7
        Else
            Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
        End If
        'Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
'</20>
        Input #7, lSSN, lTrNo, lDate, batchNo
        Close #7
        'create an empty target error file if none exists so FileLen functions on the file don't debug
        If Dir("T:\targerr.txt") = "" Then
            Open "T:\targerr.txt" For Output As #2
            Close #2
        End If
        'go to CTS1R
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "TX3Z/CTS1R" & batchNo
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'print the target error spreadsheet if there were errors
        If FileLen("T:\targerr.txt") <> 0 Then
            ExcelApp.Visible = True
            'open pull list form
            ExcelApp.Workbooks.Open ("X:\PADD\Compass\Payments\TargetErrors.xls")
            'read records from data file
            Open "T:\targerr.txt" For Input As #2
            'add data to form
            row = 7
            Do While Not EOF(2)
                Input #2, ssn, SeqNo, PmtAmt, DisbDate, DisbAmt, ChkAmt
                'SSN
                ExcelApp.Range("A" & row).Select
                ExcelApp.ActiveCell.FormulaR1C1 = Format(ssn, "@@@-@@-@@@@")
                ExcelApp.Range("B" & row).Select
                ExcelApp.ActiveCell.FormulaR1C1 = SeqNo
                ExcelApp.Range("C" & row).Select
                ExcelApp.ActiveCell.FormulaR1C1 = Format(PmtAmt, "###,##0.00")
                ExcelApp.Range("D" & row).Select
                ExcelApp.ActiveCell.FormulaR1C1 = DisbDate
                ExcelApp.Range("E" & row).Select
                ExcelApp.ActiveCell.FormulaR1C1 = Format(DisbAmt, "###,##0.00")
                ExcelApp.Range("F" & row).Select
                ExcelApp.ActiveCell.FormulaR1C1 = Format(ChkAmt, "###,##0.00")
                row = row + 1
            Loop
            Close #2
        End If
        'replace amounts in file with those from the updated batch detail if the batch is not available because it has been released
        If .GetDisplayText(1, 71, 5) = "TSX1T" Then
            Rplace
        'warn the user processing is complete and tell the user if the batch balanced
        ElseIf .GetDisplayText(11, 30, 10) = .GetDisplayText(10, 69, 10) Then
            .TransmitTerminalKey rcIBMPf10Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitTerminalKey rcIBMPrintKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If FileLen("T:\targerr.txt") <> 0 Then
                warn = MsgBox("The batch is in balance.  However, the script was not able to target one or more transactions.  Print the target error spreadsheet and review those transactions.", vbOKOnly, "Batch Complete - Target Errors")
            Else
                warn = MsgBox("The batch is in balance.  There were no target errors.", vbOKOnly, "Batch Complete")
            End If
        'warn the user if the batch does not balance and pause for the user to make corrections
        Else
100         If FileLen("T:\targerr.txt") <> 0 Then
                warn = MsgBox("The batch does not balance.  Print the target error spreadsheet and make the necessary corrections to allow the batch to balance.  Then press Insert.", vbOKOnly, "Unable to Target Errors")
            Else
                warn = MsgBox("The batch does not balance.  Make the necessary corrections to allow the batch to balance.  Then press Insert.", vbOKOnly, "Batch out of Balance")
            End If
            'access TS1D
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "TX3Z/CTS1D" & batchNo
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'wait for the user to hit insert
            .WaitForTerminalKey rcIBMInsertKey, "1:0:0"
            .TransmitTerminalKey rcIBMInsertKey
            'access TS1R
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "TX3Z/CTS1R" & batchNo
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'verify the batch is balanced
            If .GetDisplayText(11, 30, 10) = .GetDisplayText(10, 69, 10) Then
                .TransmitTerminalKey rcIBMPf10Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitTerminalKey rcIBMPrintKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                warn = MsgBox("The batch is in balance.", vbOKOnly, "Batch Complete")
            'return to prompt the user if the batch still doesn't balance
            Else
                GoTo 100
            End If
            Rplace
        End If
        'update OneLINK with refund information
        LG0H
'<16->
'        'print refund posting error report
'        If FileLen("T:\pmtlistcrreferr.txt") > 0 Then
'            'set up Excel session
'            excelApp.Visible = True
'            excelApp.Workbooks.Open ("X:\PADD\Compass\Payments\OLRefundErrors.xls")
'            'read records from data file
'            Open "T:\pmtlistcrreferr.txt" For Input As #5
'            'add data to form
'            row = 7
'            Do While Not EOF(5)
'                Input #5, SSN, CLID, DisbDate, ttrefamt, ErrMsg
'                excelApp.Range("A" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = Format(SSN, "@@@-@@-@@@@")
'                excelApp.Range("B" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = CLID
'                excelApp.Range("C" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = DisbDate
'                excelApp.Range("D" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = ttrefamt
'                excelApp.Range("E" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = ErrMsg
'                row = row + 1
'            Loop
'            Close #5
'        End If
'        'print cancelation posting error report
'        If FileLen("T:\pmtlistcrcanerr.txt") > 0 Then
'            'open pull list form
'            excelApp.Visible = True
'            excelApp.Workbooks.Open ("X:\PADD\Compass\Payments\OLCancelErrors.xls")
'            'read records from data file
'            Open "T:\pmtlistcrcanerr.txt" For Input As #5
'            'add data to form
'            row = 7
'            Do While Not EOF(5)
'                Input #5, SSN, CLID, DisbDate, ttrefamt, ErrMsg
'                excelApp.Range("A" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = Format(SSN, "@@@-@@-@@@@")
'                excelApp.Range("B" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = CLID
'                excelApp.Range("C" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = DisbDate
'                excelApp.Range("D" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = ttrefamt
'                excelApp.Range("E" & row).Select
'                excelApp.ActiveCell.FormulaR1C1 = ErrMsg
'                row = row + 1
'            Loop
'            Close #5
'        End If
'</16>
        'delete the posting data file so the payments don't get posted again by accident
        Set fso = CreateObject("Scripting.FileSystemObject")
        If Dir("T:\pmtlistcr.txt") <> "" Then
            fso.CopyFile "T:\pmtlistcr.txt", "T:\pmtlistcr.bak", True
            Kill "T:\pmtlistcr.txt"
        End If
        MsgBox "Processing Complete."
        End
    End With
End Sub
'</12>

'<22->
''<5->
''update OneLINK
'Sub LG0H()
'    With Session
''<12->
''        'exit the subroutine if there are no error transaction
'''<10>   If FileLen("T:\pmtlistcref.txt") = 0 Then Exit Sub
''        If FileLen("T:\pmtlistcref.txt") = 0 Then GoTo Canc:   '<10>
''        Open "T:\pmtlistcref.txt" For Input As #3
'''<10>   Open "T:\pmtlistcrpsterr.txt" For Output As #5
''        Open "T:\pmtlistcrreferr.txt" For Output As #5         '<10>
'
'        'update recovery variables
''<20>
'        If SP.Common.TestMode Then
'            Open "T:\pmtlistcrlog.txt" For Input As #7
'        Else
'            Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
'        End If
'        'Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
''</20>
'
'
'        Input #7, lSSN, lTrNo, lDate, BatchNo
'        Close #7
'        'do not process the refund file if there are no error transactions
'        If FileLen("T:\pmtlistcref.txt") = 0 Or _
'           lTrNo = "LG0H Canc" Or _
'           lTrNo = "done" Then
'            GoTo Canc:
'        End If
'    'process the refund errors
'        Open "T:\pmtlistcref.txt" For Input As #3
''<16->
''        'open a new error file if not in recovery mode
''        If lSSN = "" Or lSSN = "done" Then
''            Open "T:\pmtlistcrreferr.txt" For Output As #5
''        'open the existing error file and go to the next ssn if in recovery mode
''        Else
''            Open "T:\pmtlistcrreferr.txt" For Append As #5
''            Do Until lSSN = SSN
''                Input #3, SSN, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
''                    DisbDate, DisbAmt, ChkAmt, PmtCd, LName, Targ, CLID, PCV
''            Loop
''        End If
''</16>
''</12>
'        'process each transaction
'        Do While Not EOF(3)
'            'input information
'            Input #3, SSN, seqno, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
'                DisbDate, DisbAmt, ChkAmt, PmtCd, LName, Targ, CLID, PCV '<8> added PCV
'            'access LG0H
'            .TransmitTerminalKey rcIBMClearKey
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
''<7>        .TransmitANSI "LG0HC;" & SSN & ";;" & CLID
'            .TransmitANSI "LG0HI;" & SSN & ";;" & CLID                  '<7>
'            .TransmitTerminalKey rcIBMEnterKey
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'            'locate disbursement
'            Col = 20
'            Do Until .GetDisplayText(12, Col, 8) = Format(DisbDate, "MMDDYYYY")
'                Col = Col + 16
'                If Col > 68 Then
'                    .TransmitTerminalKey rcIBMPf10Key
'                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'                    Col = 20
'                End If
'            Loop
'            'select LG0M for the disbursement
'            .MoveCursor 10, Col - 1
'            .TransmitANSI "X"
'            .TransmitTerminalKey rcIBMPf4Key
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'            'calculate the previous refund amount
'            row = 9
'            PrevRefAmt = 0
'            Do
'                If .GetDisplayText(row, 3, 1) = "A" And .GetDisplayText(row, 10, 1) = "S" Then _
'                    PrevRefAmt = PrevRefAmt + CDbl(.GetDisplayText(row, 39, 10))
'                row = row + 1
'                If .GetDisplayText(row, 3, 1) = " " Then
'                    .TransmitTerminalKey rcIBMPf8Key
'                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'                    row = 9
'                    If .GetDisplayText(22, 3, 5) = "46004" Then Exit Do
'                End If
'            Loop
'            'go back to LG0H
'            .TransmitTerminalKey rcIBMPf12Key
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
''<7->       'change to change mode
'            .MoveCursor 1, 7
'            .TransmitANSI "C"
'            .TransmitTerminalKey rcIBMEnterKey
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1                '</7>
'            'update the information
'            'source code
'            .MoveCursor 2, 14
'            .TransmitANSI "E"
'            'canc amt
'            .MoveCursor 13, Col - 4
'            .TransmitANSI "S"
'            'total refund amount
''<8>        .TransmitANSI Format(PrevRefAmt + Round((PmtAmt * DisbAmt / ChkAmt), 2), "000,000.00")
'            .TransmitANSI Format(PrevRefAmt + Round((PmtAmt * (DisbAmt - PCV) / ChkAmt), 2), "000,000.00")  '<8>
'            .TransmitTerminalKey rcIBMEraseEOFKey
'            'effective date
'            .MoveCursor 14, Col
'            .TransmitANSI Format(EffDate, "MMDDYYYY")
'            .TransmitTerminalKey rcIBMPf6Key
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
''<16->
''            'add the transaction to a posting error file if it is not sucessfully updated
''            If .GetDisplayText(22, 3, 5) <> "49000" Then _
''                Write #5, SSN, CLID, DisbDate, PrevRefAmt + Round((PmtAmt * DisbAmt / ChkAmt), 2), _
''                .GetDisplayText(22, 3, 40)
'            'add a queue task to work the error if the record is not sucessfully updated
'            If .GetDisplayText(22, 3, 5) <> "49000" Then
'                LP9O CLID, SSN, DisbDate, DisbAmt, PmtAmt, Trim(.GetDisplayText(22, 3, 40))
'            End If
''</16>
''<12->
'            'update the log
''<20>
'            If SP.Common.TestMode Then
'                Open "T:\pmtlistcrlog.txt" For Output As #7
'            Else
'                Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
'            End If
'            'Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
''</20>
'            Write #7, SSN, "LG0H Ref", Format(Date, "MM/DD/YYYY"), BatchNo
'            Close #7
''</12>
'        Loop
'        'close data files
'        Close #3
''<16>   Close #5
''<12->
'        'update the log
''<20>
'            If SP.Common.TestMode Then
'                Open "T:\pmtlistcrlog.txt" For Output As #7
'            Else
'                Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
'            End If
'            'Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
''</20>
'
'        Write #7, "done", "LG0H Canc", Format(Date, "MM/DD/YYYY"), BatchNo
'        Close #7
'
''<10->
''Canc:   If FileLen("T:\pmtlistccan.txt") = 0 Then Exit Sub
''        Open "T:\pmtlistccan.txt" For Input As #3
''        Open "T:\pmtlistcrcanerr.txt" For Output As #5
'        'do not process the cancellation file if there are no error transactions
'Canc:   If FileLen("T:\pmtlistccan.txt") = 0 Or _
'           lTrNo = "done" Then
'            Exit Sub
'        End If
'        'update recovery variables
''<20>
'        If SP.Common.TestMode Then
'            Open "T:\pmtlistcrlog.txt" For Input As #7
'        Else
'            Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
'        End If
'        'Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
''</20>
'
'        Input #7, lSSN, lTrNo, lDate, BatchNo
'        Close #7
'    'process the cancellation errors
'        Open "T:\pmtlistccan.txt" For Input As #3
''<16->
''        'open a new error file if not in recovery mode
''        If lSSN = "" Or lSSN = "done" Then
''            Open "T:\pmtlistcrcanerr.txt" For Output As #5
''        'open the existing error file and go to the next ssn if in recovery mode
''        Else
''            Open "T:\pmtlistcrcanerr.txt" For Append As #5
''            Do Until lSSN = SSN Or lSSN = "" Or lSSN = "done"
''                Input #3, SSN, CLID, SeqNo, EffDate, DisbDate, DisbAmt
''            Loop
''        End If
''</16>
''</12>
'        'process each transaction
'        Do While Not EOF(3)
'            'input information
'            Input #3, SSN, CLID, seqno, EffDate, DisbDate, DisbAmt
'            'access LG0H
'            .TransmitTerminalKey rcIBMClearKey
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'            .TransmitANSI "LG0HC;" & SSN & ";;" & CLID
'            .TransmitTerminalKey rcIBMEnterKey
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'            'locate disbursement
'            Col = 20
'            Do Until .GetDisplayText(12, Col, 8) = Format(DisbDate, "MMDDYYYY")
'                Col = Col + 16
'                If Col > 68 Then
'                    .TransmitTerminalKey rcIBMPf10Key
'                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'                    Col = 20
'                End If
'            Loop
'        'update the information
'            'source code
'            .MoveCursor 2, 14
'            .TransmitANSI "E"
'            'canc amt
'            .MoveCursor 13, Col - 4
'            .TransmitANSI "U"
'            'total refund amount
'            .TransmitANSI Format(DisbAmt, "000,000.00")
'            .TransmitTerminalKey rcIBMEraseEOFKey
'            'effective date
'            .MoveCursor 14, Col
''<13->
''           .TransmitANSI Format(EffDate, "MMDDYYYY")
'            .TransmitANSI Format(DisbDate, "MMDDYYYY")
''</13>
'            .TransmitTerminalKey rcIBMPf6Key
'            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
''<16->
''            'add the transaction to a posting error file if it is not sucessfully updated
''            If .GetDisplayText(22, 3, 5) <> "49000" Then _
''                Write #5, SSN, CLID, DisbDate, DisbAmt, .GetDisplayText(22, 3, 40)
'            'add a queue task to work the error if the record is not sucessfully updated
'            If .GetDisplayText(22, 3, 5) <> "49000" Then
'                LP9O CLID, SSN, DisbDate, DisbAmt, PmtAmt, Trim(.GetDisplayText(22, 3, 50))
'            End If
''</16>
''<12->
'            'update the log
''<20>
'            If SP.Common.TestMode Then
'                Open "T:\pmtlistcrlog.txt" For Output As #7
'            Else
'                Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
'            End If
'            'Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
''</20>
'
'            Write #7, SSN, "LG0H Canc", Format(Date, "MM/DD/YYYY"), BatchNo
'            Close #7
''</12>
'        Loop
'        'close data files
'        Close #3
''<16>   Close #5
''</10>
''<12->
'        'update the log
''<20>
'            If SP.Common.TestMode Then
'                Open "T:\pmtlistcrlog.txt" For Output As #7
'            Else
'                Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
'            End If
'            'Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
''</20>
'
'        Write #7, "done", "done", Format(Date, "MM/DD/YYYY"), BatchNo
'        Close #7
''</12>
'    End With
'End Sub
''</5>

'update OneLINK
Sub LG0H()
Dim lCLID As String
Dim lDisbDate As String

    With Session
        'update recovery variables
        If Sp.Common.TestMode Then
            Open "T:\pmtlistcrlog.txt" For Input As #7
        Else
            Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
        End If
        
        Input #7, lSSN, lTrNo, lDate, batchNo
        Close #7
        'do not process the refund file if there are no error transactions
        If FileLen("T:\pmtlistcref.txt") = 0 Or _
           lTrNo = "LG0H Canc" Or _
           lTrNo = "done" Then
            GoTo Canc:
        End If
    'process the refund errors
        Open "T:\pmtlistcref.txt" For Input As #3
        
        'advance to the last record processed
        ssn = ""
        CLID = ""
        DisbDate = ""
        If lSSN <> "done" Then
            lCLID = Mid(lSSN, 10, 19)
            lDisbDate = Mid(lSSN, 29, 8)
            lSSN = Mid(lSSN, 1, 9)
            
            Do Until lSSN = ssn And CLID = lCLID And DisbDate = lDisbDate
                Input #3, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                    DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV
            Loop
        End If

        'process each transaction
        Do While Not EOF(3)
            'input information
            Input #3, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV
            'process FFELP loans
            If LnTyp = "STFFRD" Or LnTyp = "UNSTFD" Or LnTyp = "PLUS" Or LnTyp = "PLUSGB" Then
                'access LG0H
                .TransmitTerminalKey rcIBMClearKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitANSI "LG0HI;" & ssn & ";;" & CLID
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                'check if LG0H update should happen
                If check4text(6, 63, "899577") Then
                    Exit Sub
                End If
                'locate disbursement
                col = 20
                Do Until .GetDisplayText(12, col, 8) = Format(DisbDate, "MMDDYYYY")
                    col = col + 16
                    If col > 68 Then
                        .TransmitTerminalKey rcIBMPf10Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        col = 20
                        If check4text(10, 21, "DISB# 01") Then
                            col = 0
                            Exit Do
                        End If
                    End If
                Loop
                'update the information
                If col <> 0 Then
                    'select LG0M for the disbursement
                    .MoveCursor 10, col - 1
                    .TransmitANSI "X"
                    .TransmitTerminalKey rcIBMPf4Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    'calculate the previous refund amount
                    row = 9
                    PrevRefAmt = 0
                    Do
                        If .GetDisplayText(row, 3, 1) = "A" And .GetDisplayText(row, 10, 1) = "S" Then _
                            PrevRefAmt = PrevRefAmt + CDbl(.GetDisplayText(row, 39, 10))
                        row = row + 1
                        If .GetDisplayText(row, 3, 1) = " " Then
                            .TransmitTerminalKey rcIBMPf8Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            row = 9
                            If .GetDisplayText(22, 3, 5) = "46004" Then Exit Do
                        End If
                    Loop
                    'go back to LG0H
                    .TransmitTerminalKey rcIBMPf12Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    'change to change mode
                    .MoveCursor 1, 7
                    .TransmitANSI "C"
                    .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    'source code
                    .MoveCursor 2, 14
                    .TransmitANSI "E"
                    'canc amt
                    .MoveCursor 13, col - 4
                    .TransmitANSI "S"
                    'total refund amount
                    .TransmitANSI Format(PrevRefAmt + Round((PmtAmt * (DisbAmt - PCV) / ChkAmt), 2), "000,000.00")  '<8>
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    'effective date
                    .MoveCursor 14, col
                    .TransmitANSI Format(EffDate, "MMDDYYYY")
                    .TransmitTerminalKey rcIBMPf6Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    
                    'add a queue task to work the error if the record is not sucessfully updated
                    If .GetDisplayText(22, 3, 5) <> "49000" Then
                        LP9O CLID, ssn, DisbDate, DisbAmt, PmtAmt, Trim(.GetDisplayText(22, 3, 40)), "CANCRFND", "" '<26> added CANCRFND
                    End If
                'add a queue task if the disbursement isn't found
                Else
                    Sp.Common.AddLP9O ssn, "CANCRFND", , "CLID = " & CLID & ", Disb Date = " & DisbDate & ", Disb Amt = $" & DisbAmt & ", Cancel Amt = $" & PmtAmt & ", Effective Date = " & DisbDate & ", Error = no disb with this date exists"
                End If
            End If
            
            'update the log
            If Sp.Common.TestMode Then
                Open "T:\pmtlistcrlog.txt" For Output As #7
            Else
                Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
            End If
            Write #7, ssn & CLID & DisbDate, "LG0H Ref", Format(Date, "MM/DD/YYYY"), batchNo
            Close #7
        Loop
        'close data files
        Close #3
        
        'update the log
        If Sp.Common.TestMode Then
            Open "T:\pmtlistcrlog.txt" For Output As #7
        Else
            Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
        End If
        
        Write #7, "done", "LG0H Canc", Format(Date, "MM/DD/YYYY"), batchNo
        Close #7

Canc:   If FileLen("T:\pmtlistccan.txt") = 0 Or _
           lTrNo = "done" Then
            Exit Sub
        End If
        'update recovery variables
        If Sp.Common.TestMode Then
            Open "T:\pmtlistcrlog.txt" For Input As #7
        Else
            Open "X:\PADD\Logs\pmtlistcrlog.txt" For Input As #7
        End If
        
        Input #7, lSSN, lTrNo, lDate, batchNo
        Close #7
    'process the cancellation errors
        Open "T:\pmtlistccan.txt" For Input As #3
        
        'advance to the last record processed
        ssn = ""
        CLID = ""
        DisbDate = ""
        If lSSN <> "done" Then
            lCLID = Mid(lSSN, 10, 19)
            lDisbDate = Mid(lSSN, 29, 8)
            lSSN = Mid(lSSN, 1, 9)
            
            Do Until lSSN = ssn And CLID = lCLID And DisbDate = lDisbDate
                Input #3, ssn, CLID, SeqNo, EffDate, DisbDate, DisbAmt, LnTyp
            Loop
        End If
        
        'process each transaction
        Do While Not EOF(3)
            'input information
            Input #3, ssn, CLID, SeqNo, EffDate, DisbDate, DisbAmt, LnTyp
            'process FFELP loans
            If LnTyp = "STFFRD" Or LnTyp = "UNSTFD" Or LnTyp = "PLUS" Or LnTyp = "PLUSGB" Then
                'access LG0H
                .TransmitTerminalKey rcIBMClearKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitANSI "LG0HC;" & ssn & ";;" & CLID
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                'locate disbursement
                col = 20
                Do Until .GetDisplayText(12, col, 8) = Format(DisbDate, "MMDDYYYY")
                    col = col + 16
                    If col > 68 Then
                        .TransmitTerminalKey rcIBMPf10Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        col = 20
                        If check4text(10, 21, "DISB# 01") Then
                            col = 0
                            Exit Do
                        End If
                    End If
                Loop
                'update the information
                If col <> 0 Then
                    'source code
                    .MoveCursor 2, 14
                    .TransmitANSI "E"
                    'canc amt
                    .MoveCursor 13, col - 4
                    .TransmitANSI "U"
                    'total refund amount
                    .TransmitANSI Format(DisbAmt, "000,000.00")
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    'effective date
                    .MoveCursor 14, col
                    .TransmitANSI Format(DisbDate, "MMDDYYYY")
                    .TransmitTerminalKey rcIBMPf6Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    If .GetDisplayText(22, 3, 5) <> "49000" Then
                        LP9O CLID, ssn, DisbDate, DisbAmt, PmtAmt, Trim(.GetDisplayText(22, 3, 50)), "CANCRFND", "" '<26> added CANCRFND
                    End If
                'add a queue task if the disbursement isn't found
                Else
                    Sp.Common.AddLP9O ssn, "CANCRFND", , "CLID = " & CLID & ", Disb Date = " & DisbDate & ", Disb Amt = $" & DisbAmt & ", Cancel Amt = $" & PmtAmt & ", Effective Date = " & DisbDate & ", Error = no disb with this date exists"
                End If
            End If
            
            'update the log
            If Sp.Common.TestMode Then
                Open "T:\pmtlistcrlog.txt" For Output As #7
            Else
                Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
            End If
            Write #7, ssn & CLID & DisbDate, "LG0H Canc", Format(Date, "MM/DD/YYYY"), batchNo
            Close #7
        Loop
        
        'close data files
        Close #3
        
        'update the log
        If Sp.Common.TestMode Then
            Open "T:\pmtlistcrlog.txt" For Output As #7
        Else
            Open "X:\PADD\Logs\pmtlistcrlog.txt" For Output As #7
        End If
    
        Write #7, "done", "done", Format(Date, "MM/DD/YYYY"), batchNo
        Close #7

    End With
End Sub
'</22>


'<12->
'add queue tasks
Sub ATD22(ARC As String, SeqNo As Integer, Msg As String)
    With Session
        'access TD22
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "TX3Z/ATD22" & ssn
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'find the ARC
        Do
            found = .FindText(ARC, 8, 8)
            If found Then Exit Do
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If .GetDisplayText(23, 2, 5) = "90007" Then
                MsgBox "You do not have access to the ARC " & ARC & ".  Contact Systems Support to get access to the ARC and then run the script again.", , "ARC not Found"
''                Close #1
                Close #3
                Close #5
                End
            End If
        Loop

        'select the ARC
        .MoveCursor .FoundTextRow, .FoundTextColumn - 5
        .TransmitANSI "01"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'select the loans
        Do
            'find the loan
            found = .FindText(Format(SeqNo, "_ 00# "), 11, 3)
            'select the loan and go to next record if the loan is found
            If found Then
                'select the loan
                .MoveCursor .FoundTextRow, 3
                .TransmitANSI "X"
                Exit Do
            'go to the next page if the loan is not found
            Else
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            End If
        Loop
        'enter the comment
        .MoveCursor 21, 2
        .TransmitANSI Msg & "  {CXL/REF}"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
    
    End With
End Sub
'</12>

'<12->
'replace the payment amounts in the refund file with the correct amounts from the batch
Sub Rplace()
    Dim row As Integer
    
    With Session
        If FileLen("T:\pmtlistcref.txt") <> 0 Then
            'access TS1J
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "TX3Z/ITS1D" & batchNo
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'process each record in the refund file
            Open "T:\pmtlistcref.txt" For Input As #3
            Open "T:\pmtlistcreftmp.txt" For Output As #6
            row = 8
            Do Until EOF(3)
                'get data from the refund file
                Input #3, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                          DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV '<8> added PCV
                'find the transaction on the screen
'<22->
'                Do Until .GetDisplayText(row, 5, 3) & .GetDisplayText(row, 9, 2) & _
'                         .GetDisplayText(row, 12, 4) = SSN And _
'                         .GetDisplayText(row, 43, 2) = "40"
                Do Until check4text(row, 5, ssn) And check4text(row, 43, "40")
'</22>
                    row = row + 1
                    If .GetDisplayText(row, 3, 1) = " " Then
                        .TransmitTerminalKey rcIBMPf8Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        row = 8
                    End If
                    Exit Do
                Loop
                'get the pmt from the screen
                PmtAmt = CDbl(.GetDisplayText(row, 17, 10))
                'write the data out to a temp storage file with the pmt amt from the screen
                Write #6, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                          DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV '<8> added PCV
                row = row + 1
            Loop
            Close #3
            Close #6
            'copy the temp storage file back to the refund file
            Open "T:\pmtlistcreftmp.txt" For Input As #6
            Open "T:\pmtlistcref.txt" For Output As #3
            Do Until EOF(6)
                Input #6, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                          DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV '<8> added PCV
                Write #3, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
                          DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV '<8> added PCV
            Loop
            Close #6
            Close #3
        End If
    End With
End Sub
'</12>

'<28->
''<16->
''Sub LP9O(CLUID As String, Soc, DisbDt, Disb, Canc, ErrMsg As String, Queue As String)
''<26>
'Sub LP9O(CLUID As String, Soc As String, DisbDt, Disb, Canc, ErrMsg As String, Queue As String, Reason As String)
''</26>
'If Queue = "CANCRFND" Then
'    SP.Common.AddLP9O Soc, Queue, , "CLID = " & CLUID & ", Disb Date = " & DisbDt & ", Disb Amt = $" & Disb & _
'                          ", Cancel Amt = $" & Canc & ", Effective Date = " & _
'                          DisbDt & ", Error = " & ErrMsg
'ElseIf Queue = "GCNCLREI" Then
''<27>SP.Common.AddLP9O Soc, Queue, , "SCHOOL:" & CurrSchool & ", LOAN:" & LnTyp & ", APP:" & APL & ", CLUID:" & CLUID & ", ORG DISB DT:" & DisbDt & ", ORG DISB AMT:" & Disb
'    SP.Common.AddLP9O Soc, Queue, , "SCHOOL:" & School & ", LOAN:" & LnTyp & ", APP:" & APL & ", CLUID:" & CLUID & ", ORG DISB DT:" & DisbDt & ", ORG DISB AMT:" & Disb & ",Reason:" & Reason
''</27>
'ElseIf Queue = "GREISSUE" Then
''<27>SP.Common.AddLP9O Soc, Queue, , "LEND ID:" & LenderID & ", LOAN:" & LnTyp & ", APP:" & APL & ", CLUID:" & CLUID & ", ORG DISB DT:" & DisbDt & ", ORG DISB AMT:" & Disb & ", NEW DISB DATE:" & NewDisbDate & ", NEW DISB AMT:" & NewDisbAmt
'    SP.Common.AddLP9O Soc, Queue, , "SCHOOL:" & School & ", LEND ID:" & LenderID & ", LOAN:" & LnTyp & ", APP:" & APL & ", CLUID:" & CLUID & ", ORG DISB DT:" & DisbDt & ", ORG DISB AMT:" & Disb & ", NEW DISB DATE:" & NewDisbDate & ", NEW DISB AMT:" & NewDisbAmt & ",Reason:" & Reason
''</27>
'End If
''<26>
''    Dim cntr As Integer
''
''    With Session
''        cntr = 0
''        Do Until .GetDisplayText(22, 3, 5) = "44000" Or cntr = 3
''            If cntr > 0 Then
''                .Wait 5
''            End If
''            .TransmitTerminalKey rcIBMClearKey
''            .WaitForDisplayString " ", "0:0:30", 1, 2
''            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
''            .TransmitANSI "LP9OA" & Soc & ";;CANCRFND"
''            .TransmitTerminalKey rcIBMEnterKey
''            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
''            cntr = cntr + 1
''        Loop
''        'prompt user if unable to add task
''        If .GetDisplayText(22, 3, 5) <> "44000" Then
''            MsgBox "Unable to add the task.  Wait for the script to finish and then manually enter a task in " & Queue & " for " & SSN & "."
''        'enter info
''        Else
''            .MoveCursor 16, 12
'''<20>
'''            .TransmitANSI ", Disb Date = " & DisbDt & ", Disb Amt = $" & Disb & _
''                          ", Cancel Amt = $" & Canc & ", Effective Date = " & _
''                          DisbDt & ", Error = " & ErrMsg
''            .TransmitANSI "CLID = " & CLUID & ", Disb Date = " & DisbDt & ", Disb Amt = $" & Disb & _
''                          ", Cancel Amt = $" & Canc & ", Effective Date = " & _
''                          DisbDt & ", Error = " & ErrMsg
'''</20>
''            .TransmitTerminalKey rcIBMPf6Key
''            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
''            .Wait 2 'go figure why this has to be here but it wouldn't work otherwise
''        End If
''    End With
''</26>
'End Sub
'</16>

Sub LP9O(CLUID As String, Soc As String, DisbDt, Disb, Canc, ErrMsg As String, Queue As String, Optional Reason As String = "")
    If Queue = "CANCRFND" Then
        Sp.Common.AddLP9O Soc, Queue, , "CLID = " & CLUID & ", Disb Date = " & DisbDt & ", Disb Amt = $" & Disb & _
                              ", Cancel Amt = $" & Canc & ", Effective Date = " & _
                              DisbDt & ", Error = " & ErrMsg
    End If
End Sub
'</28>


'<23->
'generate the posting file detail and end the script
Function PostingDetail()
    'open the batch detail workbook
    Dim ExcelApp As excel.Application
    Set ExcelApp = CreateObject("Excel.Application")
    ExcelApp.Workbooks.Open FileName:="X:\PADD\Compass\Payments\CanRefPmtBatch.xls"
    ExcelApp.Visible = True 'display Excel window
    'enter the current date
    ExcelApp.Range("B2").Select
    ExcelApp.ActiveCell.FormulaR1C1 = Date
    'open the posting file for input
    Open "T:\pmtlistcr.txt" For Input As #1
    'enter the information from each record in the posting file
    TotPmtAmt = 0
    row = 7
    Do Until EOF(1)
        'read in the record and write the information in the workbook
'<5->               Input #1, SSN, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
            DisbDate, DisbAmt, ChkAmt, PmtCd, LName, Targ
        Input #1, ssn, SeqNo, LnTyp, EffDate, PmtAmt, Reissue, LenderID, School, _
            DisbDate, DisbAmt, ChkAmt, PmtCd, lname, Targ, CLID, PCV, OwnerID '<8> added PCV     '</5>
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = ssn
        ExcelApp.Range("B" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = lname
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = SeqNo
        ExcelApp.Range("D" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = LnTyp
        ExcelApp.Range("E" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = PmtAmt
        ExcelApp.Range("F" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = EffDate
        ExcelApp.Range("G" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = PmtCd
        ExcelApp.Range("H" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = Reissue
        ExcelApp.Range("I" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = LenderID
        ExcelApp.Range("J" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = School
        ExcelApp.Range("K" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = DisbDate
        ExcelApp.Range("L" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = DisbAmt
        ExcelApp.Range("M" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = ChkAmt
        ExcelApp.Range("N" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = Targ
        TotPmtAmt = TotPmtAmt + PmtAmt                      '<1>
        row = row + 1
    Loop
    'close the posting file
    Close #1
    'print the sum of all payment amounts                   '<1>
    ExcelApp.Range("A" & row + 1).Select
    ExcelApp.ActiveCell.FormulaR1C1 = "Total Payment Amount For This Batch:"
    ExcelApp.Range("E" & row + 1).Select
    ExcelApp.ActiveCell.FormulaR1C1 = TotPmtAmt            '</1>
'<26>
    ExcelApp.Columns("L:L").Select
    ExcelApp.selection.Columns.AutoFit
    ExcelApp.Columns("M:M").Select
    ExcelApp.selection.Columns.AutoFit
'</26>
    'hide Excel
    ExcelApp.Visible = False
    'tell the user processing is complete
    warn = MsgBox("Processing complete.", vbOKOnly, "Processing Complete")
    'display Excel window
    ExcelApp.Visible = True

    'End Commented out so that it will end after the function is called if desired.
End Function
'</23>

'target transaction
Function TargetIt(selRec As String) As Boolean
    Dim seq As Integer
    
    hit "F2"
    'access the targeting screen
    puttext 22, 17, selRec, "F6"
    
    'target the payment if the targeting screen is displayed
    If check4text(1, 72, "TSX1O") Then
        seq = 12
        'find disbursement to target
        Do
            'go to next page
            If Session.GetDisplayText(seq, 27, 2) = "  " Then
                hit "F8"
                If check4text(23, 2, "90007") Then Exit Do
                seq = 12
            End If
            
            'select disbursement if match is found
            If val(GetText(seq, 27, 2)) = SeqNo And GetText(seq, 30, 8) = DisbDate Then
                puttext seq, 3, "X", "ENTER"
                Exit Do
            End If
            seq = seq + 1
        Loop
        
        'add the record to a data file if there was an error while targeting
        If Not check4text(23, 2, "01004") Then
            Open "T:\targerr.txt" For Append As #2
            Write #2, ssn, SeqNo, PmtAmt, DisbDate, DisbAmt, ChkAmt
            Close #2
        End If
        TargetIt = True
    'return false if targeting screen is not displayed
    Else
        TargetIt = False
    End If
End Function

'clear duplicate warning
Function ClearDuplicate(selRec As String, lname As String)
    'change hotkeys
    hit "F2"
    
    'access the comment screen
    puttext 22, 17, selRec, "F4"
    
    'enter borrower last name and comment
    puttext 11, 17, lname
    puttext 19, 2, "not duplicate / " & userId, "ENTER"
    
    'return to posting screen, commit payment, and change hotkeys
    hit "F12"
    hit "ENTER"
    hit "F2"
End Function

'Loans deconverted
Function LoansDeconvertedProcessing(selRec As String, lname As String)
    'change hotkeys
    hit "F2"
    
    'access the comment screen
    puttext 22, 17, selRec, "F4"
    
    'enter borrower last name and comment
    puttext 11, 17, lname
    puttext 19, 2, "Loans Deconverted / " & userId, "ENTER"
    
    'return to posting screen, commit payment, and change hotkeys
    hit "F12"
    hit "ENTER"
    hit "F2"
    Open "T:\targerr.txt" For Append As #2
    Write #2, ssn, SeqNo, PmtAmt, DisbDate, DisbAmt, ChkAmt
    Close #2
End Function

'Account not on system
Function AccountNotOnSystemProcessing(selRec As String)
    'change hotkeys
    hit "F2"
    
    'access the comment screen
    puttext 22, 17, selRec, "F4"
    
    'enter borrower last name and comment
    puttext 11, 17, "UNKNOWN"
    puttext 19, 2, "Loans Not On COMPASS / " & userId, "ENTER"
    
    'return to posting screen, commit payment, and change hotkeys
    hit "F12"
    hit "ENTER"
    hit "F2"
    Open "T:\targerr.txt" For Append As #2
    Write #2, ssn, SeqNo, PmtAmt, DisbDate, DisbAmt, ChkAmt
    Close #2
End Function


'new  sr 151, jd, 11/01/02, 11/07/02
'<1>  sr 162, jd, 11/14/02, 11/15/02
'<2>  sr 164, jd, 11/14/02, 11/15/02
'<3>  sr 165, jd, 11/15/02, 11/18/02
'<4>  sr 211, mc, 02/03/03, 02/10/03
'<5>  sr 231, jd, 03/03/03, 03/27/03
'<6>  sr 250, jd, 03/27/03, 03/31/03
'<7>  sr 267, jd, 04/25/03, 05/06/03
'<8>  sr 286, jd, 05/05/03, 05/06/03
'<9>  sr 329, jd, 06/11/03, 06/30/03 changed \\Utahsbr1\APPS\Apps\FileTransfers\COMMON\OneLINK\OPA\ to X:\PADD\
'<10> sr 378, jd, 07/23/03, 09/22/03
'<11> sr 443, jd, 10/14/03, 10/28/03
'<12> sr 465, jd, 12/17/03, 02/03/04
'<13> sr 535, jd, 02/10/04, 02/17/04
'<14> sr 554, jd, 02/27/04, 03/04/04
'<15> sr 608, aa, 04/05/04, 04/17/04
'<16> sr 599, jd, 04/20/04, 04/28/04
'<17> sr 649, aa, 05/18/04, 05/19/04
'<18> sr 667, jd, 06/11/04, 06/11/04 updated code for changes to PO1N
'<19> sr 673, aa, 06/17/04, 06/17/04
'<20> sr 818, tp, 10/25/04, 12/02/04 added CLUID to queue task
'<21> sr1285, jd, all changes are in the form CanRefPmtInfo
'<22> sr1411, jd
'<23> sr1471, jd
'<24> sr1632, jd
'<25> sr1673, jd, added PLUSGB
'<26> sr1890, tp
'<27> sr1932, tp, 12/18/06
'<28> sr2051, jd
'<29> sr2216, db
'<30> sr2217, db



