Imports RegentsScholarshipBackEnd

Public Class SchoolCommunicationsHeader

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnGetSchoolCommunications_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetSchoolCommunications.Click
        If cmbSchoolName.Text = "" And cmbCeebCode.Text = "" Then
            MessageBox.Show("Please choose a school to view the comments", "No school selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            CType(ParentForm, frmBaseCommunications).EntityID = cmbCeebCode.Text
            CType(ParentForm, frmBaseCommunications).EntityName = cmbSchoolName.Text
            CType(ParentForm, frmBaseCommunications).LoadComments()
        End If
    End Sub

    Public Sub LoadComboBoxDataSources(ByVal forHighSchool As Boolean)
        If forHighSchool Then
            SchoolOption.PopulateSchoolDropDown(cmbSchoolName, , , SchoolOption.SchoolOptions.HighSchoolOnly)
            SchoolOption.PopulateSchoolDropDown(cmbCeebCode, , True, SchoolOption.SchoolOptions.HighSchoolOnly)
        Else
            SchoolOption.PopulateSchoolDropDown(cmbSchoolName, , , SchoolOption.SchoolOptions.JrHighSchoolOnly)
            SchoolOption.PopulateSchoolDropDown(cmbCeebCode, , True, SchoolOption.SchoolOptions.JrHighSchoolOnly)
        End If
        AddHandler cmbSchoolName.SelectedIndexChanged, AddressOf cmbSchoolNameSelectedIndexChanged
        AddHandler cmbCeebCode.SelectedIndexChanged, AddressOf cmbCeebCodeSelectedIndexChanged
    End Sub

    Private Sub cmbSchoolNameSelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RemoveHandler cmbCeebCode.SelectedIndexChanged, AddressOf cmbCeebCodeSelectedIndexChanged
        cmbCeebCode.SelectedValue = cmbSchoolName.SelectedValue
        AddHandler cmbCeebCode.SelectedIndexChanged, AddressOf cmbCeebCodeSelectedIndexChanged
    End Sub

    Private Sub cmbCeebCodeSelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RemoveHandler cmbSchoolName.SelectedIndexChanged, AddressOf cmbSchoolNameSelectedIndexChanged
        cmbSchoolName.SelectedValue = cmbCeebCode.SelectedValue
        AddHandler cmbSchoolName.SelectedIndexChanged, AddressOf cmbSchoolNameSelectedIndexChanged
    End Sub


End Class
