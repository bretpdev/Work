VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} MainMenu 
   Caption         =   "Claims Letters"
   ClientHeight    =   5880
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   3990
   OleObjectBlob   =   "MainMenu.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "MainMenu"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Private Sub btnCancel_Click()
    Me.Hide
    End
End Sub
Private Sub radClaimPaid_Click()
    Me.Hide
    ClaimPaidProc
End Sub

Private Sub radClaimReturn_Click()
    Me.Hide
    frm10Borrower.setLetter ("RETTRANS")
    frm10Borrower.Show
End Sub

Private Sub radTeacherForgiveness_Click()
    Me.Hide
    frm10Borrower.setLetter ("TLFTRANS")
    frm10Borrower.Show
End Sub

Private Sub radTeacherForgivenessDenial_Click()
    Dim SSN As String
    Dim LastFourSSN As String
    Dim FName As String
    Dim LName As String
    Dim x As Integer
    Dim x2 As Integer
    Dim UIDs(30) As String
    Dim Holder As String
    Dim cnt As Integer
    
    Me.Hide
    Holder = ""
    SSN = InputBox("SSN:", "Teacher Loan Forgiveness Denial to Nelnet")
    LastFourSSN = "XXX-XX-" & Right(SSN, 4)
    
    SP.FastPath "LG10I" & SSN
    If SP.q.Check4Text(1, 53, "LOAN BWR STATUS RECAP SELECT") Then
        'selection screen
        cnt = 1
        Do While SP.q.Check4Text(20, 3, "46004 NO MORE DATA TO DISPLAY") = False
            For x = 7 To 18
                If SP.q.GetText(x, 5, 2) <> "" Then
                    SP.PutText 19, 15, Format(SP.q.GetText(x, 5, 2), "00"), "ENTER"
                    Do While SP.q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                        For x2 = 11 To 20
                            If SP.q.GetText(x2, 4, 19) <> "" Then
                                UIDs(cnt) = (SP.q.GetText(x2, 4, 19))
                                cnt = cnt + 1
                                If cnt > 12 Then
                                    MsgBox "This borrower has more than 12 loans this letter will not work for this borrower. Ending Script."
                                    End
                                End If
                                If Holder = "" Then
                                    Holder = SP.q.GetText(5, 18, 6)
                                Else
                                    If InStr(1, Holder, SP.q.GetText(5, 18, 6)) < 0 Then
                                        Holder = Holder & "," & SP.q.GetText(5, 18, 6)
                                    End If
                                End If
                            End If
                        Next x2
                        SP.q.Hit "F8"
                    Loop
                    SP.q.Hit "F12"
                End If
            Next x
            SP.q.Hit "F8"
        Loop
    Else
        'target screen
        cnt = 1
        Do While SP.q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            For x2 = 11 To 20
                If SP.q.GetText(x2, 4, 19) <> "" Then
                    UIDs(cnt) = (SP.q.GetText(x2, 4, 19))
                    cnt = cnt + 1
                    If cnt > 30 Then
                        MsgBox "This borrower has more than 30 loans this letter will not work for this borrower. Ending Script."
                    End If
                    Holder = SP.q.GetText(5, 18, 6)
                End If
            Next x2
            SP.q.Hit "F8"
        Loop
    End If
    
    SP.q.FastPath "LP22I" & SSN
    FName = SP.q.GetText(4, 44, 12)
    LName = SP.q.GetText(4, 5, 35)
    
    frmTeacherLoanForgivenessDenial.Show
    Dim DocPath As String
    DocPath = "X:\PADD\Claims\"
    SP.Common.TestMode , DocPath
    Open "C:\Windows\Temp\ltr.dat" For Output As #1
        Write #1, "LenderNo", "FirstName", "LastName", "SSN", "ClaimDate", "Reason1", "Reason2", "Reason3", "CLUID1", "CLUID2", "CLUID3", "CLUID4", "CLUID5", "CLUID6", "CLUID7", "CLUID8", "CLUID9", "CLUID10", "CLUID11", "CLUID12", "CLUID13", "CLUID14", "CLUID15", "CLUID16", "CLUID17", "CLUID18", "CLUID19", "CLUID20", "CLUID21", "CLUID22", "CLUID23", "CLUID24", "CLUID25", "CLUID26", "CLUID27", "CLUID28", "CLUID29", "CLUID30"
        Write #1, Holder, FName, LName, LastFourSSN, frmTeacherLoanForgivenessDenial.txtClaimDate.Text, frmTeacherLoanForgivenessDenial.txtDenialReason1.Text, frmTeacherLoanForgivenessDenial.txtDenialReason2.Text, frmTeacherLoanForgivenessDenial.txtDenialReason3.Text, UIDs(1), UIDs(2), UIDs(3), UIDs(4), UIDs(5), UIDs(6), UIDs(7), UIDs(8), UIDs(9), UIDs(10), UIDs(11), UIDs(12), UIDs(13), UIDs(14), UIDs(15), UIDs(16), UIDs(17), UIDs(18), UIDs(19), UIDs(20), UIDs(21), UIDs(22), UIDs(23), UIDs(24), UIDs(25), UIDs(26), UIDs(27), UIDs(28), UIDs(29), UIDs(30)
    Close #1
    Me.Hide
    SP.Common.PrintDocs DocPath, "TLFDENNEL", "C:\Windows\Temp\ltr.dat", False
End Sub

Private Sub radTeacherForgivenessDoc_Click()
    Me.Hide
    frmTeacherLoanForginenessDoc.Show
End Sub

Private Sub radReturnLetter_Click()
    Me.Hide
    ReturnProc
End Sub
Private Sub radRejectLetter_Click()
    Me.Hide
    RejectProc
End Sub
Private Sub radManualReject_Click()
    Me.Hide
    ManualLettersProc "Manual"
End Sub
Private Sub radSupplementalReject_Click()
    Me.Hide
    ManualLettersProc "SupRej"
End Sub
Private Sub radRecallLetter_Click()
    Me.Hide
    ManualLettersProc "Recall"
End Sub
