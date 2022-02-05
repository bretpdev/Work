Imports System.Windows.Forms

Public Class Address

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddStateComboBoxEventHandler()
    End Sub

    Private Sub txtAddr1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddr1.GotFocus
        txtAddr1.SelectionLength = 0
    End Sub

    Public Function ValidUserInput(Optional ByVal bor As Borrower = Nothing) As Boolean
        'check if city and first addr line have data
        If txtAddr1.TextLength = 0 Or txtCity.TextLength = 0 Then
            SP.frmWhoaDUDE.WhoaDUDE("DUDE can't play the ukulele with out the Address 1, City, State, and Zip", "Needed Information Omitted", True)
            Return False
        End If
        If txtZip.Text.Substring(txtZip.Text.Length - 4, 4) = "0000" Then
            SP.frmWhoaDUDE.WhoaDUDE("Your Zip code has 0s like the ocean has jelly fish. Remove the last four 0s and try again.", "Needed Information Omitted", True)
            Return False
        End If
        If bor IsNot Nothing Then
            'skip borrower checks if borrower isn't passed in.
            If bor.OneLINKDemos.Addr1 <> txtAddr1.Text And txtAddr1.Text <> bor.CompassDemos.Addr1 Then
                If (InStr(txtAddr1.Text, "P.O. Box", CompareMethod.Text) OrElse _
                InStr(txtAddr1.Text, "PO Box", CompareMethod.Text) OrElse _
                InStr(txtAddr1.Text, "P O Box", CompareMethod.Text) OrElse _
                InStr(txtAddr1.Text, "P.O Box", CompareMethod.Text) OrElse _
                InStr(txtAddr1.Text, "POBox", CompareMethod.Text) OrElse _
                InStr(txtAddr1.Text, "P.O.Box", CompareMethod.Text) OrElse _
                InStr(txtAddr1.Text, "P/O Box", CompareMethod.Text)) AndAlso (bor.POBoxAllowed = False) Then
                    SP.frmWhoaDUDE.WhoaDUDE("A P.O. Box is not a valid address for this borrower.", "Invalid Character")
                    Return False
                End If
            End If
        End If
        'check if one of the invalid characters is found in either of the addr strings or the city string
        If txtAddr1.Text.IndexOf("!") <> -1 Or txtAddr1.Text.IndexOf("@") <> -1 Or _
            txtAddr1.Text.IndexOf("$") <> -1 Or txtAddr1.Text.IndexOf("%") <> -1 Or txtAddr1.Text.IndexOf("^") <> -1 Or _
            txtAddr1.Text.IndexOf("&") <> -1 Or txtAddr1.Text.IndexOf("*") <> -1 Or txtAddr1.Text.IndexOf("(") <> -1 Or _
            txtAddr1.Text.IndexOf(")") <> -1 Or txtAddr1.Text.IndexOf("-") <> -1 Or txtAddr1.Text.IndexOf("+") <> -1 Or _
            txtAddr1.Text.IndexOf("=") <> -1 Or txtAddr1.Text.IndexOf("<") <> -1 Or txtAddr1.Text.IndexOf(">") <> -1 Or _
            txtAddr1.Text.IndexOf(",") <> -1 Or txtAddr1.Text.IndexOf(".") <> -1 Or txtAddr1.Text.IndexOf("""") <> -1 Or _
            txtAddr1.Text.IndexOf(";") <> -1 Or txtAddr1.Text.IndexOf(":") <> -1 Or txtAddr1.Text.IndexOf("~") <> -1 Or _
            txtAddr1.Text.IndexOf("'") <> -1 Or txtAddr1.Text.IndexOf("?") <> -1 Or txtAddr2.Text.IndexOf("!") <> -1 Or _
            txtAddr2.Text.IndexOf("@") <> -1 Or txtAddr2.Text.IndexOf("$") <> -1 Or _
            txtAddr2.Text.IndexOf("%") <> -1 Or txtAddr2.Text.IndexOf("^") <> -1 Or txtAddr2.Text.IndexOf("&") <> -1 Or _
            txtAddr2.Text.IndexOf("*") <> -1 Or txtAddr2.Text.IndexOf("(") <> -1 Or txtAddr2.Text.IndexOf(")") <> -1 Or _
            txtAddr2.Text.IndexOf("-") <> -1 Or txtAddr2.Text.IndexOf("+") <> -1 Or txtAddr2.Text.IndexOf("=") <> -1 Or _
            txtAddr2.Text.IndexOf("<") <> -1 Or txtAddr2.Text.IndexOf(">") <> -1 Or txtAddr2.Text.IndexOf(",") <> -1 Or _
            txtAddr2.Text.IndexOf(".") <> -1 Or txtAddr2.Text.IndexOf("""") <> -1 Or txtAddr2.Text.IndexOf(";") <> -1 Or _
            txtAddr2.Text.IndexOf(":") <> -1 Or txtAddr2.Text.IndexOf("~") <> -1 Or txtAddr2.Text.IndexOf("'") <> -1 Or _
            txtAddr2.Text.IndexOf("?") <> -1 Or txtCity.Text.IndexOf("!") <> -1 Or txtCity.Text.IndexOf("@") <> -1 Or _
            txtCity.Text.IndexOf("$") <> -1 Or txtCity.Text.IndexOf("%") <> -1 Or _
            txtCity.Text.IndexOf("^") <> -1 Or txtCity.Text.IndexOf("&") <> -1 Or txtCity.Text.IndexOf("*") <> -1 Or _
            txtCity.Text.IndexOf("(") <> -1 Or txtCity.Text.IndexOf(")") <> -1 Or txtCity.Text.IndexOf("-") <> -1 Or _
            txtCity.Text.IndexOf("+") <> -1 Or txtCity.Text.IndexOf("=") <> -1 Or txtCity.Text.IndexOf("<") <> -1 Or _
            txtCity.Text.IndexOf(">") <> -1 Or txtCity.Text.IndexOf(",") <> -1 Or txtCity.Text.IndexOf(".") <> -1 Or _
            txtCity.Text.IndexOf("""") <> -1 Or txtCity.Text.IndexOf(";") <> -1 Or txtCity.Text.IndexOf(":") <> -1 Or _
            txtCity.Text.IndexOf("~") <> -1 Or txtCity.Text.IndexOf("'") <> -1 Or txtCity.Text.IndexOf("?") <> -1 Then
            'give the user an error message
            SP.frmWhoaDUDE.WhoaDUDE("(!,@,$,%,^,&,*,(,),-,+,=,<,>,,,.,"",;,:,~,',?) DUDE?! Can't be using these characters when updating an address.", "Invalid Character")
            EnableControls()
            txtAddr1.Focus()
            Return False
        End If
        'be sure that a state has been selected or entered
        If cbState.FindStringExact(cbState.Text) = -1 Then
            SP.frmWhoaDUDE.WhoaDUDE("DUDE can't play the ukulele with out a valid State code.", "Needed Information Omitted", True)
            EnableControls()
            cbState.Focus()
            Return False
        End If
        'check for a valid zip code
        If txtZip.TextLength <> 5 And txtZip.TextLength <> 9 Then
            SP.frmWhoaDUDE.WhoaDUDE("You can't do the hula without a lei of flowers and DUDE must have a five or nine digit zip code to do its thing.", "Valid Zip Code Needed", True)
            EnableControls()
            txtZip.Focus()
            Return False
        Else
            If IsNumeric(txtZip.Text) = False Then
                SP.frmWhoaDUDE.WhoaDUDE("You can't do the hula without a lei of flowers and DUDE must have a five digit zip code to do its thing.", "Five Digit Zip Needed", True)
                EnableControls()
                txtZip.Focus()
                Return False
            End If
        End If
        'if the code gets through all the data checks then the data is valid
        Return True
    End Function

    'enables controls on control
    Private Sub EnableControls()
        txtAddr1.Enabled = True
        txtAddr2.Enabled = True
        txtCity.Enabled = True
        cbState.Enabled = True
        txtZip.Enabled = True
    End Sub

    'This gives the combo boxes the functionality of looking up values in them
    Public Sub FindComboItem(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim intPos As Integer
        Dim cboSent As ComboBox = CType(sender, ComboBox)
        If cboSent.FindString(cboSent.Text) <> -1 Then
            intPos = cboSent.Text.Length
            RemoveHandler cboSent.TextChanged, AddressOf FindComboItem
            cboSent.SelectedItem = cboSent.Items(cboSent.FindString(cboSent.Text))
            AddHandler cboSent.TextChanged, AddressOf FindComboItem
            cboSent.SelectionStart = intPos
            cboSent.SelectionLength = cboSent.Text.Length - cboSent.SelectionStart
        Else
            SP.frmWhoaDUDE.WhoaDUDE("That was an invalid entry.", "Invalid Entry", True)
            cboSent.SelectAll()
        End If
    End Sub

    Public Sub AddStateComboBoxEventHandler()
        AddHandler cbState.TextChanged, AddressOf FindComboItem
    End Sub

    Public Sub RemoveStateComboBoxEventHandler()
        RemoveHandler cbState.TextChanged, AddressOf FindComboItem
    End Sub

End Class
