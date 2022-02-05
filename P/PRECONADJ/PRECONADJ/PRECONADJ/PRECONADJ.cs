using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PRECONADJ
{
    public class PRECONADJ : ScriptBase
    {
        public PRECONADJ(ReflectionInterface ri)
            : base(ri, "PRECONADJ", DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            RI.LogRun = RI.LogRun ?? new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false);
            new PreConversionAdjustment(RI, ScriptId).Process();
            RI.LogRun.LogEnd();
        }
    }
}