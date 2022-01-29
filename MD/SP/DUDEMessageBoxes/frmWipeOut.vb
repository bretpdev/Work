Imports System.Drawing
Public Class frmWipeOut
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.Hide()
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
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Public WithEvents lblMessage As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmWipeOut))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lblMessage = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(8, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(136, 136)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'lblMessage
        '
        Me.lblMessage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMessage.Location = New System.Drawing.Point(160, 8)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(416, 136)
        Me.lblMessage.TabIndex = 1
        Me.lblMessage.Text = "Label1"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(240, 160)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(99, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        '
        'frmWipeOut
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(584, 197)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(592, 224)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(592, 224)
        Me.Name = "frmWipeOut"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmWipeOut"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Hide()
    End Sub

    Public Shared Sub WipeOut(ByVal Message As String, ByVal Title As String, Optional ByVal Center As Boolean = False)
        Dim OneOfMe As New SP.frmWipeOut
        OneOfMe.lblMessage.Text = Message
        OneOfMe.Text = "Wipe Out: " & Title
        If Center Then
            OneOfMe.lblMessage.TextAlign = ContentAlignment.MiddleCenter
        Else
            OneOfMe.lblMessage.TextAlign = ContentAlignment.MiddleLeft
        End If
        Beep()
        OneOfMe.ShowDialog()
    End Sub

End Class
