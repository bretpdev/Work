VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmNSFRevPostSys 
   Caption         =   "NSF Reversals Posting"
   ClientHeight    =   1620
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4980
   OleObjectBlob   =   "frmNSFRevPostSys.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmNSFRevPostSys"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False



Private Sub btnCompass_Click()
    lbl1.Caption = "Processing reversals for Compass."
    Wait "1"
    Me.Hide
End Sub

Private Sub btnOneLINK_Click()
    lbl1.Caption = "Processing reversals for OneLINK."
    Wait "1"
    Me.Hide
End Sub

Private Sub btnCancel_Click()
    End
End Sub

