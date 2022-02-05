using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace SchedulerWeb
{
	public class SackerData
	{
		public string RequestType { get; set; }
        public int RequestTypeId { get; set; }
		public int RequestId { get; set; }
		public DateTime? DevStartDate { get; set; }
		public DateTime? DevEndDate { get; set; }
		public double DevEstimate { get; set; }
		public DateTime? TesterStartDate { get; set; }
		public DateTime? TesterEndDate { get; set; }
		public double TesterEstimate { get; set; }
		public string AssignedDeveloper { get; set; }
		public string AssignedTester { get; set; }
		public string CurrentStatus { get; set; }

		public int? RequestPriority { get; set; }
		public bool DevCompleted { get; set; }
		public bool TestCompleted { get; set; }
		public bool StartDateOffDev { get; set; }
		public bool StartDateOffTest { get; set; }
	}
}
