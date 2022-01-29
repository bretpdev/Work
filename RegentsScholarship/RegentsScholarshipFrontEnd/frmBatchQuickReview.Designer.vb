<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchQuickReview
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatchQuickReview))
        Me.pnlReviews = New System.Windows.Forms.FlowLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnProcess = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'pnlReviews
        '
        Me.pnlReviews.AutoScroll = True
        Me.pnlReviews.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.pnlReviews.Location = New System.Drawing.Point(11, 31)
        Me.pnlReviews.Name = "pnlReviews"
        Me.pnlReviews.Size = New System.Drawing.Size(738, 540)
        Me.pnlReviews.TabIndex = 0
        Me.pnlReviews.WrapContents = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(126, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(198, 23)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Last Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(330, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(198, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "First Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(534, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(190, 23)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "State Student ID"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'btnProcess
        '
        Me.btnProcess.Location = New System.Drawing.Point(344, 579)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(75, 23)
        Me.btnProcess.TabIndex = 3
        Me.btnProcess.Text = "Process"
        Me.btnProcess.UseVisualStyleBackColor = True
        '
        'frmBatchQuickReview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(761, 614)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnlReviews)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBatchQuickReview"
        Me.Text = "Batch Quick Review"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlReviews As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnProcess As System.Windows.Forms.Button
End Class
