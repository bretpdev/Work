VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} EnrVer 
   Caption         =   "Enrollment Verification"
   ClientHeight    =   2940
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4620
   OleObjectBlob   =   "EnrVer.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "EnrVer"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Private Sub Cancel_Click()
    StopLoopInd = 1
    Me.Hide
End Sub

Private Sub CommandButton1_Click()
    Okay = 1
    Me.Hide
End Sub
