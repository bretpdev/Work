namespace SubSystemShared
{
    public class UserAccessKey
    {
        public string Application { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BusinessUnitName { get; set; }
		public int BusinessUnitId { get; set; }
        public int UserID { get; set; }
        public long ID { get; set; }
    }
}
