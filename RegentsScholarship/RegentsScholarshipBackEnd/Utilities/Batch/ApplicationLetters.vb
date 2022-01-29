Imports System.IO
Imports Q

Public Class ApplicationLetters
    Private Shared ReadOnly Property LETTER_FOLDER() As String
        Get
            Dim regentsFolder As String = "X:\PADD\RegentsScholarship\"
            If (Constants.TEST_MODE) Then
                regentsFolder += "Test\"
            End If
            Return regentsFolder
        End Get
    End Property
    Private Shared ReadOnly Property SEND_TO_PRINTER() As Boolean
        Get
            Return (Not Constants.TEST_MODE)
        End Get
    End Property
    Private Const CCP_TIME_STAMP_FORMAT As String = "G" 'Month/day/year Hour:minute:second AM|PM
    Private Const COST_CENTER_CODE As String = "MA4311"
    Private Const SCRIPT_ID As String = "REGENTS"
    Private Shared SAVE_DOC_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "SaveDoc.txt"
    Private Shared LETTER_ERROR_REPORT As String = DataAccessBase.PersonalDataDirectory + "RegentsLetterErrors.txt"

    Public Enum DenialCategory
        CitizenshipOrCriminalRecord
        IncompleteApplication
        Final
        Other
    End Enum

    ''' <summary>
    ''' Entry point for printing all letters that use duplex printing.
    ''' </summary>
    Public Shared Sub PrintDuplexLetters()
        PrintConditionalApproval()
        PrintDuplexApprovals()
        'Clean up after ourselves.
        File.Delete(SAVE_DOC_DATA_FILE)

        'Notify the user if an error report was generated.
        If File.Exists(LETTER_ERROR_REPORT) Then
            Dim message As String = String.Format("Some students have an invalid address and could not have a letter generated. See the {0} file for details.", LETTER_ERROR_REPORT)
            System.Windows.Forms.MessageBox.Show(message, "Invalid Addresses", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
        End If
    End Sub

    ''' <summary>
    ''' Entry point for printing all letters that use simplex printing.
    ''' </summary>
    Public Shared Sub PrintSimplexLetters()
        'Delete the letter error report if it exists.
        If File.Exists(LETTER_ERROR_REPORT) Then
            File.Delete(LETTER_ERROR_REPORT)
        End If

        PrintDenialForCitizenshipOrCriminalRecord()
        PrintDenialForOtherReasons()
        PrintFinalDenial()
        PrintSimplexApprovals()
        'Clean up after ourselves.
        File.Delete(SAVE_DOC_DATA_FILE)
    End Sub

    Private Shared Sub ProcessStudentRecord(ByVal currentStudent As Student, ByVal dataFile As String, ByVal headerFields() As String, ByVal dataFields() As String, ByVal letterId As String, ByVal saveName As String, ByVal communicationSubject As String, ByVal communicationText As String)
        'Check that the address is marked valid. If not, write out to an error file and skip the letter stuff.
        If Not currentStudent.ContactInfo.HomeAddress.IsValid Then
            If Not File.Exists(LETTER_ERROR_REPORT) Then
                Using reportWriter As New StreamWriter(LETTER_ERROR_REPORT, False)
                    reportWriter.WriteLine("The following students were found to have an invalid address, so letters were not sent:")
                    reportWriter.WriteLine("NAME, STUDENT_ID, LETTER_ID")
                    reportWriter.Close()
                End Using
            End If
            Using reportWriter As New StreamWriter(LETTER_ERROR_REPORT, True)
                reportWriter.WriteLine(String.Format("{0} {1}, {2}, {3}", currentStudent.FirstName, currentStudent.LastName, currentStudent.StateStudentId, letterId))
                reportWriter.Close()
            End Using
            Return
        End If

        'Append to the Cost Center Printing data file.
        Using fileWriter As New StreamWriter(dataFile, True)
            fileWriter.WriteCommaDelimitedLine(dataFields)
            fileWriter.Close()
        End Using
        'Write an individual data file for the SaveDoc subroutine.
        Using saveDocWriter As New StreamWriter(SAVE_DOC_DATA_FILE, False)
            saveDocWriter.WriteCommaDelimitedLine(headerFields)
            saveDocWriter.WriteCommaDelimitedLine(dataFields)
            saveDocWriter.Close()
        End Using
        'Merge and save the letter to the student's folder.
        Dim dataFileWithBarCodes As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(SAVE_DOC_DATA_FILE, "BarcodeAccountNumber", letterId, False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
        Dim studentDirectory As String = String.Format("{0}Student_{1}\Communication\", Constants.STUDENT_DOCUMENT_ROOT, currentStudent.StateStudentId)
        If (Not Directory.Exists(studentDirectory)) Then
            Directory.CreateDirectory(studentDirectory)
        End If
        DocumentHandling.SaveDocs(LETTER_FOLDER, letterId, dataFileWithBarCodes, studentDirectory + saveName)
        'Add a communication record.
        Dim communicationRecord As New Communication()
        communicationRecord.EntityID = currentStudent.StateStudentId
        communicationRecord.EntityType = Constants.CommunicationEntityType.STUDENT
        communicationRecord.UserId = Constants.SYSTEM_USER_ID
        communicationRecord.Type = Constants.CommunicationType.LETTER
        communicationRecord.Source = Constants.CommunicationSource.USHE_STAFF
        communicationRecord.Subject = communicationSubject
        communicationRecord.Text = communicationText
        communicationRecord.TimeStamp = DateTime.Now
        communicationRecord.Is411 = False
        DataAccess.SetCommunication(communicationRecord)
        'Update the Award Status Letter Sent Date.
        currentStudent.ScholarshipApplication.BaseAward.StatusLetterSentDate = Date.Now
        currentStudent.Commit(Constants.SYSTEM_USER_ID, Student.Component.Application)
    End Sub

#Region "Duplex"
    Private Shared Sub PrintConditionalApproval()
        Dim DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSCHCOAP.txt"
        Dim HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "CostCenter"}

        'The ScholarshipApplication table can help us whittle the field down by two criteria.
        Dim conditionalStudentIds As List(Of String) = DataAccess.GetConditionallyApprovedStudentIds()
        If conditionalStudentIds.Count = 0 Then Return

        'Set up a progress form.
        Dim progressBar As New StatusBar(conditionalStudentIds.Count + 1, "Conditional Approval Letters")
        progressBar.updateStatBar("-")
        progressBar.Show()

        'Write out a header row.
        Using fileWriter As New StreamWriter(DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(HEADER_FIELDS)
            fileWriter.Close()
        End Using

        'Loop through the IDs and append to the appropriate data file.
        For Each studentId As String In conditionalStudentIds
            progressBar.updateStatBar(studentId)
            Dim conditionalStudent As Student = Student.Load(studentId)

            'The Barcode2D function needs a 10-character account number, so create
            'a separate field for that and use the student ID padded with zeros.
            Dim barcodeAccountNumber As String = studentId
            Do While barcodeAccountNumber.Length < 10
                barcodeAccountNumber = "0" + barcodeAccountNumber
            Loop
            'Define the data record.
            Dim dataFields As String() = _
            { _
                conditionalStudent.FirstName, conditionalStudent.LastName, _
                conditionalStudent.ContactInfo.HomeAddress.Line1, conditionalStudent.ContactInfo.HomeAddress.Line2, _
                conditionalStudent.ContactInfo.HomeAddress.City, conditionalStudent.ContactInfo.HomeAddress.State, conditionalStudent.ContactInfo.HomeAddress.ZipCode, conditionalStudent.ContactInfo.HomeAddress.Country, _
                conditionalStudent.StateStudentId, barcodeAccountNumber, COST_CENTER_CODE _
            }
            'Add the record to the CCP file, save a copy of the document, and create a comment.
            Dim commentSubject As String = "Conditional Approval Letter"
            Dim commentText As String = "Conditional Approval Letter sent to student."
            ProcessStudentRecord(conditionalStudent, DATA_FILE, HEADER_FIELDS, dataFields, "REGSCHCOAP", "ConditionalApprovalLetter.doc", commentSubject, commentText)
        Next

        'Add bar codes to the CCP file and send it to Cost Center Printing.
        If File.ReadAllLines(DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DATA_FILE, "BarcodeAccountNumber", "REGSCHCOAP", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Conditional Approval Letter", DocumentHandling.DestinationOrPageCount.Page2, "REGSCHCOAP", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DATA_FILE)
        progressBar.Close()
    End Sub

    Private Shared Sub PrintDuplexApprovals()
        Dim APPROVED_UESP_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGBEUESP.txt"
        Dim APPROVED_NO_UESP_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGBASEXEA.txt"
        Dim DEFERRED_BASE_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSBADFAP.txt"
        Dim DEFERRED_EXEMPLARY_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSBAEXDA.txt"
        Dim DEFERRED_UESP_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSAPBUDF.txt"
        Dim DEFERRED_EXEMPLARY_UESP_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGBAEXUDA.txt"
        Dim APPROVED_UESP_HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "BaseAwardAmount", "ExemplaryAwardAmount", "UespAwardAmount", "CostCenter"}
        Dim APPROVED_NO_UESP_HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "BaseAwardAmount", "ExemplaryAwardAmount", "CostCenter"}
        Dim DEFERRED_BASE_HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "BaseAwardAmount", "DefermentReason", "DefermentEndDate", "CostCenter"}
        Dim DEFERRED_UESP_HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "BaseAwardAmount", "UespAwardAmount", "DefermentReason", "DefermentEndDate", "CostCenter"}

        'Get a list of approved student IDs.
        Dim approvedStudentIds As List(Of String) = DataAccess.GetApprovalLetterStudentIds(False)
        If (approvedStudentIds.Count = 0) Then Return

        'Set up a progress form.
        Dim progressBar As New StatusBar(approvedStudentIds.Count + 1, "Duplex Approval Letters")
        progressBar.updateStatBar("-")
        progressBar.Show()

        'Write out header rows.
        Using fileWriter As New StreamWriter(APPROVED_UESP_DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(APPROVED_UESP_HEADER_FIELDS)
            fileWriter.Close()
        End Using
        Using fileWriter As New StreamWriter(APPROVED_NO_UESP_DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(APPROVED_NO_UESP_HEADER_FIELDS)
            fileWriter.Close()
        End Using
        Using fileWriter As New StreamWriter(DEFERRED_BASE_DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(DEFERRED_BASE_HEADER_FIELDS)
            fileWriter.Close()
        End Using
        Using fileWriter As New StreamWriter(DEFERRED_EXEMPLARY_DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(DEFERRED_BASE_HEADER_FIELDS)
            fileWriter.Close()
        End Using
        Using fileWriter As New StreamWriter(DEFERRED_UESP_DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(DEFERRED_UESP_HEADER_FIELDS)
            fileWriter.Close()
        End Using
        Using fileWriter As New StreamWriter(DEFERRED_EXEMPLARY_UESP_DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(DEFERRED_UESP_HEADER_FIELDS)
            fileWriter.Close()
        End Using

        'Loop through the IDs and append to the appropriate data files.
        For Each studentId As String In approvedStudentIds
            progressBar.updateStatBar(studentId)
            Dim approvedStudent As Student = Student.Load(studentId)

            'The Barcode2D function needs a 10-character account number, so create
            'a separate field for that and use the student ID padded with zeros.
            Dim barcodeAccountNumber As String = studentId.PadLeft(10, "0")

            'Append to the appropriate data file, depending on award status.
            If (approvedStudent.ScholarshipApplication.BaseAward.Status = Constants.AwardStatus.APPROVED) Then
                If (approvedStudent.ScholarshipApplication.UespSupplementalAward.IsApproved) Then
                    Dim dataFields As String() = _
                    { _
                        approvedStudent.FirstName, approvedStudent.LastName, _
                        approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, _
                        approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, _
                        approvedStudent.StateStudentId, barcodeAccountNumber, _
                        approvedStudent.ScholarshipApplication.BaseAward.Amount.ToString(), approvedStudent.ScholarshipApplication.ExemplaryAward.Amount.ToString(), approvedStudent.ScholarshipApplication.UespSupplementalAward.Amount.ToString(), _
                        COST_CENTER_CODE _
                    }
                    'Add the record to the CCP file, save a copy of the document, and create a comment.
                    Dim commentSubject As String = "Approval Letter--Base, Exemplary & UESP"
                    Dim commentText As String = commentSubject + " sent to student."
                    ProcessStudentRecord(approvedStudent, APPROVED_UESP_DATA_FILE, APPROVED_UESP_HEADER_FIELDS, dataFields, "REGBEUESP", "ApprovalLetter.doc", commentSubject, commentText)
                Else
                    Dim dataFields As String() = _
                    { _
                        approvedStudent.FirstName, approvedStudent.LastName, _
                        approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, _
                        approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, _
                        approvedStudent.StateStudentId, barcodeAccountNumber, _
                        approvedStudent.ScholarshipApplication.BaseAward.Amount.ToString(), approvedStudent.ScholarshipApplication.ExemplaryAward.Amount.ToString(), _
                        COST_CENTER_CODE _
                    }
                    'Add the record to the CCP file, save a copy of the document, and create a comment.
                    Dim commentSubject As String = "Approval Letter--Base & Exemplary"
                    Dim commentText As String = commentSubject + " sent to student."
                    ProcessStudentRecord(approvedStudent, APPROVED_NO_UESP_DATA_FILE, APPROVED_NO_UESP_HEADER_FIELDS, dataFields, "REGBASEXEA", "ApprovalLetter.doc", commentSubject, commentText)
                End If
            ElseIf (approvedStudent.ScholarshipApplication.BaseAward.Status = Constants.AwardStatus.DEFERRED) Then
                If (approvedStudent.ScholarshipApplication.ExemplaryAward.IsApproved AndAlso approvedStudent.ScholarshipApplication.UespSupplementalAward.IsApproved) Then
                    Dim dataFields As String() = _
                    { _
                        approvedStudent.FirstName, approvedStudent.LastName, _
                        approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, _
                        approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, _
                        approvedStudent.StateStudentId, barcodeAccountNumber, _
                        approvedStudent.ScholarshipApplication.BaseAward.Amount.ToString(), approvedStudent.ScholarshipApplication.UespSupplementalAward.Amount.ToString(), _
                        approvedStudent.ScholarshipApplication.Deferment.Reason, approvedStudent.ScholarshipApplication.Deferment.EndDate.Value.ToString("MMMM dd, yyyy"), _
                        COST_CENTER_CODE _
                    }
                    'Add the record to the CCP file, save a copy of the document, and create a comment.
                    Dim commentSubject As String = "Approval/Deferment Approval Letter--Base, Exemplary & UESP"
                    Dim commentText As String = commentSubject + " sent to student."
                    ProcessStudentRecord(approvedStudent, DEFERRED_EXEMPLARY_UESP_DATA_FILE, DEFERRED_UESP_HEADER_FIELDS, dataFields, "REGBAEXUDA", "ApprovalLetter.doc", commentSubject, commentText)
                ElseIf (approvedStudent.ScholarshipApplication.ExemplaryAward.IsApproved) Then
                    Dim dataFields As String() = _
                    { _
                        approvedStudent.FirstName, approvedStudent.LastName, _
                        approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, _
                        approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, _
                        approvedStudent.StateStudentId, barcodeAccountNumber, _
                        approvedStudent.ScholarshipApplication.BaseAward.Amount.ToString(), _
                        approvedStudent.ScholarshipApplication.Deferment.Reason, approvedStudent.ScholarshipApplication.Deferment.EndDate.Value.ToString("MMMM dd, yyyy"), _
                        COST_CENTER_CODE _
                    }
                    'Add the record to the CCP file, save a copy of the document, and create a comment.
                    Dim commentSubject As String = "Approval/Deferment Approval Letter--Base & Exemplary"
                    Dim commentText As String = commentSubject + " sent to student."
                    ProcessStudentRecord(approvedStudent, DEFERRED_EXEMPLARY_DATA_FILE, DEFERRED_BASE_HEADER_FIELDS, dataFields, "REGSBAEXDA", "ApprovalLetter.doc", commentSubject, commentText)
                ElseIf (approvedStudent.ScholarshipApplication.UespSupplementalAward.IsApproved) Then
                    Dim dataFields As String() = _
                    { _
                        approvedStudent.FirstName, approvedStudent.LastName, _
                        approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, _
                        approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, _
                        approvedStudent.StateStudentId, barcodeAccountNumber, _
                        approvedStudent.ScholarshipApplication.BaseAward.Amount.ToString(), approvedStudent.ScholarshipApplication.UespSupplementalAward.Amount.ToString(), _
                        approvedStudent.ScholarshipApplication.Deferment.Reason, approvedStudent.ScholarshipApplication.Deferment.EndDate.Value.ToString("MMMM dd, yyyy"), _
                        COST_CENTER_CODE _
                    }
                    'Add the record to the CCP file, save a copy of the document, and create a comment.
                    Dim commentSubject As String = "Approval/Deferment Approval Letter--Base & UESP"
                    Dim commentText As String = commentSubject + " sent to student."
                    ProcessStudentRecord(approvedStudent, DEFERRED_UESP_DATA_FILE, DEFERRED_UESP_HEADER_FIELDS, dataFields, "REGSAPBUDF", "ApprovalLetter.doc", commentSubject, commentText)
                Else
                    Dim dataFields As String() = _
                    { _
                        approvedStudent.FirstName, approvedStudent.LastName, _
                        approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, _
                        approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, _
                        approvedStudent.StateStudentId, barcodeAccountNumber, _
                        approvedStudent.ScholarshipApplication.BaseAward.Amount.ToString(), _
                        approvedStudent.ScholarshipApplication.Deferment.Reason, approvedStudent.ScholarshipApplication.Deferment.EndDate.Value.ToString("MMMM dd, yyyy"), _
                        COST_CENTER_CODE _
                    }
                    'Add the record to the CCP file, save a copy of the document, and create a comment.
                    Dim commentSubject As String = "Approval/Deferment Approval Letter--Base Only"
                    Dim commentText As String = commentSubject + " sent to student."
                    ProcessStudentRecord(approvedStudent, DEFERRED_BASE_DATA_FILE, DEFERRED_BASE_HEADER_FIELDS, dataFields, "REGSBADFAP", "ApprovalLetter.doc", commentSubject, commentText)
                End If
            End If
        Next studentId

        'Add bar codes to the CCP files and send them to Cost Center Printing.
        If File.ReadAllLines(APPROVED_UESP_DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(APPROVED_UESP_DATA_FILE, "BarcodeAccountNumber", "REGBEUESP", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Base, Exemplary and UESP Award Letter", DocumentHandling.DestinationOrPageCount.Page2, "REGBEUESP", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(APPROVED_UESP_DATA_FILE)
        If File.ReadAllLines(APPROVED_NO_UESP_DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(APPROVED_NO_UESP_DATA_FILE, "BarcodeAccountNumber", "REGBASEXEA", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Base & Exemplary Award Letter", DocumentHandling.DestinationOrPageCount.Page2, "REGBASEXEA", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(APPROVED_NO_UESP_DATA_FILE)
        If File.ReadAllLines(DEFERRED_BASE_DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DEFERRED_BASE_DATA_FILE, "BarcodeAccountNumber", "REGSBADFAP", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Base Award/Deferment Approval Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGSBADFAP", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DEFERRED_BASE_DATA_FILE)
        If File.ReadAllLines(DEFERRED_EXEMPLARY_DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DEFERRED_EXEMPLARY_DATA_FILE, "BarcodeAccountNumber", "REGSBAEXDA", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Base & Exemplary Award/Deferment Approval Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGSBAEXDA", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DEFERRED_EXEMPLARY_DATA_FILE)
        If File.ReadAllLines(DEFERRED_UESP_DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DEFERRED_UESP_DATA_FILE, "BarcodeAccountNumber", "REGSAPBUDF", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Base & UESP Award/Deferment Approval Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGSAPBUDF", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DEFERRED_UESP_DATA_FILE)
        If File.ReadAllLines(DEFERRED_EXEMPLARY_UESP_DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DEFERRED_EXEMPLARY_UESP_DATA_FILE, "BarcodeAccountNumber", "REGBAEXUDA", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Base, Exemplary and UESP Award Letter/Deferment Approval", DocumentHandling.DestinationOrPageCount.Page1, "REGBAEXUDA", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DEFERRED_EXEMPLARY_UESP_DATA_FILE)
        progressBar.Close()
    End Sub
#End Region 'Duplex

#Region "Simplex"
    Private Shared Sub ProcessDenialLetter(ByVal deniedStudent As Student, ByVal dataFile As String, ByVal headerFields() As String, ByVal isFinalDenialLetter As Boolean)
        'The Barcode2D function needs a 10-character account number, so create
        'a separate field for that and use the student ID padded with zeros.
        Dim barcodeAccountNumber As String = deniedStudent.StateStudentId
        Do While barcodeAccountNumber.Length < 10
            barcodeAccountNumber = "0" + barcodeAccountNumber
        Loop

        'Define the first part of the data record.
        Dim dataFields As List(Of String) = New String() {deniedStudent.FirstName, deniedStudent.LastName, _
            deniedStudent.ContactInfo.HomeAddress.Line1, deniedStudent.ContactInfo.HomeAddress.Line2, _
            deniedStudent.ContactInfo.HomeAddress.City, deniedStudent.ContactInfo.HomeAddress.State, deniedStudent.ContactInfo.HomeAddress.ZipCode, deniedStudent.ContactInfo.HomeAddress.Country, _
            deniedStudent.StateStudentId, barcodeAccountNumber _
        }.ToList()
        'Add the denial reaons.
        For reasonNumber As Integer = 0 To 5
            If (deniedStudent.ScholarshipApplication.DenialReasons.Count > reasonNumber) Then
                dataFields.Add(deniedStudent.ScholarshipApplication.DenialReasons(reasonNumber))
            Else
                dataFields.Add("")
            End If
        Next reasonNumber
        'Tack the cost center code onto the end.
        dataFields.Add(COST_CENTER_CODE)

        'Add the record to the CCP file, save a copy of the document, and create a comment.
        Dim commentSubject As String
        Dim commentText As String
        If isFinalDenialLetter Then
            commentSubject = "Final Denial Letter"
            commentText = "Final Denial Letter sent to student. Denial reasons -"
        Else
            commentSubject = "Denial Letter"
            commentText = "Denial Letter sent to student. Denial reasons -"
        End If
        For reasonNumber As Integer = 0 To 5
            If (deniedStudent.ScholarshipApplication.DenialReasons.Count > reasonNumber) Then
                commentText += String.Format(" {0}.", deniedStudent.ScholarshipApplication.DenialReasons(reasonNumber))
            Else
                Exit For
            End If
        Next reasonNumber
        Dim letterID As String
        If isFinalDenialLetter Then
            letterID = "REGFINDENY"
        Else
            letterID = "REGSCHDENY"
        End If
        ProcessStudentRecord(deniedStudent, dataFile, headerFields, dataFields.ToArray(), letterID, "DenialLetter.doc", commentSubject, commentText)
    End Sub

    Private Shared Sub PrintDenialForCitizenshipOrCriminalRecord()
        Dim DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSCHDENY.txt"
        Dim HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "DenialReason1", "DenialReason2", "DenialReason3", "DenialReason4", "DenialReason5", "DenialReason6", "CostCenter"}

        Dim deniedStudentIds As List(Of String) = DataAccess.GetDenialLetterStudentIds(DenialCategory.CitizenshipOrCriminalRecord)
        If deniedStudentIds.Count = 0 Then Return

        'Set up a progress form.
        Dim progressBar As New StatusBar(deniedStudentIds.Count + 1, "Denial Letters for citizenship or criminal record")
        progressBar.updateStatBar("-")
        progressBar.Show()

        'Write out a header row.
        Using fileWriter As New StreamWriter(DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(HEADER_FIELDS)
            fileWriter.Close()
        End Using

        'Loop through the IDs and append to the data file.
        For Each studentId As String In deniedStudentIds
            progressBar.updateStatBar(studentId)
            Dim deniedStudent As Student = Student.Load(studentId)
            ProcessDenialLetter(deniedStudent, DATA_FILE, HEADER_FIELDS, False)
        Next

        'Add bar codes to the CCP file and send it to Cost Center Printing.
        If File.ReadAllLines(DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DATA_FILE, "BarcodeAccountNumber", "REGSCHDENY", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Denial Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGSCHDENY", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), "REGSCHDENY", Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DATA_FILE)
        progressBar.Close()
    End Sub

    Public Shared Sub PrintDenialForIncompleteApplication()
        Dim DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSCHDENY.txt"
        Dim HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "DenialReason1", "DenialReason2", "DenialReason3", "DenialReason4", "DenialReason5", "DenialReason6", "CostCenter"}

        Dim incompleteApplicationIds As List(Of String) = DataAccess.GetDenialLetterStudentIds(DenialCategory.IncompleteApplication)
        If incompleteApplicationIds.Count = 0 Then Return

        'Set up a progress form.
        Dim progressBar As New StatusBar(incompleteApplicationIds.Count + 1, "Denial Letters for Incomplete Application")
        progressBar.updateStatBar("-")
        progressBar.Show()

        'Write out a header row.
        Using fileWriter As New StreamWriter(DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(HEADER_FIELDS)
            fileWriter.Close()
        End Using

        'Loop through the IDs and append to the data file.
        For Each studentId As String In incompleteApplicationIds
            progressBar.updateStatBar(studentId)
            Dim deniedStudent As Student = Student.Load(studentId)
            'The Barcode2D function needs a 10-character account number, so create
            'a separate field for that and use the student ID padded with zeros.
            Dim barcodeAccountNumber As String = deniedStudent.StateStudentId
            Do While barcodeAccountNumber.Length < 10
                barcodeAccountNumber = "0" + barcodeAccountNumber
            Loop
            'Define the first part of the data record.
            Dim dataFields As List(Of String) = New String() {deniedStudent.FirstName, deniedStudent.LastName, _
                deniedStudent.ContactInfo.HomeAddress.Line1, deniedStudent.ContactInfo.HomeAddress.Line2, _
                deniedStudent.ContactInfo.HomeAddress.City, deniedStudent.ContactInfo.HomeAddress.State, deniedStudent.ContactInfo.HomeAddress.ZipCode, deniedStudent.ContactInfo.HomeAddress.Country, _
                deniedStudent.StateStudentId, barcodeAccountNumber, "Incomplete file at deadline:" _
            }.ToList()
            'Add the denial reaons.
            For reasonNumber As Integer = 0 To 4
                If (deniedStudent.ScholarshipApplication.DenialReasons.Count > reasonNumber) Then
                    dataFields.Add(deniedStudent.ScholarshipApplication.DenialReasons(reasonNumber))
                Else
                    dataFields.Add("")
                End If
            Next reasonNumber
            'Tack the cost center code onto the end.
            dataFields.Add(COST_CENTER_CODE)
            'Add the record to the CCP file, save a copy of the document, and create a comment.
            Dim commentSubject As String = "Denial Letter"
            Dim commentText As String = "Denial Letter sent to student. Denial reason - Incomplete file at deadline."
            ProcessStudentRecord(deniedStudent, DATA_FILE, HEADER_FIELDS, dataFields.ToArray(), "REGSCHDENY", "DenialLetter.doc", commentSubject, commentText)
        Next

        'Add bar codes to the CCP file and send it to Cost Center Printing.
        If File.ReadAllLines(DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DATA_FILE, "BarcodeAccountNumber", "REGSCHDENY", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Denial Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGSCHDENY", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DATA_FILE)
        progressBar.Close()
    End Sub

    Private Shared Sub PrintDenialForOtherReasons()
        Dim DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSCHDENY.txt"
        Dim HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "DenialReason1", "DenialReason2", "DenialReason3", "DenialReason4", "DenialReason5", "DenialReason6", "CostCenter"}

        Dim deniedStudentIds As List(Of String) = DataAccess.GetDenialLetterStudentIds(DenialCategory.Other)
        If deniedStudentIds.Count = 0 Then Return

        'Set up a progress form.
        Dim progressBar As New StatusBar(deniedStudentIds.Count + 1, "Denial Letters for other reasons")
        progressBar.updateStatBar("-")
        progressBar.Show()

        'Write out a header row.
        Using fileWriter As New StreamWriter(DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(HEADER_FIELDS)
            fileWriter.Close()
        End Using

        'Loop through the IDs and append to the appropriate data file.
        For Each studentId As String In deniedStudentIds
            progressBar.updateStatBar(studentId)
            Dim deniedStudent As Student = Student.Load(studentId)
            ProcessDenialLetter(deniedStudent, DATA_FILE, HEADER_FIELDS, False)
        Next

        'Add bar codes to the CCP file and send it to Cost Center Printing.
        If File.ReadAllLines(DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DATA_FILE, "BarcodeAccountNumber", "REGSCHDENY", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Denial Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGSCHDENY", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DATA_FILE)
        progressBar.Close()
    End Sub

    Private Shared Sub PrintFinalDenial()
        Dim DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSCHDENY.txt"
        Dim HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "DenialReason1", "DenialReason2", "DenialReason3", "DenialReason4", "DenialReason5", "DenialReason6", "CostCenter"}

        Dim deniedStudentIds As List(Of String) = DataAccess.GetDenialLetterStudentIds(DenialCategory.Final)
        If deniedStudentIds.Count = 0 Then Return

        'Set up a progress form.
        Dim progressBar As New StatusBar(deniedStudentIds.Count + 1, "Final Denial Letters")
        progressBar.updateStatBar("-")
        progressBar.Show()

        'Write out a header row.
        Using fileWriter As New StreamWriter(DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(HEADER_FIELDS)
            fileWriter.Close()
        End Using

        'Loop through the IDs and append to the appropriate data file.
        For Each studentId As String In deniedStudentIds
            progressBar.updateStatBar(studentId)
            Dim deniedStudent As Student = Student.Load(studentId)
            ProcessDenialLetter(deniedStudent, DATA_FILE, HEADER_FIELDS, True)
        Next

        'Add bar codes to the CCP file and send it to Cost Center Printing.
        If File.ReadAllLines(DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(DATA_FILE, "BarcodeAccountNumber", "REGFINDENY", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Denial Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGFINDENY", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(DATA_FILE)
        progressBar.Close()
    End Sub

    Private Shared Sub PrintSimplexApprovals()
        Dim UESP_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGBAUESPA.txt"
        Dim NO_UESP_DATA_FILE As String = DataAccessBase.PersonalDataDirectory + "REGSCNWAWD.txt"
        Dim UESP_HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "BaseAwardAmount", "UespAwardAmount", "CostCenter"}
        Dim NO_UESP_HEADER_FIELDS As String() = {"FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "AccountNumber", "BarcodeAccountNumber", "BaseAwardAmount", "CostCenter"}

        'Get a list of approved student IDs.
        Dim approvedStudentIds As List(Of String) = DataAccess.GetApprovalLetterStudentIds(True)
        If (approvedStudentIds.Count = 0) Then Return

        'Set up a progress form.
        Dim progressBar As New StatusBar(approvedStudentIds.Count + 1, "Simplex Approval Letters")
        progressBar.updateStatBar("-")
        progressBar.Show()

        'Write out header rows.
        Using fileWriter As New StreamWriter(UESP_DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(UESP_HEADER_FIELDS)
            fileWriter.Close()
        End Using
        Using fileWriter As New StreamWriter(NO_UESP_DATA_FILE, False)
            fileWriter.WriteCommaDelimitedLine(NO_UESP_HEADER_FIELDS)
            fileWriter.Close()
        End Using

        'Loop through the IDs and append to the appropriate data files.
        For Each studentId As String In approvedStudentIds
            progressBar.updateStatBar(studentId)
            Dim approvedStudent As Student = Student.Load(studentId)
            'The Barcode2D function needs a 10-character account number, so create
            'a separate field for that and use the student ID padded with zeros.
            Dim barcodeAccountNumber As String = studentId
            Do While barcodeAccountNumber.Length < 10
                barcodeAccountNumber = "0" + barcodeAccountNumber
            Loop

            'Proceed according to whether the UESP award was earned.
            If (approvedStudent.ScholarshipApplication.UespSupplementalAward.IsApproved) Then
                Dim dataFields As String() = _
                { _
                    approvedStudent.FirstName, approvedStudent.LastName, _
                    approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, _
                    approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, _
                    approvedStudent.StateStudentId, barcodeAccountNumber, _
                    approvedStudent.ScholarshipApplication.BaseAward.Amount.ToString(), approvedStudent.ScholarshipApplication.UespSupplementalAward.Amount.ToString(), _
                    COST_CENTER_CODE _
                }
                'Add the record to the CCP file, save a copy of the document, and create a comment.
                Dim commentSubject As String = "Approval Letter--Base & UESP"
                Dim commentText As String = "Approval Letter--Base & UESP sent to student."
                ProcessStudentRecord(approvedStudent, UESP_DATA_FILE, UESP_HEADER_FIELDS, dataFields, "REGBAUESPA", "ApprovalLetter.doc", commentSubject, commentText)
            Else
                Dim dataFields As String() = _
                { _
                    approvedStudent.FirstName, approvedStudent.LastName, _
                    approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, _
                    approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, _
                    approvedStudent.StateStudentId, barcodeAccountNumber, _
                    approvedStudent.ScholarshipApplication.BaseAward.Amount.ToString(), _
                    COST_CENTER_CODE _
                }
                'Add the record to the CCP file, save a copy of the document, and create a comment.
                Dim commentSubject As String = "Approval Letter--Base Only"
                Dim commentText As String = "Approval Letter--Base Only sent to student."
                ProcessStudentRecord(approvedStudent, NO_UESP_DATA_FILE, NO_UESP_HEADER_FIELDS, dataFields, "REGSCNWAWD", "ApprovalLetter.doc", commentSubject, commentText)
            End If
        Next studentId

        'Add bar codes to the CCP files and send them to Cost Center Printing.
        If File.ReadAllLines(UESP_DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(UESP_DATA_FILE, "BarcodeAccountNumber", "REGBAUESPA", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Base & UESP Award Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGBAUESPA", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(UESP_DATA_FILE)
        If File.ReadAllLines(NO_UESP_DATA_FILE).Count() > 1 Then
            Dim dataFile As String = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(NO_UESP_DATA_FILE, "BarcodeAccountNumber", "REGSCNWAWD", False, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, Constants.TEST_MODE)
            DocumentHandling.CostCenterPrinting("Regents' Scholarship Base Award Letter", DocumentHandling.DestinationOrPageCount.Page1, "REGSCNWAWD", dataFile, "CostCenter", "State", Date.Now.ToString(CCP_TIME_STAMP_FORMAT), SCRIPT_ID, Constants.TEST_MODE, False, SEND_TO_PRINTER)
            File.Delete(dataFile)
        End If
        File.Delete(NO_UESP_DATA_FILE)
        progressBar.Close()
    End Sub
#End Region 'Simplex
End Class
