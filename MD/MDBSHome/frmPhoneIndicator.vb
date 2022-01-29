Imports MDBSHome.BorrowerContactPhone
Imports Q
Imports SP

Public Class frmPhoneIndicator
    Inherits FormBase

    Private borContactPhone As BorrowerContactPhone
    Private _home As String
    Private _other As String
    Private _other2 As String

    Public Sub New(ByVal tbor As BorrowerContactPhone, ByVal home As String, ByVal other As String, ByVal other2 As String)
        InitializeComponent()
        borContactPhone = tbor
        _home = home
        _other = other
        _other2 = other2
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If chkHome.Checked Then
            borContactPhone.ContactPhoneNumber = _home
            Me.Close()
        ElseIf chkOther.Checked Then
            borContactPhone.ContactPhoneNumber = _other
            Me.Close()
        ElseIf chkOther2.Checked Then
            borContactPhone.ContactPhoneNumber = _other2
            Me.Close()
        ElseIf chkManual.Checked Then
            borContactPhone.ContactPhoneNumber = tbxManual.Text
            Me.Close()
        End If
    End Sub

    Private Sub frmPhoneIndicator_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If _home = "--" Then
            tbxHome.Text = ""
        ElseIf Not _home = "" Then
            tbxHome.Text = _home
            chkHome.Enabled = True
        End If
        If _other = "--" Then
            tbxOther.Text = ""
        ElseIf Not _other = "" Then
            tbxOther.Text = _other
            chkOther.Enabled = True
        End If
        If _other2 = "--" Then
            tbxOther2.Text = ""
        ElseIf Not _other2 = "" Then
            tbxOther2.Text = _other2
            chkOther2.Enabled = True
        End If
    End Sub

    Private Sub chkHome_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkHome.CheckedChanged
        If chkHome.Checked Then
            chkOther.Checked = False
            chkOther2.Checked = False
            chkManual.Checked = False
            btnOK.Enabled = True
        End If
    End Sub

    Private Sub chkOther_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOther.CheckedChanged
        If chkOther.Checked Then
            chkHome.Checked = False
            chkOther2.Checked = False
            chkManual.Checked = False
            btnOK.Enabled = True
        End If
    End Sub

    Private Sub chkOther2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOther2.CheckedChanged
        If chkOther2.Checked Then
            chkHome.Checked = False
            chkOther.Checked = False
            chkManual.Checked = False
            btnOK.Enabled = True
        End If
    End Sub

    Private Sub chkManual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkManual.CheckedChanged
        If chkManual.Checked Then
            chkHome.Checked = False
            chkOther.Checked = False
            chkOther2.Checked = False
            btnOK.Enabled = True
        End If
    End Sub
End Class