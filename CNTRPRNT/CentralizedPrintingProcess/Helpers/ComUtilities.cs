using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CentralizedPrintingProcess
{
    class ComUtilities
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public IEmailHandler Email { get; set; }
        public Printing Printer { get; set; }
        public Fax Fax { get; set; }

        public ComUtilities(MiscDat md, IJobStatus printingStatus, IJobStatus faxStatus, IEmailHandler emailHandler = null)
        {
            var scriptId = CentralizedPrinting.ScriptId;
            var domain = AppDomain.CurrentDomain;
            var assembly = Assembly.GetExecutingAssembly();
            var mode = DataAccessHelper.CurrentMode;
            LogRun = new ProcessLogRun(scriptId, domain, assembly, DataAccessHelper.Region.Uheaa, mode, false, true);
            DA = new DataAccess(LogRun.LDA);
            Email = emailHandler ?? new InstantEmailHandler(DA);
            Printer = new Printing(md, LogRun, printingStatus, Email, DA);
            Fax = new Fax(md, LogRun, faxStatus, Email, DA);
        }
    }
}