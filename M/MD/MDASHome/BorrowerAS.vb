Imports System.Threading
Imports System.Windows.Forms
Imports Q

Public Class BorrowerAS
    Inherits SP.Borrower

    Public BorrStat(0) As String
    Public Loans(3, 0) As String 'Loans(0,x) = loan type, Loans(1,x) =repayment start date, Loans(2,x) =days delinquent, Loans(3,x) =date delinquent
    Public Installments(2, 0) As String 'Installments(0,x) =Amount, 1 = 1st due date
    Public DateLastCntct As String
    Public DateLastAtempt As String
    Public RefereceArr(4, 0) As String


    Public Num20Day As String = ""
    Public Date20Day(4) As String
    'Public StatusArr(0) As String

    Public TotalPayoffAmount As Double
    Public TotalPrincipalDue As Double
    Public TotalInterestDue As Double
    Public TotalDailyInterest As Double
    Public TotalPastDueAmount As Double
    Public DateLastPaymentReceived As String = ""
    Public LastPaymentAmount As Double
    ''ACP

    Public BorLoanStatus As String
    Public CurrPrinBal As String
    Public InterestRate As String
    Public DisbursementDate As String
    Public LoanProgram As String
    Public DueDate As String
    Public NextDueDate As String = ""
    Public DueDay As String = ""
    Public StatRateData(,) As String              'LoanSeq, StatRate
    Public AmountDue As String
    Public FirstScreen As String
    Public HasTPDD As Boolean

    Public BBArray(,) As String

    Public AddressSource As String
    Public PhoneSource As String

    ''' <summary>
    ''' Only access through BorrowerAS properties.  This is an object that can be used for values outside the normal borrower object that is specific to homepage.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScriptSpecificInfo() As MDScriptInfoSpecificToAuxiliaryServices
        Get
            Return CType(ScriptInfoToGenericBusinessUnit, MDScriptInfoSpecificToAuxiliaryServices)
        End Get
        Set(ByVal value As MDScriptInfoSpecificToAuxiliaryServices)
            ScriptInfoToGenericBusinessUnit = CType(value, MDScriptInfoSpecificToAuxiliaryServices)
        End Set
    End Property

    ''' <summary>
    ''' Has repayment schedule indicator.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HasRepaymentSchedule() As String
        Get
            Return ScriptSpecificInfo.HasRepaymentSchedule
        End Get
        Set(ByVal value As String)
            ScriptSpecificInfo.HasRepaymentSchedule = value
        End Set
    End Property

    ''' <summary>
    ''' Current amount due.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CurrentDueAmount() As Double
        Get
            Return ScriptSpecificInfo.CurrentAmountDue
        End Get
        Set(ByVal value As Double)
            ScriptSpecificInfo.CurrentAmountDue = value
        End Set
    End Property

    ''' <summary>
    ''' Outstanding late fees.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OutstandingLateFees() As Double
        Get
            Return ScriptSpecificInfo.OutstandingLateFees
        End Get
        Set(ByVal value As Double)
            ScriptSpecificInfo.OutstandingLateFees = value
        End Set
    End Property

    ''' <summary>
    ''' Monthly Payment Amount.
    ''' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MonthlyPaymentAmount() As String
        Get
            Return ScriptSpecificInfo.MonthlyPaymentAmount
        End Get
        Set(ByVal value As String)
            ScriptSpecificInfo.MonthlyPaymentAmount = value
        End Set
    End Property

    Public Sub New(ByVal BL As SP.BorrowerLite)
        MyBase.New(BL)
        ScriptInfoToGenericBusinessUnit = New MDScriptInfoSpecificToAuxiliaryServices 'create one specific to Aux Services
    End Sub

    Public Overridable Sub SystemProc4PayOffDtKeyPress(ByVal d As Date)
        SP.Q.FastPath("TX3Z/ITS2O" & SSN)
        SP.Q.PutText(7, 26, Format(d, "MM"))
        SP.Q.PutText(7, 29, Format(d, "dd"))
        SP.Q.PutText(7, 32, Format(d, "yy"))
        SP.Q.PutText(9, 16, "X")
        SP.Q.PutText(9, 54, "Y", True)
        SP.Q.Hit("ENTER")
        TotalPayoffAmount = CDbl(SP.Q.GetText(12, 29, 10))
    End Sub

    Sub GetRefereces()
        '0=ID
        '1=Name
        '2=Auth3rd
        '3=DateLastContact
        '4=Attempt
        Dim x As Integer
        SP.Q.FastPath("LP2CI" & SSN)
        If SP.Q.Check4Text(1, 65, "REFERENCE SELECT") Then
            Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                For x = 6 To 18 Step 3
                    If SP.Q.Check4Text(x + 1, 27, "A") Then
                        RefereceArr(0, UBound(RefereceArr, 2)) = Trim(SP.Q.GetText(x, 68, 9)) 'ID
                        RefereceArr(1, UBound(RefereceArr, 2)) = Trim(SP.Q.GetText(x, 7, 40)) 'Name
                        RefereceArr(2, UBound(RefereceArr, 2)) = Trim(SP.Q.GetText(x + 1, 33, 1)) 'Authorized 3rd Party
                        ReDim Preserve RefereceArr(4, UBound(RefereceArr, 2) + 1)
                    End If
                Next x
                SP.Q.Hit("F8")
            Loop
        ElseIf SP.Q.Check4Text(1, 59, "REFERENCE DEMOGRAPHICS") Then
            If SP.Q.Check4Text(6, 67, "A") Then
                RefereceArr(0, UBound(RefereceArr, 2)) = Trim(SP.Q.GetText(3, 14, 9)) 'ID
                RefereceArr(1, UBound(RefereceArr, 2)) = Trim(SP.Q.GetText(4, 44, 12)) & " " & Trim(SP.Q.GetText(4, 5, 35)) 'Name
                RefereceArr(2, UBound(RefereceArr, 2)) = Trim(SP.Q.GetText(6, 51, 1)) 'Authorized 3rd Party
                ReDim RefereceArr(4, UBound(RefereceArr, 2) + 1)
            End If
        Else
            SP.frmKnarlyDUDE.KnarlyDude("This borrower is surfin solo man! No referece was found.", "No Refereces", True)
            Exit Sub
        End If


        For x = 0 To UBound(RefereceArr, 2) - 1
            SP.Q.FastPath("TX3Z/ITD2A" & RefereceArr(0, x))
            'Obtain the Date Last Contact 
            SP.Q.PutText(10, 65, "CNTCT", True)
            If SP.Q.Check4Text(23, 2, "01019") Then
                'never had a contact with borrower
            Else
                If SP.Q.Check4Text(1, 72, "TDX2C") Then
                    RefereceArr(3, x) = Trim(SP.Q.GetText(6, 51, 1)) 'Last Contact
                ElseIf SP.Q.Check4Text(1, 72, "TDX2D") Then
                    RefereceArr(3, x) = Trim(SP.Q.GetText(6, 51, 1)) 'Last Contact
                End If
                SP.Q.Hit("F12")
            End If
            'Obtain the Date Last Attempt 
            SP.Q.PutText(10, 65, "NOCTC", True)
            If SP.Q.Check4Text(23, 2, "01019") Then
                'we have not attempted to contact the borrower previously
            Else
                If SP.Q.Check4Text(1, 72, "TDX2C") Then
                    RefereceArr(4, x) = Trim(SP.Q.GetText(6, 51, 1)) 'Attempt
                ElseIf SP.Q.Check4Text(1, 72, "TDX2D") Then
                    RefereceArr(4, x) = Trim(SP.Q.GetText(6, 51, 1)) 'Attempt
                End If
                SP.Q.Hit("F12")
            End If
        Next x
    End Sub

    Public Sub ACPEntryAnd3rdPrtyChk(ByVal ASHP As Form)
        Dim x As Integer
        Dim rec As Integer
        Dim TestStr As String
        If SP.Q.Check4Text(1, 72, "TCX06") Then
            ShowTCX06Values = True
            While SP.Q.Check4Text(24, 26, "F4=PYOF") = False
                SP.Q.Hit("F2")
            End While
            SP.Q.Hit("F4")
            SP.Q.PutText(7, 26, Format(Today, "MM"))
            SP.Q.PutText(7, 29, Format(Today, "dd"))
            SP.Q.PutText(7, 32, Format(Today, "yy"))
            SP.Q.PutText(9, 16, "X")
            SP.Q.PutText(9, 54, "Y", True)
            SP.Q.Hit("ENTER")
            TotalPayoffAmount = CDbl(SP.Q.GetText(12, 29, 10))
            TotalPrincipalDue = (CDbl(SP.Q.GetText(14, 29, 10)))
            TotalInterestDue = CDbl(SP.Q.GetText(15, 29, 10))
            TotalDailyInterest = CDbl(SP.Q.GetText(17, 29, 10))
            TotalPastDueAmount = CDbl(SP.Q.GetText(18, 29, 10))
            TotalLateFeesDue = CDbl(SP.Q.GetText(19, 29, 10))
            SP.Q.Hit("F12")
            SP.Q.Hit("F12")
            DateLastPaymentReceived = SP.Q.GetText(11, 69, 8)
            LastPaymentAmount = CDbl(SP.Q.GetText(10, 67, 10))
            'sr2091
            'AmountPastDue = CDbl(SP.Q.GetText(8, 18, 10))
            'CurrentDueAmount = CDbl(SP.Q.GetText(9, 18, 10))
            'If SP.Q.GetText(9, 18, 10) <> "0.00" Then MonthlyPA = CDbl(SP.Q.GetText(9, 18, 10))
            'TotalAmountDue = CDbl(sp.q.GetText(11, 18, 10))
            'OutstandingLateFees = CDbl(SP.Q.GetText(9, 67, 10))
            '<1502->
            NextDDAndStatRate()
            '</1502>
        Else
            ShowTCX06Values = False
        End If
        CType(ASHP, frmASHomePage).NoACPBSVCall(BorLite.NoACPBSVCall)
        CType(ASHP, frmASHomePage).ThirdPartyYes()
        If SP.Q.Check4Text(1, 72, "TCX0I") Then
            While SP.Q.Check4Text(24, 26, "F4=REL") = False
                SP.Q.Hit("F2")
            End While
            SP.Q.Hit("F4")
            If SP.Q.Check4Text(1, 74, "TXX1Y") Then
                'Prompt the user to verify ‘Yes’ or ‘No’ whether the caller is an authorized 3rd party
                SP.Processing.Visible = False

                If SP.frmYesNo.YesNo("Is the caller an authorized 3rd party?") Then
                    'Caller is authorized 3rd party
                    Found3rd = False
                    rec = 0
                    Do While Found3rd = False
                        rec += 1
                        If SP.Q.Check4Text(1, 74, "TXX1Y") = False Then
                            Do While SP.Q.Check4Text(1, 74, "TXX1Y") = False
                                SP.frmWhoaDUDE.WhoaDUDE("No way DUDE! You gota be on the Borrower Relationships Screen(TXX1Y), if you wana keep surfing. Press 'OK' when you are there.", "Bad Screen", True)
                            Loop
                        End If
                        If rec > 1 Then
                            If SP.frmYesNo.YesNo("Is the caller an authorized 3rd party?") = False Then
                                'Not Authorized
                                CType(ASHP, frmASHomePage).ThirdPartyNo()
                                Exit Do
                            End If
                        End If
                        While SP.Q.Check4Text(23, 2, "90007") = False
                            For x = 10 To 21
                                If SP.Q.Check4Text(x, 49, "Y") Then
                                    Found3rd = True
                                    Exit For
                                End If
                            Next
                            SP.Q.Hit("F8")
                        End While
                        If Found3rd = False Then
                            'warn the user that there is no authorized 3rd party and re-pause the script for the user to make the appropriate updates
                            SP.frmWhoaDUDE.WhoaDUDE("Are you lolo? There is no authorized 3rd party. How about you make it mo betta and press 'OK' when you are ready.", "No Third Party Found.", True)
                        End If
                    Loop
                Else
                    'Not Authorized
                    CType(ASHP, frmASHomePage).ThirdPartyNo()
                End If
                SP.Processing.Visible = True
            End If
        End If
        If BorLite.NoACPBSVCall = False Then
            While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False And SP.Q.Check4Text(1, 72, "TCX0C") = False
                SP.Q.Hit("F12")
            End While
        End If

        If SP.Q.Check4Text(1, 74, "TCX0D") Then
            SP.Q.PutText(15, 40, "3", True)
            TestStr = ""
            Do While InStr(TestStr, "F4=PYOFF") = 0
                SP.Q.Hit("F2")
                TestStr = SP.Q.GetText(24, 2, 80)
            Loop
            SP.Q.Hit("F4")
            SP.Q.PutText(7, 26, Format(Today, "MMddyy"))
            SP.Q.PutText(9, 16, "X")
            SP.Q.PutText(9, 54, "Y", True)
            SP.Q.Hit("ENTER")
            TotalPayoffAmount = CDbl(SP.Q.GetText(12, 29, 10))
            TotalPrincipalDue = (CDbl(SP.Q.GetText(14, 29, 10)))
            TotalInterestDue = CDbl(SP.Q.GetText(15, 29, 10))
            TotalDailyInterest = CDbl(SP.Q.GetText(17, 29, 10))
            TotalPastDueAmount = CDbl(SP.Q.GetText(18, 29, 10))
            TotalLateFeesDue = CDbl(SP.Q.GetText(19, 29, 10))

            SP.Q.Hit("F12")
            SP.Q.Hit("F12")

            NextDDAndStatRate()

            SP.Q.Hit("F12")
        End If
    End Sub

    Sub NextDDAndStatRate()

        Dim StatRateRow As Integer
        Dim StatRateSubRow As Integer

        If SP.Q.Check4Text(1, 72, "TCX0G") Then
            While SP.Q.Check4Text(24, 37, "INT") = False
                SP.Q.Hit("F2")
            End While
            SP.Q.Hit("F5")
        Else
            While SP.Q.Check4Text(24, 52, "INT") = False
                SP.Q.Hit("F2")
            End While
            SP.Q.Hit("F7")
        End If

        'init array
        ReDim StatRateData(1, 0)
        If SP.Q.Check4Text(1, 71, "TSX07") Then
            StatRateData(0, 0) = "0001"
            'get rate
            StatRateSubRow = 11
            While True
                If SP.Q.Check4Text(StatRateSubRow, 46, "A") And CDate(SP.Q.GetText(StatRateSubRow, 17, 10).Replace(" ", "/")) <= Today And CDate(SP.Q.GetText(StatRateSubRow, 29, 10).Replace(" ", "/")) >= Today Then
                    StatRateData(1, 0) = SP.Q.GetText(StatRateSubRow, 74, 5)
                    Exit While
                End If
                StatRateSubRow = StatRateSubRow + 1
                If SP.Q.Check4Text(StatRateSubRow, 3, " ") Then
                    StatRateSubRow = 11
                    SP.Q.Hit("F8")
                End If
            End While
            ReDim Preserve StatRateData(1, StatRateData.GetUpperBound(1) + 1)
        ElseIf SP.Q.Check4Text(1, 72, "TSX05") Then
            StatRateRow = 8
            While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                StatRateData(0, StatRateData.GetUpperBound(1)) = "0" & SP.Q.GetText(StatRateRow, 47, 3) 'collect seq number
                SP.Q.RIBM.MoveCursor(21, 18)
                SP.Q.Hit("End") 'blank sel field
                SP.Q.PutText(21, 18, SP.Q.GetText(StatRateRow, 3, 3), True) 'select row
                'get rate
                StatRateSubRow = 11
                While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    If SP.Q.Check4Text(StatRateSubRow, 46, "A") And CDate(SP.Q.GetText(StatRateSubRow, 17, 10).Replace(" ", "/")) <= Today And CDate(SP.Q.GetText(StatRateSubRow, 29, 10).Replace(" ", "/")) >= Today Then
                        StatRateData(1, StatRateData.GetUpperBound(1)) = SP.Q.GetText(StatRateSubRow, 74, 5)
                        ReDim Preserve StatRateData(1, StatRateData.GetUpperBound(1) + 1)
                        SP.Q.Hit("F12")
                        Exit While
                    End If
                    StatRateSubRow = StatRateSubRow + 1
                    If SP.Q.Check4Text(StatRateSubRow, 3, " ") Then
                        StatRateSubRow = 11
                        SP.Q.Hit("F8")
                    End If
                End While
                StatRateRow = StatRateRow + 1
                If SP.Q.Check4Text(StatRateRow, 4, " ") Then
                    StatRateRow = 8
                    SP.Q.Hit("F8")
                End If
            End While
        End If
        SP.Q.Hit("F12")
    End Sub

    Public Function FindACPLoanData(ByVal ASHP As Form) As Boolean
        'This function will gather Loan information return true if it Finds the TCX0D, TCX06, TCX0I or TCX0A screens, or return False if it finds a screen it doesnt recognize.
        If BorLite.NoACPBSVCall Then Exit Function
        While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False And SP.Q.Check4Text(1, 72, "TCX0C") = False
            If (ContactCode = "03" Or ContactCode = "04") And (ActivityCode = "TC" Or ActivityCode = "EM") Then 'BORROWER CONTACT
                TCX04SelectionValue = "6"
            ElseIf AttemptType = "No Answer" Then
                TCX04SelectionValue = "1"
            ElseIf AttemptType = "Answering Machine/Service" Then
                TCX04SelectionValue = "2"
            ElseIf AttemptType = "Wrong Number" Then
                TCX04SelectionValue = "3"
            ElseIf AttemptType = "Phone Busy" Then
                TCX04SelectionValue = "4"
            ElseIf AttemptType = "Disconnected Phone/Out of Service" Then
                TCX04SelectionValue = "5"
            Else '3RD PARTY CONTACT
                TCX04SelectionValue = "9"
            End If
            'depending on which screen is encountered gather info and hit F12 for the next screen
            If SP.Q.Check4Text(1, 74, "TCX02") Then
                'Enter gathered info
                If SP.Q.Check4Text(8, 2, "_") Then
                    SP.Q.PutText(8, 2, "X")
                    BorLoanStatus = SP.Q.GetText(14, 4, 7)
                    CurrPrinBal = SP.Q.GetText(14, 11, 12)
                    InterestRate = SP.Q.GetText(14, 23, 6)
                    DisbursementDate = SP.Q.GetText(14, 30, 8)
                    LoanProgram = SP.Q.GetText(14, 50, 6)
                    'DueDate = SP.Q.GetText(14, 57, 10)
                    AmountDue = SP.Q.GetText(14, 67, 8)
                    'Bor.NumDaysDelinquent = sp.q.GetText(14, 77, 3)
                Else
                    BorLoanStatus = SP.Q.GetText(13, 4, 7)
                    CurrPrinBal = SP.Q.GetText(13, 11, 12)
                    InterestRate = SP.Q.GetText(13, 23, 6)
                    DisbursementDate = SP.Q.GetText(13, 30, 8)
                    LoanProgram = SP.Q.GetText(13, 50, 6)
                    'DueDate = SP.Q.GetText(13, 57, 10)
                    AmountDue = SP.Q.GetText(13, 67, 8)
                    'Bor.NumDaysDelinquent = sp.q.GetText(13, 77, 3)
                End If
            ElseIf SP.Q.Check4Text(1, 74, "TCX04") Then
                'Enter gathered info
                SP.Q.PutText(22, 35, TCX04SelectionValue)
                SP.Q.Hit("Enter")
                If SP.Q.Check4Text(23, 2, "02110") = False Then
                    Select Case TCX04SelectionValue
                        Case "1"
                            Dim comments As New frmGetComment
                            SP.Processing.Visible = False
                            comments.ShowDialog("No Answer")
                            SP.Processing.Visible = True
                            CType(ASHP, frmASHomePage).CloseAllSubForms()
                            CType(ASHP, frmASHomePage).Close()
                            CType(ASHP, frmASHomePage).Demographic.UpdateSys()
                            SP.Processing.Visible = False
                            SP.UsrInf.ReturnToFavoriteScreen()
                            Exit Function
                        Case "2"
                            Dim comments As New frmGetComment
                            SP.Processing.Visible = False
                            comments.ShowDialog("Answering Machine/Service")
                            SP.Processing.Visible = True
                            CType(ASHP, frmASHomePage).CloseAllSubForms()
                            CType(ASHP, frmASHomePage).Close()
                            CType(ASHP, frmASHomePage).Demographic.UpdateSys()
                            SP.Processing.Visible = False
                            SP.UsrInf.ReturnToFavoriteScreen()
                            Exit Function
                        Case "3"
                            Dim comments As New frmGetComment
                            SP.Processing.Visible = False
                            comments.ShowDialog("Wrong Number")
                            SP.Processing.Visible = True
                            CType(ASHP, frmASHomePage).CloseAllSubForms()
                            CType(ASHP, frmASHomePage).Close()
                            CType(ASHP, frmASHomePage).Demographic.UpdateSys()
                            SP.Processing.Visible = False
                            SP.UsrInf.ReturnToFavoriteScreen()
                            Exit Function
                        Case "4"
                            Dim comments As New frmGetComment
                            SP.Processing.Visible = False
                            comments.ShowDialog("Phone Busy")
                            SP.Processing.Visible = True
                            CType(ASHP, frmASHomePage).CloseAllSubForms()
                            CType(ASHP, frmASHomePage).Close()
                            CType(ASHP, frmASHomePage).Demographic.UpdateSys()
                            SP.Processing.Visible = False
                            SP.UsrInf.ReturnToFavoriteScreen()
                            Exit Function
                        Case "5"
                            Dim comments As New frmGetComment
                            SP.Processing.Visible = False
                            comments.ShowDialog("Phone Out of Service/Disconnected")
                            SP.Processing.Visible = True
                            CType(ASHP, frmASHomePage).CloseAllSubForms()
                            CType(ASHP, frmASHomePage).Close()
                            CType(ASHP, frmASHomePage).Demographic.UpdateSys()
                            SP.Processing.Visible = False
                            SP.UsrInf.ReturnToFavoriteScreen()
                            Exit Function
                    End Select
                End If
            ElseIf SP.Q.Check4Text(1, 72, "TCX14") Then
                'Enter gathered info
                If SP.Q.GetText(10, 6, 2) = "BK" Then
                    TCX14SelectionValue = "1"
                ElseIf SP.Q.GetText(16, 6, 2) = "BK" Then
                    TCX14SelectionValue = "2"
                Else
                    TCX14SelectionValue = ""
                End If
                SP.Q.PutText(22, 13, TCX14SelectionValue)
            ElseIf SP.Q.Check4Text(1, 72, "TCX0A") Then
                Dim paymentAmount As String = SP.Q.GetText(13, 49, 12)
                Dim message As String = "Borrower must: Return a signed repayment obligation in order to be eligible for deferment or forbearance, and/or make full monthly payments of " + paymentAmount
                MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SP.Q.Hit("Enter")
            Else
                SP.frmWhoaDUDE.WhoaDUDE("Whoa! That's like a totally new screen form me. Im gona need some help with this.  Please contact Systems Support.", "Knarly DUDE", True)
                Return False
                Exit Function
            End If
            SP.Q.Hit("Enter") 'move to the next screen
            If SP.Q.Check4Text(23, 2, "02110") Then '02110 SELECTION INVALID FOR INCOMING CALL
                BorLite.ACPSelection = "2"
                ReturnToACP()
            End If
        End While
        If SP.Q.Check4Text(1, 74, "TCX0D") Then
            FirstScreen = "TCX0D"
        End If
        If SP.Q.Check4Text(1, 72, "TCX06") Then
            FirstScreen = "TCX06"
        End If
        If SP.Q.Check4Text(1, 72, "TCX0I") Then
            FirstScreen = "TCX0I"
        End If
        If SP.Q.Check4Text(1, 72, "TCX0C") Then
            FirstScreen = "TCX0C"
        End If
        Return True
    End Function

    Public Sub TPDDCheck()
        SP.Q.FastPath("TPDD" & SSN)
        If SP.Q.Check4Text(2, 19, "PERSONAL DEMOGRAPHIC") Then
            HasTPDD = True
        Else
            HasTPDD = False
        End If
    End Sub

    Public Sub Turbo()
        GetLG10()
        GetACH()
        GetTimes20DayLetter()
        GetOnTime48()
        GetITS0N()
        GetTS2X()
        GetTD0L()
        GetAddPhoneSource()
        'GetRefereces()
        Thread.CurrentThread.Abort()
    End Sub

    Private Sub GetTimes20DayLetter()
        Dim x As Integer
        SP.Q.FastPath("TX3Z/ITD2A" & SSN)
        'Obtain the Date Last Contact 
        SP.Q.PutText(10, 65, "CNTCT", True)
        If SP.Q.Check4Text(23, 2, "01019") Then
            'never had a contact with borrower
        Else
            If SP.Q.Check4Text(1, 72, "TDX2C") Then
                DateLastCntct = SP.Q.GetText(7, 4, 8)
            ElseIf SP.Q.Check4Text(1, 72, "TDX2D") Then
                DateLastCntct = SP.Q.GetText(15, 31, 8)
            End If
            SP.Q.Hit("F12")
        End If
        'Obtain the Date Last Attempt 
        SP.Q.FastPath("TX3Z/ITD2A" + SSN)
        SP.Q.PutText(10, 65, "NOCTC", True)
        If SP.Q.Check4Text(23, 2, "01019") Then
            'we have not attempted to contact the borrower previously
        Else
            If SP.Q.Check4Text(1, 72, "TDX2C") Then
                DateLastAtempt = SP.Q.GetText(7, 4, 8)
            ElseIf SP.Q.Check4Text(1, 72, "TDX2D") Then
                DateLastAtempt = SP.Q.GetText(15, 31, 8)
            End If
            SP.Q.Hit("F12")
        End If
        SP.Q.FastPath("TX3Z/ITD2A" + SSN)
        'obtain the # Times 20-Day Letter
        SP.Q.PutText(11, 65, "DL200", True)
        If SP.Q.Check4Text(23, 2, "01019") Then
            'the borrower has never been sent a 20-Day Letter, and the # Times 20-Day Letter value is ‘0’
            Num20Day = "0"

        Else
            If SP.Q.Check4Text(1, 72, "TDX2C") Then
                SP.Q.PutText(5, 14, "X", True)
            End If
            Num20Day = SP.Q.GetText(5, 67, 2)
            'Hit F8 to page through up to 4 records to store for occurrences of a 20-Day Letter being sent to the borrower
            Date20Day.SetValue(SP.Q.GetText(13, 31, 8), 1)
            For x = 1 To 3
                SP.Q.Hit("F8")
                If SP.Q.Check4Text(23, 2, "90007") Then
                    Exit For
                End If
                Date20Day.SetValue(SP.Q.GetText(13, 31, 8), x + 1)
            Next
            SP.Q.Hit("F12")
        End If

    End Sub

    Private Sub GetOnTime48()
        Dim x As Integer
        Dim rec As Integer
        Dim GTZ As Boolean 'Greater Than Zero
        Dim errormsg As Boolean
        SP.Q.Hit("F8")
        GTZ = False
        errormsg = False
        OnTime48Eligible = False
        SP.Q.FastPath("TX3Z/ITS26" & SSN)
        If SP.Q.Check4Text(1, 72, "TSX28") Then
            Do While SP.Q.Check4Text(23, 2, "90007") = False
                rec = 7
                Do While SP.Q.GetText(rec + 1, 59, 10) <> ""
                    rec += 1
                    SP.Q.PutText(21, 12, "")
                    SP.Q.Hit("END")
                    If IsNumeric(SP.Q.GetText(rec, 59, 10)) Then
                        If CDbl(SP.Q.GetText(rec, 59, 10)) > 0 Then
                            GTZ = True
                            SP.Q.PutText(21, 12, SP.Q.GetText(rec, 2, 2), True)

                            'Get Loan Types and Cohort status and delinquency dates
                            ReDim Preserve Loans(3, Loans.GetUpperBound(1) + 1)
                            Loans(0, Loans.GetUpperBound(1)) = SP.Q.GetText(6, 66, 6) 'loan type
                            Loans(1, Loans.GetUpperBound(1)) = SP.Q.GetText(17, 44, 8) 'repayment start date
                            SP.Q.Hit("ENTER")
                            SP.Q.Hit("ENTER")
                            Loans(2, Loans.GetUpperBound(1)) = SP.Q.GetText(9, 46, 3) 'days delinquent 
                            If Loans(2, Loans.GetUpperBound(1)) = "" Then Loans(2, Loans.GetUpperBound(1)) = "0"
                            Loans(3, Loans.GetUpperBound(1)) = SP.Q.GetText(9, 69, 8) 'date delinquent 
                            SP.Q.Hit("F12")
                            SP.Q.Hit("F12")

                            ReDim Preserve BorrStat(BorrStat.GetUpperBound(0) + 1)
                            BorrStat(BorrStat.GetUpperBound(0)) = SP.Q.GetText(3, 10, 20)
                            'End If
                            SP.Q.Hit("F12")
                        End If
                    End If
                Loop
                'If errormsg Then Exit Do
                SP.Q.Hit("F8")
            Loop
            If GTZ = False Then
                ReDim Preserve BorrStat(BorrStat.GetUpperBound(0) + 1)
                BorrStat(BorrStat.GetUpperBound(0)) = "PIF/Deconverted"
            End If
        ElseIf SP.Q.Check4Text(1, 72, "TSX29") Then
            GTZ = True
            'Get Loan Types and Cohort status and delinquency dates
            ReDim Preserve Loans(3, Loans.GetUpperBound(1) + 1)
            Loans(0, Loans.GetUpperBound(1)) = SP.Q.GetText(6, 66, 6) 'loan type
            Loans(1, Loans.GetUpperBound(1)) = SP.Q.GetText(17, 44, 8) 'repayment start date
            SP.Q.Hit("ENTER")
            SP.Q.Hit("ENTER")
            Loans(2, Loans.GetUpperBound(1)) = SP.Q.GetText(9, 46, 3) 'days delinquent 
            If Loans(2, Loans.GetUpperBound(1)) = "" Then Loans(2, Loans.GetUpperBound(1)) = "0"
            Loans(3, Loans.GetUpperBound(1)) = SP.Q.GetText(9, 69, 8) 'date delinquent 
            SP.Q.Hit("F12")
            SP.Q.Hit("F12")
            ReDim Preserve BorrStat(BorrStat.GetUpperBound(0) + 1)
            BorrStat(BorrStat.GetUpperBound(0)) = SP.Q.GetText(3, 10, 20)
            'End If
            SP.Q.Hit("F12")
            If GTZ = False Then
                ReDim Preserve BorrStat(BorrStat.GetUpperBound(0) + 1)
                BorrStat(BorrStat.GetUpperBound(0)) = "PIF/Deconverted"
            End If
        End If

        'find the most delinquent loan from ITS26
        NumDaysDelinquent = 0
        DateDelinquencyOccurred = ""
        For x = 1 To Loans.GetUpperBound(1)
            If CInt(NumDaysDelinquent) < CInt(Loans(2, x)) Then
                NumDaysDelinquent = Loans(2, x)
                DateDelinquencyOccurred = Loans(3, x)
            End If
        Next x
    End Sub


    Private Sub GetTD0L()
        AmountPastDue = 0
        CurrentDueAmount = 0
        OutstandingLateFees = 0
        TotalAmountDue = 0
        SP.Q.FastPath("TX3Z/ITD0L" & SSN)
        If SP.Q.Check4Text(1, 72, "TDX0M") Then
            SP.Q.Hit("F6")
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                AmountPastDue = AmountPastDue + CDbl(SP.Q.GetText(15, 33, 10))
                TotalAmountDue = TotalAmountDue + CDbl(SP.Q.GetText(15, 67, 10)) + CDbl(SP.Q.GetText(16, 67, 10))
                OutstandingLateFees = OutstandingLateFees + CDbl(SP.Q.GetText(16, 67, 10))
                SP.Q.Hit("F8")
            Loop
        End If
        CurrentDueAmount = TotalAmountDue - AmountPastDue
    End Sub

    Private Sub GetTS2X()
        Dim x As Integer
        SP.Q.FastPath("TX3Z/ITS2X" & SSN)
        If SP.Q.Check4Text(1, 72, "TSX2Y") Then
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For x = 8 To 20
                    If SP.Q.Check4Text(x, 7, "A") Then
                        ReDim Preserve Installments(2, Installments.GetUpperBound(1) + 1)
                        Installments(0, Installments.GetUpperBound(1)) = SP.Q.GetText(x, 15, 10) 'install amount
                        Installments(1, Installments.GetUpperBound(1)) = SP.Q.GetText(x, 40, 8) '1st due date
                    End If
                Next
                SP.Q.Hit("F8")
            Loop
        ElseIf SP.Q.Check4Text(1, 71, "TSX3W") Then
            If SP.Q.Check4Text(7, 44, "A") Then
                ReDim Preserve Installments(2, Installments.GetUpperBound(1) + 1)
                Installments(0, Installments.GetUpperBound(1)) = Replace(SP.Q.GetText(9, 24, 10), "$", "") 'install amount
                Installments(1, Installments.GetUpperBound(1)) = SP.Q.GetText(7, 19, 8) '1st due date
            End If
        End If
    End Sub
    Sub GetITS0N()
        SP.Q.FastPath("TX3Z/ITS0N" & SSN)
        Dim TempDT As DateTime
        Dim x As Integer
        Dim x2 As Integer
        Dim wasZero As Boolean
        If MonthlyPA = 0 Then
            wasZero = True
        Else
            wasZero = False
        End If
        If SP.Q.Check4Text(1, 72, "TSX0S") Then
            HasRepaymentSchedule = "Y"
            If wasZero Then
                Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    For x = 11 To 22
                        If SP.Q.GetText(x, 54, 11) <> "" Then
                            MonthlyPA = MonthlyPA + SP.Q.GetText(x, 54, 11)
                        End If
                    Next
                    SP.Q.Hit("F8")
                Loop
            End If
            DueDay = SP.Q.GetText(11, 47, 2)

            If CInt(SP.Q.GetText(11, 47, 2)) <= Now.Day Then
                TempDT = Now.AddMonths(1)
                NextDueDate = TempDT.Month & "/" & SP.Q.GetText(11, 47, 2) & "/" & TempDT.Year
            Else
                NextDueDate = Now.Month & "/" & SP.Q.GetText(11, 47, 2) & "/" & Now.Year
            End If
        ElseIf SP.Q.Check4Text(1, 72, "TSX0P") Or SP.Q.Check4Text(1, 72, "TSX0Q") Then
            HasRepaymentSchedule = "Y"
            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                For x2 = 8 To 20
                    If SP.Q.GetText(x2, 4, 2) <> "" Then
                        SP.Q.PutText(21, 12, CInt(SP.Q.GetText(x2, 4, 2)).ToString("00"), True)
                        If SP.Q.Check4Text(1, 72, "TSX0S") Then
                            If wasZero Then
                                Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                                    For x = 11 To 22
                                        If SP.Q.GetText(x, 54, 11) <> "" Then
                                            MonthlyPA = MonthlyPA + SP.Q.GetText(x, 54, 11)
                                        End If
                                    Next
                                    SP.Q.Hit("F8")
                                Loop
                            End If
                            DueDay = SP.Q.GetText(11, 47, 2)
                            If CInt(SP.Q.GetText(11, 47, 2)) <= Now.Day Then
                                TempDT = Now.AddMonths(1)
                                NextDueDate = TempDT.Month & "/" & SP.Q.GetText(11, 47, 2) & "/" & TempDT.Year
                            Else
                                NextDueDate = Now.Month & "/" & SP.Q.GetText(11, 47, 2) & "/" & Now.Year
                            End If
                            SP.Q.Hit("F12")
                        ElseIf SP.Q.Check4Text(1, 72, "TSX0Q") Then
                            Dim Z As Integer
                            Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                                For Z = 8 To 20
                                    If SP.Q.GetText(Z, 18, 2) <> "" Then
                                        SP.Q.PutText(21, 12, CInt(SP.Q.GetText(Z, 18, 2)).ToString("00"), True)
                                        If SP.Q.Check4Text(1, 72, "TSX0S") Then
                                            If wasZero Then
                                                Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                                                    For x = 11 To 22
                                                        If SP.Q.GetText(x, 54, 11) <> "" Then
                                                            MonthlyPA = MonthlyPA + SP.Q.GetText(x, 54, 11)
                                                        End If
                                                    Next
                                                    SP.Q.Hit("F8")
                                                Loop
                                            End If
                                            DueDay = SP.Q.GetText(11, 47, 2)
                                            If CInt(SP.Q.GetText(11, 47, 2)) <= Now.Day Then
                                                TempDT = Now.AddMonths(1)
                                                NextDueDate = TempDT.Month & "/" & SP.Q.GetText(11, 47, 2) & "/" & TempDT.Year
                                            Else
                                                NextDueDate = Now.Month & "/" & SP.Q.GetText(11, 47, 2) & "/" & Now.Year
                                            End If
                                            SP.Q.Hit("F12")
                                        Else
                                            SP.Q.FastPath("TX3Z/ITS0N" & SSN)
                                            SP.Q.PutText(21, 12, CInt(SP.Q.GetText(x2, 4, 2)).ToString("00"), True)
                                        End If
                                    End If
                                Next
                                SP.Q.Hit("F8")
                            Loop
                            SP.Q.Hit("F12")
                        Else
                            SP.Q.FastPath("TX3Z/ITS0N" & SSN)
                        End If
                    End If
                Next
                SP.Q.Hit("F8")
            Loop
        Else
            HasRepaymentSchedule = "N"
            NextDueDate = ""
            DueDay = ""
        End If
        If SP.Q.GetText(11, 47, 2) = "" Then

        Else

        End If
    End Sub

    Private Sub GetAddPhoneSource()
        SP.Q.FastPath("TX3Z/ITX1J;" & SSN)
        AddressSource = TranslateSourceCode(SP.Q.GetText(8, 18, 2))
        PhoneSource = TranslateSourceCode(SP.Q.GetText(19, 14, 2))
    End Sub

    Private Function TranslateSourceCode(ByVal cd As String) As String

        If cd = "01" Then
            Return "ELECTRONIC MEDIA"
        ElseIf cd = "02" Then
            Return "APPLICATION"
        ElseIf cd = "03" Then
            Return "PROMISSORY NOTE"
        ElseIf cd = "04" Then
            Return "APP/PROM NOTE"
        ElseIf cd = "05" Then
            Return "CORRESPONDENCE"
        ElseIf cd = "06" Then
            Return "REPAYMENT OBLIGATION"
        ElseIf cd = "07" Then
            Return "CORRECT FORM"
        ElseIf cd = "08" Then
            Return "DEFERMENT FORM"
        ElseIf cd = "09" Then
            Return "FORBEARANCE FORM"
        ElseIf cd = "10" Then
            Return "GUARANTEE STATEMENT"
        ElseIf cd = "11" Then
            Return "CREDIT REPORT"
        ElseIf cd = "12" Then
            Return "PARENT"
        ElseIf cd = "13" Then
            Return "SPOUSE"
        ElseIf cd = "14" Then
            Return "SIBLING"
        ElseIf cd = "15" Then
            Return "ROOMMATE"
        ElseIf cd = "16" Then
            Return "NEIGHBOR"
        ElseIf cd = "17" Then
            Return "CERTIFIED MAIL"
        ElseIf cd = "18" Then
            Return "AUNT/UNCLE"
        ElseIf cd = "19" Then
            Return "GRANDPARENT"
        ElseIf cd = "20" Then
            Return "COUSIN"
        ElseIf cd = "21" Then
            Return "NIECE/NEPHEW"
        ElseIf cd = "22" Then
            Return "CHILD"
        ElseIf cd = "23" Then
            Return "EMPLOYER"
        ElseIf cd = "24" Then
            Return "DIRECTORY ASSISTANCE"
        ElseIf cd = "25" Then
            Return "POST OFFICE"
        ElseIf cd = "26" Then
            Return "DEPART MOTOR VEHICLE"
        ElseIf cd = "27" Then
            Return "LANDLORD"
        ElseIf cd = "28" Then
            Return "MILITARY"
        ElseIf cd = "29" Then
            Return "IRS"
        ElseIf cd = "30" Then
            Return "OUTSIDE COLLECTOR"
        ElseIf cd = "31" Then
            Return "UNKNOWN"
        ElseIf cd = "32" Then
            Return "INFO FROM BRWR"
        ElseIf cd = "33" Then
            Return "RETURNED EMAIL"
        ElseIf cd = "41" Then
            Return "BORROWER PHONE CALL"
        ElseIf cd = "42" Then
            Return "2ND PRTY PHONE CALL"
        ElseIf cd = "43" Then
            Return "3RD PARTY PHONE CALL"
        ElseIf cd = "44" Then
            Return "PRISON"
        ElseIf cd = "45" Then
            Return "FRIEND"
        ElseIf cd = "46" Then
            Return "STUDENT"
        ElseIf cd = "47" Then
            Return "PAROLE"
        ElseIf cd = "48" Then
            Return "REMITTANCE CHECK"
        ElseIf cd = "49" Then
            Return "COUPON STATEMENT"
        ElseIf cd = "50" Then
            Return "FORMER EMPLOYER"
        ElseIf cd = "51" Then
            Return "BAR ASSOCIATION"
        ElseIf cd = "52" Then
            Return "VR MAILBX"
        ElseIf cd = "54" Then
            Return "ACS"
        ElseIf cd = "55" Then
            Return "WEBMASTER"
        ElseIf cd = "56" Then
            Return "GUARANTOR"
        ElseIf cd = "58" Then
            Return "NSLC"
        ElseIf cd = "97" Then
            Return "CRC"
        ElseIf cd = "98" Then
            Return "CAM"
        ElseIf cd = "99" Then
            Return "ANNUAL STATEMENT"
        Else
            Return ""
        End If
    End Function

    'write demo info to text file
    Public Sub WriteOut()
        FileOpen(1, "T:\TempDemoUpdate.txt", OpenMode.Output) ' Open file for output.
        Write(1, SSN, FirstName, MI, LastName, UserProvidedDemos.Addr1, UserProvidedDemos.Addr2, _
              UserProvidedDemos.City, UserProvidedDemos.State, UserProvidedDemos.Zip, UserProvidedDemos.HomePhoneNum, _
              UserProvidedDemos.OtherPhoneNum, "", UserProvidedDemos.OtherPhoneNum, UserProvidedDemos.HomePhoneExt, _
              UserProvidedDemos.OtherPhoneExt, UserProvidedDemos.OtherPhone2Ext, UserProvidedDemos.Email, CLAccNum, _
              NumDaysDelinquent, DateDelinquencyOccurred, Num20Day, AmountPastDue, CurrentDueAmount, TotalAmountDue, _
              OutstandingLateFees, TotalAmountDue + OutstandingLateFees, TotalPrincipalDue, TotalInterestDue, _
              DueDay, NextDueDate, DateLastPaymentReceived, MonthlyPA, ACHData.HasACH, HasRepaymentSchedule, _
              UserProvidedDemos.UPEmailVal.ToString)
        FileClose(1)
    End Sub

End Class
