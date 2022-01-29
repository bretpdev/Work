using System;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;

namespace OPSCHKPHN
{
    class CheckByPhoneProcessor : ScriptBase
    {

        private OPSEntry Data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ri"></param>
        public CheckByPhoneProcessor(ReflectionInterface ri, OPSEntry data)
            : base(ri, "OPSCKPHN")
        {
            Data = data;
        }

        public bool Process()
        {
            bool validSSN = ValidSSNOnCompassCheck();
			if (validSSN)
			{
				if (Data.TS24DOB == "")
					ProcessNoDOB();//if no DOB on Compass
				else
					CreateCheckByPhoneEntryAndComments();
			}
			return validSSN;
        }

        private bool ValidSSNOnCompassCheck()
        {
            RI.FastPath(string.Format("TX3ZITS24{0}", Data.TS24SSN));
            if (!RI.CheckForText(1, 76, "TSX25"))
            {
                MessageBox.Show("The SSN or Account Number entered is not valid on COMPASS.  Please try again.", "Not Valid On Compass", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
            }
			else //pickup needed information
            {
                Data.TS24DOB = RI.GetText(19, 68, 10);
				Data.TS24Name = RI.GetText(4, 37, 42);
				Data.TS24SSN = RI.GetText(4, 16, 11).Replace(" ", "");
				Data.AccountNumber = RI.GetText(6, 10, 12).Replace(" ", "");
				return true;
            }
        }

        private void ProcessNoDOB()
        {
            RI.FastPath(string.Format("LP22I{0}", Data.TS24SSN));
			Data.TS24DOB = string.Format("{0}/{1}/{2}", RI.GetText(4, 72, 2), RI.GetText(4, 74, 2), RI.GetText(4, 76, 4));
            string confirm;
            if (Data.ConfOpt == OPSEntry.ConfirmationOptions.Letter)
                confirm = "LETTER";
            else
                confirm = "NONE";
            string comment = string.Format("Update DOB {0} Routing Number {1}, Account Number {2}, Payment Amount {3}, Account Type {4}, Effective Date {5}, CONFIRMATION {6}", Data.TS24DOB, Data.RoutingNumber, Data.BankAccountNumber, Data.PaymentAmount, Data.CalculatedAccountType, Data.EffectiveDate, confirm);
			ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = Data.AccountNumber,
				Arc = "DOBUP",
                DelinquencyArc = "",
				ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                LoanPrograms = null,
                LoanSequences = null,
                NeedBy = null,
                ProcessFrom = null,
                ProcessOn = null,
                ProcessTo = null,
                RecipientId = null,
                RegardsCode = null,
                RegardsTo = "",
                ScriptId = ScriptId
            };
			ArcAddResults result = arc.AddArc();
			if (!result.ArcAdded)
			{
				string message = string.Format("Error adding arc to ArcAdd database for {0}, script {1}", arc.AccountNumber, ScriptId);
				ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
			}

            if (File.Exists(OPSCKPHN.LogFilePathAndName)) 
				File.Delete(OPSCKPHN.LogFilePathAndName);
            RI.FastPath("TX3ZATC00");  //lease TD22 so user can't update comments
        }

        private void CreateCheckByPhoneEntryAndComments()
        {
            string rPhase;
            string rSSN;
            //check for recovery
            if (File.Exists(OPSCKPHN.LogFilePathAndName))
            {
				using (StreamReader sr = new StreamReader(OPSCKPHN.LogFilePathAndName))
				{
					rPhase = sr.ReadLine();
					rSSN = sr.ReadLine();
				} 
                //check if the SSN from the user = the one in the recovery file
                if (rSSN != Data.TS24SSN)
                {
                    MessageBox.Show("The script is trying to recover but the SSN you provided and the SSN stored from recovery don't match.  Please contact Systems Support.","Recovery Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					System.Environment.Exit(0);
                }
            }
            else
				rPhase = "ALL";  //process normally

			if (rPhase == "ALL") //check if data needs to be written out to the DB
            {
				/*Data.BankAccountNumber = LegacyCryptography.Encrypt(Data.BankAccountNumber, LegacyCryptography.Keys.NoradOPS);*/ 
				//encryption moved into a Trigger on Norad
				DataAccess.AddEntryToDB(Data); //add to DB
				using (StreamWriter sw = new StreamWriter(OPSCKPHN.LogFilePathAndName)) //update log file
					sw.WriteCommaDelimitedLine("FILEWRITE", Data.TS24SSN);
            }

			if (rPhase == "ALL" || rPhase == "FILEWRITE") //check if the comments need to be added
            {
				AddCommentToArcAdd();
				using (StreamWriter sw = new StreamWriter(OPSCKPHN.LogFilePathAndName))
					sw.WriteCommaDelimitedLine("TD22", Data.TS24SSN);    //update log
            }
            if (File.Exists(OPSCKPHN.LogFilePathAndName))
				File.Delete(OPSCKPHN.LogFilePathAndName); //delete recovery log file
			RI.FastPath("TX3ZATC00"); //leave TD22 so user can't update comments
        }

		private void AddCommentToArcAdd()
		{
			string comment = string.Format("${1} {0} Requested payment effective date {2}.", "{0}", double.Parse(Data.PaymentAmount).ToString("00000.00"), Data.EffectiveDate);
			if (Data.ConfOpt == OPSEntry.ConfirmationOptions.Letter)
			{
				comment = string.Format(comment, "Check by phone payment confirmation letter sent.");
				ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
				{
					AccountNumber = Data.AccountNumber,
					Arc = "PHNPL",
					DelinquencyArc = "",
					ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
					Comment = comment,
					IsEndorser = false,
					IsReference = false,
					LoanPrograms = null,
					LoanSequences = null,
					NeedBy = null,
					ProcessFrom = null,
					ProcessOn = null,
					ProcessTo = null,
					RecipientId = null,
					RegardsCode = null,
					RegardsTo = "",
					ScriptId = ScriptId
				};
				ArcAddResults result = arc.AddArc();
				if (!result.ArcAdded)
				{
					string message = string.Format("Error adding arc to ArcAdd database for {0}, script {1}", arc.AccountNumber, ScriptId);
					ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
				}
			}
			else if (Data.ConfOpt == OPSEntry.ConfirmationOptions.None)
			{
				comment = string.Format(comment, "Check by phone payment.");
				ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
				{
					AccountNumber = Data.AccountNumber,
					Arc = "PHNPN",
					DelinquencyArc = "",
					ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
					Comment = comment,
					IsEndorser = false,
					IsReference = false,
					LoanPrograms = null,
					LoanSequences = null,
					NeedBy = null,
					ProcessFrom = null,
					ProcessOn = null,
					ProcessTo = null,
					RecipientId = null,
					RegardsCode = null,
					RegardsTo = "",
					ScriptId = ScriptId
				};
				ArcAddResults result = arc.AddArc();
				if (!result.ArcAdded)
				{
					string message = string.Format("Error adding arc to ArcAdd database for {0}, script {1}", arc.AccountNumber, ScriptId);
					ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
				}
			}
		}
    }
}
