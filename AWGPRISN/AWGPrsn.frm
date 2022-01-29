VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} AWGPrsn 
   Caption         =   "AWG Prison"
   ClientHeight    =   4515
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4710
   OleObjectBlob   =   "AWGPrsn.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "AWGPrsn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub CommandButton1_Click()
    Okay = 1
    Me.Hide
End Sub
Private Sub CommandButton2_Click()
    Me.Hide
    End
End Sub

Private Sub OptionButton1_Click()
ResultInd = 1
End Sub
Private Sub OptionButton2_Click()
ResultInd = 2
End Sub
Private Sub OptionButton3_Click()
ResultInd = 3
End Sub
Private Sub OptionButton4_Click()
ResultInd = 4
End Sub
