using Q;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace OPSCHKPHN
{
	public class OPSCKPHN : BatchScript
	{
		public BorrowerData BorrowerData { get; set; }
		const string LOG_FILE_NAME = "OPSCBP Log.txt";
		public static string LogFilePathAndName { get; set; }

		/// <summary>
		/// Constructor for processing from DUDE.
		/// </summary>
		/// <param name="ri"></param>
		/// <param name="tMDBorrower"></param>
		/// <param name="tRunNumber"></param>
		public OPSCKPHN(Uheaa.Common.Scripts.ReflectionInterface ri, BorrowerData tMDBorrower)
			: base(ri, "OPSCHKPHN", null, null, new List<string>(), DataAccessHelper.Region.Uheaa)
		{
			BorrowerData = tMDBorrower;
		}

		public override void Main()
		{
			LogFilePathAndName = string.Format("{0}{1}", Uheaa.Common.DataAccess.EnterpriseFileSystem.TempFolder, LOG_FILE_NAME);
			//check if the script can be run right now
			if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
			{
				MessageBox.Show("It's a weekend.  You must enter the payment directly into OPS.");
				ProcessingComplete();
			}
			OPSEntry data = new OPSEntry();
			data.CalledByDUDE = true;
			data.AccNumOrSSN = BorrowerData.AccountNumber;
			data.AccountHolderName = string.Format("{0} {1}", BorrowerData.FirstName, BorrowerData.LastName);
			data.TS24SSN = BorrowerData.Ssn;
			//populate from homepage specific information
			if (BorrowerData.ScriptInfoToGenericBusinessUnit is MDScriptInfoSpecificToCustomerService)
				ProcessBorrowerService(data);
			else if (BorrowerData.ScriptInfoToGenericBusinessUnit is MDScriptInfoSpecificToAuxiliaryServices)
				ProcessAuxService(data);
			else
				ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "The OPS Check By Phone script can't handle calls from the homepage you used.  Please either rerun the script from one of the other homepages or call Systems Support for help.", NotificationType.ErrorReport, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());

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
			BorrowerData.ScriptInfoToGenericBusinessUnit.ScriptCompletedSuccessfully = true;
			ProcessingComplete();
		}

		/// <summary>
		/// Use an AuxiliaryServices Object to populate our OPSEntry object
		/// </summary>
		/// <param name="data">OPSEntry object</param>
		private void ProcessAuxService(OPSEntry data)
		{
			//call came from aux services
			MDScriptInfoSpecificToAuxiliaryServices MDScriptInfo = (MDScriptInfoSpecificToAuxiliaryServices)BorrowerData.ScriptInfoToGenericBusinessUnit;
			try
			{
				data.RPF = DataAccess.PullRPF(data);
			}
			catch (Exception ex)
			{
				data.RPF = "0.00";
			}
			data.AppendToTotalAmountDue = (MDScriptInfo.CurrentAmountDue + MDScriptInfo.OutstandingLateFees + BorrowerData.AmountPastDue + double.Parse(data.RPF)).ToString("$#,###,##0.00");
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

		/// <summary>
		/// Use an AuxiliaryServices Object to populate our OPSEntry object
		/// </summary>
		/// <param name="data">OPSEntry object</param>
		private void ProcessBorrowerService(OPSEntry data)
		{
			//call came from borrower services
			MDScriptInfoSpecificToCustomerService MDScriptInfo = (MDScriptInfoSpecificToCustomerService)BorrowerData.ScriptInfoToGenericBusinessUnit;
			try
			{
				data.RPF = DataAccess.PullRPF(data);
			}
			catch(Exception ex)
			{
				data.RPF = "0.00";
			}
			data.AppendToTotalAmountDue = (MDScriptInfo.CurrentAmountDue + MDScriptInfo.OutstandingLateFees + BorrowerData.AmountPastDue + double.Parse(data.RPF)).ToString("$#,###,##0.00");
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
