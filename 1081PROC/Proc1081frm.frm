VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} Proc1081frm 
   Caption         =   "1081 Processing"
   ClientHeight    =   3225
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4710
   OleObjectBlob   =   "Proc1081frm.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "Proc1081frm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Public ClmNum As String
Public CLUID As String
Private cnt As Integer
Private Sub cmdAddmore_Click()
    cnt = cnt + 1
    If ClmNum = "" Then 'on first run dont add coma
        ClmNum = txtclm.Text
        CLUID = txtCLUID.Text
    Else
        ClmNum = ClmNum & "," & txtclm.Text
        CLUID = CLUID & "," & txtCLUID.Text
    End If
    txtclm.Text = ""
    txtCLUID.Text = ""
If cnt >= 18 Then
    MsgBox "Claim Limit met. You must re-run the script to add any more Claim IDs."
    Me.Hide
End If
txtclm.SetFocus
End Sub

Private Sub cmdFinish_Click()
   If ClmNum = "" Then 'on first run dont add coma
        ClmNum = txtclm.Text
        CLUID = txtCLUID.Text
    Else
        ClmNum = ClmNum & "," & txtclm.Text
        CLUID = CLUID & "," & txtCLUID.Text
    End If
    txtclm.Text = ""
    txtCLUID.Text = ""
    Me.Hide
End Sub

Private Sub UserForm_Click()

End Sub
