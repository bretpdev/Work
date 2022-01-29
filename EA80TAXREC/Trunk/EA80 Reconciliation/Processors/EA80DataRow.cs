using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EA80Reconciliation.Processors
{
	public class EA80DataRow
	{
		public string SSN { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string DocType { get; set; }
		public string DocDate { get; set; }
		public string DocName { get; set; }
		public string FileName { get; set; }
	}
}
