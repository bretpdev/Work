using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Dialog;

namespace TRDPRTYRES
{
    public class ThirdPartyAuthorization : ScriptBase
    {
        private string DataFile { get; } = $"{EnterpriseFileSystem.TempFolder}ThirdPartyDat.txt";
        public static string Script { get; set; }
        public DataAccess DA { get; set; }
        public bool OnelinkOnly { get; set; }

        public ThirdPartyAuthorization(ReflectionInterface ri)
            : base(ri, "TRDPRTYRES", DataAccessHelper.Region.Uheaa)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            RI.LogRun = RI.LogRun ?? new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, false, false);
            Script = ScriptId;
            DA = new DataAccess(RI.LogRun.LDA);
        }

        public override void Main()
        {
            bool processNext = true;
            while (processNext)
            {
                BorReferenceInfo bData = new BorReferenceInfo() { RefIsAuthed = false };
                using AcctSSNInput account = new AcctSSNInput(bData, DA);
                if (account.ShowDialog() == DialogResult.Cancel)
                    break;

                bData = account.BorData;
                OneLinkProcessing oneLink = new OneLinkProcessing(RI, DA);
                bool updatedOneLink = false;

                if (account.IsOnelink)
                {
                    if (DA.CheckLoanStatus(bData.Ssn))
                    {
                        bool? shouldProcessOl = oneLink.AddOrModifyReference(bData);
                        if (!shouldProcessOl.HasValue)
                            continue;
                        updatedOneLink = true;
                    }
                }

                CompassProcessing cProcessing = new CompassProcessing(RI, bData, DA);
                if (!cProcessing.HasOpenLoans)
                    OnelinkOnly = true;

                if (!updatedOneLink && !cProcessing.HasOpenLoans)
                {
                    string message = "Reference cannot be added to Onelink or the borrower does not have open loans in Compass.";
                    RI.LogRun.AddNotification(message, NotificationType.Other, NotificationSeverityType.Informational);
                    Info.Ok(message);
                    continue;
                }

                bool? shouldProcessCp = cProcessing.AddCompassReference(bData);
                if (!shouldProcessCp.HasValue)
                    continue;

                //Check that the address is valid.
                if (!account.Demographics.IsValidAddress)
                {
                    RI.LogRun.AddNotification("Address on file is invalid, cannot generate letter.", NotificationType.Other, NotificationSeverityType.Informational);
                    Info.Ok("Address on file is invalid, cannot generate letter.");
                    continue;
                }

                CreateLetter(account.Demographics, bData.RefIsAuthed);

                processNext = Info.YesNo("Do you want to add another reference?", "New Reference?");
            }
            RI.LogRun.LogEnd();
        }

        private void CreateLetter(SystemBorrowerDemographics demographics, bool authedRef)
        {
            string letterData = GetLetterData(demographics);
            string letterId = authedRef ? "3PACONF" : "3PADEN";
            int? success;
            if (OnelinkOnly)
                success = EcorrProcessing.AddOneLinkRecordToPrintProcessing(ScriptId, letterId, letterData, demographics.AccountNumber, "MA2324");
            else
                success = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demographics.AccountNumber, "MA2324");
            if (success.HasValue)
                Info.Ok("The letter has been created.");
            else
            {
                // Checks the Print Processing table to see if a letter has already been added today.
                // The letter is generic and only one needs to be sent per day.
                if (DA.CheckForExistingLetter(demographics.AccountNumber, letterId, ScriptId, letterData))
                    Info.Ok($"A {letterId} letter has already been sent to this borrower today. Only one letter is needed per day.");
                else
                {
                    string message = $"There was an error adding the record to Print Processing for borrower: {demographics.AccountNumber}, Letter Id: {letterId}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    Error.Ok(message);
                }
            }
            File.Delete(DataFile);
        }

        public string GetLetterData(SystemBorrowerDemographics demographics)
        {
            if (demographics.State.Length == 0 || demographics.State.ToLower() == "fc")
                demographics.State = "  ";
            string keyline = DocumentProcessing.ACSKeyLine(demographics.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            return $"{demographics.AccountNumber},{demographics.LastName},{demographics.FirstName},{demographics.Address1},{demographics.Address2},{demographics.City},{demographics.State},{demographics.ZipCode},{demographics.Country},{keyline}";
        }

    }
}