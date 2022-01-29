Attribute VB_Name = "WVFEECLUP"
'Script ID = WVFEECLUP
Private File As String
Private ErrFile As String

Private testSSN As String
Private NextSSN As String
Private SSN As String
Private LNSEQ As String
Private LateFee As String
Private EffDate As String
Private DSBDate As String
Private LFee() As String
Private Seq() As Integer
Private User As String

Sub Main()
If MsgBox("This is the Waive Late Fees Cleanup script. Click ok to run, Cancel to quit.", vbOKCancel, "Waive Late Fees Cleanup") = vbCancel Then End
User = sp.Common.GetUserID
ReDim LFee(4, 0)
File = "C:\WINDOWS\Temp\LateFee.txt"
ErrFile = "C:\WINDOWS\Temp\LateFeeError.txt"



Open ErrFile For Output As #3
Close #3

Open File For Input As 1 'read ahead with this file
Open File For Input As 2 'retrive data from here
    Do While Not EOF(1)
        Input #1, testSSN, v, v, v, v
        If SSN = "" Then SSN = testSSN
        If testSSN = SSN Then
            AddToLFee
        Else
            'new SSN found
            processLFee
            ReDim LFee(4, 0) 'clear LFee
            AddToLFee
            SSN = testSSN
        End If
    Loop
    processLFee
       
Close #2
Close #1
If Dir(File) <> "" Then Kill File
PrintErrors
MsgBox "Process Complete!"

End Sub

Sub PrintErrors()
Dim temp As String
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")
        Word.Visible = False
        Word.Documents.Add DocumentType:=wdNewBlankDocument
        'set font and insert text
        Word.Selection.Font.Size = 16
        Word.Selection.TypeText Text:="Waive Late Fees Clean Up Error Report"
        Word.Selection.TypeParagraph
        Word.Selection.TypeParagraph
        Word.Selection.Font.Size = 12
        
        Open ErrFile For Input As #3
        Do While Not EOF(3)
            Input #3, temp
            Word.Selection.TypeText Text:=temp & vbCrLf
        Loop
        Close #3
        
       Word.ActiveDocument.SaveAs FileName:="C:\WINDOWS\Temp\LateFeeErrors.doc", _
            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False
      
        'Session.Wait 2
        'Word.Application.Quit SaveChanges:=wdDoNotSaveChanges
        Word.Visible = True
End Sub

Sub AddToLFee()
    Input #2, NextSSN, LNSEQ, LateFee, EffDate, DSBDate
    ReDim Preserve LFee(4, UBound(LFee, 2) + 1)
    LFee(0, UBound(LFee, 2)) = NextSSN
    LFee(1, UBound(LFee, 2)) = LNSEQ
    LFee(2, UBound(LFee, 2)) = LateFee
    LFee(3, UBound(LFee, 2)) = EffDate
    LFee(4, UBound(LFee, 2)) = DSBDate
End Sub

Sub processLFee()
Dim x As Integer
Dim x2 As Integer
    sp.Q.FastPath "TX3Z/CTS89" & SSN
    If sp.Q.Check4Text(1, 72, "TSX8C") = False Then
        'Screen not found. (TSX8C)
        Exit Sub
    End If
    sp.Q.PutText 10, 36, "W"
    sp.Q.PutText 11, 36, GetOldestDSB
    sp.Q.PutText 12, 36, "111405" 'period end 11/14/05
    sp.Q.PutText 13, 36, "S"
    sp.Q.PutText 17, 36, "X"
    sp.Q.Hit "F10"
    If sp.Q.Check4Text(23, 2, "03725") Then '03725 NO LOAN INFORMATION FOUND FOR PERIOD BEGIN DATE ENTERED
        Exit Sub
    End If
    x = 0
    Do
        If sp.Q.Check4Text(12 + x, 3, "_") Then
            For x2 = 1 To UBound(LFee, 2)
                If sp.Q.Check4Text(12 + x, 18, Format(CInt(LFee(1, x2)), "00")) Then
                    sp.Q.PutText 12 + x, 3, "X"
                End If
            Next x2
        Else
            sp.Q.Hit "F8"
            If sp.Q.Check4Text(23, 2, "90007") Then
                Exit Do
            End If
            x = -1
        End If
        x = x + 1
    Loop
    sp.Q.Hit "F6"
    sp.Q.Hit "F6"
    If sp.Q.Check4Text(23, 2, "03732") = False Then
        'if "03732 Late Fee Override Indicators Updated" not found then write to error report
        Open ErrFile For Append As #3
            Write #3, SSN
        Close #3
    Else
        sp.Common.ATD22ByLoan SSN, "DRLFA", GetComment, Seq, "WVFEECLUP", User
    End If
    
    
    
End Sub

Function GetComment() As String
    Dim TotFee As Double
    Dim x As Integer
    Dim x2 As Integer
    Dim fee As Double
    Dim LSeq As String
    Dim Dt As String
    Dim str As String
    ReDim Seq(0)
    For x = 1 To UBound(LFee, 2)
        If LSeq <> LFee(1, x) Then
            LSeq = LFee(1, x)
            ReDim Preserve Seq(UBound(Seq) + 1)
            Seq(UBound(Seq)) = CInt(LSeq)
        End If
    Next x
    
    
    For x2 = 1 To UBound(Seq)
        fee = 0
        Dt = ""
        For x = 1 To UBound(LFee, 2)
            If CStr(Seq(x2)) = LFee(1, x) Then
                fee = fee + CDbl(LFee(2, x))
                If Dt = "" Then
                    Dt = LFee(3, x)  'Eff Date
                Else
                    Dt = Dt & ", " & LFee(3, x)  'Eff Date
                End If
            End If
        Next x
        If str = "" Then
            str = "Loan Seq - " & Seq(x2) & " " & FormatCurrency(fee, 2) & " (" & Dt & ")"
        Else
            str = str & " Loan Seq - " & Seq(x2) & " " & FormatCurrency(fee, 2) & " (" & Dt & ")"
        End If
        TotFee = TotFee + fee
    Next x2
    str = "WLFCU - " & TotFee & " " & str
    GetComment = str
End Function

Function GetOldestDSB() As String
    Dim OldestDt As Date
    Dim x As Integer
    OldestDt = CDate(LFee(4, 1))
    For x = 1 To UBound(LFee, 2)
        If CDate(LFee(4, x)) < OldestDt Then
            OldestDt = CDate(LFee(4, x))
        End If
    Next x
    GetOldestDSB = Format(OldestDt, "MMDDYY")
End Function
