using TimeTracking.Models;
using System.Web;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WebApi;
using System.Collections.Generic;

namespace TimeTracking
{
    public abstract class TimeTrackingWebViewPage<T> : UheaaWebViewPage<T, TimeTrackingBag, TimeTrackingSession> { }
    public abstract class TimeTrackingWebViewPage : UheaaWebViewPage<TimeTrackingBag, TimeTrackingSession> { }

    public abstract partial class TimeTrackingController : UheaaController<TimeTrackingBag, TimeTrackingSession>
    {
    }

    public class TimeTrackingBag : UheaaBag
    {
        public string EditMode { get; set; }
        public string Message { get; set; }
    }

    public class TimeTrackingSession : UheaaSession
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
                return "TIMETRAKUP";
            }
        }

        public string UserName
        {
            get
            {
                return HttpContext.Current.Session["UserName"].ToString();
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }

        public List<UserTime> UserTimes
        {
            get
            {
                return (List<UserTime>)HttpContext.Current.Session["UserTime"];
            }
            set
            {
                HttpContext.Current.Session["UserTime"] = value;
            }
        }
    }
}