using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace NSFREVENTR
{
    public abstract class BatchReportPrinting : ScriptCommonBase
    {

        protected TestModeResults _testModeResults;

        /// <summary>
        /// Constructor
        /// </summary>
        public BatchReportPrinting(bool testMode)
            : base(testMode)
        {
        }

        /// <summary>
        /// Generates needed file and prints them.
        /// </summary>
        public abstract void GenerateAndPrintReports();

    }
}
