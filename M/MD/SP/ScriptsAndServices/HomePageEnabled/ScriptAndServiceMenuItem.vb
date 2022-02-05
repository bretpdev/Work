Public Class ScriptAndServiceMenuItem
    Inherits System.Windows.Forms.ToolStripMenuItem

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByRef tdrData As System.Data.DataRow, ByVal Text As String)
        MyBase.New(Text)
        drData = tdrData
    End Sub

    Private drData As System.Data.DataRow
    Public Property gsData() As System.Data.DataRow
        Get
            Return drData
        End Get
        Set(ByVal value As System.Data.DataRow)
            drData = value
        End Set
    End Property


End Class
