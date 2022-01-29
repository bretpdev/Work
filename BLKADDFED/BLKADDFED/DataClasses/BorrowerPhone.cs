using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace BLKADDFED
{
    class BorrowerPhone
    {
        public Boolean Selected { get; set; }
        public string PhoneType { get; set; }
        public string MBLIndicator { get; set; }
        public string ConsentIndicator { get; set; }
        public DateTime VerifiedDate { get; set; }
        public string ValidityIndicator { get; set; }
        public string DomesticAreaCode { get; set; }
        public string DomesticPrefix { get; set; }
        public string DomesticLineNumber { get; set; }
        public string Extension { get; set; }
        public string ForeignCountryCode { get; set; }
        public string ForeignCityCode { get; set; }
        public string ForeignLocalNumber { get; set; }

        public BorrowerPhone(Phone brwPhone)
        {
            Selected = false;
            PhoneType = brwPhone.PhoneType;
            MBLIndicator = brwPhone.MBLIndicator;
            ConsentIndicator = brwPhone.ConsentIndicator;
            VerifiedDate = brwPhone.VerifiedDate;
            ValidityIndicator = brwPhone.ValidityIndicator;
            DomesticAreaCode = brwPhone.DomesticAreaCode;
            DomesticPrefix = brwPhone.DomesticPrefix;
            DomesticLineNumber = brwPhone.DomesticLineNumber;
            Extension = brwPhone.Extension;
            ForeignCountryCode = brwPhone.ForeignCountryCode;
            ForeignCityCode = brwPhone.ForeignCityCode;
            ForeignLocalNumber = brwPhone.ForeignLocalNumber;
        }
    }
}
