
namespace NTISFCFED
{
    /// <summary>
    /// Information for a document that needs to be sent to an account
    /// in an alternate format.
    /// </summary>
    public class DbData
    {
        public int DocumentDetailsId { get; set; }
        public string AccountNumber { get; set; }
        public string AltFormatType { get; set; }
        public string AltFormatDescription { get; set; }
        public string FilePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ForeignState { get; set; }
        public string Country { get; set; }
        public string ZipFiveDigits { get; set; }
        public string ZipFourDigits { get; set; }
        public string InternationalZip { get; set; }

        /// <summary>
        /// Returns a formated string that is used in the master file
        /// </summary>
        /// <returns></returns>
        public string GetBorrowerData()
        {
            string city = City == null ? "Salt Lake City" : City.Replace(",", "");
            return $"{FirstName.Replace(",", "")},{LastName.Replace(",", "")},{Address1.Replace(",", "")},{Address2.Replace(",", "")},{Address3.Replace(",", "")},{Address4.Replace(",", "")},{Address5.Replace(",", "")},{city},{State.Replace(",", "")},{ForeignState.Replace(",", "")},{Country.Replace(",", "")},{ZipFiveDigits},{ZipFourDigits},{InternationalZip},{AccountNumber}";
        }
    }
}
