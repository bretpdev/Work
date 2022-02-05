using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
    abstract class BaseBranchProcessor : CompassAchSetupFed
    {
        private enum ApprovalLetterCommentsIndicator
        {
            BankAccountHolder,
            Borrower,
            None,
        }
        protected LetterOption _option = LetterOption.PrintLetter;
        protected enum LetterOption
        {
            PrintLetter,
            SaveLetter
        }

        protected SystemBorrowerDemographics BrwDemos;
        protected RecoveryProcessor RecoveryProcessor;
        protected new ProcessLogData ProcessLogData;
        protected const string ARC = "PAUTO";
        private const string DENIAL_LETTER = "APDNYFED";
        private const string APPROVAL_LETTER = "APAPPR1FED";
        private const string APPROVAL_LETTER_FOR_BANK_ACCOUNT_HOLDER = "APBNKFED";
        private const string IMAGING_DOC_ID = "LSACH";

        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }

        public BaseBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptId, RecoveryProcessor recoveryProc, ProcessLogData processLogData, DataAccess DA, ProcessLogRun logRun)
            : base(ri)
        {
            this.DA = DA;
            LogRun = logRun;
            BrwDemos = brwDemos;
            RecoveryProcessor = recoveryProc;
            ProcessLogData = processLogData;
        }

        abstract public void Process();

        #region Letter Printing

        #region Denial Letter Processing

        /// <summary>
        /// Call when user doesn't need to be asked whether the letter should be created or not.
        /// </summary>
        protected void DenialLetter(SystemBorrowerDemographics demos, TD22CommentData td22Data)
        {
            //get denial reasons from the user
            List<string> denials = new List<string>();
            DenialReasonDialog denialOptionUI = new DenialReasonDialog(ref denials);
            denialOptionUI.ShowDialog();

            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateDenialLetter;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
            {
                //create letter
                GenerateDenialLetterAndImage(demos, td22Data, denials);
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
        protected void DenialLetter(SystemBorrowerDemographics demos, TD22CommentData td22Data, string promptTextForIfLetterShouldBeGenerated)
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
                    GenerateDenialLetterAndImage(demos, td22Data, denials);
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
        protected void DenialLetter(SystemBorrowerDemographics demos, TD22CommentData td22Data, string denialReason, string promptTextForIfLetterShouldBeGenerated)
        {
            if (MessageBox.Show(promptTextForIfLetterShouldBeGenerated, "Print Denial Letter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //create letter
                List<string> denials = new List<string>();
                denials.Add(denialReason);

                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateDenialLetter;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    GenerateDenialLetterAndImage(demos, td22Data, denials);
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
        protected void DenialLetter(SystemBorrowerDemographics demos, TD22CommentData td22Data, List<string> denialReasons, string promptTextForIfLetterShouldBeGenerated)
        {
            if (MessageBox.Show(promptTextForIfLetterShouldBeGenerated, "Print Denial Letter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateDenialLetter;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
                {
                    //create letter
                    GenerateDenialLetterAndImage(demos, td22Data, denialReasons);
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
        /// Adds denial comments
        /// </summary>
        private void AddDenialLetterComments(string ssn, TD22CommentData td22Data, bool letterSent)
        {

            if (letterSent)
            {
                td22Data.Comment = string.Format("{0}  Denial letter sent to borrower.", td22Data.Comment);
            }
            if (td22Data.Loans.Count == 0)
            {
                if (!Atd22AllLoans(ssn, ARC, td22Data.Comment, "", ScriptId, td22Data.PausePlease))
                {
                    MessageBox.Show(string.Format("You need access to the \"{0}\" ARC.  Please contact Systems Support.", ARC), "Access To ARC Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndDllScript();
                }
            }
            else
            {
                if (!Atd22ByLoan(ssn, ARC, td22Data.Comment, "", td22Data.Loans, ScriptId, td22Data.PausePlease))
                {
                    MessageBox.Show(string.Format("You need access to the \"{0}\" ARC.  Please contact Systems Support.", ARC), "Access To ARC Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndDllScript();
                }
            }

        }

        /// <summary>
        /// generates denial letter and images it
        /// </summary>
        private void GenerateDenialLetterAndImage(SystemBorrowerDemographics demos, TD22CommentData td22Data,List<string> denials)
        {
            //Create letter
            string letterData = LetterDataFormatter.GenerateDeniedLetterData(demos, denials);
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, DENIAL_LETTER, letterData, demos.AccountNumber, "MA4481");
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
                        {
                            toSend.Add(c);
                        }
                    }
                    //Create the print processing records for the loans tied to loan sequences
                    foreach (CoborrowerDemographics c in toSend)
                    {
                        string coborrowerLetterData = LetterDataFormatter.GenerateDeniedLetterDataCoborrower(c, denials);
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
                        {
                            toSend.Add(c);
                        }
                    }
                    foreach (CoborrowerDemographics c in toSend)
                    {
                        string coborrowerLetterData = LetterDataFormatter.GenerateDeniedLetterDataCoborrower(c, denials);
                        EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, DENIAL_LETTER, coborrowerLetterData, c.AccountNumber, "MA2324", c.BorrowerSSN);
                    }
                }
            }
        }

        #endregion

        #region Approval Letter Processing

        /// <summary>
        /// Call when a letter and comments need to be created.
        /// </summary>
        protected void ApprovalLetter(SystemBorrowerDemographics borrowerDemos, TD22CommentData td22Data, string nextPayDueDate, string additionalAmount)
        {
            //tweak fields as needed
            TweakAdditionalAmountAndNextPayDueDateEntries(ref nextPayDueDate, ref additionalAmount);

            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateApprovalLetter;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
            {
                //create letter
                GenerateApprovalLetterAndImage(borrowerDemos, td22Data, APPROVAL_LETTER, string.Format("{0} {1}", borrowerDemos.FirstName, borrowerDemos.LastName), nextPayDueDate, additionalAmount, borrowerDemos.Ssn, borrowerDemos.AccountNumber, false);
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
        protected void ApprovalLetter(SystemBorrowerDemographics borrowerDemos, TD22CommentData td22Data, string nextPayDueDate, string additionalAmount, string promptTextForIfLetterShouldBeGenerated, bool letterIsForBankAccountHolder)
        {
            //tweak fields as needed
            TweakAdditionalAmountAndNextPayDueDateEntries(ref nextPayDueDate, ref additionalAmount);

            //prompt user
            if (MessageBox.Show(promptTextForIfLetterShouldBeGenerated, "Print Approval Letter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (letterIsForBankAccountHolder)
                {
                    //bank account holder processing
                    ProcessBankAccountHolder(borrowerDemos, td22Data, nextPayDueDate, additionalAmount);
                }
                else
                {
                    //borrower processing
                    ProcessBorrower(borrowerDemos, td22Data, nextPayDueDate, additionalAmount);
                }
            }
            else
            {
                //be sure that the letter isn't for the bank account holder.
                if (!letterIsForBankAccountHolder)
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

        /// <summary>
        /// Bank Account Holder Processing
        /// </summary>
        private bool ProcessBankAccountHolder(SystemBorrowerDemographics borrowerDemos, TD22CommentData td22Data, string nextPayDueDate, string additionalAmount)
        {
            SystemBorrowerDemographics bankAccountHolderDemos = null;
            if (InformationHasBeenGatheredForAccountHolder(ref bankAccountHolderDemos) == false)
            {
                return false;
            }
            else
            {
                bankAccountHolderDemos.AccountNumber = borrowerDemos.AccountNumber;
                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateApprovalLetterForBankAccountHolder;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
                {
                    //create letter
                    GenerateApprovalLetterAndImage(bankAccountHolderDemos, td22Data, APPROVAL_LETTER_FOR_BANK_ACCOUNT_HOLDER,
                                                   string.Format("{0} {1}", borrowerDemos.FirstName, borrowerDemos.LastName),
                                                   nextPayDueDate, additionalAmount, borrowerDemos.Ssn, borrowerDemos.AccountNumber, true);
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
            return true;
        }

        /// <summary>
        /// Process Borrower Approval Letter
        /// </summary>
        private void ProcessBorrower(SystemBorrowerDemographics borrowerDemos, TD22CommentData td22Data, string nextPayDueDate, string additionalAmount)
        {
            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.GenerateApprovalLetter;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery phase
            {
                //create letter
                GenerateApprovalLetterAndImage(borrowerDemos, td22Data, APPROVAL_LETTER,
                                                   string.Format("{0} {1}", borrowerDemos.FirstName, borrowerDemos.LastName),
                                                   nextPayDueDate, additionalAmount, borrowerDemos.Ssn, borrowerDemos.AccountNumber, false);
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
        /// adds denial comments
        /// </summary>
        private void AddApprovalLetterComments(string ssn, TD22CommentData td22Data, ApprovalLetterCommentsIndicator commentAdditionIndicator, SystemBorrowerDemographics bankAccountHolderDemos, bool LetterSent)
        {
            //tweak comment if needed
            if (commentAdditionIndicator == ApprovalLetterCommentsIndicator.BankAccountHolder)
            {
                td22Data.Comment = string.Format("{0} {1}", td22Data.Comment, bankAccountHolderDemos.FirstName);
            }
            else if (commentAdditionIndicator == ApprovalLetterCommentsIndicator.Borrower && LetterSent)
            {
                td22Data.Comment = string.Format("{0} Approval letter sent to borrower.", td22Data.Comment);
            }
            else if (commentAdditionIndicator == ApprovalLetterCommentsIndicator.Borrower && !LetterSent)
            {
                //Do nothing else to the comment.
            }

                //add comment
                if (td22Data.Loans.Count == 0)
            {
                if (!Atd22AllLoans(ssn, ARC, td22Data.Comment, "", ScriptId, td22Data.PausePlease))
                {
                    MessageBox.Show(string.Format("You need access to the \"{0}\" ARC.  Please contact Systems Support.", ARC), "Access To ARC Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndDllScript();
                }
            }
            else
            {
                if (!Atd22ByLoan(ssn, ARC, td22Data.Comment, "", td22Data.Loans, ScriptId, td22Data.PausePlease))
                {
                    MessageBox.Show(string.Format("You need access to the \"{0}\" ARC.  Please contact Systems Support.", ARC), "Access To ARC Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndDllScript();
                }
            }
        }

        /// <summary>
        /// Generates denial letter and images it
        /// </summary>
        private void GenerateApprovalLetterAndImage(SystemBorrowerDemographics demos, TD22CommentData td22Data, string letterID, string borrowerName, string autoPayBeginDate, string additionalAmount, string ssn, string accountNumber, bool bankAccountHolder)
        {
            string letterData = LetterDataFormatter.GenerateApprovedLetterData(demos, borrowerName, autoPayBeginDate, additionalAmount, ssn, accountNumber);
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterID, letterData, accountNumber, "MA4481");
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
                        {
                            toSend.Add(c);
                        }
                    }
                    //Create the print processing records for the loans tied to loan sequences
                    foreach (CoborrowerDemographics c in toSend)
                    {
                        string coborrowerLetterData = LetterDataFormatter.GenerateApprovedLetterDataCoborrower(c, borrowerName, autoPayBeginDate, additionalAmount);
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
                        {
                            toSend.Add(c);
                        }
                    }
                    foreach (CoborrowerDemographics c in toSend)
                    {
                        string coborrowerLetterData = LetterDataFormatter.GenerateApprovedLetterDataCoborrower(c, borrowerName, autoPayBeginDate, additionalAmount);
                        EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterID, coborrowerLetterData, c.AccountNumber, "MA2324", c.BorrowerSSN);
                    }
                }
            }
        }

        /// <summary>
        /// Gathers and validates account holder information
        /// </summary>
        private bool InformationHasBeenGatheredForAccountHolder(ref SystemBorrowerDemographics bankAccountHolderDemos)
        {
            bankAccountHolderDemos = new SystemBorrowerDemographics();
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
                    {
                        //everything is good to go
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Confirms that all needed data is provided and is valid for the account holder
        /// </summary>
        private bool IsValidAccountHolderData(SystemBorrowerDemographics bankAccountHolderDemos)
        {
            string problem = string.Empty;
            if (bankAccountHolderDemos.FirstName.Length == 0)
                problem = string.Format("{0}{1}The account holder's name cannot be blank.", problem, Environment.NewLine);
            if (bankAccountHolderDemos.Address1.Length == 0)
                problem = string.Format("{0}{1}The first address line cannot be blank.", problem, Environment.NewLine);
            if (bankAccountHolderDemos.City.Length == 0)
                problem = string.Format("{0}{1}A city is required.", problem, Environment.NewLine);
            if (bankAccountHolderDemos.State.Length != 2)
                problem = string.Format("{0}{1}A two-letter state code is required.", problem, Environment.NewLine);
            if (bankAccountHolderDemos.ZipCode.Length < 5)
                problem = string.Format("{0}{1}A 5-digit or 5+4 zip code is required.", problem, Environment.NewLine);
            //Let the user know of any problems that were found.
            if (problem.Length > 0)
            {
                MessageBox.Show(string.Format("Please correct the following items:{0}{1}", Environment.NewLine, problem), "Account Holder Data Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Makes tweaks to specified data values
        /// </summary>
        private void TweakAdditionalAmountAndNextPayDueDateEntries(ref string nextPayDueDate, ref string additionalAmount)
        {
            //additional amount
            if (additionalAmount.IsNullOrEmpty())
                additionalAmount = "0";

            //next pay due date
            if (additionalAmount.IsNullOrEmpty())
                nextPayDueDate = "N/A";
            else
                if (DateTime.Parse(nextPayDueDate).Date <= DateTime.Today.AddDays(3))
                    nextPayDueDate = DateTime.Parse(nextPayDueDate).Date.AddMonths(1).ToString("MM/dd/yyyy");
        }

        #endregion

        #endregion

        /// <summary>
        /// Gets the next payment due date from TS26, using the first loan with a balance.
        /// </summary>
        protected DateTime GetNextPaymentDueDate(string ssn)
        {
            return GetNextPaymentDueDate(ssn, null);
        }

        /// <summary>
        /// Gets the next payment due date from TS26, using the first loan whose sequence is found in the passed-in collection.
        /// </summary>
        protected DateTime GetNextPaymentDueDate(string ssn, IEnumerable<int> loanSequences)
        {
            RI.FastPath(string.Format("TX3Z/ITS26{0}", ssn));
            SelectFirstLoan(loanSequences); //Selects the first loan from out list of sequences

            //We're assuming target screen (TSX29) at this point. Go to the billing information page to get the next pay due date.
            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.GetText(8, 45, 8) == "")
            {
                NextPaymentDueDate payment = new NextPaymentDueDate();
                PaymentDueDate dueDate = new PaymentDueDate(payment);
                MessageBox.Show("Could not determine the next payment due date. Please research the account and hit insert.", "Next Payment Date Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RI.PauseForInsert();
                if (dueDate.ShowDialog() == DialogResult.Cancel)
                {
                    Atd22ByBalance(BrwDemos.Ssn, "AHSOT", "", "", ScriptId, false);
                    _option = LetterOption.SaveLetter;
                    payment.PaymentDueDate = DateTime.Now.ToShortDateString();
                }
                return DateTime.Parse(payment.PaymentDueDate);
            }
            else
                return DateTime.Parse(RI.GetText(8, 45, 8));
        }

        /// <summary>
        /// Selection screen. Select the first loan that's in our list, or the first loan with a balance if the list is null.
        /// </summary>
        private void SelectFirstLoan(IEnumerable<int> loanSequences)
        {
            if (RI.CheckForText(1, 72, "TSX28"))
            {
                bool foundAUsefulLoan = false;
                while (!foundAUsefulLoan && !RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                {
                    for (int row = 8; !RI.CheckForText(row, 3, " "); row++)
                    {
                        if (loanSequences == null)
                        {
                            double balance = double.Parse(RI.GetText(row, 59, 10).Replace(",", ""));
                            foundAUsefulLoan = (balance > 0);
                        }
                        else
                        {
                            int loanSequence = int.Parse(RI.GetText(row, 14, 4));
                            foundAUsefulLoan = (loanSequences.Contains(loanSequence));
                        }
                        if (foundAUsefulLoan)
                        {
                            RI.PutText(21, 12, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter);
                            break;
                        }
                    }
                    if (!foundAUsefulLoan) { RI.Hit(ReflectionInterface.Key.F8); }
                }
            }
        }

    }
}
