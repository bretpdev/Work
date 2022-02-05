VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} SchDemoTyp 
   Caption         =   "School Update Type"
   ClientHeight    =   5595
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5100
   OleObjectBlob   =   "SchDemoTyp.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "SchDemoTyp"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


'validate school code and display data form
Private Sub CommandButton1_Click()
    Dim DateString As String
    
'verify the school code
    'prompt the user to enter a school code if none was entered
    If txtSchCd = "" Then
        MsgBox "You must enter a school code to proceed.", 48, "Enter School Code"
        Exit Sub
    'verify school code if it was entered
    Else
        'access LPSC for the school
        FastPath "LPSCI" & txtSchCd & "GEN"
        'warn the user and return to the form if the school was not found
        If Check4Text(1, 55, "INSTITUTION NAME/ID SEARCH") Then
            MsgBox "You must enter a valid school code to proceed.", 48, "Enter Valid School Code"
            Exit Sub
        End If
    End If
    
    'get date string to pass to validation function
    DateString = txtSubDt
    
'check other data, warn user and return to form if data is not valid
    'verify at least one department option was selected
    If rdioGen = False And rdioFin = False And rdioReg = False And rdioBur = False And rdioAll = False Then
        MsgBox "You must select at least one update type to proceed.", 48, "Select Update Type"
        Exit Sub
    'verify date entered is a valid date
    ElseIf Not DateFormat(DateString) Then
        MsgBox "You must enter a valid date to proceed.", 48, "Enter Date"
        Exit Sub
    End If
    
'proceed if all data is valid
    'update date form field with formatted date
    txtSubDt.Text = DateString
    'hide update type selection form
    Me.Hide
    'display data form
    SchDemoData.Show
    
    'clear data form and values from
    Unload SchDemoData
    rdioGen = False
    rdioFin = False
    rdioReg = False
    rdioBur = False
    rdioAll = False
    txtSubDt = ""
    txtSchCd = ""
End Sub

'end script
Private Sub CommandButton2_Click()
    Unload SchDemoTyp
    End
End Sub

'run updates
Private Sub CommandButton3_Click()
    Me.Hide
    SchDemoRun.Show
End Sub


