Public Class COMPASSDemographics
    Inherits Demographics

    Public Sub New(ByVal SSN As String)
        MyBase.New(SSN)
        TheSystem = WhatSystem.Compass 'ONLY FOR BACKWARD COMPATIBILITY
    End Sub

    Public Overrides Sub PopulateObjectFromSystem()
        'is the borrower located on COMPASS
        SP.Q.FastPath("TX3Z/ITX1J;" & SSN)
        If SP.Q.Check4Text(1, 71, "TXX1R") Then
            'check for address line 1 info and if it doesn't exist then act like COMPASS demo rec doesn't exist
            If Replace(SP.Q.GetText(11, 10, 30), "_", "") = "" Then
                FoundOnSystem = False 'address info does exist on COMPASS (sometimes the record exists but is not fully populated because the application hasn't been garunteed)
                Exit Sub
            End If
            Name = GetText(4, 34, 14) + " " + GetText(4, 6, 25)
            CLAccNum = GetText(3, 34, 12).Replace(" ", "")
            DOB = SP.GetText(20, 6, 10).Replace(" ", "/")
            Addr1 = Replace(SP.Q.GetText(11, 10, 30), "_", "")
            Addr2 = Replace(SP.Q.GetText(12, 10, 30), "_", "")
            SPAddrVerDt = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(10, 32, 8), " ", "/")), "Short Date")
            SPAddrInd = SP.Q.GetText(11, 55, 1)
            City = Replace(SP.Q.GetText(14, 8, 20), "_", "")
            State = Replace(SP.Q.GetText(14, 32, 2), "_", "")
            State = Replace(State, "_", "")
            Zip = Replace(SP.Q.GetText(14, 40, 9), "_", "")
            HomePhoneNum = Replace(SP.Q.GetText(17, 14, 3) & SP.Q.GetText(17, 23, 3) & SP.Q.GetText(17, 31, 4), "_", "")
            HomePhoneExt = SP.GetText(17, 40, 5).Replace("_", "")
            HomePhoneMBL = GetText(16, 20, 1)
            HomePhoneConsent = GetText(16, 30, 1)

            If SP.Q.GetText(16, 45, 2) <> "__" Then
                HomePhoneVerificationDate = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(16, 45, 8), " ", "/")), "Short Date")
            End If

            HomePhoneValidityIndicator = Replace(SP.Q.GetText(17, 54, 1), "_", "")
            SP.Q.Hit("F6")
            SP.Q.Hit("F6")
            SP.Q.Hit("F6")

            SP.Q.PutText(16, 14, "A", True)
            OtherPhoneNum = Replace(SP.Q.GetText(17, 14, 3) & SP.Q.GetText(17, 23, 3) & SP.Q.GetText(17, 31, 4), "_", "")
            OtherPhoneExt = SP.GetText(17, 40, 5).Replace("_", "")

            If SP.Q.GetText(16, 45, 2) <> "__" Then
                OtherPhoneVerificationDate = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(16, 45, 8), " ", "/")), "Short Date")
                OtherPhoneMBL = GetText(16, 20, 1)
                OtherPhoneConsent = GetText(16, 30, 1)
            End If

            OtherPhoneValidityIndicator = Replace(SP.Q.GetText(17, 54, 1), "_", "")

            SP.Q.PutText(16, 14, "W", True)
            OtherPhone2Num = Replace(SP.Q.GetText(17, 14, 3) & SP.Q.GetText(17, 23, 3) & SP.Q.GetText(17, 31, 4), "_", "")
            OtherPhone2Ext = SP.GetText(17, 40, 5).Replace("_", "")

            If SP.Q.GetText(16, 45, 2) <> "__" Then
                OtherPhone2VerificationDate = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(16, 45, 8), " ", "/")), "Short Date")
                OtherPhone2MBL = GetText(16, 20, 1)
                OtherPhone2Consent = GetText(16, 30, 1)
            End If

            OtherPhone2ValidityIndicator = Replace(SP.Q.GetText(17, 54, 1), "_", "")

            If SP.Q.Check4Text(24, 55, "F10=EML") = False Then SP.Q.Hit("F2")
            SP.Q.Hit("F10")
            'TODO: When promoting be sure in set for live
            'Live
            ''Email = SP.Q.GetText(15, 10, 60).Trim("_")
            ''If SP.Q.GetText(12, 17, 2) <> "__" Then SPEmailVerDt = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(12, 17, 8), " ", "/")), "Short Date")
            ''SPEmailInd = Replace(SP.Q.GetText(13, 14, 1), "_", "")
            'Test
            Email = SP.Q.GetText(14, 10, 60).Trim("_")
            If SP.Q.GetText(11, 17, 2) <> "__" Then SPEmailVerDt = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(11, 17, 8), " ", "/")), "Short Date")
            SPEmailInd = Replace(SP.Q.GetText(12, 14, 1), "_", "")

            FoundOnSystem = True
        Else
            FoundOnSystem = False
        End If
    End Sub
End Class
