using System;
using ACDC;

namespace ACDCFlows
{
    public class FlowStepInfoForUserSearch
    {

        public string FlowID { get; set; }
        public int FlowStepSequenceNumber { get; set; }
        public bool AccessAlsoBasedOffBusinessUnit { get; set; }
        public string AccessKey { get; set; }
        public int StaffAssignment { get; set; }
        public string ControlDisplayText { get; set; }
        public string StepDescription { get; set; }
        public string FlowDescription { get; set; }
        public string TheSystem { get; set; }
        public string DataValidationID { get; set; }
        public string Status { get; set; }
		public string StaffAssignmentLegalName { get; set; }
		public string NotificationType { get; set; }

    }

	public class DisplayUser
	{

		public string LegalName { get; set; }
		public int ID { get; set; }

	}
}
