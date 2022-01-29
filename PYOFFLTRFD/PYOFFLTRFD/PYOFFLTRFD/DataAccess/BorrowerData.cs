using System.Collections.Generic;
using Uheaa.Common.Scripts;

namespace PYOFFLTRFD
{
    public class BorrowerData
    {
        public SystemBorrowerDemographics Demos { get; set; }
        public string PayoffDate { get; set; }
        public List<PayoffData> PData { get; set; }
        public bool DemosLoaded { get; set; }

        public BorrowerData()
        {
            PData = new List<PayoffData>();
            Demos = new SystemBorrowerDemographics();
        }
    }
}