Attribute VB_Name = "Disqualif48OnTime"
Dim LogDir As String
Dim FTPDir As String
Dim RecoveryLevel As Integer
Dim RecoveryDataLine As String
Dim UID As String

Sub Main()
    Dim FileNameToProc As String
    Dim SASO35 As String
    Dim SASO39 As String
    Dim SASO46 As String
    Dim SASO48 As String
    Dim SASBBCleanUp As String
    Dim RecoveryLevelStr As String
    Dim TempDataLine As String
    
    'what does the script do
    If Not SP.Common.CalledByMBS Then If vbOK <> MsgBox("This script processes 48 on time payments disqualifications.  Click OK to continue or cancel to end the script.", vbOKCancel, "48 On Time Disqualification") Then End
    SP.Common.TestMode FTPDir, , LogDir
        
    'delete all log files
    While Dir(FTPDir & "ULWO35.LWO35R1*") <> ""
        Kill FTPDir & Dir(FTPDir & "ULWO35.LWO35R1*")
    Wend
    While Dir(FTPDir & "ULWO39.LWO39R1*") <> ""
        Kill FTPDir & Dir(FTPDir & "ULWO39.LWO39R1*")
    Wend
'<3->
    While Dir(FTPDir & "ULWO46.LWO46R1*") <> ""
        Kill FTPDir & Dir(FTPDir & "ULWO46.LWO46R1*")
    Wend
    While Dir(FTPDir & "ULWO48.LWO48R1*") <> ""
        Kill FTPDir & Dir(FTPDir & "ULWO48.LWO48R1*")
    Wend
'</3>

'<3->
'    'check if files exist
'    If Dir(FTPDir & "ULWO35.LWO35R2*") = "" Then
'        If Dir(FTPDir & "ULWO39.LWO39R2*") = "" Then
'            MsgBox "Both UTLWO35 and UTLWO39 files are missing.  Please contact Systems Support for assistance."
'        Else
'            MsgBox "No UTLWO35 file was found.  Please contact Systems Support for assistance."
'        End If
'        End
'    End If
'    If Dir(FTPDir & "ULWO39.LWO39R2*") = "" Then
'        MsgBox "No UTLWO39 file was found.  Please contact Systems Support for assistance."
'        End
'    End If
    'check if files exist
    If Dir(FTPDir & "ULWO35.LWO35R2*") = "" Or Dir(FTPDir & "ULWO39.LWO39R2*") = "" Or Dir(FTPDir & "ULWO46.LWO46R2*") = "" Or Dir(FTPDir & "ULWO48.LWO48R2*") = "" Then
        MsgBox "One or more of the following data files are missing:" & vbLf & vbLf & _
               "             -ULWO35.LWO35R2" & vbLf & _
               "             -ULWO39.LWO39R2" & vbLf & _
               "             -ULWO46.LWO46R2" & vbLf & _
               "             -ULWO48.LWO48R2" & vbLf & vbLf & _
               "Please contact Systems Support.", vbInformation
        End
    End If
'</3>
    
    'delete all old files and keep the most current
    SASO35 = DeleteOldFiles(FTPDir, "ULWO35.LWO35R2*")
    SASO39 = DeleteOldFiles(FTPDir, "ULWO39.LWO39R2*")
    SASO46 = DeleteOldFiles(FTPDir, "ULWO46.LWO46R2*")      '<3>
    SASO48 = DeleteOldFiles(FTPDir, "ULWO48.LWO48R2*")      '<3>
    If Dir(FTPDir & "BBCleanUp*") <> "" Then SASBBCleanUp = DeleteOldFiles(FTPDir, "BBCleanUp*")
    
    'check if the files are empty
    If Dir(FTPDir & "BBCleanUp*") <> "" Then
        If FileLen(FTPDir & SASO35) = 0 And FileLen(FTPDir & SASO39) = 0 And FileLen(FTPDir & SASBBCleanUp) = 0 And FileLen(FTPDir & SASO46) = 0 And FileLen(FTPDir & SASO48) = 0 Then
            MsgBox "Processing Complete.  The SAS processing files were empty."
            'delete empty files
            Kill FTPDir & SASO35
            Kill FTPDir & SASO39
            Kill FTPDir & SASO46        '<3>
            Kill FTPDir & SASO48        '<3>
            Kill FTPDir & SASBBCleanUp
            ProcComp "MBS48ONTMDISQ.TXT", False '<4>
            End
        End If
'<3>    ElseIf FileLen(FTPDir & SASO35) = 0 And FileLen(FTPDir & SASO39) = 0 Then
    ElseIf FileLen(FTPDir & SASO35) = 0 And FileLen(FTPDir & SASO39) = 0 And FileLen(FTPDir & SASO46) = 0 And FileLen(FTPDir & SASO48) = 0 Then       '<3>
        MsgBox "Processing Complete.  The SAS processing files were empty."
        'delete empty files
        Kill FTPDir & SASO35
        Kill FTPDir & SASO39
        Kill FTPDir & SASO46        '<3>
        Kill FTPDir & SASO48        '<3>
        ProcComp "MBS48ONTMDISQ.TXT", False '<4>
        End
    End If

    'check if the script is in recovery
    If Dir(LogDir & "OnTimeDisq48 Log.txt") <> "" Then
        'if file exists then retreive recover data
        Open LogDir & "OnTimeDisq48 Log.txt" For Input As #1
        Line Input #1, RecoveryDataLine
        Line Input #1, RecoveryLevelStr
        RecoveryLevel = CInt(RecoveryLevelStr)
        Close #1
    Else
        RecoveryDataLine = ""
        RecoveryLevel = 1
    End If
    
    'get user id
    UID = SP.Common.GetUserID()
    
    'cycle each SSN in the file through the disqualification process
    If RecoveryLevel = 1 Then
'<6>        If FileLen(FTPDir & SASO35) <> 0 Then CallProcOn SASO35
        If FileLen(FTPDir & SASO35) <> 0 Then CallProcOn SASO35, "U48"  '<6>
        RecoveryLevel = 2
    End If
    If RecoveryLevel = 2 Then
'<6>        If FileLen(FTPDir & SASO39) <> 0 Then CallProcOn SASO39
        If FileLen(FTPDir & SASO39) <> 0 Then CallProcOn SASO39, "U48", , True '<6>
        RecoveryLevel = 3
    End If
'<3->
    If RecoveryLevel = 3 Then
'<6>        If FileLen(FTPDir & SASO46) <> 0 Then CallProcOn SASO46
        If FileLen(FTPDir & SASO46) <> 0 Then CallProcOn SASO46, "N48"  '<6>
        RecoveryLevel = 4
    End If
    If RecoveryLevel = 4 Then
'<6>        If FileLen(FTPDir & SASO48) <> 0 Then CallProcOn SASO48, "U36", True
        If FileLen(FTPDir & SASO48) <> 0 Then CallProcOn SASO48, "U36", True  '<6>
        RecoveryLevel = 5
    End If
    If RecoveryLevel = 5 Then
        If Dir(FTPDir & "BBCleanUp*") <> "" Then If FileLen(FTPDir & SASBBCleanUp) <> 0 Then CallProcOn SASBBCleanUp
        RecoveryLevel = 6
    End If
    
    'add activity records
    If RecoveryLevel = 6 Then
        'process files
        If FileLen(FTPDir & SASO35) <> 0 Then
            Open FTPDir & SASO35 For Input As #1
            EnterTD22Comments RecoveryDataLine, "48 ON TIME PAYMENT DISQUALIFICATION - LOAN MORE THAN 30 DAYS PAST DUE", False, True
            Close #1
        End If
        RecoveryLevel = 7
    End If
    If RecoveryLevel = 7 Then
        If FileLen(FTPDir & SASO39) <> 0 Then
            Open FTPDir & SASO39 For Input As #1
            EnterTD22Comments RecoveryDataLine, "48 ON TIME PAYMENT DISQUALIFICATION - LOAN MORE THAN 15 DAYS PAST DUE 3 DIFFERENT TIMES", True, True
            Close #1
        End If
        RecoveryLevel = 8
    End If
    If RecoveryLevel = 8 Then
        If FileLen(FTPDir & SASO46) <> 0 Then
            Open FTPDir & SASO46 For Input As #1
            EnterTD22Comments RecoveryDataLine, "48 ON TIME PAYMENT DISQUALIFICATION - Lns 1st Disb on or after 5/1/06 file - LOAN MORE THAN 15 DAYS PAST DUE", True, True
            Close #1
        End If
        RecoveryLevel = 9
    End If
    If RecoveryLevel = 9 Then
        If FileLen(FTPDir & SASO48) <> 0 Then
            Open FTPDir & SASO48 For Input As #1
            EnterTD22Comments RecoveryDataLine, "36 On Time Payment Disqualification - loan more than 15 days past due", True, True, True
            Close #1
        End If
        RecoveryLevel = 10
    End If
    If RecoveryLevel = 10 Then
        If Dir(FTPDir & "BBCleanUp*") <> "" Then
            If FileLen(FTPDir & SASBBCleanUp) <> 0 Then
                Open FTPDir & SASBBCleanUp For Input As #1
                EnterTD22Comments RecoveryDataLine, "48 ON TIME PAYMENT DISQUALIFICATION - LOAN MORE THAN 15 DAYS PAST DUE 3 DIFFERENT TIMES", True, False
                Close #1
            End If
        End If
        RecoveryLevel = 11
    End If
    
'    If RecoveryLevel = 3 Then
'        If Dir(FTPDir & "BBCleanUp*") <> "" Then If FileLen(FTPDir & SASBBCleanUp) <> 0 Then CallProcOn SASBBCleanUp
'        RecoveryLevel = 4
'    End If

    
'    'add activity records
'    If RecoveryLevel = 4 Then
'        'process files
'        If FileLen(FTPDir & SASO35) <> 0 Then
'            Open FTPDir & SASO35 For Input As #1
'            EnterTD22Comments RecoveryDataLine, "48 ON TIME PAYMENT DISQUALIFICATION - LOAN MORE THAN 30 DAYS PAST DUE", False, True
'            Close #1
'        End If
'        RecoveryLevel = 5
'    End If
'    If RecoveryLevel = 5 Then
'        If FileLen(FTPDir & SASO39) <> 0 Then
'            Open FTPDir & SASO39 For Input As #1
'            EnterTD22Comments RecoveryDataLine, "48 ON TIME PAYMENT DISQUALIFICATION - LOAN MORE THAN 15 DAYS PAST DUE 3 DIFFERENT TIMES", True, True
'            Close #1
'        End If
'        RecoveryLevel = 6
'    End If
'    If RecoveryLevel = 6 Then
'        If Dir(FTPDir & "BBCleanUp*") <> "" Then
'            If FileLen(FTPDir & SASBBCleanUp) <> 0 Then
'                Open FTPDir & SASBBCleanUp For Input As #1
'                EnterTD22Comments RecoveryDataLine, "48 ON TIME PAYMENT DISQUALIFICATION - LOAN MORE THAN 15 DAYS PAST DUE 3 DIFFERENT TIMES", True, False
'                Close #1
'            End If
'        End If
'        RecoveryLevel = 7
'    End If
'</3>

    Kill LogDir & "OnTimeDisq48 Log.txt" 'delete log file

    'delete processed files
    If Dir(FTPDir & "BBCleanUp*") <> "" Then
        Kill FTPDir & SASO35
        Kill FTPDir & SASO39
        Kill FTPDir & SASO46        '<3>
        Kill FTPDir & SASO48        '<3>
        Kill FTPDir & SASBBCleanUp
    Else
        Kill FTPDir & SASO35
        Kill FTPDir & SASO39
        Kill FTPDir & SASO46        '<3>
        Kill FTPDir & SASO48        '<3>
    End If
    If Not SP.Common.CalledByMBS Then MsgBox "Processing complete", vbInformation
    
End Sub

'process each files through disqualification process
Sub CallProcOn(FileNameToProc As String, Optional BBP As String = "", Optional Comment4U36 As Boolean = False, Optional ThreeDateFileStruc As Boolean = False)
    Open FTPDir & FileNameToProc For Input As #1
    'find the last line that was processed successfully if in recovery mode
    If RecoveryDataLine <> "" Then
        While Not EOF(1) And RecoveryDataLine <> TempDataLine
            Line Input #1, TempDataLine
        Wend
        'close the file and exit the sub if the recovery line was not found in the file
        If EOF(1) Then
            Close #1
            If RecoveryDataLine = TempDataLine Then RecoveryDataLine = ""
            Exit Sub
        Else
            RecoveryDataLine = ""
        End If
    End If
'<6>    ProcOnTS5O FileNameToProc, Comment4U36
    ProcOnCTSDS FileNameToProc, Comment4U36, BBP, ThreeDateFileStruc
    Close #1
End Sub

'enter activity records
Sub EnterTD22Comments(RecoveryDataLine As String, comment As String, AddBills As Boolean, SendLetter As Boolean, Optional ARC4U36 As Boolean = False)
    Dim DL As String
    Dim DFs() As String
    Dim TempDFs() As String
    Dim lSSN As String
    Dim LDF2 As String
    Dim LnSeq() As Integer
    Dim Cmt As String

    ReDim LnSeq(0) 'initialize array
    'check if the script needs to recover
    If RecoveryDataLine <> "" Then
        ReDim TempDFs(2) 'initialize temp array
        'find the last SSN processed successfully
        While Not EOF(1) And RecoveryDataLine <> TempDFs(0)
            Line Input #1, TempDataLine
            TempDFs = Split(TempDataLine, ",")
        Wend
        ReDim DFs(2) 'initialize processing array
        'find the beginning of the next SSN which still needs processing
        If Not EOF(1) Then
            Line Input #1, DL
            DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification/bills
        End If
        While Not EOF(1) And DFs(0) = RecoveryDataLine
            Line Input #1, DL
            DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification/bills
        Wend
    End If
    
    If EOF(1) Then
        If RecoveryDataLine = TempDFs(0) Then RecoveryDataLine = ""
        Exit Sub
    End If
    
    While Not EOF(1)
        If RecoveryDataLine = "" Then 'input data only if not in recovery or if the script has already recovered
            Line Input #1, DL
            DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification/bills
        Else
            RecoveryDataLine = "" 'mark variable that script has recovered
        End If
        'prime lSSN
        If lSSN = "" Then
            lSSN = DFs(0)
            LDF2 = DFs(2)
        End If
        If lSSN = DFs(0) Then 'if same ssn is still being processed
            'copy loan seq into array
            LnSeq(UBound(LnSeq)) = DFs(1)
            ReDim Preserve LnSeq(UBound(LnSeq) + 1) 'create another opening
        Else 'if different SSN is encountered
            'add comments
            If AddBills Then Cmt = comment & ": " & LDF2 Else Cmt = comment
            If ARC4U36 = False Then
                If SP.Common.ATD22ByLoan(lSSN, "U48MD", Cmt, LnSeq, "48ONTMDISQ", UID) = False Then
                    MsgBox "You need access to the ""U48MD"" ARC.  Please contact Systems Support."
                    End
                End If
            Else
                If SP.Common.ATD22ByLoan(lSSN, "U36MD", Cmt, LnSeq, "48ONTMDISQ", UID) = False Then
                    MsgBox "You need access to the ""U36MD"" ARC.  Please contact Systems Support."
                    End
                End If
            End If
            'add an ARC to send a disqualification letter for all but clean up file
            If SendLetter Then
                If ARC4U36 = False Then
                    If SP.Common.ATD22ByLoan(lSSN, "U48IP", "", LnSeq, "48ONTMDISQ", UID) = False Then
                        'borrower is in a death, disability, bankruptcy status
                        If SP.Q.Check4Text(23, 2, "03483") Then
                            'bypass loan because 03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST
                        Else
                            MsgBox "You need access to the ""U48IP"" ARC.  Please contact Systems Support."
                            End
                        End If
    
                    End If
                Else
                    If SP.Common.ATD22ByLoan(lSSN, "U36IP", "", LnSeq, "48ONTMDISQ", UID) = False Then
                        'borrower is in a death, disability, bankruptcy status
                        If SP.Q.Check4Text(23, 2, "03483") Then
                            'bypass loan because 03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST
                        Else
                            MsgBox "You need access to the ""U36IP"" ARC.  Please contact Systems Support."
                            End
                        End If
    
                    End If
                End If
            End If
            'update recovery log file
            Open LogDir & "OnTimeDisq48 Log.txt" For Output As #2
            Print #2, lSSN
            Print #2, RecoveryLevel
            Close #2
            'reinitialize variables
            lSSN = DFs(0)
            LDF2 = DFs(2)
            ReDim LnSeq(0) 'whipe array clean
            LnSeq(UBound(LnSeq)) = DFs(1)
            ReDim Preserve LnSeq(UBound(LnSeq) + 1) 'create another opening
        End If
    Wend
    
    'do last comment
    If AddBills Then Cmt = comment & ": " & DFs(2) Else Cmt = comment
    If ARC4U36 = False Then
        If SP.Common.ATD22ByLoan(lSSN, "U48MD", Cmt, LnSeq, "48ONTMDISQ", UID) = False Then
            MsgBox "You need access to the ""U48MD"" ARC.  Please contact Systems Support."
            End
        End If
    Else
        If SP.Common.ATD22ByLoan(lSSN, "U36MD", Cmt, LnSeq, "48ONTMDISQ", UID) = False Then
            MsgBox "You need access to the ""U36MD"" ARC.  Please contact Systems Support."
            End
        End If
    End If
    If ARC4U36 = False Then
        If SP.Common.ATD22ByLoan(lSSN, "U48IP", "", LnSeq, "48ONTMDISQ", UID) = False Then
            'borrower is in a death, disability, bankruptcy status
            If SP.Q.Check4Text(23, 2, "03483") Then
                'bypass loan because 03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST
            Else
                MsgBox "You need access to the ""U48IP"" ARC.  Please contact Systems Support."
                End
            End If

        End If
    Else
        If SP.Common.ATD22ByLoan(lSSN, "U36IP", "", LnSeq, "48ONTMDISQ", UID) = False Then
            'borrower is in a death, disability, bankruptcy status
            If SP.Q.Check4Text(23, 2, "03483") Then
                'bypass loan because 03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST
            Else
                MsgBox "You need access to the ""U36IP"" ARC.  Please contact Systems Support."
                End
            End If

        End If
    End If
    
    'update recovery log file
    Open LogDir & "OnTimeDisq48 Log.txt" For Output As #2
    Print #2, lSSN
    Print #2, RecoveryLevel
    Close #2
End Sub

'this function does the disqualification process on TS5O
'<6>Sub ProcOnTS5O(SASFile As String, Comment4U36 As Boolean, Optional BBP As String = "")
Sub ProcOnCTSDS(SasFile As String, Comment4U36 As Boolean, Optional BBP As String = "", Optional ThreeDateFileStruc As Boolean = False) '<6>
    Dim DL As String
    Dim DFs() As String
    Dim row As Integer
    Dim found As Boolean
    
'<6->
'    While Not EOF(1)
'        Line Input #1, DL
'        DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification
'        fastpath "TX3ZCTS5O" & DFs(0) 'access TS5O for SSN
'        'check selection screen is displayed
'        If check4text(1, 72, "TSX3S") Then
'            'selection screen
'            row = 7
'            found = False
'            While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False And found = False
'                If check4text(row, 20, Format(DFs(1), "000#")) Then
'                    'select option for seq number
'                    puttext 22, 19, GetText(row, 3, 2), "Enter"
'                    found = True
'                Else
'                    row = row + 1
'                    'check for data
'                    If check4text(row, 4, " ") Then
'                        row = 7
'                        Hit "F8"
'                    End If
'                End If
'            Wend
'        End If
'
'        'Target screen
'        If Comment4U36 = False Then
'            If check4text(15, 21, "U48") = False Then puttext 15, 21, "U48" 'RIR program
'        Else
'            If check4text(15, 21, "U36") = False Then puttext 15, 21, "U36" 'RIR program
'        End If
'        If check4text(15, 49, "N") = False Then puttext 15, 49, "N" 'Secondary eligibility field
'        puttext 15, 73, "X", "Enter" 'RIR Eligibility
'
'        'enter comments
'        If SP.q.check4text(23, 2, "03924") Then 'PRESS F6 TO ENTER COMMENTS
'            Hit "F6"
'            If Comment4U36 = False Then
'                '48 on time payments
'                puttext 13, 2, "Borr Disqual - 48 On Time Payments", "Enter"
'            Else
'                '36 on time payments
'                puttext 13, 2, "Borr Disqual - 36 On Time Payments", "Enter"
'            End If
'        'send error message if error other than changes already processed
'        ElseIf Not SP.q.check4text(23, 2, "01003") Then 'NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED
'            SendEMail DFs(0) & " " & GetText(23, 2, 70) & " " & SASFile
'        End If
'        'Target screen
'        If Comment4U36 = False Then
'            If check4text(15, 21, "U48") = False Then puttext 15, 21, "U48" 'RIR program
'        Else
'            If check4text(15, 21, "U36") = False Then puttext 15, 21, "U36" 'RIR program
'        End If
'        If check4text(15, 49, "N") = False Then puttext 15, 49, "N" 'Secondary eligibility field
'        puttext 15, 73, "X", "Enter" 'RIR Eligibility
'
'        'enter comments
'        If SP.q.check4text(23, 2, "03924") Then 'PRESS F6 TO ENTER COMMENTS
'            Hit "F6"
'            If Comment4U36 = False Then
'                '48 on time payments
'                puttext 13, 2, "Borr Disqual - 48 On Time Payments", "Enter"
'            Else
'                '36 on time payments
'                puttext 13, 2, "Borr Disqual - 36 On Time Payments", "Enter"
'            End If
'        'send error message if error other than changes already processed
'        ElseIf Not SP.q.check4text(23, 2, "01003") Then 'NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED
'            SendEMail DFs(0) & " " & GetText(23, 2, 70) & " " & SASFile
'        End If

    While Not EOF(1)
        Line Input #1, DL
        DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification
        FastPath "TX3ZCTSDS" & DFs(0) & ";" & BBP 'access CTSDS for SSN
        'check selection screen is displayed
        If Check4Text(1, 75, "TSXDU") Then
            'selection screen
            row = 8
            found = False
            While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False And found = False
                If Check4Text(row, 8, Format(DFs(1), "00#")) Then
                    'select option for seq number
                    puttext 21, 18, GetText(row, 4, 3), "Enter"
                    found = True
                Else
                    row = row + 1
                    'check for data
                    If Check4Text(row, 5, " ") Then
                        row = 8
                        Hit "F8"
                    End If
                End If
            Wend
        End If

        'Target screen
        If Check4Text(11, 16, "Y") Then puttext 11, 16, "X" 'Eligibility
        If ThreeDateFileStruc = False Then
            puttext 13, 16, Format(CDate(Format(Replace(DFs(2), "/", ""), "##/##/####")), "MMDDYY") 'disqual date
        Else
            puttext 13, 16, Format(CDate(Format(Replace(Split(DFs(2), " ")(2), "/", ""), "##/##/####")), "MMDDYY") 'disqual date
        End If
        puttext 13, 44, "02", "Enter"
        Hit "F6"
        
        'send error message if error other than changes already processed
        If BBP = "U48" Or BBP = "N48" Then
            puttext 21, 11, "U48 On Time Manual Disqualification", "F6"
        ElseIf BBP = "U36" Then
            puttext 21, 11, "U36 On Time Manual Disqualification", "F6"
        End If
        'send error message if error other than changes already processed
        If SP.Q.Check4Text(23, 2, "01003") Then 'NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED
            SendEMail DFs(0) & " " & GetText(23, 2, 70) & " " & SasFile
        End If
'</6>
        
        'update recovery log file
        Open LogDir & "OnTimeDisq48 Log.txt" For Output As #2
        Print #2, DL
        Print #2, RecoveryLevel
        Close #2
    Wend
End Sub

Function DeleteOldFiles(FTPDir As String, Filename As String) As String
    Dim MostCurrentSASFile As String
    Dim TempSASFile As String
    Dim MCSFTDS As Date
    Dim TempFTDS As Date
    MostCurrentSASFile = Dir(FTPDir & Filename)
    MCSFTDS = FileDateTime(FTPDir & MostCurrentSASFile)
    TempSASFile = Dir()
    If TempSASFile <> "" Then TempFTDS = FileDateTime(FTPDir & TempSASFile)
    'cycle through all files with specified file name in the directory
    While TempSASFile <> ""
        'check which file is the most current
        If TempFTDS > MCSFTDS Then
            'delete older file
            Kill FTPDir & MostCurrentSASFile
            'note the more current file
            MostCurrentSASFile = TempSASFile
            MCSFTDS = TempFTDS
        Else
            'temp file is oldest and therefore can be deleted
            Kill FTPDir & TempSASFile
        End If
        TempSASFile = Dir()
        If TempSASFile <> "" Then TempFTDS = FileDateTime(FTPDir & TempSASFile)
    Wend
    DeleteOldFiles = MostCurrentSASFile
End Function

Function SendEMail(MsgText As String)
'<5->
'''Function SendEMail()
'''Dim MsgText As String
'''MsgText = "This is a test"
'Dim GW As New GroupwareTypeLibrary.Application 'create application ref
'Dim GWAccount As GroupwareTypeLibrary.Account2 'create account ref
'Dim EmailMsg As GroupwareTypeLibrary.Mail 'create message
'
'    Set GWAccount = GW.Login 'login to the current users account to use
'    Set EmailMsg = GWAccount.MailBox.Messages.Add
'
'    EmailMsg.Subject.PlainText = "48 On Time Disqualification"
'
'    If SP.TestMode Then
'        EmailMsg.BodyText.PlainText = MsgText & " (THIS IS ONLY A TEST)"
'        EmailMsg.Recipients.Add "jdavis@utahsbr.edu"
'    Else
'        EmailMsg.BodyText.PlainText = MsgText
'    End If
'    EmailMsg.Recipients.Add "tle@utahsbr.edu"
'    EmailMsg.Recipients.Add "bkenney@utahsbr.edu"
'
'    EmailMsg.Send
    
    SP.Common.SendMail "dphillips@utahsbr.edu,bkenney@utahsbr.edu", , "48 On Time Disqualification", MsgText
'</5>
End Function

'<2-> script rewritten to add two new files
'Dim LogDir As String
'Dim FTPDir As String
'
'Sub Main()
'    Dim FileNameToProc As String
'    Dim RecoveryDataLine As String
'    Dim RecoveryLevel As Integer
'    Dim RecoveryLevelStr As String
'    Dim TempDataLine As String
'    'what does the script do
'    If vbOK <> MsgBox("This script processes 48 on time payments disqualifications.  Click OK to continue or cancel to end the script.", vbOKCancel, "48 On Time Disqualification") Then End
'    SP.Common.TestMode FTPDir, , LogDir
'    'delete all log files
'    While Dir(FTPDir & "ULWO35.LWO35R1*") <> ""
'        Kill FTPDir & Dir(FTPDir & "ULWO35.LWO35R1*")
'    Wend
'    'check if file exists
'    If Dir(FTPDir & "ULWO35.LWO35R2*") = "" Then
'        MsgBox "No file was found.  Please contact Systems Support for assistance."
'        End
'    End If
'    'delete all old files and keep the most current
'    FileNameToProc = DeleteOldFiles(FTPDir)
'    'check if the file is empty
'    If FileLen(FTPDir & FileNameToProc) = 0 Then
'        MsgBox "Processing Complete.  The SAS processing file was empty."
'        Kill FTPDir & FileNameToProc 'delete empty file
'        End
'    End If
'    'check if the script is in recovery
'    If Dir(LogDir & "OnTimeDisq48 Log.txt") <> "" Then
'        'if file exists then retreive recover data
'        Open LogDir & "OnTimeDisq48 Log.txt" For Input As #1
'        Line Input #1, RecoveryDataLine
'        Line Input #1, RecoveryLevelStr
'        RecoveryLevel = CInt(RecoveryLevelStr)
'        Close #1
'    Else
'        RecoveryDataLine = ""
'        RecoveryLevel = 1
'    End If
'    'cycle each SSN in the file through the disqualification process
'    If RecoveryLevel = 1 Then
'        Open FTPDir & FileNameToProc For Input As #1
'        'find the last line that was processed successfully if in recovery mode
'        If RecoveryDataLine <> "" Then
'            While Not EOF(1) And RecoveryDataLine <> TempDataLine
'                Line Input #1, TempDataLine
'            Wend
'        End If
'        ProcOnTS5O
'        Close #1
'        RecoveryLevel = 2
'        RecoveryDataLine = "" 'mark variable that script has already recovered
'    End If
'    If RecoveryLevel > 1 Then
'        'TD22Comments
'        Open FTPDir & FileNameToProc For Input As #1
'        EnterTD22Comments RecoveryDataLine
'        Close #1
'    End If
'    Kill LogDir & "OnTimeDisq48 Log.txt" 'delete log file
'    Kill FTPDir & FileNameToProc 'delete processed file
'    MsgBox "Processing complete", vbInformation
'End Sub
'
'Sub EnterTD22Comments(RecoveryDataLine As String)
'    Dim DL As String
'    Dim DFs() As String
'    Dim TempDFs() As String
'    Dim LSSN As String
'    Dim LnSeq() As Integer
'    Dim UID As String
'    UID = SP.Common.GetUserID() 'get users ID
'    ReDim LnSeq(0) 'initialize array
'    'check if the script needs to recover
'    If RecoveryDataLine <> "" Then
'        ReDim TempDFs(2) 'initialize temp array
'        'find the last SSN processed successfully
'        While Not EOF(1) And RecoveryDataLine <> TempDFs(0)
'            Line Input #1, TempDataLine
'            TempDFs = Split(TempDataLine, ",")
'        Wend
'        ReDim DFs(2) 'initialize processing array
'        'find the beginning of the next SSN which still needs processing
'        If Not EOF(1) Then
'            Line Input #1, DL
'            DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification
'        End If
'        While Not EOF(1) And DFs(0) = RecoveryDataLine
'            Line Input #1, DL
'            DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification
'        Wend
'    End If
'    While Not EOF(1)
'        If RecoveryDataLine = "" Then 'input data only if not in recovery or if the script has already recovered
'            Line Input #1, DL
'            DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification
'        Else
'            RecoveryDataLine = "" 'mark variable that script has recovered
'        End If
'        'prime lSSN
'        If LSSN = "" Then
'            LSSN = DFs(0)
'        End If
'        If LSSN = DFs(0) Then 'if same ssn is still being processed
'            'copy loan seq into array
'            LnSeq(UBound(LnSeq)) = DFs(1)
'            ReDim Preserve LnSeq(UBound(LnSeq) + 1) 'create another opening
'        Else 'if different SSN is encountered
'            'add comments
'
'            If SP.Common.ATD22ByLoan(LSSN, "U48MD", "48 ON TIME PAYMENT DISQUALIFICATION - LOAN MORE THAN 30 DAYS PAST DUE", LnSeq, "48ONTMDISQ", UID) = False Then
'                MsgBox "You need access to the ""U48MD"" ARC.  Please contact Systems Support."
'                End
'            End If
''<1>
'                'borrower is in a death, disability, bankruptcy status
'                If SP.Common.ATD22ByLoan(LSSN, "U48IP", "", LnSeq, "48ONTMDISQ", UID) = False Then
'                    If SP.Q.check4text(23, 2, "03483") Then
'                        'bypass loan because 03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST
'                    Else
'                        MsgBox "You need access to the ""U48IP"" ARC.  Please contact Systems Support."
'                        End
'                    End If
'
'                End If
'                'update recovery log file
'                Open LogDir & "OnTimeDisq48 Log.txt" For Output As #2
'                Print #2, LSSN
'                Print #2, "2"
'                Close #2
'                'reinitialize variables
'                LSSN = DFs(0)
'                ReDim LnSeq(0) 'whipe array clean
'                LnSeq(UBound(LnSeq)) = DFs(1)
'                ReDim Preserve LnSeq(UBound(LnSeq) + 1) 'create another opening
'
'
''            If SP.Common.ATD22ByLoan(LSSN, "U48IP", "", LnSeq, "48ONTMDISQ", UID) = False Then
''                MsgBox "You need access to the ""U48IP"" ARC.  Please contact Systems Support."
''                End
''            End If
''            'update recovery log file
''            Open LogDir & "OnTimeDisq48 Log.txt" For Output As #2
''            Print #2, LSSN
''            Print #2, "2"
''            Close #2
''            'reinitialize variables
''            LSSN = DFs(0)
''            ReDim LnSeq(0) 'whipe array clean
''            LnSeq(UBound(LnSeq)) = DFs(1)
''            ReDim Preserve LnSeq(UBound(LnSeq) + 1) 'create another opening
'        End If
''</1>
'    Wend
'    'do last comment
'    'add comments
'    If SP.Common.ATD22ByLoan(LSSN, "U48MD", "48 ON TIME PAYMENT DISQUALIFICATION - LOAN MORE THAN 30 DAYS PAST DUE", LnSeq, "48ONTMDISQ", UID) = False Then
'        MsgBox "You need access to the ""U48MD"" ARC.  Please contact Systems Support."
'        End
'    End If
'    If SP.Common.ATD22ByLoan(LSSN, "U48IP", "", LnSeq, "48ONTMDISQ", UID) = False Then
''<1>
''        MsgBox "You need access to the ""U48IP"" ARC.  Please contact Systems Support."
''        End
'        If SP.Q.check4text(23, 2, "03483") Then
'            'bypass loan because 03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST
'        Else
'            MsgBox "You need access to the ""U48IP"" ARC.  Please contact Systems Support."
'            End
'        End If
''</1>
'    End If
'    'update recovery log file
'    Open LogDir & "OnTimeDisq48 Log.txt" For Output As #2
'    Print #2, LSSN
'    Print #2, "2"
'    Close #2
'End Sub
'
''this function does the disqualification process on TS5O
'Sub ProcOnTS5O()
'    Dim DL As String
'    Dim DFs() As String
'    Dim row As Integer
'    Dim Found As Boolean
'    While Not EOF(1)
'        Line Input #1, DL
'        DFs = Split(DL, ",") 'indicies: 0 = SSN, 1 = LnSeq, 2 = Date Of Disqualification
'        FastPath "TX3ZCTS5O" & DFs(0) 'access TS5O for SSN
'        'check selection screen is displayed
'        If check4text(1, 72, "TSX3S") Then
'            'selection screen
'            row = 7
'            Found = False
'            While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False And Found = False
'                If check4text(row, 20, Format(DFs(1), "000#")) Then
'                    'select option for seq number
'                    PutText 22, 19, GetText(row, 3, 2), "Enter"
'                    Found = True
'                Else
'                    row = row + 1
'                    'check for data
'                    If check4text(row, 4, " ") Then
'                        row = 7
'                        Hit "F8"
'                    End If
'                End If
'            Wend
'        End If
'        'Target screen
'        If check4text(15, 21, "U48") = False Then PutText 15, 21, "U48" 'RIR program
'        If check4text(15, 49, "N") = False Then PutText 15, 49, "N" 'Secondary eligibility field
'        PutText 15, 73, "X", "Enter" 'RIR Eligibility
'        'add comments
''<1>
'        'bypass record if it has already been changed.
'        If SP.Q.check4text(23, 2, "01003") = False Then '01003 NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED
'            Hit "F6"
'            PutText 13, 2, "Borr Disqual - 48 On Time Payments", "Enter"
'        End If
''            Hit "F6"
''</1>        PutText 13, 2, "Borr Disqual - 48 On Time Payments", "Enter"
'        'update recovery log file
'        Open LogDir & "OnTimeDisq48 Log.txt" For Output As #2
'        Print #2, DL
'        Print #2, "1"
'        Close #2
'    Wend
'End Sub
'
'Function DeleteOldFiles(FTPDir As String) As String
'    Dim MostCurrentSASFile As String
'    Dim TempSASFile As String
'    Dim MCSFTDS As Date
'    Dim TempFTDS As Date
'    MostCurrentSASFile = Dir(FTPDir & "ULWO35.LWO35R2*")
'    MCSFTDS = FileDateTime(FTPDir & MostCurrentSASFile)
'    TempSASFile = Dir()
'    If TempSASFile <> "" Then TempFTDS = FileDateTime(FTPDir & TempSASFile)
'    'cycle through all files with specified file name in the directory
'    While TempSASFile <> ""
'        'check which file is the most current
'        If TempFTDS > MCSFTDS Then
'            'delete older file
'            Kill FTPDir & MostCurrentSASFile
'            'note the more current file
'            MostCurrentSASFile = TempSASFile
'            MCSFTDS = TempFTDS
'        Else
'            'temp file is oldest and therefore can be deleted
'            Kill FTPDir & TempSASFile
'        End If
'        TempSASFile = Dir()
'        If TempSASFile <> "" Then TempFTDS = FileDateTime(FTPDir & TempSASFile)
'    Wend
'    DeleteOldFiles = MostCurrentSASFile
'End Function
'</2>


'new sr1229, aa
'<1> sr1335, tp, 11/22/05, 12/08/05
'<2> sr1373, jd
'<3> sr1498, aa
'<4> sr1533/1660, tp Removed prompts to prepare script for MBS
'<5> sr1768, jd
'<6> sr1862, aa
'<7> sr2005, aa


