using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace UNH_25790
{
    static class Program
    {
        public static ProcessLogData LogData { get; set; }
        public static RecoveryLog Recovery { get; set; }
        public static string RecoveryFile { get; set; }

        [STAThread]
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            string scriptId = "UNH_25790";
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId)) 
                return 1;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"T:\";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return 1;
            string fileName = "UNH_25790_" + dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1, dialog.FileName.Length - 4 - (dialog.FileName.LastIndexOf("\\") + 1));
            Recovery = new RecoveryLog(fileName);
            RecoveryFile = string.Format("{0}{1}.txt", EnterpriseFileSystem.LogsFolder, fileName);
            DialogResult result = CheckRecovery(dialog.FileName);

            LogData = ProcessLogger.RegisterApplication(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());

            int count = ProcessFile(dialog.FileName, result, fileName);
            Console.WriteLine("Processing Complete");
            ProcessLogger.LogEnd(LogData.ProcessLogId);
            Recovery.Delete();

            return 0;
        }

        public static DialogResult CheckRecovery(string file)
        {
            if (Recovery.RecoveryValue.IsPopulated())
            {
                if (Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0] == RecoveryFile)
                {
                    return MessageBox.Show(string.Format("There is a recovery value in {0}.\r\n\r\nClick 'Yes' to recover.\r\nClick 'No' to start the application over.", file), "Recovery File Found", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                }
            }
            return DialogResult.Yes;
        }

        public static void UpdateRecovery(string value)
        {
            Recovery.RecoveryValue = string.Format("{0},{1}", RecoveryFile, value);
        }

        private static int ProcessFile(string fileName, DialogResult result, string recFile)
        {
            int processed = 0;
            try
            {
                if (result == DialogResult.No)
                {
                    Recovery.Delete();
                    Recovery = new RecoveryLog(recFile);
                }
                else if (result == DialogResult.Cancel)
                    return 0;


                List<BorrowerData> bors = LoadFile(fileName);

                processed = AddArc(bors);
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, LogData.ExecutingAssembly, ex);
                processed = 1;
            }

            return processed;
        }

        private static List<BorrowerData> LoadFile(string fileName)
        {
            List<BorrowerData> bors = new List<BorrowerData>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (Recovery.RecoveryValue.IsPopulated())
                    {
                        if (Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1] != line.Trim())
                            continue;
                        else if (Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1] == line.Trim())
                        {
                            Recovery.RecoveryValue = "";
                            continue;
                        }
                    }
                    BorrowerData bor = new BorrowerData();
                    bor.Ssn = line.Substring(8, 9);
                    bor.Month = line.Substring(74, 2).ToInt();
                    bor.Line = line;
                    bors.Add(bor);
                }
            }
            return bors;
        }

        private static int AddArc(List<BorrowerData> bors)
        {
            BorrowerData borrower = new BorrowerData();
            int processed = 0;
            try
            {
                foreach (BorrowerData bor in bors)
                {
                    Console.WriteLine("Processing borrower {0}", bor.Ssn);
                    borrower = bor;
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion);
                    arc.AccountNumber = bor.Ssn;
                    arc.ArcTypeSelected = ArcData.ArcType.Atd22ByBalance;
                    arc.Arc = "ISCON";
                    arc.ScriptId = "NH 28665";
                    arc.ProcessOn = DateTime.Now;
                    arc.IsReference = false;
                    arc.IsEndorser = false;
                    arc.Comment = string.Format("Borr used {0} months of IS prior to conversion. If borr converted over on IS, listed value includes time until the end of the schedule. Borr has {1} month{2} left for renewal", bor.Month, (60 - bor.Month), bor.Month > 1 ? "s" : "");
                    ArcAddResults result = arc.AddArc();
                    if (result.ArcAdded)
                        UpdateRecovery(borrower.Line);
                }
            }
            catch (Exception ex)
            {
                string comment = string.Format("Error adding comment for borrower: {0}, Arc: ISCON;\r\nError: {1}", borrower.Ssn, ex.ToString());
                ProcessLogger.AddNotification(LogData.ProcessLogId, comment, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                processed = 1;
            }
            return processed;
        }
    }
}