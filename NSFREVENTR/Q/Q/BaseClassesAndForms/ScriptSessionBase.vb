Imports Q.ReflectionInterface
Imports Reflection
Imports Q.Common
Imports Q.DocumentHandling

Public MustInherit Class ScriptSessionBase
    Inherits ScriptCommonBase

    Public Enum Region
        UHEAA
        CornerStone
        None
    End Enum

    Private _ri As ReflectionInterface
    Private _rs As Session

    ''' <summary>
    ''' Reflection Interface
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property RI() As ReflectionInterface
        Get
            Return _ri
        End Get
    End Property

    ''' <summary>
    ''' Reflection Session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property RS() As Session
        Get
            Return _rs
        End Get
    End Property

    ''' <summary>
    ''' Constructor for Script Assistant Object
    ''' </summary>
    ''' <param name="tempRI">Reflection Interface</param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal tempRI As ReflectionInterface)
        MyBase.New(tempRI.TestMode)
        _ri = tempRI
        _rs = _ri.ReflectionSession
    End Sub

    ''' <summary>
    ''' Add a queue task.
    ''' </summary>
    ''' <param name="ssn">Target SSN</param>
    ''' <param name="workgroup">Workgroup ID</param>
    ''' <param name="dateDue">Date due (was defaulted to "")</param>
    ''' <param name="comment1">First comment line (was defaulted to "")</param>
    ''' <param name="comment2">Second comment line (was defaulted to "")</param>
    ''' <param name="comment3">Third comment line (was defaulted to "")</param>
    ''' <param name="comment4">Fourth comment line (was defaulted to "")</param>
    ''' <returns>True if the queue task was successfully added.</returns>
    Protected Function AddQueueTaskInLP9O(ByVal ssn As String, ByVal workgroup As String, ByVal dateDue As String, ByVal comment1 As String, ByVal comment2 As String, ByVal comment3 As String, ByVal comment4 As String) As Boolean
        Return Common.AddQueueTaskInLP9O(_rs, ssn, workgroup, dateDue, comment1, comment2, comment3, comment4)
    End Function

    ''' <summary>
    ''' Add a queue task.
    ''' </summary>
    ''' <param name="ssn">Target SSN</param>
    ''' <param name="workgroup">Workgroup ID</param>
    ''' <param name="dateDue">Date due (was defaulted to "")</param>
    ''' <param name="comment">First comment line (was defaulted to "")</param>
    ''' <returns>True if the queue task was successfully added.</returns>
    Protected Function AddQueueTaskInLP9O(ByVal ssn As String, ByVal workgroup As String, ByVal dateDue As String, ByVal comment As String) As Boolean
        Return Common.AddQueueTaskInLP9O(_rs, ssn, workgroup, dateDue, comment, "", "", "")
    End Function

    ''' <summary>
    ''' Add a queue task.
    ''' </summary>
    ''' <returns>True if the queue task was successfully added.</returns>
    Protected Function AddQueueTaskInLP9O(ByVal ssn As String, ByVal workgroup As String, ByVal dateDue As DateTime, ByVal comment As String) As Boolean
        Return Common.AddQueueTaskInLP9O(_rs, ssn, workgroup, dateDue.ToString("MM/dd/yyyy"), comment, "", "", "")
    End Function

    ''' <summary>
    ''' Add a queue task.
    ''' </summary>
    ''' <param name="ssn">Target SSN</param>
    ''' <param name="workgroup">Workgroup ID</param>
    ''' <param name="comment">First comment line (was defaulted to "")</param>
    ''' <returns>True if the queue task was successfully added.</returns>
    Protected Function AddQueueTaskInLP9O(ByVal ssn As String, ByVal workgroup As String, ByVal comment As String) As Boolean
        Return Common.AddQueueTaskInLP9O(_rs, ssn, workgroup, "", comment, "", "", "")
    End Function

    ''' <summary>
    ''' Compares the provided text to the text found in the coordinates provided
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be tested against</param>
    ''' <returns>Returns true if the strings match else false</returns>
    Protected Function Check4Text(ByVal row As Integer, ByVal column As Integer, ByVal ParamArray text() As String) As Boolean
        Return _ri.Check4Text(row, column, text)
    End Function

    ''' <summary>
    ''' Checks COMPASS loan status(es) to see which system(s) we would use for a given borrower.
    ''' The return value is a power-of-two enum that can have multiple bits set,
    ''' so it will normally need to be checked using a bit-wise AND.
    ''' </summary>
    Protected Function DetermineApplicableSystemBasedOnLoanStatus(ByVal ssn As String) As AesSystem
        Return Common.DetermineApplicableSystemBasedOnLoanStatus(_rs, ssn)
    End Function

    ''' <summary>
    ''' Clears screen and enters provided text into the Fast Path
    ''' </summary>
    ''' <param name="input">Text to be entered into the Fast Path</param>
    Protected Sub FastPath(ByVal input As String)
        _ri.FastPath(input)
    End Sub

    ''' <summary>
    ''' Clears screen and enters provided text into the Fast Path
    ''' </summary>
    ''' <param name="format">Text to be entered into the Fast Path</param>
    ''' <param name="args">Values to be inserted into the format strings placeholders, if any.</param>
    Protected Sub FastPath(ByVal format As String, ByVal ParamArray args() As Object)
        _ri.FastPath(format, args)
    End Sub

    ''' <summary>
    ''' Attempts to find the given text on the Reflection screen, starting from the upper left corner.
    ''' </summary>
    ''' <param name="text">The text to find.</param>
    ''' <returns>
    ''' A Coordinate object whose Row and Column properties point to the beginning of the found text,
    ''' or null if the text is not found.
    ''' </returns>
    Protected Function FindText(ByVal text As String) As Coordinate
        Return _ri.FindText(text)
    End Function

    ''' <summary>
    ''' Attempts to find the given text on the Reflection screen, starting from a specified position.
    ''' </summary>
    ''' <param name="text">The text to find.</param>
    ''' <param name="startRow">The row where the search will start.</param>
    ''' <param name="startColumn">The column where the search will start.</param>
    ''' <returns>
    ''' A Coordinate object whose Row and Column properties point to the beginning of the found text,
    ''' or null if the text is not found.
    ''' </returns>
    Protected Function FindText(ByVal text As String, ByVal startRow As Integer, ByVal startColumn As Integer) As Coordinate
        Return _ri.FindText(text, startRow, startColumn)
    End Function

    Protected Sub EnterText(ByVal text As String)
        _ri.EnterText(text)
    End Sub

    ''' <summary>
    ''' Returns the current cursor coordinate.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetCurrentCursorCoordinate() As Coordinate
        Return _ri.GetCurrentCursorCoordinate()
    End Function

    ''' <summary>
    ''' Get demographic information from screen LP22.
    ''' </summary>
    ''' <param name="ssnOrAcctNum">Borrower SSN or Account Number</param>
    ''' <returns>Populated BorrowerDemographics Object</returns>
    Protected Function GetDemographicsFromLP22(ByVal ssnOrAcctNum As String) As SystemBorrowerDemographics
        Return Common.GetDemographicsFromLP22(_rs, ssnOrAcctNum)
    End Function

    ''' <summary>
    ''' Get demographic information from screen TX1J.
    ''' </summary>
    ''' <param name="ssnOrAcctNum">The SSN or Account Number</param>
    ''' <returns>A populated BorrowerDemograhics object</returns>
    Protected Function GetDemographicsFromTX1J(ByVal ssnOrAcctNum As String) As SystemBorrowerDemographics
        Return Common.GetDemographicsFromTX1J(_rs, ssnOrAcctNum)
    End Function

    ''' <summary>
    ''' Gets an employer's name and address from LPEM.
    ''' </summary>
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
    Protected Function GetEmployerInfoFromLPEM(ByVal employerId As String, ByVal ParamArray preferredDepartments() As String) As EmployerDemographics
        Return Common.GetEmployerInfoFromLPEM(RS, employerId, preferredDepartments)
    End Function

    ''' <summary>
    ''' Retrieves the screen contents from the specified row and column for the full length requested.
    ''' Same as GetText, except that nothing is trimmed from either end.
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="length">The number of characters to pull from the screen</param>
    ''' <returns>The contents pulled from location specified</returns>
    Protected Function GetDisplayText(ByVal row As Integer, ByVal column As Integer, ByVal length As Integer) As String
        Return _ri.GetDisplayText(row, column, length)
    End Function

    ''' <summary>
    ''' Retrieves the text from the specifed row, column, length coordinates (this function trims off empty spaces on the front and back of the string)
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="length">The number of characters to pull from the screen (function trims spaces before returning)</param>
    ''' <returns>The text pulled from location specified minus any spaces on the front or back</returns>
    ''' <remarks></remarks>
    Protected Function GetText(ByVal row As Integer, ByVal column As Integer, ByVal length As Integer) As String
        Return _ri.GetText(row, column, length)
    End Function

    ''' <summary>
    ''' Gets the user's ID from the AES system.
    ''' </summary>
    ''' <remarks>Unlike the LP40 version, this function uses the PROF screen, which is independent of any regions.</remarks>
    Protected Function GetUserId() As String
        RI.FastPath("PROF")
        Return RI.GetText(2, 49, 7)
    End Function

    ''' <summary>
    ''' Gets the user's ID from OneLINK's LP40 screen.
    ''' </summary>
    Protected Function GetUserIDFromLP40() As String
        Return Common.GetUserIDFromLP40(_rs)
    End Function

    ''' <summary>
    ''' Transmits a key
    ''' </summary>
    ''' <param name="keyToHit">The key to transmit</param>
    ''' <returns>Returns true if the key is in the defined Key enumeration</returns>
    Protected Function Hit(ByVal keyToHit As ReflectionInterface.Key) As Boolean
        Return _ri.Hit(keyToHit)
    End Function

    ''' <summary>
    ''' Checks to see if the user is logged in.
    ''' </summary>
    ''' <returns>returns True if the user is logged in (LP40 screen is found), else false</returns>
    Protected Function IsLoggedIn() As Boolean
        Return Common.IsLoggedIn(RS)
    End Function

    ''' <summary>
    ''' Enters the given text in the coordinates given
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be entered</param>
    Protected Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String)
        _ri.PutText(row, column, text)
    End Sub

    ''' <summary>
    ''' Enters the given text in the coordinates given
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be entered</param>
    ''' <param name="blankFieldFirst">If true, the field will be blanked out before putting in text.</param>
    Protected Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal blankFieldFirst As Boolean)
        _ri.PutText(row, column, text, blankFieldFirst)
    End Sub

    ''' <summary>
    ''' Enters the given text in the coordinates given
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be entered</param>
    ''' <param name="keyToHit">The key to transmit</param>
    Protected Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal keyToHit As ReflectionInterface.Key)
        _ri.PutText(row, column, text, keyToHit)
    End Sub

    ''' <summary>
    ''' Enters the given text in the coordinates given
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be entered</param>
    ''' <param name="keyToHit">The key to transmit</param>
    ''' <param name="blankFieldFirst">If true, the field will be blanked out before putting in text.</param>
    Protected Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal keyToHit As ReflectionInterface.Key, ByVal blankFieldFirst As Boolean)
        _ri.PutText(row, column, text, keyToHit, blankFieldFirst)
    End Sub

    Protected Sub PauseForInsert()
        Common.PauseForInsert(RS)
    End Sub

    ''' <summary>
    ''' Enter comments in LP50.  Only use this version of the method if you are calling the function from a object that only inherits from ScriptSessionBase.
    ''' </summary>
    ''' <param name="ssn">The person ID: SSN for borrowers, co-borrowers, students; reference ID for references.</param>
    ''' <param name="arc">Action code</param>
    ''' <param name="scriptID">The script ID from Sacker.</param>
    ''' <param name="activityType">Activity type</param>
    ''' <param name="contactType">Contact type</param>
    ''' <param name="comment">Activity comments</param>
    ''' <param name="pauseForUserComments">True if the script should pause to allow the user to enter additional comments.</param>
    ''' <param name="isReference">True if the SSN should go in the ASSOCIATED PERSON ID space.</param>
    ''' <returns>True if the activity comment was successfully added.</returns>
    Protected Function AddCommentInLP50(ByVal ssn As String, ByVal arc As String, ByVal scriptID As String, ByVal activityType As String, ByVal contactType As String, ByVal comment As String, ByVal pauseForUserComments As Boolean, ByVal isReference As Boolean) As Boolean
        Return Common.AddCommentInLP50(RS, ssn, arc, scriptID, activityType, contactType, comment, pauseForUserComments, isReference)
    End Function

    ''' <summary>
    ''' Enter comments in LP50. Only use this version of the method if you are calling the function from a object that only inherits from ScriptSessionBase.
    ''' </summary>
    ''' <param name="ssn">The person ID: SSN for borrowers, co-borrowers, students; reference ID for references.</param>
    ''' <param name="arc">Action code</param>
    ''' <param name="scriptID">The script ID from Sacker.</param>
    ''' <param name="activityType">Activity type</param>
    ''' <param name="contactType">Contact type</param>
    ''' <param name="comment">Activity comments</param>
    ''' <returns>True if the activity comment was successfully added.</returns>
    Protected Function AddCommentInLP50(ByVal ssn As String, ByVal arc As String, ByVal scriptID As String, ByVal activityType As String, ByVal contactType As String, ByVal comment As String) As Boolean
        Return Common.AddCommentInLP50(RS, ssn, arc, scriptID, activityType, contactType, comment, False, False)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans.  Only use this version of the method if you are calling the function from a object that only inherits from ScriptSessionBase.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptID">The script ID from Sacker.</param>
    ''' <param name="pauPls">Pause to allow user to add comments (was defaulted to false)</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Protected Function ATD22AllLoans(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptID As String, ByVal pauPls As Boolean) As Common.CompassCommentScreenResults
        Return Common.ATD22AllLoans(RS, ssn, arc, comment, scriptID, pauPls)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans.  Only use this version of the method if you are calling the function from a object that only inherits from ScriptSessionBase.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptID">The script ID from Sacker.</param>
    ''' <param name="pauPls">Pause to allow user to add comments (was defaulted to false)</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Protected Function ATD22AllLoans(ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, ByVal scriptID As String, ByVal pauPls As Boolean) As Common.CompassCommentScreenResults
        Return Common.ATD22AllLoans(RS, ssn, arc, neededBy, comment, scriptID, pauPls)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans specified.  Only use this version of the method if you are calling the function from a object that only inherits from ScriptSessionBase.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">The loan sequence numbers that should be selected on TD22</param>
    ''' <param name="scriptID">The script ID from Sacker.</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments (was defaulted to false)</param>
    Protected Function ATD22ByLoan(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As System.Collections.Generic.List(Of Integer), ByVal scriptID As String, ByVal pauseForManualComments As Boolean) As Boolean
        Return Common.ATD22ByLoan(RS, ssn, arc, comment, loanSequenceNumbers, scriptID, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans in specified loan programs.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="options">Use the OR operator ("|" in C#) to string together multiple options</param>
    ''' <param name="loanPrograms">Loan programs that should be included in the selection</param>
    Protected Function ATD22ByLoanProgram(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptId As String, ByVal options As Common.TD22Options, ByVal ParamArray loanPrograms() As String) As Boolean
        Return Common.ATD22ByLoanProgram(RS, ssn, arc, comment, scriptId, options, loanPrograms)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans with a balance.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments</param>
    Protected Function ATD22ByBalance(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptId As String, ByVal pauseForManualComments As Boolean) As Boolean
        Return Common.ATD22ByBalance(RS, ssn, arc, comment, scriptId, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans with a balance.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments</param>
    ''' <param name="recipientId">Recipient SSN</param>
    Protected Function ATD22ByBalance(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptId As String, ByVal pauseForManualComments As Boolean, ByVal recipientId As String, ByVal inRegardsTo As TD22.RegardsTo, ByVal regardsToId As String) As CompassCommentScreenResults
        Return New TD22(RI).ATD22ByBalance(ssn, arc, comment, scriptId, pauseForManualComments, , , , recipientId, inRegardsTo, regardsToId)
    End Function

    ''' <summary>
    ''' This function tries to add the comment on TD37 for all loan applications.  Only use this version of the method if you are calling the function from a object that only inherits from ScriptSessionBase.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="scriptID">The script ID from Sacker.</param>
    ''' <param name="pauseForManualComments">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Protected Function ATD37AllLoans(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal scriptID As String, Optional ByVal pauseForManualComments As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD37AllLoans(RS, ssn, arc, comment, scriptID, pauseForManualComments)
    End Function

    ''' <summary>
    ''' This function tries to add the comment on TD37 for passed in app #'s.  Only use this version of the method if you are calling the function from a object that only inherits from ScriptSessionBase.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="apps">Array of app numbers</param>
    ''' <param name="scriptID">The script ID from Sacker.</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments (was defaulted to false)</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Protected Function ATD37ByLoan(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal apps As System.Collections.Generic.List(Of Integer), ByVal scriptID As String, ByVal pausePlease As Boolean) As Common.CompassCommentScreenResults
        Return Common.ATD37ByLoan(RS, ssn, arc, comment, apps, scriptID, pausePlease)
    End Function

    ''' <summary>
    ''' this function tries to add the comment on TD37 for the first app listed
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Protected Function ATD37FirstLoan(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, ByVal pausePlease As Boolean) As CompassCommentScreenResults
        Return Common.ATD37FirstLoan(RS, ssn, arc, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' this function tries to add the comment on TD37 for the first app listed
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Protected Function ATD37FirstLoan(ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, ByVal script As String, ByVal pausePlease As Boolean) As CompassCommentScreenResults
        Return Common.ATD37FirstLoan(RS, ssn, arc, neededBy, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Protected Function ATD22AllLoansBackedUpWithATD37FirstApp(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22AllLoansBackedUpWithATD37FirstApp(RI, ssn, arc, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Protected Function ATD22AllLoansBackedUpWithATD37FirstApp(ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22AllLoansBackedUpWithATD37FirstApp(RI, ssn, arc, neededBy, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">Loan sequence numbers to be marked on TD22.</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Protected Function ATD22ByLoanBackedUpWithATD37FirstApp(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As List(Of Integer), ByVal scriptId As String, Optional ByVal pauseForManualComments As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22ByLoanBackedUpWithATD37FirstApp(RI, ssn, arc, comment, loanSequenceNumbers, scriptId, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="script">Script ID from Sacker</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Protected Function ATD22AllLoansBackedUpWithATD37AllApps(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal script As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22AllLoansBackedUpWithATD37AllApps(RI, ssn, arc, comment, script, pausePlease)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">Loan sequence numbers to be marked on TD22.</param>
    ''' <param name="scriptId">Script ID from Sacker</param>
    ''' <param name="pauseForManualComments">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Protected Function ATD22ByLoanBackedUpWithATD37AllApps(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As List(Of Integer), ByVal scriptId As String, Optional ByVal pauseForManualComments As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22ByLoanBackedUpWithATD37AllApps(RI, ssn, arc, comment, loanSequenceNumbers, scriptId, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Main entry point that calls all helper functions as needed to accomplish Centralized printing, adding the static current date and adding the 2D barcode.  FOR USER SCRIPTS ONLY.  Only use this version of the method if you are calling the function from a object that only inherits from ScriptSessionBase.
    ''' </summary>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="systemToAddCommentsTo">System to add comments to (OneLINK or Compass)</param>
    ''' <param name="ssn">SSN of the borrower</param>
    ''' <param name="dataFile">The data file for the merge operation (Must have one data row)</param>
    ''' <param name="acctNumOrRefIDFieldNm">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="scriptID">The script ID from Sacker.</param>
    ''' <param name="stateCodeFieldNm">Field name for the state code field</param>
    ''' <param name="passedInDeployMethod">Desired deployment method.  If anything other than User Prompt is passsed then the user is required to use that method else the user will be given a prompt to choose a method and provide the needed information for the chosen method.</param>
    ''' <param name="ContactType">The contact type to be used when adding notes to the systems (OneLINK or Compass).  If not specified in the spec "03" should be used (was the default before moving to C#.</param>
    ''' <param name="LetterRecip">The recipient of the letter.  If not specified in the spec "borrower" should be used (was default before moving to C#).</param>
    ''' <remarks>This method expects the data file that is sent to it to have a single data row in it</remarks>
    Protected Sub GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ByVal letterID As String, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal scriptID As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal ContactType As String, ByVal LetterRecip As Barcode2DLetterRecipient)
        Q.DocumentHandling.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(RI, letterID, systemToAddCommentsTo, ssn, dataFile, acctNumOrRefIDFieldNm, scriptID, stateCodeFieldNm, passedInDeployMethod, ContactType, LetterRecip)
    End Sub

    ''' <summary>
    ''' Checks if the borrower has open loans on OneLINK.
    ''' </summary>
    ''' <param name="ssn">Borrower's SSN.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function HasOpenLoanOnOneLINK(ByVal ssn As String) As Boolean
        Return Common.HasOpenLoanOnOneLINK(RI, ssn)
    End Function

    ''' <summary>
    ''' Send an e-mail message using SMTP.
    ''' </summary>
    ''' <param name="recipient">Email address of who to send the email to.  Comma delimit multiple entries.</param>
    ''' <param name="sender">Who the email is being sent from.  Send "" to use the logged-in user.</param>
    ''' <param name="subject">The email subject line.</param>
    ''' <param name="body">The email body text.</param>
    ''' <param name="cc">Recipients to carbon copy.  Comma delimit multiple entries.</param>
    ''' <param name="bcc">Recipients to blind copy.  Comma delimit multiple entries.</param>
    ''' <param name="attachment">Attachments for email.  Comma delimit multiple entries.  Send blank string if non are desired.</param>
    ''' <param name="importance">The importance level assigned to the email.</param>
    Protected Overloads Sub SendMail(ByVal recipient As String, ByVal sender As String, ByVal subject As String, ByVal body As String, ByVal cc As String, ByVal bcc As String, ByVal attachment As String, ByVal importance As EmailImportanceLevel)
        MyBase.SendMail(RI.TestMode, recipient, sender, subject, body, cc, bcc, attachment, importance, RI.TestMode)
    End Sub

    ''' <summary>
    ''' Send an e-mail message using SMTP (provides option of HTML or plain text).
    ''' </summary>
    ''' <param name="recipient">Email address of who to send the email to.  Comma delimit multiple entries.</param>
    ''' <param name="sender">Who the email is being sent from.  Send "" to use the logged-in user.</param>
    ''' <param name="subject">The email subject line.</param>
    ''' <param name="body">The email body text.</param>
    ''' <param name="cc">Recipients to carbon copy.  Comma delimit multiple entries.</param>
    ''' <param name="bcc">Recipients to blind copy.  Comma delimit multiple entries.</param>
    ''' <param name="attachment">Attachments for email.  Comma delimit multiple entries.  Send blank string if non are desired.</param>
    ''' <param name="importance">The importance level assigned to the email.</param>
    ''' <param name="AsHTML">Indicator as to whether the email should be sent as HTML or text.</param>
    Protected Overloads Sub SendMail(ByVal recipient As String, ByVal sender As String, ByVal subject As String, ByVal body As String, ByVal cc As String, ByVal bcc As String, ByVal attachment As String, ByVal importance As EmailImportanceLevel, ByVal asHtml As Boolean)
        MyBase.SendMail(RI.TestMode, recipient, sender, subject, body, cc, bcc, attachment, importance, RI.TestMode, asHtml)
    End Sub

    ''' <summary>
    ''' Returns the Reflection Sessions Cursor location.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function SessionCursorLocation() As Coordinate
        Return Common.SessionCursorLocation(RI)
    End Function



    ''' <summary>
    ''' Checks the User/Action Qualification screen to see if the user is set up for the ARC.
    ''' </summary>
    ''' <param name="userId">The UT ID of the user to check.</param>
    ''' <param name="arc">The action request code (ARC) to check for the user.</param>
    ''' <remarks>This method simply goes to ITX68 for the given user ID and ARC and checks whether it ended up on screen TXX6C.</remarks>
    Protected Function UserHasAccessToArc(ByVal userId As String, ByVal arc As String) As Boolean
        FastPath("TX3Z/ITX68{0};{1}", userId, arc)
        Return Check4Text(1, 72, "TXX6C")
    End Function

    Protected Sub ValidateRegion(ByVal regionToValidate As Region)
        If regionToValidate = Region.None Then Return
        Dim regionIsValid As Boolean
        FastPath("TX3Z/ITX1J")
        Select Case regionToValidate
            Case Region.UHEAA
                regionIsValid = Check4Text(1, 39, "UHEAA")
            Case Region.CornerStone
                regionIsValid = Check4Text(1, 38, "UHEAAFED")
        End Select
        If Not regionIsValid Then
            Dim message As String = String.Format("You must be in the {0} region to use this script.", [Enum].GetName(GetType(Region), regionToValidate))
            Throw New StupRegionSpecifiedException(message)
        End If
    End Sub

    ''' <summary>
    ''' Pass the user ID the user entered in the Maui DUDE login screen to the common code so the common code uses the Maui DUDE user ID rather than going to LP40 for it.
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <remarks></remarks>
    Public Shared Sub SetUserIdFromMauiDude(ByVal userId As String)
        Common.SetUserIdFromMauiDude(userId)
    End Sub

    ''' <summary>
    ''' Call the common function to STUP to the specified region (change the region the user is logged in to).
    ''' </summary>
    ''' <param name="region"></param>
    ''' <remarks></remarks>
    Protected Sub STUP(ByVal region As ScriptSessionBase.Region)
        Common.STUP(RS, region, TestModeProperty)
    End Sub
End Class
