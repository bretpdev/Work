Public Class frmMonthEndReport
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
    Friend WithEvents MonthEndCRV As CrystalDecisions.Windows.Forms.CrystalReportViewer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMonthEndReport))
        Me.MonthEndCRV = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.SuspendLayout()
        '
        'MonthEndCRV
        '
        Me.MonthEndCRV.ActiveViewIndex = -1
        Me.MonthEndCRV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MonthEndCRV.DisplayGroupTree = False
        Me.MonthEndCRV.Location = New System.Drawing.Point(8, 8)
        Me.MonthEndCRV.Name = "MonthEndCRV"
        Me.MonthEndCRV.ReportSource = "C:\Program Files\E-MailTracking\MonthEndForRespTracking.rpt"
        Me.MonthEndCRV.Size = New System.Drawing.Size(928, 384)
        Me.MonthEndCRV.TabIndex = 0
        '
        'frmMonthEndReport
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(944, 413)
        Me.Controls.Add(Me.MonthEndCRV)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMonthEndReport"
        Me.Text = "Month End Report"
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
