VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmSWOLNCR 
   Caption         =   "Special Process Loan Sale Create"
   ClientHeight    =   3540
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   3780
   OleObjectBlob   =   "frmSWOLNCR.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmSWOLNCR"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnOK_Click()
        If rdoRedeem.Value = True Then
            If IsDate(txtDate.Text) = False Then
                warn = MsgBox("The date entered is not valid.  Click OK to reenter the information.", vbOKOnly, "Invalid Date")
                Exit Sub
            End If
        End If
    Me.Hide
End Sub

Private Sub rdoECASLA_Click()
    If rdoRedeem.Value = False Then
        txtDate.Text = ""
        txtDate.Enabled = False
        lbDate.Enabled = False
    End If
End Sub

Private Sub rdoRedeem_Click()
    If rdoRedeem.Value = True Then
        txtDate.Enabled = True
        lbDate.Enabled = True
    Else
        txtDate.Enabled = False
        lbDate.Enabled = False
    End If
End Sub

Private Sub rdoSpecial_Click()
    If rdoRedeem.Value = False Then
        txtDate.Text = ""
        lbDate.Enabled = False
        txtDate.Enabled = False
    End If
End Sub

