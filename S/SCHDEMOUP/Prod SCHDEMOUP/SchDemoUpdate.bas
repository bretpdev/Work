Attribute VB_Name = "SchDemoUpdate"

'prompt the user for updated school information and process updates
Sub Main()
    'display the update type selection form until it is cancelled
    Do
        SchDemoTyp.Show
    Loop
End Sub

'determine OneLINKdepartment code
Function GetOLDept() As String
    If SchDemoTyp.rdioGen = True Then
        GetOLDept = "GEN"
    ElseIf SchDemoTyp.rdioFin = True Then
        GetOLDept = "110"
    ElseIf SchDemoTyp.rdioReg = True Then
        GetOLDept = "112"
    ElseIf SchDemoTyp.rdioBur = True Then
        GetOLDept = "111"
    Else
        GetOLDept = "ALL"
    End If
End Function

'determine COMPASS department code
Function GetCSDept() As String
    If SchDemoTyp.rdioGen = True Then
        GetCSDept = "000"
    ElseIf SchDemoTyp.rdioFin = True Then
        GetCSDept = "004"
    ElseIf SchDemoTyp.rdioReg = True Then
        GetCSDept = "001"
    ElseIf SchDemoTyp.rdioBur = True Then
        GetCSDept = "003"
    Else
        GetCSDept = "ALL"
    End If
End Function

'new, jd, sr 873, 12/13/04, 03/18/05
'<1> sr1094, jd, 05/06/05, 05/06/05
'<2> sr1131, tp, 05/24/05, 05/31/05      'Updated Form SchDemoData to get the phone # one line higher on the screen.
'<3> sr1346, jd, changed field positions for changes to logon screen
'<4> sr2249, tp
