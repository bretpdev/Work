Public Class frmPasswordResetInfo
    Inherits SP.frmWipeOut

    Public Overloads Shared Sub WipeOut(ByVal Message As String, ByVal Title As String, ByVal TextToAddToTextBox As String, Optional ByVal Center As Boolean = False)
        Dim OneOfMe As New SP.frmWipeOut
        Dim TB As TextBox
        OneOfMe.lblMessage.Text = Message
        OneOfMe.Text = "Wipe Out: " & Title
        If Center Then
            OneOfMe.lblMessage.TextAlign = ContentAlignment.MiddleCenter
        Else
            OneOfMe.lblMessage.TextAlign = ContentAlignment.MiddleLeft
        End If
        'make label smaller
        OneOfMe.lblMessage.Height = 96
        'create text box and add it to form
        TB = New TextBox
        TB.ReadOnly = True
        TB.Text = TextToAddToTextBox
        'text box has same left and is 5 pixels below the label
        TB.Location = New Point(OneOfMe.lblMessage.Left, OneOfMe.lblMessage.Top + OneOfMe.lblMessage.Height + 5)
        'text box is 20 pixels high and the same width as the label
        TB.Height = 20
        TB.Width = OneOfMe.lblMessage.Width
        OneOfMe.Controls.Add(TB)
        Beep()
        OneOfMe.ShowDialog()
    End Sub
End Class
