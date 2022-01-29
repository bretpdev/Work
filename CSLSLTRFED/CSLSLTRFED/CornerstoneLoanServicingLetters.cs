using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CSLSLTRFED
{
    public class CornerstoneLoanServicingLetters : ScriptBase
    {
        private enum ScriptResults
        {
            ShowFormWithData,
            ShowFormBlankData,
            DoneProcessing,
            DoNothing,
            Cancel
        }

        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public List<LetterData> Letters { get; set; }

        public CornerstoneLoanServicingLetters(ReflectionInterface ri)
            : base(ri, "CSLSLTRFED", DataAccessHelper.Region.CornerStone)
        {
            LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true);
            DA = new DataAccess(LogRun);
            Letters = DA.GetLetterData();
            SetLetterNames();
        }

        public override void Main()
        {
            InputData userInput = new InputData();
            ScriptResults result = ScriptResults.ShowFormBlankData;
            LetterSelection selection = new LetterSelection(userInput, Letters);
            while (result != ScriptResults.DoneProcessing)
            {
                if (selection.ShowDialog() == DialogResult.Cancel)
                    break;

                result = SessionProcessing(userInput);

                if (result == ScriptResults.ShowFormWithData)
                    continue;
                else if (result == ScriptResults.ShowFormBlankData)
                {
                    //Clear out all the data
                    userInput = new InputData();
                    selection = new LetterSelection(userInput, Letters);
                    continue;
                }
                else if (result == ScriptResults.Cancel)
                    break;
                else
                {
                    selection.Close();
                    Dialog.Info.Ok("Processing Complete", "CSLSLTRFED");
                    break;
                }
            }

            LogRun.LogEnd();
        }

        private void SetLetterNames()
        {
            Parallel.ForEach(Letters, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, letter =>
            {
                letter.LetterName = DA.GetLetterName(letter.LetterId);
            });
        }

        private ScriptResults SessionProcessing(InputData userInput)
        {
            userInput.AccountNumber = AccountNumberCheck(userInput.AccountNumber);
            if (userInput.AccountNumber.IsNullOrEmpty())
                return ScriptResults.ShowFormWithData;

            SystemBorrowerDemographics bDemos = GetDemographicsFromTx1j(userInput.AccountNumber);
            List<Endorsers> endorsers = DA.GetCoborrowers(bDemos.Ssn);
            List<EndorserData> eData = new List<EndorserData>();
            if (endorsers.Count > 0)
            {
                foreach (Endorsers end in endorsers.DistinctBy(p => p.LF_EDS))
                {
                    EndorserData info = new EndorserData();
                    info.Ssn = end.LF_EDS;
                    info.SequenceNumbers = endorsers.Where(p => p.LF_EDS == end.LF_EDS).Select(p => p.LN_SEQ).ToList();
                    info.EndorserDemo = RI.GetDemographicsFromTx1j(end.LF_EDS);
                    eData.Add(info);
                }
            }

            //eData = CheckLoanPrograms(userInput.AccountNumber, loanPrograms, bDemos); //Load endorser data
            if (eData == null)
                return ScriptResults.DoNothing;

            RI.FastPath("TX3Z/ITS2X" + userInput.AccountNumber);
            if (userInput.LetterOption.ToLower().Contains("new ibr"))
                return CheckNewIBR(userInput);
            else if (userInput.LetterOption.ToLower().Contains("renewal ibr"))
                return CheckRenewalIBR(userInput);
            else if (userInput.LetterOption.ToLower().Contains("new paye"))
                return CheckNewPaye(userInput);
            else if (userInput.LetterOption.ToLower().Contains("renewal paye"))
                return CheckRenewalPaye(userInput);

            RI.FastPath("TX3Z/ITX1JB");
            RI.PutText(6, 61, userInput.AccountNumber, ReflectionInterface.Key.Enter);
            if (!RI.CheckForText(1, 71, "TXX1R-01"))
            {
                Dialog.Info.Ok("Unable to find borrowers demographics.");
                return ScriptResults.ShowFormBlankData;
            }

            RI.FastPath("TX3Z/ITX1JB");
            RI.PutText(6, 61, userInput.AccountNumber, ReflectionInterface.Key.Enter);

            return ScriptForm(bDemos, eData, userInput);
        }

        /// <summary>
        /// Checks the warehouse to see if the borrower account exists. 
        /// </summary>
        /// <param name="accountNumber">Borrower account number</param>
        /// <returns>Returns an account number for the account number or ssn passed in.</returns>
        private string AccountNumberCheck(string accountNumber)
        {
            string acctNumber = DA.GetAccountNumber(accountNumber);
            if (acctNumber.IsNullOrEmpty())
            {
                Dialog.Warning.Ok("Unable to find the borrower please try again.", "Account not found");
                //Let the user re-enter the information
                return "";
            }
            else
                return acctNumber;
        }

        private ScriptResults CheckNewIBR(InputData userInput)
        {
            if (RI.CheckForText(1, 72, "TSX2Y"))
            {
                for (int row = 8; true; row++)
                {
                    if (row > 19)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }

                    //No IB or IL schedules found the borrower can be on a new IBR
                    if (RI.CheckForText(23, 2, "90007") || RI.CheckForText(row, 11, " "))
                        break;

                    if (RI.CheckForText(row, 11, "IB", "IL"))
                    {
                        Dialog.Info.Ok("Borrower has used IBR in the past. Please select another Repayment Plan Option.");
                        using (LetterSelection letterChooser = new LetterSelection(userInput, Letters))
                        {
                            if (letterChooser.ShowDialog() == DialogResult.Cancel)
                                return ScriptResults.Cancel;
                        }
                    }
                }
                return ScriptResults.ShowFormWithData;
            }
            else//Borrower only has one schedule
            {
                if (RI.CheckForText(9, 75, "IB", "IL"))
                {
                    Dialog.Info.Ok("Borrower has used IBR in the past. Please select another Repayment Plan Option.");
                    using (LetterSelection letterChooser = new LetterSelection(userInput, Letters))
                    {
                        if (letterChooser.ShowDialog() == DialogResult.Cancel)
                            return ScriptResults.Cancel;
                    }
                }
                return ScriptResults.ShowFormWithData;
            }
        }

        private ScriptResults CheckRenewalIBR(InputData userInput)
        {
            if (RI.CheckForText(1, 72, "TSX2Y"))
            {
                for (int row = 8; true; row++)
                {
                    if (row > 19)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }

                    //No IB or IL schedules found the borrower can be on a new IBR
                    if (RI.CheckForText(23, 2, "90007") || RI.CheckForText(row, 11, " "))
                    {
                        Dialog.Info.Ok("Borrower has used IBR in the past. Please select another Repayment Plan Option.");
                        using (LetterSelection letterChooser = new LetterSelection(userInput, Letters))
                        {
                            if (letterChooser.ShowDialog() == DialogResult.Cancel)
                                return ScriptResults.Cancel;
                        }
                        return ScriptResults.ShowFormWithData;
                    }

                    if (RI.CheckForText(row, 11, "IB", "IL"))
                        break;
                }
            }
            else//Borrower only has one schedule
            {
                if (!RI.CheckForText(9, 75, "IB", "IL"))
                {
                    Dialog.Info.Ok("Borrower hasn’t used IBR in the past. Please select another Repayment Plan Option.");
                    using (LetterSelection letterChooser = new LetterSelection(userInput, Letters))
                    {
                        if (letterChooser.ShowDialog() == DialogResult.Cancel)
                            return ScriptResults.Cancel;
                    }
                    return ScriptResults.ShowFormWithData;
                }
            }
            return ScriptResults.DoNothing;
        }

        private ScriptResults CheckNewPaye(InputData userInput)
        {
            if (RI.CheckForText(1, 72, "TSX2Y"))
            {
                for (int row = 8; true; row++)
                {
                    if (row > 19)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }

                    //No IB or IL schedules found the borrower can be on a new IBR
                    if (RI.CheckForText(23, 2, "90007") || RI.CheckForText(row, 11, " "))
                        break;

                    if (RI.CheckForText(row, 11, "CA", "CP"))
                    {
                        Dialog.Info.Ok("Borrower has used PAYE in the past. Please select another Repayment Plan Option.");
                        using (LetterSelection letterChooser = new LetterSelection(userInput, Letters))
                        {
                            if (letterChooser.ShowDialog() == DialogResult.Cancel)
                                return ScriptResults.Cancel;
                        }
                        return ScriptResults.ShowFormWithData;
                    }
                }
            }
            else//Borrower only has one schedule
            {
                if (RI.CheckForText(9, 75, "CA", "CP"))
                {
                    Dialog.Info.Ok("Borrower has used PAYE in the past. Please select another Repayment Plan Option.");
                    using (LetterSelection letterChooser = new LetterSelection(userInput, Letters))
                    {
                        if (letterChooser.ShowDialog() == DialogResult.Cancel)
                            return ScriptResults.Cancel;
                    }
                    return ScriptResults.ShowFormWithData;
                }
            }
            return ScriptResults.DoNothing;
        }

        private ScriptResults CheckRenewalPaye(InputData userInput)
        {
            if (RI.CheckForText(1, 72, "TSX2Y"))
            {
                for (int row = 8; true; row++)
                {
                    if (row > 19)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }

                    //No IB or IL schedules found the borrower can be on a new IBR
                    if (RI.CheckForText(23, 2, "90007") || RI.CheckForText(row, 11, " "))
                    {
                        Dialog.Info.Ok("Borrower hasn’t used PAYE in the past. Please select another Repayment Plan Option.");
                        using (LetterSelection letterChooser = new LetterSelection(userInput, Letters))
                        {
                            if (letterChooser.ShowDialog() == DialogResult.Cancel)
                                return ScriptResults.Cancel;
                        }
                        return ScriptResults.ShowFormWithData;
                    }

                    if (RI.CheckForText(row, 11, "CA", "CP"))
                        break;
                }
            }
            else//Borrower only has one schedule
            {
                if (!RI.CheckForText(9, 75, "CA", "CP"))
                {
                    Dialog.Info.Ok("Borrower hasn’t used PAYE in the past. Please select another Repayment Plan Option.");
                    using (LetterSelection letterChooser = new LetterSelection(userInput, Letters))
                    {
                        if (letterChooser.ShowDialog() == DialogResult.Cancel)
                            return ScriptResults.Cancel;
                    }
                    return ScriptResults.ShowFormWithData;
                }
            }
            return ScriptResults.DoNothing;
        }

        private ScriptResults ScriptForm(SystemBorrowerDemographics bDemos, List<EndorserData> eData, InputData userInput)
        {
            EcorrData borrowerEcorr = EcorrProcessing.CheckEcorr(bDemos.AccountNumber);
            bool ecorrBorrower = borrowerEcorr == null ? false : borrowerEcorr.LetterIndicator;
            if (eData.Count > 0)
                foreach (EndorserData info in eData)
                    info.EndorserEcorr = EcorrProcessing.CheckEcorr(info.EndorserDemo.AccountNumber);
            using (AddressInfo addrData = new AddressInfo(eData.Count > 0, (borrowerEcorr != null && borrowerEcorr.LetterIndicator), eData.Any(p => p.EndorserEcorr.LetterIndicator)))
            {
                DialogResult result = addrData.ShowDialog();

                if (result == DialogResult.Abort)
                {
                    RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.F6);
                    RI.Hit(ReflectionInterface.Key.F6);
                    return ScriptResults.Cancel;
                }
                else if (result == DialogResult.Cancel)
                    return ScriptResults.ShowFormBlankData;

                bool letterGenerated = false;
                LetterGenerator lGen = new LetterGenerator(DA, LogRun, ScriptId, Letters);
                if (userInput.DenialReasons[0].ToLower().Contains("sent to fsa"))
                {
                    if (MessageBox.Show("Borrower’s application has been sent to FSA for approval. Is this correct?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        letterGenerated = lGen.GenerateTheLetter(userInput, bDemos, eData, ecorrBorrower);
                    else
                        return ScriptResults.ShowFormWithData;
                }
                else if (userInput.DenialReasons[0].ToLower().Contains("approved by fsa"))
                {
                    if (MessageBox.Show($"Borrower has been approved for {userInput.LetterType} {userInput.LetterOption} amount: ${userInput.AmountForDischarge}.  Is this Correct?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        letterGenerated = lGen.GenerateTheLetter(userInput, bDemos, eData, ecorrBorrower);
                    else
                        return ScriptResults.ShowFormWithData;
                }
                else
                {
                    string denial1 = userInput.DenialReasons.Count > 0 ? "-" + userInput.DenialReasons[0] : string.Empty;
                    string denial2 = userInput.DenialReasons.Count > 1 ? "-" + userInput.DenialReasons[1] : string.Empty;
                    string denial3 = userInput.DenialReasons.Count > 2 ? "-" + userInput.DenialReasons[2] : string.Empty;
                    string denial4 = userInput.DenialReasons.Count > 3 ? "-" + userInput.DenialReasons[3] : string.Empty;
                    string denial5 = userInput.DenialReasons.Count > 4 ? "-" + userInput.DenialReasons[4] : string.Empty;

                    if (MessageBox.Show($"Borrower is denied for {userInput.LetterType} {userInput.LetterOption} because: \n\n{denial1} \n\n{denial2} \n\n{denial3} \n\n{denial4} \n\n{denial5} \n\nThe script will create denial letter with this information, is this correct?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        letterGenerated = lGen.GenerateTheLetter(userInput, bDemos, eData, ecorrBorrower);
                    else
                        return ScriptResults.ShowFormWithData;
                }

                if (letterGenerated)
                    AddComment(userInput, eData.FirstOrDefault(), bDemos.IsValidAddress, borrowerEcorr == null ? false : borrowerEcorr.LetterIndicator);
            }

            if (MessageBox.Show("Would you like to create another letter?", "New Letter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return ScriptResults.ShowFormBlankData;

            return ScriptResults.DoneProcessing;
        }

        private void AddComment(InputData userData, EndorserData eData, bool hadValidMailingAddress, bool borrowerEcorr)
        {
            string letterArc = Letters.Where(p => p.LoanServicingLettersId == userData.LoanServicingLettersId).FirstOrDefault().Arc;

            string borrowerAndCoborrower = DetermineDeliveryMessage(eData, hadValidMailingAddress, borrowerEcorr);
            string comment = DetermineDispositionMessage(userData, borrowerAndCoborrower);

            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = userData.AccountNumber,
                Arc = letterArc,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                ScriptId = ScriptId
            };

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding the ARC: {letterArc}, Comment: {comment} for borrower account: {userData.AccountNumber}.";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        private string DetermineDispositionMessage(InputData userData, string borrowerAndCoborrower)
        {
            string comment = "";
            if (userData.LetterType.ToLower().Contains("discharge/ forgiveness") && userData.DenialReasons[0].ToLower().Contains("fsa"))
            {
                if (userData.DenialReasons[0].ToLower().Contains("approved by fsa"))
                    comment = $"Borrower is approved for {userData.LetterType} {userData.LetterOption} for amount: {userData.AmountForDischarge}. {borrowerAndCoborrower}";
                else
                    comment = $"Sent {userData.LetterType} {userData.LetterOption} to FSA for approval. {borrowerAndCoborrower}";
            }
            else
            {
                string denialReason = string.Empty;
                foreach (string item in userData.DenialReasons)
                    denialReason += item + ",";

                denialReason = denialReason.Remove(denialReason.LastIndexOf(","), 1);
                denialReason = denialReason.Replace("Your", "their");
                denialReason = denialReason.Replace("You", "they");
                denialReason = denialReason.Replace("you", "they");

                comment = $"Borrower is denied for {userData.LetterOption} {userData.LetterType} because {denialReason} {borrowerAndCoborrower}";
            }
            return comment;
        }

        private string DetermineDeliveryMessage(EndorserData eData, bool hadValidMailingAddress, bool borrowerEcorr)
        {
            string message = "";
            bool notifyBorrower = (hadValidMailingAddress || borrowerEcorr);
            bool notifyEndorser = false;
            if (eData != null)
                notifyEndorser = (eData.EndorserDemo.IsValidAddress || eData.EndorserEcorr.LetterIndicator);


            if (eData == null && notifyBorrower)
                message = "Sent letter to borrower.";
            else if (eData == null && !notifyBorrower)
                message = "No letter sent to borrower (invalid mailing address).";
            else if (notifyBorrower && notifyEndorser)
                message = "Sent letter to borrower and co-borrower.";
            else if (!notifyBorrower && notifyEndorser)
                message = "Sent letter to co-borrower.  No letter sent to borrower (invalid mailing address).";
            else if (notifyBorrower && !notifyEndorser)
                message = "Sent letter to borrower.  No letter sent to co-borrower (invalid mailing address).";
            else if (!notifyBorrower && !notifyEndorser)
                message = "No letter sent to borrower or co-borrower (invalid mailing address).";
            return message;
        }
    }
}