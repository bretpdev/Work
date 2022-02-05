using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WebApi;

namespace SCHRPT
{
    public abstract class SchrptWebViewPage<T> : UheaaWebViewPage<T, SchrptBag, SchrptSession> { }
    public abstract class SchrptWebViewPage : UheaaWebViewPage<SchrptBag, SchrptSession> { }
    public abstract partial class SchrptController : UheaaController<SchrptBag, SchrptSession>
    {
        public SchrptController()
        {
            SetBreadcrumbs(ViewBag.Breadcrumbs);
        }
        protected abstract void SetBreadcrumbs(Breadcrumbs b);
    }

    public class SchrptBag : UheaaBag
    {
    }

    public class SchrptSession : UheaaSession
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

        public List<string> PendingMessages
        {
            get
            {
                List<string> result = Get<List<string>>("PendingMessages") ?? new List<string>();
                Set("PendingMessages", result);
                return result;
            }
        }

        public List<string> PendingErrorMessages
        {
            get
            {
                List<string> result = Get<List<string>>("PendingErrorMessages") ?? new List<string>();
                Set("PendingErrorMessages", result);
                return result;
            }
        }
    }
}