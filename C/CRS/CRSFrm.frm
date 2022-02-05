VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} CRSFrm 
   Caption         =   "Compass CRS"
   ClientHeight    =   5775
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5700
   OleObjectBlob   =   "CRSFrm.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "CRSFrm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Result As Integer
Private CRS() As Integer    'CRS codes
Private c As Integer        'number of codes
Private YN As Variant


'<1->
'set up list for combo boxes
Private Sub UserForm_Initialize()
    YN = Array("Y", "N")
    Prn1.list() = YN
    Prn2.list() = YN
    Prn3.list() = YN
    Prn4.list() = YN
End Sub
'</1>

'the user clicked Add
Private Sub CommandButton1_Click()
    Result = 1
    VerifyInfo
End Sub

'the user clicked Remove
Private Sub CommandButton2_Click()
    Result = 2
    VerifyInfo
End Sub

'the user clicked Cancel
Private Sub CommandButton3_Click()
    Me.Hide
    End
End Sub

'<1->
'the user clicked Change
Private Sub CommandButton4_Click()
    If (Prn1 = "Y" And BrwARC = "") Or (Prn2 = "Y" And SchARC = "") Or (Prn3 = "Y" And LndrARC = "") Or (Prn4 = "Y" And EndrARC = "") Then
        MsgBox "'Y' was entered for a print indicator but no corresponding ARC was entered.", , "Missing ARC"
    ElseIf Prn1 = "" And Prn2 = "" And Prn3 = "" And Prn4 = "" Then
        MsgBox "A value ('Y' or 'N') must be entered for each print indicator.", , "Missing Value"
    Else
        Result = 4
        VerifyInfo
    End If
End Sub
'</1>

'verify info is okay
Private Sub VerifyInfo()
    'warn the user and redisplay the user form if there is a data error
    If LenderID <> "" And CRSFrm.SchoolID <> "" Then
        MsgBox "You must enter either the lender or school ID but not both.", , "Invalid Entry"
    ElseIf CRSFrm.LenderID = "" And CRSFrm.SchoolID = "" Then
        MsgBox "You must enter either the lender or school ID.", , "Invalid Entry"
    ElseIf CRSCodes = "" Then
        MsgBox "You must enter at least one CRS code.", , "Invalid Entry"
    ElseIf ParseCodes(CRSCodes) = False Then
        MsgBox "An invalid character was found in your list of codes.  You must enter multiple four-digit codes separated by commas (0001,0002,0004), or in ranges indicated by a hyphen (0001-1000), or using a combination of both (0001-0003,0005).", , "Invalid Loan Code List"
    'process the data if there are no errors
    Else
        Me.Hide
        ProcControl
    End If
End Sub

'parse individual codes out of string entered by the user
Function ParseCodes(CdStr As String) As Boolean
    Dim CommaX As Integer   'position of next comma
    Dim HyphenX As Integer  'position of hyphen
    Dim CdWrk() As String   'loan sequence number strings
    Dim x As Integer        'number of codes
    Dim Y As Integer        'range counter
    
    'assign each code to the CRS array while the working variable has commas (indicating more codes are contained therein)
    x = 0
    Do
        'determine the position of the next comma
        CommaX = InStr(1, CdStr, ",")
        If CommaX = 0 Then Exit Do
        'parse out the code and assign it to the variable
        x = x + 1
        ReDim Preserve CdWrk(1 To x) As String
        CdWrk(x) = Trim(Mid(CdStr, 1, CommaX - 1))
        CdStr = Mid(CdStr, CommaX + 1, Len(CdStr))
    Loop
    'assign the last or only code to the array
    x = x + 1
    ReDim Preserve CdWrk(1 To x) As String
    CdWrk(x) = Trim(CdStr)
    'assign codes to the array for ranges entered using a hyphen
    For i = 1 To x
        'determine the position of the hyphen
        HyphenX = InStr(1, CdWrk(i), "-")
        'write loan numbers to the array if there is a hyphen
        If HyphenX <> 0 Then
            For j = Val(Mid(CdWrk(i), 1, HyphenX - 1)) To Val(Mid(CdWrk(i), HyphenX + 1, Len(CdWrk(i))))
                x = x + 1
                ReDim Preserve CdWrk(1 To x) As String
                CdWrk(x) = str(j)
            Next j
            CdWrk(i) = "0"
        End If
    Next i
    'copy values from working array to permanent array skipping zero code numbers (used to indicate working array variables that previously indicated ranges) and verify that all numbers are valid numbers
    c = 0
    For i = 1 To x
        If IsNumeric(CdWrk(i)) = True And CdWrk(i) <> "0" Then
            c = c + 1
            ReDim Preserve CRS(1 To c) As Integer
            CRS(c) = Val(CdWrk(i))
        ElseIf IsNumeric(CdWrk(i)) = False Then
            ParseCodes = False
            Exit Function
        End If
    Next i
    ParseCodes = True
End Function

'control processing flow
Private Sub ProcControl()
    Dim ScrLtr As String
    Dim OrgID As String
    Dim i As Integer

    'set generic values based on lender/school info
    If LenderID <> "" Then
        ScrLtr = "L"
        OrgID = LenderID
    Else
        OrgID = SchoolID
        ScrLtr = "M"
    End If
    'add codes
    If Result = 1 Then
        For i = 1 To c
            AddProc ScrLtr, OrgID, "STFFRD", i
        Next i
        For i = 1 To c
            AddProc ScrLtr, OrgID, "UNSTFD", i
        Next i
        For i = 1 To c
            AddProc ScrLtr, OrgID, "PLUS", i
        Next i
        For i = 1 To c
            AddProc ScrLtr, OrgID, "PLUSGB", i
        Next i
    'remove codes
'<1->
'   Else
    ElseIf Result = 2 Then
'</1>
        For i = 1 To c
            RemoveProc ScrLtr, OrgID, "STFFRD", i
        Next i
        For i = 1 To c
            RemoveProc ScrLtr, OrgID, "UNSTFD", i
        Next i
        For i = 1 To c
            RemoveProc ScrLtr, OrgID, "PLUS", i
        Next i
        For i = 1 To c
            RemoveProc ScrLtr, OrgID, "PLUSGB", i
        Next i
'<1->
    Else
        For i = 1 To c
            ChangeProc ScrLtr, OrgID, "STFFRD", i
        Next i
        For i = 1 To c
            ChangeProc ScrLtr, OrgID, "UNSTFD", i
        Next i
        For i = 1 To c
            ChangeProc ScrLtr, OrgID, "PLUS", i
        Next i
        For i = 1 To c
            ChangeProc ScrLtr, OrgID, "PLUSGB", i
        Next i
'</1>
    End If
End Sub

'add information
Private Sub AddProc(Scr, ID, LnTyp As String, j As Integer)
    With Session
        'access PO7[L/M]
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "TX3Z/APO7" & Scr & ID & ";" & LnTyp & ";" & Format(CRS(j), "0000")
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'<2->
        'change to change mode if the combination already exists
        If .GetDisplayText(22, 2, 5) = "04623" Then
            'change to change mode
            .MoveCursor 1, 2
            .TransmitANSI "C"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        End If
'</2>
        'add the info to an error file if the target is not displayed
        If .GetDisplayText(1, 75, 5) = "POX7N" Then
            Open "T:\CRSerr.txt" For Append As #1
            Write #1, ID, LnTyp, Format(CRS(j), "0000"), Trim(.GetDisplayText(22, 2, 70)), Format(Date, "MM/DD/YYYY")
            Close #1
            Exit Sub
        End If
'<2->
        'update the status to active if it isn't already
        If .GetDisplayText(13, 26, 1) <> "A" Then
            .MoveCursor 13, 26
            .TransmitANSI "A"
        End If
'</2>
        'update the screen
        If .GetDisplayText(12, 26, 1) = "C" Then
            .MoveCursor 18, 17
            .TransmitANSI "Y"
            .MoveCursor 18, 22
            .TransmitANSI "O1000"
            .MoveCursor 18, 63
            .TransmitANSI "Y"
            .MoveCursor 18, 68
            .TransmitANSI "O1001"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'hit enter
        ElseIf .GetDisplayText(12, 26, 1) = "S" Then
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        End If
        
    End With
End Sub

'inactive the code
Private Sub RemoveProc(Scr, ID, LnTyp As String, j As Integer)
    With Session
        'access PO7[L/M]
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "TX3Z/CPO7" & Scr & ID & ";" & LnTyp & ";" & Format(CRS(j), "0000")
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        If .GetDisplayText(1, 75, 5) = "POX7N" Then
            Open "T:\CRSerr.txt" For Append As #1
            Write #1, ID, LnTyp, Format(CRS(j), "0000"), Trim(.GetDisplayText(22, 2, 70)), Format(Date, "MM/DD/YYYY")
            Close #1
            Exit Sub
        End If
        'update the status
        .MoveCursor 13, 26
        .TransmitANSI "I"
        'post the changes
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
    End With
End Sub

'<1->
'change the print indicator(s) and arc(s)
Private Sub ChangeProc(Scr, ID, LnTyp As String, j As Integer)
    With Session
        'access PO7[L/M]
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "TX3Z/CPO7" & Scr & ID & ";" & LnTyp & ";" & Format(CRS(j), "0000")
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        If .GetDisplayText(1, 75, 5) = "POX7N" Then
            Open "T:\CRSerr.txt" For Append As #1
            Write #1, ID, LnTyp, Format(CRS(j), "0000"), Trim(.GetDisplayText(22, 2, 70)), Format(Date, "MM/DD/YYYY")
            Close #1
            Exit Sub
        End If
        If .GetDisplayText(12, 26, 1) = "C" Then
            'update the letter print info
            .MoveCursor 18, 17
            .TransmitANSI Prn1
            .MoveCursor 18, 22
            .TransmitANSI BrwARC
            .MoveCursor 18, 33
            .TransmitANSI Prn2
            .MoveCursor 18, 38
            .TransmitANSI SchARC
            .MoveCursor 18, 48
            .TransmitANSI Prn3
            .MoveCursor 18, 53
            .TransmitANSI LndrARC
            .MoveCursor 18, 63
            .TransmitANSI Prn4
            .MoveCursor 18, 68
            .TransmitANSI EndrARC
            'post the changes
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        ElseIf .GetDisplayText(12, 26, 1) = "S" Then
            warn = MsgBox("A suspension record can only be added or removed, not changed.  Click OK to continue processing with the next CRS code or click Cancel to quit.", vbOKCancel, "Suspension Record")
            If warn <> 2 Then End
        End If
    End With
End Sub

'new sr611, jd, 04/01/04, 04/02/04
'<1> sr617, jd, 04/12/04, 04/12/04
'<2> sr621, jd, 04/15/04, 04/16/04
'<3> sr1895, tp, 11/22/06
