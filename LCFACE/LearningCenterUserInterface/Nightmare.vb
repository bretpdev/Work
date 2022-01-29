Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms.AxHost
Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop
Imports HtmlAgilityPack

Public Class frmNightmare
    'The PATH variable needs to be "W:\" for production, where the W drive is mapped to \\Paweb\LearningCenter\data\.
    Private Const PATH As String = "W:\"
    'Rather than make custom exceptions, just make custom message constants for the standard Exception object.
    Private Const EXCEPTION_MESSAGE_TEXT_BOX As String = "A text box is set up with inline text wrapping."
    Private Const TEXT_WRAPPING_MESSAGE As String = "The document ""<<document>>"" has one or more text boxes whose text wrapping style is ""In line with text,"" which prevents a clean conversion to HTML. Please check all text boxes to make sure they don't use this text wrapping style. A good alternative is ""Top and bottom."""

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        End
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        OpenFileDialog1.Filter = "Word Documents|*.doc;*.docx"
        OpenFileDialog1.ShowDialog()
        If String.IsNullOrEmpty(OpenFileDialog1.FileName) Then Return

        Try
            Dim htmlFile As String = CreateHtml(OpenFileDialog1.FileName.Replace("\", "\\"))
            CleanHtmlFile(htmlFile, OpenFileDialog1.FileName, False)
            MessageBox.Show("File Translation Complete!")
        Catch ex As Exception
            If ex.Message = EXCEPTION_MESSAGE_TEXT_BOX Then
                MessageBox.Show(TEXT_WRAPPING_MESSAGE.Replace("<<document>>", OpenFileDialog1.FileName), "Unable to Convert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                Throw
            End If
        End Try
    End Sub

    Private Function GetBaseFileName(ByVal fullPathAndFileName As String) As String
        Dim baseFileName As String = fullPathAndFileName
        baseFileName = baseFileName.Substring(baseFileName.LastIndexOf("\") + 1)
        baseFileName = baseFileName.Replace(" ", "_")
        baseFileName = baseFileName.Replace(".docx", "")
        baseFileName = baseFileName.Replace(".doc", "")
        Return baseFileName
    End Function

    Private Function CreateHtml(ByVal inputDocFile As String) As String
        Dim wordApp As New Word.Application
        Dim wordDoc As Word.Document = wordApp.Documents.Open(DirectCast(inputDocFile, Object), Nothing, True)
        wordApp.Visible = False
        wordDoc.AcceptAllRevisions()

        'Convert text boxes to frames and pictures to inline frames.
        'Go through them backwards, because iterating through a For Each loop only converts every other one.
        For x As Integer = wordDoc.Shapes.Count To 1 Step -1
            If wordDoc.Shapes(x).Type = MsoShapeType.msoTextBox Then
                Try
                    wordDoc.Shapes(x).ConvertToFrame().RelativeHorizontalPosition = 40

                Catch ex As Exception
                    Throw New Exception(EXCEPTION_MESSAGE_TEXT_BOX, ex)
                End Try
            ElseIf wordDoc.Shapes(x).Type = MsoShapeType.msoPicture Then
                Dim i As Word.InlineShape = wordDoc.Shapes(x).ConvertToInlineShape()
            End If
        Next x

        'Convert inline shapes to pictures.
        For x As Integer = wordDoc.InlineShapes.Count To 1 Step -1
            Dim pf As Word.PictureFormat = Nothing
            Try
                pf = wordDoc.InlineShapes(x).PictureFormat
            Catch ex As Exception
                wordDoc.InlineShapes(x).Delete()
            End Try
        Next x

        Dim outputHtmlFile As String = String.Format("{0}{1}.html", PATH, GetBaseFileName(inputDocFile))
        Dim rtfFile As String = outputHtmlFile.Replace(".html", ".rtf")
        wordDoc.SaveAs(DirectCast(rtfFile, Object), Word.WdSaveFormat.wdFormatRTF)
        wordDoc.Close()
        Do While Not File.Exists(rtfFile)
            Thread.Sleep(100)
        Loop
        wordDoc = wordApp.Documents.Open(DirectCast(rtfFile, Object), False, True)
        wordApp.Visible = False

        'copy word to clipboard
        wordApp.ActiveDocument.Range.Copy()

        'create new html from clipboard
        Dim htmlWriter As New StreamWriter(outputHtmlFile, False, System.Text.UnicodeEncoding.Unicode)
        Dim htmlOutput As String = Clipboard.GetDataObject.GetData(DataFormats.Html, False)


        Dim closingTag As String = "</html>" + Environment.NewLine
        If (Not htmlOutput.EndsWith(closingTag)) Then
            Dim closingTagIndex As Integer = htmlOutput.LastIndexOf(closingTag)
            htmlOutput = htmlOutput.Substring(0, closingTagIndex + closingTag.Length)
        End If

        htmlWriter.Write(htmlOutput)
        htmlWriter.Close()
        wordDoc.Close(Word.WdSaveOptions.wdDoNotSaveChanges)
        wordApp.Quit()
        File.Delete(rtfFile)
        FixHtml(outputHtmlFile)


        Return outputHtmlFile
    End Function

    Private Sub FixHtml(ByRef outputHtmlFile As String)
        Dim Doc As HtmlDocument = New HtmlDocument()
        Doc.Load(outputHtmlFile)
        FixTableHeight(Doc)
        FixVerticalAlignmentForTableRows(Doc)
        RemoveTDStyleAndHeight(Doc)
        Doc.Save(outputHtmlFile)

    End Sub

    Private Sub FixTableHeight(ByRef doc As HtmlDocument)
        Dim nodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//table")
        If Not nodes Is Nothing Then
            For Each node As HtmlNode In nodes
                node.SetAttributeValue("height", "0")
            Next
        End If
    End Sub


    Private Sub FixVerticalAlignmentForTableRows(ByRef doc As HtmlDocument)
        Dim nodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//table/tr")
        If Not nodes Is Nothing Then
            For Each node As HtmlNode In nodes
                node.InnerHtml = "<td width=50></td>" + node.InnerHtml
            Next
        End If
    End Sub

    Private Sub RemoveTDStyleAndHeight(ByRef doc As HtmlDocument)
        Dim nodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//table//tr/td")
        If Not nodes Is Nothing Then
            For Each node As HtmlNode In nodes
                node.SetAttributeValue("style", "")
                node.SetAttributeValue("height", "")
            Next
        End If
    End Sub


    Private Sub CleanHtmlFile(ByVal inputHtmlFile As String, ByVal document As String, ByVal isBody As Boolean)
        Dim INVISIBLE As String = Chr(194)
        Dim OPEN_QUOTE As String = Chr(226) + Chr(128) + Chr(156)
        Dim CLOSE_QUOTE As String = Chr(226) + Chr(128) + Chr(157)
        Dim APOSTROPHE As String = Chr(226) + Chr(128) + Chr(153)
        Dim EM_DASH As String = Chr(226) + Chr(128) + Chr(147)
        Dim SINGLE_QUOTE As String = Chr(226) + Chr(128) + Chr(152)

        Dim outputFile As String = inputHtmlFile.Replace(".html", ".temp.html")
        Using outputWriter As New StreamWriter(outputFile, False, System.Text.UnicodeEncoding.Unicode)
            Dim lineNumber As Integer = 0
            Dim foundUnderScore As Boolean = False
            For Each inputLine As String In File.ReadAllLines(inputHtmlFile, System.Text.UnicodeEncoding.Unicode)
                lineNumber += 1
                'if uncommented this will remove the header and formating
                'If str = "<!--StartFragment-->" Then isBody = True
                'If str = "<!--EndFragment-->" Then isBody = False
                Dim outputLine As String = inputLine
                If (Not isBody) AndAlso (lineNumber < 8) Then Continue For
                If Not isBody Then
                    RenameImages(inputLine, outputLine, document, ".jpg")
                    RenameImages(inputLine, outputLine, document, ".gif")
                    RenameImages(inputLine, outputLine, document, ".bmp")
                    RenameImages(inputLine, outputLine, document, ".png")
                End If

                outputLine = outputLine.Replace("text-decoration:underline;", "")
                outputLine = outputLine.Replace("text-underline:single;", "")

                'this allows the dot indents
                outputLine = outputLine.Replace("<meta http-equiv=Content-Type content=""text/html; charset=utf-8"">", "")

                'remove empty underlines
                If foundUnderScore Then outputLine = outputLine.Replace("&nbsp;", "")
                If outputLine.Contains("<u>") Then
                    outputLine = outputLine.Replace("&nbsp;", "")
                    foundUnderScore = True
                End If
                If outputLine.Contains("</u>") AndAlso outputLine.IndexOf("</u>") > outputLine.IndexOf("<u>") Then
                    foundUnderScore = False
                End If

                outputLine = outputLine.Replace(INVISIBLE, "")
                outputLine = outputLine.Replace(OPEN_QUOTE, """")
                outputLine = outputLine.Replace(CLOSE_QUOTE, """")
                outputLine = outputLine.Replace(APOSTROPHE, "'")
                outputLine = outputLine.Replace(EM_DASH, Chr(151))
                outputLine = outputLine.Replace(SINGLE_QUOTE, "'")
                outputWriter.WriteLine(outputLine)
            Next inputLine
            outputWriter.Close()
        End Using

        File.Delete(inputHtmlFile)
        While File.Exists(inputHtmlFile)
            Thread.Sleep(500)
        End While
        File.Move(outputFile, inputHtmlFile)
    End Sub

    Private Sub RenameImages(ByRef inputLine As String, ByRef outputLine As String, ByVal document As String, ByVal imageExtension As String)
        Do While inputLine.Contains(imageExtension)
            Dim imageNameStartIndex As Integer = inputLine.LastIndexOf("""", inputLine.IndexOf(imageExtension) - 1) + 1
            Dim imageNameLength As Integer = (inputLine.IndexOf(imageExtension) + imageExtension.Length) - imageNameStartIndex
            Dim imagePathAndName As String = inputLine.Substring(imageNameStartIndex, imageNameLength)
            Dim imageName As String = imagePathAndName.Substring(imagePathAndName.LastIndexOfAny("\/".ToCharArray()) + 1)
            Thread.Sleep(200)
            Dim oldFile As String = imagePathAndName.Replace("file:///", "")
            Dim newFile As String = String.Format("{0}_{1}", GetBaseFileName(document), imageName)
            If File.Exists(oldFile) Then
                File.Copy(oldFile, PATH + newFile)
            End If
            inputLine = inputLine.Replace(imagePathAndName, "")
            outputLine = outputLine.Replace(String.Format("""{0}""", imagePathAndName), newFile)
        Loop
    End Sub

    Private Sub btnBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatch.Click
        If FolderBrowserDialog1.ShowDialog() = DialogResult.Cancel Then Return
        Dim batchPath As String = FolderBrowserDialog1.SelectedPath
        'Get Word documents that are not backup files. "*.doc" will also match "*.docx". See MSDN for an explanation.
        Dim documents As IEnumerable(Of String) = New DirectoryInfo(batchPath).GetFiles("*.doc").Where(Function(p) Not p.Name.StartsWith("~")).Select(Function(p) p.Name)
        Dim failCount As Integer = 0
        For Each document As String In documents
            Try
                Dim htmlFile As String = CreateHtml((batchPath + "\" + document).Replace("\", "\\"))
                CleanHtmlFile(htmlFile, document, False)
            Catch ex As Exception
                If ex.Message = EXCEPTION_MESSAGE_TEXT_BOX Then
                    MessageBox.Show(TEXT_WRAPPING_MESSAGE.Replace("<<document>>", document), "Unable to Convert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    failCount += 1
                Else
                    Throw
                End If
            End Try
        Next document
        Dim message As String = String.Format("Batch Completed!  {0} Documents for Folder: {1}.", (documents.Count() - failCount).ToString(), batchPath)
        MessageBox.Show(message)
    End Sub

    Private Sub btnWebView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWebView.Click
        OpenFileDialog1.Filter = "Word Documents|*.doc;*.docx"
        OpenFileDialog1.ShowDialog()
        If String.IsNullOrEmpty(OpenFileDialog1.FileName) Then Return

        'Try
        Dim htmlFile As String = CreateHtml(OpenFileDialog1.FileName.Replace("\", "\\"))
        CleanHtmlFile(htmlFile, OpenFileDialog1.FileName, False)

        Me.Visible = False
        Dim webView As New frmWebView(htmlFile)
        webView.ShowDialog()
        CleanHtmlFile(htmlFile, OpenFileDialog1.FileName, True)
        MessageBox.Show("File Translation Complete!")
        'Catch ex As Exception
        '    If ex.Message = EXCEPTION_MESSAGE_TEXT_BOX Then
        '        MessageBox.Show(TEXT_WRAPPING_MESSAGE.Replace("<<document>>", OpenFileDialog1.FileName), "Unable to Convert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    Else
        '        Throw
        '    End If
        'End Try
        Me.Visible = True
    End Sub

    Private Sub frmNightmare_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub frmNightmare_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        Dim documents As IEnumerable(Of String) = DirectCast(e.Data.GetData(DataFormats.FileDrop), String()).Where(Function(p) p.Contains(".doc") AndAlso (Not p.StartsWith("~")))
        Dim failCount As Integer = 0
        For Each document As String In documents
            Try
                Dim htmlFile As String = CreateHtml(document.Replace("\", "\\"))
                CleanHtmlFile(htmlFile, document, False)
            Catch ex As Exception
                If ex.Message = EXCEPTION_MESSAGE_TEXT_BOX Then
                    MessageBox.Show(TEXT_WRAPPING_MESSAGE.Replace("<<document>>", document), "Unable to Convert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    failCount += 1
                Else
                    Throw
                End If
            End Try
        Next document
        Dim message As String = String.Format("Batch Completed!  {0} Documents.", (documents.Count() - failCount).ToString())
        MessageBox.Show(message)
    End Sub
End Class

