using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WebApi;

namespace CSHRCPTFED
{
    public abstract class CshRcptWebViewPage<T> : UheaaWebViewPage<T, CshRcptBag, CshRcptSession> { }

    public abstract class CshRcptWebViewPage : UheaaWebViewPage<CshRcptBag, CshRcptSession> { }

    public abstract partial class CshRcptController : UheaaController<CshRcptBag, CshRcptSession>
    {

    }

    public class CshRcptBag : UheaaBag
    {
        public string Error
        {
            get { return bag.Error; }
            set { bag.Error = value; }
        }
        public string Info
        {
            get { return bag.Info; }
            set { bag.Info = value; }
        }
    }

    public class CshRcptSession : UheaaSession
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
                return new DataAccess(PLR.LDA);
            }
        }

        
    }
}