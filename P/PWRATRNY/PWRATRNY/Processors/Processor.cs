using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Dialog;

namespace PWRATRNY
{
    public class Processor
    {
        public ReflectionInterface RI { get; set; }
        public PowerOfAttorneyData POAdata { get; set; }
        public string ScriptID { get; set; }
        private DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public List<Relationship> Relationships { get; set; }

        public Processor(ReflectionInterface ri, string scriptID, DataAccess da, ProcessLogRun logRun, PowerOfAttorneyData data)
        {
            RI = ri;
            DA = da;
            ScriptID = scriptID;
            LogRun = logRun;
            POAdata = data;
            Relationships = DA.GetRelationships();
        }

        public Processor() { } //Default constructor for the OneLINKProcessor and CompassProcessor

        /// <summary>
        /// Main point for processing to start.
        /// </summary>
        public void Process(UserPOAEntry entry)
        {
            SystemBorrowerDemographics demos = DA.GetDemos(entry.UserEnteredAccountNumberOrSSN);
            if (demos == null)
                return;
            demos.LastName = $"{demos.LastName} {demos.Suffix ?? ""}".Trim();
            POAdata = new PowerOfAttorneyData(entry, demos);

            //Create system processors.
            OneLINKProcessor onelinkProcessor = new OneLINKProcessor(RI, ScriptID, DA, LogRun, POAdata);
            CompassProcessor compassProcessor = new CompassProcessor(RI, ScriptID, DA, LogRun, POAdata);

            //Check if borrower has open loans on OneLINK.
            bool doOneLINKProcessing = RI.HasOpenLoanOnOneLINK(POAdata.BorrowerDemos.Ssn);

            //Identify reference to make POA if it already exists.
            if (doOneLINKProcessing)
                onelinkProcessor.ReferenceCheckAndGathering();
            else
                compassProcessor.ReferenceCheckAndGathering();

            //Figure out what to do with the POA and gather reference information if needed.
            string poaMessage;
            if (Question.YesNo("Has the request for this reference been approved?", "POA"))
            {
                POAdata.RequestApproved = true;
                poaMessage = "The borrower's request for Power of Attorney has been approved." +
                    " An activity comment will be added, and a Power of Attorney Approval letter will be generated." +
                    "  Hit OK to continue, or Cancel to exit script without updating the system or generating a letter.";
            }
            else
            {
                POAdata.RequestApproved = false;
                poaMessage = "The borrower reference record may be added to the system, but the reference will not be approved for Power of Attorney." +
                    " A Power of Attorney Denial letter will be generated." +
                    "  Hit OK to continue, or Cancel to exit script without updating the system or generating a letter.";
            }
            if (Question.OkCancel(poaMessage, "POA"))
            {
                if (!DisplayAddAndModifyForm())
                    return;
            }
            else
                return;

            //Add or modify OneLINK reference if needed.
            if (doOneLINKProcessing)
                onelinkProcessor.CoordinateReferenceAdditionOrModification();

            //Add or modify COMPASS reference if needed.
            compassProcessor.CoordinateReferenceAdditionOrModification();

            //Print needed letters.
            Print(POAdata);

            //Create expiration queue task if needed.
            CreateExpirationQueueTasks(doOneLINKProcessing);
        }

        private void CreateExpirationQueueTasks(bool doOneLinkProcessing)
        {
            if (!POAdata.RequestApproved || POAdata.ExpirationDate.IsNullOrEmpty())
                return;

            string comment1 = $"{POAdata.RefData.FirstName} {POAdata.RefData.LastName}";
            string comment2 = "Check Power of Attorney documentation in imaging";

            CompassProcessor compass = new CompassProcessor(RI, ScriptID, DA, LogRun, POAdata);
            if (compass.HasLoanBalance(POAdata.BorrowerDemos.Ssn))
            {
                string comment = comment1 + " " + comment2;
                if (!AddArc(POAdata, comment))
                    RI.Atd37FirstLoan(POAdata.BorrowerDemos.Ssn, "EXTPA", comment, ScriptID, RI.UserId, null);
            }
            else if (doOneLinkProcessing)
            {
                const string QUEUE = "POAEXPIR";
                DateTime expirationDate = POAdata.ExpirationDate.ToDate();
                DateTime oneYearFromNow = DateTime.Now.AddYears(1);

                //If the expiration date is more than 1 year from the current date,
                //capture that date in the comments and only create the queue task for 1 year.
                if (expirationDate > oneYearFromNow)
                {
                    expirationDate = oneYearFromNow;
                    comment2 += $". Expiration Date is {POAdata.ExpirationDate}";
                }

                AddLP9OAQueue(POAdata, QUEUE, expirationDate, $"{comment1} {comment2}");
            }
        }

        private void AddLP9OAQueue(PowerOfAttorneyData poaData, string queue, DateTime expirationDate, string comment)
        {
            QueueData data = new QueueData()
            {
                AccountIdentifier = poaData.BorrowerDemos.Ssn,
                QueueName = queue,
                DateDue = expirationDate,
                Comment = comment
            };
            QueueResults result = data.AddQueue();
            if (!result.QueueAdded)
            {
                string message = $"There was an error adding a {queue} Queue to the OLQTSKBLDR tables for borrower: {poaData.BorrowerDemos.AccountNumber}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                Error.Ok(message);
            }
        }

        private bool AddArc(PowerOfAttorneyData data, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.BorrowerDemos.Ssn,
                Arc = "EXTPA",
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                Comment = comment,
                NeedBy = data.ExpirationDate.ToDate(),
                RegardsCode = "B",
                RegardsTo = data.BorrowerDemos.Ssn,
                ScriptId = ScriptID
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding an EXTPA ARC to borrower: {data.BorrowerDemos.AccountNumber}. Error: {string.Join(",", result.Errors)}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                Error.Ok(message);
            }
            return result.ArcAdded;
        }

        /// <summary>
        /// Displays add and modify form and sets UserModified indicator.
        /// </summary>
        private bool DisplayAddAndModifyForm()
        {
            AddAndModifyForm addAndModify = new AddAndModifyForm(POAdata, DA, Relationships);
            if (addAndModify.ShowDialog() == DialogResult.Cancel)
                return false;
            else if (addAndModify.IsUpdated)
            {
                //Note what was changed.
                POAdata.UserModified = true;
                POAdata.UserModifiedAddress = addAndModify.IsUpdatedAddress;
                POAdata.UserModifiedHomePhoneNumber = addAndModify.IsUpdatedHomePhone;
                POAdata.UserModifiedAltPhone = addAndModify.IsUpdatedOtherPhone;
                POAdata.UserModifiedEmail = addAndModify.IsUpdatedEmail;
                POAdata.UserModifiedForeignPhone = addAndModify.IsUpdatedForeignPhone;
                POAdata.UserModifiedReferenceName = addAndModify.IsUpdatedName;
                POAdata.UserModifiedRelationship = addAndModify.IsUpdatedRelationship;
            }
            return true;
        }

        private void Print(PowerOfAttorneyData POAData)
        {
            string letterId = POAData.PrintApprovalLetter ? "POACONF" : "POADEN";
            string keyLine = DocumentProcessing.ACSKeyLine(POAData.BorrowerDemos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string letterData = $"{DateTime.Now:MM/dd/yyyy},{keyLine},{POAData.BorrowerDemos.FirstName},{POAData.BorrowerDemos.LastName},{POAData.BorrowerDemos.Address1},"
                + $"{POAData.BorrowerDemos.Address2 ?? ""},{POAData.BorrowerDemos.City},{POAData.BorrowerDemos.State},{POAData.BorrowerDemos.ZipCode},"
                + $"{POAData.BorrowerDemos.Country},{POAData.BorrowerDemos.AccountNumber}";

            //Add the data to the Print Processing table
            int? success = EcorrProcessing.AddRecordToPrintProcessing(ScriptID, letterId, letterData, POAData.BorrowerDemos.AccountNumber, "MA2324");
            if (!success.HasValue || success == 0)
            {
                string message = $"There was an error adding the {letterId} letter to the print processing table for borrower: {POAData.BorrowerDemos.AccountNumber} when running the PWRATRNY script. Please contact SS for help.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Error.Ok(message);
            }
        }
    }
}