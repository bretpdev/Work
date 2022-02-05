using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static DPALETTERS.Utilities;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace DPALETTERS.Confirmation
{
    public class ConfirmationProcessor : ScriptBase
    {
        public new static string ScriptId { get; set; } = "DPALETTERS";
        public ProcessLogRun LogRun { get; set; }
        public Utilities Utilities { get; set; }
        public DataAccess DA { get; set; }

        //Collections MD will run through this and require the user provide the SSN/Account Number
        public ConfirmationProcessor(ReflectionInterface ri) : base(ri, ScriptId)
        {
            //Create a ProcessLogRun from ScriptBase's internal ProcessLogData
            LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false);
            DA = new DataAccess(LogRun);
            Utilities = new Utilities(ri);
        }

        //print DPA confirmation letter
        public override void Main()
        {
            int runNumber = 0;
            ProcessLoop loopResult = ProcessLoop.Start;
            while(loopResult != ProcessLoop.Success && loopResult != ProcessLoop.Failure)
            {
                loopResult = Process(runNumber);
                runNumber++;
            }
        }    
        
        private ProcessLoop Process(int runNumber)
        {
            string docFolder = EnterpriseFileSystem.GetPath("DPA", DataAccessHelper.Region.Uheaa);

            BorrowerResponse borrowerResponse = Utilities.GetBorrowerFromUser(false);
            if(borrowerResponse.Response == ProcessLoop.Failure)
            {
                return ProcessLoop.Failure;
            }
            if (borrowerResponse.Borrower == null)
            {
                string message = $"The SSN or account number entered was not valid in the session.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Warning.Ok(message, "Borrower not Found");
                return ProcessLoop.Retry;
            }
            Borrower borrower = borrowerResponse.Borrower;
          

            BorrowerResponse comakerResponse = Utilities.GetBorrowerFromUser(true, "Enter the comaker's SSN if applicable, or press ok if not without entering anything otherwise.");
            if (comakerResponse.Response == ProcessLoop.Failure)
            {
                return ProcessLoop.Failure;
            }
            if (comakerResponse.Response == ProcessLoop.Retry)
            {
                string message = $"The cosigner SSN or account number entered was not valid in the session.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Warning.Ok(message, "Borrower not Found");
                return ProcessLoop.Retry;
            }

            Borrower comaker = comakerResponse.Borrower;
            var dateResponse = Utilities.GetDrawDate(borrower);
            if (dateResponse.Result != DialogResult.OK)
            {
                return ProcessLoop.Failure;
            }

            string dpInd = "Y";
            LC05Information info = Utilities.GetLC05Information(new LC05Information() { Borrower = borrower, Comaker = comaker, DrawDate = dateResponse.DrawDate, DPInd = dpInd});
            
            if(info == null)
            {
                return ProcessLoop.Failure;
            }

            //prompt the user for the draw amount, prompt the user to re-enter the draw amount if it is not greater than or equal to the expected payment amount
            var amountResponse = Utilities.ValidateDrawAmount(info.ExpectedPayment);
            if (amountResponse.Result != DialogResult.OK)
            {
                return ProcessLoop.Failure;
            }

            //go to LC34 to update the loans
            Utilities.UpdateLC34(info);

            //Create the letter
            var demos = RI.GetDemographicsFromLP22(borrower.Ssn);
            demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            var keyline = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //determine the draw date
            string drawDay = "";
            if(info.DrawDate?.Day == 7)
            {
                drawDay = "7th";
            }
            else if(info.DrawDate?.Day == 15)
            {
                drawDay = "15th";
            }
            else
            {
                drawDay = "22nd";
            }
            //construct the letter data
            //SSN,LASTNAME,FIRSTNAME,ADDRESS1,ADDRESS2,CITY,STATE,COUNTRY,ZIP,BILLEDAMT,DRAWDATE,DRAWDAY,KEYLINE,ACCOUNTNUMBER
            string letterData = $"{demos.Ssn},{demos.LastName},{demos.FirstName},{demos.Address1},{demos.Address2},{demos.City},{demos.State},{demos.Country},{demos.ZipCode},{string.Format("{0:N}", amountResponse.DrawAmount.Value)},\"{info.DrawDate.Value.ToString("MMMM dd, yyyy")}\",{drawDay},{keyline},{demos.AccountNumber}";
            int? ppid = EcorrProcessing.AddOneLinkRecordToPrintProcessing(ScriptId,"DPACONF",letterData,demos.AccountNumber,"MA2329");
            if(!ppid.HasValue)
            {
                string message = $"Failed to add record for Account: {demos.AccountNumber} to print processing";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok(message);
                return ProcessLoop.Failure;
            }

            var result = AddArc(demos, $"sent dpa confirmation letter to borr, first draw on {info.DrawDate.Value.ToString("MM/dd/yyyy")}");
            if(!result)
            {
                return ProcessLoop.Failure;
            }

            Dialog.Info.Ok("Completed Successfully, record added to PrintProcessing.");
            return ProcessLoop.Success;
        }

        public bool AddArc(SystemBorrowerDemographics demos, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion);
            arc.Arc = "DRNDP";
            arc.AccountNumber = demos.AccountNumber;
            arc.ActivityType = "LT";
            arc.ActivityContact = "03";
            arc.ArcTypeSelected = ArcData.ArcType.OneLINK;
            arc.Comment = comment;
            arc.ScriptId = ScriptId;
            var result = arc.AddArc();

            if(!result.ArcAdded)
            {
                string message = $"Failed to add arc to LP50 for Account: {demos.AccountNumber} Arc: {arc.Arc} Comment: {arc.Comment}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message);
                return false;
            }

            return true;
        }
    }
}
