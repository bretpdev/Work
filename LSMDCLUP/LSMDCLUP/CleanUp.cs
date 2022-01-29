using System;
using System.IO;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace LSMDCLUP
{
	public class CleanUp : BatchScriptBase
	{
		//Recovery value is acount number, phase.
		private const string ERROR_FILE = @"T:\Error LS MD Primary Phone Clean Up.txt";
		private const string RECOVERY_PHASE_ARC_ADDED = "ARC added";
		private const string RECOVERY_PHASE_PHONE_NOT_UPDATED = "Phone not updated";
		private const string RECOVERY_PHASE_PHONE_UPDATED = "Phone updated";

		public CleanUp(ReflectionInterface ri)
			: base(ri, "LSMDCLUP")
		{
		}

		public override void Main()
		{
			const string SAS_FILE = @"T:\Loan Servicing MD Primary Phone Clean Up.txt";

			if (!CalledByMasterBatchScript())
			{
				string startupMessage = "This script cleans up invalid primary phone numbers caused by a MauiDUDE error. Click OK to continue, or Cancel to quit.";
				if (MessageBox.Show(startupMessage, ScriptID, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { EndDLLScript(); }
			}

			string[] accountNumbers = File.ReadAllLines(SAS_FILE);

			//Recover if needed.
			int i = 0;
			if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
			{
				string[] recoveryValues = Recovery.RecoveryValue.Split(',');
				while (accountNumbers[i] != recoveryValues[0]) { i++; }
				if (recoveryValues[1] == RECOVERY_PHASE_PHONE_UPDATED) { AddArc(accountNumbers[i]); }
				i++;
			}

			//Process the remaining accounts.
			for (; i < accountNumbers.Length; i++)
			{
				if (UpdatePhone(accountNumbers[i])) { AddArc(accountNumbers[i]); }
			}

			//Clean up.
			File.Delete(SAS_FILE);

			if (File.Exists(ERROR_FILE))
			{
				string finnishMessage = string.Format("Processing Complete{0}. See the {1} file for accounts that had problems.", Environment.NewLine, ERROR_FILE);
				MessageBox.Show(finnishMessage, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				ProcessingComplete();
			}
		}//Main()

		private void AddArc(string accountNumber)
		{
			if (ATD22AllLoansBackedUpWithATD37FirstApp(accountNumber, "MXADD", "", false) != Common.CompassCommentScreenResults.CommentAddedSuccessfully)
			{
				AddToErrorReport(accountNumber, GetText(23, 2, 71));
			}
			UpdateRecovery(accountNumber, RECOVERY_PHASE_ARC_ADDED);
		}//AddArc()

		private void AddToErrorReport(string accountNumber, string message)
		{
			using (StreamWriter errorWriter = new StreamWriter(ERROR_FILE, true))
			{
				errorWriter.WriteLine("{0}, {1}", accountNumber, message);
			}
		}//AddToErrorReport()

		private bool UpdatePhone(string accountNumber)
		{
			//Go to the COMPASS home phone.
			FastPath("TX3Z/CTX1J;{0}", accountNumber);
			Hit(Key.F6);
			Hit(Key.F6);
			Hit(Key.F6);
			if (!Check4Text(16, 14, "H")) { PutText(16, 14, "H", Key.Enter); }

			//Check whether this account needs to be updated.
			if (!Check4Text(17, 14, "801") || !Check4Text(17, 23, "555") || !Check4Text(17, 31, "1212"))
			{
				UpdateRecovery(accountNumber, RECOVERY_PHASE_PHONE_NOT_UPDATED);
				return false;
			}

			//Get the info that we want to use from the current phone number.
			string phoneLastVerified = GetText(16, 45, 2) + GetText(16, 48, 2) + GetText(16, 51, 2);
			string sourceCode = GetText(19, 14, 2);

			//Go to the most recent non-bogus phone number and get what we need from it.
			while (Check4Text(17, 14, "801") && Check4Text(17, 23, "555") && Check4Text(17, 31, "1212")) { Hit(Key.F8); }
			string mbl = GetText(16, 20, 1);
			string consent = GetText(16, 30, 1);
			string areaCode = GetText(17, 14, 3).Replace("_", "");
			string exchange = GetText(17, 23, 3).Replace("_", "");
			string local = GetText(17, 31, 4).Replace("_", "");
			string extension = GetText(17, 40, 5).Replace("_", "");
			string validity = GetText(17, 54, 1);

			//Return to the current phone number and update it.
			while (!Check4Text(23, 2, "90007")) { Hit(Key.F7); }
			PutText(16, 20, mbl);
			PutText(16, 30, consent);
			PutText(16, 45, phoneLastVerified);
			PutText(17, 14, areaCode, true);
			PutText(17, 23, exchange, true);
			PutText(17, 31, local, true);
			PutText(17, 40, extension, true);
			PutText(17, 54, validity);
			PutText(19, 14, sourceCode);
			Hit(Key.Enter);

			if (Check4Text(23, 2, "01097"))
			{
				UpdateRecovery(accountNumber, RECOVERY_PHASE_PHONE_UPDATED);
				return true;
			}
			else
			{
				AddToErrorReport(accountNumber, GetText(23, 2, 77));
				UpdateRecovery(accountNumber, RECOVERY_PHASE_PHONE_NOT_UPDATED);
				return false;
			}
		}//UpdatePhone()

		private void UpdateRecovery(string accountNumber, string phase)
		{
			Recovery.RecoveryValue = string.Format("{0},{1}", accountNumber, phase);
		}//UpdateRecovery()
	}//class
}//namespace
