Attribute VB_Name = "AUTOFILEAV"
Sub Main()
Dim UserID As String
Dim day As String

'Get UserID
UserID = LP40()

'Only "UT00240" should run this script
If UserID <> "UT00240" Then
    MsgBox "UserID UT00240 is needed to run script", vbCritical, "Autodialer File Availability"
    ProcComp "AUTOFILEAVMBS.txt"
    End
End If

'Gets the numeric representation of the day of the week.
day = DOW(Date)

'Only proceed if it's Monday or Friday
If Not (day = 1 Or day = 5) Then
MsgBox "Script must only be run on Monday and Friday", vbCritical, "Autodialer File Availability"
    ProcComp "AUTOFILEAVMBS.txt"
    End
End If

'Monday
If day = 1 Then
    SP.Q.FastPath "TX3Z/CTX33"
    SP.Q.puttext 8, 18, "WC-AUTO-UPL", "Enter"
    SP.Q.puttext 6, 49, "I", "Enter"
    If SP.Q.Check4Text(23, 2, "01005") = True Then
        ProcComp "AUTOFILEAVMBS.txt"
        End
    Else
        MsgBox "There was a problem updating the record, please contact Systems Support.", vbExclamation, "Autodialer File Availability"
        ProcComp "AUTOFILEAVMBS.txt"
        End
    End If
    
'Friday
Else
    SP.Q.FastPath "TX3Z/CTX33"
    SP.Q.puttext 8, 18, "WC-AUTO-UPL", "Enter"
    SP.Q.puttext 6, 49, "A", "Enter"
    If SP.Q.Check4Text(23, 2, "01005") = True Then
        ProcComp "AUTOFILEAVMBS.txt"
        End
    Else
        MsgBox "There was a problem updating the record, please contact Systems Support.", vbExclamation, "Autodialer File Availability"
        ProcComp "AUTOFILEAVMBS.txt"
        End
    End If
End If

End Sub

Function LP40() As String
    With Session
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP40I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        If .GetDisplayText(1, 77, 4) <> "ANCE" Then
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        End If
        LP40 = .GetDisplayText(3, 14, 7)
    End With
End Function

Public Function DOW(ByVal GregDate As Date) As String
' Return values:
' 0 = Sunday
' 1 = Monday
' 2 = Tuesday
' 3 = Wednesday
' 4 = Thursday
' 5 = Friday
' 6 = Saturday
    Dim Y As Integer
    Dim m As Integer
    Dim d As Integer
    
    ' monthdays:
    ' This is a "template" for a year. Each number
    ' stands for a day of the week. The general idea
    ' is that, in a standard year, if Jan 1 is on a
    ' Friday, then Feb 1 will be a Monday, Mar 1
    ' will be a Monday, April 1 will be a Thursday,
    ' May 1 will be Saturday, etc..
    Dim mcode As String
    Dim monthdays() As String
    monthdays = Split("5 1 1 4 6 2 4 0 3 5 1 3")
    
    ' Grab our date info
    Y = Val(Format(GregDate, "yyyy"))
    m = Val(Format(GregDate, "mm"))
    d = Val(Format(GregDate, "dd"))
    
    ' Snatch the corresponding month code
    mcode = Val(monthdays(m - 1))
    
    ' Multiplying by 1.25 takes care of leap years,
    ' but not completely. Jan and Feb of a leap year
    ' will end up a day extra.
    ' The 'mod 7' gives us our day.
    DOW = ((Int(Y * 1.25) + mcode + d) Mod 7)
    
    ' This takes care of leap year Jan and Feb days.
    If Y Mod 4 = 0 And m < 3 Then DOW = (DOW + 6) Mod 7
End Function



