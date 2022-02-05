using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
    public partial class FlowStepControlReadOnly : System.Web.UI.UserControl
    {
        public string FlowID { get; set; }
        public int FlowStepSequenceNumber { get; set; }
        public string AccessAlsoBasedOffBusinessUnit { get; set; }
        public string AccessKey { get; set; }
        public string NotificationKey { get; set; }
        public string StaffAssignment { get; set; }
        public string StaffAssignmentCalculationID { get; set; }
        public string ControlDisplayText { get; set; }
        public string Description { get; set; }
        public string DataValidationID { get; set; }
        public string Status { get; set; }

    }
}