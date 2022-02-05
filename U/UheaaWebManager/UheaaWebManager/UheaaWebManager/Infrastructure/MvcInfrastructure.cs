using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uheaa.Common.WebApi;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Web.Mvc;

namespace UheaaWebManager
{
    public abstract class UwmWebViewPage<T> : UheaaWebViewPage<T, UheaaBag, UheaaSession> { }
    public abstract class UwmWebViewPage : UheaaWebViewPage<UwmBag, UwmSession> { }
    public abstract partial class UwmController : UheaaController<UwmBag, UwmSession>
    {
        public UwmController()
        {
            ViewBag.Breadcrumbs.Add(Breadcrumbs.Dashboard);
            SetBreadcrumbs(ViewBag.Breadcrumbs);
        }

        public abstract void SetBreadcrumbs(Breadcrumbs b);
    }

    public class UwmBag : UheaaBag
    {
    }

    public class UwmSession : UheaaSession
    {
        public LogDataAccess LDA
        {
            get
            {
                return new LogDataAccess(DataAccessHelper.CurrentMode, PLR.ProcessLogId, false, false);
            }
        }
        public DataAccess DA
        {
            get
            {
                return new DataAccess(LDA);
            }
        }
    }
}