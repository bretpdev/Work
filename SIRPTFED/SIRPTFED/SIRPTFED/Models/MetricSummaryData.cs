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
        public int MetricsSummaryId { get; set; }
        //public int ServicerCategoryId { get; set; }
        public string Category { get; set; }
        //public int ServicerMetricsId { get; set; }
        public string Metric { get; set; }
        public int CompliantRecords { get; set; }
        public int TotalRecords { get; set; }
        public int MetricMonth { get; set; }
        public int MetricYear { get; set; }
        public int AvgBacklog { get; set; }
        public decimal? SuspenseAmount { get; set; }
        public decimal? SuspenseTotal { get; set; }
    }
}
