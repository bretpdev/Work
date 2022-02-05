Class ManualComments
    Inherits FormBase

    ReadOnly _maxCharactersAllowed As Integer

    Public Sub New(ByVal comment As UserProvidedComment, ByVal maxCharactersAllowed As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        UserProvidedCommentBindingSource.DataSource = comment
        _maxCharactersAllowed = maxCharactersAllowed
        txtComment.MaxLength = _maxCharactersAllowed
        SetCounts()
    End Sub

    Private Sub tbxComment_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtComment.TextChanged
        SetCounts()
    End Sub

    Private Sub SetCounts()
        lblRemainingChar.Text = (_maxCharactersAllowed - txtComment.TextLength).ToString()
    End Sub
End Class