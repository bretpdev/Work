using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace INCIDENTRP
{
	partial class DataAccess
	{
		[UsesSproc(IncidentReportingUheaa, "spGetIncident")]
		public Incident LoadIncident(long ticketNumber)
		{
			FlattenedIncident flat = LDA.ExecuteList<FlattenedIncident>("spGetIncident", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
			Incident incident = new Incident();
			if (flat != null)
			{
				incident.Cause = flat.Cause;
				incident.BorrowerSsnAndDobAreVerified = flat.BorrowerSsnAndDobAreVerified;
				incident.Priority = flat.Priority;
				incident.Narrative = flat.Narrative;
			}
			return incident;
		}

		[UsesSproc(IncidentReportingUheaa, "spSetIncident")]
		public void SaveIncident(Incident incident, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Cause", incident.Cause ?? ""));
			parms.Add(new SqlParameter("BorrowerSsnAndDobAreVerified", incident.BorrowerSsnAndDobAreVerified));
			parms.Add(new SqlParameter("Priority", incident.Priority));
			parms.Add(new SqlParameter("Location", incident.Location));
			parms.Add(new SqlParameter("Narrative", incident.Narrative));
			LDA.Execute("spSetIncident", IncidentReportingUheaa, parms.ToArray());
		}

		#region Projection Classes

		private class FlattenedIncident
		{
			public string Cause { get; set; }
			public bool BorrowerSsnAndDobAreVerified { get; set; }
			public string Priority { get; set; }
			public string Narrative { get; set; }
		}

		#endregion Projection Classes
	}
}