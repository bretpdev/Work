Attribute VB_Name = "SkpBrwAtrn"
'Script Name = Skip Borrower Attorney Letter
'ScriptID = SKPBRWATRN

Sub Main()
Dim str As String
Dim Arr() As String
Dim rSSN As String 'recovery ssn
Dim Recover As Boolean 'true = in recovery mode
Dim ftpFolder As String
Dim LogFolder As String
Dim DocPath As String
Dim Doc As String
Dim Fname As String 'File
Dim RFName As String 'Recovery File
Dim timeStamp As String
Dim Run As Boolean
Dim FileEmpty As Boolean

    If Not SP.Common.CalledByMBS Then If MsgBox("This is the Skip Borrower Attorney Letter script. This script processes the UTLWK23 SAS file and creates a letter to the borrower reference.", vbOKCancel, "Skip Borrower Attorney Letter") = vbCancel Then End
    timeStamp = Time
    Run = False
    FileEmpty = True
    Doc = "ATNYSKP"
    DocPath = "X:\PADD\Aux Services\"
    SP.Common.TestMode ftpFolder, DocPath, LogFolder
    RFName = "SkpBrwAtrnLog.txt"
    Do
        'check for the existance of a file
        Fname = Dir(ftpFolder & "ULWK23.LWK23R2.*")
        If Fname = "" And Run = False Then
            'missing file
            MsgBox "The ULWK23.LWK23R2 file is missing! Contact System Support for assistance.", vbCritical
            End
        ElseIf Fname = "" And Run = True Then
            'script complete
            If FileEmpty Then
                MsgBox "File empty. Script complete!"
                ProcComp "MBSSKPBRWATRN.TXT", False
                End
            Else
                ProcComp "MBSSKPBRWATRN.TXT"
                End
            End If
        End If
        'if a file to process was found
        If FileLen(ftpFolder & Fname) <> 0 Then
            'detect non-empty file
            FileEmpty = False
        End If
        Recover = False
        If Dir(LogFolder & RFName) <> "" Then
            'recovery mode
            Recover = True
            Open LogFolder & RFName For Input As #3
                Input #3, rSSN
            Close #3
        End If
        'if file not empty then process file
        Run = True
        If FileLen(ftpFolder & Fname) <> 0 Then
            Open ftpFolder & Fname For Input As #1
                Do While Not EOF(1)
                   Line Input #1, str
                   Arr = Split(str, ",")
                   If rSSN = "" And Arr(0) <> "BF_SSN" Then
                        SP.Common.AddLP50 Arr(0), "KATNY", "SKPBRWATRN", "LT", "33", "Letter sent to reference for skip assistance"
                        'write to log file
                        Open LogFolder & RFName For Output As #2
                            Write #2, Arr(0)
                        Close #2
                   Else
                        If Arr(0) = rSSN Then rSSN = ""
                   End If
                Loop
            Close #1
        
            If Recover And rSSN <> "" Then
                MsgBox "The script tried to recover but the recovery SSN was not found in the file. Contact Systems Support."
                End
            End If
            
            'print letters
            SP.CostCenterPrinting.Main DocPath, Doc, "Skip Borrower Attorney Reference Letter", Page1, Doc, Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & Fname, "BF_RFR", "ATNYSKP", True, , lrReference), timeStamp, "SKPBRWATRN", Recover
        End If
        If Dir(LogFolder & RFName) <> "" Then Kill LogFolder & RFName
        If Dir(ftpFolder & Fname) <> "" Then Kill ftpFolder & Fname
    Loop
    
End Sub


'<1>, SR1647, aa

