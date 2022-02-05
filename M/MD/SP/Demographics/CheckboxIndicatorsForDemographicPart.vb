Imports System.Windows.Forms

Public Class CheckboxIndicatorsForDemographicPart

    Private _indicators As IndicatorsForDemographicPart
    Public Property Indicators() As IndicatorsForDemographicPart
        Get
            Return _indicators
        End Get
        Set(ByVal value As IndicatorsForDemographicPart)
            _indicators = value
        End Set
    End Property

    'demographics linked control (address, phone, other phone, other phone 2 or email)
    Private _linkedControl As Windows.Forms.Control
    Private _demosForUpdate As DemographicsForUpdate
    Private _bor As Borrower
    Private _partType As DemographicsForUpdate.DemographicPartType

    'only for Address part indicators
    Private _cbkAltAddress As CheckBox = Nothing

    ''' <summary>
    ''' Populates important object data elements and does data binding
    ''' </summary>
    ''' <param name="tIndicators">Indicators object</param>
    ''' <param name="tLinkedControl">Demographics part (address, phone, other phone, other phone 2 or email)</param>
    ''' <param name="tCkbAltAddress">Only for address indicators.</param>
    ''' <param name="tDemosForUpdate">Only for address indicators.</param>
    ''' <param name="tBor">Only for address indicators.</param>
    ''' <remarks></remarks>
    Public Sub DataBind(ByVal tIndicators As IndicatorsForDemographicPart, ByVal tLinkedControl As Windows.Forms.Control, ByVal tDemosForUpdate As DemographicsForUpdate, ByVal tBor As Borrower, ByVal tPartType As DemographicsForUpdate.DemographicPartType, Optional ByVal tCkbAltAddress As CheckBox = Nothing)
        _indicators = tIndicators
        IndicatorsForDemographicPartBindingSource.DataSource = _indicators
        _linkedControl = tLinkedControl
        _cbkAltAddress = tCkbAltAddress
        _demosForUpdate = tDemosForUpdate
        _bor = tBor
        _partType = tPartType
    End Sub


    Private Sub ckbNotValid_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbNotValid.CheckedChanged
        'checked if field is NOT Valid
        If ckbInvalidateFirst.Checked And ckbNotValid.Checked Then
            frmWhoaDUDE.WhoaDUDE("DUDE cannot process not valid data and first invalidate functionality at the same time.  You must either provide valid data or leave the data invalid.", "Need Valid Data")
            ckbNotValid.Checked = False
        ElseIf (_cbkAltAddress IsNot Nothing) AndAlso (_cbkAltAddress.Checked And ckbNotValid.Checked) Then
            frmWhoaDUDE.WhoaDUDE("DUDE cannot process not valid data and adding alternate address functionality at the same time.  The legal address must be valid before DUDE can add a alternate address.", "Need Valid Data")
            ckbNotValid.Checked = False
        End If
        If ckbNotValid.Checked Then
            _linkedControl.Enabled = False
        Else
            If ckbInvalidateFirst.Checked = False And ckbNotValid.Checked = False And ckbVerified.Checked = False Then
                _linkedControl.Enabled = True
            End If
        End If
    End Sub

    Private Sub ckbVerified_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbVerified.CheckedChanged
        If ckbVerified.Checked Then
            If _partType = DemographicsForUpdate.DemographicPartType.Address Then
                'for address
                'check if there is something in the text fields to protect
                If CType(_linkedControl, Address).txtAddr1.Text = "" Or CType(Me.ParentForm, frmDemographics).DemoForUpdate.Addr.txtCity.Text = "" Or CType(Me.ParentForm, frmDemographics).DemoForUpdate.Addr.txtZip.Text = "" Then
                    ckbVerified.Checked = False
                    SP.frmWipeOut.WipeOut("DUDE can't play the ukulele with one of the necessary address fields blank.", "Blank Address Field", True)
                Else
                    _linkedControl.Enabled = False
                    CType(Me.ParentForm, frmDemographics).OneLINKDemographicsForDisplay.btnUseThisAddr.Enabled = False
                    CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.btnUseThisAddr.Enabled = False
                End If
            Else
                'for everything other than address
                _linkedControl.Enabled = False
                CType(Me.ParentForm, frmDemographics).OneLINKDemographicsForDisplay.btnUseThisAddr.Enabled = False
                CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.btnUseThisAddr.Enabled = False
            End If
        Else
            If ckbInvalidateFirst.Checked = False And ckbNotValid.Checked = False And ckbVerified.Checked = False Then
                _linkedControl.Enabled = True
                CType(Me.ParentForm, frmDemographics).OneLINKDemographicsForDisplay.btnUseThisAddr.Enabled = True
                CType(Me.ParentForm, frmDemographics).CompassDemographicsForDisplay.btnUseThisAddr.Enabled = True
            End If
        End If
    End Sub

    Private Sub ckbInvalidateFirst_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbInvalidateFirst.CheckedChanged
        Dim errorMessage As String = String.Empty
        'create error message for different demographic parts
        Select Case _partType
            Case DemographicsForUpdate.DemographicPartType.Address
                errorMessage = "DUDE can't invalidate the original address first until you have modified the original address so DUDE has a new address.  Please try again."
            Case DemographicsForUpdate.DemographicPartType.HomePhone
                errorMessage = "DUDE can't invalidate the original home phone first until you have modified the original home phone so DUDE has a new home phone.  Please try again."
            Case DemographicsForUpdate.DemographicPartType.OtherPhone
                errorMessage = "DUDE can't invalidate the original other phone first until you have modified the original other phone so DUDE has a new other phone.  Please try again."
            Case DemographicsForUpdate.DemographicPartType.OtherPhone2
                errorMessage = "DUDE can't invalidate the original other phone #2 first until you have modified the original other phone #2 so DUDE has a new other phone #2.  Please try again."
            Case DemographicsForUpdate.DemographicPartType.Email
                errorMessage = "DUDE can't invalidate the original email first until you have modified the original email so DUDE has a new email.  Please try again."
        End Select
        If ckbInvalidateFirst.Checked And ckbNotValid.Checked Then
            frmWhoaDUDE.WhoaDUDE("DUDE must have valid data before it can invalidate the current data.  Please provide valid data and uncheck the not valid check box to use the invalidate first functionality.", "Need Valid Data")
            ckbInvalidateFirst.Checked = False
        ElseIf (_cbkAltAddress IsNot Nothing) AndAlso (_cbkAltAddress.Checked And ckbInvalidateFirst.Checked) Then
            frmWhoaDUDE.WhoaDUDE("DUDE can't do adding alternate address and invalidate first functionality at the same time.  Please try again.", "Need Valid Data")
            ckbInvalidateFirst.Checked = False
        ElseIf ckbInvalidateFirst.Checked And _demosForUpdate.VerifyModification(_bor, _partType) = False Then
            frmWhoaDUDE.WhoaDUDE(errorMessage, "Need Valid Data")
            ckbInvalidateFirst.Checked = False
        End If
        If ckbInvalidateFirst.Checked Then
            _linkedControl.Enabled = False
        Else
            If ckbInvalidateFirst.Checked = False And ckbNotValid.Checked = False And ckbVerified.Checked = False Then
                _linkedControl.Enabled = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Removes checkbox event handlers so the checkbox values can be modified during DUDE processing
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RemoveEvenHandlers()
        RemoveHandler ckbInvalidateFirst.CheckedChanged, AddressOf ckbInvalidateFirst_CheckedChanged
        RemoveHandler ckbVerified.CheckedChanged, AddressOf ckbVerified_CheckedChanged
        RemoveHandler ckbNotValid.CheckedChanged, AddressOf ckbNotValid_CheckedChanged
    End Sub

    ''' <summary>
    ''' Creates a copy of the CheckboxIndicatorsForDemographicPart structure but only copies over the checkbox checked values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CloneWithCheckboxesMarked() As CheckboxIndicatorsForDemographicPart
        Dim dolly As New CheckboxIndicatorsForDemographicPart()
        dolly.RemoveEvenHandlers()
        dolly.ckbInvalidateFirst.Checked = Me.ckbInvalidateFirst.Checked
        dolly.ckbNotValid.Checked = Me.ckbNotValid.Checked
        dolly.ckbVerified.Checked = Me.ckbVerified.Checked
        dolly.ckbConsent.Checked = Me.ckbConsent.Checked
        Return dolly
    End Function

End Class
