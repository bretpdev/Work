Attribute VB_Name = "HEALCOHORT"
Private SchoolID As String
Private HEAL As String
Private Year1 As String
Private Rate1 As String
Private Year2 As String
Private Rate2 As String
Private Year3 As String
Private Rate3 As String
Private unsub As Boolean
Private CRate As Boolean
Private cnt As Integer
Private UserID As String
Private FTPFolder As String

Sub Main()
cnt = 0
With Session
        warn = MsgBox("This script updates Compass HEAL Indicator and Cohort Default.  Click OK to proceed or Cancel to quit.", vbOKCancel, "Update Compass HEAL Indicator and Cohort Default")
        If warn <> 1 Then End
        
        sp.Common.TestMode FTPFolder
        
        If Dir(FTPFolder & "DSASG06.txt") = "" Then
            MsgBox "The " & FTPFolder & "DSASG06.txt file is missing.", , "Update Compass HEAL Indicator and Cohort Default"
            End
        End If
        '.TransmitTerminalKey rcIBMClearKey
        '.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        '.TransmitANSI "LP40i" & SchoolID
        '.TransmitTerminalKey rcIBMEnterKey
        '.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        '.TransmitTerminalKey rcIBMEnterKey
        '.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'UserID = .GetDisplayText(3, 14, 7)
        Open FTPFolder & "DSASG06.txt" For Input As #1
        Input #1, SchoolID, HEAL, Rate1, Year1, Rate2, Year2, Rate3, Year3 'get ride of first line.
        Do Until EOF(1)
            unsub = False
            CRate = False
            Input #1, SchoolID, HEAL, Rate1, Year1, Rate2, Year2, Rate3, Year3
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "PO2OC" & SchoolID
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If GetText(22, 2, 5) = "01022" Then 'MAKE DESIRED DATA CHANGES AND PRESS ENTER
                If Trim(HEAL) <> "" Then
                    unsub = True
                    .MoveCursor 15, 26
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 15, 26
                    .TransmitANSI HEAL
                End If
                If Trim(Rate1) <> "" Then
                    CRate = True
                    .MoveCursor 17, 29
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 17, 29
                    .TransmitANSI Rate1
                End If
                If Trim(Rate2) <> "" Then
                    CRate = True
                    .MoveCursor 17, 46
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 17, 46
                    .TransmitANSI Rate2
                End If
                If Trim(Rate3) <> "" Then
                    CRate = True
                    .MoveCursor 17, 63
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 17, 63
                    .TransmitANSI Rate3
                End If
                If Trim(Year1) <> "" Then
                    CRate = True
                    .MoveCursor 17, 38
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 17, 38
                    .TransmitANSI Year1
                End If
                If Trim(Year2) <> "" Then
                    CRate = True
                    .MoveCursor 17, 55
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 17, 55
                    .TransmitANSI Year2
                End If
                If Trim(Year3) <> "" Then
                    CRate = True
                    .MoveCursor 17, 72
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 17, 72
                    .TransmitANSI Year3
                End If
                
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                
                If CRate = False And unsub = True And GetText(22, 2, 5) = "90003" Then
                    .MoveCursor 17, 29
                    .TransmitTerminalKey rcIBMEraseEOFKey
                    .MoveCursor 17, 29
                    .TransmitANSI "0"
                    .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                End If
                
                If GetText(22, 2, 5) = "01021" Then 'ADD NECESSARY DATA AND PRESS ENTER
                    cnt = cnt + 1
                    .MoveCursor 8, 18
                    .TransmitANSI "05"
                    .MoveCursor 9, 18
                    .TransmitANSI "12"
                    .MoveCursor 10, 18
                    .TransmitANSI "020"
                    .MoveCursor 14, 2
                    If unsub Then .TransmitANSI "Update School Unsub Limit Field. "
                    If CRate Then .TransmitANSI "Update School Cohort Default Rate. "
                    
                    .TransmitANSI "{HEALCOHORT}"
                ElseIf GetText(22, 2, 5) = "01003" Then 'NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED
                    'Nothing new added
                Else
                'Do While GetText(22, 2, 5) = "04963"
                    'MsgBox "There is an error with some of the Data entered. Please correct it and press Insert when done."
                    '.WaitForTerminalKey rcIBMInsertKey, "1:00:00"
                    '.TransmitTerminalKey rcIBMEnterKey
                    '.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                'Loop
                End If
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            End If
        
        Loop
        Close #1
        Kill FTPFolder & "DSASG06.txt"
        
        MsgBox "Script complete. " & cnt & " records updated."
End With
End Sub

'gets text and trims off extra spaces
'Function GetText(Y As Integer, X As Integer, L As Integer) As String
'    GetText = Trim(Session.GetDisplayText(Y, X, L))
'End Function

