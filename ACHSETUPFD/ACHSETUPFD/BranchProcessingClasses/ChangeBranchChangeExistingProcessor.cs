using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
	class ChangeBranchChangeExistingProcessor : BaseBranchProcessor
	{
		private readonly string OPS_CHANGE_LOG;
		private readonly string FullName;

		public ChangeBranchChangeExistingProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, string fullName, RecoveryProcessor recoveryProc, ProcessLogData processLogdata, DataAccess DA, ProcessLogRun logRun)
			: base(ri, brwDemos, scriptID, recoveryProc, processLogdata, DA, logRun)
		{
			OPS_CHANGE_LOG = EnterpriseFileSystem.TempFolder + "ACH Recovery Log OPS Change.txt";
            FullName = fullName;
		}

		public override void Process()
		{
            RI.FastPath(string.Format("TX3Z/CTS7O{0}", BrwDemos.Ssn));
            CheckActiveACH(); //Check if there are any active ACH Records

			//Gather info to present to the user.
			ChangeData changeData = new ChangeData();
            changeData.BorrowerSsn = BrwDemos.Ssn;
            changeData.AccountNumber = BrwDemos.AccountNumber;
			if (!RI.CheckForText(5, 18, "_")) { changeData.RecipientSsn = RI.GetText(5, 18, 11).Replace(" ", ""); }
            if(!RI.CheckForText(7, 47, "_")) { changeData.PersonType = RI.GetText(7, 47, 1);  }
            if (!RI.CheckForText(7, 51, "_")) { changeData.EndorserSsn = RI.GetText(7, 51, 9); }
			changeData.AbaNumber = RI.GetText(6, 18, 16);
			changeData.BankAccountNumber = RI.GetText(6, 52, 25);
			changeData.AccountType = RI.GetText(9, 18, 1);
			if (!RI.CheckForText(11, 57, "_")) { changeData.AdditionalWithdrawalAmount = double.Parse(RI.GetText(11, 57, 10).Replace(",", "")); }

			//Loan sequences and due day don't matter to the user,
			//but we'll need them later, so get them while we're here.
			List<int> loanSequences = new List<int>();
			while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 17; RI.CheckForText(row, 3, "_"); row++)
				{
					if (!RI.CheckForText(row, 58, "  ")) { loanSequences.Add(int.Parse(RI.GetText(row, 11, 4))); }
				}
                RI.Hit(ReflectionInterface.Key.F8);
			}
			string dueDay = RI.GetText(7, 18, 2);

			//Show the user what we found.
			using (ChangeInfoDialog changeDialog = new ChangeInfoDialog(changeData))
			{
				changeDialog.ShowDialog();
				if (changeDialog.DialogResult != DialogResult.OK) { return; }
			}
			string additionalAmountString = (changeData.AdditionalWithdrawalAmount > 0 ? changeData.AdditionalWithdrawalAmount.ToString() : "");

			RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.ChangeOptionChangeExistingDeactivateRecord;
			if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check log for phase
			{
                //Deactivate the ACH record.
                RI.PutText(10, 18, "D", ReflectionInterface.Key.Enter);
                RI.PutText(10, 57, "Z", ReflectionInterface.Key.Enter);

				RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log with new phase
			}

			//New recovery phase
			phase = RecoveryProcessor.RecoveryPhases.ChangeOptionChangeExistingCreateNewRecord;
			if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check log for phase
			{
                //Create a new record with user-provided information.
                RI.FastPath(string.Format("TX3Z/ATS7O{0};{1};{2};{3};{4}", changeData.BorrowerSsn, changeData.RecipientSsn, changeData.AbaNumber, changeData.BankAccountNumber, dueDay));
                RI.PutText(9, 18, changeData.AccountType);
                if(changeData.PersonType == "E")
                {
                    RI.PutText(7, 47, "E");
                    RI.PutText(7, 51, changeData.EndorserSsn);
                }
                else if(changeData.PersonType == "B")
                {
                    RI.PutText(7, 47, "B");
                }
                RI.PutText(11, 57, additionalAmountString);
                RI.PutText(12, 57, "Y"); //form signed
                RI.PutText(13, 57, changeData.GetEftAsString());
				//Mark all loans gathered from the last ACH record.
				while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 17; !RI.CheckForText(row, 12, "  "); row++)
					{
						int loanSequence = int.Parse(RI.GetText(row, 12, 2));
						if (loanSequences.Contains(loanSequence)) { RI.PutText(row, 3, "A"); }
					}
                    RI.Hit(ReflectionInterface.Key.F8);
				}
                RI.Hit(ReflectionInterface.Key.Enter);
                RI.PutText(10, 18, "P", ReflectionInterface.Key.Enter);

				RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log with new phase
			}

			//See whether a check by phone has already been done (indicated by the presence of a file).
			bool checkByPhoneIsDone = false;
			DateTime nextPaymentDueDate = GetNextPaymentDueDate(BrwDemos.Ssn, loanSequences);
			if (!File.Exists(OPS_CHANGE_LOG))
			{
				checkByPhoneIsDone = FindAndCreateCheckByPhone(changeData, nextPaymentDueDate, BrwDemos.DateOfBirth, FullName);
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
                        if (!Atd22ByLoan(BrwDemos.Ssn, ARC, COMMENT, "", loanSequences, ScriptId, false))
						{
							MessageBox.Show(string.Format("You need access to the \"{0}\" ARC.  Please contact Systems Support.", ARC));
                            return;
						}

						RecoveryProcessor.UpdateLogWithNewPhase(innerPhase); //update recovery log with new phase
					}
				}
				else
				{
					TD22CommentData td22Data = new TD22CommentData(COMMENT, false);
					if ((selection.Selected & LetterSelection.Letter.Approved) == LetterSelection.Letter.Approved)
					{
						ApprovalLetter(BrwDemos, td22Data, nextPaymentDueDate.ToString("MM/dd/yy"), additionalAmountString);
					}
					if ((selection.Selected & LetterSelection.Letter.Denied) == LetterSelection.Letter.Denied)
					{
						DenialLetter(BrwDemos, td22Data);
					}
				}
			}
			MessageBox.Show("Change Completed");
		}
        
        /// <summary>
        /// Checks for an active ACH
        /// </summary>
        private void CheckActiveACH()
        {
            if (!RI.CheckForText(1, 72, "TSX7K"))
            {
                if (RI.CheckForText(1, 72, "TSX7J"))
                {
                    while (!RI.CheckForText(1, 72, "TSX7K"))
                    {
                        string message = "There appears to be multiple active records."
                            + "  Please select the record your wish to change.  Click OK when done.";
                        MessageBox.Show(message, "Select appropriate record");
                    }
                }
                else
                {
                    string message = "There doesn't appear to be any active ACH records."
                        + "  Please contact Systems Support if you feel you received this message in error.";
                    MessageBox.Show(message);
                    EndDllScript();
                }
            }
        }

        /// <summary>
        /// Figures out if Check By Phone is available
        /// </summary>
		private bool FindAndCreateCheckByPhone(ChangeData changeData, DateTime nextPaymentDueDate, string dateOfBirth, string fullName)
		{
            RI.FastPath(string.Format("TX3Z/ITS12{0}", changeData.BorrowerSsn));
			if (RI.CheckForText(1, 72, "TSX14"))
			{
				//Selection screen. Search for an active bill that has a matching due date.
				while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 8; !RI.CheckForText(row, 3, " "); row++)
					{
						if (RI.CheckForText(row, 24, "A") && RI.CheckForText(row, 5, nextPaymentDueDate.ToString("MM/dd/yy")))
						{
                            RI.PutText(21, 12, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter);
							return CreateCheckByPhone(changeData, nextPaymentDueDate, dateOfBirth, fullName);
						}
					}
                    RI.Hit(ReflectionInterface.Key.F8);
				}
			}
			else if (RI.CheckForText(6, 54, "A") && RI.CheckForText(10, 12, nextPaymentDueDate.ToString("MM/dd/yy")))
			{
				//Target screen, bill is active, and due date matches.
				return CreateCheckByPhone(changeData, nextPaymentDueDate, dateOfBirth, fullName);
			}
			//No active bill was found with a matching due date.
			return false;
		}

        /// <summary>
        /// Creates the Check By Phone payment
        /// </summary>
		private bool CreateCheckByPhone(ChangeData changeData, DateTime nextPaymentDueDate, string dateOfBirth, string fullName)
		{
			double totalAmountBilled = double.Parse(RI.GetText(13, 26, 9).Replace(",", ""));
			if (totalAmountBilled > 0)
			{
				RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.ChangeOptionChangeExistingCheckByPhone;
				if (!RecoveryProcessor.PhaseAlreadyInLog(phase))
				{
					OPSEntry ops = new OPSEntry();
					ops.SSN = changeData.BorrowerSsn;
					ops.FullName = fullName;
					ops.DOB = dateOfBirth;
					ops.RoutingNumber = changeData.AbaNumber;
					ops.BankAccountNumber = changeData.BankAccountNumber;
					ops.AcctType = (changeData.AccountType == "C" ? ACHRecord.BankAccountType.Checking : ACHRecord.BankAccountType.Savings);
					double paymentAmount = double.Parse(RI.GetText(10, 42, 14).Replace(",", "")); //Current amount due
					paymentAmount += changeData.AdditionalWithdrawalAmount;
					ops.PaymentAmount = paymentAmount.ToString("#####0.00");
					DayOfWeek[] weekend = { DayOfWeek.Saturday, DayOfWeek.Sunday };
					DateTime effectiveDate = nextPaymentDueDate;
					while (weekend.Contains(effectiveDate.DayOfWeek)) { effectiveDate = effectiveDate.AddDays(1); }
					ops.EffectiveDate = effectiveDate.ToString("MM/dd/yyyy");
                    ops.AccountNumber = changeData.AccountNumber;
					ops.AccountHolderName = fullName;
                    
					DA.AddEntryToDB(ops);

					RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log
				}
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

