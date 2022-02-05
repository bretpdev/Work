using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace ADDUPCORNQ
{
    public class CornerstoneQueues : BatchScriptBase
    {
		//Recovery value is SAS file name, index from SAS DataTable.

		private enum Mode
		{
			Add,
			Update
		}

		private readonly string ERROR_FILE;
		
		public CornerstoneQueues(ReflectionInterface ri)
            : base(ri, "ADDUPCORNQ")
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
			string addQueueSasFile = DeleteOldFilesReturnMostCurrent(FtpFolder, "ARC_QUEUE_COMPARISONS.R3*", Common.FileOptions.None);
			string updateQueueSasFile = DeleteOldFilesReturnMostCurrent(FtpFolder, "ARC_QUEUE_COMPARISONS.R4*", Common.FileOptions.None);
			if (string.IsNullOrEmpty(addQueueSasFile) && string.IsNullOrEmpty(updateQueueSasFile))
			{
				string message = "No SAS files were found.";
				MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}

			//Check for file-level recovery and process the files.
			string recoverySas = Recovery.RecoveryValue.SplitAgnosticOfQuotes(",")[0];
			if ((recoverySas == "" || recoverySas == addQueueSasFile) && !string.IsNullOrEmpty(addQueueSasFile))
			{
				AddOrUpdateTX5Z(Mode.Add, addQueueSasFile);
				Recovery.Delete();
			}
			if (!string.IsNullOrEmpty(updateQueueSasFile))
			{
				AddOrUpdateTX5Z(Mode.Update, updateQueueSasFile);
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

		private void AddOrUpdateTX5Z(Mode mode, string sasFile)
		{
			string modeString = (mode == Mode.Add ? "A" : "C");
			string[] successMessage = { "01003 NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED", "01004 RECORD SUCCESSFULLY ADDED", "01005 RECORD SUCCESSFULLY CHANGED" };

			DataTable sasTable = CreateDataTableFromFile(sasFile);
			for (int rowIndex = GetRecoveryRow(1); rowIndex < sasTable.Rows.Count; rowIndex++)
			{
				DataRow sasRow = sasTable.Rows[rowIndex];
				string queue = sasRow.Field<string>("WF_QUE");
				string subQueue = sasRow.Field<string>("WF_SUB_QUE");
				FastPath("TX3Z/{0}TX5Z{1};{2}", modeString, queue, subQueue);
				if (!Check4Text(1, 72, "TXX63"))
				{
					string message = "Error accessing queue detail screen: " + GetText(23, 2, 78);
					AddToErrorFile(sasFile, queue, subQueue, message);
					UpdateRecovery(sasFile, rowIndex);
					continue;
				}
				//We may be adding either a whole new queue, or just a sub-queue for an existing queue.
				//The only difference on the screen is that the top portion is non-writeable if we're
				//just adding a sub-queue. Rather than check whether one of the upper fields is
				//writeable (which would require a new method in ReflectionInterface), we'll let
				//PutText() check it for us. So if the first PutText() in the try block works,
				//then we're adding a new queue. If it throws an exception, the catch block will
				//just add the sub-queue.
				try
				{
					PutText(4, 34, sasRow.Field<string>("WM_QUE_FUL").Trim());
					PutText(4, 69, sasRow.Field<string>("WM_QUE_SHO").Trim());
					PutText(5, 20, sasRow.Field<string>("WF_USR_OWN_QUE").Trim());
					PutText(6, 20, sasRow.Field<string>("WN_DAY_PRD_GOA_WRK").Trim());
					if (mode == Mode.Add) { PutText(7, 20, sasRow.Field<string>("WI_RFS_QUE").Trim()); }
					PutText(7, 63, sasRow.Field<string>("WI_ASN_AUT_TSK").Trim());
					if (mode == Mode.Add) { PutText(8, 26, sasRow.Field<string>("WC_TYP_NUM_CTL_TSK").Trim()); }
					if (mode == Mode.Add) { PutText(9, 26, sasRow.Field<string>("WC_TYP_SRH_1").Trim()); }
					PutText(9, 49, sasRow.Field<string>("WC_SRT_SRH_1").Trim());
					if (mode == Mode.Add) { PutText(10, 26, sasRow.Field<string>("WC_TYP_SRH_2").Trim()); }
					PutText(10, 49, sasRow.Field<string>("WC_SRT_SRH_2").Trim());
					if (mode == Mode.Add) { PutText(11, 20, sasRow.Field<string>("WI_BCH_OLY_QUE").Trim()); }
					if (mode == Mode.Add) { PutText(11, 48, sasRow.Field<string>("WF_BCH_PGM_WRK_TSK").Trim()); }
					PutText(14, 2, sasRow.Field<string>("WX_MSG_HDR_1_TSK").Trim());
					PutText(16, 2, sasRow.Field<string>("WX_MSG_HDR_2_TSK").Trim());
					PutText(19, 33, sasRow.Field<string>("WM_SUB_QUE_FUL").Trim());
					PutText(19, 69, sasRow.Field<string>("WM_SUB_QUE_SHO").Trim());
					PutText(20, 17, sasRow.Field<string>("WF_SVR_SUB_QUE").Trim());
					if (mode == Mode.Add) { PutText(21, 17, sasRow.Field<string>("WC_TYP_SUB_QUE").Trim()); }
				}
				catch (Exception)
				{
					PutText(19, 33, sasRow.Field<string>("WM_SUB_QUE_FUL").Trim());
					PutText(19, 69, sasRow.Field<string>("WM_SUB_QUE_SHO").Trim());
					PutText(20, 17, sasRow.Field<string>("WF_SVR_SUB_QUE").Trim());
					PutText(21, 17, sasRow.Field<string>("WC_TYP_SUB_QUE").Trim());
					Hit(Key.Enter);
					PutText(4, 34, sasRow.Field<string>("WM_QUE_FUL").Trim());
					PutText(4, 69, sasRow.Field<string>("WM_QUE_SHO").Trim());
					PutText(5, 20, sasRow.Field<string>("WF_USR_OWN_QUE").Trim());
					PutText(6, 20, sasRow.Field<string>("WN_DAY_PRD_GOA_WRK").Trim());
					PutText(7, 63, sasRow.Field<string>("WI_ASN_AUT_TSK").Trim());
					PutText(9, 49, sasRow.Field<string>("WC_SRT_SRH_1").Trim());
					PutText(10, 49, sasRow.Field<string>("WC_SRT_SRH_2").Trim());
					PutText(14, 2, sasRow.Field<string>("WX_MSG_HDR_1_TSK").Trim());
					PutText(16, 2, sasRow.Field<string>("WX_MSG_HDR_2_TSK").Trim());
				}
				Hit(Key.Enter);
				if (!Check4Text(23, 2, successMessage))
				{
					string message = "Error committing queue details: " + GetText(23, 2, 78);
					AddToErrorFile(sasFile, queue, subQueue, message);
				}
				UpdateRecovery(sasFile, rowIndex);
			}//for
		}//AddOrUpdateTX5Z()

		private void AddToErrorFile(string sasFile, string queue, string subQueue, string message)
		{
			bool newFile = !File.Exists(ERROR_FILE);
			using (StreamWriter errorWriter = new StreamWriter(ERROR_FILE, true))
			{
				if (newFile) { errorWriter.WriteCommaDelimitedLine("SAS file", "Queue", "Sub-queue", "Message"); }
				errorWriter.WriteCommaDelimitedLine(sasFile, queue, subQueue, message);
			}
		}//AddToErrorFile()

		private void UpdateRecovery(string sasFile, int rowIndex)
		{
			Recovery.RecoveryValue = string.Format("{0},{1}", sasFile, rowIndex);
		}//UpdateRecovery()
    }//class
}//namespace
