using Uheaa.Common.DataAccess;

namespace CentralizedPrintingProcess
{
    public class MiscDat
	{
        public string EcorrDirectory { get; private set; }
		public string FaxDirectory { get; private set; }
        public string LetterDirectory { get; private set; }
        public string ScriptId { get; set; }

        const string efsPrint = "Central Print";
        const string efsFax = "Central Fax";
		/// <summary>
		/// Initializes all paths and database connections
		/// </summary>
		public MiscDat(string scriptId)
		{
			ScriptId = scriptId;
            EcorrDirectory = EnterpriseFileSystem.GetPath("ECORRLocation", DataAccessHelper.Region.Uheaa);
			LetterDirectory = EnterpriseFileSystem.GetPath(efsPrint, DataAccessHelper.Region.Uheaa);
            FaxDirectory = EnterpriseFileSystem.GetPath(efsFax, DataAccessHelper.Region.Uheaa);
		}
	}
}