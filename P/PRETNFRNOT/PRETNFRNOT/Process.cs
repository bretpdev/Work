using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PRETNFRNOT
{
    class Process
    {
        public string ScriptId { get; set; }
        public ProcessLogRun PL { get; set; }

        public Process(string scriptId, ProcessLogRun logRun)
        {
            ScriptId = scriptId;
            PL = logRun;
        }

        /// <summary>
        /// Create a thread for each letter
        /// </summary>
        /// <returns></returns>
        public void Run(string passedLetterId = null)
        {
            LogDataAccess lda = new LogDataAccess(DataAccessHelper.CurrentMode, PL.ProcessLogId, false, false);
            DataAccess da = new DataAccess(lda);
            List<string> letters;
            if (passedLetterId.IsPopulated())
                letters = new List<string>() { passedLetterId }; //process specific letter only
            else
                letters = new List<string>() { "TS06BSPLIT", "TS06BPRTCH", "TS06BPSTFR", "TS06BTRNCL" };

            Parallel.ForEach(letters, letter =>
            {
                PrintLetter pl = new PrintLetter();
                pl.Start(letter, da, PL);
            });
        }
    }
}