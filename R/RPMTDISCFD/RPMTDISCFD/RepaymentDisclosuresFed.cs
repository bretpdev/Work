using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using System.Threading.Tasks;
using System.Data;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Windows.Forms;

namespace RPMTDISCFD
{
	public class RepaymentDisclosuresFed : FedBatchScript
	{
		private const string SasFilePattern = "UNWS01.NWS01{0}";
		private const string EOJ_Total = "Total number of records to process";
		private const string EOJ_Success = "Total number of successful records processed";
		private const string ERR_ErrorAddingComment = "Error adding comment from file";
		private const string ERR_IncorrectDataInFile = "There was incorrect or missing data in the file";
		private const string ERR_EmptyR2File = "The R2 file is empty";
		private const string ERR_EmptyR3File = "The R3 file is empty";
		private static readonly string[] EojFields = { EOJ_Total, EOJ_Success, ERR_ErrorAddingComment, ERR_IncorrectDataInFile, ERR_EmptyR2File, ERR_EmptyR3File };

		public RepaymentDisclosuresFed(ReflectionInterface ri)
			: base(ri, "RPMTDISCFD", "ERR_BU35", "EOJ_BU35", EojFields)
		{
			//DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
		}

        private DataAccess da;
		public override void Main()
		{
            this.da = new DataAccess(ProcessLogData.ProcessLogId);
            RI.LogOut();
			List<string> logFileToDelete = new List<string>(Directory.GetFiles(EnterpriseFileSystem.FtpFolder, string.Format(SasFilePattern, "R1")));

			foreach (string logFile in logFileToDelete)//Delete the R1 file we do not process this file
				File.Delete(logFile);

            if (Recovery.RecoveryValue.IsPopulated())
                if (MessageBox.Show("There is a recovyer file. Do you want to recover?", "In Recovery", MessageBoxButtons.YesNo) == DialogResult.No)
                    Recovery.RecoveryValue = null;

			List<string> files = new List<string>();

			files.AddRange(GetFileList());

			ProcessFile(files);

			ProcessingComplete();
		}

		/// <summary>
		/// Finds all the R2 and R3 files to process.
		/// </summary>
		/// <returns>List of all the files found.</returns>
		private List<string> GetFileList()
		{
			List<string> files = new List<string>();
			List<string> r2Files = new List<string>(Directory.GetFiles(EnterpriseFileSystem.FtpFolder, string.Format(SasFilePattern, "R2.*")));
			List<string> r3Files = new List<string>(Directory.GetFiles(EnterpriseFileSystem.FtpFolder, string.Format(SasFilePattern, "R3.*")));
			int count = files.Count;
			if (!Recovery.RecoveryValue.Contains("COMPLETE"))
			{
				if (r2Files.Count > 0)
				{
					foreach (string item in r2Files)
						if (new FileInfo(item).Length > 0)
							files.Add(item);
						else
							FileHelper.DeleteEmptyFile(item, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
				}
				else //The R2 file is missing
				{
					ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "The R2 file is missing", NotificationType.NoFile, NotificationSeverityType.Warning);
				}
				if (r2Files.Count > 0 && files.Count == 0) //The R2 file is empty
				{
					Eoj.Counts[ERR_EmptyR2File].Increment();
					ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, ERR_EmptyR2File, NotificationType.EmptyFile, NotificationSeverityType.Warning);
				}
			}

			if (r3Files.Count > 0)
			{
				foreach (string item in r3Files)
					if (new FileInfo(item).Length > 0)
						files.Add(item);
					else
						FileHelper.DeleteEmptyFile(item, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
			}
			else //The R3 file is missing
			{
				ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "The R3 file is missing", NotificationType.NoFile, NotificationSeverityType.Warning);
			}
			if (r3Files.Count > 0 && (files.Count - count) == 0) //The R3 file is empty
			{
				Eoj.Counts[ERR_EmptyR3File].Increment();
				ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, ERR_EmptyR3File, NotificationType.EmptyFile, NotificationSeverityType.Warning);
			}

			if (CheckMissingFiles(r2Files, r3Files))
				return new List<string>();

			return files;
		}

		/// <summary>
		/// Checks to make sure there are files to process. If a file is missing, it notifies the user and ends the script.
		/// </summary>
		private bool CheckMissingFiles(List<string> r2Files, List<string> r3Files)
		{
			bool filesMissing = false;
			string message = "";
			if (r2Files.Count == 0 && !Recovery.RecoveryValue.Contains("COMPLETE"))
			{
				message += "R2\r\n";
				filesMissing = true;
			}
			if (r3Files.Count == 0)
			{
				message += "R3";
				filesMissing = true;
			}
			if (filesMissing)
				Dialog.Error.Ok(string.Format("The following file(s) were missing:\r\n\r\n{0}\r\n\r\nMake sure both files are available and try again", message), "Missing Files");
			return filesMissing;
		}

		/// <summary>
		/// Process each file
		/// </summary>
		/// <param name="files">The list of files found in the FTP folder</param>
		private void ProcessFile(List<string> files)
		{
			foreach (string file in files)
			{
				int stateMailCount = 0;
				string coverSheetDataFile = string.Format("{0}Cover{1}", EnterpriseFileSystem.TempFolder, Path.GetFileName(file));
				List<SasFileData> sData = GroupLoanSeq(file);
				foreach (SasFileData data in sData)
				{
					data.EcorrInfo = EcorrProcessing.CheckEcorr(data.AccountNumber);
					data.DoEcorr = (data.EcorrInfo != null && data.EcorrInfo.LetterIndicator && data.EcorrInfo.ValidEmail);

					if (!data.DoEcorr)
					{
						stateMailCount++;
						//write out the line to the coversheet data file for state mail cover sheet
						if (!File.Exists(coverSheetDataFile))
						{
							File.WriteAllText(coverSheetDataFile, data.Header);
							File.AppendAllText(coverSheetDataFile, Environment.NewLine);
							File.AppendAllText(coverSheetDataFile, data.LineData);
						}
						else
						{
							File.AppendAllText(coverSheetDataFile, Environment.NewLine);
							File.AppendAllText(coverSheetDataFile, data.LineData);
						}
					}
				}
				//print state mail cover sheet here
				string dataFile = string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, Path.GetFileName(file));
				//Copy the file to the T: drive. The new file will have all the merge fields added for printing
				File.Copy(file, dataFile, true);
				string letterId = file.ToUpper().Contains("R2") ? "RPDISCFED" : file.ToUpper().Contains("R3") ? "PLRPYMTFED" : "UNKNOWN LETTER ID";
				if (stateMailCount != 0)
				{
					string barcodedData = DocumentProcessing.AddBarcodesForBatchProcessing(coverSheetDataFile, "DF_SPE_ACC_ID", letterId, true, DocumentProcessing.LetterRecipient.Borrower);
					DocumentProcessing.PrintFederalStateMailCoverSheet(barcodedData, letterId, "STATE_IND");
					FileHelper.DeleteFile(coverSheetDataFile, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
				}

				//Recovery will be updated after the printing is complete so if there is ever anything in the recovery value we know that printing completed
				if (Recovery.RecoveryValue.IsNullOrEmpty() || (Recovery.RecoveryValue.Contains("COMPLETE") && Recovery.RecoveryValue.Contains("R2")))
					PrintLetters(dataFile, file, sData, letterId);
				//If recovery is not null, check to make sure the file is the same date
				if (Recovery.RecoveryValue.Contains("PRINT"))
					ImageLetters(dataFile, file, letterId);
				//If recovery is not null, check to make sure the file is the same date
				if (Recovery.RecoveryValue.Contains("IMGDONE") || Recovery.RecoveryValue.Contains("ARC"))
					CreateArcs(file, sData);

				//Delete the file when finished
				if (File.Exists(file))
					FileHelper.DeleteFile(file, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
			}
		}

		/// <summary>
		/// Prints and images all the disclosures at once
		/// </summary>
		/// <param name="dataFile">The location where file will be copied to for processing</param>
		/// <param name="file">The location of the actual file to process</param>
		private void PrintLetters(string dataFile, string file, List<SasFileData> sData, string letterId)
		{
			foreach (SasFileData data in sData)
			{
				GenerateDocument(data, data.EcorrInfo, data.DoEcorr, ScriptId, "UT00801", letterId, dataFile);
			}

			foreach (string fileName in Directory.GetFiles(EnterpriseFileSystem.TempFolder, "RPDISCFED*"))
			{
				//Delete Temp
				FileHelper.DeleteFile(fileName, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
			}
			foreach (string fileName in Directory.GetFiles(EnterpriseFileSystem.TempFolder, "PLRPYMTFED*"))
			{
				//Delete Temp
				FileHelper.DeleteFile(fileName, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
			}
			foreach (string fileName in Directory.GetFiles(EnterpriseFileSystem.TempFolder, "Add Return Mail Barcode Temp*"))
			{
				FileHelper.DeleteFile(fileName, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
			}
			Recovery.RecoveryValue = string.Format("0,{0},PRINT", file); //When recovering, use the original file
			//Delete the new file but keep the original to add arcs
			FileHelper.DeleteFile(dataFile, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
		}

		/// <summary>
		/// Generates either a word doc or pdf and images it based on whether the account is setup for ecorr or not.
		/// </summary>
		/// <param name="data">SasFileData object containing information about a single borrower(possibly multiple accounts)</param>
		/// <param name="ecorrInfo">EcorrData object.  Contains flags about whether an account is on ecorr</param>
		/// <param name="doEcorr">True or false</param>
		/// <param name="scriptId"></param>
		/// <param name="userId"></param>
		/// <param name="letterId">"RPDISCFED" or "PLRPYMTFED"</param>
		/// <param name="directory">Location of where to save the documents</param>
		private void GenerateDocument(SasFileData data, EcorrData ecorrInfo, bool doEcorr, string scriptId, string userId, string letterId, string directory)
		{
			string ssn = da.GetSsnFromFromAcctNo(data.AccountNumber).Result;
			if (doEcorr)
			{
				string fileName = GenerateEcorr(data, UserId, ecorrInfo, letterId, ssn, doEcorr);
			}
			else
			{
				var t1 = Task.Factory.StartNew(() => GenerateEcorr(data, userId, ecorrInfo, letterId, ssn, doEcorr), TaskCreationOptions.LongRunning);
				var t2 = Task.Factory.StartNew(() => GeneratePrinted(data, scriptId, letterId, ssn), TaskCreationOptions.LongRunning);

				Task.WhenAll(t1, t2).Wait();
			}
		}

		/// <summary>
		/// Creates a Word doc for the sasFileData object and images it.
		/// </summary>
		/// <param name="data">Object containing the account number and the linedata</param>
		/// <param name="ScriptId"></param>
		/// <param name="letterId">"RPDISCFED" or "PLRPYMTFED"</param>
		/// <param name="ssn">ssn for account</param>
		private void GeneratePrinted(SasFileData data, string ScriptId, string letterId, string ssn)
		{
			string dataFile = EnterpriseFileSystem.TempFolder + "mergeFile.txt";
			File.WriteAllLines(dataFile, new List<string>() { data.Header, data.LineData });
			string directory = EnterpriseFileSystem.TempFolder;
			string imagingFile = string.Format("{0}{1}_{2}_{3}_{4}.doc", directory, ScriptId, letterId, data.AccountNumber, Guid.NewGuid().ToBase64String());
			DocumentProcessing.AddBarcodesForBatchProcessing(dataFile, "DF_SPE_ACC_ID", letterId, false, DocumentProcessing.LetterRecipient.Borrower);
			DocumentProcessing.SaveDocs(letterId, dataFile, imagingFile);
			Word.Application DocPrint = new Word.Application();
			DocPrint.Visible = false;
			DocPrint.Documents.Add();
			foreach (string fileName in Directory.GetFiles(EnterpriseFileSystem.TempFolder, "RPMTDISCFD*"))
			{
				//Print word docs
				DocPrint.PrintOut(false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, fileName);
				FileHelper.DeleteFile(fileName, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
			}
			DocPrint.Application.Quit();

			FileHelper.DeleteFile(dataFile, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
		}

		/// <summary>
		/// Creates a PDF for the sasFileData object.
		/// </summary>
		/// <param name="data">Object containing the account number and the linedata</param>
		/// <param name="UserId"></param>
		/// <param name="ecorrInfo">EcorrData object.  Contains flags about whether an account is on ecorr</param>
		/// <param name="letterId">"RPDISCFED" or "PLRPYMTFED"</param>
		/// <param name="ssn">ssn for account</param>
		/// <returns>Name of the document</returns>
		private string GenerateEcorr(SasFileData data, string UserId, EcorrData ecorrInfo, string letterId, string ssn, bool doEcorr)
		{
			List<string> fields = data.LineData.SplitAndRemoveQuotes(",");
			List<string> addressFields = new List<string>() { fields[3], fields[4], fields[5], fields[6] + " " + fields[7] + " " + fields[8], fields[9] };
			DataTable loanDetail = GetLoanDetail(data.LineData, letterId);
			Dictionary<string, string> formFields = GetFormFields(data.LineData);
			var docProperty = doEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed;
			return PdfHelper.GenerateEcorrPdf(Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), string.Format("{0}.pdf", letterId)), data.AccountNumber, ssn, docProperty, UserId, ecorrInfo, addressFields, loanDetail, formFields);
		}

		/// <summary>
		/// Images all the letters in the sas file
		/// </summary>
		/// <param name="dataFile">The location where file will be copied to for processing</param>
		/// <param name="file">The location of the actual file to process</param>
		private void ImageLetters(string dataFile, string file, string letterId)
		{
			ImagingGenerator img = new ImagingGenerator(ScriptId, UserId);
			//Copy the file to the T: drive. The new file will have all the merge fields added for printing
			File.Copy(file, dataFile, true);
			DocumentProcessing.ImageDocs(ScriptId, "DF_SPE_ACC_ID", "RPDIS", letterId, dataFile, DocumentProcessing.LetterRecipient.Borrower, img);

			Recovery.RecoveryValue = string.Format("0,{0},IMGDONE", file); //When recovering, use the original file
			//Delete the new file but keep the original to add arcs
			FileHelper.DeleteFile(dataFile, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
		}

		/// <summary>
		/// Creates an ARC for each borrower in the file
		/// </summary>
		/// <param name="fileName">The current file being processed</param>
		private void CreateArcs(string fileName, List<SasFileData> sData)
		{
			RI.Login(UserId, da.GetPassword(UserId).Result, DataAccessHelper.Region.CornerStone);
			if (CheckForText(20, 8, "USERID==>"))
				NotifyAndEnd("Script could not login into Reflection Session please investigate and try again");
			int recoveryIndex = Recovery.RecoveryValue.IsPopulated() ? int.Parse(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0]) : 0;
			foreach (SasFileData data in sData.Skip(recoveryIndex))
			{
				Eoj.Counts[EOJ_Total].Increment();
				string docType = data.DoEcorr ? "ECORR" : "PRINTED";
				if (RI.Atd22ByLoan(data.AccountNumber, "MSRPD", string.Format("{0} REPAYMENT DISCLOSURE SENT TO BORROWER", docType), "", data.LoanSeqs, ScriptId, false))
					Eoj.Counts[EOJ_Success].Increment();
				else
				{
					string message = string.Format("{0} for borrower: {1}, File: {2}", ERR_ErrorAddingComment, data.AccountNumber, fileName);
					ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
					Err.AddRecord(ERR_ErrorAddingComment, data);
					Eoj.Counts[ERR_ErrorAddingComment].Increment();
				}
				//update recovery the recoveryIndex will be used as a counter to recover from
				Recovery.RecoveryValue = string.Format("{0},{1},ARC", ++recoveryIndex, fileName);
			}
			if (fileName.Contains("R2"))
				Recovery.RecoveryValue = string.Format("0,{0},COMPLETE", fileName);
			else
				Recovery.Delete(); //R3 is done.
            RI.LogOut();
		}

		/// <summary>
		/// adds rows to create a DataTable used for the loan detail sheet
		/// </summary>
		/// <param name="dataLine">line from the file</param>
		/// <param name="letterid">determine the columns</param>
		/// <returns>DataTable</returns>
		private static DataTable GetLoanDetail(string dataLine, string letterid)
		{
			DataTable dt = new DataTable();
			if (letterid == "RPDISCFED")
			{
				dt.Columns.AddRange(new DataColumn[]
				{
					new DataColumn("First Disbursement Date"),
					new DataColumn("Loan Sequence #"),
					new DataColumn("Date Entered Repayment"),
					new DataColumn("Estimated Unpaid Principal Balance"),
					new DataColumn("Estimated Unpaid Interest")
				});
			}
			else if (letterid == "PLRPYMTFED")
			{
				dt.Columns.AddRange(new DataColumn[]
				{
					new DataColumn("First Disbursement Date"),
					new DataColumn("Loan Sequence #"),
					new DataColumn("Date Entered Repayment"),
					new DataColumn("Estimated Unpaid Principal Balance"),
					new DataColumn("Estimated Unpaid Interest"),
					new DataColumn("Deferment End Date")
				});
			}
			else
			{
				throw new Exception(string.Format("Unrecognized LetterID {0}", letterid));
			}
			List<string> fields = dataLine.SplitAndRemoveQuotes(",");
			if (letterid == "RPDISCFED")
			{
				for (int i = 0; i < 36; i++)
				{
					string[] dataRow = new string[]
					{
						fields[65+i], //First Disbursement Date (LD_LON_1_DSB)
						fields[101+i], //Loan Sequence # (LN_SEQ)
						fields[137+i], //Date Entered Repayment (WD_LON_RPD_SR)
						fields[173+i], //Estimated Unpaid Principal Balance (LA_CPI_RPD_DIS)
						fields[209+i]  //Estimated Unpaid Interest (LA_ACR_INT_RPD)
					};
					dt.Rows.Add(dataRow);
				}
			}
			else if (letterid == "PLRPYMTFED")
			{
				for (int i = 0; i < 36; i++)
				{
					string[] dataRow = new string[]
					{
					    fields[65+i], //First Disbursement Date (LD_LON_1_DSB)
					    fields[101+i], //Loan Sequence # (LN_SEQ)
					    fields[137+i], //Date Entered Repayment (WD_LON_RPD_SR)
					    fields[173+i], //Estimated Unpaid Principal Balance (LA_CPI_RPD_DIS)
					    fields[209+i], //Estimated Unpaid Interest (LA_ACR_INT_RPD)
						fields[245+i]
					};
					dt.Rows.Add(dataRow);
				}
			}
			else
			{
				throw new Exception(string.Format("Unrecognized LetterID {0}", letterid));
			}
			return dt;
		}

		/// <summary>
		/// Get all formFields
		/// </summary>
		/// <param name="dataLines">line data</param>
		/// <returns>dictionary of fields and their values</returns>
		private static Dictionary<string, string> GetFormFields(string dataLine)
		{
			Dictionary<string, string> formFields = new Dictionary<string, string>();
			List<string> oneLine = dataLine.SplitAndRemoveQuotes(",");

			formFields.Add("NAME", oneLine[3]);//Borrowers Name
			formFields.Add("DF_SPE_ACC_ID", oneLine[1]);//Borrowers Account Number
			formFields.Add("ACSKEY", oneLine[2]);
			formFields.Add("DX_STR_ADR_1", oneLine[4]);
			formFields.Add("DX_STR_ADR_2", oneLine[5]);
			formFields.Add("DM_CT", oneLine[6]);
			formFields.Add("DC_DOM_ST", oneLine[7]);
			formFields.Add("DF_ZIP_CDE", oneLine[8]);
			formFields.Add("DM_FGN_CNY", oneLine[9]);//Foreign country
			formFields.Add("IC_LON_PGM", oneLine[10]);//Loan program
			formFields.Add("LC_TYP_SCH_DIS", oneLine[11]);//Repayment schedule type
			formFields.Add("LR_INT_RPD_DIS", oneLine[12]);//current int rate
			formFields.Add("TOT_LA_CPI_RPD_DIS", oneLine[13]);//unpaid prin
			formFields.Add("RS_LA_ANT_CAP", oneLine[14]);//interest to be capitalized
			formFields.Add("RS_PRI_RPY", oneLine[15]);//prin to be repayed
			formFields.Add("RS_LA_RPD_INT_DIS", oneLine[16]);//interest repayable
			formFields.Add("RS_LA_TOT_RPD_DIS", oneLine[17]);//estimated total repayment
			formFields.Add("LC_ITR_TYP", oneLine[18]);//interest rate type
			formFields.Add("LA_FAT_NSI", oneLine[19]);//already paid interest
			formFields.Add("LN_RPS_TRM1", oneLine[20]); //start of monthly repayment schedule
			formFields.Add("LN_RPS_TRM2", oneLine[21]);
			formFields.Add("LN_RPS_TRM3", oneLine[22]);
			formFields.Add("LN_RPS_TRM4", oneLine[23]);
			formFields.Add("LN_RPS_TRM5", oneLine[24]);
			formFields.Add("LN_RPS_TRM6", oneLine[25]);
			formFields.Add("LN_RPS_TRM7", oneLine[26]);
			formFields.Add("LN_RPS_TRM8", oneLine[27]);
			formFields.Add("LN_RPS_TRM9", oneLine[28]);
			formFields.Add("LN_RPS_TRM10", oneLine[29]);
			formFields.Add("LN_RPS_TRM11", oneLine[30]);
			formFields.Add("LN_RPS_TRM12", oneLine[31]);
			formFields.Add("LN_RPS_TRM13", oneLine[32]);
			formFields.Add("LN_RPS_TRM14", oneLine[33]);
			formFields.Add("LN_RPS_TRM15", oneLine[34]);
			formFields.Add("LA_RPS_ISL1", oneLine[35]);
			formFields.Add("LA_RPS_ISL2", oneLine[36]);
			formFields.Add("LA_RPS_ISL3", oneLine[37]);
			formFields.Add("LA_RPS_ISL4", oneLine[38]);
			formFields.Add("LA_RPS_ISL5", oneLine[39]);
			formFields.Add("LA_RPS_ISL6", oneLine[40]);
			formFields.Add("LA_RPS_ISL7", oneLine[41]);
			formFields.Add("LA_RPS_ISL8", oneLine[42]);
			formFields.Add("LA_RPS_ISL9", oneLine[43]);
			formFields.Add("LA_RPS_ISL10", oneLine[44]);
			formFields.Add("LA_RPS_ISL11", oneLine[45]);
			formFields.Add("LA_RPS_ISL12", oneLine[46]);
			formFields.Add("LA_RPS_ISL13", oneLine[47]);
			formFields.Add("LA_RPS_ISL14", oneLine[48]);
			formFields.Add("LA_RPS_ISL15", oneLine[49]);
			formFields.Add("LD_RPS_1_PAY_DU1", oneLine[50]);
			formFields.Add("LD_RPS_1_PAY_DU2", oneLine[51]);
			formFields.Add("LD_RPS_1_PAY_DU3", oneLine[52]);
			formFields.Add("LD_RPS_1_PAY_DU4", oneLine[53]);
			formFields.Add("LD_RPS_1_PAY_DU5", oneLine[54]);
			formFields.Add("LD_RPS_1_PAY_DU6", oneLine[55]);
			formFields.Add("LD_RPS_1_PAY_DU7", oneLine[56]);
			formFields.Add("LD_RPS_1_PAY_DU8", oneLine[57]);
			formFields.Add("LD_RPS_1_PAY_DU9", oneLine[58]);
			formFields.Add("LD_RPS_1_PAY_DU10", oneLine[59]);
			formFields.Add("LD_RPS_1_PAY_DU11", oneLine[60]);
			formFields.Add("LD_RPS_1_PAY_DU12", oneLine[61]);
			formFields.Add("LD_RPS_1_PAY_DU13", oneLine[62]);
			formFields.Add("LD_RPS_1_PAY_DU14", oneLine[63]);
			formFields.Add("LD_RPS_1_PAY_DU15", oneLine[64]);
			formFields.Add("CURRENT DATE", oneLine[281]);
			return formFields;
		}

		/// <summary>
		/// Load all the File Data into an object.
		/// </summary>
		/// <param name="fileToProcess">The current file to be processed</param>
		private List<SasFileData> GroupLoanSeq(string fileToProcess)
		{
			List<SasFileData> data = new List<SasFileData>();
			using (StreamReader reader = new StreamReader(fileToProcess))
			{
				string header = reader.ReadLine();//read the header

				while (!reader.EndOfStream)
				{
					SasFileData sasData = new SasFileData();
					sasData.LoanSeqs = new List<int>();
					sasData.Header = header;

					sasData.LineData = (reader.ReadLine());

					List<string> fileLine = sasData.LineData.SplitAndRemoveQuotes(",");
					sasData.AccountNumber = fileLine[1];
						int loanSeq = 0;
					for (int i = 101; i <= 136; i++)
					{
                        if (int.TryParse(fileLine[i], out loanSeq))
							sasData.LoanSeqs.Add(loanSeq);
					}
					if (sasData.LoanSeqs.Count == 0)
					{
						string message = string.Format("There was an error getting the loan sequence number for account: {0}", sasData.AccountNumber);
						ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
						Err.AddRecord(ERR_IncorrectDataInFile, sasData);
						Eoj.Counts[ERR_IncorrectDataInFile].Increment();
					}
					data.Add(sasData);
				}


			}
			return data;
		}
	}
}