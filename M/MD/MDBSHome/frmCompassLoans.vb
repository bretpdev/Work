Imports System.Windows.Forms
Imports SP.Q

Public Class frmCompassLoans
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
        dgLnHist.BackColor = Me.BackColor
        dgLnHist.ForeColor = Me.ForeColor
    End Sub

    Public Overloads Sub Show(ByVal dueDate As String)
        SP.Processing.Visible = True
        SP.Processing.Refresh()

        'only collect the data if it hasn't be collected yet
        If dgLnHist.DataSource Is Nothing Then
            Dim loanHistoryTable As New DataTable()
            loanHistoryTable.Columns.Add("C0")
            loanHistoryTable.Columns.Add("C1")
            loanHistoryTable.Columns.Add("C2")
            loanHistoryTable.Columns.Add("C3", GetType(Date))
            loanHistoryTable.Columns.Add("C4")
            loanHistoryTable.Columns.Add("C5")
            loanHistoryTable.Columns.Add("C6")
            loanHistoryTable.Columns.Add("C7")
            loanHistoryTable.Columns.Add("C8")
            loanHistoryTable.Columns.Add("C9")
            loanHistoryTable.Columns.Add("C10")
            loanHistoryTable.Columns.Add("C11")
            loanHistoryTable.Columns.Add("C12")
            loanHistoryTable.Columns.Add("C13")
            loanHistoryTable.Columns.Add("C14")
            loanHistoryTable.Columns.Add("C15")
            loanHistoryTable.Columns.Add("C16")

            'switch keys until right option is displayed
            If Check4Text(1, 74, "TCX0D") OrElse Check4Text(1, 72, "TCX06") Then
                While Check4Text(24, 52, "LOAN") = False
                    Hit("F2")
                End While
            ElseIf Check4Text(1, 72, "TCX0I") Then
                While Check4Text(24, 58, "LOAN") = False
                    Hit("F2")
                End While
            ElseIf Check4Text(1, 72, "TCX0C") Then
                While Check4Text(24, 50, "LOAN") = False
                    Hit("F2")
                End While
            End If

            'access TS26
            Hit("F8")
            If Check4Text(1, 72, "TSX28") Then
                'selection screen
                Dim selectionCounter As Integer = 1
                While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    PutText(21, 12, selectionCounter.ToString("00"), True)
                    If Check4Text(23, 2, "01032 SELECTION MUST CORRESPOND TO A DISPLAYED KEY") Then
                        'if there are no other loans to select
                        selectionCounter = 1
                        Hit("F8") 'try and page forward to the next set of loans
                    Else
                        Dim columnValues() As String = GetDataGridValuesFromTS29(dueDate)
                        If IsDate(columnValues(3)) = False Then columnValues(3) = Nothing
                        If IsDate(columnValues(9)) = False Then columnValues(9) = Nothing
                        If IsDate(columnValues(10)) = False Then columnValues(10) = Nothing
                        If IsDate(columnValues(11)) = False Then columnValues(11) = Nothing
                        If IsDate(columnValues(12)) = False Then columnValues(12) = Nothing
                        loanHistoryTable.Rows.Add(columnValues)
                        selectionCounter += 1
                        Hit("F12")
                    End If
                End While
            Else
                'target screen
                Dim columnValues() As String = GetDataGridValuesFromTS29(dueDate)
                loanHistoryTable.Rows.Add(columnValues)
            End If

            'create Datagrid Table style
            Dim tableStyle As New DataGridTableStyle()
            tableStyle.GridLineColor = Me.BackColor
            Dim column As New FlatColumn()
            With column
                .HeaderText = "Ln Seq"
                .MappingName = "C1"
                .Width = 55
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(column)

            Dim whiteColumn As New FlatColumnWhite()
            With whiteColumn
                .HeaderText = "Ln Type"
                .MappingName = "C2"
                .Width = 50
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(whiteColumn)

            Dim dateColumn As New FlatDateColumn()
            With dateColumn
                .HeaderText = "1st Disb Date"
                .MappingName = "C3"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(dateColumn)

            whiteColumn = New FlatColumnWhite()
            With whiteColumn
                .HeaderText = "Original Balance"
                .MappingName = "C4"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(whiteColumn)

            column = New FlatColumn()
            With column
                .HeaderText = "Current Principal"
                .MappingName = "C5"
                .Width = 70
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(column)

            whiteColumn = New FlatColumnWhite()
            With whiteColumn
                .HeaderText = "Interest Rate"
                .MappingName = "C6"
                .Width = 55
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(whiteColumn)

            column = New FlatColumn()
            With column
                .HeaderText = "Repay Type"
                .MappingName = "C7"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(column)

            whiteColumn = New FlatColumnWhite()
            With whiteColumn
                .HeaderText = "Status"
                .MappingName = "C8"
                .Width = 110
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(whiteColumn)

            dateColumn = New FlatDateColumn()
            With dateColumn
                .HeaderText = "Sep Date"
                .MappingName = "C9"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(dateColumn)

            Dim whiteDateColumn As New FlatDateColumnWhite()
            With whiteDateColumn
                .HeaderText = "Grace End"
                .MappingName = "C10"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(whiteDateColumn)

            dateColumn = New FlatDateColumn()
            With dateColumn
                .HeaderText = "Repay Start"
                .MappingName = "C11"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(dateColumn)

            whiteDateColumn = New FlatDateColumnWhite()
            With whiteDateColumn
                .HeaderText = "Disclosure"
                .MappingName = "C12"
                .Width = 65
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(whiteDateColumn)

            column = New FlatColumn()
            With column
                .HeaderText = "School"
                .MappingName = "C13"
                .Width = 75
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(column)

            whiteColumn = New FlatColumnWhite()
            With whiteColumn
                .HeaderText = "Bill Method"
                .MappingName = "C14"
                .Width = 55
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(whiteColumn)

            column = New FlatColumn()
            With column
                .HeaderText = "Repay Term"
                .MappingName = "C15"
                .Width = 40
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(column)

            whiteColumn = New FlatColumnWhite()
            With whiteColumn
                .HeaderText = "Due Date"
                .MappingName = "C16"
                .Width = 30
                .TextBox.ForeColor = Me.ForeColor
                .TextBox.BackColor = Me.BackColor
                .ReadOnly = True
            End With
            tableStyle.GridColumnStyles.Add(whiteColumn)

            tableStyle.RowHeadersVisible = False
            dgLnHist.TableStyles.Clear()
            dgLnHist.TableStyles.Add(tableStyle)
            dgLnHist.DataSource = New DataView(loanHistoryTable)

            While Check4Text(1, 74, "TCX0D") = False AndAlso Check4Text(1, 72, "TCX06") = False AndAlso Check4Text(1, 72, "TCX0I") = False AndAlso Check4Text(1, 72, "TCX0C") = False
                Hit("F12")
            End While
        End If

        SP.Processing.Visible = False
        Me.Show()
    End Sub

    Private Function GetDataGridValuesFromTS29(ByVal dueDate As String) As String()
        Dim columnValues(16) As String
        columnValues(1) = GetText(7, 35, 4)
        Select Case GetText(6, 66, 6)
            Case "STFFRD"
                columnValues(2) = "Sub"
            Case "UNSTFD"
                columnValues(2) = "Unsub"
            Case "SLS"
                columnValues(2) = "SLS"
            Case "PLUS"
                columnValues(2) = "PLUS"
            Case "UNSPC", "SUBSPC"
                columnValues(2) = "Spousal"
            Case "SUBCNS", "UNCNS"
                columnValues(2) = "Consol"
            Case "TILP"
                columnValues(2) = "TILP"
        End Select
        columnValues(3) = GetText(6, 18, 8)
        columnValues(4) = GetText(11, 36, 10)
        columnValues(5) = GetText(11, 12, 12)
        columnValues(6) = GetText(11, 58, 6)
        columnValues(7) = GetText(18, 42, 5)
        columnValues(8) = GetText(3, 10, 28)
        columnValues(9) = GetText(18, 17, 8)
        columnValues(10) = GetText(16, 45, 8)
        columnValues(11) = GetText(17, 44, 8)
        columnValues(12) = GetText(19, 71, 8)
        columnValues(13) = GetText(13, 29, 19)
        Hit("Enter")
        Hit("Enter")
        columnValues(14) = GetText(7, 20, 10)
        Hit("F12")
        Hit("F12")
        columnValues(15) = GetText(20, 43, 4)
        columnValues(16) = dueDate
        Return columnValues
    End Function

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub
End Class
