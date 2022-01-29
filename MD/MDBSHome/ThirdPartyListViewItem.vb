Public Class ThirdPartyListViewItem
    Inherits System.Windows.Forms.ListViewItem

    Private Shadows _name As String
    Private _relationship As String
    Private _relationshipType As String

    Public Sub New(ByVal relationshipType As String, ByVal name As String, ByVal relationship As String)
        'TODO: These private variables don't appear to be needed.
        'If commenting them out doesn't break things, remove them.
        '_name = name
        '_relationship = relationship
        '_relationshipType = relationshipType
        MyBase.Text = relationshipType
        MyBase.SubItems.Add(name)
        MyBase.SubItems.Add(relationship)
    End Sub
End Class
