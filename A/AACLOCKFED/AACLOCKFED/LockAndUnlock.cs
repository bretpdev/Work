using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace AACLOCKFED
{
	public class LockAndUnlock : FedScriptBase
	{
		public enum Action { Lock, Unlock }
		private enum LockResult { Success, FoundVariance, FailedToLock }
		private enum SelectionResult { Success, CouldNotAccessMajorBatch, CouldNotFindMinorBatch, CouldNotSelectMinorBatch }

		private readonly string _errorFile;
		private string _userId;

		public LockAndUnlock(ReflectionInterface ri)
			: base(ri, "AACLOCKFED", Region.CornerStone)
		{
			_errorFile = string.Format("{0}{1}_Errors.{2:yyyy-MM-dd.HHmm}.txt", Efs.TempFolder, ScriptID, DateTime.Now);
		}

		public override void Main()
		{
			//Check access to the batch screens.
			if (!UserHasChangeAccess("TA0H", "TA0L"))
			{
				MessageBox.Show("You do not have the correct access to run this script.", ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			FastPath("PROF");
			_userId = GetText(2, 49, 7);

			//Make sure the user has the batch numbers ready.
			if (new MajorBatchQuestion().ShowDialog() != DialogResult.OK) { return; }

			//Get the batch numbers and desired action.
			Batches batches = new Batches();
			using (BatchEntry batchEntry = new BatchEntry(batches))
			{
				if (batchEntry.ShowDialog() != DialogResult.OK) { return; }
			}

			//Lock or unlock the batches, as requested.
			string finnishMessage;
			if (batches.Action == Action.Lock)
			{
				LockBatches(batches.ToArray());
				finnishMessage = "The batches have been locked.";
			}
			else
			{
				UnlockBatches(batches.ToArray());
				finnishMessage = "The batches have been unlocked.";
			}
			if (File.Exists(_errorFile)) { finnishMessage += string.Format("{0}Please see the {1} file for problems encountered during execution.", Environment.NewLine, _errorFile); }
			MessageBox.Show(finnishMessage, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}//Main()

		private void AddErrorRecord(string majorBatchNumber, string message)
		{
			bool newFile = !File.Exists(_errorFile);
			using (StreamWriter errorWriter = new StreamWriter(_errorFile, true))
			{
				if (newFile) { errorWriter.WriteLine("Major batch number, Error"); }
				errorWriter.WriteLine("{0}, {1}", majorBatchNumber, message);
			}
		}//AddErrorRecord()

		private List<string> GetMinorBatchNumbers(string majorBatchNumber)
		{
			List<string> minorBatchNumbers = new List<string>();
			if (Check4Text(1, 72, "TAX10"))
			{
				//Target screen.
				minorBatchNumbers.Add(GetText(4, 75, 5));
			}
			else if (Check4Text(1, 72, "TAX0Q"))
			{
				//Selection screen.
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 9; !Check4Text(row, 6, "     "); row++)
					{
						minorBatchNumbers.Add(GetText(row, 6, 5));
					}//for
					if (Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
					{ Hit(Key.Enter); }
					else
					{ Hit(Key.F8); }
				}//while
			}
			return minorBatchNumbers;
		}//GetMinorBatchNumbers()

		private LockResult LockBatch()
		{
			//Go right back into CTA0H, which goes to ITX6T for the just-selected batch.
			PutText(1, 4, "CTA0H", Key.Enter, true);
			//Make sure there's no variance.
			bool hasVariance = false;
			if (!Check4Text(12, 18, " 0")) { hasVariance = true; }
			if (!Check4Text(12, 31, " 0")) { hasVariance = true; }
			string variance = GetText(20, 13, 10).Replace(",", "");
			if (variance.Length > 0 && double.Parse(variance) > 0) { hasVariance = true; }
			if (hasVariance) { return LockResult.FoundVariance; }
			//Hit enter to get to the detail screen and lock the batch.
			Hit(Key.Enter);
			PutText(21, 27, "Y", Key.Enter);
			if (Check4Text(23, 2, "02776 BATCH SUCCESSFULLY LOCKED"))
			{ return LockResult.Success; }
			else
			{ return LockResult.FailedToLock; }
		}//LockBatch()

		private void LockBatches(IEnumerable<string> majorBatchNumbers)
		{
			foreach (string majorBatchNumber in majorBatchNumbers)
			{
				FastPath("TX3Z/ITA0L");
				PutText(5, 19, majorBatchNumber, Key.Enter);
				if (!Check4Text(23, 2, "     "))
				{
					AddErrorRecord(majorBatchNumber, "Could not access the major batch.");
					continue;
				}
				IEnumerable<string> minorBatchNumbers = GetMinorBatchNumbers(majorBatchNumber);
				foreach (string minorBatchNumber in minorBatchNumbers)
				{
					SelectionResult selectionResult = SelectBatch(majorBatchNumber, minorBatchNumber);
					if (selectionResult == SelectionResult.CouldNotSelectMinorBatch)
					{
						MessageBox.Show("Error selecting minor batch. Review error and start script again.", ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Error);
						EndDLLScript();
					}
					else if (selectionResult == SelectionResult.CouldNotAccessMajorBatch)
					{
						AddErrorRecord(majorBatchNumber, "Could not access the major batch.");
						break;
					}
					else if (selectionResult == SelectionResult.CouldNotFindMinorBatch)
					{
						AddErrorRecord(majorBatchNumber, minorBatchNumber + " Could not find the minor batch.");
						continue;
					}
					else
					{
						LockResult lockResult = LockBatch();
						if (lockResult != LockResult.Success)
						{
							string errorMessage = minorBatchNumber;
							if (lockResult == LockResult.FoundVariance) { errorMessage += " variance"; }
							AddErrorRecord(majorBatchNumber, errorMessage);
							UnassignTasks();
						}
					}
				}//foreach
			}//foreach
		}//LockBatches()

		private SelectionResult SelectBatch(string majorBatchNumber, string minorBatchNumber)
		{
			FastPath("TX3Z/CTA0H");
			PutText(10, 41, majorBatchNumber, Key.Enter);
			if (!Check4Text(23, 2, "     ")) { return SelectionResult.CouldNotAccessMajorBatch; }
			bool selectedBatch = false;
			while (!selectedBatch && !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 7; !Check4Text(row, 50, "     "); row += 5)
				{
					if (Check4Text(row, 50, minorBatchNumber))
					{
						PutText(21, 18, GetText(row, 3, 1), Key.Enter);
						if (Check4Text(1, 72, "T1X01"))
						{
							selectedBatch = true;
							break;
						}
						else
						{
							return SelectionResult.CouldNotSelectMinorBatch;
						}
					}//if
				}//for
				if (!selectedBatch)
				{
					if (Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
					{ Hit(Key.Enter); }
					else
					{ Hit(Key.F8); }
				}
			}//while
			return (selectedBatch ? SelectionResult.Success : SelectionResult.CouldNotFindMinorBatch);
		}//SelectBatch()

		private void UnassignTasks()
		{
			//If a minor batch was selected but for some reason was unable to be locked, it will show up as a task in the user's queue,
			//which prevents them from going into any other batches. To remedy this, unassign whatever task is open for the user.
			bool somethingGotUnassigned = false;
			foreach (string queue in new string[] { "AD", "AQ" })
			{
				FastPath("TX3Z/CTX6J");
				PutText(7, 42, queue);
				PutText(8, 42, "A1");
				PutText(13, 42, _userId);
				Hit(Key.Enter);
				if (Check4Text(1, 72, "TXX6O"))
				{
					PutText(8, 15, "", Key.Enter, true);
					if (Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED")) { somethingGotUnassigned = true; }
				}
			}//foreach
			if (!somethingGotUnassigned)
			{
				MessageBox.Show("Error unassigning batch with variance.", ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Error);
				EndDLLScript();
			}
		}//UnassignTasks()

		private bool UnlockBatch()
		{
			//Update the status to UNLOCKED.
			try
			{
				PutText(9, 19, "U", Key.Enter);
			}
			catch (Exception)
			{
				//If the field isn't writeable, the batch is already unlocked.
			}
			return Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED");
		}//UnlockBatch()

		private void UnlockBatches(IEnumerable<string> majorBatchNumbers)
		{
			foreach (string majorBatchNumber in majorBatchNumbers)
			{
				FastPath("TX3Z/CTA0L");
				PutText(5, 19, majorBatchNumber, Key.Enter);
				if (Check4Text(1, 72, "TAX10"))
				{
					//Target screen.
					if (!UnlockBatch()) { AddErrorRecord(majorBatchNumber, GetText(4, 75, 5)); }
				}
				else if (Check4Text(1, 72, "TAX0Q"))
				{
					//Selection screen. Go through all minor batches.
					while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
					{
						//Select each batch on the screen.
						for (int row = 9; !Check4Text(row, 3, "  "); row++)
						{
							PutText(22, 18, GetText(row, 3, 2), Key.Enter, true);
							if (!UnlockBatch()) { AddErrorRecord(majorBatchNumber, GetText(4, 75, 5)); }
							Hit(Key.F12);
						}//for
						if (Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
						{ Hit(Key.Enter); }
						else
						{ Hit(Key.F8); }
					}//while
				}
				else
				{
					AddErrorRecord(majorBatchNumber, "Could not access the major batch.");
				}
			}//foreach
		}//UnlockBatches()

		private bool UserHasChangeAccess(params string[] transactionIds)
		{
			FastPath("TX3Z");
			foreach (string transactionId in transactionIds.OrderBy(p => p))
			{
				Coordinate coord = FindText(transactionId);
				while (coord == null)
				{
					Hit(Key.F8);
					coord = FindText(transactionId);
				}
				if (!Check4Text(coord.Row, 74, "C")) { return false; }
			}
			return true;
		}//UserHasChangeAccess()
	}//class
}//namespace
