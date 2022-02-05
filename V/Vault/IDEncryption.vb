Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class IDEncryption
    '128 Bit Encryption requiring a key for encryption and decryption
    Public Function EncryptString(ByVal plainText As String, _
                                        ByVal encryptionKey As String) As String
        'Strip any null character from string to be encrypted
        plainText = StripNullCharacters(plainText)
        '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
        '   ******   If it is longer than 32 bytes it will be truncated.  ******
        '   ******   If it is shorter than 32 bytes it will be padded     ******
        '   ******   with upper-case Xs.                                  ****** 
        If encryptionKey.Length > 32 Then
            encryptionKey = encryptionKey.Substring(0, 32)
        Else
            Do While encryptionKey.Length < 32
                encryptionKey += "X"
            Loop
        End If
        'Create the encryptor and write value to it after it is converted into a byte array
        Try
            Dim memStream As New MemoryStream()
            Dim keyBytes() As Byte = Encoding.ASCII.GetBytes(encryptionKey.ToCharArray())
            Dim rgbIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
            Dim encryptionStream As CryptoStream = New CryptoStream(memStream, _
              New RijndaelManaged().CreateEncryptor(keyBytes, rgbIV), _
              CryptoStreamMode.Write)
            Dim textBytes() As Byte = Encoding.ASCII.GetBytes(plainText.ToCharArray())
            encryptionStream.Write(textBytes, 0, textBytes.Length)
            encryptionStream.FlushFinalBlock()
            Dim encodedBytes() As Byte = memStream.ToArray()
            memStream.Close()
            encryptionStream.Close()
            'Return encryptes value (converted from  byte Array to a base64 string).  Base64 is MIME encoding)
            Return Convert.ToBase64String(encodedBytes)
        Catch
            Return String.Empty
        End Try
    End Function

    Public Function DecryptString(ByVal encryptedText As String, _
                                        ByVal decryptionKey As String) As String
        '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
        '   ******   If it is longer than 32 bytes it will be truncated.  ******
        '   ******   If it is shorter than 32 bytes it will be padded     ******
        '   ******   with upper-case Xs.                                  ****** 
        If decryptionKey.Length > 32 Then
            decryptionKey = decryptionKey.Substring(0, 32)
        Else
            Do While decryptionKey.Length < 32
                decryptionKey += "X"
            Loop
        End If
        Dim keyBytes() As Byte = Encoding.ASCII.GetBytes(decryptionKey.ToCharArray)
        Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedText)
        Dim memStream As New MemoryStream(encryptedBytes)
        '   ******  Create the decryptor and write value to it after it is   ******
        '   ******  converted into a byte array                              ******
        Try
            Dim rgbIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
            Dim encryptionStream As New CryptoStream(memStream, _
               New RijndaelManaged().CreateDecryptor(keyBytes, rgbIV), _
               CryptoStreamMode.Read)
            Dim decryptedBytes(encryptedBytes.Length) As Byte
            encryptionStream.Read(decryptedBytes, 0, decryptedBytes.Length)
            memStream.Close()
            encryptionStream.Close()
            '   ******   Return decypted value     ******
            Return StripNullCharacters(Encoding.ASCII.GetString(decryptedBytes))
        Catch
            Return String.Empty
        End Try
    End Function

    Public Function StripNullCharacters(ByVal rawText As String) As String
        Dim strippedText As String = rawText
        Dim intPosition As Integer = 1
        Do While intPosition > 0
            intPosition = InStr(intPosition, rawText, vbNullChar)
            If intPosition > 0 Then
                strippedText = Left$(strippedText, intPosition - 1) & _
                                  Right$(strippedText, Len(strippedText) - intPosition)
            End If
            If intPosition > strippedText.Length Then
                Exit Do
            End If
        Loop
        Return strippedText
    End Function
End Class
