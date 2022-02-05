Imports System.Reflection
Imports System.Windows.Forms

Public Class FormBase

    ''' <summary>
    ''' Returns true if any controls that implement IIsDirtyExtender have been modified by the user.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsDirty() As Boolean
        Get
            Return CustomControlUtilities.CheckForIsDirty(Me.Controls.Cast(Of Control).ToList())
        End Get
    End Property


End Class