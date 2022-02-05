Public Class frmGetFutureRunTime
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
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents radRunImmediately As System.Windows.Forms.RadioButton
    Friend WithEvents radRunFuture As System.Windows.Forms.RadioButton
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmGetFutureRunTime))
        Me.dtpDate = New System.Windows.Forms.DateTimePicker
        Me.btnOK = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.radRunFuture = New System.Windows.Forms.RadioButton
        Me.radRunImmediately = New System.Windows.Forms.RadioButton
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpDate
        '
        Me.dtpDate.CustomFormat = "MMMM dd, yyyy hh:mm:ss tt"
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDate.Location = New System.Drawing.Point(36, 40)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(232, 20)
        Me.dtpDate.TabIndex = 0
        '
        'btnOK
        '
        Me.btnOK.ForeColor = System.Drawing.Color.Red
        Me.btnOK.Location = New System.Drawing.Point(64, 120)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "Continue"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.radRunFuture)
        Me.GroupBox1.Controls.Add(Me.radRunImmediately)
        Me.GroupBox1.Controls.Add(Me.dtpDate)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.ForeColor = System.Drawing.Color.Red
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(280, 104)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Choose Run Time"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(36, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(232, 28)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "MBS should only be run for the current or following business day."
        '
        'radRunFuture
        '
        Me.radRunFuture.Location = New System.Drawing.Point(16, 40)
        Me.radRunFuture.Name = "radRunFuture"
        Me.radRunFuture.Size = New System.Drawing.Size(16, 24)
        Me.radRunFuture.TabIndex = 2
        '
        'radRunImmediately
        '
        Me.radRunImmediately.ForeColor = System.Drawing.Color.Red
        Me.radRunImmediately.Location = New System.Drawing.Point(16, 16)
        Me.radRunImmediately.Name = "radRunImmediately"
        Me.radRunImmediately.Size = New System.Drawing.Size(152, 24)
        Me.radRunImmediately.TabIndex = 1
        Me.radRunImmediately.Text = " Run Immediately"
        '
        'btnCancel
        '
        Me.btnCancel.ForeColor = System.Drawing.Color.Red
        Me.btnCancel.Location = New System.Drawing.Point(160, 120)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        '
        'frmGetFutureRunTime
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(64, Byte), CType(64, Byte))
        Me.ClientSize = New System.Drawing.Size(298, 151)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(298, 151)
        Me.MinimumSize = New System.Drawing.Size(298, 151)
        Me.Name = "frmGetFutureRunTime"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Run Time"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Enum RunOption
        Immediately
        Future
    End Enum

    Private _selectedTimeDate As DateTime
    Public ReadOnly Property SelectedTimeDate() As DateTime
        Get
            Return _selectedTimeDate
        End Get
    End Property

    Private _selectedRunOption As RunOption
    Public ReadOnly Property SelectedRunOption() As RunOption
        Get
            Return _selectedRunOption
        End Get
    End Property


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If radRunImmediately.Checked Then
            _selectedRunOption = RunOption.Immediately
            _selectedTimeDate = Now
            Me.Hide()
        ElseIf radRunFuture.Checked Then
            'run in the future
            _selectedRunOption = RunOption.Future
            If dtpDate.Value < Now Then
                MessageBox.Show("The start date time must come before the current date time.  Please try again.", "Please Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If dtpDate.Value.DayOfWeek = DayOfWeek.Saturday Or dtpDate.Value.DayOfWeek = DayOfWeek.Sunday Then
                    MessageBox.Show("You have chosen a weekend day to run on.  Master Batch is designed to only run on weekdays.  Please try again.", "Please Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    _selectedTimeDate = dtpDate.Value
                    Me.Hide()
                End If
            End If
        Else
            MessageBox.Show("You must select a run option.  Please try again.", "Please Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub frmGetFutureRunTime_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpDate.MinDate = Today
        dtpDate.MaxDate = Today.AddDays(7)
        dtpDate.Value = Now
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        End
    End Sub
End Class
