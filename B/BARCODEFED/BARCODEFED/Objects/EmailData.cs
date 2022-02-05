using Uheaa.Common.DataAccess;

namespace BARCODEFED
{
    public class EmailData
    {
        //[DbName("Email")]
        public string Recipient { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Priority { get; set; }
        public string State { get; set; }
        
    }
}