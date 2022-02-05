using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Q;

namespace OLBATLTR
{
	public class OneLinkBatchLetters : BatchScriptBase
	{
		//Recovery file format: SAS file (full path and name), record number

		private readonly string FTP_FOLDER;

		public OneLinkBatchLetters(ReflectionInterface ri)
			: base(ri, "OLBATLTR")
		{
			FTP_FOLDER = TestMode("").FtpFolder;
		}

		public override void Main()
		{
			if (!CalledByMasterBatchScript())
			{
				string message = "This script generates letters and leaves system comments in OneLINK from the SAS files listed in the OLBL_REF_SasDetail BSYS table. Click OK to continue or Cancel to quit.";
				if (MessageBox.Show(message, "OneLINK Batch Letters", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { EndDLLScript(); }
			}

			//Get the list of SAS files to process, along with their details.
			List<SasDetail> sasList = DataAccess.GetSasDetails(TestModeProperty);

			DoRecovery(sasList);
			ProcessSasFiles(sasList);
			ProcessingComplete();
		}//Main()

		private void DoRecovery(List<SasDetail> sasList)
		{
			if (string.IsNullOrEmpty(Recovery.RecoveryValue)) { return; }

			string fullPathAndFileName = Recovery.RecoveryValue.Split(',')[0];
			//Note: The Where clause below will work as long as none of the SAS files have report numbers above 19.
			//If any report numbers reach 20 or above, the lambda condition will need to be fullPathAndFileName.Contains(p.BaseFileName + "."),
			//and test files will have to have a "." after the report number.
			SasDetail recoveryDetail = sasList.Where(p => fullPathAndFileName.Contains(p.BaseFileName)).Single();
			CreateCommentsAndLetters(recoveryDetail, fullPathAndFileName);
			File.Delete(fullPathAndFileName);
			Recovery.Delete();
		}//DoRecovery()

		private void CreateCommentsAndLetters(SasDetail sasDetail, string fullPathAndFileName)
		{
			//Make a letter comment for everyone in the file.
			using (DataTable sasTable = CreateDataTableFromFile(fullPathAndFileName))
			{
				for (int currentRowIndex = GetRecoveryRow(1); currentRowIndex < sasTable.Rows.Count; currentRowIndex++)
				{
					DataRow sasRow = sasTable.Rows[currentRowIndex];
					string ssn = GetDemographicsFromLP22(sasRow.Field<string>("ACCOUNTNUMBER")).SSN;
					AddCommentInLP50(ssn, sasDetail.ActionCode, "LT", "03", sasDetail.CommentText);
					Recovery.RecoveryValue = string.Format("{0},{1}", fullPathAndFileName, currentRowIndex);
				}
			}//using

			//Print letters.
			string mergeFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(fullPathAndFileName, "ACCOUNTNUMBER", sasDetail.LetterId, true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower, TestModeProperty);
			DocumentHandling.CostCenterPrinting(TestModeProperty, sasDetail.LetterId, mergeFile, "CCC", "STATECODE", ScriptID);
			File.Delete(mergeFile);
		}//ProcessSasFile()

		private void ProcessSasFiles(List<SasDetail> sasList)
		{
			foreach (SasDetail sasDetail in sasList)
			{
				string fullPathAndFileName = DeleteOldFilesReturnMostCurrent(FTP_FOLDER, sasDetail.BaseFileName, Common.FileOptions.None);
				if (!File.Exists(fullPathAndFileName))
				{
					continue;
				}
				if (new FileInfo(fullPathAndFileName).Length == 0)
				{
					File.Delete(fullPathAndFileName);
					continue;
				}
				CreateCommentsAndLetters(sasDetail, fullPathAndFileName);
				File.Delete(fullPathAndFileName);
				Recovery.Delete();
			}//foreach
		}//ProcessSasFiles()
	}//class
}//namespace
