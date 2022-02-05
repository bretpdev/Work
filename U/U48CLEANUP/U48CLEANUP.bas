Attribute VB_Name = "U48CLEANUP"
Dim ftpFolder As String
Dim sas As String
Dim LogFolder As String
Dim logFile As String
Dim Dt() As Date
Const Err As String = "T:\U48CleanUpError.txt"
Dim UserID As String

Sub Main()
If MsgBox("This is the U48 Cleanup script.", vbOKCancel) = vbCancel Then End
UserID = SP.Common.GetUserID
SP.Common.TestMode ftpFolder, , LogFolder
logFile = LogFolder & "U48CleanUpLog.txt"
sas = Dir(ftpFolder & "U48.CLAEANUP.R2")
'missing file
If sas = "" Then
    MsgBox "SAS file U48.U48CLEANUP.R2 is missing. Contact System Support.", vbCritical
    End
End If
'get file name delete extra files
sas = SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "U48.CLAEANUP.R2")
'empty file
If FileLen(ftpFolder & sas) = 0 Then
    Kill ftpFolder & sas
    MsgBox "File empty. There were no loans to process for today."
    End
End If

WorkFile
ErrorRpt

If Dir(ftpFolder & sas) <> "" Then Kill ftpFolder & sas
MsgBox "Process Complete!"
End Sub

Sub WorkFile()
Dim str As String
Dim Arr() As String
    Open ftpFolder & sas For Input As #1
        Do While Not EOF(1)
            Line Input #1, str
            Arr = Split(str, ",")
            If Arr(0) <> "BF_SSN" Then
                WorkRec Arr(0), Format(Arr(1), "0000"), CDate(Arr(2))
            End If
            
        Loop
    Close #1
    
End Sub

Sub WorkRec(ssn As String, LnSeq As String, DueDt As Date)
Dim x As Integer
ReDim Dt(2, 0)

SP.Q.FastPath "TX3Z/ITS26" & ssn

If SP.Q.Check4Text(1, 72, "TSX28") Then
'selection screen
    For x = 8 To 20
        If SP.Q.Check4Text(x, 14, LnSeq) Then
            SP.Q.puttext 21, 12, SP.Q.GetText(x, 2, 2), "ENTER"
            Exit For
        End If
        If x = 20 Then
            SP.Q.Hit "F8"
            x = 8
        End If
    Next x
End If
'target screen
SP.Q.Hit "F2"
SP.Q.Hit "F7"
'TS31
For x = 12 To 22
    If SP.Q.GetText(x, 21, 8) = "" Then
        SP.Q.Hit "F8"
        x = 12
    Else
        'Capture Begin and End Dates of all deferments/forbearances beginning with the bill due date from the file where the cert date is less than 1/20/06.
        If CDate(Replace(SP.Q.GetText(x, 72, 8), " ", "/")) < CDate("01/20/06") And _
        (BetweenDates(CDate(Replace(SP.Q.GetText(x, 21, 8), " ", "/")), CDate(Replace(SP.Q.GetText(x, 30, 8), " ", "/")), DueDt) Or _
        CDate(Replace(SP.Q.GetText(x, 21, 8), " ", "/")) > DueDt) Then
            Dt(0, UBound(Dt, 2)) = CDate(Replace(SP.Q.GetText(x, 21, 8), " ", "/")) 'begin date
            Dt(1, UBound(Dt, 2)) = CDate(Replace(SP.Q.GetText(x, 30, 8), " ", "/")) 'end date
            Dt(2, UBound(Dt, 2)) = CDate(Replace(SP.Q.GetText(x, 72, 8), " ", "/")) 'cert date
            ReDim Preserve Dt(2, UBound(Dt, 2) + 1)
        End If
    End If
    If x = 22 Then
        SP.Q.Hit "F8"
        x = 12
    End If
    If SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") Then
        Exit For
    End If
Next x
If UBound(Dt, 2) > 0 Then ReDim Preserve Dt(2, UBound(Dt, 2) - 1) 'remove extra row
SP.Q.FastPath "TX3Z/CTS8S" & ssn
If SP.Q.Check4Text(1, 72, "TSX8V") Then
'selection screen
    For x = 8 To 20
        If SP.Q.Check4Text(x, 8, MID(LnSeq, 2, 3)) Then 'check lnseq 3 digits
            SP.Q.puttext 21, 18, SP.Q.GetText(x, 4, 2), "ENTER"
            Exit For
        End If
        If x = 20 Then
            SP.Q.Hit "F8"
            x = 8
        End If
    Next x
End If
'target screen
For x = 10 To 22
    If SP.Q.GetText(x, 19, 8) = "" Then
        SP.Q.Hit "F8"
        x = 10
    Else
        If IsInDates(CDate(SP.Q.GetText(x, 19, 8))) Then
            SP.Q.puttext x, 54, "Y"
        End If
    End If
    If x = 22 Then
        SP.Q.Hit "F8"
        x = 10
    End If
    If SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") Then Exit For
Next x
SP.Q.Hit "ENTER"
SP.Q.Hit "F4"
If SP.Q.Check4Text(23, 2, "03347 ALL UPDATES COMPLETED") = False Then
'error report
    Open Err For Append As #3
        Write #3, ssn, LnSeq, Format(DueDt, "mm/dd/yyyy")
    Close #3
End If
'add comment
Dim Seq(0) As Integer
Seq(0) = CInt(LnSeq)
If SP.Common.ATD22ByLoan(ssn, "U48OB", "U48 Override Benefit", Seq, "U48CleanUp", UserID) = False Then
    Open Err For Append As #3
        Write #3, ssn, LnSeq, Format(DueDt, "mm/dd/yyyy")
    Close #3
    ErrorRpt
    MsgBox "The U48OB ARC is required to run this script."
    End
End If
End Sub

Function BetweenDates(D1 As Date, D2 As Date, testDate As Date) As Boolean
    If testDate >= D1 And testDate <= D2 Then
        BetweenDates = True
    Else
        BetweenDates = False
    End If
End Function

Function IsInDates(testDate As Date) As Boolean
'checks if date is in the rages provided from TS31
    Dim x As Integer
    For x = 0 To UBound(Dt, 2)
        If (testDate >= Dt(0, x) And testDate <= Dt(1, x)) Then
            IsInDates = True
            Exit Function
        End If
    Next x
    IsInDates = False
End Function

Sub ErrorRpt()
Dim str As String
If Dir(Err) = "" Then Exit Sub
If FileLen(Err) = 0 Then Exit Sub
'print the error report
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")
        'open form doc
        Word.Visible = False
        Word.Documents.Add DocumentType:=wdNewBlankDocument
        'add a header
        Word.Selection.Font.Size = 20
        Word.Selection.TypeText TEXT:="Override Benefit Error Report"
        Word.Selection.TypeParagraph
        Word.Selection.TypeParagraph
        Word.Selection.Font.Size = 12
        
        Open Err For Input As #3
        While Not EOF(3)
            Line Input #3, str
            Word.Selection.TypeText TEXT:=Replace(str, """", "")
            Word.Selection.TypeParagraph
        Wend
        Close #3
        
        
        Word.ActiveDocument.SaveAs Filename:="T:\KeyLoanIdentifierDiscrepanciesRPT.doc", _
            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False
        Session.Wait 2
        Word.ActiveDocument.PrintOut False
        Word.Application.Quit SaveChanges:=wdDoNotSaveChanges
        Kill Err
End Sub

'<new> sr1697, tp, 06/27/06
