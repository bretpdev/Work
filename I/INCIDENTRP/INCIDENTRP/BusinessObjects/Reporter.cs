using SubSystemShared;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
    public class Reporter
    {
        public SqlUser User { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }

        public bool IsComplete()
        {
            if (User == null || string.IsNullOrEmpty(User.FirstName))
                return false;
            if (BusinessUnit == null || BusinessUnit.ID == 0)
                return false;
            if (string.IsNullOrEmpty(PhoneNumber))
                return false;
            return true;
        }

        public static Reporter Load(DataAccess dataAccess, long ticketNumber, string ticketType)
        {
            return dataAccess.LoadReporter(ticketNumber, ticketType);
        }

        public void Save(DataAccess dataAccess, long ticketNumber, string ticketType)
        {
            dataAccess.SaveReporter(this, ticketNumber, ticketType);
        }
    }
}