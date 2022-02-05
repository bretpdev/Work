VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmCommentMenu 
   Caption         =   "Comment Menu"
   ClientHeight    =   6240
   ClientLeft      =   45
   ClientTop       =   345
   ClientWidth     =   7080
   OleObjectBlob   =   "frmCommentMenu.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmCommentMenu"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Option Explicit
Private UserUnit() As String

Private Sub btn1Cancel_Click()
    End
End Sub

Private Sub btn1OK_Click()
    'check that SSN/Account number is populated
    If tbSSN.TextLength = 9 Then
        FastPath "LP22I" & tbSSN.Text
    Else
        FastPath "LP22I;;;;;;" & tbSSN.Text
    End If
    'check if SSN is valid and on the system
    If Check4Text(1, 62, "PERSON DEMOGRAPHICS") = False Then
        MsgBox "The SSN or account number that you entered doesn't appear to be in OneLINK.  Please try again.", vbInformation
        tbSSN.SetFocus
        tbSSN.SelStart = 0
        tbSSN.SelLength = tbSSN.TextLength
    Else
        tbSSN.Text = GetText(3, 23, 9)
        lblBorrName.Caption = GetText(4, 44, 13) & " " & GetText(4, 60, 1) & " " & GetText(4, 5, 35)
        EnableOrDisable gbSSN
        EnableOrDisable gbVerify, , True
        btn2OK.SetFocus
    End If
End Sub

Private Sub btn2Cancel_Click()
    EnableOrDisable gbVerify, True, False
    EnableOrDisable gbSSN, True, True
    tbSSN.SetFocus
End Sub

Private Sub btn2OK_Click()
    EnableOrDisable gbVerify, , False
    EnableOrDisable gbCommentOptions, , True
    lbCommentOps.SetFocus
End Sub

Private Sub btn3Cancel_Click()
    EnableOrDisable gbCommentOptions, True, False
    EnableOrDisable gbVerify, True, False
    EnableOrDisable gbSSN, True, True
    tbSSN.SetFocus
End Sub

Private Sub btn3OK_Click()
    Dim TempDateVar As String
    'do data checks
    If IsNull(lbCommentOps.Value) Then
        MsgBox "You must select one of the comment options.  Please try again.", vbCritical
        Exit Sub
    ElseIf lbCommentOps.Value = "Archived File To Imaging" Then
        If tbArchiveNum.TextLength = 0 Then
            MsgBox "When choosing the ""Archived File To Imaging"" option you must provide an Archive Number.  Please try again.", vbCritical
            Exit Sub
        End If
    ElseIf lbCommentOps.Value = "Expired Application To Be Shredded" Or lbCommentOps.Value = "Expired Application Could Not Be Located To Be Shredded" Then
        TempDateVar = tbSignDate.Text
        If tbSignDate.TextLength <> 8 Then
            MsgBox "When choosing either of the ""Expired Application"" options you must provide a sign date.  Please try again.", vbCritical
            Exit Sub
        ElseIf SP.Common.DateFormat(TempDateVar) = False Then
            MsgBox "Sign date provided is not a valid date.  Please try again.", vbCritical
            Exit Sub
        End If
    End If
    'do the comment thing
    If lbCommentOps.Value = "Mandatory Assignment" Then
        SP.Common.AddLP50 tbSSN.Text, "RD2ED", "DOCSVCSCOM", "AM", "10", "BORROWER FILE MAILED TO ED DUE TO MANDATORY ASSIGNMENT."
    ElseIf lbCommentOps.Value = "Conditional Disability" Then
        SP.Common.AddLP50 tbSSN.Text, "RD2ED", "DOCSVCSCOM", "AM", "10", "BORROWER FILE MAILED TO ED DUE TO TOTAL AND PERMANENT DISABILITY."
    ElseIf lbCommentOps.Value = "Archived File To Imaging" Then
        SP.Common.AddLP50 tbSSN.Text, "DRSRF", "DOCSVCSCOM", "MS", "10", "THE FILE WAS PULLED FROM ARCHIVES, BOX #" & tbArchiveNum.Text & ", SCANNED TO ARCHV AND THEN SHREDDED."
    ElseIf lbCommentOps.Value = "Expired Application To Be Shredded" Then
        SP.Common.AddLP50 tbSSN.Text, "MPNXS", "DOCSVCSCOM", "MS", "10", "THE EXPIRED APPLICATION WITH BORROWER SIGN DATE " & TempDateVar & " HAS BEEN SHREDDED."
    ElseIf lbCommentOps.Value = "Expired Application Could Not Be Located To Be Shredded" Then
        SP.Common.AddLP50 tbSSN.Text, "MPNXS", "DOCSVCSCOM", "MS", "10", "THE EXPIRED APPLICATION WITH BORROWER SIGN DATE " & TempDateVar & " COULD NOT BE LOCATED TO BE SHREDDED."
    End If
    If MsgBox("Would you like to process another?", vbYesNo + vbQuestion) <> vbYes Then
        'end the script
        End
    Else
        'reset form
        EnableOrDisable gbCommentOptions, True, False
        EnableOrDisable gbVerify, True, False
        EnableOrDisable gbSSN, True, True
        tbSSN.SetFocus
    End If
End Sub



Private Sub lbCommentOps_Change()
    If UserUnit(1) = "Document Services" Then
        If lbCommentOps.Value = "Archived File To Imaging" Then
            tbSignDate.Enabled = False
            lblSignDate.Enabled = False
            lblArchiveNum.Enabled = True
            tbArchiveNum.Enabled = True
            tbSignDate.Text = ""
        ElseIf lbCommentOps.Value = "Expired Application To Be Shredded" Or lbCommentOps.Value = "Expired Application Could Not Be Located To Be Shredded" Then
            tbSignDate.Enabled = True
            lblSignDate.Enabled = True
            lblArchiveNum.Enabled = False
            tbArchiveNum.Enabled = False
            tbSignDate.Text = ""
        Else
            tbSignDate.Enabled = False
            lblSignDate.Enabled = False
            lblArchiveNum.Enabled = False
            tbArchiveNum.Enabled = False
            tbSignDate.Text = ""
        End If
    End If
End Sub

Private Sub lblBorrName_Click()

End Sub

Private Sub tbSSN_Change()

End Sub

'Get data from DB
Private Sub UserForm_Initialize()
    'get the user's unit
    UserUnit = SP.Common.SQL("SELECT BusinessUnit FROM GENR_REF_BU_Agent_Xref WHERE Role = 'Member Of' AND WindowsUserID = '" & Environ("USERNAME") & "'")
    
    'add different options to list
    If UserUnit(1) = "Auxiliary Services" Then
        lbCommentOps.AddItem "Mandatory Assignment"
        lbCommentOps.AddItem "Conditional Disability"
    ElseIf UserUnit(1) = "Document Services" Then
        lbCommentOps.AddItem "Archived File To Imaging"
        lbCommentOps.AddItem "Expired Application To Be Shredded"
        lbCommentOps.AddItem "Expired Application Could Not Be Located To Be Shredded"
    End If
    'disable lower parts of the form
    EnableOrDisable gbVerify
    EnableOrDisable gbCommentOptions
End Sub

Private Sub EnableOrDisable(GB As Variant, Optional ClearFields As Boolean = False, Optional Enable As Boolean = False)
    Dim i As Integer
    While i < GB.Controls.count
        If Enable Then
            'if enabling then skip over archive and sign date text boxes and labels
            If GB.Controls(i).Name <> "tbArchiveNum" And GB.Controls(i).Name <> "tbSignDate" And _
               GB.Controls(i).Name <> "lblArchiveNum" And GB.Controls(i).Name <> "lblSignDate" Then
                GB.Controls(i).Enabled = Enable
            End If
        Else
            'if disabling then do them all
            GB.Controls(i).Enabled = Enable
        End If
        If ClearFields Then
            If TypeName(GB.Controls(i)) = "TextBox" Then
                'skip archive box
                If GB.Controls(i).Name <> "tbArchiveNum" Then
                    GB.Controls(i).Text = ""
                End If
            ElseIf TypeName(GB.Controls(i)) = "Label" And GB.Controls(i).Name = "lblBorrName" Then
                GB.Controls(i).Caption = ""
            ElseIf TypeName(GB.Controls(i)) = "ListBox" Then
                GB.Controls(i).Value = Null
            End If
        End If
        i = i + 1
    Wend
End Sub


