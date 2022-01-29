using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
    class SuspendBranchProcessor : BaseBranchProcessor
    {
        public SuspendBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc, ProcessLogData processLogdata, DataAccess DA, ProcessLogRun logRun)
            : base(ri, brwDemos, scriptID, recoveryProc, processLogdata, DA, logRun)
        {
        }

        public override void Process()
        {
            SuspendOptions suspendOptions = new SuspendOptions();
            using (SuspendMenu menu = new SuspendMenu(suspendOptions))
            {
                if (menu.ShowDialog() != DialogResult.OK) { return; }
            }

            //The spec has us calculating the next payment due date from TS7O, but then we end up always using the next payment due date from TS26
            //(which was gathered in the Add branch, and not guaranteed to be available to the Suspend branch in the VBA code).
            //Rather than go through that run-around, we're just going to get the date from TS26 ourselves.
            DateTime nextPaymentDueDate = GetNextPaymentDueDate(BrwDemos.Ssn);

            //Check that the due date is more than 3 days out from the current date.
            if (nextPaymentDueDate <= DateTime.Now.Date.AddDays(3))
            {
                MessageBox.Show("This request cannot be processed.  There is too little time to stop the payment for the next bill.");
                return;
            }

            List<int> loanSequences = null;
            DateTime suspendDate = new DateTime(1900, 1, 1);

            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.SuspendOptionSuspendRecord;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase is already in log file
            {
                //Now continue on to TS7O to suspend active loans.
                RI.FastPath(string.Format("TX3Z/CTS7O{0}", BrwDemos.Ssn));
                RI.Hit(ReflectionInterface.Key.F10);
                string message = "The borrower does not have a denied or approved direct debit record to be suspended." 
                    + " If there is a record on TS7O then it hasn't been approved. Please wait a day for the record to be approved.";
                if (RI.CheckForText(23, 2, "03528 NO DENIED/APPROVED DIRECT DEBIT FOUND FOR ENTERED ReflectionInterface.Key"))
                {
                    MessageBox.Show(message);
                    return;
                }

                //Suspend all active loans and get their sequence numbers.
                suspendDate = nextPaymentDueDate.AddMonths(suspendOptions.NumberOfBills - 1).AddDays(1);
                loanSequences = SuspendLoansAndRetrieveSequences(suspendOptions.SelectedRequestor, suspendDate);
                if (loanSequences.Count == 0)
                {
                    MessageBox.Show(message);
                    return;
                }

                RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log
            }

            //New recovery phase
            phase = RecoveryProcessor.RecoveryPhases.SuspendOptionCommentAdd;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase))
            {
                //Leave an activity comment.
                string sequenceString;
                //if in recovery and the script bombed between this phase and the last phase then the loan sequences and the suspend date will be null and the user will need to provide them again.
                //loan sequence numbers
                if (loanSequences == null || loanSequences.Count == 0)
                {
                    sequenceString = Microsoft.VisualBasic.Interaction.InputBox("Please provide the loan sequence numbers included in the suspended ACH in a comma delimited format.", "Loan Sequences Needed");
                    if (sequenceString.IsNullOrEmpty())
                    {
                        MessageBox.Show("Please do the needed research, figure out what sequence numbers are included and start the script again.", "Sequence Numbers Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new InformationForRecoveryNotProvidedException();
                    }
                }
                else
                {
                    sequenceString = loanSequences.FormatForComments();
                }

                //suspend date
                if (suspendDate == new DateTime(1900, 1, 1))
                {
                    string results = Microsoft.VisualBasic.Interaction.InputBox("Please provide the suspend date.", "Suspend Date Needed");
                    if (results.IsNullOrEmpty())
                    {
                        suspendDate = DateTime.Parse(results);
                    }
                    else
                    {
                        MessageBox.Show("Please do the needed research, figure out what the suspend date is and start the script again.", "Suspend Date Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new InformationForRecoveryNotProvidedException();
                    }
                }

                string td22Comment = string.Format("Loan(s) {0} suspended until {1:MM/dd/yyyy}.", sequenceString, suspendDate);
                if (!Atd22ByLoan(BrwDemos.Ssn, ARC, td22Comment, "", loanSequences, ScriptId, true))
                {
                    MessageBox.Show(string.Format("You need access to the \"{0}\" ARC.  Please contact Systems Support.", ARC));
                    return;
                }

                RecoveryProcessor.UpdateLogWithNewPhase(phase);
            }
            MessageBox.Show("Suspension Completed");
            return;
        }

        /// <summary>
        /// Suspends all loans for the active loan sequences
        /// </summary>
        private List<int> SuspendLoansAndRetrieveSequences(SuspendOptions.Requestor requestor, DateTime suspendDate)
        {
            string suspendReason = (requestor == SuspendOptions.Requestor.Borrower ? "B" : "A");
            List<int> activeLoanSequences = new List<int>();

            //Switch to change mode and see whether we're on a target screen or selection screen.
            RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
            if (RI.CheckForText(1, 72, "TSX7M"))
            {
                //Selection screen.
                while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                {
                    for (int row = 10; !RI.CheckForText(row, 3, " "); row++)
                    {
                        //Suspend this loan if it's active.
                        if (RI.CheckForText(row, 73, "A") && RI.CheckForText(row, 39, "    "))
                        {
                            RI.PutText(21, 18, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                            activeLoanSequences.Add(SuspendLoanAndRetrieveSequence(suspendReason, suspendDate));
                            RI.Hit(ReflectionInterface.Key.F12);
                        }
                    }//for
                    RI.Hit(ReflectionInterface.Key.F8);
                }//while
            }
            else
            {
                //Target screen. Suspend the loan if it's active.
                if (RI.CheckForText(9, 43, "A") && RI.CheckForText(15, 25, "_"))
                {
                    activeLoanSequences.Add(SuspendLoanAndRetrieveSequence(suspendReason, suspendDate));
                }
            }

            return activeLoanSequences;
        }

        /// <summary>
        /// Suspends loan for a single loan sequence.
        /// </summary>
        private int SuspendLoanAndRetrieveSequence(string suspendReason, DateTime suspendDate)
        {
            int loanSequence = int.Parse(RI.GetText(7, 21, 4));
            RI.PutText(15, 25, DateTime.Now.ToString("MMddyy")); //End date.
            RI.PutText(17, 25, suspendReason);
            RI.PutText(19, 20, suspendDate.ToString("MMddyy"));
            RI.Hit(ReflectionInterface.Key.Enter);
            return loanSequence;
        }
    }
}
