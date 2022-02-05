Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports Uheaa.Common

Public Class ErrorReport
    Inherits ErrorReport(Of Object)

    ''' <summary>
    ''' Creates a new instance of the ErrorReport class.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="reportName">The name to use for this report. "Error Report" and a date/time stamp will be appended to this name.</param>
    ''' <param name="filesystemKey">The key in CSYS.GENR_DAT_EnterpriseFileSystem that points (along with region) to the directory where the error report should be saved.</param>
    ''' <param name="region">The region (CornerStone or UHEAA) the application is running in.</param>
    Public Sub New(ByVal testMode As Boolean, ByVal reportName As String, ByVal filesystemKey As String, ByVal region As ScriptSessionBase.Region)
        MyBase.New(testMode, reportName, filesystemKey, region)
    End Sub
End Class

Public Class ErrorReport(Of T)
    Private ReadOnly _reportName As String
    Private ReadOnly _dataFile As String
    Private ReadOnly _publicationDirectory As String

    ''' <summary>
    ''' Creates a new instance of the ErrorReport class.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="reportName">The name to use for this report. "Error Report" and a date/time stamp will be appended to this name.</param>
    ''' <param name="filesystemKey">The key in CSYS.GENR_DAT_EnterpriseFileSystem that points (along with region) to the directory where the error report should be saved.</param>
    ''' <param name="region">The region (CornerStone or UHEAA) the application is running in.</param>
    Public Sub New(ByVal testMode As Boolean, ByVal reportName As String, ByVal filesystemKey As String, ByVal region As ScriptSessionBase.Region)
        Dim efs As EnterpriseFileSystem = New EnterpriseFileSystem(testMode, region)
        _dataFile = String.Format("{0}ERR_{1}.txt", efs.TempFolder, reportName)
        _publicationDirectory = efs.GetPath(filesystemKey)
        _reportName = reportName
    End Sub

    ''' <summary>
    ''' Writes a record to the error file, with fields including the message and all of the item's properties.
    ''' </summary>
    ''' <param name="message">A message stating the nature of the error.</param>
    ''' <param name="item">An object whose properties contain the data to be written to the error report.</param>
    Public Sub AddRecord(ByVal message As String, ByVal item As T)
        'Reflect into the object to get its property names, which we'll need in one or two places.
        Dim itemProperties As IEnumerable(Of String) = item.GetType().GetProperties().Select(Function(p) p.Name)

        'If this is the first record being written out, start with a header row based on the object's properties.
        If (Not File.Exists(_dataFile)) Then
            Dim headers As New List(Of String)()
            'Start with the error message.
            headers.Add("Message")
            'Add the properties of the object.
            headers.AddRange(itemProperties)
            Using headerWriter As New StreamW(_dataFile)
                headerWriter.WriteCommaDelimitedLine(headers.ToArray())
            End Using
        End If

        'Write out a data line from the message and the object's properties.
        Dim values As New List(Of String)()
        values.Add(message)
        For Each propertyName As String In itemProperties
            Dim itemProperty As Object = item.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, Nothing, item, New Object() {})
            values.Add(If(itemProperty Is Nothing, "", itemProperty.ToString()))
        Next propertyName
        Using dataWriter As New StreamW(_dataFile, True)
            dataWriter.WriteCommaDelimitedLine(values.ToArray())
        End Using
    End Sub

    ''' <summary>
    ''' Produces an HTML file based on the error data, and saves it to the directory indicated by the filesystem key specified in the constructor.
    ''' </summary>
    Public Sub Publish()
        'See if there's anything to publish.
        If (Not File.Exists(_dataFile)) Then Return

        'In case this method gets called more than once per application, use a new file name each time.
        'Slap a counter on the end if needed (i.e., Publish() gets called more than once in a given minute).
        Dim reportNumber As Integer = 1
        Dim htmlFile As String = String.Format("{0}{1} Error Report {2:MM-dd-yyyy HH.mm}.html", _publicationDirectory, _reportName, DateTime.Now)
        While File.Exists(htmlFile)
            Dim oldExtension As String = If(reportNumber = 1, ".html", String.Format(".{0}.html", reportNumber))
            reportNumber += 1
            Dim newExtension As String = String.Format(".{0}.html", reportNumber)
            htmlFile = htmlFile.Replace(oldExtension, newExtension)
        End While
        Using htmlWriter As New StreamW(htmlFile)
            'Start the HTML file and write out the report headers.
            htmlWriter.WriteLine("<html>")
            htmlWriter.WriteLine("<body>")
            htmlWriter.WriteLine(String.Format("{0}<br />", _reportName))
            htmlWriter.WriteLine("Error Report<br />")
            htmlWriter.WriteLine(String.Format("{0}<br />", htmlFile))
            htmlWriter.WriteLine("<br />")
            'Convert the data file's contents to an HTML table and write it out.
            For Each tableLine In Common.CreateDataTableFromFile(_dataFile).ToHtmlLines("  ")
                htmlWriter.WriteLine(tableLine)
            Next
            'Close out the HTML.
            htmlWriter.WriteLine("</body>")
            htmlWriter.WriteLine("</html>")
        End Using

        'Clean up the data file once it's been published.
        FS.Delete(_dataFile)
    End Sub
End Class