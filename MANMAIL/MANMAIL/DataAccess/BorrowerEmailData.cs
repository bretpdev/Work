using System;

namespace MANMAIL
{
    public class BorrowerEmailData : IEquatable<BorrowerEmailData>
    {
        public string AccountNumber { get; set; }
        public string EmailData { get; set; }
        public bool ArcNeeded { get; set; }
        public int Priority { get; set; }
        public string State { get; set; }

        public bool Equals(BorrowerEmailData other)
        {
            if(other == null)
            {
                return false;
            }
            return (AccountNumber == other.AccountNumber) &&
                (EmailData == other.EmailData) &&
                (ArcNeeded == other.ArcNeeded) &&
                (Priority == other.Priority) &&
                (State == other.State);
        }
    }
}