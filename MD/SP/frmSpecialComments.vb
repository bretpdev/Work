Public Class frmSpecialComments
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
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblARC As System.Windows.Forms.Label
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents lblText As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSpecialComments))
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblARC = New System.Windows.Forms.Label
        Me.lblDate = New System.Windows.Forms.Label
        Me.lblText = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(200, 184)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Close"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(40, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 32)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "ARC:"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(264, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 32)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Date:"
        '
        'lblARC
        '
        Me.lblARC.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblARC.Location = New System.Drawing.Point(104, 16)
        Me.lblARC.Name = "lblARC"
        Me.lblARC.Size = New System.Drawing.Size(136, 32)
        Me.lblARC.TabIndex = 3
        Me.lblARC.Text = "MXADD"
        '
        'lblDate
        '
        Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.Location = New System.Drawing.Point(328, 16)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(120, 32)
        Me.lblDate.TabIndex = 4
        Me.lblDate.Text = "06/08/78"
        '
        'lblText
        '
        Me.lblText.Location = New System.Drawing.Point(128, 56)
        Me.lblText.Name = "lblText"
        Me.lblText.Size = New System.Drawing.Size(344, 112)
        Me.lblText.TabIndex = 5
        Me.lblText.Text = "ARC text"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(8, 56)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(112, 112)
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'frmSpecialComments
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(480, 221)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblText)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.lblARC)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSpecialComments"
        Me.Text = "Maui DUDE [Special Comments]"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Overloads Sub Show(ByVal ARC As String, ByVal ARCText As String, ByVal ARCDate As String)
        lblARC.Text = ARC
        lblText.Text = ARCText
        lblDate.Text = ARCDate
        '****Me.ForeColor = SP.Gen.GeneralForeColor
        '****Me.BackColor = SP.Gen.GeneralBackColor
        '****Me.Opacity = SP.Gen.GeneralTransparency
        Me.ShowDialog()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub

    Private Sub frmSpecialComments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
