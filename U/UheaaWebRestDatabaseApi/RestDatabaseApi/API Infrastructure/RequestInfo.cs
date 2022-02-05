using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDatabaseApi
{
    public class RequestInfo
    {
        public string ApiToken { get; set; }
        public bool SuccessfulAuthentication { get; set; }
        public ResolvedToken ResolvedToken { get; set; }
        public Uheaa.Common.DataAccess.DataAccessHelper.Region Region { get; set; }
        public List<ControllerAccess> ControllersWithAccess { get; set; }
    }
}