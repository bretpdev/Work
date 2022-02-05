VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} MssAssgnOptions 
   Caption         =   "Mass Assign Options"
   ClientHeight    =   3150
   ClientLeft      =   45
   ClientTop       =   345
   ClientWidth     =   2535
   OleObjectBlob   =   "MssAssgnOptions.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "MssAssgnOptions"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Option Explicit

Public Enum MassAssignUserOpt
    MassAssign = 0
    MassAssignBySSN = 1
    MassAssignReassign = 2
End Enum

Dim Opt As MassAssignUserOpt

Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnOk_Click()
    If MA.Value Then
        Opt = MassAssign
    ElseIf MASSN.Value Then
        Opt = MassAssignBySSN
    ElseIf MARe.Value Then
        Opt = MassAssignReassign
    Else
        MsgBox "You must select one of the options.", vbOKOnly
        Exit Sub
    End If
    Me.Hide
End Sub


Public Function UserOption() As MassAssignUserOpt
    UserOption = Opt
End Function


