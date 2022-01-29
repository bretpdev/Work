Attribute VB_Name = "DMVResponse"
'log responses from requests for information from DMVs
Sub DMVResponse()
    With Session
        Script = "  {DMVRESP}"
        PauPlea = "Y"                                                   '<1>
        'prompt the user for ssn, state, and successful/unsuccessful response
        Load DMVRsp
        DMVRsp.Show
        SSN = DMVRsp.SSN
        State = DMVRsp.State
        Unload DMVRsp
'<1>    'access LP8Y
'        .TransmitTerminalKey rcIBMClearKey
'        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'        .TransmitANSI "LP8YCDFT;DDMVFLW;;" & SSN
'        .TransmitTerminalKey rcIBMEnterKey
'        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'        'prompt the user if not tasks are found
'        If .GetDisplayText(22, 3, 5) <> "46011" Then
'            Warn = MsgBox("No incomplete tasks were found for the SSN.  Verify the SSN and click OK to update LP22 and create an activity record or click Cancel to re-enter the SSN.", vbOKCancel)
'            'return to beginning of script for user to re-enter SSN if the dialog box is canceled
'            If Warn = 2 Then
'                DMVResponse
'            End If
'        'cancel available tasks
'        Else
'            row = 7
'            Do
'                If .GetDisplayText(row, 33, 1) = "A" Then
'                    .MoveCursor row, 33
'                    .TransmitANSI "X"
'                End If
'                If .GetDisplayText(row + 1, 33, 1) = " " Then
'                    row = 7
'                    If .GetDisplayText(2, 72, 2) = .GetDisplayText(2, 79, 2) Then
'                        .TransmitTerminalKey rcIBMEnterKey
'                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'                        .TransmitTerminalKey rcIBMPf6Key
'                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'                        Exit Do
'                    Else
'                        .TransmitTerminalKey rcIBMPf8Key
'                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'                    End If
'                Else
'                    row = row + 1
'                End If
'            Loop
'</1>   End If
        'set action code to unsuccessful
        ActCd = "KUDMV"
        'access LP22 and pause for the user to enter new information if response was successful
        If ResponseInd = 1 Then
            'access LP22
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "LP22C" & SSN
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'prompt the user
            MsgBox "Update the borrower demographics and hit Insert to continue."
            Do
                'pause
                .WaitForTerminalKey rcIBMInsertKey, "1:00:00"
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                .TransmitTerminalKey rcIBMPf6Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                'prompt the user to correct error if the information was not updated
                If .GetDisplayText(22, 3, 5) <> "49000" Then
                    MsgBox "The information entered was not updated.  Please make the necessary corrections and hit Insert to continue."
                Else
                    Exit Do
                End If
            Loop
            'set action code to successful
            ActCd = "KSDMV"
        End If
        'set codes
        ActTyp = "FO"
        ConTyp = "92"
        Comment = State
        'go to the Common.LP50 subroutine to add an activity record
        Common.LP50
        'return to the beginning of the script for the user to enter another borrower
        DMVResponse
    End With
End Sub

'<1> sr178, jd, 12/10/02
