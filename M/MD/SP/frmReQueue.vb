Public Class frmReQueue
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal TempSSN As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
        ReQueueDate.MinDate = Today()
        ReQueueDate.TodayDate = Today()
        SSN = TempSSN
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
    Friend WithEvents btnReQueue As System.Windows.Forms.Button
    Friend WithEvents ReQueueDate As System.Windows.Forms.MonthCalendar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents tbTaskText As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmReQueue))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnReQueue = New System.Windows.Forms.Button
        Me.ReQueueDate = New System.Windows.Forms.MonthCalendar
        Me.tbTaskText = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(296, 328)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'btnReQueue
        '
        Me.btnReQueue.Location = New System.Drawing.Point(200, 328)
        Me.btnReQueue.Name = "btnReQueue"
        Me.btnReQueue.TabIndex = 3
        Me.btnReQueue.Text = "Re-Queue"
        '
        'ReQueueDate
        '
        Me.ReQueueDate.CalendarDimensions = New System.Drawing.Size(2, 1)
        Me.ReQueueDate.Location = New System.Drawing.Point(208, 56)
        Me.ReQueueDate.MaxSelectionCount = 1
        Me.ReQueueDate.MinDate = New Date(2005, 8, 11, 0, 0, 0, 0)
        Me.ReQueueDate.Name = "ReQueueDate"
        Me.ReQueueDate.ShowToday = False
        Me.ReQueueDate.ShowTodayCircle = False
        Me.ReQueueDate.TabIndex = 4
        Me.ReQueueDate.TodayDate = New Date(2005, 8, 11, 0, 0, 0, 0)
        '
        'tbTaskText
        '
        Me.tbTaskText.Location = New System.Drawing.Point(208, 248)
        Me.tbTaskText.MaxLength = 165
        Me.tbTaskText.Multiline = True
        Me.tbTaskText.Name = "tbTaskText"
        Me.tbTaskText.Size = New System.Drawing.Size(360, 72)
        Me.tbTaskText.TabIndex = 5
        Me.tbTaskText.Text = ""
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(184, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Re-Queue Date:"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(184, 232)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(224, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Re-Queue Text:"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(8, 56)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(168, 252)
        Me.PictureBox1.TabIndex = 8
        Me.PictureBox1.TabStop = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(560, 23)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Re-Queue Task"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmReQueue
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(578, 378)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbTaskText)
        Me.Controls.Add(Me.btnReQueue)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.ReQueueDate)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(584, 384)
        Me.MinimumSize = New System.Drawing.Size(584, 384)
        Me.Name = "frmReQueue"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Re-Queue Task"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private SSN As String
    Private FormCancelled As Boolean

    'this sub uses previously gathered information to requeue a task
    Public Sub ReQueueTask()
        'add queue task
        SP.Q.FastPath("LP9OA" & SSN & ";;ZZZZCOMP")
        SP.Q.PutText(11, 25, Format(ReQueueDate.SelectionStart(), "MMddyyyy"))
        SP.Q.PutText(16, 12, SSN & ", DCALL, D, " & tbTaskText.Text)
        SP.Q.Hit("F6")
    End Sub

    Private Sub btnReQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReQueue.Click
        FormCancelled = False
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        FormCancelled = True
        tbTaskText.Clear()
        Me.Hide()
    End Sub


    Public Function InformationGathered() As Boolean
        Return Not FormCancelled
    End Function
End Class
