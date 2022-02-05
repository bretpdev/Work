using Q;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace OPSCBPFED
{
    public class OPSCBPFED : FedBatchScript
    {
        public BorrowerData borrowerData { get; set; }
        const string LOG_FILE_NAME = "OPSCBPFED Log.txt";
        public static string LogFilePathAndName { get; set; }
        /// <summary>
        /// Constructor for processing from DUDE.
        /// </summary>
        /// <param name="ri"></param>
        /// <param name="tMDBorrower"></param>
        /// <param name="tRunNumber"></param>
        public OPSCBPFED(Uheaa.Common.Scripts.ReflectionInterface ri, BorrowerData tMDBorrower)
            : base(ri, "OPSCBPFED", null, null, new List<string>())
        {
            borrowerData = tMDBorrower;
        }

        public override void Main()
        {
            LogFilePathAndName = string.Format("{0}{1}", Uheaa.Common.DataAccess.EnterpriseFileSystem.TempFolder, LOG_FILE_NAME);
            OPSEntry data = new OPSEntry();
            data.CalledByDUDE = true;
            data.AccountNumber = borrowerData.AccountNumber;
            data.AccountHolderName = string.Format("{0} {1}", borrowerData.FirstName, borrowerData.LastName);
            data.TS24SSN = borrowerData.Ssn;
            data.EmailAddress = borrowerData.Demos.Email;
            data.DaysDelinquent = borrowerData.DaysDelq;
            data.LoanPrograms = borrowerData.LoanPrograms;
            data.TotalBalance = borrowerData.TotalBalance.ToString();

            //populate from homepage specific information
            if (borrowerData.ScriptInfoToGenericBusinessUnit is MDScriptInfoSpecificToCustomerService)
                ProcessBorrowerService(data);
            else
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "The OPS Check By Phone script can't handle calls from the homepage you used.  Please either rerun the script from one of the other homepages or call Systems Support for help.", NotificationType.ErrorReport, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());

            //display the entry form
            LaunchForm(data);

            borrowerData.ScriptInfoToGenericBusinessUnit.ScriptCompletedSuccessfully = true;
            ProcessingComplete();
        }

        private void LaunchForm(OPSEntry data)
        {
            bool validSSNOnCompassEntered = false; //flag to stop the looping
            using (CheckByPhoneEntry entryForm = new CheckByPhoneEntry(data)) //create entry form
            {
                while (validSSNOnCompassEntered == false)
                {
                    if (entryForm.ShowDialog() != DialogResult.OK)
                    {
                        if (File.Exists(LogFilePathAndName))
                            File.Delete(LogFilePathAndName);
                        EndDllScript();
                    }
                    //if the user chose to continue, give user confirmation form

                    using (Confirmation conf = new Confirmation(data))
                    {
                        if (conf.ShowDialog() != DialogResult.OK)
                        {
                            if (File.Exists(LogFilePathAndName))
                                File.Delete(LogFilePathAndName);
                            EndDllScript();
                        }
                    }
                    CheckByPhoneProcessor processor = new CheckByPhoneProcessor(RI, data);
                    validSSNOnCompassEntered = processor.Process();
                }
            }
        }

        /// <summary>
        /// Use an AuxiliaryServices Object to populate our OPSEntry object
        /// </summary>
        /// <param name="data">OPSEntry object</param>
        private void ProcessBorrowerService(OPSEntry data)
        {
            //call came from borrower services
            MDScriptInfoSpecificToCustomerService MDScriptInfo = (MDScriptInfoSpecificToCustomerService)borrowerData.ScriptInfoToGenericBusinessUnit;
            try
            {
                data.RPF = DataAccess.PullRPF(data);
            }
            catch (Exception ex)
            {
                data.RPF = "0.00";
            }
            data.AppendToTotalAmountDue = (MDScriptInfo.CurrentAmountDue + MDScriptInfo.OutstandingLateFees + borrowerData.AmountPastDue + double.Parse(data.RPF)).ToString("$#,###,##0.00");
            data.AppendToMonthlyAmountDue = (MDScriptInfo.MonthlyPaymentAmount).ToString("$#,###,##0.00");
            data.PaymentAmount = data.AppendToTotalAmountDue.Replace("$", "");
            if (MDScriptInfo.HasRepaymentSchedule == "N") //Check for repayment schedule.  If none then ask user if they want to proceed
            {
                if (MessageBox.Show("No active repayment schedule.  Do you want to continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    if (File.Exists(LogFilePathAndName))
                        File.Delete(LogFilePathAndName);
                    ProcessingComplete();
                }
            }
        }
    }
}