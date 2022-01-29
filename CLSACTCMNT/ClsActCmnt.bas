Attribute VB_Name = "ClsActCmnt"
Dim recovery As Boolean 'true if in recovery mode
    'Column 1 = SSN
    'Column 2 = ARC
    'Column 3 = Request Date
    'Column 4 = Response Date
    'Column 5 = Response Code
Dim SSN As String
Dim ARC As String
Dim ReqDt As String
Dim RespDt As String
Dim RespCode As String
Dim rSSN As String
Dim rARC As String
Dim rReqDt As String
Dim rRespDt As String
Dim rRespCode As String
Const afile As String = "T:\CompRespCode.txt"
Const ErrorFile As String = "T:\ClsActCmntError.txt"
Dim LogFile As String
Dim LogFolder As String

Sub Main()
    If MsgBox("This is the 'Close Compass Activity Comment' Script. Press OK to continue.", vbOKCancel) = vbCancel Then End
    SP.common.TestMode , , LogFolder
    LogFile = "ClsActCmntLog.txt"
    LogFile = LogFolder & LogFile
    
    recovery = False
    If Dir(LogFile) <> "" Then
        If FileLen(LogFile) <> 0 Then
            Open LogFile For Input As #1
                Input #1, rSSN, rARC, rReqDt, rRespDt, rRespCode
            Close #1
            recovery = True
        End If
    End If
    If Dir(ErrorFile) = "" Then
        Open ErrorFile For Output As #2
        Close #2
    End If
    
    VerifyFile
    ProcessFile
    PrintErrors
    
    If Dir(afile) <> "" Then Kill afile
End Sub

Sub PrintErrors()
Dim str As String
    If FileLen(ErrorFile) <> 0 Then
'print the error report
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")
        'open form doc
        Word.Visible = False
        Word.Documents.Add DocumentType:=wdNewBlankDocument
        'add a header
        Word.Selection.Font.Size = 20
        Word.Selection.TypeText text:="Close Compass Activity Comment - Error File"
        Word.Selection.TypeParagraph
        Word.Selection.TypeParagraph
        Word.Selection.Font.Size = 12
        
        Open ErrorFile For Input As #3
        While Not EOF(3)
            Line Input #3, str
            Word.Selection.TypeText text:=Replace(str, """", "")
            Word.Selection.TypeParagraph
        Wend
        Close #3
        
        
        Word.ActiveDocument.SaveAs FileName:="T:\ClsActCmntError.doc", _
            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False
        Session.Wait 2
        Word.ActiveDocument.PrintOut False
        Word.Application.Quit SaveChanges:=wdDoNotSaveChanges

    End If
    Kill ErrorFile
End Sub

Sub ProcessFile()
    Dim foundRecovery As Boolean
    Open afile For Input As #1
        If recovery Then
            foundRecovery = False
            Do While Not EOF(1)
                Input #1, SSN, ARC, ReqDt, RespDt, RespCode
                If SSN = rSSN And ARC = rARC And ReqDt = rReqDt And RespDt = rRespDt And RespCode = rRespCode Then
                    foundRecovery = True
                    Exit Do
                End If
            Loop
            If foundRecovery = False Then
                MsgBox "The record the script was recovering to was not found."
                End
            End If
        End If
        Do While Not EOF(1)
            Input #1, SSN, ARC, ReqDt, RespDt, RespCode
            If SSN <> "BF_SSN" Then
            ReqDt = Mid(ReqDt, 1, 2) & Mid(ReqDt, 4, 2) & Mid(ReqDt, 9, 2)
                If Trim(RespDt) = "" Then
                'For all activity records where the Activity Response Date is Blank
                    SP.Q.FastPath "TX3Z/CTD2A" & SSN
                    SP.Q.PutText 11, 65, ARC
                    SP.Q.PutText 21, 16, ReqDt
                    SP.Q.PutText 21, 30, ReqDt
                    SP.Q.PutText 6, 60, "X", "ENTER"
                    If SP.Q.Check4Text(23, 2, "01019") = False Then 'if 01019 is found the loan may have been deconverted.
                        If SP.Q.Check4Text(1, 72, "TDX2D") Then 'Activity Detail Screen
                            If SP.Q.GetText(15, 2, 5) = "_____" Then
                                SP.Q.PutText 15, 2, RespCode, "ENTER"
                                If SP.Q.Check4Text(23, 2, "01005") = False Then
                                    'Error, Add to error report
                                    Open ErrorFile For Append As #3
                                        Write #3, SSN, ARC, ReqDt, RespDt, RespCode
                                    Close #3
                                Else
                                    Open LogFile For Output As #3
                                        Write #3, SSN, ARC, ReqDt, RespDt, RespCode
                                    Close #3
                                End If
                            End If
                        ElseIf SP.Q.Check4Text(1, 72, "TDX2C") Then
                            SP.Q.PutText 5, 14, "X", "ENTER"
                            '''''''''''''''''''''''''''''
                            Do
                                If SP.Q.GetText(15, 2, 5) = "_____" Then
                                'Populate the first Records Activity Response Code with the appropriate value
                                    SP.Q.PutText 15, 2, RespCode, "ENTER"
                                    Open LogFile For Output As #3
                                        Write #3, SSN, ARC, ReqDt, RespDt, RespCode
                                    Close #3
                                    Exit Do
                                Else
                                    SP.Q.Hit "F8"
                                End If
                                If SP.Q.Check4Text(23, 2, "01033") Then
                                'more pages
                                    SP.Q.Hit "ENTER"
                                End If
                                'if no more data to display then
                                If SP.Q.Check4Text(23, 2, "90007") Then
                                    MsgBox "Fatal Error!!! Unable to update record. Ending script."
                                    End
                                End If
                            Loop
                        End If
                    End If
                End If
            End If
        Loop
    Close #1
End Sub

Sub VerifyFile()
    'This checks to see if the file is in the correct format
    If Dir(afile) = "" Then
        MsgBox "File not found. (T:\CompRespCode.txt)"
        End
    End If
    If FileLen(afile) = 0 Then
        MsgBox "File Empty. Script Complete."
        End
    End If
    'Dim status As frmStatus
    frmStatus.Show 0
    frmStatus.lblstatus.Caption = "Verifying file format..."
    Open afile For Input As #1
        Do While Not EOF(1)
            Input #1, SSN, ARC, ReqDt, RespDt, RespCode
            On Error GoTo ErrorHandler
            If SSN <> "BF_SSN" Then
                If Len(SSN) <> 9 Or IsNumeric(SSN) = False Then
                    MsgBox "Invalid File format at: " & SSN & "," & ARC & "," & ReqDt & "," & RespDt & "," & RespCode
                    Close #1
                    status.Hide
                    End
                End If
            End If
        Loop
        
    Close #1
    frmStatus.Hide
    Exit Sub
    
ErrorHandler:
        frmStatus.Hide
        Close #1
        MsgBox "The file is corrupt. Ending script."
        End

End Sub


'<NEW> sr1409 tp, 02/02/06

