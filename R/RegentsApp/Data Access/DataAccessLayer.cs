using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace RegentsApp
{
    public class DataAccessLayer
    {
        SqlConnection conn;
        DataContext dc;

        public DataAccessLayer()
        {
            if (Properties.Settings.Default.TestMode)
                conn = new SqlConnection(Properties.Resources.ConnStringTest);
            else
                conn = new SqlConnection(Properties.Resources.ConnStringLive);
            dc = new DataContext(conn);
        }

        #region Create User / Login

        /// <summary>
        /// Creates a new user by storing all data input from the frmSetup.aspx page
        /// </summary>
        /// <param name="userName">Primary Key</param>
        /// <param name="password">User will set their password</param>
        /// <param name="email">Users E-mail</param>
        /// <param name="question1"></param>
        /// <param name="question2"></param>
        /// <param name="answer1"></param>
        /// <param name="answer2"></param>
        /// <param name="reset"></param>
        /// <returns>Boolean value as True if the data was stored in the database</returns>
        public bool CreateUser(string userName, string password, string email, int question1, int question2,
            string answer1, string answer2, bool reset, string firstName, string middleName, string lastName, DateTime dateOfBirth)
        {
            try
            {
                object[] parameters = { userName.ToUpper(), new StringEncryption().Encrypt(password), email, question1, question2, answer1, answer2, reset, firstName, middleName, lastName, dateOfBirth };
                string temp = string.Format("Execute sp_CreateAccount @Username = '{0}', @PasswordHash = '{1}', @EmailAddress = '{2}', @SecurityCode1 = {3}, @SecurityCode2 = {4}, @Answer1 = '{5}', @Answer2 = '{6}', @MustReset = {7}, @FirstName = '{8}', @MiddleName = '{9}', @LastName = '{10}', @DateOfBirth = '{11}'", parameters);
                dc.ExecuteCommand(temp);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the username and password from UserInfo table of database for the frmLogin.aspx page
        /// </summary>
        /// <param name="userName">Primary Key, used in where statement</param>
        /// <param name="password"></param>
        /// <returns>Returns single row of data found in the database</returns>
        public UserLogin UserLogin(string userName, string password)
        {
            string sqlStr = @"SELECT Username, PasswordHash FROM UserInfo WHERE Username = '" + userName.ToUpper() + "'";
            try
            {
                UserLogin ul = dc.ExecuteQuery<UserLogin>(sqlStr).Single();
                return new UserLogin { UserName = ul.UserName, PasswordHash = new StringEncryption().Decrypt(ul.PasswordHash) };
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Pulls username from UserInfo table of database for frmSetup.aspx page
        /// </summary>
        /// <param name="userName">Primary Key, used in where statement</param>
        /// <returns>Returns single row of data found</returns>
        public CheckUserName CheckName(string userName)
        {
            string sqlStr = @"SELECT Username from UserInfo WHERE Username = '" + userName.ToUpper() + "'";
            try
            {
                return dc.ExecuteQuery<CheckUserName>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public CheckUserNameForgot CheckForgotUserName(string userName)
        {
            string sqlStr = @"SELECT Username, SecurityCode1, SecurityCode2, Answer1, Answer2 from UserInfo WHERE Username = '" + userName.ToUpper() + "'";
            try
            {
                return dc.ExecuteQuery<CheckUserNameForgot>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the input email address to see if it has been used before
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns CheckEmail object containing username for the email used, or null if email has not been used</returns>
        public CheckEmail CheckEmail(string email)
        {
            string sqlStr = @"SELECT EmailAddress from UserInfo WHERE EmailAddress = '" + email + "'";
            try
            {
                return dc.ExecuteQuery<CheckEmail>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public CheckUserNameByEmail CheckUserNameByEmail(string email)
        {
            string sqlStr = @"SELECT Username, SecurityCode1, SecurityCode2, Answer1, Answer2 from UserInfo WHERE EmailAddress = '" + email + "'";
            try
            {
                return dc.ExecuteQuery<CheckUserNameByEmail>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the security questions assigned to the username
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Returns GetQuestions object containing the username and questions for the user</returns>
        public GetQuestions GetQuestions(int code)
        {
            string sqlStr = @"SELECT Description from SecurityLookup WHERE Code = " + code;
            try
            {
                return dc.ExecuteQuery<GetQuestions>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the document table to see if the application has been submitted
        /// </summary>
        /// <param name="username"></param>
        /// <returns>HasBeenSubmitted object containing username and date of submission</returns>
        public HasBeenSubmitted Submitted(string username)
        {
            string sqlStr = @"SELECT Username, SubmitDate from Document WHERE Username = '" + username.ToUpper() + "'";
            try
            {
                return dc.ExecuteQuery<HasBeenSubmitted>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates the password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Returns true if password is updated</returns>
        public bool UpdatePassword(string userName, string password)
        {
            try
            {
                object[] parameters = { userName.ToUpper(), new StringEncryption().Encrypt(password) };
                string temp = string.Format("Execute sp_UpdatePassword @Username = '{0}', @PasswordHash = '{1}'", parameters);
                dc.ExecuteCommand(temp);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public string GetFirstName(string userName)
        {
            string sqlStr = @"SELECT Firstname FROM Student WHERE Username = '" + userName.ToUpper() + "'";
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the time the user logins in
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Returns true if the login time is set in the database</returns>
        public bool LoginTime(string userName, bool created)
        {
            try
            {
                string sqlStr = string.Format("Execute sp_LoginTime @Username = '{0}', @AccountCreated = '{1}'", userName.ToUpper(), created);
                dc.ExecuteCommand(sqlStr);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the time the user logs out, unless browser is closed
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Returns true if time is set in database</returns>
        public bool LogoutTime(string userName)
        {
            try
            {
                string sqlStr = string.Format("Execute sp_LogoutTime @Username = '{0}'", userName.ToUpper());
                dc.ExecuteCommand(sqlStr);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Personal Information

        /// <summary>
        /// Adds new student to Student table of database from data input from frmPersonalInfo.aspx page
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <param name="dob"></param>
        /// <param name="gender"></param>
        /// <param name="ethnic"></param>
        /// <param name="ssid"></param>
        /// <param name="criminalRecord"></param>
        /// <param name="citizen"></param>
        /// <param name="eligible"></param>
        /// <param name="finAid"></param>
        /// <param name="howHear"></param>
        /// <param name="phone"></param>
        /// <param name="userName">Primary Key</param>
        /// <returns>Boolean value set to True if data was input into Student table</returns>
        public bool InsertStudent(string firstName, string middleName, string lastName, string dob,
            int gender, int ethnic, string ssid, Nullable<bool> criminalRecord, Nullable<bool> citizen,
            Nullable<bool> eligible, Nullable<bool> finAid, int howHear, string phone, string userName,
            Nullable<bool> _uesp)
        {
            try
            {
                string cr = "Null";
                string cit = "Null";
                string elig = "Null";
                string fin = "Null";
                string uesp = "Null";

                if (dob == null)
                    dob = "Null";
                else
                    dob = string.Format("'{0}'", dob);
                if (gender == 0)
                    gender = 1;
                if (criminalRecord.HasValue)
                    cr = criminalRecord.Value.ToString();
                if (citizen.HasValue)
                    cit = citizen.Value.ToString();
                if (eligible.HasValue)
                    elig = eligible.Value.ToString();
                if (finAid.HasValue)
                    fin = finAid.Value.ToString();
                if (_uesp.HasValue)
                    uesp = _uesp.Value.ToString();

                object[] parameters = { firstName, middleName, lastName, dob, gender, ethnic, ssid, cr, cit, elig, fin, howHear, phone, userName.ToUpper(), uesp };
                string sqlStr = string.Format("Execute sp_InsertStudent @FirstName = '{0}', @MiddleName = '{1}', @LastName = '{2}', @DateOfBirth = {3}, @GenderCode = {4}, @EthnicityCode = {5}, @StateStudentId = '{6}', @HasCriminalRecord = {7}, @IsUsCitizen = {8}, @IsEligibleForFederalAid = {9}, @IntendsToApplyForFederalAid = {10}, @HowHearAboutCode = {11}, @Phone = '{12}', @Username = '{13}', @UESP = {14}", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds new student address info into StudentAddress table with data input in frmPersonalInfo.aspx page
        /// </summary>
        /// <param name="userName">Primary Key</param>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <returns>Boolean value set to True if data was input into StudentAddress table</returns>
        public bool InsertStudentAddress(string userName, string address1, string address2,
            string city, int state, string zip)
        {
            try
            {
                object[] parameters = { address1, address2, city, state, zip, userName.ToUpper() };
                string sqlStr = string.Format("Execute sp_InsertStudentAddress @Address1 = '{0}', @Address2 = '{1}', @City = '{2}', @StateCode = {3}, @Zip = '{4}', @Username = '{5}'", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates student data to Student table with data input from frmPersonalInfo.aspx
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <param name="dob"></param>
        /// <param name="gender"></param>
        /// <param name="ethnic"></param>
        /// <param name="ssid"></param>
        /// <param name="criminalRecord"></param>
        /// <param name="citizen"></param>
        /// <param name="eligible"></param>
        /// <param name="finAid"></param>
        /// <param name="howHear"></param>
        /// <param name="phone"></param>
        /// <param name="userName">Primary Key, used in where statement</param>
        /// <returns>Boolean value to True is data was input into Student table</returns>
        public bool UpdateStudent(string firstName, string middleName, string lastName, string dob,
            int gender, int ethnic, string ssid, Nullable<bool> criminalRecord, Nullable<bool> citizen,
            Nullable<bool> eligible, Nullable<bool> finAid, int howHear, string phone, string userName,
            Nullable<bool> _uesp)
        {
            try
            {
                string cr = "Null";
                string cit = "Null";
                string elig = "Null";
                string fin = "Null";
                string uesp = "Null";

                if (dob == null)
                    dob = "Null";
                else
                    dob = string.Format("'{0}'", dob);

                if (criminalRecord.HasValue)
                    cr = criminalRecord.Value.ToString();
                if (citizen.HasValue)
                    cit = citizen.Value.ToString();
                if (eligible.HasValue)
                    elig = eligible.Value.ToString();
                if (finAid.HasValue)
                    fin = finAid.Value.ToString();
                if (_uesp.HasValue)
                    uesp = _uesp.Value.ToString();

                object[] parameters = { firstName, middleName, lastName, dob, gender, ethnic, ssid, cr, cit, elig, fin, howHear, phone, userName.ToUpper(), uesp };
                string sqlStr = string.Format("Execute sp_UpdateStudent @FirstName = '{0}', @MiddleName = '{1}', @LastName = '{2}', @DateOfBirth = {3}, @GenderCode = {4}, @EthnicityCode = {5}, @StateStudentId = '{6}', @HasCriminalRecord = {7}, @IsUsCitizen = {8}, @IsEligibleForFederalAid = {9}, @IntendsToApplyForFederalAid = {10}, @HowHearAboutCode = {11}, @Phone = '{12}', @Username = '{13}', @UESP = {14}", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates StudentAddress table with data input from frmPersonalInfo.aspx page
        /// </summary>
        /// <param name="userName">Primary Key, used in where statement</param>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <returns>Boolean value set to True if data was input into StudentAddress table</returns>
        public bool UpdateStudentAddress(string userName, string address1, string address2,
            string city, int state, string zip)
        {
            try
            {
                object[] parameters = { address1, address2, city, state, zip, userName.ToUpper() };
                string sqlStr = string.Format("Execute sp_UpdateStudentAddress @Address1 = '{0}', @Address2 = '{1}', @City = '{2}', @StateCode = {3}, @Zip = '{4}', @Username = '{5}'", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the student info from Student table of database to display on frmPersonalInfo.aspx page
        /// </summary>
        /// <param name="userName">Used in where statement to find student by primary key</param>
        /// <returns>Single row instance of all the student data</returns>
        public StudentData GetStudent(string userName)
        {
            string sqlStr = @"SELECT FirstName, MiddleName, LastName, DateOfBirth, GenderCode, EthnicityCode, StateStudentId, HasCriminalRecord, IsUsCitizen, IsEligibleForFederalAid, IntendsToApplyForFederalAid, HowHearAboutCode, Phone, UESP FROM Student WHERE Username = '" + userName.ToUpper() + "'";
            try
            {
                return dc.ExecuteQuery<StudentData>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public StudentData GetNewStudentData(string userName)
        {
            string selectStr = @"SELECT FirstName, MiddleName, LastName, DateOfBirth FROM Student WHERE UserName = '" + userName + "'";
            try
            {
                return dc.ExecuteQuery<StudentData>(selectStr).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the student address info from database to display on frmPersonalInfo.aspx page
        /// </summary>
        /// <param name="userName">Used in where statement to find student address by primary key</param>
        /// <returns>Single row instance of all the student address data</returns>
        public StudentAddress GetStudentAddress(string userName)
        {
            string sqlStr = "SELECT Address1, Address2, City, StateCode, Zip from StudentAddress WHERE Username = '" + userName.ToUpper() + "'";
            try
            {
                return dc.ExecuteQuery<StudentAddress>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (InvalidCastException ex)
            {
                string temp = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Adds a new Ethnicity to lookup table if user chooses other and enters their own
        /// </summary>
        /// <param name="ethnic"></param>
        /// <returns>Boolean value set to True if data was input into EthnicityLookup table</returns>
        public bool InsertEthnicityLookup(string description)
        {
            try
            {
                object parameters = description;
                string sqlStr = string.Format("Execute sp_InsertEthnicity @Description = '{0}'", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Inserts a new entry into Howhearabout table if user chooses 'other'
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Boolean value set to True is the data is entered into the HowHearAboutLookup table</returns>
        public bool InsertHowHear(string description)
        {
            try
            {
                object parameters = description;
                string sqlStr = string.Format("Execute sp_InsertHowHear @Description = '{0}'", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns an int for the position of the new ethnicity input into the EthnicityLookup table
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public int GetNewEthnicity(string description)
        {
            string sqlStr = @"Select Code from EthnicityLookup where Description = '" + description + "'";
            try
            {
                return dc.ExecuteQuery<int>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return 0;
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns an int for the position of the new howhearabout input into the HowHearAboutLookup table
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public int GetNewHowHear(string description)
        {
            string sqlStr = @"Select Code from HowHearAboutLookup where Description = '" + description + "'";
            try
            {
                return dc.ExecuteQuery<int>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return 0;
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        public string GetHowHearDescription(int code)
        {
            string sqlStr = @"SELECT Description FROM HowHearAboutLookup WHERE Code = " + code;
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool GetHowHearBoolean(int code)
        {
            string sqlStr = @"SELECT IsInDefault from HowHearAboutLookup WHERE Code = " + code;
            try
            {
                return dc.ExecuteQuery<bool>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public int GetHowHear(string description)
        {
            string sqlStr = @"SELECT Code from HowHearAboutLookup WHERE Description = '" + description + "' AND IsInDefault = 'True'";
            try
            {
                return dc.ExecuteQuery<int>(sqlStr).Single();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string GetEthnicityDescription(int code)
        {
            string sqlStr = @"SELECT Description from EthnicityLookup WHERE Code = " + code;
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool GetEthnicityBoolean(int code)
        {
            string sqlStr = @"SELECT IsInDefaultList from EthnicityLookup WHERE Code = " + code;
            try
            {
                return dc.ExecuteQuery<bool>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public int GetEthnicityCode()
        {
            string sqlStr = @"SELECT Code from EthnicityLookup WHERE Description = 'Other' AND IsInDefaultList = 'True'";
            try
            {
                return dc.ExecuteQuery<int>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return 0;
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        public string GetSSID(string username, string ssid)
        {
            string sqlStr = @"SELECT StateStudentID FROM Student where Username = '" + username.ToUpper() + "' AND StateStudentID = '" + ssid + "'";
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public string GetSSIDOnly(string ssid)
        {
            string sqlStr = @"SELECT StateStudentID FROM Student WHERE StateStudentID = '" + ssid + "'";
            try
            {
                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                conn.Open();
                dr = cmd.ExecuteReader();
                int counter = 0;
                while (dr.Read())
                {
                    counter++;
                }
                dr.Close();
                conn.Close();
                if (counter > 1)
                {
                    return null;
                }
                else
                {
                    return dc.ExecuteQuery<string>(sqlStr).Single();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetEmail(string userName)
        {
            string sqlStr = @"SELECT EmailAddress FROM UserInfo WHERE Username = '" + userName.ToUpper() + "'";
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        #endregion

        #region School Information

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="isInUtah"></param>
        /// <param name="highSchoolCeeb"></param>
        /// <param name="gradDate"></param>
        /// <param name="gpa"></param>
        /// <param name="collegeCode"></param>
        /// <param name="jrCeebCode"></param>
        /// <param name="attendedOther"></param>
        /// <returns></returns>
        public bool InsertSchoolInfo(string userName, Nullable<bool> isInUtah, string highSchoolCeeb,
             int gradeLevel, Nullable<bool> gradYear, float gpa, int collegeCode, string jrCeebCode, Nullable<bool> attendedOther)
        {
            try
            {
                string inUtah = "Null";
                string attended = "Null";
                string _graduationYear = "Null";

                if (isInUtah.HasValue)
                { inUtah = isInUtah.Value.ToString(); }
                if (attendedOther.HasValue)
                { attended = attendedOther.Value.ToString(); }
                if (gradYear.HasValue)
                { _graduationYear = gradYear.Value.ToString(); }
                if (highSchoolCeeb == "")
                { highSchoolCeeb = "00-002"; }
                if (jrCeebCode == "")
                { jrCeebCode = "00-000"; }

                object[] parameters = { userName.ToUpper(), inUtah, highSchoolCeeb, _graduationYear, gradeLevel, gpa, collegeCode, jrCeebCode, attended };
                string sqlStr = string.Format("Execute sp_InsertScholarshipApplication @Username = '{0}', @HighSchoolIsInUtah = {1}, @HighSchoolCeebCode = '{2}', @GraduationYear = {3}, @GradeLevel = {4}, @HighSchoolCumulativeGpa = {5}, @CollegeCode = {6}, @JuniorHighCeebCode = '{7}', @HaveAttendedOther = {8}", parameters);
                dc.ExecuteCommand(sqlStr);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates ScholarshipInformation table with data input from frmSchoolInfo.aspx page
        /// </summary>
        /// <param name="userName">Primary Key, used in where statement</param>
        /// <param name="isInUtah"></param>
        /// <param name="highSchoolCeeb"></param>
        /// <param name="gradDate"></param>
        /// <param name="gpa"></param>
        /// <param name="collegeCode"></param>
        /// <param name="jrCeebCode"></param>
        /// <param name="attendedOther"></param>
        /// <returns>Boolean value set to True if data was input into ScholarshipInformation table</returns>
        public bool UpdateSchoolInfo(string userName, Nullable<bool> isInUtah, string highSchoolCeeb, int gradeLevel,
            Nullable<bool> gradYear, float gpa, int collegeCode, string jrCeebCode, Nullable<bool> attendedOther)
        {
            try
            {
                string inUtah = "Null";
                string attended = "Null";
                string _graduationYear = "Null";

                if (isInUtah.HasValue)
                { inUtah = isInUtah.Value.ToString(); }
                if (attendedOther.HasValue)
                { attended = attendedOther.Value.ToString(); }
                if (gradYear.HasValue)
                { _graduationYear = gradYear.Value.ToString(); }

                object[] parameters = { userName.ToUpper(), inUtah, highSchoolCeeb, _graduationYear, gpa, collegeCode, jrCeebCode, attended, gradeLevel };
                string sqlStr = string.Format("Execute sp_UpdateScholarshipApplication @Username = '{0}', @HighSchoolIsInUtah = {1}, @HighSchoolCeebCode = '{2}', @GraduationYear = {3}, @HighSchoolCumulativeGpa = {4}, @CollegeCode = {5}, @JuniorHighCeebCode = '{6}', @HaveAttendedOther = {7}, @GradeLevel = {8}", parameters);
                dc.ExecuteCommand(sqlStr);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="typeCode"></param>
        /// <param name="scoreCode"></param>
        /// <returns></returns>
        public bool InsertACT(string userName, int typeCode, int scoreCode)
        {
            try
            {
                object[] parameters = { userName.ToUpper(), typeCode, scoreCode };
                string sqlStr = string.Format("Execute sp_InsertAct @Username = '{0}', @TypeCode = {1}, @ScoreCode = {2}", parameters);
                dc.ExecuteCommand(sqlStr);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates Act table with data input from frmSchoolInfo.aspx page
        /// </summary>
        /// <param name="userName">Primary Key, used in where statement</param>
        /// <param name="typeCode"></param>
        /// <param name="scoreCode"></param>
        /// <returns>Boolean value set to True if data was input into Act table</returns>
        public bool UpdateACT(string userName, int typeCode, int scoreCode)
        {
            try
            {
                object[] parameters = { userName.ToUpper(), typeCode, scoreCode };
                string sqlStr = string.Format("Execute sp_UpdateAct @Username = '{0}', @TypeCode = {1}, @ScoreCode = {2}", parameters);
                dc.ExecuteCommand(sqlStr);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the student school info from Scholarship table of database to display on frmHighSchoolInfo.aspx page
        /// </summary>
        /// <param name="userName">Used in where statement to find student by primary key</param>
        /// <returns>Single row intance of all school information data</returns>
        public SchoolInfo GetSchoolInfo(string userName)
        {
            string sqlStr = @"SELECT * FROM ScholarshipApplication WHERE Username = '" + userName.ToUpper() + "'";
            try
            {
                return dc.ExecuteQuery<SchoolInfo>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets student Composite ACT info from ACT tabel of database to display on frmHighSchoolInfo.aspx page
        /// </summary>
        /// <param name="userName">Used in where statement to find student by primary key and english type code</param>
        /// <returns>Single row instance of all ACT information</returns>
        public CompositeACT GetCompositeACT(string userName)
        {
            string sqlStr = @"SELECT * FROM ActScoreLookup A JOIN Act B ON A.Code = B.ScoreCode WHERE B.Username = '" + userName.ToUpper() + "' AND B.TypeCode = 1";
            try
            {
                return dc.ExecuteQuery<CompositeACT>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets student English ACT info from ACT tabel of database to display on frmHighSchoolInfo.aspx page
        /// </summary>
        /// <param name="userName">Used in where statement to find student by primary key and english type code</param>
        /// <returns>Single row instance of all ACT information</returns>
        public EnglishACT GetEnglishACT(string userName)
        {
            string sqlStr = @"SELECT * FROM ActScoreLookup A JOIN Act B ON A.Code = B.ScoreCode WHERE B.Username = '" + userName.ToUpper() + "' AND B.TypeCode = 2";
            try
            {
                return dc.ExecuteQuery<EnglishACT>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets student Math ACT info from ACT tabel of database to display on frmHighSchoolInfo.aspx page
        /// </summary>
        /// <param name="userName">Used in where statement to find student by primary key and math type code</param>
        /// <returns>Single row instance of all ACT information</returns>
        public MathACT GetMathACT(string userName)
        {
            string sqlStr = @"SELECT * FROM ActScoreLookup A JOIN Act B ON A.Code = B.ScoreCode WHERE B.Username = '" + userName.ToUpper() + "' AND B.TypeCode = 3";
            try
            {
                return dc.ExecuteQuery<MathACT>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets student Science ACT info from ACT tabel of database to display on frmHighSchoolInfo.aspx page
        /// </summary>
        /// <param name="userName">Used in where statement to find student by primary key and science type code</param>
        /// <returns>Single row instance of all ACT information</returns>
        public ScienceACT GetScienceACT(string userName)
        {
            string sqlStr = @"SELECT * FROM ActScoreLookup A JOIN Act B ON A.Code = B.ScoreCode WHERE B.Username = '" + userName.ToUpper() + "' AND B.TypeCode = 4";
            try
            {
                return dc.ExecuteQuery<ScienceACT>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets student Reading ACT info from ACT tabel of database to display on frmHighSchoolInfo.aspx page
        /// </summary>
        /// <param name="userName">Used in where statement to find student by primary key and Reading type code</param>
        /// <returns>Single row instance of all ACT information</returns>
        public ReadingACT GetReadingACT(string userName)
        {
            string sqlStr = @"SELECT * FROM ActScoreLookup A JOIN Act B ON A.Code = B.ScoreCode WHERE B.Username = '" + userName.ToUpper() + "' AND B.TypeCode = 5";
            try
            {
                return dc.ExecuteQuery<ReadingACT>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        internal List<SchoolNames> GetSchoolNames(string type)
        {
            string selectStr = "SELECT CeebCode, Name FROM SchoolLookup WHERE Type = '" + type + "'";
            return dc.ExecuteQuery<SchoolNames>(selectStr).ToList();
        }

        #endregion

        #region Class Information

        public bool InsertClass(string username, int type, int seqNo, int title, string grade, string weight, double credits, int collegeCode)
        {
            try
            {
                object[] parameters = { username.ToUpper(), type, seqNo, title, grade, weight, credits, collegeCode };
                string sqlStr = string.Format("Execute sp_InsertClass @Username = '{0}', @ClassTypeCode = {1}, @SequenceNo = {2}, @ClassTitleCode = {3}, @GradeLevel = '{4}', @WeightCode = '{5}', @Credits = {6}, @CollegeCode = {7}", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool UpdateClass(string username, int type, int seqNo, int title, string grade, string weight, double credits, int collegeCode)
        {
            try
            {
                object[] parameters = { username.ToUpper(), type, seqNo, title, grade, weight, credits, collegeCode };
                string sqlStr = string.Format("Execute sp_UpdateClass @Username = '{0}', @ClassTypeCode = {1}, @SequenceNo = {2}, @ClassTitleCode = {3}, @GradeLevel = '{4}', @WeightCode = '{5}', @Credits = {6}, @CollegeCode = {7}", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool InsertGrade(string userName, int ClassTypeCode, int ClassSequenceNo, int term, int gradeCode)
        {
            if (gradeCode != 0)
            {
                try
                {
                    try
                    {
                        object[] parameters = { userName.ToUpper(), ClassTypeCode, ClassSequenceNo, term, gradeCode };
                        string sqlStr = string.Format("Execute sp_InsertGrade @Username = '{0}', @ClassTypeCode = {1}, @ClassSequenceNo = {2}, @Term = {3}, @GradeCode = {4}", parameters);
                        return dc.ExecuteCommand(sqlStr) == 1;
                    }
                    catch (SqlException)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return true;
        }

        public bool UpdateGrade(string userName, int ClassTypeCode, int ClassSequenceNo, int term, int gradeCode)
        {
            if (gradeCode != 0)
            {
                try
                {
                    object[] parameters = { userName.ToUpper(), ClassTypeCode, ClassSequenceNo, term, gradeCode };
                    string sqlStr = string.Format("Execute sp_UpdateGrade @Username = '{0}', @ClassTypeCode = {1}, @ClassSequenceNo = {2}, @Term = {3}, @GradeCode = {4}", parameters);
                    return dc.ExecuteCommand(sqlStr) == 1;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
            else
                return true;
        }

        public bool DeleteClass(string userName, int ClassTypeCode, int SequenceNo)
        {
            try
            {
                object[] parameters = { userName.ToUpper(), ClassTypeCode, SequenceNo };
                string sqlStr = string.Format("Execute sp_DeleteClass @Username = '{0}', @ClassTypeCode = {1}, @SequenceNo = {2}", parameters);
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteGrade(string userName, int ClassTypeCode, int ClassSequenceNo, int term)
        {
            try
            {
                object[] parameters = { userName.ToUpper(), ClassTypeCode, ClassSequenceNo, term };
                string sqlStr = string.Format("Execute sp_DeleteGrades @Username = '{0}', @ClassTypeCode = {1}, @ClassSequenceNo = {2}, @Term = {3}", parameters);
                int deleted = dc.ExecuteCommand(sqlStr);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAllGrades(string userName, int ClassTypeCode, int ClassSequenceNo)
        {
            try
            {
                string sqlStr = "DELETE FROM Grade WHERE Username = '" + userName + "' AND ClassTypeCode = '" + ClassTypeCode + "' AND ClassSequenceNo = '" + ClassSequenceNo + "'";
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string ClassExists(string userName, int ClassTypeCode, int SequenceNo)
        {
            string sqlStr = "SELECT Username from ClassList WHERE Username = '" + userName.ToUpper() + "' AND ClassTypeCode = " + ClassTypeCode + " AND SequenceNo = " + SequenceNo;
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetGradeName(int code)
        {
            string sqlStr = @"SELECT [Description] FROM GradeLookup WHERE Code = " + code;
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool InsertClassName(string desc, int typeCode, string weightCode)
        {
            try
            {
                string sqlStr;
                if (weightCode == null)
                {
                    object[] parameters = { desc, typeCode };
                    sqlStr = string.Format("Execute sp_InsertClassName @Description = '{0}', @ClassTypeCode = {1}", parameters);
                }
                else
                {
                    object[] parameters = { desc, typeCode, weightCode };
                    sqlStr = string.Format("Execute sp_InsertClassName @Description = '{0}', @ClassTypeCode = {1}, @WeightCode = '{2}'", parameters);
                }
                return dc.ExecuteCommand(sqlStr) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetClassName(string description)
        {
            string sqlStr = @"SELECT Code from ClassTitleLookup WHERE Description = '" + description + "'";
            try
            {
                return dc.ExecuteQuery<int>(sqlStr).Single();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int CheckClassNameByCode(string description)
        {
            string sqlStr = @"SELECT ClassTypeCode from ClassTitleLookup WHERE Description = '" + description + "' AND IsInApprovedList = 'True'";
            try
            {
                return dc.ExecuteQuery<int>(sqlStr).Single();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string GetClassWeight(string description)
        {
            string sqlStr = @"SELECT WeightCode from ClassTitleLookup WHERE Description = '" + description + "'";
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public bool GetClassBoolean(int code)
        {
            string sqlStr = @"SELECT IsInApprovedList from ClassTitleLookup WHERE Code = " + code;
            try
            {
                return dc.ExecuteQuery<bool>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public int GetClassCode(int typeCode, string weightCode)
        {
            string weightFormat = string.Format(weightCode == null ? "WeightCode is null" : string.Format("WeightCode = '{0}'", weightCode));
            string sqlStr = string.Format(@"SELECT Code from ClassTitleLookup WHERE [Description] = 'Other' AND IsInApprovedList = 'True' AND ClassTypeCode = '" + typeCode + "' AND {0}", weightFormat);
            try
            {
                return dc.ExecuteQuery<int>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return 0;
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        public string GetClassDescription(int code)
        {
            string sqlStr = @"SELECT Description from ClassTitleLookup where Code = " + code;
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ListItem[] GetClassList(string sql)
        {
            string sqlStr = sql;
            try
            {
                ClassList[] standard = dc.ExecuteQuery<ClassList>(sqlStr).ToArray();
                ListItem[] standardArray = new ListItem[standard.Length];
                for (int i = 0; i < standard.Length; i++)
                {
                    ListItem li = new ListItem();
                    li.Value = standard[i].Code.ToString();
                    li.Text = standard[i].Description;
                    standardArray[i] = li;
                }
                return standardArray;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public double GetCreditsTotal(string userName, int classTypeCode)
        {
            string sqlStr = "SELECT SUM(Credits) from ClassList WHERE Username = '" + userName + "' AND ClassTypeCode = " + classTypeCode;
            try
            {
                return dc.ExecuteQuery<Double>(sqlStr).Single();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string GetWeightCode(int code)
        {
            string sqlStr = @"SELECT WeightCode FROM ClassTitleLookup WHERE Code = " + code;
            try
            {
                return dc.ExecuteQuery<string>(sqlStr).Single();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public List<ClassList> GetScienceClassList(int classTypeCode, bool isInApprovedList, string weightCode, string subject)
        {
            string selectStr = string.Empty;
            if (weightCode == string.Empty || weightCode == "  ")
            {
                selectStr = string.Format(@"SELECT Code, Description FROM ClassTitleLookup WHERE ClassTypeCode = {0} AND IsInApprovedList = '{1}' AND WeightCode IS NULL AND Description like '%{2}%'", classTypeCode, isInApprovedList, subject);
            }
            else if (subject == string.Empty)
            {
                selectStr = string.Format(@"SELECT Code, Description FROM ClassTitleLookup WHERE ClassTypeCode = {0} AND IsInApprovedList = '{1}' AND WeightCode = '{2}'", classTypeCode, isInApprovedList, weightCode);
            }
            else
            {
                selectStr = string.Format(@"SELECT Code, Description FROM ClassTitleLookup WHERE ClassTypeCode = {0} AND IsInApprovedList = '{1}' AND WeightCode = '{2}' AND Description like '%{3}%'", classTypeCode, isInApprovedList, weightCode, subject);
            }
            try
            {
                return dc.ExecuteQuery<ClassList>(selectStr).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Class Credits

        public ClassData GetClass(string username, int typeCode, int seq)
        {
            string sqlStr = @"SELECT * FROM ClassList WHERE Username = '" + username.ToUpper() + "' AND ClassTypeCode = " + typeCode + " AND SequenceNo = " + seq;
            try
            {
                return dc.ExecuteQuery<ClassData>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public GradeData GetGrade(string userName, int typeCode, int seq, int term)
        {
            string sqlStr = @"SELECT * FROM Grade WHERE Username = '" + userName.ToUpper() + "' AND ClassTypeCode = " + typeCode + " AND ClassSequenceNo = " + seq + " AND Term = " + term;
            try
            {
                return dc.ExecuteQuery<GradeData>(sqlStr).Single();
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        #endregion

        #region Reporting
        /// <summary>
        /// Gets a List of PdfRecord objects from the database for the given applicant.
        /// </summary>
        /// <param name="username">The applicant's user name.</param>
        public List<PdfRecord> GetPdfRecords(string username)
        {
            StringBuilder queryBuilder = new StringBuilder("SELECT ");
            queryBuilder.Append("COALESCE(Student.StateStudentId, '')		    AS StateStudentId, ");
            queryBuilder.Append("Document.SubmitDate                            AS ApplicationSubmitDate, ");
            queryBuilder.Append("COALESCE(Student.FirstName, '')			    AS FirstName, ");
            queryBuilder.Append("COALESCE(Student.MiddleName, '')			    AS MiddleName, ");
            queryBuilder.Append("COALESCE(Student.LastName, '')				    AS LastName, ");
            queryBuilder.Append("COALESCE(Student.DateOfBirth, '1970-01-01')    AS DateOfBirth, ");
            queryBuilder.Append("COALESCE(GenderLookup.Description, '')		    AS Gender, ");
            queryBuilder.Append("COALESCE(EthnicityLookup.Description, '')	    AS Ethnicity, ");
            queryBuilder.Append("Student.HasCriminalRecord               	    AS HasCriminalRecord, ");
            queryBuilder.Append("Student.IsUsCitizen            			    AS IsUsCitizen,");
            queryBuilder.Append("Student.IntendsToApplyForFederalAid            AS IntendsToApplyForFederalAid, ");
            queryBuilder.Append("COALESCE(HowHearAboutLookup.Description, '')   AS HowHearAbout, ");
            queryBuilder.Append("COALESCE(Student.Phone, '')					AS Phone, ");
            queryBuilder.Append("COALESCE(StudentAddress.Address1, '')		    AS Address1, ");
            queryBuilder.Append("COALESCE(StudentAddress.Address2, '')		    AS Address2, ");
            queryBuilder.Append("COALESCE(StudentAddress.City, '')			    AS City, ");
            queryBuilder.Append("COALESCE(StudentStateLookup.Abbreviation, '')  AS [State], ");
            queryBuilder.Append("COALESCE(StudentAddress.Zip, '')			    AS Zip, ");
            queryBuilder.Append("COALESCE(StudentAddress.Country, '')		    AS Country, ");
            queryBuilder.Append("UserInfo.EmailAddress, ");
            queryBuilder.Append("ScholarshipApplication.HighSchoolIsInUtah      AS HighSchoolIsInUtah, ");
            queryBuilder.Append("COALESCE(HighSchool.Name, '')				    AS HighSchoolName, ");
            queryBuilder.Append("COALESCE(HighSchool.District, '')			    AS HighSchoolDistrict, ");
            queryBuilder.Append("COALESCE(HighSchool.City, '')				    AS HighSchoolCity, ");
            queryBuilder.Append("COALESCE(HighSchoolStateLookup.Abbreviation, '') AS HighSchoolState, ");
            queryBuilder.Append("COALESCE(HighSchool.Zip, '')				    AS HighSchoolZip, ");
            queryBuilder.Append("ScholarshipApplication.GraduationYear          AS HighSchoolGraduationYear, ");
            queryBuilder.Append("COALESCE(ScholarshipApplication.HighSchoolCumulativeGpa, 0) AS HighSchoolCumulativeGpa, ");
            queryBuilder.Append("COALESCE(CollegeLookup.Description, '')	    AS College, ");
            queryBuilder.Append("COALESCE(JuniorHigh.Name, '')				    AS JuniorHighName, ");
            queryBuilder.Append("COALESCE(JuniorHigh.District, '')			    AS JuniorHighDistrict, ");
            queryBuilder.Append("COALESCE(JuniorHigh.City, '')				    AS JuniorHighCity, ");
            queryBuilder.Append("COALESCE(JuniorHighStateLookup.Abbreviation, '') AS JuniorHighState, ");
            queryBuilder.Append("COALESCE(JuniorHigh.Zip, '')				    AS JuniorHighZip,");
            queryBuilder.Append("ScholarshipApplication.HaveAttendedOther       AS HasAttendedOtherHighSchool, ");
            queryBuilder.Append("COALESCE(ActCompositeLookup.Description, '')   AS ActCompositeScore, ");
            queryBuilder.Append("COALESCE(ActEnglishLookup.Description, '')	    AS ActEnglishScore, ");
            queryBuilder.Append("COALESCE(ActMathLookup.Description, '')	    AS ActMathScore, ");
            queryBuilder.Append("COALESCE(ActScienceLookup.Description, '')	    AS ActScienceScore, ");
            queryBuilder.Append("COALESCE(ActReadingLookup.Description, '')	    AS ActReadingScore, ");
            queryBuilder.Append("COALESCE(ClassTypeLookup.Description, '')	    AS ClassType, ");
            queryBuilder.Append("ClassList.SequenceNo						    AS ClassNumber, ");
            queryBuilder.Append("COALESCE(ClassTitleLookup.Description, '')	    AS ClassName, ");
            queryBuilder.Append("ClassList.GradeLevel, ");
            queryBuilder.Append("COALESCE(ClassList.WeightCode, '')			    AS ClassWeight, ");
            queryBuilder.Append("COALESCE(ClassList.Credits, 0)				    AS Credits, ");
            queryBuilder.Append("COALESCE(GradeLookup1.Description, '')		    AS Grade1, ");
            queryBuilder.Append("COALESCE(GradeLookup2.Description, '')		    AS Grade2, ");
            queryBuilder.Append("COALESCE(GradeLookup3.Description, '')		    AS Grade3, ");
            queryBuilder.Append("COALESCE(GradeLookup4.Description, '')		    AS Grade4, ");
            queryBuilder.Append("COALESCE(GradeLookup5.Description, '')		    AS Grade5, ");
            queryBuilder.Append("COALESCE(GradeLookup6.Description, '')		    AS Grade6 ");
            queryBuilder.Append("FROM Student ");
            queryBuilder.Append("INNER JOIN Document ");
            queryBuilder.Append("ON Student.Username = Document.Username ");
            queryBuilder.Append("INNER JOIN GenderLookup ");
            queryBuilder.Append("ON Student.GenderCode = GenderLookup.Code ");
            queryBuilder.Append("INNER JOIN EthnicityLookup ");
            queryBuilder.Append("ON Student.EthnicityCode = EthnicityLookup.Code ");
            queryBuilder.Append("INNER JOIN HowHearAboutLookup ");
            queryBuilder.Append("ON Student.HowHearAboutCode = HowHearAboutLookup.Code ");
            queryBuilder.Append("INNER JOIN StudentAddress ");
            queryBuilder.Append("ON Student.Username = StudentAddress.Username ");
            queryBuilder.Append("INNER JOIN StateLookup AS StudentStateLookup ");
            queryBuilder.Append("ON StudentAddress.StateCode = StudentStateLookup.Code ");
            queryBuilder.Append("INNER JOIN UserInfo ");
            queryBuilder.Append("ON Student.Username = UserInfo.Username ");
            queryBuilder.Append("INNER JOIN ScholarshipApplication ");
            queryBuilder.Append("ON Student.Username = ScholarshipApplication.Username ");
            queryBuilder.Append("INNER JOIN SchoolLookup AS HighSchool ");
            queryBuilder.Append("ON ScholarshipApplication.HighSchoolCeebCode = HighSchool.CeebCode ");
            queryBuilder.Append("INNER JOIN StateLookup AS HighSchoolStateLookup ");
            queryBuilder.Append("ON HighSchool.StateCode = HighSchoolStateLookup.Code ");
            queryBuilder.Append("INNER JOIN CollegeLookup ");
            queryBuilder.Append("ON ScholarshipApplication.CollegeCode = CollegeLookup.Code ");
            queryBuilder.Append("INNER JOIN SchoolLookup AS JuniorHigh ");
            queryBuilder.Append("ON ScholarshipApplication.JuniorHighCeebCode = JuniorHigh.CeebCode ");
            queryBuilder.Append("INNER JOIN StateLookup AS JuniorHighStateLookup ");
            queryBuilder.Append("ON JuniorHigh.StateCode = JuniorHighStateLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActComposite ");
            queryBuilder.Append("ON Student.Username = ActComposite.Username ");
            queryBuilder.Append("AND ActComposite.TypeCode = 1 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActCompositeLookup ");
            queryBuilder.Append("ON ActComposite.ScoreCode = ActCompositeLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActEnglish ");
            queryBuilder.Append("ON Student.Username = ActEnglish.Username ");
            queryBuilder.Append("AND ActEnglish.TypeCode = 2 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActEnglishLookup ");
            queryBuilder.Append("ON ActEnglish.ScoreCode = ActEnglishLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActMath ");
            queryBuilder.Append("ON Student.Username = ActMath.Username ");
            queryBuilder.Append("AND ActMath.TypeCode = 3 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActMathLookup ");
            queryBuilder.Append("ON ActMath.ScoreCode = ActMathLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActScience ");
            queryBuilder.Append("ON Student.Username = ActScience.Username ");
            queryBuilder.Append("AND ActScience.TypeCode = 4 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActScienceLookup ");
            queryBuilder.Append("ON ActScience.ScoreCode = ActScienceLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActReading ");
            queryBuilder.Append("ON Student.Username = ActReading.Username ");
            queryBuilder.Append("AND ActReading.TypeCode = 5 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActReadingLookup ");
            queryBuilder.Append("ON ActReading.ScoreCode = ActReadingLookup.Code ");
            queryBuilder.Append("INNER JOIN ClassList ");
            queryBuilder.Append("ON Student.Username = ClassList.Username ");
            queryBuilder.Append("INNER JOIN ClassTypeLookup ");
            queryBuilder.Append("ON ClassList.ClassTypeCode = ClassTypeLookup.Code ");
            queryBuilder.Append("INNER JOIN ClassTitleLookup ");
            queryBuilder.Append("ON ClassList.ClassTitleCode = ClassTitleLookup.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade1 ");
            queryBuilder.Append("ON ClassList.Username = Grade1.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade1.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade1.ClassSequenceNo ");
            queryBuilder.Append("AND Grade1.Term = 1 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup1 ");
            queryBuilder.Append("ON Grade1.GradeCode = GradeLookup1.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade2 ");
            queryBuilder.Append("ON ClassList.Username = Grade2.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade2.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade2.ClassSequenceNo ");
            queryBuilder.Append("AND Grade2.Term = 2 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup2 ");
            queryBuilder.Append("ON Grade2.GradeCode = GradeLookup2.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade3 ");
            queryBuilder.Append("ON ClassList.Username = Grade3.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade3.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade3.ClassSequenceNo ");
            queryBuilder.Append("AND Grade3.Term = 3 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup3 ");
            queryBuilder.Append("ON Grade3.GradeCode = GradeLookup3.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade4 ");
            queryBuilder.Append("ON ClassList.Username = Grade4.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade4.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade4.ClassSequenceNo ");
            queryBuilder.Append("AND Grade4.Term = 4 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup4 ");
            queryBuilder.Append("ON Grade4.GradeCode = GradeLookup4.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade5 ");
            queryBuilder.Append("ON ClassList.Username = Grade5.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade5.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade5.ClassSequenceNo ");
            queryBuilder.Append("AND Grade5.Term = 5 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup5 ");
            queryBuilder.Append("ON Grade5.GradeCode = GradeLookup5.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade6 ");
            queryBuilder.Append("ON ClassList.Username = Grade6.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade6.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade6.ClassSequenceNo ");
            queryBuilder.Append("AND Grade6.Term = 6 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup6 ");
            queryBuilder.Append("ON Grade6.GradeCode = GradeLookup6.Code ");
            queryBuilder.Append(string.Format("WHERE Student.Username = '{0}'", username.ToUpper()));
            try
            {
                return dc.ExecuteQuery<PdfRecord>(queryBuilder.ToString()).ToList();
            }
            catch (Exception ex)
            {
                Exception x = ex.InnerException;
                return null;
            }
        }//GetPdfRecords()

        /// <summary>
        /// Gets a list of AddDataRecords object from the database for the given applicant.
        /// </summary>
        /// <param name="username">The applicant's user name.</param>
        public List<AllDataRecords> GetAllData(string username)
        {
            StringBuilder queryBuilder = new StringBuilder("SELECT ");
            queryBuilder.Append("COALESCE(Student.StateStudentId, '')		    AS StateStudentId, ");
            queryBuilder.Append("Convert(DateTime, '01/01/1900')                AS ApplicationSubmitDate, ");
            queryBuilder.Append("COALESCE(Student.FirstName, '')			    AS FirstName, ");
            queryBuilder.Append("COALESCE(Student.MiddleName, '')			    AS MiddleName, ");
            queryBuilder.Append("COALESCE(Student.LastName, '')				    AS LastName, ");
            queryBuilder.Append("COALESCE(Student.DateOfBirth, '1970-01-01')    AS DateOfBirth, ");
            queryBuilder.Append("COALESCE(GenderLookup.Description, '')		    AS Gender, ");
            queryBuilder.Append("COALESCE(EthnicityLookup.Description, '')	    AS Ethnicity, ");
            queryBuilder.Append("Student.HasCriminalRecord               	    AS HasCriminalRecord, ");
            queryBuilder.Append("Student.IsUsCitizen            			    AS IsUsCitizen,");
            queryBuilder.Append("Student.IntendsToApplyForFederalAid            AS IntendsToApplyForFederalAid, ");
            queryBuilder.Append("COALESCE(HowHearAboutLookup.Description, '')   AS HowHearAbout, ");
            queryBuilder.Append("COALESCE(Student.Phone, '')					AS Phone, ");
            queryBuilder.Append("COALESCE(StudentAddress.Address1, '')		    AS Address1, ");
            queryBuilder.Append("COALESCE(StudentAddress.Address2, '')		    AS Address2, ");
            queryBuilder.Append("COALESCE(StudentAddress.City, '')			    AS City, ");
            queryBuilder.Append("COALESCE(StudentStateLookup.Abbreviation, '')  AS [State], ");
            queryBuilder.Append("COALESCE(StudentAddress.Zip, '')			    AS Zip, ");
            queryBuilder.Append("COALESCE(StudentAddress.Country, '')		    AS Country, ");
            queryBuilder.Append("UserInfo.EmailAddress, ");
            queryBuilder.Append("ScholarshipApplication.HighSchoolIsInUtah      AS HighSchoolIsInUtah, ");
            queryBuilder.Append("COALESCE(HighSchool.Name, '')				    AS HighSchoolName, ");
            queryBuilder.Append("COALESCE(HighSchool.District, '')			    AS HighSchoolDistrict, ");
            queryBuilder.Append("COALESCE(HighSchool.City, '')				    AS HighSchoolCity, ");
            queryBuilder.Append("COALESCE(HighSchoolStateLookup.Abbreviation, '') AS HighSchoolState, ");
            queryBuilder.Append("COALESCE(HighSchool.Zip, '')				    AS HighSchoolZip, ");
            queryBuilder.Append("ScholarshipApplication.GraduationYear          AS HighSchoolGraduationYear, ");
            queryBuilder.Append("COALESCE(ScholarshipApplication.HighSchoolCumulativeGpa, 0) AS HighSchoolCumulativeGpa, ");
            queryBuilder.Append("COALESCE(CollegeLookup.Description, '')	    AS College, ");
            queryBuilder.Append("COALESCE(JuniorHigh.Name, '')				    AS JuniorHighName, ");
            queryBuilder.Append("COALESCE(JuniorHigh.District, '')			    AS JuniorHighDistrict, ");
            queryBuilder.Append("COALESCE(JuniorHigh.City, '')				    AS JuniorHighCity, ");
            queryBuilder.Append("COALESCE(JuniorHighStateLookup.Abbreviation, '') AS JuniorHighState, ");
            queryBuilder.Append("COALESCE(JuniorHigh.Zip, '')				    AS JuniorHighZip,");
            queryBuilder.Append("ScholarshipApplication.HaveAttendedOther       AS HasAttendedOtherHighSchool, ");
            queryBuilder.Append("COALESCE(ActCompositeLookup.Description, '')   AS ActCompositeScore, ");
            queryBuilder.Append("COALESCE(ActEnglishLookup.Description, '')	    AS ActEnglishScore, ");
            queryBuilder.Append("COALESCE(ActMathLookup.Description, '')	    AS ActMathScore, ");
            queryBuilder.Append("COALESCE(ActScienceLookup.Description, '')	    AS ActScienceScore, ");
            queryBuilder.Append("COALESCE(ActReadingLookup.Description, '')	    AS ActReadingScore, ");
            queryBuilder.Append("COALESCE(ClassTypeLookup.Description, '')	    AS ClassType, ");
            queryBuilder.Append("ClassList.SequenceNo						    AS ClassNumber, ");
            queryBuilder.Append("COALESCE(ClassTitleLookup.Description, '')	    AS ClassName, ");
            queryBuilder.Append("ClassList.GradeLevel, ");
            queryBuilder.Append("COALESCE(ClassList.WeightCode, '')			    AS ClassWeight, ");
            queryBuilder.Append("COALESCE(ClassList.Credits, 0)				    AS Credits, ");
            queryBuilder.Append("COALESCE(GradeLookup1.Description, '')		    AS Grade1, ");
            queryBuilder.Append("COALESCE(GradeLookup2.Description, '')		    AS Grade2, ");
            queryBuilder.Append("COALESCE(GradeLookup3.Description, '')		    AS Grade3, ");
            queryBuilder.Append("COALESCE(GradeLookup4.Description, '')		    AS Grade4, ");
            queryBuilder.Append("COALESCE(GradeLookup5.Description, '')		    AS Grade5, ");
            queryBuilder.Append("COALESCE(GradeLookup6.Description, '')		    AS Grade6 ");
            queryBuilder.Append("FROM Student ");
            queryBuilder.Append("INNER JOIN GenderLookup ");
            queryBuilder.Append("ON Student.GenderCode = GenderLookup.Code ");
            queryBuilder.Append("INNER JOIN EthnicityLookup ");
            queryBuilder.Append("ON Student.EthnicityCode = EthnicityLookup.Code ");
            queryBuilder.Append("INNER JOIN HowHearAboutLookup ");
            queryBuilder.Append("ON Student.HowHearAboutCode = HowHearAboutLookup.Code ");
            queryBuilder.Append("INNER JOIN StudentAddress ");
            queryBuilder.Append("ON Student.Username = StudentAddress.Username ");
            queryBuilder.Append("INNER JOIN StateLookup AS StudentStateLookup ");
            queryBuilder.Append("ON StudentAddress.StateCode = StudentStateLookup.Code ");
            queryBuilder.Append("INNER JOIN UserInfo ");
            queryBuilder.Append("ON Student.Username = UserInfo.Username ");
            queryBuilder.Append("INNER JOIN ScholarshipApplication ");
            queryBuilder.Append("ON Student.Username = ScholarshipApplication.Username ");
            queryBuilder.Append("INNER JOIN SchoolLookup AS HighSchool ");
            queryBuilder.Append("ON ScholarshipApplication.HighSchoolCeebCode = HighSchool.CeebCode ");
            queryBuilder.Append("INNER JOIN StateLookup AS HighSchoolStateLookup ");
            queryBuilder.Append("ON HighSchool.StateCode = HighSchoolStateLookup.Code ");
            queryBuilder.Append("INNER JOIN CollegeLookup ");
            queryBuilder.Append("ON ScholarshipApplication.CollegeCode = CollegeLookup.Code ");
            queryBuilder.Append("INNER JOIN SchoolLookup AS JuniorHigh ");
            queryBuilder.Append("ON ScholarshipApplication.JuniorHighCeebCode = JuniorHigh.CeebCode ");
            queryBuilder.Append("INNER JOIN StateLookup AS JuniorHighStateLookup ");
            queryBuilder.Append("ON JuniorHigh.StateCode = JuniorHighStateLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActComposite ");
            queryBuilder.Append("ON Student.Username = ActComposite.Username ");
            queryBuilder.Append("AND ActComposite.TypeCode = 1 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActCompositeLookup ");
            queryBuilder.Append("ON ActComposite.ScoreCode = ActCompositeLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActEnglish ");
            queryBuilder.Append("ON Student.Username = ActEnglish.Username ");
            queryBuilder.Append("AND ActEnglish.TypeCode = 2 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActEnglishLookup ");
            queryBuilder.Append("ON ActEnglish.ScoreCode = ActEnglishLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActMath ");
            queryBuilder.Append("ON Student.Username = ActMath.Username ");
            queryBuilder.Append("AND ActMath.TypeCode = 3 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActMathLookup ");
            queryBuilder.Append("ON ActMath.ScoreCode = ActMathLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActScience ");
            queryBuilder.Append("ON Student.Username = ActScience.Username ");
            queryBuilder.Append("AND ActScience.TypeCode = 4 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActScienceLookup ");
            queryBuilder.Append("ON ActScience.ScoreCode = ActScienceLookup.Code ");
            queryBuilder.Append("INNER JOIN Act AS ActReading ");
            queryBuilder.Append("ON Student.Username = ActReading.Username ");
            queryBuilder.Append("AND ActReading.TypeCode = 5 ");
            queryBuilder.Append("INNER JOIN ActScoreLookup AS ActReadingLookup ");
            queryBuilder.Append("ON ActReading.ScoreCode = ActReadingLookup.Code ");
            queryBuilder.Append("INNER JOIN ClassList ");
            queryBuilder.Append("ON Student.Username = ClassList.Username ");
            queryBuilder.Append("INNER JOIN ClassTypeLookup ");
            queryBuilder.Append("ON ClassList.ClassTypeCode = ClassTypeLookup.Code ");
            queryBuilder.Append("INNER JOIN ClassTitleLookup ");
            queryBuilder.Append("ON ClassList.ClassTitleCode = ClassTitleLookup.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade1 ");
            queryBuilder.Append("ON ClassList.Username = Grade1.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade1.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade1.ClassSequenceNo ");
            queryBuilder.Append("AND Grade1.Term = 1 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup1 ");
            queryBuilder.Append("ON Grade1.GradeCode = GradeLookup1.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade2 ");
            queryBuilder.Append("ON ClassList.Username = Grade2.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade2.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade2.ClassSequenceNo ");
            queryBuilder.Append("AND Grade2.Term = 2 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup2 ");
            queryBuilder.Append("ON Grade2.GradeCode = GradeLookup2.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade3 ");
            queryBuilder.Append("ON ClassList.Username = Grade3.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade3.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade3.ClassSequenceNo ");
            queryBuilder.Append("AND Grade3.Term = 3 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup3 ");
            queryBuilder.Append("ON Grade3.GradeCode = GradeLookup3.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade4 ");
            queryBuilder.Append("ON ClassList.Username = Grade4.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade4.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade4.ClassSequenceNo ");
            queryBuilder.Append("AND Grade4.Term = 4 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup4 ");
            queryBuilder.Append("ON Grade4.GradeCode = GradeLookup4.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade5 ");
            queryBuilder.Append("ON ClassList.Username = Grade5.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade5.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade5.ClassSequenceNo ");
            queryBuilder.Append("AND Grade5.Term = 5 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup5 ");
            queryBuilder.Append("ON Grade5.GradeCode = GradeLookup5.Code ");
            queryBuilder.Append("LEFT OUTER JOIN Grade AS Grade6 ");
            queryBuilder.Append("ON ClassList.Username = Grade6.Username ");
            queryBuilder.Append("AND ClassList.ClassTypeCode = Grade6.ClassTypeCode ");
            queryBuilder.Append("AND ClassList.SequenceNo = Grade6.ClassSequenceNo ");
            queryBuilder.Append("AND Grade6.Term = 6 ");
            queryBuilder.Append("LEFT OUTER JOIN GradeLookup AS GradeLookup6 ");
            queryBuilder.Append("ON Grade6.GradeCode = GradeLookup6.Code ");
            queryBuilder.Append(string.Format("WHERE Student.Username = '{0}'", username.ToUpper()));
            try
            {
                return dc.ExecuteQuery<AllDataRecords>(queryBuilder.ToString()).ToList();
            }
            catch (Exception ex)
            {
                Exception x = ex.InnerException;
                return null;
            }
        }//GetPdfRecords()
        #endregion

        public bool Submit(string userName)
        {
            DateTime date = DateTime.Now;
            try
            {
                string submit = string.Format("Execute sp_Submit @Username = '{0}', @SubmitDate = '{1}'", userName.ToUpper(), date);
                return dc.ExecuteCommand(submit) == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public List<ClassList> GetListOfColleges()
        {
            string selectStr = "SELECT [Description], [Code] FROM [CollegeLookup] ORDER BY [Description]";
            return dc.ExecuteQuery<ClassList>(selectStr).ToList();
        }

        internal List<string> GetAllScienceClassNames(string userName)
        {
            string selectStr = "SELECT b.Description FROM ClassList a JOIN ClassTitleLookup b ON a.ClassTitleCode = b.Code WHERE a.UserName = '" + userName + "' AND a.ClassTypeCode = 3";
            return dc.ExecuteQuery<string>(selectStr).ToList();
        }
    }
}
