using System;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace DUDILTR
{
	public class DueDiligence : BatchScriptBase
	{
		public DueDiligence(ReflectionInterface ri)
			: base(ri, "DUDILTR")
		{
		}

		public override void Main()
		{
			if (!CalledByMasterBatchScript())
			{
				string message = "This script works the DL01 queue. Hit OK to continue or Cancel to quit.";
				DialogResult startResult = MessageBox.Show(message, ScriptID, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
				if (startResult != DialogResult.OK) { return; }
			}

			WorkQueue();
			ProcessingComplete();
		}//Main()

		private void WorkQueue()
		{
			for (string ssn = SelectNextTask(); !string.IsNullOrEmpty(ssn); ssn = SelectNextTask())
			{
				//Complete the task.
				FastPath("TX3Z/ITX6XDL;01;" + ssn);
				PutText(21, 18, "01", Key.F2);
				PutText(8, 19, "C");
				PutText(9, 19, "COMPL", Key.Enter);
				if (!Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED"))
				{
					//If the task doesn't complete, notify the user and end the script.
					//This renders recovery unnecessary.
					string message = "The DUDILTR script was unable to complete the selected queue task. Please complete selected DL 01 queue task manually and rerun the script.";
					MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					EndDLLScript();
				}
			}//for
		}//WorkQueue()

		private string SelectNextTask()
		{
			FastPath("TX3Z/ITX6XDL;01");
			if (Check4Text(23, 2, "01020")) { return ""; }

			//Find the next task in "U" status.
			int taskRow = 8;
			while (!Check4Text(taskRow, 3, "  ") && !Check4Text(23, 2, "90007"))
			{
				if (Check4Text(taskRow, 75, "U")) { break; }
				taskRow += 3;
				if (taskRow > 17)
				{
					Hit(Key.F8);
					taskRow = 8;
				}
			}

			//Open the task if one was found, and return the SSN.
			if (Check4Text(taskRow, 75, "U"))
			{
				string ssn = GetText(taskRow, 6, 9);
				string selection = GetText(taskRow, 3, 2);
				PutText(21, 18, selection, Key.Enter);
				return ssn;
			}
			else
			{
				return "";
			}
		}//SelectNextTask()
	}//class
}//namespace
