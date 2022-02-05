Imports System.Drawing
Imports System.Windows.Forms
Public Class frmLoanDetail
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "
    Public Bor As SP.Borrower
    Public Sub New(ByRef B As SP.Borrower)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Bor = B
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
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents dgLoanDetail As System.Windows.Forms.DataGrid
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmLoanDetail))
        Me.btnClose = New System.Windows.Forms.Button
        Me.dgLoanDetail = New System.Windows.Forms.DataGrid
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        CType(Me.dgLoanDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(472, 320)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Close"
        '
        'dgLoanDetail
        '
        Me.dgLoanDetail.CaptionVisible = False
        Me.dgLoanDetail.DataMember = ""
        Me.dgLoanDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgLoanDetail.Location = New System.Drawing.Point(8, 104)
        Me.dgLoanDetail.Name = "dgLoanDetail"
        Me.dgLoanDetail.ReadOnly = True
        Me.dgLoanDetail.RowHeadersVisible = False
        Me.dgLoanDetail.Size = New System.Drawing.Size(936, 208)
        Me.dgLoanDetail.TabIndex = 7
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1016, 80)
        Me.PictureBox1.TabIndex = 8
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(392, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(216, 16)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "This information was gathered from LC05"
        '
        'frmLoanDetail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(952, 350)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgLoanDetail)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(960, 384)
        Me.MinimumSize = New System.Drawing.Size(960, 384)
        Me.Name = "frmLoanDetail"
        Me.ShowInTaskbar = False
        Me.Text = "Maui DUDE Loan Detail"
        CType(Me.dgLoanDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub frmLoanDetail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
        Dim DT As New DataTable
        dgLoanDetail.BackColor = Me.BackColor
        dgLoanDetail.AlternatingBackColor = Me.BackColor
        dgLoanDetail.ForeColor = Me.ForeColor

        DT.Columns.Add("Payoff Dt", GetType(Date))
        DT.Columns.Add("Amt Def", GetType(Double))
        DT.Columns.Add("Amt Coll", GetType(Double))
        DT.Columns.Add("Int Accrued", GetType(Double))
        DT.Columns.Add("Other Accrued", GetType(Double))
        DT.Columns.Add("Other Coll", GetType(Double))
        DT.Columns.Add("Tot CC", GetType(Double))
        DT.Columns.Add("Remaining CC", GetType(Double))
        DT.Columns.Add("Outstanding", GetType(Double))
        DT.Columns.Add("Subrgtn Cd", GetType(String))
        DT.Columns.Add("Subrgtn Dt", GetType(Date))
        DT.Columns.Add("Next Pmt Due", GetType(Date))
        DT.Columns.Add("Endorser", GetType(String))
        DT.Columns.Add("Def Type", GetType(String))
        Dim R As DataRow
        Dim arr(13) As String
        For Each R In CType(Bor, BorrowerLM).LC05Detail.Rows
            arr(0) = CStr(R.Item("LenderPayoffDate")).Insert(2, "/").Insert(5, "/") 'LENDER PAYOFF DATE
            arr(1) = R.Item("TotalAmountDefaulted") 'TOTAL AMOUNT DEF
            arr(2) = CDbl(R.Item("PrincipalCollected")) + CDbl(R.Item("InterestCollected")) 'TOTAL AMOUNT COLLECTED
            arr(3) = R.Item("InterestDefaulted") 'INTEREST ACCRUED
            arr(4) = CDbl(R.Item("OtherChargesAccrued")) + CDbl(R.Item("LegalCostsAccrued")) 'OTHER ACCRUED
            arr(5) = CDbl(R.Item("OtherChargesCollected")) + CDbl(R.Item("LegalCostsCollected")) 'OTHER COLLECTED
            arr(6) = CDbl(R.Item("CollectionCostsProjected")) + CDbl(R.Item("CollectionCostsAccrued")) 'total collection cost
            arr(7) = (CDbl(R.Item("CollectionCostsProjected")) + CDbl(R.Item("CollectionCostsAccrued")) - CDbl(R.Item("CollectionCostsCollected"))) 'REMAINING COLLECTION COST
            arr(8) = R.Item("OutstandingBalance") 'OUTSTANDING BALANCE
            arr(9) = R.Item("SubrogationCode") 'SUBROGATION CODE
            arr(10) = CStr(R.Item("SubrogationDate")).Insert(2, "/").Insert(5, "/") 'SUBROGATION DATE
            arr(11) = CStr(R.Item("NextPaymentDue")).Insert(2, "/").Insert(5, "/") 'NEXT PAYMENT DUE
            arr(12) = R.Item("Endorser") 'ENDORSER
            arr(13) = R.Item("DefType") 'DEF TYPE

            arr(1) = FormatNumber(arr(1), 2)
            arr(2) = FormatNumber(arr(2), 2)
            arr(3) = FormatNumber(arr(3), 2)
            arr(4) = FormatNumber(arr(4), 2)
            arr(5) = FormatNumber(arr(5), 2)
            arr(6) = FormatNumber(arr(6), 2)
            arr(7) = FormatNumber(arr(7), 2)
            arr(8) = FormatNumber(arr(8), 2)

            If IsDate(arr(0)) Then arr(0) = Format(CDate(arr(0)), "d") Else arr(0) = Nothing
            If IsDate(arr(10)) Then arr(10) = Format(CDate(arr(10)), "d") Else arr(10) = Nothing
            If IsDate(arr(11)) Then arr(11) = Format(CDate(arr(11)), "d") Else arr(11) = Nothing

            DT.Rows.Add(arr)
        Next
        If DT.Rows.Count > 0 Then
            dgLoanDetail.DataSource = DT
        End If
        MakeTableStyle()
    End Sub

    Sub MakeTableStyle()
        dgLoanDetail.TableStyles.Clear()
        'create Datagrid Table style
        Dim TS As New DataGridTableStyle
        TS.GridLineColor = Me.BackColor
        Dim C1 As New FlatDateColumn
        With C1
            .HeaderText = "Payoff Dt"
            .MappingName = "Payoff Dt"
            .Width = 70
            .TextBox.ForeColor = Color.Black
            .TextBox.BackColor = Color.White
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C1)

        Dim C2 As New FlatColumnWhite
        With C2
            .HeaderText = "Amt Def"
            .MappingName = "Amt Def"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C2)

        Dim C3 As New FlatColumn
        With C3
            .HeaderText = "Amt Coll"
            .MappingName = "Amt Coll"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C3)

        Dim C4 As New FlatColumnWhite
        With C4
            .HeaderText = "Int Accrued"
            .MappingName = "Int Accrued"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C4)

        Dim C5 As New FlatColumn
        With C5
            .HeaderText = "Other Accrued"
            .MappingName = "Other Accrued"
            .Width = 60
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C5)

        Dim C6 As New FlatColumnWhite
        With C6
            .HeaderText = "Other Coll"
            .MappingName = "Other Coll"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C6)

        Dim C7 As New FlatColumn
        With C7
            .HeaderText = "Tot CC"
            .MappingName = "Tot CC"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C7)

        Dim C8 As New FlatColumnWhite
        With C8
            .HeaderText = "Remaining CC"
            .MappingName = "Remaining CC"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C8)

        Dim C9 As New FlatColumn
        With C9
            .HeaderText = "Outstanding"
            .MappingName = "Outstanding"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C9)

        Dim C10 As New FlatColumnWhite
        With C10
            .HeaderText = "Subrgtn Cd"
            .MappingName = "Subrgtn Cd"
            .Width = 25
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C10)

        Dim C11 As New FlatDateColumn
        With C11
            .HeaderText = "Subrgtn Dt"
            .MappingName = "Subrgtn Dt"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
            .NullText = "NULL"
        End With
        TS.GridColumnStyles.Add(C11)

        Dim C12 As New FlatDateColumnWhite
        With C12
            .HeaderText = "Next Pmt Due"
            .MappingName = "Next Pmt Due"
            .Width = 70
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C12)

        Dim C13 As New FlatColumn
        With C13
            .HeaderText = "Endorser"
            .MappingName = "Endorser"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C13)

        Dim C14 As New FlatColumnWhite
        With C14
            .HeaderText = "Def Type"
            .MappingName = "Def Type"
            .Width = 65
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        TS.GridColumnStyles.Add(C14)

        TS.RowHeadersVisible = False
        dgLoanDetail.TableStyles.Add(TS)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
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

