using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CSLSLTRFED
{
    public class LetterGenerator
    {
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public string ScriptId { get; set; }
        public List<LetterData> Letters { get; set; }

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
        /// <param name="demos">SystemBorrowerDemographics object of borrowers demos</param>
        /// <param name="eDemos">EndorsersDemo object</param>
        public bool GenerateTheLetter(InputData userInput, SystemBorrowerDemographics demos, List<EndorserData> eData, bool ecorrBorrower)
        {
            string letterId = Letters.Where(p => p.LoanServicingLettersId == userInput.LoanServicingLettersId).FirstOrDefault().LetterId;
            CostCenter cc = DA.GetCostCenter(letterId);

            bool letterGenerated = false;

            //Generate the letters for death certificate
            if (Letters.Where(p => p.LoanServicingLettersId == userInput.LoanServicingLettersId).FirstOrDefault().DeathLetter)
            {
                //Generate the letter sent to the borrower
                letterGenerated = GenerateBorrowerSentToEndorser(userInput, letterId, demos, null, cc.UheaaCostCenter, false);
                //Generates all the endorser letters
                foreach (EndorserData info in eData)
                    GenerateBorrowerSentToEndorser(userInput, letterId, demos, info, cc.UheaaCostCenter, true);
            }
            else
            {
                //Generate the letter sent to the borrower
                letterGenerated = GenerateBorrower(userInput, letterId, demos, ecorrBorrower, cc.UheaaCostCenter);
                //Generates all the endorser letters
                foreach (EndorserData info in eData)
                    GenerateEndorser(userInput, letterId, demos, info, cc.UheaaCostCenter);
            }
            return letterGenerated;
        }

        /// <summary>
        /// Prints the Borrower Letter
        /// </summary>
        /// <returns></returns>
        private bool GenerateBorrower(InputData userInput, string letterId, SystemBorrowerDemographics demos, bool ecorrBorrower, string costCenter)
        {
            bool invalidAddress = CheckInvalidBorrowerAddress(userInput, demos, ecorrBorrower, letterId);
            if (invalidAddress)
                return false;

            string letterData = GetNameAndAddress(demos);
            if (letterData.Trim().IsNullOrEmpty())
            {
                string message = $"There was an error getting the borrowers address information from the warehouse for borrower: {demos.AccountNumber}.";
                Dialog.Info.Ok(message, "Error Creating Letter");
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            string detail = CreateDataFile(userInput, false);
            if (detail.IsNullOrEmpty())
                return false;
            letterData += detail;

            int ? printId = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, costCenter, DataAccessHelper.Region.CornerStone);
            if (!printId.HasValue)
            {
                string message = $"There was an error adding the {letterId} for borrower: {demos.AccountNumber} to the database.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message, "Error");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Prints the Endorser letter
        /// </summary>
        /// <returns></returns>
        private void GenerateEndorser(InputData userInput, string letterId, SystemBorrowerDemographics demos, EndorserData eData, string costCenter)
        {
            bool ecorrEndorser = (eData.EndorserEcorr == null ? false : eData.EndorserEcorr.LetterIndicator);
            bool invalidAddress = CheckInvalidEndorserAddress(userInput, demos, eData.EndorserDemo, ecorrEndorser, letterId);
            if (invalidAddress)
                return;

            string letterData = GetNameAndAddress(eData.EndorserDemo, demos);
            if (letterData.Trim().IsNullOrEmpty() || CheckInvalidEndorserAddress(userInput, demos, eData.EndorserDemo, ecorrEndorser, letterId))
            {
                string message = $"There was an error getting the endorsers address information from the warehouse for endorser: {eData.EndorserDemo.AccountNumber}.";
                Dialog.Info.Ok(message, "Error Creating Letter");
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
            string detail = CreateDataFile(userInput, true);
            if (detail.IsNullOrEmpty())
                return;
            letterData += detail;

            int ? printId = EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, eData.EndorserDemo.AccountNumber, costCenter, demos.Ssn, DataAccessHelper.Region.CornerStone);
            if (!printId.HasValue)
            {
                string message = $"There was an error adding the {letterId} for endorser: {eData.EndorserDemo.AccountNumber} to the database.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message, "Error");
            }
        }

        /// <summary>
        /// Prints a letter for the endorser about the borrower / Death letters
        /// </summary>
        private bool GenerateBorrowerSentToEndorser(InputData userInput, string letterId, SystemBorrowerDemographics demos, EndorserData eData, string costCenter, bool isEndorser)
        {
            string letterData = GetNameAndAddress(isEndorser ? eData.EndorserDemo : demos, demos);
            if (letterData.Trim().IsNullOrEmpty())
            {
                string message = $"There was an error getting the borrower or endorser information from the warehouse for account number: {demos.AccountNumber}.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            string detail = CreateDataFile(userInput, isEndorser);
            if (detail.IsNullOrEmpty())
                return false;
            letterData += detail;
            string borrowerData = GetNameAndAddress(demos);
            letterData += $",{borrowerData.SplitAndRemoveQuotes(",")[2]}"; //The borrower name is in position 2

            int? printId = 0;
            if (isEndorser)
                printId = EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, eData.EndorserDemo.AccountNumber, costCenter, demos.Ssn, DataAccessHelper.Region.CornerStone);
            else
                printId = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, costCenter, DataAccessHelper.Region.CornerStone);
            if (!printId.HasValue)
            {
                string personType = isEndorser ? "endorser" : "borrower";
                string accout = isEndorser ? eData.EndorserDemo.AccountNumber : demos.AccountNumber;
                string message = $"There was an error adding the {letterId} for {personType} : {accout} to the database.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message, "Error");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the name and address information
        /// </summary>
        /// <param name="demos">Borrower or endorser demos, depending on which letter is being printed</param>
        /// <param name="bDemos">Always borrower demographics if sending to endorser</param>
        private string GetNameAndAddress(SystemBorrowerDemographics demos, SystemBorrowerDemographics bDemos = null)
        {
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
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
            return $"{keyLine},{account},{demos.FirstName.Trim()} {demos.LastName.Trim()},{demos.Address1.Trim()},{demos.Address2.Trim()},{demos.City.Trim()},{demos.State.Trim()},{zip.Trim()},{demos.ForeignState.Trim()},{demos.Country.Trim()},";
        }

        /// <summary>
        /// Images any letter that could not be sent to the borrower due to invalid address or on Ecorr.
        /// </summary>
        public bool ImageTheLetter(InputData userInput, string letterId, SystemBorrowerDemographics demos, bool isEndorser, SystemBorrowerDemographics endorserDemos = null)
        {
            //Constants
            string accountNumberHeader = "AccountNumber";
            string stateHeader = "State";
            string docId = "CRARC";

            //Create the temp file
            string fileAccountNumber = isEndorser ? endorserDemos.AccountNumber : userInput.AccountNumber;
            string tempFile = Path.Combine(EnterpriseFileSystem.TempFolder, $@"CSLSLTRFED_{fileAccountNumber}.txt");

            //get the file header for the specific letter being sent
            string fileHeader = DA.GetFileHeader(letterId, ScriptId);

            //get the letter data information
            string letterData = GetNameAndAddress(isEndorser ? endorserDemos : demos);
            letterData += CreateDataFile(userInput, isEndorser);
            if (letterData.IsNullOrEmpty())
                return false;

            //write the data file
            using (StreamWriter sw = new StreamWriter(tempFile))
            {
                //write file header to file
                sw.WriteLine(fileHeader);
                //write letterdata to file
                sw.WriteLine(letterData);
            }

            EcorrProcessing.EcorrCentralizedPrintingAndImage(isEndorser ? endorserDemos.AccountNumber : userInput.AccountNumber, isEndorser ? endorserDemos.AccountNumber : userInput.BorrowerSsn, letterId, tempFile, "CSLSLTRFED", ScriptId, accountNumberHeader, stateHeader, demos, LogRun.ProcessLogId, true, docId);

            //Delete local temp file
            Repeater.TryRepeatedly(new Action(() => File.Delete(tempFile)));

            return true;
        }

        /// <summary>
        /// Checks if the borrower address is valid
        /// </summary>
        private bool CheckInvalidBorrowerAddress(InputData userInput, SystemBorrowerDemographics demos, bool ecorrBorrower, string letterId)
        {
            if (!demos.IsValidAddress && !ecorrBorrower)
            {
                string message = $"The borrower does not have a valid address and is not on Ecorr. Document has been imaged.";
                Dialog.Info.Ok(message);
                ImageTheLetter(userInput, letterId, demos, false);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the coborrower address is valid
        /// </summary>
        public bool CheckInvalidEndorserAddress(InputData userInput, SystemBorrowerDemographics demos, SystemBorrowerDemographics eDemos, bool ecorrEndorser, string letterId)
        {
            bool validAddress = eDemos.IsValidAddress;
            if (!eDemos.IsValidAddress && !ecorrEndorser)
            {
                string message = $"The endorser does not have a valid address and is not on Ecorr. Document has been imaged.";
                Dialog.Info.Ok(message);
                ImageTheLetter(userInput, letterId, demos, true, eDemos);
                return true;
            }
            return false;
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
            if (letter.LetterId.ToLower() == "defdnfed")
                letterOption += " Deferment";
            else if (letter.LetterId.ToLower() == "fordnfed")
                letterOption += " Forbearance";
            for (int i = 0; i < userInput.DenialReasons.Count; i++)
                denialReasons += userInput.DenialReasons[i];
            denialReasons = denialReasons.Trim();
            denialReasons = denialReasons + @"""";
            letterData = string.Format(@"{0},{1},{2},{3},{4},{5},{6:MM/dd/yyyy},{7:MM/dd/yyyy}"
                    + ",{8},{9:MM/dd/yyyy},{10:MM/dd/yyyy},{11},{11},{13},\"{14}\",{15},{16}"
                    , userInput.DenialReasons.Count() >= 1 ? @"""- " + userInput.DenialReasons[0] + @"""" : @""""""
                    , userInput.DenialReasons.Count() >= 2 ? @"""- " + userInput.DenialReasons[1] + @"""" : @""""""
                    , userInput.DenialReasons.Count() >= 3 ? @"""- " + userInput.DenialReasons[2] + @"""" : @""""""
                    , userInput.DenialReasons.Count() >= 4 ? @"""- " + userInput.DenialReasons[3] + @"""" : @""""""
                    , userInput.DenialReasons.Count() == 5 ? @"""- " + userInput.DenialReasons[4] + @"""" : @""""""
                    , userInput.SchoolName, userInput.LastDateOfAttendance, userInput.SchoolClosureDate, userInput.DefermentForbearanceType
                    , userInput.DefForbEndDate, userInput.LoanTermEndDate, userInput.LowDirectoryBegin, userInput.LowDirectoryEnd
                    , denialReasons, userInput.AmountForDischarge.IsPopulated() ? userInput.AmountForDischarge.ToDouble().ToString("#,##0.00") : ""
                    , letterOption, userInput.SchoolName);

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
                                newLine = string.Format("\"{0:C}\",", row[columnIndex].ToString().Replace("¬", ",").Replace("\"", "").Replace("$", "").ToDecimal());
                            else
                                newLine = string.Format("\"{0}\",", row[columnIndex].ToString().Replace("¬", ",").Replace("\"", ""));

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