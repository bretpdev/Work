Public Class CheckBoxExtended
    Inherits CheckBox
    Implements IIsDirtyExtender


    ''' <summary>
    ''' Is true if the user changed the value of the combo box at any point.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsDirty() As Boolean Implements IIsDirtyExtender.IsDirty
        Get
            If _originalValue = _previouslyEntered Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Private _previouslyEntered As Boolean
    Protected Property PreviouslyEntered() As Boolean
        Get
            If Me.Checked Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            _previouslyEntered = value
        End Set
    End Property

    Private _originalValue As Boolean
    Protected Property OriginalValue() As Boolean
        Get
            If Me.Checked Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            _originalValue = value
        End Set
    End Property

    Private Sub CheckBoxExtended_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.CheckedChanged
        If Me.Checked Then
            _previouslyEntered = True
        Else
            _previouslyEntered = False
        End If
    End Sub
End Class
