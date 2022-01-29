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
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;

namespace ACHSETUP
{
    abstract class BaseBranchProcessor : ScriptBase
    {

        private enum ApprovalLetterCommentsIndicator
        {
            BankAccountHolder,
            Borrower,
            None,
        }

        protected SystemBorrowerDemographics BrwDemos { get; set; }
        protected RecoveryProcessor RecoveryProcessor { get; set; }
        protected const string ARC = "PAUTO";
        private readonly string LETTER_DATA_FILE;
        private readonly string APPROVAL_LETTER_FOR_BANK_ACCOUNT_HOLDER_CALCULATED_PATH_AND_NAME;
        private string DOCUMENT_DIRECTORY { get; } = Efs.GetPath("AccountServicesFolder");
        private const string DENIAL_LETTER = "AUTOPAYDEN";
        private const string APPROVAL_LETTER = "AUTOPAYAPV";
        private const string APPROVAL_LETTER_FOR_BANK_ACCOUNT_HOLDER = "AUTPYAPBNK";

        public BaseBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc)
            : base(ri, scriptID)
        {
            BrwDemos = brwDemos;
            RecoveryProcessor = recoveryProc;
            LETTER_DATA_FILE = $"{Efs.TempFolder}ACHLetterDataFile.txt";
            APPROVAL_LETTER_FOR_BANK_ACCOUNT_HOLDER_CALCULATED_PATH_AND_NAME = $"{DOCUMENT_DIRECTORY}{APPROVAL_LETTER_FOR_BANK_ACCOUNT_HOLDER_CALCULATED_PATH_AND_NAME}";
        }

        abstract public void Process();

        #region Letter Printing

        #region Denial Letter Processing

        /// <summary>
        /// Call when user doesn't need to be asked whether the letter should be created or not.
        /// </summary>
        /// <param name="demos"></param>
        /// <param name="td22Data"></param>
        protected void DenialLetter(SystemBorrowerDemographics demos, TD22CommentData td22Data, DataAccess DA)
        {
            //get denial reasons from the user
            List<string> denials = new List<string>();
            DenialReasonDialog denialOptionUI = new DenialReasonDialog(ref denials);
            denialOptionUI.ShowDialog();

            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateDenialLetter;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
            {
                //create letter
                AddDenialLetterToPrintProcessing(demos, td22Data, denials, DA);
                RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
            }

            //New recovery phase
            phase = RecoveryProcessor.RecoveryPhases.EnterDenialLetterComments;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
            {
                //add comments
                AddDenialLetterComments(demos.Ssn, td22Data, true);
                RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
            }
        }

        /// <summary>
        /// Call when user needs to be asked whether the letter should be created or not,
        /// and needs to pick denial reasons from a form.
        /// </summary>
        /// <param name="demos"></param>
        /// <param name="td22Data"></param>
        /// <param name="promptTextForIfLetterShouldBeGenerated"></param>
        protected void DenialLetter(SystemBorrowerDemographics demos, TD22CommentData td22Data, string promptTextForIfLetterShouldBeGenerated, DataAccess DA)
        {
            if (MessageBox.Show(promptTextForIfLetterShouldBeGenerated, "Print Denial Letter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //get denial reasons from the user
                List<string> denials = new List<string>();
                DenialReasonDialog denialOptionUI = new DenialReasonDialog(ref denials);
                denialOptionUI.ShowDialog();

                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateDenialLetter;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //create letter
                    AddDenialLetterToPrintProcessing(demos, td22Data, denials, DA);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }

                //New recovery phase
                phase = RecoveryProcessor.RecoveryPhases.EnterDenialLetterComments;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //add comments
                    AddDenialLetterComments(demos.Ssn, td22Data, true);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }
            }
            else
            {
                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.EnterDenialLetterComments;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //add comments
                    AddDenialLetterComments(demos.Ssn, td22Data, false);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }
            }
        }

        /// <summary>
        /// Call when user needs to be asked whether the letter should be created or not.
        /// </summary>
        /// <param name="demos"></param>
        /// <param name="td22Data"></param>
        /// <param name="denialReason"></param>
        /// <param name="promptTextForIfLetterShouldBeGenerated"></param>
        protected void DenialLetter(SystemBorrowerDemographics demos, TD22CommentData td22Data, string denialReason, string promptTextForIfLetterShouldBeGenerated, DataAccess DA)
        {
            if (MessageBox.Show(promptTextForIfLetterShouldBeGenerated, "Print Denial Letter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //create letter
                List<string> denials = new List<string>();
                denials.Add(denialReason);

                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateDenialLetter;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    AddDenialLetterToPrintProcessing(demos, td22Data, denials, DA);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }

                //New recovery phase
                phase = RecoveryProcessor.RecoveryPhases.EnterDenialLetterComments;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //add comments
                    AddDenialLetterComments(demos.Ssn, td22Data, true);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }
            }
            else
            {

                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.EnterDenialLetterComments;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //add comments
                    AddDenialLetterComments(demos.Ssn, td22Data, false);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }
            }
        }

        /// <summary>
        /// Call when user needs to be asked whether the letter should be created or not,
        /// and the denial reasons are already known.
        /// </summary>
        /// <param name="demos"></param>
        /// <param name="td22Data"></param>
        /// <param name="denialReasons"></param>
        /// <param name="promptTextForIfLetterShouldBeGenerated"></param>
        protected void DenialLetter(SystemBorrowerDemographics demos, TD22CommentData td22Data, List<string> denialReasons, string promptTextForIfLetterShouldBeGenerated, DataAccess DA)
        {
            if (MessageBox.Show(promptTextForIfLetterShouldBeGenerated, "Print Denial Letter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateDenialLetter;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //create letter
                    AddDenialLetterToPrintProcessing(demos, td22Data, denialReasons, DA);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }

                //New recovery phase
                phase = RecoveryProcessor.RecoveryPhases.EnterDenialLetterComments;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //add comments
                    AddDenialLetterComments(demos.Ssn, td22Data, true);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }
            }
            else
            {
                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.EnterDenialLetterComments;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //add comments
                    AddDenialLetterComments(demos.Ssn, td22Data, false);
                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //add phase to recovery log
                }
            }
        }

        //adds denial comments
        private void AddDenialLetterComments(string ssn, TD22CommentData td22Data, bool letterSent)
        {
            if (letterSent)
                td22Data.Comment = $"{td22Data.Comment}  Denial letter sent to borrower.";
            if (td22Data.Loans.Count == 0)
                AddComment(ssn, td22Data.Comment, ArcData.ArcType.Atd22AllLoans);
            else
                AddComment(ssn, td22Data.Comment, ArcData.ArcType.Atd22ByLoan, null, td22Data.Loans);
        }

        private void AddComment(string ssn, string comment, ArcData.ArcType arcType, string otherArc = null, List<int> loans = null)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = ssn,
                Arc = otherArc ?? ARC,
                ArcTypeSelected = arcType,
                Comment = comment,
                ScriptId = ScriptId
            };
            if (arcType == ArcData.ArcType.Atd22ByLoan)
                arc.LoanSequences = loans;
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding the Arc: {ARC} to borrower account: {BrwDemos.AccountNumber}.";
                Dialog.Error.Ok(message);
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        //generates denial letter and images it
        private void AddDenialLetterToPrintProcessing(SystemBorrowerDemographics demos, TD22CommentData td22Data, List<string> denials, DataAccess DA)
        {
            string letterData = GetLetterDataDenial(demos, denials);
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, DENIAL_LETTER, letterData, demos.AccountNumber, "MA2324");
            //Get Coborrowers that need to be processed, if any
            List<CoborrowerDemographics> coborrowerDemos = DA.GetCoBorrowersForBorrowerByLoan(demos.Ssn);
            if (coborrowerDemos != null && coborrowerDemos.Count > 0)
            {
                //send only to the affected loans
                if (td22Data != null && td22Data.Loans != null && td22Data.Loans.Count > 0)
                {
                    List<CoborrowerDemographics> toSend = new List<CoborrowerDemographics>();
                    foreach (CoborrowerDemographics c in coborrowerDemos)
                    {
                        //if the list of coborrowers to send to does not contain a borrowers's coborrower for a given loan sequence
                        //*Note* this is not efficient but the data size should be small
                        if (td22Data.Loans.Contains(c.LoanSequence) && !toSend.Select(p => p.CoBorrowerSSN).Contains(c.CoBorrowerSSN))
                            toSend.Add(c);
                    }
                    //Create the print processing records for the loans tied to loan sequences
                    foreach (CoborrowerDemographics c in toSend)
                    {
                        string coborrowerLetterData = GetLetterDataDenialCoborrower(c, denials);
                        EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, DENIAL_LETTER, coborrowerLetterData, c.AccountNumber, "MA2324", c.BorrowerSSN);
                    }
                }
                //send to all loans
                else
                {
                    List<CoborrowerDemographics> toSend = new List<CoborrowerDemographics>();
                    foreach (CoborrowerDemographics c in coborrowerDemos)
                    {
                        //if the list of coborrowers to send to does not contain a borrowers's coborrower
                        //*Note* this is not efficient but the data size should be small
                        if (!toSend.Select(p => p.CoBorrowerSSN).Contains(c.CoBorrowerSSN))
                            toSend.Add(c);
                    }
                    foreach (CoborrowerDemographics c in toSend)
                    {
                        string coborrowerLetterData = GetLetterDataDenialCoborrower(c, denials);
                        EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, DENIAL_LETTER, coborrowerLetterData, c.AccountNumber, "MA2324", c.BorrowerSSN);
                    }
                }
            }
        }

        public string GetLetterDataDenial(SystemBorrowerDemographics demos, List<string> denials)
        {
            string formatted = GenerateDenialString(denials);
            //KeyLine,AccountNumber,Name,Address1,Address2,City,State,Zip,Country,D1,D2,D3,D4,D5
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            return $"{keyLine},{demos.AccountNumber},{demos.FirstName} {demos.LastName},{demos.Address1},{demos.Address2},{demos.City},{demos.State},{demos.ZipCode},{demos.Country},{formatted}";
        }

        public string GetLetterDataDenialCoborrower(CoborrowerDemographics demos, List<string> denials)
        {
            string formatted = GenerateDenialString(denials);   
            //KeyLine,AccountNumber,Name,Address1,Address2,City,State,Zip,Country,D1,D2,D3,D4,D5
            string keyLine = DocumentProcessing.ACSKeyLine(demos.CoBorrowerSSN, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            if (demos.Suffix.IsPopulated())
                demos.LastName = $"{demos.LastName} {demos.Suffix}";
            return $"{keyLine},{demos.BorrowerAccountNumber},{demos.FirstName} {demos.LastName},{demos.Address1},{demos.Address2},{demos.City},{demos.State},{demos.Zip},{demos.ForeignCountry},{formatted}";
        }

        public static string GenerateDenialString(List<string> denials)
        {
            int max = 5;
            int min = max - denials.Count();
            string formatted = "";
            for (int n = 0; n < denials.Count(); n++)
                formatted += "\"- " + denials[n] + "\",";

            if (min == 0)
                formatted = formatted.SafeSubString(0, formatted.Length - 1);
            else
                for (int n = 0; n < min - 1; n++)
                    formatted += ",";

            return formatted;
        }

        #endregion

        #region Approval Letter Processing

        /// <summary>
        /// Call when a letter and comments need to be created.
        /// </summary>
        protected void ApprovalLetter(SystemBorrowerDemographics borrowerDemos, TD22CommentData td22Data, string nextPayDueDate, string additionalAmount, DataAccess DA)
        {
            //tweak fields as needed
            TweakAdditionalAmountAndNextPayDueDateEntries(ref nextPayDueDate, ref additionalAmount);

            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateApprovalLetter;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
            {
                //create letter
                GenerateApprovalLetterAndImage(borrowerDemos, td22Data, APPROVAL_LETTER, $"{borrowerDemos.FirstName} {borrowerDemos.LastName}", nextPayDueDate, additionalAmount, borrowerDemos.Ssn, borrowerDemos.AccountNumber, DA);
                RecoveryProcessor.UpdateLogWithNewPhase(phase); //update log with new phase
            }

            //new recovery phase
            phase = RecoveryProcessor.RecoveryPhases.EnterApprovalLetterComments;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
            {
                //add comments
                AddApprovalLetterComments(borrowerDemos.Ssn, td22Data, ApprovalLetterCommentsIndicator.None, null, true);
                RecoveryProcessor.UpdateLogWithNewPhase(phase); //update log with new phase
            }
        }

        /// <summary>
        /// Call when user needs to be prompted whether to create the letter or not, and if the bank account holder needs to receive the letter instead of the borrower.  Comments go in regardless.
        /// </summary>
        protected void ApprovalLetter(SystemBorrowerDemographics borrowerDemos, TD22CommentData td22Data, string nextPayDueDate, string additionalAmount, string promptTextForIfLetterShouldBeGenerated, bool letterIsForBankAccountHolder, DataAccess DA)
        {
            //tweak fields as needed
            TweakAdditionalAmountAndNextPayDueDateEntries(ref nextPayDueDate, ref additionalAmount);

            //prompt user
            if (MessageBox.Show(promptTextForIfLetterShouldBeGenerated, "Print Approval Letter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (letterIsForBankAccountHolder)
                {
                    //bank account holder processing
                    SystemBorrowerDemographics bankAccountHolderDemos = null;
                    if (InformationHasBeenGatheredForAccountHolder(ref bankAccountHolderDemos) == false)
                        return;
                    else
                    {
                        RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateApprovalLetterForBankAccountHolder;
                        if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
                        {
                            //create letter
                            GenerateApprovalLetterAndImage(bankAccountHolderDemos, td22Data, APPROVAL_LETTER_FOR_BANK_ACCOUNT_HOLDER,
                                                           $"{borrowerDemos.FirstName} {borrowerDemos.LastName}",
                                                           nextPayDueDate, additionalAmount, borrowerDemos.Ssn, borrowerDemos.AccountNumber, DA);
                            RecoveryProcessor.UpdateLogWithNewPhase(phase); //update log with new phase
                        }

                        //New recovery phase
                        phase = RecoveryProcessor.RecoveryPhases.EnterApprovalLetterCommentsForBankAccountHolder;
                        if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
                        {
                            //add comments
                            AddApprovalLetterComments(borrowerDemos.Ssn, td22Data, ApprovalLetterCommentsIndicator.BankAccountHolder, bankAccountHolderDemos, true);
                            RecoveryProcessor.UpdateLogWithNewPhase(phase); //update log with new phase
                        }
                    }
                }
                else
                {
                    //borrower processing
                    RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateApprovalLetter;
                    if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
                    {
                        //create letter
                        GenerateApprovalLetterAndImage(borrowerDemos, td22Data, APPROVAL_LETTER,
                                                           $"{borrowerDemos.FirstName} {borrowerDemos.LastName}",
                                                           nextPayDueDate, additionalAmount, borrowerDemos.Ssn, borrowerDemos.AccountNumber, DA);
                        RecoveryProcessor.UpdateLogWithNewPhase(phase); //update log with new phase
                    }

                    //new recovery phase
                    phase = RecoveryProcessor.RecoveryPhases.EnterApprovalLetterComments;
                    if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
                    {
                        //add comments
                        AddApprovalLetterComments(borrowerDemos.Ssn, td22Data, ApprovalLetterCommentsIndicator.Borrower, null, true);
                        RecoveryProcessor.UpdateLogWithNewPhase(phase); //update log with new phase
                    }
                }
            }
            else
            {
                //no letter processing
                //be sure that the letter isn't for the bank account holder.
                if (letterIsForBankAccountHolder == false)
                {
                    RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateApprovalLetter;
                    if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
                    {
                        //add comments
                        AddApprovalLetterComments(borrowerDemos.Ssn, td22Data, ApprovalLetterCommentsIndicator.Borrower, null, false);
                        RecoveryProcessor.UpdateLogWithNewPhase(phase); //update log with new phase
                    }
                }
            }
        }

        //adds denial comments
        private void AddApprovalLetterComments(string ssn, TD22CommentData td22Data, ApprovalLetterCommentsIndicator commentAdditionIndicator, SystemBorrowerDemographics bankAccountHolderDemos, bool sentLetter)
        {
            //tweak comment if needed
            if (commentAdditionIndicator == ApprovalLetterCommentsIndicator.BankAccountHolder && sentLetter)
                td22Data.Comment = $"{td22Data.Comment} {bankAccountHolderDemos.FirstName}";
            else if (commentAdditionIndicator == ApprovalLetterCommentsIndicator.Borrower && sentLetter)
                td22Data.Comment = $"{td22Data.Comment} Approval letter sent to borrower.";

            //add comment
            if (td22Data.Loans.Count == 0)
                AddComment(ssn, td22Data.Comment, ArcData.ArcType.Atd22AllLoans);
            else
                AddComment(ssn, td22Data.Comment, ArcData.ArcType.Atd22ByLoan, null, td22Data.Loans);
        }

        //generates denial letter and images it
        private void GenerateApprovalLetterAndImage(SystemBorrowerDemographics demos, TD22CommentData td22Data, string letterID, string borrowerName, string autoPayBeginDate, string additionalAmount, string ssn, string accountNumber, DataAccess DA)
        {
            string letterData = GetLetterDataApproval(demos, ssn, accountNumber, borrowerName, autoPayBeginDate, additionalAmount);
            int? result = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterID, letterData, accountNumber, "MA2324");
            //Get Coborrowers that need to be processed, if any
            List<CoborrowerDemographics> coborrowerDemos = DA.GetCoBorrowersForBorrowerByLoan(demos.Ssn);
            if (coborrowerDemos != null && coborrowerDemos.Count > 0)
            {
                //send only to the affected loans
                if (td22Data != null && td22Data.Loans != null && td22Data.Loans.Count > 0)
                {
                    List<CoborrowerDemographics> toSend = new List<CoborrowerDemographics>();
                    foreach (CoborrowerDemographics c in coborrowerDemos)
                    {
                        //if the list of coborrowers to send to does not contain a borrowers's coborrower for a given loan sequence
                        //*Note* this is not efficient but the data size should be small
                        if (td22Data.Loans.Contains(c.LoanSequence) && !toSend.Select(p => p.CoBorrowerSSN).Contains(c.CoBorrowerSSN))
                            toSend.Add(c);
                    }
                    //Create the print processing records for the loans tied to loan sequences
                    foreach (CoborrowerDemographics c in toSend)
                    {
                        string coborrowerLetterData = GetLetterDataApprovalCoborrower(c, borrowerName, autoPayBeginDate, additionalAmount);
                        EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterID, coborrowerLetterData, c.AccountNumber, "MA2324", c.BorrowerSSN);
                    }
                }
                //send to all loans
                else
                {
                    List<CoborrowerDemographics> toSend = new List<CoborrowerDemographics>();
                    foreach (CoborrowerDemographics c in coborrowerDemos)
                    {
                        //if the list of coborrowers to send to does not contain a borrowers's coborrower
                        //*Note* this is not efficient but the data size should be small
                        if (!toSend.Select(p => p.CoBorrowerSSN).Contains(c.CoBorrowerSSN))
                            toSend.Add(c);
                    }
                    foreach (CoborrowerDemographics c in toSend)
                    {
                        string coborrowerLetterData = GetLetterDataApprovalCoborrower(c, borrowerName, autoPayBeginDate, additionalAmount);
                        EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterID, coborrowerLetterData, c.AccountNumber, "MA2324", c.BorrowerSSN);
                    }
                }
            }
        }

        public string GetLetterDataApproval(SystemBorrowerDemographics demos, string ssn, string accountNumber, string borrowerName, string autoPayBeginDate, string additionalAmount)
        {
            //KeyLine,AccountNumber,Name,Address1,Address2,City,State,Zip,Country,BorrowerName,AutoPayBeginDate,AdditionalAmount
            string keyLine = DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string name = ((demos is SystemBorrowerDemographics) ? $"{demos.FirstName} {demos.LastName}" : demos.FirstName);
            return $"{keyLine},{accountNumber},{name},{demos.Address1},{demos.Address2},{demos.City},{demos.State},{demos.ZipCode},{demos.Country},{borrowerName},{autoPayBeginDate},{additionalAmount}";
        }

        public string GetLetterDataApprovalCoborrower(CoborrowerDemographics demos, string borrowerName, string autoPayBeginDate, string additionalAmount)
        {
            //KeyLine,AccountNumber,Name,Address1,Address2,City,State,Zip,Country,BorrowerName,AutoPayBeginDate,AdditionalAmount
            string keyLine = DocumentProcessing.ACSKeyLine(demos.CoBorrowerSSN, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            if (demos.Suffix.IsPopulated())
                demos.LastName = $"{demos.LastName} {demos.Suffix}";
            return $"{keyLine},{demos.BorrowerAccountNumber},{demos.FirstName} {demos.LastName},{demos.Address1},{demos.Address2},{demos.City},{demos.State},{demos.Zip},{demos.ForeignCountry},{borrowerName},{autoPayBeginDate},{additionalAmount}";
        }

        //gathers and validates account holder information
        private bool InformationHasBeenGatheredForAccountHolder(ref SystemBorrowerDemographics bankAccountHolderDemos)
        {
            bankAccountHolderDemos = new BankAccountHolderDemos();
            bankAccountHolderDemos.FirstName = string.Empty;
            bankAccountHolderDemos.Address1 = string.Empty;
            bankAccountHolderDemos.Address2 = string.Empty;
            bankAccountHolderDemos.City = string.Empty;
            bankAccountHolderDemos.State = string.Empty;
            bankAccountHolderDemos.ZipCode = string.Empty;

            while (true)
            {
                AccountHolderDemosDialog acctHldrDemosUI = new AccountHolderDemosDialog(bankAccountHolderDemos);
                if (acctHldrDemosUI.ShowDialog() == DialogResult.OK)
                {
                    if (IsValidAccountHolderData(bankAccountHolderDemos))
                        return true;//everything is good to go
                }
                else
                    return false;
            }
        }

        //confirms that all needed data is provided and is valid for the account holder
        private bool IsValidAccountHolderData(SystemBorrowerDemographics bankAccountHolderDemos)
        {
            string problem = string.Empty;
            if (bankAccountHolderDemos.FirstName.Length == 0)
                problem = $"{problem}{Environment.NewLine}The account holder's name cannot be blank.";
            if (bankAccountHolderDemos.Address1.Length == 0)
                problem = $"{problem}{Environment.NewLine}The first address line cannot be blank.";
            if (bankAccountHolderDemos.City.Length == 0)
                problem = $"{problem}{Environment.NewLine}A city is required.";
            if (bankAccountHolderDemos.State.Length != 2)
                problem = $"{problem}{Environment.NewLine}A two-letter state code is required.";
            if (bankAccountHolderDemos.ZipCode.Length < 5)
                problem = $"{problem}{Environment.NewLine}A 5-digit or 5+4 zip code is required.";
            //Let the user know of any problems that were found.
            if (problem.Length > 0)
            {
                Dialog.Error.Ok($"Please correct the following items:{Environment.NewLine}{problem}", "Account Holder Data Invalid");
                return false;
            }
            return true;
        }

        //makes tweaks to specified data values
        private void TweakAdditionalAmountAndNextPayDueDateEntries(ref string nextPayDueDate, ref string additionalAmount)
        {
            //additional amount
            if (additionalAmount.IsNullOrEmpty())
                additionalAmount = "0";

            //next pay due date
            if (nextPayDueDate.IsNullOrEmpty())
                nextPayDueDate = "N/A";
            else
                if (nextPayDueDate.ToDate().Date <= DateTime.Today.AddDays(3))
                nextPayDueDate = nextPayDueDate.ToDate().Date.AddMonths(1).ToString("MM/dd/yyyy");
        }

        #endregion

        #endregion

        /// <summary>
        /// Gets the next payment due date from TS26, using the first loan with a balance.
        /// </summary>
        protected DateTime? GetNextPaymentDueDate(string ssn)
        {
            return GetNextPaymentDueDate(ssn, null);
        }

        /// <summary>
        /// Gets the next payment due date from TS26, using the first loan whose sequence is found in the passed-in collection.
        /// </summary>
        protected DateTime? GetNextPaymentDueDate(string ssn, IEnumerable<int> loanSequences)
        {
            RI.FastPath($"TX3Z/ITS26{ssn}");
            if (RI.ScreenCode == "TSX28")
            {
                //Selection screen. Select the first loan that's in our list,
                //or the first loan with a balance if the list is null.
                bool foundAUsefulLoan = false;
                while (!foundAUsefulLoan && RI.MessageCode != "90007")
                {
                    for (int row = 8; !RI.CheckForText(row, 3, " "); row++)
                    {
                        if (loanSequences == null)
                        {
                            double balance = RI.GetText(row, 59, 10).Replace(",", "").ToDouble();
                            foundAUsefulLoan = (balance > 0);
                        }
                        else
                        {
                            int loanSequence = RI.GetText(row, 14, 4).ToInt();
                            foundAUsefulLoan = (loanSequences.Contains(loanSequence));
                        }
                        if (foundAUsefulLoan)
                        {
                            RI.PutText(21, 12, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter);
                            break;
                        }
                    }
                    if (!foundAUsefulLoan)
                        RI.Hit(ReflectionInterface.Key.F8);
                }
            }

            //We're assuming target screen (TSX29) at this point. Go to the billing information page to get the next pay due date.
            RI.Hit(ReflectionInterface.Key.Enter, 2);
            try
            {
                return RI.GetText(8, 45, 8).ToDate();
            }
            catch
            {
                Dialog.Def.Ok("Could not determine the next pay due.  Please research this and Hit <Insert> when ready to proceed.");
                RI.PauseForInsert();
                using frmCannotDetermineNextPayDue frm = new frmCannotDetermineNextPayDue();
                if (frm.ShowDialog() == DialogResult.Cancel)
                    AddComment(BrwDemos.Ssn, "", ArcData.ArcType.Atd22AllLoans, "AHSOT");
                return frm.Result.ToDateNullable();
            }
        }

        //Commenting out, the BU said they might want this later.
        //protected bool GenerateLetterWithoutNextPay(SystemBorrowerDemographics demos, string borrowerName, string autoPayBeginDate, string additionalAmount, string ssn, string accountNumber)
        //{
        //    //create data file
        //    using (FileStream fs = new FileStream(LETTER_DATA_FILE, FileMode.CreateNew))
        //    {
        //        using StreamWriter sw = new StreamWriter(fs);
        //        sw.WriteLine($"SSN, KeyLine, AccountNumber, Name, Address1, Address2, City, State, Zip, Country, BorrowerName, AutoPayBeginDate, AdditionalAmount");
        //        string keyLine = DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
        //        string name = ((demos is SystemBorrowerDemographics) ? $"{demos.FirstName} {demos.LastName}" : demos.FirstName);
        //        sw.WriteLine($"{ssn},{keyLine},{accountNumber},{name},{demos.Address1},{demos.Address2},{demos.City},{demos.State},{demos.ZipCode},{demos.Country},{borrowerName},{autoPayBeginDate},{additionalAmount}");
        //        sw.Close();
        //    }

        //    DocumentProcessing.AddBarcodesForBatchProcessing(LETTER_DATA_FILE, "AccountNumber", APPROVAL_LETTER, false, DocumentProcessing.LetterRecipient.Borrower);
        //    string docPath = DOCUMENT_DIRECTORY;

        //    File.Copy($"{docPath}{APPROVAL_LETTER}.doc", $"{Efs.TempFolder}{APPROVAL_LETTER}.doc", true);
        //    DocumentProcessing.SaveDocs(APPROVAL_LETTER, LETTER_DATA_FILE, $"{Efs.GetPath("CompassACH")}{APPROVAL_LETTER}{accountNumber}.doc");
        //    Repeater.TryRepeatedly(() => File.Delete(LETTER_DATA_FILE));

        //    Dialog.Info.Ok($"The Next Pay Due field could not be determined.  A letter has been saved to {docPath} without the Next Pay Due data please review", "Error");
        //    return false;
        //}
    }
}