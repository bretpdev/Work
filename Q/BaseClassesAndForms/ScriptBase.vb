Imports Q.ReflectionInterface
Imports Reflection
Imports Q.Common
Imports Q.DocumentHandling
Imports Uheaa.Common.ProcessLogger
Imports System.Reflection

Public MustInherit Class ScriptBase
    Inherits ScriptSessionBase

    Private _calledByMauiDUDE As Boolean = False
    Private _mauiDUDEBorrower As MDBorrower
    Private _runNumber As Integer
    Private _scriptID As String

    Protected _enterpriseFileSystem As EnterpriseFileSystem
    Protected ReadOnly Property Efs() As EnterpriseFileSystem
        Get
            Return _enterpriseFileSystem
        End Get
    End Property


    ''' <summary>
    ''' The run number for the script if called by DUDE.  1 is the first run and 2 the second run.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property RunNumber() As Integer
        Get
            Return _runNumber
        End Get
    End Property

    ''' <summary>
    ''' Was the script called by Maui DUDE.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property CalledByMauiDUDE() As Boolean
        Get
            Return _calledByMauiDUDE
        End Get
    End Property

    ''' <summary>
    ''' Maui DUDE Borrower Object passed by MD.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property MauiDUDEBorrower() As MDBorrower
        Get
            Return _mauiDUDEBorrower
        End Get
    End Property

    ''' <summary>
    ''' Is passed between DUDE and the script so DUDE knows that the given script has completed successfully
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScriptCompletedSuccessfully() As Boolean
        Get
            Return _mauiDUDEBorrower.ScriptInfoToGenericBusinessUnit.ScriptCompletedSuccessfully
        End Get
        Set(ByVal value As Boolean)
            _mauiDUDEBorrower.ScriptInfoToGenericBusinessUnit.ScriptCompletedSuccessfully = value
        End Set
    End Property

    ''' <summary>
    ''' Script ID given originally to constructor.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ScriptID() As String
        Get
            If _scriptID Is Nothing Then
                Throw New System.Exception("You used the wrong constructor.  Please use the constructor that requires the script id to be passed in.")
            End If
            Return _scriptID
        End Get
    End Property

    ''' <summary>
    ''' (DO NOT USE THIS CONSTRUCTOR ***IT IS DEPRECATED***) Constructor for Script Object 
    ''' </summary>
    ''' <param name="tempRI">Reflection Interface</param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal tempRI As ReflectionInterface)
        MyBase.New(tempRI)
    End Sub

    ''' <summary>
    ''' Constructor for Script Object
    ''' </summary>
    ''' <param name="tempRI">Reflection Interface</param>
    ''' <param name="scriptID">Script id from Sacker's detail screen.</param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal tempRI As ReflectionInterface, ByVal scriptID As String)
        MyBase.New(tempRI)
        _scriptID = scriptID
        _enterpriseFileSystem = New EnterpriseFileSystem(tempRI.TestMode, Region.UHEAA)
        '_ProcessLogData = ProcessLogger.RegisterScript(scriptID, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly())
    End Sub

    ''' <summary>
    ''' Constructor for Script Object
    ''' </summary>
    ''' <param name="tempRI">Reflection Interface</param>
    ''' <param name="scriptID">Script id from Sacker's detail screen.</param>
    ''' <param name="tMDBorrower">Borrower object sent by Maui DUDE</param> 
    ''' <param name="tRunNumber">The run number for the script being called.  Some script are called twice by DUDE to allow the user to enter information but allow the script to do processing with the information at a later point.  Number 1 should be passed in for the first run of the script and 2 for the second (if there is a second).</param> 
    ''' <remarks></remarks>
    Protected Sub New(ByVal tempRI As ReflectionInterface, ByVal scriptID As String, ByVal tMDBorrower As MDBorrower, ByVal tRunNumber As Integer)
        MyBase.New(tempRI)
        _scriptID = scriptID
        _calledByMauiDUDE = True
        tMDBorrower.MonthlyPA.ToString().Replace("RPF=$", "")
        _mauiDUDEBorrower = tMDBorrower
        _runNumber = tRunNumber
        _enterpriseFileSystem = New EnterpriseFileSystem(tempRI.TestMode, Region.UHEAA)
        '_ProcessLogData = ProcessLogger.RegisterQScript(tempRI.TestMode, "UHEAA", scriptID, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly())
    End Sub

    'will be implemented in the actual script
    Public MustOverride Sub Main()

    ''' <summary>
    ''' Enter comments in LP50.  If calling from an object that inherits from ScriptBase this is the preferable version of the method to call because it uses the script ID that is originally sent in to the ScriptBase constructor.
    ''' </summary>
    ''' <param name="ssn">The person ID: SSN for borrowers, co-borrowers, students; reference ID for references.</param>
    ''' <param name="arc">Action code</param>
    ''' <param name="activityType">Activity type</param>
    ''' <param name="contactType">Contact type</param>
    ''' <param name="comment">Activity comments</param>
    ''' <param name="pauseForUserComments">True if the script should pause to allow the user to enter additional comments.</param>
    ''' <param name="isReference">True if the SSN should go in the ASSOCIATED PERSON ID space.</param>
    ''' <returns>True if the activity comment was successfully added.</returns>
    Protected Overloads Function AddCommentInLP50(ByVal ssn As String, ByVal arc As String, ByVal activityType As String, ByVal contactType As String, ByVal comment As String, ByVal pauseForUserComments As Boolean, ByVal isReference As Boolean) As Boolean
        Return Common.AddCommentInLP50(RS, ssn, arc, ScriptID, activityType, contactType, comment, pauseForUserComments, isReference)
    End Function

    ''' <summary>
    ''' Enter comments in LP50. If calling from an object that inherits from ScriptBase this is the preferable version of the method to call because it uses the script ID that is originally sent in to the ScriptBase constructor. 
    ''' </summary>
    ''' <param name="ssn">The person ID: SSN for borrowers, co-borrowers, students; reference ID for references.</param>
    ''' <param name="arc">Action code</param>
    ''' <param name="activityType">Activity type</param>
    ''' <param name="contactType">Contact type</param>
    ''' <param name="comment">Activity comments</param>
    ''' <returns>True if the activity comment was successfully added.</returns>
    Protected Overloads Function AddCommentInLP50(ByVal ssn As String, ByVal arc As String, ByVal activityType As String, ByVal contactType As String, ByVal comment As String) As Boolean
        Return Common.AddCommentInLP50(RS, ssn, arc, ScriptID, activityType, contactType, comment, False, False)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Protected Overloads Function ATD22AllLoansBackedUpWithATD37FirstApp(ByVal ssn As String, ByVal arc As String, ByVal comment As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22AllLoansBackedUpWithATD37FirstApp(RI, ssn, arc, comment, ScriptID, pausePlease)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="neededBy">The date when the queue task created by this ARC needs to be completed</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Protected Overloads Function ATD22AllLoansBackedUpWithATD37FirstApp(ByVal ssn As String, ByVal arc As String, ByVal neededBy As DateTime, ByVal comment As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22AllLoansBackedUpWithATD37FirstApp(RI, ssn, arc, neededBy, comment, ScriptID, pausePlease)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">Loan sequence numbers to be marked on TD22.</param>
    ''' <param name="pauseForManualComments">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Friend Overloads Function ATD22ByLoanBackedUpWithATD37FirstApp(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As List(Of Integer), Optional ByVal pauseForManualComments As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22ByLoanBackedUpWithATD37FirstApp(RI, ssn, arc, comment, loanSequenceNumbers, ScriptID, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pausePlease">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Protected Overloads Function ATD22AllLoansBackedUpWithATD37AllApps(ByVal ssn As String, ByVal arc As String, ByVal comment As String, Optional ByVal pausePlease As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22AllLoansBackedUpWithATD37FirstApp(RI, ssn, arc, comment, ScriptID, pausePlease)
    End Function

    ''' <summary>
    ''' Tries to add comment to TD22 and if not successful it tries to add one to TD37.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">Loan sequence numbers to be marked on TD22.</param>
    ''' <param name="pauseForManualComments">Pause to allow use to add comments</param>
    ''' <returns>Returns the result of the last function it called.</returns>
    ''' <remarks></remarks>
    Friend Overloads Function ATD22ByLoanBackedUpWithATD37AllApps(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As List(Of Integer), Optional ByVal pauseForManualComments As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD22ByLoanBackedUpWithATD37FirstApp(RI, ssn, arc, comment, loanSequenceNumbers, ScriptID, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting all loans.  If calling from an object that inherits from ScriptBase this is the preferable version of the method to call because it uses the script ID that is originally sent in to the ScriptBase constructor.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pauPls">Pause to allow user to add comments (was defaulted to false)</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Protected Overloads Function ATD22AllLoans(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal pauPls As Boolean) As Common.CompassCommentScreenResults
        Return Common.ATD22AllLoans(RS, ssn, arc, comment, ScriptID, pauPls)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans specified.  If calling from an object that inherits from ScriptBase this is the preferable version of the method to call because it uses the script ID that is originally sent in to the ScriptBase constructor.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="loanSequenceNumbers">The loan sequence numbers that should be selected on TD22</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments (was defaulted to false)</param>
    Protected Overloads Function ATD22ByLoan(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal loanSequenceNumbers As System.Collections.Generic.List(Of Integer), ByVal pauseForManualComments As Boolean) As Boolean
        Return Common.ATD22ByLoan(RS, ssn, arc, comment, loanSequenceNumbers, ScriptID, pauseForManualComments)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans in specified loan programs.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="options">Use the OR operator ("|" in C#) to string together multiple options</param>
    ''' <param name="loanPrograms">Loan programs that should be included in the selection</param>
    Protected Overloads Function ATD22ByLoanProgram(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal options As Common.TD22Options, ByVal ParamArray loanPrograms() As String) As Boolean
        Return Common.ATD22ByLoanProgram(RS, ssn, arc, comment, ScriptID, options, loanPrograms)
    End Function

    ''' <summary>
    ''' Enters an activity record/action request in COMPASS selecting only the loans with a balance.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pauseForManualComments">Pause to allow the user to add comments</param>
    Protected Overloads Function ATD22ByBalance(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal pauseForManualComments As Boolean) As Boolean
        Return Common.ATD22ByBalance(RS, ssn, arc, comment, ScriptID, pauseForManualComments)
    End Function

    ''' <summary>
    ''' This function tries to add the comment on TD37 for all loan applications.  If calling from an object that inherits from ScriptBase this is the preferable version of the method to call because it uses the script ID that is originally sent in to the ScriptBase constructor.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pauseForManualComments">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Protected Overloads Function ATD37AllLoans(ByVal ssn As String, ByVal arc As String, ByVal comment As String, Optional ByVal pauseForManualComments As Boolean = False) As CompassCommentScreenResults
        Return Common.ATD37AllLoans(RS, ssn, arc, comment, ScriptID, pauseForManualComments)
    End Function

    ''' <summary>
    ''' This function tries to add the comment on TD37 for passed in app #'s.  If calling from an object that inherits from ScriptBase this is the preferable version of the method to call because it uses the script ID that is originally sent in to the ScriptBase constructor.
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="apps">Array of app numbers</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments (was defaulted to false)</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    Protected Overloads Function ATD37ByLoan(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal apps As System.Collections.Generic.List(Of Integer), ByVal pausePlease As Boolean) As Common.CompassCommentScreenResults
        Return Common.ATD37ByLoan(RS, ssn, arc, comment, apps, ScriptID, pausePlease)
    End Function

    ''' <summary>
    ''' this function tries to add the comment on TD37 for the first app listed
    ''' </summary>
    ''' <param name="ssn">Borrower SSN</param>
    ''' <param name="arc">Action Request Code</param>
    ''' <param name="comment">Comment text for comment</param>
    ''' <param name="pausePlease">Pause to allow user to add additional comments</param>
    ''' <returns>Enumerated results for attempt to add comments</returns>
    ''' <remarks></remarks>
    Protected Overloads Function ATD37FirstLoan(ByVal ssn As String, ByVal arc As String, ByVal comment As String, ByVal pausePlease As Boolean) As Common.CompassCommentScreenResults
        Return Common.ATD37FirstLoan(RS, ssn, arc, comment, ScriptID, pausePlease)
    End Function

    ''' <summary>
    ''' ***DEPRECATED*** DO NOT USE ANY MORE!  DOES NOT USE DOCNAME OR DOCPATH ANYMORE.  THIS INFOMRATION IS NOW GATHERED DIRECTLY FROM THE DB. Main entry point that calls all helper functions as needed to accomplish Centralized printing, adding the static current date and adding the 2D barcode.  FOR USER SCRIPTS ONLY.  If calling from an object that inherits from ScriptBase this is the preferable version of the method to call because it uses the script ID that is originally sent in to the ScriptBase constructor.
    ''' </summary>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="systemToAddCommentsTo">System to add comments to (OneLINK or Compass)</param>
    ''' <param name="ssn">SSN of the borrower</param>
    ''' <param name="dataFile">The data file for the merge operation (Must have one data row)</param>
    ''' <param name="acctNumOrRefIDFieldNm">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="docName">***DEPRECATED*** Merge document name.</param>
    ''' <param name="docPath">***DEPRECATED*** Merge document path</param>
    ''' <param name="stateCodeFieldNm">Field name for the state code field</param>
    ''' <param name="passedInDeployMethod">Desired deployment method.  If anything other than User Prompt is passsed then the user is required to use that method else the user will be given a prompt to choose a method and provide the needed information for the chosen method.</param>
    ''' <param name="ContactType">The contact type to be used when adding notes to the systems (OneLINK or Compass).  If not specified in the spec "03" should be used (was the default before moving to C#.</param>
    ''' <param name="LetterRecip">The recipient of the letter.  If not specified in the spec "borrower" should be used (was default before moving to C#).</param>
    ''' <remarks>This method expects the data file that is sent to it to have a single data row in it</remarks>
    Protected Overloads Sub GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ByVal letterID As String, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal docName As String, ByVal docPath As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal ContactType As String, ByVal LetterRecip As Barcode2DLetterRecipient)
        Q.DocumentHandling.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(RI, letterID, systemToAddCommentsTo, ssn, dataFile, acctNumOrRefIDFieldNm, ScriptID, stateCodeFieldNm, passedInDeployMethod, ContactType, LetterRecip)
    End Sub

    ''' <summary>
    ''' Main entry point that calls all helper functions as needed to accomplish Centralized printing, adding the static current date and adding the 2D barcode.  FOR USER SCRIPTS ONLY.  If calling from an object that inherits from ScriptBase this is the preferable version of the method to call because it uses the script ID that is originally sent in to the ScriptBase constructor.
    ''' </summary>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="systemToAddCommentsTo">System to add comments to (OneLINK or Compass)</param>
    ''' <param name="ssn">SSN of the borrower</param>
    ''' <param name="dataFile">The data file for the merge operation (Must have one data row)</param>
    ''' <param name="acctNumOrRefIDFieldNm">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="stateCodeFieldNm">Field name for the state code field</param>
    ''' <param name="passedInDeployMethod">Desired deployment method.  If anything other than User Prompt is passsed then the user is required to use that method else the user will be given a prompt to choose a method and provide the needed information for the chosen method.</param>
    ''' <param name="ContactType">The contact type to be used when adding notes to the systems (OneLINK or Compass).  If not specified in the spec "03" should be used (was the default before moving to C#.</param>
    ''' <param name="LetterRecip">The recipient of the letter.  If not specified in the spec "borrower" should be used (was default before moving to C#).</param>
    ''' <remarks>This method expects the data file that is sent to it to have a single data row in it</remarks>
    Protected Overloads Sub GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ByVal letterID As String, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal ContactType As String, ByVal LetterRecip As Barcode2DLetterRecipient)
        Q.DocumentHandling.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(RI, letterID, systemToAddCommentsTo, ssn, dataFile, acctNumOrRefIDFieldNm, ScriptID, stateCodeFieldNm, passedInDeployMethod, ContactType, LetterRecip)
    End Sub
End Class
