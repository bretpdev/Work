using SubSystemShared;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    abstract class BaseFlowCalculationProcessor
    {
        /// <summary>
        /// Ticket data for calculator
        /// </summary>
        private TicketData _theTicket;
        public TicketData TheTicket 
        {
            get
            {
                return _theTicket;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseFlowCalculationProcessor(TicketData theTicket)
        {
            _theTicket = theTicket;
        }

        /// <summary>
        /// Actually perform calculations
        /// </summary>
        public abstract void PerformCalculations(List<SqlUser> userList, ProcessLogRun logRun);
    }
}