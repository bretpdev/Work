using System;
using System.Data;
using System.IO;
using System.Text;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
    class AccurintErrorReport : ErrorReport
    {
        /// <summary>
        /// Creates an object to facilitate reporting Accurint errors in the ACURINTR script.
        /// </summary>
        public AccurintErrorReport(int processLogId) : base("ACURINTR", processLogId)
        {
        }

    }
}
