using LSLETTERSU.Models;
using System.Web;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WebApi;

namespace LSLETTERSU
{
    public abstract class LslettersWebViewPage<T> : UheaaWebViewPage<T, LslettersuBag, LslettersuSession> { }
    public abstract class LslettersWebViewPage : UheaaWebViewPage<LslettersuBag, LslettersuSession> { }

    public abstract partial class LslettersuController : UheaaController<LslettersuBag, LslettersuSession>
    { }

    public class LslettersuBag : UheaaBag
    {
    }

    public class LslettersuSession : UheaaSession
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
                return new DataAccess(PLR);
            }
        }
        public ProjectData ProjectData
        {
            get
            {
                return (ProjectData)HttpContext.Current.Session["ProjectData"];
            }
            set
            {
                HttpContext.Current.Session["ProjectData"] = value;
            }
        }
        public string ScriptId
        {
            get
            {
                return "LSLETTERSU";
            }
        }
    }
}