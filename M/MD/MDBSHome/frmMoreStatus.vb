Imports System.Collections.Generic
Imports System.Drawing

Public Class frmMoreStatus
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    Public Overloads Function Show(ByRef statuses As List(Of String), ByVal backColor As Color, ByVal foreColor As Color) As Boolean
        lstStatus.BackColor = backColor
        lstStatus.ForeColor = foreColor
        lstStatus.DataSource = statuses.ToArray()
        Me.Show()
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class
