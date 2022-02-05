VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmLVC2 
   Caption         =   "LVC"
   ClientHeight    =   5655
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4800
   OleObjectBlob   =   "frmLVC2.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmLVC2"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnOK_Click()
    If (rad2.Value = False And rad4.Value = False And rad1.Value = False And rad3.Value = False) Then
        MsgBox "Invalid senario."
        Exit Sub
    End If
    Me.Hide
End Sub

Private Sub btnReset_Click()
    rad1.Value = False
    rad2.Value = False
    rad3.Value = False
    rad4.Value = False
End Sub

Private Sub btnReview_Click()
    MsgBox "Press 'insert' when done."
    Me.Hide
    SP.q.PauseForInsert
    Me.Show
End Sub

