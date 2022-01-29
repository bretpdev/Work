VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmLCOSchLS 
   Caption         =   "Approved Schools set up TX13 and TX10"
   ClientHeight    =   2520
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   4365
   OleObjectBlob   =   "frmLCOSchLS.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmLCOSchLS"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnOK_Click()
    If Validate = False Then Exit Sub
    Me.Hide
End Sub

Function Validate() As Boolean
    If IsDate(txtEffDate.Text) = False Then
        MsgBox "Invalid Date!"
        Validate = False
        Exit Function
    End If
    txtEffDate.Text = Format(CDate(txtEffDate.Text), "mmddyy")
    Validate = True
End Function
