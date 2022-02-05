using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACHSETUP
{
    class ChangeBranchAddRemoveProcesor : BaseBranchProcessor
    {
        private DataAccess DA { get; set; }

        public ChangeBranchAddRemoveProcesor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc)
            : base(ri, brwDemos, scriptID, recoveryProc)
        {
            DA = new DataAccess(ri.LogRun);
        }

        public override void Process()
        {
            RI.FastPath($"TX3Z/CTS7O{BrwDemos.Ssn}");
            string selection = "";
            if (RI.ScreenCode != "TSX7K")
            {
                if (RI.ScreenCode ==  "TSX7J")
                {
                    while (RI.ScreenCode != "TSX7K")
                    {
                        string message = "There appears to be multiple active records.  Please select the record you wish to change.  Press insert when done.";
                        Dialog.Def.Ok(message, "Select appropriate record");
                        RI.PauseForInsert();
                    }

                    RI.Hit(Key.F12);
                    selection = RI.GetText(22, 17, 2);
                    RI.Hit(Key.Enter);
                }
                else
                {
                    string message = "There doesn't appear to be any active ACH records.";
                    message += "  Please contact Systems Support if you feel you received this message in error.";
                    Dialog.Def.Ok(message);
                    return; //End Script
                }
            }
            string additionalAmount = RI.GetText(11, 56, 10);

            //Get loan sequence numbers from TS7O.
            GetAchLoanSequenceNumbers(out List<int> availableSequences, out List<int> currentSequences);

            //Get further loan details from TS26 and present them to the user for moving in and out of ACH.
            List<Loan> currentLoans = GatherLoanInfoForList(BrwDemos.Ssn, currentSequences);
            foreach (Loan currentLoan in currentLoans)
                currentLoan.WasOriginallyAch = true;
            List<Loan> availableLoans = GatherLoanInfoForList(BrwDemos.Ssn, availableSequences);
            foreach (Loan availableLoan in availableLoans)
                availableLoan.WasOriginallyAch = false;
            using (ChangeLoansDialog dialog = new ChangeLoansDialog(availableLoans, currentLoans))
                if (dialog.ShowDialog() != DialogResult.OK)
                    return; //End Script

            //Update TS7O.
            List<int> finalList = currentLoans.Select(p => p.Sequence).ToList();
            List<int> additions = new List<int>();
            List<int> deletions = new List<int>();
            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.ChangeOptionAddRemoveTS7OUpdate;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check for recovery
            {
                TS7OAddAndDeleteLoans(BrwDemos.Ssn, finalList, out additions, out deletions, selection);
                RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log
            }
            else
            {
                if (Dialog.Warning.OkCancel("In order to recover, the script needs the loan sequence numbers that were added and deleted.  Please do the needed research to provide the script that information.", "Needed Recovery Information"))
                {
                    InputBox<TextBox> addResults = new InputBox<TextBox>("Additions.  Please provide a comma delimited list of sequence numbers.", "Additions");
                    addResults.ShowDialog();
                    if (addResults.DialogResult != DialogResult.OK)
                        throw new InformationForRecoveryNotProvidedException();
                    InputBox<TextBox> deleteResults = new InputBox<TextBox>("Deletions.  Please provide a comma delimited list of sequence numbers.", "Deletions");
                    deleteResults.ShowDialog();
                    if (deleteResults.DialogResult != DialogResult.OK)
                        throw new InformationForRecoveryNotProvidedException();

                    //put into list
                    additions.AddRange((from a in addResults.Text.Split(',')
                                        select a.ToInt()).ToArray());
                    deletions.AddRange((from a in deleteResults.Text.Split(',')
                                        select a.ToInt()).ToArray());
                }
                else
                    throw new InformationForRecoveryNotProvidedException();
            }

            //Process deletions.
            if (deletions.Any())
            {
                string comment = $"Loan(s) {deletions.FormatForComments()} removed from autopay.";
                TD22CommentData td22Data = new TD22CommentData(comment, deletions, true);
                DenialLetter(BrwDemos, td22Data, "", "Do you want to send a denial letter?", DA);
            }

            //Process additions.
            if (additions.Any())
            {
                string comment = $"Loan(s) {additions.FormatForComments()} added to autopay.";
                TD22CommentData td22Data = new TD22CommentData(comment, true);
                DateTime? nextPaymentDueDate = GetNextPaymentDueDate(BrwDemos.Ssn, additions);
                ApprovalLetter(BrwDemos, td22Data, nextPaymentDueDate.Value.ToString("MM/dd/yy"), additionalAmount, "Do you want to send a approval letter?", false, DA);
            }

            Dialog.Def.Ok("Processing Complete");
            return;
        }

        private List<Loan> GatherLoanInfoForList(string ssn, IEnumerable<int> loanSequences)
        {
            List<Loan> loans = new List<Loan>();
            RI.FastPath($"TX3Z/ITS26{ssn}");
            if (RI.ScreenCode == "TSX28")
            {
                //Selection screen.
                while (RI.MessageCode != "90007")
                {
                    for (int row = 8; !RI.CheckForText(row, 3, " "); row++)
                    {
                        int loanSequence = RI.GetText(row, 14, 4).ToInt();
                        if (loanSequences.Contains(loanSequence))
                        {
                            Loan loan = new Loan
                            {
                                Sequence = loanSequence,
                                Program = RI.GetText(row, 19, 6),
                                Balance = RI.GetText(row, 59, 11).ToDouble(),
                                FirstDisbDate = RI.GetText(row, 5, 8).ToDate()
                            };
                            loans.Add(loan);
                        }
                    }
                    RI.Hit(Key.F8);
                }
            }
            else
            {
                //Target screen.
                int loanSequence = RI.GetText(7, 35, 4).ToInt();
                if (loanSequences.Contains(loanSequence))
                {
                    Loan loan = new Loan
                    {
                        Sequence = loanSequence,
                        Program = RI.GetText(6, 66, 6),
                        Balance = RI.GetText(11, 11, 11).ToDouble(),
                        FirstDisbDate = RI.GetText(6, 18, 8).ToDate()
                    };
                    loans.Add(loan);
                }
            }
            return loans;
        }

        private void GetAchLoanSequenceNumbers(out List<int> availableSequences, out List<int> currentSequences)
        {
            availableSequences = new List<int>();
            currentSequences = new List<int>();
            while (RI.MessageCode != "90007")
            {
                for (int row = 17; RI.CheckForText(row, 3, "_"); row++)
                {
                    int loanSequence = RI.GetText(row, 11, 4).ToInt();
                    if (RI.CheckForText(row, 58, "  "))
                        availableSequences.Add(loanSequence);
                    else
                        currentSequences.Add(loanSequence);
                }
                RI.Hit(Key.F8);
            }
        }

        private void TS7OAddAndDeleteLoans(string ssn, IEnumerable<int> finalList, out List<int> additions, out List<int> deletions, string selection)
        {
            additions = new List<int>();
            deletions = new List<int>();
            RI.FastPath($"TX3Z/CTS7O{ssn}");
            if (selection.Replace(" ", "") != "")
                RI.PutText(22, 17, selection, Key.Enter, true); //This was causing CTS7O to hangman, not sure what it was trying to do
            while (RI.MessageCode != "90007")
            {
                for (int row = 17; RI.CheckForText(row, 3, "_"); row++)
                {
                    //Decide whether an add or delete check should be performed.
                    int loanSequence = RI.GetText(row, 12, 3).ToInt();
                    if (RI.CheckForText(row, 58, "  "))
                    {
                        //Add check.
                        if (finalList.Contains(loanSequence))
                        {
                            RI.PutText(row, 3, "A");
                            additions.Add(loanSequence);
                        }
                    }
                    else
                    {
                        //Delete check.
                        if (!finalList.Contains(loanSequence))
                        {
                            RI.PutText(row, 3, "D");
                            deletions.Add(loanSequence);
                        }
                    }
                }
                RI.Hit(Key.F8);
            }
            RI.Hit(Key.Enter);
            if (RI.MessageCode == "03489")
            {
                RI.PutText(10, 18, "D");
                RI.PutText(10, 57, "B");
                RI.Hit(Key.Enter);
            }
        }
    }
}