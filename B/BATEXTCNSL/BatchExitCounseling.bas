Attribute VB_Name = "BatchExitCounseling"
Dim OAddr As Boolean 'update onelink address
Dim OPhone As Boolean 'update phone
Dim OEmail As Boolean 'update email
Dim CAddr As Boolean 'update compass address
Dim CPhone As Boolean 'update phone
Dim CEmail As Boolean 'update email
Dim ModifiedCompass As Boolean 'true if TX1J was updated
Dim User As String
Dim SchName As String
Dim Nfolder As String
Dim Nfile As String 'processing file
Dim Lfolder As String
Dim lFile As String 'log file
Dim lLine As String 'recovery line
Dim Efile As String 'error file
Dim ErrorArr() As String 'error array to be printed
'0 = SSN, 1 = firstname, 2 = lastname, 3 = middle ini, 4 = Bday, 5 = system
Dim LineArr() As String
'Indexing for LineArr() goes as follows
'0 = "first_name"
'1 = "last_name"
'2 = "middle_initial"
'3 = "ssn"
'4 = "address"
'5 = "city"
'6 = "state"
'7 = "zip"
'8 = "zip_plus"
'9 = "phone"
'10 = "email"
'11 = "school_code and school_branch"
'12 = date_of_birth
'13 = "drivers_license"
'14 = "drivers_license_state"
'15-23 = ,,,,,,,,,
'24 = "employer"
'25 = "emp_address"
'26 = "emp_city"
'27 = "emp_state"
'28 = "emp_zip"
'29 = "emp_zip_plus"
'30 = "emp_phone"
'31 = "kin_last_name"
'32 = "kin_first_name"
'33 = "kin_middle_initial"
'34 = "kin_address"
'35 = "kin_city"
'36 = "kin_state"
'37 = "kin_zip"
'38 = "kin_zip_plus"
'39 = "kin_phone"
'40 = "kin_relationship"
'41 = "ref1_last_name"
'42 = "ref1_first_name"
'43 = "ref1_middle_initial"
'44 = "ref1_address"
'45 = "ref1_city"
'46 = "ref1_state"
'47 = "ref1_zip"
'48 = "ref1_zip_plus"
'49 = "ref1_phone"
'50 = "ref1_employer"
'51 = "ref1_relationship"
'52 = "ref2_last_name"
'53 = "ref2_first_name"
'54 = "ref2_middle_initial"
'55 = "ref2_address"
'56 = "ref2_city"
'57 = "ref2_state"
'58 = "ref2_zip"
'59 = "ref2_zip_plus"
'60 = "ref2_phone"
'61 = "ref2_employer"
'62 = "ref2_relationship"
'63 = "guarantor"
'64 = "lender"
'65 = create_TimeStamp
'67-72 = ,,,,,,


Sub Main()
    If Not SP.Common.CalledByMBS Then If MsgBox("This is the Batch Exit Counseling Script. Click OK to continue.", vbOKCancel, "Exit Counseling") = vbCancel Then End '<3>
    ReDim Data(72, 0)
    ReDim ErrorArr(5, 0)
    User = SP.Common.GetUserID
    'set test/live folders
    If SP.Common.TestMode Then
        Nfolder = "X:\PADD\FTP\Test\"
        Lfolder = "X:\PADD\Logs\Test\"
    Else
        Nfolder = "X:\PADD\FTP\"
        Lfolder = "X:\PADD\Logs\"
    End If
    Nfile = Dir(Nfolder & "EXITCNSL*")
    lFile = "ExitCounselingLog.txt"
    Efile = "ExitCounselingError.txt"
    'check for file existance
    If Nfile = "" Then
        MsgBox "File not found."
        End
    End If
    'check for recovery file
    If Dir(Lfolder & lFile) <> "" Then
        If FileLen(Lfolder & lFile) <> 0 Then
            Open Lfolder & lFile For Input As #1
                Line Input #1, lLine
            Close #1
        End If
    End If
    'create error file
    Open Lfolder & Efile For Output As #1
    Close #1
    
    Do
        ProcessFile
    
        'delete file
        Kill Nfolder & Nfile
        'get new file
        Nfile = Dir(Nfolder & "EXITCNSL*")
        If Nfile = "" Then
            
            Exit Do
        End If
    Loop
    PrintError
'<3>    MsgBox "Script Complete!"
    ProcComp "MBSBATEXTCNSL.TXT"
    
    
End Sub

Sub ProcessFile()
Dim LineStr As String
    Open Nfolder & Nfile For Input As #1
    While Not EOF(1)
        ModifiedCompass = False
        Line Input #1, LineStr
        If lLine = "" Then
            'not in recovery mode
            LineStr = Replace(LineStr, """", "")
            LineArr = Split(LineStr, ",")
'<1>
            LineStr = LineArr(31)   'switch kin's first and last name
            LineArr(31) = LineArr(32)
            LineArr(32) = LineStr
            
            LineStr = LineArr(41)   'switch kin's first and last name
            LineArr(41) = LineArr(42)
            LineArr(42) = LineStr
            
            LineStr = LineArr(52)   'switch kin's first and last name
            LineArr(52) = LineArr(53)
            LineArr(53) = LineStr
'</1>
            FixAddress (4) 'address line
            FixAddress (5)  'city line
            CheckLP22
            
        Else
            'checking for recovery record
            If lLine = LineStr Then
                lLine = ""
            End If
        End If
    Wend
    Close #1
End Sub

Sub FixAddress(line As Integer)
    'remove bad characters from address
    LineArr(line) = Replace(LineArr(line), "!", "")
    LineArr(line) = Replace(LineArr(line), "@", "")
    LineArr(line) = Replace(LineArr(line), "#", "APT ")
    LineArr(line) = Replace(LineArr(line), "$", "")
    LineArr(line) = Replace(LineArr(line), "%", "")
    LineArr(line) = Replace(LineArr(line), "^", "")
    LineArr(line) = Replace(LineArr(line), "&", "")
    LineArr(line) = Replace(LineArr(line), "*", "")
    LineArr(line) = Replace(LineArr(line), "(", "")
    LineArr(line) = Replace(LineArr(line), ")", "")
    LineArr(line) = Replace(LineArr(line), "-", "")
    LineArr(line) = Replace(LineArr(line), "+", "")
    LineArr(line) = Replace(LineArr(line), "=", "")
    LineArr(line) = Replace(LineArr(line), "<", "")
    LineArr(line) = Replace(LineArr(line), ">", "")
    LineArr(line) = Replace(LineArr(line), ",", "")
    LineArr(line) = Replace(LineArr(line), ".", "")
    LineArr(line) = Replace(LineArr(line), """", "")
    LineArr(line) = Replace(LineArr(line), ";", "")
    LineArr(line) = Replace(LineArr(line), ":", "")
    LineArr(line) = Replace(LineArr(line), "~", "")
    LineArr(line) = Replace(LineArr(line), "`", "")
    LineArr(line) = Replace(LineArr(line), "?", "")
End Sub

Sub CheckLP22()
    
    OAddr = False
    OPhone = False
    OEmail = False
    
    SP.Q.FastPath "LP22I" & Replace(LineArr(3), """", "")
    'if ssn not found then add to error report and go to next record
    If SP.Q.Check4Text(1, 62, "PERSON DEMOGRAPHICS") = False Then
        Open Lfolder & Efile For Append As #2
            Write #2, LineArr(3), LineArr(0), LineArr(1), LineArr(2)
        Close #2
        Exit Sub
    End If
    'if ssn is found compare name
    If SP.Q.Check4Text(4, 44, Replace(LineArr(0), """", "")) = False Or _
    SP.Q.Check4Text(4, 5, Replace(LineArr(1), """", "")) = False Or _
    SP.Q.Check4Text(4, 72, Replace(Replace(LineArr(12), "/", ""), """", "")) = False Then 'firstname, lastname, Birthday
    'add to error report
        ReDim Preserve ErrorArr(5, UBound(ErrorArr, 2) + 1)
        ErrorArr(0, UBound(ErrorArr, 2)) = LineArr(3) 'ssn
        ErrorArr(1, UBound(ErrorArr, 2)) = LineArr(0) 'first name
        ErrorArr(2, UBound(ErrorArr, 2)) = LineArr(1) 'last name
        ErrorArr(3, UBound(ErrorArr, 2)) = LineArr(2) 'middle ini
        ErrorArr(4, UBound(ErrorArr, 2)) = LineArr(12) 'B-Day
        ErrorArr(5, UBound(ErrorArr, 2)) = "OneLINK" 'system
    End If
    'Determine if the borrower LP22 address, phone number, or email address should be updated
    If CDate(SP.Q.GetText(11, 72, 2) & "/" & SP.Q.GetText(11, 74, 2) & "/" & SP.Q.GetText(11, 76, 4)) < Date - 10 Then
        OAddr = True
    End If
    If CDate(SP.Q.GetText(14, 72, 2) & "/" & SP.Q.GetText(14, 74, 2) & "/" & SP.Q.GetText(14, 76, 4)) < Date - 10 Then
        OPhone = True
    End If
    If SP.Q.GetText(18, 71, 2) & "/" & SP.Q.GetText(18, 73, 2) & "/" & SP.Q.GetText(18, 75, 4) <> "MM/DD/CCYY" Then
        If CDate(SP.Q.GetText(18, 71, 2) & "/" & SP.Q.GetText(18, 73, 2) & "/" & SP.Q.GetText(18, 75, 4)) < Date - 10 Then
            OEmail = True
        End If
    End If
    UpdateLP22
    CheckTX1J
    checkEmployer
    KinRef
    Addcomments
End Sub

Sub UpdateLP22()
    
    'change to change mode
    SP.Q.PutText 1, 7, "C", "ENTER"
    SP.Q.PutText 3, 9, "D"

    
    If OAddr Then
        SP.Q.PutText 11, 9, LineArr(4), "END" 'address
        SP.Q.PutText 12, 9, "", "END"           'address2
        SP.Q.PutText 13, 9, Replace(LineArr(5), """", ""), "END" 'city
        SP.Q.PutText 13, 52, Replace(LineArr(6), """", "")       'state
        SP.Q.PutText 13, 60, Replace(LineArr(7), """", ""), "END" 'zip
        SP.Q.PutText 11, 57, "Y"    'validity indicator
        SP.Q.PutText 11, 72, Format(Date, "MMDDYYYY")   'effective date
    End If
    LineArr(9) = Replace(LineArr(9), """", "")
    If OPhone And LineArr(9) <> "" Then
        SP.Q.PutText 14, 17, Replace(LineArr(9), """", "") 'phone
        SP.Q.PutText 14, 56, "Y"
        SP.Q.PutText 14, 72, Format(Date, "MMDDYYYY")
    End If
    LineArr(10) = Replace(LineArr(10), """", "")
    If OEmail And LineArr(10) <> "" And InStr(1, LineArr(10), "@") <> 0 And InStr(1, LineArr(10), ".") <> 0 Then
        SP.Q.PutText 19, 9, Replace(LineArr(10), """", "") 'email
        SP.Q.PutText 18, 56, "Y"
    End If
    
    SP.Q.Hit "F6"
    SP.Q.Hit "ENTER"
    
    
End Sub

Sub CheckTX1J()
    CAddr = False
    CPhone = False
    CEmail = False
    SP.Q.FastPath "TX3Z/ITX1J;" & Replace(LineArr(3), """", "")
    If SP.Q.Check4Text(1, 71, "TXX1R") = False Then
        Exit Sub
    End If
    'if ssn is found compare name
    If SP.Q.Check4Text(4, 34, Replace(LineArr(0), """", "")) = False Or _
    SP.Q.Check4Text(4, 6, Replace(LineArr(1), """", "")) = False Or _
    SP.Q.Check4Text(4, 53, Replace(LineArr(2), """", "")) = False Or _
    SP.Q.GetText(20, 6, 2) & SP.Q.GetText(20, 9, 2) & SP.Q.GetText(20, 12, 4) <> Replace(Replace(LineArr(12), "/", ""), """", "") Then 'firstname, lastname, middle, Birthday
    'add to error report where name is different
        ReDim Preserve ErrorArr(5, UBound(ErrorArr, 2) + 1)
        ErrorArr(0, UBound(ErrorArr, 2)) = LineArr(3) 'ssn
        ErrorArr(1, UBound(ErrorArr, 2)) = LineArr(0) 'first name
        ErrorArr(2, UBound(ErrorArr, 2)) = LineArr(1) 'last name
        ErrorArr(3, UBound(ErrorArr, 2)) = LineArr(2) 'middle ini
        ErrorArr(4, UBound(ErrorArr, 2)) = LineArr(12) 'B-Day
        ErrorArr(5, UBound(ErrorArr, 2)) = "Compass" 'system
    End If
    'Determine if the borrower TX1J address, phone number, or email address should be updated
    If Trim(Replace(SP.Q.GetText(10, 32, 8), "_", " ")) <> "" Then
        If CDate(Replace(SP.Q.GetText(10, 32, 8), " ", "/")) < Date - 10 Then
            CAddr = True
            ModifiedCompass = True
        End If
    End If
    If Trim(Replace(SP.Q.GetText(16, 32, 8), "_", " ")) <> "" Then
        If CDate(Replace(SP.Q.GetText(16, 32, 8), " ", "/")) < Date - 10 Then
            CPhone = True
            ModifiedCompass = True
        End If
    End If
    SP.Q.Hit "F2"
    SP.Q.Hit "F10"
    If Trim(Replace(SP.Q.GetText(12, 17, 8), "_", " ")) <> "" Then
        If CDate(Replace(SP.Q.GetText(12, 17, 8), " ", "/")) < Date - 10 Then
            CEmail = True
            ModifiedCompass = True
        End If
    End If
    
    UpdateTX1J
End Sub

Sub UpdateTX1J()


    SP.Q.FastPath "TX3Z/CTX1J;" & Replace(LineArr(3), """", "")
    SP.Q.Hit "F6"
    SP.Q.Hit "F6"
    If CAddr Then
        SP.Q.PutText 8, 18, "55"
        SP.Q.PutText 11, 10, LineArr(4), "END"  'address
        SP.Q.PutText 12, 10, "", "END"
        SP.Q.PutText 14, 8, LineArr(5), "END"   'city
        SP.Q.PutText 14, 32, LineArr(6)         'state
        SP.Q.PutText 14, 40, LineArr(7), "END"  'zip
        SP.Q.PutText 11, 55, "Y"    'validity indicator
        SP.Q.PutText 10, 32, Format(Date, "MMDDYY") 'address last updated
        SP.Q.Hit "ENTER"
    End If
    SP.Q.Hit "F6"
    LineArr(9) = Replace(LineArr(9), """", "")
    If CPhone And LineArr(9) <> "" Then
        SP.Q.PutText 17, 14, LineArr(9) 'phone
        SP.Q.PutText 17, 54, "Y"
        SP.Q.PutText 16, 32, Format(Date, "MMDDYY")
        SP.Q.PutText 19, 14, "01" '<4>
        SP.Q.Hit "ENTER"
    End If
    LineArr(10) = Replace(LineArr(10), """", "")
    If OEmail And LineArr(10) <> "" And InStr(1, LineArr(10), "@") <> 0 And InStr(1, LineArr(10), ".") <> 0 Then
        SP.Q.Hit "F2"
        SP.Q.Hit "F10"
        SP.Q.PutText 10, 20, "55"
        SP.Q.PutText 15, 10, LineArr(10), "END" 'email
        SP.Q.PutText 13, 14, "Y"
        SP.Q.PutText 12, 17, Format(Date, "MMDDYY")
        SP.Q.Hit "ENTER"
    End If
End Sub

Sub checkEmployer()
Dim x As Integer
Dim LS As String 'loan status
Dim RC As String 'reason code
Dim comment As String
Dim found As Boolean
    found = False
    If LineArr(24) <> "" Then
        SP.Q.FastPath "LG02I" & Replace(LineArr(3), """", "")
        For x = 0 To 10
            LS = SP.Q.GetText(10 + x, 75, 2)
            RC = SP.Q.GetText(10 + x, 78, 2)
            comment = Replace(LineArr(24), """", "") & "," & Replace(LineArr(25), """", "") & "," & Replace(LineArr(26), """", "") & "," & Replace(LineArr(27), """", "") & "," & Replace(LineArr(28), """", "") & "," & Replace(LineArr(29), """", "") & "," & Replace(LineArr(30), """", "")
            If (LS = "CP" Or LS = "CR") And (RC = "DF" Or RC = "DB" Or RC = "DQ") Then
                'create queue task DEMPA
                found = True
                Exit For
            End If
            If x = 10 Then
                SP.Q.Hit "F8"
                x = 0
                If SP.Q.Check4Text(22, 3, "46004") Then
                    Exit For
                End If
            End If
        Next x
        
        If found Then
            SP.Common.AddLP9O Replace(LineArr(3), """", ""), "DEMPA", , comment
        Else
            SP.Common.AddLP50 Replace(LineArr(3), """", ""), "MEMPL", "BATEXTCNSL", "MS", "02", comment
        End If
        
    End If
End Sub

Sub KinRef()
'add a queue for each Kin, Ref1, Ref2 provided
Dim Kin As String
Dim Ref1 As String
Dim Ref2 As String
FixAddress (34)
FixAddress (44)
FixAddress (55)

Kin = Replace(LineArr(32) & "," & LineArr(33) & "," & LineArr(31) & "," & "Exit Interview" & ",*" & LineArr(40) & "," & LineArr(34) & "," & LineArr(35) & "," & LineArr(36) & "," & LineArr(37) & "," & LineArr(39), """", "")
Ref1 = Replace(LineArr(42) & "," & LineArr(43) & "," & LineArr(41) & "," & "Exit Interview" & ",*" & LineArr(51) & "," & LineArr(44) & "," & LineArr(45) & "," & LineArr(46) & "," & LineArr(47) & "," & LineArr(49), """", "")
Ref2 = Replace(LineArr(53) & "," & LineArr(54) & "," & LineArr(52) & "," & "Exit Interview" & ",*" & LineArr(62) & "," & LineArr(55) & "," & LineArr(56) & "," & LineArr(57) & "," & LineArr(58) & "," & LineArr(60), """", "")

If Replace(LineArr(32), """", "") <> "" Then
    SP.Common.AddLP9O Replace(LineArr(3), """", ""), "MREFRADD", , Kin
End If
If Kin <> Ref1 Then 'check for duplicate            '<2>
    If Replace(LineArr(42), """", "") <> "" Then
        SP.Common.AddLP9O Replace(LineArr(3), """", ""), "MREFRADD", , Ref1
    End If
End If                                              '<2>
If Kin <> Ref2 Then 'check for duplicate            '<2>
    If Replace(LineArr(53), """", "") <> "" Then
        SP.Common.AddLP9O Replace(LineArr(3), """", ""), "MREFRADD", , Ref2
    End If
End If                                              '<2>
End Sub

Sub Addcomments()
Dim found As Boolean
    SP.Q.FastPath "LPSCI" & Format(Replace(LineArr(11), """", ""), "00000000")
    SchName = SP.Q.GetText(4, 20, 20)
    SP.Common.AddLP50 Replace(LineArr(3), """", ""), "GCORR", "BATEXTCNSL", "FO", "08", "Exit Interview from: " & SchName & " processed for borrower."
    Wait (2)
    If ModifiedCompass = True Then
        If SP.Common.ATD22AllLoans(Replace(LineArr(3), """", ""), "GCORR", "Exit Interview from: " & SchName & " processed for borrower.", "BATEXTCNSL", User) = False Then
                'if ATD22 not added try ATD37
                SP.Q.FastPath "TX3Z/ATD37" & "229135282"
                    found = False
                    'find the ARC
                    Do
                        found = Session.FindText("GCORR", 8, 8)
                        If found Then Exit Do
                        Hit "F8"
                        If Check4Text(23, 2, "90007") Then
                            Exit Sub
                        End If
                    Loop
                    'select the ARC
                    SP.Q.PutText Session.FoundTextRow, Session.FoundTextColumn - 5, "01", "ENTER"
                     'check if loan found
                    If Check4Text(23, 2, "50108") Then
                        Exit Sub
                    End If
                
                    SP.Q.PutText 11, 18, "X"
                    SP.Q.PutText 21, 2, "Exit Interview from: " & SchName & " processed for borrower.", "ENTER"
                    
        End If
    End If
End Sub

Sub PrintError()
Dim Space(4) As String
Dim x As Integer
Dim x2 As Integer
    If UBound(ErrorArr, 2) > 0 Then
        'ErrorArr(5, 0)
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")
        'open form doc
        Word.Visible = False
        Word.Documents.Add DocumentType:=wdNewBlankDocument
        'add a header
        Word.Selection.Font.Size = 20
        Word.Selection.TypeText TEXT:="Key Loan Identifier Discrepancies Report"
        Word.Selection.TypeParagraph
        Word.Selection.TypeParagraph
        Word.Selection.Font.Size = 12
        For x = 1 To UBound(ErrorArr, 2)
            
            
            Word.Selection.TypeText TEXT:="Borrower SSN: " & Replace(ErrorArr(0, x), """", "")
            Word.Selection.TypeParagraph
            Word.Selection.TypeText TEXT:="First Name:      " & Replace(ErrorArr(1, x), """", "")
            Word.Selection.TypeParagraph
            Word.Selection.TypeText TEXT:="Last Name:       " & Replace(ErrorArr(2, x), """", "")
            Word.Selection.TypeParagraph
            Word.Selection.TypeText TEXT:="Middle Initial:   " & Replace(ErrorArr(3, x), """", "")
            Word.Selection.TypeParagraph
            Word.Selection.TypeText TEXT:="Birth Date:       " & Replace(ErrorArr(4, x), """", "")
            Word.Selection.TypeParagraph
            Word.Selection.TypeText TEXT:="System:            " & Replace(ErrorArr(5, x), """", "")
            Word.Selection.TypeParagraph
            Word.Selection.TypeParagraph
            
            
        Next x
        'C:\Windows\Temp\KeyLoanIdentifierDiscrepanciesRPT.txt
        Word.ActiveDocument.SaveAs FileName:="C:\Windows\Temp\KeyLoanIdentifierDiscrepanciesRPT.doc", _
            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False
        Session.Wait 2
        Word.ActiveDocument.PrintOut
        Word.Application.Quit SaveChanges:=wdDoNotSaveChanges
    End If
End Sub

'<new> sr1067, tp, 05/10/05
'<1> sr1205, tp, 07/08/05, 07/11/05
'<2> sr1501, aa
'<3> sr1540, tp, 05/17/06 Removed prompts to prepare script for MBS
'<4> sr2181, sb, COMPASS Screen Change


