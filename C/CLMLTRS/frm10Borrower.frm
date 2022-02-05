VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frm10Borrower 
   Caption         =   "Claim Return Transmittal"
   ClientHeight    =   6810
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7380
   OleObjectBlob   =   "frm10Borrower.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frm10Borrower"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


Private Letter As String

Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnOK_Click()
    If Letter = "RETTRANS" Then
        printRETTRANS
    ElseIf Letter = "TLFTRANS" Then
        printTLFTRANS
    End If
End Sub

Private Function printRETTRANS()
    Dim Name As String
    Dim Address1 As String
    Dim Address2 As String
    Dim City As String
    Dim State As String
    Dim Zip As String
    Dim Country As String
    Dim DocPath As String
    DocPath = "X:\PADD\Claims\"
    SP.Common.TestMode , DocPath
    
    SP.q.FastPath "LPSVI" & cmbServicer.Text
    If SP.q.Check4Text(1, 57, "INSTITUTION DEMOGRAPHICS") Then
        Name = SP.q.GetText(5, 21, 40)
        Address1 = SP.q.GetText(8, 21, 40)
        Address2 = SP.q.GetText(9, 21, 40)
        City = SP.q.GetText(11, 21, 30)
        Zip = SP.q.GetText(11, 66, 14)
        Country = SP.q.GetText(12, 55, 25)
        If Country = "" Then
            State = SP.q.GetText(11, 59, 2)
        Else
            State = SP.q.GetText(12, 21, 15)
        End If
                
        Open "C:\Windows\Temp\ltr.dat" For Output As #1
            Write #1, "Name", "Address1", "Address2", "City", "State", "Zip", "Country", "Name1", "Name2", "Name3", "Name4", "Name5", "Name6", "Name7", "Name8", "Name9", "Name10", "SSN1", "SSN2", "SSN3", "SSN4", "SSN5", "SSN6", "SSN7", "SSN8", "SSN9", "SSN10"
            Write #1, Name, Address1, Address2, City, State, Zip, Country, txtName1.Text, txtName2.Text, txtName3.Text, txtName4.Text, txtName5.Text, txtName6.Text, txtName7.Text, txtName8.Text, txtName9.Text, txtName10.Text, protectSSN(txtSSN1.Text), protectSSN(txtSSN2.Text), protectSSN(txtSSN3.Text), protectSSN(txtSSN4.Text), protectSSN(txtSSN5.Text), protectSSN(txtSSN6.Text), protectSSN(txtSSN7.Text), protectSSN(txtSSN8.Text), protectSSN(txtSSN9.Text), protectSSN(txtSSN10.Text)
        Close #1
        Me.Hide
        SP.Common.PrintDocs DocPath, "RETTRANS", "C:\Windows\Temp\ltr.dat", False
    Else
        MsgBox ("The Servicer ID you provided is invalid! Try again.")
    End If
End Function

Private Function printTLFTRANS()
    Dim DocPath As String
    DocPath = "X:\PADD\Claims\"
    SP.Common.TestMode , DocPath
    Open "C:\Windows\Temp\ltr.dat" For Output As #1
        Write #1, "Name1", "Name2", "Name3", "Name4", "Name5", "Name6", "Name7", "Name8", "Name9", "Name10", "SSN1", "SSN2", "SSN3", "SSN4", "SSN5", "SSN6", "SSN7", "SSN8", "SSN9", "SSN10"
        Write #1, txtName1.Text, txtName2.Text, txtName3.Text, txtName4.Text, txtName5.Text, txtName6.Text, txtName7.Text, txtName8.Text, txtName9.Text, txtName10.Text, protectSSN(txtSSN1.Text), protectSSN(txtSSN2.Text), protectSSN(txtSSN3.Text), protectSSN(txtSSN4.Text), protectSSN(txtSSN5.Text), protectSSN(txtSSN6.Text), protectSSN(txtSSN7.Text), protectSSN(txtSSN8.Text), protectSSN(txtSSN9.Text), protectSSN(txtSSN10.Text)
    Close #1
    Me.Hide
    SP.Common.PrintDocs DocPath, "TLFTRANS", "C:\Windows\Temp\ltr.dat", False
End Function

Private Sub UserForm_Initialize()
    If Letter = "RETTRANS" Then
        cmbServicer.Visible = True
        SVList = Array("700121", "700126", "700191", "700789")
        cmbServicer.list() = SVList
    ElseIf Letter = "TLFTRANS" Then
        cmbServicer.Visible = False
    End If
End Sub

Function setLetter(L As String)
    Letter = L
End Function

Public Function protectSSN(fullSSN As String) As String
        If fullSSN = "" Then
            fullSSN = ""
        Else
        'SSN isn't blank needs to be protected
        fullSSN = "XXX-XX-" & Right(fullSSN, 4)
        End If
        protectSSN = fullSSN
End Function
