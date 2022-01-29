Dim Schls() As String       'school which have received letter from KLSLT comment
Dim s As Integer            'number of schools which have recieved letter
Dim NewSchls() As String    'new schools from LG02
Dim ns As Integer           'number of new schools from LG02
Dim LtrSchls() As String    'all schools on LG29 which may need a letter
Dim ls As Integer           'number of schools which may need a letter
Dim MsgText As String
Dim Comment As String
Dim Row As Integer
Dim Phone As String
Dim AltPhone As String
Dim Email As String
Type StudentInfo
    SSN As String
    Name As String
    Address1 As String
    Address2 As String
    Address3 As String
    CityStateZip As String
    Country As String
    Phone As String
End Type
Dim StuInfo() As StudentInfo
Dim sn As Integer           'number of students for PLUS only borrowers
Dim PLOnly As String        ''Y' indicates the borrower only has PLUS loans
Dim hasTILP As Boolean      'indicates the borrower has TILP loans
Dim j As Integer            'for next loop counter
Dim sDup As String          'indicates duplicate students

Dim School As String
Dim SchName As String
Dim SchAdd As String
Dim SchAdd2 As String
Dim SchAdd3 As String
Dim SchCity As String
Dim SchST As String
Dim SchZip As String
Dim AllStopPursuit As Boolean

Private SSN As String
Private UserID As String
Private DocPath As String
Private Script As String
Private Queue As String
Private Doc As String

'work tasks in the I1 and I2 COMPASS queues
Sub I1I2ClrhsRev()
    With Session
        Queue = "I2"
        'process the tasks
        Do

            'end the script if the user clicked cancel
            If Okay = 0 Then
                Unload I1I2Clrhs
                End
            'update borrower demographics if the user clicked Locate
            ElseIf Okay = 1 Then
                'update LP22
                FastPath "LP22C" & SSN
                PutText 3, 9, "M"
                ClearAndPutText 11, 9, I1I2Clrhs.Address1
                ClearAndPutText 12, 9, I1I2Clrhs.Address2
                ClearAndPutText 13, 9, I1I2Clrhs.City
                PutText 13, 52, I1I2Clrhs.State
                ClearAndPutText 13, 60, I1I2Clrhs.ZIP
                PutText 11, 57, "Y"
                Hit "ENTER"
                If Not Check4Text(22, 3, "46012") Then
                    Wait "1"
                End If
                If Not Check4Text(22, 3, "46012") Then
                    MsgBox "Fix the problem and then press <Insert> to continue.", vbOKOnly + vbExclamation, "Fix Problem"
                    sp.PauseForInsert
                End If
                Hit "F6"
                
                'add activity record to LP50
                sp.Common.AddLP50 SSN, "KGNRL", Script, "AM", "96", "successful ch review"
                
                'update TX1J
                .TransmitTerminalKey rcIBMClearKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitANSI "TX3Z/CTX1JB;" & SSN
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitTerminalKey rcIBMPf6Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitTerminalKey rcIBMPf6Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                'source & 3rd party
                .MoveCursor 8, 18
                .TransmitANSI "0104"
                'last ver date
                .MoveCursor 10, 32
                .TransmitANSI Format(Date, "MMDDYY")
                'address 1
                .MoveCursor 11, 10
                .TransmitANSI I1I2Clrhs.Address1
                If Len(I1I2Clrhs.Address1) < 30 Then .TransmitTerminalKey rcIBMEraseEOFKey
                'validity indicator
                .MoveCursor 11, 55
                .TransmitANSI "Y"
                'address 2
                .MoveCursor 12, 10
                .TransmitANSI I1I2Clrhs.Address2
                If Len(I1I2Clrhs.Address2) < 30 Then .TransmitTerminalKey rcIBMEraseEOFKey
                'City
                .MoveCursor 14, 8
                .TransmitANSI I1I2Clrhs.City
                If Len(I1I2Clrhs.City) < 20 Then .TransmitTerminalKey rcIBMEraseEOFKey
                'Domestic State
                If I1I2Clrhs.Country = "" Then
                    .MoveCursor 14, 32
                    .TransmitANSI I1I2Clrhs.State
                    .MoveCursor 13, 52
                    .TransmitTerminalKey rcIBMEraseEOFKey
                'Foreign State and Country
                Else
                    .MoveCursor 12, 52
                    .TransmitANSI Mid(I1I2Clrhs.State, 1, 15)
                    If Len(I1I2Clrhs.State) < 15 Then .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 14, 32
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 13, 52
                    .TransmitANSI Mid(I1I2Clrhs.Country, 1, 25)
                    If Len(I1I2Clrhs.Country) < 25 Then .TransmitTerminalKey rcIBMEraseEOFKey
                End If
                'ZIP
                .MoveCursor 14, 40
                .TransmitANSI I1I2Clrhs.ZIP
                If Len(I1I2Clrhs.ZIP) < 17 Then .TransmitTerminalKey rcIBMEraseEOFKey
                'commit changes
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                'add an activity record
                ATD22 "KS2CH", ""
                FastPath "TX3ZITS24" & SSN
                If Check4Text(13, 72, " VALID ") = False Then
                    CreateKLSLTSchoolLtr
                End If
            'send a letter to the school or add a queue task if the borrower was not found on clearinghouse
            ElseIf Okay = 3 Then
                CreateKLSLTSchoolLtr
            'add and activity record if the borrower was found on clearinghouse but not located
            ElseIf Okay = 2 Then
                ATD22 "KU2CH", ""
                FastPath "TX3ZITS24" & SSN
                If Check4Text(13, 72, " VALID ") = False Then CreateKLSLTSchoolLtr
            End If
CompTask:   Unload I1I2Clrhs
        'complete the task
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "TX3Z/ITX6X" & Queue
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'select the first task
            .TransmitANSI "01"
            .TransmitTerminalKey rcIBMPf2Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "CCOMPL"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'prompt the user to continue
            warn = MsgBox("Task complete.  Click OK to work another task or Cancel to quit.", vbOKCancel, "Task Complete")
            If warn <> 1 Then
                End
            End If
        Loop
    End With
End Sub

'get the schools for new loans (guaranteed within past 30 days)
Function LG02() As String
    With Session
        LG02 = "N"
        'access LG02
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LG02I" & SSN
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'select the first loan if the selection screen is displayed
        If .GetDisplayText(21, 3, 3) = "SEL" Then
            .TransmitANSI "01"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'exit the function if the borrower/student is not found
        ElseIf .GetDisplayText(5, 22, 3) = "STU" Then
            Exit Function
        End If
        'get the school for each loan
        ns = 0
        Do Until .GetDisplayText(22, 3, 5) = "46004"
            found = .FindText("DT PROC", 1, 1)
            'get the school if the loan was guaranteed within the past 30 days and it is not a consolidation loan
            If DateValue(Format(.GetDisplayText(.FoundTextRow, .FoundTextColumn + 8, 8), "##/##/####")) >= DateValue(Date) - 30 And .GetDisplayText(1, 56, 30) <> "CON" Then
                ns = ns + 1
                ReDim Preserve NewSchls(ns) As String
                found = .FindText("SCH", 1, 1)
                NewSchls(ns) = Trim(.GetDisplayText(.FoundTextRow, .FoundTextColumn + 5, 10))
                LG02 = "Y"
            End If
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Loop
    End With
End Function

'determine the school is already in the array
Function LG29(Row As Integer) As String
    LG29 = "N"
    For I = 0 To ls
    On Error GoTo DimLtrSchls
        If LtrSchls(I) = Session.GetDisplayText(Row, 14, 8) Then
            LG29 = "Y"
            Exit For
        End If
    Next I
    Exit Function
DimLtrSchls:
End Function

'determine the school is already in the array
Function TS26() As String
    TS26 = "N"
    For I = 0 To ls
    On Error GoTo DimLtrSchls
        If LtrSchls(I) = Session.GetDisplayText(13, 18, 8) Then
            TS26 = "Y"
            Exit For
        End If
    Next I
    Exit Function
DimLtrSchls:
End Function

'determine if the school is in the array, it not, it needs a letter
Function NeedsLtr(SchCd As String) As String
    NeedsLtr = "Y"
    For j = 1 To s
        If SchCd = Schls(j) Then
            NeedsLtr = "N"
            Exit For
        End If
    Next j
End Function

'get school information
Sub GetSchInfo(School As String)
    With Session
        'access LPSC
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        .TransmitANSI "LPSCI" & School
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        'select dept 112 if it is displayed
        If .GetDisplayText(21, 3, 3) = "SEL" Then
            'find the row for dept 112
            Row = 7
            Do Until .GetDisplayText(Row, 7, 3) = "112"
                Row = Row + 1
                'go to the next page if the bottom of the page is reached
                If Row = 19 Then
                    .TransmitTerminalKey rcIBMPf8Key
                    .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
                    Row = 7
                    'use the general address if dept 112 is not found
                    If .GetDisplayText(22, 3, 5) = "46004" Then Exit Do
                End If
            Loop
            'select the row
            .TransmitANSI .GetDisplayText(Row, 2, 2)
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        End If
        'get the information
        SchName = Trim(.GetDisplayText(5, 21, 40))
        SchAdd = Trim(.GetDisplayText(8, 21, 30))
        If .GetDisplayText(9, 21, 1) <> " " Then SchAdd2 = Trim(.GetDisplayText(9, 21, 30)) Else SchAdd2 = ""
        If .GetDisplayText(10, 21, 1) <> " " Then SchAdd3 = Trim(.GetDisplayText(10, 21, 30)) Else SchAdd3 = ""
        SchCity = Trim(.GetDisplayText(11, 21, 30))
        SchST = .GetDisplayText(11, 59, 2)
        SchZip = .GetDisplayText(11, 66, 5)
        If .GetDisplayText(11, 71, 1) <> " " Then SchZip = SchZip & "-" & .GetDisplayText(11, 71, 4)
    End With
End Sub

'add an activity record
Sub ATD22(ARC As String, Msg As String)
    With Session
        'access TD22
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "TX3Z/ATD22" & SSN
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'find the ARC
        Do
            found = .FindText(ARC, 8, 8)
            If found Then Exit Do
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If .GetDisplayText(23, 2, 5) = "90007" Then
                MsgBox "ARC " & ARC & " was not found.  Contact Systems Support for assistance.", , "ARC not Found"
                End
            End If
        Loop
        'select the ARC
        .MoveCursor .FoundTextRow, .FoundTextColumn - 5
        .TransmitANSI "01"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'select the loans
        Do
            .MoveCursor 11, 3
            .TransmitANSI "XXXXXXXX"
            If .GetDisplayText(8, 75, 1) <> "+" Then Exit Do
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Loop
        'erase extra Xs
        .MoveCursor 21, 2
        .TransmitTerminalKey rcIBMEraseEOFKey
        'access expanded comment screen
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitTerminalKey rcIBMPf4Key
        'add the comment and save the changes
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI Msg & " / " & UserID & "  " & Script
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
    End With
End Sub

'get schools the borrower or students attended and generate letters for each
Sub SchoolLetters(SSNo As String, s As Integer, PL As String)
    With Session
        'access LG29
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LG29I" & SSNo
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        If Check4Text(1, 51, "STUDENT ENROLLMENT STATUS MENU") = False Then 'check to be sure they are on OneLINK
            If .GetDisplayText(20, 8, 3) = "SEL" Then
                Row = 9
                Do
                    'add the school to the array (list of schools to get letters) if it is no already there
                    If LG29(Row) = "N" Then
                        ReDim Preserve LtrSchls(ls + 1)
                        LtrSchls(ls) = .GetDisplayText(Row, 14, 8)
                        ls = ls + 1
                    End If
                    Row = Row + 1
                    If .GetDisplayText(Row, 6, 1) = " " Then
                        .TransmitTerminalKey rcIBMPf8Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        If .GetDisplayText(22, 3, 5) = "46004" Then Exit Do
                        Row = 9
                    End If
                Loop
            ElseIf .GetDisplayText(1, 74, 3) = "DIS" Then
                ls = ls + 1
                ReDim Preserve LtrSchls(ls + 1)
                LtrSchls(ls) = .GetDisplayText(10, 71, 8)
            End If
        End If
        'get info for schools that haven't received letters if there are any
        If ls <> 0 Then
            For I = 0 To UBound(LtrSchls) - 1
                Comment = Comment & " " & LtrSchls(I)
                GetSchInfo LtrSchls(I)
                Open "T:\schltrdat.txt" For Output As #2
                Write #2, "SSN", "Queue", "FirstName", "MI", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "Phone", "AltPhone", "Email", "School", "SchName", "SchAdd", "SchAdd2", "SchAdd3", "SchCity", "SchST", "SchZip", "sSSN", "sName", "sAddress1", "sAddress2", "sAddress3", "sCitySTZIP", "sCountry", "sPhone"
''                Write #2, Format(SSN, "@@@-@@-@@@@"), Queue, I1I2Clrhs.BorrowerName, "", "", UCase(I1I2Clrhs.Address1), UCase(I1I2Clrhs.Address2), UCase(I1I2Clrhs.City), UCase(I1I2Clrhs.State), UCase(I1I2Clrhs.ZIP), UCase(I1I2Clrhs.Country), Phone, AltPhone, Email, LtrSchls(I), SchName, SchAdd, SchAdd2, SchAdd3, SchCity, SchST, SchZip, StuInfo(s).SSN, StuInfo(s).Name, StuInfo(s).Address1, StuInfo(s).Address2, StuInfo(s).Address3, StuInfo(s).CityStateZip, StuInfo(s).Country, StuInfo(s).Phone
                Write #2, "XXXXX" & Mid(SSN, 6, 4), Queue, I1I2Clrhs.BorrowerName, "", "", UCase(I1I2Clrhs.Address1), UCase(I1I2Clrhs.Address2), UCase(I1I2Clrhs.City), UCase(I1I2Clrhs.State), UCase(I1I2Clrhs.ZIP), UCase(I1I2Clrhs.Country), Phone, AltPhone, Email, LtrSchls(I), SchName, SchAdd, SchAdd2, SchAdd3, SchCity, SchST, SchZip, "XXXXX" & Mid(StuInfo(s).SSN, 6, 4), StuInfo(s).Name, StuInfo(s).Address1, StuInfo(s).Address2, StuInfo(s).Address3, StuInfo(s).CityStateZip, StuInfo(s).Country, StuInfo(s).Phone
                Close #2
                'print cover letter
                Doc = "SCHLTRCVR" & PL
                sp.Common.PrintDocs DocPath, Doc, "T:\schltrdat.txt"
                'print detail
                Doc = "SCHLTRLST" & PL
                sp.Common.PrintDocs DocPath, Doc, "T:\schltrdat.txt"
                Kill "T:\schltrdat.txt"
            Next I
        End If
    End With
End Sub

'gets Student SSNs from TS26
Function GetStudentSSNFromTS26()
    Dim Row As Integer
    FastPath "TX3ZITS26" & SSN
    If Check4Text(1, 72, "TSX28") Then
       'selection screen
       Row = 8
       'search from all PLUS loans with positive balance
       While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            If Check4Text(Row, 19, "PLUS") And Check4Text(Row, 69, "CR") = False And Check4Text(Row, 64, " 0.00") = False Then
                'target loan
                If Len(GetText(Row, 2, 3)) = 1 Then
                    PutText 21, 12, "0" & GetText(Row, 2, 3), "Enter"
                Else
                    PutText 21, 12, GetText(Row, 2, 3), "Enter"
                End If
                If DupNotFound(GetText(7, 15, 9)) Then
                    'get Student SSN
                    StuInfo(UBound(StuInfo)).SSN = GetText(7, 15, 9)
                    ReDim Preserve StuInfo(UBound(StuInfo) + 1) 'add another place to the array
                End If
                Hit "F12"
            End If
            Row = Row + 1
            'check if end of screen is found
            If Check4Text(Row, 67, "  ") Then
                Row = 8
                Hit "F8" 'page forward
            End If
       Wend
    Else
       'target screen
        'get Student SSN
        StuInfo(UBound(StuInfo)).SSN = GetText(7, 15, 9)
        ReDim Preserve StuInfo(UBound(StuInfo) + 1) 'add another place to the array
    End If
End Function

'this function checks for a duplicate in the array
Function DupNotFound(SSN As String) As Boolean
    Dim I As Integer
    While I < UBound(StuInfo)
        If StuInfo(I).SSN = SSN Then
            DupNotFound = False
            Exit Function
        End If
        I = I + 1
    Wend
    DupNotFound = True
End Function

'gathers student demo info from TX1J
Sub GatherStudentDemoInfoFromTX1J()
    Dim I As Integer
    Dim FN As String
    Dim LN As String
    Dim City As String
    Dim ST As String
    Dim ZIP As String
    While I < UBound(StuInfo)
        sp.Common.GetTX1J StuInfo(I).SSN, , LN, , FN, StuInfo(I).Address1, StuInfo(I).Address2, StuInfo(I).Address3, City, ST, ZIP, StuInfo(I).Country, , StuInfo(I).Phone
        StuInfo(I).Name = FN & " " & LN
        StuInfo(I).CityStateZip = City & ", " & ST & ", " & ZIP
        I = I + 1
    Wend
End Sub

Sub CreateKLSLTSchoolLtr()
    Dim SkpBeginDt As String
    Dim I As Integer
    
    FastPath "TX3ZITD3G" & SSN
    SkpBeginDt = GetText(20, 5, 8)
    'if blank skip begin date then assume current date
    If SkpBeginDt = "" Then SkpBeginDt = Format(Date, "MM/DD/YY")
    'access TD2A
    FastPath "TX3Z/ITD2A" & SSN
    'enter ARC to search for
    PutText 11, 65, "KLSLT"
    PutText 21, 16, Replace(SkpBeginDt, "/", "")
    PutText 21, 30, Format(Date, "MMDDYY"), "Enter"
    'get the schools if the ARC is found
    If Check4Text(1, 72, "TDX2B") = False Then
        Exit Sub 'if ARC is found then do nothing and complete task
    ElseIf Not AllStopPursuit Then
        Comment = "letter(s) mailed to school(s):"
        'process letters for each plus only borrower student
        If PLOnly = "Y" Then
            For j = 0 To sn
                SchoolLetters StuInfo(j).SSN, j, "PL"
            Next j
        'process letters for borrowers
        Else
            ReDim StuInfo(0)
            SchoolLetters SSN, 0, ""
        End If
        'add a comment if there is one or warn the user if there are no letters
        If Comment = "letter(s) mailed to school(s):" Then
            MsgBox "No schools were found on LG29 or no schools were found to which letters have not already been sent so no school letters will be generated.", , "No Schools on LG29"
        Else
            Comment = Comment & " to request verification of borrower's demographic information"
            ATD22 "KLSLT", Comment
        End If
    End If
End Sub

'clear the field and enter the specified text
Function ClearAndPutText(Row As Integer, Col As Integer, Text As String)
    Session.MoveCursor Row, Col
    Hit "END"
    PutText Row, Col, Text
End Function
