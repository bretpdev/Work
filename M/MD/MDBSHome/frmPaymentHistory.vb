Imports System.Collections.Generic
Imports SP.Q

Public Class frmPaymentHistory
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
        LVHist.BackColor = Me.BackColor
        LVHist.ForeColor = Me.ForeColor
    End Sub

    Public Overloads Sub Show(ByVal numberOfOnTimePayments As Integer)
        Dim finalTransactionData As New List(Of FinancialTransaction)()
        LVHist.Items.Clear()

        'prompt user for all open loans or all loans
        Dim openOnly As Integer = 0
        Dim openOrAllForm As New frmOpenOrAll()
        openOrAllForm.Show(openOnly)
        SP.Processing.Show()
        SP.Processing.Refresh()

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
        Dim tempTransactionData As New List(Of FinancialTransaction)()
        If openOnly = 1 Then
            tempTransactionData = GetTransactionsForOpenLoans()
        ElseIf openOnly = 2 Then
            tempTransactionData = GetTransactionsForAllLoans()
        Else
            SP.Processing.Hide()
            Return
        End If

        If numberOfOnTimePayments = 0 Then
            lbl48Pay.Visible = False
        Else
            lbl48Pay.Visible = True
            lbl48Pay.Text = numberOfOnTimePayments.ToString() + " of 48 On-time Payments"
        End If

        If tempTransactionData.Count = 0 Then
            ReturnToAcp()
            SP.frmWhoaDUDE.WhoaDUDE("No results were found.", "Payment History")
            Return
        End If

        'combine values and enter into another array
        For Each transaction As FinancialTransaction In tempTransactionData
            Dim matchIndex As Integer = GetIndexOfExistingTransaction(finalTransactionData, transaction.TransactionType, transaction.EffectiveDate, transaction.PostedDate)
            If matchIndex >= 0 Then
                finalTransactionData(matchIndex).AppliedPrincipal += transaction.AppliedPrincipal
                finalTransactionData(matchIndex).AppliedInterest += transaction.AppliedInterest
                finalTransactionData(matchIndex).AppliedLateFee += transaction.AppliedLateFee
                finalTransactionData(matchIndex).TransactionAmount += transaction.TransactionAmount
                finalTransactionData(matchIndex).PrincipalBalance += transaction.PrincipalBalance
            Else
                finalTransactionData.Add(transaction)
            End If
        Next transaction

        Dim topDate As DateTime = DateTime.MinValue
        Dim finalDataIndex As Integer = 0
        Do
            For Each transaction As FinancialTransaction In finalTransactionData 'get earliest date in sequence
                If String.IsNullOrEmpty(transaction.EffectiveDate) = False AndAlso _
                (topDate = DateTime.MinValue OrElse DateTime.Parse(transaction.EffectiveDate) >= topDate) Then
                    topDate = transaction.EffectiveDate
                    finalDataIndex = finalTransactionData.IndexOf(transaction) 'record this index
                End If
            Next transaction
            If topDate = DateTime.MinValue Then
                Exit Do
            End If
            'transfer array information to List View
            Dim mostRecentTransaction As FinancialTransaction = finalTransactionData(finalDataIndex)
            Dim newItemIndex As Integer = LVHist.Items.Add(mostRecentTransaction.ReversalReason).Index
            LVHist.Items(newItemIndex).SubItems.Add(mostRecentTransaction.EffectiveDate)
            LVHist.Items(newItemIndex).SubItems.Add(mostRecentTransaction.PostedDate)
            LVHist.Items(newItemIndex).SubItems.Add(mostRecentTransaction.TransactionType)
            LVHist.Items(newItemIndex).SubItems.Add(FormatCurrency(mostRecentTransaction.TransactionAmount, 2))
            LVHist.Items(newItemIndex).SubItems.Add(FormatCurrency(mostRecentTransaction.AppliedPrincipal, 2))
            LVHist.Items(newItemIndex).SubItems.Add(FormatCurrency(mostRecentTransaction.AppliedInterest, 2))
            LVHist.Items(newItemIndex).SubItems.Add(FormatCurrency(mostRecentTransaction.AppliedLateFee, 2))

            If mostRecentTransaction.PrincipalBalance > 0 Then
                LVHist.Items(newItemIndex).SubItems.Add(FormatCurrency(mostRecentTransaction.PrincipalBalance, 2))
            Else
                LVHist.Items(newItemIndex).ImageIndex = 0
            End If
            finalTransactionData(finalDataIndex).EffectiveDate = "" ' erase data since it's already been placed
            topDate = DateTime.MinValue
        Loop

        ReturnToAcp()
    End Sub

    Private Function GetIndexOfExistingTransaction(ByVal transactionList As List(Of FinancialTransaction), ByVal transactionType As String, ByVal effectiveDate As String, ByVal postedDate As String) As Integer
        Dim existingTransaction As FinancialTransaction = transactionList.Where(Function(p) p.TransactionType = transactionType AndAlso p.EffectiveDate = effectiveDate AndAlso p.PostedDate = postedDate).SingleOrDefault()
        If existingTransaction Is Nothing Then
            Return -1
        Else
            Return transactionList.IndexOf(existingTransaction)
        End If
    End Function

    'this function collects the information for all loans on TS26
    Private Function GetTransactionsForAllLoans() As List(Of FinancialTransaction)
        If Check4Text(1, 72, "TSX28") Then
            'selection screen
            Dim transactions As New List(Of FinancialTransaction)()
            Dim loanRow As Integer = 8
            While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                Dim nextRecord As String = GetText(loanRow, 2, 2)
                If nextRecord.Length = 0 Then
                    Exit While
                End If
                PutText(21, 12, "")
                Hit("End")
                PutText(21, 12, nextRecord, True)
                transactions.AddRange(GetTransactionDataFromTargetScreen().AsEnumerable())
                Hit("F12")
                loanRow += 1
                If loanRow = 20 Then
                    loanRow = 8
                    Hit("F8")
                End If
            End While
            Return transactions
        Else
            'target screen
            Return GetTransactionDataFromTargetScreen()
        End If
    End Function

    'this function collects the information for all open loans on TS26
    Private Function GetTransactionsForOpenLoans() As List(Of FinancialTransaction)
        If Check4Text(1, 72, "TSX28") Then
            'selection screen
            Dim transactions As New List(Of FinancialTransaction)()
            Dim foundOpenLoan As Boolean = False
            Dim loanRow As Integer = 8
            While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                foundOpenLoan = False
                If (Check4Text(loanRow, 64, " 0.00") = False AndAlso Check4Text(loanRow, 69, "CR") = False) OrElse Check4Text(loanRow, 59, "          ") Then
                    foundOpenLoan = True
                    Dim nextRecord As String = GetText(loanRow, 2, 2)
                    If nextRecord.Length = 0 Then
                        Exit While
                    End If
                    PutText(21, 12, "")
                    Hit("End")
                    PutText(21, 12, nextRecord, True)
                End If
                If foundOpenLoan Then
                    transactions.AddRange(GetTransactionDataFromTargetScreen().AsEnumerable())
                    Hit("F12")
                End If
                loanRow += 1
                If loanRow = 20 Then
                    loanRow = 8
                    Hit("F8")
                End If
            End While
            Return transactions
        Else
            'target screen
            If Check4Text(11, 12, "0.00") = False AndAlso Check4Text(11, 22, "CR") = False Then
                Return GetTransactionDataFromTargetScreen()
            Else
                Return New List(Of FinancialTransaction)()
            End If
        End If
    End Function

    Private Function GetTransactionDataFromTargetScreen() As List(Of FinancialTransaction)
        Dim transactions As New List(Of FinancialTransaction)()
        Hit("F6") 'page to FIN screen
        'Blank Active Indicator
        PutText(8, 2, "")
        Hit("End")
        'blank All field
        PutText(8, 51, "")
        Hit("End")
        'Select Active/Rev option
        PutText(10, 2, "X")
        PutText(10, 51, "X")
        PutText(16, 51, "X")
        PutText(17, 51, "X")
        PutText(18, 51, "X")
        PutText(20, 51, "X", True)

        If Check4Text(1, 72, "TSX7S") Then
            'Financial info isn't found.
            Hit("F12")
            Return transactions
        End If

        Dim transactionRow As Integer = 11
        'cycle through all selection rows and get the REV REA
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            If Check4Text(transactionRow, 3, " ") = False Then
                Dim transaction As New FinancialTransaction()
                transaction.ReversalReason = GetReversalReason(GetText(transactionRow, 8, 1))
                transaction.TransactionAmount = GetText(transactionRow, 41, 10)
                transaction.PrincipalBalance = GetText(transactionRow, 68, 10)
                transactions.Add(transaction)
            End If
            transactionRow += 1
            If Check4Text(transactionRow, 3, " ") Then
                transactionRow = 11
                Hit("F8")
            End If
        End While
        'back up and enter the target screen for each selection Row
        Hit("F12")
        Hit("Enter")
        'select for row
        PutText(22, 18, "1", True)
        'collect the rest of the information for each record
        Dim i As Integer = 0
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            transactions(i).EffectiveDate = GetText(16, 66, 8)
            transactions(i).PostedDate = GetText(18, 24, 8)
            transactions(i).TransactionType = GetText(16, 24, 8)
            If transactions(i).TransactionType = "PAYMENT" Then
                transactions(i).TransactionType = GetText(17, 24, 8)
            End If
            transactions(i).AppliedPrincipal = GetText(13, 15, 10)
            transactions(i).AppliedInterest = GetText(13, 28, 10)
            transactions(i).AppliedLateFee = GetText(13, 42, 10)
            Hit("F8")
            i += 1 'move to next row in array
        End While
        Hit("F12")
        Hit("F12")
        Hit("F12")
        Return transactions
    End Function

    Private Function GetReversalReason(ByVal reasonCode As String) As String
        Select Case reasonCode
            Case "1", "6"
                Return "NSF"
            Case "2", "3", "4", "7"
                Return "Post Error"
            Case "5"
                Return "Refund"
            Case "8"
                Return "Bad Check"
            Case "9"
                Return "Rev/Apply"
            Case Else
                Return ""
        End Select
    End Function

    Private Sub ReturnToAcp()
        While Check4Text(1, 74, "TCX0D") = False AndAlso Check4Text(1, 72, "TCX06") = False AndAlso Check4Text(1, 72, "TCX0I") = False AndAlso Check4Text(1, 72, "TCX0C") = False
            Hit("F12")
        End While
        SP.Processing.Hide()
        MyBase.Show()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub
End Class
