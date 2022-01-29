using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RegentsApplicationDownload.DataAccess;
using Q;

namespace RegentsApplicationDownload
{
	class Downloader
	{
		public event EventHandler<LogEventArgs> Log;
		private readonly string ERROR_REPORT_DIRECTORY;
		private readonly string OTHER_REPORT_DIRECTORY;
		private readonly string SUCCESS_REPORT_DIRECTORY;
		private readonly string J_DRIVE_OTHER_REPORT_DIRECTORY;
		private readonly string J_DRIVE_SUCCESS_REPORT_DIRECTORY;

		private readonly WebDataAccess _web;

		public Downloader()
		{
			string baseReportDirectory = @"X:\Reports\Regents' Scholarship\";
			if (RegentsScholarshipBackEnd.Constants.TEST_MODE)
			{
				baseReportDirectory += @"Test\";
			}
			ERROR_REPORT_DIRECTORY = baseReportDirectory + @"Download Error Reports\";
			OTHER_REPORT_DIRECTORY = baseReportDirectory + @"Other Class Reports\";
			SUCCESS_REPORT_DIRECTORY = baseReportDirectory + @"Download Success Reports\";

			string applicationYear = DateTime.Now.AddMonths(6).ToString("yyyy");
			string jDriveBaseReportDirectory = string.Format(@"J:\Regents Scholarships\RS {0}\{0} App Reports\", applicationYear);
			if (RegentsScholarshipBackEnd.Constants.TEST_MODE)
			{
				jDriveBaseReportDirectory += @"Test\";
			}
			J_DRIVE_OTHER_REPORT_DIRECTORY = jDriveBaseReportDirectory + @"Other Class Reports\";
			J_DRIVE_SUCCESS_REPORT_DIRECTORY = jDriveBaseReportDirectory + @"Download Success Reports\";

			_web = new WebDataAccess(RegentsScholarshipBackEnd.Constants.TEST_MODE);
		}

		public void Run()
		{
			//Make sure we have access to the report directories.
			if (!Directory.Exists(ERROR_REPORT_DIRECTORY) || !Directory.Exists(OTHER_REPORT_DIRECTORY) || !Directory.Exists(SUCCESS_REPORT_DIRECTORY))
			{
				string message = "The report directories are not accessible. Either the network is down, or CNOC needs to map the X drive on the server.";
				throw new Exception(message);
			}
			if (!Directory.Exists(J_DRIVE_OTHER_REPORT_DIRECTORY) || !Directory.Exists(J_DRIVE_SUCCESS_REPORT_DIRECTORY))
			{
				string message = "The J drive report directories are not accessible. Either the network is down, or CNOC needs to map the J drive to the server.";
				throw new Exception(message);
			}

			//Process each new applicant (except for any problem records).
			foreach (Applicant applicant in _web.NewApplicants)
			{
				OnLog(new LogEventArgs(string.Format("Loading data for student \"{0}\"...", applicant.Username)));

				//Get the Student table info first so we can check
				//that this student isn't already in the system.
				Student student = null;
				try
				{
					student = _web.GetStudent(applicant.Username);
				}
				catch (Exception ex)
				{
					OnLog(new LogEventArgs(ex.Message));
					//We can't create a normal exception dump because we don't have a Student object to dump.
					//So create a pseudo-dump that just has the exception message.
					using (StreamWriter dumpWriter = new StreamWriter(string.Format(@"{0}ExceptionDump_{1}.txt", ERROR_REPORT_DIRECTORY, applicant.Username)))
					{
						dumpWriter.WriteLine("Error loading from Student table. Please check the following for this student:");
						dumpWriter.WriteLine("1) There is a record in the Student table.");
						dumpWriter.WriteLine("2) Student.GenderCode is valid according to the GenderLookup table.");
						dumpWriter.WriteLine("3) Student.EthnicityCode is valid according to the EthnicityLookup table.");
						dumpWriter.WriteLine("4) Student.HowHearAboutCode is valid according to the HowHearAboutLookup table.");
						dumpWriter.WriteLine("5) Student.Username exists in the UserInfo table.");
						dumpWriter.WriteLine();
						dumpWriter.WriteLine(ex.ToString());
						dumpWriter.Close();
					}
					continue;
				}
				//HACK: The following try/catch construct is here to deal with problems that should not be present in the Web tables.
				//If they persist, the above try block should be extended to cover this whole try block,
				//and something like the above catch block should be used for diagnosing the problems.
				IEnumerable<Act> actScores;
				IEnumerable<ClassList> classes;
				IEnumerable<Grade> grades;
				ScholarshipApplication scholarshipApplication;
				StudentAddress address;
				try
				{
					//Check that the student ID is not blank or already being used.
					bool studentIdIsBlank = (student.StateStudentId.Trim().Length == 0);
					bool studentIdIsBeingUsed = (RegentsDataAccess.ExistingStudents.Select(p => p.StateStudentId).Contains(student.StateStudentId));
					if (studentIdIsBlank || studentIdIsBeingUsed)
					{
						//Try generating a private school-style student ID.
						string privateId = student.LastName.SafeSubstring(0, 4).ToLower() + student.DateOfBirth.ToString("MMddyy");
						if (RegentsDataAccess.ExistingStudents.Select(p => p.StateStudentId).Contains(privateId))
						{
							//Add a record to the error report.
							ErrorData.AddRecord(student.FirstName, student.LastName, student.StateStudentId, student.Phone, student.Email);
							OnLog(new LogEventArgs(string.Format("Student ID \"{0}\" already in use.", student.StateStudentId)));
							continue;
						}
						else
						{
							student.StateStudentId = privateId;
						}
					}
					//Check that the firstName and birthdate aren't the same as an existing student.
					if (NameAndBirthdateExist(student))
					{
						//Add a record to the error report.
						ErrorData.AddRecord(student.FirstName, student.LastName, student.StateStudentId, student.Phone, student.Email);
						OnLog(new LogEventArgs(string.Format("Student name \"{0} {1}\" with birthdate \"{2}\" already in use.", student.FirstName, student.LastName, student.DateOfBirth.ToString("MM/dd/yyyy"))));
						continue;
					}

					//Get this applicant's data from the remaining tables.
					actScores = _web.GetActScores(applicant.Username).ToList();
					classes = _web.GetClasses(applicant.Username).ToList();
					grades = _web.GetGrades(applicant.Username).ToList();
					scholarshipApplication = _web.GetScholarshipApplication(applicant.Username);
					address = _web.GetAddress(applicant.Username);
				}
				catch
				{
					continue;
				}
				// Create a new Student object from the RegentsScholarshipBackEnd and fill in the details.
				RegentsScholarshipBackEnd.Student backEndStudent = new RegentsScholarshipBackEnd.Student(student.StateStudentId);
				try
				{
					//Demographics:
					backEndStudent.FirstName = student.FirstName.Trim().ToUpper();
					backEndStudent.MiddleName = student.MiddleName.Trim().ToUpper();
					backEndStudent.LastName = student.LastName.Trim().ToUpper();
					backEndStudent.DateOfBirth = student.DateOfBirth;
					backEndStudent.Gender = student.Gender.Trim();
					backEndStudent.Ethnicity = student.Ethnicity.Trim();
					backEndStudent.HasCriminalRecord = student.HasCriminalRecord;
					backEndStudent.HasUespAccount = student.UESP;
					backEndStudent.IsUsCitizen = student.IsUsCitizen;
					//Contact info:
					backEndStudent.ContactInfo.HomeAddress.Line1 = address.Address1.Trim().ToUpper();
					backEndStudent.ContactInfo.HomeAddress.Line2 = address.Address2.Trim().ToUpper();
					bool streetIsValid = RegentsScholarshipBackEnd.Validator.IsValidStreetAddress(address.Address1.Trim() + " " + address.Address2.Trim());
					backEndStudent.ContactInfo.HomeAddress.City = address.City.Trim().ToUpper();
					bool cityIsValid = RegentsScholarshipBackEnd.Validator.IsValidCity(address.City.Trim());
					backEndStudent.ContactInfo.HomeAddress.State = address.State.Trim().ToUpper();
					bool stateIsValid = RegentsScholarshipBackEnd.Validator.IsValidState(address.State.Trim());
					backEndStudent.ContactInfo.HomeAddress.ZipCode = address.Zip.Trim();
					bool zipIsValid = RegentsScholarshipBackEnd.Validator.IsValidZipCode(address.Zip.Trim());
					backEndStudent.ContactInfo.HomeAddress.Country = address.Country.Trim().ToUpper();
					backEndStudent.ContactInfo.HomeAddress.IsValid = (streetIsValid && cityIsValid && stateIsValid && zipIsValid);
					RegentsScholarshipBackEnd.Phone phone = new RegentsScholarshipBackEnd.Phone(RegentsScholarshipBackEnd.Constants.PhoneType.PRIMARY, backEndStudent.ContactInfo);
					phone.Number = student.Phone.Trim();
					phone.IsValid = RegentsScholarshipBackEnd.Validator.IsValidPhoneNumber(student.Phone.Trim());
					backEndStudent.ContactInfo.PrimaryPhone = phone;
					RegentsScholarshipBackEnd.Email email = new RegentsScholarshipBackEnd.Email(RegentsScholarshipBackEnd.Constants.EmailType.PERSONAL, backEndStudent.ContactInfo);
					email.Address = student.Email.Trim();
					email.IsValid = RegentsScholarshipBackEnd.Validator.IsValidEmailAddress(student.Email.Trim());
					backEndStudent.ContactInfo.PersonalEmail = email;
					//Application info:
					backEndStudent.ScholarshipApplication.ApplicationYear = applicant.SubmitDate.AddMonths(6).ToString("yyyy");
					backEndStudent.IsEligibleForFederalAid = student.IsEligibleForFederalAid;
					backEndStudent.IntendsToApplyForFederalAid = student.IntendsToApplyForFederalAid;
					backEndStudent.ScholarshipApplication.DocumentStatusDates.Add(RegentsScholarshipBackEnd.Constants.DocumentType.APPLICATION, applicant.SubmitDate);
					//High school info:
					backEndStudent.HighSchool.IsInUtah = scholarshipApplication.HighSchoolIsInUtah;
					backEndStudent.HighSchool.CeebCode = scholarshipApplication.HighSchoolCeebCode.Trim();
					backEndStudent.HighSchool.GraduationDate = new DateTime(int.Parse(backEndStudent.ScholarshipApplication.ApplicationYear), 5, 30);
					backEndStudent.HighSchool.CumulativeGpa = scholarshipApplication.HighSchoolCumulativeGpa;
					backEndStudent.ScholarshipApplication.NinthGradeSchool = scholarshipApplication.JuniorHighCeebCode.Trim();
					backEndStudent.ScholarshipApplication.AttendedAnotherSchool = scholarshipApplication.HaveAttendedOther;
					backEndStudent.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.COMPOSITE, actScores.Single(p => p.Type == "Composite").Score);
					backEndStudent.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.ENGLISH, actScores.Single(p => p.Type == "English").Score);
					backEndStudent.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.MATH, actScores.Single(p => p.Type == "Math").Score);
					backEndStudent.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.READING, actScores.Single(p => p.Type == "Reading").Score);
					backEndStudent.HighSchool.ActScores.Add(RegentsScholarshipBackEnd.Constants.ActCategory.SCIENCE, actScores.Single(p => p.Type == "Science").Score);
					IEnumerable<string> defaultClassTitles = _web.GetDefaultClassTitles(scholarshipApplication.HighSchoolCeebCode).ToList();
					List<string> courseCategories = new List<string>();
					courseCategories.Add(RegentsScholarshipBackEnd.Constants.CourseCategory.ENGLISH);
					courseCategories.Add(RegentsScholarshipBackEnd.Constants.CourseCategory.FOREIGN_LANGUAGE);
					courseCategories.Add(RegentsScholarshipBackEnd.Constants.CourseCategory.MATHEMATICS);
					courseCategories.Add(RegentsScholarshipBackEnd.Constants.CourseCategory.SCIENCE);
					courseCategories.Add(RegentsScholarshipBackEnd.Constants.CourseCategory.SOCIAL_SCIENCE);
					foreach (string currentCategory in courseCategories)
					{
						foreach (ClassList course in classes.Where(p => p.ClassType.Trim() == currentCategory))
						{
							RegentsScholarshipBackEnd.Course backEndCourse = new RegentsScholarshipBackEnd.Course(course.SequenceNo, backEndStudent.HighSchool.CourseCategories[currentCategory]);
							backEndCourse.Credits = course.Credits;
							backEndCourse.GradeLevel = (course.GradeLevel.Trim() == "12" ? "WC" : course.GradeLevel.Trim());
							backEndCourse.SchoolAttended = (course.GradeLevel.Trim() == "9" ? scholarshipApplication.JuniorHighCeebCode.Trim() : scholarshipApplication.HighSchoolCeebCode.Trim());
							backEndCourse.ConcurrentCollege = course.ConcurrentEnrollmentCollege;
							//Calculate the academic year taken.
							if (!Regex.IsMatch(course.GradeLevel.Trim(), @"^((9)|([1][0-2]))$"))
							{
								throw new Exception(string.Format("Could not determine academic year for {0} class titled {1}.", currentCategory, course.ClassTitle));
							}
							int courseGradeLevel = int.Parse(course.GradeLevel.Trim());
							int yearsAgo = scholarshipApplication.GradeLevel - courseGradeLevel;
							int courseCompletionYear = int.Parse(backEndStudent.ScholarshipApplication.ApplicationYear) - yearsAgo;
							string academicYearTaken = string.Format("{0:00}/{1:00}", ((courseCompletionYear - 1) % 100), (courseCompletionYear % 100));
							backEndCourse.AcademicYearTaken = academicYearTaken;
							backEndCourse.Title = course.ClassTitle.Trim();
							if (!defaultClassTitles.Contains(course.ClassTitle.Trim()))
							{
								OtherClassData.AddRecord(student.FirstName.Trim(), student.LastName.Trim(), currentCategory, course.ClassTitle.Trim(), _web.GetSchoolName(backEndCourse.SchoolAttended), course.GradeLevel, academicYearTaken);
							}
							backEndCourse.Weight = course.Weight.Trim();
							foreach (Grade grade in grades.Where(p => p.ClassType.Trim() == course.ClassType.Trim() && p.ClassSequenceNo == course.SequenceNo))
							{
								RegentsScholarshipBackEnd.Grade backEndGrade = new RegentsScholarshipBackEnd.Grade(grade.Term, backEndCourse);
								backEndGrade.Letter = grade.LetterGrade.Trim();
								backEndCourse.Grades.Add(backEndGrade);
							}
							backEndStudent.HighSchool.CourseCategories[currentCategory].Courses.Add(backEndCourse);
						}
					}

					//Commit the student to the internal database and
					//let the Web database know this applicant is done.
					OnLog(new LogEventArgs(string.Format("Saving student \"{0}\" (SSID \"{1}\") to the internal database...", applicant.Username, student.StateStudentId)));
					backEndStudent.Commit("RSS", RegentsScholarshipBackEnd.Student.Component.Application | RegentsScholarshipBackEnd.Student.Component.Demographics);
					//Set certain items directly, when they can't be set through the back-end.
					try
					{
						RegentsDataAccess.SetPlannedCollege(student.StateStudentId, scholarshipApplication.College);
						RegentsDataAccess.SetHowHearAbout(student.StateStudentId, student.HowHearAbout);
					}
					catch (Exception ex)
					{
						throw new RegentsScholarshipBackEnd.RegentsInvalidDataException(ex.Message, ex);
					}
					OnLog(new LogEventArgs(string.Format("Marking student \"{0}\" as downloaded.", applicant.Username)));
					_web.MarkApplicantAsDownloaded(applicant.Username);
					SuccessData.AddRecord(student.FirstName, student.LastName, student.StateStudentId);
				}
				catch (Exception ex)
				{
					OnLog(new LogEventArgs(ex.Message));
					DumpStudentInfo(ex, backEndStudent);
				}
			}//foreach

			OnLog(new LogEventArgs("Creating the success/error/other reports..."));
			GenerateReports();
			bool testMode = RegentsScholarshipBackEnd.Constants.TEST_MODE;
			string recipients = BsysDataAccess.GetDistributionList(BsysDataAccess.ErrorType.DownloadError);
			string subject = string.Format("Regents' Scholarship DL Success Notification--{0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
			string body = "The Regents' download script ran to completion successfully. The script reports are available for review.";
			Common.SendMail(testMode, recipients, "", subject, body, "", "", "", Common.EmailImportanceLevel.Normal, testMode);
			OnLog(new LogEventArgs("Done!"));
		}//Run()

		protected virtual void OnLog(LogEventArgs e)
		{
			//Send the event to any subscribers.
			if (Log != null) { Log(this, e); }
		}//OnLog()

		private bool NameAndBirthdateExist(Student student)
		{
			foreach (ExistingStudent existingStudent in RegentsDataAccess.ExistingStudents)
			{
				//Skip this student if the name or birthdate doesn't match.
				if (string.Compare(student.FirstName.Trim(), existingStudent.FirstName.Trim(), true) != 0) { continue; }
				if (string.Compare(student.LastName.Trim(), existingStudent.LastName.Trim(), true) != 0) { continue; };
				if (student.DateOfBirth.Date != existingStudent.DateOfBirth.Date) { continue; }
				//If we haven't skipped this student yet, everything matches.
				return true;
			}
			return false;
		}//NameAndBirthdateExist()

		private void GenerateReports()
		{
			string dateTimeStamp = DateTime.Now.ToString("MM_dd_yyyy_HHmm");
			string regentsLetterDirectory = @"X:\PADD\RegentsScholarship\";
			if (RegentsScholarshipBackEnd.Constants.TEST_MODE)
			{
				regentsLetterDirectory += @"Test\";
			}

			//Success report--create it whether there's success data or not.
			if (!SuccessData.Exists()) { SuccessData.CreateDummy(); }
			//The following block is not part of a control statement, but it does limit
			//the scope of the variables declared inside it, which keeps them from
			//conflicting with variables of the same name in the blocks below it.
			{
				string baseName = "DL_Success_" + dateTimeStamp;
				string saveFile = SUCCESS_REPORT_DIRECTORY + baseName;
				DocumentHandling.SaveDocs(regentsLetterDirectory, "REGSCAPSU", SuccessData.FileName, saveFile);
				File.Copy(saveFile + ".doc", J_DRIVE_SUCCESS_REPORT_DIRECTORY + baseName + ".doc", true);
				SuccessData.Delete();
			}

			//Other class report
			if (OtherClassData.Exists())
			{
				string baseName = "Other_Class_" + dateTimeStamp;
				string saveFile = OTHER_REPORT_DIRECTORY + baseName;
				DocumentHandling.SaveDocs(regentsLetterDirectory, "REGSCAPOTH", OtherClassData.FileName, saveFile);
				File.Copy(saveFile + ".doc", J_DRIVE_OTHER_REPORT_DIRECTORY + baseName + ".doc", true);
				OtherClassData.Delete();
			}

			//Error report
			if (ErrorData.Exists())
			{
				string saveFile = string.Format(@"{0}DL_Err_{1}", ERROR_REPORT_DIRECTORY, dateTimeStamp);
				DocumentHandling.SaveDocs(regentsLetterDirectory, "REGSCAPERR", ErrorData.FileName, saveFile);
				ErrorData.Delete();

				bool testMode = RegentsScholarshipBackEnd.Constants.TEST_MODE;
				string recipients = BsysDataAccess.GetDistributionList(BsysDataAccess.ErrorType.DownloadError);
				string subject = string.Format("Regents' Download Error Report Generated {0}", DateTime.Now.ToString("MM/dd/yyyy"));
				string message = string.Format(@"Please check {0} to view the report dated {1} to view the students that attempted to download but already have a record in the database.", ERROR_REPORT_DIRECTORY, DateTime.Now.ToString("MM/dd/yyyy"));
				Common.SendMail(testMode, recipients, "", subject, message, "", "", "", Common.EmailImportanceLevel.Normal, testMode);
			}

			//See if any database error dumps were generated today.
			string[] exceptionDumps = Directory.GetFiles(ERROR_REPORT_DIRECTORY, "ExceptionDump_*", SearchOption.TopDirectoryOnly);
			IEnumerable<string> todaysDumps = exceptionDumps.Where(p => new FileInfo(p).CreationTime.Date == DateTime.Now.Date);
			if (todaysDumps.Count() > 0)
			{
				//E-mail an error message to the appropriate people.
				bool testMode = RegentsScholarshipBackEnd.Constants.TEST_MODE;
				const string SUBJECT = "Regents' Scholarship Individual DL Error";
				string distributionList = BsysDataAccess.GetDistributionList(BsysDataAccess.ErrorType.RuntimeError);
				string message = string.Format("There was a problem downloading one or more individual student applications during the {0} Regents' download run. Please check the stack trace information to investigate.", DateTime.Now.ToString("MM/dd/yyyy"));
				Common.SendMail(testMode, distributionList, "", SUBJECT, message, "", "", "", Common.EmailImportanceLevel.Normal, testMode);
			}
		}//GenerateReports()

		private void DumpStudentInfo(Exception ex, RegentsScholarshipBackEnd.Student student)
		{
			string fileName = string.Format(@"{0}ExceptionDump_{1}.txt", ERROR_REPORT_DIRECTORY, student.StateStudentId());
			using (StreamWriter studentWriter = new StreamWriter(fileName))
			{
				studentWriter.WriteLine("Exception:");
				studentWriter.WriteLine(ex.Message);
				studentWriter.WriteLine();
				studentWriter.WriteLine("Stack trace:");
				studentWriter.WriteLine(ex.StackTrace);
				studentWriter.WriteLine();
				if (ex.InnerException != null)
				{
					studentWriter.WriteLine("Inner Exception:");
					studentWriter.WriteLine(ex.InnerException.Message);
					studentWriter.WriteLine();
					studentWriter.WriteLine("Inner Stack trace:");
					studentWriter.WriteLine(ex.InnerException.StackTrace);
					studentWriter.WriteLine();
				}
				studentWriter.WriteLine("student.StateStudentId:");
				studentWriter.WriteLine(student.StateStudentId());
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.FirstName:");
				studentWriter.WriteLine(student.FirstName);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.MiddleName:");
				studentWriter.WriteLine(student.MiddleName);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.LastName:");
				studentWriter.WriteLine(student.LastName);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.AlternateLastName:");
				studentWriter.WriteLine(student.AlternateLastName);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.DateOfBirth:");
				studentWriter.WriteLine(student.DateOfBirth);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.Ethnicity:");
				studentWriter.WriteLine(student.Ethnicity);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.Gende:");
				studentWriter.WriteLine(student.Gender);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HasCriminalRecord:");
				studentWriter.WriteLine(student.HasCriminalRecord);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.IntendsToApplyForFederalAid:");
				studentWriter.WriteLine(student.IntendsToApplyForFederalAid);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.IsEligibleForFederalAid:");
				studentWriter.WriteLine(student.IsEligibleForFederalAid);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.IsUsCitizen:");
				studentWriter.WriteLine(student.IsUsCitizen);
				studentWriter.WriteLine();
				for (int i = 0; i < student.AuthorizedThirdParties.Count; i++)
				{
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].Id:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].Id);
					studentWriter.WriteLine();
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].Name:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].Name);
					studentWriter.WriteLine();
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].Addr1:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].Address1);
					studentWriter.WriteLine();
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].Addr2:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].Address2);
					studentWriter.WriteLine();
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].City:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].City);
					studentWriter.WriteLine();
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].State:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].State);
					studentWriter.WriteLine();
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].Zip:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].Zip);
					studentWriter.WriteLine();
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].Country:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].Country);
					studentWriter.WriteLine();
					studentWriter.WriteLine("student.AuthorizedThirdParties[{0}].IsValid:", i);
					studentWriter.WriteLine(student.AuthorizedThirdParties[i].IsValid);
				}
				studentWriter.WriteLine("student.College.Name:");
				studentWriter.WriteLine(student.College.Name);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.College.NumberOfEnrolledCredits:");
				studentWriter.WriteLine(student.College.NumberOfEnrolledCredits);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.College.Term:");
				studentWriter.WriteLine(student.College.Term);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.College.TermBeginDate:");
				studentWriter.WriteLine(student.College.TermBeginDate);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.College.HasOtherScholarships:");
				studentWriter.WriteLine(student.College.HasOtherScholarships);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.College.OtherScholarshipsAmount:");
				studentWriter.WriteLine(student.College.OtherScholarshipsAmount);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.CellPhone.Number:");
				studentWriter.WriteLine(student.ContactInfo.CellPhone.Number);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.CellPhone.IsValid:");
				studentWriter.WriteLine(student.ContactInfo.CellPhone.IsValid);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.HomeAddress.Line1:");
				studentWriter.WriteLine(student.ContactInfo.HomeAddress.Line1);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.HomeAddress.Line2:");
				studentWriter.WriteLine(student.ContactInfo.HomeAddress.Line2);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.HomeAddress.City:");
				studentWriter.WriteLine(student.ContactInfo.HomeAddress.City);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.HomeAddress.State:");
				studentWriter.WriteLine(student.ContactInfo.HomeAddress.State);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.HomeAddress.ZipCodee:");
				studentWriter.WriteLine(student.ContactInfo.HomeAddress.ZipCode);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.HomeAddress.Country:");
				studentWriter.WriteLine(student.ContactInfo.HomeAddress.Country);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.HomeAddress.IsValid:");
				studentWriter.WriteLine(student.ContactInfo.HomeAddress.IsValid);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.PersonalEmail.Address:");
				studentWriter.WriteLine(student.ContactInfo.PersonalEmail.Address);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.PersonalEmail.IsValid:");
				studentWriter.WriteLine(student.ContactInfo.PersonalEmail.IsValid);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.PrimaryPhone.Number:");
				studentWriter.WriteLine(student.ContactInfo.PrimaryPhone.Number);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.PrimaryPhone.IsValid:");
				studentWriter.WriteLine(student.ContactInfo.PrimaryPhone.IsValid);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.SchoolEmail.Address:");
				studentWriter.WriteLine(student.ContactInfo.SchoolEmail.Address);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ContactInfo.SchoolEmail.IsValid:");
				studentWriter.WriteLine(student.ContactInfo.SchoolEmail.IsValid);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.ApplicationYear:");
				studentWriter.WriteLine(student.ScholarshipApplication.ApplicationYear);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.AttendedAnotherSchool:");
				studentWriter.WriteLine(student.ScholarshipApplication.AttendedAnotherSchool);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.HowTheyHeardAboutRegents:");
				studentWriter.WriteLine(student.ScholarshipApplication.HowTheyHeardAboutRegents);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.NinthGradeSchool:");
				studentWriter.WriteLine(student.ScholarshipApplication.NinthGradeSchool);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.PlannedCollegeToAttend:");
				studentWriter.WriteLine(student.ScholarshipApplication.PlannedCollegeToAttend);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.BaseAward.Status:");
				studentWriter.WriteLine(student.ScholarshipApplication.BaseAward.Status);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.BaseAward.StatusDate:");
				studentWriter.WriteLine(student.ScholarshipApplication.BaseAward.StatusDate);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.BaseAward.StatusUserId:");
				studentWriter.WriteLine(student.ScholarshipApplication.BaseAward.StatusUserId);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.BaseAward.StatusLetterSentDate:");
				studentWriter.WriteLine(student.ScholarshipApplication.BaseAward.StatusLetterSentDate);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.BaseAward.Amount:");
				studentWriter.WriteLine(student.ScholarshipApplication.BaseAward.Amount());
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.ExemplaryAward.IsApproved:");
				studentWriter.WriteLine(student.ScholarshipApplication.ExemplaryAward.IsApproved);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.ExemplaryAward.Amount:");
				studentWriter.WriteLine(student.ScholarshipApplication.ExemplaryAward.Amount);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.UespSupplementalAward.IsApproved:");
				studentWriter.WriteLine(student.ScholarshipApplication.UespSupplementalAward.IsApproved);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.ScholarshipApplication.UespSupplementalAward.Amount:");
				studentWriter.WriteLine(student.ScholarshipApplication.UespSupplementalAward.Amount);
				studentWriter.WriteLine();
				foreach (string denialReason in student.ScholarshipApplication.DenialReasons)
				{
					studentWriter.WriteLine("student.ScholarshipApplication.DenialReasons:");
					studentWriter.WriteLine(denialReason);
					studentWriter.WriteLine();
				}
				foreach (KeyValuePair<string, DateTime?> document in student.ScholarshipApplication.DocumentStatusDates)
				{
					studentWriter.WriteLine("{0} Received:", document.Key);
					studentWriter.WriteLine(document.Value.HasValue ? document.Value.Value.ToString("MM/dd/yyyy") : "(no date)");
					studentWriter.WriteLine();
				}
				foreach (RegentsScholarshipBackEnd.Review review in student.ScholarshipApplication.Reviews.Values)
				{
					studentWriter.WriteLine("{0} Review:", review.ReviewType());
					studentWriter.WriteLine(review.UserId);
					studentWriter.WriteLine(review.CompletionDate.HasValue ? review.CompletionDate.Value.ToString("MM/dd/yyyy") : "(no date)");
					studentWriter.WriteLine();
				}
				studentWriter.WriteLine("student.HighSchool.CeebCode:");
				studentWriter.WriteLine(student.HighSchool.CeebCode);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HighSchool.Name:");
				studentWriter.WriteLine(student.HighSchool.Name);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HighSchool.District:");
				studentWriter.WriteLine(student.HighSchool.District);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HighSchool.City:");
				studentWriter.WriteLine(student.HighSchool.City);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HighSchool.IsInUtah:");
				studentWriter.WriteLine(student.HighSchool.IsInUtah);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HighSchool.GraduationDate:");
				studentWriter.WriteLine(student.HighSchool.GraduationDate);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HighSchool.DiplomaIsInternationalBaccalaureate:");
				studentWriter.WriteLine(student.HighSchool.DiplomaIsInternationalBaccalaureate);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HighSchool.CumulativeGpa:");
				studentWriter.WriteLine(student.HighSchool.CumulativeGpa);
				studentWriter.WriteLine();
				studentWriter.WriteLine("student.HighSchool.UsbctStatus:");
				studentWriter.WriteLine(student.HighSchool.UsbctStatus);
				studentWriter.WriteLine();
				foreach (KeyValuePair<string, double> act in student.HighSchool.ActScores)
				{
					studentWriter.WriteLine("ACT {0} score:", act.Key);
					studentWriter.WriteLine(act.Value);
					studentWriter.WriteLine();
				}
				foreach (RegentsScholarshipBackEnd.CourseCategory category in student.HighSchool.CourseCategories.Values)
				{
					string categoryString = category.Category();
					studentWriter.WriteLine("{0}.RequirementIsMet:", categoryString);
					studentWriter.WriteLine(category.RequirementIsMet);
					studentWriter.WriteLine();
					studentWriter.WriteLine("{0}.Verification.UserId:", categoryString);
					studentWriter.WriteLine(category.Verification.UserId);
					studentWriter.WriteLine();
					studentWriter.WriteLine("{0}.Verification.TimeStamp:", categoryString);
					studentWriter.WriteLine(category.Verification.TimeStamp);
					studentWriter.WriteLine();
					foreach (RegentsScholarshipBackEnd.Course course in category.Courses)
					{
						studentWriter.WriteLine("{0} course {1} Title:", categoryString, course.SequenceNo());
						studentWriter.WriteLine(course.Title);
						studentWriter.WriteLine();
						studentWriter.WriteLine("{0} course {1} Weight:", categoryString, course.SequenceNo());
						studentWriter.WriteLine(course.Weight);
						studentWriter.WriteLine();
						studentWriter.WriteLine("{0} course {1} GradeLevel:", categoryString, course.SequenceNo());
						studentWriter.WriteLine(course.GradeLevel);
						studentWriter.WriteLine();
						studentWriter.WriteLine("{0} course {1} Credits:", categoryString, course.SequenceNo());
						studentWriter.WriteLine(course.Credits);
						studentWriter.WriteLine();
						studentWriter.WriteLine("{0} course {1} WeightedAverageGrade:", categoryString, course.SequenceNo());
						studentWriter.WriteLine(course.WeightedAverageGrade().GpaValue);
						studentWriter.WriteLine();
						studentWriter.WriteLine("{0} course {1} SchoolAttended:", categoryString, course.SequenceNo());
						studentWriter.WriteLine(course.SchoolAttended);
						studentWriter.WriteLine();
						studentWriter.WriteLine("{0} course {1} AcademicYearTaken:", categoryString, course.SequenceNo());
						studentWriter.WriteLine(course.AcademicYearTaken);
						studentWriter.WriteLine();
						studentWriter.WriteLine("{0} course {1} IsAcceptable:", categoryString, course.SequenceNo());
						studentWriter.WriteLine(course.IsAcceptable);
						studentWriter.WriteLine();
						foreach (RegentsScholarshipBackEnd.Grade grade in course.Grades.Values)
						{
							studentWriter.WriteLine("{0} course {1} term {2} Grade:", categoryString, course.SequenceNo(), grade.Term());
							studentWriter.WriteLine(grade.Letter);
							studentWriter.WriteLine();
							studentWriter.WriteLine("{0} course {1} term {2} GPA Value:", categoryString, course.SequenceNo(), grade.Term());
							studentWriter.WriteLine(grade.GpaValue());
							studentWriter.WriteLine();
						}
					}
				}
				studentWriter.Close();
			}//using
		}//DumpStudentInfo()
	}//class

	//EventArgs class for Log events
	public class LogEventArgs : EventArgs
	{
		public readonly string Message;

		public LogEventArgs(string message)
		{
			Message = message;
		}
	}//class
}//namespace
