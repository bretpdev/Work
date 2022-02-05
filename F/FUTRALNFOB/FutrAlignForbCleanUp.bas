Attribute VB_Name = "FutrAlignForbCleanUp"
Sub Main()
    Dim SASDir As String
    Dim LogDir As String
    Dim DocDir As String
    Dim Rec As String
    Dim Flds() As String
    Dim TSX0GRecs2Skip As Integer
    Dim SkipRemainingProc As Boolean
    Dim BreakOutOfLoop As Boolean
    Dim LnSeq(1) As Integer
    Dim UID As String
    Dim rPhase As String
    Dim RRec As String
    DocDir = "X:\PADD\AccountServices\"
    SP.Common.TestMode SASDir, DocDir, LogDir
    If MsgBox("This script automates the removal of invalid alignment forbearances with a start and end date in the future.  Click OK to continue.", vbOKCancel) = vbCancel Then End
    'check 4 recovery
    If Dir(LogDir & "FutrAlignForbCleanUp Log.txt") = "" Then
        'not in recovery
        'this script only processes the R5 file for the SAS job
        If Dir(SASDir & "AlignRepayForbCU.R5*") = "" Then
            MsgBox "The SAS processing file couldn't be found.  Please contact Systems Support.", vbOKOnly
            End
        ElseIf FileLen(SASDir & Dir(SASDir & "AlignRepayForbCU.R5*")) = 0 Then
            MsgBox "The SAS processing file had no data in it.  Please contact Systems Support.", vbOKOnly
            End
        End If
        'delete error log if one exists
        If Dir("T:\FutrAlignForbCleanUp Error Log.txt") <> "" Then Kill "T:\FutrAlignForbCleanUp Error Log.txt"
    Else
        'in recovery
        Open LogDir & "FutrAlignForbCleanUp Log.txt" For Input As #3
        Line Input #3, rPhase
        Line Input #3, RRec
        Close #3
    End If
    'check for recovery phase
    If rPhase = "" Or rPhase = "1" Then
        'open SAS file for processing
        Open SASDir & Dir(SASDir & "AlignRepayForbCU.R5*") For Input As #1
        'get header row
        Line Input #1, Rec
        'recover if needed
        If rPhase = "1" Then
            While RRec <> Rec
                Line Input #1, Rec
            Wend
            rPhase = "" 'mark that recovery has occurred
        End If
        'start processing
        While Not EOF(1)
            SkipRemainingProc = False 'if this flag get switched to true then the rest of processing for the target ssn will be skipped and the data will be logged to an error log file
            Line Input #1, Rec
            TSX0GRecs2Skip = 0
            'Flds indicies BF_SSN,LN_SEQ,LD_FOR_BEG,LD_FOR_END
            Flds = Split(Rec, ",")
            FastPath "TX3Z/DTS0H" & Flds(0) & ";;F15"
            If Check4Text(1, 72, "TSX32") Then
                puttext 9, 18, Format(Date, "MMDDYY"), "Enter"
                If Check4Text(1, 72, "TSX31") Then
                    TSX31 Flds(2), Flds(3), SkipRemainingProc
                Else
                    TSX30LnSeqFound Flds(1), SkipRemainingProc
                    If SkipRemainingProc = False Then TSX31 Flds(2), Flds(3), SkipRemainingProc
                End If
            ElseIf Check4Text(1, 72, "TSX0G") Then
                BreakOutOfLoop = False 'init flag
                While BreakOutOfLoop = False 'loop until error condition is found or Seq Num is found
                    TSX0G Flds(2), Flds(3), TSX0GRecs2Skip, SkipRemainingProc
                    If SkipRemainingProc = False Then
                        If Check4Text(1, 72, "TSX31") Then
                            TSX31 Flds(2), Flds(3), SkipRemainingProc
                            BreakOutOfLoop = True
                        Else
                            If TSX30LnSeqFound(Flds(1)) Then
                                TSX31 Flds(2), Flds(3), SkipRemainingProc
                                BreakOutOfLoop = True
                            Else
                                Hit "F12"
                                Hit "F12"
                                TSX0GRecs2Skip = TSX0GRecs2Skip + 1
                            End If
                        End If
                    Else
                        BreakOutOfLoop = True
                    End If
                Wend
            Else
                SkipRemainingProc = True
            End If
            If SkipRemainingProc Then WriteOutError Rec
            'update recovery log
            Open LogDir & "FutrAlignForbCleanUp Log.txt" For Output As #3
            Print #3, "1"
            Print #3, Rec
            Close #3
        Wend
        Close #1
    End If
    
    'check for recovery phase
    If rPhase = "" Or rPhase = "2" Then
        UID = SP.Common.GetUserID()
        'open SAS file for processing
        Open SASDir & Dir(SASDir & "AlignRepayForbCU.R5*") For Input As #1
        'get header row
        Line Input #1, Rec
        'recover if needed
        If rPhase = "2" Then
            While RRec <> Rec
                Line Input #1, Rec
            Wend
            rPhase = "" 'mark that recovery has occurred
        End If
        'process file
        While Not EOF(1)
            Line Input #1, Rec
            'Flds indicies BF_SSN,LN_SEQ,LD_FOR_BEG,LD_FOR_END
            Flds = Split(Rec, ",")
            LnSeq(0) = CInt(Flds(1))
            If SP.Common.ATD22ByLoan(Flds(0), "CUAFB", "Invalid alignment forbearance removed for date range " & Flds(2) & " to " & Flds(3) & ".", LnSeq(), "FUTRALNFOB", UID) = False Then
                MsgBox "You need access to the ""CUAFB"" ARC.  Please contact Systems Support."
                End
            End If
            'update recovery log
            Open LogDir & "FutrAlignForbCleanUp Log.txt" For Output As #3
            Print #3, "2"
            Print #3, Rec
            Close #3
        Wend
        Close #1
    End If
    Kill LogDir & "FutrAlignForbCleanUp Log.txt" 'delete log file
    If Dir("T:\FutrAlignForbCleanUp Error Log.txt") <> "" Then
        SP.Common.PrintDocs DocDir, "Error Log for Future Alignment Forbearance Cleanup", "T:\FutrAlignForbCleanUp Error Log.txt"
        MsgBox "Please pick up the printed error log at your printer."
    End If
    MsgBox "Processing Complete"
End Sub
 
Sub WriteOutError(Rec As String)
    Open "T:\FutrAlignForbCleanUp Error Log.txt" For Append As #2
    If FileLen("T:\FutrAlignForbCleanUp Error Log.txt") = 0 Then
        Print #2, "SSN,LnSeq,Begin,End"
    End If
    Print #2, Rec
    Close #2
End Sub
 
'searches for loan sequence number on TSX30
Function TSX30LnSeqFound(LnSeq As String, Optional SkipRemainingProc As Boolean = False) As Boolean
    Dim row As Integer
    row = 8
    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        If CInt(LnSeq) = CInt(GetText(row, 30, 3)) Then
            puttext 21, 14, GetText(row, 2, 3), "Enter"
            TSX30LnSeqFound = True
            SkipRemainingProc = False
            Exit Function
        End If
        row = row + 1
        If Check4Text(row, 3, " ") Then
            Hit "F8"
            row = 8
        End If
    Wend
    'do no match found logic
    TSX30LnSeqFound = False
    SkipRemainingProc = True
End Function
 
Sub TSX0G(ForbBegin As String, ForbEnd As String, TSX0GRecs2Skip As Integer, SkipRemainingProc As Boolean)
    Dim row As Integer
    Dim skipped As Integer
    row = 7
    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        If Check4Text(row, 7, "F - ALIGN RPY") And _
        CDate(ForbBegin) = CDate(GetText(row, 24, 8)) And _
        CDate(ForbEnd) = CDate(GetText(row, 33, 8)) Then
            'check if entry needs to be skipped
            If TSX0GRecs2Skip = skipped Then
                If Len(GetText(row, 2, 3)) = 1 Then
                    puttext 22, 13, "0" & GetText(row, 2, 3), "Enter"
                Else
                    puttext 22, 13, GetText(row, 2, 3), "Enter"
                End If
                puttext 9, 18, Format(Date, "MMDDYY"), "Enter"
                Exit Sub
            Else
                skipped = skipped + 1 'if the rec needs to be skipped then and one to skipped counter and skip rec
            End If
        End If
        row = row + 1
        If Check4Text(row, 3, " ") Then
            Hit "F8"
            row = 7
        End If
    Wend
    SkipRemainingProc = True
End Sub
 
Sub TSX31(ForbBegin As String, ForbEnd As String, SkipRemainingProc As Boolean)
    Dim row As Integer
    row = 12
    While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
        If Check4Text(row, 6, "F - ALIGN RPY") And _
        CDate(ForbBegin) = CDate(Replace(GetText(row, 21, 8), " ", "/")) And _
        CDate(ForbEnd) = CDate(Replace(GetText(row, 30, 8), " ", "/")) Then
            puttext row, 3, "X", "Enter"
            Hit "F12"
            If Check4Text(1, 72, "TSX32") Then
                Hit "F6"
                Exit Sub
            Else
                Hit "F12"
                Hit "F6"
                Exit Sub
            End If
        End If
        row = row + 1
        If Check4Text(row, 3, "_") = False Then
            Hit "F8"
            row = 12
        End If
    Wend
    SkipRemainingProc = True
End Sub

