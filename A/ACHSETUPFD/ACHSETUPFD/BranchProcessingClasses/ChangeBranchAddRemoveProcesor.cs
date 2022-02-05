using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
	class ChangeBranchAddRemoveProcesor : BaseBranchProcessor
	{
		public ChangeBranchAddRemoveProcesor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc, ProcessLogData processLogData, DataAccess DA, ProcessLogRun logRun)
			: base(ri, brwDemos, scriptID, recoveryProc, processLogData, DA, logRun)
		{

		}

		public override void Process()
		{
            RI.FastPath(string.Format("TX3Z/CTS7O{0}", BrwDemos.Ssn));
			if (!RI.CheckForText(1, 72, "TSX7K"))
			{
				if (RI.CheckForText(1, 72, "TSX7J"))
				{
					while (!RI.CheckForText(1, 72, "TSX7K"))
					{
						string message = "There appears to be multiple active records. Please manually navigate to the record you wish to change and then click OK when done.";
						MessageBox.Show(message, "Select Appropriate Record");
					}
				}
				else
				{
					string message = "There does not appear to be any active ACH records."
                         + "  Please contact Systems Support if you feel you received this message in error.";
					MessageBox.Show(message);
                    return;
				}
			}
			string additionalAmount = RI.GetText(11, 56, 10);

			//Get loan sequence numbers from TS7O.
			List<int> availableSequences;
			List<int> currentSequences;
			GetAchLoanSequenceNumbers(out availableSequences, out currentSequences);

			//Get further loan details from TS26 and present them to the user for moving in and out of ACH.
			List<Loan> currentLoans = GatherLoanInfoForList(BrwDemos.Ssn, currentSequences);
			foreach (Loan currentLoan in currentLoans)
			{
				currentLoan.WasOriginallyAch = true;
			}
			List<Loan> availableLoans = GatherLoanInfoForList(BrwDemos.Ssn, availableSequences);
			foreach (Loan availableLoan in availableLoans) { availableLoan.WasOriginallyAch = false; }
			using (ChangeLoansDialog dialog = new ChangeLoansDialog(availableLoans, currentLoans))
			{
				if (dialog.ShowDialog() != DialogResult.OK) { return; }
			}

			//Update TS7O.
			List<int> finalList = currentLoans.Select(p => p.Sequence).ToList();
			List<int> additions = new List<int>();
			List<int> deletions = new List<int>();
			RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.ChangeOptionAddRemoveTS7OUpdate;
			if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check for recovery
			{
				TS7OAddAndDeleteLoans(BrwDemos.Ssn, finalList, out additions, out deletions);
				RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery log
			}
			else
			{
				if (MessageBox.Show("In order to recover, the script needs the loan sequence numbers that were added and deleted.  Please do the needed research to provide the script that information.", "Needed Recovery Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					string addResults = Microsoft.VisualBasic.Interaction.InputBox("Additions.  Please provide a comma delimited list of sequence numbers.", "Additions");
					if (addResults.IsNullOrEmpty())
					{
						throw new InformationForRecoveryNotProvidedException();
					}
					string deleteResults = Microsoft.VisualBasic.Interaction.InputBox("Deletions.  Please provide a comma delimited list of sequence numbers.", "Deletions");
					if (deleteResults.IsNullOrEmpty())
					{
						throw new InformationForRecoveryNotProvidedException();
					}

					//put into list
					additions.AddRange((from a in addResults.Split(',')
										select int.Parse(a)).ToArray<int>());
					deletions.AddRange((from a in deleteResults.Split(',')
										select int.Parse(a)).ToArray<int>());
				}
				else
				{
					throw new InformationForRecoveryNotProvidedException();
				}
			}
			//Process deletions.
			if (deletions.Count > 0)
			{
				string comment = string.Format("Loan(s) {0} removed from autopay.", deletions.FormatForComments());
				TD22CommentData td22Data = new TD22CommentData(comment, deletions, true);
				DenialLetter(BrwDemos, td22Data, "", "Do you want to send a denial letter?");
			}

			//Process additions.
			if (additions.Count > 0)
			{
				string comment = string.Format("Loan(s) {0} added to autopay.", additions.FormatForComments());
				TD22CommentData td22Data = new TD22CommentData(comment, true);
				DateTime nextPaymentDueDate = GetNextPaymentDueDate(BrwDemos.Ssn, additions);
				ApprovalLetter(BrwDemos, td22Data, nextPaymentDueDate.ToString("MM/dd/yy"), additionalAmount, "Do you want to send a approval letter?", false);
			}

			MessageBox.Show("Processing Complete");
		}//Process()

        /// <summary>
        /// Gathers loan informtion
        /// </summary>
		private List<Loan> GatherLoanInfoForList(string ssn, IEnumerable<int> loanSequences)
		{
			List<Loan> loans = new List<Loan>();
            RI.FastPath(string.Format("TX3Z/ITS26{0}", ssn));
			if (RI.CheckForText(1, 72, "TSX28"))
			{
				//Selection screen.
				while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 8; !RI.CheckForText(row, 3, " "); row++)
					{
						int loanSequence = int.Parse(RI.GetText(row, 14, 4));
						if (loanSequences.Contains(loanSequence))
						{
							Loan loan = new Loan();
							loan.Sequence = loanSequence;
							loan.Program = RI.GetText(row, 19, 6);
							loan.Balance = double.Parse(RI.GetText(row, 59, 11));
							loan.FirstDisbDate = DateTime.Parse(RI.GetText(row, 5, 8));
							loans.Add(loan);
						}
					}//for
                    RI.Hit(ReflectionInterface.Key.F8);
				}//while
			}
			else
			{
				//Target screen.
				int loanSequence = int.Parse(RI.GetText(7, 35, 4));
				if (loanSequences.Contains(loanSequence))
				{
					Loan loan = new Loan();
					loan.Sequence = loanSequence;
					loan.Program = RI.GetText(6, 66, 6);
					loan.Balance = double.Parse(RI.GetText(11, 11, 11));
					loan.FirstDisbDate = DateTime.Parse(RI.GetText(6, 18, 8));
					loans.Add(loan);
				}
			}
			return loans;
		}

        /// <summary>
        /// Gets the loan sequences numbers that are part of the ACH
        /// </summary>
		private void GetAchLoanSequenceNumbers(out List<int> availableSequences, out List<int> currentSequences)
		{
			availableSequences = new List<int>();
			currentSequences = new List<int>();
			while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 17; RI.CheckForText(row, 3, "_"); row++)
				{
					int loanSequence = int.Parse(RI.GetText(row, 11, 4));
					if (RI.CheckForText(row, 58, "  "))
					{
						availableSequences.Add(loanSequence);
					}
					else
					{
						currentSequences.Add(loanSequence);
					}
				}//for
                RI.Hit(ReflectionInterface.Key.F8);
			}//while
		}

        /// <summary>
        /// Adds and removes loans to the TS70
        /// </summary>
		private void TS7OAddAndDeleteLoans(string ssn, IEnumerable<int> finalList, out List<int> additions, out List<int> deletions)
		{
			additions = new List<int>();
			deletions = new List<int>();
            RI.FastPath(string.Format("TX3Z/CTS7O{0}", ssn));
			while (!RI.CheckForText(1, 72, "TSX7K"))
			{
				string message = "There appears to be multiple active records."
				    + "  Please select the record your wish to change.  Click OK when done.";
				MessageBox.Show(message, "Select appropriate record");
			}
			while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 17; RI.CheckForText(row, 3, "_"); row++)
				{
					//Decide whether an add or delete check should be performed.
					int loanSequence = int.Parse(RI.GetText(row, 12, 3));
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
				}//for
                RI.Hit(ReflectionInterface.Key.F8);
			}//while
            RI.Hit(ReflectionInterface.Key.Enter);
			if (RI.CheckForText(23, 2, "03489 LOANS MUST BE ATTACHED TO THIS RQST; ENTER THE DENIAL REASON CODE"))
			{
                RI.PutText(10, 18, "D");
                RI.PutText(10, 57, "B");
                RI.Hit(ReflectionInterface.Key.Enter);
			}
		}
	}
}
