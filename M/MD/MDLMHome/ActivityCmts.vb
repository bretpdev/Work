Imports System.Reflection
Imports System.Windows.Forms
Imports System.Drawing

Public Class ActivityCmts

    Private Bor As SP.Borrower
    Private Success As Boolean


    Public Enum DaysOrNumberOf
        Days = 0
        NumberOf = 1
    End Enum


    Public Function GetSuccess() As Boolean
        Return Success
    End Function

    Public Function SetupLP50History(ByVal DorN As DaysOrNumberOf, ByVal Count As Integer) As Boolean
        '1 Activity Date
        '2 Activity Type 
        '3 Contact Type
        '4 Action Code
        '5 Action Code Description
        '6 User ID
        '7 Activity Comment
        'This is the table that will fill the datagrid

        Dim StartDt As Date
        Dim CurrentCount As Integer = 0
        StartDt = Today.AddDays(Count * -1)

        SP.Q.FastPath("LP50I" & Bor.SSN)
        If Count = 0 Or DorN = DaysOrNumberOf.NumberOf Then
            'get all days
            SP.Q.PutText(5, 14, "X", True)
        Else
            SP.Q.PutText(18, 29, Format(StartDt, "MMddyyyy"))
            SP.Q.PutText(18, 41, Format(Today, "MMddyyyy"), True)
        End If
        If SP.Q.Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then
            'found records
            SP.Q.PutText(3, 13, "X", True)
        End If

        If SP.Q.Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY") Then
            'found records
            Dim AHTBL As New DataTable
            AHTBL.Columns.Add("Nothing")
            AHTBL.Columns.Add("Activity Date")
            AHTBL.Columns.Add("Activity Type")
            AHTBL.Columns.Add("Contact Type")
            AHTBL.Columns.Add("Action Code")
            AHTBL.Columns.Add("Action Code Description")
            AHTBL.Columns.Add("User ID")
            AHTBL.Columns.Add("Activity Comment")

            Dim arr(7) As String 'This array will hold the rows to add to the table that fills the datagrid
            Do While (SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False) And (DorN <> DaysOrNumberOf.NumberOf Or CurrentCount <> Count)
                CurrentCount = CurrentCount + 1
                arr(1) = SP.Q.GetText(7, 15, 8)
                arr(2) = SP.Q.GetText(7, 2, 2)
                arr(3) = SP.Q.GetText(7, 5, 2)
                arr(4) = SP.Q.GetText(8, 2, 5)
                arr(5) = SP.Q.GetText(7, 34, 23)
                arr(6) = SP.Q.GetText(8, 69, 7)
                arr(7) = SP.Q.GetText(13, 2, 78) & " " & _
                        SP.Q.GetText(14, 2, 78) & " " & _
                        SP.Q.GetText(15, 2, 78) & " " & _
                        SP.Q.GetText(16, 2, 78) & " " & _
                        SP.Q.GetText(17, 2, 78) & " " & _
                        SP.Q.GetText(18, 2, 78) & " " & _
                        SP.Q.GetText(19, 2, 78) & " " & _
                        SP.Q.GetText(20, 2, 78) & " " & _
                        SP.Q.GetText(21, 2, 78)
                AHTBL.Rows.Add(arr)
                SP.Q.Hit("F8")
            Loop
            dgAH.TableStyles.Clear()
            dgAH.TableStyles.Add(MakeTableStyleLP50)
            DV = New DataView(AHTBL)
            dgAH.DataSource = DV
            Return True
        Else
            'no records found
            Return False
        End If


    End Function

    Function MakeTableStyleLP50() As DataGridTableStyle
        Dim dgTableStyle As New DataGridTableStyle
        dgTableStyle.PreferredRowHeight = 12
        '1 Activity Date
        '2 Activity Type 
        '3 Contact Type
        '4 Action Code
        '5 Action Code Description
        '6 User ID
        '7 Activity Comment
        Dim column1 As New MultiLineColumn
        With column1
            .MappingName = "Activity Date"
            .HeaderText = "Activity Date"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column1)
        Dim column2 As New MultiLineColumn
        With column2
            .MappingName = "Activity Type"
            .HeaderText = "Activity Type"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column2)
        Dim column3 As New MultiLineColumn
        With column3
            .MappingName = "Contact Type"
            .HeaderText = "Contact Type"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column3)
        Dim column4 As New MultiLineColumn
        With column4
            .MappingName = "Action Code"
            .HeaderText = "Action Code"
            .Width = 80
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column4)
        Dim column5 As New MultiLineColumn
        With column5
            .MappingName = "Action Code Description"
            .HeaderText = "Action Code Description"
            .Width = 90
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column5)
        Dim column6 As New MultiLineColumn
        With column6
            .MappingName = "User ID"
            .HeaderText = "User ID"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column6)
        Dim column7 As New MultiLineColumnWhite
        With column7
            .MappingName = "Activity Comment"
            .HeaderText = "Activity Comment"
            .Width = 350
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column7)

        dgTableStyle.RowHeadersVisible = False
        dgTableStyle.ForeColor = Me.ForeColor
        dgTableStyle.BackColor = Me.BackColor
        dgTableStyle.AlternatingBackColor = Me.BackColor
        dgTableStyle.HeaderBackColor = Me.ForeColor
        dgTableStyle.HeaderForeColor = Me.BackColor
        dgTableStyle.GridLineColor = Me.ForeColor
        Return dgTableStyle
    End Function


    Public Class MultiLineColumn
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
    Public Class MultiLineColumnWhite
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
