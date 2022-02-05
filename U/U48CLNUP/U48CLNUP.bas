Attribute VB_Name = "U48CLNUP"
'script id =  U48CLNUP
Dim Def As String
Dim Forb As String
Const ErrorFile As String = "T:\U48ReinstateError.txt"
Dim LogFile As String
Dim LogFolder As String
Dim RecoveryState As Integer '1 for Def, 2 for Forb
Dim RSSN As String 'if not blank then Recover
Dim rState As String
Dim rLN_SEQ As String
Dim rLD_BIL_DU_LON As String
Dim SSN As String
Dim LN_SEQ As String
Dim LD_BIL_DU_LON As String
Dim User As String
Dim FTPFolder As String



Sub Main()
    If MsgBox("This is the U48 Reinstate Clean up Script.", vbOKCancel, "U48 Reinstate") = vbCancel Then End
    
    User = sp.Common.GetUserID
    Dim str As String
    LogFile = "U48ReinstateLog.txt"
    sp.Common.TestMode FTPFolder, , LogFolder
    LogFile = LogFolder & LogFile
    Def = FTPFolder & "Def.RTF"
    Forb = FTPFolder & "Forb.RTF"
    
    RSSN = ""
    If Dir(LogFile) <> "" Then
        If FileLen(LogFile) > 0 Then
            Open LogFile For Input As #1
                Input #1, RSSN, rLN_SEQ, rLD_BIL_DU_LON, rState
            Close #1
        End If
    End If
    
    
    
    If Dir(Def) = "" Or Dir(Forb) = "" Then
        MsgBox "You are missing file Def.txt and/or Forb.txt."
        End
    End If
    
    
    RecoveryState = 1
    If RSSN = "" Or (RSSN <> "" And rState = "1") Then
        Open Def For Input As #1
            Do While Not EOF(1)
                Input #1, str
                SSN = Mid(str, 1, 9)
                LN_SEQ = Trim(Mid(str, 32, 2))
                LD_BIL_DU_LON = Mid(str, 41, 10)
                LD_BIL_DU_LON = Mid(LD_BIL_DU_LON, 1, 6) & Mid(LD_BIL_DU_LON, 9, 2)
                If RSSN <> "" Or SSN = "BF_SSN   " Then
                    If RSSN = SSN And LN_SEQ = rLN_SEQ Then RSSN = ""
                Else
                    ProccessSSN
                    Open LogFile For Output As #2
                        Write #2, SSN, LN_SEQ, LD_BIL_DU_LON, RecoveryState
                    Close #2
                End If
            Loop
        Close #1
    End If
    
    RecoveryState = 2
    Open Forb For Input As #1
        Do While Not EOF(1)
            Input #1, str
            SSN = Mid(str, 1, 9)
            LN_SEQ = Trim(Mid(str, 32, 2))
            LD_BIL_DU_LON = Mid(str, 41, 10)
            LD_BIL_DU_LON = Mid(LD_BIL_DU_LON, 1, 6) & Mid(LD_BIL_DU_LON, 9, 2)
            If RSSN <> "" Or SSN = "BF_SSN   " Then
                If RSSN = SSN And LN_SEQ = rLN_SEQ Then RSSN = ""
            Else
                ProccessSSN
                Open LogFile For Output As #2
                    Write #2, SSN, LN_SEQ, LD_BIL_DU_LON, RecoveryState
                Close #2
            End If
        Loop
    Close #1
    
    PrintErrorRpt
    If Dir(LogFile) <> "" Then Kill LogFile
    If Dir(Def) <> "" Then Kill Def
    If Dir(Forb) <> "" Then Kill Forb
    MsgBox "Script Complete."
    End
End Sub

Sub ProccessSSN()
Dim Row As Integer
Dim Found As Boolean
Dim seqArr(0) As Integer
Dim TS31Dates() As Date
Dim updated As Boolean

    ReDim TS31Dates(1, 0)
    sp.Q.FastPath "TX3Z/ITS26" & SSN
    If sp.Q.Check4Text(1, 72, "TSX28") Then
        'select loan seq
        Row = 8
        Do While sp.Q.Check4Text(23, 2, "90007") = False
            If sp.Q.GetText(Row, 3, 1) = "" Then
                sp.Q.Hit "F8"
                Row = 7
            Else
                If sp.Q.Check4Text(Row, 15, Format(LN_SEQ, "000")) Then
                    sp.Q.PutText 21, 12, "", "END"
                    sp.Q.PutText 21, 12, sp.Q.GetText(Row, 2, 2), "ENTER"
                    Exit Do
                End If
                Row = Row + 1
            End If
        Loop
    End If
    sp.Q.Hit "F2"
    sp.Q.Hit "F7"
    If sp.Q.Check4Text(1, 72, "TSX31") Then
        Row = 12
        Do While sp.Q.Check4Text(23, 2, "90007") = False
            If sp.Q.GetText(Row, 21, 8) = "" Then
                sp.Q.Hit "F8"
                Row = 12
            Else
            
                If CDate(Replace(sp.Q.GetText(Row, 72, 8), " ", "/")) <= CDate("01/20/2006") Then
                    If CDate(Replace(sp.Q.GetText(Row, 30, 8), " ", "/")) >= CDate(LD_BIL_DU_LON) Then
                        ReDim Preserve TS31Dates(1, UBound(TS31Dates, 2) + 1)
                        TS31Dates(0, UBound(TS31Dates, 2)) = CDate(Replace(sp.Q.GetText(Row, 21, 8), " ", "/"))
                        TS31Dates(1, UBound(TS31Dates, 2)) = CDate(Replace(sp.Q.GetText(Row, 30, 8), " ", "/"))
                    End If
                End If
                Row = Row + 1
            End If
        Loop
    End If

    sp.Q.FastPath "TX3Z/CTS8S" & SSN
    If sp.Q.Check4Text(1, 72, "TSX8V") Then
        'selection screen
        Row = 8
        Found = False
        Do While sp.Q.Check4Text(23, 2, "90007") = False '90007 NO MORE DATA TO DISPLAY
            If sp.Q.GetText(Row, 5, 1) = "" Then
                sp.Q.Hit "F8"
                Row = 8
            Else
                If sp.Q.Check4Text(Row, 8, Format(LN_SEQ, "000")) Then
                    Found = True
                    sp.Q.PutText 21, 18, "", "END"
                    sp.Q.PutText 21, 18, sp.Q.GetText(Row, 4, 3), "ENTER"
                    Exit Do
                End If
                Row = Row + 1
            End If
        Loop
    End If
    updated = False
    If sp.Q.Check4Text(1, 72, "TSX8W") Then
        Row = 10
        Found = False
        Do While sp.Q.Check4Text(23, 2, "90007") = False
            If sp.Q.GetText(Row, 54, 1) = "" Then
                sp.Q.Hit "ENTER"
                sp.Q.Hit "F4"
                If sp.Q.Check4Text(23, 2, "03347") Then  '03347 ALL UPDATES COMPLETED
                    updated = True
                End If
                sp.Q.Hit "F8"
                Row = 10
            Else
'                If SP.Q.Check4Text(row, 19, LD_BIL_DU_LON) Then
                If InDateRange(TS31Dates, CDate(sp.Q.GetText(Row, 19, 8))) Then
                    Found = True
                    sp.Q.PutText Row, 54, "Y"
'                    SP.Q.Hit "ENTER"
'                    SP.Q.Hit "F4"
'                    Exit Do
                End If
                Row = Row + 1
            End If
        Loop
        If updated = False Then
            'add to error report
            Open ErrorFile For Append As #3
                Write #3, SSN, LN_SEQ, LD_BIL_DU_LON
            Close #3
        Else
            'add ARC
            seqArr(0) = CInt(LN_SEQ)
            If sp.Common.ATD22ByLoan(SSN, "U48OB", "U48 Override Benefit", seqArr, "U48CLNUP", User) = False Then
                MsgBox "There was a problem adding a U48OB ARC for SSN: " & SSN & " Ending script."
                End
            End If
        End If
        
        
        
    End If
    
End Sub

Function InDateRange(dtArr() As Date, Dt As Date) As Boolean
    Dim x As Integer
    InDateRange = False
    For x = 1 To UBound(dtArr, 2)
        If Dt >= dtArr(0, x) And Dt <= dtArr(1, x) And Dt >= CDate(LD_BIL_DU_LON) Then
            InDateRange = True
            Exit For
        End If
    Next x
End Function


Sub PrintErrorRpt()
Dim str As String
If Dir(ErrorFile) = "" Then Exit Sub
If FileLen(ErrorFile) = 0 Then
    Kill ErrorFile
    Exit Sub
End If
'print the error report
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")
        'open form doc
        Word.Visible = False
        Word.Documents.Add DocumentType:=wdNewBlankDocument
        'add a header
        Word.Selection.Font.Size = 20
        Word.Selection.TypeText Text:="U48 Override Benefit Error Report"
        Word.Selection.TypeParagraph
        Word.Selection.TypeParagraph
        Word.Selection.Font.Size = 12
        
        Open ErrorFile For Input As #3
        While Not EOF(3)
            Line Input #3, str
            Word.Selection.TypeText Text:=Replace(str, """", "")
            Word.Selection.TypeParagraph
        Wend
        Close #3
        
        
        Word.ActiveDocument.SaveAs FileName:="T:\U48CLNUP.doc", _
            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False
        Session.Wait 2
        Word.ActiveDocument.PrintOut False
        Word.Application.Quit SaveChanges:=wdDoNotSaveChanges
        Kill ErrorFile
        If MsgBox("Is it safe to delete the 'T:\U48CLNUP.doc' temporary error report file?  Click Yes to delete the file if the report printed successfully or click No to delete the file later yourself.", vbYesNo + vbQuestion, "Delete Temporary File") = vbYes Then Kill "T:\U48CLNUP.doc"
End Sub


'<new> sr1424 tp, 02/19/06
