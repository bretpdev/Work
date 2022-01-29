using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    class CompassOneLinkCommentHelper
    {
        CompassTaskHelper CTH;
        OneLinkTaskHelper OTH;
        public CompassOneLinkCommentHelper(CompassTaskHelper cth, OneLinkTaskHelper oth)
        {
            this.CTH = cth;
            this.OTH = oth;
        }
        public bool AddComment(QueueInfo data, PendingDemos task, SystemCode code, string arc, string comment)
        {
            bool result = true;
            result &= CTH.AddComment(data, task, code, arc, comment);
            if (OTH != null && !task.StatusInfo.ArcsCreated.Contains("MUBFD"))
                result &= this.OTH.AddComment(data, task, code, arc, comment);
            return result;
        }
    }
}
