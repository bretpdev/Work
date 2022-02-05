Imports Q
Imports System.Collections.Generic

Public Class Demographics
    Inherits MDBorrowerDemographics

    ''' <summary>
    ''' For all system collected objects.
    ''' </summary>
    ''' <param name="tSSN"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal tSSN As String)
        SSN = tSSN
        AddressSaved = True
        PhoneSaved = True
        OtherSaved = True
        Other2Saved = True
        EmailSaved = True
        TheSystem = WhatSystem.UserProvided 'ONLY FOR BACKWARD COMPATIBILITY
    End Sub

    Public Overrides Sub PopulateObjectFromSystem()
        frmWhoaDUDE.WhoaDUDE("Whoa, major problem dude!  Contact Systems Support for help!", "Major Problem!")
        Throw New SystemException("DUDE entered an uncoded version of 'PopulateObjectFromSystem' for demographics.")
    End Sub

    'checks which processing needs doing
    Private Shared Sub AltAddressAndInvalidateFirstProcCheck(ByVal checkboxIndicators As Dictionary(Of String, CheckboxIndicatorsForDemographicPart), ByVal altAddress As Demographics, ByRef altAddressIndicator As Boolean, ByRef invalidateFirstIndicator As Boolean)
        'alternate address check
        If altAddress Is Nothing Then
            altAddressIndicator = False
        Else
            altAddressIndicator = True
        End If
        'invalidate first check
        invalidateFirstIndicator = False
        For Each indicatorSet As CheckboxIndicatorsForDemographicPart In checkboxIndicators.Values.ToList()
            If indicatorSet.ckbInvalidateFirst.Checked Then
                invalidateFirstIndicator = True
            End If
        Next
    End Sub

#Region "Compass"

    Public Shared Sub UpdateCOMPASSSystem(ByVal Source As String, ByVal systemsUpdateIndicators As UpdateDemoCompassIndicators, ByVal isSchool As Boolean, ByVal demosForUpdating As Demographics, ByVal checkboxIndicators As Dictionary(Of String, CheckboxIndicatorsForDemographicPart), ByVal altAddress As Demographics)
        Dim PSource As String
        PSource = Source
        If PSource = "25" Then PSource = "31"

        'do alt address adding and invalidate first functionality
        COMPASSInvalidateFirstAndAlternateAddrProc(Source, checkboxIndicators, altAddress, isSchool, systemsUpdateIndicators.Address, demosForUpdating)

        SP.Q.FastPath("TX3Z/CTX1J;" & demosForUpdating.SSN)
        'don't update COMPASS if error 01019 is returned
        If SP.Q.Check4Text(23, 2, "01019") = True Then Exit Sub
        'switch to address info
        SP.Q.Hit("F6")
        SP.Q.Hit("F6")
        If isSchool Then 'if the contact was a school
            If SP.Q.Check4Text(8, 18, " ") = False Then
                'only populate source if blank
                SP.Q.PutText(8, 18, Source)
                SP.Q.PutText(9, 18, "04")
            End If
            SP.Q.PutText(10, 32, SP.Q.GetText(10, 32, 2))
            SP.Q.PutText(10, 35, SP.Q.GetText(10, 35, 2))
            SP.Q.PutText(10, 38, SP.Q.GetText(10, 38, 2))
            SP.Q.PutText(11, 55, SP.Q.GetText(11, 55, 1))
            SP.Q.Hit("Enter")
        End If
        If systemsUpdateIndicators.Address Or systemsUpdateIndicators.AddressIndicator Then 'update the address validity indicator if the address was changed or verified
            If SP.Q.Check4Text(8, 13, "CODE") Then
                SP.Q.PutText(8, 18, Source)
            End If
            SP.Q.PutText(10, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
            If checkboxIndicators("Address").ckbNotValid.Checked Then
                SP.Q.PutText(11, 55, "N")
            Else
                SP.Q.PutText(11, 55, "Y")
            End If
            If systemsUpdateIndicators.Address Then
                UpdateCOMPASSAddress(demosForUpdating)
            End If
            SP.Q.Hit("Enter")
            If SP.Q.Check4Text(23, 2, "01096 ADDRESS DATA UPDATED") = False Then
                AddressSaved = False

            End If
        End If
        SP.Q.Hit("F6")

        'Invalidate all phone numbers and set them to 8015551212 if No Phone checked
        If systemsUpdateIndicators.PhoneNoPhoneIndicator Then
            'Home Phone
            PutText(17, 14, "8015551212")
            Hit("End")
            PutText(16, 20, "U")
            PutText(16, 30, "N")
            PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
            PutText(19, 14, Source)
            PutText(17, 54, "N")
            Hit("End")
            Hit("Enter")
            'Other Phone
            PutText(16, 14, "A")
            Hit("Enter")
            PutText(17, 14, "8015551212")
            Hit("End")
            PutText(16, 20, "U")
            PutText(16, 30, "N")
            PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
            PutText(19, 14, Source)
            PutText(17, 54, "N")
            Hit("End")
            Hit("Enter")
            'Work Phone
            PutText(16, 14, "W")
            Hit("Enter")
            PutText(17, 14, "8015551212")
            Hit("End")
            PutText(16, 20, "U")
            PutText(16, 30, "N")
            PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
            PutText(19, 14, Source)
            PutText(17, 54, "N")
            Hit("End")
            Hit("Enter")
            'Set all the indicators to false to skip the rest of the phone updates
            systemsUpdateIndicators.Phone = False
            systemsUpdateIndicators.PhoneIndicator = False
            systemsUpdateIndicators.OtherPhone = False
            systemsUpdateIndicators.OtherPhoneIndicator = False
            systemsUpdateIndicators.Other2Phone = False
            systemsUpdateIndicators.Other2PhoneIndicator = False
        End If

        'Check if there was a change to the phone extension
        If demosForUpdating.HomePhoneExt <> Trim(Replace(GetText(17, 40, 5), "_", "")) Then systemsUpdateIndicators.PhoneIndicator = True

        If (Not systemsUpdateIndicators.Phone And systemsUpdateIndicators.OtherPhone) Then
            'treat primary as alternate and invalidate
            PutText(16, 14, "A")
            Hit("Enter")
        End If

        If systemsUpdateIndicators.Phone Or systemsUpdateIndicators.PhoneIndicator Then
            If Check4Text(23, 2, "01103 PHONE TYPE DOES NOT CURRENTLY EXIST - TO ADD, ENTER PHONE NUMBER DATA") And (demosForUpdating.OtherPhoneNum = "" Or demosForUpdating.HomePhoneNum = "8015551212") Then
            Else
                If systemsUpdateIndicators.PhoneIndicator Then
                    If demosForUpdating.HomePhoneNum <> "8015551212" Then 'if no phone wasn't selected
                        PutText(17, 14, demosForUpdating.HomePhoneNum.Trim) 'update phone
                        If demosForUpdating.HomePhoneMBL.Trim <> "" Then
                            PutText(16, 20, demosForUpdating.HomePhoneMBL.Trim) 'update MBL
                        Else
                            PutText(16, 20, "U") 'update MBL to U if no MBL selected
                        End If
                        PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", "")) 'update date
                        PutText(19, 14, Source) 'update source
                        PutText(17, 54, IIf(systemsUpdateIndicators.Phone, "Y", "N")) 'check as valid phone
                        If demosForUpdating.HomePhoneMBL = "L" OrElse checkboxIndicators("HomePhone").ckbConsent.Checked Then
                            PutText(16, 30, "Y") 'update consent
                        Else
                            PutText(16, 30, "N")
                        End If
                        If demosForUpdating.HomePhoneExt <> "" Then
                            PutText(17, 40, demosForUpdating.HomePhoneExt.Trim) 'update extension
                            If Len(demosForUpdating.HomePhoneExt) < 5 Then Hit("End")
                        Else
                            PutText(17, 40, "")
                            Hit("End")
                        End If
                        If (GetText(17, 67, 1) <> "") Then
                            PutText(17, 67, "")
                            Hit("End") 'unmark no phone indicator
                        End If
                    End If
                    End If
                If Check4Text(17, 14, "_") = False Then 'if phone is not blank then clear foreign phone
                    PutText(18, 15, "")
                    Hit("End")
                    PutText(18, 24, "")
                    Hit("End")
                    PutText(18, 36, "")
                    Hit("End")
                    PutText(18, 53, "")
                    Hit("End")
                End If
            End If
            Hit("Enter")
            If Check4Text(23, 2, "01097 PHONE DATA UPDATED") = False And
               Check4Text(23, 2, "01100") = False And
               Check4Text(23, 2, "04323") = False And
               Check4Text(23, 2, "01022") = False And
               Check4Text(23, 2, "01299") = False And
               Check4Text(23, 2, "01003") = False And
               Check4Text(23, 2, "03417") = False And
               Check4Text(23, 2, "01005") = False And
               Check4Text(23, 2, "01099") = False Then
                Demographics.PhoneSaved = False
                'UNDONE:EmailScreenError()
            End If
        End If

        If (Not systemsUpdateIndicators.Phone And systemsUpdateIndicators.OtherPhone) Then
            'treat alternate as home
            PutText(16, 14, "H")
        Else
            'change to alternate phone
            PutText(16, 14, "A")
        End If

        Hit("Enter")

        'Check to see if there is a change to the extension
        If demosForUpdating.OtherPhoneExt <> Trim(Replace(GetText(17, 40, 5), "_", "")) Then systemsUpdateIndicators.OtherPhoneIndicator = True

        'Update / Invalidate Phone 2
        If systemsUpdateIndicators.OtherPhone Or systemsUpdateIndicators.OtherPhoneIndicator Then
            'check for error condition that creates queue tasks
            If Check4Text(17, 14, "_") And Check4Text(17, 54, "Y") Then
                PutText(17, 54, "N") 'phn # isn't valid
                Hit("End")
                PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                PutText(19, 14, Source)
                Hit("Enter")
            End If
            If Check4Text(23, 2, "01103 PHONE TYPE DOES NOT CURRENTLY EXIST - TO ADD, ENTER PHONE NUMBER DATA") And (demosForUpdating.OtherPhoneNum = "" Or demosForUpdating.OtherPhoneNum = "8015551212") Then
            Else
                If systemsUpdateIndicators.OtherPhoneIndicator Then
                    If demosForUpdating.OtherPhoneNum <> "8015551212" Then 'if no phone wasn't selected
                        PutText(17, 14, demosForUpdating.OtherPhoneNum.Trim) 'update phone
                        If demosForUpdating.OtherPhoneMBL.Trim <> "" Then
                            PutText(16, 20, demosForUpdating.OtherPhoneMBL.Trim) 'update MBL
                        Else
                            PutText(16, 20, "U") 'update MBL to U if no MBL selected
                        End If
                        PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", "")) 'update date
                        PutText(19, 14, Source) 'update source
                        PutText(17, 54, IIf(systemsUpdateIndicators.OtherPhone, "Y", "N")) 'check as valid phone
                        If demosForUpdating.OtherPhoneMBL = "L" OrElse checkboxIndicators("OtherPhone").ckbConsent.Checked Then
                            PutText(16, 30, "Y") 'update consent
                        Else
                            PutText(16, 30, "N")
                        End If
                        If demosForUpdating.OtherPhoneExt <> "" Then
                            PutText(17, 40, demosForUpdating.OtherPhoneExt.Trim) 'update extension
                            If Len(demosForUpdating.OtherPhoneExt) < 5 Then Hit("End")
                        Else
                            PutText(17, 40, "")
                            Hit("End")
                        End If
                        If (GetText(17, 67, 1) <> "") Then
                            PutText(17, 67, "")
                            Hit("End") 'unmark no phone indicator
                        End If
                    Else
                        PutText(17, 54, "N")
                        PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", "")) 'update date
                        PutText(19, 14, Source) 'update source
                    End If
                End If

                If Check4Text(17, 14, "_") = False Then 'if phone is not blank then clear foreign phone
                    PutText(18, 15, "")
                    Hit("End")
                    PutText(18, 24, "")
                    Hit("End")
                    PutText(18, 36, "")
                    Hit("End")
                    PutText(18, 53, "")
                    Hit("End")
                End If

                Hit("Enter")
                If Check4Text(23, 2, "01097 PHONE DATA UPDATED") = False And
                   Check4Text(23, 2, "01100") = False And
                   Check4Text(23, 2, "04323") = False And
                   Check4Text(23, 2, "01022") = False And
                   Check4Text(23, 2, "01299") = False And
                   Check4Text(23, 2, "01003") = False And
                   Check4Text(23, 2, "03417") = False And
                   Check4Text(23, 2, "01005") = False Then
                    Demographics.OtherSaved = False
                    'UNDONE:EmailScreenError()
                End If
            End If
        End If

        'change to work phone
        PutText(16, 14, "W")
        Hit("Enter")

        'Check to see if there is a change to the extension
        If demosForUpdating.OtherPhone2Ext <> Trim(Replace(GetText(17, 40, 5), "_", "")) Then systemsUpdateIndicators.Other2PhoneIndicator = True

        If systemsUpdateIndicators.Other2Phone Or systemsUpdateIndicators.Other2PhoneIndicator Then
            'check for error condition that creates queue tasks
            If Check4Text(17, 14, "_") And Check4Text(17, 54, "Y") Then
                PutText(17, 54, "N") 'phn # isn't valid
                Hit("End")
                PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                PutText(19, 14, Source)
                Hit("Enter")
            End If
            If Check4Text(23, 2, "01103 PHONE TYPE DOES NOT CURRENTLY EXIST - TO ADD, ENTER PHONE NUMBER DATA") And (demosForUpdating.OtherPhone2Num = "" Or demosForUpdating.OtherPhone2Num = "8015551212") Then
            Else
                If systemsUpdateIndicators.Other2PhoneIndicator Then
                    If demosForUpdating.OtherPhone2Num <> "8015551212" Then 'if no phone wasn't selected
                        PutText(17, 14, demosForUpdating.OtherPhone2Num.Trim) 'update phone
                        If demosForUpdating.OtherPhone2MBL.Trim <> "" Then
                            PutText(16, 20, demosForUpdating.OtherPhone2MBL.Trim) 'update MBL
                        Else
                            PutText(16, 20, "U") 'update MBL to U if no MBL selected
                        End If
                        PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", "")) 'update date
                        PutText(19, 14, Source) 'update source
                        PutText(17, 54, IIf(systemsUpdateIndicators.Other2Phone, "Y", "N")) 'check as valid phone
                        If demosForUpdating.OtherPhone2MBL = "L" OrElse checkboxIndicators("OtherPhone2").ckbConsent.Checked Then
                            PutText(16, 30, "Y") 'update consent
                        Else
                            PutText(16, 30, "N")
                        End If
                        If demosForUpdating.OtherPhone2Ext <> "" Then
                            PutText(17, 40, demosForUpdating.OtherPhone2Ext.Trim) 'update extension
                            If Len(demosForUpdating.OtherPhone2Ext) < 5 Then Hit("End")
                        Else
                            PutText(17, 40, "")
                            Hit("End")
                        End If
                    Else

                        PutText(17, 54, "N")
                        PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", "")) 'update date
                        PutText(19, 14, Source) 'update source
                    End If
                End If

                If Check4Text(17, 14, "_") = False Then 'if phone is not blank then clear foreign phone
                    PutText(18, 15, "")
                    Hit("End")
                    PutText(18, 24, "")
                    Hit("End")
                    PutText(18, 36, "")
                    Hit("End")
                    PutText(18, 53, "")
                    Hit("End")
                End If

                Hit("Enter")
                If Check4Text(23, 2, "01097 PHONE DATA UPDATED") = False And
                   Check4Text(23, 2, "01100") = False And
                   Check4Text(23, 2, "04323") = False And
                   Check4Text(23, 2, "01022") = False And
                   Check4Text(23, 2, "01299") = False And
                   Check4Text(23, 2, "01003") = False And
                   Check4Text(23, 2, "03417") = False And
                   Check4Text(23, 2, "01005") = False Then
                    Demographics.Other2Saved = False
                    'UNDONE:EmailScreenError()
                End If
            End If
        End If

        If systemsUpdateIndicators.Email Or systemsUpdateIndicators.EmailIndicator Then 'update email indicator
            SP.Q.Hit("F2")
            For Each addressType As String In {"H", "C"}
                SP.Q.Hit("F10")
                SP.Q.PutText(10, 14, addressType)
                SP.Q.Hit("Enter")
                If demosForUpdating.Email <> "" Then
                    For R As Integer = 14 To 18
                        SP.Q.PutText(R, 10, "")
                        SP.Q.Hit("End")
                    Next
                    SP.Q.PutText(14, 10, demosForUpdating.Email.Trim)
                Else
                    Exit For
                End If
                If (addressType <> "C") Then
                    SP.Q.PutText(9, 20, Source)
                Else
                    SP.Q.PutText(9, 20, "41")
                End If
                SP.Q.PutText(11, 17, CStr(Format(Today, "MM/dd/yy")).Replace("/", "")) 'last verified today
                If (SP.Q.Check4Text(14, 10, "_") = False And demosForUpdating.Email = "") Or checkboxIndicators("Email").ckbNotValid.Checked Then 'update email only if the user gave a new email else mark as unvalid
                    SP.Q.PutText(12, 14, "N") 'unvalidate
                Else
                    SP.Q.PutText(12, 14, "Y") 'validate
                End If
                SP.Q.Hit("Enter")
                If SP.Q.Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED") = False And SP.Q.Check4Text(23, 2, "01004") = False Then
                    EmailSaved = False
                End If
                SP.Q.Hit("F12")
            Next
        End If

    End Sub

    Private Shared Sub COMPASSInvalidateFirstAndAlternateAddrProc(ByVal Source As String, ByVal checkboxIndicators As Dictionary(Of String, CheckboxIndicatorsForDemographicPart), ByVal altAddress As Demographics, ByVal isSchool As Boolean, ByRef updateAddressIndicator As Boolean, ByVal demosForUpdating As Demographics)
        Dim doAltAddressProcessing As Boolean = False
        Dim doInvalidateFirstProcessing As Boolean = False
        Dim PSource As String
        PSource = Source
        If PSource = "25" Then PSource = "31"
        AltAddressAndInvalidateFirstProcCheck(checkboxIndicators, altAddress, doAltAddressProcessing, doInvalidateFirstProcessing)
        If doAltAddressProcessing = True Or doInvalidateFirstProcessing = True Then
            FastPath("TX3Z/CTX1J;" & demosForUpdating.SSN)
            'don't update COMPASS if error 01019 is returned
            If Check4Text(23, 2, "01019") = True Then Exit Sub
            'switch to address info
            Hit("F6")
            Hit("F6")
            'alt address adding
            If doAltAddressProcessing Then
                If isSchool Then 'if the contact was a school
                    If Check4Text(8, 18, " ") = False Then
                        'only populate source if blank
                        PutText(8, 18, Source)
                        PutText(9, 18, "04")
                    End If
                    PutText(10, 32, SP.Q.GetText(10, 32, 2))
                    PutText(10, 35, SP.Q.GetText(10, 35, 2))
                    PutText(10, 38, SP.Q.GetText(10, 38, 2))
                    PutText(11, 55, SP.Q.GetText(11, 55, 1))
                End If
                If Check4Text(8, 13, "CODE") Then
                    PutText(8, 18, Source)
                End If
                UpdateCOMPASSAddress(altAddress) 'add alt address info
                PutText(10, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                PutText(11, 55, "Y") 'address is valid
                Hit("Enter")
                updateAddressIndicator = True 'so the alt address is over written by the legal address
            End If
            'invalidate first
            If doInvalidateFirstProcessing Then
                'address
                If checkboxIndicators("Address").ckbInvalidateFirst.Checked Then
                    If isSchool Then 'if the contact was a school
                        If Check4Text(8, 18, " ") = False Then
                            'only populate source if blank
                            PutText(8, 18, Source)
                            PutText(9, 18, "04")
                        End If
                        PutText(10, 32, SP.Q.GetText(10, 32, 2))
                        PutText(10, 35, SP.Q.GetText(10, 35, 2))
                        PutText(10, 38, SP.Q.GetText(10, 38, 2))
                        PutText(11, 55, SP.Q.GetText(11, 55, 1))
                    End If
                    If Check4Text(8, 13, "CODE") Then
                        PutText(8, 18, Source)
                    End If
                    PutText(10, 32, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                    PutText(11, 55, "N") 'address is invalid
                    Hit("Enter")
                End If
                Hit("F6")
                'home phone
                If checkboxIndicators("HomePhone").ckbInvalidateFirst.Checked Then
                    PutText(16, 14, "H")
                    Hit("Enter")
                    PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                    PutText(19, 14, PSource)
                    PutText(17, 54, "N") 'phone is invalid
                    Hit("Enter")
                End If
                'other phone
                If checkboxIndicators("OtherPhone").ckbInvalidateFirst.Checked Then
                    PutText(16, 14, "A")
                    Hit("Enter")
                    PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                    PutText(19, 14, PSource)
                    PutText(17, 54, "N") 'other phone is invalid
                    Hit("Enter")
                End If
                'other phone
                If checkboxIndicators("OtherPhone2").ckbInvalidateFirst.Checked Then
                    PutText(16, 14, "W")
                    Hit("Enter")
                    PutText(16, 45, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                    PutText(19, 14, PSource)
                    PutText(17, 54, "N") 'other phone is invalid
                    Hit("Enter")
                End If
                'email
                'TODO: When promoting be sure in set for live
                'Live
                ''If checkboxIndicators("Email").ckbInvalidateFirst.Checked Then
                ''    Hit("F2")
                ''    Hit("F10")
                ''    PutText(10, 20, Source)
                ''    PutText(12, 17, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                ''    PutText(13, 14, "N") 'email is invalid
                ''    Hit("Enter")
                ''End If
                'Test
                If checkboxIndicators("Email").ckbInvalidateFirst.Checked Then
                    Hit("F2")
                    Hit("F10")
                    PutText(9, 20, "41")
                    PutText(11, 17, CStr(Format(Today, "MM/dd/yy")).Replace("/", ""))
                    PutText(12, 14, "N") 'email is invalid
                    Hit("Enter")
                End If
            End If
        End If
    End Sub

    Private Shared Sub UpdateCOMPASSAddress(ByVal demosForUpdating As Demographics)
        SP.Q.PutText(11, 10, demosForUpdating.Addr1.Trim)
        If Len(demosForUpdating.Addr1) < 30 Then SP.Q.Hit("End") 'all these if statements clear out info not typed over
        SP.Q.PutText(12, 10, demosForUpdating.Addr2.Trim)
        If Len(demosForUpdating.Addr2) < 30 Then SP.Q.Hit("End")
        SP.Q.PutText(13, 10, "")
        SP.Q.Hit("End")
        SP.Q.PutText(14, 8, demosForUpdating.City.Trim)
        If Len(demosForUpdating.City) < 20 Then SP.Q.Hit("End")
        SP.Q.PutText(14, 32, demosForUpdating.State.Trim)
        SP.Q.PutText(14, 40, "")
        SP.Q.Hit("END")
        SP.Q.PutText(14, 40, demosForUpdating.Zip.Trim)
    End Sub


#End Region

#Region "OneLINK"

    Public Shared Sub UpdateOneLINKSystem(ByVal Source As String, ByVal systemsUpdateIndicators As UpdateDemoCompassIndicators, ByVal demosForUpdating As Demographics, ByVal checkboxIndicators As Dictionary(Of String, CheckboxIndicatorsForDemographicPart), ByVal altAddress As Demographics)
        Dim TS As New TimeSpan(0, 0, 1)

        'do Invalidate first and Alternate address checks and processing
        OneLINKInvalidateFirstAndAlternateAddrProc(Source, checkboxIndicators, altAddress, systemsUpdateIndicators.Address, demosForUpdating)

        SP.Q.FastPath("LP22C" & demosForUpdating.SSN)
        'switch to address info
        SP.Q.PutText(3, 9, Source)
        If AddressSaved Then
            If systemsUpdateIndicators.Address Or systemsUpdateIndicators.AddressIndicator Then 'update the address validity indicator if the address was changed or verified
                If systemsUpdateIndicators.Address Then 'update address
                    UpdateOneLINKAddress(demosForUpdating)
                End If
                If checkboxIndicators("Address").ckbNotValid.Checked Then
                    SP.Q.PutText(10, 57, "N") 'address is Invalid
                Else
                    SP.Q.PutText(10, 57, "Y") 'address is valid
                End If
            End If
        End If
        'update phone and phone indicator if needed
        If PhoneSaved Then
            If systemsUpdateIndicators.Phone Or systemsUpdateIndicators.PhoneIndicator Then 'update indicator
                If SP.Q.Check4Text(16, 16, "_________________") = False Then
                    SP.Q.PutText(16, 16, "")
                    SP.Q.Hit("End")
                End If
                If demosForUpdating.HomePhoneNum <> "8015551212" And Not String.IsNullOrWhiteSpace(demosForUpdating.HomePhoneNum) Then
                    If checkboxIndicators("HomePhone").ckbNotValid.Checked Then
                        'check if the alt phone is valid and put it in the home phone place if it is
                        If checkboxIndicators("OtherPhone").ckbNotValid.Checked = False Then
                            systemsUpdateIndicators.Phone = True 'do update with alt phone data
                            demosForUpdating.HomePhoneNum = demosForUpdating.OtherPhoneNum
                            demosForUpdating.HomePhoneExt = demosForUpdating.OtherPhoneExt
                            checkboxIndicators("OtherPhone").ckbNotValid.Checked = True 'make the alt phone invalid since the phone number has that valid data now
                            systemsUpdateIndicators.OtherPhoneIndicator = True
                        Else
                            SP.Q.PutText(13, 38, "N") 'phone is valid
                        End If
                    Else
                        SP.Q.PutText(13, 38, "Y") 'phone is valid
                    End If
                Else
                    SP.Q.PutText(13, 38, "N") 'phone not valid
                    SP.Q.PutText(13, 12, "8015551212")
                    SP.Q.Hit("END")
                End If
                If systemsUpdateIndicators.Phone Then 'update phone
                    SP.Q.PutText(13, 12, demosForUpdating.HomePhoneNum.Trim)
                    If demosForUpdating.HomePhoneExt <> "" Then
                        SP.Q.PutText(13, 27, demosForUpdating.HomePhoneExt.Trim)
                        If Len(demosForUpdating.HomePhoneExt) < 4 Then SP.Q.Hit("End")
                    Else
                        SP.Q.PutText(13, 27, "")
                        SP.Q.Hit("End")
                    End If
                    SP.Q.PutText(13, 68, CalculateConsentIndicator(demosForUpdating.HomePhoneMBL, checkboxIndicators("HomePhone").ckbConsent.Checked))
                End If

            End If
        End If
        'update other phone and  other phone indicator if needed
        If OtherSaved Then
            If systemsUpdateIndicators.OtherPhone Or systemsUpdateIndicators.OtherPhoneIndicator Then 'update indicator
                If demosForUpdating.OtherPhoneNum <> "8015551212" And Not String.IsNullOrWhiteSpace(demosForUpdating.OtherPhoneNum) Then
                    If checkboxIndicators("OtherPhone").ckbNotValid.Checked Then
                        SP.Q.PutText(14, 38, "N") 'Other phone not valid
                    Else
                        SP.Q.PutText(14, 38, "Y") 'Other phone is valid
                    End If
                    If systemsUpdateIndicators.OtherPhone Then 'update other phone
                        SP.Q.PutText(14, 12, demosForUpdating.OtherPhoneNum.Trim)
                        If demosForUpdating.OtherPhoneExt <> "" Then
                            SP.Q.PutText(14, 27, demosForUpdating.OtherPhoneExt.Trim)
                            If Len(demosForUpdating.OtherPhoneExt) < 4 Then SP.Q.Hit("End")
                        Else
                            SP.Q.PutText(14, 27, "")
                            SP.Q.Hit("End")
                        End If
                        SP.Q.PutText(14, 68, CalculateConsentIndicator(demosForUpdating.OtherPhoneMBL, checkboxIndicators("OtherPhone").ckbConsent.Checked))
                    End If
                Else
                    SP.Q.PutText(14, 38, "N") 'Other phone not valid
                End If
            End If
        End If
        'update other phone and  other phone indicator if needed
        'If Other2Saved Then
        '    If systemsUpdateIndicators.Other2Phone Or systemsUpdateIndicators.Other2PhoneIndicator Then 'update indicator
        '        If demosForUpdating.OtherPhone2Num <> "8015551212" And Not String.IsNullOrWhiteSpace(demosForUpdating.OtherPhone2Num) Then
        '            If checkboxIndicators("OtherPhone2").ckbNotValid.Checked Then
        '                SP.Q.PutText(15, 38, "N") 'Other phone not valid
        '            Else
        '                SP.Q.PutText(15, 38, "Y") 'Other phone is valid
        '            End If
        '            If systemsUpdateIndicators.Other2Phone Then 'update other phone
        '                SP.Q.PutText(15, 12, demosForUpdating.OtherPhone2Num.Trim)
        '                If demosForUpdating.OtherPhone2Ext <> "" Then
        '                    SP.Q.PutText(15, 27, demosForUpdating.OtherPhone2Ext.Trim)
        '                    If Len(demosForUpdating.OtherPhone2Ext) < 4 Then SP.Q.Hit("End")
        '                Else
        '                    SP.Q.PutText(15, 27, "")
        '                    SP.Q.Hit("End")
        '                End If
        '                SP.Q.PutText(15, 68, CalculateConsentIndicator(demosForUpdating.OtherPhone2MBL, checkboxIndicators("OtherPhone2").ckbConsent.Checked))
        '            End If
        '        Else
        '            SP.Q.PutText(15, 38, "N") 'Other phone not valid
        '        End If
        '    End If
        'End If
        'check to be sure the OneLINK Email address has something in it if the user blanked out the email
        If EmailSaved Then
            If demosForUpdating.Email <> "" Or SP.Q.Check4Text(19, 9, "_") = False Then
                If systemsUpdateIndicators.Email Or systemsUpdateIndicators.EmailIndicator Then
                    If checkboxIndicators("Email").ckbNotValid.Checked Then
                        SP.Q.PutText(18, 56, "N") 'email not valid
                    Else
                        If systemsUpdateIndicators.Email Then
                            'only update the email if the user entered a new email
                            If demosForUpdating.Email <> "" Then 'update email
                                SP.Q.PutText(19, 9, demosForUpdating.Email)
                                If Len(demosForUpdating.Email.Trim) < 56 Then SP.Q.Hit("END")
                            End If
                        End If
                        SP.Q.PutText(18, 56, "Y") 'Other phone is valid
                    End If
                End If
            End If
        End If
        SP.Q.Hit("Enter")
        If SP.Q.Check4Text(22, 3, "46012 SCREEN VALIDATED - PRESS F6 TO POST") = False Then
            Threading.Thread.CurrentThread.Sleep(100) 'pause for one second
            SP.Q.Hit("Enter")
            If SP.Q.Check4Text(22, 3, "46012 SCREEN VALIDATED - PRESS F6 TO POST") = False Then
                SP.UserInputWait()
                SP.Q.Hit("Enter")
            End If
        End If
        SP.Q.Hit("F6")
        If SP.Q.Check4Text(22, 3, "49000 DATA SUCCESSFULLY UPDATED") = False Then
            SP.UserInputWait()
            SP.Q.Hit("F6")
        End If
    End Sub

    'calculate the value to put in the consent field
    Private Shared Function CalculateConsentIndicator(ByVal mbl As String, ByVal consent As Boolean) As String
        If mbl = "L" Then
            Return "L"
        ElseIf mbl = "M" And Not consent Then
            Return "N"
        ElseIf mbl = "M" And consent Then
            Return "P"
        Else
            Return "U"
        End If
    End Function

    Private Shared Sub OneLINKInvalidateFirstAndAlternateAddrProc(ByVal Source As String, ByVal checkboxIndicators As Dictionary(Of String, CheckboxIndicatorsForDemographicPart), ByVal altAddress As Demographics, ByRef updateAddressIndicator As Boolean, ByVal demosForUpdating As Demographics)
        Dim doAltAddressProcessing As Boolean = False
        Dim doInvalidateFirstProcessing As Boolean = False
        AltAddressAndInvalidateFirstProcCheck(checkboxIndicators, altAddress, doAltAddressProcessing, doInvalidateFirstProcessing)
        If doAltAddressProcessing = True Or doInvalidateFirstProcessing = True Then
            'either alternate address and/or invalidate first functionality needs to be performed
            FastPath("LP22C" & demosForUpdating.SSN)
            'switch to address info
            PutText(3, 9, Source)
            'do alt address add
            If doAltAddressProcessing Then
                PutText(10, 57, "Y") 'address is valid
                UpdateOneLINKAddress(altAddress) 'add alt address
                updateAddressIndicator = True 'so the alt address is over written by the legal address
            End If
            'do invalidate first
            If doInvalidateFirstProcessing Then
                'address
                If checkboxIndicators("Address").ckbInvalidateFirst.Checked Then
                    PutText(10, 57, "N") 'address is invalid
                End If
                'home phone
                If checkboxIndicators("HomePhone").ckbInvalidateFirst.Checked Then
                    PutText(13, 38, "N") 'home phone is invalid
                End If
                'other phone
                If checkboxIndicators("OtherPhone").ckbInvalidateFirst.Checked Then
                    PutText(14, 38, "N") 'other phone is invalid
                End If
                'email
                If checkboxIndicators("Email").ckbInvalidateFirst.Checked Then
                    PutText(18, 56, "N") 'email is invalid
                End If
            End If
            Hit("F6") 'post changes to system
        End If
    End Sub

    Private Shared Sub UpdateOneLINKAddress(ByVal demosForUpdating As Demographics)
        SP.Q.PutText(10, 9, demosForUpdating.Addr1.Trim)
        If Len(demosForUpdating.Addr1) < 35 Then SP.Q.Hit("End") 'all these if statements clear out info not typed over
        SP.Q.PutText(11, 9, demosForUpdating.Addr2.Trim)
        If Len(demosForUpdating.Addr2) < 35 Then SP.Q.Hit("End")
        SP.Q.PutText(12, 9, demosForUpdating.City.Trim)
        If Len(demosForUpdating.City) < 30 Then SP.Q.Hit("End")
        SP.Q.PutText(12, 52, demosForUpdating.State.Trim)
        SP.Q.PutText(12, 60, demosForUpdating.Zip.Trim)
        If Len(demosForUpdating.Zip) < 9 Then SP.Q.Hit("END")
    End Sub

#End Region

#Region "LCO"

    Public Shared Function UpdateLCOSystem(ByVal demosForUpdating As Demographics) As Boolean
        SP.Processing.Visible = True
        SP.LoginToLCO()
        SP.FastPath("TPDD" & demosForUpdating.SSN)
        If SP.Q.Check4Text(1, 19, "LCO PERSONAL INFORMATION DISPLAY") = True Then
            'Set LCO demographic info
            If demosForUpdating.Addr1 = "" Then
                SP.Q.PutText(6, 19, " ")
                SP.Q.Hit("END")
            Else
                SP.Q.PutText(6, 19, "")
                SP.Q.Hit("END")
                SP.Q.PutText(6, 19, demosForUpdating.Addr1)
            End If
            If demosForUpdating.Addr2 = "" Then
                SP.Q.PutText(7, 19, " ")
                SP.Q.Hit("END")
            Else
                SP.Q.PutText(7, 19, "")
                SP.Q.Hit("END")
                SP.Q.PutText(7, 19, demosForUpdating.Addr2)
            End If
            If demosForUpdating.City = "" Then
                SP.Q.PutText(9, 19, " ")
                SP.Q.Hit("END")
            Else
                SP.Q.PutText(9, 19, "")
                SP.Q.Hit("END")
                SP.Q.PutText(9, 19, demosForUpdating.City)
            End If
            SP.Q.PutText(9, 50, demosForUpdating.State)
            SP.Q.PutText(9, 66, demosForUpdating.Zip)
            SP.Q.Hit("END")
            SP.Q.PutText(13, 13, demosForUpdating.HomePhoneNum)
            If demosForUpdating.OtherPhoneNum = "" Then
                SP.Q.PutText(14, 13, " ")
                SP.Q.Hit("END")
                SP.Q.Hit("TAB")
                SP.Q.Hit("END")
                SP.Q.Hit("TAB")
                SP.Q.Hit("END")
            Else
                SP.Q.PutText(14, 13, demosForUpdating.OtherPhoneNum)
            End If
            If demosForUpdating.Email = "" Then
                SP.Q.PutText(19, 19, " ")
                SP.Q.Hit("END")
            Else
                SP.Q.PutText(19, 19, "")
                SP.Q.Hit("END")
                SP.Q.PutText(19, 19, demosForUpdating.Email)
            End If
            SP.Q.Hit("ENTER")
            If SP.Q.Check4Text(23, 27, "IMPROPER CHARACTERS IN FIELD") Then SP.Q.Hit("ENTER")
            SP.LoginToCompass()
            Return True
        End If
        SP.LoginToCompass()
        SP.Processing.Visible = False
        Return False
    End Function

#End Region

End Class
