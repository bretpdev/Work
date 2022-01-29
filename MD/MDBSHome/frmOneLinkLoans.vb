Public Class frmOneLinkLoans
    Public Sub New(ByVal LG10Data(,) As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvLoans.BackColor = Me.BackColor
        lvLoans.ForeColor = Me.ForeColor
        For x As Integer = LG10Data.GetLowerBound(1) To LG10Data.GetUpperBound(1)
            If String.IsNullOrEmpty(LG10Data(1, x)) Then Continue For
            Dim newItemIndex As Integer = lvLoans.Items.Add(LG10Data(1, x)).Index
            lvLoans.Items(newItemIndex).SubItems.Add(LG10Data(2, x))
            Dim dateProcessed As String = LG10Data(3, x)
            If dateProcessed IsNot Nothing AndAlso dateProcessed.Length > 5 Then dateProcessed = dateProcessed.Insert(2, "/").Insert(5, "/")
            lvLoans.Items(newItemIndex).SubItems.Add(dateProcessed)
            lvLoans.Items(newItemIndex).SubItems.Add(LG10Data(4, x))
            lvLoans.Items(newItemIndex).SubItems.Add(LG10Data(0, x))
            lvLoans.Items(newItemIndex).SubItems.Add(LG10Data(5, x))
            lvLoans.Items(newItemIndex).SubItems.Add(LG10Data(6, x))
        Next x
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
