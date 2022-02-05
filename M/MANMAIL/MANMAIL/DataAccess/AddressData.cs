namespace MANMAIL
{
    public class AddressData
    {
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string AddressIsValid { get; set; }
        public string AddressVerifiedDate { get; set; }

        /// <summary>
        /// Override to format as address city state zip
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Address1, Address2, City, State, Zip);
        }
    }
}