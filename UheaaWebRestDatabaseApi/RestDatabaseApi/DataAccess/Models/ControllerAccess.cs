using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uheaa.Common.WebApi;

namespace RestDatabaseApi
{
    public class ControllerAccess
    {
        public WebApiControllers ControllerId { get; set; }
        public string ControllerName { get; set; }
        public int ControllerActionId { get; set; }
        public string ActionName { get; set; }
    }
}