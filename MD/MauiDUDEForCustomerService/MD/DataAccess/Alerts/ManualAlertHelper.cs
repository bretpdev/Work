using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MD
{
    class ManualAlertHelper
    {
        [UsesSproc(DataAccessHelper.Database.Uls, "alerts.[GetManualBorrowerAlerts]")]
        public List<ManualAlert> GetAlerts(string ssn)
        {
            var alerts = DataAccessHelper.ExecuteList<ManualAlert>("alerts.[GetManualBorrowerAlerts]", DataAccessHelper.Database.Uls, SqlParams.Single("Ssn", ssn));
            return alerts;
        }
    }
    public class ManualAlert
    {
        public string Alert { get; set; }
        public bool AbortAfterMessageDisplay { get; set; }
    }
}
