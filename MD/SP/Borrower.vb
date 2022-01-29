Imports Q
Imports System.Collections.Generic

Public Class Borrower
    Inherits MDBorrower

    'constructor for borrower object
    Public Sub New(ByVal tBorLite As BorrowerLite)
        BorLite = tBorLite
        SSN = BorLite.SSN
        ActivityCmts = New SP.ActivityComments(SSN)
    End Sub

    Public Sub GetDemographicsFrmSystem()
        DemographicsVerified = False
        BorrowerIsLCOOnly = False
        'gather demographics info
        OneLINKDemos = New OneLINKDemographics(SSN)
        OneLINKDemos.PopulateObjectFromSystem()
        CompassDemos = New COMPASSDemographics(SSN)
        CompassDemos.PopulateObjectFromSystem()
        If OneLINKDemos.FoundOnSystem = False And CompassDemos.FoundOnSystem = False Then
            'if demographics are gathered from LCO then they are placed directly in the user updated object
            UserProvidedDemos = New LCODemographics(SSN)
            UserProvidedDemos.PopulateObjectFromSystem()
            If UserProvidedDemos.FoundOnSystem Then BorrowerIsLCOOnly = True
            CLAccNum = UserProvidedDemos.CLAccNum 'get needed items back
            DOB = UserProvidedDemos.DOB
            Name = UserProvidedDemos.Name
            FirstName = UserProvidedDemos.FirstName
            LastName = UserProvidedDemos.LastName
            MI = UserProvidedDemos.MI
        Else
            'create user provided demos object for system updates
            UserProvidedDemos = New Demographics(SSN)
            If OneLINKDemos.FoundOnSystem Then
                CLAccNum = OneLINKDemos.CLAccNum 'get needed items back
                DOB = OneLINKDemos.DOB
                Name = OneLINKDemos.Name
                FirstName = OneLINKDemos.FirstName
                LastName = OneLINKDemos.LastName
                MI = OneLINKDemos.MI
            Else
                CLAccNum = CompassDemos.CLAccNum 'get needed items back
                DOB = CompassDemos.DOB
                Name = CompassDemos.Name
                FirstName = CompassDemos.FirstName
                LastName = CompassDemos.LastName
                MI = CompassDemos.MI
            End If
        End If
        CheckPOBox()
    End Sub

    Public Function CheckPOBox() As Boolean
        'do PO box check
        CheckPOBoxLG0H()
        If POBoxAllowed = False Then
            'only check for trumping if CheckPOBoxLG0H call resulted in assignment of false
            CheckPOBoxLP50() 'check for action that trumps LG0H functionality
        End If
    End Function

    Protected Sub CheckPOBoxLG0H()
        Dim DsbDt As String
        SP.Q.FastPath("LG0HI;" & SSN)
        If SP.Q.Check4Text(22, 3, "47004") Then '47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA
            POBoxAllowed = False
            Exit Sub
        End If
        If SP.Q.Check4Text(1, 53, "DISBURSEMENT ACTIVITY SELECT") Then
            SP.Q.PutText(21, 11, "01", True)
        End If
        If SP.Q.Check4Text(1, 52, "DISBURSEMENT ACTIVITY DISPLAY") Then
            Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                'disbursement #1
                If Check4Text(18, 20, "MMDDCCYY") = False Then 'check if disbursement should even be evaluated
                    DsbDt = SP.Q.GetText(12, 20, 2) & "/" & SP.Q.GetText(12, 22, 2) & "/" & SP.Q.GetText(12, 24, 4)
                    If IsDate(DsbDt) Then
                        If CDate(DsbDt) <= Today Then
                            POBoxAllowed = True
                        Else
                            POBoxAllowed = False
                            Exit Sub
                        End If
                    Else
                        If DsbDt = "MM/DD/CCYY" Then
                            'If the ACT DSB DT is ‘MMDDCCYY’ and the Cancel Amt is equal to the Disb Amt Do not consider this loan for determining PO Box Allowance
                            If SP.Q.GetText(13, 18, 10) <> SP.Q.GetText(11, 18, 10) Then
                                POBoxAllowed = False
                                Exit Sub
                            End If
                        End If
                    End If
                End If
                'disbursement #2
                If Check4Text(18, 36, "MMDDCCYY") = False Then 'check if disbursement should even be evaluated
                    DsbDt = SP.Q.GetText(12, 36, 2) & "/" & SP.Q.GetText(12, 38, 2) & "/" & SP.Q.GetText(12, 40, 4)
                    If IsDate(DsbDt) Then
                        If CDate(DsbDt) <= Today Then
                            POBoxAllowed = True
                        Else
                            POBoxAllowed = False
                            Exit Sub
                        End If
                    Else
                        If DsbDt = "MM/DD/CCYY" Then
                            'If the ACT DSB DT is ‘MMDDCCYY’ and the Cancel Amt is equal to the Disb Amt Do not consider this loan for determining PO Box Allowance
                            If SP.Q.GetText(13, 34, 10) <> SP.Q.GetText(11, 34, 10) Then
                                POBoxAllowed = False
                                Exit Sub
                            End If
                        End If
                    End If
                End If
                'disbursement #3
                If Check4Text(18, 52, "MMDDCCYY") = False Then 'check if disbursement should even be evaluated
                    DsbDt = SP.Q.GetText(12, 52, 2) & "/" & SP.Q.GetText(12, 54, 2) & "/" & SP.Q.GetText(12, 56, 4)
                    If IsDate(DsbDt) Then
                        If CDate(DsbDt) <= Today Then
                            POBoxAllowed = True
                        Else
                            POBoxAllowed = False
                            Exit Sub
                        End If
                    Else
                        If DsbDt = "MM/DD/CCYY" Then
                            'If the ACT DSB DT is ‘MMDDCCYY’ and the Cancel Amt is equal to the Disb Amt Do not consider this loan for determining PO Box Allowance
                            If SP.Q.GetText(13, 50, 10) <> SP.Q.GetText(11, 50, 10) Then
                                POBoxAllowed = False
                                Exit Sub
                            End If
                        End If
                    End If
                End If
                'disbursement #4
                If Check4Text(18, 68, "MMDDCCYY") = False Then 'check if disbursement should even be evaluated
                    DsbDt = SP.Q.GetText(12, 68, 2) & "/" & SP.Q.GetText(12, 70, 2) & "/" & SP.Q.GetText(12, 72, 4)
                    If IsDate(DsbDt) Then
                        If CDate(DsbDt) <= Today Then
                            POBoxAllowed = True
                        Else
                            POBoxAllowed = False
                            Exit Sub
                        End If
                    Else
                        If DsbDt = "MM/DD/CCYY" Then
                            'If the ACT DSB DT is ‘MMDDCCYY’ and the Cancel Amt is equal to the Disb Amt Do not consider this loan for determining PO Box Allowance
                            If SP.Q.GetText(13, 66, 10) <> SP.Q.GetText(11, 66, 10) Then
                                POBoxAllowed = False
                                Exit Sub
                            End If
                        End If
                    End If
                End If
                SP.Hit("F8")
            Loop
        End If
    End Sub

    Protected Sub CheckPOBoxLP50()
        SP.Q.FastPath("LP50I" & SSN & ";;;;;POBOX")
        If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
            Exit Sub
        End If
        If SP.Q.Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY") Or SP.Q.Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then
            'found POBox record
            POBoxAllowed = True
        End If
    End Sub

    'mulit threaded from home pages in turbo functions
    Public Sub GetACH()
        Dim row As Integer
        Dim LnRow As Integer
        'ReDim ACHData(2)
        ACHData = New SP.ClassACHInfo 'reset variable
        ReDim ACHData.LnLvlInfo(3, 0)
        ACHData.HasACH = "No" 'init as a no
        ACHData.ACHDataFound = False 'init to false
        'access TS7O
        SP.Q.FastPath("TX3Z/ITS7O" & SSN)

        'check for and select the first approved record found
        If SP.Q.Check4Text(1, 72, "TSX7J") Then
            row = 13
            While Not SP.Q.Check4Text(23, 2, "90007")
                'select the record if it is approved
                If SP.Q.Check4Text(row, 67, "A") Then
                    SP.Q.PutText(22, 17, SP.Q.GetText(row, 3, 2), True)
                    Exit While
                End If
                row = row + 1
                'check for more pages if the last record has been reviewed
                If SP.Q.Check4Text(row, 4, " ") Then
                    SP.Q.Hit("F8")
                    row = 13
                End If
            End While
            'check if still on selection screen
            If SP.Q.Check4Text(1, 72, "TSX7J") Then
                'if still on selection screen then process for D status ACH recs
                row = 21
                While True 'see condition for exiting loop inside loop
                    If SP.Q.Check4Text(row, 67, "D") Then
                        SP.Q.PutText(22, 17, SP.Q.GetText(row, 3, 2), True) 'select record
                        ACHData.ACHDataFound = True 'record with data found
                        'get ACH level information
                        ACHData.HasACH = "No"
                        ACHData.StatusDt = SP.Q.GetText(11, 18, 10)
                        ACHData.DenialReason = SP.Q.GetText(10, 57, 1)
                        ACHData.NSFCounter = Replace(SP.Q.GetText(13, 18, 1), "", "") 'blank if underscore
                        Exit Sub
                    End If
                    row = row - 1
                    If row = 12 Then
                        row = 21
                        SP.Q.Hit("F7")
                        'check if first page is found
                        If SP.Q.Check4Text(23, 2, "90007") Then
                            Exit While 'loop exit condition not found
                        End If
                    End If
                End While
            End If
        End If
        'get data from detail screen if it is displayed and the record is approved
        If SP.Q.Check4Text(1, 72, "TSX7K") Then
            If SP.Q.Check4Text(10, 18, "A") Then
                ACHData.ACHDataFound = True 'record with data found
                'get ACH level information
                If SP.Q.Check4Text(17, 13, " ") Then
                    ACHData.HasACH = "No"
                Else
                    ACHData.HasACH = "Yes"
                End If
                ACHData.StatusDt = SP.Q.GetText(11, 18, 10)
                ACHData.NSFCounter = SP.Q.GetText(13, 18, 1).Replace("_", "") 'blank if underscore
                ACHData.AdditionalWithdrawAmt = SP.Q.GetText(11, 57, 10).Replace("_", "")
                ACHData.RoutingNumber = SP.Q.GetText(6, 18, 14)
                ACHData.AccountNumber = SP.Q.GetText(6, 52, 17)
                'get loan level information
                LnRow = 17
                While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    ACHData.LnLvlInfo(0, ACHData.LnLvlInfo.GetUpperBound(1)) = SP.Q.GetText(LnRow, 11, 4) 'seq num
                    ACHData.LnLvlInfo(1, ACHData.LnLvlInfo.GetUpperBound(1)) = SP.Q.GetText(LnRow, 23, 10) 'frst disb dt
                    ACHData.LnLvlInfo(2, ACHData.LnLvlInfo.GetUpperBound(1)) = SP.Q.GetText(LnRow, 41, 7)  'loan program
                    ACHData.LnLvlInfo(3, ACHData.LnLvlInfo.GetUpperBound(1)) = SP.Q.GetText(LnRow, 58, 3)  'ACH Sq Num
                    ReDim Preserve ACHData.LnLvlInfo(3, ACHData.LnLvlInfo.GetUpperBound(1) + 1) 'add another to array
                    LnRow = LnRow + 1
                    If IsNumeric(SP.Q.GetText(LnRow, 11, 4)) = False Then
                        LnRow = 17
                        SP.Hit("F8")
                    End If
                End While
            End If
        End If

    End Sub

    'mulit threaded from home pages in turbo functions
    Public Sub GetLG10()
        Dim i As Integer
        Dim row As Integer
        'This is realy LG10 no longer LG02

        '1 = loan type
        '2 = servicer code
        '3 = date processed
        '4 = guarantee amount
        '5 = loan status
        '6 = loan status reason code

        'access LG10
        SP.Q.FastPath("LG10I" & SSN)
        'get data from selection screen for each loan
        If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") = False Then
            If SP.Q.Check4Text(1, 75, "SELECT") Then
                ReDim LG10Data(6, 0)
                i = 0
                Do While Not SP.Q.Check4Text(20, 3, "46004")
                    i += 1
                    SP.Q.PutText(19, 15, Strings.Format(i, "00"), True)
                    If SP.Q.Check4Text(20, 3, "47001") Then
                        SP.Q.Hit("F8")
                        i = 0
                    End If
                    If SP.Q.Check4Text(1, 52, "LOAN BWR STATUS RECAP DISPLAY") Then
                        row = 10
                        Do While Not SP.Q.Check4Text(22, 3, "46004")
                            row += 1
                            ReDim Preserve LG10Data(6, UBound(LG10Data, 2) + 1)
                            LG10Data(1, UBound(LG10Data, 2)) = SP.Q.GetText(4, 13, 2)
                            LG10Data(2, UBound(LG10Data, 2)) = SP.Q.GetText(5, 27, 6)
                            LG10Data(3, UBound(LG10Data, 2)) = SP.Q.GetText(row, 26, 8)
                            LG10Data(4, UBound(LG10Data, 2)) = SP.Q.GetText(row, 37, 9)
                            LG10Data(5, UBound(LG10Data, 2)) = SP.Q.GetText(row, 59, 2)
                            LG10Data(6, UBound(LG10Data, 2)) = SP.Q.GetText(row, 64, 2)
                            LG10Data(0, UBound(LG10Data, 2)) = SP.Q.GetText(row, 48, 8)
                            If SP.Q.GetText(row + 1, 4, 5) = "" Then
                                SP.Q.Hit("F8")
                                row = 10
                            End If
                        Loop
                        SP.Q.Hit("F12")
                    End If
                Loop
            Else
                ReDim LG10Data(6, 0)
                If SP.Q.Check4Text(1, 52, "LOAN BWR STATUS RECAP DISPLAY") Then
                    row = 10
                    Do While Not SP.Q.Check4Text(22, 3, "46004")
                        row += 1
                        ReDim Preserve LG10Data(6, UBound(LG10Data, 2) + 1)
                        LG10Data(1, UBound(LG10Data, 2)) = SP.Q.GetText(4, 13, 2)
                        LG10Data(2, UBound(LG10Data, 2)) = SP.Q.GetText(5, 27, 6)
                        LG10Data(3, UBound(LG10Data, 2)) = SP.Q.GetText(row, 26, 8)
                        LG10Data(4, UBound(LG10Data, 2)) = SP.Q.GetText(row, 37, 9)
                        LG10Data(5, UBound(LG10Data, 2)) = SP.Q.GetText(row, 59, 2)
                        LG10Data(6, UBound(LG10Data, 2)) = SP.Q.GetText(row, 64, 2)
                        LG10Data(0, UBound(LG10Data, 2)) = SP.Q.GetText(row, 48, 8)
                        If SP.Q.GetText(row + 1, 4, 5) = "" Then
                            SP.Q.Hit("F8")
                            row = 10
                        End If
                    Loop
                End If
            End If
        End If

        CalculatedBorrowerStatusListFromLG10 = New List(Of String)
        For index As Integer = 0 To UBound(LG10Data, 2)
            If Not CalculatedBorrowerStatusListFromLG10.Contains(LG10Data(5, index)) Then
                CalculatedBorrowerStatusListFromLG10.Add(LG10Data(5, index))
            End If
        Next


        SP.Q.FastPath("TX3Z/ITD0L" & SSN)
        If SP.Q.Check4Text(1, 72, "TDX0M") Then
            DateDelinquencyOccurred = SP.Q.GetText(14, 35, 8)
            NumDaysDelinquent = SP.Q.GetText(9, 76, 4)
            AmountPastDue = CDbl(SP.Q.GetText(15, 35, 8))
            TotalAmountDue = CDbl(SP.Q.GetText(15, 69, 8))
            TotalLateFeesDue = CDbl(SP.Q.GetText(16, 71, 6))
        End If

    End Sub

    Private Function GetStatus(ByVal Stat As String) As String
        GetStatus = ""
        If Stat = "DE" Or Stat = "Verified Death".ToUpper Then Return "Death"
        If Stat = "Alleged Death".ToUpper Then Return "Alleged Death"
        If Stat = "DI" Or Stat = "Verified Disability".ToUpper Then Return "Disability"
        If Stat = "Alleged Disability".ToUpper Then Return "Alleged Disability"
        If Stat = "BC" Or Stat = "BH" Or Stat = "BO" Or Stat = "Verified Bankruptcy".ToUpper Then Return "Bankruptcy"
        If Stat = "Alleged Bankruptcy".ToUpper Then Return "Alleged Bankruptcy"
        If Stat = "CURE" Then Return "CURE"
        If Stat = "Repayment".ToUpper Or Stat = "RP" Then Return "In Repayment"
        If Stat = "IG" Or Stat = "In Grace".ToUpper Then Return "In Grace"
        If Stat = "IA" Or Stat = "In School".ToUpper Then Return "In School"
        If Stat = "ID" Then Return "In School/In Grace"
        If Stat = "DA" Or Stat = "Deferment".ToUpper Then Return "Deferment"
        If Stat = "FB" Or Stat = "Forbearance".ToUpper Then Return "Forbearance"
        If Stat = "CP" And (Stat = "DF" Or Stat = "DB" Or Stat = "DQ" Or Stat = "DU") Then Return "Default"
        If (Stat = "CR" And Stat = "DF") Or Stat = "PRE-CLAIM SUBMITTED" Then Return "Preclaim"
        If Stat = "CP" And Stat = "IN" Then Return "Ineligible"
    End Function

    'updates the throwing up file for MD recovery
    Public Sub SpillGuts()
        FileOpen(1, "T:\MauiDUDELastWords.txt", OpenMode.Output, OpenAccess.Write)
        PrintLine(1, "UserID:", UserId)
        PrintLine(1, "")
        PrintLine(1, " ")
        PrintLine(1, "SSN:", SSN)
        PrintLine(1, " ")
        PrintLine(1, "Name:", UserProvidedDemos.FirstName, UserProvidedDemos.MI, UserProvidedDemos.LastName)
        PrintLine(1, " ")
        PrintLine(1, "Address", "Address Valid: ", UserProvidedDemos.UPAddrVal)
        PrintLine(1, UserProvidedDemos.Addr1 & " ")
        PrintLine(1, UserProvidedDemos.Addr2 & " ")
        PrintLine(1, UserProvidedDemos.City & " " & UserProvidedDemos.State & " " & UserProvidedDemos.Zip)
        PrintLine(1, " ")
        PrintLine(1, "Phone: ", UserProvidedDemos.HomePhoneNum, UserProvidedDemos.HomePhoneExt)
        PrintLine(1, "Phone Valid: ", UserProvidedDemos.UPPhoneVal)
        PrintLine(1, "OtherPhone: ", UserProvidedDemos.OtherPhoneNum, UserProvidedDemos.OtherPhoneExt)
        PrintLine(1, "OtherPhone Valid: ", UserProvidedDemos.UPOtherVal)
        PrintLine(1, "OtherPhone2: ", UserProvidedDemos.OtherPhone2Num, UserProvidedDemos.OtherPhone2Ext)
        PrintLine(1, "OtherPhone2 Valid: ", UserProvidedDemos.UPOther2Val)
        PrintLine(1, "E-mail: ", UserProvidedDemos.Email)
        PrintLine(1, "E-mail Valid: ", UserProvidedDemos.UPEmailVal)
        PrintLine(1, " ")
        PrintLine(1, " ")
        PrintLine(1, Scripts & " ")
        PrintLine(1, " ")
        PrintLine(1, " ")

        FileClose(1)
    End Sub

    Public Sub ReturnToACP()
        'decide whether to return to TC00 directly or through TX6X if a queue was recorded then return through TX6X
        If BorLite.NoACPBSVCall Then Exit Sub
        If BorLite.Queue = "" Then 'if Queue = "" then return directly through TC00
            'return directly to TC00

            SP.Q.FastPath("TX3Z/ATC00" & SSN)
            SP.Q.PutText(19, 38, BorLite.ACPSelection, True)
        Else 'If Queue is not "" then 
            'return to TC00 through TX6X
            SP.Q.FastPath("TX3Z/ITX6X" & BorLite.Queue & ";" & BorLite.SubQueue)
            SP.Q.PutText(21, 18, "1", True) 'select task in working status
            SP.Q.Hit("F4")
        End If
        'enter demographic validation indicators
        'TODO: When promoting be sure in set for live
        'Live
        ''SP.Q.PutText(21, 66, "N")
        ' ''address
        ''If UserProvidedDemos.UPAddrVer Then
        ''    SP.Q.PutText(22, 36, "Y")
        ''Else
        ''    SP.Q.PutText(22, 36, "N")
        ''End If
        ' ''phone
        ''If UserProvidedDemos.UPPhoneNumVer Then
        ''    SP.Q.PutText(22, 52, "Y")
        ''Else
        ''    SP.Q.PutText(22, 52, "N")
        ''End If
        ' ''email
        ''If UserProvidedDemos.UPEmailVer Then
        ''    SP.Q.PutText(22, 68, "Y")
        ''Else
        ''    SP.Q.PutText(22, 68, "N")
        ''End If
        'Test
        SP.Q.PutText(22, 22, "N")
        'address
        If UserProvidedDemos.UPAddrVer Then
            SP.Q.PutText(22, 41, "Y")
        Else
            SP.Q.PutText(22, 41, "N")
        End If
        'phone
        If UserProvidedDemos.UPPhoneNumVer Then
            SP.Q.PutText(22, 60, "Y")
        Else
            SP.Q.PutText(22, 60, "N")
        End If
        'email
        If UserProvidedDemos.UPEmailVer Then
            SP.Q.PutText(22, 79, "Y")
        Else
            SP.Q.PutText(22, 79, "N")
        End If

        SP.Q.Hit("Enter")
        While SP.Q.Check4Text(23, 2, "88009") Or SP.Q.Check4Text(23, 2, "88471") Or SP.Q.Check4Text(23, 2, "88005")
            'TODO: When promoting be sure in set for live
            'Live
            ''If SP.Q.Check4Text(23, 2, "88009") Then 'if phone indicator doesn't work then change
            ''    SP.Q.PutText(22, 52, "N", True)
            ''ElseIf SP.Q.Check4Text(23, 2, "88471") Then 'if email indicator doesn't work then change
            ''    SP.Q.PutText(22, 68, "N", True)
            ''ElseIf SP.Q.Check4Text(23, 2, "88005") Then 'if address indicator doesn't work then change
            ''    SP.Q.PutText(22, 36, "N", True)
            ''End If
            'Test
            If SP.Q.Check4Text(23, 2, "88009") Then 'if phone indicator doesn't work then change
                SP.Q.PutText(22, 60, "N", True)
            ElseIf SP.Q.Check4Text(23, 2, "88471") Then 'if email indicator doesn't work then change
                SP.Q.PutText(22, 79, "N", True)
            ElseIf SP.Q.Check4Text(23, 2, "88005") Then 'if address indicator doesn't work then change
                SP.Q.PutText(22, 41, "N", True)
            End If
        End While

    End Sub

    'called from demographics
    Sub GetSpecialComment()
        Dim DA As SqlClient.SqlDataAdapter
        Dim DS As New DataSet
        Try
            SP.UsrInf.Conn.Open()
            DA = New SqlClient.SqlDataAdapter("select ARC, Days from SpecialCommentsTB", SP.UsrInf.Conn)
            DA.Fill(DS)
            SP.UsrInf.Conn.Close()
            'specialcommentarr
            Dim r As DataRow
            For Each r In DS.Tables(0).Rows
                SP.Q.FastPath("TX3Z/ITD2A" & SSN)
                SP.Q.PutText(11, 65, r("ARC"))
                SP.Q.PutText(21, 16, Format(Today.AddDays(CInt(r("Days")) * (-1)), "MMddyy")) 'todays date - Days
                SP.Q.PutText(21, 30, Format(Today, "MMddyy"))
                SP.Q.Hit("ENTER")
                If SP.Q.Check4Text(1, 72, "TDX2C") Then
                    SP.Q.PutText(5, 14, "X", True)
                    Do While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        ReDim Preserve SpecialCommentArr(2, UBound(SpecialCommentArr, 2) + 1)
                        SpecialCommentArr(0, UBound(SpecialCommentArr, 2)) = SP.Q.GetText(13, 2, 5) '0 = ARC
                        SpecialCommentArr(1, UBound(SpecialCommentArr, 2)) = SP.Q.GetText(13, 31, 8) '1 = Date
                        SpecialCommentArr(2, UBound(SpecialCommentArr, 2)) = SP.Q.GetText(17, 2, 78) & SP.Q.GetText(18, 2, 78) & SP.Q.GetText(19, 2, 78) & SP.Q.GetText(20, 2, 78) & SP.Q.GetText(21, 2, 78) & SP.Q.GetText(22, 2, 78)  '2 = Text
                        SP.Q.Hit("F8")
                    Loop
                ElseIf SP.Q.Check4Text(1, 72, "TDX2D") Then
                    ReDim Preserve SpecialCommentArr(2, UBound(SpecialCommentArr, 2) + 1)
                    SpecialCommentArr(0, UBound(SpecialCommentArr, 2)) = SP.Q.GetText(13, 2, 5) '0 = ARC
                    SpecialCommentArr(1, UBound(SpecialCommentArr, 2)) = SP.Q.GetText(13, 31, 8) '1 = Date
                    SpecialCommentArr(2, UBound(SpecialCommentArr, 2)) = SP.Q.GetText(17, 2, 78) & SP.Q.GetText(18, 2, 78) & SP.Q.GetText(19, 2, 78) & SP.Q.GetText(20, 2, 78) & SP.Q.GetText(21, 2, 78) & SP.Q.GetText(22, 2, 78)  '2 = Text
                End If
            Next
            RunSpecialComments = True
        Catch ex As Exception
            SP.frmWipeOut.WipeOut("This is like night surfin man!  The database is like totaly gone, but we can still surf if you want dude.", "Database not found", True)
            RunSpecialComments = False
        End Try
    End Sub

    'called from Demographics
    Public Sub ShowSpecialComments()
        Dim x As Integer
        If UBound(SpecialCommentArr, 2) > 0 Then
            For x = 1 To UBound(SpecialCommentArr, 2)
                Dim frm As New frmSpecialComments
                frm.Show(SpecialCommentArr(0, x), SpecialCommentArr(2, x), SpecialCommentArr(1, x))
            Next
        End If
    End Sub

    'this function is used to to commit the note dude text to the system
    Public Sub EnterCommentsIntoSystem(ByVal FromBSHomePage As Boolean)
        If FromBSHomePage Then
            'switch keys until 
            If SP.Q.Check4Text(1, 74, "TCX0D") Then
                While SP.Q.Check4Text(24, 21, "CMTS") = False
                    SP.Q.Hit("F2")
                End While
            ElseIf SP.Q.Check4Text(1, 72, "TCX0I") Or SP.Q.Check4Text(1, 72, "TCX06") Then
                While SP.Q.Check4Text(24, 29, "CMTS") = False
                    SP.Q.Hit("F2")
                End While
            End If
            If Not Check4Text(1, 74, "TCX13") Then
                SP.Q.Hit("F4")
            End If


            If Notes.Length > 354 Then
                SP.Q.PutText(12, 10, Mid(Notes, 1, 353))
                SP.Q.PutText(22, 69, "N", True)
                'find comment on td2a and enter the remaining comments
                SP.Q.FastPath("TX3ZITD2A" & SSN)
                SP.Q.PutText(8, 31, "X")
                SP.Q.PutText(7, 60, "X")
                SP.Q.PutText(21, 56, Format(Today, "MMddyy"))
                SP.Q.PutText(21, 70, Format(Today, "MMddyy"), True)
                'if selection screen is displayed then select first option
                If SP.Q.Check4Text(1, 72, "TDX2C") Then
                    SP.Q.PutText(7, 2, "X", True)
                End If
                SP.Q.Hit("F4")
                SP.Q.PutText(8, 5, Mid(Notes, 354, 1065), True)
            Else
                SP.Q.PutText(12, 10, Notes)
                If Check4Text(22, 69, "_") Then
                    SP.Q.PutText(22, 69, "N", True)
                Else
                    Hit("Enter")
                End If
            End If
        Else
            'LM HomePage
            ActivityCmts.AddCommentsToLP50(Notes, "", ContactCode, ActivityCode)
        End If
    End Sub

End Class




