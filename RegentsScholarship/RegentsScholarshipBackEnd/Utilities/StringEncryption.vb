Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class StringEncryption

    Private _encryptionKey As String

    Public Sub New(ByVal encryptionKey As String)
        _encryptionKey = encryptionKey
    End Sub

    ''' <summary>
    ''' Encrypts text
    ''' </summary>
    ''' <param name="textToBeEncrypted">Text to be encrypted</param>
    ''' <returns>Encrypted text</returns>
    ''' <remarks></remarks>
    Public Function EncryptString(ByVal textToBeEncrypted As String) As String
        Dim bytValue() As Byte
        Dim bytKey() As Byte
        Dim bytEncoded() As Byte = New Byte() {}
        Dim bytIV() As Byte = {121, 9, 10, 1, 31, 74, 11, 39, 255, 91, 45, 78, 55, 66, 22, 7}
        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim objMemoryStream As New MemoryStream
        Dim objCryptoStream As CryptoStream
        Dim objRijndaelManaged As RijndaelManaged
        'Strip any null character from string to be encrypted
        textToBeEncrypted = StripNullCharacters(textToBeEncrypted)
        'Value must be within ASCII range
        bytValue = Encoding.ASCII.GetBytes(textToBeEncrypted.ToCharArray)
        intLength = Len(_encryptionKey)
        '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
        '   ******   If it is longer than 32 bytes it will be truncated.  ******
        '   ******   If it is shorter than 32 bytes it will be padded     ******
        '   ******   with upper-case Xs.                                  ****** 
        If intLength >= 32 Then
            _encryptionKey = Strings.Left(_encryptionKey, 32)
        Else
            intLength = Len(_encryptionKey)
            intRemaining = 32 - intLength
            _encryptionKey = _encryptionKey & Strings.StrDup(intRemaining, "X")
        End If
        bytKey = Encoding.ASCII.GetBytes(_encryptionKey.ToCharArray)
        objRijndaelManaged = New RijndaelManaged
        'Create the encryptor and write value to it after it is converted into a byte array
        Try
            objCryptoStream = New CryptoStream(objMemoryStream, _
              objRijndaelManaged.CreateEncryptor(bytKey, bytIV), _
              CryptoStreamMode.Write)
            objCryptoStream.Write(bytValue, 0, bytValue.Length)
            objCryptoStream.FlushFinalBlock()
            bytEncoded = objMemoryStream.ToArray()
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch
        End Try
        'Return encrypted value (converted from  byte Array to a base64 string).  Base64 is MIME encoding)
        Return Convert.ToBase64String(bytEncoded)
    End Function

    ''' <summary>
    ''' Decrypts text
    ''' </summary>
    ''' <param name="textToBeDecrypted">Text to be decrypted</param>
    ''' <returns>Decrypted text</returns>
    ''' <remarks></remarks>
    Public Function DecryptString(ByVal textToBeDecrypted As String) As String
        Dim bytDataToBeDecrypted() As Byte
        Dim bytTemp() As Byte
        Dim bytIV() As Byte = {121, 9, 10, 1, 31, 74, 11, 39, 255, 91, 45, 78, 55, 66, 22, 7}
        Dim objRijndaelManaged As New RijndaelManaged
        Dim objMemoryStream As MemoryStream
        Dim objCryptoStream As CryptoStream
        Dim bytDecryptionKey() As Byte

        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim strReturnString As String = ""
        '   ******   Convert base64 encrypted value to byte array      ******
        bytDataToBeDecrypted = Convert.FromBase64String(textToBeDecrypted)
        '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
        '   ******   If it is longer than 32 bytes it will be truncated.  ******
        '   ******   If it is shorter than 32 bytes it will be padded     ******
        '   ******   with upper-case Xs.                                  ****** 
        intLength = Len(_encryptionKey)
        If intLength >= 32 Then
            _encryptionKey = Strings.Left(_encryptionKey, 32)
        Else
            intLength = Len(_encryptionKey)
            intRemaining = 32 - intLength
            _encryptionKey = _encryptionKey & Strings.StrDup(intRemaining, "X")
        End If
        bytDecryptionKey = Encoding.ASCII.GetBytes(_encryptionKey.ToCharArray)
        ReDim bytTemp(bytDataToBeDecrypted.Length)
        objMemoryStream = New MemoryStream(bytDataToBeDecrypted)
        '   ******  Create the decryptor and write value to it after it is   ******
        '   ******  converted into a byte array                          ******
        Try
            objCryptoStream = New CryptoStream(objMemoryStream, _
               objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV), _
               CryptoStreamMode.Read)
            objCryptoStream.Read(bytTemp, 0, bytTemp.Length)
            objCryptoStream.FlushFinalBlock()
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch
        End Try
        '   ******   Return decypted value     ******
        Return StripNullCharacters(Encoding.ASCII.GetString(bytTemp))

    End Function

    Private Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String
        Dim intPosition As Integer
        Dim strStringWithOutNulls As String
        intPosition = 1
        strStringWithOutNulls = vstrStringWithNulls
        Do While intPosition > 0
            intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)
            If intPosition > 0 Then
                strStringWithOutNulls = Left$(strStringWithOutNulls, intPosition - 1) & _
                                  Right$(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
            End If
            If intPosition > strStringWithOutNulls.Length Then
                Exit Do
            End If
        Loop
        Return strStringWithOutNulls
    End Function

End Class
