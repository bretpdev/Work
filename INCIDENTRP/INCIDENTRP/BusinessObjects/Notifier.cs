namespace INCIDENTRP
{
    public class Notifier : IsEmptyBase
    {
        //Valid values for Type, according to the LST_NotifierType table:
        public const string NONE = "";
        public const string CUSTOMER = "Customer";
        public const string PARTNER_VENDOR = "Partner/Vendor";
        public const string PHEAA_EMPLOYEE = "PHEAA Employee";
        public const string UHEAA_EMPLOYEE = "UHEAA Employee";
        public const string OTHER = "Other";

        //Valid values for Method, according to the LST_NotificaionMethod table (note that NONE and OTHER are also valid here):
        public const string CALL = "Call";
        public const string EMAIL = "E-mail";
        public const string FAX = "Fax";
        public const string IN_PERSON = "In Person";
        public const string SELF = "Self";

        //Valid values for Relationship, according to the LST_Relationship table (note that NONE and OTHER are also valid here):
        public const string CHILD = "Child";
        public const string EMPLOYER = "Employer";
        public const string GUARDIAN = "Guardian";
        public const string NEIGHBOR = "Neighbor";
        public const string NON_RELATIVE_FRIEND = "Non-Relative/Friend";
        public const string PARENT = "Parent";
        public const string RELATIVE = "Relative";
        public const string ROOMMATE = "Roommate";
        public const string SIBLING = "Sibling";
        public const string SPOUSE = "Spouse";

        public string Type { get; set; }
        public string OtherType { get; set; }
        public string Method { get; set; }
        public string OtherMethod { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Relationship { get; set; }
        public string OtherRelationship { get; set; }

        public bool IsComplete()
        {
            if (string.IsNullOrEmpty(Type) || Type == NONE)
                return false;
            if (string.IsNullOrEmpty(Method) || Method == NONE)
                return false;
            if (string.IsNullOrEmpty(Name))
                return false;
            if (string.IsNullOrEmpty(PhoneNumber))
                return false;
            return true;
        }

        public static Notifier Load(DataAccess dataAccess, long ticketNumber, string ticketType)
        {
            return dataAccess.LoadNotifier(ticketNumber, ticketType);
        }

        public void Save(DataAccess dataAccess, long ticketNumber, string ticketType)
        {
            if (IsEmpty())
                dataAccess.DeleteNotifier(ticketNumber, ticketType);
            else
                dataAccess.SaveNotifier(this, ticketNumber, ticketType);
        }
    }
}
