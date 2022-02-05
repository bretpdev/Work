using System;

namespace ACDCFlows
{
    public class Flow
    {
        public string FlowID { get; set; }
        public string System { get; set; }
        public string Description { get; set; }
		public string ControlDisplayText { get; set; }
        public string UserInterfaceDisplayIndicator { get; set; }
    }
}
