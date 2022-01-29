using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTHIST
{
    public class LC05Loan
    {
        //TODO maybe rename to make more sense
        //Expected Information
        public DateTime PurchaseDate { get; set; }
        public decimal PurchAmt { get; set; }
        public string CLID { get; set; }

        //Optional Information
        //Assignment info if loan was assigned to ED
        public DateTime? EDDate { get; set; }
        public decimal? EDAmt { get; set; }//Amount?
        public decimal? EDPrinc { get; set; }//Principal?
        public decimal? EDInt { get; set; }
        public decimal? EDLegal { get; set; }
        public decimal? EDOther { get; set; }
        public decimal? EDCC { get; set; }

    }
}
