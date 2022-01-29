Imports System.Windows.Forms

Public Class InputBox

#Region "Understanding of this is not needed to use this form (just call the shared ShowDialog function)"

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub InputBox_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        txtUserInput.Focus()
    End Sub

    ''' <summary>
    ''' Calls hidden ShowDialog (this was needed so it could be called from the shared showdialog).  This is should only be called from the Shared ShowDialog function.
    ''' </summary>
    ''' <remarks></remarks>
    Public Function CallHiddenShowDialog() As DialogResult
        Return MyBase.ShowDialog()
    End Function

    ''' <summary>
    ''' Creates and shows inputbox form.  Also, returns InputBoxResults.
    ''' </summary>
    ''' <param name="text">Text to be displayed in main body of form.</param>
    ''' <param name="title">Text in title of window.</param>
    ''' <returns>InputBoxResults</returns>
    ''' <remarks></remarks>
    Public Shared Shadows Function ShowDialog(ByVal text As String, ByVal title As String) As InputBoxResults
        Dim aMe As New InputBox()
        aMe.txtText.Text = text
        If aMe.txtText.LineCount() > 5 Then
            aMe.txtText.ScrollBars = ScrollBars.Vertical
        End If
        aMe.Text = title
        Dim re As New InputBoxResults
        re.DialogRe = aMe.CallHiddenShowDialog()
        re.UserProvidedText = aMe.txtUserInput.Text
        Return re
    End Function

    Public Shared Shadows Function ShowDialog(ByVal text As String, ByVal title As String, ByVal textForTextBox As String) As InputBoxResults
        Dim aMe As New InputBox()
        aMe.txtText.Text = text
        If aMe.txtText.LineCount() > 5 Then
            aMe.txtText.ScrollBars = ScrollBars.Vertical
        End If
        aMe.Text = title
        aMe.txtUserInput.Text = textForTextBox
        Dim re As New InputBoxResults
        re.DialogRe = aMe.CallHiddenShowDialog()
        re.UserProvidedText = aMe.txtUserInput.Text
        Return re
    End Function

#End Region

End Class
