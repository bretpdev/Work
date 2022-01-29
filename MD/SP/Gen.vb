Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Public Module Gen
    Public AutoDialerSSN As String
    Public Processing As New frmProcessing
    Public WipeOut As New frmWipeOut
    Public WhoaDude As New frmWhoaDUDE
    Public HawaiianDict As New frmHawaiianDictionary
    Public AboutDUDE As New frmAboutDUDE
    Public BorrInfo As New frm411
    Public AskDUDE As New MDAskDUDE.frmAskDUDE
    Public KnarlyDUDE As New frmKnarlyDUDE
    Public NoteDude As frmNoteDUDE
    Public GeneralForeColor As Color
    Public GeneralBackColor As Color
    Public GeneralTransparency As Double
    Public Bor As Borrower
    Public EmailCom As New frmEmailComments

    Public Function GetACH()
        Dim row As Integer
        Dim LnRow As Integer
        'ReDim ACHData(2)
        SP.Bor.ACHData = New SP.ClassACHInfo 'reset variable
        ReDim SP.Bor.ACHData.LnLvlInfo(3, 0)
        SP.Bor.ACHData.HasACH = "No" 'init as a no
        SP.Bor.ACHData.ACHDataFound = False 'init to false
        'access TS7O
        SP.Q.FastPath("TX3Z/ITS7O" & SP.Bor.SSN)

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
                        SP.Bor.ACHData.ACHDataFound = True 'record with data found
                        'get ACH level information
                        SP.Bor.ACHData.HasACH = "No"
                        SP.Bor.ACHData.StatusDt = SP.Q.GetText(11, 18, 10)
                        SP.Bor.ACHData.DenialReason = SP.Q.GetText(10, 57, 1)
                        SP.Bor.ACHData.NSFCounter = Replace(SP.Q.GetText(13, 18, 1), "_", "") 'blank if underscore
                        Exit Function
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
                SP.Bor.ACHData.ACHDataFound = True 'record with data found
                'get ACH level information
                If SP.Q.Check4Text(17, 13, " ") Then
                    SP.Bor.ACHData.HasACH = "No"
                Else
                    SP.Bor.ACHData.HasACH = "Yes"
                End If
                SP.Bor.ACHData.StatusDt = SP.Q.GetText(11, 18, 10)
                SP.Bor.ACHData.NSFCounter = SP.Q.GetText(13, 18, 1).Replace("_", "") 'blank if underscore
                SP.Bor.ACHData.AdditionalWithdrawAmt = SP.Q.GetText(11, 57, 10).Replace("_", "")
                SP.Bor.ACHData.RoutingNumber = SP.Q.GetText(6, 18, 14)
                SP.Bor.ACHData.AccountNumber = SP.Q.GetText(6, 52, 17)
                'get loan level information
                LnRow = 17
                While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    SP.Bor.ACHData.LnLvlInfo(0, SP.Bor.ACHData.LnLvlInfo.GetUpperBound(1)) = SP.Q.GetText(LnRow, 11, 4) 'seq num
                    SP.Bor.ACHData.LnLvlInfo(1, SP.Bor.ACHData.LnLvlInfo.GetUpperBound(1)) = SP.Q.GetText(LnRow, 23, 10) 'frst disb dt
                    SP.Bor.ACHData.LnLvlInfo(2, SP.Bor.ACHData.LnLvlInfo.GetUpperBound(1)) = SP.Q.GetText(LnRow, 41, 7)  'loan program
                    SP.Bor.ACHData.LnLvlInfo(3, SP.Bor.ACHData.LnLvlInfo.GetUpperBound(1)) = SP.Q.GetText(LnRow, 58, 3)  'ACH Sq Num
                    ReDim Preserve SP.Bor.ACHData.LnLvlInfo(3, SP.Bor.ACHData.LnLvlInfo.GetUpperBound(1) + 1) 'add another to array
                    LnRow = LnRow + 1
                    If IsNumeric(SP.Q.GetText(LnRow, 11, 4)) = False Then
                        LnRow = 17
                        SP.Hit("F8")
                    End If
                End While
            End If
        End If

    End Function

    Function CheckPOBox() As Boolean
        Dim DsbDt As String
        SP.Q.FastPath("LG0HI;" & SP.Bor.SSN)
        If SP.Q.Check4Text(22, 3, "47004") Then '47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA
            Return False
        End If
        If SP.Q.Check4Text(1, 53, "DISBURSEMENT ACTIVITY SELECT") Then
            SP.Q.PutText(21, 11, "01", True)
        End If
        If SP.Q.Check4Text(1, 52, "DISBURSEMENT ACTIVITY DISPLAY") Then
            Do While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                DsbDt = SP.Q.GetText(12, 20, 2) & "\" & SP.Q.GetText(12, 22, 2) & "\" & SP.Q.GetText(12, 24, 4)
                If IsDate(DsbDt) Then
                    If CDate(DsbDt) <= Today Then
                        'Allowed
                    Else
                        Return False
                    End If
                Else
                    If DsbDt = "MM\DD\CCYY" Then
                        'If the ACT DSB DT is ‘MMDDCCYY’ and the Cancel Amt (13,18,10) is equal to the Disb Amt (11,18,10) Do not consider this loan for determining PO Box Allowance
                        If SP.Q.GetText(13, 18, 10) <> SP.Q.GetText(11, 18, 10) Then
                            Return False
                        End If
                    End If
                End If

                SP.Hit("F8")
            Loop
        End If
        Return True
    End Function

    Public Function GetLG10()
        Dim i As Integer
        Dim i2 As Integer
        Dim row As Integer
        'This is realy LG10 no longer LG02

        '1 = loan type
        '2 = servicer code
        '3 = date processed
        '4 = guarantee amount
        '5 = loan status
        '6 = loan status reason code

        'access LG10
        SP.Q.FastPath("LG10I" & SP.Bor.SSN)
        'get data from selection screen for each loan
        If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") = False Then
            If SP.Q.Check4Text(1, 75, "SELECT") Then
                ReDim SP.Bor.LG10Data(6, 0)
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
                            ReDim Preserve SP.Bor.LG10Data(6, UBound(SP.Bor.LG10Data, 2) + 1)
                            SP.Bor.LG10Data(1, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(4, 13, 2)
                            SP.Bor.LG10Data(2, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(5, 27, 6)
                            SP.Bor.LG10Data(3, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 26, 8)
                            SP.Bor.LG10Data(4, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 37, 9)
                            SP.Bor.LG10Data(5, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 59, 2)
                            SP.Bor.LG10Data(6, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 64, 2)
                            SP.Bor.LG10Data(0, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 48, 8)
                            If SP.Q.GetText(row + 1, 4, 5) = "" Then
                                SP.Q.Hit("F8")
                                row = 10
                            End If
                        Loop
                        SP.Q.Hit("F12")
                    End If
                Loop
            Else
                ReDim SP.Bor.LG10Data(6, 0)
                If SP.Q.Check4Text(1, 52, "LOAN BWR STATUS RECAP DISPLAY") Then
                    row = 10
                    Do While Not SP.Q.Check4Text(22, 3, "46004")
                        row += 1
                        ReDim Preserve SP.Bor.LG10Data(6, UBound(SP.Bor.LG10Data, 2) + 1)
                        SP.Bor.LG10Data(1, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(4, 13, 2)
                        SP.Bor.LG10Data(2, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(5, 27, 6)
                        SP.Bor.LG10Data(3, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 26, 8)
                        SP.Bor.LG10Data(4, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 37, 9)
                        SP.Bor.LG10Data(5, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 59, 2)
                        SP.Bor.LG10Data(6, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 64, 2)
                        SP.Bor.LG10Data(0, UBound(SP.Bor.LG10Data, 2)) = SP.Q.GetText(row, 48, 8)
                        If SP.Q.GetText(row + 1, 4, 5) = "" Then
                            SP.Q.Hit("F8")
                            row = 10
                        End If
                    Loop
                End If
            End If
        End If

        SP.Q.FastPath("TX3Z/ITD0L" & SP.Bor.SSN)
        If SP.Q.Check4Text(1, 72, "TDX0M") Then
            SP.Bor.DateDelinquencyOccurred = SP.Q.GetText(14, 35, 8)
            SP.Bor.NumDaysDelinquent = SP.Q.GetText(9, 76, 4)
            SP.Bor.AmountPastDue = CDbl(SP.Q.GetText(15, 35, 8))
            SP.Bor.TotalAmountDue = CDbl(SP.Q.GetText(15, 69, 8))
            SP.Bor.TotalLateFeesDue = CDbl(SP.Q.GetText(16, 71, 6))
        End If

    End Function

    Public Sub GetColors(ByVal BackC As Color, ByVal ForeC As Color, ByVal OpacityT As Double)
        Dim Opacity As Double
        Dim BColor As Integer
        Dim FColor As Integer
        If Dir("C:\Program Files\MauiDUDE\MauiDUDEPref.dat") <> "" Then
            'pull info out of pref file
            FileOpen(1, "C:\Program Files\MauiDUDE\MauiDUDEPref.dat", OpenMode.Input)
            Input(1, Opacity)
            Input(1, BColor)
            Input(1, FColor)
            FileClose(1)
            'apply users pref
            BackGroundColorChange(Color.FromArgb(BColor))
            ForeGroundColorChange(Color.FromArgb(FColor))
            TransparencyChange(Opacity)

        Else
            Opacity = OpacityT
            BColor = BackC.ToArgb
            FColor = ForeC.ToArgb
            'write updated info out to the pref file
            FileOpen(1, "C:\Program Files\MauiDUDE\MauiDUDEPref.dat", OpenMode.Output)
            Write(1, Opacity, BColor, FColor)
            FileClose(1)
        End If
    End Sub

    Public Function BackGroundColorChange(ByVal BC As Color)
        Processing.BackColor = BC
        WipeOut.BackColor = BC
        WhoaDude.BackColor = BC
        'HawaiianDict.BackColor = BC
        'AboutDUDE.BackColor = BC
        BorrInfo.BackColor = BC
        GeneralBackColor = BC
        KnarlyDUDE.BackColor = BC
    End Function

    'This function updates all the misc form foreground colors
    Public Function ForeGroundColorChange(ByVal FC As Color)
        Processing.ForeColor = FC
        WipeOut.ForeColor = FC
        WhoaDude.ForeColor = FC
        'HawaiianDict.ForeColor = FC
        'AboutDUDE.ForeColor = FC
        BorrInfo.ForeColor = FC
        GeneralForeColor = FC
        KnarlyDUDE.ForeColor = FC
    End Function

    Public Function TransparencyChange(ByVal TR As Double)
        GeneralTransparency = TR
        'HawaiianDict.Opacity = GeneralTransparency
        BorrInfo.Opacity = GeneralTransparency
        'AboutDUDE.Opacity = GeneralTransparency
    End Function
    'Checks for specific text at a certain location on the screen

    'ensures the correct set of hotkeys is displayed
    Public Function CheckSet(ByVal sSet)
        If GetText(23, 23, sSet) Then Hit("F2")
    End Function

    'translates ACS keyline encrypted SSN into SSN 
    Public Function ACSTranslation(ByRef KeyLn As String) As String
        Dim i As Integer
        For i = 1 To 9
            KeyLn = UCase(KeyLn) 'capitalize everything
            Select Case Mid(KeyLn, i, 1)
                Case "M"
                    ACSTranslation = ACSTranslation & "0"
                Case "Y"
                    ACSTranslation = ACSTranslation & "9"
                Case "L"
                    ACSTranslation = ACSTranslation & "8"
                Case "A"
                    ACSTranslation = ACSTranslation & "7"
                Case "U"
                    ACSTranslation = ACSTranslation & "6"
                Case "G"
                    ACSTranslation = ACSTranslation & "5"
                Case "H"
                    ACSTranslation = ACSTranslation & "4"
                Case "T"
                    ACSTranslation = ACSTranslation & "3"
                Case "E"
                    ACSTranslation = ACSTranslation & "2"
                Case "R"
                    ACSTranslation = ACSTranslation & "1"
                Case Else
                    ACSTranslation = ""
                    Exit Function
            End Select
        Next
    End Function

    Public Sub EmailScreenError()

        Dim Subject As String
        RIBM.PrintFileName = "C:\Windows\Temp\MD Reflection Error.txt"
        RIBM.PrintFileExistsAction = rcOverwrite
        RIBM.PrintToFile = True
        RIBM.PrintScreen(rcPrintScreen, 1) 'create print screen in file
        RIBM.PrintToFile = False

        Subject = "Automated Error Email"
        If TestMode() Then
            Subject = "TEST EMAIL PLEASE IGNORE -- " & Subject
        End If
        'GWMsg.Send()
        SendMail(Environment.UserName & "@utahsbr.edu", "mauidude@utahsbr.edu", Subject, , , , "C:\Windows\Temp\MD Reflection Error.txt")
    End Sub

    Public Sub ResetBorrower()
        Bor = New Borrower
    End Sub

    'send an e-mail message using SMTP
    Public Function SendMail(ByVal mFrom As String, ByVal mTo As String, Optional ByVal mSubject As String = "", Optional ByVal mBody As String = "", Optional ByVal mCC As String = "", Optional ByVal mBCC As String = "", Optional ByVal mAttach As String = "") As Boolean
        Dim aAttach() As String
        Dim i As Integer
        Dim eMail As OSSMTP.SMTPSession
        eMail = New OSSMTP.SMTPSession

        'set server
        eMail.Server = "mail.utahsbr.edu"

        'create message
        eMail.MailFrom = mFrom
        eMail.SendTo = mTo
        eMail.CC = mCC
        eMail.BCC = mBCC
        eMail.MessageSubject = mSubject
        eMail.MessageText = mBody

        'add attachments if there are any
        If Len(mAttach) > 0 Then
            'split file names from string
            aAttach = Split(mAttach, ",")

            'add attachments
            For i = 0 To UBound(aAttach)
                eMail.Attachments.Add(aAttach(i))
            Next i
        End If

        'send message
        eMail.SendEmail()

        'verify the message was sent
        If eMail.Status = "SMTP connection closed" Then
            SendMail = True
        Else
            SendMail = False
        End If
    End Function
End Module
