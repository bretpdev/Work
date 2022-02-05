using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace NSLDSCONSO
{
    public class CsvBorrower
    {
        [CsvHeaderName("BORROWER SSN")]
        public string Ssn { get; set; }
        [CsvHeaderName("BORROWER NAME")]
        public string Name { get; set; }
        [CsvHeaderName("BORROWER DOB")]
        public DateTime DateOfBirth { get; set; }
    }
}
