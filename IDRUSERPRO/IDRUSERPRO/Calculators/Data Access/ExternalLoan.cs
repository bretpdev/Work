using Newtonsoft.Json;

namespace IDRUSERPRO
{
    public class ExternalLoan
    {
        public decimal OutstandingPrincipalBalance { get; set; }
        public decimal OutstandingInterestBalance { get; set; }
        [JsonIgnore]
        public decimal OutstandingBalance
        {
            get
            {
                return OutstandingPrincipalBalance + OutstandingInterestBalance;
            }
        }
    }
}