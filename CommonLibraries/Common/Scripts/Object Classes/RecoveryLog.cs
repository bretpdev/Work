using System;
using System.IO;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.Scripts
{
	public class RecoveryLog
	{
		private string RecValue;
		private string LogFile;
		public string RecoveryValue
		{
			get
			{
				return RecValue;
			}
			set
			{
				if (RecValue != value)
				{
					RecValue = value;
				}
				using (StreamWriter sw = new StreamW(LogFile))
				{
					sw.WriteLine(value);
				}
			}
		}

		public RecoveryLog(string fileName)
		{
			LogFile = string.Format("{0}{1}.txt", EnterpriseFileSystem.LogsFolder, fileName);
			InitializeRecoveryValue(LogFile);
		}

		public void Delete()
		{
			RecValue = string.Empty;
			FS.Delete(LogFile);
		}

		private void InitializeRecoveryValue(string logFile)
		{
            FileInfo info = new FileInfo(logFile);
			if (info.Exists && info.Length > 0)
				RecValue = FS.ReadAllLines(logFile)[0];
			else
				RecValue = string.Empty;

		}
        public int GetRecoveryRow()
        {
            int nextRow = 0;
            if (this.RecoveryValue.Length > 0)
            {
                if(!int.TryParse(this.RecoveryValue.Split(',')[0], out nextRow))
                    throw new Exception("The script is in recovery, but the first value in the recovery log is not a row number.");
            }

            nextRow += 1;
            RecoveryValue = string.Empty;
            return nextRow;
        }

	}
}
