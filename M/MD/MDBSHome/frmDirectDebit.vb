Imports System.Windows.Forms

Public Class frmDirectDebit
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    Public Overloads Sub Show(ByRef bsBorrower As BorrowerBS)
        If bsBorrower.ACHData.ACHDataFound = False Then
            For Each ctrl As Control In Me.Controls
                ctrl.Visible = False
            Next ctrl
            lblNoInfo.Visible = True
        Else
            lblNoInfo.Visible = False
            If bsBorrower.ACHData.HasACH = "Yes" Then
                lblDD.Text = bsBorrower.ACHData.HasACH
                lblSD.Text = bsBorrower.ACHData.StatusDt
                lblASN.Text = bsBorrower.ACHData.LnLvlInfo(3, 0)
                lblRN.Text = bsBorrower.ACHData.RoutingNumber
                lblAN.Text = bsBorrower.ACHData.AccountNumber
                lblAWA.Text = bsBorrower.ACHData.AdditionalWithdrawAmt
                lblNC.Text = bsBorrower.ACHData.NSFCounter
                lblDR.Text = "NA"
                LVLoanInfo.Items.Clear()
                For i As Integer = 0 To bsBorrower.ACHData.LnLvlInfo.GetUpperBound(1)
                    Dim newItemIndex As Integer = LVLoanInfo.Items.Add(bsBorrower.ACHData.LnLvlInfo(0, i)).Index
                    LVLoanInfo.Items(newItemIndex).SubItems.Add(bsBorrower.ACHData.LnLvlInfo(1, i))
                    LVLoanInfo.Items(newItemIndex).SubItems.Add(bsBorrower.ACHData.LnLvlInfo(2, i))
                Next i
            Else
                lblDD.Text = bsBorrower.ACHData.HasACH
                lblSD.Text = bsBorrower.ACHData.StatusDt
                lblDR.Text = GetDenialReason(bsBorrower.ACHData.DenialReason)
                lblASN.Text = "NA"
                lblRN.Text = "NA"
                lblAN.Text = "NA"
                lblAWA.Text = "NA"
                lblNC.Text = "NA"
            End If
        End If
        Me.Show()
    End Sub

    Function GetDenialReason(ByVal reasonCode As String) As String
        Select Case reasonCode
            Case "A"
                Return "No Eligible Loans"
            Case "B"
                Return "Borr Rqst Cancel"
            Case "C"
                Return "Bank Account"
            Case "D"
                Return "Missing Acct"
            Case "E"
                Return "Consol Recd"
            Case "F"
                Return "Missing ABA"
            Case "G"
                Return "Acct # Invld"
            Case "H"
                Return "Bank Not Mmbr"
            Case "I"
                Return "Rqst Incomplete"
            Case "J"
                Return "No Activ Repay"
            Case "K"
                Return "Addl Debit $5"
            Case "L"
                Return "Missing Void Chk"
            Case "N"
                Return "Multiple NFS's"
            Case "O"
                Return "ABA # Invalid"
            Case "P"
                Return "Elig Lns PIF"
            Case "S"
                Return "Rqt Not Signed"
            Case "X"
                Return "Pending SSN Chg"
            Case "Z"
                Return "Bank Info Change"
            Case Else
                Return ""
        End Select
    End Function

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub
End Class
