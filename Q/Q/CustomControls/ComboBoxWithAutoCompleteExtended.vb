Public Class ComboBoxWithAutoCompleteExtended
    Inherits ComboBox


    Private _invalidAutoCompleteEntry As InvalidEntryBehavior
    Public Property InvalidAutoCompleteEntry() As InvalidEntryBehavior
        Get
            Return _invalidAutoCompleteEntry
        End Get
        Set(ByVal value As InvalidEntryBehavior)
            _invalidAutoCompleteEntry = value
        End Set
    End Property


    Public Sub New()
        MyBase.New()
        Me.AutoCompleteMode = Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.AutoCompleteSource = Windows.Forms.AutoCompleteSource.ListItems
        Me.DropDownStyle = ComboBoxStyle.DropDown
    End Sub

    Public Enum InvalidEntryBehavior
        ErrorMessage
        SelectEnteredText
    End Enum

    Private Sub ComboBoxWithAutoCompleteExtended_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave
        If (Me.Items.Count <> 0) Then 'only do check if there is something in the combo box.
            If (Me.FindStringExact(Me.Text) = -1) Then
                If (_invalidAutoCompleteEntry = InvalidEntryBehavior.ErrorMessage) Then
                    MessageBox.Show("The entered item isn't in the list of options.  Please try again.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Focus()
                    Me.SelectAll()
                ElseIf (_invalidAutoCompleteEntry = InvalidEntryBehavior.SelectEnteredText) Then
                    Me.Focus()
                    Me.SelectAll()
                End If
            End If
        End If
    End Sub
End Class
