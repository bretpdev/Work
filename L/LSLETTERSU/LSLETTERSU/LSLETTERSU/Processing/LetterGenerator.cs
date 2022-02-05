using LSLETTERSU.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace LSLETTERSU.Processing
{
    public class LetterGenerator
    {
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public string ScriptId { get; set; }
        public List<LetterData> Letters { get; set; }
        public InputData UserInput { get; set; }
        private const string SystemSupportMessage = "Please contact System Support and reference Process Log ID:";

        public LetterGenerator(DataAccess da, ProcessLogRun logRun, string scriptId, List<LetterData> letters)
        {
            DA = da;
            LogRun = logRun;
            ScriptId = scriptId;
            Letters = letters;
        }

        /// <summary>
        /// Generate the letter for the borrower and endorser
        /// </summary>
        /// <param name="userInput">The Input data</param>
        /// <param name="demos">BorrowerDemographics object of borrowers demos</param>
        /// <param name="eDemos">EndorsersDemo object</param>
        public ErrorMessage GenerateTheLetter(InputData userInput, SystemBorrowerDemographics demos, List<EndorserData> eData)
        {
            UserInput = userInput;
            string letterId = Letters.Where(p => p.LoanServicingLettersId == userInput.LoanServicingLettersId).FirstOrDefault().LetterId;
            CostCenter cc = DA.GetCostCenter(letterId);

            ErrorMessage em = new ErrorMessage();

            if (!CheckValidAddress(demos))
                em.Message = "Borrower is not on Ecorr and does not have a valid mailing address. Letter will not be sent.";
            CheckValidEndAddress(eData, em);

            //Generate the letters for death certificate
            if (Letters.Where(p => p.LoanServicingLettersId == userInput.LoanServicingLettersId).FirstOrDefault().DeathLetter)
            {
                if (em.Message.IsNullOrEmpty())
                    GenerateBorrowerSentToEndorser(userInput, letterId, demos, null, cc.UheaaCostCenter, false, em);
                //Generates all the endorser letters
                foreach (EndorserData info in eData)
                    GenerateBorrowerSentToEndorser(userInput, letterId, demos, info, cc.UheaaCostCenter, true, em);
            }
            else
            {
                if (em.Message.IsNullOrEmpty())//Generate the letter sent to the borrower
                    GenerateLetter(userInput, letterId, demos, null, cc.UheaaCostCenter, false, em);
                //Generates all the endorser letters
                foreach (EndorserData info in eData)
                    GenerateLetter(userInput, letterId, demos, info, cc.UheaaCostCenter, true, em);
            }
            return em;
        }

        /// <summary>
        /// Checks the borrower to see if they are on ecorr or have a valid mailing address
        /// </summary>
        private bool CheckValidAddress(SystemBorrowerDemographics demos)
        {
            EcorrData borEcorr = EcorrProcessing.CheckEcorr(demos.AccountNumber);
            if (!demos.IsValidAddress && !(borEcorr.EbillIndicator && borEcorr.ValidEmail))
                return false;
            return true;
        }

        /// <summary>
        /// Checks each endorser to see if they are on ecorr or have a valid mailing address
        /// </summary>
        private void CheckValidEndAddress(List<EndorserData> eData, ErrorMessage em)
        {
            List<EndorserData> endData = new List<EndorserData>(eData);
            foreach (EndorserData endorserData in endData)
            {
                EcorrData ecData = EcorrProcessing.CheckEcorr(endorserData.EndorserDemo.AccountNumber);
                if (!endorserData.EndorserDemo.IsValidAddress && !(ecData.EbillIndicator && ecData.ValidEmail))
                {
                    eData.Remove(endorserData); //Remove all records from the list where the endorser does not have a valid mailing address or on ecorr
                    em.EndorserMessage += $"  Co-borrower {endorserData.EndorserDemo.AccountNumber} is not on Ecorr and does not have a valid mailing address. Letter will not be sent to Co-borrower.";
                }
            }
        }

        /// <summary>
        /// Prints the Borrower Letter
        /// </summary>
        private void GenerateLetter(InputData userInput, string letterId, SystemBorrowerDemographics demos, EndorserData eData, string costCenter, bool isEndorser, ErrorMessage em)
        {
            string letterData = isEndorser ? GetNameAndAddress(eData.EndorserDemo, demos) : GetNameAndAddress(demos);
            if (letterData.Trim().IsNullOrEmpty())
            {
                string message = $"There was an error getting the {(isEndorser ? "endorsers" : "borrowers")} address information from the warehouse for borrower: {(isEndorser ? eData.EndorserDemo.AccountNumber : demos.AccountNumber)}.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                em.Message = $"{message} {SystemSupportMessage} {LogRun.ProcessLogId}";
            }
            string detail = CreateDataFile(userInput, false);
            if (detail.IsNullOrEmpty())
                em.Message = $"Unable to create merge data file for borrower: {(isEndorser ? eData.EndorserDemo.AccountNumber : demos.AccountNumber)}";
            letterData += detail;

            int? printId;
            if (isEndorser)
                printId = EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, eData.EndorserDemo.AccountNumber, costCenter, demos.Ssn, DataAccessHelper.Region.Uheaa, ActiveDirectoryUsers.UserName);
            else
                printId = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, costCenter, DataAccessHelper.Region.Uheaa, ActiveDirectoryUsers.UserName);
            if (!printId.HasValue)
            {
                string message = $"There was an error adding the {letterId} letter for borrower: {(isEndorser ? eData.EndorserDemo.AccountNumber : demos.AccountNumber)} to the database.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                em.Message = $"{message} {SystemSupportMessage} {LogRun.ProcessLogId}";
            }
        }

        /// <summary>
        /// Prints a letter for the endorser about the borrower / Death letters
        /// </summary>
        private void GenerateBorrowerSentToEndorser(InputData userInput, string letterId, SystemBorrowerDemographics demos, EndorserData eData, string costCenter, bool isEndorser, ErrorMessage em)
        {
            string letterData = GetNameAndAddress(isEndorser ? eData.EndorserDemo : demos, demos);
            if (letterData.Trim().IsNullOrEmpty())
            {
                string message = $"There was an error getting the borrower or endorser information from the warehouse for account number: {demos.AccountNumber}.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                em.EndorserMessage = $"{message} {SystemSupportMessage} {LogRun.ProcessLogId}";
            }
            string detail = CreateDataFile(userInput, isEndorser);
            if (detail.IsNullOrEmpty())
                em.EndorserMessage = $"Unable to create merge data file for borrower: {demos.AccountNumber} regarding endoerser: {eData.EndorserDemo.AccountNumber}";
            letterData += detail;
            string borrowerData = GetNameAndAddress(demos);
            letterData += $",{borrowerData.SplitAndRemoveQuotes(",")[2]}"; //The borrower name is in position 2

            int? printId = 0;
            if (isEndorser)
                printId = EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, eData.EndorserDemo.AccountNumber, costCenter, demos.Ssn, DataAccessHelper.Region.Uheaa, ActiveDirectoryUsers.UserName);
            else
                printId = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, costCenter, DataAccessHelper.Region.Uheaa, ActiveDirectoryUsers.UserName);
            if (!printId.HasValue)
            {
                string personType = isEndorser ? "endorser" : "borrower";
                string account = isEndorser ? eData.EndorserDemo.AccountNumber : demos.AccountNumber;
                string message = $"There was an error adding the {letterId} letter for {personType} : {account} to the database.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                em.EndorserMessage = $"{message} {SystemSupportMessage} {LogRun.ProcessLogId}";
            }
        }

        /// <summary>
        /// Gets the name and address information
        /// </summary>
        /// <param name="demos">Borrower or endorser demos, depending on which letter is being printed</param>
        /// <param name="bDemos">Always borrower demographics if sending to endorser</param>
        private string GetNameAndAddress(SystemBorrowerDemographics demos, SystemBorrowerDemographics bDemos = null)
        {
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            UserInput.AcsKeyLine = keyLine;
            string zip = demos.ZipCode.Length == 9 ? demos.ZipCode.Insert(5, "-") : demos.ZipCode;
            if (demos.ForeignState.IsNullOrEmpty() || demos.ForeignState.Contains("_"))
                demos.ForeignState = "";
            if (demos.Country.IsNullOrEmpty() || demos.Country.Contains("_"))
                demos.Country = "";
            else
            {
                demos.ForeignState = demos.State.Replace("_", "");
                demos.State = "";
            }
            string account = bDemos != null ? bDemos.AccountNumber.Trim() : demos.AccountNumber.Trim();
            string suffix = (demos.Suffix ?? "").Trim();
            return $"{keyLine},{account},{demos.FirstName.Trim()} {demos.LastName.Trim()} {suffix},{demos.Address1.Trim()},{demos.Address2.Trim()},{demos.City.Trim()},{demos.State.Trim()},{zip.Trim()},{demos.ForeignState.Trim()},{demos.Country.Trim()},";
        }

        /// <summary>
        /// Creates the data file used for printing
        /// </summary>
        /// <returns></returns>
        private string CreateDataFile(InputData userInput, bool isEndorser)
        {
            string letterData = "";
            string denialReasons = @"""";
            string letterOption = userInput.LetterOption;
            LetterData letter = Letters.Where(p => p.LoanServicingLettersId == userInput.LoanServicingLettersId).FirstOrDefault();
            if (letter.LetterName.ToLower().Contains("deferment"))
                letterOption += " Deferment.";
            else if (letter.LetterName.ToLower().Contains("forbearance"))
                letterOption += " Forbearance.";
            for (int i = 0; i < userInput.DenialReasons.Count; i++)
                denialReasons += userInput.DenialReasons[i];
            denialReasons = denialReasons.Trim();
            denialReasons = denialReasons + @"""";
            letterData = $@"{(userInput.DenialReasons.Count() >= 1 ? @"""- " + userInput.DenialReasons[0]?.Trim() + @"""" : "")},{(userInput.DenialReasons.Count() >= 2 ? @"""- " + userInput.DenialReasons[1]?.Trim() + @"""" : "")},{(userInput.DenialReasons.Count() >= 3 ? @"""- " + userInput.DenialReasons[2]?.Trim() + @"""" : "")},{(userInput.DenialReasons.Count() >= 4 ? @"""- " + userInput.DenialReasons[3]?.Trim() + @"""" : "")},{(userInput.DenialReasons.Count() == 5 ? @"""- " + userInput.DenialReasons[4]?.Trim() + @"""" : "")},{userInput.SchoolName?.Trim()},{userInput.LastDateOfAttendance:MM/dd/yyyy},{userInput.SchoolClosureDate:MM/dd/yyyy},{ userInput.DefermentForbearanceType?.Trim()},{userInput.DefForbEndDate:MM /dd/yyyy},{userInput.LoanTermEndDate:MM/dd/yyyy},{userInput.BeginYear?.Trim()},{userInput.EndYear?.Trim()},{denialReasons?.Trim()},{(userInput.AmountForDischarge.IsPopulated() ? @"""" + userInput.AmountForDischarge.ToDouble().ToString("#,##0.00") + @"""" : "")},{letterOption?.Trim()},{ userInput.SchoolName?.Trim()}";
            string loanData = GenerateLoanData(userInput, isEndorser);
            if (loanData.IsNullOrEmpty())
                return letterData; // Return an empty string if the 
            letterData += $",{loanData}";
            return letterData;
        }

        /// <summary>
        /// Generates the Discharge Loan Data
        /// </summary>
        /// <returns></returns>
        private string GenerateLoanData(InputData userInput, bool isEndorser)
        {
            DataTable loanDetail = new DataTable();
            string line = string.Empty;
            int rowCount = 1;

            string sproc = Letters.Where(p => p.LoanServicingLettersId == userInput.LoanServicingLettersId).FirstOrDefault().StoredProcedureName;
            if (sproc.IsPopulated())
            {
                loanDetail = DA.ExecuteSproc(userInput.AccountNumber, sproc, isEndorser);
                if (loanDetail != null)
                {
                    foreach (DataRow row in loanDetail.Rows)
                    {
                        if (rowCount > 30)
                            break;
                        for (int columnIndex = 0; columnIndex < loanDetail.Columns.Count; columnIndex++)
                        {
                            //Sometimes a merge field needs a comma so the record will contain the symbol ¬ which is generated with altl + 170. We will replace it with a comma and surround it with "" so the merge field maintains the comma.
                            string newLine = "";
                            if (row[columnIndex].ToString().Contains("$"))
                                newLine = $@"""{row[columnIndex].ToString().Replace("¬", ",").Replace("\"", "").Replace("$", "").ToDecimal():C}""";
                            else
                                newLine = $@"""{row[columnIndex].ToString().Replace("¬", ",").Replace("\"", "")}""";

                            line += newLine;
                        }

                        rowCount++;
                    }

                    //Insert the empty Commas
                    if (loanDetail.Rows.Count < 30)//Insert the empty Commas
                    {
                        for (int rows = (loanDetail.Rows.Count * loanDetail.Columns.Count) + 1; rows < (30 * loanDetail.Columns.Count); rows++)
                            line += @",";
                    }
                    if (rowCount > 30)
                        line = line.Remove(line.Length - 1, 1);
                }
                else
                    return null;
            }
            return line;
        }
    }
}