Attribute VB_Name = "LVC"
Private UserId As String

Public Sub LVC()
    Common.ResetPublicVars
    'get userID and script ID and userID and password if in test to switch between systems
    SP.QL.GatherIDPass
    SP.QL.ToCO
    UserId = SP.Common.GetUserID
    
    'process LVCs
    Do
        Load frmLVC
        frmLVC.Show
        Unload frmLVC
    Loop
End Sub

'pass user ID to form
Public Function PassUserID() As String
    PassUserID = UserId
End Function
