Imports System.Drawing
Public Class frmQueues
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
        Me.Hide()
        'If disposing Then
        '    If Not (components Is Nothing) Then
        '        components.Dispose()
        '    End If
        'End If
        'MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lstQ As System.Windows.Forms.ListBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmQueues))
        Me.btnClose = New System.Windows.Forms.Button
        Me.lstQ = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.Location = New System.Drawing.Point(45, 212)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lstQ
        '
        Me.lstQ.Location = New System.Drawing.Point(12, 12)
        Me.lstQ.Name = "lstQ"
        Me.lstQ.Size = New System.Drawing.Size(136, 186)
        Me.lstQ.TabIndex = 2
        '
        'frmQueues
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(161, 247)
        Me.Controls.Add(Me.lstQ)
        Me.Controls.Add(Me.btnClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmQueues"
        Me.Text = "Maui DUDE Default Queues"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Overloads Function Show(ByVal arr As ArrayList) As Boolean
        If arr Is Nothing Then Exit Function 'exit function if array list is nothing
        Dim x As Integer
        lstQ.Items.Clear()
        For x = 0 To arr.Count - 1
            lstQ.Items.Add(arr.Item(x))
        Next x
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
        Me.Show()
    End Function

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub

    Private Sub frmQueues_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        Dim gr As Graphics
        gr = Me.CreateGraphics
        Dim brush As New Drawing2D.LinearGradientBrush(New PointF(0, 20), New PointF(0, Me.Height), Me.BackColor, Me.ForeColor)
        gr.FillRectangle(brush, 0, 20, Me.Width, Me.Height)
    End Sub

End Class
