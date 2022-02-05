using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
namespace NoblePhoneInvalidation
{
	public static class DataAccess
	{
		public enum Region
		{
			OneLink,
			UheaaCompass,
			CornerStoneCompass
		}

		/// <summary>
		/// Populate a list of objects with data from Noble's RAS database
		/// <param name="region">OneLink,UheaaCompass, or CornerStoneUheaa</param>
		/// <param name="logData">ProcessLogData Object</param>
		/// </summary>
		/// <returns>A list of NobleData objects</returns>
		public static List<NobleData> LoadNobleData(Region region, ProcessLogData logData)
		{
            BatchProcessingHelper helper = BatchProcessingHelper.GetNextAvailableId("", "NobleODBC");
            string NobleConnString = string.Format("Driver={0}PostgreSQL UNICODE{1};Server=UHEAAapp1.uheaa.ushe.local;Port=5432;Database=task;Uid={2};Pwd={3}", "{", "}", helper.UserName, helper.Password);
			List<string> callCampaigns = getCallCampaignForRegion(region);
			//get list of campaigns into usable form
			string campaigns = string.Format("'{0}'", string.Join("','", callCampaigns));
			//Init variables for Connection to Noble Server
			DataSet ds = new DataSet();
			OdbcConnection con = new OdbcConnection(NobleConnString);
			con.Open();
			OdbcCommand cmd;
			OdbcDataAdapter da;
			//pull call_history records from the last available table
			string query = string.Format("SELECT call_type, listid, appl, lm_filler2, areacode, phone, addi_status, status, tsr, act_date, act_time FROM call_history WHERE status in ('D', 'FX', 'WN') and appl in ({0})", campaigns);
			using (cmd = new OdbcCommand(query, con))
			{
				using (da = new OdbcDataAdapter(cmd))
				{
					try
					{
						da.Fill(ds, "NobleCallHistory"); //If table exists, pull the data.
					}
					catch (Exception ex)
					{
						ProcessLogger.AddNotification(logData.ProcessLogId, "Not able to load data from table call_history.", NotificationType.ErrorReport, NotificationSeverityType.Critical, region == Region.CornerStoneCompass ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa, Assembly.GetExecutingAssembly(), ex);
					}
				}
			}

			List<NobleData> batchRecords = DataAccessHelper.ParseDataTable<NobleData>(ds.Tables[0]);
			con.Close();
			return batchRecords;
		}

		/// <summary>
		/// Takes a Region enum and returns a list of strings
		/// </summary>
		/// <param name="region">OneLink, UheaaCompass, or CornerStoneCompass</param>
		/// <returns>List of strings from the database for the campaigns used by the region.</returns>
		private static List<string> getCallCampaignForRegion(Region region)
		{
			List<string> callCampaigns = new List<string>();
			if (region == Region.OneLink)
				callCampaigns = GetOneLinkCampaigns();
			else if (region == Region.UheaaCompass)
				callCampaigns = GetCompassUheaaCampaigns();
			else if (region == Region.CornerStoneCompass)
				callCampaigns = GetCompassCornerCampaigns();
			return callCampaigns;
		}

		/// <summary>
		/// Goes to the database and pulls a list of the APPL codes that should apply to One Link
		/// </summary>
		/// <returns>List of strings representing all call campaigns for One Link</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "GetCallCampaigns")]
		public static List<string> GetOneLinkCampaigns()
		{
			return DataAccessHelper.ExecuteList<string>("GetCallCampaigns", DataAccessHelper.Database.Csys, new SqlParameter("Region", "OneLink"));
		}

		/// <summary>
		/// Goes to the database and pulls a list of the APPL codes that should apply to Compass Uheaa
		/// </summary>
		/// <returns>List of strings representing all call campaigns for Compass Uheaa</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "GetCallCampaigns")]
		public static List<string> GetCompassUheaaCampaigns()
		{
			return DataAccessHelper.ExecuteList<string>("GetCallCampaigns", DataAccessHelper.Database.Csys, new SqlParameter("Region", "CompassUheaa"));
		}

		/// <summary>
		/// Goes to the database and pulls a list of the APPL codes that should apply to Compass CornerStone
		/// </summary>
		/// <returns>List of strings representing all call campaigns for Compass CornerStone</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "GetCallCampaigns")]
		public static List<string> GetCompassCornerCampaigns()
		{
			return DataAccessHelper.ExecuteList<string>("GetCallCampaigns", DataAccessHelper.Database.Csys, new SqlParameter("Region", "CompassCornerStone"));
		}
		/// <summary>
		/// Takes and account number and returns an ssn.  Current region is taken into account
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <returns>SSN for account number passed in.</returns>
		[UsesSproc(DataAccessHelper.Database.Cdw, "spGetSSNFromAcctNumber")]
		[UsesSproc(DataAccessHelper.Database.Udw, "spGetSSNFromAcctNumber")]
		public static string GetSSNFromAccountNumber(string accountNumber)
		{
			if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
				return DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw, new SqlParameter("AccountNumber", accountNumber));
			else if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
				return DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw, new SqlParameter("AccountNumber", accountNumber));
			else
				return "";
		}
	}
}
