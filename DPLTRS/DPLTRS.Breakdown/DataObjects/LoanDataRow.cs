using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLTRS.Breakdown.DataObjects
{
    public class LoanDataRow
    {
        //public double TotalAcc { get; set; } = 0;
        //public double TotalCol { get; set; } = 0;
        public double Balance { get; set; } = 0;
        public double TotalDef { get; set; } = 0;
        //public double PrincCur { get; set; } = 0;
        public double PrincCol { get; set; } = 0;
        public double IntAcc { get; set; } = 0;
        public double IntCol { get; set; } = 0;
        //public double IntCur { get; set; } = 0;
        public double LegalAcc { get; set; } = 0;
        public double LegalCol { get; set; } = 0;
        //public double LegalCur { get; set; } = 0;
        public double OtherAcc { get; set; } = 0;
        public double OtherCol { get; set; } = 0;
        public double OtherCur { get; set; } = 0;
        public double CCAcc { get; set; } = 0;
        public double CCCol { get; set; } = 0;
        //public double CCCur { get; set; } = 0;
        public double CCProj { get; set; } = 0;
        public string UniqueId { get; set; } = "";

    }
}
