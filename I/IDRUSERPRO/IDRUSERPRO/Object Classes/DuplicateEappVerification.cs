namespace IDRUSERPRO
{
    public class DuplicateEappVerification
    {
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EappId { get; set; }

        public override string ToString()
        {
            return string.Format("Account Number: {0} \nName: {1} {2} \nE-App Id: {3}", AccountNumber, FirstName, LastName, EappId);
        }
    }
}