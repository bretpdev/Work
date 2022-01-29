<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ActivityCmts
    Inherits System.Windows.Forms.UserControl

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal DorN As DaysOrNumberOf, ByVal Count As Integer, ByVal UserRequested As Boolean, ByVal TBor As SP.Borrower)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Bor = TBor
        Success = SetupLP50History(DorN, Count)
        If Success = False And UserRequested = True Then
            SP.frmWhoaDUDE.WhoaDUDE("It's a bum day for surfin'. This borrower has no history in the requested time frame.", "MauiDUDE")
        End If
    End Sub

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.dgAH = New System.Windows.Forms.DataGrid
        Me.DV = New System.Data.DataView
        CType(Me.dgAH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgAH
        '
        Me.dgAH.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgAH.CaptionVisible = False
        Me.dgAH.DataMember = ""
        Me.dgAH.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.dgAH.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgAH.Location = New System.Drawing.Point(0, 0)
        Me.dgAH.Name = "dgAH"
        Me.dgAH.ReadOnly = True
        Me.dgAH.RowHeadersVisible = False
        Me.dgAH.Size = New System.Drawing.Size(808, 312)
        Me.dgAH.TabIndex = 4
        '
        'ActivityCmts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgAH)
        Me.Name = "ActivityCmts"
        Me.Size = New System.Drawing.Size(808, 312)
        CType(Me.dgAH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgAH As System.Windows.Forms.DataGrid
    Friend WithEvents DV As System.Data.DataView

End Class
