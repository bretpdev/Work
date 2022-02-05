Imports System.Windows.Forms

Public Class ComboBoxExtended
    Inherits ComboBox
    Implements IIsDirtyExtender

    ' List of extended functionality
    ' - Is Dirty property that tracks if user changed the value of the text box.

    ''' <summary>
    ''' Is true if the user changed the value of the combo box at any point.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsDirty() As Boolean Implements IIsDirtyExtender.IsDirty
        Get
            If _originalValue = -1 Then
                Return False
            ElseIf _originalValue = Me.SelectedIndex Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Private _previouslyEntered As Boolean = False
    Protected Property PreviouslyEntered() As Boolean
        Get
            Return _previouslyEntered
        End Get
        Set(ByVal value As Boolean)
            _previouslyEntered = value
        End Set
    End Property

    Private _originalValue As Integer = -1
    Protected Property OriginalValue() As String
        Get
            Return _originalValue
        End Get
        Set(ByVal value As String)
            _originalValue = value
        End Set
    End Property

    Private Sub ComboBoxExtended_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Enter
        If _previouslyEntered = False Then
            _originalValue = Me.SelectedIndex
            _previouslyEntered = True
        End If
    End Sub

End Class
