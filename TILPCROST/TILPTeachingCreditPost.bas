Attribute VB_Name = "TILPTeachingCreditPost"
Option Explicit

Const MBTitle As String = "TILP Teaching Credit Posting Script"
Const LogFile As String = "TILP Teaching Credit Posting Script Log.txt"
Dim FTPDir As String
Dim docDir As String
Dim LogDir As String
Dim UID As String

Sub Main()
    Dim TotalAmtOfPaymts As String
    Dim CalcTotalAmtOfPaymts As Double
    Dim CalcTotalCountOfPaymts As Long
    Dim FileInProc As String
    Dim Rec As String
    Dim AmountsConfirmed As Boolean
    Dim PreviousBatch As String
    docDir = "X:\PADD\Operational Accounting\"
    UID = Sp.Common.GetUserID()
    Sp.Common.TestMode FTPDir, docDir, LogDir
    If MsgBox("This is the TILP Teaching Credit Posting Script.  To continue please click OK otherwise Cancel.", vbCritical + vbOKCancel, MBTitle) <> vbOK Then End
    'script can't handle more than one file at a time
    If Dir(FTPDir & "TILP Teaching Credit Entry.txt") <> "" And Dir(FTPDir & "TILPTeachingCredit.R2.txt") <> "" Then
        'if one of both file types need processing
        MsgBox "The script is only able to process one file at a time.  Please remove all but one processing file from " & FTPDir & ".", vbInformation, MBTitle
        End
    End If
    'make sure one of the files exist
    If Dir(FTPDir & "TILP Teaching Credit Entry.txt") = "" And Dir(FTPDir & "TILPTeachingCredit.R2.txt") = "" Then
        'if one of both file types need processing
        MsgBox "The script couldn't find a data file to process.  Please try again.", vbInformation, MBTitle
        End
    End If
    'decide which file is to be processed
    If Dir(FTPDir & "TILP Teaching Credit Entry.txt") <> "" Then
        FileInProc = Dir(FTPDir & "TILP Teaching Credit Entry.txt")
    Else
        FileInProc = Dir(FTPDir & "TILPTeachingCredit.R2.txt")
    End If
    'read through the entire file and calculate the total amount of the payments
    Open FTPDir & FileInProc For Input As #1
    While Not EOF(1)
        Line Input #1, Rec
        CalcTotalAmtOfPaymts = CalcTotalAmtOfPaymts + CDbl(Split(Replace(Rec, """", ""), ",")(3))
        CalcTotalCountOfPaymts = CalcTotalCountOfPaymts + 1
    Wend
    Close #1
    'recovery check
    If Dir(LogDir & LogFile) <> "" Then
        Open LogDir & LogFile For Input As #1
        Input #1, PreviousBatch
        Close #1
        'delete batch in system
        FastPath "TX3Z/DTS1G" & PreviousBatch
        hit "Enter"
        'delete recovery file
        Kill LogDir & LogFile
        'delete error log because the script is going to start over
        Kill "T:\TILP Teaching Credit Posting Errors.txt"
    Else
        'loop until the user ends the script or gives the correct total
        While AmountsConfirmed = False
            'get totals from user
            While TotalAmtOfPaymts = ""
                TotalAmtOfPaymts = InputBox("Please enter the total amount of payments for posting.", MBTitle)
                If TotalAmtOfPaymts = "" Then
                    If vbYes = MsgBox("Do you wish to end the script?", vbYesNo, MBTitle) Then End
                ElseIf IsNumeric(TotalAmtOfPaymts) = False Then
                    MsgBox "That was not a valid entry.  Please try again.", vbOKOnly + vbInformation, MBTitle
                    TotalAmtOfPaymts = ""
                End If
            Wend
            'if calculated total and user entered total don't equal then
            If Round(CDbl(TotalAmtOfPaymts), 2) = Round(CalcTotalAmtOfPaymts, 2) Then
                AmountsConfirmed = True
            Else
                TotalAmtOfPaymts = ""
                MsgBox "The amount you entered was incorrect.  Please try again.", vbOKOnly + vbInformation, MBTitle
            End If
        Wend
    End If
    WriteOffInterestAndLateFees FileInProc
    'post file
    Post CalcTotalCountOfPaymts, CalcTotalAmtOfPaymts, FileInProc
    'check for errors and print word doc with errors if some were found
    If Dir("T:\TILP Teaching Credit Posting Errors.txt") <> "" Then
        MsgBox "Printing error log.  Be sure and retrieve it from the printer.", vbCritical, MBTitle
        Sp.PrintDocs docDir, "TILP Teaching Credit Posting Errors", "T:\TILP Teaching Credit Posting Errors.txt"
        Kill "T:\TILP Teaching Credit Posting Errors.txt"
    End If
    'delete data file
    Kill FTPDir & FileInProc
    'delete recovery file
    Kill LogDir & LogFile
    MsgBox "Processing Complete!", vbInformation, MBTitle
    End
End Sub

Sub WriteOffInterestAndLateFees(FileInProc As String)
    Dim fileLine As String
    Dim ssn As String
    Dim fileSeqNum As Integer
    Dim systemSeqNum As Integer
    Dim bypassLateFeeWriteOff As Boolean
    Dim row As Integer
    Dim rowForLateFees As Integer
    Dim break As Boolean
    Open FTPDir & FileInProc For Input As #1
    While Not EOF(1)
        Line Input #1, fileLine
        fileLine = Replace(fileLine, """", "")
        'write off interest
        FastPath "TX3ZATS3Q" & Split(fileLine, ",")(0) & ";" & Format(Date, "MMDDYY")
        ssn = Replace(GetText(4, 16, 11), "-", "")
        bypassLateFeeWriteOff = False
        row = 7
        systemSeqNum = CInt(GetText(row, 20, 4))
        fileSeqNum = CInt(Split(fileLine, ",")(2))
        While systemSeqNum <> fileSeqNum
            row = row + 1
            If check4text(row, 4, " ") Then
                row = 7
                hit "F8"
            End If
            systemSeqNum = CInt(GetText(row, 20, 4))
        Wend
        'target the found Seq Num
        puttext 22, 19, GetText(row, 2, 4), "Enter"
        If check4text(23, 2, "02495 NOTHING TO WRITE OFF OR CHARGE OFF") = False Then
            Dim filePrin As Double
            Dim systemPrin As Double
            filePrin = CDbl(Split(fileLine, ",")(3))
            systemPrin = CDbl(GetText(12, 32, 11))
            If filePrin >= systemPrin And val(GetText(13, 32, 11)) <> 0 Then
                puttext 8, 42, "X"
                puttext 12, 48, "0.00"
                puttext 13, 49, GetText(13, 32, 11)
                puttext 18, 2, "Wrote off outstanding interest for TILP Teaching Credit borrower", "F11"
            Else
                bypassLateFeeWriteOff = True
            End If
        End If
        'write off late fees
        If bypassLateFeeWriteOff = False Then
            FastPath "TX3ZCTS89" & ssn
            puttext 10, 36, "W"
            puttext 11, 36, "010101"
            puttext 12, 36, Format(Date, "MMDDYY")
            puttext 13, 36, "S"
            puttext 17, 36, "X", "F10"
            If check4text(23, 2, "03726") = False Then
                rowForLateFees = 12
                While val(GetText(rowForLateFees, 18, 2)) <> val(fileSeqNum) And break = False
                    rowForLateFees = rowForLateFees + 1
                    If check4text(rowForLateFees, 3, "_") = False Then
                        rowForLateFees = 12
                        hit "F8"
                        If check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") Then
                            break = True
                        End If
                    End If
                Wend
                If break = False Then
                    puttext rowForLateFees, 3, "X"
                    hit "F6"
                    hit "F6"
                End If
            End If
        End If
    Wend
    Close #1
End Sub

Sub Post(TotPmts As Long, amount As Double, FileInProc As String)
    Dim AcctNum As String
    Dim TranType As String
    Dim LnSeqNum As String
    Dim lname As String
    Dim TranAmt As String
    Dim TranDt As String
    Dim batchNo As String
    Dim row As Integer
    'add the batch
    'access ATS1G
    FastPath "TX3Z/ATS1G;"
    'enter batch info
    puttext 6, 51, "3" 'batch code
    puttext 10, 28, CStr(TotPmts) 'items in batch
    puttext 11, 28, Format(amount, "########0.00") 'total amt of batch
    puttext 15, 28, UID 'user ID
    puttext 12, 28, "54"
    hit "Enter"
    Session.TransmitTerminalKey rcIBMPrintKey 'print screen
    batchNo = GetText(6, 18, 14)
    'note batch incase it needs to be deleted
    Open LogDir & LogFile For Output As #1
    Write #1, batchNo
    Close #1
    'add transactions to the batch
    'access ATS1J
    FastPath "TX3ZATS1J" & batchNo
    row = 8
    'add 1080 transactions***************************************************************
    Open FTPDir & FileInProc For Input As #1
    Do While Not EOF(1)
        Input #1, AcctNum, TranType, LnSeqNum, TranAmt, TranDt, lname
        'enter payment info
        puttext row, 5, AcctNum
        puttext row, 17, Format(TranAmt, "#####0.00")
        puttext row, 28, Format(Date, "MMDDYY")
        puttext row, 41, Right(TranType, 2), "Enter"
        hit "Enter"
        'clear duplicate transactions error
        If Session.GetDisplayText(23, 2, 5) = "30008" Then
            'remove data from screen
            hit "F2"
            puttext 22, 17, GetText(row, 2, 2), "F4"
            puttext 11, 17, lname
            puttext 19, 2, "not duplicate / " & UID, "Enter"
            hit "F12"
            hit "Enter"
            hit "F2"
        End If
        'target the transaction
        puttext 22, 17, GetText(row, 2, 2), "F6"
        'Do the target thing
        If ProcessTargeting(CInt(LnSeqNum), TranAmt) = False Then
            Open "T:\TILP Teaching Credit Posting Errors.txt" For Append As #6
            'if file is blank then insert header row
            If FileLen("T:\TILP Teaching Credit Posting Errors.txt") = 0 Then
                Write #6, "AcctNum", "TranAmt", "TranDt", "Error"
            End If
            'write data out to file
            Write #6, AcctNum, Format(TranAmt, "$###,##0.00"), TranDt, GetText(23, 2, 62)
            Close #6
        End If
        hit "F12"
        row = row + 1
        'go to the next screen if the screen is full
        If row = 20 Then
            If check4text(24, 13, "SET1") Then hit "F2"
            hit "F8"
            row = 8
        End If
    Loop
    Close #1
    'verify batch
    FastPath "TX3ZCTS1R" & batchNo
    'release batch if totals equal
    If CDbl(GetText(11, 28, 13)) = CDbl(GetText(10, 67, 13)) Then
        hit "F10"
        Session.TransmitTerminalKey rcIBMPrintKey 'print screen
    End If
End Sub

'this Function does targeting for the transactions
Function ProcessTargeting(SeqNo As Integer, ReappAmt As String) As Boolean
    Dim SeqNoArray() As String
    Dim ReappAmtArray() As String
    Dim row As Integer
    Dim i As Integer
    row = 12
    'mark and enter target amount for loans
    While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        'check if seq number is in gathered seq nums
        If GetText(row, 50, 2) = Format(SeqNo, "0#") Then
            puttext row, 3, "X"
            puttext row, 6, ReappAmt
        End If
        row = row + 1
        'page forward if needed
        If check4text(row, 3, "_") = False Then
            hit "F8"
            row = 12
        End If
    Wend
    hit "Enter"
    ProcessTargeting = check4text(23, 2, "01004 RECORD SUCCESSFULLY ADDED")
End Function
