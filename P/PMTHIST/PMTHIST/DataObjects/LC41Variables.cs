using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTHIST
{
    public class LC41Variables
    {
        public string Name { get; set; }
        public int Aii { get; set; } = 0;
        public int Pii { get; set; } = 0;
        public int EDInd { get; set; } = 0;
        public string AccountNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? LastEffectiveDate { get; set; }
        public decimal OriginalPrincipal { get; set; } //PrincBeg

    }
}
