using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace ACURINTC
{
    abstract class ProcessorBase
    {
        public General G { get; private set; }
        public DataAccess DA { get; private set; }
        public IReflectionInterface RI { get; private set; }
        public RejectionHelper RH { get; private set; }
        protected readonly SystemCode systemCode;
        protected readonly QueueInfo data;
        protected readonly PendingDemos task;
        protected readonly bool borrowerExistsOnOneLink;
        protected readonly CompassTaskHelper CTH;
        protected readonly OneLinkTaskHelper OTH;
        protected readonly CompassOneLinkCommentHelper COCH;
        protected readonly string demographicsSource;
        public ProcessorBase(General general, QueueInfo data, PendingDemos task, bool skipTaskClose)
        {
            this.G = general;
            this.RI = G.RI;
            this.DA = G.DA;
            this.data = data;
            this.task = task;
            this.RH = new RejectionHelper(DA, this.task.DemographicsSourceId);
            this.systemCode = G.SCH.GetSystemCode(this.task.SystemSourceId);
            this.borrowerExistsOnOneLink = TaskHelper.BorrowerExistsOnOnelink(G.RI as ReflectionInterface, task.Ssn);
            this.OTH = new OneLinkTaskHelper(G, skipTaskClose);
            this.CTH = new CompassTaskHelper(G, skipTaskClose);
            this.COCH = new CompassOneLinkCommentHelper(CTH, borrowerExistsOnOneLink ? OTH : null);
            this.demographicsSource = G.DSH.GetDemographicsSourceName(task.DemographicsSourceId);
        }

        protected RejectionResults GetRejectResult(RejectReason reason, DemographicType type, string phoneNumber = null)
        {
            var rejectReasonText = G.RRH.GetRejectReasonText(reason);
            string actionCode = RH.GetRejectActionCode(reason, type);
            string comment = "";
            if (type == DemographicType.Address)
                comment = string.Format("{0}. PDEM received: {1}.", rejectReasonText, task.GenerateBracketedAddressString());
            else if (type == DemographicType.Email)
                comment = string.Format("{0}. {1}{2}{3}.", rejectReasonText, "{", task.EmailAddress, "}");
            else if (type == DemographicType.Phone)
                comment = string.Format("{0}. PDEM received: {1}{2}{3}.", rejectReasonText, "{", phoneNumber, "}");
            return new RejectionResults()
            {
                Reason = reason,
                ReasonText = rejectReasonText,
                ActionCode = actionCode,
                Comment = comment
            };
        }



    }
}
