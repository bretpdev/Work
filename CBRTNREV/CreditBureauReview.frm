VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} CreditBureauReview 
   Caption         =   "Credit Bureau Review"
   ClientHeight    =   7650
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   8550
   OleObjectBlob   =   "CreditBureauReview.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "CreditBureauReview"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Private RawData() As String
Private FormValues() As String
Private LastTabOn As Integer 'tracks the last tab that the script was on so it can save changes to that record after the tab has changed see Save and populatetab functions

Private Sub btnBlankForm_Click()
    Tabs.Tabs.Add "Page #" & Tabs.Tabs.Count + 1, "Page #" & Tabs.Tabs.Count + 1
    ReDim Preserve FormValues(13, UBound(FormValues, 2) + 1)
     'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
    '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
    '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
    FormValues(1, UBound(FormValues, 2) - 1) = FormValues(1, 0)
    FormValues(2, UBound(FormValues, 2) - 1) = FormValues(2, 0)
    FormValues(13, UBound(FormValues, 2) - 1) = FormValues(13, 0)
End Sub

Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnComplete_Click()
    Dim KSBCRComment As String
    Dim KUBCRComment As String
    Dim KSECRComment As String
    Dim KUECRComment As String
    Dim K4AKAComment As String
    Dim MEMPLComment As String
    Dim KNOTEComment As String
    'save what ever is on the currently viewed tab
    FormValues(11, Tabs.Value) = tbReComments.Text
    If FigureCommentCode() <> "" Then FormValues(12, Tabs.Value) = FigureCommentCode()
    'do the comment thing
    If CommentCodesMarked() Then 'check if all pages have
        CompileComments KSBCRComment, KUBCRComment, KSECRComment, KUECRComment, K4AKAComment, MEMPLComment, KNOTEComment  'compile comment strings
        CreateAndEnterCommentParts KSBCRComment, KUBCRComment, KSECRComment, KUECRComment, K4AKAComment, MEMPLComment, KNOTEComment  'create comment parts for multiple record entering and enter the parts in LP50
        'complete task
        FastPath "LP9ACFCREDITB"
        Hit "F6"
        If vbYes = MsgBox("Do you want to select and work the next queue task?", vbYesNo, "Next Queue Task") Then
            Tabs.Tabs.Clear
            Tabs.Tabs.Add "Page #1", "Page #1"
            RefreshForm
            Init
        Else
            End
        End If
    End If
End Sub

'this function splits each comment into system inputable parts (app 600 chars) and enters them into LP50
Function CreateAndEnterCommentParts(KSBCR As String, KUBCR As String, KSECR As String, KUECR As String, K4AKA As String, MEMPL As String, KNOTE As String)
    Dim SizeOfFullComment As Long
    Dim NumberOfCommentEntries As Integer
    Dim counter As Integer
    'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
    '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
    '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
    '------------------------------------------KSBCR Comment-------------------------------------------------------------------
    SizeOfFullComment = Len(KSBCR)
    If SizeOfFullComment <> 0 Then 'add the comment if there is text
        'figure out how many comment entries will be needed
        While SizeOfFullComment > 0
            SizeOfFullComment = SizeOfFullComment - 570 '570 is where the comment will be cut off.  the last 50 spaces will be used for other script populated info
            NumberOfCommentEntries = NumberOfCommentEntries + 1
        Wend
        While counter < NumberOfCommentEntries
            'if first time in loop pick up the first 480 characters
            If counter = 0 Then
                AddCommentOnLP50 Mid(KSBCR, 1, 570) & " {CBRTNREV} /", "KSBCR"
            Else 'for all other times through other than the first.
                AddCommentOnLP50 Mid(KSBCR, ((counter * 570) + 1), 570) & " {CBRTNREV} /", "KSBCR"
            End If
            counter = counter + 1
        Wend
        NumberOfCommentEntries = 0
        counter = 0
    End If
    '------------------------------------------KUBCR Comment-------------------------------------------------------------------
    SizeOfFullComment = Len(KUBCR)
    If SizeOfFullComment <> 0 Then 'add the comment if there is text
        'figure out how many comment entries will be needed
        While SizeOfFullComment > 0
            SizeOfFullComment = SizeOfFullComment - 570 '570 is where the comment will be cut off.  the last 50 spaces will be used for other script populated info
            NumberOfCommentEntries = NumberOfCommentEntries + 1
        Wend
        While counter < NumberOfCommentEntries
            'if first time in loop pick up the first 480 characters
            If counter = 0 Then
                AddCommentOnLP50 Mid(KUBCR, 1, 570) & " {CBRTNREV} /", "KUBCR"
            Else 'for all other times through other than the first.
                AddCommentOnLP50 Mid(KUBCR, ((counter * 570) + 1), 570) & " {CBRTNREV} /", "KUBCR"
            End If
            counter = counter + 1
        Wend
        NumberOfCommentEntries = 0
        counter = 0
    End If
    '------------------------------------------KSECR Comment-------------------------------------------------------------------
    SizeOfFullComment = Len(KSECR)
    If SizeOfFullComment <> 0 Then 'add the comment if there is text
        'figure out how many comment entries will be needed
        While SizeOfFullComment > 0
            SizeOfFullComment = SizeOfFullComment - 570 '570 is where the comment will be cut off.  the last 50 spaces will be used for other script populated info
            NumberOfCommentEntries = NumberOfCommentEntries + 1
        Wend
        While counter < NumberOfCommentEntries
            'if first time in loop pick up the first 480 characters
            If counter = 0 Then
                AddCommentOnLP50 Mid(KSECR, 1, 570) & " {CBRTNREV} /", "KSECR"
            Else 'for all other times through other than the first.
                AddCommentOnLP50 Mid(KSECR, ((counter * 570) + 1), 570) & " {CBRTNREV} /", "KSECR"
            End If
            counter = counter + 1
        Wend
        NumberOfCommentEntries = 0
        counter = 0
    End If
    '------------------------------------------KUECR Comment-------------------------------------------------------------------
    SizeOfFullComment = Len(KUECR)
    If SizeOfFullComment <> 0 Then 'add the comment if there is text
        'figure out how many comment entries will be needed
        While SizeOfFullComment > 0
            SizeOfFullComment = SizeOfFullComment - 570 '570 is where the comment will be cut off.  the last 50 spaces will be used for other script populated info
            NumberOfCommentEntries = NumberOfCommentEntries + 1
        Wend
        While counter < NumberOfCommentEntries
            'if first time in loop pick up the first 480 characters
            If counter = 0 Then
                AddCommentOnLP50 Mid(KUECR, 1, 570) & " {CBRTNREV} /", "KUECR"
            Else 'for all other times through other than the first.
                AddCommentOnLP50 Mid(KUECR, ((counter * 570) + 1), 570) & " {CBRTNREV} /", "KUECR"
            End If
            counter = counter + 1
        Wend
        NumberOfCommentEntries = 0
        counter = 0
    End If
    '------------------------------------------K4AKA Comment-------------------------------------------------------------------
    SizeOfFullComment = Len(K4AKA)
    If SizeOfFullComment <> 0 Then 'add the comment if there is text
        'figure out how many comment entries will be needed
        While SizeOfFullComment > 0
            SizeOfFullComment = SizeOfFullComment - 570 '570 is where the comment will be cut off.  the last 50 spaces will be used for other script populated info
            NumberOfCommentEntries = NumberOfCommentEntries + 1
        Wend
        While counter < NumberOfCommentEntries
            'if first time in loop pick up the first 480 characters
            If counter = 0 Then
                AddCommentOnLP50 Mid(K4AKA, 1, 570) & " {CBRTNREV} /", "K4AKA"
            Else 'for all other times through other than the first.
                AddCommentOnLP50 Mid(K4AKA, ((counter * 570) + 1), 570) & " {CBRTNREV} /", "K4AKA"
            End If
            counter = counter + 1
        Wend
        NumberOfCommentEntries = 0
        counter = 0
    End If
    '------------------------------------------MEMPL Comment-------------------------------------------------------------------
    SizeOfFullComment = Len(MEMPL)
    If SizeOfFullComment <> 0 Then 'add the comment if there is text
        'figure out how many comment entries will be needed
        While SizeOfFullComment > 0
            SizeOfFullComment = SizeOfFullComment - 570 '570 is where the comment will be cut off.  the last 50 spaces will be used for other script populated info
            NumberOfCommentEntries = NumberOfCommentEntries + 1
        Wend
        While counter < NumberOfCommentEntries
            'if first time in loop pick up the first 480 characters
            If counter = 0 Then
                AddCommentOnLP50 Mid(MEMPL, 1, 570) & " {CBRTNREV} /", "MEMPL"
            Else 'for all other times through other than the first.
                AddCommentOnLP50 Mid(MEMPL, ((counter * 570) + 1), 570) & " {CBRTNREV} /", "MEMPL"
            End If
            counter = counter + 1
        Wend
        NumberOfCommentEntries = 0
        counter = 0
    End If
    '------------------------------------------KNOTE Comment-------------------------------------------------------------------
    SizeOfFullComment = Len(KNOTE)
    If SizeOfFullComment <> 0 Then 'add the comment if there is text
        'figure out how many comment entries will be needed
        While SizeOfFullComment > 0
            SizeOfFullComment = SizeOfFullComment - 570 '570 is where the comment will be cut off.  the last 50 spaces will be used for other script populated info
            NumberOfCommentEntries = NumberOfCommentEntries + 1
        Wend
        While counter < NumberOfCommentEntries
            'if first time in loop pick up the first 480 characters
            If counter = 0 Then
                AddCommentOnLP50 Mid(KNOTE, 1, 570) & " {CBRTNREV} /", "KNOTE"
            Else 'for all other times through other than the first.
                AddCommentOnLP50 Mid(KNOTE, ((counter * 570) + 1), 570) & " {CBRTNREV} /", "KNOTE"
            End If
            counter = counter + 1
        Wend
        NumberOfCommentEntries = 0
        counter = 0
    End If
End Function

'enter comments in LP50
Sub AddCommentOnLP50(Comment As String, Code As String)
    With Session
        'access LP50
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LP50A" & tbSSN.Text
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI Code
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        .TransmitANSI "ED"
        .TransmitANSI "84"
        'enter the comment
        .MoveCursor 13, 2
        'if the comment is less than 260
        If 260 > Len(Comment) Then
            .TransmitANSI Comment
        Else 'if it is going to take at least two .transmitansi statements
            If (260 < Len(Comment) And 520 > Len(Comment)) Then 'if it is going to take two .transmitansi statements
                .TransmitANSI Mid(Comment, 1, 260)
                .TransmitANSI Mid(Comment, 261, (Len(Comment) - 261))
            ElseIf (520 < Len(Comment)) Then 'if it is going to take two .transmitansi statements
                .TransmitANSI Mid(Comment, 1, 260)
                .TransmitANSI Mid(Comment, 261, 260)
                .TransmitANSI Mid(Comment, 521, (Len(Comment) - 521))
            End If
        End If
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
        .TransmitTerminalKey rcIBMPf6Key
        .WaitForEvent rcKbdEnabled, "60", "0", 1, 1
    End With
End Sub

'this function compiles the comment strings for entry
Function CompileComments(KSBCR As String, KUBCR As String, KSECR As String, KUECR As String, K4AKA As String, MEMPL As String, KNOTE As String)
    'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
    '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
    '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
    Dim counter As Integer
    While counter < UBound(FormValues, 2)
        If FormValues(12, counter) = "KSBCR" Then
            If KSBCR = "" Then
                KSBCR = FormValues(11, counter)
            Else
                KSBCR = KSBCR & " " & FormValues(11, counter)
            End If
        ElseIf FormValues(12, counter) = "KUBCR" Then
            If KUBCR = "" Then
                KUBCR = FormValues(11, counter)
            Else
                KUBCR = KUBCR & " " & FormValues(11, counter)
            End If
        ElseIf FormValues(12, counter) = "KSECR" Then
            If KSECR = "" Then
                KSECR = FormValues(11, counter)
            Else
                KSECR = KSECR & " " & FormValues(11, counter)
            End If
        ElseIf FormValues(12, counter) = "KUECR" Then
            If KUECR = "" Then
                KUECR = FormValues(11, counter)
            Else
                KUECR = KUECR & " " & FormValues(11, counter)
            End If
        ElseIf FormValues(12, counter) = "K4AKA" Then
            If K4AKA = "" Then
                K4AKA = FormValues(11, counter)
            Else
                K4AKA = K4AKA & " " & FormValues(11, counter)
            End If
        ElseIf FormValues(12, counter) = "MEMPL" Then
            If MEMPL = "" Then
                MEMPL = FormValues(11, counter)
            Else
                MEMPL = MEMPL & " " & FormValues(11, counter)
            End If
        ElseIf FormValues(12, counter) = "KNOTE" Then
            If KNOTE = "" Then
                KNOTE = FormValues(11, counter)
            Else
                KNOTE = KNOTE & " " & FormValues(11, counter)
            End If
        End If
        counter = counter + 1
    Wend
End Function

'this function checks to be sure that all pages have a comment code marked and that at least one of them is not Bypass
Function CommentCodesMarked() As Boolean
    'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
    '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
    '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
    Dim counter As Integer
    Dim NonBypassFound As Boolean
    While counter < UBound(FormValues, 2)
        If FormValues(12, counter) = "" Then
            MsgBox "All pages need a comment code marked.  Please review all pages and select a comment code where needed."
            Exit Function 'Returns false
        End If
        If FormValues(12, counter) <> "Bypass" Then
            If FormValues(11, counter) <> "" Then
                NonBypassFound = True
            Else
                NonBypassFound = False
                MsgBox "All pages with a valid comment code marked must have a comment text populated."
                Exit Function
            End If
        End If
        counter = counter + 1
    Wend
    If NonBypassFound = False Then
        MsgBox "You can't bypass every page.  One of them must have a valid comment code selected and the response comment field populated."
        Exit Function 'returns false
    End If
    CommentCodesMarked = True
End Function

Private Sub btnPage_Click()
    If Tabs.Value + 1 = Tabs.Tabs.Count Then
        Tabs.Value = 0
    Else
        Tabs.Value = Tabs.Value + 1
    End If
End Sub

'this function refreshes and blanks the form controls
Function RefreshForm()
    'blank boxes
    tbAccNum.Text = ""
    tbAC.Text = ""
    tbSSN.Text = ""
    tbAD.Text = ""
    tbCF1.Text = ""
    tbCF2.Text = ""
    tbCF3.Text = ""
    tbCF4.Text = ""
    tbCF5.Text = ""
    tbCF6.Text = ""
    tbCF7.Text = ""
    tbCF8.Text = ""
    tbReComments.Text = ""
    'reset radio button values
    rbKSBCR.Value = False
    rbKUBCR.Value = False
    rbKSECR.Value = False
    rbKUECR.Value = False
    rbK4AKA.Value = False
    rbMEMPL.Value = False
    rbKNOTE.Value = False
    rbBypass.Value = False
End Function

'this function populates the tab that will be displayed
Function PopulateTab()
    'populate boxes
    'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
    '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
    '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
    tbAC.Text = FormValues(0, Tabs.Value)
    tbSSN.Text = FormValues(1, Tabs.Value)
    tbAD.Text = FormValues(2, Tabs.Value)
    tbAccNum.Text = FormValues(13, Tabs.Value)
    tbCF1.Text = FormValues(3, Tabs.Value)
    tbCF2.Text = FormValues(4, Tabs.Value)
    tbCF3.Text = FormValues(5, Tabs.Value)
    tbCF4.Text = FormValues(6, Tabs.Value)
    tbCF5.Text = FormValues(7, Tabs.Value)
    tbCF6.Text = FormValues(8, Tabs.Value)
    tbCF7.Text = FormValues(9, Tabs.Value)
    tbCF8.Text = FormValues(10, Tabs.Value)
    tbReComments.Text = FormValues(11, Tabs.Value)
    'reset radio button values
    rbKSBCR.Value = False
    rbKUBCR.Value = False
    rbKSECR.Value = False
    rbKUECR.Value = False
    rbK4AKA.Value = False
    rbMEMPL.Value = False
    rbKNOTE.Value = False
    rbBypass.Value = False
    'enter selection that the user made previously if applicable
    If FormValues(12, Tabs.Value) = "KSBCR" Then
        rbKSBCR.Value = True
    ElseIf FormValues(12, Tabs.Value) = "KUBCR" Then
        rbKUBCR.Value = True
    ElseIf FormValues(12, Tabs.Value) = "KSECR" Then
        rbKSECR.Value = True
    ElseIf FormValues(12, Tabs.Value) = "KUECR" Then
        rbKUECR.Value = True
    ElseIf FormValues(12, Tabs.Value) = "K4AKA" Then
        rbK4AKA.Value = True
    ElseIf FormValues(12, Tabs.Value) = "MEMPL" Then
        rbMEMPL.Value = True
    ElseIf FormValues(12, Tabs.Value) = "KNOTE" Then
        rbKNOTE.Value = True
    ElseIf FormValues(12, Tabs.Value) = "Bypass" Then
        rbBypass.Value = True
    End If
    'figure which options should be given for comment codes
    If tbAC.Text = "KCNME" Then 'AKA records
        rbK4AKA.Enabled = True
        rbMEMPL.Enabled = False
    ElseIf tbAC.Text = "KCEMP" Then 'Employer records
        rbMEMPL.Enabled = True
        rbK4AKA.Enabled = False
    Else
        rbMEMPL.Enabled = False
        rbK4AKA.Enabled = False
    End If
    'populate last tab on variable to track what tab had changes last
    LastTabOn = Tabs.Value
End Function

'This function saves the users previously added data if applicable
Function SaveUserPopulation()
    FormValues(11, LastTabOn) = tbReComments.Text
    If FigureCommentCode() <> "" Then FormValues(12, LastTabOn) = FigureCommentCode()
End Function

'this function returns the comment code that has been selected for the given tab
Function FigureCommentCode() As String
    If rbKSBCR.Value Then
         FigureCommentCode = "KSBCR"
    ElseIf rbKUBCR.Value Then
         FigureCommentCode = "KUBCR"
    ElseIf rbKSECR.Value Then
        FigureCommentCode = "KSECR"
    ElseIf rbKUECR.Value Then
        FigureCommentCode = "KUECR"
    ElseIf rbK4AKA.Value Then
        FigureCommentCode = "K4AKA"
    ElseIf rbMEMPL.Value Then
        FigureCommentCode = "MEMPL"
    ElseIf rbKNOTE.Value Then
        FigureCommentCode = "KNOTE"
    ElseIf rbBypass.Value Then
        FigureCommentCode = "Bypass"
    End If
End Function

Private Sub LP22Review_Click()
    MsgBox "The script will now pause for you to review the account.  Please press <Insert> key when you are ready to proceed and the script will resume."
    Me.Hide
    Session.WaitForTerminalKey rcIBMInsertKey, "10:00:00"
    Session.TransmitTerminalKey rcIBMInsertKey
    Me.Show
End Sub

Private Sub Tabs_Change()
    SaveUserPopulation
    PopulateTab
End Sub

Private Sub UserForm_Initialize()
    If vbOK <> MsgBox("This script aids in Credit Bureau Review.  Click OK to continue or Cancel to end the script.", vbOKCancel, "Credit Bureau Review") Then End
    Init
End Sub

'this function initializes the forms controls and the variables that feed those controls
Function Init()
    Dim SSN As String
    Dim ActDate As String
    ReDim RawData(1, 0) 'RawData indicies: 0 = Action Code, 1 = Comment
    ReDim FormValues(13, 0)
    'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
    '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
    '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
    GetTask SSN, ActDate
    PopulateFormInfoArray SSN, ActDate, GetAccNum(SSN)
    PopulateTab
    Tabs.Tabs.Remove (Tabs.Tabs.Count - 1)
End Function

'this function populates the array that contains all the form info
Function PopulateFormInfoArray(SSN As String, ActDate As String, AccNum As String)
    Dim Temp() As String
    Dim counter As Integer
    Dim SubCounter As Integer
    'cycle through all entries in array
    While counter < UBound(RawData, 2)
        If RawData(0, counter) = "KCNME" Then 'AKA records
            Temp = Split(RawData(1, counter), ",")
            While SubCounter <= UBound(Temp)
                'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
                '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
                '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
                FormValues(0, UBound(FormValues, 2)) = RawData(0, counter) 'action code
                FormValues(1, UBound(FormValues, 2)) = SSN
                FormValues(2, UBound(FormValues, 2)) = ActDate
                FormValues(3, UBound(FormValues, 2)) = Temp(SubCounter)
                FormValues(13, UBound(FormValues, 2)) = AccNum
                Tabs.Tabs.Add "Page #" & Tabs.Tabs.Count + 1, "Page #" & Tabs.Tabs.Count + 1 'create new tab
                ReDim Preserve FormValues(13, UBound(FormValues, 2) + 1) 'create another possible entry
                SubCounter = SubCounter + 1
            Wend
            SubCounter = 0
        ElseIf RawData(0, counter) = "KCEMP" Then 'Employer records
            Temp = Split(RawData(1, counter), ",")
            While SubCounter <= UBound(Temp)
                'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
                '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
                '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
                FormValues(0, UBound(FormValues, 2)) = RawData(0, counter) 'action code
                FormValues(1, UBound(FormValues, 2)) = SSN
                FormValues(2, UBound(FormValues, 2)) = ActDate
                FormValues(13, UBound(FormValues, 2)) = AccNum
                FormValues(3, UBound(FormValues, 2)) = Temp(SubCounter)
                Tabs.Tabs.Add "Page #" & Tabs.Tabs.Count + 1, "Page #" & Tabs.Tabs.Count + 1 'create new tab
                ReDim Preserve FormValues(13, UBound(FormValues, 2) + 1) 'create another possible entry
                SubCounter = SubCounter + 1
            Wend
            SubCounter = 0
        ElseIf RawData(0, counter) = "KCINQ" Then 'inquiry record
            Temp = Split(RawData(1, counter), ",")
            While SubCounter < UBound(Temp)
                'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
                '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
                '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
                FormValues(0, UBound(FormValues, 2)) = RawData(0, counter) 'action code
                FormValues(1, UBound(FormValues, 2)) = SSN
                FormValues(2, UBound(FormValues, 2)) = ActDate
                FormValues(13, UBound(FormValues, 2)) = AccNum
                FormValues(3, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(4, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(5, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(6, UBound(FormValues, 2)) = Temp(SubCounter)
                Tabs.Tabs.Add "Page #" & Tabs.Tabs.Count + 1, "Page #" & Tabs.Tabs.Count + 1 'create new tab
                ReDim Preserve FormValues(13, UBound(FormValues, 2) + 1) 'create another possible entry
                SubCounter = SubCounter + 1
            Wend
            SubCounter = 0
        ElseIf RawData(0, counter) = "KCTRL" Then 'tradeline record
            Temp = Split(RawData(1, counter), ",")
            While SubCounter < UBound(Temp)
                'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
                '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
                '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
                FormValues(0, UBound(FormValues, 2)) = RawData(0, counter) 'action code
                FormValues(1, UBound(FormValues, 2)) = SSN
                FormValues(2, UBound(FormValues, 2)) = ActDate
                FormValues(13, UBound(FormValues, 2)) = AccNum
                FormValues(3, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(4, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(5, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(6, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(7, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(8, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(9, UBound(FormValues, 2)) = Temp(SubCounter)
                SubCounter = SubCounter + 1
                FormValues(10, UBound(FormValues, 2)) = Temp(SubCounter)
                Tabs.Tabs.Add "Page #" & Tabs.Tabs.Count + 1, "Page #" & Tabs.Tabs.Count + 1 'create new tab
                ReDim Preserve FormValues(13, UBound(FormValues, 2) + 1) 'create another possible entry
                SubCounter = SubCounter + 1
            Wend
            SubCounter = 0
        ElseIf RawData(0, counter) = "KCJDG" Then 'judgement record
            Temp = Split(RawData(1, counter), ",")
            While SubCounter < UBound(Temp)
                'FormValues indicies: 0 = Action Code, 1 = SSN, 2 = Activity Date, 3 = Tradeline#1,
                '4 = Tradeline#2, 5 = Tradeline#3, 6 = Tradeline#4, 7 = Tradeline#5, 8 = Tradeline#6,
                '9 = Tradeline#7, 10 = TradeLine#8, 11 = Comment, 12 = CommentCode, 13 = Account Number
                FormValues(0, UBound(FormValues, 2)) = RawData(0, counter) 'action code
                FormValues(1, UBound(FormValues, 2)) = SSN
                FormValues(2, UBound(FormValues, 2)) = ActDate
                FormValues(13, UBound(FormValues, 2)) = AccNum
                FormValues(3, UBound(FormValues, 2)) = Temp(SubCounter) & Temp(SubCounter + 1)
                SubCounter = SubCounter + 2
                FormValues(4, UBound(FormValues, 2)) = Temp(SubCounter) & Temp(SubCounter + 1)
                SubCounter = SubCounter + 2
                FormValues(5, UBound(FormValues, 2)) = Temp(SubCounter) & Temp(SubCounter + 1)
                SubCounter = SubCounter + 2
                FormValues(6, UBound(FormValues, 2)) = Temp(SubCounter) & Temp(SubCounter + 1)
                SubCounter = SubCounter + 2
                FormValues(7, UBound(FormValues, 2)) = Temp(SubCounter) & Temp(SubCounter + 1)
                Tabs.Tabs.Add "Page #" & Tabs.Tabs.Count + 1, "Page #" & Tabs.Tabs.Count + 1 'create new tab
                ReDim Preserve FormValues(13, UBound(FormValues, 2) + 1) 'create another possible entry
                SubCounter = SubCounter + 2
            Wend
            SubCounter = 0
        End If
        counter = counter + 1
    Wend
End Function

Private Sub UserForm_Terminate()
    End
End Sub

'gets LP9A info
Function GetTask(SSN As String, ActDate As String)
    'access the FCREDITB queue on LP9A
    FastPath "LP9ACFCREDITB"
    If Check4Text(3, 18, "FCREDITB") = False Then
        MsgBox "You have an unresolved queue task in another queue.  Please resolve that task and then restart the script."
        End
    End If
    'get SSN
'<1> SSN = GetText(9, 61, 9)
    SSN = GetText(5, 70, 9)                             '<1>
'    SSN = InputBox("SSN")  'for testing
'    If SSN = "" Then End   'for testing
    'Figure what type of record it is
'<1>If SSN <> GetText(5, 70, 9) Then
    If SSN <> GetText(9, 61, 9) Then                    '<1>
        rbKSECR.Enabled = True
        rbKUECR.Enabled = True
    Else
        rbKSBCR.Enabled = True
        rbKUBCR.Enabled = True
    End If
    GatherComments SSN, ActDate
End Function

'gathers the applicable comments form LP50
Function GatherComments(SSN As String, ActDate As String)
    Dim i As Integer
    FastPath "LP50I" & SSN & ";;;;;KC*"
    If Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then
    'if multiple records are found then
        ActDate = GetText(6, 17, 8)
        FastPath "LP50I" & SSN
        PutText 9, 20, "KC*"
        PutText 18, 29, ActDate & ActDate, "Enter" 'enter date range
        If Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then PutText 3, 13, "X", "Enter" 'select all records
    End If
    'gather all information
    While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        RawData(0, UBound(RawData, 2)) = GetText(8, 2, 5)
        RawData(1, UBound(RawData, 2)) = GetText(13, 2, 75)
        RawData(1, UBound(RawData, 2)) = RawData(1, UBound(RawData, 2)) & GetText(14, 2, 75)
        RawData(1, UBound(RawData, 2)) = RawData(1, UBound(RawData, 2)) & GetText(15, 2, 75)
        RawData(1, UBound(RawData, 2)) = RawData(1, UBound(RawData, 2)) & GetText(16, 2, 75)
        RawData(1, UBound(RawData, 2)) = RawData(1, UBound(RawData, 2)) & GetText(17, 2, 75)
        RawData(1, UBound(RawData, 2)) = RawData(1, UBound(RawData, 2)) & GetText(18, 2, 75)
        RawData(1, UBound(RawData, 2)) = RawData(1, UBound(RawData, 2)) & GetText(19, 2, 75)
        RawData(1, UBound(RawData, 2)) = RawData(1, UBound(RawData, 2)) & GetText(20, 2, 75)
        RawData(1, UBound(RawData, 2)) = Mid(RawData(1, UBound(RawData, 2)), 1, (InStr(1, RawData(1, UBound(RawData, 2)), "(") - 2))
        ReDim Preserve RawData(1, UBound(RawData, 2) + 1)
        Hit "F8"
    Wend
End Function

'this function gathers the Account Number from LP22
Function GetAccNum(SSN As String) As String
    FastPath "LP22I" & SSN
    GetAccNum = Replace(GetText(3, 60, 12), " ", "")
End Function

'new, sr777, aa, 09/16/04, 09/23/04
'<1>, sr820, aa, 10/18/04, 12/15/04
