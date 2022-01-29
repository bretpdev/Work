using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA80Reconciliation.Processors;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;
using System.Reflection;

namespace EA80Reconciliation
{
    class EA80Reconciliation
    {
		public List<string> EA27FileLines { get; set; }
		public List<string> EA27SSNs { get; set; }
		public List<string> EA80FileLines { get; set; }
		public List<string> EA80List { get; set; }
		public List<EA80DataRow> EA80Records { get; set; }
		public List<string> EA80IndexPath { get; set; }
		public string DealId { get; set; }
		public ProcessLogData LogData { get; set; }

		public EA80Reconciliation(ProcessLogData logData) 
		{
			LogData = logData;
			EA27FileLines = new List<string>();
			EA27SSNs = new List<string>();
			EA80FileLines = new List<string>();
			EA80List = new List<string>();
			EA80Records = new List<EA80DataRow>();
			EA80IndexPath = new List<string>();
		}
        /// <summary>
        /// Compare the values in the EA27 file with the file names in the EA80 Folder
        /// </summary>
		public void Reconcile(string ea27Location, string ea80FolderLocation, DateTime loanSaleDate) 
        {
            Results.Clear();
			EA27SSNs = ParseEA27File(ea27Location); //Parse EA27 file into a list of socials with a dealId
			EA80Records = ParseEA80File(ea80FolderLocation); //Pull all TAXX records out of the EA80 index files.
			InEA27MissingEA80(loanSaleDate); //send discrepancies to process logger for EA27 exists EA80 doesnt
			InEA80MissingEA27(loanSaleDate); //send discrepancies to process logger for EA80 exists EA27 doesnt
			CompareEA80RecordsToImages(); //send discrepanices to process logger for EA80 records without matching images.
			SummaryReport(); //Log summary
        }

		/// <summary>
		/// Checks the EA80List (file directory) for documents that match the document name in the EA80Records list (TAXX records from index file). Logs a discrepancy for any records without a document.
		/// </summary>
		private void CompareEA80RecordsToImages()
		{
			//Find image in EA80 folder
			Results.LogNotification("Matching up EA80 Images to Index File...");
			foreach (EA80DataRow record in EA80Records)
			{
				if (EA80List.Where(r => r.Contains(record.DocName)).Any()) //Match found
					continue; //valid record
				else
				{
					Results.LogNotification(string.Format("DISCREPANCY: Document {0} does not exist for SSN {1} from EA80 Index file {2}.", record.DocName, record.SSN, record.FileName));
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("DISCREPANCY: Document {0} does not exist for SSN {1} from EA80 Index file {2}.", record.DocName, record.SSN, record.FileName), NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
				}
			}
			Results.LogNotification("Comparing EA80 Images to Index File Completed.");
		}

		/// <summary>
		/// Logs results of EA80 records missing EA27 matches based on the loan sale date
		/// </summary>
		/// <param name="loanSaleDate">Date selected in UI</param>
		private void InEA80MissingEA27(DateTime loanSaleDate)
		{
			//Files in EA80 but not in EA27
			foreach (EA80DataRow record in EA80Records.Where(r => (loanSaleDate - r.DocDate.ToDate()).TotalDays <= 30))
			{
				if (EA27SSNs.Contains(record.SSN)) //Match found
					continue; //valid record
				else
				{
					Results.LogNotification(string.Format("DISCREPANCY: SSN {0} in EA80 index file {1} does not have a corresponding EA27 record.", record.SSN, record.FileName));
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("DISCREPANCY: SSN {0} in EA80 index file {1} does not have a corresponding EA27 record.", record.SSN, record.FileName), NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
				}
			}
			Results.LogNotification("EA27 and EA80 SSN Compare Completed.");
		}

		/// <summary>
		/// Logs results of EA27 records missing EA80 matches based on the loan sale date
		/// </summary>
		/// <param name="loanSaleDate">Date selected in UI</param>
		private void InEA27MissingEA80(DateTime loanSaleDate)
		{
			//Files in EA27 but dont have a record in EA80 meeting the criteria
			//Make sure the date of the doc is within 30 days of loan sale date
			Results.LogNotification("Comparing EA27 and EA80 SSNs...");
			foreach (string ssn in EA27SSNs)
			{
				if (EA80Records.Where(r => r.SSN == ssn && (loanSaleDate - r.DocDate.ToDate()).TotalDays <= 30).Any()) //Match found
					continue; //valid record
				else
				{
					Results.LogNotification(string.Format("DISCREPANCY: SSN {0} in EA27 file does not have a corresponding TAXX record within 30 Days of the Loan Sale Date.", ssn));
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("DISCREPANCY: SSN {0} in EA27 file does not have a corresponding TAXX record within 30 Days of the Loan Sale Date.", ssn), NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
				}
			}
		}

		/// <summary>
		/// Summarizes overall results to the results window and process logger.
		/// </summary>
		private void SummaryReport()
		{
			Results.LogNotification("Summary:");
			Results.LogNotification(string.Format("EA27 Borrower Count: {0}", EA27SSNs.Count));
			Results.LogNotification(string.Format("EA80 Borrower Count: {0}", EA80Records.Count));
			Results.LogNotification(string.Format("EA27 Deal ID: {0}", DealId));
			ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("EA27 Borrower Count: {0}", EA27SSNs.Count), NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
			ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("EA80 Borrower Count: {0}", EA80Records.Count), NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
			ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("EA27 Deal ID: {0}", DealId), NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
		}

		/// <summary>
		/// Adds all TAXX records for each EA80 index file at EA80FolderLocation to EA80Records.
		/// </summary>
		/// <param name="ea80FolderLocation">File path</param>
		/// <returns>list of TAXX data records</returns>
		private List<EA80DataRow> ParseEA80File(string ea80FolderLocation)
		{
			List<EA80DataRow> records = new List<EA80DataRow>();
			EA80List = Directory.GetFiles(ea80FolderLocation + "\\", "*.txt").ToList();
			EA80IndexPath = Directory.GetFiles(ea80FolderLocation + "\\", "*.idx").ToList();
			foreach (string file in EA80IndexPath)
			{
				using (StreamReader sr = new StreamReader(file))
				{
					Results.LogNotification("Reading EA80 Index File...");
					while (!sr.EndOfStream)
						EA80FileLines.Add(sr.ReadLine());
					//Index into record, looking at the 4th element and compare to TAXX.  
					foreach (string record in EA80FileLines.Where(r => r.Split('|')[3] == "TAXX"))
					{
						EA80DataRow data = new EA80DataRow();
						List<string> fields = record.Split('|').ToList();
						data.SSN = fields[0];
						data.LastName = fields[1];
						data.FirstName = fields[2];
						data.DocType = fields[3];
						data.DocName = fields[10];
						data.DocDate = fields[5];
						data.FileName = file;
						records.Add(data); //Add all TAXX Records to my list to compare to the EA27 ssn list
					}
				}
				Results.LogNotification("Reading of EA80 Index File Completed.");
			}
			return records;
		}

		/// <summary>
		/// Takes a location for an ea27 file and parses the ssns out of it into EA27SSNs list.
		/// </summary>
		/// <param name="ea27Location">file path</param>
		/// <returns>list of ssns</returns>
		private List<string> ParseEA27File(string ea27Location)
		{
			List<string> records = new List<string>();
			using (StreamReader sr = new StreamReader(ea27Location))
			{
				Results.LogNotification("Reading EA27 File...");
				string headerRow = sr.ReadLine();
				while (!sr.EndOfStream)
					EA27FileLines.Add(sr.ReadLine());
				if (headerRow.Substring(0, 2) != "D1")
				{
					Results.LogError("DISCREPANCY: No header record found in file {0}.", ea27Location);
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("DISCREPANCY: No header record found in file {0}.", ea27Location), NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly());
				}
				else
				{
					DealId = headerRow.Substring(50, 5);
					foreach (string record in EA27FileLines.Where(r => r.Substring(0, 2) == "01")) //read in record 1's
					{
						records.Add(record.Substring(2, 9));
					}
				}
			}
			Results.LogNotification("Reading of EA27 File Completed.");
			return records;
		}
    }
}
