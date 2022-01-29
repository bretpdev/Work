using SubSystemShared;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    class DiffCrtAndAssignedToSSAnalystFlowUserCalculationProc : BaseFlowCalculationProcessor
    {
        //constructor
        public DiffCrtAndAssignedToSSAnalystFlowUserCalculationProc(TicketData theTicket)
			: base(theTicket)
        {
        }

        public override void PerformCalculations(List<SqlUser> userList, ProcessLogRun logRun)
        {
            DataAccess dataAccess = new DataAccess(logRun);
            List<int> staff = dataAccess.GetNextTwoAssignedToOptions(DataAccess.AssignToOptions.SystemSupportAnalyst);
            TheTicket.Court = userList.Where(p => p.ID == staff.Last()).FirstOrDefault();
            if (TheTicket.AssignedTo.ID == 0)
            {
                TheTicket.AssignedTo = userList.Where(p => p.ID == staff.FirstOrDefault()).FirstOrDefault();
            }
        }
    }
}