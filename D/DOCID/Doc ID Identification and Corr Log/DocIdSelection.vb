Imports System.Collections.Generic

Public Class DocIdSelection

    Private IdSelected As String
    Public Property SelectedId() As String
        Get
            Return IdSelected
        End Get
        Set(ByVal value As String)
            IdSelected = value
        End Set
    End Property

    Dim shouldClose As Boolean = False


    Public Sub New(ByVal docIds As List(Of String), ByVal docType As String)

        InitializeComponent()

        Dim docList As New List(Of String)
        docList.Add("")
        For Each docId As String In docIds
            docList.Add(docId + " - " + docType)
        Next
        DocIdSelections.DataSource = docList
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        SelectedId = DocIdSelections.Text.Substring(0, 5)
        shouldClose = True
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub DocIdSelection_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If shouldClose = False Then
            e.Cancel = True
        End If
    End Sub

    Private Sub DocIdSelections_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DocIdSelections.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            OK_Click(sender, CType(e, EventArgs))
        End If
    End Sub
End Class