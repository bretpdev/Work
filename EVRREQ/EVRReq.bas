Attribute VB_Name = "EVRReq"
Dim SSN As String
Dim Queue As String
Dim SchCd As String
Dim ReqQ As String
Dim FlwQ As String
Dim comment As String
Dim userName As String
Dim UserInit As String
Dim UserId As String
Dim row1 As Integer
Dim column1 As Integer
Dim Comm As String

'add a task to request enrollment information
Sub EVRReqMain()
    Dim TempSSN As String
    Dim MDCallNum As String
    Dim Temp() As String
    Dim UserSchCd As String
    Dim i As Integer
    Dim AN As String
    Dim ManuallyEnteredSchCd As Boolean
    With Session
        Okay = 0
    'gather info if it's MD's first call or if MD didn't call the script
    Common.CalledByMauiDUDE TempSSN, MDCallNum
    If MDCallNum = "" Or MDCallNum = "1" Then           '<3>
        'prompt the user for the necessary information
        Load EVRReqInfo
        If MDCallNum = "1" Then
            EVRReqInfo.SSN = TempSSN
        End If
        Do
            EVRReqInfo.Show
            If Len(EVRReqInfo.SSN.Text) = 10 Then
                AN = EVRReqInfo.SSN.Text
                SSN = ""
                SP.Common.GetLP22 SSN, AN
                EVRReqInfo.SSN.Text = SSN
            End If
            If Okay <> 1 Then Exit Do
            SSN = EVRReqInfo.SSN
            If EVRReqInfo.SchCd.TextLength <> 0 Then
                SchCd = EVRReqInfo.SchCd
                UserSchCd = EVRReqInfo.SchCd.Text
            Else
                Temp = Split(EVRReqInfo.cbSchCd.Text, " -- ")
                SchCd = Temp(1)
                UserSchCd = EVRReqInfo.cbSchCd.Value
            End If
            ReqQ = EVRReqInfo.ReqQ
            FlwQ = EVRReqInfo.FlwQ
            Comm = EVRReqInfo.tbComments.Text
            If FlwQ = "" Then
                warn = MsgBox("You must select a follow-up queue.  Click OK to return to the dialog box to select a queue.", 48, "No Follow-up Queue Selected")
            Else
                Exit Do
            End If
        Loop
        If MDCallNum = "1" And Okay <> 1 Then 'called from MD and cancelled
            'if script was cancelled then create empty file and end script
            Open "C:\Windows\Temp\Maui DUDE EVR Data File.txt" For Output As #1
            Close #1
            End
        ElseIf MDCallNum = "1" Then 'collect information and store in file
            Open "C:\Windows\Temp\Maui DUDE EVR Data File.txt" For Output As #1
            Write #1, SSN
            Write #1, UserSchCd
            Write #1, SchCd
            Write #1, ReqQ
            Write #1, FlwQ
            Write #1, Comm
            Close #1
            End
        ElseIf Okay <> 1 Then 'if script is cancelled
            End
        ElseIf MDCallNum = "" Then
            'check if manual school code entry
            If InStr(1, UserSchCd, " -- ") = False Then
                'check if LPSC has the school code
                While Not LPSC(SchCd)
                    Okay = 0
                    warn = MsgBox("You entered an invalid school code.  Click OK to return to the dialog box to reenter the school code.", 48, "Invalid School Code Entered")
                    EVRReqInfo.SchCd.Text = ""
                    EVRReqInfo.Show
                    If Okay <> 1 Then
                        Unload EVRReqInfo
                        If Dir("C:\Windows\Temp\Maui DUDE EVR Data File.txt") <> "" Then Kill "C:\Windows\Temp\Maui DUDE EVR Data File.txt"
                        End
                    End If
                    If EVRReqInfo.SchCd.TextLength <> 0 Then
                        SchCd = EVRReqInfo.SchCd
                    Else
                        Temp = Split(EVRReqInfo.cbSchCd.Text, " -- ")
                        SchCd = Temp(1)
                    End If
                Wend
            End If
        End If
        Unload EVRReqInfo
    ElseIf MDCallNum = "2" Then
        'if file MD file is empty then the script was cancelled earlier and the script must be ended
        If FileLen("C:\Windows\Temp\Maui DUDE EVR Data File.txt") = 0 Then End
        'if this is the second call from MD then pull data back out of file and process
        Open "C:\Windows\Temp\Maui DUDE EVR Data File.txt" For Input As #1
        Input #1, SSN
        Input #1, UserSchCd
        Input #1, SchCd
        Input #1, ReqQ
        Input #1, FlwQ
        Input #1, Comm
        Close #1
        Load EVRReqInfo
        EVRReqInfo.SSN = SSN
        If InStr(1, UserSchCd, " -- ") Then
            While EVRReqInfo.cbSchCd.list(i) <> UserSchCd
                i = i + 1
            Wend
            EVRReqInfo.cbSchCd.ListIndex = i
            ManuallyEnteredSchCd = False
        Else
            EVRReqInfo.SchCd.Text = UserSchCd
            ManuallyEnteredSchCd = True
        End If
        i = 0
        While EVRReqInfo.ReqQ.list(i) <> ReqQ
            i = i + 1
        Wend
        EVRReqInfo.ReqQ.ListIndex = i
        i = 0
        While EVRReqInfo.FlwQ.list(i) <> FlwQ
            i = i + 1
        Wend
        EVRReqInfo.FlwQ.ListIndex = i
        EVRReqInfo.tbComments.Text = Comm
        If ManuallyEnteredSchCd Then
            'check if LPSC has the school code
            While Not LPSC(SchCd)
                Okay = 0
                warn = MsgBox("You entered an invalid school code.  Click OK to return to the dialog box to reenter the school code.", 48, "Invalid School Code Entered")
                EVRReqInfo.SchCd.Text = ""
                EVRReqInfo.Show
                If Okay <> 1 Then
                    Unload EVRReqInfo
                    If Dir("C:\Windows\Temp\Maui DUDE EVR Data File.txt") <> "" Then Kill "C:\Windows\Temp\Maui DUDE EVR Data File.txt"
                    End
                End If
                If EVRReqInfo.SchCd.TextLength <> 0 Then
                    SchCd = EVRReqInfo.SchCd
                Else
                    Temp = Split(EVRReqInfo.cbSchCd.Text, " -- ")
                    SchCd = Temp(1)
                End If
            Wend
        End If
        Unload EVRReqInfo
    End If
    'get the user name
        'access LP40
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP40I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'hit enter to select the current user
        If .GetDisplayText(1, 77, 4) <> "ANCE" Then
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        End If
        'get the username "firstname lastname"
        userName = Trim(.GetDisplayText(5, 14, 12)) & " " & Trim(.GetDisplayText(4, 14, 35))
        UserInit = .GetDisplayText(5, 14, 1) & .GetDisplayText(4, 14, 1)
        UserId = Trim(.GetDisplayText(3, 14, 8))
    'add a request task
        comment = Comm
        Queue = ReqQ
        LP9O
    'add a follow-up task
        If FlwQ = "E1" Or FlwQ = "E2" Or FlwQ = "E3" Then
            TD22
        Else
            Queue = FlwQ
            LP9O
        End If
        
        SP.Common.AddLP50 SSN, "GEVRR", "EVRREQ", "AM", "10", UserSchCd & ", " & Comm & ", " & userName & " " & UserId
        
        MsgBox "Queue Tasks Created"
        If Dir("C:\Windows\Temp\Maui DUDE EVR Data File.txt") <> "" Then Kill "C:\Windows\Temp\Maui DUDE EVR Data File.txt"             '<3>
    End With
End Sub

'add a task to the specified queue
Sub LP9O()
    With Session
        'access LP9O
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP9OA" & SSN & ";;" & Queue
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        If SchCd <> "" Then
            XYInput 16, 12, SchCd
        End If
        'warn the user to correct erroroneous information until the Open Activity Detail screen is displayed
        Do
            If .GetDisplayText(22, 3, 5) = "44000" Then Exit Do
            warn = MsgBox("Click OK to correct the error or Cancel to quit.  Once the error has been corrected, hit Enter to access the Open Activity Detail screen and then hit Insert to resume processing.", vbOKCancel, "Correct Error")
            If warn = 2 Then End
            .WaitForTerminalKey rcIBMInsertKey, "1:00:00"
            .TransmitTerminalKey rcIBMInsertKey
        Loop
        'enter info
        If SchCd = "" Then
            .MoveCursor 16, 12
        Else
            .MoveCursor 16, 21
            SchCd = ""
        End If
        If .CursorColumn = 21 Then
            .TransmitANSI ", " & comment
        Else
            .TransmitANSI comment
        End If
        comment = Trim(.GetDisplayText(16, 12, 50) & " " & _
                        .GetDisplayText(17, 12, 50) & " " & _
                        .GetDisplayText(18, 12, 50))
        'enter the user's name
        .MoveCursor 19, 12
        .TransmitANSI userName
        'post the record
        .TransmitTerminalKey rcIBMPf6Key
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .Wait 2
    End With
End Sub

'add an action request
Sub TD22()
    With Session
        'access ATD22
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "TX3Z/ATD22" & SSN
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'locate the correct action code
        col = 8
        row = 8
        Do Until .GetDisplayText(row, col, 5) = "EVFL" & Mid(FlwQ, 2, 1)
            row = row + 1
            If row = 21 And col = 8 Then
                row = 8
                col = 48
            ElseIf row = 21 And col = 48 Then
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                row = 8
                col = 8
            End If
        Loop
        'select the code
        .MoveCursor row, col - 5
        .TransmitANSI "01"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'enter the recipient ID
        .MoveCursor 6, 32
        .TransmitANSI UserId
        'select the first loan
        .MoveCursor 11, 3
        .TransmitANSI "X"
        'enter the comment
        .MoveCursor 21, 2
        .TransmitANSI comment
        'add the user's initials
        'find the end of the comment
        row = 21
        col = 2
        Do Until .GetDisplayText(row, col, 6) = "______"
            col = col + 1
            'go to the next row
            If col = 74 And row = 21 Then
                row = 22
                col = 2
            End If
            'put the comment at the end of the second row if all the space is used
            If col = 74 And row = 22 Then
                col = 73
                Exit Do
            End If
        Loop
        'enter the user's initials
        .MoveCursor row, col
        .TransmitANSI "  / " & UserInit
        'save the changes
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
    End With
End Sub

'function returns valid column and row if found else zeros for both of them.
Function TextSearch(row As Integer, Column As Integer, Text As String) As Boolean
If (Session.FindText(Text, Column, row)) Then
Column = Session.FoundTextColumn
row = Session.FoundTextRow
TextSearch = True
Else
TextSearch = False
End If
End Function

'Checks for specific text at a certain location on the screen
Function Textcheck(Y As Integer, x As Integer, i As String)
    If (Session.GetDisplayText(Y, x, Len(i)) = i) Then
    Textcheck = True
    Else
    Textcheck = False
    End If
End Function

'Enters inp into the given X,Y coordinates
Function XYInput(Y As Integer, x As Integer, inp As String, Optional Enter As Boolean = False)
Session.MoveCursor Y, x
Session.TransmitANSI inp
'if enter = true then hit enter.
If (Enter) Then
Session.TransmitTerminalKey rcIBMEnterKey
Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
End If
End Function

'verify the school code is valid
Function LPSC(SchCd As String) As Boolean
    FastPath "LPSCI" & SchCd
    If Check4Text(1, 55, "INSTITUTION NAME/ID SEARCH") Then LPSC = False Else LPSC = True
End Function

