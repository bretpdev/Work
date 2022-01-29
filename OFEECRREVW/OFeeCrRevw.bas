Attribute VB_Name = "OFeeCrRevw"
'Origination Fee Credit Reviewed
Dim UserID As String
Dim ftpFolder As String
Dim LogFolder As String
Dim Log As String
Dim Cnt As Integer
Dim tot As Double
Dim tot2 As Double
Dim BatchNum As String
'recovery
Dim rFile As String
Dim RStep As String
Dim rBatch As String
Dim RACC As String
Dim RLnSeq As String

Sub Main()
Dim str As String
Dim Arr() As String
'Dim SSN As String
'Dim LnSeq As String
Dim Seq(0) As Integer
'Dim FeeAmt As String
'Dim EffDate As String
Dim SAS2 As String
Dim Sas3 As String
If MsgBox("This is the Origination Fee Credit Reviewed script.", vbOKCancel, "Origination Fee Credit Reviewed") = vbCancel Then End
UserID = SP.Common.GetUserID
SP.Common.TestMode ftpFolder, , LogFolder
Log = LogFolder & "OFeeCrRevwLog.txt"
If Dir(Log) <> "" Then
    Open Log For Input As #1
        Input #1, rFile, RStep, rBatch, RACC, RLnSeq
        BatchNum = rBatch
    Close #1
End If
'Missing File
If Dir(ftpFolder & "ULWO09.LWO09R2*") = "" Then
    MsgBox "The file ULWO09.LWO09R2 is missing. Please contact Systems Support for assistance."
    End
End If
If Dir(ftpFolder & "ULWO09.LWO09R3*") = "" Then
    MsgBox "The file ULWO09.LWO09R3 is missing. Please contact Systems Support for assistance."
    End
End If
'<1>
'sp.Common.DeleteOldFilesReturnMostCurrent
'SAS2 = FTPFolder & Dir(FTPFolder & "ULWO09.LWO09R2*")
'SAS3 = FTPFolder & Dir(FTPFolder & "ULWO09.LWO09R3*")
SAS2 = ftpFolder & SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWO09.LWO09R2*")
Sas3 = ftpFolder & SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWO09.LWO09R3*")
'</1>
'File empty
If FileLen(SAS2) = 0 And FileLen(Sas3) = 0 Then
    If Dir(SAS2) <> "" Then Kill SAS2
    If Dir(Sas3) <> "" Then Kill Sas3
    MsgBox "There are no loans to process today. Process Complete!"
    End
End If


'R2
If FileLen(SAS2) <> 0 Then
    CreateReversal SAS2
    
End If

'R3
If FileLen(Sas3) <> 0 Then
    'create queue task
    If rFile = "" Or rFile = "R3" Then
        Open Sas3 For Input As #1
        Do While Not EOF(1)
            Line Input #1, str
            Arr = Split(str, ",")
            If Arr(0) <> "DF_SPE_ACC_ID" And RACC = "" Then
                Seq(0) = CInt(Arr(2))
                SP.Common.ATD22ByLoan TranslateACC(Arr(0)), "P1027", "Loan Seq = " & Arr(2) & " Original Origination Fee Amt = " & Arr(4) & " Orig Fee Eff Date = " & Arr(3), Seq, "OFeeCrRevw", UserID
                setRecovery "R3", "1", "", Arr(0), Arr(2)
            Else
                If Arr(0) = RACC And Arr(2) = RLnSeq Then
                    RACC = ""
                    RLnSeq = ""
                    RStep = ""
                    rFile = ""
                    rBatch = ""
                End If
            End If
        Loop
        Close #1
    End If
End If

'delete sas and log
If Dir(SAS2) <> "" Then Kill SAS2
If Dir(Sas3) <> "" Then Kill Sas3
If Dir(Log) <> "" Then Kill Log
MsgBox "Process Complete!"
End Sub



Sub CreateReversal(sas As String)
Dim DF_SPE_ACC_ID As String
    Dim DM_PRS_LST As String
    Dim LN_SEQ As String
    Dim EFF_DT_1027 As String
    Dim AMT_1027 As String
    Dim CR_AMT As String
    Dim str As String
    Dim Arr() As String
'<1>
    Dim postCnt As Integer
    Dim postTot As Double
    postCnt = 0
    postTot = 0
'</1>
    Cnt = 0
    tot = 0
    Open sas For Input As #1
    'get count and total
    Do While Not EOF(1)
        'DF_SPE_ACC_ID, DM_PRS_LST, LN_SEQ, EFF_DT_1027, AMT_1027, CR_AMT
        Line Input #1, str
        Arr = Split(str, ",")
        If Arr(0) <> "DF_SPE_ACC_ID" Then
'<1>
            If CDbl(Arr(5)) > 0 Then
                postCnt = postCnt + 1
                postTot = postTot + CDbl(Arr(4))
            End If
'</1>
            Cnt = Cnt + 1
            tot = tot + CDbl(Arr(4))
            tot2 = tot2 + CDbl(Arr(5))
            
        End If
    Loop
    Close #1
    
    If rFile = "R3" Then Exit Sub
    'create batch
    If RStep = "" Then
        SP.Q.FastPath "TX3Z/ATS1G;"
        SP.Q.puttext 6, 51, "3"
        SP.Q.puttext 10, 28, CStr(Cnt)
        SP.Q.puttext 11, 28, Format(Abs(tot), "0.00")
        SP.Q.puttext 14, 28, "6"
        SP.Q.puttext 15, 28, UserID, "ENTER"
        BatchNum = SP.Q.GetText(6, 18, 17)
        SP.Q.Hit "HOME"
        setRecovery "R2", "1", BatchNum, "", ""
    ElseIf RStep = "1" Then
        setRecovery "R2", "1", BatchNum, "", ""
    End If
    
    
    If RStep = "" Or RStep = "2" Then
        Open sas For Input As #1
        Do While Not EOF(1)
            'DF_SPE_ACC_ID, DM_PRS_LST, LN_SEQ, EFF_DT_1027, AMT_1027, CR_AMT
            Line Input #1, str
            Arr = Split(str, ",")
            If Arr(0) <> "DF_SPE_ACC_ID" And RACC = "" Then
                SP.Q.FastPath "TX3Z/ATS38" & BatchNum '& arr(0) & ";" & Format(arr(3), "MMDDYY")
                SP.Q.puttext 8, 32, Arr(0)
                SP.Q.puttext 10, 40, Format(Arr(3), "MMDDYY"), "ENTER"
                If SP.Q.Check4Text(1, 72, "TSX3D") Then
                    Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        Dim x As Integer
                        For x = 8 To 20
                            If SP.Q.Check4Text(x, 42, "10   27") And SP.Q.GetText(x, 20, 10) = Replace(Arr(4), "-", "") Then
                                SP.Q.puttext 21, 18, Format(SP.Q.GetText(x, 5, 2), "00"), "ENTER"
                                'if cdbl(Arr(4)) = 0 then sp.q.PutText
                                SP.Q.Hit "ENTER"
                                'Duplicate Handling
                                If SP.Q.Check4Text(23, 2, "05469 DUPLICATE REMITTANCE INFORMATION") Then
                                    SP.Q.Hit "F2"
                                    SP.Q.Hit "F4"
                                    SP.Q.puttext 7, 20, Arr(1)
                                    SP.Q.puttext 19, 2, "This is not a duplicate credit.", "ENTER"
                                    SP.Q.Hit "F12"
                                    SP.Q.Hit "ENTER"
                                End If
                                setRecovery "R2", "2", BatchNum, Arr(0), Arr(2)
                                Exit Do
                            End If
                        Next x
                        SP.Q.Hit "F8"
                    Loop
                ElseIf SP.Q.Check4Text(1, 72, "TSX3C") Then
                    SP.Q.Hit "ENTER"
                    If SP.Q.Check4Text(23, 2, "05469 DUPLICATE REMITTANCE INFORMATION") Then
                        SP.Q.Hit "F2"
                        SP.Q.Hit "F4"
                        SP.Q.puttext 7, 20, Arr(1)
                        SP.Q.puttext 19, 2, "This is not a duplicate credit.", "ENTER"
                        SP.Q.Hit "F12"
                        SP.Q.Hit "ENTER"
                    End If
                    setRecovery "R2", "2", BatchNum, Arr(0), Arr(2)
                End If
            Else
                If Arr(0) = RACC And Arr(2) = RLnSeq Then
                    RACC = ""
                    RLnSeq = ""
                    RStep = ""
                    rFile = ""
                    rBatch = ""
                End If
            End If
        Loop
        Close #1
    End If
    
    If RStep = "" Then
        'Verify Batch is in balance
        SP.Q.FastPath "TX3Z/CTS1R" & BatchNum & ";P"
        If SP.Q.GetText(10, 18, 7) <> SP.Q.GetText(9, 57, 7) Then
            MsgBox "The item count is off. Ending script."
            End
        ElseIf SP.Q.GetText(11, 27, 13) <> SP.Q.GetText(10, 66, 13) Then
            MsgBox "The totals are not balanced. Ending script."
            End
        Else
            'Release batch
            SP.Q.Hit "F10"
            If SP.Q.Check4Text(23, 2, "01172") = False Then
                MsgBox "The Batch was not successfully verified. Ending script."
                End
            End If
        End If
        setRecovery "R2", "3", BatchNum, "", ""
    ElseIf RStep = "3" Then
        setRecovery "R2", "3", BatchNum, "", ""
    End If
    
    CreateRePosting sas, postCnt, postTot
    
End Sub

Sub CreateRePosting(sas As String, Cnt As Integer, tot As Double)
    Dim str As String
    Dim Arr() As String
    If Cnt = 0 Then Exit Sub '<1>
    If rFile = "R3" Then Exit Sub
    
    If RStep = "" Then
        SP.Q.FastPath "TX3Z/ATS1G;"
        SP.Q.puttext 6, 51, "3"
        SP.Q.puttext 10, 28, CStr(Cnt)
        SP.Q.puttext 11, 28, Format(Abs(tot2), "0.00")
        SP.Q.puttext 12, 28, "27"
        SP.Q.puttext 15, 28, UserID, "ENTER"
        BatchNum = SP.Q.GetText(6, 18, 17)
        setRecovery "R2", "4", BatchNum, "", ""
    ElseIf RStep = "4" Then
        setRecovery "R2", "4", BatchNum, "", ""
    End If
    
    
    If RStep = "" Or RStep = "5" Then
        SP.Q.FastPath "TX3Z/ATS1D" & BatchNum
        Open sas For Input As #1
        Do While Not EOF(1)
            'DF_SPE_ACC_ID, DM_PRS_LST, LN_SEQ, EFF_DT_1027, AMT_1027, CR_AMT
            Line Input #1, str
            Arr = Split(str, ",")
            If Arr(0) <> "DF_SPE_ACC_ID" And RACC = "" Then
                AddRemittance Arr(0), Arr(1), Arr(2), Arr(5), Arr(3), "828476", "1027"
                setRecovery "R2", "5", BatchNum, Arr(0), Arr(2)
            Else
                If Arr(0) = RACC And Arr(2) = RLnSeq Then
                    RACC = ""
                    RLnSeq = ""
                    RStep = ""
                    rFile = ""
                    rBatch = ""
                End If
            End If
        Loop
        Close #1
    End If
    
    If RStep = "" Then
        'verify batch is in balance
        SP.Q.FastPath "TX3Z/CTS1R" & BatchNum & ";P"
        If SP.Q.GetText(10, 18, 7) <> SP.Q.GetText(9, 57, 7) Then
            MsgBox "The item count is off. Ending script."
            End
        ElseIf SP.Q.GetText(11, 27, 13) <> SP.Q.GetText(10, 66, 13) Then
            MsgBox "The totals are not balanced. Ending script."
            End
        Else
            'Release batch
            SP.Q.Hit "F10"
            If SP.Q.Check4Text(23, 2, "01172") = False Then
                MsgBox "The Batch was not successfully verified. Ending script."
                End
            End If
        End If
        setRecovery "R2", "6", BatchNum, "", ""
    ElseIf RStep = "6" Then
        setRecovery "R2", "6", BatchNum, "", ""
    End If
    
    If RStep = "" Or RStep = "7" Then
        'Add Comment
        Dim ln(0) As Integer
        Dim comment As String
        Open sas For Input As #1
        Do While Not EOF(1)
            'DF_SPE_ACC_ID, DM_PRS_LST, LN_SEQ, EFF_DT_1027, AMT_1027, CR_AMT
            Line Input #1, str
            Arr = Split(str, ",")
            If Arr(0) <> "DF_SPE_ACC_ID" And RACC = "" Then
                ln(0) = Arr(2)
'<1>
                'comment = "Payments recd after Orig BB applied. Rev Orig BB from loan seq " & Arr(2) & " effective " & Arr(3) & " for " & Arr(4) & ".  Reapplied " & Arr(5) & "."
                If CDbl(Arr(5)) > 0 Then
                    comment = "Payments recd after Orig BB applied. Rev Orig BB from loan seq " & Arr(2) & " effective " & Arr(3) & " for " & Arr(4) & ".  Reapplied " & Arr(5) & "."
                Else
                    comment = "Payments recd after Orig BB applied. Rev Orig BB from loan seq " & Arr(2) & " effective " & Arr(3) & " for " & Arr(4) & ".  Borrower is not eligible for Origination Fee Credit."
                End If
'</1>
                SP.Common.ATD22ByLoan TranslateACC(Arr(0)), "R1027", comment, ln, "OFeeCrRevw", UserID
                setRecovery "R2", "7", BatchNum, Arr(0), Arr(2)
            Else
                If Arr(0) = RACC And Arr(2) = RLnSeq Then
                    RACC = ""
                    RLnSeq = ""
                    RStep = ""
                    rFile = ""
                    rBatch = ""
                End If
            End If
        Loop
        Close #1
        
        SP.Common.SendMail "tle@utahsbr.edu,jbowthorpe@utahsbr.edu,mranson@utahsbr.edu", , "Origination Fee Credit Reviewed", "Total Count: " & Cnt & vbCrLf & "Total Amount: " & FormatCurrency(tot, 2), , , sas, , SP.Common.TestMode
        
    End If
End Sub

Sub AddRemittance(Acc As String, LName As String, ln As String, Amount As String, EffDt As String, InstID As String, Transmittal As String)
'<1>
    If CDbl(Amount) = 0 Then Exit Sub
'</1>
    Dim x As Integer
    Dim xx As Integer
    Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        For x = 8 To 19
            If SP.Q.Check4Text(x, 5, "__________") Then
                SP.Q.puttext x, 5, Acc
                SP.Q.puttext x, 17, Abs(Amount)
                SP.Q.puttext x, 30, Format(CDate(EffDt), "MMDDYY")
                SP.Q.puttext x, 48, InstID
                SP.Q.puttext x, 59, Transmittal
                SP.Q.Hit "ENTER"
                If SP.Q.Check4Text(23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED") = False Then
                    'Duplicate Error
                    If SP.Q.Check4Text(24, 26, "F4=COM") = False Then SP.Q.Hit "F2"
                    SP.Q.puttext 22, 17, Format(SP.Q.GetText(x, 2, 2), "00")
                    SP.Q.Hit "F4"
                    SP.Q.puttext 11, 17, LName
                    SP.Q.puttext 19, 2, "Not Duplicate benefit.", "ENTER"
                    SP.Q.Hit "F12"
                    SP.Q.Hit "ENTER"
                    If SP.Q.Check4Text(24, 26, "F4=COM") = False Then SP.Q.Hit "F2"
                    SP.Q.puttext 22, 17, Format(SP.Q.GetText(x, 2, 2), "00")
                    SP.Q.Hit "F6"
                    Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        For xx = 12 To 22
                            If SP.Q.Check4Text(xx, 50, Format(ln, "00")) Then
                                SP.Q.puttext xx, 3, "X", "ENTER"
                                SP.Q.Hit "F12"
                                Exit Do
                            End If
                        Next xx
                    Loop
'<1>
                Else
                    If SP.Q.Check4Text(24, 26, "F4=COM") = False Then SP.Q.Hit "F2"
                    SP.Q.puttext 22, 17, Format(SP.Q.GetText(x, 2, 2), "00")
                    SP.Q.Hit "F6"
                    Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        For xx = 12 To 22
                            If SP.Q.Check4Text(xx, 50, Format(ln, "00")) Then
                                SP.Q.puttext xx, 3, "X", "ENTER"
                                SP.Q.Hit "F12"
                                Exit Do
                            End If
                        Next xx
                        Hit "F8"
                    Loop
'</1>
                End If
                
                Exit Do
            End If
        Next x
        If SP.Q.Check4Text(24, 13, "SET2") = False Then SP.Q.Hit "F2" '<1>
        SP.Q.Hit "F8"
    Loop
End Sub

Function TranslateACC(tacc As String) As String
    SP.Q.FastPath "LP22I;;;"
    SP.Q.puttext 6, 33, tacc, "ENTER"
    TranslateACC = SP.Q.GetText(3, 23, 9)
End Function

Sub setRecovery(TRFile As String, TRStep As String, TRBatch As String, TRAcc As String, TRLnSeq As String)
    Open Log For Output As #50
        Write #50, TRFile, TRStep, TRBatch, TRAcc, TRLnSeq
    Close #50
    rFile = ""
    RStep = ""
    rBatch = ""
    RACC = ""
    RLnSeq = ""
End Sub

'<new> sr1923, tp, 12/12/06
'<1> sr 2065, tp, 04/13/07
