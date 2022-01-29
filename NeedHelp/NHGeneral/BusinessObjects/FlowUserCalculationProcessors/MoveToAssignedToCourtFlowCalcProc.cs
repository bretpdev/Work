using SubSystemShared;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    class MoveToAssignedToCourtFlowCalcProc : BaseFlowCalculationProcessor
    {
        //constructor
        public MoveToAssignedToCourtFlowCalcProc(TicketData theTicket)
            : base(theTicket)
        {
        }

        public override void PerformCalculations(List<SqlUser> userList, ProcessLogRun logRun)
        {
            TheTicket.AssignedTo = userList.Where(p => p.ID == TheTicket.Court.ID).FirstOrDefault();
        }
    }
}