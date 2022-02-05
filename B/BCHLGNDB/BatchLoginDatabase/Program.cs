using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Windows.Forms;
using Q;

namespace BatchLoginDatabase
{
	public class Program
	{
		[STAThread]
		static void Main()
		{
			new Program().Run();
		}

		private void Run()
		{
			bool testMode = Environment.GetCommandLineArgs().Contains("test");
			DataAccess da = new DataAccess(testMode);
			LoginCredentais lData = new LoginCredentais();

			using (frmBatchLogin userLogin = new frmBatchLogin(lData, testMode))
			{
				if (userLogin.ShowDialog() == DialogResult.Cancel)
				{
					return;
				}
				try
				{
					object validLogin = new DirectoryEntry("", "UHEAA\\" + lData.UserId, lData.Password).NativeObject;

					if (!CheckActiveDirGroup(testMode, lData))
					{
						string emailBody = string.Format("UserID: {0} tried to access the Batch Login Database Application on {1}", lData.UserId, DateTime.Now);
						Common.SendMail(testMode, "SSHELP@utahsbr.edu;cmccomb@utahsbr.edu", "Batch Login Database", "Unauthorized Batch Login Database Access Attempt", emailBody, "", "", "", Common.EmailImportanceLevel.High, testMode);
						MessageBox.Show("You do not have access rights to this script/database. Please contact System Support to inquire about this access.", "No Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
				catch (Exception)
				{
					MessageBox.Show("The UserId or Password entered was not valid");
					return;
				}
			}

			da.DeleteOldHistory();

			while (true)
			{
				IList<UserIdsAndPasswords> idsAndPasswordsFromDb = da.GetUsersAndPasswordFromDb();
				List<UserIdsAndPasswords> selectedIds = new List<UserIdsAndPasswords>();
				Results userSelection = new Results();
				frmUserIdAndPasswordInput sltIds = new frmUserIdAndPasswordInput(idsAndPasswordsFromDb, selectedIds, userSelection);

				if (sltIds.ShowDialog() == DialogResult.Cancel) { return; }

				List<UserIdsAndPasswords> updatedPasswords = new List<UserIdsAndPasswords>();
				bool process = true;
				switch (userSelection.Action)
				{	
					case 1://change
						while (true)
						{
							using (frmChange change = new frmChange(selectedIds, updatedPasswords))
							{
								DialogResult result = change.ShowDialog();
								if (result == DialogResult.Yes) { break; }
								else if (result == DialogResult.Cancel)
								{
									process = false;
									break;
								}
							}
						}
						if (process)
						{
							UpdateUserIdsAndPasswords(updatedPasswords, da, lData);
							MessageBox.Show("The Passowrds have been updated in the Database");
						}
						break;
					case 2://add
						while (true)
						{
							using (frmAdd add = new frmAdd(updatedPasswords, idsAndPasswordsFromDb))
							{
								DialogResult result = add.ShowDialog();
								if (result == DialogResult.Yes) { break; }
								else if (result == DialogResult.Cancel)
								{
									process = false;
									break;
								}
							}
						}
						if (process)
						{
							AddUserdsToTheDb(updatedPasswords, da);
							MessageBox.Show("The User Id and Password have been added to the Database");
						}
						break;
					case 3://delete
						while (true)
						{
							using (frmDelete delete = new frmDelete(selectedIds, updatedPasswords))
							{
								DialogResult result = delete.ShowDialog();
								if (result == DialogResult.Yes) { break; }
								else if (result == DialogResult.Cancel)
								{
									process = false;
									break;
								}
							}
						}
						if (process)
						{
							DeleteUserIdsAndPasswords(updatedPasswords, da, lData);
							MessageBox.Show("The User Id and Password have been deleted from the Database");
						}
						break;
				}//end switch
			}
		}//end Run()

		private void AddUserdsToTheDb(List<UserIdsAndPasswords> userIdsAndPasswordsToUpdate, DataAccess da)
		{
			foreach (UserIdsAndPasswords item in userIdsAndPasswordsToUpdate)
			{
				da.AddUserIdsAndPasswords(item);
			}
		}

		private void DeleteUserIdsAndPasswords(List<UserIdsAndPasswords> userIdsAndPasswordsToUpdate, DataAccess da, LoginCredentais lData)
		{
			foreach (UserIdsAndPasswords item in userIdsAndPasswordsToUpdate)
			{
				da.DeleteUserIdsAndPasswords(item, lData);
			}
		}

		private void UpdateUserIdsAndPasswords(List<UserIdsAndPasswords> userIdsAndPasswordsToUpdate, DataAccess da, LoginCredentais lData)
		{
			foreach (UserIdsAndPasswords item in userIdsAndPasswordsToUpdate)
			{
				da.UpdateUserIdsAndPasswords(item, lData);
			}
		}

		private bool CheckActiveDirGroup(bool testMode, LoginCredentais lData)
		{
			DirectoryEntry searchEntry = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local");
			DirectorySearcher searcher = new DirectorySearcher();
			searcher.SearchRoot = searchEntry;
			searcher.Filter = string.Format("SAMAccountName={0}",lData.UserId);
			SearchResult result = searcher.FindOne();

			if (result != null)
			{
				ResultPropertyCollection attributes = result.Properties;
				//Check if the user logging in is a developer
				int devCount = ((from fedUser in
									 (attributes["memberOf"]).OfType<string>()
								 where fedUser.ToLowerInvariant()
								 .Contains("Developers".ToLowerInvariant())
								 select fedUser).Count());

				//Check if the user logging in is in System Support
				int supCount = ((from fedUser in
									 (attributes["memberOf"]).OfType<string>()
								 where fedUser.ToLowerInvariant()
							 .Contains("SystemAnalysts".ToLowerInvariant())
								 select fedUser).Count());

				//if we are in test mode developers and system analyst should have access
				if (testMode && supCount > 0 || devCount > 0)
				{
					return true;
				}
				else if (!testMode && supCount > 0)//If this is in Live then only system analyst should have access
				{
					return true;
				}
				else//anyone else should not have access
				{
					return false;
				}
			}
			return false;
		}
	}//end Program
}
