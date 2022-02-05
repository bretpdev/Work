using System;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;

namespace OPSCBPFED
{
    class CheckByPhoneProcessor : ScriptBase
    {

        private OPSEntry Data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ri"></param>
        public CheckByPhoneProcessor(ReflectionInterface ri, OPSEntry data)
            : base(ri, "OPSCBPFED")
        {
            Data = data;
        }
        public bool Process()
        {
            bool validSSN = ValidSSNOnCompassCheck();
            if (validSSN)
            {
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

        public void CreateCheckByPhoneEntryAndComments()
        {
            string rPhase;
            string rSSN;
            //check for recovery
            if (File.Exists(OPSCBPFED.LogFilePathAndName))
            {
                using (StreamReader sr = new StreamReader(OPSCBPFED.LogFilePathAndName))
                {
                    rPhase = sr.ReadLine();
                    rSSN = sr.ReadLine();
                }
                //check if the SSN from the user = the one in the recovery file
                if (rSSN != Data.TS24SSN)
                {
                    MessageBox.Show("The script is trying to recover but the SSN you provided and the SSN stored from recovery don't match.  Please contact Systems Support.", "Recovery Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Environment.Exit(0);
                }
            }
            else
                rPhase = "ALL";

            if (rPhase == "ALL")
            {
                DataAccess.AddEntryToDB(Data); //add to DB
                using (StreamWriter sw = new StreamWriter(OPSCBPFED.LogFilePathAndName)) //update log file
                    sw.WriteCommaDelimitedLine("FILEWRITE", Data.TS24SSN);
            }

            if (rPhase == "ALL" || rPhase == "FILEWRITE") //check if the comments need to be added
            {
                AddCommentToArcAdd();
                using (StreamWriter sw = new StreamWriter(OPSCBPFED.LogFilePathAndName))
                    sw.WriteCommaDelimitedLine("TD22", Data.TS24SSN);    //update log
            }
            if (File.Exists(OPSCBPFED.LogFilePathAndName))
                File.Delete(OPSCBPFED.LogFilePathAndName); //delete recovery log file
            RI.FastPath("TX3ZATC00"); //leave TD22 so user can't update comments
        }

        private void AddCommentToArcAdd()
        {
            string comment = string.Format("{0} accepted CBP ${1} with the effective date {2}.  Curr Delq: {3}.  Ln Pgm:  {4}.", Environment.UserName, double.Parse(Data.PaymentAmount).ToString("00000.00"), Data.EffectiveDate, Data.DaysDelinquent, Data.LoanPrograms);
            if (Data.ConfOpt == OPSEntry.ConfirmationOptions.Letter)
            {
                comment = string.Format(comment, "Check by phone payment confirmation letter sent.");
                ArcData arc = new ArcData(DataAccessHelper.Region.CornerStone)
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
                ArcData arc = new ArcData(DataAccessHelper.Region.CornerStone)
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
            else if (Data.ConfOpt == OPSEntry.ConfirmationOptions.Email)
            {
                comment = string.Format(comment, "Check by phone payment confirmation email sent.");
                ArcData arc = new ArcData(DataAccessHelper.Region.CornerStone)
                {
                    AccountNumber = Data.AccountNumber,
                    Arc = "PHNPE",
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