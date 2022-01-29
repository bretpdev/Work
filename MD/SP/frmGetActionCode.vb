Imports System.Drawing
Imports System.Windows.Forms

Public Class frmGetActionCode
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
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtAction As System.Windows.Forms.TextBox
    Friend WithEvents lblCT As System.Windows.Forms.Label
    Friend WithEvents lblAT As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGetActionCode))
        Me.btnOK = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtAction = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblCT = New System.Windows.Forms.Label
        Me.lblAT = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.pnlMain = New System.Windows.Forms.Panel
        Me.pnlMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.Transparent
        Me.btnOK.ForeColor = System.Drawing.Color.Lime
        Me.btnOK.Location = New System.Drawing.Point(176, 216)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(80, 23)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Lime
        Me.Label1.Location = New System.Drawing.Point(48, 88)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(320, 23)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please enter a valid Action Code and press ""OK""."
        '
        'txtAction
        '
        Me.txtAction.Location = New System.Drawing.Point(168, 176)
        Me.txtAction.MaxLength = 5
        Me.txtAction.Name = "txtAction"
        Me.txtAction.Size = New System.Drawing.Size(100, 20)
        Me.txtAction.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Lime
        Me.Label2.Location = New System.Drawing.Point(56, 144)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 23)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Contact Code:"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Lime
        Me.Label3.Location = New System.Drawing.Point(56, 120)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 23)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Activity Code:"
        '
        'lblCT
        '
        Me.lblCT.BackColor = System.Drawing.Color.Transparent
        Me.lblCT.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCT.ForeColor = System.Drawing.Color.Lime
        Me.lblCT.Location = New System.Drawing.Point(160, 144)
        Me.lblCT.Name = "lblCT"
        Me.lblCT.Size = New System.Drawing.Size(88, 23)
        Me.lblCT.TabIndex = 5
        '
        'lblAT
        '
        Me.lblAT.BackColor = System.Drawing.Color.Transparent
        Me.lblAT.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAT.ForeColor = System.Drawing.Color.Lime
        Me.lblAT.Location = New System.Drawing.Point(160, 120)
        Me.lblAT.Name = "lblAT"
        Me.lblAT.Size = New System.Drawing.Size(88, 23)
        Me.lblAT.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Lime
        Me.Label4.Location = New System.Drawing.Point(56, 176)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 23)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Action Code:"
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.Transparent
        Me.pnlMain.BackgroundImage = CType(resources.GetObject("pnlMain.BackgroundImage"), System.Drawing.Image)
        Me.pnlMain.Controls.Add(Me.lblCT)
        Me.pnlMain.Controls.Add(Me.lblAT)
        Me.pnlMain.Controls.Add(Me.Label1)
        Me.pnlMain.Controls.Add(Me.Label4)
        Me.pnlMain.Controls.Add(Me.txtAction)
        Me.pnlMain.Controls.Add(Me.btnOK)
        Me.pnlMain.Controls.Add(Me.Label2)
        Me.pnlMain.Controls.Add(Me.Label3)
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(400, 296)
        Me.pnlMain.TabIndex = 8
        '
        'frmGetActionCode
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Blue
        Me.ClientSize = New System.Drawing.Size(400, 294)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmGetActionCode"
        Me.Text = "Maui DUDE -- Get Action Code"
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.Blue
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private blnMoving As Boolean = False
    Private MouseDownX As Integer
    Private MouseDownY As Integer

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Hide()
    End Sub

    Private Sub txtAction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAction.KeyPress
        If e.KeyChar() = Chr(13) Then
            btnOK.PerformClick()
        End If
    End Sub

    Private Sub pnlMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlMain.MouseDown
        If e.Button = MouseButtons.Left Then
            blnMoving = True
            MouseDownX = e.X
            MouseDownY = e.Y
        End If
    End Sub

    Private Sub pnlMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlMain.MouseMove
        If blnMoving Then
            Dim temp As Point = New Point
            temp.X = Me.Location.X + (e.X - MouseDownX)
            temp.Y = Me.Location.Y + (e.Y - MouseDownY)
            Me.Location = temp
        End If
    End Sub

    Private Sub pnlMain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlMain.MouseUp
        If e.Button = MouseButtons.Left Then
            blnMoving = False
        End If
    End Sub
End Class
