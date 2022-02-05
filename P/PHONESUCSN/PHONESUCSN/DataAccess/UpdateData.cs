using System;

namespace PHONESUCSN
{
    public class UpdateData
    {
        public int PhoneSuccessionId { get; set; }
        public string Ssn { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
        public string Src { get; set; }
        public string Ind { get; set; }
        public string Consent { get; set; }
        public string IsValid { get; set; }
        public bool IsForeign { get; set; }
        public DateTime VerifiedDate { get; set; }
        public string FromType { get; set; }
        public string ToType { get; set; }
        public bool IsEndorser { get; set; }
    }
}