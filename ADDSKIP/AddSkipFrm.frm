VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} AddSkipFrm 
   Caption         =   "Add Skip"
   ClientHeight    =   2715
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   3690
   OleObjectBlob   =   "AddSkipFrm.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "AddSkipFrm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Private KSKPREQTask As Boolean                                  '<1>
Dim SvcrList As Variant

Private Sub CommandButton1_Click()
    Me.Hide
End Sub

Private Sub CommandButton2_Click()
    Me.Hide
    Unload Me
    End
End Sub

Private Sub UserForm_Initialize()
    SvcrList = Array("Nelnet", "SallieMae", "UHEAA Account Services", "UHEAA Borrower Services")
    Servicer.list() = SvcrList
'<1->
    If Check4Text(1, 2, "LP9A") And Check4Text(3, 13, "WORK GROUP KSKPREQ") Then
        KSKPREQTask = True
        cSSN.Text = GetText(17, 70, 9)
    Else
        KSKPREQTask = False
    End If
'</1>
End Sub

'<1->
Public Function GetKSKPREQTask() As Boolean
    GetKSKPREQTask = KSKPREQTask
End Function
'</1>
