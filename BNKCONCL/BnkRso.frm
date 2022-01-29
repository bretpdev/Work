VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} BnkRso 
   Caption         =   "Bankruptcy Resolution"
   ClientHeight    =   3765
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   3420
   OleObjectBlob   =   "BnkRso.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "BnkRso"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False



Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnOk_Click()
    If radDischarged.Value = False And radDismissed.Value = False And radHardship.Value = False And radPartial.Value = False Then
        MsgBox "You must select a status!"
    Else
        Me.Hide
    End If
End Sub
