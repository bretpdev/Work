using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.DataAccess.ArcData;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACHSETUP
{
    class ChangeBranchChangeExistingProcessor : BaseBranchProcessor
    {
        private DataAccess DA { get; set; }
        private string OPS_CHANGE_LOG { get; set; }
        private string FullName { get; set; }

        public ChangeBranchChangeExistingProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, string fullName, RecoveryProcessor recoveryProc)
            : base(ri, brwDemos, scriptID, recoveryProc)
        {
            DA = new DataAccess(ri.LogRun);
            OPS_CHANGE_LOG = EnterpriseFileSystem.TempFolder + "ACH Recovery Log OPS Change.txt";
            FullName = fullName;
        }

        public override void Process()
        {
            RI.FastPath($"TX3Z/CTS7O{BrwDemos.Ssn}");
            if (RI.ScreenCode != "TSX7K")
            {
                if (RI.ScreenCode == "TSX7J")
                {
                    while (RI.ScreenCode != "TSX7K")
                    {
                        string message = "There appears to be multiple active records.";
                        message += "  Please select the record you wish to change.  Press insert when done.";
                        Dialog.Def.Ok(message, "Select appropriate record");
                        RI.PauseForInsert();
                    }
                }
                else
                {
                    string message = "There doesn't appear to be any active ACH records.";
                    message += "  Please contact Systems Support if you feel you received this message in error.";
                    Dialog.Def.Ok(message);
                    return;
                }
            }

            //Gather info to present to the user.
            ChangeData changeData = new ChangeData();
            changeData.BorrowerSsn = BrwDemos.Ssn;
            if (!RI.CheckForText(5, 18, "_"))
                changeData.RecipientSsn = RI.GetText(5, 18, 11).Replace(" ", "");
            changeData.AbaNumber = RI.GetText(6, 18, 16);
            changeData.AccountNumber = RI.GetText(6, 52, 25);
            changeData.AccountType = RI.GetText(9, 18, 1);
            if (!RI.CheckForText(11, 57, "_"))
                changeData.AdditionalWithdrawalAmount = RI.GetText(11, 57, 10).Replace(",", "").ToDouble();

            //Loan sequences and due day don't matter to the user,
            //but we'll need them later, so get them while we're here.
            List<int> loanSequences = new List<int>();
            while (RI.MessageCode != "90007")
            {
                for (int row = 17; RI.CheckForText(row, 3, "_"); row++)
                    if (!RI.CheckForText(row, 58, "  "))
                        loanSequences.Add(RI.GetText(row, 11, 4).ToInt());
                RI.Hit(Key.F8);
            }
            string dueDay = RI.GetText(7, 18, 2);

            //Show the user what we found.
            using (ChangeInfoDialog changeDialog = new ChangeInfoDialog(changeData))
            {
                changeDialog.ShowDialog();
                if (changeDialog.DialogResult != DialogResult.OK)
                    return;
            }
            string additionalAmountString = (changeData.AdditionalWithdrawalAmount > 0 ? changeData.AdditionalWithdrawalAmount.ToString() : "");

            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.ChangeOptionChangeExistingDeactivateRecord;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check log for phase
            {
                //Deactivate the ACH record.
                RI.PutText(10, 18, "D", Key.Enter);
                RI.PutText(10, 57, "Z", Key.Enter);

                RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log with new phase
            }


            //New recovery phase
            phase = RecoveryProcessor.RecoveryPhases.ChangeOptionChangeExistingCreateNewRecord;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check log for phase
            {
                //Create a new record with user-provided information.
                RI.FastPath($"TX3Z/ATS7O{changeData.BorrowerSsn};{changeData.RecipientSsn};{changeData.AbaNumber};{changeData.AccountNumber};{dueDay}");
                RI.PutText(9, 18, changeData.AccountType);
                RI.PutText(11, 57, additionalAmountString);
                RI.PutText(12, 57, "Y"); //form signed
                RI.PutText(13, 57, "PPD");

                //Mark all loans gathered from the last ACH record.
                while (RI.MessageCode != "90007")
                {
                    for (int row = 17; !RI.CheckForText(row, 12, "  "); row++)
                    {
                        int loanSequence = RI.GetText(row, 12, 2).ToInt();
                        if (loanSequences.Contains(loanSequence))
                            RI.PutText(row, 3, "A");
                    }
                    RI.Hit(Key.F8);
                }
                RI.Hit(Key.Enter);
                RI.PutText(10, 18, "P", Key.Enter);

                RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log with new phase
            }

            //See whether a check by phone has already been done (indicated by the presence of a file).
            DateTime? nextPaymentDueDate = GetNextPaymentDueDate(BrwDemos.Ssn, loanSequences);
            if (!nextPaymentDueDate.HasValue)
            {
                Dialog.Error.Ok("A next payment due date could not be determined which is needed to change the ACH. Canceling script.", "Script Canceled");
                return; //End Script
            }
            if (!File.Exists(OPS_CHANGE_LOG))
            {
                FindAndCreateCheckByPhone(changeData, nextPaymentDueDate.Value, BrwDemos.DateOfBirth, FullName);
                File.Create(OPS_CHANGE_LOG);
            }

            LetterSelection selection = new LetterSelection();
            using (LetterMenu menu = new LetterMenu(selection))
            {
                const string COMMENT = "Autopay record changed.";
                menu.ShowDialog();
                if (selection.Selected == LetterSelection.Letter.None)
                {
                    RecoveryProcessor.RecoveryPhases innerPhase = RecoveryProcessor.RecoveryPhases.ChangeOptionChangeExistingCommentAdd;
                    if (!RecoveryProcessor.PhaseAlreadyInLog(innerPhase)) //check log for phase
                    {
                        AddComment(BrwDemos.Ssn, COMMENT, ArcType.Atd22ByLoan, loanSequences);
                        RecoveryProcessor.UpdateLogWithNewPhase(innerPhase); //update recovery log with new phase
                    }
                }
                else
                {
                    TD22CommentData td22Data = new TD22CommentData(COMMENT, false);
                    if ((selection.Selected & LetterSelection.Letter.Approved) == LetterSelection.Letter.Approved)
                        ApprovalLetter(BrwDemos, td22Data, nextPaymentDueDate.Value.ToString("MM/dd/yy"), additionalAmountString, DA);
                    if ((selection.Selected & LetterSelection.Letter.Denied) == LetterSelection.Letter.Denied)
                        DenialLetter(BrwDemos, td22Data, DA);
                }
            }
            Dialog.Def.Ok("Change Completed");
            return;
        }

        private bool AddComment(string ssn, string comment, ArcType type, List<int> loanSequences)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = ssn,
                Arc = ARC,
                ArcTypeSelected = type,
                Comment = comment,
                ScriptId = ScriptId
            };
            if (type == ArcType.Atd22ByLoan)
                arc.LoanSequences = loanSequences;
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding ARC: {ARC} to borrower account: {ssn}.";
                Dialog.Error.Ok(message);
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            return result.ArcAdded;
        }

        private void FindAndCreateCheckByPhone(ChangeData changeData, DateTime nextPaymentDueDate, string dateOfBirth, string fullName)
        {
            RI.FastPath($"TX3Z/ITS12{changeData.BorrowerSsn}");
            if (RI.ScreenCode == "TSX14")
            {
                //Selection screen. Search for an active bill that has a matching due date.
                while (RI.MessageCode != "90007")
                {
                    for (int row = 8; !RI.CheckForText(row, 3, " "); row++)
                    {
                        if (RI.CheckForText(row, 24, "A") && RI.CheckForText(row, 5, nextPaymentDueDate.ToString("MM/dd/yy")))
                        {
                            RI.PutText(21, 12, RI.GetText(row, 2, 2), Key.Enter);
                            CreateCheckByPhone(changeData, nextPaymentDueDate, dateOfBirth, fullName);
                        }
                    }
                    RI.Hit(Key.F8);
                }
            }
            else if (RI.CheckForText(6, 54, "A") && RI.CheckForText(10, 12, nextPaymentDueDate.ToString("MM/dd/yy")))
                CreateCheckByPhone(changeData, nextPaymentDueDate, dateOfBirth, fullName);//Target screen, bill is active, and due date matches.
        }

        private void CreateCheckByPhone(ChangeData changeData, DateTime nextPaymentDueDate, string dateOfBirth, string fullName)
        {
            double totalAmountBilled = RI.GetText(13, 26, 9).Replace(",", "").ToDouble();
            if (totalAmountBilled > 0)
            {
                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.ChangeOptionChangeExistingCheckByPhone;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase))
                {
                    OPSEntry ops = new OPSEntry
                    {
                        SSN = changeData.BorrowerSsn,
                        FullName = fullName,
                        DOB = dateOfBirth,
                        RoutingNumber = changeData.AbaNumber,
                        BankAccountNumber = changeData.AccountNumber,
                        AcctType = (changeData.AccountType == "C" ? ACHRecord.BankAccountType.Checking : ACHRecord.BankAccountType.Savings)
                    };
                    double paymentAmount = RI.GetText(10, 42, 14).Replace(",", "").ToDouble(); //Current amount due
                    paymentAmount += changeData.AdditionalWithdrawalAmount;
                    ops.PaymentAmount = paymentAmount.ToString("#####0.00");
                    DayOfWeek[] weekend = {DayOfWeek.Saturday, DayOfWeek.Sunday };
                    DateTime effectiveDate = nextPaymentDueDate;
                    while (weekend.Contains(effectiveDate.DayOfWeek)) 
                        effectiveDate = effectiveDate.AddDays(1);
                    ops.EffectiveDate = effectiveDate.ToString("MM/dd/yyyy");
                    ops.AccountHolderName = fullName;
                    DA.AddEntryToDB(ops);

                    RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log
                }
            }
        }
    }
}