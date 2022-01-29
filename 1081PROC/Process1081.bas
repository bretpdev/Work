Attribute VB_Name = "Process1081"
'1081 Processing
'script ID = 1081PROC

Private ssn As String
Private lname As String
Private MI As String
Private fName As String
Private v As String
Private clm() As String
Private UID() As String

Sub Main()
'<5>
    'In some cases, local variables with the same names as some public variables
    ' are not properly going out of scope. The following function call will clear
    ' the contents of those variables so that the previous record's information
    ' does not persist into this iteration.
    ResetPublicVars
'</5>
    ssn = InputBox("Enter an SSN", "1081 Processing")
    If Len(ssn) <> 9 Then
        MsgBox "This is not a valid SSN."
        End
    End If
    v = ""
    If Sp.Common.GetLP22(ssn, v, lname, MI, fName, v, v, v, v, v, v, v, v, v, v) = False Then
        MsgBox "SSN does not exist on system."
        End
    End If
    CheckBalance
    MsgBox "Script complete."
End Sub

Function CheckBalance() As Boolean
   Dim Bal As String
   Dim X As Integer
   Dim found As Boolean
   Dim Claims As String
   Dim d As String
   
   
   found = False
   Sp.Q.FastPath "LC05I"
   
   'MsgBox CStr(DateDiff("d", Now - 30, "01072005")) & " " & Now - 30
   'End
   
   If RTrim(Sp.Q.GetText(4, 70, 11)) <> "0.00" Then
      Sp.Q.FastPath "LC44I"
      X = 0
      'Determin if a DC record exists withing the last 30 days
      Do
          For X = 0 To 11
              If Sp.Q.GetText(7 + X, 19, 2) = "DC" Then
                  d = Sp.Q.GetText(7 + X, 10, 8)
                  d = Mid(d, 1, 2) & "/" & Mid(d, 3, 2) & "/" & Mid(d, 5, 4)
                  'if date within the last 30 days
'<1->
'                 If DateDiff("d", d, Now) < 365 Then
                  If DateDiff("d", d, Now) < 30 Then
'</1>
                      found = True
                  End If
              End If
          
          
          Next X
          Sp.Q.hit "F8"
          If Sp.Q.GetText(22, 3, 5) = "46004" Then
               'no more data to display
              Exit Do
          End If
      Loop
      If found = False Then
          MsgBox "Loans have not been closed out. Try again later."
          End
      Else
          MsgBox "Determine which loans were found and press insert to continue."
          Sp.Q.PauseForInsert
          'Claims = InputBox("Enter claim numbers that were not funded.", "1081 Processing")
          Proc1081frm.Show
          If Proc1081frm.ClmNum = "" Or Proc1081frm.CLUID = "" Then
               MsgBox "You did not enter a claim. Terminating script."
               End
          End If
          clm = Split(Proc1081frm.ClmNum, ",")
          UID = Split(Proc1081frm.CLUID, ",")
          ReDim Preserve clm(17) As String
          ReDim Preserve UID(17) As String
          
          'add comments
          Sp.Common.AddLP9O ssn, "F1081FRM", , "Partial funding from Direct Loans report sent to Direct Loans"
          Sp.Common.AddLP50 ssn, "F1081", "1081PROC", "FO", "32", "Partial funding from direct loans; loan's " & Proc1081frm.ClmNum & " were not funded; sent report to Direct loans to request additional funds."
          Open "T:\P1081.txt" For Output As #1
          Write #1, "SSN", "Fname", "Lname", "MI", "ACSKeyLine", _
          "CLM1", "CLM2", "CLM3", "CLM4", "CLM5", "CLM6", "CLM7", "CLM8", "CLM9", _
          "CLM10", "CLM11", "CLM12", "CLM13", "CLM14", "CLM15", "CLM16", "CLM17", "CLM18", _
          "UID1", "UID2", "UID3", "UID4", "UID5", "UID6", "UID7", "UID8", "UID9", _
          "UID10", "UID11", "UID12", "UID13", "UID14", "UID15", "UID16", "UID17", "UID18"
          
          Write #1, ssn, fName, lname, MI, Sp.Common.ACSKeyLine(ssn), clm(0), clm(1), clm(2), clm(3), clm(4), clm(5), clm(6), clm(7), clm(8), clm(9), clm(10), clm(11), clm(12), clm(13), clm(14), clm(15), clm(16), clm(17) _
          , UID(0), UID(1), UID(2), UID(3), UID(4), UID(5), UID(6), UID(7), UID(8), UID(9), UID(10), UID(11), UID(12), UID(13), UID(14), UID(15), UID(16), UID(17)
          Close #1
          'PRINT PARTIAL FUNDING LETTER
          'save to imaging using DOC ID PCLVC
          PrintAndImageLetters "PCLVC", "DLPRTLFND"
      End If
'<4>      MsgBox "Processing Complete"'</4>
   Else
   'if balance is Zero
       Dim pi As String
       Dim ACAmounts As Double
       Dim Diff1081
       pi = InputBox("Enter the Principal/Interest Amount", "1081 Processing", Default)
       If pi = "" Then End                                              '<1>
       'interest = InputBox("Enter the Interest Amount", "1081 Processing", Default)
       Sp.Q.FastPath ("LC44I")
       Do
          For X = 0 To 11
              If Sp.Q.GetText(7 + X, 19, 2) = "AC" Then
                  d = Sp.Q.GetText(7 + X, 10, 8)
                  d = Mid(d, 1, 2) & "/" & Mid(d, 3, 2) & "/" & Mid(d, 5, 4)
                  'if AC date within the last 365 days add amounts
                  'MsgBox d & "**" & Now & "**" & DateDiff("d", d, Now) & "**" & DateDiff("d", d, Now - 30)
                  If DateDiff("d", d, Now) < 365 Then
                       ACAmounts = ACAmounts + CDbl(Sp.Q.GetText(7 + X, 22, 10))
                  End If
              End If
          Next X
          Sp.Q.hit "F8"
          If Sp.Q.GetText(22, 3, 5) = "46004" Then
              'no more data to display
              Exit Do
          End If
      Loop
'<3>  Diff1081 = pi - ACAmounts
'<4>
'     Diff1081 = ACAmounts - pi     '<3>
      Diff1081 = pi - ACAmounts
'</4>
      If Diff1081 > -25# And Diff1081 < 25# Then
           Sp.Common.AddLP50 ssn, "FSR81", "P1081", "FO", "32"
'<4>           MsgBox "Process complete."'</4>
      ElseIf Diff1081 > 25# Then      'overpayment
'<1->
'          SP.Common.AddLP9O SSN, "F1081FRM", , "Consolidation funding overpayment; " & Diff1081 & " adjustment sent to direct loans."
'          SP.Common.AddLP50 SSN, "FUO81", "1081PROC", "FO", "32", "Consolidation overpayment in the amount of " & Diff1081 & "; letter sent to Direct loans."
           Sp.Common.AddLP9O ssn, "F1081FRM", , "Consolidation funding overpayment; " & FormatCurrency(Diff1081, 2) & " adjustment sent to direct loans."
           Sp.Common.AddLP50 ssn, "FUO81", "1081PROC", "FO", "32", "Consolidation overpayment in the amount of " & FormatCurrency(Diff1081, 2) & "; letter sent to Direct loans."
'</1>
           Open "T:\P1081.txt" For Output As #1
           Write #1, "SSN", "Fname", "Lname", "MI", "ACSKeyLine", "principal_interest", "Diff1081"
           Write #1, ssn, fName, lname, MI, Sp.Common.ACSKeyLine(ssn), FormatCurrency(pi, 2), FormatCurrency(Diff1081, 2)
           Close #1
           'CREATE DOCUMENT NOTICE OF OVERPAYMENT
           'SAVE TO IMAGING, DOC ID PCLVC
           PrintAndImageLetters "PCLVC", "DLCONOVRP"
      ElseIf Diff1081 < -25# Then      'underpayment
'<1->
'          SP.Common.AddLP9O SSN, "F1081FRM", , "Consolidation funding underpayment; " & Diff1081 & " adjustment sent to direct loans."
'          SP.Common.AddLP50 SSN, "FUU81", "1081PROC", "FO", "32", "Consolidation funding underpayment; " & Diff1081 & "; adjustment sent to Direct loans."
           Sp.Common.AddLP9O ssn, "F1081FRM", , "Consolidation funding underpayment; " & FormatCurrency(Diff1081, 2) & " adjustment sent to direct loans."
           Sp.Common.AddLP50 ssn, "FUU81", "1081PROC", "FO", "32", "Consolidation funding underpayment; " & FormatCurrency(Diff1081, 2) & "; adjustment sent to Direct loans."
'</1>
           Open "T:\P1081.txt" For Output As #1
           Write #1, "SSN", "Fname", "Lname", "MI", "ACSKeyLine", "principal_interest", "Diff1081"
           Write #1, ssn, fName, lname, MI, Sp.Common.ACSKeyLine(ssn), FormatCurrency(pi, 2), FormatCurrency(Abs(Diff1081), 2, , vbFalse)
           Close #1
           'CREATE DOCUMENT NOTICE OF UNDERPAYMENT
           'SAVE TO IMAGING, DOC ID PCLVC
           
           PrintAndImageLetters "PCLVC", "DLCONUNDP"
      End If
      
   End If

End Function
'this function prints and images the credit denial letters
Function PrintAndImageLetters(DID As String, DocName As String)
    With Session
        Dim DocID As String
        Dim ArchLib As String
        Dim FD As String
        Dim FileData() As String
        Dim Dat As String
        Dim docPath As String
        Dim Doc As String
        
        Doc = DocName
        'set imaging and printing parameters
'<3->
'        If Mid(Session.CommandLineSwitches, Len(Session.CommandLineSwitches) - 7, 3) = "Dev" Or Mid(Session.CommandLineSwitches, Len(Session.CommandLineSwitches) - 7, 3) = "Tst" Then
'            ArchLib = "Test"
'            DocPath = "X:\PADD\Accounting\"
'            PrnCls = "N"
'        Else
'            ArchLib = "CR"
'            DocPath = "X:\PADD\Accounting\"
'            PrnCls = "Y"
'        End If
        docPath = "X:\PADD\Accounting\"
        If TestMode(, docPath) Then
            ArchLib = "Test"
            PrnCls = "N"
        Else
            ArchLib = "CR"
            PrnCls = "Y"
        End If
'</3>

        DocID = DID
        Dat = "T:\P1081.txt"
        Open "T:\P1081.txt" For Input As #1
        'read in header row
        Line Input #1, FD
        FileData = Split(FD, ",")
        PleaseWait.Show 0
        'set up Word object
        Dim Word As Word.Application
        Set Word = CreateObject("Word.Application")
        Word.Visible = False
        'open form doc
        Word.Documents.Open FileName:=docPath & Doc & ".doc", ConfirmConversions:=False, _
            ReadOnly:=False, AddToRecentFiles:=False, PasswordDocument:="", _
            PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
            WritePasswordTemplate:="", Format:=wdOpenFormatAuto
        Word.ActiveDocument.MailMerge.OpenDataSource name:=Dat, _
            ConfirmConversions:=False, ReadOnly:=False, LinkToSource:=True, _
            AddToRecentFiles:=False, PasswordDocument:="", PasswordTemplate:="", _
            WritePasswordDocument:="", WritePasswordTemplate:="", Revert:=False, _
            Format:=wdOpenFormatAuto, Connection:="", SQLStatement:="", SQLStatement1 _
            :="", SubType:=wdMergeSubTypeOther
        Word.ActiveDocument.SaveAs FileName:="T:\" & Doc & ".doc", _
            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False
        
        'merge the data
        With Word.ActiveDocument.MailMerge
            .Destination = wdSendToNewDocument
            .MailAsAttachment = False
            .MailAddressFieldName = ""
            .MailSubject = ""
            .SuppressBlankLines = True
            'limit the record being merged to the current record number from the merge data file
            With .DataSource
                .FirstRecord = 1
                .LastRecord = 1
            End With
            .Execute Pause:=True
        End With
        'set time stamp to add to file names so files for the same borrower are not overwritten
        TmStmp = Timer
        TmStmp = Mid(TmStmp, 1, 5) & Mid(TmStmp, 7, 2)
        'protect
        Word.ActiveDocument.Protect 2, , TmStmp
        'save the file
        Word.ActiveDocument.SaveAs FileName:="\\imgprodkofax\ascent$\UT" & ArchLib & "Other_imp\" & ssn & "_" & TmStmp & ".doc", _
            FileFormat:=wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False
        'close the file
        Word.ActiveDocument.Close
        'create the imaging control file for the doc to be imaged
        Open "\\imgprodkofax\ascent$\UT" & ArchLib & "Other_imp\" & ssn & "_" & TmStmp & ".ctl" For Output As #4
        'write info to control file for correspondence
        Print #4, "~^Folder~" & Format(Date, "MM/DD/YYYY") & " " & time & ", Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~" & ssn & "^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~" & DocID & "^Attribute~DOC_DATE~STR~" & Format(Date, "MM/DD/YYYY") & "^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~" & Format(Date, "MM/DD/YYYY") & "^Attribute~SCAN_TIME~STR~" & Format(time, "HH:MM:SS") & "^Attribute~DESCRIPTION~STR~" & Format(Date, "MM/DD/YYYY") & " " & time
        Print #4, "DesktopDoc~\\imgprodkofax\ascent$\UTCROther_imp\" & ssn & "_" & TmStmp & ".doc~" & Format(Date, "MM/DD/YYYY") & " " & time & ", Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~" & ssn & "^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~" & DocID & "^Attribute~DOC_DATE~STR~" & Format(Date, "MM/DD/YYYY") & "^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~" & Format(Date, "MM/DD/YYYY") & "^Attribute~SCAN_TIME~STR~" & Format(time, "HH:MM:SS")
        Close #4

        'close Word
        Word.Application.Quit SaveChanges:=wdDoNotSaveChanges
        'Word.Visible = True
    End With
    Close #1
    PleaseWait.Hide
    'Print the letters
    Sp.Common.PrintDocs docPath, Doc, Dat
    Kill "T:\P1081.txt"
    Kill "T:\" & Doc & ".doc"
End Function

'<1> sr1056, jd, 04/27/05, 05/06/05
'<2> sr1408, jd, changed imaging server from \\uheaa_vssql\ascent to \\imgprodkofax\ascent$
'<3> sr1745, aa
'<4> sr1837, tp, 09/28/06
'<5> sr2241, db
