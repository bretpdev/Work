Attribute VB_Name = "SENRERR"
'process tasks in the SENRERRD queue
Public Sub SEND()
    'warn user of purpose of the script
    If MsgBox("This script process tasks in the SENRERRD queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "SENERRD") <> vbOK Then End
    ResetPublicVars
    'specify the queue to process and go to the processing subroutine
    Script = "  {SENRERRD}"
    Queue = "SENRERRD"
    SenProc
End Sub

'process tasks in the SENRERRF queue
Public Sub SENF()
    'warn user of purpose of the script
    If MsgBox("This script process tasks in the SENRERRF queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "SENERRF") <> vbOK Then End
    ResetPublicVars
    'specify the queue to process and go to the processing subroutine
    Script = "  {SENRERRF}"
    Queue = "SENRERRF"
    SenProc
End Sub

'process tasks in the SENRERRI queue
Public Sub SENI()
    'warn user of purpose of the script
    If MsgBox("This script process tasks in the SENRERRI queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "SENERRI") <> vbOK Then End
    ResetPublicVars
    'specify the queue to process and go to the processing subroutine
    Script = "  {SENRERRI}"
    Queue = "SENRERRI"
    SenProc
End Sub

'process tasks in the SENRERRK queue
Public Sub SENK()
    'warn user of purpose of the script
    If MsgBox("This script process tasks in the SENRERRK queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "SENERRK") <> vbOK Then End
    ResetPublicVars
    'specify the queue to process and go to the processing subroutine
    Script = "  {SENRERRK}"
    Queue = "SENRERRK"
    SenProc
End Sub

'process tasks in the SENRERRL queue
Public Sub SENL()
    'warn user of purpose of the script
    If MsgBox("This script process tasks in the SENRERRL queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "SENERRL") <> vbOK Then End
    ResetPublicVars
    'specify the queue to process and go to the processing subroutine
    Script = "  {SENRERRL}"
    Queue = "SENRERRL"
    SenComp
End Sub

'process tasks in the SENRERRM queue
Public Sub SENM()
    'warn user of purpose of the script
    If MsgBox("This script process tasks in the SENRERRM queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "SENERRM") <> vbOK Then End
    ResetPublicVars
    'specify the queue to process and go to the processing subroutine
    Script = "  {SENRERRM}"
    Queue = "SENRERRM"
    SenProc
End Sub

'process tasks in the SENRERRO queue
Public Sub SENO()
    'warn user of purpose of the script
    If MsgBox("This script process tasks in the SENRERRO queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "SENERRO") <> vbOK Then End
    ResetPublicVars
    'specify the queue to process and go to the processing subroutine
    Script = "  {SENRERRO}"
    Queue = "SENRERRO"
    SenComp
End Sub

'process the tasks in the specified queue
Private Sub SenProc()
    'set activity record values
    ActCd = "GBSCR"
    ActTyp = "MS"
    ConTyp = "22"
    'access the queue task in LP9A
    FastPath "LP9AC" & Queue
    'end the script if there are no tasks
    If Check4Text(22, 3, "47423") Or Check4Text(22, 3, "47420") Then
        MsgBox "There are no tasks in the " & Queue & " queue.  Processing is complete."
        End
    End If
    'end the script if the user is assigned to a task in another queue
    If Not Check4Text(1, 9, Queue) Then
        MsgBox "You have an unresolved task in the " & GetText(1, 9, 8) & " queue.  You must complete the task before working the SENSERR queues."
        End
    End If
    'process each task
    Do While Not Check4Text(22, 3, "46004")
        'gather data from queue tasks screen
        SSN = GetText(17, 70, 9)
        Src = GetText(12, 19, 1)
        AGD = GetText(14, 39, 4) & GetText(14, 35, 4)
        EnrSta = GetText(14, 48, 1)
        StaEffDt = GetText(14, 59, 4) & GetText(14, 55, 4)
        SchCd = GetText(15, 11, 8)
        SchCrtDt = GetText(15, 28, 4) & GetText(15, 24, 4)

        'if the AGD is invalid, replace it with the enrollment status effective date
        If (AGD = "00000000" Or AGD = "01010001") And (EnrSta = "L" Or EnrSta = "W") Then
            AGD = StaEffDt
        End If
        'process invalid dates
        If IsDate(Format(AGD, "##/##/####")) = False Or _
           IsDate(Format(StaEffDt, "##/##/####")) = False Or _
           IsDate(Format(SchCrtDt, "##/##/####")) = False Or _
           AGD = "00000000" Or AGD = "01010001" Then
            InvalidDate
            GoTo 99
        End If
        'access LG29
        FastPath "LG29C" & SSN & ";" & SchCd
        'select the first record if the selection screen is displayed
        If Check4Text(20, 8, "SEL") Then
            Session.TransmitANSI "01"
            Hit "ENTER"
        End If
        If Check4Text(22, 3, "48012") Then
            FastPath "LG29A" & SSN & ";" & SchCd
        End If
        'add an activity record and go to the next task if the info cannot be updated because of the loan status
        If (Not Check4Text(21, 3, "44000")) And (Not Check4Text(21, 3, "46011")) Then
            Defaulted
            GoTo 99
        End If
        'enter the information from the queue task screen
        PutText 7, 41, "H"
        PutText 8, 20, EnrSta
        PutText 9, 35, StaEffDt
        PutText 10, 35, AGD
        PutText 10, 71, SchCd
        PutText 11, 35, SchCrtDt
        'post the changes
        Hit "ENTER"
        'add an activity record or end the script
        If Check4Text(21, 3, "49000") Or Check4Text(21, 3, "48003") Then
            comment = "sscr data updated"
        ElseIf Check4Text(21, 3, "10052") Then
            comment = "sscr no update"
        Else
            warn = MsgBox("An unknown error was encountered.  Please note the code displayed and report it to Systems Support.", vbOKOnly, "Unknown Error")
            End
        End If
        Common.LP50
        'return to the queue task on LP9A
99      FastPath "LP9AC" & Queue
        'complete the task
        Hit "F6"
        'go to the next task
        Hit "F8"
    Loop
    'warn the user that processing is complete
    MsgBox "Processing Complete"
End Sub

'complete the tasks in the specified queue
Private Sub SenComp()
    'access the queue task in LP9A
    FastPath "LP9AC" & Queue
    'end the script if there are no tasks
    If Check4Text(22, 3, "47423") Or Check4Text(22, 3, "47420") Then
        MsgBox "There are no tasks in the " & Queue & " queue.  Processing is complete."
        End
    End If
    'end the script if the user is assigned to a task in another queue
    If Not Check4Text(1, 9, Queue) Then
        MsgBox "You have an unresolved task in the " & GetText(1, 9, 8) & " queue.  You must complete the task before working the SENSERR queues."
        End
    End If
    'process each task
    Do While Not Check4Text(22, 3, "46004")
        'gather data from queue tasks screen
        SSN = GetText(17, 70, 9)
        'set activity record values
        ActCd = "GBSCR"
        ActTyp = "MS"
        ConTyp = "22"
        comment = "no update"
        'add an activity record
        Common.LP50
        'return to the queue task on LP9A
        FastPath "LP9AC" & Queue
        'complete the task
        Hit "F6"
        'go to the next task
        Hit "F8"
    Loop
    'warn the user that processing is complete
    MsgBox "Processing Complete"
End Sub

'add a queue task to INVLDSCR if a date is invalid
Private Sub InvalidDate()
    InstID = SchCd
    InstTyp = "001"
    'add task to INVLDSCR queue
    Queue = "INVLDSCR"
    LP9OComment1 = "sscr. invalid date.  AGD " & _
        Format(AGD, "0#/0#/####") & " ES " & EnrSta & " ESD " & _
        Format(StaEffDt, "0#/0#/####") & " SCH " & SchCd & " CD " & _
        Format(SchCrtDt, "0#/0#/####") & " SRC " & Src
    Common.LP9O
    'add activity record
    comment = "sent to queue for review"
    Common.LP50
End Sub

'add an activity record when info cannot be updated because of the loan status
Private Sub Defaulted()
    comment = "sscr cannot update due to status of loans"
    Common.LP50
End Sub
