<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPickCampaign
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPickCampaign))
        Me.lvExistingCamp = New System.Windows.Forms.ListView
        Me.chSubjectLine = New System.Windows.Forms.ColumnHeader
        Me.chDataFile = New System.Windows.Forms.ColumnHeader
        Me.chHTMLFile = New System.Windows.Forms.ColumnHeader
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lvExistingCamp
        '
        Me.lvExistingCamp.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chSubjectLine, Me.chDataFile, Me.chHTMLFile})
        Me.lvExistingCamp.FullRowSelect = True
        Me.lvExistingCamp.LabelWrap = False
        Me.lvExistingCamp.Location = New System.Drawing.Point(2, 38)
        Me.lvExistingCamp.MultiSelect = False
        Me.lvExistingCamp.Name = "lvExistingCamp"
        Me.lvExistingCamp.Size = New System.Drawing.Size(485, 147)
        Me.lvExistingCamp.TabIndex = 0
        Me.lvExistingCamp.UseCompatibleStateImageBehavior = False
        Me.lvExistingCamp.View = System.Windows.Forms.View.Details
        '
        'chSubjectLine
        '
        Me.chSubjectLine.Text = "Subject Line"
        Me.chSubjectLine.Width = 304
        '
        'chDataFile
        '
        Me.chDataFile.Text = "Data File"
        Me.chDataFile.Width = 81
        '
        'chHTMLFile
        '
        Me.chHTMLFile.Text = "HTML File"
        Me.chHTMLFile.Width = 93
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(-1, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(488, 33)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please select an existing campaign and click the open button, or click the new bu" & _
            "tton to create a new campaign."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(164, 190)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(75, 23)
        Me.btnOpen.TabIndex = 2
        Me.btnOpen.Text = "Open"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(250, 190)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(75, 23)
        Me.btnNew.TabIndex = 3
        Me.btnNew.Text = "New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'frmPickCampaign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(488, 215)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lvExistingCamp)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPickCampaign"
        Me.Text = "Pick Your Email Campaign"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvExistingCamp As System.Windows.Forms.ListView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chSubjectLine As System.Windows.Forms.ColumnHeader
    Friend WithEvents chDataFile As System.Windows.Forms.ColumnHeader
    Friend WithEvents chHTMLFile As System.Windows.Forms.ColumnHeader

End Class
