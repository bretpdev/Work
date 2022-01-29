using RFCOMAPILib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CentralizedPrintingProcess
{
    public class Fax
    {
        protected const string FAX_SERVER_NAME = "Rightfax";
        protected MiscDat MiscDat { get; set; }
        protected List<string> qcErrorEmailSentFor = new List<string>();
        private ProcessLogRun PLR { get; set; }
        protected IJobStatus JS { get; set; }
        protected DataAccess DA { get; set; }
        protected IEmailHandler EH { get; set; }
        protected ReflectionInterface RI { get; set; }

        public Fax(MiscDat misc, ProcessLogRun plr, IJobStatus js, IEmailHandler eh, DataAccess da)
        {
            MiscDat = misc;
            PLR = plr;
            JS = js;
            EH = eh;
            DA = da;
        }

        /// <summary>
        /// Remove files older than 30 days, verify that each fax document also has a database record, use rightfax to process faxes, and create EOJ reports.
        /// </summary>
        public void Process()
        {
            //Create session and log in with batch id
            RI = new ReflectionInterface();
            BatchProcessingLoginHelper.Login(PLR, RI, MiscDat.ScriptId, "BatchUheaa");
            if (!RI.IsLoggedIn)
            {
                LogError("An error was encountered while trying to log in to OneLINK and COMPASS.", NotificationSeverityType.Critical);
                return;
            }
#if !DEBUG
            try
            {
#endif
                JS.TitleColor = System.Drawing.Color.Yellow;

                //purging of network folder removing any docs older than 30 days
                PurgeDirectories();

                JS.LogItem("Checking initial status...");
                if (FaxDirectoriesExist(MiscDat.FaxDirectory))
                {
                    //initialize data for sending faxes
                    FaxServer rightFax = new FaxServer();
                    rightFax.ServerName = FAX_SERVER_NAME; //constant is "Rightfax"
                    rightFax.Protocol = CommunicationProtocolType.cpTCPIP;
                    rightFax.UseNTAuthentication = BoolType.True;
                    rightFax.OpenServer();
                    JS.LogItem($"Opened fax server, version {rightFax.Version}");

                    JS.LogItem("Beginning faxing.");
                    ProcessFaxing(rightFax);
                    JS.LogItem("Finished faxing.");

                    rightFax.CloseServer();
                    JS.LogItem("Closed fax server.");
                }

                JS.TitleColor = System.Drawing.Color.Green;
#if !DEBUG
            }
            catch (Exception ex)
            {
                JS.LogItem("ERROR: " + ex.ToString());
                JS.TitleColor = System.Drawing.Color.Red;
                PLR.AddNotification("Error in Faxing Processor.", NotificationType.HandledException, NotificationSeverityType.Critical, ex);
            }
#endif
            RI.CloseSession();

        }
        /// <summary>
        /// Print faxes, update the database records, and verify that past faxes have been received correctly.
        /// </summary>
        protected void ProcessFaxing(FaxServer rightFax)
        {
            List<PendingFaxConfirmation> pendingConfirmation = new List<PendingFaxConfirmation>();

            //retrieve fax information from DB
            List<FaxRecord> faxes = DA.GetUnprocessedFaxRecords();
            //check if documents are on network
            foreach (FaxRecord record in faxes)
            {
                string recordData = record.ToString();
                string cvrDoc = Path.Combine(MiscDat.FaxDirectory, record.SeqNum + "_CVR.doc");
                if (!File.Exists(cvrDoc))
                    cvrDoc += "x";
                string faxDoc = Path.Combine(MiscDat.FaxDirectory, record.SeqNum + "_FAX.doc");
                if (!File.Exists(cvrDoc))
                    faxDoc += "x";
                //Check to make sure we have both the cover sheet and the document
                if (!File.Exists(cvrDoc) || !File.Exists(faxDoc))
                    MissingCVRorFax(record, recordData); //either the CVR or the Fax doc did not exist
                else
                {
                    var faxHandle = FaxIt(cvrDoc, faxDoc, record, rightFax); //both documents found so do faxing
                    pendingConfirmation.Add(new PendingFaxConfirmation(faxHandle, cvrDoc, faxDoc, record));
                }
                CheckPendingFaxStatuses(pendingConfirmation, rightFax);
            }

            JS.LogItem("Waiting on pending faxes...");
            while (pendingConfirmation.Any())
            {
                Thread.Sleep(1000);
                CheckPendingFaxStatuses(pendingConfirmation, rightFax);
            }
            JS.LogItem("All pending faxes finished.");
        }

        /// <summary>
        /// Check the status of pending Faxes
        /// </summary>
        private void CheckPendingFaxStatuses(List<PendingFaxConfirmation> pending, FaxServer rightFax)
        {
            var existingFaxes = rightFax.get_Faxes(Environment.UserName.ToLower()).Cast<RFCOMAPILib.Fax>();
            foreach (var pendingRecord in pending.ToArray())
            {
                var faxRecord = existingFaxes.Where(o => o.Handle == pendingRecord.FaxHandle).Single();
                if (faxRecord.FaxStatus == FaxStatusType.fsDoneOK)
                {
                    Repeater.TryRepeatedly(() => File.Delete(pendingRecord.CvrDoc));
                    Repeater.TryRepeatedly(() => File.Delete(pendingRecord.FaxDoc));
                    //update system with notes
                    AddSuccessfulFaxCommentsToSys(pendingRecord.FaxRecord);
                    DA.MarkFaxRecordFaxed(pendingRecord.FaxRecord);
                    pending.Remove(pendingRecord);
                }
                else if (faxRecord.FaxStatus == FaxStatusType.fsDoneError || faxRecord.FaxStatus == FaxStatusType.fsError)
                {
                    LogError($"Error sending fax for record {pendingRecord.FaxRecord}: {faxRecord.StatusDescription}");
                    pending.Remove(pendingRecord);
                    //send error email
                    string attachments = pendingRecord.CvrDoc + "," + pendingRecord.FaxDoc;
                    EH.AddEmail("See Attached Document", "Failed Fax Acct Num: " + pendingRecord.FaxRecord.AccountNumber.Replace(" ", ""), pendingRecord.FaxRecord.BusinessUnit, "", attachments);
                    //process errors
                    ErrorProc(pendingRecord.FaxRecord, pendingRecord.FaxRecord.ToString());
                    DA.MarkFaxRecordDeleted(pendingRecord.FaxRecord.SeqNum);
                }
            }
        }

        /// <summary>
        /// Add an error record for the document and send out an email
        /// </summary>
        /// <param name="record">Date row that errored</param>
        /// <param name="recordData">Comment</param>
        private void MissingCVRorFax(FaxRecord record, string recordData)
        {
            //check if letter ID is already had a email sent for it
            if (!qcErrorEmailSentFor.Contains(record.LetterID))
            {
                //send error email
                string message = recordData + " failed to generate.  Please ensure that proper measures are taken to deliver the fax to the borrower quickly, and to prevent the problem from happening again.";
                EH.AddEmail(message, "Document failing to generate", record.BusinessUnit, "CentralPrintingQCError");
                LogError(message, NotificationSeverityType.Critical);
                //add letter ID to array list so mulitple email aren't sent out
                qcErrorEmailSentFor.Add(record.LetterID);
                DA.MarkFaxRecordDeleted(record.SeqNum);
            }

            ErrorProc(record, recordData);
            DA.MarkFaxRecordDeleted(record.SeqNum);
        }

        /// <summary>
        /// Generate fax and send it to server.
        /// </summary>
        /// <param name="record">Database record for current fax</param>
        /// <param name="server">Server to create the fax object from</param>
        private int FaxIt(string cvrLocation, string faxLocation, FaxRecord record, FaxServer server)
        {
            RFCOMAPILib.Fax faxer = (RFCOMAPILib.Fax)server.get_CreateObject(CreateObjectType.coFax);
            //send to real fax number if not in test mode else send to internal fax
            faxer.ToFaxNumber = "9" + record.FaxNumber;
            faxer.ToName = faxer.ToFaxNumber;
            faxer.HasCoversheet = BoolType.False;
            //add cover sheet and fax
            faxer.Attachments.Add(cvrLocation);
            faxer.Attachments.Add(faxLocation);
            faxer.Send();
            //update DB with RightFax handle
            return faxer.Handle;
        }

        /// <summary>
        /// Log errors in faxing to The reflection interface
        /// </summary>
        /// <param name="record">Database record for current fax</param>
        /// <param name="recordData">Comment to leave in reflection interface</param>
        private void ErrorProc(FaxRecord record, string recordData)
        {
            //add fax failing comments to originating system(s) only
            var originatingSystem = (OriginationSystem)Enum.Parse(typeof(OriginationSystem), record.CommentsAddedTo, true);
            string comment = record.LetterID + " failed to fax successfully";
            //add activity record for borrower
            string ssn;
            try
            {
                ssn = RI.GetDemographicsFromLP22(record.AccountNumber.Replace(" ", "")).Ssn;
            }
            catch (DemographicException)
            {
                if (originatingSystem == OriginationSystem.Compass || originatingSystem == OriginationSystem.Both)
                    RI.Atd22AllLoans(record.AccountNumber.Replace(" ", ""), "FFAIL", comment, "", "CNTRPRNT", false);
                if (!RI.Atd22AllLoans(record.AccountNumber.Replace(" ", ""), DA.ARCsAndQueuesForBusinessUnits(record.BusinessUnit, CentralizedPrintingErrorType.CFaxingErrArc), recordData, "", "CNTRPRNT", false))
                {
                    //if COMPASS queue task can't be added then send and email
                    string message = $"There was an error in adding a queue task for [[{recordData}]].  Please take the necessary steps to ensure the follow up queue task is created.  {RI.Message}";
                    LogError(message, NotificationSeverityType.Critical);
                    EH.AddEmail(message, "Error in adding queue task for failed faxing", record.BusinessUnit, "CentralPrintingSysError");
                }
                return;
            }
            //try and add queue tasks
            if (!RI.AddQueueTaskInLP9O(ssn, DA.ARCsAndQueuesForBusinessUnits(record.BusinessUnit, CentralizedPrintingErrorType.OLFaxingErrQueue), null, recordData, "", "", ""))
            {
                //if onelink queue task can't be added then try COMPASS
                if (!RI.Atd22AllLoans(ssn, DA.ARCsAndQueuesForBusinessUnits(record.BusinessUnit, CentralizedPrintingErrorType.CFaxingErrArc), recordData, "", "CNTRPRNT", false))
                {
                    //if COMPASS queue task can't be added then send and email
                    string message = $"There was an error in adding a queue task for [[{record}]].  Please take the necessary steps to ensure the follow up queue task is created. {RI.Message}";
                    LogError(message, NotificationSeverityType.Critical);
                    EH.AddEmail(message, "Error in adding queue task for failed faxing", record.BusinessUnit, "CentralPrintingSysError");
                }
            }

            if (originatingSystem == OriginationSystem.Compass || originatingSystem == OriginationSystem.Both)
                RI.Atd22AllLoans(record.AccountNumber.Replace(" ", ""), "FFAIL", comment, "", "CNTRPRNT", false);

            if (originatingSystem == OriginationSystem.OneLink || originatingSystem == OriginationSystem.Both)
                RI.AddCommentInLP50(ssn, "FA", "03", "FFAIL", comment, "CNTRPRNT");
        }

        /// <summary>
        /// create FSENT arc for successful faxes
        /// </summary>
        /// <param name="record"> Account to update </param>
        private void AddSuccessfulFaxCommentsToSys(FaxRecord record)
        {
            var originatingSystem = (OriginationSystem)Enum.Parse(typeof(OriginationSystem), record.CommentsAddedTo, true);
            string comment = record.LetterID + " was faxed successfully";
            string ssn;
            try
            {
                ssn = RI.GetDemographicsFromLP22(record.AccountNumber.Replace(" ", "")).Ssn;
            }
            catch (DemographicException)
            {
                if (originatingSystem == OriginationSystem.Compass || originatingSystem == OriginationSystem.Both)
                    RI.Atd22AllLoans(record.AccountNumber.Replace(" ", ""), "FSENT", comment, "", "CNTRPRNT", false);
                return;
            }

            if (originatingSystem == OriginationSystem.Compass || originatingSystem == OriginationSystem.Both)
                RI.Atd22AllLoans(record.AccountNumber.Replace(" ", ""), "FSENT", comment, "", "CNTRPRNT", false);

            if (originatingSystem == OriginationSystem.OneLink || originatingSystem == OriginationSystem.Both)
                RI.AddCommentInLP50(ssn, "FA", "03", "FSENT", comment, "CNTRPRNT");
        }

        /// <summary>
        /// Removes files older than 30 days old from the Fax Directory so they are not sent
        /// </summary>
        private void PurgeDirectories()
        {
            foreach (string fileName in Directory.GetFiles(MiscDat.FaxDirectory).Where(p => File.GetCreationTime(p) < DateTime.Now.AddDays(-30)))
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// Takes an Directory and validates its existence.  Errors if it doesnt and process logs it
        /// </summary>
        /// <param name="directory"> path that should exist</param>
        private bool FaxDirectoriesExist(string directory)
        {
            if (!Directory.Exists(directory))
            {
                string message = $"The \"{directory}\" directory appears to be unavailable.  Please try again when then network problems have been fixed.";
                MessageBox.Show(message, "Network Problems", MessageBoxButtons.OK, MessageBoxIcon.Error);
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