using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace SCHUPDATES
{
    public class SchoolUpdatesUheaa : ScriptBase
    {
        public SchoolUpdatesUheaa(ReflectionInterface ri)
            :base(ri, "SCHUPDATES", DataAccessHelper.Region.Uheaa)
        {
            RI.LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false);
        }

        public override void Main()
        {
            new SchoolUpdates(RI, UserId, ScriptId).Process();
        }
    }
}