using SubSystemShared;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    class FlowCalculationProcessorCoordinator
    {
        public static void CalculateNeededCourtAndAssignToValues(TicketData ticketToBeUpdated, FlowStep newStep, List<SqlUser> userList, ProcessLogRun logRun)
        {
            BaseFlowCalculationProcessor calculationProcessor;
            if (string.IsNullOrEmpty(newStep.StaffAssignmentCalculationID))
            {
                //if no calculation is needed then just assign to listed staff in table data
                ticketToBeUpdated.Court = userList.Where(p => p.ID == newStep.StaffAssignment).FirstOrDefault();
				if (ticketToBeUpdated.Court == null)
				{
					ticketToBeUpdated.Court = new SqlUser();
				}
            }
            else
            {
                switch (newStep.StaffAssignmentCalculationID)
                {
                    case "DCRFlowUserCalculationProcessor":
                        calculationProcessor = new DCRFlowUserCalculationProcessor(ticketToBeUpdated);
                        calculationProcessor.PerformCalculations(userList, logRun);
                        break;
                    case "DiffCrtAndAssignedToSSAnalystFlowUserCalculationProc":
                        calculationProcessor = new DiffCrtAndAssignedToSSAnalystFlowUserCalculationProc(ticketToBeUpdated);
                        calculationProcessor.PerformCalculations(userList, logRun);
                        break;
                    case "MoveToAssignedToCourtFlowCalcProc":
                        calculationProcessor = new MoveToAssignedToCourtFlowCalcProc(ticketToBeUpdated);
                        calculationProcessor.PerformCalculations(userList, logRun);
                        break;
                    case "MoveToBUManagerCourtFlowCalcProc":
                        calculationProcessor = new MoveToBUManagerCourtFlowCalcProc(ticketToBeUpdated);
                        calculationProcessor.PerformCalculations(userList, logRun);
                        break;
                    case "SystemSupportAnalystFlowUserCalculationProcessor":
                        calculationProcessor = new SystemsSupportAnalystFlowUserCalculationProcessor(ticketToBeUpdated);
                        calculationProcessor.PerformCalculations(userList, logRun);
                        break;
                    case "SystemSupportFlowUserCalculationProcessor":
                        calculationProcessor = new SystemSupportFlowUserCalculationProcessor(ticketToBeUpdated);
                        calculationProcessor.PerformCalculations(userList, logRun);
                        break;
                    case "SystemSupportSpecialistFlowUserCalculationProcessor":
                        calculationProcessor = new SystemSupportSpecialistFlowUserCalculationProcessor(ticketToBeUpdated);
                        calculationProcessor.PerformCalculations(userList, logRun);
                        break;
                }
            }
        }
    }
}