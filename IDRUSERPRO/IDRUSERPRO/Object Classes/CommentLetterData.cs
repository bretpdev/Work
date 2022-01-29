using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace IDRUSERPRO
{
    public class CommentLetterData
    {
        public int HisId { get; set; }
        public string Arc { get; set; }
        public string Comment { get; set; }
        public string LetterId { get; set; }
        public string LetterMergeText { get; set; }
        public string CoBorrowerArc { get; set; }
        public string CoBorrowerComment { get; set; }
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }

        public CommentLetterData() { }

        public CommentLetterData(DataAccess da, ProcessLogRun plr)
        {
            DA = da;
            PLR = plr;
        }

        private enum LetterStatus
        {
            BadAddress,
            Success,
            Retry
        }

        public bool GenerateArcAndLetter(ReflectionInterface ri, ApplicationEntry appEntry, RecoveryLog recovery, SystemBorrowerDemographics bData, List<int> loanSeq, string scriptId, string userId, bool waitingForNSLDS)
        {

            CommentLetterData data = DA.GetArcAndLetterdata(appEntry.UserInputedData.ApplicationId);

            if (appEntry.UserInputedData.MissingDocData.Any())
            {
                data.Comment = string.Format(data.Comment, string.Join("; ", appEntry.UserInputedData.MissingDocData)).Replace(".", "");
                List<string> missingDocs = new List<string>();
                foreach (string item in appEntry.UserInputedData.MissingDocData)
                {
                    missingDocs.Add("- " + item);
                }

                appEntry.UserInputedData.MissingDocData = missingDocs;
                for (int len = appEntry.UserInputedData.MissingDocData.Count; appEntry.UserInputedData.MissingDocData.Count < 5; len++)
                {
                    appEntry.UserInputedData.MissingDocData.Add("");
                }
            }

            try
            {
                if (recovery.RecoveryValue.Contains("IDR Added"))
                {
                    if (!data.Arc.IsNullOrEmpty())
                    {
                        if (waitingForNSLDS)
                            data.Comment = "Waiting for NSLDS Info.";
                        AddComment(ri, appEntry.Approved, bData.Ssn, loanSeq, data, appEntry.UserInputedData.ApplicationId.Value, scriptId, appEntry.FamilySizeHold);
                    }

                    recovery.RecoveryValue = string.Format("{0},Comment Added", appEntry.UserInputedData.ApplicationId);
                    if (waitingForNSLDS)
                        return true;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Unable to add the follow arc to the borrowers account.  ARC:{0}, ApplicationId:{1}", data.Arc, appEntry.UserInputedData.ApplicationId);
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                MessageBox.Show("There was an error adding the activity comment to the borrowers account.  Please ensure the activity record is added. \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
#if !DEBUG
            try
#endif
            {

                if (recovery.RecoveryValue.Contains("Comment Added") || recovery.RecoveryValue.Contains("Letter Generated"))
                {
                    if (!data.LetterId.IsNullOrEmpty())
                    {
                        if (data.LetterMergeText != null)
                        {
                            if (data.LetterMergeText.Contains("entered by user"))
                            {
                                while (!GetComment(data))
                                    continue;

                                this.Comment = data.LetterMergeText = data.LetterMergeText.Replace(Environment.NewLine, "");
                            }
                        }

                         if (GenerateAndImageLetter(ri, data, bData, recovery, scriptId, appEntry.UserInputedData.ApplicationId.Value, PLR.ProcessLogId, userId, appEntry, false, appEntry.AppState.Loans.EligibleLoans) == LetterStatus.Retry)
                            GenerateAndImageLetter(ri, data, ri.GetDemographicsFromTx1j(bData.AccountNumber), recovery, scriptId, appEntry.UserInputedData.ApplicationId.Value, PLR.ProcessLogId, userId, appEntry, false, appEntry.AppState.Loans.EligibleLoans, true);

                        //if (appEntry.AppState.Loans.EligibleLoans.Where(p => p.LoanType.IsIn("DLSSPL", "DLUSPL", "UNSPC", "SUBSPC", "PLUSGB")).Count() != 0)
                        //{
                        List<SystemBorrowerDemographics> endorserDemos = GetEndorserDemos(ri, bData.Ssn);
                        //if (EndorserAtTheSameAddressAsBorrower(bData, endorserDemos))
                        //    return true;

                        if(endorserDemos == null || endorserDemos.Count == 0)
                        {
                            return true;
                        }

                        //Reset recovery so that the letter will generate.
                        foreach(SystemBorrowerDemographics demos in endorserDemos)
                        {
                            recovery.RecoveryValue = "Comment Added";
                            if (GenerateAndImageLetter(ri, data, demos, recovery, scriptId, appEntry.UserInputedData.ApplicationId.Value, PLR.ProcessLogId, userId, appEntry, true, appEntry.AppState.Loans.EligibleLoans, false, bData.Ssn) == LetterStatus.Retry)
                                GenerateAndImageLetter(ri, data, demos, recovery, scriptId, appEntry.UserInputedData.ApplicationId.Value, PLR.ProcessLogId, userId, appEntry, true, appEntry.AppState.Loans.EligibleLoans, true, bData.Ssn);
                        }
                        //}
                    }
                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                string message = string.Format("Unable to generate Letter:{0} for ApplicationId:{1}", data.LetterId, appEntry.UserInputedData.ApplicationId);
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                MessageBox.Show(string.Format("There was an error generating Letter Id {0} in print processing.  Please ensure this letter is generated and imaged.", data.LetterId) + "\n\n" + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
#endif

            return true;
        }

        private static bool EndorserAtTheSameAddressAsBorrower(SystemBorrowerDemographics borrowerDemos, SystemBorrowerDemographics endorserDemos)
        {
            if (borrowerDemos.ZipCode != endorserDemos.ZipCode
            || borrowerDemos.State != endorserDemos.State
            || borrowerDemos.City != endorserDemos.City
            || borrowerDemos.Address2 != endorserDemos.Address2
            || borrowerDemos.Address1 != endorserDemos.Address1)
                return false;
            else
                return true;
        }

        private List<SystemBorrowerDemographics> GetEndorserDemos(ReflectionInterface ri, string bwrSsn)
        {
            List<SystemBorrowerDemographics> demos = new List<SystemBorrowerDemographics>();
            AccessReferrenceScreen(ri, bwrSsn);

            for (int row = 10; row < 21; row++)
            {
                if (ri.CheckForText(row, 5, "E") && ri.CheckForText(row, 52, "M") && ri.CheckForText(row, 78, "A"))
                    demos.Add(ri.GetDemographicsFromTx1j(ri.GetText(row, 13, 9)));
            }

            return demos;
        }

        private void AccessReferrenceScreen(ReflectionInterface ri, string bwrSsn)
        {
            ri.FastPath("TX3Z/ITX1JB" + bwrSsn);
            ri.Hit(ReflectionInterface.Key.F2);
            ri.Hit(ReflectionInterface.Key.F4);
        }

        private bool GetComment(CommentLetterData data)
        {
            using (ManualComments cmts = new ManualComments(string.Empty, 80))
            {
                if (cmts.ShowDialog() == DialogResult.Cancel || cmts.Comment.IsNullOrEmpty())
                {
                    MessageBox.Show("You must enter a comment");
                    return false;
                }
                this.Comment = cmts.Comment.Replace(Environment.NewLine, "");
                data.Comment = cmts.Comment.Replace(Environment.NewLine, "");
                data.LetterMergeText = cmts.Comment.Replace(Environment.NewLine, "");

                return true;
            }
        }

        /// <summary>
        /// Will add a comment to TD22
        /// </summary>
        /// <param name="approved">If this is true then it will use TD22 method all loans.  If denied it will use TD22 method byloan</param>
        /// <param name="ssn">SSN of the borrower we are adding the comment to.</param>
        /// <param name="loanSeq">List of integers of the loan sequences that were approved for IDR</param>
        /// <param name="data">Object that contains the comment text and arc to use</param>
        /// <param name="appId">integer application id generated by the database.  This will be formatted to be meaningful to the user and will be appended to the comment</param>
        private void AddComment(ReflectionInterface ri, bool approved, string ssn, List<int> loanSeq, CommentLetterData data, int appId, string scriptId, bool familySizeIncreased)
        {
            if (data.Comment != null && data.Comment.Contains("entered by user") && !familySizeIncreased)
            {
                while (!GetComment(data))
                    continue;
            }

            if (familySizeIncreased)
                data.Comment += "Application pended. Family size changed by four or more. Proces of family size required.";

            this.Comment = data.Comment = data.Comment.Replace(Environment.NewLine, "");

            data.Comment += " " + FormatAppId(appId);

            bool addedComment = false;
            if (approved)
                addedComment = ri.Atd22ByLoan(ssn, data.Arc, data.Comment, ssn, loanSeq, scriptId, false);
            else
                addedComment = ri.Atd22AllLoans(ssn, data.Arc, data.Comment, ssn, scriptId, false);

            if (!addedComment)
            {
                MessageBox.Show(string.Format("Error adding ARC: {0} Comment: {1} , please add comment manually and press insert when complete.", data.Arc, data.Comment));
                PLR.AddNotification(string.Format("Error adding ARC: {0} Comment: {1} Error:{2} , please add comment manually and press insert when complete.", data.Arc, data.Comment, ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);

                ri.PauseForInsert();
            }
        }

        //private void AddCommentCoBorrower(ReflectionInterface ri, bool approved, string ssn, List<int> loanSeq, CommentLetterData data, int appId, string scriptId, bool familySizeIncreased)
        //{
        //    if (data.Comment != null && data.Comment.Contains("entered by user") && !familySizeIncreased)
        //    {
        //        while (!GetComment(data))
        //            continue;
        //    }

        //    if (familySizeIncreased)
        //        data.Comment += "Application pended. Family size changed by four or more. Proces of family size required.";

        //    bool added = ri.Atd22ByLoan(borrowerSsn, data.CoBorrowerArc, data.CoBorrowerComment, ssn, loans, "IDRUSERPRO", false);
        //}

        /// <summary>
        /// Formats the application id so that it is meaningful to the user
        /// </summary>
        /// <param name="appid">Integer values generated by the database</param>
        /// <returns>return the formatted application id</returns>
        private string FormatAppId(int appId)
        {
            string appIdData = "P502M" + (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live ? "T" : "A");
            return appIdData + appId.ToString().PadLeft(4, '0');
        }

        /// <summary>
        /// Creates a letter for centralized printing, and auto images that letter to imaging.
        /// </summary>
        /// <param name="data">Object that contains the letter id and merge text for the letter we need to create.</param>
        /// <param name="ssn">SSN of the borrower we are creating the letter for.</param>
        private LetterStatus GenerateAndImageLetter(ReflectionInterface ri, CommentLetterData data, SystemBorrowerDemographics demos, RecoveryLog recovery, string scriptId, int appId, int processLogId, string userId, ApplicationEntry appEntry, bool isCoborrower, List<Ts26Loans> elligLoans, bool hasRetied = false, string borrowerSsn = "")
        {
            string borrowerAccountNumber = null;
            string fileToImage = string.Empty;
            if (recovery.RecoveryValue.Contains("Comment Added"))
            {
                string letterData;
                if (!isCoborrower)
                {
                    letterData = CreateLetterData(data, demos, scriptId, appEntry);
                }
                else
                {
                    borrowerAccountNumber = DA.GetAccountNumber(borrowerSsn);
                    letterData = CreateLetterDataCoBorrower(data, demos, scriptId, appEntry, borrowerAccountNumber);
                }
                EcorrData ecorrInfo = EcorrProcessing.CheckEcorr(demos.AccountNumber);
                bool onEcorr = (ecorrInfo != null && ecorrInfo.LetterIndicator && ecorrInfo.ValidEmail);

                if (demos.IsValidAddress || (!demos.IsValidAddress && onEcorr))
                {
                    if (isCoborrower)
                    {
                        bool success = AddCoborrowerArcs(ri, demos.Ssn, elligLoans, borrowerSsn, appId, data);
                        EcorrProcessing.AddCoBwrRecordToPrintProcessing(scriptId, data.LetterId, letterData, demos.AccountNumber, DA.GetCostCenterForLetter(data.LetterId).FirstOrDefault().CostCenter ?? "", borrowerSsn);
                    }
                    else
                    {
                        EcorrProcessing.AddRecordToPrintProcessing(scriptId, data.LetterId, letterData, demos.AccountNumber, DA.GetCostCenterForLetter(data.LetterId).FirstOrDefault().CostCenter ?? "");
                    }
                }
                else
                {
                    string tempDataFile = CreateDataFile(data, demos, scriptId, appEntry);
                    if (isCoborrower)
                    {
                        bool success = AddCoborrowerArcs(ri, demos.Ssn, elligLoans, borrowerSsn, appId, data);
                    }
                    AddInvalidAddressArc(ri, data, demos, scriptId, processLogId, userId, borrowerSsn, tempDataFile, isCoborrower, borrowerAccountNumber);//tempDataFile);
                    return LetterStatus.BadAddress;
                }
            }

            recovery.RecoveryValue = string.Format("{0}, Letter Archived", appId);
            return LetterStatus.Success;
        }
        
        private bool AddCoborrowerArcs(ReflectionInterface ri, string ssn, List<Ts26Loans> elligLoans, string borrowerSsn, int appId, CommentLetterData data)
        {
            if (data.CoBorrowerArc != null)
            {
                try
                {
                    List<short> coBorrowerLoans = DA.GetLoansForCoBorrower(ssn);
                    List<int> loans = coBorrowerLoans.Where(l => elligLoans.Select(eL => eL.LoanSeq.TrimLeft("0")).ToList().Contains(l.ToString())).Select(l => (int)l).ToList();
                    if(loans.Count < 1)
                    {
                        MessageBox.Show("Coborrower exists but has no elligible loans", "No Loans Elligible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false; //did not succeed
                    }
                    bool added = ri.Atd22ByLoan(borrowerSsn, data.CoBorrowerArc, data.Comment, ssn, loans, "IDRUSERPRO", false);
                    if(!added)
                    {
                        string message = string.Format("Unable to add the follow arc to the borrowers account.  ARC:{0}, ApplicationId:{1}", CoBorrowerArc, appId);
                        PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        MessageBox.Show("There was an error adding the activity comment to the borrowers account.  Please ensure the activity record is added. \n\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    string message = string.Format("Unable to add the follow arc to the borrowers account.  ARC:{0}, ApplicationId:{1}", CoBorrowerArc, appId);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    MessageBox.Show("There was an error adding the activity comment to the borrowers account.  Please ensure the activity record is added. \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return false;
        }

        private string CreateDataFile(CommentLetterData data, SystemBorrowerDemographics demos, string scriptId, ApplicationEntry appEntry)
        {
            string tempDataFile = string.Format("{0}{1}{2:MM-dd-yyyy hh-mm-ss}.txt", EnterpriseFileSystem.TempFolder, scriptId, DateTime.Now);
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            if (data.LetterMergeText != null)
                data.LetterMergeText = data.LetterMergeText.Replace(",", "");
            if (!appEntry.UserInputedData.MissingDocData.Any())
            {
                using (StreamWriter sw = new StreamW(tempDataFile))
                {
                    sw.WriteLine("ACSKeyLine,AccountNumber,Name,Address1,Address2,City,State,Zip,Country,StaticCurrentDate,Reason,AnniversaryDate");
                    sw.WriteLine(string.Format("{0},{1},{2} {3} {4},\"{5}\",\"{6}\",\"{7}\",{8},{9},{10},{11:MM/dd/yyyy},{12},{13}", keyLine, demos.AccountNumber, demos.FirstName,
                        demos.LastName, demos.Suffix, demos.Address1, demos.Address2, demos.City, demos.State.Replace("_", ""), demos.ZipCode.Replace("_", ""), demos.Country, DateTime.Now, data.LetterMergeText, !appEntry.AnniversaryDate.HasValue ? "" : appEntry.AnniversaryDate.Value.ToString("MM/dd/yyyy")));
                }
            }
            else
            {
                using (StreamWriter sw = new StreamW(tempDataFile))
                {
                    sw.WriteLine("ACSKeyLine,AccountNumber,Name,Address1,Address2,City,State,Zip,Country,StaticCurrentDate,AnniversaryDate,A1,A2,A3,A4,A5");
                    sw.WriteLine(string.Format(@"{0},{1},{2} {3} {4},""{5}"",""{6}"",""{7}"",{8},{9},{10},{11:MM/dd/yyyy},""{12}"","" {13}"","" {14}"","" {15}"","" {16}"","" {17}""", keyLine, demos.AccountNumber, demos.FirstName,
                        demos.LastName, demos.Suffix, demos.Address1, demos.Address2, demos.City, demos.State.Replace("_", ""), demos.ZipCode.Replace("_", ""), demos.Country, DateTime.Now, !appEntry.AnniversaryDate.HasValue ? "" : appEntry.AnniversaryDate.Value.ToString("MM/dd/yyyy"),
                        appEntry.UserInputedData.MissingDocData[0], appEntry.UserInputedData.MissingDocData[1], appEntry.UserInputedData.MissingDocData[2], appEntry.UserInputedData.MissingDocData[3]
                        , appEntry.UserInputedData.MissingDocData[4]));
                }
            }
            return tempDataFile;
        }

        public static string CreateLetterData(CommentLetterData data, SystemBorrowerDemographics demos, string scriptId, ApplicationEntry appEntry)
        {
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            if (data.LetterMergeText != null)
                data.LetterMergeText = data.LetterMergeText.Replace(",", "");
            //File Header "ACSKeyLine,AccountNumber,Name,Address1,Address2,City,State,Zip,Country,StaticCurrentDate,Reason,AnniversaryDate,A1,A2,A3,A4,A5"
            if (!appEntry.UserInputedData.MissingDocData.Any())
            {
                string anniversaryDate = !appEntry.AnniversaryDate.HasValue ? "" : appEntry.AnniversaryDate.Value.ToString("MM/dd/yyyy");

                return $"{keyLine},{demos.AccountNumber},{demos.FirstName} {demos.LastName} {demos.Suffix},\"{demos.Address1}\",\"{demos.Address2}\",\"{demos.City}\",{demos.State.Replace("_", "")},{demos.ZipCode.Replace("_", "")},{demos.Country},{DateTime.Now:MM/dd/yyyy},{data.LetterMergeText},{anniversaryDate},,,,,";
            }
            else
            {
                return string.Format(@"{0},{1},{2} {3} {4},""{5}"",""{6}"",""{7}"",{8},{9},{10},{11:MM/dd/yyyy},""{12}"","" {13}"","" {14}"","" {15}"","" {16}"","" {17}"","" {18}""", keyLine, demos.AccountNumber, demos.FirstName,
                    demos.LastName, demos.Suffix, demos.Address1, demos.Address2, demos.City, demos.State.Replace("_", ""), demos.ZipCode.Replace("_", ""), demos.Country, DateTime.Now, "", !appEntry.AnniversaryDate.HasValue ? "" : appEntry.AnniversaryDate.Value.ToString("MM/dd/yyyy"),
                    appEntry.UserInputedData.MissingDocData[0], appEntry.UserInputedData.MissingDocData[1], appEntry.UserInputedData.MissingDocData[2], appEntry.UserInputedData.MissingDocData[3]
                    , appEntry.UserInputedData.MissingDocData[4]);
            }
        }

        public static string CreateLetterDataCoBorrower(CommentLetterData data, SystemBorrowerDemographics demos, string scriptId, ApplicationEntry appEntry, string borrowerAccountNumber)
        {
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            if (data.LetterMergeText != null)
                data.LetterMergeText = data.LetterMergeText.Replace(",", "");
            //File Header "ACSKeyLine,AccountNumber,Name,Address1,Address2,City,State,Zip,Country,StaticCurrentDate,Reason,AnniversaryDate,A1,A2,A3,A4,A5"
            if (!appEntry.UserInputedData.MissingDocData.Any())
            {
                string anniversaryDate = !appEntry.AnniversaryDate.HasValue ? "" : appEntry.AnniversaryDate.Value.ToString("MM/dd/yyyy");

                return $"{keyLine},{borrowerAccountNumber},{demos.FirstName} {demos.LastName} {demos.Suffix},\"{demos.Address1}\",\"{demos.Address2}\",\"{demos.City}\",{demos.State.Replace("_", "")},{demos.ZipCode.Replace("_", "")},{demos.Country},{DateTime.Now:MM/dd/yyyy},{data.LetterMergeText},{anniversaryDate},,,,,";
            }
            else
            {
                return string.Format(@"{0},{1},{2} {3} {4},""{5}"",""{6}"",""{7}"",{8},{9},{10},{11:MM/dd/yyyy},""{12}"","" {13}"","" {14}"","" {15}"","" {16}"","" {17}"","" {18}""", keyLine, borrowerAccountNumber, demos.FirstName,
                    demos.LastName, demos.Suffix, demos.Address1, demos.Address2, demos.City, demos.State.Replace("_", ""), demos.ZipCode.Replace("_", ""), demos.Country, DateTime.Now, "", !appEntry.AnniversaryDate.HasValue ? "" : appEntry.AnniversaryDate.Value.ToString("MM/dd/yyyy"),
                    appEntry.UserInputedData.MissingDocData[0], appEntry.UserInputedData.MissingDocData[1], appEntry.UserInputedData.MissingDocData[2], appEntry.UserInputedData.MissingDocData[3]
                    , appEntry.UserInputedData.MissingDocData[4]);

            }
        }

        private void AddInvalidAddressArc(ReflectionInterface ri, CommentLetterData data, SystemBorrowerDemographics demos, string scriptId, int processLogId, string userId, string borrowerSsn, string tempDataFile, bool isCoborrower, string borrowerAccountNumber)
        {
            string ssn = borrowerSsn;
            if (string.IsNullOrWhiteSpace(ssn))
                ssn = demos.Ssn;
            string comment = string.Format("Unable to generate letter {0} {1}borrower has invalid address.  Letter {0} will be sent to imaging", data.LetterId, borrowerSsn.IsNullOrEmpty() ? "" : "co-");
            ri.Atd22ByBalance(ssn, "INVNL", comment, "", scriptId, false, false);
            if (isCoborrower)
            {
                EcorrProcessing.EcorrCentralizedPrintingAndImage(borrowerAccountNumber, demos.Ssn, data.LetterId, tempDataFile, userId, scriptId, "AccountNumber", "State", demos, processLogId, true, "LSARC");
            }
            else
            {
                EcorrProcessing.EcorrCentralizedPrintingAndImage(demos.AccountNumber, demos.Ssn, data.LetterId, tempDataFile, userId, scriptId, "AccountNumber", "State", demos, processLogId, true, "LSARC");
            }
        }
    }
}