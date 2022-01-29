namespace DatabasePermissions
{
	class Entity
	{
		public string Name { get; set; }
		public bool Insert { get; set; }
		public bool Update { get; set; }
		public bool Delete { get; set; }
		public bool Select { get; set; }
		public bool Execute { get; set; }
	}
}
