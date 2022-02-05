<CLSCompliant(True)> _
Public Class MDScriptInfoSpecificToBusinessUnitBase

    'So originally we changed MD and Q so that the generic Borrower object that all home pages use could be passed from MD to any and
    'all scripts.  With this plan we had one concern, what if there is an SR that requested passing something that is unique to a single 
    'home page in MD.
    'However there had never been an SR like that, so we thought the chances were fairly low, and if that happened then we would handle it when
    'it happened.  Well, it happened almost immediately.  This is a general explanation of what I have built to handle this situation.  
    'there where a couple of things I was trying to accomplish and dodge while building this.  
    ' - Make it so the borrower object didn't have a bunch of variables that weren't populated because they aren't populated for the 
    'originating home page (lots of variables that are only used for specific home page not all home pages).
    ' - Make it so the variables could be populated in the home page and be used in the script.
    ' - Make it so significant parts of DUDE logic isn't defined in Q (outside DUDE). 
    ' - Make it so that generally the variables are only useful in the originating homepage and the script (not in other home pages and 
    'not between those pieces).
    ' - Make the variables appear to be part of the home page specific borrower but in reality have it part of the generic borrower object
    'So here is what I did:
    'The MDScriptInfoSpecificToBusinessUnitBase class which is defined in Q is meant to be a generic class/object that will be inheritied by 
    'other home page specific objects (these will have data specific to a home page).  The MDBorrower object, also defined in Q project, 
    'has a property and private variable that will be an instance of the MDScriptInfoSpecificToBusinessUnitBase object 
    '(ScriptInfoToGenericBusinessUnit and _scriptInfoToGenericBusinessUnit) and will act as a place holder for the actual object that will have 
    'the home page specific data.  The objects that actually hold the home page specific data (inherit MDScriptInfoSpecificToBusinessUnitBase)
    'are also defined in Q so both the script and the homepage can access them.  These objects (example MDScriptInfoSpecificToAcctResolution) need to 
    'inherit MDScriptInfoSpecificToBusinessUnitBase (so the generic place holder can hold it), as I said earlier, and will contain home page 
    'specific data values.
    'The place holder can then be created and populated in the home page and cast to the appropriate type and then also cast on the script side when the values need to be 
    'pulled from it.  As a matter a preference I chose to also add properties in the home page specific borrower object so that it appears that the properties in 
    'your home page specific MDScriptInfoSpecificToBusinessUnitBase are actually properties of the home page borrower specific object.  I know that
    'this makes the two objects strongly coupled, but that is actually the whole idea.  Make is so you have an object that can be accessed in the 
    'script (defined in Q) that looks like it is part of the home page specific borrower object (which can't be defined in Q).

    Private _scriptCompletedSuccessfully As Boolean = False
    Public Property ScriptCompletedSuccessfully() As Boolean
        Get
            Return _scriptCompletedSuccessfully
        End Get
        Set(ByVal value As Boolean)
            _scriptCompletedSuccessfully = value
        End Set
    End Property

End Class
