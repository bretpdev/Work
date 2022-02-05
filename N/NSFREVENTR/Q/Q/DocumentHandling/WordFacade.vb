Imports System.IO
Imports System.Threading
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word

Public Class WordFacade
    Implements IDisposable

    Private _word As Word.Application

    ''' <summary>
    ''' Gets or sets the name of the font at the cursor's location.
    ''' </summary>
    Public Property FontName() As String
        Get
            Return _word.Selection.Font.Name
        End Get
        Set(ByVal value As String)
            _word.Selection.Font.Name = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the size of the font at the cursor's location.
    ''' </summary>
    Public Property FontSize() As Single
        Get
            Return _word.Selection.Font.Size
        End Get
        Set(ByVal value As Single)
            _word.Selection.Font.Size = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the visibility of the Word application.
    ''' </summary>
    Public Property Visible() As Boolean
        Get
            Return _word.Visible
        End Get
        Set(ByVal value As Boolean)
            _word.Visible = value
        End Set
    End Property


    'The constructor is kept private to enforce the use of factory methods.
    Private Sub New()
        _word = New Word.Application()
        _word.Visible = False
    End Sub

    ''' <summary>
    ''' Factory method for creating a WordFacade with a new, blank document.
    ''' </summary>
    Public Shared Function CreateDocument() As WordFacade
        Dim turd As New WordFacade()
        turd._word.Documents.Add(DocumentType:=WdNewDocumentType.wdNewBlankDocument)
        Return turd
    End Function

    ''' <summary>
    ''' Factory method for creating a WordFacade around an existing document.
    ''' </summary>
    ''' <param name="fileName">Full path and name of the document to open.</param>
    Public Shared Function OpenDocument(ByVal fileName As String) As WordFacade
        Dim turd As New WordFacade()
        turd._word.Documents.Open(FileName:=fileName, ConfirmConversions:=False, ReadOnly:=True, AddToRecentFiles:=False, Revert:=False, Format:=WdOpenFormat.wdOpenFormatAuto)
        Return turd
    End Function

    ''' <summary>
    ''' Sends the full document to the default printer.
    ''' </summary>
    Public Sub Print()
        _word.PrintOut()
    End Sub

    ''' <summary>
    ''' Saves the document to a specified location.
    ''' This method blocks until it can verify that the file exists.
    ''' </summary>
    ''' <param name="fileName">The full path and name (including extension) of the desired output document.</param>
    Public Sub SaveAs(ByVal fileName As String)
        _word.ActiveDocument.SaveAs(fileName, WdSaveFormat.wdFormatDocument)
        While Not File.Exists(fileName)
            Thread.Sleep(500)
        End While
    End Sub

    ''' <summary>
    ''' Writes text to the document at the current cursor location, without moving to the next line.
    ''' </summary>
    ''' <param name="text">The text to write to the document.</param>
    Public Sub Write(ByVal text As String)
        _word.Selection.TypeText(text)
    End Sub

    ''' <summary>
    ''' Writes text to the document at the current cursor location without moving to the next line.
    ''' Placeholders in the format string are replaced by string representations of the corresponding objects in the args array.
    ''' </summary>
    ''' <param name="format">A composite format string.</param>
    ''' <param name="args">An object to format.</param>
    Public Sub Write(ByVal format As String, ByVal ParamArray args() As Object)
        Dim text As String = String.Format(format, args)
        Write(text)
    End Sub

    ''' <summary>
    ''' Writes a blank line to the document from the current cursor location.
    ''' </summary>
    Public Sub WriteLine()
        _word.Selection.TypeParagraph()
    End Sub

    ''' <summary>
    ''' Writes text to the document at the current cursor location, ending with a hard return.
    ''' </summary>
    ''' <param name="text">The text to write to the document.</param>
    Public Sub WriteLine(ByVal text As String)
        Write(text)
        _word.Selection.TypeParagraph()
    End Sub

    ''' <summary>
    ''' Writes text to the document at the current cursor location, ending with a hard return.
    ''' Placeholders in the format string are replaced by string representations of the corresponding objects in the args array.
    ''' </summary>
    ''' <param name="format">A composite format string.</param>
    ''' <param name="args">An object to format.</param>
    Public Sub WriteLine(ByVal format As String, ByVal ParamArray args() As Object)
        Write(format, args)
        _word.Selection.TypeParagraph()
    End Sub

#Region " IDisposable Support "
    Private _isDisposed As Boolean = False 'To detect redundant calls

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If (Not _isDisposed) Then
            If (disposing) Then
                'Dispose managed objects.
            End If

            _word.Application.Quit(SaveChanges:=WdSaveOptions.wdDoNotSaveChanges)
            _word = Nothing
        End If
        _isDisposed = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
