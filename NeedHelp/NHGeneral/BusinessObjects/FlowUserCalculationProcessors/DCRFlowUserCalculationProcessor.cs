using System;
using System.Collections.Generic;
using System.Linq;
using SubSystemShared;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    class DCRFlowUserCalculationProcessor : SystemsSupportAnalystFlowUserCalculationProcessor
    {
        //constructor
        public DCRFlowUserCalculationProcessor(TicketData theTicket)
            : base(theTicket)
        {
        }

        public override void PerformCalculations(List<SqlUser> userList, ProcessLogRun logRun)
        {
            DataAccess dataAccess = new DataAccess(logRun);
            List<DCRSubjectOption> results = dataAccess.GetDCRSubjectOptions();
            DCRSubjectOption match = (from r in results
                                      where r.DCRSubjectOptionText == TheTicket.Subject
                                      select r).SingleOrDefault();
            if (match != null && match.AssignToProgrammer)
            {
                //assign to programmer
                int nextStaffMemberToBeAssigned = dataAccess.GetNextTwoAssignedToOptions(DataAccess.AssignToOptions.Programmer).FirstOrDefault();
                TheTicket.Court = userList.Where(p => p.ID == nextStaffMemberToBeAssigned).FirstOrDefault();
                if (String.IsNullOrEmpty(TheTicket.AssignedTo.LegalName.Trim()))
                {
                    TheTicket.AssignedTo = userList.Where(p => p.ID == nextStaffMemberToBeAssigned).FirstOrDefault();
                }
            }
            else
            {
                //assign to systems support specialist
                base.PerformCalculations(userList, logRun);
            }
        }
    }
}