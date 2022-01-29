using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using res = MdSession.Properties.Resources;

namespace MdSession
{
    public static class MdScripts
    {
        public static List<ScriptInfo> Scripts { get; private set; }
        static MdScripts()
        {
            Scripts = new List<ScriptInfo>()
            {
                new ScriptInfo("Incarcerated Borrowers", res.bomb, "INCARBWRS", "INCARBWRS.IncarceratedBorrowers", true, false),
                new ScriptInfo("I1/I2 Clearinghouse Review", res.house, "I1I2CHREV", "I1I2CHREV.ClearinghouseReview", true, false),
                new ScriptInfo("Compass Payment History", res.bargraph, "COMPMTHIST", "COMPMTHIST.BorrowerPaymentHistory", false, false),
                new ScriptInfo("Waive Late Fees", res.ficon, "WAVEFEES", "WAVEFEES.WaiveLateFees", false, false),
                new ScriptInfo("Reference Add", res.iicon, "REFADD", "REFADD.ReferenceAdd", false, false),
                new ScriptInfo("Re-Queue Tasks", res.applecaughtinendlessexplosionrebirthcycle, "REQUETASK", "REQUETASK.RequeueTask", true, true),
                new ScriptInfo("Activity History", res.activityhistory, "ACTHIST", "ACTHIST.ActivityHistory", false, false),
                new ScriptInfo("Create Future-Dated Queue Tasks", res.hourglass_go_icon, "CFUTDTQTSK", "CFUTDTQTSK.CreateFutureDatedQueueTasks", true, false)
            };
        }
    }
}
