Imports System.Drawing.Drawing2D
Public Class Tank
    Public BackBmp As Bitmap
    Public Gr As Graphics
    Public Width As Integer
    Public Height As Integer
    Public lefthandDown As Boolean
    Public righthandDown As Boolean
    Public TargetBatch As Integer
    Public Batch1Done As Boolean
    Public Batch2Done As Boolean
    Public Batch3Done As Boolean
    Public Batch4Done As Boolean
    Public Batch5Done As Boolean
    Public ShowTanks As Boolean


    Public Sub DrawTank()
        DrawField()
        DrawCockpit()
    End Sub

    Private Sub DrawCockpit()


        'left panel
        Dim point1 As PointF = New PointF(0, 0)
        Dim point2 As PointF = New PointF(0, Height)
        Dim point3 As PointF = New PointF(250, Height)
        Dim point4 As PointF = New PointF(250, 0)
        Dim curvePoints As PointF() = {point1, point2, point3, point4}
        'right panel
        Dim point6 As PointF = New PointF(Width - 220, 0)
        Dim point7 As PointF = New PointF(Width - 220, Height)
        Dim point8 As PointF = New PointF(Width, Height)
        Dim point9 As PointF = New PointF(Width, 0)
        Dim curvePoints2 As PointF() = {point6, point7, point8, point9}
        'top panel

        Dim rec1 As New RectangleF(200, 0, Width - 420, 350)
        'bottom panel
        Dim rec2 As New RectangleF(0, Height - 220, Width, 200)
        Dim point10 As PointF = New PointF(0, Height - 100)
        Dim point11 As PointF = New PointF(0, Height)
        Dim point12 As PointF = New PointF(Width, Height)
        Dim point13 As PointF = New PointF(Width, Height - 100)
        Dim point14 As PointF = New PointF(Width - 220, Height - 220)
        Dim point15 As PointF = New PointF(250, Height - 220)
        Dim curvePoints3 As PointF() = {point10, point11, point12, point13, point14, point15}

        'Dim B1 As New LinearGradientBrush(point1, point4, Color.DarkOliveGreen, Color.Black)
        'Dim B2 As New LinearGradientBrush(New PointF(Width - 301, 0), New PointF(Width, 0), Color.Black, Color.DarkOliveGreen)
        'Dim B3 As New LinearGradientBrush(rec1, Color.DarkOliveGreen, Color.Black, LinearGradientMode.Vertical)
        'Dim B4 As New LinearGradientBrush(rec2, Color.Black, Color.DarkOliveGreen, LinearGradientMode.Vertical)
        Dim B1 As New LinearGradientBrush(point1, point4, Color.FromArgb(64, 64, 64), Color.Black)
        Dim B2 As New LinearGradientBrush(New PointF(Width - 301, 0), New PointF(Width, 0), Color.Black, Color.FromArgb(64, 64, 64))
        Dim B3 As New LinearGradientBrush(rec1, Color.FromArgb(64, 64, 64), Color.Black, LinearGradientMode.Vertical)
        Dim B4 As New LinearGradientBrush(rec2, Color.Black, Color.FromArgb(64, 64, 64), LinearGradientMode.Vertical)
        Gr.FillPolygon(B1, curvePoints)
        Gr.FillPolygon(B2, curvePoints2)
        Gr.FillRectangle(B3, rec1)
        Gr.FillPolygon(B4, curvePoints3)

        'draw fram
        Dim SteelBrush As New TextureBrush(Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\Modified\steelBack.gif"))
        Dim recTopFrame As New RectangleF(250, 350, 440, 10)
        Dim recBottomFrame As New RectangleF(250, 440, 440, 10)
        Dim recLeftFrame As New RectangleF(250, 350, 10, 100)
        Dim recRightFrame As New RectangleF(680, 350, 10, 100)
        Gr.FillRectangle(SteelBrush, recTopFrame)
        Gr.FillRectangle(SteelBrush, recBottomFrame)
        Gr.FillRectangle(SteelBrush, recLeftFrame)
        Gr.FillRectangle(SteelBrush, recRightFrame)

        'cross hairs
        Gr.DrawEllipse(Pens.Red, New RectangleF(Width / 2 - 15, Height / 2 + 50, 30, 30))
        Gr.DrawLine(Pens.Red, New PointF(Width / 2, Height / 2 + 35), New PointF(Width / 2, Height / 2 + 95))
        Gr.DrawLine(Pens.Red, New PointF(Width / 2 - 30, Height / 2 + 65), New PointF(Width / 2 + 30, Height / 2 + 65))

        'hands
        Dim LeftHandImage As Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\lefthand.gif")
        Dim RightHandImage As Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\righthand.gif")
        If lefthandDown Then
            Gr.DrawImage(LeftHandImage, New Rectangle(Width / 2 - 250, Height - 170, 200, 200))
        Else
            Gr.DrawImage(LeftHandImage, New Rectangle(Width / 2 - 250, Height - 200, 200, 200))
        End If
        If righthandDown Then
            Gr.DrawImage(RightHandImage, New Rectangle(Width / 2 + 50, Height - 170, 200, 200))
        Else
            Gr.DrawImage(RightHandImage, New Rectangle(Width / 2 + 50, Height - 200, 200, 200))
        End If

        'bullets
        'Dim bulletImage As Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\Modified\steelBack.gif")
        'Gr.DrawImage(bulletImage, New Rectangle(Width - 180, 0, 200, 75))
        'Gr.DrawImage(bulletImage, New Rectangle(Width - 180, 75, 200, 75))
        'Gr.DrawImage(bulletImage, New Rectangle(Width - 180, 150, 200, 75))
        'Gr.DrawImage(bulletImage, New Rectangle(Width - 180, 225, 200, 75))
        'Gr.DrawImage(bulletImage, New Rectangle(Width - 180, 300, 200, 75))

        Dim TankImage As Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\SideTank3.gif")
        Gr.DrawImage(TankImage, New Rectangle(Width - 120, Height - 220, 90, 180))

    End Sub

    Private Sub DrawField()
        BackBmp = New Bitmap(Width, Height)
        Gr = Graphics.FromImage(BackBmp)
        'draw sky and land
        Dim rec1 As New RectangleF(250, 350, 440, 100)
        Dim rec2 As New RectangleF(250, 400, 440, 50)
        Dim B3 As New LinearGradientBrush(rec1, Color.LightBlue, Color.SkyBlue, LinearGradientMode.Vertical)
        Dim B4 As New LinearGradientBrush(rec2, Color.SaddleBrown, Color.Tan, LinearGradientMode.Vertical)
        Gr.FillRectangle(B3, rec1)
        Gr.FillRectangle(B4, rec2)

        'draw tanks
        If ShowTanks Then
            Dim B As Integer
            Dim tankImage As Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\tank1.gif")
            Dim tankDoneImage As Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\tankonfire.gif")
            Dim cloudImage As Image = Image.FromFile("X:\PADU\Stand Alone Apps\MasterBatch\Graphix\cloud.gif")
            'cloud
            Gr.DrawImage(cloudImage, New Rectangle(Width / 2 - 170 + (300) - ((TargetBatch - 1) * 120), Height / 2 + 30, 100, 40))
            'tanks
            B = 1
            If Batch1Done Then
                Gr.DrawImage(tankDoneImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            Else
                Gr.DrawImage(tankImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            End If
            B = 2
            If Batch2Done Then
                Gr.DrawImage(tankDoneImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            Else
                Gr.DrawImage(tankImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            End If
            B = 3
            If Batch3Done Then
                Gr.DrawImage(tankDoneImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            Else
                Gr.DrawImage(tankImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            End If
            B = 4
            If Batch4Done Then
                Gr.DrawImage(tankDoneImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            Else
                Gr.DrawImage(tankImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            End If
            B = 5
            If Batch5Done Then
                Gr.DrawImage(tankDoneImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            Else
                Gr.DrawImage(tankImage, New Rectangle(Width / 2 - 170 + (B * 120) - ((TargetBatch - 1) * 120), Height / 2 + 45, 100, 50))
            End If
        End If
    End Sub
End Class
