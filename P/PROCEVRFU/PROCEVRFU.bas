Attribute VB_Name = "PROCEVRFU"
'Process EVR Followup Tasks

Private Info() As String
Private SchComas() As String
Private SSN As String
Private InsID As String
Private Name As String
Private LogFolder As String
Private lSchID As String

Sub Main()
    If MsgBox("This script will process tasks in the EVSNTFLU queue.", vbOKCancel + vbInformation, "EVR Follow up Script") = vbCancel Then End
    ReDim Info(12, 0)
    ReDim SchComas(0)
    Check4Recovery
    getQ
End Sub

'check for unfaxed records
Sub Check4Recovery()
    lSchID = ""
    SP.Common.TestMode , , LogFolder
    
    'get ID of school being processed if script was faxing when it went down
    If Dir(LogFolder & "evrfulog.txt") <> "" Then
        Open LogFolder & "evrfulog.txt" For Input As #8
        Input #8, lSchID
        Close #8
    End If

    'read contents of recovery file into array
    If Dir("T:\evrfudat.txt") <> "" Then
        Open "T:\evrfudat.txt" For Input As #3
        While Not EOF(3)
            ReDim Preserve Info(12, UBound(Info, 2) + 1)
            Input #3, Info(0, UBound(Info, 2)), Info(1, UBound(Info, 2)), Info(2, UBound(Info, 2)), Info(3, UBound(Info, 2)), Info(4, UBound(Info, 2)), Info(5, UBound(Info, 2)), Info(6, UBound(Info, 2)), Info(7, UBound(Info, 2)), Info(8, UBound(Info, 2)), Info(9, UBound(Info, 2)), Info(11, UBound(Info, 2)), Info(12, UBound(Info, 2))
        Wend
        Close #3
    End If
    
    'warn the user if there are records that need to be faxed
    If lSchID <> "" Then
        MsgBox "The script was unable to finish faxing all of the EVRs the last time it was run.  The remaining faxes will now be sent.  When processing is complete, please run the script again to work more queue tasks.", vbExclamation, "Recovery Mode"
        PrintEVR
    End If

End Sub

Sub getQ()
    SP.q.FastPath "LP9ACEVSNTFLU"
    If SP.q.Check4Text(1, 71, "QUEUE TASK") Then
        'found queue tasks
        SSN = SP.q.GetText(17, 70, 9)
        InsID = SP.q.GetText(6, 57, 8)
        SP.q.PauseForInsert
        SP.q.FastPath "LG29I" & SSN & InsID
        'MsgBox "Press Insert when done."
        SP.q.PauseForInsert
        SP.q.FastPath "LP50I" & SSN
        SP.q.PutText 5, 14, "X"
        SP.q.Hit "ENTER"
        Name = SP.q.GetText(2, 2, 40) 'get student name
        'MsgBox "Press Insert when done."
        SP.q.PauseForInsert
        If MsgBox("Do you want to send another EVR for this borrower?", vbYesNo + vbQuestion, "EVR Followup") = vbYes Then
        'yes
            EVRYes
        Else
        'no
            EVRNo
        End If
    Else
        'no queue tasks found
        If UBound(Info, 2) > 0 Then
            PrintEVR
        Else
            MsgBox "There are no tasks to process.  Processing complete.", vbInformation, "Processing Complete"
            End
        End If
    End If
End Sub

Sub EVRYes()
    Dim Msg As String   'message for the letter.
    Dim row As Integer
    Dim found As Boolean '= true if 112 is found for school department
    Dim changed As Boolean '=true if school info was changed
    Dim dup As Boolean
    Dim OldSch As String
    
    Msg = InputBox("Enter the specific timeframe of enrollment below or click OK to request the complete enrollment history.", "EVR Followup")
    If Len(Msg) > 50 Then
        Msg = Mid(Msg, 1, 50)
    End If
    If Len(Msg) = 0 Then Msg = "Please list the complete enrollment history for the student referenced above."
    
    SP.q.FastPath "LPSCI" & InsID
    row = 0
    'locate school department 112
    If SP.q.Check4Text(1, 57, "INSTITUTION DEMOGRAPHICS") Then
    'target screen
        If SP.q.Check4Text(3, 15, "112") Then found = True
    Else
        Do
            If SP.q.Check4Text(7 + row, 7, "112") Then
                SP.q.PutText 21, 13, SP.q.GetText(7 + row, 2, 2)
                SP.q.Hit "ENTER"
                found = True
                Exit Do
            End If
            row = row + 2
            If row > 12 Or SP.q.GetText(7 + row, 2, 2) = "" Then
                SP.q.Hit "F8"
                If SP.q.Check4Text(22, 3, "46004") Then
                    found = False
                    Exit Do
                End If
            End If
        Loop
    End If
    If SP.q.Check4Text(1, 57, "INSTITUTION DEMOGRAPHICS") Then
        'found 112, get school info
        ReDim Preserve Info(12, UBound(Info, 2) + 1)
        Info(0, UBound(Info, 2)) = InsID
        Info(1, UBound(Info, 2)) = SP.q.GetText(5, 21, 30) 'school
        Info(2, UBound(Info, 2)) = SP.q.GetText(8, 21, 40) 'addr
        Info(3, UBound(Info, 2)) = SP.q.GetText(9, 21, 40) 'addr2
        Info(4, UBound(Info, 2)) = SP.q.GetText(10, 21, 40) 'addr3
        Info(5, UBound(Info, 2)) = SP.q.GetText(11, 21, 30) 'city
        Info(6, UBound(Info, 2)) = SP.q.GetText(11, 59, 2) 'state
        If SP.q.GetText(12, 21, 15) <> "" Then
            'get foreign state if it exists
            Info(6, UBound(Info, 2)) = SP.q.GetText(12, 21, 15) 'foreign state
        End If
        Info(7, UBound(Info, 2)) = SP.q.GetText(11, 66, 14) 'zip
        Info(8, UBound(Info, 2)) = SP.q.GetText(12, 55, 26) 'country
        Info(9, UBound(Info, 2)) = SP.q.GetText(15, 70, 10) 'fax
        If SP.q.GetText(16, 63, 17) <> "" Then
            Info(9, UBound(Info, 2)) = SP.q.GetText(16, 63, 17) 'foreign fax
        End If
        Info(10, UBound(Info, 2)) = Msg 'comment
        Info(11, UBound(Info, 2)) = SSN 'student ssn
        Info(12, UBound(Info, 2)) = Name 'student name
    End If
    If checkComas And checkSch = False Then
        'if school info contains comas and has not been added to the list yet then add it.
        ReDim Preserve SchComas(UBound(SchComas) + 1)
        SchComas(UBound(SchComas)) = InsID
    End If
    
    frmEVRFollowup.txtschid = Info(0, UBound(Info, 2))
    frmEVRFollowup.txtschool = Info(1, UBound(Info, 2))
    frmEVRFollowup.txtadd1 = Info(2, UBound(Info, 2))
    frmEVRFollowup.txtadd2 = Info(3, UBound(Info, 2))
    frmEVRFollowup.txtadd3 = Info(4, UBound(Info, 2))
    frmEVRFollowup.txtCity = Info(5, UBound(Info, 2))
    frmEVRFollowup.txtState = Info(6, UBound(Info, 2))
    frmEVRFollowup.txtZip = Info(7, UBound(Info, 2))
    frmEVRFollowup.txtCountry = Info(8, UBound(Info, 2))
    frmEVRFollowup.txtfax = Info(9, UBound(Info, 2))
    frmEVRFollowup.txtSSN = Info(11, UBound(Info, 2))
    frmEVRFollowup.txtName = Info(12, UBound(Info, 2))
   
    If found = False Then MsgBox "Please fill out the School Eligibility form and submit it to Systems Support.", vbExclamation, "Submit Information to Systems Support"
    frmEVRFollowup.Show
    changed = False
    If frmEVRFollowup.txtschid <> Info(0, UBound(Info, 2)) Or _
    frmEVRFollowup.txtschool <> Info(1, UBound(Info, 2)) Or _
    frmEVRFollowup.txtadd1 <> Info(2, UBound(Info, 2)) Or _
    frmEVRFollowup.txtadd2 <> Info(3, UBound(Info, 2)) Or _
    frmEVRFollowup.txtadd3 <> Info(4, UBound(Info, 2)) Or _
    frmEVRFollowup.txtCity <> Info(5, UBound(Info, 2)) Or _
    frmEVRFollowup.txtState <> Info(6, UBound(Info, 2)) Or _
    frmEVRFollowup.txtZip <> Info(7, UBound(Info, 2)) Or _
    frmEVRFollowup.txtCountry <> Info(8, UBound(Info, 2)) Or _
    frmEVRFollowup.txtfax <> Info(9, UBound(Info, 2)) Or _
    frmEVRFollowup.txtSSN <> Info(11, UBound(Info, 2)) Or _
    frmEVRFollowup.txtName <> Info(12, UBound(Info, 2)) Then
        changed = True 'school data has been changed
    End If
    
    If found = True And changed = True Then
        MsgBox "Please fill out the School IDEM Change Form and submit it to Systems Support.", vbExclamation, "Submit Information to Systems Support"
    End If
    If changed Then
    'save new info if changed
        Info(0, UBound(Info, 2)) = frmEVRFollowup.txtschid
        Info(1, UBound(Info, 2)) = frmEVRFollowup.txtschool
        Info(2, UBound(Info, 2)) = frmEVRFollowup.txtadd1
        Info(3, UBound(Info, 2)) = frmEVRFollowup.txtadd2
        Info(4, UBound(Info, 2)) = frmEVRFollowup.txtadd3
        Info(5, UBound(Info, 2)) = frmEVRFollowup.txtCity
        Info(6, UBound(Info, 2)) = frmEVRFollowup.txtState
        Info(7, UBound(Info, 2)) = frmEVRFollowup.txtZip
        Info(8, UBound(Info, 2)) = frmEVRFollowup.txtCountry
        Info(9, UBound(Info, 2)) = frmEVRFollowup.txtfax
        Info(11, UBound(Info, 2)) = frmEVRFollowup.txtSSN
        Info(12, UBound(Info, 2)) = frmEVRFollowup.txtName
    End If
    
    Open "T:\evrfudat.txt" For Append As #3
    Write #3, Info(0, UBound(Info, 2)), Info(1, UBound(Info, 2)), Info(2, UBound(Info, 2)), Info(3, UBound(Info, 2)), Info(4, UBound(Info, 2)), Info(5, UBound(Info, 2)), Info(6, UBound(Info, 2)), Info(7, UBound(Info, 2)), Info(8, UBound(Info, 2)), Info(9, UBound(Info, 2)), Info(11, UBound(Info, 2)), Info(12, UBound(Info, 2))
    Close #3
        
    SP.Common.AddLP50 SSN, "GEVRS", "PROCEVRFU", "FO", "07", "EVR sent to school code " & Info(0, UBound(Info, 2))
    Wait (2)
    SP.q.FastPath "LP9OA" & SSN & ";;" & "EVSNTFLU"
    SP.q.PutText 9, 25, InsID
    SP.q.PutText 9, 34, "001"
    SP.q.PutText 11, 25, Format(Date + 7, "MMDDYYYY")
    SP.q.Hit "ENTER"
    SP.q.Hit "F6"
    SP.q.FastPath "LP9AC"
    SP.q.Hit "F6"
    
    If MsgBox("Continue to the next task?", vbYesNo + vbQuestion, "EVR Followup") = vbYes Then
    'yes
        getQ
    Else
    'no
        PrintEVR
    End If
    
End Sub

Function checkSch() As Boolean
    'returns true if school is already added to list.
    Dim X As Integer
    For X = 0 To UBound(SchComas)
        If SchComas(X) = InsID Then
            checkSch = True 'contains school already
            Exit Function
        End If
    Next X
    checkSch = False 'does not contain school yet
End Function

Function checkComas() As Boolean
Dim X As Integer
    checkComas = False
    For X = 0 To 12
        If InStr(Info(X, UBound(Info, 2)), ",") <> 0 Then checkComas = True
    Next X
        If InStr(Info(0, UBound(Info, 2)), ",") <> 0 Then Info(0, UBound(Info, 2)) = Replace(Info(0, UBound(Info, 2)), ",", " ")
        If InStr(Info(1, UBound(Info, 2)), ",") <> 0 Then Info(1, UBound(Info, 2)) = Replace(Info(1, UBound(Info, 2)), ",", " ")
        If InStr(Info(2, UBound(Info, 2)), ",") <> 0 Then Info(2, UBound(Info, 2)) = Replace(Info(2, UBound(Info, 2)), ",", " ")
        If InStr(Info(3, UBound(Info, 2)), ",") <> 0 Then Info(3, UBound(Info, 2)) = Replace(Info(3, UBound(Info, 2)), ",", " ")
        If InStr(Info(4, UBound(Info, 2)), ",") <> 0 Then Info(4, UBound(Info, 2)) = Replace(Info(4, UBound(Info, 2)), ",", " ")
        If InStr(Info(5, UBound(Info, 2)), ",") <> 0 Then Info(5, UBound(Info, 2)) = Replace(Info(5, UBound(Info, 2)), ",", " ")
        If InStr(Info(6, UBound(Info, 2)), ",") <> 0 Then Info(6, UBound(Info, 2)) = Replace(Info(6, UBound(Info, 2)), ",", " ")
        If InStr(Info(7, UBound(Info, 2)), ",") <> 0 Then Info(7, UBound(Info, 2)) = Replace(Info(7, UBound(Info, 2)), ",", " ")
        If InStr(Info(8, UBound(Info, 2)), ",") <> 0 Then Info(8, UBound(Info, 2)) = Replace(Info(8, UBound(Info, 2)), ",", " ")
        If InStr(Info(9, UBound(Info, 2)), ",") <> 0 Then Info(9, UBound(Info, 2)) = Replace(Info(9, UBound(Info, 2)), ",", " ")
        If InStr(Info(10, UBound(Info, 2)), ",") <> 0 Then Info(10, UBound(Info, 2)) = Replace(Info(10, UBound(Info, 2)), ",", " ")
        If InStr(Info(11, UBound(Info, 2)), ",") <> 0 Then Info(11, UBound(Info, 2)) = Replace(Info(11, UBound(Info, 2)), ",", " ")
        If InStr(Info(12, UBound(Info, 2)), ",") <> 0 Then Info(12, UBound(Info, 2)) = Replace(Info(12, UBound(Info, 2)), ",", " ")
       
End Function

Sub sendEMail()
    If UBound(SchComas) > 0 Then
        Dim BodyTxt As String
        Dim X As Integer
        'create body text
        BodyTxt = "The EVR Followup script has been run and has found the following School's information to contain commas."
        For X = 0 To UBound(SchComas)
            BodyTxt = BodyTxt & vbCrLf & SchComas(X)
        Next X
        SP.Common.SendMail SP.Common.BSYSRecips("PROCEVRFU"), , "EVR Followup", BodyTxt
    End If
End Sub

Sub EVRNo()
    SP.Common.AddLP50 SSN, "GEVRN", "PROCEVRFU", "MS", "10", , True
    Wait (2)
    SP.q.FastPath "LP9AC"
    SP.q.Hit "F6"
    If MsgBox("Continue to the next task?", vbYesNo + vbQuestion, "EVR Followup") = vbYes Then
    'yes
        getQ
    ElseIf UBound(Info, 2) > 0 Then
    'no
        PrintEVR
    Else
        MsgBox "There are no EVRs to fax.  Processing complete.", vbInformation, "Processing Complete"
        End
    End If
End Sub

Sub sortEVR()
'sort the EVR Info array
Dim T0 As String
Dim T1 As String
Dim T2 As String
Dim T3 As String
Dim T4 As String
Dim T5 As String
Dim T6 As String
Dim T7 As String
Dim T8 As String
Dim T9 As String
Dim T10 As String
Dim T11 As String
Dim T12 As String
Dim X As Integer
Dim Y As Integer

For X = 1 To UBound(Info, 2) - 1
    For Y = X + 1 To UBound(Info, 2)
        If Info(0, X) < Info(0, Y) Then
            T0 = Info(0, X) 'school id
            T1 = Info(1, X)
            T2 = Info(2, X)
            T3 = Info(3, X)
            T4 = Info(4, X)
            T5 = Info(5, X)
            T6 = Info(6, X)
            T7 = Info(7, X)
            T8 = Info(8, X)
            T9 = Info(9, X)
            T10 = Info(10, X)
            T11 = Info(11, X)
            T12 = Info(12, X)
            
            Info(0, X) = Info(0, Y) 'school id
            Info(1, X) = Info(1, Y)
            Info(2, X) = Info(2, Y)
            Info(3, X) = Info(3, Y)
            Info(4, X) = Info(4, Y)
            Info(5, X) = Info(5, Y)
            Info(6, X) = Info(6, Y)
            Info(7, X) = Info(7, Y)
            Info(8, X) = Info(8, Y)
            Info(9, X) = Info(9, Y)
            Info(10, X) = Info(10, Y)
            Info(11, X) = Info(11, Y)
            Info(12, X) = Info(12, Y)
            
            Info(0, Y) = T0 'school id
            Info(1, Y) = T1
            Info(2, Y) = T2
            Info(3, Y) = T3
            Info(4, Y) = T4
            Info(5, Y) = T5
            Info(6, Y) = T6
            Info(7, Y) = T7
            Info(8, Y) = T8
            Info(9, Y) = T9
            Info(10, Y) = T10
            Info(11, Y) = T11
            Info(12, Y) = T12
        End If
    Next Y
Next X
End Sub

Function countPages(z As Integer) As Integer
    Dim sch As String
    Dim X As Integer
    Dim Counter As Integer
    Counter = 0
    For X = 1 To UBound(Info, 2)
        If Info(0, z) = Info(0, X) Then
            Counter = Counter + 1
        End If
    Next X
    countPages = Counter
End Function

Sub PrintEVR()
    Dim cnt As Integer
    Dim cntFax As Integer
    Dim r As Integer
    Dim i As Integer
    Dim oldSchID As String
    Dim DocFolder As String
    Dim SchFaxNo As String
    Dim SchName As String
    
    'get document folder
    If SP.Common.TestMode Then
        DocFolder = "X:\PADD\LoanOrigination\Test\"
    Else
        DocFolder = "X:\PADD\LoanOrigination\"
    End If
    
    'sort records
    sortEVR
    
    'find first record to process if in recovery mode
    r = 0
    If lSchID <> "" Then
        'find the first record for the school being processed when script went down
        For i = 1 To UBound(Info, 2)
            r = r + 1
            If lSchID = Info(0, i) Then Exit For
        Next i
    Else
        r = 1
    End If
    
    'group records to be faxed by school
    For cnt = r To UBound(Info, 2)
        'process files if school changes
        If oldSchID <> Info(0, cnt) Then
        
            'save and fax EVRs if not processing the first record
            If oldSchID <> "" Then
                Close #1
                SP.Common.SaveDocs DocFolder, "EVR", "T:\EVRFU.txt", "T:\EVRFU Fax Detail Info.doc"
                FaxSchoolData SchName, SchFaxNo
            End If
            
            'update recovery log
            Open LogFolder & "evrfulog.txt" For Output As #8
            Write #8, Info(0, cnt)
            Close #8
            
            'get school name and fax number
            SchName = Info(1, cnt)
            SchFaxNo = Replace(Info(9, cnt), "-", "")
            SchFaxNo = Replace(SchFaxNo, " ", "")
            SchFaxNo = Replace(SchFaxNo, ")", "")
            SchFaxNo = Replace(SchFaxNo, "(", "")
            IsLocalCall = SP.Common.Sql("SELECT * FROM GENR_LST_LocalCallZipCodes WHERE ZIPCode = '" & Mid(Info(7, cnt), 1, 5) & "'")
            If IsLocalCall(0) = "Empty" Then
                SchFaxNo = "91" & SchFaxNo
            Else
                SchFaxNo = "9" & Mid(SchFaxNo, 4, 7)
            End If
            
            'save fax cover sheet
            Open "T:\EVRFU.txt" For Output As #2
            Write #2, "School", "SchFAX", "Total_Pages"
            Write #2, SchName, SchFaxNo, countPages(cnt) + 1
            Close #2
            SP.Common.SaveDocs DocFolder, "EVR Cover Sheet", "T:\EVRFU.txt", "T:\EVRFU Fax CoverSheet.doc"
        
            'open new data file for EVRs
            Open "T:\EVRFU.txt" For Output As #1
            Write #1, "SchCd", "School", "SchAdd", "SchAdd2", "SchAdd3", "SchCity", "SchSt", "SchZIP", "SchCountry", "SchFax", "Comments", "SSN", "Name"
            
        End If
        
        'write record to EVR data file
        Write #1, Info(0, cnt), Info(1, cnt), Info(2, cnt), Info(3, cnt), Info(4, cnt), Info(5, cnt), Info(6, cnt), Info(7, cnt), Info(8, cnt), Info(9, cnt), Info(10, cnt), Info(11, cnt), Info(12, cnt)
        oldSchID = Info(0, cnt)
    Next cnt
    
    'save and fax EVRs for last school
    Close #1
    SP.Common.SaveDocs DocFolder, "EVR", "T:\EVRFU.txt", "T:\EVRFU Fax Detail Info.doc"
    FaxSchoolData SchName, SchFaxNo
    
    'delete recovery files
    Kill LogFolder & "evrfulog.txt"
    Kill "T:\evrfudat.txt"
    
    sendEMail
    
    MsgBox "Processing complete.", vbInformation, "Processing Complete"
    End
End Sub

Function FaxSchoolData(School As String, SchFaxNo As String)
    Dim FaxSrv As New FaxServer
    Dim FX As Fax
    Dim FaxDetailFileName As String
    On Error GoTo FaxErrMsg
    'set up fax server
    FaxSrv.ServerName = "IMAGING-FAX"
    FaxSrv.Protocol = CommunicationProtocolType.cpTCPIP
    FaxSrv.UseNTAuthentication = BoolType.True
    FaxSrv.OpenServer 'open server
    'process depending on testmode
    
    'fax documents
    Set FX = FaxSrv.CreateObject(CreateObjectType.coFax)
    If SP.Common.TestMode Then
        MsgBox SchFaxNo 'msgbox for testing whether fax num is correct
        FX.ToFaxNumber = "93217198" 'send internally for testing
        MsgBox "Since this is a test, the fax will be sent to the operations fax number of " & FX.ToFaxNumber & ".  If the script were being run in production, it would use the fax number of " & SchFaxNo & " for " & School & ".", vbOKOnly + vbInformation, "Test Fax"
    Else
        FX.ToFaxNumber = SchFaxNo
    End If
    FX.ToName = School
    FX.HasCoversheet = False
    FX.Attachments.aDD "T:\EVRFU Fax CoverSheet.doc"
    FX.Attachments.aDD "T:\EVRFU Fax Detail Info.doc"
    FX.SEND
    'delete data and doc files
    Kill "T:\EVRFU Fax CoverSheet.doc"
    Kill "T:\EVRFU Fax Detail Info.doc"
    FaxSrv.CloseServer 'close server
    Exit Function
FaxErrMsg:
    MsgBox "An error occured while trying to fax.  Please contact Systems Support.", vbOKOnly + vbCritical, "Fax Error"
    End
End Function
