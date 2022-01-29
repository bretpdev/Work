using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q;
namespace OPSCHKPHN
{
	public class BorrowerData
	{
		public string Ssn { get; set; }
		public string AccountNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
		public MDBorrowerDemographics Demos { get; set; }
		public MDScriptInfoSpecificToBusinessUnitBase ScriptInfoToGenericBusinessUnit { get; set; }
		public double AmountPastDue { get; set; }
		public bool DemosLoaded { get; set; }
	}
}
