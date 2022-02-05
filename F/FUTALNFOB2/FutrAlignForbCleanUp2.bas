Attribute VB_Name = "FutrAlignForbCleanUp2"
Sub Main()
    Dim SASDir As String
    Dim LogDir As String
    Dim Rec As String
    Dim Flds() As String
    Dim SearchDate As String
    Dim RLine As String
    SP.Common.TestMode SASDir, , LogDir
    If MsgBox("This script automates the erroring of COMPASS activity comments for removal of invalid alignment forbearances.  Click OK to continue.", vbOKCancel) = vbCancel Then End
    'this script only processes the R5 file for the SAS job
    If Dir(SASDir & "AlignRepayForbCU.R5*") = "" Then
        MsgBox "The SAS processing file couldn't be found.  Please contact Systems Support.", vbOKOnly
        End
    ElseIf FileLen(SASDir & Dir(SASDir & "AlignRepayForbCU.R5*")) = 0 Then
        MsgBox "The SAS processing file had no data in it.  Please contact Systems Support.", vbOKOnly
        End
    End If
    If Dir(LogDir & "FutrAlignForbCleanUp2 Log.txt") <> "" Then
        'if in recovery
        Open LogDir & "FutrAlignForbCleanUp2 Log.txt" For Input As #2
        Line Input #2, SearchDate
        Line Input #2, RLine
        Close #2
    Else
        'if not in recovery
        While IsDate(SearchDate) = False
            SearchDate = InputBox("Please enter a date.", "Please enter a date.", "MM/DD/YYYY")
        Wend
    End If
    'open SAS file for processing
    Open SASDir & Dir(SASDir & "AlignRepayForbCU.R5*") For Input As #1
    'get header row
    Line Input #1, Rec
    'check if the script needs to recover
    If RLine <> "" Then
        While RLine <> Rec
            'search for recovery point
            Line Input #1, Rec
        Wend
    End If
    While Not EOF(1)
        Line Input #1, Rec
        'Flds indicies BF_SSN,LN_SEQ,LD_FOR_BEG,LD_FOR_END
        Flds = Split(Rec, ",")
        FastPath "TX3Z/CTD2A" & Flds(0)
        puttext 11, 65, "H129Q"
        puttext 21, 16, Format(CDate(SearchDate) - 1, "MMDDYY") & Format(CDate(SearchDate), "MMDDYY"), "Enter"
        'If check4text(1, 72, "TDX2B") = False Then 'for testing
            If Check4Text(1, 72, "TDX2C") Then
                puttext 7, 2, "X", "Enter"
                puttext 6, 18, "E", "Enter"
            Else
                puttext 6, 18, "E", "Enter"
            End If
        'End If
        Open LogDir & "FutrAlignForbCleanUp2 Log.txt" For Output As #2
        Print #2, SearchDate
        Print #2, Rec
        Close #2
    Wend
    Close #1
    Kill LogDir & "FutrAlignForbCleanUp2 Log.txt"
    Kill SASDir & Dir(SASDir & "AlignRepayForbCU.R5*")
    MsgBox "Processing Complete"
End Sub
