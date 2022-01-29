using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using System.Threading;

namespace SSCHRPTFED
{
    public class SendSchoolReports : FedBatchScript
    {

        public static string TotalNumberOfFiles = "Total number of files processed.";
        public static string EOJ_EMAILS_SENT = "Total number emails sent to schools";
        public static string EOJ_MISSING_FILES = "Number missing files";
        public static string EOJ_ERROR_REPORT = "Number of records sent to error report";
        public static readonly string[] EOJ_FIELDS = { TotalNumberOfFiles, EOJ_MISSING_FILES, EOJ_EMAILS_SENT, EOJ_ERROR_REPORT };
        public static List<string> MissingFiles { get; set; }
        private ProgressBar FileProgressBar;

        public SendSchoolReports(ReflectionInterface ri)
            : base(ri, "SSCHRPTFED", "ERR_BU35", "EOJ_BU35", EOJ_FIELDS)
        {
            MissingFiles = new List<string>();
        }

        public override void Main()
        {
            StartupMessage("This is the Send School Reports Script. Click OK to continue or Cancel to quit.");

            List<SchoolInfo> sData = DataAccess.GetSchoolInfoFromDB();
            WorkReports reports = new WorkReports(RI, ProcessLogData, Recovery, Eoj);
            DialogResult result = new DialogResult();

            if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                List<string> recoveryValues = Recovery.RecoveryValue.SplitAndRemoveQuotes(",");
                if (recoveryValues.Count == 3)
                    SendSchoolReports.DeleteFile(recoveryValues[2]);

                int recoveryIndex = sData.FindIndex(p => p.SchoolCode == recoveryValues[0] && p.BranchCode == recoveryValues[1]);

                FileProgressBar = new ProgressBar(reports, sData.Skip(recoveryIndex + 1).ToList());//adding one as the found index is the last record processed we need to move to the next one
                result = FileProgressBar.ShowDialog();
            }
            else
            {
                FileProgressBar = new ProgressBar(reports, sData);
                result = FileProgressBar.ShowDialog();
            }

            FileProgressBar.Dispose();

            if (result == DialogResult.Cancel)
            {
                Recovery.Delete();
                Eoj.Delete();
                EndDllScript();
            }

            if (!CalledByJams && MissingFiles.Any())
                MessageBox.Show("The following files were missing: \n" + string.Join("\n", MissingFiles.Select(e => " - " + e).ToArray()),
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            ProcessingComplete();
        }

        public static void DeleteFile(string file)
        {
            while (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
    }
}
