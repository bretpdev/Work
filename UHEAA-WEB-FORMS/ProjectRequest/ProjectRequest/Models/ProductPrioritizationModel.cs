using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectRequest.Models
{
    public class ProductPrioritization
    {
        public string ProjectName { get; set; }
        public string BusinessUnit { get; set; }
        public string Details { get; set; }
        public int FinanceScore { get; set; }
        public int RequestorScore { get; set; }
        public int UrgencyScore { get; set; }
        public int ResourcesScore { get; set; }
        public int TotalScore { get; set; }

    }
}