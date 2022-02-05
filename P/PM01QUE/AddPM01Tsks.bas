Attribute VB_Name = "AddPM01Tsks"
Private SASDir As String
Private LogDir As String
Sub Main()
    Dim SASFileToProc As String
    Dim RecoveryLine As String
    Dim FileLine As String
    Dim fields() As String
    Dim UID As String
    Dim ErrFile As String '<3>
    If Not SP.Common.CalledByMBS Then If vbOK <> MsgBox("This script populates the PM01 queue.  Click OK to continue or Cancel to end the script.", vbOKCancel, "Populate PM01 queue") Then End '<2>
    SP.Common.TestMode SASDir, , LogDir
'<3>
    ErrFile = "T:\AddPM01TsksError.txt"
'</3>
    If Dir(SASDir & "ULWO38.LWO38R2*") = "" Then
        MsgBox "SAS ""ULWO38.LWO38R2*"" does not exist.  Please contact Systems Support for assistance.", vbCritical
        End
    End If
    If AllFilesEmpty() Then
        MsgBox "All SAS files are empty.  There is nothing to process today.", vbCritical
        ProcComp "MBSPM01QUE.TXT", False '<2>
        End
    End If
    'get user id for TD22
    UID = SP.Common.GetUserID()
    'check for recovery mode
    If Dir(LogDir & "Adding PM01 Q Tsks.txt") <> "" Then
        Open LogDir & "Adding PM01 Q Tsks.txt" For Input As #1
        Line Input #1, RecoveryLine
        Close #1
    End If
    While RetrieveOldestFile(SASFileToProc)
        Open SASDir & SASFileToProc For Input As #1
        'if recovery information is populated then search the file for the last line to be processed
        If RecoveryLine <> "" Then
            While FileLine <> RecoveryLine
                Line Input #1, FileLine
            Wend
        End If
        'process line
        While Not EOF(1)
            Line Input #1, FileLine
            fields = Split(FileLine, ",")
'<3>
            If SP.Common.ATD22AllLoans(fields(0), "RPACH", "Install amt = " & fields(1) & "; additional withdrawal amount = " & fields(2), "PM01QUE", UID) = False Then
                If SP.Q.Check4Text(23, 2, "01764 TASK RECORD ALREADY EXISTS") Then
                    'add to error file
                    Open ErrFile For Append As #5
                        Write #5, fields(0), SP.Q.GetText(4, 30, 40)
                    Close #5
                Else
                    MsgBox "You need access to the ""RPACH"" ARC to use this script.", vbCritical
                    End
                End If
            End If
'            If SP.Common.ATD22AllLoans(Fields(0), "RPACH", "Install amt = " & Fields(1) & "; additional withdrawal amount = " & Fields(2), "PM01QUE", UID) = False Then
'                MsgBox "You need access to the ""RPACH"" ARC to use this script.", vbCritical
'                End
'            End If
'</3>
            'write to recovery file
            Open LogDir & "Adding PM01 Q Tsks.txt" For Output As #2
            Print #2, FileLine
            Close #2
        Wend
        Close #1
        Kill SASDir & SASFileToProc
    Wend
'<3>
    PrintErrorFile ErrFile
'</3>
    Kill LogDir & "Adding PM01 Q Tsks.txt" 'delete log file
    '<2>MsgBox "Processing Complete", vbInformation
    ProcComp "MBSPM01QUE.TXT" '<2>
    End
End Sub


'this function iterates through all files with SAS naming convention and deletes all empty files and returns true if all of them were empty
Function AllFilesEmpty() As Boolean
    Dim Temp As String
    AllFilesEmpty = True
    Temp = Dir(SASDir & "ULWO38.LWO38R2*")
    While Temp <> ""
        If FileLen(SASDir & Temp) = 0 Then
            Kill SASDir & Temp
        Else
            AllFilesEmpty = False
        End If
        Temp = Dir()
    Wend
End Function

'this function retrieves the oldest file with SAS file naming convention
Function RetrieveOldestFile(SASFTP As String) As Boolean
    Dim Temp As String
    Dim SASFTPTDS As Date
    SASFTP = Dir(SASDir & "ULWO38.LWO38R2*")
    If SASFTP = "" Then
        RetrieveOldestFile = False 'return false to end processing loop
    Else
        SASFTPTDS = FileDateTime(SASDir & SASFTP)
        Temp = SASFTP
        While Temp <> ""
            If FileDateTime(SASDir & SASFTP) > FileDateTime(SASDir & Temp) Then
                SASFTP = Temp
            End If
            Temp = Dir()
        Wend
        RetrieveOldestFile = True 'return true to process the file just found
    End If
End Function
'<3>
Sub PrintErrorFile(EF As String)
Dim ESSN As String
Dim EName As String

    If Dir(EF) <> "" Then
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")
        'open form doc
        Word.Visible = False
        Word.Documents.Add DocumentType:=wdNewBlankDocument
        'add a header
        Word.Selection.Font.Size = 20
        Word.Selection.TypeText TEXT:="Duplicate SSN in PM 01 Queue"
        Word.Selection.TypeParagraph
        Word.Selection.TypeParagraph
        Word.Selection.Font.Size = 12
        Open EF For Input As #1
        Do While Not EOF(1)
            Input #1, ESSN, EName
            
            Word.Selection.TypeText TEXT:=ESSN & "  " & EName
            Word.Selection.TypeParagraph
        Loop
        Close #1
        'C:\Windows\Temp\KeyLoanIdentifierDiscrepanciesRPT.txt
        Word.ActiveDocument.SaveAs Filename:="T:\AddPM01TsksError.doc", _
            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False
        Session.Wait 2
        Word.ActiveDocument.PrintOut False
        Word.Application.Quit SaveChanges:=wdDoNotSaveChanges
    End If
    If Dir(EF) <> "" Then Kill EF
End Sub
'</3>
'new sr1289, aa
'<1>,sr1455, aa
'<2>,sr1549, tp, 05/01/06
'<3>,sr1668, tp, 06/13/06

