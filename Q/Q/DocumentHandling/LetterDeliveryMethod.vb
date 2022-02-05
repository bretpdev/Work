Imports Q.DocumentHandling.CentralizedPrintingDeploymentMethod
Imports Q.DataAccess
Imports Q.DocumentHandling
Imports Reflection

Public Class LetterDeliveryMethod

#Region "Properties"

    Private _originalEmailBlankOrInvalid As Boolean = False
    Public Property OriginalEmailBlankOrInvalid() As Boolean
        Get
            Return _originalEmailBlankOrInvalid
        End Get
        Set(ByVal value As Boolean)
            _originalEmailBlankOrInvalid = value
        End Set
    End Property

    Private _selectedDeploymentMethod As CentralizedPrintingDeploymentMethod
    Public Property SelectedDeploymentMethod() As CentralizedPrintingDeploymentMethod
        Get
            Return _selectedDeploymentMethod
        End Get
        Set(ByVal value As CentralizedPrintingDeploymentMethod)
            _selectedDeploymentMethod = value
        End Set
    End Property

    Private _rs As Session


#End Region

    Public Sub New(ByVal testMode As Boolean, ByVal tProcInfo As CentralizedPrintingAnd2DBarcodeInfo, ByVal defaultDeploymentMethod As CentralizedPrintingDeploymentMethod)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'load business unit combobox on fax tab
        cmbFaxBU.Items.AddRange(GetBUForLtrDelvryMethFrm(testMode).ToArray)
        If defaultDeploymentMethod <> dmUserPrompt Then
            'if a default method was sent in by application or script
            If defaultDeploymentMethod = dmEmail Then
                tabFax.Hide()
                tabEmail.Select()
                tabLetter.Hide()
            ElseIf defaultDeploymentMethod = dmFax Then
                tabFax.Select()
                tabEmail.Hide()
                tabLetter.Hide()
            End If
        Else
            tabLetter.Select()
        End If
        'check for email update options
        If tProcInfo.BrwDemographics.EmailValidityIndicator = "N" OrElse tProcInfo.BrwDemographics.Email = "" Then
            OriginalEmailBlankOrInvalid = True
            radUpdateEmail.Visible = True
        Else
            OriginalEmailBlankOrInvalid = False
            radUpdateEmail.Visible = False
            radDoNotUpdateEmail.Checked = True
        End If
        cmbFaxBU.SelectedItem = tProcInfo.UsersBusinessUnit
        txtFaxSender.Text = tProcInfo.UsersFirstName
        txtEmailTo.Text = tProcInfo.BrwDemographics.Email
        txtFaxTo.Text = tProcInfo.BrwDemographics.FName + " " + tProcInfo.BrwDemographics.LName
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'do data checks based off what option is selected
        If tabMethods.SelectedTab.Equals(tabEmail) Then 'email selected
            _selectedDeploymentMethod = dmEmail
            If txtEmailTo.TextLength = 0 OrElse txtEmailConfirmTo.TextLength = 0 Then
                MsgBox("You must provide a ""To"" email address and double enter it into the ""Confirm To"" field.", vbInformation, "Delivery Method")
                Exit Sub
            End If
            If txtEmailSubject.TextLength = 0 Then
                MsgBox("You must provide a ""Subject"" for the email.", vbInformation, "Delivery Method")
                Exit Sub
            End If
            'check that double entry for email address matches
            If UCase(txtEmailConfirmTo.Text) <> UCase(txtEmailTo.Text) Then
                MsgBox("The ""To"" and ""Confirm To"" fields must match.", vbInformation, "Delivery Method")
                Exit Sub
            End If
            If radDoNotUpdateEmail.Checked = False AndAlso radUpdateEmail.Checked = False Then
                MsgBox("You must select whether to update the system with the entered email address or not.", vbInformation, "Delivery Method")
                Exit Sub
            End If
        ElseIf tabMethods.SelectedTab.Equals(tabFax) Then 'fax selected
            _selectedDeploymentMethod = dmFax
            If txtFaxFaxNum.TextLength < 10 OrElse txtFaxConfirmFaxNum.TextLength < 10 Then
                MsgBox("You must provide a fax number (including area code) for the fax to be sent to and confirm it.", vbInformation, "Delivery Method")
                Exit Sub
            End If
            If txtFaxTo.TextLength = 0 Then
                MsgBox("You must provide who the fax is being sent to in the ""To"" field.", vbInformation, "Delivery Method")
                Exit Sub
            End If
            If txtFaxSubject.TextLength = 0 Then
                MsgBox("You must provide a ""Subject"" for the fax.", vbInformation, "Delivery Method")
                Exit Sub
            End If
            If txtFaxConfirmFaxNum.Text <> txtFaxFaxNum.Text Then
                MsgBox("The fax number and the confirmed fax number must match.", vbInformation, "Delivery Method")
                Exit Sub
            End If
        ElseIf tabMethods.SelectedTab.Equals(tabLetter) Then 'letter selected
            _selectedDeploymentMethod = dmLetter
        End If
        Me.Hide()
    End Sub

End Class