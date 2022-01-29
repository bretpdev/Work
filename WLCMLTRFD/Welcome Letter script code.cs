using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;
using Q;
using Key = Q.ReflectionInterface.Key;
using System;
using System.Windows.Forms;

namespace WCMELTRFED
{
	public class WelcomeLetterFed : FedBatchScriptBase
	{
		private const string EOJ_LETTERS_IN_FILE = "Total number of letters in the SAS file";
		private const string EOJ_LETTERS_GENERATED = "Number of letters generated";
		private const string EOJ_ERROR_ARC = "Number of ARC errors";
		private static readonly string[] EOJ_FIELDS = { EOJ_LETTERS_IN_FILE, EOJ_LETTERS_GENERATED, EOJ_ERROR_ARC };
		private const string SCRIPT_ID = "WCMELTRFED";
		private Credentials _loginData;
		private DataAccess _da;

		public WelcomeLetterFed(ReflectionInterface ri)
			: base(ri, SCRIPT_ID, "ERR_BU35", "EOJ_BU35", EOJ_FIELDS)
		{
			_da = new DataAccess(TestModeProperty, Region.CornerStone);
		}

		public override void Main()
		{
			StartupMessage("This script will print Welcome Letters for Borrowers Who Have Just Been Added to the System");

			string fileToProcess = PromptForFile();

			while (_loginData == null)
			{
				_loginData = Credentials.FromPrompt();
				RI.LogOut();
				if (_loginData != null && !RI.Login(_loginData.UserName, _loginData.Password, Region.CornerStone))
				{
					MessageBox.Show("You must put in a valid username and password to move forward", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
					_loginData = null;
				}
			}

			RI.LogOut();

			if (string.IsNullOrEmpty(Recovery.RecoveryValue) || Recovery.RecoveryValue.Contains("0,PrintingStarted"))
			{
				Recovery.RecoveryValue = "0,PrintingStarted";

				string coverDoc = string.Format(@"{0}{1} {2} CoverSheet.doc", Efs.GetPath("WelcomeLetter"), fileToProcess.Substring(3), UserID);
				string borrowerDoc = string.Format(@"{0}{1} {2}.doc", Efs.GetPath("WelcomeLetter"), fileToProcess.Substring(3), UserID);
				string coverSheetFolder = TestModeProperty ? @"X:\PADD\General\Test\" : @"X:\PADD\General\";
				string dataFile = string.Empty;

				string coverSheetFile = string.Empty;

				if (fileToProcess.Contains("R2"))
				{
					coverSheetFile = CreateStateMailCS("CSWELCOMEP", fileToProcess, "DC_DOM_ST");
					DocumentHandling.SaveDocs(coverSheetFolder, "Scripted State Mail Cover Sheet", coverSheetFile, coverDoc);
					dataFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(fileToProcess, "DF_SPE_ACC_ID", "CSWELCOMEP", true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, TestModeProperty);
					DocumentHandling.SaveDocs(TestModeProperty, "CSWELCOMEP", dataFile, borrowerDoc,Efs,Region.CornerStone);
				}
				else if (fileToProcess.Contains("R3"))
				{
					coverSheetFile = CreateStateMailCS("CSWELCOME3", fileToProcess, "DC_DOM_ST");
					DocumentHandling.SaveDocs(coverSheetFolder, "Scripted State Mail Cover Sheet", coverSheetFile, coverDoc);
					dataFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(fileToProcess, "DF_SPE_ACC_ID", "CSWELCOME3", true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, TestModeProperty);
					DocumentHandling.SaveDocs(TestModeProperty, "CSWELCOME3", dataFile, borrowerDoc, Efs, Region.CornerStone);
				}
				else if (fileToProcess.Contains("R4"))
				{
					coverSheetFile = CreateStateMailCS("CSWELCOME2", fileToProcess, "DC_DOM_ST");
					DocumentHandling.SaveDocs(coverSheetFolder, "Scripted State Mail Cover Sheet", coverSheetFile, coverDoc);
					dataFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(fileToProcess, "DF_SPE_ACC_ID", "CSWELCOME2", true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, TestModeProperty);
					DocumentHandling.SaveDocs(TestModeProperty, "CSWELCOME2", dataFile, borrowerDoc, Efs, Region.CornerStone);
				}
				else if (fileToProcess.Contains("R5"))
				{
					coverSheetFile = CreateStateMailCS("CSWELCOME5", fileToProcess, "DC_DOM_ST");
					DocumentHandling.SaveDocs(coverSheetFolder, "Scripted State Mail Cover Sheet", coverSheetFile, coverDoc);
					dataFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(fileToProcess, "DF_SPE_ACC_ID", "CSWELCOME5", true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, TestModeProperty);
					DocumentHandling.SaveDocs(TestModeProperty, "CSWELCOME5", dataFile, borrowerDoc, Efs, Region.CornerStone);
				}
				else if (fileToProcess.Contains("R6"))
				{
					
					coverSheetFile = CreateStateMailCS("CSWELCOME4", fileToProcess, "DC_DOM_ST");
					DocumentHandling.SaveDocs(coverSheetFolder, "Scripted State Mail Cover Sheet", coverSheetFile, coverDoc);
					dataFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(fileToProcess, "DF_SPE_ACC_ID", "CSWELCOME4", true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, TestModeProperty);
					DocumentHandling.SaveDocs(TestModeProperty, "CSWELCOME4", dataFile, borrowerDoc, Efs, Region.CornerStone);
				}
				else if (fileToProcess.Contains("R13"))
				{
					coverSheetFile = CreateStateMailCS("WLCMDEFFOR", fileToProcess, "DC_DOM_ST");
					DocumentHandling.SaveDocs(coverSheetFolder, "Scripted State Mail Cover Sheet", coverSheetFile, coverDoc);
					dataFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(fileToProcess, "DF_SPE_ACC_ID", "WLCMDEFFOR", true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, TestModeProperty);
					DocumentHandling.SaveDocs(TestModeProperty, "WLCMDEFFOR", dataFile, borrowerDoc, Efs, Region.CornerStone);
				}

				File.Delete(dataFile);
				File.Delete(coverSheetFile);//delete the coversheet data file
				Recovery.RecoveryValue = string.Format("0,Printing Complete");
			}

			if (Check4Text(16, 2, "LOGON"))
			{
				RI.Login(_loginData.UserName, _loginData.Password, Region.CornerStone);
			}

			int fileIndex = 0;

			if (fileToProcess.Contains("R3")) { fileIndex = 1; }
			else if (fileToProcess.Contains("R4")) { fileIndex = 2; }
			else if (fileToProcess.Contains("R5")) { fileIndex = 3; }
			else if (fileToProcess.Contains("R6")) { fileIndex = 4; }
			else if (fileToProcess.Contains("R13")) { fileIndex = 5; }


			string arc = string.Empty;
			string comment = string.Empty;

			switch (fileIndex)
			{
				case 0://R2
					arc = "WELCC";
					comment = "Welcome letter current";
					break;

				case 1://R3
					arc = "WELCF";
					comment = "Welcome letter transfer forbearance";
					break;

				case 2://R4
					arc = "WELCA";
					comment = "Welcome letter ACH";
					break;

				case 3://R5
					arc = "WELCD";
					comment = "Welcome letter Delinquent ";
					break;
				case 4://R6
					arc = "WELCX";
					comment = "Welcome letter ACH & transfer forbearance";
					break;
				case 5://R13
					arc = "WELCO";
					comment = "Welcome letter other";
					break;
			}

			IEnumerable<BorrowerData> fileInfo = readFileForProcessing(fileToProcess);

			int i;
			for (i = GetRecoveryRow(); i < fileInfo.Count() + 1; i++)
			{
				BorrowerData bor = fileInfo.ElementAt(i - 1);

				if (!ATD22ByBalance(bor.AcctNumber, arc, comment, false))
				{
					Err.AddRecord("The following ARC was not added to the borrowers Account", new { AccountNumber = bor.AcctNumber, Arc = arc });
					Eoj.Counts[EOJ_ERROR_ARC].Increment();
				}

				Recovery.RecoveryValue = string.Format("{0},{1}", i, fileToProcess);
			}

			i = 0;//completed file set recovery count back to 0
			Recovery.RecoveryValue = string.Format("{0},{1}", i, fileToProcess);

			File.Delete(fileToProcess);
			Recovery.Delete();
			ProcessingComplete();

		}//end main

		private string CreateStateMailCS(string letterId, string dataFile, string stateCodeFieldName)
		{
			const string FEDERAL_BU = "Federal Servicing";

			DataAccess _da = new DataAccess(TestModeProperty, Region.CornerStone);

			int domesticCount = 0;
			int foreignCount = 0;
			string coverSheetInstructions = _da.GetCostCenterInstructions(letterId);

			using (StreamReader fileRead = new StreamReader(dataFile))
			{
				List<string> headerFields = fileRead.ReadLine().SplitAgnosticOfQuotes(",");
				int stateCodeIndex = headerFields.IndexOf(stateCodeFieldName);

				while (!fileRead.EndOfStream)
				{
					List<string> dataFields = fileRead.ReadLine().SplitAgnosticOfQuotes(",");
					string stateCode = dataFields[stateCodeIndex];

					if (stateCode == "FC")
					{
						foreignCount++;
					}
					else
					{
						domesticCount++;
					}
				}//end while
			}//end using

			int total = (domesticCount + foreignCount);

			for (int count = 0; count < total; count++)//get the totals of the number of letters in each coversheet
			{
				Eoj.Counts[EOJ_LETTERS_GENERATED].Increment();
			}

			string coverSheetDataFile = string.Format("{0}{1}_cover.txt", Efs.TempFolder, SCRIPT_ID);
			const string COST_CENTER = "MA441";

			using (StreamWriter fileWrite = new StreamWriter(coverSheetDataFile))
			{
				string letterName = _da.GetLetterName(letterId);
				string pageCount = _da.GetPaperSheetCountForBatch2D(letterId).ToString();
				fileWrite.WriteCommaDelimitedLine("BU", "Description", "NumPages", "Cost", "Standard", "Foreign", "CoverComment");
				fileWrite.WriteCommaDelimitedLine(FEDERAL_BU, letterName, "1", COST_CENTER, domesticCount.ToString(), foreignCount.ToString(), coverSheetInstructions);
			}

			_da.AddFederalCostCenterPrintingRecord(letterId, foreignCount, domesticCount, COST_CENTER);

			return coverSheetDataFile;
		}//end CreateStateMailCoversheet

		private List<BorrowerData> readFileForProcessing(string file)
		{
			List<BorrowerData> borInfo = new List<BorrowerData>();
			DataTable dt = Common.CreateDataTableFromFile(file);

			foreach (DataRow dr in dt.Rows)
			{
				BorrowerData bor = new BorrowerData();
				bor.AcctNumber = dr.Field<string>("DF_SPE_ACC_ID");
				borInfo.Add(bor);
				Eoj.Counts[EOJ_LETTERS_IN_FILE].Increment();
			}

			return borInfo;
		}//end readFileForProcessing

		private string PromptForFile()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			DialogResult dr = DialogResult.None;

			if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
			{
				MessageBox.Show("The script is running in recovery mode.  Select the file to resume processing from the temporary folder.", "Recovery Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
				dialog.InitialDirectory = Efs.TempFolder;
			}
			else
			{
				dialog.InitialDirectory = Efs.FtpFolder;
			}

			while (dr != DialogResult.OK)
			{
				dr = dialog.ShowDialog();

				if (dr == DialogResult.Cancel)
				{
					EndDLLScript();
				}
			}

			FileInfo fi = new FileInfo(dialog.FileName);
			if (fi.Length < 1)
			{
				NotifyAndEnd("The SAS file {0} was empty", dialog.SafeFileName);
			}

			string newFileName = string.Format("{0}{1}", Efs.TempFolder, dialog.SafeFileName);

			if (string.IsNullOrEmpty(Recovery.RecoveryValue))
			{
				try
				{
					fi.MoveTo(newFileName);
				}
				catch (Exception ex)
				{
					NotifyAndEnd("There was an error while moving the file.  Resolve the problem and run the script again.{0}", ex.Message);
				}
			}

			return newFileName;
		}
	}
}
