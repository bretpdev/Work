Public MustInherit Class FedScriptBase
    Inherits ScriptBase

    ''' <summary>
    ''' Creates a convenience layer for writing scripts in the federal region and automatically validates to the federal region.
    ''' </summary>
    ''' <param name="ri"></param>
    ''' <param name="scriptId"></param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal ri As ReflectionInterface, ByVal scriptId As String)
        MyBase.New(ri, scriptId)
        MyBase._enterpriseFileSystem = New EnterpriseFileSystem(ri.TestMode, Region.CornerStone)
        ValidateRegion(Region.CornerStone)
    End Sub


    ''' <summary>
    ''' Creates a convenience layer for writing scripts in the federal region and validates to the specified region.
    ''' </summary>
    ''' <param name="ri">An interface to the running Reflection session.</param>
    ''' <param name="scriptId">Script ID from Sacker's detail screen.</param>
    ''' <param name="regionToValidate">
    ''' The region the script is required to start in.
    ''' Note that validating the required region involves changing screens. To avoid this behavior, select "None."
    ''' </param>
    Protected Sub New(ByVal ri As ReflectionInterface, ByVal scriptId As String, ByVal regionToValidate As Region)
        MyBase.New(ri, scriptId)
        MyBase._enterpriseFileSystem = New EnterpriseFileSystem(ri.TestMode, Region.CornerStone)
        ValidateRegion(regionToValidate)
    End Sub

    ''' <summary>
    ''' Creates a convenience layer for writing scripts in the federal region.
    ''' </summary>
    ''' <param name="ri">An interface to the running Reflection session.</param>
    ''' <param name="scriptId">Script ID from Sacker's detail screen.</param>
    ''' <param name="regionToValidate">
    ''' The region the script is required to start in.
    ''' Note that validating the required region involves changing screens. To avoid this behavior, select "None."
    ''' </param>
    ''' <param name="borrower">Borrower object sent by Maui DUDE.</param>
    ''' <param name="runNumber">
    ''' The run number for the script being called.
    ''' Some script are called twice by DUDE to allow the user to enter information
    ''' but allow the script to do processing with the information at a later point.
    ''' Number 1 should be passed in for the first run of the script and 2 for the second
    ''' (if there is a second).
    ''' </param>
    ' Protected Sub New(ByVal ri As ReflectionInterface, ByVal scriptId As String, ByVal regionToValidate As Region, ByVal borrower As MDBorrower, ByVal runNumber As Integer)
    ' MyBase.New(ri, scriptId, borrower, runNumber)
    ' ValidateRegion(regionToValidate)
    '  End Sub
End Class
