Public Class LCODemographics
    Inherits Demographics

    Public Sub New(ByVal SSN As String)
        MyBase.New(SSN)
        TheSystem = WhatSystem.LCO 'ONLY FOR BACKWARD COMPATIBILITY
    End Sub

    Public Overrides Sub PopulateObjectFromSystem()

        SP.Processing.Visible = True
        SP.LoginToLCO()
        SP.Q.FastPath("TPDD" & SSN)
        If SP.Q.Check4Text(1, 19, "LCO PERSONAL INFORMATION DISPLAY") = True Then
            'Gather LCO demographic info
            FirstName = Replace(SP.Q.GetText(4, 19, 12), "_", " ")
            LastName = Replace(SP.Q.GetText(4, 35, 22), "_", " ")
            Name = FirstName & " " & LastName
            Addr1 = Replace(SP.Q.GetText(6, 19, 29), "_", " ")
            Addr2 = Replace(SP.Q.GetText(7, 19, 29), "_", " ")
            City = Replace(SP.Q.GetText(9, 19, 19), "_", " ")
            State = Replace(SP.Q.GetText(9, 50, 2), "_", " ")
            Zip = Replace(SP.Q.GetText(9, 66, 5), "_", " ")
            HomePhoneNum = Replace(SP.Q.GetText(13, 13, 12), "_", " ")
            OtherPhoneNum = Replace(SP.Q.GetText(14, 13, 12), "_", " ")
            Email = Replace(SP.Q.GetText(19, 19, 55), "_", " ")
            DOB = Replace(SP.Q.GetText(5, 61, 10), " ", "/")
            CLAccNum = GetText(3, 62, 12).Replace("-", "")
            'remove extra spaces
            Addr1 = Trim(Addr1)
            Addr2 = Trim(Addr2)
            City = Trim(City)
            State = Trim(State)
            Zip = Trim(Zip)
            HomePhoneNum = Trim(HomePhoneNum)
            OtherPhoneNum = Trim(OtherPhoneNum)
            Email = Trim(Email)
            DOB = Trim(DOB)

            SP.LoginToCompass()
            SP.Processing.Visible = False
            FoundOnSystem = True
        Else
            SP.LoginToCompass()
            SP.Processing.Visible = False
            SP.frmWhoaDUDE.WhoaDUDE("That was totally knarly, but DUDE needs a valid SSN or account number from OneLINK.", "Knarly DUDE", True)
            FoundOnSystem = False
        End If

    End Sub

End Class
