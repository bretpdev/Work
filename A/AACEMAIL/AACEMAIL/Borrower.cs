using Uheaa.Common;

namespace AACEMAIL
{
    class Borrower
    {
        [CsvHeaderName("Account")]
        public string Ssn { get; set; }
        [CsvHeaderName("Borrower Name")]
        public string Name { get; set; }
        [CsvHeaderName("Borrower Email Address")]
        public string Email { get; set; }
    }
}