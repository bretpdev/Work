Attribute VB_Name = "INTERNET"
Dim ActCd As String
Dim Comment As String
Dim ConTyp As String
Dim ActTyp As String

Sub INTERNET()
    With Session
        'warn the user of the purpose of the script and end the script if the dialog box is cancelled
        warn = MsgBox("This script completes all tasks in the INTERNET queue.  Click OK to continue or Cancel to quit.", vbOKCancel, "Complete INTERNET")
        If warn <> 1 Then End
        'go to LP9A
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
            .TransmitANSI "LP9AC"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
            If .GetDisplayText(1, 66, 5) = "QUEUE" Then
                .TransmitANSI "INTERNET"
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
                Else: MsgBox "You have an unresolved task in the " & .GetDisplayText(1, 9, 8) & " queue.  You must complete the task before working the INTERNET queue."
                End
            End If
        Do
        'determine whether SSN is borrower or endorser
            If .GetDisplayText(9, 23, 1) = "B" Then
                ActCd = "KUBIT"
            End If
            If .GetDisplayText(9, 23, 1) = "E" Then
                ActCd = "KABA2"
            End If
        'use hotkeys to access LP50
            .TransmitTerminalKey rcIBMPf2Key
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
            .TransmitTerminalKey rcIBMPf10Key
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        'enter codes and the comment
            Comment = "INTERNET task completed by script.  Task not a due diligence requirement   {INTERNET}"
            ActTyp = "TT"
            ConTyp = "36"
            .TransmitANSI ActCd
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI ActTyp
            .TransmitANSI ConTyp
            .MoveCursor 13, 2
            .TransmitANSI Comment
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitTerminalKey rcIBMPf6Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
         'return to LP9A to complete the task
            .TransmitTerminalKey rcIBMPf12Key
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
         'complete task
            .TransmitTerminalKey rcIBMPf2Key
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
            .TransmitTerminalKey rcIBMPf6Key
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
         'select next task
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
         'warn the user and end the script if no more tasks are found
            If .GetDisplayText(22, 3, 5) = "46004" Then Exit Do
        Loop
            MsgBox "There are no more tasks in the INTERNET queue.  Processing is complete."
            End
   End With
End Sub
