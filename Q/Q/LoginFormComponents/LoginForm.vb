Public Class LoginForm

#Region "Understanding of this is not needed to use this form (just call the shared ShowDialog function)"

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Public Function CallsHiddenShowDialog() As DialogResult
        Return MyBase.ShowDialog()
    End Function

    ''' <summary>
    ''' Creates form, shows it, returns a null object if cancel was hit otherwise it returns a populated LoginInfo object.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Shadows Sub ShowDialog()
        Dim instanceOfMe As New LoginForm()
    End Sub

    ''' <summary>
    ''' Creates form, shows it, returns a null object if cancel was hit otherwise it returns a populated LoginInfo object.
    ''' </summary>
    ''' <param name="promptText">Text to put on form as instructions or other need notifiying text.</param>
    ''' <remarks></remarks>
    Public Shared Shadows Sub ShowDialog(ByVal promptText As String)
        Dim instanceOfMe As New LoginForm()
        instanceOfMe.txtText.Visible = True
        instanceOfMe.txtText.Text = promptText
        If instanceOfMe.txtText.LineCount() > 2 Then
            instanceOfMe.txtText.ScrollBars = ScrollBars.Vertical
        End If
    End Sub

#End Region

End Class
