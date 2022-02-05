using System.Data;
using System.IO;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace ADDUPCORNA
{
    public class CornerstoneArcs : BatchScriptBase
    {
		//Recovery value is SAS file name, index from SAS DataTable.

		private enum Mode
		{
			Add,
			Update
		}

		private readonly string ERROR_FILE;

		public CornerstoneArcs(ReflectionInterface ri)
            : base(ri, "ADDUPCORNA")
        {
			ERROR_FILE = DataAccess.PersonalDataDirectory + ScriptID + "_Errors.txt";
		}

        public override void Main()
        {
			//Check that we're in the CornerStone region.
			FastPath("TX3Z/ATC00;");
			if (!Check4Text(1, 38, "UHEAAFED"))
			{
				string message = "Please log into the CornerStone region before running this script.";
				MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}

			//Find the most recent SAS files.
			string addArcSasFile = DeleteOldFilesReturnMostCurrent(FtpFolder, "ARC_QUEUE_COMPARISONS.R7*", Common.FileOptions.None);
			string updateArcSasFile = DeleteOldFilesReturnMostCurrent(FtpFolder, "ARC_QUEUE_COMPARISONS.R8*", Common.FileOptions.None);
			if (string.IsNullOrEmpty(addArcSasFile) && string.IsNullOrEmpty(updateArcSasFile))
			{
				string message = "No SAS files were found.";
				MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}

			//Check for file-level recovery and process the files.
			string recoverySas = Recovery.RecoveryValue.SplitAgnosticOfQuotes(",")[0];
			if ((recoverySas == "" || recoverySas == addArcSasFile) && !string.IsNullOrEmpty(addArcSasFile))
			{
				AddOrUpdateTD00(Mode.Add, addArcSasFile);
				Recovery.Delete();
			}
			if (!string.IsNullOrEmpty(updateArcSasFile))
			{
				AddOrUpdateTD00(Mode.Update, updateArcSasFile);
				Recovery.Delete();
			}

			//Wrap up.
			if (File.Exists(ERROR_FILE))
			{
				string message = "Processing is complete, but some errors were encountered.";
				message += string.Format(" See {0} for a list of errors.", ERROR_FILE);
				MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				ProcessingComplete();
			}
		}//Main()

		private void AddOrUpdateTD00(Mode mode, string sasFile)
		{
            //changed line below from string modeString = (mode == Mode.Add ? "A" : "C"); doesn't make much sense but as per Jeremy B. 
            //we won't likely be using this script after this so I'm not going to take time to untangle stuff so it makes sense.
			string modeString = (mode == Mode.Add ? "C" : "C");
			string[] successMessage = (mode == Mode.Add ? new string[] { "01004 RECORD SUCCESSFULLY ADDED" } : new string[] { "01003 NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED", "01005 RECORD SUCCESSFULLY CHANGED" });

			DataTable sasTable = CreateDataTableFromFile(sasFile);
			int rowIndex = GetRecoveryRow(1);
			while (rowIndex < sasTable.Rows.Count)
			{
				DataRow sasRow = sasTable.Rows[rowIndex];
				string arc = sasRow.Field<string>("PF_REQ_ACT");
				FastPath("TX3Z/{0}TD00{1}", modeString, arc);
				if (!Check4Text(1, 72, "TDX03"))
				{
					string message = "Error accessing ARC detail screen: " + GetText(23, 2, 78);
					AddToErrorFile(sasFile, arc, message);
					UpdateRecovery(sasFile, rowIndex);
					rowIndex++;
					continue;
				}
				PutText(4, 57, sasRow.Field<string>("PX_ACT_DSC_REQ").Trim());
				PutText(7, 30, sasRow.Field<string>("PC_REQ_ACT_RCP").Trim());
				PutText(7, 66, sasRow.Field<string>("PC_DD_ACT_STA").Trim());
				PutText(9, 30, sasRow.Field<string>("PC_TYP_REQ_ACT").Trim());
                PutText(9, 66, sasRow.Field<string>("PF_LTR").Trim().TrimStart('U').Insert(0,"T")); //replace U with T at the front of the letter id.
				PutText(10, 30, sasRow.Field<string>("PC_SUP_PCA").Trim());
				PutText(10, 66, sasRow.Field<string>("II_PRT_LTR_DDB_LIT").Trim());
				PutText(11, 30, sasRow.Field<string>("PI_LOG_ATY_ACT").Trim());
				PutText(11, 66, sasRow.Field<string>("PF_REQ_ACT_WRK").Trim());
				PutText(12, 30, sasRow.Field<string>("PC_CCI_CLM_COL_ATY").Trim());
				PutText(13, 30, sasRow.Field<string>("PI_USR_REQ_ACT").Trim());
				PutText(13, 66, sasRow.Field<string>("PI_MNL_TSK_CLO").Trim());
				PutText(15, 30, sasRow.Field<string>("PF_NXT_ONL_TRX").Trim());
				PutText(15, 66, sasRow.Field<string>("PC_NXT_ONL_TRX_MOD").Trim());
				PutText(16, 30, sasRow.Field<string>("PI_ATY_AUT_CMP").Trim());
				PutText(17, 30, sasRow.Field<string>("WF_QUE").Trim());
				PutText(19, 15, sasRow.Field<string>("PX_ACT_COL_AUT_REQ").Trim());
				PutText(21, 18, sasRow.Field<string>("PI_QUE_STA_ASN").Trim());
				PutText(21, 28, sasRow.Field<string>("PI_QUE_STA_CAN").Trim());
				PutText(21, 43, sasRow.Field<string>("PI_QUE_STA_CMP").Trim());
				PutText(21, 54, sasRow.Field<string>("PI_QUE_STA_HLD").Trim());
				PutText(21, 68, sasRow.Field<string>("PI_QUE_STA_PRB").Trim());
				PutText(22, 43, sasRow.Field<string>("PI_QUE_STA_UAS").Trim());
				PutText(17, 66, sasRow.Field<string>("PF_RSP_ACT").Trim());
				DataRow[] matchingArcs = sasTable.Select(string.Format("PF_REQ_ACT = '{0}'", sasRow["PF_REQ_ACT"]));
				Hit(Key.Enter);
				if (!Check4Text(23, 2, successMessage))
				{
					string message = "Error committing ARC details: " + GetText(23, 2, 78);
					AddToErrorFile(sasFile, arc, message);
				}
				else if (matchingArcs.Length > 1)
				{
					Hit(Key.F6);
					for (int i = 1; i < matchingArcs.Length; i++)
					{
						PutText(i + 9, 8, matchingArcs[i].Field<string>("PF_RSP_ACT").Trim());
					}
					Hit(Key.Enter);
					if (!Check4Text(23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED"))
					{
						string message = "Error adding response codes: " + GetText(23, 2, 78);
						AddToErrorFile(sasFile, arc, message);
					}
				}
				UpdateRecovery(sasFile, rowIndex + matchingArcs.Length - 1);
				rowIndex += matchingArcs.Length;
			}//while
		}//AddOrUpdateTX5Z()

		private void AddToErrorFile(string sasFile, string arc, string message)
		{
			bool newFile = !File.Exists(ERROR_FILE);
			using (StreamWriter errorWriter = new StreamWriter(ERROR_FILE, true))
			{
				if (newFile) { errorWriter.WriteCommaDelimitedLine("SAS file", "ARC", "Message"); }
				errorWriter.WriteCommaDelimitedLine(sasFile, arc, message);
			}
		}//AddToErrorFile()

		private void UpdateRecovery(string sasFile, int rowIndex)
		{
			Recovery.RecoveryValue = string.Format("{0},{1}", sasFile, rowIndex);
		}//UpdateRecovery()
	}//class
}//namespace
