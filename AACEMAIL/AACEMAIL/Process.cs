using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace AACEMAIL
{
    class Process
    {
        public string ScriptId { get; set; }
        public ProcessLogData LogData { get; set; }
        public RecoveryLog Recovery { get; set; }

        public Process(string scriptId, ProcessLogData logData)
        {
            ScriptId = scriptId;
            LogData = logData;
            Recovery = new RecoveryLog("AACEmailLog");

            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(string.Format("AAC Email :: Version {0}.{1}.{2}.{3}", v.Major, v.Minor, v.Build, v.Revision));
            Console.WriteLine("");
        }

        /// <summary>
        /// Gets the file to process, loads the borrowers and starts the Update process
        /// </summary>
        /// <returns>0 if Successful, 1 if there were errors</returns>
        public int Run()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = EnterpriseFileSystem.FtpFolder;
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return 0;

                ReflectionInterface ri = GetSession();
                if (ri != null)
                {
                    List<Borrower> borrowers = LoadBorrowers(dialog.FileName).OrderBy(p => p.Ssn).ToList();
                    if (borrowers == null)
                        return 1;
                    UpdateEmail(ri, borrowers);
                }
                Recovery.Delete();
                ri.CloseSession();
#if !DEBUG
                Repeater.TryRepeatedly(() => File.Delete(dialog.FileName));
#endif
            }
            catch (Exception ex)
            {
                ProcessLogger.AddNotification(LogData.ProcessLogId, "Error in processing AACEMAIL", NotificationType.ErrorReport, NotificationSeverityType.Critical, LogData.ExecutingAssembly, ex);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Loads all the data in a List of Borrower
        /// </summary>
        /// <param name="fileName">The file to be processed</param>
        /// <returns>List<Borrower> data</returns>
        private List<Borrower> LoadBorrowers(string fileName)
        {
            var results = CsvHelper.ParseTo<Borrower>(File.ReadAllLines(fileName));
            if (results.HasErrors)
            {
                string message = results.GenerateErrorMessage();
                ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Console.WriteLine(message);
                return null;
            }
            return results.ValidLines.Select(o => o.ParsedEntity).ToList();
        }

        /// <summary>
        /// Creates a Reflection Session and logins in with an available uheaa batch id
        /// </summary>
        /// <returns>ReflectionInterface if the session was created and logged in</returns>
        private ReflectionInterface GetSession()
        {
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingHelper.GetNextAvailableId(ScriptId, "BatchUheaa");

            if (helper != null)
                if (ri.Login(helper.UserName, helper.Password))
                    return ri; //Return the RI object if the login was successful

            ri.CloseSession();
            string message = "There was an error getting a batch id for login";
            ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            Console.WriteLine(message);
            return null; //Either the helper is null or the login was not successful, return null
        }

        /// <summary>
        /// Updates the Email address in CTX1J
        /// </summary>
        /// <param name="ri">The ReflectionInterface</param>
        /// <param name="borrowers">The current borrower to be processed</param>
        private void UpdateEmail(ReflectionInterface ri, List<Borrower> borrowers)
        {
            foreach (Borrower bor in borrowers)
            {
                if (bor.Ssn.ToDouble() <= Recovery.RecoveryValue.ToDouble())
                    continue;
                Console.WriteLine("Processing Borrower: {0}, new Email: {1}", bor.Ssn, bor.Email);
                ri.FastPath("TX3Z/CTX1JB;" + bor.Ssn);
                ri.Hit(ReflectionInterface.Key.F2);
                ri.Hit(ReflectionInterface.Key.F10);
                ri.PutText(9, 20, "31");
                ri.PutText(14, 10, bor.Email, ReflectionInterface.Key.Enter);
                if (ri.MessageCode != "01004")
                {
                    if (ri.MessageCode == "01005")
                        continue;
                    string message = string.Format("Error adding email: {0} for borrower: {1}", bor.Email.IsPopulated() ? bor.Email : "NULL", bor.Ssn);
                    ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine(message);
                }
                Recovery.RecoveryValue = bor.Ssn;
            }
        }
    }
}