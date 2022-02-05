using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace CLMPMTPST
{
    public class FileReader
    {
        ProcessLogRun LogRun { get; set; }
		public string Error { get; set; }

        public FileReader(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

		/// <summary>
		/// Reads the SAS file and parses the payment info.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
        public List<Payment> ReadFile(string fileName)
        {
			//Read the contents into a list, noting errors found.
			DataTable sasTable = FileSystemHelper.CreateDataTableFromFile(fileName);
			List<SasFileError> errors = new List<SasFileError>();
			List<Payment> payments = new List<Payment>();
			foreach (DataRow sasRow in sasTable.Rows)
			{
				string ssn = sasRow.Field<string>("BF_SSN");
				string lastName = sasRow.Field<string>("DM_PRS_LST");
				string guarantorCode = sasRow.Field<string>("GUAR_CODE");
				try
				{
					double amount = double.Parse(sasRow.Field<string>("POAMT"));
					DateTime effectiveDate = DateTime.ParseExact(sasRow.Field<string>("EFF_DATE"), Payment.DATE_FORMAT, null);
					List<int> loanSequences = new List<int>();
					foreach (string sequence in sasRow.Field<string>("LN_SEQ").Split(' '))
					{
						loanSequences.Add(int.Parse(sequence));
					}
					payments.Add(new Payment(ssn, amount, effectiveDate, guarantorCode, loanSequences, lastName));
				}
				catch (FormatException ex)
				{
					int lineNumber = sasTable.Rows.IndexOf(sasRow) + 1;
					errors.Add(new SasFileError(lineNumber, ssn, lastName, ex.Message));
					LogRun.AddNotification($"Record could not be read from SAS file {fileName}. Line: {lineNumber}, SSN: {ssn}, Last name: {lastName}, Error: {ex.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
				}
			}

			//See if any errors were found, record them in a single error message (this will be shown to the user)
			if (errors.Count > 0)
			{
				StringBuilder messageBuilder = new StringBuilder();
				messageBuilder.AppendFormat($"Some records could not be read from the {fileName} SAS file:{Environment.NewLine}");
				foreach (SasFileError error in errors)
				{
					messageBuilder.AppendFormat($"Line: {error.LineNumber}, SSN: {error.Ssn}, Last name: {error.LastName}{Environment.NewLine}");
				}
				Error = messageBuilder.ToString();
				IssueNotification(Error);
				LogRun.LogEnd();
				return null;
			}

			return payments;
		}

		/// <summary>
		/// Logs issue and notifies user with a prompt.
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="ex"></param>
		private void IssueNotification(string errorMessage, Exception ex = null)
		{
			LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex); // Log issue
			Dialog.Error.Ok(errorMessage, LppClaimPaymentPosting.ScriptId); // Notify user of issue
		}

	}
}
