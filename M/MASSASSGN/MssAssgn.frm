VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} MssAssgn 
   Caption         =   "Mass Assign Tasks"
   ClientHeight    =   4605
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   3420
   OleObjectBlob   =   "MssAssgn.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "MssAssgn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False




Dim NoList As Variant

Private Sub CommandButton1_Click()
    Me.Hide
End Sub

Private Sub CommandButton2_Click()
    Me.Hide
    End
End Sub

Private Sub UserForm_Initialize()
    NoList = Array("10", "20", "30", "ALL")
    No.list() = NoList
End Sub

