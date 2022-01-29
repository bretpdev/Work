Imports System.Reflection
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmActivityHistory
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

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
    Friend WithEvents lbltittle As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents dgAH As System.Windows.Forms.DataGrid
    Friend WithEvents DV As System.Data.DataView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmActivityHistory))
        Me.lbltittle = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.dgAH = New System.Windows.Forms.DataGrid
        Me.DV = New System.Data.DataView
        CType(Me.dgAH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbltittle
        '
        Me.lbltittle.Font = New System.Drawing.Font("Bodoni MT Black", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbltittle.Location = New System.Drawing.Point(48, 8)
        Me.lbltittle.Name = "lbltittle"
        Me.lbltittle.Size = New System.Drawing.Size(752, 23)
        Me.lbltittle.TabIndex = 0
        Me.lbltittle.Text = "Borrower Activity History"
        Me.lbltittle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnClose.Location = New System.Drawing.Point(384, 360)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        '
        'dgAH
        '
        Me.dgAH.CaptionVisible = False
        Me.dgAH.DataMember = ""
        Me.dgAH.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.dgAH.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgAH.Location = New System.Drawing.Point(16, 40)
        Me.dgAH.Name = "dgAH"
        Me.dgAH.ReadOnly = True
        Me.dgAH.RowHeadersVisible = False
        Me.dgAH.Size = New System.Drawing.Size(808, 312)
        Me.dgAH.TabIndex = 3
        '
        'DV
        '
        '
        'frmActivityHistory
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(840, 390)
        Me.Controls.Add(Me.dgAH)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lbltittle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmActivityHistory"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE Borrower 30 Day Activity History"
        CType(Me.dgAH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private ID As String

    Public Overloads Function Show(ByVal IDt As String, ByVal Tittle As String, ByVal from As String) As Boolean
        ID = IDt
        Me.Text = Tittle
        lbltittle.Text = Tittle
        '****Me.ForeColor = SP.GeneralForeColor
        '****Me.BackColor = SP.GeneralBackColor
        '****Me.Opacity = SP.GeneralTransparency
        dgAH.BackColor = Me.BackColor
        dgAH.ForeColor = Me.ForeColor

        If from = "TD2A" Then
            If SetupTD2AHistory(365) Then
                Me.Show()
            Else
                Try
                    AppActivate("Maui DUDE Auxiliary Services HomePage")
                Catch ex As Exception
                End Try
            End If
        ElseIf from = "LP50" Then
            If SetupLP50History(365) Then
                Me.Show()
            Else
                Try
                    AppActivate("Maui DUDE Auxiliary Services HomePage")
                Catch ex As Exception
                End Try
            End If
        End If
    End Function

    Function SetupTD2AHistory(ByVal days As Integer) As Boolean
        Dim x As Integer
        '1 Request Code
        '2 Response Code 
        '3 Request Description 
        '4 Request Date  
        '5 Response Date  
        '6 Requestor 
        '7 Performed 
        '8 Text, all rows

        Dim StartDt As Date
        StartDt = Today.AddDays(days * -1)
        SP.Processing.Visible = True
        SP.Processing.Refresh()

        'While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
        '    SP.WhoaDude.WhoaDUDE("No way DUDE! You gota be on the ACP Screen TCX0D, TCX06 or TCX0I, if you wana keep surfing. Press 'OK' when you are there.", "Bad Screen", True)
        'End While
        'While SP.Q.Check4Text(24, 56, "F10=ATY") = False And SP.Q.Check4Text(24, 63, "F10=ATY") = False And SP.Q.Check4Text(24, 64, "F10=ATY") = False
        '    SP.Q.Hit("F2")
        'End While
        'SP.Q.Hit("F10")
        SP.Q.FastPath("TX3Z/ITD2A" & ID)
        If SP.Q.Check4Text(1, 72, "TDX2B") Then
            SP.Q.PutText(21, 16, Format(StartDt, "MM"))
            SP.Q.PutText(21, 19, Format(StartDt, "dd"))
            SP.Q.PutText(21, 22, Format(StartDt, "yy"))

            SP.Q.PutText(21, 30, Format(Today, "MM"))
            SP.Q.PutText(21, 33, Format(Today, "dd"))
            SP.Q.PutText(21, 36, Format(Today, "yy"))
        End If
        SP.Q.Hit("ENTER")
        If SP.Q.Check4Text(1, 72, "TDX2C") Then
            SP.Q.PutText(5, 14, "X", True)
        End If

        'This is the table that will fill the datagrid
        Dim AHTBL As New DataTable
        AHTBL.Columns.Add("Nothing")
        AHTBL.Columns.Add("Request Code")
        AHTBL.Columns.Add("Response Code")
        AHTBL.Columns.Add("Request Description")
        AHTBL.Columns.Add("Request Date")
        AHTBL.Columns.Add("Response Date")
        AHTBL.Columns.Add("Requestor")
        AHTBL.Columns.Add("Performed")
        AHTBL.Columns.Add("Text")
        Dim arr(8) As String 'This array will hold the rows to add to the table that fills the datagrid

        If SP.Q.Check4Text(23, 2, "01019 ENTERED KEY NOT FOUND") = False Then
            'Get borrower activity records
            x = 0
            Do While SP.Q.Check4Text(23, 2, "90007") = False
                x += 1
                'ReDim Preserve ActivityRecs(8, x)

                arr(1) = SP.Q.GetText(13, 2, 5)
                arr(2) = SP.Q.GetText(15, 2, 5)
                arr(3) = SP.Q.GetText(13, 8, 20)
                arr(4) = SP.Q.GetText(13, 31, 8)
                arr(5) = SP.Q.GetText(15, 31, 8)
                arr(6) = SP.Q.GetText(13, 40, 9)
                arr(7) = SP.Q.GetText(15, 51, 9)
                arr(8) = SP.Q.GetText(17, 2, 78) & " " & SP.Q.GetText(18, 2, 78) & " " & SP.Q.GetText(19, 2, 78)
                AHTBL.Rows.Add(arr)
                SP.Q.Hit("F8")
                If SP.Q.Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA") Then
                    SP.Q.Hit("ENTER")
                    SP.Q.PutText(5, 14, "X", True)
                End If
            Loop
        End If
        'While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
        '    SP.Q.Hit("F12")
        'End While


        dgAH.TableStyles.Clear()
        dgAH.TableStyles.Add(MakeTableStyle)

        DV = New DataView(AHTBL)
        dgAH.DataSource = DV

        'no records found
        If AHTBL.Rows.Count = 0 Then
            SP.frmWhoaDUDE.WhoaDUDE("It's a bum day for surfin'. This borrower has no history in the requested time frame.", "MauiDUDE")
            SP.Processing.Visible = False
            Return False
        End If

        SP.Processing.Visible = False
        Return True
    End Function

    Function SetupLP50History(ByVal days As Integer) As Boolean
        '1 Activity Date
        '2 Activity Type 
        '3 Contact Type
        '4 Action Code
        '5 Action Code Description
        '6 User ID
        '7 Activity Comment
        'This is the table that will fill the datagrid

        Dim StartDt As Date
        StartDt = Today.AddDays(days * -1)
        SP.Processing.Visible = True
        SP.Processing.Refresh()

        SP.Q.FastPath("LP50I" & ID)
        If days = 0 Then
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
            Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
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
            SP.Processing.Visible = False
            Return True
        Else
            'no records found
            SP.frmWhoaDUDE.WhoaDUDE("It's a bum day for surfin'. This borrower has no history in the requested time frame.", "MauiDUDE")
            SP.Processing.Visible = False
            Return False
        End If


        SP.Processing.Visible = False
    End Function

    Function MakeTableStyle() As DataGridTableStyle
        Dim dgTableStyle As New DataGridTableStyle
        dgTableStyle.PreferredRowHeight = 12

        Dim column1 As New MultiLineColumn
        With column1
            .MappingName = "Request Code"
            .HeaderText = "Request"
            .Width = 55
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column1)
        Dim column2 As New MultiLineColumn
        With column2
            .MappingName = "Response Code"
            .HeaderText = "Response"
            .Width = 55
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column2)
        Dim column3 As New MultiLineColumn
        With column3
            .MappingName = "Request Description"
            .HeaderText = "Request Description"
            .Width = 110
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column3)
        Dim column4 As New MultiLineColumn
        With column4
            .MappingName = "Request Date"
            .HeaderText = "Req Date"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column4)
        Dim column5 As New MultiLineColumn
        With column5
            .MappingName = "Response Date"
            .HeaderText = "Resp Date"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column5)
        Dim column6 As New MultiLineColumn
        With column6
            .MappingName = "Requestor"
            .HeaderText = "Requestor"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column6)
        Dim column7 As New MultiLineColumn
        With column7
            .MappingName = "Performed"
            .HeaderText = "Performed"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column7)
        Dim column8 As New MultiLineColumnWhite
        With column8
            .MappingName = "Text"
            .HeaderText = "Text"
            .Width = 338
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
        End With
        dgTableStyle.GridColumnStyles.Add(column8)
        dgTableStyle.RowHeadersVisible = False
        dgTableStyle.ForeColor = Me.ForeColor
        dgTableStyle.BackColor = Me.BackColor
        dgTableStyle.AlternatingBackColor = Me.BackColor
        dgTableStyle.HeaderBackColor = Me.ForeColor
        dgTableStyle.HeaderForeColor = Me.BackColor
        dgTableStyle.GridLineColor = Me.ForeColor
        Return dgTableStyle
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


    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub DV_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles DV.ListChanged
        dgAH.TableStyles.Clear()
        If lbltittle.Text.IndexOf("TD2A") <> -1 Then
            dgAH.TableStyles.Add(MakeTableStyle)
        ElseIf lbltittle.Text.IndexOf("LP50") <> -1 Then
            dgAH.TableStyles.Add(MakeTableStyleLP50)
        End If
    End Sub

    Private Sub frmActivityHistory_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        dgAH.Width = Me.Width - 28
        dgAH.Height = Me.Height - 120
    End Sub

End Class


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