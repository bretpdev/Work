Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Module ExtensionMethods
    <Extension()> _
    Public Function To2DArray(ByVal loans As List(Of Loan)) As String(,)
        Dim loanArray(0 To 6, 0 To loans.Count - 1) As String
        For i As Integer = 0 To loans.Count - 1
            loanArray(0, i) = loans(i).LoanType
            loanArray(1, i) = loans(i).RepaymentStartDate
            loanArray(2, i) = loans(i).DaysDelinquent.ToString()
            loanArray(3, i) = loans(i).DelinquencyDate
            loanArray(4, i) = loans(i).Installment.ToString("0.00")
            loanArray(5, i) = loans(i).Term.ToString()
            loanArray(6, i) = loans(i).EftRate.ToString("0.000")
        Next i
        Return loanArray
    End Function
End Module
