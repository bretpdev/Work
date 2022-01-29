Public Class frmGroupReport
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal backColor As Color, ByVal foreColor As Color)
        MyBase.New()
        Me.BackColor = backColor
        Me.ForeColor = foreColor
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents crvGroup As CrystalDecisions.Windows.Forms.CrystalReportViewer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmGroupReport))
        Me.crvGroup = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.btnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'crvGroup
        '
        Me.crvGroup.ActiveViewIndex = -1
        Me.crvGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.crvGroup.Location = New System.Drawing.Point(8, 16)
        Me.crvGroup.Name = "crvGroup"
        Me.crvGroup.ReportSource = Nothing
        Me.crvGroup.Size = New System.Drawing.Size(1000, 464)
        Me.crvGroup.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(928, 488)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        '
        'frmGroupReport
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1016, 517)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.crvGroup)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmGroupReport"
        Me.Text = "frmGroupReport"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmGroupReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim sqlCom As SqlClient.SqlCommand = New SqlClient.SqlCommand("select A.GroupID ,A.Description from GroupTB A", Conn)
        'Conn.Open()
        'Dim sqlR As SqlClient.SqlDataReader = sqlCom.ExecuteReader
        'Do While sqlR.Read
        '    cboGroup.Items.Add(CStr(sqlR("GroupID")))
        'Loop
        'sqlR.Close()
        'Conn.Close()
        Dim rpt As New rptGroup2
        crvGroup.ReportSource = rpt
    End Sub

    'Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim DA As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter("select A.GroupID ,A.Description ,B.Question ,B.Answer ,B.Revised from GroupTB A inner join QuestionTB B on A.GroupID = B.GroupID where A.GroupID = '" & cboGroup.Text & "'", Conn)
    '    Dim DS As DataSet = New DataSet
    '    DA.Fill(DS)
    '    Dim rpt As New rptGroup
    '    rpt.SetDataSource(DS)
    '    crvGroup.ReportSource = rpt
    'End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub
End Class
