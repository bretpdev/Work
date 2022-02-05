using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Q;
using Reflection; //TODO: Yank this and remove the reference to Reflection.
using Key = Q.ReflectionInterface.Key;

namespace SCHMPNORCM
{
	public class SchoolSerialSetup : ScriptBase
	{
		private DataAccess _dataAccess;

		private List<SchoolEmailInfo> _schoolEmailInfo;
		private string[] info; //From school file: 1 = school code, 2 = effectiveDate, 3 = selected "Set School to" radio button text, 4 = selected "Required Options" radio button or check box text, 5 = "ELMRES" if the check box is checked ("" otherwise), 6 = school name (not from file; set from ClearinghouseAndNSLDSSchoolValidation() and CheckForValidSchoolandLoan())
		private List<string> _loanPrograms;
		private List<string> _notifications;
		private string _userId;

		public SchoolSerialSetup(ReflectionInterface ri)
			: base(ri, "SchMPNorCM")
		{
			_dataAccess = new DataAccess(RI.TestMode);
			_schoolEmailInfo = new List<SchoolEmailInfo>();
			_loanPrograms = new List<string>();
			_loanPrograms.Add("STFFRD");
			_loanPrograms.Add("UNSTFD");
			_loanPrograms.Add("PLUS");
			_notifications = new List<string>();
			_notifications.Add("APPCHG");
			_notifications.Add("APPREJ");
			_notifications.Add("BRWAPP");
			_notifications.Add("BRWINC");
			_notifications.Add("DSBCHG");
			_notifications.Add("EMAIL");
			_notifications.Add("INCMPL");
			_notifications.Add("INHINC");
			_notifications.Add("LONADJ");
			_notifications.Add("SAPLST");
			_notifications.Add("SCHRFD");
			_notifications.Add("EXTINT");
			_userId = GetUserIDFromLP40();
		}

		public override void Main()
		{
			//TODO: Modify the startup message to describe the script's purpose.
			string startupMessage = "This script does something magical and revolutionary. Click OK to continue, or Cancel to quit.";
			if (MessageBox.Show(startupMessage, ScriptID, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { EndDLLScript(); }

			//TODO: Fill in the script logic.
		}//Main()

		//TODO: Put this in Q.
		private void LocalLogIn(string LogonMode)
		{
			//int warn;
			////disconnect and reconnect if the user is connected
			//if (!Check4Text(16, 10, ">"))
			//{
			//    Session.Disconnect();
			//    Session.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1);
			//    Session.Connect();
			//}
			////wait for the logon screen to be displayed
			//Session.WaitForDisplayString(">", "0:0:30", 16, 10);
			////enter production (live) regions and wait for the greetings screen to be displayed
			//switch (LogonMode)
			//{
			//    case "Live":
			//    case "DevL":
			//        PutText(16, 12, "PHEAA", Key.Enter);
			//        Session.WaitForDisplayString("USERID", "0:0:30", 20, 8);
			//        PutText(20, 18, "UT00");
			//    case "Test":
			//    case "DevT":
			//        PutText(16, 12, "QTOR", Key.Enter);
			//        Session.WaitForDisplayString("USERID", "0:0:30", 20, 8);
			//        PutText(20, 18, "UT00");
			//}
			//while (true)
			//{
			//    //go to the InsertErr error handling subroutine if the user doesn't hit Insert
			//    //TODO: On Error GoTo InsertErr
			//    //wait for user to enter the password
			//    Session.WaitForTerminalKey(rcIBMInsertKey, "0:1:0");
			//    //enter if the user didn't already
			//    if (Check4Text(20, 8, "USERID") || Check4Text(8, 22, "USERID"))
			//    {
			//        //stop trying to log on if the developer hit insert without entering his user ID
			//        if (LogonMode.StartsWith("Dev") && Check4Text(20, 22, " ") && Check4Text(8, 34, " "))
			//        {
			//            //hit the insert key to toggle back to insert mode
			//            Hit(Key.Insert);
			//            //access the visual basic editor
			//            Session.EditMacro("");
			//            return;
			//        }
			//        Hit(Key.Enter);
			//        //warn the user and exit if the user does not have test region access
			//        if (Check4Text(16, 2, "ON008"))
			//        {
			//            MessageBox.Show("You are not authorized to access the Onelink and COMPASS test regions.  Contact System Operations if you need access to the test regions.", "Test Region Access Not Available");
			//            return;
			//        }
			//    }
			//    //exit the loop if the logon screen is no longer displayed
			//    else
			//    {
			//        //hit the insert key to toggle back to insert mode since the user hit insert on the wrong screen
			//        Hit(Key.Insert);
			//        break;
			//    }
			//    if (!Check4Text(20, 8, "USERID") && !Check4Text(8, 22, "USERID")) { break; }
			//}
			////select regions and other functions
			//switch (LogonMode)
			//{
			//    //change screen colors and access user's favorite screen in live
			//    case "Live":
			//    case "DevL":
			//        //set the color of updateable fields to green
			//        Session.SetColorMap(rcUnprotNormAlpha, rcGreen, rcBlack);
			//    //select the OneLINK and COMPASS test regions
			//    case "Test":
			//    case "DevT":
			//        //select the setup/sys ID
			//        Session.FindText("RS/UT", 3, 5);
			//        PutText(Session.FoundTextRow, Session.FoundTextColumn - 2, "X", Key.Enter);
			//        //change the color of updateable fields to magenta
			//        Session.SetColorMap(rcUnprotNormAlpha, rcMagenta, rcBlack);
			//}
			//return;
			////warn the user if Insert was not pressed when logging in
			////TODO: InsertErr:
			//MessageBox.Show("The script waited for a minute for you to hit Insert after you entered your password.  Since you didn't hit Insert, the script will end without completing some tasks.  In the future, please hit Insert whenever a script pauses and you are ready for it to resume.");
			//EndDLLScript();
		}//LocalLogIn()

		private void RunSchool(SetupDetails details)
		{
			if (details.SchoolSetTo == SetupDetails.SetupType.SerialMpn)
			{
				using (StreamWriter schoolInfoWriter = new StreamWriter(@"C:\Windows\temp\SchoolInfo.txt"))
				{
					schoolInfoWriter.WriteCommaDelimitedLine("School Name", "School Code", "Address1", "Address2", "City", "State", "Zip", "Country", "StateCode");
					CheckForValidSchoolandLoan(details);
					SerialMPNProcessing(schoolInfoWriter); //TODO: The above file is assumed to still be open for writing in this function.
				}
				string LtrID = (details.Program == SetupDetails.LoanProgram.Plus ? "SCHMPNP" : "SCHMPNS");
				GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(LtrID, DocumentHandling.CentralizedPrintingSystemToAddComments.stacBoth, "", @"C:\Windows\temp\SchoolInfo.txt", "SchMPNorCM", "StateCode", DocumentHandling.CentralizedPrintingDeploymentMethod.dmLetter, "", DocumentHandling.Barcode2DLetterRecipient.lrOther);
				SendEmails(details.SchoolSetTo);
			}
			else if (details.SchoolSetTo == SetupDetails.SetupType.Commonline)
			{
				CheckForValidSchoolandLoan(details);
				CommonlineProcessing(details);
				SendEmails(details.SchoolSetTo);
			}
			else
			{
				ClearinghouseAndNSLDSSchoolValidation();
				string sscrIndicator = (details.SchoolSetTo == SetupDetails.SetupType.Clearinghouse ? "C" : "N");
				ClearinghouseAndNSLDSProcessing(sscrIndicator, details.SchoolSetTo);
				SendEmails(details.SchoolSetTo);
			}
		}//RunSchool()

		//This function does the processing for Clearinghouse and NSLDS setup
		private void ClearinghouseAndNSLDSProcessing(string SSCRIndicator, SetupDetails.SetupType setupType)
		{
			DateTime EffectiveDate;
			FastPath("LPSCC" + info[1]);
			//If a selection screen is given
			if (Check4Text(1, 49, "INSTITUTION DEPARTMENT SELECTION"))
			{
				Coordinate foundText = FindText(" GEN ", 2, 2);
				while (foundText == null)
				{
					Hit(Key.F8);
					if (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY"))
					{
						MessageBox.Show("The school hasn't got a general department set up.");
						EndDLLScript();
					}
					foundText = FindText(" GEN ", 2, 2);
				}
				PutText(21, 13, GetText(foundText.Row, (foundText.Column - 4), 2), Key.Enter);
			}
			Hit(Key.F10);
			PutText(16, 61, SSCRIndicator);
			if (setupType == SetupDetails.SetupType.Nslds)
			{
				EffectiveDate = DateTime.Parse(info[2].ToDateFormat());
				PutText(16, 63, EffectiveDate.ToString("MMddyyyy"), Key.Enter);
			}
			else
			{
				EffectiveDate = DateTime.Parse(info[2].ToDateFormat());
				PutText(16, 63, EffectiveDate.ToString("MMddyyyy"), Key.Enter);
				FastPath("TX3Z/CTX0Y{0};000", info[1]);
				Hit(Key.F10);
				PutText(16, 19, "Y", Key.Enter);
			}
		}//ClearinghouseAndNSLDSProcessing()

		private void CommonlineProcessing(SetupDetails details)
		{
			int NotificationCounter = 0;
			FastPath("LPSCC{0}", info[1]);
			if (Check4Text(1, 49, "INSTITUTION DEPARTMENT SELECTION"))
			{
				Coordinate foundText = FindText(" GEN ", 6, 6);
				while (foundText == null)
				{
					Hit(Key.F8);
					if (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY"))
					{
						MessageBox.Show("The school hasn't got a general department set up.");
						EndDLLScript();
					}
					foundText = FindText(" GEN ", 6, 6);
				}
				PutText(21, 13, GetText(foundText.Row, (foundText.Column - 4), 2), Key.Enter);
			}

			PutText(20, 13, ReturnCLAPP(details));
			PutText(21, 13, ReturnCLChange(details), Key.Enter);

			if (!Check4Text(22, 3, "49000 DATA SUCCESSFULLY UPDATED") && !Check4Text(22, 3, "49007 NO FIELDS UPDATED"))
			{
				MessageBox.Show("There was a problem updating LPSC.  The script can't continue processing.");
				EndDLLScript();
			}

			int NotificationLimit = 0;
			if (details.CommonlineApplication && details.CommonlineChange)
			{
				NotificationLimit = _notifications.Count;
			}
			else if (details.CommonlineApplication && !details.CommonlineChange && !details.CommonlineDisbursementRoster && !details.ModificationResponse && !details.HoldAllDisbursements && !details.ServiceBureauParticipant)
			{
				NotificationLimit = 7;
			}
			else if (details.CommonlineApplication || details.ModificationResponse)
			{
				NotificationLimit = 6;
			}

			bool MRespOnly = (details.ModificationResponse && !details.CommonlineChange);
			while (NotificationCounter <= NotificationLimit)
			{
				FastPath("LP0XA");
				PutText(8, 49, info[1]);
				PutText(10, 49, "001");
				PutText(12, 49, _notifications[NotificationCounter]); //see init. and globals
				PutText(14, 49, "C01", Key.Enter);
				
				if (MRespOnly) { PutText(20, 22, "1"); }

				if (Check4Text(22, 3, "48002 ENTERED KEY ALREADY EXISTS"))
				{
					FastPath("LP0XC");
					PutText(8, 49, info[1]);
					PutText(10, 49, "001");
					PutText(12, 49, _notifications[NotificationCounter], Key.Enter); //see init. and globals

					if (MRespOnly) { PutText(20, 22, "1"); }

					if (_notifications[NotificationCounter] == "EXTINT")
					{
						PutText(7, 22, "N");
					}
					else
					{
						PutText(4, 77, "C01", Key.Enter);
					}
				}
				Hit(Key.F6);
				NotificationCounter++;
			}

			UpdateCompassLO(details);
		}//CommonlineProcessing()

		//Does processing for Serial MPN
		private void SerialMPNProcessing(StreamWriter schoolInfoWriter)
		{
			string SchoolName;
			string Address1;
			string Address2;
			string City;
			string State;
			string StateCode;
			string ZIP;
			string Country;
			int Counter;
			Counter = 0;
			//Open
			FastPath("LPSCC{0}", info[1]);
			if (Check4Text(1, 49, "INSTITUTION DEPARTMENT SELECTION"))
			{
				Coordinate foundText = FindText(" GEN ", 6, 6);
				while (foundText == null)
				{
					Hit(Key.F8);
					if (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY"))
					{
						MessageBox.Show("The school hasn't got a general department set up.");
						EndDLLScript();
					}
					foundText = FindText(" GEN ", 6, 6);
				}
				PutText(21, 13, GetText(foundText.Row, (foundText.Column - 4), 2), Key.Enter);
			}
			if (Check4Text(1, 57, "INSTITUTION DEMOGRAPHICS"))
			{
				SchoolName = GetText(5, 21, 40);
				Address1 = GetText(8, 21, 30);
				Address2 = GetText(9, 21, 30).Trim('_');
				City = GetText(11, 21, 30);
				State = GetText(11, 59, 2);
				ZIP = GetText(11, 66, 14);
				if (State == "FC")
				{
					State = GetText(12, 21, 15).Replace("_", "");
					Country = GetText(12, 55, 25);
					StateCode = "FC";
				}
				else
				{
					StateCode = State;
					Country = "";
				}
				Hit(Key.F10);
				schoolInfoWriter.WriteCommaDelimitedLine(SchoolName, info[1], Address1, Address2, City, State, ZIP, Country, StateCode);
				schoolInfoWriter.Close();
				if (Check4Text(1, 46, "SCHOOL SPECIFIC INFORMATION DISPLAY"))
				{
					Hit(Key.F10);
					if (info[4] == "STFFRD")
					{
						PutText(7, 26, "1", Key.Enter);
					}
					else
					{
						PutText(8, 26, "1", Key.Enter);
					}
					while (Counter < 2)
					{
						if (info[4] == "PLUS")
						{
							FastPath("TX3Z/CTX10{0}{1}", info[1], info[4]);
							Counter = 2;
						}
						else
						{
							FastPath("TX3Z/CTX10{0}{1}", info[1], _loanPrograms[Counter]);
						}

						PutText(14, 24, "Y");
						PutText(15, 17, info[2]);
						PutText(15, 41, " ", Key.Enter);
						Counter++;
					}

					FastPath("LP54A");
					PutText(3, 20, info[1]);
					PutText(4, 20, "001");
					PutText(10, 19, "QLSRL", Key.Enter);
					PutText(7, 2, "LT");
					PutText(7, 5, "07");
					PutText(11, 2, "SCHOOL SET AS SERIAL FOR " + info[4] + " Loan {SchMPNorCMLsetup} / " + _userId);
					Hit(Key.F6);
				}
				else
				{
					MessageBox.Show("For some reason LPCS wasn't able to be accessed.");
					EndDLLScript();
				}
			}
			else
			{
				MessageBox.Show("The School ID has not been set up in OneLINK.");
				EndDLLScript();
			}
		}//SerialMPNProcessing()

		//This function figures out if the school is valid for Clearhouse and/or NSLDS processing.
		private void ClearinghouseAndNSLDSSchoolValidation()
		{
			FastPath("LPSCC{0}", info[1]);
			if (Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"))
			{
				MessageBox.Show("School ID has not been set up on OneLINK.  Please add the school ID information before running this script.");
				EndDLLScript();
			}
			info[6] = GetText(5, 21, 40);
			FastPath("TX3Z/CTX0Y{0}", info[1]);
			if (Check4Text(23, 2, "30021 INSTITUTION NOT DEFINED ON INSTITUTION DEMOGRAPHICS"))
			{
				MessageBox.Show("School ID has not been set up on COMPASS on TX0Y.  Please add the school ID information before running this script.");
				EndDLLScript();
			}
		}//ClearinghouseAndNSLDSSchoolValidation()

		private void CheckForValidSchoolandLoan(SetupDetails details)
		{
			int CommonlineCounter = 0;
			int ErrorCounter = 0; //If ErrorCounter counts any errors the script will stop.
			FastPath("LPSCC{0}", info[1]);
			if (Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"))
			{
				MessageBox.Show("School ID has not been set up on OneLINK.  Please add the school ID information before running this script.");
				ErrorCounter++;
			}
			else
			{
				if (Check4Text(1, 57, "INSTITUTION DEMOGRAPHICS"))
				{
					info[6] = GetText(5, 21, 40);
				}
				else if (Check4Text(1, 49, "INSTITUTION DEPARTMENT SELECTION"))
				{
					PutText(21, 13, "01", Key.Enter);
					info[6] = GetText(5, 21, 40);
				}
			}
			FastPath("TX3Z/CTX0Y{0}", info[1]);
			if (Check4Text(23, 2, "30021 INSTITUTION NOT DEFINED ON INSTITUTION DEMOGRAPHICS"))
			{
				MessageBox.Show("School ID has not been set up on COMPASS on TX0Y.  Please add the school ID information before running this script.");
				ErrorCounter++;
			}
			while (CommonlineCounter < 3)  //for Commonline processing
			{
				if (details.SchoolSetTo == SetupDetails.SetupType.SerialMpn)  //for Commonline processing
				{
					if (info[4] == "PLUS")
					{
						FastPath("TX3Z/CTX10{0}{1}", info[1], info[4]);
					}
					else
					{
						FastPath("TX3Z/CTX10{0}{1}", info[1], _loanPrograms[CommonlineCounter]);
					}
				}
				else
				{
					FastPath("TX3Z/CTX10{0}{1}", info[1], _loanPrograms[CommonlineCounter]); //for Commonline processing
				}
				if (Check4Text(23, 2, "50510 SCHOOL NOT FOUND"))
				{
					MessageBox.Show("School ID has not been set up on COMPASS on TX10.  Please add the school ID information before running this script.");
					ErrorCounter = ErrorCounter + 1;
				}
				FastPath("TX3Z/CTX13");
				PutText(5, 18, info[1]);
				if (details.SchoolSetTo == SetupDetails.SetupType.SerialMpn)  //for Commonline processing
				{
					if (info[4] == "PLUS")
					{
						PutText(7, 18, info[4]);
					}
					else
					{
						PutText(7, 18, _loanPrograms[CommonlineCounter]);
					}
				}
				else
				{
					PutText(7, 18, _loanPrograms[CommonlineCounter]); //for Commonline processing
				}
				PutText(9, 18, "000749", Key.Enter);
				if (Check4Text(23, 2, "50510 SCHOOL NOT FOUND"))
				{
					MessageBox.Show("School ID has not been set up on COMPASS on TX13.  Please add the school ID information before running this script.");
					ErrorCounter++;
				}
				if (Check4Text(23, 2, "47004") || Check4Text(23, 2, "30021") || Check4Text(23, 2, "50510"))
				{
					MessageBox.Show("School has not been set up on OneLink and Compass.  Please have the school added to the system before running the script");
					ErrorCounter++;
				}
				if (details.SchoolSetTo == SetupDetails.SetupType.SerialMpn && info[4] == "PLUS") //for Commonline processing
				{
					CommonlineCounter = 3; //goes through the loop once
				}
				else if (details.SchoolSetTo == SetupDetails.SetupType.SerialMpn && info[4] == "STFFRD" && CommonlineCounter == 1)
				{
					CommonlineCounter = 3; //goes through the loop twice if STFFRD
				}
				else
				{
					CommonlineCounter++; //for Commonline processing must do loop 3 times
				}
				if (ErrorCounter > 0)
				{
					MessageBox.Show("School ID has not been set up on the screens and systems you have been given.  Please add the school ID information before running this script.");
					EndDLLScript();
				}
			}
		}//CheckForValidSchoolandLoan()

		//Retrieves the user ID.
		private string UserIdRe()
		{
			string UserID;
			using (StreamReader userInfoReader = new StreamReader(@"C:\Windows\temp\userinfo.txt"))
			{
				UserID = userInfoReader.ReadLine().SplitAgnosticOfQuotes(",")[0];
			}
			return UserID;
		}//UserIdRe()

		private void UpdateCompassLO(SetupDetails details)
		{
			int x;
			string Ltype = "";
			for (x = 1; x <= 3; x++)
			{
				if (x == 1) { Ltype = "STFFRD"; }
				if (x == 2) { Ltype = "UNSTFD"; }
				if (x == 3) { Ltype = "PLUS"; }
				FastPath("PO3DC{0}{1}", info[1], Ltype);
				if (Check4Text(7, 24, "_")) { PutText(7, 24, "A"); }
				PutText(12, 17, "Y");
				PutText(12, 39, "A");
				if (details.HoldAllDisbursements)
				{
					PutText(12, 77, "Y");
				}
				else
				{
					PutText(12, 77, "N");
				}
				if (details.ServiceBureauParticipant && details.Elmres) { PutText(15, 29, "ELMRES"); }
				PutText(17, 15, "N");
				Hit(Key.F6);
				if (!Check4Text(22, 2, "01003 NO FIELDS UPDATED"))
				{
					if (!Check4Text(2, 32, "SCHOOL ACTIVITY DETAIL"))
					{
						PutText(7, 29, "N");
						PutText(7, 68, "UO01BBNO");
						PutText(9, 32, "Y");
						PutText(9, 57, "N");
						PutText(11, 27, "N");
						PutText(13, 37, "N");
						if (Check4Text(17, 31, "_")) { PutText(17, 31, "N"); }
						if (Check4Text(17, 58, "_")) { PutText(17, 58, "N"); }
						Hit(Key.Enter);
						Hit(Key.F6);
					}
					PutText(8, 18, "05");
					PutText(9, 18, "12");
					PutText(10, 18, "020");
					PutText(14, 2, "Adding school as Commonline.", Key.Enter);
				}
			}
			if (info[5] == "")
			{
				//Access PO4F in add mode for each loan type
				if (details.CommonlineDisbursementRoster)
				{
					//CLDISB
					for (x = 1; x <= 3; x++)
					{
						if (x == 1) { Ltype = "STFFRD"; }
						if (x == 2) { Ltype = "UNSTFD"; }
						if (x == 3) { Ltype = "PLUS"; }
						FastPath("PO4FA04;{0};;{1};CLDISB", info[1], Ltype);
						if (!Check4Text(22, 2, "04691 ALL ACTIVE NOTIFICATION ID'S ALREADY ESTABLISHED FOR ENTERED KEY"))
						{
							PutText(9, 9, "A");
							PutText(11, 30, info[1]);
							PutText(11, 77, "04");
							PutText(13, 9, "B");
							PutText(13, 18, "C");
							PutText(13, 42, "004");
							PutText(14, 32, "N");
							PutText(14, 58, "N");
							PutText(16, 21, "I=MGMAILUT");
							PutText(17, 13, "UTREGION");
							PutText(17, 33, "CL ROST V4 OUT");
							PutText(19, 15, "03");
							PutText(19, 64, "A");
							PutText(20, 15, "02");
							PutText(20, 64, "A");
							PutText(21, 15, "08");
							PutText(21, 64, "A");
							Hit(Key.F6);
						}
					}
				}
				if (details.CommonlineChange)
				{
					//CHGRSP
					for (x = 1; x <= 3; x++)
					{
						if (x == 1) { Ltype = "STFFRD"; }
						if (x == 2) { Ltype = "UNSTFD"; }
						if (x == 3) { Ltype = "PLUS"; }
						FastPath("PO4FA04;{0};;{1};CHGRSP", info[1], Ltype);
						if (!Check4Text(22, 2, "04691 ALL ACTIVE NOTIFICATION ID'S ALREADY ESTABLISHED FOR ENTERED KEY"))
						{
							PutText(9, 9, "A");
							PutText(11, 30, info[1]);
							PutText(11, 77, "04");
							PutText(13, 9, "B");
							PutText(13, 18, "C");
							PutText(13, 42, "004");
							PutText(14, 32, "N");
							PutText(14, 58, "N");
							PutText(16, 21, "I=MGMAILUT");
							PutText(17, 13, "UTREGION");
							PutText(17, 33, "CL RESP OUT ");
							PutText(19, 15, "03");
							PutText(19, 64, "A");
							PutText(20, 15, "02");
							PutText(20, 64, "A");
							PutText(21, 15, "08");
							PutText(21, 64, "A");
							Hit(Key.F6);
						}
					}
				}
				if (details.ModificationResponse)
				{
					//MODRSP
					for (x = 1; x <= 3; x++)
					{
						if (x == 1) { Ltype = "STFFRD"; }
						if (x == 2) { Ltype = "UNSTFD"; }
						if (x == 3) { Ltype = "PLUS"; }
						FastPath("PO4FA04;{0};;{1};MODRSP", info[1], Ltype);
						if (!Check4Text(22, 2, "04691 ALL ACTIVE NOTIFICATION ID'S ALREADY ESTABLISHED FOR ENTERED KEY"))
						{
							PutText(9, 9, "A");
							PutText(11, 30, info[1]);
							PutText(11, 77, "04");
							PutText(13, 9, "B");
							PutText(13, 18, "C");
							PutText(13, 42, "004");
							PutText(14, 32, "N");
							PutText(14, 58, "N");
							PutText(16, 21, "I=MGMAILUT");
							PutText(17, 13, "UTREGION");
							PutText(17, 33, "CL RESP OUT ");
							PutText(19, 15, "03");
							PutText(19, 64, "A");
							PutText(20, 15, "02");
							PutText(20, 64, "A");
							PutText(21, 15, "08");
							PutText(21, 64, "A");
							Hit(Key.F6);
						}
					}
				}
			}
		}//UpdateCompassLO()

		private void SendEmails(SetupDetails.SetupType setupType)
		{
			bool Found;
			string Ltype = "";
			Found = false;
			if (setupType == SetupDetails.SetupType.Commonline) { Ltype = "CL"; }
			if (setupType == SetupDetails.SetupType.SerialMpn) { Ltype = "Serial"; }

			foreach (SchoolEmailInfo ear in _schoolEmailInfo)
			{
				if (ear.SchoolCode == info[1] && ear.SetupType == Ltype && info[4] == ear.LoanType)
				{
					Found = true;
				}
				else if (ear.SchoolCode == info[1] && ear.SetupType == Ltype && info[4] != ear.LoanType)
				{
					//TODO: See if LoanType would be better as a List.
					ear.LoanType += ", " + info[4]; //Loan Type
					Found = true;
				}
			}

			if (!Found)
			{
				SchoolEmailInfo ear = new SchoolEmailInfo();
				ear.SchoolCode = info[1];
				ear.SchoolName = info[6];
				ear.SetupType = Ltype;
				ear.LoanType = info[4];
				_schoolEmailInfo.Add(ear);
			}
		}//SendEmails()

		private void SendEmailFromArray()
		{
			if (_schoolEmailInfo.Count > 0)
			{
				string eSubject = "";
				string eBody = "";
				//subject
				foreach (SchoolEmailInfo ear in _schoolEmailInfo)
				{
					//Check to see if the school is a UT school.
					if (_dataAccess.IsUtahSchool(ear.SchoolCode))
					{
						if (ear.SetupType == "CL") { eSubject = ear.SchoolName + " - " + ear.SchoolCode + " has been set up as a Commonline school."; }
						if (ear.SetupType == "Serial") { eSubject = ear.SchoolName + " - " + ear.SchoolCode + " has been set up as a Serial school."; }

						//Msg text
						if (ear.SetupType == "CL") { eBody = "This school has been set up for the following Commonline processing: " + ear.LoanType + "."; }
						if (ear.SetupType == "Serial") { eBody = "This school has been set as a Serial school for " + ear.LoanType + " Loan Type(s)."; }
						List<string> recipients = DataAccess.GetEmailRecipienlList(RI.TestMode, "School MPN Commonline Setup", DataAccessBase.EmailLookupOption.None);
						SendMail(RI.TestMode, string.Join(";", recipients.ToArray()), "", eSubject, eBody, "", "", "", Common.EmailImportanceLevel.Normal, RI.TestMode);
					}
				}
			}
			_schoolEmailInfo = new List<SchoolEmailInfo>();
		}//SendEmailFromArray()

		private void UpdateProductionRegion(SetupDetails details, string SchFile)
		{
			_schoolEmailInfo = new List<SchoolEmailInfo>();
			LocalLogIn("Live");
			string UserID = GetUserIDFromLP40();
			using (StreamReader schoolFileReader = new StreamReader(SchFile))
			{
				while (!schoolFileReader.EndOfStream)
				{
					info = schoolFileReader.ReadLine().SplitAgnosticOfQuotes(",").ToArray();
					RunSchool(details);
				}
			}
			if (File.Exists(SchFile)) { File.Delete(SchFile); }
			SendEmailFromArray();
			MessageBox.Show("The processing was completed successfully.");
		}//UpdateProductionRegion()

		private void UpdateTestRegion(SetupDetails details, string SchFile)
		{
			_schoolEmailInfo = new List<SchoolEmailInfo>();
			LocalLogIn("Test");
			string UserID = GetUserIDFromLP40();
			using (StreamReader schoolFileReader = new StreamReader(SchFile))
			{
				while (!schoolFileReader.EndOfStream)
				{
					info = schoolFileReader.ReadLine().SplitAgnosticOfQuotes(",").ToArray();
					RunSchool(details);
				}
			}
			if (File.Exists(SchFile)) { File.Delete(SchFile); }
			SendEmailFromArray();
			MessageBox.Show("The processing was completed successfully.");
		}//UpdateTestRegion()

		//TODO: Put this in the SetupDetails class.
		private string ReturnCLAPP(SetupDetails details)
		{
			if (details.CommonlineApplication && details.SchoolSetTo == SetupDetails.SetupType.Commonline)
			{
				return "004";
			}
			else
			{
				return "";
			}
		}//ReturnCLAPP()

		//TODO: Put this in the SetupDetails class.
		private string ReturnCLChange(SetupDetails details)
		{
			if (details.CommonlineChange && details.SchoolSetTo == SetupDetails.SetupType.Commonline)
			{
				return "004";
			}
			else
			{
				return "";
			}
		}//ReturnCLChange()
	}//class
}//namespace
