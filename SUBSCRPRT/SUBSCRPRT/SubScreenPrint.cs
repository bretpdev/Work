using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace SUBSCRPRT
{
    public class SubScreenPrint : ScriptBase
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }

        public SubScreenPrint(ReflectionInterface ri)
            : base(ri, "SUBSCRPRT", DataAccessHelper.CurrentRegion)
        {
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;

            LogRun = ri.LogRun ?? new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true);
            DA = new DataAccess(LogRun);
        }

        public override void Main()
        {
            Application.EnableVisualStyles();
#if !DEBUG
            Application.SetCompatibleTextRenderingDefault(false);

            if (!Dialog.Info.OkCancel("This application will print all the accounts in a subrogation status."))
                return;
#endif

            // Gives the user a list of installed printers to choose from for the printing.
            PrinterSelection sel = new PrinterSelection();
            if (sel.ShowDialog() != DialogResult.OK)
                return;

            DA.LoadData();
            List<PrintData> accounts = DA.GetAvailableData();
            foreach (PrintData data in accounts)
            {
                RI.FastPath("LG0HI*");
                RI.PutText(4, 42, "", true);
                RI.PutText(6, 42, "", true);
                RI.PutText(8, 42, "", true);
                RI.PutText(10, 42, data.CLUID, ReflectionInterface.Key.Enter, true);
                if (!RI.CheckForText(22, 3, "47004") && RI.CheckForText(1, 52, "DISBURSEMENT ACTIVITY DISPLAY"))
                {
                    RI.Hit(ReflectionInterface.Key.PrintScreen);
                    RI.Hit(ReflectionInterface.Key.F10);
                    while (RI.CheckForText(10, 19, "_") && RI.GetText(10, 27, 2) != "01")
                    {
                        RI.Hit(ReflectionInterface.Key.PrintScreen);
                        RI.Hit(ReflectionInterface.Key.F10);
                    }
                }
                else
                    LogRun.AddNotification($"There were no records found for borrower: {data.BF_SSN}, CLUID: {data.CLUID}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.SetProcessed(data.PrintDataId);
            }

            Dialog.Info.Ok("Processing Complete", "Complete");
            LogRun.LogEnd();
        }
    }
}