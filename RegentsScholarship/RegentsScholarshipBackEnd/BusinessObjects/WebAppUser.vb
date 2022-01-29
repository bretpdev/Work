Public Class WebAppUser
    Private Const ENCRYPTION_KEY As String = "Regents e muito legal, mas pode dar-lhe uma ulcera ou conduzi-lo a bebida."

#Region "Properties"
    Private _address1 As String
    Public Property Address1() As String
        Get
            Return _address1
        End Get
        Set(ByVal value As String)
            _address1 = value
        End Set
    End Property

    Private _address2 As String
    Public Property Address2() As String
        Get
            Return _address2
        End Get
        Set(ByVal value As String)
            _address2 = value
        End Set
    End Property

    Private _city As String
    Public Property City() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property

    Private _country As String
    Public Property Country() As String
        Get
            Return _country
        End Get
        Set(ByVal value As String)
            _country = value
        End Set
    End Property

    Private _emailAddress As String
    Public Property EmailAddress() As String
        Get
            Return _emailAddress
        End Get
        Set(ByVal value As String)
            _emailAddress = value
        End Set
    End Property

    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Private _lastName As String
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _newPassword As String
    Public Property NewPassword() As String
        Get
            Return _newPassword
        End Get
        Set(ByVal value As String)
            _newPassword = value
        End Set
    End Property

    Private _phoneNumber As String
    Public Property PhoneNumber() As String
        Get
            Return _phoneNumber
        End Get
        Set(ByVal value As String)
            _phoneNumber = value
        End Set
    End Property

    Private _state As String
    Public Property State() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property

    Private _stateStudentID As String
    Public Property StateStudentID() As String
        Get
            Return _stateStudentID
        End Get
        Set(ByVal value As String)
            _stateStudentID = value
        End Set
    End Property

    Private _username As String
    Public Property Username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Private _zip As String
    Public Property Zip() As String
        Get
            Return _zip
        End Get
        Set(ByVal value As String)
            _zip = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Creates random password
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetRandomPassword()
        Dim newPassword As New List(Of Char)()
        Dim r As New Random(System.DateTime.Now.Millisecond)
        While newPassword.Count <> 10
            'decide if the next character is going to be a number or a letter
            Dim numOrLtr As Integer = r.Next(0, 2) '0 = num and 1 = ltr
            If numOrLtr = 0 Then
                'add number to list of characters that will eventually be the new password
                newPassword.Add(r.Next(0, 9).ToString())
            Else
                Dim aPWChar As Char = Convert.ToChar(r.Next(65, 90))
                'decide if the letter should stay upper case or be changed to lower case
                Dim upperOrLower As Integer = r.Next(0, 2) '0 = upper and 1 = lower
                If upperOrLower = 0 Then
                    newPassword.Add(aPWChar)
                Else
                    newPassword.Add(Char.ToLower(aPWChar))
                End If
            End If
        End While
        'set new password
        _newPassword = New String(newPassword.ToArray())
    End Sub

    Public Function GetAndEncryptPassword() As String
        Dim encrypt As New StringEncryption(ENCRYPTION_KEY)
        Return encrypt.EncryptString(_newPassword)
    End Function
End Class
