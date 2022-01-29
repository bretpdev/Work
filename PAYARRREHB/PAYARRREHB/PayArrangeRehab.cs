using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.DocumentProcessing;

namespace PAYARRREHB
{
    public partial class PayArrangeRehab : ScriptBase
    {
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }
        public PayArrangeRehab(ReflectionInterface ri)
            : base(ri, "PAYARRREHB", DataAccessHelper.Region.Uheaa)
        {
            PLR = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false);
        }

        public override void Main()
        {
            DA = new DataAccess(PLR.LDA);
            if (DA.CheckUserAccess() > 0)
            {
                if (ShowForm(DA) == DialogResult.OK)
                    Dialog.Info.Ok("Processing Complete");
            }
            else
                Dialog.Info.Ok("You do not have the correct access to run this application. If your access level needs to be changed, please direct this error to your supervisor.", "No Access");
        }

        private DialogResult ShowForm(DataAccess da)
        {
            Payment = new PaymentArrangement();

            ArrangementForm arrangement = new ArrangementForm(this, da);
            DialogResult result = arrangement.ShowDialog();

            if (Payment.Type == ArrangementType.Setup)
                return DialogResult.OK;

            if (!ProgramState.ProcessCoborrowers)
                return DialogResult.OK;

            List<CoBorrower> cobs = da.GetCoborrowers(Payment.SSN);
            foreach (var cob in cobs.DistinctBy(o => o.AccountNumber))
            {
                arrangement = new ArrangementForm(this, cob.CoBorrowerSSN, da);
                result = arrangement.ShowDialog();
            }

            return result;
        }

        public bool HasLC05
        {
            get
            {
                return CheckLC05();
            }
        }
        public bool HasLP22
        {
            get
            {
                return CheckLP22();
            }
        }

        public PaymentArrangement Payment { get; set; }

        /// <summary>
        /// Check if the borrower exists and retrieve the SSN/Account Number
        /// </summary>
        /// <returns>True if borrower found, False if not found</returns>
        public bool CheckLP22()
        {
            if (!Payment.SSN.IsNullOrEmpty())
                RI.FastPath("LP22I" + Payment.SSN);
            else
                RI.FastPath(string.Format("LP22I;;;;;;{0};;", Payment.AccountNumber));
            return LoadDemographics();

        }

        /// <summary>
        /// Retrieves the borrowers demographics from the system.
        /// </summary>
        /// <param name="needSsn"></param>
        private bool LoadDemographics()
        {
            if (RI.CheckForText(22, 3, "47004"))
            {
                Dialog.Info.Ok("No borrower found, please try again", "No Borrower Found");
                return false;
            }
            if (Payment.SSN.IsNullOrEmpty())
                Payment.SSN = RI.GetText(3, 23, 9);
            else
                Payment.AccountNumber = RI.GetText(3, 60, 12).Replace(" ", "");
            Payment.FirstName = RI.GetText(4, 44, 12);
            Payment.LastName = RI.GetText(4, 5, 30);
            Payment.Address1 = RI.GetText(10, 9, 35);
            Payment.Address2 = RI.GetText(11, 9, 35);
            Payment.City = RI.GetText(12, 9, 35);
            Payment.State = RI.GetText(12, 52, 2);
            Payment.Zip = RI.GetText(12, 60, 5);
            return true; //Borrower was found and demo's gathered
        }

        /// <summary>
        /// Check to see if borrower has an LC05 record and the status of the borrower loans.
        /// </summary>
        /// <returns></returns>
        public bool CheckLC05()
        {
            RI.FastPath("LC05I" + Payment.SSN);
            if (RI.CheckForText(22, 3, "47004"))
            {
                Dialog.Info.Ok("Borrower does not have an LC05 record", "No LC05 Found");
                return false;
            }
            if (!CheckLoanStatus())
                return false;
            return true;
        }

        /// <summary>
        /// Checks LC05 to verify that the borrower is in default and not transfered to ED.
        /// </summary>
        /// <returns></returns>
        private bool CheckLoanStatus()
        {
            while (!RI.CheckForText(22, 3, "46004"))
            {
                int row = 7;
                for (int i = 0; i < 5; i++)
                {
                    string selection = RI.GetText(row, 3, 1);
                    if (!selection.IsNullOrEmpty())
                    {
                        RI.PutText(21, 13, selection, ReflectionInterface.Key.Enter, true);
                        if (RI.CheckForText(1, 60, "DEFAULT/CLAIM DISPLAY"))
                        {
                            if (RI.CheckForText(4, 10, "03"))
                                if (RI.CheckForText(19, 73, "MMDDCCYY"))
                                    return true;
                        }
                        RI.Hit(ReflectionInterface.Key.F12);
                    }
                    row += 3;
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            Dialog.Info.Ok("Borrower is not in default", "Not Eligible");
            return false;
        }

        /// <summary>
        /// Processes each type of Payment Arrangement
        /// </summary>
        public void Process()
        {

            //state.isCoborrower = cobs.Count > 0 ? true : false;

            if (Payment.Type == ArrangementType.Generate)
            {
                Generate();
            }
            else if (Payment.Type == ArrangementType.Setup)
            {
                Setup();
            }
        }

        const string LetterId = "RHBAGRMNT";
        const string CostCenter = "MA2329";
        /// <summary>
        /// Creates a 30 day hold, adds an LP9O task, prints the letter and adds a comment to the borrower account.
        /// </summary>
        private void Generate()
        {
            if (!CheckLC05())
                return;
            CreateHold(30, DateTime.Now); //Add 30 days to the hold

            //Create the comments to add an LP9O task
            string comment1 = "Follow up to ensure signed rehab agreement has";
            string comment2 = "been received.";
            if (!RI.AddQueueTaskInLP9O(Payment.SSN, "DRHBAGMT", DateTime.Now.AddDays(7), comment1, comment2, "", ""))
                PLR.AddNotification($"There was an error adding an DRHBAGMT task in LP9O for {Payment.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Warning);

            //Create a data file and send it to centralized printing
            string dataLine = CreateDataLine();
            var result = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, LetterId, dataLine, Payment.AccountNumber, CostCenter);
            string comment = $"Rehab Agreement Sent to Borrower. Monthly payment amount is {Payment.Amount:C}, 1ST due date is {Payment.DueDate:MMddyyyy}, agreement to be returned by {DateTime.Now.AddDays(60):MMddyyyy}.";
            var arc = new ArcData(DataAccessHelper.Region.Pheaa)
            {
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                ScriptId = base.ScriptId,
                ActivityType = "LT",
                ActivityContact = "03",
                Arc = "DRHBA",
                AccountNumber = Payment.AccountNumber,
                Comment = comment
            };
            if (!arc.AddArc().ArcAdded)
                PLR.AddNotification($"Error adding DRHBA comment for borrower {Payment.AccountNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
        }


        /// <summary>
        /// Creates a payment arrangement, puts a 7 day hold, creates an LP9O task and comments the account
        /// </summary>
        private void Setup()
        {
            if (!CheckLC05())
                return;
            CreatePaymentArrangement();
            CreateHold(20, Payment.DueDate.Value); //Add a 20 day hold

            //Create comments to add an LP9O task
            string comment = "Follow up to ensure first rehab payment was made";
            if (!AddQueueTaskInLP9O(Payment.SSN, "DRHB1PMT", Payment.DueDate.Value.AddDays(1), comment, "", "", ""))
                PLR.AddNotification($"There was an error adding a DRHB1PMT task in LP9O for {Payment.AccountNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);

            comment = "Ensure borr’s rehab payment was received within 20 days of due date";
            if (!AddQueueTaskInLP9O(Payment.SSN, "DRHBFLLW ", Payment.DueDate.Value.AddDays(21), comment, "", "", ""))
                PLR.AddNotification("There was an error adding a DRHB1PMT task in LP9O for {Payment.AccountNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);


            //Add a comment to the borrower account
            comment = string.Format("Borrower returned signed rehabilitation agreement form. 1st due date {0}, monthly payment amount {1:C}."
                + " Updated billing code to 11, OK to bill. Placed hold type 11 on LC18 until {2}", Payment.DueDate.Value.ToString("MMddyyyy"), Payment.Amount,
                Payment.DueDate.Value.AddDays(20).ToString("MMddyyyy"));
            var arc = new ArcData(DataAccessHelper.Region.Pheaa)
            {
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                ScriptId = base.ScriptId,
                ActivityType = "FO",
                ActivityContact = "04",
                Arc = "DRWTN",
                AccountNumber = Payment.AccountNumber,
                Comment = comment
            };
            if (!arc.AddArc().ArcAdded)
                PLR.AddNotification($"Error adding DRWTN comment for borrower {Payment.AccountNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
        }

        /// <summary>
        /// Create a hold on the account in LC18
        /// </summary>
        private void CreateHold(int addDays, DateTime baseDate)
        {
            RI.FastPath("LC18C" + Payment.SSN);

            //if (Payment.Type == ArrangementType.Setup)
            //    RI.PutText(14, 72, "Y");

            RI.PutText(15, 12, baseDate.AddDays(addDays).ToString("MMddyyyy"), true);

            RI.PutText(15, 30, (Payment.Type == ArrangementType.Setup) ? "11" : "14", ReflectionInterface.Key.Enter);

            if (!RI.CheckForText(22, 3, "49000"))
            {
                PLR.AddNotification($"There was an error adding a hold for borrower {Payment.AccountNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok("There was an error adding a hold. The application will now end", "Error Adding Hold");
                EndDllScript();
            }
        }

        /// <summary>
        /// Adds all the data needed to print the letter to a temp file.
        /// </summary>
        /// <returns></returns>
        private string CreateDataLine()
        {
            decimal collectionCost = GetCollectionCost();
            string keyLine = DocumentProcessing.ACSKeyLine(Payment.SSN, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string day = Payment.DueDate.Value.Day.ToString();
            if (day == "1")
                day += "st";
            else if (day == "7" || day == "15")
                day += "th";
            else
                day += "nd";
            string result = string.Join(",", keyLine, Payment.AccountNumber,
                Payment.FirstName, Payment.LastName, Payment.Address1, Payment.Address2, Payment.City, Payment.State, Payment.Zip, "",
                DateTime.Now.AddDays(60).ToString("MMddyyyy").Insert(2, "/").Insert(5, "/"), Payment.Amount.ToString().Replace(",", ""),
                Payment.DueDate.Value.ToString("MMddyyyy").Insert(2, "/").Insert(5, "/"), day, collectionCost.ToString("N").Replace(",", ""),
                DateTime.Now.Date.ToString("MMddyyyy").Insert(2, "/").Insert(5, "/"));
            return result;
        }

        /// <summary>
        /// Get the Total collection costs from LC10 and multiply it by 16%
        /// </summary>
        /// <returns></returns>
        private decimal GetCollectionCost()
        {
            RI.FastPath("LC10I" + Payment.SSN);
            string prinAmount = RI.GetText(8, 70, 10);
            string interestAmount = RI.GetText(9, 70, 10);
            decimal payAmount = (prinAmount.ToDecimal() + interestAmount.ToDecimal()) * .16m;
            return payAmount;
        }

        /// <summary>
        /// Creates a payment arrangement in LC34
        /// </summary>
        private void CreatePaymentArrangement()
        {
            RI.FastPath(string.Format("LC34C{0};01", Payment.SSN));
            RI.PutText(4, 10, "11");
            RI.PutText(4, 21, string.Format("{0:N}", Payment.Amount), true);
            string date = GetNextDueDate(14);
            RI.PutText(4, 42, date);
            RI.PutText(2, 56, "Y", ReflectionInterface.Key.Enter);
            if (!RI.CheckForText(22, 3, "49233"))
            {
                PLR.AddNotification($"There was an error creating a payment arrangement in LC34 for borrower {Payment.AccountNumber} Error: {RI.GetText(22, 3, 75)}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok(string.Format("There was an error creating a payment arrangement in LC34. The application will now end. \n\n {0}", RI.GetText(22, 3, 75)), "Error Creating Payment");
                EndDllScript();
            }
        }

        /// <summary>
        /// Figures out the next date a payment can be made
        /// </summary>
        /// <returns></returns>
        private string GetNextDueDate(int days)
        {
            DateTime date14 = DateTime.Now.AddDays(days); //Add days for billing
            DateTime selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Payment.DueDay);
            selectedDate = selectedDate <= DateTime.Now ? selectedDate.AddMonths(1) : selectedDate;
            if (date14 >= selectedDate) //If the selected day falls short of the 14 day billing cycle, go out another month.
                selectedDate = selectedDate.AddMonths(1);
            Payment.DueDate = selectedDate;
            return selectedDate.ToString("MMddyyyy");
        }
    }
}
