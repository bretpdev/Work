using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
	class RemoveBranchProcessor : BaseBranchProcessor
	{
        public RemoveBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc, ProcessLogData processLogData, DataAccess DA, ProcessLogRun logRun)
			: base(ri, brwDemos, scriptID, recoveryProc, processLogData, DA, logRun)
		{
		}

		public override void Process()
		{
            List<int> loanSequences = null;

            //Get denial reasons from the user.
            List<string> denialReasons = new List<string>();
            using (DenialReasonDialog denialOptionUI = new DenialReasonDialog(ref denialReasons))
            {
                denialOptionUI.ShowDialog();
            }
            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.RemoveOptionDeactivateRecord;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
            {
                RI.FastPath(string.Format("TX3Z/CTS7O{0}", BrwDemos.Ssn));
                while (!RI.CheckForText(1, 72, "TSX7K"))
                {
                    if (RI.CheckForText(23, 2, "03512 NO PRE-AUTHORIZED DEBIT INFORMATION FOUND"))
                    {
                        string message = "There is no Autopay on this account.  No action taken.";
                        MessageBox.Show(message, "No Autopay on account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        string message = "There is more than one active record. Please select the record your wish to change. Click OK when done or Cancel to end the script.";
                        if ((MessageBox.Show(message, "Select appropriate record", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop)) == DialogResult.Cancel) { return; }
                    }
                }

                //Note all loans that will be affected, and remove autopay.
                loanSequences = GetAchLoans();
                RI.PutText(10, 18, "D", ReflectionInterface.Key.Enter);
                string reasonCode = GetReasonCode(denialReasons[0]);
                RI.PutText(10, 57, reasonCode, ReflectionInterface.Key.Enter);

                RecoveryProcessor.UpdateLogWithNewPhase(phase);
            }

			//Prompt for a denial letter and leave an activity record.
			string sequenceString;

            //if in recovery and the script bombed between this phase and the last phase then the loan sequences will be null and the user will need to provide them again.
            //loan sequence numbers
            if (loanSequences == null)
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

			string comment = string.Format("Loan(s) {0} removed from Autopay.", sequenceString);
			TD22CommentData td22Data = new TD22CommentData(comment, true);
			DenialLetter(BrwDemos, td22Data, denialReasons, "Do you want to send a denial letter?");
			MessageBox.Show("Removal Complete");
            return;
		}

        /// <summary>
        /// Gets a list of ACH loans
        /// </summary>
		private List<int> GetAchLoans()
		{
			List<int> loanSequences = new List<int>();
			while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 17; RI.CheckForText(row, 3, "_"); row++)
				{
					//Add the loan if it is connected to ACH
					if (!RI.CheckForText(row, 58, "  "))
                    {
                        loanSequences.Add(int.Parse(RI.GetText(row, 12, 3)));
                    }
				}
                RI.Hit(ReflectionInterface.Key.F8);
			}//while
			return loanSequences;
		}

        /// <summary>
        /// Gets the Reason code for each reason type
        /// </summary>
		private string GetReasonCode(string reasonText)
		{
			string reasonCode = "";
			switch (reasonText)
			{
				case "You have no loans eligible for Autopay":
					reasonCode = "A";
					break;
				case "You requested cancellation of Autopay":
					reasonCode = "B";
					break;
				case "Your bank account was reported as closed":
					reasonCode = "C";
					break;
				case "Missing account number":
					reasonCode = "D";
					break;
				case "Missing ABA number":
					reasonCode = "E";
					break;
				case "Your bank account number is invalid":
					reasonCode = "G";
					break;
				case "Your Autopay form is incomplete":
					reasonCode = "I";
					break;
				case "Your account is not in repayment":
					reasonCode = "J";
					break;
				//case "Missing voided check":
				//    reasonCode = "L";
				//    break;
				case "You've had multiple insufficient funds":
					reasonCode = "N";
					break;
				case "The ABA number you provided is invalid":
					reasonCode = "O";
					break;
				case "Your Autopay request was not signed":
					reasonCode = "S";
					break;
				case "Banking information was changed":
					reasonCode = "Z";
					break;
			}//switch
			return reasonCode;
		}
	}
}
