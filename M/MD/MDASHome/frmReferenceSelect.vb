Public Class frmReferenceSelect
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lvRef As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmReferenceSelect))
        Me.lvRef = New System.Windows.Forms.ListView
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'lvRef
        '
        Me.lvRef.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lvRef.FullRowSelect = True
        Me.lvRef.Location = New System.Drawing.Point(8, 48)
        Me.lvRef.MultiSelect = False
        Me.lvRef.Name = "lvRef"
        Me.lvRef.Size = New System.Drawing.Size(416, 160)
        Me.lvRef.TabIndex = 0
        Me.lvRef.View = System.Windows.Forms.View.Details
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(416, 24)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Select the referece and press OK. "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(136, 224)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(224, 224)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 155
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Ref ID"
        Me.ColumnHeader2.Width = 76
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Last Contact"
        Me.ColumnHeader3.Width = 84
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Attempt"
        Me.ColumnHeader4.Width = 81
        '
        'frmReferenceSelect
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(432, 273)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lvRef)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmReferenceSelect"
        Me.Text = "Reference Select"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public index As Integer
    Public Overloads Sub ShowDialog(ByVal List(,) As String)
        '****Me.BackColor = SP.Gen.GeneralBackColor
        '****Me.ForeColor = SP.Gen.GeneralForeColor
        '****Me.Opacity = SP.Gen.GeneralTransparency
        Dim x As Integer
        For x = 0 To UBound(List, 2) - 1
            Dim i As New ListViewItem
            i.Text = List(1, x)
            i.SubItems.Add(List(0, x))
            i.SubItems.Add(List(3, x))
            i.SubItems.Add(List(4, x))
            lvRef.Items.Add(i)
        Next
        Me.ShowDialog()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        index = -1
        Me.Hide()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        index = lvRef.SelectedIndices(0)
        Me.Hide()
    End Sub
End Class
