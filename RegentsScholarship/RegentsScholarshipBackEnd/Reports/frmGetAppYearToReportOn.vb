Public Class frmGetAppYearToReportOn

    'the selected application year
    Public ReadOnly Property ApplicationYear() As String
        Get
            Return cmbAppYear.Text
        End Get
    End Property


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        cmbAppYear.Items.Add("All")
        Dim years As List(Of String) = Lookups.ProgramYears.Select(Function(p) p.Year).OrderByDescending(Function(p) p).ToList()
        For Each year As String In years
            cmbAppYear.Items.Add(year)
        Next
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub
End Class