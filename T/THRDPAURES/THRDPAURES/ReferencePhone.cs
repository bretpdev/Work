using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THRDPAURES
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
        public string LastVerifiedDate { get; set; }
        public bool? DomesticPhoneValid { get; set; }
        public bool? ForeignPhoneValid { get; set; }
    }
}
