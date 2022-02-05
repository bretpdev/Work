Imports System.Windows.Forms

Public Class InputBoxResults

    Private _dialogRe As DialogResult
    ''' <summary>
    ''' Dialog results (which button the user clicked).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DialogRe() As DialogResult
        Get
            Return _dialogRe
        End Get
        Set(ByVal value As DialogResult)
            _dialogRe = value
        End Set
    End Property

    ''' <summary>
    ''' The text typed into the text box by the user.
    ''' </summary>
    ''' <remarks></remarks>
    Private _userProvidedText As String
    Public Property UserProvidedText() As String
        Get
            Return _userProvidedText
        End Get
        Set(ByVal value As String)
            _userProvidedText = value
        End Set
    End Property


End Class
