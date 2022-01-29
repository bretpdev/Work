Attribute VB_Name = "EVR"
Dim EVRQ As String
Dim Name As String
Dim School As String
Dim SchAdd As String
Dim SchAdd2 As String
Dim SchAdd3 As String
Dim SchCity As String
Dim SchST As String
Dim SchZIP As String
Dim SchFAX As String
Dim SchCountry As String
Dim LocalComments As String
Dim ReportedProblemSchools() As String
Private Const TEMP_DIR As String = "T:\"

'process tasks in the URGEVRRQ queue
Public Sub URGEVRRQ()
    'warn user of purpose of the script
    warn = MsgBox("This script process tasks in the URGEVRRQ queue to send EVRs to schools.  Click OK to continue or Cancel to quit.", vbOKCancel, "EVR")
    'end the script if the dialog box is canceled
    If warn = 2 Then End
    'specify the queue to process and go to the processing subroutine
    Script = "  {EVR}"
    EVRQ = "URGEVRRQ"
    EVRProc
End Sub

'process tasks in the EVRREQST queue
Public Sub EVRREQST()
    'warn user of purpose of the script
    warn = MsgBox("This script process tasks in the EVRREQST queue to send EVRs to schools.  Click OK to continue or Cancel to quit.", vbOKCancel, "EVR")
    'end the script if the dialog box is canceled
    If warn = 2 Then End
    'specify the queue to process and go to the processing subroutine
    Script = "  {EVR}"
    EVRQ = "EVRREQST"
    EVRProc
End Sub

'process tasks in the SENRERRC queue
Public Sub SENRERRC()
    'warn user of purpose of the script
    warn = MsgBox("This script process tasks in the SENRERRC queue to send EVRs to schools.  Click OK to continue or Cancel to quit.", vbOKCancel, "EVR")
    'end the script if the dialog box is canceled
    If warn = 2 Then End
    'specify the queue to process and go to the processing subroutine
    Script = "  {EVR}"
    EVRQ = "SENRERRC"
    EVRProc
End Sub

'process EVR tasks
Private Sub EVRProc()
    ResetPublicVars
    Dim row As Integer
    Dim RecoveryPhase As String
    Dim Log As String
    SP.Common.TestMode , , Log
    If Dir(Log & "EVR Recovery Log.txt") = "" Then
        'process normally
        Open Log & "EVR Recovery Log.txt" For Output As #8
        Write #8, "ALL"
        Close #8
        Open TEMP_DIR & "evrdat.txt" For Output As #1
        Write #1, "SSN", "Name", "School", "SchCd", "SchAdd", "SchAdd2", "SchAdd3", "SchCity", "SchST", "SchZIP", "SchFAX", "SchCountry", "Comments"
        Close #1
        RecoveryPhase = "ALL"
    Else
        'process in recovery
        Open Log & "EVR Recovery Log.txt" For Input As #8
        Input #8, RecoveryPhase
        Close #8
    End If
    ReDim ReportedProblemSchools(0)
    Script = "  {EVR}"
    LP9OComment1 = ""
    LP9OComment2 = ""
    LP9OComment3 = ""
    LP9OComment4 = ""
    prnind = ""
    If RecoveryPhase <> "ALL TASKS FINISHED" And RecoveryPhase <> "SCHOOL FILES SEPARATED" Then
        'access the queue task in LP9A
        FastPath "LP9AC" & EVRQ
        'end the script if there are no tasks
        If Check4Text(22, 3, "47423") Or Check4Text(22, 3, "47420") Then
            MsgBox "There are no tasks in the " & EVRQ & " queue.  Processing is complete."
            'delete log as the script ends successfully
            Kill Log & "EVR Recovery Log.txt"
            End
        End If
        'end the script if the user is assigned to a task in another queue
        If Not Check4Text(1, 9, EVRQ) Then
            MsgBox "You have an unresolved task in the " & GetText(1, 9, 8) & " queue.  You must complete the task before working the " & EVRQ & " queue."
            'delete log as the script ends successfully
            Kill Log & "EVR Recovery Log.txt"
            End
        End If
        'process each task
        Do
            'gather data from queue tasks screen
            SSN = GetText(17, 70, 9)
            Name = GetText(17, 6, 35)
            SchCd = GetText(12, 11, 8)
            'clear information from previous task from variables
            School = ""
            SchAdd = ""
            SchAdd2 = ""
            SchAdd3 = ""
            SchCity = ""
            SchST = ""
            SchZIP = ""
            SchFAX = ""
            SchCountry = ""
            'prompt the user to review to see if an EVR is needed and pause the script
            MsgBox "Review Clearinghouse and NSLDS to determine if an EVR is needed.", vbOKOnly, "Review for EVR"
            PauseForInsert
            Hit "INSERT"
            FastPath "LG29I" & SSN
            PauseForInsert
            FastPath "LP50I" & SSN
            PutText 5, 14, "X", "Enter"
            PauseForInsert
            warn = MsgBox("Is an EVR needed.  Click 'NO' if you wish to re-queue or if a EVR isn't needed.", vbYesNo, "EVR Needed")
            'create an EVR if one is needed
            If warn = 6 Then
                If vbYes = MsgBox("Do you want to request a specific time frame of enrollment?", vbYesNo, "Specific Time Frame") Then
                    LocalComments = InputBox("Please enter your comments (comments must be under 50 characters).", "Comments for EVR form.")
                    While Len(LocalComments) > 50
                        LocalComments = InputBox("The comments entered were " & (Len(LocalComments) - 50) & " character(s) to long. Please enter your comments (comments must be under 50 characters).", "Comments for EVR form.", LocalComments)
                    Wend
                Else 'use default statement
                    LocalComments = "Please list the complete enrollment history for the student referenced above."
                End If
                'access LPSC
                FastPath "LPSCI" & SchCd
                'select dept 112
                If Check4Text(21, 3, "SEL") Then
                    'find the row for dept 112
                    row = 7
                    Do Until Check4Text(row, 7, "112")
                        row = row + 1
                        'go to the next page if the bottom of the page is reached
                        If row = 19 Then
                            Hit "F8"
                            'prompt the user to enter the information is dept 112 is not found
                            If Check4Text(22, 3, "46004") Then
                                warn = MsgBox("Click OK to enter the missing address or FAX information.  Also fill out a School Eligibility Form with the missing information and give the form to System Support.", vbOKOnly, "Enter Missing Information")
                                EnterInfo
                                GoTo 49
                            End If
                            row = 7
                        End If
                    Loop
                    'select the row
                    Session.TransmitANSI GetText(row, 2, 2)
                    Hit "ENTER"
                End If
                'get the school information if dept 112 was found
                If Check4Text(7, 15, "112") Then
                    School = GetText(5, 21, 40)
                    School = Mid(School, 1, 30)
                    SchAdd = GetText(8, 21, 30)
                    If Not Check4Text(9, 21, " ") Then SchAdd2 = GetText(9, 21, 30)
                    If Not Check4Text(10, 21, " ") Then SchAdd3 = GetText(10, 21, 30)
                    SchCity = GetText(11, 21, 30)
                    'If the school is foreign then get the state and the fax form the foreign fields
                    'else get them from the normal fields
                    If Check4Text(11, 59, "FC") Then
                        SchST = GetText(12, 21, 26)
                        If Not Check4Text(16, 63, " ") Then SchFAX = GetText(16, 63, 17)
                        SchFAX = Replace(SchFAX, " ", "")
                    Else
                        SchST = GetText(11, 59, 2)
                        If Not Check4Text(15, 70, " ") Then SchFAX = Format(GetText(15, 70, 10), "(###) ###-####")
                    End If
                    SchZIP = GetText(11, 66, 5)
                    If GetText(12, 55, 25) <> "" Then SchCountry = GetText(12, 55, 25)
                    If Not Check4Text(11, 71, " ") Then SchZIP = SchZIP & "-" & GetText(11, 71, 4)
                    If SchAdd = "" Or SchFAX = "" Then
                        warn = MsgBox("Click OK to enter the missing address or FAX information.  Also fill out a School IDEM Change Form with the missing information and give the form to System Support.", vbOKOnly, "Enter Missing Information")
                        EnterInfo
                    End If
                'prompt the user for the information if dept 112 was not found
                Else
                    warn = MsgBox("Click OK to enter the missing address or FAX information.  Also fill out a School Eligibility Form with the missing information and give the form to System Support.", vbOKOnly, "Enter Missing Information")
                    EnterInfo
                End If
                If RecoveryPhase = "ALL" Then
49                      Open TEMP_DIR & "evrdat.txt" For Append As #1
                    RemoveCommasAndSendEmail SchCd, School, SchAdd, SchAdd2, SchAdd3, LocalComments
                    Write #1, Format(SSN, "@@@-@@-@@@@"), Name, School, SchCd, SchAdd, SchAdd2, SchAdd3, SchCity, SchST, SchZIP, SchFAX, SchCountry, LocalComments
                    Close #1
                    Open Log & "EVR Recovery Log.txt" For Output As #8
                    Write #8, "WRITTEN TO FILE"
                    Close #8
                End If
                prnind = "Y"
                'add an activity record
                PauPlea = "N"
                ActCd = "GEVRS"
                ActTyp = "FO"
                ConTyp = "07"
                comment = "evr sent to school code " & SchCd
                If RecoveryPhase = "ALL" Or RecoveryPhase = "WRITTEN TO FILE" Then
                    Common.LP50
                    RecoveryPhase = "ALL"
                    Open Log & "EVR Recovery Log.txt" For Output As #8
                    Write #8, "LP50 COMMENT"
                    Close #8
                End If
                'add a queue task
                InstID = SchCd
                InstTyp = "001"
                Queue = "EVSNTFLU"
                Common.DateDue = CStr(Date + 7)
                If RecoveryPhase = "ALL" Or RecoveryPhase = "LP50 COMMENT" Then
                    Common.LP9O
                    RecoveryPhase = "ALL"
                    Open Log & "EVR Recovery Log.txt" For Output As #8
                    Write #8, "TASK COMPLETE"
                    Close #8
                End If
            Else    'If task doesn't need a evr or it needs to be re-queued
                'ask user if they want to re-queue the task
                If (vbYes = MsgBox("Would you like to re-queue the task?", vbYesNo, "Re-Queue Task?")) Then
                    Dim ReQueueDate As String
                    ReQueueDate = InputBox("Please enter the date that the task needs to be re-queued on.", "Re-Queue Date")
                    While Not DateFormatter(ReQueueDate)
                        ReQueueDate = InputBox("Please enter the date that the task needs to be re-queued on.", "Re-Queue Date")
                    Wend
                    ActCd = "GEVRN"
                    ActTyp = "MS"
                    ConTyp = "10"
                    comment = EVRQ & " task re-queued " & ReQueueDate
                    If RecoveryPhase = "ALL" Then
                        Common.LP50
                        RecoveryPhase = "ALL"
                        Open Log & "EVR Recovery Log.txt" For Output As #8
                        Write #8, "TASK COMPLETE"
                        Close #8
                    End If
                Else 'add an activity record if no EVR is needed
                    PauPlea = "Y"
                    ActCd = "GEVRN"
                    ActTyp = "MS"
                    ConTyp = "10"
                    comment = ""
                    If RecoveryPhase = "ALL" Then
                        Common.LP50
                        RecoveryPhase = "ALL"
                        Open Log & "EVR Recovery Log.txt" For Output As #8
                        Write #8, "TASK COMPLETE"
                        Close #8
                    End If
                End If
            End If
            If RecoveryPhase = "ALL" Or RecoveryPhase = "TASK COMPLETE" Then
                'access the queue task in LP9A
                FastPath "LP9AC" & Queue
                If comment = EVRQ & " task re-queued " & ReQueueDate Then
                    PutText 4, 22, Format(CDate(ReQueueDate), "MMDDYYYY")
                    Hit "F11"
                Else
                    'complete the task
                    Hit "F6"
                End If
                warn = MsgBox("Click OK to process the next task or Cancel to print the EVRs and end the script.", vbOKCancel, "Next Task or Print EVRs")
                If warn = 2 Then Exit Do
                'go to the next task
                Hit "F8"
                'stop processing if no more tasks are found
                If Check4Text(22, 3, "46004") Then Exit Do
                RecoveryPhase = "ALL"
                Open Log & "EVR Recovery Log.txt" For Output As #8
                Write #8, "ALL"
                Close #8
            End If
        Loop
        RecoveryPhase = "ALL TASKS FINISHED"
        Open Log & "EVR Recovery Log.txt" For Output As #8
        Write #8, "ALL TASKS FINISHED"
        Close #8
    End If
    'print the EVRs if there are any to print
    If RecoveryPhase = "ALL TASKS FINISHED" Then
        CreateSchoolSpecificFiles Log
        Open Log & "EVR Recovery Log.txt" For Output As #8
        Write #8, "SCHOOL FILES SEPARATED"
        Close #8
        RecoveryPhase = "SCHOOL FILES SEPARATED"
    End If
    If RecoveryPhase = "SCHOOL FILES SEPARATED" Then
        FaxSchoolData
        Open Log & "EVR Recovery Log.txt" For Output As #8
        Write #8, "DONE"
        Close #8
        RecoveryPhase = "DONE"
    End If
    'delete log as the script ends successfully
    Kill Log & "EVR Recovery Log.txt"
    'warn the user that processing is complete
    MsgBox "Processing Complete", vbOKOnly + vbInformation, "Processing Complete"
End Sub

'prompt the user to enter missing school information
Private Sub EnterInfo()
    Okay = 0
    'pass values to the dialog box
    EVRSchInfo.School.Value = School
    EVRSchInfo.SchAdd.Value = SchAdd
    EVRSchInfo.SchCity.Value = SchCity
    EVRSchInfo.SchST.Value = SchST
    EVRSchInfo.SchZIP.Value = SchZIP
    EVRSchInfo.SchFAX.Value = SchFAX
    EVRSchInfo.SchCountry.Value = SchCountry
    Load EVRSchInfo
    'display the dialog box for the user to enter missing information
    EVRSchInfo.Show
    'get the information entered
    SchCountry = EVRSchInfo.SchCountry.Value
    School = EVRSchInfo.School
    SchAdd = EVRSchInfo.SchAdd
    SchCity = EVRSchInfo.SchCity
    SchST = EVRSchInfo.SchST
    SchZIP = EVRSchInfo.SchZIP
    If Len(SchZIP) = 9 Then SchZIP = Format(SchZIP, "#####-####")
    SchFAX = EVRSchInfo.SchFAX
    If Len(SchFAX) = 10 Then SchFAX = Format(SchFAX, "(###) ###-####")
    Unload EVRSchInfo
    'end the script if the dialog box was cancelled
    If Okay <> 1 Then End
End Sub

'format strings in MM/DD/YYYY format
Private Function DateFormatter(ByRef dt As String, Optional ErrorMessage As String = "That wasn't a valid date.  Please try again.") As Boolean
    'return an invalid date if the length is not 6, 8, or 10 (lengths of valid date strings)
    If Len(dt) <> 6 And Len(dt) <> 8 And Len(dt) <> 10 Then
        DateFormatter = False
    'format 6 digit date with no slashes
    ElseIf Len(dt) = 6 And IsDate(Format(dt, "##/##/##")) = True Then
        DateFormatter = True
        dt = Format(DateValue(Format(dt, "##/##/##")), "MM/DD/YYYY")
    'format 6 digit date with slashes
    ElseIf Len(dt) = 8 And IsDate(dt) = True Then
        DateFormatter = True
        dt = Format(DateValue(dt), "MM/DD/YYYY")
    'format 8 digit date with no slashes
    ElseIf Len(dt) = 8 And IsDate(Format(dt, "##/##/####")) Then
        DateFormatter = True
        dt = Format(dt, "##/##/####")
    'Check if 10 digit string is a date
    ElseIf Len(dt) = 10 And IsDate(dt) Then
        DateFormatter = True
    Else
        'an thing that didn't fit into one of the above catergories is not a date
        DateFormatter = False
    End If
    If DateFormatter = False Then
        MsgBox ErrorMessage
    Else
        If CDate(dt) <= Date Then
            DateFormatter = False
            MsgBox "The re-queue date must be a future date."
        End If
    End If
End Function

Private Function CreateSchoolSpecificFiles(Log As String)
    Dim Line As String
    Dim Fields() As String
    Dim Pages As String
    Dim School As String
    Dim SchFAX As String
    Dim DialFaxNum As String
    Dim IsLocalCall() As String
    
    If FileLen(TEMP_DIR & "evrdat.txt") = 120 Then
        'delete log as the script ends successfully
        Kill Log & "EVR Recovery Log.txt"
        'warn the user that processing is complete
        MsgBox "Processing Complete"
        End
    End If
    Open TEMP_DIR & "evrdat.txt" For Input As #2
    Line Input #2, Line 'header row
    'file format Format(SSN, "@@@-@@-@@@@"), Name, School, SchCd, SchAdd, SchAdd2, SchAdd3, SchCity, SchST, SchZIP, SchFAX, SchCountry
    While Not EOF(2)
        Line Input #2, Line
        Line = Replace(Line, """", "")
        Fields = Split(Line, ",") 'split into records
        'remove all possible windows file system special characters
        Fields(2) = Replace(Fields(2), "/", " ")
        Fields(2) = Replace(Fields(2), "\", " ")
        Fields(2) = Replace(Fields(2), "*", " ")
        Fields(2) = Replace(Fields(2), "?", " ")
        Fields(2) = Replace(Fields(2), "$", " ")
        Fields(2) = Replace(Fields(2), "&", " ")
        Fields(2) = Replace(Fields(2), "~", " ")
        Fields(2) = Replace(Fields(2), "+", " ")
        Fields(2) = Replace(Fields(2), "|", " ")
        Fields(2) = Replace(Fields(2), "'", " ")
        Fields(2) = Replace(Fields(2), """", " ")
        Fields(2) = Replace(Fields(2), "^", " ")
        Fields(2) = Replace(Fields(2), "%", " ")
        Fields(2) = Replace(Fields(2), "!", " ")
        Fields(2) = Replace(Fields(2), "#", " ")
        Fields(2) = Replace(Fields(2), ":", " ")
        Fields(2) = Replace(Fields(2), ";", " ")
        Fields(2) = Replace(Fields(2), ".", " ")
        Fields(2) = Replace(Fields(2), "`", " ")
        Fields(2) = Replace(Fields(2), "[", " ")
        Fields(2) = Replace(Fields(2), "]", " ")
        Fields(2) = Replace(Fields(2), "{", " ")
        Fields(2) = Replace(Fields(2), "}", " ")
        'strip out any bad chars for fax number
        DialFaxNum = Replace(Fields(10), "-", "")
        DialFaxNum = Replace(DialFaxNum, " ", "")
        DialFaxNum = Replace(DialFaxNum, ")", "")
        DialFaxNum = Replace(DialFaxNum, "(", "")
        IsLocalCall = SP.Common.Sql("SELECT * FROM GENR_LST_LocalCallZipCodes WHERE ZIPCode = '" & Fields(9) & "'")
        If IsLocalCall(0) = "Empty" Then
            DialFaxNum = "91" & DialFaxNum
        Else
            DialFaxNum = "9" & DialFaxNum
        End If
        
        'split in to school specific files
        If Dir(TEMP_DIR & "evrdat " & Fields(2) & ".txt") <> "" Then
            'if file for school exists then write info out to data file
            Open TEMP_DIR & "evrdat " & Fields(2) & ".txt" For Append As #4
            Print #4, Line
            Close #4
            'increase page counter by one
            'gather info
            Open TEMP_DIR & "evrdatCover Sheet data" & Fields(2) & "_" & DialFaxNum & "_.txt" For Input As #4
            Input #4, Pages, School, SchFAX
            Input #4, Pages, School, SchFAX
            Close #4
            'write new info out to file
            Open TEMP_DIR & "evrdatCover Sheet data" & Fields(2) & "_" & DialFaxNum & "_.txt" For Output As #4
            Write #4, "Total Pages", "School", "SchFAX"
            Write #4, CStr(CInt(Pages) + 1), School, SchFAX
            Close #4
        Else 'create new files
            'create Cover Sheet file
            Open TEMP_DIR & "evrdatCover Sheet data" & Fields(2) & "_" & DialFaxNum & "_.txt" For Output As #4
            Write #4, "Total Pages", "School", "SchFAX"
            Write #4, "2", Fields(2), Fields(10)
            Close #4
            'create report file
            Open TEMP_DIR & "evrdat " & Fields(2) & ".txt" For Output As #4
            Write #4, "SSN", "Name", "School", "SchCd", "SchAdd", "SchAdd2", "SchAdd3", "SchCity", "SchST", "SchZIP", "SchFAX", "SchCountry", "Comments"
            Print #4, Line
            Close #4
        End If
    Wend
    Close #2
    Kill TEMP_DIR & "evrdat.txt"
End Function

Private Function FaxSchoolData()
    Dim School As String
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
    If SP.Common.TestMode() Then
        While Dir(TEMP_DIR & "evrdat *.txt") <> ""
            School = Mid(Dir(TEMP_DIR & "evrdat *.txt"), 8, (Len(Dir(TEMP_DIR & "evrdat *.txt")) - 11))
            FaxDetailFileName = Dir(TEMP_DIR & "evrdatCover Sheet data" & School & "_*_.txt")
            SP.Common.SaveDocs "X:\PADD\LoanOrigination\Test\", "EVR Cover Sheet", TEMP_DIR & FaxDetailFileName, TEMP_DIR & "EVR Fax CoverSheet.doc"
            SP.Common.SaveDocs "X:\PADD\LoanOrigination\Test\", "EVR", TEMP_DIR & "evrdat " & School & ".txt", TEMP_DIR & "EVR Fax Detail Info.doc"
            'fax documents
            Set FX = FaxSrv.CreateObject(CreateObjectType.coFax)
            FX.ToFaxNumber = "93217198" 'send internally for testing
            MsgBox "Since this is a test, the fax will be sent to the operations fax number of " & FX.ToFaxNumber & ".  If the script were being run in production, it would use the fax number of " & Split(FaxDetailFileName, "_")(1) & " for " & School & ".", vbOKOnly + vbInformation, "Test Fax"
            FX.ToName = School
            FX.HasCoversheet = False
            FX.Attachments.aDD TEMP_DIR & "EVR Fax CoverSheet.doc"
            FX.Attachments.aDD TEMP_DIR & "EVR Fax Detail Info.doc"
            FX.SEND
            'delete data and doc files
            Kill TEMP_DIR & "EVR Fax CoverSheet.doc"
            Kill TEMP_DIR & "EVR Fax Detail Info.doc"
            Kill TEMP_DIR & "evrdat " & School & ".txt" 'kill info file
            Kill TEMP_DIR & FaxDetailFileName 'kill cover sheet info file
        Wend
    Else
        While Dir(TEMP_DIR & "evrdat *.txt") <> ""
            School = Mid(Dir(TEMP_DIR & "evrdat *.txt"), 8, (Len(Dir(TEMP_DIR & "evrdat *.txt")) - 11))
            FaxDetailFileName = Dir(TEMP_DIR & "evrdatCover Sheet data" & School & "_*_.txt")
            SP.Common.SaveDocs "X:\PADD\LoanOrigination\", "EVR Cover Sheet", TEMP_DIR & FaxDetailFileName, TEMP_DIR & "EVR Fax CoverSheet.doc"
            SP.Common.SaveDocs "X:\PADD\LoanOrigination\", "EVR", TEMP_DIR & "evrdat " & School & ".txt", TEMP_DIR & "EVR Fax Detail Info.doc"
            'fax documents
            Set FX = FaxSrv.CreateObject(CreateObjectType.coFax)
            FX.ToFaxNumber = Split(FaxDetailFileName, "_")(1)
            FX.ToName = School
            FX.HasCoversheet = False
            FX.Attachments.aDD TEMP_DIR & "EVR Fax CoverSheet.doc"
            FX.Attachments.aDD TEMP_DIR & "EVR Fax Detail Info.doc"
            FX.SEND
            'delete data and doc files
            Kill TEMP_DIR & "EVR Fax CoverSheet.doc"
            Kill TEMP_DIR & "EVR Fax Detail Info.doc"
            Kill TEMP_DIR & "evrdat " & School & ".txt" 'kill info file
            Kill TEMP_DIR & FaxDetailFileName 'kill cover sheet info file
        Wend
    End If
    FaxSrv.CloseServer 'close server
    Exit Function
FaxErrMsg:
    MsgBox "An error occured while trying to fax.  Please contact Systems Support.", vbOKOnly + vbCritical, "Fax Error"
    End
End Function

Private Function RemoveCommasAndSendEmail(ByRef SchCd As String, ByRef School As String, ByRef SchAdd As String, ByRef SchAdd2 As String, ByRef SchAdd3 As String, ByRef LocalComments As String)
    Dim Recip As String
    Name = Replace(Name, ",", "")
    School = Replace(School, ",", "")
    LocalComments = Replace(LocalComments, ",", "")
    If InStr(1, SchAdd, ",") <> 0 Or InStr(1, SchAdd2, ",") <> 0 Or InStr(1, SchAdd3, ",") <> 0 Then
        If EmailAlreadySent(SchCd) = False Then
            'add school to array so another email isn't sent
            ReDim Preserve ReportedProblemSchools(UBound(ReportedProblemSchools) + 1)
            ReportedProblemSchools(UBound(ReportedProblemSchools)) = SchCd
            'Remove commas
            SchAdd = Replace(SchAdd, ",", " ")
            SchAdd2 = Replace(SchAdd2, ",", " ")
            SchAdd3 = Replace(SchAdd3, ",", " ")
            'send email
            SP.Common.SendMail SP.Common.BSYSRecips("EVR"), , "Remove comma in school address.", "School Code: " & SchCd
        End If
    End If
End Function

'this function decides if an email has already been sent for the school
Private Function EmailAlreadySent(SC As String) As Boolean
    Dim i As Integer
    While i < UBound(ReportedProblemSchools)
        If ReportedProblemSchools(i) = SC Then
            EmailAlreadySent = True 'match was found return true to not send email
            Exit Function
        End If
        i = i + 1
    Wend
    EmailAlreadySent = False
End Function
