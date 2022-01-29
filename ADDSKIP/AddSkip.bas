Attribute VB_Name = "AddSkip"
'add an administrative skip on LP5C
Sub AddSkipMain()
    With Session
    'prompt the user for the SSN and servicer
        AddSkipFrm.Show
        SSN = AddSkipFrm.cSSN
    'add the skip on LP5C
        'access LP5C
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP5CA" & SSN
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'warn the user and end the script if the target is not displayed
        If .GetDisplayText(1, 74, 3) <> "DIS" Then
            MsgBox "The Skiptracing Request screen was not displayed.  Check the SSN and run the script again.", , "Skip Not Added"
            End
        End If
        'enter the INSTITUTION ID
        .MoveCursor 3, 39
        Select Case AddSkipFrm.Servicer
            Case "Nelnet"
                .TransmitANSI "700121"
            Case "SallieMae"
                .TransmitANSI "700191"
            Case "UHEAA Account Services", "UHEAA Borrower Services"
                .TransmitANSI "700126"
        End Select
        'enter the INSTITUTION TYPE
        .MoveCursor 3, 48
        .TransmitANSI "003"
        'enter the SEQUENCE TYPE CODE
        .MoveCursor 5, 22
        .TransmitANSI "20"
        'post the changes
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'warn the user if there is an error and end the script
        If .GetDisplayText(22, 3, 5) <> "48003" Then
'<2->
'           MsgBox "Either the address or phone number must be invalid to add a skip.  This borrower has both a valid address and valid phone number.  Please review to verify that skip activity is needed.", , "Skip Not Added"
            If .GetDisplayText(22, 3, 5) = "40192" Then
                MsgBox "This borrower is not a skip.  Both the Address and Phone are Valid.", vbCritical, "Borrower not a Skip"
            ElseIf .GetDisplayText(22, 3, 5) = "04016" Then
                MsgBox "This is a duplicate skip tracing request.", vbCritical, "Duplicate Skip Tracing Request"
            ElseIf .GetDisplayText(22, 3, 5) = "00029" Then
                MsgBox "The borrower's loans are not eligible for skip tracing.", vbCritical, "Loans not Eligible for Skip Tracing"
            End If
'</2>
            CantAddAsSkip
            End
        End If
    'add an activity record
        ActCd = "KRSKP"
        ActTyp = "AM"
        ConTyp = "10"
        Comment = "skip only request from " & AddSkipFrm.Servicer
        Common.LP50
        
    'finis
'<1->
        If AddSkipFrm.GetKSKPREQTask() Then
            CompleteTask
        End If
'</1>
        Unload AddSkipFrm
        MsgBox "Processing Complete.", , "Processing Complete"
    End With
End Sub

'<1->
Function CompleteTask()
    FastPath "LP9ACKSKPREQ;;SKP;;"
    Hit "F6"
End Function

Function CantAddAsSkip()
    SP.Common.AddLP50 SSN, "KGNRL", "ADDSKIP", "AM", "10", "Admin Skip request not processed, borrower not eligible for skip tracing."
    CompleteTask
End Function
'</1>

'new sr 580, jd, 04/08/04, 04/12/04
'<1> sr 771, aa, 09/07/04, 10/14/04
'<2> sr1986, jd
