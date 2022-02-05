Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms
Imports OSSMTP
Imports Reflection
Imports Reflection.Constants
Imports System.Threading.Thread
Imports System.Collections.Generic
Imports System.Reflection


Public Class Common

    Private Shared _userID As String = String.Empty
    Public Shared ReadOnly Property UserID(ByVal rs As Session) As String
        Get
            If _userID = String.Empty Then
                'if user id hasn't been gathered yet then gather
                _userID = GetUserIDFromLP40(rs)
            End If
            Return _userID
        End Get
    End Property

#Region "Enums"

    Public Enum ACSKeyLinePersonType
        Borrower
        Reference
    End Enum

    Public Enum ACSKeyLineAddressType
        Legal
        Alternate
        Temporary
    End Enum

    Public Enum PhoneType
        Home
        Alternate
        Work
    End Enum

    ''' <summary>
    ''' Loan servicing system (COMPASS, LCO, OneLINK) that we use in a Reflection session.
    ''' </summary>
    ''' <remarks>
    ''' Being a power-of-two enum, multiple AES systems can
    ''' be represented in one variable using a bit-wise OR.
    ''' </remarks>
    <FlagsAttribute()> _
    Public Enum AesSystem
        None = 0
        Compass = 1
        Lco = 2
        OneLink = 4
    End Enum

    Public Enum CompassCommentScreenResults
        SSNNotFoundOnSystem
        ARCNotFound
        NoLoansFoundForBorrower
        NotAbleToAccessCommentScreen
        NotAbleToFindOptionToMark
        NotAbleToFindSpaceForAdditionalComments
        ErroredDuringPosting
        CommentAddedSuccessfully
        RecipientFieldBlank
    End Enum

    Public Enum DeleteActivityCommentsResults
        ArcNotFoundOrInactive
        SsnIsNotABorrower
        SsnNotFound
        Success
    End Enum

    Public Enum EmailImportanceLevel
        High
        Low
        Normal
    End Enum

    <FlagsAttribute()> _
    Public Enum LoanStatus
        None = 0
        Closed = 1
        Open = 2
    End Enum

    Public Enum MSOpenMode
        Input = Microsoft.VisualBasic.OpenMode.Input
        Output = Microsoft.VisualBasic.OpenMode.Output
        Append = Microsoft.VisualBasic.OpenMode.Append
        Binary = Microsoft.VisualBasic.OpenMode.Binary
        Random = Microsoft.VisualBasic.OpenMode.Random
    End Enum

    <FlagsAttribute()> _
    Public Enum FileOptions
        None = 0
        ErrorOnEmpty = 1
        ErrorOnMissing = 2
    End Enum

    Public Enum TD22Options
        None = 0
        PauseForUserComments = 1
        OnlySelectLoansWithABalance
    End Enum

#End Region


#Region "CompassFunctions"
#Region "ATD37"
    ''' <summary>
    ''' This function tries to add the comment on TD37 for all loan applications.
    ''' </summary>
    ''' <param name="ri">Instance of ReflectionInterface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Public Shared Function ATD37AllLoans(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return ATD37AllLoans(ri.ReflectionSession, ssn, arc, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' This function tries to add the comment on TD37 for all loan applications.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Public Shared Function ATD37AllLoans(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Dim currentPage As Integer = 0
        Dim currentRow As Integer = 11
        Dim dummy As String = UserID(rs) 'Make sure we hit LP40 before adding comments, if needed.

        While True
            ReflectionInterface.FastPath(rs, "TX3Z/ATD37" & ssn)
            If Not ReflectionInterface.Check4Text(rs, 1, 72, "TDX38") Then
                Return CompassCommentScreenResults.SSNNotFoundOnSystem
            End If
            'find the ARC
            Dim arcLocation As Coordinate = ReflectionInterface.FindText(rs, arc, 8, 8)
            Do While arcLocation Is Nothing
                ReflectionInterface.Hit(rs, ReflectionInterface.Key.F8)
                If ReflectionInterface.Check4Text(rs, 23, 2, "90007 NO MORE DATA TO DISPLAY") Then
                    Return CompassCommentScreenResults.ARCNotFound
                End If
                arcLocation = ReflectionInterface.FindText(rs, arc, 8, 8)
            Loop
            'select the ARC
            ReflectionInterface.PutText(rs, arcLocation.Row, arcLocation.Column - 5, "01", ReflectionInterface.Key.Enter)
            'exit the function if the selection screen is not displayed
            If Not ReflectionInterface.Check4Text(rs, 1, 72, "TDX39") Then
                If ReflectionInterface.Check4Text(rs, 23, 2, "50108") Then
                    Return CompassCommentScreenResults.NoLoansFoundForBorrower
                Else
                    Return CompassCommentScreenResults.NotAbleToAccessCommentScreen
                End If
            End If
            'Go to the appropriate page.
            For i As Integer = 1 To currentPage
                ReflectionInterface.Hit(rs, ReflectionInterface.Key.F8)
                If ReflectionInterface.Check4Text(rs, 23, 2, "90007 NO MORE DATA TO DISPLAY") Then
                    Exit While
                End If
            Next i
            'Check that there's a line to mark on this row.
            If Not ReflectionInterface.Check4Text(rs, currentRow, 18, "_") Then
                Exit While
            End If
            'mark the next app
            ReflectionInterface.PutText(rs, currentRow, 18, "X", ReflectionInterface.Key.Enter)
            If ReflectionInterface.Check4Text(rs, 23, 2, "01490 MUST ENTER AT LEAST ONE SELECTION") Then
                Return CompassCommentScreenResults.NotAbleToFindOptionToMark
            End If
            If ReflectionInterface.Check4Text(rs, 23, 2, "01764 TASK RECORD ALREADY EXISTS") Then
                Return CompassCommentScreenResults.NotAbleToAccessCommentScreen
            End If
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.F4)
            ReflectionInterface.PutText(rs, 8, 5, comment)
            'check if the function should pause for user entry
            If pausePlease Then
                MsgBox("Please enter your own comments and click OK when you are ready to proceed.")
            End If
            'find empty space for further entry
            Dim blankLocation As Coordinate = ReflectionInterface.FindText(rs, "____", 8, 5)
            If blankLocation Is Nothing Then
                Return CompassCommentScreenResults.NotAbleToFindSpaceForAdditionalComments
            Else
                ReflectionInterface.PutText(rs, blankLocation.Row, blankLocation.Column, String.Format("  {0}{1}{2} /{3}", "{", script, "}", UserID(rs)))
            End If
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.Enter)
            If ReflectionInterface.Check4Text(rs, 23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED") = False Then
                Return CompassCommentScreenResults.ErroredDuringPosting
            End If
            currentRow += 1
            'Increase the page count if we've marked a whole page.
            If currentRow > 18 Then
                currentPage += 1
                currentRow = 11
            End If
        End While
        Return CompassCommentScreenResults.CommentAddedSuccessfully
    End Function

    ''' <summary>
    ''' This function tries to add the comment on TD37 for passed in app #'s
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="apps">Array of app numbers</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD37ByLoan(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal apps As List(Of Integer), ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return ATD37ByLoan(ri.ReflectionSession, ssn, arc, comment, apps, script, pausePlease)
    End Function

    ''' <summary>
    ''' This function tries to add the comment on TD37 for passed in app #'s
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="apps">Array of app numbers</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD37ByLoan(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal apps As List(Of Integer), ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Dim row As Integer
        Dim dummy As String = UserID(rs) 'Make sure we hit LP40 before adding comments, if needed.
        ReflectionInterface.FastPath(rs, "TX3Z/ATD37" & ssn)
        If Not ReflectionInterface.Check4Text(rs, 1, 72, "TDX38") Then
            Return CompassCommentScreenResults.SSNNotFoundOnSystem
        End If
        'find the ARC
        Dim arcLocation As Coordinate = ReflectionInterface.FindText(rs, arc, 8, 8)
        Do While arcLocation Is Nothing
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.F8)
            If ReflectionInterface.Check4Text(rs, 23, 2, "90007 NO MORE DATA TO DISPLAY") Then
                Return CompassCommentScreenResults.ARCNotFound
            End If
            arcLocation = ReflectionInterface.FindText(rs, arc, 8, 8)
        Loop
        'select the ARC
        ReflectionInterface.PutText(rs, arcLocation.Row, arcLocation.Column - 5, "01", ReflectionInterface.Key.Enter)
        'exit the function if the selection screen is not displayed
        If Not ReflectionInterface.Check4Text(rs, 1, 72, "TDX39") Then
            Return CompassCommentScreenResults.NotAbleToAccessCommentScreen
        End If
        row = 11
        'mark appropriate apps
        While ReflectionInterface.Check4Text(rs, 23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            For Each app As Integer In apps
                If CInt(ReflectionInterface.GetText(rs, row, 37, 4)) = app Then
                    ReflectionInterface.PutText(rs, row, 18, "X")
                End If
            Next
            row = row + 1
            If ReflectionInterface.Check4Text(rs, row, 18, " ") Then
                ReflectionInterface.Hit(rs, ReflectionInterface.Key.F8)
                row = 11
            End If
        End While
        ReflectionInterface.Hit(rs, ReflectionInterface.Key.Enter)
        If ReflectionInterface.Check4Text(rs, 23, 2, "01490 MUST ENTER AT LEAST ONE SELECTION") Then
            Return CompassCommentScreenResults.NotAbleToFindOptionToMark
        End If
        If ReflectionInterface.Check4Text(rs, 23, 2, "01764 TASK RECORD ALREADY EXISTS") Then
            Return CompassCommentScreenResults.NotAbleToAccessCommentScreen
        End If
        ReflectionInterface.Hit(rs, ReflectionInterface.Key.F4)
        ReflectionInterface.PutText(rs, 8, 5, comment)
        'check if the function should pause for user entry
        If pausePlease Then
            MsgBox("Please enter your own comments and click OK when you are ready to proceed.")
        End If
        'find empty space for further entry
        Dim blankLocation As Coordinate = ReflectionInterface.FindText(rs, "____", 8, 5)
        If blankLocation Is Nothing Then
            Return CompassCommentScreenResults.NotAbleToFindSpaceForAdditionalComments
        Else
            ReflectionInterface.PutText(rs, blankLocation.Row, blankLocation.Column, String.Format("  {0}{1}{2} /{3}", "{", script, "}", UserID(rs)))
        End If
        ReflectionInterface.Hit(rs, ReflectionInterface.Key.Enter)
        If ReflectionInterface.Check4Text(rs, 23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED") = False Then
            Return CompassCommentScreenResults.ErroredDuringPosting
        End If
        Return CompassCommentScreenResults.CommentAddedSuccessfully
    End Function

    ''' <summary>
    ''' this function tries to add the comment on TD37 for the first app listed
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD37FirstLoan(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return ATD37FirstLoan(ri.ReflectionSession, ssn, arc, DateTime.MinValue, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' this function tries to add the comment on TD37 for the first app listed
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD37FirstLoan(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return ATD37FirstLoan(ri.ReflectionSession, ssn, arc, neededBy, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' this function tries to add the comment on TD37 for the first app listed
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD37FirstLoan(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return ATD37FirstLoan(rs, ssn, arc, DateTime.MinValue, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' this function tries to add the comment on TD37 for the first app listed
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Private Shared Function ATD37FirstLoan(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Dim found As Boolean
        Dim dummy As String = UserID(rs) 'Make sure we hit LP40 before adding comments, if needed.

        ATD37FirstLoan = CompassCommentScreenResults.CommentAddedSuccessfully
        ReflectionInterface.FastPath(rs, "TX3Z/ATD37" & ssn)
        If Not ReflectionInterface.Check4Text(rs, 1, 72, "TDX38") Then
            Return CompassCommentScreenResults.SSNNotFoundOnSystem
        End If
        'find the ARC
        Do
            found = rs.FindText(arc, 8, 8)
            If found Then Exit Do
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.F8)
            If ReflectionInterface.Check4Text(rs, 23, 2, "90007 NO MORE DATA TO DISPLAY") Then
                Return CompassCommentScreenResults.ARCNotFound
            End If
        Loop
        'select the ARC
        ReflectionInterface.PutText(rs, rs.FoundTextRow, rs.FoundTextColumn - 5, "01", ReflectionInterface.Key.Enter)
        'exit the function if the selection screen is not displayed
        If Not ReflectionInterface.Check4Text(rs, 1, 72, "TDX39") Then
            Return CompassCommentScreenResults.NotAbleToAccessCommentScreen
        End If
        'Put in the NEEDED BY date if the system allows it.
        If (neededBy <> DateTime.MinValue) Then
            Try
                ReflectionInterface.PutText(rs, 6, 13, neededBy.ToString("MMddyy"))
            Catch ex As Exception
                'Ignore if this ARC doesn't allow us to set the NEEDED BY date.
            End Try
        End If
        'mark the first app
        ReflectionInterface.PutText(rs, 11, 18, "X")
        If ReflectionInterface.Check4Text(rs, 23, 2, "01490 MUST ENTER AT LEAST ONE SELECTION") Then
            Return CompassCommentScreenResults.NotAbleToFindOptionToMark
        End If
        If ReflectionInterface.Check4Text(rs, 23, 2, "01764 TASK RECORD ALREADY EXISTS") Then
            Return CompassCommentScreenResults.NotAbleToAccessCommentScreen
        End If
        'enter short comments
        If Len(comment) < 132 Then
            ReflectionInterface.PutText(rs, 21, 2, String.Format("{0}  {1}{2}{3} /{4}", comment, "{", script, "}", UserID(rs)))
            If pausePlease Then
                MsgBox("Do you want to enter additional comments?  Add them now then click OK if you do else simply click OK now.", MsgBoxStyle.OkOnly + MsgBoxStyle.Question, "Add Additional Comments")
            End If
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.Enter)
            If Not ReflectionInterface.Check4Text(rs, 23, 2, "02860") Then Return CompassCommentScreenResults.ErroredDuringPosting
            'enter long comments
        Else
            'fill the first screen
            ReflectionInterface.PutText(rs, 21, 2, Mid(comment, 1, 154), ReflectionInterface.Key.Enter)
            If Not ReflectionInterface.Check4Text(rs, 23, 2, "02860") Then
                Return CompassCommentScreenResults.ErroredDuringPosting
            End If
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.F4)
            'enter the rest on the expanded comments screen
            For k = 155 To Len(comment) Step 260
                rs.TransmitANSI(Mid(comment, k, 260))
            Next
            rs.TransmitANSI("  {" & script & "} /" & UserID(rs))
            If pausePlease Then
                MsgBox("Do you want to enter additional comments?  Add them now then click OK if you do else simply click OK now.", MsgBoxStyle.OkOnly + MsgBoxStyle.Question, "Add Additional Comments")
            End If
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.Enter)
            If Not ReflectionInterface.Check4Text(rs, 23, 2, "02114") Then Return CompassCommentScreenResults.ErroredDuringPosting
        End If
    End Function
#End Region

#Region "ATD22"
    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pauPls">Pause to allow use to add comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD22AllLoans(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pauPls As Boolean = False) As CompassCommentScreenResults
        Return ATD22AllLoans(ri.ReflectionSession, ssn, arc, TD22.NO_DATE, comment, script, pauPls)
    End Function

    Public Shared Function ATD22AllLoansNoSession(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pauPls As Boolean = False) As CompassCommentScreenResults
        Return ATD22AllLoans(ri, ssn, arc, comment, script, pauPls)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pauPls">Pause to allow use to add comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD22AllLoans(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, ByVal script As String, Optional ByVal pauPls As Boolean = False) As CompassCommentScreenResults
        Return ATD22AllLoans(ri.ReflectionSession, ssn, arc, neededBy, comment, script, pauPls)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow use to add comments</param>
    ''' <param name="dateFrom">The beginning date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="dateTo">The ending date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="recipientId">The SSN of the person for whom the action is intended, if not the borrower.</param>
    ''' <param name="inRegardsTo">The entity type that the action request is in regards to. If not applicable, use RegardsTo.None.</param>
    ''' <param name="regardsToId">The SSN of the borrower, if the action is being requested for someone other than the borrower in regards to the borrower.</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Public Shared Function ATD22AllLoans(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal neededBy As DateTime, ByVal recipientId As String, ByVal inRegardsTo As TD22.RegardsTo, ByVal regardsToId As String, ByVal pauseForManualComments As Boolean) As CompassCommentScreenResults
        Dim compassComments As New TD22(ri)
        Return compassComments.ATD22AllLoans(ssn, arc, comment, scriptId, pauseForManualComments, dateFrom, dateTo, neededBy, recipientId, inRegardsTo, regardsToId)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pauPls">Pause to allow user to add comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Public Shared Function ATD22AllLoans(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pauPls As Boolean = False) As CompassCommentScreenResults
        Return ATD22AllLoans(rs, ssn, arc, TD22.NO_DATE, comment, script, pauPls)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pauPls">Pause to allow user to add comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Private Shared Function ATD22AllLoans(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, ByVal script As String, Optional ByVal pauPls As Boolean = False) As CompassCommentScreenResults
        Dim compassComments As New TD22(rs)
        Return compassComments.ATD22AllLoans(ssn, arc, comment, script, pauPls, TD22.NO_DATE, TD22.NO_DATE, neededBy)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans specified.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">The loan sequence numbers that should be selected on TD22</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments</param>
    Public Shared Function ATD22ByLoan(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As List(Of Integer), ByVal scriptId As String, Optional ByVal pauseForManualComments As Boolean = False) As Boolean
        Return ATD22ByLoan(ri.ReflectionSession, ssn, arc, comment, loanSequenceNumbers, scriptId, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans specified
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">The sequence numbers of the loans that should be selected on TD22.</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow use to add comments</param>
    ''' <param name="dateFrom">The beginning date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="dateTo">The ending date for the item being requested. If not applicable, use TD22.NO_DATE.</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="recipientId">The SSN of the person for whom the action is intended, if not the borrower.</param>
    ''' <param name="inRegardsTo">The entity type that the action request is in regards to. If not applicable, use RegardsTo.None.</param>
    ''' <param name="regardsToId">The SSN of the borrower, if the action is being requested for someone other than the borrower in regards to the borrower.</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Public Shared Function ATD22ByLoan(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As IEnumerable(Of Integer), ByVal scriptId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal neededBy As DateTime, ByVal recipientId As String, ByVal inRegardsTo As TD22.RegardsTo, ByVal regardsToId As String, ByVal pauseForManualComments As Boolean) As CompassCommentScreenResults
        Dim compassComments As New TD22(ri)
        Return compassComments.ATD22ByLoan(ssn, arc, loanSequenceNumbers, comment, scriptId, pauseForManualComments, dateFrom, dateTo, neededBy, recipientId, inRegardsTo, regardsToId)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans specified.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">The loan sequence numbers that should be selected on TD22</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments</param>
    Public Shared Function ATD22ByLoan(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As List(Of Integer), ByVal scriptId As String, Optional ByVal pauseForManualComments As Boolean = False) As Boolean
        Dim compassComments As New TD22(rs)
        Return (compassComments.ATD22ByLoan(ssn, arc, loanSequenceNumbers, comment, scriptId, pauseForManualComments) = CompassCommentScreenResults.CommentAddedSuccessfully)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans with a balance.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments</param>
    Public Shared Function ATD22ByBalance(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptId As String, Optional ByVal pauseForManualComments As Boolean = False) As Boolean
        Return ATD22ByBalance(ri.ReflectionSession, ssn, arc, comment, scriptId, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans with a balance.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments</param>
    Public Shared Function ATD22ByBalance(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptId As String, Optional ByVal pauseForManualComments As Boolean = False) As Boolean
        Dim compassComments As New TD22(rs)
        Return (compassComments.ATD22ByBalance(ssn, arc, comment, scriptId, pauseForManualComments) = CompassCommentScreenResults.CommentAddedSuccessfully)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans in specified loan programs.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="options">Use the OR operator ("|" in C#) to string together multiple options</param>
    ''' <param name="loanPrograms">Loan programs that should be included in the selection</param>
    Public Shared Function ATD22ByLoanProgram(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptId As String, ByVal options As TD22Options, ByVal ParamArray loanPrograms() As String) As Boolean
        Return ATD22ByLoanProgram(ri.ReflectionSession, ssn, arc, comment, scriptId, options, loanPrograms)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans in specified loan programs.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="options">Use the OR operator ("|" in C#) to string together multiple options</param>
    ''' <param name="loanPrograms">Loan programs that should be included in the selection</param>
    Public Shared Function ATD22ByLoanProgram(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptId As String, ByVal options As TD22Options, ByVal ParamArray loanPrograms() As String) As Boolean
        Dim onlySelectLoansWithABalance As Boolean = ((options And TD22Options.OnlySelectLoansWithABalance) = TD22Options.OnlySelectLoansWithABalance)
        Dim pauseForManualComments As Boolean = ((options And TD22Options.PauseForUserComments) = TD22Options.PauseForUserComments)
        Dim compassComments As New TD22(rs)
        Return (compassComments.ATD22ByLoanProgram(ssn, arc, loanPrograms, comment, onlySelectLoansWithABalance, scriptId, pauseForManualComments) = CompassCommentScreenResults.CommentAddedSuccessfully)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD22AllLoansBackedUpWithATD37FirstApp(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        ATD22AllLoansBackedUpWithATD37FirstApp = ATD22AllLoans(ri, ssn, arc, comment, script, pausePlease)
        If ATD22AllLoansBackedUpWithATD37FirstApp <> CompassCommentScreenResults.CommentAddedSuccessfully Then
            ATD22AllLoansBackedUpWithATD37FirstApp = ATD37FirstLoan(ri, ssn, arc, comment, script, pausePlease)
        End If
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD22AllLoansBackedUpWithATD37FirstApp(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        ATD22AllLoansBackedUpWithATD37FirstApp = ATD22AllLoans(ri, ssn, arc, neededBy, comment, script, pausePlease)
        If ATD22AllLoansBackedUpWithATD37FirstApp <> CompassCommentScreenResults.CommentAddedSuccessfully Then
            ATD22AllLoansBackedUpWithATD37FirstApp = ATD37FirstLoan(ri, ssn, arc, neededBy, comment, script, pausePlease)
        End If
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">Loan sequence numbers to be marked on TD22.</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD22ByLoanBackedUpWithATD37FirstApp(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As List(Of Integer), ByVal scriptId As String, Optional ByVal pauseForManualComments As Boolean = False) As CompassCommentScreenResults
        If (ATD22ByLoan(ri, ssn, arc, comment, loanSequenceNumbers, scriptId, pauseForManualComments)) Then
            ATD22ByLoanBackedUpWithATD37FirstApp = CompassCommentScreenResults.CommentAddedSuccessfully
        Else
            ATD22ByLoanBackedUpWithATD37FirstApp = ATD37FirstLoan(ri, ssn, arc, comment, scriptId, pauseForManualComments)
        End If
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD22AllLoansBackedUpWithATD37AllApps(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        ATD22AllLoansBackedUpWithATD37AllApps = ATD22AllLoans(ri, ssn, arc, comment, script, pausePlease)
        If ATD22AllLoansBackedUpWithATD37AllApps <> CompassCommentScreenResults.CommentAddedSuccessfully Then
            ATD22AllLoansBackedUpWithATD37AllApps = ATD37AllLoans(ri, ssn, arc, comment, script, pausePlease)
        End If
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">Loan sequence numbers to be marked on TD22.</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Public Shared Function ATD22ByLoanBackedUpWithATD37AllApps(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As List(Of Integer), ByVal scriptId As String, Optional ByVal pauseForManualComments As Boolean = False) As CompassCommentScreenResults
        If ATD22ByLoan(ri, ssn, arc, comment, loanSequenceNumbers, scriptId, pauseForManualComments) Then
            ATD22ByLoanBackedUpWithATD37AllApps = CompassCommentScreenResults.CommentAddedSuccessfully
        Else
            ATD22ByLoanBackedUpWithATD37AllApps = ATD37AllLoans(ri, ssn, arc, comment, scriptId, pauseForManualComments)
        End If
    End Function
#End Region

#Region "TX1JBorrowerDemographics"
    ''' <summary>
    ''' Get demographic information from screen TX1J.
    ''' </summary>
    ''' <param name="ri">The Reflection Interface</param>
    ''' <param name="ssnOrAcctNum">The SSN or Account Number</param>
    ''' <returns>A populated BorrowerDemograhics object without email information gathered</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDemographicsFromTX1J(ByVal ri As ReflectionInterface, ByVal ssnOrAcctNum As String) As SystemBorrowerDemographics
        Return GetDemographicsFromTX1J(ri.ReflectionSession, ssnOrAcctNum, False)
    End Function

    ''' <summary>
    ''' Get demographic information from screen TX1J.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssnOrAcctNum">The SSN or Account Number</param>
    ''' <returns>A populated BorrowerDemograhics object without email information gathered</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDemographicsFromTX1J(ByVal rs As Session, ByVal ssnOrAcctNum As String) As SystemBorrowerDemographics
        Return GetDemographicsFromTX1J(rs, ssnOrAcctNum, False)
    End Function

    ''' <summary>
    ''' Get demographic information from screen TX1J.
    ''' </summary>
    ''' <param name="ri">The Reflection Interface</param>
    ''' <param name="ssnOrAcctNum">The SSN or Account Number</param>
    ''' <param name="includeEmail">bool value as to whether email should be gathered or not.</param>
    ''' <returns>A populated BorrowerDemograhics object</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDemographicsFromTX1J(ByVal ri As ReflectionInterface, ByVal ssnOrAcctNum As String, ByVal includeEmail As Boolean) As SystemBorrowerDemographics
        Return GetDemographicsFromTX1J(ri.ReflectionSession, ssnOrAcctNum, includeEmail)
    End Function

    ''' <summary>
    ''' Get demographic information from screen TX1J.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssnOrAcctNum">The SSN or Account Number</param>
    ''' <returns>A populated BorrowerDemograhics object</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDemographicsFromTX1J(ByVal rs As Session, ByVal ssnOrAcctNum As String, ByVal includeEmail As Boolean) As SystemBorrowerDemographics
        Dim brwDemographics As New SystemBorrowerDemographics()
        'access TX1J
        If ssnOrAcctNum.Length = 9 Then
            ReflectionInterface.FastPath(rs, "TX3Z/ITX1J;" + ssnOrAcctNum)
        Else
            ReflectionInterface.FastPath(rs, "TX3Z/ITX1J;")
            ReflectionInterface.PutText(rs, 6, 61, ssnOrAcctNum, ReflectionInterface.Key.Enter)
        End If
        'get demographic info if the target is displayed
        If ReflectionInterface.Check4Text(rs, 1, 71, "TXX1R") Then
            brwDemographics = GetDemographics(rs, includeEmail)
        Else
            Throw New DemographicRetrievalException("The given SSN or Account number couldn't be found on the system.  Please contact Systems Support.")
        End If
        Return brwDemographics
    End Function


    ''' <summary>
    ''' Get a list of demographic records from TX1J.
    ''' </summary>
    ''' <param name="ri"></param>
    ''' <param name="ssnOrAcctNum"></param>
    ''' <param name="addressType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetListOfDemographicsFromTX1J(ByVal ri As ReflectionInterface, ByVal ssnOrAcctNum As String, ByVal addressType As ACSKeyLineAddressType, ByVal includeEmail As Boolean) As List(Of SystemBorrowerDemographics)
        Dim brwDemographics As New List(Of SystemBorrowerDemographics)

        With ri
            'access TX1J
            If ssnOrAcctNum.Length = 9 Then
                .FastPath("TX3Z/ITX1J;" + ssnOrAcctNum)
            Else
                .FastPath("TX3Z/ITX1J;")
                .PutText(6, 61, ssnOrAcctNum, ReflectionInterface.Key.Enter)
            End If
            'get demographic info if the target is displayed
            If .Check4Text(1, 71, "TXX1R") Then
                .Hit(ReflectionInterface.Key.F6)
                .Hit(ReflectionInterface.Key.F6)

                Select Case addressType
                    Case ACSKeyLineAddressType.Legal
                        'the legal address is the default so nothing needs to be done
                    Case ACSKeyLineAddressType.Alternate
                        .PutText(16, 14, "B", ReflectionInterface.Key.Enter)
                    Case ACSKeyLineAddressType.Temporary
                        .PutText(16, 14, "D", ReflectionInterface.Key.Enter)
                    Case Else
                        Throw New DemographicRetrievalException("The AddressType enum provided is not valid.")
                End Select
                While Not .Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY")
                    brwDemographics.Add(GetDemographics(ri.ReflectionSession, includeEmail))
                    .Hit(ReflectionInterface.Key.F8)
                End While
            Else
                Throw New DemographicRetrievalException("The given SSN or account number couldn't be found on the system.")
            End If
        End With

        Return brwDemographics
    End Function


    ''' <summary>
    ''' Get demopgraphic data from TX1J by passing in ReflectionInterface instead of Session
    ''' </summary>
    ''' <param name="ri"></param>
    ''' <param name="includeEmail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDemographicData(ByVal ri As ReflectionInterface, ByVal includeEmail As Boolean) As SystemBorrowerDemographics
        Return GetDemographics(ri.ReflectionSession, includeEmail)
    End Function


    ''' <summary>
    ''' Get demographic information from TX1J.
    ''' </summary>
    ''' <param name="rs"></param>
    ''' <param name="includeEmail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDemographics(ByVal rs As Session, ByVal includeEmail As Boolean) As SystemBorrowerDemographics
        Dim brwDemographics As New SystemBorrowerDemographics()

        brwDemographics.SSN = ReflectionInterface.GetText(rs, 3, 12, 11).Replace(" ", "")
        brwDemographics.AccountNumber = ReflectionInterface.GetText(rs, 3, 34, 12)
        brwDemographics.LName = ReflectionInterface.GetText(rs, 4, 6, 23)
        brwDemographics.MI = ReflectionInterface.GetText(rs, 4, 53, 1).Replace("_", "")
        brwDemographics.FName = ReflectionInterface.GetText(rs, 4, 34, 13)
        brwDemographics.Addr1 = ReflectionInterface.GetText(rs, 11, 10, 30)
        brwDemographics.Addr2 = ReflectionInterface.GetText(rs, 12, 10, 30).Trim("_")
        brwDemographics.Addr3 = ReflectionInterface.GetText(rs, 13, 10, 30).Trim("_")
        brwDemographics.City = ReflectionInterface.GetText(rs, 14, 8, 20)


        'get phone info
        Dim brwPhone As New Phone()
        brwPhone = GetPhones(rs)

        'if the country is not blank, get the foreign state (if it is blank, leave the state blank)
        If Not ReflectionInterface.Check4Text(rs, 13, 52, "_") Then
            brwDemographics.State = ReflectionInterface.GetText(rs, 12, 52, 15).Trim("_")
            'use the foreign phone if it is not blank
            ''brwDemographics.Phone = (ReflectionInterface.GetText(rs, 18, 15, 3) + ReflectionInterface.GetText(rs, 18, 24, 5) + ReflectionInterface.GetText(rs, 18, 36, 11) + ReflectionInterface.GetText(rs, 18, 53, 5)).Replace("_", "")
            brwDemographics.Phone = brwPhone.ForeignCountryCode + brwPhone.ForeignCityCode + brwPhone.ForeignLocalNumber
            'use the domestic phone if the foreign phone is blank
            If String.IsNullOrEmpty(brwDemographics.Phone.Length) Then
                ''brwDemographics.Phone = (ReflectionInterface.GetText(rs, 17, 14, 3) + ReflectionInterface.GetText(rs, 17, 23, 3) + ReflectionInterface.GetText(rs, 17, 31, 4)).Replace("_", "")
                brwDemographics.Phone = brwPhone.DomesticAreaCode + brwPhone.DomesticPrefix + brwPhone.DomesticLineNumber
            End If
        Else
            'if the country is blank, get the domestic state and use the domestic phone
            brwDemographics.State = ReflectionInterface.GetText(rs, 14, 32, 2)
            ''brwDemographics.Phone = (ReflectionInterface.GetText(rs, 17, 14, 3) + ReflectionInterface.GetText(rs, 17, 23, 3) + ReflectionInterface.GetText(rs, 17, 31, 4)).Replace("_", "")
            brwDemographics.Phone = brwPhone.DomesticAreaCode + brwPhone.DomesticPrefix + brwPhone.DomesticLineNumber
        End If
        brwDemographics.Zip = ReflectionInterface.GetText(rs, 14, 40, 17)
        If brwDemographics.Zip.Length = 9 Then brwDemographics.Zip = brwDemographics.Zip.Substring(0, 5) + "-" + brwDemographics.Zip.Substring(5, 4)
        brwDemographics.Country = ReflectionInterface.GetText(rs, 13, 52, 25).Trim("_")
        brwDemographics.AddrValidityIndicator = ReflectionInterface.GetText(rs, 11, 55, 1)

        'new for block address phone
        DateTime.TryParse(ReflectionInterface.GetText(rs, 10, 32, 8).Replace(" ", "/"), brwDemographics.AddrValidityDate)

        brwDemographics.DOB = ReflectionInterface.GetText(rs, 20, 6, 10).Replace(" ", "/")
        If includeEmail Then
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.F2)
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.F10)
            brwDemographics.EmailValidityIndicator = ReflectionInterface.GetText(rs, 12, 14, 1)
            DateTime.TryParse(ReflectionInterface.GetText(rs, 11, 17, 8).Replace(" ", "/"), brwDemographics.EmailValidityDate)
            If Not ReflectionInterface.Check4Text(rs, 14, 10, "___") Then brwDemographics.Email = ReflectionInterface.GetText(rs, 14, 10, 60).Trim()
            If Not ReflectionInterface.Check4Text(rs, 15, 10, "___") Then brwDemographics.Email = brwDemographics.Email + ReflectionInterface.GetText(rs, 15, 10, 60).Trim()
            If Not ReflectionInterface.Check4Text(rs, 16, 10, "___") Then brwDemographics.Email = brwDemographics.Email + ReflectionInterface.GetText(rs, 16, 10, 60).Trim()
            If Not ReflectionInterface.Check4Text(rs, 17, 10, "___") Then brwDemographics.Email = brwDemographics.Email + ReflectionInterface.GetText(rs, 17, 10, 60).Trim()
            If Not ReflectionInterface.Check4Text(rs, 18, 10, "___") Then brwDemographics.Email = brwDemographics.Email + ReflectionInterface.GetText(rs, 18, 10, 60).Trim()
        End If

        Return brwDemographics
    End Function


    ''' <summary>
    ''' Get a list of phone number records from TX1J.
    ''' </summary>
    ''' <param name="ri"></param>
    ''' <param name="ssnOrAcctNum"></param>
    ''' <param name="phoneType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetListOfPhonesFromTX1J(ByVal ri As ReflectionInterface, ByVal ssnOrAcctNum As String, ByVal phoneType As PhoneType) As List(Of Phone)
        Dim brwPhones As New List(Of Phone)

        With ri
            'access TX1J
            If ssnOrAcctNum.Length = 9 Then
                .FastPath("TX3Z/ITX1J;" + ssnOrAcctNum)
            Else
                .FastPath("TX3Z/ITX1J;")
                .PutText(6, 61, ssnOrAcctNum, ReflectionInterface.Key.Enter)
            End If
            'get demographic info if the target is displayed
            If .Check4Text(1, 71, "TXX1R") Then
                .Hit(ReflectionInterface.Key.F6)
                .Hit(ReflectionInterface.Key.F6)
                .Hit(ReflectionInterface.Key.F6)
                Select Case phoneType
                    Case phoneType.Home
                        .PutText(16, 14, "H", ReflectionInterface.Key.Enter)
                    Case phoneType.Alternate
                        .PutText(16, 14, "A", ReflectionInterface.Key.Enter)
                    Case phoneType.Work
                        .PutText(16, 14, "W", ReflectionInterface.Key.Enter)
                    Case Else
                        Throw New DemographicRetrievalException("The PhoneType enum provided is not valid.")
                End Select
                While Not .Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") _
                        And Not .Check4Text(23, 2, "01103 PHONE TYPE DOES NOT CURRENTLY EXIST") _
                        And Not .Check4Text(23, 2, "01105 PHONE TYPE DOES NOT CURRENTLY EXIST")
                    brwPhones.Add(GetPhones(ri.ReflectionSession))
                    .Hit(ReflectionInterface.Key.F8)
                End While
            Else
                Throw New DemographicRetrievalException("The given SSN or account number couldn't be found on the system.")
            End If
        End With

        Return brwPhones
    End Function

    ''' <summary>
    ''' Get phone data data from TX1J by passing in ReflectionInterface instead of Session
    ''' </summary>
    ''' <param name="ri"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPhoneData(ByVal ri As ReflectionInterface) As Phone
        Return GetPhones(ri.ReflectionSession)
    End Function


    ''' <summary>
    ''' Get phone data from TX1J.
    ''' </summary>
    ''' <param name="rs"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPhones(ByVal rs As Session) As Phone
        Dim brwPhone As New Phone()

        brwPhone.PhoneType = ReflectionInterface.GetText(rs, 16, 14, 1)
        brwPhone.MBLIndicator = ReflectionInterface.GetText(rs, 16, 20, 1)
        brwPhone.ConsentIndicator = ReflectionInterface.GetText(rs, 16, 30, 1)
        DateTime.TryParse(ReflectionInterface.GetText(rs, 16, 45, 8).Replace(" ", "/"), brwPhone.VerifiedDate)
        brwPhone.DomesticAreaCode = ReflectionInterface.GetText(rs, 17, 14, 3).Replace("_", "")
        brwPhone.DomesticPrefix = ReflectionInterface.GetText(rs, 17, 23, 3).Replace("_", "")
        brwPhone.DomesticLineNumber = ReflectionInterface.GetText(rs, 17, 31, 4).Replace("_", "")
        brwPhone.ValidityIndicator = ReflectionInterface.GetText(rs, 17, 54, 1)
        brwPhone.ForeignCountryCode = ReflectionInterface.GetText(rs, 18, 15, 3).Replace("_", "")
        brwPhone.ForeignCityCode = ReflectionInterface.GetText(rs, 18, 24, 5).Replace("_", "")
        brwPhone.ForeignLocalNumber = ReflectionInterface.GetText(rs, 18, 36, 11).Replace("_", "")

        If Not ReflectionInterface.Check4Text(rs, 17, 14, "_") Then
            brwPhone.Extension = ReflectionInterface.GetText(rs, 17, 40, 5).Replace("_", "")
        Else
            brwPhone.Extension = ReflectionInterface.GetText(rs, 18, 53, 5).Replace("_", "")
        End If

        Return brwPhone
    End Function
#End Region

#Region "TX0YSchoolDemographics"
    ''' <summary>
    ''' Gets school demographic information from TX0Y
    ''' </summary>
    ''' <param name="ri">The Reflection Interface</param>
    ''' <param name="SchCode">School Code</param>
    ''' <param name="Dept">The department code to target.  Before the C# conversion this was defualted to blank string.</param>
    ''' <returns>A populated School Demographics object</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSchoolDemographicsFromTX0Y(ByVal ri As ReflectionInterface, ByVal schCode As String, ByVal dept As String) As SchoolDemographics
        Return GetSchoolDemographicsFromTX0Y(ri.ReflectionSession, schCode, dept)
    End Function

    ''' <summary>
    ''' Gets school demographic information from TX0Y
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="SchCode">School Code</param>
    ''' <param name="Dept">The department code to target.  Before the C# conversion this was defualted to blank string.</param>
    ''' <returns>A populated School Demographics object</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSchoolDemographicsFromTX0Y(ByVal rs As Session, ByVal schCode As String, ByVal dept As String) As SchoolDemographics
        Dim Row As Integer
        Dim SchDemographics As New SchoolDemographics
        'access TX0Y
        ReflectionInterface.FastPath(rs, "TX3Z/ITX0Y" & schCode)
        'exit the function if the school code  is not found
        If ReflectionInterface.Check4Text(rs, 1, 73, "TXX00") Then
            Throw New DemographicRetrievalException("The school information couldn't be found.  Please contact Systems Support")
        End If
        'select the department indicated by the SchDpt variable of the general dept (000) if the SchDpt is not found
        If ReflectionInterface.Check4Text(rs, 1, 72, "TXX04") Then
            Row = 9
            Do Until ReflectionInterface.GetText(rs, Row, 5, 3) = dept
                Row = Row + 1
                If ReflectionInterface.Check4Text(rs, Row, 5, "   ") Then
                    ReflectionInterface.Hit(rs, ReflectionInterface.Key.F8)
                    If ReflectionInterface.Check4Text(rs, 23, 2, "90007") Then
                        SchDemographics.Department = "000"
                        ReflectionInterface.FastPath(rs, "TX3Z/ITX0Y" & schCode)
                    End If
                    Row = 9
                End If
            Loop
            ReflectionInterface.PutText(rs, 22, 18, ReflectionInterface.GetText(rs, Row, 2, 2), ReflectionInterface.Key.Enter)
        End If
        'get the school information
        SchDemographics.Name = ReflectionInterface.GetText(rs, 6, 19, 40)
        SchDemographics.Addr1 = ReflectionInterface.GetText(rs, 9, 23, 40)
        If Not ReflectionInterface.Check4Text(rs, 10, 23, "_") Then SchDemographics.Addr2 = ReflectionInterface.GetText(rs, 10, 23, 30) Else SchDemographics.Addr2 = String.Empty
        If Not ReflectionInterface.Check4Text(rs, 11, 23, "_") Then SchDemographics.Addr3 = ReflectionInterface.GetText(rs, 11, 23, 30) Else SchDemographics.Addr3 = String.Empty
        SchDemographics.City = ReflectionInterface.GetText(rs, 12, 23, 40)
        SchDemographics.State = ReflectionInterface.GetText(rs, 13, 23, 2)
        SchDemographics.Zip = ReflectionInterface.GetText(rs, 13, 46, 9)
        If Len(SchDemographics.Zip) > 5 Then SchDemographics.Zip = SchDemographics.Zip.Insert(5, "-")
        Return SchDemographics
    End Function
#End Region
#End Region


#Region "LCOFunctions"
#Region "TPDDBorrowerDemographics"

    ''' <summary>
    ''' Get demographic information from LCO screen TPDD.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">The Borrower's SSN</param>
    ''' <param name="promptForLogin">True to show a prompt for the user to log into LCO.</param>
    ''' <returns>A BorrowerDemographics object populated with the information on TPDD, or null if the borrower isn't in LCO.</returns>
    Public Shared Function GetDemographicsFromTPDD(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal promptForLogin As Boolean) As SystemBorrowerDemographics
        Return GetDemographicsFromTPDD(ri.ReflectionSession, ssn, promptForLogin)
    End Function

    ''' <summary>
    ''' Get demographic information from LCO screen TPDD.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">The Borrower's SSN</param>
    ''' <param name="promptForLogin">True to show a prompt for the user to log into LCO.</param>
    ''' <returns>A BorrowerDemographics object populated with the information on TPDD, or null if the borrower isn't in LCO.</returns>
    Public Shared Function GetDemographicsFromTPDD(ByVal rs As Session, ByVal ssn As String, ByVal promptForLogin As Boolean, ByVal testMode As Boolean) As SystemBorrowerDemographics
        If promptForLogin AndAlso testMode Then
            MessageBox.Show("Please login to LCO test if applicable, and click OK when done.", "Login to LCO", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End If
        Dim borrower As SystemBorrowerDemographics = Nothing
        'Access TPDD.
        ReflectionInterface.FastPath(rs, "TPDD" + ssn)
        'Get demographic info if the target screen is displayed.
        If ReflectionInterface.Check4Text(rs, 1, 19, "LCO PERSONAL INFORMATION DISPLAY") Then
            borrower = New BorrowerDemographics()
            borrower.AccountNumber = ReflectionInterface.GetText(rs, 3, 62, 12).Replace("-", "")
            borrower.LName = ReflectionInterface.GetText(rs, 4, 35, 26)
            If ReflectionInterface.Check4Text(rs, 4, 33, "_") Then
                borrower.MI = String.Empty
            Else
                borrower.MI = ReflectionInterface.GetText(rs, 4, 33, 1)
            End If
            borrower.FName = ReflectionInterface.GetText(rs, 4, 19, 13)
            borrower.Addr1 = ReflectionInterface.GetText(rs, 6, 19, 31)
            If ReflectionInterface.Check4Text(rs, 7, 19, "_") Then
                borrower.Addr2 = String.Empty
            Else
                borrower.Addr2 = ReflectionInterface.GetText(rs, 7, 19, 31)
            End If
            If ReflectionInterface.Check4Text(rs, 8, 19, "_") Then
                borrower.Addr3 = String.Empty
            Else
                borrower.Addr3 = ReflectionInterface.GetText(rs, 8, 19, 31)
            End If
            borrower.City = ReflectionInterface.GetText(rs, 9, 19, 23)
            If ReflectionInterface.Check4Text(rs, 9, 50, "_") Then
                'Foreign state
                borrower.State = ReflectionInterface.GetText(rs, 10, 19, 14)
            Else
                'US state
                borrower.State = ReflectionInterface.GetText(rs, 9, 50, 2)
            End If
            If ReflectionInterface.Check4Text(rs, 10, 55, "_") Then
                borrower.Country = String.Empty
            Else
                borrower.Country = ReflectionInterface.GetText(rs, 10, 55, 14)
            End If
            borrower.Zip = ReflectionInterface.GetText(rs, 9, 66, 10).Replace(" ", "").Replace("_", "")
            borrower.DOB = ReflectionInterface.GetText(rs, 5, 61, 10).Replace(" ", "/")
            borrower.Phone = ReflectionInterface.GetText(rs, 13, 13, 12).Replace(" ", "")
        End If
        If promptForLogin AndAlso testMode Then
            MessageBox.Show("Please log off LCO test if applicable, and click OK when done.", "Log off LCO", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End If
        Return borrower
    End Function


#End Region

#Region "UPUR"
    ''' <summary>
    ''' Enters a comment in LCO.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pursuitCode">The code for the PCODE field</param>
    Public Shared Sub AddCommentsInUPUR(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal comment As String, ByVal pursuitCode As String)
        AddCommentsInUPUR(ri.ReflectionSession, ssn, comment, pursuitCode)
    End Sub

    ''' <summary>
    ''' Enters a comment in LCO.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pursuitCode">The code for the PCODE field</param>
    Public Shared Sub AddCommentsInUPUR(ByVal rs As Session, ByVal ssn As String, ByVal comment As String, ByVal pursuitCode As String)
        Const COMMENT_AREA_LENGTH As Integer = 260
        Dim dummy As String = UserID(rs) 'Make sure we hit LP40 before adding comments, if needed.

        'Loop over segments of the comment as large as will fit in the comment area.
        'Smaller comments will only go through the loop once.
        For segmentStart As Integer = 0 To comment.Length Step COMMENT_AREA_LENGTH
            ReflectionInterface.FastPath(rs, "UPUR" + ssn)
            'Page up until the addition screen is found.
            Do While Not ReflectionInterface.Check4Text(rs, 1, 21, "** PURSUIT ACTIVITY - RECORD ADDITION **")
                ReflectionInterface.Hit(rs, ReflectionInterface.Key.Up)
            Loop
            'Enter the user ID, pursuit code, and current segment of the comment.
            ReflectionInterface.PutText(rs, 3, 29, UserID(rs).Substring(4, 3))
            ReflectionInterface.PutText(rs, 6, 29, pursuitCode)
            ReflectionInterface.PutText(rs, 6, 35, comment.SafeSubstring(segmentStart, COMMENT_AREA_LENGTH), ReflectionInterface.Key.Enter)
        Next segmentStart
    End Sub
#End Region
#End Region


#Region "OneLinkFunctions"
#Region "LG02"
    ''' <summary>
    ''' Get loan information from LG02.
    ''' You must be on a LG02 target screen before calling this method.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <returns>
    ''' An LG02LoanInfo object populated with the pertinent information from LG02,
    ''' or null if the current screen is not a recognized LG02 target screen.
    ''' </returns>
    Public Shared Function GetLoanInfoFromLG02(ByVal ri As ReflectionInterface) As LG02LoanInfo
        Return GetLoanInfoFromLG02(ri.ReflectionSession)
    End Function

    ''' <summary>
    ''' Get loan information from LG02.
    ''' You must be on a LG02 target screen before calling this method.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <returns>
    ''' An LG02LoanInfo object populated with the pertinent information from LG02,
    ''' or null if the current screen is not a recognized LG02 target screen.
    ''' </returns>
    Public Shared Function GetLoanInfoFromLG02(ByVal rs As Session) As LG02LoanInfo
        If ReflectionInterface.Check4Text(rs, 1, 56, "CONSOLIDATION APPLICATION") Then
            Return New LG02LoanInfo With { _
                .Cluid = ReflectionInterface.GetText(rs, 5, 31, 19), _
                .LoanHolderCode = ReflectionInterface.GetText(rs, 11, 46, 6), _
                .LoanServicerCode = ReflectionInterface.GetText(rs, 11, 71, 6), _
                .LoanType = "CL", _
                .StudentName = String.Empty, _
                .StudentSsn = String.Empty _
            }
        ElseIf ReflectionInterface.Check4Text(rs, 1, 60, "PLUS LOAN APPLICATION") Then
            Dim loanInfo As New LG02LoanInfo With { _
                .LoanType = "PL", _
                .StudentName = ReflectionInterface.GetText(rs, 9, 8, 9), _
                .StudentSsn = ReflectionInterface.GetText(rs, 9, 18, 30) _
            }
            If ReflectionInterface.Check4Text(rs, 5, 62, "DT ") Then
                'Common
                loanInfo.Cluid = ReflectionInterface.GetText(rs, 2, 35, 19)
                loanInfo.LoanHolderCode = ReflectionInterface.GetText(rs, 18, 42, 6)
                loanInfo.LoanServicerCode = ReflectionInterface.GetText(rs, 18, 64, 6)
            ElseIf ReflectionInterface.Check4Text(rs, 5, 62, "BWR") Then
                'Pre-common
                loanInfo.Cluid = ReflectionInterface.GetText(rs, 2, 33, 19)
                loanInfo.LoanHolderCode = ReflectionInterface.GetText(rs, 19, 44, 6)
                loanInfo.LoanServicerCode = ReflectionInterface.GetText(rs, 19, 73, 6)
            Else
                MessageBox.Show("The LG02 screen displayed is not recognized.  Contact Systems Support for assistance.", "Screen Not Recognized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return Nothing
            End If
            Return loanInfo
        ElseIf ReflectionInterface.Check4Text(rs, 1, 60, "PLUS MASTER PROM NOTE") Then
            Dim loanInfo As New LG02LoanInfo With { _
                .Cluid = ReflectionInterface.GetText(rs, 3, 30, 19), _
                .LoanHolderCode = ReflectionInterface.GetText(rs, 18, 41, 6), _
                .LoanServicerCode = ReflectionInterface.GetText(rs, 18, 64, 6) _
            }
            If ReflectionInterface.Check4Text(rs, 10, 4, "PLUS GRADUATE LOAN") Then
                loanInfo.StudentName = String.Empty
                loanInfo.StudentSsn = String.Empty
                loanInfo.LoanType = "GB"
            ElseIf ReflectionInterface.Check4Text(rs, 10, 4, "STD") Then
                loanInfo.StudentSsn = ReflectionInterface.GetText(rs, 10, 8, 9)
                loanInfo.StudentName = ReflectionInterface.GetText(rs, 10, 18, 30)
                loanInfo.LoanType = "PL"
            Else
                MessageBox.Show("The LG02 screen displayed is not recognized.  Contact Systems Support for assistance.", "Screen Not Recognized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return Nothing
            End If
            Return loanInfo
        ElseIf ReflectionInterface.Check4Text(rs, 1, 61, "SLS LOAN APPLICATION") Then
            Return New LG02LoanInfo With { _
                .Cluid = ReflectionInterface.GetText(rs, 2, 33, 19), _
                .LoanHolderCode = ReflectionInterface.GetText(rs, 19, 44, 6), _
                .LoanServicerCode = ReflectionInterface.GetText(rs, 19, 73, 6), _
                .LoanType = "SL", _
                .StudentName = String.Empty, _
                .StudentSsn = String.Empty _
            }
        ElseIf ReflectionInterface.Check4Text(rs, 1, 56, "STAFFORD MASTER PROM NOTE") Then
            Dim loanInfo As New LG02LoanInfo With { _
                .Cluid = ReflectionInterface.GetText(rs, 3, 32, 19), _
                .LoanHolderCode = ReflectionInterface.GetText(rs, 18, 49, 6), _
                .LoanServicerCode = ReflectionInterface.GetText(rs, 18, 73, 6), _
                .StudentName = String.Empty, _
                .StudentSsn = String.Empty _
            }
            If ReflectionInterface.Check4Text(rs, 4, 59, "SUB") Then
                loanInfo.LoanType = "SF"
            Else
                loanInfo.LoanType = "SU"
            End If
            Return loanInfo
        ElseIf ReflectionInterface.Check4Text(rs, 1, 56, "STAFFORD LOAN APPLICATION") Then
            Dim loanInfo As New LG02LoanInfo With { _
                .Cluid = ReflectionInterface.GetText(rs, 3, 33, 19), _
                .LoanHolderCode = ReflectionInterface.GetText(rs, 18, 49, 6), _
                .LoanServicerCode = ReflectionInterface.GetText(rs, 18, 73, 6), _
                .StudentName = String.Empty, _
                .StudentSsn = String.Empty _
            }
            If ReflectionInterface.Check4Text(rs, 4, 59, "SUB") Then
                loanInfo.LoanType = "SF"
            Else
                loanInfo.LoanType = "SU"
            End If
            Return loanInfo
        Else
            MessageBox.Show("The LG02 screen displayed is not recognized.  Contact Systems Support for assistance.", "Screen Not Recognized", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If
    End Function

#End Region

#Region "LP22BorrowerDemographics"

    ''' <summary>
    ''' Get demographic information from screen LP22.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssnOrAcctNum">Borrower SSN or Account Number</param>
    ''' <returns>Populated BorrowerDemographics Object</returns>
    Public Shared Function GetDemographicsFromLP22(ByVal ri As ReflectionInterface, ByVal ssnOrAcctNum As String) As SystemBorrowerDemographics
        Return GetDemographicsFromLP22(ri.ReflectionSession, ssnOrAcctNum)
    End Function

    ''' <summary>
    ''' Get demographic information from screen LP22.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssnOrAcctNum">Borrower SSN or Account Number</param>
    ''' <returns>Populated BorrowerDemographics Object</returns>
    Public Shared Function GetDemographicsFromLP22(ByVal rs As Session, ByVal ssnOrAcctNum As String) As SystemBorrowerDemographics
        'Access LP22.
        If ssnOrAcctNum.Length = 10 Then
            ReflectionInterface.FastPath(rs, "LP22I;;;;L;;" & ssnOrAcctNum)
        ElseIf ssnOrAcctNum.Length = 9 Then
            ReflectionInterface.FastPath(rs, "LP22I" & ssnOrAcctNum)
        Else
            Throw New DemographicRetrievalException("A valid 10 digit Account Number or 9 digit Social Security Number wasn't provided.")
        End If
        'Check that the LP22 target screen is displayed.
        If Not ReflectionInterface.Check4Text(rs, 1, 62, "PERSON DEMOGRAPHICS") Then
            Throw New DemographicRetrievalException("A valid 10 digit Account Number or 9 digit Social Security Number wasn't provided.")
        End If

        'Define, populate, and return a BorrowerDemographics object.
        Dim brwDemographics As New SystemBorrowerDemographics()
        brwDemographics.SSN = ReflectionInterface.GetText(rs, 3, 23, 9)
        brwDemographics.AccountNumber = ReflectionInterface.GetText(rs, 3, 60, 12)
        brwDemographics.LName = ReflectionInterface.GetText(rs, 4, 5, 35)
        brwDemographics.MI = ReflectionInterface.GetText(rs, 4, 60, 1)
        brwDemographics.FName = ReflectionInterface.GetText(rs, 4, 44, 12)
        brwDemographics.Addr1 = ReflectionInterface.GetText(rs, 10, 9, 35)
        brwDemographics.Addr2 = ReflectionInterface.GetText(rs, 11, 9, 35)
        brwDemographics.City = ReflectionInterface.GetText(rs, 12, 9, 35)
        brwDemographics.State = ReflectionInterface.GetText(rs, 12, 52, 2)
        brwDemographics.Country = ReflectionInterface.GetText(rs, 11, 55, 25)
        brwDemographics.Zip = ReflectionInterface.GetText(rs, 12, 60, 5) + ReflectionInterface.GetText(rs, 12, 65, 4)
        brwDemographics.AddrValidityIndicator = ReflectionInterface.GetText(rs, 10, 57, 1)
        Dim validityMonth As Integer
        Dim validityDay As Integer
        Dim validityYear As Integer
        If Not ReflectionInterface.Check4Text(rs, 10, 72, "MMDDCCYY") Then
            validityMonth = Integer.Parse(ReflectionInterface.GetText(rs, 10, 72, 2))
            validityDay = Integer.Parse(ReflectionInterface.GetText(rs, 10, 74, 2))
            validityYear = Integer.Parse(ReflectionInterface.GetText(rs, 10, 76, 4))
            brwDemographics.AddrValidityDate = New Date(validityYear, validityMonth, validityDay)
        End If
        If Not ReflectionInterface.Check4Text(rs, 4, 72, "MMDDCCYY") Then
            brwDemographics.DOB = ReflectionInterface.GetText(rs, 4, 72, 8).ToDateFormat()
        End If
        brwDemographics.Phone = ReflectionInterface.GetText(rs, 13, 12, 10)
        brwDemographics.PhoneValidityIndicator = ReflectionInterface.GetText(rs, 13, 38, 1)
        brwDemographics.AltPhone = ReflectionInterface.GetText(rs, 14, 12, 10)
        brwDemographics.AltPhoneValidityIndicator = ReflectionInterface.GetText(rs, 14, 38, 1)
        If Not ReflectionInterface.Check4Text(rs, 13, 44, "MMDDCCYY") Then
            validityMonth = Integer.Parse(ReflectionInterface.GetText(rs, 13, 44, 2))
            validityDay = Integer.Parse(ReflectionInterface.GetText(rs, 13, 46, 2))
            validityYear = Integer.Parse(ReflectionInterface.GetText(rs, 13, 48, 4))
            brwDemographics.PhoneValidityDate = New Date(validityYear, validityMonth, validityDay)
        End If
        brwDemographics.Email = ReflectionInterface.GetText(rs, 19, 9, 56)
        brwDemographics.EmailValidityIndicator = ReflectionInterface.GetText(rs, 18, 56, 1)
        If Not ReflectionInterface.Check4Text(rs, 18, 71, "MMDDCCYY") Then
            validityMonth = Integer.Parse(ReflectionInterface.GetText(rs, 18, 71, 2))
            validityDay = Integer.Parse(ReflectionInterface.GetText(rs, 18, 73, 2))
            validityYear = Integer.Parse(ReflectionInterface.GetText(rs, 18, 75, 4))
            brwDemographics.EmailValidityDate = New Date(validityYear, validityMonth, validityDay)
        End If
        Return brwDemographics
    End Function

#End Region

#Region "LP40UserID"
    ''' <summary>
    ''' Gets the user's ID.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    Public Shared Function GetUserIDFromLP40(ByVal ri As ReflectionInterface) As String
        Return GetUserIDFromLP40(ri.ReflectionSession)
    End Function

    ''' <summary>
    ''' Gets the user's ID.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    Public Shared Function GetUserIDFromLP40(ByVal rs As Session) As String
        ReflectionInterface.FastPath(rs, "LP40I")
        If Not ReflectionInterface.Check4Text(rs, 1, 77, "ANCE") Then
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.Enter)
        End If
        Return rs.GetDisplayText(3, 14, 7)
    End Function
#End Region

#Region "LP50Add"
    ''' <summary>
    ''' Enter comments in LP50.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">The person ID: SSN for borrowers, co-borrowers, students; reference ID for references.</param>
    ''' <param name="arc">Action code</param>
    ''' <param name="scriptId">Script ID</param>
    ''' <param name="activityType">Activity type</param>
    ''' <param name="contactType">Contact type</param>
    ''' <param name="comment">Activity comments</param>
    ''' <param name="pauseForUserComments">True if the script should pause to allow the user to enter additional comments.</param>
    ''' <param name="isReference">True if the SSN should go in the ASSOCIATED PERSON ID space.</param>
    ''' <returns>True if the activity comment was successfully added.</returns>
    Public Shared Function AddCommentInLP50(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal scriptId As String, Optional ByVal activityType As String = "", Optional ByVal contactType As String = "", Optional ByVal comment As String = "", Optional ByVal pauseForUserComments As Boolean = False, Optional ByVal isReference As Boolean = False) As Boolean
        Return AddCommentInLP50(ri.ReflectionSession, ssn, arc, scriptId, activityType, contactType, comment, pauseForUserComments, isReference)
    End Function

    ''' <summary>
    ''' Enter comments in LP50.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">The person ID: SSN for borrowers, co-borrowers, students; reference ID for references.</param>
    ''' <param name="arc">Action code</param>
    ''' <param name="scriptId">Script ID</param>
    ''' <param name="activityType">Activity type</param>
    ''' <param name="contactType">Contact type</param>
    ''' <param name="comment">Activity comments</param>
    ''' <param name="pauseForUserComments">True if the script should pause to allow the user to enter additional comments.</param>
    ''' <param name="isReference">True if the SSN should go in the ASSOCIATED PERSON ID space.</param>
    ''' <returns>True if the activity comment was successfully added.</returns>
    Public Shared Function AddCommentInLP50(ByVal rs As Session, ByVal ssn As String, ByVal arc As String, ByVal scriptId As String, Optional ByVal activityType As String = "", Optional ByVal contactType As String = "", Optional ByVal comment As String = "", Optional ByVal pauseForUserComments As Boolean = False, Optional ByVal isReference As Boolean = False) As Boolean
        If isReference Then
            ReflectionInterface.FastPath(rs, "LP50A;" + ssn & ";;" + activityType + ";" + contactType + ";" + arc)
        Else
            ReflectionInterface.FastPath(rs, "LP50A" + ssn + ";;;" + activityType + ";" + contactType + ";" + arc)
        End If
        If ReflectionInterface.Check4Text(rs, 22, 3, "47004") Then
            Return False
        End If

        'Pause for the user to enter the activity type and contact type if the parameter is blank.
        If String.IsNullOrEmpty(activityType) Then
            MessageBox.Show("Enter the activity type and the contact type, and then hit <insert> to continue ths script.", "Enter Contact Type and Activity Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
            rs.WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.Insert)
        ElseIf ReflectionInterface.Check4Text(rs, 7, 2, "__") Then
            ReflectionInterface.PutText(rs, 7, 2, activityType + contactType)
        End If

        'Enter the comment.
        Dim commentAppendage As String = If(pauseForUserComments, String.Empty, "  {" + scriptId + "}")
        'Split the comment into multiple parts.
        If Len(comment) > 580 Then 'The comment text is to long.
            'Try to preform the command which will create a debug situation and in turn bring the problem to a programmer's attention.
            ReflectionInterface.PutText(rs, 13, 2, comment + commentAppendage)
        ElseIf Len(comment) > 480 Then 'Greater than 480 but less than the 580 limit.
            ReflectionInterface.PutText(rs, 13, 2, comment.Substring(0, 240))
            ReflectionInterface.PutText(rs, rs.CursorRow, rs.CursorColumn, comment.Substring(240, 240))
            ReflectionInterface.PutText(rs, rs.CursorRow, rs.CursorColumn, comment.Substring(480, 240) + commentAppendage)
        ElseIf Len(comment) > 240 Then 'Greater that 240 but less than 480.
            ReflectionInterface.PutText(rs, 13, 2, comment.Substring(0, 240))
            ReflectionInterface.PutText(rs, rs.CursorRow, rs.CursorColumn, comment.Substring(240, 240) + commentAppendage)
        Else 'Less than 240 and thus can be transmitted in one statement.
            ReflectionInterface.PutText(rs, 13, 2, comment)
        End If
        If pauseForUserComments Then
            Dim result As DialogResult = MessageBox.Show("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", "Enter Additional Questions?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                PauseForInsert(rs)
            End If
        End If
        Dim row As Integer = 13
        Dim column As Integer = 2
        Do Until row = 21
            If ReflectionInterface.Check4Text(rs, row, column, "____________", "            ") AndAlso _
            ReflectionInterface.Check4Text(rs, row + 1, 2, "_", " ") Then
                ReflectionInterface.PutText(rs, row, column, commentAppendage)
                Exit Do
            ElseIf column = 62 Then
                column = 2
                row += 1
            Else
                column += 1
            End If
        Loop

        ReflectionInterface.Hit(rs, ReflectionInterface.Key.F6)
        Return ReflectionInterface.Check4Text(rs, 22, 3, "48003", "48081")
    End Function

#End Region

#Region "LP50JudgeName"
    ''' <summary>
    ''' Gets the name of the judge from an activity record comment.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Borrower SSN</param>
    Public Shared Function GetJudgeNameFromLP50(ByVal ri As ReflectionInterface, ByVal ssn As String) As String
        Return GetJudgeNameFromLP50(ri.ReflectionSession, ssn)
    End Function

    ''' <summary>
    ''' Gets the name of the judge from an activity record comment.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower SSN</param>
    Public Shared Function GetJudgeNameFromLP50(ByVal rs As Session, ByVal ssn As String) As String
        ReflectionInterface.FastPath(rs, "LP50I" + ssn)
        ReflectionInterface.PutText(rs, 9, 20, "DJGNM", ReflectionInterface.Key.Enter)

        If ReflectionInterface.Check4Text(rs, 3, 2, "SEL") Then
            ReflectionInterface.PutText(rs, 3, 13, "X", ReflectionInterface.Key.Enter)
        ElseIf Not ReflectionInterface.Check4Text(rs, 4, 2, "TYPE") Then
            MsgBox("The judge's name was not found.  Add the name to the garnishment document and to OneLINK after the script ends.", 48, "Judge Name not Found")
            Return String.Empty
        End If
        Dim judgeLine As String = ReflectionInterface.GetText(rs, 13, 2, 35)
        Return judgeLine.Substring(0, judgeLine.IndexOf(" "))
    End Function
#End Region

#Region "LP9O"
    ''' <summary>
    ''' Add a queue task.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="ssn">Target SSN</param>
    ''' <param name="workgroup">Workgroup ID</param>
    ''' <param name="dateDue">Date due</param>
    ''' <param name="comment1">First comment line</param>
    ''' <param name="comment2">Second comment line</param>
    ''' <param name="comment3">Third comment line</param>
    ''' <param name="comment4">Fourth comment line</param>
    ''' <returns>True if the queue task was successfully added.</returns>
    Public Shared Function AddQueueTaskInLP9O(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal workgroup As String, Optional ByVal dateDue As String = "", Optional ByVal comment1 As String = "", Optional ByVal comment2 As String = "", Optional ByVal comment3 As String = "", Optional ByVal comment4 As String = "") As Boolean
        Return AddQueueTaskInLP9O(ri.ReflectionSession, ssn, workgroup, dateDue, comment1, comment2, comment3, comment4)
    End Function

    ''' <summary>
    ''' Add a queue task.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Target SSN</param>
    ''' <param name="workgroup">Workgroup ID</param>
    ''' <param name="dateDue">Date due</param>
    ''' <param name="comment1">First comment line</param>
    ''' <param name="comment2">Second comment line</param>
    ''' <param name="comment3">Third comment line</param>
    ''' <param name="comment4">Fourth comment line</param>
    ''' <returns>True if the queue task was successfully added.</returns>
    Public Shared Function AddQueueTaskInLP9O(ByVal rs As Session, ByVal ssn As String, ByVal workgroup As String, Optional ByVal dateDue As String = "", Optional ByVal comment1 As String = "", Optional ByVal comment2 As String = "", Optional ByVal comment3 As String = "", Optional ByVal comment4 As String = "") As Boolean
        'Keep trying to access LP9O until the system is ready or three tries.
        For i As Integer = 1 To 3
            ReflectionInterface.FastPath(rs, "LP9OA" + ssn + ";;" + workgroup)
            If ReflectionInterface.Check4Text(rs, 22, 3, "44000") Then
                Exit For
            Else
                rs.Wait(5)
            End If
        Next
        'See if we got in.
        If Not ReflectionInterface.Check4Text(rs, 22, 3, "44000") Then
            Return False
        End If

        'Add the queue task.
        If dateDue <> String.Empty Then ReflectionInterface.PutText(rs, 11, 25, Date.Parse(dateDue).ToString("MMddyyyy"))
        ReflectionInterface.PutText(rs, 16, 12, comment1)
        ReflectionInterface.PutText(rs, 17, 12, comment2)
        ReflectionInterface.PutText(rs, 18, 12, comment3)
        ReflectionInterface.PutText(rs, 19, 12, comment4)
        ReflectionInterface.Hit(rs, ReflectionInterface.Key.F6)
        rs.Wait(2) 'Go figure why this has to be here but it wouldn't work otherwise.
        'Verify update.
        If Not ReflectionInterface.Check4Text(rs, 22, 3, "48003") Then Return False
        Return True
    End Function

#End Region

#Region "LPEM"
    ''' <summary>
    ''' Gets an employer's name and address from LPEM.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="employerId">The employer's ID</param>
    ''' <returns>
    ''' A EmployerDemographics object with the applicable properties set.
    ''' </returns>
    Public Shared Function GetEmployerInfoFromLPEM(ByVal ri As ReflectionInterface, ByVal employerId As String) As EmployerDemographics
        Return GetEmployerInfoFromLPEM(ri.ReflectionSession, employerId)
    End Function

    ''' <summary>
    ''' Gets an employer's name and address from LPEM.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="employerId">The employer's ID</param>
    ''' <param name="preferredDepartments">
    ''' Department code to select if the INSTITUTION DEPARTMENT SELECTION screen is encountered.
    ''' The department code is found in column 7 of the selection screen, starting on row 7.
    ''' If multiple department codes are passed in, the function will try to find the first one;
    ''' failing that, it will try to find the next one, and so on until all passed-in codes are exhausted.
    ''' If no department codes are passed in, or none of the passed-in codes are found on the selection screen,
    ''' the function will prompt the user to select the desired department.
    ''' </param>
    ''' <returns>
    ''' An EmployerDemographics object with the applicable properties set.
    ''' </returns>
    Public Shared Function GetEmployerInfoFromLPEM(ByVal rs As Session, ByVal employerId As String, ByVal ParamArray preferredDepartments() As String) As EmployerDemographics
        ReflectionInterface.FastPath(rs, "LPEMI" + employerId)
        If (ReflectionInterface.Check4Text(rs, 1, 61, "DEPARTMENT SELECTION")) Then
            'Look for the preferred departments.
            Dim foundDepartment As Boolean = False
            For Each preferredDepartment As String In preferredDepartments
                For row As Integer = 7 To 19 Step 2
                    If (ReflectionInterface.Check4Text(rs, row, 7, preferredDepartment)) Then
                        foundDepartment = True
                        Dim selection As String = ReflectionInterface.GetText(rs, row, 3, 1)
                        ReflectionInterface.PutText(rs, 21, 13, selection, ReflectionInterface.Key.Enter)
                        Exit For
                    End If
                Next row
                If (foundDepartment) Then Exit For
            Next preferredDepartment
        End If
        'If we're still on the selection screen, either no preferred departments were passed in, or none were found.
        Do While ReflectionInterface.Check4Text(rs, 1, 61, "DEPARTMENT SELECTION")
            Dim message As String = "More than one employer record was found on the system. Please select the correct one and hit <INSERT> to continue."
            Dim caption As String = "Multiple Records Found"
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            PauseForInsert(rs)
        Loop
        If Not ReflectionInterface.Check4Text(rs, 1, 57, "INSTITUTION DEMOGRAPHICS") Then
            Throw New DemographicRetrievalException("The employer information wasn't found on the system.  Please contact Systems Support.")
        End If

        Dim demographics As New EmployerDemographics With { _
            .Addr1 = ReflectionInterface.GetText(rs, 8, 21, 40), _
            .Addr2 = ReflectionInterface.GetText(rs, 9, 21, 40), _
            .City = ReflectionInterface.GetText(rs, 11, 21, 30), _
            .State = ReflectionInterface.GetText(rs, 11, 59, 2), _
            .Zip = ReflectionInterface.GetText(rs, 11, 66, 9), _
            .Name = ReflectionInterface.GetText(rs, 5, 21, 40) _
            }
        Return demographics
    End Function

#End Region
#End Region


#Region "ACSKeyline"
    ''' <summary>
    ''' Calculates the ACS keyline
    ''' </summary>
    ''' <param name="ssn">SSN of borrower or Reference ID of reference</param>
    ''' <param name="personType">Person Type (was defaulted to borrower)</param>
    ''' <param name="addressType">Address Type (was defaulted to legal)</param>
    ''' <returns>Calculated ACS keyline</returns>
    Public Shared Function ACSKeyLine(ByVal ssn As String, ByVal personType As ACSKeyLinePersonType, ByVal addressType As ACSKeyLineAddressType) As String
        'Translate person type.
        Dim personTypeString As String
        If personType = ACSKeyLinePersonType.Borrower Then
            personTypeString = "P"
        Else
            personTypeString = "R"
        End If

        'Translate address type.
        Dim addressTypeString As String
        If addressType = ACSKeyLineAddressType.Legal Then
            addressTypeString = "L"
        ElseIf addressType = ACSKeyLineAddressType.Alternate Then
            addressTypeString = "A"
        Else
            addressTypeString = "T"
        End If

        'Encode the ssn variable based on the person type.
        Dim encodedSsn As String = String.Empty
        If personType = ACSKeyLinePersonType.Borrower Then
            'Encode SSN
            Dim nextLetter As String = String.Empty
            For i As Integer = 0 To 8
                Select Case ssn.Substring(i, 1)
                    Case 1
                        nextLetter = "R"
                    Case 2
                        nextLetter = "E"
                    Case 3
                        nextLetter = "T"
                    Case 4
                        nextLetter = "H"
                    Case 5
                        nextLetter = "G"
                    Case 6
                        nextLetter = "U"
                    Case 7
                        nextLetter = "A"
                    Case 8
                        nextLetter = "L"
                    Case 9
                        nextLetter = "Y"
                    Case 0
                        nextLetter = "M"
                End Select
                encodedSsn += nextLetter
            Next i
        Else
            'Encode reference ID
            encodedSsn = ssn.Substring(0, 2) + "/" + ssn.Substring(3, 6)
        End If

        'Add person type and address type to the encoded SSN/reference ID to get the working keyline.
        Dim workingKeyline As String = personTypeString + encodedSsn + Date.Now.ToString("MMdd") + addressTypeString

        'Convert working keyline characters to 4-bit numbers and calculate the check digit.
        Dim checkDigit As Integer = 0
        For i As Integer = 0 To workingKeyline.Length - 1
            Dim keylineBitValue As Integer = 0
            Select Case workingKeyline.Substring(i, 1)
                Case "A"
                    keylineBitValue = 1
                Case "B"
                    keylineBitValue = 2
                Case "C"
                    keylineBitValue = 3
                Case "D"
                    keylineBitValue = 4
                Case "E"
                    keylineBitValue = 5
                Case "F"
                    keylineBitValue = 6
                Case "G"
                    keylineBitValue = 7
                Case "H"
                    keylineBitValue = 8
                Case "I"
                    keylineBitValue = 9
                Case "J"
                    keylineBitValue = 10
                Case "K"
                    keylineBitValue = 11
                Case "L"
                    keylineBitValue = 12
                Case "M"
                    keylineBitValue = 13
                Case "N"
                    keylineBitValue = 14
                Case "O"
                    keylineBitValue = 15
                Case "P"
                    keylineBitValue = 0
                Case "Q"
                    keylineBitValue = 1
                Case "R"
                    keylineBitValue = 2
                Case "S"
                    keylineBitValue = 3
                Case "T"
                    keylineBitValue = 4
                Case "U"
                    keylineBitValue = 5
                Case "V"
                    keylineBitValue = 6
                Case "W"
                    keylineBitValue = 7
                Case "X"
                    keylineBitValue = 8
                Case "Y"
                    keylineBitValue = 9
                Case "Z"
                    keylineBitValue = 10
                Case "/"
                    keylineBitValue = 15
                Case Else
                    keylineBitValue = Integer.Parse(workingKeyline.Substring(i, 1))
            End Select
            'Multiply the value by 2 if the index is even.
            If i Mod 2 = 0 Then keylineBitValue *= 2
            'Add the digits together. Keep doing this until we get a single digit.
            Do While keylineBitValue.ToString().Length > 1
                Dim keylineBitString As String = keylineBitValue.ToString()
                keylineBitValue = 0
                For x As Integer = 0 To keylineBitString.Length - 1
                    keylineBitValue += Integer.Parse(keylineBitString.Substring(x, 1))
                Next x
            Loop
            'Add the sum to the check digit.
            checkDigit += keylineBitValue
        Next i

        'Subtract the rightmost digit of the check digit from 10 to get the final check digit.
        checkDigit = 10 - (checkDigit Mod 10)
        'The check digit should be a single digit, so if it's 10, set it to 0.
        If checkDigit = 10 Then checkDigit = 0
        'Add the check digit to the end of the working keyline.
        Return String.Format("#{0}{1}#", workingKeyline, checkDigit.ToString())
    End Function

    ''' <summary>
    ''' Translates an ACS Keyline back to an SSN.
    ''' </summary>
    ''' <param name="keyline">The 9-character ACS Keyline to translate.</param>
    Public Shared Function GetSsnFromKeyline(ByVal keyline As String) As String
        Dim ssn As String = String.Empty
        For charPosition As Integer = 0 To 8
            Select Case keyline.Substring(charPosition, 1).ToUpper()
                Case "M"
                    ssn += "0"
                Case "Y"
                    ssn += "9"
                Case "L"
                    ssn += "8"
                Case "A"
                    ssn += "7"
                Case "U"
                    ssn += "6"
                Case "G"
                    ssn += "5"
                Case "H"
                    ssn += "4"
                Case "T"
                    ssn += "3"
                Case "E"
                    ssn += "2"
                Case "R"
                    ssn += "1"
                Case Else
                    MessageBox.Show("One of the keyline characters wasn't a valid ACS Keyline character. The script will now end.")
                    EndDLLScript()
            End Select
        Next charPosition
        Return ssn
    End Function
#End Region

#Region "AKA"
    ''' <summary>
    ''' Go to LP22 to get the alternate billing name to be used as the AKA
    ''' </summary>
    ''' <param name="ri">The Reflection Interface</param>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <param name="name">Borrower's Name</param>
    ''' <returns>AKA Name</returns>
    ''' <remarks></remarks>
    Public Shared Function AKA(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal name As String) As String
        Return AKA(ri.ReflectionSession, ssn, name)
    End Function

    ''' <summary>
    ''' Go to LP22 to get the alternate billing name to be used as the AKA
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <param name="name">Borrower's Name</param>
    ''' <returns>AKA Name</returns>
    ''' <remarks></remarks>
    Public Shared Function AKA(ByVal rs As Session, ByVal ssn As String, ByVal name As String) As String
        ReflectionInterface.FastPath(rs, "LP22I" + ssn)
        'use the hotkeys to page to the alternate address
        ReflectionInterface.Hit(rs, ReflectionInterface.Key.F2)
        ReflectionInterface.Hit(rs, ReflectionInterface.Key.F10)
        ReflectionInterface.Hit(rs, ReflectionInterface.Key.F10)
        'get the alternate billing name to be used as the AKA
        Dim alternateBillingName As String = ReflectionInterface.GetText(rs, 9, 27, 50)
        'if the alt billing name is not the same as the borrower's name and is not blank, add "AKA " to the front of it
        If name <> alternateBillingName AndAlso alternateBillingName.Length > 0 Then
            Return "AKA " + alternateBillingName
        Else
            Return String.Empty
        End If
    End Function
#End Region

#Region "CreateDataTableFromFile"
    ''' <summary>
    ''' Reads a comma-delimited file into a DataTable.
    ''' </summary>
    ''' <param name="fileName">The full path and file name of the file to be read.</param>
    ''' <returns>A DataTable object populated with the file's contents.</returns>
    Public Shared Function CreateDataTableFromFile(ByVal fileName As String) As DataTable
        Return CreateDataTableFromFile(fileName, True, New String() {})
    End Function

    ''' <summary>
    ''' Reads a comma-delimited file into a DataTable.
    ''' </summary>
    ''' <param name="fileName">The full path and file name of the file to be read.</param>
    ''' <param name="firstRowIsHeader">True if the first line in the file has header fields.</param>
    ''' <param name="columnNames">The names to be used for the DataTable's columns.</param>
    ''' <returns>A DataTable object populated with the file's contents.</returns>
    Public Shared Function CreateDataTableFromFile(ByVal fileName As String, ByVal firstRowIsHeader As Boolean, ByVal ParamArray columnNames() As String) As DataTable
        Dim tempDictionary As New Dictionary(Of String, Type)
        For Each field In columnNames
            tempDictionary.Add(field, System.Type.GetType("System.String"))
        Next
        Return CreateDataTableFromFile(fileName, firstRowIsHeader, tempDictionary)
    End Function

    Public Shared Function CreateDataTableFromFile(ByVal fileName As String, ByVal firstRowIsHeader As Boolean, ByVal fields As Dictionary(Of String, Type)) As DataTable
        Dim fileTable As New DataTable()
        Using fileReader As New StreamReader(fileName)
            If firstRowIsHeader = True Then
                'Define columns based on whether the client has specified them.
                If fields.Count > 0 Then
                    'ignore header row
                    Dim dumb As String = fileReader.ReadLine()
                    'Use the provided column names.
                    For Each columnName As KeyValuePair(Of String, System.Type) In fields
                        fileTable.Columns.Add(columnName.Key, columnName.Value)
                    Next
                Else
                    'Read column names from the first row of the file.
                    Dim headerFields As List(Of String) = fileReader.ReadLine().SplitAgnosticOfQuotes(",")
                    For Each headerField As String In headerFields
                        fileTable.Columns.Add(headerField)
                    Next
                End If
            Else
                'Define columns based on whether the client has specified them.
                If fields.Count > 0 Then
                    'Use the provided column names.
                    For Each columnName As KeyValuePair(Of String, System.Type) In fields
                        fileTable.Columns.Add(columnName.Key, columnName.Value)
                    Next
                Else
                    'error condition can't do this
                    Throw New Exception(String.Format("A fatal error has occurred.  Please contact a member of Process Automation.  INFO: File {0} has no header row and a custom one wasn't provided to the common function {1}", fileName, MethodBase.GetCurrentMethod()))
                End If
            End If
            'Assign the remaining file lines to table rows.
            Do While Not fileReader.EndOfStream
                Dim dataFields As List(Of String) = fileReader.ReadLine().SplitAgnosticOfQuotes(",")
                'Make sure the line has the right number of elements.
                If dataFields.Count = fileTable.Columns.Count Then
                    fileTable.Rows.Add(dataFields.ToArray())
                Else
                    Dim messageBuilder As New StringBuilder()
                    messageBuilder.Append(String.Format("Problem reading file {0}:{1}", fileName, Environment.NewLine))
                    messageBuilder.Append(String.Format("Expected {0} fields, but found {1} fields in the line starting with {2}", fileTable.Columns.Count, dataFields.Count, dataFields(0)))
                    Throw New Exception(messageBuilder.ToString())
                End If
            Loop
            fileReader.Close()
        End Using
        Return fileTable
    End Function
#End Region

#Region "GeneralFileHandling"
    ''' <summary>
    ''' Deletes a file, checking that it is really gone before returning.
    ''' </summary>
    ''' <param name="fileName">Full path and name of file to be deleted.</param>
    Public Shared Sub KillUntilDead(ByVal fileName As String)
        If Not File.Exists(fileName) Then Return
        File.Delete(fileName)
        Do While File.Exists(fileName)
            Thread.Sleep(500)
        Loop
    End Sub

    ''' <summary>
    ''' Sorts file data
    ''' </summary>
    ''' <param name="FilePathAndName">The full path and file name of file to sort.</param>
    ''' <param name="ElementToSortBy">The index of the column to sort.</param>
    ''' <param name="HeaderRow">The header row to prepend to the file.  Before the C# conversion this was set as an empty string.  If and empty string is passed, the function won't prepend anything to the file.</param>
    ''' <remarks></remarks>
    Public Shared Sub SortFile(ByVal filePathAndName As String, ByVal elementToSortBy As Integer, ByVal headerRow As String)
        Dim fileRecords As New List(Of String)()
        FileOpen(1, filePathAndName, OpenMode.Input)
        Do Until EOF(1)
            fileRecords.Add(LineInput(1))
        Loop
        FileClose(1)

        For currentRecord As Integer = 0 To fileRecords.Count - 1
            For laterRecord As Integer = currentRecord + 1 To fileRecords.Count - 1
                Dim currentFields() As String = fileRecords(currentRecord).Split(",") 'create array pointer to array
                Dim laterFields() As String = fileRecords(laterRecord).Split(",") 'create array pointer to array
                If currentFields(elementToSortBy) > laterFields(elementToSortBy) Then
                    Dim swapVariable As String = fileRecords(laterRecord)
                    fileRecords(laterRecord) = fileRecords(currentRecord)
                    fileRecords(currentRecord) = swapVariable
                End If
            Next laterRecord
        Next currentRecord

        FileOpen(2, filePathAndName, OpenMode.Output)
        'header row
        If headerRow.Length > 0 Then WriteLine(2, headerRow)
        For Each sortedRecord As String In fileRecords
            WriteLine(2, sortedRecord)
        Next sortedRecord
        FileClose(2)
    End Sub
#End Region

#Region "Loans"
    ''' <summary>
    ''' Checks whether any member of a list of loan types falls under a given type key.
    ''' </summary>
    ''' <param name="loanTypes">The list of loan types to be checked.</param>
    ''' <param name="typeKey">The type key used by the database.</param>
    ''' <param name="testMode">True if the application is running in test mode.</param>
    ''' <returns>True if any of the loan types match the type key.</returns>
    Public Shared Function HasLoanOfType(ByVal loanTypes As List(Of String), ByVal typeKey As String, ByVal testMode As Boolean) As Boolean
        Dim typesByKey As List(Of String) = DataAccess.GetLoanTypes(typeKey, testMode)
        For Each loanType As String In loanTypes
            If typesByKey.Contains(loanType) Then
                Return True
            End If
        Next loanType
        Return False
    End Function

    ''' <summary>
    ''' Checks LG02 and LG10 to determine if the borrower has open loans on OneLINK
    ''' </summary>
    ''' <param name="ri">Reflection Interface</param>
    ''' <param name="SSN">Borrower's SSN</param>
    ''' <returns>True if the borrower has open loans on OneLINK else false.</returns>
    ''' <remarks></remarks>
    Public Shared Function HasOpenLoanOnOneLINK(ByVal ri As ReflectionInterface, ByVal ssn As String) As Boolean
        Return HasOpenLoanOnOneLINK(ri.ReflectionSession, ssn)
    End Function

    ''' <summary>
    ''' Checks LG02 and LG10 to determine if the borrower has open loans on OneLINK
    ''' </summary>
    ''' <param name="rs">Reflection Session</param>
    ''' <param name="SSN">Borrower's SSN</param>
    ''' <returns>True if the borrower has open loans on OneLINK else false.</returns>
    ''' <remarks></remarks>
    Public Shared Function HasOpenLoanOnOneLINK(ByVal rs As Session, ByVal ssn As String) As Boolean
        Return OpenLoanFinder.HasOpenLoanOnOneLINK(rs, ssn)
    End Function

    ''' <summary>
    ''' Checks whether a loan type falls under a given type key.
    ''' </summary>
    ''' <param name="loanType">The loan type to be checked.</param>
    ''' <param name="typeKey">The type key used by the database.</param>
    ''' <param name="testMode">True if the application is running in test mode.</param>
    ''' <returns>True if the loan type matches the type key.</returns>
    Public Shared Function IsLoanType(ByVal loanType As String, ByVal typeKey As String, ByVal testMode As Boolean) As Boolean
        Return DataAccess.GetLoanTypes(typeKey, testMode).Contains(loanType)
    End Function

    ''' <summary>
    ''' Checks TS26 for a loan with a non-zero current balance.
    ''' </summary>
    ''' <param name="ri">Reflection Interface</param>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <returns>Common.AesSystem.Compass if an open loan is found, otherwise Common.AesSystem.None.</returns>
    Public Shared Function LookForOpenLoanOnCompass(ByVal ri As ReflectionInterface, ByVal ssn As String) As AesSystem
        Return OpenLoanFinder.LookForOpenLoanOnCompass(ri, ssn)
    End Function

    ''' <summary>
    ''' Checks LG02 and LG10 for a loan whose status indicates it's open.
    ''' </summary>
    ''' <param name="ri">Reflection Interface</param>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <returns>
    ''' Common.AesSystem.OneLink if a loan is found with an open status code,
    ''' Common.AesSystem.Compass if a loan is found with a status of CP and an LC05 status of 04 or 03 with a UHEAA servicer code,
    ''' Common.AesSystem.OneLink if a loan is found with a status of CP and an LC05 status of 04 or 03 with a non-UHEAA servicer code,
    ''' a combination of the above two if both UHEAA and non-UHEAA servicer codes are found on loans in CP status with an LC05 status of 03,
    ''' or Common.AesSystem.None if no open loans are found.
    ''' </returns>
    ''' <remarks>
    ''' Open status codes include CR, AL, DA, FB, IA, ID, IG, IM, RP, UA, and UB.
    ''' A status of CP is considered open if the LC05 status is 03 or 04 and there is no transfer date.
    ''' </remarks>
    Public Shared Function LookForOpenLoanOnOneLink(ByVal ri As ReflectionInterface, ByVal ssn As String) As AesSystem
        Return OpenLoanFinder.LookForOpenLoanOnOneLink(ri, ssn)
    End Function

    ''' <summary>
    ''' Checks TS26 for a borrower's loan status(es).
    ''' </summary>
    ''' <remarks>
    ''' "DECONVERTED", "PAID IN FULL", and "CLAIM PAID" are considered closed.
    ''' All other statuses are considered open.
    ''' A power-of-two enum is returned so that multiple loan statuses can be represented if needed.
    ''' </remarks>
    Private Shared Function GetCompassLoanStatuses(ByVal rs As Session, ByVal ssn As String) As LoanStatus
        Dim CLOSED_STATUSES As String() = {"DECONVERTED", "PAID IN FULL", "CLAIM PAID"}
        Dim loanStatuses As LoanStatus = LoanStatus.None

        If (ssn.StartsWith("P")) Then
            ReflectionInterface.FastPath(rs, "TX3Z/ITX1J;{0}", ssn)
            ssn = ReflectionInterface.GetText(rs, 7, 11, 11).Replace(" ", "")
        ElseIf (ssn.StartsWith("RF@")) Then
            ReflectionInterface.FastPath(rs, "LP2CI;{0}", ssn)
            ssn = ReflectionInterface.GetText(rs, 3, 39, 9)
        End If

        ReflectionInterface.FastPath(rs, "TX3Z/ITS26{0}", ssn)
        If (ReflectionInterface.Check4Text(rs, 1, 72, "TSX29")) Then
            'Target screen.
            If (ReflectionInterface.Check4Text(rs, 3, 10, CLOSED_STATUSES)) Then
                loanStatuses = loanStatuses Or LoanStatus.Closed
            Else
                loanStatuses = loanStatuses Or LoanStatus.Open
            End If
        ElseIf (ReflectionInterface.Check4Text(rs, 1, 72, "TSX28")) Then
            'Selection screen.
            Dim row As Integer = 8
            Do While (ReflectionInterface.Check4Text(rs, row, 2, "  ") = False AndAlso ReflectionInterface.Check4Text(rs, 23, 2, "90007 NO MORE DATA TO DISPLAY") = False)
                'Select the next loan.
                ReflectionInterface.PutText(rs, 21, 12, ReflectionInterface.GetText(rs, row, 2, 2), ReflectionInterface.Key.Enter, True)
                'Get the loan status.
                If (ReflectionInterface.Check4Text(rs, 3, 10, CLOSED_STATUSES)) Then
                    loanStatuses = loanStatuses Or LoanStatus.Closed
                Else
                    loanStatuses = loanStatuses Or LoanStatus.Open
                End If
                'Stop looking if we've found both open and closed loans.
                Dim foundOpenLoan As Boolean = ((loanStatuses And LoanStatus.Open) <> 0)
                Dim foundClosedLoan As Boolean = ((loanStatuses And LoanStatus.Closed) <> 0)
                If (foundOpenLoan AndAlso foundClosedLoan) Then Exit Do
                'Back out and move on to the next row.
                ReflectionInterface.Hit(rs, ReflectionInterface.Key.F12)
                row += 1
                If (row > 19) Then
                    ReflectionInterface.Hit(rs, ReflectionInterface.Key.F8)
                    row = 8
                End If
            Loop
        End If

        Return loanStatuses
    End Function
#End Region

#Region "Login"


    ''' <summary>
    ''' Checks to see if the user is logged in.
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <returns>returns True if the user is logged in (LP40 screen is found), else false</returns>
    ''' <remarks></remarks>
    Public Shared Function IsLoggedIn(ByVal rs As Session) As Boolean
        IsLoggedIn = False
        rs.TransmitTerminalKey(rcIBMClearKey)
        rs.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        rs.MoveCursor(1, 1)
        rs.TransmitANSI("PROF")
        If ReflectionInterface.GetText(rs, 26, 9, 7) = "" Then
            'If hangman is not found, hit Enter to continue to the profile screen and check that we got there.
            ReflectionInterface.Hit(rs, ReflectionInterface.Key.Enter)
            Return ReflectionInterface.Check4Text(rs, 2, 13, "=== PROFILE OPTIONS ===")
        Else
            'If hangman is found, we're not logged in. Clear the hangman.
            rs.TransmitTerminalKey(rcIBMResetKey)
            rs.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            Return False
        End If
    End Function
#End Region

#Region "SASFileProcessing"
    ''' <summary>
    ''' Gets the path and name of the newest file that matches a given search pattern.
    ''' </summary>
    ''' <param name="path">The directory in which to look for files.</param>
    ''' <param name="searchPattern">The pattern that the file name must match. Wild cards are acceptable.</param>
    ''' <param name="options">Flags for additional checks.</param>
    ''' <returns>
    ''' The path and name of the newest file in the given path,
    ''' or an empty string if no matches are found and the ErrorOnMissing option is not selected.
    ''' </returns>
    ''' <remarks>Exceptions will not be generated if their respective options are not selected.</remarks>
    ''' <exception cref="FileNotFoundException"></exception>
    ''' <exception cref="FileEmptyException"></exception>
    Public Shared Function DeleteOldFilesReturnMostCurrent(ByVal path As String, ByVal searchPattern As String, ByVal options As FileOptions) As String
        'Is the file missing?
        Dim foundFiles As String() = Directory.GetFiles(path, searchPattern)
        If foundFiles.Length = 0 Then
            'See whether to throw an exception or return an empty string.
            If (options And FileOptions.ErrorOnMissing) = FileOptions.ErrorOnMissing Then
                Throw New FileNotFoundException()
            Else
                Return String.Empty
            End If
        End If

        'Find the newest file.
        Dim newestFile As String = foundFiles(0)
        For Each foundFile As String In foundFiles
            If File.GetLastWriteTime(foundFile) < File.GetLastWriteTime(newestFile) Then
                File.Delete(foundFile)
            ElseIf File.GetLastWriteTime(foundFile) > File.GetLastWriteTime(newestFile) Then
                File.Delete(newestFile)
                newestFile = foundFile
            End If
        Next foundFile

        'See whether the file is empty, if needed.
        If (options And FileOptions.ErrorOnEmpty) = FileOptions.ErrorOnEmpty _
        AndAlso New FileInfo(newestFile).Length = 0 Then
            Throw New FileEmptyException("File Empty", newestFile)
        End If

        Return newestFile
    End Function

    ''' <summary>
    ''' Returns the name of the oldest file that matches the search pattern given.
    ''' </summary>
    ''' <param name="path">Path to search</param>
    ''' <param name="searchPattern">Search pattern</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ReturnOldestFile(ByVal path As String, ByVal searchPattern As String) As String
        'Is the file missing?
        Dim foundFiles As String() = Directory.GetFiles(path, searchPattern)
        If foundFiles.Length = 0 Then
            Return String.Empty
        End If

        'Find the oldest file.
        Dim oldestFile As String = foundFiles(0)
        For Each foundFile As String In foundFiles
            If File.GetLastWriteTime(foundFile) < File.GetLastWriteTime(oldestFile) Then
                oldestFile = foundFile
            End If
        Next foundFile

        Return oldestFile
    End Function

    ''' <summary>
    ''' Searches for the next file (as determined by the Visual Basic Dir() function) that is populated.
    ''' If file found is empty then it deletes it and searches for another else it returns it.
    ''' </summary>
    ''' <param name="path">Path to the file.</param>
    ''' <param name="searchPattern">The pattern that the file name must match. Wild cards are acceptable.</param>
    ''' <returns>The full path and name of the next populated file, or an empty string if no populated files remain.</returns>
    Public Shared Function GetNextPopulatedFile(ByVal path As String, ByVal searchPattern As String) As String
        Dim nextPopulatedFile As String = Dir(path + searchPattern)
        If nextPopulatedFile.Length > 0 Then
            While New FileInfo(path + nextPopulatedFile).Length = 0
                File.Delete(path + nextPopulatedFile)
                nextPopulatedFile = Dir(path + searchPattern)
                If String.IsNullOrEmpty(nextPopulatedFile) Then
                    Return String.Empty
                End If
            End While
        End If
        Return nextPopulatedFile
    End Function
#End Region

#Region "SendMail"
    ''' <summary>
    ''' Send an e-mail message using SMTP (sends as plain text [not HTML])
    ''' </summary>
    ''' <param name="testMode">If in test mode.</param>
    ''' <param name="mto">Email address of who to send the email to.  Comma delimit multiple entries.</param>
    ''' <param name="mFrom">Who the email is being sent from.  Send "" if you want the sub to figure it out for you.</param>
    ''' <param name="mSubject">The email subject line.</param>
    ''' <param name="mBody">The email body text.</param>
    ''' <param name="mCC">Recipients to carbon copy.  Comma delimit multiple entries.</param>
    ''' <param name="mBCC">Recipients to blind copy.  Comma delimit multiple entries.</param>
    ''' <param name="mAttach">Attachments for email.  Comma delimit multiple entries.  Send blank string if non are desired.</param>
    ''' <param name="mImportance">The importance level assigned to the email.</param>
    ''' <param name="TestIt">If this indicator is set to true and the application is in test mode then the current user will be added to the to list and "THIS IS A TEST" is added to the subject and cody text.  Was defaulted to True before the C# conversion.</param>
    ''' <remarks></remarks>
    Public Shared Sub SendMail(ByVal testMode As Boolean, ByVal mto As String, ByVal mFrom As String, ByVal mSubject As String, ByVal mBody As String, ByVal mCC As String, ByVal mBCC As String, ByVal mAttach As String, ByVal mImportance As EmailImportanceLevel, ByVal TestIt As Boolean)
        SendMail(testMode, mto, mFrom, mSubject, mBody, mCC, mBCC, mAttach, mImportance, TestIt, False)
    End Sub

    ''' <summary>
    ''' Send an e-mail message using SMTP (sends as plain text [not HTML])
    ''' </summary>
    ''' <param name="mode">ConfigurationMode</param>
    ''' <param name="mto">Email address of who to send the email to.  Comma delimit multiple entries.</param>
    ''' <param name="mFrom">Who the email is being sent from.  Send "" if you want the sub to figure it out for you.</param>
    ''' <param name="mSubject">The email subject line.</param>
    ''' <param name="mBody">The email body text.</param>
    ''' <param name="mCC">Recipients to carbon copy.  Comma delimit multiple entries.</param>
    ''' <param name="mBCC">Recipients to blind copy.  Comma delimit multiple entries.</param>
    ''' <param name="mAttach">Attachments for email.  Comma delimit multiple entries.  Send blank string if non are desired.</param>
    ''' <param name="mImportance">The importance level assigned to the email.</param>
    ''' <param name="TestIt">If this indicator is set to true and the application is in test mode then the current user will be added to the to list and "THIS IS A TEST" is added to the subject and cody text.  Was defaulted to True before the C# conversion.</param>
    ''' <remarks></remarks>
    Public Shared Sub SendMailByMode(ByVal mode As DataAccessBase.ConfigurationMode, ByVal mto As String, ByVal mFrom As String, ByVal mSubject As String, ByVal mBody As String, ByVal mCC As String, ByVal mBCC As String, ByVal mAttach As String, ByVal mImportance As EmailImportanceLevel, ByVal TestIt As Boolean)
        Select Case mode
            Case DataAccessBase.ConfigurationMode.Live
                SendMail(False, mto, mFrom, mSubject, mBody, mCC, mBCC, mAttach, mImportance, TestIt, False)
            Case Else
                SendMail(True, mto, mFrom, mSubject, mBody, mCC, mBCC, mAttach, mImportance, TestIt, False)
        End Select
    End Sub

    ''' <summary>
    ''' Send an e-mail message using SMTP (provides option of HTML or plain text)
    ''' </summary>
    ''' <param name="testMode">If in test mode.</param>
    ''' <param name="mto">Email address of who to send the email to.  Comma delimit multiple entries.</param>
    ''' <param name="mFrom">Who the email is being sent from.  Send "" if you want the sub to figure it out for you.</param>
    ''' <param name="mSubject">The email subject line.</param>
    ''' <param name="mBody">The email body text.</param>
    ''' <param name="mCC">Recipients to carbon copy.  Comma delimit multiple entries.</param>
    ''' <param name="mBCC">Recipients to blind copy.  Comma delimit multiple entries.</param>
    ''' <param name="mAttach">Attachments for email.  Comma delimit multiple entries.  Send blank string if non are desired.</param>
    ''' <param name="mImportance">The importance level assigned to the email.</param>
    ''' <param name="TestIt">If this indicator is set to true and the application is in test mode then the current user will be added to the to list and "THIS IS A TEST" is added to the subject and cody text.  Was defaulted to True before the C# conversion.</param>
    ''' <param name="AsHTML">Indicator as to whether the email should be sent as HTML or text.</param>
    ''' <remarks></remarks>
    Public Shared Sub SendMail(ByVal testMode As Boolean, ByVal mto As String, ByVal mFrom As String, ByVal mSubject As String, ByVal mBody As String, ByVal mCC As String, ByVal mBCC As String, ByVal mAttach As String, ByVal mImportance As EmailImportanceLevel, ByVal TestIt As Boolean, ByVal AsHTML As Boolean)
        Dim aAttach() As String
        Dim I As Integer
        Dim Email As New OSSMTP.SMTPSession()
        Dim SecondsWaited As Integer
        Dim importanceLevel As importance_level
        Select Case mImportance
            Case EmailImportanceLevel.High
                importanceLevel = importance_level.ImportanceHigh
            Case EmailImportanceLevel.Low
                importanceLevel = importance_level.ImportanceLow
            Case EmailImportanceLevel.Normal
                importanceLevel = importance_level.ImportanceNormal
        End Select

        'set server and time out (default = 10 secs, set to 20 secs)
        Email.Server = "mail.utahsbr.edu"
        Email.Timeout = 20000

        'add test text and recipient
        If TestIt AndAlso testMode Then
            mto = mto & "," & Environ("USERNAME") & "@utahsbr.edu"
            mSubject = mSubject & " -- THIS IS A TEST"
            mBody = "THIS IS A TEST" & vbCrLf & vbCrLf & mBody
        End If

        'create message
        If mFrom = "" Then mFrom = Environ("USERNAME") & "@utahsbr.edu"
        Email.MailFrom = mFrom
        Email.SendTo = mto
        Email.CC = mCC
        Email.BCC = mBCC
        Email.MessageSubject = mSubject
        If AsHTML Then
            Email.MessageHTML = mBody
        Else
            Email.MessageText = mBody
        End If
        Email.Importance = importanceLevel

        'add attachments if there are any
        If Len(mAttach) > 0 Then
            'split file names from string
            aAttach = Split(mAttach, ",")

            'add attachments
            For I = 0 To UBound(aAttach)
                Email.Attachments.Add(aAttach(I))
            Next I
        End If

        'send message
        Email.SendEmail()

        'wait up to five seconds for the email to successfully be sent
        While Email.Status <> "SMTP connection closed"
            Threading.Thread.Sleep(New TimeSpan(0, 0, 1))
            SecondsWaited = SecondsWaited + 1
            If SecondsWaited = 5 AndAlso Email.Status <> "SMTP connection closed" Then
                'if the script has waited 5 seconds then give error message and exit function
                Throw New EmailException("Your message was not sent for the following reason:  " & Email.Status & "  Please contact Process Automation for assistance.")
            End If
        End While
        'if the script makes it through loop above then it successfully sent the email and received the all is OK message
    End Sub

    ''' <summary>
    ''' Send an e-mail message using SMTP (provides option of HTML or plain text)
    ''' </summary>
    ''' <param name="testMode">If in test mode.</param>
    ''' <param name="mto">Email address of who to send the email to.  Comma delimit multiple entries.</param>
    ''' <param name="mFrom">Who the email is being sent from.  Send "" if you want the sub to figure it out for you.</param>
    ''' <param name="mSubject">The email subject line.</param>
    ''' <param name="mBody">The email body text.</param>
    ''' <param name="mCC">Recipients to carbon copy.  Comma delimit multiple entries.</param>
    ''' <param name="mBCC">Recipients to blind copy.  Comma delimit multiple entries.</param>
    ''' <param name="mAttach">Attachments for email.  Comma delimit multiple entries.  Send blank string if non are desired.</param>
    ''' <param name="mImportance">The importance level assigned to the email.</param>
    ''' <param name="TestIt">If this indicator is set to true and the application is in test mode then the current user will be added to the to list and "THIS IS A TEST" is added to the subject and cody text.  Was defaulted to True before the C# conversion.</param>
    ''' <param name="AsHTML">Indicator as to whether the email should be sent as HTML or text.</param>
    ''' <remarks></remarks>
    Public Shared Sub SendMailBatchEmail(ByVal testMode As Boolean, ByVal mto As String, ByVal mFrom As String, ByVal mSubject As String, ByVal mBody As String, ByVal mCC As String, ByVal mBCC As String, ByVal mAttach As String, ByVal mImportance As EmailImportanceLevel, ByVal TestIt As Boolean, ByVal AsHTML As Boolean)
        Dim aAttach() As String
        Dim I As Integer
        Dim Email As New OSSMTP.SMTPSession()
        Dim SecondsWaited As Integer
        Dim importanceLevel As importance_level
        Select Case mImportance
            Case EmailImportanceLevel.High
                importanceLevel = importance_level.ImportanceHigh
            Case EmailImportanceLevel.Low
                importanceLevel = importance_level.ImportanceLow
            Case EmailImportanceLevel.Normal
                importanceLevel = importance_level.ImportanceNormal
        End Select

        'set server and time out (default = 10 secs, set to 20 secs)
        Email.Server = "smtp2.utahsbr.edu"
        Email.Timeout = 20000

        'add test text and recipient
        If TestIt AndAlso testMode Then
            mto = mto & "," & Environ("USERNAME") & "@utahsbr.edu"
            mSubject = mSubject & " -- THIS IS A TEST"
            mBody = "THIS IS A TEST" & vbCrLf & vbCrLf & mBody
        End If

        'create message
        If mFrom = "" Then mFrom = Environ("USERNAME") & "@utahsbr.edu"
        Email.MailFrom = mFrom
        Email.SendTo = mto
        Email.CC = mCC
        Email.BCC = mBCC
        Email.MessageSubject = mSubject
        If AsHTML Then
            Email.MessageHTML = mBody
        Else
            Email.MessageText = mBody
        End If
        Email.Importance = importanceLevel

        'add attachments if there are any
        If Len(mAttach) > 0 Then
            'split file names from string
            aAttach = Split(mAttach, ",")

            'add attachments
            For I = 0 To UBound(aAttach)
                Email.Attachments.Add(aAttach(I))
            Next I
        End If

        'send message
        Email.SendEmail()

        'wait up to five seconds for the email to successfully be sent
        While Email.Status <> "SMTP connection closed"
            Threading.Thread.Sleep(New TimeSpan(0, 0, 1))
            SecondsWaited = SecondsWaited + 1
            If SecondsWaited = 5 AndAlso Email.Status <> "SMTP connection closed" Then
                'if the script has waited 5 seconds then give error message and exit function
                Throw New EmailException("Your message was not sent for the following reason:  " & Email.Status & "  Please contact Process Automation for assistance.")
            End If
        End While
        'if the script makes it through loop above then it successfully sent the email and received the all is OK message
    End Sub
#End Region

#Region "SystemBasedOnLoanStatus"
    ''' <summary>
    ''' Checks COMPASS loan status(es) to see which system(s) we would use for a given borrower.
    ''' The return value is a power-of-two enum that can have multiple bits set,
    ''' so it will normally need to be checked using a bit-wise AND.
    ''' </summary>
    Public Shared Function DetermineApplicableSystemBasedOnLoanStatus(ByVal ri As ReflectionInterface, ByVal ssn As String) As AesSystem
        Return DetermineApplicableSystemBasedOnLoanStatus(ri.ReflectionSession, ssn)
    End Function

    ''' <summary>
    ''' Checks COMPASS loan status(es) to see which system(s) we would use for a given borrower.
    ''' The return value is a power-of-two enum that can have multiple bits set,
    ''' so it will normally need to be checked using a bit-wise AND.
    ''' </summary>
    Public Shared Function DetermineApplicableSystemBasedOnLoanStatus(ByVal rs As Session, ByVal ssn As String) As AesSystem
        Dim applicableSystems As AesSystem = AesSystem.None
        Dim loanStatuses As LoanStatus = GetCompassLoanStatuses(rs, ssn)
        If ((loanStatuses And LoanStatus.Closed) = LoanStatus.Closed OrElse (loanStatuses = LoanStatus.None)) Then applicableSystems = applicableSystems Or AesSystem.OneLink
        If ((loanStatuses And LoanStatus.Open) = LoanStatus.Open) Then applicableSystems = applicableSystems Or AesSystem.Compass
        Return applicableSystems
    End Function
#End Region

#Region "TestMode"
    ''' <summary>
    ''' Determine if the user is running the script in test mode and return object with set directory locations
    ''' </summary>
    ''' <param name="DocFolder">The directory where the letter is saved</param>
    ''' <param name="RI">The Reflection Interface with TestMode populated</param>
    ''' <returns>TestModeResults with all object level variables populated</returns>
    ''' <remarks></remarks>
    Public Shared Function TestMode(ByVal docFolder As String, ByRef RI As ReflectionInterface) As TestModeResults
        Return TestMode(docFolder, RI.TestMode)
    End Function

    ''' <summary>
    ''' Determine if the user is running the script in test mode and return object with set directory locations
    ''' </summary>
    ''' <param name="docFolder">The directory where the letter is saved</param>
    ''' <param name="tempTestMode">Is the script operating in Test Mode</param>
    ''' <returns>TestModeResults with all object level variables populated</returns>
    ''' <remarks></remarks>
    Public Shared Function TestMode(ByVal docFolder As String, ByRef tempTestMode As Boolean) As TestModeResults
        Dim ReturnResults As New TestModeResults()
        If tempTestMode Then
            ReturnResults.IsInTestMode = True
            ReturnResults.FtpFolder = DataAccessBase.SASDataFileDirectory + "Test\"
            ReturnResults.DocFolder = docFolder & "Test\"
            ReturnResults.LogFolder = DataAccessBase.RecoveryLogDirectory + "Test\"
        Else
            ReturnResults.IsInTestMode = False
            ReturnResults.FtpFolder = DataAccessBase.SASDataFileDirectory
            ReturnResults.DocFolder = docFolder
            ReturnResults.LogFolder = DataAccessBase.RecoveryLogDirectory
        End If
        Return ReturnResults
    End Function

    ''' <summary>
    ''' Used just to test if in test mode, this is to be delete after promotion of LP22 changes
    ''' </summary>
    ''' <param name="rs"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function IsInTestMode(ByVal rs As Session) As Boolean
        Dim testMode As Boolean = (rs.CommandLineSwitches.ToLower().Contains("tst.rsf") OrElse rs.CommandLineSwitches.ToLower().Contains("dev.rsf"))
        Return testMode
    End Function
#End Region

#Region "Unix"
    ''' <summary>
    ''' Converts a file to use UNIX newline characters (LF) instead of DOS newline characters (CR/LF).
    ''' </summary>
    ''' <param name="fileName">The full path and name of the file to convert.</param>
    ''' <remarks>
    ''' Rather than globally stripping out all CR characters from the file, this function
    ''' explicitly looks for CR/LF sequences and replaces them with LF.
    ''' Hence, if the input file has any CR characters that aren't immediately followed
    ''' by an LF character, they will be left intact.
    ''' </remarks>
    Public Shared Sub Dos2UnixNewline(ByVal fileName As String)
        'Use a temporary file for writing out the converted data.
        Dim tempFile As String = DataAccess.PersonalDataDirectory + "Dos2Unix"
        Try
            Using fileReader As New StreamReader(fileName)
                Using tempWriter As New StreamWriter(tempFile)
                    'Read the first character in the file so that we can
                    'start checking for CR/LF as soon as we enter the loop.
                    Dim previousChar As Char = Convert.ToChar(fileReader.Read())
                    While (Not fileReader.EndOfStream)
                        Dim currentChar As Char = Convert.ToChar(fileReader.Read())
                        If (previousChar = "\r" AndAlso currentChar = "\n") Then
                            'Write out LF and advance past the current LF character
                            'so that it doesn't become the previous character.
                            tempWriter.Write("\n")
                            currentChar = Convert.ToChar(fileReader.Read())
                        Else
                            'Write out the previous character rather than the current one,
                            'since the current one may be the first part of a CR/LF sequence.
                            tempWriter.Write(previousChar)
                        End If
                        'Write out the last character.
                        previousChar = currentChar
                    End While
                    tempWriter.Write(previousChar)
                End Using
            End Using
            'Overwrite the original file with the converted file.
            File.Copy(tempFile, fileName, True)
        Finally
            'Always delete the temporary file.
            File.Delete(tempFile)
        End Try
    End Sub 'Unix2Dos()

    ''' <summary>
    ''' Converts a file to use DOS newline characters (CR/LF) instead of UNIX newline characters (LF).
    ''' </summary>
    ''' <param name="fileName">The full path and name of the file to convert.</param>
    ''' <remarks>
    ''' Rather than globally prepend every LF character with a CR character,
    ''' this function explicitly checks for LF characters already preceded by
    ''' a CR character and will not replace instances where that sequence is found.
    ''' Hence, if the input file is already in DOS (Windows) format or a mixed
    ''' UNIX/DOS format, the result will still be in proper DOS format.
    ''' </remarks>
    Public Shared Sub Unix2DosNewline(ByVal fileName As String)
        'Use a temporary file for writing out the converted data.
        Dim tempFile As String = DataAccess.PersonalDataDirectory + "Unix2Dos"
        Try
            Using fileReader As New StreamReader(fileName)
                Using tempWriter As New StreamWriter(tempFile)
                    Dim previousChar As Char = "\0"
                    While (Not fileReader.EndOfStream)
                        Dim currentChar As Char = Convert.ToChar(fileReader.Read())
                        If (currentChar = "\n" AndAlso previousChar <> "\r") Then
                            tempWriter.Write("\r\n")
                        Else
                            tempWriter.Write(currentChar)
                        End If
                        previousChar = currentChar
                    End While
                End Using
            End Using
            'Overwrite the original file with the converted file.
            File.Copy(tempFile, fileName, True)
        Finally
            'Always delete the temporary file.
            File.Delete(tempFile)
        End Try
    End Sub 'Unix2Dos()
#End Region

#Region "UserID"
    ''' <summary>
    ''' Gets the user's Windows user name from Environment object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function WindowsUserName() As String
        Return Environment.UserName
    End Function

    ''' <summary>
    ''' Maui DUDE needs to use the user ID the user entered on the login screen.  This method sets the value of the _userID property so Maui DUDE can use it.
    ''' Andy says if you use this anywhere but Maui DUDE he will come back to haunt you.
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <remarks></remarks>
    Public Shared Sub SetUserIdFromMauiDude(ByVal userId As String)
        _userID = userId
    End Sub
#End Region

#Region "VBAStyleFileHandling"
    ''' <summary>
    ''' Replicates VBA method for opening a file
    ''' </summary>
    ''' <param name="fileName">File name</param>
    ''' <param name="fileNumber">File handle number</param>
    ''' <param name="openMode">Open mode</param>
    ''' <remarks></remarks>
    Public Shared Sub VbaStyleFileOpen(ByVal fileName As String, ByVal fileNumber As Integer, ByVal openMode As MSOpenMode)
        FileOpen(fileNumber, fileName, openMode)
    End Sub

    ''' <summary>
    ''' Replicates VBA method for writing to a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <param name="fields">Fields to add</param>
    ''' <remarks></remarks>
    Public Shared Sub VbaStyleFileWrite(ByVal fileNumber As Integer, ByVal ParamArray fields() As String)
        Write(fileNumber, fields)
    End Sub

    ''' <summary>
    ''' Replicates VBA method for writing a line to a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <param name="output">Fields to output</param>
    ''' <remarks></remarks>
    Public Shared Sub VbaStyleFileWriteLine(ByVal fileNumber As Integer, ByVal ParamArray output() As String)
        WriteLine(fileNumber, output)
    End Sub

    ''' <summary>
    ''' Replicates VBA method for inputing a field from a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function VbaStyleFileInput(ByVal fileNumber As Integer) As String
        Dim field As String = String.Empty
        Input(fileNumber, field)
        Return field
    End Function

    ''' <summary>
    ''' Replicates VBA method for inputing a line from a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function VbaStyleFileLineInput(ByVal fileNumber) As String
        Return LineInput(fileNumber)
    End Function

    ''' <summary>
    ''' Replicates VBA method for closing a file.
    ''' </summary>
    ''' <param name="fileNumbers">File handle numbers</param>
    ''' <remarks></remarks>
    Public Shared Sub VbaStyleFileClose(ByVal ParamArray fileNumbers() As Integer)
        FileClose(fileNumbers)
    End Sub

    ''' <summary>
    ''' Replicates VBA method for closing a file
    ''' </summary>
    ''' <param name="fileNumber">File handle number</param>
    ''' <returns>Whether at end of file or not</returns>
    ''' <remarks></remarks>
    Public Shared Function VbaStyleEOF(ByVal fileNumber) As Boolean
        Return EOF(fileNumber)
    End Function

#End Region





    Public Shared Function DeleteActivityComments(ByVal ri As ReflectionInterface, ByVal ssn As String, ByVal arc As String, ByVal fromDate As DateTime, ByVal toDate As DateTime) As DeleteActivityCommentsResults
        ri.FastPath("TX3Z/CTD2A{0}", ssn)
        ri.PutText(11, 65, arc)
        ri.PutText(21, 16, fromDate.ToString("MMddyy"))
        ri.PutText(21, 30, toDate.ToString("MMddyy"))
        ri.Hit(ReflectionInterface.Key.Enter)
        'Check for ARC error.
        Select Case ri.GetText(23, 2, 78)
            Case "01488 ACTION RESPONSE CODE NOT FOUND OR IS INACTIVE"
                Return DeleteActivityCommentsResults.ArcNotFoundOrInactive
            Case "01080 SSN ENTERED IS NOT A BORROWER"
                Return DeleteActivityCommentsResults.SsnIsNotABorrower
            Case "01019 ENTERED KEY NOT FOUND"
                Return DeleteActivityCommentsResults.SsnNotFound
            Case Else
                If (ri.Check4Text(1, 72, "TDX2C")) Then
                    ri.PutText(5, 14, "X", ReflectionInterface.Key.Enter)
                    While (Not ri.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                        ri.PutText(6, 18, "E", ReflectionInterface.Key.Enter)
                        ri.Hit(ReflectionInterface.Key.F8)
                    End While
                Else
                    ri.PutText(6, 18, "E", ReflectionInterface.Key.Enter)
                End If
                Return DeleteActivityCommentsResults.Success
        End Select
    End Function 'DeleteActivityComments()


    ''' <summary>
    ''' This function tries to create a temporary file to see if the user has write access in the current directory
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function HasWriteAccess() As Boolean
        Dim TempFileName As String
        'Set random number generator
        Randomize()
        'Setup Error Handler in case user doesn't have write access.
        Try
            'Create a temporary file that is named "TempFileName" & [a number between 0 and 1000]
            TempFileName = "Tempfile" & Int((1000 - 0 + 1) * Rnd() + 0)
            'Try and create file if an error occures goto ErrorHandler else
            'close the file and delete it.
            FileOpen(3, CurDir() & "\" & TempFileName, OpenMode.Output)
            FileClose(3)
            Kill(CurDir() & "\" & TempFileName)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Checks to see if the string passed in only has numbers in it.
    ''' </summary>
    ''' <param name="str">String to test against</param>
    ''' <returns>True if all characters in the passed in string are numbers else false.</returns>
    ''' <remarks></remarks>
    Function IsNum(ByVal str As String) As Boolean
        If String.IsNullOrEmpty(str) Then Return False
        Dim justTheLetters As String = str
        justTheLetters = justTheLetters.Replace("1", "")
        justTheLetters = justTheLetters.Replace("2", "")
        justTheLetters = justTheLetters.Replace("3", "")
        justTheLetters = justTheLetters.Replace("4", "")
        justTheLetters = justTheLetters.Replace("5", "")
        justTheLetters = justTheLetters.Replace("6", "")
        justTheLetters = justTheLetters.Replace("7", "")
        justTheLetters = justTheLetters.Replace("8", "")
        justTheLetters = justTheLetters.Replace("9", "")
        justTheLetters = justTheLetters.Replace("0", "")
        Return String.IsNullOrEmpty(justTheLetters)
    End Function


    ''' <summary>
    ''' Determine if the address is a PO Box.
    ''' </summary>
    ''' <param name="address">The address</param>
    ''' <returns>True if is a PO Box else False.</returns>
    ''' <remarks></remarks>
    Public Shared Function IsPOBox(ByVal address As String) As Boolean
        'convert address to all uppercase to avoid case sensitivity problems
        address = address.ToUpper()
        If address.Contains("PO BOX") Then Return True
        If address.Contains("P.O. BOX") Then Return True
        If address.Contains("P O BOX") Then Return True
        If address.Contains("P.O BOX") Then Return True
        If address.Contains("POBOX") Then Return True
        If address.Contains("P.O.BOX") Then Return True
        If address.Contains("P/O BOX") Then Return True
        Return False
    End Function


    ''' <summary>
    ''' Check whether a given lender ID is affiliated with a given lender.
    ''' </summary>
    ''' <param name="lenderCode">The lender ID to check for.</param>
    ''' <param name="testMode">True if the application is running in test mode.</param>
    ''' <param name="lenderName">The name of the lender.</param>
    ''' <returns>True if the lender ID in question is affiliated with the given lender.</returns>
    ''' <remarks>
    ''' Some lenders may have multiple IDs that are used for different purposes.
    ''' This function, coupled with a database table, allows client code to know whether an ID matches a lender.
    ''' </remarks>
    Public Shared Function IsAffiliatedLenderCode(ByVal lenderCode As String, ByVal testMode As Boolean, Optional ByVal lenderName As String = "UHEAA") As Boolean
        Return DataAccess.GetAffiliatedLenderIds(lenderName, testMode).Contains(lenderCode)
    End Function


    ''' <summary>
    ''' Throws EndDLLException to end DLL functionality (this should be caught and handled by your main method)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub EndDLLScript()
        Throw New EndDLLException("This exception is throw to allow the script to exit gracefully.  Please contact a member of Systems Support if you recieve it.")
    End Sub


    'COMMMENTED OUT BECAUSE THE FILE IS GOING AWAY AND IN ORDER TO THE SAME DATA FROM WHAT WE ARE DOING NOW WE WOULD NEED TO PASS IN THE 
    'RS OBJECT SO THIS SUB COULD PULL THE USER ID FROM LP40.  COMMENTED OUT 01/08/2010 BY AA.  IF NO PROBLEMS FOR A YEAR OR SO THEN THIS
    'CAN BE DELETED.
    ''''''''' <summary>
    ''''''''' Retrieves user information from userinfo.txt file.
    ''''''''' </summary>
    ''''''''' <returns>Populated UserInfoData object.</returns>
    ''''''''' <remarks></remarks>
    '' '' ''Public Shared Function GetUserInfoFromPhysicalFile() As UserInfoData
    '' '' ''    Return DataAccess.GetUserInfoFromTextFileForDataAccess()
    '' '' ''End Function


    ''' <summary>
    ''' Pauses script for user to do stuff and allows them to hit Insert when they are ready for the script to resume.
    ''' </summary>
    ''' <param name="rs">Reflection Session</param>
    ''' <remarks></remarks>
    Public Shared Sub PauseForInsert(ByVal rs As Session)
        rs.WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
        ReflectionInterface.Hit(rs, ReflectionInterface.Key.Insert)
    End Sub


    ''' <summary>
    ''' Returns the Reflection Sessions Cursor location.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SessionCursorLocation(ByVal ri As ReflectionInterface) As Coordinate
        Dim c As New Coordinate()
        c.Column = ri.ReflectionSession.CursorColumn
        c.Row = ri.ReflectionSession.CursorRow
        Return c
    End Function


    ''' <summary>
    ''' STUP to the specified region (change the region the user is logged in to).
    ''' </summary>
    ''' <param name="rs"></param>
    ''' <param name="region"></param>
    ''' <param name="testMode"></param>
    ''' <remarks></remarks>
    Public Shared Sub STUP(ByVal rs As Session, ByVal region As ScriptSessionBase.Region, ByVal testMode As Boolean)
        Dim fastPathText As String = ""

        'determine text to put in the fastpath based on the region to STUP to and the test mode
        If region = ScriptSessionBase.Region.UHEAA Then
            If Not testMode Then
                fastPathText = "STUPUT"
            Else
                fastPathText = "STUPQ0RS"
            End If
        ElseIf region = ScriptSessionBase.Region.CornerStone Then
            If Not testMode Then
                fastPathText = "STUPKU"
            Else
                fastPathText = "STUPVUK3"
            End If
        Else
            Throw New StupRegionSpecifiedException("You must specify 'ScriptSessionBase.Region.UHEAA' or 'ScriptSessionBase.Region.CornerStone' to use the STUP function.")
        End If

        'STUP to the desired region through the fastpath
        ReflectionInterface.FastPath(rs, fastPathText)
        If region = ScriptSessionBase.Region.CornerStone Then ReflectionInterface.Hit(rs, ReflectionInterface.Key.F10)
    End Sub

End Class
