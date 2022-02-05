using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Uheaa.Common.DataAccess
{
    public class BatchProcessingHelper
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }

        public SqlConnection Connection { get; set; }

        private BatchProcessingHelper()
        {
        }

        /// <summary>
        /// Gets the next available username and password for the given region.
        /// </summary>
        /// <param name="scriptId">DO NOT PROVIDE UNLESS REQUIRED BY YOUR APPLICATION TO USE A SPECIFIC ID</param>
        /// <param name="loginType">The region/type of login</param>
        /// <returns>BatchProcessingHelper object with UserName and Password provided</returns>
        public static BatchProcessingHelper GetNextAvailableId(string scriptId, string loginType)
        {
            scriptId = scriptId ?? "";

            int loginTypeValue = DataAccessHelper.ExecuteSingle<int>("GetLoginTypeId", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("LoginType", loginType));
            var cmd = DataAccessHelper.GetCommand("GetNextAvailableBatchId", DataAccessHelper.Database.BatchProcessing, DataAccessHelper.CurrentMode);

            cmd.Parameters.Add(SqlParams.Single("LoginType", loginTypeValue));
            cmd.Parameters.Add(SqlParams.Single("SackerScriptId", scriptId));
            BatchProcessingHelper helper = new BatchProcessingHelper();
            Repeater.TryRepeatedly(() =>
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        throw new Exception();
                    helper.UserName = reader.GetString(0);
                    //helper.Password = reader.GetString(1);
                    helper.Connection = cmd.Connection;
                }
            }, 34, 3000, true);
            return helper;
        }

        /// <summary>
        /// Gets the next available username and password for the given region.
        /// </summary>
        /// <param name="scriptId">The script id for the application being run</param>
        /// <param name="loginType">The region/type of login</param>
        /// <param name="inactiveIds">List of IDs that have been used unsuccessfully.</param>
        /// <returns>BatchProcessingHelper object with UserName and Password provided</returns>
        public static BatchProcessingHelper GetNextAvailableId(string scriptId, string loginType, List<string> inactiveIds)
        {
            int loginTypeValue = DataAccessHelper.ExecuteSingle<int>("GetLoginTypeId", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("LoginType", loginType));
            var cmd = DataAccessHelper.GetCommand("GetNextAvailableUniqueBatchId", DataAccessHelper.Database.BatchProcessing, DataAccessHelper.CurrentMode);

            cmd.Parameters.Add(SqlParams.Single("LoginType", loginTypeValue));
            cmd.Parameters.Add(SqlParams.Single("SackerScriptId", scriptId));
            if (inactiveIds.Count > 0)
                cmd.Parameters.Add(SqlParams.Single("InactiveIDs", ToTable(inactiveIds)));
            BatchProcessingHelper helper = new BatchProcessingHelper();
            Repeater.TryRepeatedly(() =>
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        throw new Exception();
                    helper.UserName = reader.GetString(0);
                    //helper.Password = reader.GetString(1);
                    helper.LoginId = reader.GetInt32(2);
                    helper.Connection = cmd.Connection;
                }
            }, 34, 3000, true);
            return helper;
        }

        /// <summary>
        /// The sproc to get next available ID has a user defined table with varialbe UserName. You need to send a list of IDs in a table with a column named UserName 
        /// in order for the sproc to read in the data.
        /// </summary>
        /// <param name="inactiveIds"></param>
        /// <returns>DataTable with one column and 1..N rows of IDs</returns>
        private static object ToTable(List<string> inactiveIds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UserName");
            foreach (string id in inactiveIds)
            {
                DataRow r = dt.NewRow();
                r["UserName"] = id;
                dt.Rows.Add(r);
            }
            return dt;
        }

        public static void SetInactive(int loginId, string scriptId, string reason)
        {
            DataAccessHelper.Execute("SetInactive", DataAccessHelper.Database.BatchProcessing,
                SqlParams.Single("LoginId", loginId), SqlParams.Single("ScriptId", scriptId), SqlParams.Single("Reason", reason));
        }

        public static void AddInvalidLogin(int loginId, string scriptId, string reason)
        {
            DataAccessHelper.Execute("AddInvalidLogin", DataAccessHelper.Database.BatchProcessing,
                SqlParams.Single("LoginId", loginId), SqlParams.Single("ScriptId", scriptId), SqlParams.Single("Reason", reason));
        }

        /// <summary>
        /// Clears the pool for the connection, closes and disposes the connection.
        /// Removes the login from the database making the current username/password available.
        /// </summary>
        /// <param name="helper"></param>
        public static void CloseConnection(BatchProcessingHelper helper)
        {
            SqlConnection.ClearPool(helper.Connection);
            helper.Connection.Close();
            helper.Connection.Dispose();
        }
    }
}
