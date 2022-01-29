Imports Uheaa.Common

Public Class NORADLoginInfo

    Const KEY As String = "NoRaD is A Fine Name FOR a DB but What aBoUt Farfignutin?"
    Const LOGIN_INFO_FILE As String = "X:\PADU\Security Access\NORAD\NORAD LOGIN.txt"


    ''' <summary>
    ''' Retrieves and decrypts NORAD Login information from file.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RetrieveNORADUserName() As String
        Dim userName As String
        Using sr As New StreamR(LOGIN_INFO_FILE)
            Dim pwd As String = sr.ReadLine() 'read in pwd which is not needed
            userName = sr.ReadLine()
        End Using
        'decrypt data
        Dim decrypter As New StringEncryption(KEY)
        userName = decrypter.DecryptString(userName)
        Return userName
    End Function

    ''' <summary>
    ''' Retrieves and decrypts NORAD Login information from file.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RetrieveNORADPassword() As String
        Dim pwd As String
        Using sr As New StreamR(LOGIN_INFO_FILE)
            pwd = sr.ReadLine()
        End Using
        'decrypt data
        Dim decrypter As New StringEncryption(KEY)
        pwd = decrypter.DecryptString(pwd)
        Return pwd
    End Function

    ''' <summary>
    ''' Encrypts and saves NORAD login information to file.  NOT TO BE USED IN THE NORMAL COURSE OF ANY SCRIPTS.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SetNORADLoginInfo()
        Dim userName As String = "user name here"
        Dim password As String = "password here"
        Dim fileHandle As String = FreeFile()
        FileOpen(fileHandle, LOGIN_INFO_FILE, OpenMode.Output, OpenAccess.Write, OpenShare.LockReadWrite)
        Dim encrypter As New StringEncryption(KEY)
        userName = encrypter.EncryptString(userName)
        password = encrypter.EncryptString(password)
        PrintLine(fileHandle, password)
        PrintLine(fileHandle, userName)
        FileClose(fileHandle)
    End Sub

End Class
