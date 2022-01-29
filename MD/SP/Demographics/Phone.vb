Public Class Phone

    Private Sub txtPhone1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPhone1.TextChanged
        If txtPhone1.TextLength = 3 Then
            txtPhone2.Focus()
        End If
    End Sub

    Private Sub txtPhone2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPhone2.TextChanged
        If txtPhone2.TextLength = 3 Then
            txtPhone3.Focus()
        End If
    End Sub

    Private Sub txtPhone3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPhone3.TextChanged
        If txtPhone3.TextLength = 4 Then
            txtExt.Focus()
        End If
    End Sub

    Public Function ValidUserInput(ByVal bor As Borrower) As Boolean
        'if any of the phone fields have data then validate the data
        If (txtPhone1.TextLength <> 0 Or txtPhone2.TextLength <> 0 Or txtPhone3.TextLength <> 0) Then
            'be sure that the phone number is only numeric, has ten digits, and doesn't have a 0 or a 1 in the 4th digit place
            If txtPhone1.TextLength <> 3 Or txtPhone2.TextLength <> 3 Or txtPhone3.TextLength <> 4 Then
                'give the user an error message
                SP.frmWhoaDUDE.WhoaDUDE("The phone number must be numeric, have 10 digits, and not have a '1' or '0' in the 1st or 4th position.", "Invalid Phone Number", True)
                'enable phone number boxes and buttons
                EnableControls()
                txtPhone1.Focus()
                Return False
            Else
                'check if 
                If IsNumeric(txtPhone1.Text) = False Or IsNumeric(txtPhone2.Text) = False Or IsNumeric(txtPhone3.Text) = False Or (txtExt.Text <> "" And IsNumeric(txtExt.Text) = False) Or
                txtPhone2.Text.Substring(0, 1) = "0" Or txtPhone2.Text.Substring(0, 1) = "1" Or txtPhone1.Text.Substring(0, 1) = "0" Or txtPhone1.Text.Substring(0, 1) = "1" Then
                    SP.frmWhoaDUDE.WhoaDUDE("The phone number must be numeric, have 10 digits, and not have a '1' or '0' in the 1st or 4th position.", "Invalid Phone Number", True)
                    'enable phone number boxes and buttons
                    EnableControls()
                    txtPhone1.Focus()
                    Return False
                End If
            End If
            If (txtPhone2.Text + txtPhone3.Text).Distinct().Count() = 1 Then
                SP.frmWhoaDUDE.WhoaDUDE("The last 7 digits of the phone number cannot all be the same number.", "Invalid Phone Number", True)
                'enable phone number boxes and buttons
                EnableControls()
                txtPhone1.Focus()
                Return False
            End If
        End If
            Return True
    End Function

    'enables controls on control
    Private Sub EnableControls()
        txtPhone1.Enabled = True
        txtPhone2.Enabled = True
        txtPhone3.Enabled = True
        txtExt.Enabled = True
        cbxPhnMBL.Enabled = True
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cbxPhnMBL.SelectedItem = "U"
    End Sub
End Class
