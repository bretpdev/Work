Imports Q
Imports System.Collections.Generic

Public Class DemographicsForUpdate


    Public Enum DemographicPartType
        All
        Address
        HomePhone
        OtherPhone
        OtherPhone2
        Email
    End Enum


    Private _demos As Demographics
    Private _indicators As Dictionary(Of String, CheckboxIndicatorsForDemographicPart)



    Public Sub SyncWithMainFormControls(ByRef tDemos As Demographics, ByRef tIndicators As Dictionary(Of String, CheckboxIndicatorsForDemographicPart))
        _demos = tDemos
        _indicators = tIndicators
    End Sub

    ''' <summary>
    ''' Populates the demographic object that is going to be used to update the system with data from the form controls and the borrower object.
    ''' </summary>
    ''' <param name="bor">The Borrower object.</param>
    ''' <remarks></remarks>
    Public Overridable Sub PopulateDemographicObject(ByVal bor As Borrower)
        ' ckbEmailVal.Checked, IsSchool)
        _demos.CLAccNum = bor.CLAccNum
        _demos.FirstName = bor.FirstName
        _demos.MI = bor.MI
        _demos.LastName = bor.LastName
        _demos.Addr1 = Addr.txtAddr1.Text
        _demos.Addr2 = Addr.txtAddr2.Text
        _demos.UPAddrVal = Not _indicators("Address").ckbNotValid.Checked
        _demos.City = Addr.txtCity.Text
        _demos.State = Addr.cbState.Text
        _demos.Zip = Addr.txtZip.Text
        _demos.HomePhoneNum = HomePhn.txtPhone1.Text & HomePhn.txtPhone2.Text & HomePhn.txtPhone3.Text
        _demos.HomePhoneExt = HomePhn.txtExt.Text
        _demos.UPPhoneVal = Not _indicators("HomePhone").ckbNotValid.Checked
        _demos.HomePhoneMBL = HomePhn.cbxPhnMBL.Text
        _demos.OtherPhoneNum = OtherPhn.txtPhone1.Text & OtherPhn.txtPhone2.Text & OtherPhn.txtPhone3.Text
        _demos.OtherPhoneExt = OtherPhn.txtExt.Text
        _demos.UPOtherVal = Not _indicators("OtherPhone").ckbNotValid.Checked
        _demos.OtherPhoneMBL = OtherPhn.cbxPhnMBL.Text
        _demos.OtherPhone2Num = Other2Phn.txtPhone1.Text & Other2Phn.txtPhone2.Text & Other2Phn.txtPhone3.Text
        _demos.OtherPhone2Ext = Other2Phn.txtExt.Text
        _demos.UPOther2Val = Not _indicators("OtherPhone2").ckbNotValid.Checked
        _demos.OtherPhone2MBL = Other2Phn.cbxPhnMBL.Text
        _demos.Email = txtEmail.Text
        _demos.UPEmailVal = Not _indicators("Email").ckbNotValid.Checked
        bor.ContactCode = CType(Me.ParentForm, frmDemographics).txtContactCode.Text
        bor.ActivityCode = CType(Me.ParentForm, frmDemographics).txtActivityCode.Text
        bor.UserProvidedDemos.UPAddrVer = _indicators("Address").ckbVerified.Checked
        bor.UserProvidedDemos.UPPhoneNumVer = _indicators("HomePhone").ckbVerified.Checked
        bor.UserProvidedDemos.UPOtherVer = _indicators("OtherPhone").ckbVerified.Checked
        bor.UserProvidedDemos.UPEmailVer = _indicators("Email").ckbVerified.Checked
        bor.UserProvidedDemos.UPPhoneConsent = _indicators("HomePhone").ckbConsent.Checked
        bor.UserProvidedDemos.UPOtherConsent = _indicators("OtherPhone").ckbConsent.Checked
        bor.UserProvidedDemos.UPOther2Consent = _indicators("OtherPhone2").ckbConsent.Checked
    End Sub

    Public Sub PrePopulateCheck(ByRef compassAndOneLINKDiffer As Boolean, ByVal bor As Borrower)
        compassAndOneLINKDiffer = False
        'check if demographic info matches
        If bor.BorrowerIsLCOOnly = False Then
            If bor.OneLINKDemos.Addr1 = bor.CompassDemos.Addr1 And bor.OneLINKDemos.Addr2 = bor.CompassDemos.Addr2 And bor.OneLINKDemos.City = bor.CompassDemos.City And _
               bor.OneLINKDemos.State = bor.CompassDemos.State And bor.OneLINKDemos.Zip = bor.CompassDemos.Zip And bor.OneLINKDemos.Phone = bor.CompassDemos.Phone And _
               (bor.OneLINKDemos.OtherPhoneNum = bor.CompassDemos.OtherPhoneNum Or bor.OneLINKDemos.OtherPhoneNum = bor.CompassDemos.OtherPhone2Num Or _
                ((bor.OneLINKDemos.OtherPhoneNum = "801-555-1212" And bor.CompassDemos.OtherPhone2Num = "") Or (bor.OneLINKDemos.OtherPhoneNum = "801-555-1212" And bor.CompassDemos.OtherPhoneNum = ""))) And bor.OneLINKDemos.Email = bor.CompassDemos.Email And _
               bor.OneLINKDemos.SPAddrInd = bor.CompassDemos.SPAddrInd And bor.OneLINKDemos.HomePhoneValidityIndicator = bor.CompassDemos.HomePhoneValidityIndicator And _
               bor.OneLINKDemos.OtherPhoneValidityIndicator = bor.CompassDemos.OtherPhoneValidityIndicator And (bor.OneLINKDemos.SPEmailInd = bor.CompassDemos.SPEmailInd Or (bor.OneLINKDemos.SPEmailInd <> bor.CompassDemos.SPEmailInd And bor.OneLINKDemos.Email = "" And bor.CompassDemos.Email = "")) Then
                'if demographic info matches then copy the info into the text boxes
                CopyObjectDataToTextBoxes(bor.CompassDemos)
                'check if COMPASS info is provided, if not then copy OneLINK info into the textboxes
            ElseIf "" = bor.CompassDemos.Addr1 And "" = bor.CompassDemos.Addr2 And "" = bor.CompassDemos.City And _
               "" = bor.CompassDemos.State And "" = bor.CompassDemos.Zip And "" = bor.CompassDemos.Phone And _
               "" = bor.CompassDemos.OtherPhoneNum And "" = bor.CompassDemos.OtherPhone2Num And "" = bor.CompassDemos.Email Then
                CopyObjectDataToTextBoxes(bor.OneLINKDemos)
            ElseIf "" = bor.OneLINKDemos.Addr1 And "" = bor.OneLINKDemos.Addr2 And "" = bor.OneLINKDemos.City And _
               "" = bor.OneLINKDemos.State And "" = bor.OneLINKDemos.Zip And "" = bor.OneLINKDemos.Phone And _
               "" = bor.OneLINKDemos.OtherPhoneNum And "" = bor.OneLINKDemos.Email Then
                CopyObjectDataToTextBoxes(bor.CompassDemos)
            Else
                compassAndOneLINKDiffer = True
            End If
        Else
            'copy from UserProvidedDemos object because LCO demos may be in it
            CopyObjectDataToTextBoxes(bor.UserProvidedDemos)
        End If
    End Sub

    'This function copies the info in the passed in demographic object to the editable text boxes
    Public Sub CopyObjectDataToTextBoxes(ByVal objectToCopyFrom As Demographics)
        'remove handler for blanking out state code
        RemoveStateComboBoxEventHandler()
        Addr.cbState.SelectedIndex = 0 'set selected index to 0
        'figure which state to select in the state combo box
        While Addr.cbState.Text <> objectToCopyFrom.State And Addr.cbState.SelectedIndex < Addr.cbState.Items.Count
            Addr.cbState.SelectedIndex = Addr.cbState.SelectedIndex + 1
        End While
        'if the state code isn't found then it must be a foreign state and thus MauiDUDE can't be used
        If Addr.cbState.SelectedIndex = Addr.cbState.Items.Count Then
            SP.frmWipeOut.WipeOut("The address must be a foreign address.  Maui DUDE doesn't jive with foreign addresses.", "Foreign Address Error", True)
            Addr.cbState.SelectedIndex = 0
            Exit Sub
        End If
        'copy address
        If TypeOf objectToCopyFrom Is LCODemographics Then
            Addr.txtAddr1.Text = objectToCopyFrom.Addr1
            Addr.txtAddr2.Text = objectToCopyFrom.Addr2
            Addr.txtCity.Text = objectToCopyFrom.City
            Addr.txtZip.Text = objectToCopyFrom.Zip
            HomePhn.txtPhone1.Text = Mid(objectToCopyFrom.HomePhoneNum, 1, 3)
            HomePhn.txtPhone2.Text = Mid(objectToCopyFrom.HomePhoneNum, 4, 3)
            HomePhn.txtPhone3.Text = Mid(objectToCopyFrom.HomePhoneNum, 7, 4)
            HomePhn.txtExt.Clear()
            HomePhn.cbxPhnMBL.Text = objectToCopyFrom.HomePhoneMBL
            If (HomePhn.cbxPhnMBL.Text.Length = 0) Then HomePhn.cbxPhnMBL.Text = "U"
            If objectToCopyFrom.HomePhoneConsent = "Y" Then
                _indicators("HomePhone").Indicators.Consent = True
                _indicators("HomePhone").ckbConsent.Checked = True
            Else
                _indicators("HomePhone").Indicators.Consent = False
                _indicators("HomePhone").ckbConsent.Checked = False
            End If
            OtherPhn.txtPhone1.Text = Mid(objectToCopyFrom.OtherPhoneNum, 1, 3)
            OtherPhn.txtPhone2.Text = Mid(objectToCopyFrom.OtherPhoneNum, 4, 3)
            OtherPhn.txtPhone3.Text = Mid(objectToCopyFrom.OtherPhoneNum, 7, 4)
            OtherPhn.txtExt.Clear()
            OtherPhn.cbxPhnMBL.Text = objectToCopyFrom.HomePhoneMBL
            If (OtherPhn.cbxPhnMBL.Text.Length = 0) Then OtherPhn.cbxPhnMBL.Text = "U"
            If objectToCopyFrom.OtherPhoneConsent = "Y" Then
                _indicators("OtherPhone").Indicators.Consent = True
                _indicators("OtherPhone").ckbConsent.Checked = True
            Else
                _indicators("OtherPhone").Indicators.Consent = False
                _indicators("OtherPhone").ckbConsent.Checked = False
            End If
            Other2Phn.txtPhone1.Clear()
            Other2Phn.txtPhone2.Clear()
            Other2Phn.txtPhone3.Clear()
            Other2Phn.txtExt.Clear()
            Other2Phn.cbxPhnMBL.Text = objectToCopyFrom.HomePhoneMBL
            If (Other2Phn.cbxPhnMBL.Text.Length = 0) Then Other2Phn.cbxPhnMBL.Text = "U"
            If objectToCopyFrom.OtherPhone2Consent = "Y" Then
                _indicators("OtherPhone2").Indicators.Consent = True
                _indicators("OtherPhone2").ckbConsent.Checked = True
            Else
                _indicators("OtherPhone2").Indicators.Consent = False
                _indicators("OtherPhone2").ckbConsent.Checked = False
            End If
            txtEmail.Text = objectToCopyFrom.Email
        Else 'Compass and OneLINK addresses

            'address
            Addr.txtAddr1.Text = Mid(objectToCopyFrom.Addr1, 1, 30)
            Addr.txtAddr2.Text = Mid(objectToCopyFrom.Addr2, 1, 30)
            Addr.txtCity.Text = Mid(objectToCopyFrom.City, 1, 20)
            Addr.txtZip.Text = objectToCopyFrom.Zip
            If objectToCopyFrom.SPAddrInd = "N" Then
                _indicators("Address").Indicators.NotValid = True
                _indicators("Address").ckbNotValid.Checked = True
            Else
                _indicators("Address").Indicators.NotValid = False
                _indicators("Address").ckbNotValid.Checked = False
            End If

            'copy phone
            HomePhn.txtPhone1.Text = Mid(objectToCopyFrom.Phone, 1, 3)
            HomePhn.txtPhone2.Text = Mid(objectToCopyFrom.Phone, 4, 3)
            HomePhn.txtPhone3.Text = Mid(objectToCopyFrom.Phone, 7, 4)
            HomePhn.txtExt.Text = objectToCopyFrom.HomePhoneExt
            HomePhn.cbxPhnMBL.Text = objectToCopyFrom.HomePhoneMBL
            If (HomePhn.cbxPhnMBL.Text.Length = 0) Then HomePhn.cbxPhnMBL.Text = "U"
            If objectToCopyFrom.HomePhoneValidityIndicator = "N" Then
                _indicators("HomePhone").Indicators.NotValid = True
                _indicators("HomePhone").ckbNotValid.Checked = True
            Else
                _indicators("HomePhone").Indicators.NotValid = False
                _indicators("HomePhone").ckbNotValid.Checked = False
            End If
            If objectToCopyFrom.HomePhoneConsent = "Y" Then
                _indicators("HomePhone").Indicators.Consent = True
                _indicators("HomePhone").ckbConsent.Checked = True
            Else
                _indicators("HomePhone").Indicators.Consent = False
                _indicators("HomePhone").ckbConsent.Checked = False
            End If

            'copy other phone
            OtherPhn.txtPhone1.Text = Mid(objectToCopyFrom.OtherPhoneNum, 1, 3)
            OtherPhn.txtPhone2.Text = Mid(objectToCopyFrom.OtherPhoneNum, 4, 3)
            OtherPhn.txtPhone3.Text = Mid(objectToCopyFrom.OtherPhoneNum, 7, 4)
            OtherPhn.txtExt.Text = objectToCopyFrom.OtherPhoneExt
            OtherPhn.cbxPhnMBL.Text = objectToCopyFrom.OtherPhoneMBL
            If (OtherPhn.cbxPhnMBL.Text.Length = 0) Then OtherPhn.cbxPhnMBL.Text = "U"
            If objectToCopyFrom.OtherPhoneValidityIndicator = "N" Then
                _indicators("OtherPhone").Indicators.NotValid = True
                _indicators("OtherPhone").ckbNotValid.Checked = True
            Else
                _indicators("OtherPhone").Indicators.NotValid = False
                _indicators("OtherPhone").ckbNotValid.Checked = False
            End If
            If objectToCopyFrom.OtherPhoneConsent = "Y" Then
                _indicators("OtherPhone").Indicators.Consent = True
                _indicators("OtherPhone").ckbConsent.Checked = True
            Else
                _indicators("OtherPhone").Indicators.Consent = False
                _indicators("OtherPhone").ckbConsent.Checked = False
            End If

            'copy other phone 
            If TypeOf objectToCopyFrom Is OneLINKDemographics Then
                'onelink
                Other2Phn.txtPhone1.Clear()
                Other2Phn.txtPhone2.Clear()
                Other2Phn.txtPhone3.Clear()
                Other2Phn.txtExt.Clear()
                Other2Phn.cbxPhnMBL.Text = "U"
                _indicators("OtherPhone2").Indicators.NotValid = False
                _indicators("OtherPhone2").ckbNotValid.Checked = False
                _indicators("OtherPhone2").ckbConsent.Checked = False
            Else
                'compass
                Other2Phn.txtPhone1.Text = Mid(objectToCopyFrom.OtherPhone2Num, 1, 3)
                Other2Phn.txtPhone2.Text = Mid(objectToCopyFrom.OtherPhone2Num, 4, 3)
                Other2Phn.txtPhone3.Text = Mid(objectToCopyFrom.OtherPhone2Num, 7, 4)
                Other2Phn.txtExt.Text = objectToCopyFrom.OtherPhone2Ext
                Other2Phn.cbxPhnMBL.Text = objectToCopyFrom.OtherPhone2MBL
                If (Other2Phn.cbxPhnMBL.Text.Length = 0) Then Other2Phn.cbxPhnMBL.Text = "U"
                If objectToCopyFrom.OtherPhone2ValidityIndicator = "N" Then
                    _indicators("OtherPhone2").Indicators.NotValid = True
                    _indicators("OtherPhone2").ckbNotValid.Checked = True
                Else
                    _indicators("OtherPhone2").Indicators.NotValid = False
                    _indicators("OtherPhone2").ckbNotValid.Checked = False
                End If
                If objectToCopyFrom.OtherPhone2Consent = "Y" Then
                    _indicators("OtherPhone2").Indicators.Consent = True
                    _indicators("OtherPhone2").ckbConsent.Checked = True
                Else
                    _indicators("OtherPhone2").Indicators.Consent = False
                    _indicators("OtherPhone2").ckbConsent.Checked = False
                End If
            End If

            'copy email
            txtEmail.Text = objectToCopyFrom.Email
            If objectToCopyFrom.SPEmailInd = "N" Then
                _indicators("Email").Indicators.NotValid = True
                _indicators("Email").ckbNotValid.Checked = True
            Else
                _indicators("Email").Indicators.NotValid = False
                _indicators("Email").ckbNotValid.Checked = False
            End If
        End If
        'add the handler again
        AddStateComboBoxEventHandler()
        Addr.txtAddr1.Focus()
    End Sub

    Public Sub ResetControls()
        'blank all boxes
        Addr.txtAddr1.Clear()
        Addr.txtAddr2.Clear()
        Addr.txtCity.Clear()
        Addr.txtZip.Clear()
        HomePhn.txtPhone1.Clear()
        HomePhn.txtPhone2.Clear()
        HomePhn.txtPhone3.Clear()
        HomePhn.txtExt.Clear()
        HomePhn.cbxPhnMBL.Text = ""
        OtherPhn.txtPhone1.Clear()
        OtherPhn.txtPhone2.Clear()
        OtherPhn.txtPhone3.Clear()
        OtherPhn.txtExt.Clear()
        OtherPhn.cbxPhnMBL.Text = ""
        Other2Phn.txtPhone1.Clear()
        Other2Phn.txtPhone2.Clear()
        Other2Phn.txtPhone3.Clear()
        Other2Phn.txtExt.Clear()
        Other2Phn.cbxPhnMBL.Text = ""
        txtEmail.Clear()
        'remove state combo box handler
        RemoveStateComboBoxEventHandler()
        Addr.cbState.SelectedIndex = 0
        'add handler again
        AddStateComboBoxEventHandler()
    End Sub

    'this function validates all the user entered data after the Save and Continue button is sp.q.hited
    Public Function ValidUserInput(ByRef verificationRequired As Boolean, ByRef addressWarning As Boolean, ByRef compassAndOneLINKDiffer As Boolean, ByVal bor As Borrower) As Boolean
        Dim counter As Integer
        Dim Found As Boolean
        Dim verifyModResult As Boolean = VerifyModification(bor, DemographicPartType.All)
        If MadeDemoChange(compassAndOneLINKDiffer, bor, verifyModResult) = False Then
            If ((CType(Me.ParentForm, frmDemographics).txtActivityCode.Text = "TC" Or CType(Me.ParentForm, frmDemographics).txtActivityCode.Text = "EM") And CType(Me.ParentForm, frmDemographics).txtContactCode.Text = "04") = False And verificationRequired = False Then
                Return True
            End If
        End If
        If Addr.ValidUserInput(bor) = False Then
            'enable address boxes and buttons
            CType(Me.ParentForm, frmDemographics).OneLINKDemographicsForDisplay.btnUseThisAddr.Enabled = True
            CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.btnUseThisAddr.Enabled = True
            _indicators("Address").ckbVerified.Checked = False
            Return False
        End If
        'check if no phone number check box is checked 
        If CType(Me.ParentForm, frmDemographics).ckbNoPhone.Checked = False Then
            'if any of the phone fields have data then validate the data
            If (HomePhn.txtPhone1.TextLength <> 0 Or HomePhn.txtPhone2.TextLength <> 0 Or HomePhn.txtPhone3.TextLength <> 0) Or _
            (OtherPhn.txtPhone1.TextLength <> 0 Or OtherPhn.txtPhone2.TextLength <> 0 Or OtherPhn.txtPhone3.TextLength <> 0) Or _
            (Other2Phn.txtPhone1.TextLength <> 0 Or Other2Phn.txtPhone2.TextLength <> 0 Or Other2Phn.txtPhone3.TextLength <> 0) Then
                If HomePhn.ValidUserInput(bor) = False Then
                    CType(Me.ParentForm, frmDemographics).OneLINKDemographicsForDisplay.btnUseThisAddr.Enabled = True
                    CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.btnUseThisAddr.Enabled = True
                    _indicators("HomePhone").ckbVerified.Checked = False
                    Return False
                End If
            End If
            If OtherPhn.ValidUserInput(bor) = False Then
                CType(Me.ParentForm, frmDemographics).OneLINKDemographicsForDisplay.btnUseThisAddr.Enabled = True
                CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.btnUseThisAddr.Enabled = True
                _indicators("OtherPhone").ckbVerified.Checked = False
                Return False
            End If
            If Other2Phn.ValidUserInput(bor) = False Then
                CType(Me.ParentForm, frmDemographics).OneLINKDemographicsForDisplay.btnUseThisAddr.Enabled = True
                CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.btnUseThisAddr.Enabled = True
                _indicators("OtherPhone2").ckbVerified.Checked = False
                Return False
            End If
        End If
        'check if no email is checked
        If _indicators("Email").ckbNotValid.Checked = False Then
            'check if there is anything in the email text box
            If txtEmail.TextLength <> 0 Then
                'Be sure there is one "@" and at least one non-consecutive "." in the string, and no invalid characters.
                If txtEmail.Text.IndexOf("@") = -1 Or (txtEmail.Text.IndexOf("@") <> txtEmail.Text.LastIndexOf("@")) _
                Or txtEmail.Text.IndexOf(".") = -1 Or InStr(txtEmail.Text, "..") <> 0 _
                Or txtEmail.Text.IndexOf(" ") <> -1 Or txtEmail.Text.IndexOf(",") <> -1 _
                Or txtEmail.Text.IndexOf(";") <> -1 Or txtEmail.Text.IndexOf(":") <> -1 _
                Or txtEmail.Text.IndexOf("<") <> -1 Or txtEmail.Text.IndexOf(">") <> -1 _
                Or txtEmail.Text.IndexOf("(") <> -1 Or txtEmail.Text.IndexOf(")") <> -1 _
                Or txtEmail.Text.IndexOf("[") <> -1 Or txtEmail.Text.IndexOf("]") <> -1 _
                Or txtEmail.Text.IndexOf("{") <> -1 Or txtEmail.Text.IndexOf("}") <> -1 _
                Or txtEmail.Text.IndexOf("'") <> -1 Or txtEmail.Text.IndexOf("""") <> -1 _
                Or txtEmail.Text.IndexOf("\t") <> -1 Or txtEmail.Text.IndexOf("\r") <> -1 _
                Or txtEmail.Text.IndexOf("\f") <> -1 Or txtEmail.Text.IndexOf("\n") <> -1 _
                Then
                    'give the user an error message
                    SP.frmWhoaDUDE.WhoaDUDE("The email address must have one '@' a '.' and no spaces, quotation marks, or '<>()[],;:' in it, much like a surfer needs a wave to ride.", "Invalid Email Address", True)
                    'enable phone number boxes and buttons
                    txtEmail.Enabled = True
                    CType(Me.ParentForm, frmDemographics).OneLINKDemographicsForDisplay.btnUseThisAddr.Enabled = True
                    CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.btnUseThisAddr.Enabled = True
                    txtEmail.Focus()
                    _indicators("Email").ckbVerified.Checked = False
                    Return False
                End If
            End If
        End If
        'be sure the user has entered both a contact and activity code
        If CType(Me.ParentForm, frmDemographics).txtActivityCode.TextLength <> 2 Or CType(Me.ParentForm, frmDemographics).txtContactCode.TextLength <> 2 Then
            SP.frmWhoaDUDE.WhoaDUDE("You must provide a valid activity and contact code.", "Activity and Contact Code Needed", True)
            CType(Me.ParentForm, frmDemographics).txtActivityCode.Focus()
            Return False
        End If
        'be sure that one of the demographic parts (addr, phone, email) is verified as correct or modified
        If _indicators("OtherPhone2").ckbVerified.Checked = False And _indicators("OtherPhone").ckbVerified.Checked = False And _indicators("Address").ckbVerified.Checked = False And _indicators("HomePhone").ckbVerified.Checked = False And _indicators("Email").ckbVerified.Checked = False And verifyModResult = False And CType(Me.ParentForm, frmDemographics).cboAttempt.Text = "" Then
            SP.frmWhoaDUDE.WhoaDUDE("The borrower demographic information must be updated or verified in order to continue.", "Update or Verification Needed", True)
            Return False
        End If
        'check for valid entry in activity code
        While counter < frmDemographics.ActivityCode.Count
            If UCase(CType(Me.ParentForm, frmDemographics).txtActivityCode.Text) = frmDemographics.ActivityCode(counter) Then
                Found = True
            End If
            counter = counter + 1
        End While
        'if a match was found then move to the next box else stay in textbox and highlight text
        If Found = False Then
            SP.frmWhoaDUDE.WhoaDUDE("DUDE couldn't do the hula with that Activity Code entry.", "Bad Activity Code", True)
            CType(Me.ParentForm, frmDemographics).txtActivityCode.Focus()
            CType(Me.ParentForm, frmDemographics).txtActivityCode.SelectAll()
            Return False
        End If
        counter = 0
        Found = False
        'check for valid contact code entry
        While counter < frmDemographics.ContactCode.Count And Found = False
            If UCase(CType(Me.ParentForm, frmDemographics).txtContactCode.Text) = frmDemographics.ContactCode(counter) Then
                Found = True
            End If
            counter = counter + 1
        End While
        'if a match was found then move to the next box else stay in textbox and highlight text
        If Found = False Then
            SP.frmWhoaDUDE.WhoaDUDE("DUDE couldn't do the hula with that Contact Code entry.", "Bad Contact Code", True)
            CType(Me.ParentForm, frmDemographics).txtContactCode.Focus()
            CType(Me.ParentForm, frmDemographics).txtContactCode.SelectAll()
            Return False
        End If

        If _indicators("Address").ckbNotValid.Checked Then
            If addressWarning = False Then
                If ((bor.OneLINKDemos.Addr1 = Addr.txtAddr1.Text And bor.OneLINKDemos.Addr2 = Addr.txtAddr2.Text And bor.OneLINKDemos.City = Addr.txtCity.Text And _
                        bor.OneLINKDemos.State = Addr.cbState.Text And bor.OneLINKDemos.Zip = Addr.txtZip.Text) = False And bor.OneLINKDemos.SPAddrInd = "Y") Or _
                        ((Addr.txtAddr1.Text = bor.CompassDemos.Addr1 And Addr.txtAddr2.Text = bor.CompassDemos.Addr2 And Addr.txtCity.Text = bor.CompassDemos.City And _
                        Addr.cbState.Text = bor.CompassDemos.State And Addr.txtZip.Text = bor.CompassDemos.Zip) = False And bor.CompassDemos.SPAddrInd = "Y") Then
                    'compass or onelink addresses do not match the given address and one of them is valid
                    addressWarning = True
                    If SP.frmYesNo.YesNo("Knarly! Looks like there might be another valid address. Duwana check it out?") Then
                        Return False
                    End If
                End If
            End If
        End If

        If ((HomePhn.txtPhone1.Text = OtherPhn.txtPhone1.Text And HomePhn.txtPhone2.Text = OtherPhn.txtPhone2.Text And HomePhn.txtPhone3.Text = OtherPhn.txtPhone3.Text) And _indicators("HomePhone").ckbNotValid.Checked = False And _indicators("OtherPhone").ckbNotValid.Checked = False) Then
            _indicators("OtherPhone").ckbNotValid.Checked = True
        ElseIf ((HomePhn.txtPhone1.Text = Other2Phn.txtPhone1.Text And HomePhn.txtPhone2.Text = Other2Phn.txtPhone2.Text And HomePhn.txtPhone3.Text = Other2Phn.txtPhone3.Text) And _indicators("HomePhone").ckbNotValid.Checked = False And _indicators("OtherPhone2").ckbNotValid.Checked = False) Then
            _indicators("OtherPhone2").ckbNotValid.Checked = True
        ElseIf ((OtherPhn.txtPhone1.Text = Other2Phn.txtPhone1.Text And OtherPhn.txtPhone2.Text = Other2Phn.txtPhone2.Text And OtherPhn.txtPhone3.Text = Other2Phn.txtPhone3.Text) And _indicators("OtherPhone").ckbNotValid.Checked = False And _indicators("OtherPhone2").ckbNotValid.Checked = False) Then
            _indicators("OtherPhone2").ckbNotValid.Checked = True
        End If

        If CType(Me.ParentForm, frmDemographics).cboAttempt.Text <> "" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "TE" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "TT" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "T1" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "T2" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "T3" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "T4" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "T5" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "T6" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "T7" And CType(Me.ParentForm, frmDemographics).txtActivityCode.Text <> "T8" Then
            SP.frmWhoaDUDE.WhoaDUDE("That Attempt Type just stuffed DUDE in a barrel.", "Bad Attempt type", True)
            Return False
        End If
        bor.DemographicsVerified = True
        Return True
    End Function

    Private Function MadeDemoChange(ByVal compassAndOneLINKDiffer As Boolean, ByVal bor As Borrower, ByRef verifyModResult As Boolean) As Boolean
        If _indicators("OtherPhone2").ckbVerified.Checked = False And _indicators("OtherPhone").ckbVerified.Checked = False And _indicators("Address").ckbVerified.Checked = False And _indicators("HomePhone").ckbVerified.Checked = False And _indicators("Email").ckbVerified.Checked = False And (verifyModResult = False Or (verifyModResult = True And compassAndOneLINKDiffer = True And Addr.txtAddr1.Text = "")) And CType(Me.ParentForm, frmDemographics).cboAttempt.Text = "" Then
            Return False
        Else
            Return True
        End If
    End Function

    'this function verifies that a modification took place in the text boxes
    Public Function VerifyModification(ByVal bor As Borrower, ByVal whatToVerify As DemographicPartType) As Boolean
        Dim COMPASSInfoFound As Boolean
        'decide if COMPASS information was gathered
        If CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.lblSystemError.Visible = False Then
            COMPASSInfoFound = True
        End If
        If whatToVerify = DemographicPartType.All Then
            'check if any data elements <> possible starting values
            If (Addr.txtAddr1.Text <> bor.OneLINKDemos.Addr1) Or _
               (Addr.txtAddr2.Text <> bor.OneLINKDemos.Addr2) Or _
               (Addr.txtCity.Text <> bor.OneLINKDemos.City) Or _
               (Addr.cbState.Text <> bor.OneLINKDemos.State) Or _
               (Addr.txtZip.Text <> bor.OneLINKDemos.Zip) Or _
               (Replace(HomePhn.txtPhone1.Text & "-" & HomePhn.txtPhone2.Text & "-" & HomePhn.txtPhone3.Text, "-", "") <> Replace(bor.OneLINKDemos.Phone, "-", "")) Or _
               (HomePhn.txtExt.Text <> bor.OneLINKDemos.HomePhoneExt) Or _
               (Replace(OtherPhn.txtPhone1.Text & "-" & OtherPhn.txtPhone2.Text & "-" & OtherPhn.txtPhone3.Text, "-", "") <> Replace(bor.OneLINKDemos.OtherPhoneNum, "-", "")) Or _
               (OtherPhn.txtExt.Text <> bor.OneLINKDemos.OtherPhoneExt) Or _
               (_indicators("Address").ckbNotValid.Checked And bor.OneLINKDemos.SPAddrInd = "Y") Or _
               (_indicators("HomePhone").ckbNotValid.Checked And bor.OneLINKDemos.HomePhoneValidityIndicator = "Y") Or _
               (_indicators("OtherPhone").ckbNotValid.Checked And bor.OneLINKDemos.OtherPhoneValidityIndicator = "Y") Or _
               (_indicators("Email").ckbNotValid.Checked And bor.OneLINKDemos.SPEmailInd = "Y") Or _
               (txtEmail.Text <> bor.OneLINKDemos.Email) Then
                Return True
            End If
            'if compass info was found compare text in boxes to both systems  
            If COMPASSInfoFound Then
                'check if any data elements <> possible starting values
                If (Addr.txtAddr1.Text <> bor.CompassDemos.Addr1) Or _
                   (Addr.txtAddr2.Text <> bor.CompassDemos.Addr2) Or _
                   (Addr.txtCity.Text <> bor.CompassDemos.City) Or _
                   (Addr.cbState.Text <> bor.CompassDemos.State) Or _
                   (Addr.txtZip.Text <> bor.CompassDemos.Zip) Or _
                   (Replace(HomePhn.txtPhone1.Text & "-" & HomePhn.txtPhone2.Text & "-" & HomePhn.txtPhone3.Text, "-", "") <> Replace(bor.CompassDemos.Phone, "-", "")) Or _
                   (Replace(OtherPhn.txtPhone1.Text & "-" & OtherPhn.txtPhone2.Text & "-" & OtherPhn.txtPhone3.Text, "-", "") <> Replace(bor.CompassDemos.OtherPhoneNum, "-", "")) Or _
                   (Replace(Other2Phn.txtPhone1.Text & "-" & Other2Phn.txtPhone2.Text & "-" & Other2Phn.txtPhone3.Text, "-", "") <> Replace(bor.CompassDemos.OtherPhone2Num, "-", "")) Or _
                   (Other2Phn.txtExt.Text <> "") Or _
                   (HomePhn.cbxPhnMBL.Text <> bor.CompassDemos.HomePhoneMBL) Or _
                   (OtherPhn.cbxPhnMBL.Text <> bor.CompassDemos.OtherPhoneMBL) Or _
                   (Other2Phn.cbxPhnMBL.Text <> bor.CompassDemos.OtherPhone2MBL) Or _
                   (_indicators("Address").ckbNotValid.Checked And bor.CompassDemos.SPAddrInd = "Y") Or _
                   (_indicators("HomePhone").ckbNotValid.Checked And bor.CompassDemos.HomePhoneValidityIndicator = "Y") Or _
                   (_indicators("OtherPhone").ckbNotValid.Checked And bor.CompassDemos.OtherPhoneValidityIndicator = "Y") Or _
                   (_indicators("OtherPhone2").ckbNotValid.Checked And bor.CompassDemos.OtherPhone2ValidityIndicator = "Y") Or _
                   (_indicators("Email").ckbNotValid.Checked And bor.CompassDemos.SPEmailInd = "Y") Or _
                   (txtEmail.Text <> bor.CompassDemos.Email) Then
                    Return True
                End If
            End If
        ElseIf whatToVerify = DemographicPartType.Address Then
            'check if any data elements <> possible starting values
            If (Addr.txtAddr1.Text <> bor.OneLINKDemos.Addr1) Or _
               (Addr.txtAddr2.Text <> bor.OneLINKDemos.Addr2) Or _
               (Addr.txtCity.Text <> bor.OneLINKDemos.City) Or _
               (Addr.cbState.Text <> bor.OneLINKDemos.State) Or _
               (Addr.txtZip.Text <> bor.OneLINKDemos.Zip) Then
                Return True
            End If
            'if compass info was found compare text in boxes to both systems  
            If COMPASSInfoFound Then
                'check if any data elements <> possible starting values
                If (Addr.txtAddr1.Text <> bor.CompassDemos.Addr1) Or _
                   (Addr.txtAddr2.Text <> bor.CompassDemos.Addr2) Or _
                   (Addr.txtCity.Text <> bor.CompassDemos.City) Or _
                   (Addr.cbState.Text <> bor.CompassDemos.State) Or _
                   (Addr.txtZip.Text <> bor.CompassDemos.Zip) Then
                    Return True
                End If
            End If
        ElseIf whatToVerify = DemographicPartType.HomePhone Then
            'check if any data elements <> possible starting values
            If (Replace(HomePhn.txtPhone1.Text & "-" & HomePhn.txtPhone2.Text & "-" & HomePhn.txtPhone3.Text, "-", "") <> Replace(bor.OneLINKDemos.Phone, "-", "")) Or _
               (HomePhn.txtExt.Text <> bor.OneLINKDemos.HomePhoneExt) Then
                Return True
            End If
            'if compass info was found compare text in boxes to both systems  
            If COMPASSInfoFound Then
                'check if any data elements <> possible starting values
                If (Replace(HomePhn.txtPhone1.Text & "-" & HomePhn.txtPhone2.Text & "-" & HomePhn.txtPhone3.Text, "-", "") <> Replace(bor.CompassDemos.Phone, "-", "")) Or _
               (HomePhn.txtExt.Text <> bor.CompassDemos.HomePhoneExt) Then
                    Return True
                End If
            End If
        ElseIf whatToVerify = DemographicPartType.OtherPhone Then
            'check if any data elements <> possible starting values
            If (Replace(OtherPhn.txtPhone1.Text & "-" & OtherPhn.txtPhone2.Text & "-" & OtherPhn.txtPhone3.Text, "-", "") <> Replace(bor.OneLINKDemos.OtherPhoneNum, "-", "")) Or _
               (OtherPhn.txtExt.Text <> bor.OneLINKDemos.OtherPhoneExt) Then
                Return True
            End If
            'if compass info was found compare text in boxes to both systems  
            If COMPASSInfoFound Then
                'check if any data elements <> possible starting values
                If (Replace(OtherPhn.txtPhone1.Text & "-" & OtherPhn.txtPhone2.Text & "-" & OtherPhn.txtPhone3.Text, "-", "") <> Replace(bor.CompassDemos.OtherPhoneNum, "-", "")) Or _
               (OtherPhn.txtExt.Text <> bor.CompassDemos.OtherPhoneExt) Then
                    Return True
                End If
            End If
        ElseIf whatToVerify = DemographicPartType.OtherPhone2 Then
            'if compass info was found compare text in boxes to both systems  
            If COMPASSInfoFound Then
                'check if any data elements <> possible starting values
                If (Replace(Other2Phn.txtPhone1.Text & "-" & Other2Phn.txtPhone2.Text & "-" & Other2Phn.txtPhone3.Text, "-", "") <> Replace(bor.CompassDemos.OtherPhone2Num, "-", "")) Or _
               (Other2Phn.txtExt.Text <> bor.CompassDemos.OtherPhone2Ext) Then
                    Return True
                End If
            End If
        ElseIf whatToVerify = DemographicPartType.Email Then
            'check if any data elements <> possible starting values
            If (txtEmail.Text <> bor.OneLINKDemos.Email) Then
                Return True
            End If
            'if compass info was found compare text in boxes to both systems  
            If COMPASSInfoFound Then
                'check if any data elements <> possible starting values
                If (txtEmail.Text <> bor.CompassDemos.Email) Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Public Function CreateCommentString(ByVal objectToCheckAgainst As Demographics) As String
        Dim Comment1 As String
        Dim Comment2 As String 'will be added to the back of the comment1 string when both are composed
        'figure contents of the comment string
        'address
        If _indicators("Address").ckbVerified.Checked Or Addr.txtAddr1.Text <> objectToCheckAgainst.Addr1 Or _
           Addr.txtAddr2.Text <> objectToCheckAgainst.Addr2 Then
            If _indicators("Address").ckbNotValid.Checked Then
                Comment1 = "AI "
                Comment2 = "Address Verif Invalid, "
            Else
                Comment1 = "AV "
                Comment2 = "Address Verif, "
            End If
        Else
            Comment1 = "AX "
            Comment2 = "Address Not Verif, "
        End If

        'phone
        If _indicators("HomePhone").ckbVerified.Checked Or _
        HomePhn.txtPhone1.Text & HomePhn.txtPhone2.Text & HomePhn.txtPhone3.Text <> objectToCheckAgainst.Phone Then
            If _indicators("HomePhone").ckbNotValid.Checked Then
                Comment1 = Comment1 & "PI "
                Comment2 = Comment2 & "Phone Verif Invalid, "
            Else
                Comment1 = Comment1 & "PV "
                Comment2 = Comment2 & "Phone Verif, "
            End If
        Else
            Comment1 = Comment1 & "PX "
            Comment2 = Comment2 & "Phone Not Verif, "
        End If

        'other phone
        If _indicators("OtherPhone").ckbVerified.Checked Or _
            (OtherPhn.txtPhone1.Text & OtherPhn.txtPhone2.Text & OtherPhn.txtPhone3.Text <> (objectToCheckAgainst.OtherPhoneNum)) Then
            If _indicators("OtherPhone").ckbNotValid.Checked Then
                Comment1 = Comment1 & "O1I "
                Comment2 = Comment2 & "Other Phone 1 Verif Invalid, "
            Else
                Comment1 = Comment1 & "O1V "
                Comment2 = Comment2 & "Other Phone 1 Verif, "
            End If
        ElseIf objectToCheckAgainst.OtherPhoneNum <> "" Or _
            OtherPhn.txtPhone1.Text & OtherPhn.txtPhone2.Text & OtherPhn.txtPhone3.Text <> "" Then
            Comment1 = Comment1 & "O1X "
            Comment2 = Comment2 & "Other Phone 1 Not Verif, "
        End If

        If TypeOf objectToCheckAgainst Is COMPASSDemographics Then
            'other phone 2
            If _indicators("OtherPhone2").ckbVerified.Checked Or (Other2Phn.txtPhone1.Text & Other2Phn.txtPhone2.Text & Other2Phn.txtPhone3.Text <> objectToCheckAgainst.OtherPhone2Num) Then
                If _indicators("OtherPhone2").ckbNotValid.Checked Then
                    Comment1 = Comment1 & "O2I "
                    Comment2 = Comment2 & "Other Phone 2 Verif Invalid, "
                Else
                    Comment1 = Comment1 & "O2V "
                    Comment2 = Comment2 & "Other Phone 2 Verif, "
                End If
            ElseIf (objectToCheckAgainst.OtherPhone2Num <> "" Or _
            Other2Phn.txtPhone1.Text & Other2Phn.txtPhone2.Text & Other2Phn.txtPhone3.Text <> "") Then
                Comment1 = Comment1 & "O2X "
                Comment2 = Comment2 & "Other Phone 2 Not Verif, "
            End If
        End If

        'email
        If _indicators("Email").ckbVerified.Checked Or (txtEmail.Text <> objectToCheckAgainst.Email) Then

            If _indicators("Email").ckbNotValid.Checked Then
                Comment1 = Comment1 & "EI "
                Comment2 = Comment2 & "Email Verif Invalid, "
            Else
                Comment1 = Comment1 & "EV "
                Comment2 = Comment2 & "Email Verif, "
            End If
        Else
            Comment1 = Comment1 & "EX "
            Comment2 = Comment2 & "Email Not Verif, "
        End If
        'No Phone
        If CType(Me.ParentForm, frmDemographics).ckbNoPhone.Checked Then
            Comment1 = Comment1 & "NP "
            Comment2 = Comment2 & "No Phone, "
        Else
            Comment1 = Comment1 & "   "
        End If

        If TypeOf objectToCheckAgainst Is COMPASSDemographics Then
            Comment2 = Comment2 & objectToCheckAgainst.Addr1 & " " & objectToCheckAgainst.Addr2 & " " & objectToCheckAgainst.City & " " & objectToCheckAgainst.State & " " & objectToCheckAgainst.Zip & " " & objectToCheckAgainst.Phone & "," & objectToCheckAgainst.OtherPhoneNum & "," & objectToCheckAgainst.OtherPhone2Num
        Else
            Comment2 = Comment2 & objectToCheckAgainst.Addr1 & " " & objectToCheckAgainst.Addr2 & " " & objectToCheckAgainst.City & " " & objectToCheckAgainst.State & " " & objectToCheckAgainst.Zip & " " & objectToCheckAgainst.Phone & "," & objectToCheckAgainst.OtherPhoneNum
        End If



        Return Comment1 & Comment2 & "{MAUIDUDE}"
    End Function

    Public Sub NoPhone(ByVal checkBoxChecked As Boolean)
        If checkBoxChecked Then
            HomePhn.txtPhone1.Text = "801"
            HomePhn.txtPhone2.Text = "555"
            HomePhn.txtPhone3.Text = "1212"
            HomePhn.txtExt.Clear()
            HomePhn.Enabled = False
            OtherPhn.txtPhone1.Text = "801"
            OtherPhn.txtPhone2.Text = "555"
            OtherPhn.txtPhone3.Text = "1212"
            OtherPhn.txtExt.Clear()
            OtherPhn.Enabled = False
            Other2Phn.txtPhone1.Text = "801"
            Other2Phn.txtPhone2.Text = "555"
            Other2Phn.txtPhone3.Text = "1212"
            Other2Phn.txtExt.Clear()
            Other2Phn.Enabled = False
        Else
            HomePhn.txtPhone1.Clear()
            HomePhn.txtPhone2.Clear()
            HomePhn.txtPhone3.Clear()
            HomePhn.txtExt.Clear()
            HomePhn.Enabled = True
            OtherPhn.txtPhone1.Clear()
            OtherPhn.txtPhone2.Clear()
            OtherPhn.txtPhone3.Clear()
            OtherPhn.txtExt.Clear()
            OtherPhn.Enabled = True
            Other2Phn.txtPhone1.Clear()
            Other2Phn.txtPhone2.Clear()
            Other2Phn.txtPhone3.Clear()
            Other2Phn.txtExt.Clear()
            Other2Phn.Enabled = True
        End If
    End Sub


    Public Sub AddStateComboBoxEventHandler()
        Addr.AddStateComboBoxEventHandler()
    End Sub

    Public Sub RemoveStateComboBoxEventHandler()
        Addr.RemoveStateComboBoxEventHandler()
    End Sub
End Class
