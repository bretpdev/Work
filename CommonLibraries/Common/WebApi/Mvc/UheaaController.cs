using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Uheaa.Common.WebApi
{
    public abstract class UheaaController : UheaaController<UheaaBag, UheaaSession>
    { }
    public abstract class UheaaController<BAG, SES> : Controller where BAG : UheaaBag, new() where SES : UheaaSession, new()
    {
        public virtual Action OnWebApiConnectionError
        {
            get
            {
                return new Action(() =>
                {
                    Response.Redirect("~/Error/ApiConnection", true);
                });
            }
        }
        public new BAG ViewBag
        {
            get
            {
                var bag = new BAG();
                bag.SetBag(base.ViewBag);
                return bag;
            }
        }
        protected new SES Session
        {
            get
            {
                var ses = new SES();
                ses.SetSession(base.Session);
                ses.SetOnWebApiConnectionError(OnWebApiConnectionError);
                return ses;
            }
        }
    }
}