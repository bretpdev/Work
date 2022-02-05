using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace RegentsApplicationDownload.DataAccess
{
	class WebDataAccess
	{
		public const string DATA_DIRECTORY = @"C:\Windows\Temp\";
		private DataContext _webDatabase;

		public WebDataAccess(bool testMode)
		{
			string catalog = (testMode ? "RegentsAppTest" : "RegentsApp");
			string connectionString = string.Format("Data Source=ROSS-SQL;Initial Catalog={0};Persist Security Info=True;User ID=procauto;Password=procauto", catalog);
			_webDatabase = new DataContext(connectionString);
		}

		//Keep a static list of new applicants to be sure it only gets queried once.
		private IEnumerable<Applicant> _newApplicants;
		public IEnumerable<Applicant> NewApplicants
		{
			get
			{
				if (_newApplicants == null)
				{
					string query = "SELECT Username, SubmitDate FROM Document WHERE (HasDownloaded IS NULL OR HasDownloaded = 0)";
					_newApplicants = _webDatabase.ExecuteQuery<Applicant>(query).ToList();
				}
				return _newApplicants;
			}
		}

		public IEnumerable<Act> GetActScores(string username)
		{
			StringBuilder queryBuilder = new StringBuilder("SELECT");
			queryBuilder.Append(" B.Description AS Type");
			queryBuilder.Append(" ,CONVERT(FLOAT, A.ScoreCode) AS Score");
			queryBuilder.Append(" FROM Act A");
			queryBuilder.Append(" INNER JOIN ActTypeLookup B");
			queryBuilder.Append(" ON A.TypeCode = B.Code");
			queryBuilder.AppendFormat(" WHERE A.Username = '{0}'", username);
			return _webDatabase.ExecuteQuery<Act>(queryBuilder.ToString());
		}

		public IEnumerable<ClassList> GetClasses(string username)
		{
			StringBuilder queryBuilder = new StringBuilder("SELECT");
			queryBuilder.Append(" B.Description AS ClassType");
			queryBuilder.Append(" ,A.SequenceNo");
			queryBuilder.Append(" ,COALESCE(C.Description, '') AS ClassTitle");
			queryBuilder.Append(" ,A.GradeLevel");
			queryBuilder.Append(" ,COALESCE(A.WeightCode, '') AS Weight");
			queryBuilder.Append(" ,COALESCE(A.Credits, 0) AS Credits");
			queryBuilder.Append(" ,COALESCE(D.Description, '') AS ConcurrentEnrollmentCollege");
			queryBuilder.Append(" FROM ClassList A");
			queryBuilder.Append(" INNER JOIN ClassTypeLookup B");
			queryBuilder.Append(" ON A.ClassTypeCode = B.Code");
			queryBuilder.Append(" INNER JOIN ClassTitleLookup C");
			queryBuilder.Append(" ON A.ClassTitleCode = C.Code");
			queryBuilder.Append(" LEFT OUTER JOIN CollegeLookup D");
			queryBuilder.Append(" ON A.CollegeCode = D.Code");
			queryBuilder.AppendFormat(" WHERE A.Username = '{0}'", username);
			return _webDatabase.ExecuteQuery<ClassList>(queryBuilder.ToString());
		}

		public IEnumerable<Grade> GetGrades(string username)
		{
			StringBuilder queryBuilder = new StringBuilder("SELECT");
			queryBuilder.Append(" B.Description AS ClassType");
			queryBuilder.Append(" ,A.ClassSequenceNo");
			queryBuilder.Append(" ,A.Term");
			queryBuilder.Append(" ,COALESCE(C.Description, '') AS LetterGrade");
			queryBuilder.Append(" FROM Grade A");
			queryBuilder.Append(" INNER JOIN ClassTypeLookup B");
			queryBuilder.Append(" ON A.ClassTypeCode = B.Code");
			queryBuilder.Append(" INNER JOIN GradeLookup C");
			queryBuilder.Append(" ON A.GradeCode = C.Code");
			queryBuilder.AppendFormat(" WHERE A.Username = '{0}'", username);
			return _webDatabase.ExecuteQuery<Grade>(queryBuilder.ToString());
		}

		public ScholarshipApplication GetScholarshipApplication(string username)
		{
			StringBuilder queryBuilder = new StringBuilder("SELECT");
			queryBuilder.Append(" A.HighSchoolIsInUtah");
			queryBuilder.Append(" ,A.HighSchoolCeebCode");
			queryBuilder.Append(" ,COALESCE(A.HighSchoolCumulativeGpa, 0) AS HighSchoolCumulativeGpa");
			queryBuilder.Append(" ,COALESCE(B.Description, '') AS College");
			queryBuilder.Append(" ,A.JuniorHighCeebCode");
			queryBuilder.Append(" ,COALESCE(A.HaveAttendedOther, 'FALSE') AS HaveAttendedOther");
			queryBuilder.Append(" ,COALESCE(A.GradeLevel, 12) AS GradeLevel");
			queryBuilder.Append(" FROM ScholarshipApplication A");
			queryBuilder.Append(" INNER JOIN CollegeLookup B");
			queryBuilder.Append(" ON A.CollegeCode = B.Code");
			queryBuilder.AppendFormat(" WHERE A.Username = '{0}'", username);
			return _webDatabase.ExecuteQuery<ScholarshipApplication>(queryBuilder.ToString()).Single();
		}

		public Student GetStudent(string username)
		{
			StringBuilder queryBuilder = new StringBuilder("SELECT");
			queryBuilder.Append(" COALESCE(A.FirstName, '') AS FirstName");
			queryBuilder.Append(" ,COALESCE(A.MiddleName, '') AS MiddleName");
			queryBuilder.Append(" ,COALESCE(A.LastName, '') AS LastName");
			queryBuilder.Append(" ,A.DateOfBirth");
			queryBuilder.Append(" ,COALESCE(B.Description, '') AS Gender");
			queryBuilder.Append(" ,COALESCE(C.Description, '') AS Ethnicity");
			queryBuilder.Append(" ,A.StateStudentId");
			queryBuilder.Append(" ,COALESCE(A.HasCriminalRecord, 'FALSE') AS HasCriminalRecord");
			queryBuilder.Append(" ,COALESCE(A.IsUsCitizen, 'TRUE') AS IsUsCitizen");
			queryBuilder.Append(" ,COALESCE(A.IsEligibleForFederalAid, 'TRUE') AS IsEligibleForFederalAid");
			queryBuilder.Append(" ,COALESCE(A.IntendsToApplyForFederalAid, 'TRUE') AS IntendsToApplyForFederalAid");
			queryBuilder.Append(" ,COALESCE(D.Description, '') AS HowHearAbout");
			queryBuilder.Append(" ,E.EmailAddress AS Email");
			queryBuilder.Append(" ,COALESCE(A.Phone, '') AS Phone");
			queryBuilder.Append(" ,COALESCE(A.UESP, 'FALSE') AS UESP");
			queryBuilder.Append(" FROM Student A");
			queryBuilder.Append(" INNER JOIN GenderLookup B");
			queryBuilder.Append(" ON A.GenderCode = B.Code");
			queryBuilder.Append(" INNER JOIN EthnicityLookup C");
			queryBuilder.Append(" ON A.EthnicityCode = C.Code");
			queryBuilder.Append(" INNER JOIN HowHearAboutLookup D");
			queryBuilder.Append(" ON A.HowHearAboutCode = D.Code");
			queryBuilder.Append(" INNER JOIN UserInfo E");
			queryBuilder.Append(" ON A.Username = E.Username");
			queryBuilder.AppendFormat(" WHERE A.Username = '{0}'", username);
			return _webDatabase.ExecuteQuery<Student>(queryBuilder.ToString()).Single();
		}

		public StudentAddress GetAddress(string username)
		{
			StringBuilder queryBuilder = new StringBuilder("SELECT");
			queryBuilder.Append(" COALESCE(A.Address1, '') AS Address1");
			queryBuilder.Append(" ,COALESCE(A.Address2, '') AS Address2");
			queryBuilder.Append(" ,COALESCE(A.City, '') AS City");
			queryBuilder.Append(" ,B.Abbreviation AS State");
			queryBuilder.Append(" ,COALESCE(A.Zip, '') AS Zip");
			queryBuilder.Append(" ,COALESCE(A.Country, '') AS Country");
			queryBuilder.Append(" ,COALESCE(A.IsValid, 'FALSE') AS IsValid");
			queryBuilder.Append(" FROM StudentAddress A");
			queryBuilder.Append(" INNER JOIN StateLookup B");
			queryBuilder.Append(" ON A.StateCode = B.Code");
			queryBuilder.AppendFormat(" WHERE A.Username = '{0}'", username);
			return _webDatabase.ExecuteQuery<StudentAddress>(queryBuilder.ToString()).Single();
		}

		public IEnumerable<string> GetDefaultClassTitles(string ceebCode)
		{
			StringBuilder queryBuilder = new StringBuilder("SELECT Description");
			queryBuilder.Append(" FROM ClassTitleLookup");
			queryBuilder.Append(" WHERE IsInApprovedList = 1");
			queryBuilder.AppendFormat(" AND (ConditionalSchoolCode IS NULL OR ConditionalSchoolCode = '{0}')", ceebCode);
			return _webDatabase.ExecuteQuery<string>(queryBuilder.ToString());
		}

		public string GetSchoolName(string ceebCode)
		{
			string query = string.Format("SELECT Name FROM SchoolLookup WHERE CeebCode = '{0}'", ceebCode);
			return _webDatabase.ExecuteQuery<string>(query).SingleOrDefault() ?? "";
		}//GetSchoolName()

		public void MarkApplicantAsDownloaded(string username)
		{
			string command = string.Format("UPDATE Document SET HasDownloaded = 1 WHERE Username = '{0}'", username);
			_webDatabase.ExecuteCommand(command);
		}//MarkApplicantAsDownloaded()
	}//class
}//namespace
