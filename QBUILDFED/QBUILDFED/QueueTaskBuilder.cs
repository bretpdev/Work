using System;
using System.Collections.Generic;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace QBUILDFED
{
    public class QueueTaskBuilder
    {
        //Recovery value is SAS file path/name, SSN, ARC.

        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public ReflectionInterface RI { get; set; }
        public RecoveryLog Recovery { get; set; }
        public FileErrors Errors { get; set; }
        public string ScriptId { get; set; }

        public QueueTaskBuilder(ProcessLogRun logRun, string scriptId)
        {
            LogRun = logRun;
            ScriptId = scriptId;
            Recovery = new RecoveryLog($"{scriptId}_{DateTime.Now.Date.To6DigitDate()}");
        }

        public int Process()
        {
            DA = new DataAccess(LogRun.LDA);
            List<SasInstructions> sasList = DA.GetSasList();

            //See if we need to recover.
            if (!Recovery.RecoveryValue.IsNullOrEmpty())
                Recover();

            Errors = new FileErrors(LogRun);
            //If recovery existed it is done. Check that all of the needed files are present and populated.
            Errors.CheckForFileErrorConditions(sasList);

            //Process all of the files.
            foreach (SasInstructions sas in sasList)
                ProcessFiles(sas.FileName);

            //had errors while processing files...
            if (Errors.HasErrors)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Gets the file needed to recover and processes that file.
        /// </summary>
        private void Recover()
        {
            string recoverySas = Recovery.RecoveryValue.Split(',')[0];
            WorkFile(recoverySas);
            Repeater.TryRepeatedly(() => FS.Delete(recoverySas));
        }

        /// <summary>
        /// Processes a given SAS file.
        /// </summary>
        /// <param name="sasFileName">Sas file to process.</param>
        private bool ProcessFiles(string sasFileName)
        {
            Console.WriteLine($"Processing {sasFileName}");
            string[] foundFiles = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, sasFileName);
            foreach (string foundFile in foundFiles)
            {
                if (new FileInfo(foundFile).Length != 0)
                {
                    if (Errors.CheckFileFormat(foundFile))
                    {
                        WorkFile(foundFile);
                        Repeater.TryRepeatedly(() => FS.Delete(foundFile));
                    }
                }
                else
                    Repeater.TryRepeatedly(() => FS.Delete(foundFile));
            }

            return (foundFiles.Length > 0);
        }

        /// <summary>
        /// Reads in the SAS file to process it.
        /// </summary>
        private void WorkFile(string sasFile)
        {
            using (StreamReader sasReader = new StreamR(sasFile))
            {
                //Move to the recovery point if needed.
                if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
                {
                    string[] recoveryValues = Recovery.RecoveryValue.Split(',');
                    string recoverySsn = recoveryValues[1];
                    string recoveryArc = recoveryValues[2];
                    string ssn = "";
                    string arc = "";
                    while (ssn != recoverySsn || arc != recoveryArc)
                    {
                        List<string> fields = sasReader.ReadLine().SplitAndRemoveQuotes(",");
                        ssn = fields[0];
                        arc = fields[1];
                    }
                }

                //Process the rest of the file.
                while (!sasReader.EndOfStream)
                    ProcessRecord(sasReader.ReadLine(), sasFile);

                //we are done processing this file clear recovery for the next file.
                Recovery.Delete();
            }
        }

        /// <summary>
        /// Processes a record in the SAS file.
        /// </summary>
        /// <param name="sasLine">file line to process.</param>
        /// <param name="sasFile">the file we are processing.</param>
        private void ProcessRecord(string sasLine, string sasFile)
        {
            QueueBuilderRecord record = new QueueBuilderRecord();
            record = record.Populate(sasLine, DA);
            bool result = false;
            if (record.LoanSequences == null)
                result = AddArc(record.SSN, record.ARC, record.Comment, record.RecipientId, ScriptId, record.RegardsToId, ArcData.ArcType.Atd22AllLoans);
            else
                result = AddArc(record.SSN, record.ARC, record.Comment, record.RecipientId, ScriptId, record.RegardsToId, ArcData.ArcType.Atd22ByLoan, record.LoanSequences);

            if (!result)
            {
                string byLoan = record.LoanSequences != null ? "true" : "false";
                string errorMessage = $"Error trying to add Arc: {record.ARC} AddArcByLoan: {byLoan}";
                AddError(record.SSN, sasLine, sasFile, errorMessage, NotificationSeverityType.Critical);
            }

            Recovery.RecoveryValue = $"{sasFile},{record.SSN},{record.ARC}";
        }

        private bool AddArc(string ssn, string arc, string comment, string recipient, string scriptId, string regardsTo, ArcData.ArcType arcType, List<int> loans = null)
        {
            ArcData ad = new ArcData(DataAccessHelper.Region.CornerStone);
            {
                ad.AccountNumber = ssn;
                ad.Arc = arc;
                ad.ArcTypeSelected = arcType;
                ad.Comment = comment;
                ad.RecipientId = recipient;
                ad.ScriptId = scriptId;
                ad.RegardsTo = regardsTo;
                ad.IsEndorser = false;
                ad.IsReference = false;
                ad.LoanSequences = loans;
            };

            return ad.AddArc().ArcAdded;
            
        }

        /// <summary>
        /// Adds a record to the error report.
        /// </summary>
        private void AddError(string ssn, string sasLine, string sasFile, string errorInformation, NotificationSeverityType severity)
        {
            string errorLine = sasLine;
            string accountNumber = ssn;
            //Using the original text from the SAS file, replace the SSN with
            //the account number and write the line out to the error file.
            if (ssn.Length == 9)//Some of the queue builder files have ssns some have account numbers.
            {
                accountNumber = DA.GetAccountNumberFromSsn(ssn);
                int indexOfSsn = (errorLine.StartsWith("\"") ? 1 : 0);
                errorLine = errorLine.Remove(indexOfSsn, 9);
                errorLine = errorLine.Insert(indexOfSsn, accountNumber);
            }
            LogRun.AddNotification($"An Error occurred for the following line in the file {sasFile}: \n {errorLine}, Account Number: {accountNumber} Info: {errorInformation}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }
    }
}