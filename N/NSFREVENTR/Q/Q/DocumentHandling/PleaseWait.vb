Public NotInheritable Class PleaseWait

    Private Shared _instance As PleaseWait

    Public Shared Sub ShowForm()
        If _instance Is Nothing Then
            _instance = New PleaseWait
        End If
        _instance.Show()
        _instance.Refresh()
    End Sub

    Public Shared Sub HideForm()
        _instance.Hide()
    End Sub
End Class
