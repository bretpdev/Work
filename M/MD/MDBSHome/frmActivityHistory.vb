Imports System.Collections.Generic
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports SP.Q

Public Class frmActivityHistory
    Public Enum FromScreen
        LP50
        TD2A
    End Enum

    Private _borrower As SP.Borrower

    Public Sub New(ByVal borrower As SP.Borrower)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        _borrower = borrower
    End Sub

    Public Overloads Function Show(ByVal numberOfDaysOfHistoryToFetch As Integer, ByVal title As String, ByVal from As FromScreen) As Boolean
        Me.Text = title
        lbltittle.Text = title
        dgAH.BackColor = Me.BackColor
        dgAH.ForeColor = Me.ForeColor

        If from = FromScreen.TD2A Then
            PopulateActivityHistoryDisplayFromTD2A(numberOfDaysOfHistoryToFetch)
            Me.Show()
        ElseIf from = FromScreen.LP50 Then
            If PopulateActivityHistoryDisplayFromLP50(numberOfDaysOfHistoryToFetch) Then
                Me.Show()
            Else
                Try
                    AppActivate("Maui DUDE Borrower Services HomePage")
                Catch ex As Exception
                    Try
                        AppActivate("Maui DUDE Loan Managment HomePage")
                    Catch ex2 As Exception
                    End Try
                End Try
            End If
        End If
    End Function

    Private Sub PopulateActivityHistoryDisplayFromTD2A(ByVal numberOfDaysOfHistoryToFetch As Integer)
        SP.Processing.Visible = True
        SP.Processing.Refresh()

        While Check4Text(1, 74, "TCX0D") = False AndAlso Check4Text(1, 72, "TCX06") = False AndAlso Check4Text(1, 72, "TCX0I") = False AndAlso Check4Text(1, 72, "TCX0C") = False
            SP.frmWhoaDUDE.WhoaDUDE("No way DUDE! You gota be on the ACP Screen TCX0D, TCX06, TCX0I, or TCX0C if you wanna keep surfing. Press 'OK' when you are there.", "Bad Screen", True)
        End While
        While Check4Text(24, 56, "F10=ATY") = False AndAlso Check4Text(24, 63, "F10=ATY") = False AndAlso Check4Text(24, 64, "F10=ATY") = False
            Hit("F2")
        End While
        Hit("F10")
        If Check4Text(1, 72, "TDX2B") Then
            Dim startDate As DateTime = DateTime.Now.AddDays(numberOfDaysOfHistoryToFetch * -1)
            PutText(21, 16, startDate.ToString("MMddyy"))
            PutText(21, 30, DateTime.Now.ToString("MMddyy"))
        End If
        Hit("ENTER")
        If Check4Text(1, 72, "TDX2C") Then
            PutText(5, 14, "X", True)
        End If

        'This is the table that will fill the datagrid
        Dim activityHistoryTable As New DataTable()
        activityHistoryTable.Columns.Add("Nothing")
        activityHistoryTable.Columns.Add("Request Code")
        activityHistoryTable.Columns.Add("Response Code")
        activityHistoryTable.Columns.Add("Request Description")
        activityHistoryTable.Columns.Add("Request Date")
        activityHistoryTable.Columns.Add("Response Date")
        activityHistoryTable.Columns.Add("Requestor")
        activityHistoryTable.Columns.Add("Performed")
        activityHistoryTable.Columns.Add("Text")

        If Check4Text(23, 2, "01019 ENTERED KEY NOT FOUND") = False Then
            'Get borrower activity records
            Do While Check4Text(23, 2, "90007") = False
                Dim columnValues(8) As String
                columnValues(1) = GetText(13, 2, 5)
                columnValues(2) = GetText(15, 2, 5)
                columnValues(3) = GetText(13, 8, 20)
                columnValues(4) = GetText(13, 31, 8)
                columnValues(5) = GetText(15, 31, 8)
                columnValues(6) = GetText(13, 40, 9)
                columnValues(7) = GetText(15, 51, 9)
                columnValues(8) = String.Format("{0} {1} {2}", GetText(17, 2, 78), GetText(18, 2, 78), GetText(19, 2, 78))
                activityHistoryTable.Rows.Add(columnValues)
                Hit("F8")
                If Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                    Hit("ENTER")
                    PutText(5, 14, "X", True)
                End If
            Loop
        End If
        While Check4Text(1, 74, "TCX0D") = False AndAlso Check4Text(1, 72, "TCX06") = False AndAlso Check4Text(1, 72, "TCX0I") = False AndAlso Check4Text(1, 72, "TCX0C") = False
            Hit("F12")
        End While

        dgAH.TableStyles.Clear()
        dgAH.TableStyles.Add(GetTableStyleForTD2A())
        dgAH.DataSource = New DataView(activityHistoryTable)

        SP.Processing.Visible = False
    End Sub

    Private Function PopulateActivityHistoryDisplayFromLP50(ByVal numberOfDaysOfHistoryToFetch As Integer) As Boolean
        SP.Processing.Visible = True
        SP.Processing.Refresh()

        FastPath("LP50I" + _borrower.SSN)
        If numberOfDaysOfHistoryToFetch = 0 Then
            'get all days
            PutText(5, 14, "X", True)
        Else
            Dim startDate As DateTime = DateTime.Now.AddDays(numberOfDaysOfHistoryToFetch * -1)
            PutText(18, 29, Format(startDate, "MMddyyyy"))
            PutText(18, 41, Format(Today, "MMddyyyy"), True)
        End If
        If Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then
            'found records
            PutText(3, 13, "X", True)
        End If

        If Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY") = False Then
            'no records found
            SP.frmWhoaDUDE.WhoaDUDE("It's a bum day for surfin'. This borrower has no history in the requested time frame.", "MauiDUDE")
            SP.Processing.Visible = False
            Return False
        End If

        'This is the table that will fill the datagrid
        Dim activityHistoryTable As New DataTable()
        activityHistoryTable.Columns.Add("Nothing")
        activityHistoryTable.Columns.Add("Action Code")
        activityHistoryTable.Columns.Add("Code Description")
        activityHistoryTable.Columns.Add("Activity Date")
        activityHistoryTable.Columns.Add("User ID")
        activityHistoryTable.Columns.Add("Activity Comment")

        Do While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            Dim columnValues(5) As String
            columnValues(1) = GetText(8, 2, 5)
            columnValues(2) = GetText(9, 15, 60)
            columnValues(3) = GetText(7, 15, 8)
            columnValues(4) = GetText(8, 69, 7)
            Dim commentLines As New List(Of String)()
            commentLines.Add(GetText(13, 2, 78))
            commentLines.Add(GetText(14, 2, 78))
            commentLines.Add(GetText(15, 2, 78))
            commentLines.Add(GetText(16, 2, 78))
            commentLines.Add(GetText(17, 2, 78))
            commentLines.Add(GetText(18, 2, 78))
            commentLines.Add(GetText(19, 2, 78))
            commentLines.Add(GetText(20, 2, 78))
            commentLines.Add(GetText(21, 2, 78))
            columnValues(5) = String.Join(" ", commentLines.ToArray())
            activityHistoryTable.Rows.Add(columnValues)
            Hit("F8")
        Loop
        dgAH.TableStyles.Clear()
        dgAH.TableStyles.Add(GetTableStyleForLP50())
        dgAH.DataSource = New DataView(activityHistoryTable)
        SP.Processing.Visible = False
        Return True
    End Function

    Private Function GetTableStyleForTD2A() As DataGridTableStyle
        Dim dgTableStyle As New DataGridTableStyle()
        dgTableStyle.PreferredRowHeight = 12

        Dim column As New MultiLineColumn()
        With column
            .MappingName = "Request Code"
            .HeaderText = "Request"
            .Width = 55
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Response Code"
            .HeaderText = "Response"
            .Width = 55
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Request Description"
            .HeaderText = "Request Description"
            .Width = 110
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Request Date"
            .HeaderText = "Req Date"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Response Date"
            .HeaderText = "Resp Date"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Requestor"
            .HeaderText = "Requestor"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Performed"
            .HeaderText = "Performed"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Text"
            .HeaderText = "Text"
            .Width = 338
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        dgTableStyle.RowHeadersVisible = False
        dgTableStyle.ForeColor = Me.ForeColor
        dgTableStyle.BackColor = Me.BackColor
        dgTableStyle.AlternatingBackColor = Me.BackColor
        dgTableStyle.HeaderBackColor = Me.ForeColor
        dgTableStyle.HeaderForeColor = Me.BackColor
        dgTableStyle.GridLineColor = Me.ForeColor
        Return dgTableStyle
    End Function

    Private Function GetTableStyleForLP50() As DataGridTableStyle
        Dim dgTableStyle As New DataGridTableStyle()
        dgTableStyle.PreferredRowHeight = 12

        Dim column As New MultiLineColumn()
        With column
            .MappingName = "Action Code"
            .HeaderText = "Action Code"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Code Description"
            .HeaderText = "Code Description"
            .Width = 110
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Activity Date"
            .HeaderText = "Activity Date"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "User ID"
            .HeaderText = "User ID"
            .Width = 80
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        column = New MultiLineColumn()
        With column
            .MappingName = "Activity Comment"
            .HeaderText = "Activity Comment"
            .Width = 350
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column)

        dgTableStyle.RowHeadersVisible = False
        dgTableStyle.ForeColor = Me.ForeColor
        dgTableStyle.BackColor = Me.BackColor
        dgTableStyle.AlternatingBackColor = Me.BackColor
        dgTableStyle.HeaderBackColor = Me.ForeColor
        dgTableStyle.HeaderForeColor = Me.BackColor
        dgTableStyle.GridLineColor = Me.ForeColor
        Return dgTableStyle
    End Function

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub DV_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles DV.ListChanged
        dgAH.TableStyles.Clear()
        If lbltittle.Text.Contains("TD2A") Then
            dgAH.TableStyles.Add(GetTableStyleForTD2A())
        ElseIf lbltittle.Text.Contains("LP50") Then
            dgAH.TableStyles.Add(GetTableStyleForLP50())
        End If
    End Sub

    Private Sub frmActivityHistory_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        dgAH.Width = Me.Width - 28
        dgAH.Height = Me.Height - 120
    End Sub

    Private Class MultiLineColumn
        Inherits DataGridTextBoxColumn

        Private mTxtAlign As HorizontalAlignment
        Private mDrawTxt As New StringFormat
        Private mbAdjustHeight As Boolean = True
        Private m_intPreEditHeight As Integer
        Private m_rownum As Integer
        Dim WithEvents dg As DataGrid
        Private arHeights As ArrayList

        Private Sub GetHeightList()
            Dim mi As MethodInfo = dg.GetType().GetMethod("get_DataGridRows", _
            BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase Or _
            BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or _
            BindingFlags.Static)
            Dim dgra As Array = CType(mi.Invoke(Me.dg, Nothing), Array)
            arHeights = New ArrayList
            Dim dgRowHeight As Object
            For Each dgRowHeight In dgra
                If dgRowHeight.ToString().EndsWith("DataGridRelationshipRow") = True Then
                    arHeights.Add(dgRowHeight)
                End If
            Next
        End Sub

        Public Sub New()
            mTxtAlign = HorizontalAlignment.Left
            mDrawTxt.Alignment = StringAlignment.Near
            Me.ReadOnly = True
        End Sub

        Protected Overloads Overrides Sub Edit(ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal bounds _
    As System.Drawing.Rectangle, ByVal [readOnly] As Boolean, ByVal instantText _
    As String, ByVal cellIsVisible As Boolean)
            MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
            Me.TextBox.TextAlign = mTxtAlign
            Me.TextBox.Multiline = mbAdjustHeight
            If rowNum = source.Count - 1 Then
                GetHeightList()
            End If
        End Sub

        Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
            Static bPainted As Boolean = False

            If Not bPainted Then
                dg = Me.DataGridTableStyle.DataGrid
                GetHeightList()
            End If
            'clear the cell 
            g.FillRectangle(backBrush, bounds)
            'draw the value 
            Dim s As String = Me.GetColumnValueAtRow([source], rowNum).ToString()
            Dim r As New RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height)
            r.Inflate(0, -1)
            ' get the height column should be 
            Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
            Dim h As Integer = sDraw.Height + 15
            If mbAdjustHeight Then
                Try
                    Dim pi As PropertyInfo = arHeights(rowNum).GetType().GetProperty("Height")
                    ' get current height 
                    Dim curHeight As Integer = pi.GetValue(arHeights(rowNum), Nothing)
                    ' adjust height 
                    If h > curHeight Then
                        pi.SetValue(arHeights(rowNum), h, Nothing)
                        Dim sz As Size = dg.Size
                        dg.Size = New Size(sz.Width - 1, sz.Height - 1)
                        dg.Size = sz
                    End If
                Catch
                    ' something wrong leave default height 
                    GetHeightList()
                End Try
            End If
            g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
            bPainted = True

        End Sub

        Private Property DataAlignment() As HorizontalAlignment
            Get
                Return mTxtAlign
            End Get
            Set(ByVal Value As HorizontalAlignment)
                mTxtAlign = Value
                If mTxtAlign = HorizontalAlignment.Center Then
                    mDrawTxt.Alignment = StringAlignment.Center
                ElseIf mTxtAlign = HorizontalAlignment.Right Then
                    mDrawTxt.Alignment = StringAlignment.Far
                Else
                    mDrawTxt.Alignment = StringAlignment.Near
                End If
            End Set
        End Property

        Private Property AutoAdjustHeight() As Boolean
            Get
                Return mbAdjustHeight
            End Get
            Set(ByVal Value As Boolean)
                mbAdjustHeight = Value
                Try
                    dg.Invalidate()
                Catch
                End Try
            End Set
        End Property
    End Class

    Private Class MultiLineColumnWhite
        Inherits DataGridTextBoxColumn

        Private mTxtAlign As HorizontalAlignment
        Private mDrawTxt As New StringFormat
        Private mbAdjustHeight As Boolean = True
        Private m_intPreEditHeight As Integer
        Private m_rownum As Integer
        Dim WithEvents dg As DataGrid
        Private arHeights As ArrayList

        Private Sub GetHeightList()
            Dim mi As MethodInfo = dg.GetType().GetMethod("get_DataGridRows", _
            BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase Or _
            BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or _
            BindingFlags.Static)
            Dim dgra As Array = CType(mi.Invoke(Me.dg, Nothing), Array)
            arHeights = New ArrayList
            Dim dgRowHeight As Object
            For Each dgRowHeight In dgra
                If dgRowHeight.ToString().EndsWith("DataGridRelationshipRow") = True Then
                    arHeights.Add(dgRowHeight)
                End If
            Next
        End Sub

        Public Sub New()
            mTxtAlign = HorizontalAlignment.Left
            mDrawTxt.Alignment = StringAlignment.Near
            Me.ReadOnly = True
        End Sub

        Protected Overloads Overrides Sub Edit(ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal bounds _
    As System.Drawing.Rectangle, ByVal [readOnly] As Boolean, ByVal instantText _
    As String, ByVal cellIsVisible As Boolean)
            MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
            Me.TextBox.TextAlign = mTxtAlign
            Me.TextBox.Multiline = mbAdjustHeight
            If rowNum = source.Count - 1 Then
                GetHeightList()
            End If
        End Sub

        Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
            Static bPainted As Boolean = False
            foreBrush = Brushes.Black
            backBrush = Brushes.WhiteSmoke
            If Not bPainted Then
                dg = Me.DataGridTableStyle.DataGrid
                GetHeightList()
            End If
            'clear the cell 
            g.FillRectangle(backBrush, bounds)
            'draw the value 
            Dim s As String = Me.GetColumnValueAtRow([source], rowNum).ToString()
            Dim r As New RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height)
            r.Inflate(0, -1)
            ' get the height column should be 
            Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
            Dim h As Integer = sDraw.Height + 15
            If mbAdjustHeight Then
                Try
                    Dim pi As PropertyInfo = arHeights(rowNum).GetType().GetProperty("Height")
                    ' get current height 
                    Dim curHeight As Integer = pi.GetValue(arHeights(rowNum), Nothing)
                    ' adjust height 
                    If h > curHeight Then
                        pi.SetValue(arHeights(rowNum), h, Nothing)
                        Dim sz As Size = dg.Size
                        dg.Size = New Size(sz.Width - 1, sz.Height - 1)
                        dg.Size = sz
                    End If
                Catch
                    ' something wrong leave default height 
                    GetHeightList()
                End Try
            End If

            g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
            bPainted = True

        End Sub

        Private Property DataAlignment() As HorizontalAlignment
            Get
                Return mTxtAlign
            End Get
            Set(ByVal Value As HorizontalAlignment)
                mTxtAlign = Value
                If mTxtAlign = HorizontalAlignment.Center Then
                    mDrawTxt.Alignment = StringAlignment.Center
                ElseIf mTxtAlign = HorizontalAlignment.Right Then
                    mDrawTxt.Alignment = StringAlignment.Far
                Else
                    mDrawTxt.Alignment = StringAlignment.Near
                End If
            End Set
        End Property

        Private Property AutoAdjustHeight() As Boolean
            Get
                Return mbAdjustHeight
            End Get
            Set(ByVal Value As Boolean)
                mbAdjustHeight = Value
                Try
                    dg.Invalidate()
                Catch
                End Try
            End Set
        End Property
    End Class
End Class
