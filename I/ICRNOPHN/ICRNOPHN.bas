Attribute VB_Name = "ICRNOPHN"
Const logFile As String = "ICRNOPHNLog.txt"
Private ftpFolder As String
Private LogFolder As String
Private DocFolder As String
Private RecID As String
Private SASNum As String

Sub Main()
    Dim i As Integer
    
    DocFolder = "X:\PADD\LoanManagement\"
    SP.Common.TestMode ftpFolder, DocFolder, LogFolder

    If Not SP.Common.CalledByMBS Then MsgBox "This is the Increase Collection Revenue - No Valid Phone script.", vbOKOnly

    'Check recovery file
    RecID = ""
    If Dir(LogFolder & logFile) <> "" Then
        Open LogFolder & logFile For Input As #1
            Input #1, RecID
        Close #1
    End If

    'Make sure all files are accounted for. NOTE: If script is in recovery it will skip this step.
    If RecID = "" Then
        For i = 2 To 7
            If Dir(ftpFolder & "ULWM36.LWM36R" & i & ".*.*") = "" Then
                MsgBox "Files are not available. Please contact Systems Support for assistance."
                End
            End If
        Next
    End If
    
    'Process Files
    For i = 2 To 7
       Do While ProcSASFile("ULWM36.LWM36R" & i & "*")
       Loop
    Next
    
    Kill LogFolder & logFile 'Delete the Log
        
    Common.ProcComp "MBSICRNOPHN.TXT"

End Sub
'Returns true while a file is found.
Function ProcSASFile(sas As String) As Boolean
    Dim SasFile As String
    SasFile = Dir(ftpFolder & sas)
    If SasFile = "" Then
        ProcSASFile = False
        Exit Function
    Else
       SASNum = MID(SasFile, 14, 1)
       ProcSASFile = True
    End If
    If FileLen(ftpFolder & SasFile) > 0 Then
        AddCom ftpFolder & SasFile
        PrintLetters ftpFolder & SasFile
    End If
    'Delete SAS File
    Kill ftpFolder & SasFile
End Function

Sub AddCom(sas As String)
    Dim ssn As String
    Dim ActionCode As String
    Dim DF_SPE_ACC_ID As String
    Dim DM_PRS_1 As String
    Dim DM_PRS_LST As String
    Dim DX_STR_ADR_1 As String
    Dim DX_STR_ADR_2 As String
    Dim DM_CT As String
    Dim DC_DOM_ST As String
    Dim DF_ZIP As String
    Dim DM_FGN_CNY As String
    Dim TOT_PAYOFF As String
    Dim ACSKEY As String
    Dim STATE_IND As String
    Dim COST_CENTER_CODE As String
      
    'Assign action code appropriately
    Select Case SASNum
        Case "2"
            ActionCode = "DLIV1"
        Case "3"
            ActionCode = "DLIV2"
        Case "4"
            ActionCode = "DLIV3"
        Case "5"
            ActionCode = "DLIV4"
        Case "6"
            ActionCode = "DLIV5"
        Case "7"
            ActionCode = "DLIV6"
    End Select
    
    Open sas For Input As #1
    
    Do While Not EOF(1)
        Input #1, ssn, DF_SPE_ACC_ID, DM_PRS_1, DM_PRS_LST, DX_STR_ADR_1, DX_STR_ADR_2, DM_CT, DC_DOM_ST, DF_ZIP, DM_FGN_CNY, TOT_PAYOFF, ACSKEY, STATE_IND, COST_CENTER_CODE
        If RecID = "" And ssn <> "SSN" Then
            SP.Common.AddLP50 ssn, ActionCode, "ICRNOPHN", "LT", "03"
            UpdateLog ssn 'Update recovery log with borrower that is being processed
        Else
            If RecID = ssn Then RecID = ""
        End If
    Loop
    
    Close #1
End Sub

Sub PrintLetters(sas As String)
    Dim ssn As String
    Dim DF_SPE_ACC_ID As String
    Dim DM_PRS_1 As String
    Dim DM_PRS_LST As String
    Dim DX_STR_ADR_1 As String
    Dim DX_STR_ADR_2 As String
    Dim DM_CT As String
    Dim DC_DOM_ST As String
    Dim DF_ZIP As String
    Dim DM_FGN_CNY As String
    Dim TOT_PAYOFF As String
    Dim ACSKEY As String
    Dim STATE_IND As String
    Dim COST_CENTER_CODE As String
    Dim Doc411 As String
    Dim DocDesc As String
    
    'Assign doc attributes appropriately
    Select Case SASNum
        Case "2"
            Doc411 = "DFNOPHN1"
            DocDesc = "Default No Phone Letter 1"
        Case "3"
           Doc411 = "DFNOPHN2"
            DocDesc = "Default No Phone Letter 2"
        Case "4"
            Doc411 = "DFNOPHN3"
            DocDesc = "Default No Phone Letter 3"
        Case "5"
            Doc411 = "DFNOPHN4"
            DocDesc = "Default No Phone Letter 4"
        Case "6"
            Doc411 = "DFNOPHN5"
            DocDesc = "Default No Phone Letter 5"
        Case "7"
            Doc411 = "DFNOPHN6"
            DocDesc = "Default No Phone Letter 6"
    End Select
    
    Barcode2D.AddBarcodeAndStaticCurrentDate sas, "DF_SPE_ACC_ID", Doc411
    SP.CostCenterPrinting.Main DocFolder, Doc411, DocDesc, Page1, Doc411, sas, Now, "ICRNOPHN"
End Sub

Sub UpdateLog(Acc As String)
    Open LogFolder & logFile For Output As #2
        Write #2, Acc
    Close #2
End Sub



