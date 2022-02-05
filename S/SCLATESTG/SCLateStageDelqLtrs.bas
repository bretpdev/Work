Attribute VB_Name = "SCLateStageDelqLtrs"
Option Explicit

Const TitleTxt As String = "Special Campaign - Late Stage Delinquent Letters"
Const LogFileNm As String = "Special Camp Late Stage Delq Ltrs.txt"

Sub Main()
    Dim i As Integer
    Dim FTPDir As String
    Dim DocDir As String
    Dim LogDir As String
    Dim FilesInProc(5) As String
    Dim TDS As String
    Dim InRecovery As Boolean
    Dim rSSN As String
    Dim rPhase As String
    TDS = CStr(Now)
    If SP.Common.CalledByMBS() = False Then
        'prompt user as to the purpose of the script
        If MsgBox("This is the Special Campaign - Late Stage Delinquent Letters script.  To continue please click OK.", vbInformation + vbOKCancel, TitleTxt) <> vbOK Then End
    End If
    'set up test variables
    DocDir = "X:\PADD\LoanManagement\"
    SP.Common.TestMode FTPDir, DocDir, LogDir
    'check if the script is in recovery
    If Dir(LogDir & LogFileNm) <> "" Then
        'in recovery
        MsgBox "The script is recovering.", vbInformation, TitleTxt
        Open LogDir & LogFileNm For Input As #1
        Input #1, rSSN, rPhase
        Close #1
        InRecovery = True
    Else
        'not in recovery
        InRecovery = False
        rPhase = "0"
        'Get rid of any old files
        i = 2
        Do While i < 8
            SP.Common.DeleteOldFilesReturnMostCurrent FTPDir, "ULWM31.LWM31R" & i & "*"
            i = i + 1
        Loop
        'verify existance of all files
        i = 2
        While i < 8
            If Dir(FTPDir & "ULWM31.LWM31R" & CStr(i) & "*") = "" Then
                MsgBox "One or more of the ""ULWM31.LWM31"" SAS files could not be found.  Please contact Systems Support.", vbCritical, TitleTxt
                End
            End If
            i = i + 1
        Wend
    End If
    'process all files
    While Dir(FTPDir & "ULWM31.LWM31R2*") <> ""
        'add all activity comments
        FilesInProc(0) = AddLP50Comments(FTPDir, "ULWM31.LWM31R2*", "ALTS1", 1, LogDir & LogFileNm, rSSN, CInt(rPhase))
        FilesInProc(1) = AddLP50Comments(FTPDir, "ULWM31.LWM31R3*", "ALTS2", 2, LogDir & LogFileNm, rSSN, CInt(rPhase))
        FilesInProc(2) = AddLP50Comments(FTPDir, "ULWM31.LWM31R4*", "ALTT1", 3, LogDir & LogFileNm, rSSN, CInt(rPhase))
        FilesInProc(3) = AddLP50Comments(FTPDir, "ULWM31.LWM31R5*", "ALTT2", 4, LogDir & LogFileNm, rSSN, CInt(rPhase))
        FilesInProc(4) = AddLP50Comments(FTPDir, "ULWM31.LWM31R6*", "ALTV1", 5, LogDir & LogFileNm, rSSN, CInt(rPhase))
        FilesInProc(5) = AddLP50Comments(FTPDir, "ULWM31.LWM31R7*", "ALTV2", 6, LogDir & LogFileNm, rSSN, CInt(rPhase))
        If 7 > CInt(rPhase) Then 'recovery check
            'print letters if files have data
            If FileLen(FTPDir & FilesInProc(0)) > 0 Then SP.CostCenterPrinting.Main DocDir, "DANOPHND1", "Special Campaign - Late Stage Delinquent Ltrs #1", Page1, "DANOPHND1", Barcode2D.AddBarcodeAndStaticCurrentDate(FTPDir & FilesInProc(0), "DF_SPE_ACC_ID", "DANOPHND1", True), TDS, "SCLATESTG", InRecovery
            If FileLen(FTPDir & FilesInProc(1)) > 0 Then SP.CostCenterPrinting.Main DocDir, "DANOPHND2", "Special Campaign - Late Stage Delinquent Ltrs #2", Page1, "DANOPHND2", Barcode2D.AddBarcodeAndStaticCurrentDate(FTPDir & FilesInProc(1), "DF_SPE_ACC_ID", "DANOPHND2", True), TDS, "SCLATESTG", InRecovery
            If FileLen(FTPDir & FilesInProc(2)) > 0 Then SP.CostCenterPrinting.Main DocDir, "DANOPHND3", "Special Campaign - Late Stage Delinquent Ltrs #3", Page1, "DANOPHND3", Barcode2D.AddBarcodeAndStaticCurrentDate(FTPDir & FilesInProc(2), "DF_SPE_ACC_ID", "DANOPHND3", True), TDS, "SCLATESTG", InRecovery
            If FileLen(FTPDir & FilesInProc(3)) > 0 Then SP.CostCenterPrinting.Main DocDir, "DANOPHND4", "Special Campaign - Late Stage Delinquent Ltrs #4", Page1, "DANOPHND4", Barcode2D.AddBarcodeAndStaticCurrentDate(FTPDir & FilesInProc(3), "DF_SPE_ACC_ID", "DANOPHND4", True), TDS, "SCLATESTG", InRecovery
            If FileLen(FTPDir & FilesInProc(4)) > 0 Then SP.CostCenterPrinting.Main DocDir, "DANOPHND5", "Special Campaign - Late Stage Delinquent Ltrs #5", Page1, "DANOPHND5", Barcode2D.AddBarcodeAndStaticCurrentDate(FTPDir & FilesInProc(4), "DF_SPE_ACC_ID", "DANOPHND5", True), TDS, "SCLATESTG", InRecovery
            If FileLen(FTPDir & FilesInProc(5)) > 0 Then SP.CostCenterPrinting.Main DocDir, "DANOPHND6", "Special Campaign - Late Stage Delinquent Ltrs #6", Page1, "DANOPHND6", Barcode2D.AddBarcodeAndStaticCurrentDate(FTPDir & FilesInProc(5), "DF_SPE_ACC_ID", "DANOPHND6", True), TDS, "SCLATESTG", InRecovery
            Open LogDir & LogFileNm For Output As #2
            Write #2, "", "7"
            Close #2
        End If
        If 8 > CInt(rPhase) Then 'recovery check
            'delete all files processed
            Kill FTPDir & FilesInProc(0)
            Kill FTPDir & FilesInProc(1)
            Kill FTPDir & FilesInProc(2)
            Kill FTPDir & FilesInProc(3)
            Kill FTPDir & FilesInProc(4)
            Kill FTPDir & FilesInProc(5)
            Open LogDir & LogFileNm For Output As #2
            Write #2, "", "8"
            Close #2
        End If
        rPhase = 0
    Wend
    Kill LogDir & LogFileNm 'delete log file
    ProcComp "MBSSCLATESTG.txt"
End Sub

Function AddLP50Comments(FTPDir As String, FileNmTmplt As String, ActCd As String, Phase As Integer, LogPath As String, rSSN As String, rPhase As Integer) As String
    Dim FileInProc As String
    Dim Rec As String
    'get file name
    FileInProc = Dir(FTPDir & FileNmTmplt)
    'when function is doen return file name that is in process
    AddLP50Comments = FileInProc
    'check if logic should be performed
    If Phase >= rPhase Then
        If FileLen(FTPDir & FileInProc) = 0 Then
            Exit Function 'exit sub if file is empty
        End If
        'otherwise process file
        Open FTPDir & FileInProc For Input As #1
        Line Input #1, Rec 'bypass header row
        'check for recovery
        If Phase = rPhase Then
            While CStr(Split(Rec, ",")(0)) <> rSSN
                Line Input #1, Rec
            Wend
        End If
        'process file
        While Not EOF(1)
            Line Input #1, Rec
            SP.Common.AddLP50 CStr(Split(Rec, ",")(0)), ActCd, "SCLATESTG", "LT", "03"
            Open LogPath For Output As #2
            Write #2, CStr(Split(Rec, ",")(0)), CStr(Phase)
            Close #2
        Wend
        Close #1
    End If
End Function
