Public Class frmValidatingUIDs
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        
        'Add any initialization after the InitializeComponent() call
        Dim Back As New Bitmap(Me.Width, Me.Height)
        Dim G As Graphics
        G = Graphics.FromImage(Back)
        G.Clear(Color.Transparent)
        G.DrawString(lblTred1.Text, lblTred1.Font, Brushes.Olive, lblTred1.Location.X, lblTred1.Location.Y)
        G.DrawString(lblTred2.Text, lblTred2.Font, Brushes.Olive, lblTred2.Location.X, lblTred2.Location.Y)
        G.DrawString(lblstr.Text, lblstr.Font, Brushes.Red, lblstr.Location.X, lblstr.Location.Y)

        Me.BackgroundImage = Back
        'Panel2.BackgroundImage = Back
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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblTred1 As System.Windows.Forms.Label
    Friend WithEvents lblTred2 As System.Windows.Forms.Label
    Friend WithEvents lblstr As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmValidatingUIDs))
        Me.lblTred1 = New System.Windows.Forms.Label
        Me.lblTred2 = New System.Windows.Forms.Label
        Me.lblstr = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.SuspendLayout()
        '
        'lblTred1
        '
        Me.lblTred1.BackColor = System.Drawing.Color.Transparent
        Me.lblTred1.Font = New System.Drawing.Font("MS Outlook", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.lblTred1.ForeColor = System.Drawing.Color.Olive
        Me.lblTred1.Location = New System.Drawing.Point(16, 16)
        Me.lblTred1.Name = "lblTred1"
        Me.lblTred1.Size = New System.Drawing.Size(336, 16)
        Me.lblTred1.TabIndex = 1
        Me.lblTred1.Text = "11111111111111111111111111111111111111111111111111"
        Me.lblTred1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblTred1.Visible = False
        '
        'lblTred2
        '
        Me.lblTred2.BackColor = System.Drawing.Color.Transparent
        Me.lblTred2.Font = New System.Drawing.Font("MS Outlook", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.lblTred2.ForeColor = System.Drawing.Color.Olive
        Me.lblTred2.Location = New System.Drawing.Point(16, 80)
        Me.lblTred2.Name = "lblTred2"
        Me.lblTred2.Size = New System.Drawing.Size(336, 16)
        Me.lblTred2.TabIndex = 2
        Me.lblTred2.Text = "11111111111111111111111111111111111111111111111111"
        Me.lblTred2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblTred2.Visible = False
        '
        'lblstr
        '
        Me.lblstr.BackColor = System.Drawing.Color.Transparent
        Me.lblstr.Font = New System.Drawing.Font("Stencil", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblstr.ForeColor = System.Drawing.Color.Red
        Me.lblstr.Location = New System.Drawing.Point(32, 40)
        Me.lblstr.Name = "lblstr"
        Me.lblstr.Size = New System.Drawing.Size(312, 32)
        Me.lblstr.TabIndex = 3
        Me.lblstr.Text = "S e a r c h i n g . . ."
        Me.lblstr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblstr.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
        Me.Panel1.Location = New System.Drawing.Point(352, 8)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 90)
        Me.Panel1.TabIndex = 4
        '
        'frmValidatingUIDs
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(560, 112)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblTred2)
        Me.Controls.Add(Me.lblTred1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblstr)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmValidatingUIDs"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmValidatingUIDs"
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.Black
        Me.ResumeLayout(False)

    End Sub

#End Region

    
End Class
