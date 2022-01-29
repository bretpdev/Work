VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} ClaimInput 
   Caption         =   "Claim Letters Input"
   ClientHeight    =   6795
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5820
   OleObjectBlob   =   "ClaimInput.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "ClaimInput"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False



Private Type LoanStruct
    LoanType As String
    LoanStatus As String
    HolderID As String
    ServicerID As String
    UniqueID As String
End Type
Private Enum HolderOrServicer
    Holder
    Servicer
End Enum
Private Loans() As LoanStruct
Public OkWasClicked As Boolean

Private Sub txtSSN_Exit(ByVal Cancel As MSForms.ReturnBoolean)
    If Len(txtSSN.Text) <> 9 Or IsNumeric(txtSSN.Text) = False Then
        MsgBox "Please check that the SSN is correct."
    Else
        GatherLoanInfo
        txtBorrName = GetText(3, 2, 30)
        PopulateComboBox Me.cmbHolderID, Holder
        PopulateComboBox Me.cmbServicerID, Servicer
        'Put the first list item of each combo box into the Value spot.
        cmbHolderID.Value = cmbHolderID.list(0, 0)
        cmbServicerID.Value = cmbServicerID.list(0, 0)
        'Load the list of unique IDs for the selected servicer.
        ShowUniqueIDs cmbServicerID.Value
    End If
End Sub

Private Sub UserForm_Initialize()
    OkWasClicked = False
    cmbClaimType.list() = Array("default", "bankruptcy", "closed school", "death", "disability", "abreviated cure", "false certification", "ineligible borrower")
    cmbHolderID.Clear
    cmbHolderID.Value = ""
    cmbServicerID.Clear
    cmbServicerID.Value = ""
    lstAvailableUniqueIDs.Clear
End Sub

Private Sub GatherLoanInfo()
    ReDim Loans(0)
    
    'Get loan information for this borrower from LG02.
    FastPath "LG02I" & txtSSN.Text
    If Check4Text(1, 70, "APPLICATION") Then
        'Target screen. Go to LG10 to get all the data needed.
        FastPath "LG10I" & txtSSN.Text
        Loans(0).LoanType = GetText(4, 13, 2)
        Loans(0).LoanStatus = GetText(11, 59, 2)
        If Loans(0).LoanStatus <> "CA" And Loans(0).LoanStatus <> "PF" And Loans(0).LoanStatus <> "PC" And Loans(0).LoanStatus <> "PN" Then
            'Get the remaining loan data.
            Loans(0).HolderID = GetText(5, 18, 8)
            Loans(0).ServicerID = GetText(5, 27, 8)
            Loans(0).UniqueID = GetText(11, 4, 19)
        End If
    Else
        'Selection screen. Look at each loan.
        Dim Row As Integer
        Do While Check4Text(22, 3, "     ")
            Row = 10
            Do While Row < 21 And Check4Text(Row, 2, "  ") = False
                Loans(UBound(Loans)).LoanType = GetText(Row, 5, 2)
                Loans(UBound(Loans)).LoanStatus = GetText(Row, 75, 2)
                If Loans(UBound(Loans)).LoanStatus <> "CA" _
                    And Loans(UBound(Loans)).LoanStatus <> "PF" _
                    And Loans(UBound(Loans)).LoanStatus <> "PC" _
                    And Loans(UBound(Loans)).LoanStatus <> "PN" _
                Then
                    'Select the loan
                    PutText 21, 13, GetText(Row, 2, 2), "ENTER"
                    'Get data based on the loan type.
                    Select Case Loans(UBound(Loans)).LoanType
                    Case "CL"   'Consolidation
                        Loans(UBound(Loans)).HolderID = GetText(11, 46, 8)
                        Loans(UBound(Loans)).ServicerID = GetText(11, 71, 8)
                        Loans(UBound(Loans)).UniqueID = GetText(5, 31, 19)
                    Case "SL"   'SLS
                        Loans(UBound(Loans)).HolderID = GetText(19, 44, 8)
                        Loans(UBound(Loans)).ServicerID = GetText(19, 73, 8)
                        Loans(UBound(Loans)).UniqueID = GetText(2, 33, 19)
                    Case Else   'Stafford
                        Loans(UBound(Loans)).HolderID = GetText(18, 49, 8)
                        Loans(UBound(Loans)).ServicerID = GetText(18, 73, 8)
                        Loans(UBound(Loans)).UniqueID = GetText(3, 33, 19)
                    End Select
                    'Resize the Loans array and back out to the loan selection screen.
                    ReDim Preserve Loans(UBound(Loans) + 1)
                    Hit "F12"
                End If
                Row = Row + 1
            Loop
            Hit "F8"
        Loop
        'Take the empty array index off the end of the Loans array.
        If UBound(Loans) > 0 Then
            ReDim Preserve Loans(UBound(Loans) - 1)
        End If
    End If
End Sub

Private Sub PopulateComboBox(ByRef Combo As ComboBox, ByVal HorS As HolderOrServicer)
    Dim i As Integer
    Combo.Clear
    For i = 0 To UBound(Loans)
        If HorS = Holder Then
            If ExistsInComboBox(Combo, Loans(i).HolderID) = False Then
                Combo.AddItem Loans(i).HolderID
            End If
        Else    'Servicer
            If ExistsInComboBox(Combo, Loans(i).ServicerID) = False Then
                Combo.AddItem Loans(i).ServicerID
            End If
        End If
    Next i
End Sub

'Sadly, VBA doesn't seem to have something like this as a member function of ComboBox.
Private Function ExistsInComboBox(ByRef Combo As ComboBox, ByVal Text As String) As Boolean
    Dim Item As Integer
    ExistsInComboBox = False
    For Item = 0 To Combo.ListCount - 1
        If Combo.list(Item, 0) = Text Then
            ExistsInComboBox = True
            Exit Function
        End If
    Next Item
End Function

Private Sub cmbServicerID_Change()
    If cmbServicerID <> "" Then
        'Load the list of unique IDs for the selected servicer.
        ShowUniqueIDs cmbServicerID.Value
    End If
End Sub

'Reload the list of unique IDs based on the selected servicer.
Private Sub ShowUniqueIDs(ByVal Servicer As String)
    Dim i As Integer
    lstAvailableUniqueIDs.Clear
    For i = 0 To UBound(Loans)
        If Loans(i).ServicerID = Servicer Then
            lstAvailableUniqueIDs.AddItem Loans(i).UniqueID
        End If
    Next i
End Sub

Private Sub btnAddUniqueID_Click()
    Dim x As Integer
    For x = 0 To lstAvailableUniqueIDs.ListCount
        If lstAvailableUniqueIDs.Selected(x) Then
            lstSelectedUniqueIDs.AddItem (lstAvailableUniqueIDs.list(x))
            lstAvailableUniqueIDs.RemoveItem (x)
        End If
    Next x
End Sub

Private Sub btnRemoveUniqueID_Click()
    Dim x As Integer
    For x = 0 To lstSelectedUniqueIDs.ListCount
        If lstSelectedUniqueIDs.Selected(x) Then
            lstAvailableUniqueIDs.AddItem (lstSelectedUniqueIDs.list(x))
            lstSelectedUniqueIDs.RemoveItem (x)
        End If
    Next x
End Sub

Private Sub btnOK_Click()
    OkWasClicked = True
    Me.Hide
End Sub

Private Sub btnCancel_Click()
    Me.Hide
    End
End Sub
