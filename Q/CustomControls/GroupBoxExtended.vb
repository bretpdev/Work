Imports System.Windows.Forms

Public Class GroupBoxExtended
    Inherits GroupBox
    Implements IIsDirtyExtender


    Public ReadOnly Property IsDirty() As Boolean Implements IIsDirtyExtender.IsDirty
        Get
            Return CustomControlUtilities.CheckForIsDirty(Me.Controls.Cast(Of Control).ToList())
        End Get
    End Property

End Class
