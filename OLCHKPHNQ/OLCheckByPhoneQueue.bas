Attribute VB_Name = "OLCheckByPhoneQueue"
Public Sub Main()
    Dim InRecovery As Boolean
    Dim LastSSNProc As String
    Dim ssn As String
    Dim Name As String
    Dim PayAmt As String
    Dim RoutingNum As String
    Dim AccNum As String
    Dim AccType As String
    Dim CommentAddedFor As String
    Dim PaymentEffDate As String
    Dim DocFolder As String
    Dim BorrAN As String
    
    If Not CalledByMBS Then
        If vbOK <> MsgBox("This script processes CKBYPHON queue tasks.  Click OK to continue or Cancel to end the script.", vbOKCancel, "CKBYPHONE Queue Processing") Then End
    End If
    DocFolder = "X:\PADD\CheckbyPhone\"
    SP.Common.TestMode , DocFolder
    
    'check if the script has already been run to completion yet today
    If Dir(DocFolder & "CBP " & Format(Date, "MMDDYY") & ".txt") <> "" Then
        MsgBox "The script has already run to completion successfully today."
        End
    End If
    
    LastSSNProc = Replace(DoRecoveryThing(), """", "")
    If LastSSNProc <> "" Then InRecovery = True
    
    While GetQueueTaskInfo(ssn, Name, PayAmt, RoutingNum, AccNum, AccType, CommentAddedFor, BorrAN, PaymentEffDate)
        If LastSSNProc <> ssn Or InRecovery = False Then 'recover check
            'add activity comment
            Wait "2"
            AddLP50 ssn, "DPNCK", "OLCHKPHNQ", "AM", "10", "Check by phone payment being added to FirstPointe. Effective " + PaymentEffDate + " for payment amount" + PayAmt + "."
            CommentAddedFor = ssn 'note that a comment has been added for the SSN
            'insert info into the data file
            Open "T:\CBP.txt" For Append As #1
            Write #1, ssn, Name, PayAmt, RoutingNum, AccNum, AccType, BorrAN, PaymentEffDate
            Close #1
        Else 'if in recovery
            CommentAddedFor = ssn 'note that a comment has been added for the SSN (the last time that the script was processed)
            InRecovery = False
        End If
    Wend
    
    CreateReport
    'create comma delimited file without quotes and with account type codes for FirstPointe
    Open "T:\CBP.txt" For Input As #1
    Open DocFolder & "CBP " & Format(Date, "MMDDYY") & ".txt" For Output As #2
    Do While Not EOF(1)
        Input #1, ssn, Name, PayAmt, RoutingNum, AccNum, AccType, BorrAN, PaymentEffDate
        If AccType = "C" Then AccType = "27" Else AccType = "37"
        Print #2, ssn & "," & Name & "," & PayAmt & "," & RoutingNum & "," & AccNum & "," & AccType & "," & PaymentEffDate
    Loop
    Close
    
    Kill "T:\CBP.txt"
    ProcComp "MBSOLCHKPHNQ.txt"
End Sub

'this function gets the next queue task
Private Function GetQueueTaskInfo(ssn As String, Name As String, PayAmt As String, RoutingNum As String, AccNum As String, AccType As String, Optional CommentAddedFor As String = "", Optional BorrAN As String, Optional PaymentEffDate As String) As Boolean
    FastPath "LP9ACCKBYPHON" 'access the CKBYPHON queue
    
    'check if anything is found in the queue
    If Check4Text(1, 66, "QUEUE SELECTION") And (Check4Text(22, 3, "47423 ASSIGNED TASKS NOT FOUND") Or Check4Text(22, 3, "47420")) Then
        MsgBox "There are no tasks in the queue to process."
        'check if there's a data file
        If Dir("T:\CBP.txt") <> "" Then
            MsgBox "The script will now create a report from what information it has."
            CreateReport 'create report if data file exists
        End If
        ProcComp "MBSOLCHKPHNQ.txt"
    End If
    
    If Check4Text(3, 24, "CKBYPHON") = False Then
        MsgBox "You have an unresolved task in another queue.  Please resolve it and run the script."
        End
    End If
    
    'complete the task for the SSN if a comment has been added for the SSN (see variable assignment in Main function)
    If CommentAddedFor = GetText(17, 70, 9) Then
        Hit "F6"
        Hit "F8"
    End If
    
    'process the next task if on exists
    If Check4Text(1, 71, "QUEUE TASK") And Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False Then 'check if there are any more tasks
        'retrieve queue task info
        ssn = GetText(17, 70, 9)
        Name = GetText(17, 6, 37)
        PayAmt = GetText(12, 12, 8)
        RoutingNum = GetText(12, 21, 9)
        AccNum = GetText(12, 31, 20)
        BorrAN = Replace(SP.Q.GetText(17, 52, 12), " ", "")
        AccType = GetText(12, 51, 1)
        PaymentEffDate = GetText(12, 52, 10)
        GetQueueTaskInfo = True 'a queue task was retrieved
    Else 'there are no more queue tasks
        MsgBox "All queue tasks have been processed.  The script will now e-mail a report to accounting.  Please stand by."
        GetQueueTaskInfo = False 'a queue task wasn't retrieved
    End If
End Function

'this function retrieves the last SSN successfully processed
Private Function DoRecoveryThing()
    Dim dataLine As String
    Dim DataFields() As String
    If Dir("T:\CBP.txt") = "" Then
        DoRecoveryThing = ""
        Exit Function
    Else
        MsgBox "The script is now in recovery mode."
        Open "T:\CBP.txt" For Input As #1
        'find the end of the file
        While Not EOF(1)
            Line Input #1, dataLine
            DataFields = Split(dataLine, ",")
            'return the last successfully processed
            DoRecoveryThing = DataFields(0)
        Wend
        Close #1
    End If
End Function

'This function creates the Excel Report
Private Function CreateReport()
    Dim dataLine As String
    Dim DataFields() As String
    Dim Total As Currency
    Dim RowCounter As Integer
    Dim counter As Integer
    Dim ExcelApp As Excel.Application
    
    Set ExcelApp = CreateObject("Excel.Application")
    'open data file
    Open "T:\CBP.txt" For Input As #1
    
    'figure totals
    While Not EOF(1)
        Line Input #1, dataLine
        DataFields = Split(dataLine, ",")
        Total = Total + CCur(Replace(DataFields(2), """", ""))
        RowCounter = RowCounter + 1 'increment array row counter
    Wend
    Close #1
    
    'sort comma delimited file in excel, create report, print report
    With ExcelApp
        .Visible = True
        .Workbooks.OpenText Filename:="T:\CBP.txt", Origin _
        :=437, StartRow:=1, DataType:=xlDelimited, TextQualifier:=xlDoubleQuote _
        , ConsecutiveDelimiter:=False, Tab:=True, Semicolon:=False, Comma:=True _
        , Space:=False, Other:=False, FieldInfo:=Array(Array(1, 9), Array(2, 2), _
        Array(3, 2), Array(4, 9), Array(5, 9), Array(6, 9), Array(7, 2)), TrailingMinusNumbers:=True
        
        .columns("A:A").Select
        .Selection.Insert Shift:=xlDown
        .Range("D1", "D" & RowCounter).Copy .Range("A1", "A" & RowCounter)
        .Range("D1", "D" & RowCounter).Delete
        .columns("A:A").Select
        .Range("A1:F" & RowCounter).Sort Key1:=.Range("A1"), Order1:=xlAscending, header:= _
        xlGuess, OrderCustom:=1, MatchCase:=False, Orientation:=xlTopToBottom, _
        DataOption1:=xlSortNormal
        'write out header rows
        .Rows("1:1").Select
        .Selection.Insert Shift:=xlDown
        .Worksheets("CBP").Cells(1, 1).value = "Account Number"
        .Worksheets("CBP").Cells(1, 2).value = "Full Name"
        .Worksheets("CBP").Cells(1, 3).value = "Payment Amount" ''Switched to conform with report.
        .Worksheets("CBP").Cells(1, 4).value = "Effective Date" ''
        RowCounter = RowCounter + 1
        RowCounter = RowCounter + 1
        RowCounter = RowCounter + 1
        RowCounter = RowCounter + 1
        .Worksheets("CBP").Cells(RowCounter, 1).value = "Total # Items:"
        .Worksheets("CBP").Cells(RowCounter, 2).value = RowCounter - 4
        RowCounter = RowCounter + 1
        .Worksheets("CBP").Cells(RowCounter, 1).value = "Total Payment Amount:"
        .Worksheets("CBP").Cells(RowCounter, 2).value = Total

        'sort rows
        .Cells.Select
        'auto fit all columns
        .columns("A:F").Select
        .Selection.columns.AutoFit
        'add two rows at top of report then enter a report name
        .Rows("1:1").Select
        .Selection.Insert Shift:=xlDown
        .Selection.Insert Shift:=xlDown
        .Worksheets("CBP").Cells(1, 1).value = "OneLINK Check By Phone " & CStr(Date)
        'set up report to print to one page wide and landscape
        With .ActiveSheet.PageSetup
            .Orientation = xlLandscape
            .PaperSize = xlPaperLetter
            .FirstPageNumber = xlAutomatic
            .Order = xlDownThenOver
            .BlackAndWhite = False
            .Zoom = False
            .FitToPagesWide = 1
            .FitToPagesTall = 1
            .PrintErrors = xlPrintErrorsDisplayed
        End With
        'Save the worksheet to the X drive.
        If (SP.Common.TestMode()) Then
            .ActiveWorkbook.SaveAs Filename:="X:\PADD\CheckByPhone\Reports\Test\CBP Balance Report " & Format(Date, "MMDDYY") & ".xls", FileFormat:=xlNormal, CreateBackup:=False
        Else
            .ActiveWorkbook.SaveAs Filename:="X:\PADD\CheckByPhone\Reports\CBP Balance Report " & Format(Date, "MMDDYY") & ".xls", FileFormat:=xlNormal, CreateBackup:=False
        End If
        .ActiveWindow.Close False
        .Quit
    End With
    'Notify users that the report is available.
    SP.Common.SendMail SP.Common.RecipientString("OneLINK Chk By Phone.txt"), , "Check by Phone Posting Report available", "Today's CBP report is available in X:\PADD\CheckByPhone\Reports.", , , , , SP.Common.TestMode()
End Function

'used for testing only
Private Sub addtasks()
    AddLP9O "529664720", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529580804", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529158292", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529139269", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528882215", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528800566", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528481182", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528134983", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528114045", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "526687646", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "520600759", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "455908986", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "519862511", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "402233083", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528933698", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528412481", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "485962397", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529860666", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528298006", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529984520", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "551915335", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "543649965", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528797247", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "552598485", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529270048", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529454169", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "519949070", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "272847362", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "045423169", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "575115226", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528981920", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "512764548", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "320502486", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "553732393", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529254588", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528821721", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "518041090", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "570745963", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529803728", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529113287", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "466435554", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "601202879", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529047070", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528921705", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529649300", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "425256237", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "647054317", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "263949497", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "550633575", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529809777", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529769177", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529661700", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529112143", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528049845", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "518133728", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "278526695", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "509765922", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "193688770", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529290889", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "519136661", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "444569303", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528948886", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528794315", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "585491115", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "432335206", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "519648220", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528887303", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "527151170", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529215846", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528636920", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "398420049", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528170274", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529111252", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "457576294", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528961049", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "617263744", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528868109", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "519921940", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "529232366", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "528693548", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "445780538", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "521761016", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    AddLP9O "243438700", "CKBYPHON", , "$00077.54 801321212 8013212121          S"
    MsgBox "Done"
End Sub

