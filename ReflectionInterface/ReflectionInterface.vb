Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings
Imports Microsoft.Office.Interop
Imports System.Data.SqlClient
Imports System.IO
Imports DMATRIXLib


Public Class ReflectionInterface
	Public Enum DestinationOrPageCount
		DocServices = -2
		BusinessUnit = -1
		OnePage = 1
		TwoPages = 2
		ThreePages = 3
		FourPages = 4
	End Enum

	Private Structure BusinessUnitStruct
		Public BusinessUnit As String
		Public CostCenter As String
	End Structure

	Private RefSession As Object
	Public TestMode As Boolean

	Public Sub New(ByVal tTestMode As Boolean)
		TestMode = tTestMode
		RefSessionSet()
	End Sub

	Public Function GetRefSession() As Object
		Return RefSession
	End Function

	'try and login
	Public Function LoginSuccessfully(ByVal TheID As String, ByVal ThePW As String) As Boolean
		'wait for the logon screen to be displayed
		RefSession.WaitForDisplayString(">", "0:0:30", 16, 10)
		If TestMode Then		  'test
			PutText(16, 12, "QTOR", True)
			'wait for the greetings screen to be displayed
			RefSession.WaitForDisplayString("USERID", "0:0:30", 20, 8)
			PutText(20, 18, TheID)
			PutText(20, 40, ThePW, True)
			If Check4Text(20, 8, "USERID==>") Then
				LoginSuccessfully = False
				Exit Function
			End If
			RefSession.FindText("RS/UT", 3, 5)
			PutText(RefSession.FoundTextRow, RefSession.FoundTextColumn - 2, "X", True)
		Else		  'live 
			PutText(16, 12, "PHEAA", True)
			'wait for the greetings screen to be displayed
			RefSession.WaitForDisplayString("USERID", "0:0:30", 20, 8)
			PutText(20, 18, TheID)
			PutText(20, 40, ThePW, True)
			If Check4Text(20, 8, "USERID==>") Then
				LoginSuccessfully = False
				Exit Function
			End If
		End If
		'check connection on LP00
		FastPath("LP00")
		If Check4Text(3, 32, "M A I N   M E N U") Then
			LoginSuccessfully = True
		Else
			LoginSuccessfully = False
		End If
	End Function

	'creates session and connects to hera and links to code
	Private Sub RefSessionSet()
		'create Reflection session
		RefSession = CreateObject("ReflectionIBM.Session")
		RefSession.BDTIgnoreScrollLock = True
		RefSession.Visible = True
		RefSession.Hostname = "hera"
		RefSession.Connect()
	End Sub

	Public Sub EndSession()
		RefSession.Exit()
	End Sub

	'send an e-mail message using SMTP
	Public Shared Function SendMail(ByVal mFrom As String, ByVal mTo As String, Optional ByVal mSubject As String = "", Optional ByVal mBody As String = "", Optional ByVal mCC As String = "", Optional ByVal mBCC As String = "", Optional ByVal mAttach As String = "") As Boolean
		Dim aAttach() As String
		Dim i As Integer
		Dim eMail As OSSMTP.SMTPSession
		eMail = New OSSMTP.SMTPSession

		'set server
		eMail.Server = "mail.utahsbr.edu"

		'create message
		eMail.MailFrom = mFrom
		eMail.SendTo = mTo
		eMail.CC = mCC
		eMail.BCC = mBCC
		eMail.MessageSubject = mSubject
		eMail.MessageText = mBody

		'add attachments if there are any
		If Len(mAttach) > 0 Then
			'split file names from string
			aAttach = Split(mAttach, ",")

			'add attachments
			For i = 0 To UBound(aAttach)
				eMail.Attachments.Add(aAttach(i))
			Next i
		End If

		'send message
		eMail.SendEmail()

		'verify the message was sent
		If eMail.Status = "SMTP connection closed" Then
			SendMail = True
		Else
			SendMail = False
		End If
	End Function

	Public Function ACSKeyLine(ByVal SSN As String, Optional ByVal PersonTyp As String = "P", Optional ByVal AddressTyp As String = "L") As String
		Dim SSNi(9) As String		'array of individual SSN characters
		Dim SSNc As String		  'decoded SSN
		Dim tKL As String		   'temp keyline
		Dim ChkDig As Integer		  'check digit
		Dim KLBit As Integer		   'keyline bit value
		Dim i As Integer

		'encrypt SSN
		If PersonTyp = "P" Then
			For i = 0 To 9
				Select Case Mid(SSN, i + 1, 1)
					Case 1
						SSNi(i) = "R"
					Case 2
						SSNi(i) = "E"
					Case 3
						SSNi(i) = "T"
					Case 4
						SSNi(i) = "H"
					Case 5
						SSNi(i) = "G"
					Case 6
						SSNi(i) = "U"
					Case 7
						SSNi(i) = "A"
					Case 8
						SSNi(i) = "L"
					Case 9
						SSNi(i) = "Y"
					Case 0
						SSNi(i) = "M"
				End Select
			Next i
			SSNc = SSNi(0) & SSNi(1) & SSNi(2) & SSNi(3) & SSNi(4) & SSNi(5) & _
			   SSNi(6) & SSNi(7) & SSNi(8)
			'encrypt ref id
		Else
			SSNc = Mid(SSN, 1, 2) & "/" & Mid(SSN, 4, 6)
		End If
		'add person type and address type to encrypted SSN/ref id for temp keyline
		tKL = PersonTyp & SSNc & String.Format("{0:0#}", Now.Month.ToString) & String.Format("{0:0#}", Now.Day.ToString) & AddressTyp
		'convert temp keyline characters to 4-bit numbers and calculate check digit
		ChkDig = 0
		For i = 0 To Len(tKL)
			Select Case Mid(tKL, i + 1, 1)
				Case "A"
					KLBit = 1
				Case "B"
					KLBit = 2
				Case "C"
					KLBit = 3
				Case "D"
					KLBit = 4
				Case "E"
					KLBit = 5
				Case "F"
					KLBit = 6
				Case "G"
					KLBit = 7
				Case "H"
					KLBit = 8
				Case "I"
					KLBit = 9
				Case "J"
					KLBit = 10
				Case "K"
					KLBit = 11
				Case "L"
					KLBit = 12
				Case "M"
					KLBit = 13
				Case "N"
					KLBit = 14
				Case "O"
					KLBit = 15
				Case "P"
					KLBit = 0
				Case "Q"
					KLBit = 1
				Case "R"
					KLBit = 2
				Case "S"
					KLBit = 3
				Case "T"
					KLBit = 4
				Case "U"
					KLBit = 5
				Case "V"
					KLBit = 6
				Case "W"
					KLBit = 7
				Case "X"
					KLBit = 8
				Case "Y"
					KLBit = 9
				Case "Z"
					KLBit = 10
				Case "/"
					KLBit = 15
				Case Else
					KLBit = Val(Mid(tKL, i + 1, 1))
			End Select
			'multiply the value by 2 if the position is odd
			If (i + 1) = 1 Or (i + 1) = 3 Or (i + 1) = 5 Or (i + 1) = 7 Or _
			(i + 1) = 9 Or (i + 1) = 11 Or (i + 1) = 13 Or (i + 1) = 15 _
			 Then KLBit = KLBit * 2
			'add the two digits of sum
			KLBit = Val(Mid(String.Format("{0:0#}", KLBit), 1, 1)) + Val(Mid(String.Format("{0:0#}", KLBit), 2, 1))
			'add the two digits of the sum
			KLBit = Val(Mid(String.Format("{0:0#}", KLBit), 1, 1)) + Val(Mid(String.Format("{0:0#}", KLBit), 2, 1))
			'add the sum to the check digit
			ChkDig = ChkDig + KLBit
		Next i
		'subtract the right digit of the check digit from 10 to get the final check digit
		ChkDig = 10 - Val(Right(ChkDig, 1))
		'if the check digit is 10, the check digit is 0
		If ChkDig = 10 Then ChkDig = 0
		'add the check digit to the end of the temp keyline to get the ACS Keyline
		ACSKeyLine = "#" & tKL & ChkDig & "#"
	End Function

    Public Function PrintDoc(ByVal Doc As String, ByVal Folder As String, ByVal inTest As Boolean, Optional ByVal Dat As String = "T:\TILPdat.txt")
        Dim newDoc As String

        If inTest = True Then Folder = Folder & "Test\"

        Doc = Folder & Doc & ".doc"

        'create the document
        Dim Word As New Word.Application
        Word.Visible = False
        'open merge document
        Word.Documents.Open(FileName:=Doc, ConfirmConversions:=False, _
         ReadOnly:=True, AddToRecentFiles:=False, PasswordDocument:="", _
         PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
         WritePasswordTemplate:="")
        'set data file
        Word.ActiveDocument.MailMerge.OpenDataSource(Name:=Dat, _
         ConfirmConversions:=False, ReadOnly:= _
         False, LinkToSource:=True, AddToRecentFiles:=False, PasswordDocument:="", _
         PasswordTemplate:="", WritePasswordDocument:="", WritePasswordTemplate:= _
         "", Revert:=False, Connection:="", SQLStatement _
         :="", SQLStatement1:="")
        'perform merge
        With Word.ActiveDocument.MailMerge
            .Destination = .Destination.wdSendToNewDocument
            .SuppressBlankLines = True
            .Execute(Pause:=False)
        End With
        'close form file
        Word.Documents(Doc).Close(False)
        If inTest Then
            'Show the file on screen.
            Word.Visible = True
        Else
            'print the file
            Word.ActiveDocument.PrintOut(Background:=False)
            Word.Application.Quit(False)
            MsgBox("Please retrieve your document from the printer.", MsgBoxStyle.Information, "Document Printed")
        End If
    End Function

	Function AddBarcodeAndStaticCurrentDate(ByVal FileToProc As String, ByVal AcctNumFieldNm As String, ByVal LetterID As String, ByVal TestMode As Boolean, Optional ByVal NewFile As Boolean = False, Optional ByVal AddStaticCurrentDate As Boolean = True) As String
		Dim HeaderRowAddition As String
		Dim OriginalHeaderRow As String
		Dim OHRFields() As String
		Dim AcctNumIndex As Integer
		Dim PaperSheetNumber As Integer
		Dim PaperSheetI As Integer
		Dim LineI As Long
		Dim Fields() As String
		Dim FieldI As Integer
		Dim NumberOfFields As Integer
		Dim NewRowData As String
		Dim RMBC() As String
		Dim SMBC() As String
		Dim Con As SqlClient.SqlConnection
		Dim Com As SqlClient.SqlCommand
		Dim Reader As SqlClient.SqlDataReader
		'query DB to get paper sheet count
		If TestMode Then
			Con = New SqlConnection("Data Source=""BART\BART"";Initial Catalog=BSYS;Integrated Security=SSPI;")
		Else
			Con = New SqlConnection("Data Source=NOCHOUSE;Initial Catalog=BSYS;Integrated Security=SSPI;")
		End If
		Com = New SqlCommand("SELECT Pages, Duplex FROM LTDB_DAT_CentralPrintingDocData WHERE ID = '" & LetterID & "'", Con)
		Con.Open()
		Reader = Com.ExecuteReader()


		If Reader.Read = False Then
			MsgBox("The letter id that the script is using doesn't appear to exist.  Please contact a member of Systems Support", vbCritical, "Error")
			Return ""
		ElseIf Reader("Pages") = 0 Then
			MsgBox("The paper sheet count for the letter id that the script is using isn't populated.  Please contact a member of Systems Support", vbCritical, "Error")
			Return ""
		Else
			If Reader("Pages") = 1 Then			 'if page number equals 1 then the sheet count is 1
				PaperSheetNumber = 1
			ElseIf Reader("Duplex") = False Then			 'if not duplex then sheet count equals page count
				PaperSheetNumber = Reader("Pages")
			Else			 'if marked to do duplex then figure out how many pages it is going to take
				PaperSheetNumber = Reader("Pages") \ 2
				If (Reader("Pages") Mod 2) > 0 Then
					PaperSheetNumber = PaperSheetNumber + 1
				End If
			End If
		End If
		'buffer letter id with spaces
		While Len(LetterID) <> 10
			LetterID = " " & LetterID
		End While

		FileOpen(1, FileToProc, OpenMode.Input)
        FileOpen(2, "T:\Add Return Mail Barcode Temp " & LetterID & ".txt", OpenMode.Output)
		'get header row
		OriginalHeaderRow = LineInput(1)
		'set up header row addition
		If PaperSheetNumber = 1 Then
			HeaderRowAddition = "BC1,BC2,BC3,BC4,BC5,BC6," & _
			  "SMBC1,SMBC2,SMBC3,SMBC4,"
		ElseIf PaperSheetNumber = 2 Then
			HeaderRowAddition = "BC1,BC2,BC3,BC4,BC5,BC6," & _
			  "SMBC1,SMBC2,SMBC3,SMBC4," & _
			  "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4,"
		ElseIf PaperSheetNumber = 3 Then
			HeaderRowAddition = "BC1,BC2,BC3,BC4,BC5,BC6," & _
			  "SMBC1,SMBC2,SMBC3,SMBC4," & _
			  "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4," & _
			  "SMBC_Pg3_Ln1,SMBC_Pg3_Ln2,SMBC_Pg3_Ln3,SMBC_Pg3_Ln4,"
		ElseIf PaperSheetNumber = 4 Then
			HeaderRowAddition = "BC1,BC2,BC3,BC4,BC5,BC6," & _
			  "SMBC1,SMBC2,SMBC3,SMBC4," & _
			  "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4," & _
			  "SMBC_Pg3_Ln1,SMBC_Pg3_Ln2,SMBC_Pg3_Ln3,SMBC_Pg3_Ln4," & _
			  "SMBC_Pg4_Ln1,SMBC_Pg4_Ln2,SMBC_Pg4_Ln3,SMBC_Pg4_Ln4,"
		Else
			MsgBox("The paper sheet count for the letter id that the script is using isn't populated.  Please contact a member of Systems Support", vbCritical, "Error")
			Return ""
		End If
		'add static current date if desired
		If AddStaticCurrentDate Then
			HeaderRowAddition = HeaderRowAddition & "StaticCurrentDate,"
		End If
		'remove quotes from header row and split out into array
		OriginalHeaderRow = Replace(OriginalHeaderRow, """", "")
		OHRFields = Split(OriginalHeaderRow, ",")
		'get total number of fields in record
		NumberOfFields = UBound(OHRFields)
		'find account number index
		While UCase(OHRFields(AcctNumIndex)) <> UCase(AcctNumFieldNm)
			AcctNumIndex = AcctNumIndex + 1
		End While
		'add new header row to revised file
		PrintLine(2, HeaderRowAddition & OriginalHeaderRow)
		'process all other records
		While Not EOF(1)
			LineI = LineI + 1
			'blank array
			ReDim Fields(NumberOfFields)
			'read record fields in one at a time
			For FieldI = 0 To NumberOfFields
				Input(1, Fields(FieldI))
			Next
			'return mail barcode
			RMBC = Split(EncNDM(Replace(Fields(AcctNumIndex), " ", "") & LetterID & String.Format("{0:MMddyyyy}", Now.Today.ToString)), vbCrLf)
			'add return mail data to string
			NewRowData = """" & RMBC(0) & """,""" & RMBC(1) & """,""" & RMBC(2) & """,""" & RMBC(3) & """,""" & RMBC(4) & """,""" & RMBC(5) & """"
			'state mail barcode
			PaperSheetI = 0
			While PaperSheetI <> PaperSheetNumber
				If PaperSheetI = 0 Then
					SMBC = Split(EncNDM("1" & CStr(PaperSheetNumber) & "0" & CStr(PaperSheetI + 1) & String.Format("{0:00000#}", LineI)), vbCrLf)
				Else
					SMBC = Split(EncNDM("0" & CStr(PaperSheetNumber) & "0" & CStr(PaperSheetI + 1) & String.Format("{0:00000#}", LineI)), vbCrLf)
				End If
				NewRowData = NewRowData & ",""" & SMBC(0) & """,""" & SMBC(1) & """,""" & SMBC(2) & """,""" & SMBC(3) & """"
				PaperSheetI = PaperSheetI + 1
			End While
			'add static current date if desired
			If AddStaticCurrentDate Then
				NewRowData = NewRowData & ",""" & String.Format("{0:mmmm dd, yyyy}", Now.Today.ToString) & """"
			End If
			'write the rest of the data out to row then write row out to file
			For FieldI = 0 To NumberOfFields
				NewRowData = NewRowData & ",""" & Fields(FieldI) & """"
			Next
			PrintLine(2, NewRowData)
		End While
		FileClose(1)
		FileClose(2)
		If NewFile Then
            AddBarcodeAndStaticCurrentDate = "T:\Add Return Mail Barcode Temp " & LetterID & ".txt"
		Else
			'delete original data file
			Kill(FileToProc)
			'copy new data file to original file location
            File.Move("T:\Add Return Mail Barcode Temp " & LetterID & ".txt", FileToProc)
			AddBarcodeAndStaticCurrentDate = FileToProc
		End If
	End Function


	Private Function EncNDM(ByVal DataToEncode As String, Optional ByVal ProcTilde As Integer = 0, Optional ByVal EncMode As Integer = 0, Optional ByVal PrefFormat As Integer = 0) As String
		'Format the data to the Data Matrix Font by calling the ActiveX DLL:
		Dim DMFontEncoder As DMATRIXLib.Datamatrix
		DMFontEncoder = New Datamatrix
		DMFontEncoder.FontEncode(DataToEncode, ProcTilde, EncMode, PrefFormat, EncNDM)
	End Function

#Region " General Stuff "

	'this function will transmit a key for you
	Public Overloads Function Hit(ByVal key As String, ByVal keyset As String)
		Try
			key = UCase(key)
			If Check4Text(23, 23, keyset) Then
				RefSession.TransmitTerminalKey(rcIBMPf2Key)
				RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
			End If
			Select Case key
				Case "F1"
					RefSession.TransmitTerminalKey(rcIBMPf1Key)
				Case "F2"
					RefSession.TransmitTerminalKey(rcIBMPf2Key)
				Case "F3"
					RefSession.TransmitTerminalKey(rcIBMPf3Key)
				Case "F4"
					RefSession.TransmitTerminalKey(rcIBMPf4Key)
				Case "F5"
					RefSession.TransmitTerminalKey(rcIBMPf5Key)
				Case "F6"
					RefSession.TransmitTerminalKey(rcIBMPf6Key)
				Case "F7"
					RefSession.TransmitTerminalKey(rcIBMPf7Key)
				Case "F8"
					RefSession.TransmitTerminalKey(rcIBMPf8Key)
				Case "F9"
					RefSession.TransmitTerminalKey(rcIBMPf9Key)
				Case "F10"
					RefSession.TransmitTerminalKey(rcIBMPf10Key)
				Case "F11"
					RefSession.TransmitTerminalKey(rcIBMPf11Key)
				Case "F12"
					RefSession.TransmitTerminalKey(rcIBMPf12Key)
				Case "ENTER"
					RefSession.TransmitTerminalKey(rcIBMEnterKey)
				Case "CLEAR"
					RefSession.TransmitTerminalKey(rcIBMClearKey)
				Case "END"
					RefSession.TransmitTerminalKey(rcIBMEraseEOFKey)
				Case "UP"
					RefSession.TransmitTerminalKey(rcIBMPA1Key)
				Case "TAB"
					RefSession.TransmitTerminalKey(rcIBMTabKey)
				Case "HOME"
					RefSession.TransmitTerminalKey(rcIBMHomeKey)
				Case "INS"
					RefSession.TransmitTerminalKey(rcIBMInsertKey)
				Case Else
					MsgBox("There has been a key code error.  Please contact a programmer.", MsgBoxStyle.Critical)
			End Select
			RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
		Catch ex As Exception
			MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
		End Try
	End Function

	'this function will transmit a key for you
	Public Overloads Function Hit(ByVal key As String)
		Try
			key = UCase(key)
			Select Case key
				Case "F1"
					RefSession.TransmitTerminalKey(rcIBMPf1Key)
				Case "F2"
					RefSession.TransmitTerminalKey(rcIBMPf2Key)
				Case "F3"
					RefSession.TransmitTerminalKey(rcIBMPf3Key)
				Case "F4"
					RefSession.TransmitTerminalKey(rcIBMPf4Key)
				Case "F5"
					RefSession.TransmitTerminalKey(rcIBMPf5Key)
				Case "F6"
					RefSession.TransmitTerminalKey(rcIBMPf6Key)
				Case "F7"
					RefSession.TransmitTerminalKey(rcIBMPf7Key)
				Case "F8"
					RefSession.TransmitTerminalKey(rcIBMPf8Key)
				Case "F9"
					RefSession.TransmitTerminalKey(rcIBMPf9Key)
				Case "F10"
					RefSession.TransmitTerminalKey(rcIBMPf10Key)
				Case "F11"
					RefSession.TransmitTerminalKey(rcIBMPf11Key)
				Case "F12"
					RefSession.TransmitTerminalKey(rcIBMPf12Key)
				Case "ENTER"
					RefSession.TransmitTerminalKey(rcIBMEnterKey)
				Case "CLEAR"
					RefSession.TransmitTerminalKey(rcIBMClearKey)
				Case "END"
					RefSession.TransmitTerminalKey(rcIBMEraseEOFKey)
				Case "UP"
					RefSession.TransmitTerminalKey(rcIBMPA1Key)
				Case "TAB"
					RefSession.TransmitTerminalKey(rcIBMTabKey)
				Case "HOME"
					RefSession.TransmitTerminalKey(rcIBMHomeKey)
				Case "INS"
					RefSession.TransmitTerminalKey(rcIBMInsertKey)
				Case Else
					MsgBox("There has been a key code error.  Please contact a programmer.", MsgBoxStyle.Critical)
			End Select
			RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
		Catch ex As Exception
			MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
		End Try
	End Function

	'Enters information into the Fast Path.
	Public Function FastPath(ByVal inp As String)
		Try
			RefSession.TransmitTerminalKey(rcIBMClearKey)
			RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
			RefSession.TransmitANSI(inp)
			RefSession.TransmitTerminalKey(rcIBMEnterKey)
			RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
		Catch ex As Exception
			MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
		End Try
	End Function

	'Enters inp into the given X,Y coordinates
	Public Function PutText(ByVal y As Integer, ByVal x As Integer, ByVal inp As String, Optional ByVal Enter As Boolean = False)
		Try
			RefSession.MoveCursor(y, x)
			RefSession.TransmitANSI(inp)
			'if enter = true then hit enter.
			If (Enter) Then
				RefSession.TransmitTerminalKey(rcIBMEnterKey)
				RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
			End If
		Catch ex As Exception
			MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
		End Try
	End Function

	'Checks for specific text at a certain location on the screen
	Public Function Check4Text(ByVal y As Integer, ByVal x As Integer, ByVal i As String) As Boolean
		Try
			If (RefSession.GetDisplayText(y, x, Len(i)) = i) Then
				Check4Text = True
			Else
				Check4Text = False
			End If
		Catch ex As Exception
			MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
		End Try
	End Function

	Public Function GetText(ByVal row As Integer, ByVal Column As Integer, ByVal Length As Integer) As String
		Return Trim(RefSession.GetDisplayText(row, Column, Length))
	End Function

	Function PauseForInsert()
		RefSession.WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
		Hit("INS")
	End Function

#End Region

#Region "CostCenterPrinting"
	'The CostCenterPrinting subroutine is the same as SP.CostCenterPrinting.Main()
	Public Function CostCenterPrinting(ByVal DocPath As String, ByVal Doc As String, ByVal DocDescription As String, ByVal PagesPerDoc As DestinationOrPageCount, ByVal DocID As String, ByVal Dat As String, ByVal TimeStp As String, ByVal ScriptID As String, ByVal inTest As Boolean, Optional ByVal Recover As Boolean = False) As Boolean
		Dim FileArray1(,) As String		  'list of data files
		Dim CoverArray() As String		  'list of cover sheet files
		Dim Fname() As String
		Dim x As Integer
		Dim CCC As String
		Dim InMasterFile As Boolean		  ' this is true if the comment is already found in the master file
		Dim CoverComment As String
		Dim CCCount As Integer
		Dim LogFile As String
		Dim PagesPerDocStr As String
		If inTest Then
			LogFile = "X:\PADD\Logs\Test\CCP" & ScriptID & ".txt"
		Else
			LogFile = "X:\PADD\Logs\CCP" & ScriptID & ".txt"
		End If
		'create comment for variable page letters
		'when PagesPerDocStr = "" no record will be added to the Master CC File
		PagesPerDocStr = PagesPerDoc
		If PagesPerDoc = DestinationOrPageCount.BusinessUnit Then
			CoverComment = "Deliver mail to business unit for processing"
			PagesPerDocStr = ""
		ElseIf PagesPerDoc = DestinationOrPageCount.DocServices Then
			CoverComment = "Special Handling Required - Document Services"
			PagesPerDocStr = ""
		End If

		FileArray1 = CCPSplit(Dat)
		If Dir(LogFile) = "" Then
			Dim LogWriter As StreamWriter
			'Loop until we get access to the file.
			Do While LogWriter Is Nothing
				Try
					LogWriter = New StreamWriter(LogFile)
				Catch e As Exception
					'Can't open the file yet. Eat the exception and try again.
				End Try
			Loop
			LogWriter.WriteLine(TimeStp)
			LogWriter.Close()
		End If
		If Recover = False Then
			'this is NOT in recovery mode, write over the old log file.
			Dim LogWriter As StreamWriter
			'Loop until we get access to the file.
			Do While LogWriter Is Nothing
				Try
					LogWriter = New StreamWriter(LogFile)
				Catch e As Exception
					'Can't open the file yet. Eat the exception and try again.
				End Try
			Loop
			LogWriter.WriteLine(TimeStp)
			LogWriter.Close()
		End If
		'create cover sheets
		Dim CoverWriter As StreamWriter
		For x = 1 To UBound(FileArray1, 2)
			Fname = Split(FileArray1(2, x), "\")
			CCC = Mid(Fname(UBound(Fname)), Len(Fname(UBound(Fname))) - 9, 6)
            ReDim Preserve CoverArray(x)
            CoverArray(x) = "T:\" & Mid(Fname(UBound(Fname)), 1, Len(Fname(UBound(Fname))) - 4) & "Cover.txt"
			CoverWriter = New StreamWriter(CoverArray(x))
			CoverWriter.WriteLine("BU,Description,NumPages,Cost,Standard,Foreign,CoverComment")
			CoverWriter.WriteLine(GetBU(CCC) & "," & DocDescription & "," & PagesPerDocStr & "," & CCC & "," & FileArray1(1, x) & "," & FileArray1(0, x) & "," & CoverComment)
			CoverWriter.Close()
			InMasterFile = False
			'print cover sheet and corelating data file
            If Recover = False Then
                PrintDoc("Scripted State Mail Cover Sheet", "X:\PADD\General\", inTest, CoverArray(x))
                If Dir(CoverArray(x)) <> "" Then
                    Kill(CoverArray(x))                   'delete cover sheet data file
                End If
                'print letters for this cover sheet.
                PrintDoc(Doc, DocPath, inTest, FileArray1(2, x))
            Else
                'recovery = true
                MsgBox("Recovery = true")
                Dim LogReader As New StreamReader(LogFile)
                TimeStp = LogReader.ReadLine()
                LogReader.Close()
                CCCount = CInt(FileArray1(0, x))
                If CCCount = 0 Then
                    CCCount = CInt(FileArray1(1, x))
                End If
                If FindInMasterCCFile(Now.ToShortDateString, DocID, CStr(CCCount), CCC, FileArray1(2, x), TimeStp, inTest) = False Then
                    PrintDoc("Scripted State Mail Cover Sheet", "X:\PADD\General\", inTest, CoverArray(x))
                    PrintDoc(Doc, DocPath, inTest, FileArray1(2, x))
                Else
                    'comment exists in master file already, documents already printed printing.
                    InMasterFile = True
                End If
            End If
			If Dir(CoverArray(x)) <> "" Then
				Kill(CoverArray(x))				'delete cover sheet data file
			End If
			If Dir(FileArray1(2, x)) <> "" Then
				Kill(FileArray1(2, x))				'delete data file
			End If

			'Add record to the Master Cost Center File if (your not in recovery, or if you are in recovery and the file is not found in the Master CC File) and also this is not a -1 or -2 PagesPerDoc
			Log("Checking whether to update the Master Cost Center file")
			If (Recover = False Or (Recover = True And InMasterFile = False)) And PagesPerDocStr <> "" Then
				Dim MasterCostCenterFileWriter As StreamWriter
				If inTest Then
					If Dir("X:\PADD\General\Test\MasterCostCenterFile.txt") = "" Then
						Log("Writing the header to the Master Cost Center file")
                        MasterCostCenterFileWriter = New StreamWriter("X:\PADD\General\Test\MasterCostCenterFile.txt")
						MasterCostCenterFileWriter.WriteLine("Date,LetterID,Foreign,Count,CostCenterCode,File,TimeStamp")
						MasterCostCenterFileWriter.Close()
					End If
					'loop until file access is granted
					Do While MasterCostCenterFileWriter Is Nothing
						Log("Opening the Master Cost Center file to write data")
						Try
                            MasterCostCenterFileWriter = New StreamWriter("X:\PADD\General\Test\MasterCostCenterFile.txt", True)
						Catch e As Exception
							'Can't open the file yet. Eat the exception and try again.
							Log("Couldn't open the Master Cost Center file. Will try again." & vbLf & "Reported problem: " & e.ToString)
						End Try
					Loop
				Else				'Not in test
					If Dir("X:\PADD\General\MasterCostCenterFile.txt") = "" Then
						MasterCostCenterFileWriter = New StreamWriter("X:\PADD\General\MasterCostCenterFile.txt")
						MasterCostCenterFileWriter.WriteLine("Date,LetterID,Foreign,Count,CostCenterCode,File,TimeStamp")
						MasterCostCenterFileWriter.Close()
					End If
					'loop until file access is granted
					Do While MasterCostCenterFileWriter Is Nothing
						Try
                            MasterCostCenterFileWriter = New StreamWriter("X:\PADD\General\MasterCostCenterFile.txt", True)
						Catch e As Exception
							'Can't open the file yet. Eat the exception and try again.
						End Try
					Loop
				End If
				Log("Adding data to the Master Cost Center file")
				If CInt(FileArray1(0, x)) > 0 Then
                    MasterCostCenterFileWriter.WriteLine("""" + Now.ToShortDateString & """,""" & DocID & """,""" & "F" & """,""" & FileArray1(0, x) & """,""" & CCC & """,""" & FileArray1(2, x) & """,""" & TimeStp & """")
				End If
				If CInt(FileArray1(1, x)) > 0 Then
                    MasterCostCenterFileWriter.WriteLine("""" + Now.ToShortDateString & """,""" & DocID & """,""" & "" & """,""" & FileArray1(1, x) & """,""" & CCC & """,""" & FileArray1(2, x) & """,""" & TimeStp & """")
				End If
				MasterCostCenterFileWriter.Close()
			End If
		Next x
	End Function

	Function FindInMasterCCFile(ByVal dt As String, ByVal LtrID As String, ByVal Cnt As String, ByVal CCC As String, ByVal Fl As String, ByVal TS As String, ByVal inTest As Boolean) As Boolean
		'accepts info to search for in Master cost center file, returns true if file is found
		Dim l As String		  'line from file
		Dim LArray() As String		  'line split up
		Dim MasterCostCenterFileReader As StreamReader
		'loop until file access is granted
		Do
			Try
				If inTest Then
					MasterCostCenterFileReader = New StreamReader("X:\PADD\General\Test\MasterCostCenterFile.txt")
				Else
					MasterCostCenterFileReader = New StreamReader("X:\PADD\General\MasterCostCenterFile.txt")
				End If
			Catch e As Exception
				'Can't open the file yet. Eat the exception and try again.
			End Try
		Loop

		l = MasterCostCenterFileReader.ReadLine()
		Do While l <> Nothing
			LArray = Split(Replace(l, """", ""), ",")
			If dt = LArray(0) And _
			LtrID = LArray(1) And _
			Cnt = LArray(3) And _
			CCC = LArray(4) And _
			Fl = LArray(5) And _
			TS = LArray(6) Then
				MasterCostCenterFileReader.Close()
				Return True
			End If
			l = MasterCostCenterFileReader.ReadLine()
		Loop
		MasterCostCenterFileReader.Close()
		Return False
	End Function

	Function GetBU(ByVal CCC As String) As String
		Dim BUCCC(13) As BusinessUnitStruct
		BUCCC(1).BusinessUnit = "LGP Administration"
		BUCCC(1).CostCenter = "MA2331"
		BUCCC(2).BusinessUnit = "LGP Guarantor Operations"
		BUCCC(2).CostCenter = "MA2330"
		BUCCC(3).BusinessUnit = "LGP Policy & Development"
		BUCCC(3).CostCenter = "MA2325"
		BUCCC(4).BusinessUnit = "LGP Postclaims"
		BUCCC(4).CostCenter = "MA2329"
		BUCCC(5).BusinessUnit = "LGP Special Default Prevention"
		BUCCC(5).CostCenter = "MA2328"
		BUCCC(6).BusinessUnit = "LPP Administration"
		BUCCC(6).CostCenter = "MA2326"
		BUCCC(7).BusinessUnit = "LPP Customer Service"
		BUCCC(7).CostCenter = "MA3817"
		BUCCC(8).BusinessUnit = "LPP Lender Services"
		BUCCC(8).CostCenter = "MA2327"
		BUCCC(9).BusinessUnit = "LPP Other Operations"
		BUCCC(9).CostCenter = "MA2322"
		BUCCC(10).BusinessUnit = "LPP Portfolio Servicing"
		BUCCC(10).CostCenter = "MA2324"
		BUCCC(11).BusinessUnit = "UESP Trust"
		BUCCC(11).CostCenter = "MA3440"
		BUCCC(12).BusinessUnit = "UHEAA Human Resources"
		BUCCC(12).CostCenter = "MA4061"
		BUCCC(13).BusinessUnit = "UHEAA Operational Accounting"
		BUCCC(13).CostCenter = "MA4119"

		Dim x As Integer
		For x = 1 To 13
			If BUCCC(x).CostCenter = CCC Then
				Return BUCCC(x).BusinessUnit
			End If
		Next x
	End Function

	Function CCPSplit(ByVal dataFile As String) As String(,)
		'This function returns an array containing foriegn count, non-foriegn count and file name for
		'each Cost Center Code contained in the dataFile inserted in to the function.
		Dim l As String
		Dim Fname() As String
		Dim LineArray() As String
		Dim CostCode As String
		Dim OldCostCode As String
		Dim StateStr As String
		Dim StateForeign As Boolean
		Dim FileArray(,) As String
		Dim File As String
		Dim a As Integer
		Dim b As Integer
		Dim Header As String
		Dim reader As StreamReader
		Dim writer As StreamWriter

		ReDim FileArray(2, 0)		  'column 0 = Foriegn Count, 1 = Non - Foriegn Count, 2 = file name
		Fname = Split(dataFile, "\")
		If IO.File.Exists(dataFile) = False Then
			MsgBox("File " & dataFile & " is missing. Contact Systems Support.")
			Return FileArray
		End If

		'Loop until files access is granted.
		Try
			reader = New StreamReader(dataFile)
		Catch e As Exception
			'Can't open the file yet. Eat the exception and try again.
		End Try

		l = reader.ReadLine
		Do While l <> Nothing
			a = InStrRev(l, ",")
			b = InStrRev(l, ",", a - 1)
			LineArray = Split(Mid(l, b + 1), ",")

			CostCode = Replace(LineArray(1), """", "")
			StateStr = Replace(LineArray(0), """", "")

			If CostCode <> "COST_CENTER_CODE" Then			 'skip header row
				If CostCode <> OldCostCode Then
					If Not IsNothing(writer) Then
						writer.Close()
					End If
                    File = "T:\" & Fname(UBound(Fname)) & CostCode & ".txt"
					ReDim Preserve FileArray(2, UBound(FileArray, 2) + 1)
					FileArray(2, UBound(FileArray, 2)) = File
					FileArray(0, UBound(FileArray, 2)) = 0					  'foriegn count
					FileArray(1, UBound(FileArray, 2)) = 0					  'non-foriegn count
					writer = New StreamWriter(File)
					writer.WriteLine(Header)
					writer.WriteLine(l)
				ElseIf CostCode = OldCostCode Then
					writer.WriteLine(l)
				End If
				If StateStr = "FC" Or Trim(StateStr) = "" Then
					FileArray(0, UBound(FileArray, 2)) = CStr(CInt(FileArray(0, UBound(FileArray, 2))) + 1)
				Else
					FileArray(1, UBound(FileArray, 2)) = CStr(CInt(FileArray(1, UBound(FileArray, 2))) + 1)
				End If
			Else
				Header = l
			End If
			OldCostCode = CostCode
			l = reader.ReadLine
		Loop
		writer.Close()
		reader.Close()
		Return FileArray
	End Function
#End Region

	'I really wish this were never necessary, but sometimes there's no better way to pinpoint a problem.
	Public Sub Log(ByVal logMessage As String)
        Dim LogFile As New StreamWriter("T:\ReflectionInterfaceLog.txt")
		LogFile.WriteLine(logMessage)
		LogFile.Close()
	End Sub
End Class
