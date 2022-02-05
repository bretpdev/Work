using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace LENDERUPDT
{
	public class LENDERUPDT
	{
		private ProcessLogRun logRun { get; set; }
		private ReflectionInterface RI { get; set; }
        private DataAccess DA;
        private static string ScriptId { get { return "LENDERUPDT"; } }

		public LENDERUPDT(ProcessLogRun PLR)
		{
            logRun = PLR;
            DA = new DataAccess(PLR.ProcessLogId);
		}

 

		/// <summary>
		/// Processing method for the script
		/// </summary>
		/// <returns>0 if successful</returns>
		public int Process()
		{
			List<LenderUpdates> records = DA.GetUnprocessedLenderUpdates();
            if(records.Count == 0)
            {
                string message = string.Format("There are no records found for processing at this time."); 
                ProcessLogger.AddNotification(logRun.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return 0;
            }

            RI = new ReflectionInterface();
            var log = BatchProcessingLoginHelper.Login(logRun, RI, DA.ScriptId, "BatchUheaa");
            if (log == null) 
             { 
                string message = string.Format("Unable to login with ID: {0}", log.UserName); 
                ProcessLogger.AddNotification(logRun.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return 1;
            }

            foreach (LenderUpdates record in records)
                UpdateLenderData(record, logRun);

            RI.CloseSession();
			ProcessLogger.LogEnd(logRun.ProcessLogId);
			return 0;
		}

        /// <summary>
        /// Validates the Mod field
        /// </summary>
        /// <param name="record">Database record to process</param>
        /// <param name="logRun"></param>
        private void UpdateLenderData(LenderUpdates record, ProcessLogRun logRun)
		{
			if (record.Mod.IsIn("A","C"))//Add or change mode
				UpdateLender(record, logRun);
			else
				logRun.AddNotification(string.Format("Unknown mod field value {0} for {1}", record.Mod, record.LenderId), NotificationType.ErrorReport, NotificationSeverityType.Critical);
		}

		/// <summary>
		/// Adds/Modifies the new lender data from the record passed in.  Process Logs if unsucessful.
		/// </summary>
		/// <param name="record">Database record to process</param>
		/// <param name="logRun"></param>
		private void UpdateLender(LenderUpdates record, ProcessLogRun logRun)
        {
            if (record.Mod == "A")
                RI.FastPath("TX3Z/ATX0H" + record.LenderId + ";000");
            else
                RI.FastPath("TX3Z/CTX0H" + record.LenderId);

            if (!RI.CheckForText(1, 73, "TXX05")) // Didn't reach the screen.
            {
                logRun.AddNotification(string.Format("Failed to reach the institution demographics screen (TXX05) for lender record {0}.  Screen {1} reached instead.  Error: {2}", record.LenderUpdateId, RI.GetText(1, 73, 5), RI.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }

            string messageCode = PopulateTXX05(record);

            if (messageCode.IsIn("01004", "01005")) // Added successfully.
                DA.SetLenderUpdateProcessed(record);
            else
                logRun.AddNotification(string.Format("Attempted to add/modify a lender to compass for lender record {0} but the addition/modification was unsuccessful.  Error: {1}", record.LenderUpdateId, RI.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        private string PopulateTXX05(LenderUpdates record)
        {
            RI.PutText( 6, 19, record.FullName, true);
            RI.PutText( 7, 19, record.ShortName, true);
            RI.PutText(11, 23, record.Address1, true);
            RI.PutText(12, 23, record.Address2, true);
            RI.PutText(14, 13, record.City, true);
            RI.PutText(14, 53, record.State, true);
            RI.PutText(14, 69, record.Zip, true);
            RI.PutText(22, 10, record.Valid ? "Y" : "N", true);
            RI.PutText(22, 31, record.DateVerified.Month < 10 ? "0" + record.DateVerified.Month.ToString() : record.DateVerified.Month.ToString(), true);
            RI.PutText(22, 34, record.DateVerified.Day < 10 ? "0" + record.DateVerified.Day.ToString() : record.DateVerified.Day.ToString(), true);
            RI.PutText(22, 37, record.DateVerified.Year.ToString().SafeSubString(2, 2), Key.Enter, true);

            if (record.Mod == "A")
            {
                if(record.Type.Contains(" "))
                    record.Type = record.Type.TrimRight(" ");
                if(record.Type.Length == 1)
                    record.Type = record.Type.PadLeft(2, '0');
                if(record.Type.ToInt() == 0 || record.Type.ToInt() > 10)
                    logRun.AddNotification(string.Format("Lender Id: {0} has invalid type of {1}",record.LenderId, record.Type), NotificationType.FileFormatProblem, NotificationSeverityType.Critical);

                RI.PutText(6, 16, record.Type.TrimRight(" "), true);
                RI.PutText(11, 22, "N", true);
                RI.PutText(12, 22, "N", Key.Enter, true);
            }

            return RI.MessageCode;
        }
	}
}
