Imports System.IO
Imports System.Text
Imports System.Threading

Public Class frmWebView
    Private _htmlFile As String
    Private _document As mshtml.HTMLDocument

    Public Sub New(ByVal htmlFile As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _htmlFile = htmlFile
    End Sub

    Private Sub frmWebView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AxWebBrowser1.Navigate2(DirectCast(_htmlFile, Object))
        _document = AxWebBrowser1.Document()
        _document.designMode = "On"
    End Sub

    Private Function GetHeader(ByVal fileName As String) As String
        Dim headerBuilder As New StringBuilder()
        For Each fileLine As String In File.ReadAllLines(fileName)
            If fileLine.StartsWith("<!--StartFragment-->") Then Exit For
            headerBuilder.Append(fileLine)
        Next fileLine
        Return headerBuilder.ToString()
    End Function

    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub ToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick
        Select Case e.Button.ImageIndex
            Case 0
                _document.execCommand("Bold")
            Case 1
                _document.execCommand("Italic")
            Case 2
                _document.execCommand("Underline")
            Case 4
                Dim header As String = GetHeader(_htmlFile)
                Dim temporaryFile As String = _htmlFile.Replace(".html", "temp.html")
                Using wOutput As New StreamWriter(temporaryFile, False, UnicodeEncoding.Unicode)
                    wOutput.Write(header)
                    wOutput.Write("<body>")
                    wOutput.Write(_document.body.innerHTML)
                    wOutput.Write("</body></html>")
                    wOutput.Close()
                End Using
                File.Delete(_htmlFile)
                While File.Exists(_htmlFile)
                    Thread.Sleep(500)
                End While
                File.Move(temporaryFile, _htmlFile)
                Me.Hide()
            Case 10
                _document.execCommand("Undo")
            Case 11
                _document.execCommand("Redo")
        End Select
    End Sub
End Class
