using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace TIMETRAKUP
{
    class DataAccess
    {
        [UsesSproc(DataAccessHelper.Database.Csys, "spGetUserIdAndFullName")]
        public static List<User> AllUsers()
        {
            return DataAccessHelper.ExecuteList<User>("spGetUserIdAndFullName", DataAccessHelper.Database.Csys, new SqlParameter("Status", true));
        }

        [UsesSproc(DataAccessHelper.Database.Reporting, "spGetAllTimesForUser")]
        public static List<UserTime> GetTimesForUser(User selectedUser, bool unstoppedTime)
        {
            return DataAccessHelper.ExecuteList<UserTime>("spGetAllTimesForUser", DataAccessHelper.Database.Reporting,
                new SqlParameter("SqlUserID", selectedUser.SqlUserId), new SqlParameter("UnstoppedTime", unstoppedTime)
                ).OrderBy(p => p.TimeTrackingId).ToList();
        }

        [UsesSproc(DataAccessHelper.Database.Reporting, "spUpdateRecord")]
        public static bool UpdateRecord(UserTime updatedRecord)
        {
            try
            {
                SqlCommand comm = DataAccessHelper.GetCommand("spUpdateRecord", DataAccessHelper.Database.Reporting);
                comm.Parameters.AddWithValue("TimeTrackingId", updatedRecord.TimeTrackingId);
                comm.Parameters.AddWithValue("StartTime", updatedRecord.StartTime);
                comm.Parameters.AddWithValue("EndTime", updatedRecord.EndTime);
                comm.Parameters.AddWithValue("SqlUserId", GetActiveUserId());
                return DataAccessHelper.ExecuteSingle<int>(comm) == 1;
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\n\r\n" + ex.InnerException);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\n\r\n" + ex.InnerException);
            }
            return false;
        }
        
        [UsesSproc(DataAccessHelper.Database.Csys, "spACDC_GetSquid")]
        private static int GetActiveUserId()
        {
            return DataAccessHelper.ExecuteSingle<int>("spACDC_GetSquid", DataAccessHelper.Database.Csys,
                new SqlParameter("WindowsUsername", Environment.UserName));
        }
    }
}
