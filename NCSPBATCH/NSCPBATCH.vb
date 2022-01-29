Imports System.IO.File

Module NSCPBATCH
    Public Sub Main()
        'prompt user
        Select Case Month(Today)
            Case 2, 6, 7, 8, 10
                If MsgBox("This application reviews NCSP accounts and closes them if they meet specified criteria.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "New Century Scholarship Program Batch Processing") = MsgBoxResult.Cancel Then
                    End
                End If
            Case Else
                MsgBox("This application reviews NCSP accounts and closes them if they meet specified criteria.  However, there are no processes that need to be run this month.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "New Century Scholarship Program Batch Processing")
                End
        End Select       

        'display splash screen
        SplashForm.Show()
        SplashForm.Refresh()
        SplashForm.lblStatus.Text = "New Century Scholarship Program Batch Processing"

        'set up new copy of data file
        If Not Exists("T:\NCSP_batch_dat.txt") Then
            FileOpen(1, "T:\NCSP_batch_dat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            WriteLine(1, "AcctID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Reason", "BalanceOwed")
            FileClose(1)
        End If

        SetUpConnections()

        'run processes
        ProcessFallSemesterRule()
        ProcessProofofEnrollment()
        ProcessProofofCompletion()

        'print letters
        If ProcessedRecordsCount > 0 Then
            ResetProgressBar(2, "Printing")

            'print coversheet
            FileOpen(1, "T:\NCSP_cover_dat.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            WriteLine(1, "BU", "Description", "Cost", "Standard", "Foreign")
            WriteLine(1, "School Services", "Closed Account Letters", "", ProcessedRecordsCount, 0)
            FileClose(1)
            PrintDocs(CoverDocPath & "Scripted State Mail Cover Sheet (DEL TO BU).doc", "T:\NCSP_cover_dat.txt")
            SplashForm.ProgressBar.PerformStep()

            'print letters
            PrintDocs(DocFolder & "NCSACNTCLS.doc")
            SplashForm.ProgressBar.PerformStep()

            'clean up files
            If Exists("T:\NCSPdat.txt") Then Delete("T:\NCSPdat.txt")
            If Exists("T:\NCSP_batch_dat.txt") Then Delete("T:\NCSP_batch_dat.txt")
            If Exists("T:\NCSP_cover_dat.txt") Then Delete("T:\NCSP_cover_dat.txt")

            'prompt user
            MsgBox("Processing complete.", MsgBoxStyle.Information, "New Century Scholarship Program Batch Processing")
        Else
            MsgBox("There were no accounts which needed to be closed.  Processing complete.", MsgBoxStyle.Information, "New Century Scholarship Program Batch Processing")
        End If

        'close splash form
        SplashForm.Close()
    End Sub
End Module