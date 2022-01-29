Imports System.Windows.Forms
Public Class frmPrivateLoans
    Inherits System.Windows.Forms.Form

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub

    Public Overloads Sub Show(ByVal loanTable As DataTable)
        SP.Processing.Visible = True
        SP.Processing.Refresh()

        'create Datagrid Table style
        Dim tableStyle As New DataGridTableStyle()
        tableStyle.GridLineColor = Me.BackColor
        Dim column As New FlatColumn()
        With column
            .HeaderText = "App ID"
            .MappingName = "C1"
            .Width = 95
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        tableStyle.GridColumnStyles.Add(column)

        Dim whiteDateColumn As New FlatDateColumnWhite()
        With whiteDateColumn
            .HeaderText = "App Create Date"
            .MappingName = "C2"
            .Width = 95
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        tableStyle.GridColumnStyles.Add(whiteDateColumn)

        column = New FlatColumn()
        With column
            .HeaderText = "Loan Type"
            .MappingName = "C3"
            .Width = 95
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        tableStyle.GridColumnStyles.Add(column)

        Dim whiteColumn As New FlatColumnWhite()
        With whiteColumn
            .HeaderText = "Status"
            .MappingName = "C4"
            .Width = 95
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        tableStyle.GridColumnStyles.Add(whiteColumn)

        column = New FlatColumn()
        With column
            .HeaderText = "Incomp Reasons"
            .MappingName = "C5"
            .Width = 157
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        tableStyle.GridColumnStyles.Add(column)

        whiteColumn = New FlatColumnWhite()
        With whiteColumn
            .HeaderText = "Susp Reasons"
            .MappingName = "C6"
            .Width = 158
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        tableStyle.GridColumnStyles.Add(whiteColumn)

        column = New FlatColumn()
        With column
            .HeaderText = "Rej Reasons"
            .MappingName = "C7"
            .Width = 158
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        tableStyle.GridColumnStyles.Add(column)

        tableStyle.RowHeadersVisible = False
        dgPvLnHist.TableStyles.Clear()
        dgPvLnHist.TableStyles.Add(tableStyle)
        dgPvLnHist.DataSource = New DataView(loanTable)

        SP.Processing.Visible = False
        Me.Show()
    End Sub
End Class
