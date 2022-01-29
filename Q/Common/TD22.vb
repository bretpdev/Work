Imports System
Imports System.Collections.Generic
Imports Key = Q.ReflectionInterface.Key
Imports Result = Q.Common.CompassCommentScreenResults

Public Class TD22
    Public Enum RegardsTo
        None
        Borrower
        Endorser
        Reference
        Student
    End Enum

    Private Const MAX_EXPANDED_COMMENT_LENGTH As Integer = 1234
    Public Const NO_DATE As DateTime = #12:00:00 AM#

    Private _ri As ReflectionInterface

    Public Sub New(ByVal ri As ReflectionInterface)
        _ri = ri
    End Sub

    Public Sub New(ByVal rs As Reflection.Session)
        'This class is built using ReflectionInterface's instance methods, since they're much cleaner than the
        'static methods, so instantiate our private ReflectionInterface object using the passed-in Session.
        'Test mode is irrelevant to anything that this class does, so an arbitrary value is used.
        _ri = New ReflectionInterface(rs, False)
    End Sub

#Region "API"
    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans.
    ''' </summary>
    ''' <param name="ssnOrAccountNumber">The borrower's SSN or account number.</param>
    ''' <param name="arc">Action Request Code.</param>
    ''' <param name="comment">Comment text.</param>
    ''' <param name="scriptId">Script ID from Sacker.</param>
    ''' <param name="pauseForManualComments">True to allow the user to add comments.</param>
    ''' <param name="dateFrom">The beginning date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="dateTo">The ending date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="neededByDate">The date at which the resulting queue task becomes critical. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="recipientId">The SSN of the person for whom the action is intended, if not the borrower.</param>
    ''' <param name="inRegardsTo">The entity type that the action request is in regards to. If not applicable, use RegardsTo.None.</param>
    ''' <param name="regardsToId">The SSN of the borrower, if the action is being requested for someone other than the borrower in regards to the borrower.</param>
    Public Function ATD22AllLoans(ByVal ssnOrAccountNumber As String, ByVal arc As String, ByVal comment As String, Optional ByVal scriptId As String = "", Optional ByVal pauseForManualComments As Boolean = False, Optional ByVal dateFrom As DateTime = NO_DATE, Optional ByVal dateTo As DateTime = NO_DATE, Optional ByVal neededByDate As DateTime = NO_DATE, Optional ByVal recipientId As String = "", Optional ByVal inRegardsTo As RegardsTo = RegardsTo.None, Optional ByVal regardsToId As String = "") As Result
        Dim loanSelector As New AllLoansSelector(_ri)
        Return CreateSingleArc(ssnOrAccountNumber, arc, comment, loanSelector, scriptId, pauseForManualComments, dateFrom, dateTo, neededByDate, recipientId, inRegardsTo, regardsToId)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans with a positive balance.
    ''' </summary>
    ''' <param name="ssnOrAccountNumber">The borrower's SSN or account number.</param>
    ''' <param name="arc">Action Request Code.</param>
    ''' <param name="comment">Comment text.</param>
    ''' <param name="scriptId">Script ID from Sacker.</param>
    ''' <param name="pauseForManualComments">True to allow the user to add comments.</param>
    ''' <param name="dateFrom">The beginning date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="dateTo">The ending date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="neededByDate">The date at which the resulting queue task becomes critical. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="recipientId">The SSN of the person for whom the action is intended, if not the borrower.</param>
    ''' <param name="inRegardsTo">The entity type that the action request is in regards to. If not applicable, use RegardsTo.None.</param>
    ''' <param name="regardsToId">The SSN of the borrower, if the action is being requested for someone other than the borrower in regards to the borrower.</param>
    Public Function ATD22ByBalance(ByVal ssnOrAccountNumber As String, ByVal arc As String, ByVal comment As String, Optional ByVal scriptId As String = "", Optional ByVal pauseForManualComments As Boolean = False, Optional ByVal dateFrom As DateTime = NO_DATE, Optional ByVal dateTo As DateTime = NO_DATE, Optional ByVal neededByDate As DateTime = NO_DATE, Optional ByVal recipientId As String = "", Optional ByVal inRegardsTo As RegardsTo = RegardsTo.None, Optional ByVal regardsToId As String = "") As Result
        Dim loanSelector As New ByBalanceSelector(_ri)
        Return CreateSingleArc(ssnOrAccountNumber, arc, comment, loanSelector, scriptId, pauseForManualComments, dateFrom, dateTo, neededByDate, recipientId, inRegardsTo, regardsToId)
    End Function

    Public Shared Function CreateNewTd22(ByVal Ref As ReflectionInterface) As TD22
        Return New TD22(Ref.ReflectionSession)
    End Function


    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans specified.
    ''' </summary>
    ''' <param name="ssnOrAccountNumber">The borrower's SSN or account number.</param>
    ''' <param name="arc">Action Request Code.</param>
    ''' <param name="loanSequenceNumbers">The sequence numbers of the loans that should be selected on TD22.</param>
    ''' <param name="comment">Comment text.</param>
    ''' <param name="scriptId">Script ID from Sacker.</param>
    ''' <param name="pauseForManualComments">True to allow the user to add comments.</param>
    ''' <param name="dateFrom">The beginning date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="dateTo">The ending date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="neededByDate">The date at which the resulting queue task becomes critical. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="recipientId">The SSN of the person for whom the action is intended, if not the borrower.</param>
    ''' <param name="inRegardsTo">The entity type that the action request is in regards to. If not applicable, use RegardsTo.None.</param>
    ''' <param name="regardsToId">The SSN of the borrower, if the action is being requested for someone other than the borrower in regards to the borrower.</param>
    Public Function ATD22ByLoan(ByVal ssnOrAccountNumber As String, ByVal arc As String, ByVal loanSequenceNumbers As IEnumerable(Of Integer), ByVal comment As String, Optional ByVal scriptId As String = "", Optional ByVal pauseForManualComments As Boolean = False, Optional ByVal dateFrom As DateTime = NO_DATE, Optional ByVal dateTo As DateTime = NO_DATE, Optional ByVal neededByDate As DateTime = NO_DATE, Optional ByVal recipientId As String = "", Optional ByVal inRegardsTo As RegardsTo = RegardsTo.None, Optional ByVal regardsToId As String = "") As Result
        Dim loanSelector As New ByLoanSelector(_ri, loanSequenceNumbers)
        Return CreateSingleArc(ssnOrAccountNumber, arc, comment, loanSelector, scriptId, pauseForManualComments, dateFrom, dateTo, neededByDate, recipientId, inRegardsTo, regardsToId)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans belonging to the programs specified.
    ''' </summary>
    ''' <param name="ssnOrAccountNumber">The borrower's SSN or account number.</param>
    ''' <param name="arc">Action Request Code.</param>
    ''' <param name="loanPrograms">The sequence numbers of the loans that should be selected on TD22.</param>
    ''' <param name="comment">Comment text.</param>
    ''' <param name="onlySelectPositiveBalance">True if the selections should be restricted to loans with a positive balance.</param>
    ''' <param name="scriptId">Script ID from Sacker.</param>
    ''' <param name="pauseForManualComments">True to allow the user to add comments.</param>
    ''' <param name="dateFrom">The beginning date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="dateTo">The ending date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="neededByDate">The date at which the resulting queue task becomes critical. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="recipientId">The SSN of the person for whom the action is intended, if not the borrower.</param>
    ''' <param name="inRegardsTo">The entity type that the action request is in regards to. If not applicable, use RegardsTo.None.</param>
    ''' <param name="regardsToId">The SSN of the borrower, if the action is being requested for someone other than the borrower in regards to the borrower.</param>
    Public Function ATD22ByLoanProgram(ByVal ssnOrAccountNumber As String, ByVal arc As String, ByVal loanPrograms As IEnumerable(Of String), ByVal comment As String, Optional ByVal onlySelectPositiveBalance As Boolean = False, Optional ByVal scriptId As String = "", Optional ByVal pauseForManualComments As Boolean = False, Optional ByVal dateFrom As DateTime = NO_DATE, Optional ByVal dateTo As DateTime = NO_DATE, Optional ByVal neededByDate As DateTime = NO_DATE, Optional ByVal recipientId As String = "", Optional ByVal inRegardsTo As RegardsTo = RegardsTo.None, Optional ByVal regardsToId As String = "") As Result
        Dim loanSelector As New ByLoanProgramSelector(_ri, loanPrograms, onlySelectPositiveBalance)
        Return CreateSingleArc(ssnOrAccountNumber, arc, comment, loanSelector, scriptId, pauseForManualComments, dateFrom, dateTo, neededByDate, recipientId, inRegardsTo, regardsToId)
    End Function
#End Region 'API

#Region "Where the work happens"
    Private Function AccessCommentScreen(ByVal ssnOrAccountNumber As String, ByVal arc As String) As Result
        _ri.FastPath("TX3Z/ATD22;")
        _ri.PutText(8, 37, ssnOrAccountNumber)
        _ri.PutText(10, 37, arc)
        _ri.Hit(Key.Enter)
        If _ri.Check4Text(23, 2, "01019 ENTERED KEY NOT FOUND") Then
            Return Result.SSNNotFoundOnSystem
        ElseIf _ri.Check4Text(23, 2, "02695 USER HAS NO AUTHORIZATION TO MAKE ACTION REQUESTS") Then
            Return Result.ARCNotFound
        ElseIf _ri.Check4Text(23, 2, "50108") Then
            Return Result.NoLoansFoundForBorrower
        ElseIf Not _ri.Check4Text(1, 72, "TDX24") Then
            Return Result.NotAbleToAccessCommentScreen
        Else
            Return Result.CommentAddedSuccessfully
        End If
    End Function

    Private Function ApplyScriptId(ByVal comment As String, ByVal scriptId As String) As String
        If Not String.IsNullOrEmpty(scriptId) Then comment += " {" + scriptId + "}"
        Return comment
    End Function

    Private Function CreateArcOnTD22(ByVal ssnOrAccountNumber As String, ByVal arc As String, ByVal comment As String, ByVal loanSelector As Selector, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal neededByDate As DateTime, ByVal recipientId As String, ByVal inRegardsTo As RegardsTo, ByVal regardsToId As String) As Result
        Dim screenResult As Result = AccessCommentScreen(ssnOrAccountNumber, arc)
        If screenResult <> Result.CommentAddedSuccessfully Then Return screenResult

        FillInUpperFields(dateFrom, dateTo, neededByDate, recipientId, inRegardsTo, regardsToId)
        Dim errorFound As Result = loanSelector.SelectLoans()
        If errorFound <> Result.CommentAddedSuccessfully Then
            Return errorFound
        End If

        If EnterComments(comment) Then
            Return Result.CommentAddedSuccessfully
        Else
            Return Result.ErroredDuringPosting
        End If
    End Function

    Private Function CreateMultipleArcs(ByVal ssnOrAccountNumber As String, ByVal arc As String, ByVal comment As String, ByVal loanSelector As Selector, ByVal scriptId As String, ByVal pauseForManualComments As Boolean, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal neededByDate As DateTime, ByVal recipientId As String, ByVal inRegardsTo As RegardsTo, ByVal regardsToId As String) As Result
        Dim formattedScriptId As String = FormatScriptId(scriptId)
        If pauseForManualComments Then
            comment = GetManualComments(comment, formattedScriptId)
        End If
        Dim commentChunks As List(Of String) = GetCommentChunks(comment)
        'Don't pause for manual comments until the last chunk.
        For chunkNumber As Integer = 0 To commentChunks.Count - 2
            Dim td22Result As Result = CreateArcOnTD22(ssnOrAccountNumber, arc, commentChunks(chunkNumber), loanSelector, dateFrom, dateTo, neededByDate, recipientId, inRegardsTo, regardsToId)
            If td22Result <> Common.CompassCommentScreenResults.CommentAddedSuccessfully Then Return td22Result
        Next
        Return CreateArcOnTD22(ssnOrAccountNumber, arc, commentChunks.Last(), loanSelector, dateFrom, dateTo, neededByDate, recipientId, inRegardsTo, regardsToId)
    End Function

    Private Function CreateSingleArc(ByVal ssnOrAccountNumber As String, ByVal arc As String, ByVal comment As String, ByVal loanSelector As Selector, ByVal scriptId As String, ByVal pauseForManualComments As Boolean, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal neededByDate As DateTime, ByVal recipientId As String, ByVal inRegardsTo As RegardsTo, ByVal regardsToId As String) As Result
        Dim formattedScriptId As String = FormatScriptId(scriptId)
        If pauseForManualComments Then
            comment = GetManualComments(comment, formattedScriptId)
        End If
        comment += formattedScriptId
        If comment.Length > MAX_EXPANDED_COMMENT_LENGTH Then Throw New Exception("The requested comment will not fit on ATD22.")
        Return CreateArcOnTD22(ssnOrAccountNumber, arc, comment, loanSelector, dateFrom, dateTo, neededByDate, recipientId, inRegardsTo, regardsToId)
    End Function

    Private Function GetManualComments(ByVal comment As String, ByVal formattedScriptId As String) As String
        Dim com As New UserProvidedComment()
        com.Text = comment
        Using manComments As New ManualComments(com, MAX_EXPANDED_COMMENT_LENGTH - formattedScriptId.Length)
            If manComments.ShowDialog() = DialogResult.OK Then
                Return com.Text
            Else
                Return comment
            End If
        End Using
    End Function

    Private Function FormatScriptId(ByVal scriptId As String) As String
        Dim formattedScriptId As String = String.Empty
        If Not String.IsNullOrEmpty(scriptId) Then
            formattedScriptId = " {" + scriptId + "}"
        End If
        Return formattedScriptId
    End Function

    Private Function EnterComments(ByVal comment As String) As Boolean
        Const PRIMARY_COMMENT_AREA_LENGTH As Integer = 154
        Const EXPANDED_COMMENT_AREA_LINE_LENGTH As Integer = 72
        'NOTE: At the time of writing, the expanded comment screen has 15 comment lines, allowing up to 1080 characters.

        If comment.Length < PRIMARY_COMMENT_AREA_LENGTH Then
            _ri.PutText(21, 2, comment)
            _ri.Hit(Key.Enter)
            'The user may have had to go to the expanded comments screen to enter additional comments,
            'so our check for success will depend on the current screen ID.
            Dim success As Boolean = False
            If _ri.Check4Text(1, 72, "TDX24") Then
                success = _ri.Check4Text(23, 2, "02860 PROCESSING FOR SELECTED ACTION CODES HAS BEEN COMPLETED")
            ElseIf _ri.Check4Text(1, 71, "TDX0O") Then
                success = _ri.Check4Text(23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED")
            End If
            Return success
        Else
            'Fill the first screen.
            _ri.PutText(21, 2, comment.SafeSubstring(0, PRIMARY_COMMENT_AREA_LENGTH), Key.Enter)
            If Not _ri.Check4Text(23, 2, "02860 PROCESSING FOR SELECTED ACTION CODES HAS BEEN COMPLETED") Then Return False
            'Enter the rest on the expanded comments screen.
            _ri.Hit(Key.F4)
            'EnterText() and PutText() don't allow more than 260 characters at a time (due to a session limitation),
            'so loop through conveniently-sized pieces of the comment to enter it all into the screen.
            For segmentStart As Integer = PRIMARY_COMMENT_AREA_LENGTH To comment.Length - 1 Step EXPANDED_COMMENT_AREA_LINE_LENGTH
                _ri.EnterText(comment.SafeSubstring(segmentStart, EXPANDED_COMMENT_AREA_LINE_LENGTH))
            Next segmentStart
            _ri.Hit(Key.Enter)
            Return _ri.Check4Text(23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED")
        End If
    End Function

    ''' <summary>
    ''' Fills in the fields above the comment area on ATD22. Default values will be skipped.
    ''' </summary>
    ''' <remarks>If a field is not writeable for the present ARC, it will be silently ignored.</remarks>
    Private Sub FillInUpperFields(ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal neededByDate As DateTime, ByVal recipientId As String, ByVal inRegardsTo As RegardsTo, ByVal regardsToId As String)
        If dateFrom <> NO_DATE Then
            Try
                _ri.PutText(5, 54, dateFrom.ToString("MMddyy"))
            Catch ex As Exception
            End Try
        End If
        If dateTo <> NO_DATE Then
            Try
                _ri.PutText(5, 67, dateTo.ToString("MMddyy"))
            Catch ex As Exception
            End Try
        End If
        If neededByDate <> NO_DATE Then
            Try
                _ri.PutText(6, 13, neededByDate.ToString("MMddyy"))
            Catch ex As Exception
            End Try
        End If
        If Not String.IsNullOrEmpty(recipientId) Then
            Try
                _ri.PutText(6, 32, recipientId)
            Catch ex As Exception
            End Try
        End If
        If inRegardsTo <> RegardsTo.None Then
            Dim regardsToCode As String = ""
            Select Case inRegardsTo
                Case RegardsTo.Borrower
                    regardsToCode = "B"
                Case RegardsTo.Endorser
                    regardsToCode = "E"
                Case RegardsTo.Reference
                    regardsToCode = "R"
                Case RegardsTo.Student
                    regardsToCode = "S"
            End Select
            Try
                _ri.PutText(7, 19, regardsToCode)
            Catch ex As Exception
            End Try
        End If
        If Not String.IsNullOrEmpty(regardsToId) Then
            Try
                _ri.PutText(7, 36, regardsToId)
            Catch ex As Exception
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Very large comments (>1234 characters) won't fit in TD22, even with the expanded comment screen.
    ''' This function is used by the Multi versions of the TD22 API to break the comment into
    ''' 1234-character chunks that can then be entered into the system using a series of ARCs.
    ''' </summary>
    ''' <param name="comment">Full comment text to be used in ATD22.</param>
    Private Function GetCommentChunks(ByVal comment As String) As List(Of String)
        Dim commentChunks As New List(Of String)()
        commentChunks.Add(comment.SafeSubstring(0, MAX_EXPANDED_COMMENT_LENGTH))
        For nextChunkStart As Integer = MAX_EXPANDED_COMMENT_LENGTH To comment.Length - 1 Step MAX_EXPANDED_COMMENT_LENGTH
            commentChunks.Add(comment.SafeSubstring(nextChunkStart, MAX_EXPANDED_COMMENT_LENGTH))
        Next
        Return commentChunks
    End Function

    Private Sub PromptForManualComments()
        Dim message As String = "You may enter additional comments at this time."
        message += "  Leave this message up while you type directly into the system."
        message += "  Click OK only when you are finished typing comments"
        message += " and are ready for the script to continue."
        MessageBox.Show(message, "Enter Additional Comments", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region 'Where the work happens

#Region "Selector classes"
    ''' <summary>
    ''' Abstract class for selecting loans from ATD22.
    ''' Loan selection criteria are embodied in subclasses.
    ''' </summary>
    Private MustInherit Class Selector
        Protected _ri As ReflectionInterface

        Protected Sub New(ByVal ri As ReflectionInterface)
            _ri = ri
        End Sub

        ''' <summary>
        ''' Selects loans on ATD22 based on the chosen criteria.
        ''' </summary>
        Public Function SelectLoans() As Common.CompassCommentScreenResults
            Dim firstLoanSequence As String = "000"
            Do While Not _ri.Check4Text(11, 5, firstLoanSequence)
                Dim row As Integer = 11
                Do While _ri.Check4Text(row, 3, "_")
                    MarkLoanIfApplicable(row)
                    row += 1
                Loop

                firstLoanSequence = _ri.GetText(11, 5, 3)
                'Page forward.
                _ri.Hit(Key.F8)

                'Clear any loans that the system won't allow to be marked.
                Do While _ri.Check4Text(23, 2, "03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST")
                    _ri.Hit(Key.EndKey)
                    _ri.Hit(Key.F8)
                Loop
            Loop
            If _ri.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") Then
                Return Result.CommentAddedSuccessfully
            ElseIf _ri.Check4Text(23, 2, "90003") Then
                Return Result.RecipientFieldBlank
            Else
                Return Result.ErroredDuringPosting
            End If
        End Function

        Protected Sub MarkLoan(ByVal row As Integer)
            _ri.PutText(row, 3, "X")
        End Sub

        Protected MustOverride Sub MarkLoanIfApplicable(ByVal row As Integer)
    End Class

    ''' <summary>
    ''' Creates a class whose SelectLoans() method will select all loans.
    ''' </summary>
    Private Class AllLoansSelector
        Inherits Selector

        ''' <summary></summary>
        ''' <param name="ri">Interface to the current Reflection session.</param>
        Public Sub New(ByVal ri As ReflectionInterface)
            MyBase.New(ri)
        End Sub

        Protected Overrides Sub MarkLoanIfApplicable(ByVal row As Integer)
            'Always mark the loan.
            MarkLoan(row)
        End Sub
    End Class

    ''' <summary>
    ''' Creates a class whose SelectLoans() method will select loans with a positive balance.
    ''' </summary>
    Private Class ByBalanceSelector
        Inherits Selector

        ''' <summary></summary>
        ''' <param name="ri">Interface to the current Reflection session.</param>
        Public Sub New(ByVal ri As ReflectionInterface)
            MyBase.New(ri)
        End Sub

        Protected Overrides Sub MarkLoanIfApplicable(ByVal row As Integer)
            'Mark this loan if it has a positive balance.
            Dim balance As Double = Double.Parse(_ri.GetText(row, 68, 10))
            If balance > 0 AndAlso Not _ri.Check4Text(row, 78, "-") Then MarkLoan(row)
        End Sub
    End Class

    ''' <summary>
    ''' Creates a class whose SelectLoans() method will select loans found in the passed-in collection.
    ''' </summary>
    Private Class ByLoanSelector
        Inherits Selector

        Private _loanSequences As IEnumerable(Of Integer)

        ''' <summary></summary>
        ''' <param name="ri">Interface to the current Reflection session.</param>
        ''' <param name="loanSequences">Loan sequence numbers to be selected.</param>
        Public Sub New(ByVal ri As ReflectionInterface, ByVal loanSequences As IEnumerable(Of Integer))
            MyBase.New(ri)
            _loanSequences = loanSequences
        End Sub

        Protected Overrides Sub MarkLoanIfApplicable(ByVal row As Integer)
            'Mark this loan if it's in the collection.
            Dim rowLoanSequence As Integer = Integer.Parse(_ri.GetText(row, 5, 3))
            If _loanSequences.Contains(rowLoanSequence) Then MarkLoan(row)
        End Sub
    End Class

    ''' <summary>
    ''' Creates a class whose SelectLoans() method will select loans matching any of the passed-in programs.
    ''' </summary>
    Private Class ByLoanProgramSelector
        Inherits Selector

        Private _loanPrograms As IEnumerable(Of String)
        Private _onlySelectPositiveBalance As Boolean

        ''' <summary></summary>
        ''' <param name="ri">Interface to the current Reflection session.</param>
        ''' <param name="loanPrograms">Loan programs whose loans should be selected.</param>
        Public Sub New(ByVal ri As ReflectionInterface, ByVal loanPrograms As IEnumerable(Of String), ByVal onlySelectPositiveBalance As Boolean)
            MyBase.New(ri)
            _loanPrograms = loanPrograms
            _onlySelectPositiveBalance = onlySelectPositiveBalance
        End Sub

        Protected Overrides Sub MarkLoanIfApplicable(ByVal row As Integer)
            Dim loanProgram As String = _ri.GetText(row, 61, 6)
            If _loanPrograms.Contains(loanProgram) Then
                If _onlySelectPositiveBalance Then
                    Dim balance As Double = Double.Parse(_ri.GetText(row, 68, 10))
                    If balance > 0 AndAlso Not _ri.Check4Text(row, 78, "-") Then MarkLoan(row)
                Else
                    MarkLoan(row)
                End If
            End If
        End Sub
    End Class
#End Region 'Selector classes
End Class
