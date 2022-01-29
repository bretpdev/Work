Public Class frmGetComment
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    Public Overloads Function ShowDialog(ByVal comment As String) As Boolean
        txtcomment.Text = comment
        txtcomment.SelectionLength = 0
        txtcomment.SelectionStart = txtcomment.Text.Length
        Me.ShowDialog()
        Me.Activate()
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SP.Q.PutText(12, 10, txtcomment.Text, True)
        Me.Close()
    End Sub
End Class
