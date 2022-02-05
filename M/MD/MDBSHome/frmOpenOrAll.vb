Public Class frmOpenOrAll
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
    End Sub

    Public Overloads Sub Show(ByRef openOnly As Integer)
        Me.ShowDialog()
        If rbOpen.Checked Then
            openOnly = 1
        ElseIf rbAll.Checked Then
            openOnly = 2
        Else
            openOnly = 3
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        rbOpen.Checked = False
        rbAll.Checked = False
        Me.Hide()
    End Sub
End Class
