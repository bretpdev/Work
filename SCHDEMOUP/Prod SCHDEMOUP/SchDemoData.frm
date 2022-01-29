VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} SchDemoData 
   Caption         =   "School Demographics Update Data Input"
   ClientHeight    =   7875
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6300
   OleObjectBlob   =   "SchDemoData.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "SchDemoData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Dim STList As Variant

Private Sub Frame3_Click()

End Sub

Private Sub UserForm_Initialize()
    'set up state list
    STList = Array("AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY")
    lstState.list() = STList
    
    'get school name from screen displayed
    txtSchName = GetText(5, 21, 40)
    
    'access OneLINK LPSC for the dept selected if not gen or all
    If SchDemoTyp.rdioGen = False And SchDemoTyp.rdioAll = False Then
        FastPath "LPSCI" & SchDemoTyp.txtSchCd & SchDemoUpdate.GetOLDept
    End If
    
    'populate form fields with values from LPSC if the department exists
    If Not Check4Text(1, 55, "INSTITUTION NAME/ID SEARCH") Then
        txtAdd1 = GetText(8, 21, 40)
        txtAdd2 = GetText(9, 21, 40)
        txtAdd3 = GetText(10, 21, 40)
        txtCity = GetText(11, 21, 30)
        'get the domestic address information
        If (GetText(11, 59, 2) <> "" And GetText(11, 59, 2) <> "FC") Or _
        GetText(15, 19, 10) <> "" Or GetText(15, 70, 10) <> "" Then '<1>
'           GetText(16, 19, 10) <> "" Or GetText(16, 70, 10) <> "" Then '</1>
            lstState = GetText(11, 59, 2)
'<2>
'            txtDomPhn = GetText(16, 19, 10)
'            txtDomExt = GetText(16, 34, 4)
'            txtDomFax = GetText(16, 70, 10)
            txtDomPhn = GetText(15, 19, 10)
            txtDomExt = GetText(15, 34, 4)
            txtDomFax = GetText(15, 70, 10)
'</2>
            txtFgnState = ""
            txtFgnCountry = ""
            txtPhnIC = ""
            txtPhnCny = ""
            txtPhnCty = ""
            txtPhnLcl = ""
            txtFgnExt = ""
            txtFaxIC = ""
            txtFaxCny = ""
            txtFaxCty = ""
            txtFaxLcl = ""
        'get the foreign address information
        Else
            lstState = ""
            txtDomPhn = ""
            txtDomExt = ""
            txtDomFax = ""
            txtFgnState = GetText(12, 21, 15)
            txtFgnCountry = GetText(12, 55, 25)
'<2>
'            txtPhnIC = GetText(17, 19, 3)
'            txtPhnCny = GetText(17, 22, 3)
'            txtPhnCty = GetText(17, 25, 4)
'            txtPhnLcl = GetText(17, 29, 7)
            txtPhnIC = GetText(16, 19, 3)
            txtPhnCny = GetText(16, 22, 3)
            txtPhnCty = GetText(16, 25, 4)
            txtPhnLcl = GetText(16, 29, 7)
'           txtFgnExt = GetText(16, 34, 4)
            txtFgnExt = GetText(15, 34, 4)

'            txtFaxIC = GetText(17, 63, 3)
'            txtFaxCny = GetText(17, 66, 3)
'            txtFaxCty = GetText(17, 69, 4)
'            txtFaxLcl = GetText(17, 73, 7)
            txtFaxIC = GetText(16, 63, 3)
            txtFaxCny = GetText(16, 66, 3)
            txtFaxCty = GetText(16, 69, 4)
            txtFaxLcl = GetText(16, 73, 7)
'</2>
        End If
        txtZip = GetText(11, 66, 17)
    End If
    
    'access TX0Y for gen dept if all is selected or selected dept
    If SchDemoTyp.rdioAll = True Then
        FastPath "TX3Z/ITX0Y" & SchDemoTyp.txtSchCd & "000"
    Else
        FastPath "TX3Z/ITX0Y" & SchDemoTyp.txtSchCd & SchDemoUpdate.GetCSDept
    End If
    'populate contact info form fields if the department exists
    If Not Check4Text(1, 73, "TXX00") Then
'<4>
'        txtConFirst = SchDemoRun.GetText_(15, 32, 10)
'        txtConLast = SchDemoRun.GetText_(16, 32, 20)
        txtConFirst = SchDemoRun.GetText_(16, 31, 10)
        txtConLast = SchDemoRun.GetText_(16, 56, 20)
'</4>
    End If
End Sub

'Next - add data and return for more data
Private Sub CommandButton1_Click()
    If Not AddData Then Exit Sub
    Me.Hide
End Sub

'Cancel - end script
Private Sub CommandButton2_Click()
    Unload SchDemoData
    Unload SchDemoTyp
    End
End Sub

'Complete - add data and display run updates form
Private Sub CommandButton3_Click()
    If Not AddData Then Exit Sub
    Unload SchDemoData
    SchDemoRun.Show
End Sub

'disable/enable fields if related fields are edited
Private Sub lstState_Change()
    FgnLock
End Sub

Private Sub txtDomPhn_Change()
    FgnLock
End Sub

Private Sub txtDomExt_Change()
    FgnLock
End Sub

Private Sub txtDomFax_Change()
    FgnLock
End Sub

Private Sub txtfgnState_Change()
    DomLock
End Sub

Private Sub txtfgnCountry_Change()
    DomLock
End Sub

Private Sub txtPhnIC_Change()
    DomLock
End Sub

Private Sub txtPhnCny_Change()
    DomLock
End Sub

Private Sub txtPhnCty_Change()
    DomLock
End Sub

Private Sub txtPhnLcl_Change()
    DomLock
End Sub

Private Sub txtFgnExt_Change()
    DomLock
End Sub

Private Sub txtFaxIC_Change()
    DomLock
End Sub

Private Sub txtFaxCny_Change()
    DomLock
End Sub

Private Sub txtFaxCty_Change()
    DomLock
End Sub

Private Sub txtFaxLcl_Change()
    DomLock
End Sub

'add data to the text file
Private Function AddData() As Boolean
    AddData = True

    'warn the user and exit the function if both contact names are not either populated or blank
    If (txtConFirst = "" And txtConLast <> "") Or _
       (txtConFirst <> "" And txtConLast = "") Then
        MsgBox "You must enter both contact names.", 48, "Enter Both Contact Names"
        AddData = False
        Exit Function
    End If
    
    'warn the user and exit the function if both foreign state and country are not either populated or blank
    If (txtFgnState = "" And txtFgnCountry <> "") Or _
       (txtFgnState <> "" And txtFgnCountry = "") Then
        MsgBox "Both foreign state and country must be entered if either is entered.", 48, "Enter Foreign State and Country"
        AddData = False
        Exit Function
    End If
    
    'warn the user and exit the function if both domestic and foreign state/country blank
    If txtFgnState = "" And lstState = "" Then
        MsgBox "You must enter either a domestic state code or the foreign state and country.", 48, "Enter Domestic State or Foreign State and Country"
        AddData = False
        Exit Function
    End If
    
    'add data to the data file
    Open "C:\Windows\Temp\SchoolUpdates.txt" For Append As #1
    Write #1, Trim(txtSchName), SchDemoTyp.txtSchCd, SchDemoUpdate.GetOLDept, _
              SchDemoUpdate.GetCSDept, Trim(txtAdd1), Trim(txtAdd2), Trim(txtAdd3), _
              Trim(txtCity), lstState, Trim(txtZip), Trim(txtFgnState), _
              Trim(txtFgnCountry), Trim(txtDomPhn), Trim(txtDomExt), Trim(txtDomFax), _
              Trim(txtPhnIC), Trim(txtPhnCny), Trim(txtPhnCty), Trim(txtPhnLcl), _
              Trim(txtFgnExt), Trim(txtFaxIC), Trim(txtFaxCny), Trim(txtFaxCty), _
              Trim(txtFaxLcl), Trim(txtConFirst), Trim(txtConLast), SchDemoTyp.txtSubDt
    Close #1
End Function

'disable or enable foreign fields if domestic fields are populated or not
Private Function FgnLock()
    'disable foreign fields if any domestic fields are populated
    If lstState <> "" Or txtDomPhn <> "" Or txtDomExt <> "" Or txtDomFax <> "" Then
        txtFgnState.Enabled = False
        txtFgnState.BackColor = vbScrollBars
        txtFgnCountry.Enabled = False
        txtFgnCountry.BackColor = vbScrollBars
        txtPhnIC.Enabled = False
        txtPhnIC.BackColor = vbScrollBars
        txtPhnCny.Enabled = False
        txtPhnCny.BackColor = vbScrollBars
        txtPhnCty.Enabled = False
        txtPhnCty.BackColor = vbScrollBars
        txtPhnLcl.Enabled = False
        txtPhnLcl.BackColor = vbScrollBars
        txtFgnExt.Enabled = False
        txtFgnExt.BackColor = vbScrollBars
        txtFaxIC.Enabled = False
        txtFaxIC.BackColor = vbScrollBars
        txtFaxCny.Enabled = False
        txtFaxCny.BackColor = vbScrollBars
        txtFaxCty.Enabled = False
        txtFaxCty.BackColor = vbScrollBars
        txtFaxLcl.Enabled = False
        txtFaxLcl.BackColor = vbScrollBars
    'enable foreign fields if all domestic fields are blank
    ElseIf lstState = "" And txtDomPhn = "" And txtDomExt = "" And txtDomFax = "" Then
        txtFgnState.Enabled = True
        txtFgnState.BackColor = vbWindowBackground
        txtFgnCountry.Enabled = True
        txtFgnCountry.BackColor = vbWindowBackground
        txtPhnIC.Enabled = True
        txtPhnIC.BackColor = vbWindowBackground
        txtPhnCny.Enabled = True
        txtPhnCny.BackColor = vbWindowBackground
        txtPhnCty.Enabled = True
        txtPhnCty.BackColor = vbWindowBackground
        txtPhnLcl.Enabled = True
        txtPhnLcl.BackColor = vbWindowBackground
        txtFgnExt.Enabled = True
        txtFgnExt.BackColor = vbWindowBackground
        txtFaxIC.Enabled = True
        txtFaxIC.BackColor = vbWindowBackground
        txtFaxCny.Enabled = True
        txtFaxCny.BackColor = vbWindowBackground
        txtFaxCty.Enabled = True
        txtFaxCty.BackColor = vbWindowBackground
        txtFaxLcl.Enabled = True
        txtFaxLcl.BackColor = vbWindowBackground
    End If
End Function

'disable or enable domestic fields if foreign fields are populated or not
Private Function DomLock()
    'disable domestic fields if any foreign fields are populated
    If txtFgnState <> "" Or txtFgnCountry <> "" Or txtPhnIC <> "" Or txtPhnCny <> "" Or txtPhnCty <> "" Or txtPhnLcl <> "" Or txtFgnExt <> "" Or txtFaxIC <> "" Or txtFaxCny <> "" Or txtFaxCty <> "" Or txtFaxLcl <> "" Then
        lstState.Enabled = False
        lstState.BackColor = vbScrollBars
        txtDomPhn.Enabled = False
        txtDomPhn.BackColor = vbScrollBars
        txtDomExt.Enabled = False
        txtDomExt.BackColor = vbScrollBars
        txtDomFax.Enabled = False
        txtDomFax.BackColor = vbScrollBars
    'enable domestic fields if all foreign fields are blank
    ElseIf txtFgnState = "" And txtFgnCountry = "" And txtPhnIC = "" And txtPhnCny = "" And txtPhnCty = "" And txtPhnLcl = "" And txtFgnExt = "" And txtFaxIC = "" And txtFaxCny = "" And txtFaxCty = "" And txtFaxLcl = "" Then
        lstState.Enabled = True
        lstState.BackColor = vbWindowBackground
        txtDomPhn.Enabled = True
        txtDomPhn.BackColor = vbWindowBackground
        txtDomExt.Enabled = True
        txtDomExt.BackColor = vbWindowBackground
        txtDomFax.Enabled = True
        txtDomFax.BackColor = vbWindowBackground
    End If
End Function


