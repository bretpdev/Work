Imports System.Windows.Forms
Public Class frmCompassLoans
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
        dgLnHist.BackColor = Me.BackColor
        dgLnHist.ForeColor = Me.ForeColor
        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.Hide()
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
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents IL As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgLnHist As System.Windows.Forms.DataGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmCompassLoans))
        Me.IL = New System.Windows.Forms.ImageList(Me.components)
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.dgLnHist = New System.Windows.Forms.DataGrid
        CType(Me.dgLnHist, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'IL
        '
        Me.IL.ImageSize = New System.Drawing.Size(16, 16)
        Me.IL.ImageStream = CType(resources.GetObject("IL.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IL.TransparentColor = System.Drawing.Color.Transparent
        '
        'btnClose
        '
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(472, 208)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(1000, 23)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "COMPASS Loans"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgLnHist
        '
        Me.dgLnHist.CaptionVisible = False
        Me.dgLnHist.DataMember = ""
        Me.dgLnHist.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgLnHist.Location = New System.Drawing.Point(8, 40)
        Me.dgLnHist.Name = "dgLnHist"
        Me.dgLnHist.ReadOnly = True
        Me.dgLnHist.Size = New System.Drawing.Size(1000, 160)
        Me.dgLnHist.TabIndex = 6
        '
        'frmCompassLoans
        '
        Me.AcceptButton = Me.btnClose
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1020, 238)
        Me.Controls.Add(Me.dgLnHist)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1028, 272)
        Me.MinimumSize = New System.Drawing.Size(1028, 272)
        Me.Name = "frmCompassLoans"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "COMPASS Loan History"
        CType(Me.dgLnHist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Overloads Sub Show(ByVal DD As String)
        Dim SelectionCounter As Integer
        Dim arr(16) As String
        Dim TB As New DataTable
        Dim TS As New DataGridTableStyle
        Dim DV As New DataView


        SP.Processing.Visible = True
        SP.Processing.Refresh()
        'only collect the data if it hasn't be collected yet
        If dgLnHist.DataSource Is Nothing Then
            'add columns to table
            TB.Columns.Add("C0")
            TB.Columns.Add("C1")
            TB.Columns.Add("C2")
            TB.Columns.Add("C3", GetType(Date))
            TB.Columns.Add("C4")
            TB.Columns.Add("C5")
            TB.Columns.Add("C6")
            TB.Columns.Add("C7")
            TB.Columns.Add("C8")
            TB.Columns.Add("C9")
            TB.Columns.Add("C10")
            TB.Columns.Add("C11")
            TB.Columns.Add("C12")
            TB.Columns.Add("C13")
            TB.Columns.Add("C14")
            TB.Columns.Add("C15")
            TB.Columns.Add("C16")
            'switch keys until right option is displayed
            If SP.Q.Check4Text(1, 74, "TCX0D") Or SP.Q.Check4Text(1, 72, "TCX06") Then
                While SP.Q.Check4Text(24, 52, "LOAN") = False
                    SP.Q.Hit("F2")
                End While
            ElseIf SP.Q.Check4Text(1, 72, "TCX0I") Then
                While SP.Q.Check4Text(24, 58, "LOAN") = False
                    SP.Q.Hit("F2")
                End While
            ElseIf SP.Q.Check4Text(1, 72, "TCX0C") Then
                While SP.Q.Check4Text(24, 50, "LOAN") = False
                    SP.Q.Hit("F2")
                End While
            End If
            'access TS26
            SP.Q.Hit("F8")
            If SP.Q.Check4Text(1, 72, "TSX28") Then
                'selection screen
                SelectionCounter = 1
                While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    If SelectionCounter > 9 Then
                        SP.Q.PutText(21, 12, CStr(SelectionCounter), True)
                    Else
                        SP.Q.PutText(21, 12, "0" & CStr(SelectionCounter), True)
                    End If
                    If SP.Q.Check4Text(23, 2, "01032 SELECTION MUST CORRESPOND TO A DISPLAYED KEY") Then
                        'if there are no other loans to select
                        SelectionCounter = 1
                        SP.Q.Hit("F8") 'try and page forward to the next set of loans
                    Else

                        arr(1) = SP.Q.GetText(7, 35, 4)
                        If SP.Q.GetText(6, 66, 6) = "STFFRD" Then
                            arr(2) = "Sub"
                        ElseIf SP.Q.GetText(6, 66, 6) = "UNSTFD" Then
                            arr(2) = "Unsub"
                        ElseIf SP.Q.GetText(6, 66, 6) = "SLS" Then
                            arr(2) = "SLS"
                        ElseIf SP.Q.GetText(6, 66, 6) = "PLUS" Then
                            arr(2) = "PLUS"
                        ElseIf SP.Q.GetText(6, 66, 6) = "UNSPC" Or SP.Q.GetText(6, 66, 6) = "SUBSPC" Then
                            arr(2) = "Spousal"
                        ElseIf SP.Q.GetText(6, 66, 6) = "SUBCNS" Or SP.Q.GetText(6, 66, 6) = "UNCNS" Then
                            arr(2) = "Consol"
                        End If
                        arr(3) = SP.Q.GetText(6, 18, 8)
                        arr(4) = SP.Q.GetText(11, 36, 10)
                        arr(5) = SP.Q.GetText(11, 12, 12)
                        arr(6) = SP.Q.GetText(11, 58, 6)
                        arr(7) = SP.Q.GetText(18, 42, 8)
                        arr(8) = SP.Q.GetText(3, 10, 28)
                        arr(9) = SP.Q.GetText(18, 17, 8)
                        arr(10) = SP.Q.GetText(16, 45, 8)
                        arr(11) = SP.Q.GetText(17, 44, 8)
                        arr(12) = SP.Q.GetText(19, 71, 8)
                        arr(13) = SP.Q.GetText(13, 29, 19)
                        SP.Q.Hit("Enter")
                        SP.Q.Hit("Enter")
                        arr(14) = SP.Q.GetText(7, 20, 10)
                        SP.Q.Hit("F12")
                        SP.Q.Hit("F12")
                        arr(15) = SP.Q.GetText(20, 43, 4)
                        SP.Q.Hit("F12")
                        arr(16) = DD
                        SelectionCounter += 1

                        If IsDate(arr(3)) Then arr(3) = arr(3) Else arr(3) = Nothing
                        If IsDate(arr(9)) Then arr(9) = arr(9) Else arr(9) = Nothing
                        If IsDate(arr(10)) Then arr(10) = arr(10) Else arr(10) = Nothing
                        If IsDate(arr(11)) Then arr(11) = arr(11) Else arr(11) = Nothing
                        If IsDate(arr(12)) Then arr(12) = arr(12) Else arr(12) = Nothing
                        'add row to table
                        TB.Rows.Add(arr)
                    End If
                End While
            Else
                'target screen
                'gather information
                arr(1) = SP.Q.GetText(7, 35, 4)
                If SP.Q.GetText(6, 66, 6) = "STFFRD" Then
                    arr(2) = "Sub"
                ElseIf SP.Q.GetText(6, 66, 6) = "UNSTFD" Then
                    arr(2) = "Unsub"
                ElseIf SP.Q.GetText(6, 66, 6) = "SLS" Then
                    arr(2) = "SLS"
                ElseIf SP.Q.GetText(6, 66, 6) = "PLUS" Then
                    arr(2) = "PLUS"
                ElseIf SP.Q.GetText(6, 66, 6) = "UNSPC" Or SP.Q.GetText(6, 66, 6) = "SUBSPC" Then
                    arr(2) = "Spousal"
                ElseIf SP.Q.GetText(6, 66, 6) = "SUBCNS" Or SP.Q.GetText(6, 66, 6) = "UNCNS" Then
                    arr(2) = "Consol"
                End If
                arr(3) = SP.Q.GetText(6, 18, 8)
                arr(4) = SP.Q.GetText(11, 36, 10)
                arr(5) = SP.Q.GetText(11, 12, 12)
                arr(6) = SP.Q.GetText(11, 58, 6)
                arr(7) = SP.Q.GetText(18, 42, 5)
                arr(8) = SP.Q.GetText(3, 10, 28)
                arr(9) = SP.Q.GetText(18, 17, 8)
                arr(10) = SP.Q.GetText(16, 45, 8)
                arr(11) = SP.Q.GetText(17, 44, 8)
                arr(12) = SP.Q.GetText(19, 71, 8)
                arr(13) = SP.Q.GetText(13, 29, 19)

                SP.Q.Hit("Enter")
                SP.Q.Hit("Enter")
                arr(14) = SP.Q.GetText(7, 20, 10)
                SP.Q.Hit("F12")
                SP.Q.Hit("F12")
                arr(15) = SP.Q.GetText(20, 43, 4)
                arr(16) = DD
                TB.Rows.Add(arr)
            End If

            'create Datagrid Table style
            TS.GridLineColor = Me.BackColor
            Dim C1 As New FlatColumn
            With C1
                .HeaderText = "Ln Seq"
                .MappingName = "C1"
                .Width = 55
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C1)

            Dim C2 As New FlatColumnWhite
            With C2
                .HeaderText = "Ln Type"
                .MappingName = "C2"
                .Width = 50
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C2)

            Dim C3 As New FlatDateColumn
            With C3
                .HeaderText = "1st Disb Date"
                .MappingName = "C3"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C3)

            Dim C4 As New FlatColumnWhite
            With C4
                .HeaderText = "Original Balance"
                .MappingName = "C4"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C4)

            Dim C5 As New FlatColumn
            With C5
                .HeaderText = "Current Principal"
                .MappingName = "C5"
                .Width = 70
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C5)

            Dim C6 As New FlatColumnWhite
            With C6
                .HeaderText = "Interest Rate"
                .MappingName = "C6"
                .Width = 55
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C6)

            Dim C7 As New FlatColumn
            With C7
                .HeaderText = "Repay Type"
                .MappingName = "C7"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C7)

            Dim C8 As New FlatColumnWhite
            With C8
                .HeaderText = "Status"
                .MappingName = "C8"
                .Width = 110
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C8)

            Dim C9 As New FlatDateColumn
            With C9
                .HeaderText = "Sep Date"
                .MappingName = "C9"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C9)

            Dim C10 As New FlatDateColumnWhite
            With C10
                .HeaderText = "Grace End"
                .MappingName = "C10"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C10)

            Dim C11 As New FlatDateColumn
            With C11
                .HeaderText = "Repay Start"
                .MappingName = "C11"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C11)

            Dim C12 As New FlatDateColumnWhite
            With C12
                .HeaderText = "Disclosure"
                .MappingName = "C12"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C12)

            Dim C13 As New FlatColumn
            With C13
                .HeaderText = "School"
                .MappingName = "C13"
                .Width = 75
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C13)

            Dim C14 As New FlatColumnWhite
            With C14
                .HeaderText = "Bill Method"
                .MappingName = "C14"
                .Width = 55
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C14)

            Dim C15 As New FlatColumn
            With C15
                .HeaderText = "Repay Term"
                .MappingName = "C15"
                .Width = 40
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C15)

            Dim C16 As New FlatColumnWhite
            With C16
                .HeaderText = "Due Date"
                .MappingName = "C16"
                .Width = 30
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            TS.GridColumnStyles.Add(C16)

            TS.RowHeadersVisible = False
            dgLnHist.TableStyles.Clear()
            dgLnHist.TableStyles.Add(TS)
            DV = New DataView(TB)
            dgLnHist.DataSource = DV

            While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False And SP.Q.Check4Text(1, 72, "TCX0C") = False
                SP.Q.Hit("F12")
            End While
        End If



        SP.Processing.Visible = False
        Me.Show()
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


        'r.Inflate(0, -1)
        '' get the height column should be 
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


        'r.Inflate(0, -1)
        '' get the height column should be 
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


        'r.Inflate(0, -1)
        '' get the height column should be 
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


        'r.Inflate(0, -1)
        '' get the height column should be 
        Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
        Dim h As Integer = CType((sDraw.Height + 15), Integer)

        If IsDate(s) Then s = CDate(s).ToShortDateString Else s = ""
        g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
        bPainted = True

    End Sub

End Class

