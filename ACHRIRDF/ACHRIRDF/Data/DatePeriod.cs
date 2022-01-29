using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACHRIRDF
{
    class DatePeriod
    {
        public DatePeriod()
        {

        }

        public DatePeriod(string BF_SSN, int LN_SEQ, DateTime Begin_Date, DateTime End_Date, string OwnerCode)
        {
            this.BF_SSN = BF_SSN;
            this.LN_SEQ = LN_SEQ;
            this.Begin_Date = Begin_Date;
            this.End_Date = End_Date;
            this.OwnerCode = OwnerCode;
        }

        public string BF_SSN { get; set; }
        public int LN_SEQ { get; set; }
        public DateTime Begin_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string OwnerCode { get; set; }
    }
}
