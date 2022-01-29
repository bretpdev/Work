Public Class frmEmployer
    Public Sub New()
        InitializeComponent()
        SP.FastPath("LC20I")
        If SP.Q.Check4Text(22, 3, "48012") Then
            grpBxEmployeeInfo.Visible = False
            lblNoInfo.Visible = True
        Else
            lblEmpName1Value.Text = Trim(SP.Q.GetText(2, 43, 38))
            lblEmpName2Value.Text = Trim(SP.Q.GetText(3, 43, 38))
            lblAddr1Value.Text = Trim(SP.Q.GetText(4, 43, 38))
            lblAddr2Value.Text = Trim(SP.Q.GetText(5, 43, 38))
            lblCityValue.Text = Trim(SP.Q.GetText(6, 43, 25))
            lblStateValue.Text = Trim(SP.Q.GetText(6, 74, 2))
            lblZipValue.Text = Trim(SP.Q.GetText(7, 43, 9))
            lblPhoneVal.Text = Trim(SP.Q.GetText(7, 60, 10))
            grpBxEmployeeInfo.Visible = True
            lblNoInfo.Visible = False
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class