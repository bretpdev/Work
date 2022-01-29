Attribute VB_Name = "R301Queue"
Private Type QueueData
    ssn As String
    loanSequence As String
    Status As String
    DateRequested As String
    WasWorked As Boolean
End Type

Private Type Transaction
    loanSequence As String
    amount As String
    IsCredit As Boolean
    Type As String
End Type

Private Type WriteUpLoan
    Sequence As String
    CreditAmount As String
End Type

Private Const TEMP_FOLDER As String = "T:\"
Private Const TRANSACTION_DATA As String = TEMP_FOLDER & "R301TransactionLevelData.txt"
Private m_UserId As String
Private m_ReassignmentUserId As String
Private m_QueueData() As QueueData

Public Sub Main()
    If MsgBox("This script will work tasks in the R301 queue.", vbOKCancel) <> vbOK Then End
    
    m_UserId = Sp.Common.GetUserID
    'm_UserId = "UT00044" 'For test
    m_ReassignmentUserId = Sp.Common.SqlEx("SELECT AssignID FROM R301_LST_Users WHERE UserID = '" & m_UserId & "'")(0, 0)
    If m_ReassignmentUserId = "" Then
        MsgBox "You are not authorized to use this script. Ending Script."
        End
    End If
    
    FastPath "TX3Z/ITX6XR301"
    If check4text(23, 2, "80014") Then
        MsgBox "You do not have access to the R301 Queue. Contact System Support."
        End
    ElseIf check4text(23, 2, "01020") Then
        MsgBox "The queue is empty. Process Complete."
        End
    End If
    
    Dim row As Integer
    ReDim m_QueueData(0)
    Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        For row = 8 To 20 Step 3
            If GetText(row, 47, 10) <> "" Then
                m_QueueData(UBound(m_QueueData)).ssn = GetText(row, 6, 9)
                m_QueueData(UBound(m_QueueData)).loanSequence = GetText(row, 15, 4)
                m_QueueData(UBound(m_QueueData)).Status = GetText(row, 75, 1)
                m_QueueData(UBound(m_QueueData)).DateRequested = GetText(row, 47, 10)
                ReDim Preserve m_QueueData(UBound(m_QueueData) + 1)
            End If
        Next row
        hit "F8"
    Loop
    ReDim Preserve m_QueueData(UBound(m_QueueData) - 1)
    
    WorkQueue
    
    Dim docPath As String
    Dim refundData As String
    docPath = "X:\PADD\BorrowerServices\"
    TestMode , docPath
    refundData = RollUpDataFile(TRANSACTION_DATA)
    If Dir(refundData) <> "" Then
        PrintDocs docPath, "RFDPIFB", DocCreateAndDeploy.AddBarcodeAndStaticCurrentDate(refundData, "AN", "RFDPIFB", True)
        Kill refundData
    End If
    If Dir(TRANSACTION_DATA) <> "" Then Kill TRANSACTION_DATA
    
    MsgBox "Process Complete!"
End Sub

'Takes an R301 data file at a transaction level and creates a new data file (R301a)
'grouped at the account level, with the transaction amounts being summed.
Private Function RollUpDataFile(ByVal transactionDataFile As String) As String
    Dim keyline As String
    Dim firstName As String
    Dim lastName As String
    Dim address1 As String
    Dim address2 As String
    Dim city As String
    Dim state As String
    Dim zip As String
    Dim country As String
    Dim accountNumber As String
    Dim amount As String
    Dim processedAccountNumbers As String
    Dim accountTotal As Double
    Dim searchAccountNumber As String
    Dim costCenter As String
    Dim v As String
    Dim accountDataFile As String
    accountDataFile = TEMP_FOLDER & "R301AccountLevelData.txt"
    
    If Dir(transactionDataFile) <> "" Then
        'Create a new account-level data file with just a header row to start out.
        Open accountDataFile For Output As #97
            Write #97, "AN", "FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "Keyline", "RefundAmt", "State_Ind", "COST_CENTER_CODE"
        Close #97
        
        Open transactionDataFile For Input As #99
        'Get column headers out of the way.
        Input #99, accountNumber, firstName, lastName, address1, address2, city, state, zip, country, keyline, amount, state, costCenter
        Do While Not EOF(99)
            Input #99, accountNumber, firstName, lastName, address1, address2, city, state, zip, country, keyline, amount, state, costCenter
            accountTotal = 0
            'For accounts that haven't been summed yet, go through the file and get a sum of the amounts.
            If InStr(1, processedAccountNumbers, accountNumber) = False Then
                processedAccountNumbers = processedAccountNumbers & accountNumber & ","
                Open transactionDataFile For Input As #98
                Do While Not EOF(98)
                    Input #98, searchAccountNumber, v, v, v, v, v, v, v, v, v, amount, v, v
                    If searchAccountNumber = accountNumber Then accountTotal = accountTotal + CDbl(amount)
                Loop
                Close #98
                Open accountDataFile For Append As #97
                Write #97, accountNumber, firstName, lastName, address1, address2, city, state, zip, country, keyline, FormatCurrency(accountTotal, 2), state, costCenter
                Close #97
            End If
        Loop
        Close #99
    End If
    
    RollUpDataFile = accountDataFile
End Function

Private Sub WorkQueue()
    Dim workedATask As Boolean
    workedATask = False
    Dim X As Integer
    For X = LBound(m_QueueData) To UBound(m_QueueData)
        If m_QueueData(X).Status <> "A" And CDate(m_QueueData(X).DateRequested) < Date - 20 And m_QueueData(X).WasWorked = False Then
            If TaskIsValid(m_QueueData(X).ssn, m_QueueData(X).loanSequence) Then
                workedATask = True
                LocITS26 m_QueueData(X).ssn, m_QueueData(X).loanSequence
            End If
        End If
    Next X
    If workedATask = False Then
        MsgBox "There was no task found that could be worked. Ending script."
        End
    End If
End Sub

Private Sub LocITS26(ByVal ssn As String, ByVal loanSequence As String)
    Dim X As Integer
    Dim hasBalance As Boolean
    Dim hasCredit As Boolean
    Dim loanBalance As String
    Dim accountBalance As Double
    hasBalance = False
    hasCredit = False
    
    accountBalance = 0
    FastPath "TX3Z/ITS26" & ssn
    If check4text(1, 72, "TSX28") Then
        'Selection screen.
        Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            For X = 8 To 20
                If GetText(X, 59, 10) <> "0.00" And GetText(X, 59, 10) <> "" And check4text(X, 69, "CR") = False Then
                    hasBalance = True
                    loanBalance = GetText(X, 59, 8)
                    accountBalance = accountBalance + CDbl(loanBalance)
                End If
                If check4text(X, 69, "CR") Then
                    hasCredit = True
                End If
            Next X
            hit "F8"
        Loop
    ElseIf check4text(1, 72, "TSX29") Then
        'Target screen.
        If GetText(11, 18, 4) <> "0.00" And GetText(11, 18, 4) <> "" And check4text(11, 22, "CR") = False Then
            hasBalance = True
            loanBalance = GetText(11, 12, 8)
            accountBalance = CDbl(loanBalance)
        End If
        If check4text(11, 22, "CR") Then
            hasCredit = True
        End If
    End If
    
    If hasCredit = False Then
        CloseR301Tasks ssn
        CloseR304Tasks ssn
        AddCommentInTC00 ssn, "Completed R301 queue task. All loans have zero balace at time of review. {R301QUEUE}"
    End If
    
    If hasBalance Then
        AssignQueueTask ssn, m_ReassignmentUserId
    ElseIf hasCredit Then
        LocITS2C ssn, loanSequence, accountBalance
    End If
End Sub

Private Sub LocITS2C(ByVal ssn As String, ByVal loanSequence As String, ByVal accountBalance As Double)
    Dim writeUpLoans() As WriteUpLoan
    ReDim writeUpLoans(0)
    Dim comment As String
    Dim transactions() As Transaction
    ReDim transactions(0)
    Dim row As Integer
    Dim transactionRow As Integer
    Dim loanExistsWithPositiveBalance As Boolean
    loanExistsWithPositiveBalance = False
    Dim totalCreditBalance As Double
    Dim refundAmount As String

    'Gather transaction information for each loan with CR.
    FastPath "TX3Z/ITS2C" & ssn
    hit "ENTER"
    If check4text(1, 72, "TSX2M") Then
        'Selection screen.
        Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            For row = 7 To 20
                If check4text(row, 69, "CR") Then
                    totalCreditBalance = totalCreditBalance + CDbl(GetText(row, 57, 12))
                    puttext 21, 18, Format(GetText(row, 2, 2), "00"), "ENTER"
                    Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        For transactionRow = 11 To 21
                            If GetText(transactionRow, 33, 4) <> "" Then
                                transactions(UBound(transactions)).loanSequence = GetText(5, 29, 4)
                                transactions(UBound(transactions)).amount = GetText(transactionRow, 39, 12)
                                transactions(UBound(transactions)).IsCredit = check4text(transactionRow, 51, "CR")
                                transactions(UBound(transactions)).Type = GetText(transactionRow, 33, 4)
                                ReDim Preserve transactions(UBound(transactions) + 1)
                            End If
                        Next transactionRow
                        hit "F8"
                    Loop
                    hit "F12"
                ElseIf GetText(row, 66, 3) <> ".00" And GetText(row, 66, 3) <> "" Then
                    loanExistsWithPositiveBalance = True
                End If
            Next row
            hit "F8"
        Loop
    Else
        'Target screen.
        If check4text(7, 78, "CR") Then
            totalCreditBalance = CDbl(GetText(7, 67, 11))
            Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For transactionRow = 11 To 21
                    If GetText(transactionRow, 33, 4) <> "" Then
                        transactions(UBound(transactions)).loanSequence = GetText(5, 29, 4)
                        transactions(UBound(transactions)).amount = GetText(transactionRow, 39, 12)
                        transactions(UBound(transactions)).IsCredit = check4text(transactionRow, 51, "CR")
                        transactions(UBound(transactions)).Type = GetText(transactionRow, 33, 4)
                        ReDim Preserve transactions(UBound(transactions) + 1)
                    End If
                Next transactionRow
                hit "F8"
            Loop
        ElseIf check4text(7, 78, "CR") = False And CDbl(GetText(7, 67, 11)) > 0 Then
            loanExistsWithPositiveBalance = True
        End If
    End If
    'Remove empty array index.
    If UBound(transactions) > 0 Then ReDim Preserve transactions(UBound(transactions) - 1)
    
    'If the payment that caused the over payment is in (1010,1027,1029,1036,1037,1038,1039,1041,5002) and there does exist a 1070 or 1080 consolidation payment transaction the script should do the following
    If (transactions(0).Type = "1010" Or transactions(0).Type = "1027" Or transactions(0).Type = "1029" _
        Or transactions(0).Type = "1036" Or transactions(0).Type = "1037" Or transactions(0).Type = "1038" _
        Or transactions(0).Type = "1039" Or transactions(0).Type = "1041" Or transactions(0).Type = "5002") _
        And (CreditTransactionsContainType(transactions, "1070") Or CreditTransactionsContainType(transactions, "1080")) Then
            AssignQueueTask ssn, "UT00049"
            Exit Sub
    'If the payment that caused the over pay situation is a military/americorp  1035 or 1020 or 1021
    ElseIf transactions(0).Type = "1035" Or transactions(0).Type = "1020" Or transactions(0).Type = "1021" Then
        AssignQueueTask ssn, "UT00296"
        Exit Sub
    'If the payment that caused the overpayment is a 1070 (out of network consol pmt), the script should do the following
    ElseIf transactions(0).Type = "1070" Then
        AssignQueueTask ssn, "UT00296"
        Exit Sub
    'If the payment that caused the over pay situation is a 1027, 1029 or 1025 and there are no prior payments by the borrower (1010)
    ElseIf HasPrior1010(transactions) Then
        AssignQueueTask ssn, m_ReassignmentUserId
        Exit Sub
    ElseIf transactions(0).Type = "1040" Or transactions(0).Type = "1045" Then
        AssignQueueTask ssn, "UT00296"
        Exit Sub
    ElseIf transactions(0).Type = "1010" Then
        'process refund
    Else
        AssignQueueTask ssn, "UT00296"
        Exit Sub
    End If
    
    If loanExistsWithPositiveBalance = False And totalCreditBalance < 5# Then
        'Write off.
        FastPath "TX3Z/ATS1V" & GetAccountNumber(ssn)
        Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            For row = 8 To 17
                If GetText(row, 11, 2) <> "" Then
                    writeUpLoans(UBound(writeUpLoans)).Sequence = GetText(row, 11, 2)
                    writeUpLoans(UBound(writeUpLoans)).CreditAmount = Replace(Replace(GetText(row, 49, 11), "$", ""), ",", "")
                    ReDim Preserve writeUpLoans(UBound(writeUpLoans) + 1)
                    puttext row, 4, "X"
                End If
            Next row
            ReDim Preserve writeUpLoans(UBound(writeUpLoans) - 1)
            'Enter and post comment.
            comment = "Completed R301 queue task.  Processed write up loan seq "
            Dim X As Integer
            For X = 0 To UBound(writeUpLoans)
                comment = comment & writeUpLoans(X).Sequence & " amount " & writeUpLoans(X).CreditAmount & ", "
            Next X
            comment = Mid(comment, 1, Len(comment) - 2)
            comment = comment & " {R301QUEUE}"
            puttext 19, 2, comment, "F6"
            'Check for more loans.
            hit "F8"
        Loop

        'Complete and close task.
        FastPath "TX3Z/ITX6T" & ssn
        Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            For row = 7 To 20
                If GetText(row, 3, 1) <> "" And check4text(row, 8, "R3") And check4text(row, 24, "01") Then
                    If WriteUpLoansContainLoanSequence(writeUpLoans, GetText(row, 49, 4)) Then
                        puttext 21, 18, Format(GetText(row, 2, 2), "00"), "ENTER"
                        refundAmount = GetText(7, 20, 14)
                        'Blank out refund amount.
                        puttext 9, 12, "Y0"
                        hit "END"
                        hit "ENTER"
                        hit "F11"
                        puttext 9, 19, "COMPL"
                        puttext 8, 19, "C", "ENTER"
                        If check4text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED") = False Then
                            puttext 9, 19, "", "END"
                            hit "ENTER"
                        End If
                        Dim writeOffLoanSequences(0) As Integer
                        writeOffLoanSequences(0) = CInt(loanSequence)
                        ATD22ByLoan ssn, "R3WUP", "Completed R301 queue task.  Processed Write Up - " & loanSequence & ", " & refundAmount & ", Total Write up Amount for all loans is " & totalCreditBalance, writeOffLoanSequences, "R301QUEUE", m_UserId
                        Exit Do
                    End If
                End If
            Next row
            hit "F8"
        Loop
    ElseIf loanExistsWithPositiveBalance = False And totalCreditBalance >= 5# Then
        'Generate refund.
        If (CreditTransactionsContainSequenceOfType(transactions, "1010", loanSequence) Or CreditTransactionsContainSequenceOfType(transactions, "1027", loanSequence) Or CreditTransactionsContainSequenceOfType(transactions, "1029", loanSequence)) And CreditTransactionsContainSequenceOfType(transactions, "1070", loanSequence) = False And CreditTransactionsContainSequenceOfType(transactions, "1080", loanSequence) = False Then
            'Complete and close task.
            FastPath "TX3Z/ITX6T" & ssn
            Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For row = 7 To 20
                    If GetText(row, 3, 1) <> "" And check4text(row, 8, "R3") And check4text(row, 24, "01") Then
                        If loanSequence = GetText(row, 49, 4) Then
                            puttext 21, 18, Format(GetText(row, 2, 2), "00"), "ENTER"
                            refundAmount = GetText(7, 20, 14)
                            If (accountBalance <> val(refundAmount)) Then
                                AssignQueueTask ssn, "UT00296"
                                Exit Sub
                            End If
                            
                            If check4text(12, 25, "_________") Or GetText(12, 25, 9) = "" Or check4text(12, 25, "828476") Then
                                puttext 19, 13, "1"
                                hit "F4"
                                hit "F11"
                            End If
                            
                            puttext 9, 12, "Y"
                            hit "ENTER"
                            hit "F11"
                            puttext 8, 19, "C", "ENTER"
                            If check4text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED") = False Then
                                puttext 9, 19, "", "END"
                                hit "ENTER"
                            End If
                            Dim refundLoanSequences(0) As Integer
                            refundLoanSequences(0) = CInt(loanSequence)
                            ATD22ByLoan ssn, "R3RFX", "Completed R301 queue task.  Process refund to borrower for  " & refundAmount, refundLoanSequences, "R301QUEUE", m_UserId
                            AddLetterRecord ssn, refundAmount
                            Exit Do
                        End If
                    End If
                Next row
                hit "F8"
            Loop
        Else
            CloseR301Tasks ssn
        End If
    End If
End Sub

Private Sub AddLetterRecord(ByVal ssn As String, ByVal amount As String)
    Dim firstName  As String
    Dim lastName  As String
    Dim address1  As String
    Dim address2 As String
    Dim city As String
    Dim state As String
    Dim zip As String
    Dim country As String
    Dim accountNumber As String
    GetTX1J ssn, accountNumber, lastName, , firstName, address1, address2, , city, state, zip, country
    If Dir(TRANSACTION_DATA) = "" Then
        Open TRANSACTION_DATA For Output As #5
            Write #5, "AN", "FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "Keyline", "RefundAmt", "State_Ind", "COST_CENTER_CODE"
            Write #5, accountNumber, firstName, lastName, address1, address2, city, state, zip, country, ACSKeyLine(ssn), amount, state, "MA4119"
        Close #5
    Else
        Open TRANSACTION_DATA For Append As #5
            Write #5, accountNumber, firstName, lastName, address1, address2, city, state, zip, country, ACSKeyLine(ssn), amount, state, "MA4119"
        Close #5
    End If
End Sub

Private Function WriteUpLoansContainLoanSequence(ByRef writeUpLoans() As WriteUpLoan, ByVal loanSequence As String) As Boolean
    Dim X As Integer
    For X = 0 To UBound(writeUpLoans)
        If loanSequence = Format(writeUpLoans(X).Sequence, "0000") Then
            WriteUpLoansContainLoanSequence = True
            Exit Function
        End If
    Next X
    WriteUpLoansContainLoanSequence = False
End Function

Private Function GetAccountNumber(ByVal ssn As String) As String
    FastPath "TX3Z/ITX1JB;" & ssn
    GetAccountNumber = Replace(GetText(3, 34, 12), " ", "")
End Function

'Check whether the payment that caused the over pay situation is in (1027,1029,1036,1037,1038,1039,5002,1025)
'and there are prior 1010 payments by the borrower.
Private Function HasPrior1010(ByRef transactions() As Transaction) As Boolean
    Dim hasBaseType As Boolean
    Dim has1010 As Boolean
    Dim X As Integer
    HasPrior1010 = False
    hasBaseType = False
    has1010 = False
    For X = 0 To UBound(transactions)
        If transactions(0).Type = "1027" Or transactions(0).Type = "1029" Or transactions(0).Type = "1036" _
            Or transactions(0).Type = "1037" Or transactions(0).Type = "1038" Or transactions(0).Type = "1039" _
            Or transactions(0).Type = "1025" Or transactions(0).Type = "5002" And transactions(X).IsCredit Then
            hasBaseType = True
        End If
        If transactions(X).Type = "1010" Then
            has1010 = True
        End If
        If hasBaseType And has1010 Then
            HasPrior1010 = True
            Exit Function
        End If
    Next X
End Function

Private Function CreditTransactionsContainType(ByRef transactions() As Transaction, ByVal transactionType As String) As Boolean
    Dim X As Integer
    For X = 0 To UBound(transactions)
        If transactions(X).Type = transactionType And transactions(X).IsCredit Then
            CreditTransactionsContainType = True
            Exit Function
        End If
    Next X
    CreditTransactionsContainType = False
End Function

Private Function CreditTransactionsContainSequenceOfType(ByRef transactions() As Transaction, ByVal transactionType As String, ByVal loanSequence As String) As Boolean
    Dim X As Integer
    For X = 0 To UBound(transactions)
        If transactions(X).Type = transactionType And transactions(X).loanSequence = loanSequence And transactions(X).IsCredit Then
            CreditTransactionsContainSequenceOfType = True
            Exit Function
        End If
    Next X
    CreditTransactionsContainSequenceOfType = False
End Function

Private Sub AssignQueueTask(ByVal ssn As String, ByVal userId As String)
    FastPath "TX3Z/CTX6J"
    puttext 7, 42, "R3"
    puttext 8, 42, "01"
    puttext 9, 42, ssn & "*"
    puttext 9, 76, "L", "ENTER"
    
    If check4text(2, 27, "ASSIGN/REPRIORITIZE TASKS SELECTION") Then
        Dim row As Integer
        Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            For row = 9 To 19 Step 2
                If GetText(row, 2, 2) <> "" Then
                    puttext 21, 18, Format(GetText(row, 2, 2), "00"), "ENTER"
                    puttext 8, 15, userId
                    hit "ENTER"
                    hit "F12"
                End If
            Next row
            hit "F8"
        Loop
    Else
        puttext 8, 15, userId
        hit "ENTER"
    End If
End Sub

Private Sub AddCommentInTC00(ByVal ssn As String, ByVal comment As String)
    FastPath "TX3Z/ATC00" & ssn
    puttext 19, 38, "4", "ENTER"
    puttext 12, 10, comment, "ENTER"
End Sub

'Check whether all request dates for a borrower are within the last 20 days.
Private Function TaskIsValid(ByVal ssn As String, ByVal loanSequence As String) As Boolean
    Dim X As Integer
    For X = LBound(m_QueueData) To UBound(m_QueueData)
        If m_QueueData(X).ssn = ssn Then
            If m_QueueData(X).loanSequence = loanSequence Then m_QueueData(X).WasWorked = True
            If CDate(m_QueueData(X).DateRequested) > Date - 20 Then
                TaskIsValid = False
                Exit Function
            End If
        End If
    Next X
    TaskIsValid = True
End Function

Private Sub CloseR301Tasks(ByVal ssn As String)
    Dim row As Integer
    
    FastPath "TX3Z/ITX6T" & ssn
    If check4text(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Sub
    
    Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        For row = 7 To 15 Step 4
            If check4text(row, 8, "R3") And check4text(row, 24, "01") Then
                puttext 21, 18, Format(GetText(row, 2, 2), "00"), "ENTER"
                If check4text(1, 72, "J0X01") Then
                    FastPath "TX3Z/ITX6T" & ssn
                    puttext 21, 18, Format(GetText(row, 2, 2), "00"), "F2"
                    puttext 8, 19, "C", "ENTER"
                Else
                    puttext 9, 12, "Y"
                    Session.TransmitANSI "0"
                    hit "END"
                    hit "ENTER"
                    hit "F11"
                    puttext 8, 19, "C", "ENTER"
                End If
                'Go back into the queue and look for more R301 tasks.
                FastPath "TX3Z/ITX6T" & ssn
                If check4text(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Sub
                row = 3
            End If
        Next row
        hit "F8"
    Loop
End Sub

Private Sub CloseR304Tasks(ByVal ssn As String)
    Dim row As Integer
    
    FastPath "TX3Z/ITX6T" & ssn
    If check4text(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Sub
    
    Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        For row = 7 To 15 Step 4
            If check4text(row, 8, "R3") And check4text(row, 24, "04") And check4text(row + 1, 76, "W") Then
                puttext 21, 18, Format(GetText(row, 2, 2), "00")
                hit "F2"
                puttext 8, 19, "C", "ENTER"
                'Go back into the queue and look for more R304 tasks.
                FastPath "TX3Z/ITX6T" & ssn
                If check4text(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Sub
                row = 3
            End If
        Next row
        hit "F8"
    Loop
End Sub
