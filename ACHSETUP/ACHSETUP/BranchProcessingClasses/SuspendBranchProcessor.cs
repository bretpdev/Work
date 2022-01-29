using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACHSETUP
{
    class SuspendBranchProcessor : BaseBranchProcessor
    {
        public SuspendBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc)
            : base(ri, brwDemos, scriptID, recoveryProc)
        {
        }

        public override void Process()
        {
            SuspendOptions suspendOptions = new SuspendOptions();
            using SuspendMenu menu = new SuspendMenu(suspendOptions);
            if (menu.ShowDialog() != DialogResult.OK)
                return; //End Script

            //The spec has us calculating the next payment due date from TS7O, but then we end up always using the next payment due date from TS26
            //(which was gathered in the Add branch, and not guaranteed to be available to the Suspend branch in the VBA code).
            //Rather than go through that run-around, we're just going to get the date from TS26 ourselves.
            DateTime? nextPaymentDueDate = GetNextPaymentDueDate(BrwDemos.Ssn);
            if (!nextPaymentDueDate.HasValue)
            {
                Dialog.Error.Ok("A next payment due date could not be determined which is needed to suspend the ACH. Canceling script.", "Script Canceled");
                return; //End Script
            }

            //Check that the due date is more than 3 days out from the current date.
            if (nextPaymentDueDate <= DateTime.Now.Date.AddDays(3))
            {
                Dialog.Info.Ok("This request cannot be processed.  There is too little time to stop the payment for the next bill.");
                return; //End Script
            }

            List<int> loanSequences = null;
            DateTime suspendDate = new DateTime(1900, 1, 1);

            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.SuspendOptionSuspendRecord;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase is already in log file
            {
                //Now continue on to TS7O to suspend active loans.
                RI.FastPath($"TX3Z/CTS7O{BrwDemos.Ssn}");
                RI.Hit(Key.F10);
                string message = "The borrower doesn't have a denied or approved direct debit record to be suspended.";
                message += "  If there's a record on TS7O then it hasn't been approved.";
                message += "  Please wait a day for the record to be approved.";
                if (RI.MessageCode == "03528")
                {
                    Dialog.Info.Ok(message);
                    return; //End Script
                }

                //Suspend all active loans and get their sequence numbers.
                suspendDate = nextPaymentDueDate.Value.AddMonths(suspendOptions.NumberOfBills - 1).AddDays(1);
                loanSequences = SuspendLoansAndRetrieveSequences(suspendOptions.SelectedRequestor, suspendDate);
                if (loanSequences.Count == 0)
                {
                    Dialog.Info.Ok(message);
                    return; //End Script
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
                    InputBox<TextBox> results = new InputBox<TextBox>("Please provide the loan sequence numbers included in the suspended ACH in a comma delimited format.", "Loan Sequences Needed");
                    if (results.DialogResult == DialogResult.OK)
                        sequenceString = results.Text;
                    else
                    {
                        Dialog.Error.Ok("Please do the needed research, figure out what sequence numbers are included and start the script again.", "Sequence Numbers Needed");
                        throw new InformationForRecoveryNotProvidedException();
                    }
                }
                else
                    sequenceString = loanSequences.FormatForComments();

                //suspend date
                if (suspendDate == new DateTime(1900, 1, 1))
                {
                    InputBox<TextBox> results = new InputBox<TextBox>("Please provide the suspend date.", "Suspend Date Needed");
                    if (results.DialogResult == DialogResult.OK)
                        suspendDate = results.Text.ToDate();
                    else
                    {
                        Dialog.Error.Ok("Please do the needed research, figure out what the suspend date is and start the script again.", "Suspend Date Needed");
                        throw new InformationForRecoveryNotProvidedException();
                    }
                }

                string td22Comment = $"Loan(s) {sequenceString} suspended until {suspendDate:MM/dd/yyyy}.";
                ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    AccountNumber = BrwDemos.Ssn,
                    Arc = ARC,
                    ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                    Comment = td22Comment,
                    LoanSequences = loanSequences,
                    ScriptId = ScriptId
                };
                ArcAddResults result = arc.AddArc();
                if (!result.ArcAdded)
                {
                    string message = $"There was an error adding ARC: {ARC} to account {BrwDemos.AccountNumber}.";
                    Dialog.Error.Ok(message);
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return; //End Script
                }

                RecoveryProcessor.UpdateLogWithNewPhase(phase);
            }
            Dialog.Info.Ok("Suspension Completed");
            return;
        }

        private List<int> SuspendLoansAndRetrieveSequences(SuspendOptions.Requestor requestor, DateTime suspendDate)
        {
            string suspendReason = (requestor == SuspendOptions.Requestor.Borrower ? "B" : "A");
            List<int> activeLoanSequences = new List<int>();

            //Switch to change mode and see whether we're on a target screen or selection screen.
            RI.PutText(1, 4, "C", Key.Enter);
            if (RI.ScreenCode == "TSX7M")
            {
                //Selection screen.
                while (RI.MessageCode != "90007")
                {
                    for (int row = 10; !RI.CheckForText(row, 3, " "); row++)
                    {
                        //Suspend this loan if it's active.
                        if (RI.CheckForText(row, 73, "A") && RI.CheckForText(row, 39, "    "))
                        {
                            RI.PutText(21, 18, RI.GetText(row, 2, 3), Key.Enter, true);
                            activeLoanSequences.Add(SuspendLoanAndRetrieveSequence(suspendReason, suspendDate));
                            RI.Hit(Key.F12);
                        }
                    }
                    RI.Hit(Key.F8);
                }
            }
            else
            {
                //Target screen. Suspend the loan if it's active.
                if (RI.CheckForText(9, 43, "A") && RI.CheckForText(15, 25, "_"))
                    activeLoanSequences.Add(SuspendLoanAndRetrieveSequence(suspendReason, suspendDate));
            }

            return activeLoanSequences;
        }

        private int SuspendLoanAndRetrieveSequence(string suspendReason, DateTime suspendDate)
        {
            int loanSequence = RI.GetText(7, 21, 4).ToInt();
            RI.PutText(15, 25, DateTime.Now.ToString("MMddyy")); //End date.
            RI.PutText(17, 25, suspendReason);
            RI.PutText(19, 20, suspendDate.ToString("MMddyy"));
            RI.Hit(Key.Enter);
            return loanSequence;
        }
    }
}