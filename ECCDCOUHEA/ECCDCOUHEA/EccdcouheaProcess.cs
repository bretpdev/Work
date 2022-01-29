using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ECCDCOUHEA
{
    public class EccdcouheaProcess
    {
        public string path {get;}
        public ProcessLogRun logRun { get; set; }
        public LogDataAccess LDA { get; set; }

        public EccdcouheaProcess(ProcessLogRun logRun, string path)
        {
            this.logRun = logRun;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, false);
            this.path = path;
        }

        public int Process()
        {
            FileLoad file = new FileLoad(path, logRun);
            List<ArcData> arcs = file.ParseCsv(file.PickCsv());
            foreach(ArcData arc in arcs)
            {
                ArcAddResults result = arc.AddArc();
                if(!result.ArcAdded)
                {
                    logRun.AddNotification(string.Format("Unable to add Arc for ECCDCOUHEA Arc:{0} AccountNumber:{1} RecipientId:{2} Comment:{3}",
                    arc.Arc, arc.AccountNumber, arc.RecipientId, arc.Comment), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }
            return 0;
        }
    }
}
