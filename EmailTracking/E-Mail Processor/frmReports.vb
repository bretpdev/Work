Public Class frmReports
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

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
    Friend WithEvents Viewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmReports))
        Me.Viewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.SuspendLayout()
        '
        'Viewer
        '
        Me.Viewer.ActiveViewIndex = -1
        Me.Viewer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Viewer.DisplayGroupTree = False
        Me.Viewer.Location = New System.Drawing.Point(8, 8)
        Me.Viewer.Name = "Viewer"
        Me.Viewer.ReportSource = Nothing
        Me.Viewer.Size = New System.Drawing.Size(928, 384)
        Me.Viewer.TabIndex = 0
        '
        'frmReports
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(944, 413)
        Me.Controls.Add(Me.Viewer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmReports"
        Me.Text = "Reports"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Overloads Sub ShowDialog(ByVal ReportToDisplay As String)
        Viewer.ReportSource = ReportToDisplay
        Me.Show()
    End Sub

    Public Overloads Sub ShowDialog(ByVal ReportToDisplay As Object) 'ByVal ReportToDisplay As String)
        Viewer.ReportSource = ReportToDisplay
        Me.Show()
    End Sub
End Class
