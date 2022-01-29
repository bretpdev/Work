using System.IO;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using System;

namespace ACCURINT
{
	public class AccurintRequestFile
	{
		public bool Exists { get { return File.Exists(FileName); } }
		public string FileName { get; set; }
		public bool IsEmpty { get { return new FileInfo(FileName).Length == 0; } }
		public int RecordCount { get; private set; }

		private readonly string ArchiveFolder;

		public AccurintRequestFile()
		{
			FileName = EnterpriseFileSystem.TempFolder + "AccurintRequestFile.txt";
            ArchiveFolder = EnterpriseFileSystem.GetPath("Accurint Archive");
			try
			{
				RecordCount = (File.Exists(FileName) ? File.ReadAllLines(FileName).Length : 0);
			}
			catch(Exception ex)
            {
				Dialog.Warning.Ok($"Error reading records in {FileName}. Record count undetermined. Error: {ex.Message}");
            }
		}

		public void AddRecord(string ssn, string lastName, string firstName, string birthDate, string address1, string address2, string city, string state, string zip)
		{
			using (StreamWriter fileWriter = new StreamWriter(FileName,true))
			{
				fileWriter.WriteCommaDelimitedLine(ssn, lastName, firstName, birthDate, address1, address2, city, state, zip);
			}
			RecordCount++;
		}

		public void ArchiveCopy(string archiveFileName)
		{
			File.Copy(FileName, ArchiveFolder + archiveFileName, true);
		}

		public void Delete()
		{
			File.Delete(FileName);
			RecordCount = 0;
		}
	}
}
