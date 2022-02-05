using SCHRPT.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WebApi;

namespace SCHRPT
{
    public class DataAccess
    {
        private LogDataAccess wda;
        private DataAccessHelper.Database db;
        public DataAccess(LogDataAccess wda)
        {
            this.wda = wda;
            db = DataAccessHelper.Database.Cls;
        }

        #region Schools
        public List<School> GetSchools()
        {
            var result = wda.ExecuteList<School>("schrpt.GetSchools", db);
            return result.Result;
        }

        public School GetSchool(int schoolId)
        {
            var result = wda.ExecuteSingle<School>("schrpt.GetSchool", db, Sp("SchoolId", schoolId));
            return result.Result;
        }

        public int AddSchool(string name, string schoolCode, string branchCode)
        {
            var result = wda.ExecuteSingle<int>("schrpt.AddSchool", db, Sp("Name", name), Sp("SchoolCode", schoolCode), Sp("BranchCode", branchCode), USR());
            return result.Result;
        }

        public void UpdateSchool(int schoolId, string name, string schoolCode, string branchCode)
        {
            wda.Execute("schrpt.UpdateSchool", db, Sp("SchoolId", schoolId), Sp("Name", name), Sp("SchoolCode", schoolCode), Sp("BranchCode", branchCode));
        }

        public void RetireSchool(int schoolId)
        {
            wda.Execute("schrpt.RetireSchool", db, Sp("SchoolId", schoolId), USR());
        }
        #endregion

        #region Recipients
        public List<Recipient> GetRecipients()
        {
            var result = wda.ExecuteList<Recipient>("schrpt.GetRecipients", db);
            return result.Result;
        }

        public Recipient GetRecipient(int recipientId)
        {
            var result = wda.ExecuteSingle<Recipient>("schrpt.GetRecipient", db, Sp("RecipientId", recipientId));
            return result.Result;
        }

        public int AddRecipient(string name, string email, string companyName)
        {
            var result = wda.ExecuteSingle<int>("schrpt.AddRecipient", db, Sp("Name", name), Sp("Email", email), Sp("CompanyName", companyName), USR());
            return result.Result;
        }

        public void UpdateRecipient(int recipientId, string name, string email, string companyName)
        {
            wda.Execute("schrpt.UpdateRecipient", db, Sp("RecipientId", recipientId), Sp("Name", name), Sp("Email", email), Sp("CompanyName", companyName));
        }

        public void RetireRecipient(int recipientId)
        {
            wda.Execute("schrpt.RetireRecipient", db, Sp("RecipientId", recipientId), USR());
        }
        #endregion

        #region SchoolRecipients
        public List<SchoolRecipient> GetSchoolRecipients(int? schoolId, int? recipientId)
        {
            var result = wda.ExecuteList<SchoolRecipient>("schrpt.GetSchoolRecipients", db, Sp("SchoolId", schoolId), Sp("RecipientId", recipientId));
            return result.Result;
        }

        public void RetireSchoolRecipient(int schoolRecipientId)
        {
            wda.Execute("schrpt.RetireSchoolRecipient", db, Sp("SchoolRecipientId", schoolRecipientId), USR());
        }

        public void AddSchoolRecipient(int schoolId, int recipientId, int reportTypeId)
        {
            wda.Execute("schrpt.AddSchoolRecipient", db, Sp("SchoolId", schoolId), Sp("RecipientId", recipientId), Sp("ReportTypeId", reportTypeId), USR());
        }
        #endregion

        public List<ReportType> GetReportTypes()
        {
            var result = wda.ExecuteList<ReportType>("schrpt.GetReportTypes", db);
            return result.Result;
        }


        public DashboardModel GetDashboardInfo()
        {
            var result = wda.ExecuteSingle<DashboardModel>("schrpt.GetDashboardInfo", db);
            return result.Result;
        }

        private SqlParameter USR()
        {
            return Sp("WindowsUserName", Environment.UserName);
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}