namespace RCDIALER
{
    public class DialerData
    {
        public int OutboundCallsId { get; set; }
        public string RCID { get; set; }
        public string ServicerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string CellPhone { get; set; }
        public string MonthlyRepaymentAmount { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public string DaysDelinquent { get; set; }
        public string DelinquentBucket { get; set; }
    }
}