using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace REPAYTERM
{
    public class RepayTerm : ScriptBase
    {
        public RepayTerm(ReflectionInterface ri)
            : base(ri, "REPAYTERM")
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;
            ri.LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true);
            RepaymentTermForm term = new RepaymentTermForm(ri);
            term.ShowDialog();
            ri.LogRun.LogEnd();
        }
    }
}