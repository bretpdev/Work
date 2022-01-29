VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmTeacherLoanForgivenessDenial 
   Caption         =   "Teacher Loan Forgiveness Denial to Nelnet"
   ClientHeight    =   3510
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4710
   OleObjectBlob   =   "frmTeacherLoanForgivenessDenial.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmTeacherLoanForgivenessDenial"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False



Private Sub btnOK_Click()
    If SP.Common.DateFormat(txtClaimDate.Text) = False Then
        MsgBox "Date must be in the format MM/DD/YYYY Try again.", vbCritical, "Invalid Date!"
    Else
        Me.Hide
    End If
End Sub
