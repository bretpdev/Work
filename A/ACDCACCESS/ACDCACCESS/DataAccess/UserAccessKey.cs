using Q;

namespace ACDCAccess
{
    class UserAccessKey
    {
        public string Application { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public int UserID { get; set; }
        public long ID { get; set; }
        public string LegalName { get; set; }
    }
}
