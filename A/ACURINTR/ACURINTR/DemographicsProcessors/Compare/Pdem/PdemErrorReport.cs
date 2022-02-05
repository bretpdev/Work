using System;
using System.Data;
using System.IO;
using System.Text;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ACURINTR.DemographicsProcessors
{
    class PdemErrorReport : ErrorReport
    {
        /// <summary>
        /// Creates an object to facilitate reporting PDEM errors in the ACURINTR script.
        /// </summary>
        public PdemErrorReport(string queueName, int processLogId) : base(queueName, processLogId)
        {
        }
    }
}
