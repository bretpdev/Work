using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace FORBPROFED
{
    public class ArcProcessor
    {
        public static void AddArcError(ForbProcessingRecord record, DataAccess da, ProcessLogRun logRun, string comment, List<int> loans)
        {
            BusinessUnits bu = da.GetBusinessUnit(record.BusinessUnitId);
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion);
            {
                arc.ArcTypeSelected = loans == null ? ArcData.ArcType.Atd22AllLoans : ArcData.ArcType.Atd22ByLoan;
                arc.ResponseCode = null;
                arc.AccountNumber = record.AccountNumber;
                arc.RecipientId = null;
                arc.Arc = bu.ARC;
                arc.ScriptId = Program.ScriptId;
                arc.ProcessOn = DateTime.Now;
                arc.Comment = comment;
                arc.ProcessFrom = null;
                arc.ProcessTo = null;
                arc.NeedBy = null;
                arc.RegardsTo = null;
                arc.RegardsCode = null;
                arc.LoanSequences = loans;
            }

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                logRun.AddNotification("Unable to add arc for ForbearanceProcessingId: " + record.ForbearanceProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
            bool set = da.SetProcessedOn(record.ForbearanceProcessingId, result.ArcAddProcessingId);
            if (!set)
            {
                logRun.AddNotification("Unable to set record processed, ForbearanceProcessingId: " + record.ForbearanceProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
        }

        public static void AddArcSuccess(ForbProcessingRecord record, DataAccess da, ProcessLogRun logRun, List<int> loans)
        {
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion);
            {
                arc.ArcTypeSelected = loans == null ? ArcData.ArcType.Atd22AllLoans : ArcData.ArcType.Atd22ByLoan;
                arc.ResponseCode = null;
                arc.AccountNumber = record.AccountNumber;
                arc.RecipientId = null;
                arc.Arc = "FBAPV";
                arc.ScriptId = Program.ScriptId;
                arc.ProcessOn = DateTime.Now;
                arc.Comment = string.Format("Forbearance added for {0} through {1} Forbearance Type: {2}", record.StartDate.ToShortDateString(), record.EndDate.ToShortDateString(), record.ForbearanceType);
                arc.ProcessFrom = null;
                arc.ProcessTo = null;
                arc.NeedBy = null;
                arc.RegardsTo = null;
                arc.RegardsCode = null;
                arc.LoanSequences = loans;
            }

            ArcAddResults result = arc.AddArc();
            if(!result.ArcAdded)
            {
                logRun.AddNotification("Unable to add arc for ForbearanceProcessingId: " + record.ForbearanceProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
            bool set = da.SetProcessedOn(record.ForbearanceProcessingId, result.ArcAddProcessingId);
            if (!set)
            {
                logRun.AddNotification("Unable to set record processed, ForbearanceProcessingId: " + record.ForbearanceProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
        }
    }
}
