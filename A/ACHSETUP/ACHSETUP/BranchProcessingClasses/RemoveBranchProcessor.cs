using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACHSETUP
{
    class RemoveBranchProcessor : BaseBranchProcessor
	{
        private DataAccess DA { get; set; }

        public RemoveBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc)
			: base(ri, brwDemos, scriptID, recoveryProc)
		{
            DA = new DataAccess(ri.LogRun);
        }

		public override void Process()
		{
            List<int> loanSequences = null;

            //Get denial reasons from the user.
            List<string> denialReasons = new List<string>();
			using DenialReasonDialog denialOptionUI = new DenialReasonDialog(ref denialReasons);
                denialOptionUI.ShowDialog();
            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.RemoveOptionDeactivateRecord;
            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check if phase should be processed
            {
			    RI.FastPath($"TX3Z/CTS7O{BrwDemos.Ssn}");
			    while (RI.ScreenCode != "TSX7K")
			    {
                    if (RI.MessageCode == "03512")
                    {
                        string message = "There doesn't appear to be Autopay on this account.  No action taken.";
                        MessageBox.Show(message, "No Autopay on account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; //End Script
                    }
                    else
                    {
                        string message = "There appears to be multiple active records.  Please select the record you wish to change.  Press insert when done or Cancel to end the script.";
                        if ((MessageBox.Show(message, "Select appropriate record", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop)) == DialogResult.Cancel)
                            return; //End Script
                        RI.PauseForInsert();
                    }
			    }

                //Note all loans that will be affected, and remove autopay.
                loanSequences = GetAchLoans();
				RI.PutText(10, 18, "D", Key.Enter);
				string reasonCode = GetReasonCode(denialReasons[0]);
				RI.PutText(10, 57, reasonCode, Key.Enter);

                RecoveryProcessor.UpdateLogWithNewPhase(phase);
            }

			//Prompt for a denial letter and leave an activity record.
			string sequenceString;

            //if in recovery and the script bombed between this phase and the last phase then the loan sequences will be null and the user will need to provide them again.
            //loan sequence numbers
            if (loanSequences == null)
            {
                InputBox<TextBox> results = new InputBox<TextBox>("Please provide the loan sequence numbers included in the suspended ACH in a comma delimited format.", "Loan Sequences Needed");
                results.ShowDialog();
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

			string comment = $"Loan(s) {sequenceString} removed from Autopay.";
			TD22CommentData td22Data = new TD22CommentData(comment, true);
			DenialLetter(BrwDemos, td22Data, denialReasons, "Do you want to send a denial letter?", DA);
			Dialog.Def.Ok("Removal Complete");
            return;
		}

		private List<int> GetAchLoans()
		{
			List<int> loanSequences = new List<int>();
			while (RI.MessageCode != "90007")
			{
				for (int row = 17; RI.CheckForText(row, 3, "_"); row++)
					if (!RI.CheckForText(row, 58, "  ")) //Add the loan if it is connected to ACH
						loanSequences.Add(RI.GetText(row, 12, 3).ToInt());
				RI.Hit(Key.F8);
			}
			return loanSequences;
		}

		private string GetReasonCode(string reasonText)
		{
            return reasonText switch
            {
                "You have no loans eligible for Autopay" => "A",
                "You requested cancellation of Autopay" => "B",
                "Your bank account was reported as closed" => "C",
                "Missing account number" => "D",
                "Missing ABA number" => "F",
                "Your bank account number is invalid" => "G",
                "Your Autopay form is incomplete" => "I",
                "Your account is not in repayment" => "J",
                "Missing voided check" => "L",
                "You've had multiple insufficient funds" => "N",
                "The ABA number you provided is invalid" => "O",
                "Your Autopay request was not signed" => "S",
                "Banking information was changed" => "Z",
                "Your bank does not participate in Autopay" => "H",
                _ => "I",
            };
        }
	}
}