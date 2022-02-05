using SubSystemShared;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    class SystemSupportSpecialistFlowUserCalculationProcessor : BaseFlowCalculationProcessor
    {
        //constructor
        public SystemSupportSpecialistFlowUserCalculationProcessor(TicketData theTicket)
            : base(theTicket)
        {
        }

        public override void PerformCalculations(List<SqlUser> userList, ProcessLogRun logRun)
        {
            DataAccess dataAccess = new DataAccess(logRun);
            int nextStaffMemberToBeAssigned = dataAccess.GetNextTwoAssignedToOptions(DataAccess.AssignToOptions.SystemSupportSpecialist).FirstOrDefault();
            TheTicket.Court = userList.Where(p => p.ID == nextStaffMemberToBeAssigned).FirstOrDefault();
            if (TheTicket.AssignedTo.ID == 0)
                TheTicket.AssignedTo = userList.Where(p => p.ID == nextStaffMemberToBeAssigned).FirstOrDefault();
        }
    }
}