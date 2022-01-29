Imports System.Collections.Generic
Imports System.Data
Imports System.IO

Public Class EndOfJobReport
    Private ReadOnly _reportName As String
    Private ReadOnly _dataFile As String
    Private ReadOnly _publicationDirectory As String

    Private _counts As Dictionary(Of String, Count)
    Public ReadOnly Property Counts() As Dictionary(Of String, Count)
        Get
            Return _counts
        End Get
    End Property

    ''' <summary>
    ''' Creates a new instance of the EndOfJobReport class.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="reportName">The name to use for this report. "End of Job Report" and a date/time stamp will be appended to this name.</param>
    ''' <param name="filesystemKey">The key in CSYS.GENR_DAT_EnterpriseFileSystem that points (along with region) to the directory where the end-of-job report should be saved.</param>
    ''' <param name="region">The region (CornerStone or UHEAA) the application is running in.</param>
    Public Sub New(ByVal testMode As Boolean, ByVal reportName As String, ByVal filesystemKey As String, ByVal region As ScriptSessionBase.Region, ByVal headers As IEnumerable(Of String))
        Dim efs As EnterpriseFileSystem = New EnterpriseFileSystem(testMode, region)
        _counts = New Dictionary(Of String, Count)
        _dataFile = String.Format("{0}EOJ_{1}.txt", efs.TempFolder, reportName)
        _publicationDirectory = efs.GetPath(filesystemKey)
        _reportName = reportName

        'See if there's already a data file, which indicates we're in recovery.
        If (File.Exists(_dataFile)) Then
            'Recover the existing counts.
            Dim recoveryTable As DataTable = Common.CreateDataTableFromFile(_dataFile)
            For Each column As DataColumn In recoveryTable.Columns
                Dim recoveryValue As Integer = Integer.Parse(recoveryTable.Rows(0)(column.ColumnName))
                Dim recoveryCount As New Count(recoveryValue)
                AddHandler recoveryCount.ValueChanged, AddressOf UpdateDataFile
                _counts.Add(column.ColumnName, recoveryCount)
            Next column
        Else
            'Start all counts at 0.
            For Each header As String In headers
                Dim newCount As New Count(0)
                AddHandler newCount.ValueChanged, AddressOf UpdateDataFile
                _counts.Add(header, newCount)
            Next header
        End If
    End Sub

    ''' <summary>
    ''' Produces an HTML file based on the counts, and saves it to the directory indicated by the filesystem key specified in the constructor.
    ''' </summary>
    Public Sub Publish()
        Publish("Complete")
    End Sub

    Public Sub Publish(ByVal message As String)
        'In case this method gets called more than once per application, use a new file name each time.
        'Slap a counter on the end if needed (i.e., Publish() gets called more than once in a given minute).
        Dim reportNumber As Integer = 1
        Dim htmlFile As String = String.Format("{0}{1} End of Job Report {2:MM-dd-yyyy HH.mm}.html", _publicationDirectory, _reportName, DateTime.Now)
        While File.Exists(htmlFile)
            Dim oldExtension As String = If(reportNumber = 1, ".html", String.Format(".{0}.html", reportNumber))
            reportNumber += 1
            Dim newExtension As String = String.Format(".{0}.html", reportNumber)
            htmlFile = htmlFile.Replace(oldExtension, newExtension)
        End While

        Using htmlWriter As New StreamWriter(htmlFile)
            'Start the HTML file and write out the report headers.
            htmlWriter.WriteLine("<html>")
            htmlWriter.WriteLine("<body>")
            htmlWriter.WriteLine(String.Format("{0}<br />", _reportName))
            htmlWriter.WriteLine("End of Job Report<br />")
            htmlWriter.WriteLine(String.Format("{0}<br />", htmlFile))
            htmlWriter.WriteLine("<br />")

            'See if there's anything to publish.
            If (Not File.Exists(_dataFile)) Then
                htmlWriter.WriteLine(String.Format("{0}. <br />", message))
            Else

                'Get the counts into a DataTable so we can use the ToHtmlLines() extension method.
                Dim countTable As New DataTable()
                countTable.Columns.Add("Item")
                countTable.Columns.Add("Count")
                For Each reportItem As KeyValuePair(Of String, Count) In _counts
                    countTable.Rows.Add(reportItem.Key, reportItem.Value.ToString())
                Next reportItem


                'Get HTML from the DataTable and write it out.
                For Each tableLine In countTable.ToHtmlLines("  ")
                    htmlWriter.WriteLine(tableLine)
                Next tableLine
            End If
            'Close out the HTML.
            htmlWriter.WriteLine("</body>")
            htmlWriter.WriteLine("</html>")
        End Using


        If (File.Exists(_dataFile)) Then
            'Clean up the data file once it's been published, and re-initialize the counts.
            File.Delete(_dataFile)
            Dim headers As New List(Of String)()
            For Each header As String In _counts.Keys
                headers.Add(header)
            Next header
            _counts = New Dictionary(Of String, Count)
            For Each header As String In headers
                Dim newCount As New Count(0)
                AddHandler newCount.ValueChanged, AddressOf UpdateDataFile
                _counts.Add(header, newCount)
            Next header
        End If
    End Sub

    Private Sub UpdateDataFile()
        Using dataWriter As New StreamWriter(_dataFile, False)
            dataWriter.WriteLine(String.Join(",", _counts.Keys.ToArray()))
            dataWriter.WriteLine(String.Join(",", _counts.Values.Select(Function(p) p.ToString()).ToArray()))
        End Using
    End Sub
End Class
