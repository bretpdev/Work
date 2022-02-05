VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} DMVRsp 
   Caption         =   "DMV Response"
   ClientHeight    =   2895
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   3090
   OleObjectBlob   =   "DMVRsp.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "DMVRsp"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Dim STlist As Variant

Private Sub CommandButton1_Click()
    Me.Hide
End Sub

Private Sub CommandButton2_Click()
    Me.Hide
    End
End Sub

Private Sub success_Click()
    ResponseInd = 1
End Sub

Private Sub unsuccess_Click()
    ResponseInd = 2
End Sub

Private Sub UserForm_Initialize()
    STlist = Array("AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY")
    State.list() = STlist
End Sub

