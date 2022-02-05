using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using System.Data.SqlClient;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace ACURINTR
{
	class DataAccess
	{
		private static string loanServicingManagerId;

		/// <summary>
		/// Gets the user ID of the manager of Borrower Services.
		/// </summary>
		[UsesSproc(DataAccessHelper.Database.Bsys, "GetManagerOfBusinessUnit")]
		public static string LoanServicingManagerId(ProcessLogData LogData)
		{
				try
				{
                    loanServicingManagerId = DataAccessHelper.ExecuteSingle<string>("GetManagerOfBusinessUnit", DataAccessHelper.Database.Bsys, new SqlParameter("BusinessUnit", "Loan Servicing"));
				}
				catch (Exception)
				{
					string message = "A unique user ID for the manager of Borrower Services was not found. Please contact Process Automation for assistance.";
					Console.WriteLine(message);
					ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly());
					throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary;
				}
			return loanServicingManagerId;
		}

		/// <summary>
		/// Gets a list of sources and their associated locate types,
		/// OneLINK source codes, and COMPASS source codes from BSYS.
		/// </summary>
		[UsesSproc(DataAccessHelper.Database.Bsys, "GetSourceLocateTypes")]
		public static List<SystemCode> SystemCodes()
		{
			return DataAccessHelper.ExecuteList<SystemCode>("GetSourceLocateTypes", DataAccessHelper.Database.Bsys);
		}

		/// <summary>
		/// Gets a list of the queues to process and methods for parsing and processing them.
		/// </summary>
		[UsesSproc(DataAccessHelper.Database.Bsys, "GetAccurintrQueues")]
		public static List<QueueData> GetQueues()
		{
			return DataAccessHelper.ExecuteList<QueueData>("GetAccurintrQueues", DataAccessHelper.Database.Bsys);
		}

		/// <summary>
		/// Gets a list of locate-related queue names from BSYS.
		/// </summary>
		[UsesSproc(DataAccessHelper.Database.Bsys, "GetLocateQueues")]
		public static List<string> LocateQueues()
		{
			return DataAccessHelper.ExecuteList<string>("GetLocateQueues", DataAccessHelper.Database.Bsys);
		}

		/// <summary>
		/// Gets a list of demographics sources, reject reasons, and their associated
		/// action codes for address and phone processing from BSYS.
		/// </summary>
		[UsesSproc(DataAccessHelper.Database.Bsys, "GetRejectActionsForSource")]
		public static List<RejectAction> GetRejectActions(string demographicsSource)
		{
			return DataAccessHelper.ExecuteList<RejectAction>("GetRejectActionsForSource", DataAccessHelper.Database.Bsys, new SqlParameter("DemographicsSource", demographicsSource));
		}

		/// <summary>
		/// Gets a list of state codes from BSYS.
		/// </summary>
		[UsesSproc(DataAccessHelper.Database.Bsys, "GetStateCodes")]
		public static List<string> StateCodes()
		{
			return DataAccessHelper.ExecuteList<string>("GetStateCodes", DataAccessHelper.Database.Bsys);
		}
	}
}
