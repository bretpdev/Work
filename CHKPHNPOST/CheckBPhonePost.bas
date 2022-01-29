Attribute VB_Name = "CheckBPhonePost"
'OneLink Check By Phone Posting

Dim ErrorA() As String

Sub Main()
    Dim file As String 'file name being processed
    Dim payAmount As Double 'total of payment amounts
    Dim formPayAmount As Double 'from frmCheckByPhone
    Dim Trans As Integer 'count of number of transactions
    Dim dataStr As String
    Dim Data() As String
    Dim batchID As String
    Dim X As Integer
    Dim userName() As String

    ReDim ErrorA(1, 0) As String
    If MsgBox("This is the OneLINK Check by Phone Posting Script. Click OK to continue, or Cancel to end the script.", vbOKCancel, "Check By Phone Posting") = vbCancel Then End
    frmCheckByPhone.TextBox1.Text = ""
    frmCheckByPhone.Show
    
    formPayAmount = frmCheckByPhone.TextBox1.Text

    
    If Sp.Common.TestMode Then
        file = "X:\PADD\CheckbyPhone\Test\CBP " & Format(Date, "MMDDYY") & ".txt"
    Else
        file = "X:\PADD\CheckbyPhone\CBP " & Format(Date, "MMDDYY") & ".txt"
    End If
    If Dir(file) = "" Then
        MsgBox "The check by phone file is missing.  Please contact Systems Support for assistance if the file should be available."
        End
    End If
    If FileLen(file) = 0 Then
        MsgBox "The check by phone file is empty.  Please contact Systems Support for assistance if this is incorrect."
        End
    End If
    Trans = 0
    Open file For Input As #3
    Do While Not EOF(3)
        Line Input #3, dataStr
        Data = Split(dataStr, ",")
        payAmount = payAmount + Data(2)
        Trans = Trans + 1
    Loop
    Close #3
    
    While (Round(payAmount, 2) <> Round(formPayAmount, 2))
        MsgBox "The total amount entered does not match the amount in the file. Please enter the amount again.", vbExclamation, "Check By Phone Posting"
        frmCheckByPhone.Show
        formPayAmount = frmCheckByPhone.TextBox1.Text
    Wend
    
    
    Sp.Q.FastPath "LC38A" & Format(Date, "MMDDYYYY")
    Sp.Q.puttext 9, 32, Format(Trans, "0000000")
    Sp.Q.puttext 9, 42, fDouble11(payAmount)
    Sp.Q.hit "TAB"
    Sp.Q.puttext 9, 72, "BR", "ENTER"
    'retrieve batch ID
    For X = 0 To 11
        If Sp.Q.GetText(9 + X, 6, 12) = "" Then
            If X = 0 Then
                Sp.Q.hit "F7"
                X = 12
            End If
            Sp.Q.puttext 9 + X - 1, 2, "X", "ENTER"
            Exit For
        End If
        batchID = Sp.Q.GetText(9 + X, 6, 12)
        If X = 11 Then
            Sp.Q.hit "F8"
            X = -1
            If Sp.Q.check4text(22, 3, "46004") Then
                Sp.Q.puttext 9 + 11, 2, "X", "ENTER"
                Exit For
            End If
        End If
    Next X
    
    X = 0
    Open file For Input As #3
    Do While Not EOF(3)
        Line Input #3, dataStr
        Data = Split(dataStr, ",")

        Sp.Q.puttext 9 + X, 2, Data(0)
        Sp.Q.puttext 9 + X, 18, fDouble11(CDbl(Data(2)))
        'sp.Q.puttext 9 + X, 34, Format(Date, "MMDDYYYY")
        Sp.Q.puttext 9 + X, 34, Format(Data(6), "MMDDYYYY")
        Sp.Q.hit "ENTER"
        If Sp.Q.check4text(22, 3, "40021") Then
            'add SSN and payment ammount to error file.
            ReDim Preserve ErrorA(1, UBound(ErrorA, 2) + 1)
            ErrorA(0, UBound(ErrorA, 2)) = Data(0)
            ErrorA(1, UBound(ErrorA, 2)) = FormatCurrency(CDbl(Data(2)), 2)
        End If
        If Sp.Q.check4text(22, 3, "44068") Then
            Sp.Q.hit "ENTER"
        End If
        If X >= 11 Then X = -1
        X = X + 1
    Loop
    Close #3
    Sp.Q.hit "F12"
    'find batch id
    Session.FindText batchID, 1, 1
    Sp.Q.puttext Session.FoundTextRow, Session.FoundTextColumn - 4, "X"
    Sp.Q.hit "F2"
    If Sp.Q.check4text(22, 3, "44034 BATCH TOTALS ARE VERIFIED") Then
        Sp.Q.FastPath "LC90I"
        Sp.Q.hit "ENTER"
        Dim BatchSeq As String
        BatchSeq = Right(batchID, 4)
        Session.FindText BatchSeq, 1, 1
        If (Session.FoundTextRow = 0) Then
            Do While Session.FoundTextRow = 0
                Sp.Q.hit "F8"
                Session.FindText BatchSeq, 1, 1
            Loop
        End If

        Sp.Q.puttext 21, 13, Sp.Q.GetText(Session.FoundTextRow, Session.FoundTextColumn - 27, 2)
        Sp.Q.hit "ENTER"
        
        Session.TransmitTerminalKey rcIBMPrintKey
        Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        
        MsgBox "Batch entry is complete and batch totals have been verified."
    Else
        MsgBox "Batch entry is complete, however batch totals do not match, please review error report and correct manually."
        'print error report
        ''''''''''''''''''''''''''''''
        Dim Doc As String
        Doc = "CheckBPhonePostError"
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")

        'open form doc
        Word.Documents.Open FileName:="T:\" & Doc & ".doc", ConfirmConversions:=False, _
            ReadOnly:=False, AddToRecentFiles:=False, PasswordDocument:="", _
            PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
            WritePasswordTemplate:="", Format:=wdOpenFormatAuto

        
        
        Word.Visible = False
        Word.Documents.Add DocumentType:=wdNewBlankDocument
        'set font and insert text
        Word.selection.Font.Size = 20
        Word.selection.TypeText Text:="OneLINK Check By Phone Posting - Error Report"
        Word.selection.TypeParagraph
        Word.selection.TypeParagraph
        Word.selection.Font.Size = 16
        Word.selection.Font.Underline = wdUnderlineThick
        Word.selection.TypeText Text:="SSN:        Payment Amount:"
        Word.selection.Font.Underline = wdUnderlineNone
        Word.selection.TypeParagraph
        
        For X = 1 To UBound(ErrorA, 2)
            Word.selection.Font.Size = 10
        
            Word.selection.TypeText Text:=ErrorA(0, X) & "          " & ErrorA(1, X)
            Word.selection.TypeParagraph
            
        Next X
        
       'save document
       Word.ActiveDocument.PrintOut
       
'       Word.ActiveDocument.SaveAs FileName:="C:\Windows\Temp\" & Doc & ".doc", _
'            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
'            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
'            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
'            SaveAsAOCELetter:=False
       
        Session.Wait 2

        Word.Application.Quit SaveChanges:=wdDoNotSaveChanges
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        End
    End If
    
    'Generate Funds Transfer Forms (FUNDSTRAN)

    userName = Sp.Common.Sql("SELECT FirstName + ' ' + LastName AS UserName FROM SYSA_LST_Users WHERE WindowsUserName = '" & Sp.Common.WindowsUserName & "'")

    FundForms Format(formPayAmount, "$###,###,##0.00"), userName
    
    
End Sub

'create data file and print docs FUNDSTRAN
Sub FundForms(DocAmt As Double, RequestedBy() As String)
    'create wire transfer memo
    Open "T:\CheckBPhonePost Funds Transfer dat.txt" For Output As #1
        Write #1, "DeadLineDate", "DeadLineTime", "Program", "From", "To", "Purpose", "Desc", "Amt", "ReqBy", "CurrDatAndTime"
        Write #1, Format(Date, "MM/DD/YYYY"), "4:30 PM", "Loan Guarantee Program (LGP)", "Borrowers", "US Bank 153195082182 - FSLRF Checking", "Approved ACH Batch", "OneLINK Check by Phone", Format(DocAmt, "$#,###,##0.00"), RequestedBy(1), CStr(Now)
    Close #1

        Sp.Common.PrintDocs "X:\PADD\Operational Accounting\", "FUNDSTRAN", "T:\CheckBPhonePost Funds Transfer dat.txt"

    Kill "T:\CheckBPhonePost Funds Transfer dat.txt"
End Sub

Function fDouble11(d As Double) As String
    Dim num As String
    num = FormatNumber(d, 2)
    Dim X As Integer
    'formate for 14 digit number
    While Len(num) < 12
        num = "0" & num
    Wend
    fDouble11 = num
End Function







