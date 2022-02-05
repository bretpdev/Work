<CLSCompliant(True)> _
Public Class MDScriptInfoSpecificToAcctResolution
    Inherits MDScriptInfoSpecificToBusinessUnitBase

    ' *** PLEASE SEE DOCUMENTATION IN MDScriptInfoSpecificToBusinessUnitBase CLASS DEFINITION FOR HOW TO USE THIS CODE ***

    Private _employerID As String
    Public Property EmployerID() As String
        Get
            Return _employerID
        End Get
        Set(ByVal value As String)
            _employerID = value
        End Set
    End Property


End Class
