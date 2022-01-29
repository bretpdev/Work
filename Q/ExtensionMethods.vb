Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports Uheaa.Common

Public Module ExtensionMethods
    ''' <summary>
    ''' Enumerates the separators that may be used between chunks of numbers in an SSN.
    ''' </summary>
    ''' <remarks>
    ''' Created for the MaskSSNs() extension method, to specify how SSNs should appear after masking them.
    ''' Incidentally, these are also the formats searched for by that method to determine whether a string of numbers is an SSN.
    ''' </remarks>
    Public Enum Separator
        ''' <summary>
        ''' Chunks will not be separated. e.g., 123456789
        ''' </summary>
        None
        ''' <summary>
        ''' Chunks will be separated by a dash (hyphen). e.g., 123-45-6789
        ''' </summary>
        Dash
        ''' <summary>
        ''' Chunks will be separated by a dot. e.g., 123.45.6789
        ''' </summary>
        Dot
        ''' <summary>
        ''' Chunks will be separated by a space. e.g., 123 45 6789
        ''' </summary>
        Space
        ''' <summary>
        ''' Chunks will have the same separator (if any) that they did in the original input string.
        ''' </summary>
        SameAsOriginal
    End Enum

    ''' <summary>
    ''' Finds SSNs within a string and masks them.
    ''' </summary>
    ''' <param name="originalString">The string to search for SSNs.</param>
    ''' <param name="numberOfDigitsToMask">The number of digits (1-9) in the SSN to replace with the mask character. Masking will start at the left.</param>
    ''' <param name="maskCharacter">The character to use for masking the SSN digits.</param>
    ''' <param name="separatorForMaskedSSNs">The separator to use for the masked SSN.</param>
    <Extension()> _
    Public Function MaskSSNs(ByVal originalString As String, ByVal numberOfDigitsToMask As Integer, ByVal maskCharacter As Char, ByVal separatorForMaskedSSNs As Separator) As String
        If (originalString Is Nothing OrElse originalString.Length < 9) Then Return originalString

        Const SSN_REGEX As String = "\d{3}[-\. ]?\d{2}[-\. ]?\d{4}" '3 digits, dash/dot/space (optional), 2 digits, dash/dot/space (optional), 4 digits

        'Use two phases of regular expression matching to get the best results.
        Dim foundSsns As New HashSet(Of String)()
        'Phase 1: Look for SSN patterns that don't have digits before or after them.
        For Each candidate As Match In RegularExpressions.Regex.Matches(originalString, "(^|[^\d])" + SSN_REGEX + "([^\d]|$)")
            'Phase 2: The above matches include the preceding and/or succeeding non-digit (if present),
            'so get the exact SSN out of each match.
            foundSsns.Add(RegularExpressions.Regex.Match(candidate.Value, SSN_REGEX).Value)
        Next candidate

        'Phase 3: Profit!
        Dim ssnDictionary As Dictionary(Of String, String) = GetSsnMasks(foundSsns, numberOfDigitsToMask, maskCharacter, separatorForMaskedSSNs)

        'Replace each instance of found SSNs with its masked version.
        Dim maskedString As String = originalString
        For Each maskPair As KeyValuePair(Of String, String) In ssnDictionary
            maskedString = maskedString.Replace(maskPair.Key, maskPair.Value)
        Next maskPair
        Return maskedString
    End Function

    'Takes a collection of SSNs and returns a dictionary that associates a masked representation with each SSN.
    'Assumes the collection only contains SSNs in the format defined by SSN_REGEX in the MaskSSNs() method.
    Private Function GetSsnMasks(ByVal ssns As IEnumerable(Of String), ByVal numberOfDigitsToMask As Integer, ByVal maskCharacter As Char, ByVal separatorForMaskedSSNs As Separator) As Dictionary(Of String, String)
        Dim requestedSeparator As String = ""
        Select Case separatorForMaskedSSNs
            Case Separator.Dash
                requestedSeparator = "-"
            Case Separator.Dot
                requestedSeparator = "."
            Case Separator.Space
                requestedSeparator = " "
        End Select

        'Set up a dictionary to associate each SSN with its masked version.
        Dim ssnDictionary As New Dictionary(Of String, String)()
        For Each foundSsn As String In New HashSet(Of String)(ssns)
            Dim maskedSsn As New StringBuilder()
            Dim numberOfDigitsMasked As Integer = 0
            Dim maskIndex As Integer = 0

            'Take care of the first three digits.
            For i As Integer = maskIndex To maskIndex + 2
                If (numberOfDigitsMasked < numberOfDigitsToMask) Then
                    'Output a mask character.
                    maskedSsn.Append(maskCharacter)
                    numberOfDigitsMasked += 1
                Else
                    'Output the digit as-is.
                    maskedSsn.Append(foundSsn.Substring(maskIndex, 1))
                End If
                maskIndex += 1
            Next i
            'See if there's a separator.
            Dim detectedSeparator As String = ""
            If (RegularExpressions.Regex.IsMatch(foundSsn.Substring(maskIndex, 1), "[-\. ]")) Then
                'There's a separator. Make note of what it is and advance the chunkIndex past it.
                detectedSeparator = foundSsn.Substring(maskIndex, 1)
                maskIndex += 1
            End If
            'Output the requested separator (which will be the detected separator if SameAsOriginal was requested).
            If (separatorForMaskedSSNs = Separator.SameAsOriginal) Then
                maskedSsn.Append(detectedSeparator)
            Else
                maskedSsn.Append(requestedSeparator)
            End If

            'Now the next two digits.
            For i As Integer = maskIndex To maskIndex + 1
                If (numberOfDigitsMasked < numberOfDigitsToMask) Then
                    maskedSsn.Append(maskCharacter)
                    numberOfDigitsMasked += 1
                Else
                    maskedSsn.Append(foundSsn.Substring(maskIndex, 1))
                End If
                maskIndex += 1
            Next i
            If (RegularExpressions.Regex.IsMatch(foundSsn.Substring(maskIndex, 1), "[-\. ]")) Then
                detectedSeparator = foundSsn.Substring(maskIndex, 1)
                maskIndex += 1
            End If
            If (separatorForMaskedSSNs = Separator.SameAsOriginal) Then
                maskedSsn.Append(detectedSeparator)
            Else
                maskedSsn.Append(requestedSeparator)
            End If

            'And the last four digits.
            For i As Integer = maskIndex To maskIndex + 3
                If (numberOfDigitsMasked < numberOfDigitsToMask) Then
                    maskedSsn.Append(maskCharacter)
                    numberOfDigitsMasked += 1
                Else
                    maskedSsn.Append(foundSsn.Substring(maskIndex, 1))
                End If
                maskIndex += 1
            Next i

            'Add it to the dictionary.
            ssnDictionary.Add(foundSsn, maskedSsn.ToString())
        Next foundSsn
        Return ssnDictionary
    End Function

    ''' <summary>
    ''' Retrieves a substring from this instance.
    ''' The substring starts at a specifiec character position and has up to a specified length.
    ''' </summary>
    ''' <param name="originalString">The instance from which a substring is retrieved.</param>
    ''' <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
    ''' <param name="length">The maximum number of characters in the substring.</param>
    ''' <returns>A string starting at the given index with no more than the given length.</returns>
    <Extension()> _
    Public Function SafeSubstring(ByVal originalString As String, ByVal startIndex As Integer, ByVal length As Integer) As String
        If originalString.Length >= startIndex + length Then
            Return originalString.Substring(startIndex, length)
        Else
            Return originalString.Substring(startIndex)
        End If
    End Function

    ''' <summary>
    ''' Applies a selection filter on a DataTable and returns the results as a new DataTable.
    ''' </summary>
    ''' <param name="originalTable">The DataTable on which to apply the filter.</param>
    ''' <param name="filterExpression">The criteria to use to filter the rows.</param>
    <Extension()> _
    Public Function SelectIntoNewDataTable(ByVal originalTable As DataTable, ByVal filterExpression As String) As DataTable
        'Apply the selection criteria.
        Dim selectedRows As DataRow() = originalTable.Select(filterExpression)
        'If all the rows in the table were selected, just leave the original table as-is.
        If selectedRows.Count() = originalTable.Rows.Count Then
            Return originalTable
        End If

        'Get a copy of the original table so that the schema is preserved.
        Dim newTable As DataTable = originalTable.Copy()
        'The Copy() operation also copies the contents, which we don't want.
        newTable.Clear()
        'Copy the selected rows into the new table.
        For Each selectedRow As DataRow In selectedRows
            Dim newRow As DataRow = newTable.NewRow()
            For i As Integer = 0 To newTable.Columns.Count - 1
                newRow.Item(i) = selectedRow.Item(i)
            Next
            newTable.Rows.Add(newRow)
        Next
        'Return the results.
        Return newTable
    End Function

    ''' <summary>
    ''' Splits a string into fields based on a given delimiter,
    ''' regardless of whether there are quotes around some or all fields.
    ''' </summary>
    ''' <param name="originalString">The string to be split.</param>
    ''' <param name="delimiter">The delimiter on which to split the string.</param>
    ''' <returns>
    ''' List(Of String) with the fields that appeared between the delimiters.
    ''' Quotes and leading/trailing white space are stripped out.
    ''' </returns>
    ''' <remarks>The delimiter cannot contain quotes.</remarks>
    <Extension()> _
    Public Function SplitAgnosticOfQuotes(ByVal originalString As String, ByVal delimiter As String) As List(Of String)
        Return SplitAgnosticOfQuotes(originalString, delimiter, True)
    End Function

    <Extension()> _
    Public Function SplitAgnosticOfQuotes(ByVal originalString As String, ByVal delimiter As String, ByVal trim As Boolean) As List(Of String)
        'Go through the original string and break out pieces separated by delimiters that are not within quotes.
        Dim fields As New List(Of String)
        Dim withinQuotes As Boolean = False
        Dim fieldStart As Integer = 0
        If originalString Is Nothing Then
            Return New List(Of String)
        End If
        For currentIndex As Integer = 0 To originalString.Length - 1
            'Watch for quotation marks.
            If originalString.Substring(currentIndex, 1) = """" Then
                'Toggle the boolean.
                withinQuotes = Not withinQuotes
                Continue For
            End If

            'Watch for delimiters not within quotes.
            If (Not withinQuotes) AndAlso originalString.Substring(currentIndex, 1) = delimiter Then
                If (trim) Then
                    'Add the field to the list, with the quotes stripped out and white space trimmed.
                    fields.Add(originalString.Substring(fieldStart, currentIndex - fieldStart).Replace("""", String.Empty).Trim())
                Else
                    fields.Add(originalString.Substring(fieldStart, currentIndex - fieldStart).Replace("""", String.Empty))
                End If
                'Update the fieldStart variable to the index after the delimiter.
                fieldStart = currentIndex + 1
                Continue For
            End If
        Next currentIndex
        If (trim) Then
            'The last field has no delimiter after it, so it isn't caught by the loop. Add it now.
            fields.Add(originalString.Substring(fieldStart).Replace("""", String.Empty).Trim())
        Else
            fields.Add(originalString.Substring(fieldStart).Replace("""", String.Empty))
        End If

        Return fields
    End Function

    ''' <summary>
    ''' Inserts two "/" into string so string is in a valid date format.  Does not add digits to string/date.
    ''' </summary>
    ''' <param name="stringToFormat"></param>
    ''' <returns>String with two "/" added.</returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToDateFormat(ByVal stringToFormat As String) As String
        Return stringToFormat.Insert(4, "/").Insert(2, "/")
    End Function

    ''' <summary>
    ''' Creates an HTML representation of the DataTable, complete with indents and newlines.
    ''' </summary>
    ''' <param name="indent">The string to use for indenting sub-tags.</param>
    <Extension()> _
    Public Function ToHtmlLines(ByVal table As DataTable, ByVal indent As String) As IEnumerable(Of String)
        Dim htmlLines As New List(Of String)
        'Start the table.
        htmlLines.Add("<table>")
        'Write out the header row from the column names.
        htmlLines.Add(String.Format("{0}<tr>", indent))
        For Each column As DataColumn In table.Columns
            htmlLines.Add(String.Format("{0}{0}<th>{1}</th>", indent, column.ColumnName))
        Next column
        htmlLines.Add(String.Format("{0}</tr>", indent))
        'Write out each data row.
        For Each row As DataRow In table.Rows
            htmlLines.Add(String.Format("{0}<tr>", indent))
            For Each column As DataColumn In table.Columns
                htmlLines.Add(String.Format("{0}{0}<td>{1}</td>", indent, row(column.ColumnName)))
            Next column
            htmlLines.Add(String.Format("{0}</tr>", indent))
        Next row
        'End the table and return the final product.
        htmlLines.Add("</table>")
        Return htmlLines
    End Function

    ''' <summary>
    ''' Inserts two "-" into string so string is formatted like a SSN. Return format ###-##-####.
    ''' </summary>
    ''' <param name="stringToFormat"></param>
    ''' <returns>String with two "-" added.</returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSSNFormat(ByVal stringToFormat As String) As String
        Return stringToFormat.Insert(5, "-").Insert(3, "-")
    End Function

    ''' <summary>
    ''' Writes all arguments as quoted fields in a comma-delimited line.
    ''' </summary>
    ''' <param name="writer">The StreamWriter object that references the file to be written to.</param>
    ''' <param name="fields">A comma-separated list of string values to include in the line.</param>
    <Extension()>
    Public Sub WriteCommaDelimitedLine(ByVal writer As System.IO.StreamWriter, ByVal ParamArray fields() As Object)
        'Build the comma-delimited line's format string based on the number of values in the ParamArray.
        Dim lineBuilder As New System.Text.StringBuilder()
        For i As Integer = 0 To fields.GetUpperBound(0)
            lineBuilder.Append(String.Format(",""{0}{1}{2}""", "{", i.ToString(), "}"))
        Next i

        'Write the line out to the file, stripping off the leading comma.
        writer.WriteLine(String.Format(lineBuilder.ToString(), fields).Substring(1))
    End Sub

    ''' <summary>
    ''' Writes a comma-delimited file from a DataTable's contents.
    ''' </summary>
    ''' <param name="fileName">The full path and file name of the file to be written.</param>
    ''' <param name="writeHeaderRow">True if the file should include a header row.</param>
    <Extension()> _
    Public Sub WriteToFile(ByVal table As DataTable, ByVal fileName As String, ByVal writeHeaderRow As Boolean)
        table.WriteToFile(fileName, writeHeaderRow, New String() {})
    End Sub

    ''' <summary>
    ''' Writes a comma-delimited file from a DataTable's contents.
    ''' </summary>
    ''' <param name="fileName">The full path and file name of the file to be written.</param>
    ''' <param name="writeHeaderRow">True if the file should include a header row.</param>
    ''' <param name="headerFields">The names to be used for the file's header fields.</param>
    <Extension()> _
    Public Sub WriteToFile(ByVal table As DataTable, ByVal fileName As String, ByVal writeHeaderRow As Boolean, ByVal ParamArray headerFields() As String)
        Using fileWriter As New StreamW(fileName, False)
            'If desired, write the header row depending on whether header fields were provided.
            If writeHeaderRow Then
                If headerFields.Count() > 0 Then
                    'Use the field names provided by the client.
                    fileWriter.WriteCommaDelimitedLine(headerFields)
                Else
                    'Use the DataTable's column names.
                    Dim columnNames As New List(Of String)
                    For Each column As DataColumn In table.Columns
                        columnNames.Add(column.ColumnName)
                    Next column
                    fileWriter.WriteCommaDelimitedLine(columnNames.ToArray())
                End If
            End If
            'Write the table rows out to the file.
            For Each row As DataRow In table.Rows
                Dim fields As New List(Of String)
                For i As Integer = 0 To table.Columns.Count - 1
                    fields.Add(row.Item(i))
                Next i
                fileWriter.WriteCommaDelimitedLine(fields.ToArray())
            Next row
            fileWriter.Close()
        End Using
    End Sub

    ''' <summary>
    ''' Writes a collection of objects to a comma-delimited file with the object's properties as a header row.
    ''' The file is written to the directory defined by DataAccessBase.PersonalDataDirectory, and is named after the object's class name.
    ''' </summary>
    ''' <typeparam name="T">The type of the items in the collection.</typeparam>
    ''' <param name="collection">Any collection that implements IEnumerable and contains objects that are simple property holders.</param>
    ''' <returns>The path and name of the file that's created.</returns>
    <Extension()>
    Public Function WriteToFile(Of T)(ByVal collection As IEnumerable(Of T)) As String
        'Determine the object's type and get an array of its properties.
        Dim objectType As Type = GetType(T)
        Dim objectProperties() As PropertyInfo = objectType.GetProperties()

        'Open up the file that we're going to write to.
        Dim fileName As String = String.Format("{0}{1}Data.txt", DataAccessBase.PersonalDataDirectory, objectType.Name)
        Using fileWriter As New StreamW(fileName)
            'Write the object's property names as the header row.
            Dim propertyNames As IEnumerable(Of String) = objectProperties.Select(Function(p) p.Name)
            fileWriter.WriteCommaDelimitedLine(propertyNames.ToArray())

            'Write all the objects in the collection out to the file.
            For Each item As T In collection
                Dim itemProperties As New List(Of String)()
                For Each objectProperty As PropertyInfo In objectProperties
                    Dim itemProperty As Object = objectType.InvokeMember(objectProperty.Name, BindingFlags.GetProperty, Nothing, item, New Object() {})
                    itemProperties.Add(itemProperty.ToString())
                Next objectProperty
                fileWriter.WriteCommaDelimitedLine(itemProperties.ToArray())
            Next item
            fileWriter.Close()
        End Using

        Return fileName
    End Function

    ''' <summary>
    ''' Checks if a string is a valid date or not.
    ''' </summary>
    ''' <param name="stringToCheck"></param>
    ''' <returns>True if valid date else false.</returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function IsValidDate(ByVal stringToCheck As String) As Boolean
        Return IsDate(stringToCheck)
    End Function

    ''' <summary>
    ''' Checks if a string is numeric or not.
    ''' </summary>
    ''' <param name="stringToCheck"></param>
    ''' <returns>True if numeric else false.</returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function IsNumeric(ByVal stringToCheck As String) As Boolean
        Return Microsoft.VisualBasic.IsNumeric(stringToCheck)
    End Function

    ''' <summary>
    ''' Returns a new string in which all occurrences of a specified string in the current string are replaced with another specified string.
    ''' This version is not case-sensitive when looking for oldValue.
    ''' </summary>
    ''' <param name="stringToSearch"></param>
    ''' <param name="oldValue">A string to be replaced (case insensitive).</param>
    ''' <param name="newValue">A string to replace all occurrences of oldValue.</param>
    <Extension()> _
    Public Function ReplaceCaseInsensitive(ByVal stringToSearch As String, ByVal oldValue As String, ByVal newValue As String) As String
        If (String.IsNullOrEmpty(stringToSearch)) Then Return stringToSearch

        Dim i As Integer = 0
        Do While i <= stringToSearch.Length - oldValue.Length
            If (String.Compare(stringToSearch.Substring(i, oldValue.Length), oldValue, True) = 0) Then
                Dim builder As New StringBuilder()
                builder.Append(stringToSearch.Substring(0, i))
                builder.Append(newValue)
                builder.Append(stringToSearch.Substring(i + oldValue.Length))
                stringToSearch = builder.ToString()
            End If
        Loop
        Return stringToSearch
    End Function
End Module
