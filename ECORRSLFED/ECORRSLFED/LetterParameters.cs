using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECORRSLFED
{
    public class LetterParameters
    {
        public HeaderFooterData AddressLine { get; set; }
        public DataTable LoanDetail { get; set; }
        public Dictionary<string, string> FormFields { get; set; }
    }
}
