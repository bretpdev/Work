using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.IO;
using System.Threading;

namespace EMAILBATCH
{
	public class BatchProcessor : ScriptSessionBase
	{

		private EmailBatchRecord _record;
		private string _fileToProcess;
		private TestModeResults _directories;
		const string LOG_FILE = "EMAIL BATCH LOG.txt";

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ri">Reflection Interface</param>
		/// <param name="record">Record from DB</param>
		/// <param name="ftpFolder">Folder where the SAS files are found</param>
		public BatchProcessor(ReflectionInterface ri, EmailBatchRecord record, TestModeResults directories)
			: base(ri)
		{
			_directories = directories;
			_record = record;
			_fileToProcess = Common.DeleteOldFilesReturnMostCurrent(_directories.FtpFolder, _record.SASFile + "*", Common.FileOptions.ErrorOnEmpty | Common.FileOptions.ErrorOnMissing);
		}

		/// <summary>
		/// Processes all files associated with the DB record.
		/// </summary>
		public void ProcessRecord()
		{
			int emailMessagesSent = 0;
			DynamicMerger merger = new DynamicMerger(TestModeProperty, _fileToProcess, string.Format("{0}{1}", _directories.DocFolder, _record.HTMLFile));
			string rAccountNumber = string.Empty;
			string log = string.Format("{0}{1}", _directories.LogFolder, LOG_FILE);
			//get recovery values if log exists
			if (File.Exists(log))
			{
				//if log file exists get SSN out of it
				VbaStyleFileOpen(log, 5, Common.MSOpenMode.Input);
				rAccountNumber = VbaStyleFileInput(5);
				VbaStyleFileClose(5);
			}
			MergedEmailData data = merger.DoMergeOnNextRecord();
			//recover if there is a recovery SSN
			while (!string.IsNullOrEmpty(rAccountNumber) && data != null && rAccountNumber != data.AccountNumber)
			{
				data = merger.DoMergeOnNextRecord();
				emailMessagesSent++;
			}
			while (data != null)
			{
				if (TestModeProperty)
				{
					//if in test mode switch recipient to tester email
					data.Body = string.Format("If this email was sent in a production environment it would have been sent to {0}. {1}{1}{2}", data.Recipient, Environment.NewLine, data.Body);
					data.Recipient = string.Format("{0}@utahsbr.edu", WindowsUserName());
				}
				//Before sending the first real e-mail, send a sample copy to Systems Support.
				if (emailMessagesSent == 0)
				{
					string subject = "[Batch E-mail: New campaign starting] " + _record.SubjectLine;
					SendMail(TestModeProperty, "sshelp@utahsbr.edu", _record.SendingAddress, _record.SubjectLine, data.Body, string.Empty, string.Empty, string.Empty, Common.EmailImportanceLevel.Normal, true, true);
				}
				//send email
				SendMail(TestModeProperty, data.Recipient, _record.SendingAddress, _record.SubjectLine, data.Body, string.Empty, string.Empty, string.Empty, Common.EmailImportanceLevel.Normal, true, true);
				//do activity comments
				if (_record.ActionCode != null)
				{
					//OneLINK
					BorrowerDemographics demos = GetDemographicsFromLP22(data.AccountNumber);
					AddCommentInLP50(demos.SSN, _record.ActionCode, "EMAILBATCH", "EM", "10", _record.CommentText, false, false);
				}
				if (_record.ARC != null)
				{
					//COMPASS
					BorrowerDemographics demos = GetDemographicsFromTX1J(data.AccountNumber);
					if (ATD22ByBalance(demos.SSN, _record.ARC, _record.CommentText, "EMAILBATCH", false) == false)
					{
						ATD37FirstLoan(demos.SSN, _record.ARC, _record.CommentText, "EMAILBATCH", false);
					}
				}
				//write out for recovery
				VbaStyleFileOpen(log, 5, Common.MSOpenMode.Output);
				VbaStyleFileWrite(5, data.AccountNumber);
				VbaStyleFileClose(5);
				//do next record
				data = merger.DoMergeOnNextRecord();
				emailMessagesSent++;
				//sleep for one minute after 5000 email messages are sent
				if (emailMessagesSent % 5000 == 0) { Thread.Sleep(new TimeSpan(0, 1, 0)); }
			}
			merger.CloseFile();
			if (File.Exists(_fileToProcess)) { File.Delete(_fileToProcess); }
			if (File.Exists(log)) { File.Delete(log); }
		}
	}
}
