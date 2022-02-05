using System;
using System.IO;
using Uheaa.Common.DataAccess;

namespace ACCURINT
{
	class SpecialRequestErrorFile
	{
		public string FileName { get; private set; }

		public SpecialRequestErrorFile()
		{
			FileName = EnterpriseFileSystem.GetPath("ERR_Accurint") + "Could not add KUBSS Activity Comment.csv";
		}

		public void AddRecord(string ssn, string address, string city, string state, string zip, string message)
		{
			File.AppendAllText(FileName, string.Format("{0:MM/dd/yyyy},{1},{2},{3},{4},{5},{6}", DateTime.Now, ssn, address, city, state, zip, message));
		}
	}
}
