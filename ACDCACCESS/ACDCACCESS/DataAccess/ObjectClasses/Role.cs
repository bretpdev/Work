using System;

namespace ACDCAccess
{
	public class Role
	{
		public int RoleID { get; set; }
		public string RoleName { get; set; }
		public int AddedBy { get; set; }
		public DateTime StartDate { get; set; }
		public int RemovedBy { get; set; }
		public DateTime EndDate { get; set; }
	}
}
