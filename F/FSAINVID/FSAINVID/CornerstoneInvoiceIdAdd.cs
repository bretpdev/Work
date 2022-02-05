using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace FSAINVID
{
	public class CornerstoneInvoiceIdAdd : FedBatchScriptBase
	{
		private const string LOANS_WITH_FSA_ID = "Total Number of Loans with FSA Invoice ID Added";
		private const string BORROWERS_WITH_FSA_ID = "Total Number of Borrowers with FSA Invoice ID Added";
		private static readonly string[] EOJ_FIELDS = { LOANS_WITH_FSA_ID, BORROWERS_WITH_FSA_ID };

		public CornerstoneInvoiceIdAdd(ReflectionInterface ri)
			: base(ri, "FSAINVID", "ERR_BU01", "EOJ_BU01", EOJ_FIELDS)
		{
		}

		public override void Main()
		{
			const string FILE_TO_PROCESS = "Loan Transfer Request Report.txt";
			string foundFile = string.Empty;
			try
			{
				foundFile = Common.DeleteOldFilesReturnMostCurrent(Efs.FtpFolder, FILE_TO_PROCESS, Common.FileOptions.ErrorOnMissing);
			}
			catch (FileNotFoundException)
			{
				NotifyAndEnd(string.Format(" The {0} file could not be found.  Please investigate and try again.", FILE_TO_PROCESS));
			}
			IEnumerable<InvoiceRecord> fileInfo = ReadFile(foundFile);
			
			for (int i = GetRecoveryRow(); i < fileInfo.Count()+1; i++)
			{	
				InvoiceRecord bor = fileInfo.ElementAt(i - 1);
				bor.AccountNumber = GetDemographicsFromTX1J(bor.SSN).AccountNumber;
				bor.NewFsaInvoiceId = UpdateFsaId(bor.FsaInvoiceId);//update the invoice id to the correct format
				FastPath("TX3ZCTS7C{0}", bor.SSN);
				if (Check4Text(1, 72, "TSX3S"))//selection screen
				{
					bool updatedAtLeast1Loan = false;
					while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
					{
						for (int row = 7; !Check4Text(row, 4, " "); row++)
						{
							bor.LoanSeq = GetText(row, 20, 4);
							PutText(22, 19, GetText(row, 3, 3),Key.Enter,true);
							if (AddFsaInvoiceId(bor)) { updatedAtLeast1Loan = true; }
							Hit(Key.F12);
						}
						Hit(Key.F8);

					}//end while

					if (updatedAtLeast1Loan) { Eoj.Counts[BORROWERS_WITH_FSA_ID].Increment(); }

				}//end if
				else//target screen only one loan
				{
					bor.LoanSeq = "0001";
					if (AddFsaInvoiceId(bor)) { Eoj.Counts[BORROWERS_WITH_FSA_ID].Increment(); }
				}
				Recovery.RecoveryValue = i.ToString();//set recovery value
			}//End for
			File.Delete(foundFile);
			ProcessingComplete();
		}//end main

		private string UpdateFsaId(string fsaInvoiceId)
		{
			//Wremove the . and replace with an A and put a U in front of the first number
			return string.Concat("U", fsaInvoiceId).Replace(".", "A");
		}

		private bool AddFsaInvoiceId(InvoiceRecord bor)
		{
			if (Check4Text(9, 48, "_"))
			{
				PutText(14, 48, "N");//required fields on TS7C
				if (GetText(18, 19, 2) == "__")
				{
					PutText(18, 19, DetermineGraceMonths(GetText(6, 38, 6).TrimEnd(' ')) ? "6" : "0");
				}
				PutText(9, 48, bor.NewFsaInvoiceId);
				Hit(Key.Enter);

				if (Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGE"))
				{
					Eoj.Counts[LOANS_WITH_FSA_ID].Increment();
					return true;
				}
				else
				{
					Err.AddRecord("There was an error in adding the FSA Invoice Id for: ", new {AccountNumber= bor.AccountNumber, LoanSeq = bor.LoanSeq, FsaInvoiceId = bor.FsaInvoiceId });
					return false;
				}
			}//end if 
			else { return false; }
		}//End UpdateFSAInvoice

		private List<InvoiceRecord> ReadFile(string fileToProcess)
		{
			DataTable dt = Common.CreateDataTableFromFile(fileToProcess);
			return dt.AsEnumerable().Select(dr => new InvoiceRecord
			{
				SSN = dr.Field<string>("SSN"),
				FsaInvoiceId = dr.Field<string>("FSAINVOICEID")
			}).ToList();
		}//end read file

		private bool DetermineGraceMonths(string loanPgm)
		{
			string[] loanPgmWithGrace = { "DLSTFD", "DLUNST", "STFFRD", "TEACH", "UNSTFD" };
			if (loanPgmWithGrace.Contains(loanPgm)) { return true; }
			else { return false; }
		}
	}//end CornerstoneInvoiceIdAdd
}//end namespce
