Imports System.IO
Imports Q

Public Class PaymentLetters
    Public Shared Sub PrintLetters(ByVal userId As String, ByVal batchNumber As String)
        Dim stateStudentIds As List(Of String) = DataAccess.GetStudentIdsForPaymentLetters(batchNumber)
        Dim dataFileName As String = String.Format("{0}rsdat.txt", Q.DataAccessBase.PersonalDataDirectory)

        For Each id As String In stateStudentIds
            'Load the student and create a data file of the payments from this batch.
            Dim approvedStudent As Student = Student.Load(id)
            Dim batchPayments As IEnumerable(Of Payment) = approvedStudent.Payments.Where(Function(p) p.BatchNumber = batchNumber)
            If (Not CreateDataFile(dataFileName, approvedStudent, batchPayments)) Then Continue For

            'Determine the correct letter to print based on the payment types.
            Dim paymentTypes As IEnumerable(Of String) = batchPayments.Select(Function(p) p.Type)
            Dim letterId As String
            If paymentTypes.Contains(Constants.PaymentType.BASE) Then
                'if student has base award payments
                If paymentTypes.Contains(Constants.PaymentType.EXEMPLARY) AndAlso paymentTypes.Contains(Constants.PaymentType.UESP) Then
                    'base, exemplary, and UESP - RSTRFBAEU
                    letterId = "RSTRFBAEU"
                ElseIf paymentTypes.Contains(Constants.PaymentType.EXEMPLARY) Then
                    'base and exemplary - RSTRFDBAE
                    letterId = "RSTRFDBAE"
                ElseIf paymentTypes.Contains(Constants.PaymentType.UESP) Then
                    'base and UESP - RSFTRFBU
                    letterId = "RSFTRFBU"
                Else
                    'base only - RSFTRFB
                    letterId = "RSFTRFB"
                End If
            Else
                'exemplary only - RSCRHRBAL
                letterId = "RSCRHRBAL"
            End If

            'Create a Demographics object for the centralized printing function call.
            Dim studentDemographics As New SystemBorrowerDemographics()
            studentDemographics.AccountNumber = approvedStudent.StateStudentId
            studentDemographics.FName = approvedStudent.FirstName
            studentDemographics.LName = approvedStudent.LastName
            studentDemographics.Addr1 = approvedStudent.ContactInfo.HomeAddress.Line1
            studentDemographics.Addr2 = approvedStudent.ContactInfo.HomeAddress.Line2
            studentDemographics.City = approvedStudent.ContactInfo.HomeAddress.City
            studentDemographics.State = approvedStudent.ContactInfo.HomeAddress.State
            studentDemographics.Zip = approvedStudent.ContactInfo.HomeAddress.ZipCode
            studentDemographics.Country = approvedStudent.ContactInfo.HomeAddress.Country
            studentDemographics.EmailValidityIndicator = "Y"

            'Set the student's document directory and the file name for this letter.
            Dim fullPath As String = String.Format("{0}Student_{1}\Payments\", Constants.STUDENT_DOCUMENT_ROOT, approvedStudent.StateStudentId)
            If (Not Directory.Exists(fullPath)) Then Directory.CreateDirectory(fullPath)
            Dim lastSemesterPayment As Payment = GetLastSemesterPayment(batchPayments)
            Dim savedDocumentName As String = String.Format("{0}{1}_{2}_{3}_{4:0000}.doc", fullPath, letterId, lastSemesterPayment.College.Replace(" ", "_"), lastSemesterPayment.Semester, lastSemesterPayment.Year)

            'Have centralized printing queue the letter and save a copy to the student's document directory.
            If (File.Exists(savedDocumentName)) Then File.Delete(savedDocumentName)
            Q.DocumentHandling.GiveMeItAll_Without_ReflectionInterface(studentDemographics, letterId, dataFileName, "AccountNumber", "State", Q.DocumentHandling.CentralizedPrintingDeploymentMethod.dmLetter, DocumentHandling.Barcode2DLetterRecipient.lrOther, Constants.TEST_MODE, savedDocumentName)

            'Update the student's award status if eligibility is completed.
            Dim isFinalPayment As Boolean = False
            If (paymentTypes.Contains(Constants.PaymentType.EXEMPLARY)) Then
                'Check whether credit hours payable are exhausted.
                If (approvedStudent.CumulativeCreditHoursPaid() >= Constants.MAX_CREDIT_HOURS_PAYABLE) Then isFinalPayment = True

                'Check whether semesters payable are exhausted.
                Dim distinctSemesters As New List(Of Payment.SemesterYear)()
                For Each semesterYear As Payment.SemesterYear In approvedStudent.Payments.Select(Function(p) New Payment.SemesterYear(p.Semester, p.Year))
                    Dim sy As Payment.SemesterYear = semesterYear
                    If (distinctSemesters.Where(Function(p) p.Semester = sy.Semester AndAlso p.Year = sy.Year).Count() = 0) Then
                        distinctSemesters.Add(sy)
                    End If
                Next semesterYear
                If (distinctSemesters.Count() >= Constants.MAX_SEMESTERS_PAYABLE) Then isFinalPayment = True
            Else
                isFinalPayment = True
            End If
            If (isFinalPayment) Then
                approvedStudent.ScholarshipApplication.BaseAward.Status = Constants.AwardStatus.ELIGIBILITY_COMPLETED
                approvedStudent.ScholarshipApplication.BaseAward.StatusDate = DateTime.Now
                approvedStudent.Commit(userId, Student.Component.Application)
            End If
        Next id
    End Sub

    Private Shared Function CreateDataFile(ByVal dataFileName As String, ByVal approvedStudent As Student, ByVal batchPayments As IEnumerable(Of Payment)) As Boolean
        Dim lastSemesterPayment As Payment = GetLastSemesterPayment(batchPayments)
        If (lastSemesterPayment Is Nothing) Then Return False
        Dim term As String = String.Format("{0} {1}", lastSemesterPayment.Semester, lastSemesterPayment.Year)
        Dim institution As String = lastSemesterPayment.College
        Dim hours As Double = lastSemesterPayment.Credits
        Dim totalPaymentAmount As String = batchPayments.Sum(Function(p) p.Amount).ToString("C")

        Using dataWriter As New StreamWriter(dataFileName, False)
            dataWriter.WriteCommaDelimitedLine("AccountNumber", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "Term", "Institution_Attending", "Total_Payment_Amount", "Hrs_Enrolled")
            dataWriter.WriteCommaDelimitedLine(approvedStudent.StateStudentId, approvedStudent.FirstName, approvedStudent.LastName, approvedStudent.ContactInfo.HomeAddress.Line1, approvedStudent.ContactInfo.HomeAddress.Line2, approvedStudent.ContactInfo.HomeAddress.City, approvedStudent.ContactInfo.HomeAddress.State, approvedStudent.ContactInfo.HomeAddress.ZipCode, approvedStudent.ContactInfo.HomeAddress.Country, term, institution, totalPaymentAmount, hours)
        End Using
        Return True
    End Function

    Private Shared Function GetLastSemesterPayment(ByVal batchPayments As IEnumerable(Of Payment)) As Payment
        'Only positive payments need be considered.
        Dim positivePayments As IEnumerable(Of Payment) = batchPayments.Where(Function(p) p.Amount > 0)
        If (positivePayments.Count() = 0) Then Return Nothing
        'Find the latest year and get all payments for that year.
        Dim latestYear As Integer = positivePayments.Select(Function(p) p.Year).Max()
        Dim latestPayments As IEnumerable(Of Payment) = positivePayments.Where(Function(p) p.Year = latestYear)
        If (latestPayments.Count() = 1) Then Return latestPayments.Single()
        'Working backwards, return the first semester we come across.
        Dim fallPayments As IEnumerable(Of Payment) = latestPayments.Where(Function(p) p.Semester = Constants.CollegeTerm.FALL)
        If (fallPayments.Count() = 1) Then
            Return fallPayments.Single()
        ElseIf (fallPayments.Count() > 1) Then
            Return fallPayments.OrderBy(Function(p) p.Amount).Last()
        End If
        Dim summerPayments As IEnumerable(Of Payment) = latestPayments.Where(Function(p) p.Semester = Constants.CollegeTerm.SUMMER)
        If (summerPayments.Count() = 1) Then
            Return summerPayments.Single()
        ElseIf (summerPayments.Count() > 1) Then
            Return summerPayments.OrderBy(Function(p) p.Amount).Last()
        End If
        Dim springPayments As IEnumerable(Of Payment) = latestPayments.Where(Function(p) p.Semester = Constants.CollegeTerm.SPRING)
        If (springPayments.Count() = 1) Then
            Return springPayments.Single()
        ElseIf (springPayments.Count() > 1) Then
            Return springPayments.OrderBy(Function(p) p.Amount).Last()
        End If
        Dim winterPayments As IEnumerable(Of Payment) = latestPayments.Where(Function(p) p.Semester = Constants.CollegeTerm.WINTER)
        If (winterPayments.Count() = 1) Then
            Return winterPayments.Single()
        ElseIf (winterPayments.Count() > 1) Then
            Return winterPayments.OrderBy(Function(p) p.Amount).Last()
        End If
        'If nothing has been returned by now, the list was empty.
        Return Nothing
    End Function
End Class
