Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Public Class NullReportfrm
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
    Friend WithEvents crvNullReport As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents btnClose As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(NullReportfrm))
        Me.crvNullReport = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.btnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'crvNullReport
        '
        Me.crvNullReport.ActiveViewIndex = -1
        Me.crvNullReport.Location = New System.Drawing.Point(8, 8)
        Me.crvNullReport.Name = "crvNullReport"
        Me.crvNullReport.ReportSource = "C:\Enterprise Program Files\Tell DUDE\nullRP.rpt"
        Me.crvNullReport.Size = New System.Drawing.Size(792, 472)
        Me.crvNullReport.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(720, 488)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        '
        'NullReportfrm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(808, 517)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.crvNullReport)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "NullReportfrm"
        Me.Text = "Tell DUDE's Null Report"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub
End Class
