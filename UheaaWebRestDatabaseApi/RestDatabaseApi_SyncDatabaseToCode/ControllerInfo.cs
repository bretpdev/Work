using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.WebApi;

namespace RestDatabaseApi_SyncDatabaseToCode
{
    class ControllerInfo
    {
        public WebApiControllers ControllerId { get; set; }
        public string ControllerName { get; set; }
        public List<string> ActionNames { get; set; } = new List<string>();
    }
}
