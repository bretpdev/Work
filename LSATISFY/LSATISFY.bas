Attribute VB_Name = "LSATISFY"
Private ftpFolder As String

'creates tasks in the LSATISFY queue for borrowers listed in a text file created by SAS
Sub LSATISFY()
    With Session
        'tell the user what the script does and end the script if the dialog box is canceled
        If Not SP.Common.CalledByMBS Then
            warn = MsgBox("This script creates tasks in the LSATISFY queue for borrowers listed in a text file created by SAS.  Click OK to continue or Cancel to quit.", vbOKCancel, "Satisfaction of Judgment")
            If warn <> 1 Then
                End
            End If
        End If
        SP.TestMode ftpFolder
        
        'specify the file to get from PHEAA's Cyprus server and go to the Common.FTP subroutine to get the file
'<1->
        Dim Temp As String
        Temp = Dir("T:\ULWPF2.LWPF2R2")
        If Temp <> "" Then Kill "T:\ULWPF2.LWPF2R2"
        Set fso = CreateObject("Scripting.FileSystemObject")
        SasFile = Common.SASProcessing(ftpFolder & "ULWPF2.LWPF2R2.*.*", "MBSLSATISFY.TXT")
        
        While SasFile <> ""         '<3>
            fso.movefile ftpFolder & SasFile, "T:\ULWPF2.LWPF2R2"
            
            SasFile = "ULWPF2.LWPF2R2"
            'Common.FTP
'</1>
            'open the input file
            Open "T:\ULWPF2.LWPF2R2" For Input As #1
            Input #1, ssn
            If DateValue(ssn) < Date - 3 Then
                warn = MsgBox("The SAS data file was created on " & Format(ssn, "MM/DD/YYYY") & ".  Since the file was not created recently, it appears the process may not have run as expected.  Do you want to process the file anyway?", vbYesNo, "Old Data File")
                If warn = 7 Then
                    End
                End If
            End If
            Queue = "LSATISFY"
            DateDue = Date
            LP9OComment1 = "satisfy legal judgment"
            LP9OComment2 = ""
            LP9OComment3 = ""
            LP9OComment4 = ""
            Do While Not EOF(1)
                Input #1, ssn
                Common.LP9O
            Loop
            Close #1
            '<3->
            Kill "T:\ULWPF2.LWPF2R2"
            SasFile = GetNextPopulatedFile(ftpFolder, "ULWPF2.LWPF2R2.*.*")
            '</3>
        Wend            '<3>
        'tell the user that processing is complete
'<2->
'       MsgBox "Processing Complete"
        ProcComp "MBSLSATISFY.txt"
'</2>
    End With
End Sub

'searches for the next file to be populated.  If file found is empty then it deletes it and searches for another.
Function GetNextPopulatedFile(Pname As String, Fname As String) As String
    GetNextPopulatedFile = Dir(Pname & Fname)
    If GetNextPopulatedFile <> "" Then
        While FileLen(Pname & GetNextPopulatedFile) = 0
            Kill Pname & GetNextPopulatedFile
            GetNextPopulatedFile = Dir(Pname & Fname)
            If GetNextPopulatedFile = "" Then
                Exit Function
            End If
        Wend
    End If
End Function

'<1> sr 455, aa, 10/24/03
'<2> sr1565, jd, disabled prompts if called by MBS
'<3> sr2020, aa

