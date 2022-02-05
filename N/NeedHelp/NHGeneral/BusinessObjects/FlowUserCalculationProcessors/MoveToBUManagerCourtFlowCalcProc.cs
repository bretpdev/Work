using SubSystemShared;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    class MoveToBUManagerCourtFlowCalcProc : BaseFlowCalculationProcessor
    {
        //constructor
        public MoveToBUManagerCourtFlowCalcProc(TicketData theTicket)
            : base(theTicket)
        {
        }

        public override void PerformCalculations(List<SqlUser> userList, ProcessLogRun logRun)
        {
            DataAccess dataAccess = new DataAccess(logRun);
            string manager = dataAccess.GetManager(TheTicket.Unit);
            if (manager != "")
                TheTicket.Court = userList.Where(p => (p.FirstName + " " + p.LastName) == manager).FirstOrDefault();
        }
    }
}