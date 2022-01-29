using System;
using System.IO;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace CORNTSTQUE
{
    public class TestQueueAccessUpdate : ScriptBase
    {
		private readonly string ERROR_FILE;
		private Credentials _loginData;

        public TestQueueAccessUpdate(ReflectionInterface ri)
            : base(ri, "CORNTSTQUE")
        {
			ERROR_FILE = Efs.TempFolder + ScriptID + "_Errors.txt";
        }

        public override void Main()
        {
			RI.LogOut();
			while (_loginData == null)
			{
				_loginData = Credentials.FromPrompt();
				if (_loginData != null && !RI.Login(_loginData.UserName, _loginData.Password, Region.CornerStone))
				{
					MessageBox.Show("You must put in a valid username and password to move forward", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
					_loginData = null;
				}
			}

			ProcessingDetails details = DetailsForm();
			if (!TestModeProperty)
			{//If we are running in live we want to update live and log into test
				if (AddQueueAccess(details)) { LogIntoTest(); }//
				else { return; }
			}
			AddQueueAccess(details);//this will always update test
			string finnishMessage = "Processing Complete!";
			if (File.Exists(ERROR_FILE))
			{
				finnishMessage = "Processing is complete, but there were errors.";
				finnishMessage += string.Format(" See {0} for details.", ERROR_FILE);
			}
			MessageBox.Show(finnishMessage, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }//Main()


		private bool AddQueueAccess(ProcessingDetails details)
		{
			FastPath("TX3Z/ATC00");
			if (!Check4Text(1, 38, "UHEAAFED"))
			{
				string cornMessage = "You must be logged into the CornerStone region before using this script.";
				MessageBox.Show(cornMessage, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			//Add or delete queue access for the cross-product of all user IDs and queues entered.
			string mode = (details.SelectedAction == ProcessingDetails.Action.Add ? "A" : "D");
			string success = (details.SelectedAction == ProcessingDetails.Action.Add ? "01004" : "01006");
			foreach (UserDetail user in details.Users)
			{
				foreach (QueueDetail queue in details.Queues)
				{
					FastPath("TX3Z/{0}TX64{1};{2};{3}", mode, queue.Queue, queue.SubQueue, user.ID);
					if (!Check4Text(1, 72, "TXX67"))
					{
						AddToErrorFile(user.ID, queue.Queue, queue.SubQueue);
						continue;
					}
					if (details.SelectedAction == ProcessingDetails.Action.Add)
					{
						PutText(10, 33, user.Type.Substring(0, 1));
					}
					Hit(Key.Enter);
					if (!Check4Text(23, 2, success)) { AddToErrorFile(user.ID, queue.Queue, queue.SubQueue); }
				}//foreach
			}//foreach
			return true;
		}
		private ProcessingDetails DetailsForm()
		{
			//Get the user IDs and ARCs from the user.
			ProcessingDetails details = new ProcessingDetails();
			using (MainForm form = new MainForm(details))
			{
				if (form.ShowDialog() != DialogResult.OK) { return null; }
			}
			return details;
		}

		private void LogIntoTest()
		{
			//change region to CornerStone test region
			FastPath("LOG");
			PutText(16, 12, "QTOR");
			RI.Hit(Key.Enter);
			PutText(20, 18, _loginData.UserName);
			PutText(20, 40, _loginData.Password);
			RI.Hit(Key.Enter);
			Coordinate cr = new Coordinate();
			cr = RI.FindText("K1/KU VUK1");
			PutText(cr.Row, (cr.Column - 2), "X");
			RI.Hit(Key.Enter);
			RI.Hit(Key.F10);
		}
		private void AddToErrorFile(string userId, string queue, string subQueue)
		{
			using (StreamWriter errorWriter = new StreamWriter(ERROR_FILE, true))
			{
				errorWriter.WriteLine("{0}, {1}/{2}", userId, queue, subQueue);
			}
		}//AddToErrorFile()
    }//class
}//namespace
