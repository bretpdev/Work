Partial Public Class frm3rdParty
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.Hide()
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lv3rdParty As System.Windows.Forms.ListView
    Friend WithEvents RelationshipType As System.Windows.Forms.ColumnHeader
    Friend WithEvents PersonOrInstitution As System.Windows.Forms.ColumnHeader
    Friend WithEvents Relationship As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClose As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frm3rdParty))
        Me.lv3rdParty = New System.Windows.Forms.ListView
        Me.RelationshipType = New System.Windows.Forms.ColumnHeader
        Me.PersonOrInstitution = New System.Windows.Forms.ColumnHeader
        Me.Relationship = New System.Windows.Forms.ColumnHeader
        Me.btnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lv3rdParty
        '
        Me.lv3rdParty.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.RelationshipType, Me.PersonOrInstitution, Me.Relationship})
        Me.lv3rdParty.FullRowSelect = True
        Me.lv3rdParty.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lv3rdParty.Location = New System.Drawing.Point(8, 8)
        Me.lv3rdParty.MultiSelect = False
        Me.lv3rdParty.Name = "lv3rdParty"
        Me.lv3rdParty.Size = New System.Drawing.Size(456, 128)
        Me.lv3rdParty.TabIndex = 0
        Me.lv3rdParty.View = System.Windows.Forms.View.Details
        '
        'RelationshipType
        '
        Me.RelationshipType.Text = "Relationship Type"
        Me.RelationshipType.Width = 104
        '
        'PersonOrInstitution
        '
        Me.PersonOrInstitution.Text = "Person/Institution Name"
        Me.PersonOrInstitution.Width = 197
        '
        'Relationship
        '
        Me.Relationship.Text = "Relationship"
        Me.Relationship.Width = 134
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(200, 144)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        '
        'frm3rdParty
        '
        Me.AcceptButton = Me.btnClose
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(470, 171)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lv3rdParty)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(480, 200)
        Me.Name = "frm3rdParty"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "3rd Party Info"
        Me.ResumeLayout(False)
    End Sub
End Class
