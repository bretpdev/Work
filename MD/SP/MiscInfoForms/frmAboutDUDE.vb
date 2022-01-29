Public Class frmAboutDUDE
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmAboutDUDE))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(240, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Maui DUDE stands for:"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(224, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "- Maui:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(272, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Just sounds good."
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(224, 96)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 23)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "- D:"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(272, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Digital"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(224, 112)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 23)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "- U:"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(272, 112)
        Me.Label7.Name = "Label7"
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Utility"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(272, 128)
        Me.Label8.Name = "Label8"
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "for"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(224, 144)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(40, 23)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "- D:"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(272, 144)
        Me.Label10.Name = "Label10"
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Doing"
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(224, 160)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(40, 23)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "- E:"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(272, 160)
        Me.Label12.Name = "Label12"
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "Everything"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(176, 200)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 12
        Me.btnOK.Text = "OK"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(24, 56)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(136, 136)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 13
        Me.PictureBox1.TabStop = False
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(8, 8)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(408, 40)
        Me.Label13.TabIndex = 14
        Me.Label13.Text = "Tell us about Maui DUDE Big Kahuna!"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmAboutDUDE
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(424, 237)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(432, 264)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(432, 264)
        Me.Name = "frmAboutDUDE"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About DUDE"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub
End Class
