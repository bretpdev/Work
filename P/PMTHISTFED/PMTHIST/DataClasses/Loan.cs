using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMTHISTFED
{
    public class Loan : ListViewItem
    {

        public Loan(string seqNum, string firstDisbDt, string prgm, string bal)
            : base(new string[] {seqNum, firstDisbDt, prgm, bal})
        {
            SequenceNum = seqNum;
            FirstDisbDate = firstDisbDt;
            Program = prgm;
            Balance = bal;
        }

        public string SequenceNum { get; set; }
        public string FirstDisbDate { get; set; }
        public string Program { get; set; }
        public string Balance { get; set; }

    }
}
