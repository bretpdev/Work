using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Uheaa.Common.WebApi;
using Uheaa.Common;

namespace RestDatabaseApi.Controllers
{
    public class NobleController : RestDatabaseApiControllerBase
    {
        public override WebApiControllers ControllerId => WebApiControllers.Noble;
        public override string ControllerFriendlyName => "Noble DB";
        [HttpPost]
        public DialerInfo GetDialerInfo()
        {
            string accountNumber = HttpContext.Current.Request.Form["accountnumber"];
            var da = new NobleDataAccess(Info.Region);
            var info = da.GetDialerInfo(accountNumber);
            if (info != null)
            {
                return info;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [HttpPost]
        public List<ManualDialing> GetManualDialingList()
        {
            string campaign = HttpContext.Current.Request.Form["CallingCampaign"];
            string agent = HttpContext.Current.Request.Form["Agent"];
            var da = new NobleDataAccess(Info.Region);
            string sproc = da.ValidateSproc(campaign);
            if (sproc.IsPopulated())
            {
                var info = da.GetManualDialingList(sproc, agent, campaign);
                if (info != null)
                {
                    return info;
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}