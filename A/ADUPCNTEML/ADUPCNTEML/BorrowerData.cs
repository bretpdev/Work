using Uheaa.Common.DataAccess;

namespace ADUPCNTEML
{
    public class BorrowerData
    {
        [DbName("AccountNumber")]
        public string AccountNumber { get; set; }
        [DbName("BF_SSN")]
        public string SSN { get; set;  }
        [DbName("EmailAddress")]
        public string EmailAddress { get; set; }
        [DbName("SourceCode")]
        public string SourceCode { get; set; }
        [DbName("ValidityDate")]
        public string ValidityDate { get; set; }
    }
}