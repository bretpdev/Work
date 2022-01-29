Imports System.Drawing

Public Class frmProcessing
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'imaging
        Dim MyAssbly As System.Reflection.Assembly
        MyAssbly = Me.GetType.Assembly
        Dim RM As New Resources.ResourceManager("SP.MauiDUDERes", MyAssbly)
        If Today.Month = 12 Then
            'Surfer2.Image = CType(RM.GetObject("SantaGoinLeft"), System.Drawing.Image)
            Surfer.Image = CType(RM.GetObject("SantaGoinRight"), System.Drawing.Image)
        ElseIf Today.Month = 1 Or Today.Month = 2 Or Today.Month = 3 Or Today.Month = 4 Or Today.Month = 5 Then
            'Surfer2.Image = CType(RM.GetObject("ShaggyGoingLeft"), System.Drawing.Image)
            Surfer.Image = CType(RM.GetObject("ShaggyGoingRight"), System.Drawing.Image)
        Else
            'Surfer2.Image = CType(RM.GetObject("DopyGoingLeft"), System.Drawing.Image)
            Surfer.Image = CType(RM.GetObject("DopyGoinRight"), System.Drawing.Image)
        End If
        'Surfin.IsBackground = True
        'Surfin.Start()
        'Surfin.Suspend()
        'Randomize() 'init random number generator
        'If CInt(Int(2 * Rnd()) + 1) = 1 And (Today.Month = 11 Or _
        '   Today.Month = 12 Or Today.Month = 1 Or Today.Month = 2) Then
        'Snowin = True
        'Else
        '    Snowin = False
        'End If
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
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
    Friend WithEvents Surfer As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProcessing))
        Me.Surfer = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Surfer
        '
        Me.Surfer.BackColor = System.Drawing.Color.Transparent
        Me.Surfer.Font = New System.Drawing.Font("Chiller", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Surfer.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Surfer.Location = New System.Drawing.Point(184, 12)
        Me.Surfer.Name = "Surfer"
        Me.Surfer.Size = New System.Drawing.Size(136, 135)
        Me.Surfer.TabIndex = 3
        Me.Surfer.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'frmProcessing
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.LightGray
        Me.ClientSize = New System.Drawing.Size(512, 224)
        Me.ControlBox = False
        Me.Controls.Add(Me.Surfer)
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(512, 224)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(512, 224)
        Me.Name = "frmProcessing"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Processing Request"
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.Red
        Me.ResumeLayout(False)

    End Sub

#End Region

    'Private Surfin As New Threading.Thread(AddressOf Surf)
    Private Snowin As Boolean

    Private Sub frmProcessing_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        ReDrawBackGround()
        'If Me.Visible Then
        'Surfin.Resume()
        'Else
        '    Surfin.Suspend()
        'End If
    End Sub

    Private Sub ReDrawBackGround()
        Dim APoint As System.Drawing.Point
        Dim WaterBrush As Drawing2D.LinearGradientBrush
        Dim SkyBrush As Drawing2D.LinearGradientBrush
        Dim SunBrush As Drawing2D.LinearGradientBrush
        Dim TextPen As New System.Drawing.SolidBrush(Me.ForeColor)
        Dim TheFont As New Font("Chiller", 20, FontStyle.Bold)
        Dim RectText As New System.Drawing.RectangleF(4, 156, 496, 40)
        Dim StrFormat As New System.Drawing.StringFormat
        StrFormat.Alignment = StringAlignment.Center
        Dim Pen1 As New Pen(Color.Black)
        Dim gr As Graphics = Me.CreateGraphics
        'Create a EMPTY bitmap from that graphics
        Dim Bmp As New Bitmap(Me.Width, Me.Height, gr)
        'Create a Graphics object in memory from that bitmap
        Dim grMem As Graphics = Graphics.FromImage(Bmp)
        'Sky
        Dim BiggerRec As New RectangleF(0, 0, Me.Width, Me.Height - 100)
        'water
        Dim bigRec As New RectangleF(0, Me.Height - 101, Me.Width, 100)
        Dim SunRec As RectangleF
        'Draw Sky
        If Now.Hour < 10 Then
            'morning
            SunRec = New RectangleF(0, Me.Height - 10, Me.Width, Me.Height - 20)
            WaterBrush = New Drawing2D.LinearGradientBrush(bigRec, Color.Cyan, Color.DarkBlue, Drawing2D.LinearGradientMode.Vertical)
            SkyBrush = New Drawing2D.LinearGradientBrush(BiggerRec, Color.Salmon, Color.LemonChiffon, Drawing2D.LinearGradientMode.Vertical)
            SunBrush = New Drawing2D.LinearGradientBrush(SunRec, Color.Yellow, Color.LightGoldenrodYellow, Drawing2D.LinearGradientMode.Vertical)
            grMem.FillRectangle(SkyBrush, BiggerRec)
            'If Snowin = False Then grMem.FillEllipse(SunBrush, CSng((Me.Width / 2)) - 50, 50, 100, 100)
            grMem.FillEllipse(SunBrush, CSng((Me.Width / 2)) - 50, 50, 100, 100)
            grMem.FillRectangle(WaterBrush, bigRec)
        ElseIf Now.Hour < 15 Then
            'afternoon
            SunRec = New RectangleF(0, Me.Height - 10, Me.Width, Me.Height)
            WaterBrush = New Drawing2D.LinearGradientBrush(bigRec, Color.Blue, Color.DarkTurquoise, Drawing2D.LinearGradientMode.Vertical)
            SkyBrush = New Drawing2D.LinearGradientBrush(BiggerRec, Color.Aqua, Color.LightBlue, Drawing2D.LinearGradientMode.Vertical)
            SunBrush = New Drawing2D.LinearGradientBrush(SunRec, Color.Yellow, Color.Gold, Drawing2D.LinearGradientMode.Vertical)
            grMem.FillRectangle(SkyBrush, BiggerRec)
            'If Snowin = False Then grMem.FillEllipse(SunBrush, CSng((Me.Width / 2)) - 50, 0, 100, 100)
            grMem.FillEllipse(SunBrush, CSng((Me.Width / 2)) - 50, 0, 100, 100)
            grMem.FillRectangle(WaterBrush, bigRec)
        Else
            'evening
            SunRec = New RectangleF(0, Me.Height - 10, Me.Width, Me.Height)
            WaterBrush = New Drawing2D.LinearGradientBrush(bigRec, Color.Cyan, Color.Indigo, Drawing2D.LinearGradientMode.Vertical)
            SkyBrush = New Drawing2D.LinearGradientBrush(BiggerRec, Color.Purple, Color.LightSkyBlue, Drawing2D.LinearGradientMode.Vertical)
            SunBrush = New Drawing2D.LinearGradientBrush(SunRec, Color.Wheat, Color.WhiteSmoke, Drawing2D.LinearGradientMode.Vertical)
            grMem.FillRectangle(SkyBrush, BiggerRec)
            'If Snowin = False Then grMem.FillEllipse(SunBrush, CSng((Me.Width / 2)) - 50, 100, 100, 100)
            grMem.FillEllipse(SunBrush, CSng((Me.Width / 2)) - 50, 100, 100, 100)
            grMem.FillRectangle(WaterBrush, bigRec)
        End If
        ''snow
        'If Snowin Then
        '    'Sky
        '    While I < 100
        '        'randomly place snow flakes on screen
        '        SnowFlake = New RectangleF(CInt(Int(Me.Width * Rnd()) + 1), CInt(Int(Me.Height * Rnd()) + 1), CInt(Int((3 * Rnd()) + 1)), CInt(Int(3 * Rnd()) + 1))
        '        grMem.FillRectangle(SnowBrush, SnowFlake)
        '        I = I + 1
        '    End While
        'End If
        'label at bottom
        If Today.Month = 12 Then
            APoint = New System.Drawing.Point(184, 50)
        Else
            APoint = New System.Drawing.Point(184, 8)
        End If
        Surfer.Location = APoint
        grMem.DrawString("Surfing. . . . . . ", TheFont, TextPen, RectText, StrFormat)
        Me.BackgroundImage = Bmp
    End Sub

    'Private Sub Surf()
    '    Dim ReDrawBG As Integer = 0
    '    Dim APoint As System.Drawing.Point
    '    If Today.Month = 12 Then
    '        APoint = New System.Drawing.Point(-136, 50)
    '    Else
    '        APoint = New System.Drawing.Point(-136, 8)
    '    End If
    '    Surfer.Location = APoint
    '    Surfer2.Location = APoint
    '    While 1
    '        APoint.X = -136
    '        While APoint.X < Me.Width
    '            If Snowin Then
    '                'redraw background and snow every twentieth iteration
    '                If ReDrawBG <> 40 Then
    '                    ReDrawBG += 1
    '                Else
    '                    ReDrawBackGround()
    '                    ReDrawBG = 0
    '                End If
    '            End If
    '            Surfer.Location = APoint
    '            'Surfer.Refresh()
    '            Me.Refresh()
    '            Threading.Thread.CurrentThread.Sleep(1)
    '            APoint.X() += 2
    '        End While
    '        While APoint.X > -136
    '            If Snowin Then
    '                'redraw background and snow every twentieth iteration
    '                If ReDrawBG <> 40 Then
    '                    ReDrawBG += 1
    '                Else
    '                    ReDrawBackGround()
    '                    ReDrawBG = 0
    '                End If
    '            End If
    '            Surfer2.Location = APoint
    '            'Surfer2.Refresh()
    '            Me.Refresh()
    '            Threading.Thread.CurrentThread.Sleep(1)
    '            APoint.X() -= 2
    '        End While
    '    End While
    'End Sub

End Class
