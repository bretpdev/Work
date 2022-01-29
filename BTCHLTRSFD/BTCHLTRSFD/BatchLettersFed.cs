using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace BTCHLTRSFD
{
	public class BatchLettersFed : FedBatchScript
	{
		public BatchLettersFed(ReflectionInterface ri)
			: base(ri, "BTCHLTRSFD", "ERR_BU35", "EOJ_BU35", new List<string>() { string.Empty })
		{
		}

		public override void Main()
		{
			StartupMessage("This script will produce forbearance, deferment and income sensitive letters and form requested the previous day. Please set the printer to Duplex and click OK to continue, or Cancel to quit.");
			PrintLetters();
			try
			{
				ProcessingComplete();
			}
			catch (EndDLLException) //script completed correctly
			{
				System.Environment.Exit(0);
			}


		}//end main

		/// <summary>
		/// When in recovery will finish processing the printing.
		/// </summary>
		private void FinishRecovery()
		{
			List<string> recoveryValues = Recovery.RecoveryValue.SplitAndRemoveQuotes(",");
			DatabaseData item = DataAccess.GetRecoveryFileData(recoveryValues[2].ToInt());
			string file = recoveryValues[1];
			if (recoveryValues[0] == "ARC")
				AddArc(file, item);
			else
				DoPrinting(file, item, !item.Arc.IsNullOrEmpty());

			File.Delete(file);
		}

		/// <summary>
		/// Generates ecorr letters and sends non ecorr letters to the printer.
		/// </summary>
		/// <param name="file">File to process.</param>
		/// <param name="item">DatabaseData object for letter campaign.</param>
		private void DoPrinting(string file, DatabaseData item, bool addArc)
		{
			if (item.DoNotProcessEcorr)
				DocumentProcessing.CostCenterPrinting(item.LetterId, file, item.StateFieldCodeName, ScriptId, item.AccountNumberFieldName, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, item.CostcenterFieldCodeName);
			else
				EcorrProcessing.EcorrCostCenterPrinting(item.LetterId, file, UserId, item.StateFieldCodeName, ScriptId, item.AccountNumberFieldName, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, ProcessLogData.ProcessLogId, item.BorrowerSsnIndex);
			
			if (!addArc)
				File.Delete(file);
			Recovery.Delete();

		}

		/// <summary>
		/// Gets all the letter records and sends them to cost center printing.
		/// </summary>
		private void PrintLetters()
		{
			if (!Recovery.RecoveryValue.IsNullOrEmpty())
				FinishRecovery();

			List<DatabaseData> data = DataAccess.GetLettersFromDb();
			foreach (DatabaseData item in data)
			{
				List<string> files = new List<string>();

				if (item.ProcessAllFiles)
					files = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, item.SasFilePattern).ToList();
				else
				{
					string file = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, item.SasFilePattern);
					if (!file.IsNullOrEmpty())
						files.Add(file);
				}

				if (!item.OkIfMissing && files.Count == 0)
					ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "The following file was missing: " + item.SasFilePattern, NotificationType.NoFile, NotificationSeverityType.Informational);

				foreach (string file in files)
				{
					if (IsSasFileEmpty(file))
					{
						//If the file is empty then delete the file and continue to the next file.
						File.Delete(file);
						continue;
					}

					DoPrinting(file, item, !item.Arc.IsNullOrEmpty());

					if (!item.Arc.IsNullOrEmpty())
						AddArc(file, item);

					File.Delete(file);
				}
			}
		}

		/// <summary>
		/// Adds a comment to TD22
		/// </summary>
		/// <param name="file">File to process.</param>
		/// <param name="item">Object that has the arc and comment to use.</param>
		private void AddArc(string file, DatabaseData item)
		{
			using (StreamReader sr = new StreamReader(file))
			{
				//Read out the header
				sr.ReadLine();

				int recoverycount = 1;
				if (!Recovery.RecoveryValue.IsNullOrEmpty() && Recovery.RecoveryValue.Contains("ARC"))
				{
					List<string> recoveryValue = Recovery.RecoveryValue.SplitAndRemoveQuotes(",");
					while (recoverycount != recoveryValue[3].ToInt())
					{
						sr.ReadLine();
						recoverycount++;
					}
				}

				for (; !sr.EndOfStream; recoverycount++)
				{
					string accountNumber = sr.ReadLine().SplitAndRemoveQuotes(",")[0];
					ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
					{
						AccountNumber = accountNumber,
						Arc = item.Arc,
						Comment = item.Comment,
						ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
						ScriptId = ScriptId,
						RecipientId = string.Empty
					};
					ArcAddResults result = arc.AddArc();
					if (!result.ArcAdded) 
						ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, result.Errors.Count > 0 ? string.Join("\r\n", result.Errors) : string.Format("Arc '{0}' was not added to the account '{1}' successfully.", arc.Arc, arc.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);

					Recovery.RecoveryValue = string.Format("ARC,{0},{1},{2}", file, item.BatchLettersFedId, recoverycount);
				}
			}
		}

		/// <summary>
		/// Checks to see if a file is empty.  There are some files that generate a header record when they are empty
		/// </summary>
		/// <param name="file">File to Check</param>
		/// <returns>True if the file has more than 2 lines.</returns
		public bool IsSasFileEmpty(string file)
		{
			using (StreamReader sr = new StreamReader(file))
			{
				sr.ReadLine();
				return sr.EndOfStream;  //if we reached end of stream, file is empty.  otherwise, it has more than one line and isn't empty.
			}
		}
	}//end batch letters fed
}
