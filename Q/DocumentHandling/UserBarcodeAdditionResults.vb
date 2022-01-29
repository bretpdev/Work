Public Class UserBarcodeAdditionResults

    ''' <summary>
    ''' Return the account number found in the file.
    ''' </summary>
    ''' <remarks></remarks>
    Private _acctNum As String
    Public Property AcctNum() As String
        Get
            Return _acctNum
        End Get
        Set(ByVal value As String)
            _acctNum = value
        End Set
    End Property

    ''' <summary>
    ''' File name that is ready for processing.  Could be same name as fileToProc or a new file name depending on set value of newFile.
    ''' </summary>
    ''' <remarks></remarks>
    Private _newFileName As String
    Public Property NewFileName() As String
        Get
            Return _newFileName
        End Get
        Set(ByVal value As String)
            _newFileName = value
        End Set
    End Property



End Class
