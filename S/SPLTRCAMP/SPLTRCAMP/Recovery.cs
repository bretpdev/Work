using Q;

namespace SPLTRCAMP
{
    public class Recovery : ScriptCommonBase
    {
        public bool InRecovery { get; set; }
        public string RecoveryLog { get; set; }
        public string AccountNumber { get; set; }

        public Recovery(bool testMode)
            : base(testMode)
        {
            InRecovery = false;
            RecoveryLog = string.Empty;
            AccountNumber = string.Empty;
        }

        /// <summary>
        /// Gets the contents of the recovery log file.
        /// </summary>
        public string GetLogContents()
        {
            string errorLog;
            InRecovery = true;
            VbaStyleFileOpen(RecoveryLog, 3, Common.MSOpenMode.Input);
            AccountNumber = VbaStyleFileInput(3);
            errorLog = VbaStyleFileInput(3);
            VbaStyleFileClose(3);
            return errorLog;
        }


        public void UpdateLogContents(string accountNumber, string errorLog)
        {
            VbaStyleFileOpen(RecoveryLog, 3, Common.MSOpenMode.Output);
            VbaStyleFileWriteLine(3, accountNumber, errorLog);
            VbaStyleFileClose(3);
        }

    }
}
