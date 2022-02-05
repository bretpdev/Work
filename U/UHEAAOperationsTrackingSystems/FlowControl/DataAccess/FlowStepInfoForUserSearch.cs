using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace UHEAAOperationsTrackingSystems
{
    public class FlowStepInfoForUserSearch
    {

        public string FlowID { get; set; }
        public int FlowStepSequenceNumber { get; set; }
        public bool AccessAlsoBasedOffBusinessUnit { get; set; }
        public string AccessKey { get; set; }
        public string NotificationKey { get; set; }
        public int StaffAssignment { get; set; }
        public string ControlDisplayText { get; set; }
        public string StepDescription { get; set; }
        public string FlowDescription { get; set; }
        public string TheSystem { get; set; }
        public string DataValidationID { get; set; }
        public string Status { get; set; }
        public string StaffAssignmentLegalName { get; set; }

    }

	public class DisplayUser
	{

		public string LegalName { get; set; }
		public int ID { get; set; }

	}
}
