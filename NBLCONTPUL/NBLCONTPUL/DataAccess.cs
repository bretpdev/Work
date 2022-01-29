using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;

namespace NBLCONTPUL
{
    public static class DataAccess
    {
        public enum Region
        {
            OneLink=0,
            Uheaa=1,
            CornerStone=2,
            Exclusion=3
        }

        #region ProcessRecords

        /// <summary>
        /// Hits Noble's server and gets back the calls made based on Region
        /// </summary>
        /// <param name="region">Uheaa, CornerStone, or OneLink</param>
        /// <param name="logData">ProcessLogData object</param>
        /// <returns>List of NobleData objects containing information about calls</returns>
        public static List<NobleData> LoadNobleData(Region region, ProcessLogData logData)
        {
			BatchProcessingHelper helper = BatchProcessingHelper.GetNextAvailableId("", "NobleODBC");
			string NobleConnString = string.Format("Driver={0}PostgreSQL UNICODE{1};Server=UHEAAapp1.uheaa.ushe.local;Port=5432;Database=task;Uid={2};Pwd={3}", "{", "}", helper.UserName, helper.Password);
			//get list of campaigns into usable form
			List<string> callCampaigns = GetCallCampaigns(region);
			string campaigns = string.Format("'{0}'", string.Join("','", callCampaigns));

			List<string> exclusionCallCampaigns = GetCallCampaigns(Region.Exclusion);
			string exclusionCampaigns = string.Format("'{0}'", string.Join("','", exclusionCallCampaigns));

			//Init variables for Connection to Noble Server
			DataSet ds = new DataSet();
			OdbcConnection con = new OdbcConnection(NobleConnString);
			con.Open();
			//pull call_history records from the last available table
			string query = string.Format("SELECT rowid, call_type, listid, appl, lm_filler2, areacode, phone, addi_status, status, tsr, act_date, act_time, time_connect, time_acw, time_hold, lm_filler1 FROM call_history WHERE call_type != 5 and appl in ({0}) and appl not in ({1}) and status in ('RC', 'D', 'PT', 'DF', 'WN', 'FB', 'P', 'LM', 'RP')", campaigns, exclusionCampaigns);
			string query2 = string.Format("SELECT rowid, call_type, listid, appl, filler2, ani_acode, ani_phone, addi_status, status, tsr, call_date, call_time, time_connect, time_acwork, time_holding, filler1 FROM inboundlog WHERE call_type != 5 and appl in ({0}) and appl not in ({1}) and status in ('RC', 'D', 'PT', 'DF', 'WN', 'FB', 'P', 'LM', 'RP')", campaigns, exclusionCampaigns);

            using (OdbcCommand cmd = new OdbcCommand(query, con))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(cmd))
                {
                    try
                    {
                        da.Fill(ds, "NobleCallHistory"); //If table exists, pull the data.
                    }
                    catch (Exception ex)
                    {
                        ProcessLogger.AddNotification(logData.ProcessLogId, "Not able to load data from table call_history.", NotificationType.ErrorReport, NotificationSeverityType.Critical, region == Region.CornerStone ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa, Assembly.GetExecutingAssembly(), ex);
                    }
                }
            }
            using (OdbcCommand cmd = new OdbcCommand(query2, con))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(cmd))
                {
                    try
                    {
                        da.Fill(ds, "NobleCallHistory"); //If table exists, pull the data.
                    }
                    catch (Exception ex)
                    {
                        ProcessLogger.AddNotification(logData.ProcessLogId, "Not able to load data from table InboundLog.", NotificationType.ErrorReport, NotificationSeverityType.Critical, region == Region.CornerStone ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa, Assembly.GetExecutingAssembly(), ex);
                    }
                }
            }
            List<NobleData> batchRecords = DataAccessHelper.ParseDataTable<NobleData>(ds.Tables[0]);

            con.Close();
            return batchRecords;
        }

        /// <summary>
        /// Returns a list of Campaigns that pertain to a specific region
        /// <param name="region">Uheaa, CornerStone, OneLink, or Exclusion</param>
        /// </summary>
        /// <returns>List of call campaigns</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetContactCampaigns")]
        public static List<string> GetCallCampaigns(Region region)
        {
            string regionName = "";
            if (region == Region.CornerStone)
                regionName = "Cornerstone";
            else if (region == Region.OneLink)
                regionName = "Onelink";
            else if (region == Region.Uheaa)
                regionName = "UHEAA";
            else if (region == Region.Exclusion)
                regionName = "Exclusion";

            return DataAccessHelper.ExecuteList<string>("Noble.GetContactCampaigns", DataAccessHelper.Database.Csys,
                new SqlParameter("RegionName", regionName));
        }

        /// <summary>
        /// Takes the agentCode supplied by Noble and gets the username back
        /// </summary>
        /// <param name="agentCode">Agent code that was on the the call</param>
        /// <returns>string</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetAgentName")]
        public static string GetAgentName(string agentCode)
        {
            return DataAccessHelper.ExecuteList<string>("[Noble].GetAgentName", DataAccessHelper.Database.Csys,
                new SqlParameter("AgentCode", agentCode)).SingleOrDefault();
        }

        /// <summary>
        /// Takes the agentCode supplied by Noble and find the other windows user for that Id
        /// </summary>
        /// <param name="agentCode">Agent code that was on the the call</param>
        /// <returns>string</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetAgentOtherId")]
        public static string GetOtherUserId(string agentCode)
        {
            return DataAccessHelper.ExecuteList<string>("[Noble].GetAgentOtherId", DataAccessHelper.Database.Csys,
                new SqlParameter("AgentCode", agentCode)).SingleOrDefault();
        }

        /// <summary>
        /// Pulls the ContactCampaignId back from csys.
        /// </summary>
        /// <param name="record">Noble Data Record</param>
        /// <param name="region">Cornerstone, OneLink, or Uheaa</param>
        /// <param name="logData">Process logger</param>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetContactCampaignId")]
        public static int GetContactCampaignId(NobleData record, Region region, ProcessLogData LogData)
        {
            string regionName = "";
            if (region == Region.CornerStone)
                regionName = "Cornerstone";
            else if (region == Region.Uheaa)
                regionName = "Uheaa";
            else if (region == Region.OneLink)
                regionName = "Onelink";
            return DataAccessHelper.ExecuteSingle<int>("[Noble].GetContactCampaignId", DataAccessHelper.Database.Csys, new SqlParameter("RegionName", regionName ?? ""), new SqlParameter("Campaign", record.Campaign ?? ""));
        }

        /// <summary>
        /// Inserts records into the database based on region and database.
        /// </summary>
        /// <param name="record">Noble Data Record</param>
        /// <param name="region">Cornerstone, OneLink, or Uheaa</param>
        /// <param name="database">CLS or ULS</param>
        /// <param name="campaignId">integer from CSYS</param>
        /// <param name="logData">Process logger</param>
        [UsesSproc(DataAccessHelper.Database.Cls, "[Noble].InsertNobleContactCalls")]
        [UsesSproc(DataAccessHelper.Database.Uls, "[Noble].InsertNobleContactCalls")]
        public static void InsertCalls(NobleData record, Region region, DataAccessHelper.Database database, int campaignId, ProcessLogData logData)
        {
            string regionName = "";
            if (region == Region.CornerStone)
                regionName = "Cornerstone";
            else if (region == Region.Uheaa)
                regionName = "Uheaa";
            else if (region == Region.OneLink)
                regionName = "Onelink";

            DataAccessHelper.Execute("[Noble].InsertNobleContactCalls", database,
                new SqlParameter("NobleRowId", record.NobleRowId), new SqlParameter("RegionName", regionName ?? ""), new SqlParameter("Telephone", record.PhoneNumber ?? ""), new SqlParameter("Category", record.CallType ?? ""),
                new SqlParameter("ListId", record.ListId ?? ""), new SqlParameter("Campaign", record.Campaign ?? ""), new SqlParameter("CampaignId", campaignId.ToString() ?? ""),
                new SqlParameter("AgentCode", record.AgentCode ?? ""), new SqlParameter("Disposition", record.Disposition ?? ""), new SqlParameter("AgentDisposition", record.AgentDisposition ?? ""),
                new SqlParameter("CallDate", record.CallDateOut ?? record.CallDateIn ?? ""), new SqlParameter("CallTime", record.CallTimeOut ?? record.CallTimeIn ?? ""), new SqlParameter("TimeConnected", record.TimeConnected ?? ""),
                new SqlParameter("TimeHold", record.TimeHoldOut ?? record.TimeHoldIn ?? ""), new SqlParameter("TimeACW", record.TimeACWOut ?? record.TimeACWIn ?? ""), new SqlParameter("Filler1", record.Filler1Out ?? record.Filler1In ?? ""),
                new SqlParameter("SSN", record.SSN ?? ""), new SqlParameter("AccountNumber", record.AccountNumber ?? ""), new SqlParameter("Filler2", record.IdentifierOut ?? record.IdentifierIn ?? ""),
                new SqlParameter("AgentCode2", record.AgentCode2 ?? ""), new SqlParameter("AgentName", record.AgentName ?? ""));
        }

        #endregion

        #region UserInterface

        /// <summary>
        /// Pulls a list of groups from the Noble.Groups table on CSYS
        /// </summary>
        /// <returns>List of strings</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetGroups")]
        public static List<string> GetGroups()
        {
            return DataAccessHelper.ExecuteList<string>("Noble.GetGroups", DataAccessHelper.Database.Csys);
        }

        /// <summary>
        /// Returns the value of the status column in the Noble.ContactCampaigns table for the given callCampaign/group combo.
        /// </summary>
        /// <param name="callCampaign">string. Max 6 characters</param>
        /// <param name="group">string. max 15 characters</param>
        /// <returns>true for active and false for inactive</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetActiveFlag")]
        public static bool GetActiveFlag(string callCampaign, string group)
        {
            return DataAccessHelper.ExecuteList<bool>("Noble.GetActiveFlag", DataAccessHelper.Database.Csys, new SqlParameter("Campaign", callCampaign), new SqlParameter("Group", group)).SingleOrDefault();
        }

        /// <summary>
        /// Returns the value of the invalidate column in the Noble.ContactCampaigns table for the given callCampaign/group combo.
        /// </summary>
        /// <param name="callCampaign">string. Max 6 characters</param>
        /// <param name="group">string. max 15 characters</param>
        /// <returns>true for active and false for inactive</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetInvalidateFlag")]
        public static bool GetInvalidateFlag(string callCampaign, string group)
        {
            return DataAccessHelper.ExecuteList<bool>("Noble.GetInvalidateFlag", DataAccessHelper.Database.Csys,
                new SqlParameter("Campaign", callCampaign), new SqlParameter("Group", group)).SingleOrDefault();
        }

        /// <summary>
        /// Returns the value of the CallType column in the Noble.ContactCampaigns table for the given callCampaign/group combo.
        /// </summary>
        /// <param name="callCampaign">string. Max 6 characters</param>
        /// <param name="group">string. max 15 characters</param>
        /// <returns>true for inbound and false for outbound</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetCallType")]
        public static bool GetCallType(string callCampaign, string group)
        {
            return DataAccessHelper.ExecuteList<bool>("Noble.GetCallType", DataAccessHelper.Database.Csys,
                new SqlParameter("Campaign", callCampaign), new SqlParameter("Group", group)).SingleOrDefault();
        }

        /// <summary>
        /// Returns the value of the CampaignName column in the Noble.ContactCampaigns table for the given callCampaign/group combo.
        /// </summary>
        /// <param name="callCampaign">string. Max 6 characters</param>
        /// <param name="group">string. max 15 characters</param>
        /// <returns>Description of the campaign</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetCampaignName")]
        public static string GetCampaignDescription(string callCampaign, string group)
        {
            return DataAccessHelper.ExecuteList<string>("Noble.GetCampaignName", DataAccessHelper.Database.Csys,
                new SqlParameter("Campaign", callCampaign), new SqlParameter("Group", group)).SingleOrDefault();
        }

        /// <summary>
        /// Applies the parameters to the record in Noble.ContactCampaigns in CSYS.
        /// </summary>
        /// <param name="group">string. max 15 characters</param>
        /// <param name="callCampaign">string. Max 6 characters</param>
        /// <param name="description">string. Max 100 characters</param>
        /// <param name="callType">bool  true for inbound false for outbound</param>
        /// <param name="active">bool</param>
        /// <param name="invalidate">bool</param>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].UpdateCampaign")]
        public static void UpdateCallCampaign(string group, string callCampaign, string description, bool callType, bool active, bool invalidate)
        {
            DataAccessHelper.Execute("Noble.UpdateCampaign", DataAccessHelper.Database.Csys,
                new SqlParameter("Group", group), new SqlParameter("Campaign", callCampaign), new SqlParameter("Description", description),
                new SqlParameter("CallType", callType ? 1 : 0), new SqlParameter("Status", active ? 1 : 0), new SqlParameter("Invalidate", invalidate ? 1 : 0),
                new SqlParameter("User", Environment.UserName));
        }

        /// <summary>
        /// Applies the parameters to a new record in Noble.ContactCampaigns in CSYS.
        /// </summary>
        /// <param name="group">string. max 15 characters</param>
        /// <param name="callCampaign">string. Max 6 characters</param>
        /// <param name="description">string. Max 100 characters</param>
        /// <param name="callType">bool  true for inbound false for outbound</param>
        /// <param name="active">bool</param>
        /// <param name="invalidate">bool</param>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].AddCampaign")]
        public static void AddCampaign(string group, string callCampaign, string description, bool callType, bool active, bool invalidate)
        {
            DataAccessHelper.Execute("Noble.AddCampaign", DataAccessHelper.Database.Csys,
                new SqlParameter("Group", group), new SqlParameter("Campaign", callCampaign), new SqlParameter("Description", description),
                new SqlParameter("CallType", callType ? 1 : 0), new SqlParameter("Status", active ? 1 : 0), new SqlParameter("Invalidate", invalidate ? 1 : 0),
                new SqlParameter("User", Environment.UserName));
        }
        #endregion

        /// <summary>
        /// Gets the TSRMASTER_SEC table and inserts/updates it into our csys database
        /// <param name="logData">Processlogger</param>
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].GetUsernameList")]
        [UsesSproc(DataAccessHelper.Database.Csys, "[Noble].InsertUsername")]
        public static void GetNobleUsernameRelation(ProcessLogData logData)
        {
            BatchProcessingHelper helper = BatchProcessingHelper.GetNextAvailableId("", "NobleODBC");
            string NobleConnString = string.Format("Driver={0}PostgreSQL UNICODE{1};Server=UHEAAapp1.uheaa.ushe.local;Port=5432;Database=task;Uid={2};Pwd={3}", "{", "}", helper.UserName, helper.Password);
            //Init variables for Connection to Noble Server
            DataSet ds = new DataSet();
            OdbcConnection con = new OdbcConnection(NobleConnString);
            con.Open();
            //pull TSRMASTER_SEC records 
            string query = string.Format("SELECT trim(both ' ' from Username) AS Username, TSR, last_update FROM TSRMASTER_SEC");

            using (OdbcCommand cmd = new OdbcCommand(query, con))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(cmd))
                {
                    try
                    {
                        da.Fill(ds, "NobleUsers"); //If table exists, pull the data.
                    }
                    catch (Exception ex)
                    {
                        ProcessLogger.AddNotification(logData.ProcessLogId, "Not able to load data from table call_history.", NotificationType.ErrorReport, NotificationSeverityType.Critical, DataAccessHelper.Region.CornerStone, Assembly.GetExecutingAssembly(), ex);
                    }
                }
            }

            List<NobleUsers> userRecords = DataAccessHelper.ParseDataTable<NobleUsers>(ds.Tables[0]);

            con.Close();
            //Load values for UserList table to compare it to the newest version and add any records that dont exist yet
            List<string> databaseRecords = DataAccessHelper.ExecuteList<string>("[Noble].GetUsernameList", DataAccessHelper.Database.Csys).ToList().Select(p => p.Trim()).ToList();
            foreach (NobleUsers record in userRecords.Where(p => !p.Username.IsIn(databaseRecords.ToArray())))
            {
                DataAccessHelper.Execute("[Noble].InsertUsername", DataAccessHelper.Database.Csys, new SqlParameter("Username", record.Username), new SqlParameter("TSR", record.TSR), new SqlParameter("LastUpdated", record.last_update ?? DateTime.Now.ToShortDateString()));
            }
        }
    }
}
