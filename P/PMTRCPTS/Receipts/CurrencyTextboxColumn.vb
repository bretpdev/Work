Public Class CurrencyTextboxColumn
        Inherits DataGridTextBoxColumn
        Private mTxtAlign As HorizontalAlignment
        Private mDrawTxt As New StringFormat
        Private mbAdjustHeight As Boolean = True
        Private m_intPreEditHeight As Integer
        Private m_rownum As Integer
        Dim WithEvents dg As DataGrid
        Private arHeights As ArrayList

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
        Static bPainted As Boolean = False

        Dim s As String = Me.GetColumnValueAtRow([source], rowNum).ToString()
        Dim r As New RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height)

        If Not s = "" Then
            s = CDbl(s)
        End If
        'If s = "4" And Me.HeaderText = "Pri" Then
        '    foreBrush = Brushes.Red
        '    'Me.TextBox.ForeColor = Color.Red
        'End If
        'If s = "Current" And Me.HeaderText = "Status" Then
        '    foreBrush = Brushes.LimeGreen
        '    'Me.TextBox.BackColor = Color.Green
        'End If

        If Not bPainted Then
            dg = Me.DataGridTableStyle.DataGrid
        End If

        'clear the cell 
        g.FillRectangle(backBrush, bounds)


        'r.Inflate(0, -1)
        '' get the height column should be 
        Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
        Dim h As Integer = CType((sDraw.Height + 15), Integer)


        g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
        bPainted = True

    End Sub



End Class
