Public Class OneLINKDemographics
    Inherits Demographics

    Public Sub New(ByVal SSN As String)
        MyBase.New(SSN)
        TheSystem = WhatSystem.Onelink 'ONLY FOR BACKWARD COMPATIBILITY
    End Sub

    Public Overrides Sub PopulateObjectFromSystem()
        'determine what was passed to the object a account number (length = 10) or SSN 
        '****SP.Bor.BorrowerIsLCOOnly = False
        'Public Function GetOneLINKInfo(ByRef Bor As Borrower) As Boolean
        If SSN.Length = 10 Then
            SP.Q.FastPath("LP22I;;;;;;" & SSN)
            '****Delete line above and if then statement around
        Else
            SP.Q.FastPath("LP22I" & SSN)
        End If
        'check if the user is logged into OneLINK and COMPASS
        If SP.Q.Check4Text(1, 2, "INVALID COMMAND SYNTAX") Or SP.Q.Check4Text(3, 11, "CICS0001: OPERATOR IS NOT LOGGED ON.") Then
            'GetExistingSession(SSN)
            If SP.Q.RIBM Is Nothing Then
                SP.frmWipeOut.WipeOut("That totally didn't work out.  How about you log in to OneLINK and COMPASS.", "Log In Problem", True)
                FoundOnSystem = False
            End If
        End If
        'is the borrower located on OneLINK
        If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
            FoundOnSystem = False
            Exit Sub
        End If
        'gather the Demographic information
        SSN = SP.Q.GetText(3, 23, 9) 'SSN
        CLAccNum = SP.Q.GetText(3, 60, 12).Replace(" ", "") 'account number
        Name = SP.Q.GetText(4, 44, 12) & " " & SP.Q.GetText(4, 60, 1) & " " & SP.Q.GetText(4, 5, 35) 'name
        FirstName = SP.Q.GetText(4, 44, 12)
        MI = SP.Q.GetText(4, 60, 1)
        LastName = SP.Q.GetText(4, 5, 35)
        DOB = SP.Q.GetText(4, 72, 8) 'Date of Birth
        DOB = DOB.Insert(4, "/")
        DOB = DOB.Insert(2, "/")
        'sp.Bor.DOB = sp.Bor.DOB
        Addr1 = SP.Q.GetText(10, 9, 35) 'address 1
        Addr2 = SP.Q.GetText(11, 9, 35) 'address 2
        SPAddrInd = SP.Q.GetText(10, 57, 1)
        SPAddrVerDt = SP.Q.GetText(10, 72, 8) 'tp
        SPAddrVerDt = SPAddrVerDt.Insert(4, "/")
        SPAddrVerDt = SPAddrVerDt.Insert(2, "/")
        City = SP.Q.GetText(12, 9, 30)
        State = SP.Q.GetText(12, 52, 2)
        Zip = SP.Q.GetText(12, 60, 9)
        HomePhoneNum = SP.Q.GetText(13, 12, 10)
        HomePhoneExt = SP.Q.GetText(13, 27, 4)
        HomePhoneValidityIndicator = SP.Q.GetText(13, 38, 1)
        HomePhoneVerificationDate = SP.Q.GetText(13, 44, 8)
        HomePhoneVerificationDate = HomePhoneVerificationDate.Insert(4, "/")
        HomePhoneVerificationDate = HomePhoneVerificationDate.Insert(2, "/")
        HomePhoneMBL = GetText(13, 58, 1)
        HomePhoneConsent = GetText(13, 68, 1)
        OtherPhoneNum = SP.Q.GetText(14, 12, 10)
        OtherPhoneExt = SP.Q.GetText(14, 27, 4)
        OtherPhoneVerificationDate = GetText(14, 44, 8)
        OtherPhoneVerificationDate = OtherPhoneVerificationDate.Insert(4, "/")
        OtherPhoneVerificationDate = OtherPhoneVerificationDate.Insert(2, "/")
        OtherPhoneValidityIndicator = SP.Q.GetText(14, 38, 1)
        OtherPhoneMBL = GetText(14, 58, 1)
        OtherPhoneConsent = GetText(14, 68, 1)
        Email = SP.Q.GetText(19, 9, 56)
        SPEmailInd = SP.Q.GetText(18, 56, 1)
        SPEmailVerDt = SP.Q.GetText(18, 71, 8)
        SPEmailVerDt = SPEmailVerDt.Insert(4, "/")
        SPEmailVerDt = SPEmailVerDt.Insert(2, "/")
        'IF the foreign address if populate the change the state to FC so dude will think its a foreign address
        If SP.Q.GetText(16, 16, 10) <> "" Then State = "FC"
        FoundOnSystem = True
    End Sub

End Class
