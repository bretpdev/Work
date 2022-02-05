using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UheaaWebManager.Models
{
    public class ControllerAccessModel
    {
        public int ControllerId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int ControllerActionId { get; set; }
        public bool HasAccess { get; set; }
    }
}