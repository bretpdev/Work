using DPALETTERS.Forms;
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

namespace DPALETTERS.Cancellation
{
    public class CancellationProcessor : ScriptBase
    {
        public new static string ScriptId { get; set; } = "DPALETTERS";
        public ProcessLogRun LogRun { get; set; }
        public Utilities Utilities { get; set; }
        public DataAccess DA { get; set; }

        //Collections MD will run through this and require the user provide the SSN/Account Number
        public CancellationProcessor(ReflectionInterface ri) : base(ri,ScriptId)
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
            while (loopResult != ProcessLoop.Success && loopResult != ProcessLoop.Failure)
            {
                loopResult = Process(runNumber);
                runNumber++;
            }
        }

        public ProcessLoop Process(int runNumber)
        {
            string docFolder = EnterpriseFileSystem.GetPath("DPA", DataAccessHelper.Region.Uheaa);
            CancellationResponse response = null;

            using (DPACancellation cancellation = new DPACancellation())
            {
                var result = cancellation.ShowDialog();
                if (result == DialogResult.OK)
                {
                    response = cancellation.Response;
                }
                else
                {
                    return ProcessLoop.Failure;
                }
            };
            var docInfo = response.GetDocumentInfoFromResponse();

            //Make sure the SSN and account number exist on the system
            Borrower borrower = Utilities.GetBorrowerFromIdentifier(response.BorrowerAccountIdentifier);
            if(borrower == null)
            {
                string message = $"Provided borrower account identifier does not exist on system. {response.BorrowerAccountIdentifier}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Warning.Ok(message, "Bad Account Identifier");
                return ProcessLoop.Retry;
            }

            //Make sure the cosigner exists on the system if one was provided
            Borrower cosigner = null;
            if (!response.CosignerAccountIdentifier.IsNullOrEmpty())
            {
                cosigner = Utilities.GetBorrowerFromIdentifier(response.CosignerAccountIdentifier);
                if (cosigner == null)
                {
                    string message = $"Provided cosigner account identifier does not exist on system. {response.CosignerAccountIdentifier}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    Dialog.Warning.Ok(message, "Bad Account Identifier");
                    return ProcessLoop.Retry;
                }
            }

            //Access LC05
            string dpInd = "N";
            LC05Information info = Utilities.GetLC05Information(new LC05Information() { Borrower = borrower, Comaker = cosigner, DrawDate = null, DPInd = dpInd });

            //warn the user and end the script if the letter chosen does not correspond to the aux status of the loan with the greatest status date
            if(docInfo.Document == "DPACANP" && info.LoanStatus == "O" && docInfo.CommentText != "MANUAL DPACANP LETTER") //The manual letter is to make testing the DPACANP letter easier
            {
                string message = $"A PIF, rehabilitation, or consolidation cancellation letter cannot be generated for the borrower because not all of the borrower's loans are closed. Borrower has open loans.";
                LogRun.AddNotification( message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Warning.Ok(message, "Open Loans");
                return ProcessLoop.Retry;
            }
            
            //Go to LC34 to update the loans if there are open loans
            if(info.LoanStatus == "O" && docInfo.CommentText != "MANUAL DPACANP LETTER")
            {
                Utilities.UpdateLC34(info);
            }

            //Create the letter
            var demos = RI.GetDemographicsFromLP22(borrower.Ssn);
            demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            var keyline = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            
            //construct the letter data, DPACANP and DPACANO share the same letter data
            //SSN,LASTNAME,FIRSTNAME,ADDRESS1,ADDRESS2,CITY,STATE,COUNTRY,ZIP,VARTXT,KEYLINE,ACCOUNTNUMBER
            string letterData = $"{demos.Ssn},{demos.LastName},{demos.FirstName},{demos.Address1},{demos.Address2},{demos.City},{demos.State},{demos.Country},{demos.ZipCode},{docInfo.VariableText},{keyline},{demos.AccountNumber}";
            int? ppid = EcorrProcessing.AddOneLinkRecordToPrintProcessing(ScriptId, docInfo.Document, letterData, demos.AccountNumber, "MA2329");
            if (!ppid.HasValue)
            {
                string message = $"Failed to add record for Account: {demos.AccountNumber} to print processing  Letter {docInfo.Document}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message);
                return ProcessLoop.Failure;
            }

            //Add the arc and comment adding the record to the PrintProcessing table
            var arcAdded = RI.AddCommentInLP50(demos.Ssn, "LT", "03", docInfo.ActionCode, docInfo.CommentText, ScriptId);
            if(!arcAdded)
            {
                string message = $"Failed to add arc to LP50 for Account: {demos.AccountNumber} Arc: {docInfo.ActionCode} Comment: {docInfo.CommentText}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message);
                return ProcessLoop.Failure;
            }

            Dialog.Info.Ok("Completed Successfully, record added to PrintProcessing.");
            return ProcessLoop.Success;
        }
    }
}
