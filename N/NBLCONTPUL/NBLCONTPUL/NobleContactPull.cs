using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;

namespace NBLCONTPUL
{
	public class NobleContactPull
	{
		public ProcessLogData LogData { get; set; }

		public string ScriptId
		{
			get { return "NBLCONTPUL"; }
		}

		/// <summary>
		/// Main entry point for program.  
		/// Depending on Command Line args, this app will either insert calls made for the day into CLS and ULS, 
		/// or bring up a UI to edit call campaigns (given args[1] = "UI")
		/// </summary>
		/// <param name="args">Command Line Args</param>
		/// <returns>0 for successful run</returns>
		public static int Main(string[] args)
		{
			if (!DataAccessHelper.StandardArgsCheck(args, "NBLCONTPUL"))
				return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
				return 1;
			
			DataAccessHelper.CurrentRegion = DataAccessHelper.Region.None;
			
			if (args[1].ToUpper() == "UI") //Allow user to change call campaigns and add new call campaigns
			{
				new UpdateCampaigns().ShowDialog();
			}
			else //command line processing for inserts into database
			{
				new NobleContactPull().Process();
			}
			return 0;
		}

		/// <summary>
		/// Starts off 3 threads inserting records into the database for each region
		/// </summary>
		public void Process()
		{
			LogData = ProcessLogger.RegisterApplication(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
			DataAccess.GetNobleUsernameRelation(LogData); //pull TSRMASTER_SEC down from noble
			var oneLink = Task.Factory.StartNew(() => InsertCalls(DataAccess.Region.OneLink));
			var uheaa = Task.Factory.StartNew(() => InsertCalls(DataAccess.Region.Uheaa));
			var cornerStone = Task.Factory.StartNew(() => InsertCalls(DataAccess.Region.CornerStone));
			Task.WhenAll(oneLink, uheaa, cornerStone).Wait();
		}

		/// <summary>
		/// Gets a list of calls from Noble and inserts the data into either CLS or ULS based on region.
		/// </summary>
		/// <param name="region">enum CornerStone, OneLink, or Uheaa</param>
		private void InsertCalls(DataAccess.Region region)
		{
			List<NobleData> calls = DataAccess.LoadNobleData(region, LogData);
			foreach (NobleData record in calls)
			{
				
				NobleDataTrim(record);
				if (record.AgentCode.IsPopulated())
				{
					record.AgentCode2 = DataAccess.GetOtherUserId(record.AgentCode);
					record.AgentName = DataAccess.GetAgentName(record.AgentCode);
				}
				int campaignId = DataAccess.GetContactCampaignId(record, region, LogData); 
				DataAccess.InsertCalls(record, region, region == DataAccess.Region.CornerStone ? DataAccessHelper.Database.Cls : DataAccessHelper.Database.Uls, campaignId, LogData);
				Console.WriteLine(string.Format("ContactCall record added to {0} region for accountnumber: {1}", region.ToString(), record.AccountNumber));
			}
		}

		/// <summary>
		/// Takes a data record and trims each property
		/// </summary>
		/// <param name="record">Noble Data record</param>
		public void NobleDataTrim(NobleData record)
		{
			var info = typeof(NobleData).GetProperties();
			foreach (PropertyInfo property in info)
			{
				if (property.GetValue(record, null) != null && !property.Name.IsIn("PhoneNumber", "SSN", "AccountNumber"))
				{
					property.SetValue(record, property.GetValue(record, null).ToString().Trim(), null);
				}
			}
		}
	}
}
