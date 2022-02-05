using System;
using System.Collections.Generic;
using System.IO;
using Uheaa.Common.ProcessLogger;

namespace CentralizedPrintingProcess
{
    public abstract class PrintingBase
    {
        protected MiscDat MiscDat { get; set; }
        protected ProcessLogRun PLR { get; set; }
        protected IJobStatus JS { get; set; }
        protected IEmailHandler EH { get; set; }
        protected DataAccess DA { get; set; }
        protected List<string> qcErrorEmailSentFor = new List<string>();

        public PrintingBase(MiscDat miscData, ProcessLogRun plr, IJobStatus js, IEmailHandler eh, DataAccess da)
        {
            MiscDat = miscData;
            PLR = plr;
            JS = js;
            EH = eh;
            DA = da;
        }

        public abstract void ProcessPrinting();
        public abstract string PrintDirectory { get; }


        public void Process()
        {
#if !DEBUG
            try
            {
#endif
            JS.TitleColor = System.Drawing.Color.Yellow;

            //Check that print directories are available.
            //current region path
            JS.LogItem("Checking initial status...");
            string printDirectory = MiscDat.LetterDirectory;
            if (PrintDirectoriesExist(printDirectory))
            {
                JS.LogItem("Beginning Printing.");
                ProcessPrinting();
                JS.LogItem("Finished Printing.");
            }

            JS.TitleColor = System.Drawing.Color.Green;
#if !DEBUG

            }
            catch (Exception ex)
            {
                JS.LogItem("ERROR: " + ex.ToString());
                JS.TitleColor = System.Drawing.Color.Red;
                PLR.AddNotification("Error in Printing Processor.", NotificationType.HandledException, NotificationSeverityType.Critical, ex);
            }
#endif

        }


        /// <summary>
        /// Takes a directory and validates its existence.  Errors if it doesnt and process logs it
        /// </summary>
        /// <param name="directory"> path that should exist</param>
        protected bool PrintDirectoriesExist(string directory)
        {
            if ((!Directory.Exists(directory)))
            {
                string message = $"The \"{directory}\" directory appears to be unavailable.  Please try again when then network problems have been fixed.";
                JS.LogItem(message);
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Write the error to Process Logger and the Job Status Control
        /// </summary>
        protected void LogError(string error, NotificationSeverityType severity = NotificationSeverityType.Warning)
        {
            PLR.AddNotification(error, NotificationType.ErrorReport, severity);
            JS.LogItem(error);
        }
    }
}