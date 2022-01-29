Imports System.Collections.Generic
Imports System.Data.Linq
Imports System.Linq
Imports Q

Public Class DataAccess
    Inherits DataAccessBase

    Private Shared _liveDocId As DataContext
    Private Shared _testDocId As DataContext
    Private Shared Function DocIdContext(ByVal testMode As Boolean) As DataContext
        If (testMode) Then
            If (_testDocId Is Nothing) Then
                _testDocId = New DataContext("Data Source=OPSDEV;Initial Catalog=DOCID;Integrated Security=SSPI;")
            End If
            Return _testDocId
        Else
            If (_liveDocId Is Nothing) Then
                _liveDocId = New DataContext("Data Source=NOCHOUSE;Initial Catalog=DOCID;Integrated Security=SSPI;")
            End If
            Return _liveDocId
        End If
    End Function

    Private Shared _testBSYS As DataContext
    Private Shared _liveBSYS As DataContext
    Private Shared Function BSYSContext(ByVal testMode As Boolean) As DataContext
        If (testMode) Then
            _testBSYS = New DataContext("Data Source=OPSDEV;Initial Catalog=BSYS;Integrated Security=SSPI;")
        Else
            If (_liveBSYS Is Nothing) Then
                _liveBSYS = New DataContext("Data Source=NOCHOUSE;Initial Catalog=BSYS;Integrated Security=SSPI;")
            End If
        End If
    End Function

    ''' <summary>
    ''' Retrieves the ARC from the CorrLog table for the given doc ID and doc type,
    ''' or null if the doc ID and doc type are not in the table.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="docId">The 5-character doc ID.</param>
    ''' <param name="docType">The document type description.</param>
    Public Shared Function GetCorrLoggingArc(ByVal testMode As Boolean, ByVal docId As String, ByVal docType As String) As String
        If (String.IsNullOrEmpty(docType)) Then
            'No doc type is passed in if the user types in a doc ID, so find the ARC based on just the doc ID.
            Dim query As String = String.Format("SELECT ARC FROM CorrLog WHERE DocID LIKE '%{0}%'", docId)
            Dim arcs As List(Of String) = DocIdContext(testMode).ExecuteQuery(Of String)(query).ToList()
            Select Case arcs.Count
                Case 0
                    Return Nothing
                Case 1
                    Return arcs.Single()
                Case Else
                    'Some doc IDs have multiple entries, in which case we need to pick the ARC for the "Unspecified" doc type.
                    query += " AND DocType = 'Unspecified'"
                    Return DocIdContext(testMode).ExecuteQuery(Of String)(query).SingleOrDefault()
            End Select
        Else
            'If both doc ID and doc type are specified, be particular about the selection.
            Dim query As String = String.Format("SELECT ARC FROM CorrLog WHERE DocID LIKE '%{0}%' AND DocType = '{1}'", docId, docType)
            Return DocIdContext(testMode).ExecuteQuery(Of String)(query).SingleOrDefault()
        End If
    End Function

    ''' <summary>
    ''' Returns a comma-separated code number and sequence number from the CorrLog table for a given ARC.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="arc">The ARC for which to retrieve a code number and sequence number.</param>
    ''' <remarks>This should only be called with an ARC that exists in the CorrLog table. An exception will be thrown otherwise.</remarks>
    Public Shared Function GetCorrLoggingCodes(ByVal testMode As Boolean, ByVal arc As String) As String
        Dim query As String = String.Format("SELECT TOP 1 CodeNumber + ',' + SeqNum FROM CorrLog WHERE ARC = '{0}'", arc)
        Return DocIdContext(testMode).ExecuteQuery(Of String)(query).Single()
    End Function

    ''' <summary>
    ''' Returns the ARC that is accociated with the DOC ID
    ''' </summary>
    ''' <param name="testMode"></param>
    ''' <param name="docId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetArcFromDocID(ByVal testMode As Boolean, ByVal docId As String) As String
        Dim query As String = String.Format("SELECT ARC FROM LTDB_DAT_DocDetail WHERE DocID = '{0}' AND ARC IS NOT NULL", docId)
        Dim testContext As New DataContext("Data Source=OPSDEV;Initial Catalog=BSYS;Integrated Security=SSPI;")
        Return testContext.ExecuteQuery(Of String)(query).SingleOrDefault()
    End Function
End Class
