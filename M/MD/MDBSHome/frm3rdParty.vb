Imports System.Collections.Generic

Public Class frm3rdParty
    Public Sub New(ByVal thirdPartyListViewItems As List(Of ThirdPartyListViewItem))
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
        For Each thirdParty As ThirdPartyListViewItem In thirdPartyListViewItems
            lv3rdParty.Items.Add(CType(thirdParty, System.Windows.Forms.ListViewItem))
        Next thirdParty
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub
End Class
