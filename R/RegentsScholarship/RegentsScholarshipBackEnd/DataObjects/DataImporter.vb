Imports System.IO
Imports System.Net.Mail
Imports System.Text
Imports System.Text.RegularExpressions
Imports Q

''' <summary>
''' Reads records from plain-text scholarship application data files and feeds the data into the appropriate database tables.
''' </summary>
Public Class DataImporter
    ''' <summary>
    ''' Enumerates the attributes (fields) of the records in the data file.
    ''' </summary>
    ''' <remarks>To be useful, these must exactly match the file.</remarks>
    Private Enum Attribute
        StudentFirstName
        StudentMiddleName
        StudentLastName
        StudentAddress
        StudentCity
        StudentState
        StudentZip
        StudentPhone
        StudentEmail
        StudentDateOfBirth
        StudentGender
        StudentEthnicity
        StateStudentId
        HasCriminalRecord
        IsUsCitizen
        IsEligibleForFederalAid
        IntendsToApplyForFederalAid
        EnrollmentYear
        HighSchoolIsInUtah
        HighSchoolName
        HighSchoolCeebCode
        HighSchoolCity
        HighSchoolState
        HighSchoolDistrict
        HighSchoolGraduationMonthYear
        HighSchoolGpa
        ActCompositeScore
        EnglishClass1Title
        EnglishClass1GradeLevel
        EnglishClass1Credits
        EnglishClass1Grade1
        EnglishClass1Grade2
        EnglishClass1Grade3
        EnglishClass1Grade4
        EnglishClass1Grade5
        EnglishClass1Grade6
        EnglishClass2Title
        EnglishClass2GradeLevel
        EnglishClass2Credits
        EnglishClass2Grade1
        EnglishClass2Grade2
        EnglishClass2Grade3
        EnglishClass2Grade4
        EnglishClass2Grade5
        EnglishClass2Grade6
        EnglishClass3Title
        EnglishClass3GradeLevel
        EnglishClass3Credits
        EnglishClass3Grade1
        EnglishClass3Grade2
        EnglishClass3Grade3
        EnglishClass3Grade4
        EnglishClass3Grade5
        EnglishClass3Grade6
        EnglishClass4Title
        EnglishClass4GradeLevel
        EnglishClass4Credits
        EnglishClass4Grade1
        EnglishClass4Grade2
        EnglishClass4Grade3
        EnglishClass4Grade4
        EnglishClass4Grade5
        EnglishClass4Grade6
        EnglishClass5Title
        EnglishClass5GradeLevel
        EnglishClass5Credits
        EnglishClass5Grade1
        EnglishClass5Grade2
        EnglishClass5Grade3
        EnglishClass5Grade4
        EnglishClass5Grade5
        EnglishClass5Grade6
        EnglishClass6Title
        EnglishClass6GradeLevel
        EnglishClass6Credits
        EnglishClass6Grade1
        EnglishClass6Grade2
        EnglishClass6Grade3
        EnglishClass6Grade4
        EnglishClass6Grade5
        EnglishClass6Grade6
        MathematicsClass1Title
        MathematicsClass1GradeLevel
        MathematicsClass1Credits
        MathematicsClass1Grade1
        MathematicsClass1Grade2
        MathematicsClass1Grade3
        MathematicsClass1Grade4
        MathematicsClass1Grade5
        MathematicsClass1Grade6
        MathematicsClass2Title
        MathematicsClass2GradeLevel
        MathematicsClass2Credits
        MathematicsClass2Grade1
        MathematicsClass2Grade2
        MathematicsClass2Grade3
        MathematicsClass2Grade4
        MathematicsClass2Grade5
        MathematicsClass2Grade6
        MathematicsClass3Title
        MathematicsClass3GradeLevel
        MathematicsClass3Credits
        MathematicsClass3Grade1
        MathematicsClass3Grade2
        MathematicsClass3Grade3
        MathematicsClass3Grade4
        MathematicsClass3Grade5
        MathematicsClass3Grade6
        MathematicsClass4Title
        MathematicsClass4GradeLevel
        MathematicsClass4Credits
        MathematicsClass4Grade1
        MathematicsClass4Grade2
        MathematicsClass4Grade3
        MathematicsClass4Grade4
        MathematicsClass4Grade5
        MathematicsClass4Grade6
        MathematicsClass5Title
        MathematicsClass5GradeLevel
        MathematicsClass5Credits
        MathematicsClass5Grade1
        MathematicsClass5Grade2
        MathematicsClass5Grade3
        MathematicsClass5Grade4
        MathematicsClass5Grade5
        MathematicsClass5Grade6
        MathematicsClass6Title
        MathematicsClass6GradeLevel
        MathematicsClass6Credits
        MathematicsClass6Grade1
        MathematicsClass6Grade2
        MathematicsClass6Grade3
        MathematicsClass6Grade4
        MathematicsClass6Grade5
        MathematicsClass6Grade6
        ScienceClass1Title
        ScienceClass1GradeLevel
        ScienceClass1Credits
        ScienceClass1Grade1
        ScienceClass1Grade2
        ScienceClass1Grade3
        ScienceClass1Grade4
        ScienceClass1Grade5
        ScienceClass1Grade6
        ScienceClass2Title
        ScienceClass2GradeLevel
        ScienceClass2Credits
        ScienceClass2Grade1
        ScienceClass2Grade2
        ScienceClass2Grade3
        ScienceClass2Grade4
        ScienceClass2Grade5
        ScienceClass2Grade6
        ScienceClass3Title
        ScienceClass3GradeLevel
        ScienceClass3Credits
        ScienceClass3Grade1
        ScienceClass3Grade2
        ScienceClass3Grade3
        ScienceClass3Grade4
        ScienceClass3Grade5
        ScienceClass3Grade6
        ScienceClass4Title
        ScienceClass4GradeLevel
        ScienceClass4redits
        ScienceClass4Grade1
        ScienceClass4Grade2
        ScienceClass4Grade3
        ScienceClass4Grade4
        ScienceClass4Grade5
        ScienceClass4Grade6
        ScienceClass5Title
        ScienceClass5GradeLevel
        ScienceClass5Credits
        ScienceClass5Grade1
        ScienceClass5Grade2
        ScienceClass5Grade3
        ScienceClass5Grade4
        ScienceClass5Grade5
        ScienceClass5Grade6
        ScienceClass6Title
        ScienceClass6GradeLevel
        ScienceClass6Credits
        ScienceClass6Grade1
        ScienceClass6Grade2
        ScienceClass6Grade3
        ScienceClass6Grade4
        ScienceClass6Grade5
        ScienceClass6Grade6
        SocialScienceClass1Title
        SocialScienceClass1GradeLevel
        SocialScienceClass1Credits
        SocialScienceClass1Grade1
        SocialScienceClass1Grade2
        SocialScienceClass1Grade3
        SocialScienceClass1Grade4
        SocialScienceClass1Grade5
        SocialScienceClass1Grade6
        SocialScienceClass2Title
        SocialScienceClass2GradeLevel
        SocialScienceClass2Credits
        SocialScienceClass2Grade1
        SocialScienceClass2Grade2
        SocialScienceClass2Grade3
        SocialScienceClass2Grade4
        SocialScienceClass2Grade5
        SocialScienceClass2Grade6
        SocialScienceClass3Title
        SocialScienceClass3GradeLevel
        SocialScienceClass3Credits
        SocialScienceClass3Grade1
        SocialScienceClass3Grade2
        SocialScienceClass3Grade3
        SocialScienceClass3Grade4
        SocialScienceClass3Grade5
        SocialScienceClass3Grade6
        SocialScienceClass4Title
        SocialScienceClass4GradeLevel
        SocialScienceClass4Credits
        SocialScienceClass4Grade1
        SocialScienceClass4Grade2
        SocialScienceClass4Grade3
        SocialScienceClass4Grade4
        SocialScienceClass4Grade5
        SocialScienceClass4Grade6
        SocialScienceClass5Title
        SocialScienceClass5GradeLevel
        SocialScienceClass5Credits
        SocialScienceClass5Grade1
        SocialScienceClass5Grade2
        SocialScienceClass5Grade3
        SocialScienceClass5Grade4
        SocialScienceClass5Grade5
        SocialScienceClass5Grade6
        SocialScienceClass6Title
        SocialScienceClass6GradeLevel
        SocialScienceClass6Credits
        SocialScienceClass6Grade1
        SocialScienceClass6Grade2
        SocialScienceClass6Grade3
        SocialScienceClass6Grade4
        SocialScienceClass6Grade5
        SocialScienceClass6Grade6
        SocialScienceClass7Title
        SocialScienceClass7GradeLevel
        SocialScienceClass7Credits
        SocialScienceClass7Grade1
        SocialScienceClass7Grade2
        SocialScienceClass7Grade3
        SocialScienceClass7Grade4
        SocialScienceClass7Grade5
        SocialScienceClass7Grade6
        ForeignLanguageClass1Title
        ForeignLanguageClass1GradeLevel
        ForeignLanguageClass1Credits
        ForeignLanguageClass1Grade1
        ForeignLanguageClass1Grade2
        ForeignLanguageClass1Grade3
        ForeignLanguageClass1Grade4
        ForeignLanguageClass1Grade5
        ForeignLanguageClass1Grade6
        ForeignLanguageClass2Title
        ForeignLanguageClass2GradeLevel
        ForeignLanguageClass2Credits
        ForeignLanguageClass2Grade1
        ForeignLanguageClass2Grade2
        ForeignLanguageClass2Grade3
        ForeignLanguageClass2Grade4
        ForeignLanguageClass2Grade5
        ForeignLanguageClass2Grade6
        ForeignLanguageClass3Title
        ForeignLanguageClass3GradeLevel
        ForeignLanguageClass3Credits
        ForeignLanguageClass3Grade1
        ForeignLanguageClass3Grade2
        ForeignLanguageClass3Grade3
        ForeignLanguageClass3Grade4
        ForeignLanguageClass3Grade5
        ForeignLanguageClass3Grade6
        ForeignLanguageClass4Title
        ForeignLanguageClass4GradeLevel
        ForeignLanguageClass4Credits
        ForeignLanguageClass4Grade1
        ForeignLanguageClass4Grade2
        ForeignLanguageClass4Grade3
        ForeignLanguageClass4Grade4
        ForeignLanguageClass4Grade5
        ForeignLanguageClass4Grade6
        ForeignLanguageClass5Title
        ForeignLanguageClass5GradeLevel
        ForeignLanguageClass5Credits
        ForeignLanguageClass5Grade1
        ForeignLanguageClass5Grade2
        ForeignLanguageClass5Grade3
        ForeignLanguageClass5Grade4
        ForeignLanguageClass5Grade5
        ForeignLanguageClass5Grade6
        DefermentLeave
        SSN
        College
        UespAwardAmount
        ExemplaryAwardEarned
        BaseAwardAmount
    End Enum 'Attribute

    ''' <summary>
    ''' The number of fields/attributes in a record; useful for checking that a record is complete.
    ''' </summary>
    ''' <remarks>This should always be the last value in the enum + 1.</remarks>
    Private Const RECORD_LENGTH As Integer = Attribute.BaseAwardAmount + 1
    Private Const ERROR_FILE As String = "T:\RegentsDataErrors.txt"
    'HACK: Set values for special import of old applications.
    Private Const APPLICATION_YEAR As Integer = 2008
    Private Shared ReadOnly AWARD_STATUS = Constants.AwardStatus.APPROVED

    Private Structure RecordWithValidation
        Public Record() As String
        Public AddressIsValid As Boolean
        Public PhoneIsValid As Boolean
        Public EmailIsValid As Boolean
    End Structure

    ''' <summary>
    ''' Reads records from plain-text scholarship application data files and feeds the data into the appropriate database tables.
    ''' </summary>
    ''' <param name="dataFile">The file (with path) containing application data.</param>
    ''' <exception cref="FileNotFoundException">Thrown when <paramref name="dataFile"/> does not exist.</exception>
    Public Shared Sub ImportData(ByVal dataFile As String)
        'Check that the data file exists.
        If Not File.Exists(dataFile) Then
            Throw New FileNotFoundException()
        End If

        'Remove any existing error file so we can start fresh.
        If File.Exists(ERROR_FILE) Then
            File.Delete(ERROR_FILE)
        End If

        'Read the file into an in-memory list of records with validation attributes.
        Dim recordList As List(Of RecordWithValidation) = ReadRecordsFromFile(dataFile)

        'Check for identifiable problems.
        Dim problemRecords As New List(Of RecordWithValidation)()
        For Each recordItem As RecordWithValidation In recordList
            Dim problemString As String = IdentifyRecordProblems(recordItem.Record)
            If Not String.IsNullOrEmpty(problemString) Then
                'Add the problem string and record to the error file.
                Using errorWriter As New StreamWriter(ERROR_FILE, True)
                    errorWriter.WriteLine(problemString)
                    errorWriter.WriteLine(String.Join(ControlChars.Tab, recordItem.Record))
                    errorWriter.Close()
                End Using
                'Add this record to the list of problem records.
                problemRecords.Add(recordItem)
            End If
        Next recordItem

        'Remove any problem records from the list so they don't get committed to the database.
        For Each problemRecord As RecordWithValidation In problemRecords
            recordList.Remove(problemRecord)
        Next problemRecord

        'Retrieve existing student IDs from the database so that anyone
        'in the data file who's already in the database can be skipped.
        Dim existingStudentIds As IEnumerable(Of String) = DataAccess.GetExistingStudentIds().ToList()

        'Maintain a count of records that were successfully imported.
        Dim sucessCount As Integer = recordList.Count

        'Plug the values into the database.
        For Each recordItem As RecordWithValidation In recordList
            'Skip this student if he/she is already in the database.
            If existingStudentIds.Contains(recordItem.Record(Attribute.StateStudentId).Trim()) Then
                sucessCount -= 1
                Continue For
            End If

            'The import process takes up a lot of memory, so to avoid "Out of Memory"
            'exceptions, release any unused memory between each student.
            GC.Collect()

            Dim stud As Student = Nothing
            Try
                stud = GetStudentObjectFromRecord(recordItem)
                stud.Commit("RSS", Student.Component.Application Or Student.Component.Demographics)
            Catch ex As Exception
                'Any records with invalid data will be sent to the error file.
                Dim currentException As Exception = ex
                Dim errorMessage As String = ex.Message
                'Recursively get each inner exception's message and append it to the error message.
                Do While currentException.InnerException IsNot Nothing
                    errorMessage += String.Format(" {0}", currentException.InnerException.Message)
                    currentException = currentException.InnerException
                Loop
                'Write the error message and record line to the data file.
                Using errorWriter As New StreamWriter(ERROR_FILE, True)
                    errorWriter.WriteLine(errorMessage)
                    errorWriter.WriteLine(String.Join(ControlChars.Tab, recordItem.Record))
                    errorWriter.Close()
                End Using
                sucessCount -= 1
            End Try
        Next recordItem

        'Check for an error file and notify the user.
        If File.Exists(ERROR_FILE) Then
            Dim message As String = String.Format("{0} records loaded.{1}See the {2} file for a list of records that could not be imported.", sucessCount.ToString(), Environment.NewLine, ERROR_FILE)
            Windows.Forms.MessageBox.Show(message, "See error file", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Warning)
        Else
            Dim message As String = String.Format("All {0} records successfully loaded.", sucessCount.ToString())
            Windows.Forms.MessageBox.Show(message, "Done", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
        End If
    End Sub 'ImportData()

    Private Shared Function ReadRecordsFromFile(ByVal dataFile As String) As List(Of RecordWithValidation)
        Dim recordList As New List(Of RecordWithValidation)()
        Using dataReader As New StreamReader(dataFile)
            Do While Not dataReader.EndOfStream
                'Read in a record.
                Dim record() As String = dataReader.ReadLine().Split(ControlChars.Tab)
                'Strip the extra quote marks from the fields. (They can really mess things up later.)
                For field As Integer = 0 To Attribute.ForeignLanguageClass5Grade6
                    record(field) = record(field).Replace("""", "")
                Next field
                'A lot of students have several leading zeros in their ID, so strip those.
                record(Attribute.StateStudentId) = record(Attribute.StateStudentId).TrimStart("0")

                'The student's address, phone, and e-mail validity need to be set
                'at load time, but they're not in the data file.
                'We need to call on the Validator class to check those items,
                'and store the results in a RecordWithValidation instance.
                Dim validation As New RecordWithValidation()
                validation.Record = record
                validation.AddressIsValid = _
                    Validator.IsValidStreetAddress(record(Attribute.StudentAddress).Trim()) _
                    AndAlso Validator.IsValidCity(record(Attribute.StudentCity).Trim()) _
                    AndAlso Validator.IsValidState(record(Attribute.StudentState).Trim()) _
                    AndAlso Validator.IsValidZipCode(record(Attribute.StudentZip).Trim())
                validation.PhoneIsValid = Validator.IsValidPhoneNumber(record(Attribute.StudentPhone).Trim())
                validation.EmailIsValid = Validator.IsValidEmailAddress(record(Attribute.StudentEmail).Trim())

                'Now that everything is set, add the RecordWithValidation to the list.
                recordList.Add(validation)
            Loop
            dataReader.Close()
        End Using

        Return recordList
    End Function 'ReadRecordsFromFile()

    ''' <summary>
    ''' Checks a record for problems we can anticipate.
    ''' </summary>
    ''' <param name="record">String array representing a record that has been split into fields.</param>
    ''' <returns>Colon-delimited string of problems, or empty string if no problems are found.</returns>
    Private Shared Function IdentifyRecordProblems(ByVal record() As String) As String
        Const PRIVATE_SCHOOL_FORMAT As String = "^[A-Za-z]{4}\d{6}$"

        'Problem descriptions will be added to this string list as they arise.
        Dim problemDescriptions As New List(Of String)()

        'Wrong number of fields.
        If record.Length <> RECORD_LENGTH Then
            problemDescriptions.Add(String.Format("Wrong number of fields. There should be {0}, but the record has {1}.", RECORD_LENGTH, record.Count()))
        End If

        'First name is missing.
        If record(Attribute.StudentFirstName).Trim().Length = 0 Then
            problemDescriptions.Add("Missing student's first name.")
        End If

        'Last name is missing.
        If record(Attribute.StudentLastName).Trim().Length = 0 Then
            problemDescriptions.Add("Missing student's last name.")
        End If

        'Date of birth is missing.
        If record(Attribute.StudentDateOfBirth).Trim().Length = 0 Then
            problemDescriptions.Add("Missing student's date of birth.")
        End If

        'Gender is missing.
        If record(Attribute.StudentGender).Trim().Length = 0 Then
            problemDescriptions.Add("Missing student's gender.")
        End If

        'Student ID is missing.
        If record(Attribute.StateStudentId).Trim().Length = 0 Then
            problemDescriptions.Add("Missing state student ID.")
        End If

        'Student ID looks suspect.
        Dim dummyInteger As Integer = 0
        Dim studentId As String = record(Attribute.StateStudentId).Trim()
        Dim studentIdIsTooLong As Boolean = studentId.Length > 10
        Dim studentIdIsTooShort As Boolean = studentId.Length < 4
        Dim studentIdIsNumeric As Boolean = Integer.TryParse(record(Attribute.StateStudentId).Trim(), dummyInteger)
        Dim studentIdIsAllZeros As Boolean = studentIdIsNumeric AndAlso Integer.Parse(studentId) = 0
        Dim studentIdIsPrivateFormat As Boolean = Regex.IsMatch(studentId, PRIVATE_SCHOOL_FORMAT)
        Dim studentIdIsFormattedCorrectly As Boolean = (studentIdIsNumeric OrElse studentIdIsPrivateFormat)
        If studentIdIsTooLong OrElse studentIdIsTooShort OrElse (Not studentIdIsFormattedCorrectly) OrElse studentIdIsAllZeros Then
            problemDescriptions.Add("The state student ID appears incorrect (too long, too short, or bad format).")
        End If

        'U.S. ciziten is marken "no" and eligibility indicator is missing.
        If record(Attribute.IsUsCitizen).Trim() = "0" AndAlso record(Attribute.IsEligibleForFederalAid).Trim().Length = 0 Then
            problemDescriptions.Add("Missing eligibility for federal aid indicator for non-U.S. citizen.")
        End If

        'High school CEEB code is missing.
        If record(Attribute.HighSchoolCeebCode).Trim().Length = 0 Then
            problemDescriptions.Add("Missing high school CEEB code.")
        End If

        'GPA is missing.
        If record(Attribute.HighSchoolGpa).Trim().Length = 0 Then
            problemDescriptions.Add("Missing high school GPA.")
        End If

        Return If(problemDescriptions.Count > 0, String.Join(":", problemDescriptions.ToArray()), "")
    End Function 'IdentifyRecordProblems()

    Private Shared Function GetStudentObjectFromRecord(ByVal recordItem As RecordWithValidation) As Student
        Dim studentId As String = recordItem.Record(Attribute.StateStudentId).Trim()
        'Check that the student ID is not already in use.
        If (DataAccess.GetExistingStudentIds().Contains(studentId)) Then
            'Try generating a private school-style student ID.
            Dim abbreviatedName As String = recordItem.Record(Attribute.StudentLastName).Trim().SafeSubstring(0, 4).ToLower()
            Dim mmddyy As String
            Dim idSeq As Integer = 1
            Try
                mmddyy = DateTime.Parse(recordItem.Record(Attribute.StudentDateOfBirth).Trim()).ToString("MMddyy")
            Catch ex As Exception
                'Fall back to a sequence number if there's no date of birth.
                mmddyy = idSeq.ToString("000000")
            End Try
            studentId = abbreviatedName + mmddyy
            'Re-check that the new student ID is not in use, and keep trying new ones until one works.
            Do While DataAccess.GetExistingStudentIds().Contains(studentId)
                idSeq += 1
                studentId = abbreviatedName + idSeq.ToString("000000")
            Loop
        End If

        Dim newStudent As New Student(studentId)
        newStudent.FirstName = recordItem.Record(Attribute.StudentFirstName).Trim().ToUpper()
        newStudent.MiddleName = recordItem.Record(Attribute.StudentMiddleName).Trim().ToUpper()
        newStudent.LastName = recordItem.Record(Attribute.StudentLastName).Trim().ToUpper()
        newStudent.SocialSecurityNumber = recordItem.Record(Attribute.SSN).Replace("-", "").Replace("""", "").Trim()

        newStudent.ContactInfo.HomeAddress.Line1 = recordItem.Record(Attribute.StudentAddress).Trim().ToUpper()
        newStudent.ContactInfo.HomeAddress.City = recordItem.Record(Attribute.StudentCity).Trim().ToUpper()
        newStudent.ContactInfo.HomeAddress.State = recordItem.Record(Attribute.StudentState).Trim().ToUpper()
        newStudent.ContactInfo.HomeAddress.ZipCode = recordItem.Record(Attribute.StudentZip).Trim()
        newStudent.ContactInfo.PrimaryPhone.Number = recordItem.Record(Attribute.StudentPhone).Trim()
        newStudent.ContactInfo.PrimaryPhone.IsValid = recordItem.PhoneIsValid
        newStudent.ContactInfo.PersonalEmail.Address = recordItem.Record(Attribute.StudentEmail).Trim()
        newStudent.ContactInfo.PersonalEmail.IsValid = recordItem.EmailIsValid

        Try
            newStudent.DateOfBirth = Date.Parse(recordItem.Record(Attribute.StudentDateOfBirth).Trim())
        Catch ex As Exception
            newStudent.DateOfBirth = Nothing
        End Try
        newStudent.Gender = recordItem.Record(Attribute.StudentGender).Trim().ToUpper()
        newStudent.Ethnicity = recordItem.Record(Attribute.StudentEthnicity).Trim()
        newStudent.HasCriminalRecord = (recordItem.Record(Attribute.HasCriminalRecord).ToLower().Contains("y"))
        newStudent.IsUsCitizen = (recordItem.Record(Attribute.IsUsCitizen).ToLower().Contains("y"))

        newStudent.IsEligibleForFederalAid = (recordItem.Record(Attribute.IsEligibleForFederalAid).ToLower().Contains("y"))
        newStudent.IntendsToApplyForFederalAid = (recordItem.Record(Attribute.IntendsToApplyForFederalAid).ToLower().Contains("y"))

        Dim uespAmount As Double = Double.Parse(recordItem.Record(Attribute.UespAwardAmount))
        If (uespAmount > 0) Then
            newStudent.ScholarshipApplication.UespSupplementalAward.IsApproved = True
            newStudent.ScholarshipApplication.UespSupplementalAward.Amount = uespAmount
        End If
        newStudent.ScholarshipApplication.ExemplaryAward.IsApproved = (recordItem.Record(Attribute.ExemplaryAwardEarned).ToLower().Contains("y"))

        newStudent.ScholarshipApplication.PlannedCollegeToAttend = recordItem.Record(Attribute.College)

        newStudent.HighSchool.IsInUtah = (recordItem.Record(Attribute.HighSchoolIsInUtah).ToLower().Contains("y"))
        newStudent.HighSchool.CeebCode = recordItem.Record(Attribute.HighSchoolCeebCode).Trim()
        If (newStudent.HighSchool.CeebCode = "") Then
            If (newStudent.HighSchool.IsInUtah) Then
                newStudent.HighSchool.CeebCode = "00-002"
            Else
                newStudent.HighSchool.CeebCode = "00-003"
            End If
        End If
        If (Not String.IsNullOrEmpty(recordItem.Record(Attribute.HighSchoolGraduationMonthYear).Trim())) Then
            newStudent.HighSchool.GraduationDate = Date.Parse(recordItem.Record(Attribute.HighSchoolGraduationMonthYear).Trim())
        End If
        Try
            newStudent.HighSchool.CumulativeGpa = Double.Parse(recordItem.Record(Attribute.HighSchoolGpa).Trim())
        Catch ex As Exception
            newStudent.HighSchool.CumulativeGpa = 0
        End Try
        If Not String.IsNullOrEmpty(recordItem.Record(Attribute.ActCompositeScore).Trim()) Then
            newStudent.HighSchool.ActScores.Add(Constants.ActCategory.COMPOSITE, Double.Parse(recordItem.Record(Attribute.ActCompositeScore).Trim()))
        End If

        'Add the courses from each class type to the course list.
        Dim category As CourseCategory = newStudent.HighSchool.CourseCategories(Constants.CourseCategory.ENGLISH)
        category.Courses.AddRange(GetCoursesForClassType(category, recordItem, Attribute.EnglishClass1Title, Attribute.EnglishClass6Title))
        category = newStudent.HighSchool.CourseCategories(Constants.CourseCategory.FOREIGN_LANGUAGE)
        category.Courses.AddRange(GetCoursesForClassType(category, recordItem, Attribute.ForeignLanguageClass1Title, Attribute.ForeignLanguageClass5Title))
        category = newStudent.HighSchool.CourseCategories(Constants.CourseCategory.MATHEMATICS)
        category.Courses.AddRange(GetCoursesForClassType(category, recordItem, Attribute.MathematicsClass1Title, Attribute.MathematicsClass6Title))
        category = newStudent.HighSchool.CourseCategories(Constants.CourseCategory.SCIENCE)
        category.Courses.AddRange(GetCoursesForClassType(category, recordItem, Attribute.ScienceClass1Title, Attribute.ScienceClass6Title))
        category = newStudent.HighSchool.CourseCategories(Constants.CourseCategory.SOCIAL_SCIENCE)
        category.Courses.AddRange(GetCoursesForClassType(category, recordItem, Attribute.SocialScienceClass1Title, Attribute.SocialScienceClass7Title))

        'HACK: Add all reviews, and mark them as done as of the import date.
        Dim initialTranscriptReview As New Review(Constants.ReviewType.INITIAL_TRANSCRIPT, newStudent.ScholarshipApplication)
        initialTranscriptReview.CompletionDate = DateTime.Now.Date
        initialTranscriptReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(initialTranscriptReview)
        Dim secondTranscriptReview As New Review(Constants.ReviewType.SECOND_TRANSCRIPT, newStudent.ScholarshipApplication)
        secondTranscriptReview.CompletionDate = DateTime.Now.Date
        secondTranscriptReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(secondTranscriptReview)
        Dim finalTranscriptReview As New Review(Constants.ReviewType.FINAL_TRANSCRIPT, newStudent.ScholarshipApplication)
        finalTranscriptReview.CompletionDate = DateTime.Now.Date
        finalTranscriptReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(finalTranscriptReview)
        Dim classReview As New Review(Constants.ReviewType.CLASS_, newStudent.ScholarshipApplication)
        classReview.CompletionDate = DateTime.Now.Date
        classReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(classReview)
        Dim categoryReview As New Review(Constants.ReviewType.CATEGORY, newStudent.ScholarshipApplication)
        categoryReview.CompletionDate = DateTime.Now.Date
        categoryReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(categoryReview)
        Dim initialAwardReview As New Review(Constants.ReviewType.INITIAL_AWARD, newStudent.ScholarshipApplication)
        initialAwardReview.CompletionDate = DateTime.Now.Date
        initialAwardReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(initialAwardReview)
        Dim firstQuickReview As New Review(Constants.ReviewType.FIRST_QUICK, newStudent.ScholarshipApplication)
        firstQuickReview.CompletionDate = DateTime.Now.Date
        firstQuickReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(firstQuickReview)
        Dim baseAwardReview As New Review(Constants.ReviewType.BASE_AWARD, newStudent.ScholarshipApplication)
        baseAwardReview.CompletionDate = DateTime.Now.Date
        baseAwardReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(baseAwardReview)
        Dim exemplaryAwardReview As New Review(Constants.ReviewType.EXEMPLARY_AWARD, newStudent.ScholarshipApplication)
        exemplaryAwardReview.CompletionDate = DateTime.Now.Date
        exemplaryAwardReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(exemplaryAwardReview)
        Dim uespAwardReview As New Review(Constants.ReviewType.UESP_AWARD, newStudent.ScholarshipApplication)
        uespAwardReview.CompletionDate = DateTime.Now.Date
        uespAwardReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(uespAwardReview)
        Dim secondQuickReview As New Review(Constants.ReviewType.SECOND_QUICK, newStudent.ScholarshipApplication)
        secondQuickReview.CompletionDate = DateTime.Now.Date
        secondQuickReview.UserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.Reviews.Add(secondQuickReview)

        'HACK: Set the award status according to what's being imported.
        newStudent.ScholarshipApplication.BaseAward.Status = AWARD_STATUS
        newStudent.ScholarshipApplication.BaseAward.StatusUserId = Constants.SYSTEM_USER_ID
        newStudent.ScholarshipApplication.BaseAward.StatusDate = DateTime.Now

        Return newStudent
    End Function 'GetStudentObjectFromRecord()

    Private Shared Function GetCoursesForClassType(ByVal courseCategory As CourseCategory, ByVal recordItem As RecordWithValidation, ByVal firstClassTitleIndex As Integer, ByVal lastClassTitleIndex As Integer) As List(Of Course)
        Const ATTRIBUTES_PER_COURSE As Integer = 9
        Const TERMS_PER_COURSE As Integer = 6
        Const GRADE_OFFSET As Integer = 2   'Number of attributes between the class title and the first term grade.

        Dim courses As New List(Of Course)
        'The courses for each class type are given a sequence number, which is a simple increment.
        Dim sequenceNumber As Integer = 0

        'Loop through all the classes of this category.
        For classTitle As Integer = firstClassTitleIndex To lastClassTitleIndex Step ATTRIBUTES_PER_COURSE
            sequenceNumber += 1

            'Set a flag to determine whether the class should be marked "In Progress."
            Dim classIsInProgress As Boolean = False

            'Skip this class if it's not filled in.
            If String.IsNullOrEmpty(recordItem.Record(classTitle).Trim()) Then
                Continue For
            End If

            'Declare a new Course object and set the sequence number, title, grade level, and credits.
            Dim newCourse As New Course(sequenceNumber, courseCategory)
            newCourse.Title = recordItem.Record(classTitle).Trim()
            newCourse.GradeLevel = recordItem.Record(classTitle + 1).Trim()
            newCourse.Credits = Double.Parse(recordItem.Record(classTitle + 2).Trim())

            'Fill in the academic year the course was taken. (Assumes everyone graduated from 12th grade.)
            Dim courseGradeLevel As Integer
            If (Not Integer.TryParse(newCourse.GradeLevel, courseGradeLevel)) Then courseGradeLevel = 12
            Dim yearsAgo As Integer = 12 - courseGradeLevel
            Dim courseCompletionYear As Integer = APPLICATION_YEAR - yearsAgo
            Dim academicYearTaken As String = String.Format("{0:00}/{1:00}", ((courseCompletionYear - 1) Mod 100), (courseCompletionYear Mod 100))
            newCourse.AcademicYearTaken = academicYearTaken

            'There's no indication of junior high/middle school, so set the school attended as the high school in every case.
            newCourse.SchoolAttended = recordItem.Record(Attribute.HighSchoolCeebCode)

            'Loop through the grades, adding each one to a list.
            Dim grades As New GradeDictionary()
            For term = 1 To TERMS_PER_COURSE
                Dim letterGrade As String = recordItem.Record(classTitle + GRADE_OFFSET + term).Trim()
                'A grade of "IP" means the class is in progress and should be marked as such.
                'We also don't want to add "IP" grades to the list, so skip it after setting the flag.
                If letterGrade.ToLower().StartsWith("i") Then
                    classIsInProgress = True
                    Continue For
                End If
                'Skip any grades that aren't filled in or say "NA."
                If String.IsNullOrEmpty(letterGrade) OrElse letterGrade.ToLower().Contains("n") Then
                    Continue For
                End If

                Dim thisGrade As New Grade(term, newCourse)
                thisGrade.Letter = letterGrade
                grades.Add(thisGrade)
            Next term

            'Assign the grade list to the course.
            newCourse.Grades = grades

            'Mark the class "In Progress" and set the grade level to "WC" if the flag is set.
            If classIsInProgress Then
                newCourse.IsAcceptable = "In Progress"
                newCourse.GradeLevel = "WC"
            End If

            'Add this course to the list.
            courses.Add(newCourse)
        Next classTitle

        Return courses
    End Function 'GetCoursesForClassType()
End Class
