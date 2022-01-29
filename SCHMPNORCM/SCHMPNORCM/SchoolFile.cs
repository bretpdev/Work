using System.IO;
using Q;

namespace SCHMPNORCM
{
	class SchoolFile
	{
		private const string FILE_NAME = @"T:\SchoolMPNSetup.txt";

		public bool Exists { get { return File.Exists(FILE_NAME); } }

		private int _recordCount;
		public int RecordCount { get { return _recordCount; } }

		public SchoolFile()
		{
			//TODO: Do we need to account for recovery, and read an existing file if found?
			_recordCount = 0;
		}

		public bool Delete()
		{
			_recordCount = 0;
			if (File.Exists(FILE_NAME))
			{
				File.Delete(FILE_NAME);
				return true;
			}
			else
			{
				return false;
			}
		}//Delete()

		//TODO: Consider using "Add()" and keeping an in-memory list of objects.
		public void AddSchool(SetupDetails details)
		{
			string effectiveDateText = details.EffectiveDate.ToString("MMddyy");
			string optionalOptionText = (details.Elmres ? "ELMRES" : "");
			string setupTypeText = GetSetupTypeText(details.SchoolSetTo);
			using (StreamWriter schoolFileWriter = new StreamWriter(FILE_NAME, true))
			{
				if (details.Program == SetupDetails.LoanProgram.Stafford)
				{
					schoolFileWriter.WriteCommaDelimitedLine(details.SchoolCode, effectiveDateText, setupTypeText, "STFFRD", optionalOptionText);
					_recordCount++;
				}
				if (details.Program == SetupDetails.LoanProgram.Plus)
				{
					schoolFileWriter.WriteCommaDelimitedLine(details.SchoolCode, effectiveDateText, setupTypeText, "PLUS", optionalOptionText);
					_recordCount++;
				}
				if (details.CommonlineApplication)
				{
					schoolFileWriter.WriteCommaDelimitedLine(details.SchoolCode, effectiveDateText, setupTypeText, "CL App", optionalOptionText);
					_recordCount++;
				}
				if (details.CommonlineChange)
				{
					schoolFileWriter.WriteCommaDelimitedLine(details.SchoolCode, effectiveDateText, setupTypeText, "CL Change", optionalOptionText);
					_recordCount++;
				}
				if (details.CommonlineDisbursementRoster)
				{
					schoolFileWriter.WriteCommaDelimitedLine(details.SchoolCode, effectiveDateText, setupTypeText, "CL Disbursement Roster", optionalOptionText);
					_recordCount++;
				}
				if (details.ModificationResponse)
				{
					schoolFileWriter.WriteCommaDelimitedLine(details.SchoolCode, effectiveDateText, setupTypeText, "Modification Response", optionalOptionText);
					_recordCount++;
				}
				if (details.HoldAllDisbursements)
				{
					schoolFileWriter.WriteCommaDelimitedLine(details.SchoolCode, effectiveDateText, setupTypeText, "Hold All Disb", optionalOptionText);
					_recordCount++;
				}
				if (details.ServiceBureauParticipant)
				{
					schoolFileWriter.WriteCommaDelimitedLine(details.SchoolCode, effectiveDateText, setupTypeText, "Service Bureau Participant", optionalOptionText);
					_recordCount++;
				}
			}//using
		}//WriteSetupDetails()

		private string GetSetupTypeText(SetupDetails.SetupType setupType)
		{
			switch (setupType)
			{
				case SetupDetails.SetupType.Clearinghouse:
					return "Clearinghouse";
				case SetupDetails.SetupType.Commonline:
					return "Commonline";
				case SetupDetails.SetupType.Nslds:
					return "NSLDS";
				case SetupDetails.SetupType.SerialMpn:
					return "Serial MPN";
				default:
					return "";
			}//switch
		}//GetSetupTypeText()
	}//class
}//namespace
