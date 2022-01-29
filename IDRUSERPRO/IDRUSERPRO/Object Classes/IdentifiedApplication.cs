using System;

namespace IDRUSERPRO
{
    public class IdentifiedApplication
    {
        public string Account_Identifier { get; set; }
        public int App_ID { get; set; }
        public DateTime? Received_Date { get; set; }
        public string Application_Type { get; set; }
        public string E_App_ID { get; set; }
        public string Status { get; set; }
    }

    public class BorrowerNotFound
    {
        public string Borrower_Not_Found { get; set; }
    }
}