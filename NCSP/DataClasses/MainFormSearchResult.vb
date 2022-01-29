Public Class MainFormSearchResult
    Inherits ListViewItem

    Private _accountID As String
    Public ReadOnly Property AccountID() As String
        Get
            Return _accountID
        End Get
    End Property

    Public Sub New(ByVal firstName As String, ByVal mi As String, ByVal lastName As String, ByVal address1 As String, ByVal city As String, ByVal accountID As String)
        MyBase.New(New String() {firstName, mi, lastName, address1, city})
        _accountID = accountID
    End Sub

End Class
