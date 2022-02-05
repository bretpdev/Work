using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    class DemographicsSourceHelper
    {
        public List<DemographicsSourceInfo> Sources = new List<DemographicsSourceInfo>();
        public DemographicsSourceHelper(DataAccess da)
        {
            this.Sources = da.GetDemographicsSources();
        }
        public string GetDemographicsSourceName(DemographicsSource source)
        {
            return this.Sources.SingleOrDefault(o => o.DemographicsSourceId == source).Name;
        }
    }
}
