using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Q;
namespace QCDBUser
{
        class DataAccess : DataAccessBase
        {
            public static List<string> GetBusinessUnits(bool testMode)
            {
                string query = "SELECT Distinct BU FROM QCTR_DAT_BU";
                return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
            }

            public static List<string> GetQCIncidentUserBU(bool testMode, string userID)
            {
                string query = "SELECT GENR_REF_BU_Agent_Xref.BusinessUnit FROM SYSA_LST_UserIDInfo INNER JOIN GENR_REF_BU_Agent_Xref ON SYSA_LST_UserIDInfo.WindowsUserName = GENR_REF_BU_Agent_Xref.WindowsUserID WHERE (SYSA_LST_UserIDInfo.UserID = '" + userID + "')";
                return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
            }

            public static List<string> GetFullName(bool testMode, string userID)
            {
                string query = "SELECT B.FirstName + ' ' + B.LastName FROM SYSA_LST_UserIDInfo AS A INNER JOIN SYSA_LST_Users AS B ON A.WindowsUserName = B.WindowsUserName WHERE (A.UserID = '" + userID + "')";
                return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
            }

            public static List<string> GetLoginName(bool testMode, string userID)
            {
                string query = "SELECT WindowsUserName FROM SYSA_LST_UserIDInfo WHERE (UserID = '" + userID + "')";
                return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
            }

            public static List<string> GetUserID(bool testMode, string windowsID)
            {
                string query = "SELECT UserID FROM SYSA_LST_UserIDInfo WHERE (WindowsUserName = '" + windowsID + "')";
                return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
            }

            public static List<string> GetUserID(bool testMode)
            {
                string query = "SELECT Distinct UserID FROM SYSA_LST_UserIDInfo";
                return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
            }

            public static List<UserIntervention> GetQCIncidents(bool testMode, string BU)
            {
                string query = "SELECT A.ActivityDate, A.ID, A.ReportName, A.UserID, A.Description, A.RequiredDays, A.BusinessUnit, A.SavedDate, B.PriorityCategory, B.PriorityUrgency FROM QCTR_DAT_UserIntervention AS A INNER JOIN QCTR_LST_ReportsToProcess AS B ON A.ReportName = B.ReportName WHERE A.BusinessUnit = '" + BU + "'";
                return BsysDataContext(testMode).ExecuteQuery<UserIntervention>(query).ToList();
            }

            public static string GetQCBUCount(bool testMode, string BU)
            {
                string query = "SELECT Count(A.ActivityDate) FROM QCTR_DAT_UserIntervention AS A INNER JOIN QCTR_LST_ReportsToProcess AS B ON A.ReportName = B.ReportName WHERE A.BusinessUnit = '" + BU + "'";
                return BsysDataContext(testMode).ExecuteQuery<int>(query).Single().ToString();
            }

            public static List<String> GetSubject(bool testMode, string reportName) //Needed this as a patch to acquire Subject. 
            {
                string query = "SELECT Subject FROM QCTR_LST_ReportsToProcess WHERE ReportName = '" + reportName + "'";
                return BsysDataContext(testMode).ExecuteQuery<String>(query).ToList();
            }

            public static void SaveForLater(bool testMode, string uniqueID, string description)
            {
                DateTime currentTime;
                currentTime = DateTime.Now;
                string query = "UPDATE QCTR_DAT_UserIntervention SET SavedDate = '" + currentTime + "', Description = '" + description.Replace("'","`") + "'  WHERE (ID = '" + uniqueID + "')";
                //return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
                BsysDataContext(testMode).ExecuteCommand(query);
                return;
            }

            public static  void DiscardRecord(bool testMode, string uniqueID)
            {
                string query = "Delete From QCTR_DAT_UserIntervention WHERE (ID = '" + uniqueID + "')";
                //return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
                BsysDataContext(testMode).ExecuteCommand(query);
                return;
            }

            public static IssueInfo InsertRecord(bool testMode, string reportName, string userRunningScriptName, string savedDate, string bUnit, string respName, string reqDate, string actDate, string desc, string category, string urgency)
            {
                DateTime requiredDate = DateTime.Now.AddDays(Convert.ToDouble(reqDate));

                string query = string.Format("EXEC spQCTR_QCDBUser_CreateIssue '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'", reportName, userRunningScriptName, bUnit, respName, requiredDate, Convert.ToDateTime(actDate), desc.Replace("'", "`"), category, urgency);

                return  BsysDataContext(testMode).ExecuteQuery<IssueInfo>(query).SingleOrDefault();
            }

            public static EmailInfo GetEmailInfo(bool testMode, string issueNumber) 
            {
                string query = "SELECT Issue, History FROM QCTR_DAT_Issue WHERE ID = '" + issueNumber + "'";
                return BsysDataContext(testMode).ExecuteQuery<EmailInfo>(query).Single();
            }

            public static EmailRecipients GetEmailRecipients(bool testMode, string IssueID)
            {
                string query = string.Format("EXEC spQCTR_GetEmailData '{0}'", IssueID);

                return BsysDataContext(testMode).ExecuteQuery<EmailRecipients>(query).Single();
            }
        }//class
}
