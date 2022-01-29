VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} EVRSchInfo 
   Caption         =   "EVR School Information"
   ClientHeight    =   3360
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6555
   OleObjectBlob   =   "EVRSchInfo.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "EVRSchInfo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Dim STlist As Variant

Private Sub CommandButton1_Click()
    Okay = 1
    Me.Hide
End Sub

Private Sub CommandButton2_Click()
    Me.Hide
End Sub

Private Sub UserForm_Initialize()
    STlist = Array("AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FC", "FL", "GA", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY")
    SchST.list() = STlist
End Sub

