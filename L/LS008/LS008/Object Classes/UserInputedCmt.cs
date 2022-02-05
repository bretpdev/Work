using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS008
{
    public class UserInputedCmt
    {
        public string Arc { get; set;}
        public string ArcComment { get; set; }
        public string Ls008Comment { get; set; }
        public string Recipient { get; set; }
        public List<int> LoanSeqs { get; set; }
    }
}
