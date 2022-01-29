Public Class DistrictCommunicationHeader

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub LoadComboBoxDataSources()
        DistrictOption.PopulateDistrictDropDown(cmbDistrictName, True)
    End Sub

    Private Sub btnGetDistrictCommunications_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetDistrictCommunications.Click
        If cmbDistrictName.Text = "" Then
            MessageBox.Show("Please choose a district to view the comments", "No district selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            CType(ParentForm, frmBaseCommunications).EntityID = cmbDistrictName.SelectedValue
            CType(ParentForm, frmBaseCommunications).EntityName = cmbDistrictName.Text
            CType(ParentForm, frmBaseCommunications).LoadComments()
        End If
    End Sub
End Class
