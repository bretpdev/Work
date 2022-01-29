using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace IBRFORBCLN
{
	public class ForbearanceCleaner : BatchScriptBase
	{
		//Recovery value is the row index from Excel.

		public ForbearanceCleaner(ReflectionInterface ri)
			: base(ri, "IBRFORBCLN")
		{
		}

		public override void Main()
		{
			string startupMessage = "This script fills in the forbearance begin date on the \"No IBR forb\" tab in the \"UT IBR manual review\" spreadsheet.";
			startupMessage += " Click OK to continue, or Cancel to quit.";
			if (MessageBox.Show(startupMessage, ScriptID, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { EndDLLScript(); }

			string fileName = GetFile();
			if (string.IsNullOrEmpty(fileName)) { return; }

			Excel.Worksheet xl = OpenSpreadsheet(fileName);
			int excelRow = GetRecoveryRow(0);
			if (excelRow == 0) { excelRow = 2; }
			string ssnCell = string.Format("A{0}", excelRow);
			object ssn = xl.get_Range(ssnCell, ssnCell).Value2;
			while (ssn != null)
			{
				//Grab the loan sequence and IBR start date from the spreadsheet.
				string loanSequenceCell = string.Format("B{0}", excelRow);
				string loanSequence = xl.get_Range(loanSequenceCell, loanSequenceCell).Value2.ToString();
				string ibrStartDateCell = string.Format("C{0}", excelRow);
				DateTime ibrStartDate = DateTime.Parse(xl.get_Range(ibrStartDateCell, ibrStartDateCell).Value2.ToString());

				//Calculate the F43 begin date from the system.
				DateTime f43BeginDate = GetF43BeginDate(ssn.ToString(), loanSequence, ibrStartDate);

				//Write out the calculated date to the spreadsheet.
				string f43StartDateCell = string.Format("N{0}", excelRow);
				xl.get_Range(f43StartDateCell, f43StartDateCell).Value2 = f43BeginDate.ToString("yyyy-MM-dd");

				//Set recovery and move to the next row.
				Recovery.RecoveryValue = excelRow.ToString();
				excelRow++;
				ssnCell = string.Format("A{0}", excelRow);
				ssn = xl.get_Range(ssnCell, ssnCell).Value2;
			}//while

			ProcessingComplete();
		}//Main()

		private string GetFile()
		{
			const string MESSAGE = "Please find and select the \"UT IBR manual review.xls\" spreadsheet.";
			string fileName = "";
			using (OpenFileDialog dialog = new OpenFileDialog())
			{
				DialogResult result = dialog.ShowDialog();
				fileName = dialog.FileName;
				while (result != DialogResult.Cancel && !fileName.EndsWith("UT IBR manual review.xls"))
				{
					MessageBox.Show(MESSAGE, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Error);
					result = dialog.ShowDialog();
					fileName = dialog.FileName;
				}
			}//using
			return fileName;
		}//GetFile()

		private DateTime GetBillDueDate(string ssn, DateTime ibrStartDate)
		{
			FastPath("TX3Z/ITS12{0}", ssn);
			if (Check4Text(23, 2, "01721 BORROWER HAS NOT BEEN BILLED"))
			{
				return DateTime.MinValue;
			}
			else
			{
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 8; !Check4Text(row, 3, " "); row++)
					{
						DateTime dueDate = DateTime.Parse(GetText(row, 5, 8));
						bool isActive = Check4Text(row, 24, "A");
						if (isActive && dueDate < ibrStartDate) { return dueDate; }
					}
					if (Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
					{
						Hit(Key.Enter);
					}
					else
					{
						Hit(Key.F8);
					}
				}//while
			}
			//If there is no active due date prior to the IBR start date,
			//return DateTime.MinValue to ensure that the deferment/forbearance end date gets used.
			return DateTime.MinValue;
		}//GetDueDate()

		private DateTime GetDefermentOrForbearanceEndDate(string ssn, string loanSequence, DateTime ibrStartDate)
		{
			FastPath("TX3Z/ITSAY{0}", ssn);
			if (Check4Text(1, 73, "TSXAX"))
			{
				//No deferments/forbearances.
				return DateTime.MinValue;
			}
			else if (Check4Text(1, 72, "TSXB0"))
			{
				//Target screen.
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int targetRow = 9; !Check4Text(targetRow, 3, " "); targetRow++)
					{
						DateTime endDate = DateTime.Parse(GetText(targetRow, 40, 8));
						if (endDate < ibrStartDate) { return endDate; }
					}
					Hit(Key.F8);
				}//while
			}
			else
			{
				//Selection screen.
				string paddedLoanSequence = loanSequence.PadLeft(3, '0');
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int selectionRow = 9; !Check4Text(selectionRow, 3, " "); selectionRow++)
					{
						if (Check4Text(selectionRow, 14, paddedLoanSequence))
						{
							PutText(22, 17, GetText(selectionRow, 2, 2), Key.Enter);
							while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
							{
								for (int targetRow = 9; !Check4Text(targetRow, 3, " "); targetRow++)
								{
									DateTime endDate = DateTime.Parse(GetText(targetRow, 40, 8));
									if (endDate < ibrStartDate) { return endDate; }
								}
								Hit(Key.F8);
							}//while
						}//if
					}//for
					Hit(Key.F8);
				}//while
			}
			//If there is no deferment/forbearance end date prior to the IBR start date,
			//return DateTime.MinValue to ensure that the bill due date gets used.
			return DateTime.MinValue;
		}//GetDefermentOrForbearanceEndDate()

		private DateTime GetF43BeginDate(string ssn, string loanSequence, DateTime ibrStartDate)
		{
			DateTime billDueDate = GetBillDueDate(ssn, ibrStartDate);
			DateTime defermentOrForbearanceEndDate = GetDefermentOrForbearanceEndDate(ssn, loanSequence, ibrStartDate);
			DateTime moreRecentDate = (billDueDate > defermentOrForbearanceEndDate ? billDueDate : defermentOrForbearanceEndDate);
			return moreRecentDate.AddDays(1);
		}//GetF43BeginDate()

		private Excel.Worksheet OpenSpreadsheet(string fileName)
		{
			Excel.Application xl = new Excel.ApplicationClass();
			xl.Visible = true;
			xl.Workbooks.Open(fileName, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, false, false, false);
			Excel.Worksheet sheet = (Excel.Worksheet)xl.Worksheets["No IBR forb"];
			return sheet;
		}//OpenSpreadsheet()
	}//class
}//namespace
