Imports System.IO
'Imports PowerPoint
Imports Microsoft.Office.Interop
Imports System.Threading
Public Class frmTraining
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
    Friend WithEvents lstPP As System.Windows.Forms.ListBox
    Friend WithEvents btnrun As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmTraining))
        Me.lstPP = New System.Windows.Forms.ListBox
        Me.btnrun = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'lstPP
        '
        Me.lstPP.Location = New System.Drawing.Point(16, 32)
        Me.lstPP.Name = "lstPP"
        Me.lstPP.Size = New System.Drawing.Size(264, 186)
        Me.lstPP.TabIndex = 0
        '
        'btnrun
        '
        Me.btnrun.Location = New System.Drawing.Point(16, 232)
        Me.btnrun.Name = "btnrun"
        Me.btnrun.Size = New System.Drawing.Size(128, 23)
        Me.btnrun.TabIndex = 1
        Me.btnrun.Text = "Run"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(152, 232)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(128, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(264, 24)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Below is a list of Training Presentations."
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(280, 32)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(248, 184)
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'frmTraining
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(528, 270)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnrun)
        Me.Controls.Add(Me.lstPP)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmTraining"
        Me.Text = "Training Presentations"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Overloads Function Show(ByVal TransPar As Double, ByVal BackColor As Color, ByVal ForeColor As Color) As Boolean
        'set opacity level
        Me.Opacity = TransPar
        'set backcolor
        Me.BackColor = BackColor
        'set forecolor
        Me.ForeColor = ForeColor
        Me.ShowDialog()

    End Function

    Private Sub frmTraining_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim path As String = "X:\Training Modules\"
        Dim str As String
        lstPP.Items.Clear()
        If Directory.Exists(path) Then
            For Each str In Directory.GetFiles(path)
                If str.EndsWith(".ppt") Then
                    str = str.Substring(str.LastIndexOf("\"))
                    str = str.Substring(1, str.LastIndexOf(".") - 1)
                    lstPP.Items.Add(str)
                End If
            Next
        End If

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnrun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnrun.Click
        RunPPT()
    End Sub

    Private Sub lstPP_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstPP.DoubleClick
        RunPPT()
    End Sub

    Sub RunPPT()
        SP.KnarlyDUDE.KnarlyDude("To end the slide show early, press the Escape button.", "MauiDUDE")
        If lstPP.SelectedItem = "" Then Exit Sub
        Dim str As String
        Dim oApp As PowerPoint.Application = New PowerPoint.Application
        Dim oPres As PowerPoint.Presentation
        Dim oPresentations As PowerPoint.Presentations
        Dim oSettings As PowerPoint.SlideShowSettings
        Dim oSlideShowWindows As PowerPoint.SlideShowWindows
        oApp.Visible = True
        oApp.WindowState = PowerPoint.PpWindowState.ppWindowMinimized
        oPresentations = oApp.Presentations
        Do While CopyPPT(lstPP.SelectedItem & ".ppt") = False
        Loop
        oPres = oPresentations.Open("C:\Windows\Temp\tempPPT.ppt", True, True, False)
        oSettings = oPres.SlideShowSettings
        oSettings.StartingSlide = 1
        oSettings.EndingSlide = oPres.Slides.Count
        Me.Visible = False
        oSettings.Run()
        Try
            Do While oPres.SlideShowWindow Is Nothing = False
                Thread.Sleep(500)
            Loop
        Catch
        End Try
        'close the empty powerpoint window
        Dim pArr() As Process
        Dim p As Process
        For Each p In Process.GetProcesses
            If p.MainWindowTitle = oApp.Caption Then
                p.CloseMainWindow()
            End If
        Next

        Me.Close()


    End Sub

    Function CopyPPT(ByVal str As String) As Boolean
        Try
            File.Copy("X:\Training Modules\" & str, "C:\Windows\Temp\tempPPT.ppt", True)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
