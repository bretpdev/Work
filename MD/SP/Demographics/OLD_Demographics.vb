Imports System.IO
Imports System.Drawing
Imports Q

Public Class OLD_Demographics
    '' '' ''Inherits MDBorrowerDemographics

    '' '' ''Public Sub New(ByVal tTheSystem As WhatSystem, ByVal tSSN As String)
    '' '' ''    SSN = tSSN
    '' '' ''    TheSystem = tTheSystem
    '' '' ''    'decide how to collect the data for the object
    '' '' ''    If TheSystem = WhatSystem.Compass Then
    '' '' ''        FoundOnSystem = GetCOMPASSInfo()
    '' '' ''    ElseIf TheSystem = WhatSystem.Onelink Then
    '' '' ''        FoundOnSystem = GetOneLINKInfo()
    '' '' ''    ElseIf TheSystem = WhatSystem.LCO Then
    '' '' ''        FoundOnSystem = GetLCODemographics()
    '' '' ''    ElseIf TheSystem = WhatSystem.UserProvided Then
    '' '' ''        AddressSaved = True
    '' '' ''        PhoneSaved = True
    '' '' ''        OtherSaved = True
    '' '' ''        Other2Saved = True
    '' '' ''        EmailSaved = True
    '' '' ''    End If
    '' '' ''End Sub

    '' '' ''Public Overrides Function GetCOMPASSInfo() As Boolean
    '' '' ''    'is the borrower located on COMPASS
    '' '' ''    SP.Q.FastPath("TX3Z/ITX1J;" & SSN)
    '' '' ''    If SP.Q.Check4Text(1, 71, "TXX1R") Then
    '' '' ''        'check for address line 1 info and if it doesn't exist then act like COMPASS demo rec doesn't exist
    '' '' ''        If Replace(SP.Q.GetText(11, 10, 30), "_", "") = "" Then
    '' '' ''            Return False 'address info does exist on COMPASS (sometimes the record exists but is not fully populated because the application hasn't been garunteed)
    '' '' ''        End If
    '' '' ''        Name = GetText(4, 34, 14) + " " + GetText(4, 6, 25)
    '' '' ''        CLAccNum = GetText(3, 34, 12).Replace(" ", "")
    '' '' ''        DOB = SP.GetText(20, 6, 10).Replace(" ", "/")
    '' '' ''        Addr1 = Replace(SP.Q.GetText(11, 10, 30), "_", "")
    '' '' ''        Addr2 = Replace(SP.Q.GetText(12, 10, 30), "_", "")
    '' '' ''        SPAddrVerDt = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(10, 32, 8), " ", "/")), "Short Date")
    '' '' ''        SPAddrInd = SP.Q.GetText(11, 55, 1)
    '' '' ''        City = Replace(SP.Q.GetText(14, 8, 20), "_", "")
    '' '' ''        State = Replace(SP.Q.GetText(14, 32, 2), "_", "")
    '' '' ''        State = Replace(State, "_", "")
    '' '' ''        Zip = Replace(SP.Q.GetText(14, 40, 9), "_", "")

    '' '' ''        PhoneNum = Replace(SP.Q.GetText(17, 14, 3) & SP.Q.GetText(17, 23, 3) & SP.Q.GetText(17, 31, 4), "_", "")
    '' '' ''        PhoneExt = SP.GetText(17, 40, 5).Replace("_", "")
    '' '' ''        If SP.Q.GetText(16, 32, 2) <> "__" Then SPPhnVerDt = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(16, 32, 8), " ", "/")), "Short Date")
    '' '' ''        SPPhnInd = Replace(SP.Q.GetText(17, 54, 1), "_", "")
    '' '' ''        SP.Q.Hit("F6")
    '' '' ''        SP.Q.Hit("F6")
    '' '' ''        SP.Q.Hit("F6")

    '' '' ''        SP.Q.PutText(16, 14, "A", True)
    '' '' ''        OtherPhone = Replace(SP.Q.GetText(17, 14, 3) & SP.Q.GetText(17, 23, 3) & SP.Q.GetText(17, 31, 4), "_", "")
    '' '' ''        OtherExt = SP.GetText(17, 40, 5).Replace("_", "")
    '' '' ''        If SP.Q.GetText(16, 32, 2) <> "__" Then SPOtPhnVerDt = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(16, 32, 8), " ", "/")), "Short Date")
    '' '' ''        SPOtPhnInd = Replace(SP.Q.GetText(17, 54, 1), "_", "")

    '' '' ''        SP.Q.PutText(16, 14, "W", True)
    '' '' ''        OtherPhone2 = Replace(SP.Q.GetText(17, 14, 3) & SP.Q.GetText(17, 23, 3) & SP.Q.GetText(17, 31, 4), "_", "")
    '' '' ''        Other2Ext = SP.GetText(17, 40, 5).Replace("_", "")
    '' '' ''        If SP.Q.GetText(16, 32, 2) <> "__" Then SPOt2PhnVerDt = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(16, 32, 8), " ", "/")), "Short Date")
    '' '' ''        SPOt2PhnInd = Replace(SP.Q.GetText(17, 54, 1), "_", "")

    '' '' ''        If SP.Q.Check4Text(24, 55, "F10=EML") = False Then SP.Q.Hit("F2")
    '' '' ''        SP.Q.Hit("F10")
    '' '' ''        Email = Replace(SP.Q.GetText(15, 10, 60), "_", "").Trim
    '' '' ''        If SP.Q.GetText(12, 17, 2) <> "__" Then SPEmailVerDt = Microsoft.VisualBasic.Format(CDate(Replace(SP.Q.GetText(12, 17, 8), " ", "/")), "Short Date")
    '' '' ''        SPEmailInd = Replace(SP.Q.GetText(13, 14, 1), "_", "")
    '' '' ''        Return True
    '' '' ''    Else
    '' '' ''        Return False
    '' '' ''    End If
    '' '' ''End Function

    '' '' ''Public Overrides Sub SetCOMPASSInfo(ByVal Source As String, ByVal AddressNeedsUpdate As Boolean, ByVal PhoneNeedsUpdate As Boolean, ByVal AltPhoneNeedsUpdate As Boolean, ByVal WorkPhoneNeedsUpdate As Boolean, ByVal EmailNeedsUpdate As Boolean, ByVal AddressValidityNeedsUpdate As Boolean, ByVal PhoneValidityNeedsUpdate As Boolean, ByVal AltPhoneValidityNeedsUpdate As Boolean, ByVal WorkPhoneValidityNeedsUpdate As Boolean, ByVal EmailValidityNeedsUpdate As Boolean, ByVal AddressNotValid_Checked As Boolean, ByVal PhoneNotValid_Checked As Boolean, ByVal AltPhoneNotValid_Checked As Boolean, ByVal WorkPhoneNotValid_Checked As Boolean, ByVal EmailNotValid_Checked As Boolean, ByVal IsSchool As Boolean)
    '' '' ''    Dim PSource As String
    '' '' ''    PSource = Source
    '' '' ''    'AddressSaved = True
    '' '' ''    'PhoneSaved = True
    '' '' ''    'OtherSaved = True
    '' '' ''    'Other2Saved = True
    '' '' ''    'EmailSaved = True
    '' '' ''    If PSource = "25" Then PSource = "31"

    '' '' ''    SP.Q.FastPath("TX3Z/CTX1J;" & SSN)
    '' '' ''    'don't update COMPASS if error 01019 is returned
    '' '' ''    If SP.Q.Check4Text(23, 2, "01019") = True Then Exit Sub
    '' '' ''    'switch to address info
    '' '' ''    SP.Q.Hit("F6")
    '' '' ''    SP.Q.Hit("F6")
    '' '' ''    If IsSchool Then 'if the contact was a school
    '' '' ''        If SP.Q.Check4Text(8, 18, " ") = False Then
    '' '' ''            'only populate source if blank
    '' '' ''            SP.Q.PutText(8, 18, Source)
    '' '' ''            SP.Q.PutText(9, 18, "04")
    '' '' ''        End If
    '' '' ''        SP.Q.PutText(10, 32, SP.Q.GetText(10, 32, 2))
    '' '' ''        SP.Q.PutText(10, 35, SP.Q.GetText(10, 35, 2))
    '' '' ''        SP.Q.PutText(10, 38, SP.Q.GetText(10, 38, 2))
    '' '' ''        SP.Q.PutText(11, 55, SP.Q.GetText(11, 55, 1))
    '' '' ''    End If
    '' '' ''    If AddressNeedsUpdate Or AddressValidityNeedsUpdate Then 'update the address validity indicator if the address was changed or verified
    '' '' ''        If SP.Q.Check4Text(8, 13, "CODE") Then
    '' '' ''            SP.Q.PutText(8, 18, Source)
    '' '' ''        End If
    '' '' ''        SP.Q.PutText(10, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
    '' '' ''        If AddressNotValid_Checked Then
    '' '' ''            SP.Q.PutText(11, 55, "N")
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(11, 55, "Y")
    '' '' ''        End If
    '' '' ''        If AddressNeedsUpdate Then
    '' '' ''            SP.Q.PutText(11, 10, Addr1.Trim)
    '' '' ''            If Len(Addr1) < 30 Then SP.Q.Hit("End") 'all these if statements clear out info not typed over
    '' '' ''            SP.Q.PutText(12, 10, Addr2.Trim)
    '' '' ''            If Len(Addr2) < 30 Then SP.Q.Hit("End")
    '' '' ''            SP.Q.PutText(13, 10, "")
    '' '' ''            SP.Q.Hit("End")
    '' '' ''            SP.Q.PutText(14, 8, City.Trim)
    '' '' ''            If Len(City) < 20 Then SP.Q.Hit("End")
    '' '' ''            SP.Q.PutText(14, 32, State.Trim)
    '' '' ''            SP.Q.PutText(14, 40, "")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''            SP.Q.PutText(14, 40, Zip.Trim)
    '' '' ''        End If
    '' '' ''        SP.Q.Hit("Enter")
    '' '' ''        If SP.Q.Check4Text(23, 2, "01096 ADDRESS DATA UPDATED") = False Then
    '' '' ''            AddressSaved = False
    '' '' ''            'SP.UserInputWait()
    '' '' ''            'SP.Q.Hit("Enter")

    '' '' ''        End If
    '' '' ''    End If
    '' '' ''    SP.Q.Hit("F6")
    '' '' ''    'update phone and phone indicator
    '' '' ''    If PhoneExt <> Trim(Replace(SP.Q.GetText(17, 40, 5), "_", "")) Then PhoneNeedsUpdate = True

    '' '' ''    If PhoneNeedsUpdate Or PhoneValidityNeedsUpdate Then
    '' '' ''        If SP.Q.Check4Text(23, 2, "01103 PHONE TYPE DOES NOT CURRENTLY EXIST - TO ADD, ENTER PHONE NUMBER DATA") And (OtherPhone = "" Or PhoneNum = "8015551212") Then
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(16, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
    '' '' ''            SP.Q.PutText(19, 14, PSource)
    '' '' ''            SP.Q.PutText(17, 54, "Y")
    '' '' ''            SP.Q.PutText(18, 15, "")
    '' '' ''            SP.Q.Hit("End")
    '' '' ''            SP.Q.PutText(18, 24, "")
    '' '' ''            SP.Q.Hit("End")
    '' '' ''            SP.Q.PutText(18, 36, "")
    '' '' ''            SP.Q.Hit("End")
    '' '' ''            SP.Q.PutText(18, 53, "")
    '' '' ''            SP.Q.Hit("End")
    '' '' ''            If PhoneValidityNeedsUpdate Then
    '' '' ''                If PhoneNotValid_Checked Then
    '' '' ''                    If AltPhoneNotValid_Checked = False Then
    '' '' ''                        'update phone with alternate phone
    '' '' ''                        PhoneNeedsUpdate = True
    '' '' ''                        PhoneNum = OtherPhone
    '' '' ''                        PhoneExt = OtherExt
    '' '' ''                        AltPhoneNotValid_Checked = True 'invalidate alt phone
    '' '' ''                        AltPhoneValidityNeedsUpdate = True
    '' '' ''                        PhoneNotValid_Checked = False
    '' '' ''                    Else
    '' '' ''                        SP.Q.PutText(17, 54, "N")
    '' '' ''                    End If
    '' '' ''                Else
    '' '' ''                    SP.Q.PutText(17, 54, "Y")
    '' '' ''                End If
    '' '' ''            End If
    '' '' ''            If PhoneNeedsUpdate Then
    '' '' ''                If PhoneNum <> "8015551212" Then 'if no phone wasn't selected
    '' '' ''                    SP.Q.PutText(17, 14, PhoneNum.Trim) 'update phone
    '' '' ''                    If PhoneExt <> "" Then
    '' '' ''                        SP.Q.PutText(17, 40, PhoneExt.Trim)
    '' '' ''                        If Len(PhoneExt) < 5 Then SP.Q.Hit("End")
    '' '' ''                    Else
    '' '' ''                        SP.Q.PutText(17, 40, "")
    '' '' ''                        SP.Q.Hit("End")
    '' '' ''                    End If
    '' '' ''                    SP.Q.PutText(17, 67, "") 'unmark no phone indicator
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    If PhoneNotValid_Checked Then
    '' '' ''                        SP.Q.PutText(17, 54, "N")
    '' '' ''                    Else
    '' '' ''                        SP.Q.PutText(17, 54, "Y")
    '' '' ''                    End If
    '' '' ''                Else
    '' '' ''                    SP.Q.PutText(17, 14, "8015551212")
    '' '' ''                    SP.Q.Hit("END")
    '' '' ''                    SP.Q.PutText(17, 54, "N")
    '' '' ''                End If

    '' '' ''                If SP.Q.Check4Text(17, 14, "_") = False Then 'if phone is not blank then clear foreign phone
    '' '' ''                    SP.Q.PutText(18, 15, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 24, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 36, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 53, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                End If
    '' '' ''            End If
    '' '' ''            SP.Q.Hit("Enter")
    '' '' ''            If SP.Q.Check4Text(23, 2, "01097 PHONE DATA UPDATED") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01100") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "04323") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01022") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01299") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01003") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "03417") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01005") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01099") = False Then
    '' '' ''                PhoneSaved = False
    '' '' ''                SP.Q.EmailScreenError()
    '' '' ''                'SP.UserInputWait()
    '' '' ''                'SP.Q.Hit("Enter")
    '' '' ''            End If
    '' '' ''        End If
    '' '' ''    End If
    '' '' ''    If OtherExt <> Trim(Replace(SP.Q.GetText(17, 40, 5), "_", "")) Then AltPhoneNeedsUpdate = True
    '' '' ''    If AltPhoneNeedsUpdate Or AltPhoneValidityNeedsUpdate Then
    '' '' ''        'change to alternate phone
    '' '' ''        SP.Q.PutText(16, 14, "A")
    '' '' ''        SP.Q.Hit("Enter")
    '' '' ''        '<SR#2251->
    '' '' ''        'check for error condition that creates queue tasks
    '' '' ''        If SP.Q.Check4Text(17, 14, "_") And SP.Q.Check4Text(17, 54, "Y") And SP.Q.Check4Text(17, 67, "X") Then
    '' '' ''            SP.Q.PutText(17, 54, "N") 'phn # isn't valid
    '' '' ''            SP.Q.PutText(17, 67, "") 'remove no phone indicator
    '' '' ''            SP.Q.Hit("End")
    '' '' ''            SP.Q.PutText(16, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
    '' '' ''            SP.Q.PutText(19, 14, PSource, True)
    '' '' ''        End If
    '' '' ''        '</SR#2251>
    '' '' ''        If SP.Q.Check4Text(23, 2, "01103 PHONE TYPE DOES NOT CURRENTLY EXIST - TO ADD, ENTER PHONE NUMBER DATA") And (OtherPhone = "" Or OtherPhone = "8015551212") Then
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(16, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
    '' '' ''            SP.Q.PutText(19, 14, PSource)
    '' '' ''            If AltPhoneValidityNeedsUpdate Then
    '' '' ''                If AltPhoneNotValid_Checked Then
    '' '' ''                    'If AltPhoneNotValid_Checked And Other <> "8015551212" Then
    '' '' ''                    'SP.Q.PutText(17, 57 + Z + Z + Z, "N")
    '' '' ''                    SP.Q.PutText(17, 54, "N")
    '' '' ''                Else
    '' '' ''                    'SP.Q.PutText(17, 57 + Z + Z + Z, "Y")
    '' '' ''                    SP.Q.PutText(17, 54, "Y")
    '' '' ''                End If
    '' '' ''            End If
    '' '' ''            If AltPhoneNeedsUpdate Then
    '' '' ''                If OtherPhone <> "8015551212" Then   'if no phone wasn't selected
    '' '' ''                    'SP.Q.PutText(17, 15 + Z, SP.Bor.OtherPhone.Trim) 'update phone
    '' '' ''                    SP.Q.PutText(17, 14, OtherPhone.Trim) 'update phone
    '' '' ''                    If OtherExt <> "" Then
    '' '' ''                        SP.Q.PutText(17, 40, OtherExt.Trim)
    '' '' ''                        If Len(OtherExt) < 5 Then SP.Q.Hit("End")
    '' '' ''                    Else
    '' '' ''                        SP.Q.PutText(17, 40, "")
    '' '' ''                        SP.Q.Hit("End")
    '' '' ''                    End If
    '' '' ''                    SP.Q.PutText(17, 67, "") 'unmark no phone indicator
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    If AltPhoneNotValid_Checked Then
    '' '' ''                        SP.Q.PutText(17, 54, "N")
    '' '' ''                    Else
    '' '' ''                        SP.Q.PutText(17, 54, "Y")
    '' '' ''                    End If
    '' '' ''                Else
    '' '' ''                    'SP.Q.PutText(17, 15 + Z, "") 'blank phone
    '' '' ''                    SP.Q.PutText(17, 14, "") 'blank phone
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.Hit("Tab")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.Hit("Tab")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.Hit("Tab")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 15, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 24, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 36, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 53, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(17, 67, "X") 'mark no phone indicator
    '' '' ''                    SP.Q.PutText(17, 54, "Y")
    '' '' ''                    'If Not AltPhoneNotValid_Checked Then sp.q.puttext(17, 67, "X") 'mark no phone indicator
    '' '' ''                End If
    '' '' ''            End If
    '' '' ''            SP.Q.Hit("Enter")
    '' '' ''            If SP.Q.Check4Text(23, 2, "01097 PHONE DATA UPDATED") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01100") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "04323") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01022") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01299") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01003") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "03417") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01005") = False Then
    '' '' ''                OtherSaved = False
    '' '' ''                SP.Q.EmailScreenError()
    '' '' ''                'SP.UserInputWait()
    '' '' ''                'SP.Q.Hit("Enter")
    '' '' ''            End If
    '' '' ''            'If (SP.Bor.OtherPhone2 = "" And SP.Q.Check4Text(17, 57 + Z + Z + Z, "N")) = False Then

    '' '' ''            'End If

    '' '' ''        End If
    '' '' ''    End If

    '' '' ''    If Other2Ext <> Trim(Replace(SP.Q.GetText(17, 40, 5), "_", "")) Then WorkPhoneNeedsUpdate = True
    '' '' ''    If WorkPhoneNeedsUpdate Or WorkPhoneValidityNeedsUpdate Then
    '' '' ''        'change to work phone
    '' '' ''        SP.Q.PutText(16, 14, "W")
    '' '' ''        SP.Q.Hit("Enter")
    '' '' ''        '<SR#2251->
    '' '' ''        'check for error condition that creates queue tasks
    '' '' ''        If SP.Q.Check4Text(17, 14, "_") And SP.Q.Check4Text(17, 54, "Y") And SP.Q.Check4Text(17, 67, "X") Then
    '' '' ''            SP.Q.PutText(17, 54, "N") 'phn # isn't valid
    '' '' ''            SP.Q.PutText(17, 67, "") 'remove no phone indicator
    '' '' ''            SP.Q.Hit("End")
    '' '' ''            SP.Q.PutText(16, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
    '' '' ''            SP.Q.PutText(19, 14, PSource, True)
    '' '' ''        End If
    '' '' ''        '</SR#2251>
    '' '' ''        If SP.Q.Check4Text(23, 2, "01103 PHONE TYPE DOES NOT CURRENTLY EXIST - TO ADD, ENTER PHONE NUMBER DATA") And (OtherPhone2 = "" Or OtherPhone2 = "8015551212") Then
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(16, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
    '' '' ''            SP.Q.PutText(19, 14, PSource)
    '' '' ''            If WorkPhoneValidityNeedsUpdate Then
    '' '' ''                If WorkPhoneNotValid_Checked Then
    '' '' ''                    SP.Q.PutText(17, 54, "N")
    '' '' ''                Else
    '' '' ''                    SP.Q.PutText(17, 54, "Y")
    '' '' ''                End If
    '' '' ''            End If
    '' '' ''            If WorkPhoneNeedsUpdate Then
    '' '' ''                SP.Q.PutText(17, 54, "Y")
    '' '' ''                If OtherPhone2 <> "8015551212" And OtherPhone2 <> "" Then  'if no phone wasn't selected
    '' '' ''                    SP.Q.PutText(17, 14, OtherPhone2.Trim) 'update phone
    '' '' ''                    If Other2Ext <> "" Then
    '' '' ''                        SP.Q.PutText(17, 40, Other2Ext.Trim)
    '' '' ''                        If Len(Other2Ext) < 5 Then SP.Q.Hit("End")
    '' '' ''                    Else
    '' '' ''                        SP.Q.PutText(17, 40, "")
    '' '' ''                        SP.Q.Hit("End")
    '' '' ''                    End If
    '' '' ''                    SP.Q.PutText(17, 67, "") 'unmark no phone indicator
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    If WorkPhoneNotValid_Checked Then
    '' '' ''                        'If WorkPhoneNotValid_Checked And Other2 <> "8015551212" Then
    '' '' ''                        'SP.Q.PutText(17, 57 + Z + Z + Z, "N")
    '' '' ''                        SP.Q.PutText(17, 54, "N")
    '' '' ''                    Else
    '' '' ''                        'SP.Q.PutText(17, 57 + Z + Z + Z, "Y")
    '' '' ''                        SP.Q.PutText(17, 54, "Y")
    '' '' ''                    End If
    '' '' ''                Else
    '' '' ''                    SP.Q.PutText(17, 14, "") 'blank phone
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.Hit("Tab")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.Hit("Tab")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.Hit("Tab")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 15, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 24, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 36, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(18, 53, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                    SP.Q.PutText(17, 67, "X") 'mark no phone indicator
    '' '' ''                    SP.Q.PutText(17, 54, "Y")
    '' '' ''                    'If Not WorkPhoneNotValid_Checked Then SP.Q.PutText(17, 67 + Z, "X") 'mark no phone indicator
    '' '' ''                End If
    '' '' ''            End If
    '' '' ''            SP.Q.Hit("Enter")
    '' '' ''            If SP.Q.Check4Text(23, 2, "01097 PHONE DATA UPDATED") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01100") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "04323") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01022") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01299") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01003") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "03417") = False And _
    '' '' ''               SP.Q.Check4Text(23, 2, "01005") = False Then
    '' '' ''                Other2Saved = False
    '' '' ''                SP.Q.EmailScreenError()
    '' '' ''                'SP.UserInputWait()
    '' '' ''                'SP.Q.Hit("Enter")
    '' '' ''            End If
    '' '' ''        End If

    '' '' ''    End If

    '' '' ''    'E-mail
    '' '' ''    If EmailNeedsUpdate Or EmailValidityNeedsUpdate Then 'update email indicator
    '' '' ''        SP.Q.Hit("F2")
    '' '' ''        SP.Q.Hit("F10")
    '' '' ''        'check to be sure the COMPASS Email address has something in it if the user blanked out the email
    '' '' ''        If Email <> "" Or SP.Q.Check4Text(15, 10, "_") = False Then
    '' '' ''            SP.Q.PutText(10, 20, Source)
    '' '' ''            SP.Q.PutText(12, 17, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
    '' '' ''            If SP.Q.Check4Text(15, 10, "_") = False And Email = "" Then 'update email only if the user gave a new email else mark as unvalid
    '' '' ''                SP.Q.PutText(13, 14, "N") 'unvalidate
    '' '' ''            Else
    '' '' ''                SP.Q.PutText(13, 14, "Y") 'validate
    '' '' ''            End If
    '' '' ''            If EmailNeedsUpdate Then 'update email 
    '' '' ''                If Email <> "" Then
    '' '' ''                    SP.Q.PutText(15, 10, Email.Trim)
    '' '' ''                    SP.Q.Hit("END")
    '' '' ''                    If Len(Email) < 67 Then SP.Q.Hit("END")
    '' '' ''                End If

    '' '' ''            End If
    '' '' ''            If EmailNotValid_Checked Then
    '' '' ''                SP.Q.PutText(13, 14, "N")
    '' '' ''            Else
    '' '' ''                SP.Q.PutText(13, 14, "Y")
    '' '' ''            End If
    '' '' ''            SP.Q.Hit("Enter")
    '' '' ''            If SP.Q.Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED") = False And SP.Q.Check4Text(23, 2, "01004") = False Then
    '' '' ''                EmailSaved = False
    '' '' ''                SP.Q.EmailScreenError()
    '' '' ''                'SP.UserInputWait()
    '' '' ''                'SP.Q.Hit("Enter")
    '' '' ''            End If
    '' '' ''            SP.Q.Hit("F12")
    '' '' ''        End If
    '' '' ''    End If

    '' '' ''End Sub

    '' '' ''Public Overrides Function GetOneLINKInfo() As Boolean
    '' '' ''    'determine what was passed to the object a account number (length = 10) or SSN 
    '' '' ''    '****SP.Bor.BorrowerIsLCOOnly = False
    '' '' ''    'Public Function GetOneLINKInfo(ByRef Bor As Borrower) As Boolean
    '' '' ''    If SSN.Length = 10 Then
    '' '' ''        SP.Q.FastPath("LP22I;;;;;;" & SSN)
    '' '' ''        '****Delete line above and if then statement around
    '' '' ''    Else
    '' '' ''        SP.Q.FastPath("LP22I" & SSN)
    '' '' ''    End If
    '' '' ''    'check if the user is logged into OneLINK and COMPASS
    '' '' ''    If SP.Q.Check4Text(1, 2, "INVALID COMMAND SYNTAX") Or SP.Q.Check4Text(3, 11, "CICS0001: OPERATOR IS NOT LOGGED ON.") Then
    '' '' ''        'GetExistingSession(SSN)
    '' '' ''        If SP.Q.RIBM Is Nothing Then
    '' '' ''            SP.frmWipeOut.WipeOut("That totally didn't work out.  How about you log in to OneLINK and COMPASS.", "Log In Problem", True)
    '' '' ''            Return False
    '' '' ''        End If
    '' '' ''    End If
    '' '' ''    'is the borrower located on OneLINK
    '' '' ''    If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
    '' '' ''        'WhoaDude.WhoaDUDE("That was totally knarly, but DUDE needs a valid SSN or account number from OneLINK.", "Knarly DUDE", True)
    '' '' ''        Return False
    '' '' ''    End If
    '' '' ''    ''warn the user if the borrower has a foreign address
    '' '' ''    'If sp.q.GetText(13, 52, 2) = "FC" Then
    '' '' ''    '    WhoaDude.WhoaDUDE("Sorry DUDE, this borrower is way outta town.  You'll hafta update their foreign OneLINK address manually.", "Knarly DUDE", True)
    '' '' ''    '    Return False
    '' '' ''    'End If
    '' '' ''    'gather the Demographic information
    '' '' ''    SSN = SP.Q.GetText(3, 23, 9) 'SSN
    '' '' ''    CLAccNum = SP.Q.GetText(3, 60, 12).Replace(" ", "") 'account number
    '' '' ''    Name = SP.Q.GetText(4, 44, 12) & " " & SP.Q.GetText(4, 60, 1) & " " & SP.Q.GetText(4, 5, 35) 'name
    '' '' ''    FirstName = SP.Q.GetText(4, 44, 12)
    '' '' ''    MI = SP.Q.GetText(4, 60, 1)
    '' '' ''    LastName = SP.Q.GetText(4, 5, 35)
    '' '' ''    DOB = SP.Q.GetText(4, 72, 8) 'Date of Birth
    '' '' ''    DOB = DOB.Insert(4, "/")
    '' '' ''    DOB = DOB.Insert(2, "/")
    '' '' ''    'sp.Bor.DOB = sp.Bor.DOB
    '' '' ''    Addr1 = SP.Q.GetText(11, 9, 35) 'address 1
    '' '' ''    Addr2 = SP.Q.GetText(12, 9, 35) 'address 2
    '' '' ''    SPAddrInd = SP.Q.GetText(11, 57, 1)
    '' '' ''    SPAddrVerDt = SP.Q.GetText(11, 72, 8) 'tp
    '' '' ''    SPAddrVerDt = SPAddrVerDt.Insert(4, "/")
    '' '' ''    SPAddrVerDt = SPAddrVerDt.Insert(2, "/")
    '' '' ''    City = SP.Q.GetText(13, 9, 30)
    '' '' ''    State = SP.Q.GetText(13, 52, 2)
    '' '' ''    Zip = SP.Q.GetText(13, 60, 9)
    '' '' ''    PhoneNum = SP.Q.GetText(14, 17, 10)
    '' '' ''    PhoneExt = SP.Q.GetText(14, 38, 4)
    '' '' ''    SPPhnInd = SP.Q.GetText(14, 56, 1)
    '' '' ''    SPPhnVerDt = SP.Q.GetText(14, 72, 8) 'tp
    '' '' ''    SPPhnVerDt = SPPhnVerDt.Insert(4, "/")
    '' '' ''    SPPhnVerDt = SPPhnVerDt.Insert(2, "/")
    '' '' ''    OtherPhone = SP.Q.GetText(15, 17, 10) 'tp
    '' '' ''    OtherExt = SP.Q.GetText(15, 38, 4)
    '' '' ''    SPOtPhnInd = SP.Q.GetText(15, 62, 1) 'tp
    '' '' ''    Email = SP.Q.GetText(19, 9, 56)
    '' '' ''    SPEmailInd = SP.Q.GetText(18, 56, 1)
    '' '' ''    SPEmailVerDt = SP.Q.GetText(18, 71, 8)
    '' '' ''    SPEmailVerDt = SPEmailVerDt.Insert(4, "/")
    '' '' ''    SPEmailVerDt = SPEmailVerDt.Insert(2, "/")
    '' '' ''    'IF the foreign address if populate the change the state to FC so dude will think its a foreign address
    '' '' ''    If SP.Q.GetText(16, 16, 10) <> "" Then State = "FC"
    '' '' ''    Return True
    '' '' ''End Function

    '' '' ''Public Overrides Sub SetOneLINKInfo(ByVal Source As String, ByVal AddressNeedsUpdate As Boolean, ByVal PhoneNeedsUpdate As Boolean, ByVal AltPhoneNeedsUpdate As Boolean, ByVal EmailNeedsUpdate As Boolean, ByVal AddressValidityNeedsUpdate As Boolean, ByVal PhoneValidityNeedsUpdate As Boolean, ByVal AltPhoneValidityNeedsUpdate As Boolean, ByVal EmailValidityNeedsUpdate As Boolean, ByVal AddressNotValid_Checked As Boolean, ByVal PhoneNotValid_Checked As Boolean, ByVal AltPhoneNotValid_Checked As Boolean, ByVal EmailNotValid_Checked As Boolean, ByVal CompassClass As MDBorrowerDemographics)
    '' '' ''    Dim TS As New TimeSpan(0, 0, 1)
    '' '' ''    SP.Q.FastPath("LP22C" & SSN)
    '' '' ''    'switch to address info
    '' '' ''    SP.Q.PutText(3, 9, Source)
    '' '' ''    If CompassClass.AddressSaved Then
    '' '' ''        If AddressNeedsUpdate Or AddressValidityNeedsUpdate Then 'update the address validity indicator if the address was changed or verified
    '' '' ''            SP.Q.PutText(11, 72, CStr(Format(Today, "MM/dd/yyyy")).Replace("/", ""))
    '' '' ''            If AddressNeedsUpdate Then 'update address
    '' '' ''                SP.Q.PutText(11, 9, Addr1.Trim)
    '' '' ''                If Len(Addr1) < 35 Then SP.Q.Hit("End") 'all these if statements clear out info not typed over
    '' '' ''                SP.Q.PutText(12, 9, Addr2.Trim)
    '' '' ''                If Len(Addr2) < 35 Then SP.Q.Hit("End")
    '' '' ''                SP.Q.PutText(13, 9, City.Trim)
    '' '' ''                If Len(City) < 30 Then SP.Q.Hit("End")
    '' '' ''                SP.Q.PutText(13, 52, State.Trim)
    '' '' ''                SP.Q.PutText(13, 60, Zip.Trim)
    '' '' ''                If Len(Zip) < 9 Then SP.Q.Hit("END")
    '' '' ''            End If
    '' '' ''            If AddressNotValid_Checked Then
    '' '' ''                SP.Q.PutText(11, 57, "N") 'address is Invalid
    '' '' ''            Else
    '' '' ''                SP.Q.PutText(11, 57, "Y") 'address is valid
    '' '' ''            End If
    '' '' ''        End If
    '' '' ''    End If
    '' '' ''    'update phone and phone indicator if needed
    '' '' ''    If CompassClass.PhoneSaved Then
    '' '' ''        If PhoneNeedsUpdate Or PhoneValidityNeedsUpdate Then 'update indicator
    '' '' ''            If SP.Q.Check4Text(16, 16, "_________________") = False Then
    '' '' ''                SP.Q.PutText(16, 16, "")
    '' '' ''                SP.Q.Hit("End")
    '' '' ''            End If
    '' '' ''            SP.Q.PutText(14, 72, CStr(Format(Today, "MM/dd/yyyy")).Replace("/", ""))
    '' '' ''            If PhoneNum <> "8015551212" Then
    '' '' ''                If PhoneNotValid_Checked Then
    '' '' ''                    'check if the alt phone is valid and put it in the home phone place if it is
    '' '' ''                    If AltPhoneNotValid_Checked = False Then
    '' '' ''                        PhoneNeedsUpdate = True 'do update with alt phone data
    '' '' ''                        PhoneNum = OtherPhone
    '' '' ''                        PhoneExt = OtherExt
    '' '' ''                        AltPhoneNotValid_Checked = True 'make the alt phone invalid since the phone number has that valid data now
    '' '' ''                        AltPhoneValidityNeedsUpdate = True
    '' '' ''                    Else
    '' '' ''                        SP.Q.PutText(14, 56, "N") 'phone is valid
    '' '' ''                    End If
    '' '' ''                Else
    '' '' ''                    SP.Q.PutText(14, 56, "Y") 'phone is valid
    '' '' ''                End If
    '' '' ''            Else
    '' '' ''                SP.Q.PutText(14, 56, "N")       'home phone not valid
    '' '' ''                SP.Q.PutText(14, 17, "8015551212")
    '' '' ''                SP.Q.Hit("END")
    '' '' ''            End If
    '' '' ''            If PhoneNeedsUpdate Then 'update phone
    '' '' ''                SP.Q.PutText(14, 17, PhoneNum.Trim)
    '' '' ''                If PhoneExt <> "" Then
    '' '' ''                    SP.Q.PutText(14, 38, PhoneExt.Trim)
    '' '' ''                    If Len(PhoneExt) < 4 Then SP.Q.Hit("End")
    '' '' ''                Else
    '' '' ''                    SP.Q.PutText(14, 38, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                End If
    '' '' ''            End If

    '' '' ''        End If
    '' '' ''    End If
    '' '' ''    'update other phone and  other phone indicator if needed
    '' '' ''    If CompassClass.OtherSaved Then
    '' '' ''        If AltPhoneNeedsUpdate Or AltPhoneValidityNeedsUpdate Then 'update indicator
    '' '' ''            If OtherPhone <> "8015551212" Then
    '' '' ''                'SP.Q.PutText(15, 62, "Y")
    '' '' ''                If AltPhoneNotValid_Checked Then
    '' '' ''                    SP.Q.PutText(15, 62, "N") 'Other phone not valid
    '' '' ''                Else
    '' '' ''                    SP.Q.PutText(15, 62, "Y") 'Other phone is valid
    '' '' ''                End If
    '' '' ''            Else
    '' '' ''                SP.Q.PutText(15, 62, "N")
    '' '' ''            End If
    '' '' ''            If AltPhoneNeedsUpdate Then 'update other phone
    '' '' ''                SP.Q.PutText(15, 17, OtherPhone.Trim)
    '' '' ''                If OtherExt <> "" Then
    '' '' ''                    SP.Q.PutText(15, 38, OtherExt.Trim)
    '' '' ''                    If Len(OtherExt) < 4 Then SP.Q.Hit("End")
    '' '' ''                Else
    '' '' ''                    SP.Q.PutText(15, 38, "")
    '' '' ''                    SP.Q.Hit("End")
    '' '' ''                End If
    '' '' ''            End If

    '' '' ''        End If
    '' '' ''    End If
    '' '' ''    'check to be sure the OneLINK Email address has something in it if the user blanked out the email
    '' '' ''    If CompassClass.EmailSaved Then
    '' '' ''        If Email <> "" Or SP.Q.Check4Text(19, 9, "_") = False Then
    '' '' ''            If EmailNeedsUpdate Or EmailValidityNeedsUpdate Then
    '' '' ''                If EmailNotValid_Checked Then
    '' '' ''                    SP.Q.PutText(18, 56, "N") 'Other phone not valid
    '' '' ''                Else
    '' '' ''                    If EmailNeedsUpdate Then
    '' '' ''                        'only update the email if the user entered a new email
    '' '' ''                        If Email <> "" Then 'update email
    '' '' ''                            SP.Q.PutText(19, 9, Email)
    '' '' ''                            If Len(Email.Trim) < 56 Then SP.Q.Hit("END")
    '' '' ''                        End If
    '' '' ''                    End If
    '' '' ''                    SP.Q.PutText(18, 56, "Y") 'Other phone is valid
    '' '' ''                End If
    '' '' ''            End If
    '' '' ''        End If
    '' '' ''    End If
    '' '' ''    SP.Q.Hit("Enter")
    '' '' ''    If SP.Q.Check4Text(22, 3, "46012 SCREEN VALIDATED - PRESS F6 TO POST") = False Then
    '' '' ''        Threading.Thread.CurrentThread.Sleep(100) 'pause for one second
    '' '' ''        SP.Q.Hit("Enter")
    '' '' ''        If SP.Q.Check4Text(22, 3, "46012 SCREEN VALIDATED - PRESS F6 TO POST") = False Then
    '' '' ''            SP.UserInputWait()
    '' '' ''            SP.Q.Hit("Enter")
    '' '' ''        End If
    '' '' ''    End If
    '' '' ''    SP.Q.Hit("F6")
    '' '' ''    If SP.Q.Check4Text(22, 3, "49000 DATA SUCCESSFULLY UPDATED") = False Then
    '' '' ''        SP.UserInputWait()
    '' '' ''        SP.Q.Hit("F6")
    '' '' ''    End If
    '' '' ''End Sub

    '' '' ''Public Overrides Function GetLCODemographics() As Boolean
    '' '' ''    SP.Processing.Visible = True
    '' '' ''    SP.LoginToLCO()
    '' '' ''    SP.Q.FastPath("TPDD" & SSN)
    '' '' ''    If SP.Q.Check4Text(1, 19, "LCO PERSONAL INFORMATION DISPLAY") = True Then
    '' '' ''        'Gather LCO demographic info
    '' '' ''        FirstName = Replace(SP.Q.GetText(4, 19, 12), "_", " ")
    '' '' ''        LastName = Replace(SP.Q.GetText(4, 35, 22), "_", " ")
    '' '' ''        Name = FirstName & " " & LastName
    '' '' ''        Addr1 = Replace(SP.Q.GetText(6, 19, 29), "_", " ")
    '' '' ''        Addr2 = Replace(SP.Q.GetText(7, 19, 29), "_", " ")
    '' '' ''        City = Replace(SP.Q.GetText(9, 19, 19), "_", " ")
    '' '' ''        State = Replace(SP.Q.GetText(9, 50, 2), "_", " ")
    '' '' ''        Zip = Replace(SP.Q.GetText(9, 66, 5), "_", " ")
    '' '' ''        PhoneNum = Replace(SP.Q.GetText(13, 13, 12), "_", " ")
    '' '' ''        OtherPhone = Replace(SP.Q.GetText(14, 13, 12), "_", " ")
    '' '' ''        Email = Replace(SP.Q.GetText(19, 19, 55), "_", " ")
    '' '' ''        DOB = Replace(SP.Q.GetText(5, 61, 10), " ", "/")
    '' '' ''        CLAccNum = GetText(3, 62, 12).Replace("-", "")
    '' '' ''        'remove extra spaces
    '' '' ''        Addr1 = Trim(Addr1)
    '' '' ''        Addr2 = Trim(Addr2)
    '' '' ''        City = Trim(City)
    '' '' ''        State = Trim(State)
    '' '' ''        Zip = Trim(Zip)
    '' '' ''        PhoneNum = Trim(PhoneNum)
    '' '' ''        OtherPhone = Trim(OtherPhone)
    '' '' ''        Email = Trim(Email)
    '' '' ''        DOB = Trim(DOB)

    '' '' ''        SP.LoginToCompass()
    '' '' ''        SP.Processing.Visible = False
    '' '' ''        Return True
    '' '' ''    Else
    '' '' ''        SP.LoginToCompass()
    '' '' ''        SP.Processing.Visible = False
    '' '' ''        SP.frmWhoaDUDE.WhoaDUDE("That was totally knarly, but DUDE needs a valid SSN or account number from OneLINK.", "Knarly DUDE", True)
    '' '' ''        Return False
    '' '' ''    End If
    '' '' ''End Function

    '' '' ''Public Overrides Function SetLCODemographics() As Boolean
    '' '' ''    SP.Processing.Visible = True
    '' '' ''    SP.LoginToLCO()
    '' '' ''    SP.FastPath("TPDD" & SSN)
    '' '' ''    If SP.Q.Check4Text(1, 19, "LCO PERSONAL INFORMATION DISPLAY") = True Then
    '' '' ''        '****SP.Bor.BorrowerIsLCOOnly = True

    '' '' ''        'Set LCO demographic info
    '' '' ''        If Addr1 = "" Then
    '' '' ''            SP.Q.PutText(6, 19, " ")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(6, 19, "")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''            SP.Q.PutText(6, 19, Addr1)
    '' '' ''        End If
    '' '' ''        If Addr2 = "" Then
    '' '' ''            SP.Q.PutText(7, 19, " ")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(7, 19, "")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''            SP.Q.PutText(7, 19, Addr2)
    '' '' ''        End If
    '' '' ''        If City = "" Then
    '' '' ''            SP.Q.PutText(9, 19, " ")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(9, 19, "")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''            SP.Q.PutText(9, 19, City)
    '' '' ''        End If
    '' '' ''        SP.Q.PutText(9, 50, State)
    '' '' ''        SP.Q.PutText(9, 66, Zip)
    '' '' ''        SP.Q.Hit("END")
    '' '' ''        SP.Q.PutText(13, 13, PhoneNum)
    '' '' ''        If OtherPhone = "" Then
    '' '' ''            SP.Q.PutText(14, 13, " ")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''            SP.Q.Hit("TAB")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''            SP.Q.Hit("TAB")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(14, 13, OtherPhone)
    '' '' ''        End If
    '' '' ''        If Email = "" Then
    '' '' ''            SP.Q.PutText(19, 19, " ")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''        Else
    '' '' ''            SP.Q.PutText(19, 19, "")
    '' '' ''            SP.Q.Hit("END")
    '' '' ''            SP.Q.PutText(19, 19, Email)
    '' '' ''        End If
    '' '' ''        SP.Q.Hit("ENTER")
    '' '' ''        If SP.Q.Check4Text(23, 27, "IMPROPER CHARACTERS IN FIELD") Then SP.Q.Hit("ENTER")
    '' '' ''        SP.LoginToCompass()
    '' '' ''        Return True
    '' '' ''    End If
    '' '' ''    SP.LoginToCompass()
    '' '' ''    SP.Processing.Visible = False
    '' '' ''    Return False
    '' '' ''End Function

End Class
