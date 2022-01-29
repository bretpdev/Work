Public Class frmNoteDUDE
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByRef B As SP.Borrower)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Bor = B
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
    Protected Friend WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents tbNoteText As System.Windows.Forms.TextBox
    Protected Friend WithEvents btnOK As System.Windows.Forms.Button
    Protected Friend WithEvents btnCancel As System.Windows.Forms.Button
    Protected Friend WithEvents Label2 As System.Windows.Forms.Label
    Protected Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNoteDUDE))
        Me.tbNoteText = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbNoteText
        '
        Me.tbNoteText.Location = New System.Drawing.Point(8, 112)
        Me.tbNoteText.MaxLength = 1400
        Me.tbNoteText.Multiline = True
        Me.tbNoteText.Name = "tbNoteText"
        Me.tbNoteText.Size = New System.Drawing.Size(672, 256)
        Me.tbNoteText.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("PosterBodoni BT", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(104, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(576, 32)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Note"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnOK
        '
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(256, 376)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 24)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(360, 376)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("PosterBodoni BT", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(104, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(576, 32)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "DUDE"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(16, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(80, 96)
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'frmNoteDUDE
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(688, 413)
        Me.ControlBox = False
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbNoteText)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(696, 440)
        Me.MinimumSize = New System.Drawing.Size(696, 440)
        Me.Name = "frmNoteDUDE"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Note DUDE"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Protected Bor As SP.Borrower

    Public Overloads Function Show(ByVal ForCompass As Boolean) As Boolean

        If ForCompass Then
            tbNoteText.MaxLength = 1400
        Else
            'This is for OneLINK
            tbNoteText.MaxLength = 600
        End If
        Bor.Notes = tbNoteText.Text
        MyBase.Show()
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Cancel()
    End Sub

    Public Overridable Sub Cancel()
        tbNoteText.Text = Bor.Notes
        Me.Hide()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        OK()
    End Sub

    Public Overridable Sub OK()
        Bor.Notes = tbNoteText.Text
        Bor.SpillGuts()
        Me.Hide()
    End Sub

    'this sub is used from the home page to enter MauiDUDE created comments
    Public Sub EnterComment(ByVal TextToEnterIntoNoteDUDE As String)
        tbNoteText.Text = tbNoteText.Text & " " & TextToEnterIntoNoteDUDE
        Bor.Notes = tbNoteText.Text
        Bor.SpillGuts()
    End Sub

    'this sub removes the passed string from the note dude text
    Public Sub RemoveComment(ByVal TextToRemoveFromNoteDude As String)
        tbNoteText.Text = tbNoteText.Text.Replace(TextToRemoveFromNoteDude, "")
        Bor.Notes = tbNoteText.Text
        Bor.SpillGuts()
    End Sub

End Class
