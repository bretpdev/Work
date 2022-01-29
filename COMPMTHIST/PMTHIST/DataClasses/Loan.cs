using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COMPMTHIST
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
            if (Balance.EndsWith("CR")) //credit
                Balance = "-" + Balance.Substring(0, Balance.Length - 2);
        }

        public string SequenceNum { get; set; }
        public string FirstDisbDate { get; set; }
        public string Program { get; set; }
        public string Balance { get; set; }

    }
}
