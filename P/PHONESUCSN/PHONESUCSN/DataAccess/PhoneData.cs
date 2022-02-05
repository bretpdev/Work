using System;

namespace PHONESUCSN
{
    public class PhoneData
    {
        public int PhoneSuccessionId { get; set; }
        public string Ssn { get; set; }
        public string HomePhone { get; set; }
        public string HomeExt { get; set; }
        public string HomeSrc { get; set; }
        public string HomeInd { get; set; }
        public string HomeConsent { get; set; }
        public bool HomeIsValid { get; set; }
        public bool HomeIsForeign { get; set; }
        public DateTime? HomeVerifiedDate { get; set; }
        public string AltPhone { get; set; }
        public string AltExt { get; set; }
        public string AltSrc { get; set; }
        public string AltInd { get; set; }
        public string AltConsent { get; set; }
        public bool AltIsValid { get; set; }
        public bool AltIsForeign { get; set; }
        public DateTime? AltVerifiedDate { get; set; }
        public string WorkPhone { get; set; }
        public string WorkExt { get; set; }
        public string WorkSrc { get; set; }
        public string WorkInd { get; set; }
        public string WorkConsent { get; set; }
        public bool WorkIsValid { get; set; }
        public bool WorkIsForeign { get; set; }
        public DateTime? WorkVerifiedDate { get; set; }
        public bool IsEndorser { get; set; }
    }
}