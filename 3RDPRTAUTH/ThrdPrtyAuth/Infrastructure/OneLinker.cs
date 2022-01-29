using Uheaa.Common.DataAccess;

namespace ThrdPrtyAuth.Infrastructure
{
    public class OneLinker
    {
        public OneLinker()
        {
            OnEcorr = "0";
        }

        [DbName("CoBorrowerSSN")]
        public string CoBorSsn { get; set; }

        [DbName("AccountIdentifier")]
        public string AccountIdentifier { get; set; }

        [DbName("Name")]
        public string Name { get; set; }

        [DbName("AccountNumber")]
        public string AccountNumber { get; set; }

        [DbName("Address1")]
        public string Address1 { get; set; }

        [DbName("Address2")]
        public string Address2 { get; set; }

        [DbName("Address3")]
        public string Address3 { get; set; }

        [DbName("City")]
        public string City { get; set; }

        [DbName("State")]
        public string State { get; set; }

        [DbName("Zip")]
        public string Zip { get; set; }

        [DbName("ForeignState")]
        public string ForState { get; set; }

        [DbName("ForeignCountry")]
        public string ForCountry { get; set; }

        [DbName("OnEcorr")]
        public string OnEcorr { get; set; }
    }
}
