using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace SIRPTFED
{
    public class MetricSummaryData
    {
        public int ServicerMetricsId { get; set; }
        public string ServicerCategory { get; set; }
        public string ServicerMetric { get; set; }
        public string ServicerMetricGoal { get; set; }
        public int CompliantRecords { get; set; }
        public int TotalRecords { get; set; }
        public int MetricMonth { get; set; }
        public int MetricYear { get; set; }
        public int AvgBacklog { get; set; }

    }
}
