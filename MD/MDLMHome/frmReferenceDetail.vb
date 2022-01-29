Public Class frmReferenceDetail


    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub PopulateForm(ByRef Ref As Reference)
        tbID.Text = Ref.ID
        tbName.Text = Ref.TheName
        tbRelationship.Text = Ref.Relationship
        tbStatus.Text = Ref.Status
        tbThirdPartyAuth.Text = Ref.ThirdPartyAuth
        tbAddr1.Text = Ref.Addr1
        tbAddr2.Text = Ref.Addr2
        tbCity.Text = Ref.City
        tbState.Text = Ref.State
        tbZip.Text = Ref.Zip
        tbAddrVal.Text = Ref.AddrValInd
        tbPhone.Text = Ref.Phone
        tbPhoneVal.Text = Ref.PhoneValInd
        tbOPhone.Text = Ref.OPhone
        tbOPhoneVal.Text = Ref.OPhoneValInd
        tbEmail.Text = Ref.Email
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub
End Class