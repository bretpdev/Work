Imports System.Windows.Forms

Public Class TextBoxExtended
    Inherits TextBox
    Implements IIsDirtyExtender

    ' List of extended functionality
    ' - Line Count function that returns the total number of lines in a textbox if multi-line
    ' - Is Dirty property that tracks if user changed the value of the text box.

    ''' <summary>
    ''' Returns true if the user changed the value of the text box at any point.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsDirty() As Boolean Implements IIsDirtyExtender.IsDirty
        Get
            If _originalValue Is Nothing Then
                Return False
            ElseIf _originalValue = Me.Text Then
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

    Private _originalValue As String = Nothing
    Protected Property OriginalValue() As String
        Get
            Return _originalValue
        End Get
        Set(ByVal value As String)
            _originalValue = value
        End Set
    End Property

    Private Shared EM_GETLINECOUNT As Integer = Convert.ToInt32("0xBA", 16)
    'Integer.Parse("0xBA", Globalization.NumberStyles.HexNumber)

    ''' <summary>
    ''' Returns the number of lines in the text box.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LineCount()
        Dim msg As Message = Message.Create(Me.Handle, EM_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero)
        MyBase.DefWndProc(msg)
        Return msg.Result.ToInt32
    End Function

    Private Sub TextBoxWithIsDirtyIndicator_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter
        If _previouslyEntered = False Then
            _originalValue = Me.Text
            _previouslyEntered = True
        End If
    End Sub

End Class
