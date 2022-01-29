Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports SP.Q

Public Class frmBorrowerBenefits
    Public Sub New(ByVal bsBorrower As BorrowerBS)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        While Check4Text(1, 74, "TCX0D") = False AndAlso Check4Text(1, 72, "TCX06") = False AndAlso Check4Text(1, 72, "TCX0I") = False AndAlso Check4Text(1, 72, "TCX0C") = False
            SP.frmWhoaDUDE.WhoaDUDE("No way DUDE! You gota be on the ACP Screen TCX0D, TCX06, TCX0I, or TCX0C if you wanna keep surfing. Press 'OK' when you are there.", "Bad Screen", True)
        End While
        While (GetText(24, 2, 76).Contains("F8=LOAN")) = False
            Hit("F2")
        End While
        Hit("F8")
        Dim benefitsDataList As New List(Of BorrowerBenefitData)()
        If Check4Text(1, 72, "TSX29") Then
            Dim benefitsData As New BorrowerBenefitData()
            benefitsData.LoanSequence = GetText(7, 35, 4)
            benefitsData.FirstDisbursementDate = GetText(6, 18, 8)
            benefitsData.LoanType = GetText(6, 66, 6)
            benefitsData.CurrentBalance = GetText(11, 12, 10)
            benefitsData.BeginningBalance = GetText(11, 36, 10)
            benefitsData.InterestRate = GetText(11, 58, 22)
            Hit("ENTER")
            Hit("F11")
            Hit("F6")
            If Check4Text(1, 72, "TSXE3") Then
                If Check4Text(12, 10, "QUALIFICATION") Then
                    benefitsData.OnTimeMontlyPayments = GetText(15, 32, 3)
                Else
                    benefitsData.OnTimeMontlyPayments = String.Format("{0} {1}", GetText(12, 35, 10), GetText(12, 62, 8))
                End If
                benefitsData.RequiredOnTimePayments = GetText(15, 41, 3)
            End If
            benefitsDataList.Add(benefitsData)
        Else
            Do While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For row As Integer = 8 To 20
                    If GetText(row, 2, 2).Length > 0 Then
                        Dim benefitsData As New BorrowerBenefitData()
                        benefitsData.LoanSequence = GetText(row, 14, 4)
                        benefitsData.FirstDisbursementDate = GetText(row, 5, 8)
                        benefitsData.LoanType = GetText(row, 19, 6)
                        If Double.Parse(GetText(row, 59, 10).Replace(",", "")) > 0 And Check4Text(row, 69, "CR") = False Then
                            PutText(21, 12, "")
                            Hit("End")
                            PutText(21, 12, GetText(row, 2, 2), True)
                            If Check4Text(1, 72, "TSX29") Then
                                benefitsData.CurrentBalance = GetText(11, 12, 10)
                                benefitsData.BeginningBalance = GetText(11, 36, 10)
                                benefitsData.InterestRate = GetText(11, 58, 22)
                                Hit("ENTER")
                                Hit("F11")
                                Hit("F6")
                                If Check4Text(1, 72, "TSXE3") Then
                                    If Check4Text(12, 10, "QUALIFICATION") Then
                                        benefitsData.OnTimeMontlyPayments = GetText(15, 32, 3)
                                    Else
                                        benefitsData.OnTimeMontlyPayments = String.Format("{0} {1}", GetText(12, 35, 10), GetText(12, 62, 8))
                                    End If
                                    benefitsData.RequiredOnTimePayments = GetText(15, 41, 3)
                                End If
                            End If
                        End If
                        benefitsDataList.Add(benefitsData)
                        Do While Check4Text(1, 72, "TSX28") = False
                            Hit("F12")
                        Loop
                    End If
                Next row
                Hit("F8")
            Loop
        End If
        Do While Check4Text(1, 74, "TCX0D") = False And Check4Text(1, 72, "TCX06") = False And Check4Text(1, 72, "TCX0I") = False And Check4Text(1, 72, "TCX0C") = False
            Hit("F12")
        Loop

        For benefitsIndex As Integer = 0 To benefitsDataList.Count - 1
            For Each loanRate As LoanInterestRate In bsBorrower.StatutoryInterestRates
                If benefitsDataList(benefitsIndex).LoanSequence = loanRate.LnSeqText Then
                    benefitsDataList(benefitsIndex).StatutoryInterestRate = loanRate.InterestRateText
                End If
            Next loanRate
            'load ACH Part to array
            For achIndex As Integer = 0 To bsBorrower.ACHData.LnLvlInfo.GetUpperBound(1)
                benefitsDataList(benefitsIndex).HasAch = "No"
                If IsNumeric(bsBorrower.ACHData.LnLvlInfo(0, achIndex)) Then
                    If benefitsDataList(benefitsIndex).LoanSequence = Integer.Parse(bsBorrower.ACHData.LnLvlInfo(0, achIndex)).ToString("0000") Then
                        'load Stat rate to array
                        benefitsDataList(benefitsIndex).HasAch = "Yes"
                        Exit For
                    End If
                End If
            Next achIndex
        Next benefitsIndex

        'create Datagrid Table Style and columns
        Dim TS As New DataGridTableStyle()
        TS.GridLineColor = Me.BackColor
        TS.ReadOnly = True
        Dim column As New FlatColumn()
        With column
            .HeaderText = "Ln Seq"
            .MappingName = "C1"
            .Width = 55
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(column)

        Dim dateColumn As New FlatDateColumn()
        With dateColumn
            .HeaderText = "1st Disb Dt"
            .MappingName = "C2"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(dateColumn)

        column = New FlatColumn()
        With column
            .HeaderText = "Loan Type"
            .MappingName = "C3"
            .Width = 55
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "Orig. Balance"
            .MappingName = "C4"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "Current Principal"
            .MappingName = "C5"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "Current Rate"
            .MappingName = "C6"
            .Width = 110
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "Stat. Rate"
            .MappingName = "C7"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "ACH Part"
            .MappingName = "C8"
            .Width = 55
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "ACH Rate"
            .MappingName = "C9"
            .Width = 55
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "RIR Payments"
            .MappingName = "C10"
            .Width = 200
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "RIR Program"
            .MappingName = "C11"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)

        column = New FlatColumn()
        With column
            .HeaderText = "HEP"
            .MappingName = "C12"
            .Width = 40
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        TS.GridColumnStyles.Add(column)
        TS.RowHeadersVisible = False

        Dim tb As New DataTable()
        tb.Columns.Add("C0")
        tb.Columns.Add("C1")
        tb.Columns.Add("C2", GetType(Date))
        tb.Columns.Add("C3")
        tb.Columns.Add("C4")
        tb.Columns.Add("C5")
        tb.Columns.Add("C6")
        tb.Columns.Add("C7")
        tb.Columns.Add("C8")
        tb.Columns.Add("C9")
        tb.Columns.Add("C10")
        tb.Columns.Add("C11")
        tb.Columns.Add("C12")

        Dim arr(12) As String 'This array will hold the rows to add to the table that fills the datagrid
        For Each benefitsData As BorrowerBenefitData In benefitsDataList
            If benefitsData.LoanSequence.Length = 0 Then Continue For
            arr(1) = benefitsData.LoanSequence
            If IsDate(benefitsData.FirstDisbursementDate) Then
                arr(2) = benefitsData.FirstDisbursementDate
            Else
                arr(2) = Nothing
            End If
            arr(3) = benefitsData.LoanType
            arr(4) = benefitsData.BeginningBalance
            arr(5) = benefitsData.CurrentBalance
            arr(6) = benefitsData.InterestRate
            arr(7) = benefitsData.StatutoryInterestRate
            arr(8) = benefitsData.HasAch
            arr(9) = benefitsData.AchRate
            arr(10) = benefitsData.RirPayments
            arr(11) = benefitsData.RirRate
            arr(12) = benefitsData.Hep
            tb.Rows.Add(arr)
        Next benefitsData
        dgBB.TableStyles.Clear()
        dgBB.TableStyles.Add(TS)
        Dim DV As New DataView(tb)
        dgBB.DataSource = DV
        SP.Processing.Visible = False
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub

    Private Sub frmBorrowerBenefits_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        Dim gr As Graphics = Me.CreateGraphics()
        Dim brush As New Drawing2D.LinearGradientBrush(New PointF(0, 0), New PointF(Me.Height, Me.Width), Me.BackColor, Me.ForeColor)
        gr.FillRectangle(brush, 0, 0, Me.Width, Me.Height)
    End Sub
End Class


Public Class FlatColumn
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

        foreBrush = New SolidBrush(SP.UsrInf.FColor)
        backBrush = New SolidBrush(SP.UsrInf.BColor)

        If Not bPainted Then
            dg = Me.DataGridTableStyle.DataGrid
        End If

        'clear the cell 
        g.FillRectangle(backBrush, bounds)

        ' get the height column should be 
        Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
        Dim h As Integer = CType((sDraw.Height + 15), Integer)

        g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
        bPainted = True
    End Sub
End Class

Public Class FlatDateColumn
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

        foreBrush = New SolidBrush(SP.UsrInf.FColor)
        backBrush = New SolidBrush(SP.UsrInf.BColor)

        If Not bPainted Then
            dg = Me.DataGridTableStyle.DataGrid
        End If

        'clear the cell 
        g.FillRectangle(backBrush, bounds)

        ' get the height column should be 
        Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
        Dim h As Integer = CType((sDraw.Height + 15), Integer)

        If IsDate(s) Then s = CDate(s).ToShortDateString Else s = ""
        g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
        bPainted = True
    End Sub
End Class

Public Class FlatColumnWhite
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
        foreBrush = Brushes.Black
        backBrush = Brushes.WhiteSmoke

        If Not bPainted Then
            dg = Me.DataGridTableStyle.DataGrid
        End If

        'clear the cell 
        g.FillRectangle(backBrush, bounds)

        ' get the height column should be 
        Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
        Dim h As Integer = CType((sDraw.Height + 15), Integer)

        g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
        bPainted = True
    End Sub
End Class

Public Class FlatDateColumnWhite
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
        foreBrush = Brushes.Black
        backBrush = Brushes.WhiteSmoke

        If Not bPainted Then
            dg = Me.DataGridTableStyle.DataGrid
        End If

        'clear the cell 
        g.FillRectangle(backBrush, bounds)

        ' get the height column should be 
        Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
        Dim h As Integer = CType((sDraw.Height + 15), Integer)

        If IsDate(s) Then s = CDate(s).ToShortDateString Else s = ""
        g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
        bPainted = True
    End Sub
End Class
