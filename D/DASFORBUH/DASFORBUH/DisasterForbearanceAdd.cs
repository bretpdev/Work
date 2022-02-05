using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using static DASFORBUH.ForbearanceHelper;

namespace DASFORBUH
{
    public class DisasterForbearanceAdd
    {
        private ProcessLogRun plr;
        private DataAccess da;
        private Action<string> logMessage;

        public DisasterForbearanceAdd(ProcessLogRun plr, Action<string> logMessage)
        {
            this.plr = plr;
            this.logMessage = logMessage;
            this.da = new DataAccess(new LogDataAccess(DataAccessHelper.CurrentMode, this.plr.ProcessLogId, false, true), DataAccessHelper.CurrentRegion);
        }
         public int Process(ReflectionInterface ri)
        {
            //Add new work to the tables when processing
            bool result = da.AddWork();
            if(!result)
            {
                LogError("Failed to add new work.", NotificationSeverityType.Critical);
                return 0;
            }

            return ProcessWork(ri);
        }

        public int ProcessWork(ReflectionInterface ri)
        {
            var fh = new ForbearanceHelper(ri);
            var pendingWork = da.GetPendingWork();
            logMessage(string.Format("Found {0} pending records in database.  Beginning processing.", pendingWork.Count));
            int successCount = 0;
            int errorCount = 0;
            foreach (ProcessQueueData item in pendingWork)
            {
                if (item.ForbearanceAddedAt == null)
                {
                    ForbearanceAddResults delinquencyOverrideResults = null;
                    ForbearanceAddResults forbearanceResults = null;
                    var reflectionError = "";

                    if (item.DelinquencyOverride)
                    {
                        delinquencyOverrideResults = fh.AddDelinquencyOverrideForbearance(item);
                        if(delinquencyOverrideResults != null && !delinquencyOverrideResults.ForbearanceAdded)
                        {
                            reflectionError = ri.Message;
                            if (ri.ScreenCode == "TSX31")
                                reflectionError = ri.GetText(21, 2, 40).Trim();
                        }
                    }

                    //Add the disaster forbearance
                    forbearanceResults = fh.AddForbearance(item);

                    if (forbearanceResults.ForbearanceAdded && (delinquencyOverrideResults == null || delinquencyOverrideResults.ForbearanceAdded))
                    {
                        da.MarkForbearanceAdded(item.ProcessQueueId);
                        if (item.ArcAddProcessingId != null)
                            successCount++;
                    }
                    else
                    {
                        if (!forbearanceResults.ForbearanceAdded)
                        {
                            reflectionError = ri.Message;
                            if (ri.ScreenCode == "TSX31")
                                reflectionError = ri.GetText(21, 2, 40).Trim();
                        }
                        var results = AddErrorArc(item, reflectionError);
                        if (results.ArcAdded)
                        {
                            da.MarkArcAdded(item.ProcessQueueId, results.ArcAddProcessingId);
                            LogError(forbearanceResults.ErrorMessage, NotificationSeverityType.Warning);
                        }
                        else
                        {
                            LogError($"Failed to add DASFB ARC. {forbearanceResults.ErrorMessage}");
                        }
                        errorCount++;
                        continue;
                    }
                }
                if (item.ArcAddProcessingId == null)
                {
                    var results = AddArc(item);
                    if (results.ArcAdded)
                    {
                        da.MarkArcAdded(item.ProcessQueueId, results.ArcAddProcessingId);
                        successCount++;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
            }
            logMessage($"Processing Complete.  {successCount} successful records, {errorCount} records with errors.");

            DataAccessHelper.CloseAllManagedConnections();
            return successCount > 0 ? 0 : 1; //If at least one record succeeded, return success
        }

        private void LogError(string message, NotificationSeverityType severityType = NotificationSeverityType.Critical)
        {
            plr.AddNotification(message, NotificationType.ErrorReport, severityType);
        }

        private ArcAddResults AddArc(ProcessQueueData item)
        {
            ArcData ad = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = item.AccountNumber,
                Arc = "FBAPV",
                Comment = item.BeginDate.ToShortDateString() + "," + item.ActivityComment,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                ScriptId = "DASFORBUH"
            };
            var results = ad.AddArc();
            return results;
        }

        private ArcAddResults AddErrorArc(ProcessQueueData item, string message)
        {
            ArcData ad = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = item.AccountNumber,
                Arc = "DASFB",
                Comment = "Disaster Administrative Forbearance," + message + "," + item.BeginDate.ToShortDateString() + "," + item.EndDate.ToShortDateString(),
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                ScriptId = "DASFORBUH"
            };
            var results = ad.AddArc();
            return results;
        }

    }
}
