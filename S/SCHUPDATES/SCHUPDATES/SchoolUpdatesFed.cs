using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace SCHUPDATES
{
    public class SchoolUpdatesFed : ScriptBase
    {
        public SchoolUpdatesFed(ReflectionInterface ri)
            : base(ri, "SCHUPDATES", DataAccessHelper.Region.CornerStone)
        {
            RI.LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false);
        }

        public override void Main()
        {
            new SchoolUpdates(RI, UserId, ScriptId).Process();
        }
    }
}