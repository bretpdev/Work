Attribute VB_Name = "SKPCOMCNUP"
Private SASF As String
Private DocDir As String
Private FTPDir As String
Private ErrDat As String
Private User As String

Sub Main()
    User = SP.Common.GetUserID
    ErrDat = "T:\SKPCOMCNUPErr.dat"
    DocDir = "X:\PADD\Aux Services\"
    SP.Common.TestMode FTPDir, DocDir
    
    MsgBox "You are about to run the Skip Comments from OneLINK to COMPASS Clean Up script. Please verify there are batches to process.", vbInformation
    
    'Process batches
    While RunFile("SkipCommOL2COCleanUp.R2.*")
    Wend
    
    'Print errors if they exist
    If Dir(ErrDat) <> "" Then
        SP.Common.PrintDocs DocDir, "SKPCOMCNUPErrRep", ErrDat
        Kill ErrDat
    End If
    
    MsgBox "Processing complete", vbInformation
End Sub

'Returns true while a file is found.
Function RunFile(sas As String) As Boolean
SASF = Dir(FTPDir & sas)
    If SASF = "" Then
        RunFile = False
        Exit Function
    Else
        RunFile = True
    End If
    If FileLen(FTPDir & SASF) > 0 Then
        Addcomments FTPDir & SASF
    End If
    Kill FTPDir & SASF
End Function

Sub Addcomments(SASF)
    Dim ssn As String
    Dim ARC As String
    Dim ActType As String
    Dim AtcCont As String
    Dim ARCDate As String
    Dim ARCComm As String
    Open SASF For Input As 1
        Do While Not EOF(1)
            Input #1, ssn, ARC, ActType, AtcCont, ARCDate, ARCComm
            SP.Common.ATD22AllLoans ssn, ARC, ARCComm, "SKPCOMCNUP", User
            Session.Wait 2
            If Not SP.Q.Check4Text(23, 2, "02860") And Not SP.Q.Check4Text(23, 2, "02114") Then
                Open ErrDat For Append As 2
                    If FileLen(ErrDat) = 0 Then
                         Write #2, "SSN", "ARC", "ActType", "AtcCont", "ARCDate", "ARCComm"
                    End If
                    Write #2, ssn, ARC, ActType, AtcCont, ARCDate, ARCComm
                Close #2
            End If
        Loop
    Close #1
End Sub
