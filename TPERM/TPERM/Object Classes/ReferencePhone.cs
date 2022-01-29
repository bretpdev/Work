using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPERM
{
    public class ReferencePhone
    {
        public string Phone { get; set; }
        public string PhoneExtension { get; set; }
        public string ForeignPhone { get; set; }
        public string ForeignPhoneExtension { get; set; }
        public string SourceCode { get; set; }
        public string Mbl { get; set; }
        public string Consent { get; set; }
        public bool IsValid { get; set; }
    }
}
