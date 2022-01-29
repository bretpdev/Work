Public Class prog
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
    Friend WithEvents lblbar As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lblbar = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblbar
        '
        Me.lblbar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblbar.Location = New System.Drawing.Point(8, 8)
        Me.lblbar.Name = "lblbar"
        Me.lblbar.Size = New System.Drawing.Size(300, 16)
        Me.lblbar.TabIndex = 0
        '
        'prog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(320, 37)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblbar)
        Me.Name = "prog"
        Me.Text = "Update in progress..."
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
